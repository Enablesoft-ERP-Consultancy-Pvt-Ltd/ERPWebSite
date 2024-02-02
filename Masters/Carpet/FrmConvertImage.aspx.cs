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
using System.IO;
using System.Text;

public partial class Masters_Carpet_FrmConvertImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (DDlType.SelectedValue == "0")
        {
            draftorder();
        }
        else if (DDlType.SelectedValue == "1")
        {
            ordermaster();
        }
        else if (DDlType.SelectedValue == "2")
        {
            Item_image();
        }
        else if (DDlType.SelectedValue == "3")
        {
            Ref_draftorder();
        }
        else if (DDlType.SelectedValue == "4")
        {
            Ref_order();
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
    private void draftorder()
    {//where OrderDetailId>481
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select OrderDetailId,Photo from DRAFT_ORDER_DETAIL   order by orderdetailid ");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["photo"].ToString() != "")
                {
                    Random rnd = new Random();
                    //this.imgLogo.Visible = true;
                    byte[] buffer = (byte[])ds.Tables[0].Rows[i]["photo"];
                    System.IO.MemoryStream mStrm = new System.IO.MemoryStream(buffer);
                    System.Drawing.Image im = System.Drawing.Image.FromStream(mStrm);
                    string targetPath = Server.MapPath("../../ImageDraftorder/d" + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "draftdetail.gif");
                    string img = "~\\ImageDraftorder\\d" + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "draftdetail.gif";
                    FileInfo TheFile = new FileInfo(Server.MapPath("~\\ImageDraftorder\\d") + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "draftdetail.gif");
                    if (TheFile.Exists)
                    {
                        File.Delete(MapPath("~\\ImageDraftorder\\d") + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "draftdetail.gif");
                    }

                    {
                        GenerateThumbnails(0.3, mStrm, targetPath);
                    }
                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update DRAFT_ORDER_DETAIL Set Photo1='" + img + "' Where OrderDetailId=" + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "");
                }
                //this.imgLogo.ImageUrl = "logo.bmp?rnd=" + rnd.NextDouble();
            }
        }
    }
    private void ordermaster()
    {//WHERE OrderDetailId>422
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select OrderDetailId,Photo from orderdetail  ORDER BY OrderDetailId ");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["Photo"].ToString() != "")
                {

                    Random rnd = new Random();
                    //this.imgLogo.Visible = true;
                    byte[] buffer = (byte[])ds.Tables[0].Rows[i]["photo"];
                    System.IO.MemoryStream mStrm = new System.IO.MemoryStream(buffer);
                    System.Drawing.Image im = System.Drawing.Image.FromStream(mStrm);
                    string targetPath = Server.MapPath("../../ImageDraftorder/" + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "orderdetail.gif");
                    string img = "~\\ImageDraftorder\\" + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "orderdetail.gif";
                    FileInfo TheFile = new FileInfo(Server.MapPath("~\\ImageDraftorder\\") + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "orderdetail.gif");
                    if (TheFile.Exists)
                    {
                        File.Delete(MapPath("~\\ImageDraftorder\\") + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "orderdetail.gif");
                    }
                    {
                        GenerateThumbnails(0.3, mStrm, targetPath);
                    }
                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update orderdetail Set Photo1='" + img + "' Where OrderDetailId=" + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "");
                }
            }
        }
    }
    private void Item_image()
    {//where finishedid>2241
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select finishedid as orderdetailid,photo from main_item_image  order by finishedid ");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["photo"].ToString() != "")
                {
                    Random rnd = new Random();
                    //this.imgLogo.Visible = true;
                    byte[] buffer = (byte[])ds.Tables[0].Rows[i]["photo"];
                    System.IO.MemoryStream mStrm = new System.IO.MemoryStream(buffer);
                    System.Drawing.Image im = System.Drawing.Image.FromStream(mStrm);
                    string targetPath = Server.MapPath("../../Item_Image/" + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "_Item.gif");
                    string img = "~\\Item_Image\\" + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "_Item.gif";
                    FileInfo TheFile = new FileInfo(Server.MapPath("~\\Item_Image\\") + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "_Item.gif");
                    if (TheFile.Exists)
                    {
                        File.Delete(MapPath("~\\Item_Image\\") + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "_Item.gif");
                    }

                    {
                        GenerateThumbnails(0.3, mStrm, targetPath);
                    }
                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update main_item_image Set Photo1='" + img + "' Where finishedid=" + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "");
                    //this.imgLogo.ImageUrl = "logo.bmp?rnd=" + rnd.NextDouble();
                }
            }
        }
    }
    private void Ref_draftorder()
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select OrderDetailId,Photo from Draft_Order_ReferenceImage");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Random rnd = new Random();
                //this.imgLogo.Visible = true;
                byte[] buffer = (byte[])ds.Tables[0].Rows[i]["photo"];
                System.IO.MemoryStream mStrm = new System.IO.MemoryStream(buffer);
                System.Drawing.Image im = System.Drawing.Image.FromStream(mStrm);
                string targetPath = Server.MapPath("../../ImageDraftorder/d" + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "draftref.gif");
                string img = "~\\ImageDraftorder\\d" + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "draftref.gif";
                FileInfo TheFile = new FileInfo(Server.MapPath("~\\ImageDraftorder\\d") + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "draftref.gif");
                if (TheFile.Exists)
                {
                    File.Delete(MapPath("~\\ImageDraftorder\\d") + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "draftref.gif");
                }

                {
                    GenerateThumbnails(0.3, mStrm, targetPath);
                }
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update Draft_Order_ReferenceImage Set Photo1='" + img + "' Where OrderDetailId=" + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "");
                //this.imgLogo.ImageUrl = "logo.bmp?rnd=" + rnd.NextDouble();
            }
        }
    }
    private void Ref_order()
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select OrderDetailId,Photo from ORDER_REFERENCEIMAGE order by OrderDetailId ");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Random rnd = new Random();
                //this.imgLogo.Visible = true;
                byte[] buffer = (byte[])ds.Tables[0].Rows[i]["photo"];
                System.IO.MemoryStream mStrm = new System.IO.MemoryStream(buffer);
                System.Drawing.Image im = System.Drawing.Image.FromStream(mStrm);
                string targetPath = Server.MapPath("../../ImageDraftorder/" + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "ORDERref.gif");
                string img = "~\\ImageDraftorder\\" + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "ORDERref.gif";
                FileInfo TheFile = new FileInfo(Server.MapPath("~\\ImageDraftorder\\") + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "ORDERref.gif");
                if (TheFile.Exists)
                {
                    File.Delete(MapPath("~\\ImageDraftorder\\") + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "ORDERref.gif");
                }

                {
                    GenerateThumbnails(0.3, mStrm, targetPath);
                }
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update ORDER_REFERENCEIMAGE Set Photo1='" + img + "' Where OrderDetailId=" + ds.Tables[0].Rows[i]["orderdetailid"].ToString() + "");
                //this.imgLogo.ImageUrl = "logo.bmp?rnd=" + rnd.NextDouble();
            }
        }
    }
}

