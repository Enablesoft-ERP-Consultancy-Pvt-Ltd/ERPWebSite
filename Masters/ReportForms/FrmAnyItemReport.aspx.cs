using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class Masters_ReportForms_FrmAnyItemReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                    from Companyinfo CI, Company_Authentication CA 
                    Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName 

                    Select Distinct VF.CATEGORY_ID, VF.CATEGORY_NAME 
                    From AnyItemIssueMasterDetail a 
                    JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = a.Item_Finished_ID 
                    Where a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " And a.CompanyID = " + Session["CurrentWorkingCompanyID"] + @" 
                    Order By VF.CATEGORY_NAME 

                    Select Val,Type from Sizetype

                    Select Distinct a.DepartmentID, D.DepartmentName  
                    From AnyItemIssueMasterDetail a
                    JOIN Department D ON D.DepartmentId = a.DepartmentID 
                    Where a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " And a.CompanyID = " + Session["CurrentWorkingCompanyID"] + @" 
                    Order By D.DepartmentName 

                    Select Distinct a.EmpID, EI.EmpName + case when Isnull(Ei.Empcode, '') = '' Then '' Else '[' + EI.Empcode + ']'  End EmpName 
                    From AnyItemIssueMasterDetail a
                    JOIN EmpInfo EI ON EI.EmpId = a.EmpID 
                    Where a.MasterCompanyID =" + Session["varMasterCompanyIDForERP"] + " And a.CompanyID = " + Session["CurrentWorkingCompanyID"] + @" 
                    Order By EmpName ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 2, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDDepartment, ds, 3, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 4, true, "--Select--");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = @"Select Distinct a.EmpID, EI.EmpName + case when Isnull(Ei.Empcode, '') = '' Then '' Else '[' + EI.Empcode + ']'  End EmpName 
                    From AnyItemIssueMasterDetail a
                    JOIN EmpInfo EI ON EI.EmpId = a.EmpID 
                    Where a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " And a.CompanyID = " + Session["CurrentWorkingCompanyID"] + @" 
                    And a.DepartmentID = " + DDDepartment.SelectedValue + @"
                    Order By EmpName ";
        UtilityModule.ConditionalComboFill(ref DDEmployeeName, Str, true, "--Plz Select--");

    }
    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = @"Select Distinct VF.CATEGORY_ID, VF.CATEGORY_NAME 
                    From AnyItemIssueMasterDetail a 
                    JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = a.Item_Finished_ID 
                    Where a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " And a.CompanyID = " + Session["CurrentWorkingCompanyID"] + @" 
                    And a.DepartmentID = " + DDDepartment.SelectedValue + " And a.EmpID = " + DDEmployeeName.SelectedValue + @" 
                    Order By VF.CATEGORY_NAME ";
        UtilityModule.ConditionalComboFill(ref DDCategory, Str, true, "--Plz Select--");
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
    }
    private void ddlcategorycange()
    {
        DDQuality.Items.Clear();
        DDDesign.Items.Clear();
        DDColor.Items.Clear();
        DDShape.Items.Clear();
        DDSize.Items.Clear();
        TRDDQuality.Visible = false;
        TRDDDesign.Visible = false;
        TRDDColor.Visible = false;
        TRDDShape.Visible = false;
        TRDDSize.Visible = false;
        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME 
                      FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on 
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] Where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        TRDDQuality.Visible = true;
                        break;
                    case "2":
                        TRDDDesign.Visible = true;
                        break;
                    case "3":
                        TRDDColor.Visible = true;
                        break;
                    case "4":
                        TRDDShape.Visible = true;
                        break;
                    case "5":
                        TRDDSize.Visible = true;
                        break;
                    case "6":
                        TRShadeColor.Visible = true;
                        break;
                }
            }
        }

        string stritem = @"Select Distinct VF.ITEM_ID, VF.ITEM_NAME
                    From AnyItemIssueMasterDetail a 
                    JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = a.Item_Finished_ID And VF.CATEGORY_ID = " + DDCategory.SelectedValue + @" 
                    Where a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " And a.CompanyID = " + Session["CurrentWorkingCompanyID"];
        if (DDDepartment.SelectedIndex > 0)
        {
            stritem = stritem + " And a.DepartmentID = " + DDDepartment.SelectedValue;
        }
        if (DDEmployeeName.SelectedIndex > 0)
        {
            stritem = stritem + " And a.EmpID = " + DDEmployeeName.SelectedValue;
        }
        stritem = stritem + " Order By VF.ITEM_NAME ";

        UtilityModule.ConditionalComboFill(ref ddItemName, stritem, true, "---Select Item----");
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(DDQuality, DDDesign, DDColor, DDShape, DDShadeColor);
    }
    private void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList ShadeColorName)
    {
        string Str = @"Select Distinct VF.QualityId, VF.QualityName 
                From AnyItemIssueMasterDetail a 
                JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = a.Item_Finished_ID And VF.CATEGORY_ID = " + DDCategory.SelectedValue + @" 
	                And VF.ITEM_ID = " + ddItemName.SelectedValue + @" 
                Where a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " And a.CompanyID = " + Session["CurrentWorkingCompanyID"];
        if (DDDepartment.SelectedIndex > 0)
        {
            Str = Str + " And a.DepartmentID = " + DDDepartment.SelectedValue;
        }
        if (DDEmployeeName.SelectedIndex > 0)
        {
            Str = Str + " And a.EmpID = " + DDEmployeeName.SelectedValue;
        }
        Str = Str + " Order By VF.QualityName ";

        Str = Str + @" Select Distinct VF.DesignId, VF.DesignName 
                From AnyItemIssueMasterDetail a 
                JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = a.Item_Finished_ID And VF.CATEGORY_ID = " + DDCategory.SelectedValue + @" 
	                And VF.ITEM_ID = " + ddItemName.SelectedValue + @" 
                Where a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " And a.CompanyID = " + Session["CurrentWorkingCompanyID"];
        if (DDDepartment.SelectedIndex > 0)
        {
            Str = Str + " And a.DepartmentID = " + DDDepartment.SelectedValue;
        }
        if (DDEmployeeName.SelectedIndex > 0)
        {
            Str = Str + " And a.EmpID = " + DDEmployeeName.SelectedValue;
        }
        Str = Str + " Order By VF.DesignName ";

        Str = Str + @" Select Distinct VF.ColorID, VF.ColorName 
                From AnyItemIssueMasterDetail a 
                JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = a.Item_Finished_ID And VF.CATEGORY_ID = " + DDCategory.SelectedValue + @" 
	                And VF.ITEM_ID = " + ddItemName.SelectedValue + @" 
                Where a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " And a.CompanyID = " + Session["CurrentWorkingCompanyID"];
        if (DDDepartment.SelectedIndex > 0)
        {
            Str = Str + " And a.DepartmentID = " + DDDepartment.SelectedValue;
        }
        if (DDEmployeeName.SelectedIndex > 0)
        {
            Str = Str + " And a.EmpID = " + DDEmployeeName.SelectedValue;
        }
        Str = Str + " Order By VF.ColorName ";

        Str = Str + @" Select Distinct VF.ShapeID, VF.ShapeName 
                From AnyItemIssueMasterDetail a 
                JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = a.Item_Finished_ID And VF.CATEGORY_ID = " + DDCategory.SelectedValue + @" 
	                And VF.ITEM_ID = " + ddItemName.SelectedValue + @" 
                Where a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " And a.CompanyID = " + Session["CurrentWorkingCompanyID"];
        if (DDDepartment.SelectedIndex > 0)
        {
            Str = Str + " And a.DepartmentID = " + DDDepartment.SelectedValue;
        }
        if (DDEmployeeName.SelectedIndex > 0)
        {
            Str = Str + " And a.EmpID = " + DDEmployeeName.SelectedValue;
        }
        Str = Str + " Order By VF.ShapeName ";

        Str = Str + @" Select Distinct VF.ShadeColorID, VF.ShadeColorName 
                From AnyItemIssueMasterDetail a 
                JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = a.Item_Finished_ID And VF.CATEGORY_ID = " + DDCategory.SelectedValue + @" 
	                And VF.ITEM_ID = " + ddItemName.SelectedValue + @" 
                Where a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " And a.CompanyID = " + Session["CurrentWorkingCompanyID"];
        if (DDDepartment.SelectedIndex > 0)
        {
            Str = Str + " And a.DepartmentID = " + DDDepartment.SelectedValue;
        }
        if (DDEmployeeName.SelectedIndex > 0)
        {
            Str = Str + " And a.EmpID = " + DDEmployeeName.SelectedValue;
        }
        Str = Str + " Order By VF.ShadeColorName ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref Quality, ds, 0, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Design, ds, 1, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Color, ds, 2, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Shape, ds, 3, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref ShadeColorName, ds, 4, true, "--SELECT--");
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void FillSize()
    {
        string size = "";
        string str = "";

        switch (DDsizetype.SelectedValue)
        {
            case "1":
                size = "Sizemtr";
                break;
            case "0":
                size = "Sizeft";
                break;
            case "2":
                size = "Sizeinch";
                break;
            default:
                size = "Sizeft";
                break;
        }
        //size Query

        str = "Select Distinct S.Sizeid,S." + size + " As  " + size + @" From Size S 
                 Where shapeid=" + DDShape.SelectedValue + " And S.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by " + size + "";

        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--SELECT--");

    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string Where = "";
        lblmsg.Text = "";
        if (DDCategory.SelectedIndex > 0)
        {
            Where = Where + " And VF.CATEGORY_ID = " + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            Where = Where + " And VF.Item_ID = " + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            Where = Where + " And VF.QualityID = " + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            Where = Where + " And VF.DesignID = " + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            Where = Where + " And VF.ColorID = " + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            Where = Where + " And VF.ShapeID = " + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            Where = Where + " And VF.SizeID = " + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            Where = Where + " And VF.ShadeColorID = " + DDShadeColor.SelectedValue;
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_GetAnyItemIssueDetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 120;
        //********
        cmd.Parameters.AddWithValue("@CompanyId", DDcompany.SelectedValue);
        cmd.Parameters.AddWithValue("@DepartmentID", DDDepartment.SelectedValue);
        cmd.Parameters.AddWithValue("@EmpID", DDEmployeeName.SelectedValue);
        cmd.Parameters.AddWithValue("@FromDate", txtfromdate.Text);
        cmd.Parameters.AddWithValue("@ToDate", txttodate.Text);
        cmd.Parameters.AddWithValue("@Where", Where);

        DataTable dt = new DataTable();
        dt.Load(cmd.ExecuteReader());
        //*************
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);

        if (ds.Tables[0].Rows.Count > 0)
        {
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;

            GridView1.DataSource = ds;
            GridView1.DataBind();
            lblmsg.Text = "Wait....";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
             "attachment;filename = Any Item Issue Detail " + DateTime.Now + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                //Apply text style to each Row
                GridView1.Rows[i].Attributes.Add("class", "textmode");
            }
            GridView1.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            lblmsg.Text = "Done.....";
            //*************

        }
        else
        {
            lblmsg.Text = "No Records found...";
        }
    }
}