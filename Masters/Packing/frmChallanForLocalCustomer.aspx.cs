using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Packing_frmChallanForLocalCustomer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            DataSet ds = null;
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And 
                            CA.UserId=" + Session["varuserId"] + @" And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                            select CustomerId,CustomerCode From Customerinfo Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                            select Val,Type from sizetype
                            select ICM.CATEGORY_ID,ICM.CATEGORY_NAME 
                            from ITEM_CATEGORY_MASTER ICM 
                            inner join CategorySeparate cs on ICM.CATEGORY_ID=cs.Categoryid and cs.id=0 And CS.MasterCompanyid=" + Session["varcompanyId"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "--Select Company Name--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds, 1, true, "--Select Customer Code--");
            UtilityModule.ConditionalComboFillWithDS(ref DDSizetype, ds, 2, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 3, true, "--Plz Select--");
            txtChallanDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDOrderNo, "select OrderId,CustomerOrderNo from OrderMaster Where CustomerId=" + DDCustomerCode.SelectedValue + " And CompanyId=" + DDCompanyName.SelectedValue + "", true, "--Select Order No.--");
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDitemName, "select ITEM_ID,ITEM_NAME from ITEM_MASTER where CATEGORY_ID=" + DDCategory.SelectedValue + " And MasterCompanyid=" + Session["varcompanyId"] + "", true, "--Plz Select--");
        Fillcombo();
    }
    protected void Fillcombo()
    {
        TDColor.Visible = false;
        TDDesign.Visible = false;
        TDQuality.Visible = false;
        TDShape.Visible = false;
        TDSize.Visible = false;
        TDShadeColor.Visible = false;

        DDColor.Items.Clear();
        DDDesign.Items.Clear();
        DDShape.Items.Clear();
        DDQuality.Items.Clear();
        DDSize.Items.Clear();

        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME 
                      FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on 
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        TDQuality.Visible = true;
                        break;
                    case "2":
                        TDDesign.Visible = true;
                        UtilityModule.ConditionalComboFill(ref DDDesign, "select DesignId,Designname from Design Where MasterCompanyid=" + Session["varcompanyid"] + " Order by Designname", true, "--Plz Select--");
                        break;
                    case "3":
                        TDColor.Visible = true;
                        UtilityModule.ConditionalComboFill(ref DDColor, "select ColorId,ColorName from color Where MasterCompanyid=" + Session["varcompanyid"] + " Order by ColorName", true, "--Plz Select--");
                        break;
                    case "4":
                        TDShape.Visible = true;
                        UtilityModule.ConditionalComboFill(ref DDShape, "select ShapeId,Shapename from Shape Where MasterCompanyid=" + Session["varcompanyid"] + " Order by Shapename", true, "--Plz Select--");
                        break;
                    case "5":
                        TDSize.Visible = true;
                        break;
                    case "6":
                        TDShadeColor.Visible = true;
                        break;
                    case "10":
                        TDColor.Visible = true;
                        break;
                }
            }
        }
    }
    protected void DDitemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDQuality, "select QualityId,QualityName from Quality where Item_Id=" + DDitemName.SelectedValue + " And MasterCompanyid=" + Session["varcompanyId"] + " Order by QualityName", true, "--Plz Select--");
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = string.Empty;
        str = "select Sizeid,case When 0=" + DDSizetype.SelectedValue + " Then Sizeft Else Case When 1=" + DDSizetype.SelectedValue + " Then Sizemtr Else case when 2=" + DDSizetype.SelectedValue + " Then Sizeinch Else Sizeft End End End As Size from size Where ShapeId=" + DDShape.SelectedValue + " And MastercompanyId=" + Session["varcompanyId"];
        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--Plz Select--");
    }
    protected void DDSizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDShape_SelectedIndexChanged(sender, e);
    }
    protected void FillStockGrid()
    {
        DataSet ds = null;

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlParameter[] param = new SqlParameter[10];
            param[0] = new SqlParameter("@OrderId", SqlDbType.Int);
            param[1] = new SqlParameter("@CategoryId", SqlDbType.Int);
            param[2] = new SqlParameter("@Itemid", SqlDbType.Int);
            param[3] = new SqlParameter("@QualityId", SqlDbType.Int);
            param[4] = new SqlParameter("@DesignId", SqlDbType.Int);
            param[5] = new SqlParameter("@ColorId", SqlDbType.Int);
            param[6] = new SqlParameter("@ShapeId", SqlDbType.Int);
            param[7] = new SqlParameter("@SizeId", SqlDbType.Int);
            param[8] = new SqlParameter("@ShadeColorId", SqlDbType.Int);
            param[9] = new SqlParameter("@SizeType", SqlDbType.Int);

            //Assign value
            param[0].Value = DDOrderNo.SelectedIndex <= 0 ? "0" : DDOrderNo.SelectedValue;
            int ItemFinishedId = UtilityModule.getItemFinishedId(DDitemName, DDQuality, DDDesign, DDColor, DDShape, DDSize, txtProdCode, DDshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            param[1].Value = DDCategory.SelectedIndex <= 0 ? "0" : DDCategory.SelectedValue;
            param[2].Value = DDitemName.SelectedIndex <= 0 ? "0" : DDitemName.SelectedValue;
            param[3].Value = DDQuality.SelectedIndex <= 0 ? "0" : DDQuality.SelectedValue;
            param[4].Value = DDDesign.SelectedIndex <= 0 ? "0" : DDDesign.SelectedValue;
            param[5].Value = DDColor.SelectedIndex <= 0 ? "0" : DDColor.SelectedValue;
            param[6].Value = DDShape.SelectedIndex <= 0 ? "0" : DDShape.SelectedValue;
            param[7].Value = DDSize.SelectedIndex <= 0 ? "0" : DDSize.SelectedValue;
            param[8].Value = DDshade.SelectedIndex <= 0 ? "0" : DDshade.SelectedValue;
            param[9].Value = DDSizetype.SelectedIndex < 0 ? "0" : DDSizetype.SelectedValue;

            ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Pro_getStockNo", param);

            GVStockDetail.DataSource = ds;
            GVStockDetail.DataBind();
            if (ds.Tables[0].Rows.Count == 0)
            {
                lblMsg.Text = "No Stock Available for this Combination..";
            }

        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();

        }


    }
    protected void btnShowDetail_Click(object sender, EventArgs e)
    {
        FillStockGrid();
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnShowDetail_Click(sender, e);
    }
    protected void GVStockDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Save")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            { con.Open(); }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[15];
                param[0] = new SqlParameter("@ChallanId", SqlDbType.Int);
                param[1] = new SqlParameter("@ChallanDetailId", SqlDbType.Int);
                param[2] = new SqlParameter("@Companyid", SqlDbType.Int);
                param[3] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 100);
                param[4] = new SqlParameter("@ChallanDate", SqlDbType.SmallDateTime);
                param[5] = new SqlParameter("@Item_Finished_id", SqlDbType.Int);
                param[6] = new SqlParameter("@StockNo", SqlDbType.BigInt);
                param[7] = new SqlParameter("@TStockNo", SqlDbType.VarChar, 100);
                param[8] = new SqlParameter("@CustomerId", SqlDbType.Int);
                param[9] = new SqlParameter("@OrderId", SqlDbType.Int);
                param[10] = new SqlParameter("@Qty", SqlDbType.Int);
                param[11] = new SqlParameter("@UserId", SqlDbType.Int);
                param[12] = new SqlParameter("@MasterCompanyid", SqlDbType.Int);
                param[13] = new SqlParameter("@Msg", SqlDbType.VarChar, 50);
                param[14] = new SqlParameter("@SizeType", SqlDbType.Int);

                if (hnchallanId.Value == "" || hnchallanId.Value == null)
                {
                    hnchallanId.Value = "0";
                }
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = hnchallanId.Value;
                param[1].Value = 0;
                param[2].Value = DDCompanyName.SelectedValue;
                param[3].Value = txtChallanNo.Text;
                param[4].Value = txtChallanDate.Text;
                //Getrow index
                GridViewRow gvr = (GridViewRow)((Button)e.CommandSource).NamingContainer;
                int rowindex = gvr.RowIndex;
                Label lblItemFinishedid = ((Label)GVStockDetail.Rows[rowindex].FindControl("lblitemFinishedid"));
                TextBox txtPackQty = ((TextBox)GVStockDetail.Rows[rowindex].FindControl("txtPackQty"));
                ////
                param[5].Value = lblItemFinishedid.Text;
                param[6].Value = 0;
                param[7].Value = "";
                param[8].Value = DDCustomerCode.SelectedIndex <= 0 ? "0" : DDCustomerCode.SelectedValue;
                param[9].Value = DDOrderNo.SelectedIndex <= 0 ? "0" : DDOrderNo.SelectedValue;
                param[10].Value = txtPackQty.Text == "" ? "0" : txtPackQty.Text;
                param[11].Value = Session["varuserid"];
                param[12].Value = Session["varcompanyid"];
                param[13].Direction = ParameterDirection.Output;
                param[14].Value = DDSizetype.SelectedIndex < 0 ? "0" : DDSizetype.SelectedValue;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveLocalChallan", param);
                hnchallanId.Value = param[0].Value.ToString();
                Tran.Commit();
                lblMsg.Text = param[13].Value.ToString();
                //ScriptManager.RegisterClientScriptBlock(Page, GetType(), "save", "alert('" + param[13].Value.ToString() + "');", true);
                FillStockGrid();
                FillChallanDetail();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblMsg.Text = ex.Message;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
        }

    }
    protected void FillChallanDetail()
    {

        string str = @"select vf.Category_name,vf.ITEM_NAME+' '+vf.QualityName+' '+vf.designName+' '+vf.ColorName+' '+vf.ShapeName+' '+case when 0=CD.Sizetype Then vf.SizeFt Else Case when 1=CD.Sizetype Then Vf.SizeMtr Else case When 2=CD.Sizetype then vf.SizeFt Else vf.SizeFt End End End as itemDescription,CM.ChallanId,CD.ChallanDetailId,TStockNo,Qty from challanMaster CM inner join ChallanDetail  CD on CM.ChallanId=CD.ChallanId
                     inner join V_finisheditemDetail vf on CD.item_Finished_Id=vf.Item_Finished_id where CM.ChallanId=" + hnchallanId.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GvChallanDetail.DataSource = ds;
        GvChallanDetail.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {
            txttotalPackQty.Text = ds.Tables[0].Compute("sum(Qty)", "").ToString();
        }
    }
    protected void txtStockNo_TextChanged(object sender, EventArgs e)
    {
        if (txtStockNo.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            { con.Open(); }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[15];
                param[0] = new SqlParameter("@ChallanId", SqlDbType.Int);
                param[1] = new SqlParameter("@ChallanDetailId", SqlDbType.Int);
                param[2] = new SqlParameter("@Companyid", SqlDbType.Int);
                param[3] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 100);
                param[4] = new SqlParameter("@ChallanDate", SqlDbType.SmallDateTime);
                param[5] = new SqlParameter("@TStockNo", SqlDbType.VarChar, 100);
                param[6] = new SqlParameter("@CustomerId", SqlDbType.Int);
                param[7] = new SqlParameter("@OrderId", SqlDbType.Int);
                param[8] = new SqlParameter("@UserId", SqlDbType.Int);
                param[9] = new SqlParameter("@MasterCompanyid", SqlDbType.Int);
                param[10] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                param[11] = new SqlParameter("@SizeType", SqlDbType.Int);

                if (hnchallanId.Value == "" || hnchallanId.Value == null)
                {
                    hnchallanId.Value = "0";
                }
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = hnchallanId.Value;
                param[1].Value = 0;
                param[2].Value = DDCompanyName.SelectedValue;
                param[3].Value = txtChallanNo.Text;
                param[4].Value = txtChallanDate.Text;
                param[5].Value = txtStockNo.Text;// lblItemFinishedid.Text;
                param[6].Value = DDCustomerCode.SelectedIndex <= 0 ? "0" : DDCustomerCode.SelectedValue;
                param[7].Value = DDOrderNo.SelectedIndex <= 0 ? "0" : DDOrderNo.SelectedValue;
                param[8].Value = Session["varuserid"];
                param[9].Value = Session["varcompanyid"];
                param[10].Direction = ParameterDirection.Output;
                param[11].Value = DDSizetype.SelectedIndex < 0 ? "0" : DDSizetype.SelectedValue;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveLocalChallan_StockWIse", param);
                hnchallanId.Value = param[0].Value.ToString();
                Tran.Commit();
                txtStockNo.Text = "";
                lblMsg.Text = param[10].Value.ToString();
                //ScriptManager.RegisterClientScriptBlock(Page, GetType(), "saveStockWise", "alert('" + param[13].Value.ToString() + "');", true);
                FillStockGrid();
                FillChallanDetail();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblMsg.Text = ex.Message;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
        }
    }
}