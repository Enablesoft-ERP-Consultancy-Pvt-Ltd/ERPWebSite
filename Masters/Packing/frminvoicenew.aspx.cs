using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Masters_Packing_frminvoicenew : System.Web.UI.Page
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

            txtinvoiceno_AutoCompleteExtender.ContextKey = DDSession.SelectedValue;

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
        string str = string.Empty;
        if (Session["varMasterCompanyIDForERP"].ToString() == "14")
        {
            str = @"select  I.Invoiceid,I.Destcode,I.Dtstamp,DM.Buyername,I.Delvwk,DM.Consignee_Address,DM.payingagent_address,
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
                        Where I.consignorId = " + DDcompany.SelectedValue + " And I.Tinvoiceno='" + txtinvoiceno.Text + "' And I.InvoiceYear="+DDSession.SelectedValue+"";
        }
        else
        {
            str = @"select Top 100 I.Invoiceid,I.Destcode,I.Dtstamp,DM.Buyername,I.Delvwk,DM.Consignee_Address,DM.payingagent_address,
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
        
        }

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
}
