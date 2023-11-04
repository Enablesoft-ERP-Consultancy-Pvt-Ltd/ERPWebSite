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
//    protected void lnkbtnName_Click(object sender, EventArgs e)
//    {
//        ModalPopupExtender1.Show();
//        LinkButton lnk = sender as LinkButton;

//        if (lnk != null)
//        {
//            GridViewRow grv = lnk.NamingContainer as GridViewRow;
//            hngridrowindex.Value = grv.RowIndex.ToString();            

//            //int IssueOrderId = Convert.ToInt32(DGStock.Rows[grv.RowIndex].Cells[16].Text);
//            int IssueOrderId = Convert.ToInt32(DGStock.Rows[grv.RowIndex].Cells[3].Text);
//            int ProcessId = Convert.ToInt32(((Label)DGStock.Rows[grv.RowIndex].FindControl("lblProcessId")).Text);
//            int Item_Finished_Id = Convert.ToInt32(((Label)DGStock.Rows[grv.RowIndex].FindControl("lblFinishedid")).Text);
//            string VarStockNo = Convert.ToString(DGStock.Rows[grv.RowIndex].Cells[0].Text);

//            SqlParameter[] param = new SqlParameter[4];
//            param[0] = new SqlParameter("@processid", ProcessId);
//            param[1] = new SqlParameter("@Finishedid", Item_Finished_Id);
//            param[2] = new SqlParameter("@issueorderid", IssueOrderId);
//            param[3] = new SqlParameter("@TStockNo", VarStockNo);

//            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetRawDetailForStockNo", param);

////            string str = "";

////            str = @"select Distinct ITEM_NAME,QualityName,ShadeColorName,Lotno 
////                From ProcessRawMaster PM 
////                inner join ProcessRawTran PT on PM.PRMid=PT.PRMid
////                inner join V_FinishedItemDetail v on PT.Finishedid=v.ITEM_FINISHED_ID
////                inner join Process_issue_detail_" + ProcessId + @" PID on PM.Prorderid=PID.IssueOrderId 
////                inner join PROCESS_CONSUMPTION_DETAIL PCD on PM.Prorderid=PCD.ISSUEORDERID and PT.Finishedid=PCd.IFINISHEDID and 
////                        PCd.PROCESSID=" + ProcessId + @" And PID.Issue_Detail_Id=PCd.ISSUE_DETAIL_ID
////                Where PM.TypeFlag = 0 And PM.Processid=" + ProcessId + " and PM.Prorderid=" + IssueOrderId + " and PM.trantype=0 and PID.Item_Finished_Id=" + Item_Finished_Id;

////            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

