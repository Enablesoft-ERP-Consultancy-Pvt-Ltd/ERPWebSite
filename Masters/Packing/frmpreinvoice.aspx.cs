using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Office2010.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using ClosedXML.Excel;
using System.Configuration;

public partial class Masters_Packing_frmpreinvoice : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CarriageId,CarriageName From Carriage where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by CarriageName
                           Select GoodsReceiptId, StationName from GoodsReceipt Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by StationName
                           Select TransModeid,TransModeName from Transmode Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by TransModename
                           Select GoodsReceiptId, StationName from GoodsReceipt Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by StationName
                           select Unitid,UnitName From unit Where unitid in(1,2) and MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                           Select CurrencyId,CurrencyName from CurrencyInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                           Select PaymentId, PaymentName from Payment Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by PaymentName
                           select Distinct CI.CompanyId,CI.Companyname From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by Companyname
                           Select [Year], [Session] From [Session] Order By Year Desc ";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDPreCarriage, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDReceiptAt, ds, 1, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDByAirSea, ds, 2, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDPortLoad, ds, 3, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDunit, ds, 4, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDCurrency, ds, 5, false, "");
            if (DDCurrency.Items.Count > 0)
            {
                DDCurrency.SelectedIndex = 0;
                DDCurrency_SelectedIndexChanged(sender, new EventArgs());
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDDelivery, ds, 6, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 7, false, "");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDSession, ds, 8, false, "");
            //Dates
            txtinvoicedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtdispatchdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            if (variable.VarGSTForInvoiceFormNew == "1")
            {
                TRGST.Visible = true;
                TRManualGST.Visible = false;
            }
        }
    }
    protected void DDCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDcif, "select CurrencyTypeRs,CurrencyTypeRs From CurrencyInfo where currencyid=" + DDCurrency.SelectedValue + "", false, "");
    }
    protected void txtdestcode_TextChanged(object sender, EventArgs e)
    {
        string str = @"select Buyername,Consignee_Address,payingagent_address,PortofDisc,Country
                From Destinationmaster Where Destcode='" + txtdestcode.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {

            txtbuyer.Text = ds.Tables[0].Rows[0]["buyername"].ToString();
            txtconsignee.Text = ds.Tables[0].Rows[0]["Consignee_Address"].ToString();
            txtconsigneeaddress.Text = ds.Tables[0].Rows[0]["payingagent_address"].ToString();
            txtportofdischarge.Text = ds.Tables[0].Rows[0]["PortofDisc"].ToString();
            txtcountryoffinaldest.Text = ds.Tables[0].Rows[0]["Country"].ToString();
        }
        else
        {
            txtbuyer.Text = "";
            txtconsignee.Text = "";
            txtconsigneeaddress.Text = "";
            txtportofdischarge.Text = "";
            txtcountryoffinaldest.Text = "";
        }
    }
    protected void txtecisno_TextChanged(object sender, EventArgs e)
    {
        string str = @"SELECT DISTINCT I.Tinvoiceno,PI.GSTType FROM INVOICE  I(NoLock)  INNER JOIN PACKINGINFORMATION  PI(NoLock) ON I.PACKINGID=PI.PACKINGID 
            WHERE PI.ECISNO='" + txtecisno.Text + "' And I.consignorId = " + DDcompany.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (variable.VarGSTForInvoiceFormNew == "1")
            {
                DDGSType.SelectedValue = ds.Tables[0].Rows[0]["GSTType"].ToString();
            }
            txtinvoiceno.Text = ds.Tables[0].Rows[0]["TinvoiceNo"].ToString();
            txtinvoiceno_TextChanged(sender, new EventArgs());
        }
        else
        {
            Refreshcontrol(Ecisnoblank: false);
            hninvid.Value = "0";
            DG.DataSource = null;
            DG.DataBind();
            FillgridaccordingtoEcisno();
        }

    }
    protected void FillgridaccordingtoEcisno()
    {
        //SqlParameter[] param = new SqlParameter[3];
        //param[0] = new SqlParameter("@ecisno", txtecisno.Text);
        //param[1] = new SqlParameter("@invoicedate", txtinvoicedate.Text);
        //param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
        //param[2].Direction = ParameterDirection.Output;

        ////        string str = @"select 0 as Rollno,CN.stockno,CN.TStockNo,vf.QualityName,vf.QualityName+' '+vf.designName as QualityDesign,vf.designName,vf.ColorName,vf.SizeMtr as Size,vf.widthmtr,vf.lengthmtr,Vf.AreaMtr as Area,
        ////                    isnull(dbo.F_Getarticlerate(PA.articleno,'" + txtinvoicedate.Text + @"'),0) as Rate,DD.Pono,Replace(convert(nvarchar(11),DD.Podate,106),' ','-') as Podate,DD.articleno,PA.weight_roll,PA.Netwt,PA.volume_roll,PA.pcs_roll,CN.Item_Finished_Id
        ////                    From DispatchPlanmaster DM inner join DispatchPlanDetail DD on DM.ID=DD.Masterid
        ////                    inner join DispatchPlanStockNo DS on DD.DetailId=DS.Plandetailid
        ////                    inner join CarpetNumber CN on DS.stockno=CN.StockNo
        ////                    inner join V_FinishedItemDetail vf on Cn.Item_Finished_Id=vf.ITEM_FINISHED_ID
        ////                    inner join Packingarticle PA on DD.articleno=PA.ArticleNo
        ////                    Where DD.ECISNo='" + txtecisno.Text + "' and DS.invoicegenerated=0";
        ////        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getecisnopackingdata", param);

        DataSet ds = new DataSet();

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_getecisnopackingdata", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 1000;

        cmd.Parameters.AddWithValue("@ecisno", txtecisno.Text);
        cmd.Parameters.AddWithValue("@invoicedate", txtinvoicedate.Text);
        cmd.Parameters.Add("@msg", SqlDbType.VarChar,200);
        cmd.Parameters["@msg"].Direction = ParameterDirection.Output;

        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);

        //SqlParameter[] param = new SqlParameter[1];
        //param[0] = new SqlParameter("@issueorderid", DDFolioNo.SelectedValue);

        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_WEAVERFOLIO_MATERIAL_DETAIL_HAFIZIA_REPORT", param);



        if (cmd.Parameters["@msg"].Value.ToString() != "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "checkecisno", "alert('" + cmd.Parameters["@msg"].Value.ToString() + "')", true);
            return;
        }
        //fill        
        if (ds.Tables[1].Rows.Count > 0)
        {
            txtgrwt.Text = ds.Tables[1].Compute("sum(grosswt)", "").ToString();
            txtnetwt.Text = ds.Tables[1].Compute("sum(netwt)", "").ToString();
            txtvol.Text = ds.Tables[1].Compute("sum(volume)", "").ToString();
            txtrolls.Text = ds.Tables[1].Compute("sum(rolls)", "").ToString();
            txtponodate.Text = ds.Tables[1].Rows[0]["PONo_Date"].ToString();
            txtdescription.Text = ds.Tables[1].Rows[0]["descofgoods"].ToString();
            txtotherref.Text = ds.Tables[1].Rows[0]["Otherref"].ToString();
        }
        else
        {
            txtgrwt.Text = "";
            txtnetwt.Text = "";
            txtvol.Text = "";
            txtrolls.Text = "";
            txtponodate.Text = "";
            txtdescription.Text = "";
            txtotherref.Text = "";
        }
        Fillgrid(ds);
    }
    protected void Fillgrid(DataSet ds)
    {
        DG.DataSource = ds.Tables[0];
        DG.DataBind();

        if (ds.Tables[0].Rows.Count > 0)
        {
            //txtpcs.Text = ds.Tables[0].Compute("count(tstockno)", "").ToString();
            //txttotalarea.Text = Math.Round(Convert.ToDouble(ds.Tables[0].Compute("sum(area)", "")), 2).ToString();
            //txtinvamt.Text = Math.Round(Convert.ToDouble(ds.Tables[0].Compute("sum(Rate)", "")), 2).ToString();
            txtpcs.Text = ds.Tables[0].Rows[0]["PCS"].ToString();
            txttotalarea.Text = ds.Tables[0].Rows[0]["TotalArea"].ToString();
            txtinvamt.Text = ds.Tables[0].Rows[0]["Amount"].ToString();
        }
        else
        {
            txtpcs.Text = "";
            txttotalarea.Text = "";
            txtinvamt.Text = "";
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        //*******table type
        DataTable dt = new DataTable();
        dt.Columns.Add("RollNo", typeof(int));
        dt.Columns.Add("Articleno", typeof(string));
        dt.Columns.Add("finishedid", typeof(int));
        dt.Columns.Add("Width", typeof(string));
        dt.Columns.Add("Length", typeof(string));
        dt.Columns.Add("Area", typeof(float));
        dt.Columns.Add("Price", typeof(float));
        dt.Columns.Add("stockno", typeof(int));
        dt.Columns.Add("tstockno", typeof(string));
        dt.Columns.Add("Quality", typeof(string));
        dt.Columns.Add("Design", typeof(string));
        dt.Columns.Add("color", typeof(string));
        dt.Columns.Add("Pono", typeof(string));
        dt.Columns.Add("Podate", typeof(DateTime));
        dt.Columns.Add("RateDate", typeof(DateTime));
        //*******
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            GridViewRow gvr = DG.Rows[i];
            Label lblrollno = (Label)gvr.FindControl("lblrollno");
            Label lblarticleno = (Label)gvr.FindControl("lblarticleno");
            Label lblitemfinishedid = (Label)gvr.FindControl("lblitemfinishedid");
            Label lblwidth = (Label)gvr.FindControl("lblwidth");
            Label lbllength = (Label)gvr.FindControl("lbllength");
            Label lblarea = (Label)gvr.FindControl("lblarea");
            TextBox txtrate = (TextBox)gvr.FindControl("txtrate");
            Label lblstockno = (Label)gvr.FindControl("lblstockno");
            Label lbltstockno = (Label)gvr.FindControl("lbltstockno");
            Label lblquality = (Label)gvr.FindControl("lblquality");
            Label lbldesign = (Label)gvr.FindControl("lbldesign");
            Label lblcolour = (Label)gvr.FindControl("lblcolour");
            Label lblpono = (Label)gvr.FindControl("lblpono");
            Label lblpodate = (Label)gvr.FindControl("lblpodate");
            Label lblratedate = (Label)gvr.FindControl("lblratedate");
            //*****
            DataRow dr = dt.NewRow();
            dr["RollNo"] = lblrollno.Text;
            dr["Articleno"] = lblarticleno.Text;
            dr["finishedid"] = lblitemfinishedid.Text;
            dr["Width"] = lblwidth.Text;
            dr["Length"] = lbllength.Text;
            dr["Area"] = lblarea.Text;
            dr["Price"] = txtrate.Text;
            dr["stockno"] = lblstockno.Text;
            dr["tstockno"] = lbltstockno.Text;
            dr["Quality"] = lblquality.Text;
            dr["Design"] = lbldesign.Text;
            dr["color"] = lblcolour.Text;
            dr["pono"] = lblpono.Text;
            dr["podate"] = lblpodate.Text == "" ? DBNull.Value : (object)lblpodate.Text;
            dr["Ratedate"] = lblratedate.Text == "" ? DBNull.Value : (object)lblratedate.Text;
            dt.Rows.Add(dr);
        }
        //*****
        if (dt.Rows.Count > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("Proc_saveInvoice_Packingplan", con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30000;

                cmd.Parameters.Add("@InvoiceId", SqlDbType.Int);
                cmd.Parameters["@Invoiceid"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@invoiceid"].Value = hninvid.Value;
                cmd.Parameters.AddWithValue("@Invoiceno", txtinvoiceno.Text);
                cmd.Parameters.AddWithValue("@invoicedate", txtinvoicedate.Text);
                cmd.Parameters.AddWithValue("@Destcode", txtdestcode.Text);
                cmd.Parameters.AddWithValue("@Ecisno", txtecisno.Text);
                cmd.Parameters.AddWithValue("@Dtstamp", txtdtstamp.Text);
                cmd.Parameters.AddWithValue("@Delvwk", txtDelvwk.Text);
                cmd.Parameters.AddWithValue("@Buyer", txtbuyer.Text);
                cmd.Parameters.AddWithValue("@consignee_address", txtconsignee.Text);
                cmd.Parameters.AddWithValue("@Payingagent_address", txtconsigneeaddress.Text);
                cmd.Parameters.AddWithValue("@Precarriageid", DDPreCarriage.SelectedValue);
                cmd.Parameters.AddWithValue("@placeofreceipt", DDReceiptAt.SelectedValue);
                cmd.Parameters.AddWithValue("@vessel_flightno", DDByAirSea.SelectedValue);
                cmd.Parameters.AddWithValue("@Portofdischarge", txtportofdischarge.Text);
                cmd.Parameters.AddWithValue("@finaldestination", txtfinaldestination.Text);
                cmd.Parameters.AddWithValue("@unitid", DDunit.SelectedValue);
                cmd.Parameters.AddWithValue("@currencyid", DDCurrency.SelectedValue);
                cmd.Parameters.AddWithValue("@CIF", DDcif.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@descriptionofgoods", txtdescription.Text);
                cmd.Parameters.AddWithValue("@Pono_Date", txtponodate.Text);
                cmd.Parameters.AddWithValue("@Exporterref", txtexporterref.Text);
                cmd.Parameters.AddWithValue("@Otherref", txtotherref.Text);
                cmd.Parameters.AddWithValue("@countryoforigin", txtcountryoforigin.Text);
                cmd.Parameters.AddWithValue("@countryoffinaldest", txtcountryoffinaldest.Text);
                cmd.Parameters.AddWithValue("@termsofdeliveryid", DDDelivery.SelectedValue);
                cmd.Parameters.AddWithValue("@sealNo", txtsealno.Text);
                cmd.Parameters.AddWithValue("@BOEDate", txtboedate.Text == "" ? DBNull.Value : (object)txtboedate.Text);
                cmd.Parameters.AddWithValue("@Shipbillno", txtshipbillno.Text);
                cmd.Parameters.AddWithValue("@Blno", txtBlno.Text);
                cmd.Parameters.AddWithValue("@shipbilldate", txtshipbilldate.Text == "" ? DBNull.Value : (object)txtshipbilldate.Text);
                cmd.Parameters.AddWithValue("@Bldate", txtblDate.Text == "" ? DBNull.Value : (object)txtblDate.Text);
                cmd.Parameters.AddWithValue("@shipmentId", txtshipmentid.Text);
                cmd.Parameters.AddWithValue("@truckNo", txttruckno.Text);
                cmd.Parameters.AddWithValue("@Dispatchdate", txtdispatchdate.Text);
                cmd.Parameters.AddWithValue("@grosswt", txtgrwt.Text == "" ? "0" : txtgrwt.Text);
                cmd.Parameters.AddWithValue("@Netwt", txtnetwt.Text == "" ? "0" : txtnetwt.Text);
                cmd.Parameters.AddWithValue("@volume", txtvol.Text == "" ? "0" : txtvol.Text);
                cmd.Parameters.AddWithValue("@Totalpcs", txtpcs.Text == "" ? "0" : txtpcs.Text);
                cmd.Parameters.AddWithValue("@totalrolls", txtrolls.Text == "" ? "0" : txtrolls.Text);
                cmd.Parameters.AddWithValue("@Totalarea", txttotalarea.Text == "" ? "0" : txttotalarea.Text);
                cmd.Parameters.AddWithValue("@Insurance", txtinsurance.Text == "" ? "0" : txtinsurance.Text);
                cmd.Parameters.AddWithValue("@freight", txtfreight.Text == "" ? "0" : txtfreight.Text);
                cmd.Parameters.AddWithValue("@grossamt", txtgramount.Text == "" ? "0" : txtgramount.Text);
                cmd.Parameters.AddWithValue("@invamount", txtinvamt.Text == "" ? "0" : txtinvamt.Text);
                cmd.Parameters.AddWithValue("@contents", txtcontents.Text);
                cmd.Parameters.AddWithValue("@dtrecord", dt);
                cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
                cmd.Parameters.AddWithValue("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
                cmd.Parameters.AddWithValue("@PortLoad", DDPortLoad.SelectedValue);
                cmd.Parameters.AddWithValue("@Companyid", DDcompany.SelectedValue);
                cmd.Parameters.AddWithValue("@CGST", txtcgst.Text == "" ? "0" : txtcgst.Text);
                cmd.Parameters.AddWithValue("@SGST", txtsgst.Text == "" ? "0" : txtsgst.Text);
                cmd.Parameters.AddWithValue("@IGST", txtIgst.Text == "" ? "0" : txtIgst.Text);
                cmd.Parameters.AddWithValue("@GSTType", TRGST.Visible == true ? DDGSType.SelectedValue : "0");

                #region
                // SqlParameter[] param = new SqlParameter[54];
                //param[0] = new SqlParameter("@InvoiceId", SqlDbType.Int);
                //param[0].Direction = ParameterDirection.InputOutput;
                //param[0].Value = hninvid.Value;
                //param[1] = new SqlParameter("@Invoiceno", txtinvoiceno.Text);
                //param[2] = new SqlParameter("@invoicedate", txtinvoicedate.Text);
                //param[3] = new SqlParameter("@Destcode", txtdestcode.Text);
                //param[4] = new SqlParameter("@Ecisno", txtecisno.Text);
                //param[5] = new SqlParameter("@Dtstamp", txtdtstamp.Text);
                //param[6] = new SqlParameter("@Delvwk", txtDelvwk.Text);
                //param[7] = new SqlParameter("@Buyer", txtbuyer.Text);
                //param[8] = new SqlParameter("@consignee_address", txtconsignee.Text);
                //param[9] = new SqlParameter("@Payingagent_address", txtconsigneeaddress.Text);
                //param[10] = new SqlParameter("@Precarriageid", DDPreCarriage.SelectedValue);
                //param[11] = new SqlParameter("@placeofreceipt", DDReceiptAt.SelectedValue);
                //param[12] = new SqlParameter("@vessel_flightno", DDByAirSea.SelectedValue);
                //param[13] = new SqlParameter("@Portofdischarge", txtportofdischarge.Text);
                //param[14] = new SqlParameter("@finaldestination", txtfinaldestination.Text);
                //param[15] = new SqlParameter("@unitid", DDunit.SelectedValue);
                //param[16] = new SqlParameter("@currencyid", DDCurrency.SelectedValue);
                //param[17] = new SqlParameter("@CIF", DDcif.SelectedItem.Text);
                //param[18] = new SqlParameter("@descriptionofgoods", txtdescription.Text);
                //param[19] = new SqlParameter("@Pono_Date", txtponodate.Text);
                //param[20] = new SqlParameter("@Exporterref", txtexporterref.Text);
                //param[21] = new SqlParameter("@Otherref", txtotherref.Text);
                //param[22] = new SqlParameter("@countryoforigin", txtcountryoforigin.Text);
                //param[23] = new SqlParameter("@countryoffinaldest", txtcountryoffinaldest.Text);
                //param[24] = new SqlParameter("@termsofdeliveryid", DDDelivery.SelectedValue);
                //param[25] = new SqlParameter("@sealNo", txtsealno.Text);
                //param[26] = new SqlParameter("@BOEDate", txtboedate.Text == "" ? DBNull.Value : (object)txtboedate.Text);
                //param[27] = new SqlParameter("@Shipbillno", txtshipbillno.Text);
                //param[28] = new SqlParameter("@Blno", txtBlno.Text);
                //param[29] = new SqlParameter("@shipbilldate", txtshipbilldate.Text == "" ? DBNull.Value : (object)txtshipbilldate.Text);
                //param[30] = new SqlParameter("@Bldate", txtblDate.Text == "" ? DBNull.Value : (object)txtblDate.Text);
                //param[31] = new SqlParameter("@shipmentId", txtshipmentid.Text);
                //param[32] = new SqlParameter("@truckNo", txttruckno.Text);
                //param[33] = new SqlParameter("@Dispatchdate", txtdispatchdate.Text);
                //param[34] = new SqlParameter("@grosswt", txtgrwt.Text == "" ? "0" : txtgrwt.Text);
                //param[35] = new SqlParameter("@Netwt", txtnetwt.Text == "" ? "0" : txtnetwt.Text);
                //param[36] = new SqlParameter("@volume", txtvol.Text == "" ? "0" : txtvol.Text);
                //param[37] = new SqlParameter("@Totalpcs", txtpcs.Text == "" ? "0" : txtpcs.Text);
                //param[38] = new SqlParameter("@totalrolls", txtrolls.Text == "" ? "0" : txtrolls.Text);
                //param[39] = new SqlParameter("@Totalarea", txttotalarea.Text == "" ? "0" : txttotalarea.Text);
                //param[40] = new SqlParameter("@Insurance", txtinsurance.Text == "" ? "0" : txtinsurance.Text);
                //param[41] = new SqlParameter("@freight", txtfreight.Text == "" ? "0" : txtfreight.Text);
                //param[42] = new SqlParameter("@grossamt", txtgramount.Text == "" ? "0" : txtgramount.Text);
                //param[43] = new SqlParameter("@invamount", txtinvamt.Text == "" ? "0" : txtinvamt.Text);
                //param[44] = new SqlParameter("@contents", txtcontents.Text);
                //param[45] = new SqlParameter("@dtrecord", dt);
                //param[46] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                //param[46].Direction = ParameterDirection.Output;
                //param[47] = new SqlParameter("@userid", Session["varuserid"]);
                //param[48] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
                //param[49] = new SqlParameter("@PortLoad", DDPortLoad.SelectedValue);
                //param[50] = new SqlParameter("@Companyid", DDcompany.SelectedValue);
                //param[51] = new SqlParameter("@CGST", txtcgst.Text == "" ? "0" : txtcgst.Text);
                //param[52] = new SqlParameter("@SGST", txtsgst.Text == "" ? "0" : txtsgst.Text);
                //param[53] = new SqlParameter("@IGST", txtIgst.Text == "" ? "0" : txtIgst.Text);
                //******
                //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Proc_saveInvoice_Packingplan", param);
                #endregion
                //******
                cmd.ExecuteNonQuery();
                Tran.Commit();
                lblmsg.Text = cmd.Parameters["@msg"].Value.ToString();
                hninvid.Value = cmd.Parameters["@invoiceid"].Value.ToString();
            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "al", "alert('No rocords available in Data grid to Save Data.')", true);
        }
    }

    protected void txtinvoiceno_TextChanged(object sender, EventArgs e)
    {
        string str = @"select Top 100 I.Invoiceid,I.Destcode,I.Dtstamp,DM.Buyername,I.Delvwk,DM.Consignee_Address,DM.payingagent_address,
                        I.PreCarrier,I.Receipt,I.ShipingId,I.PortLoad,I.PortUnload,I.DestinationAdd,PM.UnitId,PM.CurrencyId,PM.CIF,I.descriptionofgoods,
                        Replace(CONVERT(nvarchar(11),I.InvoiceDate,106),' ','-') as InvoiceDate,I.TorderNo,I.ERef,I.OtherRef,
                        I.countryoforigin,I.countryoffinaldest,I.DelTerms,I.SealNo,REPlace(convert(nvarchar(11),I.BOEDate,106),' ','-') as Boedate,
                        I.sbillno,Replace(CONVERT(nvarchar(11),i.sbilldate,106),' ','-') as Sbilldate,I.BLNo,Replace(CONVERT(nvarchar(11),i.BlDt,106),' ','-') as BLdate,
                        I.shipmentid,I.truckno,Replace(CONVERT(nvarchar(11),i.Dispatchdate,106),' ','-') as Dispatchdate,I.contents,I.GrossWt,I.NetWt,I.volume,I.NoOfRolls,
                        PD.TStockNo,PD.Quality+' '+Pd.Design as QualityDesign,PD.articleno,PD.Color as colorname,PD.Width+'x'+PD.Length as Size,
                        PD.Area,PD.Price as Rate,PD.Pono,PD.Podate,PD.FinishedId as Item_finished_id,PD.RollNo,PD.Width as Widthmtr,PD.Length as Lengthmtr,
                        PD.StockNo,PD.Design as Designname,PD.Quality as Qualityname,PD.Ratedate,isnull(I.Cgst,0) as Cgst,isnull(I.Sgst,0) as Sgst,isnull(I.Igst,0) as Igst, 
                        PI.PCS, PI.Area TotalArea, PI.Amount 
                        from Invoice I(NoLock) 
                        join PACKING PM(NoLock) on I.PackingId=PM.PackingId
                        join Destinationmaster DM(NoLock) on  I.Destcode=DM.Destcode 
                        join PackingInformation PD(NoLock) on PM.PackingId=PD.PackingId                         
						JOIN (Select PackingID, Sum(Pcs) PCS, Round(Sum(Area), 2) Area, Round(Sum(Price), 2) Amount 
							From PackingInformation(Nolock) Group By PackingID) PI ON PI.PackingID = I.InvoiceId 
                        Where I.consignorId = " + DDcompany.SelectedValue + " And I.Tinvoiceno='" + txtinvoiceno.Text + "'";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            hninvid.Value = ds.Tables[0].Rows[0]["Invoiceid"].ToString();
            btndelete.Visible = true;
            txtdestcode.Text = ds.Tables[0].Rows[0]["destcode"].ToString();
            txtdtstamp.Text = ds.Tables[0].Rows[0]["Dtstamp"].ToString();
            txtbuyer.Text = ds.Tables[0].Rows[0]["Buyername"].ToString();
            txtDelvwk.Text = ds.Tables[0].Rows[0]["Delvwk"].ToString();
            txtconsignee.Text = ds.Tables[0].Rows[0]["consignee_address"].ToString();
            txtconsigneeaddress.Text = ds.Tables[0].Rows[0]["Payingagent_address"].ToString();
            DDPreCarriage.SelectedValue = ds.Tables[0].Rows[0]["Precarrier"].ToString();
            DDReceiptAt.SelectedValue = ds.Tables[0].Rows[0]["Receipt"].ToString();
            DDByAirSea.SelectedValue = ds.Tables[0].Rows[0]["shipingid"].ToString();
            DDPortLoad.SelectedValue = ds.Tables[0].Rows[0]["Portload"].ToString();
            txtportofdischarge.Text = ds.Tables[0].Rows[0]["Portunload"].ToString();
            txtfinaldestination.Text = ds.Tables[0].Rows[0]["Destinationadd"].ToString();
            DDunit.SelectedValue = ds.Tables[0].Rows[0]["unitid"].ToString();
            DDCurrency.SelectedValue = ds.Tables[0].Rows[0]["currencyid"].ToString();
            DDCurrency_SelectedIndexChanged(sender, new EventArgs());
            if (DDcif.Items.FindByText(ds.Tables[0].Rows[0]["Cif"].ToString()) != null)
            {
                DDcif.SelectedValue = ds.Tables[0].Rows[0]["Cif"].ToString();
            }
            txtdescription.Text = ds.Tables[0].Rows[0]["descriptionofgoods"].ToString();
            txtinvoicedate.Text = ds.Tables[0].Rows[0]["invoicedate"].ToString();
            txtponodate.Text = ds.Tables[0].Rows[0]["Torderno"].ToString();
            txtexporterref.Text = ds.Tables[0].Rows[0]["Eref"].ToString();
            txtotherref.Text = ds.Tables[0].Rows[0]["Otherref"].ToString();
            txtcountryoforigin.Text = ds.Tables[0].Rows[0]["countryoforigin"].ToString();
            txtcountryoffinaldest.Text = ds.Tables[0].Rows[0]["countryoffinaldest"].ToString();
            DDDelivery.SelectedValue = ds.Tables[0].Rows[0]["Delterms"].ToString();
            txtsealno.Text = ds.Tables[0].Rows[0]["sealno"].ToString();
            txtboedate.Text = ds.Tables[0].Rows[0]["Boedate"].ToString();
            txtBlno.Text = ds.Tables[0].Rows[0]["Blno"].ToString();
            txtshipbillno.Text = ds.Tables[0].Rows[0]["sbillno"].ToString();
            txtshipbilldate.Text = ds.Tables[0].Rows[0]["Sbilldate"].ToString();
            txtblDate.Text = ds.Tables[0].Rows[0]["bldate"].ToString();
            txtshipmentid.Text = ds.Tables[0].Rows[0]["shipmentid"].ToString();
            txttruckno.Text = ds.Tables[0].Rows[0]["truckno"].ToString();
            txtdispatchdate.Text = ds.Tables[0].Rows[0]["Dispatchdate"].ToString();
            txtcontents.Text = ds.Tables[0].Rows[0]["contents"].ToString();
            txtgrwt.Text = ds.Tables[0].Rows[0]["grosswt"].ToString();
            txtnetwt.Text = ds.Tables[0].Rows[0]["Netwt"].ToString();
            txtvol.Text = ds.Tables[0].Rows[0]["volume"].ToString();
            txtpcs.Text = "";
            txtrolls.Text = ds.Tables[0].Rows[0]["Noofrolls"].ToString();
            txttotalarea.Text = "";
            txtinvamt.Text = "";
            txtcgst.Text = ds.Tables[0].Rows[0]["Cgst"].ToString();
            txtsgst.Text = ds.Tables[0].Rows[0]["Sgst"].ToString();
            txtIgst.Text = ds.Tables[0].Rows[0]["Igst"].ToString();
            if (variable.VarGSTForInvoiceFormNew == "1")
            {
                btnUpdateGSTType.Visible = true;
            }

            //*************************Fill Grid detail
            //            str = @"select PD.TStockNo,PD.Quality+' '+Pd.Design as QualityDesign,PD.articleno,PD.Color as colorname,PD.Width+'x'+PD.Length as Size,
            //                    PD.Area,PD.Price as Rate,PD.Pono,PD.Podate,PD.FinishedId as Item_finished_id,PD.RollNo,PD.Width as Widthmtr,PD.Length as Lengthmtr,
            //                    PD.StockNo,PD.Design as Designname,PD.Quality as Qualityname
            //                    From Packing PM inner join PackingInformation PD on PM.PackingId=PD.PackingId
            //                    and PM.PackingId=" + hninvid.Value;
            //            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            Fillgrid(ds);
            //
        }
        else
        {
            hninvid.Value = "0";
            if (Session["varMasterCompanyIDForERP"].ToString() == "21")
            {
                //DG.DataSource = null;
                //DG.DataBind();
                // Refreshcontrol();
            }
            else
            {
                DG.DataSource = null;
                DG.DataBind();
                Refreshcontrol();

            }
            btndelete.Visible = false;

            if (variable.VarGSTForInvoiceFormNew == "1")
            {
                btnUpdateGSTType.Visible = false;
            }
        }
    }
    protected void Refreshcontrol(Boolean Ecisnoblank = true)
    {
        //txtdestcode.Text = "";
        if (Ecisnoblank == true)
        {
            txtecisno.Text = "";
        }
        txtdtstamp.Text = "";
        //txtbuyer.Text = "";
        txtDelvwk.Text = "";
        //txtconsignee.Text = "";
        //txtconsigneeaddress.Text = "";



        DDPreCarriage.SelectedIndex = -1;
        DDReceiptAt.SelectedIndex = -1;
        DDByAirSea.SelectedIndex = -1;
        DDPortLoad.SelectedIndex = -1;
        // txtportofdischarge.Text = "";
        txtfinaldestination.Text = "";
        DDunit.SelectedIndex = -1;
        DDCurrency.SelectedIndex = -1;
        DDcif.SelectedIndex = -1;
        txtdescription.Text = "";
        txtinvoicedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        txtponodate.Text = "";
        txtexporterref.Text = "";
        txtotherref.Text = "";
        //txtcountryoforigin.Text = "";
        //txtcountryoffinaldest.Text = "";

        DDDelivery.SelectedIndex = -1;
        txtsealno.Text = "";
        txtboedate.Text = "";
        txtBlno.Text = "";
        txtshipbillno.Text = "";
        txtshipbilldate.Text = "";
        txtblDate.Text = "";
        txtshipmentid.Text = "";
        txttruckno.Text = "";
        txtdispatchdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        txtcontents.Text = "";
        txtgrwt.Text = "";
        txtnetwt.Text = "";
        txtvol.Text = "";
        txtpcs.Text = "";
        txtrolls.Text = "";
        txttotalarea.Text = "";
        txtinvamt.Text = "";
        txtcgst.Text = "";
        txtsgst.Text = "";
        txtIgst.Text = "";

    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@invoiceid", hninvid.Value);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            //***************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DeleteInvoice_New", param);
            if (param[2].Value.ToString() != "")
            {
                lblmsg.Text = param[2].Value.ToString();
                Tran.Rollback();
            }
            else
            {
                lblmsg.Text = "Invoice Deleted Successfully.";
                hninvid.Value = "0";
                Tran.Commit();
            }
            txtinvoiceno_TextChanged(sender, new EventArgs());
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void txtrate_Changed(object sender, EventArgs e)
    {
        int Rowindex = ((sender as TextBox).NamingContainer as GridViewRow).RowIndex;
        TextBox txtrate = (TextBox)DG.Rows[Rowindex].FindControl("txtrate");
        Label lblamount = (Label)DG.Rows[Rowindex].FindControl("lblamount");
        lblamount.Text = txtrate.Text;
        //**********Get Total amount
        txtinvamt.Text = GetTotalamount().ToString();
        //**********
    }
    protected double GetTotalamount()
    {
        double amt = 0;
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            Label lblamount = (Label)DG.Rows[i].FindControl("lblamount");
            amt = amt + Convert.ToDouble(lblamount.Text == "" ? "0" : lblamount.Text);
        }
        return amt;
    }
    protected void DDGSType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnUpdateGSTType_Click(object sender, EventArgs e)
    {

        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@invoiceid", hninvid.Value);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@GSTType", TRGST.Visible == true ? DDGSType.SelectedValue : "0");


            //***************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UpdateGSTType_New", param);
            if (param[2].Value.ToString() != "")
            {
                lblmsg.Text = param[2].Value.ToString();
                Tran.Rollback();
            }
            else
            {
                lblmsg.Text = "GST Type Updated Successfully.";
                hninvid.Value = "0";
                Tran.Commit();
            }
            txtinvoiceno_TextChanged(sender, new EventArgs());
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DDcompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select  lutarnno From CompanyInfo where  CompanyId='" + DDcompany.SelectedValue + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            chcklutarnno.Text = ds.Tables[0].Rows[0]["lutarnno"].ToString();
        }
    }
    protected void DDSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtinvoiceno_AutoCompleteExtender.ContextKey = DDSession.SelectedValue;
    }
    protected void btnimport_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        //********************************
        if (fileupload.HasFile)
        {
            //***********check File type
            if (Path.GetExtension(fileupload.FileName) != ".xlsx" && Path.GetExtension(fileupload.FileName) != ".xls")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "al4", "alert('Please select valid .xls or .xlsx file')", true);
                return;
            }
            //***********
            DataTable dt = new DataTable();
            dt.Columns.Add("Order", typeof(string));
            dt.Columns.Add("Art_No", typeof(string));
            dt.Columns.Add("IKEA_Desc", typeof(string));
            dt.Columns.Add("Price", typeof(float));
            dt.Columns.Add("Cur", typeof(string));
            dt.Columns.Add("Dest_Code_Rcv", typeof(string));
            dt.Columns.Add("SL", typeof(int));
            dt.Columns.Add("Csm_No", typeof(string));
            dt.Columns.Add("Disp_Date", typeof(DateTime));
            dt.Columns.Add("Shp_Id", typeof(string));
            dt.Columns.Add("Bkd_Qty", typeof(int));
            dt.Columns.Add("Csm_UL", typeof(int));
            dt.Columns.Add("Csm_val", typeof(float));
            dt.Columns.Add("CsmGroVol", typeof(decimal));
            dt.Columns.Add("CsmGroWei", typeof(decimal));
            dt.Columns.Add("CsmNetWei", typeof(decimal));
            dt.Columns.Add("pkgno", typeof(string));
            dt.Columns.Add("hsn", typeof(string));
            dt.Columns.Add("sealno", typeof(string));
            dt.Columns.Add("truckno", typeof(string));
          //  dt.Columns.Add("dispatchdate", typeof(string));
            dt.Columns.Add("invoiceno", typeof(string));
            dt.Columns.Add("invoicedate", typeof(DateTime));


            if (!Directory.Exists(Server.MapPath("~/PreInvoice/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/PreInvoice/"));
            }
            fileupload.SaveAs(Server.MapPath("~/PreInvoice/" + fileupload.FileName.ToString()));
            string filename = Server.MapPath("~/PreInvoice/" + fileupload.FileName.ToString());
            using (var document = SpreadsheetDocument.Open(filename, true))
            {
                try
                {
                    WorkbookPart wbPart = document.WorkbookPart;
                    Sheet sheet = (Sheet)wbPart.Workbook.Sheets.FirstChild;
                    WorksheetPart wsp = (WorksheetPart)wbPart.GetPartById(sheet.Id);
                    IEnumerable<Row> row = sheet.Elements<Row>();
                    for (int rNo = 2; rNo <10 ; rNo++)
                    {
                        //if (wsp.Readcell("A" + rNo).Trim() == "")
                        //{
                        //    break;
                        //}
                        ////Get quality,Design,color size from article creation

                        //string articleno = "";
                        //if (Session["varMasterCompanyIDForERP"].ToString() == "14")
                        //{
                        //    double articleno2 = Convert.ToDouble(wsp.Readcell("C" + rNo).Trim());  //articleno
                        //    articleno = Convert.ToString(articleno2);
                        //}
                        //else
                        //{
                        //    articleno = wsp.Readcell("C" + rNo).Trim();//articleno
                        //}

                        //DataTable dtitemdesc = UtilityModule.getarticledescription(articleno);
                        ////                        
                        //if (dtitemdesc.Rows.Count == 0)
                        //{
                        //    lblmsg.Text = "Article No does not exists For Row Number " + rNo + ".'";
                        //    //ScriptManager.RegisterStartupScript(Page, GetType(), "al", "alert('Article No does not exists For Row Number " + rNo + ".')", true);
                        //    dt.Rows.Clear();
                        //    break;
                        //}
                        ////****Rate Date
                        //string rateDate = wsp.Readcell("M" + rNo).Trim();
                        //if (rateDate == "")
                        //{
                        //    //lblmsg.Text = "Article No does not exists For Row Number " + rNo + ".'";
                        //    lblmsg.Text = "Rate Date can not be blank For Row Number " + rNo + ".'";
                        //    dt.Rows.Clear();
                        //    break;
                        //}
                        //**************
                      
                        DataRow dr = dt.NewRow();
                        if (!string.IsNullOrEmpty(wsp.Readcell("A" + rNo).ToString()))
                        {
                            dr["Order"] = wsp.Readcell("A" + rNo).Trim();
                            dr["Art_No"] = wsp.Readcell("B" + rNo).Trim();
                            dr["IKEA_Desc"] = wsp.Readcell("C" + rNo).Trim();
                            dr["Price"] = wsp.Readcell("D" + rNo).Trim();
                            dr["Cur"] = wsp.Readcell("E" + DDCurrency.SelectedItem.Text).Trim(); ;
                            dr["Dest_Code_Rcv"] = wsp.Readcell("F" + rNo).Trim();
                            dr["SL"] = wsp.Readcell("G" + rNo).Trim();
                            dr["Csm_No"] = wsp.Readcell("H" + rNo).Trim();
                            dr["Disp_Date"] = DateTime.FromOADate(Convert.ToDouble(wsp.Readcell("I" + rNo))).ToString("dd-MMM-yyyy");
                            dr["Shp_Id"] = wsp.Readcell("J" + rNo).Trim();
                            dr["Bkd_Qty"] = Regex.Match(wsp.Readcell("K" + rNo).Trim(), @"\d+").Value;
                            dr["Csm_UL"] = Regex.Match(wsp.Readcell("L" + rNo).Trim(), @"\d+").Value;
                            dr["Csm_val"] = Regex.Match(wsp.Readcell("M" + rNo).Trim(), @"\d+").Value;
                            dr["CsmGroVol"] = wsp.Readcell("N" + rNo).Trim();
                            dr["CsmGroWei"] = wsp.Readcell("O" + rNo).Trim();
                            dr["CsmNetWei"] = wsp.Readcell("P" + rNo).Trim();
                            dr["pkgno"] = wsp.Readcell("Q" + rNo).Trim();
                            dr["hsn"] = wsp.Readcell("R" + rNo).Trim();
                            dr["sealno"] = wsp.Readcell("S" + rNo).Trim();
                            dr["truckno"] = wsp.Readcell("T" + rNo).Trim();
                            //dr["dispatchdate"] = DateTime.FromOADate(Convert.ToDouble(wsp.Readcell("U" + rNo))).ToString("dd-MMM-yyyy");
                            dr["invoiceno"] = wsp.Readcell("V" + rNo).Trim();
                            dr["invoicedate"] = DateTime.FromOADate(Convert.ToDouble(wsp.Readcell("W" + rNo))).ToString("dd-MMM-yyyy");
                            dt.Rows.Add(dr);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                      //  FillGrid(dt);

                        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        SqlTransaction Tran = con.BeginTransaction();
                        try
                        {

                            SqlParameter[] param = new SqlParameter[15];
                            param[0] = new SqlParameter("@userid", Session["varuserid"]);
                            param[1] = new SqlParameter("@mastercomanyId", Session["varMasterCompanyIDForERP"]);
                            param[2] = new SqlParameter("@dtdetail", dt);
                            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                            param[3].Direction = ParameterDirection.Output;
                            param[4] = new SqlParameter("@Precarriageby", DDPreCarriage.SelectedItem.Text);
                            param[5] = new SqlParameter("@placeofreceipt", DDReceiptAt.SelectedItem.Text);
                            param[6] = new SqlParameter("@vessel_flightno", DDByAirSea.SelectedItem.Text);
                            param[7] = new SqlParameter("@Portofdischarge", txtportofdischarge.Text);
                            param[8] = new SqlParameter("@Portofloading",DDPortLoad.SelectedItem.Text);
                            param[9] = new SqlParameter("@finaldestination", txtfinaldestination.Text);
                            param[10] = new SqlParameter("@countryfinaldestination", txtcountryoffinaldest.Text);
                            param[11] = new SqlParameter("@countryoforigin", txtcountryoforigin.Text);
                            param[12] = new SqlParameter("@DLVWEEK", txtDelvwk.Text);
                            param[13] = new SqlParameter("@CURR", DDCurrency.SelectedItem.Text);
                            param[14] = new SqlParameter("@IGST", txtIgst.Text==""?"0":txtIgst.Text);
                          
                            
 

 
                            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_savepreinvoice", param);
                            lblerrormsg.Text = param[3].Value.ToString();
                           // hnplanid.Value = param[0].Value.ToString();
                            Tran.Commit();
                            //FillGridDetail();
                        }
                        catch (Exception ex)
                        {
                            Tran.Rollback();
                            lblmsg.Text = ex.Message;

                        }
                        finally
                        {
                            con.Close();
                            con.Dispose();
                        }


                    }
                    else
                    {
                        if (lblmsg.Text == "")
                        {
                            lblmsg.Text = "Excel sheet has no data to import.";
                        }
                        DG.DataSource = null;
                        DG.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    lblmsg.Text = ex.Message;
                }
                finally
                {
                    document.Close();
                    document.Dispose();
                    //File.Delete(filename);
                }
            }
        }







    }
    protected void btnprintinv_Click(object sender, EventArgs e)
    {
          if (!Directory.Exists(Server.MapPath("~/InvoiceExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/InvoiceExcel/"));
        }
        
        try
        {
            string Path = "";
            string Pathpdf = "";
            string str = "";

            //str = @"select ROW_NUMBER() over(order by(select 1)) srno,pi.*,dm.*,pa.*,ci.LUTARNNo,ci.LUTIssueDate from preinvoice pi join Destinationmaster dm  on left(pi.desccode, CHARINDEX('-', REVERSE('-' + pi.desccode)))=dm.Destcode left join Packingarticle pa on pi.articleno=pa.ArticleNo left join CompanyInfo ci on ci.MasterCompanyid=pi.MasterCompanyi where pi.invoiceno='" + txtinvoiceno.Text + "' order by dm.Id";

            str = @"select ROW_NUMBER() over(order by(select 1)) srno,pi.*,dm.*,pa.*,ci.LUTARNNo,ci.LUTIssueDate,c.CurrentRateRefRs from preinvoice pi join Destinationmaster dm  on  pi.desccode=dm.Destcode left join Packingarticle pa on pi.articleno=pa.ArticleNo left join CompanyInfo ci on ci.MasterCompanyid=pi.MasterCompanyid left join CurrencyInfo c on c.CurrencyName=pi.curr  where pi.invoiceno='" + txtinvoiceno.Text + "' order by dm.Id";


            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            var xapp = new XLWorkbook();
            string invoicename=string.Empty;
            if (ds != null)
            {
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["igst"]) > 0)
                {
                    invoicename = "TAX INVOICE";
                }
                else { invoicename = "INVOICE"; }
                //if (ds.Tables[0].Rows[0]["curr"].ToString().ToUpper() == "USD")
                //{
                //    invoicename = "Tax Invoice";
                //}
                //else
                //{ invoicename = "Invoice"; }
                    
                var sht = xapp.Worksheets.Add(invoicename);
                int sum = ds.Tables[0].AsEnumerable().Sum(a => a.Field<int>("csmul"));
                var orders = ds.Tables[0].AsEnumerable().Select(a => a.Field<string>("orderid"));
                string buyerordernos = string.Join(",", orders);

                //************set cell width
                //Page
                //  sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                sht.PageSetup.AdjustTo(95);
                sht.PageSetup.FitToPages(1, 1);
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                sht.PageSetup.VerticalDpi = 300;
                sht.PageSetup.HorizontalDpi = 300;
                sht.PageSetup.Margins.Top = 0.2;
                sht.PageSetup.Margins.Bottom = 0.1;
                sht.PageSetup.Margins.Right = 0.1;
                sht.PageSetup.Margins.Left = 0.1;
                sht.Column("N").Width = 12.89;
                sht.Column("B").Width = 13.89;
                sht.Column("O").Width = 10.89;
                sht.Column("K").Width = 10.89;
                sht.Column("P").Width = 10.89;
                sht.Column("Q").Width = 10.89;
                sht.Row(7).Height = 20.80;
                sht.Row(8).Height = 20.80;
                sht.Style.Font.FontName = "Arial";



                //*****Header   
                //if (ds.Tables[0].Rows[0]["curr"].ToString().ToUpper() == "USD")
                //{
                sht.Cell("B1").Value = invoicename;
                  
                //}
                //else { sht.Cell("B1").Value = "INVOICE"; }
                sht.Range("B1:Q1").Style.Font.FontName = "Arial";
                sht.Range("B1:Q1").Style.Font.FontSize = 16;
                sht.Range("B1:Q1").Style.Font.Bold = true;
                sht.Range("B1:Q1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B1:Q1").Merge();
                if (chklut.Checked)
                {
                    sht.Cell("B2").Value = "EXPORT MEANT FOR EXPORT WITHOUT PAYMENT OF IGST AGAINST LUT No. " + Convert.ToString(ds.Tables[0].Rows[0]["LUTARNNo"]) + "";
                    sht.Range("B2:M2").Style.Font.FontName = "Arial";
                    sht.Range("B2:M2").Style.Font.FontSize = 12;
                    sht.Range("B2:M2").Style.Font.Bold = true;
                    //  sht.Range("B2:M2").Style.Fill.BackgroundColor = XLColor.Yellow;
                    sht.Range("B2:M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("B2:M2").Merge();

                    sht.Cell("N2").Value = "Date -" + Convert.ToDateTime(ds.Tables[0].Rows[0]["LUTIssueDate"]).ToString("dd/MMM/yyyy");
                    sht.Range("N2:Q2").Style.Font.FontName = "Arial";
                    sht.Range("N2:Q2").Style.Font.FontSize = 12;
                    sht.Range("N2:Q2").Style.Font.Bold = true;
                    // sht.Range("N2:Q2").Style.Fill.BackgroundColor = XLColor.Yellow;
                    sht.Range("N2:Q2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("N2:Q2").Merge();
                    sht.Range("N2:Q2").Style.NumberFormat.Format = "dd/MMM/yyyy";



                }
                sht.Range("B3").Value = "EXPORTER";
                sht.Range("B3:D3").Style.Font.FontName = "Arial";
                sht.Range("B3:D3").Style.Font.FontSize = 12;
                //  sht.Range("A3:C3").Style.Alignment.WrapText = true;
                sht.Range("B3:D3").Merge();
                //*****************

                //CompanyName
                sht.Range("B4").Value = "Diamond Exports";
                sht.Range("B4:E4").Style.Font.FontName = "Arial";
                sht.Range("B4:E4").Style.Font.FontSize = 12;
                // sht.Range("B4:E4").Style.Font.Bold = true;
                //  sht.Range("A3:C4").Style.Alignment.WrapText = true;
                sht.Range("B4:E4").Merge();
                //Address

                sht.Range("B5").Value = "DIAMOND PLEX , BAMRAULI KATARA FATEHABAD ROAD AGRA (UTTAR PRADESH) - 282006 INDIA";
                sht.Range("B5:F6").Style.Font.FontName = "Arial";
                sht.Range("B5:F6").Style.Font.FontSize = 11;
                sht.Range("B5:F6").Style.Alignment.WrapText = true;
                sht.Range("B5:F6").Merge();
                //address2
                sht.Range("B7").Value = "STATE OF ORIGIN & CODE : UTTAR PRADESH & 09 DISTRICT & CODE : AGRA & 118 GSTIN : 09AALFD7697M2ZD";
                sht.Range("B7:F8").Style.Font.FontName = "Arial";
                sht.Range("B7:F8").Style.Font.FontSize = 11;
                sht.Range("B7:F8").Style.Alignment.WrapText = true;
                sht.Range("B7:F8").Merge();

                sht.Range("I3").Value = "Invoice No.";
                sht.Range("I3:J3").Style.Font.FontName = "Arial";
                sht.Range("I3:J3").Style.Font.FontSize = 12;
                sht.Range("I3:J3").Merge();

                sht.Range("K3").Value = Convert.ToString(ds.Tables[0].Rows[0]["invoiceno"]);
                sht.Range("K3:L3").Style.Font.FontName = "Arial";
                sht.Range("K3:L3").Style.Font.FontSize = 12;
                sht.Range("K3:L3").Merge();

                sht.Range("I4").Value = "Invoice Date";
                sht.Range("I4:J4").Style.Font.FontName = "Arial";
                sht.Range("I4:J4").Style.Font.FontSize = 12;
                //  sht.Range("I4:J4").Style.Font.Bold = true;
                sht.Range("I4:J4").Merge();
                sht.Range("K4").Value = Convert.ToString(ds.Tables[0].Rows[0]["invoicedate"]);
                sht.Range("K4:L4").Style.Font.FontName = "Arial";
                sht.Range("K4:L4").Style.Font.FontSize = 12;
                sht.Range("K4:L4").Style.NumberFormat.Format = "dd/MMM/yyyy";
                sht.Range("K4:L4").Merge();
                sht.Range("M3").Value = "Office of the Commissioner of Customs(Prev.)";
                sht.Range("M3:Q3").Style.Font.FontName = "Arial";
                sht.Range("M3:Q3").Style.Font.FontSize = 12;
                sht.Range("M3:Q3").Merge();
                sht.Range("M4").Value = "Reg No/IEC-0610004182  YEAR-2010";
                sht.Range("M4:Q4").Style.Font.FontName = "Arial";
                sht.Range("M4:Q4").Style.Font.FontSize = 12;
                sht.Range("M4:Q4").Merge();
                sht.Range("I5").Value = "BUYER'S ORDER NO.";
                sht.Range("I5:K5").Style.Font.FontName = "Arial";
                sht.Range("I5:K5").Style.Font.FontSize = 12;
                sht.Range("I5:K5").Merge();
                sht.Range("L5").Value = "'" + buyerordernos;
                sht.Range("L5:Q8").Style.Font.FontName = "Arial";
                sht.Range("L5:Q8").Style.Font.FontSize = 12;
                sht.Range("L5:Q8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("L5:Q8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("L5:Q8").Merge();
                sht.Range("I6:K8").Merge();
              //  sht.Range("L6:Q8").Merge();.

                sht.Range("I4:Q4").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("H3:Q3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("H4:Q4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("B1:B31").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("Q1:Q31").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("K5:K13").Style.Border.RightBorder = XLBorderStyleValues.Thin;

                sht.Range("I3:I27").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("D19:D27").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("M14:I18").Style.Border.RightBorder = XLBorderStyleValues.Thin;

                sht.Range("B2:Q2").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("B1:Q1").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("B8:Q8").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("B27:Q27").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("B29:Q29").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("B31:Q31").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("B13:Q13").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("B18:Q18").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("B21:H21").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("I21:Q21").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("B24:H24").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("M14:M21").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("L28:L31").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("N28:N31").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("P28:P31").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("B9").Value = "CONSIGNEE";
                sht.Range("B9:C9").Style.Font.FontName = "Arial";
                sht.Range("B9:C9").Style.Font.FontSize = 12;
                sht.Range("B9:C9").Merge();
                sht.Range("B9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("B10").Value = Convert.ToString(ds.Tables[0].Rows[0]["Consignee_Address"]);
                sht.Range("B10:F13").Style.Font.FontName = "Arial";
                sht.Range("B10:F13").Style.Font.FontSize = 12;
                sht.Range("B10:F13").Merge();
                sht.Range("B10:F13").Style.Alignment.WrapText = true;
                sht.Range("G9").Value = Convert.ToString(ds.Tables[0].Rows[0]["Consignee_dt"]);
                sht.Range("G9:H9").Style.Font.FontName = "Arial";
                sht.Range("G9:H9").Style.Font.FontSize = 12;
                sht.Range("G9:H9").Merge();
                sht.Range("G9:H9").Style.Alignment.WrapText = true;
                sht.Range("G9:H9").Style.Font.Bold = true;
                sht.Range("G9:H9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("I9").Value = "OTHER REFERENCE(S)";
                sht.Range("I9:K9").Style.Font.FontName = "Arial";
                sht.Range("I9:K9").Style.Font.FontSize = 12;
                sht.Range("I9:L9").Merge();
                sht.Range("I11").Value = "CONSIGNMENT NO";
                sht.Range("I11:K11").Style.Font.FontName = "Arial";
                sht.Range("I11:K11").Style.Font.FontSize = 12;
                //sht.Range("I11:K11").Style.Font.Bold = true;
                sht.Range("I11:K11").Merge();
                sht.Range("I13").Value = "SUPPLIER NO";
                sht.Range("I13:K13").Style.Font.FontName = "Arial";
                sht.Range("I13:K13").Style.Font.FontSize = 12;
                //sht.Range("I13:K13").Style.Font.Bold = true;
                sht.Range("I13:K13").Merge();

                sht.Range("L9").Value = "";
                sht.Range("L9:Q9").Style.Font.FontName = "Arial";
                sht.Range("L9:Q9").Style.Font.FontSize = 12;
                sht.Range("L9:Q9").Merge();
                sht.Range("L11").Value = Convert.ToString(ds.Tables[0].Rows[0]["csmno"]);
                sht.Range("L11:Q11").Style.Font.FontName = "Arial";
                sht.Range("L11:Q11").Style.Font.FontSize = 12;
                sht.Range("L11:Q11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("L11:Q11").Merge();
                sht.Range("L11:Q11").Style.Font.Bold = true;
                sht.Range("L13").Value = "23130";
                sht.Range("L13:Q13").Style.Font.FontName = "Arial";
                sht.Range("L13:Q13").Style.Font.FontSize = 12;
                sht.Range("L13:Q13").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("L13:Q13").Merge();
                sht.Range("L13:Q13").Style.Font.Bold = true;

                sht.Range("B14").Value = "RECEIVER";
                sht.Range("B14:C14").Style.Font.FontName = "Arial";
                sht.Range("B14:C14").Style.Font.FontSize = 12;
                sht.Range("B14:C14").Merge();
                sht.Range("B14").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("B15").Value = Convert.ToString(ds.Tables[0].Rows[0]["Receiver_address"]);
                sht.Range("B15:F18").Style.Font.FontName = "Arial";
                sht.Range("B15:F18").Style.Font.FontSize = 12;
                sht.Range("B15:F18").Merge();
                sht.Range("B15:F18").Style.Alignment.WrapText = true;
                sht.Range("G14").Value = Convert.ToString(ds.Tables[0].Rows[0]["Consignee_dt"]);
                sht.Range("G14:H14").Style.Font.FontName = "Arial";
                sht.Range("G14:H14").Style.Font.FontSize = 12;
                sht.Range("G14:H14").Merge();
                sht.Range("G14:H14").Style.Alignment.WrapText = true;
                sht.Range("G14:H14").Style.Font.Bold = true;
                sht.Range("G14:H14").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("I14").Value = "1. PAYING AGENT";
                sht.Range("I14:M14").Style.Font.FontName = "Arial";
                sht.Range("I14:M14").Style.Font.FontSize = 12;
                sht.Range("I14:M14").Merge();
                sht.Range("I14").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("I15").Value = Convert.ToString(ds.Tables[0].Rows[0]["payingagent_address"]);
                sht.Range("I15:M18").Style.Font.FontName = "Arial";
                sht.Range("I15:M18").Style.Alignment.WrapText = true;
                sht.Range("I15:M18").Style.Font.FontSize = 11;
                sht.Range("I15:M18").Merge();
                sht.Range("I15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("N14").Value = "2. BUYER ( if other than consignee)";
                sht.Range("N14:Q14").Style.Font.FontName = "Arial";
                sht.Range("N14:Q14").Style.Font.FontSize = 12;
                sht.Range("N14:Q14").Merge();
                sht.Range("N15").Value = Convert.ToString(ds.Tables[0].Rows[0]["otherthanconsignee_address"]);
                sht.Range("N15:Q18").Style.Font.FontName = "Arial";
                sht.Range("N15:Q18").Style.Font.FontSize = 11;
                sht.Range("N15:Q18").Style.Alignment.WrapText = true;
                sht.Range("N15:Q18").Merge();
                sht.Range("N15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("B19").Value = "PRE-CARRIAGE BY";
                sht.Range("B19:D19").Style.Font.FontName = "Arial";
                sht.Range("B19:D19").Style.Font.FontSize = 12;
                sht.Range("B19:D19").Merge();
                sht.Range("B19:D19").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B20").Value = Convert.ToString(ds.Tables[0].Rows[0]["precarriageby"]);
                sht.Range("B20:D21").Style.Font.FontName = "Arial";
                sht.Range("B20:D21").Style.Font.FontSize = 12;
                sht.Range("B20:D21").Merge();
                sht.Range("B20:D21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B20:D21").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("E19").Value = "PLACE OF RECEIPT BY PRE-CARRIER";
                sht.Range("E19:H19").Style.Font.FontName = "Arial";
                sht.Range("E19:H19").Style.Font.FontSize = 11;
                sht.Range("E19:H19").Merge();
                sht.Range("E19:H19").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E20").Value = Convert.ToString(ds.Tables[0].Rows[0]["placeofreceipt"]);
                sht.Range("E20:H21").Style.Font.FontName = "Arial";
                sht.Range("E20:H21").Style.Font.FontSize = 12;
                sht.Range("E20:H21").Merge();
                sht.Range("E20:H21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E20:H21").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("I19").Value = "COUNTRY OF ORIGIN OF GOODS";
                sht.Range("I19:M19").Style.Font.FontName = "Arial";
                sht.Range("I19:M19").Style.Font.FontSize = 12;
                sht.Range("I19:M19").Merge();
                sht.Range("I19:M19").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("I20").Value = Convert.ToString(ds.Tables[0].Rows[0]["countryoforigin"]);
                sht.Range("I20:M21").Style.Font.FontName = "Arial";
                sht.Range("I20:M21").Style.Font.FontSize = 12;
                sht.Range("I20:M21").Merge();
                sht.Range("I20:M21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("I20:M21").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("N19").Value = "COUNTRY OF FINAL DESTINATION";
                sht.Range("N19:Q19").Style.Font.FontName = "Arial";
                sht.Range("N19:Q19").Style.Font.FontSize = 11;
                sht.Range("N19:Q19").Merge();
                sht.Range("N19:Q19").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("N20").Value = Convert.ToString(ds.Tables[0].Rows[0]["COUNTRY"]);
                sht.Range("N20:Q21").Style.Font.FontName = "Arial";
                sht.Range("N20:Q21").Style.Font.FontSize = 12;
                sht.Range("N20:Q21").Merge();
                sht.Range("N20:Q21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("N20:Q21").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);





                sht.Range("B22").Value = "VESSEL/FLIGHT NO.";
                sht.Range("B22:D22").Style.Font.FontName = "Arial";
                sht.Range("B22:D22").Style.Font.FontSize = 12;
                sht.Range("B22:D22").Merge();
                sht.Range("B22:D22").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B23").Value = "";    //Convert.ToString(ds.Tables[0].Rows[0]["vesselno"]); 
                sht.Range("B23:D24").Style.Font.FontName = "Arial";
                sht.Range("B23:D24").Style.Font.FontSize = 12;
                sht.Range("B23:D24").Merge();
                sht.Range("B23:D24").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B23:D24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("E22").Value = "PORT OF LOADING";
                sht.Range("E22:H22").Style.Font.FontName = "Arial";
                sht.Range("E22:H22").Style.Font.FontSize = 11;
                sht.Range("E22:H22").Merge();
                sht.Range("E22:H22").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E23").Value = Convert.ToString(ds.Tables[0].Rows[0]["portofloading"]);
                sht.Range("E23:H24").Style.Font.FontName = "Arial";
                sht.Range("E23:H24").Style.Font.FontSize = 12;
                sht.Range("E23:H24").Merge();
                sht.Range("E23:H24").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E23:H24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("B25").Value = "PORT OF DISCHARGE";
                sht.Range("B25:D25").Style.Font.FontName = "Arial";
                sht.Range("B25:D25").Style.Font.FontSize = 12;
                sht.Range("B25:D25").Merge();
                sht.Range("B25:D25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B26").Value = Convert.ToString(ds.Tables[0].Rows[0]["portofdisc"]);
                sht.Range("B26:D27").Style.Font.FontName = "Arial";
                sht.Range("B26:D27").Style.Font.FontSize = 12;
                sht.Range("B26:D27").Merge();
                sht.Range("B26:D27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B26:D27").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("E25").Value = "FINAL DESTINATION";
                sht.Range("E25:H25").Style.Font.FontName = "Arial";
                sht.Range("E25:H25").Style.Font.FontSize = 11;
                sht.Range("E25:H25").Merge();
                sht.Range("E25:H25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E26").Value = Convert.ToString(ds.Tables[0].Rows[0]["COUNTRY"]);
                sht.Range("E26:H27").Style.Font.FontName = "Arial";
                sht.Range("E26:H27").Style.Font.FontSize = 12;
                sht.Range("E26:H27").Merge();
                sht.Range("E26:H27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E26:H27").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("I23").Value = "Term of";
                sht.Range("I23:J23").Style.Font.FontName = "Arial";
                sht.Range("I23:J23").Style.Font.FontSize = 12;
                sht.Range("I23:J23").Merge();
                sht.Range("I23:J23").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("I23:J23").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("L23").Value = "PRICE ARE F.C.A INDIA PORT PAYMENT : D/P.";
                sht.Range("L23:P23").Style.Font.FontName = "Arial";
                sht.Range("L23:P23").Style.Font.FontSize = 12;
                sht.Range("L23:P23").Merge();
                sht.Range("L23:P23").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("L23:P23").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("I24").Value = "Delivery and Payment";
                sht.Range("I24:K24").Style.Font.FontName = "Arial";
                sht.Range("I24:K24").Style.Font.FontSize = 12;
                sht.Range("I24:JK4").Merge();
                sht.Range("I24:K24").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("I24:K24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("L24").Value = "Delivery Week";
                sht.Range("L24:M24").Style.Font.FontName = "Arial";
                sht.Range("L24:M24").Style.Font.FontSize = 12;
                sht.Range("L24:M24").Merge();
                sht.Range("L24:M24").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("L24:M24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("N24").Value = Convert.ToString(ds.Tables[0].Rows[0]["DLVWEEK"]);
                sht.Range("N24:P24").Style.Font.FontName = "Arial";
                sht.Range("N24:P24").Style.Font.FontSize = 12;
                sht.Range("N24:P24").Merge();
                sht.Range("N24:P24").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("N24:P24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                sht.Range("B28:D28").Value = "Marks & Nos.";
                sht.Range("B28:D28").Merge();
                sht.Range("B28", "D28").Style.Alignment.WrapText = true;
                sht.Range("B28", "D28").Style.Font.Bold = true;
                sht.Range("B28", "D28").Style.Font.FontSize = 11;
                sht.Range("B28", "D28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B28:D28").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B29:D29").Value = "Container No.";
                sht.Range("B29:D29").Merge();
                sht.Range("B29", "D29").Style.Alignment.WrapText = true;
                sht.Range("B29", "D29").Style.Font.Bold = true;
                sht.Range("B29", "D29").Style.Font.FontSize = 11;
                sht.Range("B29", "D29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B29:D29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("E28").Value = "No. and kind of Packages";
                // sht.Range("B28:D29").Value = "Marks & Nos./Container No.";
                sht.Range("E28", "G29").Style.Alignment.WrapText = true;
                sht.Range("E28", "G29").Style.Font.Bold = true;
                sht.Range("E28", "G29").Style.Font.FontSize = 11;
                sht.Range("E28", "G29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E28", "G29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("E28:G29").Merge();


                sht.Range("H28").Value = "Description of Goods";
                sht.Range("H28", "K29").Style.Alignment.WrapText = true;
                sht.Range("H28", "K29").Style.Font.Bold = true;
                sht.Range("H28", "K29").Style.Font.FontSize = 11;
                sht.Range("H28", "K29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H28", "K29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("H28:K29").Merge();


                sht.Range("L28").Value = "QUANTITY";
                sht.Range("L28:M29").Merge();
                sht.Range("L28", "M29").Style.Alignment.WrapText = true;
                sht.Range("L28", "M29").Style.Font.Bold = true;
                sht.Range("L28", "M29").Style.Font.FontSize = 11;
                sht.Range("L28", "M29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("L28", "M29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("N28").Value = "RATE-";
                sht.Range("N28:O28").Merge();
                sht.Range("N28").Style.Alignment.WrapText = true;
                sht.Range("N28").Style.Font.Bold = true;
                sht.Range("N28").Style.Font.FontSize = 11;
                sht.Range("N28:O28").Style.Font.FontName = "Arial";
                sht.Range("N28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("N28:O28").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("N29").Value = "("+ds.Tables[0].Rows[0]["CURR"].ToString()+ ")"+ "-FCA";
                sht.Range("N29:O29").Merge();
                sht.Range("N29").Style.Alignment.WrapText = true;
                sht.Range("N29").Style.Font.Bold = true;
                sht.Range("N29").Style.Font.FontSize = 11;
                sht.Range("N29:O29").Style.Font.FontName = "Arial";
                sht.Range("N29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("N29:O29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                sht.Range("P28").Value = "AMOUNT-";
                sht.Range("P28:Q28").Merge();
                sht.Range("P28").Style.Alignment.WrapText = true;
                sht.Range("P28").Style.Font.Bold = true;
                sht.Range("P28").Style.Font.FontSize = 11;
                sht.Range("P28:Q28").Style.Font.FontName = "Arial";
                sht.Range("P28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("P28:Q28").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("P29").Value = "(" + ds.Tables[0].Rows[0]["CURR"].ToString() + ")" + "-FCA";
                sht.Range("P29:Q29").Merge();
                sht.Range("P29").Style.Alignment.WrapText = true;
                sht.Range("P29").Style.Font.Bold = true;
                sht.Range("P29").Style.Font.FontSize = 11;
                sht.Range("P29:Q29").Style.Font.FontName = "Arial";
                sht.Range("P29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("P29:Q29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                sht.Range("B30").Value = "IKEA";
                sht.Range("B30:D30").Merge();
                sht.Range("B30", "D30").Style.Alignment.WrapText = true;
                sht.Range("B30", "D30").Style.Font.Bold = true;
                sht.Range("B30", "D30").Style.Font.FontSize = 11;
                sht.Range("B30", "D30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B30", "D30").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("B31").Value = "1 TO " + sum + " PACKAGE";
                sht.Range("B31:D31").Merge();
                sht.Range("B31", "D31").Style.Alignment.WrapText = true;
                sht.Range("B31", "D31").Style.Font.Bold = true;
                sht.Range("B31", "D31").Style.Font.FontSize = 11;
                sht.Range("B31", "D31").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E30").Value = "Description of Product";
                // sht.Range("B28:D29").Value = "Marks & Nos./Container No.";
                sht.Range("E30", "G31").Style.Alignment.WrapText = true;
                sht.Range("E30", "G31").Style.Font.Bold = true;
                sht.Range("E30", "G31").Style.Font.FontSize = 11;
                sht.Range("E30", "G31").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E30", "G31").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("E30:G31").Merge();
                int row = 32;
                int pagerowcount = 15;
                StringBuilder STRBALE = new StringBuilder();
                decimal? totalamt = 0, totalpcs = 0, igst = 0, netwt = 0, wt = 0, cbm = 0;

                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                {

                    //wt += Convert.ToDecimal(ds.Tables[0].Rows[ii]["weight_roll"]);
                    //netwt += Convert.ToDecimal(ds.Tables[0].Rows[ii]["Netwt"]);
                    //cbm += Convert.ToDecimal(ds.Tables[0].Rows[ii]["volume_roll"]);
                    wt += Convert.ToDecimal(ds.Tables[0].Rows[ii]["csmgrowei"]);
                    netwt += Convert.ToDecimal(ds.Tables[0].Rows[ii]["csmgrownet"]);
                    cbm += Convert.ToDecimal(ds.Tables[0].Rows[ii]["csmgroval"]);
                    //double wt = ds.Tables[0].AsEnumerable().Sum(a => a.Field<double>("weight_roll"));
                    //Int16 netwt = ds.Tables[0].AsEnumerable().Sum(a => a.Field<Int16>("Netwt"));
                    //double cbm = ds.Tables[0].AsEnumerable().Sum(a => a.Field<double>("volume_roll"));
                    sht.Range("B" + row).Value = Convert.ToString(ds.Tables[0].Rows[ii]["srno"]);
                    sht.Range("B" + row + ":B" + (row)).Merge();
                    sht.Range("B" + row + ":B" + (row)).Style.Alignment.WrapText = true;
                    sht.Range("B" + row + ":B" + (row)).Style.Font.FontSize = 10;
                    sht.Range("B" + row + ":B" + (row)).Style.Font.Bold = true;
                    sht.Range("B" + row + ":B" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("B" + row + ":B" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    sht.Range("C" + row).Value = "Package";
                    sht.Range("C" + row + ":D" + (row)).Merge();
                    sht.Range("C" + row + ":D" + (row)).Style.Alignment.WrapText = true;
                    sht.Range("C" + row + ":D" + (row)).Style.Font.FontSize = 10;
                    sht.Range("C" + row + ":D" + (row)).Style.Font.Bold = true;
                    sht.Range("C" + row + ":D" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C" + row + ":D" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    sht.Range("E" + row).Value = Convert.ToString(ds.Tables[0].Rows[ii]["pkgno"]);
                    sht.Range("E" + row + ":I" + (row)).Merge();
                    sht.Range("E" + row + ":I" + (row)).Style.Alignment.WrapText = true;
                    sht.Range("E" + row + ":I" + (row)).Style.Font.FontSize = 10;
                    sht.Range("E" + row + ":I" + (row)).Style.Font.Bold = true;
                    sht.Range("E" + row + ":I" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("E" + row + ":I" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("J" + row).Value = "PO#";
                    sht.Range("J" + row + ":J" + (row)).Merge();
                    sht.Range("J" + row + ":J" + (row)).Style.Alignment.WrapText = true;
                    sht.Range("J" + row + ":J" + (row)).Style.Font.FontSize = 10;
                    sht.Range("J" + row + ":J" + (row)).Style.Font.Bold = true;
                    sht.Range("J" + row + ":J" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("J" + row + ":J" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    row += 1;
                    sht.Range("C" + row).Value = Convert.ToString(ds.Tables[0].Rows[ii]["itemdesc"]);
                    sht.Range("C" + row + ":I" + (row)).Merge();
                    sht.Range("C" + row + ":I" + (row)).Style.Alignment.WrapText = true;
                    sht.Range("C" + row + ":I" + (row)).Style.Font.FontSize = 10;
                    sht.Range("C" + row + ":I" + (row)).Style.Font.Bold = true;
                    sht.Range("C" + row + ":I" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C" + row + ":I" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    sht.Range("J" + row).Value = Convert.ToString(ds.Tables[0].Rows[ii]["orderid"]);
                    sht.Range("J" + row + ":K" + (row)).Merge();
                    sht.Range("J" + row + ":K" + (row)).Style.Alignment.WrapText = true;
                    sht.Range("J" + row + ":K" + (row)).Style.Font.FontSize = 10;
                    sht.Range("J" + row + ":K" + (row)).Style.Font.Bold = true;
                    sht.Range("J" + row + ":K" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("J" + row + ":K" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    sht.Range("L" + row).Value = Convert.ToString(ds.Tables[0].Rows[ii]["bkdqty"]) + " Pcs";
                    sht.Range("L" + row + ":M" + (row)).Merge();
                    sht.Range("L" + row + ":M" + (row)).Style.Alignment.WrapText = true;
                    sht.Range("L" + row + ":M" + (row)).Style.Font.FontSize = 10;
                    sht.Range("L" + row + ":M" + (row)).Style.Font.Bold = true;
                    sht.Range("L" + row + ":M" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("L" + row + ":M" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    totalpcs += Convert.ToInt32(ds.Tables[0].Rows[ii]["bkdqty"]);
                    if (ds.Tables[0].Rows[0]["curr"].ToString().ToUpper() == "USD")
                    {
                        sht.Range("N" + row).Value = Convert.ToDecimal(Convert.ToDecimal(ds.Tables[0].Rows[ii]["price"]) / Convert.ToDecimal(ds.Tables[0].Rows[ii]["CurrentRateRefRs"]));
                        sht.Range("O" + row).Value = "each"; 
                    }
                    else
                    { sht.Range("N" + row).Value = Convert.ToDecimal(ds.Tables[0].Rows[ii]["price"]); sht.Range("O" + row).Value = "each"; }
                    //sht.Range("N" + row + ":O" + (row)).Merge();
                    sht.Range("N" + row ).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("N" + row + ":O" + (row)).Style.Alignment.WrapText = true;
                    sht.Range("N" + row + ":O" + (row)).Style.Font.FontSize = 12;
                    sht.Range("N" + row + ":O" + (row)).Style.Font.Bold = true;
                    sht.Range("N" + row + ":O" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("N" + row + ":O" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    int iqty = 0;
                    decimal rate = 0, totalval = 0;
                    if (ds.Tables[0].Rows[0]["curr"].ToString().ToUpper() == "USD")
                    {
                        totalval = Convert.ToDecimal(ds.Tables[0].Rows[ii]["bkdqty"]) * (Convert.ToDecimal(ds.Tables[0].Rows[ii]["price"]) / Convert.ToDecimal(ds.Tables[0].Rows[ii]["CurrentRateRefRs"]));
                    }
                    else {
                        totalval = Convert.ToDecimal(ds.Tables[0].Rows[ii]["bkdqty"]) * Convert.ToDecimal(ds.Tables[0].Rows[ii]["price"]);
                    
                    
                    }
                   

                    sht.Range("p" + row).Value = totalval;
                    sht.Range("P" + row ).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("p" + row + ":Q" + (row)).Merge();
                    sht.Range("p" + row + ":Q" + (row)).Style.Alignment.WrapText = true;
                    sht.Range("p" + row + ":Q" + (row)).Style.Font.FontSize = 12;
                    sht.Range("p" + row + ":Q" + (row)).Style.Font.Bold = true;
                    sht.Range("p" + row + ":Q" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("p" + row + ":Q" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    totalamt += totalval;
                    row += 1;
                    sht.Range("C" + row).Value = "Article NO." + Convert.ToString(ds.Tables[0].Rows[ii]["articleno"]);
                    sht.Range("C" + row + ":G" + (row)).Merge();
                    sht.Range("C" + row + ":G" + (row)).Style.Alignment.WrapText = true;
                    sht.Range("C" + row + ":G" + (row)).Style.Font.FontSize = 10;
                    sht.Range("C" + row + ":G" + (row)).Style.Font.Bold = true;
                    sht.Range("C" + row + ":G" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C" + row + ":G" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    sht.Range("J" + row).Value = "HS Code: " + Convert.ToString(ds.Tables[0].Rows[ii]["hsn"]);
                    sht.Range("J" + row + ":K" + (row)).Merge();
                    sht.Range("J" + row + ":K" + (row)).Style.Alignment.WrapText = true;
                    sht.Range("J" + row + ":K" + (row)).Style.Font.FontSize = 10;
                    sht.Range("J" + row + ":K" + (row)).Style.Font.Bold = true;
                    sht.Range("J" + row + ":K" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("J" + row + ":K" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    row += 1;
                    sht.Range("C" + row).Value = "Complete article description";
                    sht.Range("C" + row + ":I" + (row)).Merge();
                    sht.Range("C" + row + ":I" + (row)).Style.Alignment.WrapText = true;
                    sht.Range("C" + row + ":I" + (row)).Style.Font.FontSize = 10;
                    sht.Range("C" + row + ":I" + (row)).Style.Font.Bold = true;
                    sht.Range("C" + row + ":I" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C" + row + ":I" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    row += 1;
                    sht.Range("C" + row).Value = Convert.ToString(ds.Tables[0].Rows[ii]["contents"]);
                    sht.Range("C" + row + ":I" + (row)).Merge();
                    sht.Range("C" + row + ":I" + (row)).Style.Alignment.WrapText = true;
                    sht.Range("C" + row + ":I" + (row)).Style.Font.FontSize = 10;
                    sht.Range("C" + row + ":I" + (row)).Style.Font.Bold = true;
                    sht.Range("C" + row + ":I" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C" + row + ":I" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    row += 1;
                    sht.Range("C" + row).Value = "Package:" + Convert.ToString(ds.Tables[0].Rows[ii]["pcs_roll"]) + " Pcs";
                    sht.Range("C" + row + ":D" + (row)).Merge();
                    sht.Range("C" + row + ":D" + (row)).Style.Alignment.WrapText = true;
                    sht.Range("C" + row + ":D" + (row)).Style.Font.FontSize = 10;
                    sht.Range("C" + row + ":D" + (row)).Style.Font.Bold = true;
                    sht.Range("C" + row + ":D" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C" + row + ":D" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    //sht.Range("F" + row).Value = "Net wt:" + Convert.ToString(ds.Tables[0].Rows[ii]["Netwt"]) + " Kg";
                    //sht.Range("F" + row + ":G" + (row)).Merge();
                    //sht.Range("F" + row + ":G" + (row)).Style.Alignment.WrapText = true;
                    //sht.Range("F" + row + ":G" + (row)).Style.Font.FontSize = 10;
                    //sht.Range("F" + row + ":G" + (row)).Style.Font.Bold = true;
                    //sht.Range("F" + row + ":G" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    //sht.Range("F" + row + ":G" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    //sht.Range("H" + row).Value = "Gross wt:" + Convert.ToString(ds.Tables[0].Rows[ii]["Weight_roll"]) + " Kg";
                    //sht.Range("H" + row + ":I" + (row)).Merge();
                    //sht.Range("H" + row + ":I" + (row)).Style.Alignment.WrapText = true;
                    //sht.Range("H" + row + ":I" + (row)).Style.Font.FontSize = 10;
                    //sht.Range("H" + row + ":I" + (row)).Style.Font.Bold = true;
                    //sht.Range("H" + row + ":I" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    //sht.Range("H" + row + ":I" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);                     

                    row += 1;
                    sht.Range("B" + (row - 8) + ":B" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("Q" + (row - 8) + ":Q" + (row)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    sht.Range("L" + (row - 8) + ":L" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("N" + (row - 8) + ":N" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("P" + (row - 8) + ":P" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                }
                if (row < 46)
                {
                    int lesslastrow = row;
                    row += (45 - row);
                    sht.Range("B" + (lesslastrow) + ":B" + (row + pagerowcount)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("Q" + (lesslastrow) + ":Q" + (row + pagerowcount)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    sht.Range("L" + (lesslastrow) + ":L" + (row + pagerowcount)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("N" + (lesslastrow) + ":N" + (row + pagerowcount)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("P" + (lesslastrow) + ":P" + (row + pagerowcount)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                }
                else
                {
                    int lastrow = row - 1;

                    row += pagerowcount - 1;
                    sht.Range("B" + (lastrow) + ":B" + (lastrow + pagerowcount)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("Q" + (lastrow) + ":Q" + (lastrow + pagerowcount)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    sht.Range("L" + (lastrow) + ":L" + (lastrow + pagerowcount)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("N" + (lastrow) + ":N" + (lastrow + pagerowcount)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("P" + (lastrow) + ":P" + (lastrow + pagerowcount)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;


                }

                sht.Range("B" + (row) + ":Q" + (row)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                row += 1;
                sht.Range("B" + row).Value = "TOTAL Amount chargeable";
                sht.Range("B" + row + ":D" + (row)).Merge();
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.WrapText = true;
                sht.Range("B" + row + ":D" + (row)).Style.Font.FontSize = 10;
                sht.Range("B" + row + ":D" + (row)).Style.Font.Bold = true;
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("B" + (row) + ":Q" + (row)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("B" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("Q" + (row)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("E" + row).Value = "( IN WORDS ):";
                sht.Range("E" + row + ":G" + (row)).Merge();
                sht.Range("E" + row + ":G" + (row)).Style.Alignment.WrapText = true;
                sht.Range("E" + row + ":G" + (row)).Style.Font.FontSize = 10;
                sht.Range("E" + row + ":G" + (row)).Style.Font.Bold = true;
                sht.Range("E" + row + ":G" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("E" + row + ":G" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("L" + row).Value = totalpcs + " Pcs";
                sht.Range("L" + row + ":M" + (row)).Merge();
                sht.Range("L" + row + ":M" + (row)).Style.Alignment.WrapText = true;
                sht.Range("L" + row + ":M" + (row)).Style.Font.FontSize = 10;
                sht.Range("L" + row + ":M" + (row)).Style.Font.Bold = true;
                sht.Range("L" + row + ":M" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("L" + row + ":M" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                if (Convert.ToInt32(ds.Tables[0].Rows[0]["igst"]) > 0)
                {
                    sht.Range("N" + row).Value = "TAX AMOUNT";
                }
                else
                { sht.Range("N" + row).Value = "TOTAL AMOUNT"; }
                sht.Range("N" + row + ":O" + (row)).Merge();
                sht.Range("N" + row + ":O" + (row)).Style.Alignment.WrapText = true;
                sht.Range("N" + row + ":O" + (row)).Style.Font.FontSize = 10;
                sht.Range("N" + row + ":O" + (row)).Style.Font.Bold = true;
                sht.Range("N" + row + ":O" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("N" + row + ":O" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range("P" + row).Value = String.Format("{0:#0.00}", (Convert.ToDouble(totalamt)));
                sht.Range("P" + row).Style.NumberFormat.Format = "#,###0.00";
                sht.Range("P" + row + ":Q" + (row)).Merge();
                sht.Range("P" + row + ":Q" + (row)).Style.Alignment.WrapText = true;
                sht.Range("P" + row + ":Q" + (row)).Style.Font.FontSize = 12;
                sht.Range("P" + row + ":Q" + (row)).Style.Font.Bold = true;
                sht.Range("P" + row + ":Q" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("P" + row + ":Q" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("L" + (row) + ":L" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("N" + (row) + ":N" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("P" + (row) + ":P" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("L" + (row) + ":Q" + (row)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //  sht.Range("B" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("Q" + (row)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                row += 1;
                sht.Range("L" + (row) + ":L" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("N" + (row) + ":N" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("P" + (row) + ":P" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("L" + (row) + ":Q" + (row)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //  sht.Range("B" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("Q" + (row)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                string amountinwords = "";
                string amt = totalamt.ToString();
                string val = "", paise = "";

                if (amt.IndexOf('.') > 0)
                {
                    val = amt.ToString().Split('.')[0];
                    if (ds.Tables[0].Rows[0]["CURR"].ToString() == "USD")
                    {
                        amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val)) + " Dollor";
                    }
                    else
                    {
                        amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val)) +" Rupee";
                    }
                    
                }
                else
                {
                    if (ds.Tables[0].Rows[0]["CURR"].ToString() == "USD")
                    {
                        amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(totalamt)) + " Dollor ";
                    }
                    else { amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(totalamt)) + " Rupee "; }
                }

                decimal total = decimal.Round((decimal)totalamt, 2, MidpointRounding.AwayFromZero);
                string Pointamt = string.Format("{0:0.00}", Convert.ToString(total));
                val = "";
                if (Pointamt.IndexOf('.') > 0)
                {
                    val = Pointamt.ToString().Split('.')[1];
                    if (Convert.ToInt32(val) > 0)
                    {
                        string valamount = string.Format("{0:0.00}", Convert.ToString(val));
                        if (ds.Tables[0].Rows[0]["CURR"].ToString() == "USD")
                        {
                            paise = " and"+ ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(valamount)) + " Cent ";
                        }
                        else
                        {
                            paise = " and" + ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(valamount)) + " paise ";

                        }
                    }
                }
                amountinwords = amountinwords + " " + paise + "Only";
                sht.Range("B" + row).Value = amountinwords.ToUpper();
                sht.Range("B" + row + ":K" + (row)).Merge();
                sht.Range("B" + row + ":K" + (row)).Style.Alignment.WrapText = true;
                sht.Range("B" + row + ":K" + (row)).Style.Font.FontSize = 10;
                sht.Range("B" + row + ":K" + (row)).Style.Font.Bold = true;
                sht.Range("B" + row + ":K" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B" + row + ":K" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("B" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("Q" + (row)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                if (Convert.ToInt32(ds.Tables[0].Rows[0]["igst"]) > 0)
                {
                    sht.Range("P" + row).Value = "IGST |" + Convert.ToString(ds.Tables[0].Rows[0]["igst"]) + " %";
                    sht.Range("P" + row).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("P" + row + ":Q" + (row)).Merge();
                    sht.Range("P" + row + ":Q" + (row)).Style.Alignment.WrapText = true;
                    sht.Range("P" + row + ":Q" + (row)).Style.Font.FontSize = 12;
                    sht.Range("P" + row + ":Q" + (row)).Style.Font.Bold = true;

                    sht.Range("Q" + row).Value = String.Format("{0:#0.00}", (Convert.ToDouble((totalamt * Convert.ToDecimal(ds.Tables[0].Rows[0]["igst"])) / 100)));
                    sht.Range("Q" + row).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("Q" + row + ":Q" + (row)).Merge();
                    sht.Range("Q" + row + ":Q" + (row)).Style.Alignment.WrapText = true;
                    sht.Range("Q" + row + ":Q" + (row)).Style.Font.FontSize = 12;
                    sht.Range("Q" + row + ":Q" + (row)).Style.Font.Bold = true;
                }


                row += 2;
                sht.Range("B" + row).Value = "Total Gross weight";
                sht.Range("B" + row + ":D" + (row)).Merge();
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.WrapText = true;
                sht.Range("B" + row + ":D" + (row)).Style.Font.FontSize = 10;
                sht.Range("B" + row + ":D" + (row)).Style.Font.Bold = true;
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //sht.Range("B" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("Q" + (row)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                sht.Range("G" + row).Value = wt + "    " + "KGS";
                sht.Range("G" + row + ":H" + (row)).Merge();
                sht.Range("G" + row + ":H" + (row)).Style.Alignment.WrapText = true;
                sht.Range("G" + row + ":H" + (row)).Style.Font.FontSize = 10;
                sht.Range("G" + row + ":H" + (row)).Style.Font.Bold = true;


                sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                row += 1;
                sht.Range("B" + row).Value = "Total Net weight";
                sht.Range("B" + row + ":D" + (row)).Merge();
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.WrapText = true;
                sht.Range("B" + row + ":D" + (row)).Style.Font.FontSize = 10;
                sht.Range("B" + row + ":D" + (row)).Style.Font.Bold = true;
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("G" + row).Value = netwt + "    " + "KGS";
                sht.Range("G" + row + ":H" + (row)).Merge();
                sht.Range("G" + row + ":H" + (row)).Style.Alignment.WrapText = true;
                sht.Range("G" + row + ":H" + (row)).Style.Font.FontSize = 10;
                sht.Range("G" + row + ":H" + (row)).Style.Font.Bold = true;
                //sht.Range("B" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("Q" + (row)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                row += 1;
                sht.Range("B" + row).Value = "Total Vol.";
                sht.Range("B" + row + ":D" + (row)).Merge();
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.WrapText = true;
                sht.Range("B" + row + ":D" + (row)).Style.Font.FontSize = 10;
                sht.Range("B" + row + ":D" + (row)).Style.Font.Bold = true;
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("G" + row).Value = cbm + "    " + "CBM";
                sht.Range("G" + row + ":H" + (row)).Merge();
                sht.Range("G" + row + ":H" + (row)).Style.Alignment.WrapText = true;
                sht.Range("G" + row + ":H" + (row)).Style.Font.FontSize = 10;
                sht.Range("G" + row + ":H" + (row)).Style.Font.Bold = true;
                if (chkrex.Checked)
                {

                    sht.Range("I" + row).Value = "REX NO. INREX0610004182EC026";
                    sht.Range("I" + row + ":K" + (row)).Merge();
                    sht.Range("I" + row + ":K" + (row)).Style.Alignment.WrapText = true;
                    sht.Range("I" + row + ":K" + (row)).Style.Font.FontSize = 10;
                    sht.Range("I" + row + ":K" + (row)).Style.Font.Bold = true;
                }




                row += 1;
                sht.Range("B" + row).Value = "Certified that";
                sht.Range("B" + row + ":D" + (row)).Merge();
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.WrapText = true;
                sht.Range("B" + row + ":D" + (row)).Style.Font.FontSize = 8;
                sht.Range("B" + row + ":D" + (row)).Style.Font.Bold = true;
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                row += 1;
                sht.Range("B" + row).Value = "1. Manufacturers are themselves Exporters of Rugs and Carpets in India and having Import - Export Code No. 0610004182 and RBI Code No. -6392040-2600009";
                sht.Range("B" + row + ":K" + (row + 1)).Merge();
                sht.Range("B" + row + ":K" + (row + 1)).Style.Alignment.WrapText = true;
                sht.Range("B" + row + ":K" + (row + 1)).Style.Font.FontSize = 8;
                sht.Range("B" + row + ":K" + (row + 1)).Style.Font.Bold = true;
                sht.Range("B" + row + ":K" + (row + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B" + row + ":K" + (row + 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("M" + row).Value = "Signature";
                sht.Range("M" + row + ":N" + (row)).Merge();
                sht.Range("M" + row + ":N" + (row)).Style.Alignment.WrapText = true;
                sht.Range("M" + row + ":N" + (row)).Style.Font.FontSize = 12;
                sht.Range("M" + row + ":N" + (row)).Style.Font.Bold = true;
                sht.Range("M" + row + ":N" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("M" + row + ":N" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("P" + row).Value = "Diamond Exports";
                sht.Range("P" + row + ":Q" + (row)).Merge();
                sht.Range("P" + row + ":Q" + (row)).Style.Alignment.WrapText = true;
                sht.Range("P" + row + ":Q" + (row)).Style.Font.FontSize = 12;
                sht.Range("P" + row + ":Q" + (row)).Style.Font.Bold = true;
                sht.Range("P" + row + ":Q" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("P" + row + ":Q" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("M" + (row) + ":Q" + (row)).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                row += 2;
                sht.Range("B" + row).Value = "2. Goods Exported are of 'Indian Origin' and Made in India.";
                sht.Range("B" + row + ":K" + (row)).Merge();
                sht.Range("B" + row + ":K" + (row)).Style.Alignment.WrapText = true;
                sht.Range("B" + row + ":K" + (row)).Style.Font.FontSize = 8;
                sht.Range("B" + row + ":K" + (row)).Style.Font.Bold = true;
                sht.Range("B" + row + ":K" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B" + row + ":K" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                row += 1;
                sht.Range("B" + row).Value = "3. The Goods are factory produced";
                sht.Range("B" + row + ":K" + (row)).Merge();
                sht.Range("B" + row + ":K" + (row)).Style.Alignment.WrapText = true;
                sht.Range("B" + row + ":K" + (row)).Style.Font.FontSize = 8;
                sht.Range("B" + row + ":K" + (row)).Style.Font.Bold = true;
                sht.Range("B" + row + ":K" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B" + row + ":K" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                row += 1;
                sht.Range("B" + row).Value = "DECLARATION: WE DECLARE THAT THIS INVOICE SHOWS THE ACTUAL PRICE OF THE GOODS DESCRIBED AND THAT ALL PARTICULARS ARE TRUE AND CORRECT";
                sht.Range("B" + row + ":K" + (row + 1)).Merge();
                sht.Range("B" + row + ":K" + (row + 1)).Style.Alignment.WrapText = true;
                sht.Range("B" + row + ":K" + (row + 1)).Style.Font.FontSize = 8;
                sht.Range("B" + row + ":K" + (row + 1)).Style.Font.Bold = true;
                sht.Range("B" + row + ":K" + (row + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B" + row + ":K" + (row + 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                if (chkrex.Checked)
                {
                    row += 2;
                    sht.Range("B" + row).Value = "STATEMENT OF ORIGIN";
                    sht.Range("B" + row + ":K" + (row)).Merge();
                    sht.Range("B" + row + ":K" + (row)).Style.Alignment.WrapText = true;
                    sht.Range("B" + row + ":K" + (row)).Style.Font.FontSize = 8;
                    sht.Range("B" + row + ":K" + (row)).Style.Font.Bold = true;
                    sht.Range("B" + row + ":K" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("B" + row + ":K" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    row += 1;
                    sht.Range("B" + row).Value = "THE EXPORTER INREX0610004182EC026 OF THE PRODUCTS COVERED BY THIS DOCUMENT DECLARES THAT, EXCEPT WHERE OTHERWISE CLEARLY INDICATED, THESE PRODUCTS ARE OF INDIA PREFERENTIAL ORIGIN ACCORDING TO RULES OF ORIGIN OF THE GENERALISED SYSTEM OF PREFERENCES OF THE EUROPEAN UNION AND THAT THE ORIGIN CRITERION MET IS 'P'.";
                    sht.Range("B" + row + ":K" + (row + 2)).Merge();
                    sht.Range("B" + row + ":K" + (row + 2)).Style.Alignment.WrapText = true;
                    sht.Range("B" + row + ":K" + (row + 2)).Style.Font.FontSize = 8;
                    sht.Range("B" + row + ":K" + (row + 2)).Style.Font.Bold = true;
                    sht.Range("B" + row + ":K" + (row + 2)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("B" + row + ":K" + (row + 2)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                }
                sht.Range("N" + row).Value = "Authorised Signatory";
                sht.Range("N" + row + ":O" + (row)).Merge();
                sht.Range("N" + row + ":O" + (row)).Style.Alignment.WrapText = true;
                sht.Range("N" + row + ":O" + (row)).Style.Font.FontSize = 12;
                sht.Range("N" + row + ":O" + (row)).Style.Font.Bold = true;
                sht.Range("N" + row + ":O" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("N" + row + ":O" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                row += 1;
                if (chkrex.Checked)
                {
                    sht.Range("B" + (row - 13) + ":B" + (row + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("Q" + (row - 13) + ":Q" + (row + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    sht.Range("M" + (row - 8) + ":M" + (row + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("B" + (row + 1) + ":Q" + (row + 1)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                else
                {
                    sht.Range("B" + (row - 11) + ":B" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("Q" + (row - 11) + ":Q" + (row)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    sht.Range("M" + (row - 5) + ":M" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("B" + (row) + ":Q" + (row)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;


                }
                string Fileextension = "xlsx";
                string filename = "Invoice." + Fileextension + "";
                Path = Server.MapPath("~/InvoiceExcel/" + filename);
                xapp.SaveAs(Path);

                xapp.Dispose();


                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                //   Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
            }

        }
        catch (Exception ex)
        { 
        }

    }
    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
        // Confirms that an HtmlForm control is rendered for the
        // specified ASP.NET server control at run time.
    }
    protected void btnprintpkglist_Click(object sender, EventArgs e)
    {
        if (!Directory.Exists(Server.MapPath("~/InvoiceExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/InvoiceExcel/"));
        }

        try
        {
            string Path = "";
            string Pathpdf = "";
            string str = "";

            //str = @"select ROW_NUMBER() over(order by(select 1)) srno,pi.*,dm.*,pa.*,ci.LUTARNNo,ci.LUTIssueDate from preinvoice pi join Destinationmaster dm  on left(pi.desccode, CHARINDEX('-', REVERSE('-' + pi.desccode)))=dm.Destcode left join Packingarticle pa on pi.articleno=pa.ArticleNo left join CompanyInfo ci on ci.MasterCompanyid=pi.MasterCompanyid where pi.invoiceno='" + txtinvoiceno.Text + "' order by dm.Id";

            str = @"select ROW_NUMBER() over(order by(select 1)) srno,pi.*,dm.*,pa.*,ci.LUTARNNo,ci.LUTIssueDate from preinvoice pi join Destinationmaster dm  on pi.desccode=dm.Destcode left join Packingarticle pa on pi.articleno=pa.ArticleNo left join CompanyInfo ci on ci.MasterCompanyid=pi.MasterCompanyid where pi.invoiceno='" + txtinvoiceno.Text + "' order by dm.Id";


            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            var xapp = new XLWorkbook();

            var sht = xapp.Worksheets.Add("Packing");
            int sum = ds.Tables[0].AsEnumerable().Sum(a => a.Field<int>("csmul"));
            var orders = ds.Tables[0].AsEnumerable().Select(a => a.Field<string>("orderid"));
            string buyerordernos = string.Join(",", orders);
            //************set cell width
            //Page
            //  sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
            sht.PageSetup.AdjustTo(95);
            sht.PageSetup.FitToPages(1, 1);
            sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
            sht.PageSetup.VerticalDpi = 300;
            sht.PageSetup.HorizontalDpi = 300;
            sht.PageSetup.Margins.Top = 0.2;
            sht.PageSetup.Margins.Bottom = 0.1;
            sht.PageSetup.Margins.Right = 0.1;
            sht.PageSetup.Margins.Left = 0.1;
            sht.Column("N").Width = 12.89;
            sht.Column("B").Width = 13.89;
            sht.Column("O").Width = 10.89;
            sht.Column("K").Width = 10.89;
            sht.Column("P").Width = 15.89;
            sht.Column("Q").Width = 10.89;
            sht.Style.Font.FontName = "Arial";



            //*****Header                
            sht.Cell("B1").Value = "PACKING LIST";
            sht.Range("B1:Q1").Style.Font.FontName = "Arial";
            sht.Range("B1:Q1").Style.Font.FontSize = 16;
            sht.Range("B1:Q1").Style.Font.Bold = true;
            sht.Range("B1:Q1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("B1:Q1").Merge();
            if (chklut.Checked)
            {
                sht.Cell("B2").Value = "EXPORT MEANT FOR EXPORT WITHOUT PAYMENT OF IGST AGAINST LUT No. " + Convert.ToString(ds.Tables[0].Rows[0]["LUTARNNo"]) + "";
                sht.Range("B2:M2").Style.Font.FontName = "Arial";
                sht.Range("B2:M2").Style.Font.FontSize = 12;
                sht.Range("B2:M2").Style.Font.Bold = true;
                //  sht.Range("B2:M2").Style.Fill.BackgroundColor = XLColor.Yellow;
                sht.Range("B2:M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B2:M2").Merge();

                sht.Cell("N2").Value = "Date -" + Convert.ToDateTime(ds.Tables[0].Rows[0]["LUTIssueDate"]).ToString("dd/MMM/yyyy");
                sht.Range("N2:Q2").Style.Font.FontName = "Arial";
                sht.Range("N2:Q2").Style.Font.FontSize = 12;
                sht.Range("N2:Q2").Style.Font.Bold = true;
                // sht.Range("N2:Q2").Style.Fill.BackgroundColor = XLColor.Yellow;
                sht.Range("N2:Q2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("N2:Q2").Merge();
                sht.Range("N2:Q2").Style.NumberFormat.Format = "dd/MMM/yyyy";



            }
            sht.Range("B3").Value = "EXPORTER";
            sht.Range("B3:D3").Style.Font.FontName = "Arial";
            sht.Range("B3:D3").Style.Font.FontSize = 12;
            //  sht.Range("A3:C3").Style.Alignment.WrapText = true;
            sht.Range("B3:D3").Merge();
            //*****************

            //CompanyName
            sht.Range("B4").Value = "Diamond Exports";
            sht.Range("B4:E4").Style.Font.FontName = "Arial";
            sht.Range("B4:E4").Style.Font.FontSize = 12;
            // sht.Range("B4:E4").Style.Font.Bold = true;
            //  sht.Range("A3:C4").Style.Alignment.WrapText = true;
            sht.Range("B4:E4").Merge();
            //Address

            sht.Range("B5").Value = "DIAMOND PLEX , BAMRAULI KATARA FATEHABAD ROAD AGRA (UTTAR PRADESH) - 282006 INDIA";
            sht.Range("B5:F6").Style.Font.FontName = "Arial";
            sht.Range("B5:F6").Style.Font.FontSize = 11;
            sht.Range("B5:F6").Style.Alignment.WrapText = true;
            sht.Range("B5:F6").Merge();
            //address2
            sht.Range("B7").Value = "STATE OF ORIGIN & CODE : UTTAR PRADESH & 09 DISTRICT & CODE : AGRA & 118 GSTIN : 09AALFD7697M2ZD";
            sht.Range("B7:F8").Style.Font.FontName = "Arial";
            sht.Range("B7:F8").Style.Font.FontSize = 11;
            sht.Range("B7:F8").Style.Alignment.WrapText = true;
            sht.Range("B7:F8").Merge();

            sht.Range("I3").Value = "Invoice No.";
            sht.Range("I3:J3").Style.Font.FontName = "Arial";
            sht.Range("I3:J3").Style.Font.FontSize = 12;
            sht.Range("I3:J3").Merge();

            sht.Range("K3").Value = Convert.ToString(ds.Tables[0].Rows[0]["invoiceno"]);
            sht.Range("K3:L3").Style.Font.FontName = "Arial";
            sht.Range("K3:L3").Style.Font.FontSize = 12;
            sht.Range("K3:L3").Merge();

            sht.Range("I4").Value = "Invoice Date";
            sht.Range("I4:J4").Style.Font.FontName = "Arial";
            sht.Range("I4:J4").Style.Font.FontSize = 12;
            //  sht.Range("I4:J4").Style.Font.Bold = true;
            sht.Range("I4:J4").Merge();
            sht.Range("K4").Value = Convert.ToString(ds.Tables[0].Rows[0]["invoicedate"]);
            sht.Range("K4:L4").Style.Font.FontName = "Arial";
            sht.Range("K4:L4").Style.Font.FontSize = 12;
            sht.Range("K4:L4").Style.NumberFormat.Format = "dd/MMM/yyyy";
            sht.Range("K4:L4").Merge();
            sht.Range("M3").Value = "Office of the Commissioner of Customs(Prev.)";
            sht.Range("M3:Q3").Style.Font.FontName = "Arial";
            sht.Range("M3:Q3").Style.Font.FontSize = 12;
            sht.Range("M3:Q3").Merge();
            sht.Range("M4").Value = "Reg No/IEC-0610004182  YEAR-2010";
            sht.Range("M4:Q4").Style.Font.FontName = "Arial";
            sht.Range("M4:Q4").Style.Font.FontSize = 12;
            sht.Range("M4:Q4").Merge();
            sht.Range("I5").Value = "BUYER'S ORDER NO.";
            sht.Range("I5:K5").Style.Font.FontName = "Arial";
            sht.Range("I5:K5").Style.Font.FontSize = 12;
            sht.Range("I5:K5").Merge();
            sht.Range("L5").Value = "'" + buyerordernos;
            sht.Range("L5:Q8").Style.Font.FontName = "Arial";
            sht.Range("L5:Q8").Style.Font.FontSize = 12;
            sht.Range("L5:Q8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("L5:Q8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("L5:Q8").Merge();
            sht.Range("I6:K8").Merge();
            sht.Range("I4:Q4").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("H3:Q3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("H4:Q4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B1:B31").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("Q1:Q31").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("K5:K13").Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("I3:I27").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("D19:D27").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("M14:I18").Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("B2:Q2").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("B1:Q1").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("B8:Q8").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("B27:Q27").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("B29:Q29").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("B31:Q31").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("B13:Q13").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("B18:Q18").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("B21:H21").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("I21:Q21").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("B24:H24").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("M14:M21").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("L28:L31").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("N28:N31").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("P28:P31").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("B9").Value = "CONSIGNEE";
            sht.Range("B9:C9").Style.Font.FontName = "Arial";
            sht.Range("B9:C9").Style.Font.FontSize = 12;
            sht.Range("B9:C9").Merge();
            sht.Range("B9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B10").Value = Convert.ToString(ds.Tables[0].Rows[0]["Consignee_Address"]);
            sht.Range("B10:F13").Style.Font.FontName = "Arial";
            sht.Range("B10:F13").Style.Font.FontSize = 12;
            sht.Range("B10:F13").Merge();
            sht.Range("B10:F13").Style.Alignment.WrapText = true;
            sht.Range("G9").Value = Convert.ToString(ds.Tables[0].Rows[0]["Consignee_dt"]);
            sht.Range("G9:H9").Style.Font.FontName = "Arial";
            sht.Range("G9:H9").Style.Font.FontSize = 12;
            sht.Range("G9:H9").Merge();
            sht.Range("G9:H9").Style.Alignment.WrapText = true;
            sht.Range("G9:H9").Style.Font.Bold = true;
            sht.Range("G9:H9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("I9").Value = "OTHER REFERENCE(S)";
            sht.Range("I9:K9").Style.Font.FontName = "Arial";
            sht.Range("I9:K9").Style.Font.FontSize = 12;
            sht.Range("I9:L9").Merge();
            sht.Range("I11").Value = "CONSIGNMENT NO";
            sht.Range("I11:K11").Style.Font.FontName = "Arial";
            sht.Range("I11:K11").Style.Font.FontSize = 12;
            //sht.Range("I11:K11").Style.Font.Bold = true;
            sht.Range("I11:K11").Merge();
            sht.Range("I13").Value = "SUPPLIER NO";
            sht.Range("I13:K13").Style.Font.FontName = "Arial";
            sht.Range("I13:K13").Style.Font.FontSize = 12;
            //sht.Range("I13:K13").Style.Font.Bold = true;
            sht.Range("I13:K13").Merge();

            sht.Range("L9").Value = "";
            sht.Range("L9:Q9").Style.Font.FontName = "Arial";
            sht.Range("L9:Q9").Style.Font.FontSize = 12;
            sht.Range("L9:Q9").Merge();
            sht.Range("L11").Value = Convert.ToString(ds.Tables[0].Rows[0]["csmno"]);
            sht.Range("L11:Q11").Style.Font.FontName = "Arial";
            sht.Range("L11:Q11").Style.Font.FontSize = 12;
            sht.Range("L11:Q11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("L11:Q11").Merge();
            sht.Range("L11:Q11").Style.Font.Bold = true;
            sht.Range("L13").Value = "23130";
            sht.Range("L13:Q13").Style.Font.FontName = "Arial";
            sht.Range("L13:Q13").Style.Font.FontSize = 12;
            sht.Range("L13:Q13").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("L13:Q13").Merge();
            sht.Range("L13:Q13").Style.Font.Bold = true;

            sht.Range("B14").Value = "RECEIVER";
            sht.Range("B14:C14").Style.Font.FontName = "Arial";
            sht.Range("B14:C14").Style.Font.FontSize = 12;
            sht.Range("B14:C14").Merge();
            sht.Range("B14").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B15").Value = Convert.ToString(ds.Tables[0].Rows[0]["Receiver_address"]);
            sht.Range("B15:F18").Style.Font.FontName = "Arial";
            sht.Range("B15:F18").Style.Font.FontSize = 12;
            sht.Range("B15:F18").Merge();
            sht.Range("B15:F18").Style.Alignment.WrapText = true;
            sht.Range("G14").Value = Convert.ToString(ds.Tables[0].Rows[0]["Consignee_dt"]);
            sht.Range("G14:H14").Style.Font.FontName = "Arial";
            sht.Range("G14:H14").Style.Font.FontSize = 12;
            sht.Range("G14:H14").Merge();
            sht.Range("G14:H14").Style.Alignment.WrapText = true;
            sht.Range("G14:H14").Style.Font.Bold = true;
            sht.Range("G14:H14").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("I14").Value = "1. PAYING AGENT";
            sht.Range("I14:M14").Style.Font.FontName = "Arial";
            sht.Range("I14:M14").Style.Font.FontSize = 12;
            sht.Range("I14:M14").Merge();
            sht.Range("I14").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("I15").Value = Convert.ToString(ds.Tables[0].Rows[0]["payingagent_address"]);
            sht.Range("I15:M18").Style.Font.FontName = "Arial";
            sht.Range("I15:M18").Style.Alignment.WrapText = true;
            sht.Range("I15:M18").Style.Font.FontSize = 11;
            sht.Range("I15:M18").Merge();
            sht.Range("I15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("N14").Value = "2. BUYER ( if other than consignee)";
            sht.Range("N14:Q14").Style.Font.FontName = "Arial";
            sht.Range("N14:Q14").Style.Font.FontSize = 12;
            sht.Range("N14:Q14").Merge();
            sht.Range("N15").Value = Convert.ToString(ds.Tables[0].Rows[0]["otherthanconsignee_address"]);
            sht.Range("N15:Q18").Style.Font.FontName = "Arial";
            sht.Range("N15:Q18").Style.Font.FontSize = 11;
            sht.Range("N15:Q18").Style.Alignment.WrapText = true;
            sht.Range("N15:Q18").Merge();
            sht.Range("N15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("B19").Value = "PRE-CARRIAGE BY";
            sht.Range("B19:D19").Style.Font.FontName = "Arial";
            sht.Range("B19:D19").Style.Font.FontSize = 12;
            sht.Range("B19:D19").Merge();
            sht.Range("B19:D19").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("B20").Value = Convert.ToString(ds.Tables[0].Rows[0]["precarriageby"]);
            sht.Range("B20:D21").Style.Font.FontName = "Arial";
            sht.Range("B20:D21").Style.Font.FontSize = 12;
            sht.Range("B20:D21").Merge();
            sht.Range("B20:D21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("B20:D21").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("E19").Value = "PLACE OF RECEIPT BY PRE-CARRIER";
            sht.Range("E19:H19").Style.Font.FontName = "Arial";
            sht.Range("E19:H19").Style.Font.FontSize = 11;
            sht.Range("E19:H19").Merge();
            sht.Range("E19:H19").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("E20").Value = Convert.ToString(ds.Tables[0].Rows[0]["placeofreceipt"]);
            sht.Range("E20:H21").Style.Font.FontName = "Arial";
            sht.Range("E20:H21").Style.Font.FontSize = 12;
            sht.Range("E20:H21").Merge();
            sht.Range("E20:H21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("E20:H21").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            sht.Range("I19").Value = "COUNTRY OF ORIGIN OF GOODS";
            sht.Range("I19:M19").Style.Font.FontName = "Arial";
            sht.Range("I19:M19").Style.Font.FontSize = 12;
            sht.Range("I19:M19").Merge();
            sht.Range("I19:M19").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("I20").Value = Convert.ToString(ds.Tables[0].Rows[0]["countryoforigin"]);
            sht.Range("I20:M21").Style.Font.FontName = "Arial";
            sht.Range("I20:M21").Style.Font.FontSize = 12;
            sht.Range("I20:M21").Merge();
            sht.Range("I20:M21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("I20:M21").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("N19").Value = "COUNTRY OF FINAL DESTINATION";
            sht.Range("N19:Q19").Style.Font.FontName = "Arial";
            sht.Range("N19:Q19").Style.Font.FontSize = 11;
            sht.Range("N19:Q19").Merge();
            sht.Range("N19:Q19").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("N20").Value = Convert.ToString(ds.Tables[0].Rows[0]["COUNTRY"]);
            sht.Range("N20:Q21").Style.Font.FontName = "Arial";
            sht.Range("N20:Q21").Style.Font.FontSize = 12;
            sht.Range("N20:Q21").Merge();
            sht.Range("N20:Q21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("N20:Q21").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);





            sht.Range("B22").Value = "VESSEL/FLIGHT NO.";
            sht.Range("B22:D22").Style.Font.FontName = "Arial";
            sht.Range("B22:D22").Style.Font.FontSize = 12;
            sht.Range("B22:D22").Merge();
            sht.Range("B22:D22").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("B23").Value = "";    //Convert.ToString(ds.Tables[0].Rows[0]["vesselno"]); 
            sht.Range("B23:D24").Style.Font.FontName = "Arial";
            sht.Range("B23:D24").Style.Font.FontSize = 12;
            sht.Range("B23:D24").Merge();
            sht.Range("B23:D24").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("B23:D24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("E22").Value = "PORT OF LOADING";
            sht.Range("E22:H22").Style.Font.FontName = "Arial";
            sht.Range("E22:H22").Style.Font.FontSize = 11;
            sht.Range("E22:H22").Merge();
            sht.Range("E22:H22").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("E23").Value = Convert.ToString(ds.Tables[0].Rows[0]["portofloading"]);
            sht.Range("E23:H24").Style.Font.FontName = "Arial";
            sht.Range("E23:H24").Style.Font.FontSize = 12;
            sht.Range("E23:H24").Merge();
            sht.Range("E23:H24").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("E23:H24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            sht.Range("B25").Value = "PORT OF DISCHARGE";
            sht.Range("B25:D25").Style.Font.FontName = "Arial";
            sht.Range("B25:D25").Style.Font.FontSize = 12;
            sht.Range("B25:D25").Merge();
            sht.Range("B25:D25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("B26").Value = Convert.ToString(ds.Tables[0].Rows[0]["portofdisc"]);
            sht.Range("B26:D27").Style.Font.FontName = "Arial";
            sht.Range("B26:D27").Style.Font.FontSize = 12;
            sht.Range("B26:D27").Merge();
            sht.Range("B26:D27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("B26:D27").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("E25").Value = "FINAL DESTINATION";
            sht.Range("E25:H25").Style.Font.FontName = "Arial";
            sht.Range("E25:H25").Style.Font.FontSize = 11;
            sht.Range("E25:H25").Merge();
            sht.Range("E25:H25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("E26").Value = Convert.ToString(ds.Tables[0].Rows[0]["COUNTRY"]);
            sht.Range("E26:H27").Style.Font.FontName = "Arial";
            sht.Range("E26:H27").Style.Font.FontSize = 12;
            sht.Range("E26:H27").Merge();
            sht.Range("E26:H27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("E26:H27").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            sht.Range("I23").Value = "Term of";
            sht.Range("I23:J23").Style.Font.FontName = "Arial";
            sht.Range("I23:J23").Style.Font.FontSize = 12;
            sht.Range("I23:J23").Merge();
            sht.Range("I23:J23").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("I23:J23").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            sht.Range("L23").Value = "PRICE ARE F.C.A INDIA PORT PAYMENT : D/P.";
            sht.Range("L23:P23").Style.Font.FontName = "Arial";
            sht.Range("L23:P23").Style.Font.FontSize = 12;
            sht.Range("L23:P23").Merge();
            sht.Range("L23:P23").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("L23:P23").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("I24").Value = "Delivery and Payment";
            sht.Range("I24:K24").Style.Font.FontName = "Arial";
            sht.Range("I24:K24").Style.Font.FontSize = 12;
            sht.Range("I24:JK4").Merge();
            sht.Range("I24:K24").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("I24:K24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("L24").Value = "Delivery Week";
            sht.Range("L24:M24").Style.Font.FontName = "Arial";
            sht.Range("L24:M24").Style.Font.FontSize = 12;
            sht.Range("L24:M24").Merge();
            sht.Range("L24:M24").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("L24:M24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("N24").Value = Convert.ToString(ds.Tables[0].Rows[0]["DLVWEEK"]);
            sht.Range("N24:P24").Style.Font.FontName = "Arial";
            sht.Range("N24:P24").Style.Font.FontSize = 12;
            sht.Range("N24:P24").Merge();
            sht.Range("N24:P24").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("N24:P24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);



            sht.Range("B28:D28").Value = "Marks & Nos.";
            sht.Range("B28:D28").Merge();
            sht.Range("B28", "D28").Style.Alignment.WrapText = true;
            sht.Range("B28", "D28").Style.Font.Bold = true;
            sht.Range("B28", "D28").Style.Font.FontSize = 11;
            sht.Range("B28", "D28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("B28:D28").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("B29:D29").Value = "Container No.";
            sht.Range("B29:D29").Merge();
            sht.Range("B29", "D29").Style.Alignment.WrapText = true;
            sht.Range("B29", "D29").Style.Font.Bold = true;
            sht.Range("B29", "D29").Style.Font.FontSize = 11;
            sht.Range("B29", "D29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("B29:D29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            sht.Range("E28").Value = "No. and kind of Packages";
            // sht.Range("B28:D29").Value = "Marks & Nos./Container No.";
            sht.Range("E28", "G29").Style.Alignment.WrapText = true;
            sht.Range("E28", "G29").Style.Font.Bold = true;
            sht.Range("E28", "G29").Style.Font.FontSize = 11;
            sht.Range("E28", "G29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("E28", "G29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("E28:G29").Merge();


            sht.Range("H28").Value = "Description of Goods";
            sht.Range("H28", "K29").Style.Alignment.WrapText = true;
            sht.Range("H28", "K29").Style.Font.Bold = true;
            sht.Range("H28", "K29").Style.Font.FontSize = 11;
            sht.Range("H28", "K29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("H28", "K29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("H28:K29").Merge();


            sht.Range("L28").Value = "QUANTITY";
            sht.Range("L28:M29").Merge();
            sht.Range("L28", "M29").Style.Alignment.WrapText = true;
            sht.Range("L28", "M29").Style.Font.Bold = true;
            sht.Range("L28", "M29").Style.Font.FontSize = 11;
            sht.Range("L28", "M29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("L28", "M29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            sht.Range("N28").Value = "Package#";
            sht.Range("N28:N29").Merge();
            sht.Range("N28").Style.Alignment.WrapText = true;
            sht.Range("N28").Style.Font.Bold = true;
            sht.Range("N28").Style.Font.FontSize = 11;
            sht.Range("N28:N29").Style.Font.FontName = "Arial";
            sht.Range("N28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("N28:N29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("O28").Value = "Art NO.";
            sht.Range("O28:O29").Merge();
            sht.Range("O28").Style.Alignment.WrapText = true;
            sht.Range("O28").Style.Font.Bold = true;
            sht.Range("O28").Style.Font.FontSize = 11;
            sht.Range("O28:O29").Style.Font.FontName = "Arial";
            sht.Range("O28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("O28:O29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("P28").Value = "Qty/Package";
            sht.Range("P28:P29").Merge();
            sht.Range("P28").Style.Alignment.WrapText = true;
            sht.Range("P28").Style.Font.Bold = true;
            sht.Range("P28").Style.Font.FontSize = 11;
            sht.Range("P28:P29").Style.Font.FontName = "Arial";
            sht.Range("P28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("P28:P29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("Q28").Value = "Total";
            sht.Range("Q28:Q29").Merge();
            sht.Range("Q28").Style.Alignment.WrapText = true;
            sht.Range("Q28").Style.Font.Bold = true;
            sht.Range("Q28").Style.Font.FontSize = 11;
            sht.Range("Q28:Q29").Style.Font.FontName = "Arial";
            sht.Range("Q28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("Q28:Q29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            //sht.Range("P28").Value = "AMOUNT-" + ds.Tables[0].Rows[0]["CURR"] + "-FCA";
            //sht.Range("P28:Q29").Merge();
            //sht.Range("P28").Style.Alignment.WrapText = true;
            //sht.Range("P28").Style.Font.Bold = true;
            //sht.Range("P28").Style.Font.FontSize = 11;
            //sht.Range("P28:Q29").Style.Font.FontName = "Arial";
            //sht.Range("P28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //sht.Range("P28:Q29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            sht.Range("B30").Value = "IKEA";
            sht.Range("B30:D30").Merge();
            sht.Range("B30", "D30").Style.Alignment.WrapText = true;
            sht.Range("B30", "D30").Style.Font.Bold = true;
            sht.Range("B30", "D30").Style.Font.FontSize = 11;
            sht.Range("B30", "D30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("B30", "D30").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            sht.Range("B31").Value = "1 TO " + sum + " PACKAGE";
            sht.Range("B31:D31").Merge();
            sht.Range("B31", "D31").Style.Alignment.WrapText = true;
            sht.Range("B31", "D31").Style.Font.Bold = true;
            sht.Range("B31", "D31").Style.Font.FontSize = 11;
            sht.Range("B31", "D31").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("E30").Value = "Description of Product";
            // sht.Range("B28:D29").Value = "Marks & Nos./Container No.";
            sht.Range("E30", "G31").Style.Alignment.WrapText = true;
            sht.Range("E30", "G31").Style.Font.Bold = true;
            sht.Range("E30", "G31").Style.Font.FontSize = 11;
            sht.Range("E30", "G31").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("E30", "G31").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("E30:G31").Merge();
            int row = 32;
            int pagerowcount = 15;
            StringBuilder STRBALE = new StringBuilder();
            decimal? totalamt = 0, totalpcs = 0, igst = 0, netwt = 0, wt = 0, cbm = 0;

            for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
            {

                wt += Convert.ToDecimal(ds.Tables[0].Rows[ii]["csmgrowei"]);
                netwt += Convert.ToDecimal(ds.Tables[0].Rows[ii]["csmgrownet"]);
                cbm += Convert.ToDecimal(ds.Tables[0].Rows[ii]["csmgroval"]);
                //double wt = ds.Tables[0].AsEnumerable().Sum(a => a.Field<double>("weight_roll"));
                //Int16 netwt = ds.Tables[0].AsEnumerable().Sum(a => a.Field<Int16>("Netwt"));
                //double cbm = ds.Tables[0].AsEnumerable().Sum(a => a.Field<double>("volume_roll"));
                sht.Range("B" + row).Value = Convert.ToString(ds.Tables[0].Rows[ii]["srno"]);
                sht.Range("B" + row + ":D" + (row)).Merge();
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.WrapText = true;
                sht.Range("B" + row + ":D" + (row)).Style.Font.FontSize = 10;
                sht.Range("B" + row + ":D" + (row)).Style.Font.Bold = true;
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("C" + row).Value = "Package";
                sht.Range("C" + row + ":D" + (row)).Merge();
                sht.Range("C" + row + ":D" + (row)).Style.Alignment.WrapText = true;
                sht.Range("C" + row + ":D" + (row)).Style.Font.FontSize = 10;
                sht.Range("C" + row + ":D" + (row)).Style.Font.Bold = true;
                sht.Range("C" + row + ":D" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("C" + row + ":D" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("E" + row).Value = Convert.ToString(ds.Tables[0].Rows[ii]["pkgno"]);
                sht.Range("E" + row + ":I" + (row)).Merge();
                sht.Range("E" + row + ":I" + (row)).Style.Alignment.WrapText = true;
                sht.Range("E" + row + ":I" + (row)).Style.Font.FontSize = 10;
                sht.Range("E" + row + ":I" + (row)).Style.Font.Bold = true;
                sht.Range("E" + row + ":I" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("E" + row + ":I" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("J" + row).Value = "PO#";
                sht.Range("J" + row + ":J" + (row)).Merge();
                sht.Range("J" + row + ":J" + (row)).Style.Alignment.WrapText = true;
                sht.Range("J" + row + ":J" + (row)).Style.Font.FontSize = 10;
                sht.Range("J" + row + ":J" + (row)).Style.Font.Bold = true;
                sht.Range("J" + row + ":J" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("J" + row + ":J" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                row += 1;
                sht.Range("C" + row).Value = Convert.ToString(ds.Tables[0].Rows[ii]["itemdesc"]);
                sht.Range("C" + row + ":I" + (row)).Merge();
                sht.Range("C" + row + ":I" + (row)).Style.Alignment.WrapText = true;
                sht.Range("C" + row + ":I" + (row)).Style.Font.FontSize = 10;
                sht.Range("C" + row + ":I" + (row)).Style.Font.Bold = true;
                sht.Range("C" + row + ":I" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("C" + row + ":I" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                sht.Range("J" + row).Value = Convert.ToString(ds.Tables[0].Rows[ii]["orderid"]);
                sht.Range("J" + row + ":K" + (row)).Merge();
                sht.Range("J" + row + ":K" + (row)).Style.Alignment.WrapText = true;
                sht.Range("J" + row + ":K" + (row)).Style.Font.FontSize = 10;
                sht.Range("J" + row + ":K" + (row)).Style.Font.Bold = true;
                sht.Range("J" + row + ":K" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("J" + row + ":K" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("L" + row).Value = Convert.ToString(ds.Tables[0].Rows[ii]["bkdqty"]) + " Pcs";
                sht.Range("L" + row + ":M" + (row)).Merge();
                sht.Range("L" + row + ":M" + (row)).Style.Alignment.WrapText = true;
                sht.Range("L" + row + ":M" + (row)).Style.Font.FontSize = 10;
                sht.Range("L" + row + ":M" + (row)).Style.Font.Bold = true;
                sht.Range("L" + row + ":M" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("L" + row + ":M" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                int iqty = 0;
                decimal rate = 0, totalval = 0;
                totalval = Convert.ToInt32(ds.Tables[0].Rows[ii]["bkdqty"]) * Convert.ToDecimal(ds.Tables[0].Rows[ii]["csmul"]);
                totalpcs += Convert.ToInt32(ds.Tables[0].Rows[ii]["bkdqty"]);
                sht.Range("N" + row).Value = Convert.ToString(ds.Tables[0].Rows[ii]["pkgno"]) + "   " + Convert.ToString(ds.Tables[0].Rows[ii]["articleno"]) + "   " + Convert.ToString(ds.Tables[0].Rows[ii]["pcs_roll"]) + "   " + Convert.ToString(ds.Tables[0].Rows[ii]["bkdqty"]);
                sht.Range("N" + row + ":Q" + (row)).Merge();
                sht.Range("N" + row + ":Q" + (row)).Style.Alignment.WrapText = true;
                sht.Range("N" + row + ":Q" + (row)).Style.Font.FontSize = 10;
                sht.Range("N" + row + ":Q" + (row)).Style.Font.Bold = true;
                sht.Range("N" + row + ":Q" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("N" + row + ":Q" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
               

                //sht.Range("p" + row).Value = totalval;
                //sht.Range("p" + row + ":Q" + (row)).Merge();
                //sht.Range("p" + row + ":Q" + (row)).Style.Alignment.WrapText = true;
                //sht.Range("p" + row + ":Q" + (row)).Style.Font.FontSize = 10;
                //sht.Range("p" + row + ":Q" + (row)).Style.Font.Bold = true;
                //sht.Range("p" + row + ":Q" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                //sht.Range("p" + row + ":Q" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                totalamt += totalval;
                row += 1;
                sht.Range("C" + row).Value = "Article NO." + Convert.ToString(ds.Tables[0].Rows[ii]["articleno"]);
                sht.Range("C" + row + ":G" + (row)).Merge();
                sht.Range("C" + row + ":G" + (row)).Style.Alignment.WrapText = true;
                sht.Range("C" + row + ":G" + (row)).Style.Font.FontSize = 10;
                sht.Range("C" + row + ":G" + (row)).Style.Font.Bold = true;
                sht.Range("C" + row + ":G" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("C" + row + ":G" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("J" + row).Value = "HS Code: " + Convert.ToString(ds.Tables[0].Rows[ii]["hsn"]);
                sht.Range("J" + row + ":K" + (row)).Merge();
                sht.Range("J" + row + ":K" + (row)).Style.Alignment.WrapText = true;
                sht.Range("J" + row + ":K" + (row)).Style.Font.FontSize = 10;
                sht.Range("J" + row + ":K" + (row)).Style.Font.Bold = true;
                sht.Range("J" + row + ":K" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("J" + row + ":K" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                row += 1;
                sht.Range("C" + row).Value = "Complete article description";
                sht.Range("C" + row + ":I" + (row)).Merge();
                sht.Range("C" + row + ":I" + (row)).Style.Alignment.WrapText = true;
                sht.Range("C" + row + ":I" + (row)).Style.Font.FontSize = 10;
                sht.Range("C" + row + ":I" + (row)).Style.Font.Bold = true;
                sht.Range("C" + row + ":I" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("C" + row + ":I" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                row += 1;
                sht.Range("C" + row).Value = Convert.ToString(ds.Tables[0].Rows[ii]["contents"]);
                sht.Range("C" + row + ":I" + (row)).Merge();
                sht.Range("C" + row + ":I" + (row)).Style.Alignment.WrapText = true;
                sht.Range("C" + row + ":I" + (row)).Style.Font.FontSize = 10;
                sht.Range("C" + row + ":I" + (row)).Style.Font.Bold = true;
                sht.Range("C" + row + ":I" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("C" + row + ":I" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                row += 1;
                sht.Range("C" + row).Value = "Package:" + Convert.ToString(ds.Tables[0].Rows[ii]["pcs_roll"]) + " Pcs";
                sht.Range("C" + row + ":D" + (row)).Merge();
                sht.Range("C" + row + ":D" + (row)).Style.Alignment.WrapText = true;
                sht.Range("C" + row + ":D" + (row)).Style.Font.FontSize = 10;
                sht.Range("C" + row + ":D" + (row)).Style.Font.Bold = true;
                sht.Range("C" + row + ":D" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("C" + row + ":D" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("F" + row).Value = "Net wt:" + Convert.ToString(ds.Tables[0].Rows[ii]["Netwt"]) + " Kg";
                sht.Range("F" + row + ":G" + (row)).Merge();
                sht.Range("F" + row + ":G" + (row)).Style.Alignment.WrapText = true;
                sht.Range("F" + row + ":G" + (row)).Style.Font.FontSize = 10;
                sht.Range("F" + row + ":G" + (row)).Style.Font.Bold = true;
                sht.Range("F" + row + ":G" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("F" + row + ":G" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("H" + row).Value = "Gross wt:" + Convert.ToString(ds.Tables[0].Rows[ii]["Weight_roll"]) + " Kg";
                sht.Range("H" + row + ":I" + (row)).Merge();
                sht.Range("H" + row + ":I" + (row)).Style.Alignment.WrapText = true;
                sht.Range("H" + row + ":I" + (row)).Style.Font.FontSize = 10;
                sht.Range("H" + row + ":I" + (row)).Style.Font.Bold = true;
                sht.Range("H" + row + ":I" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("H" + row + ":I" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);        


                row += 2;
                sht.Range("B" + (row - 8) + ":B" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("Q" + (row - 8) + ":Q" + (row)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("L" + (row - 8) + ":L" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("N" + (row - 8) + ":N" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
               // sht.Range("P" + (row - 8) + ":P" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

            }
            if (row < 46)
            {
                int lesslastrow = row;
                row += (45 - row);
                sht.Range("B" + (lesslastrow) + ":B" + (row + pagerowcount)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("Q" + (lesslastrow) + ":Q" + (row + pagerowcount)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("L" + (lesslastrow) + ":L" + (row + pagerowcount)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("N" + (lesslastrow) + ":N" + (row + pagerowcount)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
             //   sht.Range("P" + (lesslastrow) + ":P" + (row + pagerowcount)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

            }
            else
            {
                int lastrow = row - 1;

                row += pagerowcount - 1;
                sht.Range("B" + (lastrow) + ":B" + (lastrow + pagerowcount)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("Q" + (lastrow) + ":Q" + (lastrow + pagerowcount)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("L" + (lastrow) + ":L" + (lastrow + pagerowcount)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("N" + (lastrow) + ":N" + (lastrow + pagerowcount)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
               // sht.Range("P" + (lastrow) + ":P" + (lastrow + pagerowcount)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;


            }

            sht.Range("B" + (row) + ":Q" + (row)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            row += 1;
            sht.Range("B" + row).Value = "";
            sht.Range("B" + row + ":D" + (row)).Merge();
            sht.Range("B" + row + ":D" + (row)).Style.Alignment.WrapText = true;
            sht.Range("B" + row + ":D" + (row)).Style.Font.FontSize = 10;
            sht.Range("B" + row + ":D" + (row)).Style.Font.Bold = true;
            sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + (row) + ":Q" + (row)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("B" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("Q" + (row)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("E" + row).Value = "";
            sht.Range("E" + row + ":G" + (row)).Merge();
            sht.Range("E" + row + ":G" + (row)).Style.Alignment.WrapText = true;
            sht.Range("E" + row + ":G" + (row)).Style.Font.FontSize = 10;
            sht.Range("E" + row + ":G" + (row)).Style.Font.Bold = true;
            sht.Range("E" + row + ":G" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("E" + row + ":G" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            sht.Range("L" + row).Value = totalpcs + " Pcs";
            sht.Range("L" + row + ":M" + (row)).Merge();
            sht.Range("L" + row + ":M" + (row)).Style.Alignment.WrapText = true;
            sht.Range("L" + row + ":M" + (row)).Style.Font.FontSize = 10;
            sht.Range("L" + row + ":M" + (row)).Style.Font.Bold = true;
            sht.Range("L" + row + ":M" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("L" + row + ":M" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("N" + row).Value = "";
            sht.Range("N" + row + ":O" + (row)).Merge();
            sht.Range("N" + row + ":O" + (row)).Style.Alignment.WrapText = true;
            sht.Range("N" + row + ":O" + (row)).Style.Font.FontSize = 10;
            sht.Range("N" + row + ":O" + (row)).Style.Font.Bold = true;
            sht.Range("N" + row + ":O" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("N" + row + ":O" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            sht.Range("P" + row).Value = "";
            sht.Range("P" + row + ":Q" + (row)).Merge();
            sht.Range("P" + row + ":Q" + (row)).Style.Alignment.WrapText = true;
            sht.Range("P" + row + ":Q" + (row)).Style.Font.FontSize = 10;
            sht.Range("P" + row + ":Q" + (row)).Style.Font.Bold = true;
            sht.Range("P" + row + ":Q" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("P" + row + ":Q" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("L" + (row) + ":L" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("N" + (row) + ":N" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("P" + (row) + ":P" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("L" + (row) + ":Q" + (row)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //  sht.Range("B" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("Q" + (row)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            row += 1;
            sht.Range("L" + (row) + ":L" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("N" + (row) + ":N" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("P" + (row) + ":P" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("L" + (row) + ":Q" + (row)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //  sht.Range("B" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("Q" + (row)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            string amountinwords = "";
            string amt = totalamt.ToString();
            string val = "", paise = "";

            if (amt.IndexOf('.') > 0)
            {
                val = amt.ToString().Split('.')[0];
                amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val));
            }
            else
            {
                amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(totalamt));
            }

            string Pointamt = string.Format("{0:0.00}", Convert.ToString(totalamt));
            val = "";
            if (Pointamt.IndexOf('.') > 0)
            {
                val = Pointamt.ToString().Split('.')[1];
                if (Convert.ToInt32(val) > 0)
                {
                    paise = ds.Tables[0].Rows[0]["CURR"] + " " + ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val)) + " ";
                }
            }
            amountinwords = "";
            sht.Range("B" + row).Value = amountinwords.ToUpper();
            sht.Range("B" + row + ":K" + (row)).Merge();
            sht.Range("B" + row + ":K" + (row)).Style.Alignment.WrapText = true;
            sht.Range("B" + row + ":K" + (row)).Style.Font.FontSize = 10;
            sht.Range("B" + row + ":K" + (row)).Style.Font.Bold = true;
            sht.Range("B" + row + ":K" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("B" + row + ":K" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("B" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("Q" + (row)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            row += 1;
            sht.Range("B" + row).Value = "Total Package/ Carton";
            sht.Range("B" + row + ":D" + (row)).Merge();
            sht.Range("B" + row + ":D" + (row)).Style.Alignment.WrapText = true;
            sht.Range("B" + row + ":D" + (row)).Style.Font.FontSize = 10;
            sht.Range("B" + row + ":D" + (row)).Style.Font.Bold = true;
            sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            sht.Range("E" + row).Value = sum.ToString();
          //  sht.Range("E" + row).Style.NumberFormat.Format = "###0.00";
            sht.Range("E" + row + ":F" + (row)).Merge();
            sht.Range("E" + row + ":F" + (row)).Style.Alignment.WrapText = true;
            sht.Range("E" + row + ":F" + (row)).Style.Font.FontSize = 10;
            sht.Range("E" + row + ":F" + (row)).Style.Font.Bold = true;
            sht.Range("E" + row + ":F" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("E" + row + ":F" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            row += 1;
            sht.Range("B" + row).Value = "Total Gross weight";
            sht.Range("B" + row + ":D" + (row)).Merge();
            sht.Range("B" + row + ":D" + (row)).Style.Alignment.WrapText = true;
            sht.Range("B" + row + ":D" + (row)).Style.Font.FontSize = 10;
            sht.Range("B" + row + ":D" + (row)).Style.Font.Bold = true;
            sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            //sht.Range("B" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //sht.Range("Q" + (row)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("E" + row).Value = wt + "    " + "KGS";
            sht.Range("E" + row).Style.NumberFormat.Format = "###0.00";
            sht.Range("E" + row + ":F" + (row)).Merge();
            sht.Range("E" + row + ":F" + (row)).Style.Alignment.WrapText = true;
            sht.Range("E" + row + ":F" + (row)).Style.Font.FontSize = 10;
            sht.Range("E" + row + ":F" + (row)).Style.Font.Bold = true;


            sht.Range("E" + row + ":F" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("E" + row + ":F" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            row += 1;
            sht.Range("B" + row).Value = "Total Net weight";
            sht.Range("B" + row + ":D" + (row)).Merge();
            sht.Range("B" + row + ":D" + (row)).Style.Alignment.WrapText = true;
            sht.Range("B" + row + ":D" + (row)).Style.Font.FontSize = 10;
            sht.Range("B" + row + ":D" + (row)).Style.Font.Bold = true;
            sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            sht.Range("E" + row).Value = netwt + "    " + "KGS";
            sht.Range("E" + row).Style.NumberFormat.Format = "###0.00";
            sht.Range("E" + row + ":F" + (row)).Merge();
            sht.Range("E" + row + ":F" + (row)).Style.Alignment.WrapText = true;
            sht.Range("E" + row + ":F" + (row)).Style.Font.FontSize = 10;
            sht.Range("E" + row + ":F" + (row)).Style.Font.Bold = true;
            //sht.Range("B" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //sht.Range("Q" + (row)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            row += 1;
            sht.Range("B" + row).Value = "Total Vol.";
            sht.Range("B" + row + ":D" + (row)).Merge();
            sht.Range("B" + row + ":D" + (row)).Style.Alignment.WrapText = true;
            sht.Range("B" + row + ":D" + (row)).Style.Font.FontSize = 10;
            sht.Range("B" + row + ":D" + (row)).Style.Font.Bold = true;
            sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            sht.Range("E" + row).Value = cbm + "    " + "CBM";
            sht.Range("E" + row).Style.NumberFormat.Format = "###0.00";
            sht.Range("E" + row + ":F" + (row)).Merge();
            sht.Range("E" + row + ":F" + (row)).Style.Alignment.WrapText = true;
            sht.Range("E" + row + ":F" + (row)).Style.Font.FontSize = 10;
            sht.Range("E" + row + ":F" + (row)).Style.Font.Bold = true;
            if (chkrex.Checked)
            {

                sht.Range("I" + row).Value = "REX NO. INREX0610004182EC026";
                sht.Range("I" + row + ":K" + (row)).Merge();
                sht.Range("I" + row + ":K" + (row)).Style.Alignment.WrapText = true;
                sht.Range("I" + row + ":K" + (row)).Style.Font.FontSize = 10;
                sht.Range("I" + row + ":K" + (row)).Style.Font.Bold = true;
            }




            row += 1;
            //sht.Range("B" + row).Value = "Certified that";
            //sht.Range("B" + row + ":D" + (row)).Merge();
            //sht.Range("B" + row + ":D" + (row)).Style.Alignment.WrapText = true;
            //sht.Range("B" + row + ":D" + (row)).Style.Font.FontSize = 8;
            //sht.Range("B" + row + ":D" + (row)).Style.Font.Bold = true;
            //sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //sht.Range("B" + row + ":D" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            row += 1;
            //sht.Range("B" + row).Value = "1. Manufacturers are themselves Exporters of Rugs and Carpets in India and having Import - Export Code No. 0610004182 and RBI Code No. -6392040-2600009";
            //sht.Range("B" + row + ":K" + (row + 1)).Merge();
            //sht.Range("B" + row + ":K" + (row + 1)).Style.Alignment.WrapText = true;
            //sht.Range("B" + row + ":K" + (row + 1)).Style.Font.FontSize = 8;
            //sht.Range("B" + row + ":K" + (row + 1)).Style.Font.Bold = true;
            //sht.Range("B" + row + ":K" + (row + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //sht.Range("B" + row + ":K" + (row + 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("M" + row).Value = "Signature";
            sht.Range("M" + row + ":N" + (row)).Merge();
            sht.Range("M" + row + ":N" + (row)).Style.Alignment.WrapText = true;
            sht.Range("M" + row + ":N" + (row)).Style.Font.FontSize = 12;
            sht.Range("M" + row + ":N" + (row)).Style.Font.Bold = true;
            sht.Range("M" + row + ":N" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("M" + row + ":N" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("P" + row).Value = "Diamond Exports";
            sht.Range("P" + row + ":Q" + (row)).Merge();
            sht.Range("P" + row + ":Q" + (row)).Style.Alignment.WrapText = true;
            sht.Range("P" + row + ":Q" + (row)).Style.Font.FontSize = 12;
            sht.Range("P" + row + ":Q" + (row)).Style.Font.Bold = true;
            sht.Range("P" + row + ":Q" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("P" + row + ":Q" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            sht.Range("M" + (row) + ":Q" + (row)).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            row += 2;
            //sht.Range("B" + row).Value = "2. Goods Exported are of 'Indian Origin' and Made in India.";
            //sht.Range("B" + row + ":K" + (row)).Merge();
            //sht.Range("B" + row + ":K" + (row)).Style.Alignment.WrapText = true;
            //sht.Range("B" + row + ":K" + (row)).Style.Font.FontSize = 8;
            //sht.Range("B" + row + ":K" + (row)).Style.Font.Bold = true;
            //sht.Range("B" + row + ":K" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //sht.Range("B" + row + ":K" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            row += 1;
            //sht.Range("B" + row).Value = "3. The Goods are factory produced";
            //sht.Range("B" + row + ":K" + (row)).Merge();
            //sht.Range("B" + row + ":K" + (row)).Style.Alignment.WrapText = true;
            //sht.Range("B" + row + ":K" + (row)).Style.Font.FontSize = 8;
            //sht.Range("B" + row + ":K" + (row)).Style.Font.Bold = true;
            //sht.Range("B" + row + ":K" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //sht.Range("B" + row + ":K" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            row += 1;
            //sht.Range("B" + row).Value = "DECLARATION: WE DECLARE THAT THIS INVOICE SHOWS THE ACTUAL PRICE OF THE GOODS DESCRIBED AND THAT ALL PARTICULARS ARE TRUE AND CORRECT";
            //sht.Range("B" + row + ":K" + (row + 1)).Merge();
            //sht.Range("B" + row + ":K" + (row + 1)).Style.Alignment.WrapText = true;
            //sht.Range("B" + row + ":K" + (row + 1)).Style.Font.FontSize = 8;
            //sht.Range("B" + row + ":K" + (row + 1)).Style.Font.Bold = true;
            //sht.Range("B" + row + ":K" + (row + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //sht.Range("B" + row + ":K" + (row + 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            if (chkrex.Checked)
            {
                row += 2;
                sht.Range("B" + row).Value = "STATEMENT OF ORIGIN";
                sht.Range("B" + row + ":K" + (row)).Merge();
                sht.Range("B" + row + ":K" + (row)).Style.Alignment.WrapText = true;
                sht.Range("B" + row + ":K" + (row)).Style.Font.FontSize = 8;
                sht.Range("B" + row + ":K" + (row)).Style.Font.Bold = true;
                sht.Range("B" + row + ":K" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B" + row + ":K" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                row += 1;
                sht.Range("B" + row).Value = "THE EXPORTER INREX0610004182EC026 OF THE PRODUCTS COVERED BY THIS DOCUMENT DECLARES THAT, EXCEPT WHERE OTHERWISE CLEARLY INDICATED, THESE PRODUCTS ARE OF INDIA PREFERENTIAL ORIGIN ACCORDING TO RULES OF ORIGIN OF THE GENERALISED SYSTEM OF PREFERENCES OF THE EUROPEAN UNION AND THAT THE ORIGIN CRITERION MET IS 'P'.";
                sht.Range("B" + row + ":K" + (row + 2)).Merge();
                sht.Range("B" + row + ":K" + (row + 2)).Style.Alignment.WrapText = true;
                sht.Range("B" + row + ":K" + (row + 2)).Style.Font.FontSize = 8;
                sht.Range("B" + row + ":K" + (row + 2)).Style.Font.Bold = true;
                sht.Range("B" + row + ":K" + (row + 2)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B" + row + ":K" + (row + 2)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            }
            sht.Range("N" + row).Value = "Authorised Signatory";
            sht.Range("N" + row + ":O" + (row)).Merge();
            sht.Range("N" + row + ":O" + (row)).Style.Alignment.WrapText = true;
            sht.Range("N" + row + ":O" + (row)).Style.Font.FontSize = 12;
            sht.Range("N" + row + ":O" + (row)).Style.Font.Bold = true;
            sht.Range("N" + row + ":O" + (row)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("N" + row + ":O" + (row)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            row += 1;
            if (chkrex.Checked)
            {
                sht.Range("B" + (row - 13) + ":B" + (row + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("Q" + (row - 13) + ":Q" + (row + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("M" + (row - 8) + ":M" + (row + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("B" + (row + 1) + ":Q" + (row + 1)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            else
            {
                sht.Range("B" + (row - 12) + ":B" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("Q" + (row - 12) + ":Q" + (row)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("M" + (row - 5) + ":M" + (row)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("B" + (row) + ":Q" + (row)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;


            }
            string Fileextension = "xlsx";
            string filename = "PackingList." + Fileextension + "";
            Path = Server.MapPath("~/InvoiceExcel/" + filename);
            xapp.SaveAs(Path);

            xapp.Dispose();


            //Download File
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            //   Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            Response.End();

        }
        catch (Exception ex)
        {
        }


    }
    protected void btnprint_Click(object sender, EventArgs e)
    {
        if (!Directory.Exists(Server.MapPath("~/InvoiceExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/InvoiceExcel/"));
        }

        try
        {
            if (chkdevac.Checked)
            {
                string Path = "";
                string Pathpdf = "";
                string str = "";

                str = @"select ROW_NUMBER() over(order by(select 1)) srno,pi.*,dm.*,pa.* from preinvoice pi join Destinationmaster dm  on pi.desccode=dm.Destcode left join Packingarticle pa on pi.articleno=pa.ArticleNo where pi.invoiceno='" + txtinvoiceno.Text + "' order by dm.Id";


                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                var xapp = new XLWorkbook();
                List<preinvoicemodel> plist = new List<preinvoicemodel>();
                int sum = ds.Tables[0].AsEnumerable().Sum(a => a.Field<int>("csmul"));
                int countsr = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    for (int i = 1; i <= Convert.ToInt32(dr["csmul"]); i++)
                    {
                        countsr++;
                        plist.Add(new preinvoicemodel { artileno = Convert.ToString(dr["articleno"]), orderid = Convert.ToString(dr["orderid"]), srno = countsr, dlweek = Convert.ToString(dr["dlvweek"]), csmul = Convert.ToInt32(dr["orderid"]), invoiceno = Convert.ToString(dr["invoiceno"]), bkdqty = sum, portofloading = Convert.ToString(dr["portofloading"]), portofdischarge = Convert.ToString(dr["portofdisc"]), destcode = Convert.ToString(dr["desccode"]), csmgrowei = Convert.ToInt32(dr["weight_roll"]), csmgrownet = Convert.ToInt32(dr["netwt"]), csmno = Convert.ToString(dr["csmno"]), size = Convert.ToString(dr["palletsize"]), pcs = Convert.ToInt32(dr["pcs_roll"]) });
                    }

                }

                //************set cell width
                //Page
                int countsheet = 1;
                var sht = xapp.Worksheets.Add("DAVAC");
                sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                //  sht.PageSetup.AdjustTo(30);
                // sht.PageSetup.FitToPages(1, 1);
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                sht.PageSetup.VerticalDpi = 100;
                sht.PageSetup.HorizontalDpi = 100;
                sht.PageSetup.Margins.Top = 0.0;
                sht.PageSetup.Margins.Bottom = 0.0;
                sht.PageSetup.Margins.Right = 0.0;
                sht.PageSetup.Margins.Left = 0.0;
                int row = 5;
                int roweven = 5;
                foreach (var lstdr in plist.OrderBy(a => a.orderid))
                {
                    if (countsheet % 2 != 0)
                    {
                        //sht.Column("N").Width = 12.89;
                        //sht.Column("B").Width = 13.89;
                        //sht.Column("O").Width = 10.89;
                        //sht.Column("K").Width = 10.89;
                        if (countsheet > 2)
                        { row += 5; }
                        sht.Style.Font.FontName = "Arial";
                        using (var range = sht.Range("B" + row + ":G" + row))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }
                        sht.Range("B" + row).Value = "DAVAC / MULTIPACK LABEL";
                        sht.Range("B" + row + ":G" + row).Merge();
                        sht.Range("B" + row, "G" + row).Style.Alignment.WrapText = true;
                        sht.Range("B" + row, "G" + row).Style.Font.Bold = true;
                        sht.Range("B" + row, "G" + row).Style.Font.FontSize = 11;
                        sht.Range("B" + row, "G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + row, "G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        row = row + 1;
                        using (var range = sht.Range("B" + row + ":G" + row))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("B" + row).Value = "IKEA";
                        sht.Range("B" + row + ":D" + row).Merge();
                        sht.Range("B" + row, "D" + row).Style.Alignment.WrapText = true;
                        sht.Range("B" + row, "D" + row).Style.Font.Bold = true;
                        sht.Range("B" + row, "D" + row).Style.Font.FontSize = 11;
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("E" + row).Value = lstdr.destcode;
                        sht.Range("E" + row + ":G" + row).Merge();
                        sht.Range("E" + row, "G" + row).Style.Alignment.WrapText = true;
                        sht.Range("E" + row, "G" + row).Style.Font.Bold = true;
                        sht.Range("E" + row, "G" + row).Style.Font.FontSize = 11;
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        row = row + 1;
                        using (var range = sht.Range("B" + row + ":G" + row))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("B" + row).Value = "PORT OF LOADING";
                        sht.Range("B" + row + ":D" + row).Merge();
                        sht.Range("B" + row, "D" + row).Style.Alignment.WrapText = true;
                        sht.Range("B" + row, "D" + row).Style.Font.Bold = true;
                        sht.Range("B" + row, "D" + row).Style.Font.FontSize = 11;
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("E" + row).Value = lstdr.portofloading;
                        sht.Range("E" + row + ":G" + row).Merge();
                        sht.Range("E" + row, "G" + row).Style.Alignment.WrapText = true;
                        sht.Range("E" + row, "G" + row).Style.Font.Bold = true;
                        sht.Range("E" + row, "G" + row).Style.Font.FontSize = 11;
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        row = row + 1;
                        using (var range = sht.Range("B" + row + ":G" + row))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("B" + row).Value = "PORT OF DISCHARGE";
                        sht.Range("B" + row + ":D" + row).Merge();
                        sht.Range("B" + row, "D" + row).Style.Alignment.WrapText = true;
                        sht.Range("B" + row, "D" + row).Style.Font.Bold = true;
                        sht.Range("B" + row, "D" + row).Style.Font.FontSize = 11;
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("E" + row).Value = lstdr.portofdischarge;
                        sht.Range("E" + row + ":G" + row).Merge();
                        sht.Range("E" + row, "G" + row).Style.Alignment.WrapText = true;
                        sht.Range("E" + row, "G" + row).Style.Font.Bold = true;
                        sht.Range("E" + row, "G" + row).Style.Font.FontSize = 11;
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        row = row + 1;
                        using (var range = sht.Range("B" + row + ":G" + row))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("B" + row).Value = "SUPPLIER NO.";
                        sht.Range("B" + row + ":D" + row).Merge();
                        sht.Range("B" + row, "D" + row).Style.Alignment.WrapText = true;
                        sht.Range("B" + row, "D" + row).Style.Font.Bold = true;
                        sht.Range("B" + row, "D" + row).Style.Font.FontSize = 11;
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("E" + row).Value = "23130";
                        sht.Range("E" + row + ":G" + row).Merge();
                        sht.Range("E" + row, "G" + row).Style.Alignment.WrapText = true;
                        sht.Range("E" + row, "G" + row).Style.Font.Bold = true;
                        sht.Range("E" + row, "G" + row).Style.Font.FontSize = 11;
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        row = row + 1;
                        using (var range = sht.Range("B" + row + ":G" + row))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                       
                        sht.Range("B" + row).Value = "ORDER NO.";
                        sht.Range("B" + row + ":D" + row).Merge();
                        sht.Range("B" + row, "D" + row).Style.Alignment.WrapText = true;
                        sht.Range("B" + row, "D" + row).Style.Font.Bold = true;
                        sht.Range("B" + row, "D" + row).Style.Font.FontSize = 11;
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("E" + row).Value = lstdr.orderid;
                        sht.Range("E" + row + ":G" + row).Merge();
                        sht.Range("E" + row, "G" + row).Style.Alignment.WrapText = true;
                        sht.Range("E" + row, "G" + row).Style.Font.Bold = true;
                        sht.Range("E" + row, "G" + row).Style.Font.FontSize = 11;
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        row = row + 1;
                        using (var range = sht.Range("B" + row + ":G" + row))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("B" + row).Value = "ARTICLE NO.";
                        sht.Range("B" + row + ":D" + row).Merge();
                        sht.Range("B" + row, "D" + row).Style.Alignment.WrapText = true;
                        sht.Range("B" + row, "D" + row).Style.Font.Bold = true;
                        sht.Range("B" + row, "D" + row).Style.Font.FontSize = 11;
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("E" + row).Value = lstdr.artileno;
                        sht.Range("E" + row + ":G" + row).Merge();
                        sht.Range("E" + row, "G" + row).Style.Alignment.WrapText = true;
                        sht.Range("E" + row, "G" + row).Style.Font.Bold = true;
                        sht.Range("E" + row, "G" + row).Style.Font.FontSize = 11;
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        row = row + 1;
                        using (var range = sht.Range("B" + row + ":G" + row))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("B" + row).Value = "SIZE";
                        sht.Range("B" + row + ":D" + row).Merge();
                        sht.Range("B" + row, "D" + row).Style.Alignment.WrapText = true;
                        sht.Range("B" + row, "D" + row).Style.Font.Bold = true;
                        sht.Range("B" + row, "D" + row).Style.Font.FontSize = 11;
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("E" + row).Value = lstdr.size;
                        sht.Range("E" + row + ":G" + row).Merge();
                        sht.Range("E" + row, "G" + row).Style.Alignment.WrapText = true;
                        sht.Range("E" + row, "G" + row).Style.Font.Bold = true;
                        sht.Range("E" + row, "G" + row).Style.Font.FontSize = 11;
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        row = row + 1;
                        using (var range = sht.Range("B" + row + ":G" + row))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("B" + row).Value = "PACKAGES NO.";
                        sht.Range("B" + row + ":D" + row).Merge();
                        sht.Range("B" + row, "D" + row).Style.Alignment.WrapText = true;
                        sht.Range("B" + row, "D" + row).Style.Font.Bold = true;
                        sht.Range("B" + row, "D" + row).Style.Font.FontSize = 11;
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("E" + row).Value = "'" + countsheet + "/" + sum;
                        sht.Range("E" + row + ":G" + row).Merge();
                        sht.Range("E" + row, "G" + row).Style.Alignment.WrapText = true;
                        sht.Range("E" + row, "G" + row).Style.Font.Bold = true;
                        sht.Range("E" + row, "G" + row).Style.Font.FontSize = 11;
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        row = row + 1;
                        using (var range = sht.Range("B" + row + ":G" + row))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("B" + row).Value = "NO.PCS / Packages";
                        sht.Range("B" + row + ":D" + row).Merge();
                        sht.Range("B" + row, "D" + row).Style.Alignment.WrapText = true;
                        sht.Range("B" + row, "D" + row).Style.Font.Bold = true;
                        sht.Range("B" + row, "D" + row).Style.Font.FontSize = 11;
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("E" + row).Value = lstdr.pcs;
                        sht.Range("E" + row + ":G" + row).Merge();
                        sht.Range("E" + row, "G" + row).Style.Alignment.WrapText = true;
                        sht.Range("E" + row, "G" + row).Style.Font.Bold = true;
                        sht.Range("E" + row, "G" + row).Style.Font.FontSize = 11;
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        row = row + 1;
                        using (var range = sht.Range("B" + row + ":G" + row))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("B" + row).Value = "GROSS WT.";
                        sht.Range("B" + row + ":D" + row).Merge();
                        sht.Range("B" + row, "D" + row).Style.Alignment.WrapText = true;
                        sht.Range("B" + row, "D" + row).Style.Font.Bold = true;
                        sht.Range("B" + row, "D" + row).Style.Font.FontSize = 11;
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("E" + row).Value = lstdr.csmgrowei;
                        sht.Range("E" + row + ":G" + row).Merge();
                        sht.Range("E" + row, "G" + row).Style.Alignment.WrapText = true;
                        sht.Range("E" + row, "G" + row).Style.Font.Bold = true;
                        sht.Range("E" + row, "G" + row).Style.Font.FontSize = 11;
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        row = row + 1;
                        using (var range = sht.Range("B" + row + ":G" + row))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("B" + row).Value = "NET WT.";
                        sht.Range("B" + row + ":D" + row).Merge();
                        sht.Range("B" + row, "D" + row).Style.Alignment.WrapText = true;
                        sht.Range("B" + row, "D" + row).Style.Font.Bold = true;
                        sht.Range("B" + row, "D" + row).Style.Font.FontSize = 11;
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("E" + row).Value = lstdr.csmgrownet;
                        sht.Range("E" + row + ":G" + row).Merge();
                        sht.Range("E" + row, "G" + row).Style.Alignment.WrapText = true;
                        sht.Range("E" + row, "G" + row).Style.Font.Bold = true;
                        sht.Range("E" + row, "G" + row).Style.Font.FontSize = 11;
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        row = row + 1;
                        using (var range = sht.Range("B" + row + ":G" + row))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("B" + row).Value = "DATE";
                        sht.Range("B" + row + ":D" + row).Merge();
                        sht.Range("B" + row, "D" + row).Style.Alignment.WrapText = true;
                        sht.Range("B" + row, "D" + row).Style.Font.Bold = true;
                        sht.Range("B" + row, "D" + row).Style.Font.FontSize = 11;
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("E" + row).Value = lstdr.dlweek;
                        sht.Range("E" + row + ":G" + row).Merge();
                        sht.Range("E" + row, "G" + row).Style.Alignment.WrapText = true;
                        sht.Range("E" + row, "G" + row).Style.Font.Bold = true;
                        sht.Range("E" + row, "G" + row).Style.Font.FontSize = 11;
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        row = row + 1;
                        using (var range = sht.Range("B" + row + ":G" + row))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("B" + row).Value = "INVOICE NO.";
                        sht.Range("B" + row + ":D" + row).Merge();
                        sht.Range("B" + row, "D" + row).Style.Alignment.WrapText = true;
                        sht.Range("B" + row, "D" + row).Style.Font.Bold = true;
                        sht.Range("B" + row, "D" + row).Style.Font.FontSize = 11;
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("E" + row).Value = lstdr.invoiceno;
                        sht.Range("E" + row + ":G" + row).Merge();
                        sht.Range("E" + row, "G" + row).Style.Alignment.WrapText = true;
                        sht.Range("E" + row, "G" + row).Style.Font.Bold = true;
                        sht.Range("E" + row, "G" + row).Style.Font.FontSize = 11;
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        row = row + 1;
                        using (var range = sht.Range("B" + row + ":G" + row))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("B" + row).Value = "CONSIGNMENT NO.";
                        sht.Range("B" + row + ":D" + row).Merge();
                        sht.Range("B" + row, "D" + row).Style.Alignment.WrapText = true;
                        sht.Range("B" + row, "D" + row).Style.Font.Bold = true;
                        sht.Range("B" + row, "D" + row).Style.Font.FontSize = 11;
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + row, "D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("E" + row).Value = lstdr.csmno;
                        sht.Range("E" + row + ":G" + row).Merge();
                        sht.Range("E" + row, "G" + row).Style.Alignment.WrapText = true;
                        sht.Range("E" + row, "G" + row).Style.Font.Bold = true;
                        sht.Range("E" + row, "G" + row).Style.Font.FontSize = 11;
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("E" + row, "G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                        using (var range = sht.Range("B" + row + ":G" + row))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }
                        row += 1;
                        using (var range = sht.Range("B" + row + ":G" + row))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }
                        sht.Range("B" + row).Value = "MADE IN INDIA";
                        sht.Range("B" + row + ":G" + row).Merge();
                        sht.Range("B" + row, "G" + row).Style.Alignment.WrapText = true;
                        sht.Range("B" + row, "G" + row).Style.Font.Bold = true;
                        sht.Range("B" + row, "G" + row).Style.Font.FontSize = 11;
                        sht.Range("B" + row, "G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + row, "G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        //sht.Range("E" + row).Value = "23130";
                        //sht.Range("E" + row + ":G" + row).Merge();
                        //sht.Range("E" + row, "G" + row).Style.Alignment.WrapText = true;
                        //sht.Range("E" + row, "G" + row).Style.Font.Bold = true;
                        //sht.Range("E" + row, "G" + row).Style.Font.FontSize = 11;
                        //sht.Range("E" + row, "G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        //sht.Range("E" + row, "G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        //row = row + 1;
                        //using (var range = sht.Range("B" + row + ":G" + row))
                        //{
                        //    range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                        //    range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                        //    range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                        //    range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        //}


                    }

                    if (countsheet % 2 == 0)
                    {
                        //sht.Column("N").Width = 12.89;
                        //sht.Column("B").Width = 13.89;
                        //sht.Column("O").Width = 10.89;
                        //sht.Column("K").Width = 10.89;
                        if (countsheet > 2)
                        { roweven += 5; }
                        sht.Style.Font.FontName = "Arial";
                        using (var range = sht.Range("J" + roweven + ":O" + roweven))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }
                        sht.Range("J" + roweven).Value = "DAVAC / MULTIPACK LABEL";
                        sht.Range("J" + roweven + ":O" + roweven).Merge();
                        sht.Range("J" + roweven, "O" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("J" + roweven, "O" + roweven).Style.Font.Bold = true;
                        sht.Range("J" + roweven, "O" + roweven).Style.Font.FontSize = 11;
                        sht.Range("J" + roweven, "O" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("J" + roweven, "O" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        roweven = roweven + 1;
                        using (var range = sht.Range("J" + roweven + ":O" + roweven))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("J" + roweven).Value = "IKEA";
                        sht.Range("J" + roweven + ":L" + roweven).Merge();
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.Bold = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.FontSize = 11;
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("M" + roweven).Value = lstdr.destcode;
                        sht.Range("M" + roweven + ":O" + roweven).Merge();
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.Bold = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.FontSize = 11;
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        roweven = roweven + 1;
                        using (var range = sht.Range("J" + roweven + ":O" + roweven))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("J" + roweven).Value = "PORT OF LOADING";
                        sht.Range("J" + roweven + ":L" + roweven).Merge();
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.Bold = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.FontSize = 11;
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("M" + roweven).Value = lstdr.portofloading;
                        sht.Range("M" + roweven + ":O" + roweven).Merge();
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.Bold = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.FontSize = 11;
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        roweven = roweven + 1;
                        using (var range = sht.Range("J" + roweven + ":O" + roweven))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("J" + roweven).Value = "PORT OF DISCHARGE";
                        sht.Range("J" + roweven + ":L" + roweven).Merge();
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.Bold = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.FontSize = 11;
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("M" + roweven).Value = lstdr.portofdischarge;
                        sht.Range("M" + roweven + ":O" + roweven).Merge();
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.Bold = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.FontSize = 11;
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        roweven = roweven + 1;
                        using (var range = sht.Range("J" + roweven + ":O" + roweven))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("J" + roweven).Value = "SUPPLIER NO.";
                        sht.Range("J" + roweven + ":L" + roweven).Merge();
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.Bold = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.FontSize = 11;
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("M" + roweven).Value = "23130";
                        sht.Range("M" + roweven + ":O" + roweven).Merge();
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.Bold = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.FontSize = 11;
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        roweven = roweven + 1;
                        using (var range = sht.Range("J" + roweven + ":O" + roweven))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        //sht.Range("J" + roweven).Value = "ORDER NO.";
                        //sht.Range("J" + roweven + ":L" + roweven).Merge();
                        //sht.Range("J" + roweven, "L" + roweven).Style.Alignment.WrapText = true;
                        //sht.Range("J" + roweven, "L" + roweven).Style.Font.Bold = true;
                        //sht.Range("J" + roweven, "L" + roweven).Style.Font.FontSize = 11;
                        //sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        //sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        //sht.Range("M" + roweven).Value = "23130";
                        //sht.Range("M" + roweven + ":O" + roweven).Merge();
                        //sht.Range("M" + roweven, "O" + roweven).Style.Alignment.WrapText = true;
                        //sht.Range("M" + roweven, "O" + roweven).Style.Font.Bold = true;
                        //sht.Range("M" + roweven, "O" + roweven).Style.Font.FontSize = 11;
                        //sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        //sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        //roweven = roweven + 1;
                        //using (var range = sht.Range("J" + roweven + ":O" + roweven))
                        //{
                        //    range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                        //    range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                        //    range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                        //    range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        //}

                        sht.Range("J" + roweven).Value = "ORDER NO.";
                        sht.Range("J" + roweven + ":L" + roweven).Merge();
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.Bold = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.FontSize = 11;
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("M" + roweven).Value = lstdr.orderid;
                        sht.Range("M" + roweven + ":O" + roweven).Merge();
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.Bold = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.FontSize = 11;
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        roweven = roweven + 1;
                        using (var range = sht.Range("J" + roweven + ":O" + roweven))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("J" + roweven).Value = "ARTICLE NO.";
                        sht.Range("J" + roweven + ":L" + roweven).Merge();
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.Bold = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.FontSize = 11;
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("M" + roweven).Value = lstdr.artileno;
                        sht.Range("M" + roweven + ":O" + roweven).Merge();
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.Bold = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.FontSize = 11;
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        roweven = roweven + 1;
                        using (var range = sht.Range("J" + roweven + ":O" + roweven))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("J" + roweven).Value = "SIZE";
                        sht.Range("J" + roweven + ":L" + roweven).Merge();
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.Bold = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.FontSize = 11;
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("M" + roweven).Value = lstdr.size;
                        sht.Range("M" + roweven + ":O" + roweven).Merge();
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.Bold = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.FontSize = 11;
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        roweven = roweven + 1;
                        using (var range = sht.Range("J" + roweven + ":O" + roweven))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("J" + roweven).Value = "PACKAGES NO.";
                        sht.Range("J" + roweven + ":L" + roweven).Merge();
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.Bold = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.FontSize = 11;
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("M" + roweven).Value ="'"+ countsheet + "/" + sum;
                        sht.Range("M" + roweven + ":O" + roweven).Merge();
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.Bold = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.FontSize = 11;
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        roweven = roweven + 1;
                        using (var range = sht.Range("J" + roweven + ":O" + roweven))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("J" + roweven).Value = "NO.PCS / Packages";
                        sht.Range("J" + roweven + ":L" + roweven).Merge();
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.Bold = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.FontSize = 11;
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("M" + roweven).Value = lstdr.pcs;
                        sht.Range("M" + roweven + ":O" + roweven).Merge();
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.Bold = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.FontSize = 11;
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        roweven = roweven + 1;
                        using (var range = sht.Range("J" + roweven + ":O" + roweven))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("J" + roweven).Value = "GROSS WT.";
                        sht.Range("J" + roweven + ":L" + roweven).Merge();
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.Bold = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.FontSize = 11;
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("M" + roweven).Value = lstdr.csmgrowei;
                        sht.Range("M" + roweven + ":O" + roweven).Merge();
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.Bold = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.FontSize = 11;
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        roweven = roweven + 1;
                        using (var range = sht.Range("J" + roweven + ":O" + roweven))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("J" + roweven).Value = "NET WT.";
                        sht.Range("J" + roweven + ":L" + roweven).Merge();
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.Bold = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.FontSize = 11;
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("M" + roweven).Value = lstdr.csmgrownet;
                        sht.Range("M" + roweven + ":O" + roweven).Merge();
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.Bold = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.FontSize = 11;
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        roweven = roweven + 1;
                        using (var range = sht.Range("J" + roweven + ":O" + roweven))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("J" + roweven).Value = "DATE";
                        sht.Range("J" + roweven + ":L" + roweven).Merge();
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.Bold = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.FontSize = 11;
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("M" + roweven).Value = lstdr.dlweek;
                        sht.Range("M" + roweven + ":O" + roweven).Merge();
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.Bold = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.FontSize = 11;
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        roweven = roweven + 1;
                        using (var range = sht.Range("J" + roweven + ":O" + roweven))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("J" + roweven).Value = "INVOICE NO.";
                        sht.Range("J" + roweven + ":L" + roweven).Merge();
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.Bold = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.FontSize = 11;
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("M" + roweven).Value = lstdr.invoiceno;
                        sht.Range("M" + roweven + ":O" + roweven).Merge();
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.Bold = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.FontSize = 11;
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        roweven = roweven + 1;
                        using (var range = sht.Range("J" + roweven + ":O" + roweven))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("J" + roweven).Value = "CONSIGNMENT NO.";
                        sht.Range("J" + roweven + ":L" + roweven).Merge();
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.Bold = true;
                        sht.Range("J" + roweven, "L" + roweven).Style.Font.FontSize = 11;
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("J" + roweven, "L" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Range("M" + roweven).Value = lstdr.csmno;
                        sht.Range("M" + roweven + ":O" + roweven).Merge();
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.Bold = true;
                        sht.Range("M" + roweven, "O" + roweven).Style.Font.FontSize = 11;
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("M" + roweven, "O" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                        roweven += 1;
                        using (var range = sht.Range("J" + roweven + ":O" + roweven))
                        {
                            range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }

                        sht.Range("J" + roweven).Value = "MADE IN INDIA";
                        sht.Range("J" + roweven + ":O" + roweven).Merge();
                        sht.Range("J" + roweven, "O" + roweven).Style.Alignment.WrapText = true;
                        sht.Range("J" + roweven, "O" + roweven).Style.Font.Bold = true;
                        sht.Range("J" + roweven, "O" + roweven).Style.Font.FontSize = 11;
                        sht.Range("J" + roweven, "O" + roweven).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("J" + roweven, "O" + roweven).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    }

                    countsheet++;
                }
                

                string Fileextension = "xlsx";
                string filename = "DAVAC." + Fileextension + "";
                Path = Server.MapPath("~/InvoiceExcel/" + filename);
                xapp.SaveAs(Path);

                xapp.Dispose();


                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                //   Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
            }
            if (chkvdf.Checked)
            {

                string Path = "";
                string Pathpdf = "";
                string str = "";

                //str = @"select ROW_NUMBER() over(order by(select 1)) srno,pi.*,dm.*,pa.*,ci.LUTARNNo,ci.LUTIssueDate from preinvoice pi join Destinationmaster dm  on left(pi.desccode, CHARINDEX('-', REVERSE('-' + pi.desccode)))=dm.Destcode left join Packingarticle pa on pi.articleno=pa.ArticleNo left join CompanyInfo ci on ci.MasterCompanyid=pi.MasterCompanyid where pi.invoiceno='" + txtinvoiceno.Text + "' order by dm.Id";

                str = @"select ROW_NUMBER() over(order by(select 1)) srno,pi.*,dm.*,pa.*,ci.LUTARNNo,ci.LUTIssueDate,ci.CompanyName,ci.GSTNo from preinvoice pi join Destinationmaster dm  on pi.desccode=dm.Destcode left join Packingarticle pa on pi.articleno=pa.ArticleNo left join CompanyInfo ci on ci.MasterCompanyid=pi.MasterCompanyid where pi.invoiceno='" + txtinvoiceno.Text + "' order by dm.Id";


                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                int sum = ds.Tables[0].AsEnumerable().Sum(a => a.Field<int>("csmul"));
                decimal grosswt = ds.Tables[0].AsEnumerable().Sum(a => a.Field<decimal>("csmgrowei"));
                decimal netwt = ds.Tables[0].AsEnumerable().Sum(a => a.Field<decimal>("csmgrownet"));
                decimal cbm = ds.Tables[0].AsEnumerable().Sum(a => a.Field<decimal>("csmgroval"));
                if (ds.Tables[0].Rows.Count > 0)
                {

                    XLWorkbook xapp = new XLWorkbook(Server.MapPath("~/VDFExcel/VDFExcel_daimond.xlsx"));
                    IXLWorksheet sht = xapp.Worksheet(1);
                    //var sht = xapp.Worksheets.Add("DAVAC");
                    sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                    //  sht.PageSetup.AdjustTo(30);
                     sht.PageSetup.FitToPages(1, 1);
                    sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                    sht.PageSetup.VerticalDpi = 100;
                    sht.PageSetup.HorizontalDpi = 100;
                    sht.PageSetup.Margins.Top = 0.0;
                    sht.PageSetup.Margins.Bottom = 0.0;
                    sht.PageSetup.Margins.Right = 0.0;
                    sht.PageSetup.Margins.Left = 0.2;
                    sht.Column("F").Width = 12.89;
                    sht.Column("F").Width = 12.89;
                    sht.Column("H").Width = 15.89;
                    //sht.Range("A5:M8").Style.Font.FontSize = 10;
                    //sht.Range("A5:M8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    var a = sht.Cell(5, 1);
                    a.RichText.AddText("SUPPLIER NAME: ").SetFontName("Arial Black");
                    //var cd = sht.Cell(5, 3);
                    //cd.SetValue(Convert.ToString(ds.Tables[0].Rows[0]["companyname"]));
                      //a.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["companyname"])).SetFontName("Cambria");

                    sht.Range("C5").SetValue(Convert.ToString(ds.Tables[0].Rows[0]["companyname"]));
                    sht.Range("C5:D5").Merge();
                    sht.Range("C5:D5").Style.Font.FontName = "Arial";
                    sht.Range("C5:D5").Style.Font.FontSize = 12;
                    sht.Range("C5:D5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    //sht.Range("C5:D5").Style.Font.Bold = true;


                     var a1 = sht.Cell(7, 1);
                     a1.SetValue("India");
                     a1.Style.Font.FontName = "Arial";
                     a1.Style.Font.FontSize = 10;
                      a1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                      var a2 = sht.Cell(8, 1);
                      a2.Style.Font.FontName = "Arial";
                      a2.Style.Font.FontSize = 10;
                     // a2.RichText.AddText("Phone").SetFontName("Arial Black");
                      a2.SetValue("Phone");
                      a2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                      var a3 = sht.Cell(9, 1);
                     // a3.RichText.AddText("Fax").SetFontName("Arial Black");
                      a3.SetValue("Fax");
                      a3.Style.Font.FontName = "Arial";
                      a3.Style.Font.FontSize = 10;

                    var b = sht.Cell(10 , 1);
                    b.RichText.AddText("GSTIN.# ").SetFontName("Arial Black");
                   // b.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["GSTNo"])).SetFontName("Cambria");
                    sht.Range("C10").SetValue(Convert.ToString(ds.Tables[0].Rows[0]["GSTNo"]));
                    sht.Range("C10:D10").Merge();
                    sht.Range("C10:D10").Style.Font.FontName = "Arial";
                    sht.Range("C10:D10").Style.Font.FontSize = 12;
                    sht.Range("C10:D10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                   

                    var D = sht.Cell(12, 1);
                    D.RichText.AddText("SUPPLIER No. ").SetFontName("Arial Black");
                  //  D.RichText.AddText("21899").SetFontName("Cambria");

                    sht.Range("C12").SetValue("23130");
                    sht.Range("C12:D12").Merge();
                    sht.Range("C12:D12").Style.Font.FontName = "Arial";
                    sht.Range("C12:D12").Style.Font.FontSize = 12;
                    sht.Range("C12:D12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    var E = sht.Cell(5, 5);
                    E.RichText.AddText("INVOICE No. ").SetFontName("Arial Black");
                      //E.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["invoiceno"])).SetFontName("Cambria");
                    sht.Range("H5").SetValue(Convert.ToString(ds.Tables[0].Rows[0]["invoiceno"]));
                    sht.Range("H5:I5").Merge();
                    sht.Range("H5:I5").Style.Font.FontName = "Arial";
                    sht.Range("H5:I5").Style.Font.FontSize = 12;
                    sht.Range("H5:I5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                    var F = sht.Cell(6, 5);
                    F.RichText.AddText("INVOICE DATE : ").SetFontName("Arial Black");
                    //  F.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["invoicedate"])).SetFontName("Cambria");

                    sht.Range("H6").SetValue(Convert.ToDateTime(ds.Tables[0].Rows[0]["invoicedate"]).ToString("dd/MMM/yyyy"));
                      sht.Range("H6:I6").Merge();
                      sht.Range("H6:I6").Style.Font.FontName = "Arial";
                      sht.Range("H6:I6").Style.Font.FontSize = 12;
                      sht.Range("H6:I6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                   //   sht.Range("H6:16").Style.NumberFormat.Format = "dd/MMM/yyyy";



                    var G = sht.Cell(7, 5);
                    G.RichText.AddText("APLL CSGN NO ").SetFontName("Arial Black");
                     // G.RichText.AddText("ECIS NO " + Convert.ToString(ds.Tables[0].Rows[0]["csmno"])).SetFontName("Cambria");

                      sht.Range("H7").SetValue(Convert.ToString(ds.Tables[0].Rows[0]["csmno"]));
                      sht.Range("H7:I7").Merge();
                      sht.Range("H7:I7").Style.Font.FontName = "Arial";
                      sht.Range("H7:I7").Style.Font.FontSize = 12;
                      sht.Range("H7:I7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                    var H = sht.Cell(5, 10);
                    H.RichText.AddText("CBM DECLARED : ").SetFontName("Arial Black");
                    //  H.RichText.AddText(string.Format(Convert.ToString(ds.Tables[0].Rows[0]["Volume"]), "#,##0.000") + " CBM").SetFontName("Arial Black");
                    sht.Range("M5").SetValue(cbm +" CBM");
                    //sht.Range("M7:M7").Merge();
                    sht.Range("M5:M5").Style.Font.FontName = "Arial";
                    sht.Range("M5:M5").Style.Font.FontSize = 12;
                    sht.Range("M5:M5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    var I = sht.Cell(6, 10);
                    I.RichText.AddText("NET WEIGHT : ").SetFontName("Arial Black");
                      //I.RichText.AddText(string.Format(Convert.ToString(ds.Tables[0].Rows[0]["netwt"]), "#,##0.000") + " KG").SetFontName("Arial Black");
                    sht.Range("M6").SetValue(netwt + " KGS");
                    //sht.Range("M7:M7").Merge();
                    sht.Range("M6:M6").Style.Font.FontName = "Arial";
                    sht.Range("M6:M6").Style.Font.FontSize = 12;
                    sht.Range("M6:M6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                  

                    var J = sht.Cell(7, 10);
                    J.RichText.AddText("GROSS WEIGHT : ").SetFontName("Arial Black");
                    // J.RichText.AddText(string.Format(Convert.ToString(ds.Tables[0].Rows[0]["csmgrowei"]), "#,##0.000") + " KG").SetFontName("Arial Black");
                    sht.Range("M7").SetValue(grosswt + " KGS");
                    //sht.Range("M7:M7").Merge();
                    sht.Range("M7:M7").Style.Font.FontName = "Arial";
                    sht.Range("M7:M7").Style.Font.FontSize = 12;
                    sht.Range("M7:M7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("M7:M7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    sht.Range("A9:M11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    var K = sht.Cell(13, 1);
                    K.RichText.AddText("DESTINATION CODE: ").SetFontName("Arial Black");
                   //   K.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["destcode"])).SetFontName("Cambria");
                    sht.Range("D13").SetValue(Convert.ToString(ds.Tables[0].Rows[0]["destcode"]));
                    //sht.Range("M7:M7").Merge();
                    sht.Range("D13:D13").Style.Font.FontName = "Arial";
                    sht.Range("D13:D13").Style.Font.FontSize = 12;
                    sht.Range("D13:D13").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    var L = sht.Cell(15, 1);
                    L.RichText.AddText("DELIVERY WEEK: ").SetFontName("Arial Black");
                   //  L.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["Dlvweek"])).SetFontName("Arial Black");
                    sht.Range("D15").SetValue(Convert.ToString(ds.Tables[0].Rows[0]["Dlvweek"]));
                     //sht.Range("M7:M7").Merge();
                     sht.Range("D15:D15").Style.Font.FontName = "Arial";
                     sht.Range("D15:D15").Style.Font.FontSize = 12;
                     sht.Range("D15:D15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                   

                    var N = sht.Cell(13, 5);
                    N.RichText.AddText("TRUCK No. ").SetFontName("Arial Black");
                  //  N.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["truckno"])).SetFontName("Arial Black");
                    sht.Range("H13").SetValue(Convert.ToString(ds.Tables[0].Rows[0]["truckno"]));
                    sht.Range("H13:I13").Merge();
                    sht.Range("H13:I13").Style.Font.FontName = "Arial";
                    sht.Range("H13:I13").Style.Font.FontSize = 12;
                    sht.Range("H13:I13").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                    //sht.Range("D15").SetValue(Convert.ToString(ds.Tables[0].Rows[0]["Dlvweek"]));
                    //sht.Range("M7:M7").Merge();
                    //sht.Range("D15:D15").Style.Font.FontName = "Arial";
                    //sht.Range("D15:D15").Style.Font.FontSize = 10;
                    //sht.Range("D15:D15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                    var M = sht.Cell(14, 5);
                    M.RichText.AddText("SEAL NO : ").SetFontName("Arial Black");
                    //M.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["sealno"])).SetFontName("Cambria");
                    //M.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("H14").SetValue(Convert.ToString(ds.Tables[0].Rows[0]["sealno"]));
                    sht.Range("H14:I14").Merge();
                    sht.Range("H14:I14").Style.Font.FontName = "Arial";
                    sht.Range("H14:I14").Style.Font.FontSize = 12;
                    sht.Range("H14:I14").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    sht.Range("J14").SetValue("(TO BE FILLED BY APLL)");
                    sht.Range("J14:L14").Merge();
                    sht.Range("J14:L14").Style.Font.FontName = "Arial";
                    sht.Range("J14:L14").Style.Font.FontSize = 12;
                    sht.Range("J14:L14").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                    var c = sht.Cell(15, 5);
                    c.RichText.AddText("SHIPMENT ID: ").SetFontName("Arial Black");
                  //  c.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["shppingid"])).SetFontName("Cambria");

                    sht.Range("H15").SetValue(Convert.ToString(ds.Tables[0].Rows[0]["shppingid"]));
                    sht.Range("H15:I15").Merge();
                    sht.Range("H15:I15").Style.Font.FontName = "Arial";
                    sht.Range("H15:I15").Style.Font.FontSize = 12;
                    sht.Range("H15:I15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                    sht.Range("J15").SetValue("PALLETS BY (PLEASE TICK ONE) Ö APLL /  SELF");
                    sht.Range("J15:M15").Merge();
                    sht.Range("J15:M15").Style.Font.FontName = "Arial";
                    sht.Range("J15:M15").Style.Font.FontSize = 11;
                    sht.Range("J15:M15").Style.Font.Bold = true;
                    sht.Range("J15:M15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    var O = sht.Cell(16, 5);
                    O.RichText.AddText("TRUCK DESP. DATE ").SetFontName("Arial Black");
                    sht.Range("H16").SetValue(Convert.ToDateTime(ds.Tables[0].Rows[0]["Dispdate"]).ToString("dd/MMM/yyy"));
                    sht.Range("H16:I16").Merge();
                    sht.Range("H16:I16").Style.Font.FontName = "Arial";
                    sht.Range("H16:I16").Style.Font.FontSize = 12;
                    sht.Range("H16:I16").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    //sht.Range("H16:I16").Style.NumberFormat.Format = "dd/MMM/yyyy";
                  //  O.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["Dispdate"])).SetFontName("Arial Black");
                    //**************Details
                    int row = 22,noofpkgs=0,noofqty=0;
                    double totalcbm = 0;
                    decimal noofpallet=0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sht.Range("A" + row + ":M" + row).Style.Font.FontName = "Arial";
                        sht.Range("A" + row + ":M" + row).Style.Font.FontSize = 10;
                        sht.Range("A" + row + ":M" + row).Style.Font.Bold = true;
                        sht.Range("A" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        using (var ar = sht.Range("A" + row + ":M" + row))
                        {
                            ar.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            ar.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            ar.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            ar.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }
                        noofqty += Convert.ToInt32(ds.Tables[0].Rows[i]["bkdqty"]);
                        noofpallet += Convert.ToDecimal(ds.Tables[0].Rows[i]["csmul"]);
                        noofpkgs += Convert.ToInt32(ds.Tables[0].Rows[i]["csmul"]);
                        totalcbm += Convert.ToDouble(ds.Tables[0].Rows[i]["csmgroval"]);

                        sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["orderid"]);
                        sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Articleno"]);
                        sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["bkdqty"].ToString() + " Pcs");
                        sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["csmul"]);
                        sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["csmul"]);
                        sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["csmgroval"]);
                         sht.Range("F" + row).Style.NumberFormat.Format = "###0.000";

                        sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["csmul"].ToString() + "  PALLET");
                        sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["bkdqty"].ToString() + " Pcs");
                        sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["csmul"]);
                        sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["csmul"]);
                        sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["palletsize"].ToString()+" CM");
                        sht.Range("L" + row).SetValue("S"+ds.Tables[0].Rows[i]["sl"].ToString());

                        row = row + 1;
                    }
                    //************           
                    row = row + 1;
                    sht.Range("A" + row).SetValue("Total");
                    sht.Range("A" + row ).Style.Font.Bold = true;
                    sht.Range("A" + row ).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C" + row).SetValue(noofqty+" PCS");
                    sht.Range("C" + row).Style.Font.Bold = true;
                    sht.Range("C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("H" + row).SetValue(noofqty + " PCS");
                    sht.Range("H" + row).Style.Font.Bold = true;
                    sht.Range("H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("D" + row).SetValue(noofpkgs);
                    sht.Range("D" + row).Style.Font.Bold = true;
                    sht.Range("D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("E" + row).SetValue(noofpallet);
                    sht.Range("E" + row).Style.Font.Bold = true;
                    sht.Range("E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("I" + row).SetValue(noofpkgs);
                    sht.Range("I" + row).Style.Font.Bold = true;
                    sht.Range("I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("J" + row).SetValue(noofpallet);
                    sht.Range("J" + row).Style.Font.Bold = true;
                    sht.Range("J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("F" + row).SetValue(totalcbm);
                    sht.Range("F" + row).Style.NumberFormat.Format = "###0.000";
                    sht.Range("F" + row).Style.Font.Bold = true;
                    sht.Range("F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    using (var ar = sht.Range("A" + row + ":M" + row))
                    {
                        ar.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                        ar.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                        ar.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                        ar.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    }
                    for (int i = 0; i <= 2; i++)
                    {
                        sht.Range("A" + row + ":M" + row).Style.Font.FontName = "Arial";
                        sht.Range("A" + row + ":M" + row).Style.Font.FontSize = 10;
                        sht.Range("A" + row + ":M" + row).Style.Font.Bold = true;
                        sht.Range("A" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        using (var ar = sht.Range("A" + row + ":M" + row))
                        {
                            ar.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            ar.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            ar.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            ar.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }
                        i = i + 1;
                        row = row + 1;
                    }
                    //*******************
                    row = row + 1;
                    sht.Range("A" + row + ":C" + row).Merge();
                    sht.Range("I" + row + ":M" + row).Merge();
                    sht.Range("A" + row + ":M" + row).Style.Font.FontName = "Arial Black";
                    sht.Range("A" + row + ":M" + row).Style.Font.FontSize = 10;
                    sht.Range("A" + row + ":M" + row).Style.Font.Bold = true;
                    sht.Range("A" + row).SetValue("VENDOR’S SIGNATURE & STAMP");
                    sht.Range("I" + row).SetValue("APLL ACKNOWLEDGEMENT at W/H");
                    var a4 = sht.Cell(row, 5);
                    sht.Range("A" + row).Style.Font.FontSize = 10;
                    a4.RichText.AddText("Please").SetFontName("Arial Black");
                    a4.RichText.AddText(" Ö").SetFontName("Symbol").SetBold();
                    //**********
                    //row = row + 1;
                    //sht.Range("G" + row + ":J" + row).Merge();
                    //sht.Range("G" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    //var B1 = sht.Cell(row, 7);
                    //B1.RichText.AddText("· ").SetFontName("Symbol").SetFontSize(10).SetBold();
                    //B1.RichText.AddText("Received By :").SetFontName("Arial").SetFontSize(10).SetBold();
                    //*******
                    //row = row + 1;
                    //sht.Range("G" + row + ":J" + row).Merge();
                    //sht.Range("G" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    //var B2 = sht.Cell(row, 7);
                    //B2.RichText.AddText("· ").SetFontName("Symbol").SetFontSize(10).SetBold();
                    //B2.RichText.AddText("Condition of Cargo :").SetFontName("Arial").SetFontSize(10).SetBold();
                    //**********
                    //row = row + 3;
                    //var a4 = sht.Cell(row, 1);
                    //sht.Range("A" + row).Style.Font.FontSize = 10;
                    //a4.RichText.AddText("Please").SetFontName("Arial Black");
                    //a4.RichText.AddText(" Ö").SetFontName("Symbol").SetBold();

                    //**************
                    row = row + 1;
                    sht.Range("E" + row + ":E" + (row + 4)).Style.Font.FontName = "Arial";
                    sht.Range("E" + row + ":E" + (row + 4)).Style.Font.FontSize = 7.5;
                    sht.Range("E" + row + ":E" + (row + 4)).Style.Font.Bold = true;
                    sht.Range("E" + row + ":E" + (row + 4)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    //sht.Range("D" + row + ":J" + (row + 4)).Style.Font.FontName = "Arial";
                    //sht.Range("D" + row + ":J" + (row + 4)).Style.Font.FontSize = 7.5;
                    //sht.Range("D" + row + ":J" + (row + 4)).Style.Font.Bold = true;
                    //sht.Range("D" + row + ":J" + (row + 4)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    using (var range = sht.Range("E" + row + ":F" + (row + 4)))
                    {
                        range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                        range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                        range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                        range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    }
                    sht.Range("E" + row).SetValue("DEEC");
                    //var B1 = sht.Cell(row, 8);
                    //B1.RichText.AddText("· ").SetFontName("Symbol").SetFontSize(10).SetBold();
                    //B1.RichText.AddText("Received By :").SetFontName("Arial").SetFontSize(10).SetBold();
                    sht.Range("H" + row).SetValue("Received By :");
                    sht.Range("H" + row + ":M" + row).Merge();
                    sht.Range("H" + row + ":M" + row).Style.Font.FontName = "Times New Roman";
                    sht.Range("H" + row + ":M" + row).Style.Font.FontSize = 10;
                    sht.Range("H" + row + ":M" + row).Style.Font.Bold = false;
                    sht.Range("H" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    //using (var range = sht.Range("D" + row + ":J" + (row + 4)))
                    //{
                    //    range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                    //    range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                    //    range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                    //    range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    //}
                    row = row + 1;
                    sht.Range("E" + row).SetValue("DEPB + EPCG");
                   
                    //var B2 = sht.Cell(row, 8);
                    //B2.RichText.AddText("· ").SetFontName("Symbol").SetFontSize(10).SetBold();
                    //B2.RichText.AddText("Date & Time Reporting at Warehouse By :").SetFontName("Arial").SetFontSize(10).SetBold();
                    sht.Range("H" + row).SetValue("Date & Time Reporting at Warehouse By :");
                    sht.Range("H" + row + ":M" + row).Merge();
                    sht.Range("H" + row + ":M" + row).Style.Font.FontName = "Times New Roman";
                    sht.Range("H" + row + ":M" + row).Style.Font.FontSize = 10;
                    sht.Range("H" + row + ":M" + row).Style.Font.Bold = false;
                    sht.Range("H" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    row = row + 1;

                    sht.Range("E" + row).SetValue("DRAWBACK");
                     sht.Range("F" + row).Value = "Ö";
                    sht.Range("F" + row).Style.Font.FontName = "Symbol";
                    sht.Range("F" + row).Style.Font.SetFontColor(XLColor.Blue);
                    sht.Range("F" + row).Style.Font.Bold = true;
                    //var B3 = sht.Cell(row, 8);
                    //B3.RichText.AddText("· ").SetFontName("Symbol").SetFontSize(10).SetBold();
                    //B3.RichText.AddText("Date & Time of Offloading :").SetFontName("Arial").SetFontSize(10).SetBold();
                    sht.Range("H" + row).SetValue("Date & Time of Offloading :");
                    sht.Range("H" + row + ":M" + row).Merge();
                    sht.Range("H" + row + ":M" + row).Style.Font.FontName = "Times New Roman";
                    sht.Range("H" + row + ":M" + row).Style.Font.FontSize = 10;
                    sht.Range("H" + row + ":M" + row).Style.Font.Bold = false;
                    sht.Range("H" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    row = row + 1;
                    sht.Range("E" + row).SetValue("WHITE");
                    //var B4 = sht.Cell(row, 8);
                    //B4.RichText.AddText("· ").SetFontName("Symbol").SetFontSize(10).SetBold();
                    //B4.RichText.AddText("Condition of the cargo :").SetFontName("Arial").SetFontSize(10).SetBold();
                    sht.Range("H" + row).SetValue("Condition of the cargo :");
                    sht.Range("H" + row + ":M" + row).Merge();
                    sht.Range("H" + row + ":M" + row).Style.Font.FontName = "Times New Roman";
                    sht.Range("H" + row + ":M" + row).Style.Font.FontSize = 10;
                    sht.Range("H" + row + ":M" + row).Style.Font.Bold = false;
                    sht.Range("H" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    row = row + 1;
                    sht.Range("E" + row).SetValue("100% EOU");
                    //sht.Range("D" + row + ":G" + row).Merge();
                    //sht.Range("H" + row + ":J" + row).Merge();
                    //sht.Range("D" + row).Value = "Truck Gate-out Date n Time";
                    //**********
                    row = row + 2;
                    sht.Range("A" + row + ":B" + row).Merge();
                    sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial";
                    sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 10;
                    sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;
                    sht.Range("A" + row).Value = "TERMS & CONDITIONS";
                    //*******
                    row = row + 1;
                    //
                    sht.Range("A" + row + ":M" + row).Merge();
                    sht.Range("A" + row + ":M" + (row + 3)).Style.Font.FontName = "Times New Roman";
                    sht.Range("A" + row + ":M" + (row + 3)).Style.Font.FontSize = 7;
                    sht.Range("A" + row + ":M" + (row + 3)).Style.Font.Bold = false;
                    sht.Range("A" + row).Value = "All transaction with APLL are subject to the terms and condition stipulated in the company’s cargo receipt (copies available on request from company) APLL may exclude or limit its liabilities and apply certain cases";
                    //
                    row = row + 1;
                    sht.Range("A" + row + ":M" + row).Merge();
                    sht.Range("A" + row).Value = "   1)    This Vendor Declaration form is applicable only if it is validated and signed by an authorized signatory of APLL warehouse or appointed warehouse operator in additions to that of shipper’s or it’s representative.";
                    //
                    row = row + 1;

                    sht.Range("A" + row + ":M" + row).Merge();
                    sht.Range("A" + row).Value = "   2)    This Vendor Declaration form should be completed and presented to APLL or it’s appointed warehouse operator for processing and validatio";
                    //
                    row = row + 1;
                    sht.Range("A" + row + ":M" + row).Merge();
                    sht.Range("A" + row).Value = "APLL reserves the right to verify and adjust shipper’s declared cargo measurement in accordance with the actual declare measurement";

                    using (var range = sht.Range("A" + (row-15) + ":A" + (row)))
                    {
                        range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                        //range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                        //range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                        //range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    }
                    using (var range = sht.Range("M" + (row - 15) + ":M" + (row)))
                    {
                       // range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                        range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                        //range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                        //range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    }
                    using (var range = sht.Range("A" + (row) + ":M" + (row)))
                    {
                        // range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                        //range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                        //range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                        range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    }

                    //Save
                    string PathVDF = "";
                    string Fileextension = "xlsx";
                    string filename = "Vdf" + "." + Fileextension ;
                    PathVDF = Server.MapPath("~/Tempexcel/" + filename);
                    xapp.SaveAs(PathVDF);
                    xapp.Dispose();
                    //Download File
                    Response.Clear();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                    Response.WriteFile(PathVDF);
                    Response.End();
                    //*************
                    lblmsg.Text = "VDF Excel Format downloaded successfully.";

                    //string Fileextension = "xlsx";
                    //string filename = "VDF." + Fileextension + "";
                    //Path = Server.MapPath("~/InvoiceExcel/" + filename);
                    //xapp.SaveAs(Path);

                    //xapp.Dispose();


                    ////Download File
                    //Response.Clear();
                    //Response.ContentType = "application/vnd.ms-excel";
                    ////   Response.ContentType = "application/pdf";
                    //Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                    //Response.WriteFile(Path);
                    //Response.End();

                }
            }

        }
        catch (Exception ex)
        {
        }

    


    }
    public class preinvoicemodel
    {
        public int srno { get; set; }
        public string orderid { get; set; }
        public string artileno { get; set; }
        //public string itemdesc { get; set; }
        //public float price { get; set; }
        //public string curr { get; set; }
        public string destcode { get; set; }
        //public int sl { get; set; }
       // public string csmno { get; set; }
        public string invoiceno { get; set; }
        //public DateTime invoicedate { get; set; }
        //public DateTime dispdate { get; set; }
        //public string shippingid { get; set; }
        public int bkdqty { get; set; }
        public int csmul { get; set; }
        public int csmval { get; set; }
        public int csmgroval { get; set; }
        public int pcs { get; set; }
        public int csmgrowei { get; set; }
        public int csmgrownet { get; set; }
        public string csmno { get; set; }
        public string size { get; set; }
        //public string truckno { get; set; }
        //public string precarriageby { get; set; }
        //public string placeofreceipt { get; set; }
        //public string vesselno { get; set; }
        public string portofloading { get; set; }
        public string portofdischarge { get; set; }
        //public string finaldestination { get; set; }
        //public string countryoffinaldestination { get; set; }
        //public string countryoforigin { get; set; }
        public string dlweek { get; set; }

    
    }
}
