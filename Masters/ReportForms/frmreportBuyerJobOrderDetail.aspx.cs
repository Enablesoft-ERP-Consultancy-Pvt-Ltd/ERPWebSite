using System;
using System.Web.UI;
using System.Data.SqlClient;
using System.Data;
using System.Text;
public partial class Masters_ReportForms_frmreportBuyerJobOrderDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.COmpanyid And CA.Userid=" + Session["varuserid"] + @"
                         select shapeid,shapeName from Shape order by ShapeId
                         select CustomerId,CustomerCode+'/'+CompanyName from Customerinfo Where MasterCompanyid=" + Session["varcompanyid"] + @" order by Customercode
                         select Process_Name_id, Process_Name From Process_Name_Master PN,V_JobProcessId V Where PN.Process_name_Id=V.ProcessId Order by Process_Name";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDShape, ds, 1, true, "--Plz Select Shape--");
            UtilityModule.ConditionalComboFillWithDS(ref DDcustomer, ds, 2, true, "--Plz Select Customer--");
            UtilityModule.ConditionalComboFillWithDS(ref DDprocessName, ds, 3, true, "--Plz Select Process--");

            RDjob_PurchaseDetail.Checked = true;

            ds.Dispose();
        }
    }
    protected void DDcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDOrder, "select OM.Orderid,OM.LocalOrder+'#'+OM.customerorderno as OrderNo from orderMaster OM Where OM.CustomerId=" + DDcustomer.SelectedValue + " And OM.CompanyId=" + DDCompany.SelectedValue + " and OM.Status=0 order by orderNo", true, "--Plz Select Order No--");
    }
    protected void DDprocessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDprocessName.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref DDEmployee, "select Distinct E.Empid,E.Empname From Empinfo E,V_JobEmpIdProcessId v Where V.Empid=E.EmpId And V.ProcessId=" + DDprocessName.SelectedValue + "", true, "--Plz Select Employee/Party--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDEmployee, "select Distinct E.Empid,E.Empname From Empinfo E,V_JobEmpIdProcessId v Where V.Empid=E.EmpId", true, "--Plz Select Employee/Party--");
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        if (RDjob_PurchaseDetail.Checked == true)
        {
            JOb_Purchasedetail();
        }
        else
        {
            OrderDetail();
        }
    }
    protected void JOb_Purchasedetail()
    {
        DataSet DS = new DataSet();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlParameter[] array = new SqlParameter[4];
            array[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
            array[1] = new SqlParameter("@OrderId", SqlDbType.Int);
            array[2] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            array[3] = new SqlParameter("@UserId", SqlDbType.Int);

            array[0].Value = DDCompany.SelectedValue;
            array[1].Value = DDOrder.SelectedValue;
            array[2].Value = Session["varcompanyId"];
            array[3].Value = Session["varuserid"];

            DS = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Pro_BuyerOrderStatus", array);
            if (DS.Tables[0].Rows.Count > 0)
            {
                Session["dsFilename"] = "~\\ReportSchema\\RptBuyerJobWiseStatus.xsd";
                Session["rptFilename"] = "Reports/RptBuyerJobWiseStatus.rpt";
                Session["GetDataset"] = DS;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
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
            DS.Dispose();
        }
    }
    protected void OrderDetail()
    {
        DataSet DS = new DataSet();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlParameter[] array = new SqlParameter[4];
            array[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
            array[1] = new SqlParameter("@OrderId", SqlDbType.Int);
            array[2] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            array[3] = new SqlParameter("@CustomerId", SqlDbType.Int);

            array[0].Value = DDCompany.SelectedValue;
            array[1].Value = DDOrder.SelectedIndex <= 0 ? "0" : DDOrder.SelectedValue;
            array[2].Value = Session["varcompanyId"];
            array[3].Value = DDcustomer.SelectedValue;

            DS = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "[Pro_OrderDetail]", array);

            if (DS.Tables[0].Rows.Count > 0)
            {
                Session["dsFilename"] = "~\\ReportSchema\\RptBuyerDetails.xsd";
                Session["rptFilename"] = "Reports/RptBuyerDetails.rpt";
                Session["GetDataset"] = DS;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
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
            DS.Dispose();
        }
    }
}