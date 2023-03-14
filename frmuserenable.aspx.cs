using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

public partial class frmuserenable : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyNo"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDusertype, "select Id,Usertype from Usertype order by id", true, "--Plz Select--");
        }
    }
    protected void Fillgrid()
    {
        string str;
        DataSet ds;
        str = "select Username,Designation,userid,loginflag from Newuserdetail Where usertype=" + DDusertype.SelectedValue + " order by username";
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GDView.DataSource = ds.Tables[0];
        GDView.DataBind();
    }
    protected void DDusertype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillgrid();
    }
    protected void GDView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType==DataControlRowType.DataRow)
        {
            Label lblloginflag = ((Label)e.Row.FindControl("lblloginflag"));
            CheckBox chkitem = ((CheckBox)e.Row.FindControl("chkitem"));
            if (lblloginflag.Text=="0")
            {
                chkitem.Checked = true;
                e.Row.BackColor = Color.Green;
            }
            else
            {
                chkitem.Checked = false;
                e.Row.BackColor = Color.Red;
            }
        }
    }
    protected void btnenableDisable_Click(object sender, EventArgs e)
    {
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("Userid", typeof(int));
        dtrecords.Columns.Add("Loginflag", typeof(int));
        //****************
        for (int i = 0; i < GDView.Rows.Count; i++)
        {
            CheckBox chkitem = ((CheckBox)GDView.Rows[i].FindControl("chkitem"));
            Label lbluserid = ((Label)GDView.Rows[i].FindControl("lbluserid"));
            Label lblloginflag = ((Label)GDView.Rows[i].FindControl("lblloginflag"));
            DataRow dr = dtrecords.NewRow();
            dr["userid"] = lbluserid.Text;
            dr["loginflag"] = (chkitem.Checked == true ? 0 : 1);
            dtrecords.Rows.Add(dr);
        }
        //***************
        SqlParameter[] param=new SqlParameter[1];
        param[0] = new SqlParameter("@dtrecords", dtrecords);

        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_updateuserstatus", param);
        ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('User Enabled_Disabled successfully..');", true);
        Fillgrid();
    }
}