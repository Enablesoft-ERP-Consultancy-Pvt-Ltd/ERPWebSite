using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Packing_FrmOtherDocs : System.Web.UI.Page
{
    string Msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            Clear();
            UtilityModule.ConditionalComboFill(ref DDcmpName, "select Distinct CI.CompanyId,CI.Companyname From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Companyname", true, "--Select");

            if (DDcmpName.Items.Count > 0)
            {
                DDcmpName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcmpName.Enabled = false;
            }

            UtilityModule.ConditionalComboFill(ref DDSession, "Select distinct Year,Session From Session Order by Year Desc", true, "--Select--");
        }
    }
    protected void LnkForeignDocBill_Click(object sender, EventArgs e)
    {
        PnlAnnexure1.Visible = false;
        PnlBankLetter.Visible = false;
        PnlBnkLetter2.Visible = false;
        PnlExportValDeclaration.Visible = false;
        PnlForeignDocBill.Visible = true;
        PnlGRRelease.Visible = false;
        PnlOriginalDoc.Visible = false;

    }

    protected void LnkBankLetter_Click(object sender, EventArgs e)
    {
        PnlAnnexure1.Visible = false;
        PnlBankLetter.Visible = true;
        PnlBnkLetter2.Visible = false;
        PnlExportValDeclaration.Visible = false;
        PnlForeignDocBill.Visible = false;
        PnlGRRelease.Visible = false;
        PnlOriginalDoc.Visible = false;



    }
    protected void LnkExpValDec_Click(object sender, EventArgs e)
    {
        PnlAnnexure1.Visible = false;
        PnlBankLetter.Visible = false;
        PnlBnkLetter2.Visible = false;
        PnlExportValDeclaration.Visible = true;
        PnlForeignDocBill.Visible = false;
        PnlGRRelease.Visible = false;
        PnlOriginalDoc.Visible = false;
    }
    protected void LnkAnxI_Click(object sender, EventArgs e)
    {
        PnlAnnexure1.Visible = true;
        PnlBankLetter.Visible = false;
        PnlBnkLetter2.Visible = false;
        PnlExportValDeclaration.Visible = false;
        PnlForeignDocBill.Visible = false;
        PnlGRRelease.Visible = false;
        PnlOriginalDoc.Visible = false;
    }
    protected void LnkBnkLetter2_Click(object sender, EventArgs e)
    {
        PnlAnnexure1.Visible = false;
        PnlBankLetter.Visible = false;
        PnlBnkLetter2.Visible = true;
        PnlExportValDeclaration.Visible = false;
        PnlForeignDocBill.Visible = false;
        PnlGRRelease.Visible = false;
        PnlOriginalDoc.Visible = false;
    }
    protected void LnkGR_Click(object sender, EventArgs e)
    {
        PnlAnnexure1.Visible = false;
        PnlBankLetter.Visible = false;
        PnlBnkLetter2.Visible = false;
        PnlExportValDeclaration.Visible = false;
        PnlForeignDocBill.Visible = false;
        PnlGRRelease.Visible = true;
        PnlOriginalDoc.Visible = false;
    }
    protected void LnkOrigDoc_Click(object sender, EventArgs e)
    {
        PnlAnnexure1.Visible = false;
        PnlBankLetter.Visible = false;
        PnlBnkLetter2.Visible = false;
        PnlExportValDeclaration.Visible = false;
        PnlForeignDocBill.Visible = false;
        PnlGRRelease.Visible = false;
        PnlOriginalDoc.Visible = true;
    }

    private void Fill_ForeignDocBill()
    {
        string Strsql;

        TxtORGBl.Text = "";
        TxtORGCertAnalysis.Text = "";
        TxtOrgDraft.Text = "";
        TxtORGCertofOrigin.Text = "";
        TxtORGCunsInv.Text = "";
        TxtORGCustinvoice.Text = "";
        TxtORGInspOlley.Text = "";
        TxtOrgOthDoc.Text = "";
        TxtOrgOthDoc.Text = "";
        TxtORGPackList.Text = "";
        TxtORGSCD.Text = "";
        TxtORGWeight.Text = "";
        TxtDupBL.Text = "";
        TxtDupCertAnalysis.Text = "";
        TxtDupCertofOrigin.Text = "";
        TxtDupCunsInv.Text = "";
        TxtDupCustinvoice.Text = "";
        TxtDupDraft.Text = "";
        TxtDupInspOlley.Text = "";
        TxtDupOthDoc.Text = "";
        TxtDupPackList.Text = "";
        TxtDupSCD.Text = "";
        TxtDupWeight.Text = "";
        DDDoctype.SelectedIndex = -1;
        //  CmbDocumentType.SelectedIndex = -1;
        if (DDInvoiceNo.SelectedIndex != -1)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

//            Strsql = @"Select Ci.Companyname,isnull(InvoiceAmount,0) InvoiceAmount,isnull(TotalRolls,0) NoOfRolls ,sum(cast(p.Totalpcs as int) * cast(p.TotalArea as int)) as  Area,
//            Sum(cast(TotalPcs as int)) Pcs from CustomerInfo Ci,Invoice I,v_packingmaster P Where P.Packingid=I.Packingid and Ci.CustomerId=I.Cosigneeid 
//            and I.Packingid=" + DDInvoiceNo.SelectedValue + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " group by InvoiceAmount,TotalRolls,Ci.Companyname ";

            Strsql = @"Select Ci.Companyname,isnull(InvoiceAmount,0) InvoiceAmount,isnull(sum(PI.TotalRoll),0) NoOfRolls ,sum((PI.Area)) as  Area,
            Sum(cast(PI.TotalPcs as int)) Pcs From CustomerInfo Ci JOIN Invoice I ON Ci.CustomerId=I.Cosigneeid 
			JOIN PackingInformation PI ON PI.Packingid=I.Packingid  
            Where PI.Packingid=" + DDInvoiceNo.SelectedValue + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " group by InvoiceAmount,Ci.Companyname ";

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Div1.Visible = true;
                LblPcs1.Text = ds.Tables[0].Rows[0]["PCs"].ToString();
                LblArea1.Text = ds.Tables[0].Rows[0]["Area"].ToString();
                LblAmt1.Text = ds.Tables[0].Rows[0]["InvoiceAmount"].ToString();
                LblRolls1.Text = ds.Tables[0].Rows[0]["NoOfRolls"].ToString();

            }
            else
            {
                LblPcs1.Text = "";
                LblArea1.Text = "";
                LblAmt1.Text = "";
                LblRolls1.Text = "";
                Div1.Visible = false;
            }
            Strsql = @"Select Packingid,DocumentType,Replace( convert(nvarchar(11),DocDate,106),' ','-') DocDate,DraftsOri,DraftsDup,CustInVoiceOri,CustInvoiceDup,CunsInvoiceOri,
                    CunsInvoiceDup,CertofOriginOri,CertOfOriginDup,WeightNoteOri,WeightNoteDup,InspOlleycertOri,InspOlleyCertDup,BLOri,BLDup,CertAnalysisOri,CertAnalysisDup,
                    PackListori,PackListDup,OtherDocumentOri,OtherDocumentDup,SCDOri,SCDDup from ForeignDocumentryBill where Packingid=" + DDInvoiceNo.SelectedValue;
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtDocDate.Text = ds.Tables[0].Rows[0]["DocDate"].ToString();
                TxtORGBl.Text = ds.Tables[0].Rows[0]["BLDup"].ToString();
                TxtDupBL.Text = ds.Tables[0].Rows[0]["BLOri"].ToString();
                TxtOrgDraft.Text = ds.Tables[0].Rows[0]["DraftsDup"].ToString();
                TxtDupDraft.Text = ds.Tables[0].Rows[0]["DraftsOri"].ToString();
                TxtDupCunsInv.Text = ds.Tables[0].Rows[0]["CunsInvoiceDup"].ToString();
                TxtORGCunsInv.Text = ds.Tables[0].Rows[0]["CunsInvoiceOri"].ToString();
                TxtDupCustinvoice.Text = ds.Tables[0].Rows[0]["CustInvoiceDup"].ToString();
                TxtORGCustinvoice.Text = ds.Tables[0].Rows[0]["CustInVoiceOri"].ToString();
                TxtDupCertAnalysis.Text = ds.Tables[0].Rows[0]["CertAnalysisDup"].ToString();
                TxtORGCertAnalysis.Text = ds.Tables[0].Rows[0]["CertAnalysisOri"].ToString();
                TxtDupCertofOrigin.Text = ds.Tables[0].Rows[0]["CertOfOriginDup"].ToString();
                TxtORGCertofOrigin.Text = ds.Tables[0].Rows[0]["CertofOriginOri"].ToString();
                TxtDupPackList.Text = ds.Tables[0].Rows[0]["PackListDup"].ToString();
                TxtORGPackList.Text = ds.Tables[0].Rows[0]["PackListori"].ToString();
                TxtDupOthDoc.Text = ds.Tables[0].Rows[0]["OtherDocumentDup"].ToString();
                TxtOrgOthDoc.Text = ds.Tables[0].Rows[0]["OtherDocumentOri"].ToString();
                TxtDupInspOlley.Text = ds.Tables[0].Rows[0]["InspOlleyCertDup"].ToString();
                TxtORGInspOlley.Text = ds.Tables[0].Rows[0]["InspOlleycertOri"].ToString();
                TxtDupWeight.Text = ds.Tables[0].Rows[0]["WeightNoteDup"].ToString();
                TxtORGWeight.Text = ds.Tables[0].Rows[0]["WeightNoteOri"].ToString();
                TxtORGSCD.Text = ds.Tables[0].Rows[0]["ScdOri"].ToString();
                TxtDupSCD.Text = ds.Tables[0].Rows[0]["ScdDup"].ToString();
            }
            else
            {
                LblPcs1.Text = "";
                LblArea1.Text = "";
                LblAmt1.Text = "";
                LblRolls1.Text = "";
                Div1.Visible = false;
                //LblConsignee.Text = "";
                txtDocDate.Text = DateTime.Now.ToShortDateString();
            }
        }
    }

    protected void Btnsave_Click(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SaveBankLetter(Tran);
            SaveForeignDocBill(Tran);
            SaveExportValueDeclaration(Tran);
            SaveAnnexure1(Tran);
            SaveBankLetter1(Tran);
            SaveGRrelease(Tran);
            SaveOriginalDoc(Tran);

            Clear();
            Tran.Commit();
            ViewState["InvoiceId"] = DDInvoiceNo.SelectedValue;
            Msg = "Record(s) has been saved!";
            MessageSave(Msg);
        }



        catch (Exception ex)
        {
            Tran.Rollback();
            MessageSave(ex.Message);
        }

    }
    protected void DDSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = @" Select PM.Packingid,PM.Tinvoiceno+space(2)+'Dated'+space(2)+Convert(nvarchar,Packingdate,106) From Packing PM,   
                       Invoice I Where PM.PackingID=I.InvoiceId  AND PM.ConsignorID=" + DDcmpName.SelectedValue + " AND  I.InvoiceYear=" + DDSession.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        UtilityModule.ConditionalComboFill(ref DDInvoiceNo, Str, true, "--Select--");

    }
    private void Fill_BankLetter()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string Strsql;
        string Strsql2;
        TxtPBLInvno.Text = "";
        TxtSpecificationList.Text = "";
        TxtPBLBillExchange.Text = "";
        TxtPBLBillno.Text = "";
        TxtPBLSBillno.Text = "";
        TxtPBLSdfAppendix.Text = "";
        txtPBLExchCtrlCopy.Text = "";
        TxtPBLSingContDec.Text = "";
        Strsql = @"select Packingid,Replace(convert(nvarchar(11),DocDate,106),' ','-') DocDate,Invoice,SpecificationList,BillOfExch,BLNo,SBillNo,FormSDF,ExchangeCont,
                   SingleCountryDec from bankletter where packingid=" + DDInvoiceNo.SelectedValue;
        DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Strsql);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            TxtPBLDocDate.Text = Ds.Tables[0].Rows[0]["DocDate"].ToString();
            TxtPBLInvno.Text = Ds.Tables[0].Rows[0]["Invoice"].ToString();
            TxtSpecificationList.Text = Ds.Tables[0].Rows[0]["SpecificationList"].ToString();
            TxtPBLBillExchange.Text = Ds.Tables[0].Rows[0]["BillOfExch"].ToString();
            TxtPBLBillno.Text = Ds.Tables[0].Rows[0]["BLNo"].ToString();
            TxtPBLSBillno.Text = Ds.Tables[0].Rows[0]["SBillNo"].ToString();
            TxtPBLSdfAppendix.Text = Ds.Tables[0].Rows[0]["FormSDF"].ToString();
            txtPBLExchCtrlCopy.Text = Ds.Tables[0].Rows[0]["ExchangeCont"].ToString();
            TxtPBLSingContDec.Text = Ds.Tables[0].Rows[0]["SingleCountryDec"].ToString();
        }
        else
        {
            TxtPBLDocDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            Strsql2 = @"select TInvoiceNo,isnull(sbillno,'') as sbillNo,isnull(BLNo,'') as BLNo from Invoice where InvoiceId=" + DDInvoiceNo.SelectedValue;
            DataSet Ds2 = SqlHelper.ExecuteDataset(con, CommandType.Text, Strsql2);
            if (Ds2.Tables[0].Rows.Count > 0)
            {
                TxtPBLInvno.Text = Ds2.Tables[0].Rows[0]["TInvoiceNo"].ToString();
                TxtPBLBillno.Text = Ds2.Tables[0].Rows[0]["BLNo"].ToString();
                TxtPBLSBillno.Text = Ds2.Tables[0].Rows[0]["SBillNo"].ToString();
            }
        }
    }
    private void Fill_ExportValueDeclaration()
    {
        string Strsql;

        TxtPlace.Text = "";
        PnlETxtdocdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        Strsql = @"Select Packingid,SaleonConsBas,WhetSelByRel,IfYes,Replace( convert(nvarchar(11),Docdate,106),' ','-') Docdate,Place From ExportvalueDeclaration Where Packingid=" + DDInvoiceNo.SelectedValue;
        SqlDataReader dr = SqlHelper.ExecuteReader(con, CommandType.Text, Strsql);
        if (dr.Read())
        {
            if (dr["SaleonConsBas"].ToString() == "1")
            {
                DDConsSale.SelectedIndex = 1;
            }
            else if (dr["SaleonConsBas"].ToString() == "2")
            {
                DDConsSale.SelectedIndex = 2;
            }
            if (dr["WhetSelByRel"].ToString() == "1")
            {
                DDSeller.SelectedIndex = 1;
            }
            else if (dr["WhetSelByRel"].ToString() == "2")
            {
                DDSeller.SelectedIndex = 2;
            }
            if (dr["IfYes"].ToString() == "1")
            {
                DDPrice.SelectedIndex = 1;
            }
            else if (dr["IfYes"].ToString() == "2")
            {
                DDPrice.SelectedIndex = 2;
            }

            PnlETxtdocdate.Text = dr["Docdate"].ToString();
            TxtPlace.Text = dr["Place"].ToString();
        }
        dr.Close();
    }
    private void Fill_BankLetter1()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string Strsql;
        TxtBLDrafts.Text = "";
        TxtBLDupDrafts.Text = "";
        TxtBLInv.Text = "";
        TxtBLDupInv.Text = "";
        TxtBLPackList.Text = "";
        TxtBLDupPackList.Text = "";
        TxtBLBill.Text = "";
        TxtBLDupBill.Text = "";
        TxtBLAWB.Text = "";
        TxtBLDupAWB.Text = "";
        TxtBLCertOrigin.Text = "";
        TxtBLDupCertOrigin.Text = "";
        TxtBLInspol.Text = "";
        TxtBLDupInspol.Text = "";
        TxtBLSCD.Text = "";
        TxtBLDupSCD.Text = "";
        TxtBLOthDoc.Text = "";
        TxtBLDupOthDoc.Text = "";
        Strsql = @"Select Packingid,Replace( convert(nvarchar(11),DocDate,106),' ','-') DocDate,DraftsO,DragtsD,InvoiceO,InvoiceD,PListO,PlistD,BLO,BLD,AWBO,AWBD,CertOriginO,
                   CertoriginD,InsPoloO,InsPoloD,SCDO,SCDD,OtherDocO,OtherDocD from BankLetter1 Where Packingid=" + DDInvoiceNo.SelectedValue;
        SqlDataReader Dr = SqlHelper.ExecuteReader(con, CommandType.Text, Strsql);
        if (Dr.Read())
        {
            TxtBLDrafts.Text = Dr["DraftsO"].ToString();
            TxtBLDupDrafts.Text = Dr["DragtsD"].ToString();
            TxtBLInv.Text = Dr["InvoiceO"].ToString();
            TxtBLDupInv.Text = Dr["InvoiceD"].ToString();
            TxtBLPackList.Text = Dr["PListO"].ToString();
            TxtBLDupPackList.Text = Dr["PlistD"].ToString();
            TxtBLBill.Text = Dr["BLO"].ToString();
            TxtBLDupBill.Text = Dr["BLD"].ToString();
            TxtBLAWB.Text = Dr["AWBO"].ToString();
            TxtBLDupAWB.Text = Dr["AWBD"].ToString();
            TxtBLCertOrigin.Text = Dr["CertOriginO"].ToString();
            TxtBLDupCertOrigin.Text = Dr["CertoriginD"].ToString();
            TxtBLInspol.Text = Dr["InsPoloO"].ToString();
            TxtBLDupInspol.Text = Dr["InsPoloD"].ToString();
            TxtBLSCD.Text = Dr["SCDO"].ToString();
            TxtBLDupSCD.Text = Dr["SCDD"].ToString();
            TxtBLOthDoc.Text = Dr["OtherDocO"].ToString();
            TxtBLDupOthDoc.Text = Dr["OtherDocD"].ToString();
            TxtPBL2DocDate.Text = Dr["DocDate"].ToString();
        }
        else
        {
            TxtPBL2DocDate.Text = DateTime.Now.ToShortDateString();
        }

        Dr.Close();
    }

    private void Fill_GRrelease()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string Strsql;
        // Dim Ds As New Data.DataSet
        TxtPGRInv.Text = "";
        TxtPGRAWB.Text = "";
        TxtPGRSDF.Text = "";
        TxtPGRExchangeControl.Text = "";
        TxtPGRTTNo.Text = "";
        Strsql = "select Packingid,InvoiceNo,AWB,SDFform,ExControl,RTTNo,Replace( convert(nvarchar(11),RTTDate,106),' ','-') RTTDate from GRrelease where packingid=" + DDInvoiceNo.SelectedValue;
        DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Strsql);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            TxtPGRInv.Text = Ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
            TxtPGRAWB.Text = Ds.Tables[0].Rows[0]["AWB"].ToString();
            TxtPGRSDF.Text = Ds.Tables[0].Rows[0]["SDFform"].ToString();
            TxtPGRExchangeControl.Text = Ds.Tables[0].Rows[0]["Excontrol"].ToString();
            TxtPGRTTNo.Text = Ds.Tables[0].Rows[0]["RTTNo"].ToString();
            TxtPGRTTDate.Text = Ds.Tables[0].Rows[0]["RTTdate"].ToString();
        }
    }
    private void SaveGRrelease(SqlTransaction Tran)
    {
        TxtPGRTTDate.Text = TxtPGRTTDate.Text == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : TxtPGRTTDate.Text;
        SqlParameter[] _Param = new SqlParameter[7];

        _Param[0] = new SqlParameter("@Packingid", SqlDbType.Int);
        _Param[1] = new SqlParameter("@InvoiceNo", SqlDbType.NVarChar, 100);
        _Param[2] = new SqlParameter("@AWB", SqlDbType.NVarChar, 100);
        _Param[3] = new SqlParameter("@SDFform", SqlDbType.NVarChar, 100);
        _Param[4] = new SqlParameter("@ExControl", SqlDbType.NVarChar, 100);
        _Param[5] = new SqlParameter("@RTTNo", SqlDbType.NVarChar, 100);
        _Param[6] = new SqlParameter("@RTTDate", SqlDbType.DateTime);





        _Param[0].Value = DDInvoiceNo.SelectedValue;
        _Param[1].Value = TxtPGRInv.Text;
        _Param[2].Value = TxtPGRAWB.Text;
        _Param[3].Value = TxtPGRSDF.Text;
        _Param[4].Value = TxtPGRExchangeControl.Text;
        _Param[5].Value = TxtPGRTTNo.Text;
        _Param[6].Value = TxtPGRTTDate.Text;
        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveGRrelease", _Param);

    }
    private void OriginalDoc()
    {
        string Strsql;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        TxtOriginalDoc.Text = "";
        TxtPkgList.Text = "";
        TxtCourierCo.Text = "";
        TxtAWBNo.Text = "";

        TxtPnlOrigDocDate.Text = "";
        Strsql = "select  * from Originaldoc where packingid=" + DDInvoiceNo.SelectedValue;
        DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Strsql);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            TxtOriginalDoc.Text = Ds.Tables[0].Rows[0]["InvoiceCopy"].ToString();
            TxtPkgList.Text = Ds.Tables[0].Rows[0]["packinglist"].ToString();
            TxtCourierCo.Text = Ds.Tables[0].Rows[0]["courierco"].ToString();
            TxtAWBNo.Text = Ds.Tables[0].Rows[0]["AWBNO"].ToString();
            TxtPnlOrigDocDate.Text = Ds.Tables[0].Rows[0]["date"].ToString();
        }
        else
        {
            TxtPnlOrigDocDate.Text = DateTime.Now.ToShortDateString();
        }
    }
    private void SaveOriginalDoc(SqlTransaction Tran)
    {
        TxtPnlOrigDocDate.Text = TxtPnlOrigDocDate.Text == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : TxtPnlOrigDocDate.Text;
        SqlParameter[] _Param = new SqlParameter[6];
        _Param[0] = new SqlParameter("@PackingID", SqlDbType.Int);
        _Param[1] = new SqlParameter("@InvoiceCopy", SqlDbType.NVarChar, 100);
        _Param[2] = new SqlParameter("@PackingList", SqlDbType.NVarChar, 100);
        _Param[3] = new SqlParameter("@CourierCo", SqlDbType.NVarChar, 100);
        _Param[4] = new SqlParameter("@AWBNO", SqlDbType.NVarChar, 100);
        _Param[5] = new SqlParameter("@DATE", SqlDbType.DateTime);

        _Param[0].Value = DDInvoiceNo.SelectedValue;
        _Param[1].Value = TxtOriginalDoc.Text;
        _Param[2].Value = TxtPkgList.Text;
        _Param[3].Value = TxtCourierCo.Text;
        _Param[4].Value = TxtAWBNo.Text;
        _Param[5].Value = TxtPnlOrigDocDate.Text;
        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveOriginalDoc", _Param);

    }
    private void SaveBankLetter(SqlTransaction Tran)
    {
        TxtPBLDocDate.Text = TxtPBLDocDate.Text == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : TxtPBLDocDate.Text;
        SqlParameter[] _Param = new SqlParameter[10];

        _Param[0] = new SqlParameter("@InvoiceNo", SqlDbType.NVarChar, 50);
        _Param[1] = new SqlParameter("@SpecificationList", SqlDbType.NVarChar, 50);
        _Param[2] = new SqlParameter("@BillExchange", SqlDbType.NVarChar, 20);
        _Param[3] = new SqlParameter("@Billno", SqlDbType.NVarChar, 20);
        _Param[4] = new SqlParameter("@SBillno", SqlDbType.NVarChar, 20);
        _Param[5] = new SqlParameter("@SdfAppendix", SqlDbType.NVarChar, 30);
        _Param[6] = new SqlParameter("@ExchCtrlCopy", SqlDbType.NVarChar, 50);
        _Param[7] = new SqlParameter("@SingleContDesc", SqlDbType.NVarChar, 50);
        _Param[8] = new SqlParameter("@DocDate", SqlDbType.DateTime);
        _Param[9] = new SqlParameter("@DDInvoiceNo", SqlDbType.Int);



        _Param[0].Value = TxtPBLInvno.Text.Trim();
        _Param[1].Value = TxtSpecificationList.Text.Trim();
        _Param[2].Value = TxtPBLBillExchange.Text.Trim();
        _Param[3].Value = TxtPBLBillno.Text.Trim();
        _Param[4].Value = TxtPBLSBillno.Text.Trim();
        _Param[5].Value = TxtPBLSdfAppendix.Text.Trim();
        _Param[6].Value = txtPBLExchCtrlCopy.Text.Trim();
        _Param[7].Value = TxtPBLSingContDec.Text.Trim();
        _Param[8].Value = TxtPBLDocDate.Text;
        _Param[9].Value = DDInvoiceNo.SelectedValue;
        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveBankLetter", _Param);


    }

    private void SaveForeignDocBill(SqlTransaction Tran)
    {
        txtDocDate.Text = txtDocDate.Text == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : txtDocDate.Text;
        SqlParameter[] _Param = new SqlParameter[25];
        _Param[0] = new SqlParameter("@InvoiceNo", SqlDbType.Int);
        _Param[1] = new SqlParameter("@DocumentType", SqlDbType.NVarChar, 20);
        _Param[2] = new SqlParameter("@DocDate", SqlDbType.SmallDateTime);
        _Param[3] = new SqlParameter("@DraftsOri", SqlDbType.NVarChar, 100);
        _Param[4] = new SqlParameter("@DraftsDup", SqlDbType.NVarChar, 100);
        _Param[5] = new SqlParameter("@CustInVoiceOri", SqlDbType.NVarChar, 100);
        _Param[6] = new SqlParameter("@CustInvoiceDup", SqlDbType.NVarChar, 100);
        _Param[7] = new SqlParameter("@CunsInvoiceOri", SqlDbType.NVarChar, 50);
        _Param[8] = new SqlParameter("@CunsInvoiceDup", SqlDbType.NVarChar, 50);
        _Param[9] = new SqlParameter("@CertofOriginOri", SqlDbType.NVarChar, 100);
        _Param[10] = new SqlParameter("@CertOfOriginDup", SqlDbType.NVarChar, 100);
        _Param[11] = new SqlParameter("@WeightNoteOri", SqlDbType.NVarChar, 100);
        _Param[12] = new SqlParameter("@WeightNoteDup", SqlDbType.NVarChar, 100);
        _Param[13] = new SqlParameter("@InspOlleycertOri", SqlDbType.NVarChar, 100);
        _Param[14] = new SqlParameter("@InspOlleyCertDup", SqlDbType.NVarChar, 100);
        _Param[15] = new SqlParameter("@BLOri", SqlDbType.NVarChar, 100);
        _Param[16] = new SqlParameter("@BLDup", SqlDbType.NVarChar, 100);
        _Param[17] = new SqlParameter("@CertAnalysisOri", SqlDbType.NVarChar, 100);
        _Param[18] = new SqlParameter("@CertAnalysisDup", SqlDbType.NVarChar, 100);
        _Param[19] = new SqlParameter("@PackListori", SqlDbType.NVarChar, 100);
        _Param[20] = new SqlParameter("@PackListDup", SqlDbType.NVarChar, 100);
        _Param[21] = new SqlParameter("@OtherDocumentOri", SqlDbType.NVarChar, 100);
        _Param[22] = new SqlParameter("@OtherDocumentDup", SqlDbType.NVarChar, 100);
        _Param[23] = new SqlParameter("@SCDOri", SqlDbType.NVarChar, 100);
        _Param[24] = new SqlParameter("@SCDDup", SqlDbType.NVarChar, 100);




        _Param[0].Value = DDInvoiceNo.SelectedValue;
        _Param[1].Value = DDDoctype.SelectedValue;
        _Param[2].Value = txtDocDate.Text;
        _Param[3].Value = TxtOrgDraft.Text;
        _Param[4].Value = TxtDupDraft.Text;
        _Param[5].Value = TxtORGCustinvoice.Text;
        _Param[6].Value = TxtDupCustinvoice.Text;
        _Param[7].Value = TxtORGCunsInv.Text;
        _Param[8].Value = TxtDupCunsInv.Text;
        _Param[9].Value = TxtORGCertofOrigin.Text;
        _Param[10].Value = TxtDupCertofOrigin.Text;
        _Param[11].Value = TxtORGWeight.Text;
        _Param[12].Value = TxtDupWeight.Text;
        _Param[13].Value = TxtORGInspOlley.Text;
        _Param[14].Value = TxtDupInspOlley.Text;
        _Param[15].Value = TxtORGBl.Text;
        _Param[16].Value = TxtDupBL.Text;
        _Param[17].Value = TxtORGCertAnalysis.Text;
        _Param[18].Value = TxtDupCertAnalysis.Text;
        _Param[19].Value = TxtORGPackList.Text;
        _Param[20].Value = TxtDupPackList.Text;
        _Param[21].Value = TxtOrgOthDoc.Text;
        _Param[22].Value = TxtDupOthDoc.Text;
        _Param[23].Value = TxtORGSCD.Text;
        _Param[24].Value = TxtDupSCD.Text;
        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveForeignDocBill", _Param);


    }
    private void SaveAnnexure1(SqlTransaction Tran)
    {
        txtDocDate.Text = txtDocDate.Text == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : txtDocDate.Text;
        SqlParameter[] _Param = new SqlParameter[4];

        _Param[0] = new SqlParameter("@InvoiceNo", SqlDbType.Int);
        _Param[1] = new SqlParameter("@Range", SqlDbType.NVarChar, 200);
        _Param[2] = new SqlParameter("@Div", SqlDbType.NVarChar, 200);
        _Param[3] = new SqlParameter("@Commisionorate", SqlDbType.NVarChar, 200);


        _Param[0].Value = DDInvoiceNo.SelectedValue;
        _Param[1].Value = TxtPAnxRange.Text;
        _Param[2].Value = TxtPAnxDivision.Text;
        _Param[3].Value = TxtPANXCommision.Text;
        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveAnnexure1", _Param);

    }
    private void SaveBankLetter1(SqlTransaction Tran)
    {
        TxtPBL2DocDate.Text = TxtPBL2DocDate.Text == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : TxtPBL2DocDate.Text;
        SqlParameter[] _Param = new SqlParameter[20];
        _Param[0] = new SqlParameter("@InvoiceNo", SqlDbType.Int);
        _Param[1] = new SqlParameter("@DocDate", SqlDbType.SmallDateTime);
        _Param[2] = new SqlParameter("@DraftsO", SqlDbType.NVarChar, 100);
        _Param[3] = new SqlParameter("@DragtsD", SqlDbType.NVarChar, 100);
        _Param[4] = new SqlParameter("@InvoiceO", SqlDbType.NVarChar, 100);
        _Param[5] = new SqlParameter("@InvoiceD", SqlDbType.NVarChar, 100);
        _Param[6] = new SqlParameter("@PListO", SqlDbType.NVarChar, 100);
        _Param[7] = new SqlParameter("@PlistD", SqlDbType.NVarChar, 100);
        _Param[8] = new SqlParameter("@BLO", SqlDbType.NVarChar, 100);
        _Param[9] = new SqlParameter("@BLD", SqlDbType.NVarChar, 100);
        _Param[10] = new SqlParameter("@AWBO", SqlDbType.NVarChar, 100);
        _Param[11] = new SqlParameter("@AWBD", SqlDbType.NVarChar, 100);
        _Param[12] = new SqlParameter("@CertOriginO", SqlDbType.NVarChar, 100);
        _Param[13] = new SqlParameter("@CertoriginD", SqlDbType.NVarChar, 100);
        _Param[14] = new SqlParameter("@InsPoloO", SqlDbType.NVarChar, 100);
        _Param[15] = new SqlParameter("@InsPoloD", SqlDbType.NVarChar, 100);
        _Param[16] = new SqlParameter("@SCDO", SqlDbType.NVarChar, 100);
        _Param[17] = new SqlParameter("@SCDD", SqlDbType.NVarChar, 100);
        _Param[18] = new SqlParameter("@OtherDocO", SqlDbType.NVarChar, 100);
        _Param[19] = new SqlParameter("@OtherDocD", SqlDbType.NVarChar, 100);





        _Param[0].Value = DDInvoiceNo.SelectedValue;
        _Param[1].Value = TxtPBL2DocDate.Text;
        _Param[2].Value = TxtBLDrafts.Text;
        _Param[3].Value = TxtBLDupDrafts.Text;
        _Param[4].Value = TxtBLInv.Text;
        _Param[5].Value = TxtBLDupInv.Text;
        _Param[6].Value = TxtBLPackList.Text;
        _Param[7].Value = TxtBLDupPackList.Text;
        _Param[8].Value = TxtBLBill.Text;
        _Param[9].Value = TxtBLDupBill.Text;
        _Param[10].Value = TxtBLAWB.Text;
        _Param[11].Value = TxtBLDupAWB.Text;
        _Param[12].Value = TxtBLCertOrigin.Text;
        _Param[13].Value = TxtBLDupCertOrigin.Text;
        _Param[14].Value = TxtBLInspol.Text;
        _Param[15].Value = TxtBLDupInspol.Text;
        _Param[16].Value = TxtBLSCD.Text;
        _Param[17].Value = TxtBLDupSCD.Text;
        _Param[18].Value = TxtBLOthDoc.Text;
        _Param[19].Value = TxtBLDupOthDoc.Text;

        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveBankLetter1", _Param);

        TxtBLDrafts.Text = "";
        TxtBLDupDrafts.Text = "";
        TxtBLInv.Text = "";
        TxtBLDupInv.Text = "";
        TxtBLPackList.Text = "";
        TxtBLDupPackList.Text = "";
        TxtBLBill.Text = "";
        TxtBLDupBill.Text = "";
        TxtBLAWB.Text = "";
        TxtBLDupAWB.Text = "";
        TxtBLCertOrigin.Text = "";
        TxtBLDupCertOrigin.Text = "";
        TxtBLInspol.Text = "";
        TxtBLDupInspol.Text = "";
        TxtBLSCD.Text = "";
        TxtBLDupSCD.Text = "";
        TxtBLOthDoc.Text = "";
        TxtBLDupOthDoc.Text = "";
    }
    private void SaveExportValueDeclaration(SqlTransaction Tran)
    {
        PnlETxtdocdate.Text = PnlETxtdocdate.Text == "" ? DateTime.Now.ToString() : PnlETxtdocdate.Text;
        SqlParameter[] _Param = new SqlParameter[6];
        _Param[0] = new SqlParameter("@PackingID", SqlDbType.Int);
        _Param[1] = new SqlParameter("@SaleonConsBas", SqlDbType.NVarChar, 100);
        _Param[2] = new SqlParameter("@WhetSelByRel", SqlDbType.NVarChar, 100);
        _Param[3] = new SqlParameter("@IfYes", SqlDbType.NVarChar, 100);
        _Param[4] = new SqlParameter("@Docdate", SqlDbType.SmallDateTime);
        _Param[5] = new SqlParameter("@Place", SqlDbType.NVarChar, 100);



        _Param[0].Value = DDInvoiceNo.SelectedValue;
        _Param[1].Value = DDConsSale.SelectedValue;
        _Param[2].Value = DDSeller.SelectedValue;
        _Param[3].Value = DDPrice.SelectedValue;
        _Param[4].Value = PnlETxtdocdate.Text;
        _Param[5].Value = TxtPlace.Text;



        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveExportValueDeclaration", _Param);

    }

    private void Clear()
    {
        // clear foreighn doc bill
        TxtORGBl.Text = "";
        TxtORGCertAnalysis.Text = "";
        TxtOrgDraft.Text = "";
        TxtORGCertofOrigin.Text = "";
        TxtORGCunsInv.Text = "";
        TxtORGCustinvoice.Text = "";
        TxtORGInspOlley.Text = "";
        TxtOrgOthDoc.Text = "";
        TxtOrgOthDoc.Text = "";
        TxtORGPackList.Text = "";
        TxtORGSCD.Text = "";
        TxtORGWeight.Text = "";
        TxtDupBL.Text = "";
        TxtDupCertAnalysis.Text = "";
        TxtDupCertofOrigin.Text = "";
        TxtDupCunsInv.Text = "";
        TxtDupCustinvoice.Text = "";
        TxtDupDraft.Text = "";
        TxtDupInspOlley.Text = "";
        TxtDupOthDoc.Text = "";
        TxtDupPackList.Text = "";
        TxtDupSCD.Text = "";
        TxtDupWeight.Text = "";
        DDDoctype.SelectedIndex = -1;
        // clear bank letter
        TxtPBLInvno.Text = "";
        TxtSpecificationList.Text = "";
        TxtPBLBillExchange.Text = "";
        TxtPBLBillno.Text = "";
        TxtPBLSBillno.Text = "";
        TxtPBLSdfAppendix.Text = "";
        txtPBLExchCtrlCopy.Text = "";
        TxtPBLSingContDec.Text = "";
        //Clear bankletter1
        TxtBLDrafts.Text = "";
        TxtBLDupDrafts.Text = "";
        TxtBLInv.Text = "";
        TxtBLDupInv.Text = "";
        TxtBLPackList.Text = "";
        TxtBLDupPackList.Text = "";
        TxtBLBill.Text = "";
        TxtBLDupBill.Text = "";
        TxtBLAWB.Text = "";
        TxtBLDupAWB.Text = "";
        TxtBLCertOrigin.Text = "";
        TxtBLDupCertOrigin.Text = "";
        TxtBLInspol.Text = "";
        TxtBLDupInspol.Text = "";
        TxtBLSCD.Text = "";
        TxtBLDupSCD.Text = "";
        TxtBLOthDoc.Text = "";
        TxtBLDupOthDoc.Text = "";
        // clear GRRelease
        TxtPGRInv.Text = "";
        TxtPGRAWB.Text = "";
        TxtPGRSDF.Text = "";
        TxtPGRExchangeControl.Text = "";
        TxtPGRTTNo.Text = "";
        //clear OriginalDoc
        TxtOriginalDoc.Text = "";
        TxtPkgList.Text = "";
        TxtCourierCo.Text = "";
        TxtAWBNo.Text = "";
        TxtPnlOrigDocDate.Text = "";
        Div1.Visible = false;
        DDConsSale.SelectedIndex = 0;
        DDSeller.SelectedIndex = 0;
        DDPrice.SelectedIndex = 0;
        PnlETxtdocdate.Text = "";
        TxtPlace.Text = "";
        TxtPAnxRange.Text = "";
        TxtPAnxDivision.Text = "";
        TxtPANXCommision.Text = "";

    }
    protected void DDInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Fill_ForeignDocBill();
            Fill_BankLetter();
            Fill_BankLetter1();
            Fill_GRrelease();
            Fill_ExportValueDeclaration();
            ViewState["InvoiceId"] = DDInvoiceNo.SelectedValue;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Masters/Packing/FrmOtherDocs.aspx");
        }
    }
    protected void btnPrintBankLetr_Click(object sender, EventArgs e)
    {
        try
        {
            string Qry = @"SELECT  Bank . BankName , Bank.Street,Bank.City,Bank.State,Bank.Country,BL.Invoice,BL.SpecificationList,BL.BillOfExch,BL.BLNo,BL.SBillNo,BL.FormSDF,BL.ExchangeCont,
                BL. SingleCountryDec,Invoice.InvoiceDate,Invoice.NoOfRolls,Invoice.BLNo,Invoice.DestinationAdd,Invoice.TInvoiceNo,Invoice.sbillno,TD.GoodsName,TD.CurrencyName,
                TD.Total,CI.CompanyName,BL.DocDate,Signatory.SignatoryName,TD.ExtraCharges FROM (BankLetter BL INNER JOIN (((Bank INNER JOIN VIEW_CompInfo CI ON Bank.BankId = CI.BankName ) 
                INNER JOIN INVOICE Invoice ON CI.CompanyId = Invoice.consignorId) LEFT OUTER JOIN  Signatory ON  CI.Sigantory = Signatory.SignatoryId ) 
                ON  BL.Packingid = Invoice.PackingId ) LEFT OUTER JOIN  Temp_Detail TD  ON  BL.Packingid = TD.PackingId where BL.PackingId=" + ViewState["InvoiceId"];
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["CommanFormula"] = "{bankLetter.PackingId}=" + ViewState["InvoiceId"] + " ";
                Session["ReportPath"] = "Reports/RptBankLetter.rpt";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
            }
            else
            {
                Msg = "No Records found!";
                MessageSave(Msg);
            }
        }
        catch (Exception ex)
        {
            Msg = "Error in viewing Bank Letter report!";
            MessageSave(Msg);
        }
    }
    protected void BtnPrintFBill_Click(object sender, EventArgs e)
    {
        string Qry = @"SELECT fr. DocumentType , fr. DraftsOri , fr. DraftsDup , fr. CustInVoiceOri , fr. CustInvoiceDup , fr. CunsInvoiceOri , 
                    fr. CunsInvoiceDup , fr. CertofOriginOri , fr. CertOfOriginDup , fr. WeightNoteOri , fr. WeightNoteDup , fr. InspOlleycertOri , fr. InspOlleyCertDup , 
            fr. BLOri , fr. BLDup , fr. CertAnalysisOri , fr. CertAnalysisDup , fr. PackListori , fr. PackListDup , fr. OtherDocumentOri , fr. OtherDocumentDup ,  
            Invoice . BLNo ,  Invoice . TInvoiceNo ,  Invoice . sbillno ,  Temp_Detail . DestinationAdd ,  Temp_Detail . NoOfRolls ,  Temp_Detail . CurrencyName , 
            Temp_Detail . Total ,  CompanyInfo . CompanyName ,  CompanyInfo . CompAddr1 ,  CompanyInfo . CompAddr2 ,  CompanyInfo . CompAddr3 ,  CompanyInfo . IECode ,  
            Bank . BankName ,  Bank . Street ,  Bank . City ,  Bank . State ,  Bank . Country ,  bank_1 . BankName ,  bank_1 . Street ,  bank_1 . City ,  bank_1 . State , 
            bank_1 . Country ,  Signatory . SignatoryName ,  Invoice . VesselName , fr. DocDate , fr. SCDOri , fr. SCDDup ,  Temp_Detail . GoodsName ,  Temp_Detail . ExtraCharges , 
            Invoice . Terms ,  DilveryTerm . TermName ,  Invoice . BlDt ,  Invoice . sbilldate ,  CustomerInfo . CustomerName ,  CustomerInfo . Address 
            FROM   ((((((( ForeignDocumentryBill  fr INNER JOIN  INVOICE   Invoice  ON fr. Packingid = Invoice . PackingId ) LEFT OUTER JOIN  Temp_Detail  
        Temp_Detail  ON  Invoice . PackingId = Temp_Detail . PackingId ) INNER JOIN  VIEW_CompInfo   CompanyInfo  ON  Invoice . consignorId = CompanyInfo . CompanyId ) 
        INNER JOIN  V_CustomerInfo   CustomerInfo  ON  Invoice . cosigneeId = CustomerInfo . CustomerId ) LEFT OUTER JOIN  Term   DilveryTerm  
        ON  Invoice . CreditId = DilveryTerm . TermId ) LEFT OUTER JOIN  V_Bank_1   bank_1  ON  CustomerInfo . CurrencyId = bank_1 . BankId ) INNER JOIN  Bank   Bank 
        ON  CompanyInfo . BankName = Bank . BankId ) LEFT OUTER JOIN  Signatory   Signatory  ON  CompanyInfo . Sigantory = Signatory . SignatoryId 
         where fr.PackingId=" + ViewState["InvoiceId"];
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["CommanFormula"] = "{ForeignDocumentryBill.PackingId}=" + ViewState["InvoiceId"] + " ";
            Session["ReportPath"] = "Reports/RptForeignDocumentryBill.rpt";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
        }
        else
        {
            Msg = "No Records found!";
            MessageSave(Msg);
        }
    }
    private void MessageSave(string msg)
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('");
        stb.Append(msg);
        stb.Append("');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
}