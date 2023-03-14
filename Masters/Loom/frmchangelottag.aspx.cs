using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ClosedXML.Excel;
using System.IO;

public partial class Masters_Loom_frmchangelottag : System.Web.UI.Page
{
    static string btnclickflag = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                            select UnitsId,UnitName from Units order by UnitName
                            select PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where ProcessType=1 and MasterCompanyid=" + Session["varcompanyid"] + @" order by PROCESS_NAME_ID
                            Select ID, BranchName 
                            From BRANCHMASTER BM(nolock) 
                            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDprocess, ds, 2, true, "--Plz Select--");
            
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 3, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            //txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            //if (Session["canedit"].ToString() == "1")
            //{
            //    TDEdit.Visible = true;
            //}
            //if (variable.Carpetcompany == "1")
            //{
            //    TRempcodescan.Visible = true;
            //}
            //if (variable.VarWeaverRawMaterialIssueToCompleteStatus == "1" && Session["usertype"].ToString() == "1")
            //{
            //    TDForCompleteStatus.Visible = true;
            //}
            if (Session["varcompanyid"].ToString() == "21")
            {
                TDTxtTotalPcs.Visible = true;
                btnPreviewStockNo.Visible = false;
            }
            if (Session["varcompanyid"].ToString() == "22")
            {
              //  TDFolioIssueDate.Visible = true;
                TDFolioNo.Visible = true;

                if (DDprocess.Items.Count > 0)
                {
                    DDprocess.SelectedIndex = 1;                   
                    
                }
            }
        }
    }
    protected void DDprocess_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (DDprocess.SelectedItem.Text.ToUpper())
        {
            case "WEAVING":
                //TDProductionunit.Visible = true;
                //TDLoomNo.Visible = true;
                break;
            default:
                TDProductionunit.Visible = false;
                TDLoomNo.Visible = false;
                FillFolioNo();
                break;
        }

    }
    protected void FillFolioNo(object sender = null)
    {
        string str = @"select Distinct PM.IssueOrderId,PM.ChallanNo from PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + @" PM left join employee_Processorderno EMp on PM.issueorderid=EMp.issueorderid and EMp.processid=" + DDprocess.SelectedValue + @"
                     left join empinfo ei on emp.empid=ei.empid 
                    Where PM.Companyid=" + DDcompany.SelectedValue + " And IsNull(PM.DEPARTMENTTYPE, 0) = 0 ";
        //if (TDForCompleteStatus.Visible == true)
        //{
        //    if (ChkForCompleteStatus.Checked == true)
        //    {
        //        str = str + " and PM.status='Complete'";
        //    }
        //    else
        //    {
                str = str + " and PM.status='Pending'";
        //    }
        //}
        //else
        //{
        //    str = str + " and PM.status='Pending'";
        //}
        if (TDProductionunit.Visible == true)
        {
            if (DDProdunit.SelectedIndex > 0)
            {
                str = str + " and Units=" + DDProdunit.SelectedValue;
            }
            else
            {
                str = str + " and Units=0";
            }
        }
        if (TDLoomNo.Visible == true)
        {
            if (DDLoomNo.SelectedIndex > 0)
            {
                str = str + " and LoomId=" + DDLoomNo.SelectedValue;
            }
            else
            {
                str = str + " and Loomid=0";
            }
        }
        //if (txtWeaverIdNoscan.Text != "")
        //{
        //    str = str + " and EI.empcode='" + txtWeaverIdNoscan.Text + "'";
        //}
        //if (TDFolioNo.Visible == true)
        //{
        //    if (txtFolioNo.Text != "")
        //    {
        //        str = str + " and PM.IssueOrderId='" + txtFolioNo.Text + "'";
        //    }
        //}

        UtilityModule.ConditionalComboFill(ref DDFoliono, str, true, "--Plz Select--");
        if (DDFoliono.Items.Count > 0)
        {
            if (sender != null)
            {
                DDFoliono.SelectedIndex = 1;
                DDFoliono_SelectedIndexChanged(sender, new EventArgs());
            }

        }
    }
    protected void DDProdunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        DG.DataSource = null;
        DG.DataBind();
        //        string str = @"select Distinct PL.UID,PL.LoomNo,case when ISNUMERIC(loomno)=1 Then CONVERT(int,loomno) Else 9999999 End as Loom from PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + @" PM inner join ProductionLoomMaster PL
        //                    on PM.LoomId=PL.UID and PM.Status='Pending'
        //                    And PL.companyid=" + DDcompany.SelectedValue + " and  PL.UnitId=" + DDProdunit.SelectedValue + @" order by Loom,LoomNo
        //                    select Top(1) Godownid from Productionunitgodown Where Produnitid=" + DDProdunit.SelectedValue + " order by id desc";
        string str = @"select Distinct PL.UID,PL.LoomNo,case when ISNUMERIC(loomno)=1 Then CONVERT(int,loomno) Else 9999999 End as Loom from PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + @" PM inner join ProductionLoomMaster PL
                    on PM.LoomId=PL.UID   And PL.companyid=" + DDcompany.SelectedValue + " and  PL.UnitId=" + DDProdunit.SelectedValue + @"  Left Join Employee_ProcessorderNo EMP on PM.issueorderid=EMP.issueorderid and EMP.processid=" + DDprocess.SelectedValue + @"
                    Left join empinfo ei on EMp.empid=Ei.empid  ";
        //if (txtWeaverIdNoscan.Text != "")
        //{
        //    str = str + " Where Ei.empcode='" + txtWeaverIdNoscan.Text + "'";
        //}
        //if (TDFolioNo.Visible == true)
        //{
        //    if (txtFolioNo.Text != "")
        //    {
        //        str = str + " Where PM.IssueOrderId='" + txtFolioNo.Text + "'";
        //    }
        //}
        str = str + " order by Loom,LoomNo";
        str = str + "  select Top(1) Godownid from Productionunitgodown Where Produnitid=" + DDProdunit.SelectedValue + " order by id desc";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        UtilityModule.ConditionalComboFillWithDS(ref DDLoomNo, ds, 0, true, "--Plz Selec--");
        //*********
        //********
        if (ds.Tables[1].Rows.Count > 0)
        {
            hngodownid.Value = ds.Tables[1].Rows[0]["godownid"].ToString();
        }
        else
        {
            hngodownid.Value = "0";
        }
        if (DDLoomNo.Items.Count > 0)
        {
            DDLoomNo.SelectedIndex = DDLoomNo.Items.Count - 1;
            DDLoomNo_SelectedIndexChanged(sender, new EventArgs());
        }


    }
    protected void DDLoomNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DG.DataSource = null;
        DG.DataBind();
        FillFolioNo(sender);

    }
    protected void DDFoliono_SelectedIndexChanged(object sender, EventArgs e)
    {
//        if (chkEdit.Checked == true)
//        {
        string str = @"select Distinct PRM.ChalanNo,PRM.ChalanNo 
                        From ProcessRawMaster PRM 
                        inner join ProcessRawTran PRT on PRM.PRMid=PRT.PRMid 
                        Where PRM.BeamType=0 and PRM.TranType=0 And PRM.TypeFlag = 0 and PRM.CompanyID = " + DDcompany.SelectedValue + " and PRM.Processid=" + DDprocess.SelectedValue + "";
       
        if (!string.IsNullOrEmpty(Convert.ToString(txtFolioNo.Text)))
        {
            str = str + " and PRM.Prorderid=" + txtFolioNo.Text + "";
        }
        else
        {
            str = str + " and PRM.Prorderid=" + DDFoliono.SelectedValue + "";

        }
            UtilityModule.ConditionalComboFill(ref DDissueno, str, true, "--Plz Select--");
//            if (Session["varcompanyid"].ToString() == "21")
//            {
//                Fillgrid();
//            }

//            if (Session["varcompanyid"].ToString() == "22")
//            {
//                string str2 = @"select isnull(Replace(convert (varchar(11), PM.AssignDate,106),' ','-'),'') as AssignDate
//                        from PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + " PM  Where PM.issueorderid=" + DDFoliono.SelectedValue + @"";

//                DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
//                if (ds2.Tables[0].Rows.Count > 0)
//                {
//                    txtFolioIssueDate.Text = ds2.Tables[0].Rows[0]["AssignDate"].ToString();
//                }
//            }
//        }
//        else
//        {
            Fillgrid();
        //}

    }
    protected void Fillgrid()
    {

        string str = @"select  dbo.F_getItemDescription(PRT.Finishedid,prt.flagsize) as ItemDescription,PRM.PrmId,PRM.ChalanNo,prt.TagNo,prt.Lotno,PRT.PRTid 
                        From ProcessRawMaster PRM 
                        inner join ProcessRawTran PRT on PRM.PRMid=PRT.PRMid 
						JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = PRT.Finishedid 
                        Where PRM.BeamType=0 and PRM.TranType=0 And PRM.TypeFlag = 0 and PRM.CompanyID = " + DDcompany.SelectedValue + @" and PRM.Processid=" + DDprocess.SelectedValue + "";
        if (!string.IsNullOrEmpty(Convert.ToString(txtFolioNo.Text)))
        {
            str = str + " and PRM.Prorderid=" + txtFolioNo.Text + "";
        }
        else
        {
            str = str + " and PRM.Prorderid=" + DDFoliono.SelectedValue + "";
        
        }

        if (!string.IsNullOrEmpty(Convert.ToString(DDissueno.SelectedValue)))
        {
            if (Convert.ToInt32(DDissueno.SelectedValue) > 0)
            {
                str = str + " and ChalanNo=" + DDissueno.SelectedValue + "";
            }
        
        }
//        string str = @"select dbo.F_getItemDescription(PCD.IFINISHEDID,PCd.ISizeflag) as ItemDescription,U.UnitName,
//                        case when " + DDprocess.SelectedValue + @"=1 then Round(Sum(CASE WHEN PCD.ICALTYPE<>1 THEN CASE WHEN PM.UnitId=1 Then PCD.IQTY*1.196 else PCD.IQTY END ELSE CASE WHEN PM.UnitId=1 
//                        Then PCD.IQTY*1.196 else PCD.IQTY END END),3) else
//                        Round((CASE WHEN PCD.ICALTYPE<>1 THEN CASE WHEN PM.UnitId=1 Then PCD.IQTY*1.196 else PCD.IQTY END ELSE CASE WHEN PM.UnitId=1 
//                        Then PCD.IQTY*1.196 else PCD.IQTY END END),3) end as OnePcdconsmpqty,
//                       
//                        ISNULL(ROUND(SUM(CASE WHEN PCD.ICALTYPE=0 OR PCD.ICALTYPE=2 THEN CASE WHEN PM.UNITID=1 THEN PD.QTY*PCD.IQTY*1.196 * 
//				        Case When VF.MasterCompanyId in (28, 16) Then VF.AreaMtr Else PD.Area End 
//				        ELSE PD.QTY*PCD.IQTY * Case When VF.MasterCompanyId in (28, 16) Then Round(VF.AreaFt * 144.0 / 1296.0, 4, 1) Else PD.Area End END ELSE 
//				        CASE WHEN PM.UNITID=1 THEN PD.QTY*PCD.IQTY*1.196 ELSE PD.QTY*PCD.IQTY END END),3),0) consmpqty,
//                        dbo.[Get_ProcessIssueQtyNew](PCD.IFINISHEDID,PCD.PROCESSID,PM.IssueOrderId,0) as IssuedQty,
//                        ROUND(ISNULL(ROUND(SUM(CASE WHEN PCD.ICALTYPE=0 OR PCD.ICALTYPE=2 THEN CASE WHEN PM.UNITID=1 THEN PD.QTY*PCD.IQTY*1.196 * 
//				        Case When VF.MasterCompanyId in (28, 16) Then VF.AreaMtr Else PD.Area End 
//				        ELSE PD.QTY*PCD.IQTY * Case When VF.MasterCompanyId in (28, 16) Then Round(VF.AreaFt * 144.0 / 1296.0, 4, 1) Else PD.Area End END ELSE 
//				        CASE WHEN PM.UNITID=1 THEN PD.QTY*PCD.IQTY*1.196 ELSE PD.QTY*PCD.IQTY END END),3),0)-dbo.[Get_ProcessIssueQtyNew](PCD.IFINISHEDID,PCD.PROCESSID,PM.IssueOrderId,0),3) as BalanceQty,
//                        PCD.IFINISHEDID,PCD.IUNITID,PCD.ISizeflag,PM.issueorderid
//                        From PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + @" PM 
//                        JOIN PROCESS_ISSUE_DETAIL_" + DDprocess.SelectedValue + @" PD on PM.IssueOrderId=PD.IssueOrderId
//                        JOIN PROCESS_CONSUMPTION_DETAIL PCD on PD.IssueOrderId=PCD.ISSUEORDERID and PD.Issue_Detail_Id=PCD.ISSUE_DETAIL_ID and PCD.PROCESSID=" + DDprocess.SelectedValue + @" 
//                        JOIN Unit U on PCD.IUNITID=U.UnitId 
//                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = PD.Item_Finished_Id 
//                        Where PM.issueorderid=" + DDFoliono.SelectedValue + @"
//                        group by PCD.IFINISHEDID,PCd.ISizeflag,u.UnitName,PCD.IFINISHEDID,PCD.PROCESSID,PM.IssueOrderId,PCD.IUNITID,PCD.ICALTYPE,PM.UnitId,PCD.IQTY order by ItemDescription";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DG.DataSource = ds.Tables[0];
            DG.DataBind();
        }
        else
        {
            DG.DataSource = null;
            DG.DataBind();
        }

        if (Session["varcompanyid"].ToString() == "22")
        {
            string str2 = @"select isnull(Replace(convert (varchar(11), PM.AssignDate,106),' ','-'),'') as AssignDate
                        from PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + " PM  Where"; 
            if (!string.IsNullOrEmpty(Convert.ToString(txtFolioNo.Text)))
        {
            str2 = str2 + "  PM.IssueOrderId=" + txtFolioNo.Text + "";
        }
        else
        {
            str2 = str2 + "  PM.IssueOrderId=" + DDFoliono.SelectedValue + "";

        }
            
         //   PM.issueorderid=" + DDFoliono.SelectedValue + @"";

            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                //txtFolioIssueDate.Text = ds2.Tables[0].Rows[0]["AssignDate"].ToString();
            }

            if (DG.Rows.Count > 0)
            {
                CheckBox ChkBoxHeader = (CheckBox)DG.HeaderRow.FindControl("ChkAllItem");
                ChkBoxHeader.Checked = true;

                foreach (GridViewRow row in DG.Rows)
                {
                    CheckBox Chkboxitem = (CheckBox)row.FindControl("Chkboxitem");

                    if (ChkBoxHeader.Checked == true)
                    {
                        Chkboxitem.Checked = true;
                    }
                    else
                    {
                        Chkboxitem.Checked = false;
                    }
                }
            }
            
        }

    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
