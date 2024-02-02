using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_frmitemspecification : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            UtilityModule.ConditionalComboFill(ref ddcategory, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "---Select --");
        }
    }
    protected void ddcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dditem, "select ITEM_ID,ITEM_NAME from item_master where CATEGORY_ID=" + ddcategory.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ITEM_NAME", true, "--Select ItemName--");

    }
    protected void BtnShowData_Click(object sender, EventArgs e)
    {
        selectall.Visible = true;
        ShowDataInGrid();
    }
    private void ShowDataInGrid()
    {

        string str = "";
        try
        {

            str = @"select Item_Id,QualityName,QualityId  from Quality where Item_id=" + dditem.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            DGDetail.DataSource = Ds;
            DGDetail.DataBind();
            str = "select QualityId from Employee_ItemDetail where ItemId=" + dditem.SelectedValue + " And EmpId=" + Request.QueryString["a"] + "";
            Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (Ds.Tables[0].Rows.Count>0)
            {
                for (int i = 0; i < DGDetail.Rows.Count; i++)
                {
                    for (int j = 0; j < Ds.Tables[0].Rows.Count; j++)
                    {
                        if (Ds.Tables[0].Rows[j]["QualityId"].ToString() == ((Label)DGDetail.Rows[i].FindControl("lblqualityid")).Text)
                        {
                            ((CheckBox)DGDetail.Rows[i].FindControl("Chkbox")).Checked = true;
                        }
                      
                    }
                }

            }
        }
        catch (Exception ex)
        {
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
        }
    }
    protected void dditem_SelectedIndexChanged(object sender, EventArgs e)
    {

        ShowDataInGrid();
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
            SqlParameter[] _arrpara = new SqlParameter[6];
            _arrpara[0] = new SqlParameter("@EmpId", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@CategoryId", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@ItemId", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@QualityId", SqlDbType.Int);
            _arrpara[4] = new SqlParameter("@UserId", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);

            _arrpara[0].Value = Request.QueryString["a"].ToString();
            _arrpara[1].Value = ddcategory.SelectedValue;

            _arrpara[4].Value = Session["varuserid"].ToString();
            _arrpara[5].Value = Session["varMasterCompanyIDForERP"].ToString();

            //Delete existing Record
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete from Employee_ItemDetail Where ItemId=" + dditem.SelectedValue + "");
            string strinsert="";
            for (int i = 0; i < DGDetail.Rows.Count; i++)
            {
                if (((CheckBox)DGDetail.Rows[i].FindControl("Chkbox")).Checked == true)
                {
                    _arrpara[2].Value = ((Label)DGDetail.Rows[i].FindControl("lblitemid")).Text;
                    _arrpara[3].Value = ((Label)DGDetail.Rows[i].FindControl("lblqualityid")).Text;

                    strinsert = strinsert + " insert into Employee_ItemDetail(EmpId,CategoryId,ItemId,QualityId,UserId,MasterCompanyId)values(" + _arrpara[0].Value + "," + _arrpara[1].Value + "," + _arrpara[2].Value + "," + _arrpara[3].Value + ", " + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ")";
                    //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "insert into Employee_ItemDetail(EmpId,CategoryId,ItemId,QualityId,UserId,MasterCompanyId)values(" + _arrpara[0].Value + "," + _arrpara[1].Value + "," + _arrpara[2].Value + "," + _arrpara[3].Value + ", " + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ")");
                }
            }
            if (strinsert!="")
            {
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, strinsert);
            }

            Tran.Commit();
            lblMessage.Visible = true;
            lblMessage.Text = "Data Successfully Saved.......";

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Masters/Campany/frmitemspecification.aspx");
            Tran.Rollback();
            lblMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }


    }
    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGDetail, "Select$" + e.Row.RowIndex);
        }

    }
    protected void DGDetail_RowCreated(object sender, GridViewRowEventArgs e)
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