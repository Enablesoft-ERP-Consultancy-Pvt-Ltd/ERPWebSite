using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_frmReportForInvoiceAmtDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyNo"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtFromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttoDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "select CustomerId,CustomerCode+'/'+CompanyName from Customerinfo Order by CustomerCode", true, "--Select Customer Code");
        }
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDInvoiceNo, "select InvoiceId,TInvoiceNo+'/'+Replace(Convert(nvarchar(11),InvoiceDate,106),' ','-') As InvoiceNo From Invoice Where ConsignorId = " + Session["CurrentWorkingCompanyID"] + " And CosigneeId = " + DDCustomerCode.SelectedValue + " and InvoiceDate>='" + txtFromDate.Text + "' And InvoiceDate<='" + txttoDate.Text + "'", true, "--Select Invoice No--");
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        lblErrormsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string str = @"select CMP.CompanyName,CMP.CompAddr1,CMP.CompTel,CI.CustomerName,CI.CustomerCode,INV.TInvoiceNo,InvoiceDate,CC.CurrencyName,Round(Sum(case When CalTypeAmt=1 Then Pcs*Price Else case when P.UnitId=2 Then case when S.ShapeName='Round' Then case When P.ConsigneeId=3 Then Round((pcs*Areaft/4*22/7),2)*Price Else Round((pcs*Areaft/9/4*22/7),2)*Price End else case When P.ConsigneeId=3 Then  Round(pcs*areaft/9,2)*price Else Round(pcs*areaft,2)*price End End else case When P.UnitId=1 Then Pcs*Areamtr*Price Else pcs*Areaft*Price  End End end),2) InvoiceAmt,
                    isnull(v.RecAmt,0) As RecAmt,isnull(V.DBKAmt,0) As DBKAmt,isnull(INV.License_Commission,0) As License_Commission,isnull(INV.Agent_Commission,0) As Agent_Commission,Bldt,'" + txtFromDate.Text + "' As FromDate,'" + txttoDate.Text + @"' As ToDate
                    from Packing P,PackingInformation PI,V_FinishedItemDetail vf,Shape S,Currencyinfo CC,Companyinfo CMP,Invoice INV left outer join
                    Customerinfo CI on INV.CosigneeId=CI.CustomerId left outer Join v_InvoiceRecAmt V on INV.Invoiceid=V.InvoiceId
                     
                    Where INV.Status=1 And  P.PackingId=PI.PackingId And  PI.Finishedid=vf.item_finished_id and s.shapeid=vf.shapeid And P.PackingId=INV.PackingId And P.CurrencyId=CC.CurrencyId And INV.ConsignorId=CMP.CompanyId 
                    And P.consignorId = " + Session["CurrentWorkingCompanyID"] + " And INV.InvoiceDate>='" + txtFromDate.Text + "' And INV.INVOICEDate<='" + txttoDate.Text + "'";

            if (DDCustomerCode.SelectedIndex > 0)
            {
                str = str + " And CI.CustomerId=" + DDCustomerCode.SelectedValue + "";
            }
            if (DDInvoiceNo.SelectedIndex > 0)
            {
                str = str + " And INV.InvoiceId=" + DDInvoiceNo.SelectedValue + "";
            }

            str = str + " group by P.consignorId, CMP.CompanyName,CMP.CompAddr1,CMP.CompTel,CI.CustomerName,CI.CustomerCode,INV.TInvoiceNo,InvoiceDate,CC.CurrencyName,v.RecAmt,V.DBKAmt,INV.License_Commission,INV.Agent_Commission,Bldt";
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptInvoiceDetail.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptInvoiceDetail.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
        }
        catch (Exception ex)
        {
            lblErrormsg.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
}