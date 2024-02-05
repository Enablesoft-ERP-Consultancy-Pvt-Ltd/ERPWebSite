using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ZDevi_FrmPropertyPartnerDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
            String Str = @"Select PM.PropertyID, PM.PropertyName + ' (' + EM.EmployeeName + ')' PropertyName
                From PropertyMaster PM(Nolock) 
                JOIN EmployeeMaster EM(Nolock) ON EM.EmployeeID = PM.PropertyOwnerID 
                Order By PM.PropertyName ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref DDPropertyName, ds, 0, false, "");
            PropertyNameSelectedChanged();
        }
    }

    protected void DDPropertyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        PropertyNameSelectedChanged();
    }

    private void PropertyNameSelectedChanged()
    {
        SqlParameter[] para = new SqlParameter[1];
        para[0] = new SqlParameter("@PropertyID", SqlDbType.Int);

        para[0].Value = DDPropertyName.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_PropertyPartnerDetail", para);

        DGPartnerDetail.DataSource = ds.Tables[0];
        DGPartnerDetail.DataBind();
    }
    protected void DGPartnerDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGPartnerDetail, "Select$" + e.Row.RowIndex);
        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        string StrDetail = "";
        for (int i = 0; i < DGPartnerDetail.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DGPartnerDetail.Rows[i].FindControl("Chkboxitem"));
            if (Chkboxitem.Checked == true)
            {
                string strOrderid = DGPartnerDetail.DataKeys[i].Value.ToString();

                Label lblPartnerID = (Label)DGPartnerDetail.Rows[i].FindControl("lblPartnerID");
                TextBox txtPercentage = (TextBox)DGPartnerDetail.Rows[i].FindControl("txtPercentage");
                double Percentage = Convert.ToDouble(txtPercentage.Text == "" ? "0" : txtPercentage.Text);
                if (StrDetail == "")
                {
                    StrDetail = lblPartnerID.Text + "|" + Percentage + "~";
                }
                else
                {
                    StrDetail = StrDetail + lblPartnerID.Text + "|" + Percentage + "~";
                }
            }
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[5];
            _arrPara[0] = new SqlParameter("@PropertyID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@StrDetail", SqlDbType.NVarChar, 2000);
            _arrPara[2] = new SqlParameter("@UserID", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            _arrPara[0].Value = DDPropertyName.SelectedValue;
            _arrPara[1].Value = StrDetail;
            _arrPara[2].Value = Session["varuserid"].ToString();
            _arrPara[3].Value = Session["varMasterCompanyIDForERP"].ToString();
            _arrPara[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_Save_PropertyPartnerDetail", _arrPara);

            if (_arrPara[11].Value.ToString() == "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Data Successfully Saved');", true);
                tran.Commit();
            }
            else
            {
                lblErrorMsg.Text = _arrPara[11].Value.ToString();
                tran.Rollback();
            }
        }
        catch (Exception ex)
        {

            Logs.WriteErrorLog("FrmPropertyPartnerDetail|" + ex.Message);
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

    protected void BtnPreview_Click(object sender, EventArgs e)
    {

    }
}