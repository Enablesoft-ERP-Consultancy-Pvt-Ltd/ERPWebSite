using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Text;
public partial class Masters_Campany_WareHouse : CustomPage
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
            Fill_Grid();
        }
        Label1.Visible = false;
        Session["ReportPath"] = "Reports/warehouse.rpt";
        Session["CommanFormula"] = "";
    }
    private void Fill_Grid()
    {
        gdWarehouse.DataSource = Fill_Grid_Data();
        gdWarehouse.DataBind();
        fill_combo();
    }
    protected void cmbCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        gdWarehouse.DataSource = Fill_Grid_Data();
        gdWarehouse.DataBind();
        
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {            
            string str = "";

            str = @"SELECT WHM.Warehouseid as SrNo,CI.CustomerCode, WHM.Warehousecode, 
                      WHM.Warehousename, WHM.Address, WHM.City, WHM.State,WHM.Country,isnull(WHM.WHConsignee,'') as WHConsignee,
                      isnull(WHM.WHBuyerOtherConsignee,'') as WHBuyerOtherConsignee, isnull(WHM.WHShipTo,'') as WHShipTo,
                      isnull(WHM.WHCountryFinalDestination,'') as WHCountryFinalDestination,isnull(WHM.WHPortOfLoading,'') as WHPortOfLoading,
                      isnull(WHM.WHPortOfDischarge,'') as WHPortOfDischarge,isnull(WHM.WHPlaceOfDelivery,'') as WHPlaceOfDelivery,isnull(WHM.WHFinalDestination,'') as WHFinalDestination
                      FROM dbo.WarehouseMaster  WHM INNER JOIN
                      dbo.customerinfo CI ON WHM.Customerid =CI.CustomerId where WHM.MasterCompanyId=" + Session["varCompanyId"] + "";

//             str = @"SELECT WHM.Warehouseid as SrNo,CI.CustomerCode, WHM.Warehousecode, 
//                      WHM.Warehousename, WHM.Address, WHM.City, WHM.State,WHM.Country
//                      FROM dbo.WarehouseMaster  WHM INNER JOIN
//                      dbo.customerinfo CI ON WHM.Customerid =CI.CustomerId where WHM.MasterCompanyId=" + Session["varCompanyId"] + "";
             if (cmbCustomerCode.SelectedIndex > 0)
             {
                 str = str + " and WHM.Customerid=" + cmbCustomerCode.SelectedValue + "";
             }
             str = str + " order by Warehouseid";
            //  string str1 = "select CustomerCode,Customerid from customerinfo ";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/WareHouse.aspx");
            //
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
    private void fill_combo()
    {
        //CommanFunction.FillCombo(cmbCustomerCode, "select  Customerid,CustomerCode from customerinfo Where MasterCompanyId=" + Session["varCompanyid"] + " ORDER by customercode " );

        UtilityModule.ConditionalComboFill(ref cmbCustomerCode, "select  Customerid,CustomerCode from customerinfo Where MasterCompanyId=" + Session["varCompanyid"] + " ORDER by customercode", true, "--Select--");

        UtilityModule.ConditionalComboFill(ref DDPortOfLoading, "Select GoodsReceiptId, StationName from GoodsReceipt where MasterCompanyId=" + Session["varCompanyId"] + @" order by StationName", true, "--Select--");
    }
    protected void gdWarehouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gdWarehouse.SelectedDataKey.Value.ToString();
        txtid.Text = id;
        ViewState["WareHouseId"] = txtid.Text;

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@warehouseid", id);
            param[1] = new SqlParameter("@MasterCompanyId", Session["VarCompanyId"]);
            param[2] = new SqlParameter("@UserId", Session["VarUserId"]);

            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "PRO_GETWAREHOUSEDETAIL", param);

            if (ds.Tables[0].Rows.Count == 1)
            { 


                cmbCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["CustomerId"].ToString();
                //cmbCustomerCode.SelectedItem.Text = ds.Tables[0].Rows[0]["CustomerCode"].ToString();
                txtWareHouseCode.Text = ds.Tables[0].Rows[0]["Warehousecode"].ToString();
                txtWareHouseName.Text = ds.Tables[0].Rows[0]["Warehousename"].ToString();
                txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtState.Text = ds.Tables[0].Rows[0]["State"].ToString();
                txtCountry.Text = ds.Tables[0].Rows[0]["Country"].ToString();
                txtConsignee.Text = ds.Tables[0].Rows[0]["WHConsignee"].ToString();
                txtBuyerOtherThanConsignee.Text = ds.Tables[0].Rows[0]["WHBuyerOtherConsignee"].ToString();
                txtShipTo.Text = ds.Tables[0].Rows[0]["WHShipTo"].ToString();
                txtCountryFinalDestination.Text = ds.Tables[0].Rows[0]["WHCountryFinalDestination"].ToString();
                DDPortOfLoading.SelectedValue = ds.Tables[0].Rows[0]["WHPortOfLoading"].ToString();
                txtPortOfDischarge.Text = ds.Tables[0].Rows[0]["WHPortOfDischarge"].ToString();
                txtPlaceOfDelivery.Text = ds.Tables[0].Rows[0]["WHPlaceOfDelivery"].ToString();
                txtFinalDestination.Text = ds.Tables[0].Rows[0]["WHFinalDestination"].ToString();                

            }
            //BindGrid();            

            Tran.Commit();
        }
        catch (Exception ex)
        {
            lbl.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

        //txtid.Text = gdWarehouse.SelectedRow.Cells[0].Text;
        ////Session["id"] = txtid.Text;
        //ViewState["WareHouseId"] = txtid.Text;
        //cmbCustomerCode.SelectedItem.Text = gdWarehouse.SelectedRow.Cells[1].Text.Replace("&nbsp;", "");
        //txtWareHouseCode.Text = gdWarehouse.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
        //txtWareHouseName.Text = gdWarehouse.SelectedRow.Cells[3].Text.Replace("&nbsp;", "");
        //txtCity.Text = gdWarehouse.SelectedRow.Cells[5].Text.Replace("&nbsp;", "");
        //txtAddress.Text = gdWarehouse.SelectedRow.Cells[4].Text.Replace("&nbsp;", "");
        //txtState.Text = gdWarehouse.SelectedRow.Cells[6].Text.Replace("&nbsp;", "");
        //txtCountry.Text = gdWarehouse.SelectedRow.Cells[7].Text.Replace("&nbsp;", "");
        btnSave.Text = "Update";
        btndelete.Visible = true;
    }
    protected void gdWarehouse_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdWarehouse, "select$" + e.Row.RowIndex);
        }
    }
    protected void Save_Click(object sender, EventArgs e)
    {
        Validated();
        if (txtWareHouseCode.Text != "" && txtWareHouseName.Text != "" && cmbCustomerCode.SelectedIndex>0)
        {
           
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlTransaction Tran = con.BeginTransaction();
                try
                {
                    SqlParameter[] _arrpara = new SqlParameter[18];
                    _arrpara[0] = new SqlParameter("@Warehouseid", SqlDbType.Int);
                    _arrpara[1] = new SqlParameter("@customerid", SqlDbType.Int);
                    _arrpara[2] = new SqlParameter("@Warehousecode", SqlDbType.NVarChar, 50);
                    _arrpara[3] = new SqlParameter("@Warehousename", SqlDbType.NVarChar, 50);
                    _arrpara[4] = new SqlParameter("@Address", SqlDbType.NVarChar, 50);
                    _arrpara[5] = new SqlParameter("@City", SqlDbType.NVarChar, 50);
                    _arrpara[6] = new SqlParameter("@State", SqlDbType.NVarChar, 50);
                    _arrpara[7] = new SqlParameter("@Country", SqlDbType.NVarChar, 50);
                    _arrpara[8] = new SqlParameter("@varuserid", SqlDbType.Int);
                    _arrpara[9] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                    _arrpara[10] = new SqlParameter("@WHConsignee", SqlDbType.VarChar,500);
                    _arrpara[11] = new SqlParameter("@WHBuyerOtherConsignee", SqlDbType.VarChar,500);
                    _arrpara[12] = new SqlParameter("@WHShipTo", SqlDbType.VarChar,500);
                    _arrpara[13] = new SqlParameter("@WHCountryFinalDestination", SqlDbType.VarChar,50);
                    _arrpara[14] = new SqlParameter("@WHPortOfLoading", SqlDbType.VarChar,50);
                    _arrpara[15] = new SqlParameter("@WHPortOfDischarge", SqlDbType.VarChar,50);
                    _arrpara[16] = new SqlParameter("@WHPlaceOfDelivery", SqlDbType.VarChar,50);
                    _arrpara[17] = new SqlParameter("@WHFinalDestination", SqlDbType.VarChar,50);
                    if (btnSave.Text == "Update")
                    {
                        _arrpara[0].Value = ViewState["WareHouseId"];
                    }
                    else
                    {
                        _arrpara[0].Value = Convert.ToInt32(txtid.Text);
                    }
                    _arrpara[1].Value = cmbCustomerCode.SelectedValue;
                    _arrpara[2].Value = txtWareHouseCode.Text.ToUpper();
                    _arrpara[3].Value = txtWareHouseName.Text.ToUpper();
                    _arrpara[4].Value = txtAddress.Text.ToUpper();
                    _arrpara[5].Value = txtCity.Text.ToUpper();
                    _arrpara[6].Value = txtState.Text.ToUpper();
                    _arrpara[7].Value = txtCountry.Text.ToUpper();
                    _arrpara[8].Value = Session["varuserid"].ToString();
                    _arrpara[9].Value = Session["varCompanyId"].ToString();
                    _arrpara[10].Value = txtConsignee.Text.ToUpper();
                    _arrpara[11].Value = txtBuyerOtherThanConsignee.Text.ToUpper();
                    _arrpara[12].Value = txtShipTo.Text.ToUpper();
                    _arrpara[13].Value = txtCountryFinalDestination.Text.ToUpper();
                    _arrpara[14].Value = DDPortOfLoading.SelectedIndex<0 ? "0" :DDPortOfLoading.SelectedValue;
                    _arrpara[15].Value = txtPortOfDischarge.Text.ToUpper();
                    _arrpara[16].Value = txtPlaceOfDelivery.Text.ToUpper();
                    _arrpara[17].Value = txtFinalDestination.Text.ToUpper();
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_WAREHOUSE", _arrpara);
                    Tran.Commit();
                    Label1.Visible = true;
                    Label1.Text = "Save Details............";
                    txtid.Text = "0";
                    cmbCustomerCode.SelectedIndex = -1;
                    txtWareHouseCode.Text = "";
                    txtWareHouseName.Text = "";
                    txtCity.Text = "";
                    txtState.Text = "";
                    txtCountry.Text = "";
                    txtAddress.Text = "";
                    txtConsignee.Text = "";
                    txtBuyerOtherThanConsignee.Text = "";
                    txtCountryFinalDestination.Text = "";
                    //txtPortOfLoading.Text = "";
                    DDPortOfLoading.SelectedIndex = 0;
                    txtPortOfDischarge.Text = "";
                    txtPlaceOfDelivery.Text = "";
                    txtFinalDestination.Text = "";
                    Fill_Grid();
                }
                catch (Exception ex)
                {
                    Label1.Visible = true;
                    Label1.Text = ex.Message;
                    Tran.Rollback();
                    Logs.WriteErrorLog("Masters_Campany_frmBank|cmdSave_Click|" + ex.Message);
                    ClientScript.RegisterStartupScript(this.GetType(), "onload", "onError();", true);
                    UtilityModule.MessageAlert(ex.Message, "Master/Campany/WareHouse.aspx");
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
                    if (Request.QueryString["id"] != null)
                    {
                        if (Request.QueryString["id"] == "1")
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "onload", "onSuccess();", true);
                        }
                    }
                }
            
        }
        else
        {
            if (Label1.Text == "Warehouse Name already exists............")
            {
                Label1.Visible = true;
                Label1.Text = "Warehouse Name already exists............";
            }
            else
            {
                Label1.Visible = true;
                Label1.Text = "Please Fill Details.............";
            }
        }
        btnSave.Text = "Save";
        btndelete.Visible = false;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {
            if (Request.QueryString["id"] == "1")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "onload", "onSuccess();", true);
            }
        }
        else
        {
            txtid.Text = "0";
            cmbCustomerCode.SelectedIndex = -1;
            txtWareHouseCode.Text = "";
            txtWareHouseName.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtCountry.Text = "";
            txtAddress.Text = "";
            btndelete.Visible = false;
            btnSave.Text = "Save";
        }
    }
    protected void gdWarehouse_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdWarehouse.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void uc1_sendMessageToThePage(object sender, MyEventArgs e)
    {
        //int customercode = e.Ecustomercode;
        // int warehousecode = e.Ewarehousecode;
        //int warehousename = e.Ewarehousename;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Session["ReportPath"] = "Reports/warehouse.rpt";
        Session["CommanFormula"] = "";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Priview();", true);
        //Report();
    }
    private void Report()
    {
        string qry = @"  SELECT CustomerName,CustomerCode,Warehouseid,Warehousecode,Warehousename,WarehouseMaster.Address,City,State,WarehouseMaster.Country
 FROM   customerinfo INNER JOIN WarehouseMaster ON customerinfo.CustomerId=WarehouseMaster.Customerid Where WarehouseMaster.MasterCompanyId=" + Session["varCompanyId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\warehouseNew.rpt";
            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\warehouseNew.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    private void Validated()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql;
            if (btnSave.Text == "Update")
            {
                strsql = "select Warehousecode,Warehousename from WarehouseMaster where Warehouseid !=" + ViewState["WareHouseId"] + " and  ( Warehousecode='" + txtWareHouseCode.Text + "' or Warehousename='" + txtWareHouseName.Text + "')  And MasterCompanyid=" + Session["varCompanyId"];
            }
            else
            {
                strsql = "select Warehousecode,Warehousename from WarehouseMaster where Warehousecode='" + txtWareHouseCode.Text + "' or Warehousename='" + txtWareHouseName.Text + "' And MasterCompanyid=" + Session["varCompanyId"];
            }
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Label1.Visible = true;
                Label1.Text = "Warehouse Name already exists............";
                txtWareHouseCode.Text = "";
                txtWareHouseCode.Focus();
            }
            else
            {
                Label1.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/WareHouse.aspx");
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
            SqlParameter[] _array = new SqlParameter[4];
            _array[0] = new SqlParameter("@WareHouseId", ViewState["WareHouseId"].ToString());
            _array[1] = new SqlParameter("@Message", SqlDbType.NVarChar, 100);
            _array[2] = new SqlParameter("@VarUserId", Session["varuserid"].ToString());
            _array[3] = new SqlParameter("@VarCompanyId", Session["varCompanyId"].ToString());
            _array[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteWareHouse", _array);
            Tran.Commit();
            Label1.Visible = true;
            Label1.Text = _array[1].Value.ToString();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            Label1.Visible = true;
            Label1.Text = ex.Message;            
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/WareHouse.aspx");

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
        btnSave.Text = "Save";
        txtid.Text = "0";
        cmbCustomerCode.SelectedIndex = -1;
        txtWareHouseCode.Text = "";
        txtWareHouseName.Text = "";
        txtCity.Text = "";
        txtState.Text = "";
        txtCountry.Text = "";
        txtAddress.Text = "";
    }
    protected void gdWarehouse_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        //if (e.Row.RowType == DataControlRowType.Header)
           // e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        //if (e.Row.RowType == DataControlRowType.DataRow &&
                //  e.Row.RowState == DataControlRowState.Alternate)
           // e.Row.CssClass = "alternate";
    }

    
}
