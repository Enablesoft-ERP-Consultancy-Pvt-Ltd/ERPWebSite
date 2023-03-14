using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;

public partial class Masters_Payroll_frmapplyLeave : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDleavetype, "SELECT LEAVEID,NAME+space(5)+CODE FROM HR_LEAVETYPE", true, "--Plz Select--");
            txtdateofapplication.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtfrom.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtto.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            //calfrom.StartDate = System.DateTime.Now.AddDays(-3);
            //calto.StartDate = System.DateTime.Now.AddDays(-3);
            //caldateofapplication.StartDate = System.DateTime.Now.AddDays(-5);

            calculatedays();
        }
    }
    protected void btnsearchemp_Click(object sender, EventArgs e)
    {
        fillemployee();
        fillemployeeleavebalance();
    }
    protected void fillemployeeleavebalance()
    {
        try
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@empid", hnempid.Value);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "[PRO_HR_EMPLEAVEBALANCE]", param);
            DGleavebal.DataSource = ds.Tables[0];
            DGleavebal.DataBind();

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void fillemployee()
    {
        try
        {
            txtempname.Text = "";
            hnempid.Value = "0";
            string str = @"SELECT EI.EMPID,EI.EMPNAME FROM EMPINFO EI With (nolock) INNER JOIN HR_EMPLOYEEINFORMATION HEI With (nolock) ON EI.EMPID=HEI.EMPID
                         AND EI.EMPCODE='" + txtempcode.Text + "' AND HEI.RESIGNSTATUS=0";

            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                str = str + " And EI.USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR = 1";
            }

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                hnempid.Value = ds.Tables[0].Rows[0]["empid"].ToString();
                txtempname.Text = ds.Tables[0].Rows[0]["empname"].ToString();

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('Emp. Code does not exists in ERP or Status is Resigned !!!')", true);
                txtempcode.Text = "";
            }
            txtempcode.Focus();
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    protected void btnapply_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[12];
            param[0] = new SqlParameter("@Applicationid", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = 0;
            param[1] = new SqlParameter("@Dateofapplication", txtdateofapplication.Text);
            param[2] = new SqlParameter("@empid", hnempid.Value);
            param[3] = new SqlParameter("@LeaveTypeid", DDleavetype.SelectedValue);
            param[4] = new SqlParameter("@fromdate", txtfrom.Text);
            param[5] = new SqlParameter("@Todate", txtto.Text);
            param[6] = new SqlParameter("@Leaveperiod", rdisthalf.Checked == true ? 1 : (rdiindhalf.Checked == true ? 2 : 3));
            param[7] = new SqlParameter("@Reason", txtreasonforleave.Text);
            param[8] = new SqlParameter("@Address", txtaddressduringleave.Text);
            param[9] = new SqlParameter("@Userid", Session["varuserid"]);
            param[10] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[10].Direction = ParameterDirection.Output;
            param[11] = new SqlParameter("@leavecount", txtleavecount.Text == "" ? "0" : txtleavecount.Text);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_HR_SAVELEAVEAPPLICATION", param);
            Tran.Commit();
            if (param[10].Value.ToString() != "")
            {
                lblmsg.Text = param[10].Value.ToString();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('Leave Applied Successfully !!!')", true);
                lblmsg.Text = "Leave applied successfully....";
                //*****************Upload
                Savedocument(Convert.ToInt64(param[0].Value));
                //*******************Upload File
                fillemployeeleavebalance();
                Refreshcontrols();
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    private void Savedocument(long applicationid)
    {
        if (fileupload.FileName != "")
        {
            string filename = Path.GetFileName(fileupload.PostedFile.FileName);
            string Folderpath = Server.MapPath("../../HrLeavedocs");
            //Check folder
            if (!Directory.Exists(Folderpath))
            {
                Directory.CreateDirectory(Folderpath);
            }
            //
            string targetPath = Server.MapPath("../../HrLeavedocs/" + applicationid + ".gif");
            string img = "~\\HrLeavedocs\\" + applicationid + ".gif";
            //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
            Stream strm = fileupload.PostedFile.InputStream;
            var targetFile = targetPath;
            if (fileupload.FileName != null && fileupload.FileName != "")
            {
                GenerateThumbnails(0.3, strm, targetFile);
            }
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update hr_leaveapplication Set fileupload='" + img + "' Where applicationid=" + applicationid + "");
            //lblsignatureimage.ImageUrl = img + "?" + DateTime.Now.Ticks.ToString();
        }
    }

    private void Refreshcontrols()
    {
        txtdateofapplication.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        txtfrom.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        txtto.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        rdisthalf.Checked = false;
        rdiindhalf.Checked = false;
        rdboth.Checked = true;
        txtempcode.Text = "";
        txtempname.Text = "";
        hnempid.Value = "0";
        txtreasonforleave.Text = "";
        txtaddressduringleave.Text = "";
        txtleavecount.Text = "";
    }
    private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
    {
        using (var image = System.Drawing.Image.FromStream(sourcePath))
        {
            var newWidth = (int)(image.Width * scaleFactor);
            var newHeight = (int)(image.Height * scaleFactor);
            var thumbnailImg = new Bitmap(newWidth, newHeight);
            var thumbGraph = Graphics.FromImage(thumbnailImg);
            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbGraph.DrawImage(image, imageRectangle);
            thumbnailImg.Save(targetPath, image.RawFormat);
        }
    }
    protected void txtto_TextChanged(object sender, EventArgs e)
    {
        calculatedays();
    }
    protected void calculatedays()
    {
        DateTime dtfrom, dtto;
        dtfrom = DateTime.Parse(txtfrom.Text).Date;
        dtto = DateTime.Parse(txtto.Text).Date;
        TimeSpan ts;
        ts = dtto - dtfrom;
        decimal days = Convert.ToInt16(ts.TotalDays + 1);
        if (rdisthalf.Checked == true || rdiindhalf.Checked == true)
        {
            txtleavecount.Text = (days / 2).ToString();
        }
        else
        {
            txtleavecount.Text = days.ToString();

        }
    }
    protected void rdisthalf_CheckedChanged(object sender, EventArgs e)
    {
        calculatedays();
    }
    protected void rdiindhalf_CheckedChanged(object sender, EventArgs e)
    {
        calculatedays();
    }
    protected void rdboth_CheckedChanged(object sender, EventArgs e)
    {
        calculatedays();
    }
}