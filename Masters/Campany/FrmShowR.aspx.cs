using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Masters_Campany_FrmShowR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
    }
    protected void preview_Click(object sender, EventArgs e)
    {
        Session["ReportPath"] = "Reports/RptInvoiceType_destiniNew.rpt";
        Session["CommanFormula"] = "{Invoice.InvoiceID}=" + ViewState["InvoiceId"] + " ";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
        Session["varMasterCompanyIDForERP"] = "6";
        string var = txtname.Text;
       
    }
}