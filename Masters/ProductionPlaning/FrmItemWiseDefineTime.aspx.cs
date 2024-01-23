using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
public partial class Masters_ProductionPlaning_FrmItemWiseDefineTime : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["varMasterCompanyIDForERP"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            lablechange();
            string Str = @"Select ICM.CATEGORY_ID, ICM.CATEGORY_NAME 
                    From ITEM_CATEGORY_MASTER ICM(Nolock) 
                    JOIN CategorySeparate CS(Nolock) ON CS.Categoryid = ICM.CATEGORY_ID And CS.id = 0 
                    Where ICM.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" 
                    Order By ICM.CATEGORY_NAME 
                    Select Process_Name_ID, PROCESS_NAME 
                    From PROCESS_NAME_MASTER PNM(Nolock) Where PNM.PROCESS_NAME_ID in (5, 99, 33, 1, 29, 89, 2, 12, 4, 19, 35, 6, 7, 15) Order By AddProcessName ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCategoryName, ds, 0, false, "");
            FillCategorySelectedChange();
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Select Process--");
        }
    }
    protected void DDCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategorySelectedChange();
    }
    private void FillCategorySelectedChange()
    {
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
        TDShape.Visible = false;
        TDSize.Visible = false;
        TDShadeColor.Visible = false;
        string strsql = @"SELECT IPM.CATEGORY_PARAMETERS_ID, IPM.CATEGORY_ID, IPM.PARAMETER_ID, PM.PARAMETER_NAME 
                    FROM ITEM_CATEGORY_PARAMETERS IPM(Nolock) 
                    JOIN PARAMETER_MASTER PM(Nolock) ON PM.PARAMETER_ID = IPM.PARAMETER_ID And PM.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @"  
                    Where IPM.CATEGORY_ID = " + DDCategoryName.SelectedValue;

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
                        break;
                    case "3":
                        TDColor.Visible = true;
                        break;
                    case "4":
                        TDShape.Visible = true;
                        break;
                    case "5":
                        TDSize.Visible = true;
                        break;
                    case "6":
                        TDShadeColor.Visible = true;
                        break;
                }
            }
        }
        UtilityModule.ConditionalComboFill(ref DDItemName, @"Select IM.ITEM_ID, IM.ITEM_NAME 
                    From ITEM_MASTER IM(Nolock) 
                    Where IM.CATEGORY_ID = " + DDCategoryName.SelectedValue + " And IM.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" Order By IM.ITEM_NAME", true, "--Select ItemName--");
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemNameSelectedChange();
    }
    private void ItemNameSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDQuality, @"Select Distinct VF.QualityID, VF.QualityName 
            From V_FinishedItemDetail VF(Nolock) 
            Where VF.CATEGORY_ID = " + DDCategoryName.SelectedValue + " And VF.ITEM_ID = " + DDItemName.SelectedValue + @" 
            And VF.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" And VF.DesignID > 0 And VF.ColorId > 0 And VF.SizeID > 0", true, "--Select Quality--");
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        QualitySelectedChange();
    }
    private void QualitySelectedChange()
    {
        string Str = @"Select Distinct VF.DesignID, VF.DesignName 
            From V_FinishedItemDetail VF(Nolock) 
            Where VF.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" And VF.DesignID > 0 And VF.ColorId > 0 And VF.SizeID > 0 And 
            VF.CATEGORY_ID = " + DDCategoryName.SelectedValue + " And VF.ITEM_ID = " + DDItemName.SelectedValue;
        if (TDQuality.Visible == true && DDQuality.SelectedIndex > 0)
        {
            Str = Str + " And VF.QualityID = " + DDQuality.SelectedValue;
        }
        Str = Str + " Order By VF.DesignName";
        UtilityModule.ConditionalComboFill(ref DDDesign, Str, true, "--Select Design--");
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        DesignSelectedChange();
    }
    private void DesignSelectedChange()
    {
        string Str = @"Select Distinct VF.ColorID, VF.ColorName 
            From V_FinishedItemDetail VF(Nolock) 
            Where VF.CATEGORY_ID = " + DDCategoryName.SelectedValue + " And VF.ITEM_ID = " + DDItemName.SelectedValue + @" And 
            VF.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" And VF.ColorId > 0 And VF.SizeID > 0";
        if (TDQuality.Visible == true && DDQuality.SelectedIndex > 0)
        {
            Str = Str + " And VF.QualityID = " + DDQuality.SelectedValue;
        }
        if (TDDesign.Visible == true && DDDesign.SelectedIndex > 0)
        {
            Str = Str + " And VF.DesignID = " + DDDesign.SelectedValue;
        }
        Str = Str + " Order By VF.ColorName";
        UtilityModule.ConditionalComboFill(ref DDColor, Str, true, "--Select Color--");
    }
    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        ColorSelectedChange();
    }
    private void ColorSelectedChange()
    {
        string Str = @"Select Distinct VF.ShapeID, VF.ShapeName 
            From V_FinishedItemDetail VF(Nolock) 
            Where VF.CATEGORY_ID = " + DDCategoryName.SelectedValue + " And VF.ITEM_ID = " + DDItemName.SelectedValue + @" And 
            VF.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" And VF.SizeID > 0";
        if (TDQuality.Visible == true && DDQuality.SelectedIndex > 0)
        {
            Str = Str + " And VF.QualityID = " + DDQuality.SelectedValue;
        }
        if (TDDesign.Visible == true && DDDesign.SelectedIndex > 0)
        {
            Str = Str + " And VF.DesignID = " + DDDesign.SelectedValue;
        }
        if (TDColor.Visible == true && DDColor.SelectedIndex > 0)
        {
            Str = Str + " And VF.ColorID = " + DDColor.SelectedValue;
        }
        Str = Str + " Order By VF.ShapeName";

        UtilityModule.ConditionalComboFill(ref DDShape, Str, true, "--Select Shape--");
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShapeSelectedChange();
    }
    private void ShapeSelectedChange()
    {
        string Size = "VF.SizeFt";
        if (ChkForMtr.Checked == true)
        {
            Size = "VF.SizeMtr";
        }
        string Str = @"Select Distinct VF.SizeID, " + Size + @" Size 
            From V_FinishedItemDetail VF(Nolock) 
            Where VF.CATEGORY_ID = " + DDCategoryName.SelectedValue + " And VF.ITEM_ID = " + DDItemName.SelectedValue + @" And 
            VF.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" And VF.SizeID > 0";
        if (TDQuality.Visible == true && DDQuality.SelectedIndex > 0)
        {
            Str = Str + " And VF.QualityID = " + DDQuality.SelectedValue;
        }
        if (TDDesign.Visible == true && DDDesign.SelectedIndex > 0)
        {
            Str = Str + " And VF.DesignID = " + DDDesign.SelectedValue;
        }
        if (TDColor.Visible == true && DDColor.SelectedIndex > 0)
        {
            Str = Str + " And VF.ColorID = " + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            Str = Str + " And VF.ShapeID = " + DDShape.SelectedValue;
        }
        Str = Str + " Order By " + Size;

        UtilityModule.ConditionalComboFill(ref DDSize, Str, true, "--Select Size--");
    }
    protected void ChkForMtr_CheckedChanged(object sender, EventArgs e)
    {
        ShapeSelectedChange();
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        string DetailData = "";

        for (int i = 0; i < DGItemDescription.Rows.Count; i++)
        {
            TextBox TxtDays = ((TextBox)DGItemDescription.Rows[i].FindControl("TxtDays"));
            TextBox TxtHour = ((TextBox)DGItemDescription.Rows[i].FindControl("TxtHour"));
            TextBox TxtMinutes = ((TextBox)DGItemDescription.Rows[i].FindControl("TxtMinutes"));
            

            if (TxtDays.Text != "" || TxtHour.Text != "" || TxtMinutes.Text != "")
            {
                Label Item_Finished_ID = ((Label)DGItemDescription.Rows[i].FindControl("lblItem_Finished_ID"));

                DetailData = DetailData + Item_Finished_ID.Text + '|' + TxtDays.Text + '|' + TxtHour.Text + '|' + TxtMinutes.Text + '~';
            }
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[5];

            param[0] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
            param[1] = new SqlParameter("@DetailData", DetailData);
            param[2] = new SqlParameter("@UserID", Session["varuserid"]);
            param[3] = new SqlParameter("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
            param[4] = new SqlParameter("@Msg", SqlDbType.VarChar, 250);
            param[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SavePPlanningItemWiseDefineTime", param);
            Tran.Commit();
            if (param[4].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('" + param[4].Value + "');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('DATA SAVED SUCCESSFULLY.');", true);
            }
        }
        catch (Exception ex)
        {
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DGItemDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FillDetailBack();
    }
    protected void DGItemDescription_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGItemDescription, "Select$" + e.Row.RowIndex);
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {

    }
 
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblCategoryName.Text = ParameterList[5];
        lblItemName.Text = ParameterList[6];
        lblQualityName.Text = ParameterList[0];
        lblDesignName.Text = ParameterList[1];
        lblColorName.Text = ParameterList[2];
        lblShapeName.Text = ParameterList[3];
        lblSizeName.Text = ParameterList[4];
        LblShadeColor.Text = ParameterList[7];
    }
    protected void BtnShowData_Click(object sender, EventArgs e)
    {
        string Size = " VF.SizeFt";
        if (ChkForMtr.Checked == true)
        {
            Size = " VF.SizeMtr";
        }
        string str = @"Select VF.Item_Finished_ID, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName, " + Size + @" Size, VF.ShadeColorName, 
                    IsNull(PPIDT.[Days], '') [Days], IsNull(PPIDT.[Hours], '') [Hours], IsNull(PPIDT.[Minutes], '') [Minutes] 
                    From V_FinishedItemDetail VF(Nolock) 
                    LEFT JOIN PPlanningItemWiseDefineTime PPIDT(Nolock) ON PPIDT.Item_Finished_ID = VF.ITEM_FINISHED_ID And PPIDT.ProcessID = " + DDProcessName.SelectedValue + @" 
                    Where VF.DesignID > 0 And VF.ColorId > 0 And VF.SizeID > 0 And 
                    VF.CATEGORY_ID = " + DDCategoryName.SelectedValue + " And VF.Item_ID = " + DDItemName.SelectedValue;
       
        if (TDQuality.Visible == true && DDQuality.SelectedIndex > 0)
        {
            str = str + " And VF.QualityID = " + DDQuality.SelectedValue;
        }

        if (TDDesign.Visible == true && DDDesign.SelectedIndex > 0)
        {
            str = str + " And VF.DesignID = " + DDDesign.SelectedValue;
        }

        if (TDColor.Visible == true && DDColor.SelectedIndex > 0)
        {
            str = str + " And VF.ColorID = " + DDColor.SelectedValue;
        }

        if (TDShape.Visible == true && DDShape.SelectedIndex > 0)
        {
            str = str + " And VF.ShapeID = " + DDShape.SelectedValue;
        }

        if (TDSize.Visible == true && DDSize.SelectedIndex > 0)
        {
            str = str + " And VF.SizeID = " + DDSize.SelectedValue;
        }

        if (TDShadeColor.Visible == true && ddShadeColor.SelectedIndex > 0)
        {
            str = str + " And VF.ShadeColor = " + ddShadeColor.SelectedValue;
        }

        str = str + " Order By VF.CATEGORY_NAME, VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName, " + Size;

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            DGItemDescription.DataSource = Ds.Tables[0];
            DGItemDescription.DataBind();
        }
    }
}