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
        bool IsUpdated = false;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.IsUpdated = Convert.ToBoolean(ConfigurationManager.AppSettings["IsUpdate"]);
            if (this.IsUpdated)
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

