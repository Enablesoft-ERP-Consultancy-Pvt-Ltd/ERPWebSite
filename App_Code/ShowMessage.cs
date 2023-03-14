using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
/// <summary>
/// Summary description for ShowMessage
/// </summary>
public static class ShowMessage
{
    public static void Alert(string message)
    {
        string cleanMessage = message.Replace("'", "\\'");
        Page page = HttpContext.Current.CurrentHandler as Page;
        string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "')</script>";
        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
        {
            page.ClientScript.RegisterClientScriptBlock(typeof(ShowMessage), "alert", script);
        }
    }
}
