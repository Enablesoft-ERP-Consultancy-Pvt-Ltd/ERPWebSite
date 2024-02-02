using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
public partial class Masters_ReportForms_FrmOrderSummaryReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.COmpanyid And CA.Userid=" + Session["varuserid"] + @"
                           select CustomerId,CustomerCode+'/'+CompanyName from Customerinfo Where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by Customercode";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDcustomer, ds, 1, true, "--Plz Select Customer--");
            ds.Dispose();
        }
    }
    protected DataSet BindGrid()
    {
        DataSet ds = new DataSet();
        lblMessage.Text = "";

        try
        {
            //SqlParameter[] param = new SqlParameter[2];
            //param[0] = new SqlParameter("@orderid", DDOrder.SelectedIndex > 0 ? DDOrder.SelectedValue : "0");
            //param[1] = new SqlParameter("@customerid", DDcustomer.SelectedIndex > 0 ? DDcustomer.SelectedValue : "0");
            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ordersummary", param);

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_ordersummary", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;
            cmd.Parameters.AddWithValue("@orderid", DDOrder.SelectedIndex > 0 ? DDOrder.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@customerid", DDcustomer.SelectedIndex > 0 ? DDcustomer.SelectedValue : "0");
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            //*************
            ds.Tables.Add(dt);

            if (ds.Tables[0].Rows.Count > 0)
            {
                TDExport.Visible = true;
            }
            else
            {
                TDExport.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }

        return ds;
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        GVDetails.DataSource = BindGrid();
        GVDetails.DataBind();
    }
    protected void DDcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDOrder, "select OM.Orderid,OM.LocalOrder+'#'+OM.customerorderno as OrderNo from orderMaster OM Where OM.CustomerId=" + DDcustomer.SelectedValue + " And OM.CompanyId=" + DDCompany.SelectedValue + " and OM.Status=0 order by Om.orderid", true, "--Plz Select Order No--");
    }
    protected void btnexport_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "ORDERSUMMARYREPORT_" + DateTime.Now + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        GridView gv = new GridView();
        DataSet ds = BindGrid();
        //*************** 
        ds.Tables[0].Columns.Remove(ds.Tables[0].Columns["orderid"]);
        ds.Tables[0].Columns.Remove(ds.Tables[0].Columns["Item_finished_id"]);
        ds.Tables[0].Columns.Remove(ds.Tables[0].Columns["unit"]);
        ds.Tables[0].Columns["customerorderNo"].ColumnName = "Buyer Order No.";

        if (Session["VarCompanyNo"].ToString() != "39")
        {
            ds.Tables[0].Columns.Remove(ds.Tables[0].Columns["BalanceQty"]);
            ds.Tables[0].Columns.Remove(ds.Tables[0].Columns["DISPATCHED"]);
        }

        if (Session["VarCompanyNo"].ToString() == "39")
        {
            ds.Tables[0].Columns["OFFLOOM"].ColumnName = "RECEIVED";
        }

        //***************
        gv.DataSource = ds;
        gv.DataBind();


        gv.GridLines = GridLines.Both;
        gv.HeaderStyle.Font.Bold = true;
        gv.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
    protected void lnkunderfinishing_Click(object sender, EventArgs e)
    {
        string fruitName = ((sender as LinkButton).NamingContainer as GridViewRow).Cells[0].Text;

        lblMessage.Text = "";
        LinkButton lnk = sender as LinkButton;

        if (lnk != null)
        {
            GridViewRow grv = lnk.NamingContainer as GridViewRow;
            Label lblorderid = (Label)GVDetails.Rows[grv.RowIndex].FindControl("lblorderid");
            Label lblitemfinishedid = (Label)GVDetails.Rows[grv.RowIndex].FindControl("lblitemfinishedid");
            Label lblorderNo = (Label)GVDetails.Rows[grv.RowIndex].FindControl("lblorderNo");
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@orderid", lblorderid.Text);
                param[1] = new SqlParameter("@item_finished_id", lblitemfinishedid.Text);

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_OrderUnderFinishing", param);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Charset = "";
                    string FileName = "ORDER_UNDERFINISHING_" + DateTime.Now + ".xls";
                    StringWriter strwritter = new StringWriter();
                    HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                    GridView gv1 = new GridView();
                    gv1.DataSource = ds;
                    gv1.DataBind();


                    gv1.GridLines = GridLines.Both;
                    gv1.HeaderStyle.Font.Bold = true;
                    gv1.RenderControl(htmltextwrtter);
                    Response.Write(strwritter.ToString());
                    Response.End();
                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = ex.Message;
            }
        }
    }
    protected void GVDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkFull = (LinkButton)e.Row.FindControl("lblUF");
            ScriptManager sm = (ScriptManager)Page.Master.FindControl("ScriptManager1");
            sm.RegisterPostBackControl(lnkFull);

            for (int i = 0; i < GVDetails.Columns.Count; i++)
            {
                if (Session["varMasterCompanyIDForERP"].ToString() == "39")
                {
                    if (GVDetails.Columns[i].HeaderText == "BALANCE" || GVDetails.Columns[i].HeaderText == "DISPATCHED")
                    {
                        GVDetails.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (GVDetails.Columns[i].HeaderText == "BALANCE" || GVDetails.Columns[i].HeaderText == "DISPATCHED")
                    {
                        GVDetails.Columns[i].Visible = false;
                    }
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (Session["VarCompanyNo"].ToString() == "39")
            {
                e.Row.Cells[12].Text = "RECEIVED";
                //GVDetails.Columns[15].HeaderText = "DISPATCHED";
            }
        }
    }
}