using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ClosedXML.Excel;
public partial class Masters_ReportForms_FrmOrderSummaryReportOthers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.COmpanyid And CA.Userid=" + Session["varuserid"] + @"
                           select CustomerId,CustomerCode+'/'+CompanyName from Customerinfo Where MasterCompanyid=" + Session["varcompanyid"] + @" order by Customercode";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDcustomer, ds, 1, true, "--Plz Select Customer--");
            ds.Dispose();
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            UtilityModule.ConditonalListFill(ref lstProcess, @"SELECT PROCESS_NAME_ID,PROCESS_NAME FROM PROCESS_NAME_MASTER WHERE PROCESSTYPE=1 AND PROCESS_NAME_ID>1 AND PROCESS_NAME NOT LIKE '100%'
                                                               AND PROCESS_NAME NOT LIKE 'AQL%' AND PROCESS_NAME NOT LIKE 'NC%' ORDER BY PROCESS_NAME");

            if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
            {
                TRDDCustName.Visible = false;

                UtilityModule.ConditionalComboFill(ref DDOrder, "select OM.Orderid,OM.LocalOrder+'#'+OM.customerorderno as OrderNo from orderMaster OM Where OM.CompanyId=" + DDCompany.SelectedValue + " and OM.Status=0 order by Om.orderid", true, "--Plz Select Order No--");
            }
        }

    }
    protected DataSet BindGrid()
    {
        DataSet ds = new DataSet();
        lblMessage.Text = "";

        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@customerid", DDcustomer.SelectedValue);
            param[2] = new SqlParameter("@orderid", DDOrder.SelectedValue);
            param[3] = new SqlParameter("@shipfrom", txtfromdate.Text);
            param[4] = new SqlParameter("@shipto", txttodate.Text);
            param[5] = new SqlParameter("@MasterCompanyId", Session["VarCompanyId"]);

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_ORDERSUMMARYOTHERS", param);

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
        DataSet ds = BindGrid();
        GVDetails.DataSource = ds;
        GVDetails.DataBind();
        if (ds.Tables[0].Rows.Count == 0)
        {
            lblMessage.Text = "No records found...";
        }
    }
    protected void DDcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str ="select OM.Orderid,OM.LocalOrder+'#'+OM.customerorderno as OrderNo from orderMaster OM Where OM.CustomerId=" + DDcustomer.SelectedValue + " And OM.CompanyId=" + DDCompany.SelectedValue + " and OM.Status=0 order by Om.orderid";
        if (Convert.ToInt32(Session["varcompanyId"]) == 27)
        {
            Str = "select OM.Orderid, OM.customerorderno as OrderNo From OrderMaster OM Where OM.CustomerId=" + DDcustomer.SelectedValue + " And OM.CompanyId=" + DDCompany.SelectedValue + " and OM.Status=0 order by Om.orderid";
        }

        UtilityModule.ConditionalComboFill(ref DDOrder, Str, true, "--Plz Select Order No--");
    }
    protected void btnexport_Click(object sender, EventArgs e)
    {
        DataSet ds = BindGrid();
        if (ds.Tables[0].Rows.Count > 0)
        {

            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 0;

            //Headers
            sht.Range("A1").Value = "BUYER ORDER NO.";
            sht.Range("B1").Value = "LOCAL ORDER NO.";
            sht.Range("C1").Value = "ORDER DATE";
            sht.Range("D1").Value = "DISPATCH DATE";
            sht.Range("E1").Value = "QUALITY";
            sht.Range("F1").Value = "DESIGN";
            sht.Range("G1").Value = "COLOR";
            sht.Range("H1").Value = "SIZE";
            sht.Range("I1").Value = "TOTAL PCS";
            sht.Range("J1").Value = "TOTAL AREA";
            sht.Range("K1").Value = "TO BE ISSUED";
            sht.Range("L1").Value = "ON LOOM";
            sht.Range("M1").Value = "OFF LOOM";
            sht.Range("N1").Value = "UNDER FINISHING";
            sht.Range("O1").Value = "FINISHED";
            sht.Range("P1").Value = "PACKED";
            sht.Range("Q1").Value = "PACKED_TO_OTHER";
            sht.Range("R1").Value = "PACKED_FROM_OTHER";

            sht.Range("I1:R1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A1:R1").Style.Font.Bold = true;

            row = 2;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CUSTOMERORDERNO"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["LOCALORDER"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ORDERDATEEXCEL"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["DISPATCHDATEEXCEL"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["DESIGNNAME"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["COLORNAME"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["SIZE"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["TOTALPCS"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["TOTALAREA"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["TOBEISSUED"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["ONLOOM"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["OFFLOOM"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["UNDERFINISHING"]);
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["FINISHED"]);
                sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["PACKED"]);
                sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["PACKED_TO_OTHER"]);
                sht.Range("R" + row).SetValue(ds.Tables[0].Rows[i]["PACKED_FROM_OTHER"]);

                row = row + 1;

            }
            sht.Range("I" + row).Value = sht.Evaluate("SUM(I2:I" + (row - 1) + ")");
            sht.Range("J" + row).Value = sht.Evaluate("SUM(J2:J" + (row - 1) + ")");
            sht.Range("K" + row).Value = sht.Evaluate("SUM(K2:K" + (row - 1) + ")");
            sht.Range("L" + row).Value = sht.Evaluate("SUM(L2:L" + (row - 1) + ")");
            sht.Range("M" + row).Value = sht.Evaluate("SUM(M2:M" + (row - 1) + ")");
            sht.Range("N" + row).Value = sht.Evaluate("SUM(N2:N" + (row - 1) + ")");
            sht.Range("O" + row).Value = sht.Evaluate("SUM(O2:O" + (row - 1) + ")");
            sht.Range("P" + row).Value = sht.Evaluate("SUM(P2:P" + (row - 1) + ")");
            sht.Range("Q" + row).Value = sht.Evaluate("SUM(Q2:Q" + (row - 1) + ")");
            sht.Range("R" + row).Value = sht.Evaluate("SUM(R2:R" + (row - 1) + ")");
            sht.Range("I" + row + ":R" + row).Style.Font.Bold = true;
            sht.Columns(1, 20).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("ORDERSUMMARY (SHIPFROM : " + txtfromdate.Text + " TO :" + txttodate.Text + ")" + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
        }
        else
        {
            lblMessage.Text = "No records found...";
        }
        //************************


    }
    //public override void VerifyRenderingInServerForm(Control control)
    //{
    //    /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
    //       server control at run time. */
    //}

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

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_OrderUnderFinishingOthers", param);
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
        }
    }



    protected void btngo_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < lstProcess.Items.Count; i++)
        {
            if (lstProcess.Items[i].Selected)
            {
                //Check if process Already Exists
                if (!lstSelectProcess.Items.Contains(lstProcess.Items[i]))
                {
                    lstSelectProcess.Items.Add(new ListItem(lstProcess.Items[i].Text, lstProcess.Items[i].Value));
                }
            }
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        List<ListItem> lstselected = new List<ListItem>();

        foreach (ListItem liItems in lstSelectProcess.Items)
        {
            if (liItems.Selected == true)
            {
                lstselected.Add(liItems);
            }
        }

        //3. Loop through the List "lstSelected" and
        // remove ListItems from ListBox "lstSelectProcess" that are in 
        // lstSelected List
        foreach (ListItem liSelected in lstselected)
        {
            lstSelectProcess.Items.Remove(liSelected);
        }

    }
    protected void btngetdatajobwise_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        try
        {
            //***************JOBS
            DataTable dt = new DataTable();
            dt.Columns.Add("Processname", typeof(string));
            dt.Columns.Add("SeqNo", typeof(int));

            for (int i = 0; i < lstSelectProcess.Items.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Processname"] = lstSelectProcess.Items[i].Text;
                dr["SeqNo"] = i + 1;
                dt.Rows.Add(dr);
            }
            //*****************
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_ORDERSUMMARYJOBWISE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;
            //********
            cmd.Parameters.AddWithValue("@companyId", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@customerid", DDcustomer.SelectedIndex > 0 ? DDcustomer.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@orderid", DDOrder.SelectedIndex > 0 ? DDOrder.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@Shipfrom", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Shipto", txttodate.Text);
            cmd.Parameters.AddWithValue("@mastercompanyid", Session["varcompanyId"]);
            cmd.Parameters.AddWithValue("@dt", dt);
            DataTable dt1 = new DataTable();

            dt1.Load(cmd.ExecuteReader());
            //*************
            DataSet ds = new DataSet();
            ds.Tables.Add(dt1);

            ds.Tables[0].Columns.Remove("orderid");
            ds.Tables[0].Columns.Remove("Item_finished_id");
            ds.Tables[0].Columns.Remove("orderdateexcel");
            ds.Tables[0].Columns.Remove("Dispatchdateexcel");
            ds.Tables[0].Columns.Remove("UNIT");
            //Export to excel
            if (ds.Tables[0].Rows.Count > 0)
            {

                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;

                GridView1.DataSource = ds;
                GridView1.DataBind();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition",
                 "attachment;filename=ORDERSUMMARYJOBWISE" + DateTime.Now + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //Apply text style to each Row
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                lblMessage.Text = "Done.....";
                //*************
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altordersumm", "alert('No records found...');", true);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "";
        }
    }
    protected void DDOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select Replace(convert(nvarchar(11),OrderDate,106),' ','-') as Orderdate,Replace(convert(nvarchar(11),DispatchDate,106),' ','-') as DispatchDate From OrderMaster WHere orderid=" + DDOrder.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            switch (Session["varcompanyId"].ToString())
            {
                case "21":
                    txtfromdate.Text = ds.Tables[0].Rows[0]["orderdate"].ToString();
                    txttodate.Text = ds.Tables[0].Rows[0]["orderdate"].ToString();
                    break;
                default:
                    txtfromdate.Text = ds.Tables[0].Rows[0]["Dispatchdate"].ToString();
                    txttodate.Text = ds.Tables[0].Rows[0]["Dispatchdate"].ToString();
                    break;
            }

        }
    }
    protected void BtnBuyerDetail_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        lblMessage.Text = "";

        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@customerid", DDcustomer.SelectedValue);
            param[2] = new SqlParameter("@orderid", DDOrder.SelectedValue);
            param[3] = new SqlParameter("@shipfrom", txtfromdate.Text);
            param[4] = new SqlParameter("@shipto", txttodate.Text);
            param[5] = new SqlParameter("@MasterCompanyId", Session["VarCompanyId"]);

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_ORDERSUMMARYOTHERSNEW", param);
            if (ds.Tables[0].Rows.Count > 0)
            {

                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                string SelectionFormula = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                //Headers
                ///SelectionFormula

                sht.Range("A1").Value = "BUYERDETAIL (SHIPFROM : " + txtfromdate.Text + " TO :" + txttodate.Text + ")";
                sht.Range("A1:D1").Style.Font.Bold = true;
                sht.Range("A1:D1").Merge();

                sht.Range("A2").Value = "CUSTOMER CODE";
                sht.Range("B2").Value = "BUYER ORDER NO.";
                sht.Range("C2").Value = "ORDER DATE";
                sht.Range("D2").Value = "DISPATCH DATE";

                sht.Range("A2:D2").Style.Font.Bold = true;

                row = 3;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CUSTOMERCODE"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CUSTOMERORDERNO"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ORDERDATEEXCEL"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["DISPATCHDATEEXCEL"]);

                    row = row + 1;

                }

                sht.Columns(1, 20).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("ORDERSUMMARY (SHIPFROM : " + txtfromdate.Text + " TO :" + txttodate.Text + ")" + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();
            }
            else
            {
                lblMessage.Text = "No records found...";
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;

        }
    }
}