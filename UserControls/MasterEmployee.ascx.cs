using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using ClosedXML.Excel;

public partial class UserControls_MasterEmployee : System.Web.UI.UserControl 
{
    public enum enumCardType
    {
        pdf , jpg
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            Tabemp.ActiveTabIndex = 0;
            txtEmpId.Text = "0";
            string Str =@"SELECT DepartmentId,DepartmentName FROM Department Where MasterCompanyId=" + Session["varCompanyId"] + @" Order By DepartmentName 
                Select Distinct TT.ID,TT.Type From TDSType TT,TDS_MASTER TM Where TT.ID=TM.TYPEID And MasterCompanyId=" + Session["varCompanyId"] + @" 
                select stateid,statename from state_master
                Select ID, BranchName 
                From BRANCHMASTER BM(nolock) 
                JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"] + " Order By BranchName";
            
            DataSet DSQ = SqlHelper.ExecuteDataset(Str);
            UtilityModule.ConditionalComboFillWithDS(ref DDDepartment, DSQ, 0, true, "--Select Department--");

            if (Request.QueryString["ABC"] == "1")
            {
                DDDepartment.SelectedValue = "2";
            }

            UtilityModule.ConditonalChkBoxListFill(ref ChLProcess, "Select Process_Name_id,Process_Name from Process_Name_Master Where MasterCompanyId=" + Session["varCompanyId"] + @" order by Process_Name_id ");
            UtilityModule.ConditionalComboFillWithDS(ref DDType, DSQ, 1, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDstate, DSQ, 2, true, "--select state--");
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, DSQ, 3, false, "");

            Session["CommanFormula"] = "";
            txtEmpName.Focus();
            
            if (Session["varCompanyNo"].ToString() == "7")
            {
                tdfax.Visible = false;
                tdfaxid.Visible = false;
                tdopen.Visible = false;
            }
            else
            {
                btnadd.Visible = true;
            }
            switch (Session["varcompanyid"].ToString())
            {
                case "9"://for hafizia "9"
                    address.Text = "Village/city";
                    TREmpName2.Visible = true;
                    TREmpName2SecondRow.Visible = true;
                    break;
                case "12":
                    chkvendorrate.Visible = true;
                    TREmpName2.Visible = false;
                    break;
                default:
                    address.Text = "Address";
                    TREmpName2.Visible = false;
                    break;
            }

