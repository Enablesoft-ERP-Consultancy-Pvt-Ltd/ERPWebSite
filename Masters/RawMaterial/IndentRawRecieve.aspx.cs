using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_RawMaterial_ProcessRawRecieve : System.Web.UI.Page
{
    static int MasterCompanyId;
    static int Item_finished_id;
    static int rowindex = 0;
    static string TempIndentDate = "";
    //    string msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnsave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnsave, null) + ";");
        MasterCompanyId = Convert.ToInt16(Session["varMasterCompanyIDForERP"]);
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {

            HPRMID.Value = "0";
            string Qry = @"select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By Companyname
                         Select Distinct PROCESS_NAME_ID,process_name from PROCESS_NAME_MASTER pm inner join IndentMaster im on pm.PROCESS_NAME_ID=im.processid And pm.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by process_name 
                         Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName
                         Select VarCompanyType,VarProdCode,VARQCTYPE,flagforsampleorder From MasterSetting
                         select godownid From Modulewisegodown Where ModuleName='" + Page.Title + @"'
                         Select ID, BranchName 
                            From BRANCHMASTER BM(nolock) 
                            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"];

            DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, DSQ, 0, true, "Select Comp Name");
            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
                CompNameSelectedChanged();
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, DSQ, 5, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }
            UtilityModule.ConditionalComboFillWithDS(ref ddProcessName, DSQ, 1, true, "Select Process Name");
            UtilityModule.ConditionalComboFillWithDS(ref ddgodown, DSQ, 2, true, "Select GodownName");
            hncomp.Value = Session["varMasterCompanyIDForERP"].ToString();

            // DataRow dr = AllEnums.MasterTables.Mastersetting.ToTable().Select()[0];
            if (Convert.ToInt16(DSQ.Tables[3].Rows[0]["flagforsampleorder"]) == 1)
            {
                checkforsampleorder.Visible = true;
            }
            else
            {
                checkforsampleorder.Visible = false;
            }
            if (ddCompName.Items.Count > 0)
            {
                //    ddCompName.SelectedIndex = 1;
                CompNameSelectedChanged();
            }
            if (ddgodown.Items.Count > 0)
            {
                //ddgodown.SelectedIndex = 1;
                if (DSQ.Tables[4].Rows.Count > 0)
                {
                    if (ddgodown.Items.FindByValue(DSQ.Tables[4].Rows[0]["godownid"].ToString()) != null)
                    {
                        ddgodown.SelectedValue = DSQ.Tables[4].Rows[0]["godownid"].ToString();
                        ddgodown_SelectedIndexChanged(sender, new EventArgs());
                    }
                }
                else
                {
                    if (ddgodown.Items.FindByValue("1") != null)
                    {
                        if (Session["varMasterCompanyIDForERP"].ToString() == "30")
                        {
                            ddgodown.SelectedValue = "2";
                        }
                        else if (Session["varMasterCompanyIDForERP"].ToString() == "9")
                        {
                            ddgodown.SelectedValue = "3";
                            ddgodown.Enabled = false;
                        }
                        else
                        {
                            ddgodown.SelectedValue = "1";
                        }
                        ddgodown_SelectedIndexChanged(sender, new EventArgs());
                    }
                }

            }
            ViewState["VarCompanyType"] = DSQ.Tables[3].Rows[0]["VarCompanyType"].ToString();
            ViewState["VARQCTYPE"] = DSQ.Tables[3].Rows[0]["VARQCTYPE"].ToString();

            ViewState["VarGENRATEINDENTWITHOUTLOT_STOCK"] = variable.VarGENRATEINDENTWITHOUTLOT_STOCK;

            switch (Convert.ToInt32(DSQ.Tables[3].Rows[0]["VarProdCode"]))
            {
                case 0:
                    procode.Visible = false;
                    break;
                case 1:
                    procode.Visible = true;
                    break;
            }
            //Without BOM
            if (Session["WithoutBOM"].ToString() == "1")
            {
                TDIssuedQty.Visible = false;
                TDLotNo.Visible = false;
            }
            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "7":
                    TDIssuedQty.Visible = false;
                    TDLotNo.Visible = false;
                    TDtxtpendingQty.Visible = false;
                    txtdate.Enabled = false;
                    tdchal.Visible = false;
                    btnqcchkpreview.Visible = false;
                    chkrec.Visible = false;
                    btnpreview.Visible = false;
                    TxtPartyChallanNo.Enabled = false;
                    tdchallan.Visible = false;
                    break;
                case "10":
                    TDLotNo.Visible = false;
                    break;
                case "5":
                    TxtGateInNo.ReadOnly = true;
                    ChkForReceiveAnyColor.Visible = true;
                    break;
                case "6":
                case "12":
                    TDLShort.Visible = true;
                    TDshrinkage.Visible = true;
                    break;
                case "14":
                    TDloss.Visible = false;
                    TDreturnQty.Visible = false;
                    break;
                case "16":
                    //TxtLoss.Enabled = false;
                    TDTxtMoisture.Visible = true;
                    break;
                case "21":
                    TDGateInNo.Visible = false;
                    TDTagRemark.Visible = true;
                    break;
                case "22":
                    customlotno.Visible = true;
                    Label42.Text = "Inwards No";
                    Label14.Text = "Bill No";
                    Label19.Text = "Issue LotNo";
                    Label31.Text = "Issue TagNo";
                    break;
                case "28":
                    //TxtLoss.Enabled = false;
                    TDTxtMoisture.Visible = true;
                    break;
                case "43":
                    Label31.Text = "UCN No";
                    TDIssueQtyOnMachine.Visible = true;
                    break;
                case "45":
                    TDTagNo.Visible = true;
                    break;
            }
            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            lablechange();
            TDNoOFHank.Visible = false;
            if (MySession.TagNowise == "1")
            {
                TDTagNo.Visible = true;
            }
            if (variable.VarBINNOWISE == "1")
            {
                TDBinno.Visible = true;
            }
            if (Session["canedit"].ToString() == "1")
            {
                TDedit.Visible = true;
                TDcomplete.Visible = true;
            }
            if (variable.VarMATERIALRECEIVEWITHLEGALVENDOR == "1")
            {
                Tdlegalvendor.Visible = true;

            }

            if (variable.VarGSTForIndentRawIssue == "1")
            {
                ChkForGSTReport.Visible = true;
            }


        }
    }
    protected void Filllegalvencor()
    {
        UtilityModule.ConditionalComboFill(ref DDlegalvendor, "select EI.EmpId,EmpName from EmpInfo EI inner join EmpProcess EP on EI.EmpId=EP.EmpId where processId=" + ddProcessName.SelectedValue + " AND EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ei.empname", true, "--Select--");
    }
    protected void ddCompName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompNameSelectedChanged();
    }
    private void CompNameSelectedChanged()
    {
        if (ddProcessName.Items.Count > 0)
        {
            ddProcessName.SelectedIndex = 1;
            ProcessSelectedChange();
        }
    }
    protected void ddProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessSelectedChange();
        if (Tdlegalvendor.Visible == true)
        {
            Filllegalvencor();
        }
    }
    private void ProcessSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref ddempname, "SELECT distinct E.EmpId,E.EmpName FROM IndentMaster IM INNER JOIN  EmpInfo E ON IM.PartyId=E.EmpId And CompanyId=" + ddCompName.SelectedValue + " And IM.Processid=" + ddProcessName.SelectedValue + " order by e.EmpName", true, "Select Emp");
        HPRMID.Value = "0";

        if (ddProcessName.SelectedValue == "5")
        {
            if (variable.Carpetcompany == "1")
            {
                ChkForReDyeing.Visible = true;
            }
        }
        else
        {
            ChkForReDyeing.Checked = false;
            ChkForReDyeing.Visible = false;
        }
    }
    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmpolyeeSelectedChange();
        if (Tdlegalvendor.Visible == true)
        {
            if (DDlegalvendor.Items.FindByValue(ddempname.SelectedValue) != null)
            {
                DDlegalvendor.SelectedValue = ddempname.SelectedValue;
            }
        }
    }
    private void EmpolyeeSelectedChange()
    {
        string str, status = "";
        if (chkcomplete.Checked == true)
        {
            status = "Complete";
        }
        else
        {
            status = "Pending";
        }

        if (Convert.ToInt32(Session["varcompanyNo"]) == 42)
        {
            UtilityModule.ConditionalComboFill(ref ddgodown, @"Select GM.GodownID, GM.GodownName  
                    From GodownMaster GM(Nolock) 
                    JOIN GodownWiseEmp GE(Nolock) ON GE.GoDownID = GM.GodownID 
                    Where GE.EmpID = " + ddempname.SelectedValue + @"
                    Order By GM.GodownName ", true, "Select Order No");
        }

        if (Session["VarcompanyNo"].ToString() == "7")
        {
            str = @"select distinct IM.IndentId,IndentNo+'/'+localorder + '/'+CustomerOrderNo  
                  From IndentMaster IM inner join PP_ProcessRawtran PRT  on PRT.IndentId=IM.IndentId inner join PP_ProcessRawMaster PRM ON PRM.PRMID=PRT.PRMID inner join 
                  indentdetail id on id.IndentId=im.IndentId left outer join ordermaster om on om.orderid=id.orderid inner join orderdetail od On om.orderid=od.orderid inner join 
                  V_FinishedItemDetail v On od.Item_Finished_Id= v.Item_Finished_Id inner join UserRights_Category uc On v.CATEGORY_ID=uc.CategoryId  left outer join 
                  OrderProcessPlanning pm On om.orderid=pm.orderid                  
                  Where om.status=0 AND im.Status <> 'Cancelled' and Im.status='" + status + "' and uc.userid=" + Session["varuserid"] + @" and pm.FinalStatus=1 and 
                  PRM.ProcessId=" + ddProcessName.SelectedValue + " And PRM.CompanyId=" + ddCompName.SelectedValue + " and  PRM.EMPID=" + ddempname.SelectedValue + @" And 
                  im.BranchID = " + DDBranchName.SelectedValue + " And V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

        }
        else if (Session["varMasterCompanyIDForERP"].ToString() == "3" || Session["varMasterCompanyIDForERP"].ToString() == "10" || Session["WithoutBOM"].ToString() == "1")
        {
            str = @"select distinct IM.IndentId,IndentNo+'/'+localorder + '/'+CustomerOrderNo  
                  From IndentMaster IM inner join PP_ProcessRawtran PRT  on PRT.IndentId=IM.IndentId inner join PP_ProcessRawMaster PRM ON PRM.PRMID=PRT.PRMID inner join 
                  indentdetail id on id.IndentId=im.IndentId left outer join ordermaster om on om.orderid=id.orderid inner join orderdetail od On om.orderid=od.orderid 
                  inner join V_FinishedItemDetail v On od.Item_Finished_Id= v.Item_Finished_Id  
                  Where om.status=0 AND im.Status <> 'Cancelled' and Im.status='" + status + "'  and PRM.ProcessId=" + ddProcessName.SelectedValue + @" And 
                  PRM.CompanyId=" + ddCompName.SelectedValue + " and  PRM.EMPID=" + ddempname.SelectedValue + " And im.BranchID = " + DDBranchName.SelectedValue + @" And 
                  V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        }
        else
        {
            if (Session["VarcompanyNo"].ToString() == "6")
            {
                if (checkforsampleorder.Checked == true)
                {
                    str = @"select distinct IM.IndentId,IndentNo+'/'+localorder + '/'+CustomerOrderNo  
                  From IndentMaster IM inner join PP_ProcessRawtran PRT  on PRT.IndentId=IM.IndentId inner join PP_ProcessRawMaster PRM ON PRM.PRMID=PRT.PRMID inner join 
                  indentdetail id on id.IndentId=im.IndentId left outer join ordermaster om on om.orderid=id.orderid inner join orderdetail od On om.orderid=od.orderid 
                  inner join V_FinishedItemDetail v On od.Item_Finished_Id= v.Item_Finished_Id  
                  Where om.status=0 AND im.Status<>'Cancelled' and im.status='" + status + "'  and PRM.ProcessId=" + ddProcessName.SelectedValue + @" And 
                  PRM.CompanyId=" + ddCompName.SelectedValue + " and  PRM.EMPID=" + ddempname.SelectedValue + " And im.BranchID = " + DDBranchName.SelectedValue + @" And 
                  V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                }
                else
                {
                    str = @"Select distinct IM.IndentId,Customercode+'/'+IndentNo+'/'+om.CustomerOrderNo  from IndentMaster IM inner join PP_ProcessRawtran PRT  on PRT.IndentId=IM.IndentId 
                  inner join PP_ProcessRawMaster PRM ON PRM.PRMID=PRT.PRMID inner join indentdetail id on id.IndentId=im.IndentId and im.Status<>'Cancelled'  and Im.status='" + status + @"' left outer join ordermaster om on 
                  om.orderid=id.orderid inner join Customerinfo CI on  CI.CustomerId=Om.CustomerId Where PRM.Companyid=" + ddCompName.SelectedValue + @" And 
                    PRM.ProcessId=" + ddProcessName.SelectedValue + " And im.BranchID = " + DDBranchName.SelectedValue + " And PRM.EMPID=" + ddempname.SelectedValue;
                }
            }
            else
            {
                if (ChkEdit.Checked == true)
                {
                    str = @"Select distinct IM.IndentId,case When " + Session["varMasterCompanyIDForERP"].ToString() + @"=9 Then om.localOrder+' / '+IndentNo else IndentNo+'-'+CI.CustomerCode+'/'+om.CustomerOrderNo End  from IndentMaster IM inner join PP_ProcessRawtran PRT  on PRT.IndentId=IM.IndentId 
                  inner join PP_ProcessRawMaster PRM ON PRM.PRMID=PRT.PRMID inner join indentdetail id on id.IndentId=im.IndentId left outer join ordermaster om on 
                  om.orderid=id.orderid  Inner join Customerinfo CI On CI.CustomerId=om.CustomerId  Where PRM.Companyid=" + ddCompName.SelectedValue + @" And 
                  PRM.ProcessId=" + ddProcessName.SelectedValue + " And im.BranchID = " + DDBranchName.SelectedValue + " And PRM.EMPID=" + ddempname.SelectedValue;
                }
                else
                {
                    str = @"Select distinct IM.IndentId,case when " + Session["varMasterCompanyIDForERP"].ToString() + @"=9 Then LocalOrder+' / '+ IndentNo else IndentNo+'-'+CI.CustomerCode+'/'+om.CustomerOrderNo End  from 
                         IndentMaster IM Inner join IndentDetail Id on Im.IndentId=ID.IndentId and im.Status<>'Cancelled' and Im.status='" + status + @"'  left outer join ordermaster om on 
                         om.orderid=id.orderid  Inner join Customerinfo CI On CI.CustomerId=om.CustomerId  
                        Where IM.Companyid=" + ddCompName.SelectedValue + " And ProcessId=" + ddProcessName.SelectedValue + " And im.BranchID = " + DDBranchName.SelectedValue + @" And PartyId=" + ddempname.SelectedValue;
                }
            }
        }
        UtilityModule.ConditionalComboFill(ref ddindent, str, true, "Select Indent No");
        HPRMID.Value = "0";
    }
    protected void ddindent_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvdetail.DataSource = null;
        gvdetail.DataBind();
        IndentSelectedChange();

    }
    private void IndentSelectedChange()
    {
        string str = "";

        if (Session["varMasterCompanyIDForERP"].ToString() == "42")
        {
            DateTime EndDate=Convert.ToDateTime("2024-02-29 00:00:00.000");

            str = @"Select distinct IM.IndentId, replace(convert(varchar(11),isnull(IM.Date,''),106), ' ','-') as IndentDate from IndentMaster IM   
                        Where IM.Companyid=" + ddCompName.SelectedValue + " And ProcessId=" + ddProcessName.SelectedValue + "  and IM.IndentId=" + ddindent.SelectedValue;

            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                TempIndentDate = Ds.Tables[0].Rows[0]["IndentDate"].ToString();

                if (Convert.ToDateTime(TempIndentDate) <= EndDate)
                {
                    ViewState["VarGENRATEINDENTWITHOUTLOT_STOCK"] = "1";
                }
            }
        }

        UtilityModule.ConditionalComboFill(ref ddChallanNo, @"Select Distinct PPM.PrmId,PPM.ChallanNo from PP_ProcessRawMaster PPM,PP_ProcessRawTran PPT Where 
        PPM.PrmId=PPT.PrmId And PPM.CompanyID=" + ddCompName.SelectedValue + " And PPM.ProcessID=" + ddProcessName.SelectedValue + " And PPm.EmpId=" + ddempname.SelectedValue + " And PPT.IndentID=" + ddindent.SelectedValue, true, "Select Challan No");
    }
    protected void ddChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvdetail.DataSource = null;
        gvdetail.DataBind();
        ChallanNoSelectedChange();
    }
    private void ChallanNoSelectedChange()
    {
        ddCatagory.Items.Clear();
        string str = "Select Distinct PM.PrmId,ChallanNo From PP_ProcessRecMaster PM,PP_ProcessRecTran PT Where PM.PrmId=PT.PRtId And PM.CompanyId=0";
        if (ChkEdit.Checked == true)
        {
            str = "Select Distinct PM.PrmId,ChallanNo From PP_ProcessRecMaster PM,PP_ProcessRecTran PT Where PM.PrmId=PT.Prmid And PM.CompanyId=" + ddCompName.SelectedValue + @" And 
                  PM.Processid=" + ddProcessName.SelectedValue + " and PM.Empid=" + ddempname.SelectedValue + " And IndentId=" + ddindent.SelectedValue + @" And 
                  IssPrmId=" + ddChallanNo.SelectedValue;
        }
        UtilityModule.ConditionalComboFill(ref ddPartyChallanNo, str, true, "Select Party Challan No");
        dditemname.SelectedIndex = -1;
        fillorderdetail();
        ReceiveIssItemCheckedChanged();
    }
    protected void ChkEdit_CheckedChanged(object sender, EventArgs e)
    {
        EditCheckedChanged();

        if (Session["usertype"].ToString() != "1" && variable.VarOnlyPreviewButtonShowOnAllEditForm == "1")
        {
            btnsave.Visible = false;
            BtnUpdateStatus.Visible = false;
            gvdetail.Columns[10].Visible = false;
            gvdetail.Columns[11].Visible = false;
        }
    }
    private void EditCheckedChanged()
    {
        txtidnt.Text = "";
        if (ddProcessName.Items.Count > 0)
        {
            ddProcessName.SelectedIndex = 0;
        }
        if (ddempname.Items.Count > 0)
        {
            ddempname.SelectedIndex = 0;
        }
        txtidnt.Focus();
        if (ChkEdit.Checked == true)
        {
            TdPartyChallanNo.Visible = true;
            if (Session["varMasterCompanyIDForERP"].ToString() == "21")
            {
                TdlnkupdatebillNo.Visible = true;
            }
            if (Session["usertype"].ToString() == "1")
            {
                BtnUpdateStatus.Visible = true;
            }
        }
        else
        {
            TdlnkupdatebillNo.Visible = false;
            TdPartyChallanNo.Visible = false;
            BtnUpdateStatus.Visible = false;
        }
    }
    protected void ddPartyChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        PartyChallanNoSelectedChenge();
        ViewState["flag"] = 1;
    }
    private void PartyChallanNoSelectedChenge()
    {
        HPRMID.Value = ddPartyChallanNo.SelectedValue;
        string Str = "Select Distinct replace(convert(varchar(11),isnull(Date,''),106), ' ','-') Date,GateInNo,NoOfHank,ChallanNo,isnull(PM.Checkedby,'') as Checkedby,isnull(PM.Approvedby,'') as Approvedby,isnull(PM.BillNo,'') as BillNo  From PP_ProcessRecMaster PM,PP_ProcessRecTran PT Where PM.PrmId=" + HPRMID.Value;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            txtdate.Text = Ds.Tables[0].Rows[0]["Date"].ToString();
            TxtGateInNo.Text = Ds.Tables[0].Rows[0]["GateInNo"].ToString();
            TxtNoOFHank.Text = Ds.Tables[0].Rows[0]["NoOfHank"].ToString();
            TxtPartyChallanNo.Text = Ds.Tables[0].Rows[0]["ChallanNo"].ToString();
            txtcheckedby.Text = Ds.Tables[0].Rows[0]["checkedby"].ToString();
            txtapprovedby.Text = Ds.Tables[0].Rows[0]["approvedby"].ToString();
            txtBillNo.Text = Ds.Tables[0].Rows[0]["BillNo"].ToString();
            Fill_Grid();
            qulitychk.Visible = true;
        }
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
    }
    private void ddlcategorycange()
    {
        dquality.Items.Clear();
        dddesign.Items.Clear();
        ddcolor.Items.Clear();
        ddshape.Items.Clear();
        ddsize.Items.Clear();
        ddlshade.Items.Clear();
        string str = "";
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        if (ChkForReceiveIssItem.Checked == true)
        {
            //            UtilityModule.ConditionalComboFill(ref dditemname, @"SELECT DISTINCT VF.ITEM_ID,VF.ITEM_NAME From IndentDetail ID,V_FinishedItemDetail VF
            //            Where ID.IFinishedId=VF.ITEM_FINISHED_ID And ID.IndentId=" + ddindent.SelectedValue + " And VF.CATEGORY_ID=" + ddCatagory.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select Item--");

            str = @"select DISTINCT VF.ITEM_ID,VF.ITEM_NAME from PP_ProcessRawMaster PM inner join PP_ProcessRawTran PT
                        on PM.PRMid=PT.PRMid inner join IndentMaster IM on IM.IndentID=pt.Indentid
                        inner join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID
                        Where IM.IndentId=" + ddindent.SelectedValue + " and vf.CATEGORY_ID=" + ddCatagory.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            UtilityModule.ConditionalComboFill(ref dditemname, str, true, "--Select Item--");

        }
        else
        {
            UtilityModule.ConditionalComboFill(ref dditemname, @"SELECT DISTINCT VF.ITEM_ID,VF.ITEM_NAME From IndentDetail ID,V_FinishedItemDetail VF
            Where ID.OFinishedId=VF.ITEM_FINISHED_ID And ID.IndentId=" + ddindent.SelectedValue + " And VF.CATEGORY_ID=" + ddCatagory.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select Item--");
        }
        string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                      " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                      " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCatagory.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        ql.Visible = true;
                        break;
                    case "2":
                        dsn.Visible = true;
                        if (ChkForReceiveIssItem.Checked == true)
                        {
                            //                            UtilityModule.ConditionalComboFill(ref dddesign, @"SELECT DISTINCT dbo.Design.designId, dbo.Design.designName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                            //                            dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                            //                            dbo.Design ON dbo.ITEM_PARAMETER_MASTER.DESIGN_ID = dbo.Design.designId
                            //                            WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select Design--");

                            str = @"select DISTINCT VF.designId,VF.designName from PP_ProcessRawMaster PM inner join PP_ProcessRawTran PT
                                on PM.PRMid=PT.PRMid inner join IndentMaster IM on IM.IndentID=pt.Indentid
                                inner join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID
                                Where  IM.IndentId=" + ddindent.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                            UtilityModule.ConditionalComboFill(ref dddesign, str, true, "--Select Design--");


                        }
                        else
                        {
                            UtilityModule.ConditionalComboFill(ref dddesign, @"SELECT DISTINCT dbo.Design.designId, dbo.Design.designName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                            dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.OFinishedId INNER JOIN
                            dbo.Design ON dbo.ITEM_PARAMETER_MASTER.DESIGN_ID = dbo.Design.designId
                            WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select Design--");
                        }
                        break;
                    case "3":
                        clr.Visible = true;
                        if (ChkForReceiveIssItem.Checked == true)
                        {
                            //                            UtilityModule.ConditionalComboFill(ref ddcolor, @"SELECT DISTINCT dbo.color.ColorId, dbo.color.ColorName FROm dbo.ITEM_PARAMETER_MASTER INNER JOIN
                            //                            dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                            //                            dbo.color ON dbo.ITEM_PARAMETER_MASTER.COLOR_ID = dbo.color.ColorId
                            //                            WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select Color--");

                            str = @"select DISTINCT VF.ColorId,VF.ColorName from PP_ProcessRawMaster PM inner join PP_ProcessRawTran PT
                                on PM.PRMid=PT.PRMid inner join IndentMaster IM on IM.IndentID=pt.Indentid
                                inner join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID
                                Where  IM.IndentId=" + ddindent.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

                            UtilityModule.ConditionalComboFill(ref ddcolor, str, true, "--Select Color--");
                        }
                        else
                        {
                            UtilityModule.ConditionalComboFill(ref ddcolor, @"SELECT DISTINCT dbo.color.ColorId, dbo.color.ColorName FROm dbo.ITEM_PARAMETER_MASTER INNER JOIN
                            dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.OFinishedId INNER JOIN
                            dbo.color ON dbo.ITEM_PARAMETER_MASTER.COLOR_ID = dbo.color.ColorId
                            WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select Color--");
                        }
                        break;
                    case "4":
                        shp.Visible = true;
                        if (ChkForReceiveIssItem.Checked == true)
                        {
                            //                            UtilityModule.ConditionalComboFill(ref ddshape, @"SELECT DISTINCT dbo.Shape.ShapeId, dbo.Shape.ShapeName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                            //                            dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                            //                            dbo.Shape ON dbo.ITEM_PARAMETER_MASTER.SHAPE_ID = dbo.Shape.ShapeId
                            //                            WHERE dbo.IndentDetail.IndentId = " + ddindent.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select Shape--");

                            str = @"select DISTINCT VF.ShapeId,VF.ShapeName from PP_ProcessRawMaster PM inner join PP_ProcessRawTran PT
                                on PM.PRMid=PT.PRMid inner join IndentMaster IM on IM.IndentID=pt.Indentid
                                inner join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID
                                Where  IM.IndentId=" + ddindent.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

                            UtilityModule.ConditionalComboFill(ref ddshape, str, true, "--Select Shape--");
                        }
                        else
                        {
                            UtilityModule.ConditionalComboFill(ref ddshape, @"SELECT DISTINCT dbo.Shape.ShapeId, dbo.Shape.ShapeName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                            dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.OFinishedId INNER JOIN
                            dbo.Shape ON dbo.ITEM_PARAMETER_MASTER.SHAPE_ID = dbo.Shape.ShapeId
                            WHERE dbo.IndentDetail.IndentId = " + ddindent.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select Shape--");
                        }
                        break;
                    case "5":
                        sz.Visible = true;
                        string strSize;
                        if (DDsizetype.SelectedIndex == 0)
                        {
                            strSize = " Sizeft";
                        }
                        else if (DDsizetype.SelectedIndex == 1)
                        {
                            strSize = "Sizemtr";
                        }
                        else
                        {
                            strSize = "Sizeinch";
                        }
                        if (ChkForReceiveIssItem.Checked == true)
                        {
                            //                            UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size." + strSize + @" FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                            //                        dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                            //                        dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
                            //                        WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Size in Ft");

                            str = @"select DISTINCT S.SizeId, S." + strSize + @" from PP_ProcessRawMaster PM inner join PP_ProcessRawTran PT
                                    on PM.PRMid=PT.PRMid inner join IndentMaster IM on IM.IndentID=pt.Indentid
                                    inner join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID
                                    inner join size S on s.SizeId=vf.SizeId
                                    Where  IM.IndentId=" + ddindent.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                            UtilityModule.ConditionalComboFill(ref ddsize, str, true, "--Select Size--");
                        }
                        else
                        {
                            UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size." + strSize + @" FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                        dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.OFinishedId INNER JOIN
                        dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
                        WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Size in Ft");
                        }
                        break;
                    case "6":
                        shd.Visible = true;
                        break;
                    case "10":
                        clr.Visible = true;
                        if (ChkForReceiveIssItem.Checked == true)
                        {
                            //                            UtilityModule.ConditionalComboFill(ref ddcolor, @"SELECT DISTINCT dbo.color.ColorId, dbo.color.ColorName FROm dbo.ITEM_PARAMETER_MASTER INNER JOIN
                            //                            dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                            //                            dbo.color ON dbo.ITEM_PARAMETER_MASTER.COLOR_ID = dbo.color.ColorId
                            //                            WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select Color--");

                            str = @"select DISTINCT VF.ColorId,VF.ColorName from PP_ProcessRawMaster PM inner join PP_ProcessRawTran PT
                                on PM.PRMid=PT.PRMid inner join IndentMaster IM on IM.IndentID=pt.Indentid
                                inner join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID
                                Where  IM.IndentId=" + ddindent.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

                            UtilityModule.ConditionalComboFill(ref ddcolor, str, true, "--Select Color--");
                        }
                        else
                        {
                            UtilityModule.ConditionalComboFill(ref ddcolor, @"SELECT DISTINCT dbo.color.ColorId, dbo.color.ColorName FROm dbo.ITEM_PARAMETER_MASTER INNER JOIN
                            dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.OFinishedId INNER JOIN
                            dbo.color ON dbo.ITEM_PARAMETER_MASTER.COLOR_ID = dbo.color.ColorId
                            WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select Color--");
                        }
                        break;
                }
            }
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        FILL_ITEM_CHANGED();
        QCShowORNot();
    }
    private void FILL_ITEM_CHANGED()
    {
        txtcode.Text = "";
        string str = "";
        UtilityModule.ConditionalComboFill(ref ddlunit, "SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + dditemname.SelectedValue + " And i.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Unit");
        if (ddlunit.Items.Count > 0)
        {
            ddlunit.SelectedIndex = 1;
        }
        ddlshade.Items.Clear();
        if (ChkForReceiveIssItem.Checked == true)
        {
            //            UtilityModule.ConditionalComboFill(ref dquality, @"select Distinct Vf.QualityId,Vf.QualityName From IndentDetail Id,V_FinishedItemDetail Vf
            //            Where ID.IFInishedid=VF.Item_Finished_id And ID.IndentId=" + ddindent.SelectedValue + " And Vf.Item_Id=" + dditemname.SelectedValue + " And Vf.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Quality");

            str = @"select DISTINCT VF.QualityId,VF.QualityName from PP_ProcessRawMaster PM inner join PP_ProcessRawTran PT
                  on PM.PRMid=PT.PRMid inner join IndentMaster IM on IM.IndentID=pt.Indentid
                  inner join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID
                  Where IM.IndentId=" + ddindent.SelectedValue + " and Vf.Item_Id=" + dditemname.SelectedValue + " And Vf.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

            UtilityModule.ConditionalComboFill(ref dquality, str, true, "Select Quality");



        }
        else
        {
            UtilityModule.ConditionalComboFill(ref dquality, @"SELECT DISTINCT VF.QualityId,VF.QualityName From IndentDetail ID,V_FinishedItemDetail VF
        Where ID.OFinishedId=VF.ITEM_FINISHED_ID And ID.IndentId=" + ddindent.SelectedValue + " And VF.ITEM_ID=" + dditemname.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Quallity");
        }
    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        FILL_QUANTITYCHANGE();
    }
    private void FILL_QUANTITYCHANGE()
    {
        if (ChkForReceiveIssItem.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddlshade, @"SELECT DISTINCT dbo.ShadeColor.ShadecolorId, dbo.ShadeColor.ShadeColorName FROM   dbo.ITEM_PARAMETER_MASTER INNER JOIN
            dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN  dbo.ShadeColor ON dbo.ITEM_PARAMETER_MASTER.SHADECOLOR_ID = dbo.ShadeColor.ShadecolorId
            WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " and dbo.ITEM_PARAMETER_MASTER.Item_Id=" + dditemname.SelectedValue + " and dbo.ITEM_PARAMETER_MASTER.Quality_Id=" + dquality.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Plz Select--");
        }
        else if (ChkForReceiveAnyColor.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddlshade, @"SELECT DISTINCT ShadecolorId,ShadeColorName FROM ShadeColor Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By ShadeColorName", true, "Select ShadeColor");
        }
        else
        {
            if (variable.Carpetcompany == "1")
            {
                string str = "";
                if (ViewState["VarGENRATEINDENTWITHOUTLOT_STOCK"].ToString() == "1")
                {
                    str = @" select Distinct Vf.ShadecolorId,Vf.ShadeColorName From Indentdetail ID inner Join V_FinishedItemDetail vf on ID.OFinishedId=vf.ITEM_FINISHED_ID
                            where ID.IndentId=" + ddindent.SelectedValue + " and vf.ITEM_ID=" + dditemname.SelectedValue + " and Vf.QualityId=" + dquality.SelectedValue + " order by ShadeColorName";
                }
                else
                {
                    str = @" select Distinct Vf.ShadecolorId,Vf.ShadeColorName From Indentdetail ID inner Join V_FinishedItemDetail vf on ID.OFinishedId=vf.ITEM_FINISHED_ID
                            where ID.IndentId=" + ddindent.SelectedValue + " and Vf.QualityId=" + dquality.SelectedValue + @" and LotNo in(
                            select PRT.LotNo From PP_ProcessRawMaster PRM inner join PP_ProcessRawTran PRT on PRM.PRMid=PRT.PRMid Where PRM.PRMid=" + ddChallanNo.SelectedValue + ")";
                }
                UtilityModule.ConditionalComboFill(ref ddlshade, str, true, "Select ShadeColor");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref ddlshade, @"SELECT DISTINCT dbo.ShadeColor.ShadecolorId, dbo.ShadeColor.ShadeColorName FROM   dbo.ITEM_PARAMETER_MASTER INNER JOIN
            dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.OFinishedId INNER JOIN  dbo.ShadeColor ON dbo.ITEM_PARAMETER_MASTER.SHADECOLOR_ID = dbo.ShadeColor.ShadecolorId
            WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " and dbo.ITEM_PARAMETER_MASTER.Item_Id=" + dditemname.SelectedValue + " and dbo.ITEM_PARAMETER_MASTER.Quality_Id=" + dquality.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select ShadeColor");
            }
        }
        fill_qty();
    }
    protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_qty();
    }
    protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_qty();
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_qty();
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_qty();
    }
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {

        fill_qty();
        if (Session["varcompanyno"].ToString() == "4")
        {
            GetDyingTypes();
        }
        if (TDRECSHADE.Visible == true)
        {
            if (ViewState["VarGENRATEINDENTWITHOUTLOT_STOCK"].ToString() == "1")
            {
                UtilityModule.ConditionalComboFill(ref DDRECSHADE, @" select Distinct ID.ofinishedid,Vf.ShadeColorName From Indentdetail ID inner Join V_FinishedItemDetail vf on ID.OFinishedId=vf.ITEM_FINISHED_ID
                                                     where ID.IndentId=" + ddindent.SelectedValue + " and Vf.QualityId=" + dquality.SelectedValue + @" ", true, "Select ShadeColor");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDRECSHADE, @" select Distinct ID.ofinishedid,Vf.ShadeColorName From Indentdetail ID inner Join V_FinishedItemDetail vf on ID.OFinishedId=vf.ITEM_FINISHED_ID
                                                     where ID.IndentId=" + ddindent.SelectedValue + " and Vf.QualityId=" + dquality.SelectedValue + @" and LotNo in(
                                                     select PRT.LotNo From PP_ProcessRawMaster PRM inner join PP_ProcessRawTran PRT on PRM.PRMid=PRT.PRMid Where PRM.PRMid=" + ddChallanNo.SelectedValue + ")", true, "Select ShadeColor");
            }
        }
    }
    private void fill_qty()
    {
        int color = 0;
        int quality = 0;
        int design = 0;
        int shape = 0;
        int size = 0;
        int shadeColor = 0;

        if ((ql.Visible == true && dquality.SelectedIndex > 0) || ql.Visible != true)
        {
            quality = 1;
        }
        if (dsn.Visible == true && dddesign.SelectedIndex > 0 || dsn.Visible != true)
        {
            design = 1;
        }
        if (clr.Visible == true && ddcolor.SelectedIndex > 0 || clr.Visible != true)
        {
            color = 1;
        }
        if (shp.Visible == true && ddshape.SelectedIndex > 0 || shp.Visible != true)
        {
            shape = 1;
        }
        if (sz.Visible == true && ddsize.SelectedIndex > 0 || sz.Visible != true)
        {
            size = 1;
        }
        if (shd.Visible == true && ddlshade.SelectedIndex > 0 || shd.Visible != true)
        {
            shadeColor = 1;
        }
        if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
        {
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, txtcode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            Item_finished_id = Varfinishedid;
            hnfinishedid.Value = Item_finished_id.ToString();

            FillLotNo(Item_finished_id);

            if (DDLotNo.Items.Count > 0)
            {
                DDLotNo.SelectedIndex = 1;
                if (TDTagNo.Visible == true)
                {
                    FillTagNo(Item_finished_id);
                    if (DDTagNo.Items.Count > 0)
                    {
                        DDTagNo.SelectedIndex = 1;
                        txttagNo.Text = DDTagNo.SelectedItem.Text;
                    }
                }
            }
            if (DDOrderNo.Items.Count > 0)
            {
                DDOrderNo.SelectedIndex = 1;
                fill_Order_qty();

            }
        }
        else
        {
            DDOrderNo.Items.Clear();
            txtrec.Text = "";
            txtprerec.Text = "";
        }
        //Fill dyingtypes

    }
    protected void FillLotNo(int Varfinishedid)
    {
        if (ChkForReceiveIssItem.Checked == true || ChkForReceiveAnyColor.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDOrderNo, @"Select Distinct ID.OrderId,LocalOrder+' / '+Customerorderno as OrderNo  
                From IndentDetail ID,OrderMaster OM Where OM.OrderId=ID.OrderId And IndentId=" + ddindent.SelectedValue, true, "--select--");
            if (variable.Carpetcompany == "1")
            {
                string str = @"Select Distinct PRT.LotNo,PRT.LotNo From IndentDetail ID,PP_ProcessRawtran PRT 
                Where ID.IndentId=PRT.IndentId And PRT.IndentId=" + ddindent.SelectedValue + " And PRMid=" + ddChallanNo.SelectedValue;

                if (ChkForReceiveAnyColor.Checked == false)
                {
                    str = str + @" And PRT.finishedid = " + Varfinishedid;
                }
                UtilityModule.ConditionalComboFill(ref DDLotNo, str, true, "--Select--");
            }
            else
            {

                UtilityModule.ConditionalComboFill(ref DDLotNo, @"Select Distinct PRT.LotNo,PRT.LotNo From IndentDetail ID,PP_ProcessRawtran PRT 
                Where ID.IndentId=PRT.IndentId And PRT.IndentId=" + ddindent.SelectedValue + " And PRMid=" + ddChallanNo.SelectedValue, true, "--Select--");
            }
        }
        else
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "7" || Session["varMasterCompanyIDForERP"].ToString() == "3" || Session["varMasterCompanyIDForERP"].ToString() == "10" || checkforsampleorder.Checked == true || Session["WithoutBOM"].ToString() == "1")
            {
                UtilityModule.ConditionalComboFill(ref DDOrderNo, @"select distinct OM.OrderId,LocalOrder+' / '+customerorderno as OrderNo From IndentDetail ID inner join IndentMaster IM on IM.IndentId=ID.IndentId inner join OrderMaster OM on OM.OrderId=ID.OrderId inner join 
                PP_ProcessRawtran PRT on ID.IndentId=PRT.IndentId where ID.OFinishedId=" + Varfinishedid + " and PRT.IndentId=" + ddindent.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--select--");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDOrderNo, @"Select distinct PPC.OrderId,LocalOrder+' / '+customerorderno as OrderNo From PP_Consumption PPC inner join 
                IndentDetail ID on ID.PPNO=PPC.PPID inner join IndentMaster IM on IM.IndentId=ID.IndentId inner join OrderMaster OM on OM.OrderId=PPC.OrderId inner join 
                PP_ProcessRawtran PRT on ID.IndentId=PRT.IndentId where PPC.FinishedId=" + Varfinishedid + " and PRT.IndentId=" + ddindent.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select--");
            }
            string str = "";
            if (variable.Carpetcompany == "1")
            {
                if (ViewState["VarGENRATEINDENTWITHOUTLOT_STOCK"].ToString() == "1")
                {
                    str = @"SELECT DISTINCT PRT.LOTNO,PRT.LOTNO FROM INDENTDETAIL ID INNER JOIN PP_PROCESSRAWTRAN PRT ON ID.INDENTID=PRT.INDENTID
                                AND ID.IFINISHEDID=PRT.FINISHEDID 
                                WHERE ID.INDENTID=" + ddindent.SelectedValue + " AND PRT.PRMID=" + ddChallanNo.SelectedValue + "  AND ID.OFINISHEDID=" + Varfinishedid;
                }
                else
                {
                    str = @"Select Distinct ID.LotNo,ID.LotNo From IndentDetail ID,PP_ProcessRawtran PRT 
                            Where ID.IndentId=PRT.IndentId And PRT.IndentId=" + ddindent.SelectedValue + " And PRMid=" + ddChallanNo.SelectedValue + @" And 
                            ID.OFinishedid=" + Varfinishedid;
                }
            }
            else
            {
                str = @"Select Distinct ID.LotNo,ID.LotNo From IndentDetail ID,PP_ProcessRawtran PRT 
            Where ID.IndentId=PRT.IndentId And PRT.IndentId=" + ddindent.SelectedValue + " And PRMid=" + ddChallanNo.SelectedValue + @" And 
            ID.OFinishedid=" + Varfinishedid;
            }
            UtilityModule.ConditionalComboFill(ref DDLotNo, str, true, "--Select--");
        }
    }

    protected void FillTagNo(int Varfinishedid)
    {
        string Str = "";
        if (ChkForReceiveIssItem.Checked == true || ChkForReceiveAnyColor.Checked == true)
        {
            if (variable.Carpetcompany == "1")
            {
                if (ViewState["VarGENRATEINDENTWITHOUTLOT_STOCK"].ToString() == "1")
                {
                    Str = @"Select Distinct PRt.TagNo,PRT.TagNo From IndentDetail ID,PP_ProcessRawtran PRT 
                Where ID.IndentId=PRT.IndentId And PRT.IndentId=" + ddindent.SelectedValue + " And PRMid=" + ddChallanNo.SelectedValue + " and PRT.LotNo='" + DDLotNo.SelectedItem.Text + "'  and PRT.FInishedid=" + hnfinishedid.Value + "";

                }
                else
                {
                    Str = @"Select Distinct PRt.TagNo,PRT.TagNo From IndentDetail ID,PP_ProcessRawtran PRT 
                    Where ID.IndentId=PRT.IndentId And PRT.IndentId=" + ddindent.SelectedValue + " And PRMid=" + ddChallanNo.SelectedValue + " and PRT.LotNo='" + DDLotNo.SelectedItem.Text + "' and ID.Lotno='" + DDLotNo.SelectedItem.Text + "'";
                    
                    if (ChkForReceiveAnyColor.Checked == false)
                    {
                        Str = Str + " and PRT.FInishedid=" + hnfinishedid.Value;
                    }
                }
            }
            else
            {
                Str = @"Select Distinct PRt.TagNo,PRT.TagNo From IndentDetail ID,PP_ProcessRawtran PRT 
               Where ID.IndentId=PRT.IndentId And PRT.IndentId=" + ddindent.SelectedValue + " And PRMid=" + ddChallanNo.SelectedValue + " and PRT.LotNo='" + DDLotNo.SelectedItem.Text + "' and ID.Lotno='" + DDLotNo.SelectedItem.Text + "'";
            }
            UtilityModule.ConditionalComboFill(ref DDTagNo, Str, true, "--Select--");
        }
        else
        {
            string str = "";
            if (variable.Carpetcompany == "1")
            {
                if (ViewState["VarGENRATEINDENTWITHOUTLOT_STOCK"].ToString() == "1")
                {
                    str = @"SELECT DISTINCT PRT.TagNO,PRT.TagNO FROM INDENTDETAIL ID INNER JOIN PP_PROCESSRAWTRAN PRT ON ID.INDENTID=PRT.INDENTID
                                AND ID.IFINISHEDID=PRT.FINISHEDID 
                                WHERE ID.INDENTID=" + ddindent.SelectedValue + " AND PRT.PRMID=" + ddChallanNo.SelectedValue + "  AND ID.OFINISHEDID=" + Varfinishedid + " and Prt.Lotno='" + DDLotNo.SelectedItem.Text + "'";
                }
                else
                {
                    str = @"Select Distinct ID.TagNo,ID.TagNo From IndentDetail ID,PP_ProcessRawtran PRT 
                      Where ID.IndentId=PRT.IndentId And PRT.IndentId=" + ddindent.SelectedValue + " And PRMid=" + ddChallanNo.SelectedValue + @" And 
                      ID.OFinishedid=" + Varfinishedid + " and ID.Lotno='" + DDLotNo.SelectedItem.Text + "'";
                }

            }
            else
            {
                str = @"Select Distinct ID.TagNo,ID.TagNo From IndentDetail ID,PP_ProcessRawtran PRT 
                      Where ID.IndentId=PRT.IndentId And PRT.IndentId=" + ddindent.SelectedValue + " And PRMid=" + ddChallanNo.SelectedValue + @" And 
                      ID.OFinishedid=" + Varfinishedid + " and ID.Lotno='" + DDLotNo.SelectedItem.Text + "'";
            }
            UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "--Select--");
        }
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_Order_qty();
    }
    private void fill_Order_qty()
    {
        TxtIndentQty.Text = "0";
        TxtIssueQty.Text = "0";
        txtprerec.Text = "0";
        if (DDOrderNo.SelectedIndex > 0)
        {
            int color = 0;
            int quality = 0;
            int design = 0;
            int shape = 0;
            int size = 0;
            int shadeColor = 0;
            if ((ql.Visible == true && dquality.SelectedIndex > 0) || ql.Visible != true)
            {
                quality = 1;
            }
            if (dsn.Visible == true && dddesign.SelectedIndex > 0 || dsn.Visible != true)
            {
                design = 1;
            }
            if (clr.Visible == true && ddcolor.SelectedIndex > 0 || clr.Visible != true)
            {
                color = 1;
            }
            if (shp.Visible == true && ddshape.SelectedIndex > 0 || shp.Visible != true)
            {
                shape = 1;
            }
            if (sz.Visible == true && ddsize.SelectedIndex > 0 || sz.Visible != true)
            {
                size = 1;
            }
            if (shd.Visible == true && ddlshade.SelectedIndex > 0 || shd.Visible != true)
            {
                shadeColor = 1;
            }
            if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
            {
                //if (ChkForReceiveIssItem.Checked == false)
                //{
                int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, txtcode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                hnfinishedid.Value = Varfinishedid.ToString();
                SqlParameter[] arr = new SqlParameter[13];

                arr[0] = new SqlParameter("@IndentID", SqlDbType.Int);
                arr[1] = new SqlParameter("@PRMID", SqlDbType.Int);
                arr[2] = new SqlParameter("@FinishedID", SqlDbType.Int);
                arr[3] = new SqlParameter("@IndentQty", SqlDbType.Float);
                arr[4] = new SqlParameter("@IssueQty", SqlDbType.Float);
                arr[5] = new SqlParameter("@RecQty", SqlDbType.Float);
                arr[6] = new SqlParameter("@LotNo", SqlDbType.NVarChar, 50);
                arr[7] = new SqlParameter("@VarReceiveIssItemFlag", SqlDbType.Int);
                arr[8] = new SqlParameter("@RetQty", SqlDbType.Float);
                arr[9] = new SqlParameter("@TagNo", SqlDbType.VarChar, 50);
                arr[10] = new SqlParameter("@challanwiserec", SqlDbType.Float);
                arr[10].Direction = ParameterDirection.Output;
                arr[11] = new SqlParameter("@OFInishedid", SqlDbType.Int);
                arr[12] = new SqlParameter("@Moisture", SqlDbType.Int);
                arr[12].Direction = ParameterDirection.Output;

                arr[0].Value = ddindent.SelectedValue;
                arr[1].Value = ddChallanNo.SelectedValue;
                arr[2].Value = Varfinishedid;
                arr[3].Direction = ParameterDirection.Output;
                arr[4].Direction = ParameterDirection.Output;
                arr[5].Direction = ParameterDirection.Output;
                arr[8].Direction = ParameterDirection.Output;
                arr[6].Value = TDLotNo.Visible == true ? DDLotNo.SelectedValue : "Without Lot No";
                if (ChkForReceiveIssItem.Checked == true)
                {
                    arr[7].Value = 1;
                }
                else if (ChkForReceiveAnyColor.Checked == true)
                {
                    arr[7].Value = 2;
                }
                else
                {
                    arr[7].Value = 0;
                }
                arr[9].Value = ((TDTagNo.Visible == true && DDTagNo.Items.Count > 0) ? DDTagNo.SelectedItem.Text : "Without Tag No");
                int oFinishedid = 0;
                if (TDRECSHADE.Visible = true && ChkForReceiveIssItem.Checked == true)
                {
                    oFinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, txtcode, DDRECSHADE, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                }
                arr[11].Value = oFinishedid;
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_PP_Indent_Iss_Rec_Qty", arr);

                TxtIndentQty.Text = arr[3].Value.ToString();
                TxtIssueQty.Text = arr[4].Value.ToString();
                txtprerec.Text = arr[5].Value.ToString();
                TxtMoisture.Text = arr[12].Value.ToString();

                Double challanwiserec = (Double)arr[10].Value;

                if (ChkForReceiveIssItem.Checked == true)
                {
                    txtpending.Text = Math.Round((Convert.ToDouble(TxtIssueQty.Text) - Convert.ToDouble(txtprerec.Text)), 3).ToString();
                }
                else
                {
                    txtpending.Text = Math.Round((Convert.ToDouble(TxtIndentQty.Text) - Convert.ToDouble(txtprerec.Text)), 3).ToString();

                }
                txtrec.Focus();
                //if (variable.Carpetcompany == "1")
                //{
                //    txtrec.Text = (Convert.ToDouble(TxtIssueQty.Text) - challanwiserec).ToString();
                //    txtrec_TextChanged(txtrec, new EventArgs());
                //}
                txtretrn.Text = arr[8].Value.ToString();
            }
            else
            {
                DDOrderNo.Items.Clear();
                txtrec.Text = "";
                txtprerec.Text = "";
            }
        }
        else
        {
            txtrec.Text = "";
            txtprerec.Text = "";
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        validate();
        if (LblErrorMessage.Visible == false && LblErrorMessage.Text == "")
        {
            //fill_qty();
            if (ChkForReceiveIssItem.Checked == true && variable.Carpetcompany == "1")
            {
                CheckIssRecPendingqty();
            }
            else
            {
                Check_Qty();
            }
            save_detail();
        }
    }
    private void validate()
    {

        string str = null;
        LblErrorMessage.Text = "";
        LblErrorMessage.Visible = false;
        try
        {
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, txtcode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));

            string VarLotNo = TDLotNo.Visible == true ? DDLotNo.SelectedValue : "Without Lot No";
            string TagNo = TDTagNo.Visible == false ? "Without Tag No" : (txttagNo.Text == "" ? "Without Tag No" : txttagNo.Text.Trim());
            int VarReceiveIssItem = ChkForReceiveIssItem.Checked == true ? 1 : 0;
            double VarRecQty = (txtrec.Text == "" ? 0 : Convert.ToDouble(txtrec.Text)) - (TxtBellWeight.Text == "" ? 0 : Convert.ToDouble(TxtBellWeight.Text));
            #region DuplicateEntry
            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "16":
                case "28":
                case "42":
                    break;
                default:
                    str = "Select isnull(FinishedId,0) From PP_ProcessRectran Where PRMID=" + HPRMID.Value + " And Finishedid=" + Varfinishedid + " And IndentId=" + ddindent.SelectedValue + " And LotNo='" + VarLotNo + "' And Rec_Iss_ItemFlag=" + VarReceiveIssItem + " and TagNo='" + TagNo + "' and issprmid=" + ddChallanNo.SelectedValue + "";
                    if (TDRECSHADE.Visible == true)
                    {
                        str = str + " and Rec_ofinishedid=" + DDRECSHADE.SelectedValue;
                    }

                    if (btnsave.Text == "Update")
                    {
                        str = str + "  And PRtid !=" + gvdetail.SelectedDataKey.Value;
                    }
                    int a = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str));

                    if (a != 0)
                    {
                        LblErrorMessage.Visible = true;
                        LblErrorMessage.Text = "Duplicate Entry..........";

                    }
                    break;
            }

            #endregion
            //********Check Bin No Condition
            if (variable.VarBINNOWISE == "1" && variable.VarCHECKBINCONDITION == "1")
            {
                SqlParameter[] arr = new SqlParameter[7];
                arr[0] = new SqlParameter("@BinNo", ddbinno.SelectedItem.Text);
                arr[1] = new SqlParameter("@godownid", ddgodown.SelectedValue);
                arr[2] = new SqlParameter("@ItemFinishedid", Varfinishedid);
                arr[3] = new SqlParameter("@QTY", VarRecQty);
                arr[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                arr[4].Direction = ParameterDirection.Output;
                arr[5] = new SqlParameter("@TagNo", TagNo);
                arr[6] = new SqlParameter("@LotNo", DDLotNo.SelectedItem.Text);
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_BINNOVALIDATE", arr);
                if (arr[4].Value.ToString() != "")
                {
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = arr[4].Value.ToString();
                }

            }
            //********
        }
        catch (Exception ex)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
    }
    

    protected void save_detail()
    {
        if (txtrec.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                SqlParameter[] arr = new SqlParameter[55];
                arr[0] = new SqlParameter("@prmid", SqlDbType.Int);
                arr[1] = new SqlParameter("@companyid", SqlDbType.Int);
                arr[2] = new SqlParameter("@empid", SqlDbType.Int);
                arr[3] = new SqlParameter("@processid", SqlDbType.Int);
                arr[4] = new SqlParameter("@issuedate", SqlDbType.DateTime);
                arr[5] = new SqlParameter("@prtid", SqlDbType.Int);
                arr[6] = new SqlParameter("@finishedId", SqlDbType.Int);
                arr[7] = new SqlParameter("@godownId", SqlDbType.Int);
                arr[8] = new SqlParameter("@RecQuantity", SqlDbType.Float);
                arr[9] = new SqlParameter("@indentid", SqlDbType.Int);
                arr[10] = new SqlParameter("@UnitId", SqlDbType.Int);
                arr[11] = new SqlParameter("@IssPrmID", SqlDbType.Int);
                arr[12] = new SqlParameter("@IssPrtID", SqlDbType.Int);
                arr[13] = new SqlParameter("@ID", SqlDbType.Int);
                arr[14] = new SqlParameter("@GatePass", SqlDbType.NVarChar, 50);
                arr[15] = new SqlParameter("@lotno", SqlDbType.NVarChar, 50);
                arr[16] = new SqlParameter("@challanno", SqlDbType.NVarChar, 50);
                arr[17] = new SqlParameter("@retqty", SqlDbType.Int);
                arr[18] = new SqlParameter("@OrderId", SqlDbType.Int);
                arr[19] = new SqlParameter("@Finish_Type_Id", SqlDbType.Int);
                arr[20] = new SqlParameter("@LossQty", SqlDbType.Float);
                arr[21] = new SqlParameter("@GateInNo", SqlDbType.NVarChar, 50);
                arr[22] = new SqlParameter("@NoOFHank", SqlDbType.Int);
                arr[23] = new SqlParameter("@varuserid", SqlDbType.Int);
                arr[24] = new SqlParameter("@VarReceiveIssItem", SqlDbType.Int);
                arr[25] = new SqlParameter("@PenaltyDebitNote", SqlDbType.Float);
                arr[26] = new SqlParameter("@Remark", SqlDbType.NVarChar, 250);
                arr[27] = new SqlParameter("@Sizeflag", SqlDbType.Int);
                arr[28] = new SqlParameter("@Rate", SqlDbType.Float);
                arr[29] = new SqlParameter("@RateFlag", SqlDbType.Int);
                arr[30] = new SqlParameter("@DyingMatch", SqlDbType.VarChar, 20);
                arr[31] = new SqlParameter("@DyeingType", SqlDbType.VarChar, 20);
                arr[32] = new SqlParameter("@Dyeing", SqlDbType.VarChar, 20);
                arr[33] = new SqlParameter("@LShort", SqlDbType.Float);
                arr[34] = new SqlParameter("@Shrinkage", SqlDbType.Float);
                arr[35] = new SqlParameter("@TagNo", SqlDbType.VarChar, 50);
                arr[36] = new SqlParameter("@Rec_ofinishedid", SqlDbType.Int);
                arr[37] = new SqlParameter("@RecWithoutTag", SqlDbType.Int);
                arr[38] = new SqlParameter("@Checkedby", SqlDbType.VarChar, 50);
                arr[39] = new SqlParameter("@Approvedby", SqlDbType.VarChar, 50);
                arr[40] = new SqlParameter("@BinNo", SqlDbType.VarChar, 50);
                arr[41] = new SqlParameter("@MANUALTAGENTRY", SqlDbType.Int);
                arr[42] = new SqlParameter("@MANUALTAGNo", SqlDbType.VarChar, 50);
                arr[43] = new SqlParameter("@BillNo", SqlDbType.VarChar, 50);
                arr[44] = new SqlParameter("@ReDyeingStatus", SqlDbType.Int);
                arr[45] = new SqlParameter("@Legalvendorid", SqlDbType.Int);
                arr[46] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                arr[47] = new SqlParameter("@TagRemark", SqlDbType.VarChar, 200);
                arr[48] = new SqlParameter("@CustomerOrderId", SqlDbType.Int);
                arr[49] = new SqlParameter("@Moisture", SqlDbType.Float);
                arr[50] = new SqlParameter("@BellWeight", SqlDbType.Float);
                arr[51] = new SqlParameter("@BranchID", SqlDbType.Int);
                arr[52] = new SqlParameter("@MANUALLOTENTRY", SqlDbType.Int);
                arr[53] = new SqlParameter("@MANUALLOTNo", SqlDbType.VarChar, 50);
                arr[54] = new SqlParameter("@IssueQtyOnMachine", SqlDbType.Float);

                //***********************************************************
                arr[0].Direction = ParameterDirection.InputOutput;
                arr[0].Value = HPRMID.Value;
                arr[1].Value = ddCompName.SelectedValue;
                arr[2].Value = ddempname.SelectedValue;
                arr[3].Value = ddProcessName.SelectedValue;
                arr[4].Value = txtdate.Text;
                arr[5].Direction = ParameterDirection.InputOutput;
                arr[5].Value = 0;
                int ItemFinishedId = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, txtcode, tran, ddlshade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                arr[6].Value = ItemFinishedId;
                arr[7].Value = Convert.ToInt32(ddgodown.SelectedValue);

                double VarRecQty = Convert.ToDouble(txtrec.Text == "" ? "0" : txtrec.Text) - Convert.ToDouble(TxtBellWeight.Text == "" ? "0" : TxtBellWeight.Text);

                arr[8].Value = VarRecQty;
                arr[9].Value = ddindent.SelectedValue;
                arr[10].Value = ddlunit.SelectedValue;
                arr[11].Value = ddChallanNo.SelectedValue;
                arr[12].Value = 0;
                arr[13].Direction = ParameterDirection.Output;
                arr[14].Direction = ParameterDirection.Output;
                string LotNo = "";
                if (chkchangeLotno.Checked)
                {
                    LotNo = txtlotno.Text == "" ? "Without Lot No" : txtlotno.Text.Trim();
                }
                else
                {
                    LotNo = TDLotNo.Visible == true ? DDLotNo.SelectedItem.Text : "Without Tag No";
                }
                arr[15].Value = LotNo;
                arr[16].Direction = ParameterDirection.InputOutput;
                arr[16].Value = TxtPartyChallanNo.Text.ToUpper();
                arr[17].Value = Convert.ToDouble(txtretrn.Text != "" ? txtretrn.Text : "0");
                arr[18].Value = DDOrderNo.SelectedValue;
                arr[19].Value = 0;
                arr[20].Value = TxtLoss.Text == "" ? "0" : TxtLoss.Text;
                arr[21].Value = TxtGateInNo.Text;
                arr[21].Direction = ParameterDirection.InputOutput;
                arr[22].Value = TxtNoOFHank.Text == "" ? "0" : TxtNoOFHank.Text;
                arr[23].Value = Session["varuserid"].ToString();
                arr[24].Value = ChkForReceiveIssItem.Checked == true ? 1 : ChkForReceiveAnyColor.Checked == true ? 2 : 0;
                arr[25].Value = TxtPenalty.Text == "" ? "0" : TxtPenalty.Text;
                arr[26].Value = txtremarks.Text;
                arr[27].Value = DDsizetype.Visible == true ? DDsizetype.SelectedValue : "0";
                arr[28].Value = TDRate.Visible == true && TxtRate.Text == "" ? "0" : TxtRate.Text == "" ? "0" : TxtRate.Text;
                arr[29].Value = ChkForWithoutRate.Checked == true ? 1 : 0;
                arr[30].Value = TDDyeingMatch.Visible == true ? DDDyeingMatch.SelectedItem.Text : "";
                arr[31].Value = TDDyingType.Visible == true ? DDDyingType.SelectedItem.Text : "";
                arr[32].Value = TDDyeing.Visible == true ? DDDyeing.SelectedItem.Text : "";
                arr[33].Value = txtLShort.Text == "" ? "0" : txtLShort.Text;
                arr[34].Value = txtshrinkage.Text == "" ? "0" : txtshrinkage.Text;
                string TagNo = "";
                if (chkchangeTagno.Checked)
                {
                    TagNo = txttagNo.Text == "" ? "Without Tag No" : txttagNo.Text.Trim();
                }
                else
                {
                    TagNo = TDTagNo.Visible == true ? DDTagNo.SelectedItem.Text : "Without Tag No";
                }
                //   TagNo = TDTagNo.Visible == true ? DDTagNo.SelectedItem.Text : "Without Tag No";
                arr[35].Value = TagNo;
                arr[36].Value = TDRECSHADE.Visible == true ? DDRECSHADE.SelectedValue : "0";
                arr[37].Value = chkwithouttag.Visible == false ? "0" : (chkwithouttag.Checked == true ? "1" : "0");
                arr[38].Value = txtcheckedby.Text;
                arr[39].Value = txtapprovedby.Text;
                string BinNo = "";
                BinNo = TDBinno.Visible == true ? ddbinno.SelectedItem.Text : "";
                arr[40].Value = BinNo;
                arr[41].Value = chkchangeTagno.Checked == true ? 1 : 0;
                arr[42].Value = txttagNo.Text == "" ? "Without Tag No" : txttagNo.Text.Trim();
                arr[43].Value = txtBillNo.Text;
                arr[44].Value = ChkForReDyeing.Checked == true ? 1 : 0;
                arr[45].Value = Tdlegalvendor.Visible == false ? "0" : (DDlegalvendor.SelectedIndex > 0 ? DDlegalvendor.SelectedValue : "0");
                arr[46].Direction = ParameterDirection.Output;
                arr[47].Value = txtTagRemark.Text;
                arr[48].Value = DDOrderNo.SelectedValue;
                arr[49].Value = TDTxtMoisture.Visible == true ? (TxtMoisture.Text == "" ? "0" : TxtMoisture.Text) : "0";
                arr[50].Value = TxtBellWeight.Text == "" ? "0" : TxtBellWeight.Text;
                arr[51].Value = DDBranchName.SelectedValue;
                arr[52].Value = chkchangeLotno.Checked == true ? 1 : 0;
                arr[53].Value = txtlotno.Text == "" ? "Without Lot No" : txtlotno.Text.Trim();
                arr[54].Value = TDIssueQtyOnMachine.Visible == true ? (txtIssueQtyOnMachine.Text == "" ? "0" : txtIssueQtyOnMachine.Text) : "0";

                SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[PRO_PP_PRM_recieve]", arr);
                //*******************************************************

                if (arr[46].Value.ToString() != "")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + arr[46].Value.ToString() + "');", true);
                    tran.Rollback();
                }
                else
                {
                    HPRMID.Value = arr[0].Value.ToString();
                    TxtPartyChallanNo.Text = arr[16].Value.ToString();
                    TxtGateInNo.Text = arr[21].Value.ToString();
                    lblsrno.Text = "Sr No. Generated. " + arr[0].Value.ToString();

                    //RecQty For stock update
                    double RecQty = 0.00, Lshort = 0.00, Shrinkage = 0.00;
                    RecQty = Convert.ToDouble(txtrec.Text == "" ? "0" : txtrec.Text) - Convert.ToDouble(TxtBellWeight.Text == "" ? "0" : TxtBellWeight.Text);
                    if (txtLShort.Text != "" && txtLShort.Text != "0")
                    {
                        Lshort = RecQty * Convert.ToDouble(txtLShort.Text) / 100;
                    }
                    if (txtshrinkage.Text != "" && txtshrinkage.Text != "0")
                    {
                        Shrinkage = RecQty * Convert.ToDouble(txtshrinkage.Text) / 100; ;
                    }
                    RecQty = RecQty - (Lshort + Shrinkage);
                    //
                    //****************Stock Update
                    if (ChkForReceiveIssItem.Checked == true && (chkwithouttag.Visible == true && chkwithouttag.Checked == true))
                    {
                        TagNo = "Without Tag No";
                    }
                    if (chkchangeTagno.Checked == true)
                    {
                        TagNo = txttagNo.Text == "" ? "Without Tag No" : txttagNo.Text;
                    }

                    if (chkchangeLotno.Checked == true)
                    {
                        LotNo = txtlotno.Text == "" ? "Without Lot No" : txtlotno.Text;
                    }
                    //****************
                    if (btnsave.Text == "Update")
                    {
                        UtilityModule.StockStockTranTableUpdate2(ItemFinishedId, Convert.ToInt32(ddgodown.SelectedValue), Convert.ToInt32(ddCompName.SelectedValue), chkchangeLotno.Checked ? txtlotno.Text : LotNo, Convert.ToDouble(txtrec.Text == "" ? "0" : txtrec.Text), txtdate.Text.ToString(), txtdate.Text.ToString(), "pp_processrectran", Convert.ToInt32(arr[5].Value), tran, 1, true, 1, true, Convert.ToDouble(Session["issueqty"]), 0, unitid: Convert.ToInt16(ddlunit.SelectedValue), TagNo: TagNo, BinNo: BinNo);
                    }
                    else
                    {
                        UtilityModule.StockStockTranTableUpdate(ItemFinishedId, Convert.ToInt32(ddgodown.SelectedValue), Convert.ToInt32(ddCompName.SelectedValue), chkchangeLotno.Checked ? txtlotno.Text : LotNo, RecQty, txtdate.Text.ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), "pp_processrectran", Convert.ToInt32(arr[5].Value), tran, 1, true, 1, 0, unitid: Convert.ToInt16(ddlunit.SelectedValue), TagNo: TagNo, BinNo: BinNo);
                    }
                    ViewState["DetailID"] = arr[5].Value;
                    ViewState["masterID"] = arr[0].Value;

                    QCSAVE(tran, Convert.ToInt32(arr[0].Value), Convert.ToInt32(arr[5].Value));
                    tran.Commit();
                    txtcode.Text = "";
                    txtremarks.Text = "";
                    txtprerec.Text = "";
                    txtpending.Text = "";
                    txtrec.Text = "";
                    TxtLoss.Text = "";
                    TxtPenalty.Text = "";
                    TxtRate.Text = "";
                    txtLShort.Text = "";
                    txtshrinkage.Text = "";
                    txtTagRemark.Text = "";
                    DDDyingType.SelectedItem.Text = "";
                    DDDyeingMatch.SelectedItem.Text = "";
                    DDDyeing.SelectedItem.Text = "";
                    chkwithouttag.Checked = false;
                    if (TDLotNo.Visible == true)
                    {
                        DDLotNo.SelectedIndex = 0;
                    }
                    if (dsn.Visible == true)
                    {
                        dddesign.SelectedIndex = 0;
                    }
                    if (clr.Visible == true)
                    {
                        ddcolor.SelectedIndex = 0;
                    }
                    if (shp.Visible == true)
                    {
                        ddshape.SelectedIndex = 0;
                    }
                    if (sz.Visible == true)
                    {
                        ddsize.SelectedIndex = 0;
                    }
                    if (shd.Visible == true)
                    {
                        ddlshade.SelectedIndex = 0;
                    }
                    if (TDTagNo.Visible == true)
                    {
                        DDTagNo.SelectedIndex = -1;
                        txttagNo.Text = "";
                    }
                    if (customlotno.Visible == true)
                    {
                        DDLotNo.SelectedIndex = -1;
                        txtlotno.Text = "";
                    }
                    chkchangeTagno.Checked = false;
                    chkchangeLotno.Checked = false;
                    //ChkForReDyeing.Checked = false;
                    // dquality.SelectedIndex = 0;
                    TxtBellWeight.Text = "";
                    TxtMoisture.Text = "";
                    txtIssueQtyOnMachine.Text = "";
                    btnsave.Text = "Save";
                    txtidnt.Enabled = false;
                    Fill_Grid();
                    fillorderdetail();
                    for (int i = 0; i < grdqualitychk.Rows.Count; i++)
                    {
                        CheckBox chk = (CheckBox)grdqualitychk.Rows[i].FindControl("CheckBox1");
                        chk.Checked = false;
                    }
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


    }
    protected void txtrec_TextChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Visible = false;
        LblErrorMessage.Text = "";
        TxtLoss.Text = "0";
        if (ChkForReceiveIssItem.Checked == false)
        {
            if (Convert.ToInt32(Session["VarcompanyNo"]) == 7)
            {
                FillLossQty();
            }
            else
            {
                Check_Qty();
            }
        }
        else if (ChkForReceiveIssItem.Checked == true)
        {
            if (variable.Carpetcompany == "1")
            {
                CheckIssRecPendingqty();
            }
        }
    }
    protected void CheckIssRecPendingqty()
    {
        string LotNo, TagNo;
        LotNo = TDLotNo.Visible == true && DDLotNo.Items.Count > 0 ? DDLotNo.SelectedItem.Text : "Without Lot No";
        TagNo = TDTagNo.Visible == true && DDTagNo.Items.Count > 0 ? DDTagNo.SelectedItem.Text : "Without Tag No";
        double varpendingqtytorec = 0;

        double PercentageExecssQtyForIndentRawRec = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select PercentageExecssQtyForIndentRawRec From MasterSetting"));

        SqlParameter[] param = new SqlParameter[10];
        param[0] = new SqlParameter("@indentid", ddindent.SelectedValue);
        param[1] = new SqlParameter("@ofinishedid", 0);
        param[2] = new SqlParameter("@Lotno", LotNo);
        param[3] = new SqlParameter("@Tagno", TagNo);
        param[4] = new SqlParameter("@issprmid", ddChallanNo.SelectedValue);
        param[5] = new SqlParameter("@pendingqty", SqlDbType.Decimal);
        param[5].Direction = ParameterDirection.Output;
        param[5].Scale = 3;
        param[5].Precision = 18;
        param[6] = new SqlParameter("@Receiveqty", SqlDbType.Decimal);
        param[6].Direction = ParameterDirection.Output;
        param[6].Scale = 3;
        param[6].Precision = 18;
        param[7] = new SqlParameter("@ifinishedid", hnfinishedid.Value);

        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getPendingIssqtytoRec", param);
        varpendingqtytorec = Convert.ToDouble(param[5].Value);

        varpendingqtytorec = varpendingqtytorec + (varpendingqtytorec * PercentageExecssQtyForIndentRawRec / 100);
        if (varpendingqtytorec < Convert.ToDouble(txtrec.Text == "" ? "0" : txtrec.Text) - Convert.ToDouble(TxtBellWeight.Text == "" ? "0" : TxtBellWeight.Text))
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Recieve qty must be less than or equal to total Pending issue qty";
            MessageSave("Recieve qty must be less than or equal to issue qty");
            txtrec.Text = "";
            txtrec.Focus();
            return;
        }
    }

    protected void Check_Qty()
    {
        if (Convert.ToInt32(Session["VarcompanyNo"]) == 21 && ChkForReDyeing.Checked == true && ChkForReceiveAnyColor.Checked == true)
        {
            Double VarRecQty = Convert.ToDouble(txtrec.Text == "" ? "0" : txtrec.Text);
            Double VarPendingQty = Convert.ToDouble(txtpending.Text == "" ? "0" : txtpending.Text);

            if (VarRecQty > VarPendingQty)
            {
                LblErrorMessage.Text = "Recieve qty must be less than or equal to total Pending issue qty";
                MessageSave("Recieve qty must be less than or equal to issue qty");
                txtrec.Text = "";
                txtrec.Focus();
                return;
            }
        }
        else
        {
            string LotNo, TagNo;
            LotNo = TDLotNo.Visible == true && DDLotNo.Items.Count > 0 ? DDLotNo.SelectedItem.Text : "Without Lot No";
            TagNo = TDTagNo.Visible == true && DDTagNo.Items.Count > 0 ? DDTagNo.SelectedItem.Text : "Without Tag No";
            double varpendingqtytorec = 0;

            double VarRecQty = (txtrec.Text == "" ? 0 : Convert.ToDouble(txtrec.Text)) - (TxtBellWeight.Text == "" ? 0 : Convert.ToDouble(TxtBellWeight.Text));
            double VarIndentQty = TxtIndentQty.Text == "" ? 0 : Convert.ToDouble(TxtIndentQty.Text);
            double VarReturnQty = txtretrn.Text == "" ? 0 : Convert.ToDouble(txtretrn.Text);
            double VarPendingQty = txtpending.Text == "" ? 0 : Convert.ToDouble(txtpending.Text);
            double PercentageExecssQtyForIndentRawRec = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select PercentageExecssQtyForIndentRawRec From MasterSetting"));
            if (variable.Carpetcompany == "1")
            {
                if (ChkForReceiveIssItem.Checked == false)
                {
                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@indentid", ddindent.SelectedValue);
                    param[1] = new SqlParameter("@ofinishedid", hnfinishedid.Value);
                    param[2] = new SqlParameter("@Lotno", LotNo);
                    param[3] = new SqlParameter("@Tagno", TagNo);
                    param[4] = new SqlParameter("@issprmid", ddChallanNo.SelectedValue);
                    param[5] = new SqlParameter("@pendingqty", SqlDbType.Decimal);
                    param[5].Direction = ParameterDirection.Output;
                    param[5].Scale = 3;
                    param[5].Precision = 18;
                    param[6] = new SqlParameter("@Receiveqty", SqlDbType.Decimal);
                    param[6].Direction = ParameterDirection.Output;
                    param[6].Scale = 3;
                    param[6].Precision = 18;

                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getPendingIssqtytoRec", param);
                    VarRecQty = VarRecQty + Convert.ToDouble(param[6].Value);
                    varpendingqtytorec = Convert.ToDouble(param[5].Value);

                    varpendingqtytorec = varpendingqtytorec + (varpendingqtytorec * PercentageExecssQtyForIndentRawRec / 100);
                    if (varpendingqtytorec < Convert.ToDouble(txtrec.Text == "" ? "0" : txtrec.Text) - Convert.ToDouble(TxtBellWeight.Text == "" ? "0" : TxtBellWeight.Text))
                    {
                        LblErrorMessage.Visible = true;
                        LblErrorMessage.Text = "Recieve qty must be less than or equal to total Pending issue qty";
                        MessageSave("Recieve qty must be less than or equal to issue qty");
                        txtrec.Text = "";
                        txtrec.Focus();
                        return;
                    }
                    else
                    {
                        VarRecQty = (txtrec.Text == "" ? 0 : Convert.ToDouble(txtrec.Text)) - (TxtBellWeight.Text == "" ? 0 : Convert.ToDouble(TxtBellWeight.Text));
                        VarIndentQty = TxtIndentQty.Text == "" ? 0 : Convert.ToDouble(TxtIndentQty.Text);
                        //double VarReturnQty = txtretrn.Text == "" ? 0 : Convert.ToDouble(txtretrn.Text);
                        VarPendingQty = txtpending.Text == "" ? 0 : Convert.ToDouble(txtpending.Text);
                        VarIndentQty = Math.Round(VarIndentQty * PercentageExecssQtyForIndentRawRec / 100, 3);
                        //if (VarRecQty - VarReturnQty > VarPendingQty + VarIndentQty)
                        if (VarRecQty > VarPendingQty + VarIndentQty)
                        {
                            LblErrorMessage.Visible = true;
                            LblErrorMessage.Text = "Recieve qty must be less than or equal to PQty";
                            MessageSave("Recieve qty must be less than or equal to Pqty");
                            txtrec.Text = "";
                            txtrec.Focus();
                            return;
                        }
                    }
                }
                else
                {
                    VarIndentQty = Math.Round(VarIndentQty * PercentageExecssQtyForIndentRawRec / 100, 3);
                    //if (VarRecQty - VarReturnQty > VarPendingQty + VarIndentQty)
                    if (VarRecQty > VarPendingQty + VarIndentQty)
                    {
                        LblErrorMessage.Visible = true;
                        LblErrorMessage.Text = "Recieve qty must be less than or equal to issue qty";
                        MessageSave("Recieve qty must be less than or equal to issue qty");
                        txtrec.Text = "";
                        txtrec.Focus();
                        return;
                    }
                }
            }
            else
            {
                VarIndentQty = Math.Round(VarIndentQty * PercentageExecssQtyForIndentRawRec / 100, 3);
                //if (VarRecQty - VarReturnQty > VarPendingQty + VarIndentQty)
                if (VarRecQty > VarPendingQty + VarIndentQty)
                {
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = "Recieve qty must be less than or equal to issue qty";
                    MessageSave("Recieve qty must be less than or equal to issue qty");
                    txtrec.Text = "";
                    txtrec.Focus();
                    return;
                }
            }

            if (variable.Dyingautoloss == "1")
            {
                FillLossQty();
            }
        }
    }
    protected void txtidnt_TextChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        if (txtidnt.Text != "")
        {
            try
            {
                string str = @"Select Distinct IM.IndentID,IM.CompanyId,IM.PartyId,IM.ProcessID 
                From IndentMaster IM,PP_ProcessRawTran PPT 
                Where IM.IndentID=PPT.IndentID And IM.CompanyId = " + ddCompName.SelectedValue + " And IM.IndentNo='" + txtidnt.Text + @"' AND
                IM.BranchID = " + DDBranchName.SelectedValue + @" And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                if (chkcomplete.Checked == true)
                {
                    str = str + " and Im.status='Complete'";
                }
                else
                {
                    str = str + " and Im.status='Pending'";
                }
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddProcessName.SelectedValue = ds.Tables[0].Rows[0]["ProcessID"].ToString();
                    ProcessSelectedChange();
                    ddempname.SelectedValue = ds.Tables[0].Rows[0]["PartyId"].ToString();
                    EmpolyeeSelectedChange();
                    ddindent.SelectedValue = ds.Tables[0].Rows[0]["IndentID"].ToString();
                    IndentSelectedChange();
                    if (ddChallanNo.Items.Count > 0)
                    {
                        ddChallanNo.SelectedIndex = 1;
                        ChallanNoSelectedChange();
                        if (ChkEdit.Checked == true)
                        {
                            if (ddPartyChallanNo.Items.Count > 0)
                            {
                                ddPartyChallanNo.SelectedIndex = 1;
                                PartyChallanNoSelectedChenge();
                            }
                        }
                        if (ddCatagory.Items.Count > 0)
                        {
                            ddCatagory.SelectedIndex = 1;
                            ddlcategorycange();
                        }
                    }
                }
                else
                {
                    LblErrorMessage.Text = "IndentNo does not exists or Indent is complete...........";
                    LblErrorMessage.Visible = true;
                    if (ddChallanNo.SelectedIndex > 0)
                    {
                        ddChallanNo.SelectedIndex = -1;
                        ddChallanNo_SelectedIndexChanged(ddChallanNo, e);
                    }
                    txtidnt.Text = "";
                    txtidnt.Focus();
                }
            }
            catch (Exception ex)
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = ex.Message;
            }
        }
    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarProcess_Issue_Detail_Id = Convert.ToInt32(gvdetail.DataKeys[e.RowIndex].Value);
            SqlParameter[] _arrPara = new SqlParameter[5];
            _arrPara[0] = new SqlParameter("@PRTID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@COUNT", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@Msg", SqlDbType.NVarChar, 250);
            _arrPara[3] = new SqlParameter("@Userid", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@Mastercompanyid", SqlDbType.Int);

            _arrPara[0].Value = VarProcess_Issue_Detail_Id;
            if (gvdetail.Rows.Count == 1)
            {
                _arrPara[1].Value = 1;
            }
            _arrPara[2].Direction = ParameterDirection.Output;
            _arrPara[3].Value = Session["varuserid"];
            _arrPara[4].Value = Session["varMasterCompanyIDForERP"];

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PP_PRM_RECEIVE_DELETE", _arrPara);
            Tran.Commit();
            Fill_Grid();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = _arrPara[2].Value.ToString();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    public void fillchkbox()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @"Select QcmasterID from QCDETAIL where RecieveID=" + HPRMID.Value + " and Qcvalue='1' and RefName='PP_ProcessRecTran'";
        SqlDataAdapter sda = new SqlDataAdapter(qry, con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            for (int j = 0; j < grdqualitychk.Rows.Count; j++)
            {
                //if (((Label)grdqualitychk.Rows[j].FindControl("Label1")).Text == ds.Tables[0].Rows[i]["QcmasterID"].ToString())
                if (Convert.ToString(grdqualitychk.DataKeys[j].Value) == ds.Tables[0].Rows[i]["QcmasterID"].ToString())
                {
                    CheckBox chk = (CheckBox)grdqualitychk.Rows[j].FindControl("CheckBox1");
                    chk.Checked = true;
                }
            }
        }

    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 5)
        {
            Session["ReportPath"] = "Reports/IndentRawRecDuplicate.rpt";
            Session["CommanFormula"] = "{V_IndentRawRec.PRMid}=" + HPRMID.Value + "";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
        }
        else
        {
            if (ChkForGSTReport.Checked == true)
            {
                ReportGST();
            }
            else
            {
                Report();
            }
            //Report();
        }
    }
    private void Report()
    {
//        string qry = @"WITH summary AS (
//
//            SELECT  row_NUMBER() OVER(PARTITION BY V.RecQuantity order by v.LOCALORDER DESC
//                                 ) AS RK,V.CATEGORY_NAME, V.ITEM_NAME, V.QualityName, V.DESCRIPTION, V.GodownName, V.RecQuantity, IsNull(VR.RetQty, 0) As RetQty, V.lotno, V.EmpName, 
//            V.CompanyName, V.gatepassno, V.indentno, V.EmpAddress, V.EmpPhoneNo, V.EmpMobile, V.CompanyAddress, V.CompanyPhoneNo, V.CompanyFaxNo, V.TinNo, 
//            V.PRMid, V.Date, V.CHALLANNO, MastercompanyId, DyingMatch, DyeingType, Dyeing,  V.CustomerOrderNo, V.Buyercode, PROCESS_NAME, Rec_Iss_ItemFlag, 
//            RRRemark, Lshort, Shrinkage, V.TagNo, V.GateInNo, GSTIN, EMPGSTIN, CheckedBy, ApprovedBy, od.customercode, od.OrderNo, od.Localorder, OD.Merchantname, V.LossQty,V.BILLNo
//            FROM V_IndentRawRec V(Nolock)  
//            LEFT JOIN V_IndentRawReturnQty VR(Nolock) ON VR.PRMid = V.PRMid AND VR.PRTid = V.PRTid 
//            CROSS APPLY (Select * From dbo.F_GetPPNo_OrderNo(V.PPNo)) OD  
//            Where V.PRMid = " + HPRMID.Value + ") SELECT * FROM summary WHERE RK=1";

        string qry = "";

        if (Session["VarCompanyNo"].ToString() == "43")
        {
            qry = @"Select V.CATEGORY_NAME, V.ITEM_NAME, V.QualityName, V.DESCRIPTION, V.GodownName, V.RecQuantity, IsNull(VR.RetQty, 0) As RetQty, V.lotno, V.EmpName, 
            V.CompanyName, V.gatepassno, V.indentno, V.EmpAddress, V.EmpPhoneNo, V.EmpMobile, V.CompanyAddress, V.CompanyPhoneNo, V.CompanyFaxNo, V.TinNo, 
            V.PRMid, V.Date, V.CHALLANNO, MastercompanyId, DyingMatch, DyeingType, Dyeing,  V.CustomerOrderNo, V.Buyercode, PROCESS_NAME, Rec_Iss_ItemFlag, 
            RRRemark, Lshort, Shrinkage, V.TagNo, V.GateInNo, GSTIN, EMPGSTIN, CheckedBy, ApprovedBy, od.customercode, od.OrderNo, od.Localorder, OD.Merchantname, V.LossQty,V.BILLNo
            ,V.UserName,v.UnitName,isnull((Select isnull(sum(ID.Quantity),0) from IndentDetail ID Where ID.OFinishedId=V.FinishedId and Id.lotno=V.LotNo and ID.TagNo=V.TagNo and Id.IndentId=V.IndentId),0) as IndentQty
            ,isnull((select Distinct VF2.QualityName+',' From ProcessProgram PP(NoLock) JOIN OrderMaster OM2(NoLock) on PP.Order_ID=OM2.OrderId  
	            JOIN OrderDetail OD2 ON OM2.OrderId=OD2.OrderId JOIN V_FinishedItemDetail VF2(NoLock) ON OD2.Item_Finished_Id=VF2.ITEM_FINISHED_ID
	            Where PPID=V.PPNo for xml path('')),'') as OrderQuality,isnull(IssueQtyOnMachine,0) as IssueQtyOnMachine 
            FROM V_IndentRawRec V(Nolock)  
            LEFT JOIN V_IndentRawReturnQty VR(Nolock) ON VR.PRMid = V.PRMid AND VR.PRTid = V.PRTid 
            CROSS APPLY (Select * From dbo.F_GetPPNo_OrderNo(V.PPNo)) OD  
            Where V.PRMid = " + HPRMID.Value + " ";
        }
        else
        {
            qry = @"Select V.CATEGORY_NAME, V.ITEM_NAME, V.QualityName, V.DESCRIPTION, V.GodownName, V.RecQuantity, IsNull(VR.RetQty, 0) As RetQty, V.lotno, 
            V.EmpName, V.CompanyName, V.gatepassno, V.indentno, V.EmpAddress, V.EmpPhoneNo, V.EmpMobile, V.CompanyAddress, V.CompanyPhoneNo, V.CompanyFaxNo, 
            V.TinNo, V.PRMid, V.Date, V.CHALLANNO, V.MastercompanyId, V.DyingMatch, V.DyeingType, V.Dyeing,  V.CustomerOrderNo, V.Buyercode, V.PROCESS_NAME, 
            V.Rec_Iss_ItemFlag, V.RRRemark, V.Lshort, V.Shrinkage, V.TagNo, V.GateInNo, V.GSTIN, V.EMPGSTIN, V.CheckedBy, V.ApprovedBy, od.customercode, 
            OD.OrderNo, OD.Localorder, OD.Merchantname, V.LossQty, V.BILLNo, V.UserName, V.UnitName, V.LossQty 
            FROM V_IndentRawRec V(Nolock) 
            LEFT JOIN V_IndentRawReturnQty VR(Nolock) ON VR.PRMid = V.PRMID AND VR.PRTid = V.PRTID 
            CROSS APPLY (Select * From dbo.F_GetPPNo_OrderNo(V.PPNo)) OD 
            Where V.PRMid = " + HPRMID.Value + " ";
        }       


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "6":
                case "12":
                    Session["rptFileName"] = "~\\Reports\\IndentRawRecNewArtindia.rpt";
                    break;
                case "14":
                    Session["rptFileName"] = "~\\Reports\\IndentRawRecNewEMIKEA.rpt";
                    break;
                case "27":
                    Session["rptFileName"] = "~\\Reports\\IndentRawRecNewAntique.rpt";
                    break;
                case "21":
                    Session["rptFileName"] = "~\\Reports\\IndentRawRecNewKaysons.rpt";
                    break;
                case "44":
                    Session["rptFileName"] = "~\\Reports\\IndentRawRecagni.rpt";
                    break;
                case "22":
                    Session["rptFileName"] = "~\\Reports\\IndentRawRecNewDiamond.rpt";
                    break;
                case "43":
                    Session["rptFileName"] = "~\\Reports\\IndentRawRecNewCI.rpt";
                    break;
                default:
                    Session["rptFileName"] = "~\\Reports\\IndentRawRecNew.rpt";
                    break;
            }
            // Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\IndentRawRecNew.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    private void ReportGST()
    {
        SqlParameter[] _array = new SqlParameter[4];
        _array[0] = new SqlParameter("@PRMID", SqlDbType.Int);
        _array[1] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        _array[2] = new SqlParameter("@UserId", SqlDbType.Int);

        _array[0].Value = HPRMID.Value;
        _array[1].Value = Session["varMasterCompanyIDForERP"];
        _array[2].Value = Session["varuserId"];

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetIndentRawReceiveWithGSTReport", _array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptIndentRawReceiveWithGST.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptIndentRawReceiveWithGST.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    protected void FillLossQty()
    {
        double VarRecQty = (txtrec.Text == "" ? 0 : Convert.ToDouble(txtrec.Text)) - (TxtBellWeight.Text == "" ? 0 : Convert.ToDouble(TxtBellWeight.Text));
        double VarRetQty = txtretrn.Text == "" ? 0 : Convert.ToDouble(txtretrn.Text);
        VarRecQty = VarRecQty - VarRetQty;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int ItemFinishedId = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, txtcode, Tran, ddlshade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            string str = "Select LossPercent From IndentDetail where IndentId=" + ddindent.SelectedValue;
            if (ChkForReceiveAnyColor.Checked != true)
            {
                str = str + " And OFinishedid=" + ItemFinishedId;
            }
            Double LossPercent = Convert.ToDouble(SqlHelper.ExecuteScalar(Tran, CommandType.Text, str));

            //TxtLoss.Text = Math.Round(((100 / (100 - LossPercent)) * VarRecQty - (VarRecQty)), 3).ToString();
            if (LossPercent > 0)
            {
                if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 16)
                {
                    TxtLoss.Text = Math.Round(((100 / (100 - LossPercent)) * VarRecQty - (VarRecQty)), 3).ToString();
                }
                else
                {
                    TxtLoss.Text = Math.Round(LossPercent / 100 * VarRecQty, 3).ToString();
                }
            }
            Tran.Commit();
        }
        catch
        {
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void ChkForReceiveIssItem_CheckedChanged(object sender, EventArgs e)
    {
        ReceiveIssItemCheckedChanged();
        chkchangeTagno.Checked = false;
        chkchangeTagno.Visible = true;
        if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 22)
        {
            chkchangeLotno.Checked = false;
            chkchangeLotno.Visible = true;
        }
        if (variable.Carpetcompany == "1")
        {
            if (ChkForReceiveIssItem.Checked == true)
            {
                LblColorShade.Text = "UNDYED COLOR";
                TDRECSHADE.Visible = true;
                chkchangeTagno.Visible = false;
                chkchangeLotno.Visible = false;
                //chkwithouttag.Visible = true;
            }
            else
            {
                LblColorShade.Text = "SHADE_COLOR";
                TDRECSHADE.Visible = false;
            }

            if (ChkForReceiveIssItem.Checked == true && ddProcessName.SelectedValue == "5")
            {
                ChkForReDyeing.Visible = false;
            }
            else if (ChkForReceiveIssItem.Checked == false && ddProcessName.SelectedValue == "5")
            {
                ChkForReDyeing.Visible = true;
            }

           


        }
        if (ChkForReceiveIssItem.Checked == true)
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "9")
            {
                ddgodown.Enabled = true;
            }
        }
        else
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "9")
            {
                ddgodown.SelectedValue = "3";
                ddgodown.Enabled = false;
            }
        }
    }
    private void ReceiveIssItemCheckedChanged()
    {
        if (ChkForReceiveIssItem.Checked == true)
        {
            //            UtilityModule.ConditionalComboFill(ref ddCatagory, @"SELECT DISTINCT VF.CATEGORY_ID,VF.CATEGORY_NAME From IndentMaster IM,IndentDetail ID,V_FinishedItemDetail VF inner join UserRights_Category UC on(UC.CategoryId=VF.Category_ID And UC.UserId=" + Session["varuserid"] + @")
            //            Where IM.IndentID=ID.IndentID And ID.IFinishedId=VF.ITEM_FINISHED_ID And IM.PartyId=" + ddempname.SelectedValue + " And IM.ProcessID=" + ddProcessName.SelectedValue + " And IM.IndentId=" + ddindent.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Category Name");

            string str = @"select DISTINCT VF.CATEGORY_ID,VF.CATEGORY_NAME from PP_ProcessRawMaster PM inner join PP_ProcessRawTran PT
                            on PM.PRMid=PT.PRMid inner join IndentMaster IM on IM.IndentID=pt.Indentid
                            inner join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID
                            inner join UserRights_Category UC on(UC.CategoryId=VF.Category_ID And UC.UserId=" + Session["varuserid"] + @")
                            Where  IM.PartyId=" + ddempname.SelectedValue + " And IM.ProcessID=" + ddProcessName.SelectedValue + " And IM.IndentId=" + ddindent.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

            UtilityModule.ConditionalComboFill(ref ddCatagory, str, true, "Select Category Name");

            txtretrn.ReadOnly = true;
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddCatagory, @"SELECT DISTINCT VF.CATEGORY_ID,VF.CATEGORY_NAME From IndentMaster IM,IndentDetail ID,V_FinishedItemDetail VF inner join UserRights_Category UC on(UC.CategoryId=VF.Category_ID And UC.UserId=" + Session["varuserid"] + @")
            Where IM.IndentID=ID.IndentID And ID.OFinishedId=VF.ITEM_FINISHED_ID And IM.PartyId=" + ddempname.SelectedValue + " And IM.ProcessID=" + ddProcessName.SelectedValue + " And IM.IndentId=" + ddindent.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Category Name");
            txtretrn.ReadOnly = false;
        }
        if (ddCatagory.SelectedIndex > 0)
        {
            ddlcategorycange();
        }
    }
    protected void btnqcchkpreview_Click(object sender, EventArgs e)
    {
        reportQcheck();
    }
    private void reportQcheck()
    {

        string SName = "";
        string QCValue = "";
        string qry = "";
        DataSet ds = new DataSet();

        qry = @"  SELECT V_IndentRawRec.CATEGORY_NAME,V_IndentRawRec.ITEM_NAME,V_IndentRawRec.QualityName,V_IndentRawRec.DESCRIPTION,V_IndentRawRec.GodownName,V_IndentRawRec.RecQuantity,V_IndentRawRec.lotno,V_IndentRawRec.EmpName,V_IndentRawRec.CompanyName,V_IndentRawRec.gatepassno,V_IndentRawRec.indentno,V_IndentRawRec.EmpAddress,V_IndentRawRec.EmpPhoneNo,V_IndentRawRec.EmpMobile,V_IndentRawRec.CompanyAddress,V_IndentRawRec.CompanyPhoneNo,V_IndentRawRec.CompanyFaxNo,V_IndentRawRec.TinNo,V_IndentRawRec.PRMid
                   FROM   V_IndentRawRec where V_IndentRawRec.PRMid=" + HPRMID.Value + "";

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        string str = @"Select SName, Case when QCValue='1' then 'YES' else 'NO' END QCValue from QCdetail QCD inner join QCMaster QCM ON 
                         QCD.QCMasterID=QCM.ID Inner Join QCParameter QCP ON QCM.ParaID=QCP.ParaID
                         Where RefName= 'PP_ProcessRecTran' and QCD.RecieveID=" + HPRMID.Value + "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        SqlDataAdapter sda = new SqlDataAdapter(str, con);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (SName == "" && QCValue == "")
            {
                SName = dt.Rows[i]["SName"].ToString();
                QCValue = dt.Rows[i]["QCValue"].ToString();
            }
            else
            {
                SName = SName + ' ' + dt.Rows[i]["SName"].ToString();
                QCValue = QCValue + ' ' + dt.Rows[i]["QCValue"].ToString();
            }
        }
        DataTable mytable = new DataTable();
        mytable.Columns.Add("SName", typeof(string));
        mytable.Columns.Add("QCValue", typeof(string));
        mytable.Rows.Add(SName, QCValue);
        ds.Tables.Add(mytable);
        Session["ReportPath"] = "Reports/IndentRawRecQCNew.rpt";
        Session["dsFileName"] = "~\\ReportSchema\\IndentRawRecQCNew.xsd";


        if (ds.Tables[0].Rows.Count > 0)
        {
            //Session["rptFileName"] = "~\\Reports\\PGenrateIndentNEW.rpt";
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            //Session["dsFileName"] = "~\\ReportSchema\\PGenrateIndentNEW.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }

    }
    //protected void gvdetail_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //Add CSS class on header row.
    //    if (e.Row.RowType == DataControlRowType.Header)
    //        e.Row.CssClass = "header";

    //    //Add CSS class on normal row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Normal)
    //        e.Row.CssClass = "normal";

    //    //Add CSS class on alternate row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Alternate)
    //        e.Row.CssClass = "alternate";
    //}
    //protected void grdqualitychk_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //Add CSS class on header row.
    //    if (e.Row.RowType == DataControlRowType.Header)
    //        e.Row.CssClass = "header";

    //    //Add CSS class on normal row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Normal)
    //        e.Row.CssClass = "normal";

    //    //Add CSS class on alternate row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Alternate)
    //        e.Row.CssClass = "alternate";
    //}
    private void fillorderdetail()
    {

        string sql = @"SELECT Category_Name+'  '+V.ITEM_NAME+'  '+QualityName+'  '+DesignName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName
            +'  '+isnull(id.Dyingmatch,'')+'   '+isnull(id.dyeingType,'')+'   '+isnull(id.dyeing,'') as Description,SUM(Quantity) AS QTY,IM.INDENTID as indentid
            ,v.item_finished_id as finishedid,CATEGORY_ID,ITEM_ID,QualityId,
            ColorId,designId,SizeId,ShapeId,ShadecolorId,unitid,ID.TagNO,ID.Lotno
            FROM INDENTMASTER IM(Nolock) 
            INNER JOIN INDENTDETAIL ID(Nolock) ON IM.INDENTID=ID.INDENTID 
            INNER JOIN V_FinishedItemDetail V(Nolock) ON V.ITEM_FINISHED_ID=ID.OFinishedId 
            Inner Join V_PPFinishedid VP(Nolock) on ID.OFinishedId=VP.FinishedId and ID.PPNo=VP.PPID ";
            if(Session["VarCompanyNo"].ToString()!="30") 
            {
                sql = sql + @"  inner join (select  Distinct Prt.Finishedid,PRt.lotno 
                        From PP_ProcessRawMaster PRM(Nolock) 
                        join PP_ProcessRawTran PRT(Nolock) on PRM.PRMid=PRT.PRMid and PRM.PRMid=" + ddChallanNo.SelectedValue + @") As IssDetail on ID.lotno=IssDetail.Lotno and vp.IFinishedid=IssDetail.Finishedid ";
            } 
            sql = sql + " WHERE IM.INDENTID=" + ddindent.SelectedValue + " And IM.MasterCompanyId=" + Session["varcompanyNo"] + @"
            GROUP BY Category_Name ,V.ITEM_NAME ,QualityName,DesignName,ColorName,
            ShadeColorName,ShapeName,CATEGORY_ID,ITEM_ID,QualityId,ColorId,designId,
            SizeId,ShapeId,ShadecolorId,v.item_finished_id,IM.INDENTID,id.Dyingmatch,
            id.dyeingType,id.dyeing,unitid,ID.TagNO,ID.Lotno ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            dGORDER.DataSource = ds;
            dGORDER.DataBind();
            dGORDER.Visible = true;
            tdorder.Visible = true;
        }
        else
        {
            dGORDER.Visible = false;
            tdorder.Visible = false;
        }
    }
    protected void dGORDER_SelectedIndexChanged(object sender, EventArgs e)
    {
        int r = Convert.ToInt32(dGORDER.SelectedIndex.ToString());
        string category = ((Label)dGORDER.Rows[r].FindControl("lblcategoryid")).Text;
        string Item = ((Label)dGORDER.Rows[r].FindControl("lblitem_id")).Text;
        string Quality = ((Label)dGORDER.Rows[r].FindControl("lblQualityid")).Text;
        string Color = ((Label)dGORDER.Rows[r].FindControl("lblColorid")).Text;
        string design = ((Label)dGORDER.Rows[r].FindControl("lbldesignid")).Text;
        string shape = ((Label)dGORDER.Rows[r].FindControl("lblshapeid")).Text;
        string shadecolor = ((Label)dGORDER.Rows[r].FindControl("lblshadecolorid")).Text;
        string size = ((Label)dGORDER.Rows[r].FindControl("lblsizeid")).Text;
        string Qty = ((Label)dGORDER.Rows[r].FindControl("lblqty")).Text;
        string RecQty = ((Label)dGORDER.Rows[r].FindControl("lblrecqty")).Text;
        string unitid = ((Label)dGORDER.Rows[r].FindControl("lblunitid")).Text;
        string lbllotno = ((Label)dGORDER.Rows[r].FindControl("lbllotno")).Text;
        string lbltagno = ((Label)dGORDER.Rows[r].FindControl("lbltagno")).Text;
        string lblfinishedid = ((Label)dGORDER.Rows[r].FindControl("lblfinished")).Text;
        //lblfinished
        //string orderdet = ((Label)dGORDER.Rows[r].FindControl("lblorderdet")).Text;
        if (ddCatagory.Visible == true)
        {
            ddCatagory.SelectedValue = category;
            ddlcategorycange();
        }
        if (dditemname.Visible == true)
        {
            dditemname.SelectedValue = Item;
            FILL_ITEM_CHANGED();
        }
        if (dquality.Visible == true)
        {
            dquality.SelectedValue = Quality;
            FILL_QUANTITYCHANGE();
        }
        if (dddesign.Visible == true)
        {
            dddesign.SelectedValue = design;

        }
        if (ddcolor.Visible == true)
        {
            ddcolor.SelectedValue = Color;
        }
        if (ddshape.Visible == true)
        {
            ddshape.SelectedValue = shape;
        }
        if (ddsize.Visible == true)
        {
            ddsize.SelectedValue = size;
        }
        if (ddlshade.Visible == true)
        {
            ddlshade.SelectedValue = shadecolor;
            if (Session["varMasterCompanyIDForERP"].ToString() == "4")
            {
                GetDyingTypes();
            }
        }
        if (ddlunit.Items.FindByValue(unitid) != null)
        {
            ddlunit.SelectedValue = unitid;
        }
        //fill_qty();

        FillLotNo(Convert.ToInt32(lblfinishedid));
        if (DDOrderNo.Items.Count > 0)
        {
            DDOrderNo.SelectedIndex = 1;
        }
        if (DDLotNo.Items.FindByValue(lbllotno) != null)
        {
            DDLotNo.SelectedValue = lbllotno;
            DDLotNo_SelectedIndexChanged(sender, e);
        }
        if (TDTagNo.Visible == true)
        {
            if (DDTagNo.Items.FindByValue(lbltagno) != null)
            {
                DDTagNo.SelectedValue = lbltagno;
                DDTagNo_SelectedIndexChanged(sender, e);
            }
        }
        if (TDLotNo.Visible == false)
        {
            fill_Order_qty();
        }
        if (variable.Carpetcompany != "1")
        {
            txtrec.Text = RecQty;
            txtrec_TextChanged(sender, e);
        }
    }
    protected void dGORDER_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dGORDER, "Select$" + e.Row.RowIndex);
        }
    }
    public string getgiven(string strval, string strval1, string strval2, string Lotno, String Tagno)
    {
        string val = "";
        int ManualTagEntry = 0;
        ////DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(sum(RecQuantity),0)+isnull(sum(Lossqty),0) from PP_ProcessRecTran where indentid=" + strval1 + " and finishedid=" + strval + " and Lotno='" + Lotno + "' and tagno='" + Tagno + "'");

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(ManualTagEntry,0) as ManualTagEntry From PP_ProcessRecTran Where IndentId=" + strval1 + " and Finishedid=" + strval + " and Lotno='" + Lotno + "' ");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ManualTagEntry = Convert.ToInt32(ds.Tables[0].Rows[0]["ManualTagEntry"].ToString());
        }

        //if (ManualTagEntry == 1)
        //{
        //    DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(Sum(recqty),0) as recqty From V_IndentRecWithIssrecItemflag Where IndentId=" + strval1 + " and Finishedid=" + strval + " and Lotno='" + Lotno + "' ");
        //    val = Convert.ToString(Math.Round(Convert.ToDouble(strval2) - Convert.ToDouble(dt.Tables[0].Rows[0][0].ToString()), 3));
        //}
        //else
        //{
        //    DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(Sum(recqty),0) as recqty From V_IndentRecWithIssrecItemflag Where IndentId=" + strval1 + " and Finishedid=" + strval + " and Lotno='" + Lotno + "' and TagNo='" + Tagno + "'");
        //    val = Convert.ToString(Math.Round(Convert.ToDouble(strval2) - Convert.ToDouble(dt.Tables[0].Rows[0][0].ToString()), 3));
        //}


        if (ManualTagEntry == 1)
        {
            DataSet dt = new DataSet();
            if (Session["varMasterCompanyIDForERP"].ToString() == "22" || Session["varMasterCompanyIDForERP"].ToString() == "30")
            {
                dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(Sum(recqty),0) as recqty From V_IndentRecWithIssrecItemflag Where IndentId=" + strval1 + " and Finishedid=" + strval + "");
                val = Convert.ToString(Math.Round(Convert.ToDouble(strval2) - Convert.ToDouble(dt.Tables[0].Rows[0][0].ToString()), 3));
            }
            else
            {
                dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(Sum(recqty),0) as recqty From V_IndentRecWithIssrecItemflag Where IndentId=" + strval1 + " and Finishedid=" + strval + " and Lotno='" + Lotno + "' ");
                val = Convert.ToString(Math.Round(Convert.ToDouble(strval2) - Convert.ToDouble(dt.Tables[0].Rows[0][0].ToString()), 3));

            }
        }
        else
        {
            DataSet dt = new DataSet();
            if (Session["varMasterCompanyIDForERP"].ToString() == "22" || Session["varMasterCompanyIDForERP"].ToString() == "30")
            {
                dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(Sum(recqty),0) as recqty From V_IndentRecWithIssrecItemflag Where IndentId=" + strval1 + " and Finishedid=" + strval + "");
                val = Convert.ToString(Math.Round(Convert.ToDouble(strval2) - Convert.ToDouble(dt.Tables[0].Rows[0][0].ToString()), 3));
            }
            else
            {
                dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(Sum(recqty),0) as recqty From V_IndentRecWithIssrecItemflag Where IndentId=" + strval1 + " and Finishedid=" + strval + " and Lotno='" + Lotno + "' and TagNo='" + Tagno + "'");
                val = Convert.ToString(Math.Round(Convert.ToDouble(strval2) - Convert.ToDouble(dt.Tables[0].Rows[0][0].ToString()), 3));
                
            }
        }




        // DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(Sum(recqty),0) as recqty From V_IndentRecWithIssrecItemflag Where IndentId=" + strval1 + " and Finishedid=" + strval + " and Lotno='" + Lotno + "' and TagNo='" + Tagno + "'");
        //val = Convert.ToString(Math.Round(Convert.ToDouble(strval2) - Convert.ToDouble(dt.Tables[0].Rows[0][0].ToString()), 3));
        return val;
    }

    private void Fill_Grid()
    {
        gvdetail.DataSource = fill_Data_grid();
        gvdetail.DataBind();
    }
    private DataSet fill_Data_grid()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            //            string strsql = @"SELECT VF.CATEGORY_NAME,VF.ITEM_NAME,QualityName +'  '+ designName +'  '+ ColorName +'  '+ ShadeColorName +'  '+ ShapeName +'  '+ SizeMtr Description,
            //                             GM.GodownName,LotNo,PPT.RecQuantity,PPT.PRTid,PPT.TagNo,Ppt.Lossqty,PPT.Rate FROM PP_ProcessRecTran PPT,V_FinishedItemDetail VF,GodownMaster GM
            //                             Where PPT.Finishedid=VF.ITEM_FINISHED_ID And GM.GoDownID=PPT.Godownid And PPT.PRMID=" + HPRMID.Value + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];


            string strsql = @"SELECT VF.CATEGORY_NAME,VF.ITEM_NAME,QualityName +'  '+ designName +'  '+ ColorName +'  '+ ShadeColorName +'  '+ ShapeName +'  '+ SizeMtr Description,
                             GM.GodownName,LotNo,PPT.RecQuantity,PPT.PRTid,PPT.TagNo,Ppt.Lossqty,PPT.Rate,PPT.FinishedId,PPT.GodownId,PPM.CompanyID,ISNULL(PPT.Remark,'') as TagRemark 
                             FROM PP_ProcessRecTran PPT INNER JOIN PP_ProcessRecMaster PPM ON PPM.PRMID=PPT.PRMID
                             INNER JOIN V_FinishedItemDetail VF ON PPT.Finishedid=VF.ITEM_FINISHED_ID
                             INNER JOIN GodownMaster GM ON GM.GoDownID=PPT.Godownid
                             Where PPT.PRMID=" + HPRMID.Value + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            if (ChkEdit.Checked == true)
            {
                if (ddindent.SelectedIndex > 0)
                {
                    strsql = strsql + " And PPT.IndentId=" + ddindent.SelectedValue;
                }
                if (ddChallanNo.SelectedIndex > 0)
                {
                    strsql = strsql + " And PPT.IssPrmID=" + ddChallanNo.SelectedValue;
                }
            }
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
            Logs.WriteErrorLog("Masters_Rawmeterial_rawRecieve|fill_Data_grid|" + ex.Message);
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
    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtidnt.Enabled = true;
        txtidnt.Text = "";
        ddProcessName.SelectedValue = null;
        ddempname.SelectedValue = null;
        ddindent.SelectedValue = null;
        txtcode.Text = "";

        ddCatagory.SelectedValue = null;
        dditemname.SelectedValue = null;
        ddgodown.SelectedValue = null;
        dquality.SelectedValue = null;
        dddesign.SelectedValue = null;
        ddcolor.SelectedValue = null;
        ddshape.SelectedValue = null;
        ddsize.SelectedValue = null;
        ddlshade.SelectedValue = null;
        txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        btnsave.Text = "Save";
        ddCatagory.Enabled = true;
        dditemname.Enabled = true;
        dquality.Enabled = true;
        dddesign.Enabled = true;
        ddcolor.Enabled = true;
        ddshape.Enabled = true;
        ddsize.Enabled = true;
        ddlshade.Enabled = true;
        ddgodown.Enabled = true;
        ddlunit.Enabled = true;
        ddCompName.Enabled = true;
        ddProcessName.Enabled = true;
        ddempname.Enabled = true;
        ddindent.Enabled = true;
        txtdate.Enabled = true;

        Response.Redirect("IndentRawRecieve.aspx");
    }
    protected void gvdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvdetail, "Select$" + e.Row.RowIndex);
            for (int i = 0; i < gvdetail.Columns.Count; i++)
            {
                if (gvdetail.Columns[i].HeaderText == "LossQty")
                {
                    TextBox txtgloss = (TextBox)e.Row.FindControl("txtgloss");
                    if (txtgloss != null)
                    {
                        //txtgloss.Enabled = false;
                    }


                    //if (Session["varMasterCompanyIDForERP"].ToString() != "16")
                    //{
                    //    TextBox txtgloss = (TextBox)e.Row.FindControl("txtgloss");
                    //    if (txtgloss != null)
                    //    {
                    //        txtgloss.Enabled = false;
                    //    }
                    //}
                }
                if (gvdetail.Columns[i].HeaderText == "Qty")
                {
                    if (Session["varMasterCompanyIDForERP"].ToString() != "21")
                    {
                        TextBox txtRecQty = (TextBox)e.Row.FindControl("txtRecQty");
                        if (txtRecQty != null)
                        {
                            txtRecQty.Enabled = false;
                        }
                    }
                }

                if (Session["varMasterCompanyIDForERP"].ToString() == "21")
                {
                    if (gvdetail.Columns[i].HeaderText == "Tag Remark")
                    {
                        gvdetail.Columns[i].Visible = true;
                    }
                }

            }
        }
        if (Convert.ToInt32(Session["VarcompanyNo"]) == 7)
        {
            e.Row.Cells[3].Visible = false; // 3 for lot no
            e.Row.Cells[4].Visible = false;// 4 for godown
        }
        if (hncomp.Value == "10")
        {
            gvdetail.Columns[3].Visible = false;
        }
    }
    protected void gvdetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvdetail.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Session.Remove("issueqty");
        Session.Remove("inhand");
        Session.Remove("finishedid");
        Response.Redirect("../../main.aspx");
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        LblQuality.Text = ParameterList[0];
        LblDesign.Text = ParameterList[1];
        LblColor.Text = ParameterList[2];
        LblShape.Text = ParameterList[3];
        LblSize.Text = ParameterList[4];
        LblCategory.Text = ParameterList[5];
        LblItemName.Text = ParameterList[6];
        LblColorShade.Text = ParameterList[7];
    }
    private void QCSAVE(SqlTransaction Tran, int ReceiveId, int ReceiveDetailId)
    {
        string checkpara = "";
        string noncheck = "";

        for (int i = 0; i < grdqualitychk.Rows.Count; i++)
        {
            CheckBox chk = (CheckBox)grdqualitychk.Rows[i].FindControl("CheckBox1");
            if (chk.Checked)
            {
                if (checkpara == "")
                {
                    checkpara = grdqualitychk.DataKeys[i].Value.ToString();
                }
                else
                {
                    checkpara = checkpara + "," + grdqualitychk.DataKeys[i].Value.ToString();
                }
            }
            else
                if (noncheck == "")
                {
                    noncheck = grdqualitychk.DataKeys[i].Value.ToString();
                }
                else
                {
                    noncheck = noncheck + "," + grdqualitychk.DataKeys[i].Value.ToString();
                }
        }
        SqlParameter[] _arrpara1 = new SqlParameter[5];
        _arrpara1[0] = new SqlParameter("@ReceiveId", SqlDbType.Int);
        _arrpara1[1] = new SqlParameter("@ReceiveDetailId", SqlDbType.Int);
        _arrpara1[2] = new SqlParameter("@checkpara", SqlDbType.NVarChar, 50);
        _arrpara1[3] = new SqlParameter("@noncheck", SqlDbType.NVarChar, 50);
        _arrpara1[4] = new SqlParameter("@TableName", SqlDbType.NVarChar, 50);
        _arrpara1[0].Value = ReceiveId;
        _arrpara1[1].Value = ReceiveDetailId;
        _arrpara1[2].Value = checkpara;
        _arrpara1[3].Value = noncheck;
        _arrpara1[4].Value = "PP_ProcessRecTran";
        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PurchaseReceivequalitychk", _arrpara1);
    }
    private void QCShowORNot()
    {
        if (Convert.ToInt32(ViewState["VARQCTYPE"]) == 1)
        {
            qulitychk.Visible = true;
            fillgrdquality();
            if (Convert.ToInt32(ViewState["flag"]) == 1)
            {
                fillchkbox();
            }
        }
        else
        {
            qulitychk.Visible = false;
        }
    }
    private void fillgrdquality()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select QCMaster.ID,SrNo,ParaName from QCParameter inner join QCMaster on QCParameter.ParaID=QCMaster.ParaID where CategoryID=" + ddCatagory.SelectedValue + " and ItemID=" + dditemname.SelectedValue + " and ProcessID=" + ddProcessName.SelectedValue + " order by SrNo");
        grdqualitychk.DataSource = ds;
        grdqualitychk.DataBind();
    }
    protected void txtcode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds;
        string Str;
        if (txtcode.Text != "")
        {
            Str = "SELECT DISTINCT IM.Category_Id,IPM.* FROM  ITEM_PARAMETER_MASTER IPM INNER JOIN IndentDetail ID ON IPM.ITEM_FINISHED_ID = ID.OFinishedId inner join Item_Master IM on IM.Item_Id=IPM.Item_Id  WHERE ID.IndentId = " + ddindent.SelectedValue + " and  ProductCode='" + txtcode.Text + "' And IPM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddCatagory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                ddlcategorycange();
                dditemname.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                FILL_ITEM_CHANGED();
                dquality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                FILL_QUANTITYCHANGE();
                if (dsn.Visible == true)
                {
                    dddesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                }
                if (clr.Visible == true)
                {
                    ddcolor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                }
                if (shp.Visible == true)
                {
                    ddshape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                }
                if (sz.Visible == true)
                {
                    ddsize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                }
                if (shd.Visible == true)
                {
                    ddlshade.SelectedValue = ds.Tables[0].Rows[0]["SHADECOLOR_ID"].ToString();
                }
                fill_qty();
            }
            else
            {
                txtcode.Text = "";
                txtcode.Focus();
            }
        }
        else
        {
            ddCatagory.SelectedIndex = 0;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();

        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER IPM inner join item_Master IM on IM.Item_Id=IPM.Item_Id inner join CategorySeparate CS on CS.CategoryId=IM.Category_Id  where ProductCode Like  '" + prefixText + "%' And IM.MasterCompanyId=" + MasterCompanyId;
        //string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER  where ProductCode Like  '" + prefixText + "%'";
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(strQuery, con);
        da.Fill(ds);
        count = ds.Tables[0].Rows.Count;
        con.Close();
        List<string> ProductCode = new List<string>();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            ProductCode.Add(ds.Tables[0].Rows[i][0].ToString());
        }
        con.Close();
        return ProductCode.ToArray();
    }
    protected void DDLotNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtlotno.Text = DDLotNo.SelectedItem.Text;
        if (TDTagNo.Visible == true)
        {

            int color = 0;
            int quality = 0;
            int design = 0;
            int shape = 0;
            int size = 0;
            int shadeColor = 0;
            if ((ql.Visible == true && dquality.SelectedIndex > 0) || ql.Visible != true)
            {
                quality = 1;
            }
            if (dsn.Visible == true && dddesign.SelectedIndex > 0 || dsn.Visible != true)
            {
                design = 1;
            }
            if (clr.Visible == true && ddcolor.SelectedIndex > 0 || clr.Visible != true)
            {
                color = 1;
            }
            if (shp.Visible == true && ddshape.SelectedIndex > 0 || shp.Visible != true)
            {
                shape = 1;
            }
            if (sz.Visible == true && ddsize.SelectedIndex > 0 || sz.Visible != true)
            {
                size = 1;
            }
            if (shd.Visible == true && ddlshade.SelectedIndex > 0 || shd.Visible != true)
            {
                shadeColor = 1;
            }
            if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
            {
                //if (ChkForReceiveIssItem.Checked == false)
                //{
                int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, txtcode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                hnfinishedid.Value = Varfinishedid.ToString();
                FillTagNo(Varfinishedid);
                if (DDTagNo.Items.Count > 0)
                {
                    DDTagNo.SelectedIndex = 0;
                    txttagNo.Text = "";
                }
            }
        }
        if (TDTagNo.Visible == false)
        {
            fill_Order_qty();
        }
    }
    protected void ChkForReceiveAnyColor_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForReceiveAnyColor.Checked == true)
        {
            ChkForReceiveIssItem.Checked = false;
            ChkForReceiveIssItem.Enabled = false;
            ReceiveIssItemCheckedChanged();
            FILL_QUANTITYCHANGE();
            TDRate.Visible = true;
            TdChkWithoutRate.Visible = false;
        }
        else
        {
            ChkForReceiveIssItem.Enabled = true;
            TDRate.Visible = false;
            TdChkWithoutRate.Visible = true;
        }
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strSize, str;
        if (DDsizetype.SelectedIndex == 0)
        {
            strSize = " Sizeft";
        }
        else if (DDsizetype.SelectedIndex == 1)
        {
            strSize = "Sizemtr";
        }
        else
        {
            strSize = "Sizeinch";
        }
        if (ChkForReceiveIssItem.Checked == true)
        {
            //            UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size." + strSize + @" FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
            //                        dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
            //                        dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
            //                        WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Size in Ft");

            str = @"select DISTINCT S.SizeId, S." + strSize + @" from PP_ProcessRawMaster PM inner join PP_ProcessRawTran PT
                                    on PM.PRMid=PT.PRMid inner join IndentMaster IM on IM.IndentID=pt.Indentid
                                    inner join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID
                                    inner join size S on s.SizeId=vf.SizeId
                                    Where  IM.IndentId=" + ddindent.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

            UtilityModule.ConditionalComboFill(ref ddsize, str, true, "Size in Ft");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size." + strSize + @" FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                        dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.OFinishedId INNER JOIN
                        dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
                        WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select Size--");
        }
    }

    private void MessageSave(string msg)
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('");
        stb.Append(msg);
        stb.Append("');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
    protected void GetDyingTypes()
    {
        //GetDyingTypes
        string str = "select DyingMatch,Dyeing,DyeingType from indentdetail  where ofinishedid=" + Item_finished_id + " and indentid=" + ddindent.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ChkForReceiveIssItem.Checked == false)
            {
                //TRdyingTypes.Visible = true;
                TDDyeingMatch.Visible = true;
                TDDyingType.Visible = true;
                TDDyeing.Visible = true;
            }
            DDDyeingMatch.SelectedItem.Text = ds.Tables[0].Rows[0]["DyingMatch"].ToString();
            DDDyingType.SelectedItem.Text = ds.Tables[0].Rows[0]["DyeingType"].ToString();
            DDDyeing.SelectedItem.Text = ds.Tables[0].Rows[0]["Dyeing"].ToString();
        }
        else
        {
            //TRdyingTypes.Visible = false;
            TDDyeingMatch.Visible = false;
            TDDyingType.Visible = false;
            TDDyeing.Visible = false;
        }


    }


    protected void DDTagNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        txttagNo.Text = DDTagNo.SelectedItem.Text;
        fill_Order_qty();
    }

    protected void gvdetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        ChkForReDyeing.Enabled = false;
        gvdetail.EditIndex = e.NewEditIndex;
        Fill_Grid();
    }
    protected void gvdetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        ChkForReDyeing.Enabled = true;
        gvdetail.EditIndex = -1;
        Fill_Grid();
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
    protected void gvdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Popup(true);
        txtpwd.Focus();
        rowindex = e.RowIndex;

    }
    protected void Updatedetails(int rowindex)
    {

        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarProcess_Issue_Detail_Id = Convert.ToInt32(gvdetail.DataKeys[rowindex].Value);
            TextBox txtrate = (TextBox)gvdetail.Rows[rowindex].FindControl("txtrate");
            TextBox txtgloss = (TextBox)gvdetail.Rows[rowindex].FindControl("txtgloss");
            TextBox txtRecQty = (TextBox)gvdetail.Rows[rowindex].FindControl("txtRecQty");

            SqlParameter[] _arrPara = new SqlParameter[12];
            _arrPara[0] = new SqlParameter("@PRTID", VarProcess_Issue_Detail_Id);
            _arrPara[1] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            _arrPara[1].Direction = ParameterDirection.Output;
            _arrPara[2] = new SqlParameter("@Userid", Session["varuserid"]);
            _arrPara[3] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            _arrPara[4] = new SqlParameter("@Rate", txtrate.Text == "" ? "0" : txtrate.Text);
            _arrPara[5] = new SqlParameter("@Lossqty", txtgloss.Text == "" ? "0" : txtgloss.Text);
            _arrPara[6] = new SqlParameter("@Indentid", ddindent.SelectedValue);
            _arrPara[7] = new SqlParameter("@Checkedby", txtcheckedby.Text);
            _arrPara[8] = new SqlParameter("@Approvedby", txtapprovedby.Text);
            _arrPara[9] = new SqlParameter("@BillNo", txtBillNo.Text);
            _arrPara[10] = new SqlParameter("@ReDyeingStatus", ChkForReDyeing.Checked == true ? 1 : 0);
            _arrPara[11] = new SqlParameter("@RecQuantity", txtRecQty.Text == "" ? "0" : txtRecQty.Text);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PP_PRM_RECEIVE_UPDATE", _arrPara);
            Tran.Commit();
            gvdetail.EditIndex = -1;
            Fill_Grid();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = _arrPara[1].Value.ToString();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void txtpwd_TextChanged(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        if (variable.VarDyeingeditpwd == txtpwd.Text)
        {
            Updatedetails(rowindex);
            Popup(false);
        }
        else
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Please Enter Correct Password..";
        }
        gvdetail.EditIndex = -1;
        Fill_Grid();
    }

    protected void ddgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkForReceiveIssItem.Checked == true)
        {
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, txtcode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));

            if (variable.VarCHECKBINCONDITION == "1")
            {
                UtilityModule.FillBinNO(ddbinno, Convert.ToInt32(ddgodown.SelectedValue), Varfinishedid, New_Edit: 0);
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref ddbinno, @"Select Distinct PRT.BInNo,PRT.BinNo From IndentDetail ID,PP_ProcessRawtran PRT 
                Where ID.IndentId=PRT.IndentId And PRT.IndentId=" + ddindent.SelectedValue + " And PRMid=" + ddChallanNo.SelectedValue + " and PRT.finishedid=" + Varfinishedid + "", true, "--Select--");
            }

        }
        else
        {
            if (variable.VarCHECKBINCONDITION == "1")
            {
                int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, txtcode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                UtilityModule.FillBinNO(ddbinno, Convert.ToInt32(ddgodown.SelectedValue), Varfinishedid, New_Edit: 0);
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref ddbinno, "select BInId,BInNo From Binmaster Where GODOWNID=" + ddgodown.SelectedValue + " order by BinNo", true, "--Plz Select--");
            }

        }

    }

    protected void chkchangeTagno_CheckedChanged(object sender, EventArgs e)
    {
        txttagNo.Enabled = false;
        if (chkchangeTagno.Checked == true)
        {
            txttagNo.Enabled = true;
        }
    }
    protected void chkchangeLotno_CheckedChanged(object sender, EventArgs e)
    {
        txtlotno.Enabled = false;
        if (chkchangeLotno.Checked == true)
        {
            txtlotno.Enabled = true;
        }
    }
    protected void ChkForReDyeing_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForReDyeing.Checked == true)
        {
            if(MasterCompanyId==21)
            {
                ChkForReceiveAnyColor.Visible =true ;
            }
            ChkForReceiveIssItem.Visible = false;
        }
        else
        {
            ChkForReceiveIssItem.Visible = true;
        }
    }
    protected void lnkupdatebillNo_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@prmid", ddPartyChallanNo.SelectedValue);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@MastercompanyId", Session["varMasterCompanyIDForERP"]);
            param[4] = new SqlParameter("@ChallanNo", TxtPartyChallanNo.Text);
            param[5] = new SqlParameter("@BillNo", txtBillNo.Text);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEINDENTRECBILLNO_CHALLANNO", param);
            Tran.Commit();
            lblmsg.Text = param[1].Value.ToString();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altupd", "alert('" + param[1].Value.ToString() + "')", true);
            ddPartyChallanNo_SelectedIndexChanged(sender, new EventArgs());
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void BtnUpdateStatus_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        LblErrorMessage.Visible = false;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] parparam = new SqlParameter[3];
            parparam[0] = new SqlParameter("@IndentID", ddindent.SelectedValue);
            parparam[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            parparam[1].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "PRO_UPDATEINDENTSTATUS_COMPLETE", parparam);
            tran.Commit();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = parparam[1].Value.ToString();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/IndentRawRecieve.aspx");
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void BtnForComplete_Click(object sender, EventArgs e)
    {
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update IndentMaster Set Status = 'Complete' Where IndentID = " + ddindent.SelectedValue);
        ScriptManager.RegisterStartupScript(Page, GetType(), "altupd", "alert('Status update successfully')", true);

    }
}
