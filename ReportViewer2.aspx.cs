using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

public partial class Default23 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScript", "SetBrowserMode();", true);
        //string path = Request.QueryString["ReportName"];
        //string function = Request.QueryString["CommanFn"];
        string path = Session["ReportPath"].ToString();
        string function = Session["CommanFormula"].ToString();
        LblFormula.Visible = true;
        LblPath.Visible = true;
        LblPath.Text = path;
        LblFormula.Text = function;
        FileInfo ReportPath=new FileInfo (Server.MapPath(path));
        if (ReportPath.Exists)
        {
            loadReport(Session["ReportPath"].ToString(), Session["CommanFormula"].ToString());
            CrystalReportViewer1.RefreshReport();
        }
        else
        {
            Response.Write("Report Path Not Exists");
        }
    }
    private void loadReport(string ReportPath, string CommanFormula)
    {
        string VarA = Request.QueryString["A"];
        string VarB = Request.QueryString["B"];
        string VarC = Request.QueryString["c"];
        string VarD = Request.QueryString["D"];
        string varServer=ConfigurationManager.AppSettings["varServerName"];
        string varxyz = ConfigurationManager.AppSettings["varPasswordName"];
        string UserName = ConfigurationManager.AppSettings["VaruserName"];
        string VarBaseName = ConfigurationManager.AppSettings["VarBaseName"];
        ReportDocument crystalreport1 = new ReportDocument();
        crystalreport1.Load(Server.MapPath(ReportPath));
        //crystalreport1.SetDatabaseLogon("sa", "eit", varServer);
        crystalreport1.SetDatabaseLogon(UserName, varxyz, varServer, VarBaseName);
       // crystalreport1.DataDefinition.FormulaFields["ExcessMatBal"].Text =Var;
        crystalreport1.OpenSubreport("RptDyerReceiveDetail").DataDefinition.FormulaFields["ExcessMatBal"].Text = VarA;
        crystalreport1.OpenSubreport("RptDyerReceiveDetail").DataDefinition.FormulaFields["ExcessMatBalAmt"].Text = VarB;
        crystalreport1.OpenSubreport("RptDyerReceiveDetail").DataDefinition.FormulaFields["Price"].Text = VarC;
        crystalreport1.OpenSubreport("RptDyerReceiveDetail").DataDefinition.FormulaFields["FinalHissab"].Text = VarD;        
        crystalreport1.RecordSelectionFormula = CommanFormula;
        CrystalReportViewer1.ReportSource = crystalreport1;
        CrystalReportViewer1.DataBind();
    }
}
