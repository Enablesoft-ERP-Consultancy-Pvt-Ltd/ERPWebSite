using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MySession
/// </summary>
public class MySession
{
    private static System.Web.SessionState.HttpSessionState Session
    {
        get
        {
            return HttpContext.Current.Session;
        }
    }
    public static string ProductionEditPwd
    {
        get
        {
            if (Session["ProductionEditPwd"] == null)
                Session["ProductionEditPwd"] = "";
            return (string)Session["ProductionEditPwd"];
        }
        set
        {
            Session["ProductionEditPwd"] = value;
        }
    }
    public static string IndentAsProduction
    {
        get
        {
            if (Session["IndentAsProduction"] == null)
                Session["IndentAsProduction"] = "";
            return (string)Session["IndentAsProduction"];
        }
        set
        {
            Session["IndentAsProduction"] = value;
        }
    }
    public static string InvoiceReportType
    {
        get
        {
            if (Session["InvoiceReportType"] == null)
                Session["InvoiceReportType"] = "";
            return (string)Session["InvoiceReportType"];

        }
        set
        {
            Session["InvoiceReportType"] = value;
        }
    }
    public static string RoundMtrFlag
    {
        get
        {
            if (Session["RoundMtrFlag"] == null)
                Session["RoundMtrFlag"] = "";
            return (string)Session["RoundMtrFlag"];

        }
        set
        {
            Session["RoundMtrFlag"] = value;
        }
    }

    public static string RoundFtFlag
    {
        get
        {
            if (Session["RoundFtFlag"] == null)
                Session["RoundFtFlag"] = "";
            return (string)Session["RoundFtFlag"];

        }
        set
        {
            Session["RoundFtFlag"] = value;
        }
    }
    public static string Stockapply
    {
        get
        {
            if (Session["Stockapply"] == null)
                Session["Stockapply"] = "";
            return (string)Session["Stockapply"];

        }
        set
        {
            Session["Stockapply"] = value;
        }
    }
    public static string TagNowise
    {
        get
        {
            if (Session["TagNowise"] == null)
                Session["TagNowise"] = "";
            return (string)Session["TagNowise"];

        }
        set
        {
            Session["TagNowise"] = value;
        }
    }
}