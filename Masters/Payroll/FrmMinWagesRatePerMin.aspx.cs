using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Carpet_FrmMinWagesRatePerMin : System.Web.UI.Page
{

    static int MasterCompanyId;
    static bool VarBool;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varMasterCompanyIDForERP"]);
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (IsPostBack != true)
        {
            BindGrid();
            txtEffectiveDate.Attributes.Add("readonly", "readonly");
            txtEffectiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }

    private void CHECKVALIDCONTROL()
    {
        llMessageBox.Text = "";

        if (UtilityModule.VALIDTEXTBOX(txtMinWagesRate) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtEffectiveDate) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        llMessageBox.Visible = true;
        UtilityModule.SHOWMSG(llMessageBox);
    B: ;
    }
    private void SubmitData()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[6];
            if (ViewState["MinWagesRateId"] == null)
            {
                ViewState["MinWagesRateId"] = 0;
            }
            _arrpara[0] = new SqlParameter("@MinWagesRateId", SqlDbType.Int);
            _arrpara[0].Direction = ParameterDirection.InputOutput;
            _arrpara[0].Value = ViewState["MinWagesRateId"];
            _arrpara[1] = new SqlParameter("@MinWagesRatePerMin", txtMinWagesRate.Text == "" ? "0" : txtMinWagesRate.Text);
            _arrpara[2] = new SqlParameter("@EffectiveDate", txtEffectiveDate.Text);
            _arrpara[3] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);
            _arrpara[3].Direction = ParameterDirection.Output;
            _arrpara[4] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
            _arrpara[5] = new SqlParameter("@UserId", Session["varuserid"]);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_SaveMinWagesRatePerMin]", _arrpara);

            llMessageBox.Visible = true;
            llMessageBox.Text = _arrpara[3].Value.ToString();
            //llMessageBox.Text = "Data Successfully Saved.......";

            ViewState["MinWagesRateId"] = 0;
            Tran.Commit();
            ClearAfterSave();
            BindGrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            ViewState["MinWagesRateId"] = 0;
            llMessageBox.Visible = true;
            llMessageBox.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (llMessageBox.Text == "")
        {
            SubmitData();
        }
    }
    private void ClearAfterSave()
    {
        txtMinWagesRate.Text = "";
        txtEffectiveDate.Text = "";
    }
    protected void BindGrid()
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
            param[0] = new SqlParameter("@MinWagesRateId", "0");
            //param[1] = new SqlParameter("@Where", where);

            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetMinWagesRatePerMin", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                GVMinWagesRate.DataSource = ds.Tables[0];
                GVMinWagesRate.DataBind();
            }
            else
            {
                GVMinWagesRate.DataSource = null;
                GVMinWagesRate.DataBind();
            }

            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            llMessageBox.Text = ex.Message;
            con.Close();
        }
    }
    protected void GVMinWagesRate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVMinWagesRate, "select$" + e.Row.RowIndex);
        }
    }

}