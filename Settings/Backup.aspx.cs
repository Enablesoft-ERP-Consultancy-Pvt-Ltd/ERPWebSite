using IExpro.Core.Common;
using IExpro.Infrastructure.Services;
using IExpro.Web.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml.Xsl;

public partial class Settings_Backup : BasePage
{
    BackupService bckpSrv;
    public Settings_Backup()
    {
        var connectionString = ConfigurationManager.ConnectionStrings["IExproContext"].ConnectionString;
        var backupFolder = ConfigurationManager.AppSettings["BackupFolder"];
        this.bckpSrv = new BackupService(connectionString, backupFolder);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDatabase();
            BindBackup();
        }

    }
    protected void BindDatabase()
    {
        ddlDatabase.DataSource = this.bckpSrv.GetAllUserDatabases().Select((x, index) => new { Value = index++, Name = x });
        ddlDatabase.DataTextField = "Name";
        ddlDatabase.DataValueField = "Value";
        ddlDatabase.DataBind();
    }

    protected void BindBackup()
    {
        rptBackup.DataSource = this.bckpSrv.BackupList();
        rptBackup.DataBind();
    }

    private void DownloadFile(string file)
    {
        using (FileStream fs = new FileStream(file, FileMode.Open))
        {
            byte[] buffer = new byte[4096];
            int count = 0;

            Response.AddHeader("Content-Disposition", "attachment;filename=" + fs.Name);
            while ((count = fs.Read(buffer, 0, buffer.Length)) > 0)
            {
                Response.OutputStream.Write(buffer, 0, count);
                Response.Flush();
            }
        }
    }

    protected void rptBackup_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            int _backupId = int.Parse(((HiddenField)e.Item.FindControl("hdnBackupId")).Value);
            string filePath = ((HiddenField)e.Item.FindControl("hdnPath")).Value;
            if (e.CommandName == "View")
            {
                DownloadFile(filePath);
            }
            else if (e.CommandName == "Delete")
            {
                var result = this.bckpSrv.DelBackUp(_backupId);
                if (result > 0)
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    BindBackup();
                }

            }
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (ddlDatabase.SelectedValue != "")
        {
            bckpSrv.BackupDatabase(ddlDatabase.SelectedItem.Text);
        }
    }
}