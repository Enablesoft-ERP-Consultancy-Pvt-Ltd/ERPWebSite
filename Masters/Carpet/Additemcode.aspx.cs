using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Masters_Carpet_itemcode : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            lablechange();
            UtilityModule.ConditionalComboFill(ref DDCategory, "Select DISTINCT CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And CS.Id=0 And IM.MasterCompanyId=" + Session["varCompanyId"] + "  Order by CATEGORY_ID", true, "--Select--");
            string s = Request.QueryString["Category"];
            DDCategory.SelectedValue = s;
            UtilityModule.ConditionalComboFill(ref DDItemName, "Select Item_Id,Item_Name from Item_Master Where Category_Id=" + DDCategory.SelectedValue + "  And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
            string s2 = Request.QueryString["Item"];
            DDItemName.SelectedValue = s2;
            UtilityModule.ConditionalComboFill(ref DdQuality, @"SELECT QualityId,QualityName FROM Quality Where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyid=" + Session["varCompanyId"] + "", true, "--Select--");
            string s3 = Request.QueryString["Quality"];
            DdQuality.SelectedValue = s3;
            ErrorMessage.Visible = false;
            CommanFunction.FillCombo(DDUnit, "select UnitId,UnitName from Unit where UnitTypeId=1");
            fill_QualityCode();
            fill_grid1();
        }
        //fill_grid();
        Session["ReportPath"] = "Reports/Rptitemcode.rpt";
        Session["CommanFormula"] = "";
        TxtCode.Visible = false;
        Txtitemcode.Visible = false;
    }
    public void lablechange()
    {
        try
        {
            String[] ParameterList = new String[8];
            ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
            lblqualityname.Text = ParameterList[0];
            lblcategoryname.Text = ParameterList[5];
            lblitemname.Text = ParameterList[6];
            subquality.Text = "SUB_" + lblqualityname.Text;
            qualitycode.Text = lblqualityname.Text;


        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Additemcode.aspx");
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('" + ex.Message + "');", true);
            // Lblerrer.Visible = true;
            //  Lblerrer.Text = "Data base errer..................";
        }       
    }

    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtFinishedid.Text = "Category=" + DDCategory.SelectedValue.ToString();
        UtilityModule.ConditionalComboFill(ref DDItemName, "Select Item_Id,Item_Name from Item_Master Where Category_Id=" + DDCategory.SelectedValue + " And MasterCompanyid= " + Session["varCompanyId"] + "", true, "--Select--");
        // fill_grid1();        
    }

    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtqlt.Text = "Item=" + DDItemName.SelectedValue + "&Category=" + DDCategory.SelectedValue.ToString();
        UtilityModule.ConditionalComboFill(ref DdQuality, @"SELECT QualityId,QualityName FROM Quality Where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyid=" + Session["varCompanyId"] + "" , true, "--Select--");
        fill_grid();
        ErrorMessage.Visible = false;
    }

    private void fill_QualityCode()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            TxtCode.Text = SqlHelper.ExecuteScalar(con, CommandType.Text, "Select Item_Code from Item_Master IM INNER JOIN Quality Q ON IM.Item_Id=Q.Item_Id Where Q.Qualityid=" + DdQuality.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] +"").ToString();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Additemcode.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void DdQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        ErrorMessage.Visible = false;
        fill_QualityCode();
        fill_grid1();
    }

    protected void fill_grid1()
    {
        Gvitemdetail.DataSource = getdetail();
        Gvitemdetail.DataBind();
    }

    protected DataSet getdetail()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select distinct Q.QualityId Quality_Id,Item_Name,QualityName SubItemName from Item_Master IM,CategorySeparate CS,Quality Q Where IM.Category_Id=CS.CategoryId And IM.Item_Id=Q.Item_Id And CS.Id=1 And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order By Item_Name");
        }

        catch (Exception ex)
        {
            Logs.WriteErrorLog("error");
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Additemcode.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

        return ds;
    }

    protected void Gditemcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = Gditemcode.SelectedDataKey.Value.ToString();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = null;
        DataSet ds1 = null;
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT SUBQUANTITY,QUANTITY,qualityid,qualitycode,UnitId from qualitycodemaster where qualitycodeid=" + Gditemcode.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "");
        ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT Quality_Id,Percentage from qualitycodeDetail where qualitycodeid=" + Gditemcode.SelectedValue + " Order By Quality_Id");
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                // DDItemName.Text = ds.Tables[0].Rows[0]["typeid"].ToString();
                //UtilityModule.ConditionalComboFill(ref DdQuality, "SELECT  distinct   dbo.Quality.QualityId, dbo.Quality.QualityName FROM  dbo.ITEM_CATEGORY_MASTER INNER JOIN dbo.ITEM_MASTER ON dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID = dbo.ITEM_MASTER.CATEGORY_ID INNER JOIN dbo.ITEM_PARAMETER_MASTER ON dbo.ITEM_MASTER.ITEM_ID = dbo.ITEM_PARAMETER_MASTER.ITEM_ID INNER JOIN dbo.Quality ON dbo.ITEM_PARAMETER_MASTER.QUALITY_ID = dbo.Quality.QualityId where dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID=" + DDItemName.SelectedValue, true, "--Select--");
                UtilityModule.ConditionalComboFill(ref DdQuality, @"SELECT QualityId,QualityName FROM Quality Where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
                TxtCode.Text = ds.Tables[0].Rows[0]["qualitycode"].ToString();
                DdQuality.SelectedValue = ds.Tables[0].Rows[0]["qualityId"].ToString();
                DDUnit.SelectedValue = ds.Tables[0].Rows[0]["UnitId"].ToString();
                TxtQuantity.Text = ds.Tables[0].Rows[0]["Quantity"].ToString();
                Txtitemcode.Text = ds.Tables[0].Rows[0]["subQuantity"].ToString();
                Btnsave.Text = "Update";
            }
            int n = Gvitemdetail.Rows.Count;
            int j = 0;
            int num = ds1.Tables[0].Rows.Count;
            for (int i = 0; i <= n && j < num; i++)
            {
                GridViewRow row = Gvitemdetail.Rows[i];
                int strID = Convert.ToInt32(Gvitemdetail.Rows[i].Cells[0].Text);
                string strname = Gvitemdetail.Rows[i].Cells[1].Text;
                if (strID == Convert.ToInt32(ds1.Tables[0].Rows[j]["Quality_Id"]))
                {
                    ((TextBox)Gvitemdetail.Rows[i].FindControl("txtp_tage")).Text = ds1.Tables[0].Rows[j]["Percentage"].ToString();
                    j++;
                }
                else
                {
                    ((TextBox)Gvitemdetail.Rows[i].FindControl("txtp_tage")).Text = "";
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Additemcode.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void Btnsave_Click(object sender, EventArgs e)
    {
        TxtCode.Visible = true;
        Txtitemcode.Visible = true;
        VALIDATE__CODE();
        string Str = "Select * from qualityCodeMaster Where Item_Id=" + DDItemName.SelectedValue + " And Qualityid=" + DdQuality.SelectedValue + " And SubQuantity='" + Txtitemcode.Text + "'";
        if (Btnsave.Text == "Update")
        {
            Str = Str + @" And QualityCodeid<>" + Gditemcode.SelectedValue + "";
        }
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            ErrorMessage.Visible = true;
            lblErrorMessage.Text = "Quality Code AllReady Exits......";
            TxtCode.Visible = false;
            Txtitemcode.Visible = false;
        }
        else
        {
            if (lblErrorMessage.Visible == false)
            {
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                try
                {
                    SqlParameter[] _arrPara = new SqlParameter[9];
                    _arrPara[0] = new SqlParameter("@QualityCodeId", SqlDbType.Int);
                    _arrPara[1] = new SqlParameter("@Item_Id", SqlDbType.Int);
                    _arrPara[2] = new SqlParameter("@qualityId", SqlDbType.Int);
                    _arrPara[3] = new SqlParameter("@Subquality", SqlDbType.NVarChar, 150);
                    _arrPara[4] = new SqlParameter("@quantity", SqlDbType.Float);
                    _arrPara[5] = new SqlParameter("@QualityCode", SqlDbType.NVarChar, 150);
                    _arrPara[6] = new SqlParameter("@varuserid", SqlDbType.Int);
                    _arrPara[7] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                    _arrPara[8] = new SqlParameter("@UnitId", SqlDbType.Int);

                    _arrPara[1].Value = DDItemName.SelectedValue;
                    _arrPara[2].Value = DdQuality.SelectedValue;
                    _arrPara[3].Value = Txtitemcode.Text;
                    _arrPara[4].Value = TxtQuantity.Text;
                    _arrPara[5].Value = TxtCode.Text;
                    _arrPara[6].Value = Session["varuserid"].ToString();
                    _arrPara[7].Value = Session["varCompanyId"].ToString();
                    _arrPara[8].Value = DDUnit.SelectedValue;

                    if (Btnsave.Text == "Update")
                    {
                        _arrPara[0].Value = Gditemcode.SelectedValue;
                    }
                    else
                    {
                        _arrPara[0].Value = 0;
                    }
                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_QualityCode", _arrPara);
                    ErrorMessage.Visible = false;
                    fillDetail();
                    Btnsave.Text = "Save";
                    TxtCode.Visible = false;
                    Txtitemcode.Visible = false;
                    refreshform();
                }
                catch (Exception ex)
                {
                    UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Additemcode.aspx");
                    ErrorMessage.Visible = true;
                    lblErrorMessage.Text = "Some IMportent Field Missing.........";
                    TxtCode.Visible = false;
                    Txtitemcode.Visible = false;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            fill_grid();
        }
    }

    public void VALIDATE__CODE()
    {
        SqlConnectionStringBuilder strSql = new SqlConnectionStringBuilder(string.Empty);
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        SqlCommand cmd = new SqlCommand();
        Txtitemcode.Text = null;
        TxtCode.Text = null;
        string str = SqlHelper.ExecuteScalar(con, CommandType.Text, "Select Item_Code from Item_Master IM INNER JOIN Quality Q ON IM.Item_Id=Q.Item_Id Where Q.Qualityid=" + DdQuality.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + "").ToString();
        int n = Gvitemdetail.Rows.Count;
        int sum = 0;
        int a;
        int VarLength = DdQuality.SelectedValue.ToString().Length;
        switch (VarLength)
        {
            case 1:
                str = str + " 000" + (DdQuality.SelectedValue.ToString());
                break;
            case 2:
                str = str + " 00" + (DdQuality.SelectedValue.ToString());
                break;
            case 3:
                str = str + " " + (DdQuality.SelectedValue.ToString());
                break;
        }
        //str =str+" 000"+(DdQuality.SelectedValue.ToString());
        for (int i = 0; i < n; i++)
        {
            GridViewRow row = Gvitemdetail.Rows[i];
            string strID = Gvitemdetail.Rows[i].Cells[0].Text;
            string strname = Gvitemdetail.Rows[i].Cells[2].Text;
            string strpercentage = ((TextBox)Gvitemdetail.Rows[i].FindControl("txtp_tage")).Text;
            if (strpercentage != "" && strpercentage != "0")
            {
                a = Convert.ToInt32(strpercentage);
                if (Txtitemcode.Text == "")
                {
                    Txtitemcode.Text = (strpercentage + "% " + strname);
                }
                else
                {
                    Txtitemcode.Text = (Txtitemcode.Text + " + " + strpercentage + "% " + strname);
                }
                sum += a;
                VarLength = strID.Length;
                switch (VarLength)
                {
                    case 1:
                        str = str + " " + strpercentage + "%" + "000" + (strID);
                        break;
                    case 2:
                        str = str + " " + strpercentage + "%" + "00" + (strID);
                        break;
                    case 3:
                        str = str + " " + strpercentage + "%" + (strID);
                        break;
                }
            }
            TxtCode.Text = str;
        }
        if (sum != 100)
        {
            ErrorMessage.Visible = true;
            lblErrorMessage.Text = "PERCENTAGE TOTAL SHOULD BE EQUAL TO 100.....";
            TxtCode.Visible = false;
            Txtitemcode.Visible = false;
        }
        else
        {
            ErrorMessage.Visible = false;
        }
    }

    protected void fill_grid()
    {
        Gditemcode.DataSource = getCode();
        Gditemcode.DataBind();
    }

    protected DataSet getCode()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, @"Select QM.QualityCodeId as Sr_No,Q.QualityName as " + lblqualityname.Text + @",QM.SubQuantity as " + subquality.Text + @",
                QM.QualityCode as " + qualitycode.Text + @",QM.Quantity from Item_Master I,Quality Q,QualityCodeMaster QM Where I.Item_Id=QM.Item_Id And
                Q.Qualityid=QM.Qualityid And QM.Item_Id=" + DDItemName.SelectedValue + " And I.MasterCompanyId=" + Session["varCompanyId"] + "");
        }

        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Additemcode.aspx");
            Logs.WriteErrorLog("error");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return ds;
    }

    //********************************************************************************************************************/+
    public void fillDetail()
    {
        SqlConnectionStringBuilder strSql = new SqlConnectionStringBuilder(string.Empty);
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        int codeid = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select max(QualityCodeId) from qualityCodeMaster"));
        int detailid = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select isnull(max(detailId),0)+1 from QualityCodeDetail"));
        con.Close();
        SqlCommand cmd = new SqlCommand();
        Txtitemcode.Text = null;
        int n = Gvitemdetail.Rows.Count;
        int a;
        string str = "000" + (DdQuality.SelectedValue.ToString());
        try
        {
            if (Btnsave.Text == "Update")
            {
                SqlHelper.ExecuteNonQuery(con, CommandType.Text, "Delete From  QualityCodeDetail where QualityCodeId=" + Gditemcode.SelectedValue);
                codeid = Convert.ToInt32(Gditemcode.SelectedValue);
            }
            for (int i = 0; i < n; i++)
            {
                GridViewRow row = Gvitemdetail.Rows[i];
                int strID = Convert.ToInt32(Gvitemdetail.Rows[i].Cells[0].Text);
                string strname = Gvitemdetail.Rows[i].Cells[1].Text;
                string strpercentage = ((TextBox)Gvitemdetail.Rows[i].FindControl("txtp_tage")).Text;
                if (strpercentage != "")
                {
                    a = Convert.ToInt32(strpercentage);
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, "Insert INTO QualityCodeDetail (detailId,QualityCodeId,Quality_Id,Percentage) values (" + detailid + "," + codeid + "," + strID + "," + Convert.ToInt32(strpercentage) + ")");
                    detailid++;
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Additemcode.aspx");

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    private void refreshform()
    {
        TxtQuantity.Text = "";
        TxtCode.Text = "";
        Txtitemcode.Text = "";
        fill_grid1();
    }

    protected void Gditemcode_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Gditemcode, "Select$" + e.Row.RowIndex);
        }
    }       
    protected void refreshcategory_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            int catgid = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select max(CATEGORY_ID) from ITEM_CATEGORY_MASTER"));
            int srno = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, " select  ISnull (max(Sr_No),0)+1 from CategorySeparate"));

            string str2 = "insert into CategorySeparate values(" + srno + ",0," + catgid + "," + Session["varuserid"] + "," + Session["varCompanyId"] + ")";
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, str2);
        }

        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Additemcode.aspx");
            Logs.WriteErrorLog("error");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

        UtilityModule.ConditionalComboFill(ref DDCategory, "Select DISTINCT CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And CS.Id=0 And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order by CATEGORY_ID", true, "--Select--");
    }
    protected void refreshitem_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "Select Item_Id,Item_Name from Item_Master Where Category_Id=" + DDCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
    }
    protected void refreshquality_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DdQuality, @"SELECT QualityId,QualityName FROM Quality Where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] +"", true, "--Select--");
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void Gvitemdetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    protected void Gditemcode_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
}