//        if (e.Row.RowType == DataControlRowType.DataRow)
//        {

//            for (int i = 0; i < DG.Columns.Count; i++)
//            {
//                if (variable.VarBINNOWISE == "1")
//                {
//                    if (DG.Columns[i].HeaderText == "BinNo")
//                    {
//                        DG.Columns[i].Visible = true;
//                    }
//                }
//                else
//                {
//                    if (DG.Columns[i].HeaderText == "BinNo")
//                    {
//                        DG.Columns[i].Visible = false;
//                    }
//                }
//            }
//            DropDownList DDGodown = ((DropDownList)e.Row.FindControl("DDGodown"));
//            string str = @"Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + @" Order by GodownName
//                           select godownid From Modulewisegodown Where ModuleName='" + Page.Title + "'";

//            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
//            UtilityModule.ConditionalComboFillWithDS(ref DDGodown, ds, 0, true, "--Plz Select--");

//            if (hngodownid.Value == "0")
//            {
//                if (ds.Tables[1].Rows.Count > 0)
//                {
//                    if (DDGodown.Items.FindByValue(ds.Tables[1].Rows[0]["godownid"].ToString()) != null)
//                    {
//                        DDGodown.SelectedValue = ds.Tables[1].Rows[0]["godownid"].ToString();
//                    }
//                }
//                else
//                {
//                    if (DDGodown.Items.Count > 0)
//                    {
//                        DDGodown.SelectedIndex = 1;
//                    }
//                }
//            }
//            else
//            {
//                if (DDGodown.Items.FindByValue(hngodownid.Value) != null)
//                {
//                    DDGodown.SelectedValue = hngodownid.Value;
//                }
//            }
//            DDgodown_SelectedIndexChanged(DDGodown, new EventArgs());
//            ds.Dispose();
//        }
    }
    protected void DDBinNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList DDBinNo = (DropDownList)sender;
        GridViewRow row = (GridViewRow)DDBinNo.Parent.Parent;
        Label Ifinishedid = ((Label)row.FindControl("lblifinishedid"));
        DropDownList ddlgodown = ((DropDownList)row.FindControl("DDGodown"));

        DropDownList ddLotno = ((DropDownList)row.FindControl("DDLotNo"));
        if (Session["VarcompanyNo"].ToString() == "22")
        {
            SqlParameter[] array = new SqlParameter[4];
            array[0] = new SqlParameter("@CompanyId", DDcompany.SelectedValue);
            array[1] = new SqlParameter("@GodownId", ddlgodown.SelectedValue);
            array[2] = new SqlParameter("@item_finished_id", Ifinishedid.Text);
            array[3] = new SqlParameter("@MASTERCOMPANYID", Session["varcompanyNo"]);
            array[4] = new SqlParameter("@BinNo",  variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : "");

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_DDBIND_LOTNO", array);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddLotno.DataSource = ds;
                ddLotno.DataTextField = "Lotno";
                ddLotno.DataValueField = "Lotno";
                ddLotno.DataBind();
                ddLotno.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }
        else
        {
            string str = "select Distinct S.Lotno,S.Lotno from Stock S Where S.companyid=" + DDcompany.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Qtyinhand>0";
            if (variable.VarBINNOWISE == "1")
            {
                str = str + " and BinNo='" + DDBinNo.SelectedItem.Text + "'";
            }
            UtilityModule.ConditionalComboFill(ref ddLotno, str, true, "--Plz Select--");
        }
       
        if (ddLotno.Items.Count > 0)
        {
            ddLotno.SelectedIndex = 1;
            DDLotno_SelectedIndexChanged(ddLotno, e);
        }
    }
    protected void DDgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlgodown = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlgodown.Parent.Parent;
        Label Ifinishedid = ((Label)row.FindControl("lblifinishedid"));

        if (variable.VarBINNOWISE == "1")
        {
            DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));
            string str = "select Distinct S.BinNo,S.BinNo from Stock S Where S.companyid=" + DDcompany.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Qtyinhand>0";
            UtilityModule.ConditionalComboFill(ref DDBinNo, str, true, "--Plz Select--");
            if (DDBinNo.Items.Count > 0)
            {
                DDBinNo.SelectedIndex = 1;
            }
        }
        else
        {
            int index = row.RowIndex;

            DropDownList ddLotno = ((DropDownList)row.FindControl("DDLotNo"));

            if (Session["VarcompanyNo"].ToString() == "22")
            {
                SqlParameter[] array = new SqlParameter[4];
                array[0] = new SqlParameter("@CompanyId", DDcompany.SelectedValue);
                array[1] = new SqlParameter("@GodownId", ddlgodown.SelectedValue);
                array[2] = new SqlParameter("@item_finished_id", Ifinishedid.Text);
                array[3] = new SqlParameter("@MASTERCOMPANYID", Session["varcompanyNo"]);                

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_DDBIND_LOTNO", array);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddLotno.DataSource = ds;
                    ddLotno.DataTextField = "Lotno";
                    ddLotno.DataValueField = "Lotno";
                    ddLotno.DataBind();
                    ddLotno.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
            else
            {
                string str = "select Distinct S.Lotno,S.Lotno from Stock S Where S.companyid=" + DDcompany.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Qtyinhand>0";
                UtilityModule.ConditionalComboFill(ref ddLotno, str, true, "--Plz Select--");
            }           
            
            if (ddLotno.Items.Count > 0)
            {
                ddLotno.SelectedIndex = 1;
                DDLotno_SelectedIndexChanged(ddLotno, e);
            }           
        }

    }
    protected void DDLotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlLotno = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlLotno.Parent.Parent;
        int index = row.RowIndex;
        //Label Ifinishedid = ((Label)DG.Rows[index].FindControl("lblifinishedid"));
        Label Ifinishedid = (Label)row.FindControl("lblifinishedid");
        //DropDownList DDTagNo = ((DropDownList)DG.Rows[index].FindControl("DDTagNo"));
        //DropDownList ddlgodown = ((DropDownList)DG.Rows[index].FindControl("DDgodown"));
        DropDownList DDTagNo = ((DropDownList)row.FindControl("DDTagNo"));
        DropDownList ddlgodown = ((DropDownList)row.FindControl("DDgodown"));
        DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));

        if (Session["VarcompanyNo"].ToString() == "22")
        {
            SqlParameter[] array = new SqlParameter[6];
            array[0] = new SqlParameter("@CompanyId", DDcompany.SelectedValue);
            array[1] = new SqlParameter("@GodownId", ddlgodown.SelectedValue);
            array[2] = new SqlParameter("@item_finished_id", Ifinishedid.Text);
            array[3] = new SqlParameter("@Lotno", ddlLotno.Text);
            array[4] = new SqlParameter("@BinNo",  variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : "");
            array[5] = new SqlParameter("@MASTERCOMPANYID", Session["varcompanyNo"]);
            

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_DDBIND_TAGNO", array);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DDTagNo.DataSource = ds;
                DDTagNo.DataTextField = "TagNo";
                DDTagNo.DataValueField = "TagNo";
                DDTagNo.DataBind();
                DDTagNo.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }
        else
        {
            string str = "select Distinct S.TagNo,S.Tagno from Stock S Where S.companyid=" + DDcompany.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Lotno='" + ddlLotno.Text + "' and S.Qtyinhand>0";
            if (variable.VarBINNOWISE == "1")
            {
                str = str + " and BinNo='" + DDBinNo.SelectedItem.Text + "'";
            }
            UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "--Plz Select--");
        }

       
        if (DDTagNo.Items.Count > 0)
        {
            DDTagNo.SelectedIndex = 1;
            DDTagno_SelectedIndexChanged(DDTagNo, e);
        }
    }
    protected void DDTagno_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddTagno = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddTagno.Parent.Parent;
        int index = row.RowIndex;
        int Ifinishedid = Convert.ToInt32(((Label)row.FindControl("lblifinishedid")).Text);
        Label lblstockqty = ((Label)row.FindControl("lblstockqty"));
        DropDownList ddgodown = ((DropDownList)row.FindControl("DDgodown"));
        DropDownList ddlotno = ((DropDownList)row.FindControl("DDLotNo"));
        DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));

        Double StockQty = UtilityModule.getstockQty(DDcompany.SelectedValue, ddgodown.SelectedValue, ddlotno.Text, Ifinishedid, ddTagno.Text, BinNo: (variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : ""));
        lblstockqty.Text = StockQty.ToString();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
         
        if (Session["varcompanyid"].ToString() == "21")
        {
            string status = "";
            if (TxtTotalPcs.Text == "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please enter total pcs.');", true);
                TxtTotalPcs.Focus();
                return;
            }
           
            for (int i = 0; i < DG.Rows.Count; i++)
            {
                CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
                TextBox txtissueqty = ((TextBox)DG.Rows[i].FindControl("txtissueqty"));

                if (Chkboxitem.Checked == true)   // Change when Updated Completed
                {
                    status = "1";
                }
                if (txtissueqty.Text == "" && Chkboxitem.Checked == true)   // Change when Updated Completed
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Issue qty can not be blank');", true);
                    txtissueqty.Focus();
                    return;
                }
                if (Chkboxitem.Checked == true && (Convert.ToDecimal(txtissueqty.Text) <= 0))   // Change when Updated Completed
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Issue qty always greater then zero');", true);
                    txtissueqty.Focus();
                    return;
                }


                //if (Chkboxitem.Checked == false && chkEdit.Checked == false)    // Change when Updated Completed
                //{
                //    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select all check boxes');", true);
                //    return;
                //}
                //else
                //{
                //    if (txtissueqty.Text == "" && Chkboxitem.Checked == true)   // Change when Updated Completed
                //    {
                //        ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Issue qty can not be blank');", true);
                //        txtissueqty.Focus();
                //        return;
                //    }
                //    if (Chkboxitem.Checked == true && (Convert.ToDecimal(txtissueqty.Text) <= 0))   // Change when Updated Completed
                //    {
                //        ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Issue qty always greater then zero');", true);
                //        txtissueqty.Focus();
                //        return;
                //    }
                //}
            }

            if (status == "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check boxes');", true);
                return;
            }
        }
       
        //********sql table Type
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("ifinishedid", typeof(int));
        dtrecords.Columns.Add("IUnitid", typeof(int));
        dtrecords.Columns.Add("Isizeflag", typeof(int));
        dtrecords.Columns.Add("Godownid", typeof(int));
        dtrecords.Columns.Add("Lotno", typeof(string));
        dtrecords.Columns.Add("TagNo", typeof(string));
        dtrecords.Columns.Add("issueqty", typeof(float));
        //dtrecords.Columns.Add("Noofcone", typeof(int));
        dtrecords.Columns.Add("Prorderid", typeof(int));
        dtrecords.Columns.Add("ConsmpQty", typeof(float));
        dtrecords.Columns.Add("BinNo", typeof(string));
        //*******************
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtissueqty = ((TextBox)DG.Rows[i].FindControl("txtissueqty"));
            DropDownList DDGodown = ((DropDownList)DG.Rows[i].FindControl("DDGodown"));
            DropDownList DDLotNo = ((DropDownList)DG.Rows[i].FindControl("DDLotNo"));
            DropDownList DDTagNo = ((DropDownList)DG.Rows[i].FindControl("DDTagNo"));
            DropDownList DDBinNo = ((DropDownList)DG.Rows[i].FindControl("DDBinNo"));

            if (Chkboxitem.Checked == true && (txtissueqty.Text != "") && DDGodown.SelectedIndex > 0 && DDLotNo.SelectedIndex > 0 && DDTagNo.SelectedIndex > 0)
            {
                Label lblitemfinishedid = ((Label)DG.Rows[i].FindControl("lblifinishedid"));
                Label lblunitid = ((Label)DG.Rows[i].FindControl("lbliunitid"));
                Label lblflagsize = ((Label)DG.Rows[i].FindControl("lblisizeflag"));
                string Lotno = DDLotNo.Text;
                string TagNo = DDTagNo.Text;
                //TextBox txtnoofcone = ((TextBox)DG.Rows[i].FindControl("txtnoofcone"));
                Label lblissueorderid = ((Label)DG.Rows[i].FindControl("lblissueorderid"));
                Label lblconsmpqty = ((Label)DG.Rows[i].FindControl("lblconsmpqty"));
                //*********************
                DataRow dr = dtrecords.NewRow();
                dr["ifinishedid"] = lblitemfinishedid.Text;
                dr["IUnitid"] = lblunitid.Text;
                dr["Isizeflag"] = lblflagsize.Text;
                dr["Godownid"] = DDGodown.SelectedValue;
                dr["Lotno"] = Lotno;
                dr["TagNo"] = TagNo;
                dr["IssueQty"] = txtissueqty.Text == "" ? "0" : txtissueqty.Text;
                //dr["Noofcone"] = txtnoofcone.Text == "" ? "0" : txtnoofcone.Text;
                dr["Prorderid"] = lblissueorderid.Text;
                dr["consmpqty"] = lblconsmpqty.Text;
                dr["BinNo"] = variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : "";
                dtrecords.Rows.Add(dr);
            }
        }
        //if (Session["varcompanyid"].ToString() == "21")
        //{
        //    if (chkEdit.Checked == false)     // Change when Updated Completed
        //    {
        //        if (dtrecords.Rows.Count != DG.Rows.Count)
        //        {
        //            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please check stock qty in all rows');", true);
        //            return;
        //        }
        //    }
        //}

        if (Session["varcompanyid"].ToString() == "22")
        {
            //DateTime IssueDate = Convert.ToDateTime(txtissuedate.Text.ToString());
            //DateTime FolioDate = Convert.ToDateTime(txtFolioIssueDate.Text.ToString());

            //if (IssueDate < FolioDate)
            //{
            //    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select date greater then folio date');", true);
            //    return;
            //}

            //if (chkEdit.Checked == false)     // Change when Updated Completed
            //{
            //    if (dtrecords.Rows.Count != DG.Rows.Count)
            //    {
            //        ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please check stock qty in all rows');", true);
            //        return;
            //    }
            //}
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        if (dtrecords.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[14];
                param[0] = new SqlParameter("@PrmId", SqlDbType.Int);
                //if (chkEdit.Checked == true && Session["varcompanyid"].ToString() == "21")
                //{
                //    param[0].Value = DDissueno.SelectedValue;
                //}
                //else
                //{
                    param[0].Value = 0;
              //  }

                param[0].Direction = ParameterDirection.InputOutput;
                param[1] = new SqlParameter("@companyid", DDcompany.SelectedValue);
                param[2] = new SqlParameter("@Processid", DDprocess.SelectedValue);
                param[3] = new SqlParameter("@Prorderid", DDFoliono.SelectedValue);
                param[4] = new SqlParameter("@issueDate", DateTime.Now);
                param[5] = new SqlParameter("@userid", Session["varuserid"]);
                param[6] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
                param[7] = new SqlParameter("@dtrecords", dtrecords);
                param[8] = new SqlParameter("@TranType", SqlDbType.TinyInt);
                param[8].Value = 0;
                param[9] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[9].Direction = ParameterDirection.Output;
                param[10] = new SqlParameter("@EWayBillNo", "");
                param[11] = new SqlParameter("@StockNoQty", TxtTotalPcs.Text == "" ? "0" : TxtTotalPcs.Text);
                param[12] = new SqlParameter("@FolioChallanNo", DDFoliono.SelectedIndex > 0 ? DDFoliono.SelectedItem.Text : "");
                param[13] = new SqlParameter("@CHALANNO", SqlDbType.VarChar, 50);
                param[13].Value = "";
                param[13].Direction = ParameterDirection.InputOutput;

                ///**********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveWeftIssueonLoom", param);
                //*******************
                ViewState["reportid"] = param[0].Value.ToString();
               // txtissueno.Text = param[13].Value.ToString();
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
                TxtTotalPcs.Text = "";

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
    protected void FillissueGrid()
    {
        string str = @"select dbo.F_getItemDescription(PT.Finishedid,PT.flagsize) as ItemDescription,
                    PT.Lotno,PT.TagNo,PT.IssueQuantity,PM.chalanNo,Replace(CONVERT(nvarchar(11),PM.date,106),' ','-') as IssueDate,PM.prmid,PT.Prtid,PM.prorderid,PM.processid,
                    isnull(PM.EWayBillNo,'') as EWayBillNo
                    from ProcessRawMaster PM inner join ProcessRawTran PT on PM.PRMid=PT.PRMid
                    and PM.BeamType=0 and PM.TranType=0 And PM.TypeFlag = 0 and PM.prmid=" + hnissueid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
        //if (chkEdit.Checked == true)
        //{
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        txtissueno.Text = ds.Tables[0].Rows[0]["chalanno"].ToString();
        //        txtissuedate.Text = ds.Tables[0].Rows[0]["issuedate"].ToString();
        //        TxtEWayBillNo.Text = ds.Tables[0].Rows[0]["EWayBillNo"].ToString();
        //    }
        //    else
        //    {
        //        txtissueno.Text = "";
        //        txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        //    }

        //}

    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@processid", DDprocess.SelectedValue);
        param[1] = new SqlParameter("@Prmid", hnissueid.Value);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_WeftIssuereport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\RptWeftissueonLoom.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptWeftissueonLoom.xsd";
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
        BtnUpdateStockNoQty.Visible = false;
        DDLoomNo.SelectedIndex = -1;
        DDFoliono.SelectedIndex = -1;
        TDIssueNo.Visible = false;
        DG.DataSource = null;
        DG.DataBind();
        gvdetail.DataSource = null;
        gvdetail.DataBind();
        //txtissueno.Text = "";
        //txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

        //if (variable.VarWeaverRawMaterialIssueToCompleteStatus == "1" && Session["usertype"].ToString() == "1")
        //{
        //    TDForCompleteStatus.Visible = true;
        //}

        //if (chkEdit.Checked == true)
        //{
        //    TDIssueNo.Visible = true;
        //    DDissueno.SelectedIndex = -1;
        //    if (Session["varcompanyid"].ToString() == "21" || Session["varcompanyid"].ToString() == "16" || Session["varcompanyid"].ToString() == "28")
        //    {
        //        if (variable.VarWeaverRawMaterialIssueToCompleteStatus == "1" && Session["usertype"].ToString() == "1")
        //        {
        //            TDForCompleteStatus.Visible = true;
        //            //ChkForCompleteStatus.Checked = false;
        //        }
        //    }
        //    else
        //    {
        //        if (variable.VarWeaverRawMaterialIssueToCompleteStatus == "1" && Session["usertype"].ToString() == "1")
        //        {
        //            TDForCompleteStatus.Visible = false;
        //            ChkForCompleteStatus.Checked = false;
        //        }
        //    }
           
        //    if (Session["varcompanyid"].ToString() == "21")
        //    {
        //        BtnUpdateStockNoQty.Visible = true;
        //    }
        //}
    }
    protected void DDissueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnissueid.Value = DDissueno.SelectedValue;
       // FillissueGrid();
        Fillgrid();
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

    protected void DG_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DG.EditIndex = e.NewEditIndex;
        Fillgrid();
    }
    protected void DG_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DG.EditIndex = -1;
        Fillgrid();
    }
    protected void DG_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
           // Label lblhqty = (Label)DG.Rows[e.RowIndex].FindControl("lblhqty");
            Label lblprmid = (Label)DG.Rows[e.RowIndex].FindControl("lblprmid");
            Label lblprtid = (Label)DG.Rows[e.RowIndex].FindControl("lblprtid");
           // Label lblprorderid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprorderid");
           // TextBox txtqty = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtqty");
            TextBox txttag = (TextBox)DG.Rows[e.RowIndex].FindControl("txtTagNo");
            TextBox txtlot = (TextBox)DG.Rows[e.RowIndex].FindControl("txtLotNo");
          //  Label lblprocessid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprocessid");
            //**************
            SqlParameter[] param = new SqlParameter[9];
            param[0] = new SqlParameter("@prmid", lblprmid.Text);
            param[1] = new SqlParameter("@prtid", lblprtid.Text);
            param[2] = new SqlParameter("@prorderid", 0);
            param[3] = new SqlParameter("@TAG", txttag.Text == "" ? "0" : txttag.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@LOT", txtlot.Text == "" ? "0" : txtlot.Text);
            param[6] = new SqlParameter("@processid", 0);
            param[7] = new SqlParameter("@userid", Session["varuserid"]);
            param[8] = new SqlParameter("@EWayBillNo", "");
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATETAGLOT", param);
            lblmessage.Text = param[4].Value.ToString();
            Tran.Commit();
            DG.EditIndex = -1;
            Fillgrid();
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
    protected void gvdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblhqty = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblhqty");
            Label lblprmid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprmid");
            Label lblprtid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprtid");
            Label lblprorderid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprorderid");
            TextBox txtqty = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtqty");
            Label lblprocessid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprocessid");
            //**************
            SqlParameter[] param = new SqlParameter[9];
            param[0] = new SqlParameter("@prmid", lblprmid.Text);
            param[1] = new SqlParameter("@prtid", lblprtid.Text);
            param[2] = new SqlParameter("@prorderid", lblprorderid.Text);
            param[3] = new SqlParameter("@hqty", lblhqty.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@qty", txtqty.Text == "" ? "0" : txtqty.Text);
            param[6] = new SqlParameter("@processid", lblprocessid.Text);
            param[7] = new SqlParameter("@userid", Session["varuserid"]);
            param[8] = new SqlParameter("@EWayBillNo", "");
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_updateweftissue", param);
            lblmessage.Text = param[4].Value.ToString();
            Tran.Commit();
            gvdetail.EditIndex = -1;
            FillissueGrid();
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
            Label lblprmid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprmid");
            Label lblprtid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprtid");
            Label lblprorderid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprorderid");
            Label lblprocessid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprocessid");

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@prmid", lblprmid.Text);
            param[1] = new SqlParameter("@prtid", lblprtid.Text);
            param[2] = new SqlParameter("@prorderid", lblprorderid.Text);
            param[3] = new SqlParameter("@Processid", lblprocessid.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_deleteweftissue", param);
            lblmessage.Text = param[4].Value.ToString();
            Tran.Commit();
            FillissueGrid();
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
    protected void DG_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblprmid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprmid");
            Label lblprtid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprtid");
            Label lblprorderid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprorderid");
            Label lblprocessid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprocessid");

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@prmid", lblprmid.Text);
            param[1] = new SqlParameter("@prtid", lblprtid.Text);
            param[2] = new SqlParameter("@prorderid", lblprorderid.Text);
            param[3] = new SqlParameter("@Processid", lblprocessid.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_deleteweftissue", param);
            lblmessage.Text = param[4].Value.ToString();
            Tran.Commit();
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
    protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    {
//        string str = @"select Top(1) EMP.ProcessId,EMP.EMPID From Employee_Processorderno EMP inner join EmpInfo EI on EMP.Empid=EI.EmpId
//                       Where EI.EmpCode='" + txtWeaverIdNoscan.Text + "'";

//        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
//        if (ds.Tables[0].Rows.Count > 0)
//        {
//            if (DDprocess.Items.FindByValue(ds.Tables[0].Rows[0]["Processid"].ToString()) != null)
//            {
//                DDprocess.SelectedValue = ds.Tables[0].Rows[0]["Processid"].ToString();
//                DDprocess_SelectedIndexChanged(sender, new EventArgs());
//                //***********Fill PROduction UNI
//                str = @"SELECT DISTINCT PIM.UNITS FROM PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + @" PIM INNER JOIN EMPLOYEE_PROCESSORDERNO EMP 
//                ON PIM.ISSUEORDERID=EMP.ISSUEORDERID AND EMP.PROCESSID=" + DDprocess.SelectedValue + " Where EMP.EMpid=" + ds.Tables[0].Rows[0]["empid"];

//                DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
//                if (ds1.Tables[0].Rows.Count > 0)
//                {
//                    if (DDProdunit.Items.FindByValue(ds1.Tables[0].Rows[0]["UNITS"].ToString()) != null)
//                    {
//                        DDProdunit.SelectedValue = ds1.Tables[0].Rows[0]["UNITS"].ToString();
//                        DDProdunit_SelectedIndexChanged(sender, new EventArgs());
//                    }
//                }
//                //**************
//                DDissueno.Focus();

//            }
//        }
//        else
//        {
//            ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('Employee Code does not exists in this Department.')", true);
//          //  txtWeaverIdNoscan.Focus();
//        }
    }
    protected void ChkForCompleteStatus_CheckedChanged(object sender, EventArgs e)
    {
        //if (ChkForCompleteStatus.Checked == false)
        //{
        //    DDLoomNo.SelectedIndex = -1;
        //    DDFoliono.Items.Clear();
        //    TDIssueNo.Visible = false;
        //    DG.DataSource = null;
        //    DG.DataBind();
        //    gvdetail.DataSource = null;
        //    gvdetail.DataBind();
        //    txtissueno.Text = "";
        //    txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        //}
        //else if (ChkForCompleteStatus.Checked == true)
        //{
        //    DDLoomNo.SelectedIndex = -1;
        //    DDFoliono.Items.Clear();
        //    DDLoomNo_SelectedIndexChanged(sender, new EventArgs());

        //}
    }
    protected void UpdateStockNoQty()
    {
        if (Session["varcompanyid"].ToString() == "21")
        {
            if (DDFoliono.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select folio number.');", true);
                DDFoliono.Focus();
                return;
            }
            if (DDissueno.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select number.');", true);
                DDissueno.Focus();
                return;
            }
            if (TxtTotalPcs.Text == "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please enter total pcs.');", true);
                TxtTotalPcs.Focus();
                return;
            }
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[12];
                param[0] = new SqlParameter("@PrmID", DDissueno.SelectedValue);
                param[1] = new SqlParameter("@Prorderid", DDFoliono.SelectedValue);
                param[2] = new SqlParameter("@StockNoQty", TxtTotalPcs.Text == "" ? "0" : TxtTotalPcs.Text);
                param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[3].Direction = ParameterDirection.Output;

                ///**********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateWeftStockNoOnLoom", param);
                //*******************
                if (param[3].Value.ToString() != "")
                {
                    lblmessage.Text = param[3].Value.ToString();
                    Tran.Rollback();
                }
                else
                {
                    Tran.Commit();
                    lblmessage.Text = "DATA UPDATED SUCCESSFULLY.";
                    TxtTotalPcs.Text = "";
                    DDissueno.SelectedIndex = 0;
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
    }

    protected void BtnUpdateStockNoQty_Click(object sender, EventArgs e)
    {
        btnclickflag = "";

        btnclickflag = "BtnUpdateStockNoQty";
        txtpwd.Focus();
        Popup(true);          
    }
    protected void btnPreviewStockNo_Click(object sender, EventArgs e)
    {
        if (DDFoliono.SelectedIndex > 0)
        {
            string str = @"Select LS.Issueorderid FolioNo, LS.StockNo, LS.TstockNo, IsNull(PRM.ChalanNo, '') ChallanNo 
                    From LoomStockNo LS(Nolock) 
                    LEFT JOIN ProcessRawMasterLoomStockNo PRMLS(Nolock) ON PRMLS.IssueOrderID = LS.Issueorderid AND LS.StockNo = PRMLS.StockNo 
                    LEFT JOIN ProcessRawMaster PRM(Nolock) ON PRM.PRMid = PRMLS.PRMID And PRM.TypeFlag = 0 
                    Where LS.ProcessID = 1 And LS.Issueorderid = " + DDFoliono.SelectedValue;

            if (DDissueno.SelectedIndex > 0)
            {
                str = str + " And PRMLS.PRMID = " + DDissueno.SelectedValue;
            }

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;

                GridView1.DataSource = ds;
                GridView1.DataBind();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition",
                 "attachment;filename=Folio Stock No Wise Challan" + DateTime.Now + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //Apply text style to each Row
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                //*************
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('No Record Found');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select folio number');", true);
        }
    }

    protected void btnCheck_Click(object sender, EventArgs e)
    {
        if (MySession.ProductionEditPwd == txtpwd.Text)
        {
            if (btnclickflag == "BtnUpdateStockNoQty")
            {
                UpdateStockNoQty();
            }           
            Popup(false);
        }
        else
        {
            lblmessage.Visible = true;
            lblmessage.Text = "Please Enter Correct Password..";
        }
    }
    void Popup(bool isDisplay)
    {
        StringBuilder builder = new StringBuilder();
        if (isDisplay)
        {
            builder.Append("<script>");
            builder.Append("ShowPopup();</script>");
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", builder.ToString());
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ShowPopup", builder.ToString(), false);
        }
        else
        {
            builder.Append("<script>");
            builder.Append("HidePopup();</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HidePopup", builder.ToString(), false);
        }
    }

    protected void txtFolioNo_TextChanged(object sender, EventArgs e)
    {
       
       
         //   DDFoliono_SelectedIndexChanged(sender, new EventArgs());
        if (!string.IsNullOrEmpty(txtFolioNo.Text))
        {

            DDFoliono_SelectedIndexChanged(sender, new EventArgs());
        
        }




//        string str = @"select PIM.IssueOrderId From PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + @" PIM
//                       Where PIM.IssueOrderId='" + txtFolioNo.Text + "'";

//        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
//        if (ds.Tables[0].Rows.Count > 0)
//        {
            
//            DDprocess_SelectedIndexChanged(sender, new EventArgs());
//            //***********Fill PROduction UNI
//            str = @"SELECT DISTINCT PIM.UNITS FROM PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + @" PIM Where PIM.IssueOrderID=" + ds.Tables[0].Rows[0]["IssueOrderId"];

//            DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
//            if (ds1.Tables[0].Rows.Count > 0)
//            {
//                if (DDProdunit.Items.FindByValue(ds1.Tables[0].Rows[0]["UNITS"].ToString()) != null)
//                {
//                    DDProdunit.SelectedValue = ds1.Tables[0].Rows[0]["UNITS"].ToString();
//                    DDProdunit_SelectedIndexChanged(sender, new EventArgs());
//                }
//            }
//            //**************
//            DDissueno.Focus();

//        }
//        else
//        {
//            ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('Folio No does not exists in this Department.')", true);
//            txtFolioNo.Focus();
//        }
    }
}
