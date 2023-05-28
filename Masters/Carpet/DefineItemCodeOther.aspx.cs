using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Services;
using IExpro.Web.Models;
using System.Web.Services;
using System.Text.Json;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using System.Runtime.CompilerServices;
using DocumentFormat.OpenXml.Office.Word;

public partial class Masters_Carpet_DefineItemCodeOther : System.Web.UI.Page
{

    public static int ItemFinishedId { get; set; }
    public static int CompanyId { get; set; }
    public static int UserId { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            Masters_Carpet_DefineItemCodeOther.ItemFinishedId = Request.QueryString["SrNo"] != null ? Convert.ToInt32(Request.QueryString["SrNo"]) : 0;
            Masters_Carpet_DefineItemCodeOther.CompanyId = Session["varCompanyId"] != null ? Convert.ToInt32(Session["varCompanyId"]) : 0;
            Masters_Carpet_DefineItemCodeOther.UserId = Session["varuserid"] != null ? Convert.ToInt32(Session["varuserid"]) : 0;

            this.BindPhotoList();

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            try
            {
                string str = @"Select HS_CODE,MIN_ODRDERQTY,PRICE,INNER_PACKING,MASTER_PACKING,MASTER_PKG_SIZE,MASTER_PKG_SIZE_1,MASTER_PKG_SIZE_2,DESCRIPTION,VOLUME,DRAWBACKRATE,
            LOAD_20,LOAD_40,LOAD_40HQ,MATERIALREMARKS,FINISHREMARKS,PACKINGREMARKS,ISNULL(Iron,0) IRONWT,ISNULL(Glass,0) WOODWT,ISNULL(NetWeight,0) NETWT FROM ITEM_PARAMETER_OTHER,MAIN_ITEM_IMAGE
            WHERE finishedid=Item_Finished_ID AND Item_Finished_ID =" + Request.QueryString["SrNo"] + " And MAIN_ITEM_IMAGE.MasterCompanyid=" + Session["varCompanyId"];
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    TxtHsCode.Text = ds.Tables[0].Rows[0]["HS_CODE"].ToString();
                    TxtMinOrderQty.Text = ds.Tables[0].Rows[0]["MIN_ODRDERQTY"].ToString();
                    TxtPrice.Text = ds.Tables[0].Rows[0]["PRICE"].ToString();
                    TxtInnerPkg.Text = ds.Tables[0].Rows[0]["INNER_PACKING"].ToString();
                    TxtMasterPkg.Text = ds.Tables[0].Rows[0]["MASTER_PACKING"].ToString();
                    TxtMasterPkgSize.Text = ds.Tables[0].Rows[0]["MASTER_PKG_SIZE"].ToString();
                    TxtMasterPkgSize1.Text = ds.Tables[0].Rows[0]["MASTER_PKG_SIZE_1"].ToString();
                    TxtMasterPkgSize2.Text = ds.Tables[0].Rows[0]["MASTER_PKG_SIZE_2"].ToString();
                    TxtDesc.Text = ds.Tables[0].Rows[0]["DESCRIPTION"].ToString();
                    TxtVolume.Text = ds.Tables[0].Rows[0]["VOLUME"].ToString();
                    TxtDrawBackRate.Text = ds.Tables[0].Rows[0]["DRAWBACKRATE"].ToString();
                    TxtLoad20.Text = ds.Tables[0].Rows[0]["LOAD_20"].ToString();
                    TxtLoad40.Text = ds.Tables[0].Rows[0]["LOAD_40"].ToString();
                    Txtload40HQ.Text = ds.Tables[0].Rows[0]["LOAD_40HQ"].ToString();
                    txtMaterialremarks.Text = ds.Tables[0].Rows[0]["MATERIALREMARKS"].ToString();
                    TxtFinishRemarks.Text = ds.Tables[0].Rows[0]["FINISHREMARKS"].ToString();
                    TxtPkgRemarks.Text = ds.Tables[0].Rows[0]["PACKINGREMARKS"].ToString();
                    TxtIronWt.Text = ds.Tables[0].Rows[0]["IRONWT"].ToString();
                    TxtWoodWt.Text = ds.Tables[0].Rows[0]["WOODWT"].ToString();
                    TxtNetWt.Text = ds.Tables[0].Rows[0]["NETWT"].ToString();

                }
            }

