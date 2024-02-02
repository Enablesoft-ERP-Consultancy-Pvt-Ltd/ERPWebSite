using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_RawMaterial_GodownTransfer : System.Web.UI.Page
{
    int ItemFinishedId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        Fill_Grid();
        if (!IsPostBack)
        {
            string Qry = @"select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By Companyname
                         select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by CATEGORY_id
                        select godownid,godownname from godownmaster Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by godownid
                        select godownid,godownname from godownmaster Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by godownid";
            DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddlcompany, DSQ, 0, true, "Select Comp Name");

            if (ddlcompany.Items.Count > 0)
            {
                ddlcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddlcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddlcatagoryname, DSQ, 1, true, "Select Category");
            UtilityModule.ConditionalComboFillWithDS(ref ddfromgodown, DSQ, 2, true, "Select Godown");
            UtilityModule.ConditionalComboFillWithDS(ref ddtogodown, DSQ, 4, true, "Select Godown");
            lablechange();
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        lblsizename.Text = ParameterList[4];
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds;
        string Str;
        if (TxtProdCode.Text != "")
        {
            ddlcatagoryname.SelectedIndex = 0;
            Str = "select IPM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM where IPM.ITEM_ID=IM.ITEM_ID and ProductCode='" + TxtProdCode.Text + "' And IM.MasterCompanyId= " + Session["varMasterCompanyIDForERP"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string Qry = @"SELECT dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME  FROM  dbo.CategorySeparate INNER JOIN
               dbo.ITEM_CATEGORY_MASTER ON dbo.CategorySeparate.Categoryid = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID And ITEM_CATEGORY_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                Qry = Qry + "  Select Distinct Item_Id,Item_Name from Item_Master where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And  Category_Id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString()) + "";
                Qry = Qry + " select qualityid,qualityname from quality where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  And item_id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["ITEM_ID"].ToString());
                Qry = Qry + " select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order  by DesignName ";
                Qry = Qry + " SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                Qry = Qry + " select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by ShapeName ";
                Qry = Qry + "  SELECT SIZEID,SIZEFT fROM SIZE WhERE MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " ANd SHAPEID=" + Convert.ToInt32(ds.Tables[0].Rows[0]["SHAPE_ID"].ToString());
                DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
                UtilityModule.ConditionalComboFillWithDS(ref ddlcatagoryname, DSQ, 0, true, "Select Catagory");
                ddlcatagoryname.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddlitemname, DSQ, 1, true, "--Select Item--");
                ddlitemname.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref dquality, DSQ, 2, true, "Select Quallity");
                dquality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref dddesign, DSQ, 3, true, "Select Design");
                dddesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddcolor, DSQ, 4, true, "--Select Color--");
                ddcolor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddshape, DSQ, 5, true, "--Select Shape--");
                ddshape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddsize, DSQ, 6, true, "--SELECT SIZE--");
                ddsize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                Session["finishedid"] = ds.Tables[0].Rows[0]["Item_Finished_id"].ToString();
                if (Convert.ToInt32(dquality.SelectedValue) > 0)
                {
                    ql.Visible = true;
                }
                else
                {
                    ql.Visible = false;
                }
                if (Convert.ToInt32(dddesign.SelectedValue) > 0)
                {
                    dsn.Visible = true;
                }
                else
                {
                    dsn.Visible = false;
                }
                int c = (ddcolor.SelectedIndex > 0 ? Convert.ToInt32(ddcolor.SelectedValue) : 0);
                if (c > 0)
                {
                    clr.Visible = true;
                }
                else
                {
                    clr.Visible = false;
                }
                int s = (ddshape.SelectedIndex > 0 ? Convert.ToInt32(ddshape.SelectedValue) : 0);
                if (s > 0)
                {
                    shp.Visible = true;
                }
                else
                {
                    shp.Visible = false;
                }
                int si = (ddsize.SelectedIndex > 0 ? Convert.ToInt32(ddsize.SelectedValue) : 0);
                if (si > 0)
                {
                    sz.Visible = true;
                }
                else
                {
                    sz.Visible = false;
                }
                UtilityModule.ConditionalComboFill(ref ddlunit, "SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + ddlitemname.SelectedValue + " And i.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Unit");
                lblcode.Visible = false;
            }
        }
        else
        {
            lblcode.Visible = true;
            TxtProdCode.Focus();
        }
    }
    protected void ddlcatagoryname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
        UtilityModule.ConditionalComboFill(ref ddlitemname, "Select Distinct Item_Id,Item_Name from Item_Master where Category_Id=" + ddlcatagoryname.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select Item--");
    }
    private void ddlcategorycange()
    {
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                      " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                      " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddlcatagoryname.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        ql.Visible = true;
                        UtilityModule.ConditionalComboFill(ref dquality, "select Qualityid,Qualityname from Quality Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select Quality--");
                        break;
                    case "2":
                        clr.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddcolor, "SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select Color--");
                        break;
                    case "3":
                        dsn.Visible = true;
                        UtilityModule.ConditionalComboFill(ref dddesign, "select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  Order  by DesignName ", true, "Select Design");
                        break;
                    case "4":
                        shp.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddshape, "select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by ShapeName", true, "--Select Shape--");
                        break;
                    case "5":
                        sz.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid ", true, "Size in Ft");
                        break;
                }
            }
        }
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlunit.SelectedValue == "1")
        {
            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizemtr from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "select size");
        }
        else if (ddlunit.SelectedValue == "2")
        {
            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "select size");
        }
    }
    protected void ddlitemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Qry = @"SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + ddlitemname.SelectedValue + "  And i.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                     select qualityid,qualityname from quality where item_id=" + ddlitemname.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
        UtilityModule.ConditionalComboFillWithDS(ref ddlunit, DSQ, 0, true, "Select Unit");
        UtilityModule.ConditionalComboFillWithDS(ref dquality, DSQ, 1, true, "Select Quallity");
    }
    private void returnfinishedid()
    {
        ItemFinishedId = Convert.ToInt32(UtilityModule.getItemFinishedId(ddlitemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddShadeColor, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"])));
    }
    protected void ddfromgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        returnfinishedid();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        int quantity = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select qtyinhand from stock where ITEM_FINISHED_ID=" + ItemFinishedId + "and godownid=" + ddfromgodown.SelectedValue + "and companyid=" + ddlcompany.SelectedValue));
        Session["inhand"] = quantity;
        txtqty.Text = quantity.ToString();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        raw_stock_update();
        if (lblqty.Visible != true)
        {
            TxtProdCode.Text = "";
            ddlcatagoryname.SelectedValue = null;
            ddlitemname.SelectedValue = null;
            ddlunit.SelectedValue = null;
            txtdate.Text = "";
            dquality.SelectedValue = null;
            dddesign.SelectedValue = null;
            ddcolor.SelectedValue = null;
            ddshape.SelectedValue = null;
            ddsize.SelectedValue = null;
            ddfromgodown.SelectedValue = null;
            ddtogodown.SelectedValue = null;
            txtqty.Text = "";
        }
        Fill_Grid();
    }
    private void raw_stock_update()
    {
        try
        {
            returnfinishedid();
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            double inhand = Convert.ToDouble(SqlHelper.ExecuteScalar(con, CommandType.Text, "select qtyinhand from stock where ITEM_FINISHED_ID=" + ItemFinishedId + "and godownid=" + ddfromgodown.SelectedValue + "and companyid=" + ddlcompany.SelectedValue));
            double trqty = Convert.ToDouble(txtqty.Text);
            if (trqty <= inhand)
            {
                string strstock = "select * from stock where ITEM_FINISHED_ID=" + ItemFinishedId + "and companyid=" + ddlcompany.SelectedValue + "and Godownid=" + ddtogodown.SelectedValue;
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strstock);
                int gdtranid = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select isnull(max(gdtranid),0)+1 from godowntran"));
                int gtpassid = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select isnull(max(gatepassid),0)+1 from gatepass"));
                DateTime date = DateTime.Now;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int gdstockid = Convert.ToInt32(ds.Tables[0].Rows[0]["stockid"].ToString());
                    string gdtran = "insert into godowntran(gdtranid,stockid,companyid,fromgodownid,togodownid,quantity,RecDate)values(" + gdtranid + "," + gdstockid + "," + ddlcompany.SelectedValue + "," + ddfromgodown.SelectedValue + "," + ddtogodown.SelectedValue + "," + txtqty.Text + ",'" + date + "')";
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, gdtran);
                    string gatepass = "insert into gatepass(gatepassid,gatepassno,TableName,recno)values(" + gtpassid + "," + gdtranid + ", 'Godowntran',0)";
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, gatepass);
                    string strupdate = "update stock set Qtyinhand=(Qtyinhand+" + Convert.ToDouble(txtqty.Text) + ")where ITEM_FINISHED_ID=" + ItemFinishedId + "and companyid=" + ddlcompany.SelectedValue + "and Godownid=" + ddtogodown.SelectedValue;
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, strupdate);
                    string strupdate2 = "update stock set Qtyinhand=(Qtyinhand-" + Convert.ToDouble(txtqty.Text) + ")where ITEM_FINISHED_ID=" + ItemFinishedId + "and companyid=" + ddlcompany.SelectedValue + "and Godownid=" + ddfromgodown.SelectedValue;
                    DataSet dt1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                    SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt1.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'gatepass'," + gtpassid + ",getdate(),'Insert')");
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, strupdate2);
                }
                else
                {
                    string strupdate3 = "update stock set Qtyinhand=(Qtyinhand-" + Convert.ToDouble(txtqty.Text) + ")where ITEM_FINISHED_ID=" + ItemFinishedId + "and companyid=" + ddlcompany.SelectedValue + "and Godownid=" + ddfromgodown.SelectedValue;
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, strupdate3);
                    int VarStockid = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select isnull(max(stockid),0)+1 from stock"));
                    string strinsert = "insert into stock( StockID,ITEM_FINISHED_ID,Qtyinhand,QtyAssigned,OpenStock,Godownid,companyid,Price,LotNo,typeid )values(" + VarStockid + "," + ItemFinishedId + "," + Convert.ToDouble(txtqty.Text) + ",0,0," + ddtogodown.SelectedValue + "," + ddlcompany.SelectedValue + ",0,0,1)";
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, strinsert);
                    string gdtran2 = "insert into godowntran(gdtranid,stockid,companyid,fromgodownid,togodownid,quantity,RecDate)values(" + gdtranid + "," + VarStockid + "," + ddlcompany.SelectedValue + "," + ddfromgodown.SelectedValue + "," + ddtogodown.SelectedValue + "," + txtqty.Text + ",'" + date + "')";
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, gdtran2);
                }
                int stockid = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select isnull(max(stockid),0) from stock"));
                int stocktranid = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select isnull(max(StockTranid),0)+1 from StockTran"));
                string insert2 = "insert into stocktran(Stockid,StockTranid,trantype,quantity,TranDate,Userid,RealDate,TableName,PRTid) values(" + stockid + "," + stocktranid + ",0," + Convert.ToDouble(txtqty.Text) + ",'" + date + "',0,'" + date + "','Godowntran'," + gdtranid + ")";
                SqlHelper.ExecuteNonQuery(con, CommandType.Text, insert2);
                lblqty.Visible = false;
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'godowntran'," + gdtranid + ",getdate(),'Insert')");


            }
            else
            {
                lblqty.Visible = true;
                txtqty.Focus();
                txtqty.Text = inhand.ToString();
            }
        }

        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/GodownTransfer.aspx");
        }
    }
    private void Fill_Grid()
    {
        gvdetail.DataSource = fill_Data_grid();
        gvdetail.DataBind();
    }
    private DataSet fill_Data_grid()
    {
        DataSet ds = null;
        try
        {
            string strsql = @"SELECT DISTINCT dbo.GodownTran.gdtranid,
                           dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME, dbo.ITEM_MASTER.ITEM_NAME, dbo.ITEM_PARAMETER_MASTER.DESCRIPTION, 
                           dbo.GodownMaster.GodownName, dbo.stock.Qtyinhand, dbo.GodownTran.Quantity FROM dbo.ITEM_CATEGORY_MASTER INNER JOIN
                           dbo.ITEM_MASTER INNER JOIN dbo.ITEM_PARAMETER_MASTER INNER JOIN dbo.GodownMaster INNER JOIN dbo.stock INNER JOIN
                           dbo.GodownTran ON dbo.stock.StockID = dbo.GodownTran.Stockid ON dbo.GodownMaster.GoDownID = dbo.GodownTran.ToGodownid ON 
                           dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.stock.ITEM_FINISHED_ID ON 
                           dbo.ITEM_MASTER.ITEM_ID = dbo.ITEM_PARAMETER_MASTER.ITEM_ID And ITEM_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" ON 
                           dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID = dbo.ITEM_MASTER.CATEGORY_ID order by dbo.GodownTran.gdtranid";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/GodownTransfer.aspx");
            Logs.WriteErrorLog("Masters_RawMeterial_GodownTransfer|fill_Data_grid|" + ex.Message);
        }
        return ds;
    }
    protected void gvdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvdetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gvdetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvdetail.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void gvdetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = null;
        try
        {
            ds = null;
            string sql = @"SELECT DISTINCT dbo.GodownTran.gdtranid,dbo.GodownTran.stockid,dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID, dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID,dbo.GodownTran.companyid, dbo.ITEM_PARAMETER_MASTER.ProductCode, dbo.ITEM_MASTER.CATEGORY_ID, dbo.ITEM_MASTER.ITEM_ID, 
                         dbo.Unit.UnitId, dbo.ITEM_PARAMETER_MASTER.QUALITY_ID, dbo.ITEM_PARAMETER_MASTER.DESIGN_ID, 
                         dbo.ITEM_PARAMETER_MASTER.COLOR_ID, dbo.ITEM_PARAMETER_MASTER.SHAPE_ID, dbo.ITEM_PARAMETER_MASTER.SIZE_ID, 
                         dbo.GodownTran.FromGodownid, dbo.GodownTran.ToGodownid, dbo.GodownTran.Quantity, dbo.GodownTran.RecDate
                         FROM  dbo.ITEM_MASTER INNER JOIN  dbo.ITEM_PARAMETER_MASTER INNER JOIN dbo.GodownMaster INNER JOIN
                         dbo.stock INNER JOIN  dbo.GodownTran ON dbo.stock.StockID = dbo.GodownTran.Stockid ON dbo.GodownMaster.GoDownID = dbo.GodownTran.ToGodownid ON 
                         dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.stock.ITEM_FINISHED_ID ON 
                         dbo.ITEM_MASTER.ITEM_ID = dbo.ITEM_PARAMETER_MASTER.ITEM_ID INNER JOIN
                         dbo.Unit ON dbo.ITEM_MASTER.UnitTypeID = dbo.Unit.UnitTypeID where dbo.GodownTran.gdtranid=" + gvdetail.SelectedValue + " And companyid = " + ddlcompany.SelectedValue + " And ITEM_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);

            txtgdid.Text = ds.Tables[0].Rows[0]["gdtranid"].ToString();
            string Qry = @"select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by CATEGORY_id
                           Select Distinct Item_Id,Item_Name from Item_Master where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " ANd Category_Id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["category_id"].ToString()) + @"
                           SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where i.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And  item_id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["item_id"].ToString()) + @"
                           select qualityid,qualityname from quality where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  And item_id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["item_id"].ToString()) + @"
                           select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order  by DesignName
                           SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                           select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Shapeid";
            DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddlcatagoryname, DSQ, 0, true, "Select Category");
            ddlcatagoryname.SelectedValue = ds.Tables[0].Rows[0]["category_id"].ToString();
            UtilityModule.ConditionalComboFillWithDS(ref ddlitemname, DSQ, 1, true, "--select Item--");
            ddlitemname.SelectedValue = ds.Tables[0].Rows[0]["item_id"].ToString();
            UtilityModule.ConditionalComboFillWithDS(ref ddlunit, DSQ, 2, true, "Select Unit");
            UtilityModule.ConditionalComboFillWithDS(ref dquality, DSQ, 3, true, "Select Quallity");
            dquality.SelectedValue = ds.Tables[0].Rows[0]["quality_id"].ToString();
            ddlunit.SelectedValue = ds.Tables[0].Rows[0]["unitid"].ToString();
            UtilityModule.ConditionalComboFillWithDS(ref dddesign, DSQ, 4, true, "--Select Design--");
            UtilityModule.ConditionalComboFillWithDS(ref ddcolor, DSQ, 5, true, "--Select Color--");
            UtilityModule.ConditionalComboFillWithDS(ref ddshape, DSQ, 6, true, "--Select Shape--");

            dddesign.SelectedValue = ds.Tables[0].Rows[0]["design_id"].ToString();
            ddcolor.SelectedValue = ds.Tables[0].Rows[0]["color_id"].ToString();
            ddshape.SelectedValue = ds.Tables[0].Rows[0]["shape_id"].ToString();
            TxtProdCode.Text = ds.Tables[0].Rows[0]["productcode"].ToString();
            txtdate.Text = ds.Tables[0].Rows[0]["RecDate"].ToString();
            Session["stockid"] = ds.Tables[0].Rows[0]["stockid"].ToString();
            Session["finishedid"] = ds.Tables[0].Rows[0]["item_finished_id"].ToString();
            if (ddlunit.SelectedValue == "1")
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizemtr from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "select size");
            }
            else if (ddlunit.SelectedValue == "2")
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "select size");
            }
            ddfromgodown.SelectedValue = ds.Tables[0].Rows[0]["fromgodownid"].ToString();
            ddtogodown.SelectedValue = ds.Tables[0].Rows[0]["togodownid"].ToString();
            txtqty.Text = ds.Tables[0].Rows[0]["quantity"].ToString();
            Session["quantity"] = ds.Tables[0].Rows[0]["quantity"].ToString();
            if (Convert.ToInt32(dquality.SelectedValue) > 0)
            {
                ql.Visible = true;
            }
            else
            {
                ql.Visible = false;
            }
            if (Convert.ToInt32(dddesign.SelectedValue) > 0)
            {
                dsn.Visible = true;
            }
            else
            {
                dsn.Visible = false;
            }
            int c = (ddcolor.SelectedIndex > 0 ? Convert.ToInt32(ddcolor.SelectedValue) : 0);
            if (c > 0)
            {
                clr.Visible = true;
            }
            else
            {
                clr.Visible = false;
            }
            int s = (ddshape.SelectedIndex > 0 ? Convert.ToInt32(ddshape.SelectedValue) : 0);
            if (s > 0)
            {
                shp.Visible = true;
            }
            else
            {
                shp.Visible = false;
            }
            int si = (ddsize.SelectedIndex > 0 ? Convert.ToInt32(ddsize.SelectedValue) : 0);
            if (si > 0)
            {
                sz.Visible = true;
            }
            else
            {
                sz.Visible = false;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/GodownTransfer.aspx");
        }
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            returnfinishedid();
            DateTime date = DateTime.Now;
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            string strstock = "select * from stock where ITEM_FINISHED_ID=" + ItemFinishedId + "and companyid=" + ddlcompany.SelectedValue + "and Godownid=" + ddtogodown.SelectedValue;
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strstock);
            int gdstockid = Convert.ToInt32(ds.Tables[0].Rows[0]["stockid"].ToString());
            string gdtran = "delete from godowntran where gdtranid=" + txtgdid.Text;
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, gdtran);
            string strupdate = "update stock set Qtyinhand=(Qtyinhand-" + Convert.ToDouble(txtqty.Text) + ")where ITEM_FINISHED_ID=" + ItemFinishedId + "and companyid=" + ddlcompany.SelectedValue + "and Godownid=" + ddtogodown.SelectedValue;
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, strupdate);
            string strupdate2 = "update stock set Qtyinhand=(Qtyinhand+" + Convert.ToDouble(txtqty.Text) + ")where ITEM_FINISHED_ID=" + ItemFinishedId + "and companyid=" + ddlcompany.SelectedValue + "and Godownid=" + ddfromgodown.SelectedValue;
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, strupdate2);
            string insert2 = "delete from stocktran where stockid=" + gdstockid + "and tablename='godowntran'and prtid=" + txtgdid.Text;
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, insert2);
            DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
            SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'godowntran'," + txtgdid.Text + ",getdate(),'Delete')");
            lblqty.Visible = false;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/GodownTransfer.aspx");
        }

        TxtProdCode.Text = "";
        ddlcatagoryname.SelectedValue = null;
        ddlitemname.SelectedValue = null;
        ddlunit.SelectedValue = null;
        txtdate.Text = "";
        dquality.SelectedValue = null;
        dddesign.SelectedValue = null;
        ddcolor.SelectedValue = null;
        ddshape.SelectedValue = null;
        ddsize.SelectedValue = null;
        ddfromgodown.SelectedValue = null;
        ddtogodown.SelectedValue = null;
        txtqty.Text = "";
        txtgdid.Text = "";
        Fill_Grid();
    }
}