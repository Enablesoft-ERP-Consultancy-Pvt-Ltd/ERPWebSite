using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
public partial class Masters_Repier_RapierOrderMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            try
            {
                DataSet ds = new DataSet();
                string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                    From Companyinfo CI(Nolock) 
                    JOIN Company_Authentication CA(Nolock) ON CI.CompanyId = CA.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                    Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By CompanyName 
                    Select PROCESS_NAME_ID, PROCESS_NAME From Process_Name_Master (Nolock) Where MasterCompanyid = " + Session["varCompanyId"] + @" And Process_Name = 'RAIPER MAKING' Order By PROCESS_NAME 
                    Select UnitId, Unitname From Unit(Nolock) Order By UnitID ";

                ds = SqlHelper.ExecuteDataset(str);

                UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--Select--");

                if (DDCompanyName.Items.Count > 0)
                {
                    DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                    DDCompanyName.Enabled = false;
                }

                UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Select--");
                if (DDProcessName.Items.Count > 0)
                {
                    DDProcessName.SelectedIndex = 1;
                    DDProcessNameSelectedIndex();
                }                

                UtilityModule.ConditionalComboFillWithDS(ref DDunit, ds, 2, false, "");
                lablechange();
                TxtAssignDate.Text = (DateTime.Now).ToString("dd-MMM-yyyy");
                TxtRequiredDate.Text = (DateTime.Now).ToString("dd-MMM-yyyy");
                ViewState["ID"] = "0";
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Rapier/RapierOrderMasterPageLoad.aspx");
            }
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
    }
    protected void DDProcessName_SelectedIndexChanged1(object sender, EventArgs e)
    {
        DDProcessNameSelectedIndex();
    }
    protected void DDProcessNameSelectedIndex()
    {
        string str = @"Select Distinct EI.EmpId, EI.EmpName 
            From EmpInfo EI(Nolock) 
            JOIN EmpProcess EP(Nolock) ON EP.Empid = EI.Empid And EP.Processid = " + DDProcessName.SelectedValue;
 
        if(ChkForEdit .Checked ==true)
        {
            str = str + @" JOIN RapierOrderMaster ROM(Nolock) ON ROM.EmpID = EI.EmpID ";
        }
        str = str + @" Where EI.MasterCompanyId = " + Session["varCompanyId"] + @" And IsNull(EI.blacklist, 0) = 0
        Order By EI.EmpName";

        str = str + @" SELECT ICM.CATEGORY_ID, ICM.CATEGORY_NAME 
            FROM ITEM_CATEGORY_MASTER ICM(Nolock) 
            JOIN CategorySeparate CS(Nolock) ON CS.Categoryid = ICM.CATEGORY_ID And CS.ID = 0 
            JOIN UserRights_Category URC(Nolock) ON URC.CategoryId = ICM.CATEGORY_ID And URC.UserId = " + Session["varuserId"] + @" 
            Where ICM.MasterCompanyID = " + Session["varCompanyId"];

        DataSet ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 0, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDCategoryName, ds, 1, true, "--Select--");
        ViewState["ID"] = "0";
    }
    protected void DDCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategorySelectedChange();
    }
    private void FillCategorySelectedChange()
    {
        string Str;
        Str = @"SELECT IM.ITEM_ID, IM.ITEM_NAME 
            From ITEM_MASTER IM(Nolock) 
            Where MasterCompanyid = " + Session["varCompanyId"] + " And CATEGORY_ID = " + DDCategoryName.SelectedValue;

        UtilityModule.ConditionalComboFill(ref DDItemName, Str, true, "---Select---");
        if (DDItemName.Items.Count > 0)
        {
            DDItemName.SelectedIndex = 1;
            Fill_Description();
        }
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Description();
    }
    private void Fill_Description()
    {
        string STR;
        STR = @"SELECT VF.ITEM_FINISHED_ID, Replace(VF.QualityName + '  ' + VF.DesignName + '  ' + VF.ColorName + '  ' + VF.ShapeName + '  ' + 
                    Case When " + DDunit.SelectedValue + " = 1 Then VF.SizeMtr Else Case When " + DDunit.SelectedValue + @" = 6 Then VF.SizeInch Else SizeFt End End  + '  ' + VF.ShadeColorName, '   ', '') Description
                From V_FinishedItemDetail VF(Nolock) 
                Where VF.MasterCompanyid = " + Session["varCompanyId"] + " And VF.CATEGORY_ID = " + DDCategoryName.SelectedValue + " And VF.ITEM_ID = " + DDItemName.SelectedValue + @" 
                Order By Replace(VF.QualityName + '  ' + VF.DesignName + '  ' + VF.ColorName + '  ' + VF.ShapeName + '  ' + 
                    Case When " + DDunit.SelectedValue + " = 1 Then VF.SizeMtr Else Case When " + DDunit.SelectedValue + @" = 6 Then VF.SizeInch Else SizeFt End End  + '  ' + VF.ShadeColorName, '   ', '')";

        UtilityModule.ConditionalComboFill(ref DDDescription, STR, true, "--Select--");
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[15];
            _arrpara[0] = new SqlParameter("@ID", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@CompanyID", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@ProcessID", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@Empid", SqlDbType.Int);
            _arrpara[4] = new SqlParameter("@ChallanNo", SqlDbType.NVarChar);
            _arrpara[5] = new SqlParameter("@OrderDate", SqlDbType.DateTime);
            _arrpara[6] = new SqlParameter("@RequiredDate", SqlDbType.DateTime);
            _arrpara[7] = new SqlParameter("@Remark", SqlDbType.NVarChar);
            _arrpara[8] = new SqlParameter("@UnitId", SqlDbType.Int);
            _arrpara[9] = new SqlParameter("@Item_Finished_id", SqlDbType.Int);
            _arrpara[10] = new SqlParameter("@Qty", SqlDbType.Float);
            _arrpara[11] = new SqlParameter("@Rate", SqlDbType.Float);
            _arrpara[12] = new SqlParameter("@UserID", SqlDbType.Int);
            _arrpara[13] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            _arrpara[14] = new SqlParameter("@Msg", SqlDbType.NVarChar, 250);

            _arrpara[0].Direction = ParameterDirection.InputOutput;
            _arrpara[0].Value = ViewState["ID"];
            _arrpara[1].Value = DDCompanyName.SelectedValue;
            _arrpara[2].Value = DDProcessName.SelectedValue; 
            _arrpara[3].Value = DDEmployeeName.SelectedValue;
            _arrpara[4].Value = TxtChallanNo.Text;
            _arrpara[4].Direction = ParameterDirection.InputOutput;
            _arrpara[5].Value = TxtAssignDate.Text;
            _arrpara[6].Value = TxtRequiredDate.Text;
            _arrpara[7].Value = TxtRemarks.Text;
            _arrpara[8].Value = DDunit.SelectedValue;
            _arrpara[9].Value = DDDescription.SelectedValue;
            _arrpara[10].Value = TxtQtyRequired.Text;
            _arrpara[11].Value = TxtRate.Text;
            _arrpara[12].Value = Session["varuserid"];
            _arrpara[13].Value = Session["varCompanyId"];
            _arrpara[14].Direction = ParameterDirection.InputOutput;
            _arrpara[14].Value = "";

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_RapierOrderMaster", _arrpara);
            Tran.Commit();
            if (_arrpara[14].Value.ToString() == "Successfully data saved")
            {
                ViewState["ID"] = _arrpara[0].Value;
                TxtChallanNo.Text = _arrpara[4].Value.ToString();
                DDDescription.SelectedIndex = 0;
                TxtQtyRequired.Text = "";
                TxtRate.Text = "";
                Fill_Grid();
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert(' " + _arrpara[14].Value + " ');", true);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Rapier/RapierOrderMaster/PRO_RapierOrderMaster");
            Tran.Rollback();
            ViewState["ID"] = "0";
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void Fill_Grid()
    {
        DGOrderdetail.DataSource = GetDetail();
        DGOrderdetail.DataBind();
    }
    //*********************************************************FIll Gride*********************************************************
    private DataSet GetDetail()
    {
        DataSet DS = null;
        string sqlstr = @"Select a.ID, b.DetailID, VF.CATEGORY_NAME Category, VF.ITEM_NAME Item, Replace(VF.QualityName + '  ' + VF.DesignName + '  ' + VF.ColorName + '  ' + VF.ShapeName + '  ' + 
                Case When b.UnitID = 1 Then VF.SizeMtr Else Case When b.UnitID = 6 Then VF.SizeInch Else SizeFt End End  + '  ' + VF.ShadeColorName, '   ', '') [Description], Qty, Rate 
                From RapierOrderMaster a(nolock)
                JOIN RapierOrderDetail b(Nolock) ON b.ID = a.ID 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.Item_Finished_ID 
                Where a.ID = " + ViewState["ID"];
        try
        {
            DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sqlstr);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Rapier/RapierOrderMaster/FillGrid()");
        }
        return DS;
    }

    protected void DGOrderdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderdetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@ID", ViewState["ID"]);
        param[1] = new SqlParameter("@UserID", Session["varCompanyId"]);
        param[2] = new SqlParameter("@Mastercompanyid", Session["varCompanyId"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetRapierOrderMasterForReport", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptRapierOrderMaster.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRapierOrderMaster.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }
    }
    protected void DGOrderdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarDetail_Id = Convert.ToInt32(DGOrderdetail.DataKeys[e.RowIndex].Value);
            
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@ID", ViewState["ID"]);
            param[1] = new SqlParameter("@DetailID", VarDetail_Id);
            param[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@Userid", Session["varuserid"]);
            param[4] = new SqlParameter("@MasterCompanyid", Session["varcompanyid"]);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_RapierOrderMasterDelete", param);
            Tran.Commit();
            Fill_Grid();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Rapier/RapierOrderMaster/GridDataDelete()");
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillChallanNo();
    }

    private void FillChallanNo()
    {
        ViewState["ID"] = "0";
        if (DDCompanyName.SelectedIndex > 0 && DDProcessName.SelectedIndex > 0 && DDEmployeeName.SelectedIndex > 0)
        {
            string STR;
            STR = @"Select ID, ChallanNo 
                From RapierOrderMaster(Nolock) 
                Where MasterCompanyID = " + Session["varCompanyId"] + " And CompanyID = " + DDCompanyName.SelectedValue + " And ProcessID = " + DDProcessName.SelectedValue + " And EmpID = " + DDEmployeeName.SelectedValue;

            UtilityModule.ConditionalComboFill(ref DDChallanNo, STR, true, "--Select--");

            ViewState["ID"] = DDChallanNo.SelectedValue;
        }
    }

    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = @"Select ID, ChallanNo, REPLACE(CONVERT(NVARCHAR(11), OrderDate, 106), ' ', '-') OrderDate, REPLACE(CONVERT(NVARCHAR(11), RequiredDate, 106), ' ', '-') RequiredDate, Remark 
                    From RapierOrderMaster(Nolock)
                    Where ID = " + DDChallanNo.SelectedValue;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            TxtChallanNo.Text = Ds.Tables[0].Rows[0]["ChallanNo"].ToString();
            TxtAssignDate.Text = Ds.Tables[0].Rows[0]["OrderDate"].ToString();
            TxtRequiredDate.Text = Ds.Tables[0].Rows[0]["RequiredDate"].ToString();
            TxtRemarks.Text = Ds.Tables[0].Rows[0]["Remark"].ToString();
            ViewState["ID"] = Ds.Tables[0].Rows[0]["ID"].ToString();
            Fill_Grid();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('No record found');", true);
        }
    }
    protected void ChkForEdit_CheckedChanged(object sender, EventArgs e)
    {
        TDDDChallanNo.Visible = false;
        if (DDProcessName.Items.Count > 0)
        {
            DDProcessNameSelectedIndex();
        }
        if (ChkForEdit.Checked == true)
        {
            TDDDChallanNo.Visible = true;
            FillChallanNo();
        }
    }
}