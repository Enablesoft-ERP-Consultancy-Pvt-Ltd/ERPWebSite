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
public partial class Masters_Carpet_AddPhotoRefImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["img"].ToString() == "pp")
            {
                if (Request.QueryString["PPI"] != null)
                {
                    newPreview1.ImageUrl = "~/PurchaseImage/" + Request.QueryString["SrNo"] + "_PindentIssueImage.gif";
                }
                else
                {
                    newPreview1.ImageUrl = "~/PurchaseImage/" + Request.QueryString["SrNo"] + "_PindentImage.gif";
                }
            }
            else if (Request.QueryString["img"].ToString() == "COSTING")
            {
                string str = "select Costingid,ImageName from costingimage where Costingid=" + Request.QueryString["srno"];
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string FileName, Fileext;
                    FileName = ds.Tables[0].Rows[0]["Imagename"].ToString();
                    Fileext = FileName.Substring(FileName.LastIndexOf("."));
                    newPreview1.ImageUrl = "~/CostingImage/" + Request.QueryString["SrNo"] + Fileext;
                }

            }
            else if (Request.QueryString["img"].ToString() == "FrmCostingMaster")
            {
                string str = "Select CostingItemMasterID, IsNull(ImageName, '') ImageName From CostingItemMaster(Nolock) Where IsNull(ImageName, '') <> '' And CostingItemMasterID = " + Request.QueryString["srno"];

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string FileName, Fileext;
                    FileName = ds.Tables[0].Rows[0]["ImageName"].ToString();
                    Fileext = FileName.Substring(FileName.LastIndexOf("."));
                    newPreview1.ImageUrl = "~/FrmCostingMaster/" + Request.QueryString["SrNo"] + Fileext;
                }
            }
            else
                newPreview1.ImageUrl = "~/OrderImage/" + Request.QueryString["SrNo"] + "_OrderImage.gif";
        }
    }
    protected void BTNSAVE_Click(object sender, EventArgs e)
    {
        string img = "";
        string imagpath = "";

        int VarOrderDetailId = Convert.ToInt32(Request.QueryString["SrNo"]);
        if (PhotoImage.HasFile)
        {
            if (Request.QueryString["img"].ToString() == "pp")
            {
                string file = Path.GetFileName(PhotoImage.PostedFile.FileName);
                //For Direct Purchase Order
                if (Request.QueryString["PPI"] != null)
                {
                    img = "~\\PurchaseImage\\" + VarOrderDetailId + "_PindentIssueImage.gif";
                    imagpath = Server.MapPath(img);
                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update PurchaseIndentissueTran Set ImagePath='" + img + "' Where Pindentissuetranid=" + VarOrderDetailId);
                    FileInfo TheFile = new FileInfo(Server.MapPath("~/PurchaseImage/") + VarOrderDetailId + "_PindentIssueImage.gif");
                }
                else
                {
                    img = "~\\PurchaseImage\\" + VarOrderDetailId + "_PindentImage.gif";
                    imagpath = Server.MapPath(img);
                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update PurchaseIndentDetail Set ImageName='" + img + "' Where PIndentDetailId=" + VarOrderDetailId);
                    FileInfo TheFile = new FileInfo(Server.MapPath("~/PurchaseImage/") + VarOrderDetailId + "_PindentImage.gif");
                }
                //if (TheFile.Exists)
                //{
                //    File.Delete(MapPath("~/PurchaseImage/") + VarOrderDetailId + "_PindentImage.gif");
                //}
                if (PhotoImage.FileName != null && PhotoImage.FileName != "")
                {
                    // string imgCompney = Server.MapPath("~/PurchaseImage/") + VarOrderDetailId + "_PindentImage.gif";

                    PhotoImage.SaveAs(imagpath);
                }
            }
            else if (Request.QueryString["img"].ToString() == "COSTING")
            {
                string file = Path.GetFileName(PhotoImage.PostedFile.FileName);
                string Fileext = file.Substring(file.LastIndexOf("."));
                img = "~\\CostingImage\\" + VarOrderDetailId + Fileext;
                imagpath = Server.MapPath(img);
                int Imgsize = PhotoImage.PostedFile.ContentLength / 1024; //kb

                //***********Insert into DB
                string str = @"Delete from costingImage Where CostingID=" + VarOrderDetailId + @"
                           Insert into costingImage(CostingID,ImageName,ImgSize)values('" + VarOrderDetailId + "','" + file + "' ," + Imgsize + ")";
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                PhotoImage.SaveAs(imagpath);
            }
            else if (Request.QueryString["img"].ToString() == "FrmCostingMaster")
            {
                string file = Path.GetFileName(PhotoImage.PostedFile.FileName);
                string Fileext = file.Substring(file.LastIndexOf("."));
                img = "~\\FrmCostingMaster\\" + VarOrderDetailId + Fileext;
                imagpath = Server.MapPath(img);
                int Imgsize = PhotoImage.PostedFile.ContentLength / 1024; //kb

                //***********Insert into DB
                string str = @"Update CostingItemMaster Set ImageName = '" + file + "',ImgSize = " + Imgsize + " Where CostingItemMasterID = " + VarOrderDetailId;

                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                PhotoImage.SaveAs(imagpath);
            }
            else
            {
                string file = Path.GetFileName(PhotoImage.PostedFile.FileName);
                img = "~\\OrderImage\\" + VarOrderDetailId + "_OrderImage.gif";
                imagpath = Server.MapPath(img);
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update OrderDetail Set ImageName='" + img + "' Where OrderDetailId=" + VarOrderDetailId);

                FileInfo TheFile = new FileInfo(Server.MapPath("~/OrderImage/") + VarOrderDetailId + "_OrderImage.gif");
                if (TheFile.Exists)
                {
                    File.Delete(MapPath("~/OrderImage/") + VarOrderDetailId + "_OrderImage.gif");
                }
                if (PhotoImage.FileName != null && PhotoImage.FileName != "")
                {
                    string imgCompney = Server.MapPath("~/OrderImage/") + VarOrderDetailId + "_OrderImage.gif";
                    PhotoImage.SaveAs(imgCompney);
                }
            }
        }
    }
}