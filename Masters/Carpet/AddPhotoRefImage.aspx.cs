using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO.Compression;
using CrystalDecisions.CrystalReports;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
public partial class Masters_Carpet_AddPhotoRefImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            if (Request.QueryString["orderdetailid"] != null && Request.QueryString["orderid"] != null)
            {
                DataSet ds = null;
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                try
                {
                    con.Open();
                    ds = null;
                    string sql;
                    sql = @"SELECT Photo  From OrderDetail Where orderdetailID=" + Request.QueryString["orderdetailid"] + " and orderid= " + Request.QueryString["orderid"] + @"
                                 select photo from ORDER_REFERENCEIMAGE where orderdetailid=" + Request.QueryString["orderdetailid"] + "";
                    ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.newPreview1.ImageUrl = ds.Tables[0].Rows[0]["Photo"].ToString();
                    }
                    else
                    {
                        this.newPreview1.Visible = false;
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        this.ImageReferenceImage.ImageUrl = ds.Tables[1].Rows[0]["Photo"].ToString();
                    }
                    else
                    {
                        this.ImageReferenceImage.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                    lblMessage.Visible = true;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                        con.Dispose();
                    }
                }
            }
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
    protected void BTNSAVE_Click(object sender, EventArgs e)
    {
        SqlConnection myConnection = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        myConnection.Open();
        SqlHelper.ExecuteNonQuery(myConnection, CommandType.Text, "DELETE TEMP_ORDER_PHOTO");
        SqlHelper.ExecuteNonQuery(myConnection, CommandType.Text, "DELETE TEMP_ORDER_REFERENCE");
        if (PhotoImage.FileName != "")
        {
            string filename = Path.GetFileName(PhotoImage.PostedFile.FileName);
            string targetPath = Server.MapPath("../../ImageDraftorder/" + Request.QueryString["orderdetailid"].ToString() + "orderdetail.gif");
            string img = "~\\ImageDraftorder\\" + Request.QueryString["orderdetailid"].ToString() + "orderdetail.gif";
            Stream strm = PhotoImage.PostedFile.InputStream;
            var targetFile = targetPath;

            FileInfo TheFile = new FileInfo(Server.MapPath("~\\ImageDraftorder\\") + Request.QueryString["orderdetailid"].ToString() + "orderdetail.gif");
            if (TheFile.Exists)
            {
                File.Delete(MapPath("~\\ImageDraftorder\\") + Request.QueryString["orderdetailid"].ToString() + "orderdetail.gif");
            }
            if (PhotoImage.FileName != null && PhotoImage.FileName != "")
            {
                GenerateThumbnails(0.3, strm, targetFile);
            }
            this.newPreview1.ImageUrl = img;
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update orderdetail set Photo='" + img + "' where orderdetailID=" + Request.QueryString["orderdetailid"] + " and orderid= " + Request.QueryString["orderid"] + " ");
            //byte[] myimage = new byte[PhotoImage.PostedFile.ContentLength];
            //HttpPostedFile Image = PhotoImage.PostedFile;
            //Image.InputStream.Read(myimage, 0, (int)PhotoImage.PostedFile.ContentLength);
            //SqlCommand storeimage = new SqlCommand("Insert into TEMP_ORDER_PHOTO(Photo) values(@image)", myConnection);
            //storeimage.Parameters.Add("@image", SqlDbType.Image, myimage.Length).Value = myimage;
            //System.Drawing.Image img = System.Drawing.Image.FromStream(PhotoImage.PostedFile.InputStream);
            //storeimage.ExecuteNonQuery();
        }
        if (FileReferenceImage.FileName != "")
        {
            //byte[] myimage = new byte[FileReferenceImage.PostedFile.ContentLength];
            //HttpPostedFile Image = FileReferenceImage.PostedFile;
            //Image.InputStream.Read(myimage, 0, (int)FileReferenceImage.PostedFile.ContentLength);
            //SqlCommand storeimage = new SqlCommand("Insert into TEMP_ORDER_REFERENCE(REFERENCEIMAGE) values(@image)", myConnection);
            //storeimage.Parameters.Add("@image", SqlDbType.Image, myimage.Length).Value = myimage;
            //System.Drawing.Image img = System.Drawing.Image.FromStream(FileReferenceImage.PostedFile.InputStream);
            //storeimage.ExecuteNonQuery();
            string filename = Path.GetFileName(FileReferenceImage.PostedFile.FileName);
            string targetPath = Server.MapPath("../../ImageDraftorder/" + Request.QueryString["orderdetailid"].ToString() + "ORDERref.gif");
            string img = "~\\ImageDraftorder\\" + Request.QueryString["orderdetailid"].ToString() + "ORDERref.gif";
            //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
            Stream strm = FileReferenceImage.PostedFile.InputStream;
            var targetFile = targetPath;
            FileInfo TheFile = new FileInfo(Server.MapPath("~\\ImageDraftorder\\") + Request.QueryString["orderdetailid"].ToString() + "ORDERref.gif");
            if (TheFile.Exists)
            {
                File.Delete(MapPath("~\\ImageDraftorder\\") + Request.QueryString["orderdetailid"].ToString() + "ORDERref.gif");
            }
            if (FileReferenceImage.FileName != null && FileReferenceImage.FileName != "")
            {
                GenerateThumbnails(0.3, strm, targetFile);
            }

            this.ImageReferenceImage.ImageUrl = img;
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete ORDER_REFERENCEIMAGE where orderdetailid=" + Request.QueryString["orderdetailid"].ToString() + " Insert into ORDER_REFERENCEIMAGE(OrderDetailId,Photo) values(" + Request.QueryString["orderdetailid"].ToString() + ",'" + img + "')");
        }
        myConnection.Close();
        myConnection.Dispose();
        //if (Session["varcompanyno"].ToString() == "7" && Request.QueryString["orderid"].ToString() != "" && Request.QueryString["orderdetailid"].ToString() != "")
        //{
        //    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "UPDATE ORDERDETAIL SET PHOTO=(SELECT PHOTO FROM TEMP_ORDER_PHOTO) WHERE ORDERID=" + Request.QueryString["orderid"].ToString() + " AND ORDERDETAILID=" + Request.QueryString["orderdetailid"].ToString()+ " " + "  INSERT INTO ORDER_REFERENCEIMAGE SELECT " + Request.QueryString["orderdetailid"].ToString() + ",REFERENCEIMAGE FROM TEMP_ORDER_REFERENCE ");
        //}
    }
}