using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class UserControls_MasterWeightPenalityRate : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {

            txtid.Text = "0";
            BindGrid();
            BindItemName();

            txtEffectiveDate.Attributes.Add("readonly", "readonly");
            txtEffectiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            //txtcolor.Focus();
        }
        lblMessage.Visible = false;
    }
    private void BindItemName()
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "select ITEM_ID,ITEM_NAME from ITEM_MASTER IM INNER JOIN CategorySeparate CS ON IM.CATEGORY_ID=CS.Categoryid where CS.id=0 and IM.MasterCompanyid=" + Session["varCompanyId"] + @" Order by IM.Item_Name", true, "--Plz Select--");

    }
   
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDItemName.SelectedIndex > 0)
        {            
            BindGrid();
            //txtEffectiveDate.Text = "";
        }
    }   
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDItemName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtFromPercentage) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtToPercentage) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtRate) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtEffectiveDate) == false)
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
    private void SubmitData()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[8];
            if (ViewState["Id"] == null)
            {
                ViewState["Id"] = 0;
            }
            _arrpara[0] = new SqlParameter("@Id", SqlDbType.Int);
            _arrpara[0].Direction = ParameterDirection.InputOutput;
            _arrpara[0].Value = ViewState["Id"];
            _arrpara[1] = new SqlParameter("@ItemId", DDItemName.SelectedValue);
            _arrpara[2] = new SqlParameter("@FromPerc", txtFromPercentage.Text == "" ? "0" : txtFromPercentage.Text);
            _arrpara[3] = new SqlParameter("@ToPerc", txtToPercentage.Text == "" ? "0" : txtToPercentage.Text);  
            _arrpara[4] = new SqlParameter("@Rate", txtRate.Text == "" ? "0" : txtRate.Text);
            _arrpara[5] = new SqlParameter("@EffectiveDatefrom", txtEffectiveDate.Text);           
            _arrpara[6] = new SqlParameter("@UserId", Session["varuserid"]);
            _arrpara[7] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);
            _arrpara[7].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_SaveWeightPenalityMaster]", _arrpara);

            lblMessage.Visible = true;
            lblMessage.Text = _arrpara[7].Value.ToString();
            //llMessageBox.Text = "Data Successfully Saved.......";            

            ViewState["Id"] = 0;
            Tran.Commit();
            ClearAfterSave();
            BindGrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            ViewState["Id"] = 0;
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;           
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (lblMessage.Text == "")
        {
            SubmitData();
        }
    }
    private void ClearAfterSave()
    {

        txtFromPercentage.Text = "";
        txtToPercentage.Text = "";
        txtRate.Text = "";      
        txtEffectiveDate.Text = "";   
        btnSave.Text = "Save";        
    }
    protected void BindGrid()
    {
        string where = "";

        if (DDItemName.SelectedIndex > 0)
        {
            where = where + " and WPM.ItemId=" + DDItemName.SelectedValue;
        }
        where = where + " and WPM.EffectiveDateTo Is Null";

        where = where + @" Order by IM.Item_Name";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@Id", "0");
            param[1] = new SqlParameter("@Where", where);

            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetWeightPenalityMasterRate", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                GVWeightPenalityRate.DataSource = ds.Tables[0];
                GVWeightPenalityRate.DataBind();
            }
            else
            {
                GVWeightPenalityRate.DataSource = null;
                GVWeightPenalityRate.DataBind();
            }

            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblMessage.Text = ex.Message;
            con.Close();
        }
    }
    protected void GVWeightPenalityRate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.GVWeightPenalityRate, "select$" + e.Row.RowIndex);
        }
    }
    protected void GVWeightPenalityRate_SelectedIndexChanged(object sender, EventArgs e)
    {
        int r = Convert.ToInt32(GVWeightPenalityRate.SelectedIndex.ToString());

        lblMessage.Text = "";

        btnSave.Text = "Save";

        string id = GVWeightPenalityRate.SelectedDataKey.Value.ToString();
        hnId.Value = id;

        ViewState["Id"] = id;

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Id", id);

            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetWeightPenalityMasterRate", param);

            if (ds.Tables[0].Rows.Count == 1)
            {
                DDItemName.SelectedValue = ds.Tables[0].Rows[0]["ItemId"].ToString();
                txtFromPercentage.Text = ds.Tables[0].Rows[0]["FromPerc"].ToString();
                txtToPercentage.Text = ds.Tables[0].Rows[0]["ToPerc"].ToString();

                txtRate.Text = ds.Tables[0].Rows[0]["Rate"].ToString();
                txtEffectiveDate.Text = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[0]["EffectiveDateFrom"].ToString()).ToString("dd-MMM-yyyy"));
            }
            //BindGrid();            

            Tran.Commit();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        btnSave.Text = "Update";
    }

    //protected void btnrpt_Click(object sender, EventArgs e)
    //{
    //    Report();
    //}
    //private void Report()
    //{
    //    string qry = @" SELECT ColorName  FROM   Color Where MasterCompanyId=" + Session["varCompanyId"] + "  ORDER BY ColorName";
    //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        Session["rptFileName"] = "~\\Reports\\ColorNew.rpt";
    //        //Session["rptFileName"] = Session["ReportPath"];
    //        Session["GetDataset"] = ds;
    //        Session["dsFileName"] = "~\\ReportSchema\\ColorNew.xsd";
    //        StringBuilder stb = new StringBuilder();
    //        stb.Append("<script>");
    //        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
    //        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
    //    }
    //}
  
    //protected void btnpreview_Click(object sender, EventArgs e)
    //{
    //    string str = "select Colorid,ColorName from Color";
    //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        Session["rptfilename"] = "~\\Reports\\Rptcolornew.rpt";
    //        Session["Getdataset"] = ds;
    //        Session["dsFilename"] = "~\\ReportSchema\\Rptcolornew.xsd";
    //        StringBuilder stb = new StringBuilder();
    //        stb.Append("<script>");
    //        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
    //        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
    //    }

    //}
}