            ClearAll();
            //Fill_Grid();
            //**********************
            if (variable.VarPayrollDetail == "1")
            {
                TabpanelHrDetail.Visible = true;
                UtilityModule.NewChkBoxListFill(ref chkdocuments, "select ID,DocumentName From DocumentMaster order by ID");
            }
            else
            {
                TabpanelHrDetail.Visible = false;
            }
        }
    }
    private void Fill_Grid()
    {
        dgRaw1.DataSource = Fill_Grid_Data();
        dgRaw1.DataBind();
        Session["ReportPath"] = "Reports/Weaver.rpt";
        if (DDDepartment.SelectedIndex > 1)
        {
            Session["CommanFormula"] = "{EmpInfo.Departmentid}=" + DDDepartment.SelectedValue;
        }
        if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
        {
            Session["CommanFormula"] = Session["CommanFormula"] + " And {EmpInfo.USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR} = 1";
        }
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"select B.EmpId as SrNo,D.DepartmentName   Department,B.EmpName,B.FatherName, B.Address,B.Address2,B.Address3,B.PhoneNo,
                            B.Mobile,B.Fax,isnull(B.District,'') As District,isnull(sm.StateName,'') As StateName,B.EmpCode,B.PanNo,B.AadharNo,B.GSTNo, 
                            case When B.Blacklist=1 Then 'Blocked' Else 'Active' End as Status 
                            From EmpInfo B
                            JOIN Department D ON D.DepartmentId = B.DepartmentId 
                            Left join State_master sm on B.StateId=sm.stateid 
                            where 1 = 1";

            if (Convert.ToInt32(Session["varCompanyNo"]) == 16 || Convert.ToInt32(Session["varCompanyNo"]) == 28)
            {
                strsql = strsql + " And B.BranchID = " + DDBranchName.SelectedValue;
            }

            if (trEmployeeType.Visible == true && ddlEmployeeType.SelectedIndex > 0)
            {
                strsql = strsql + " And B.EmployeeType=" + ddlEmployeeType.SelectedValue;
            }
            if (txtempcode.Text != "")
            {
                strsql = strsql + " And B.EMpcode='" + txtempcode.Text + "'";
            }
            if (txtEmpName.Text != "")
            {
                strsql = strsql + " And B.EMPNAME='" + txtEmpName.Text + "'";
            }
            if (DDDepartment.SelectedIndex > 0)
            {
                strsql = strsql + " And D.Departmentid=" + DDDepartment.SelectedValue + "";
            }
            if (DDparttype.SelectedValue != "-1")
            {
                strsql = strsql + " And  B.PartyType=" + DDparttype.SelectedValue;
            }

            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                strsql = strsql + " And B.USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR = 1";
            }

            strsql = strsql + " And B.MasterCompanyId=" + Session["varCompanyId"] + " order by Empname";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmWeaver.aspx");
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
    protected void cmdCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
    }
    protected void dgRaw1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = dgRaw1.SelectedDataKey.Value.ToString();
        //Session["id"] = id;
        ViewState["id"] = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            ClearAll();
            DataSet ds = null, ds1 = null;
            string Str = @"select EmpId, Departmentid, EmpName, Address, PhoneNo, Mobile, Fax, Remarks, CSTNo, 
            EmailAdd, TinNo, PanNo, userid, MasterCompanyid, TypeId, PartyType, District, StateId, EmpCode, Address2, Address3, Capacity, Blacklist,
            AadharNo, Status, FatherName, GSTNo, EmployeeType,Replace(convert(nvarchar(11),DOB,106),' ','-') as DOB, Nominee,Replace(convert(nvarchar(11),DOJ,106),' ','-') as DOJ, Pensionallow, OTallow, Stafftype, EMPACNo, IPNo, EMP_BankName, 
            EMP_BranchName, EMP_IfscCode, EMP_AcNo,empgroupid,isnull(submitdocids,0) as submitdocids,isnull(Guarantorname,'') as Guarantorname,isnull(emptype,'') as Emptype,Emp_designation,isnull(empphoto,'') as empphoto, 
            isnull(EMP_GSTTYPE,0) as EMP_GSTTYPE, isnull(Purchase_EmpBankDetails,'') as Purchase_EmpBankDetails,isnull(EmpVendorName2,'') as EmpVendorName2,
            isnull(EmpVendorAddress2,'') EmpVendorAddress2, IsNull(Emp2PanNo,'') Emp2PanNo, IsNull(Emp2AadharNo,'') Emp2AadharNo, IsNull(Emp2MobileNo,'') Emp2MobileNo 
            from EmpInfo(nolock) WHERE MasterCompanyId=" + Session["varCompanyId"] + " And EmpId=" + id;

            if (Convert.ToInt32(Session["usertype"]) > 1)
            {
                Str = Str + "And Blacklist = 0 ";
            }
            
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);            

            ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select ProcessId from EmpProcess(nolock) WHERE EmpId=" + id + " order by ProcessId");
            int n = ds1.Tables[0].Rows.Count;

            int a = ChLProcess.Items.Count;
            if (a > 0)
            {
                for (int i = 0, j = 0; i < a; i++)
                {
                    if (j < n)
                    {
                        if ((ChLProcess.Items[i].Value == ds1.Tables[0].Rows[j]["ProcessId"].ToString()))
                        {
                            ChLProcess.Items[i].Selected = true;
                            j++;
                        }
                        else
                        {
                            ChLProcess.Items[i].Selected = false;
                        }
                    }
                    else
                    {
                        ChLProcess.Items[i].Selected = false;
                    }
                }
            }
            //***************Documents          

            string Documents = ds.Tables[0].Rows[0]["submitdocids"].ToString();
            string[] split = Documents.Split(',');

            foreach (string item in split)
            {
                for (int i = 0; i < chkdocuments.Items.Count; i++)
                {
                    if (item == chkdocuments.Items[i].Value)
                    {
                        chkdocuments.Items[i].Selected = true;
                        break;
                    }
                }
            }
            //****************
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtEmpId.Text = ds.Tables[0].Rows[0]["EmpId"].ToString();
                DDDepartment.SelectedValue = ds.Tables[0].Rows[0]["DepartmentId"].ToString();
                txtEmpName.Text = ds.Tables[0].Rows[0]["EmpName"].ToString();
                txtFatherName.Text = ds.Tables[0].Rows[0]["FatherName"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtPhone.Text = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
                txtMobile.Text = ds.Tables[0].Rows[0]["Mobile"].ToString();
                txtFax.Text = ds.Tables[0].Rows[0]["Fax"].ToString();
                txtCST.Text = ds.Tables[0].Rows[0]["CSTNo"].ToString();
                txtEmailadd.Text = ds.Tables[0].Rows[0]["EmailAdd"].ToString();
                txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                txtTin.Text = ds.Tables[0].Rows[0]["TinNo"].ToString();
                TxtPanNo.Text = ds.Tables[0].Rows[0]["PanNo"].ToString();
                DDparttype.SelectedValue = ds.Tables[0].Rows[0]["PartyType"].ToString();
                btnSave.Text = "Update";
                txtDistrict.Text = ds.Tables[0].Rows[0]["District"].ToString();
                DDstate.SelectedValue = ds.Tables[0].Rows[0]["stateid"].ToString();
                txtempcode.Text = ds.Tables[0].Rows[0]["EmpCode"].ToString();
                txtaddress2.Text = ds.Tables[0].Rows[0]["Address2"].ToString();
                txtaddress3.Text = ds.Tables[0].Rows[0]["Address3"].ToString();
                txtcapacity.Text = ds.Tables[0].Rows[0]["Capacity"].ToString();
                txtaadharNo.Text = ds.Tables[0].Rows[0]["AadharNo"].ToString();
                txtGstNo.Text = ds.Tables[0].Rows[0]["GSTNo"].ToString().ToUpper();
                txtdesignation.Text = ds.Tables[0].Rows[0]["emp_designation"].ToString();
                DDGSType.SelectedValue = ds.Tables[0].Rows[0]["EMP_GSTTYPE"].ToString();
                txtBankDetails.Text = ds.Tables[0].Rows[0]["Purchase_EmpBankDetails"].ToString();
                if (ds.Tables[0].Rows[0]["empphoto"].ToString() != "")
                {
                    lblphotoimage.ImageUrl = ds.Tables[0].Rows[0]["empphoto"].ToString() + "?" + DateTime.Now.Ticks.ToString();
                }
                else
                {
                    lblphotoimage.ImageUrl = null;
                }
                if (ds.Tables[0].Rows[0]["blacklist"].ToString() == "1")
                {
                    chkblacklist.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["PartyType"].ToString() == "1")
                {
                    trEmployeeType.Visible = true;
                    ddlEmployeeType.SelectedValue = ds.Tables[0].Rows[0]["EmployeeType"].ToString();
                    ddlEmployeeType_SelectedIndexChanged(sender, new EventArgs());
                }
                else
                {
                    trEmployeeType.Visible = false;
                    ddlEmployeeType.SelectedValue = "-1";
                }
                if (TREmpName2.Visible == true)
                {
                    txtEmpName2.Text = ds.Tables[0].Rows[0]["EmpVendorName2"].ToString();
                    txtEmpAddress2.Text = ds.Tables[0].Rows[0]["EmpVendorAddress2"].ToString();
                    TxtEmp2PanNo.Text= ds.Tables[0].Rows[0]["Emp2PanNo"].ToString();
                    TxtEmp2AadharNo.Text = ds.Tables[0].Rows[0]["Emp2AadharNo"].ToString();
                    TxtEmp2MobileNo.Text = ds.Tables[0].Rows[0]["Emp2MobileNo"].ToString();
                }
                else
                {
                    txtEmpName2.Text = "";
                    txtEmpAddress2.Text = "";
                    TxtEmp2AadharNo.Text = "";
                    TxtEmp2PanNo.Text = "";
                    TxtEmp2MobileNo.Text = "";
                }


                //**************HR DETAIL
                if (TRHrDetail.Visible == true)
                {

                    txtdob.Text = ds.Tables[0].Rows[0]["DOB"].ToString();
                    txtdoj.Text = ds.Tables[0].Rows[0]["DOJ"].ToString();
                    txtnominee.Text = ds.Tables[0].Rows[0]["Nominee"].ToString();
                    if (DDpensionallow.Items.FindByText(ds.Tables[0].Rows[0]["Pensionallow"].ToString()) != null)
                    {
                        DDpensionallow.SelectedValue = ds.Tables[0].Rows[0]["Pensionallow"].ToString();
                    }
                    if (DDOvertimeallow.Items.FindByText(ds.Tables[0].Rows[0]["OTallow"].ToString()) != null)
                    {
                        DDOvertimeallow.SelectedValue = ds.Tables[0].Rows[0]["OTallow"].ToString();
                    }
                    if (DDstafftype.Items.FindByText(ds.Tables[0].Rows[0]["Stafftype"].ToString()) != null)
                    {
                        DDstafftype.SelectedValue = ds.Tables[0].Rows[0]["Stafftype"].ToString();
                    }

                    txtempacno.Text = ds.Tables[0].Rows[0]["EMPACNO"].ToString();
                    txtIPno.Text = ds.Tables[0].Rows[0]["IPNO"].ToString();
                    txtbankname.Text = ds.Tables[0].Rows[0]["EMP_BANKNAME"].ToString();
                    txtbranchname.Text = ds.Tables[0].Rows[0]["EMP_BranchName"].ToString();
                    txtifsccode.Text = ds.Tables[0].Rows[0]["EMP_IFSCCODE"].ToString();
                    txtacno.Text = ds.Tables[0].Rows[0]["EMP_ACNO"].ToString();
                    if (DDempgroup.Items.FindByValue(ds.Tables[0].Rows[0]["empgroupid"].ToString()) != null)
                    {
                        DDempgroup.SelectedValue = ds.Tables[0].Rows[0]["empgroupid"].ToString();
                    }
                    txtGuarantor.Text = ds.Tables[0].Rows[0]["guarantorname"].ToString();
                    DDemptype.SelectedValue = ds.Tables[0].Rows[0]["emptype"].ToString();

                }
                //********************
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmWeaver.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void dgRaw1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.dgRaw1, "Select$" + e.Row.RowIndex);
            Label lblstatus = ((Label)e.Row.FindControl("lblstatus"));
            if (lblstatus.Text == "Blocked")
            {
                e.Row.BackColor = System.Drawing.Color.Red;
            }
            if (variable.VarPayrollDetail == "1")
            {

                for (int i = 0; i < dgRaw1.Columns.Count; i++)
                {

                    if (dgRaw1.Columns[i].HeaderText == "Upload Docs")
                    {
                        dgRaw1.Columns[i].Visible = true;
                        dgRaw1.Columns[i].ShowHeader = false;
                    }
                }

            }
        }
    }
    private void ClearAll()
    {
        txtAddress.Text = "";
        txtempcode.Text = "";
        txtCST.Text = "";
        txtFax.Text = "";
        txtMobile.Text = "";
        txtPhone.Text = "";
        txtRemarks.Text = "";
        txtTin.Text = "";
        txtEmailadd.Text = "";
        txtEmpName.Text = "";
        txtOpeningBal.Text = "";
        TxtPanNo.Text = "";

        txtEmpId.Text = "0";
        btnSave.Text = "Save";
        txtDistrict.Text = "";
        txtaddress2.Text = "";
        txtaddress3.Text = "";
        txtcapacity.Text = "";
        chkblacklist.Checked = false;
        txtaadharNo.Text = "";
        txtGstNo.Text = "";
        txtFatherName.Text = "";
        txtdob.Text = "";
        txtnominee.Text = "";
        txtdoj.Text = "";
        txtempacno.Text = "";
        txtIPno.Text = "";
        txtbankname.Text = "";
        txtbranchname.Text = "";
        txtifsccode.Text = "";
        txtacno.Text = "";
        txtGuarantor.Text = "";
        DDempgroup.SelectedIndex = -1;
        chkdocuments.ClearSelection();
        txtdesignation.Text = "";
        UtilityModule.ConditonalChkBoxListFill(ref ChLProcess, "Select Process_Name_id,Process_Name from Process_Name_Master Where MasterCompanyId=" + Session["varCompanyId"] + " order by Process_Name_id");
        DDemptype.SelectedValue = "";
        txtEmpName2.Text = "";
        txtEmpAddress2.Text = "";
        TxtEmp2AadharNo.Text = "";
        TxtEmp2PanNo.Text = "";
        TxtEmp2MobileNo.Text = "";
    }
    protected void dgRaw1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgRaw1.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {
            if (Request.QueryString["id"] == "1")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", "onSuccess()", true);
            }
        }
        else
        {
            txtAddress.Text = "";
            txtCST.Text = "";
            txtEmpId.Text = "0";
            txtEmpName.Text = "";
            txtFax.Text = "";
            txtMobile.Text = "";
            txtOpeningBal.Text = "";
            TxtPanNo.Text = "";
            txtPhone.Text = "";
            txtRemarks.Text = "";
            txtTin.Text = "";
            txtEmailadd.Text = "";
            btnSave.Text = "Save";
            txtDistrict.Text = "";
            txtaddress2.Text = "";
            txtaddress3.Text = "";
            txtGstNo.Text = "";
            if (TREmpName2.Visible == true)
            {
                txtEmpName2.Text = "";
                txtEmpAddress2.Text = "";
            }
            if (TREmpName2SecondRow.Visible == true)
            {
                TxtEmp2AadharNo.Text = "";
                TxtEmp2PanNo.Text = "";
                TxtEmp2MobileNo.Text = "";
            }
        }
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
            SqlParameter[] _arrPara = new SqlParameter[56];
            _arrPara[0] = new SqlParameter("@EmpId", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@DepartmentId", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@EmpName", SqlDbType.NVarChar, 50);
            _arrPara[3] = new SqlParameter("@Address", SqlDbType.NVarChar, 100);
            _arrPara[4] = new SqlParameter("@PhoneNo", SqlDbType.NVarChar, 50);
            _arrPara[5] = new SqlParameter("@Mobile", SqlDbType.NVarChar, 50);
            _arrPara[6] = new SqlParameter("@Fax", SqlDbType.NVarChar, 50);
            _arrPara[7] = new SqlParameter("@Remarks", SqlDbType.NVarChar, 150);
            _arrPara[8] = new SqlParameter("@CSTNo", SqlDbType.NVarChar, 50);
            _arrPara[9] = new SqlParameter("@EmailAdd", SqlDbType.NVarChar, 50);
            _arrPara[10] = new SqlParameter("@TinNo", SqlDbType.NVarChar, 50);
            _arrPara[11] = new SqlParameter("@PanNo", SqlDbType.NVarChar, 50);
            _arrPara[12] = new SqlParameter("@varuserid", SqlDbType.Int);
            _arrPara[13] = new SqlParameter("@varCompanyId", SqlDbType.Int);
            _arrPara[14] = new SqlParameter("@ProcessDetail", SqlDbType.NVarChar, 100);
            _arrPara[15] = new SqlParameter("@TypeID", SqlDbType.Int);
            _arrPara[16] = new SqlParameter("@Partytype", SqlDbType.Int);
            _arrPara[17] = new SqlParameter("@Msg", SqlDbType.NVarChar, 100);
            _arrPara[18] = new SqlParameter("@District", SqlDbType.VarChar, 50);
            _arrPara[19] = new SqlParameter("@stateid", SqlDbType.Int);
            _arrPara[20] = new SqlParameter("@EmpCode", SqlDbType.VarChar, 30);
            _arrPara[21] = new SqlParameter("@Address2", SqlDbType.VarChar, 250);
            _arrPara[22] = new SqlParameter("@Address3", SqlDbType.VarChar, 250);
            _arrPara[23] = new SqlParameter("@Capacity", SqlDbType.Int);
            _arrPara[24] = new SqlParameter("@Blacklist", SqlDbType.TinyInt);
            _arrPara[25] = new SqlParameter("@AadharNo", SqlDbType.VarChar, 50);
            _arrPara[26] = new SqlParameter("@FatherName", SqlDbType.VarChar, 100);
            _arrPara[27] = new SqlParameter("@GSTNo", SqlDbType.VarChar, 100);
            _arrPara[28] = new SqlParameter("@EmployeeType", SqlDbType.Int);
            _arrPara[29] = new SqlParameter("@DOB", SqlDbType.DateTime);
            _arrPara[30] = new SqlParameter("@Nominee", SqlDbType.VarChar, 50);
            _arrPara[31] = new SqlParameter("@DOJ", SqlDbType.DateTime);
            _arrPara[32] = new SqlParameter("@Pensionallow", SqlDbType.VarChar, 5);
            _arrPara[33] = new SqlParameter("@OTallow", SqlDbType.VarChar, 5);
            _arrPara[34] = new SqlParameter("@Stafftype", SqlDbType.VarChar, 5);
            _arrPara[35] = new SqlParameter("@EMPACNO", SqlDbType.VarChar, 30);
            _arrPara[36] = new SqlParameter("@IPNo", SqlDbType.VarChar, 30);
            _arrPara[37] = new SqlParameter("@EMP_BankName", SqlDbType.VarChar, 50);
            _arrPara[38] = new SqlParameter("@EMP_BranchName", SqlDbType.VarChar, 50);
            _arrPara[39] = new SqlParameter("@EMP_Ifsccode", SqlDbType.VarChar, 30);
            _arrPara[40] = new SqlParameter("@EMP_Acno", SqlDbType.VarChar, 30);
            _arrPara[41] = new SqlParameter("@Empgroupid", SqlDbType.Int);
            _arrPara[42] = new SqlParameter("@SubmitDocids", SqlDbType.VarChar, 100);
            _arrPara[43] = new SqlParameter("@userType", SqlDbType.Int);
            _arrPara[44] = new SqlParameter("@Guarantorname", SqlDbType.VarChar, 50);
            _arrPara[45] = new SqlParameter("@Emptype", SqlDbType.VarChar, 30);
            _arrPara[46] = new SqlParameter("@Emp_Designation", txtdesignation.Text);
            _arrPara[47] = new SqlParameter("@EMP_GSTTYPE", SqlDbType.Int);
            _arrPara[48] = new SqlParameter("@Purchase_EmpBankDetails", SqlDbType.VarChar,200);
            _arrPara[49] = new SqlParameter("@EmpVendorName2", SqlDbType.VarChar, 50);
            _arrPara[50] = new SqlParameter("@EmpVendorAddress2", SqlDbType.VarChar, 200);
            _arrPara[51] = new SqlParameter("@Emp2PanNo", SqlDbType.VarChar, 50);
            _arrPara[52] = new SqlParameter("@Emp2AadharNo", SqlDbType.VarChar, 50);
            _arrPara[53] = new SqlParameter("@Emp2MobileNo", SqlDbType.VarChar, 50);
            _arrPara[54] = new SqlParameter("@BranchID", SqlDbType.Int);

            _arrPara[0].Value = txtEmpId.Text;
            _arrPara[1].Value = DDDepartment.SelectedValue;
            _arrPara[2].Value = txtEmpName.Text.ToUpper();
            _arrPara[3].Value = txtAddress.Text.ToUpper();
            _arrPara[4].Value = txtPhone.Text.ToUpper();
            _arrPara[5].Value = txtMobile.Text.ToUpper();
            _arrPara[6].Value = txtFax.Text.ToUpper();
            _arrPara[7].Value = txtRemarks.Text.ToUpper();
            _arrPara[8].Value = txtCST.Text.ToUpper();
            _arrPara[9].Value = txtEmailadd.Text.ToUpper();
            _arrPara[10].Value = txtTin.Text.ToUpper();
            _arrPara[11].Value = TxtPanNo.Text.ToUpper();
            _arrPara[12].Value = Session["varuserid"].ToString();
            _arrPara[13].Value = Session["varCompanyId"].ToString();

            int n = ChLProcess.Items.Count;
            string str = null;
            for (int i = 0; i < n; i++)
            {
                if (ChLProcess.Items[i].Selected)
                {
                    str = str == null ? ChLProcess.Items[i].Value : str + "," + ChLProcess.Items[i].Value;
                }
            }
            _arrPara[14].Value = str;
            _arrPara[15].Value = DDType.SelectedIndex != -1 ? DDType.SelectedValue : "0";
            _arrPara[16].Value = DDparttype.SelectedValue;
            _arrPara[17].Direction = ParameterDirection.Output;
            _arrPara[18].Value = txtDistrict.Text.ToUpper();
            _arrPara[19].Value = DDstate.SelectedIndex != -1 ? DDstate.SelectedValue : "0";
            _arrPara[20].Value = txtempcode.Text.ToUpper();
            _arrPara[21].Value = txtaddress2.Text.ToUpper();
            _arrPara[22].Value = txtaddress3.Text.ToUpper();
            _arrPara[23].Value = txtcapacity.Text == "" ? "0" : txtcapacity.Text;
            _arrPara[24].Value = chkblacklist.Checked == true ? 1 : 0;
            _arrPara[25].Value = txtaadharNo.Text.ToUpper();
            _arrPara[26].Value = txtFatherName.Text;
            _arrPara[27].Value = txtGstNo.Text.ToUpper();
            if (DDparttype.SelectedValue == "1")
            {
                _arrPara[28].Value = ddlEmployeeType.SelectedValue;
            }
            else
            {
                _arrPara[28].Value = "-1";
            }

            _arrPara[29].Value = txtdob.Text == "" ? DBNull.Value : (object)txtdob.Text;
            _arrPara[30].Value = txtnominee.Text;
            _arrPara[31].Value = txtdoj.Text == "" ? DBNull.Value : (object)txtdoj.Text;
            _arrPara[32].Value = DDpensionallow.SelectedItem.Text;
            _arrPara[33].Value = DDOvertimeallow.SelectedItem.Text;
            _arrPara[34].Value = DDstafftype.SelectedItem.Text;
            _arrPara[35].Value = txtempacno.Text;
            _arrPara[36].Value = txtIPno.Text;
            _arrPara[37].Value = txtbankname.Text;
            _arrPara[38].Value = txtbranchname.Text;
            _arrPara[39].Value = txtifsccode.Text;
            _arrPara[40].Value = txtacno.Text;
            _arrPara[41].Value = DDempgroup.SelectedIndex > 0 ? DDempgroup.SelectedValue : "0";
            string Submitdocids = "";
            //*************
            if (variable.VarPayrollDetail == "1")
            {
                for (int i = 0; i < chkdocuments.Items.Count; i++)
                {
                    if (chkdocuments.Items[i].Selected)
                    {
                        Submitdocids = Submitdocids == "" ? chkdocuments.Items[i].Value : Submitdocids + "," + chkdocuments.Items[i].Value;
                    }
                }
            }
            //**************
            _arrPara[42].Value = Submitdocids;
            _arrPara[43].Value = Session["usertype"].ToString();
            _arrPara[44].Value = txtGuarantor.Text;
            _arrPara[45].Value = DDemptype.SelectedItem.Text;
            _arrPara[47].Value = DDGSType.SelectedValue;
            _arrPara[48].Value = txtBankDetails.Text;
            _arrPara[49].Value = txtEmpName2.Text;
            _arrPara[50].Value = txtEmpAddress2.Text;
            _arrPara[51].Value = TxtEmp2PanNo.Text;
            _arrPara[52].Value = TxtEmp2AadharNo.Text;
            _arrPara[53].Value = TxtEmp2MobileNo.Text;
            _arrPara[54].Value = DDBranchName.SelectedValue;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVE_FRM_EMP", _arrPara);
            lblErr.Visible = true;
            lblErr.Text = "";
            lblErr.Text = _arrPara[17].Value.ToString();
            Tran.Commit();
            // SqlhelperEnum.RefreshTable(AllEnums.MasterTables.Empinfo);
            //*************Save Docs
            SaveImage(Convert.ToInt32(txtEmpId.Text));
            //*************
            ClearAll();
            Fill_Grid();
            //**********

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmWeaver.aspx");
            Tran.Rollback();
            lblErr.Visible = true;
            lblErr.Text = ex.Message;
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
        //Validated();
        Store_Data();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Report();
    }
    private void Report()
    {
        Session["ReportPath"] = "Reports/EmployeeInfoNew.rpt";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[5];
            _arrPara[0] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@DepartmentId", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@Partytype", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@Location", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", SqlDbType.Int);


            _arrPara[0].Value = Session["varCompanyId"].ToString();
            _arrPara[1].Value = DDDepartment.SelectedValue == "" ? "0" : DDDepartment.SelectedValue;
            _arrPara[2].Value = DDparttype.SelectedValue;
            _arrPara[3].Value = ddlEmployeeType.SelectedValue;
            _arrPara[4].Value = 0;
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                _arrPara[4].Value = 1;
            }

            //**********         

            DataSet ds = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, "Pro_GetEmployeeMasterReportData", _arrPara);

            Session["dsFileName"] = "~\\ReportSchema\\EmployeeInfoNew.xsd";
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (DDDepartment.SelectedItem.Text == "PURCHASE")
                {
                    PurchaseVendorDetailReport(ds);
                }
                else
                {

                    Session["rptFileName"] = Session["ReportPath"];
                    Session["GetDataset"] = ds;
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            tran.Rollback();
            lblErr.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }



        //        string qry = @"SELECT Ei.EmpName,Ei.Address,Ei.PhoneNo,Ei.Mobile,Ei.TinNo,Ei.PanNo,DP.DepartmentName,Address2,Address3," + DDparttype.SelectedValue + @" as Partytype
        //                      FROM   EmpInfo EI INNER JOIN  Department DP ON EI.Departmentid=DP.DepartmentId 
        //                      Where EI.MasterCompanyId=" + Session["varCompanyId"];
        //        if (DDDepartment.SelectedIndex > 0)
        //        {
        //            qry = qry + " And Ei.DepartmentId=" + DDDepartment.SelectedValue;
        //        }
        //        if (DDparttype.SelectedIndex >= 0)
        //        {
        //            qry = qry + " And Ei.Partytype=" + DDparttype.SelectedValue;
        //        }
        //        qry = qry + " ORDER BY DP.DepartmentName ";
        //        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            Session["rptFileName"] = "~\\Reports\\WeaverNew.rpt";
        //            //Session["rptFileName"] = Session["ReportPath"];
        //            Session["GetDataset"] = ds;
        //            Session["dsFileName"] = "~\\ReportSchema\\WeaverNew.xsd";
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
    private void Validated()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql;
            if (btnSave.Text == "Update")
            {
                strsql = "select EmpName from EmpInfo Where DepartmentId=" + DDDepartment.SelectedValue + " And EmpId!='" + ViewState["id"].ToString() + "' and EmpName='" + txtEmpName.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                strsql = "select EmpName from EmpInfo Where DepartmentId=" + DDDepartment.SelectedValue + " And EmpName='" + txtEmpName.Text + "' And MasterCompanyId= " + Session["varCompanyId"];
            }
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblErr.Text = "Employee already exists............";
                txtEmpName.Text = "";
                txtEmpName.Focus();
            }
            else
            {
                lblErr.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmWeaver.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    }

    protected void BtnAddDepartment_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDDepartment, "SELECT DepartmentId,DepartmentName FROM Department Where MasterCompanyId=" + Session["varCompanyId"] + " Order By DepartmentId", true, "--Select Department--");
    }
    //protected void btnclose_Click(object sender, EventArgs e)
    //{
    //    if (Request.QueryString["ABC"] == "1")
    //    {
    //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "CloseForm();", true);
    //    }
    //    else
    //    {
    //        Response.Redirect("~/main.aspx");
    //    }
    //}

    protected void DDDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtEmpName_AutoCompleteExtender.ContextKey = DDDepartment.SelectedValue;
        ClearAll();
        Fill_Grid();

    }
    protected void DDparttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDparttype.SelectedValue == "1")
        {
            trEmployeeType.Visible = true;
        }
        else
        {
            trEmployeeType.Visible = false;
        }

        Fill_Grid();
    }
    protected void ddlEmployeeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        TRHrDetail.Visible = false;
        Tremptype.Visible = false;
        if (ddlEmployeeType.SelectedValue == "0")
        {
            if (DDDepartment.SelectedItem.Text.ToUpper() == "PRODUCTION")
            {
                Tremptype.Visible = true;
            }
            if (variable.VarPayrollDetail == "1")
            {
                TRHrDetail.Visible = true;
                UtilityModule.ConditionalComboFill(ref DDempgroup, "select GroupId,GroupName From HR_GroupMaster order by GroupName", true, "--Plz Select--");

            }

        }
        Fill_Grid();
    }
    protected void txtempcode_TextChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void refreshgrouponemployee_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDempgroup, "select GroupId,GroupName From HR_GroupMaster order by GroupName", true, "--Plz Select--");
    }
    protected void lnkdel(object sender, EventArgs e)
    {
        lblErr.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            LinkButton lnkdel = sender as LinkButton;
            GridViewRow grv = lnkdel.NamingContainer as GridViewRow;
            int empid = Convert.ToInt32(dgRaw1.DataKeys[grv.RowIndex].Value);
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@Empid", empid);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@MastercompanyId", Session["varcompanyNo"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            //**********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEEMPLOYEE", param);
            lblErr.Text = param[3].Value.ToString();
            Tran.Commit();
            Fill_Grid();

        }
        catch (Exception ex)
        {
            lblErr.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void txtEmpName_TextChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void SaveImage(int Empid)
    {

        if (fileuploadphoto.FileName != "")
        {
            string filename = Path.GetFileName(fileuploadphoto.PostedFile.FileName);
            string Folderpath = Server.MapPath("../../Hrdocs");
            //Check folder
            if (!Directory.Exists(Folderpath))
            {
                Directory.CreateDirectory(Folderpath);
            }
            //
            string targetPath = Server.MapPath("../../Hrdocs/" + Empid + "_photo.gif");

            FileInfo file = new FileInfo(targetPath);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
            }

            string img = "~\\Hrdocs\\" + Empid + "_photo.gif";
            //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
            Stream strm = fileuploadphoto.PostedFile.InputStream;
            var targetFile = targetPath;
            if (fileuploadphoto.FileName != null && fileuploadphoto.FileName != "")
            {
                GenerateThumbnails(0.3, strm, targetFile);
            }
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update empinfo Set Empphoto='" + img + "' Where empid=" + Empid + "");
            lblphotoimage.ImageUrl = img + "?" + DateTime.Now.Ticks.ToString(); ;
        }
    }
    private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
    {
        using (var image = System.Drawing.Image.FromStream(sourcePath))
        {
            var newWidth = (int)(image.Width * scaleFactor);
            var newHeight = (int)(image.Height * scaleFactor);
            var thumbnailImg = new Bitmap(newWidth, newHeight);
            var thumbGraph = Graphics.FromImage(thumbnailImg);
            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbGraph.DrawImage(image, imageRectangle);
            thumbnailImg.Save(targetPath, image.RawFormat);
        }
    }
    //protected void btnidentitycard_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        SqlParameter[] param = new SqlParameter[1];
    //        param[0] = new SqlParameter("@empid", txtEmpId.Text == "" ? "0" : txtEmpId.Text);

    //        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETEMPLOYEEIDENTITYCARD", param);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ds.Tables[0].Columns.Add("Image", typeof(System.Byte[]));

    //            foreach (DataRow dr in ds.Tables[0].Rows)
    //            {

    //                if (Convert.ToString(dr["empphoto"]) != "")
    //                {
    //                    FileInfo TheFile = new FileInfo(Server.MapPath(dr["empphoto"].ToString()));
    //                    if (TheFile.Exists)
    //                    {
    //                        string img = dr["empphoto"].ToString();
    //                        img = Server.MapPath(img);
    //                        Byte[] img_Byte = File.ReadAllBytes(img);
    //                        dr["Image"] = img_Byte;
    //                    }
    //                }
    //            }
    //            Session["rptFileName"] = "~\\Reports\\rptempidentitycard.rpt";
    //            Session["GetDataset"] = ds;
    //            Session["dsFileName"] = "~\\ReportSchema\\rptempidentitycard.xsd";
    //            StringBuilder stb = new StringBuilder();
    //            stb.Append("<script>");
    //            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
    //            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    //        }
    //        else
    //        {
    //            ScriptManager.RegisterStartupScript(Page, GetType(), "altmsg", "alert('No records found !!!')", true);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErr.Text = ex.Message;
    //    }
    //}
    protected void btnallidentitycard_Click(object sender, EventArgs e)
    {
        if(txtEmpId.Text == "" || txtEmpId.Text == "0")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altmsg", "alert('JPG can be downloaded only for single employee, Please select the single')", true);
        }
        else
        {
            createIDCard(enumCardType.jpg);
        }
        
    }

    protected void btnidentitycardpdf_Click(object sender, EventArgs e)
    {
        createIDCard(enumCardType.pdf);
    }
    private void createIDCard(enumCardType cardType)
    {
        try
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@empid", txtEmpId.Text == "" ? "0" : txtEmpId.Text);
            param[1] = new SqlParameter("@Deptid", DDDepartment.SelectedIndex > 0 ? DDDepartment.SelectedValue : "0");
            param[2] = new SqlParameter("@empcode", txtempcode_identitycard.Text);
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                param[3] = new SqlParameter("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                param[3] = new SqlParameter("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETEMPLOYEEIDENTITYCARD", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("Image", typeof(System.Byte[]));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Convert.ToString(dr["empphoto"]) != "")
                    {
                        FileInfo TheFile = new FileInfo(Server.MapPath(dr["empphoto"].ToString()));
                        if (TheFile.Exists)
                        {
                            string img = dr["empphoto"].ToString();
                            img = Server.MapPath(img);
                            Byte[] img_Byte = File.ReadAllBytes(img);
                            dr["Image"] = img_Byte;
                        }
                    }
                }
                if (Convert.ToInt32(Session["varCompanyId"]) == 16 || Convert.ToInt32(Session["varCompanyId"]) == 28)
                {
                    
                    StringBuilder stb = new StringBuilder();
                    if (cardType == enumCardType.pdf || ds.Tables[0].Rows.Count > 1)
                    {
                        
                        Session["rptFileName"] = "~\\Reports\\rptempidentitycard.rpt";
                        Session["GetDataset"] = ds;
                        Session["dsFileName"] = "~\\ReportSchema\\rptempidentitycard.xsd";
                        stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opnpdf", stb.ToString(), false);
                    }
                    else
                    {
                        Session["rptFileName"] = "~\\Reports\\rptempidentitycardBackSide.rpt";
                        string reportFile = Session["rptFileName"].ToString();
                        Session["GetDataset"] = ds;
                        Session["dsFileName"] = "~\\ReportSchema\\rptempidentitycard.xsd";
                        stb = new StringBuilder();
                        stb.Append("<script>");
                        //stb.Append("window.open('../../Viewpdftoimage.aspx','_blank', 'Front', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        stb.Append("window.open('../../Viewpdftoimage.aspx?file=" + Server.UrlEncode(reportFile) + "','_blank', 'Front', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                        Session["rptFileName"] = "~\\Reports\\rptempidentitycardFrontSide.rpt";
                        //System.Threading.Thread.Sleep(2000);
                        reportFile = Session["rptFileName"].ToString();
                        stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../Viewpdftoimage.aspx?file=" + Server.UrlEncode(reportFile) + "','_blank', 'Back', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn2", stb.ToString(), false);
                    }
                   
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\rptempidentitycard.rpt";
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\rptempidentitycard.xsd";
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altmsg", "alert('No records found !!!')", true);
            }
        }
        catch (Exception ex)
        {
            lblErr.Text = ex.Message;
        }
    }
    protected void PurchaseVendorDetailReport(DataSet ds)
    {
        lblErr.Text = "";
        try
        {           
           
            //***********
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                sht.Range("A1").Value = "Purchase Vendor Detail";
                sht.Range("A1:H1").Style.Font.FontName = "Calibri";
                sht.Range("A1:H1").Style.Font.Bold = true;
                sht.Range("A1:H1").Style.Font.FontSize = 12;
                sht.Range("A1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:H1").Merge();

                sht.Column("A").Width = 38.44;
                sht.Column("B").Width = 50.22;
                sht.Column("C").Width = 14.22;
                sht.Column("D").Width = 12.44;
                sht.Column("E").Width = 12.44;
                sht.Column("F").Width = 27.44;
                sht.Column("G").Width = 16.44;
                sht.Column("H").Width = 11.33;
               

                //*******Header
                sht.Range("A2").Value = "Vendor Name";
                sht.Range("B2").Value = "Address";
                sht.Range("C2").Value = "Contact Person";
                sht.Range("D2").Value = "Mob No";
                sht.Range("E2").Value = "Phone No";
                sht.Range("F2").Value = "E-mail";
                sht.Range("G2").Value = "Gst No";
                sht.Range("H2").Value = "Pan No"; 

                sht.Range("A2:H2").Style.Font.FontName = "Calibri";
                sht.Range("A2:H2").Style.Font.FontSize = 11;
                sht.Range("A2:H2").Style.Font.Bold = true;
                //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 3;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":H" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":H" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                    sht.Range("A" + row).Style.Alignment.SetWrapText();
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["FullAddress"]);
                    sht.Range("B" + row).Style.Alignment.SetWrapText();
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ContactPerson"]);
                    sht.Range("C" + row).Style.Alignment.SetWrapText();
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Mobile"]);
                    sht.Range("D" + row).Style.Alignment.SetWrapText();
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["PhoneNo"]);
                    sht.Range("E" + row).Style.Alignment.SetWrapText();
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["EmailAdd"]);
                    sht.Range("F" + row).Style.Alignment.SetWrapText();
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["GstNo"]);
                    sht.Range("G" + row).Style.Alignment.SetWrapText();
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["PanNo"]);
                    sht.Range("H" + row).Style.Alignment.SetWrapText();                    

                    row = row + 1;
                }

                ////*************
                //sht.Columns(1, 30).AdjustToContents();

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "H")))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("PurchaseVendorDetailreport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblErr.Text = ex.Message;
        }
    }

   

}