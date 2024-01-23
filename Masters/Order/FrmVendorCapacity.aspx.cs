using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class FrmVendorCapacity : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtid.Text = "0";
            UtilityModule.ConditionalComboFill(ref DDVendor, "select Empid, Empname from empinfo order by Empname", true, "----select Vendor----");
            UtilityModule.ConditionalComboFill(ref DDMonth, "select Month_id, Month_Name from MonthTable ", true, "----select----");
            UtilityModule.ConditionalComboFill(ref DDyear, "select year,year year1 from session order by Year desc", true, "----select----");
            FillGrid();
            DDMonth.SelectedValue = DateTime.Now.Month.ToString();
            DDyear.SelectedValue = DateTime.Now.Year.ToString();

        }

    }

    protected void FillGrid()
    {
        string str = @"select V_id as Sr_No, EI.EmpName as VendorName, M.Month_Name as Month, Year,
        VC.Capacity from Vendorcapacity VC inner join Empinfo EI on VC.Vendorid=EI.Empid 
        inner join MonthTable M on VC.Month_id=M.Month_id where VC.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "";
        if (DDVendor.SelectedIndex > 0)
        {
            str = str + "  and vc.vendorid=" + DDVendor.SelectedValue;
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        DataSet ds = new DataSet();
        ds = SqlHelper.ExecuteDataset(str);
        GDview.DataSource = ds;
        GDview.DataBind();
        ds.Dispose();
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[8];

            param[0] = new SqlParameter("@Vendorid", SqlDbType.Int);
            param[1] = new SqlParameter("@Month_id", SqlDbType.Int);
            param[2] = new SqlParameter("@Year", SqlDbType.VarChar, 6);
            param[3] = new SqlParameter("@Capacity", SqlDbType.Int);
            param[4] = new SqlParameter("@varuserid", SqlDbType.Int);
            param[5] = new SqlParameter("@MasterCompanyid", SqlDbType.Int);
            param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 50);
            param[7] = new SqlParameter("@V_id", SqlDbType.Int);



            param[0].Value = DDVendor.SelectedIndex < 0 ? "0" : DDVendor.SelectedValue;
            param[1].Value = DDMonth.SelectedIndex < 0 ? "0" : DDMonth.SelectedValue;
            param[2].Value = DDyear.SelectedIndex < 0 ? "0" : DDyear.SelectedValue;
            param[3].Value = txtcapacity.Text;
            param[4].Value = Session["varuserid"].ToString();
            param[5].Value = Session["varMasterCompanyIDForERP"].ToString();
            param[6].Direction = ParameterDirection.Output;
            param[7].Value = Convert.ToInt32(txtid.Text);


            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "pro_vendorcapacity", param);
            // lblErrorMsg.Text = param[6].Value.ToString();
            txtid.Text = "0";
            tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "alert", "alert('" + param[6].Value + "');", true);

        }
        catch (Exception ex)
        {
            tran.Rollback();
            lblErrorMsg.Text = ex.Message;
            con.Close();
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (con != null)
            {
                con.Dispose();
            }
        }
        FillGrid();
        txtcapacity.Text = "";
    }
    protected void DDVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void GDview_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = GDview.SelectedDataKey.Value.ToString();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from Vendorcapacity where V_id =" + id);
        try
        {

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtid.Text = ds.Tables[0].Rows[0]["V_id"].ToString();
                txtcapacity.Text = ds.Tables[0].Rows[0]["Capacity"].ToString();
                DDVendor.SelectedValue = ds.Tables[0].Rows[0]["Vendorid"].ToString();
                DDyear.SelectedValue = ds.Tables[0].Rows[0]["Year"].ToString();
                DDMonth.SelectedValue = ds.Tables[0].Rows[0]["Month_id"].ToString();

            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = ex.Message;
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
    protected void GDview_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["OnClick"] = ClientScript.GetPostBackEventReference(this, "Select$" + e.Row.RowIndex);
            e.Row.Attributes.Add("onmouseover", "self.MouseOverOldColor=this.style.backgroundColor;this.style.backgroundColor='yellow'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=self.MouseOverOldColor");
            //e.Row.Attributes["onmouseover"] = "javascript:setmouseovercolor(this)";
            //e.Row.Attributes["onmouseout"] = "javascript:setmouseoutcolor(this)";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GDview, "select$" + e.Row.RowIndex);

        }
    }
    protected void GDview_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GDview.PageIndex = e.NewPageIndex;
        FillGrid();
    }
    protected void GDview_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int stid;
        //string stid = GDview.SelectedDataKey.Value.ToString();
        stid = Convert.ToInt32(GDview.DataKeys[e.RowIndex].Value);
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            DataSet ds = SqlHelper.ExecuteDataset(tran, CommandType.Text, "Select * From vendorcapacity Where V_id=" + stid + "");

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@V_id", SqlDbType.Int);
            param[1] = new SqlParameter("@Month_id", SqlDbType.Int);
            param[2] = new SqlParameter("@Year", SqlDbType.VarChar, 6);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4] = new SqlParameter("Vendorid", SqlDbType.Int);

            param[0].Value = ds.Tables[0].Rows[0]["V_id"].ToString();
            //param[1].Value = ds.Tables[0].Rows[0]["Vendorid"].ToString();
            param[1].Value = ds.Tables[0].Rows[0]["Month_id"].ToString();
            param[2].Value = ds.Tables[0].Rows[0]["Year"].ToString();
            param[3].Direction = ParameterDirection.Output;
            param[4].Value = ds.Tables[0].Rows[0]["vendorid"].ToString();

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "pro_DeleteVendorCapacity", param);
            lblErrorMsg.Text = param[3].Value.ToString();
            tran.Commit();
            FillGrid();
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
}