using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;

public partial class FrmShadeColorReceipe : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx"); 
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref ddcategory, "Select Distinct CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER where mastercompanyId=" + Session["varcompanyId"] + " order by category_name", true, "--Select--");
            fillgrid();
        }
    }

    protected void fillgrid()
    {
        string str = @"Select dqr.SHADECOLORID AS ID,DQR.SHADECOLORNAME,dqr.brandid,dqr.itemid,br.brandName,qr.ITEM_NAME,dqr.DateAdded from ShadeColor_res dqr  inner join ITEM_MASTER qr 
                    on dqr.itemid=qr.ITEM_ID inner join brand br on 
                    dqr.brandid=br.brandid where dqr.MasterCompanyid=" + Session["varcompanyid"];
        //if (ddquality.SelectedIndex > 0)
        //{
        //    str = str + " And dqr.Quality_id=" + ddquality.SelectedValue;
        //}
        //if (ddshadecolor.SelectedIndex > 0)
        //{
        //    str = str + " And dqr.Shadedcolor_id=" + ddshadecolor.SelectedValue;
        //}
        str = str + "order by dateadded desc";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gv.DataSource = ds;
        gv.DataBind();
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter("@brandid", SqlDbType.Int);
            param[1] = new SqlParameter("@itemId", SqlDbType.Int);
            param[2] = new SqlParameter("@shadecolorId", SqlDbType.Int);
            param[3] = new SqlParameter("@shadecolorname", SqlDbType.NVarChar,50);
            param[4] = new SqlParameter("@User_id", SqlDbType.Int);
            param[5] = new SqlParameter("@MasterCompany_id", SqlDbType.Int);
            param[6] = new SqlParameter("@Action", SqlDbType.NVarChar, 50);
            param[7] = new SqlParameter("@msg", SqlDbType.VarChar, 50);

            param[0].Value = ddbrand.SelectedValue;
            param[1].Value = ddlitem.SelectedValue;
            param[3].Value = txtshadecolor.Text;
            param[4].Value = Session["varuserid"].ToString();
            param[5].Value = Session["varCompanyId"].ToString();
            param[6].Value = "INSERT";
            param[2].Direction = ParameterDirection.Output;
            param[7].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "PRO_MANAGE_SHADECOLOR", param);
            ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('" + param[7].Value + "');", true);
            tran.Commit();
            fillgrid();

        }
        catch (Exception ex)
        {
            tran.Rollback();
            lblmsg.Text = ex.Message;
            con.Close();

        }

    }

    protected void ddcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddlitem, "select ITEM_ID, ITEM_NAME from ITEM_MASTER where CATEGORY_ID=" + ddcategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "---Select --");
    }

    protected void ddlitem_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddbrand, "select Brandid,BrandName from Brand where item_id=" + ddlitem.SelectedValue + " and mastercompanyId=" + Session["varcompanyId"] + " order by BrandName desc", true, "--Select--");
    }

//    protected void ddquality_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        UtilityModule.ConditionalComboFill(ref ddshadecolor, @"select Distinct Sc.ShadecolorId,Sc.ShadeColorName from item_parameter_master Im inner join shadecolor sc
//                                                            on Im.shadecolor_id=Sc.ShadecolorId where im.Quality_id=" + ddquality.SelectedValue + " and im.mastercompanyId=" + Session["varcompanyId"] + "", true, "--Select--");
//        fillgrid();
//    }

    protected void ddshadecolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }
    protected void gv_RowEditing(object sender, GridViewEditEventArgs e )
    {
        gv.EditIndex = e.NewEditIndex;
        fillgrid();
    }
    protected void gv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gv.EditIndex = -1;
        fillgrid();
    }
    protected void gv_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();

        try
        {
            string QualityId, ShadecolorId, Rate, RateNew, str, ID;
            //QualityId = ((Label)gv.Rows[e.RowIndex].FindControl("lblBrandId")).Text;
            //ShadecolorId = ((Label)gv.Rows[e.RowIndex].FindControl("lblitemid")).Text;
            //Rate = ((Label)gv.Rows[e.RowIndex].FindControl("lblRate")).Text;
            RateNew = ((TextBox)gv.Rows[e.RowIndex].FindControl("txtgdshadecolorname")).Text;
            ID = gv.DataKeys[e.RowIndex].Value.ToString();

//            str = @"select Distinct ORate from PROCESSCONSUMPTIONDETAIL  PD inner join V_FinishedItemDetail Vf on
//                  PD.OFINISHEDID=vf.ITEM_FINISHED_ID and vf.QualityId=" + QualityId + " and vf.ShadecolorId=" + ShadecolorId + " and ORATE=" + Rate;
//            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
//            if (ds.Tables[0].Rows.Count > 0)
//            {
//                lblmsg.Text = "Rate used in BOM.....";

//            }
//            else
//            {
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Update ShadeColor_res set SHADECOLORNAME='" + RateNew + "' Where shadecolorId=" + ID);
                lblmsg.Text = "Rate Updated successfully....";
        //    }
            Tran.Commit();
            gv.EditIndex = -1;
            fillgrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }

    }
    protected void gv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();

        try
        {
            string QualityId, ShadecolorId, Rate, RateNew, str, ID;
            //QualityId = ((Label)gv.Rows[e.RowIndex].FindControl("lblbrandId")).Text;
            //ShadecolorId = ((Label)gv.Rows[e.RowIndex].FindControl("lblitemid")).Text;
            //Rate = ((Label)gv.Rows[e.RowIndex].FindControl("lblRate")).Text;
           
            ID = gv.DataKeys[e.RowIndex].Value.ToString();

//            str = @"select Distinct ORate from PROCESSCONSUMPTIONDETAIL  PD inner join V_FinishedItemDetail Vf on
//                  PD.OFINISHEDID=vf.ITEM_FINISHED_ID and vf.QualityId=" + QualityId + " and vf.ShadecolorId=" + ShadecolorId + " and ORATE=" + Rate;
//            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
//            if (ds.Tables[0].Rows.Count > 0)
//            {
//                lblmsg.Text = "Rate used in BOM.....";

//            }
//            else
//            {
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete from ShadeColor_res  Where shadecolorId=" + ID);
                lblmsg.Text = "Shade Color Name deleted successfully....";
          //  }
            Tran.Commit();
            fillgrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
}