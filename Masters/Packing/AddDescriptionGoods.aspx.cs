using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Packing_AddDescriptionGoods : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            TxtDescriptionOfGoods.Focus();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (TxtDescriptionOfGoods.Text != "")
        {
            CheckDuplicateData();
            if (lblErrorMessage.Visible == false)
            {
                Store_Data();
            }
            TxtDescriptionOfGoods.Text = "";
            btnSave.Text = "Save";
            btndelete.Visible = false;
        }
        else
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = "Fill Details........";
        }
    }
    private void CheckDuplicateData()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string strsql = @"Select * from GoodsDesc Where GoodsName='" + TxtDescriptionOfGoods.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = "Color AlReady Exits........";
            TxtDescriptionOfGoods.Text = "";
            TxtDescriptionOfGoods.Focus();
        }
        else
        {
            lblErrorMessage.Visible = false;
        }
    }
    private void Store_Data()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            CheckDuplicateData();
            if (lblErrorMessage.Visible == false)
            {
                SqlParameter[] _arrPara = new SqlParameter[4];
                _arrPara[0] = new SqlParameter("@GoodsId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@GoodsName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@varCompanyId", SqlDbType.Int);

                _arrPara[0].Value = 0;
                _arrPara[1].Value = TxtDescriptionOfGoods.Text.ToUpper();
                _arrPara[2].Value = Session["varuserid"].ToString();
                _arrPara[3].Value = Session["varCompanyId"].ToString();
                con.Open();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_Goods_Description", _arrPara);
                ClearAll();
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = "Save Details.............";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/AddDescriptionGoods.aspx");
            Logs.WriteErrorLog("Masters_Carpet_FrmColor|cmdSave_Click|" + ex.Message);
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
        //fill_grid();
    }
    private void fill_grid()
    {
        //gdcolor.DataSource = Fill_Grid_Data();
        //gdcolor.DataBind();
    }

    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"Select GoodsId,GoodsName From GoodsDesc Where MasterCompanyId=" + Session["varCompanyId"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            ds.Tables[0].Columns["GoodsName"].ColumnName = "Goods Name";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/AddDescriptionGoods.aspx");
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
    private void ClearAll()
    {
        TxtDescriptionOfGoods.Text = "";
    }
}
