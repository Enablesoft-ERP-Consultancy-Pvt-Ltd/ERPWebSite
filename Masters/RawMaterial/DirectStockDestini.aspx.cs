using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_process_itemRecieve : System.Web.UI.Page
{
    int ItemFinishedId = 0;
    string remainstock = null;
    static int MasterCompanyId;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varMasterCompanyIDForERP"]);
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        txtstockid.Text = "0";
        if (!IsPostBack)
        {
            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            string str = @"select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By Companyname 
                    Select godownid,godownname from godownmaster Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"      
                    Select VarProdCode from MasterSetting " + @"
                    SELECT ID,FINISHED_TYPE_NAME FROM FINISHED_TYPE where  MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + "  ORDER BY FINISHED_TYPE_NAME";
            DataSet ds = SqlHelper.ExecuteDataset(str);

            UtilityModule.ConditionalComboFillWithDS(ref ddlcompany, ds, 0, true, "Select Comp Name");
            UtilityModule.ConditionalComboFillWithDS(ref ddlgodown, ds, 1, true, "Select Godown");
            UtilityModule.ConditionalComboFillWithDS(ref ddFINISHED_TYPE, ds, 3, true, "--Select--");
            
            if (ddlcompany.Items.Count > 0)
            {
                ddlcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddlcompany.Enabled = false;
            }

            int VarProdCode = Convert.ToInt32(ds.Tables[2].Rows[0][0].ToString());
            if (VarProdCode == 1)
            {
                TdFINISHED_TYPE.Visible = true;
                code.Visible = true;
            }
            else
            {
                code.Visible = false;
            }
            lablechange();
            Session["ReportPath"] = "reports/openstock.rpt";
            //Session["CommanFormula"] = "{openstock.StockID}="+txtstockid.Text.Trim();
            Session["CommanFormula"] = "";
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
        lblshadecolor.Text = ParameterList[7];
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
        shd.Visible = false;
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
                        UtilityModule.ConditionalComboFill(ref dddesign, "select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order  by DesignName ", true, "Select Design");
                        break;
                    case "4":
                        shp.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddshape, "select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by ShapeName", true, "--Select Shape--");
                        break;
                    case "5":
                        sz.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid ", true, "Size in Ft");
                        break;
                    case "6":
                        shd.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddlshade, "select shadecolorid,shadecolorname from shadecolor Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "Select Shadecolor");
                        break;
                }
            }
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (txtopeningstock.Text != "")
        {
            ItemFinishedId = UtilityModule.getItemFinishedId(ddlitemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            if (ddlcatagorytype.SelectedValue == "1")
            {
                saverawdetail();
            }
            else
            {
                savecarpetdetail();
            }
            lblsave.Visible = false;
            fill_grid();
            txtopeningstock.Text = "";
            dquality.SelectedValue = null;
            dddesign.SelectedValue = null;
            ddcolor.SelectedValue = null;
            ddshape.SelectedValue = null;
            ddsize.SelectedValue = null;
            TxtProdCode.Text = "";
            txtlotno.Text = "";
            lblerror.Visible = false;
            lblmessage.Visible = false;
            txtprefix.Visible = false;
            Txtpostfix.Visible = false;
            btnsave.Text = "Save";
            ddlgodown.Enabled = true;
            ddlunit.SelectedValue = null;
        }
        else
        {
            lblsave.Visible = true;
        }
    }
    private void saverawdetail()
    {
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            ItemFinishedId = UtilityModule.getItemFinishedId(ddlitemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            string strstock = "select * from stock where ITEM_FINISHED_ID=" + ItemFinishedId + "and companyid=" + ddlcompany.SelectedValue + "and Godownid=" + ddlgodown.SelectedValue + "and lotno='" + txtlotno.Text + "'   And  FINISHED_TYPE_ID=" + ddFINISHED_TYPE.SelectedValue + "";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strstock);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int openstock = Convert.ToInt32(ds.Tables[0].Rows[0]["openstock"].ToString());
                if (ds.Tables[0].Rows[0]["openstock"].ToString() == null)
                {
                    string strupdate = "update stock set Qtyinhand=Qtyinhand+" + txtopeningstock.Text + ", OpenStock=" + Convert.ToDouble(txtopeningstock.Text) + " where ITEM_FINISHED_ID=" + ItemFinishedId + "and companyid=" + ddlcompany.SelectedValue + "and Godownid=" + ddlgodown.SelectedValue + "And FINISHED_TYPE_ID=" + ddFINISHED_TYPE.SelectedValue + "";
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, strupdate);
                }
                else
                {
                    string strupdate = "update stock set Qtyinhand=Qtyinhand-" + openstock + "+" + txtopeningstock.Text + ", OpenStock=" + Convert.ToDouble(txtopeningstock.Text) + " where ITEM_FINISHED_ID=" + ItemFinishedId + "and companyid=" + ddlcompany.SelectedValue + "and Godownid=" + ddlgodown.SelectedValue + "And FINISHED_TYPE_ID=" + ddFINISHED_TYPE.SelectedValue + "";
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, strupdate);
                }
            }
            else
            {
                int VarStockid = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select isnull(max(stockid),0)+1 from stock"));
                string strinsert = "insert into stock( StockID,ITEM_FINISHED_ID,Qtyinhand,QtyAssigned,OpenStock,Godownid,companyid,Price,LotNo,TypeId, FINISHED_TYPE_ID )values(" + VarStockid + "," + ItemFinishedId + "," + Convert.ToDouble(txtopeningstock.Text) + ",0," + Convert.ToDouble(txtopeningstock.Text) + "," + ddlgodown.SelectedValue + "," + ddlcompany.SelectedValue + ",0,'" + txtlotno.Text.ToString() + "'," + ddlcatagorytype.SelectedValue + "," + ddFINISHED_TYPE.SelectedValue + ")";
                SqlHelper.ExecuteNonQuery(con, CommandType.Text, strinsert);
            }
            int stockid = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select isnull(max(stockid),0) from stock"));
            DateTime date = DateTime.Now;
            int stocktranid = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select isnull(max(StockTranid),0)+1 from StockTran"));
            //int prt = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select isnull(max(StockNo),0)+1 from CarpetNumber"));
            if (btnsave.Text == "Update")
            {
                string Qry = "delete from stocktran where stockid =" + Convert.ToInt32(Session["stockid"].ToString()) + " and stocktranid=" + Convert.ToInt32(Session["stocktranid"].ToString());
                Qry = Qry + "      insert into stocktran(Stockid,StockTranid,trantype,quantity,TranDate,Userid,RealDate,TableName,PRTid)values(" + Convert.ToInt32(Session["stockid"].ToString()) + "," + Convert.ToInt32(Session["stocktranid"].ToString()) + ",1," + Convert.ToDouble(txtopeningstock.Text) + ",'" + date + "',0,'" + date + "','Opening stock',0)";
                SqlHelper.ExecuteNonQuery(con, CommandType.Text, Qry);

            }
            else
            {
                string insert2 = "insert into stocktran(Stockid,StockTranid,trantype,quantity,TranDate,Userid,RealDate,TableName,PRTid)values(" + stockid + "," + stocktranid + ",1," + Convert.ToDouble(txtopeningstock.Text) + ",'" + date + "',0,'" + date + "','Opening stock',0)";
                SqlHelper.ExecuteNonQuery(con, CommandType.Text, insert2);
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/DirectStockDestini.aspx");
            lblmessage.Visible = true;
            lblmessage.Text = ex.Message;
        }
    }
    private void savecarpetdetail()
    {
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            int VarOpeningQty = Convert.ToInt32(txtopeningstock.Text);
            int a = 0;
            for (int i = 1; i <= VarOpeningQty; i++)
            {
                string VarTStockNoCheck = txtprefix.Text + Txtpostfix.Text;
                DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select * from CarpetNumber Where TstockNo='" + VarTStockNoCheck + "'");
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    a = 1;
                }
            }
            if (a == 0)
            {
                if (btnsave.Text == "Update")
                {
                    saverawdetail2();
                }
                else
                {
                    saverawdetail();
                }
                ItemFinishedId = UtilityModule.getItemFinishedId(ddlitemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                string VarTStockNo = txtprefix.Text + Txtpostfix.Text;
                int VarTStockNo1 = Convert.ToInt32(Txtpostfix.Text == "" ? "0" : Txtpostfix.Text);
                SqlParameter[] arr = new SqlParameter[7];
                arr[0] = new SqlParameter("@finishedid", SqlDbType.Int);
                arr[1] = new SqlParameter("@companyid", SqlDbType.Int);
                arr[2] = new SqlParameter("@TStockNo", SqlDbType.NVarChar);
                arr[3] = new SqlParameter("@TStockNo1", SqlDbType.Int);
                arr[4] = new SqlParameter("@prefix", SqlDbType.NVarChar);
                arr[5] = new SqlParameter("@recdate", SqlDbType.DateTime);
                arr[6] = new SqlParameter("@stock", SqlDbType.Int);
                arr[0].Value = ItemFinishedId;
                arr[1].Value = ddlcompany.SelectedValue;
                arr[2].Value = VarTStockNo;
                arr[3].Value = VarTStockNo1;
                arr[4].Value = txtprefix.Text;
                arr[5].Value = DateTime.Now;
                arr[6].Value = txtstockid.Text;
                for (int i = 1; i <= VarOpeningQty; i++)
                {
                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "[PRO_directstock]", arr);
                    Txtpostfix.Text = Convert.ToString(VarTStockNo1 + 1);
                    VarTStockNo = txtprefix.Text + Txtpostfix.Text;
                    VarTStockNo1 = Convert.ToInt32(Txtpostfix.Text);
                    arr[2].Value = VarTStockNo;
                    arr[3].Value = VarTStockNo1;
                }
                lblerror.Visible = false;
            }
            else
            {
                lblerror.Visible = true;
                lblmessage.Visible = false;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/DirectStockDestini.aspx");
            lblmessage.Visible = true;
            lblmessage.Text = ex.Message;
        }
    }
    protected void ddlcatagorytype_SelectedIndexChanged(object sender, EventArgs e)
    {
        CategoryTypeSelectedChange();
    }
    private void CategoryTypeSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref ddlcatagoryname, @"SELECT dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME  FROM  dbo.CategorySeparate INNER JOIN
        dbo.ITEM_CATEGORY_MASTER ON dbo.CategorySeparate.Categoryid = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID
        WHERE dbo.CategorySeparate.id =" + ddlcatagorytype.SelectedValue + " And ITEM_CATEGORY_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Catagory");
        if (ddlcatagorytype.SelectedValue == "0")
        {
            Txtpostfix.Visible = true;
            txtprefix.Visible = true;
            txtprefix.Text = "";
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            string postfix = (SqlHelper.ExecuteScalar(con, CommandType.Text, "sELECT IsNull(Max(Postfix),0)+1 from CarpetNumber Where prefix Like '%'")).ToString();
            Txtpostfix.Text = postfix;
        }
        else
        {
            Txtpostfix.Visible = false;
            txtprefix.Visible = false;
        }
        fill_grid();
    }
    protected void txtprefix_TextChanged(object sender, EventArgs e)
    {
        Txtpostfix.Text = UtilityModule.CalculatePostFix(txtprefix.Text).ToString();
    }
    private void fill_grid()
    {
        gvcarpetdetail.DataSource = fill_Data_grid();
        gvcarpetdetail.DataBind();
    }
    private DataSet fill_Data_grid()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            int category = Convert.ToInt32(ddlcatagoryname.SelectedIndex);
            int item = ddlitemname.SelectedIndex > 0 ? Convert.ToInt32(ddlitemname.SelectedValue) : 0;
            int quality = dquality.SelectedIndex > 0 ? Convert.ToInt32(dquality.SelectedValue) : 0;
            int design = dddesign.SelectedIndex > 0 ? Convert.ToInt32(dddesign.SelectedValue) : 0;
            int color = ddcolor.SelectedIndex > 0 ? Convert.ToInt32(ddcolor.SelectedValue) : 0;
            int shape = ddshape.SelectedIndex > 0 ? Convert.ToInt32(ddshape.SelectedValue) : 0;
            int size = ddsize.SelectedIndex > 0 ? Convert.ToInt32(ddsize.SelectedValue) : 0;
            int ShadeColor = ddlshade.SelectedIndex > 0 ? Convert.ToInt32(ddlshade.SelectedValue) : 0;
            string str1, str2, str3, str4, str5, str6, str7;
            if (item > 0)
            {
                str1 = "and VF.Item_Id=" + item;
            }
            else
            {
                str1 = "";
            }
            if (quality > 0)
            {
                str2 = " and VF.QualityId=" + quality;
            }
            else
            {
                str2 = "";
            }
            if (design > 0)
            {
                str3 = " and VF.DesignId=" + design;
            }
            else
            {
                str3 = "";
            }
            if (color > 0)
            {
                str4 = " and VF.ColorId=" + color;
            }
            else
            {
                str4 = "";
            }
            if (shape > 0)
            {
                str5 = " and VF.ShapeId=" + shape;
            }
            else
            {
                str5 = "";
            }
            if (size > 0)
            {
                str6 = "and VF.SizeId=" + size;
            }
            else
            {
                str6 = "";
            }
            if (ShadeColor > 0)
            {
                str7 = "and VF.ShadecolorId=" + ShadeColor;
            }
            else
            {
                str7 = "";
            }
            string strsql1 = @"SELECT S.STOCKID,VF.CATEGORY_NAME,VF.ITEM_NAME,VF.QUALITYNAME + Space(2)+DESIGNNAME + Space(2)+COLORNAME + Space(2)+SHAPENAME+ Space(2)+
                             SIZEMTR+ Space(4)+FT.FINISHED_TYPE_NAME DESCRIPTION,S.OPENSTOCK,S.QTYINHAND ,g.GodownName as godown
                             FROM V_FINISHEDITEMDETAIL VF,STOCK S LEFT OUTER JOIN FINISHED_TYPE FT ON S.FINISHED_TYPE_ID=FT.ID inner join 
                             GodownMaster g On g.GoDownID=s.Godownid WHERE VF.ITEM_FINISHED_ID=S.ITEM_FINISHED_ID AND S.QTYINHAND<>0 AND VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" And 
                             S.TYPEID=" + ddlcatagorytype.SelectedValue + " " + str1 + " " + str2 + " " + str3 + " " + str4 + " " + str5 + " " + str6 + " " + str7 + " ORDER BY VF.CATEGORY_NAME,VF.ITEM_NAME";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql1);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/DirectStockDestini.aspx");
            Logs.WriteErrorLog("Masters_process_DerictStock|Fill_Grid_Data|" + ex.Message);
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
    protected void gvcarpetdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvcarpetdetail, "Select$" + e.Row.RowIndex);
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
        fill_grid();
    }
    protected void ddlitemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlitemname.SelectedIndex > 0)
        {
            txtprefix.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Item_Code from Item_Master Where Item_Id=" + ddlitemname.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"]).ToString();
            string get_year = DateTime.Now.ToString("dd-MMM-yyyy");
            string lastTwoChars = get_year.Substring(get_year.Length - 2);
            txtprefix.Text = (txtprefix.Text + "-" + lastTwoChars).Replace(" ", "");
        }
        else
        {
            txtprefix.Text = "";
        }
        Txtpostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix((txtprefix.Text).ToUpper()));
        UtilityModule.ConditionalComboFill(ref ddlunit, "SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + ddlitemname.SelectedValue + " And i.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Unit");
        UtilityModule.ConditionalComboFill(ref dquality, "select qualityid,qualityname from quality where item_id=" + ddlitemname.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Quallity");
        fill_grid();
    }
    private void saverawdetail2()
    {
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            ItemFinishedId = UtilityModule.getItemFinishedId(ddlitemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            string strstock = "select * from stock where ITEM_FINISHED_ID=" + ItemFinishedId + "and companyid=" + ddlcompany.SelectedValue + "and Godownid=" + ddlgodown.SelectedValue + "and lotno='" + txtlotno.Text + "' And  FINISHED_TYPE_ID=" + ddFINISHED_TYPE.SelectedValue + "";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strstock);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string Qry = "update stock set Qtyinhand=" + txtopeningstock.Text + ", OpenStock=" + Convert.ToDouble(txtopeningstock.Text) + " where ITEM_FINISHED_ID=" + ItemFinishedId + "and companyid=" + ddlcompany.SelectedValue + "and Godownid=" + ddlgodown.SelectedValue + " And FINISHED_TYPE_ID=" + ddFINISHED_TYPE.SelectedValue + "";
                Qry = Qry + "     delete from carpetnumber where item_finished_id=" + ItemFinishedId;
                SqlHelper.ExecuteNonQuery(con, CommandType.Text, Qry);
                string postfix = (SqlHelper.ExecuteScalar(con, CommandType.Text, "sELECT IsNull(Max(Postfix),0)+1 from CarpetNumber Where prefix Like '" + txtprefix.Text + "%'")).ToString();
                Txtpostfix.Text = postfix;
            }
            else
            {
                int VarStockid = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select isnull(max(stockid),0)+1 from stock"));
                string strinsert = "insert into stock( StockID,ITEM_FINISHED_ID,Qtyinhand,QtyAssigned,OpenStock,Godownid,companyid,Price,LotNo,TypeId,FINISHED_TYPE_ID )values(" + VarStockid + "," + ItemFinishedId + "," + Convert.ToDouble(txtopeningstock.Text) + ",0," + Convert.ToDouble(txtopeningstock.Text) + "," + ddlgodown.SelectedValue + "," + ddlcompany.SelectedValue + ",0,0," + ddlcatagorytype.SelectedValue + "," + ddFINISHED_TYPE.SelectedValue + ")";
                SqlHelper.ExecuteNonQuery(con, CommandType.Text, strinsert);
            }
            string qry1 = @"select isnull(max(stockid),0) from stock
            select isnull(max(StockTranid),0)+1 from StockTran
            select isnull(max(StockNo),0)+1 from CarpetNumber";
            DataSet ds1 = SqlHelper.ExecuteDataset(qry1);
            int stockid = Convert.ToInt32(ds1.Tables[0].Rows[0][0].ToString());
            DateTime date = DateTime.Now;
            int stocktranid = Convert.ToInt32(ds1.Tables[1].Rows[0][0].ToString());
            int prt = Convert.ToInt32(ds1.Tables[0].Rows[2][0].ToString());
            if (btnsave.Text == "Update")
            {
                string delete = "delete from stocktran where stockid =" + Convert.ToInt32(Session["stockid"].ToString()) + " and stocktranid=" + Convert.ToInt32(Session["stocktranid"].ToString());
                delete = delete + "   insert into stocktran(Stockid,StockTranid,trantype,quantity,TranDate,Userid,RealDate,TableName,PRTid)values(" + Convert.ToInt32(Session["stockid"].ToString()) + "," + Convert.ToInt32(Session["stocktranid"].ToString()) + ",1," + Convert.ToDouble(txtopeningstock.Text) + ",'" + date + "',0,'" + date + "','Opening stock'," + prt + ")";
                SqlHelper.ExecuteNonQuery(con, CommandType.Text, delete);
            }

            else
            {
                string insert2 = "insert into stocktran(Stockid,StockTranid,trantype,quantity,TranDate,Userid,RealDate,TableName,PRTid)values(" + stockid + "," + stocktranid + ",1," + Convert.ToDouble(txtopeningstock.Text) + ",'" + date + "',0,'" + date + "','Opening stock'," + prt + " )";
                SqlHelper.ExecuteNonQuery(con, CommandType.Text, insert2);
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/DirectStockDestini.aspx");
            lblmessage.Visible = true;
            lblmessage.Text = ex.Message;
        }
    }
    protected void gvcarpetdetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            ds = null;
            string str = @"SELECT DISTINCT stock.StockID, stock.OpenStock, stock.Qtyinhand,stock.lotno,
                        (SELECT     TStockNos FROM  Get_StockNoRec_carpet_Wise(stock.ITEM_FINISHED_ID) AS Get_StockNoRec_carpet_Wise_1) AS StockNos, 
                        ITEM_PARAMETER_MASTER.QUALITY_ID, ITEM_PARAMETER_MASTER.DESIGN_ID, ITEM_PARAMETER_MASTER.COLOR_ID,ITEM_PARAMETER_MASTER.shadecolor_id,
                        ITEM_PARAMETER_MASTER.SHAPE_ID, ITEM_PARAMETER_MASTER.SIZE_ID, ITEM_MASTER.ITEM_ID, 
                        ITEM_MASTER.CATEGORY_ID,stocktran.stocktranid, stock.Companyid, stock.Godownid, stock.TypeId, Unit.UnitId, stock.ITEM_FINISHED_ID
                        FROM ITEM_PARAMETER_MASTER INNER JOIN stock ON ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = stock.ITEM_FINISHED_ID INNER JOIN
                        ITEM_MASTER ON ITEM_PARAMETER_MASTER.ITEM_ID = ITEM_MASTER.ITEM_ID INNER JOIN
                        Unit ON ITEM_MASTER.UnitTypeID = Unit.UnitTypeID inner join stocktran on stocktran.stockid=stock.stockid 
                        Where stock.StockID=" + gvcarpetdetail.SelectedValue + " And stock.Companyid = " + ddlcompany.SelectedValue + " And ITEM_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
            int stockid = Convert.ToInt32(ds.Tables[0].Rows[0]["stockid"]);
            txtstockid.Text = ds.Tables[0].Rows[0]["stockid"].ToString();

            ddlcatagorytype.SelectedValue = ds.Tables[0].Rows[0]["typeid"].ToString();
            string Qry = @" SELECT dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME 
            FROM CategorySeparate INNER JOIN ITEM_CATEGORY_MASTER ON dbo.CategorySeparate.Categoryid = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID
            WHERE dbo.CategorySeparate.id =" + ddlcatagorytype.SelectedValue + " And ITEM_CATEGORY_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
            Select Distinct Item_Id,Item_Name from Item_Master where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And  Category_Id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["category_id"].ToString()) + @"
            SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where i.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And item_id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["item_id"].ToString()) + @"
            select qualityid,qualityname from quality where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And  item_id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["item_id"].ToString()) + @"
            select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order  by DesignName
            SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
            select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by Shapeid
            select shadecolorid,shadecolorname from shadecolor Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

            if (ddlunit.SelectedValue == "1")
            {
                Qry = Qry + "   select sizeid,sizemtr from size where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " ANd Shapeid=" + Convert.ToInt32(ds.Tables[0].Rows[0]["shape_id"].ToString());
            }
            else if (ddlunit.SelectedValue == "2")
            {
                Qry = Qry + "   select sizeid,sizeft from size where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And Shapeid=" + Convert.ToInt32(ds.Tables[0].Rows[0]["shape_id"].ToString());
            }
            DataSet dsq = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddlcatagoryname, dsq, 0, true, "Select Catagory");
            ddlcatagoryname.SelectedValue = ds.Tables[0].Rows[0]["category_id"].ToString();
            UtilityModule.ConditionalComboFillWithDS(ref ddlitemname, dsq, 1, true, "--select Item--");
            ddlitemname.SelectedValue = ds.Tables[0].Rows[0]["item_id"].ToString();
            UtilityModule.ConditionalComboFillWithDS(ref ddlunit, dsq, 2, true, "Select Unit");
            UtilityModule.ConditionalComboFillWithDS(ref dquality, dsq, 3, true, "Select Quallity");
            dquality.SelectedValue = ds.Tables[0].Rows[0]["quality_id"].ToString();
            ddlunit.SelectedValue = ds.Tables[0].Rows[0]["unitid"].ToString();
            UtilityModule.ConditionalComboFillWithDS(ref dddesign, dsq, 4, true, "--Select Design--");
            UtilityModule.ConditionalComboFillWithDS(ref ddcolor, dsq, 5, true, "--Select Color--");
            UtilityModule.ConditionalComboFillWithDS(ref ddshape, dsq, 6, true, "--Select Shape--");
            UtilityModule.ConditionalComboFillWithDS(ref ddlshade, dsq, 7, true, "Select Shadecolor");
            dddesign.SelectedValue = ds.Tables[0].Rows[0]["design_id"].ToString();
            ddcolor.SelectedValue = ds.Tables[0].Rows[0]["color_id"].ToString();
            ddshape.SelectedValue = ds.Tables[0].Rows[0]["shape_id"].ToString();
            ddlshade.SelectedValue = ds.Tables[0].Rows[0]["shadecolor_id"].ToString();
            txtlotno.Text = ds.Tables[0].Rows[0]["lotno"].ToString();
            int finishedid = Convert.ToInt32(ds.Tables[0].Rows[0]["item_finished_id"].ToString());
            if (ddlunit.SelectedValue == "1")
            {
                UtilityModule.ConditionalComboFillWithDS(ref ddsize, dsq, 8, true, "select size");
            }
            else if (ddlunit.SelectedValue == "2")
            {
                UtilityModule.ConditionalComboFillWithDS(ref ddsize, dsq, 8, true, "select size");
            }


            ddsize.SelectedValue = ds.Tables[0].Rows[0]["size_id"].ToString();
            ddlgodown.SelectedValue = ds.Tables[0].Rows[0]["godownid"].ToString();
            txtopeningstock.Text = ds.Tables[0].Rows[0]["openstock"].ToString();
            Session["qtyinhand"] = ds.Tables[0].Rows[0]["qtyinhand"].ToString();
            Session["OpenStock"] = ds.Tables[0].Rows[0]["openstock"].ToString();
            Session["stocktranid"] = ds.Tables[0].Rows[0]["stocktranid"].ToString();
            Session["stockid"] = ds.Tables[0].Rows[0]["stockid"].ToString();
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
            int sc = (ddlshade.SelectedIndex > 0 ? Convert.ToInt32(ddlshade.SelectedValue) : 0);
            if (sc > 0)
            {
                shd.Visible = true;
            }
            else
            {
                shd.Visible = false;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/DirectStockDestini.aspx");
            lblmessage.Visible = true;
            lblmessage.Text = ex.Message;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        btnsave.Text = "Update";
        txtprefix.Text = "";
        Txtpostfix.Text = "";
        ddlgodown.Enabled = false;
        txtopeningstock.Focus();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER IPM inner join item_Master IM on IM.Item_Id=IPM.Item_Id inner join CategorySeparate CS on CS.CategoryId=IM.Category_Id  where ProductCode Like  '" + prefixText + "%' And IM.MasterCompanyId=" + MasterCompanyId;
        //string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER  where ProductCode Like  '" + prefixText + "%'";
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(strQuery, con);
        da.Fill(ds);
        count = ds.Tables[0].Rows.Count;
        con.Close();
        List<string> ProductCode = new List<string>();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            ProductCode.Add(ds.Tables[0].Rows[i][0].ToString());
        }
        con.Close();
        return ProductCode.ToArray();
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds1;
        string Str;
        if (TxtProdCode.Text != "")
        {
            ddlcatagoryname.SelectedIndex = -1;
            Str = @"select IPM.*,IM.CATEGORY_ID,cs.id from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,CategorySeparate cs where IPM.ITEM_ID=IM.ITEM_ID and im.CATEGORY_ID=cs.Categoryid
                  And ProductCode='" + TxtProdCode.Text + "'";
            ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                string Qry = @"SELECT dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME  FROM  dbo.CategorySeparate INNER JOIN
                dbo.ITEM_CATEGORY_MASTER ON dbo.CategorySeparate.Categoryid = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID And ITEM_CATEGORY_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                Qry = Qry + "  Select Distinct Item_Id,Item_Name from Item_Master where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And Category_Id=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["CATEGORY_ID"].ToString()) + " ";
                Qry = Qry + "  select qualityid,qualityname from quality where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And item_id=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["ITEM_ID"].ToString());
                Qry = Qry + @" select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order  by DesignName";
                Qry = Qry + @" SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                Qry = Qry + @" select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by ShapeName";
                Qry = Qry + @"  SELECT SIZEID,SIZEFT fROM SIZE WhERE MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And SHAPEID=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["SIZE_ID"].ToString());

                DataSet DSQ = SqlHelper.ExecuteDataset(Qry);

                UtilityModule.ConditionalComboFillWithDS(ref ddlcatagoryname, DSQ, 0, true, "Select Catagory");
                ddlcatagoryname.SelectedValue = ds1.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddlitemname, DSQ, 1, true, "--Select Item--");
                ddlitemname.SelectedValue = ds1.Tables[0].Rows[0]["ITEM_ID"].ToString();

                UtilityModule.ConditionalComboFillWithDS(ref dquality, DSQ, 2, true, "Select Quallity");
                dquality.SelectedValue = ds1.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref dddesign, DSQ, 3, true, "Select Design");
                dddesign.SelectedValue = ds1.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddcolor, DSQ, 4, true, "--Select Color--");
                ddcolor.SelectedValue = ds1.Tables[0].Rows[0]["COLOR_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddshape, DSQ, 5, true, "--Select Shape--");
                ddshape.SelectedValue = ds1.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddsize, DSQ, 6, true, "--SELECT SIZE--");
                ddsize.SelectedValue = ds1.Tables[0].Rows[0]["SIZE_ID"].ToString();
                Session["finishedid"] = ds1.Tables[0].Rows[0]["Item_Finished_id"].ToString();
                if (Convert.ToInt32(dquality.SelectedValue) > 0)
                {
                    ql.Visible = true;
                }
                else
                {
                    ql.Visible = false;
                }
                int d = (dddesign.SelectedIndex > 0 ? Convert.ToInt32(dddesign.SelectedValue) : 0);
                if (d > 0)
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
                Label2.Visible = false;
            }
            else
            {
                Label2.Visible = true;
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
            }
        }
        else
        {
            ddlcatagoryname.SelectedIndex = 0;
        }
    }
    private void raw_stock_update2()
    {
        ItemFinishedId = UtilityModule.getItemFinishedId(ddlitemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        string strupdate = "update stock set Qtyinhand=" + remainstock + "where ITEM_FINISHED_ID=" + ItemFinishedId + "and companyid=" + ddlcompany.SelectedValue + "and Godownid=" + ddlgodown.SelectedValue;
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strupdate);
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Response.Redirect("../../main.aspx");
    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_grid();
    }
    protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_grid();
    }
    protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_grid();
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_grid();
    }
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_grid();
    }
    protected void gvcarpetdetail_RowCreated(object sender, GridViewRowEventArgs e)
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
}