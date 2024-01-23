using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Net;

public partial class Masters_Hissab_Frmadvanceforanisha : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {

            UtilityModule.ConditionalComboFill(ref ddjobname, "select process_name_id, process_name from process_name_master where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by process_name", true, "-------SELECT---------");
            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
            //fillgrid();
            
        }
    }
    protected void ddjobname_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddpartyname, "select ei.EmpId,EmpName from EmpInfo ei inner join EmpProcess EP on ei.EmpId=EP.EmpId where processId=" + ddjobname.SelectedValue + " order by empname", true, "--Select--");
    }

    private void fillgrid()
    {
        string str = " SELECT AAA.ID, PNM.PROCESS_NAME, EI.EmpName, AAA.AdvanceAmt, AAA.Date from advanceamountforanisa AAA inner join process_name_master PNM on AAA.Jobid = PNM.PROCESS_NAME_ID inner join Empinfo EI on AAA.EmpId =EI.EmpId Where 1=1";
        if (ddjobname.SelectedIndex > 0)
        {
            str = str + " and AAA.jobid=" + ddjobname.SelectedValue + "";
        }
        if (ddpartyname.SelectedIndex > 0)
        {
            str = str + " and AAA.EmpId=" + ddpartyname.SelectedValue + "";
        }
        str = str + " Order by AAA.Date desc";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        DataSet ds = SqlHelper.ExecuteDataset(str);
        gridv.DataSource = ds;
        gridv.DataBind();
    }
    protected void btnsave_Click(object sender, EventArgs e)
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
            param[0] = new SqlParameter("@varuserid ", SqlDbType.Int);
            param[1] = new SqlParameter("@Jobid", SqlDbType.Int);
            param[2] = new SqlParameter("@EmpId ", SqlDbType.Int);
            param[3] = new SqlParameter("@AdvanceAmt ", SqlDbType.Float);
            param[4] = new SqlParameter("@Date ", SqlDbType.DateTime);
            param[5] = new SqlParameter("@msg", SqlDbType.VarChar, 50);

            param[0].Value = Session["varuserid"].ToString();
            param[1].Value = ddjobname.SelectedIndex < 0 ? "0" : ddjobname.SelectedValue;
            param[2].Value = ddpartyname.SelectedIndex < 0 ? "0" : ddpartyname.SelectedValue;
            param[3].Value = txtamount.Text;
            param[4].Value = txtdate.Text;
            param[5].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_advanceamountforanisa", param);
            lblmsg.Text = param[5].Value.ToString();
            Tran.Commit();
            fillgrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
            con.Close();

        }
    }
    protected void ddpartyname_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }
}