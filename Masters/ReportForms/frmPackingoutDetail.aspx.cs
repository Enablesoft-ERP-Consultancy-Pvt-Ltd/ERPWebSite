using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_frmPackingoutDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }

    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = "";
        if (Session["VarCompanyNo"].ToString() == "36")
        {
            str = @"select PM.TInvoiceNo,vf.QualityName,vf.designName,vf.ColorName,PD.Width+'x'+PD.Length as Size,
                        sum(Pd.pcs) as pcs,Sum(PD.area) as Area,'" + txtfromdate.Text + "' as Fromdate,'" + txttodate.Text + @"' as Todate,dbo.F_GetstockNo(PD.ID) As StockNo
                        From Packing PM inner join PackingInformation PD on PM.PackingId=PD.PackingId
                        inner join V_FinishedItemDetail vf on PD.FinishedId=vf.ITEM_FINISHED_ID
                        Where PM.ConsignorId = " + Session["CurrentWorkingCompanyID"] + " And PM.PackingDate>='" + txtfromdate.Text + "' and PM.PackingDate<='" + txttodate.Text + @"'
                        group by PM.TInvoiceNo,vf.QualityName,vf.designName,vf.ColorName,PD.Width,PD.Length,PD.ID";
        }
        else
        {
             str = @"select PM.TInvoiceNo,vf.QualityName,vf.designName,vf.ColorName,PD.Width+'x'+PD.Length as Size,
                        sum(Pd.pcs) as pcs,Sum(PD.area) as Area,'" + txtfromdate.Text + "' as Fromdate,'" + txttodate.Text + @"' as Todate
                        From Packing PM inner join PackingInformation PD on PM.PackingId=PD.PackingId
                        inner join V_FinishedItemDetail vf on PD.FinishedId=vf.ITEM_FINISHED_ID
                        Where PM.ConsignorId = " + Session["CurrentWorkingCompanyID"] + " And PM.PackingDate>='" + txtfromdate.Text + "' and PM.PackingDate<='" + txttodate.Text + @"'
                        group by PM.TInvoiceNo,vf.QualityName,vf.designName,vf.ColorName,PD.Width,PD.Length";
        }
  

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Session["VarCompanyNo"].ToString() == "36")
            {
                Session["rptFileName"] = "~\\Reports\\RptPackingoutDetailPrasad.rpt";
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\RptPackingoutDetail.rpt";
            }

            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptPackingoutDetail.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No Records Found..')", true);
        }

    }
}