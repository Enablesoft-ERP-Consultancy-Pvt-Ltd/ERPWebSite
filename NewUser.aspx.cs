using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class NewUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            //CommanFunction.FillCombo(ddUserType, "Select * from UserType where Id<>1 order by Id");
            string str = "Select * from UserType";
            if (Session["varCompanyId"].ToString() == "21")
            {               
                    str = str + " Where ID!=6"; 
            }           
                str = str + " order by Id";
                if (Session["varCompanyId"].ToString() == "22")
                {
                    chkbackentry.Visible = true;
                }
                else
                { chkbackentry.Visible = false; }

                if (Session["varCompanyId"].ToString() == "44")
                {
                    chkprodcons.Visible = true;
                    chkdevcons.Visible = true;
                }
                else
                {
                    chkprodcons.Visible = false;
                    chkdevcons.Visible = false;
                }
            CommanFunction.FillCombo(ddUserType, str);
            Fill_Grid();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[13];
            _arrPara[0] = new SqlParameter("@USERNAME", SqlDbType.NVarChar, 60);
            _arrPara[1] = new SqlParameter("@Designation", SqlDbType.NVarChar, 60);
            _arrPara[2] = new SqlParameter("@LoginName", SqlDbType.NVarChar, 60);
            _arrPara[3] = new SqlParameter("@PASSWORD", SqlDbType.NVarChar, 100);
            _arrPara[4] = new SqlParameter("@DeptId", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _arrPara[6] = new SqlParameter("@VarUserId", SqlDbType.Int);
            _arrPara[7] = new SqlParameter("@EditUserId", SqlDbType.Int);
            _arrPara[8] = new SqlParameter("@UserType", SqlDbType.Int);
            _arrPara[9] = new SqlParameter("@canedit", SqlDbType.TinyInt);
            _arrPara[10] = new SqlParameter("@canbackdateentry", SqlDbType.TinyInt);
            _arrPara[11] = new SqlParameter("@canseeDevelopmentcons", SqlDbType.TinyInt);
            _arrPara[12] = new SqlParameter("@canseeProductioncons", SqlDbType.TinyInt);

            _arrPara[0].Value = txtUser.Text.ToUpper();
            _arrPara[1].Value = txtDesignation.Text.ToUpper();
            _arrPara[2].Value = txtlogin.Text;
            _arrPara[3].Value = txtPassword.Text;
            //_arrPara[3].Value = UtilityModule.Encrypt(txtPassword.Text);
            _arrPara[4].Value = txtdepartment.Text.Trim() == "" ? 0 : Convert.ToInt32(txtdepartment.Text);
            _arrPara[5].Value = Session["varCompanyId"];
            _arrPara[6].Value = Session["varuserid"];
            _arrPara[7].Value = 0;
            if (btnSave.Text == "Update")
            {
                _arrPara[7].Value = DG.SelectedDataKey.Value;
            }
            _arrPara[8].Value = ddUserType.SelectedValue;   //USER LEVEL 4 IN HR Means PayRollType OR Not 
            _arrPara[9].Value = chkcanedit.Checked == true ? 1 : 0;
            _arrPara[10].Value = chkbackentry.Checked == true ? 1 : 0;
            _arrPara[11].Value = chkdevcons.Checked == true ? 1 : 0;
            _arrPara[12].Value = chkprodcons.Checked == true ? 1 : 0;
            con.Open();
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_NEWUSER", _arrPara);
            btnSave.Text = "Save";
            lblErr.Text = "Data Saved Successfully";
            Fill_Grid();
            txtUser.Text = "";
            txtDesignation.Text = "";
            txtlogin.Text = "";
            //txtPassword.Text = "";
            txtPassword.Attributes.Clear();
            txtdepartment.Text = "";
            ddUserType.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "NewUser.aspx");
            Logs.WriteErrorLog("Login|cmdLogin_Click|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void btnlog_Click(object sender, EventArgs e)
    {
        Response.Redirect("login.aspx");
    }
    protected void txtlogin_TextChanged(object sender, EventArgs e)
    {
        lblErr.Text = "";
        if (txtlogin.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[3];
                _arrPara[0] = new SqlParameter("@LoginName", SqlDbType.NVarChar, 60);
                _arrPara[1] = new SqlParameter("@CompanyId", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@Result", SqlDbType.Int);

                _arrPara[0].Value = txtlogin.Text.Trim().ToUpper();
                _arrPara[1].Value = Session["varCompanyId"];
                _arrPara[2].Direction = ParameterDirection.Output;
                con.Open();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_ValidateUser", _arrPara);
                if (Convert.ToInt16(_arrPara[2].Value) == 1)
                {
                    //LblLogin.Visible = true;
                    txtlogin.Text = "";
                    txtlogin.Focus();
                }
                else
                {
                    //LblLogin.Visible = false;
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "NewUser.aspx");
                lblErr.Text = "LoginName validations fail.........";
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    private void Fill_Grid()
    {
        DG.DataSource = fill_Data_grid();
        DG.DataBind();
    }
    private DataSet fill_Data_grid()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"Select UserId SrNo,UserName,Designation,LoginName,PassWord from NewUserDetail where CompanyId=" + Session["varCompanyId"] + " and UserType!=6 Order By UserId";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "NewUser.aspx");
            Logs.WriteErrorLog("Masters_process_ProcessRawIssue|fill_Data_grid|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        return ds;
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DG, "select$" + e.Row.RowIndex);
        }
    }
    protected void DG_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblErr.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarUserId = Convert.ToInt32(DG.DataKeys[e.RowIndex].Value);
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "DELETE From NewUserDetail Where UserId<>1 And UserId=" + VarUserId + "");
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "DELETE From UserRights Where UserId<>1 And UserId=" + VarUserId + "");
            Tran.Commit();
            Fill_Grid();
            lblErr.Visible = true;
            lblErr.Text = "Successfully Deleted...";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "NewUser.aspx");
            Tran.Rollback();
            lblErr.Visible = true;
            lblErr.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DG_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblErr.Text = "";
        int VarUserId = Convert.ToInt32(DG.SelectedDataKey.Value);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select * From NewUserDetail Where UserId=" + VarUserId);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {               

                txtUser.Text = ds.Tables[0].Rows[0]["UserName"].ToString();
                txtDesignation.Text = ds.Tables[0].Rows[0]["Designation"].ToString();
                txtlogin.Text = ds.Tables[0].Rows[0]["LoginName"].ToString();
                //txtPassword.Text = ds.Tables[0].Rows[0]["PassWord"].ToString();
                string pwd = ds.Tables[0].Rows[0]["PassWord"].ToString();
                //string pwd = UtilityModule.Decrypt(ds.Tables[0].Rows[0]["PassWord"].ToString());
                txtPassword.Attributes.Add("value", pwd);
                txtdepartment.Text = ds.Tables[0].Rows[0]["DeptId"].ToString();
                CommanFunction.FillCombo(ddUserType, "Select ID,UserType from UserType order by Id");
                ddUserType.SelectedValue = ds.Tables[0].Rows[0]["UserType"].ToString();
                chkcanedit.Checked = ds.Tables[0].Rows[0]["canedit"].ToString() == "1" ? true : false;
                chkbackentry.Checked = ds.Tables[0].Rows[0]["canbackdateentry"].ToString() == "1" ? true : false;
                chkprodcons.Checked = ds.Tables[0].Rows[0]["canseeProductioncons"].ToString() == "1" ? true : false;
                chkdevcons.Checked = ds.Tables[0].Rows[0]["canseeDevelopmentcons"].ToString() == "1" ? true : false;

                if (Session["varCompanyId"].ToString() == "21")
                {
                    if (VarUserId == 50)
                    {
                        txtUser.Enabled = false;
                        txtDesignation.Enabled = false;
                        txtlogin.Enabled = false;
                        txtdepartment.Enabled = false;
                        ddUserType.Enabled = false;
                        chkcanedit.Enabled = false;
                       
                    }
                    else
                    {
                        txtUser.Enabled = true;
                        txtDesignation.Enabled = true;
                        txtlogin.Enabled = true;
                        txtdepartment.Enabled = true;
                        ddUserType.Enabled = true;
                        chkcanedit.Enabled = true;

                    }
                }
               
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "NewUser.aspx");
            lblErr.Text = ex.Message;
            // Logs.WriteErrorLog("Masters_Campany_Design|Fill_Grid_Data|" + Ex.Message);
        }
        btnSave.Text = "Update";
    }
    protected void Btnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("NewUser.aspx");
    }
    //protected void DG_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //Add CSS class on header row.
    //    if (e.Row.RowType == DataControlRowType.Header)
    //        e.Row.CssClass = "header";

    //    //Add CSS class on normal row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Normal)
    //        e.Row.CssClass = "normal";

    //    //Add CSS class on alternate row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Alternate)
    //        e.Row.CssClass = "alternate";
    //}
}