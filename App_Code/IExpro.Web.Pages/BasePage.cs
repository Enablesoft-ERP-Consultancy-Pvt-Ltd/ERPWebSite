using IExproERP.UI.Models;
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
        protected virtual new CustomSerializePrincipal User
        { get; set; }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.IsNewTheme = Convert.ToBoolean(ConfigurationManager.AppSettings["IsNewTheme"]);

            this.User = new CustomSerializePrincipal()
            {
                CompanyId = Session["CurrentWorkingCompanyID"] != null ? Convert.ToInt32(Session["CurrentWorkingCompanyID"]) : 0,
                IExproId = Session["varCompanyId"] != null ? Convert.ToInt32(Session["varCompanyId"]) : 0,
                UserId = Session["varuserid"] != null ? Convert.ToInt32(Session["varuserid"]) : 0
            };

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

