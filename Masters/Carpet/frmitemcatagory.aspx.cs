using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class frmitemcatagory : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            string str = @"Select PROCESS_NAME_ID, PROCESS_NAME 
                From PROCESS_NAME_MASTER PNM(Nolock) 
                Where MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @" Order By Process_Name ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 0, true, "--Plz Select--");

            switch (Session["varCompanyNo"].ToString())
            {
                case "3":
                    tdHSCode.Visible = true;
                    tdtxtHSCode.Visible = true;
                    break;
                default:
                    tdHSCode.Visible = false;
                    tdtxtHSCode.Visible = false;
                    break;
            }
            lablechange();
            fill_grid();
            txtcatagory.Focus();
        }
        Lblerrer.Visible = false;
    }
    private void fill_grid()
    {
        gditemcatagory.DataSource = Fill_Grid_Data();
        gditemcatagory.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        try
        {
            string strsql = @"select CATEGORY_ID as Sr_No,Category_Name as " + lblcategoryname.Text + ",Code,HSCODE from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Category_Name ";
            if (tdtxtHSCode.Visible == false)
            {
                strsql = @"select CATEGORY_ID as Sr_No,Category_Name as " + lblcategoryname.Text + ",Code from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Category_Name ";
            }
            ds = SqlHelper.ExecuteDataset(strsql);
        }
        catch (Exception ex)
        {
            Lblerrer.Visible = true;
            Lblerrer.Text = ex.Message;
        }
        return ds;
    }
    private void Store_Data()
    {
        Validated();
        if (txtcatagory.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                if (txtcatagory.Text != "")
                {
                    SqlParameter[] _arrPara1 = new SqlParameter[15];
                    _arrPara1[0] = new SqlParameter("@CATEGORY_NAME", SqlDbType.VarChar, 50);
                    _arrPara1[1] = new SqlParameter("@PARAMETER_ID_1", SqlDbType.Int);
                    _arrPara1[2] = new SqlParameter("@PARAMETER_ID_2", SqlDbType.Int);
                    _arrPara1[3] = new SqlParameter("@PARAMETER_ID_3", SqlDbType.Int);
                    _arrPara1[4] = new SqlParameter("@PARAMETER_ID_4", SqlDbType.Int);
                    _arrPara1[5] = new SqlParameter("@PARAMETER_ID_5", SqlDbType.Int);
                    _arrPara1[8] = new SqlParameter("@PARAMETER_ID_6", SqlDbType.Int);
                    _arrPara1[6] = new SqlParameter("@category_id", SqlDbType.Int);
                    _arrPara1[7] = new SqlParameter("@CODE", SqlDbType.NChar, 10);
                    _arrPara1[9] = new SqlParameter("@varuserid", SqlDbType.Int);
                    _arrPara1[10] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                    _arrPara1[11] = new SqlParameter("@HSCODE", SqlDbType.NVarChar);
                    _arrPara1[12] = new SqlParameter("@categorySeperateDetail", SqlDbType.NVarChar, 100);
                    _arrPara1[13] = new SqlParameter("@PoufTypeCategory", SqlDbType.Int);
                    _arrPara1[14] = new SqlParameter("@FirstProcessID", SqlDbType.Int);

                    _arrPara1[0].Value = txtcatagory.Text.ToUpper();
                    _arrPara1[1].Value = chk_1.Checked == true ? 1 : 0;
                    _arrPara1[2].Value = chk_2.Checked == true ? 2 : 0;
                    _arrPara1[3].Value = chk_3.Checked == true ? 3 : 0;
                    _arrPara1[4].Value = chk_4.Checked == true ? 4 : 0;
                    _arrPara1[5].Value = chk_5.Checked == true ? 5 : 0;
                    _arrPara1[8].Value = chk_6.Checked == true ? 6 : 0;
                    _arrPara1[9].Value = Session["varuserid"].ToString();
                    _arrPara1[10].Value = Session["varMasterCompanyIDForERP"].ToString();
                    _arrPara1[11].Value = TxtHSCode.Text;
                    int n = ChkBoxList.Items.Count;
                    string str = null;
                    for (int i = 0; i < n; i++)
                    {
                        if (ChkBoxList.Items[i].Selected)
                        {
                            str = str == null ? ChkBoxList.Items[i].Value : str + "," + ChkBoxList.Items[i].Value;
                        }
                    }
                    _arrPara1[12].Value = str;
                    if (btnSave.Text == "Update")
                    {
                        _arrPara1[6].Value = gditemcatagory.SelectedValue;
                        btnSave.Text = "Save";
                    }
                    else
                    {
                        _arrPara1[6].Value = 0;
                    }
                    _arrPara1[7].Value = txtode.Text.ToUpper();

                    _arrPara1[13].Value = 0;
                    if (ChkPoufTypeCategory.Checked == true)
                    {
                        _arrPara1[13].Value = 1;
                    }
                    _arrPara1[14].Value = DDProcessName.SelectedValue;

                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_ITEM_CATEGORY_PARAMETERS1", _arrPara1);
                    Tran.Commit();
                    Lblerrer.Text = "";
                    visilbe();
                }
                else
                {
                    Lblerrer.Text = "Importent field missing.............";
                }
            }
            catch (Exception ex)
            {
                Lblerrer.Visible = true;
                Lblerrer.Text = ex.Message;
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
        }
        else
        {
            if (Lblerrer.Text == "Category Name Or Code already exists............")
            {
                Lblerrer.Visible = true;
                Lblerrer.Text = "CATEGORYNAME already exists............";
            }
            else
            {
                Lblerrer.Visible = true;
                Lblerrer.Text = "Please Fill Details............";
            }
        }
        fill_grid();
    }
    private void visilbe()
    {
        for (int i = 0; i < ChkBoxList.Items.Count; i++)
        {
            ChkBoxList.Items[i].Selected = false;
        }
    }
    private void ClearAll()
    {
        txtcatagory.Text = "";
        chk_1.Checked = false;
        chk_2.Checked = false;
        chk_3.Checked = false;
        chk_4.Checked = false;
        chk_5.Checked = false;
        chk_6.Checked = false;
        ChkBoxList.Items[0].Selected = false;
        ChkBoxList.Items[1].Selected = false;
        txtode.Text = "";
        TxtHSCode.Text = "";
        txtcatagory.Focus();
        chk_1.Enabled = true;
        chk_2.Enabled = true;
        chk_3.Enabled = true;
        chk_4.Enabled = true;
        chk_5.Enabled = true;
        chk_6.Enabled = true;
        ChkPoufTypeCategory.Checked = false;
        DDProcessName.SelectedIndex = 0;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Store_Data();
        ClearAll();
        txtcatagory.Focus();
        btnSave.Text = "Save";
        btndelete.Visible = false;
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Lblerrer.Text = "";
        lblMessage.Text = "";
        btnSave.Text = "Save";
        ClearAll();
    }
    protected void gditemcatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        ClearAll();
        DataSet ds;
        string id = gditemcatagory.SelectedDataKey.Value.ToString();
        //Session["id"] = id;
        ViewState["CategoryId"] = id;
        ds = SqlHelper.ExecuteDataset("select * from ITEM_CATEGORY_PARAMETERS WHERE CATEGORY_ID=" + id + "order by PARAMETER_ID");
        btnSave.Text = "Update";
        btndelete.Visible = true;
        try
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (Convert.ToInt32(dr["PARAMETER_ID"]))
                {
                    case 1:
                        chk_1.Checked = true;
                        break;
                    case 2:
                        chk_2.Checked = true;
                        break;
                    case 3:
                        chk_3.Checked = true;
                        break;
                    case 4:
                        chk_4.Checked = true;
                        break;
                    case 5:
                        chk_5.Checked = true;
                        break;
                    case 6:
                        chk_6.Checked = true;
                        break;
                }
            }
            txtcatagory.Text = gditemcatagory.Rows[gditemcatagory.SelectedIndex].Cells[1].Text;
            txtode.Text = gditemcatagory.Rows[gditemcatagory.SelectedIndex].Cells[2].Text;
            //TxtHSCode.Text = gditemcatagory.Rows[gditemcatagory.SelectedIndex].Cells[3].Text;
            DataSet ds2 = SqlHelper.ExecuteDataset("select * from CategorySeparate WHERE CATEGORYID=" + id + "");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < ChkBoxList.Items.Count; j++)
                {
                    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                    {
                        if (ChkBoxList.Items[j].Value == ds2.Tables[0].Rows[i]["id"].ToString())
                        {
                            ChkBoxList.Items[j].Selected = true;
                        }
                    }
                }
            }
            DataSet ds3 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, 
            @"Select Item_finished_id From V_finisheditemdetail Where CATEGORY_ID = " + id + @"
            Select PoufTypeCategory, FirstProcessID From ITEM_CATEGORY_MASTER Where CATEGORY_ID = " + id + "");
            if (ds3.Tables[0].Rows.Count > 0)
            {
                chk_1.Enabled = false;
                chk_2.Enabled = false;
                chk_3.Enabled = false;
                chk_4.Enabled = false;
                chk_5.Enabled = false;
                chk_6.Enabled = false;
            }
            ChkPoufTypeCategory.Checked = false;
            if (ds3.Tables[1].Rows.Count > 0)
            {
                if (ds3.Tables[1].Rows[0]["PoufTypeCategory"].ToString() == "1")
                {
                    ChkPoufTypeCategory.Checked = true;
                }
                DDProcessName.SelectedValue = ds3.Tables[1].Rows[0]["FirstProcessID"].ToString();
            }
        }
        catch (Exception ex)
        {
            Lblerrer.Text = ex.Message;
        }
    }
    protected void gditemcatagory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gditemcatagory, "Select$" + e.Row.RowIndex);
        }
    }
    private void Validated()
    {
        try
        {
            string strsql;
            if (btnSave.Text == "Update")
            {
                strsql = "select CATEGORY_NAME,code from ITEM_CATEGORY_MASTER where CATEGORY_ID<>" + ViewState["CategoryId"] + " and (CATEGORY_NAME='" + txtcatagory.Text + "') And  MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            else
            {
                strsql = "select CATEGORY_NAME,code from ITEM_CATEGORY_MASTER where CATEGORY_NAME='" + txtcatagory.Text + "' And  MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            DataSet ds = SqlHelper.ExecuteDataset(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Lblerrer.Visible = true;
                Lblerrer.Text = "Category Name Or Code already exists............";
                txtcatagory.Text = "";
                txtcatagory.Focus();
            }
            else
            {
                Lblerrer.Text = "";
            }
        }
        catch (Exception ex)
        {
            Lblerrer.Text = ex.Message;
        }
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
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select CATEGORY_ID from ITEM_MASTER where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " AND  CATEGORY_ID=" + ViewState["CategoryId"].ToString()));
            SqlParameter[] _array = new SqlParameter[2];
            _array[0] = new SqlParameter("@CATEGORY_ID", ViewState["CategoryId"].ToString());
            _array[1] = new SqlParameter("@Message", SqlDbType.NVarChar, 100);
            _array[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteItem_Category", _array);
            Tran.Commit();
            Lblerrer.Visible = true;
            Lblerrer.Text = _array[1].Value.ToString();
            txtcatagory.Text = "";
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            Lblerrer.Visible = true;
            Lblerrer.Text = ex.Message;
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
        btnSave.Text = "Save";
        ClearAll();
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblqualityname1.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        lblsizename.Text = ParameterList[4];
        lblcategoryname.Text = ParameterList[5];
        lblshqadename.Text = ParameterList[7];
    }
  
  
}