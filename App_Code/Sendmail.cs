using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Configuration;
using System.Net.Mail;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Sendmail
/// </summary>
public class Sendmail
{
    public static bool SendMail(string pTo, string pSubject, string pBody, string pFromSMTP, string pFromDispName = "", string pAttachments = "", string pCC = "", string pAttchmntName = "", string pBCC = "")
    {
        try
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * From Mailsetting");
            if (ds.Tables[0].Rows.Count > 0)
            {
                SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("mailsettings/" + pFromSMTP);
                MailMessage objMail = new MailMessage();
                if (pTo == "")
                {
                    pTo = ds.Tables[0].Rows[0]["Receiveaddress"].ToString();
                }

                objMail.To.Add(pTo);
                if (pCC != "") { objMail.CC.Add(pCC); }
                if (pBCC != "") { objMail.Bcc.Add(pBCC); }
                objMail.Subject = pSubject;
                objMail.Body = pBody;
                objMail.IsBodyHtml = true;
                //foreach (string pAttachment in pAttachments.Split('|'))
                //{
                //    if ((pAttachment != ""))
                //    {
                //        string fileName = pAttchmntName == "" ? Path.GetFileName(pAttachment) : pAttchmntName;
                //        objMail.Attachments.Add(new Attachment(new MemoryStream(File.ReadAllBytes(pAttachment)), fileName));
                //    }
                //}


                string fileName = pAttchmntName == "" ? Path.GetFileName(pAttachments) : pAttchmntName;
                objMail.Attachments.Add(new Attachment(new MemoryStream(File.ReadAllBytes(pAttachments)), fileName));
                // objMail.From = new MailAddress(smtpSection.From, pFromDispName);
                //SmtpClient smClient = new SmtpClient(smtpSection.Network.Host, smtpSection.Network.Port);
                //smClient.EnableSsl = smtpSection.Network.EnableSsl;
                //smClient.UseDefaultCredentials = smtpSection.Network.DefaultCredentials;
                //smClient.Credentials = new System.Net.NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);
                if (pFromDispName == "")
                {
                    pFromDispName = ds.Tables[0].Rows[0]["DisplayName"].ToString();
                }
                objMail.From = new MailAddress(ds.Tables[0].Rows[0]["smtpuser"].ToString(), pFromDispName);
                SmtpClient smClient = new SmtpClient()
                {
                    Host = ds.Tables[0].Rows[0]["smtpserver"].ToString(),
                    Port = Convert.ToInt32(ds.Tables[0].Rows[0]["smtpPort"]),
                    EnableSsl = Convert.ToBoolean(ds.Tables[0].Rows[0]["Enablessl"]),
                    Credentials = new System.Net.NetworkCredential(ds.Tables[0].Rows[0]["smtpuser"].ToString(), ds.Tables[0].Rows[0]["smtppass"].ToString()),
                   
                };
                ////string xyz = "Sending mail async";
                ////smClient.SendAsync(objMail, xyz);
             
                smClient.Send(objMail);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }
}