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

public partial class Masters_Hissab_FrmFinishingProcessHissabReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varCompanyId"] + @" Order by Companyname 
                        Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where MasterCompanyId=" + Session["varCompanyId"] + @" Order By PROCESS_NAME
                        select CI.CustomerId,CI.CustomerCode from customerinfo  CI order by CustomerCode";

            DataSet ds = SqlHelper.ExecuteDataset(str);
            CommanFunction.FillComboWithDS(DDCompany, ds, 0);
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds, 2, true, "--Select--");
            TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (variable.VarFinishingNewModuleWise == "1")
        {
            UtilityModule.ConditionalComboFill(ref DDEmpName, "select EI.EmpId,EI.EmpName+case When isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as Empname From EmpInfo EI inner join EmpProcess EMP on EI.EmpId=EMP.EmpId Where EMP.ProcessId=" + DDProcessName.SelectedValue + " order by EmpName", true, "--Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDEmpName, "Select Distinct EI.EmpId,EI.EmpName+case When isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as Empname from Empinfo EI,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM WHERE PIM.EmpId=EI.EmpId And CompanyId=" + DDCompany.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"] + " Order By EmpName", true, "--Select--");
        }

        UtilityModule.ConditionalComboFill(ref DDCategory, "select ICM.CATEGORY_ID,ICM.CATEGORY_NAME From Item_category_Master ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=CS.Categoryid and CS.id=0", true, "--Plz Select--");

    }
    protected void DDEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmpSelectedChanged();
    }
    private void EmpSelectedChanged()
    {
        //string Str = "";
        //if (DDProcessName.SelectedIndex > 0)
        //{

        //}

    }
    protected void DDcustcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select Om.OrderId,Om.CustomerOrderNo From OrderMaster OM where CompanyId=" + DDCompany.SelectedValue + " and CustomerId=" + DDcustcode.SelectedValue + "  and  Om.Status=0 order by CustomerOrderNo";
        UtilityModule.ConditionalComboFill(ref DDorderno, str, true, "--Select--");
    }
    private void CHECKVALIDCONTROL()
    {
        lblmsg.Text = "";

        if (UtilityModule.VALIDDROPDOWNLIST(DDCompany) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDProcessName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDcustcode) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDorderno) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtFromDate) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtFromDate) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(lblmsg);
    B: ;
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/main.aspx");
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        CATEGORY_DEPENDS_CONTROLS(sender);
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str1 = "";
        if (DDProcessName.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {

            Str1 = @"Select Distinct VF.QualityId,VF.QualityName From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.Process_Rec_Id=PD.Process_Rec_Id And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];

            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            UtilityModule.ConditionalComboFill(ref DDQuality, Str1, true, "--Select--");
        }
        else
        {
            Str1 = @"Select Distinct VF.Qualityid,VF.QualityNAME from V_FinishedItemDetail VF Where  VF.MasterCompanyId=" + Session["varCompanyId"] + " and vf.qualityname<>''";
            if (DDCategory.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.Category_id=" + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            Str1 = Str1 + " Order BY VF.QualityNAME ";
            UtilityModule.ConditionalComboFill(ref DDQuality, Str1, true, "--Select--");
        }
        if (DDQuality.Items.Count > 0)
        {
            DDQuality.SelectedIndex = 0;
            DDQuality_SelectedIndexChanged(sender, new EventArgs());
        }
        //************
        UtilityModule.ConditionalComboFill(ref DDShape, "select ShapeId,ShapeName From Shape", true, "--Plz Select--");
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(DDDesign, DDColor, DDShape, DDShadeColor);
    }
    private void CATEGORY_DEPENDS_CONTROLS(object sender = null)
    {
        string Str1 = "";
        if (DDProcessName.SelectedIndex > 0)
        {

            Str1 = @"Select Distinct VF.ITEM_ID,VF.ITEM_NAME From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.Process_Rec_Id=PD.Process_Rec_Id And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];


            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (DDCategory.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
            }
            UtilityModule.ConditionalComboFill(ref ddItemName, Str1, true, "--Select--");

        }

        TRDDQuality.Visible = false;
        TRDDDesign.Visible = false;
        TRDDColor.Visible = false;
        TRDDShape.Visible = false;
        TRDDSize.Visible = false;
        TRDDShadeColor.Visible = false;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from ITEM_CATEGORY_PARAMETERS Where CATEGORY_ID=" + DDCategory.SelectedValue);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in Ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        TRDDQuality.Visible = true;
                        break;
                    case "2":
                        TRDDDesign.Visible = true;
                        break;
                    case "3":
                        TRDDColor.Visible = true;
                        break;
                    case "4":
                        TRDDShape.Visible = true;
                        break;
                    case "5":
                        TRDDSize.Visible = true;
                        break;
                    case "6":
                        TRDDShadeColor.Visible = true;

                        //                        Str1 = @"Select Distinct VF.ShadecolorId,VF.shadecolorname from View_StockTranGetPassDetail PM,
                        //                        V_FinishedItemDetail VF Where PM.finishedid=VF.Item_finished_id And VF.MasterCompanyId=" + Session["varCompanyId"];
                        //                        if (DDCategory.SelectedIndex > 0)
                        //                        {
                        //                            Str1 = Str1 + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
                        //                        }
                        //                        Str1 = Str1 + " Order BY VF.shadecolorname ";
                        Str1 = @"Select Distinct VF.ShadecolorId,VF.shadecolorname from V_FinishedItemDetail VF Where VF.MasterCompanyId=" + Session["varCompanyId"] + " and vf.shadecolorname<>''";
                        if (DDCategory.SelectedIndex > 0)
                        {
                            Str1 = Str1 + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
                        }
                        if (ddItemName.SelectedIndex > 0)
                        {
                            Str1 = Str1 + " And VF.Item_id=" + ddItemName.SelectedValue;
                        }
                        if (DDQuality.SelectedIndex > 0)
                        {
                            Str1 = Str1 + " And VF.Qualityid=" + DDQuality.SelectedValue;

                        }
                        Str1 = Str1 + " Order BY VF.shadecolorname ";
                        UtilityModule.ConditionalComboFill(ref DDShadeColor, Str1, true, "--Select--");
                        break;
                }
            }
        }
        if (ddItemName.Items.Count > 0)
        {
            ddItemName.SelectedIndex = 0;
            if (sender != null)
            {
                ddItemName_SelectedIndexChanged(sender, new EventArgs());
            }
        }
    }
    private void QDCSDDFill(DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Shade, object sender = null)
    {
        string Str1 = "";
        if (DDProcessName.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {

            Str1 = @"Select Distinct VF.designId,VF.designName From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.Process_Rec_Id=PD.Process_Rec_Id And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];

            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0 && TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.QualityId=" + DDQuality.SelectedValue;
            }
            Str1 = Str1 + " order by Designname";
            UtilityModule.ConditionalComboFill(ref DDDesign, Str1, true, "--Select--");
        }
        if (DDProcessName.SelectedIndex > 0 && TRDDColor.Visible == true)
        {

            Str1 = @"Select Distinct VF.ColorId,VF.ColorName From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.Process_Rec_Id=PD.Process_Rec_Id And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];


            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0 && TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.QualityId=" + DDQuality.SelectedValue;
            }
            Str1 = Str1 + " order by Colorname";
            UtilityModule.ConditionalComboFill(ref DDColor, Str1, true, "--Select--");
        }
        if (DDProcessName.SelectedIndex > 0 && TRDDShape.Visible == true)
        {

            Str1 = @"Select Distinct VF.ShapeId,VF.ShapeName From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.Process_Rec_Id=PD.Process_Rec_Id And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];


            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0 && TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.QualityId=" + DDQuality.SelectedValue;
            }

            UtilityModule.ConditionalComboFill(ref DDShape, Str1, true, "--Select--");
        }
        if (DDProcessName.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {

            Str1 = @"Select Distinct VF.ShadecolorId,VF.ShadeColorName From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.Process_Rec_Id=PD.Process_Rec_Id And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];


            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0 && TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.QualityId=" + DDQuality.SelectedValue;
            }
            UtilityModule.ConditionalComboFill(ref DDShadeColor, Str1, true, "--Select--");
        }

    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        lblsizename.Text = ParameterList[4];
        lblshadename.Text = ParameterList[7];
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str1 = "";
        string strSize = "Sizeft";
        if (chkmtr.Checked == true)
        {
            strSize = "Sizemtr";
        }
        if (DDProcessName.SelectedIndex > 0 && TRDDSize.Visible == true)
        {

            Str1 = @"Select Distinct VF.SizeId,VF." + strSize + " as size From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                        PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF Where PM.Process_Rec_Id=PD.Process_Rec_Id And 
                        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];


            if (DDEmpName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
            {
                Str1 = Str1 + " And VF.QualityId=" + DDQuality.SelectedValue;
            }
            Str1 = Str1 + " order by size";
            UtilityModule.ConditionalComboFill(ref DDSize, Str1, true, "--Select--");
        }
    }
    protected void ChkForDate_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForDate.Checked == true)
        {
            trDates.Visible = true;
        }
        else
        {
            trDates.Visible = false;
        }
    }
    protected void chkmtr_CheckedChanged(object sender, EventArgs e)
    {
        DDShape_SelectedIndexChanged(sender, e);
    }
    protected void btnprint_Click(object sender, EventArgs e)
    {
        if (lblmsg.Text == "")
        {
            //CHECKVALIDCONTROL();
            try
            {
                if (DDProcessName.SelectedIndex >= 0)
                {
                    ProcessReceiveDetailSummary();
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Hissab/FrmFinishingProcessHissabReport.aspx");
                lblmsg.Text = ex.Message;
                lblmsg.Visible = true;
            }
        }
    }
    private void ProcessReceiveDetailSummary()
    {
        DataSet ds = new DataSet();
        string where = DDProcessName.SelectedItem.Text + "  PAYMENT SHEET ";
        string strCondition = "And PM.CompanyId=" + DDCompany.SelectedValue;
        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            strCondition = strCondition + " And PM.ReceiveDate>='" + TxtFromDate.Text + "' And PM.ReceiveDate<='" + TxtToDate.Text + "'";
            where = where + " From:" + TxtFromDate.Text + " and To: " + TxtToDate.Text + ",";
        }
        if (DDcustcode.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.Customerid=" + DDcustcode.SelectedValue;
            where = where + " Cust Code:" + DDcustcode.SelectedItem.Text + ",";
        }
        if (DDorderno.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.orderid=" + DDorderno.SelectedValue;
            where = where + " Order No:" + DDorderno.SelectedItem.Text + ",";
        }
        //if (DDChallanNo.SelectedIndex > 0)
        //{
        //    strCondition = strCondition + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue;
        //    where = where + " ChallanNo:" + DDChallanNo.SelectedItem.Text + ",";
        //}
        if (DDCategory.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            where = where + " ItemName:" + ddItemName.SelectedItem.Text + ",";
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
            where = where + " Quality:" + DDQuality.SelectedItem.Text + ",";
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
            where = where + " Design:" + DDDesign.SelectedItem.Text + ",";
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        if (TxtLocalOrderNo.Text != "")
        {
            strCondition = strCondition + " And OM.LocalOrder = '" + TxtLocalOrderNo.Text + "'";
        }
        //End Conditions
        SqlParameter[] param = new SqlParameter[8];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        param[2] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[3] = new SqlParameter("@Todate", TxtToDate.Text);
        param[4] = new SqlParameter("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        param[6] = new SqlParameter("@Where", strCondition);
        param[7] = new SqlParameter("@LocalOrderNo", TxtLocalOrderNo.Text);

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "FINISHINGPROCESSHISSABREPORT", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            Decimal TQty = 0, TAmount = 0;
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 6;

            sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            sht.PageSetup.AdjustTo(85);
            sht.PageSetup.PaperSize = XLPaperSize.A4Paper;

            //
            sht.PageSetup.Margins.Top = 1.21;
            sht.PageSetup.Margins.Left = 0.47;
            sht.PageSetup.Margins.Right = 0.36;
            sht.PageSetup.Margins.Bottom = 0.19;
            sht.PageSetup.Margins.Header = 2.20;
            sht.PageSetup.Margins.Footer = 0.3;
            sht.PageSetup.SetScaleHFWithDocument();

            sht.Column("A").Width = 19.56;
            sht.Column("B").Width = 13.22;
            sht.Column("C").Width = 10.89;
            sht.Column("D").Width = 10.89;
            sht.Column("E").Width = 10.89;
            sht.Column("F").Width = 10.89;
            sht.Column("G").Width = 10.89;
            sht.Column("H").Width = 10.89;
            sht.Column("I").Width = 10.89;
            sht.Column("J").Width = 10.89;
            sht.Column("K").Width = 10.89;
            sht.Column("L").Width = 10.89;
            sht.Column("M").Width = 10.89;
            sht.Column("N").Width = 10.89;
            sht.Column("O").Width = 8.89;
            sht.Column("P").Width = 10.22;
            sht.Column("Q").Width = 6.33;
            sht.Column("R").Width = 10.89;

            /////***********
            sht.Range("A1:R1").Merge();
            sht.Range("A1").Value = DDProcessName.SelectedItem.Text + " " + "PAYMENT SHEET ";
            sht.Range("A2:R2").Merge();
            sht.Range("A2").Value = "Filter By :  " + where;
            sht.Row(2).Height = 30;
            sht.Range("A1:R1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:R2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:R2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A1:R2").Style.Font.Bold = true;
            //***********Filter By Item_Name

            sht.Range("A3").Value = "OrderNo";
            sht.Range("A3").Style.Font.FontName = "Calibri";
            sht.Range("A3").Style.Font.FontSize = 12;
            sht.Range("A3").Style.Font.SetBold();
            sht.Range("A3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A3").Style.Fill.BackgroundColor = XLColor.LightGray;

            if (ds.Tables[1].Rows.Count > 0)
            {
                sht.Range("B3").Value = ds.Tables[1].Rows[0]["CustomerOrderNo"].ToString();
                sht.Range("B3").Style.Font.FontName = "Calibri";
                sht.Range("B3").Style.Font.FontSize = 12;
                sht.Range("B3").Style.Font.SetBold();
                sht.Range("B3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B3").Style.Fill.BackgroundColor = XLColor.LightGray;
                sht.Range("B3").Style.Alignment.WrapText = true;

                int row2 = 3;
                int noofrows2 = 0;
                int i2 = 0;
                int Dynamiccol2 = 2;
                int Dynamiccolstart2 = Dynamiccol2 + 1;
                int Dynamiccolend2;
                int Totalcol2;

                DataTable dtdistinct2 = ds.Tables[1].DefaultView.ToTable(true, "Size");
                noofrows2 = dtdistinct2.Rows.Count;

                for (i2 = 0; i2 < noofrows2; i2++)
                {
                    string UnitName = "";
                    DataRow[] foundRows;
                    foundRows = ds.Tables[1].Select("size='" + dtdistinct2.Rows[i2]["Size"].ToString() + "' ");
                    if (foundRows.Length > 0)
                    {
                        foreach (DataRow row5 in foundRows)
                        {
                            UnitName = row5["UnitName"].ToString();
                            break;
                        }
                    }

                    Dynamiccol2 = Dynamiccol2 + 1;
                   //sht.Cell(row2, Dynamiccol2).Value = ds.Tables[1].Rows[0]["UnitName"];

                    sht.Cell(row2, Dynamiccol2).Value = UnitName;
                    sht.Cell(row2, Dynamiccol2).Style.Font.Bold = true;
                    sht.Cell(row2, Dynamiccol2).Style.Font.FontName = "Calibri";
                    sht.Cell(row2, Dynamiccol2).Style.Font.FontSize = 10;
                    sht.Cell(row2, Dynamiccol2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell(row2, Dynamiccol2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Cell(row2, Dynamiccol2).Style.Alignment.WrapText = true;
                    //sht.Cell(row2, Dynamiccol2).Value = dtdistinct2.Rows[i2]["ReceiveDate"].ToString();
                }
                Dynamiccolend2 = Dynamiccol2;

                Totalcol2 = Dynamiccolend2 + 1;



                //sht.Range(sht.Cell(row2, Totalcol2), sht.Cell(row2, (Totalcol2 + 3))).Merge();
                //sht.Range(sht.Cell(row2, Totalcol2), sht.Cell(row2, (Totalcol2 + 3))).Value = "TOTAL COLOR WISE PCS";
                //sht.Range(sht.Cell(row2, Totalcol2), sht.Cell(row2, (Totalcol2 + 3))).Style.Font.Bold = true;
                //sht.Range(sht.Cell(row2, Totalcol2), sht.Cell(row2, (Totalcol2 + 3))).Style.Font.FontName = "Calibri";
                //sht.Range(sht.Cell(row2, Totalcol2), sht.Cell(row2, (Totalcol2 + 3))).Style.Font.FontSize = 10;
                //sht.Range(sht.Cell(row2, Totalcol2), sht.Cell(row2, (Totalcol2 + 3))).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Range(sht.Cell(row2, Totalcol2), sht.Cell(row2, (Totalcol2 + 3))).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                //sht.Range(sht.Cell(row2, Totalcol2), sht.Cell(row2, (Totalcol2 + 3))).Style.Alignment.WrapText = true;

                //sht.Range(Totalcol2 + ":" + (Totalcol2 + 3)).Merge();                   
                sht.Cell(row2, Totalcol2).Value = "TOTAL COLOR WISE PCS";
                sht.Cell(row2, Totalcol2).Style.Font.Bold = true;
                sht.Cell(row2, Totalcol2).Style.Font.FontName = "Calibri";
                sht.Cell(row2, Totalcol2).Style.Font.FontSize = 10;
                sht.Cell(row2, Totalcol2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell(row2, Totalcol2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Cell(row2, Totalcol2).Style.Alignment.WrapText = true;

                Dynamiccolend2 = Totalcol2;

                Totalcol2 = Dynamiccolend2 + 1;
                sht.Range(row2 + ":" + (row2 + 3)).Merge();
                sht.Cell(row2, Totalcol2).Value = "TOTAL COLOR WISE AREA M2";
                sht.Cell(row2, Totalcol2).Style.Font.Bold = true;
                sht.Cell(row2, Totalcol2).Style.Font.FontName = "Calibri";
                sht.Cell(row2, Totalcol2).Style.Font.FontSize = 10;
                sht.Cell(row2, Totalcol2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell(row2, Totalcol2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Cell(row2, Totalcol2).Style.Alignment.WrapText = true;
                Dynamiccolend2 = Totalcol2;

                Totalcol2 = Dynamiccolend2 + 1;
                sht.Range(row2 + ":" + (row2 + 3)).Merge();
                sht.Cell(row2, Totalcol2).Value = "RATE";
                sht.Cell(row2, Totalcol2).Style.Font.Bold = true;
                sht.Cell(row2, Totalcol2).Style.Font.FontName = "Calibri";
                sht.Cell(row2, Totalcol2).Style.Font.FontSize = 10;
                sht.Cell(row2, Totalcol2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell(row2, Totalcol2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Cell(row2, Totalcol2).Style.Alignment.WrapText = true;
                Dynamiccolend2 = Totalcol2;

                Totalcol2 = Dynamiccolend2 + 1;
                sht.Range(row2 + ":" + (row2 + 3)).Merge();
                sht.Cell(row2, Totalcol2).Value = "TOTAL AMOUNT";
                sht.Cell(row2, Totalcol2).Style.Font.Bold = true;
                sht.Cell(row2, Totalcol2).Style.Font.FontName = "Calibri";
                sht.Cell(row2, Totalcol2).Style.Font.FontSize = 10;
                sht.Cell(row2, Totalcol2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell(row2, Totalcol2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Cell(row2, Totalcol2).Style.Alignment.WrapText = true;

                row2 = row2 + 1;
            }

            sht.Range("A4:A6").Merge();
            sht.Range("A4").Value = "REF. / STYLE / DESIGN NO";
            sht.Range("A4:A6").Style.Font.FontName = "Calibri";
            sht.Range("A4:A6").Style.Font.FontSize = 11;
            sht.Range("A4:A6").Style.Font.SetBold();
            sht.Range("A4:A6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A4:A6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A4:A6").Style.Alignment.WrapText = true;

            sht.Range("B4:B6").Merge();
            sht.Range("B4").Value = "COLOR";
            sht.Range("B4:B6").Style.Font.FontName = "Calibri";
            sht.Range("B4:B6").Style.Font.FontSize = 11;
            sht.Range("B4:B6").Style.Font.SetBold();
            sht.Range("B4:B6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("B4:B6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("B4:B6").Style.Alignment.WrapText = true;

            int Dynamiccol3 = 2;
            int Dynamiccolstart3 = Dynamiccol3 + 1;
            int Dynamiccolend3 = 0;

            int Dynamiccolend4 = 0;
            int Totalcol4;

            int Dynamiccol5 = 2;
            int Dynamiccolstart5 = Dynamiccol5 + 1;
            int Dynamiccolend5 = 0;
            int Totalcol5;
            int Dynamiccol6 = 2;
            int Dynamiccolstart6 = Dynamiccol6 + 1;
            int Dynamiccolend6 = 0;

            if (ds.Tables[1].Rows.Count > 0)
            {
                int row3 = 4;
                int noofrows3 = 0;
                int i3 = 0;

                int Totalcol3;

                DataTable dtdistinct3 = ds.Tables[1].DefaultView.ToTable(true, "Size");
                noofrows3 = dtdistinct3.Rows.Count;

                for (i3 = 0; i3 < noofrows3; i3++)
                {
                    Dynamiccol3 = Dynamiccol3 + 1;
                    sht.Cell(row3, Dynamiccol3).Value = dtdistinct3.Rows[i3]["Size"].ToString();
                    sht.Cell(row3, Dynamiccol3).Style.Font.Bold = true;
                    sht.Cell(row3, Dynamiccol3).Style.Font.FontName = "Calibri";
                    sht.Cell(row3, Dynamiccol3).Style.Font.FontSize = 10;
                    sht.Cell(row3, Dynamiccol3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell(row3, Dynamiccol3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Cell(row3, Dynamiccol3).Style.Alignment.WrapText = true;
                }
                Dynamiccolend3 = Dynamiccol3;
                Dynamiccolend4 = Dynamiccol3;
                Dynamiccolend5 = Dynamiccol3;
                Dynamiccolend6 = Dynamiccol3;
                row3 = row3 + 3;



            }


            DataTable dtdistinctItem = ds.Tables[2].DefaultView.ToTable(true, "ItemName", "QualityName", "DesignName", "ColorName");

            row = row + 1;
            foreach (DataRow dritem in dtdistinctItem.Rows)
            {
                Dynamiccolend4 = Dynamiccol3;
                using (var a = sht.Range("A" + row + ":S" + row))
                {
                    a.Style.Font.Bold = true;
                    a.Style.Font.FontName = "Calibri";
                    a.Style.Font.FontSize = 10;
                    a.Style.Alignment.WrapText = true;
                    a.Style.Alignment.WrapText = true;
                    a.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    a.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    a.Style.Fill.BackgroundColor = XLColor.LightGray;
                }

                sht.Range("A" + row).SetValue(dritem["ItemName"] + "" + dritem["QualityName"] + "" + dritem["DesignName"]);
                sht.Range("B" + row).SetValue(dritem["ColorName"]);

                for (int m = Dynamiccolstart3; m <= Dynamiccolend3; m++)
                {
                    var itemname = sht.Cell(4, m).Value;
                    //decimal WeightQty = 0;
                    //decimal Penality = 0;
                    string IssueQty = "0";

                    DataRow[] foundRows;
                    foundRows = ds.Tables[2].Select("size='" + itemname + "' and ItemName='" + dritem["ItemName"] + "' and QualityName='" + dritem["QualityName"] + "' and DesignName='" + dritem["DesignName"] + "' and ColorName='" + dritem["ColorName"] + "' ");
                    if (foundRows.Length > 0)
                    {
                        foreach (DataRow row4 in foundRows)
                        {
                            IssueQty = row4["Qty"].ToString();
                            break;
                        }
                    }
                    sht.Cell(row, m).SetValue(IssueQty);

                }

                Totalcol4 = Dynamiccolend4 + 1;
                sht.Cell(row, Totalcol4).Value = ds.Tables[2].Compute("sum(Qty)", "ItemName='" + dritem["ItemName"] + "' and QualityName='" + dritem["QualityName"] + "' and DesignName='" + dritem["DesignName"] + "' and ColorName='" + dritem["ColorName"] + "'");
                sht.Cell(row, Totalcol4).Style.Font.Bold = true;
                sht.Cell(row, Totalcol4).Style.Font.FontName = "Calibri";
                sht.Cell(row, Totalcol4).Style.Font.FontSize = 10;
                sht.Cell(row, Totalcol4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell(row, Totalcol4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Cell(row, Totalcol4).Style.Alignment.WrapText = true;
                Dynamiccolend4 = Totalcol4;

                Totalcol4 = Dynamiccolend4 + 1;
                sht.Cell(row, Totalcol4).Value = ds.Tables[2].Compute("sum(TotalArea)", "ItemName='" + dritem["ItemName"] + "' and QualityName='" + dritem["QualityName"] + "' and DesignName='" + dritem["DesignName"] + "' and ColorName='" + dritem["ColorName"] + "'");
                sht.Cell(row, Totalcol4).Style.Font.Bold = true;
                sht.Cell(row, Totalcol4).Style.Font.FontName = "Calibri";
                sht.Cell(row, Totalcol4).Style.Font.FontSize = 10;
                sht.Cell(row, Totalcol4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell(row, Totalcol4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Cell(row, Totalcol4).Style.Alignment.WrapText = true;
                Dynamiccolend4 = Totalcol4;

                decimal TotalSumOneRow = 0;
                DataTable dtdistinctReceiveItem = ds.Tables[0].DefaultView.ToTable(true, "EmpName");
                row = row + 1;

                foreach (DataRow drReceiveitem in dtdistinctReceiveItem.Rows)
                {
                    //DataTable dtdistinctEmpName = ds.Tables[0].DefaultView.ToTable(true, "EmpName");
                    //foreach (DataRow drEmpName in dtdistinctEmpName.Rows)
                    //{
                    //    sht.Range("A" + row).SetValue(drEmpName["EmpName"]);
                    //}

                    sht.Range("A" + row).SetValue(drReceiveitem["EmpName"]);

                    for (int k = Dynamiccolstart6; k <= Dynamiccolend6; k++)
                    {
                        var itemname = sht.Cell(4, k).Value;
                        decimal IssueQty = 0;

                        DataRow[] foundRows;
                        foundRows = ds.Tables[0].Select("size='" + itemname + "' and EmpName='" + drReceiveitem["EmpName"] + "' and  ItemName='" + dritem["ItemName"] + "' and QualityName='" + dritem["QualityName"] + "' and DesignName='" + dritem["DesignName"] + "' and ColorName='" + dritem["ColorName"] + "' ");
                        if (foundRows.Length > 0)
                        {
                            IssueQty = Convert.ToDecimal(ds.Tables[0].Compute("sum(Qty)", "size='" + itemname + "' and EmpName='" + drReceiveitem["EmpName"] + "' and  ItemName='" + dritem["ItemName"] + "' and QualityName='" + dritem["QualityName"] + "' and DesignName='" + dritem["DesignName"] + "' and ColorName='" + dritem["ColorName"] + "' "));
                        }
                        TotalSumOneRow = TotalSumOneRow + IssueQty;
                        sht.Cell(row, k).SetValue(IssueQty);

                    }
                    Dynamiccolend5 = Dynamiccolend6;
                    Totalcol5 = Dynamiccolend5 + 1;
                    //sht.Cell(row, Totalcol4).Value = ds.Tables[1].Compute("sum(Qty)", "ItemName='" + dritem["ItemName"] + "' and QualityName='" + dritem["QualityName"] + "' and DesignName='" + dritem["DesignName"] + "' and ColorName='" + dritem["ColorName"] + "'");
                    //sht.Cell(row, Totalcol5).Value = TotalSumOneRow;
                    sht.Cell(row, Totalcol5).Value = (ds.Tables[0].Compute("sum(Qty)", "EmpName='" + drReceiveitem["EmpName"] + "' and  ItemName='" + dritem["ItemName"] + "' and QualityName='" + dritem["QualityName"] + "' and DesignName='" + dritem["DesignName"] + "' and ColorName='" + dritem["ColorName"] + "' "));
                    sht.Cell(row, Totalcol5).Style.Font.Bold = true;
                    sht.Cell(row, Totalcol5).Style.Font.FontName = "Calibri";
                    sht.Cell(row, Totalcol5).Style.Font.FontSize = 10;
                    sht.Cell(row, Totalcol5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell(row, Totalcol5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Cell(row, Totalcol5).Style.Alignment.WrapText = true;
                    Dynamiccolend5 = Totalcol5;

                    Totalcol5 = Dynamiccolend5 + 1;
                    sht.Cell(row, Totalcol5).Value = ds.Tables[0].Compute("sum(Area)", "ItemName='" + dritem["ItemName"] + "' and QualityName='" + dritem["QualityName"] + "' and DesignName='" + dritem["DesignName"] + "' and ColorName='" + dritem["ColorName"] + "' and EmpName='" + drReceiveitem["EmpName"] + "' ");
                    sht.Cell(row, Totalcol5).Style.Font.Bold = true;
                    sht.Cell(row, Totalcol5).Style.Font.FontName = "Calibri";
                    sht.Cell(row, Totalcol5).Style.Font.FontSize = 10;
                    sht.Cell(row, Totalcol5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell(row, Totalcol5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Cell(row, Totalcol5).Style.Alignment.WrapText = true;
                    Dynamiccolend5 = Totalcol5;


                    Totalcol5 = Dynamiccolend5 + 1;

                    decimal rate = 0;
                    DataRow[] foundRows2;
                    foundRows2 = ds.Tables[0].Select("ItemName='" + dritem["ItemName"] + "' and QualityName='" + dritem["QualityName"] + "' and DesignName='" + dritem["DesignName"] + "' and ColorName='" + dritem["ColorName"] + "' and EmpName='" + drReceiveitem["EmpName"] + "' ");
                    if (foundRows2.Length > 0)
                    {
                        foreach (DataRow row4 in foundRows2)
                        {
                            rate = Convert.ToDecimal(row4["Rate"].ToString());
                            break;
                        }
                    }
                    sht.Cell(row, Totalcol5).Value = rate;
                    sht.Cell(row, Totalcol5).Style.Font.Bold = true;
                    sht.Cell(row, Totalcol5).Style.Font.FontName = "Calibri";
                    sht.Cell(row, Totalcol5).Style.Font.FontSize = 10;
                    sht.Cell(row, Totalcol5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell(row, Totalcol5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Cell(row, Totalcol5).Style.Alignment.WrapText = true;
                    Dynamiccolend5 = Totalcol5;

                    Totalcol5 = Dynamiccolend5 + 1;
                    sht.Cell(row, Totalcol5).Value = ds.Tables[0].Compute("sum(Amount)", "ItemName='" + dritem["ItemName"] + "' and QualityName='" + dritem["QualityName"] + "' and DesignName='" + dritem["DesignName"] + "' and ColorName='" + dritem["ColorName"] + "' and EmpName='" + drReceiveitem["EmpName"] + "' ");
                    sht.Cell(row, Totalcol5).Style.Font.Bold = true;
                    sht.Cell(row, Totalcol5).Style.Font.FontName = "Calibri";
                    sht.Cell(row, Totalcol5).Style.Font.FontSize = 10;
                    sht.Cell(row, Totalcol5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell(row, Totalcol5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Cell(row, Totalcol5).Style.Alignment.WrapText = true;
                    Dynamiccolend5 = Totalcol5;

                    row = row + 1;
                }

                row = row + 1;
            }
            row = row + 2;

            sht.Range("A" + row).Value = "PREPARED BY SADDAM";
            sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Calibri";
            sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 11;
            sht.Range("A" + row + ":B" + row).Style.Font.SetBold();
            sht.Range("A" + row + ":B" + row).Style.Fill.BackgroundColor = XLColor.LightGray;
            sht.Range("A" + row + ":B" + row).Merge();
            sht.Range("A" + row + ":B" + row).Style.Alignment.SetWrapText();
            sht.Range("A" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A" + row + ":B" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

            using (var a = sht.Range("A" + row + ":B" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            sht.Range("D" + row).Value = "CHECKED BY ( Habib / Mahtab )";
            sht.Range("D" + row + ":F" + row).Style.Font.FontName = "Calibri";
            sht.Range("D" + row + ":F" + row).Style.Font.FontSize = 11;
            sht.Range("D" + row + ":F" + row).Style.Font.SetBold();
            sht.Range("D" + row + ":F" + row).Style.Fill.BackgroundColor = XLColor.LightGray;
            sht.Range("D" + row + ":F" + row).Merge();
            sht.Range("D" + row + ":F" + row).Style.Alignment.SetWrapText();
            sht.Range("D" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("D" + row + ":F" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

            using (var a = sht.Range("D" + row + ":F" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            sht.Range("H" + row).Value = "VERIFIED BY - A/C";
            sht.Range("H" + row + ":J" + row).Style.Font.FontName = "Calibri";
            sht.Range("H" + row + ":J" + row).Style.Font.FontSize = 11;
            sht.Range("H" + row + ":J" + row).Style.Font.SetBold();
            sht.Range("H" + row + ":J" + row).Style.Fill.BackgroundColor = XLColor.LightGray;
            sht.Range("H" + row + ":J" + row).Merge();
            sht.Range("H" + row + ":J" + row).Style.Alignment.SetWrapText();
            sht.Range("H" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("H" + row + ":J" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

            using (var a = sht.Range("H" + row + ":J" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            row = row + 1;

            sht.Range("A" + (row + 2)).Value = "";
            sht.Range("A" + row + ":B" + (row + 2)).Style.Font.FontName = "Calibri";
            sht.Range("A" + row + ":B" + (row + 2)).Style.Font.FontSize = 11;
            sht.Range("A" + row + ":B" + (row + 2)).Style.Font.SetBold();
            sht.Range("A" + row + ":B" + (row + 2)).Merge();
            sht.Range("A" + row + ":B" + (row + 2)).Style.Alignment.SetWrapText();
            sht.Range("A" + row + ":B" + (row + 2)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A" + row + ":B" + (row + 2)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

            using (var a = sht.Range("A" + row + ":B" + (row + 2)))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            sht.Range("D" + (row + 2)).Value = "";
            sht.Range("D" + row + ":F" + (row + 2)).Style.Font.FontName = "Calibri";
            sht.Range("D" + row + ":F" + (row + 2)).Style.Font.FontSize = 11;
            sht.Range("D" + row + ":F" + (row + 2)).Style.Font.SetBold();
            sht.Range("D" + row + ":F" + (row + 2)).Merge();
            sht.Range("D" + row + ":F" + (row + 2)).Style.Alignment.SetWrapText();
            sht.Range("D" + row + ":F" + (row + 2)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("D" + row + ":F" + (row + 2)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

            using (var a = sht.Range("D" + row + ":F" + (row + 2)))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            sht.Range("H" + (row + 2)).Value = "";
            sht.Range("H" + row + ":J" + (row + 2)).Style.Font.FontName = "Calibri";
            sht.Range("H" + row + ":J" + (row + 2)).Style.Font.FontSize = 11;
            sht.Range("H" + row + ":J" + (row + 2)).Style.Font.SetBold();
            sht.Range("H" + row + ":J" + (row + 2)).Merge();
            sht.Range("H" + row + ":J" + (row + 2)).Style.Alignment.SetWrapText();
            sht.Range("H" + row + ":J" + (row + 2)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("H" + row + ":J" + (row + 2)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

            using (var a = sht.Range("H" + row + ":J" + (row + 2)))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            row = row + 1;

            //row = 3;
            //Decimal tqty = 0;
            //string empname = "";
            //int rowfrom = 0, rowto = 0;

            //DataTable dtdistinct1 = ds.Tables[0].DefaultView.ToTable(true, "EmpName");

            //foreach (DataRow dr2 in dtdistinct1.Rows)
            //{
            //    DataView dv1 = new DataView(ds.Tables[0]);
            //    dv1.RowFilter = "EmpName='" + dr2["EmpName"] + "' ";
            //    DataSet ds2 = new DataSet();
            //    ds2.Tables.Add(dv1.ToTable());

            //    //row = row + 1;

            //    //sht.Range("A" + row).Value = "Emp Name";
            //    //sht.Range("B" + row).Value = "Quality";
            //    //sht.Range("C" + row).Value = "Design";
            //    //sht.Range("D" + row).Value = "Color";
            //    //sht.Range("E" + row).Value = "Shape";

            //    //sht.Range("F" + row).Value = "Size";
            //    //sht.Range("G" + row).Value = "Rec Qty";                      
            //    //sht.Range("A" + row + ":G" + row).Style.Font.SetBold();

            //    DataTable dtdistinctItem = ds2.Tables[0].DefaultView.ToTable(true, "EmpName", "Item_Name", "QualityName", "DesignName", "ColorName", "ShapeName", "Width", "Length", "item_finished_id");

            //    rowfrom = row + 1;
            //    foreach (DataRow dritem in dtdistinctItem.Rows)
            //    {
            //        row = row + 1;
            //        sht.Range("A" + row).SetValue(dritem["empname"]);
            //        sht.Range("B" + row).SetValue(dritem["QUALITYNAME"]);
            //        sht.Range("C" + row).SetValue(dritem["DesignName"]);
            //        sht.Range("D" + row).SetValue(dritem["ColorName"]);
            //        sht.Range("E" + row).SetValue(dritem["ShapeName"]);
            //        sht.Range("F" + row).SetValue(dritem["Width"].ToString() + 'X' + dritem["Length"].ToString());

            //        var qty = ds.Tables[0].Compute("sum(Qty)", "EmpName='" + dr2["EmpName"] + "' and Item_Finished_Id='" + dritem["Item_Finished_Id"] + "'");
            //        tqty = tqty + Convert.ToDecimal(qty);
            //        sht.Range("G" + row).SetValue(qty);

            //        rowto = row;
            //    }
            //    row = row + 1;
            //    sht.Range("F" + row).Value = "Total";
            //    sht.Range("G" + row).FormulaA1 = "=SUM(G" + rowfrom + ":$G$" + rowto + ")";
            //    sht.Range("F" + row + ":G" + row).Style.Font.SetBold();
            //    row = row + 1;

            //    //using (var a = sht.Range("A" + (rowfrom-1) + ":G" + (rowto+1)))
            //    //{
            //    //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //    //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //    //    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //    //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //    //}
            //}

            ////*************GRAND TOTAL
            //sht.Range("H" + row + ":J" + row).Style.Font.SetBold();
            //sht.Range("F" + row).Value = "Grand Total";
            //sht.Range("G" + row).SetValue(tqty);
            //sht.Range("F" + row + ":G" + row).Style.Font.SetBold();

            //***********BOrders
            using (var a = sht.Range("A1" + ":S" + (row - 1)))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            ////*************
            //sht.Columns(1, 30).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("FinishingProcessHissabReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }




    //protected void FolioHindiReportHafizia()
    //{
    //    DataSet ds = new DataSet();
    //    if (DDFolioNo.SelectedIndex > 0)
    //    {
    //        lblmsg.Text = "";
    //        try
    //        {

    //            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //            if (con.State == ConnectionState.Closed)
    //            {
    //                con.Open();
    //            }
    //            SqlCommand cmd = new SqlCommand("PRO_WEAVERFOLIO_HAFIZIA_HINDI_REPORT", con);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            cmd.CommandTimeout = 1000;

    //            cmd.Parameters.AddWithValue("@issueorderid", DDFolioNo.SelectedValue);
    //            //cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
    //            SqlDataAdapter ad = new SqlDataAdapter(cmd);
    //            cmd.ExecuteNonQuery();
    //            ad.Fill(ds);


    //            //SqlParameter[] param = new SqlParameter[1];
    //            //param[0] = new SqlParameter("@issueorderid", DDFolioNo.SelectedValue);


    //            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_WEAVERFOLIO_HAFIZIA_HINDI_REPORT", param);

    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
    //                {
    //                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
    //                }
    //                string Path = "";
    //                var xapp = new XLWorkbook();
    //                var sht = xapp.Worksheets.Add("FolioReport");
    //                //int row = 0;

    //                sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
    //                sht.PageSetup.AdjustTo(85);
    //                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;

    //                //
    //                sht.PageSetup.Margins.Top = 1.21;
    //                sht.PageSetup.Margins.Left = 0.47;
    //                sht.PageSetup.Margins.Right = 0.36;
    //                sht.PageSetup.Margins.Bottom = 0.19;
    //                sht.PageSetup.Margins.Header = 2.20;
    //                sht.PageSetup.Margins.Footer = 0.3;
    //                sht.PageSetup.SetScaleHFWithDocument();

    //                sht.Column("A").Width = 15.67;
    //                sht.Column("B").Width = 15.22;
    //                sht.Column("C").Width = 7.78;
    //                sht.Column("D").Width = 8.22;
    //                sht.Column("E").Width = 9.22;
    //                sht.Column("F").Width = 10.89;
    //                sht.Column("G").Width = 11.22;
    //                sht.Column("H").Width = 9.89;
    //                sht.Column("I").Width = 9.89;
    //                sht.Column("J").Width = 14.33;
    //                sht.Column("K").Width = 9.89;
    //                sht.Column("L").Width = 9.89;
    //                sht.Column("M").Width = 9.89;
    //                sht.Column("N").Width = 9.89;
    //                //sht.Column("O").Width = 10.33;
    //                //sht.Column("P").Width = 8.33;
    //                //sht.Column("Q").Width = 6.22;
    //                //sht.Column("R").Width = 6.22;

    //                sht.Row(2).Height = 35;
    //                sht.Row(3).Height = 30;
    //                sht.Row(4).Height = 30;
    //                sht.Row(5).Height = 30;
    //                sht.Row(6).Height = 30;
    //                sht.Row(7).Height = 30;
    //                sht.Row(8).Height = 30;
    //                sht.Row(9).Height = 30;
    //                sht.Row(10).Height = 30;
    //                sht.Row(11).Height = 30;


    //                // sht.Row(1).Height = 13.80;

    //                sht.Range("A1:N1").Merge();
    //                sht.Range("A1").Value = "ORDER - HANDLOOM DURRIE (JOB WORK)";

    //                sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("A1:N1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
    //                sht.Range("A1:N1").Style.Alignment.SetWrapText();
    //                sht.Range("A1:N1").Style.Font.FontName = "Calibri";
    //                sht.Range("A1:N1").Style.Font.FontSize = 18;
    //                sht.Range("A1:N1").Style.Font.Bold = true;
    //                //*******Header

    //                sht.Range("A2").Value = ds.Tables[0].Rows[0]["CompanyName"];
    //                sht.Range("A2:B2").Style.Font.FontName = "Calibri";
    //                sht.Range("A2:B2").Style.Font.FontSize = 14;
    //                sht.Range("A2:B2").Style.Font.SetBold();
    //                sht.Range("A2:B2").Merge();
    //                sht.Range("A2:B2").Style.Alignment.SetWrapText();
    //                sht.Range("A2:B2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
    //                sht.Range("A2:B2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


    //                sht.Range("A3").Value = ds.Tables[0].Rows[0]["COMPADDR1"] + " " + ds.Tables[0].Rows[0]["COMPADDR2"];
    //                sht.Range("A3:B4").Style.Font.FontName = "Calibri";
    //                sht.Range("A3:B4").Style.Font.FontSize = 11;
    //                sht.Range("A3:B4").Style.Font.SetBold();
    //                sht.Range("A3:B4").Merge();
    //                sht.Range("A3:B4").Style.Alignment.SetWrapText();
    //                sht.Range("A3:B4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
    //                sht.Range("A3:B4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


    //                sht.Range("A5").Value = "GSTIN";
    //                sht.Range("A5").Style.Font.FontName = "Calibri";
    //                sht.Range("A5").Style.Font.FontSize = 10;
    //                sht.Range("A5").Style.Font.SetBold();

    //                sht.Range("B5").Value = ds.Tables[0].Rows[0]["CompanyGstNo"];
    //                sht.Range("B5").Style.Font.FontName = "Calibri";
    //                sht.Range("B5").Style.Font.FontSize = 10;
    //                //sht.Range("B5").Merge();
    //                sht.Range("B5").Style.Alignment.SetWrapText();
    //                sht.Range("B5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

    //                sht.Range("A6").Value = "Phone No";
    //                sht.Range("A6").Style.Font.FontName = "Calibri";
    //                sht.Range("A6").Style.Font.FontSize = 10;
    //                sht.Range("A6").Style.Font.SetBold();
    //                sht.Range("A6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("B6").Value = ds.Tables[0].Rows[0]["CompanyPhoneNo"];
    //                sht.Range("B6").Style.Font.FontName = "Calibri";
    //                sht.Range("B6").Style.Font.FontSize = 10;
    //                //sht.Range("B6").Merge();
    //                sht.Range("B6").Style.Alignment.SetWrapText();
    //                sht.Range("B6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("B6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


    //                sht.Range("C2").Value = "Weaver Name & Address";
    //                sht.Range("C2:D3").Style.Font.FontName = "Calibri";
    //                sht.Range("C2:D3").Style.Font.FontSize = 11;
    //                sht.Range("C2:D3").Style.Font.SetBold();
    //                //sht.Range("A2").Merge();
    //                sht.Range("C2:D3").Merge();
    //                sht.Range("C2:D3").Style.Alignment.SetWrapText();
    //                sht.Range("C2:D3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("C2:D3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("E2").Value = ds.Tables[0].Rows[0]["EmpName"];
    //                sht.Range("E2:G2").Style.Font.FontName = "Calibri";
    //                sht.Range("E2:G2").Style.Font.FontSize = 11;
    //                //sht.Range("B2:D2").Style.Font.SetBold();
    //                sht.Range("E2:G2").Merge();
    //                sht.Range("E2:G2").Style.Alignment.SetWrapText();
    //                sht.Range("E2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
    //                //sht.Range("E2:G2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("E3").Value = ds.Tables[0].Rows[0]["Address"];
    //                sht.Range("E3:G3").Style.Font.FontName = "Calibri";
    //                sht.Range("E3:G3").Style.Font.FontSize = 11;
    //                //sht.Range("E2:N5").Style.Font.SetBold();
    //                sht.Range("E3:G3").Style.Alignment.SetWrapText();
    //                sht.Range("E3:G3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
    //                sht.Range("E3:G3").Merge();

    //                sht.Range("C4").Value = "Deliver To";
    //                sht.Range("C4:D4").Style.Font.FontName = "Calibri";
    //                sht.Range("C4:D4").Style.Font.FontSize = 11;
    //                sht.Range("C4:D4").Style.Font.SetBold();
    //                //sht.Range("A2").Merge();
    //                sht.Range("C4:D4").Merge();
    //                sht.Range("C4:D4").Style.Alignment.SetWrapText();
    //                sht.Range("C4:D4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("C4:D4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("E4").Value = ds.Tables[0].Rows[0]["EmpVendorName2"];
    //                sht.Range("E4:G4").Style.Font.FontName = "Calibri";
    //                sht.Range("E4:G4").Style.Font.FontSize = 11;
    //                //sht.Range("B2:D2").Style.Font.SetBold();
    //                sht.Range("E4:G4").Merge();
    //                sht.Range("E4:G4").Style.Alignment.SetWrapText();
    //                sht.Range("E4:G4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("E4:G4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("C5").Value = "Deliver To Address";
    //                sht.Range("C5:D6").Style.Font.FontName = "Calibri";
    //                sht.Range("C5:D6").Style.Font.FontSize = 11;
    //                sht.Range("C5:D6").Style.Font.SetBold();
    //                //sht.Range("A2").Merge();
    //                sht.Range("C5:D6").Merge();
    //                sht.Range("C5:D6").Style.Alignment.SetWrapText();
    //                sht.Range("C5:D6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("C5:D6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("E5").Value = ds.Tables[0].Rows[0]["EmpVendorAddress2"];
    //                sht.Range("E5:G6").Style.Font.FontName = "Calibri";
    //                sht.Range("E5:G6").Style.Font.FontSize = 11;
    //                //sht.Range("B2:D2").Style.Font.SetBold();
    //                sht.Range("E5:G6").Merge();
    //                sht.Range("E5:G6").Style.Alignment.SetWrapText();
    //                sht.Range("E5:G6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("E5:G6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("H2").Value = "Pan No";
    //                sht.Range("H2:I2").Style.Font.FontName = "Calibri";
    //                sht.Range("H2:I2").Style.Font.FontSize = 11;
    //                sht.Range("H2:I2").Style.Font.SetBold();
    //                //sht.Range("A2").Merge();
    //                sht.Range("H2:I2").Merge();
    //                sht.Range("H2:I2").Style.Alignment.SetWrapText();
    //                sht.Range("H2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("H2:I2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("J2").Value = ds.Tables[0].Rows[0]["EmpPanNo"];
    //                sht.Range("J2:J2").Style.Font.FontName = "Calibri";
    //                sht.Range("J2:J2").Style.Font.FontSize = 11;
    //                //sht.Range("B2:D2").Style.Font.SetBold();
    //                sht.Range("J2:J2").Merge();
    //                sht.Range("J2:J2").Style.Alignment.SetWrapText();
    //                sht.Range("J2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("J2:J2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("H3").Value = "Aadhar No";
    //                sht.Range("H3:I3").Style.Font.FontName = "Calibri";
    //                sht.Range("H3:I3").Style.Font.FontSize = 11;
    //                sht.Range("H3:I3").Style.Font.SetBold();
    //                //sht.Range("A2").Merge();
    //                sht.Range("H3:I3").Merge();
    //                sht.Range("H3:I3").Style.Alignment.SetWrapText();
    //                sht.Range("H3:I3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("H3:I3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("J3").Value = ds.Tables[0].Rows[0]["EmpAadharNo"];
    //                sht.Range("J3:J3").Style.Font.FontName = "Calibri";
    //                sht.Range("J3:J3").Style.Font.FontSize = 11;
    //                sht.Range("J3:J3").Style.NumberFormat.NumberFormatId = 1;
    //                //sht.Range("B2:D2").Style.Font.SetBold();
    //                sht.Range("J3:J3").Merge();
    //                sht.Range("J3:J3").Style.Alignment.SetWrapText();
    //                sht.Range("J3:J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("J3:J3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


    //                sht.Range("H4").Value = "Mobile No";
    //                sht.Range("H4:I4").Style.Font.FontName = "Calibri";
    //                sht.Range("H4:I4").Style.Font.FontSize = 11;
    //                sht.Range("H4:I4").Style.Font.SetBold();
    //                //sht.Range("A2").Merge();
    //                sht.Range("H4:I4").Merge();
    //                sht.Range("H4:I4").Style.Alignment.SetWrapText();
    //                sht.Range("H4:I4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("H4:I4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("J4").Value = ds.Tables[0].Rows[0]["EmpMobileNo"];
    //                sht.Range("J4:J4").Style.Font.FontName = "Calibri";
    //                sht.Range("J4:J4").Style.Font.FontSize = 11;
    //                //sht.Range("B2:D2").Style.Font.SetBold();
    //                sht.Range("J4:J4").Merge();
    //                sht.Range("J4:J4").Style.Alignment.SetWrapText();
    //                sht.Range("J4:J4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("J4:J4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


    //                sht.Range("K2:L2").Value = "FOLIO NO.";
    //                sht.Range("K2:L2").Style.Font.FontName = "Calibri";
    //                sht.Range("K2:L2").Style.Font.FontSize = 11;
    //                sht.Range("K2:L2").Style.Font.SetBold();
    //                sht.Range("K2:L2").Merge();
    //                sht.Range("K2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("K2:L2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("M2").Value = ds.Tables[0].Rows[0]["IssueOrderId"];
    //                sht.Range("M2:N2").Style.Font.FontName = "Calibri";
    //                sht.Range("M2:N2").Style.Font.FontSize = 11;
    //                //sht.Range("Q2:R2").Style.Font.SetBold();
    //                sht.Range("M2:N2").Merge();
    //                sht.Range("M2:N2").Style.Alignment.SetWrapText();
    //                sht.Range("M2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("M2:N2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


    //                sht.Range("K3").Value = "SR.NO";
    //                sht.Range("K3:L3").Style.Font.FontName = "Calibri";
    //                sht.Range("K3:L3").Style.Font.FontSize = 11;
    //                sht.Range("K3:L3").Style.Font.SetBold();
    //                sht.Range("K3:L3").Merge();
    //                sht.Range("K3:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("K3:L3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("M3").Value = ds.Tables[0].Rows[0]["LocalOrder"];
    //                sht.Range("M3:N3").Style.Font.FontName = "Calibri";
    //                sht.Range("M3:N3").Style.Font.FontSize = 11;
    //                //sht.Range("Q3:R3").Style.Font.SetBold();
    //                sht.Range("M3:N3").Merge();
    //                sht.Range("M3:N3").Style.Alignment.SetWrapText();
    //                sht.Range("M3:N3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("M3:N3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


    //                sht.Range("K4").Value = "ORDER DATE";
    //                sht.Range("K4:L4").Style.Font.FontName = "Calibri";
    //                sht.Range("K4:L4").Style.Font.FontSize = 11;
    //                sht.Range("K4:L4").Style.Font.SetBold();
    //                sht.Range("K4:L4").Merge();
    //                sht.Range("K4:L4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("K4:L4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("M4").Value = ds.Tables[0].Rows[0]["OrderDate"];
    //                sht.Range("M4:N4").Style.Font.FontName = "Calibri";
    //                sht.Range("M4:N4").Style.Font.FontSize = 11;
    //                //sht.Range("Q5:R5").Style.Font.SetBold();
    //                sht.Range("M4:N4").Merge();
    //                sht.Range("M4:N4").Style.Alignment.SetWrapText();
    //                sht.Range("M4:N4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("M4:N4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("K5").Value = "DELIVERY DATE";
    //                sht.Range("K5:L5").Style.Font.FontName = "Calibri";
    //                sht.Range("K5:L5").Style.Font.FontSize = 11;
    //                sht.Range("K5:L5").Style.Font.SetBold();
    //                sht.Range("K5:L5").Merge();
    //                sht.Range("K5:L5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("K5:L5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("M5").Value = ds.Tables[0].Rows[0]["DispatchDate"];
    //                sht.Range("M5:N5").Style.Font.FontName = "Calibri";
    //                sht.Range("M5:N5").Style.Font.FontSize = 11;
    //                //sht.Range("Q6:R6").Style.Font.SetBold();
    //                sht.Range("M5:N5").Merge();
    //                sht.Range("M5:N5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("M5:N5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


    //                sht.Range("A7").Value = "Quality (Weight)";
    //                sht.Range("A7:A7").Style.Font.FontName = "Calibri";
    //                sht.Range("A7:A7").Style.Font.FontSize = 11;
    //                sht.Range("A7:A7").Style.Font.SetBold();
    //                sht.Range("A7:A7").Merge();
    //                sht.Range("A7:A7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("A7:A7").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


    //                sht.Range("A8").Value = ds.Tables[0].Rows[0]["QualityGrmPerMeterMinus"];
    //                sht.Range("A8:A10").Style.Font.FontName = "Calibri";
    //                sht.Range("A8:A10").Style.Font.FontSize = 11;
    //                //sht.Range("B6:D6").Style.Font.SetBold();
    //                sht.Range("A8:A10").Merge();
    //                sht.Range("A8:A10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("A8:A10").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("B7").Value = "Quality (Product)";
    //                sht.Range("B7:D7").Style.Font.FontName = "Calibri";
    //                sht.Range("B7:D7").Style.Font.FontSize = 11;
    //                sht.Range("B7:D7").Style.Font.SetBold();
    //                sht.Range("B7:D7").Merge();
    //                sht.Range("B7:D7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("B7:D7").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


    //                sht.Range("B8").Value = ds.Tables[0].Rows[0]["QUALITYNAME"];
    //                sht.Range("B8:D10").Style.Font.FontName = "Calibri";
    //                sht.Range("B8:D10").Style.Font.FontSize = 11;
    //                //sht.Range("B6:D6").Style.Font.SetBold();
    //                sht.Range("B8:D10").Merge();
    //                sht.Range("B8:D10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("B8:D10").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("E7").Value = "Date";
    //                sht.Range("E7").Style.Font.FontName = "Calibri";
    //                sht.Range("E7").Style.Font.FontSize = 11;
    //                sht.Range("E7").Style.Font.SetBold();

    //                if (ds.Tables[1].Rows.Count > 0)
    //                {
    //                    int row2 = 7;
    //                    int noofrows2 = 0;
    //                    int i2 = 0;
    //                    int Dynamiccol2 = 5;
    //                    int Dynamiccolstart2 = Dynamiccol2 + 1;
    //                    int Dynamiccolend2;
    //                    int Totalcol2;

    //                    DataTable dtdistinct2 = ds.Tables[1].DefaultView.ToTable(true, "ReceiveDate");
    //                    noofrows2 = dtdistinct2.Rows.Count;

    //                    for (i2 = 0; i2 < noofrows2; i2++)
    //                    {
    //                        Dynamiccol2 = Dynamiccol2 + 1;
    //                        sht.Cell(row2, Dynamiccol2).Value = dtdistinct2.Rows[i2]["ReceiveDate"].ToString();
    //                    }
    //                    Dynamiccolend2 = Dynamiccol2;
    //                    //Totalcol2 = Dynamiccolend2 + 1;
    //                    //sht.Cell(row, Totalcol).Value = "Total";
    //                    //sht.Cell(row, Totalcol).Style.Font.Bold = true;

    //                    row2 = row2 + 1;

    //                    for (int k = Dynamiccolstart2; k <= Dynamiccolend2; k++)
    //                    {
    //                        var itemname = sht.Cell(7, k).Value;
    //                        decimal WeightQty = 0;
    //                        decimal Penality = 0;
    //                        string ProcessRecId = "";

    //                        DataRow[] foundRows;
    //                        foundRows = ds.Tables[1].Select("ReceiveDate='" + itemname + "' ");
    //                        if (foundRows.Length > 0)
    //                        {
    //                            foreach (DataRow row3 in foundRows)
    //                            {
    //                                ProcessRecId = row3["ProcessRecId"].ToString();
    //                                break;
    //                            }

    //                            WeightQty = Convert.ToDecimal(ds.Tables[1].Compute("sum(Weight)", "ReceiveDate='" + itemname + "' "));
    //                            Penality = Convert.ToDecimal(ds.Tables[1].Compute("sum(Penality)", "ReceiveDate='" + itemname + "' "));
    //                        }
    //                        sht.Cell(row2, k).SetValue(ProcessRecId);
    //                        sht.Cell(row2, k).Style.Alignment.SetWrapText();
    //                        //sht.Cell(row2, k).Style.NumberFormat.NumberFormatId = 1;                            
    //                        //sht.Cell(row2, k).Style.Alignment.WrapText = true;
    //                        //sht.Cell(row2, k).Comment.Style.Alignment.SetAutomaticSize();                            
    //                        sht.Cell(9, k).Value = WeightQty;
    //                        sht.Cell(10, k).Value = Penality;
    //                    }
    //                }


    //                sht.Range("E8").Value = "Slip No.";
    //                sht.Range("E8").Style.Font.FontName = "Calibri";
    //                sht.Range("E8").Style.Font.FontSize = 11;
    //                sht.Range("E8").Style.Font.SetBold();

    //                sht.Range("E9").Value = "Weight ";
    //                sht.Range("E9").Style.Font.FontName = "Calibri";
    //                sht.Range("E9").Style.Font.FontSize = 11;
    //                sht.Range("E9").Style.Font.SetBold();

    //                sht.Range("E10").Value = "Deduction ";
    //                sht.Range("E10").Style.Font.FontName = "Calibri";
    //                sht.Range("E10").Style.Font.FontSize = 11;
    //                sht.Range("E10").Style.Font.SetBold();

    //                sht.Range("A11").Value = "Design";
    //                sht.Range("A11").Style.Font.FontName = "Calibri";
    //                sht.Range("A11").Style.Font.FontSize = 11;
    //                sht.Range("A11").Style.Font.SetBold();
    //                sht.Range("A11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("A11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("B11").Value = "Color ";
    //                sht.Range("B11").Style.Font.FontName = "Calibri";
    //                sht.Range("B11").Style.Font.FontSize = 11;
    //                sht.Range("B11").Style.Font.SetBold();
    //                sht.Range("B11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("B11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("C11").Value = "Rate/PC ";
    //                sht.Range("C11").Style.Font.FontName = "Calibri";
    //                sht.Range("C11").Style.Font.FontSize = 11;
    //                sht.Range("C11").Style.Font.SetBold();
    //                sht.Range("C11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("C11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("D11").Value = "Size(" + ds.Tables[0].Rows[0]["UnitName"] + ")";
    //                sht.Range("D11").Style.Font.FontName = "Calibri";
    //                sht.Range("D11").Style.Font.FontSize = 11;
    //                sht.Range("D11").Style.Font.SetBold();
    //                sht.Range("D11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("D11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("E11").Value = "Order Pcs";
    //                sht.Range("E11").Style.Font.FontName = "Calibri";
    //                sht.Range("E11").Style.Font.FontSize = 11;
    //                sht.Range("E11").Style.Font.SetBold();
    //                sht.Range("E11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("E11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                int row = 11;
    //                int column = 4;
    //                int noofrows = 0;
    //                int i = 0;
    //                int Dynamiccol = 5;
    //                int Dynamiccolstart = Dynamiccol + 1;
    //                int Dynamiccolend;
    //                int Totalcol;
    //                decimal Area = 0;
    //                decimal TotalArea = 0;

    //                DataTable dtdistinct = ds.Tables[1].DefaultView.ToTable(true, "ReceiveDate");
    //                noofrows = dtdistinct.Rows.Count;

    //                for (i = 0; i < noofrows; i++)
    //                {
    //                    Dynamiccol = Dynamiccol + 1;
    //                    sht.Cell(row, Dynamiccol).Value = dtdistinct.Rows[i]["ReceiveDate"].ToString();
    //                    sht.Cell(row, Dynamiccol).Style.Font.Bold = true;
    //                    sht.Cell(row, Dynamiccol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                    sht.Cell(row, Dynamiccol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                }
    //                Dynamiccolend = Dynamiccol;
    //                Totalcol = Dynamiccolend + 1;
    //                sht.Cell(row, Totalcol).Value = "Total ";
    //                sht.Cell(row, Totalcol).Style.Font.FontName = "Calibri";
    //                sht.Cell(row, Totalcol).Style.Font.FontSize = 11;
    //                sht.Cell(row, Totalcol).Style.Font.Bold = true;
    //                sht.Cell(row, Totalcol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Cell(row, Totalcol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);                  

    //                row = row + 1;

    //                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
    //                {

    //                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[j]["DesignName"]);
    //                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[j]["ColorName"]);
    //                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[j]["Rate"]);
    //                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[j]["Size"]);
    //                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[j]["OrderQty"]);

    //                    sht.Range("A" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                    sht.Range("A" + row + ":E" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                    Area = Convert.ToDecimal(ds.Tables[0].Rows[j]["Area"]);
    //                    TotalArea = TotalArea + Area;

    //                    decimal TotalSumOneRow = 0;
    //                    for (int k = Dynamiccolstart; k <= Dynamiccolend; k++)
    //                    {
    //                        var itemname = sht.Cell(11, k).Value;
    //                        decimal RecQty = 0;

    //                        DataRow[] foundRows;
    //                        foundRows = ds.Tables[1].Select("ReceiveDate='" + itemname + "' and Item_Finished_Id='" + ds.Tables[0].Rows[j]["Item_Finished_Id"] + "' ");
    //                        if (foundRows.Length > 0)
    //                        {
    //                            RecQty = Convert.ToDecimal(ds.Tables[1].Compute("sum(Pcs)", "ReceiveDate='" + itemname + "' and Item_Finished_Id='" + ds.Tables[0].Rows[j]["Item_Finished_Id"] + "' "));
    //                        }
    //                        //IssRecConQty = IssQty + RecQty + ConsQty;
    //                        TotalSumOneRow = TotalSumOneRow + RecQty;
    //                        sht.Cell(row, k).Value = RecQty;
    //                        sht.Cell(row, k).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                        sht.Cell(row, k).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                    }
    //                    Totalcol = Dynamiccolend + 1;
    //                    sht.Cell(row, Totalcol).Value = TotalSumOneRow;
    //                    sht.Cell(row, Totalcol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                    sht.Cell(row, Totalcol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                    sht.Row(row).Height = 30;
    //                    row = row + 1;


    //                }

    //                sht.Range("C" + row).Value = "Total";
    //                sht.Range("D" + row).Value = TotalArea;
    //                sht.Range("E" + row).FormulaA1 = "=SUM(E12" + ":$E$" + (row - 1) + ")";
    //                sht.Range("C" + row + ":E" + row).Style.Font.SetBold();
    //                sht.Range("C" + row + ":E" + row).Style.Font.FontName = "Calibri";
    //                sht.Range("C" + row + ":E" + row).Style.Font.FontSize = 11;
    //                row = row + 1;

    //                sht.Row(row).Height = 30;
    //                row = row + 1;


    //                sht.Range("A" + row + ":C" + (row + 1)).Merge();
    //                sht.Range("A" + row + ":C" + (row + 1)).Style.Font.FontName = "Calibri";
    //                sht.Range("A" + row + ":C" + (row + 1)).Style.Font.FontSize = 12;
    //                //sht.Range("B6:D6").Style.Font.SetBold();                   
    //                sht.Range("A" + row + ":C" + (row + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("A" + row).Value = "";

    //                sht.Range("D" + row + ":J" + (row + 1)).Merge();
    //                sht.Range("D" + row + ":J" + (row + 1)).Style.Font.FontName = "Calibri";
    //                sht.Range("D" + row + ":J" + (row + 1)).Style.Font.FontSize = 12;
    //                //sht.Range("B6:D6").Style.Font.SetBold();                   
    //                sht.Range("D" + row + ":J" + (row + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("D" + row).Value = "";


    //                sht.Range("K" + row + ":N" + (row + 1)).Merge();
    //                sht.Range("K" + row + ":N" + (row + 1)).Style.Font.FontName = "Calibri";
    //                sht.Range("K" + row + ":N" + (row + 1)).Style.Font.FontSize = 12;
    //                //sht.Range("B6:D6").Style.Font.SetBold();                   
    //                sht.Range("K" + row + ":N" + (row + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("K" + row).Value = "";

    //                sht.Row(row).Height = 30;
    //                row = row + 2;

    //                sht.Range("A" + row + ":C" + row).Value = "Prepared By";
    //                sht.Range("A" + row + ":C" + row).Style.Font.FontName = "Calibri";
    //                sht.Range("A" + row + ":C" + row).Style.Font.FontSize = 12;
    //                //sht.Range("B6:D6").Style.Font.SetBold();
    //                sht.Range("A" + row + ":C" + row).Merge();
    //                sht.Range("A" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

    //                sht.Range("K" + row + ":N" + row).Value = "Checked By";
    //                sht.Range("K" + row + ":N" + row).Style.Font.FontName = "Calibri";
    //                sht.Range("K" + row + ":N" + row).Style.Font.FontSize = 12;
    //                //sht.Range("B6:D6").Style.Font.SetBold();
    //                sht.Range("K" + row + ":N" + row).Merge();
    //                sht.Range("K" + row + ":N" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

    //                sht.Row(row).Height = 25;
    //                row = row + 1;

    //                //////*************
    //                // sht.Columns(1, 20).AdjustToContents();
    //                //********************
    //                //***********BOrders
    //                using (var a = sht.Range("A1" + ":N" + (row-1)))
    //                {
    //                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
    //                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //                }

    //                string Fileextension = "xlsx";
    //                string filename = UtilityModule.validateFilename("WeaverFolioReportHindi_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
    //                Path = Server.MapPath("~/Tempexcel/" + filename);
    //                xapp.SaveAs(Path);
    //                xapp.Dispose();
    //                //Download File
    //                Response.ClearContent();
    //                Response.ClearHeaders();
    //                // Response.Clear();
    //                Response.ContentType = "application/vnd.ms-excel";
    //                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
    //                Response.WriteFile(Path);
    //                // File.Delete(Path);
    //                Response.End();
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            lblmsg.Text = ex.Message;
    //        }

    //    }

    //}
    //protected void FolioMaterialDetailReportHafizia()
    //{
    //    DataSet ds = new DataSet();
    //    if (DDFolioNo.SelectedIndex > 0)
    //    {
    //        lblmsg.Text = "";
    //        try
    //        {

    //            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //            if (con.State == ConnectionState.Closed)
    //            {
    //                con.Open();
    //            }
    //            SqlCommand cmd = new SqlCommand("PRO_WEAVERFOLIO_MATERIAL_DETAIL_HAFIZIA_REPORT", con);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            cmd.CommandTimeout = 1000;

    //            cmd.Parameters.AddWithValue("@issueorderid", DDFolioNo.SelectedValue);
    //            //cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
    //            SqlDataAdapter ad = new SqlDataAdapter(cmd);
    //            cmd.ExecuteNonQuery();
    //            ad.Fill(ds);

    //            //SqlParameter[] param = new SqlParameter[1];
    //            //param[0] = new SqlParameter("@issueorderid", DDFolioNo.SelectedValue);

    //            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_WEAVERFOLIO_MATERIAL_DETAIL_HAFIZIA_REPORT", param);

    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
    //                {
    //                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
    //                }
    //                string Path = "";
    //                var xapp = new XLWorkbook();
    //                var sht = xapp.Worksheets.Add("FolioMaterialReport");
    //                //int row = 0;

    //                sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
    //                sht.PageSetup.AdjustTo(85);
    //                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;

    //                //
    //                sht.PageSetup.Margins.Top = 1.21;
    //                sht.PageSetup.Margins.Left = 0.47;
    //                sht.PageSetup.Margins.Right = 0.36;
    //                sht.PageSetup.Margins.Bottom = 0.19;
    //                sht.PageSetup.Margins.Header = 2.20;
    //                sht.PageSetup.Margins.Footer = 0.3;
    //                sht.PageSetup.SetScaleHFWithDocument();

    //                //sht.Column("A").Width = 22.78;
    //                //sht.Column("B").Width = 18.33;
    //                //sht.Column("C").Width = 11.67;
    //                //sht.Column("D").Width = 8.22;
    //                //sht.Column("E").Width = 10.67;
    //                //sht.Column("F").Width = 10.33;
    //                //sht.Column("G").Width = 10.33;
    //                //sht.Column("H").Width = 10.33;
    //                //sht.Column("I").Width = 10.33;
    //                //sht.Column("J").Width = 10.33;
    //                //sht.Column("K").Width = 10.33;
    //                //sht.Column("L").Width = 10.33;
    //                //sht.Column("M").Width = 10.33;
    //                //sht.Column("N").Width = 10.33;
    //                //sht.Column("O").Width = 10.33;
    //                //sht.Column("P").Width = 10.33;
    //                //sht.Column("Q").Width = 10.33;
    //                //sht.Column("R").Width = 10.33;

    //                sht.Column("A").Width = 15.67;
    //                sht.Column("B").Width = 15.22;
    //                sht.Column("C").Width = 7.78;
    //                sht.Column("D").Width = 8.22;
    //                sht.Column("E").Width = 9.22;
    //                sht.Column("F").Width = 10.89;
    //                sht.Column("G").Width = 11.22;
    //                sht.Column("H").Width = 9.89;
    //                sht.Column("I").Width = 9.89;
    //                sht.Column("J").Width = 14.33;
    //                sht.Column("K").Width = 9.89;
    //                sht.Column("L").Width = 9.89;
    //                sht.Column("M").Width = 9.89;
    //                sht.Column("N").Width = 9.89;


    //                sht.Row(1).Height = 35;
    //                sht.Row(2).Height = 30;
    //                sht.Row(3).Height = 30;
    //                sht.Row(4).Height = 30;
    //                sht.Row(5).Height = 30;
    //                sht.Row(6).Height = 30;
    //                sht.Row(7).Height = 30;
    //                sht.Row(8).Height = 30;
    //                sht.Row(9).Height = 30;
    //                sht.Row(10).Height = 30;
    //                sht.Row(11).Height = 30;

    //                // sht.Row(1).Height = 13.80;

    //                sht.Range("A1:N1").Merge();
    //                sht.Range("A1").Value = "RAW MATERIAL ISSUE (JOB WORK)";

    //                sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("A1:N1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
    //                sht.Range("A1:N1").Style.Alignment.SetWrapText();
    //                sht.Range("A1:N1").Style.Font.FontName = "Calibri";
    //                sht.Range("A1:N1").Style.Font.FontSize = 18;
    //                sht.Range("A1:N1").Style.Font.Bold = true;
    //                //*******Header

    //                sht.Range("A2").Value = ds.Tables[0].Rows[0]["CompanyName"];
    //                sht.Range("A2:B2").Style.Font.FontName = "Calibri";
    //                sht.Range("A2:B2").Style.Font.FontSize = 14;
    //                sht.Range("A2:B2").Style.Font.SetBold();
    //                sht.Range("A2:B2").Merge();
    //                sht.Range("A2:B2").Style.Alignment.SetWrapText();
    //                sht.Range("A2:B2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
    //                sht.Range("A2:B2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


    //                sht.Range("A3").Value = ds.Tables[0].Rows[0]["COMPADDR1"] + " " + ds.Tables[0].Rows[0]["COMPADDR2"];
    //                sht.Range("A3:B4").Style.Font.FontName = "Calibri";
    //                sht.Range("A3:B4").Style.Font.FontSize = 11;
    //                sht.Range("A3:B4").Style.Font.SetBold();
    //                sht.Range("A3:B4").Merge();
    //                sht.Range("A3:B4").Style.Alignment.SetWrapText();
    //                sht.Range("A3:B4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
    //                sht.Range("A3:B4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("A5").Value = "GSTIN";
    //                sht.Range("A5").Style.Font.FontName = "Calibri";
    //                sht.Range("A5").Style.Font.FontSize = 10;
    //                sht.Range("A5").Style.Font.SetBold();

    //                sht.Range("B5").Value = ds.Tables[0].Rows[0]["CompanyGstNo"];
    //                sht.Range("B5:B5").Style.Font.FontName = "Calibri";
    //                sht.Range("B5:B5").Style.Font.FontSize = 10;
    //                //sht.Range("B5:D5").Style.Font.SetBold();
    //                sht.Range("B5:B5").Merge();
    //                sht.Range("B5:B5").Style.Alignment.SetWrapText();
    //                sht.Range("B5:B5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

    //                sht.Range("A6").Value = "Phone No";
    //                sht.Range("A6").Style.Font.FontName = "Calibri";
    //                sht.Range("A6").Style.Font.FontSize = 10;
    //                sht.Range("A6").Style.Font.SetBold();
    //                sht.Range("A6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("B6").Value = ds.Tables[0].Rows[0]["CompanyPhoneNo"];
    //                sht.Range("B6:B6").Style.Font.FontName = "Calibri";
    //                sht.Range("B6:B6").Style.Font.FontSize = 10;
    //                //sht.Range("B5:D5").Style.Font.SetBold();
    //                sht.Range("B6:B6").Merge();
    //                sht.Range("B6:B6").Style.Alignment.SetWrapText();
    //                sht.Range("B6:B6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("B6:B6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


    //                sht.Range("C2").Value = "Weaver Name & Address";
    //                sht.Range("C2:D3").Style.Font.FontName = "Calibri";
    //                sht.Range("C2:D3").Style.Font.FontSize = 11;
    //                sht.Range("C2:D3").Style.Font.SetBold();
    //                //sht.Range("A2").Merge();
    //                sht.Range("C2:D3").Merge();
    //                sht.Range("C2:D3").Style.Alignment.SetWrapText();
    //                sht.Range("C2:D3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("C2:D3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("E2").Value = ds.Tables[0].Rows[0]["EmpName"];
    //                sht.Range("E2:G2").Style.Font.FontName = "Calibri";
    //                sht.Range("E2:G2").Style.Font.FontSize = 11;
    //                //sht.Range("B2:D2").Style.Font.SetBold();
    //                sht.Range("E2:G2").Merge();
    //                sht.Range("E2:G2").Style.Alignment.SetWrapText();
    //                sht.Range("E2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
    //                //sht.Range("E2:G2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("E3").Value = ds.Tables[0].Rows[0]["Address"];
    //                sht.Range("E3:G3").Style.Font.FontName = "Calibri";
    //                sht.Range("E3:G3").Style.Font.FontSize = 11;
    //                //sht.Range("E2:N5").Style.Font.SetBold();
    //                sht.Range("E3:G3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
    //                sht.Range("E3:G3").Merge();
    //                sht.Range("E2:G3").Style.Alignment.SetWrapText();

    //                sht.Range("C4").Value = "Deliver To";
    //                sht.Range("C4:D4").Style.Font.FontName = "Calibri";
    //                sht.Range("C4:D4").Style.Font.FontSize = 11;
    //                sht.Range("C4:D4").Style.Font.SetBold();
    //                //sht.Range("A2").Merge();
    //                sht.Range("C4:D4").Merge();
    //                sht.Range("C4:D4").Style.Alignment.SetWrapText();
    //                sht.Range("C4:D4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("C4:D4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("E4").Value = ds.Tables[0].Rows[0]["EmpVendorName2"];
    //                sht.Range("E4:G4").Style.Font.FontName = "Calibri";
    //                sht.Range("E4:G4").Style.Font.FontSize = 11;
    //                //sht.Range("B2:D2").Style.Font.SetBold();
    //                sht.Range("E4:G4").Merge();
    //                sht.Range("E4:G4").Style.Alignment.SetWrapText();
    //                sht.Range("E4:G4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("E4:G4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("C5").Value = "Deliver To Address";
    //                sht.Range("C5:D6").Style.Font.FontName = "Calibri";
    //                sht.Range("C5:D6").Style.Font.FontSize = 11;
    //                sht.Range("C5:D6").Style.Font.SetBold();
    //                //sht.Range("A2").Merge();
    //                sht.Range("C5:D6").Merge();
    //                sht.Range("C5:D6").Style.Alignment.SetWrapText();
    //                sht.Range("C5:D6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("C5:D6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("E5").Value = ds.Tables[0].Rows[0]["EmpVendorAddress2"];
    //                sht.Range("E5:G6").Style.Font.FontName = "Calibri";
    //                sht.Range("E5:G6").Style.Font.FontSize = 11;
    //                //sht.Range("B2:D2").Style.Font.SetBold();
    //                sht.Range("E5:G6").Merge();
    //                sht.Range("E5:G6").Style.Alignment.SetWrapText();
    //                sht.Range("E5:G6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("E5:G6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("H2").Value = "Pan No";
    //                sht.Range("H2:I2").Style.Font.FontName = "Calibri";
    //                sht.Range("H2:I2").Style.Font.FontSize = 11;
    //                sht.Range("H2:I2").Style.Font.SetBold();
    //                //sht.Range("A2").Merge();
    //                sht.Range("H2:I2").Merge();
    //                sht.Range("H2:I2").Style.Alignment.SetWrapText();
    //                sht.Range("H2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("H2:I2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("J2").Value = ds.Tables[0].Rows[0]["EmpPanNo"];
    //                sht.Range("J2:J2").Style.Font.FontName = "Calibri";
    //                sht.Range("J2:J2").Style.Font.FontSize = 11;
    //                //sht.Range("B2:D2").Style.Font.SetBold();
    //                sht.Range("J2:J2").Merge();
    //                sht.Range("J2:J2").Style.Alignment.SetWrapText();
    //                sht.Range("J2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("J2:J2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("H3").Value = "Aadhar No";
    //                sht.Range("H3:I3").Style.Font.FontName = "Calibri";
    //                sht.Range("H3:I3").Style.Font.FontSize = 11;
    //                sht.Range("H3:I3").Style.Font.SetBold();
    //                //sht.Range("A2").Merge();
    //                sht.Range("H3:I3").Merge();
    //                sht.Range("H3:I3").Style.Alignment.SetWrapText();
    //                sht.Range("H3:I3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("H3:I3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("J3").Value = ds.Tables[0].Rows[0]["EmpAadharNo"];
    //                sht.Range("J3:J3").Style.Font.FontName = "Calibri";
    //                sht.Range("J3:J3").Style.Font.FontSize = 11;
    //                sht.Range("J3:J3").Style.NumberFormat.NumberFormatId = 1;
    //                //sht.Range("B2:D2").Style.Font.SetBold();
    //                sht.Range("J3:J3").Merge();
    //                sht.Range("J3:J3").Style.Alignment.SetWrapText();
    //                sht.Range("J3:J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("J3:J3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


    //                sht.Range("H4").Value = "Mobile No";
    //                sht.Range("H4:I4").Style.Font.FontName = "Calibri";
    //                sht.Range("H4:I4").Style.Font.FontSize = 11;
    //                sht.Range("H4:I4").Style.Font.SetBold();
    //                //sht.Range("A2").Merge();
    //                sht.Range("H4:I4").Merge();
    //                sht.Range("H4:I4").Style.Alignment.SetWrapText();
    //                sht.Range("H4:I4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("H4:I4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("J4").Value = ds.Tables[0].Rows[0]["EmpMobileNo"];
    //                sht.Range("J4:J4").Style.Font.FontName = "Calibri";
    //                sht.Range("J4:J4").Style.Font.FontSize = 11;
    //                //sht.Range("B2:D2").Style.Font.SetBold();
    //                sht.Range("J4:J4").Merge();
    //                sht.Range("J4:J4").Style.Alignment.SetWrapText();
    //                sht.Range("J4:J4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("J4:J4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


    //                sht.Range("K2:L2").Value = "FOLIO NO.";
    //                sht.Range("K2:L2").Style.Font.FontName = "Calibri";
    //                sht.Range("K2:L2").Style.Font.FontSize = 11;
    //                sht.Range("K2:L2").Style.Font.SetBold();
    //                sht.Range("K2:L2").Merge();
    //                sht.Range("K2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("K2:L2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("M2").Value = ds.Tables[0].Rows[0]["IssueOrderId"];
    //                sht.Range("M2:N2").Style.Font.FontName = "Calibri";
    //                sht.Range("M2:N2").Style.Font.FontSize = 11;
    //                //sht.Range("Q2:R2").Style.Font.SetBold();
    //                sht.Range("M2:N2").Merge();
    //                sht.Range("M2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("M2:N2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("K3").Value = "SR.NO";
    //                sht.Range("K3:L3").Style.Font.FontName = "Calibri";
    //                sht.Range("K3:L3").Style.Font.FontSize = 11;
    //                sht.Range("K3:L3").Style.Font.SetBold();
    //                sht.Range("K3:L3").Merge();
    //                sht.Range("K3:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("K3:L3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("M3").Value = ds.Tables[0].Rows[0]["LocalOrder"];
    //                sht.Range("M3:N3").Style.Font.FontName = "Calibri";
    //                sht.Range("M3:N3").Style.Font.FontSize = 11;
    //                //sht.Range("Q3:R3").Style.Font.SetBold();
    //                sht.Range("M3:N3").Merge();
    //                sht.Range("M3:N3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("M3:N3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("K4").Value = "ORDER DATE";
    //                sht.Range("K4:L4").Style.Font.FontName = "Calibri";
    //                sht.Range("K4:L4").Style.Font.FontSize = 11;
    //                sht.Range("K4:L4").Style.Font.SetBold();
    //                sht.Range("K4:L4").Merge();
    //                sht.Range("K4:L4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("K4:L4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("M4").Value = ds.Tables[0].Rows[0]["OrderDate"];
    //                sht.Range("M4:N4").Style.Font.FontName = "Calibri";
    //                sht.Range("M4:N4").Style.Font.FontSize = 11;
    //                //sht.Range("Q5:R5").Style.Font.SetBold();
    //                sht.Range("M4:N4").Merge();
    //                sht.Range("M4:N4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("M4:N4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


    //                sht.Range("K5").Value = "DELIVERY DATE";
    //                sht.Range("K5:L5").Style.Font.FontName = "Calibri";
    //                sht.Range("K5:L5").Style.Font.FontSize = 11;
    //                sht.Range("K5:L5").Style.Font.SetBold();
    //                sht.Range("K5:L5").Merge();
    //                sht.Range("K5:L5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("K5:L5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("M5").Value = ds.Tables[0].Rows[0]["DispatchDate"];
    //                sht.Range("M5:N5").Style.Font.FontName = "Calibri";
    //                sht.Range("M5:N5").Style.Font.FontSize = 11;
    //                //sht.Range("Q6:R6").Style.Font.SetBold();
    //                sht.Range("M5:N5").Merge();
    //                sht.Range("M5:N5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("M5:N5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


    //                sht.Range("A7").Value = "Material Detail";
    //                sht.Range("A7:D10").Style.Font.FontName = "Calibri";
    //                sht.Range("A7:D10").Style.Font.FontSize = 11;
    //                //sht.Range("B6:D6").Style.Font.SetBold();
    //                sht.Range("A7:D10").Merge();
    //                sht.Range("A7:D10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("A7:D10").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("E7").Value = "Date";
    //                sht.Range("E7").Style.Font.FontName = "Calibri";
    //                sht.Range("E7").Style.Font.FontSize = 11;
    //                sht.Range("E7").Style.Font.SetBold();

    //                if (ds.Tables[1].Rows.Count > 0)
    //                {
    //                    int row2 = 7;
    //                    int noofrows2 = 0;
    //                    int i2 = 0;
    //                    int Dynamiccol2 = 5;
    //                    int Dynamiccolstart2 = Dynamiccol2 + 1;
    //                    int Dynamiccolend2;
    //                    int Totalcol2;

    //                    DataTable dtdistinct2 = ds.Tables[1].DefaultView.ToTable(true, "Date");
    //                    noofrows2 = dtdistinct2.Rows.Count;

    //                    for (i2 = 0; i2 < noofrows2; i2++)
    //                    {
    //                        Dynamiccol2 = Dynamiccol2 + 1;
    //                        sht.Cell(row2, Dynamiccol2).Value = dtdistinct2.Rows[i2]["Date"].ToString();
    //                    }
    //                    Dynamiccolend2 = Dynamiccol2;
    //                    //Totalcol2 = Dynamiccolend2 + 1;
    //                    //sht.Cell(row, Totalcol).Value = "Total";
    //                    //sht.Cell(row, Totalcol).Style.Font.Bold = true;

    //                    row2 = row2 + 1;

    //                    for (int k = Dynamiccolstart2; k <= Dynamiccolend2; k++)
    //                    {
    //                        var itemname = sht.Cell(7, k).Value;
    //                        string ChallanNo = "";

    //                        DataRow[] foundRows;
    //                        foundRows = ds.Tables[1].Select("Date='" + itemname + "' ");
    //                        if (foundRows.Length > 0)
    //                        {
    //                            foreach (DataRow row3 in foundRows)
    //                            {
    //                                ChallanNo = row3["ChallanNo"].ToString();
    //                                break;
    //                            }
    //                        }
    //                        sht.Cell(row2, k).SetValue(ChallanNo);
    //                        sht.Cell(row2, k).Style.Alignment.SetWrapText();
    //                        //sht.Cell(row2, k).Style.NumberFormat.NumberFormatId = 1;                            
    //                        //sht.Cell(row2, k).Style.Alignment.WrapText = true;
    //                        //sht.Cell(row2, k).Comment.Style.Alignment.SetAutomaticSize();                            

    //                    }
    //                }

    //                sht.Range("E8").Value = "Slip No";
    //                sht.Range("E8").Style.Font.FontName = "Calibri";
    //                sht.Range("E8").Style.Font.FontSize = 11;
    //                sht.Range("E8").Style.Font.SetBold();

    //                sht.Range("E9").Value = "Lagat ";
    //                sht.Range("E9:E10").Style.Font.FontName = "Calibri";
    //                sht.Range("E9:E10").Style.Font.FontSize = 11;
    //                sht.Range("E9:E10").Style.Font.SetBold();
    //                sht.Range("E9:E10").Merge();
    //                sht.Range("E9").Style.Alignment.SetWrapText();
    //                sht.Range("E9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Row(9).Height = 30;


    //                sht.Range("A11").Value = "Yarn Detail & Count Ply ";
    //                sht.Range("A11").Style.Font.FontName = "Calibri";
    //                sht.Range("A11").Style.Font.FontSize = 11;
    //                sht.Range("A11").Style.Font.SetBold();
    //                sht.Range("A11").Merge();
    //                sht.Range("A11").Style.Alignment.SetWrapText();
    //                sht.Range("A11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("A11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("B11").Value = "Shade Color";
    //                sht.Range("B11").Style.Font.FontName = "Calibri";
    //                sht.Range("B11").Style.Font.FontSize = 11;
    //                sht.Range("B11").Style.Font.SetBold();
    //                sht.Range("B11").Merge();
    //                sht.Range("B11").Style.Alignment.SetWrapText();
    //                sht.Range("B11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("B11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("C11").Value = "Lot No";
    //                sht.Range("C11").Style.Font.FontName = "Calibri";
    //                sht.Range("C11").Style.Font.FontSize = 11;
    //                sht.Range("C11").Style.Font.SetBold();
    //                sht.Range("C11").Merge();
    //                sht.Range("C11").Style.Alignment.SetWrapText();
    //                sht.Range("C11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("C11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Range("D11").Value = "WT 10%";
    //                sht.Range("D11").Style.Font.FontName = "Calibri";
    //                sht.Range("D11").Style.Font.FontSize = 11;
    //                sht.Range("D11").Style.Font.SetBold();
    //                sht.Range("D11").Merge();
    //                sht.Range("D11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Range("D11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


    //                int row = 11;
    //                int noofrows = 0;
    //                int i = 0;
    //                int Dynamiccol = 5;
    //                int Dynamiccolstart = Dynamiccol + 1;
    //                int Dynamiccolend;
    //                int Totalcol;

    //                DataTable dtdistinct = ds.Tables[1].DefaultView.ToTable(true, "Date", "Type");
    //                //DataView dv3 = new DataView(dtdistinct);
    //                //dv3.RowFilter = "Type='ISSUE'";
    //                //dv3.Sort = "Date";
    //                //DataSet ds3 = new DataSet();
    //                //ds3.Tables.Add(dv3.ToTable());
    //                noofrows = dtdistinct.Rows.Count;

    //                for (i = 0; i < noofrows; i++)
    //                {
    //                    Dynamiccol = Dynamiccol + 1;
    //                    sht.Cell(row, Dynamiccol).Value = dtdistinct.Rows[i]["Date"].ToString();
    //                    sht.Cell(row, Dynamiccol).Style.Font.Bold = true;
    //                    sht.Cell(row, Dynamiccol).Style.Font.FontName = "Calibri";
    //                    sht.Cell(row, Dynamiccol).Style.Font.FontSize = 11;
    //                    sht.Cell(row, Dynamiccol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                    sht.Cell(row, Dynamiccol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                }
    //                Dynamiccolend = Dynamiccol;
    //                Totalcol = Dynamiccolend + 1;
    //                sht.Cell(row, Totalcol).Value = "Total Issue";
    //                sht.Cell(row, Totalcol).Style.Font.FontName = "Calibri";
    //                sht.Cell(row, Totalcol).Style.Font.FontSize = 11;
    //                sht.Cell(row, Totalcol).Style.Font.Bold = true;
    //                sht.Cell(row, Totalcol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Cell(row, Totalcol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Cell(row, Totalcol + 1).Value = "Total Receive";
    //                sht.Cell(row, Totalcol + 1).Style.Font.FontName = "Calibri";
    //                sht.Cell(row, Totalcol + 1).Style.Font.FontSize = 11;
    //                sht.Cell(row, Totalcol + 1).Style.Font.Bold = true;
    //                sht.Cell(row, Totalcol + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                sht.Cell(row, Totalcol + 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                sht.Row(row).Height = 30;
    //                row = row + 1;

    //                DataTable dtdistinctDateType = ds.Tables[0].DefaultView.ToTable(true, "Item_Name", "QualityName", "DesignName", "ShadeColorName", "FinishedId");
    //                DataView dv1 = new DataView(dtdistinctDateType);
    //                dv1.Sort = "Item_Name";
    //                DataTable dtdistinctDateType1 = dv1.ToTable();

    //                foreach (DataRow dr in dtdistinctDateType1.Rows)
    //                {
    //                    decimal RecQty = 0;

    //                    //sht.Range("A" + row + ":D" + row).Merge();
    //                    //sht.Range("A" + row + ":D" + row).SetValue(dr["Item_Name"] + " " + dr["QualityName"] + " " + dr["DesignName"] + " " + dr["ShadeColorName"]);

    //                    sht.Range("A" + row).SetValue(dr["Item_Name"] + " " + dr["QualityName"]);
    //                    sht.Range("A" + row).Style.Alignment.SetWrapText();
    //                    sht.Range("B" + row).SetValue(dr["ShadeColorName"]);
    //                    sht.Range("C" + row).SetValue("");
    //                    sht.Range("D" + row).SetValue("");
    //                    sht.Range("E" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Compute("sum(Qty)", "Item_Name='" + dr["Item_Name"] + "' and QualityName='" + dr["QualityName"] + "' and DesignName='" + dr["DesignName"] + "' and ShadeColorName='" + dr["ShadeColorName"] + "' ")));

    //                    sht.Range("A" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                    sht.Range("A" + row + ":E" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                    DataTable dtdistinctDateType2 = ds.Tables[1].DefaultView.ToTable(true, "Date", "Type");
    //                    DataView dv2 = new DataView(dtdistinctDateType2);
    //                    dv2.Sort = "Date";
    //                    DataTable dtdistinctDateType3 = dv2.ToTable();
    //                    foreach (DataRow dr2 in dtdistinctDateType3.Rows)
    //                    {
    //                        //sht.Range("F" + row).SetValue(dr2["Type"]);

    //                        decimal TotalSumOneRow = 0;
    //                        for (int k = Dynamiccolstart; k <= Dynamiccolend; k++)
    //                        {
    //                            var itemname = sht.Cell(11, k).Value;
    //                            decimal IssQty = 0;

    //                            decimal IssRecConQty = 0;

    //                            DataRow[] foundRows;
    //                            foundRows = ds.Tables[1].Select("Date='" + itemname + "' and Type='" + dr2["Type"] + "' and FinishedId='" + dr["FinishedId"] + "' ");
    //                            if (foundRows.Length > 0)
    //                            {
    //                                if (dr2["Type"].ToString() == "ISSUE")
    //                                {
    //                                    IssQty = Convert.ToDecimal(ds.Tables[1].Compute("sum(IssueQty)", "Date='" + itemname + "' and Type='" + dr2["Type"] + "' and FinishedId='" + dr["FinishedId"] + "' "));
    //                                }
    //                                else if (dr2["Type"].ToString() == "RECEIVE")
    //                                {
    //                                    RecQty = Convert.ToDecimal(ds.Tables[1].Compute("sum(RecQty)", "Type='RECEIVE' and FinishedId='" + dr["FinishedId"] + "' "));
    //                                }
    //                            }

    //                            IssRecConQty = IssQty;
    //                            TotalSumOneRow = TotalSumOneRow + IssRecConQty;
    //                            sht.Cell(row, k).Value = IssRecConQty;
    //                            sht.Cell(row, k).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                            sht.Cell(row, k).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


    //                        }
    //                        Totalcol = Dynamiccolend + 1;
    //                        sht.Cell(row, Totalcol).Value = TotalSumOneRow;
    //                        sht.Cell(row, Totalcol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                        sht.Cell(row, Totalcol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


    //                        sht.Cell(row, Totalcol + 1).Value = RecQty;
    //                        sht.Cell(row, Totalcol + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                        sht.Cell(row, Totalcol + 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    //                    }
    //                    sht.Row(row).Height = 30;
    //                    row = row + 1;

    //                }
    //                sht.Row(row).Height = 30;
    //                row = row + 1;
    //                //sht.Range("A" + row).SetValue("Total");                   


    //                sht.Range("A" + row + ":C" + (row + 1)).Merge();
    //                sht.Range("A" + row + ":C" + (row + 1)).Style.Font.FontName = "Calibri";
    //                sht.Range("A" + row + ":C" + (row + 1)).Style.Font.FontSize = 12;
    //                //sht.Range("B6:D6").Style.Font.SetBold();                   
    //                sht.Range("A" + row + ":C" + (row + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("A" + row).Value = "";

    //                sht.Range("D" + row + ":J" + (row + 1)).Merge();
    //                sht.Range("D" + row + ":J" + (row + 1)).Style.Font.FontName = "Calibri";
    //                sht.Range("D" + row + ":J" + (row + 1)).Style.Font.FontSize = 12;
    //                //sht.Range("B6:D6").Style.Font.SetBold();                   
    //                sht.Range("D" + row + ":J" + (row + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("D" + row).Value = "";

    //                sht.Range("K" + row + ":N" + (row + 1)).Merge();
    //                sht.Range("K" + row + ":N" + (row + 1)).Style.Font.FontName = "Calibri";
    //                sht.Range("K" + row + ":N" + (row + 1)).Style.Font.FontSize = 12;
    //                //sht.Range("B6:D6").Style.Font.SetBold();                   
    //                sht.Range("K" + row + ":N" + (row + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //                sht.Range("K" + row).Value = "";

    //                sht.Row(row).Height = 30;
    //                row = row + 2;

    //                sht.Range("A" + row + ":C" + row).Value = "Prepared By";
    //                sht.Range("A" + row + ":C" + row).Style.Font.FontName = "Calibri";
    //                sht.Range("A" + row + ":C" + row).Style.Font.FontSize = 12;
    //                //sht.Range("B6:D6").Style.Font.SetBold();
    //                sht.Range("A" + row + ":C" + row).Merge();
    //                sht.Range("A" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

    //                sht.Range("K" + row + ":N" + row).Value = "Checked By";
    //                sht.Range("K" + row + ":N" + row).Style.Font.FontName = "Calibri";
    //                sht.Range("K" + row + ":N" + row).Style.Font.FontSize = 12;
    //                //sht.Range("B6:D6").Style.Font.SetBold();
    //                sht.Range("K" + row + ":N" + row).Merge();
    //                sht.Range("K" + row + ":N" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

    //                sht.Row(row).Height = 30;
    //                row = row + 1;

    //                //////*************
    //                //sht.Columns(1, 20).AdjustToContents();
    //                //********************
    //                //***********BOrders
    //                using (var a = sht.Range("A1" + ":N" + (row-1)))
    //                {
    //                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
    //                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //                }

    //                string Fileextension = "xlsx";
    //                string filename = UtilityModule.validateFilename("WeaverFolioMaterialDetailReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
    //                Path = Server.MapPath("~/Tempexcel/" + filename);
    //                xapp.SaveAs(Path);
    //                xapp.Dispose();
    //                //Download File
    //                Response.ClearContent();
    //                Response.ClearHeaders();
    //                // Response.Clear();
    //                Response.ContentType = "application/vnd.ms-excel";
    //                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
    //                Response.WriteFile(Path);
    //                // File.Delete(Path);
    //                Response.End();
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            lblmsg.Text = ex.Message;
    //        }

    //    }

    //}
    //protected void BtnPrintSummaryFolio_Click(object sender, EventArgs e)
    //{
    //    DataSet ds = new DataSet();

    //    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //    if (con.State == ConnectionState.Closed)
    //    {
    //        con.Open();
    //    }
    //    SqlCommand cmd = new SqlCommand("Pro_FolioReceiveDetail", con);
    //    cmd.CommandType = CommandType.StoredProcedure;
    //    cmd.CommandTimeout = 30000;

    //    cmd.Parameters.AddWithValue("@IssueOrderID", DDFolioNo.SelectedValue);
    //    cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
    //    SqlDataAdapter ad = new SqlDataAdapter(cmd);
    //    cmd.ExecuteNonQuery();
    //    ad.Fill(ds);

    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        Session["rptFileName"] = "Reports/RptFolioReceiveDetail.rpt";
    //        Session["dsFileName"] = "~\\ReportSchema\\RptFolioReceiveDetail.xsd";
    //        Session["GetDataset"] = ds;
    //        StringBuilder stb = new StringBuilder();
    //        stb.Append("<script>");
    //        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
    //        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
    //    }
    //}
}