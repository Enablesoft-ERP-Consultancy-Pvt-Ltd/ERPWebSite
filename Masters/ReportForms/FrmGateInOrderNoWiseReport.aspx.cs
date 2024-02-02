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

public partial class Masters_ReportForms_FrmGateInOrderNoWiseReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by Companyname";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                CompanySelectedChange();
            }
           
            //***********Month Year
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy"); 

            if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
            {
                TRCustomerCode.Visible = false;
            }
        }
    }
    private void CompanySelectedChange()
    {
        if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
        {
            string str = @"Select OrderId,LocalOrder+ ' / ' +CustomerOrderNo From OrderMaster where CompanyId=" + DDCompany.SelectedValue + " Order By CustomerOrderNo";
            if (Session["varMasterCompanyIDForERP"].ToString() == "16")
            {
                str = @"Select OrderId, CustomerOrderNo From OrderMaster where CompanyId=" + DDCompany.SelectedValue + " Order By CustomerOrderNo";
            }
            UtilityModule.ConditionalComboFill(ref DDOrderNo, str, true, "--Select--");
        }
        UtilityModule.ConditionalComboFill(ref DDCustCode, "Select CustomerId,CustomerCode From CustomerInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By CustomerCode", true, "--Select--");
    }
    protected void DDCustCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = @"Select OrderId, case when " + variable.VarORDERNODROPDOWNWITHLOCALORDER + @" = 1 THen LocalOrder + ' / ' + CustomerOrderNo Else customerorderno End OrderNo 
            From OrderMaster 
            Where CustomerId = " + DDCustCode.SelectedValue + " And CompanyId = " + DDCompany.SelectedValue;      
        Str = Str + " Order By OrderNo";

        UtilityModule.ConditionalComboFill(ref DDOrderNo, Str, true, "--Select--");
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        GATEINORDERNOWISEREPORT();
    }    
    
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedChange();
    }    
    
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (RDPOSTATUS.Checked == true)
        //{

        //    string str = @"select Replace(CONVERT(nvarchar(11),Dateadded,106),' ','-') as Dateadded From Ordermaster Where orderid=" + DDOrderNo.SelectedValue;
        //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        txtfromdate.Text = ds.Tables[0].Rows[0]["Dateadded"].ToString();
        //        txttodate.Text = ds.Tables[0].Rows[0]["Dateadded"].ToString();
        //    }
        //    else
        //    {
        //        txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        //        txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        //    }

        //}
    }
     
    protected void ChkForDate_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForDate.Checked == true)
        {
            
            TRMonthyear.Visible = true;
        }
        else
        {           
            TRMonthyear.Visible = false;
        }
    }    
    
    private void GATEINORDERNOWISEREPORT()
    {

        String str = "";
        if (ChkForDate.Checked == true)
        {
            str = str + " and cast(GIM.GateInDate as date)>='" + txtfromdate.Text + "' and cast(GIM.GateInDate as date)<='" + txttodate.Text + "'";
        }
        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " and OM.customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
        }        
        //if (DDorderstatus.SelectedIndex > 0)
        //{
        //    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        //}

        ////int Rowcount = 0;
        ////SqlParameter[] param = new SqlParameter[4];
        ////param[0] = new SqlParameter("@companyId", DDCompany.SelectedValue);
        ////param[1] = new SqlParameter("@processid", 1);
        ////param[2] = new SqlParameter("orderid", DDOrderNo.SelectedValue);
        ////param[3] = new SqlParameter("LocalOrder", txtlocalOrderNo.Text);
        ////DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETFOLIOWISEDETAIL", param);


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GateInOrderNoWiseReportDetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);        
        cmd.Parameters.AddWithValue("@Where", str);
        cmd.Parameters.AddWithValue("@USERID", Session["VarUserId"]);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);      

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************
        con.Close();
        con.Dispose();

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("GATEIN DETAIL");
            int row = 0;
            //*******************

            sht.Range("A1:J1").Merge();
            sht.Range("A1").SetValue(ds.Tables[0].Rows[0]["CompanyName"]);
            sht.Range("A1:J1").Style.Font.Bold = true;
            sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:J1").Style.Font.FontName = "Calibri";
            sht.Range("A1:J1").Style.Font.FontSize = 12;

            sht.Range("A2:J2").Merge();
            sht.Range("A2").SetValue(ds.Tables[0].Rows[0]["CompAddr1"]+" "+ds.Tables[0].Rows[0]["CompAddr2"]+" "+ds.Tables[0].Rows[0]["CompAddr3"]);
            sht.Range("A2:J2").Style.Font.Bold = true;
            sht.Range("A2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:J2").Style.Font.FontName = "Calibri";
            sht.Range("A2:J2").Style.Font.FontSize = 11;
            sht.Range("A2:J2").Style.Alignment.SetWrapText();

            sht.Range("A3:J3").Merge();

            if (ChkForDate.Checked == true)
            {
                sht.Range("A3").SetValue("REORT FROM : " + txtfromdate.Text + " To " + txttodate.Text);
            }
            else
            {
                sht.Range("A3").SetValue("");
            }            
            sht.Range("A3:J3").Style.Font.Bold = true;
            sht.Range("A3:J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A3:J3").Style.Font.FontName = "Calibri";
            sht.Range("A3:J3").Style.Font.FontSize = 12;

            sht.Range("A4").Value = "REC DATE";
            sht.Range("B4").Value = "GATEIN/CHALLAN NO";
            sht.Range("C4").Value = "CUSTOMER CODE";

            sht.Range("D4").Value = "ORDER NO";
            sht.Range("E4").Value = "ITEM NAME";
            sht.Range("F4").Value = "QUALITY";
            sht.Range("G4").Value = "SHADE COLOR";
            sht.Range("H4").Value = "LOT NO";
            sht.Range("I4").Value = "QTY";
            sht.Range("J4").Value = "REMARK";  

            sht.Range("A4:J4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A4:J4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A4:J4").Style.Alignment.WrapText = true;
            sht.Range("A4:J4").Style.Font.SetBold();
            ////sht.Columns("I:L").Width = 10.11;
            //*************          
            row = 5;
            int rowfrom = 0;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Range("A" + row + ":L" + row).Style.Font.SetBold();

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["GateInDate"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["GATEINNO"]+" /"+ ds.Tables[0].Rows[i]["CHALLANNO"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);

                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["LotNo"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Remark"]);
                
                row = row + 1;
            }

            //*************
            using (var a = sht.Range("A1" + ":J" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*************
            sht.Columns(1, 12).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("GateInOrderNoWiseReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "Foliowise", "alert('No Record Found!');", true);
        }
    }

   
    
}