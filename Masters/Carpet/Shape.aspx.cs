using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_Shape : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtid.Text = "0";
            lablechange();
            Fill_Grid();
        }
        LblError.Visible = false;
    }
    public void lablechange()
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string shape = SqlHelper.ExecuteScalar(con, CommandType.Text, "select DISTINCT ps.Parameter_name from parameter_setting ps ,master_parameter mp where ps.Parameter_Id=mp.Parameter_Id And  ps.company_id=" + Session["varMasterCompanyIDForERP"] + "  and  ps.parameter_id='4'").ToString();
            lblshapeyname.Text = shape;
        }
        catch (Exception ex)
        {
            //UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Shape.aspx");
            LblError.Visible = true;
            LblError.Text = "Data base errer..................";
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

    private void Fill_Grid()
    {
        gvShape.DataSource = Fill_Grid_Data();
        gvShape.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

        try
        {
            string strsql = "select ShapeId as Sr_No,ShapeName as '" + lblshapeyname.Text + "' from Shape where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ShapeId";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Shape.aspx");
            Logs.WriteErrorLog("Masters_Campany_Shape|Fill_Grid_Data|" + ex.Message);
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
    protected void gvShape_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvShape, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gvShape_SelectedIndexChanged(object sender, EventArgs e)
    {
         string id = gvShape.SelectedDataKey.Value.ToString();
        // Session["id"] = id;
         ViewState["id"] = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from Shape where ShapeId=" + id + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
   try
   {
    if (ds.Tables[0].Rows.Count == 1)
            
       {
           txtid.Text = ds.Tables[0].Rows[0]["ShapeId"].ToString();
           txtShape.Text = ds.Tables[0].Rows[0]["ShapeName"].ToString();
          
        
        }
    }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Shape.aspx");
            Logs.WriteErrorLog("Masters_Campany_countrymaster|cmdSave_Click|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();

            }
            if (con != null)
            {
                con.Dispose();
            }
         }
   btndelete.Visible = true;
   BtnSave.Text = "Update";

}
    private void CheckDuplicateData()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string strsql = @"Select * from Shape Where ShapeName='" + txtShape.Text + "' and ShapeId !=" + txtid.Text + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            LblError.Visible = true;
            LblError.Text = "Shape AlReady Exits........";
            txtShape.Text = "";
            txtShape.Focus();
        }
        else
        {
            LblError.Visible = false;
        }
    }
    
protected void BtnSave_Click(object sender, EventArgs e)
{
    if (txtShape.Text != "")
    {
        CheckDuplicateData();
        if (LblError.Visible == false)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[4];
                _arrPara[0] = new SqlParameter("@ShapeId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@ShapeName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@varCompanyId", SqlDbType.Int);

                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                _arrPara[1].Value = txtShape.Text.ToUpper();
                _arrPara[2].Value = Session["varuserid"].ToString();
                _arrPara[3].Value = Session["varMasterCompanyIDForERP"].ToString();

                con.Open();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_Shape", _arrPara);



                txtid.Text = "0";
                txtShape.Text = "";
                LblError.Visible = true;
                LblError.Text = "Save Details.....";

            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Shape.aspx");
                Logs.WriteErrorLog("Masters_Campany_Shape|cmdSave_Click|" + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();

                }
                if (con != null)
                {
                    con.Dispose();
                }
            }
            Fill_Grid();
            btndelete.Visible = false;
            BtnSave.Text = "Save";
        }
    }
    else
    {
        LblError.Visible = true;
        LblError.Text = "Fill Details........";
    }
    }


protected void gvShape_PageIndexChanging(object sender, GridViewPageEventArgs e)
{
    gvShape.PageIndex = e.NewPageIndex;
    Fill_Grid();
}
protected void BtnNew_Click(object sender, EventArgs e)
{
    txtid.Text = "0";
    txtShape.Text = "";
    BtnSave.Text = "Save";
}
protected void BtnClose_Click(object sender, EventArgs e)
{

}
protected void btndelete_Click(object sender, EventArgs e)
{
    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    con.Open();
    try
    {
        SqlParameter[] parparam = new SqlParameter[3];
        parparam[0] = new SqlParameter("@id", ViewState["id"].ToString());
        parparam[1] = new SqlParameter("@varCompanyId", Session["varMasterCompanyIDForERP"].ToString());
        parparam[2] = new SqlParameter("@varuserid", Session["varuserid"].ToString());

        int id = SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Proc_DeleteShape", parparam);
        if (id > 0)
        {
           LblError.Visible = true;
            LblError.Text = "Record(s) has been deleted!";
        }
        else
        {
            LblError.Visible = true;
            LblError.Text = "Value in Use...";

        }

        #region Author: Rajeev, Date: 26-Nov-12 Converted into Proc
        //int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select SHAPE_ID from ITEM_PARAMETER_MASTER where SHAPE_ID=" + ViewState["id"].ToString()));
        //if (id <= 0)
        //{
        //    SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from Shape where ShapeId=" + ViewState["id"].ToString());
        //    DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
        //    SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'Shape'," + ViewState["id"].ToString() + ",getdate(),'Delete')");
        //    LblError.Visible = true;
        //    LblError.Text = "Value Deleted...";
        //}
        //else
        //{
        //    LblError.Visible = true;
        //    LblError.Text = "Value in Use...";

        //}
        #endregion
    }
    catch (Exception ex)
    {
        UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Shape.aspx");
    }
    finally
    {
        if (con.State == ConnectionState.Open)
        {
            con.Close();
            con.Dispose();
        }
    }
    Fill_Grid();
    btndelete.Visible = false;
    BtnSave.Text = "Save";
    txtShape.Text = "";
    txtid.Text = "0";

}
}