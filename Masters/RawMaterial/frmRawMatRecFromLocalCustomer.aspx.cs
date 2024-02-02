using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_RawMaterial_frmRawMatRecFromLocalCustomer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");

        }
        if (!IsPostBack)
        {
            string Qry = @"Select Distinct CI.CompanyId,CompanyName from Companyinfo CI Inner Join Company_Authentication CA on CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by CompanyName
            Select Customerid,CustomerCode from Customerinfo  Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by Customercode
            select Category_Id,Category_Name from item_Category_Master ICM inner join CategorySeparate CS on ICM.Category_id=cs.CategoryId
            And CS.id=1 And CS.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
            select godownId,GodownName from GodownMaster";
            DataSet ds;
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Qry);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");
            
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDCustomer, ds, 1, true, "--Plz Select Customer--");
            UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, ds, 2, true, "--Plz Select Category--");
            UtilityModule.ConditionalComboFillWithDS(ref DDgodown, ds, 3, false, "");
            txtRecDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            LabelChange();
            hnReceiveId.Value = "0";
        }
    }
    protected void LabelChange()
    {
        string[] ParameterList = new string[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt16(Session["varMasterCompanyIDForERP"]));
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        lblsizename.Text = ParameterList[4];
        lblcategoryname.Text = ParameterList[5];
        lblsizename.Text = ParameterList[6];
        lblshadecolor.Text = ParameterList[7];
    }
    protected void DDCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDOrderNo, "select Orderid,customerorderNo from OrderMaster where Companyid=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomer.SelectedValue + "", true, "--Plz Select OrderNo--");
        hnReceiveId.Value = "0";
        RefreshControl();
        gvdetail.DataSource = null;
        gvdetail.DataBind();
        txtChallanNo.Text = "";
    }

    protected void ddlcategorychange()
    {
        try
        {
            ql.Visible = false;
            clr.Visible = false;
            dsn.Visible = false;
            shp.Visible = false;
            sz.Visible = false;
            string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                          " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                          " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCatagory.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    switch (dr["PARAMETER_ID"].ToString())
                    {
                        case "1":
                            ql.Visible = true;
                            break;
                        case "2":
                            dsn.Visible = true;
                            UtilityModule.ConditionalComboFill(ref dddesign, "select distinct designId, designName from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By designName ", true, "--Select Design--");
                            break;
                        case "3":
                            clr.Visible = true;
                            UtilityModule.ConditionalComboFill(ref ddcolor, "select distinct colorid, colorname from color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By colorname ", true, "--Select Color--");
                            break;
                        case "4":
                            shp.Visible = true;
                            UtilityModule.ConditionalComboFill(ref ddshape, "select distinct ShapeId, ShapeName from shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  Order By ShapeName ", true, "--Select Shape--");
                            break;
                        case "5":
                            sz.Visible = true;
                            //ChkForMtr.Checked = false;
                            break;
                        case "6":
                            shd.Visible = true;
                            UtilityModule.ConditionalComboFill(ref ddlshade, "select distinct ShadecolorId, ShadeColorName from ShadeColor Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By ShadeColorName ", true, "--Select ShadeColor--");
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchageIndentIssue.aspx");
        }
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dditemname, "select distinct item_id,item_name  from ITEM_MASTER where category_id=" + ddCatagory.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By item_name ", true, "--Select Item--");

        ddlcategorychange();
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dquality, "select distinct qualityid, qualityname from quality where item_id=" + dditemname.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By qualityname ", true, "--Select Item--");
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
            SqlParameter[] array = new SqlParameter[18];
            array[0] = new SqlParameter("@ReceiveId", SqlDbType.Int);
            array[1] = new SqlParameter("@DetailId", SqlDbType.Int);
            array[2] = new SqlParameter("@CompanyId", SqlDbType.Int);
            array[3] = new SqlParameter("@CustomerId", SqlDbType.Int);
            array[4] = new SqlParameter("@OrderId", SqlDbType.Int);
            array[5] = new SqlParameter("@ReceiveDate", SqlDbType.SmallDateTime);
            array[6] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 50);
            array[7] = new SqlParameter("@TransPortName", SqlDbType.VarChar, 100);
            array[8] = new SqlParameter("@TransportAdd", SqlDbType.VarChar, 250);
            array[9] = new SqlParameter("@Drivername", SqlDbType.VarChar, 60);
            array[10] = new SqlParameter("@VehicleNo", SqlDbType.VarChar, 50);
            array[11] = new SqlParameter("@Godownid", SqlDbType.Int);
            array[12] = new SqlParameter("@LotNo", SqlDbType.VarChar, 50);
            array[13] = new SqlParameter("@Qty", SqlDbType.Float);
            array[14] = new SqlParameter("@UserId", SqlDbType.Int);
            array[15] = new SqlParameter("@MasterCompanyid", SqlDbType.Int);
            array[16] = new SqlParameter("@Finishedid", SqlDbType.Int);
            array[17] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            array[0].Direction = ParameterDirection.InputOutput;
            if (hnReceiveId.Value == "" || hnReceiveId.Value == null)
            {
                hnReceiveId.Value = "0";
            }
            array[0].Value = hnReceiveId.Value;
            array[1].Value = 0;
            array[1].Direction = ParameterDirection.Output;
            array[2].Value = DDCompanyName.SelectedValue;
            array[3].Value = DDCustomer.SelectedValue;
            array[4].Value = DDOrderNo.SelectedValue;
            array[5].Value = txtRecDate.Text;
            array[6].Value = txtChallanNo.Text;
            array[6].Direction = ParameterDirection.InputOutput;
            array[7].Value = txtTransportName.Text;
            array[8].Value = txtTransportAddress.Text;
            array[9].Value = txtDriver.Text;
            array[10].Value = txtVehicleNo.Text;
            array[11].Value = DDgodown.SelectedValue;
            array[12].Value = "Without Lot No";
            array[13].Value = txtRecQty.Text;
            array[14].Value = Session["Varuserid"];
            array[15].Value = Session["varMasterCompanyIDForERP"];
            int varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, Tran, ddlshade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            array[16].Value = varfinishedid;
            array[17].Direction = ParameterDirection.Output;
            //Insert Date
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveRawMatRecCust", array);
            //Update Stock
            UtilityModule.StockStockTranTableUpdate(Convert.ToInt32(array[16].Value), Convert.ToInt32(array[11].Value), Convert.ToInt32(array[2].Value), Convert.ToString(array[12].Value), Convert.ToDouble(array[13].Value), Convert.ToString(array[5].Value), DateTime.Now.ToString("dd-MMM-yyyy"), "RawMatRecCust", Convert.ToInt32(array[1].Value), Tran, 1, true, 1, 0);
            Tran.Commit();

            hnReceiveId.Value = array[0].Value.ToString();
            lblMessage.Text = array[17].Value.ToString();
            txtChallanNo.Text = array[6].Value.ToString();

            FillGrid();
            RefreshControl();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void RefreshControl()
    {
        txtRecQty.Text = "";
        dquality.SelectedIndex = -1;
        ddlshade.SelectedIndex = -1;
        dddesign.SelectedIndex = -1;
        ddcolor.SelectedIndex = -1;
        ddshape.SelectedIndex = -1;
        ddsize.SelectedIndex = -1;
        dquality.Focus();
    }
    protected void FillGrid()
    {
        string str = @"select vf.Category_Name,vf.Item_Name,vf.QualityName+' ' +vf.DesignName+' '+vf.ColorName+' '+vf.ShadeColorName+' '+vf.ShapeName+' '+vf.Sizeft As Description
                    ,GM.GodownName,RD.LotNo,RD.Qty,RD.ReceiveId,RD.Detailid from RawMatRecCustMaster RM,RawMatRecCustDetail RD,V_FinisheditemDetail vf,
                     GodownMaster GM where RM.ReceiveId=RD.ReceiveId
                     And RD.Finishedid=vf.Item_FInished_id And GM.GodownId=RD.GodownId And RD.ReceiveId=" + hnReceiveId.Value + "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        gvdetail.DataSource = ds;
        gvdetail.DataBind();

    }
    protected void gvdetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
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
    protected void chkTransportInformation_CheckedChanged(object sender, EventArgs e)
    {
        if (chkTransportInformation.Checked == true)
        {
            DivTransPort.Visible = true;

        }
        else
        {
            DivTransPort.Visible = false;
        }
    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        { con.Open(); }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] array = new SqlParameter[3];
            array[0] = new SqlParameter("@ReceiveId", SqlDbType.Int);
            array[1] = new SqlParameter("@DetailId", SqlDbType.Int);
            array[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 50);

            array[0].Value = ((Label)gvdetail.Rows[e.RowIndex].FindControl("lblReceiveId")).Text;
            array[1].Value = gvdetail.DataKeys[e.RowIndex].Value.ToString();
            array[2].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteRawMatRecCust", array);
            Tran.Commit();
            lblMessage.Text = array[2].Value.ToString();

            FillGrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }

    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnReceiveId.Value = "0";
        RefreshControl();
        gvdetail.DataSource = null;
        gvdetail.DataBind();
        txtChallanNo.Text = "";
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string str = @"select * from V_RawMatRecCustReport Where ReceiveId=" + hnReceiveId.Value + "";
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "Reports/RptRawMatRecCustDetail.rpt"; ;
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RawMatRecCustDetail.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=10px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found...!');", true);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
}