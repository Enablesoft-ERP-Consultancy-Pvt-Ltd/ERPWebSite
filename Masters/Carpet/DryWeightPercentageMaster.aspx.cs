using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Text;

public partial class Masters_Campany_DryWeightPercentageMaster : System.Web.UI.Page
{
    static int QualityId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtid.Text = "0";
            Fill_Grid();
        }
        Lblerr.Visible = false;
    }
    private void Fill_Grid()
    {
        gdQuality.DataSource = Fill_Grid_Data();
        gdQuality.DataBind();

    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "";
            strsql = @"select distinct Q.QualityId as Sr_No,QualityName,Q.QualityId,isnull(DWM.DryWeightPercentage,0) as DryWeightPercentage,isnull(DWM.DWMID,0) as DWMID
                        From Quality Q(NoLock) JOIN ITEM_MASTER IM(NoLock) ON Q.Item_Id=IM.ITEM_ID 
                        JOIN ITEM_CATEGORY_MASTER ICM(NoLock) ON IM.CATEGORY_ID =ICM.CATEGORY_ID 
                        JOIN CategorySeparate CS(NoLock) ON ICM.CATEGORY_ID=CS.Categoryid and CS.id=0
                        LEFT JOIN DryWeightPercentageMaster DWM(NoLock) ON Q.QualityId=DWM.QualityId  
                        where Q.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            //if (txtsearchQuality.Text != "")
            //{
            //    strsql = strsql + " and QualityName like '" + txtsearchQuality.Text + "%'";
            //}
            strsql = strsql + " order by Q.QualityName";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/DryWeightPercentageMaster.aspx");
            Logs.WriteErrorLog("Masters_Carpet_DryWeightPercentageMaster|Fill_Grid_Data|" + ex.Message);
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
    protected void txtDryWeightPercentage_TextChanged(object sender, EventArgs e)
    {
        txtid.Text = "0";
        Lblerr.Visible = false;
        Lblerr.Text = "";

        TextBox txt = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
        txt.Focus();

        Label lblQualityId = (Label)gvRow.FindControl("lblQualityId");
        Label lblDWMID = (Label)gvRow.FindControl("lblDWMID");
        txtid.Text = lblDWMID.Text;

        TextBox txtDryWeightPercentage = (TextBox)gvRow.FindControl("txtDryWeightPercentage");

        if (Convert.ToInt32(txtDryWeightPercentage.Text) > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[6];
                _arrPara[0] = new SqlParameter("@DWMID", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@QualityId", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@DryWeightPercentage", SqlDbType.Float);
                _arrPara[3] = new SqlParameter("@UserId", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);


                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                _arrPara[1].Value = lblQualityId.Text;
                _arrPara[2].Value = txtDryWeightPercentage.Text;
                _arrPara[3].Value = Session["varuserid"].ToString();
                _arrPara[4].Value = Session["varMasterCompanyIDForERP"].ToString();

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SaveDryWeightPercentage", _arrPara);
                Tran.Commit();
                Lblerr.Visible = true;
                Lblerr.Text = "Save Details....";
                txtDryWeightPercentage.Text = "";               
                txtid.Text = "0";
                QualityId = 0;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                UtilityModule.MessageAlert(ex.Message, "Master/Carpet/DryWeightPercentageMaster.aspx");
                Lblerr.Visible = true;
                Lblerr.Text = ex.Message;
                Logs.WriteErrorLog("Masters_Carpet_DryWeightPercentageMaster|cmdSave_Click|" + ex.Message);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", "onError();", true);
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
//    protected void gdQuality_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        string id = gdQuality.SelectedDataKey.Value.ToString();
//        //Session["id"] = id;
//        ViewState["id"] = id;
//        txtid.Text = "0";
//        string str = "";
//        str = @" select Q.QualityId as Sr_No,QualityName,Q.QualityId,isnull(DWM.DryWeightPercentage,0) as DryWeightPercentage,isnull(DWM.DWMID,0) as DWMID
//                        from Quality Q LEFT JOIN DryWeightPercentageMaster DWM ON Q.QualityId=DWM.QualityId
//                        where Q.QualityId=" + id + " And Q.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "";
//        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

//        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
//        try
//        {
//            if (ds.Tables[0].Rows.Count == 1)
//            {
//                txtid.Text = ds.Tables[0].Rows[0]["DWMID"].ToString();
//                QualityId = Convert.ToInt32(ds.Tables[0].Rows[0]["QualityId"].ToString());
//                txtQualityName.Text = ds.Tables[0].Rows[0]["QualityName"].ToString();
//                txtDryWeightPercentage.Text = ds.Tables[0].Rows[0]["DryWeightPercentage"].ToString();
//            }
//        }
//        catch (Exception ex)
//        {
//            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/DryWeightPercentageMaster.aspx");
//            Logs.WriteErrorLog("Masters_Campany_DryWeightPercentageMaster|Fill_Grid_Data|" + ex.Message);
//        }
//        finally
//        {
//            if (con.State == ConnectionState.Open)
//            {
//                con.Close();
//                con.Dispose();
//            }
//        }
//        btnsave.Text = "Update";

//    }
    protected void gdQuality_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdQuality.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void gdQuality_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gdQuality, "select$" + e.Row.RowIndex);
        }
    }
    //protected void btnsave_Click(object sender, EventArgs e)
    //{
    //    if (txtQualityName.Text != "" && (txtDryWeightPercentage.Text != "" && txtDryWeightPercentage.Text != "0"))
    //    {

    //        if (Lblerr.Visible == false)
    //        {
    //            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //            if (con.State == ConnectionState.Closed)
    //            {
    //                con.Open();
    //            }
    //            SqlTransaction Tran = con.BeginTransaction();
    //            try
    //            {
    //                SqlParameter[] _arrPara = new SqlParameter[6];
    //                _arrPara[0] = new SqlParameter("@DWMID", SqlDbType.Int);
    //                _arrPara[1] = new SqlParameter("@QualityId", SqlDbType.Int);
    //                _arrPara[2] = new SqlParameter("@DryWeightPercentage", SqlDbType.Float);
    //                _arrPara[3] = new SqlParameter("@UserId", SqlDbType.Int);
    //                _arrPara[4] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);


    //                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
    //                _arrPara[1].Value = QualityId;
    //                _arrPara[2].Value = txtDryWeightPercentage.Text;
    //                _arrPara[3].Value = Session["varuserid"].ToString();
    //                _arrPara[4].Value = Session["varMasterCompanyIDForERP"].ToString();

    //                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SaveDryWeightPercentage", _arrPara);
    //                Tran.Commit();
    //                Lblerr.Visible = true;
    //                Lblerr.Text = "Save Details....";
    //                txtDryWeightPercentage.Text = "";
    //                txtQualityName.Text = "";
    //                txtid.Text = "0";
    //                QualityId = 0;
    //            }
    //            catch (Exception ex)
    //            {
    //                Tran.Rollback();
    //                UtilityModule.MessageAlert(ex.Message, "Master/Carpet/DryWeightPercentageMaster.aspx");
    //                Lblerr.Visible = true;
    //                Lblerr.Text = ex.Message;
    //                Logs.WriteErrorLog("Masters_Campany_DryWeightPercentageMaster|cmdSave_Click|" + ex.Message);
    //                Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", "onError();", true);
    //            }
    //            finally
    //            {
    //                if (con.State == ConnectionState.Open)
    //                {
    //                    con.Close();

    //                }
    //                if (con != null)
    //                {
    //                    con.Dispose();
    //                }
    //            }
    //            Fill_Grid();
    //            btnsave.Text = "Save";

    //        }
    //    }
    //    else
    //    {
    //        Lblerr.Visible = true;
    //        Lblerr.Text = "Fill Details....";
    //    }
    //}

    protected void gdQuality_Init(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
    }

}