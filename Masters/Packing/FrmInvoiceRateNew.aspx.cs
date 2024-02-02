using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Packing_FrmInvoiceRateNew : System.Web.UI.Page
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
            Fill_Grid();

            if (Session["varMasterCompanyIDForERP"].ToString() != "30")
            {
                RDAreaWise.Checked = true;
                if (MySession.InvoiceReportType == "1")
                {
                    RDPcsWise.Checked = true;
                }
            }
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
            //
            string strsql = "";
            strsql = "select CalTypeAmt from packinginformation where packingid=" + TxtInvoiceId.Text + "";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                VarCalType = Convert.ToInt16(ds.Tables[0].Rows[0]["caltypeamt"]);
                if (VarCalType == 0)
                {
                    RDAreaWise.Checked = true;
                }
                else
                {
                    RDPcsWise.Checked = true;
                }

            }

            strsql=@"Select PI.Id,PI.PackingId, VF.QualityId,VF.DesignId,VF.ColorId,PI.FinishedId,PI.Quality as QualityName,PI.Design as DesignName,
                            PI.Color as ColorName,case When P.UnitId=1 Then VF.SizeMtr Else VF.SizeFt End as Size,Pcs,Area,Price,Case When " + VarCalType + @"=0 Then Area*Price Else 
                            Pcs*Price End Amt," + VarCalType + @" as CalType,P.UnitId 
                            From Packing P JOIN  PackingInformation PI ON P.PackingId=PI.PackingId
                            JOIN V_FinishedItemDetail VF ON PI.Finishedid=VF.Item_Finished_id
                            Where P.PackingId=" + TxtInvoiceId.Text + "  And P.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "";  
                     

