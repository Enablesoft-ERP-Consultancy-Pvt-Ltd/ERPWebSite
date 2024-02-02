using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
public partial class JoinNow : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        btncreate.Attributes.Add("onclick", "return pwdvalidate()");
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            //  UtilityModule.ConditionalComboFillLogin(ref ddlbusiness, "SELECT BusinessId,BusinessType from BusinessMaster order by BusinessId ", true, "-Select-");
        }
    }
    protected void btncreate_Click(object sender, EventArgs e)
    {
        checkDuplicate();
        if (lblerr.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[8];
                _arrPara[0] = new SqlParameter("@ContactName", SqlDbType.NVarChar, 200);
                _arrPara[1] = new SqlParameter("@Loginname", SqlDbType.NVarChar, 200);
                _arrPara[2] = new SqlParameter("@Password", SqlDbType.NVarChar, 100);
                _arrPara[3] = new SqlParameter("@PhoneNo", SqlDbType.NVarChar, 50);
                _arrPara[4] = new SqlParameter("@Email", SqlDbType.NVarChar, 50);
                _arrPara[5] = new SqlParameter("@Companyname", SqlDbType.NVarChar, 200);

                _arrPara[0].Value = txtContactname.Text.Trim();
                _arrPara[1].Value = TxtUserName.Text.Trim();
                _arrPara[2].Value = txtpwd.Text.Trim();
                _arrPara[3].Value = txtmobile.Text.Trim();
                _arrPara[4].Value = txtemail.Text.Trim();
                _arrPara[5].Value = txtcompanyname.Text.Trim();
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_CreateNewCompany", _arrPara);
                Tran.Commit();
                refresh();
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Your Company is create Successfully!');", true);
                // Response.Redirect("CompanyLogin.aspx");            
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", ex.Message, true);
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
    }
    public void refresh()
    {
        TxtUserName.Text = "";
        txtContactname.Text = "";
        txtcompanyname.Text = "";
        txtconfirmpwd.Text = "";
        txtpwd.Text = "";
        txtmobile.Text = "";
        txtemail.Text = "";
        //ddlbusiness.Items.Clear();
    }
    protected void txtemail_TextChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        SqlCommand cmd = new SqlCommand("Select Email from Companyinfo where Email='" + txtemail.Text + "' ", con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {

            dr.Read();
            txtemail.Text = "";
            //lblmsg.Text = "Email Already Exist";
            //lblmsg.ForeColor = System.Drawing.Color.Red;
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Email Id is already exist');", true);
            txtemail.Focus();
        }
        dr.Close();
        con.Close();
    }
    protected void TxtUserName_TextChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        SqlCommand cmd = new SqlCommand("select Loginname From NewUserDetail where Loginname='" + TxtUserName.Text + "' ", con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            TxtUserName.Text = "";
            TxtUserName.Focus();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('UserName is already exist! Please Type Another Name');", true);

        }
        con.Close();
        dr.Close();
    }
    protected void txtcompanyname_TextChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlCommand cmd = new SqlCommand("Select CompanyName from Companyinfo where Companyname='" + txtcompanyname.Text + "'", con);
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            txtcompanyname.Text = "";
            txtcompanyname.Focus();
            ScriptManager.RegisterStartupScript(Page, GetType(), "Opn1", "alert('Company Name is already exist! Please Type Another Name');", true);
        }
        con.Close();
        dr.Close();
    }
    protected void checkDuplicate()
    {

        lblerr.Visible = false;
        lblerr.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlCommand cmd = new SqlCommand("Select Loginname from NewUserDetail where Loginname='" + TxtUserName.Text + "'", con);
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            lblerr.Visible = true;
            lblerr.Text = "User Name Already exists....";
        }
        con.Close();
        dr.Close();
    }
}