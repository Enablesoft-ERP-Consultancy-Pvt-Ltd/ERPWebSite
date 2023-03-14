using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class OtherExpense : System.Web.UI.Page
{
    int CWOEID = 0;
    string Msg = "";
    static int MasterCompanyId;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varCompanyId"]);
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            lablechange();
            UtilityModule.ConditionalComboFill(ref ddCategoryName, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order by CATEGORY_Id", true, "--SELECT--");
            UtilityModule.ConditionalComboFill(ref ddchangename, "SELECT ExpID,ChargeName from ExpenseName Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT CHARGE--");
            TxtProdCode.Text = Request.QueryString["ProdCode"];
            Prod_Code_Text_Change();
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        lblsizename.Text = ParameterList[4];
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
    }

    protected void ddcategoryname_SelectedIndexChanged(object sender, EventArgs e)
    {
        CATEGORY_DEPENDS_CONTROLS();
    }
    private void CATEGORY_DEPENDS_CONTROLS()
    {
        Quality.Visible = false;
        Design.Visible = false;
        Color.Visible = false;
        Shape.Visible = false;
        Size.Visible = false;
        Shade.Visible = false;
        UtilityModule.ConditionalComboFill(ref ddItemName, "Select ITEM_ID,ITEM_NAME from ITEM_MASTER Where CATEGORY_ID=" + ddCategoryName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from ITEM_CATEGORY_PARAMETERS Where CATEGORY_ID=" + ddCategoryName.SelectedValue + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in Ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        Quality.Visible = true;
                        break;
                    case "2":
                        Design.Visible = true;
                        break;
                    case "3":
                        Color.Visible = true;
                        break;
                    case "4":
                        Shape.Visible = true;
                        break;
                    case "5":
                        Size.Visible = true;
                        break;
                    case "6":
                        Shade.Visible = true;
                        break;
                }
            }
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(ddQuality, ddDesign, ddColor, ddShape, ddShade, Convert.ToInt32(ddItemName.SelectedValue));
        FILLGRID();
    }
    private void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Shade, int Itemid)
    {
        UtilityModule.ConditionalComboFill(ref Quality, "SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + Itemid + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref Design, "SELECT DESIGNID,DESIGNNAME from DESIGN Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref Color, "SELECT COLORID,COLORNAME FROM COLOR Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref Shape, "SELECT SHAPEID,SHAPENAME FROM SHAPE Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref Shade, "SELECT ShadecolorId,ShadeColorName FROM ShadeColor Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SIZEID,SIZEFT FROM SIZE WhERE SHAPEID=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        Prod_Code_Text_Change();
    }
    private void Prod_Code_Text_Change()
    {
        DataSet ds;
        string Str;
        if (TxtProdCode.Text != "")
        {
            ddCategoryName.SelectedIndex = 0;
            Str = "select IPM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM where IPM.ITEM_ID=IM.ITEM_ID and ProductCode='" + TxtProdCode.Text + "' And IM.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                CATEGORY_DEPENDS_CONTROLS();
                ddItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                FILLGRID();
                QDCSDDFill(ddQuality, ddDesign, ddColor, ddShape, ddShade, Convert.ToInt32(ddItemName.SelectedValue));
                ddQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                ddDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                ddColor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                ddShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SIZEID,SIZEFT fROM SIZE WhERE SHAPEID=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
                ddSize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
            }
            else
            {
                lblMessage.Text = "ITEM CODE DOES NOT EXISTS....";
                ddCategoryName.SelectedIndex = 0;
                CATEGORY_DEPENDS_CONTROLS();
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
            }
        }
        else
        {
            ddCategoryName.SelectedIndex = 0;
            CATEGORY_DEPENDS_CONTROLS();
        }
    }
    //protected void ddchangename_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    txtpercentage.Text = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT IsNull(Percentage,0) from ExpenseName WHERE ExpID=" + ddchangename.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
    //}
    protected void btnsave_Click(object sender, EventArgs e)
    {

        CHECKVALIDCONTROL();
        if (lblMessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                TxtProdCode.Text = "";
                SqlParameter[] _arrpara = new SqlParameter[9];
                _arrpara[0] = new SqlParameter("@CWOEID", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@COMPANYID", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@CUSTOMERID", SqlDbType.Int);
                _arrpara[3] = new SqlParameter("@FINISHEDID", SqlDbType.Int);
                _arrpara[4] = new SqlParameter("@EXPID", SqlDbType.Int);
                _arrpara[5] = new SqlParameter("@PERCENTAGE", SqlDbType.Float);
                _arrpara[6] = new SqlParameter("@ID", SqlDbType.Int);
                _arrpara[7] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrpara[8] = new SqlParameter("@varCompanyId", SqlDbType.Int);

                _arrpara[0].Value = CWOEID;
                //_arrpara[1].Value = ddcomapnyname.SelectedValue;
                //_arrpara[2].Value = ddcustomercode.SelectedValue;
                _arrpara[1].Value = 0;
                _arrpara[2].Value = 0;
                int Varfinishedid = UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, ddShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                _arrpara[3].Value = Varfinishedid;
                _arrpara[4].Value = ddchangename.SelectedValue;
                _arrpara[5].Value = 0;// txtpercentage.Text;
                _arrpara[6].Direction = ParameterDirection.Output;
                _arrpara[7].Value = Session["varuserid"].ToString();
                _arrpara[8].Value = Session["varCompanyId"].ToString();
                //Select CWOEID,COMPANYID,CUSTOMERID,FINISHEDID,EXPID,PERCENTAGE from CUSTWISEOTHEREXPENCE
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_CUSTWISEOTHEREXPENCE]", _arrpara);
                int ID = Convert.ToInt32(_arrpara[6].Value);
                if (ID == 0)
                {
                    lblMessage.Text = "DUPLICATE DATA EXISTS.....";
                }
                else if (ID == 1)
                {
                    lblMessage.Text = "DATA SAVED SUCESSFULLY....";
                }
                Tran.Commit();
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Carpet/OtherExpense.aspx");
                lblMessage.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                    con.Dispose();
                }
            }
            // txtpercentage.Text = "";
            Save_Refresh();
            FILLGRID();
        }
    }
    private void FILLGRID()
    {
        dgotherexpense.DataSource = Fill_Grid_Data();
        dgotherexpense.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {

            string strsql = @"SELECT CWOEID SRNO,IsNull(QualityName,'') QUALITY,IsNull(DesignName,'') DESIGN,IsNull(ColorName,'') COLOR,
            IsNull(ShapeName,'') SHAPE,IsNull(SizeFt,'') SIZE,CHARGENAME,CWE.PERCENTAGE from CUSTWISEOTHEREXPENCE CWE INNER JOIN EXPENSENAME E ON CWE.EXPID=E.EXPID INNER JOIN 
            Item_Parameter_Master IPM ON CWE.FINISHEDID=IPM.ITEM_FINISHED_ID INNER JOIN ITEM_MASTER IM ON IPM.ITEM_ID=IM.ITEM_ID Left Outer Join Quality Q ON 
            IPM.Quality_id=Q.Qualityid Left Outer Join Design D ON IPM.Design_id=D.Designid Left Outer Join Color C ON IPM.Color_id=C.Colorid Left Outer Join Shape Sh ON 
            IPM.Shape_id=Sh.Shapeid Left Outer Join Size S ON IPM.Size_id=S.Sizeid WHERE IM.CATEGORY_ID=" + ddCategoryName.SelectedValue + " AND IM.ITEM_ID=" + ddItemName.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"] + "and IPM.ProductCode='" + Request.QueryString["ProdCode"] + "'";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/OtherExpense.aspx");
            Logs.WriteErrorLog("Charge|Fill_Grid_Data|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        return ds;
    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        //if (UtilityModule.VALIDDROPDOWNLIST(ddcomapnyname) == false)
        //{
        //    goto a;
        //}
        //if (UtilityModule.VALIDDROPDOWNLIST(ddcustomercode) == false)
        //{
        //    goto a;
        //}        
        if (UtilityModule.VALIDDROPDOWNLIST(ddCategoryName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddItemName) == false)
        {
            goto a;
        }
        if (ddQuality.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddQuality) == false)
            {
                goto a;
            }
        }
        if (ddDesign.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddDesign) == false)
            {
                goto a;
            }
        }
        if (ddColor.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddColor) == false)
            {
                goto a;
            }
        }
        if (ddShape.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddShape) == false)
            {
                goto a;
            }
        }
        if (ddSize.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddSize) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddchangename) == false)
        {
            goto a;
        }
        //if (UtilityModule.VALIDTEXTBOX(txtpercentage) == false)
        //{
        //    goto a;
        //}
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
    }
    private void Save_Refresh()
    {
        ddchangename.SelectedIndex = 0;
        ddchangename.Focus();
    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        //ddcomapnyname.SelectedIndex = 0;
        //ddcustomercode .SelectedIndex = 0;
        ddCategoryName.SelectedIndex = 0;
        CATEGORY_DEPENDS_CONTROLS();
        //ddItemName.SelectedIndex = 0;
        ddchangename.SelectedIndex = 0;
        // txtpercentage.Text = "";
        TxtProdCode.Text = "";
        FILLGRID();
        ddcomapnyname.Focus();
        btnsave.Text = "Save";
    }
    protected void refresh_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddchangename, "SELECT ExpID,ChargeName from ExpenseName Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER Where MasterCompanyId=" + MasterCompanyId + " And  ProductCode Like  '" + prefixText + "%'";
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(strQuery, con);
        da.Fill(ds);
        count = ds.Tables[0].Rows.Count;
        con.Close();
        List<string> ProductCode = new List<string>();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            ProductCode.Add(ds.Tables[0].Rows[i][0].ToString());
        }
        con.Close();
        return ProductCode.ToArray();
    }
    protected void dgotherexpense_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void dgotherexpense_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        dgotherexpense.EditIndex = -1;
        FILLGRID();
    }
    protected void dgotherexpense_RowEditing(object sender, GridViewEditEventArgs e)
    {
        dgotherexpense.EditIndex = e.NewEditIndex;
        FILLGRID();
    }
    protected void dgotherexpense_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int SRNO = Convert.ToInt32(dgotherexpense.DataKeys[e.RowIndex].Value);
        TextBox TxtPercentage = (TextBox)dgotherexpense.Rows[e.RowIndex].FindControl("TxtPercentage");
        string Qry = " Update CUSTWISEOTHEREXPENCE SET PERCENTAGE=" + TxtPercentage.Text + " Where CWOEID=" + SRNO + "AND MasterCompanyid=" + Session["varCompanyId"];
        int i = SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Qry);
        FILLGRID();
        dgotherexpense.EditIndex = -1;
    }
    private void MessageSave(string msg)
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('");
        stb.Append(msg);
        stb.Append("');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
}
