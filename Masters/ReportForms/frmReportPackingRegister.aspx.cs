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

public partial class Masters_ReportForms_frmReportPackingRegister : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            //SqlhelperEnum.FillDropDown(AllEnums.MasterTables.Units, DDUnit, pID: "UnitsId", pName: "Unitname");
            string str = null;
            DataSet ds = null;
            str = @"select U.UnitsId,U.UnitName from Units U inner join Units_authentication UA on U.unitsId=UA.UnitsId and UA.Userid=" + Session["varuserid"] + @" order by U.unitsId
                  select Item_id,Item_name from Item_master IM,categoryseparate CS Where IM.category_id=CS.CategoryId And CS.id=0 order by Item_name
                  select  ShapeId,ShapeName from Shape with(nolock)
                  select ColorId,ColorName from color order by ColorName";

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDUnit, ds, 0, true, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDArticle, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDShape, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDColor, ds, 3, true, "--Plz Select--");


            txtFromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

        }
    }
    protected void btnprint_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            string str = @"select *,'" + txtFromdate.Text + "' As FromDate,'" + txtToDate.Text + @"' As ToDate 
            From V_PackingRegister 
            Where CompanyID = " + Session["CurrentWorkingCompanyID"] + " And UnitsId=" + DDUnit.SelectedValue + " And Item_id=" + DDArticle.SelectedValue + @" And 
            Date>='" + txtFromdate.Text + "' And Date<='" + txtToDate.Text + "' And MasterCompanyid=" + Session["varcompanyid"] + "";
            if (DDColor.SelectedIndex > 0)
            {
                str = str + "  and colorid=" + DDColor.SelectedValue;
            }
            if (DDShape.SelectedIndex > 0)
            {
                str = str + "  and shapeid=" + DDShape.SelectedValue;
            }
            if (ddSize.SelectedIndex > 0)
            {
                str = str + "  and Sizeid=" + ddSize.SelectedValue;
            }
            str = str + @"
                 select CompanyId,'~/Images/Logo/' + cast(CompanyId as nvarchar)+'_company.gif' Photo From Companyinfo Where CompanyId=" + Session["CurrentWorkingCompanyID"];
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //for Image
                ds.Tables[1].Columns.Add("Image", typeof(System.Byte[]));
                foreach (DataRow dr in ds.Tables[1].Rows)
                {

                    if (Convert.ToString(dr["Photo"]) != "")
                    {
                        FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                        if (TheFile.Exists)
                        {
                            string img = dr["Photo"].ToString();
                            img = Server.MapPath(img);
                            Byte[] img_Byte = File.ReadAllBytes(img);
                            dr["Image"] = img_Byte;
                        }
                    }
                }
                Session["rptFileName"] = "~\\Reports\\RptPackingRegister.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptPackingRegister.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Information", "alert('No records found');", true);
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddSize, "select SizeId,SizeMtr from size where Shapeid=" + DDShape.SelectedValue + " order by SizeMtr", true, "--Plz Select--");
    }
}