            catch (Exception ex)
            {
                LblMsg.Text = ex.Message.ToString();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void BTNSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {


            SqlParameter[] _param = new SqlParameter[22];
            _param[0] = new SqlParameter("@ItemFinishedID", Request.QueryString["SrNo"]);
            _param[1] = new SqlParameter("@HSCODE", TxtHsCode.Text);
            _param[2] = new SqlParameter("@Description", TxtDesc.Text);
            _param[3] = new SqlParameter("@MinOrderQty", TxtMinOrderQty.Text);
            _param[4] = new SqlParameter("@Price", TxtPrice.Text);
            _param[5] = new SqlParameter("@InnerPacking", TxtInnerPkg.Text);
            _param[6] = new SqlParameter("@MasterPacking", TxtMasterPkg.Text);
            _param[7] = new SqlParameter("@MasterPackingSize", TxtMasterPkgSize.Text);
            _param[8] = new SqlParameter("@MasterPackingSize1", TxtMasterPkgSize1.Text);
            _param[9] = new SqlParameter("@MasterPackingSize2", TxtMasterPkgSize2.Text);
            _param[10] = new SqlParameter("@Volume", TxtVolume.Text);
            _param[11] = new SqlParameter("@DrawBackRate", TxtDrawBackRate.Text);
            _param[12] = new SqlParameter("@Load20", TxtLoad20.Text);
            _param[13] = new SqlParameter("@Load40", TxtLoad40.Text);
            _param[14] = new SqlParameter("@Load40HQ", Txtload40HQ.Text);
            _param[15] = new SqlParameter("@MaterialRemarks", txtMaterialremarks.Text);
            _param[16] = new SqlParameter("@FinishRemarks", TxtFinishRemarks.Text);
            _param[17] = new SqlParameter("@PackingRemarks", TxtPkgRemarks.Text);
            _param[18] = new SqlParameter("@IronWt", TxtIronWt.Text);
            _param[19] = new SqlParameter("@WoodWt", TxtWoodWt.Text);
            _param[20] = new SqlParameter("@NetWt", TxtNetWt.Text);
            _param[21] = new SqlParameter("@MasterCompanyId", Session["VarCompanyId"]);

            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Save_Parameter_Other", _param);
            LblMsg.Text = "Record(s) has been saved successfully!";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Masters/Carpet/DefineItemcodeOther");
            LblMsg.Text = ex.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }



    private void BindPhotoList()
    {

        if (Masters_Carpet_DefineItemCodeOther.ItemFinishedId > 0)
        {
            string query = @"Select PhotoId,ItemFinishId,PhotoName,IsPrime 
  from tblItemPhoto Where ItemFinishId=@ItemFinishId";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@ItemFinishId", Masters_Carpet_DefineItemCodeOther.ItemFinishedId));

            using (SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {
                using (DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, query, parameters.ToArray()))
                {
                    rptPhotoList.DataSource = ds.Tables[0];
                    rptPhotoList.DataBind();
                }


            }
        }
    }
    protected void DeletePhoto(object sender, EventArgs e)
    {
        string folderPath = ConfigurationManager.AppSettings["ImagePath"];
        int photoId = int.Parse(((sender as LinkButton).NamingContainer.FindControl("lblPhotoId") as HiddenField).Value);
        string photoName = ((sender as LinkButton).NamingContainer.FindControl("hdnPhoto") as HiddenField).Value;
        if (photoId > 0)
        {
            using (SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM tblItemPhoto WHERE PhotoId = @PhotoId", con))
                {
                    cmd.Parameters.AddWithValue("@PhotoId", photoId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    if (File.Exists(Path.Combine(folderPath, photoName)))
                    {
                        File.Delete(Path.Combine(folderPath, photoName));
                    }
                }
                this.BindPhotoList();
            }

        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (Masters_Carpet_DefineItemCodeOther.ItemFinishedId > 0)
        {

            string folderPath = ConfigurationManager.AppSettings["ImagePath"];
            if (PhotoImage.HasFiles)
            {
                int index = 1;
                foreach (HttpPostedFile uploadedFile in PhotoImage.PostedFiles)
                {
                    //Check whether Directory (Folder) exists.
                    if (!Directory.Exists(folderPath))
                    {
                        //If Directory (Folder) does not exists. Create it.
                        Directory.CreateDirectory(folderPath);
                    }
                    string imageName = "product-" + index.ToString() + "-F" + Masters_Carpet_DefineItemCodeOther.ItemFinishedId.ToString() + "-" + DateTime.Now.Ticks.ToString() + "-img" + Path.GetExtension(uploadedFile.FileName);
                    string imgPath = Path.Combine(folderPath, imageName);
                    uploadedFile.SaveAs(imgPath);

                    //                    string query = @"Insert into MAIN_ITEM_IMAGE(FINISHEDID,PHOTO,MasterCompanyId)
                    //VALUES(@ItemFinishId,@PhotoId,@CompanyId)";

                    string query = @"INSERT INTO tblItemPhoto
(PhotoName,ItemFinishId,IsPrime,IsPublished,CreatedBy,CreatedOn)
VALUES(@PhotoName,@ItemFinishId,@IsPrime,@IsPublished,@CreatedBy,@CreatedOn)";


                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("@ItemFinishId", Masters_Carpet_DefineItemCodeOther.ItemFinishedId));
                    parameters.Add(new SqlParameter("@PhotoName", imageName));
                    parameters.Add(new SqlParameter("@IsPrime", rdPrime.SelectedItem.Value));
                    parameters.Add(new SqlParameter("@IsPublished", true));
                    parameters.Add(new SqlParameter("@CreatedBy", Masters_Carpet_DefineItemCodeOther.UserId));
                    parameters.Add(new SqlParameter("@CreatedOn", DateTime.Now));

                    var result = SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, query, parameters.ToArray());


                }
            }

            this.BindPhotoList();
        }
    }


    /// <summary>
    /// Logs in the user
    /// </summary>
    /// <param name="Username">The username</param>
    /// <param name="Password">The password</param>
    /// <returns>true if login successful</returns>
    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetAttributeMaster()
    {
        var result = new List<SelectedList>();
        SqlConnection dbcon = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {

            string query = @"SELECT AttributeId,CompanyId,AttributeName,Description,IsPublished,CreatedBy,CreatedOn
    FROM tblItemAttributeMaster Where CompanyId=@CompanyId";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@CompanyId", Masters_Carpet_DefineItemCodeOther.CompanyId));
            DataSet ds = SqlHelper.ExecuteDataset(dbcon, CommandType.Text, query, parameters.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
            {
                result = ds.Tables[0].AsEnumerable().Select(x => new
                SelectedList
                {
                    ItemId = x.Field<int>("AttributeId"),
                    ItemName = x.Field<string>("AttributeName")
                }).ToList();
            }
            var json = JsonSerializer.Serialize(result);

            return json;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            dbcon.Close();
            dbcon.Dispose();
        }






    }

    /// <summary>
    /// Logs in the user
    /// </summary>
    /// <param name="Username">The username</param>
    /// <param name="Password">The password</param>
    /// <returns>true if login successful</returns>
    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static bool SaveData(int attributeId, string attribute)
    {

        string query = @"INSERT INTO tblItemAttributes
(ItemFinishId,AttributeId,AttributeValue,IsPublished,CreatedBy,CreatedOn)
VALUES(@ItemFinishId,@AttributeId,@AttributeValue,@IsPublished,@CreatedBy,@CreatedOn)";

        if (!string.IsNullOrEmpty(attribute))
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@ItemFinishId", Masters_Carpet_DefineItemCodeOther.ItemFinishedId));
            parameters.Add(new SqlParameter("@AttributeId", attributeId));
            parameters.Add(new SqlParameter("@AttributeValue", attribute));
            parameters.Add(new SqlParameter("@IsPublished", true));
            parameters.Add(new SqlParameter("@CreatedBy", Masters_Carpet_DefineItemCodeOther.UserId));
            parameters.Add(new SqlParameter("@CreatedOn", DateTime.Now));

            var result = SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, query, parameters.ToArray());

            if (result > 0)
            {
                return true;
            }
            else
            { return false; }
        }
        else { return false; }

    }






    /// <summary>
    /// Logs in the user
    /// </summary>
    /// <param name="Username">The username</param>
    /// <param name="Password">The password</param>
    /// <returns>true if login successful</returns>
    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static bool DeleteProperty(int attributeId, string attribute)
    {

        string query = @"Delete From  tblItemAttributes Where ItemFinishId=@ItemFinishId and AttributeId=@AttributeId
and AttributeValue=@AttributeValue";

        if (!string.IsNullOrEmpty(attribute))
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@ItemFinishId", Masters_Carpet_DefineItemCodeOther.ItemFinishedId));
            parameters.Add(new SqlParameter("@AttributeId", attributeId));
            parameters.Add(new SqlParameter("@AttributeValue", attribute));
            var result = SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, query, parameters.ToArray());

            if (result > 0)
            {
                return true;
            }
            else
            { return false; }
        }
        else { return false; }

    }










    /// <summary>
    /// Logs in the user
    /// </summary>
    /// <param name="Username">The username</param>
    /// <param name="Password">The password</param>
    /// <returns>true if login successful</returns>
    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetItemsPropertyList()
    {




        string resultString = "";
        SqlConnection dbcon = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {

            string query = @"Select * from tblItemAttributeMaster x inner join tblItemAttributes y 
on x.AttributeId=y.AttributeId Where y.ItemFinishId=@ItemFinishId 
and x.CompanyId=@CompanyId";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@ItemFinishId", Masters_Carpet_DefineItemCodeOther.ItemFinishedId));
            parameters.Add(new SqlParameter("@CompanyId", Masters_Carpet_DefineItemCodeOther.CompanyId));
            DataSet ds = SqlHelper.ExecuteDataset(dbcon, CommandType.Text, query, parameters.ToArray());
            var result = ds.Tables[0].AsEnumerable().Select(x => new
            {
                AttributeId = x.Field<int>("AttributeId"),
                AttributeName = x.Field<string>("AttributeName"),
                AttributeValue = x.Field<string>("AttributeValue"),
            }).ToList();
            resultString = JsonSerializer.Serialize(result);
            dbcon.Close();
            dbcon.Dispose();


        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }



        return resultString;


    }


    public string GetImage(string fileName)
    {
        string imgDataURL = string.Empty;
        string folderPath = ConfigurationManager.AppSettings["ImagePath"];
        string imgPath = Path.Combine(folderPath, fileName);

        if (File.Exists(imgPath))
        {
            byte[] byteData = System.IO.File.ReadAllBytes(imgPath);
            string imreBase64Data = Convert.ToBase64String(byteData);
            imgDataURL = string.Format("data:image/jpg;base64,{0}", imreBase64Data);

        }



        return imgDataURL;
    }











    /// <summary>
    /// Logs in the user
    /// </summary>
    /// <param name="Username">The username</param>
    /// <param name="Password">The password</param>
    /// <returns>true if login successful</returns>
    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static bool SaveCosting(decimal OldPrice, decimal Price, decimal Cost, decimal Discount, decimal IsArrival, decimal IsCall)
    {

        string query = @"INSERT INTO tblItemCosting
(ItemFinishId,CompanyId,OldPrice,Price,Cost,Discount,IsArrival,IsCall,IsPublished,CreatedBy,CreatedOn)
VALUES
(@ItemFinishId,@CompanyId,@OldPrice,@Price,@Cost,@Discount,@IsArrival,@IsCall,@IsPublished,@CreatedBy,@CreatedOn)
";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@ItemFinishId", Masters_Carpet_DefineItemCodeOther.ItemFinishedId));
            parameters.Add(new SqlParameter("@CompanyId", Masters_Carpet_DefineItemCodeOther.CompanyId));
            parameters.Add(new SqlParameter("@OldPrice", OldPrice));
            parameters.Add(new SqlParameter("@Price", Price));
            parameters.Add(new SqlParameter("@Cost", Cost));
            parameters.Add(new SqlParameter("@Discount", Discount));
            parameters.Add(new SqlParameter("@IsArrival", IsArrival));
            parameters.Add(new SqlParameter("@IsCall", IsCall));
            parameters.Add(new SqlParameter("@IsPublished", true));
            parameters.Add(new SqlParameter("@CreatedBy", Masters_Carpet_DefineItemCodeOther.UserId));
            parameters.Add(new SqlParameter("@CreatedOn", DateTime.Now));

            var result = SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, query, parameters.ToArray());

            if (result > 0)
            {
                return true;
            }
            else
            { return false; }
    

    }

























    protected void btnCosting_Click(object sender, EventArgs e)
    {
        string query = @"INSERT INTO tblItemCosting
(ItemFinishId,CompanyId,OldPrice,Price,Cost,Discount,IsArrival,IsCall,IsPublished,CreatedBy,CreatedOn)
VALUES
(@ItemFinishId,@CompanyId,@OldPrice,@Price,@Cost,@Discount,@IsArrival,@IsCall,@IsPublished,@CreatedBy,@CreatedOn)
";

        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(new SqlParameter("@ItemFinishId", Masters_Carpet_DefineItemCodeOther.ItemFinishedId));
        parameters.Add(new SqlParameter("@CompanyId", Masters_Carpet_DefineItemCodeOther.CompanyId));
        parameters.Add(new SqlParameter("@OldPrice", 0.0));
        parameters.Add(new SqlParameter("@Price", Convert.ToDecimal(txtItemPrice.Text)));
        parameters.Add(new SqlParameter("@Cost", 0.0));
        parameters.Add(new SqlParameter("@Discount", Convert.ToDecimal(txtDiscount.Text)));
        parameters.Add(new SqlParameter("@IsArrival", chkArrival.Checked));
        parameters.Add(new SqlParameter("@IsCall", chkCall.Checked));
        parameters.Add(new SqlParameter("@IsPublished", true));
        parameters.Add(new SqlParameter("@CreatedBy", Masters_Carpet_DefineItemCodeOther.UserId));
        parameters.Add(new SqlParameter("@CreatedOn", DateTime.Now));

        var result = SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, query, parameters.ToArray());

       
    }
}

