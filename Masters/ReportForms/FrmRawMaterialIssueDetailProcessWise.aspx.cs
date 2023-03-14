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
using ClosedXML.Excel;

public partial class Masters_ReportForms_FrmRawMaterialIssueDetailProcessWise : System.Web.UI.Page
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
                        select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM JOIN CategorySeparate CS ON ICM.CATEGORY_ID=Cs.Categoryid and Cs.id=1
                        select val,type from sizetype
                        select  UnitsId,UnitName from  units with(nolock) Where Mastercompanyid=" + Session["varcompanyid"] + @"";
            DataSet ds = SqlHelper.ExecuteDataset(str);
            CommanFunction.FillComboWithDS(DDCompany, ds, 0);
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, "--select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 3, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDUnits, ds, 4, true, "--select--");
            ds.Dispose();
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
           
            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName_SelectedIndexChanged(sender, e);
            }

            TxtFromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

        }
    }   
  
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FillEmployee();
    }
    //protected void FillEmployee()
    //{
    //    string str;
    //    str = "select Distinct Ei.EmpId,ei.EmpName + case when Isnull(ei.empcode,'')<>'' Then ' ['+ei.empcode+']' else '' end as Empname from empinfo ei inner join  EmpProcess EP on ei.EmpId=Ep.EmpId";
    //    if (DDProcessName.SelectedIndex > 0)
    //    {
    //        str = str + " Where ep.ProcessId=" + DDProcessName.SelectedValue;
    //    }
    //    str = str + " order by EmpName";
    //    UtilityModule.ConditionalComboFill(ref DDEmpName, str, true, "--Select--");
    //}
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        categoryselectedChange();
        ddItemName_SelectedIndexChanged(sender, e);
    }
    protected void categoryselectedChange()
    {
        TRDDQuality.Visible = false;
        TRDDDesign.Visible = false;
        TRDDColor.Visible = false;
        TRDDShape.Visible = false;
        TRDDSize.Visible = false;
        TRDDShadeColor.Visible = false;

        string str;
        str = "select distinct vf.ITEM_ID,vf.ITEM_NAME from ProcessRawTran PT inner Join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID Where vf.CATEGORY_ID=" + DDCategory.SelectedValue + " order by Item_name";

        //if (RDRawMaterial.Checked == true)
        //{
        //    str = "select distinct vf.ITEM_ID,vf.ITEM_NAME from ProcessRawTran PT inner Join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID Where vf.CATEGORY_ID=" + DDCategory.SelectedValue + " order by Item_name";
        //}
        //else if (DDProcessName.SelectedIndex > 0)
        //{
        //    str = "select distinct vf.ITEM_ID,vf.ITEM_NAME from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD inner join V_FinishedItemDetail vf on Pd.Item_Finished_Id=vf.ITEM_FINISHED_ID Where   vf.CATEGORY_ID=" + DDCategory.SelectedValue + " order by ITEM_NAME";
        //}
        //else
        //{
        //    str = "select ITEM_ID,ITEM_NAME from ITEM_MASTER where CATEGORY_ID=" + DDCategory.SelectedValue + " order by Item_name";
        //}
        UtilityModule.ConditionalComboFill(ref ddItemName, str, true, "--All--");


        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select PARAMETER_ID  from item_category_parameters where CATEGORY_ID=" + DDCategory.SelectedValue);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in Ds.Tables[0].Rows)
            {
                switch (dr["Parameter_id"].ToString())
                {
                    case "1":
                        TRDDQuality.Visible = true;
                        break;
                    case "2":
                        TRDDDesign.Visible = true;
                        FillDesign();
                        break;
                    case "3":
                        TRDDColor.Visible = true;
                        FillColor();
                        break;
                    case "4":
                        TRDDShape.Visible = true;
                        FillShape();
                        break;
                    case "5":
                        TRDDSize.Visible = true;
                        FillSize();
                        break;
                    case "6":
                        TRDDShadeColor.Visible = true;
                        FillShadecolor();
                        break;
                }
            }
        }
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TRDDQuality.Visible == true)
        {
            string str;

            str = "select distinct vf.QualityId,vf.QualityName from ProcessRawTran PT inner Join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID Where Vf.Mastercompanyid=" + Session["varcompanyid"];
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + " And Vf.Item_Id=" + ddItemName.SelectedValue;
            }
            str = str + " order by QualityName";

            //if (RDRawMaterial.Checked == true)
            //{
            //    str = "select distinct vf.QualityId,vf.QualityName from ProcessRawTran PT inner Join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID Where Vf.Mastercompanyid=" + Session["varcompanyid"];
            //    if (ddItemName.SelectedIndex > 0)
            //    {
            //        str = str + " And Vf.Item_Id=" + ddItemName.SelectedValue;
            //    }
            //    str = str + " order by QualityName";

            //}
            //else if (DDProcessName.SelectedIndex > 0)
            //{
            //    str = "select distinct vf.QualityId,vf.QualityName from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD inner join V_FinishedItemDetail vf on Pd.Item_Finished_Id=vf.ITEM_FINISHED_ID Where   Vf.Mastercompanyid=" + Session["varcompanyid"];
            //    if (ddItemName.SelectedIndex > 0)
            //    {
            //        str = str + " And Vf.Item_Id=" + ddItemName.SelectedValue;
            //    }
            //    str = str + " order by QualityName";
            //}
            //else
            //{
            //    str = "select QualityId,QualityName from quality Where Mastercompanyid=" + Session["varcompanyid"];
            //    if (ddItemName.SelectedIndex > 0)
            //    {
            //        str = str + " And Item_Id=" + ddItemName.SelectedValue;
            //    }
            //    str = str + " order by QualityName";
            //}
            UtilityModule.ConditionalComboFill(ref DDQuality, str, true, "--All--");
        }
        if (ddItemName.SelectedIndex > 0)
        {
            FillDesign();
            FillColor();
            FillShape();
            FillShadecolor();
        }
    }
    protected void FillDesign()
    {
        string str;
        if (TRDDDesign.Visible == true)
        {
            str = "select distinct vf.designId,vf.designName from ProcessRawTran PT inner Join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID Where vf.Mastercompanyid=" + Session["varcompanyid"];
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
            }
            if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            {
                str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
            }
            str = str + " order by designname";

            //if (RDRawMaterial.Checked == true)
            //{
            //    str = "select distinct vf.designId,vf.designName from ProcessRawTran PT inner Join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID Where vf.Mastercompanyid=" + Session["varcompanyid"];
            //    if (ddItemName.SelectedIndex > 0)
            //    {
            //        str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
            //    }
            //    if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            //    {
            //        str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
            //    }
            //    str = str + " order by designname";
            //}
            //else if (DDProcessName.SelectedIndex > 0)
            //{
            //    str = "select distinct vf.designId,vf.designName from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD inner join V_FinishedItemDetail vf on Pd.Item_Finished_Id=vf.ITEM_FINISHED_ID Where   vf.Mastercompanyid=" + Session["varcompanyid"];
            //    if (ddItemName.SelectedIndex > 0)
            //    {
            //        str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
            //    }
            //    if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            //    {
            //        str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
            //    }
            //    str = str + " order by designname";
            //}
            //else
            //{
            //    str = "select distinct vf.designId,vf.designName from V_FinishedItemDetail vf Where vf.Mastercompanyid=" + Session["varcompanyid"];
            //    if (ddItemName.SelectedIndex > 0)
            //    {
            //        str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
            //    }
            //    if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            //    {
            //        str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
            //    }
            //    str = str + " order by designname";
            //}
            UtilityModule.ConditionalComboFill(ref DDDesign, str, true, "--select--");
        }
    }
    protected void FillColor()
    {
        string str;
        if (TRDDColor.Visible == true)
        {
            str = "select distinct vf.colorid,vf.ColorName from ProcessRawTran PT inner Join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID Where vf.Mastercompanyid=" + Session["varcompanyid"];
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
            }
            if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            {
                str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
            }
            if (TRDDDesign.Visible == true && DDDesign.SelectedIndex > 0)
            {
                str = str + " And vf.designid=" + DDDesign.SelectedValue;
            }
            str = str + " order by colorname";

            //if (RDRawMaterial.Checked == true)
            //{
            //    str = "select distinct vf.colorid,vf.ColorName from ProcessRawTran PT inner Join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID Where vf.Mastercompanyid=" + Session["varcompanyid"];
            //    if (ddItemName.SelectedIndex > 0)
            //    {
            //        str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
            //    }
            //    if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            //    {
            //        str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
            //    }
            //    if (TRDDDesign.Visible == true && DDDesign.SelectedIndex > 0)
            //    {
            //        str = str + " And vf.designid=" + DDDesign.SelectedValue;
            //    }
            //    str = str + " order by colorname";
            //}
            //else if (DDProcessName.SelectedIndex > 0)
            //{
            //    str = "select distinct vf.colorid,vf.colorname from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD inner join V_FinishedItemDetail vf on Pd.Item_Finished_Id=vf.ITEM_FINISHED_ID Where  vf.Mastercompanyid=" + Session["varcompanyid"];
            //    if (ddItemName.SelectedIndex > 0)
            //    {
            //        str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
            //    }
            //    if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            //    {
            //        str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
            //    }
            //    if (TRDDDesign.Visible == true && DDDesign.SelectedIndex > 0)
            //    {
            //        str = str + " And vf.designid=" + DDDesign.SelectedValue;
            //    }
            //    str = str + " order by colorname";
            //}
            //else
            //{
            //    str = "select distinct vf.colorid,vf.colorname from V_FinishedItemDetail vf Where vf.Mastercompanyid=" + Session["varcompanyid"];
            //    if (ddItemName.SelectedIndex > 0)
            //    {
            //        str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
            //    }
            //    if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            //    {
            //        str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
            //    }
            //    if (TRDDDesign.Visible == true && DDDesign.SelectedIndex > 0)
            //    {
            //        str = str + " And vf.designid=" + DDDesign.SelectedValue;
            //    }
            //    str = str + " order by colorname";
            //}
            UtilityModule.ConditionalComboFill(ref DDColor, str, true, "--select--");
        }
    }
    protected void FillShape()
    {
        string str;
        if (TRDDShape.Visible == true)
        {
            str = "select distinct vf.ShapeId,vf.ShapeName from V_FinishedItemDetail vf Where vf.Mastercompanyid=" + Session["varcompanyid"];
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
            }
            str = str + " order by shapename";

            //if (DDProcessName.SelectedIndex > 0)
            //{
            //    str = "select distinct vf.ShapeId,vf.ShapeName from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD inner join V_FinishedItemDetail vf on Pd.Item_Finished_Id=vf.ITEM_FINISHED_ID Where  vf.Mastercompanyid=" + Session["varcompanyid"];
            //    if (ddItemName.SelectedIndex > 0)
            //    {
            //        str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
            //    }

            //    str = str + " order by ShapeName";
            //}
            //else
            //{
            //    str = "select distinct vf.ShapeId,vf.ShapeName from V_FinishedItemDetail vf Where vf.Mastercompanyid=" + Session["varcompanyid"];
            //    if (ddItemName.SelectedIndex > 0)
            //    {
            //        str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
            //    }
            //    str = str + " order by shapename";
            //}
            UtilityModule.ConditionalComboFill(ref DDShape, str, true, "--select--");
        }
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDesign();
        FillShadecolor();
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillColor();
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void FillSize()
    {
        string str;
        string strSize;
        switch (DDsizetype.SelectedValue)
        {
            case "0":
                strSize = "Sizeft";
                break;
            case "1":
                strSize = "Sizemtr";
                break;
            case "2":
                strSize = "Sizeinch";
                break;
            default:
                strSize = "Sizeft";
                break;
        }
        if (TRDDSize.Visible == true)
        {
            if (DDProcessName.SelectedIndex > 0)
            {
                str = "select distinct vf.SizeId,vf." + strSize + " as Size from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD inner join V_FinishedItemDetail vf on Pd.Item_Finished_Id=vf.ITEM_FINISHED_ID Where  vf.Mastercompanyid=" + Session["varcompanyid"];
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
                }
                if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
                {
                    str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
                }
                if (TRDDDesign.Visible == true && DDDesign.SelectedIndex > 0)
                {
                    str = str + " And vf.designid=" + DDDesign.SelectedValue;
                }
                if (TRDDColor.Visible == true && DDColor.SelectedIndex > 0)
                {
                    str = str + " And vf.colorid=" + DDColor.SelectedValue;
                }
                if (DDShape.SelectedIndex > 0)
                {
                    str = str + " and vf.ShapeId=" + DDShape.SelectedValue;
                }
                str = str + " order by Size";
            }
            else
            {
                str = "select distinct vf.SizeId,vf." + strSize + " as Size from V_FinishedItemDetail vf Where vf.Mastercompanyid=" + Session["varcompanyid"];
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
                }
                if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
                {
                    str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
                }
                if (TRDDDesign.Visible == true && DDDesign.SelectedIndex > 0)
                {
                    str = str + " And vf.designid=" + DDDesign.SelectedValue;
                }
                if (TRDDColor.Visible == true && DDColor.SelectedIndex > 0)
                {
                    str = str + " And vf.colorid=" + DDColor.SelectedValue;
                }
                if (DDShape.SelectedIndex > 0)
                {
                    str = str + " and vf.ShapeId=" + DDShape.SelectedValue;
                }
                str = str + " order by Size";
            }
            UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--select--");
        }
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void FillShadecolor()
    {
        string str;
        if (TRDDShadeColor.Visible == true)
        {
            str = "select distinct vf.ShadecolorId,vf.ShadeColorName from ProcessRawTran PT inner Join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID Where vf.Item_Id=" + ddItemName.SelectedValue;
            if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            {
                str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
            }
            str = str + " order by vf.ShadeColorName";
            UtilityModule.ConditionalComboFill(ref DDShadeColor, str, true, "--select--");
        }
    }
    protected void ChkForDate_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForDate.Checked == true)
        {
            trDates.Visible = true;
            TxtFromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
        else
        {
            trDates.Visible = false;
        }
    }

    protected void ProcessRawMaterialIssueDetail()
    {
        lblMessage.Text = "";
        try
        {
            string Where = "";

            if (DDUnits.SelectedIndex > 0)
            {
                Where = Where + " and PIM.Units=" + DDUnits.SelectedValue;
            }
            //if (DDProcessName.SelectedIndex > 0)
            //{
            //    Where = Where + " and PRM.ProcessId=" + DDProcessName.SelectedValue;
            //}           
            if (DDCategory.SelectedIndex > 0)
            {
                Where = Where + " and VF.Category_id=" + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Where = Where + " and VF.Item_Id=" +ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                Where = Where + " and VF.qualityid=" +DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                Where = Where + " and VF.DesignId=" + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                Where = Where + " and vf.Colorid=" + DDColor.SelectedValue;
            }
            if (DDShape.SelectedIndex > 0)
            {
                Where = Where + " and vf.shapeid=" + DDShape.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0)
            {
                Where = Where + " and vf.SizeId=" + DDSize.SelectedValue;
            }
            if (DDShadeColor.SelectedIndex > 0)
            {
                Where = Where + " and vf.ShadeColorId" + DDShadeColor.SelectedValue;
            }
            //Where = Where + " and Prm.ReceiveDate>='" + TxtFromDate.Text + "' and prm.receivedate<='" + TxtToDate.Text + "'";

            //if (ChkForDate.Checked == true)
            //{
            //    Where = Where + " and Prm.ReceiveDate>='" +TxtFromDate.Text + "' and prm.receivedate<='" +TxtToDate.Text + "'";
            //}
           
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETRAWMATERIALISSUEDETAILPROCESSWISE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;
            cmd.Parameters.AddWithValue("@CompanyId", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@ProcessId", DDProcessName.SelectedValue);            
            cmd.Parameters.AddWithValue("@Fromdate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@TOdate", TxtToDate.Text);


            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************
            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                //***********
                sht.Row(1).Height = 24;
                sht.Range("A1:K1").Merge();
                sht.Range("A1:K1").Style.Font.FontSize = 10;
                sht.Range("A1:K1").Style.Font.Bold = true;
                sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:K1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1:K1").Style.Alignment.WrapText = true;
                //************
                sht.Range("A1").SetValue("MATERIAL RAW ISSUE DETAIL"+" FROM"+" "+TxtFromDate.Text+" "+"TO"+ " "+TxtToDate.Text);
                //Detail headers                
                sht.Range("A2:K2").Style.Font.FontSize = 10;
                sht.Range("A2:K2").Style.Font.Bold = true;

                sht.Range("A2").Value = "DATE";
                sht.Range("B2").Value = "PROCESS NAME";
                sht.Range("C2").Value = "LOOM NO";
                sht.Range("D2").Value = "FOLIONO";
                sht.Range("E2").Value = "ISSUE CHALLANNO";
                sht.Range("F2").Value = "RAW MATERIAL DESCRIPTION";
                sht.Range("G2").Value = "LOT NO";
                sht.Range("H2").Value = "TAG NO";
                sht.Range("I2").Value = "ISSUE QTY";
                sht.Range("J2").Value = "GODOWN NAME";
                sht.Range("K2").Value = "USER NAME";  
                //sht.Range("H2:O2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                using (var a = sht.Range("A2:K2"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                int row = 3;
                //int Lastissueorderid = 0;
                ////**********Sorting
                //DataView dv = ds.Tables[0].DefaultView;
                //dv.Sort = "prorderid,TYPE";
                //DataSet ds1 = new DataSet();
                //ds1.Tables.Add(dv.ToTable());
                ////***************

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    //sht.Range("H" + row + ":O" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    //var Onepcslagattotal = ds.Tables[0].Compute("sum(ONEPCSLAGAT)", "prorderid=" + ds1.Tables[0].Rows[i]["prorderid"] + "");

                    //if (Session["varcompanyId"].ToString() == "21")
                    //{
                    //    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
                    //    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Prorderid"]);
                    //}
                    //else
                    //{
                    //    if (Lastissueorderid != Convert.ToInt32(ds1.Tables[0].Rows[i]["Prorderid"]))
                    //    {
                    //        sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
                    //        sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Prorderid"]);
                    //        Lastissueorderid = Convert.ToInt32(ds1.Tables[0].Rows[i]["Prorderid"]);
                    //    }
                    //}

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Date"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["PROCESS_NAME"]);

                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["LoomNo"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["IssueOrderId"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ChalanNo"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ItemDescription"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Lotno"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["TagNo"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["IssueQty"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["GodownName"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["UserName"]);   

                    //sht.Range("K" + row).FormulaA1 = "=I" + row + "*J" + row;
                    //sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["Weightrec"]);
                    //sht.Range("M" + row).FormulaA1 = "=J" + row + "/" + Onepcslagattotal + "*" + 100;
                    //sht.Cell(row, "M").Style.NumberFormat.Format = "#,##0.00";
                    //sht.Range("N" + row).FormulaA1 = "=M" + row + "*L" + row + "/100";
                    //sht.Cell(row, "N").Style.NumberFormat.Format = "#,##0.000";
                    //sht.Range("O" + row).FormulaA1 = "=H" + row + "-N" + row;
                    //sht.Cell(row, "O").Style.NumberFormat.Format = "#,##0.000";

                    using (var a = sht.Range("A" + row + ":K" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    row = row + 1;
                }
                ds.Dispose();               

                sht.Columns(1, 25).AdjustToContents();
                //************** Save
                String Path;

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("RawMaterialIssueDetailReport_" + DateTime.Now + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('No records found')", true);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
        }
    }
    
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        ProcessRawMaterialIssueDetail();
       
    }
}