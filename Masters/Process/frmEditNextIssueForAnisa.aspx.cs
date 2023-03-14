using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
public partial class Masters_Process_frmEditNextIssueForAnisa : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyID"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            //UtilityModule.ConditionalComboFill(ref ddUnits, "select U.UnitsId,U.UnitName from Units U inner join Units_authentication UA on U.unitsId=UA.UnitsId and UA.Userid=" + Session["varuserid"] + " order by U.unitsId", true, "");
            if (Session["varcompanyid"].ToString()=="8")
            {
                UtilityModule.ConditionalComboFill(ref DDTOProcess, "select PROCESS_NAME_ID,Process_name from PROCESS_NAME_MASTER Where PROCESS_NAME_ID<>1 order by Process_Name", true, "--Plz Select Process--");    
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDTOProcess, @"Select PNM.PROCESS_NAME_ID, PNM.Process_name 
                    From PROCESS_NAME_MASTER PNM(Nolock) 
                    JOIN UserRightsProcess URP(Nolock) ON URP.ProcessId = PNM.PROCESS_NAME_ID And URP.Userid = " + Session["varuserid"] + @" 
                    Where PNM.PROCESS_NAME_ID <> 1 and PNM.Processtype = 1 order by PNM.Process_Name", true, "--Plz Select Process--");
            }
            

        }

    }
    protected void txtWeaverIdNo_TextChanged(object sender, EventArgs e)
    {

        string str = "";
        DataSet ds;
        SqlConnection con = new SqlConnection(ErpGlobalNew.DBCONNECTIONSTRINGNEW);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            if (txtWeaverIdNo.Text != "")
            {

                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Empid,Empname from empinfo Where Empcode='" + txtWeaverIdNo.Text + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (lstWeaverName.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) == null)
                    {

                        lstWeaverName.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Empname"].ToString(), ds.Tables[0].Rows[0]["Empid"].ToString()));
                    }

                    txtWeaverIdNo.Text = "";
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('No Weaver found at this ID No..');", true);
                }

                ds.Dispose();

            }
            txtWeaverIdNo.Focus();
        }
        catch (Exception ex)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void btnDeleteName_Click(object sender, EventArgs e)
    {
        lstWeaverName.Items.Remove(lstWeaverName.SelectedItem);
    }
    protected void btnshowDetail_Click(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void FillGrid()
    {
        String strempid = "";
        DataSet ds;
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@Unitid", SqlDbType.Int);
        param[1] = new SqlParameter("@Processid", SqlDbType.Int);
        param[2] = new SqlParameter("@Empid", SqlDbType.VarChar,100);
        param[3] = new SqlParameter("@CompanyID", SqlDbType.Int);

        param[0].Value = 0;
        param[1].Value = DDTOProcess.SelectedValue;
        for (int i = 0; i < lstWeaverName.Items.Count; i++)
        {
            strempid = strempid == "" ? lstWeaverName.Items[i].Value : strempid + "," + lstWeaverName.Items[i].Value;
        }
        param[2].Value = strempid;
        param[3].Value = Session["CurrentWorkingCompanyID"];

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetNextissueDetailForAnisa", param);
        //if (ds.Tables[0].Rows.Count > 0)
        //{
            DGDetail.DataSource = ds;
            DGDetail.DataBind();
        //}
        //else
        //{
        //    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Alert1", "alert('Data not found for this selection...');", true);
        //    DGDetail.DataSource = null;
        //    DGDetail.DataBind();
        //    return;
        //}
    }
    protected void DGDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            string lblissueorderid = ((Label)DGDetail.Rows[e.RowIndex].FindControl("lblIssueorderId")).Text;
            string lblissueDetailid = ((Label)DGDetail.Rows[e.RowIndex].FindControl("lblIssueDetailid")).Text;
            string lblProcessid = ((Label)DGDetail.Rows[e.RowIndex].FindControl("lblProcessid")).Text;
            string lblStockNo = ((Label)DGDetail.Rows[e.RowIndex].FindControl("lblStockNo")).Text;
            

            SqlParameter[] param=new SqlParameter[7];
            param[0] = new SqlParameter("@IssueOrderid",lblissueorderid);
            param[1] = new SqlParameter("@IssueDetailid", lblissueDetailid);
            param[2] = new SqlParameter("@Processid", lblProcessid);
            param[3] = new SqlParameter("@StockNo", lblStockNo);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar,100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@userid", Session["varuserid"]);
            param[6] = new SqlParameter("@mastercompanyid", Session["varcompanyId"]);            
            
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteNextIssue", param);

            if (param[4].Value.ToString() != "")
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = param[4].Value.ToString();
            }            
            Tran.Commit();
            FillGrid();   
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
}