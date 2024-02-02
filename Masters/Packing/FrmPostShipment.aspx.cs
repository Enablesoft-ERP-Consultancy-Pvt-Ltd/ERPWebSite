using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Data.SqlTypes;

public partial class Masters_Packing_FrmPostShipment : System.Web.UI.Page
{
    int varid = 0;
    static int varIdForDbk = 0;
    string Msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            Refresh();
            UtilityModule.ConditionalComboFill(ref DDSession, "Select Year,Session From Session Order by Year Desc", true, "--Select--");
        }
    }
    public void fillAmtGrids()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        GDVAmtDetail.DataSource = "";
        GDVAmtDetail.DataBind();
        GDVAmtDetail2.DataSource = "";
        GDVAmtDetail2.DataBind();
        string Qry = "select invoiceid,replace( convert(nvarchar(11),recamtdate,106),' ','-') recamtdate,isnull(amt,0) amt,id,RTTNo From InvoiceAmtRec Where invoiceid=" + DDInvoiceNo.SelectedValue + @"
                        Group By invoiceid,id,amt,recamtdate,RTTNo  Having amt > 0
                        Select invoiceid,replace( convert(nvarchar(11),recamtdate,106),' ','-') recamtdate,id,isnull(DBKAmt,0) as DBKAmt,RTTNo From InvoiceAmtRec Where invoiceid=" + DDInvoiceNo.SelectedValue + @"
                        Group By invoiceid,recamtdate,id,DBKAmt,RTTNo Having DBKAmt > 0 ";

        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Qry);
        if (ds.Tables[0].Rows.Count > 0)
        {

            GDVAmtDetail.DataSource = ds.Tables[0];
            GDVAmtDetail.DataBind();
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            GDVAmtDetail2.DataSource = ds.Tables[1];
            GDVAmtDetail2.DataBind();
        }
        #region
        //For i = 0 To ds1.Tables(0).Rows.Count - 1
        //    If Val(ds1.Tables(0).Rows(i)("amt")) > 0 Then
        //        dgAmtDtl.Rows.Add()
        //        dgAmtDtl.Item("invsID", dgAmtDtl.Rows.Count - 1).Value = ds1.Tables(0).Rows(i)("invoiceid")
        //        dgAmtDtl.Item("id", dgAmtDtl.Rows.Count - 1).Value = ds1.Tables(0).Rows(i)("id")
        //        dgAmtDtl.Item("RecAmt", dgAmtDtl.Rows.Count - 1).Value = ds1.Tables(0).Rows(i)("amt")
        //        dgAmtDtl.Item("recDate", dgAmtDtl.Rows.Count - 1).Value = Format(ds1.Tables(0).Rows(i)("recamtdate"), "dd-MMM-yyyy")
        //        dgAmtDtl.Item("RTTNo", dgAmtDtl.Rows.Count - 1).Value = ds1.Tables(0).Rows(i)("RTTNo")
        //    End If

        //    If Val(ds1.Tables(0).Rows(i)("DBKAmt")) > 0 Then
        //        dgAmtDt2.Rows.Add()
        //        dgAmtDt2.Item("DBKinvsID", dgAmtDt2.Rows.Count - 1).Value = ds1.Tables(0).Rows(i)("invoiceid")
        //        dgAmtDt2.Item("DBKId", dgAmtDt2.Rows.Count - 1).Value = ds1.Tables(0).Rows(i)("id")
        //        dgAmtDt2.Item("DBKRecAmt", dgAmtDt2.Rows.Count - 1).Value = ds1.Tables(0).Rows(i)("DBKAmt")
        //        dgAmtDt2.Item("DBKRecDate", dgAmtDt2.Rows.Count - 1).Value = Format(ds1.Tables(0).Rows(i)("recamtdate"), "dd-MMM-yyyy")
        //    End If
        //Next
        #endregion
    }
    private void MessageSave(string msg)
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('");
        stb.Append(msg);
        stb.Append("');</script>");
        //ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        ScriptManager.RegisterStartupScript(Page, GetType(), "opn", stb.ToString(), true);
               
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {


            string str = "UPDATE INVOICE set Exchrate=" + (txtExRate.Text == "" ? "0" : txtExRate.Text) + ",sbillno='" + txtshpBillNo.Text + "', ";
            str = str + "Sbilldate=" + (TxtShpBillDate.Text == "" ? "NUll" : "'" + TxtShpBillDate.Text + "'") + ",Blno='" + txtBlNo.Text + "',Bldt=" + (txtBlDate.Text == "" ? "NUll" : "'" + txtBlDate.Text + "'") + ", ";
            str = str + "Vesselname='" + txtvesselName.Text + "',brcExchRate=" + (txtBRCExrate.Text == "" ? 0 : Convert.ToDecimal(txtBRCExrate.Text)) + ",";
            str = str + "DocType='" + DDDocType.SelectedItem.Text + "',DrawBackamount=" + (txtDBKAmt.Text == "" ? "0" : txtDBKAmt.Text) + " ,";
            str = str + "FIRCNo='" + txtFircno.Text + "',FIRCNoDate=" + (txtFIRCDate.Text == "" ? "NUll" : "'" + txtFIRCDate.Text + "'") + ", ";
            str = str + "FormANo='" + txtFormANo.Text + "',FormADate=" + (txtformADate.Text == "" ? "NUll" : "'" + txtformADate.Text + "'") + ", ";
            str = str + "OpenPolicyNo='" + txtpolicyno.Text + "',CreditNo='" + TxtLetterCreditNo.Text + "', ";
            str = str + "CreditNoDate=" + (TxtCRNoDate.Text == "" ? "NUll" : "'" + TxtCRNoDate.Text + "'") + ",PInvoiceNo='" + txtPerfInvno.Text + "', ";
            str = str + "Package='" + txtPkgType.Text + "',Container='" + txtContainer.Text + "', ";
            str = str + "ContainerNo='" + txtContinerNo.Text + "',SealNo='" + txtSealNo.Text + "', ";
            str = str + "PInvoiceDate=" + (txtPerfInvDate.Text == "" ? "NUll" : "'" + txtPerfInvDate.Text + "'") + ", ";
            str = str + "Agent_Commission=" + (txtCommission.Text == "" ? "0" : txtCommission.Text) + ",License_Commission=" + (txtLicenseCommission.Text == "" ? "0" : txtLicenseCommission.Text) + ",";
            str = str + "BankSubmissionDate=" + (txtbanksubmissionDate.Text == "" ? "NUll" : "'" + txtbanksubmissionDate.Text + "'") + ",banksubmissionrefno='" + txtBankSubmissionRefNo.Text + "' ,";
            str = str + "FlightDate=" + (txtFlightDate.Text == "" ? "NUll" : "'" + txtFlightDate.Text + "'") + ",FlightNo='" + txtFlightNo.Text + "' ,";
            str = str + "Terms=" + (txtCrdays.Text == "" ? "NUll" : "'" + txtCrdays.Text + "'") + ", ";
            str = str + "LeoDate=" + (txtLEODate.Text== "" ? "NUll" : "'" + txtLEODate.Text + "'") + "  where invoiceid = " + DDInvoiceNo.SelectedValue + "";
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            Tran.Commit();
            Refresh();
            if (DDInvoiceNo.Items.Count > 0)
            {
                DDInvoiceNo.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            Msg = ex.Message;
            Tran.Rollback();
            MessageSave(Msg);
        }
        Msg = "Record(s) has been saved!";
        MessageSave(Msg);
    }
    protected void DDSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Qry = @"select I.Invoiceid,I.TInvoiceNo From Invoice I,Packing P 
        Where P.InvoiceNo=I.InvoiceId And I.Status=1 And I.InvoiceType<>3 And 
        I.InvoiceYear=" + DDSession.SelectedValue + " And P.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And P.ConsignorId = " + Session["CurrentWorkingCompanyID"] + " Order By I.TinvoiceNo desc";
        UtilityModule.ConditionalComboFill(ref DDInvoiceNo, Qry, true, "--Select--");
    }
    protected void DDInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Refresh();
        try
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select exchrate,sbillno,convert(varchar(11),sbilldate,106) sbilldate ,blno,convert(varchar(11),bldt,106) bldt,
                        vesselname,terms,brcExchRate,DocType,DrawBackamount,FIRCNo,convert(varchar(11),FIRCNoDate,106) FIRCNoDate,FormANo,convert(varchar(11),FormADate,106) FormADate,  
                        OpenPolicyNo, CreditNo, convert(varchar(11),CreditNoDate,106) CreditNoDate, PInvoiceNo, Package, Container, ContainerNo, SealNo,convert(varchar(11),
                        PInvoiceDate,106) PInvoiceDate,Agent_Commission,isnull(License_Commission,0) As License_Commission,BankSubmissionDate,BankSubmissionRefNo,
                        isnull(convert(varchar(11),FlightDate,106),'') as FlightDate,isnull(FlightNo,'') as FlightNo, isnull(convert(varchar(11),LeoDate,106),'') as LeoDate,
                        isnull(InvoiceAmount,0) as InvoiceAmount
                        From invoice where invoiceid=" + DDInvoiceNo.SelectedValue);

            txtExRate.Text = ds.Tables[0].Rows[0]["exchrate"].ToString();
            txtshpBillNo.Text = ds.Tables[0].Rows[0]["sbillno"].ToString();
            TxtShpBillDate.Text = ds.Tables[0].Rows[0]["sbilldate"].ToString().Replace(" ", "-");
            txtBlNo.Text = ds.Tables[0].Rows[0]["blno"].ToString();
            //txtBlDate.Text = IIf(IsDBNull(ds.Tables(0).Rows(0)("bldt")), Format(Date.Today, "dd-MMM-yyyy"), ds.Tables(0).Rows(0)("bldt"))
            txtBlDate.Text = ds.Tables[0].Rows[0]["bldt"].ToString().Replace(" ", "-");
            txtvesselName.Text = ds.Tables[0].Rows[0]["vesselname"].ToString();
            txtCrdays.Text = ds.Tables[0].Rows[0]["terms"].ToString();
            txtBRCExrate.Text = ds.Tables[0].Rows[0]["brcExchRate"].ToString();
            txtDBKAmt.Text = ds.Tables[0].Rows[0]["DrawBackamount"].ToString();
            DDDocType.Text = ds.Tables[0].Rows[0]["DocType"].ToString();
            txtFircno.Text = ds.Tables[0].Rows[0]["FIRCNo"].ToString();
            txtFIRCDate.Text = ds.Tables[0].Rows[0]["FIRCNoDate"].ToString().Replace(" ", "-");
            txtFormANo.Text = ds.Tables[0].Rows[0]["FormANo"].ToString();
            txtformADate.Text = ds.Tables[0].Rows[0]["FormADate"].ToString().Replace(" ", "-");
            txtpolicyno.Text = ds.Tables[0].Rows[0]["OpenPolicyNo"].ToString();
            TxtCRNoDate.Text = ds.Tables[0].Rows[0]["CreditNoDate"].ToString().Replace(" ", "-");
            txtPerfInvno.Text = ds.Tables[0].Rows[0]["PInvoiceNo"].ToString();
            txtPkgType.Text = ds.Tables[0].Rows[0]["Package"].ToString();
            txtContainer.Text = ds.Tables[0].Rows[0]["Container"].ToString();
            txtContinerNo.Text = ds.Tables[0].Rows[0]["ContainerNo"].ToString();
            txtSealNo.Text = ds.Tables[0].Rows[0]["SealNo"].ToString();
            txtPerfInvDate.Text = ds.Tables[0].Rows[0]["PInvoiceDate"].ToString().Replace(" ", "-");
            TxtLetterCreditNo.Text = ds.Tables[0].Rows[0]["CreditNo"].ToString();
            txtCommission.Text = ds.Tables[0].Rows[0]["Agent_Commission"].ToString();
            txtLicenseCommission.Text = ds.Tables[0].Rows[0]["License_Commission"].ToString();
            txtbanksubmissionDate.Text = ds.Tables[0].Rows[0]["banksubmissionDate"].ToString();
            txtBankSubmissionRefNo.Text = ds.Tables[0].Rows[0]["bankSubmissionRefNo"].ToString();
            txtFlightNo.Text = ds.Tables[0].Rows[0]["flightNo"].ToString();
            txtFlightDate.Text = ds.Tables[0].Rows[0]["FlightDate"].ToString().Replace(" ", "-");
            txtLEODate.Text = ds.Tables[0].Rows[0]["LeoDate"].ToString().Replace(" ", "-");
            lblTotalInvoiceAmt.Text = ds.Tables[0].Rows[0]["InvoiceAmount"].ToString();
            if (DDDocType.SelectedIndex <= 0)
            {
                DDDocType.Items.Add("Shipment");
                // DDDocType.SelectedItem.Text = "Shipment";
            }
            fillAmtGrids();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Masters/Packing/FrmPostShipment.aspx");
        }
        BtnSaveReAmt.Text = "Save Rec Amt.";
        BtnSaveDrRecAmt.Text = "Save Drawback Rec Amt.";
        txtInvDrRecAmt.Text = "";
        txtinvRecAmt.Text = "";
        txtRttno.Text = "";
    }
    protected void BtnSaveReAmt_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Visible = false;
        LblErrorMessage.Text = "";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            txtdate.Text = txtdate.Text == "" ? DateTime.Now.ToString() : txtdate.Text;
            SqlParameter[] _param = new SqlParameter[8];
            _param[0] = new SqlParameter("@InvoiceNo", DDInvoiceNo.SelectedValue);
            _param[1] = new SqlParameter("@Date", txtdate.Text);
            _param[2] = new SqlParameter("@RecAmt", txtinvRecAmt.Text);
            _param[3] = new SqlParameter("@RttNo", txtRttno.Text);
            _param[4] = new SqlParameter("@VarID", ViewState["varid"]);
            _param[5] = new SqlParameter("@Output", SqlDbType.NVarChar, 200);
            _param[5].Direction = ParameterDirection.Output;
            _param[6] = new SqlParameter("@Flag1", SqlDbType.Int);
            _param[6].Value = 0;
            if (BtnSaveReAmt.Text == "Save Rec Amt.")
            {
                _param[7] = new SqlParameter("@Flag", SqlDbType.Int);
                _param[7].Value = 0;
            }
            else if (BtnSaveReAmt.Text == "Modify Rec.Amt")
            {
                _param[7] = new SqlParameter("@Flag", SqlDbType.Int);
                _param[7].Value = 1;
            }
            if (Convert.ToDecimal(txtinvRecAmt.Text) != 0)
            {
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_SaveUpdateInvoiceAmtRec", _param);

                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = _param[5].Value.ToString();
                //Msg = _param[5].Value.ToString();
            }
        }
        catch (Exception ex)
        {

        }
        MessageSave(Msg);
        txtinvRecAmt.Text = "";
        txtRttno.Text = "";
        varid = 0;
        BtnSaveReAmt.Text = "Save Rec Amt.";
        fillAmtGrids();
    }
    public bool TotalAmountCheck()
    {
        Double varTotal = 0;
        // Dim i As Integer
        if (GDVAmtDetail2.Rows.Count > 0)
        {

            if (BtnSaveDrRecAmt.Text == "Save Drawback Rec Amt.")
            {
                for (int i = 0; i < GDVAmtDetail2.Rows.Count; i++)
                {
                    GridViewRow row = GDVAmtDetail2.Rows[i];
                    TextBox TxtGDVAMT = (TextBox)row.FindControl("TxtDBKRecAmt");
                    varTotal += Convert.ToDouble(TxtGDVAMT.Text);

                }
            }

            else if (BtnSaveDrRecAmt.Text == "Modify Drawback Rec.Amt")
            {
                for (int i = 0; i < GDVAmtDetail2.Rows.Count; i++)
                {
                    GridViewRow row = GDVAmtDetail2.Rows[i];
                    int itmid = Convert.ToInt32(GDVAmtDetail2.Rows[i].Cells[1].Text);
                    if (Convert.ToInt32(ViewState["varIdForDbk"].ToString()) != itmid)
                    {
                        TextBox TxtGDVAMT = (TextBox)row.FindControl("TxtDBKRecAmt");
                        varTotal += Convert.ToDouble(TxtGDVAMT.Text);
                    }

                }
            }

            varTotal += Convert.ToDouble(txtInvDrRecAmt.Text);
        }
        else
        {
            varTotal = Convert.ToDouble(txtInvDrRecAmt.Text);
        }

        if (Convert.ToDouble(txtDBKAmt.Text == "" ? "0" : txtDBKAmt.Text) < varTotal)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void BtnSaveDrRecAmt_Click(object sender, EventArgs e)
    {
         LblErrorMessage.Visible = false;
         LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            if (TotalAmountCheck() == true)
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "The Receive Amount Is Greater Than DBK Amount";
                return;

                //Msg = "The Receive Amount Is Greater Than DBK Amount";
                //MessageSave(Msg);
                //return;
            }
            txtInvDrRecAmtDate.Text = txtInvDrRecAmtDate.Text == "" ? DateTime.Now.ToString() : txtInvDrRecAmtDate.Text;
            SqlParameter[] _param = new SqlParameter[8];
            _param[0] = new SqlParameter("@InvoiceNo", DDInvoiceNo.SelectedValue);
            _param[1] = new SqlParameter("@Date", txtInvDrRecAmtDate.Text);
            _param[2] = new SqlParameter("@RecAmt", Convert.ToInt32(txtInvDrRecAmt.Text));
            _param[3] = new SqlParameter("@RttNo", txtRttno.Text);
            _param[4] = new SqlParameter("@VarID", ViewState["varIdForDbk"]);
            _param[5] = new SqlParameter("@Output", SqlDbType.NVarChar, 200);
            _param[5].Direction = ParameterDirection.Output;
            _param[6] = new SqlParameter("@Flag1", SqlDbType.Int);
            _param[6].Value = 1;
            if (BtnSaveDrRecAmt.Text == "Save Drawback Rec Amt.")
            {
                _param[7] = new SqlParameter("@Flag", SqlDbType.Int);
                _param[7].Value = 0;
            }
            else if (BtnSaveDrRecAmt.Text == "Modify Drawback Rec.Amt")
            {
                _param[7] = new SqlParameter("@Flag", SqlDbType.Int);
                _param[7].Value = 1;
            }
            if (Convert.ToDouble(txtInvDrRecAmt.Text) != 0)
            {
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_SaveUpdateInvoiceAmtRec", _param);
                Msg = _param[5].Value.ToString();
            }
            MessageSave(Msg);
            txtInvDrRecAmt.Text = "";
            varIdForDbk = 0;
            BtnSaveDrRecAmt.Text = "Save Drawback Rec Amt.";
            fillAmtGrids();
        }
        catch (Exception ex)
        {
            MessageSave(Msg);
            con.Close();
            con.Dispose();
        }
    }

    protected void GDVAmtDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (GDVAmtDetail.Rows.Count == 0)
            return;
        ViewState["varid"] = GDVAmtDetail.Rows[GDVAmtDetail.SelectedIndex].Cells[1].Text;
        //varid= Convert.ToInt32( GDVAmtDetail.Rows[GDVAmtDetail.SelectedIndex].Cells[1].Text);
        TextBox TxtGDVAMT = (TextBox)GDVAmtDetail.Rows[GDVAmtDetail.SelectedIndex].FindControl("TxtRecAmt");
        txtinvRecAmt.Text = TxtGDVAMT.Text;
        TextBox TxtGDVDT = (TextBox)GDVAmtDetail.Rows[GDVAmtDetail.SelectedIndex].FindControl("TxtRecDate");
        TextBox TxtGDVRTTNo = (TextBox)GDVAmtDetail.Rows[GDVAmtDetail.SelectedIndex].FindControl("TxtRTTNo");
        txtdate.Text = TxtGDVDT.Text;
        txtRttno.Text = TxtGDVRTTNo.Text;
        BtnSaveReAmt.Text = "Modify Rec.Amt";
    }
    protected void GDVAmtDetail2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (GDVAmtDetail2.Rows.Count == 0)
            return;
        ViewState["varIdForDbk"] = GDVAmtDetail2.Rows[GDVAmtDetail2.SelectedIndex].Cells[1].Text;
        // varIdForDbk = Convert.ToInt32( GDVAmtDetail2.Rows[GDVAmtDetail2.SelectedIndex].Cells[1].Text);
        TextBox TxtGDVDrAmt = (TextBox)GDVAmtDetail2.Rows[GDVAmtDetail2.SelectedIndex].FindControl("TxtDBKRecAmt");
        TextBox TxtGDVDrDate = (TextBox)GDVAmtDetail2.Rows[GDVAmtDetail2.SelectedIndex].FindControl("TxtDBKRecDate");
        txtInvDrRecAmt.Text = TxtGDVDrAmt.Text;
        txtInvDrRecAmtDate.Text = TxtGDVDrDate.Text;
        BtnSaveDrRecAmt.Text = "Modify Drawback Rec.Amt";
    }
    protected void GDVAmtDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GDVAmtDetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void GDVAmtDetail2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GDVAmtDetail2, "Select$" + e.Row.RowIndex);
        }
    }
    private void Refresh()
    {
        txtExRate.Text = "";
        txtshpBillNo.Text = "";
        TxtShpBillDate.Text = "";
        txtBlNo.Text = "";
        //txtBlDate.Text = IIf(IsDBNull(ds.Tables(0).Rows(0)("bldt")), Format(Date.Today, "dd-MMM-yyyy"), ds.Tables(0).Rows(0)("bldt"))
        txtBlDate.Text = "";
        txtvesselName.Text = "";
        txtCrdays.Text = "";
        txtBRCExrate.Text = "";
        txtDBKAmt.Text = "";
        // DDDocType.Text = "";
        txtFircno.Text = "";
        txtFIRCDate.Text = "";
        txtFormANo.Text = "";
        txtformADate.Text = "";
        txtpolicyno.Text = "";
        TxtCRNoDate.Text = "";
        txtPerfInvno.Text = "";
        txtPkgType.Text = "";
        txtContainer.Text = "";
        txtContinerNo.Text = "";
        txtSealNo.Text = "";
        TxtLetterCreditNo.Text = "";
        txtPerfInvDate.Text = "";
        DDDocType.Items.Clear();
        txtCommission.Text = "";
        txtLicenseCommission.Text = "";
        txtbanksubmissionDate.Text = "";
        txtBankSubmissionRefNo.Text = "";
        txtFlightNo.Text = "";
        txtFlightDate.Text = "";
        txtLEODate.Text = "";
    }
}
