using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Masters_Carpet_AddProcess : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            Fill_Grid();
        }
        LblErrer.Visible = false;

    }
    private void Fill_Grid()
    {
        DGCreateProcess.DataSource = Get_Detail();
        DGCreateProcess.DataBind();
    }
    protected void DGCreateProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtProcessName.Text = DGCreateProcess.SelectedRow.Cells[1].Text;
        TxtShortName.Text = DGCreateProcess.SelectedRow.Cells[2].Text;
        btnsave.Text = "Update";
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        CreateProcess();
        Fill_Grid();

    }
    //*******************************************************************
    private DataSet Get_Detail()
    {
        DataSet DS = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string sqlstr = "Select PROCESS_NAME_ID as ID,PROCESS_NAME,ShortName from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyid"] + " Order by PROCESS_NAME_ID";
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddProcess.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

        return DS;
    }
    protected void DGProcess_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGCreateProcess, "Select$" + e.Row.RowIndex);
        }
    }
    private void validate_processName()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string sqlstr;
            if (btnsave.Text == "Update")
            {
                sqlstr = "Select Isnull(PROCESS_NAME_ID,0) from PROCESS_NAME_MASTER where PROCESS_NAME='" + TxtProcessName.Text + "' and PROCESS_NAME_ID !=" + DGCreateProcess.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                sqlstr = "Select Isnull(max(PROCESS_NAME_ID),0) from PROCESS_NAME_MASTER where PROCESS_NAME='" + TxtProcessName.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
            }
            int Processid = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, sqlstr));
            if (Processid > 0)
            {
                LblErrer.Visible = true;
                LblErrer.Text = "Process Name Already Exists......";
                TxtProcessName.Text = "";
                TxtProcessName.Focus();
            }
            else
            {
                LblErrer.Visible = false;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddProcess.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void TxtProcessName_TextChanged(object sender, EventArgs e)
    {

        validate_processName();

    }
    private void CreateProcess()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            validate_processName();
            con.Open();
            if (LblErrer.Visible == false)
            {
                if (btnsave.Text == "Update")
                {
                    string sqlstr = "Update PROCESS_NAME_MASTER set PROCESS_NAME='" + (TxtProcessName.Text).ToUpper() + "',ShortName='" + (TxtShortName.Text).ToUpper() + "' where PROCESS_NAME_ID=" + DGCreateProcess.SelectedValue;
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, sqlstr);
                    DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                    SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'PROCESS_NAME_MASTER'," + DGCreateProcess.SelectedValue + ",getdate(),'Update')");
                }
                else
                {
                    int id = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "Select isnull(Max(PROCESS_NAME_ID),0)+1 From PROCESS_NAME_MASTER"));
                    string sqlstr = "Insert into PROCESS_NAME_MASTER (PROCESS_NAME_ID,PROCESS_NAME,ShortName,UserId,MasterCompanyId,Processtype) values (" + id + ",'" + TxtProcessName.Text.ToUpper() + "','" + TxtShortName.Text.ToUpper() + "'," + Session["varuserid"] + "," + Session["varCompanyId"] + ",0)";
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, sqlstr);
                    DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                    SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'PROCESS_NAME_MASTER'," + id + ",getdate(),'Insert')");
                }
                btnsave.Text = "Save";
                TxtProcessName.Text = "";
                TxtShortName.Text = "";
                LblErrer.Visible = true;
                LblErrer.Text = "Save Details......";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddProcess.aspx");
            LblErrer.Visible = true;
            LblErrer.Text = "Process Name Already Exists......";
            TxtProcessName.Text = "";
            TxtProcessName.Focus();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }

    protected void DGCreateProcess_RowCreated(object sender, GridViewRowEventArgs e)
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
}