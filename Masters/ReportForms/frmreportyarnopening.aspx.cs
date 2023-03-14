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

public partial class Masters_ReportForms_frmreportyarnopening : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.COmpanyid And CA.Userid=" + Session["varuserid"] + @"                           
                           select Ei.EmpId,Ei.EmpName+case when Ei.empcode<>'' Then '['+Ei.empcode+']' Else '' End as Empname from EmpInfo EI inner join Department D on EI.Departmentid=D.DepartmentId 
                           and D.DepartmentName='YARN OPENING' and Ei.mastercompanyid=" + Session["varcompanyid"] + @" order by EmpName
                           select customerid,CustomerCode+'  '+companyname as customer from customerinfo WHere mastercompanyid=" + Session["varcompanyid"] + " order by customer";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            UtilityModule.NewChkBoxListFillWithDs(ref chkemp, ds, 1);
            UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds, 2, true, "--Plz Select--");
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (variable.VarYARNOPENINGISSUEEMPWISE == "1")
            {
                Trdeptname.Visible = true;
                UtilityModule.ConditionalComboFill(ref DDdeptname, "SELECT D.DepartmentId,D.DepartmentName FROM DEPARTMENT D WHERE  D.DEPARTMENTNAME IN('YARN OPENING','WEFT DEPARTMENT')", true, "--Plz Select--");

            }
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        if (RDreceive.Checked == true)
        {
            Receivedetail();
        }
        else if (RDLoss.Checked == true)
        {
            LossDetail();
        }
        else if (RDissuerecdetail.Checked == true)
        {
            if (Session["varCompanyId"].ToString() == "21")
            {
                IssuereceivedetailKaysons();
            }
            else
            {
                Issuereceivedetail();
            }
        }
    }
    protected void Receivedetail()
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string empid = "";
            string Filterby = "";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < chkemp.Items.Count; i++)
            {
                if (chkemp.Items[i].Selected)
                {
                    sb.Append(chkemp.Items[i].Value + ",");
                }
            }
            empid = sb.ToString().TrimEnd(',');
            //
            if (empid == "")
            {
                for (int i = 0; i < chkemp.Items.Count; i++)
                {

                    sb.Append(chkemp.Items[i].Value + ",");

                }
                empid = sb.ToString().TrimEnd(',');
            }
            //Date flag
            int Dateflag = 0;
            if (chkdate.Checked == true)
            {
                Dateflag = 1;
            }
            string str = "select *," + Dateflag + " as Dateflag,'" + txtfromdate.Text + "' as FromDate,'" + txttodate.Text + "' as Todate from V_yarnOpeningrecDetail where companyid=" + DDcompany.SelectedValue;
            if (DDdeptname.SelectedIndex > 0)
            {
                str = str + " and Departmentid=" + DDdeptname.SelectedValue;
            }
            if (empid != "")
            {
                str = str + " and empid in(" + empid + ")";
            }
            if (Ddemplocation.SelectedIndex > 0)
            {
                str = str + " and EMPLOCATION=" + Ddemplocation.SelectedValue;
                Filterby = Filterby + " EMPLOCATION " + Ddemplocation.SelectedItem.Text;
            }
            if (chkdate.Checked == true)
            {
                str = str + " and Receivedate>='" + txtfromdate.Text + "'   and Receivedate<='" + txttodate.Text + "'";
                Filterby = Filterby + ", Receivedate FROM : " + txtfromdate.Text + "  and Receivedate To  : " + txttodate.Text;
            }
            if (txtLotno.Text != "")
            {
                str = str + " and Lotno='" + txtLotno.Text + "'";
                Filterby = Filterby + ", Lotno- " + txtLotno.Text;
            }
            if (txtTagno.Text != "")
            {
                str = str + " and Tagno='" + txtTagno.Text + "'";
                Filterby = Filterby + ", Tag No- " + txtTagno.Text;
            }
            if (DDcustcode.SelectedIndex > 0)
            {
                str = str + "  and customerid=" + DDcustcode.SelectedValue;
                Filterby = Filterby + ", Customer code- " + DDcustcode.SelectedItem.Text;
            }
            if (DDorderno.SelectedIndex > 0)
            {
                str = str + "  and orderid=" + DDorderno.SelectedValue;
                Filterby = Filterby + ", Order No.- " + DDorderno.SelectedItem.Text;
            }
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (chkexcelexport.Checked == true)
            {
                if (chksummary.Checked == true)
                {
                    ReceiveexcelexportSummary(ds, filterby: Filterby);
                    return;
                }
                else
                {
                    Receiveexcelexport(ds, filterby: Filterby);
                    return;
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (chksummary.Checked == true)
                {
                    Session["rptFileName"] = "~\\Reports\\rptyarnopeningrecDetailSummary.rpt";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\rptyarnopeningrecDetail.rpt";
                }
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptyarnopeningrecDetail.xsd";
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
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void Receiveexcelexport(DataSet ds, string filterby = "")
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("YOP");
            //**************
            sht.Range("A1:M1").Merge();
            sht.Range("A1:M1").Style.Font.FontSize = 11;
            sht.Range("A1:M1").Style.Font.Bold = true;
            sht.Range("A1:M1").Style.NumberFormat.Format = "@";
            sht.Range("A1:M1").Style.Alignment.WrapText = true;
            sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:M1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1").Value = DDcompany.SelectedItem.Text + " (YOP RECEIVE REPORT) " + filterby;
            sht.Row(1).Height = 24.00;
            //                     
            sht.Range("A2:M2").Style.Font.FontSize = 11;

            using (var a = sht.Range("A2:M2"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            sht.Range("A2:M2").Style.Font.Bold = true;
            sht.Range("I2:M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            sht.Range("A2").Value = "Date";
            sht.Range("B2").Value = "Department Name";
            sht.Range("C2").Value = "Employee";
            sht.Range("D2").Value = "Emp Code";
            sht.Range("E2").Value = "Item Description";
            sht.Range("F2").Value = "Shade No.";
            sht.Range("G2").Value = "Lot No.";
            sht.Range("H2").Value = "Tag No.";
            sht.Range("I2").Value = "Wt.(kg)";
            sht.Range("J2").Value = "No of Cones";

            if (Session["varcompanyId"].ToString() == "21")
            {
                sht.Column(11).Hide();
                sht.Column(12).Hide();
            }
            else
            {
                sht.Range("K2").Value = "Rate";
                sht.Range("L2").Value = "Amount";
                sht.Range("M2").Value = "Party ChallanNo";
            }

            //***********************
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "Receivedate";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            int row = 3;
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                using (var a = sht.Range("A" + row + ":M" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Receivedate"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Department"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Empname"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["empcode"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Item_Name"].ToString() + ' ' + ds1.Tables[0].Rows[i]["QualityName"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Shadecolorname"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["TagNo"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Recqty"]);
                sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Noofcone"]);
                if (Session["varcompanyId"].ToString() != "21")
                {
                    sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);
                    sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["Amount"]);
                    sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["PartyChallanNo"]);
                }

                row = row + 1;
            }
            //********Grand Total
            var Received = sht.Evaluate("SUM(I3:I" + (row - 1) + ")");
            var noofcone = sht.Evaluate("SUM(J3:J" + (row - 1) + ")");


            sht.Range("I" + row).SetValue(Received);
            sht.Range("J" + row).SetValue(noofcone);
            sht.Range("L" + row).FormulaA1 = "SUM(L3:L" + (row - 1) + ")";

            using (var a = sht.Range("A" + row + ":M" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //********************
            sht.Columns(1, 30).AdjustToContents();
            //********************


            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("YOPRECIVE_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();

        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }
    }
    protected void ReceiveexcelexportSummary(DataSet ds, string filterby = "")
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("YOP");
            //**************
            sht.Range("A1:G1").Merge();
            sht.Range("A1:G1").Style.Font.FontSize = 11;
            sht.Range("A1:G1").Style.Font.Bold = true;
            sht.Range("A1:G1").Style.NumberFormat.Format = "@";
            sht.Range("A1:G1").Style.Alignment.WrapText = true;
            sht.Range("A1:G1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:G1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1").Value = DDcompany.SelectedItem.Text + " (YOP RECEIVE SUMMARY REPORT) " + filterby;
            sht.Row(1).Height = 24.00;
            //                     
            sht.Range("A2:G2").Style.Font.FontSize = 11;

            using (var a = sht.Range("A2:G2"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            sht.Range("A2:G2").Style.Font.Bold = true;
            sht.Range("F2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            sht.Range("A2").Value = "Date";
            sht.Range("B2").Value = "Item Description";
            sht.Range("C2").Value = "Shade No.";
            sht.Range("D2").Value = "Lot No.";
            sht.Range("E2").Value = "Tag No.";
            sht.Range("F2").Value = "Wt.(kg)";
            sht.Range("G2").Value = "No of Cones";

            //***********************
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "Receivedate";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            int row = 3;
            DataTable dtdistinctrecdate = ds1.Tables[0].DefaultView.ToTable(true, "Receivedate");
            DataView dvrecdate = new DataView(dtdistinctrecdate);
            dvrecdate.Sort = "receivedate";
            DataTable dtdistinct1 = dvrecdate.ToTable();
            foreach (DataRow dr in dtdistinct1.Rows)
            {
                sht.Range("A" + row).SetValue(dr["Receivedate"]);
                DataView dvitemdesc = new DataView(ds1.Tables[0]);
                dvitemdesc.RowFilter = "RECEIVEDATE='" + dr["Receivedate"] + "'";
                DataSet dsitemdesc = new DataSet();
                dsitemdesc.Tables.Add(dvitemdesc.ToTable());
                DataTable dtdistinctitemdesc = dsitemdesc.Tables[0].DefaultView.ToTable(true, "Item_name", "Qualityname", "Shadecolorname", "Lotno", "TagNo");
                Decimal tqty = 0;
                Decimal tNoofcone = 0;
                foreach (DataRow dritemdesc in dtdistinctitemdesc.Rows)
                {
                    row = row + 1;
                    sht.Range("B" + row).SetValue(dritemdesc["Item_Name"].ToString() + ' ' + dritemdesc["QualityName"]);
                    sht.Range("C" + row).SetValue(dritemdesc["Shadecolorname"]);
                    sht.Range("D" + row).SetValue(dritemdesc["Lotno"]);
                    sht.Range("E" + row).SetValue(dritemdesc["TagNo"]);
                    var qty = ds1.Tables[0].Compute("sum(Recqty)", "Receivedate='" + dr["receivedate"] + "' and Item_name='" + dritemdesc["Item_name"] + "' and QualityName='" + dritemdesc["QualityName"] + "' and Shadecolorname='" + dritemdesc["shadecolorname"] + "' and Lotno='" + dritemdesc["Lotno"] + "' and TagNo='" + dritemdesc["Tagno"] + "'");
                    var noofcone = ds1.Tables[0].Compute("sum(noofcone)", "Receivedate='" + dr["receivedate"] + "' and Item_name='" + dritemdesc["Item_name"] + "' and QualityName='" + dritemdesc["QualityName"] + "' and Shadecolorname='" + dritemdesc["shadecolorname"] + "' and Lotno='" + dritemdesc["Lotno"] + "' and TagNo='" + dritemdesc["Tagno"] + "'");
                    tqty = tqty + Convert.ToDecimal(qty);
                    tNoofcone = tNoofcone + Convert.ToDecimal(noofcone);
                    sht.Range("F" + row).SetValue(qty);
                    sht.Range("G" + row).SetValue(noofcone);
                }
                row = row + 1;
                sht.Range("E" + row).SetValue("TOTAL");
                sht.Range("F" + row).SetValue(tqty);
                sht.Range("G" + row).SetValue(tNoofcone);
                row = row + 1;
            }


            //for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            //{
            //    using (var a = sht.Range("A" + row + ":I" + row))
            //    {
            //        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //    }
            //    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Receivedate"]);
            //    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Empname"]);
            //    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["empcode"]);
            //    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Item_Name"].ToString() + ' ' + ds1.Tables[0].Rows[i]["QualityName"]);
            //    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Shadecolorname"]);
            //    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
            //    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["TagNo"]);
            //    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Recqty"]);
            //    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Noofcone"]);
            //    row = row + 1;
            //}
            //********Grand Total            
            using (var a = sht.Range("A3:G" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //********************
            sht.Columns(1, 30).AdjustToContents();
            //********************


            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("YOPRECEIVESUMMARY_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();

        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }
    }
    protected void LossDetail()
    {
        string empid = "", title = "";
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < chkemp.Items.Count; i++)
        {
            if (chkemp.Items[i].Selected)
            {
                sb.Append(chkemp.Items[i].Value + ",");
            }
        }
        empid = sb.ToString().TrimEnd(',');
        //*******************
        string str = @"select  CI.CompanyName,vf.ITEM_NAME,vf.qualityname,vf.ShadeColorName,YIT.Lotno,YIT.Tagno,
                        Sum(YIT.issueqty-isnull(VRT.Retqty,0)) as Issueqty,Sum(YCS.LossQty) as Lossqty,sum(YCS.GainQty) as Gainqty,isnull(sum(VR.ReceivedQty),0) as Recqty,'" + txtfromdate.Text + "' as FromDate,'" + txttodate.Text + @"'
                        From YarnOpeningIssueMaster YIM inner join YarnOpeningIssueTran YIT
                        on YIM.ID=YIT.MasterId 
                        INNER JOIN EMPINFO EI ON YIM.VENDORID=EI.EMPID
                        inner join V_FinishedItemDetail vf on YIT.Item_Finished_id=vf.ITEM_FINISHED_ID
                        inner join YarnOpeningCompleteStatus YCS on YIT.Detailid=YCS.DetailId
                        left join V_getYarnOpeningReceivedQty VR on YIT.Detailid=VR.issuemasterDetailid 
                        inner join CompanyInfo CI on YIM.CompanyId=CI.CompanyId
                        left join V_getYarnOpeningReturnQty VRT on YIT.detailid=VRT.Issuedetailid
                        left join ordermaster om on YIT.orderid=Om.orderid
                        Where YIM.CompanyId=" + DDcompany.SelectedValue + "  and cast(Replace(convert(nvarchar(11),YCS.Dateadded,106),' ','-') as Datetime)>='" + txtfromdate.Text + "' and cast(Replace(convert(nvarchar(11),YCS.Dateadded,106),' ','-')as Datetime)<='" + txttodate.Text + "'";
        if (empid != "")
        {
            str = str + " and YIM.vendorid in(" + empid + ")";
        }
        if (Ddemplocation.SelectedIndex > 0)
        {
            str = str + " and isnull(EI.EMPLOYEETYPE,0)=" + Ddemplocation.SelectedValue;
        }
        if (txtLotno.Text != "")
        {
            str = str + " and YIT.Lotno='" + txtLotno.Text + "'";
            title = title + " Lot No-" + txtLotno.Text + "";
        }
        if (txtTagno.Text != "")
        {
            str = str + " and YIT.Tagno='" + txtTagno.Text + "'";
            title = title + " Tag No-" + txtTagno.Text + "";
        }
        if (DDcustcode.SelectedIndex > 0)
        {
            str = str + " and OM.customerid=" + DDcustcode.SelectedValue;
            title = title + " customer -" + DDcustcode.SelectedItem.Text + "";
        }
        if (DDorderno.SelectedIndex > 0)
        {
            str = str + " and YIT.orderid=" + DDorderno.SelectedValue;
            title = title + " OrderNo -" + DDorderno.SelectedItem.Text + "";
        }
        str = str + "  group by CI.CompanyName,vf.ITEM_NAME,vf.qualityname,vf.ShadeColorName,YIT.Lotno,YIT.Tagno";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("YOP");
            //**************
            sht.Range("A1:I1").Merge();
            sht.Range("A1:I1").Style.Font.FontName = "Tahoma";
            sht.Range("A1:I1").Style.Font.FontSize = 10;
            sht.Range("A1:I1").Style.Font.Bold = true;
            sht.Range("A1:I1").Style.NumberFormat.Format = "@";
            sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:I1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1").Value = ds.Tables[0].Rows[0]["CompanyName"] + " (YOP LOSS_GAIN REPORT)  From " + txtfromdate.Text + " To " + txttodate.Text + "  " + title;
            sht.Row(1).Height = 24.00;
            //         
            sht.Range("A2:I2").Style.Font.FontName = "Tahoma";
            sht.Range("A2:I2").Style.Font.FontSize = 10;
            sht.Range("A2:I2").Style.Font.Bold = true;
            sht.Range("A2:I2").Style.NumberFormat.Format = "@";
            sht.Range("E2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A2").Value = "ITEM_NAME";
            sht.Range("B2").Value = "SHADE NO";
            sht.Range("C2").Value = "LOT NO";
            sht.Range("D2").Value = "TAG NO";
            sht.Range("E2").Value = "ISSUE QTY";
            sht.Range("F2").Value = "REC. QTY";
            sht.Range("G2").Value = "GAIN QTY";
            sht.Range("H2").Value = "LOSS QTY";
            sht.Range("I2").Value = "STATUS";
            //***********************
            int i;
            i = 3;
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                sht.Range("A" + i).SetValue(ds.Tables[0].Rows[j]["QualityName"]);
                sht.Range("B" + i).SetValue(ds.Tables[0].Rows[j]["shadecolorname"]);
                sht.Range("C" + i).SetValue(ds.Tables[0].Rows[j]["Lotno"]);
                sht.Range("D" + i).SetValue(ds.Tables[0].Rows[j]["Tagno"]);
                sht.Range("E" + i).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[j]["issueqty"]));
                sht.Range("F" + i).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[j]["Recqty"]));
                sht.Range("G" + i).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[j]["gainqty"]));
                sht.Range("H" + i).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[j]["Lossqty"]));
                sht.Range("I" + i).SetValue("Complete");

                sht.Range("A" + i + ":I" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":I" + i).Style.Font.FontSize = 10;
                sht.Range("A" + i + ":D" + i).Style.NumberFormat.Format = "@";
                sht.Range("E" + i + ":H" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("E" + i + ":H" + i).Style.NumberFormat.Format = "#,###0.000";
                i = i + 1;
            }

            sht.Range("F" + i).Value = "Total";
            sht.Range("H" + i).Value = ds.Tables[0].Compute("Sum(Lossqty)", "");
            sht.Range("G" + i).Value = ds.Tables[0].Compute("Sum(gainqty)", "");

            sht.Range("G" + i + ":H" + i).Style.Font.FontName = "Tahoma";
            sht.Range("G" + i + ":H" + i).Style.Font.FontSize = 10;
            sht.Range("G" + i + ":H" + i).Style.Font.Bold = true;
            sht.Range("G" + i + ":H" + i).Style.NumberFormat.Format = "#,###0.000";
            sht.Range("G" + i + ":H" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            sht.Columns(1, 30).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("YOPLOSS_GAIN_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();

        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }
    }
    protected void Issuereceivedetail()
    {
        string empid = "", Filterby = "";
        Decimal Bal = 0;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < chkemp.Items.Count; i++)
        {
            if (chkemp.Items[i].Selected)
            {
                sb.Append(chkemp.Items[i].Value + ",");
            }
        }
        empid = sb.ToString().TrimEnd(',');

        string str = "";
        if (chksummary.Checked == true)
        {
            str = @"Select a.EMPNAME, a.ITEM_NAME + ' ' + a.QUALITYNAME Item_Name, a.UNITNAME, Sum(a.ISSUEQTY - a.RETQTY) ISSUEQTY, Sum(a.RECEIVEDQTY) RECEIVEDQTY, Sum(a.LOSSQTY) LOSSQTY, Sum(a.GAINQTY) GAINQTY, a.RATE  
            From V_Yarnopeningissuereceivedetail a(Nolock) 
            Where a.Companyid=" + DDcompany.SelectedValue;
        }
        else if (ChkSummaryForPayment.Checked == true)
        {
            str = @"Select a.EMPNAME, '' Item_Name, a.UNITNAME, Sum(a.ISSUEQTY - a.RETQTY) ISSUEQTY, Sum(a.RECEIVEDQTY) RECEIVEDQTY, Sum(a.LOSSQTY) LOSSQTY, Sum(a.GAINQTY) GAINQTY, a.RATE  
            From V_Yarnopeningissuereceivedetail a(Nolock) 
            Where a.Companyid=" + DDcompany.SelectedValue;
        }
        else
        {
            str = "select * From V_Yarnopeningissuereceivedetail Where companyid=" + DDcompany.SelectedValue;
            if (DDStatus.SelectedIndex > 0)
            {
                str = str + " and Status='" + DDStatus.SelectedItem.Text + "'";
            }
        }
        
        if (DDdeptname.SelectedIndex > 0)
        {
            str = str + " and Departmentid=" + DDdeptname.SelectedValue;
        }
        if (empid != "")
        {
            str = str + " and vendorid in(" + empid + ")";
        }
        if (Ddemplocation.SelectedIndex > 0)
        {
            str = str + " and emplocation=" + Ddemplocation.SelectedValue;
        }
        if (chkdate.Checked == true)
        {
            str = str + "  and Issuedate>='" + txtfromdate.Text + "' and Issuedate<='" + txttodate.Text + "'";
            Filterby = Filterby + " From Date-" + txtfromdate.Text + " and To Date-" + txttodate.Text;
        }
        if (txtLotno.Text != "")
        {
            str = str + " and Lotno='" + txtLotno.Text + "'";
            Filterby = Filterby + " Lot No-" + txtLotno.Text + "";
        }
        if (txtTagno.Text != "")
        {
            str = str + " and Tagno='" + txtTagno.Text + "'";
            Filterby = Filterby + " Tag No-" + txtTagno.Text + "";
        }
        if (DDcustcode.SelectedIndex > 0)
        {
            str = str + " and customerid=" + DDcustcode.SelectedValue;
            Filterby = Filterby + " Customer-" + DDcustcode.SelectedItem.Text + "";
        }
        if (DDorderno.SelectedIndex > 0)
        {
            str = str + " and Orderid=" + DDorderno.SelectedValue;
            Filterby = Filterby + " OrderNo-" + DDorderno.SelectedItem.Text + "";
        }

        if (chksummary.Checked == true)
        {
            str = str + @" Group By a.EMPNAME, a.ITEM_NAME, a.QUALITYNAME, a.UNITNAME, a.RATE Order BY a.EMPNAME, a.ITEM_NAME + ' ' + a.QUALITYNAME";
        }
        else if (ChkSummaryForPayment.Checked == true)
        {
            str = str + @" Group By a.EMPNAME, a.UNITNAME, a.RATE Order BY a.EMPNAME";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("YOP");
            //**************

            if (chksummary.Checked == true || ChkSummaryForPayment.Checked == true)
            {
                sht.Range("A1:J1").Merge();
                sht.Range("A1:J1").Style.Font.FontSize = 11;
                sht.Range("A1:J1").Style.Font.Bold = true;
                sht.Range("A1:J1").Style.NumberFormat.Format = "@";
                sht.Range("A1:J1").Style.Alignment.WrapText = true;
                sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:J1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1").Value = DDcompany.SelectedItem.Text + " (YOP ISSUE RECEIVE REPORT) " + Filterby;
                sht.Row(1).Height = 24.00;
                //                     
                sht.Range("A2:J2").Style.Font.FontSize = 11;

                using (var a = sht.Range("A2:J2"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Range("A2:J2").Style.Font.Bold = true;
                sht.Range("D2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("D2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                sht.Range("A2").Value = "Emp. Name";
                sht.Range("B2").Value = "Item";
                sht.Range("C2").Value = "Unit";
                sht.Range("D2").Value = "Issued qty.";
                sht.Range("E2").Value = "Received qty.";
                sht.Range("F2").Value = "Bal Qty.";
                sht.Range("G2").Value = "Loss Qty.";
                sht.Range("H2").Value = "Gain Qty.";
                sht.Range("I2").Value = "Rate";
                sht.Range("J2").Value = "Amount";
                //***********************
                int row = 3;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    using (var a = sht.Range("A" + row + ":J" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("D" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("D" + row + ":J" + row).Style.NumberFormat.Format = "#,##0.000";

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Empname"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"].ToString());
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["unitname"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["issueqty"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Receivedqty"]);
                    Bal = Convert.ToDecimal(ds.Tables[0].Rows[i]["issueqty"]) - (Convert.ToDecimal(ds.Tables[0].Rows[i]["Receivedqty"]) + Convert.ToDecimal(ds.Tables[0].Rows[i]["Lossqty"]));
                    sht.Range("F" + row).SetValue(Bal);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Lossqty"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["gainqty"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                    sht.Range("J" + row).FormulaA1 = "=E" + row + '*' + "I" + row;
                    row = row + 1;
                }
                using (var a = sht.Range("D" + row + ":J" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Range("D" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("D" + row + ":J" + row).Style.Font.Bold = true;
                sht.Range("D" + row + ":J" + row).Style.NumberFormat.Format = "#,##0.000";

                sht.Range("D" + row).FormulaA1 = "SUM(D3:D" + (row - 1) + ")";
                sht.Range("E" + row).FormulaA1 = "SUM(E3:E" + (row - 1) + ")";
                sht.Range("F" + row).FormulaA1 = "SUM(F3:F" + (row - 1) + ")";
                sht.Range("G" + row).FormulaA1 = "SUM(G3:G" + (row - 1) + ")";
                sht.Range("H" + row).FormulaA1 = "SUM(H3:H" + (row - 1) + ")";
                sht.Range("J" + row).FormulaA1 = "SUM(J3:J" + (row - 1) + ")";
            }
            else
            {
                sht.Range("A1:Y1").Merge();
                sht.Range("A1:Y1").Style.Font.FontSize = 11;
                sht.Range("A1:Y1").Style.Font.Bold = true;
                sht.Range("A1:Y1").Style.NumberFormat.Format = "@";
                sht.Range("A1:Y1").Style.Alignment.WrapText = true;
                sht.Range("A1:Y1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:Y1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1").Value = DDcompany.SelectedItem.Text + " (YOP ISSUE RECEIVE REPORT) " + Filterby;
                sht.Row(1).Height = 24.00;
                //                     
                sht.Range("A2:Y2").Style.Font.FontSize = 11;

                using (var a = sht.Range("A2:Y2"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Range("A2:Y2").Style.Font.Bold = true;
                sht.Range("J2:Y2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("P2:Y2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("A2").Value = "Challan Date";
                sht.Range("B2").Value = "Department Name";
                sht.Range("C2").Value = "Emp. Name";
                sht.Range("D2").Value = "Issue No/Rec Challan No";

                sht.Range("E2").Value = "Customer Code";
                sht.Range("F2").Value = "Customer Order No";

                sht.Range("G2").Value = "Item";
                sht.Range("H2").Value = "Shade Color";
                sht.Range("I2").Value = "Unit";
                sht.Range("J2").Value = "Lot No.";
                sht.Range("K2").Value = "Tag No.";
                sht.Range("L2").Value = "Issued qty.";
                sht.Range("M2").Value = "Ret Qty";
                sht.Range("N2").Value = "Actual Iss Qty";
                sht.Range("O2").Value = "Received qty.";
                sht.Range("P2").Value = "Bal Qty.";
                sht.Range("Q2").Value = "Challan Status";
                sht.Range("R2").Value = "Loss Qty.";
                sht.Range("S2").Value = "Gain Qty.";
                sht.Range("T2").Value = "Rate";
                sht.Range("U2").Value = "Amount";
                sht.Range("V2").Value = "Cone Type";
                sht.Range("W2").Value = "Ply Type";
                sht.Range("X2").Value = "Transport Type";
                sht.Range("Y2").Value = "Party ChallanNo";
                
                //***********************
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "ID asc,Itemdescription";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());
                int row = 3;
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    using (var a = sht.Range("A" + row + ":Y" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("L" + row + ":P" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("L" + row + ":P" + row).Style.NumberFormat.Format = "#,##0.000";
                    sht.Range("R" + row + ":U" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("R" + row + ":S" + row).Style.NumberFormat.Format = "#,##0.000";

                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["issuedate"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Department"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Empname"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["issueno"]);

                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["CustomerCode"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["CustomerOrderNo"]);

                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Item_Name"].ToString() + ' ' + ds1.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Shadecolorname"]);
                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["unitname"]);
                    sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
                    sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["TagNo"]);
                    sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["ISSUEQTY"]);
                    sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["RETQTY"]);
                    sht.Range("N" + row).SetValue(Convert.ToDecimal(ds1.Tables[0].Rows[i]["issueqty"]) - (Convert.ToDecimal(ds1.Tables[0].Rows[i]["RETQTY"])));
                    sht.Range("O" + row).SetValue(ds1.Tables[0].Rows[i]["Receivedqty"]);
                    Bal = Convert.ToDecimal(ds1.Tables[0].Rows[i]["issueqty"]) - (Convert.ToDecimal(ds1.Tables[0].Rows[i]["RETQTY"])) - (Convert.ToDecimal(ds1.Tables[0].Rows[i]["Receivedqty"]) + Convert.ToDecimal(ds1.Tables[0].Rows[i]["Lossqty"]));
                    sht.Range("P" + row).SetValue(Bal);
                    sht.Range("Q" + row).SetValue(ds1.Tables[0].Rows[i]["status"]);
                    sht.Range("R" + row).SetValue(ds1.Tables[0].Rows[i]["Lossqty"]);
                    sht.Range("S" + row).SetValue(ds1.Tables[0].Rows[i]["gainqty"]);
                    sht.Range("T" + row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);
                    sht.Range("U" + row).FormulaA1 = "=O" + row + '*' + "T" + row;
                    
                    sht.Range("V" + row).SetValue(ds1.Tables[0].Rows[i]["ConeType"]);
                    sht.Range("W" + row).SetValue(ds1.Tables[0].Rows[i]["PlyType"]);
                    sht.Range("X" + row).SetValue(ds1.Tables[0].Rows[i]["TransportType"]);
                    sht.Range("Y" + row).SetValue(ds1.Tables[0].Rows[i]["PartyChallanNo"]);
                    row = row + 1;
                }
                using (var a = sht.Range("L" + row + ":Y" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //********Grand Total
                //var issued = sht.Evaluate("SUM(I3:I" + (row - 1) + ")");
                //var Received = sht.Evaluate("SUM(J3:J" + (row - 1) + ")");
                //var Balanced = sht.Evaluate("SUM(K3:K" + (row - 1) + ")");
                //var Lossqty = sht.Evaluate("SUM(M3:M" + (row - 1) + ")");
                //var Gainqty = sht.Evaluate("SUM(N3:N" + (row - 1) + ")");

                sht.Range("L" + row + ":P" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("L" + row + ":P" + row).Style.Font.Bold = true;
                sht.Range("L" + row + ":P" + row).Style.NumberFormat.Format = "#,##0.000";
                sht.Range("R" + row + ":S" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("R" + row + ":U" + row).Style.Font.Bold = true;
                sht.Range("R" + row + ":S" + row).Style.NumberFormat.Format = "#,##0.000";

                sht.Range("L" + row).FormulaA1 = "SUM(L3:L" + (row - 1) + ")";
                sht.Range("M" + row).FormulaA1 = "SUM(M3:M" + (row - 1) + ")";
                sht.Range("N" + row).FormulaA1 = "SUM(N3:N" + (row - 1) + ")";
                sht.Range("O" + row).FormulaA1 = "SUM(O3:O" + (row - 1) + ")";
                sht.Range("P" + row).FormulaA1 = "SUM(P3:P" + (row - 1) + ")";
                sht.Range("R" + row).FormulaA1 = "SUM(R3:R" + (row - 1) + ")";
                sht.Range("S" + row).FormulaA1 = "SUM(S3:S" + (row - 1) + ")";
                sht.Range("U" + row).FormulaA1 = "SUM(U3:U" + (row - 1) + ")";

            }
            //********************
            sht.Columns(1, 30).AdjustToContents();
            //********************


            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("YOPISSUERECIVE_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();

        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }
    }

    protected void IssuereceivedetailKaysons()
    {
        string empid = "", Filterby = "";
        Decimal Bal = 0;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < chkemp.Items.Count; i++)
        {
            if (chkemp.Items[i].Selected)
            {
                sb.Append(chkemp.Items[i].Value + ",");
            }
        }
        empid = sb.ToString().TrimEnd(',');

        string str = "";
        if (chksummary.Checked == true)
        {
            str = @"Select a.EMPNAME, a.ITEM_NAME + ' ' + a.QUALITYNAME Item_Name, a.UNITNAME, Sum(a.ISSUEQTY) ISSUEQTY, Sum(a.RECEIVEDQTY) RECEIVEDQTY, Sum(a.LOSSQTY) LOSSQTY, Sum(a.GAINQTY) GAINQTY, a.RATE  
            From V_Yarnopeningissuereceivedetail a(Nolock) 
            Where a.Companyid=" + DDcompany.SelectedValue;
        }
        else if (ChkSummaryForPayment.Checked == true)
        {
            str = @"Select a.EMPNAME, '' Item_Name, a.UNITNAME, Sum(a.ISSUEQTY) ISSUEQTY, Sum(a.RECEIVEDQTY) RECEIVEDQTY, Sum(a.LOSSQTY) LOSSQTY, Sum(a.GAINQTY) GAINQTY, a.RATE  
            From V_Yarnopeningissuereceivedetail a(Nolock) 
            Where a.Companyid=" + DDcompany.SelectedValue;
        }
        else
        {
            str = "select * From V_Yarnopeningissuereceivedetail Where companyid=" + DDcompany.SelectedValue;
        }
        if (DDdeptname.SelectedIndex > 0)
        {
            str = str + " and Departmentid=" + DDdeptname.SelectedValue;
        }
        if (empid != "")
        {
            str = str + " and vendorid in(" + empid + ")";
        }
        if (Ddemplocation.SelectedIndex > 0)
        {
            str = str + " and emplocation=" + Ddemplocation.SelectedValue;
        }
        if (chkdate.Checked == true)
        {
            str = str + "  and Issuedate>='" + txtfromdate.Text + "' and Issuedate<='" + txttodate.Text + "'";
            Filterby = Filterby + " From Date-" + txtfromdate.Text + " and To Date-" + txttodate.Text;
        }
        if (txtLotno.Text != "")
        {
            str = str + " and Lotno='" + txtLotno.Text + "'";
            Filterby = Filterby + " Lot No-" + txtLotno.Text + "";
        }
        if (txtTagno.Text != "")
        {
            str = str + " and Tagno='" + txtTagno.Text + "'";
            Filterby = Filterby + " Tag No-" + txtTagno.Text + "";
        }
        if (DDcustcode.SelectedIndex > 0)
        {
            str = str + " and customerid=" + DDcustcode.SelectedValue;
            Filterby = Filterby + " Customer-" + DDcustcode.SelectedItem.Text + "";
        }
        if (DDorderno.SelectedIndex > 0)
        {
            str = str + " and Orderid=" + DDorderno.SelectedValue;
            Filterby = Filterby + " OrderNo-" + DDorderno.SelectedItem.Text + "";
        }

        if (chksummary.Checked == true)
        {
            str = str + @" Group By a.EMPNAME, a.ITEM_NAME, a.QUALITYNAME, a.UNITNAME, a.RATE Order BY a.EMPNAME, a.ITEM_NAME + ' ' + a.QUALITYNAME";
        }
        else if (ChkSummaryForPayment.Checked == true)
        {
            str = str + @" Group By a.EMPNAME, a.UNITNAME, a.RATE Order BY a.EMPNAME";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("YOP");
            //**************

            if (chksummary.Checked == true || ChkSummaryForPayment.Checked == true)
            {
                sht.Range("A1:J1").Merge();
                sht.Range("A1:J1").Style.Font.FontSize = 11;
                sht.Range("A1:J1").Style.Font.Bold = true;
                sht.Range("A1:J1").Style.NumberFormat.Format = "@";
                sht.Range("A1:J1").Style.Alignment.WrapText = true;
                sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:J1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1").Value = DDcompany.SelectedItem.Text + " (YOP ISSUE RECEIVE REPORT) " + Filterby;
                sht.Row(1).Height = 24.00;
                //                     
                sht.Range("A2:J2").Style.Font.FontSize = 11;

                using (var a = sht.Range("A2:J2"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Range("A2:J2").Style.Font.Bold = true;
                sht.Range("D2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("D2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                sht.Range("A2").Value = "Emp. Name";
                sht.Range("B2").Value = "Item";
                sht.Range("C2").Value = "Unit";
                sht.Range("D2").Value = "Issued qty.";
                sht.Range("E2").Value = "Received qty.";
                sht.Range("F2").Value = "Bal Qty.";
                sht.Range("G2").Value = "Loss Qty.";
                sht.Range("H2").Value = "Gain Qty.";
                //sht.Range("I2").Value = "Rate";
                //sht.Range("J2").Value = "Amount";
                //***********************
                int row = 3;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    using (var a = sht.Range("A" + row + ":J" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("D" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("D" + row + ":J" + row).Style.NumberFormat.Format = "#,##0.000";

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Empname"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"].ToString());
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["unitname"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["issueqty"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Receivedqty"]);
                    Bal = Convert.ToDecimal(ds.Tables[0].Rows[i]["issueqty"]) - (Convert.ToDecimal(ds.Tables[0].Rows[i]["Receivedqty"]) + Convert.ToDecimal(ds.Tables[0].Rows[i]["Lossqty"]));
                    sht.Range("F" + row).SetValue(Bal);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Lossqty"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["gainqty"]);
                    //sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                    //sht.Range("J" + row).FormulaA1 = "=E" + row + '*' + "I" + row;
                    row = row + 1;
                }
                using (var a = sht.Range("D" + row + ":J" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Range("D" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("D" + row + ":J" + row).Style.Font.Bold = true;
                sht.Range("D" + row + ":J" + row).Style.NumberFormat.Format = "#,##0.000";

                sht.Range("D" + row).FormulaA1 = "SUM(D3:D" + (row - 1) + ")";
                sht.Range("E" + row).FormulaA1 = "SUM(E3:E" + (row - 1) + ")";
                sht.Range("F" + row).FormulaA1 = "SUM(F3:F" + (row - 1) + ")";
                sht.Range("G" + row).FormulaA1 = "SUM(G3:G" + (row - 1) + ")";
                sht.Range("H" + row).FormulaA1 = "SUM(H3:H" + (row - 1) + ")";
                //sht.Range("J" + row).FormulaA1 = "SUM(J3:J" + (row - 1) + ")";
            }
            else
            {
                sht.Range("A1:Q1").Merge();
                sht.Range("A1:Q1").Style.Font.FontSize = 11;
                sht.Range("A1:Q1").Style.Font.Bold = true;
                sht.Range("A1:Q1").Style.NumberFormat.Format = "@";
                sht.Range("A1:Q1").Style.Alignment.WrapText = true;
                sht.Range("A1:Q1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:Q1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1").Value = DDcompany.SelectedItem.Text + " (YOP ISSUE RECEIVE REPORT) " + Filterby;
                sht.Row(1).Height = 24.00;
                //                     
                sht.Range("A2:Q2").Style.Font.FontSize = 11;

                using (var a = sht.Range("A2:Q2"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Range("A2:Q2").Style.Font.Bold = true;
                sht.Range("J2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("N2:Q2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("A2").Value = "Challan Date";
                sht.Range("B2").Value = "Department Name";
                sht.Range("C2").Value = "Emp. Name";
                sht.Range("D2").Value = "Challan No";
                sht.Range("E2").Value = "Item";
                sht.Range("F2").Value = "Shade Color";
                sht.Range("G2").Value = "Unit";
                sht.Range("H2").Value = "Lot No.";
                sht.Range("I2").Value = "Tag No.";
                sht.Range("J2").Value = "Issued qty.";
                sht.Range("K2").Value = "Received qty.";
                sht.Range("L2").Value = "Bal Qty.";
                sht.Range("M2").Value = "Challan Status";
                sht.Range("N2").Value = "Loss Qty.";
                sht.Range("O2").Value = "Gain Qty.";
                sht.Range("P2").Value = "Machine No";
                //sht.Range("P2").Value = "Rate";
                //sht.Range("Q2").Value = "Amount";
                ////***********************
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "ID asc,Itemdescription";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());
                int row = 3;
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    using (var a = sht.Range("A" + row + ":Q" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("J" + row + ":L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("J" + row + ":L" + row).Style.NumberFormat.Format = "#,##0.000";
                    sht.Range("N" + row + ":Q" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("N" + row + ":O" + row).Style.NumberFormat.Format = "#,##0.000";

                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["issuedate"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Department"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Empname"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["issueno"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Item_Name"].ToString() + ' ' + ds1.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Shadecolorname"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["unitname"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["TagNo"]);
                    sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["issueqty"]);
                    sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Receivedqty"]);
                    Bal = Convert.ToDecimal(ds1.Tables[0].Rows[i]["issueqty"]) - (Convert.ToDecimal(ds1.Tables[0].Rows[i]["Receivedqty"]) + Convert.ToDecimal(ds1.Tables[0].Rows[i]["Lossqty"]));
                    sht.Range("L" + row).SetValue(Bal);
                    sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["status"]);
                    sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["Lossqty"]);
                    sht.Range("O" + row).SetValue(ds1.Tables[0].Rows[i]["gainqty"]);
                    sht.Range("P" + row).SetValue(ds1.Tables[0].Rows[i]["IssueMachineNo"]);
                    //sht.Range("P" + row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);
                    //sht.Range("Q" + row).FormulaA1 = "=K" + row + '*' + "P" + row;
                    row = row + 1;
                }
                using (var a = sht.Range("J" + row + ":Q" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //********Grand Total
                //var issued = sht.Evaluate("SUM(I3:I" + (row - 1) + ")");
                //var Received = sht.Evaluate("SUM(J3:J" + (row - 1) + ")");
                //var Balanced = sht.Evaluate("SUM(K3:K" + (row - 1) + ")");
                //var Lossqty = sht.Evaluate("SUM(M3:M" + (row - 1) + ")");
                //var Gainqty = sht.Evaluate("SUM(N3:N" + (row - 1) + ")");

                sht.Range("J" + row + ":L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("J" + row + ":L" + row).Style.Font.Bold = true;
                sht.Range("J" + row + ":L" + row).Style.NumberFormat.Format = "#,##0.000";
                sht.Range("N" + row + ":O" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("N" + row + ":Q" + row).Style.Font.Bold = true;
                sht.Range("N" + row + ":O" + row).Style.NumberFormat.Format = "#,##0.000";

                sht.Range("J" + row).FormulaA1 = "SUM(J3:J" + (row - 1) + ")";
                sht.Range("K" + row).FormulaA1 = "SUM(K3:K" + (row - 1) + ")";
                sht.Range("L" + row).FormulaA1 = "SUM(L3:L" + (row - 1) + ")";
                sht.Range("N" + row).FormulaA1 = "SUM(N3:N" + (row - 1) + ")";
                sht.Range("O" + row).FormulaA1 = "SUM(O3:O" + (row - 1) + ")";
                sht.Range("Q" + row).FormulaA1 = "SUM(Q3:Q" + (row - 1) + ")";

            }
            //********************
            sht.Columns(1, 30).AdjustToContents();
            //********************


            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("YOPISSUERECIVE_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();

        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }
    }
    protected void DDcustcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str="";
        if (Convert.ToInt32(Session["varcompanyId"]) == 16 || Convert.ToInt32(Session["varcompanyId"]) == 28)
        {
            Str =@"select orderid,CustomerOrderNo orderno 
            from ordermaster 
            where CompanyId=" + DDcompany.SelectedValue + " and customerid=" + DDcustcode.SelectedValue + @" and status=0 order by orderno";
        }
        else
        {
            Str =@"select orderid,LocalOrder+' '+CustomerOrderNo as orderno 
            from ordermaster 
            where CompanyId=" + DDcompany.SelectedValue + " and customerid=" + DDcustcode.SelectedValue + @" and status=0 order by orderno";
        }
        UtilityModule.ConditionalComboFill(ref DDorderno, Str, true, "--Plz Select--");

    }
    protected void DDdeptname_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @" select EI.EmpId,EI.EmpName + CASE WHEN EI.EMPCODE<>'' THEN ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME from empinfo EI inner join Department D 
                           on EI.departmentId=D.DepartmentId Where D.Departmentid=" + DDdeptname.SelectedValue + @"
                           and isnull(Ei.Blacklist,0)=0 order by EmpName  ";

        UtilityModule.ConditonalChkBoxListFill(ref chkemp, str);
    }
}