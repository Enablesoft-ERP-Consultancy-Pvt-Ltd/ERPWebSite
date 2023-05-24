using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_PunchCardIndent_FrmPunchCardIndentOrder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                            Select UnitID, UnitName From Unit(Nolock) Where UnitID in (1, 2,6)
                            Select Id,Name From PunchCardIndentType(Nolock) order by Id ";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDunit, ds, 1, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDPunchCardIndentType, ds, 2, false, "");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                FillCustomercode();
            }

            if (DDPunchCardIndentType.Items.Count > 0)
            {
                DDPunchCardIndentType.SelectedIndex = 0;
            }
           
            DDunit.SelectedValue = "1";
            txtAssignDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtRequiredDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
            }
            FillCustomerOrderNo();

            //switch (Session["varcompanyId"].ToString())
            //{
            //    case "2":

            //switch(Session["VarCompanyId"].ToString())
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
         FillCustomercode(); 
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
        if (chkEdit.Checked == true)
        {
            string str = "";
            str = @"Select distinct EI.EmpId,EI.EmpName from PUNCHCARDINDENT_ORDERMASTER MIM inner join PUNCHCARDINDENT_ORDERDETAIL MID on MIM.ID=MID.MasterID 
                    INNER JOIN EmpInfo EI ON MIM.EmpId=EI.EmpId
                    INNER JOIN Department DP ON EI.DepartmentId=DP.DepartmentId 
                    Where MID.OrderId=" + DDCustomerOrderNo.SelectedValue + " and EI.DepartmentId=DP.DepartmentId  And DP.DepartmentName='DESIGNING' and isnull(Ei.blacklist,0)=0 order by EI.Empname";

            UtilityModule.ConditionalComboFill(ref DDDesignerName, str, true, "--Plz Select--");
        }
        else
        {
            string str = "";
            str = @"Select EI.EmpId,EI.EmpName from EmpInfo EI, Department DP Where EI.DepartmentId=DP.DepartmentId 
                And DP.DepartmentName='DESIGNING' And EI.MasterCompanyId=" + Session["varCompanyId"] + " and isnull(Ei.blacklist,0)=0 order by EI.Empname";

            UtilityModule.ConditionalComboFill(ref DDDesignerName, str, true, "--Plz Select--");
        }
    }
    protected void DDCustomerOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDesignerName();
        Fillgrid();

        hnissueid.Value = "0";

        //gvdetail.DataSource = null;
        //gvdetail.DataBind();

        DDDesignerName.SelectedIndex = -1;
        DDissueno.Items.Clear();
        txtissueno.Text = "";
    }
    protected void FillIssueNo()
    {
        if (chkEdit.Checked == true)
        {
            string str = @"select Distinct MIM.ID,MIM.ChallanNo from PUNCHCARDINDENT_ORDERMASTER MIM inner join PUNCHCARDINDENT_ORDERDETAIL MID on MIM.ID=MID.MasterID
                           and MID.OrderID=" + DDCustomerOrderNo.SelectedValue + " and MIM.EmpID=" + DDDesignerName.SelectedValue + " ";

            UtilityModule.ConditionalComboFill(ref DDissueno, str, true, "--Plz Select--");
        }
        //else
        //{
        //    Fillgrid();
        //}
    }
    protected void DDDesignerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillIssueNo();

    }
    //protected void DDMapStencilType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (chkEdit.Checked == true)
    //    {
    //        FillIssueNo();
    //        gvdetail.DataSource = null;
    //        gvdetail.DataBind();
    //        txtissueno.Text = "";

    //    }
    //    hnissueid.Value = "0";
    //    Fillgrid();
    //}
    protected void Fillgrid()
    {
//        string str = @"select OM.OrderId,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,VF.SIZEMtr,
//                        CASE WHEN " + DDunit.SelectedValue + @" = 2 THEN VF.SIZEFT ELSE VF.SIZEMtr END As Size,
//                        sum(OD.QtyRequired) as OrderQty,OD.Item_Finished_Id, dbo.[GET_MAPISSUEQTY](OD.Item_Finished_Id,OM.OrderID," + DDMapStencilType.SelectedValue + "," + DDCompany.SelectedValue + @") as PreIssueQty,
//                        CASE WHEN " + DDunit.SelectedValue + @" = 2 THEN VF.ProdAreaFt ELSE VF.AreaMtr END As Area
//                        from OrderMaster OM INNER JOIN OrderDetail OD ON OM.OrderId=OD.OrderId
//                        INNER JOIN V_FinishedItemDetail VF ON OD.Item_Finished_Id=VF.Item_Finished_Id
//                        Where OM.Orderid=" + DDCustomerOrderNo.SelectedValue + @"
//                        Group By OM.OrderId,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,OD.OrderUnitId,VF.SIZEFT,VF.SIZEMTR,VF.SIZEINCH,OD.Item_Finished_Id,VF.ProdAreaFt,VF.AreaMtr";


        string str = @"select OM.OrderId,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,VF.SIZEMtr,
                        CASE WHEN OD.OrderUnitId = 6 THEN VF.SizeInch else Case When OD.OrderUnitId = 1 THEN VF.SizeMtr ELSE VF.SizeFt END End As Size,
                        sum(OD.QtyRequired) as OrderQty,OD.Item_Finished_Id,0 as PreIssueQty,                       
                        CASE WHEN OD.OrderUnitId = 6 THEN VF.AreaFt else case when OD.OrderUnitId= 1 THEN VF.AreaMtr ELSE VF.AreaFt END End As Area
                        from OrderMaster OM INNER JOIN OrderDetail OD ON OM.OrderId=OD.OrderId
                        INNER JOIN V_FinishedItemDetail VF ON OD.Item_Finished_Id=VF.Item_Finished_Id
                        Where OM.Orderid=" + DDCustomerOrderNo.SelectedValue + @"
                        Group By OM.OrderId,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,OD.OrderUnitId,VF.SIZEFT,VF.SIZEMTR,VF.SIZEINCH,OD.Item_Finished_Id,VF.ProdAreaFt,VF.AreaMtr,VF.AreaFt";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();

    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
            ////for (int i = 0; i < DG.Columns.Count; i++)
            ////{
            ////    if (variable.VarBINNOWISE == "1")
            ////    {
            ////        if (DG.Columns[i].HeaderText == "BinNo")
            ////        {
            ////            DG.Columns[i].Visible = true;
            ////        }
            ////    }
            ////    else
            ////    {
            ////        if (DG.Columns[i].HeaderText == "BinNo")
            ////        {
            ////            DG.Columns[i].Visible = false;
            ////        }
            ////    }
            ////}
            //DropDownList DDMapType = ((DropDownList)e.Row.FindControl("DDMapType"));
            //string str = @"select ID,MapType from MapType order by MapType";

            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            //UtilityModule.ConditionalComboFillWithDS(ref DDMapType, ds, 0, true, "--Plz Select--");
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
                TextBox txtPerSetQty = ((TextBox)DG.Rows[i].FindControl("txtPerSetQty"));
                TextBox txtNoOfSet = ((TextBox)DG.Rows[i].FindControl("txtNoOfSet"));
               // Label lblBalToRecQty = ((Label)DG.Rows[i].FindControl("lblBalToRecQty"));

                //if (Chkboxitem.Checked == false)   // Change when Updated Completed
                //{
                //    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please Select Checkbox');", true);               
                //    return;
                //}

                if ((txtPerSetQty.Text == "" || txtNoOfSet.Text=="") && Chkboxitem.Checked == true)   // Change when Updated Completed
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Per Set Qty/NoOf Set Qty can not be blank');", true);
                    txtPerSetQty.Focus();
                    return;
                }
                if (Chkboxitem.Checked == true && ((Convert.ToDecimal(txtPerSetQty.Text) <= 0) || Convert.ToDecimal(txtNoOfSet.Text) <= 0))   // Change when Updated Completed
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Per Set Qty/NoOf Set Qty always greater then zero');", true);
                    txtPerSetQty.Focus();
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
                TextBox txtPerSetQty = ((TextBox)DG.Rows[i].FindControl("txtPerSetQty"));
                TextBox txtNoOfSet = ((TextBox)DG.Rows[i].FindControl("txtNoOfSet"));
                //Label lblBalToRecQty = ((Label)DG.Rows[i].FindControl("lblBalToRecQty"));
                Label lblOrderQty = ((Label)DG.Rows[i].FindControl("lblOrderQty"));
                Label lblItemFinishedId = ((Label)DG.Rows[i].FindControl("lblItemFinishedId"));
                Label lblOrderId = ((Label)DG.Rows[i].FindControl("lblOrderId"));
                Label lblArea = ((Label)DG.Rows[i].FindControl("lblArea"));


                if (Chkboxitem.Checked == true && (txtPerSetQty.Text != "") && (txtNoOfSet.Text != "") && DDCompany.SelectedIndex > 0 && DDCustomerOrderNo.SelectedIndex > 0)
                {
                    Strdetail = Strdetail + txtPerSetQty.Text + '|' + txtNoOfSet.Text + '|' + lblOrderQty.Text + '|' + lblItemFinishedId.Text + '|' + lblOrderId.Text + '|' + lblArea.Text  + '~';
                }
            }
        
            if (Strdetail!="")
            {
                SqlTransaction Tran = con.BeginTransaction();
                try
                {
                    SqlParameter[] param = new SqlParameter[14];
                    param[0] = new SqlParameter("@ID", SqlDbType.Int);
                    param[0].Value = hnissueid.Value;
                    param[0].Direction = ParameterDirection.InputOutput;
                    param[1] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
                    param[2] = new SqlParameter("@CustomerId", DDCustomerCode.SelectedValue);
                    param[3] = new SqlParameter("@EmpId", DDDesignerName.SelectedValue);
                    param[4] = new SqlParameter("@AssignDate", txtAssignDate.Text);
                    param[5] = new SqlParameter("@RequiredDate", txtRequiredDate.Text);
                    param[6] = new SqlParameter("@userid", Session["varuserid"]);
                    param[7] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
                    param[8] = new SqlParameter("@StringDetail", Strdetail);                  
                    param[9] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                    param[9].Direction = ParameterDirection.Output;
                    param[10] = new SqlParameter("@UnitID", SqlDbType.TinyInt);
                    param[10].Value = DDunit.SelectedValue;
                    param[11] = new SqlParameter("@Remarks", TxtRemarks.Text);
                    param[12] = new SqlParameter("@CHALLANNO", SqlDbType.VarChar, 50);
                    param[12].Value = "";
                    param[12].Direction = ParameterDirection.InputOutput;
                    param[13] = new SqlParameter("@PunchCardIndentType", SqlDbType.TinyInt);
                    param[13].Value = DDPunchCardIndentType.SelectedValue;


                    ///**********
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SavePunchCardIndent_Order", param);
                    //*******************
                    //ViewState["reportid"] = param[0].Value.ToString();
                    txtissueno.Text = param[12].Value.ToString();
                    hnissueid.Value = param[0].Value.ToString();
                    Tran.Commit();
                    if (param[9].Value.ToString() != "")
                    {
                        lblmessage.Text = param[9].Value.ToString();
                    }
                    else
                    {
                        lblmessage.Text = "DATA SAVED SUCCESSFULLY.";
                        Fillgrid();
                        FillissueGrid();
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
    protected void FillissueGrid()
    {
        TxtRemarks.Text = "";
        string str = @"select MIM.Id,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,
                        CASE WHEN OD.OrderUnitId = 6 THEN VF.SizeInch else Case When OD.OrderUnitId = 1 THEN VF.SizeMtr ELSE VF.SizeFt END End As Size,
                        MID.DetailId,MID.OrderId,MID.ItemFinishedID,MIM.CompanyId,MIM.CustomerId,MIM.EmpId,MIM.ChallanNo,
                        Replace(CONVERT(nvarchar(11),MIM.AssignDate,106),' ','-') as AssignDate,Replace(CONVERT(nvarchar(11),MIM.RequiredDate,106),' ','-') as RequiredDate,
                        MID.PerSetQty,MID.NoOfSet, MIM.UnitID ,MIM.Remarks,MID.TotalSetQty,MIM.PunchCardIndentType                       
                        from PUNCHCARDINDENT_ORDERMASTER MIM 
                        INNER JOIN PUNCHCARDINDENT_ORDERDetail MID ON MIM.Id=MID.MasterId 
                        INNER JOIN V_FinishedItemDetail VF ON MID.ItemFinishedId=VF.Item_Finished_Id
                        INNER JOIN OrderDetail OD ON MID.OrderId=OD.OrderID and MID.ItemFinishedId=OD.Item_Finished_id                      
                        Where MIM.CompanyId = " + DDCompany.SelectedValue;

        //where MIM.ID=" + hnissueid.Value;
        if (txtEditIssueNo.Text != "")
        {
            str = str + " and MIM.ChallanNo='" + txtEditIssueNo.Text + "'";
        }
        else
        {
            str = str + " and MIM.ID=" + hnissueid.Value + "";
        }

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
                txtissueno.Text = ds.Tables[0].Rows[0]["ChallanNo"].ToString();
                txtAssignDate.Text = ds.Tables[0].Rows[0]["AssignDate"].ToString();
                txtRequiredDate.Text = ds.Tables[0].Rows[0]["RequiredDate"].ToString();
                //DDCompany.SelectedValue = ds.Tables[0].Rows[0]["CompanyId"].ToString();
                DDunit.SelectedValue = ds.Tables[0].Rows[0]["UnitId"].ToString();
                FillCustomercode();
                DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["CustomerId"].ToString();
              
                FillCustomerOrderNo();
                DDCustomerOrderNo.SelectedValue = ds.Tables[0].Rows[0]["OrderId"].ToString();
                FillDesignerName();
                DDDesignerName.SelectedValue = ds.Tables[0].Rows[0]["EmpId"].ToString();               
                FillIssueNo();
                DDissueno.SelectedValue = ds.Tables[0].Rows[0]["ID"].ToString();
                TxtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                DDPunchCardIndentType.SelectedValue = ds.Tables[0].Rows[0]["PunchCardIndentType"].ToString();
            }
            else
            {
                txtissueno.Text = "";
                txtAssignDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
                txtRequiredDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@Id", hnissueid.Value);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_PunchCardIndentOrderReport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\RptPunchCardIndentOrderReport.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptPunchCardIndentOrderReport.xsd";
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
            TDEditIssueNo.Visible = true;
            DDCustomerCode.SelectedIndex = -1;
            DDCustomerOrderNo.SelectedIndex = -1;
            DDDesignerName.SelectedIndex = -1;
            TDIssueNo.Visible = true;
            DDissueno.SelectedIndex = -1;
            DG.DataSource = null;
            DG.DataBind();
            gvdetail.DataSource = null;
            gvdetail.DataBind();
            txtissueno.Text = "";
            txtAssignDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
           
        }
        else
        {
            TDEditIssueNo.Visible = false;
            txtEditIssueNo.Text = "";
            DDCustomerCode.SelectedIndex = -1;
            DDCustomerOrderNo.SelectedIndex = -1;
            DDDesignerName.SelectedIndex = -1;
            TDIssueNo.Visible = false;
            DDissueno.SelectedIndex = -1;
            DG.DataSource = null;
            DG.DataBind();
            gvdetail.DataSource = null;
            gvdetail.DataBind();
            txtissueno.Text = "";
            txtAssignDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            
        }
    }
    protected void txtEditIssueNo_TextChanged(object sender, EventArgs e)
    {
        FillissueGrid();
        Fillgrid();
        DDissueno_SelectedIndexChanged(sender, new EventArgs());
    }
    protected void DDissueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkEdit.Checked == true)
        {
            txtEditIssueNo.Text = "";
        }
       
         hnissueid.Value =DDissueno.SelectedIndex >0 ? DDissueno.SelectedValue : "0";        
        
        FillissueGrid();
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
        FillissueGrid();
    }
    protected void gvdetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvdetail.EditIndex = -1;
        FillissueGrid();
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
        //    Label lblId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblId");
        //    Label lblDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblDetailId");
        //    Label lblOrderId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblOrderId");
        //    TextBox txtqty = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtqty");
        //    Label lblItemFinishedId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblItemFinishedId");
        //    DropDownList DDMapType = (DropDownList)gvdetail.Rows[e.RowIndex].FindControl("DDMapType");
        //    Label lblMapStencilType = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblMapStencilType");
        //    TextBox txtremarks = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtRemarks");
        //    TextBox txtMapIssueRate = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtMapIssueRate");
        //    Label lblMapArea = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblMapArea");


        //    //**************
        //    SqlParameter[] param = new SqlParameter[14];
        //    param[0] = new SqlParameter("@Id", lblId.Text);
        //    param[1] = new SqlParameter("@DetailId", lblDetailId.Text);
        //    param[2] = new SqlParameter("@OrderId", lblOrderId.Text);
        //    param[3] = new SqlParameter("@hqty", lblhqty.Text);
        //    param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
        //    param[4].Direction = ParameterDirection.Output;
        //    param[5] = new SqlParameter("@MapIssueQty", txtqty.Text == "" ? "0" : txtqty.Text);
        //    param[6] = new SqlParameter("@ItemFinishedId", lblItemFinishedId.Text);
        //    param[7] = new SqlParameter("@userid", Session["varuserid"]);
        //    param[8] = new SqlParameter("@MapIssueType", DDMapType.SelectedValue);
        //    param[9] = new SqlParameter("@MapStencilType", lblMapStencilType.Text);
        //    param[10] = new SqlParameter("@MapRemarks", txtremarks.Text);
        //    param[11] = new SqlParameter("@MapIssueRate", txtMapIssueRate.Text == "" ? "0" : txtMapIssueRate.Text);
        //    param[12] = new SqlParameter("@RateCalcType", DDRateCalcType.SelectedValue);
        //    param[13] = new SqlParameter("@MapIssueArea", lblMapArea.Text);
        //    //*************
        //    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEMAPISSUE", param);
        //    lblmessage.Text = param[4].Value.ToString();
        //    Tran.Commit();
        //    gvdetail.EditIndex = -1;
        //    FillissueGrid();
        //    Fillgrid();
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
            Label lblId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblId");
            Label lblDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblDetailId");
            Label lblOrderId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblOrderId");
            Label lblItemFinishedId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblItemFinishedId");

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@ID", lblId.Text);
            param[1] = new SqlParameter("@DetailID", lblDetailId.Text);
            param[2] = new SqlParameter("@OrderId", lblOrderId.Text);
            param[3] = new SqlParameter("@ItemFinishedID", lblItemFinishedId.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEPUNCHCARDINDENTORDER", param);
            lblmessage.Text = param[4].Value.ToString();
            Tran.Commit();
            FillissueGrid();
            Fillgrid();
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