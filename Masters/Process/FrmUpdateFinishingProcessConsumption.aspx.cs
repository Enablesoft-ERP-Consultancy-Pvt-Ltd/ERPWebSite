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

public partial class Masters_Process_FrmUpdateFinishingProcessConsumption : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName
                           select PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where ProcessType=1 and Process_Name_id<>1 and MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by PROCESS_NAME_ID";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDprocess, ds, 1, true, "--Plz Select--");
        }
    }
    protected void DDprocess_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillFolioNo();
    }
    protected void FillFolioNo(object sender = null)
    {
        string str = @"select Distinct PM.IssueOrderId,PM.ChallanNo from PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + @" PM left join employee_Processorderno EMp on PM.issueorderid=EMp.issueorderid and EMp.processid=" + DDprocess.SelectedValue + @"
                     left join empinfo ei on emp.empid=ei.empid Where PM.Companyid=" + DDcompany.SelectedValue + " and PM.status='Pending' ";

        UtilityModule.ConditionalComboFill(ref DDFoliono, str, true, "--Plz Select--");
        if (DDFoliono.Items.Count > 0)
        {
            if (sender != null)
            {
                if (txtissueno.Text != "")
                {
                    //DDFoliono.SelectedItem.Text = txtissueno.Text;

                    DDFoliono.SelectedValue = txtissueno.Text;
                    DDFoliono_SelectedIndexChanged(sender, new EventArgs());
                }
            }
        }
    }
    protected void txtissueno_TextChanged(object sender, EventArgs e)
    {
        FillFolioNo(sender);
    }
    protected void DDFoliono_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtissueno.Text = DDFoliono.SelectedItem.Text;
        Fillgrid();
        FillConsumptionQty();
    }
    protected void Fillgrid()
    {
        string str = @"Select Issue_Detail_Id,PM.issueorderid,ICM.Category_Name+' '+IM.Item_Name+' '+IPM.QDCS + Space(2) + Case When PM.Unitid=1 Then SizeMtr Else case When PM.unitid=6 Then Sizeinch  Else  SizeFt End End ItemDescription,Length,Width,
                        Length + 'x' + Width Size,ROund(Area*Qty,4) as Area,Rate,Comm,Qty,Amount,REPLACE(CONVERT(nvarchar(11),PM.AssignDate,106),' ','-') as AssignDate,REPLACE(CONVERT(nvarchar(11),PD.ReqbyDate,106),' ','-') as ReqbyDate,PM.Unitid,isnull(PM.Purchaseflag,0) as Purchaseflag,PM.Caltype ,
                        isnull(PM.Remarks,'') as Remarks,isnull(Pm.instruction,'') as Instruction,pm.FlagFixOrWeight,isnull(PM.ChallanNo,'') as ChallanNo                        
                        From PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + " PM INNER JOIN PROCESS_ISSUE_DETAIL_" + DDprocess.SelectedValue + @" PD ON PM.IssueOrderID=PD.IssueOrderId
                        INNER JOIN ViewFindFinishedidItemidQDCSS IPM ON PD.Item_Finished_ID=IPM.FinishedId 
                        INNER JOIN Item_Master IM ON IM.Item_Id=IPM.Item_Id
                        INNER JOIN ITEM_CATEGORY_MASTER ICM ON IM.Category_Id=ICM.Category_Id                        
                        Where PM.IssueOrderid=" + DDFoliono.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By Issue_Detail_Id Desc";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGOrderdetail.DataSource = ds.Tables[0];
        DGOrderdetail.DataBind();
        txtissuedate.Text = ds.Tables[0].Rows[0]["AssignDate"].ToString();

    }
    protected void FillConsumptionQty()
    {
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@issueorderid", DDFoliono.SelectedValue);
        param[1] = new SqlParameter("@ProcessId", DDprocess.SelectedValue);
        param[2] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
        param[3] = new SqlParameter("@UserId", Session["VarUserId"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "FILLFINISHINGPROCESSCONSUMPTION", param);
        DGConsumption.DataSource = ds.Tables[0];
        DGConsumption.DataBind();

    }
    protected void DGOrderdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //TextBox txtqty = (TextBox)e.Row.FindControl("txtqty");
            //if (txtqty != null)
            //{
            //    if (hnordercaltype.Value == "1" && variable.VarGENERATESTOCKNOONTAGGING == "1")
            //    {
            //        txtqty.Enabled = false;
            //    }
            //}
        }
    }
    protected void btnupdateconsmp_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@issueorderid", DDFoliono.SelectedValue);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@ProcessId", DDprocess.SelectedValue);
            param[3] = new SqlParameter("@IssueDate", txtissuedate.Text);
            param[4] = new SqlParameter("@MasterCompanyId", Session["varcompanyNo"]);

            //******
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateFinishingProcessCurrentConsumption", param);
            //******
            lblmessage.Text = param[1].Value.ToString();
            Tran.Commit();
            FillConsumptionQty();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }

}
