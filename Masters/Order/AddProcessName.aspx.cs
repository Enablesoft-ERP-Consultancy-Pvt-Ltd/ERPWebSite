using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ClosedXML.Excel;
using System.IO;

public partial class Masters_Order_AddProcessName : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditonalChkBoxListFill(ref ChkBoxListProcessName, @"Select PROCESS_NAME_ID, PROCESS_NAME 
                        From PROCESS_NAME_MASTER(Nolock) Where IsNull(AddProcessName, 0) <> 0 And MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By PROCESS_NAME");

            if (Request.QueryString["OrderID"] != null)
            {
                DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select Distinct a.ProcessID 
                    From OrderDetailWithProcessID a(nolock) 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = a.Item_Finished_ID And 
                            VF.CATEGORY_ID = " + Request.QueryString["CategoryID"] + " And VF.ITEM_ID = " + Request.QueryString["ItemID"] + @" And 
                            VF.QualityID = " + Request.QueryString["QualityID"] + " And VF.DesignID = " + Request.QueryString["DesignID"] + @" And 
                            VF.ColorID = " + Request.QueryString["ColorID"] + @" 
                        Where OrderID = " + Request.QueryString["OrderID"] + "  Order By ProcessID");
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < ChkBoxListProcessName.Items.Count; j++)
                        {
                            if (Convert.ToInt32(ChkBoxListProcessName.Items[j].Value) == Convert.ToInt32(Ds.Tables[0].Rows[i]["ProcessID"]))
                            {
                                ChkBoxListProcessName.Items[j].Selected = true;
                            }
                        }
                    }
                }
            }
        }
    }
    protected void ChkForAllSelect_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForAllSelect.Checked == true)
        {
            for (int j = 0; j < ChkBoxListProcessName.Items.Count; j++)
            {
                ChkBoxListProcessName.Items[j].Selected = true;
            }
        }
        else
        {
            for (int j = 0; j < ChkBoxListProcessName.Items.Count; j++)
            {
                ChkBoxListProcessName.Items[j].Selected = false;
            }
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
            string VarProcessID = "";
            for (int i = 0; i < ChkBoxListProcessName.Items.Count; i++)
            {
                if (ChkBoxListProcessName.Items[i].Selected)
                {
                    if (VarProcessID == "")
                    {
                        VarProcessID = ChkBoxListProcessName.Items[i].Value + '|';
                    }
                    else
                    {
                        VarProcessID = VarProcessID + ChkBoxListProcessName.Items[i].Value + '|';
                    }
                }
            }
            SqlParameter[] param = new SqlParameter[10];
            param[0] = new SqlParameter("@OrderID", Request.QueryString["OrderID"]);
            param[1] = new SqlParameter("@CategoryID", Request.QueryString["CategoryID"]);
            param[2] = new SqlParameter("@ItemID", Request.QueryString["ItemID"]);
            param[3] = new SqlParameter("@QualityID", Request.QueryString["QualityID"]);
            param[4] = new SqlParameter("@DesignID", Request.QueryString["DesignID"]);
            param[5] = new SqlParameter("@ColorID", Request.QueryString["ColorID"]);
            param[6] = new SqlParameter("@UserID", Session["varuserid"]);
            param[7] = new SqlParameter("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
            param[8] = new SqlParameter("@ProcessIDs", VarProcessID);
            param[9] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[9].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveOrderDetailWithProcessID", param);
            Tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('" + param[9].Value.ToString() + "');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('" + ex.Message + "');", true);
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
}