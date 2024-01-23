using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;

public partial class Masters_Order_OrderStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = "SELECT DISTINCT CATEGORY_ID,CATEGORY_NAME From item_category_master Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                          select CustomerId,CustomerCode+' / '+CompanyName as Customer from customerinfo Where mastercompanyid=" + Session["varMasterCompanyIDForERP"] + " order by customer";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, ds, 0, true, "-ALL-");
            UtilityModule.ConditionalComboFillWithDS(ref DDbuyer, ds, 1, true, "-Select-");
            FillCheckorders(0);
            FillChangestatus(0);
            fillgrid();
            if (variable.Carpetcompany == "1")
            {
                if (ddordertype.Items.FindByText("Production Status") != null)
                {
                    var index = ddordertype.Items.IndexOf(ddordertype.Items.FindByText("Production Status"));
                    ddordertype.Items[index].Text = "Dyeing Status";

                }
                //********add Production  status
                ddordertype.Items.Add(new ListItem("Production Status", "3"));
            }
            //*******

        }
    }
    private void fillgrid()
    {
        string str = "";
        string qry = "";
        if (ddCatagory.SelectedIndex > 0)
        {
            qry = " and v.CATEGORY_ID=" + ddCatagory.SelectedValue + " ";
        }
        if (DDbuyer.SelectedIndex > 0)
        {
            qry = qry + " and OM.customerid=" + DDbuyer.SelectedValue + " ";
        }
        if (ddordertype.SelectedValue == "0")
        {
            str = @"select distinct LocalOrder+'/'+ CustomerOrderNo as OrderNo,om.orderid,om.Remarks as Remark 
                   From OrderMaster om  inner join orderdetail od On om.orderid=od.orderid inner join 
                   V_FinishedItemDetail v On od.Item_Finished_Id= v.Item_Finished_Id 
                   Where  om.Status=" + DDStatus.SelectedValue + " And V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " " + qry + " order by OM.orderid";
        }
        else if (ddordertype.SelectedValue == "1")
        {
            str = @"select distinct 'P.O.No '+Challanno+'  '+LocalOrder+'/'+ CustomerOrderNo as OrderNo,od.PindentIssueid as orderid,om.Remarks as Remark 
                  From OrderMaster om  inner join PurchaseIndentIssue od On om.orderid=od.orderid inner join
                  v_Order_category v On v.orderid=od.orderid
                  where  od.Status='" + DDStatus.SelectedItem.Text + "' and MasterCompanyId=" + Session["varcompanyno"] + " " + qry + " order by orderid";
        }
        else if (ddordertype.SelectedValue == "2")
        {
            str = @"Select distinct 'I.No '+IndentNo+'  '+LocalOrder+'/'+ CustomerOrderNo as OrderNo,im.indentid as orderid,om.Remarks as Remark 
                  From OrderMaster om  inner join indentdetail od On om.orderid=od.orderid inner join 
                  Indentmaster im On od.indentid=im.indentid inner join v_Order_category v On v.orderid=od.orderid
                  Where  im.Status='" + DDStatus.SelectedItem.Text + "' and MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " " + qry + " order by orderid";
        }
        else if (ddordertype.SelectedValue == "3")
        {
            str = @"select Distinct  orderNo,orderid,Remark From 
                (SELECT DISTINCT PIM.IssueOrderId as orderNo,PIM.Issueorderid as orderid,PIM.Remarks as Remark  FROM PROCESS_ISSUE_MASTER_1  PIM INNER JOIN EMPINFO EI ON PIM.EMPID=EI.EMPID
                Where PIM.Status='" + DDStatus.SelectedItem.Text + @"'";
            if (DDweaver.SelectedIndex > 0)
            {
                str = str + " and Pim.empid=" + DDweaver.SelectedValue;
            }
            else
            {
                str = str + " and Pim.empid=0";
            }

            str = str + @"   UNION ALL
                SELECT DISTINCT PIM.IssueOrderId as orderNo,PIM.Issueorderid as orderid,PIM.Remarks as Remark
                FROM EMPLOYEE_PROCESSORDERNO EMP INNER JOIN Process_issue_master_1 PIM on EMP.issueorderid=PIM.issueorderid inner join EMPINFO EI ON EMP.EMPID=EI.EMPID WHERE PROCESSID=1
                AND PIM.Status='" + DDStatus.SelectedItem.Text + "' ";
            if (DDweaver.SelectedIndex > 0)
            {
                str = str + " and Ei.empid=" + DDweaver.SelectedValue;
            }
            else
            {
                str = str + " and Ei.empid=0";
            }
            str = str + " ) A";

            str = str + " order by orderNo ";
        }

        if (str != "")
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            {
                DGOrderDetail.DataSource = ds;
                DGOrderDetail.DataBind();
            }
        }

    }
    protected void DGOrderDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        string remark = "";
        //sql Table Type
        DataTable dt = new DataTable();
        dt.Columns.Add("Remark", typeof(string));
        dt.Columns.Add("Orderid", typeof(int));

        for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
        {
            if (((CheckBox)DGOrderDetail.Rows[i].FindControl("Chkbox")).Checked == true)
            {
                remark = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtRemark")).Text;
                DataRow dr = dt.NewRow();
                dr["remark"] = remark;
                dr["orderid"] = DGOrderDetail.DataKeys[i].Value;
                dt.Rows.Add(dr);
            }
        }
        if (dt.Rows.Count > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@dt", dt);
                param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[1].Direction = ParameterDirection.Output;
                param[2] = new SqlParameter("@userid", Session["varuserid"]);
                param[3] = new SqlParameter("@Mastercompanyid", Session["varcompanyNo"]);
                param[4] = new SqlParameter("@Status", ddordertype.SelectedValue == "0" ? ddststatuschange.SelectedValue : ddststatuschange.SelectedItem.Text);
                param[5] = new SqlParameter("@OrderType", ddordertype.SelectedValue);
                //****
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_CHANGEORDERSTATUS", param);
                Tran.Commit();
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + param[1].Value.ToString() + "');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('" + ex.Message + "');", true);
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            #region
            //for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
            //{
            //    if (((CheckBox)DGOrderDetail.Rows[i].FindControl("Chkbox")).Checked == true)
            //    {
            //        remark = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtRemark")).Text;
            //        if (ddordertype.SelectedValue == "0")
            //        {
            //            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update ordermaster set status=" + ddststatuschange.SelectedValue + " , Remarks='" + remark + "'  where orderid =" + DGOrderDetail.DataKeys[i].Value + "");
            //        }
            //        else if (ddordertype.SelectedValue == "1")
            //        {
            //            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update PurchaseIndentIssue set status='" + ddststatuschange.SelectedItem.Text + "'  where PindentIssueid =" + DGOrderDetail.DataKeys[i].Value + "");
            //        }
            //        else
            //        {
            //            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update indentmaster set status='" + ddststatuschange.SelectedItem.Text + "'  where  indentid =" + DGOrderDetail.DataKeys[i].Value + "");
            //        }

            //    }
            //}
            #endregion
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Nodata", "alert('Please select atleast one checkbox')", true);
        }
        //SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update ordermaster set status=" + ddststatuschange.SelectedValue+ " where orderid in(" + CustOrderNo + ")");

        saverefresh();
    }
    protected void DDStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }
    private void saverefresh()
    {
        fillgrid();
        if (ddststatuschange.Items.FindByValue("0") != null)
        {
            ddststatuschange.SelectedValue = "0";
        }
        if (DDStatus.Items.FindByValue("0") != null)
        {
            DDStatus.SelectedValue = "0";
        }

    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }
    protected void ddordertype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Tdemp.Visible = false;
        DDweaver.SelectedIndex = -1;
        if (ddordertype.SelectedValue == "3")
        {
            FillCheckorders(3);
            FillChangestatus(3);
            Tdemp.Visible = true;
            fillemployee();
        }
        else
        {
            FillCheckorders(0);
            FillChangestatus(0);
        }
        fillgrid();
    }
    protected void DDbuyer_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }
    protected void fillemployee()
    {
        string Str = "";
        Str = @"select Distinct empid,EmpName From 
                (SELECT DISTINCT EI.EMPID,EI.EMPNAME FROM PROCESS_ISSUE_MASTER_1  PIM INNER JOIN EMPINFO EI ON PIM.EMPID=EI.EMPID
                UNION ALL
                SELECT DISTINCT EI.EMPID,EI.EMPNAME + CASE WHEN ISNULL(EI.EMPCODE,'')<>'' THEN ' ['+EI.EMPCODE +']' ELSE '' END
                FROM EMPLOYEE_PROCESSORDERNO EMP INNER JOIN EMPINFO EI ON EMP.EMPID=EI.EMPID WHERE PROCESSID=1) A order by EmpName";
        UtilityModule.ConditionalComboFill(ref DDweaver, Str, true, "--Plz Select--");
    }
    protected void DDweaver_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }
    protected void FillCheckorders(int orderType)
    {
        DDStatus.Items.Clear();
        switch (orderType)
        {
            case 3:
                DDStatus.Items.Add(new ListItem("Pending", "0"));
                break;
            default:
                DDStatus.Items.Add(new ListItem("Pending", "0"));
                DDStatus.Items.Add(new ListItem("Complete", "1"));
                DDStatus.Items.Add(new ListItem("Cancel", "2"));
                break;
        }
    }
    protected void FillChangestatus(int orderType)
    {
        ddststatuschange.Items.Clear();
        switch (orderType)
        {
            case 3:
                ddststatuschange.Items.Add(new ListItem("Complete", "1"));
                break;
            default:
                ddststatuschange.Items.Add(new ListItem("Pending", "0"));
                ddststatuschange.Items.Add(new ListItem("Complete", "1"));
                ddststatuschange.Items.Add(new ListItem("Cancel", "2"));
                break;
        }
    }
}