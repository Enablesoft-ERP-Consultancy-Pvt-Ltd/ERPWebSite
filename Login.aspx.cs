using System;
using ERP.Security;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using System.Text;
using System.IO.Compression;
using CrystalDecisions.CrystalReports;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string val = ConfigurationManager.AppSettings["xxpc36"];
        if (val == "7")
            logname.Text = "ORDER TRACKER";
        else
            logname.Text = "Export-Erp...";
        String s = Request.QueryString["Message"];
        lblErr.Text = s;
        txtUser.Focus();
        if (!IsPostBack)
        {
            try
            {
                string str = "select PWD_ENCRYPTED From Mastersetting Where PWD_ENCRYPTED=1";
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    str = "SELECT USERID,PASSWORD FROM NEWUSERDETAIL";
                    ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string update = "";
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string pwd = UtilityModule.Decrypt(ds.Tables[0].Rows[i]["Password"].ToString());
                            update = update + " Update Newuserdetail set Password='" + pwd + "' where userid=" + ds.Tables[0].Rows[i]["userid"].ToString() + "";
                        }
                        if (update != "")
                        {
                            update = update + "  Update MasterSetting set PWD_ENCRYPTED=0";
                            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, update);
                        }
                    }
                }
                //string str = "select PWD_ENCRYPTED From Mastersetting Where PWD_ENCRYPTDECRYPTFORM=1";
                //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    str = "SELECT USERID,PASSWORD FROM NEWUSERDETAIL";
                //    ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        string update = "";
                //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //        {
                //            string pwd = UtilityModule.Encrypt(ds.Tables[0].Rows[i]["Password"].ToString());
                //            update = update + " Update Newuserdetail set Password='" + pwd + "' where userid=" + ds.Tables[0].Rows[i]["userid"].ToString() + "";
                //        }
                //        if (update != "")
                //        {
                //            update = update + "  Update MasterSetting set PWD_ENCRYPTED=1";
                //            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, update);
                //        }
                //    }
                //}
                //else
                //{
                //    str = "select PWD_ENCRYPTED From Mastersetting Where PWD_ENCRYPTED=1";
                //    ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        str = "SELECT USERID,PASSWORD FROM NEWUSERDETAIL";
                //        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                //        if (ds.Tables[0].Rows.Count > 0)
                //        {
                //            string update = "";
                //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //            {
                //                string pwd = UtilityModule.Decrypt(ds.Tables[0].Rows[i]["Password"].ToString());
                //                update = update + " Update Newuserdetail set Password='" + pwd + "' where userid=" + ds.Tables[0].Rows[i]["userid"].ToString() + "";
                //            }
                //            if (update != "")
                //            {
                //                update = update + "  Update MasterSetting set PWD_ENCRYPTED=0";
                //                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, update);
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    protected void btnnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("NewUser.aspx");
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[12];
            _arrPara[0] = new SqlParameter("@USERNAME", SqlDbType.NVarChar, 50);
            _arrPara[1] = new SqlParameter("@PASSWORD", SqlDbType.NVarChar, 100);
            _arrPara[2] = new SqlParameter("@varuserid", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@varusername", SqlDbType.VarChar, 50);
            _arrPara[4] = new SqlParameter("@varDepartment", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@status", SqlDbType.Int);
            _arrPara[6] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _arrPara[7] = new SqlParameter("@CompanyName", SqlDbType.VarChar, 60);
            _arrPara[8] = new SqlParameter("@canedit", SqlDbType.TinyInt);
            _arrPara[9] = new SqlParameter("@usertype", SqlDbType.Int);
            _arrPara[10] = new SqlParameter("@CurrentWorkingCompanyID", SqlDbType.Int);
            _arrPara[11] = new SqlParameter("@LicenseKey", SqlDbType.VarChar, 100);

            _arrPara[0].Value = txtUser.Text.Trim();
            _arrPara[1].Value = txtPassword.Text;
            // _arrPara[1].Value = UtilityModule.Encrypt(txtPassword.Text);
            _arrPara[2].Direction = ParameterDirection.Output;
            _arrPara[3].Direction = ParameterDirection.Output;
            _arrPara[4].Direction = ParameterDirection.Output;
            _arrPara[5].Direction = ParameterDirection.Output;
            _arrPara[6].Value = ConfigurationManager.AppSettings["xxpc36"];
            _arrPara[7].Direction = ParameterDirection.Output;
            _arrPara[8].Direction = ParameterDirection.Output;
            _arrPara[9].Direction = ParameterDirection.Output;
            _arrPara[10].Direction = ParameterDirection.Output;
            _arrPara[11].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_LOGIN", _arrPara);

            if (Convert.ToInt32(_arrPara[5].Value) == 0)
            {
                lblErr.Text = "Login fail: Wrong UserName Or Password";
            }
            else if (Convert.ToInt32(_arrPara[5].Value) == 1)
            {
                //string lKey = _arrPara[11].Value.ToString();
                //if (!string.IsNullOrEmpty(lKey) && common.isLicenseValid(lKey, Convert.ToInt32(_arrPara[6].Value)))
                //{
                Session["varuserid"] = _arrPara[2].Value;
                //Session["varusername"] = _arrPara[3].Value;
                Session["varDepartment"] = _arrPara[4].Value;
                Session["varCompanyName"] = _arrPara[7].Value;
                string name = _arrPara[3].Value.ToString().ToLower();
                Session["varMasterCompanyIDForERP"] = ConfigurationManager.AppSettings["xxpc36"];
                Session["varusername"] = "Welcome  " + name[0].ToString().ToUpper() + name.Substring(1);
                Session["UserName"] = _arrPara[3].Value.ToString().ToUpper();
                Session["canedit"] = _arrPara[8].Value.ToString();
                Session["usertype"] = _arrPara[9].Value;
                Session["CurrentWorkingCompanyID"] = _arrPara[10].Value;

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
                    @"Select Varcompanyno,ProdCodeValidation,ProductionEditPwd,WithoutBOM,
                    DefaultCompId,IndentAsProduction,InvoiceReportType, RoundFtFlag,RoundMtrFlag,Stockapply,TagNoWise,carpetcompany,DyingProgramWithFullArea,
                    BOM_PurchaseincludeProcess,ReportWithpdf,withbuyercode,DyingAutoloss,JoborderNewModule,GatepassDeptwise,VarNewConsumptionWise,VarNewQualitySize,
                    varqctype,VarNewProcessConsumpRate,VarExtraPcsPercentage,VarExtraPcsProduction,VargetorderimagefromItemcode,DyeingIssueOthershade,CarpetPrefixauto,
                    MaintainProcessissueSeq,MaintainProcessReceiveSeq,NextProcessUserAuthentication,FinishingIssueSeqWise,FinishingNewModuleWise,PayrollDetail,ReportPwd,
                    AreaMtrRound,MasterBuyercodeSeqWise,DYEINGEDITPWD,LoomNoGenerated,CUSTOMERWISEPURCHASEWITHOUTBOM,katiwithbomcaltype,VarMfactor,RoundMtrFlag,DefaultSizeId,
                    DyeingprogramwithExportArea,MaintainStockSeries,KATIWITHEXPORTSIZE,Ft_To_Mtr_SizeRound,DefaultProductionunit,BinNowise,GENERATEINDENTTAGNOAUTOGENERATED,
                    STOCKNOPREFIX,PURCHASERECEIVEAUTOGENLOTNO,BAZARQAPEROSONNAME,TAGGINGWITHINTERNALPRODUCTION,AdvancePaymentExportExcelPwd,CHECKBINCONDITION,flagforsampleorder,
                    varLoomProductionRateUpdatePwd,GENERATEINDENTGODOWNID,MOTTELINGEDITPWD,CHANGESTOCKSTATUS_PWD,PAYMENTDEL_PWD,LOOMBAZARBYPERCENTAGE,DEMORDERWITHLOCALQDCS,
                    PURCHASEORDER_INDENTOTHERVENDOR,WEAVERORDERWITHOUTCUSTCODE,PRODUCTIONINTERNAL_EXTERNAL,GENERATESTOCKNOONTAGGING,YARNOPENINGISSUEEMPWISE,
                    GENRATEINDENTWITHOUTLOT_STOCK,INTERNALPRODUCTION_AREAWISE,MANYFOLIORAWISSUE_SINGLECHALAN,FINISHINGRATE_LOCATIONWISE,FINISHERJOBRATEFOR_OLDFORM,
                    VarGeneratePurchaseOrderChallanNoCompWise,VarAddFinisherJobRateByFtMtr,VarGetYarnOpeningRateFromMaster,ORDERNODROPDOWNWITHLOCALORDER,
                    MANYINDENTROWISSUE_SINGLECHALLAN,WEAVERPURCHASEORDER_FULLAREA,SHOWMATERIALISSUEDONFOLIO_BAZARFORM,VarIndentIssRecReportDataWithSample,
                    VarWeaverRawMaterialIssueToCompleteStatus,MANYFOLIORAWRECEIVE_SINGLECHALAN,VarDyeingProgramReportWithOrderLagat,SALCANCEL_PWD,
                    DEFINEPROCESSRATE_LOCATIONWISE,MANYCUSTOMERORDERISSUE_SINGLEPRODUCTIONORDERNO,MATERIALRECEIVEWITHLEGALVENDOR,PURCHASEORDERWITHLEGALVENDOR,
                    UpdatePurchaseOrderGSTType,ProductionOrderPcsWise,IndentRawIssueRateManual,GSTForIndentRawIssue,GSTForInvoiceFormNew,CompanyWiseChallanNoGenerated,
                    StopBackDateEntryOnProduction,StopBackDateEntryOnAllForms,SubmitDataNextDay, SAMPLECODEEDIT_DELETE_PWD, HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE, ORDER_STOCK_ASSIGN,
                    FinishingCalTypeNormalSqYardInchArea,OnlyPreviewButtonShowOnAllEditForm,IndentRawReturnSaveEditPassword,WeaverRawReceiveSaveEditPassword,FinishingOrderIssuePassword,
                    FinishingOrderReceivePassword, VarSubCompanyNo,VarConsumptionInMtr,VarProductionSizeItemWise,VarAreaFtRound,GetPurchaseRateFromMaster, MasterDate 
                    From Mastersetting(Nolock)");

                DateTime SystemDate = new DateTime();
                DateTime ExecDate = new DateTime();

                SystemDate = DateTime.Now.Date;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ExecDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["MasterDate"]);
                }

                if (Convert.ToInt32(ConfigurationManager.AppSettings["xxpc36"]) == 44)
                {
                    ExecDate = DateTime.Today.AddDays(-2);
                }
                if (SystemDate > ExecDate)
                {
                    Session["varuserid"] = null;
                    Session["varDepartment"] = null;
                    Session["varCompanyName"] = null;
                    Session["varMasterCompanyIDForERP"] = null;
                    Session["varusername"] = null;
                    Session["UserName"] = null;
                    Session["canedit"] = null;
                    Session["usertype"] = null;
                    Session["CurrentWorkingCompanyID"] = null;

                    UtilityModule.DeleteExeFolder();

                    Response.Redirect("Login.aspx");
                    //Response.Redirect("main.aspx");
                    return;
                }

                Session["VarcompanyNo"] = ds.Tables[0].Rows[0]["Varcompanyno"];
                Session["ProdCodeValidation"] = ds.Tables[0].Rows[0]["ProdCodeValidation"];
                MySession.ProductionEditPwd = ds.Tables[0].Rows[0]["ProductionEditPwd"].ToString();
                Session["WithoutBOM"] = ds.Tables[0].Rows[0]["WithoutBOM"];
                Session["DCompanyId"] = ds.Tables[0].Rows[0]["DefaultCompId"];
                Session["varSubCompanyId"] = ds.Tables[0].Rows[0]["VarSubCompanyNo"];

                MySession.IndentAsProduction = ds.Tables[0].Rows[0]["IndentAsProduction"].ToString();
                MySession.InvoiceReportType = ds.Tables[0].Rows[0]["InvoiceReportType"].ToString();
                MySession.RoundMtrFlag = ds.Tables[0].Rows[0]["RoundMtrFlag"].ToString();
                MySession.RoundFtFlag = ds.Tables[0].Rows[0]["RoundFtFlag"].ToString();
                MySession.Stockapply = ds.Tables[0].Rows[0]["Stockapply"].ToString();
                MySession.TagNowise = ds.Tables[0].Rows[0]["TagNowise"].ToString();

                variable.Carpetcompany = ds.Tables[0].Rows[0]["carpetcompany"].ToString();
                variable.DyingProgramWithFullArea = ds.Tables[0].Rows[0]["DyingProgramWithFullArea"].ToString();//=1
                variable.BOM_PurchaseIncludeProcess = ds.Tables[0].Rows[0]["BOM_PurchaseincludeProcess"].ToString();//=1
                variable.ReportWithpdf = ds.Tables[0].Rows[0]["ReportWithpdf"].ToString();//=1
                variable.Withbuyercode = ds.Tables[0].Rows[0]["withbuyercode"].ToString();
                variable.Dyingautoloss = ds.Tables[0].Rows[0]["DyingAutoloss"].ToString();
                variable.JoborderNewModule = ds.Tables[0].Rows[0]["JoborderNewModule"].ToString();
                variable.Gatepassdeptwise = ds.Tables[0].Rows[0]["gatepassdeptwise"].ToString();
                variable.VarNewConsumptionWise = ds.Tables[0].Rows[0]["VarNewConsumptionWise"].ToString();
                variable.VarNewQualitySize = ds.Tables[0].Rows[0]["VarNewQualitySize"].ToString();
                variable.VarQctype = ds.Tables[0].Rows[0]["varqctype"].ToString();
                variable.VarNewProcessConsumpRate = ds.Tables[0].Rows[0]["VarNewProcessConsumpRate"].ToString();
                variable.VarExtraPcsPercentage = ds.Tables[0].Rows[0]["VarExtraPcsPercentage"].ToString();
                variable.VarExtraPcsProduction = ds.Tables[0].Rows[0]["VarExtraPcsProduction"].ToString();
                variable.VargetorderimagefromItemcode = ds.Tables[0].Rows[0]["VargetorderimagefromItemcode"].ToString();
                variable.VarDyeingIssueOthershade = ds.Tables[0].Rows[0]["DyeingIssueOthershade"].ToString();
                variable.VarCarpetPrefixauto = ds.Tables[0].Rows[0]["CarpetPrefixauto"].ToString();
                variable.VarMaintainProcessissueSeq = ds.Tables[0].Rows[0]["MaintainProcessissueSeq"].ToString();
                variable.VarMaintainProcessReceiveSeq = ds.Tables[0].Rows[0]["MaintainProcessReceiveSeq"].ToString();
                variable.VarNextProcessUserAuthentication = ds.Tables[0].Rows[0]["NextProcessUserAuthentication"].ToString();
                variable.VarFinishingIssueSeqWise = ds.Tables[0].Rows[0]["FinishingIssueSeqWise"].ToString();
                variable.VarFinishingNewModuleWise = ds.Tables[0].Rows[0]["FinishingNewModuleWise"].ToString();
                variable.VarPayrollDetail = ds.Tables[0].Rows[0]["PayrollDetail"].ToString();
                variable.VarReportPwd = ds.Tables[0].Rows[0]["ReportPwd"].ToString();
                variable.VarAreaMtrRound = ds.Tables[0].Rows[0]["AreaMtrRound"].ToString();
                variable.VarMasterBuyercodeSeqWise = ds.Tables[0].Rows[0]["MasterBuyercodeSeqWise"].ToString();
                variable.VarDyeingeditpwd = ds.Tables[0].Rows[0]["DYEINGEDITPWD"].ToString();
                variable.VarLoomNoGenerated = ds.Tables[0].Rows[0]["LoomNoGenerated"].ToString();
                variable.VarCUSTOMERWISEPURCHASEWITHOUTBOM = ds.Tables[0].Rows[0]["CUSTOMERWISEPURCHASEWITHOUTBOM"].ToString();
                variable.Varkatiwithbomcaltype = ds.Tables[0].Rows[0]["katiwithbomcaltype"].ToString();
                variable.VarMfactor = ds.Tables[0].Rows[0]["VarMfactor"].ToString();
                variable.VarRoundMtrFlag = Convert.ToInt16(ds.Tables[0].Rows[0]["RoundMtrFlag"]);
                variable.VarDefaultSizeId = ds.Tables[0].Rows[0]["DefaultSizeId"].ToString();
                variable.VarDyeingprogramwithExportArea = ds.Tables[0].Rows[0]["DyeingprogramwithExportArea"].ToString();
                variable.VarMaintainStockSeries = ds.Tables[0].Rows[0]["MaintainStockSeries"].ToString();
                variable.VarKATIWITHEXPORTSIZE = ds.Tables[0].Rows[0]["KATIWITHEXPORTSIZE"].ToString();
                variable.VarFt_To_Mtr_SizeRound = ds.Tables[0].Rows[0]["Ft_To_Mtr_SizeRound"].ToString();
                variable.VarDefaultProductionunit = ds.Tables[0].Rows[0]["DefaultProductionunit"].ToString();
                variable.VarBINNOWISE = ds.Tables[0].Rows[0]["BinNowise"].ToString();
                variable.VarGENERATEINDENTTAGNOAUTOGENERATED = ds.Tables[0].Rows[0]["GENERATEINDENTTAGNOAUTOGENERATED"].ToString();
                variable.VarSTOCKNOPREFIX = ds.Tables[0].Rows[0]["STOCKNOPREFIX"].ToString();
                variable.VarPURCHASERECEIVEAUTOGENLOTNO = ds.Tables[0].Rows[0]["PURCHASERECEIVEAUTOGENLOTNO"].ToString();
                variable.VarBAZARQAPEROSONNAME = ds.Tables[0].Rows[0]["BAZARQAPEROSONNAME"].ToString();
                variable.VarTAGGINGWITHINTERNALPRODUCTION = ds.Tables[0].Rows[0]["TAGGINGWITHINTERNALPRODUCTION"].ToString();
                variable.VarAdvancePaymentExportExcelpwd = ds.Tables[0].Rows[0]["AdvancePaymentExportExcelPwd"].ToString();
                variable.VarCHECKBINCONDITION = ds.Tables[0].Rows[0]["CHECKBINCONDITION"].ToString();
                variable.Varflagforsampleorder = Convert.ToInt32(ds.Tables[0].Rows[0]["flagforsampleorder"]);
                variable.VarLoomProductionRateUpdatePwd = ds.Tables[0].Rows[0]["VarLoomProductionRateUpdatePwd"].ToString();
                variable.VarGENERATEINDENTGODOWNID = ds.Tables[0].Rows[0]["GENERATEINDENTGODOWNID"].ToString();
                variable.VarMOTTELINGEDITPWD = ds.Tables[0].Rows[0]["MOTTELINGEDITPWD"].ToString();
                variable.VarCHANGESTOCKSTATUS_PWD = ds.Tables[0].Rows[0]["CHANGESTOCKSTATUS_PWD"].ToString();
                variable.VarPAYMENTDEL_PWD = ds.Tables[0].Rows[0]["PAYMENTDEL_PWD"].ToString();
                variable.VarLOOMBAZARBYPERCENTAGE = ds.Tables[0].Rows[0]["LOOMBAZARBYPERCENTAGE"].ToString();
                variable.VarDEMORDERWITHLOCALQDCS = ds.Tables[0].Rows[0]["DEMORDERWITHLOCALQDCS"].ToString();
                variable.VarPURCHASEORDER_INDENTOTHERVENDOR = ds.Tables[0].Rows[0]["PURCHASEORDER_INDENTOTHERVENDOR"].ToString();
                variable.VarWEAVERORDERWITHOUTCUSTCODE = ds.Tables[0].Rows[0]["WEAVERORDERWITHOUTCUSTCODE"].ToString();
                variable.VarPRODUCTIONINTERNAL_EXTERNAL = ds.Tables[0].Rows[0]["PRODUCTIONINTERNAL_EXTERNAL"].ToString();
                variable.VarGENERATESTOCKNOONTAGGING = ds.Tables[0].Rows[0]["GENERATESTOCKNOONTAGGING"].ToString();
                variable.VarYARNOPENINGISSUEEMPWISE = ds.Tables[0].Rows[0]["YARNOPENINGISSUEEMPWISE"].ToString();
                variable.VarGENRATEINDENTWITHOUTLOT_STOCK = ds.Tables[0].Rows[0]["GENRATEINDENTWITHOUTLOT_STOCK"].ToString();
                variable.VarINTERNALPRODUCTION_AREAWISE = ds.Tables[0].Rows[0]["INTERNALPRODUCTION_AREAWISE"].ToString();
                variable.VarMANYFOLIORAWISSUE_SINGLECHALAN = ds.Tables[0].Rows[0]["MANYFOLIORAWISSUE_SINGLECHALAN"].ToString();
                variable.VarFINISHINGRATE_LOCATIONWISE = ds.Tables[0].Rows[0]["FINISHINGRATE_LOCATIONWISE"].ToString();
                variable.VarFINISHERJOBRATEFOR_OLDFORM = ds.Tables[0].Rows[0]["FINISHERJOBRATEFOR_OLDFORM"].ToString();
                variable.VarGeneratePurchaseOrderChallanNoCompWise = ds.Tables[0].Rows[0]["VarGeneratePurchaseOrderChallanNoCompWise"].ToString();
                variable.VarAddFinisherJobRateByFtMtr = ds.Tables[0].Rows[0]["VarAddFinisherJobRateByFtMtr"].ToString();
                variable.VarGetYarnOpeningRateFromMaster = ds.Tables[0].Rows[0]["VarGetYarnOpeningRateFromMaster"].ToString();
                variable.VarORDERNODROPDOWNWITHLOCALORDER = ds.Tables[0].Rows[0]["ORDERNODROPDOWNWITHLOCALORDER"].ToString();
                variable.VarMANYINDENTROWISSUE_SINGLECHALLAN = ds.Tables[0].Rows[0]["MANYINDENTROWISSUE_SINGLECHALLAN"].ToString();
                variable.VarWEAVERPURCHASEORDER_FULLAREA = ds.Tables[0].Rows[0]["WEAVERPURCHASEORDER_FULLAREA"].ToString();
                variable.VarSHOWMATERIALISSUEDONFOLIO_BAZARFORM = ds.Tables[0].Rows[0]["SHOWMATERIALISSUEDONFOLIO_BAZARFORM"].ToString();
                variable.VarIndentIssRecReportDataWithSample = ds.Tables[0].Rows[0]["VarIndentIssRecReportDataWithSample"].ToString();
                variable.VarWeaverRawMaterialIssueToCompleteStatus = ds.Tables[0].Rows[0]["VarWeaverRawMaterialIssueToCompleteStatus"].ToString();
                variable.VarMANYFOLIORAWRECEIVE_SINGLECHALAN = ds.Tables[0].Rows[0]["MANYFOLIORAWRECEIVE_SINGLECHALAN"].ToString();
                variable.VarDyeingProgramReportWithOrderLagat = ds.Tables[0].Rows[0]["VarDyeingProgramReportWithOrderLagat"].ToString();
                variable.VarSALCANCEL_PWD = ds.Tables[0].Rows[0]["SALCANCEL_PWD"].ToString();
                variable.VarDEFINEPROCESSRATE_LOCATIONWISE = ds.Tables[0].Rows[0]["DEFINEPROCESSRATE_LOCATIONWISE"].ToString();
                variable.VarMANYCUSTOMERORDERISSUE_SINGLEPRODUCTIONORDERNO = ds.Tables[0].Rows[0]["MANYCUSTOMERORDERISSUE_SINGLEPRODUCTIONORDERNO"].ToString();
                variable.VarMATERIALRECEIVEWITHLEGALVENDOR = ds.Tables[0].Rows[0]["MATERIALRECEIVEWITHLEGALVENDOR"].ToString();
                variable.VarPURCHASEORDERWITHLEGALVENDOR = ds.Tables[0].Rows[0]["PURCHASEORDERWITHLEGALVENDOR"].ToString();
                variable.VarUpdatePurchaseOrderGSTType = ds.Tables[0].Rows[0]["UpdatePurchaseOrderGSTType"].ToString();
                variable.VarProductionOrderPcsWise = ds.Tables[0].Rows[0]["ProductionOrderPcsWise"].ToString();
                variable.VarIndentRawIssueRateManual = ds.Tables[0].Rows[0]["IndentRawIssueRateManual"].ToString();
                variable.VarGSTForIndentRawIssue = ds.Tables[0].Rows[0]["GSTForIndentRawIssue"].ToString();
                variable.VarGSTForInvoiceFormNew = ds.Tables[0].Rows[0]["GSTForInvoiceFormNew"].ToString();
                variable.VarCompanyWiseChallanNoGenerated = ds.Tables[0].Rows[0]["CompanyWiseChallanNoGenerated"].ToString();
                variable.VarStopBackDateEntryOnProduction = ds.Tables[0].Rows[0]["StopBackDateEntryOnProduction"].ToString();
                variable.VarStopBackDateEntryOnAllForms = ds.Tables[0].Rows[0]["StopBackDateEntryOnAllForms"].ToString();
                variable.VarSubmitDataNextDay = ds.Tables[0].Rows[0]["SubmitDataNextDay"].ToString();
                variable.VarSAMPLECODEEDIT_DELETE_PWD = ds.Tables[0].Rows[0]["SAMPLECODEEDIT_DELETE_PWD"].ToString();
                variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE = ds.Tables[0].Rows[0]["HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE"].ToString();
                variable.ORDER_STOCK_ASSIGN = ds.Tables[0].Rows[0]["ORDER_STOCK_ASSIGN"].ToString();
                variable.VarFinishingCalTypeNormalSqYardInchArea = ds.Tables[0].Rows[0]["FinishingCalTypeNormalSqYardInchArea"].ToString();
                variable.VarOnlyPreviewButtonShowOnAllEditForm = ds.Tables[0].Rows[0]["OnlyPreviewButtonShowOnAllEditForm"].ToString();
                variable.VarIndentRawReturnSaveEditPassword = ds.Tables[0].Rows[0]["IndentRawReturnSaveEditPassword"].ToString();
                variable.VarWeaverRawReceiveSaveEditPassword = ds.Tables[0].Rows[0]["WeaverRawReceiveSaveEditPassword"].ToString();
                variable.VarFinishingOrderIssuePassword = ds.Tables[0].Rows[0]["FinishingOrderIssuePassword"].ToString();
                variable.VarFinishingOrderReceivePassword = ds.Tables[0].Rows[0]["FinishingOrderReceivePassword"].ToString();
                variable.VarConsumptionInMtr = ds.Tables[0].Rows[0]["VarConsumptionInMtr"].ToString();
                variable.VarProductionSizeItemWise = ds.Tables[0].Rows[0]["VarProductionSizeItemWise"].ToString();
                variable.VarAreaFtRound = ds.Tables[0].Rows[0]["VarAreaFtRound"].ToString();
                variable.VarRoundFtFlag = Convert.ToInt32(ds.Tables[0].Rows[0]["RoundFtFlag"].ToString());
                variable.VarGetPurchaseRateFromMaster = ds.Tables[0].Rows[0]["GetPurchaseRateFromMaster"].ToString();
                
                //PageLoad();
                UtilityModule.DeleteExeFolder();

                Response.Redirect("main.aspx");
            }
        }
        catch (Exception ex)
        {
            bool result;
            result = ex.Message.Contains("Access");
            if (result == false)
            {
                lblErr.Text = ex.Message;
            }
            // UtilityModule.MessageAlert(ex.Message, "/Login.aspx");
            Logs.WriteErrorLog("Login|cmdLogin_Click|" + ex.Message);
        }
    }
    private void PageLoad()
    {
        string Path = Server.MapPath("~\\App_Code");

        if (Directory.Exists(Path))
        {
            DeleteDirectory(Path);
        }
        Path = Server.MapPath("~\\Reports");

        if (Directory.Exists(Path))
        {
            DeleteDirectory(Path);
        }
        Path = Server.MapPath("~\\UserControls");

        if (Directory.Exists(Path))
        {
            DeleteDirectory(Path);
        }
        Path = Server.MapPath("~\\HRUserControls");

        if (Directory.Exists(Path))
        {
            DeleteDirectory(Path);
        }
        Path = Server.MapPath("~\\Masters");

        if (Directory.Exists(Path))
        {
            DeleteDirectory(Path);
        }
    }
    private void DeleteDirectory(string path)
    {
        foreach (string filename in Directory.GetFiles(path))
        {
            File.Delete(filename);
        }
        foreach (string subfolders in Directory.GetDirectories(path))
        {
            Directory.Delete(subfolders, true);
        }
    }

	

}