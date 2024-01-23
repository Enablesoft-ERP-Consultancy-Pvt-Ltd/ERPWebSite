using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class CategorySeparate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            UtilityModule.ConditionalComboFill(ref DDcategoryName, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by CATEGORY_Id", true, "--SELECT--");
            DDcategorytype.SelectedIndex = 2;
            txtid.Text = "0";
            lablechange();
            Fill_Grid();
        }
        Lblerror.Visible = false;
           
    }
    public void lablechange()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"select DISTINCT ps.Parameter_name from parameter_setting ps where ps.parameter_id='6' And PS.Company_Id=" + Session["varMasterCompanyIDForERP"];
            con.Open();
            string categoryname = SqlHelper.ExecuteScalar(con, CommandType.Text, strsql).ToString();
            lblcategoryname.Text = categoryname;
            lblcategorytype.Text = categoryname+"_TYPE";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/CategorySeparate.aspx");
            Lblerror.Visible = true;
            Lblerror.Text = "Data base errer..................";
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
    protected void Gvcategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Gvcategory.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (duplicate() == false)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[5];
                _arrPara[0] = new SqlParameter("@Category_Typeid", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@Category_id", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@txtid", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                _arrPara[0].Value = DDcategorytype.SelectedValue;
                _arrPara[1].Value = DDcategoryName.SelectedValue;
                _arrPara[3].Value = Session["varuserid"].ToString();
                _arrPara[4].Value = Session["varMasterCompanyIDForERP"].ToString();
                _arrPara[2].Value = txtid.Text;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_CategorySeparate", _arrPara);
                //  ds.Tables[0].Columns["customerid"].ColumnName = "SerialNo.";
                Fill_Grid();
                
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Carpet/CategorySeparate.aspx");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                    con.Dispose();
                }
            }
            Lblerror.Visible = true;
            Lblerror.Text = "Save Details......";
            txtid.Text = "0";
            btnsave.Text = "Save";
        }
        else
        {
            Lblerror.Visible = true;
            Lblerror.Text = "Value already exit";
        }
        UtilityModule.ConditionalComboFill(ref DDcategoryName, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by CATEGORY_Id", true, "--SELECT--");
    }
    private void Fill_Grid()
    {
        Gvcategory.DataSource = Fill_Grid_Data();
        Gvcategory.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "Select distinct Sr_no, case when id=0 then 'FinishedItem' else 'RawMaterial' End " + lblcategorytype.Text + ",CATEGORY_NAME AS " +lblcategoryname.Text+" from CategorySeparate cS,ITEM_CATEGORY_MASTER Im Where CS.Categoryid=IM.Category_id and Id=" + DDcategorytype.SelectedValue  +" And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);          
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/CategorySeparate.aspx");
            Logs.WriteErrorLog("Masters_Campany_frmBank|Fill_Grid_Data|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        return ds;
    }
    protected void DDcategorytype_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDcategoryName, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER  Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by CATEGORY_Id", true, "--SELECT--");
        Fill_Grid();
    }
    protected void Gvcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = Gvcategory.SelectedDataKey.Value.ToString();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from CategorySeparate where sr_no=" + id);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                
                DDcategoryName.SelectedValue = ds.Tables[0].Rows[0]["categoryid"].ToString();
                DDcategorytype.SelectedValue =  ds.Tables[0].Rows[0]["id"].ToString();
                
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/CategorySeparate.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        txtid.Text = id;
        btnsave.Text = "Update";

    }
    protected void Gvcategory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Gvcategory, "Select$" + e.Row.RowIndex);
        }
    }
    Boolean duplicate()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        Boolean a=true;
        try
        {          
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from CategorySeparate where id='" + DDcategorytype.SelectedValue + "' and categoryid='" + DDcategoryName.SelectedValue + "' and sr_no !=" + txtid.Text + " ");
            
            if (ds.Tables[0].Rows.Count >0)
            {
                a = true;

            }
            else
            {
                a=false;
            }
        }
        catch (Exception ex) { UtilityModule.MessageAlert(ex.Message, "Master/Carpet/CategorySeparate.aspx"); }
        finally
        {
            con.Close();
        }
        return a;
    }
    protected void DDcategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
   
}