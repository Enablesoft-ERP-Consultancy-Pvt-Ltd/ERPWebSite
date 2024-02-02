using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

public partial class Masters_Sample_frmsamplemaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            UtilityModule.ConditionalComboFill(ref DDsizetype, "select Val,type from sizetype", false, "");
            UtilityModule.ConditionalComboFill(ref DDbuyer, "select customerid,CustomerCode+'/'+CompanyName as Customer from customerinfo  order by customer", true, "--Plz Select--");
            if (DDbuyer.Items.Count > 0)
            {
                DDbuyer.SelectedIndex = 0;
            }
            hnid.Value = "0";
        }
    }
    protected void DDpurpose_SelectedIndexChanged(object sender, EventArgs e)
    {
        TDbuyer.Visible = false;
        TDgeneral.Visible = true;
        switch (DDpurpose.SelectedIndex)
        {
            case 0:
                break;
            case 1:
                TDbuyer.Visible = true;
                TDgeneral.Visible = false;
                break;
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
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
            SqlParameter[] param = new SqlParameter[14];
            param[0] = new SqlParameter("@ID", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hnid.Value == "" ? "0" : hnid.Value;
            param[1] = new SqlParameter("@SampleDate", txtDate.Text);
            param[2] = new SqlParameter("@Purpose", DDpurpose.SelectedIndex);
            param[3] = new SqlParameter("@Customerid", TDbuyer.Visible == true ? DDbuyer.SelectedValue : "0");
            param[4] = new SqlParameter("@Purposeval", TDgeneral.Visible == true ? txtgeneral.Text : "");
            param[5] = new SqlParameter("@Product", txtproduct.Text);
            param[6] = new SqlParameter("@Size", txtSize.Text);
            param[7] = new SqlParameter("@flagsize", DDsizetype.SelectedValue);
            param[8] = new SqlParameter("@Fabric", txtFabric.Text);
            param[9] = new SqlParameter("@Description", txtdesc.Text);
            param[10] = new SqlParameter("@userid", Session["varuserid"]);
            param[11] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[12] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[12].Direction = ParameterDirection.Output;
            param[13] = new SqlParameter("@Samplecode", SqlDbType.VarChar, 50);
            param[13].Direction = ParameterDirection.Output;
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_generatesamplecode", param);
            Tran.Commit();
            lblmsg.Text = param[12].Value.ToString();
            hnid.Value = param[0].Value.ToString();
            txtsamplecode.Text = param[13].Value.ToString();
            //Save Image            
            SaveImage(Convert.ToInt32(hnid.Value));
            refreshcontrol();
            //
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void refreshcontrol()
    {
        txtDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        DDpurpose.SelectedIndex = 0;
        DDpurpose_SelectedIndexChanged(DDpurpose, new EventArgs());
        txtgeneral.Text = "";
        txtproduct.Text = "";
        txtSize.Text = "";
        txtFabric.Text = "";
        txtdesc.Text = "";
        lblimage.ImageUrl = null;
        hnid.Value = "0";
    }
    protected void SaveImage(int Id)
    {
        if (PhotoImage.FileName != "")
        {
            string filename = Path.GetFileName(PhotoImage.PostedFile.FileName);
            string Folderpath = Server.MapPath("../../SampleImage");
            //Check folder
            if (!Directory.Exists(Folderpath))
            {
                Directory.CreateDirectory(Folderpath);
            }
            //
            string targetPath = Server.MapPath("../../SampleImage/" + Id + "_Sample.gif");
            
            FileInfo file = new FileInfo(targetPath);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
            }  

            string img = "~\\SampleImage\\" + Id + "_Sample.gif";
            //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
            Stream strm = PhotoImage.PostedFile.InputStream;
            var targetFile = targetPath;
            if (PhotoImage.FileName != null && PhotoImage.FileName != "")
            {
                GenerateThumbnails(0.3, strm, targetFile);
            }
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update Samplecode Set imgpath='" + img + "' Where id=" + Id + "");
            lblimage.ImageUrl = img + "?" + DateTime.Now.Ticks.ToString();
        }
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
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        hnid.Value = "0";
        if (chkedit.Checked == true)
        {
            TRSamplecode.Visible = true;
            btndelete.Visible = true;
        }
        else
        {
            TRSamplecode.Visible = false;
            btndelete.Visible = false;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string str = @"select ID,Samplecode,Replace(convert(nvarchar(11),sampledate,106),' ','-') as SampleDate,Purpose,Customerid,Purposeval,Product,Size,flagsize,Fabric,Description
                    ,isnull(imgpath,'') as imgpath From Samplecode  where Samplecode='" + txttypesamplecode.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            hnid.Value = ds.Tables[0].Rows[0]["Id"].ToString();
            txtsamplecode.Text = ds.Tables[0].Rows[0]["samplecode"].ToString();
            txtDate.Text = ds.Tables[0].Rows[0]["SampleDate"].ToString();
            DDpurpose.SelectedIndex = Convert.ToInt16(ds.Tables[0].Rows[0]["Purpose"]);
            DDpurpose_SelectedIndexChanged(DDpurpose, e);
            switch (ds.Tables[0].Rows[0]["Purpose"].ToString())
            {
                case "0":
                    txtgeneral.Text = ds.Tables[0].Rows[0]["purposeval"].ToString();
                    break;
                case "1":
                    DDbuyer.SelectedValue = ds.Tables[0].Rows[0]["customerid"].ToString();
                    break;
            }
            txtproduct.Text = ds.Tables[0].Rows[0]["Product"].ToString();
            txtSize.Text = ds.Tables[0].Rows[0]["Size"].ToString();
            DDsizetype.SelectedValue = ds.Tables[0].Rows[0]["flagsize"].ToString();
            txtFabric.Text = ds.Tables[0].Rows[0]["fabric"].ToString();
            txtdesc.Text = ds.Tables[0].Rows[0]["Description"].ToString();
            if (ds.Tables[0].Rows[0]["imgpath"] != "")
            {
                lblimage.ImageUrl = ds.Tables[0].Rows[0]["imgpath"].ToString() + "?" + DateTime.Now.Ticks.ToString();

            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Invalid Sample code!');", true);
            txttypesamplecode.Focus();
        }
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            string str = "select isnull(imgpath,'') as imgpath from samplecode where id=" + hnid.Value;
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["imgpath"] != "")
                {
                    if (File.Exists(Server.MapPath(ds.Tables[0].Rows[0]["imgpath"].ToString())))
                    {
                        File.Delete(Server.MapPath(ds.Tables[0].Rows[0]["imgpath"].ToString()));
                    }
                }
                str = "Delete from samplecode Where Id=" + hnid.Value;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            }
            Tran.Commit();
            hnid.Value = "0";
            lblmsg.Text = "Sample code deleted successfully.";
            txttypesamplecode.Text = "";
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;

        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = @"select ID,Samplecode,SampleDate,case When Purpose=0 Then 'GENERAL' WHEN Purpose=1 Then 'BUYER' Else '' End As Purpose,
                    case When S.Customerid>0 Then CI.CustomerCode+'/'+ci.CompanyName Else S.Purposeval End as Purposeval,
                    S.Product,S.Size,St.Type as flagsize,S.Fabric,S.Description,S.Imgpath
                    from Samplecode  S left join customerinfo CI on S.Customerid=Ci.CustomerId
                    left join SizeType ST on S.flagsize=ST.Val Where S.samplecode='" + txtsamplecode.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].Columns.Add("image", typeof(System.Byte[]));
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["imgpath"].ToString() != "")
                {
                    FileInfo file = new FileInfo(Server.MapPath(dr["imgpath"].ToString()));
                    if (file.Exists)
                    {
                        string img = dr["imgpath"].ToString();
                        img = Server.MapPath(img);
                        Byte[] img_byte = File.ReadAllBytes(img);
                        dr["image"] = img_byte;
                    }
                }
            }
            Session["rptFileName"] = "~\\Reports\\rptsamplemaster.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptsamplemaster.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }
    }
}