using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_process_IndentRowIssuenew : System.Web.UI.Page
{
    int ItemFinishedId = 0;
    static int MasterCompanyId;

    //string remainstock;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varMasterCompanyIDForERP"]);

        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {

            hnqty.Value = "0";
            ViewState["Prmid"] = 0;
            ViewState["Prtid"] = 0;

            string str = @"select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By Companyname
                          select DISTINCT PROCESS_NAME_ID,process_name from PROCESS_NAME_MASTER pm inner join IndentMaster im on pm.PROCESS_NAME_ID=im.processid And pm.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by PROCESS_NAME_ID
                         Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName
                         select godownid From Modulewisegodown Where ModuleName='" + Page.Title + @"'
                 Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"];

            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "Select Comp Name");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 4, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddProcessName, ds, 1, true, "Select Process Name");
            UtilityModule.ConditionalComboFillWithDS(ref ddgodown, ds, 2, true, "Select Godown");
            if (ds.Tables[3].Rows.Count > 0)
            {
                if (ddgodown.Items.FindByValue(ds.Tables[3].Rows[0]["godownid"].ToString()) != null)
                {
                    ddgodown.SelectedValue = ds.Tables[3].Rows[0]["godownid"].ToString();
                }
            }

            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            lablechange();
            Session["ReportPath"] = "Reports/IndentRawIssue.rpt";
            Session["CommanFormula"] = "{V_IndentRawIssue.PRMid}=" + ViewState["Prmid"];
            //Without BOM
            if (Session["withoutBOM"].ToString() == "1")
            {
                ChKForOrder.Checked = true;
                ChKForOrder_CheckedChanged(sender, e);
            }
            switch (Session["varcompanyNo"].ToString())
            {
                case "5":
                    txtchalanno.ReadOnly = true;
                    break;
                case "6":
                    txtchalanno.ReadOnly = true;
                    TDInputDesc.Visible = true;
                    break;
                case "7":
                    if (ddProcessName.Items.Count > 0)
                    {
                        ddProcessName.SelectedIndex = 1;
                        dprocess_change();
                    }
                    ChKForOrder.Checked = true;
                    cchkforoorder_checked();
                    btnnew.Visible = true;
                    btnstatus.Visible = true;
                    break;
                case "10":
                    TDLotNo.Visible = false;
                    break;
                case "12":
                    txtchalanno.ReadOnly = true;
                    TDInputDesc.Visible = true;
                    break;
                case "16":
                    TDMoisture.Visible = true;
                    break;
                case "28":
                    TDMoisture.Visible = true;
                    break;
                case "42":
                    TDMoisture.Visible = true;
                    break;
                case "43":
                    Label17.Text = "UCN No";
                    break;
                case "22":
                    TDGenerateIndentDate.Visible = true;
                    break;
            }
            if (MySession.TagNowise == "1")
            {
                TDTagNo.Visible = true;
            }
            if (variable.VarBINNOWISE == "1")
            {
                TDBinNo.Visible = true;
            }
            if (variable.VarIndentRawIssueRateManual == "1")
            {
                TDManualRate.Visible = true;
            }
            if (variable.VarGSTForIndentRawIssue == "1")
            {
                TDGSTType.Visible = true;
                ChkForGSTReport.Visible = true;
            }

            if (variable.VarCompanyWiseChallanNoGenerated == "1")
            {
                txtchalanno.Enabled = false;
            }
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        lblSize.Text = ParameterList[4];
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
        lblshadecolor.Text = ParameterList[7];
    }
    protected void ddProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        dprocess_change();
    }
    private void dprocess_change()
    {
        UtilityModule.ConditionalComboFill(ref ddempname, "SELECT distinct E.EmpId,E.EmpName FROM IndentMaster IM INNER JOIN  EmpInfo E ON IM.PartyId=E.EmpId And IM.Processid=" + ddProcessName.SelectedValue + " ANd IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And IM.CompanyId=" + ddCompName.SelectedValue + " order by E.EmpName", true, "Select Emp");
    }
    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_emp_change();
    }
    private void fill_emp_change()
    {
        string str = "";
        if (Session["varcompanyno"].ToString() == "7")
        {
            str = @"select distinct im.IndentId, IndentNo+'/'+localorder + '/'+CustomerOrderNo as IndentNo 
                from IndentMaster im 
                inner join Indentdetail id on id.IndentID=im.IndentID 
                inner join ordermaster om on om.orderid=id.orderid 
                inner join orderdetail od On om.orderid=od.orderid 
                inner join V_FinishedItemDetail v On od.Item_Finished_Id= v.Item_Finished_Id 
                inner join UserRights_Category uc On v.CATEGORY_ID=uc.CategoryId  
                left outer join OrderProcessPlanning pm On om.orderid=pm.orderid 
                Where  om.status=0 and pm.FinalStatus=1 AND im.Status not in('complete','cancelled')  and  uc.userid=" + Session["varuserid"] + @" and  
                im.partyid=" + ddempname.SelectedValue + " and im.processid=" + ddProcessName.SelectedValue + " And im.CompanyId=" + ddCompName.SelectedValue + @" And 
                im.BranchID = " + DDBranchName.SelectedValue + " And Im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        }
        else if (Session["varcompanyNo"].ToString() == "3" || Session["varcompanyNo"].ToString() == "10")
        {
            str = @"select distinct im.IndentId, IndentNo+'/'+localorder + '/'+CustomerOrderNo as IndentNo from IndentMaster im inner join 
                Indentdetail id on id.IndentID=im.IndentID inner join ordermaster om on om.orderid=id.orderid inner join orderdetail od On om.orderid=od.orderid inner join 
                V_FinishedItemDetail v On od.Item_Finished_Id= v.Item_Finished_Id 
                Where  om.status=0  AND im.Status not in('complete','cancelled') and  im.partyid=" + ddempname.SelectedValue + @" and 
                im.processid=" + ddProcessName.SelectedValue + " And im.CompanyId=" + ddCompName.SelectedValue + " And im.BranchID = " + DDBranchName.SelectedValue + @" And 
                Im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        }
        else
        {
            if (MySession.IndentAsProduction == "1")
            {
                if (ChKForOrder.Checked == true)
                {
                    str = @"select distinct im.IndentId, IndentNo+'/'+localorder + '/'+CustomerOrderNo as IndentNo from IndentMaster im inner join 
                Indentdetail id on id.IndentID=im.IndentID inner join ordermaster om on om.orderid=id.orderid inner join orderdetail od On om.orderid=od.orderid inner join 
                V_FinishedItemDetail v On od.Item_Finished_Id= v.Item_Finished_Id 
                Where  om.status=0  AND im.Status not in('complete','cancelled') and  im.partyid=" + ddempname.SelectedValue + @" 
                and im.processid=" + ddProcessName.SelectedValue + " And im.CompanyId=" + ddCompName.SelectedValue + " And im.BranchID = " + DDBranchName.SelectedValue + @" And 
                Im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                }
                else
                {
                    int Approvalflag = 0;
                    if (Session["varMasterCompanyIDForERP"].ToString() == "6")
                    {
                        Approvalflag = 1;
                    }
                    str = @"select distinct im.IndentId, Customercode+'/'+IndentNo+'/'+localorder +'/'+CustomerOrderNo as IndentNo from IndentMaster im inner join 
                 Indentdetail id on id.IndentID=im.IndentID inner join ordermaster om on om.orderid=id.orderid inner join Customerinfo CI on  CI.CustomerId=Om.CustomerId
                 Where im.Approvalflag=" + Approvalflag + " and  im.partyid=" + ddempname.SelectedValue + @" AND im.Status not in('complete','cancelled') and 
                 im.processid=" + ddProcessName.SelectedValue + " And im.Companyid=" + ddCompName.SelectedValue + " And im.BranchID = " + DDBranchName.SelectedValue + @" 
                 And im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                }
            }
            else
            {
                if (Session["WithoutBOM"].ToString() == "1")
                {
                    str = @"select distinct im.IndentId, IndentNo+'/'+localorder + '/'+CustomerOrderNo as IndentNo from IndentMaster im inner join 
                Indentdetail id on id.IndentID=im.IndentID inner join ordermaster om on om.orderid=id.orderid inner join orderdetail od On om.orderid=od.orderid inner join 
                V_FinishedItemDetail v On od.Item_Finished_Id= v.Item_Finished_Id 
                Where  om.status=0  AND  im.Status Not in('complete','cancelled') and  im.partyid=" + ddempname.SelectedValue + @" and 
                im.BranchID = " + DDBranchName.SelectedValue + " And im.processid=" + ddProcessName.SelectedValue + " And im.CompanyId=" + ddCompName.SelectedValue + @" And 
                Im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                }
                else
                {
                    if (Session["varMasterCompanyIDForERP"].ToString() == "9")
                    {
                        str = @"select distinct im.IndentId, localorder +'/'+IndentNo as IndentNo from IndentMaster im inner join 
                 Indentdetail id on id.IndentID=im.IndentID inner join ordermaster om on om.orderid=id.orderid
                 Where im.Approvalflag=1 and  im.partyid=" + ddempname.SelectedValue + @" AND im.Status not in('complete','cancelled') and 
                 im.processid=" + ddProcessName.SelectedValue + " And im.CompanyId=" + ddCompName.SelectedValue + " And im.BranchID = " + DDBranchName.SelectedValue + @" And 
                 im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                    }
                    else
                    {
                        str = @"select distinct im.IndentId, IndentNo+'/'+localorder +'/'+CustomerOrderNo as IndentNo from IndentMaster im inner join 
                 Indentdetail id on id.IndentID=im.IndentID inner join ordermaster om on om.orderid=id.orderid
                 Where im.Approvalflag=1 and  im.partyid=" + ddempname.SelectedValue + @" AND im.Status not in('complete','cancelled') and 
                 im.processid=" + ddProcessName.SelectedValue + " And im.CompanyId=" + ddCompName.SelectedValue + " And im.BranchID = " + DDBranchName.SelectedValue + @" And 
                 im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                    }
                }
            }
        }
        UtilityModule.ConditionalComboFill(ref ddindentno, str, true, "Select Order No");
        switch (Session["varcompanyNo"].ToString())
        {
            case "7":
                Btnorder.Visible = true;
                break;
            case "3":
                Btnorder.Visible = true;
                break;
            case "10":
                Btnorder.Visible = true;
                break;
        }

    }
    protected void ddindentno_SelectedIndexChanged(object sender, EventArgs e)
    {


        string stritems = @"select OD.OrderDetailId,Vf.ITEM_NAME+' '+VF.QualityName+' '+Vf.designName+' '+Vf.ColorName+' '+vf.ShapeName+' '+case when OD.flagsize=1 Then Vf.SizeMtr When OD.flagsize=2 Then
                    vf.sizeinch ELse vf.sizeft End+' '+case when Vf.SizeId>0 Then Sz.Type Else '' ENd as ItemDescription
                    From  IndentDetail pp inner join OrderDetail OD on pp.Orderdetailid =od.OrderDetailId inner join OrderMaster Om  on Om.OrderId=Od.OrderId
                     --and pp.OrderDetailId=od.OrderDetailId
                    inner Join V_FinishedItemDetail vf on OD.Item_Finished_Id=vf.ITEM_FINISHED_ID
                    Left join SizeType Sz on Od.flagsize=Sz.val Where OM.Companyid=" + ddCompName.SelectedValue + "and pp.IndentId=" + ddindentno.SelectedValue + @" 
                     group by OD.OrderDetailId,Vf.ITEM_NAME,VF.QualityName,Vf.designName,Vf.ColorName,
					vf.ShapeName,OD.flagsize,Vf.SizeMtr,vf.sizeft,sizeinch,Vf.SizeId,Sz.Type order by OD.orderdetailid";
        UtilityModule.ConditionalComboFill(ref DDitemdescription, stritems, true, "--Plz Select--");




      
    }

    protected void DDitemdescription_SelectedIndexChanged(object sender, EventArgs e)
    {

        int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyType From MasterSetting"));
        switch (VarCompanyNo)
        {
            case 1:
                procode.Visible = false;
                break;
            case 2:
                procode.Visible = true;
                break;
        }
//        if (Convert.ToInt32(Session["varcompanyNo"]) == 42)
//        {
//            UtilityModule.ConditionalComboFill(ref ddgodown, @"Select Distinct ID.GodownID, GM.GodownName  
//                From IndentDetail ID(Nolock)
//                JOIN GodownMaster GM(Nolock) ON GM.GoDownID = ID.GodownID 
//                Where IndentId = " + ddindentno.SelectedValue + @" 
//                Order By GM.GodownName ", true, "Select Godown");
//        }

//        if (Convert.ToInt32(Session["varcompanyNo"]) == 22)
//        {
//            string str2 = "";
//            str2 = "select IsNull(REPLACE(CONVERT(NVARCHAR(11),Date,106),' ','-'),'') As GenerateIndentDate  From IndentMaster where IndentId=" + ddindentno.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
//            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
//            if (ds2.Tables[0].Rows.Count > 0)
//            {
//                txtGenerateIndentDate.Text = ds2.Tables[0].Rows[0]["GenerateIndentDate"].ToString();
//            }
//        }

        fillordergrid();
        trorder.Visible = true;
        //Input Description
        if (TDInputDesc.Visible == true)
        {
            FillInputDescription();
        }
        //

        string str = "select IsNull(OrderWiseFlag,0) As OrderWiseFlag  From IndentMaster where IndentId=" + ddindentno.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["OrderWiseFlag"]) == 1)
            {
                ChKForOrder.Checked = true;
                checkBoxEvent();
                TDForOrderWise.Visible = true;
                if (DDitemdescription.SelectedIndex > 0)
                {
                    UtilityModule.ConditionalComboFill(ref ddCatagory, @"  SELECT DISTINCT icm.CATEGORY_ID , icm.CATEGORY_NAME  FROM ITEM_MASTER im INNER JOIN
                 ITEM_PARAMETER_MASTER ipm ON im.ITEM_ID = ipm.ITEM_ID And ipm.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN ITEM_CATEGORY_MASTER icm ON im.CATEGORY_ID = icm.CATEGORY_ID INNER JOIN
                 OrderLocalConsumption OC  ON  ipm.ITEM_FINISHED_ID = OC.FinishedId Inner join UserRights_Category UC on(icm.Category_ID=UC.CategoryId And UC.UserId=" + Session["varuserid"] + @")
                 Where OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ") and IndentId =" + ddindentno.SelectedValue + ")", true, "Select Category Name");
                    CommanFunction.FillCombo(ddlunit, @"Select DISTINCT U.UnitId,UnitName from OrderLocalConsumption OC,Unit U
                 Where  U.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And U.UnitId=OC.UnitId And OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ") Order By U.UnitId");


                }
                else
                {

                    UtilityModule.ConditionalComboFill(ref ddCatagory, @"  SELECT DISTINCT icm.CATEGORY_ID , icm.CATEGORY_NAME  FROM ITEM_MASTER im INNER JOIN
                 ITEM_PARAMETER_MASTER ipm ON im.ITEM_ID = ipm.ITEM_ID And ipm.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN ITEM_CATEGORY_MASTER icm ON im.CATEGORY_ID = icm.CATEGORY_ID INNER JOIN
                 OrderLocalConsumption OC  ON  ipm.ITEM_FINISHED_ID = OC.FinishedId Inner join UserRights_Category UC on(icm.Category_ID=UC.CategoryId And UC.UserId=" + Session["varuserid"] + @")
                 Where OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ")", true, "Select Category Name");
                    CommanFunction.FillCombo(ddlunit, @"Select DISTINCT U.UnitId,UnitName from OrderLocalConsumption OC,Unit U
                 Where  U.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And U.UnitId=OC.UnitId And OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ") Order By U.UnitId");
                
                
                
                
                }

//                switch (Session["varcompanyNo"].ToString())
//                {
//                    case "7":
//                        UtilityModule.ConditionalComboFill(ref ddCatagory, @"  SELECT DISTINCT icm.CATEGORY_ID , icm.CATEGORY_NAME  FROM ITEM_MASTER im INNER JOIN
//                 ITEM_PARAMETER_MASTER ipm ON im.ITEM_ID = ipm.ITEM_ID And ipm.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN ITEM_CATEGORY_MASTER icm ON im.CATEGORY_ID = icm.CATEGORY_ID INNER JOIN
//                 OrderLocalConsumption OC  ON  ipm.ITEM_FINISHED_ID = OC.FinishedId Inner join UserRights_Category UC on(icm.Category_ID=UC.CategoryId And UC.UserId=" + Session["varuserid"] + @")
//                 Where OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ")", true, "Select Category Name");

//                        CommanFunction.FillCombo(ddlunit, @"Select DISTINCT U.UnitId,UnitName from OrderLocalConsumption OC,Unit U
//                 Where  U.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And U.UnitId=OC.UnitId And OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ") Order By U.UnitId");
//                        break;
                   // default:
                       
                 //       break;
               // }
                Fill_GridForLocalConsump();
            }
            else
            {
                ChKForOrder.Checked = false;
                TDForOrderWise.Visible = false;
                checkBoxEvent();

                if (MySession.IndentAsProduction == "1")
                {
                    UtilityModule.ConditionalComboFill(ref ddCatagory, @"  select distinct vf.CATEGORY_ID,vf.CATEGORY_NAME from indentmaster idm inner join IndentDetail ID on idm.indentid=id.indentid inner join PP_Consumption PP
                    on ID.PPNo=PP.PPId and Id.Ofinishedid=PP.Finishedid  inner join V_FinishedItemDetail  vf on vf.ITEM_FINISHED_ID=pp.IFinishedid
                 Where idm.partyid=" + ddempname.SelectedValue + " and idm.processid=" + ddProcessName.SelectedValue + " and  idm.IndentId=" + ddindentno.SelectedValue, true, "Select Category Name");
                }
                else
                {
                    if(DDitemdescription.SelectedIndex>0)
                    {
                    UtilityModule.ConditionalComboFill(ref ddCatagory, @"  SELECT DISTINCT icm.CATEGORY_ID , icm.CATEGORY_NAME  FROM ITEM_MASTER im INNER JOIN
                 ITEM_PARAMETER_MASTER ipm ON im.ITEM_ID = ipm.ITEM_ID And ipm.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN ITEM_CATEGORY_MASTER icm ON im.CATEGORY_ID = icm.CATEGORY_ID INNER JOIN
                 IndentDetail  id INNER JOIN   IndentMaster idm ON id.IndentId = idm.IndentID ON  ipm.ITEM_FINISHED_ID = id.IFinishedId Inner join UserRights_Category UC on(icm.Category_ID=UC.CategoryId And UC.UserId=" + Session["varuserid"] + @")
                 Where idm.partyid=" + ddempname.SelectedValue + " and idm.processid=" + ddProcessName.SelectedValue + " and  idm.IndentId=" + ddindentno.SelectedValue+" and  id.orderdetailid=" + DDitemdescription.SelectedValue, true, "Select Category Name");
                    }
                    else
                    {
                        UtilityModule.ConditionalComboFill(ref ddCatagory, @"  SELECT DISTINCT icm.CATEGORY_ID , icm.CATEGORY_NAME  FROM ITEM_MASTER im INNER JOIN
                 ITEM_PARAMETER_MASTER ipm ON im.ITEM_ID = ipm.ITEM_ID And ipm.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN ITEM_CATEGORY_MASTER icm ON im.CATEGORY_ID = icm.CATEGORY_ID INNER JOIN
                 IndentDetail  id INNER JOIN   IndentMaster idm ON id.IndentId = idm.IndentID ON  ipm.ITEM_FINISHED_ID = id.IFinishedId Inner join UserRights_Category UC on(icm.Category_ID=UC.CategoryId And UC.UserId=" + Session["varuserid"] + @")
                 Where idm.partyid=" + ddempname.SelectedValue + " and idm.processid=" + ddProcessName.SelectedValue + " and  idm.IndentId=" + ddindentno.SelectedValue , true, "Select Category Name");
                    
                    
                    
                    }
                }
                CommanFunction.FillCombo(ddlunit, @"Select DISTINCT U.UnitId,UnitName from PP_Consumption PP,IndentDetail ID,ORDER_CONSUMPTION_DETAIL OCD,Unit U,ProcessProgram P
                 Where ID.PPNo=PP.PPId And OCD.OrderDetailId=PP.OrderDetailId And U.UnitId=OCD.IUnitId And P.PPID=PP.PPId And OCD.ProcessiD=P.Process_iD
                 And Id.IndentId=" + ddindentno.SelectedValue + " And P.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By U.UnitId");
            }
        }




    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
    }
    private void ddlcategorycange()
    {
        dditemname.Items.Clear();
        dquality.Items.Clear();
        dddesign.Items.Clear();
        ddcolor.Items.Clear();
        ddshape.Items.Clear();
        ddsize.Items.Clear();
        ddlshade.Items.Clear();
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
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
                        if (ChKForOrder.Checked == true)
                        {
                            UtilityModule.ConditionalComboFill(ref dddesign, @"SELECT DISTINCT dbo.Design.designId, dbo.Design.designName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      OrderLocalConsumption OC ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = OC.FinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.Design ON dbo.ITEM_PARAMETER_MASTER.DESIGN_ID = dbo.Design.designId
                       WHERE  OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ")", true, "--Select Design--");
                        }
                        else
                        {
                            if (MySession.IndentAsProduction == "1")
                            {
                                UtilityModule.ConditionalComboFill(ref dddesign, @"select distinct vf.designId,vf.designName from IndentDetail ID inner join PP_Consumption PP
                                on ID.PPNo=PP.PPId and Id.Ofinishedid=PP.Finishedid   inner join V_FinishedItemDetail  vf on vf.ITEM_FINISHED_ID=pp.IFinishedid
                                WHERE  ID.IndentId =" + ddindentno.SelectedValue + "  and vf.category_id=" + ddCatagory.SelectedValue, true, "--Select Design--");
                            }
                            else
                            {
                                if (DDitemdescription.SelectedIndex > 0)
                                {
                                    UtilityModule.ConditionalComboFill(ref dddesign, @"SELECT DISTINCT dbo.Design.designId, dbo.Design.designName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.Design ON dbo.ITEM_PARAMETER_MASTER.DESIGN_ID = dbo.Design.designId
                       WHERE  dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " and dbo.IndentDetail.orderdetailid =" + DDitemdescription.SelectedValue, true, "--Select Design--");
                                }
                                else
                                {

                                    UtilityModule.ConditionalComboFill(ref dddesign, @"SELECT DISTINCT dbo.Design.designId, dbo.Design.designName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.Design ON dbo.ITEM_PARAMETER_MASTER.DESIGN_ID = dbo.Design.designId
                       WHERE  dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue , true, "--Select Design--");
                                
                                
                                
                                }
                            }
                        }
                        break;
                    case "3":
                        clr.Visible = true;
                        if (ChKForOrder.Checked == true)
                        {
                            UtilityModule.ConditionalComboFill(ref ddcolor, @"SELECT DISTINCT dbo.color.ColorId, dbo.color.ColorName FROm dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      OrderLocalConsumption OC ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = OC.FinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.color ON dbo.ITEM_PARAMETER_MASTER.COLOR_ID = dbo.color.ColorId
                      WHERE OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ")", true, "--Select Color--");
                        }
                        else
                        {
                            if (MySession.IndentAsProduction == "1")
                            {
                                UtilityModule.ConditionalComboFill(ref ddcolor, @"select distinct vf.ColorId,vf.ColorName from IndentDetail ID inner join PP_Consumption PP
                                on ID.PPNo=PP.PPId and Id.Ofinishedid=PP.Finishedid   inner join V_FinishedItemDetail  vf on vf.ITEM_FINISHED_ID=pp.IFinishedid
                                WHERE  ID.IndentId =" + ddindentno.SelectedValue + "  and vf.category_id=" + ddCatagory.SelectedValue, true, "--Select Color--");
                            }
                            else
                            {
                                if (DDitemdescription.SelectedIndex > 0)
                                {
                                    UtilityModule.ConditionalComboFill(ref ddcolor, @"SELECT DISTINCT dbo.color.ColorId, dbo.color.ColorName FROm dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.color ON dbo.ITEM_PARAMETER_MASTER.COLOR_ID = dbo.color.ColorId
                      WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " and isnull(IndentDetail.orderdetailid,0)= " + DDitemdescription.SelectedValue + "", true, "--Select Color--");
                                }
                                else
                                {

                                    UtilityModule.ConditionalComboFill(ref ddcolor, @"SELECT DISTINCT dbo.color.ColorId, dbo.color.ColorName FROm dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.color ON dbo.ITEM_PARAMETER_MASTER.COLOR_ID = dbo.color.ColorId
                      WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue , true, "--Select Color--");
                                }
                            }
                        }
                        break;
                    case "4":
                        shp.Visible = true;
                        if (ChKForOrder.Checked == true)
                        {
                            UtilityModule.ConditionalComboFill(ref ddshape, @"SELECT DISTINCT dbo.Shape.ShapeId, dbo.Shape.ShapeName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      OrderLocalConsumption OC ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = OC.FinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.Shape ON dbo.ITEM_PARAMETER_MASTER.SHAPE_ID = dbo.Shape.ShapeId
                      WHERE OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ")", true, "--Select Shape--");
                        }
                        else
                        {
                            if (MySession.IndentAsProduction == "1")
                            {
                                UtilityModule.ConditionalComboFill(ref ddshape, @"select distinct vf.ShapeId,vf.ShapeName from IndentDetail ID inner join PP_Consumption PP
                                on ID.PPNo=PP.PPId and Id.Ofinishedid=PP.Finishedid  inner join V_FinishedItemDetail  vf on vf.ITEM_FINISHED_ID=pp.IFinishedid
                                WHERE  ID.IndentId =" + ddindentno.SelectedValue + "  and vf.category_id=" + ddCatagory.SelectedValue, true, "--Select Shape--");
                            }
                            else
                            {
                                if (DDitemdescription.SelectedIndex > 0)
                                {

                                    UtilityModule.ConditionalComboFill(ref ddshape, @"SELECT DISTINCT dbo.Shape.ShapeId, dbo.Shape.ShapeName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.Shape ON dbo.ITEM_PARAMETER_MASTER.SHAPE_ID = dbo.Shape.ShapeId
                      WHERE dbo.IndentDetail.IndentId = " + ddindentno.SelectedValue + " and isnull(IndentDetail.orderdetailid,0)=" + DDitemdescription.SelectedValue + " ", true, "--Select Shape--");
                                }
                                else
                                {
                                    UtilityModule.ConditionalComboFill(ref ddshape, @"SELECT DISTINCT dbo.Shape.ShapeId, dbo.Shape.ShapeName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.Shape ON dbo.ITEM_PARAMETER_MASTER.SHAPE_ID = dbo.Shape.ShapeId
                      WHERE dbo.IndentDetail.IndentId = " + ddindentno.SelectedValue , true, "--Select Shape--");
                                
                                }
                            }
                        }
                        break;
                    case "5":
                        sz.Visible = true;
                        //CheckBox1.Checked = false;
                        if (ChKForOrder.Checked == true)
                        {
                            UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size.SizeFt FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      OrderLocalConsumption OC ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = OC.FinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
                      WHERE OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ")", true, "Size in Ft");
                        }
                        else
                        {
                            if (MySession.IndentAsProduction == "1")
                            {
                                UtilityModule.ConditionalComboFill(ref ddsize, @"select distinct vf.SizeId,vf.SizeFt from IndentDetail ID inner join PP_Consumption PP
                                on ID.PPNo=PP.PPId and Id.Ofinishedid=PP.Finishedid   inner join V_FinishedItemDetail  vf on vf.ITEM_FINISHED_ID=pp.IFinishedid
                                WHERE  ID.IndentId =" + ddindentno.SelectedValue + "  and vf.category_id=" + ddCatagory.SelectedValue, true, "--Select Size--");
                            }
                            else
                            {
                                if (DDitemdescription.SelectedIndex > 0)
                                {


                                    UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size.SizeFt FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
                      WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " and isnull(IndentDetail.orderdetailid,0)= " + DDitemdescription.SelectedValue + "", true, "Size in Ft");
                                }
                                else
                                {
                                    UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size.SizeFt FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
                      WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue , true, "Size in Ft");
                                
                                
                                }

                            }
                        }
                        break;
                    case "6":
                        shd.Visible = true;
                        break;
                    case "10":
                        clr.Visible = true;
                        if (ChKForOrder.Checked == true)
                        {
                            UtilityModule.ConditionalComboFill(ref ddcolor, @"SELECT DISTINCT dbo.color.ColorId, dbo.color.ColorName FROm dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      OrderLocalConsumption OC ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = OC.FinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.color ON dbo.ITEM_PARAMETER_MASTER.COLOR_ID = dbo.color.ColorId
                      WHERE OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ")", true, "--Select Color--");
                        }
                        else
                        {
                            if (DDitemdescription.SelectedIndex > 0)
                            {
                                UtilityModule.ConditionalComboFill(ref ddcolor, @"SELECT DISTINCT dbo.color.ColorId, dbo.color.ColorName FROm dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.color ON dbo.ITEM_PARAMETER_MASTER.COLOR_ID = dbo.color.ColorId
                      WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " and isnull(IndentDetail.orderdetailid,0)= " + DDitemdescription.SelectedValue + "", true, "--Select Color--");
                            }
                            else
                            {
                                UtilityModule.ConditionalComboFill(ref ddcolor, @"SELECT DISTINCT dbo.color.ColorId, dbo.color.ColorName FROm dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.color ON dbo.ITEM_PARAMETER_MASTER.COLOR_ID = dbo.color.ColorId
                      WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue , true, "--Select Color--");
                            
                            
                            
                            }
                        }
                        break;
                }
            }
        }
        if (ChKForOrder.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref dditemname, @"SELECT DISTINCT dbo.ITEM_MASTER.ITEM_ID, dbo.ITEM_MASTER.ITEM_NAME FROM  dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      OrderLocalConsumption OC ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = OC.FinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.ITEM_MASTER ON dbo.ITEM_PARAMETER_MASTER.ITEM_ID = dbo.ITEM_MASTER.ITEM_ID
                      WHERE OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ") AND dbo.ITEM_MASTER.CATEGORY_ID =" + ddCatagory.SelectedValue, true, "--Select Item--");
        }
        else
        {
            if (MySession.IndentAsProduction == "1")
            {
                UtilityModule.ConditionalComboFill(ref dditemname, @"select distinct vf.ITEM_ID,vf.ITEM_NAME from IndentDetail ID inner join PP_Consumption PP
                on ID.PPNo=PP.PPId and Id.Ofinishedid=PP.Finishedid   inner join V_FinishedItemDetail  vf on vf.ITEM_FINISHED_ID=pp.IFinishedid
                WHERE ID.IndentId =" + ddindentno.SelectedValue + " AND vf.CATEGORY_ID =" + ddCatagory.SelectedValue, true, "--Select Item--");
            }
            else
            {

                if (DDitemdescription.SelectedIndex > 0)
                {
                    UtilityModule.ConditionalComboFill(ref dditemname, @"SELECT DISTINCT dbo.ITEM_MASTER.ITEM_ID, dbo.ITEM_MASTER.ITEM_NAME FROM  dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.ITEM_MASTER ON dbo.ITEM_PARAMETER_MASTER.ITEM_ID = dbo.ITEM_MASTER.ITEM_ID
                      WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " AND dbo.ITEM_MASTER.CATEGORY_ID =" + ddCatagory.SelectedValue + " and isnull(IndentDetail.orderdetailid,0)= " + DDitemdescription.SelectedValue + " ", true, "--Select Item--");
                }
                else
                {
                    UtilityModule.ConditionalComboFill(ref dditemname, @"SELECT DISTINCT dbo.ITEM_MASTER.ITEM_ID, dbo.ITEM_MASTER.ITEM_NAME FROM  dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.ITEM_MASTER ON dbo.ITEM_PARAMETER_MASTER.ITEM_ID = dbo.ITEM_MASTER.ITEM_ID
                      WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " AND dbo.ITEM_MASTER.CATEGORY_ID =" + ddCatagory.SelectedValue , true, "--Select Item--");
                
                
                }
            }
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ITEM_CHANGED();
    }
    private void ITEM_CHANGED()
    {
        txtissqty.Text = "";
        txtpendingqty.Text = "";
        txtpreissue.Text = "";
        txtissue.Text = "";

        if (ChKForOrder.Checked == true)
        {
            CommanFunction.FillCombo(ddlunit, @"select distinct UnitId,UnitName from OrderLocalConsumption OC inner join Unit U on U.UnitId=OCD.IUnitId where OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ") Order By U.UnitId");
            UtilityModule.ConditionalComboFill(ref dquality, @"SELECT DISTINCT dbo.Quality.QualityId,dbo.Quality.QualityName
                     FROM  dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      OrderLocalConsumption OC ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = OC.FinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.Quality ON dbo.ITEM_PARAMETER_MASTER.QUALITY_ID = dbo.Quality.QualityId
                     WHERE OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ") and quality.item_id=" + dditemname.SelectedValue, true, "Select Quallity");
        }
        else
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "44")
            {
                CommanFunction.FillCombo(ddlunit, @"select Distinct UnitId,unitName From  Unit U,Item_Master IM where U.UnitTypeId=IM.UnitTypeId And ITEM_ID=" + dditemname.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
            }
            else
            {
                CommanFunction.FillCombo(ddlunit, @"Select distinct U.UnitId,UnitName from PP_Consumption PP inner join IndentDetail ID on ID.PPNo=PP.PPId inner Join IndentMaster IM ON IM.IndentId=Id.IndentId inner join ORDER_CONSUMPTION_DETAIL OCD on OCD.OrderDetailId=PP.OrderDetailId inner join Unit U on U.UnitId=OCD.IUnitId and OCD.Processid=" + ddProcessName.SelectedValue + " Where IM.IndentID=" + ddindentno.SelectedValue + " Order By U.UnitId");
            }
            
            dquality.Items.Clear();
            ddlshade.Items.Clear();
            if (MySession.IndentAsProduction == "1")
            {
                UtilityModule.ConditionalComboFill(ref dquality, @"select distinct vf.QualityId,vf.QualityName from IndentDetail ID inner join PP_Consumption PP
                on ID.PPNo=PP.PPId  inner join V_FinishedItemDetail  vf on vf.ITEM_FINISHED_ID=pp.IFinishedid
                WHERE ID.IndentId =" + ddindentno.SelectedValue + "and vf.item_id=" + dditemname.SelectedValue, true, "Select Quallity");
            }
            else
            {
                if (DDitemdescription.SelectedIndex > 0)
                {
                    UtilityModule.ConditionalComboFill(ref dquality, @"SELECT DISTINCT dbo.Quality.QualityId,dbo.Quality.QualityName
                     FROM  dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.Quality ON dbo.ITEM_PARAMETER_MASTER.QUALITY_ID = dbo.Quality.QualityId
                     WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + "and quality.item_id=" + dditemname.SelectedValue + " and isnull(IndentDetail.orderdetailid,0)= " + DDitemdescription.SelectedValue + "", true, "Select Quallity");
                }
                else
                {
                    UtilityModule.ConditionalComboFill(ref dquality, @"SELECT DISTINCT dbo.Quality.QualityId,dbo.Quality.QualityName
                     FROM  dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN
                      dbo.Quality ON dbo.ITEM_PARAMETER_MASTER.QUALITY_ID = dbo.Quality.QualityId
                     WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + "and quality.item_id=" + dditemname.SelectedValue , true, "Select Quallity");     
                
                
                }
            }
        }
    }
    private void CheckGSTType()
    {
        Label1.Visible = false;
        Label1.Text = "";
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select PRTid,GSTType from PP_ProcessRawTran where PRMid=" + ViewState["Prmid"] + "  order by PRTid");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            if (DDGSType.SelectedValue != Ds.Tables[0].Rows[0]["GSTType"].ToString())
            {
                Label1.Visible = true;
                Label1.Text = "Please select same GST Type";
                return;
            }
        }
        FillGSTIGST();
    }
    protected void save_detail()
    {
        CHECKVALIDCONTROL();
        if (lblMessage.Text == "")
        {
            Validated();
        }
        if (lblMessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                SqlParameter[] arr = new SqlParameter[30];
                arr[0] = new SqlParameter("@prmid", SqlDbType.Int);
                arr[1] = new SqlParameter("@companyid", SqlDbType.Int);
                arr[2] = new SqlParameter("@empid", SqlDbType.Int);
                arr[3] = new SqlParameter("@processid", SqlDbType.Int);
                arr[4] = new SqlParameter("@issuedate", SqlDbType.DateTime);
                arr[5] = new SqlParameter("@chalanno", SqlDbType.NVarChar, 150);
                arr[6] = new SqlParameter("@varuserid", SqlDbType.Int);
                arr[7] = new SqlParameter("@varcompanyid", SqlDbType.Int);
                arr[8] = new SqlParameter("@prtid", SqlDbType.Int);
                arr[9] = new SqlParameter("@finishedId", SqlDbType.Int);
                arr[10] = new SqlParameter("@godownId", SqlDbType.Int);
                arr[11] = new SqlParameter("@issueQuantity", SqlDbType.Float);
                arr[12] = new SqlParameter("@indentid", SqlDbType.Int);
                arr[13] = new SqlParameter("@unitid", SqlDbType.Int);
                arr[14] = new SqlParameter("@lotno", SqlDbType.NVarChar, 50);
                arr[15] = new SqlParameter("@Finish_Type", SqlDbType.Int);
                arr[16] = new SqlParameter("@Remark", SqlDbType.NVarChar, 250);
                arr[17] = new SqlParameter("@Sizeflag", SqlDbType.Int);
                arr[18] = new SqlParameter("@CanQty", SqlDbType.Float);
                arr[19] = new SqlParameter("@TagNo", SqlDbType.NVarChar, 50);
                arr[20] = new SqlParameter("@BinNo", SqlDbType.VarChar, 50);
                arr[21] = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                arr[22] = new SqlParameter("@ManualRate", SqlDbType.Float);
                arr[23] = new SqlParameter("@GSTType", SqlDbType.Int);
                arr[24] = new SqlParameter("@CGST", SqlDbType.Float);
                arr[25] = new SqlParameter("@SGST", SqlDbType.Float);
                arr[26] = new SqlParameter("@IGST", SqlDbType.Float);
                arr[27] = new SqlParameter("@Moisture", SqlDbType.Float);
                arr[28] = new SqlParameter("@BranchID", SqlDbType.Int);
                arr[29] = new SqlParameter("@orderdetailid", SqlDbType.Int);

                arr[0].Direction = ParameterDirection.InputOutput;
                arr[0].Value = ViewState["Prmid"];
                arr[1].Value = ddCompName.SelectedValue;
                arr[2].Value = ddempname.SelectedValue;
                arr[3].Value = ddProcessName.SelectedValue;
                arr[4].Value = txtdate.Text;
                arr[5].Direction = ParameterDirection.InputOutput;
                arr[5].Value = txtchalanno.Text;
                arr[6].Value = Session["varuserid"].ToString();
                arr[7].Value = Session["varMasterCompanyIDForERP"].ToString();
                int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, tran, ddlshade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                arr[8].Direction = ParameterDirection.InputOutput;
                arr[8].Value = ViewState["Prtid"];
                arr[9].Value = Varfinishedid;
                arr[11].Value = txtissqty.Text;
                arr[12].Value = ddindentno.SelectedValue;
                arr[13].Value = ddlunit.SelectedValue;
                arr[15].Value = 0;
                arr[16].Value = txtremarks.Text;
                arr[17].Value = DDsizetype.Visible == true ? DDsizetype.SelectedValue : "0";
                arr[18].Value = 0;
                if (ChKForOrder.Checked == true)
                {
                    arr[10].Value = ddgodown.SelectedValue;
                    arr[14].Value = "Without Lot No";
                }
                else
                {
                    arr[10].Value = ddgodown.SelectedValue;
                    arr[14].Value = ddlotno.SelectedItem.Text;
                }
                string TagNo;
                if (TDTagNo.Visible == true)
                {
                    TagNo = DDTagNo.SelectedItem.Text;
                }
                else
                {
                    TagNo = "Without Tag No";
                }
                arr[19].Value = TagNo;
                string BinNo = "";
                BinNo = TDBinNo.Visible == false ? "" : DDBinNo.SelectedItem.Text;
                arr[20].Value = BinNo;
                arr[21].Direction = ParameterDirection.Output;

                arr[22].Value = TDManualRate.Visible == true ? TxtManualRate.Text == "" ? "0" : TxtManualRate.Text : "0";
                arr[23].Value = TDGSTType.Visible == true ? DDGSType.SelectedValue : "0";
                arr[24].Value = TDGSTType.Visible == true ? lblCGST.Text : "0";
                arr[25].Value = TDGSTType.Visible == true ? lblSGST.Text : "0";
                arr[26].Value = TDGSTType.Visible == true ? lblIGST.Text : "0";
                arr[27].Value = TDMoisture.Visible == true ? TxtMoisture.Text : "0";
                arr[28].Value = DDBranchName.SelectedValue;
                arr[29].Value = DDitemdescription.SelectedValue;

                SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[PRO_PP_PRM_agni]", arr);
                txtchalanno.Text = arr[5].Value.ToString();
                ViewState["Prmid"] = arr[0].Value.ToString();
                ViewState["Prtid"] = arr[8].Value.ToString();
                lblMessage.Visible = true;
                lblMessage.Text = arr[21].Value.ToString();

                Session["ReportPath"] = "Reports/IndentRawIssue.rpt";
                Session["CommanFormula"] = "{V_IndentRawIssue.PRMid}=" + ViewState["Prmid"] + "";
                ViewState["Prtid"] = 0;
                tran.Commit();
                if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) != 7)
                {
                    btnpriview.Visible = true;
                }
                if (variable.VarMANYINDENTROWISSUE_SINGLECHALLAN == "1")
                {
                    ddCompName.Enabled = true;
                    ddProcessName.Enabled = true;
                    ddempname.Enabled = true;
                    txtdate.Enabled = true;
                    txtchalanno.Enabled = true;
                }
                else
                {
                    pnl1.Enabled = true;
                }
                btnsave.Text = "Save";
                Fill_Grid();
                if (ChKForOrder.Checked == true)
                {
                    Fill_GridForLocalConsump();
                }
                save_refresh();
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
                tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (txtissqty.Text != "")
        {
            if (variable.VarGSTForIndentRawIssue == "1")
            {
                CheckGSTType();
            }
            else
            {
                Label1.Visible = false;
                Label1.Text = "";
            }
        }
        if (Label1.Visible == false && Label1.Text == "")
        {
            if (Session["varcompanyno"].ToString() == "7")
            {
                if (lblqty.Visible == false)
                {
                    save_detail();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "Issue Qty Is greater then Stock Qty;", true);
                }
            }
            else
            {
                if (lblqty.Visible == false)
                {
                    save_detail();
                }
            }
        }
    }
    private void save_refresh()
    {
        txtissue.Text = "";
        TxtProdCode.Text = "";
        txtpendingqty.Text = "";
        txtpreissue.Text = "";
        txtissqty.Text = "";
        txtstock.Text = "";
        GodownSelectedChanged();
        txtremarks.Text = "";
        TxtMoisture.Text = "";
        //Btnorder.Visible = false;
        if (TDLotNo.Visible == true)
        {
            ddlotno.SelectedIndex = 0;
        }
        ddlotno.Focus();
    }
    protected void btnnew_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Session["varcompanyno"]) == 7)
        {
            ViewState["Prmid"] = 0;
            ViewState["Prtid"] = 0;
            pnl1.Enabled = true;
            ddempname.SelectedValue = null;
            ddindentno.SelectedValue = null;
            txtchalanno.Text = "";
            txtissue.Text = "";
            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtProdCode.Text = "";
            btnsave.Text = "Save";
            lbldate.Visible = false;
            Label1.Visible = false;
            Lblfinished.Visible = false;
            Btnorder.Visible = false;
        }
        else
        {
            refresh_form();
        }
    }
    private void Fill_Grid()
    {
        gvdetail.DataSource = fill_Data_grid();
        gvdetail.DataBind();
        Session["ReportPath"] = "Reports/IndentRawIssue.rpt";
        Session["CommanFormula"] = "{V_IndentRawIssue.PRMid}=" + ViewState["Prmid"] + "";
    }
    private DataSet fill_Data_grid()
    {
        DataSet ds = null;
        string strsql;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            if (ChKForOrder.Checked == false)
            {
                strsql = @"SELECT prt.PRTid,prt.LotNo, icm.CATEGORY_NAME, im.ITEM_NAME, gm.GodownName, 
                         prt.IssueQuantity,IPM1.QDCS + Space(2)+isnull(SizeMtr,'') DESCRIPTION,prt.Finishedid as Finishedid,prt.Tagno  FROM  
                         pp_ProcessRawTran prt  INNER JOIN ITEM_PARAMETER_MASTER ipm ON prt.Finishedid = ipm.ITEM_FINISHED_ID INNER JOIN
                         GodownMaster gm ON prt.Godownid = gm.GoDownID INNER JOIN
                         item_master im on im.item_id=ipm.item_id inner join
                         iTEM_CATEGORY_MASTER icm ON im.Category_id = icm.CATEGORY_ID inner join
                         pp_ProcessRawMaster prm on  prt.prmid=prm.prmid inner Join
                         ViewFindFinishedidItemidQDCSS IPM1 on IPM.Item_Finished_Id=IPM1.Finishedid
                         where prm.prmid=" + ViewState["Prmid"] + " And im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            else
            {
                strsql = @"SELECT prt.PRTid,prt.LotNo, icm.CATEGORY_NAME, im.ITEM_NAME, gm.GodownName, 
                           prt.IssueQuantity,IPM1.QDCS + Space(2)+isnull(SizeMtr,'') DESCRIPTION,prt.Finishedid as Finishedid,prt.Tagno  FROM  
                           pp_ProcessRawTran prt  INNER JOIN ITEM_PARAMETER_MASTER ipm ON prt.Finishedid = ipm.ITEM_FINISHED_ID Left outer JOIN
                           GodownMaster gm ON prt.Godownid = gm.GoDownID INNER JOIN
                           item_master im on im.item_id=ipm.item_id inner join
                           iTEM_CATEGORY_MASTER icm ON im.Category_id = icm.CATEGORY_ID inner join
                           pp_ProcessRawMaster prm on  prt.prmid=prm.prmid inner Join
                           ViewFindFinishedidItemidQDCSS IPM1 on IPM.Item_Finished_Id=IPM1.Finishedid
                           where prm.prmid=" + ViewState["Prmid"] + " And im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_process_ProcessRawIssue|fill_Data_grid|" + ex.Message);
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


    protected void gvdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvdetail, "Select$" + e.Row.RowIndex);
        }
        if (Session["VarcompanyNo"].ToString() == "7")
        {
            gvdetail.Columns[5].Visible = false; //5 For LotNo 
            gvdetail.Columns[3].Visible = false;// 3 for Godown
        }
    }
    protected void gvdetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvdetail.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    private int save_finishedid()
    {
        try
        {
            int VarOutfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            ItemFinishedId = VarOutfinishedid;
        }
        catch
        {

        }
        return ItemFinishedId;
    }
    protected void txtdate_TextChanged(object sender, EventArgs e)
    {
        if (txtdate.Text == "")
        {
            lbldate.Visible = true;
        }
        else
        {
            lbldate.Visible = false;
        }
    }
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        //if (CheckBox1.Checked == false)
        //{
        string strSize;
        //if (ChkFt.Checked == true)
        //{
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
        UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size." + strSize + @" FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                      dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
                       WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Size in Ft");
        //        }
        //        else
        //        {
        //            UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size.Sizemtr FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
        //                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
        //                      dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
        //                       WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Size in mtr");
        //        }
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds;
        string Str;
        if (TxtProdCode.Text != "")
        {
            ddCatagory.SelectedIndex = 0;
            Str = @" SELECT DISTINCT 
                      dbo.IndentDetail.IndentDetailId, dbo.IndentDetail.IndentId, dbo.ITEM_MASTER.CATEGORY_ID, dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID, 
                      dbo.ITEM_PARAMETER_MASTER.QUALITY_ID, dbo.ITEM_PARAMETER_MASTER.DESIGN_ID, dbo.ITEM_PARAMETER_MASTER.COLOR_ID, 
                      dbo.ITEM_PARAMETER_MASTER.SHAPE_ID, dbo.ITEM_PARAMETER_MASTER.SIZE_ID, dbo.ITEM_PARAMETER_MASTER.DESCRIPTION, 
                      dbo.ITEM_PARAMETER_MASTER.ITEM_ID, dbo.ITEM_PARAMETER_MASTER.ProductCode, dbo.ITEM_PARAMETER_MASTER.SHADECOLOR_ID  FROM  dbo.ITEM_MASTER INNER JOIN
                      dbo.ITEM_PARAMETER_MASTER ON dbo.ITEM_MASTER.ITEM_ID = dbo.ITEM_PARAMETER_MASTER.ITEM_ID INNER JOIN
                      dbo.IndentDetail INNER JOIN
                      dbo.IndentMaster ON dbo.IndentDetail.IndentId = dbo.IndentMaster.IndentID ON 
                      dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId where productcode='" + TxtProdCode.Text + "' And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                UtilityModule.ConditionalComboFill(ref ddCatagory, @"  SELECT DISTINCT dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID AS Expr1, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME, dbo.IndentDetail.IFinishedId 
                      FROM  dbo.ITEM_MASTER INNER JOIN
                      dbo.ITEM_PARAMETER_MASTER ON dbo.ITEM_MASTER.ITEM_ID = dbo.ITEM_PARAMETER_MASTER.ITEM_ID INNER JOIN
                      dbo.ITEM_CATEGORY_MASTER ON dbo.ITEM_MASTER.CATEGORY_ID = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID INNER JOIN
                      dbo.IndentDetail INNER JOIN
                      dbo.IndentMaster ON dbo.IndentDetail.IndentId = dbo.IndentMaster.IndentID ON 
                      dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId
                     where dbo.IndentMaster.partyid=" + ddempname.SelectedValue + " and IndentMaster.processid=" + ddProcessName.SelectedValue + " and dbo.IndentDetail.IndentId=" + ddindentno.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Category Name");
                ddCatagory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();

                string Qry = "Select Distinct Item_Id,Item_Name from Item_Master where Category_Id=" + ddCatagory.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                Qry = Qry + " select qualityid,qualityname from quality where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And item_id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["ITEM_ID"].ToString());
                Qry = Qry + " select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order  by DesignName ";
                Qry = Qry + " SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by colorid ";
                Qry = Qry + " select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Shapeid ";
                Qry = Qry + " SELECT SIZEID,SIZEFT fROM SIZE WhERE MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And SHAPEID=" + Convert.ToInt32(ds.Tables[0].Rows[0]["SHAPE_ID"].ToString()) + "";
                Qry = Qry + " select shadecolorid,shadecolorname from shadecolor Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by shadecolorid ";
                DataSet DSQ = SqlHelper.ExecuteDataset(Qry);

                UtilityModule.ConditionalComboFillWithDS(ref dditemname, DSQ, 0, true, "--Select Item--");
                dditemname.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref dquality, ds, 1, true, "Select Quallity");
                dquality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref dddesign, DSQ, 2, true, "--Select Design--");
                dddesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddcolor, DSQ, 3, true, "--Select Color--");
                ddcolor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddshape, DSQ, 4, true, "--Select Shape--");
                ddshape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddsize, DSQ, 5, true, "--SELECT SIZE--");
                ddsize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddlshade, DSQ, 6, true, "Select ShadeColor");
                ddlshade.SelectedValue = ds.Tables[0].Rows[0]["SHADECOLOR_ID"].ToString();

                Session["finishedid"] = ds.Tables[0].Rows[0]["Item_Finished_id"].ToString();
                Session["indentid"] = ds.Tables[0].Rows[0]["IndentId"].ToString();
                Session["IndentdetailId"] = ds.Tables[0].Rows[0]["IndentDetailId"].ToString();

                int q = (dquality.SelectedIndex > 0 ? Convert.ToInt32(dquality.SelectedValue) : 0);
                if (q > 0)
                {
                    ql.Visible = true;
                }
                else
                {
                    ql.Visible = false;

                }
                if (Convert.ToInt32(dddesign.SelectedValue) > 0)
                {
                    dsn.Visible = true;
                }
                else
                {
                    dsn.Visible = false;
                }
                int c = (ddcolor.SelectedIndex > 0 ? Convert.ToInt32(ddcolor.SelectedValue) : 0);
                if (c > 0)
                {
                    clr.Visible = true;

                }
                else
                {
                    clr.Visible = false;
                }
                int s = (ddshape.SelectedIndex > 0 ? Convert.ToInt32(ddshape.SelectedValue) : 0);
                if (s > 0)
                {
                    shp.Visible = true;
                }
                else
                {
                    shp.Visible = false;
                }
                int si = (ddsize.SelectedIndex > 0 ? Convert.ToInt32(ddsize.SelectedValue) : 0);
                if (si > 0)
                {
                    sz.Visible = true;
                }
                else
                {
                    sz.Visible = false;
                }
                int sd = (ddlshade.SelectedIndex > 0 ? Convert.ToInt32(ddlshade.SelectedValue) : 0);
                if (sd > 0)
                {
                    shd.Visible = true;
                }
                else
                {
                    shd.Visible = false;
                }
                CommanFunction.FillCombo(ddlunit, @"select distinct UnitId,UnitName from PP_Consumption PP inner join IndentDetail ID on ID.PPNo=PP.PPId inner Join IndentMaster IM on IM.IndentId=Id.IndentId inner join ORDER_CONSUMPTION_DETAIL OCD on OCD.OrderDetailId=PP.OrderDetailId inner join Unit U on U.UnitId=OCD.IUnitId where IndentNo=" + ddindentno.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By U.UnitId");
                Label2.Visible = false;
            }
            else
            {
                Label2.Visible = true;
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
            }
        }
        else
        {
            ddCatagory.SelectedIndex = 0;
        }
    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        FILL_QUALITY_CHANGE();
    }
    private string CheckStockQty()
    {
        string str = "";
        try
        {
            SqlParameter[] parparam = new SqlParameter[7];
            parparam[0] = new SqlParameter("@IndentID", ddindentno.SelectedValue);
            parparam[1] = new SqlParameter("@FinishedID", ViewState["FinishedID"]);
            parparam[2] = new SqlParameter("@TxtQty", txtissqty.Text);
            parparam[3] = new SqlParameter("@PrtId", ViewState["Prtid"]);
            parparam[4] = new SqlParameter("@Message", SqlDbType.NVarChar, 2);
            parparam[5] = new SqlParameter("@LotNo", ddlotno.SelectedItem.Text);
            parparam[6] = new SqlParameter("@CompanyId", ddCompName.SelectedValue);
            parparam[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_CheckStockQty", parparam);
            str = parparam[4].Value.ToString();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Masters/RawMaterial/IndentRawIssue");
        }
        return str;
    }
    private void FILL_QUALITY_CHANGE()
    {
        if (ChKForOrder.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddlshade, @"SELECT DISTINCT dbo.ShadeColor.ShadecolorId, dbo.ShadeColor.ShadeColorName FROM   dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      OrderLocalConsumption OC ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = OC.FinishedId INNER JOIN
                      dbo.ShadeColor ON dbo.ITEM_PARAMETER_MASTER.SHADECOLOR_ID = dbo.ShadeColor.ShadecolorId
                      WHERE OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ") AND dbo.ITEM_PARAMETER_MASTER.ITEM_ID=" + dditemname.SelectedValue + " AND dbo.ITEM_PARAMETER_MASTER.Quality_Id=" + dquality.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddlshade, @"SELECT DISTINCT dbo.ShadeColor.ShadecolorId, dbo.ShadeColor.ShadeColorName FROM   dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                      dbo.ShadeColor ON dbo.ITEM_PARAMETER_MASTER.SHADECOLOR_ID = dbo.ShadeColor.ShadecolorId
                      WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " AND dbo.ITEM_PARAMETER_MASTER.ITEM_ID=" + dditemname.SelectedValue + " AND dbo.ITEM_PARAMETER_MASTER.Quality_Id=" + dquality.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select--");
        }
    }
    protected void ddgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        GodownSelectedChanged();
        switch (Session["varcompanyNo"].ToString())
        {
            case "10"://for raas india
                ddlotno_SelectedIndexChanged(sender, e);
                break;
        }

    }
    private void GodownSelectedChanged()
    {

        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        if (variable.Carpetcompany == "1")
        {
            string str = "";
            if (variable.VarGENRATEINDENTWITHOUTLOT_STOCK == "1")
            {
                str = "select Distinct Lotno,Lotno From stock WHere Companyid=" + ddCompName.SelectedValue + " and Godownid=" + ddgodown.SelectedValue + " and ITEM_FINISHED_ID=" + Varfinishedid;
                if (MySession.Stockapply == "True")
                {
                    str = str + " and Round(Qtyinhand,3)>0";
                }
                //str = "SELECT Distinct LotNo,LotNo FROM IndentDetail Where IndentId=" + ddindentno.SelectedValue + " and ifinishedid=" + Varfinishedid;
            }
            else
            {

                str = "SELECT Distinct LotNo,LotNo FROM IndentDetail Where IndentId=" + ddindentno.SelectedValue + " and ifinishedid=" + Varfinishedid;
            }
            UtilityModule.ConditionalComboFill(ref ddlotno, str, true, "Select LotNo");

        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddlotno, "SELECT Distinct LotNo,LotNo FROM IndentDetail Where IndentId=" + ddindentno.SelectedValue + "", true, "Select LotNo");
        }
        SqlParameter[] _array = new SqlParameter[8];
        _array[0] = new SqlParameter("@Finishedid", Varfinishedid);
        _array[1] = new SqlParameter("@indentId", ddindentno.SelectedValue);
        _array[2] = new SqlParameter("@StockQty", SqlDbType.Float);
        _array[3] = new SqlParameter("@IndentQty", SqlDbType.Float);
        _array[4] = new SqlParameter("@PreIssueQty", SqlDbType.Float);
        _array[5] = new SqlParameter("@PendingQty", SqlDbType.Float);
        _array[6] = new SqlParameter("@varcompanyType", SqlDbType.Int);
        _array[7] = new SqlParameter("@orderdetailid", DDitemdescription.SelectedValue);
        _array[2].Direction = ParameterDirection.Output;
        _array[3].Direction = ParameterDirection.Output;
        _array[4].Direction = ParameterDirection.Output;
        _array[5].Direction = ParameterDirection.Output;
        _array[6].Direction = ParameterDirection.Output;

        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_FillIndent_Issue_Qty_agni", _array);
        if (_array[6].Value.ToString() == "7")
        {
            txtstock.Text = "0";
            if (Convert.ToDouble(_array[2].Value) > 0)
            {
                txtstock.Text = _array[2].Value.ToString();//For Stock
            }
        }
        else
        {
            txtstock.Text = "0";
            txtissue.Text = _array[3].Value.ToString();//IndentQty
            txtpreissue.Text = _array[4].Value.ToString();//PreIssue
            txtpendingqty.Text = _array[5].Value.ToString();//PendingQty
        }
        getstockQty(Varfinishedid);
        //}
    }
    private void refresh_form()
    {
        ViewState["Prtid"] = 0;
        pnl1.Enabled = true;
        ddempname.SelectedValue = null;
        ddProcessName.SelectedValue = null;
        ddindentno.SelectedValue = null;
        ddempname.SelectedValue = null;
        txtchalanno.Text = "";
        ddCatagory.SelectedValue = null;
        dditemname.SelectedValue = null;
        ddgodown.SelectedValue = null;
        txtissue.Text = "";
        dquality.SelectedValue = null;
        dddesign.SelectedValue = null;
        ddcolor.SelectedValue = null;
        ddshape.SelectedValue = null;
        ddsize.SelectedValue = null;
        ddlshade.SelectedValue = null;
        txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        TxtProdCode.Text = "";
        btnsave.Text = "Save";
        lbldate.Visible = false;
        Label1.Visible = false;
        Lblfinished.Visible = false;
        ddCompName.Enabled = true;
        ddProcessName.Enabled = true;
        ddempname.Enabled = true;
        txtdate.Enabled = true;
        txtchalanno.Enabled = true;
    }
    private void Validated()
    {
        try
        {
            string Lotno, BinNo;
            if (TDLotNo.Visible == true)
            {
                Lotno = ddlotno.SelectedItem.Text;
            }
            else
            {
                Lotno = "Without Lot No";
            }
            if (TDBinNo.Visible == true)
            {
                BinNo = DDBinNo.SelectedItem.Text;
            }
            else
            {
                BinNo = "";
            }
            //chetck stock flag           

            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            string strsql;
            DataSet ds;
            strsql = "Select finishedid from pp_processrawtran where finishedid=" + Varfinishedid + " and prmid =" + ViewState["Prmid"] + @" And 
            indentid=" + ddindentno.SelectedValue + " and orderdetailid=" + DDitemdescription.SelectedValue + " and Lotno='" + Lotno + "' and BinNo='" + BinNo + "'  ";
            if (TDTagNo.Visible == true)
            {
                strsql = strsql + " And TagNo = '" + DDTagNo.SelectedItem.Text + "'";
            }
            if (ddlotno.Visible == true)
            {
                strsql = strsql + " And LotNo='" + ddlotno.SelectedItem.Text + "'";
            }
            if (btnsave.Text == "Update")
            {
                strsql = strsql + " And Prtid!='" + ViewState["Prtid"] + "'";
            }
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);

            if (Session["varCompanyNo"].ToString() != "42")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Duplicate Entry.......";
                    return;
                }
            }
           
            if (Convert.ToInt32(Session["VarcompanyNo"]) == 5)
            {
                string ChkMsg = CheckStockQty();
                if (ChkMsg == "G")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Qty should not be greater than assigned stock";
                    return;
                }
            }
            if (txtchalanno.Text != "")
            {
                strsql = "select ChallanNo  from PP_ProcessRawmaster Where ChallanNo='" + txtchalanno.Text + "' And PrmId<>" + ViewState["Prmid"] + "";
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "ChallanNo Already Exists......";
                    return;
                }
            }
            Double qty;
            if (Convert.ToInt32(Session["varcompanyno"]) == 7)
            {

                qty = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select isnull((sum(pQty)-sum(Aqty)),0) from OrderAssignQty where FinishedId=" + Varfinishedid + " and orderid in(select orderid from indentdetail where indentid=" + ddindentno.SelectedValue + ")"));
            }
            else
            {
                qty = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT  Isnull(SUM(qtyinhand),0) as qtyinhand FROM stock where item_finished_id=" + Varfinishedid + " and godownid=" + ddgodown.SelectedValue + " And LotNo='" + Lotno + "' And CompanyId=" + ddCompName.SelectedValue + ""));
            }
            if (MySession.Stockapply == "True")
            {
                if (Convert.ToDouble(qty) < Convert.ToDouble(txtissqty.Text))
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Issue Qty Can not be greater than Stock Qty...";
                    txtstock.Text = qty.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.Visible = true;
        }
    }
    protected void txtissqty_TextChanged(object sender, EventArgs e)
    {
        int VarCompanyNo = Convert.ToInt32(Session["varcompanyno"].ToString());
        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        double stock = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(sum(qtyinhand),0) from stock where item_finished_id=" + Varfinishedid + "").ToString());
        if (VarCompanyNo == 7 && txtissqty.Text != "" || VarCompanyNo == 3 || VarCompanyNo == 10 || ChKForOrder.Checked == true)
        {
            //int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "");
            for (int i = 0; i < DGShowConsumption.Rows.Count; i++)
            {
                if (DGShowConsumption.Rows[i].Cells[3].Text == "Issue" && Varfinishedid.ToString() == ((Label)DGShowConsumption.Rows[i].FindControl("Finishedid")).Text)
                {
                    if (DGShowConsumption.Rows[i].Cells[7].Text == "")
                    {
                        DGShowConsumption.Rows[i].Cells[7].Text = "0";
                    }
                    if (Convert.ToDouble(txtissqty.Text) > Convert.ToDouble(txtstock.Text) || Convert.ToDouble(txtissqty.Text) <= 0 || Convert.ToDouble(txtissqty.Text) > Convert.ToDouble(DGShowConsumption.Rows[i].Cells[7].Text))
                    {
                        lblqty.Visible = true;
                    }
                    else
                    {
                        lblqty.Visible = false;
                    }
                }
                stock = Convert.ToDouble(txtstock.Text);
            }
            double reorder = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(sum(MinStockQty),0) from Reorderqty where item_finished_id=" + Varfinishedid + "").ToString());
            if ((stock - Convert.ToDouble(txtissqty.Text)) < reorder)
            {
                lblqty.Text = "Stock Qty is less then Ideal Quantity";
                lblqty.Visible = true;
            }
            else
            {
                lblqty.Text = "Please Check Qty.........";
                lblqty.Visible = false;
            }
        }
        else
        {
            if (VarCompanyNo != 4)
            {
                double VarPercentQty = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select PercentageExecssQtyForIndent from MasterSetting"));
                double PendingQty = Convert.ToDouble(txtpendingqty.Text) + (Convert.ToDouble(txtpendingqty.Text) * VarPercentQty / 100);
                if (Convert.ToDouble(txtissqty.Text == "" ? "0" : txtissqty.Text) > PendingQty || Convert.ToDouble(txtissqty.Text == "" ? "0" : txtissqty.Text) <= 0)
                {
                    lblqty.Visible = true;
                    lblqty.Text = lblqty.Text + " i.e " + PendingQty.ToString();
                }
                else
                {
                    lblqty.Visible = false;
                }
            }
        }
        if (Convert.ToInt32(Session["VarcompanyNo"]) == 5)
        {
            lblMessage.Text = "";
            string ChkMsg = CheckStockQty();
            if (ChkMsg == "G")
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Qty should not be greater than assigned stock";
                return;
            }
        }
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Session.Remove("finishedid");
        Session.Remove("indentid");
        Session.Remove("indentdetailid");
    }
    protected void ddlotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        ViewState["FinishedID"] = Varfinishedid;
        if (TDBinNo.Visible == true)
        {
            DDBinNo.SelectedIndex = -1;
            string str = "select Distinct BinNo,BinNo as BinNo1 from Stock Where Item_Finished_id=" + Varfinishedid + " and Godownid=" + ddgodown.SelectedValue + " and CompanyId=" + ddCompName.SelectedValue + "  and Round(Qtyinhand,3)>0 and Lotno='" + ddlotno.SelectedItem.Text + "' order by BinNo";
            UtilityModule.ConditionalComboFill(ref DDBinNo, str, true, "--Plz Select--");

            if (DDBinNo.Items.Count > 0)
            {
                DDBinNo.SelectedIndex = 1;

            }
            DDBinNo_SelectedIndexChanged(sender, new EventArgs());
        }
        else
        {

            FillTagNo(Varfinishedid, sender);
        }
    }
    protected void FillTagNo(int Varfinishedid, object sender = null)
    {

        string str = "";
        if (TDTagNo.Visible == true)
        {
            DDTagNo.SelectedIndex = -1;
            str = "select Distinct TagNo,TagNo From Stock  Where Companyid=" + ddCompName.SelectedValue + " and Godownid=" + ddgodown.SelectedValue + " and LotNo='" + ddlotno.SelectedItem.Text + "' and ITEM_FINISHED_ID=" + Varfinishedid;
            if (MySession.Stockapply == "True")
            {
                str = str + " and Round(Qtyinhand,3)>0";
            }
            if (TDBinNo.Visible == true)
            {
                str = str + " and BinNo='" + (DDBinNo.SelectedIndex > 0 ? DDBinNo.SelectedItem.Text : "") + "'";
            }
            UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "Select Tagno");
            if (DDTagNo.Items.Count > 0)
            {
                if (sender != null)
                {
                    DDTagNo.SelectedIndex = 1;
                    DDTagNo_SelectedIndexChanged(sender, new EventArgs());
                }
            }
        }
        else
        {
            getstockQty(Varfinishedid);
        }
        // double qty = 0;

    }
    protected void getstockQty(int Varfinishedid)
    {
        double qty = 0;
        string Lotno;
        string TagNo;
        string BinNo = "";
        if (TDLotNo.Visible == true && ddlotno.SelectedItem != null)
        {
            Lotno = ddlotno.SelectedItem.Text;
        }
        else
        {
            Lotno = "Without Lot No";
        }
        if (TDTagNo.Visible == true)
        {
            if (DDTagNo.Items.Count > 0)
            {
                TagNo = DDTagNo.SelectedItem.Text;
            }
            else
            {
                TagNo = "Without Tag No";
            }
        }
        else
        {
            TagNo = "Without Tag No";
        }
        if (TDBinNo.Visible == true)
        {
            if (DDBinNo.Items.Count > 0)
            {
                BinNo = DDBinNo.SelectedItem.Text;
            }
            else
            {
                BinNo = "";
            }
        }
        ///*************************************************************
        if (Convert.ToInt32(Session["varcompanyno"]) == 7)
        {
            //qty = Convert.ToDouble(SqlHelper.ExecuteScalar(con, CommandType.Text, @"select sum(qty) from PurchaseReceiveDetail pr,PurchaseIndentIssue pm where pm.PindentIssueid=pr.PindentIssueid and finishedid=" + Varfinishedid + " and orderid in(select orderid from indentdetail where indentid="+ddindentno.SelectedValue+")"));
            qty = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select isnull((sum(pQty)-sum(Aqty)),0) from OrderAssignQty where FinishedId=" + Varfinishedid + " and orderid in(select orderid from indentdetail where indentid=" + ddindentno.SelectedValue + ")"));
        }
        else if (ChKForOrder.Checked == true)
        {
            //qty = Convert.ToDouble(SqlHelper.ExecuteScalar(con, CommandType.Text, @"select isnull((sum(pQty)- sum(Aqty)),0) from OrderAssignQty where  FinishedId=" + Varfinishedid + " and orderid in (select orderid from indentdetail where indentid=" + ddindentno.SelectedValue + ")"));
            qty = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT  Round(Isnull(SUM(qtyinhand),0),3) as qtyinhand FROM stock where item_finished_id=" + Varfinishedid + " and godownid=" + ddgodown.SelectedValue + " And LotNo='" + Lotno + "' And CompanyId=" + ddCompName.SelectedValue + " and BinNo='" + BinNo + "'"));
        }
        else
        {
            //qty = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT  Round(Isnull(SUM(qtyinhand),0),3) as qtyinhand FROM stock where item_finished_id=" + Varfinishedid + " and godownid=" + ddgodown.SelectedValue + " And LotNo='" + Lotno + "' and Tagno='" + TagNo + "' And CompanyId=" + ddCompName.SelectedValue + ""));
            qty = UtilityModule.getstockQty(ddCompName.SelectedValue, ddgodown.SelectedValue, Lotno, Varfinishedid, TagNo, BinNo: BinNo);
        }
        if (qty > 0)
        {
            txtstock.Text = qty.ToString();
        }
        else
        {
            txtstock.Text = "0";
        }
    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (TDBinNo.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDBinNo) == false)
            {
                goto a;
            }
        }
        if (TDLotNo.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddlotno) == false)
            {
                goto a;
            }
        }
        if (TDTagNo.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDTagNo) == false)
            {
                goto a;
            }
        }
        if (TDGodown.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddgodown) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(dditemname) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }

    a:
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
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
    protected void ChKForOrder_CheckedChanged(object sender, EventArgs e)
    {
        cchkforoorder_checked();
    }
    private void cchkforoorder_checked()
    {
        if (ChKForOrder.Checked == true)
        {
            TDGodown.Visible = true;
            TDIndentQty.Visible = false;
            TDPreIssue.Visible = true;
            TDPending.Visible = true;
            if (Session["varcompanyNo"].ToString() == "7")
            {
                TDLotNo.Visible = false;
            }
            TDLotNo.Visible = true;
            TDStock.Visible = true;
        }
        else
        {
            TDGodown.Visible = true;
            TDLotNo.Visible = true;
        }
    }
    private void Fill_GridForLocalConsump()
    {
        DGShowConsumption.DataSource = GetDetail();
        DGShowConsumption.DataBind();
        for (int i = 0; i < DGShowConsumption.Rows.Count; i++)
        {
            if (DGShowConsumption.Rows[i].Cells[3].Text == "Issue")
            {
                TextBox txtGridissueQty = ((TextBox)DGShowConsumption.Rows[i].FindControl("txtIssueQty"));
                string qty = DGShowConsumption.Rows[i].Cells[4].Text;
                string Issqty = txtGridissueQty.Text == "" ? "0" : txtGridissueQty.Text;
                Double bal = Convert.ToDouble(qty) - Convert.ToDouble(Issqty);

                //DGShowConsumption.Rows[i].Cells[7].Text = (Convert.ToInt32(DGShowConsumption.Rows[i].Cells[4].Text) -Convert.ToInt32((txtGridissueQty.Text==""?"0":txtGridissueQty.Text))).ToString();
                DGShowConsumption.Rows[i].Cells[7].Text = bal.ToString();
            }
            else
            {
                ((TextBox)DGShowConsumption.Rows[i].FindControl("txtIssueQty")).Text = "0";
            }
        }
    }
    private DataSet GetDetail()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string strsql = @"SELECT Category_Name ,VF.ITEM_NAME ,QualityName+'  '+DesignName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName+'  '+ CASE WHEN OD.SizeUnit=1 Then SizeMtr Else SizeFt End  Description,isnull(sum(Qty),0) as qty,'Issue' As Status,OD.Finishedid,OD.OrderId,'0' BalQty FROM OrderLocalConsumption OD Inner JOIN V_FinishedItemDetail VF ON OD.FinishedId=VF.Item_Finished_Id 
                             INNER JOIN ITEM_PARAMETER_MASTER IPM ON OD.FinishedId=IPM.Item_Finished_Id And IPM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"  Where orderdetailid in(Select Distinct orderdetailid from  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + @" ) 
                             group by Category_Name ,VF.ITEM_NAME ,QualityName,DesignName,ColorName,ShadeColorName,ShapeName,OD.SizeUnit,SizeMtr,SizeFt,OD.Finishedid,OD.OrderId 
                            Union
                            SELECT Category_Name ,VF.ITEM_NAME ,QualityName+'  '+DesignName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName+'  '+ CASE WHEN PT.UnitID=1 Then SizeMtr Else SizeFt End  Description,isnull(sum(RecQuantity),0) As Qty,'Receive' As Status,PT.Finishedid,PM.OrderId As OrderId,'0' BalQty FROM PP_ProcessRecMaster PM Inner Join PP_ProcessRecTran PT on PM.PrmId=PT.Prmid Inner JOIN V_FinishedItemDetail VF ON PT.FinishedId=VF.Item_Finished_Id 
                            INNER JOIN ITEM_PARAMETER_MASTER IPM ON PT.FinishedId=IPM.Item_Finished_Id Where  IndentId=" + ddindentno.SelectedValue + " And IPM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                            group by Category_Name ,VF.ITEM_NAME ,QualityName,DesignName,ColorName,ShadeColorName,ShapeName,pt.Unitid,SizeMtr,SizeFt,Finishedid,OrderId";
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        return ds;
    }
    protected void checkBoxEvent()
    {
        if (ChKForOrder.Checked == true)
        {
            TDGodown.Visible = true;
            TDIndentQty.Visible = false;
            TDPreIssue.Visible = true;
            TDPending.Visible = false;
            TDLotNo.Visible = false;
            if (Session["varcompanyNo"].ToString() == "7")
            {
                TDLotNo.Visible = false;
            }

            TDStock.Visible = true;
            ChKForOrder.Enabled = false;
        }
        else
        {
            ChKForOrder.Enabled = true;
            TDIndentQty.Visible = true;
            TDPreIssue.Visible = true;
            TDPending.Visible = true;
            TDLotNo.Visible = true;
        }
    }
    public string getgiven(string strVal, string strval1)
    {
        string val = "0";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Isnull(Sum(IssueQuantity),0) As IssueQuantity From PP_ProcessRawMaster PM, PP_ProcessRawTran PT where PM.PrmId=PT.Prmid And PT.Finishedid=" + strVal + " And PT.IndentId=" + ddindentno.SelectedValue + "");
        val = ds.Tables[0].Rows[0]["IssueQuantity"].ToString();
        hnissueqty.Value = val;
        return val;
    }
    protected void btnpriview_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 5)
        {
            Session["ReportPath"] = "Reports/IndentRawIssueDuplicate1.rpt";
            Session["CommanFormula"] = "{V_IndentRawIssue.PRMid}=" + ViewState["Prmid"] + "";
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
        }
    }
    private void Report()
    {
        string qry = @"SELECT EmpName,Address,PhoneNo,Mobile,CompanyName,CompanyAddress,ComPanyPhoneNo,CompanyFaxNo,TinNo,Category_Name,Item_Name,QualityName,Designname,
                      colorname,shapename,shadecolorname,SizeMtr,ChallanNo,Indentid,PRMid,IssueQuantity,LotNo,GodownNAme,ShortName,ShadeColorName,unitname,
                      companyid,IndentNo,Date,flagsize,CanQty,MastercompanyId,Process_Name,Buyercode,localorder,customerorderno,(select top(1) Remark from PP_ProcessRawTran where PRMid=" + ViewState["Prmid"] + @" and remark<>'') as Remark,Tagno,GSTIN,EmpGStno,BinNo,UserName
                     FROM V_IndentRawIssue where V_IndentRawIssue.PRMid=" + ViewState["Prmid"] + " And CompanyId=" + ddCompName.SelectedValue + "  ORDER BY V_IndentRawIssue.Item_Name ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        string str = @"SELECT IssQty,CATEGORY_NAME,ITEM_NAME,Quality_Id,QualityName,PRMid,unitname FROM IndentRawIssue Where PRMid=" + ViewState["Prmid"] + " ORDER BY Quality_Id";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        SqlDataAdapter sda = new SqlDataAdapter(str, con);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        ds.Tables.Add(dt);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "16")
            {
                Session["rptFileName"] = "~\\Reports\\IndentRawIssueNEWChampo.rpt";
            }
            else if (Session["varMasterCompanyIDForERP"].ToString() == "30")
            {
                Session["rptFileName"] = "~\\Reports\\IndentRawIssueNEWSamara.rpt";
            }
            else if (Session["varMasterCompanyIDForERP"].ToString() == "42")
            {
                Session["rptFileName"] = "~\\Reports\\IndentRawIssueNEWVikramMirzapur.rpt";
            }
            else if (Session["varMasterCompanyIDForERP"].ToString() == "44")
            {
                Session["rptFileName"] = "~\\Reports\\IndentRawIssueagni.rpt";
            }
            else
            {
                if (MySession.TagNowise == "1")
                {
                    Session["rptFileName"] = "~\\Reports\\IndentRawIssueTagwiseNEW.rpt";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\IndentRawIssueNEW.rpt";
                }
            }
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\IndentRawIssueNEW.xsd";
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

        _array[0].Value = ViewState["Prmid"];
        _array[1].Value = Session["varMasterCompanyIDForERP"];
        _array[2].Value = Session["varuserId"];

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetIndentRawIssueWithGSTReport", _array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptIndentRawIssueWithGST.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptIndentRawIssueWithGST.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    //protected void DGShowConsumption_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGShowConsumption_SelectedIndexChanged(object sender, EventArgs e)
    {
        int r = Convert.ToInt32(DGShowConsumption.SelectedIndex.ToString());
        string FINISHEDID = ((Label)DGShowConsumption.Rows[r].FindControl("Finishedid")).Text;
        hnorderid.Value = ((Label)DGShowConsumption.Rows[r].FindControl("OrderId")).Text;
        hnqty.Value = ((TextBox)DGShowConsumption.Rows[r].FindControl("txtIssueQty")).Text;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select CATEGORY_ID,ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId from V_FinishedItemDetail where item_finished_id=" + FINISHEDID + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ddCatagory.Visible == true)
            {
                ddCatagory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                ddlcategorycange();
            }
            if (dditemname.Visible == true)
            {
                dditemname.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                ITEM_CHANGED();
            }
            if (dquality.Visible == true)
            {
                dquality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
                FILL_QUALITY_CHANGE();
            }
            if (dddesign.Visible == true)
            {
                dddesign.SelectedValue = ds.Tables[0].Rows[0]["designId"].ToString();
            }
            if (ddcolor.Visible == true)
            {
                ddcolor.SelectedValue = ds.Tables[0].Rows[0]["ColorId"].ToString();
            }
            if (ddshape.Visible == true)
            {
                ddshape.SelectedValue = ds.Tables[0].Rows[0]["ShapeId"].ToString();
            }
            if (ddsize.Visible == true)
            {
                ddsize.SelectedValue = ds.Tables[0].Rows[0]["SizeId"].ToString();
            }
            if (ddlshade.Visible == true)
            {
                ddlshade.SelectedValue = ds.Tables[0].Rows[0]["ShadecolorId"].ToString();
            }
            if (ddgodown.Items.Count > 0)
            {
                ddgodown.SelectedIndex = 1;
                GodownSelectedChanged();
            }
        }
    }

    protected void DGShowConsumption_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGShowConsumption, "Select$" + e.Row.RowIndex);
        }
    }
    private void fillordergrid()
    {
        DataSet ds = new DataSet();
        if (DDitemdescription.SelectedIndex > 0)
        {
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select CATEGORY_NAME +' '+ ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName AS description,Quantity as qty from IndentMaster IM INNER JOIN INDENTDETAIL ID ON ID.INDENTID=IM.INDENTID INNER JOIN V_FinishedItemDetail VD ON VD.ITEM_FINISHED_ID=ID.OFINISHEDID where id.indentid=" + ddindentno.SelectedValue + " And VD.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "and ID.orderdetailid="+DDitemdescription.SelectedValue);

        }
        else
        {
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select CATEGORY_NAME +' '+ ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName AS description,Quantity as qty from IndentMaster IM INNER JOIN INDENTDETAIL ID ON ID.INDENTID=IM.INDENTID INNER JOIN V_FinishedItemDetail VD ON VD.ITEM_FINISHED_ID=ID.OFINISHEDID where id.indentid=" + ddindentno.SelectedValue + " And VD.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
        }

        dgorder.DataSource = ds;
        dgorder.DataBind();
    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[5];
            arr[0] = new SqlParameter("@Prtid", SqlDbType.Int);
            arr[1] = new SqlParameter("@TableName", SqlDbType.NVarChar, 50);
            arr[2] = new SqlParameter("@Count", SqlDbType.Int);
            arr[3] = new SqlParameter("@FlagDeleteOrNot", SqlDbType.Int);
            arr[4] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            arr[0].Value = gvdetail.DataKeys[e.RowIndex].Value;
            arr[1].Value = "PP_ProcessRawTran";
            arr[2].Value = gvdetail.Rows.Count;
            arr[3].Direction = ParameterDirection.Output;
            arr[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_UpdateStockIssueDelete", arr);
            tran.Commit();
            //save_refresh();
            if (arr[4].Value.ToString() != "")
            {
                Label1.Text = arr[4].Value.ToString();
                Label1.Visible = true;
            }
            else
            {
                ddgodown.SelectedIndex = 0;
                if (gvdetail.Rows.Count == 1)
                {
                    ViewState["Prmid"] = 0;
                    ViewState["Prtid"] = 0;
                    txtchalanno.Text = "";
                }

                if (Convert.ToInt32(arr[3].Value) == 1)
                {
                    Label1.Text = "AlReady Received Data....";
                    Label1.Visible = true;
                }
                else
                {
                    Fill_Grid();
                    Fill_GridForLocalConsump();
                }
            }
        }
        catch (Exception ex)
        {
            Label1.Text = ex.Message;
            Label1.Visible = true;
            tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void btnstatus_Click(object sender, EventArgs e)
    {
        if (ddindentno.SelectedIndex > 0)
        {
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "update indentmaster set status='complete' where indentid=" + ddindentno.SelectedValue + "");
            fill_emp_change();
        }
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
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
        if (MySession.IndentAsProduction == "1")
        {
            UtilityModule.ConditionalComboFill(ref ddsize, @"select distinct vf.SizeId,vf." + strSize + @" from IndentDetail ID inner join PP_Consumption PP
                                on ID.PPNo=PP.PPId  inner join V_FinishedItemDetail  vf on vf.ITEM_FINISHED_ID=pp.IFinishedid
                                WHERE  ID.IndentId =" + ddindentno.SelectedValue + "  and vf.category_id=" + ddCatagory.SelectedValue, true, "--Select Size--");
        }
        else
        {

            UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size." + strSize + @" FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                      dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
                       WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, " -Select Size- ");
        }

    }
    protected void dgorder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgorder, "Select$" + e.Row.RowIndex);
        }
    }
    protected void dgorder_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    protected void FillInputDescription()
    {
        string str;
        DataSet ds;
        //        str = @"select Distinct vf.ITEM_FINISHED_ID as finishedid,IndentId,vf.CATEGORY_ID,vf.ITEM_ID,vf.QualityId,vf.designId,vf.ColorId,vf.ShadecolorId,vf.ShapeId,vf.SizeId,vf.CATEGORY_NAME+'  '+vf.ITEM_NAME+' '+vf.QualityName+
        //             ' '+vf.designName+' '+vf.ColorName+' '+vf.ShadeColorName+' '+vf.ShapeName+' '+vf.SizeMtr as ItemDescription
        //            from IndentDetail ID inner join PP_Consumption PP
        //            on PP.PPId=Id.PPNo  and ID.oFinishedid=PP.FinishedId
        //            inner join V_FinishedItemDetail Vf on vf.ITEM_FINISHED_ID=PP.IFinishedId
        //            Where indentid=" + ddindentno.SelectedValue;

        str = @"select Distinct vf.ITEM_FINISHED_ID as finishedid,IM.IndentId,vf.CATEGORY_ID,vf.ITEM_ID,
            vf.QualityId,vf.designId,vf.ColorId,vf.ShadecolorId,vf.ShapeId,vf.SizeId,vf.CATEGORY_NAME+'  '+vf.ITEM_NAME+' '+vf.QualityName+
            ' '+vf.designName+' '+vf.ColorName+' '+vf.ShadeColorName+' '+vf.ShapeName+' '+
            vf.SizeMtr as ItemDescription,OCD.IUNITID from IndentMaster IM inner join IndentDetail ID on IM.indentid=ID.indentid 
            inner join PP_Consumption PP on PP.PPId=Id.PPNo  and ID.oFinishedid=PP.FinishedId
            inner join V_FinishedItemDetail Vf on vf.ITEM_FINISHED_ID=PP.IFinishedId
            left join ORDER_CONSUMPTION_DETAIL OCD on PP.OrderDetailId=OCD.ORDERDETAILID
            and OCD.OFINISHEDID=PP.FinishedId and OCD.IFINISHEDID=PP.IFinishedid
            and OCd.PROCESSID=IM.ProcessID  Where IM.indentid=" + ddindentno.SelectedValue;

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GDInputDescp.DataSource = ds;
        GDInputDescp.DataBind();
    }
    protected void GDInputDescp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GDInputDescp, "Select$" + e.Row.RowIndex);

        }
    }
    protected void GDInputDescp_SelectedIndexChanged(object sender, EventArgs e)
    {

        int r = Convert.ToInt32(GDInputDescp.SelectedIndex.ToString());
        string category = ((Label)GDInputDescp.Rows[r].FindControl("lblcategoryid")).Text;
        string Item = ((Label)GDInputDescp.Rows[r].FindControl("lblitem_id")).Text;
        string Quality = ((Label)GDInputDescp.Rows[r].FindControl("lblQualityid")).Text;
        string Color = ((Label)GDInputDescp.Rows[r].FindControl("lblColorid")).Text;
        string design = ((Label)GDInputDescp.Rows[r].FindControl("lbldesignid")).Text;
        string shape = ((Label)GDInputDescp.Rows[r].FindControl("lblshapeid")).Text;
        string shadecolor = ((Label)GDInputDescp.Rows[r].FindControl("lblshadecolorid")).Text;
        string size = ((Label)GDInputDescp.Rows[r].FindControl("lblsizeid")).Text;
        string Iunitid = ((Label)GDInputDescp.Rows[r].FindControl("lblIunitid")).Text;

        ddCatagory.SelectedValue = category;
        ddlcategorycange();
        dditemname.SelectedValue = Item;
        ITEM_CHANGED();
        if (ql.Visible == true)
        {
            dquality.SelectedValue = Quality;
            FILL_QUALITY_CHANGE();
        }
        if (dsn.Visible == true)
        {
            dddesign.SelectedValue = design;
        }
        if (clr.Visible == true)
        {
            ddcolor.SelectedValue = Color;
        }
        if (shp.Visible == true)
        {
            ddshape.SelectedValue = shape;
        }
        if (sz.Visible == true)
        {
            DDsizetype.SelectedIndex = 1;
            DDsizetype_SelectedIndexChanged(sender, e);
            ddsize.SelectedValue = size;
        }
        if (shd.Visible == true)
        {
            ddlshade.SelectedValue = shadecolor;
        }
        if (ddlunit.Items.FindByValue(Iunitid) != null)
        {
            ddlunit.SelectedValue = Iunitid;
        }
        ddgodown.SelectedIndex = -1;
        ddgodown_SelectedIndexChanged(sender, e);
        //Change Color

        //
    }

    protected void DDTagNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        ViewState["FinishedID"] = Varfinishedid;
        getstockQty(Varfinishedid);
    }

    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddgodown.Items.Count > 0 && ddgodown.SelectedIndex > 0)
        {
            ddgodown_SelectedIndexChanged(sender, new EventArgs());
        }
    }
    protected void DDBinNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        ViewState["FinishedID"] = Varfinishedid;
        if (TDTagNo.Visible == true)
        {
            FillTagNo(Varfinishedid, sender);
        }
        else
        {
            getstockQty(Varfinishedid);
        }
    }
    private void FillGSTIGST()
    {
        Label1.Visible = false;
        Label1.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[11];
            param[0] = new SqlParameter("@ProcessId", ddProcessName.SelectedValue);
            param[1] = new SqlParameter("@CategoryId", ddCatagory.SelectedValue);
            param[2] = new SqlParameter("@ItemId", dditemname.SelectedValue);
            param[3] = new SqlParameter("@QualityId", dquality.SelectedValue);
            param[4] = new SqlParameter("@EffectiveDate", txtdate.Text);
            param[5] = new SqlParameter("@GSTType", DDGSType.SelectedValue);
            param[6] = new SqlParameter("@CGSTRate", SqlDbType.Float);
            param[6].Direction = ParameterDirection.Output;
            param[7] = new SqlParameter("@SGSTRate", SqlDbType.Float);
            param[7].Direction = ParameterDirection.Output;
            param[8] = new SqlParameter("@IGSTRate", SqlDbType.Float);
            param[8].Direction = ParameterDirection.Output;
            param[9] = new SqlParameter("@CompanyID", ddCompName.SelectedValue);
            param[10] = new SqlParameter("@BranchID", 1);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_GetCGST_SGST_IGST_Rate", param);

            if (DDGSType.SelectedIndex > 0)
            {
                if (param[6].Value.ToString() != "" && param[7].Value.ToString() != "" || param[8].Value.ToString() != "")
                {
                    lblCGST.Text = param[6].Value.ToString();
                    lblSGST.Text = param[7].Value.ToString();
                    lblIGST.Text = param[8].Value.ToString();
                }
                else
                {
                    lblCGST.Text = "0";
                    lblSGST.Text = "0";
                    lblIGST.Text = "0";
                    Label1.Visible = true;
                    Label1.Text = "Please add GST/IGST regarding selected item";
                    return;
                }
            }
            else
            {
                lblCGST.Text = "0";
                lblSGST.Text = "0";
                lblIGST.Text = "0";

            }

            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            Label1.Text = ex.Message;
            con.Close();
        }
    }
    protected void DDGSType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDGSType.SelectedValue == "1")
        {
            TDCGST.Visible = true;
            TDSGST.Visible = true;
            TDIGST.Visible = false;
            FillGSTIGST();
        }
        else if (DDGSType.SelectedValue == "2")
        {
            TDCGST.Visible = false;
            TDSGST.Visible = false;
            TDIGST.Visible = true;
            FillGSTIGST();
        }
        else
        {
            TDCGST.Visible = false;
            TDSGST.Visible = false;
            TDIGST.Visible = false;
            lblCGST.Text = "0";
            lblSGST.Text = "0";
            lblIGST.Text = "0";
            // fill_text();
        }
    }
}