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


public partial class Masters_ReportForms_FrmBazaarTdsDetails : System.Web.UI.Page
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
                           select Process_Name_id,Process_Name from Process_Name_Master where Process_Name_id=1 ";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDWeaver, ds, 1, true, "---Plz Select---");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 2, true, "---Plz Select---");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedIndex = 1;                
            }
           
            txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }   
    protected void DDWeaver_SelectedIndexChanged(object sender, EventArgs e)
    {
       // FillFolioNo();
    }    
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";       
        BazaarTdsDetails();
       


        //if (chkSizeColorWise.Checked == true)
        //{
        //    BazaarPaymentDetailsSizeColorWise();            
        //}
        //else
        //{
        //    BazaarPaymentDetails();
        //}
    }
    protected void BazaarTdsDetails()
    {

        lblmsg.Text = "";
        try
        {
            string str = "";
            if (DDDeductionType.SelectedIndex > 0)
            {
                if (DDDeductionType.SelectedIndex == 1)
                {
                    str = str + " and PRD.TDSAmt>0";
                }
                else if (DDDeductionType.SelectedIndex == 2)
                {
                    str = str + " and PRD.TDSAmt=0";
                }                
            }
            if (DDWeaver.SelectedIndex > 0)
            {
                str = str + " and PRM.EmpId=" + DDWeaver.SelectedValue;
            }            
            if (ChkselectDate.Checked == true)
            {
                str = str + " and PRM.RECEIVEDATE>='" + txtfromDate.Text + "' and PRM.RECEIVEDATE<='" + txttodate.Text + "'";                
            }           
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@Processid", DDProcessName.SelectedValue);
            param[2] = new SqlParameter("@where", str);
            param[3] = new SqlParameter("@FromDate", txtfromDate.Text);
            param[4] = new SqlParameter("@ToDate", txttodate.Text);
           //// param[5] = new SqlParameter("@ChkNameWise", chkNameWise.Checked == true ? 1 : 0);
           //// param[6] = new SqlParameter("@ChkVoucher", chkVoucher.Checked == true ? 1 : 0); 


            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETWEAVINGBAZAARTDSDETAILS", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ChkForSummary.Checked == true)
                {
                    Session["rptFileName"] = "~\\Reports\\RptBazaarTDSSummary.rpt";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\RptBazaarTDSDetails.rpt";
                }
                               
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptBazaarTDSDetails.xsd";

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
    
    
    //protected void chkNameWise_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chkNameWise.Checked == true)
    //    {
    //        chkVoucher.Checked = false;
    //        chkSizeColorWise.Checked = false;
    //    }
       
    //}
    
}