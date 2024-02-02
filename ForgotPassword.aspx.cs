using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Data;
public partial class ForgotPassword : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    string query;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (this.txtimgcode.Text == this.Session["CaptchaImageText"].ToString())
        {
            DataSet ds = new DataSet();
            query = "Select UserPwd from CompanyLogin where Email='" + txtEmail.Text + "'";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            sda.Fill(ds);
            var fromAddress = new MailAddress("design.home86@gmail.com", "Ashish");
            var toAddress = new MailAddress(txtEmail.Text);
            const string fromPassword = "";
            const string subject = "Export ERP";
            string body = "Your Export ERP EmailID  " + txtEmail.Text + " & Password is " + ds.Tables[0].Rows[0][0].ToString() + " So Enjoy The Export ERP...";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Email successfully sent!');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Image code is not valid!');", true);
        }
        this.txtimgcode.Text = "";
    }
}