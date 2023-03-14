using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_RawMaterial_FrmGateInRegister : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = "";
            str = @"Select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By Companyname";
           
            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "Select Comp Name");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewState["GateInOutRegisterID"] = 0;
            //fill_grid();    
            BindGateType();

            if (Session["varuserid"].ToString() == "1")
            {
                chkEdit.Visible = true;
            }
            else
            {
                chkEdit.Visible = false;
            }
        }
    }
    private void BindGateType()
    {
        if (ddGateType.SelectedValue == "1")
        {
            LblErrorMessage.Text = "";
            TdGPNo.Visible = false;
            TdChallanNo.Visible = true;
            TDOutTime.Visible = false;
            TDInTime.Visible = true;
            refreshForm();
            fill_grid();
            txtInTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }
        else if (ddGateType.SelectedValue == "2")
        {
            LblErrorMessage.Text = "";
            TdGPNo.Visible = true;
            TdChallanNo.Visible = false;
            TDOutTime.Visible = true;
            TDInTime.Visible = false;
            refreshForm();
            fill_grid();
            txtOutTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }
    }
    protected void ddGateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGateType();
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
            SqlParameter[] arr = new SqlParameter[21];
            arr[0] = new SqlParameter("@GateInOutRegisterID", SqlDbType.Int);
            arr[1] = new SqlParameter("@GateInOutDate", SqlDbType.DateTime);
            arr[2] = new SqlParameter("@CompanyID", SqlDbType.Int);
            arr[3] = new SqlParameter("@PartyName", SqlDbType.VarChar, 50);
            arr[4] = new SqlParameter("@Qty", SqlDbType.Float);
            arr[5] = new SqlParameter("@Unit", SqlDbType.VarChar, 10);
            arr[6] = new SqlParameter("@MaterialDescription", SqlDbType.VarChar, 250);
            arr[7] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 50);
            arr[8] = new SqlParameter("@GPNo", SqlDbType.VarChar, 50);
            arr[9] = new SqlParameter("@VehicleNo", SqlDbType.VarChar, 20);
            arr[10] = new SqlParameter("@Through", SqlDbType.VarChar, 50);
            arr[11] = new SqlParameter("@MobileNo", SqlDbType.VarChar, 15);
            arr[12] = new SqlParameter("@InTime", SqlDbType.VarChar, 15);
            arr[13] = new SqlParameter("@OutTime", SqlDbType.VarChar, 15);
            arr[14] = new SqlParameter("@Remarks", SqlDbType.VarChar, 300);
            arr[15] = new SqlParameter("@GateTypeInOut", SqlDbType.Int);
            arr[16] = new SqlParameter("@UserID", SqlDbType.Int);
            arr[17] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            arr[18] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = ViewState["GateInOutRegisterID"];
            arr[1].Value = txtdate.Text;
            arr[2].Value = ddCompName.SelectedValue;
            arr[3].Value = txtPartyName.Text;
            arr[4].Value = TxtQty.Text == "" ? "0" : TxtQty.Text;
            arr[5].Value = txtUnit.Text;
            arr[6].Value = txtMaterialDescription.Text;
            arr[7].Value = TxtChallanNo.Text;
            arr[8].Value = txtGPNo.Text;
            arr[9].Value = txtVehicleNo.Text.ToUpper();
            arr[10].Value = txtThrough.Text;
            arr[11].Value = txtMobileNo.Text;
            if (ddGateType.SelectedValue == "1")
            {
                txtInTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
                arr[12].Value = txtInTime.Text;
            }
            else
            {
                arr[12].Value = "";
            }
            if (ddGateType.SelectedValue == "2")
            {
                txtOutTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
                arr[13].Value = txtOutTime.Text;
            }
            else
            {
                arr[13].Value = "";                
            } 
            arr[14].Value = txtremarks.Text;
            arr[15].Value = ddGateType.SelectedValue;  
            arr[16].Value = Session["varuserid"];
            arr[17].Value = Session["varCompanyId"];
            arr[18].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[PRO_GateInOutRegister]", arr);
            LblErrorMessage.Text = arr[18].Value.ToString();
            LblErrorMessage.Visible = true;
            //if (arr[17].Value.ToString() != "")
            //{
            //    ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('" + arr[18].Value.ToString() + "');", true);
            //}
            //ViewState["GateInOutRegisterID"] = arr[0].Value;           
            tran.Commit();
            btnsave.Text = "Save";
            refreshForm();
            fill_grid();

            if (ddGateType.SelectedValue == "1")
            {
                txtInTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            }
            else if (ddGateType.SelectedValue == "2")
            {
                txtOutTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            }
        }
        catch (Exception ex)
        {
            tran.Rollback();
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    private void fill_grid()
    {
        DataSet ds = new DataSet();

        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@GateTypeInOut", ddGateType.SelectedValue);
        param[1] = new SqlParameter("@CompanyId", ddCompName.SelectedValue);
        param[2] = new SqlParameter("@UserID", Session["varuserid"]);
        param[3] = new SqlParameter("@MASTERCOMPANYID", Session["varCompanyId"]);
        param[4] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
        param[4].Direction = ParameterDirection.Output;

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetGateInOutRegisterDetail", param);
        if (param[4].Value.ToString() != "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('" + param[4].Value + "');", true);
            return;
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvdetail.DataSource = ds;
            gvdetail.DataBind();
        }
        else
        {
            gvdetail.DataSource = "";
            gvdetail.DataBind();
        }
    }

    private void refreshForm()
    {
        //txtPartyName.Text = "";
        TxtQty.Text = "";
        txtUnit.Text = "";
        txtMaterialDescription.Text = "";
        TxtChallanNo.Text = "";
        txtVehicleNo.Text = "";
        txtThrough.Text = "";
        txtMobileNo.Text = "";
        txtInTime.Text = "";
        txtremarks.Text = "";
        txtGPNo.Text = "";
        txtOutTime.Text = "";        
    }
    protected void gvdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvdetail, "Select$" + e.Row.RowIndex);
        }

        for (int i = 0; i < gvdetail.Columns.Count; i++)
        {
            if (ddGateType.SelectedValue == "1")
            {
                if (Session["varcompanyid"].ToString() == "44")
                {
                    if (gvdetail.Columns[i].HeaderText == "GPNo" || gvdetail.Columns[i].HeaderText == "OutTime")
                    {
                        gvdetail.Columns[i].Visible = true;
                    }

                    if (gvdetail.Columns[i].HeaderText == "ChallanNo" || gvdetail.Columns[i].HeaderText == "InTime")
                    {
                        gvdetail.Columns[i].Visible = false;
                    }


                }
                else
                {
                    if (gvdetail.Columns[i].HeaderText == "GPNo" || gvdetail.Columns[i].HeaderText == "OutTime")
                    {
                        gvdetail.Columns[i].Visible = false;
                    }

                    if (gvdetail.Columns[i].HeaderText == "ChallanNo" || gvdetail.Columns[i].HeaderText == "InTime")
                    {
                        gvdetail.Columns[i].Visible = true;
                    }
                
                
                }
                
            }
            if (ddGateType.SelectedValue == "2")
            {
                if (gvdetail.Columns[i].HeaderText == "GPNo" || gvdetail.Columns[i].HeaderText == "OutTime")
                {
                    gvdetail.Columns[i].Visible = true;
                }
               
                    if (gvdetail.Columns[i].HeaderText == "ChallanNo" || gvdetail.Columns[i].HeaderText == "InTime")
                    {
                        gvdetail.Columns[i].Visible = false;
                    }
                
            }
        }
    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[5];
            arr[0] = new SqlParameter("@GateInOutRegisterId", SqlDbType.Int);
            arr[1] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            arr[2] = new SqlParameter("@UserID", SqlDbType.Int);
            arr[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);           
           
            arr[0].Value = gvdetail.DataKeys[e.RowIndex].Value;
            arr[1].Value = Session["varCompanyId"];           
            arr[2].Value = Session["varuserid"];
            arr[3].Direction = ParameterDirection.Output;
           
            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_DeleteGateInOutRegisterData", arr);
            tran.Commit();
            LblErrorMessage.Text = arr[3].Value.ToString();
            LblErrorMessage.Visible = true;          
            fill_grid();
           
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
            tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void gvdetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }    
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        //Determine the RowIndex of the Row whose Button was clicked.
        int rowIndex = ((sender as Button).NamingContainer as GridViewRow).RowIndex;

        //Get the value of column from the DataKeys using the RowIndex.
        int id = Convert.ToInt32(gvdetail.DataKeys[rowIndex].Values[0]);

        //Reference the GridView Row.
        GridViewRow row = gvdetail.Rows[rowIndex];

        string name="";
        if (ddGateType.SelectedValue == "1")
        {
            //Fetch value of ChallanNo.
             name = (row.FindControl("lblChallanNoForReport") as Label).Text;
        }
        else if (ddGateType.SelectedValue == "2")
        {
            //Fetch value of GatePass No.
             name = (row.FindControl("lblGPNoForReport") as Label).Text;
        }

        //**************
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@GateTypeInOut", ddGateType.SelectedValue);
        param[1] = new SqlParameter("@GateInOutNo", name);
        param[2] = new SqlParameter("@mastercompanyid",Convert.ToInt32(Session["varCompanyId"]));
        param[3] = new SqlParameter("@GateInOutRegisterid", id);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetGateInOutRegisterDetailForPreview", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToInt32(Session["varCompanyId"]) == 44)
            {
                Session["rptFileName"] = "~\\Reports\\RptGateInOutMaterialRegisterDetailagni.rpt";
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\RptGateInOutMaterialRegisterDetail.rpt";
            }
            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptGateInOutMaterialDetail.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else 
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No Record Found!');", true);
        }        

    }
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {        
        if(chkEdit.Checked==true)
        {
            TDForDate.Visible = true;
            txtDateForEdit.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
        else
        {
            TDForDate.Visible = false;
            txtDateForEdit.Text = "";
            gvdetail.DataSource = "";
            gvdetail.DataBind();
        }        
    }
    protected void BtnShowData_Click(object sender, EventArgs e)
    {
        Fill_GridByDate();
    }
    private void Fill_GridByDate()
    {
        DataSet ds = new DataSet();

        SqlParameter[] param = new SqlParameter[6];
        param[0] = new SqlParameter("@GateTypeInOut", ddGateType.SelectedValue);
        param[1] = new SqlParameter("@CompanyId", ddCompName.SelectedValue);
        param[2] = new SqlParameter("@SelectedDate", txtDateForEdit.Text);
        param[3] = new SqlParameter("@UserID", Session["varuserid"]);
        param[4] = new SqlParameter("@MASTERCOMPANYID", Session["varCompanyId"]);
        param[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
        param[5].Direction = ParameterDirection.Output;

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetGateInOutRegisterDetailByDate", param);
        if (param[5].Value.ToString() != "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('" + param[5].Value + "');", true);
            return;
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvdetail.DataSource = ds;
            gvdetail.DataBind();
        }
        else
        {
            gvdetail.DataSource = "";
            gvdetail.DataBind();
        }
    }
}