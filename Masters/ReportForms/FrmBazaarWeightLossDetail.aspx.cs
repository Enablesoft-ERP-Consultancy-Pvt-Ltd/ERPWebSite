using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ClosedXML.Excel;
using System.IO;


public partial class Masters_ReportForms_FrmBazaarWeightLossDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
            string str = @"select CompanyId,CompanyName From Companyinfo CI where MasterCompanyid=" + Session["varcompanyId"] + @" order by CompanyId
                           select EI.EmpId,EI.EmpName + case When isnull(Ei.empcode,'')='' then '' else ' ['+EI.empcode+']' end as Empname  From EmpInfo  EI inner Join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentName='PRODUCTION' and Ei.MastercompanyId=" + Session["varcompanyId"] + @" INNER JOIN EmpProcess EP ON EP.Empid=EI.EmpId and EP.ProcessId=1 order by EmpName
                           select CATEGORY_ID,CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM 
                           Select CustomerId,CustomerCode From CustomerInfo Where MasterCompanyId=" + Session["varCompanyId"] + " Order By CustomerCode";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDWeaver, ds, 1, true, "---Plz Select---");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, "---Plz Select---");
            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerName, ds, 3, true, "---Plz Select---");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
            if (DDCategory.Items.Count > 0)
            {
                DDCategory.SelectedIndex = 1;
                BindCategory();
            }

            txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

        }
    }
    protected void DDQtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQuality();
        DDDesign.Items.Clear();
        DDSize.Items.Clear();
    }
    protected void FillQuality()
    {
        string str = @"select Distinct Q.QualityId,Q.QualityName From ITEM_MASTER IM inner Join Quality Q on Q.Item_Id=IM.ITEM_ID inner Join CategorySeparate cs on IM.CATEGORY_ID=cs.Categoryid where 1=1";
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + "  and IM.Category_id=" + DDCategory.SelectedValue;
        }
        if (DDQtype.SelectedIndex > 0)
        {
            str = str + "  and IM.Item_id=" + DDQtype.SelectedValue;
        }
        str = str + "  order by QualityName";
        UtilityModule.ConditionalComboFill(ref DDQuality, str, true, "---Plz Select---");
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDesign();
    }
    protected void FillDesign()
    {
        string str = @"select Distinct Vf.designId,vf.designName From V_finishedItemDetailnew Vf Where Vf.designId>0";
        if (DDQtype.SelectedIndex > 0)
        {
            str = str + "  and vf.Item_id=" + DDQtype.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
        }

        str = str + "  order by designName";
        UtilityModule.ConditionalComboFill(ref DDDesign, str, true, "---Plz Select---");
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";

        if (chkDesignWiseWeightLossDetails.Checked == true)
        {
            BazaarDesignWiseWeightLossDetails();
        }
        else
        {
            BazaarWeightLossDetails();
        }
    }
    protected void BazaarWeightLossDetails()
    {

        lblmsg.Text = "";
        try
        {
            string str = "";

            if (DDWeaver.SelectedIndex > 0)
            {
                str = str + " and PRM.EmpId=" + DDWeaver.SelectedValue;
            }
            if (DDCustomerName.SelectedIndex > 0)
            {
                str = str + " and CC.CustomerId=" + DDCustomerName.SelectedValue;
            }
            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and Vf.Item_id=" + DDQtype.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and Vf.Qualityid=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
            }
            //if (DDColor.SelectedIndex > 0)
            //{
            //    str = str + " and vf.Colorid=" + DDColor.SelectedValue;                
            //}
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
            }
            if (ChkselectDate.Checked == true)
            {
                str = str + " and PRM.RECEIVEDATE>='" + txtfromDate.Text + "' and PRM.RECEIVEDATE<='" + txttodate.Text + "'";
            }
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@Processid", 1);
            param[2] = new SqlParameter("@where", str);
            param[3] = new SqlParameter("@FromDate", txtfromDate.Text);
            param[4] = new SqlParameter("@ToDate", txttodate.Text);
            //// param[5] = new SqlParameter("@ChkNameWise", chkNameWise.Checked == true ? 1 : 0);
            ////param[6] = new SqlParameter("@ChkVoucher", chkVoucher.Checked == true ? 1 : 0);          

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETWEAVINGBAZAARWEIGHTLOSSDETAILS", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //if (chkVoucher.Checked == true)
                //{
                //    Session["rptFileName"] = "~\\Reports\\RptWeaverVoucher.rpt";
                //}
                //else
                //{
                //    Session["rptFileName"] = "~\\Reports\\RptBazaarPaymentDetails.rpt";
                //}

                Session["rptFileName"] = "~\\Reports\\RptBazaarWeightLossDetails.rpt";

                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptBazaarWeightLossDetails.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void BazaarDesignWiseWeightLossDetails()
    {
        lblmsg.Text = "";
        try
        {
            string str = "";

            if (DDWeaver.SelectedIndex > 0)
            {
                str = str + " and PRM.EmpId=" + DDWeaver.SelectedValue;
            }
            if (DDCustomerName.SelectedIndex > 0)
            {
                str = str + " and CC.CustomerId=" + DDCustomerName.SelectedValue;
            }
            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and Vf.Item_id=" + DDQtype.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and Vf.Qualityid=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
            }
            //if (DDColor.SelectedIndex > 0)
            //{
            //    str = str + " and vf.Colorid=" + DDColor.SelectedValue;                
            //}
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
            }
            if (ChkselectDate.Checked == true)
            {
                str = str + " and PRM.RECEIVEDATE>='" + txtfromDate.Text + "' and PRM.RECEIVEDATE<='" + txttodate.Text + "'";
            }
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@Processid", 1);
            param[2] = new SqlParameter("@where", str);
            param[3] = new SqlParameter("@FromDate", txtfromDate.Text);
            param[4] = new SqlParameter("@ToDate", txttodate.Text);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETWEAVINGBAZAARDESIGNWISEWEIGHTLOSSDETAILS", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptBazaarDesignWiseWeightLossDetails.rpt";

                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptBazaarDesignWiseWeightLossDetails.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void BindCategory()
    {
        UtilityModule.ConditionalComboFill(ref DDQtype, "select IM.ITEM_ID,IM.ITEM_NAME From Item_Master IM Where IM.category_id=" + DDCategory.SelectedValue + " order by ITEM_NAME", true, "--Plz Select--");
        Fillcombo();
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCategory();
    }
    protected void Fillcombo()
    {
        Trquality.Visible = false;
        //Trdesign.Visible = false;
        //Trcolor.Visible = false;
        //Trsize.Visible = false;
        //Trshadecolor.Visible = false;
        string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                  " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                  " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        Trquality.Visible = false;
                        break;
                    case "2":
                        Trdesign.Visible = false;
                        break;
                    case "3":
                        Trcolor.Visible = false;
                        break;
                    case "5":
                        Trsize.Visible = false;
                        break;
                    case "6":
                        Trshadecolor.Visible = false;
                        break;
                }
            }
        }

    }
}