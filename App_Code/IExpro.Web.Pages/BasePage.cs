using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BasePage
/// </summary>
/// 

namespace IExpro.Web.Pages
{
    public class BasePage : System.Web.UI.Page
    {
        bool IsNewTheme = false;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.IsNewTheme = Convert.ToBoolean(ConfigurationManager.AppSettings["IsNewTheme"]);
            if (this.IsNewTheme)
            {

                this.MasterPageFile = "~/App.master";
            }
            else
            {
                this.MasterPageFile = "~/Site.master";
            }
        }



    }
}

