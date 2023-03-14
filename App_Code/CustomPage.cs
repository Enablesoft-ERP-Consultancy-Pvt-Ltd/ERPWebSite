using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for WebPage
/// </summary>

public class CustomPage : System.Web.UI.Page
{
    protected override void OnPreInit(EventArgs e)        
    {
        base.OnPreInit(e);
        if (Session["CTHEME"] != null)
            Theme = Session["CTHEME"].ToString();
        else
            Theme = "Default";
    }

    
}

