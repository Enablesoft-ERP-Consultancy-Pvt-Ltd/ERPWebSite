using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.IO;

public partial class Masters_ReportForms_frmwarpingreports : System.Web.UI.Page
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
                            Select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + @" order by CATEGORY_NAME
                            select customerid,CustomerCode+'  '+companyname as customer from customerinfo WHere mastercompanyid=" + Session["varcompanyid"] + @" order by customer
                            select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where Process_Name in('WARPING WOOL','WARPING COTTON') and MasterCompanyid=" + Session["varcompanyid"] + @"
                            select Unitsid,UnitName from Units Where MasterCompanyid=" + Session["varcompanyid"] + @" ";
            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 1, true, "ALL");
            UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcess, ds, 3, true, "--Plz Select --");
            UtilityModule.ConditionalComboFillWithDS(ref DDProductionUnit, ds, 4, true, "--Plz Select --");            
            txtfromdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

           

            switch (Convert.ToInt16(Session["varcompanyId"]))
            {
                case 21:  ///Kaysons
                    TRWarpingMaterialIssueDetail.Visible = true;
                    break;
                case 14:  ///EasternMill
                    TRWarpingOrderWithBeamFolio.Visible = true;                    
                    break;
                default:
                    TRWarpingMaterialIssueDetail.Visible = false;
                    TRWarpingOrderWithBeamFolio.Visible = false;
                    break;
            }

            if (TRWarpingOrderWithBeamFolio.Visible == true)
            {
                BindWarpingIssueNo();
            }
        }
    }
    protected void BindWarpingIssueNo()
    {
        string str = "select Id,IssueNo from WARPORDERMASTER Where CompanyId=" + DDCompany.SelectedValue + @"";
        str = str + " order by Id";
        UtilityModule.ConditionalComboFill(ref DDWarpingIssueNo, str, true, "--Plz Select --");
    }
    protected void DDProductionUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select UID,LoomNo from ProductionLoomMaster Where UnitId=" + DDProductionUnit.SelectedValue + " and CompanyId=" + DDCompany.SelectedValue + "";
        str = str + " order by LoomNo";
        UtilityModule.ConditionalComboFill(ref DDLoomNo, str, true, "ALL");
    }
    protected void DDLoomNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select PM.IssueOrderId,PM.IssueOrderId from PROCESS_ISSUE_MASTER_1 PM 
                    where PM.Companyid=" + DDCompany.SelectedValue + " and PM.Units=" + DDProductionUnit.SelectedValue + " and PM.LoomId=" + DDLoomNo.SelectedValue;
        UtilityModule.ConditionalComboFill(ref DDFolioNo, str, true, "--Plz Select--");
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select Item_id,ITEM_NAME From Item_master where MasterCompanyid=" + Session["varcompanyId"] + "";
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " and Category_id=" + DDCategory.SelectedValue;
        }
        str = str + " order by ITEM_NAME";
        UtilityModule.ConditionalComboFill(ref ddItemName, str, true, "ALL");
        if (ddItemName.Items.Count > 0)
        {
            ddItemName_SelectedIndexChanged(sender, new EventArgs());
        }
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        TRDDQuality.Visible = true;
        if (ddItemName.SelectedItem.Text.Trim() != "ALL")
        {
            UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality Where Item_Id=" + ddItemName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyid"] + " order by QualityName", true, "ALL");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality Where MasterCompanyId=" + Session["varCompanyid"] + "  order by QualityName", true, "ALL");
        }
        DDQuality_SelectedIndexChanged(sender, e);
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {

        TRDDDesign.Visible = false;
        TRDDColor.Visible = false;
        TRDDShadeColor.Visible = false;
        TRDDShape.Visible = false;
        TRDDSize.Visible = false;

        string qry = @"select DESIGNID,DESIGNNAME from DESIGN Where MasterCompanyId=" + Session["varCompanyid"] + @" ORDER BY DESIGNNAME
        select COLORID,COLORNAME from color Where MasterCompanyId=" + Session["varCompanyid"] + @" ORDER BY COLORNAME
        SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR Where MasterCompanyId=" + Session["varCompanyid"] + @" ORDER BY SHADECOLORNAME
        SELECT SHAPEID, SHAPENAME FROM SHAPE Where MasterCompanyId=" + Session["varCompanyid"] + " ORDER BY SHAPENAME";
        DataSet ds = SqlHelper.ExecuteDataset(qry);

        string str = @"select PARAMETER_ID from ITEM_CATEGORY_PARAMETERS where category_id=" + DDCategory.SelectedValue;
        DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds1.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                switch (dr["Parameter_Id"].ToString())
                {
                    case "2":
                        TRDDDesign.Visible = true;
                        UtilityModule.ConditionalComboFillWithDS(ref DDDesign, ds, 0, true, "ALL");
                        break;
                    case "3":
                        TRDDColor.Visible = true;
                        UtilityModule.ConditionalComboFillWithDS(ref DDColor, ds, 1, true, "ALL");
                        break;
                    case "4":
                        TRDDShape.Visible = true;
                        UtilityModule.ConditionalComboFillWithDS(ref DDShape, ds, 3, true, "ALL");
                        break;
                    case "5":
                        TRDDSize.Visible = true;
                        break;
                    case "6":
                        TRDDShadeColor.Visible = true;
                        UtilityModule.ConditionalComboFillWithDS(ref DDShadeColor, ds, 2, true, "ALL");
                        break;
                }
            }
        }

    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        TRDDSize.Visible = true;
        string str = @"SELECT SIZEID, SIZEft AS SIZENAME FROM SIZE WHERE MasterCompanyId=" + Session["varCompanyid"];
        if (DDShape.SelectedIndex > 0)
        {
            str = str + " and SHAPEID=" + DDShape.SelectedValue;
        }
        str = str + "ORDER BY SIZEID";
        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "ALL");
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        if (RDWarpingIssueDetail.Checked == true)
        {
            WarpingissueDetail();
        }
        else if (RDBeamstock.Checked == true)
        {
            BeamStock();
        }
        else if (RDWarpingsumm.Checked == true)
        {
            Warpsummary();
        }
        else if (RDWarpingBeamReceiveDetail.Checked == true)
        {
            WarpingBeamReceiveDetail();
        }
        else if (RDLoomBeamIssueDetail.Checked == true)
        {
            LoomBeamIssueDetail();
        }
        else if (RDLoomBeamReceiveDetail.Checked == true)
        {
            LoomBeamReceiveDetail();
        }
        else if (RDLoomBeamPendingDetail.Checked == true)
        {
            LoomBeamPendingDetail();
        }
        else if (RDWarpingBeamReceiveDetailWithRate.Checked == true)
        {
            WarpingBeamReceiveDetailWithRateMcNo();
        }
        else if (RDWarpingMaterialIssueDetail.Checked == true)
        {
            WarpingMaterialissueDetail();
        }
        else if (RDWarpingOrderWithBeamFolio.Checked == true)
        {
            WarpingOrderWithBeamFolioDetail();
        }
    }
    protected void LoomBeamIssueDetail()
    {
        string str = "";
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and VF.Qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and VF.DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and VF.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            str = str + " and VF.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and VF.sizeid=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            str = str + " and VF.shadecolorid=" + DDShadeColor.SelectedValue;
        }

        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@Where", str);
        param[2] = new SqlParameter("@FromDate", txtfromdate.Text);
        param[3] = new SqlParameter("@Todate", txttodate.Text);
        //*********
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetWarpingLoomBeamIssueDetail", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("LoomBeamIssueDetail");

            //*************
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:M1").Merge();
            sht.Range("A1:M1").Style.Font.FontSize = 10;
            sht.Range("A1:M1").Style.Font.Bold = true;
            sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:M1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:M1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("Loom Beam IssueDetail From : " + txtfromdate.Text + " To : " + txttodate.Text + "");

            sht.Range("A2:M2").Style.Font.FontSize = 10;
            sht.Range("A2:M2").Style.Font.Bold = true;

            sht.Range("A2").Value = "Issue Date";
            sht.Range("B2").Value = "Beam Description";
            sht.Range("C2").Value = "Lot No";
            sht.Range("D2").Value = "Tag No";
            sht.Range("E2").Value = "Unit Name";
            sht.Range("F2").Value = "Loom No";
            sht.Range("G2").Value = "Folio No";
            sht.Range("H2").Value = "Beam No";
            sht.Range("I2").Value = "Gross Weight";
            sht.Range("J2").Value = "Tare Weight";
            sht.Range("K2").Value = "Net Weight";
            sht.Range("L2").Value = "Pcs";
            sht.Range("M2").Value = "User Name";

            int row = 3;


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["IssueDate"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["BeamDescription"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["LotNo"]);
                sht.Range("C" + row).Style.NumberFormat.Format = "@";
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["TagNo"]);
                sht.Range("D" + row).Style.NumberFormat.Format = "@";
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["UnitName"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["LoomNo"]);

                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["FolioChallanNo"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["BeamNo"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["GrossWeight"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["TareWeight"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["NetWeight"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Pcs"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["UserName"]);

                row = row + 1;
            }
            ds.Dispose();

            using (var a = sht.Range("A1" + ":M" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            //*************************************************
            String Path;
            sht.Columns(1, 26).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("LoomBeamIssueDetail_" + DateTime.Now + "." + Fileextension);
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
    }
    protected void WarpingBeamReceiveDetail()
    {
        string str = "";
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and VF.Qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and VF.DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and VF.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            str = str + " and VF.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and VF.sizeid=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            str = str + " and VF.shadecolorid=" + DDShadeColor.SelectedValue;
        }

        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@ProcessID", DDProcess.SelectedValue);
        param[2] = new SqlParameter("@Where", str);
        param[3] = new SqlParameter("@FromDate", txtfromdate.Text);
        param[4] = new SqlParameter("@Todate", txttodate.Text);
        //*********
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetWarpingBeamReceiveDetail", param);
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
            //**************
            sht.Columns("A").Width = 15.78;
            sht.Columns("B").Width = 15.78;
            sht.Columns("C").Width = 35.78;

            //Headers
            sht.Range("A1").Value = "Receive Date";
            sht.Range("B1").Value = "EmpName";

            sht.Range("C1").Value = "ItemDescription";
            sht.Range("D1").Value = "UnitName";
            sht.Range("E1").Value = "LotNo";
            sht.Range("F1").Value = "TagNo";
            sht.Range("G1").Value = "PCSBEAM";
            sht.Range("H1").Value = "NOOFBEAMREQ";
            sht.Range("I1").Value = "GodownName";
            sht.Range("J1").Value = "IssueNo";
            sht.Range("K1").Value = "ReceiveQty";
            sht.Range("L1").Value = "BeamNo";
            sht.Range("M1").Value = "UserName";
            sht.Range("N1").Value = "Production Unit";
            sht.Range("O1").Value = "Company Name";

            sht.Range("A1:O1").Style.Font.Bold = true;
            sht.Range("G1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("K1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            //******************************
            row = 2;
            decimal Bal = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":L" + row).Style.Alignment.SetWrapText();

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["ReceiveDate"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);

                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ItemDescription"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["UnitName"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["LotNo"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["TagNo"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["PCSBEAM"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["NOOFBEAMREQ"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["GodownName"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["IssueNo"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["ReceiveQty"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["BeamNo"]);
                sht.Range("L" + row).Style.NumberFormat.Format = "@";
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["UserName"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["ProductionUnitName"]);
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["CompanyName"]);
                row = row + 1;
            }
            //**********Total
            sht.Columns(4, 30).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("BeamReceiveDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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

            #region

            //ds.Tables[0].Columns.Remove("ID");
            //ds.Tables[0].Columns.Remove("DetailID");
            ////Export to excel
            //GridView GridView1 = new GridView();
            //GridView1.AllowPaging = false;

            //GridView1.DataSource = ds;
            //GridView1.DataBind();
            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition",
            // "attachment;filename=BeamReceiveDetail" + DateTime.Now + ".xls");
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);

            //for (int i = 0; i < GridView1.Rows.Count; i++)
            //{
            //    //Apply text style to each Row
            //    GridView1.Rows[i].Attributes.Add("class", "textmode");                 
            //}
            //GridView1.RenderControl(hw);

            ////style to format numbers to string
            //string style = @"<style> .textmode { mso-number-format:\@; } </style>";

            //Response.Write(style);
            //Response.Output.Write(sw.ToString());
            //Response.Flush();
            //Response.End();
            ////*************
            #endregion

        }
    }
    protected void WarpingissueDetail()
    {
        string Filterby = "Filter By-From:" + txtfromdate.Text + " To: " + txttodate.Text + "";
        int Row;
        string str = @"select WIM.IssueDate,D.DepartmentName,vf.QualityName,vf.ShadeColorName,WID.LotNo,WID.TagNo,Sum(WID.IssueQty) as Qty,WIM.ID as IssueNo,
                    WOM.Issueno as WarpOrderNO,ROUND(ISNULL(WR.NETWT,0),3) AS RECWT, U.UnitName, NUD.UserName, CI.CompanyName 
                    From WarpRawIssueMaster WIM(Nolock)
                    inner join WarpRawissueDetail WID(Nolock) on WIM.id=WID.Id
                    inner join warpordermaster WOM(Nolock) on Wid.issuemasterid = WOM.ID
                    inner join  V_FinishedItemDetail vf(Nolock) on WID.Ifinishedid=vf.ITEM_FINISHED_ID
                    inner join Department D(Nolock) on WIM.Deptid=D.DepartmentId
                    inner join CompanyInfo ci(Nolock) on WIM.companyid=Ci.companyid 
                    LEFT JOIN V_WARPNETWTRECEIVE_ISSUENOWISE WR(Nolock) ON WOM.ID=WR.ISSUEMASTERID 
                    LEFT JOIN Units U(Nolock) ON U.UnitsId = WOM.Units 
                    JOIN NewUserDetail NUD(Nolock) ON NUD.UserId = WOM.UserID 
                    WHere WIM.CompanyId=" + DDCompany.SelectedValue + " and WIM.issuedate>='" + txtfromdate.Text + "' and WIM.issuedate<='" + txttodate.Text + "'";
        if (DDProcess.SelectedIndex > 0)
        {
            str = str + " and WoM.Processid=" + DDProcess.SelectedValue;
            Filterby = Filterby + "-" + "Process :" + DDProcess.SelectedItem.Text;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf.ITEM_ID=" + ddItemName.SelectedValue;
            Filterby = Filterby + "-" + "ItemName :" + ddItemName.SelectedItem.Text;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.Qualityid=" + DDQuality.SelectedValue;
            Filterby = Filterby + "-" + "Quality :" + DDQuality.SelectedItem.Text;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            str = str + " and vf.shadecolorid=" + DDShadeColor.SelectedValue;
            Filterby = Filterby + "-" + "ShadeNo :" + DDShadeColor.SelectedItem.Text;
        }
        if (txtLotno.Text != "")
        {
            str = str + " and WID.Lotno='" + txtLotno.Text + "'";
            Filterby = Filterby + "-" + "LotNo :" + txtLotno.Text;
        }
        if (txtTagno.Text != "")
        {
            str = str + " and WID.Tagno='" + txtTagno.Text + "'";
            Filterby = Filterby + "-" + "TagNo :" + txtTagno.Text;
        }

        str = str + @" group by WIM.IssueDate,D.DepartmentName,vf.QualityName,vf.ShadeColorName,WID.LotNo,WID.TagNo,WIM.ID,Wom.issueno,WR.NETWT, 
        U.UnitName, NUD.UserName, CI.CompanyName order by WIM.issuedate";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("WarpingIssueDetails");
            //****************
            //****************
            sht.Range("A1:M1").Merge();
            sht.Range("A1:M1").Style.Font.FontSize = 11;
            sht.Range("A1:M1").Style.Font.Bold = true;
            sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:M1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1").SetValue(ds.Tables[0].Rows[0]["CompanyName"] + " WARPING ISSUE DETAIL");
            sht.Row(1).Height = 21.75;

            sht.Range("A2:M2").Merge();
            sht.Range("A2:M2").Style.Font.FontSize = 11;
            sht.Range("A2:M2").Style.Font.Bold = true;
            sht.Range("A2:M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:M2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2").SetValue(Filterby);
            sht.Row(2).Height = 21.75;

            sht.Range("A3:M3").Style.Font.FontSize = 11;
            sht.Range("A3:M3").Style.Font.Bold = true;
            sht.Range("A3:M3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("G3:I3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Row(3).Height = 18.00;
            //
            sht.Range("A3").SetValue("Date");
            sht.Range("B3").SetValue("Department");
            sht.Range("C3").SetValue("Quality");
            sht.Range("D3").SetValue("ShadeNo.");
            sht.Range("E3").SetValue("LotNo.");
            sht.Range("F3").SetValue("TagNo.");
            sht.Range("G3").SetValue("Qty");
            sht.Range("H3").SetValue("RecQty");
            sht.Range("I3").SetValue("BalQty");
            sht.Range("J3").SetValue("Issue Challan No.");
            sht.Range("K3").SetValue("Warp Order No.");
            sht.Range("L3").SetValue("Production Unit");
            sht.Range("M3").SetValue("User Name");
            Row = 4;
            if (DDProcess.SelectedItem.Text != "WARPING COTTON")
            {
                sht.Column("H").Hide();
                sht.Column("I").Hide();
            }
            int rowcount = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rowcount; i++)
            {
                sht.Range("J" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A" + Row + ":H" + Row).Style.Font.FontSize = 11;

                sht.Range("A" + Row).SetValue(ds.Tables[0].Rows[i]["Issuedate"]);
                sht.Range("B" + Row).SetValue(ds.Tables[0].Rows[i]["Departmentname"]);
                sht.Range("C" + Row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("D" + Row).SetValue(ds.Tables[0].Rows[i]["Shadecolorname"]);
                sht.Range("E" + Row).SetValue(ds.Tables[0].Rows[i]["Lotno"]);
                sht.Range("F" + Row).SetValue(ds.Tables[0].Rows[i]["Tagno"]);
                sht.Range("G" + Row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                sht.Range("H" + Row).SetValue(ds.Tables[0].Rows[i]["RecWt"]);
                sht.Range("I" + Row).FormulaA1 = "=G" + Row + "-H" + Row;
                sht.Range("J" + Row).SetValue(ds.Tables[0].Rows[i]["IssueNO"]);
                sht.Range("K" + Row).SetValue(ds.Tables[0].Rows[i]["Warporderno"]);
                sht.Range("L" + Row).SetValue(ds.Tables[0].Rows[i]["UnitName"]);
                sht.Range("M" + Row).SetValue(ds.Tables[0].Rows[i]["UserName"]);
                Row = Row + 1;
            }
            //**********Total
            //var issued = sht.Evaluate("SUM(G4:G" + (Row - 1) + ")");
            //*************
            sht.Columns(1, 15).AdjustToContents();
            //sht.Range("G" + Row).SetValue(issued);
            sht.Range("G" + Row).FormulaA1 = "=SUM(G4:G" + (Row - 1) + ")";
            sht.Range("H" + Row).FormulaA1 = "=SUM(H4:H" + (Row - 1) + ")";
            sht.Range("I" + Row).FormulaA1 = "=SUM(I4:I" + (Row - 1) + ")";
            //**************Save
            //******SAVE FILE
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("WARPING ISSUE DETAIL-" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "al", "alert('No records found..')", true);
        }
    }
    protected void BeamStock()
    {
        int Row;
        string str = "";

        if (Session["VarCompanyNo"].ToString() == "14")
        {
            str = @"select WLM.LoomNo as BeamNo,dbo.F_BeamNoDescription(WLM.LoomNo)as BeamDescription,GM.GodownName
                    ,sum(WD.Pcs-isnull(WD.IssuePcs,0)) as Pcs,WLM.Grossweight,WLM.TareWeight,WLM.NetWeight
                    From WarpLoommaster WLM inner Join LoomStock ls on WLM.LoomNo=Ls.LoomNo
                    inner join WarpLoomDetail WD on WLM.ID=WD.ID
                    inner join godownmaster GM on LS.GodownId=GM.GoDownID
                    Where Round(LS.Qtyinhand,3)>0.09 and WLM.Receivedate>'2017-08-08 00:00:00.000'
                    and WLM.LoomNo not in('4573','4574','4575','4576','4577','4578','4579','4580','4581','4582','4583','4584','4585','4586','4587','4588','4589','4590','17477',
                    '17478','19159','19160','19161','19162','19163','19164','19165','41233')
                    group by WLM.LoomNo,GM.GodownName,WLM.Grossweight,WLM.TareWeight,WLM.NetWeight
                    having sum(WD.Pcs-isnull(WD.IssuePcs,0))>0
                    order by case When ISNUMERIC(WLM.LoomNo)=1 then cast(WLM.LoomNo as int) else 999999 end";
        }
        else
        {

            str = @"select WLM.LoomNo as BeamNo,dbo.F_BeamNoDescription(WLM.LoomNo)as BeamDescription,GM.GodownName
                    ,sum(WD.Pcs-isnull(WD.IssuePcs,0)) as Pcs,WLM.Grossweight,WLM.TareWeight,WLM.NetWeight
                    From WarpLoommaster WLM inner Join LoomStock ls on WLM.LoomNo=Ls.LoomNo
                    inner join WarpLoomDetail WD on WLM.ID=WD.ID
                    inner join godownmaster GM on LS.GodownId=GM.GoDownID
                    Where Round(LS.Qtyinhand,3)>0.09
                    group by WLM.LoomNo,GM.GodownName,WLM.Grossweight,WLM.TareWeight,WLM.NetWeight
                    order by case When ISNUMERIC(WLM.LoomNo)=1 then cast(WLM.LoomNo as int) else 999999 end";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("BeamStockDetail");
            //****************
            //*************
            sht.Range("A1:G1").Merge();
            sht.Range("A1:G1").Style.Font.FontSize = 11;
            sht.Range("A1:G1").Style.Font.Bold = true;
            sht.Range("A1:G1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:G1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1").SetValue("BEAM STOCK DETAIL");
            sht.Row(1).Height = 21.75;



            sht.Range("A2:G2").Style.Font.FontSize = 11;
            sht.Range("A2:G2").Style.Font.Bold = true;
            sht.Range("D2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("E2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("F2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            sht.Row(2).Height = 18.00;
            //
            sht.Range("A2").SetValue("Beam No.");
            sht.Range("B2").SetValue("Beam Description");
            sht.Range("C2").SetValue("Godown Name");
            sht.Range("D2").SetValue("Pcs(Beam)");
            sht.Range("E2").SetValue("Gross Weight");
            sht.Range("F2").SetValue("Tare Weight");
            sht.Range("G2").SetValue("Net Weight");

            Row = 3;

            int rowcount = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rowcount; i++)
            {
                sht.Range("A" + Row + ":H" + Row).Style.Font.FontSize = 11;

                sht.Range("A" + Row).SetValue(ds.Tables[0].Rows[i]["BeamNo"]);
                sht.Range("B" + Row).SetValue(ds.Tables[0].Rows[i]["BeamDescription"]);
                sht.Range("C" + Row).SetValue(ds.Tables[0].Rows[i]["godownname"]);
                sht.Range("D" + Row).SetValue(ds.Tables[0].Rows[i]["Pcs"]);
                sht.Range("E" + Row).SetValue(ds.Tables[0].Rows[i]["grossweight"]);
                sht.Range("F" + Row).SetValue(ds.Tables[0].Rows[i]["Tareweight"]);
                sht.Range("G" + Row).SetValue(ds.Tables[0].Rows[i]["Netweight"]);
                Row = Row + 1;

            }
            //**********Total
            var Tpcs = sht.Evaluate("SUM(D3:D" + (Row - 1) + ")");
            var Tgrosswt = sht.Evaluate("SUM(E3:E" + (Row - 1) + ")");
            var TTarewt = sht.Evaluate("SUM(F3:F" + (Row - 1) + ")");
            var Tnetwt = sht.Evaluate("SUM(G3:G" + (Row - 1) + ")");
            //*************
            sht.Columns(1, 10).AdjustToContents();
            sht.Range("D" + Row).SetValue(Tpcs);
            sht.Range("E" + Row).SetValue(Tgrosswt);
            sht.Range("F" + Row).SetValue(TTarewt);
            sht.Range("G" + Row).SetValue(Tnetwt);
            //**************Save
            //******SAVE FILE
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("BEAMSTOCKDETAIL-" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "al1", "alert('No records found..')", true);
        }
    }
    protected void Warpsummary()
    {
        string str = "";
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf1.ITEM_ID=" + ddItemName.SelectedValue;

        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf1.Qualityid=" + DDQuality.SelectedValue;

        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf1.DesignId=" + DDDesign.SelectedValue;

        }

        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and vf1.colorid=" + DDColor.SelectedValue;

        }

        if (DDShape.SelectedIndex > 0)
        {
            str = str + " and vf1.shapeid=" + DDShape.SelectedValue;

        }

        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and vf1.sizeid=" + DDSize.SelectedValue;

        }

        if (DDShadeColor.SelectedIndex > 0)
        {
            str = str + " and vf1.shadecolorid=" + DDShadeColor.SelectedValue;

        }


        SqlParameter[] param = new SqlParameter[6];
        param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@Customerid", DDcustcode.SelectedIndex > 0 ? DDcustcode.SelectedValue : "0");
        param[2] = new SqlParameter("@orderid", DDorderno.SelectedIndex > 0 ? DDorderno.SelectedValue : "0");
        param[3] = new SqlParameter("@Where", str);
        param[4] = new SqlParameter("@FromDate", txtfromdate.Text);
        param[5] = new SqlParameter("@Todate", txttodate.Text);
        //*********
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetWarpSummorderwise", param);
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
            //**************
            //Headers
            sht.Range("A1").Value = "Req Date";
            sht.Range("B1").Value = "DepartmentName";
            sht.Range("C1").Value = "Order No.";
            sht.Range("D1").Value = "Article Description";
            sht.Range("E1").Value = "Order Qty";
            sht.Range("F1").Value = "Already Issued";
            sht.Range("G1").Value = "For Issue Bal.";
            sht.Range("H1").Value = "Beam Description";
            sht.Range("I1").Value = "Req Qty.";
            sht.Range("J1").Value = "Rec Qty";
            sht.Range("K1").Value = "Bal Qty";
            sht.Range("L1").Value = "Production Unit";
            sht.Range("M1").Value = "User Name";
            sht.Range("N1").Value = "COMPANY NAME";
            sht.Range("A1:N1").Style.Font.Bold = true;
            sht.Range("E1:G1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("I1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            //******************************
            row = 2;
            decimal Bal = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Targetdate"]);
                sht.Range("B" + row).SetValue("WARPING SECTION");
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["customerorderNo"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Itemdescription"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Orderqty"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Alreadyissued"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Issuebal"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Beamdescription"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Reqqty"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Receiveqty"]);
                Bal = Convert.ToDecimal(ds.Tables[0].Rows[i]["Reqqty"]) - Convert.ToDecimal(ds.Tables[0].Rows[i]["Receiveqty"]);
                sht.Range("K" + row).SetValue(Bal);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["UnitName"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["UserName"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["CompanyName"]);
                row = row + 1;
            }
            //**********Total
            sht.Columns(1, 30).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("WarpingSummary_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "al3", "alert('No records found..')", true);
        }
    }
    protected void DDcustcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDorderno, @"select orderid,LocalOrder+' '+CustomerOrderNo as orderno from ordermaster where CompanyId=" + DDCompany.SelectedValue + " and customerid=" + DDcustcode.SelectedValue + @"
                                            and status=0 order by orderno", true, "--Plz Select--");
    }

    protected void LoomBeamReceiveDetail()
    {
        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("LoomBeamReceiveDetail");

        //*************
        //***********
        sht.Row(1).Height = 24;
        sht.Range("A1:M1").Merge();
        sht.Range("A1:M1").Style.Font.FontSize = 10;
        sht.Range("A1:M1").Style.Font.Bold = true;
        sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A1:M1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("A1:M1").Style.Alignment.WrapText = true;
        //************
        sht.Range("A1").SetValue("Loom Beam ReceiveDetail From : " + txtfromdate.Text + " To : " + txttodate.Text + "");

        sht.Range("A2:M2").Style.Font.FontSize = 10;
        sht.Range("A2:M2").Style.Font.Bold = true;

        sht.Range("A2").Value = "Receive Date";
        sht.Range("B2").Value = "Unit Name";
        sht.Range("C2").Value = "Loom No";
        sht.Range("D2").Value = "Folio No";
        sht.Range("E2").Value = "Receive No";
        sht.Range("F2").Value = "Beam No";
        sht.Range("G2").Value = "Beam Description";
        sht.Range("H2").Value = "Lot No";
        sht.Range("I2").Value = "Tag No";
        sht.Range("J2").Value = "Weight";
        sht.Range("K2").Value = "RecBeam Pcs";
        sht.Range("L2").Value = "Emp Name";
        sht.Range("M2").Value = "User Name";

        int row = 3;

        string str = "";
        if (DDProductionUnit.SelectedIndex > 0)
        {
            str = str + " and PIM.Units=" + DDProductionUnit.SelectedValue;
        }
        if (DDLoomNo.SelectedIndex > 0)
        {
            str = str + " and PIM.LoomId=" + DDLoomNo.SelectedValue;
        }
        if (DDFolioNo.SelectedIndex > 0)
        {
            str = str + " and PRM.Prorderid =" + DDFolioNo.SelectedValue;
        }

        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and VF.Qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and VF.DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and VF.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            str = str + " and VF.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and VF.sizeid=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            str = str + " and VF.shadecolorid=" + DDShadeColor.SelectedValue;
        }

        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@Where", str);
        param[2] = new SqlParameter("@FromDate", txtfromdate.Text);
        param[3] = new SqlParameter("@Todate", txttodate.Text);
        //*********
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetLoomBeamReceiveDetailExcelReport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["ReceiveDate"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["UnitName"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["LoomNo"]);
                sht.Range("C" + row).Style.NumberFormat.Format = "@";
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["FolioChallanNo"]);
                sht.Range("D" + row).Style.NumberFormat.Format = "@";
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ChalanNo"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["BeamNo"]);

                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["BeamDescription"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["LotNo"]);
                sht.Range("H" + row).Style.NumberFormat.Format = "@";
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["TagNo"]);
                sht.Range("I" + row).Style.NumberFormat.Format = "@";
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Weight"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["BeamPcs"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["UserName"]);

                row = row + 1;
            }
            ds.Dispose();

        }

        using (var a = sht.Range("A1" + ":M" + row))
        {
            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        }

        //*************************************************
        String Path;
        sht.Columns(1, 26).AdjustToContents();
        string Fileextension = "xlsx";
        string filename = UtilityModule.validateFilename("LoomBeamReceiveDetail_" + DateTime.Now + "." + Fileextension);
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

    protected void LoomBeamPendingDetail()
    {
        DataSet DS = new DataSet();

        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("LoomBeamPendingDetail");

        //*************
        //***********
        sht.Row(1).Height = 24;
        sht.Range("A1:J1").Merge();
        sht.Range("A1:J1").Style.Font.FontSize = 10;
        sht.Range("A1:J1").Style.Font.Bold = true;
        sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A1:J1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("A1:J1").Style.Alignment.WrapText = true;
        //************
        sht.Range("A1").SetValue("Loom Beam PendingDetail From : " + txtfromdate.Text + " To : " + txttodate.Text + "");

        sht.Range("A2:J2").Style.Font.FontSize = 10;
        sht.Range("A2:J2").Style.Font.Bold = true;

        sht.Range("A2").Value = "Unit Name";
        sht.Range("B2").Value = "Loom No";
        sht.Range("C2").Value = "Folio No";
        sht.Range("D2").Value = "Beam Description";
        sht.Range("E2").Value = "Beam No";
        sht.Range("F2").Value = "Issued Pcs";
        sht.Range("G2").Value = "Weight";
        sht.Range("H2").Value = "Bazaar Pcs";
        sht.Range("I2").Value = "Beam RecPcs";
        sht.Range("J2").Value = "Beam BalPcs";

        int row = 3;

        string str = "";
        if (DDProductionUnit.SelectedIndex > 0)
        {
            str = str + " and PIM.Units=" + DDProductionUnit.SelectedValue;
        }
        if (DDLoomNo.SelectedIndex > 0)
        {
            str = str + " and PIM.LoomId=" + DDLoomNo.SelectedValue;
        }
        if (DDFolioNo.SelectedIndex > 0)
        {
            str = str + " and PRM.Prorderid =" + DDFolioNo.SelectedValue;
        }

        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and VF.Qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and VF.DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and VF.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            str = str + " and VF.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and VF.sizeid=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            str = str + " and VF.shadecolorid=" + DDShadeColor.SelectedValue;
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_GetLoomBeamPendingDetailExcelReport", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@Where", str);
        cmd.Parameters.AddWithValue("@FromDate", txtfromdate.Text);
        cmd.Parameters.AddWithValue("@Todate", txttodate.Text);

        // DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(DS);
        //*************

        con.Close();
        con.Dispose();
        //***********
        if (DS.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(DS.Tables[0].Rows[i]["UnitName"]);
                sht.Range("B" + row).SetValue(DS.Tables[0].Rows[i]["LoomNo"]);
                sht.Range("B" + row).Style.NumberFormat.Format = "@";
                sht.Range("C" + row).SetValue(DS.Tables[0].Rows[i]["FolioChallanNo"]);
                sht.Range("D" + row).SetValue(DS.Tables[0].Rows[i]["BeamDescription"]);
                sht.Range("E" + row).SetValue(DS.Tables[0].Rows[i]["BeamNo"]);
                sht.Range("F" + row).SetValue(DS.Tables[0].Rows[i]["BeamPcs"]);
                sht.Range("G" + row).SetValue(DS.Tables[0].Rows[i]["Weight"]);
                sht.Range("H" + row).SetValue(DS.Tables[0].Rows[i]["BazaarPcs"]);
                sht.Range("I" + row).SetValue(DS.Tables[0].Rows[i]["BeamReceivePcs"]);
                sht.Range("J" + row).SetValue((Convert.ToInt32(DS.Tables[0].Rows[i]["BeamPcs"]) - (Convert.ToInt32(DS.Tables[0].Rows[i]["BazaarPcs"]) + Convert.ToInt32(DS.Tables[0].Rows[i]["BeamReceivePcs"]))));

                row = row + 1;
            }
            DS.Dispose();

        }

        using (var a = sht.Range("A1" + ":J" + row))
        {
            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        }

        //*************************************************
        String Path;
        sht.Columns(1, 26).AdjustToContents();
        string Fileextension = "xlsx";
        string filename = UtilityModule.validateFilename("LoomBeamPendingDetail_" + DateTime.Now + "." + Fileextension);
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
    protected void WarpingBeamReceiveDetailWithRateMcNo()
    {
        string str = "";
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and VF.Qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and VF.DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and VF.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            str = str + " and VF.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and VF.sizeid=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            str = str + " and VF.shadecolorid=" + DDShadeColor.SelectedValue;
        }

        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@ProcessID", DDProcess.SelectedValue);
        param[2] = new SqlParameter("@Where", str);
        param[3] = new SqlParameter("@FromDate", txtfromdate.Text);
        param[4] = new SqlParameter("@Todate", txttodate.Text);
        //*********
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetWarpingBeamReceiveDetailWithRateMachineNo", param);
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
            //**************
            sht.Columns("A").Width = 15.78;
            sht.Columns("B").Width = 15.78;
            sht.Columns("C").Width = 35.78;

            //Headers
            sht.Range("A1").Value = "Receive Date";
            sht.Range("B1").Value = "Warper Name";

            sht.Range("C1").Value = "ItemDescription";
            sht.Range("D1").Value = "UnitName";
            sht.Range("E1").Value = "LotNo";
            sht.Range("F1").Value = "TagNo";
            sht.Range("G1").Value = "PCSBEAM";
            sht.Range("H1").Value = "GodownName";
            sht.Range("I1").Value = "IssueNo";
            sht.Range("J1").Value = "BeamNo";
            sht.Range("K1").Value = "Warping M/C No";
            sht.Range("L1").Value = "Rate";
            sht.Range("M1").Value = "Amount";
            sht.Range("A1:M1").Style.Font.Bold = true;
            sht.Range("G1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("I1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            //******************************
            row = 2;
            decimal Bal = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                //sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;
                sht.Range("A" + row + ":L" + row).Style.Alignment.SetWrapText();

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["ReceiveDate"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);

                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ItemDescription"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["UnitName"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["LotNo"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["TagNo"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["PCSBEAM"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["GodownName"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["IssueNo"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["BeamNo"]);
                sht.Range("J" + row).Style.NumberFormat.Format = "@";
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["MCNo"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["Amount"]);


                row = row + 1;
            }
            //**********Total
            sht.Columns(4, 30).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("BeamReceiveDetailWithRate_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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

            #region

            //ds.Tables[0].Columns.Remove("ID");
            //ds.Tables[0].Columns.Remove("DetailID");
            ////Export to excel
            //GridView GridView1 = new GridView();
            //GridView1.AllowPaging = false;

            //GridView1.DataSource = ds;
            //GridView1.DataBind();
            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition",
            // "attachment;filename=BeamReceiveDetail" + DateTime.Now + ".xls");
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);

            //for (int i = 0; i < GridView1.Rows.Count; i++)
            //{
            //    //Apply text style to each Row
            //    GridView1.Rows[i].Attributes.Add("class", "textmode");                 
            //}
            //GridView1.RenderControl(hw);

            ////style to format numbers to string
            //string style = @"<style> .textmode { mso-number-format:\@; } </style>";

            //Response.Write(style);
            //Response.Output.Write(sw.ToString());
            //Response.Flush();
            //Response.End();
            ////*************
            #endregion

        }
    }

    protected void WarpingMaterialissueDetail()
    {
        string Filterby = "Filter By-From:" + txtfromdate.Text + " To: " + txttodate.Text + "";
        string str = "";
        int Row;
//        string str = @"select WIM.IssueDate,D.DepartmentName,vf.QualityName,vf.ShadeColorName,WID.LotNo,WID.TagNo,Sum(WID.IssueQty) as Qty,WIM.ID as IssueNo,
//                    WOM.Issueno as WarpOrderNO,ROUND(ISNULL(WR.NETWT,0),3) AS RECWT
//                    From WarpRawIssueMaster  WIM  inner join WarpRawissueDetail WID on WIM.id=WID.Id
//                    inner join warpordermaster WOM on Wid.issuemasterid = WOM.ID
//                    inner join  V_FinishedItemDetail vf on WID.Ifinishedid=vf.ITEM_FINISHED_ID
//                    inner join Department D on WIM.Deptid=D.DepartmentId
//                    inner join CompanyInfo ci on WIM.companyid=Ci.companyid 
//                    LEFT JOIN V_WARPNETWTRECEIVE_ISSUENOWISE WR ON WOM.ID=WR.ISSUEMASTERID
//                    WHere WIM.CompanyId=" + DDCompany.SelectedValue + " and WIM.issuedate>='" + txtfromdate.Text + "' and WIM.issuedate<='" + txttodate.Text + "'";
//        if (DDProcess.SelectedIndex > 0)
//        {
//            str = str + " and WoM.Processid=" + DDProcess.SelectedValue;
//            Filterby = Filterby + "-" + "Process :" + DDProcess.SelectedItem.Text;
//        }
//        if (ddItemName.SelectedIndex > 0)
//        {
//            str = str + " and vf.ITEM_ID=" + ddItemName.SelectedValue;
//            Filterby = Filterby + "-" + "ItemName :" + ddItemName.SelectedItem.Text;
//        }
//        if (DDQuality.SelectedIndex > 0)
//        {
//            str = str + " and vf.Qualityid=" + DDQuality.SelectedValue;
//            Filterby = Filterby + "-" + "Quality :" + DDQuality.SelectedItem.Text;
//        }
//        if (DDShadeColor.SelectedIndex > 0)
//        {
//            str = str + " and vf.shadecolorid=" + DDShadeColor.SelectedValue;
//            Filterby = Filterby + "-" + "ShadeNo :" + DDShadeColor.SelectedItem.Text;
//        }
//        if (txtLotno.Text != "")
//        {
//            str = str + " and WID.Lotno='" + txtLotno.Text + "'";
//            Filterby = Filterby + "-" + "LotNo :" + txtLotno.Text;
//        }
//        if (txtTagno.Text != "")
//        {
//            str = str + " and WID.Tagno='" + txtTagno.Text + "'";
//            Filterby = Filterby + "-" + "TagNo :" + txtTagno.Text;
//        }

//        str = str + " group by WIM.IssueDate,D.DepartmentName,vf.QualityName,vf.ShadeColorName,WID.LotNo,WID.TagNo,WIM.ID,Wom.issueno,WR.NETWT order by WIM.issuedate";
//        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);


         if (DDProcess.SelectedIndex > 0)
         {
             str = str + " and WoM.Processid=" + DDProcess.SelectedValue;
             Filterby = Filterby + "-" + "Process :" + DDProcess.SelectedItem.Text;
         }
         if (ddItemName.SelectedIndex > 0)
         {
             str = str + " and vf.ITEM_ID=" + ddItemName.SelectedValue;
             Filterby = Filterby + "-" + "ItemName :" + ddItemName.SelectedItem.Text;
         }
         if (DDQuality.SelectedIndex > 0)
         {
             str = str + " and vf.Qualityid=" + DDQuality.SelectedValue;
             Filterby = Filterby + "-" + "Quality :" + DDQuality.SelectedItem.Text;
         }
         if (DDShadeColor.SelectedIndex > 0)
         {
             str = str + " and vf.shadecolorid=" + DDShadeColor.SelectedValue;
             Filterby = Filterby + "-" + "ShadeNo :" + DDShadeColor.SelectedItem.Text;
         }
         if (txtLotno.Text != "")
         {
             str = str + " and WID.Lotno='" + txtLotno.Text + "'";
             Filterby = Filterby + "-" + "LotNo :" + txtLotno.Text;
         }
         if (txtTagno.Text != "")
         {
             str = str + " and WID.Tagno='" + txtTagno.Text + "'";
             Filterby = Filterby + "-" + "TagNo :" + txtTagno.Text;
         }

        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@ProcessID", DDProcess.SelectedValue);
        param[2] = new SqlParameter("@Where", str);
        param[3] = new SqlParameter("@FromDate", txtfromdate.Text);
        param[4] = new SqlParameter("@Todate", txttodate.Text);
        //*********
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetWarpingMaterialIssueDetailReport", param);
        

        if (ds.Tables[0].Rows.Count > 0)
        {
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("WarpingMaterialIssueDetails");
            //****************
            //****************
            sht.Range("A1:K1").Merge();
            sht.Range("A1:K1").Style.Font.FontSize = 11;
            sht.Range("A1:K1").Style.Font.Bold = true;
            sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:K1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1").SetValue("WARPING MATERIAL ISSUE DETAIL");
            sht.Row(1).Height = 21.75;

            sht.Range("A2:K2").Merge();
            sht.Range("A2:K2").Style.Font.FontSize = 11;
            sht.Range("A2:K2").Style.Font.Bold = true;
            sht.Range("A2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:K2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2").SetValue(Filterby);
            sht.Row(2).Height = 21.75;

            sht.Range("A3:K3").Style.Font.FontSize = 11;
            sht.Range("A3:K3").Style.Font.Bold = true;
            sht.Range("A3:K3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("G3:I3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Row(3).Height = 18.00;
            //
            sht.Range("A3").SetValue("Date");
            sht.Range("B3").SetValue("Department");
            sht.Range("C3").SetValue("Process");
            sht.Range("D3").SetValue("Description");

            sht.Range("E3").SetValue("Quality");
            sht.Range("F3").SetValue("ShadeNo.");
            sht.Range("G3").SetValue("LotNo.");
            sht.Range("H3").SetValue("TagNo.");
            sht.Range("I3").SetValue("Pcs");
            sht.Range("J3").SetValue("Qty");  
            sht.Range("K3").SetValue("Issue Challan No.");
            sht.Range("L3").SetValue("Warp Order No.");
            Row = 4;
           
            int rowcount = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rowcount; i++)
            {
                sht.Range("J" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A" + Row + ":H" + Row).Style.Font.FontSize = 11;

                sht.Range("A" + Row).SetValue(ds.Tables[0].Rows[i]["Issuedate"]);
                sht.Range("B" + Row).SetValue(ds.Tables[0].Rows[i]["Departmentname"]);
                sht.Range("C" + Row).SetValue(ds.Tables[0].Rows[i]["ProcessName"]);
                sht.Range("D" + Row).SetValue(ds.Tables[0].Rows[i]["ItemDescription"]);

                sht.Range("E" + Row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("F" + Row).SetValue(ds.Tables[0].Rows[i]["Shadecolorname"]);
                sht.Range("G" + Row).SetValue(ds.Tables[0].Rows[i]["Lotno"]);
                sht.Range("H" + Row).SetValue(ds.Tables[0].Rows[i]["Tagno"]);
                sht.Range("I" + Row).SetValue(ds.Tables[0].Rows[i]["BeamPcs"]);
                sht.Range("J" + Row).SetValue(ds.Tables[0].Rows[i]["Qty"]);              
                sht.Range("K" + Row).SetValue(ds.Tables[0].Rows[i]["IssueNO"]);
                sht.Range("L" + Row).SetValue(ds.Tables[0].Rows[i]["Warporderno"]);
                Row = Row + 1;

            }
            //**********Total
            //var issued = sht.Evaluate("SUM(G4:G" + (Row - 1) + ")");
            //*************
            sht.Columns(1, 15).AdjustToContents();
            //sht.Range("G" + Row).SetValue(issued);
            sht.Range("J" + Row).FormulaA1 = "=SUM(J4:J" + (Row - 1) + ")";           
            //**************Save
            //******SAVE FILE
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("WARPINGMATERIALISSUEDETAIL-" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "al", "alert('No records found..')", true);
        }
    }


    protected void WarpingOrderWithBeamFolioDetail()
    {
        DataSet ds = new DataSet();
        string str = "";
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and VF.Qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and VF.DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and VF.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            str = str + " and VF.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and VF.sizeid=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            str = str + " and VF.shadecolorid=" + DDShadeColor.SelectedValue;
        }

        //SqlParameter[] param = new SqlParameter[4];
        //param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        //param[1] = new SqlParameter("@Id", DDWarpingIssueNo.SelectedValue);
        //param[2] = new SqlParameter("@Where", str);
        ////param[3] = new SqlParameter("@FromDate", txtfromdate.Text);
        ////param[4] = new SqlParameter("@Todate", txttodate.Text);
        ////*********
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetWarpingOrderWithBeam_FolioDetail", param);


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_GetWarpingOrderWithBeam_FolioDetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@Id", DDWarpingIssueNo.SelectedValue);
        cmd.Parameters.AddWithValue("@Where", str);
        //cmd.Parameters.AddWithValue("@FromDate", txtfromdate.Text);
        //cmd.Parameters.AddWithValue("@Todate", txttodate.Text);

        // DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();
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
            //**************
            sht.Columns("A").Width = 11.67;
            sht.Columns("B").Width = 40.78;
            sht.Columns("C").Width = 35.78;
            sht.Columns("D").Width = 35.78;
            sht.Columns("E").Width = 16.15;
            sht.Columns("F").Width = 8.22;
            sht.Columns("G").Width = 11.67;
            sht.Columns("H").Width = 11.67;
            sht.Columns("I").Width = 11.67;
            sht.Columns("J").Width = 8.67;
            sht.Columns("K").Width = 10.67;
            sht.Columns("L").Width = 12.67;
            sht.Columns("M").Width = 10.67;
            sht.Columns("N").Width = 9.33;
            sht.Columns("O").Width = 10.33;
            sht.Columns("P").Width = 10.33;
          

            //Headers
            sht.Range("A1").Value = "Issue Date";
            sht.Range("B1").Value = "Beam Description";
            sht.Range("C1").Value = "LotNo";
            sht.Range("D1").Value = "TagNo";
            sht.Range("E1").Value = "Warping IssueNo";
            sht.Range("F1").Value = "BeamNo";
            sht.Range("G1").Value = "Gross Weight";
            sht.Range("H1").Value = "Tare Weight";
            sht.Range("I1").Value = "Net Weight";
            sht.Range("J1").Value = "BeamPcs";
            sht.Range("K1").Value = "Unit Name";
            sht.Range("L1").Value = "LoomNo";
            sht.Range("M1").Value = "Folio No";
            sht.Range("N1").Value = "Folio Qty";
            sht.Range("O1").Value = "Bazaar Qty";
            sht.Range("P1").Value = "Pending Qty";            
            sht.Range("A1:P1").Style.Font.Bold = true;
            //sht.Range("G1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            //sht.Range("I1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
          
            row = 2;
            decimal Bal = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                //sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;
                sht.Range("A" + row + ":P" + row).Style.Alignment.SetWrapText();

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["IssueDate"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["BeamDescription"]);

                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["LotNo"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["TagNo"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["IssueNo"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["BeamNo"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Grossweight"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["TareWeight"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["NetWeight"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["pcs"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["UnitName"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["LoomNo"]);
                sht.Range("L" + row).Style.NumberFormat.Format = "@";
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["IssueOrderID"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["POrderQty"]);
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["BazaarQty"]);
                sht.Range("P" + row).SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["POrderQty"]) - Convert.ToDouble(ds.Tables[0].Rows[i]["BazaarQty"]));
               
                row = row + 1;
            }
            ////**********Total
            //sht.Columns(4, 30).AdjustToContents();
            ////********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("WarpingOrderWithBeamFolioDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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

            #region

            //ds.Tables[0].Columns.Remove("ID");
            //ds.Tables[0].Columns.Remove("DetailID");
            ////Export to excel
            //GridView GridView1 = new GridView();
            //GridView1.AllowPaging = false;

            //GridView1.DataSource = ds;
            //GridView1.DataBind();
            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition",
            // "attachment;filename=BeamReceiveDetail" + DateTime.Now + ".xls");
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);

            //for (int i = 0; i < GridView1.Rows.Count; i++)
            //{
            //    //Apply text style to each Row
            //    GridView1.Rows[i].Attributes.Add("class", "textmode");                 
            //}
            //GridView1.RenderControl(hw);

            ////style to format numbers to string
            //string style = @"<style> .textmode { mso-number-format:\@; } </style>";

            //Response.Write(style);
            //Response.Output.Write(sw.ToString());
            //Response.Flush();
            //Response.End();
            ////*************
            #endregion

        }
    }
}