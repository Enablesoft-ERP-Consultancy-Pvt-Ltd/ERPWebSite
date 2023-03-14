using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Threading;
using System.Configuration;
using System.Web.SessionState;
using ClosedXML.Excel;
using System.Data.SqlClient;
using System.Text;


/// <summary>
/// Summary description for ScheduleTasks
/// </summary>
public class ScheduleTasks
{
    ////public void SendAfterThreeDaysmails()
    ////{
    ////    try
    ////    {

    ////        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    ////        if (con.State == ConnectionState.Closed)
    ////        {
    ////            con.Open();
    ////        }
    ////        SqlCommand cmd = new SqlCommand("PRO_GETINDENTWISEDETAILEXCELREPORTFOREMAIL", con);
    ////        cmd.CommandType = CommandType.StoredProcedure;
    ////        cmd.CommandTimeout = 300;

    ////        cmd.Parameters.AddWithValue("@Companyid",0);
    ////        cmd.Parameters.AddWithValue("@Where", "");

    ////        DataSet ds = new DataSet();
    ////        SqlDataAdapter ad = new SqlDataAdapter(cmd);
    ////        cmd.ExecuteNonQuery();
    ////        ad.Fill(ds);
    ////        //*************

    ////        con.Close();
    ////        con.Dispose();

    ////        //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
    ////        if (ds.Tables[0].Rows.Count > 0)
    ////        {              
               
    ////            string Path = "";
    ////            var xapp = new XLWorkbook();
    ////            var sht = xapp.Worksheets.Add("IndentPendingDetail");
    ////            int row = 0;

    ////            sht.Column("A").Width = 20.22;
    ////            sht.Column("B").Width = 20.22;
    ////            sht.Column("C").Width = 20.22;
    ////            sht.Column("D").Width = 20.22;
    ////            sht.Column("E").Width = 20.22;
    ////            sht.Column("F").Width = 20.22;
    ////            sht.Column("G").Width = 20.22;


    ////            //*******Header
    ////            sht.Range("A2").Value = "Indent No";
    ////            sht.Range("B2").Value = "Issue Date";
    ////            sht.Range("C2").Value = "Required Date";
    ////            sht.Range("D2").Value = "Dyer Name";
    ////            sht.Range("E2").Value = "Iss Qty";
    ////            sht.Range("F2").Value = "Rec Qty";
    ////            sht.Range("G2").Value = "Pending Qty";


    ////            sht.Range("A2:G2").Style.Font.FontName = "Arial Unicode MS";
    ////            sht.Range("A2:G2").Style.Font.FontSize = 10;
    ////            sht.Range("A2:G2").Style.Font.Bold = true;

    ////            sht.Range("A5:R5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    ////            sht.Range("A5:R5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
    ////            sht.Range("A5:R5").Style.Alignment.SetWrapText();

    ////            row = row + 3;

    ////            Decimal amt = 0;
    ////            string tagno = "";
    ////            string LotNo = "";
    ////            ////string IndentNo = "";

    ////            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
    ////            {

    ////                sht.Range("A" + row + ":G" + row).Style.Font.FontName = "Arial Unicode MS";
    ////                sht.Range("A" + row + ":G" + row).Style.Font.FontSize = 10;

    ////                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[j]["IndentNo"]);
    ////                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[j]["Date"]);
    ////                //sht.Range("B" + row).Style.Alignment.SetWrapText();
    ////                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[j]["ReqDate"]);
    ////                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[j]["EmpName"]);
    ////                sht.Range("D" + row).Style.Alignment.SetWrapText();
    ////                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[j]["IssueQty"]);
    ////                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[j]["RecQty"]);
    ////                sht.Range("G" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[j]["IssueQty"]) - Convert.ToDecimal(ds.Tables[0].Rows[j]["RecQty"]));

    ////                row = row + 1;
    ////            }

    ////            //*************
    ////            using (var a = sht.Range("A1:G" + row))
    ////            {
    ////                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    ////                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    ////                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
    ////                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    ////            }

    ////            sht.Columns(1, 30).AdjustToContents();

