using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Hissab_FrmMapReceiveHissab : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            string Str = "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by CompanyName
                         Delete TEMP_HISSAB_WISE_CONSUMPTION Where Userid=" + Session["varuserid"] + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, Ds, 0, true, "--SELECT--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
                //CompanyNameSelectedIndexChanged();
            }

            FillDesignerName();

            TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");           
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            CheckForEditSelectedChanges();
            ViewState["Hissab_No"] = 0;
            //switch (Convert.ToInt16(Session["varMasterCompanyIDForERP"]))
            //{
            //    case 9: //for Hafizia
            //        TDPoOrderNo.Visible = true;
            //        TDsrno.Visible = true;
            //        TDAdditionAmt.Visible = false;
            //        TDDeductionAmt.Visible = false;
            //        TDItemDescription.Visible = true;
            //        BtnShowItemDescription.Visible = true;
            //        break;
            //    case 16: //for Champo
            //        TDPoOrderNo.Visible = true;
            //        TDsrno.Visible = true;
            //        TDAdditionAmt.Visible = true;
            //        TDDeductionAmt.Visible = true;
            //        BtnSaveAllOneTime.Visible = true;                    
            //        break;
            //    case 30: //for Samara
            //        TDPoOrderNo.Visible = true;
            //        TDsrno.Visible = true;
            //        TDAdditionAmt.Visible = true;
            //        TDDeductionAmt.Visible = true;
            //        break;
            //    case 42: //for VikramMirzapur
            //         TDPoOrderNo.Visible = true;
            //        TDsrno.Visible = true;
            //        TDAdditionAmt.Visible = false;
            //        TDDeductionAmt.Visible = false;
            //        TDItemDescription.Visible = false;
            //        TDBonusAmt.Visible = true;
            //        break;
            //    default:
            //        TDPoOrderNo.Visible = true;
            //        TDsrno.Visible = true;
            //        TDAdditionAmt.Visible = false;
            //        TDDeductionAmt.Visible = false;
            //        TDItemDescription.Visible = false;
            //        TDBonusAmt.Visible = false;
            //        break;
            //}
        }
    }

    protected void FillDesignerName()
    {
        if (ChkForEdit.Checked == true)
        {
            string str = "";
            str = @"Select distinct EI.EmpId,EI.EmpName from MAPRECEIVE_HISSAB MIM 
                    INNER JOIN EmpInfo EI ON MIM.DesignerId=EI.EmpId
                    INNER JOIN Department DP ON EI.DepartmentId=DP.DepartmentId 
                    Where MIM.CompanyId=" + DDCompanyName.SelectedValue + " and EI.DepartmentId=DP.DepartmentId  And DP.DepartmentName='DESIGNING' and isnull(Ei.blacklist,0)=0 order by EI.Empname";

            UtilityModule.ConditionalComboFill(ref DDDesignerName, str, true, "--Plz Select--");
        }
        else
        {
            string str = "";
            str = @"Select EI.EmpId,EI.EmpName from EmpInfo EI, Department DP Where EI.DepartmentId=DP.DepartmentId 
                And DP.DepartmentName='DESIGNING' And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and isnull(Ei.blacklist,0)=0 order by EI.Empname";

            UtilityModule.ConditionalComboFill(ref DDDesignerName, str, true, "--Plz Select--");
        }

        ViewState["Hissab_No"] = 0;
        ShowButton();
    }

    protected void DDDesignerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DGDetail.DataSource = null;
        //DGDetail.DataBind();
        EmployerNameSelectedIndexChanged();
        ShowButton();
    }
    protected void DDMapStencilType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DGDetail.DataSource = null;
        //DGDetail.DataBind();
        EmployerNameSelectedIndexChanged();
        ShowButton();
    }
    protected void DDIssueNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select Replace(convert(nvarchar(11),isnull(MIN(MRM.ReceiveDate),Getdate()),106),' ','-') As FromDate,Replace(convert(nvarchar(11),
                        isnull(MAX(MRM.ReceiveDate),getdate()),106),' ','-') as ToDate
                        From MAP_RECEIVEMASTER MRM(NoLock) JOIN MAP_RECEIVEDETAIL MRD(NoLock) ON MRM.RID=MRD.RID                  
                        Where MRD.ID=" + DDIssueNo.SelectedValue + " and MRM.EmpId=" + DDDesignerName.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        TxtFromDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["FromDate"]);
        TxtToDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["Todate"]);

        ShowButton();
    }
    private void EmployerNameSelectedIndexChanged()
    {
        if (DDDesignerName.SelectedIndex > 0 && ChkForEdit.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDSlipNo, "Select Distinct MapReceiveHissabNo,MapReceiveHissabNo as HissabNo1 from MAPRECEIVE_HISSAB(NoLock) Where PaymentFlag=0 And CompanyId=" + DDCompanyName.SelectedValue + " And DesignerId=" + DDDesignerName.SelectedValue + " order by HissabNo1", true, "--SELECT--");
        }
        if (DDDesignerName.SelectedIndex > 0)
        {           

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("PRO_BindMapIssueNoForHissab", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@CompanyId", DDCompanyName.SelectedValue);            
            cmd.Parameters.AddWithValue("@DesignerId", DDDesignerName.SelectedValue);
            cmd.Parameters.AddWithValue("@MapTraceType", DDMapStencilType.SelectedValue);
            cmd.Parameters.AddWithValue("@Mastercompanyid", Session["VarCompanyNo"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            UtilityModule.ConditionalComboFillWithDS(ref DDIssueNo, ds, 0, true, "--Plz Select--");


            ////UtilityModule.ConditionalComboFill(ref DDPOOrderNo, str, true, "--Plz Select--");
        }
        ViewState["Hissab_No"] = 0;
    }   
    
    private void ShowButton()
    {        
            if (DDCompanyName.SelectedIndex > 0 && DDDesignerName.SelectedIndex > 0  && ChkForEdit.Checked == false)
            {
                BtnShowData.Visible = true;
                //if (Convert.ToInt16(Session["varMasterCompanyIDForERP"]) == 16)
                //{
                //    BtnShowData.Visible = false;
                //}
            }
            else
            {
                BtnShowData.Visible = false;
            }       
        
    }
    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGDetail, "select$" + e.Row.RowIndex);

            //for (int i = 0; i < DGDetail.Columns.Count; i++)
            //{
            //    if (DGDetail.Columns[i].HeaderText == "Bonus Amt")
            //    {
            //        if (Session["varMasterCompanyIDForERP"].ToString() == "42")
            //        {
            //            DGDetail.Columns[i].Visible = true;
            //        }
            //        else
            //        {
            //            DGDetail.Columns[i].Visible = false;
            //        }
            //    }               

            //}

        }
    }
    protected void BtnShowData_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        selectall.Visible = true;
       
         TxtHissabNo.Text = "";
               
        ShowDataInGrid();
    }
    private void ShowDataInGrid()
    {
        string Str = "";
        try
        {
            if (DDCompanyName.SelectedIndex > 0 && DDDesignerName.SelectedIndex > 0)
            {
                if (ChkForEdit.Checked == false)
                {                    

                    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("PRO_GETMAPRECEIVEHISSABDATA", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300;

                    cmd.Parameters.AddWithValue("@CompanyId", DDCompanyName.SelectedValue);                    
                    cmd.Parameters.AddWithValue("@DesignerID", DDDesignerName.SelectedValue);
                    cmd.Parameters.AddWithValue("@MapTraceType", DDMapStencilType.SelectedValue);
                    cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
                    cmd.Parameters.AddWithValue("@TODate", TxtToDate.Text);
                    cmd.Parameters.AddWithValue("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
                    cmd.Parameters.AddWithValue("@IssueNoId", DDIssueNo.SelectedIndex > 0 ? DDIssueNo.SelectedValue : "0");                  
                   

                    DataSet Ds = new DataSet();
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    cmd.ExecuteNonQuery();
                    ad.Fill(Ds);

                    //if (Session["VarCompanyNo"].ToString() == "9")
                    //{
                    //    if (Ds.Tables[1].Rows.Count > 0)
                    //    {
                    //        UtilityModule.ConditionalComboFillWithDS(ref DDItemDescription, Ds, 1, true, "--SELECT--");                            
                    //    }
                    //}

                    DGDetail.DataSource = Ds.Tables[0];
                    DGDetail.DataBind();

                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        txttotalpcs.Text = Ds.Tables[0].Compute("Sum(Qty)", "").ToString();                        
                        txttotalarea.Text =Convert.ToString(Math.Round(Convert.ToDouble(Ds.Tables[0].Compute("Sum(ReceiveMapArea)", "")), 4));                       
                        txtamount.Text = Math.Round(Convert.ToDouble(Ds.Tables[0].Compute("Sum(TAmount)", "")), 2).ToString();

                    }
                    else
                    {
                        txttotalpcs.Text = "";
                        txttotalarea.Text = "";
                        txtamount.Text = "";                       
                        ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    }
                }

            }
        }
        catch (Exception ex)
        {
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
        }
    }
    private void ForCheckAllRows()
    {
        for (int i = 0; i < DGDetail.Rows.Count; i++)
        {
            GridViewRow row = DGDetail.Rows[i];
            if (Convert.ToInt32(DGDetail.Rows[i].Cells[12].Text) == 1)
            {
                ((CheckBox)row.FindControl("Chkbox")).Checked = true;
            }
            else
            {
                ((CheckBox)row.FindControl("Chkbox")).Checked = false;
            }
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SaveOther();
    }

    private void SaveOther()
    {
        int VarEditVarible = 0;
        string Str = "";
        string DetailData = "";
        //*************CHECK DATE
        if (Convert.ToDateTime(TxtFromDate.Text) > Convert.ToDateTime(TxtDate.Text) || Convert.ToDateTime(TxtToDate.Text) > Convert.ToDateTime(TxtDate.Text))
        {

            ScriptManager.RegisterStartupScript(Page, GetType(), "date", "alert('Slip Date can not be less than From and To Date.');", true);
            return;
        }
        //*************
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }       


        string status = "";
        for (int i = 0; i < DGDetail.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DGDetail.Rows[i].FindControl("Chkbox"));          

            if (Chkboxitem.Checked == true)   // Change when Updated Completed
            {
                status = "1";
            } 
        }
        if (status == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check boxes');", true);
            return;
        }

        for (int i = 0; i < DGDetail.Rows.Count; i++)
        {
            if (((CheckBox)DGDetail.Rows[i].FindControl("Chkbox")).Checked == true)
            {
                TextBox TxtRate = (TextBox)DGDetail.Rows[i].FindControl("TxtRate");
                Label lblunitid = (Label)DGDetail.Rows[i].FindControl("lblunitid");
                Label lblItemFinishedId = (Label)DGDetail.Rows[i].FindControl("lblItemFinishedId");
                Label lblReceiveMapArea = (Label)DGDetail.Rows[i].FindControl("lblReceiveMapArea");
                Label lblqty = (Label)DGDetail.Rows[i].FindControl("lblqty");
                Label lblamount = (Label)DGDetail.Rows[i].FindControl("lblamount");
                Label lblIssueNoId = (Label)DGDetail.Rows[i].FindControl("lblIssueNoId");
                Label lblMapStencilNo = (Label)DGDetail.Rows[i].FindControl("lblMapStencilNo");
                Label lblMapIssueType = (Label)DGDetail.Rows[i].FindControl("lblMapIssueType");

                if (DetailData == "")
                {
                    DetailData = TxtRate.Text + "|" + lblunitid.Text + "|" + lblItemFinishedId.Text + "|" + lblReceiveMapArea.Text + "|" + lblqty.Text + "|" + lblamount.Text + "|" + lblIssueNoId.Text + "|" + lblMapStencilNo.Text + "|" + lblMapIssueType.Text + "~";
                }
                else
                {
                    DetailData = DetailData + TxtRate.Text + "|" + lblunitid.Text + "|" + lblItemFinishedId.Text + "|" + lblReceiveMapArea.Text + "|" + lblqty.Text + "|" + lblamount.Text + "|" + lblIssueNoId.Text + "|" + lblMapStencilNo.Text + "|" + lblMapIssueType.Text + "~";
                }

            }
        }
        if (DetailData == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box');", true);
            return;
        }

        if (DetailData != "")
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("[PRO_SAVEMAPRECEIVE_HISSAB]", con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 3000;

                cmd.Parameters.AddWithValue("@CompanyId", DDCompanyName.SelectedValue);
                cmd.Parameters.AddWithValue("@DesignerId", DDDesignerName.SelectedValue);
                cmd.Parameters.AddWithValue("@MapTraceType", DDMapStencilType.SelectedValue);
                cmd.Parameters.AddWithValue("@MapIssueNo", 0);
                cmd.Parameters.AddWithValue("@MapReceiveHissabNo", SqlDbType.Int);
                cmd.Parameters["@MapReceiveHissabNo"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@HissabDate", TxtDate.Text);
                cmd.Parameters.AddWithValue("@ChallanNo", "");
                cmd.Parameters.AddWithValue("@VarHissabNo", ViewState["Hissab_No"]);
                cmd.Parameters.AddWithValue("@UnitId", 0);

                if (ChkForEdit.Checked == true)
                {
                    VarEditVarible = VarEditVarible + 1;
                    cmd.Parameters.AddWithValue("@MRHID", VarEditVarible);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MRHID", 0);
                }
                cmd.Parameters.AddWithValue("@DetailData", DetailData);
                cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
                cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);
                cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
                cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyNo"]);      
                cmd.Parameters.Add(new SqlParameter("@MSG", SqlDbType.VarChar, 300));
                cmd.Parameters["@MSG"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                if (cmd.Parameters["@MSG"].Value.ToString() != "")
                {
                    lblMessage.Text = "";
                    lblMessage.Visible = true;
                    lblMessage.Text = cmd.Parameters["@MSG"].Value.ToString();
                    return;
                }

                ViewState["Hissab_No"] = cmd.Parameters["@MapReceiveHissabNo"].Value.ToString();
                Tran.Commit();
                lblMessage.Visible = true;
                lblMessage.Text = "Data Inserted Successfully !";
                ShowDataInGrid();
                TxtHissabNo.Text = ViewState["Hissab_No"].ToString();
                ChkForAllSelect.Checked = false;                
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void ChkForEdit_CheckedChanged(object sender, EventArgs e)
    {
        EditSelectedChange();
    }
    private void EditSelectedChange()
    {
        CheckForEditSelectedChanges();
        FillDesignerName();
    }
    private void CheckForEditSelectedChanges()
    {
        if (ChkForEdit.Checked == true)
        {
            TDSlipNoForEdit.Visible = true;
            TDDDSlipNo.Visible = true;
            BtnDelete.Visible = true;
            TDPoOrderNo.Visible = false;           
        }
        else
        {
            TDPoOrderNo.Visible = true;
            BtnDelete.Visible = false;
            TDSlipNoForEdit.Visible = false;
            TDDDSlipNo.Visible = false;
            TxtSlipNo.Text = "";
            DDSlipNo.Items.Clear();
            if (DDDesignerName.Items.Count > 0)
            {
                DDDesignerName.SelectedIndex = 0;
            }         

        }        
        //CompanyNameSelectedIndexChanged();
    }
    protected void TxtSlipNo_TextChanged(object sender, EventArgs e)
    {
        lblMessage.Visible = false;
        if (TxtSlipNo.Text != "")
        {        

            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
                @"Select CompanyId,DesignerId,MapIssueNo,MapReceiveHissabNo,MapTraceType,replace(convert(varchar(11),HissabDate,106), ' ','-') as Date,ChallanNo 
                    From MAPRECEIVE_HISSAB Where PaymentFlag=0 And CompanyID = " + DDCompanyName.SelectedValue + " And MapReceiveHissabNo=" + TxtSlipNo.Text + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                //CompanyId,ProcessID,EmpId,ProcessOrderNo,HissabNo,Date,Finishedid,StockNo,Qty,Area,Rate,Amount,Weight,Penality,PRemarks,ChallanNo,UnitId                               

                DDDesignerName.SelectedValue = Ds.Tables[0].Rows[0]["DesignerId"].ToString();
                FillDesignerName();
                DDMapStencilType.SelectedValue = Ds.Tables[0].Rows[0]["MapTraceType"].ToString();
                TxtHissabNo.Text = "";
                DDSlipNo.SelectedValue = Ds.Tables[0].Rows[0]["MapReceiveHissabNo"].ToString();
                EmployerNameSelectedIndexChanged();
                SlipNoSelectedChanges();
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Pls Enter Proper Slip No";
            }
        }
        else
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Pls. Enter Proper Slip No";
        }
    }
    protected void DDSlipNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        SlipNoSelectedChanges();
    }
    private void SlipNoSelectedChanges()
    {
        ViewState["Hissab_No"] = DDSlipNo.SelectedValue;
        if (DDSlipNo.SelectedIndex > 0)
        {
            TxtHissabNo.Text = DDSlipNo.Text;
            TxtDate.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select IsNull(replace(convert(varchar(11),HissabDate,106), ' ','-'),'') as Date From MAPRECEIVE_HISSAB Where MapReceiveHissabNo=" + DDSlipNo.SelectedValue + "").ToString();
        }
        ShowDataInGrid();
    }
    protected void BtnPriview_Click(object sender, EventArgs e)
    {

        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@CompanyId", DDCompanyName.SelectedValue);
        param[1] = new SqlParameter("@MapReceiveHissabNo", ViewState["Hissab_No"]);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_MapReceiveHissabReport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptMapReceiveHissabSummary.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptMapReceiveHissabSummary.xsd";
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
    protected void DGDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGDetail.PageIndex = e.NewPageIndex;
        ShowDataInGrid();
    }
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@MapReceiveHissab_No", ViewState["Hissab_No"]);
            param[1] = new SqlParameter("@DesignerID", DDDesignerName.SelectedValue);
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEMAPRECEIVEHISSAB", param);
            if (param[4].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + param[4].Value.ToString() + "');", true);
                Tran.Rollback();
            }
            else
            {
                Tran.Commit();
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Slip successfully deleted!');", true);
                ChkForEdit.Checked = false;
                BtnDelete.Visible = false;
                TDSlipNoForEdit.Visible = false;
                TDDDSlipNo.Visible = false;
                TxtSlipNo.Text = "";
                TxtHissabNo.Text = "";
                DDSlipNo.Items.Clear();
                ViewState["Hissab_No"] = 0;
                ShowButton();
            }           


        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DDPOOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
