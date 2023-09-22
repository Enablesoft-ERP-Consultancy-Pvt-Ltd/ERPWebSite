using System;
using System.IO;
using System.Linq;
public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["varCompanyId"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                imgLogo.ImageUrl.DefaultIfEmpty();
                if (File.Exists(Server.MapPath("~/Images/Logo/" + Session["varCompanyId"] + "_company.gif")))
                {
                    imgLogo.ImageUrl = "~/Images/Logo/" + Session["varCompanyId"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
                }
                LblCompanyName.Text = Session["varCompanyName"].ToString();
                LblUserName.Text = Session["varusername"].ToString();
            }
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
}