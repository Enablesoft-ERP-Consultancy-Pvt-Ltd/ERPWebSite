using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_PunchCardIndent_FrmPunchCardIndentReceive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName 
                from Companyinfo CI,Company_Authentication CA 
                Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName 
                Select Id,Name From PunchCardIndentType(Nolock) order by Id ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                FillCustomercode();
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDPunchCardIndentType, ds, 1, false, "");

            if (DDPunchCardIndentType.Items.Count > 0)
            {
                DDPunchCardIndentType.SelectedIndex = 0;
            }

            txtReceiveDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            //txtRequiredDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
            }

            hnReceiveid.Value = "0";
            FillCustomerOrderNo();

            //switch (Session["varMasterCompanyIDForERP"].ToString())
            //{
            //    case "30":
            //        TDCustomerCode.Visible = false;
            //        FillCustomerOrderNo();
            //        break;
            //    default:
            //        TDCustomerCode.Visible = true;
            //        break;
            //}
        }
    }
    protected void FillissueGrid()
    {
        string str = @"select MIM.Id,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,
                 CASE WHEN OD.OrderUnitId = 6 THEN VF.SizeInch else Case When OD.OrderUnitId = 1 THEN VF.SizeMtr ELSE VF.SizeFt END End As Size,
                --CASE WHEN MIM.UnitID = 2 THEN VF.SIZEFT ELSE VF.SIZEMtr END As Size, 
                MID.DetailId,MID.OrderId,MID.ItemFinishedID,MIM.CompanyId,MIM.CustomerId,MIM.EmpId,MIM.PunchCardIndentType,MIM.ChallanNo,
                Replace(CONVERT(nvarchar(11),MIM.AssignDate,106),' ','-') as AssignDate,Replace(CONVERT(nvarchar(11),MIM.RequiredDate,106),' ','-') as RequiredDate,
                MID.PerSetQty,MId.NoOfSet,MId.TotalSetQty,
                isnull((select isnull(sum(MRD.ReceiveNoOfSet),0) from PUNCHCARDINDENT_RECEIVEMASTER MRM INNER JOIN PUNCHCARDINDENT_RECEIVEDETAIL MRD ON MRM.RID=MRD.RID
	                Where MRD.IssueID=MIM.ID and MRM.CompanyId=MIM.CompanyId and MRD.ItemFinishedId=MID.ItemFinishedId and MRD.OrderID=MID.OrderID and MRM.PunchCardIndentType=MIM.PunchCardIndentType),0) as PreReceiveQty,
                --dbo.[GET_PUNCHCARDRECEIVEQTY](MIM.ID, MID.ItemFinishedId,MID.OrderID,MIM.PunchCardIndentType," + Session["varMasterCompanyIDForERP"] + @") as PreReceiveQty,
                MIM.Remarks,U.UnitName
                From PUNCHCARDINDENT_ORDERMASTER MIM 
                INNER JOIN PUNCHCARDINDENT_ORDERDETAIL MID ON MIM.Id=MID.MasterId 
                INNER JOIN V_FinishedItemDetail VF ON MID.ItemFinishedId=VF.Item_Finished_Id
                INNER JOIN OrderDetail OD ON MID.OrderId=OD.OrderID and MID.ItemFinishedId=OD.Item_Finished_id
                INNER JOIN Unit U ON MIM.UnitID=U.UnitId
                where MIM.CompanyId=" + DDCompany.SelectedValue + " ";

        if (txtIssueNo.Text != "")
        {
            str = str + " and MIM.ChallanNo='" + txtIssueNo.Text + "'";
        }
        else if (DDissueno.SelectedIndex > 0)
        {
            str = str + " and MIM.Id=" + DDissueno.SelectedValue + "";
        }
        else
        {
            str = str + " and MIM.ChallanNo=''";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DG.DataSource = ds.Tables[0];
            DG.DataBind();

            txtIssueNo.Text = ds.Tables[0].Rows[0]["ChallanNo"].ToString();
            //DDCompany.SelectedValue = ds.Tables[0].Rows[0]["CompanyId"].ToString();
            FillCustomercode();
            DDPunchCardIndentType.SelectedValue = ds.Tables[0].Rows[0]["PunchCardIndentType"].ToString();
            DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["CustomerId"].ToString();
            FillCustomerOrderNo();
            DDCustomerOrderNo.SelectedValue = ds.Tables[0].Rows[0]["OrderId"].ToString();
            FillDesignerName();
            DDDesignerName.SelectedValue = ds.Tables[0].Rows[0]["EmpId"].ToString();            
            FillIssueNo();
            DDissueno.SelectedValue = ds.Tables[0].Rows[0]["ID"].ToString();
            TxtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
            lblPunchCardIssueUnit.Text = ds.Tables[0].Rows[0]["UnitName"].ToString();            
        }
        else
        {
            txtIssueNo.Text = "";
            DG.DataSource = null;
            DG.DataBind();
        }

    }
    protected void FillIssueNo()
    {

        string str = @"select Distinct MIM.ID,MIM.ChallanNo from PUNCHCARDINDENT_ORDERMASTER MIM inner join PUNCHCARDINDENT_ORDERDETAIL MID on MIM.ID=MID.MasterID
                       Where MIM.CompanyId=" + DDCompany.SelectedValue + " and MIM.CustomerId=" + DDCustomerCode.SelectedValue + " and  MID.OrderID=" + DDCustomerOrderNo.SelectedValue + @" 
                       and MIM.EmpID=" + DDDesignerName.SelectedValue + " and MIM.PunchCardIndentType=" + DDPunchCardIndentType.SelectedValue + " ";

        UtilityModule.ConditionalComboFill(ref DDissueno, str, true, "--Plz Select--");

    }
    protected void txtIssueNo_TextChanged(object sender, EventArgs e)
    {

        FillissueGrid();
        if (chkEdit.Checked == true)
        {
            DDissueno_SelectedIndexChanged(sender, new EventArgs());
        }
    }
    protected void FillCustomercode()
    {
        string str = @"select Distinct CU.Customerid,Cu.customercode from OrderDetail OD  
                       inner join  OrderMaster OM on  OM.OrderId=OD.OrderId
                       inner Join Customerinfo cu on om.customerid=cu.customerid
                       Where OM.CompanyId=" + DDCompany.SelectedValue + " and Status=0 order by customercode";

        UtilityModule.ConditionalComboFill(ref DDCustomerCode, str, true, "--Plz Select--");
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"].ToString() == "30")
        {
            FillCustomerOrderNo();
        }
        else
        {
            FillCustomercode();
        }
        
    }
    protected void FillCustomerOrderNo()
    {
        string str = "";
        str = @"select Distinct OM.OrderId,OM.CustomerOrderNo+'/ '+OM.LocalOrder as OrderNo from OrderDetail OD  inner join  OrderMaster OM on  OM.OrderId=OD.OrderId 
                    Where OM.Status=0 and OM.CompanyId=" + DDCompany.SelectedValue;
        if (DDCustomerCode.SelectedIndex > 0)
        {
            str = str + " and OM.CustomerId=" + DDCustomerCode.SelectedValue;
        }
        str = str + " order by OM.OrderId";

        UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, str, true, "--Plz Select--");
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCustomerOrderNo();
        //FillDesignerName();

        DG.DataSource = null;
        DG.DataBind();

    }
    protected void FillDesignerName()
    {
        string str = "";
        if (chkEdit.Checked == true)
        {
            str = @"Select distinct EI.EmpId,EI.EmpName from PUNCHCARDINDENT_ORDERMASTER MIM inner join PUNCHCARDINDENT_ORDERDETAIL MID on MIM.ID=MID.MasterID 
                    INNER JOIN EmpInfo EI ON MIM.EmpId=EI.EmpId
                    INNER JOIN Department DP ON EI.DepartmentId=DP.DepartmentId 
                    Where MID.OrderId=" + DDCustomerOrderNo.SelectedValue + " and EI.DepartmentId=DP.DepartmentId  and isnull(Ei.blacklist,0)=0";
        }
        else
        {
            str = @"Select EI.EmpId,EI.EmpName from EmpInfo EI, Department DP Where EI.DepartmentId=DP.DepartmentId 
                And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and isnull(Ei.blacklist,0)=0 ";

        }
        str = str + " And DP.DepartmentName='DESIGNING'";
        
        //if (DDMapStencilType.SelectedValue == "3")
        //{
        //    str = str + " And DP.DepartmentName='PRODUCTION'";
        //}
        //else
        //{
        //    str = str + " And DP.DepartmentName='DESIGNING'";
        //}

        str = str + " order by EI.Empname";
        UtilityModule.ConditionalComboFill(ref DDDesignerName, str, true, "--Plz Select--");
    }
    protected void DDCustomerOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDesignerName();
        FillissueGrid();
        //Fillgrid();

        //hnissueid.Value = "0";

        //gvdetail.DataSource = null;
        //gvdetail.DataBind();

        DDDesignerName.SelectedIndex = -1;
        DDissueno.Items.Clear();
        //txtissueno.Text = "";
    }
    protected void DDDesignerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillissueGrid();
        FillIssueNo();
    }
    //protected void DDMapStencilType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    hnReceiveid.Value = "0";
    //    FillIssueNo();
    //}
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }

    private void CHECKVALIDCONTROL()
    {
        lblmessage.Text = "";
        //if (UtilityModule.VALIDDROPDOWNLIST(DDCompany) == false)
        //{
        //    goto a;
        //}   
       
        //if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerCode) == false)
        //{
        //    goto a;
        //}
        if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerOrderNo) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDDesignerName) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        lblmessage.Visible = true;
        UtilityModule.SHOWMSG(lblmessage);
    B: ;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();

        if (lblmessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            for (int i = 0; i < DG.Rows.Count; i++)
            {
                CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
                TextBox txtReceiveNoOfSet = ((TextBox)DG.Rows[i].FindControl("txtReceiveNoOfSet"));
                //TextBox txtNoOfSet = ((TextBox)DG.Rows[i].FindControl("txtNoOfSet"));
                Label lblItemFinishedId = ((Label)DG.Rows[i].FindControl("lblItemFinishedId"));
                Label lblOrderId = ((Label)DG.Rows[i].FindControl("lblOrderId"));
                Label lblId = ((Label)DG.Rows[i].FindControl("lblId"));
                Label lblDetailId = ((Label)DG.Rows[i].FindControl("lblDetailId"));

                //if (Chkboxitem.Checked == false)   // Change when Updated Completed
                //{
                //    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please Select Checkbox');", true);               
                //    return;
                //}

                if ((txtReceiveNoOfSet.Text == "") && Chkboxitem.Checked == true)   // Change when Updated Completed
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Receive NoOf Set Qty can not be blank');", true);
                    txtReceiveNoOfSet.Focus();
                    return;
                }
                if (Chkboxitem.Checked == true && (Convert.ToDecimal(txtReceiveNoOfSet.Text) <= 0))   // Change when Updated Completed
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Receive NoOf Set Qty always greater then zero');", true);
                    txtReceiveNoOfSet.Focus();
                    return;
                }
                //if (Convert.ToDecimal(txtReceiveQty.Text == "" ? "0" : txtReceiveQty.Text) > Convert.ToDecimal(lblBalToRecQty.Text) && Chkboxitem.Checked == true)   // Change when Updated Completed
                //{
                //    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Receive qty can not be greater than balance qty');", true);
                //    txtReceiveQty.Text = "";
                //    txtReceiveQty.Focus();
                //    return;
                //}
            }

            string Strdetail = "";
            for (int i = 0; i < DG.Rows.Count; i++)
            {
                CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
                TextBox txtReceiveNoOfSet = ((TextBox)DG.Rows[i].FindControl("txtReceiveNoOfSet"));
                //Label lblBalToRecQty = ((Label)DG.Rows[i].FindControl("lblBalToRecQty"));
                Label lblItemFinishedId = ((Label)DG.Rows[i].FindControl("lblItemFinishedId"));
                Label lblOrderId = ((Label)DG.Rows[i].FindControl("lblOrderId"));
                Label lblId = ((Label)DG.Rows[i].FindControl("lblId"));
                Label lblDetailId = ((Label)DG.Rows[i].FindControl("lblDetailId"));

                if (Chkboxitem.Checked == true && (txtReceiveNoOfSet.Text != "") && DDCompany.SelectedIndex >= 0)
                {
                    Strdetail = Strdetail + txtReceiveNoOfSet.Text + '|' + lblItemFinishedId.Text + '|' + lblOrderId.Text + '|' + lblId.Text + '|' + lblDetailId.Text + '~';
                }
            }

            if (Strdetail != "")
            {
                SqlTransaction Tran = con.BeginTransaction();
                try
                {
                    SqlParameter[] param = new SqlParameter[12];
                    param[0] = new SqlParameter("@RID", SqlDbType.Int);
                    param[0].Value = hnReceiveid.Value;
                    param[0].Direction = ParameterDirection.InputOutput;
                    param[1] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
                    param[2] = new SqlParameter("@CustomerId", DDCustomerCode.SelectedValue);
                    param[3] = new SqlParameter("@EmpId", DDDesignerName.SelectedValue);
                    param[4] = new SqlParameter("@ReceiveDate", txtReceiveDate.Text);
                    param[5] = new SqlParameter("@userid", Session["varuserid"]);
                    param[6] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
                    param[7] = new SqlParameter("@StringDetail", Strdetail);
                    param[8] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                    param[8].Direction = ParameterDirection.Output;
                    param[9] = new SqlParameter("@Remarks", TxtRemarks.Text);
                    param[10] = new SqlParameter("@CHALLANNO", SqlDbType.VarChar, 50);
                    param[10].Value = "";
                    param[10].Direction = ParameterDirection.InputOutput;
                    param[11] = new SqlParameter("@PunchCardIndentType", SqlDbType.TinyInt);
                    param[11].Value = DDPunchCardIndentType.SelectedValue;


                    ///**********
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SavePunchCardIndent_ReceiveOrder", param);
                    //*******************
                    //ViewState["reportid"] = param[0].Value.ToString();
                    txtReceiveNo.Text = param[10].Value.ToString();
                    hnReceiveid.Value = param[0].Value.ToString();
                    Tran.Commit();
                    if (param[8].Value.ToString() != "")
                    {
                        lblmessage.Text = param[8].Value.ToString();
                    }
                    else
                    {
                        lblmessage.Text = "DATA SAVED SUCCESSFULLY.";
                        FillissueGrid();
                        FillReceiveGrid();
                    }

                }
                catch (Exception ex)
                {
                    Tran.Rollback();
                    lblmessage.Text = ex.Message;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
            }
        }
    }
    protected void FillReceiveGrid()
    {
        string str = @"select MRM.RId,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,
                        CASE WHEN OD.OrderUnitId = 6 THEN VF.SizeInch else Case When OD.OrderUnitId = 1 THEN VF.SizeMtr ELSE VF.SizeFt END End As Size,
                        --CASE WHEN MRD.UnitID = 2 THEN VF.SIZEFT ELSE VF.SIZEMtr END As Size, 
                        MRD.RDetailId,MRD.OrderId,MRD.ItemFinishedID,MRM.CompanyId,MRM.CustomerId,MRM.EmpId,MRM.PunchCardIndentType,MRM.ChallanNo,
                        Replace(CONVERT(nvarchar(11),MRM.ReceiveDate,106),' ','-') as ReceiveDate,
                        MRD.ReceivePerSetQty,MRD.ReceiveNoOfSet,MRD.TotalReceiveQty,MRD.IssueID,MRD.IssueDetailID, MRM.Remarks                       
                        From PUNCHCARDINDENT_RECEIVEMASTER MRM INNER JOIN PUNCHCARDINDENT_RECEIVEDETAIL MRD ON MRM.RId=MRD.RID
                        INNER JOIN V_FinishedItemDetail VF ON MRD.ItemFinishedId=VF.Item_Finished_Id
                        INNER JOIN OrderDetail OD ON MRD.OrderId=OD.OrderID and MRD.ItemFinishedId=OD.Item_Finished_id                        
                        Where MRM.CompanyId = " + DDCompany.SelectedValue + " And MRM.RId = " + hnReceiveid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvdetail.DataSource = ds.Tables[0];
            gvdetail.DataBind();
        }
        else
        {
            gvdetail.DataSource = null;
            gvdetail.DataBind();
        }
        if (chkEdit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtReceiveNo.Text = ds.Tables[0].Rows[0]["ChallanNo"].ToString();
                txtReceiveDate.Text = ds.Tables[0].Rows[0]["ReceiveDate"].ToString();
                //DDCompany.SelectedValue = ds.Tables[0].Rows[0]["CompanyId"].ToString();
                DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["CustomerId"].ToString();
                DDCustomerOrderNo.SelectedValue = ds.Tables[0].Rows[0]["OrderId"].ToString();
                DDDesignerName.SelectedValue = ds.Tables[0].Rows[0]["EmpId"].ToString();
                TxtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
            }
            else
            {
                TxtRemarks.Text = "";
                txtReceiveNo.Text = "";
                txtReceiveDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@RID", hnReceiveid.Value);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_PunchCardIndentOrderReceiveReport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptPunchCardIndentReceiveOrderReport.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptPunchCardIndentReceiveOrderReport.xsd";
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
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
        if (chkEdit.Checked == true)
        {
            TDReceiveNo.Visible = true;
        }
        else
        {
            TDReceiveNo.Visible = false;
            DDReceiveNo.Items.Clear();
        }
    }
    protected void FillReceiveNo()
    {
        string str = @"select Distinct MRM.RID,MRM.ChallanNo from PUNCHCARDINDENT_RECEIVEMASTER MRM inner join PUNCHCARDINDENT_RECEIVEDETAIL MRD on MRM.RID=MRD.RID
                       Where MRD.IssueID =" + DDissueno.SelectedValue + " ";

        UtilityModule.ConditionalComboFill(ref DDReceiveNo, str, true, "--Plz Select--");

    }
    protected void DDissueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkEdit.Checked == true)
        {
            FillReceiveNo();
            gvdetail.DataSource = null;
            gvdetail.DataBind();
        }
        else
        {
            gvdetail.DataSource = null;
            gvdetail.DataBind();
            //hnissueid.Value = DDissueno.SelectedValue;
            hnReceiveid.Value = "0";
            txtIssueNo.Text = "";
            FillissueGrid();
        }
    }
    protected void DDReceiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnReceiveid.Value = DDReceiveNo.SelectedValue;
        FillReceiveGrid();
    }
    protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow && gvdetail.EditIndex == e.Row.RowIndex)
        //{
        //    DropDownList DDMapType = (DropDownList)e.Row.FindControl("DDMapType");

        //    string str = @"select ID,MapType from MapType order by MapType";

        //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //    UtilityModule.ConditionalComboFillWithDS(ref DDMapType, ds, 0, true, "--Plz Select--");

        //    string selectedMapIssueType = DataBinder.Eval(e.Row.DataItem, "MapIssueType").ToString();
        //    DDMapType.Items.FindByValue(selectedMapIssueType).Selected = true;

        //}
    }
    protected void gvdetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvdetail.EditIndex = e.NewEditIndex;
        FillReceiveGrid();
        //FillissueGrid();
    }
    protected void gvdetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvdetail.EditIndex = -1;
        FillReceiveGrid();
        //FillissueGrid();
    }
    protected void gvdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}
        //SqlTransaction Tran = con.BeginTransaction();
        //try
        //{
        //    Label lblhqty = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblhqty");
        //    Label lblRId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblRId");
        //    Label lblRDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblRDetailId");
        //    Label lblId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblId");
        //    Label lblDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblDetailId");
        //    Label lblOrderId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblOrderId");
        //    TextBox txtqty = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtqty");
        //    Label lblItemFinishedId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblItemFinishedId");
        //    DropDownList DDMapType = (DropDownList)gvdetail.Rows[e.RowIndex].FindControl("DDMapType");

        //    TextBox txtMapReceiveRate = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtMapReceiveRate");
        //    Label lblReceiveMapArea = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblReceiveMapArea");
        //    Label lblReceiveRateCalcType = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblReceiveRateCalcType");
        //    Label lblMapIssueType = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblMapIssueType");

        //    //**************
        //    SqlParameter[] param = new SqlParameter[15];
        //    param[0] = new SqlParameter("@RId", lblRId.Text);
        //    param[1] = new SqlParameter("@RDetailId", lblRDetailId.Text);
        //    param[2] = new SqlParameter("@Id", lblId.Text);
        //    param[3] = new SqlParameter("@DetailId", lblDetailId.Text);
        //    param[4] = new SqlParameter("@OrderId", lblOrderId.Text);
        //    param[5] = new SqlParameter("@hqty", lblhqty.Text);
        //    param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
        //    param[6].Direction = ParameterDirection.Output;
        //    param[7] = new SqlParameter("@ReceiveQty", txtqty.Text == "" ? "0" : txtqty.Text);
        //    param[8] = new SqlParameter("@ItemFinishedId", lblItemFinishedId.Text);
        //    param[9] = new SqlParameter("@userid", Session["varuserid"]);
        //    param[10] = new SqlParameter("@MapStencilType", DDMapStencilType.SelectedValue);
        //    param[11] = new SqlParameter("@ReceiveRATE", txtMapReceiveRate.Text == "" ? "0" : txtMapReceiveRate.Text);
        //    param[12] = new SqlParameter("@ReceiveRateCalcType", lblReceiveRateCalcType.Text);
        //    param[13] = new SqlParameter("@ReceiveMapArea", lblReceiveMapArea.Text);
        //    param[14] = new SqlParameter("@MapIssueType", lblMapIssueType.Text);
        //    //*************
        //    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEMAPRECEIVE", param);
        //    lblmessage.Text = param[6].Value.ToString();
        //    Tran.Commit();
        //    gvdetail.EditIndex = -1;
        //    FillissueGrid();
        //    FillReceiveGrid();
        //}
        //catch (Exception ex)
        //{
        //    lblmessage.Text = ex.Message;
        //    Tran.Rollback();
        //}
        //finally
        //{
        //    con.Dispose();
        //    con.Close();
        //}
    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblhReceiveNoOfSet = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblhReceiveNoOfSet");
            Label lblRId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblRId");
            Label lblRDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblRDetailId");
            Label lblIssueId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblIssueId");
            Label lblIssueDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblIssueDetailId");
            Label lblOrderId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblOrderId");
            //TextBox txtReceiveNoOfSet = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtReceiveNoOfSet");
            Label lblItemFinishedId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblItemFinishedId");

            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@RId", lblRId.Text);
            param[1] = new SqlParameter("@RDetailid", lblRDetailId.Text);
            param[2] = new SqlParameter("@IssueId", lblIssueId.Text);
            param[3] = new SqlParameter("@IssueDetailid", lblIssueDetailId.Text);
            param[4] = new SqlParameter("@OrderId", lblOrderId.Text);
            param[5] = new SqlParameter("@ItemFinishedId", lblItemFinishedId.Text);
            param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[6].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEPUNCHCARDINDENTRECEIVEORDER", param);
            lblmessage.Text = param[6].Value.ToString();
            Tran.Commit();
            FillissueGrid();
            FillReceiveGrid();
            //***************
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }

}