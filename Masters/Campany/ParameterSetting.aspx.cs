using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_ParameterSetting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            filltxtbox();
        }
    }
    private void filltxtbox()
    {
        DataSet ds = null;
        try
        {
            string sqlstr = "Select Parameter_Id,Parameter_Name from Parameter_Setting where Company_Id=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(sqlstr);
            int n = ds.Tables[0].Rows.Count;
            if (n > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    switch (Convert.ToInt32(ds.Tables[0].Rows[i]["Parameter_Id"]))
                    {
                        case 1:
                            TxtQuality.Text = ds.Tables[0].Rows[i]["Parameter_Name"].ToString();
                            break;
                        case 2:
                            TxtDesign.Text = ds.Tables[0].Rows[i]["Parameter_Name"].ToString();
                            break;
                        case 3:
                            TxtColor.Text = ds.Tables[0].Rows[i]["Parameter_Name"].ToString();
                            break;
                        case 4:
                            TxtShape.Text = ds.Tables[0].Rows[i]["Parameter_Name"].ToString();
                            break;
                        case 5:
                            TxtSize.Text = ds.Tables[0].Rows[i]["Parameter_Name"].ToString();
                            break;
                        case 6:
                            TxtCategory.Text = ds.Tables[0].Rows[i]["Parameter_Name"].ToString();
                            break;
                        case 7:
                            TxtItem.Text = ds.Tables[0].Rows[i]["Parameter_Name"].ToString();
                            break;
                        case 8:
                            TxtShadeColor.Text = ds.Tables[0].Rows[i]["Parameter_Name"].ToString();
                            break;
                    }
                }
            }
            else
            {
                TxtCategory.Text = "Category";
                TxtItem.Text = "Item";
                TxtQuality.Text = "Quality";
                TxtDesign.Text = "Design";
                TxtColor.Text = "Color";
                TxtShadeColor.Text = "ColorShade";
                TxtShape.Text = "Shape";
                TxtSize.Text = "Size";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/ParameterSetting.aspx");
            Lblmsg.Visible = true;
            Lblmsg.Text = ex.Message;
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[11];
            _arrpara[0] = new SqlParameter("@Company_Id", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@User_Id", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@Category", SqlDbType.NVarChar, 20);
            _arrpara[3] = new SqlParameter("@Item", SqlDbType.NVarChar, 20);
            _arrpara[4] = new SqlParameter("@Quality", SqlDbType.NVarChar, 20);
            _arrpara[5] = new SqlParameter("@Design", SqlDbType.NVarChar, 20);
            _arrpara[6] = new SqlParameter("@Color", SqlDbType.NVarChar, 20);
            _arrpara[7] = new SqlParameter("@ShadeColor", SqlDbType.NVarChar, 20);
            _arrpara[8] = new SqlParameter("@Shape", SqlDbType.NVarChar, 20);
            _arrpara[9] = new SqlParameter("@Size", SqlDbType.NVarChar, 20);

            _arrpara[0].Value = Session["varCompanyId"];
            _arrpara[1].Value = Session["varuserid"];
            _arrpara[2].Value = TxtCategory.Text.ToUpper();
            _arrpara[3].Value = TxtItem.Text.ToUpper();
            _arrpara[4].Value = TxtQuality.Text.ToUpper();
            _arrpara[5].Value = TxtDesign.Text.ToUpper();
            _arrpara[6].Value = TxtColor.Text.ToUpper();
            _arrpara[7].Value = TxtShadeColor.Text.ToUpper();
            _arrpara[8].Value = TxtShape.Text.ToUpper();
            _arrpara[9].Value = TxtSize.Text.ToUpper();
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_ParameterSetting", _arrpara);
            Tran.Commit();
            Lblmsg.Text = "Data saved........";
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            Lblmsg.Text = ex.Message;
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/ParameterSetting.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
}