//            strsql = @"Select Replace(Str(VF.QualityId)+'|'+Str(VF.DesignId)+'|'+Str(VF.ColorId),' ','') Sr_No,PI.Quality as QualityName,PI.Design as DesignName,PI.Color as ColorName,Sum(Pcs) Pcs,
//                             Sum(Area) Area,Price,Case When " + VarCalType + @"=0 Then Sum(Area)*Price Else 
//                             Sum(Pcs)*Price End Amt From Packing P,PackingInformation PI,V_FinishedItemDetail VF Where P.PackingId=PI.PackingId And 
//                             PI.Finishedid=VF.Item_Finished_id And P.PackingId=" + TxtInvoiceId.Text + "  And P.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
//                             Group By VF.QualityId,VF.DesignId,VF.ColorId,PI.Quality,PI.Design,PI.Color,Price";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmInvoiceRateNew.aspx");
            Logs.WriteErrorLog("Masters_Packing_FrmInvoiceRateNew|Fill_Grid_Data|" + ex.Message);
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

        string Strdetail = "";
        for (int i = 0; i < GDItemDetail.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)GDItemDetail.Rows[i].FindControl("Chkboxitem"));
            Label lblId = ((Label)GDItemDetail.Rows[i].FindControl("lblId"));
            Label lblCalType = ((Label)GDItemDetail.Rows[i].FindControl("lblCalType"));
            Label lblUnitId = ((Label)GDItemDetail.Rows[i].FindControl("lblUnitId"));
            Label lblPackingId = ((Label)GDItemDetail.Rows[i].FindControl("lblPackingId"));
            TextBox TxtPrice = ((TextBox)GDItemDetail.Rows[i].FindControl("TxtPrice"));
            TextBox TxtArea = ((TextBox)GDItemDetail.Rows[i].FindControl("TxtArea"));
            TextBox TxtPcs = ((TextBox)GDItemDetail.Rows[i].FindControl("TxtPcs"));
            Label lblFinishedId = ((Label)GDItemDetail.Rows[i].FindControl("lblFinishedId"));

            if (Chkboxitem.Checked == true && (TxtPrice.Text.Trim() != "0" && TxtArea.Text.Trim() != "0" && TxtPcs.Text != "0"))
            {
                Strdetail = Strdetail + lblId.Text + '|' + lblCalType.Text + '|' + lblUnitId.Text + '|' + lblPackingId.Text + '|' + TxtPrice.Text + '|' + TxtArea.Text + '|' + TxtPcs.Text + '|' + lblFinishedId.Text + '~';
            }
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@UserID", Session["varuserid"]);
            param[1] = new SqlParameter("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
            param[2] = new SqlParameter("@StringDetail", Strdetail);
            param[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 200);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@CalTypeAmt", RDAreaWise.Checked == true ? 0 : 1);

            ///**********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdatePackingRateNew", param);
            //*******************               

            //lblmessage.Text = param[6].Value.ToString();
            lblmessage.Text = param[3].Value.ToString();
           
            Tran.Commit();
            Fill_Grid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmessage.Text = ex.Message;
           
            //lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

//        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
//        if (con.State == ConnectionState.Closed)
//        {
//            con.Open();
//        }
//        SqlTransaction Tran = con.BeginTransaction();
//        try
//        {

//            for (int i = 0; i < GDItemDetail.Rows.Count; i++)
//            {
//                string strOrderid = GDItemDetail.DataKeys[i].Value.ToString();
//                TextBox TxtRate1 = (TextBox)GDItemDetail.Rows[i].FindControl("TxtPrice");
//                TextBox TxtAmount1 = (TextBox)GDItemDetail.Rows[i].FindControl("TxtAmt");

//                //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

//                SqlParameter[] arrPara = new SqlParameter[7];
//                arrPara[0] = new SqlParameter("@InvoiceId", SqlDbType.Int);
//                arrPara[1] = new SqlParameter("@QualityId", SqlDbType.Int);
//                arrPara[2] = new SqlParameter("@DesignId", SqlDbType.Int);
//                arrPara[3] = new SqlParameter("@ColorId", SqlDbType.Int);
//                arrPara[4] = new SqlParameter("@Price", SqlDbType.Float);
//                arrPara[5] = new SqlParameter("@Amt", SqlDbType.Float);
//                arrPara[6] = new SqlParameter("@CalTypeAmt", SqlDbType.Float);

//                arrPara[0].Value = TxtInvoiceId.Text;
//                arrPara[1].Value = Convert.ToInt32(strOrderid.Split('|')[0]);
//                arrPara[2].Value = Convert.ToInt32(strOrderid.Split('|')[1]);
//                arrPara[3].Value = Convert.ToInt32(strOrderid.Split('|')[2]);
//                arrPara[4].Value = TxtRate1.Text;
//                arrPara[5].Value = TxtAmount1.Text;
//                arrPara[6].Value = RDAreaWise.Checked == true ? 0 : 1;
//                string Str = @"Update PackingInformation Set Price=" + arrPara[4].Value + @",CalTypeAmt=" + arrPara[6].Value + @" From PackingInformation PI,Packing P,V_FinishedItemDetail VF Where P.PackingId=PI.PackingId And 
//                          PI.Finishedid=VF.Item_Finished_id And P.PackingId=" + arrPara[0].Value + " And VF.QualityId=" + arrPara[1].Value + " And VF.DesignId=" + arrPara[2].Value + " And VF.ColorId=" + arrPara[3].Value + "";

//                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, Str);
//                //DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
//                int dt = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus"));
//                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'PackingInformation'," + arrPara[0].Value + ",getdate(),'Update')");
//            }
//            Tran.Commit();
//            lblmessage.Text = "Data Saved Succeessfully......";
//        }
//        catch (Exception ex)
//        {
//            Tran.Rollback();
//            lblmessage.Text = ex.Message;
//        }
//        finally
//        {
//            con.Close();
//            con.Dispose();
//        }

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
    protected void RDPcsWise_CheckedChanged(object sender, EventArgs e)
    {
        if (RDPcsWise.Checked == true)
        {
            Fill_Grid();
        }
    }
    protected void RDAreaWise_CheckedChanged(object sender, EventArgs e)
    {
        if (RDAreaWise.Checked == true)
        {
            Fill_Grid();
        }
    }
}