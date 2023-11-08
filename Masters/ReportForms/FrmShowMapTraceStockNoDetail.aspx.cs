using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
public partial class Masters_ReportForms_FrmShowMapTraceStockNoDetail : CustomPage
{

    private const string SCRIPT_DOFOCUS =
  @"window.setTimeout('DoFocus()', 1);
            function DoFocus()
            {
                try {
                    document.getElementById('REQUEST_LASTFOCUS').focus();
                } catch (ex) {}
            }";


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            HookOnFocus(this.Page as Control);

            //replaces REQUEST_LASTFOCUS in SCRIPT_DOFOCUS with the posted value from Request["__LASTFOCUS"]
            //and registers the script to start after Update panel was rendered
            ScriptManager.RegisterStartupScript(
                this,
                typeof(Masters_ReportForms_FrmShowMapTraceStockNoDetail),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);
            if (Session["varcompanyNo"].ToString() == "16" || Session["varcompanyNo"].ToString() == "28")
            {
                DGStock.Columns[11].Visible = true;
                DGStock.Columns[12].Visible = true;
                trStockRemark.Visible = true;
            }

            if (Session["VarCompanyNo"].ToString() == "42")
            {
                btnpack.Visible = true;
                trStockRemark.Visible = true;
                //BtnSaveRemark.Visible = false;
            }
            if (Session["usertype"].ToString() == "1")
            {
                trStockRemark.Visible = true;
                btnpack.Visible = true;
            }

            txtStockNo.Text = "";
            txtStockNo.Focus();
        }
    }
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarNum = 0;
            if (txtStockNo.Text != "")
            {
                //string StrNew = "";
                //string[] Str = txtStockNo.Text.Split(',');

                //foreach (string arrStr in Str)
                //{
                //    if (VarNum == 0)
                //    {
                //        StrNew = "'" + arrStr + "'";
                //        VarNum = 1;
                //    }
                //    else
                //    {
                //        StrNew = StrNew + "," + "'" + arrStr + "'";
                //    }
                //}


                SqlParameter[] _array = new SqlParameter[4];
                _array[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
                _array[1] = new SqlParameter("@MSStockNo", SqlDbType.VarChar,50);
                _array[2] = new SqlParameter("@UserId", SqlDbType.Int);
                _array[3] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);                

                _array[0].Value = Session["CurrentWorkingCompanyID"].ToString();
                _array[1].Value = txtStockNo.Text.Trim();
                _array[2].Value = Session["VarUserId"].ToString();
                _array[3].Value = Session["VarcompanyNo"].ToString();              
               

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_FillMapTraceStockNoStatusDetail", _array);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DGStock.DataSource = ds.Tables[0];
                    DGStock.DataBind();
                }
                else
                {
                    DGStock.DataSource = null;
                    DGStock.DataBind();
                }


                
                //**********Confirm
                lblmsg.Text = "";
                //btnconfirm.Visible = false;
                btnPreview.Visible = false;
                //if (Ds.Tables[0].Rows.Count > 0)
                //{
                //    btnPreview.Visible = true;
                //    if (Convert.ToInt32(Ds.Tables[0].Rows[0]["Packingid"]) != 0)
                //    {
                //        if (Session["usertype"].ToString() == "1")
                //        {
                //            btnconfirm.Visible = true;
                //        }
                //    }
                //    if (Session["varcompanyNo"].ToString() == "28" && Session["usertype"].ToString() == "1")
                //    {
                //        btnconfirm.Visible = true;
                //    }
                //}
                //////***********

                //DGStock.DataSource = Ds;
                //DGStock.DataBind();

                //VarNum = 0;
              
            }
            if (DGStock.Rows.Count == 0)
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "No Records Found or Stock No. is not available";
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmShowMapTraceStockNoDetail.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DGStock_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int VarRawDetailShowOrNot = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            ////e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DG, "select$" + e.Row.RowIndex);
            //LinkButton linkforseeDetail = e.Row.FindControl("linkforseeDetail") as LinkButton;
            //Label lblprocessId = e.Row.FindControl("lblProcessId") as Label;

            //if (lblprocessId.Text == "1")
            //{
            //    VarRawDetailShowOrNot = 1;
            //}
            //if (lblprocessId.Text != "1")
            //{
            //    linkforseeDetail.Visible = false;
            //}
            //if (lblprocessId.Text == "117" && VarRawDetailShowOrNot == 0)
            //{
            //    linkforseeDetail.Visible = true;
            //}
        }
    }
    protected void txtStockNo_TextChanged(object sender, EventArgs e)
    {
        BtnShow_Click(sender, e);
    }

    private void HookOnFocus(Control CurrentControl)
    {
        //checks if control is one of TextBox, DropDownList, ListBox or Button
        if ((CurrentControl is TextBox) ||
            (CurrentControl is DropDownList) ||
            (CurrentControl is ListBox) ||
            (CurrentControl is Button))
            //adds a script which saves active control on receiving focus in the hidden field __LASTFOCUS.
            (CurrentControl as WebControl).Attributes.Add(
                "onfocus",
                "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");

        //checks if the control has children
        if (CurrentControl.HasControls())
            //if yes do them all recursively
            foreach (Control CurrentChildControl in CurrentControl.Controls)
                HookOnFocus(CurrentChildControl);
    }
    
    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        //if (ChkForStockRawIssueDetail.Checked == true)
        //{
        //    StockWiseRawMasterialIssueDetail();
        //}
        //else
        //{
            Report();
        //}
    }
    
    protected void Report()
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "STOCKNOSTATUSREORT" + DateTime.Now + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        DGStock.GridLines = GridLines.Both;
        DGStock.HeaderStyle.Font.Bold = true;
        DGStock.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        Response.End();
    }
    //protected void BtnSaveRemark_Click(object sender, EventArgs e)
    //{
    //    lblmsg.Text = "";

    //    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //    if (con.State == ConnectionState.Closed)
    //    {
    //        con.Open();
    //    }
    //    SqlTransaction Tran = con.BeginTransaction();
    //    try
    //    {
    //        SqlParameter[] param = new SqlParameter[5];
    //        param[0] = new SqlParameter("@TstockNo", txtStockNo.Text);
    //        param[1] = new SqlParameter("@userid", Session["varuserid"]);
    //        param[2] = new SqlParameter("@mastercompanyId", Session["varcompanyNo"]);
    //        param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
    //        param[3].Direction = ParameterDirection.Output;
    //        param[4] = new SqlParameter("@Remark", TxtStockNoRemark.Text);

    //        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_SAVE_UPDATE_STOCKNOREMARK", param);
    //        lblmsg.Text = param[3].Value.ToString();
    //        Tran.Commit();
    //        txtStockNo.Text = "";
    //        TxtStockNoRemark.Text = "";
    //        txtStockNo.Focus();
    //    }
    //    catch (Exception ex)
    //    {
    //        Tran.Rollback();
    //        lblmsg.Text = ex.Message;
    //    }
    //    finally
    //    {
    //        con.Close();
    //        con.Dispose();
    //    }
    //}

    protected void btnpack_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@MSStockNo", txtStockNo.Text);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);           
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;

            //param[2] = new SqlParameter("@Remark", TxtStockNoRemark.Text.Trim());
            //**
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DIRECTMAPTRACESTOCKPACK", param);
            if (param[2].Value.ToString() != "")
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = param[2].Value.ToString();
                Tran.Rollback();
            }
            else
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Map/Trace Stock No. StockOut Successfully.";
                Tran.Commit();
            }

        }
        catch (Exception ex)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
}