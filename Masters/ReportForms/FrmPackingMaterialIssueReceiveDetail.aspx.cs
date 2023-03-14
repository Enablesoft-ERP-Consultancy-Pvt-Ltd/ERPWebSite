using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ClosedXML.Excel;
using System.IO;


public partial class Masters_ReportForms_FrmPackingMaterialIssueReceiveDetail : System.Web.UI.Page
{
    public static string Export = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,Companyname from Companyinfo CI(Nolock),Company_Authentication CA(Nolock) 
            Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By Companyname 
            Select Distinct  a.DepartmentId, D.DepartmentName 
            From PackingMaterialIssueMaster a(Nolock) 
            JOIN Department D(Nolock) ON D.DepartmentId = a.DepartmentId 
            Where a.MasterCompanyId = " + Session["varCompanyId"] + @" And a.CompanyID = " + Session["CurrentWorkingCompanyID"].ToString() + @"
            Order by D.DepartmentName 
            Select Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=cs.Categoryid and cs.id=1 and ICM.MasterCompanyid=" + Session["varcompanyid"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--Plz Select--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDDepartment, ds, 1, true, "--Plz Select--");

            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, "--Plz Select--");

            txtFromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
    }
    private void ddlcategorycange()
    {
        DDQuality.Items.Clear();
        DDDesign.Items.Clear();
        DDColor.Items.Clear();
        DDShape.Items.Clear();
        DDSize.Items.Clear();
        TRDDQuality.Visible = false;
        TRDDDesign.Visible = false;
        TRDDColor.Visible = false;
        TRDDShape.Visible = false;
        TRDDSize.Visible = false;
        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME 
                      FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on 
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        TRDDQuality.Visible = true;
                        break;
                    case "2":
                        TRDDDesign.Visible = true;
                        break;
                    case "3":
                        TRDDColor.Visible = true;
                        break;
                    case "6":
                        //TDIColorShade.Visible = true;
                        break;
                    case "4":
                        TRDDShape.Visible = true;
                        break;
                    case "5":
                        TRDDSize.Visible = true;
                        break;
                    case "10":
                        //TDIcolor.Visible = true;
                        break;
                }
            }
        }

        string stritem = "select distinct IM.Item_Id,IM.Item_Name from  Item_Parameter_Master IPM  inner Join Item_Master IM on IM.Item_Id=IPM.Item_Id inner join Item_Category_Master ICM on ICM.Category_Id=IM.Category_Id where  IM.Category_Id=" + DDCategory.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " order by IM.item_name";
        UtilityModule.ConditionalComboFill(ref ddItemName, stritem, true, "---Select Item----");
    }
    private void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, int Itemid)
    {
        string Str = @"SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + Itemid + " And MasterCompanyId=" + Session["varCompanyId"] + @" Order By QUALITYNAME
                     SELECT DESIGNID,DESIGNNAME from DESIGN Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By DESIGNNAME
                     SELECT COLORID,COLORNAME FROM COLOR Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By COLORNAME
                     SELECT SHAPEID,SHAPENAME FROM SHAPE Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By SHAPENAME
                     SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR Where  MasterCompanyId=" + Session["varCompanyId"] + " Order By SHADECOLORNAME";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref Quality, ds, 0, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Design, ds, 1, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Color, ds, 2, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Shape, ds, 3, true, "--SELECT--");
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(DDQuality, DDDesign, DDColor, DDShape, Convert.ToInt16(ddItemName.SelectedValue));
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void FillSize()
    {
        string size = "";
        string str = "";

        size = "Sizeft";
        str = "Select Distinct S.Sizeid,S." + size + " As  " + size + @" From Size S 
                 Where shapeid=" + DDShape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " order by " + size + "";

        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--SELECT--");
    }

    protected void btnprint_Click(object sender, EventArgs e)
    {
        PackingIssueReceiveDetail();
    }

    protected void PackingIssueReceiveDetail()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_GetPackingRawMaterialIssueReceiveDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@CompanyID", DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@DepartmentID", DDDepartment.SelectedIndex <= 0 ? "0" : DDDepartment.SelectedValue);
            cmd.Parameters.AddWithValue("@CategoryID", ddItemName.SelectedIndex <= 0 ? "0" : ddItemName.SelectedValue);
            cmd.Parameters.AddWithValue("@ItemID", ddItemName.SelectedIndex <= 0 ? "0" : ddItemName.SelectedValue);
            cmd.Parameters.AddWithValue("@Fromdate", txtFromdate.Text);
            cmd.Parameters.AddWithValue("@ToDate", txtToDate.Text);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            con.Close();
            con.Dispose();
            if (ds.Tables[0].Rows.Count > 0)
            {
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("Packing Material Iss Rec Detail");

                //*************
                //***********
                sht.Row(1).Height = 24;
                sht.Range("A1:L1").Merge();
                sht.Range("A1:L1").Style.Font.FontSize = 10;
                sht.Range("A1:L1").Style.Font.Bold = true;
                sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:L1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1:L1").Style.Alignment.WrapText = true;
                //************
                sht.Range("A1").SetValue("Packing RawMaterial Issue Receive Detail (From -" + txtFromdate.Text + " To-" + txtToDate.Text + ") ");

                sht.Range("A2:L2").Style.Font.FontSize = 10;
                sht.Range("A2:L2").Style.Font.Bold = true;

                sht.Range("C2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("F2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("A2").Value = "Company Name";
                sht.Range("B2").Value = "Date";
                sht.Range("C2").Value = "Challan No";
                sht.Range("D2").Value = "Category Name";
                sht.Range("E2").Value = "Description";
                sht.Range("F2").Value = "Issue Qty";
                sht.Range("G2").Value = "Rec Qty";
                sht.Range("H2").Value = "Reject Qty";
                sht.Range("I2").Value = "Unit Name";
                sht.Range("J2").Value = "Godown Name";
                sht.Range("K2").Value = "Remark";
                sht.Range("L2").Value = "User Name";

                int row = 3;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CompanyName"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Date"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ChallanNo"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["CategoryName"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ItemDescription"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["IssueQty"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["ReceiveQty"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["RejectQty"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["UnitName"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["GodownName"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["Remarks"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["UserName"]);

                    sht.Range("C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    row = row + 1;
                }

                ds.Dispose();
                
                String Path;
                sht.Columns(1, 18).AdjustToContents();
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("Packing Material Iss Rec _" + DateTime.Now + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JobPendalt", "alert('No records found..')", true);
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Masters/RawMaterial/IndentRawIssue");
            ScriptManager.RegisterStartupScript(Page, GetType(), "JobPendalt", "alert('" + ex.Message + "')", true);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
}