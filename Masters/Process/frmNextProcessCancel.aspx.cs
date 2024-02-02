using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Process_frmNextProcessCancel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
            return;
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By CompanyName", true, "--SelectCompany");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFill(ref DDProcessName, "select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By PROCESS_NAME", true, "--Select Process--");
        }

    }
    protected void DDProcessName_SelectedIndexChanged1(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDEmployeeName, "select Distinct E.EmpId,E.EmpName+'/'+Address from Empinfo E,Process_Issue_Master_" + DDProcessName.SelectedValue + " PM Where E.EmpId=PM.Empid", true, "--Select---");
    }
    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DGStockNo.DataSource = null;
        DGStockNo.DataBind();
        UtilityModule.ConditionalComboFill(ref DDPOrderNo, @"select Distinct PM.issueOrderId,PM.IssueOrderId from Process_Issue_Master_" + DDProcessName.SelectedValue + " PM,Process_Issue_Detail_" + DDProcessName.SelectedValue + @" PD Where PM.IssueOrderId=PD.IssueOrderid
                                                            And Issue_Detail_id in(select IssueDetailid from carpetNumber CN,Process_Stock_Detail PD
                                                            where CN.StockNo=PD.stockNo And CN.Pack=0 And ToProcessId=" + DDProcessName.SelectedValue + @" And IssRecStatus=1 And CurrentProStatus=" + DDProcessName.SelectedValue +@")
                                                             And PM.CompanyId=" + DDCompanyName.SelectedValue + " And PM.EmpId=" + DDEmployeeName.SelectedValue + " Order by PM.IssueOrderId desc", true, "--Select--");

    }
    protected void FillGrid()
    {
        DGStockNo.DataSource = null;
        DGStockNo.DataBind();
        string str = @"select Distinct TStockNo,Issue_Detail_Id from CarpetNumber CN,Process_Stock_Detail PS,Process_Issue_Detail_" + DDProcessName.SelectedValue + @" PD
                     where CN.StockNo=PS.StockNo And CN.pack=0 And ToProcessId=" + DDProcessName.SelectedValue + @" And IssRecStatus=1 And PD.Issue_Detail_Id=PS.IssueDetaiLid And CurrentproStatus=" + DDProcessName.SelectedValue + @"
                     And PD.IssueOrderId=" + DDPOrderNo.SelectedValue + "  Order by TStockNo";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGStockNo.DataSource = ds.Tables[0];
        DGStockNo.DataBind();
    }
    protected void DDPOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void TxtStockno_TextChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        if (TxtStockno.Text != "")
        {
            BtnCancel_Click(sender, e);
        }
    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        string str;
        DataSet ds;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            str = @"select ProcessDetailId,Issue_Detail_Id,FromProcessid from CarpetNumber CN,Process_Stock_Detail PS,Process_Issue_Detail_" + DDProcessName.SelectedValue + @" PD Where CN.StockNo=PS.StockNo
                 And PS.issueDetailId=PD.Issue_Detail_Id And CurrentProStatus=" + DDProcessName.SelectedValue + " And PD.IssueOrderId=" + DDPOrderNo.SelectedValue + @"
                 And TStockNo='" + TxtStockno.Text + "' And IssrecStatus=1 And CN.Pack=0 And CN.CompanyID = " + DDCompanyName.SelectedValue;
            ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                SqlParameter[] _array=new SqlParameter[9];
                _array[0] = new SqlParameter("@ProcessId", SqlDbType.Int);
                _array[1] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
                _array[2] = new SqlParameter("@IssueDetailId", SqlDbType.Int);
                _array[3] = new SqlParameter("@TStockNo", SqlDbType.NVarChar,500);
                _array[4] = new SqlParameter("@ProcessDetailId", SqlDbType.Int);
                _array[5] = new SqlParameter("@Msg", SqlDbType.NVarChar,500);
                _array[6] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                _array[7] = new SqlParameter("@UserId", SqlDbType.Int);
                _array[8] = new SqlParameter("@FromProcessId", SqlDbType.Int);

                _array[0].Value = DDProcessName.SelectedValue;
                _array[1].Value = DDPOrderNo.SelectedValue;
                _array[2].Value = ds.Tables[0].Rows[0]["Issue_Detail_Id"];
                _array[3].Value =TxtStockno.Text.ToUpper();
                _array[4].Value = ds.Tables[0].Rows[0]["ProcessDetailId"];
                _array[5].Direction = ParameterDirection.Output;
                _array[6].Value = Session["varMasterCompanyIDForERP"];
                _array[7].Value = Session["Varuserid"];
                _array[8].Value = ds.Tables[0].Rows[0]["FromProcessid"];


                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_CancelStockNo", _array);
                Tran.Commit();
                FillGrid();
                TxtStockno.Text = "";
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = _array[5].Value.ToString();
            }
            else
            {
                
                LblErrorMessage.Text = "Status for this stock No is Received or not available or already canceled..";
                Tran.Commit();
                return;

            }
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
}