using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class App : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Session["varMasterCompanyIDForERP"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            if (Session["varMasterCompanyIDForERP"].ToString() == "20")
            {
                //   trheader.Visible = false;
            }
            imgLogo.ImageUrl.DefaultIfEmpty();
            if (File.Exists(Server.MapPath("~/Images/Logo/" + Session["varMasterCompanyIDForERP"] + "_company.gif")))
            {
                imgLogo.ImageUrl = "~/Images/Logo/" + Session["varMasterCompanyIDForERP"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
            }
            LblCompanyName.Text = Session["varCompanyName"].ToString();
            LblUserName.Text = Session["varusername"].ToString();
            LblFrmName.Text = Page.Title.ToUpper().ToString();


        }
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
}
