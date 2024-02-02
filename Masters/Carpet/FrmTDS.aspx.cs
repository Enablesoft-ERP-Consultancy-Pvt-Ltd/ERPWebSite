using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Carpet_FrmTDS : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {            
            lblMessage.Visible = false;
            TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            CommanFunction.FillCombo(DDType, "Select ID,Type From TDSType order by type");
            TxtMinimumAmount.Text = "0";
            Fill_Grid();
            TxtTDS.Focus();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (lblMessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[7];
                _arrPara[0] = new SqlParameter("@ID", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@TypeID", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@TDS", SqlDbType.Float);
                _arrPara[3] = new SqlParameter("@MinAmt", SqlDbType.Float);
                _arrPara[4] = new SqlParameter("@FromDate", SqlDbType.SmallDateTime);
                _arrPara[5] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[6] = new SqlParameter("@varCompanyId ", SqlDbType.Int);
                _arrPara[0].Value = 0;
                _arrPara[1].Value = DDType.SelectedValue;
                _arrPara[2].Value = TxtTDS.Text;
                _arrPara[3].Value = TxtMinimumAmount.Text;
                _arrPara[4].Value = TxtFromDate.Text;
                _arrPara[5].Value = Session["varuserid"].ToString();
                _arrPara[6].Value = Session["varMasterCompanyIDForERP"].ToString();
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_TDS_MASTER", _arrPara);
                Tran.Commit();
                ClearAll();
                lblMessage.Visible = true;
                lblMessage.Text = "DATA SAVED ........";
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                Logs.WriteErrorLog("Masters_Carpet_FrmTDS_MASTER|cmdSave_Click|" + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                if (con != null)
                {
                    con.Dispose();
                }
            }
            Fill_Grid();
        }
    }

    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (UtilityModule.VALIDTEXTBOX(TxtTDS) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtMinimumAmount) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        lblMessage.Visible = true;
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
    }
    private void Fill_Grid()
    {
        
        string str = "SELECT TM.ID Sr_No,TYPE,TDS,MINAMT,Replace(Convert(VarChar(11),FROMDATE,106), ' ','-') FROMDATE,Replace(Convert(VarChar(11),ENDDATE,106), ' ','-') ENDDATE FROM TDS_MASTER TM,TDSType TT WHERE TM.TYPEID=TT.ID  And MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + "";
        if (DDType.SelectedIndex!=-1)
        {
            str=str + "  and TM.TYPEID=" + DDType.SelectedValue;
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,str);
        DG.DataSource = ds;
        DG.DataBind();
    }
    private void ClearAll()
    {
        TxtTDS.Text = "";
        TxtMinimumAmount.Text = "0";
    }

    protected void DG_SelectedIndexChanged(object sender, EventArgs e)
    {
        //string id = DG.SelectedDataKey.Value.ToString();
        //Session["id"] = id;
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from Color WHERE ColorId=" + id);
        //try
        //{
        //    if (ds.Tables[0].Rows.Count == 1)
        //    {

        //    }
        //}
        //catch (Exception)
        //{

        //}
        //finally
        //{
        //    if (con.State == ConnectionState.Open)
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //}
        //btnSave.Text = "Update";
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DG, "Select$" + e.Row.RowIndex);
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {

    }
    //protected void DG_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //Add CSS class on header row.
    //    if (e.Row.RowType == DataControlRowType.Header)
    //        e.Row.CssClass = "header";

    //    //Add CSS class on normal row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Normal)
    //        e.Row.CssClass = "normal";

    //    //Add CSS class on alternate row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Alternate)
    //        e.Row.CssClass = "alternate";
    //}
    protected void DDType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
}