using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Masters_Campany_Design : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
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
        DGSignature.DataSource = Get_Detail();
        DGSignature.DataBind();
    }
    protected void DGSignature_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["id"] = DGSignature.SelectedDataKey.Value.ToString();
        TxtSignature.Text = DGSignature.SelectedRow.Cells[1].Text;
        btnsave.Text = "Update";
        btndelete.Visible = true;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        EnterSignature();
        Fill_Grid();
        btnsave.Text = "Save";
        btndelete.Visible = false;
   }
    //*******************************************************************
    private DataSet Get_Detail()
    {
        DataSet DS = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string sqlstr = "Select SignatoryId as ID,SignatoryName from Signatory Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by SignatoryId";
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmSignature.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return DS;
    }
    protected void DGSignature_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGSignature, "Select$" + e.Row.RowIndex);
        }
    }
    private void validate_signature()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string sqlstr;
            if (btnsave.Text == "Update")
            {
                sqlstr = "Select Isnull(SignatoryId,0) from Signatory where SignatoryName='" + TxtSignature.Text + "' and SignatoryID !=" + DGSignature.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            else
            {
                sqlstr = "Select Isnull(max(SignatoryId),0) from Signatory where SignatoryName='" + TxtSignature.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
                int Processid = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, sqlstr));
            if (Processid > 0)
            {
                LblErrer.Visible = true;
                LblErrer.Text = "Signatory Already Exists......";
                TxtSignature.Text = "";
                TxtSignature.Focus();
            }
            else
            {
                LblErrer.Visible = false;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmSignature.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void TxtSignature_TextChanged(object sender, EventArgs e)
    {
        validate_signature();
    }
    private void EnterSignature()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if(TxtSignature.Text!="")
            {
                try
                {
                    validate_signature();
                    string status;
                    int id;
                    if (LblErrer.Visible == false)
                    {
                        if (btnsave.Text == "Update")
                        {
                            string sqlstr = "Update Signatory set SignatoryName='" + TxtSignature.Text.ToUpper() + "' where SignatoryID=" + DGSignature.SelectedValue;
                            SqlHelper.ExecuteNonQuery(con, CommandType.Text, sqlstr);
                            id =Convert.ToInt32 (DGSignature.SelectedValue.ToString());
                            status = "Update";
                            LblErrer.Visible = true;
                            LblErrer.Text = "Value updated.............";
                            btnsave.Text = "Save";
                            TxtSignature.Text = "";
                        }
                        else
                        {
                             id = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Max(SignatoryId),0)+1 from Signatory"));
                            string sqlstr = "Insert into Signatory (SignatoryId,SignatoryName,userid,MasterCompanyid) values (" + id + ",'" + TxtSignature.Text.ToUpper() + "'," + Convert.ToInt32(Session["varuserid"])+","+Convert.ToInt32(Session["varMasterCompanyIDForERP"])+")";
                            SqlHelper.ExecuteNonQuery(con, CommandType.Text, sqlstr);
                            status = "Insert";
                            LblErrer.Visible = true;
                            LblErrer.Text = "Value added.............";
                            btnsave.Text = "Save";
                            TxtSignature.Text = "";
                        }
                        DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                        SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'Signatory'," + id + ",getdate(),'"+status+"')");
                   }

                }
                catch (Exception ex)
                {
                    UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmSignature.aspx");
                    LblErrer.Visible = true;
                    LblErrer.Text = "Process Name Already Exists......";
                    TxtSignature.Text = "";
                    TxtSignature.Focus();
                }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
        else
        {
            LblErrer.Visible = true;
            LblErrer.Text = "Fill Details......";
        }
    }
   
     protected void btndelete_Click(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Sigantory from CompanyInfo where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And Sigantory=" + Session["id"].ToString()));
            if (id <= 0)
            {
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from Signatory where SignatoryId=" + Session["id"].ToString());
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'Signatory'," + Session["id"].ToString() + ",getdate(),'Delete')");
                TxtSignature.Text = "";
                LblErrer.Visible = true;
                LblErrer.Text = "Value Deleted...";
            }
            else
            {
                LblErrer.Text = "Value in Use...";
                TxtSignature.Text = "";
            }
        }
        catch ( Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmSignature.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        Fill_Grid();
        btndelete.Visible = false;
        btnsave.Text = "Save";
    }

     protected void DGSignature_RowCreated(object sender, GridViewRowEventArgs e)
     {
         //Add CSS class on header row.
        // if (e.Row.RowType == DataControlRowType.Header)
            // e.Row.CssClass = "header";

         //Add CSS class on normal row.
       //  if (e.Row.RowType == DataControlRowType.DataRow &&
                   //e.Row.RowState == DataControlRowState.Normal)
            // e.Row.CssClass = "normal";

         //Add CSS class on alternate row.
        // if (e.Row.RowType == DataControlRowType.DataRow &&
                  // e.Row.RowState == DataControlRowState.Alternate)
             //e.Row.CssClass = "alternate";
     }
}