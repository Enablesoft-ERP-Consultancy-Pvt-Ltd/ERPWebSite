using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Campany_FrmTermsConditions : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
            ViewState["TCID"] = 0;
            UtilityModule.ConditionalComboFill(ref DDFormatType, "select TCFID,FormatName From TermsConditionFormats ", true, "--Plz Select--");
            Fill_Grid();

        }
        lbblerr.Visible = false;
    }
    private void Fill_Grid()
    {
        string where = "";
        if (DDFormatType.SelectedIndex > 0)
        {
            where = where + " and TC.TCFID=" + DDFormatType.SelectedValue;
            //where = where + " and TC.TCFID=0" ;
        }


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@TCID", "0");
            param[1] = new SqlParameter("@Where", where);

            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetTermsConditions", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                dgTermsConditions.DataSource = ds;
                dgTermsConditions.DataBind();
            }
            else
            {
                dgTermsConditions.DataSource = null;
                dgTermsConditions.DataBind();
            }

            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lbblerr.Text = ex.Message;
            con.Close();
        }
    }
    protected void dgTermsConditions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgTermsConditions, "Select$" + e.Row.RowIndex);
        }
    }
    protected void dgTermsConditions_SelectedIndexChanged(object sender, EventArgs e)
    {
        int r = Convert.ToInt32(dgTermsConditions.SelectedIndex.ToString());

        lbblerr.Text = "";

        BtnSave.Text = "Save";

        string id = dgTermsConditions.SelectedDataKey.Value.ToString();
        hnid.Value = id;

        ViewState["TCID"] = id;

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@TCID", id);

            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetTermsConditions", param);

            if (ds.Tables[0].Rows.Count == 1)
            {
                DDFormatType.SelectedValue = ds.Tables[0].Rows[0]["TCFID"].ToString();
                txtTermsConditions.Text = ds.Tables[0].Rows[0]["TermsConditions"].ToString();
            }
            //BindGrid();            

            Tran.Commit();
        }
        catch (Exception ex)
        {
            lbblerr.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        BtnSave.Text = "Update";
    }

    private void CHECKVALIDCONTROL()
    {
        lbblerr.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDFormatType) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtTermsConditions) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        lbblerr.Visible = true;
        UtilityModule.SHOWMSG(lbblerr);
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
            if (ViewState["TCID"] == null)
            {
                ViewState["TCID"] = 0;
            }
            _arrpara[0] = new SqlParameter("@TCID", SqlDbType.Int);
            _arrpara[0].Direction = ParameterDirection.InputOutput;
            _arrpara[0].Value = ViewState["TCID"];
            _arrpara[1] = new SqlParameter("@TCFID", DDFormatType.SelectedValue == "" ? "0" : DDFormatType.SelectedValue);
            _arrpara[2] = new SqlParameter("@TermsConditions", txtTermsConditions.Text);
            _arrpara[3] = new SqlParameter("@UserId", Session["varUserId"].ToString());
            _arrpara[4] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"].ToString());
            _arrpara[5] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);
            _arrpara[5].Direction = ParameterDirection.Output;


            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_SaveTermsConditions]", _arrpara);

            lbblerr.Visible = true;
            lbblerr.Text = _arrpara[5].Value.ToString();
            //llMessageBox.Text = "Data Successfully Saved.......";

            ViewState["TCID"] = 0;
            Tran.Commit();
            ClearAfterSave();
            Fill_Grid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            ViewState["TCID"] = 0;
            lbblerr.Visible = true;
            lbblerr.Text = ex.Message;
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
        if (lbblerr.Text == "")
        {
            SubmitData();
        }
    }
    private void ClearAfterSave()
    {
        //lbblerr.Visible = false;
        txtTermsConditions.Text = "";
        BtnSave.Text = "Save";

    }
    protected void DDFormatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
}