using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.Services;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Text;

public partial class FrmConsumptionMaster : CustomPage
{
    public string mode = "";

    protected void BindQualityType()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            Hashtable param = new Hashtable();
            param.Add("@mode", "Get");
            DataTable dt = DataAccess.fetch("Pro_GetQualityType", param);

            ddlQualityType.DataValueField = "ITEM_ID";
            ddlQualityType.DataTextField = "ITEM_NAME";
            ddlQualityType.DataSource = dt;
            ddlQualityType.DataBind();
            ddlQualityType.Items.Insert(0, "--Select--");
            ddlQualityType.Items[0].Value = "0";

            tran.Commit();

            ViewState["CarpetType"] = ddlQualityType.SelectedValue;
        }
        catch (Exception ex)
        {
            tran.Rollback();

            //lblmsg.Text = ex.Message;
            con.Close();
            throw ex;
        }
    }
    protected void BindQualityName()
    {
        //if (Convert.ToInt32(ddlQualityType.SelectedValue) > 0)
        //{
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                Hashtable param = new Hashtable();
                param.Add("@mode", "Get");
                param.Add("@Item_Id", ddlQualityType.SelectedValue);
                DataTable dt = DataAccess.fetch("Pro_GetQualityName", param);

                ddlQuality.DataValueField = "QualityID";
                ddlQuality.DataTextField = "QualityName";
                ddlQuality.DataSource = dt;
                ddlQuality.DataBind();
                ddlQuality.Items.Insert(0, "--Select--");
                ddlQuality.Items[0].Value = "0";

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }
        //}
        //else
        //{
        //}
    }
    protected void BindDesignName()
    {
        if (Convert.ToInt32(ddlQuality.SelectedValue) > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (ddlQuality.SelectedIndex != 0)
                {
                    Hashtable param = new Hashtable();
                    param.Add("@mode", "Get");
                    param.Add("@Item_Id", ddlQualityType.SelectedValue);
                    param.Add("@QualityId", ddlQuality.SelectedValue);
                    DataTable dt = DataAccess.fetch("Pro_GetDesignName", param);

                    ddlDesignName.DataValueField = "designId";
                    ddlDesignName.DataTextField = "designName";
                    ddlDesignName.DataSource = dt;
                    ddlDesignName.DataBind();
                    ddlDesignName.Items.Insert(0, "--ALL Design--");
                    ddlDesignName.Items[0].Value = "0";
                }
                else
                {
                    ddlDesignName.Items.Clear();
                    ddlDesignName.DataSource = null;
                    ddlDesignName.DataBind();
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Please select quality type');", true);
        }
    }
    protected void BindColorName()
    {
        if (Convert.ToInt32(ddlDesignName.SelectedValue) > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                Hashtable param = new Hashtable();
                param.Add("@mode", "Get2");
                param.Add("@designId", ddlDesignName.SelectedValue);
                DataTable dt = DataAccess.fetch("Pro_GetColorName", param);

                ddlColorName.DataValueField = "colorid";
                ddlColorName.DataTextField = "ColorName";
                ddlColorName.DataSource = dt;
                ddlColorName.DataBind();
                ddlColorName.Items.Insert(0, "--ALL Colors--");
                ddlColorName.Items[0].Value = "0";

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }
        }
        else
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                Hashtable param = new Hashtable();
                param.Add("@mode", "Get2");
                param.Add("@designId", 0);
                DataTable dt = DataAccess.fetch("Pro_GetColorName", param);

                ddlColorName.DataValueField = "colorid";
                ddlColorName.DataTextField = "ColorName";
                ddlColorName.DataSource = dt;
                ddlColorName.DataBind();
                ddlColorName.Items.Insert(0, "--ALL Colors--");
                ddlColorName.Items[0].Value = "0";

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }
        }
    }
    protected void BindSizeShape()
    {
        if (Convert.ToInt32(ddlQuality.SelectedIndex) > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (ddlQuality.SelectedIndex != 0)
                {
                    Hashtable param = new Hashtable();
                    param.Add("@QualityId", ddlQuality.SelectedValue);
                    param.Add("@AddSizeDate", tbSizeDate.Text == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : tbSizeDate.Text);
                    DataTable dt = DataAccess.fetch("pro_GetConsumptionSizeShape", param);

                    ddlSizeShape.DataValueField = "SizeId";
                    ddlSizeShape.DataTextField = "SizeShape";
                    ddlSizeShape.DataSource = dt;
                    ddlSizeShape.DataBind();
                    ddlSizeShape.Items.Insert(0, "--ALL Size--");
                    ddlSizeShape.Items[0].Value = "0";
                }
                else
                {
                    ddlSizeShape.Items.Clear();
                    ddlSizeShape.DataSource = null;
                    ddlSizeShape.DataBind();
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Please select quality name');", true);
        }
    }
    protected void BindGDConsumptionMaster()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[3];

            if (mode == "Insert")
            {
                hnID.Value = "";
                //ddlQuality.SelectedValue = "0";
                ddlDesignName.SelectedValue = "0";
                btnSave.Text = "Save";
            }
            param[0] = new SqlParameter("@ID", hnID.Value);
            param[1] = new SqlParameter("@QualityId", ddlQuality.SelectedValue);
            param[2] = new SqlParameter("@DesignId", ddlDesignName.SelectedValue);

            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "pro_GetAllConsumptionMasterSize", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                GDConsumptionMaster.DataSource = ds;
                GDConsumptionMaster.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                GDConsumptionMaster.DataSource = dt;
                GDConsumptionMaster.DataBind();  

            }

            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            //lblmsg.Text = ex.Message;
            con.Close();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            if (Session["varcompanyId"].ToString() == "20" && variable.VarNewQualitySize == "1")
            {
                hncomp.Value = "20";
                SizeDate.Visible = true;
                tbSizeDate.Attributes.Add("readonly", "readonly");
            }
            else
            {
                SizeDate.Visible = false;                
            }
            BindQualityType();
            BindQualityName();          
            BindGDConsumptionMaster();            
        }
    }
    protected void ddlQualityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindQualityName();        
    }
    protected void ddlQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlQuality.SelectedValue) > 0)
        {
            BindDesignName();
            BindSizeShape();
            BindColorName();
            BindGDConsumptionMaster();
        }
    }
    protected void ddlDesignName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlDesignName.SelectedValue) >= 0)
        {
            BindColorName();
            BindGDConsumptionMaster();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Savedetail();
        BindGDConsumptionMaster();
        mode = "";
        return;
    }
    protected void Savedetail()
    {
        lblMessage.Text = "";
        if (ddlQuality.SelectedIndex > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[14];
                param[0] = new SqlParameter("@ID", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = hnID.Value == "" ? "0" : hnID.Value;
                param[1] = new SqlParameter("@QualityTypeId", ddlQualityType.SelectedIndex > 0 ? ddlQualityType.SelectedValue : "0");
                param[2] = new SqlParameter("@QualityId", ddlQuality.SelectedIndex > 0 ? ddlQuality.SelectedValue : "0");
                param[3] = new SqlParameter("@DesignId", ddlDesignName.SelectedIndex > 0 ? ddlDesignName.SelectedValue : "0");
                param[4] = new SqlParameter("@ColorId", ddlColorName.SelectedIndex > 0 ? ddlColorName.SelectedValue : "0");
                param[5] = new SqlParameter("@SizeId", ddlSizeShape.SelectedIndex > 0 ? ddlSizeShape.SelectedValue : "0");
                param[6] = new SqlParameter("@TypeId", ddlType.SelectedIndex > 0 ? ddlType.SelectedValue : "0");
                param[7] = new SqlParameter("@TypeName", ddlType.SelectedItem.Text);
                param[8] = new SqlParameter("@WoolConsump", tbWoolConsump.Text == "" ? "0" : tbWoolConsump.Text);
                param[9] = new SqlParameter("@WeavingRate", tbWeavingRate.Text == "" ? "0" : tbWeavingRate.Text);
                param[10] = new SqlParameter("@Commission", tbCommission.Text == "" ? "0" : tbCommission.Text);
                param[11] = new SqlParameter("@EffectiveDate", tbEffectiveDate.Text == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : tbEffectiveDate.Text);              
                param[12] = new SqlParameter("@ToDate", null);                
                param[13] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[13].Direction = ParameterDirection.Output;

                //**********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveConsumptionMaster", param);
                //DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_SaveConsumptionMaster", param);

                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    if(tbEffectiveDate.Text==ds.Tables[0]
                //}

                

                lblMessage.Text = param[13].Value.ToString();
                if (lblMessage.Text == "")
                {
                    lblMessage.Text = "Data saved successfully";
                   
                    mode = "Insert";
                    Refreshcontrol();
                }
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
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('Please select quality...')", true);
        }
    }
    protected void Refreshcontrol()
    {
       // ddlQualityType.SelectedValue = "0";
        ddlType.SelectedValue = "0";
        //ddlQuality.SelectedValue = "0";
        ddlDesignName.SelectedValue = "0";
        ddlColorName.SelectedValue = "0";
        ddlSizeShape.SelectedValue = "0";
        ddlType.SelectedValue = "0";
        tbWoolConsump.Text = "";
        tbWeavingRate.Text = "";
        tbCommission.Text = "";
        tbEffectiveDate.Text = "";
        hnID.Value = "";
    }   
    protected void GDConsumptionMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GDConsumptionMaster.PageIndex = e.NewPageIndex;
        BindGDConsumptionMaster();
    }

    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        Session["ReportPath"] = "Reports/ConsumptionMasterReport.rpt";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {

            SqlParameter[] _arrPara = new SqlParameter[3];
            _arrPara[0] = new SqlParameter("@ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@QualityId", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@DesignId", SqlDbType.Int);          

            _arrPara[0].Value = hnID.Value=="" ? "0" : hnID.Value;
            _arrPara[1].Value = ddlQuality.SelectedValue == "" ? "0" : ddlQuality.SelectedValue;
            _arrPara[2].Value = ddlDesignName.SelectedValue == "" ? "0" : ddlDesignName.SelectedValue;

            DataSet ds = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, "pro_GetAllConsumptionMasterSizeReportData", _arrPara);

            Session["dsFileName"] = "~\\ReportSchema\\ConsumptionMasterReport.xsd";
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = Session["ReportPath"];
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            tran.Rollback();
            lblMessage.Text = ex.Message;           
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void tbSizeDate_TextChanged(object sender, EventArgs e)
    {
        BindSizeShape();
    }   
}