//        string str = @"select Replace(convert(nvarchar(11),isnull(MIN(PRM.ReceiveDate),Getdate()),106),' ','-') As FromDate,Replace(convert(nvarchar(11),
//                    isnull(MAX(PRM.ReceiveDate),getdate()),106),' ','-') as ToDate
//                    From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PRM(NoLock) inner Join PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PRD(NoLock) on PRM.Process_Rec_Id=PRD.Process_Rec_Id
//                    Where PRD.IssueOrderId=" + DDPOOrderNo.SelectedValue + " and PRM.EmpId=" + DDEmployerName.SelectedValue;
//        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
//        TxtFromDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["FromDate"]);
//        TxtToDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["Todate"]);
//        FillSrno();

//        //if (Convert.ToInt16(Session["varMasterCompanyIDForERP"]) == 16)
//        //{
//        //    BtnShowData.Visible = true;
//        //}
    }
    protected void btnprintvoucher_Click(object sender, EventArgs e)
    {      

        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@CompanyId", DDCompanyName.SelectedValue);
        param[1] = new SqlParameter("@MapReceiveHissabNo", ViewState["Hissab_No"]);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_MapReceiveHissabVoucherReport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptVoucherMapReceiveHissab.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptVoucherMapReceiveHissab.xsd";
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
    protected void TxtToDate_TextChanged(object sender, EventArgs e)
    {
        TxtDate.Text = TxtToDate.Text;
    }
    
}