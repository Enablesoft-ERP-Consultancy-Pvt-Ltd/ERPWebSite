using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_RawMaterial_FrmGateInOutMaterial : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = "";
            str = @"Select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By Companyname 
            Select ID, BranchName From BRANCHMASTER BM(nolock) JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];
           
            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "Select Comp Name");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 1, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }
            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            ViewState["GateInMaterialRegisterID"] = 0;
            ViewState["GateOutMaterialRegisterID"]=0;
            BindGateType();
            if (Convert.ToInt32(Session["varCompanyId"]) == 16 || Convert.ToInt32(Session["varCompanyId"]) == 28)
            {
                ddGateType.Enabled = true;
            }
        }
    }
    private void BindGateType()
    {
        if (ddGateType.SelectedValue == "1")
        {
            LblErrorMessage.Text = "";
            TdGPNo.Visible = false;
            TdChallanNo.Visible = true;
            TDOutTime.Visible = false;
            TDInTime.Visible = true;            
            refreshForm();
            TRMaterialIn.Visible = true;
            fill_MaterialIngrid();
            txtInTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            TRMaterialOut.Visible = false;
            BindGatePassNo();
        }
        else if (ddGateType.SelectedValue == "2")
        {
            LblErrorMessage.Text = "";
            TdGPNo.Visible = true;
            TdChallanNo.Visible = false;
            TDOutTime.Visible = true;
            TDInTime.Visible = false;           
            refreshForm();
            TRMaterialOut.Visible = true;
            fill_MaterialOutgrid();
            txtOutTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            TRMaterialIn.Visible = false;
            BindGatePassNo();
        }
    }
    protected void ddGateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGateType();
    }
    protected void DDMaterialReturnType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    private void BindGatePassNo()
    {
        string str = "";
        if (ddGateType.SelectedValue == "1")
        {
            str = @"Select Distinct GateInMaterialRegisterId,GatePassInNo from GateInMaterialRegister Order By GatePassInNo";
        }
        else if (ddGateType.SelectedValue == "2")
        {
            str = @"Select Distinct GateOutMaterialRegisterId,GatePassOutNo from GateOutMaterialRegister Order By GatePassOutNo";
        }
        DataSet ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDGatePassNo, ds, 0, true, "Select GatePassNo");
    }
    protected void DDGatePassNo_SelectedIndexChanged(object sender, EventArgs e)
    {        
            if (ddGateType.SelectedValue == "1")
            {
                fill_MaterialIngrid();
            }
            else if (ddGateType.SelectedValue == "2")
            {
                fill_MaterialOutgrid();
            }        
    }
    protected void ChKForEdit_CheckedChanged(object sender, EventArgs e)
    {
        DDGatePassNo.Items.Clear();
        TDGatePassNo.Visible = false;
        //BindGateType();
        if (ChKForEdit.Checked == true)
        {
            TDGatePassNo.Visible = true;
            BindGatePassNo();
        }
       
        
        //DDGatePassNo.Items.Clear();
        //TxtGateInNo.Text = "";
        //txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        //Td3.Visible = false;
        //if (ChKForEdit.Checked == true)
        //{
        //    Td3.Visible = true;
        //    EmpSelectedChange();
        //}
    }
    private void SaveData()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            if (ddGateType.SelectedValue == "1")
            {
                SqlParameter[] arr = new SqlParameter[22];
                arr[0] = new SqlParameter("@GateInMaterialRegisterID", SqlDbType.Int);
                arr[1] = new SqlParameter("@GateInMaterialDate", SqlDbType.DateTime);
                arr[2] = new SqlParameter("@CompanyID", SqlDbType.Int);
                arr[3] = new SqlParameter("@PartyName", SqlDbType.VarChar, 50);
                arr[4] = new SqlParameter("@Qty", SqlDbType.Float);
                arr[5] = new SqlParameter("@Unit", SqlDbType.VarChar, 10);
                arr[6] = new SqlParameter("@MaterialDescription", SqlDbType.VarChar, 250);
                arr[7] = new SqlParameter("@GatePassInNo", SqlDbType.VarChar, 50);                
                arr[8] = new SqlParameter("@VehicleNo", SqlDbType.VarChar, 20);
                arr[9] = new SqlParameter("@Through", SqlDbType.VarChar, 50);
                arr[10] = new SqlParameter("@MobileNo", SqlDbType.VarChar, 15);
                arr[11] = new SqlParameter("@InTime", SqlDbType.VarChar, 15);              
                arr[12] = new SqlParameter("@Remarks", SqlDbType.VarChar, 300);
                arr[13] = new SqlParameter("@GateType", SqlDbType.Int);
                arr[14] = new SqlParameter("@MaterialType", SqlDbType.Int);
                arr[15] = new SqlParameter("@UserID", SqlDbType.Int);
                arr[16] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
                arr[17] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                arr[18] = new SqlParameter("@BranchID", SqlDbType.Int);

                arr[0].Direction = ParameterDirection.InputOutput;
                arr[0].Value = ViewState["GateInMaterialRegisterID"];
                arr[1].Value = txtdate.Text;
                arr[2].Value = ddCompName.SelectedValue;
                arr[3].Value = txtPartyName.Text;
                arr[4].Value = TxtQty.Text == "" ? "0" : TxtQty.Text;
                arr[5].Value = txtUnit.Text;
                arr[6].Value = txtMaterialDescription.Text;
                arr[7].Direction = ParameterDirection.InputOutput;
                //arr[7].Value = ViewState["GateInMaterialRegisterID"];
                //arr[7].Value = TxtGatePassInNo.Text;                
                arr[8].Value = txtVehicleNo.Text.ToUpper();
                arr[9].Value = txtThrough.Text;
                arr[10].Value = txtMobileNo.Text;
                arr[11].Value = txtInTime.Text;                
                arr[12].Value = txtremarks.Text;
                arr[13].Value = ddGateType.SelectedValue;
                arr[14].Value = DDMaterialReturnType.SelectedValue;
                arr[15].Value = Session["varuserid"];
                arr[16].Value = Session["varCompanyId"];
                arr[17].Direction = ParameterDirection.Output;
                arr[18].Value = DDBranchName.SelectedValue;

                SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[PRO_GateInMaterialRegister]", arr);
                LblErrorMessage.Text = arr[17].Value.ToString();
                LblErrorMessage.Visible = true;
                //ViewState["GateInMaterialRegisterID"] = arr[0].Value;           
                tran.Commit();
                btnsave.Text = "Save";
                refreshForm();
                fill_MaterialIngrid();
            }
            else if (ddGateType.SelectedValue == "2")
            {
                SqlParameter[] arr = new SqlParameter[21];
                arr[0] = new SqlParameter("@GateOutMaterialRegisterID", SqlDbType.Int);
                arr[1] = new SqlParameter("@GateOutMaterialDate", SqlDbType.DateTime);
                arr[2] = new SqlParameter("@CompanyID", SqlDbType.Int);
                arr[3] = new SqlParameter("@PartyName", SqlDbType.VarChar, 50);
                arr[4] = new SqlParameter("@Qty", SqlDbType.Float);
                arr[5] = new SqlParameter("@Unit", SqlDbType.VarChar, 10);
                arr[6] = new SqlParameter("@MaterialDescription", SqlDbType.VarChar, 250);              
                arr[7] = new SqlParameter("@GatePassOutNo", SqlDbType.VarChar, 50);
                arr[8] = new SqlParameter("@VehicleNo", SqlDbType.VarChar, 20);
                arr[9] = new SqlParameter("@Through", SqlDbType.VarChar, 50);
                arr[10] = new SqlParameter("@MobileNo", SqlDbType.VarChar, 15);              
                arr[11] = new SqlParameter("@OutTime", SqlDbType.VarChar, 15);
                arr[12] = new SqlParameter("@Remarks", SqlDbType.VarChar, 300);
                arr[13] = new SqlParameter("@GateType", SqlDbType.Int);
                arr[14] = new SqlParameter("@MaterialType", SqlDbType.Int);
                arr[15] = new SqlParameter("@UserID", SqlDbType.Int);
                arr[16] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
                arr[17] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                arr[18] = new SqlParameter("@BranchID", SqlDbType.Int);
                
                arr[0].Direction = ParameterDirection.InputOutput;
                arr[0].Value = ViewState["GateOutMaterialRegisterID"];
                arr[1].Value = txtdate.Text;
                arr[2].Value = ddCompName.SelectedValue;
                arr[3].Value = txtPartyName.Text;
                arr[4].Value = TxtQty.Text == "" ? "0" : TxtQty.Text;
                arr[5].Value = txtUnit.Text;
                arr[6].Value = txtMaterialDescription.Text;

                //arr[7].Value = txtGatePassOutNo.Text;
                arr[7].Direction = ParameterDirection.InputOutput;
                arr[8].Value = txtVehicleNo.Text.ToUpper();
                arr[9].Value = txtThrough.Text;
                arr[10].Value = txtMobileNo.Text;             
                arr[11].Value = txtOutTime.Text;
                arr[12].Value = txtremarks.Text;
                arr[13].Value = ddGateType.SelectedValue;
                arr[14].Value = DDMaterialReturnType.SelectedValue;
                arr[15].Value = Session["varuserid"];
                arr[16].Value = Session["varCompanyId"];
                arr[17].Direction = ParameterDirection.Output;
                arr[18].Value = DDBranchName.SelectedValue;

                SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[PRO_GateOutMaterialRegister]", arr);
                LblErrorMessage.Text = arr[17].Value.ToString();
                LblErrorMessage.Visible = true;
                //ViewState["GateOutMaterialRegisterID"] = arr[0].Value;           
                tran.Commit();
                btnsave.Text = "Save";
                refreshForm();
                fill_MaterialOutgrid();
            }

            if (ddGateType.SelectedValue == "1")
            {
                txtInTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            }
            else if (ddGateType.SelectedValue == "2")
            {
                txtOutTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            }
        }
        catch (Exception ex)
        {
            tran.Rollback();
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SaveData();
    }
    private void refreshForm()
    {
        txtPartyName.Text = "";
        TxtQty.Text = "";
        txtUnit.Text = "";
        txtMaterialDescription.Text = "";
        TxtGatePassInNo.Text = "";
        txtVehicleNo.Text = "";
        txtThrough.Text = "";
        txtMobileNo.Text = "";
        txtInTime.Text = "";
        txtremarks.Text = "";
        txtGatePassOutNo.Text = "";
        txtOutTime.Text = "";
    }   
    private void fill_MaterialIngrid()
    {
        string Where = "";
        if (ChKForEdit.Checked == true)
        {
            if (DDGatePassNo.SelectedIndex > 0)
            {
                Where = Where + " And GatePassInNo=" + DDGatePassNo.SelectedValue;
            }
            else
            {
                if (ddCompName.SelectedIndex > 0)
                {
                    Where = Where + " And CI.CompanyId=" + ddCompName.SelectedValue;
                }
                Where = Where + " and CAST(GateInMaterialDate as date)= CAST(GETDATE() AS DATE)";
                //Where = Where + " And GateInMaterialDate>= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0)";
                //Where = Where + " And GateInMaterialDate < DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) + 1, 0)"; 
            }
        }
        if (ChKForEdit.Checked == false)
        {
            if (ddCompName.SelectedIndex > 0)
            {
                Where = Where + " And CI.CompanyId=" + ddCompName.SelectedValue;
            }
            Where = Where + " and CAST(GateInMaterialDate as date)= CAST(GETDATE() AS DATE)";
            //Where = Where + " And GateInMaterialDate>= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0)";
            //Where = Where + " And GateInMaterialDate < DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) + 1, 0)"; 
        }
       // Where = Where + " Order by GIMR.GatePassInNo desc";

        DataSet ds = new DataSet();       

        SqlParameter[] param = new SqlParameter[2];
        //param[0] = new SqlParameter("@GateType", ddGateType.SelectedValue);
        //param[1] = new SqlParameter("@CompanyId", ddCompName.SelectedValue);
        param[0] = new SqlParameter("@Where", Where);
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetGateInMaterialRegisterDetail", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvMaterialIndetail.DataSource = ds;
            gvMaterialIndetail.DataBind();
        }
        else
        {
            gvMaterialIndetail.DataSource = "";
            gvMaterialIndetail.DataBind();
        }
    }
    protected void gvMaterialIndetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvMaterialIndetail, "Select$" + e.Row.RowIndex);
        }   

    }
    protected void gvMaterialIndetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[5];
            arr[0] = new SqlParameter("@GateInMaterialRegisterId", SqlDbType.Int);
            arr[1] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            arr[2] = new SqlParameter("@UserID", SqlDbType.Int);
            arr[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            arr[0].Value = gvMaterialIndetail.DataKeys[e.RowIndex].Value;
            arr[1].Value = Session["varCompanyId"];           
            arr[2].Value = Session["varuserid"];
            arr[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_DeleteGateInMaterialRegisterData", arr);
            tran.Commit();
            LblErrorMessage.Text = arr[3].Value.ToString();
            LblErrorMessage.Visible = true;
            fill_MaterialIngrid();
           
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
            tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void gvMaterialIndetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    private void fill_MaterialOutgrid()
    {
        string Where = "";
        if (ChKForEdit.Checked == true)
        {
            if (DDGatePassNo.SelectedIndex > 0)
            {
                Where = Where + " And GatePassOutNo=" + DDGatePassNo.SelectedValue;
            }
            else
            {
                if (ddCompName.SelectedIndex > 0)
                {
                    Where = Where + " And CI.CompanyId=" + ddCompName.SelectedValue;
                }
                Where = Where + " and CAST(GateOutMaterialDate as date)= CAST(GETDATE() AS DATE)";

                //Where = Where + " And GateOutMaterialDate>= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0)";
                //Where = Where + " And GateOutMaterialDate < DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) + 1, 0)";

                
            }
        }
        if (ChKForEdit.Checked == false)
        {
            if (ddCompName.SelectedIndex > 0)
            {
                Where = Where + " And CI.CompanyId=" + ddCompName.SelectedValue;
            }
            Where = Where + " and CAST(GateOutMaterialDate as date)= CAST(GETDATE() AS DATE)";

            //Where = Where + " And GateOutMaterialDate>= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0)";
            //Where = Where + " And GateOutMaterialDate < DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) + 1, 0)";
        }
        DataSet ds = new DataSet();

        SqlParameter[] param = new SqlParameter[2];
       // param[0] = new SqlParameter("@GateType", ddGateType.SelectedValue);
        //param[1] = new SqlParameter("@CompanyId", ddCompName.SelectedValue);
        param[0] = new SqlParameter("@Where", Where);
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetGateOutMaterialRegisterDetail", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GVMaterialOutDetail.DataSource = ds;
            GVMaterialOutDetail.DataBind();
        }
        else
        {
            GVMaterialOutDetail.DataSource = "";
            GVMaterialOutDetail.DataBind();
        }

    }
    protected void GVMaterialOutDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVMaterialOutDetail, "Select$" + e.Row.RowIndex);
        } 

    }
    protected void GVMaterialOutDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[5];
            arr[0] = new SqlParameter("@GateOutMaterialRegisterId", SqlDbType.Int);
            arr[1] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            arr[2] = new SqlParameter("@UserID", SqlDbType.Int);
            arr[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            arr[0].Value = GVMaterialOutDetail.DataKeys[e.RowIndex].Value;
            arr[1].Value = Session["varCompanyId"];
            arr[2].Value = Session["varuserid"];
            arr[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_DeleteGateOutMaterialRegisterData", arr);
            tran.Commit();
            LblErrorMessage.Text = arr[3].Value.ToString();
            LblErrorMessage.Visible = true;
            fill_MaterialOutgrid();

        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
            tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void GVMaterialOutDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    protected void BtnPreviewIn_Click(object sender, EventArgs e)
    {
        //Determine the RowIndex of the Row whose Button was clicked.
        int rowIndex = ((sender as Button).NamingContainer as GridViewRow).RowIndex;

        //Get the value of column from the DataKeys using the RowIndex.
        int id = Convert.ToInt32(gvMaterialIndetail.DataKeys[rowIndex].Values[0]);
        //ViewState["InMaterialRegisterId"] = id;

        ViewState["OutMaterialRegisterId"] = id;

        report();
    }
    protected void BtnPreviewOut_Click(object sender, EventArgs e)
    {
        ViewState["OutMaterialRegisterId"] = 0;
        //Determine the RowIndex of the Row whose Button was clicked.
        int rowIndex = ((sender as Button).NamingContainer as GridViewRow).RowIndex;

        //Get the value of column from the DataKeys using the RowIndex.
        int id = Convert.ToInt32(GVMaterialOutDetail.DataKeys[rowIndex].Values[0]);

        ViewState["OutMaterialRegisterId"] = id;

        report();
    }
    private void report()
    {
        DataSet ds = new DataSet();
        // string qry = "";
        // string str = "";
        SqlParameter[] array = new SqlParameter[2];
        array[0] = new SqlParameter("@GateOutMaterialRegisterId", SqlDbType.Int);
        array[1] = new SqlParameter("@GateInOutType", SqlDbType.Int);

        array[0].Value = ViewState["OutMaterialRegisterId"];
        array[1].Value = ddGateType.SelectedValue;

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForGateOutMaterialReport", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\GateOutMaterial.rpt";

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\GateOutMaterial.xsd";
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
}