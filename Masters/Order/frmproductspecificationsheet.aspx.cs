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
using System.Text;

public partial class Masters_Order_frmproductspecificationsheet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CI.CompanyId,CI.CompanyName from CompanyInfo CI inner join Company_Authentication CA on Ci.CompanyId=CA.CompanyId
                  WHere CI.MasterCompanyid=" + Session["varcompanyid"] + " and CA.UserId=" + Session["varuserid"] + @"  order by CompanyName";
            UtilityModule.ConditionalComboFill(ref DDcompanyName, str, true, "Plz Select--");

            if (DDcompanyName.Items.Count > 0)
            {
                DDcompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompanyName.Enabled = false;
            }

            switch (Session["usertype"].ToString())
            {
                case "1":                
                    btnApprove.Visible = true;
                    Changeapprovebuttoncolor(0);
                    break;
                default:
                    btnApprove.Visible = false;
                    break;
            }
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
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@Docid", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hndocid.Value;
            param[1] = new SqlParameter("@Companyid", DDcompanyName.SelectedValue);
            param[2] = new SqlParameter("@DocNo", SqlDbType.VarChar, 50);
            param[2].Value = txtdocno.Text;
            param[2].Direction = ParameterDirection.InputOutput;
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            //*********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEPRODUCTSPECIFICATIONMASTER", param);
            lblmsg.Text = param[4].Value.ToString();
            txtdocno.Text = param[2].Value.ToString();
            hndocid.Value = param[0].Value.ToString();

            // at the time of update delete all the data in tables
            string str1 = @"DELETE FROM PRODUCTIONSPECIFICATIONDETAIL WHERE DOCID=" + hndocid.Value;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);

            insertinto_ProductSpecificationDetail(Tran);

            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + param[4].Value.ToString() + "')", true);

            Tran.Commit();
            //**********Upload image
            SaveImage(Convert.ToInt32(hndocid.Value));
            //**********

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

    private void insertinto_ProductSpecificationDetail(SqlTransaction Tran)
    {
        string str = @"Insert into PRODUCTIONSPECIFICATIONDETAIL(DOCID,DATE,PRODUCTDEVELOPER,MERCHANDISER,BUYERNAME,TYPEOFPRODUCT,DESIGNNAME,COLORS,WEIGHTPCS_RAW,WEIGHTPCS_FINISH,WOOL,COTTON,VISCOSE,TUFTINGCLOTH,
                        THIRDBACKINGCLOTH,NIWARWIDTH,TYPEOFDYEING,WARP,WEFT,REED,PICKS,TUFTDENSITY,WEAVINGPATTERN,PILEHEIGHT_RAW,PILEHEIGHT_FINISH,WASHING,LATEXRECIPE,TYPEOFYARN,
                        PLYOFYARN,STITCHPERINCH,TOLERENCE_LENGTH,TOLERENCE_WIDTH,TOLERENCE_WEIGHT,PROCESSFLOW,PileHeight_RawCut,PileHeight_FinishCut,SizeTolerenceActualSize1,
                        SizeTolerenceActualSize2,SizeTolerenceActualSize3,SizeTolerenceActualSize4,SizeTolerenceActualSize5,SizeTolerenceLength1,SizeTolerenceLength2,SizeTolerenceLength3,
                        SizeTolerenceLength4,SizeTolerenceLength5,SizeTolerenceBreadth1,SizeTolerenceBreadth2,SizeTolerenceBreadth3,SizeTolerenceBreadth4,SizeTolerenceBreadth5,
                        WeightTolerenceRawActualWt1,WeightTolerenceRawActualWt2,WeightTolerenceRawActualWt3,WeightTolerenceRawActualWt4,WeightTolerenceRawActualWt5,WeightTolerenceRawWtMin1,
                        WeightTolerenceRawWtMin2,WeightTolerenceRawWtMin3,WeightTolerenceRawWtMin4,WeightTolerenceRawWtMin5,WeightTolerenceRawWtMax1,WeightTolerenceRawWtMax2,
                        WeightTolerenceRawWtMax3,WeightTolerenceRawWtMax4,WeightTolerenceRawWtMax5,WeightTolerenceFinishActualWt1,WeightTolerenceFinishActualWt2,WeightTolerenceFinishActualWt3,
                        WeightTolerenceFinishActualWt4,WeightTolerenceFinishActualWt5,WeightTolerenceFinishWtMin1,WeightTolerenceFinishWtMin2,WeightTolerenceFinishWtMin3,WeightTolerenceFinishWtMin4
                        ,WeightTolerenceFinishWtMin5,WeightTolerenceFinishWtMax1,WeightTolerenceFinishWtMax2,WeightTolerenceFinishWtMax3
                        ,WeightTolerenceFinishWtMax4,WeightTolerenceFinishWtMax5,StitchingSPI,StitchingNeedleNo,StitchingSewingThread,StitchingFillerWeight,
                        RawMaterial4, RawMaterial5, RawMaterial6, RawMaterial7, RawMaterial8, RawMaterial9, RawMaterial10)
                 values(" + hndocid.Value + ",'" + txtdate.Text.Replace("'", "''") + "','" + txtproductdeveloper.Text.Replace("'", "''") + "','" + txtmerchandiser.Text.Replace("'", "''") + "','" + txtbuyerName.Text.Replace("'", "''") + "','" + txttypeofproduct.Text.Replace("'", "''") + "','" + txtstyle_designname.Text.Replace("'", "''") + "','" + txtcolors.Text.Replace("'", "''") + "','" + txtrawweight.Text.Replace("'", "''") + "','" + txtfinishweight.Text.Replace("'", "''") + "','" + txtwool.Text.Replace("'", "''") + "','" + txtcotton.Text.Replace("'", "''") + "','" + txtviscose.Text.Replace("'", "''") + "','" + txttuftingcloth.Text.Replace("'", "''") + @"',
                          '" + txtthirdbackingcloth.Text.Replace("'", "''") + "','" + txtniwarwidth.Text.Replace("'", "''") + "','" + txttypeofdyeing.Text.Replace("'", "''") + "','" + txtwarp.Text.Replace("'", "''") + "','" + txtweft.Text.Replace("'", "''") + "','" + txtreeds.Text.Replace("'", "''") + "','" + txtpicks.Text.Replace("'", "''") + "','" + txttuftdensity.Text.Replace("'", "''") + @"',
                          '" + txtweavingpattern.Text.Replace("'", "''") + "','" + txtrawpileheightLoop.Text.Replace("'", "''") + "','" + txtfinishpileheightLoop.Text.Replace("'", "''") + "','" + txtwashing.Text.Replace("'", "''") + "','" + txtlatexrecipe.Text.Replace("'", "''") + "','" + txttypeofyarn.Text.Replace("'", "''") + @"',
                          '" + txtplyofyarn.Text.Replace("'", "''") + "','" + txtstitchperinch.Text.Replace("'", "''") + "','" + txtlengthtolerence.Text.Replace("'", "''") + "','" + txtwidthtolerence.Text.Replace("'", "''") + "','" + txtweighttolerence.Text.Replace("'", "''") + "','" + txtprocessflow.Text.Replace("'", "''") + @"'
        ,'" + txtRawPileHeightCut.Text.Replace("'", "''") + "','" + txtFinishPileHeightCut.Text.Replace("'", "''") + "','" + txtSizeTolerenceActualSize1.Text.Replace("'", "''") + "','" + txtSizeTolerenceActualSize2.Text.Replace("'", "''") + "','" + txtSizeTolerenceActualSize3.Text.Replace("'", "''") + @"'
        ,'" + txtSizeTolerenceActualSize4.Text.Replace("'", "''") + "','" + txtSizeTolerenceActualSize5.Text.Replace("'", "''") + "','" + txtSizeTolerenceLength1.Text.Replace("'", "''") + "','" + txtSizeTolerenceLength2.Text.Replace("'", "''") + "','" + txtSizeTolerenceLength3.Text.Replace("'", "''") + @"'
        ,'" + txtSizeTolerenceLength4.Text.Replace("'", "''") + "','" + txtSizeTolerenceLength5.Text.Replace("'", "''") + "','" + txtSizeTolerenceBreadth1.Text.Replace("'", "''") + "','" + txtSizeTolerenceBreadth2.Text.Replace("'", "''") + "','" + txtSizeTolerenceBreadth3.Text.Replace("'", "''") + @"'
        ,'" + txtSizeTolerenceBreadth4.Text.Replace("'", "''") + "','" + txtSizeTolerenceBreadth5.Text.Replace("'", "''") + "','" + txtWeightTolerenceActualWt1.Text.Replace("'", "''") + "','" + txtWeightTolerenceActualWt2.Text.Replace("'", "''") + "','" + txtWeightTolerenceActualWt3.Text.Replace("'", "''") + @"'
        ,'" + txtWeightTolerenceActualWt4.Text.Replace("'", "''") + "','" + txtWeightTolerenceActualWt5.Text.Replace("'", "''") + "','" + txtWeightTolerenceRawWtMin1.Text.Replace("'", "''") + "','" + txtWeightTolerenceRawWtMin2.Text.Replace("'", "''") + "','" + txtWeightTolerenceRawWtMin3.Text.Replace("'", "''") + @"'
        ,'" + txtWeightTolerenceRawWtMin4.Text.Replace("'", "''") + "','" + txtWeightTolerenceRawWtMin5.Text.Replace("'", "''") + "','" + txtWeightTolerenceRawWtMax1.Text.Replace("'", "''") + "','" + txtWeightTolerenceRawWtMax2.Text.Replace("'", "''") + "','" + txtWeightTolerenceRawWtMax3.Text.Replace("'", "''") + @"'
        ,'" + txtWeightTolerenceRawWtMax4.Text.Replace("'", "''") + "','" + txtWeightTolerenceRawWtMax5.Text.Replace("'", "''") + "','" + txtWeightTolerenceFinishActualWt1.Text.Replace("'", "''") + "','" + txtWeightTolerenceFinishActualWt2.Text.Replace("'", "''") + "','" + txtWeightTolerenceFinishActualWt3.Text.Replace("'", "''") + @"'
        ,'" + txtWeightTolerenceFinishActualWt4.Text.Replace("'", "''") + "','" + txtWeightTolerenceFinishActualWt5.Text.Replace("'", "''") + "','" + txtWeightTolerenceFinishWtMin1.Text.Replace("'", "''") + "','" + txtWeightTolerenceFinishWtMin2.Text.Replace("'", "''") + "','" + txtWeightTolerenceFinishWtMin3.Text.Replace("'", "''") + @"'
        ,'" + txtWeightTolerenceFinishWtMin4.Text.Replace("'", "''") + "','" + txtWeightTolerenceFinishWtMin5.Text.Replace("'", "''") + "','" + txtWeightTolerenceFinishWtMax1.Text.Replace("'", "''") + "','" + txtWeightTolerenceFinishWtMax2.Text.Replace("'", "''") + "','" + txtWeightTolerenceFinishWtMax3.Text.Replace("'", "''") + @"'
        ,'" + txtWeightTolerenceFinishWtMax4.Text.Replace("'", "''") + "','" + txtWeightTolerenceFinishWtMax5.Text.Replace("'", "''") + "','" + txtStitchingSPI.Text.Replace("'", "''") + "','" + txtStitchingNeedleNo.Text.Replace("'", "''") + "','" + txtStitchingSewingThread.Text.Replace("'", "''") + "','" + txtStitchingFillerWeight.Text.Replace("'", "''") + @"', 
        '" + TxtRawMaterial4.Text.Replace("'", "''") + "','" + TxtRawMaterial5.Text.Replace("'", "''") + "','" + TxtRawMaterial6.Text.Replace("'", "''") + "','" + TxtRawMaterial7.Text.Replace("'", "''") + "','" + TxtRawMaterial8.Text.Replace("'", "''") + "','" + TxtRawMaterial9.Text.Replace("'", "''") + "','" + TxtRawMaterial10.Text.Replace("'", "''") + "')";

        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
    }
    protected void SaveImage(int Id)
    {
        if (PhotoImage.FileName != "")
        {
            string filename = Path.GetFileName(PhotoImage.PostedFile.FileName);
            string Folderpath = Server.MapPath("../../PSSIMAGE");
            //Check folder
            if (!Directory.Exists(Folderpath))
            {
                Directory.CreateDirectory(Folderpath);
            }
            //
            string targetPath = Server.MapPath("../../PSSIMAGE/" + Id + "_PSS.gif");

            FileInfo file = new FileInfo(targetPath);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
            }  

            string img = "~\\PSSIMAGE\\" + Id + "_PSS.gif";
            //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
            Stream strm = PhotoImage.PostedFile.InputStream;
            var targetFile = targetPath;
            if (PhotoImage.FileName != null && PhotoImage.FileName != "")
            {
                GenerateThumbnails(0.5, strm, targetFile);
            }
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update PRODUCTIONSPECIFICATIONMASTER Set imgpath='" + img + "' Where Docid=" + Id + "");
            lblimage.ImageUrl = img + "?" + DateTime.Now.Ticks.ToString(); ;
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
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            string str = @"SELECT Ci.companyName,NU.UserName,isnull(NU2.UserName,'') as ApprovalUserName, * 
                    FROM PRODUCTIONSPECIFICATIONMASTER PSM 
                    INNER JOIN PRODUCTIONSPECIFICATIONDETAIL PSD ON PSM.DOCID=PSD.DOCID 
                    inner join CompanyInfo ci on Psm.COMPANYID=ci.CompanyId 
                    JOIN NewUserDetail NU ON PSM.UserId=NU.UserId 
                    LEFT JOIN NewUserDetail NU2 ON PSM.Approval_UserID=NU2.UserId
                    Where PSM.Docid=" + hndocid.Value;

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //Add Image
                ds.Tables[0].Columns.Add("image", typeof(System.Byte[]));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Convert.ToString(dr["imgpath"]) != "")
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

                Session["rptFileName"] = "~\\Reports\\rptproductionspecificsheet1.rpt";
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptproductionspecificsheet.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt1", "alert('No records found.')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void Changeapprovebuttoncolor(int approvestatus = 0)
    {
        switch (approvestatus)
        {
            case 1:
                btnApprove.BackColor = System.Drawing.Color.Green;
                break;
            default:
                btnApprove.BackColor = System.Drawing.Color.Red;
                break;
        }
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        TdDesignsearch.Visible = false;
        TDbuyersearch.Visible = false;
        TDDocno.Visible = false;
        hndocid.Value = "0";
        DDDocNo.Items.Clear();
        refreshcontrol();
        btnsave.Visible = true;
        btndelete.Visible = true;
       
        if (chkedit.Checked == true)
        {
            TDDocno.Visible = true;
            TdDesignsearch.Visible = true;
            TDbuyersearch.Visible = true;
            fillDocno();
            if (Session["usertype"].ToString() != "1")
            {
                btnsave.Visible = false;
                btndelete.Visible = false;
            }
           
        }
    }    
    protected void fillDocno()
    {
        string str = @"select PM.DOCID,PM.DOCNO From PRODUCTIONSPECIFICATIONMASTER PM inner join PRODUCTIONSPECIFICATIONDETAIL PD on PM.Docid=PD.Docid
                      Where PM.COMPANYID=" + DDcompanyName.SelectedValue;
        if (txtdesignsearch.Text != "")
        {
            str = str + " and PD.designname='" + txtdesignsearch.Text.Trim() + "'";
        }
        if (txtbuyersearch.Text != "")
        {
            str = str + " and PD.Buyername like'" + txtbuyersearch.Text.Trim() + "%'";
        }
        str = str + " order by DOCID desc";
        UtilityModule.ConditionalComboFill(ref DDDocNo, str, true, "--Plz Select--");
    }
    protected void DDDocNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hndocid.Value = DDDocNo.SelectedValue;
        refreshcontrol();
        FillDataback();


    }
    protected void refreshcontrol()
    {
        txtdate.Text = "";
        txtproductdeveloper.Text = "";
        txtmerchandiser.Text = "";
        txtbuyerName.Text = "";
        txttypeofproduct.Text = "";
        txtstyle_designname.Text = "";
        txtcolors.Text = "";
        txtrawweight.Text = "";
        txtfinishweight.Text = "";
        txtwool.Text = "";
        txtcotton.Text = "";
        txtviscose.Text = "";

        txttuftingcloth.Text = "";
        txtthirdbackingcloth.Text = "";
        txtniwarwidth.Text = "";
        txttypeofdyeing.Text = "";
        txtwarp.Text = "";
        txtweft.Text = "";
        txtreeds.Text = "";
        txtpicks.Text = "";
        txttuftdensity.Text = "";
        txtweavingpattern.Text = "";
        txtrawpileheightLoop.Text = "";
        txtfinishpileheightLoop.Text = "";        
        txtwashing.Text = "";
        txtlatexrecipe.Text = "";
        txttypeofyarn.Text = "";
        txtplyofyarn.Text = "";
        txtstitchperinch.Text = "";
        txtlengthtolerence.Text = "";
        txtwidthtolerence.Text = "";
        txtweighttolerence.Text = "";
        txtprocessflow.Text = "";

        txtRawPileHeightCut.Text = "";
        txtFinishPileHeightCut.Text = "";
        txtSizeTolerenceActualSize1.Text = "";
        txtSizeTolerenceActualSize2.Text = "";
        txtSizeTolerenceActualSize3.Text = "";
        txtSizeTolerenceActualSize4.Text = "";
        txtSizeTolerenceActualSize5.Text = "";
        txtSizeTolerenceLength1.Text = "";
        txtSizeTolerenceLength2.Text = "";
        txtSizeTolerenceLength3.Text = "";
        txtSizeTolerenceLength4.Text = "";
        txtSizeTolerenceLength5.Text = "";
        txtSizeTolerenceBreadth1.Text = "";
        txtSizeTolerenceBreadth2.Text = "";
        txtSizeTolerenceBreadth3.Text = "";
        txtSizeTolerenceBreadth4.Text = "";
        txtSizeTolerenceBreadth5.Text = "";
        txtWeightTolerenceActualWt1.Text = "";
        txtWeightTolerenceActualWt2.Text = "";
        txtWeightTolerenceActualWt3.Text = "";
        txtWeightTolerenceActualWt4.Text = "";
        txtWeightTolerenceActualWt5.Text = "";
        txtWeightTolerenceRawWtMin1.Text = "";
        txtWeightTolerenceRawWtMin2.Text = "";
        txtWeightTolerenceRawWtMin3.Text = "";
        txtWeightTolerenceRawWtMin4.Text = "";
        txtWeightTolerenceRawWtMin5.Text = "";
        txtWeightTolerenceRawWtMax1.Text = "";
        txtWeightTolerenceRawWtMax2.Text = "";
        txtWeightTolerenceRawWtMax3.Text = "";
        txtWeightTolerenceRawWtMax4.Text = "";
        txtWeightTolerenceRawWtMax5.Text = "";
        txtWeightTolerenceFinishActualWt1.Text = "";
        txtWeightTolerenceFinishActualWt2.Text = "";
        txtWeightTolerenceFinishActualWt3.Text = "";
        txtWeightTolerenceFinishActualWt4.Text = "";
        txtWeightTolerenceFinishActualWt5.Text = "";
        txtWeightTolerenceFinishWtMin1.Text = "";
        txtWeightTolerenceFinishWtMin2.Text = "";
        txtWeightTolerenceFinishWtMin3.Text = "";
        txtWeightTolerenceFinishWtMin4.Text = "";
        txtWeightTolerenceFinishWtMin5.Text = "";
        txtWeightTolerenceFinishWtMax1.Text = "";
        txtWeightTolerenceFinishWtMax2.Text = "";
        txtWeightTolerenceFinishWtMax3.Text = "";
        txtWeightTolerenceFinishWtMax4.Text = "";
        txtWeightTolerenceFinishWtMax5.Text = "";
        txtStitchingSPI.Text = "";
        txtStitchingNeedleNo.Text = "";
        txtStitchingSewingThread.Text = "";
        txtStitchingFillerWeight.Text = "";
        TxtRawMaterial4.Text = "";
        TxtRawMaterial5.Text = "";
        TxtRawMaterial6.Text = "";
        TxtRawMaterial7.Text = "";
        TxtRawMaterial8.Text = "";
        TxtRawMaterial9.Text = "";
        TxtRawMaterial10.Text = "";
    }
    protected void FillDataback()
    {
        string str = @"SELECT PSM.DOCNO,PSM.IMGPATH,PSD.DATE,PSD.PRODUCTDEVELOPER,PSD.MERCHANDISER,PSD.BUYERNAME,PSD.TYPEOFPRODUCT,PSD.DESIGNNAME,PSD.COLORS,PSD.WEIGHTPCS_RAW,PSD.WEIGHTPCS_FINISH,PSD.WOOL,PSD.COTTON,PSD.VISCOSE,PSD.TUFTINGCLOTH,
                    THIRDBACKINGCLOTH,PSD.NIWARWIDTH,PSD.TYPEOFDYEING,PSD.WARP,PSD.WEFT,PSD.REED,PSD.PICKS,PSD.TUFTDENSITY,PSD.WEAVINGPATTERN,PSD.PILEHEIGHT_RAW,PSD.PILEHEIGHT_FINISH,PSD.WASHING,PSD.LATEXRECIPE,PSD.
                    TYPEOFYARN,PSD.PLYOFYARN,PSD.STITCHPERINCH,PSD.TOLERENCE_LENGTH,PSD.TOLERENCE_WIDTH,PSD.TOLERENCE_WEIGHT,PSD.PROCESSFLOW,
                    PSD.PileHeight_RawCut,PSD.PileHeight_FinishCut,PSD.SizeTolerenceActualSize1,PSD.SizeTolerenceActualSize2,PSD.SizeTolerenceActualSize3,PSD.SizeTolerenceActualSize4,PSD.SizeTolerenceActualSize5,
                    PSD.SizeTolerenceLength1,PSD.SizeTolerenceLength2,PSD.SizeTolerenceLength3,PSD.SizeTolerenceLength4,PSD.SizeTolerenceLength5,PSD.SizeTolerenceBreadth1,PSD.SizeTolerenceBreadth2,
                    PSD.SizeTolerenceBreadth3,PSD.SizeTolerenceBreadth4,PSD.SizeTolerenceBreadth5,PSD.WeightTolerenceRawActualWt1,PSD.WeightTolerenceRawActualWt2,PSD.WeightTolerenceRawActualWt3,
                    PSD.WeightTolerenceRawActualWt4,PSD.WeightTolerenceRawActualWt5,PSD.WeightTolerenceRawWtMin1,PSD.WeightTolerenceRawWtMin2,PSD.WeightTolerenceRawWtMin3,PSD.WeightTolerenceRawWtMin4,
                    PSD.WeightTolerenceRawWtMin5,PSD.WeightTolerenceRawWtMax1,PSD.WeightTolerenceRawWtMax2,PSD.WeightTolerenceRawWtMax3,PSD.WeightTolerenceRawWtMax4,PSD.WeightTolerenceRawWtMax5,
                    PSD.WeightTolerenceFinishActualWt1,PSD.WeightTolerenceFinishActualWt2,PSD.WeightTolerenceFinishActualWt3,PSD.WeightTolerenceFinishActualWt4,PSD.WeightTolerenceFinishActualWt5,
                    PSD.WeightTolerenceFinishWtMin1,PSD.WeightTolerenceFinishWtMin2,PSD.WeightTolerenceFinishWtMin3,PSD.WeightTolerenceFinishWtMin4,PSD.WeightTolerenceFinishWtMin5,
                    PSD.WeightTolerenceFinishWtMax1,PSD.WeightTolerenceFinishWtMax2,PSD.WeightTolerenceFinishWtMax3,PSD.WeightTolerenceFinishWtMax4,PSD.WeightTolerenceFinishWtMax5,
                    PSD.StitchingSPI,PSD.StitchingNeedleNo,PSD.StitchingSewingThread,PSD.StitchingFillerWeight,isnull(NUD.UserName,'') as UserName,isnull(PSM.ApproveStatus,0) as ApproveStatus,
                    isnull(NUD2.UserName,'') as ApprovalUserName, PSD.RawMaterial4, PSD.RawMaterial5, PSD.RawMaterial6, PSD.RawMaterial7, PSD.RawMaterial8, PSD.RawMaterial9, PSD.RawMaterial10 
                    FROM PRODUCTIONSPECIFICATIONMASTER PSM 
                    INNER JOIN PRODUCTIONSPECIFICATIONDETAIL PSD ON PSM.DOCID=PSD.DOCID
                    JOIN NewUserDetail NUD ON PSM.UserId=NUD.UserId
                    LEFT JOIN NewUserDetail NUD2 ON PSM.Approval_UserID=NUD2.UserId
                    Where PSM.Docid=" + hndocid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtdocno.Text = ds.Tables[0].Rows[0]["DocNo"].ToString();
            lblimage.ImageUrl = ds.Tables[0].Rows[0]["IMGPATH"].ToString();
            txtdate.Text = ds.Tables[0].Rows[0]["Date"].ToString();
            txtproductdeveloper.Text = ds.Tables[0].Rows[0]["Productdeveloper"].ToString();
            txtmerchandiser.Text = ds.Tables[0].Rows[0]["merchandiser"].ToString();
            txtbuyerName.Text = ds.Tables[0].Rows[0]["buyername"].ToString();
            txttypeofproduct.Text = ds.Tables[0].Rows[0]["typeofproduct"].ToString();
            txtstyle_designname.Text = ds.Tables[0].Rows[0]["designname"].ToString();
            txtcolors.Text = ds.Tables[0].Rows[0]["colors"].ToString();
            txtrawweight.Text = ds.Tables[0].Rows[0]["Weightpcs_raw"].ToString();
            txtfinishweight.Text = ds.Tables[0].Rows[0]["weightpcs_finish"].ToString();
            txtwool.Text = ds.Tables[0].Rows[0]["wool"].ToString();
            txtcotton.Text = ds.Tables[0].Rows[0]["cotton"].ToString();
            txtviscose.Text = ds.Tables[0].Rows[0]["viscose"].ToString();

            txttuftingcloth.Text = ds.Tables[0].Rows[0]["tuftingcloth"].ToString();
            txtthirdbackingcloth.Text = ds.Tables[0].Rows[0]["thirdbackingcloth"].ToString();
            txtniwarwidth.Text = ds.Tables[0].Rows[0]["niwarwidth"].ToString();
            txttypeofdyeing.Text = ds.Tables[0].Rows[0]["typeofdyeing"].ToString();
            txtwarp.Text = ds.Tables[0].Rows[0]["warp"].ToString();
            txtweft.Text = ds.Tables[0].Rows[0]["weft"].ToString();
            txtreeds.Text = ds.Tables[0].Rows[0]["reed"].ToString();
            txtpicks.Text = ds.Tables[0].Rows[0]["picks"].ToString();
            txttuftdensity.Text = ds.Tables[0].Rows[0]["tuftdensity"].ToString();
            txtweavingpattern.Text = ds.Tables[0].Rows[0]["weavingpattern"].ToString();
            txtrawpileheightLoop.Text = ds.Tables[0].Rows[0]["pileheight_raw"].ToString();
            txtfinishpileheightLoop.Text = ds.Tables[0].Rows[0]["pileheight_finish"].ToString();            
            txtwashing.Text = ds.Tables[0].Rows[0]["washing"].ToString();
            txtlatexrecipe.Text = ds.Tables[0].Rows[0]["latexrecipe"].ToString();
            txttypeofyarn.Text = ds.Tables[0].Rows[0]["typeofyarn"].ToString();
            txtplyofyarn.Text = ds.Tables[0].Rows[0]["plyofyarn"].ToString();
            txtstitchperinch.Text = ds.Tables[0].Rows[0]["stitchperinch"].ToString();
            txtlengthtolerence.Text = ds.Tables[0].Rows[0]["Tolerence_length"].ToString();
            txtwidthtolerence.Text = ds.Tables[0].Rows[0]["Tolerence_width"].ToString();
            txtweighttolerence.Text = ds.Tables[0].Rows[0]["Tolerence_weight"].ToString();
            txtprocessflow.Text = ds.Tables[0].Rows[0]["Processflow"].ToString();

            txtRawPileHeightCut.Text = ds.Tables[0].Rows[0]["PileHeight_RawCut"].ToString();
            txtFinishPileHeightCut.Text = ds.Tables[0].Rows[0]["PileHeight_FinishCut"].ToString();
            txtSizeTolerenceActualSize1.Text = ds.Tables[0].Rows[0]["SizeTolerenceActualSize1"].ToString();
            txtSizeTolerenceActualSize2.Text = ds.Tables[0].Rows[0]["SizeTolerenceActualSize2"].ToString();
            txtSizeTolerenceActualSize3.Text = ds.Tables[0].Rows[0]["SizeTolerenceActualSize3"].ToString();
            txtSizeTolerenceActualSize4.Text = ds.Tables[0].Rows[0]["SizeTolerenceActualSize4"].ToString();
            txtSizeTolerenceActualSize5.Text = ds.Tables[0].Rows[0]["SizeTolerenceActualSize5"].ToString();
            txtSizeTolerenceLength1.Text = ds.Tables[0].Rows[0]["SizeTolerenceLength1"].ToString();
            txtSizeTolerenceLength2.Text = ds.Tables[0].Rows[0]["SizeTolerenceLength2"].ToString();
            txtSizeTolerenceLength3.Text = ds.Tables[0].Rows[0]["SizeTolerenceLength3"].ToString();
            txtSizeTolerenceLength4.Text = ds.Tables[0].Rows[0]["SizeTolerenceLength4"].ToString();
            txtSizeTolerenceLength5.Text = ds.Tables[0].Rows[0]["SizeTolerenceLength5"].ToString();
            txtSizeTolerenceBreadth1.Text = ds.Tables[0].Rows[0]["SizeTolerenceBreadth1"].ToString();
            txtSizeTolerenceBreadth2.Text = ds.Tables[0].Rows[0]["SizeTolerenceBreadth2"].ToString();
            txtSizeTolerenceBreadth3.Text = ds.Tables[0].Rows[0]["SizeTolerenceBreadth3"].ToString();
            txtSizeTolerenceBreadth4.Text = ds.Tables[0].Rows[0]["SizeTolerenceBreadth4"].ToString();
            txtSizeTolerenceBreadth5.Text = ds.Tables[0].Rows[0]["SizeTolerenceBreadth5"].ToString();
            txtWeightTolerenceActualWt1.Text = ds.Tables[0].Rows[0]["WeightTolerenceRawActualWt1"].ToString();
            txtWeightTolerenceActualWt2.Text = ds.Tables[0].Rows[0]["WeightTolerenceRawActualWt2"].ToString();
            txtWeightTolerenceActualWt3.Text = ds.Tables[0].Rows[0]["WeightTolerenceRawActualWt3"].ToString();
            txtWeightTolerenceActualWt4.Text = ds.Tables[0].Rows[0]["WeightTolerenceRawActualWt4"].ToString();
            txtWeightTolerenceActualWt5.Text = ds.Tables[0].Rows[0]["WeightTolerenceRawActualWt5"].ToString();
            txtWeightTolerenceRawWtMin1.Text = ds.Tables[0].Rows[0]["WeightTolerenceRawWtMin1"].ToString();
            txtWeightTolerenceRawWtMin2.Text = ds.Tables[0].Rows[0]["WeightTolerenceRawWtMin2"].ToString();
            txtWeightTolerenceRawWtMin3.Text = ds.Tables[0].Rows[0]["WeightTolerenceRawWtMin3"].ToString();
            txtWeightTolerenceRawWtMin4.Text = ds.Tables[0].Rows[0]["WeightTolerenceRawWtMin4"].ToString();
            txtWeightTolerenceRawWtMin5.Text = ds.Tables[0].Rows[0]["WeightTolerenceRawWtMin5"].ToString();
            txtWeightTolerenceRawWtMax1.Text = ds.Tables[0].Rows[0]["WeightTolerenceRawWtMax1"].ToString();
            txtWeightTolerenceRawWtMax2.Text = ds.Tables[0].Rows[0]["WeightTolerenceRawWtMax2"].ToString();
            txtWeightTolerenceRawWtMax3.Text = ds.Tables[0].Rows[0]["WeightTolerenceRawWtMax3"].ToString();
            txtWeightTolerenceRawWtMax4.Text = ds.Tables[0].Rows[0]["WeightTolerenceRawWtMax4"].ToString();
            txtWeightTolerenceRawWtMax5.Text = ds.Tables[0].Rows[0]["WeightTolerenceRawWtMax5"].ToString();
            txtWeightTolerenceFinishActualWt1.Text = ds.Tables[0].Rows[0]["WeightTolerenceFinishActualWt1"].ToString();
            txtWeightTolerenceFinishActualWt2.Text = ds.Tables[0].Rows[0]["WeightTolerenceFinishActualWt2"].ToString();
            txtWeightTolerenceFinishActualWt3.Text = ds.Tables[0].Rows[0]["WeightTolerenceFinishActualWt3"].ToString();
            txtWeightTolerenceFinishActualWt4.Text = ds.Tables[0].Rows[0]["WeightTolerenceFinishActualWt4"].ToString();
            txtWeightTolerenceFinishActualWt5.Text = ds.Tables[0].Rows[0]["WeightTolerenceFinishActualWt5"].ToString();
            txtWeightTolerenceFinishWtMin1.Text = ds.Tables[0].Rows[0]["WeightTolerenceFinishWtMin1"].ToString();
            txtWeightTolerenceFinishWtMin2.Text = ds.Tables[0].Rows[0]["WeightTolerenceFinishWtMin2"].ToString();
            txtWeightTolerenceFinishWtMin3.Text = ds.Tables[0].Rows[0]["WeightTolerenceFinishWtMin3"].ToString();
            txtWeightTolerenceFinishWtMin4.Text = ds.Tables[0].Rows[0]["WeightTolerenceFinishWtMin4"].ToString();
            txtWeightTolerenceFinishWtMin5.Text = ds.Tables[0].Rows[0]["WeightTolerenceFinishWtMin5"].ToString();
            txtWeightTolerenceFinishWtMax1.Text = ds.Tables[0].Rows[0]["WeightTolerenceFinishWtMax1"].ToString();
            txtWeightTolerenceFinishWtMax2.Text = ds.Tables[0].Rows[0]["WeightTolerenceFinishWtMax2"].ToString();
            txtWeightTolerenceFinishWtMax3.Text = ds.Tables[0].Rows[0]["WeightTolerenceFinishWtMax3"].ToString();
            txtWeightTolerenceFinishWtMax4.Text = ds.Tables[0].Rows[0]["WeightTolerenceFinishWtMax4"].ToString();
            txtWeightTolerenceFinishWtMax5.Text = ds.Tables[0].Rows[0]["WeightTolerenceFinishWtMax5"].ToString();
            txtStitchingSPI.Text = ds.Tables[0].Rows[0]["StitchingSPI"].ToString();
            txtStitchingNeedleNo.Text = ds.Tables[0].Rows[0]["StitchingNeedleNo"].ToString();
            txtStitchingSewingThread.Text = ds.Tables[0].Rows[0]["StitchingSewingThread"].ToString();
            txtStitchingFillerWeight.Text = ds.Tables[0].Rows[0]["StitchingFillerWeight"].ToString();

            TxtRawMaterial4.Text = ds.Tables[0].Rows[0]["RawMaterial4"].ToString();
            TxtRawMaterial5.Text = ds.Tables[0].Rows[0]["RawMaterial5"].ToString();
            TxtRawMaterial6.Text = ds.Tables[0].Rows[0]["RawMaterial6"].ToString();
            TxtRawMaterial7.Text = ds.Tables[0].Rows[0]["RawMaterial7"].ToString();
            TxtRawMaterial8.Text = ds.Tables[0].Rows[0]["RawMaterial8"].ToString();
            TxtRawMaterial9.Text = ds.Tables[0].Rows[0]["RawMaterial9"].ToString();
            TxtRawMaterial10.Text = ds.Tables[0].Rows[0]["RawMaterial10"].ToString();

            lblEnteredBy.Text = "Entered By:";
            lblEnteredByText.Text = ds.Tables[0].Rows[0]["UserName"].ToString();

            lblApprovedBy.Text = "Approved By:";
            lblApprovedByText.Text = ds.Tables[0].Rows[0]["ApprovalUserName"].ToString();

            if (btnApprove.Visible == true)
            {
                Changeapprovebuttoncolor(Convert.ToInt16(ds.Tables[0].Rows[0]["approvestatus"]));
            }           
        }
    }
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        fillDocno();
    }
    protected void btndelete_Click(object sender, EventArgs e)
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
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@Docid", hndocid.Value);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@Imgpath", SqlDbType.VarChar, 200);
            param[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEPRODUCTIONSPECIFICATIONSHEET", param);
            Tran.Commit();
            if (param[3].Value.ToString() != "")
            {
                lblmsg.Text = param[3].Value.ToString();
            }
            else
            {
                lblmsg.Text = "DOC No. Deleted Successfully.";
                fillDocno();
                Deleteimage(param[4].Value.ToString());
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
    protected void Deleteimage(string filename = "")
    {
        if (filename != "")
        {
            if (File.Exists(Server.MapPath(filename)))
            {
                File.Delete(Server.MapPath(filename));
            }
        }
    }
    protected void btnsearchbuyer_Click(object sender, EventArgs e)
    {
        fillDocno();
    }
    protected void DDcompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkedit.Checked == true)
        {
            fillDocno();
        }
    }
    protected void btnApprove_Click(object sender, EventArgs e)
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
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@Docid", hndocid.Value);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_APPROVEPRODUCTSPECIFICATIONSHEET", param);
            Tran.Commit();
            lblmsg.Text = param[3].Value.ToString();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altappv", "alert('" + param[3].Value.ToString() + "')", true);
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
}

