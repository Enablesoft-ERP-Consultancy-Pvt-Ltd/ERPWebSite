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


public partial class Masters_ReportForms_FrmBazaarPaymentDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
            string str = @"select CompanyId,CompanyName From Companyinfo CI where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by CompanyId
                           select EI.EmpId,EI.EmpName + case When isnull(Ei.empcode,'')='' then '' else ' ['+EI.empcode+']' end as Empname  From EmpInfo  EI inner Join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentName='PRODUCTION' and Ei.MastercompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN EmpProcess EP ON EP.Empid=EI.EmpId and EP.ProcessId=1 order by EmpName
                           select CATEGORY_ID,CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM 
                           Select CustomerId,CustomerCode From CustomerInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By CustomerCode";
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
    protected void DDWeaver_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillFolioNo();
    }
    protected void FillFolioNo()
    {
        string str = "";
        str = @"Select distinct PRM.Process_Rec_Id,PRM.ChallanNo from Process_Receive_Master_1 PRM where PRM.CompanyId=" + DDCompany.SelectedValue + @" and PRM.EmpId=" + DDWeaver.SelectedValue + @" and PRM.FinalBazaarFlag=1";
        if (DDReportType.SelectedIndex > 0)
        {
            if (DDReportType.SelectedIndex == 1)
            {
                str = str + " and PRM.SampleFlag=0";
            }
            else if (DDReportType.SelectedIndex == 2)
            {
                str = str + " and PRM.SampleFlag=1";
            }
        }
        UtilityModule.ConditionalComboFill(ref DDFolioNo, str, true, "---Plz Select---");
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
        FillSize();
        //if (Trshadecolor.Visible == true)
        //{
        //    FillShade();
        //}
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
    protected void FillSize()
    {
        string size = "ProdSizeFt";
        if (Chkmtrsize.Checked == true)
        {
            size = "ProdSizemtr";
        }

        string str = @"select Distinct  Vf.Sizeid,vf." + size + " as Size  From V_finishedItemDetailnew Vf Where Vf.Sizeid>0";
        if (DDQtype.SelectedIndex > 0)
        {
            str = str + "  and vf.Item_id=" + DDQtype.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + "  and vf.DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + "  and vf.Colorid=" + DDColor.SelectedValue;
        }

        str = str + "  order by Size";
        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "---Plz Select---");
    }
    //protected void Chkmtrsize_CheckedChanged(object sender, EventArgs e)
    //{
    //    FillSize();
    //}
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        if (chkSizeColorWise.Checked == true)
        {
            BazaarPaymentDetailsSizeColorWise();            
        }
        else
        {
            BazaarPaymentDetails();
        }
    }
    protected void BazaarPaymentDetails()
    {

        lblmsg.Text = "";
        try
        {
            string str = "";            
            if (DDReportType.SelectedIndex > 0)
            {
                if (DDReportType.SelectedIndex ==1)
                {
                    str = str + " and PRM.SampleFlag=0";
                }
                else if(DDReportType.SelectedIndex == 2)
                {
                    str = str + " and PRM.SampleFlag=1";
                }                
            }
            if (DDWeaver.SelectedIndex > 0)
            {
                str = str + " and PRM.EmpId=" + DDWeaver.SelectedValue;
            }
            if (DDFolioNo.SelectedIndex > 0)
            {
                str = str + " and PRM.Process_Rec_Id=" + DDFolioNo.SelectedValue;               
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
            param[5] = new SqlParameter("@ChkNameWise", chkNameWise.Checked == true ? 1 : 0);
            param[6] = new SqlParameter("@ChkVoucher", chkVoucher.Checked == true ? 1 : 0);            
            
           // param[3] = new SqlParameter("@EMpid", (DDWeaver.SelectedIndex > 0 ? DDWeaver.SelectedValue : "0"));

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETWEAVINGBAZAARPAYMENTDETAILS", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (chkVoucher.Checked == true)
                {
                    Session["rptFileName"] = "~\\Reports\\RptWeaverVoucher.rpt";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\RptBazaarPaymentDetails.rpt";
                }
                               
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptBazaarPaymentDetails.xsd";

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
    protected void BazaarPaymentDetailsSizeColorWise()
    {
        lblmsg.Text = "";
        try
        {
            string str = "";            
            if (DDReportType.SelectedIndex > 0)
            {
                if (DDReportType.SelectedIndex == 1)
                {
                    str = str + " and PRM.SampleFlag=0";
                }
                else if (DDReportType.SelectedIndex == 2)
                {
                    str = str + " and PRM.SampleFlag=1";
                }
            }
            if (DDWeaver.SelectedIndex > 0)
            {
                str = str + " and PRM.EmpId=" + DDWeaver.SelectedValue;
            }
            if (DDFolioNo.SelectedIndex > 0)
            {
                str = str + " and PRM.Process_Rec_Id=" + DDFolioNo.SelectedValue;
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
            param[5] = new SqlParameter("@ChkNameWise", chkNameWise.Checked == true ? 1 : 0);
            param[6] = new SqlParameter("@ChkVoucher", chkVoucher.Checked == true ? 1 : 0);
            // param[3] = new SqlParameter("@EMpid", (DDWeaver.SelectedIndex > 0 ? DDWeaver.SelectedValue : "0"));

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETWEAVINGBAZAARPAYMENTDETAILSCOLORSIZEWISE", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptColorWiseBazarSummary.rpt";               
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptColorWiseBazarSummary.xsd";

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
                  " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        Trquality.Visible = true;
                        break;
                    case "2":
                        Trdesign.Visible = true;
                        break;
                    case "3":
                        Trcolor.Visible = false;
                        break;
                    case "5":
                        Trsize.Visible = true;
                        break;
                    case "6":
                        Trshadecolor.Visible = true;
                        break;
                }
            }
        }

    }   
    
    protected void chkNameWise_CheckedChanged(object sender, EventArgs e)
    {
        if (chkNameWise.Checked == true)
        {
            chkVoucher.Checked = false;
            chkSizeColorWise.Checked = false;
        }
       
    }
    protected void chkVoucher_CheckedChanged(object sender, EventArgs e)
    {
        if (chkVoucher.Checked == true)
        {
            chkNameWise.Checked = false;
            chkSizeColorWise.Checked = false;
        }
    }
    protected void chkSizeColorWise_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSizeColorWise.Checked == true)
        {
            chkNameWise.Checked = false;
            chkVoucher.Checked = false;
        }
    }
}