//            GDLinkedtoCustomer.DataSource = ds;
//            GDLinkedtoCustomer.DataBind();
//        }
//    }
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
    //protected void btnconfirm_Click(object sender, EventArgs e)
    //{
    //    lblmsg.Text = "";
    //    #region
    //    //string str = "", msg = "";
    //    //DataSet ds = null;
    //    //string Tstockno = txtStockNo.Text;
    //    //string[] split = Tstockno.Split(',');
    //    //foreach (string item in split)
    //    //{
    //    //    str = @"select isnull(PackingId,0) as PackingId From carpetNumber Where Tstockno='" + item + "' and PackingId=999999999";
    //    //    ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
    //    //    if (ds.Tables[0].Rows.Count > 0)
    //    //    {
    //    //        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update carpetnumber set Pack=0,Packingid=0,PackingDetailID=0 Where Tstockno='" + item + "'");
    //    //        msg = msg + " StockNo - " + item + " confirmed sucessfully";
    //    //    }
    //    //    else
    //    //    {
    //    //        msg = msg + " StockNo -" + item + " can not confirmed";
    //    //    }
    //    //}
    //    //lblmsg.Text = msg;
    //    #endregion
    //    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //    if (con.State == ConnectionState.Closed)
    //    {
    //        con.Open();
    //    }
    //    SqlTransaction Tran = con.BeginTransaction();
    //    try
    //    {

    //        SqlParameter[] param = new SqlParameter[4];
    //        param[0] = new SqlParameter("@TstockNo", txtStockNo.Text);
    //        param[1] = new SqlParameter("@userid", Session["varuserid"]);
    //        param[2] = new SqlParameter("@mastercompanyId", Session["varcompanyNo"]);
    //        param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
    //        param[3].Direction = ParameterDirection.Output;
    //        //*******
    //        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_STOCKCONFIRMED", param);
    //        lblmsg.Text = param[3].Value.ToString();
    //        Tran.Commit();
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
    //protected void btnprintstockrawdetail_Click(object sender, EventArgs e)
    //{
    //    GetStockRawdetail();
    //    ModalPopupExtender1.Show();

    //}
    //protected void GetStockRawdetail()
    //{
        
    //    int rowindex = Convert.ToInt16(hngridrowindex.Value);
    //    int IssueOrderId = Convert.ToInt32(DGStock.Rows[rowindex].Cells[3].Text);        
    //    int ProcessId = Convert.ToInt32(((Label)DGStock.Rows[rowindex].FindControl("lblProcessId")).Text);
    //    int Item_Finished_Id = Convert.ToInt32(((Label)DGStock.Rows[rowindex].FindControl("lblFinishedid")).Text);
    //    string ReceiveDate = DGStock.Rows[rowindex].Cells[6].Text;
    //    //*****************
    //    SqlParameter[] param = new SqlParameter[4];
    //    param[0] = new SqlParameter("@processid", ProcessId);
    //    param[1] = new SqlParameter("@Finishedid", Item_Finished_Id);
    //    param[2] = new SqlParameter("@issueorderid", IssueOrderId);
    //    param[3] = new SqlParameter("@Receivedate", ReceiveDate);

    //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetStockRawdetailForOthers", param);

    //    //Export to excel
    //    GridView GridView1 = new GridView();
    //    GridView1.AllowPaging = false;

    //    GridView1.DataSource = ds;
    //    GridView1.DataBind();

    //    Response.Clear();
    //    Response.Buffer = true;
    //    Response.AddHeader("content-disposition",
    //     "attachment;filename=STOCKRAWDETAIL" + DateTime.Now + ".xls");
    //    Response.Charset = "";
    //    Response.ContentType = "application/vnd.ms-excel";
    //    StringWriter sw = new StringWriter();
    //    HtmlTextWriter hw = new HtmlTextWriter(sw);

    //    for (int i = 0; i < GridView1.Rows.Count; i++)
    //    {
    //        //Apply text style to each Row
    //        GridView1.Rows[i].Attributes.Add("class", "textmode");
    //    }
    //    GridView1.RenderControl(hw);

    //    //style to format numbers to string
    //    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
    //    Response.Write(style);
    //    Response.Output.Write(sw.ToString());
    //    Response.Flush();
    //    Response.End();

    //}
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
    //protected void StockWiseRawMasterialIssueDetail()
    //{
    //    SqlParameter[] param = new SqlParameter[4];
    //    param[0] = new SqlParameter("@processid", 1);
    //    param[1] = new SqlParameter("@Prmid", 0);
    //    param[2] = new SqlParameter("@TStockNo", txtStockNo.Text);
    //    param[3] = new SqlParameter("@Type", 1);
    //    //************
    //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_STOCKNOWISEMATERIALISSUE", param);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {

    //        Session["rptFileName"] = "~\\Reports\\Rptstocknowisematerialissue.rpt";
    //        Session["GetDataset"] = ds;
    //        Session["dsFileName"] = "~\\ReportSchema\\Rptstocknowisematerialissue.xsd";

    //        StringBuilder stb = new StringBuilder();
    //        stb.Append("<script>");
    //        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
    //        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
    //    }
    //}
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