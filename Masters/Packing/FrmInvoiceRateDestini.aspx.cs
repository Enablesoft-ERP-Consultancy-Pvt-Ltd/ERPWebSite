using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Packing_FrmInvoiceRateDestini : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            TxtInvoiceId.Text = Request.QueryString["ID"];
            if (Session["varMasterCompanyIDForERP"].ToString() =="1")
            {
                RDAreaWise.Checked = true;
                RDPcsWise.Checked = false;
            }
            else
            {
                RDAreaWise.Checked = false;
                RDPcsWise.Checked = true;
            }
            Fill_Grid();
        }
    }
    private void Fill_Grid()
    {
        GDItemDetail.DataSource = Fill_Grid_Data();
        GDItemDetail.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            int VarCalType = 0;
            if (RDPcsWise.Checked == true)
            {
                VarCalType = 1;
            }
            string strsql = @"Select Replace(Str(VF.QualityId)+'|'+Str(VF.DesignId)+'|'+Str(VF.ColorId),' ','') Sr_No,QualityName,DesignName,ColorName,Sum(Pcs) Pcs,
                             Sum(Area) Area,Price,Case When " + VarCalType + @"=0 Then Sum(Area)*Price Else 
                             Sum(Pcs)*Price End Amt From Packing P,PackingInformation PI,V_FinishedItemDetail VF Where P.PackingId=PI.PackingId And 
                             PI.Finishedid=VF.Item_Finished_id And P.InvoiceNo=" + TxtInvoiceId.Text + "  And P.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                             Group By VF.QualityId,VF.DesignId,VF.ColorId,QualityName,DesignName,ColorName,Price";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmInvoiceRateDestini.aspx");
            Logs.WriteErrorLog("Masters_Packing_FrmInvoiceRate|Fill_Grid_Data|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        return ds;
    }
    protected void GDItemDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        GridViewRow row = GDItemDetail.Rows[index];
        if (Convert.ToInt32(ViewState["Function"]) == 1)
        {
            if (RDAreaWise.Checked == true)
            {
                ((TextBox)GDItemDetail.Rows[index].FindControl("TxtAmt")).Text = (Convert.ToDouble(((TextBox)GDItemDetail.Rows[index].FindControl("TxtPrice")).Text) * Convert.ToDouble(((TextBox)GDItemDetail.Rows[index].FindControl("TxtArea")).Text)).ToString();
            }
            else
            {
                ((TextBox)GDItemDetail.Rows[index].FindControl("TxtAmt")).Text = (Convert.ToDouble(((TextBox)GDItemDetail.Rows[index].FindControl("TxtPrice")).Text) * Convert.ToDouble(((TextBox)GDItemDetail.Rows[index].FindControl("TxtPcs")).Text)).ToString();
            }

            ViewState["Function"] = 0;
        }
    }
    protected void BTNShowAmt_Click(object sender, EventArgs e)
    {
        ViewState["Function"] = 1;
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < GDItemDetail.Rows.Count; i++)
        {
            string strOrderid = GDItemDetail.DataKeys[i].Value.ToString();
            TextBox TxtRate1 = (TextBox)GDItemDetail.Rows[i].FindControl("TxtPrice");
            TextBox TxtAmount1 = (TextBox)GDItemDetail.Rows[i].FindControl("TxtAmt");

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlParameter[] arrPara = new SqlParameter[7];
            arrPara[0] = new SqlParameter("@InvoiceId", SqlDbType.Int);
            arrPara[1] = new SqlParameter("@QualityId", SqlDbType.Int);
            arrPara[2] = new SqlParameter("@DesignId", SqlDbType.Int);
            arrPara[3] = new SqlParameter("@ColorId", SqlDbType.Int);
            arrPara[4] = new SqlParameter("@Price", SqlDbType.Float);
            arrPara[5] = new SqlParameter("@Amt", SqlDbType.Float);
            arrPara[6] = new SqlParameter("@CalTypeAmt", SqlDbType.Float);

            arrPara[0].Value = TxtInvoiceId.Text;
            arrPara[1].Value = Convert.ToInt32(strOrderid.Split('|')[0]);
            arrPara[2].Value = Convert.ToInt32(strOrderid.Split('|')[1]);
            arrPara[3].Value = Convert.ToInt32(strOrderid.Split('|')[2]);
            arrPara[4].Value = TxtRate1.Text;
            arrPara[5].Value = TxtAmount1.Text;
            arrPara[6].Value = RDAreaWise.Checked == true ? 0 : 1;
            string Str = @"Update PackingInformation Set Price=" + arrPara[4].Value + @",CalTypeAmt=" + arrPara[6].Value + @" From PackingInformation PI,Packing P,V_FinishedItemDetail VF Where P.PackingId=PI.PackingId And 
                          PI.Finishedid=VF.Item_Finished_id And P.InvoiceNo=" + arrPara[0].Value + " And VF.QualityId=" + arrPara[1].Value + " And VF.DesignId=" + arrPara[2].Value + " And VF.ColorId=" + arrPara[3].Value + "";

            SqlHelper.ExecuteNonQuery(con, CommandType.Text, Str);
            DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'PackingInformation'," + arrPara[0].Value + ",getdate(),'Update')");
            con.Close();
            con.Dispose();
            lblMessage.Visible = true;
            lblMessage.Text = "Data Saved Successfully";
        }
    }
    protected void RDPcsWise_CheckedChanged(object sender, EventArgs e)
    {
        RDAreaWise.Checked = false;
        RadioButtonSelectedChange();
    }
    protected void RDAreaWise_CheckedChanged(object sender, EventArgs e)
    {
        RDPcsWise.Checked = false;
        RadioButtonSelectedChange();
    }
    private void RadioButtonSelectedChange()
    {
        if (RDAreaWise.Checked == true)
        {
            RDPcsWise.Checked = false;
        }
        else if (RDPcsWise.Checked == true)
        {
            RDAreaWise.Checked = false;
        }
    }
    protected void GDItemDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
}