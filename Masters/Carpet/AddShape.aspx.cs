using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Carpet_AddShape : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtid.Text = "0";
            lablechange();
            Fill_Grid();
        }
        LblError.Visible = false;
    }
    public void lablechange()
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string shape = SqlHelper.ExecuteScalar(con, CommandType.Text, "select DISTINCT ps.Parameter_name from parameter_setting ps ,master_parameter mp where ps.Parameter_Id=mp.Parameter_Id And Ps.Company_Id=" + Session["varCompanyId"] + "  and  ps.parameter_id='4'").ToString();
            lblshapeyname.Text = shape;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddShape.aspx");
            LblError.Visible = true;
            LblError.Text = "Data base errer..................";
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
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        if (txtShape.Text != "")
        {
            CheckDuplicateData();
            if (LblError.Visible == false)
            {
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                try
                {
                    SqlParameter[] _arrPara = new SqlParameter[4];
                    _arrPara[0] = new SqlParameter("@ShapeId", SqlDbType.Int);
                    _arrPara[1] = new SqlParameter("@ShapeName", SqlDbType.NVarChar, 50);
                    _arrPara[2] = new SqlParameter("@varuserid", SqlDbType.Int);
                    _arrPara[3] = new SqlParameter("@varCompanyId", SqlDbType.Int);

                    _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                    _arrPara[1].Value = txtShape.Text.ToUpper();
                    _arrPara[2].Value = Session["varuserid"].ToString();
                    _arrPara[3].Value = Session["varCompanyId"].ToString();
                    con.Open();
                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_Shape", _arrPara);



                    txtid.Text = "0";
                    txtShape.Text = "";
                    LblError.Visible = true;
                    LblError.Text = "Save Details.....";

                }
                catch (Exception ex)
                {
                    UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddShape.aspx");
                    Logs.WriteErrorLog("Masters_Campany_Shape|cmdSave_Click|" + ex.Message);
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
                Fill_Grid();
                btndelete.Visible = false;
                BtnSave.Text = "Save";
            }
        }
        else
        {
            LblError.Visible = true;
            LblError.Text = "Fill Details........";
        }
    }
    private void CheckDuplicateData()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string strsql = @"Select * from Shape Where ShapeName='" + txtShape.Text + "' and ShapeId !=" + txtid.Text + " And MasterCompanyId=" + Session["varCompanyId"];
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            LblError.Visible = true;
            LblError.Text = "Shape AlReady Exits........";
            txtShape.Text = "";
            txtShape.Focus();
        }
        else
        {
            LblError.Visible = false;
        }
    }
    private void Fill_Grid()
    {
        gvShape.DataSource = Fill_Grid_Data();
        gvShape.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

        try
        {
            string strsql = "select ShapeId as Sr_No,ShapeName  from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " order by ShapeId";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddShape.aspx");
            Logs.WriteErrorLog("Masters_Campany_Shape|Fill_Grid_Data|" + ex.Message);
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
    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            SqlParameter[] _array = new SqlParameter[5];
            _array[0] = new SqlParameter("@ShapeId", ViewState["id"]);
            _array[1] = new SqlParameter("@MasterCompanyId", Session["varCompanyId"]);
            _array[2] = new SqlParameter("@UserId", Session["varuserid"]);
            _array[3] = new SqlParameter("@VarMsg", SqlDbType.NVarChar, 500);
            _array[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_Delete_Shape", _array);
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('" + _array[3].Value + "');", true);           
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddShape.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        Fill_Grid();
        btndelete.Visible = false;
        BtnSave.Text = "Save";
        txtShape.Text = "";
        txtid.Text = "0";
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {

    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        txtid.Text = "0";
        txtShape.Text = "";
        BtnSave.Text = "Save";
    }
    protected void gvShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gvShape.SelectedDataKey.Value.ToString();
        //Session["id"] = id;
        ViewState["id"] = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from Shape where ShapeId=" + id);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["ShapeId"].ToString();
                txtShape.Text = ds.Tables[0].Rows[0]["ShapeName"].ToString();


            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddShape.aspx");
            Logs.WriteErrorLog("Masters_Campany_countrymaster|cmdSave_Click|" + ex.Message);
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
        btndelete.Visible = true;
        BtnSave.Text = "Update";
    }
    protected void gvShape_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvShape, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gvShape_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvShape.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    //protected void gvShape_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //Add CSS class on header row.
    //    if (e.Row.RowType == DataControlRowType.Header)
    //        e.Row.CssClass = "header";

    //    //Add CSS class on normal row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Normal)
    //        e.Row.CssClass = "normal";

    //    //Add CSS class on alternate row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Alternate)
    //        e.Row.CssClass = "alternate";
    //}
}