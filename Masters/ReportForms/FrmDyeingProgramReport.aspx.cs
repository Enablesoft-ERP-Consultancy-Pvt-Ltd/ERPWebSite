using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_FrmDyeingProgramReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDCompany, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName", true, "--Select--");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                UtilityModule.ConditionalComboFill(ref ddcustomer, "SELECT DISTINCT CI.Customerid,CI.Customercode + SPACE(5) +CompanyName from customerinfo CI,Ordermaster OM,OrderDetail OD,JobAssigns JA Where CI.Customerid=OM.Customerid And OD.OrderId=OM.OrderId And OD.OrderId=JA.OrderId And OD.Item_Finished_Id=JA.Item_Finished_Id And PreProdAssignedQty>0 And OD.Tag_Flag=1 and Companyid=" + DDCompany.SelectedValue + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CustomerId", true, "--Select--");
            }

            txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDCompany.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref ddcustomer, "SELECT DISTINCT CI.Customerid,CI.Customercode + SPACE(5)+CompanyName from customerinfo CI,Ordermaster OM,OrderDetail OD,JobAssigns JA Where CI.Customerid=OM.Customerid And OD.OrderId=OM.OrderId And OD.OrderId=JA.OrderId And OD.Item_Finished_Id=JA.Item_Finished_Id And PreProdAssignedQty>0 And OD.Tag_Flag=1 and Companyid=" + DDCompany.SelectedValue + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CustomerId", true, "--Select--");
        }
    }
    protected void ddcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCustomerOrder();
    }
    private void BindCustomerOrder()
    {
//        string str1 = @"Select OM.OrderId,LocalOrder+' / '+CustomerOrderNo+' / '+CustomerCode+' / '+replace(convert(varchar(11),ProdReqDate,106), ' ','-') as OrderNo 
//                          From CustomerInfo CI,Ordermaster OM,OrderDetail OD,JobAssigns JA Where CI.Customerid=OM.Customerid And OD.OrderId=OM.OrderId And 
//                          OD.OrderId=JA.OrderId And OD.Item_Finished_Id=JA.Item_Finished_Id And PreProdAssignedQty>0 And OD.Tag_Flag=1 And 
//                          OM.CustomerId=" + ddcustomer.SelectedValue + @" And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And OM.OrderId Not IN(Select Order_Id From ProcessProgram WHERE Process_ID=" + ddprocess.SelectedValue + @") 
//                          Group By  OM.Orderid,LocalOrder,CustomerOrderNo,CustomerCode,ProdReqDate Order By ProdReqDate ASC";
        string str = "";
        if (ChkselectDate.Checked == true)
        {
            str = str + " and OM.OrderDate>='" + txtfromDate.Text + "' and OM.OrderDate<='" + txttodate.Text + "'";
        }

        string str1 = @"Select OM.OrderId,LocalOrder+' / '+CustomerOrderNo+' / '+CustomerCode+' / '+replace(convert(varchar(11),ProdReqDate,106), ' ','-') as OrderNo 
                          From CustomerInfo CI,Ordermaster OM,OrderDetail OD,JobAssigns JA Where OM.Status = 0 And CI.Customerid=OM.Customerid And OD.OrderId=OM.OrderId And 
                          OD.OrderId=JA.OrderId And OD.Item_Finished_Id=JA.Item_Finished_Id And PreProdAssignedQty>0 And OD.Tag_Flag=1 And 
                          OM.CustomerId=" + ddcustomer.SelectedValue + @" And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + str +@" 
                          Group By  OM.Orderid,LocalOrder,CustomerOrderNo,CustomerCode,ProdReqDate Order By ProdReqDate ASC";
        UtilityModule.ConditonalChkBoxListFill(ref chekboxlist, str1);
    }
    protected void chekboxlist_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public string OrderId = "";
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        DataSet ds;
        lblErrmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[4];
            _arrPara[0] = new SqlParameter("@Order_Id", SqlDbType.VarChar, 50);            
            _arrPara[1] = new SqlParameter("@varuserid", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@varCompanyId", SqlDbType.Int);
            //_arrPara[3] = new SqlParameter("@PROCESSID", SqlDbType.Int);

            int n = chekboxlist.Items.Count;
            for (int i = 0; i < n; i++)
            {
                if (chekboxlist.Items[i].Selected)
                {
                    //_arrPara[2].Value = chekboxlist.Items[i].Value;
                    OrderId += chekboxlist.Items[i].Value + ",";                   
                }
            }

            _arrPara[0].Value = OrderId;           
            _arrPara[1].Value = Session["varuserid"].ToString();
            _arrPara[2].Value = Session["varMasterCompanyIDForERP"].ToString();
            //_arrPara[3].Value = 1;

            ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetReportDyeingProcessProgQtyDetails", _arrPara);

            if (ds.Tables[0].Rows.Count > 0)
            {
                
                 Session["rptFileName"] = "~\\Reports\\RptDyeingProgramDetailMaltiRugs.rpt";
                 Session["GetDataset"] = ds;
                 Session["dsFileName"] = "~\\ReportSchema\\RptDyeingProgramDetailMaltiRugs.xsd";
               
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Information", "alert('No records found');", true);
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = ex.Message;
            Tran.Rollback();

        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
}