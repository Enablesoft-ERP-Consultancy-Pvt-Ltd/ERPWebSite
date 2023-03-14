using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class Masters_Process_frmEditProductionReceive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

    }
    protected void txtfolioNo_TextChanged(object sender, EventArgs e)
    {
        DataSet ds = BindGrid();
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGReceiveDetail.DataSource = ds;
            DGReceiveDetail.DataBind();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "alert", "alert('No records found')", true);
            DGReceiveDetail.DataSource = null;
            DGReceiveDetail.DataBind();
        }
    }
    protected DataSet BindGrid()
    {
        DataSet ds = new DataSet();

        if (txtfolioNo.Text != "")
        {
            try
            {
                string str = @"select PM.ReceiveDate,CN.TStockNo,PD.IssueOrderId As FolioNo,v.ITEM_NAME,v.ColorName
                             ,isnull(PD.Wyply,0) as Wyply,isnull(PD.Cyply,0) as cyply,Weight,Width,Length,PD.Warp_10cm As Warp,PD.Weft_10cm As Weft,PD.Straightness As  Strainghtness,
                              PD.Design,PD.OBA,PD.Date_Stamp,PD.StockNoRemarks,PD.IssueOrderId,PD.Issue_Detail_Id,PD.Process_Rec_Id,PD.Process_Rec_Detail_Id,PD.Area,PM.UnitId,PM.CalType,PD.Item_Finished_Id
                              from PROCESS_RECEIVE_MASTER_1 PM,PROCESS_RECEIVE_DETAIL_1 PD,V_FinishedItemDetail V,Process_Stock_Detail PSD,CarpetNumber CN
                              Where PM.Process_Rec_Id=Pd.Process_Rec_Id  And pd.Process_Rec_Detail_Id=psd.ReceiveDetailId 
                              And psd.StockNo=cn.StockNo And pd.Item_Finished_Id=v.ITEM_FINISHED_ID  And psd.ToProcessId=1
                              And PD.IssueOrderId=" + txtfolioNo.Text;
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    DGReceiveDetail.DataSource = ds;
                //    DGReceiveDetail.DataBind();
                //}
                //else
                //{
                //    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "alert", "alert('No records found')", true);
                //    DGReceiveDetail.DataSource = null;
                //    DGReceiveDetail.DataBind();
                //}
            }
            catch (Exception ex)
            {
                lblerrMsg.Text = ex.Message;
            }
            finally
            {

            }

        }
        return ds;
    }
    protected void txtWidth_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = ((TextBox)sender).NamingContainer as GridViewRow;
        TextBox TxtLength = ((TextBox)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("txtlength"));
        TxtLength.Text = ((TextBox)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("txtlength")).Text;
        TextBox TxtWidth = ((TextBox)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("txtWidth"));
        TxtWidth.Text = ((TextBox)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("txtWidth")).Text;

        Label lblArea = ((Label)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("lblArea"));
        Label lblUnitId = ((Label)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("lblUnitId"));
        Label lblFinishedid = ((Label)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("lblItemFinishedid"));
        Label lblCalType = ((Label)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("lblCalType"));

        ////////////////
        int FootLength = 0;
        int FootWidth = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        string Str = "";
        if (TxtLength.Text != "")
        {
            if (Convert.ToInt32(lblUnitId.Text) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(TxtLength.Text));
                TxtLength.Text = Str;
                FootLength = Convert.ToInt32(Str.Split('.')[0]);
                FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootLengthInch > 11)
                {
                    lblerrMsg.Text = "Inch value must be less than 12";
                    TxtLength.Text = "";
                    TxtLength.Focus();
                }
            }
        }
        if (TxtWidth.Text != "")
        {
            if (Convert.ToInt32(lblUnitId.Text) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(TxtWidth.Text));
                TxtWidth.Text = Str;
                FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootWidthInch > 11)
                {
                    lblerrMsg.Text = "Inch value must be less than 12";
                    TxtWidth.Text = "";
                    TxtWidth.Focus();
                }
            }
        }
        if (TxtLength.Text != "" && TxtWidth.Text != "")
        {
            int Shape = 0;
            Shape = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From V_FinishedItemDetail Where Item_Finished_Id=" + lblFinishedid.Text + " And MasterCompanyId=" + Session["varCompanyId"] + ""));

            if (Convert.ToInt32(lblUnitId.Text) == 1)
            {
                lblArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(lblCalType.Text), Shape));
            }
            if (Convert.ToInt32(lblUnitId.Text) == 2)
            {
                lblArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(lblCalType.Text), Shape));
            }
        }

    }
    protected void txtLength_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = ((TextBox)sender).NamingContainer as GridViewRow;
        TextBox TxtLength = ((TextBox)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("txtlength"));
        TxtLength.Text = ((TextBox)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("txtlength")).Text;
        TextBox TxtWidth = ((TextBox)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("txtWidth"));
        TxtWidth.Text = ((TextBox)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("txtWidth")).Text;

        Label lblArea = ((Label)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("lblArea"));
        Label lblUnitId = ((Label)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("lblUnitId"));
        Label lblFinishedid = ((Label)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("lblItemFinishedid"));
        Label lblCalType = ((Label)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("lblCalType"));

        ////////////////
        int FootLength = 0;
        int FootWidth = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        string Str = "";
        if (TxtLength.Text != "")
        {
            if (Convert.ToInt32(lblUnitId.Text) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(TxtLength.Text));
                TxtLength.Text = Str;
                FootLength = Convert.ToInt32(Str.Split('.')[0]);
                FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootLengthInch > 11)
                {
                    lblerrMsg.Text = "Inch value must be less than 12";
                    TxtLength.Text = "";
                    TxtLength.Focus();
                }
            }
        }
        if (TxtWidth.Text != "")
        {
            if (Convert.ToInt32(lblUnitId.Text) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(TxtWidth.Text));
                TxtWidth.Text = Str;
                FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootWidthInch > 11)
                {
                    lblerrMsg.Text = "Inch value must be less than 12";
                    TxtWidth.Text = "";
                    TxtWidth.Focus();
                }
            }
        }
        if (TxtLength.Text != "" && TxtWidth.Text != "")
        {
            int Shape = 0;
            Shape = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From V_FinishedItemDetail Where Item_Finished_Id=" + lblFinishedid.Text + " And MasterCompanyId=" + Session["varCompanyId"] + ""));

            if (Convert.ToInt32(lblUnitId.Text) == 1)
            {
                lblArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(lblCalType.Text), Shape));
            }
            if (Convert.ToInt32(lblUnitId.Text) == 2)
            {
                lblArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(lblCalType.Text), Shape));
            }
        }

    }
    protected void DGReceiveDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblIssueOrderid = ((Label)DGReceiveDetail.Rows[e.RowIndex].FindControl("lblIssueOrderId"));
            Label lblIssueDetailId = ((Label)DGReceiveDetail.Rows[e.RowIndex].FindControl("lblIssueDetailId"));
            Label lblProcess_Rec_Id = ((Label)DGReceiveDetail.Rows[e.RowIndex].FindControl("lblProcess_Rec_Id"));
            Label lblProcess_Rec_Detail_Id = ((Label)DGReceiveDetail.Rows[e.RowIndex].FindControl("lblProcess_Rec_Detail_Id"));
            Label lblArea = ((Label)DGReceiveDetail.Rows[e.RowIndex].FindControl("lblArea"));

            TextBox txtWyply = ((TextBox)DGReceiveDetail.Rows[e.RowIndex].FindControl("txtWyPly"));
            TextBox txtCyply = ((TextBox)DGReceiveDetail.Rows[e.RowIndex].FindControl("txtCyply"));
            TextBox txtWeight = ((TextBox)DGReceiveDetail.Rows[e.RowIndex].FindControl("txtWeight"));
            TextBox txtWidth = ((TextBox)DGReceiveDetail.Rows[e.RowIndex].FindControl("txtWidth"));
            TextBox txtLength = ((TextBox)DGReceiveDetail.Rows[e.RowIndex].FindControl("txtlength"));

            TextBox txtwarp = ((TextBox)DGReceiveDetail.Rows[e.RowIndex].FindControl("txtwarp"));
            TextBox txtweft = ((TextBox)DGReceiveDetail.Rows[e.RowIndex].FindControl("txtweft"));

            TextBox txtStraightness = ((TextBox)DGReceiveDetail.Rows[e.RowIndex].FindControl("txtStraightness"));
            TextBox txtDesign = ((TextBox)DGReceiveDetail.Rows[e.RowIndex].FindControl("txtDesign"));
            TextBox txtOBA = ((TextBox)DGReceiveDetail.Rows[e.RowIndex].FindControl("txtOBA"));

            TextBox txtremarks = ((TextBox)DGReceiveDetail.Rows[e.RowIndex].FindControl("txtremarks"));

            string str = "Update Process_receive_Detail_1 set Wyply=" + txtWyply.Text + ",cyply=" + txtCyply.Text + ",Weight=" + txtWeight.Text + ",Width='" + txtWidth.Text + "',Length='" + txtLength.Text + "',Area=" + lblArea.Text + @",
                        Warp_10cm='" + txtwarp.Text + "',Weft_10cm='" + txtweft.Text + "',Straightness='" + txtStraightness.Text + "',Design='" + txtDesign.Text + "',OBA='" + txtOBA.Text + "',StockNoRemarks='" + txtremarks.Text + "' where Process_Rec_Id=" + lblProcess_Rec_Id.Text + " And Process_Rec_Detail_Id=" + lblProcess_Rec_Detail_Id.Text;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);

            Tran.Commit();
            lblerrMsg.Text = "Data Updated Successfully.......";
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblerrMsg.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void DGReceiveDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] array = new SqlParameter[9];
            array[0] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
            array[1] = new SqlParameter("@IssueDetailid", SqlDbType.Int);
            array[2] = new SqlParameter("@ProcessRecId", SqlDbType.Int);
            array[3] = new SqlParameter("@ProcessRecDetailId", SqlDbType.Int);
            array[4] = new SqlParameter("@ProcessId", SqlDbType.Int);
            array[5] = new SqlParameter("@TStockNo", SqlDbType.VarChar, 50);
            array[6] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            array[7] = new SqlParameter("@MastercompanyId", SqlDbType.Int);
            array[8] = new SqlParameter("@Userid", SqlDbType.Int);

            array[0].Value = ((Label)DGReceiveDetail.Rows[e.RowIndex].FindControl("lblIssueOrderId")).Text;
            array[1].Value = ((Label)DGReceiveDetail.Rows[e.RowIndex].FindControl("lblIssueDetailId")).Text;
            array[2].Value = ((Label)DGReceiveDetail.Rows[e.RowIndex].FindControl("lblProcess_Rec_Id")).Text;
            array[3].Value = ((Label)DGReceiveDetail.Rows[e.RowIndex].FindControl("lblProcess_Rec_Detail_Id")).Text;
            array[4].Value = 1;
            array[5].Value = ((TextBox)DGReceiveDetail.Rows[e.RowIndex].FindControl("txtStockNo")).Text;
            array[6].Direction = ParameterDirection.Output;
            array[7].Value = Session["varcompanyId"].ToString();
            array[8].Value = Session["varuserid"].ToString();
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteProductionDateForAnisa", array);

            Tran.Commit();
            lblerrMsg.Text = array[6].Value.ToString();
            txtfolioNo_TextChanged(sender, e);

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblerrMsg.Text = ex.Message;

        }
        finally
        {
        }
    }
    protected void btnexport_Click(object sender, EventArgs e)
    {
        DataSet ds = BindGrid();
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "FOLIODETAIL_" + DateTime.Now + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        GridView gv1 = new GridView();
        //IssueOrderId	Issue_Detail_Id	Process_Rec_Id	Process_Rec_Detail_Id	Area	UnitId	CalType	Item_Finished_Id

        ds.Tables[0].Columns.Remove("IssueOrderId");
        ds.Tables[0].Columns.Remove("Issue_Detail_Id");
        ds.Tables[0].Columns.Remove("Process_Rec_Id");
        ds.Tables[0].Columns.Remove("Process_Rec_Detail_Id");
        ds.Tables[0].Columns.Remove("UnitId");
        ds.Tables[0].Columns.Remove("CalType");
        ds.Tables[0].Columns.Remove("Item_Finished_Id");
        ds.AcceptChanges();

        gv1.DataSource = ds;
        gv1.DataBind();


        gv1.GridLines = GridLines.Both;
        gv1.HeaderStyle.Font.Bold = true;
        gv1.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        Response.End();
    }
}