    ////            string Fileextension = "xlsx";
    ////            string filename = UtilityModule.validateFilename("IndentWiseDetailForEmail_" + DateTime.Now + "." + Fileextension);
    ////            Path = HttpRuntime.AppDomainAppPath + "\\Tempexcel\\" + filename;
    ////            xapp.SaveAs(Path);
    ////            xapp.Dispose();
    ////            string attchmnt;
    ////            attchmnt = Path;
    ////            Thread email = new Thread(delegate()
    ////            {
    ////                Sendmail.SendMail(pTo: "", pSubject: "ERP: Indent Pending Detail.", pBody: "", pFromSMTP: "smtp_1", pFromDispName: "", pAttachments: attchmnt, pCC: "", pAttchmntName: "", pBCC: "");
    ////            });
    ////            email.IsBackground = true;
    ////            email.Start();


    ////            //Sendmail.SendMail(pTo: "", pSubject: "Indent Pending Detail.", pBody: "", pFromSMTP: "smtp_1", pFromDispName: "", pAttachments: attchmnt, pCC: "", pAttchmntName: "", pBCC: "");


    ////        }
           
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        throw;
    ////    }
    ////}
    public void Sendweeklymails(string StartDate = "", string EndDate = "")
    {

        GenerateOrderReports(StartDate, EndDate);
    }
    protected void GenerateOrderReports(string StartDate, string EndDate)
    {
        //        string str = @"select CI.CompanyName,CompAddr1,CompAddr2,CompAddr3,CompFax,CompTel,OM.OrderId,OM.LocalOrder,OM.CustomerOrderNo,CC.CustomerCode,OM.OrderDate,OM.DispatchDate,
        //                    V.ProductCode,V.Category_Name,v.item_name,V.QualityName,V.DesignName,V.Colorname,V.Shapename,
        //                    case When OrderUnitId=1 Then V.Sizemtr Else case When OrderUnitId=2 Then V.SizeFt Else case When OrderUnitId=6 then V.Sizeinch Else '' End End End Size,U.unitName 
        //                    ,Sum(QtyRequired)OrderQty from OrderMaster OM,OrderDetail OD,V_FinisheditemDetail V,Companyinfo CI,Customerinfo CC
        //                    ,Unit U Where OM.OrderId=OD.OrderId  And CC.CustomerId=OM.CustomerId 
        //                    And  OD.Item_Finished_id=V.Item_Finished_Id And Om.CompanyId=CI.COmpanyId And OD.OrderUnitId=U.UnitId        
        //                    and Dateadded>='" + StartDate + "' and Dateadded<='" + EndDate + @"'
        //                    group by CI.CompanyName,CompAddr1,CompAddr2,CompAddr3,CompFax,CompTel,OM.OrderDate,OM.DispatchDate,CC.CustomerCode,OM.OrderId,OM.LocalOrder,OM.CustomerOrderNo,V.Category_Name,V.ProductCode,v.item_name,V.QualityName,V.DesignName,V.Colorname,V.Shapename,
        //                    OrderUnitId,V.Sizemtr,V.SizeFt,V.Sizeinch,U.unitName";
        //        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            string dsfilename = "ReportSchema\\RptBuyerDetailsFormail.xsd";
        //            string rptfilename = "Reports\\RptBuyerDetailsFormail.rpt";
        //            ReportDocument RD = new ReportDocument();
        //            RD.Load(HttpRuntime.AppDomainAppPath + rptfilename);
        //            ds.WriteXmlSchema(HttpRuntime.AppDomainAppPath + dsfilename);
        //            RD.SetDataSource(ds);
        //            string attchmnt;
        //            attchmnt = HttpRuntime.AppDomainAppPath + "\\Attachments\\OrderDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".Pdf";
        //            RD.ExportToDisk(ExportFormatType.PortableDocFormat, attchmnt);
        //            RD.Close();
        //            RD.Dispose();
        //            RD = null;
        //            GC.Collect();
        //            string mailbody = "", ccAddrs = "", bccAddrs = "";
        //            ccAddrs = "dharmkamboj@gmail.com";
        //            Sendmail.SendMail("mkmanojkumar70@gmail.com", "ERP: Weekly Order Entry Detail.", mailbody, "smtp_1", "ERP Support", attchmnt, ccAddrs, pBCC: bccAddrs);

        //        }
        string str = @"select CC.CompanyName,OM.Orderdate,vf.ITEM_NAME,ISNULL(CQ.QualityNameAToC,vf.QualityName) as Quality,ISNULL(Cd.DesignNameAToC,vf.designname) as Designname,
                        isnull(c.ColorNameToC,vf.ColorName) as Colorname,vf.ShapeName,case when OD.flagsize=1 Then vf.SizeMtr when Od.flagsize=2 Then vf.SizeInch else vf.SizeFt end as size,
                        Sum(Od.qtyrequired) as Qty,Sum(Od.Qtyrequired*TotalArea) as Area,OD.unitrate,Sum(OD.amount) as Amount,Cur.CurrencyName,OM.DispatchDate
                        From OrderMaster OM inner join OrderDetail OD on OM.OrderId=OD.orderid
                        inner join V_FinishedItemDetail vf on od.Item_Finished_Id=vf.ITEM_FINISHED_ID
                        inner join customerinfo CC on OM.CustomerId=CC.Customerid
                        left join CustomerQuality cq on Od.CQID=cq.SrNo
                        left join CustomerDesign cd on od.DSRNO=Cd.srno
                        left join CustomerColor c on od.CSRNO=c.SrNo
                        left join currencyinfo cur on OD.CurrencyId=Cur.CurrencyId Where OM.Dateadded>='" + StartDate + "' and OM.Dateadded<='" + EndDate + @"'
                        group by  CC.CompanyName,OM.Orderdate,vf.ITEM_NAME,CQ.QualityNameAToC,vf.QualityName,Cd.DesignNameAToC,vf.designname,
                        c.ColorNameToC,vf.ColorName,vf.ShapeName,OD.flagsize,vf.SizeMtr,vf.SizeInch,vf.SizeFt,OD.unitrate,Cur.CurrencyName,OM.DispatchDate";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("BuyerOrderDetails");

            sht.Range("A1:N1").Style.Font.FontSize = 11;
            sht.Range("A1:N1").Style.Font.Bold = true;
            sht.Range("A1:N1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("I1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Row(1).Height = 18.00;
            //
            sht.Range("A1").SetValue("Buyer");
            sht.Range("B1").SetValue("Order Date");
            sht.Range("C1").SetValue("Item_Name");
            sht.Range("D1").SetValue("Quality.");
            sht.Range("E1").SetValue("Design");
            sht.Range("F1").SetValue("Color");
            sht.Range("G1").SetValue("Shape");
            sht.Range("H1").SetValue("Size");
            sht.Range("I1").SetValue("Qty");
            sht.Range("J1").SetValue("Area");
            sht.Range("K1").SetValue("Rate");
            sht.Range("L1").SetValue("Amount");
            sht.Range("M1").SetValue("Currency");
            sht.Range("N1").SetValue("Dispatch Date");
            int Row = 2;
            
            int rowcount = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rowcount; i++)
            {
                sht.Range("A" + Row + ":N" + Row).Style.Font.FontSize = 11;

                sht.Range("A" + Row).SetValue(ds.Tables[0].Rows[i]["CompanyName"]);
                sht.Range("B" + Row).SetValue(ds.Tables[0].Rows[i]["orderdate"]);
                sht.Range("C" + Row).SetValue(ds.Tables[0].Rows[i]["Item_name"]);
                sht.Range("D" + Row).SetValue(ds.Tables[0].Rows[i]["Quality"]);
                sht.Range("E" + Row).SetValue(ds.Tables[0].Rows[i]["Designname"]);
                sht.Range("F" + Row).SetValue(ds.Tables[0].Rows[i]["Colorname"]);
                sht.Range("G" + Row).SetValue(ds.Tables[0].Rows[i]["Shapename"]);
                sht.Range("H" + Row).SetValue(ds.Tables[0].Rows[i]["Size"]);
                sht.Range("I" + Row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                sht.Range("J" + Row).SetValue(ds.Tables[0].Rows[i]["Area"]);
                sht.Range("K" + Row).SetValue(ds.Tables[0].Rows[i]["Unitrate"]);
                sht.Range("L" + Row).SetValue(ds.Tables[0].Rows[i]["Amount"]);
                sht.Range("M" + Row).SetValue(ds.Tables[0].Rows[i]["Currencyname"]);
                sht.Range("N" + Row).SetValue(ds.Tables[0].Rows[i]["DispatchDate"]);
                Row = Row + 1;
            }
            //**********Total
            var Qty = sht.Evaluate("SUM(I2:I" + (Row - 1) + ")");
            var Area = sht.Evaluate("SUM(J2:J" + (Row - 1) + ")");
            var Amount = sht.Evaluate("SUM(L2:L" + (Row - 1) + ")");
            //*************
            sht.Range("I" + Row).SetValue(Qty);
            sht.Range("J" + Row).SetValue(Area);
            sht.Range("L" + Row).SetValue(Amount);
            sht.Columns(1, 15).AdjustToContents();
            //******SAVE FILE
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("BUYERORDERDETAILS-" + DateTime.Now + "." + Fileextension);
            Path = HttpRuntime.AppDomainAppPath + "\\Tempexcel\\" + filename;
            xapp.SaveAs(Path);
            xapp.Dispose();
            string attchmnt;
            attchmnt = Path;
            //Sendmail.SendMail("mkmanojkumar70@gmail.com", "ERP: Weekly Order Entry Detail.", "", "smtp_1", "ERP Support", attchmnt, "", pBCC: "");
            Thread email = new Thread(delegate()
                {
                    Sendmail.SendMail(pTo: "", pSubject: "ERP: Weekly Order Entry Detail.", pBody: "", pFromSMTP: "smtp_1", pFromDispName: "", pAttachments: attchmnt, pCC: "", pAttchmntName: "", pBCC: "");
                });
            email.IsBackground = true;
            email.Start();
        }

    }
    public void UserEnable_Disable(Boolean Enable = false)
    {
        int companyid = Convert.ToInt16(ConfigurationManager.AppSettings["xxpc36"]);
        switch (companyid)
        {
            case 8:
                UsertimingtoLogin_Out(Enable);
                break;
        }

    }
    private void UsertimingtoLogin_Out(Boolean Enable)
    {
        string str;
        if (Enable == true)
        {
            str = "Update Newuserdetail set loginflag=0 Where usertype<>1"; //Usertype 1 Means Administrator
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        }
        else
        {
            str = "Update Newuserdetail set loginflag=1 Where usertype<>1";
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Session != null)
                {

                }
            }
        }
    }
    public void SenddailyTNAupdate()
    {
        string msg = "";
        string Number = "";
        string str = @"select Distinct CI.CustomerCode+' # '+om.CustomerOrderNo as OrderNo from OrderMaster OM 
                        inner join customerinfo CI on OM.customerid=CI.CustomerId
                        Where not exists(select Distinct OM.OrderId from OrderMaster OM
                        inner join OrderRawMaterialProcurement ORP on OM.OrderId=ORP.orderid
                        inner join orderProductionTna OPT on OM.OrderId=OPT.orderid
                        inner join orderInspectionTna OIT on OM.OrderId=OIT.orderid
                        Where Replace(convert(nvarchar(11),dateadded,106),' ','-')=REPLACE(CONVERT(nvarchar(11),getdate()-3,106),' ','-')) and Replace(convert(nvarchar(11),dateadded,106),' ','-')=REPLACE(CONVERT(nvarchar(11),getdate()-3,106),' ','-')";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            int companyid = Convert.ToInt16(ConfigurationManager.AppSettings["xxpc36"]);
            switch (companyid)
            {
                case 12:
                    Number = "9717331705,9839049112,8756266111";
                    break;
            }
            //Looping 
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                msg = msg == "" ? ds.Tables[0].Rows[i]["OrderNo"].ToString() : msg + "," + ds.Tables[0].Rows[i]["orderno"].ToString();
            }
            string message = "Dear Sir,\nThese customer's order T&A is not updated " + msg + ".";
            UtilityModule.SendMessage(Number, message, companyid);
            //
        }
    }
    public void Auto_Save_Barcode()
    {
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "EXEC PRO_AUTOSAVEFROMBARCODE");
    }
    public void Autosavebarcodestart()
    {
        var thread = new Thread(new ThreadStart(Auto_Save_Barcode));
        thread.IsBackground = true;
        thread.Name = "BackgroundChecker";
        thread.Start();
    }
}