using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services.Description;

public partial class Masters_Campany_FrmLoommaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillGrid();
            fillGridForEdit();
        }
    }
    protected void FillGrid()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        string str = @"select '' as looms,'' Quality_ID,'' ITEM_ID,'' min_width,'' max_width,'' max_length,'' unit,'' date,''empid,''varuserid,''varcompanyid";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
        Gvloom.DataSource = ds.Tables[0];
        Gvloom.DataBind();

    }
    protected void fillGridForEdit()
    {
        string str = @"select DetailId,IM.Item_Id As ItemId,IM.ITEM_NAME,Q.QualityId,Q.QualityName,Looms,Min_Width,Max_Width,Max_length,U.UnitId,U.unitname from loommaster LM,Item_Master IM,Quality Q,Unit U
                     Where LM.Item_Id=IM.Item_Id And LM.Quality_Id=q.QualityId And LM.Unit=U.UnitId And Empid=" + Request.QueryString["a"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGLoomDetils.DataSource = ds;
        DGLoomDetils.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {
            DivEdit.Style.Add("Display", "Block");
        }
        else
        {
            DivEdit.Style.Add("Display", "None");
        }
    }
    protected void GridView1_Load(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void DDitem_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow Gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
        DropDownList dditem = (DropDownList)sender;
        DropDownList ddquality = ((DropDownList)Gvr.FindControl("DDquality"));
        UtilityModule.ConditionalComboFill(ref ddquality, "select QualityId,QualityName from quality where Item_id=" + dditem.SelectedValue + " And MasterCompanyId=" + Session["varcompanyid"] + " order by QualityName", true, "--Plz Select Quality--");

    }
    protected void DDitem_SelectedIndexChangedForEdit(object sender, EventArgs e)
    {
        GridViewRow Gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
        DropDownList dditem = (DropDownList)sender;
        DropDownList DDquality = ((DropDownList)Gvr.FindControl("DDquality"));
        UtilityModule.ConditionalComboFill(ref DDquality, "select QualityId,QualityName from quality where Item_id=" + dditem.SelectedValue + " And MasterCompanyId=" + Session["varcompanyid"] + " order by QualityName", true, "--Plz Select Quality--");

    }
    protected void Gvloom_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlitem = (e.Row.FindControl("DDitem") as DropDownList);
            DropDownList ddlunit = (e.Row.FindControl("DDunit") as DropDownList);
            UtilityModule.ConditionalComboFill(ref ddlitem, "select Item_Id,Item_Name from Item_Master  Where MasterCompanyId=" + Session["varcompanyId"] + " Order by Item_name", true, "--Plz Select Item--");
            UtilityModule.ConditionalComboFill(ref ddlunit, "select Unitid,Unitname from Unit Where MasterCompanyId=" + Session["varcompanyId"] + "Order by Unitname", true, "--Plz Select Unit--");

        }
    }
    protected void btnclose_click(object sender, EventArgs e)
    {
        DivLooMDetails.Visible = false;
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        //Validate();
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[11];
                _arrPara[0] = new SqlParameter("@ITEM_ID", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@Quality_ID", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@looms", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@min_width", SqlDbType.VarChar, 100);
                _arrPara[4] = new SqlParameter("@max_width", SqlDbType.VarChar, 100);
                _arrPara[5] = new SqlParameter("@max_length", SqlDbType.VarChar, 100);
                _arrPara[6] = new SqlParameter("@unit", SqlDbType.VarChar, 100);
                _arrPara[7] = new SqlParameter("@empid", SqlDbType.Int);
                _arrPara[8] = new SqlParameter("@userid", SqlDbType.Int);
                _arrPara[9] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
                _arrPara[10] = new SqlParameter("@msg", SqlDbType.VarChar, 100);



                _arrPara[7].Value = Request.QueryString["a"].ToString();
                _arrPara[8].Value = Session["varuserid"].ToString();
                _arrPara[9].Value = Session["varCompanyId"].ToString();
                _arrPara[10].Direction = ParameterDirection.Output;

                for (int i = 0; i < Gvloom.Rows.Count; i++)
                {
                    DropDownList DDitem = ((DropDownList)Gvloom.Rows[i].FindControl("DDitem"));
                    DropDownList DDquality = ((DropDownList)Gvloom.Rows[i].FindControl("DDquality"));

                    //_arrPara[0].Value = ((DropDownList)Gvloom.Rows[i].FindControl("DDitem")).SelectedValue;
                    //_arrPara[1].Value = ((DropDownList)Gvloom.Rows[i].FindControl("DDquality")).SelectedValue;
                    _arrPara[0].Value = DDitem.SelectedIndex <= 0 ? "0" : DDitem.SelectedValue;
                    _arrPara[1].Value = DDquality.SelectedIndex <= 0 ? "0" : DDquality.SelectedValue;
                    _arrPara[2].Value = ((TextBox)Gvloom.Rows[i].FindControl("txtlooms")).Text;
                    _arrPara[3].Value = ((TextBox)Gvloom.Rows[i].FindControl("txtminwidth")).Text;
                    _arrPara[4].Value = ((TextBox)Gvloom.Rows[i].FindControl("txtmaxwidth")).Text;
                    _arrPara[5].Value = ((TextBox)Gvloom.Rows[i].FindControl("txtmaxlength")).Text;
                    DropDownList DDunit = ((DropDownList)Gvloom.Rows[i].FindControl("DDunit"));
                    _arrPara[6].Value = DDunit.SelectedIndex <= 0 ? "0" : DDunit.SelectedValue;

                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_loommaster", _arrPara);
                    Tran.Commit();
                    lblErr.Text = _arrPara[10].Value.ToString();
                    fillGridForEdit();
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmloommaster.aspx");
                Tran.Rollback();

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void DGLoomDetils_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGLoomDetils.EditIndex = e.NewEditIndex;

        //Bind data to the GridView control.
        fillGridForEdit();
    }
    protected void DGLoomDetils_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGLoomDetils.EditIndex = -1;
        //Bind data to the GridView control.
        fillGridForEdit();
    }
    protected void DGLoomDetils_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int DetailId = Convert.ToInt16(DGLoomDetils.DataKeys[e.RowIndex].Value);


            TextBox txtloom = ((TextBox)DGLoomDetils.Rows[e.RowIndex].FindControl("txtloom"));
            TextBox txtminwidth = ((TextBox)DGLoomDetils.Rows[e.RowIndex].FindControl("txtminwidth"));
            TextBox txtmaxwidth = ((TextBox)DGLoomDetils.Rows[e.RowIndex].FindControl("txtmaxwidth"));
            TextBox txtmaxlength = ((TextBox)DGLoomDetils.Rows[e.RowIndex].FindControl("txtmaxlength"));
            DropDownList DDitem = ((DropDownList)DGLoomDetils.Rows[e.RowIndex].FindControl("DDitem"));
            DropDownList DDquality = ((DropDownList)DGLoomDetils.Rows[e.RowIndex].FindControl("DDquality"));

            string str = "Update loommaster set looms='" + txtloom.Text + "',Min_Width='" + txtminwidth.Text + "',Max_Width='" + txtmaxwidth.Text + "',Max_length='" + txtmaxlength.Text + "',ITEM_ID=" + DDitem.SelectedValue + ",Quality_ID=" + DDquality.SelectedValue + " where DetailId=" + DetailId;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            Tran.Commit();
            lblErr.Text = "Data updated successfully........";
            DGLoomDetils.EditIndex = -1;
            fillGridForEdit();

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblErr.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void DGLoomDetils_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {

            int DetailId = Convert.ToInt16(DGLoomDetils.DataKeys[e.RowIndex].Value);


            string str = "Delete from loommaster where DetailId=" + DetailId;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            Tran.Commit();
            lblErr.Text = "Data Deleted successfully........";

            fillGridForEdit();

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblErr.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }

    }
    protected void DGLoomDetils_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlitem1 = (e.Row.FindControl("DDitem") as DropDownList);
            DropDownList DDquality = (e.Row.FindControl("DDquality") as DropDownList);
            DropDownList ddlunit = (e.Row.FindControl("DDunit") as DropDownList);
            Label lblitemid = (e.Row.FindControl("lblItemId") as Label);
            Label lblUnitId = (e.Row.FindControl("lblUnitId") as Label);
            Label lblQualityId = (e.Row.FindControl("lblQualityId") as Label);

            UtilityModule.ConditionalComboFill(ref ddlitem1, "select Item_Id,Item_Name from Item_Master  Where MasterCompanyId=" + Session["varcompanyId"] + " Order by Item_name", true, "--Plz Select Item--");
            UtilityModule.ConditionalComboFill(ref ddlunit, "select Unitid,Unitname from Unit Where MasterCompanyId=" + Session["varcompanyId"] + "Order by Unitname", true, "--Plz Select Unit--");
            ddlitem1.SelectedValue = lblitemid.Text;
            ddlunit.SelectedValue = lblUnitId.Text;
            UtilityModule.ConditionalComboFill(ref DDquality, "select QualityId,QualityName from quality where Item_id=" + ddlitem1.SelectedValue + " And MasterCompanyId=" + Session["varcompanyid"] + " order by QualityName", true, "--Plz Select Quality--");
            DDquality.SelectedValue = lblQualityId.Text;
        }
    }

    protected void txtenterContent_TextChanged(object sender, EventArgs e)
    {

        string str = @"select EI.EmpName Weaver,EI.Address Village,QM.QualityName,Looms,Max_Width As MaxWidth,Min_Width As MinWidth,Max_Length As MaxLength from LoomMaster LM,Quality QM,Empinfo EI
                          Where LM.EmpId=EI.EmpId And QM.QualityId=LM.Quality_id";
        if (RDQuality.Checked == true)
        {
            str = str + "  And QM.QualityName like '%" + txtenterContent.Text + "%'";
        }
        if (RdVillage.Checked == true)
        {
            str = str + "  And EI.Address like '%" + txtenterContent.Text + "%'";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gdLoomDetails.DataSource = ds;
            gdLoomDetails.DataBind();
            DivLooMDetails.Visible = true;

        }
        else
        {
            DivLooMDetails.Visible = false;
        }

    }
}