using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using System.Text;
using ERP.Security;

public partial class LoginErp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String s = Request.QueryString["Message"];
        lblErr.Text = s;
        txtUser.Focus();
        divStatus.Visible = false;
    }
    protected void btnnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("JoinNow.aspx");
    }
    public static DataTable ImageTable(string ImageFile)
    {
        DataTable data = new DataTable();
        DataRow row;
        data.TableName = "Images";
        data.Columns.Add("img", System.Type.GetType("System.Byte[]"));
        data.Columns.Add("ImgFlage", System.Type.GetType("System.String"));
        if (ImageFile != "")
        {
            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + ImageFile, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            row = data.NewRow();
            row["img"] = br.ReadBytes(Convert.ToInt32(br.BaseStream.Length));
            row["ImgFlage"] = "1";
            data.Rows.Add(row);
            br = null;
            fs.Close();
            fs = null;
        }
        else
        {
            row = data.NewRow();
            System.Text.ASCIIEncoding imgacc = new ASCIIEncoding();

            row["img"] = imgacc.GetBytes("0");
            row["ImgFlage"] = "0";
            data.Rows.Add(row);
        }
        return data;
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
       
       
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[9];
            _arrPara[0] = new SqlParameter("@USERNAME", SqlDbType.NVarChar, 50);
            _arrPara[1] = new SqlParameter("@PASSWORD", SqlDbType.NVarChar, 50);
            _arrPara[2] = new SqlParameter("@varuserid", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@varusername", SqlDbType.VarChar, 50);
            _arrPara[4] = new SqlParameter("@varDepartment", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@status", SqlDbType.Int);
            _arrPara[6] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _arrPara[7] = new SqlParameter("@CompanyName", SqlDbType.VarChar, 60);
            _arrPara[8] = new SqlParameter("@LicenseKey", SqlDbType.VarChar, 100);

            _arrPara[0].Value = txtUser.Text.Trim();
            _arrPara[1].Value = txtPassword.Text;
            _arrPara[2].Direction = ParameterDirection.Output;
            _arrPara[3].Direction = ParameterDirection.Output;
            _arrPara[4].Direction = ParameterDirection.Output;
            _arrPara[5].Direction = ParameterDirection.Output;
            _arrPara[6].Direction = ParameterDirection.Output; ;
            _arrPara[7].Direction = ParameterDirection.Output;
            _arrPara[8].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "[PRO_LOGINIExpro]", _arrPara);
            if (Convert.ToInt32(_arrPara[5].Value) == 0)
            {
                //lblErr.Text = "Login fail: Wrong UserName Or Password";
                divStatus.Visible = true;
                string script = @"document.getElementById('" + divStatus.ClientID + "');setTimeout(function(){document.getElementById('" + divStatus.ClientID + "').style.display='none';},3000);";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "somekey", script, true);
            }
            else if (Convert.ToInt32(_arrPara[5].Value) == 1)
            {
                //string lKey = _arrPara[11].Value.ToString();
                //if (!string.IsNullOrEmpty(lKey) && common.isLicenseValid(lKey, Convert.ToInt32(_arrPara[6].Value)))
                //{
                    Session["varuserid"] = _arrPara[2].Value;
                    Session["varusername"] = _arrPara[3].Value;
                    Session["varDepartment"] = _arrPara[4].Value;
                    Session["varCompanyName"] = _arrPara[7].Value;
                    string name = Session["varusername"].ToString().ToLower();
                    Session["varCompanyId"] = _arrPara[6].Value;
                    Session["varusername"] = "Welcome  " + name[0].ToString().ToUpper() + name.Substring(1);
                    Session["VarcompanyNo"] = _arrPara[6].Value;
                    Response.Redirect("main.aspx");
                //}
                //else
                //{
                //    lblErr.Text = "Your license is expired, please renew.";

                //    string path = Server.MapPath("Web.config");
                //    common.cleaup(path);
                //}
            }

        }
        catch (Exception ex)
        {
            ModalPopupExtender1.Hide();
            lblErr.Text = ex.Message;
            // UtilityModule.MessageAlert(ex.Message, "/Login.aspx");
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
    public DataTable getdata(string str)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        SqlCommand cmd = new SqlCommand("Select * from info where CustomerID='" + txtUser.Text + "'", con);
        //cmd.CommandType = CommandType.StoredProcedure;
        //cmd.Parameters.AddWithValue("@str", str);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        con.Dispose();
        return dt;
    }

}


