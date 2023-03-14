using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Campany_Detail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            fill_grid();

        }

    }
    protected void Gvdetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    protected void Gvdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Gvdetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void fill_grid()
    {

        string strsql = "select detailid, DName,Fathers_Name,RESI_Address,TEL_No from detail";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Gvdetail.DataSource = ds.Tables[0];
            Gvdetail.DataBind();
        }
        else
        {
            Gvdetail.DataSource = null;
            Gvdetail.DataBind();
        }
    }
    protected void refresh_form()
    {
        txtname.Text = "";
        txtfathername.Text = "";
        txtresiaddress.Text = "";
        txttelno.Text = ""; 

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[8];
            _arrpara[0] = new SqlParameter("@DName", SqlDbType.VarChar, 50);
            _arrpara[1] = new SqlParameter("@Fathers_Name", SqlDbType.VarChar, 50);
            _arrpara[2] = new SqlParameter("@RESI_Address", SqlDbType.VarChar, 50);
            _arrpara[3] = new SqlParameter("@TEL_No", SqlDbType.Int);
            _arrpara[4] = new SqlParameter("@userid", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
            _arrpara[6] = new SqlParameter("@detailid", SqlDbType.Int);
            _arrpara[7] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);



            _arrpara[0].Value = txtname.Text;
            _arrpara[1].Value = txtfathername.Text;
            _arrpara[2].Value = txtresiaddress.Text;
            _arrpara[3].Value = txttelno.Text;
            _arrpara[4].Value = Session["varuserid"].ToString();
            _arrpara[5].Value = Session["varcompanyid"].ToString();
            _arrpara[6].Direction = ParameterDirection.InputOutput;

            if (btnSave.Text == "Update")
            {
                _arrpara[6].Value = ViewState["detailid"];
            }
            else
            {
                _arrpara[6].Value = 0;
            }
            _arrpara[7].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_Detail", _arrpara);
            lblErr.Visible = true;
            lblErr.Text = _arrpara[7].Value.ToString();
            Tran.Commit();
            ViewState["detailid"] = 0;
            
            fill_grid();
        }
        catch (Exception ex)
        {
            lblErr.Visible = true;
            lblErr.Text = ex.Message;
            Tran.Rollback();
            Logs.WriteErrorLog("Master_Company_Detail|BtnSave_Click|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master_Company_Detail.aspx");

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
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["id"] == "1")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "OnLoad", "onSuccess();", true);

                }
            }
        }
        refresh_form();


    }
    protected void Gvdetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["detailid"] = (int)Gvdetail.SelectedDataKey.Value;
        int rowindex = Gvdetail.SelectedIndex;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            //DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "select DName,Fathers_Name,RESI_Address,TEL_No from detail");

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    txtname.Text = ds.Tables[0].Rows[0]["DName"].ToString();
            //    txtfathername.Text = ds.Tables[0].Rows[0]["Fathers_Name"].ToString();
            //    txtresiaddress.Text = ds.Tables[0].Rows[0]["RESI_Address"].ToString();
            //    txttelno.Text = ds.Tables[0].Rows[0]["TEL_No"].ToString();
            //}
            Label lblDName = ((Label)Gvdetail.Rows[rowindex].FindControl("lblDName"));
            Label lblFathers_Name = ((Label)Gvdetail.Rows[rowindex].FindControl("lblFathers_Name"));
            Label lblRESI_Address = ((Label)Gvdetail.Rows[rowindex].FindControl("lblRESI_Address"));
            Label lblTEL_No = ((Label)Gvdetail.Rows[rowindex].FindControl("lblTEL_No"));
            txtname.Text = lblDName.Text; //DirectorName
            txtfathername.Text = lblFathers_Name.Text;//fathers_name
            txtresiaddress.Text = lblRESI_Address.Text;//RESI. Address 
            txttelno.Text = lblTEL_No.Text;// TEL No 
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/Detail.aspx");
            Logs.WriteErrorLog("Masters_Campany_Detail|Fill_Grid_Data|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        btnSave.Text = "Update";
        btndelete.Visible = true;
        //refresh_form();

    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            string str = "Delete from detail where detailid=" + ViewState["detailid"];

            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            Tran.Commit();
            lblErr.Text = "Data Deleted successfully........";

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/Detail.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        fill_grid();
        btndelete.Visible = false;
        refresh_form();
    }
}