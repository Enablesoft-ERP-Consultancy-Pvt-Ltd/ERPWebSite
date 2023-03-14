using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO.Compression;
using System.Text;

public partial class Masters_Campany_FrmBunkarMaster : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }        
        //newPreview.ImageUrl = "~/images/Logo/1_compney.gif";
        if (!IsPostBack)
        {
           txtBunkarId.Text = "0";
            BindContractor();
            Fill_Grid();


            txtJoiningDate.Attributes.Add("readonly", "readonly");
            txtJoiningDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");           
        }

        lblerr.Visible = false;
    }
    protected void BindContractor()
    {       
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            using (con)
            {
                using (SqlCommand cmd = new SqlCommand("PRO_DDBINDCONTRACTOR", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            DDContractorName.DataSource = dr;
                            DDContractorName.DataTextField = "EmpName";
                            DDContractorName.DataValueField = "EmpId";
                            DDContractorName.DataBind();
                            DDContractorName.Items.Insert(0, new ListItem("--Select--", "0"));
                        }
                    }
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Error:" + ex.Message.ToString());
        }
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void DDContractorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    private void Fill_Grid()
    {
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@UserID", Session["VarUserId"]);
        param[1] = new SqlParameter("@MasterCompanyId", Session["VarCompanyId"]);
        param[2] = new SqlParameter("@ContractorId", DDContractorName.SelectedValue);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "FILLBUNKARMASTER", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GVBunkarMaster.DataSource = ds.Tables[0];
            GVBunkarMaster.DataBind();
        }
        else
        {
            GVBunkarMaster.DataSource = null;
            GVBunkarMaster.DataBind();
        }        

        //GVBunkarMaster.DataSource = Fill_Grid_Data();
        //GVBunkarMaster.DataBind();
    }
    private void CHECKVALIDCONTROL()
    {
        lblerr.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDContractorName) == false)
        {
            goto a;
        }       
        if (UtilityModule.VALIDTEXTBOX(txtBunkarName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtJoiningDate) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        lblerr.Visible = true;
    UtilityModule.SHOWMSG(lblerr);
    B: ;
    }
    public void Store_Data()
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[11];
            _arrPara[0] = new SqlParameter("@BMID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@ContractorId", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@BunkarName", SqlDbType.VarChar, 150);
            _arrPara[3] = new SqlParameter("@FatherName", SqlDbType.VarChar, 150);
            _arrPara[4] = new SqlParameter("@Address", SqlDbType.VarChar, 500);
            _arrPara[5] = new SqlParameter("@MobileNo", SqlDbType.VarChar, 15);
            _arrPara[6] = new SqlParameter("@JoiningDate", SqlDbType.DateTime);
            _arrPara[7] = new SqlParameter("@Status", SqlDbType.Int);           
            _arrPara[8] = new SqlParameter("@UserId", SqlDbType.Int);
            _arrPara[9] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _arrPara[10] = new SqlParameter("@MSG", SqlDbType.NVarChar, 200);

            _arrPara[0].Value =txtBunkarId.Text;
            _arrPara[1].Value = DDContractorName.SelectedValue;
            _arrPara[2].Value = txtBunkarName.Text.ToUpper();
            _arrPara[3].Value = txtFatherName.Text.ToUpper();
            _arrPara[4].Value = txtAddress.Text.ToUpper();
            _arrPara[5].Value = txtMobileNo.Text;
            _arrPara[6].Value = txtJoiningDate.Text;
            _arrPara[7].Value = ChkBunkarBlackList.Checked == true ? 1 : 0;
            _arrPara[8].Value = Session["varuserid"].ToString();
            _arrPara[9].Value = Session["varCompanyId"].ToString();
            _arrPara[10].Direction = ParameterDirection.Output;


            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVE_BUNKAR_MASTER", _arrPara);
            lblerr.Visible = true;
            lblerr.Text = "";
            lblerr.Text = _arrPara[10].Value.ToString();
            Tran.Commit(); 
            ClearAll();
            Fill_Grid();
            //**********

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/FrmBunkarMaster.aspx");
            Tran.Rollback();
            lblerr.Visible = true;
            lblerr.Text = ex.Message;
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


    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
         CHECKVALIDCONTROL();
         if (lblerr.Text == "")
         {
             Store_Data();
         }
    }
    private void ClearAll()
    {
        DDContractorName.SelectedValue = "0";
        txtBunkarName.Text = "";
        txtFatherName.Text = "";
        txtAddress.Text = "";
        txtMobileNo.Text = "";
        txtJoiningDate.Text = "";
        ChkBunkarBlackList.Checked = false;        
       
    }
    protected void GVBunkarMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = GVBunkarMaster.SelectedDataKey.Value.ToString();     
        ViewState["BMID"] = id;

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@BMID", id);

            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetBunkarMasterDetail", param);

            if (ds.Tables[0].Rows.Count == 1)
            {
                txtBunkarId.Text = ds.Tables[0].Rows[0]["BMID"].ToString();
                DDContractorName.SelectedValue = ds.Tables[0].Rows[0]["ContractorId"].ToString();
                txtBunkarName.Text = ds.Tables[0].Rows[0]["BunkarName"].ToString();
                txtFatherName.Text = ds.Tables[0].Rows[0]["FatherName"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtMobileNo.Text = ds.Tables[0].Rows[0]["MobileNo"].ToString();
                txtJoiningDate.Text = ds.Tables[0].Rows[0]["BunkarJoiningDate"].ToString();
                if (ds.Tables[0].Rows[0]["Status"].ToString() == "1")
                {
                    ChkBunkarBlackList.Checked = true;                   
                }
            }
           // Fill_Grid();            

            Tran.Commit();
        }
        catch (Exception ex)
        {
            lblerr.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        btnSave.Text = "Update";

        
    }
    protected void GVBunkarMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVBunkarMaster, "select$" + e.Row.RowIndex);
        }
    }
    protected void GVBunkarMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVBunkarMaster.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }

    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        report();
    }
    private void report()
    {
//        string qry = @"SELECT CompanyName,CompAddr1,CompAddr2,CompAddr3,CompFax,CompTel,RBICode,IECode,PANNr,CSTNo,TinNo,SignatoryName 
//                       FROM CompanyInfo INNER JOIN  Signatory ON CompanyInfo.Sigantory=Signatory.SignatoryId where CompanyInfo.MasterCompanyId=" + Session["varCompanyId"];
//        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
//        if (ds.Tables[0].Rows.Count > 0)
//        {
//            Session["rptFileName"] = "~\\Reports\\CompanyReportNew.rpt";
//            //Session["rptFileName"] = Session["ReportPath"];
//            Session["GetDataset"] = ds;
//            Session["dsFileName"] = "~\\ReportSchema\\CompanyReportNew.xsd";
//            StringBuilder stb = new StringBuilder();
//            stb.Append("<script>");
//            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
//            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
//        }
//        else
//        {
//            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
//        }
    }
    protected void GVBunkarMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        //if (e.Row.RowType == DataControlRowType.Header)
        //    e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        //if (e.Row.RowType == DataControlRowType.DataRow &&
        // e.Row.RowState == DataControlRowState.Alternate)
        // e.Row.CssClass = "alternate";
    }
    protected void GVBunkarMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        lblerr.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //LinkButton lnkdel = sender as LinkButton;
            //GridViewRow grv = lnkdel.NamingContainer as GridViewRow;
            int BMID = Convert.ToInt32(GVBunkarMaster.DataKeys[e.RowIndex].Value);
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@BMID", BMID);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@MastercompanyId", Session["varcompanyNo"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            //**********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEBUNKARMASTER", param);
            lblerr.Text = param[3].Value.ToString();
            Tran.Commit();
            Fill_Grid();

        }
        catch (Exception ex)
        {
            lblerr.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        
    }
}
