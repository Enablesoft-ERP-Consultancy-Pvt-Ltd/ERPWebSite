using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Order_FrmItemWiseRateDefine : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        LblMessage.Text = "";
        if (!IsPostBack)
        {

            string str = @"Select CI.CompanyId,CompanyName 
            From CompanyInfo CI(Nolock)
            JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + " And CA.MasterCompanyid = " + Session["varCompanyId"] + @" order by CompanyName 
            Select UnitID, UnitName From Unit(Nolock) Where UnitID in (1, 2) Order By UnitName";

            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, true, "--Select--");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                CompanySelectedIndexChange();
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDUnitName, ds, 1, true, "--Select--");
            DDUnitName.SelectedValue = "2";
        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedIndexChange();
    }
    private void CompanySelectedIndexChange()
    {
        SqlParameter[] para = new SqlParameter[3];
        para[0] = new SqlParameter("@DropDownIDs", SqlDbType.VarChar, 100);
        para[1] = new SqlParameter("@FillDropDownType", SqlDbType.Int);
        para[2] = new SqlParameter("@CategoryType", SqlDbType.Int);

        para[0].Value = DDCompany.SelectedValue + "|0|0|0|0|0|0";
        para[1].Value = 0;
        para[2].Value = DDCategoryType.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_ItemWiseRateDefine_DropDown", para);

        UtilityModule.ConditionalComboFillWithDS(ref DDcustomer, ds, 0, true, "--SELECT--");
    }
    protected void DDCategoryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDCategoryType.SelectedValue == "0")
        {
            TDCustomer.Visible = true;
            Fill_CategoryName();
        }
        else
        {
            TDCustomer.Visible = false;
            Fill_CategoryName();
        }
    }
    protected void DDcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_CategoryName();
    }
    private void Fill_CategoryName()
    {
        SqlParameter[] para = new SqlParameter[3];
        para[0] = new SqlParameter("@DropDownIDs", SqlDbType.VarChar, 100);
        para[1] = new SqlParameter("@FillDropDownType", SqlDbType.Int);
        para[2] = new SqlParameter("@CategoryType", SqlDbType.Int);

        para[0].Value = DDCompany.SelectedValue + "|" + DDcustomer.SelectedValue + "|0|0|0|0|0";
        para[1].Value = 1;
        para[2].Value = DDCategoryType.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_ItemWiseRateDefine_DropDown", para);

        UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 0, true, "--SELECT--");
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
    }
    private void ddlcategorycange()
    {
        TDQuality.Visible = false;
        TdDESIGN.Visible = false;
        TDColor.Visible = false;
        TDShadeColor.Visible = false;

        if (DDcustomer.SelectedIndex > 0 && DDCategory.SelectedIndex > 0)
        {
            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter("@DropDownIDs", SqlDbType.VarChar, 100);
            para[1] = new SqlParameter("@FillDropDownType", SqlDbType.Int);
            para[2] = new SqlParameter("@CategoryType", SqlDbType.Int);

            para[0].Value = DDCompany.SelectedValue + "|" + DDcustomer.SelectedValue + "|" + DDCategory.SelectedValue + "|0|0|0|0";
            para[1].Value = 2;
            para[2].Value = DDCategoryType.SelectedValue;

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_ItemWiseRateDefine_DropDown", para);

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
                            TdDESIGN.Visible = true;
                            break;
                        case "3":
                            TDColor.Visible = true;
                            break;
                    }
                }
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDItemName, ds, 1, true, "--SELECT--");
        }
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDcustomer.SelectedIndex > 0 && DDCategory.SelectedIndex > 0 && DDItemName.SelectedIndex > 0)
        {
            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter("@DropDownIDs", SqlDbType.VarChar, 100);
            para[1] = new SqlParameter("@FillDropDownType", SqlDbType.Int);
            para[2] = new SqlParameter("@CategoryType", SqlDbType.Int);

            para[0].Value = DDCompany.SelectedValue + "|" + DDcustomer.SelectedValue + "|" + DDCategory.SelectedValue + "|" + DDItemName.SelectedValue + "|0|0|0";
            para[1].Value = 3;
            para[2].Value = DDCategoryType.SelectedValue;

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_ItemWiseRateDefine_DropDown", para);

            UtilityModule.ConditionalComboFillWithDS(ref DDQualityName, ds, 0, true, "--SELECT--");
        }
    }
    protected void DDQualityName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDcustomer.SelectedIndex > 0 && DDCategory.SelectedIndex > 0 && DDItemName.SelectedIndex > 0 && DDQualityName.SelectedIndex > 0)
        {
            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter("@DropDownIDs", SqlDbType.VarChar, 100);
            para[1] = new SqlParameter("@FillDropDownType", SqlDbType.Int);
            para[2] = new SqlParameter("@CategoryType", SqlDbType.Int);

            para[0].Value = DDCompany.SelectedValue + "|" + DDcustomer.SelectedValue + "|" + DDCategory.SelectedValue + "|" + DDItemName.SelectedValue + "|" + DDQualityName.SelectedValue + "|0|0";
            para[1].Value = 4;
            para[2].Value = DDCategoryType.SelectedValue;

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_ItemWiseRateDefine_DropDown", para);

            UtilityModule.ConditionalComboFillWithDS(ref DDDesignName, ds, 0, true, "--SELECT--");
        }
    }

    protected void DDDesignName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDcustomer.SelectedIndex > 0 && DDCategory.SelectedIndex > 0 && DDItemName.SelectedIndex > 0 && DDQualityName.SelectedIndex > 0 && DDDesignName.SelectedIndex > 0)
        {
            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter("@DropDownIDs", SqlDbType.VarChar, 100);
            para[1] = new SqlParameter("@FillDropDownType", SqlDbType.Int);
            para[2] = new SqlParameter("@CategoryType", SqlDbType.Int);

            para[0].Value = DDCompany.SelectedValue + "|" + DDcustomer.SelectedValue + "|" + DDCategory.SelectedValue + "|" + DDItemName.SelectedValue + "|" + DDQualityName.SelectedValue + "|" + DDDesignName.SelectedValue + "|0";
            para[1].Value = 5;
            para[2].Value = DDCategoryType.SelectedValue;

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_ItemWiseRateDefine_DropDown", para);

            UtilityModule.ConditionalComboFillWithDS(ref DDColorName, ds, 0, true, "--SELECT--");
        }
    }

    protected void BtnShowData_Click(object sender, EventArgs e)
    {
        DGItemDetail.DataSource = null;
        DGItemDetail.DataBind();

        SqlParameter[] para = new SqlParameter[3];
        para[0] = new SqlParameter("@DropDownIDs", SqlDbType.VarChar, 100);
        para[1] = new SqlParameter("@FillDropDownType", SqlDbType.Int);
        para[2] = new SqlParameter("@CategoryType", SqlDbType.Int);

        string Str = DDCompany.SelectedValue + "|" + DDcustomer.SelectedValue + "|" + DDCategory.SelectedValue + "|" + DDItemName.SelectedValue + "|" + DDQualityName.SelectedValue + "|";

        if (TdDESIGN.Visible == true && DDDesignName.SelectedIndex > 0)
        {
            Str = Str + DDDesignName.SelectedValue + "|";
        }
        else
        {
            Str = Str + "0|";
        }
        if (TDColor.Visible == true && DDColorName.SelectedIndex > 0)
        {
            Str = Str + DDColorName.SelectedValue;
        }
        else
        {
            Str = Str + "0";
        }


        para[0].Value = Str;
        para[1].Value = 6;
        para[2].Value = DDCategoryType.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_ItemWiseRateDefine_DropDown", para);
        DGItemDetail.DataSource = ds;
        DGItemDetail.DataBind();
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        string DetailTable = "";
        for (int i = 0; i < DGItemDetail.Rows.Count; i++)
        {
            Label lblCategoryID = (Label)DGItemDetail.Rows[i].FindControl("lblCategoryID");
            Label lblItemID = (Label)DGItemDetail.Rows[i].FindControl("lblItemID");
            Label lblQualityID = (Label)DGItemDetail.Rows[i].FindControl("lblQualityID");
            Label lblDesignID = (Label)DGItemDetail.Rows[i].FindControl("lblDesignID");
            Label lblColorID = (Label)DGItemDetail.Rows[i].FindControl("lblColorID");
            TextBox TxtRate = (TextBox)DGItemDetail.Rows[i].FindControl("TxtRate");

            if (TxtRate.Text != "")
            {
                if (DetailTable == "")
                {
                    DetailTable = lblCategoryID.Text + "|" + lblItemID.Text + "|" + lblQualityID.Text + "|" + lblDesignID.Text + "|" + lblColorID.Text + "|" + TxtRate.Text + "~";
                }
                else
                {
                    DetailTable = DetailTable + lblCategoryID.Text + "|" + lblItemID.Text + "|" + lblQualityID.Text + "|" + lblDesignID.Text + "|" + lblColorID.Text + "|" + TxtRate.Text + "~";
                }
            }
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] parparam = new SqlParameter[8];

            parparam[0] = new SqlParameter("@CompanyID", DDCompany.SelectedValue);
            parparam[1] = new SqlParameter("@CustomerID", DDcustomer.SelectedValue);
            parparam[2] = new SqlParameter("@CalType", DDCalType.SelectedValue);
            parparam[3] = new SqlParameter("@UnitID", DDUnitName.SelectedValue);
            parparam[4] = new SqlParameter("@DetailData", DetailTable);
            parparam[5] = new SqlParameter("@UserID", Session["varuserId"].ToString().Trim());
            parparam[6] = new SqlParameter("@MasterCompanyID", Session["varCompanyId"].ToString().Trim());
            parparam[7] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            parparam[7].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_Save_ItemWiseRateDefineMaster", parparam);
            if (parparam[7].Value.ToString() == "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('Data saved successfully');", true);
                Tran.Commit();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + parparam[7].Value.ToString() + "');", true);
                Tran.Rollback();
            }
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmItemWiseRateDefine");
            LblMessage.Text = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void BtnPrev_Click(object sender, EventArgs e)
    {

    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Main.aspx");
    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmItemWiseRateDefine.aspx");
    }
}
