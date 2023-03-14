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

public partial class Masters_Campany_frmvendorwiserate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {

            string Qry = @"select PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER  order by PROCESS_NAME
                           select ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM  order by CATEGORY_NAME";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Qry);
            UtilityModule.ConditionalComboFillWithDS(ref DDprocess, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, ds, 1, true, "--Plz Select--");
            ViewState["vendorid"] = Request.QueryString["vid"];
        }
    }
    protected void CategoryDependControls()
    {
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
        TDShape.Visible = false;
        TDSize.Visible = false;
        TDShade.Visible = false;

        UtilityModule.ConditionalComboFill(ref dditemname, "select ITEM_ID,ITEM_NAME from ITEM_MASTER where CATEGORY_ID=" + ddCatagory.SelectedValue + " order by ITEM_NAME", true, "--Select--");
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select PARAMETER_ID from ITEM_CATEGORY_PARAMETERS Where CATEGORY_ID=" + ddCatagory.SelectedValue + "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (Convert.ToString(dr["PARAMETER_ID"]))
                {
                    case "1":
                        TDQuality.Visible = true;
                        break;
                    case "2":
                        TDDesign.Visible = true;

                        break;
                    case "3":
                        TDColor.Visible = true;

                        break;
                    case "4":
                        TDShape.Visible = true;
                        break;
                    case "5":
                        TDSize.Visible = true;
                        break;
                    case "6":
                        TDShade.Visible = true;
                        break;
                }
            }

        }
    }
    protected void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Sizetype, DropDownList Shade, int Itemid, System.Web.UI.HtmlControls.HtmlTableCell tdQuality = null, System.Web.UI.HtmlControls.HtmlTableCell tdDesign = null, System.Web.UI.HtmlControls.HtmlTableCell tdcolor = null, System.Web.UI.HtmlControls.HtmlTableCell tdshape = null, System.Web.UI.HtmlControls.HtmlTableCell tdshade = null)
    {
        if (tdQuality.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref Quality, "select QualityId,QualityName from Quality Where Item_Id=" + Itemid + " order by QualityName", true, "--Select--");
        }

        string str;
        str = @"SELECT DESIGNID,DESIGNNAME from DESIGN Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By DESIGNNAME
            SELECT COLORID,COLORNAME FROM COLOR Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By COLORNAME
            SELECT SHAPEID,SHAPENAME FROM SHAPE Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By SHAPENAME
            select ShadecolorId,ShadeColorName from  shadecolor order by ShadeColorName";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (tdDesign.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref Design, ds, 0, true, "--Select--");
        }
        if (tdcolor.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref Color, ds, 1, true, "--Select--");
        }
        if (tdshape.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref Shape, ds, 2, true, "--Select--");
            UtilityModule.ConditionalComboFill(ref Sizetype, "select val,type from sizetype", false, "");
        }
        if (tdshade.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref Shade, ds, 3, true, "--Select--");
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(ddquality, dddesign, ddcolor, ddshape, DDsizetype, ddlshade, Convert.ToInt16(dditemname.SelectedValue), TDQuality, TDDesign, TDColor, TDShape, TDShade);
        FillGrid();

    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int varfinishedid = UtilityModule.getItemFinishedId(dditemname, ddquality, dddesign, ddcolor, ddshape, ddsize, txtprodcode, Tran, ddlshade, "", Convert.ToInt32(Session["varCompanyId"]));
            string insert = "insert into vendorwiseitemrate(VendorId,Item_Finished_id,Rate,Userid,Sizeflag,Processid) values (" + ViewState["vendorid"] + "," + varfinishedid + "," + txtrate.Text + "," + Session["varuserid"] + "," +(TDSize.Visible==true?DDsizetype.SelectedValue:"0") + "," + DDprocess.SelectedValue + ")";
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, insert);
            lblmsg.Text = "Data Saved successfully...";
            Tran.Commit();
            FillGrid();
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
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        CategoryDependControls();
        FillGrid();
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize(DDsizetype, ddshape, ddsize);
    }
    protected void FillGrid()
    {
        string str = @"select vf.CATEGORY_NAME,vf.ITEM_NAME+' '+vf.QualityName+' '+vf.designName+' '+vf.ColorName+' '+vf.shapename+' '+case When V.Sizeflag=0 Then vf.SizeFt When v.Sizeflag=1 Then vf.SizeMtr else vf.SizeInch End as ItemDescription,
                        v.Rate,v.Dateadded 
                        from vendorwiseitemrate V inner join V_FinishedItemDetail vf on
                        V.Item_Finished_id=vf.ITEM_FINISHED_ID Where V.Vendorid=" + ViewState["vendorid"];
        if (ddCatagory.SelectedIndex > 0)
        {
            str = str + " and Vf.category_id=" + ddCatagory.SelectedValue;
        }
        if (dditemname.SelectedIndex > 0)
        {
            str = str + " and Vf.Item_id=" + dditemname.SelectedValue;
        }
        str = str + " order by v.id desc";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GDDetail.DataSource = ds.Tables[0];
        GDDetail.DataBind();

    }
    protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize(DDsizetype, ddshape,ddsize);
    }
    protected void FillSize(DropDownList SizeType, DropDownList Shape, DropDownList Size)
    {
        string size = "";
        string str = "";

        switch (SizeType.SelectedValue)
        {
            case "1":
                size = "Sizemtr";
                break;
            case "0":
                size = "Sizeft";
                break;
            case "2":
                size = "Sizeinch";
                break;
            default:
                size = "Sizeft";
                break;
        }

        str = "Select Distinct S.Sizeid,S." + size + " As  " + size + @" From Size S 
                 Where shapeid=" + Shape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " order by " + size + "";

        UtilityModule.ConditionalComboFill(ref Size, str, true, "--Select--");
    }
  
}