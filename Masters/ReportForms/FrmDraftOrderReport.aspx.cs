using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using ClosedXML.Excel;

public partial class Masters_ReportForms_FrmDraftOrderReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by Companyname";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                CompanySelectedChange();
            }
        }
    }
    private void CompanySelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDCustCode, "Select CustomerId,CustomerCode From CustomerInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By CustomerCode", true, "--Select--");
    }
    protected void DDCustCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDDraftOrderNo, "Select OrderId,OrderNo From DRAFT_ORDER_MASTER where CustomerId=" + DDCustCode.SelectedValue + " And CompanyId=" + DDCompany.SelectedValue + " Order By orderno", true, "--Select--");

    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        DraftOrderReport();
    }

    private void DraftOrderReport()
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@orderid", DDDraftOrderNo.SelectedValue);
        param[1] = new SqlParameter("@MasterCompanyId", Session["VarcompanyNo"]);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETDRAFTORDERREPORTDETAIL", param);

        //if (ds.Tables[2].Rows.Count > 0)
        //{
        ds.Tables[2].TableName = "Table3";
        //}
        ds.Tables[1].TableName = "Table2";
        ds.Tables[0].TableName = "Table1";

        ds.Tables[0].Columns.Add("ImageName", typeof(System.Byte[]));
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (Convert.ToString(dr["Photo"]) != "")
            {
                FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                if (TheFile.Exists)
                {
                    string img = dr["Photo"].ToString();
                    img = Server.MapPath(img);
                    Byte[] img_Byte = File.ReadAllBytes(img);
                    dr["ImageName"] = img_Byte;
                }
            }
        }


        ds.Tables[0].Columns.Add("CompanyLogo2", typeof(System.Byte[]));
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (Convert.ToString(dr["CompanyLogo"]) != "")
            {
                FileInfo TheFile = new FileInfo(Server.MapPath(dr["CompanyLogo"].ToString()));
                if (TheFile.Exists)
                {
                    string img = dr["CompanyLogo"].ToString();
                    img = Server.MapPath(img);
                    Byte[] img_Byte = File.ReadAllBytes(img);
                    dr["CompanyLogo2"] = img_Byte;
                }
            }
        }

        //string STR1 = @"select OrderDetailId,Photo from Draft_Order_ReferenceImage where OrderDetailId in(select orderdetailid from draft_order_detail where orderid=" + ViewState["orderid"] + ") ";
        //SqlDataAdapter sda1 = new SqlDataAdapter(STR1, con);
        //DataTable dt1 = new DataTable();
        //sda1.Fill(dt1);
        //ds.Tables.Add(dt1);
        ds.Tables[1].Columns.Add("RefImageName", typeof(System.Byte[]));
        if (ds.Tables[1].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                if (Convert.ToString(dr["Photo"]) != "")
                {
                    FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                    if (TheFile.Exists)
                    {
                        string img = dr["Photo"].ToString();
                        img = Server.MapPath(img);
                        Byte[] img_Byte = File.ReadAllBytes(img);
                        dr["RefImageName"] = img_Byte;
                    }
                }
            }
        }

        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\RptDraftOrderReportDetail.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptDraftOrderReportDetail.xsd";
            Session["GetDataset"] = ds;

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
        ds.Dispose();
    }

    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedChange();
    }

}