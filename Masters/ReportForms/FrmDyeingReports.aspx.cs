using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;
using System.Text;
public partial class Masters_ReportForms_FrmDyeingReports : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by Companyname 
            select E.Empid,EmpName+'/'+Address From Empinfo E,EmpProcess EP where E.Empid=EP.Empid And ProcessId=5 And E.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
            select Category_Id,Category_Name From Item_Category_Master CM,CategorySeparate CS where CM.Category_Id=CS.Categoryid And Id=1 And CM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            DataSet ds = SqlHelper.ExecuteDataset(str);
            CommanFunction.FillComboWithDS(DDCompany, ds, 0);
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                UtilityModule.ConditionalComboFill(ref DDCustCode, "Select CustomerId,CustomerCode From CustomerInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By CustomerCode", true, "--Select--");
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDDyerName, ds, 1, true, "All");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, "--select--");
            imgLogo.ImageUrl.DefaultIfEmpty();
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varMasterCompanyIDForERP"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
            LblCompanyName.Text = Session["varCompanyName"].ToString();
            LblUserName.Text = Session["varusername"].ToString();
            RDDyingProgramDelStatus.Checked = true;
            TxtFdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDCustCode, "Select CustomerId,CustomerCode From CustomerInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By CustomerCode", true, "--Select--");
    }
    protected void DDCustCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str =@"select orderId,CustomerOrderNo From OrderMaster where CustomerId=" + DDCustCode.SelectedValue + " And CompanyId=" + DDCompany.SelectedValue;

        if (Session["varMasterCompanyIDForERP"].ToString() == "16" || Session["varMasterCompanyIDForERP"].ToString() == "28")
        {
            Str = @"select orderId,CustomerOrderNo From OrderMaster where Status = 0 And CustomerId=" + DDCustCode.SelectedValue + " And CompanyId=" + DDCompany.SelectedValue;
        }
        UtilityModule.ConditionalComboFill(ref DDOrderNo, Str, true, "--Select--");
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");

    }
    protected void DDDyerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDDyerName.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref DDIndentno, "select IndentId,IndentNo From IndentMaster where PartyId=" + DDDyerName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by IndentNo", true, "--Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDIndentno, "select IndentId,IndentNo From IndentMaster Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by IndentNo", true, "--Select--");
        }

    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDItem, "select Item_Id,Item_Name From item_Master where Category_Id=" + DDCategory.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Item_Name", true, "All");

    }
    protected void DDItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDItem.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref DDSubItem, "select QualityId,QualityName From Quality Where Item_Id=" + DDItem.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by QualityName", true, "All");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDSubItem, "select QualityId,QualityName From Quality Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  order by QualityName", true, "All");
        }
    }
    protected void DDSubItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select ShadeColorId,ShadeColorName From Item_Parameter_Master IM,shadeColor S where IM.ShadeColor_Id=S.ShadeColorId And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        if (DDSubItem.SelectedIndex > 0)
        {
            str = str + " And Quality_Id=" + DDSubItem.SelectedValue;
        }
        str = str + " order by ShadeColorName";
        UtilityModule.ConditionalComboFill(ref DDColor, str, true, "All");
    }
    protected void DyerWiseDyingReport()
    {
        lblMessage.Text = "";
        if (lblMessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "Truncate Table TempOrderWiseDyeingBal");

                string str = @"select E.EmpName,IndentNo,Item_Name,QualityName,ShadeColorName,
                        IsNull(Sum(IssueQuantity),0) As IssQty,0 As RecQty,IM.ReqDate,'' As LatebyDays,IM.IndentId,0,0
                       From IndentMaster IM,PP_ProcessRawMaster PM,PP_ProcessRawTran PT,V_FinishedItemDetail V,EmpInfo E
                       where IM.IndentId=PT.IndentId And PM.PRMID=PT.PRMID And V.Item_Finished_id=PT.Finishedid And PM.EmpId=E.EmpId  And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

                if (DDDyerName.SelectedIndex > 0)
                {
                    str = str + " And E.EmpId=" + DDDyerName.SelectedValue;
                }
                if (DDIndentno.SelectedIndex > 0)
                {
                    str = str + " And IM.IndentId=" + DDIndentno.SelectedValue;
                }
                if (DDItem.SelectedIndex > 0)
                {
                    str = str + " And V.Item_Id=" + DDItem.SelectedValue;
                }
                if (DDSubItem.SelectedIndex > 0)
                {
                    str = str + " And V.QualityId=" + DDSubItem.SelectedValue;
                }
                if (DDColor.SelectedIndex > 0)
                {
                    str = str + " And V.ShadeColorId=" + DDColor.SelectedValue;
                }
                str = str + "  group by E.EmpName,IndentNo,Item_Name,QualityName,ShadeColorName,IM.ReqDate,IM.IndentId";
                SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "Insert into TempOrderWiseDyeingBal " + str + "");
                DataSet Ds = SqlHelper.ExecuteDataset(tran, CommandType.Text, "select Distinct IndentId,DueDate From TempOrderWiseDyeingBal ");
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                    {
                        str = @"select E.EmpName,IndentNo,Item_Name,QualityName,ShadeColorName,
                            0 As IssQty,IsNull(Sum(REcQuantity),0)  As RecQty,'" + Ds.Tables[0].Rows[i]["DueDate"] + @"','' As LatebyDays,IM.IndentId,0,0  From
                            PP_ProcessRecMaster PM,PP_ProcessRecTran PT,V_FinishedItemDetail V,IndentMaster IM,EmpInfo E
                            where PM.PRMID=PT.PRMID And E.EmpID=PM.EMpID And V.Item_Finished_ID=PT.Finishedid And IM.IndentId=PT.Indentid
                            And PT.IndentId=" + Ds.Tables[0].Rows[i]["IndentId"] + " And V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                        if (DDItem.SelectedIndex > 0)
                        {
                            str = str + " And V.Item_Id=" + DDItem.SelectedValue;
                        }
                        if (DDSubItem.SelectedIndex > 0)
                        {
                            str = str + " And V.QualityId=" + DDSubItem.SelectedValue;
                        }
                        if (DDColor.SelectedIndex > 0)
                        {
                            str = str + " And V.ShadeColorId=" + DDColor.SelectedValue;
                        }
                        str = str + " group by E.EmpName,IndentNo,Item_Name,QualityName,ShadeColorName,IM.IndentId";
                        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "Insert into TempOrderWiseDyeingBal " + str + "");
                    }

                }
                tran.Commit();
                Session["ReportPath"] = "Reports/DyerWiseDyingReportForPoshNEW.rpt";
                Session["CommanFormula"] = "";
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "priview1();", true);
                Report1();

            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmDyeingReports.aspx.aspx");
                tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCustCode) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDOrderNo) == false)
        {
            goto a;
        }

        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
    }
    private static int GetDelayDays(DateTime ReqDate, DateTime RecDate)
    {
        // DelayDays = DateDiff(DateInterval.Day, ReqDate, RecDate);
        TimeSpan ts = RecDate.Subtract(ReqDate);
        int DelayDays = ts.Days;
        if (DelayDays < 0)
        {
            DelayDays = 0;
        }
        return DelayDays;
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        if (RDDyingProgramDelStatus.Checked == true)
        {
            DYINGPROGRAMDELIVERYSTATUSREPORT();
        }
        if (RDDyingProgAgainstDyer.Checked == true)
        {
            DyerWiseDyingDetail();
        }

        if (RDdyingLedger.Checked == true)
        {
            DyeingLedger();
        }
        if (RDDHDyerWise.Checked == true)
        {
            DyeingHissabDyerWise();
        }
        if (RDDyeingHissab1.Checked == true)
        {
            CHECKVALIDCONTROL();
            if (lblMessage.Text == "")
            {
                SqlParameter[] _arrpara = new SqlParameter[2];
                _arrpara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@EmpId ", SqlDbType.Int);
                _arrpara[0].Value = DDOrderNo.SelectedValue;
                if (DDDyerName.SelectedIndex > 0)
                {
                    _arrpara[1].Value = DDDyerName.SelectedValue;
                }
                else
                {
                    _arrpara[1].Value = 0;
                }
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_OrderDetailForDyeing", _arrpara);
                Session["ReportPath"] = "Reports/RptOrderWiseDyingHissab.rpt";
                Session["dsFileName"] = "~\\ReportSchema\\RptOrderWiseDyingHissab.xsd";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["rptFileName"] = Session["ReportPath"];
                    Session["GetDataset"] = ds;
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                }

            }
        }
    }
    protected void DYINGPROGRAMDELIVERYSTATUSREPORT()
    {
        CHECKVALIDCONTROL();
        if (lblMessage.Text == "")
        {
            Session["ReportPath"] = "Reports/RptDyeingAgainst_BuyerCode_OrderNoNEW.rpt";
            Session["CommanFormula"] = "{OrderMaster.CustomerId}=" + DDCustCode.SelectedValue + " And {OrderMaster.OrderId}=" + DDOrderNo.SelectedValue + "";
            Report1();
        }
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/main.aspx");
    }
    protected void OrderWiseDyeingHissab()
    {
        CHECKVALIDCONTROL();
        if (lblMessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                SqlHelper.ExecuteNonQuery(tran, CommandType.Text, " Truncate Table Temp_OrderWiseDyeingHissab");
                //SqlHelper.ExecuteNonQuery(tran, CommandType.Text, " Truncate Table Temp_OrderWiseDyeingHissabForRec");
                string str = @" select OFinishedid,C.CompanyName,CompAddr1+','+CompAddr2+','+CompAddr3 As CompAdd,V.Item_NAme,QualityName,ShadeColorName,EmpName,Rate,IM.IndentId,Isnull([dbo].[F_QtytoDye](Id.OFinishedid,OrderId,PPNo),0) As DyeQty,
                              0 As IssQty,0 As RecQty,0 As Loss,0 As SessionId,OrderId,'Issue',0,0
                              From IndentMaster IM,IndentDetail Id,V_FinishedItemDetail V,CompanyInfo C,EmpInfo E where IM.IndentId=ID.IndentId And V.Item_Finished_ID=ID.OFinishedID And IM.PartyId=E.EmpId And IM.CompanyId=C.CompanyId And IM.ProcessId=5
                              And  ID.OrderId=" + DDOrderNo.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " group by  Id.OFinishedid,V.Item_Name,QualityName,ShadeColorName,IM.IndentId,EmpName,Rate,OrderId,PPNo,C.CompanyName,CompAddr1,CompAddr2,CompAddr3 ";
                SqlHelper.ExecuteNonQuery(tran, CommandType.Text, " Insert into Temp_OrderWiseDyeingHissab " + str + "");
                DataSet ds = SqlHelper.ExecuteDataset(tran, CommandType.Text, "Select IndentId,OrderId From Temp_OrderWiseDyeingHissab");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        str = @"select IFinishedid,C.CompanyName,CompAddr1+','+CompAddr2+','+CompAddr3 As CompAdd,V.Item_NAme,QualityName,ShadeColorName,EmpName,Rate,IM.IndentId,0 As DyeQty,
                           Isnull([dbo].[F_IssQtytoDyer](Id.IFinishedid,IM.IndentId),0) As IssQty,0 As RecQty,0 As Loss,0 As SessionId,ID.OrderId,'Issue',99999,0
                           From IndentMaster IM,IndentDetail Id,V_FinishedItemDetail V,EmpInfo E,CompanyInfo C where IM.IndentId=ID.IndentId And V.Item_Finished_ID=ID.IFinishedID And IM.PartyId=E.EmpId And C.CompanyId=IM.CompanyId And IM.ProcessId=5
                           And IM.IndentId=" + ds.Tables[0].Rows[i]["IndentId"] + " And ID.OrderId=" + ds.Tables[0].Rows[i]["OrderId"] + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  group by  Id.IFinishedid,V.Item_Name,QualityName,ShadeColorName,IM.IndentId,EmpName,Rate,ID.OrderId,C.CompanyName,CompAddr1,CompAddr2,CompAddr3";

                        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, " Insert into Temp_OrderWiseDyeingHissab " + str + "");
                        str = @"select OFinishedid,C.CompanyName,CompAddr1+','+CompAddr2+','+CompAddr3 As CompAdd,V.Item_NAme,QualityName,ShadeColorName,EmpName,Rate,IM.IndentId,0 As DyeQty,
                              0 As IssQty,Isnull([dbo].[F_RecQtytoDyer](Id.OFinishedid,IM.IndentId),0)  As RecQty,0 As Loss,0 As SessionId,ID.OrderId,'Receive',LossPercent,Isnull([dbo].[F_LossQtytoDyer](Id.OFinishedid,IM.IndentId),0)  As LossQty
                              From IndentMaster IM,IndentDetail Id,V_FinishedItemDetail V,EmpInfo E,CompanyInfo C where IM.IndentId=ID.IndentId And V.Item_Finished_ID=ID.OFinishedID And IM.PartyId=E.EmpId And C.CompanyId=IM.CompanyId And IM.ProcessId=5
                              And IM.IndentId=" + ds.Tables[0].Rows[i]["IndentId"] + "  And ID.OrderId=" + ds.Tables[0].Rows[i]["OrderId"] + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " group by  Id.OFinishedid,V.Item_Name,QualityName,ShadeColorName,IM.IndentId,EmpName,Rate,ID.OrderId,C.CompanyName,CompAddr1,CompAddr2,CompAddr3,LossPercent";
                        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, " Insert into Temp_OrderWiseDyeingHissab " + str + "");
                    }
                }
                str = "select IsNull(Round(Sum(case When Type='Issue' Then IssQty Else 0 End)-Sum(Case When type='Receive' then RecQty+LossQty Else 0 End),3),0) As ExcessMaterial From Temp_OrderWiseDyeingHissab where OrderId=" + DDOrderNo.SelectedValue;
                DataSet ds1 = SqlHelper.ExecuteDataset(tran, CommandType.Text, str);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    TxtExcessMatBal.Text = Convert.ToString(ds1.Tables[0].Rows[0][0]);
                    ViewState["TxtExcessMatBal"] = ds1.Tables[0].Rows[0][0];
                    str = @"select IsNull(Max(price),0) As Price From Stock where Item_Finished_Id in(select T.FinishedId From PP_ProcessRawMaster PM,PP_ProcessRawTran PT,Temp_OrderWiseDyeingHissab T where PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" And PM.PRMID=PT.PRMID
                       And PT.IndentId=T.IndentId And PT.Finishedid=T.Finishedid And T.LossPercent=99999) And LotNo in(
                       select LotNo From PP_ProcessRawMaster PM,PP_ProcessRawTran PT,Temp_OrderWiseDyeingHissab T where PM.PRMID=PT.PRMID
                       And PT.IndentId=T.IndentId And PT.Finishedid=T.Finishedid And T.LossPercent=99999)";
                    double Price = Convert.ToDouble(SqlHelper.ExecuteScalar(tran, CommandType.Text, str));
                    Txtprice.Text = Convert.ToString(Price);
                    ViewState["Txtprice"] = Price;
                    TxtexcessMatBalAmt.Text = Convert.ToString(Convert.ToDouble(TxtExcessMatBal.Text) * Price);
                    ViewState["TxtexcessMatBalAmt"] = Convert.ToDouble(TxtExcessMatBal.Text) * Price;
                }
                double Amount = Convert.ToDouble(SqlHelper.ExecuteScalar(tran, CommandType.Text, "Select IsNull(Sum((RecQty+LossQty)*Rate),0) From Temp_OrderWiseDyeingHissab where OrderId=" + DDOrderNo.SelectedValue + ""));
                TxtFinalHissab.Text = Convert.ToString(Amount - Convert.ToDouble(TxtexcessMatBalAmt.Text));
                ViewState["TxtFinalHissab"] = Amount - Convert.ToDouble(TxtexcessMatBalAmt.Text);
                tran.Commit();
                Session["ReportPath"] = "Reports/RptOrderWiseDyeingHissabNEW.rpt";
                Session["CommanFormula"] = "{OrderMaster.CustomerId}=" + DDCustCode.SelectedValue + " And {OrderMaster.OrderId}=" + DDOrderNo.SelectedValue + "";
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "priview();", true);
                Report1();
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmDyeingReports.aspx.aspx");
                tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }
    }
    private void Report1()
    {
        DataSet ds = new DataSet();
        if (Convert.ToString(Session["ReportPath"]) == "Reports/DyerWiseDyingReportForPoshNEW.rpt")
        {

            string qry = @" SELECT ItemName,ColorName,DyerName,IndentNo,Subitem,IssQty,RecQty,LatebyDays,DueDate
 FROM   TempOrderWiseDyeingBal  ORDER BY DyerName,IndentNo";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            Session["dsFileName"] = "~\\ReportSchema\\DyerWiseDyingReportForPoshNEW.xsd";
        }
        else if (Convert.ToString(Session["ReportPath"]) == "Reports/RptDyeingAgainst_BuyerCode_OrderNoNEW.rpt")
        {
            SqlParameter[] _array = new SqlParameter[4];
            _array[0] = new SqlParameter("@OrderId", DDOrderNo.SelectedValue);
            _array[1] = new SqlParameter("@CustomerId", DDCustCode.SelectedValue);
            _array[2] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            _array[3] = new SqlParameter("@MasterCompanyId", Session["varcompanyNo"]);
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_RptDyeingAgainst_BuyerCode_OrderNoNEW", _array);
            Session["dsFileName"] = "~\\ReportSchema\\RptDyeingAgainst_BuyerCode_OrderNoNEW.xsd";
        }
        if (Convert.ToString(Session["ReportPath"]) == "Reports/RptOrderWiseDyeingHissabNEW.rpt")
        {
            string qry = @" SELECT Temp_OrderWiseDyeingHissab.CompanyName,Temp_OrderWiseDyeingHissab.CompAdd,Temp_OrderWiseDyeingHissab.ItemName,Temp_OrderWiseDyeingHissab.QualityName,
Temp_OrderWiseDyeingHissab.ColorName,Temp_OrderWiseDyeingHissab.EmpName,Temp_OrderWiseDyeingHissab.DyeQty,Temp_OrderWiseDyeingHissab.IssQty,customerinfo.CustomerCode,
OrderMaster.CustomerOrderNo,OrderMaster.OrderId,Temp_OrderWiseDyeingHissab.Type
 FROM   Temp_OrderWiseDyeingHissab INNER JOIN OrderMaster ON Temp_OrderWiseDyeingHissab.OrderId=OrderMaster.OrderId INNER JOIN customerinfo ON OrderMaster.CustomerId=customerinfo.CustomerId And Customerinfo.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
 WHERE Temp_OrderWiseDyeingHissab.Type=N'Issue' and OrderMaster.CustomerId=" + DDCustCode.SelectedValue + " And OrderMaster.OrderId=" + DDOrderNo.SelectedValue + " ORDER BY Temp_OrderWiseDyeingHissab.EmpName,Temp_OrderWiseDyeingHissab.ItemName,Temp_OrderWiseDyeingHissab.QualityName,Temp_OrderWiseDyeingHissab.ColorName";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            string str = @"SELECT Temp_OrderWiseDyeingHissab.ItemName,Temp_OrderWiseDyeingHissab.QualityName,Temp_OrderWiseDyeingHissab.ColorName,Temp_OrderWiseDyeingHissab.EmpName,Temp_OrderWiseDyeingHissab.Rate,
 Temp_OrderWiseDyeingHissab.RecQty,Temp_OrderWiseDyeingHissab.OrderId,Temp_OrderWiseDyeingHissab.Type,Temp_OrderWiseDyeingHissab.LossPercent,Temp_OrderWiseDyeingHissab.LossQty
 FROM   Temp_OrderWiseDyeingHissab where Temp_OrderWiseDyeingHissab.Type=N'Receive'
 ORDER BY Temp_OrderWiseDyeingHissab.EmpName,Temp_OrderWiseDyeingHissab.ItemName,Temp_OrderWiseDyeingHissab.QualityName,Temp_OrderWiseDyeingHissab.ColorName,Temp_OrderWiseDyeingHissab.Rate";
            SqlDataAdapter sda = new SqlDataAdapter(str, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ds.Tables.Add(dt);
            ds.Tables[1].Columns.Add("TxtExcessMatBal", typeof(double));
            ds.Tables[1].Columns.Add("Txtprice", typeof(double));
            ds.Tables[1].Columns.Add("TxtexcessMatBalAmt", typeof(double));
            ds.Tables[1].Columns.Add("TxtFinalHissab", typeof(double));
            // ds.Tables[1].Rows.Add(ViewState["TxtExcessMatBal"], ViewState["Txtprice"], ViewState["TxtexcessMatBalAmt"], ViewState["TxtFinalHissab"]);
            ds.Tables[1].Rows[0]["TxtExcessMatBal"] = ViewState["TxtExcessMatBal"];
            ds.Tables[1].Rows[0]["Txtprice"] = ViewState["Txtprice"];
            ds.Tables[1].Rows[0]["TxtexcessMatBalAmt"] = ViewState["TxtexcessMatBalAmt"];
            ds.Tables[1].Rows[0]["TxtFinalHissab"] = ViewState["TxtFinalHissab"];
            Session["dsFileName"] = "~\\ReportSchema\\RptOrderWiseDyeingHissabNEW.xsd";
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Session["rptFileName"] = "~\\Reports\\rpt_rawmeterialstock_detailNEW.rpt";
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            //Session["dsFileName"] = "~\\ReportSchema\\rpt_rawmeterialstock_detailNEW.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void DyerWiseDyingDetail()
    {

        string str = " Select * From V_DyingReport Where Companyid=" + DDCompany.SelectedValue + "";
        if (DDDyerName.SelectedIndex > 0)
        {
            str = str + " And  EmpId=" + DDDyerName.SelectedValue;
        }
        if (DDItem.SelectedIndex > 0)
        {
            str = str + " And Item_Id=" + DDItem.SelectedValue;
        }
        if (DDSubItem.SelectedIndex > 0)
        {
            str = str + " And QualityId=" + DDSubItem.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " And ShadeColorId=" + DDColor.SelectedValue;
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        Session["ReportPath"] = "Reports/RptDyerWiseDyingDetail.rpt";//RptDyerWiseDyingDetail2
        Session["dsFileName"] = "~\\ReportSchema\\RptDyerWiseDyingDetail.Xsd";
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "MsgPopUp('No record Found.....');", true);
        }
    }
    protected void DyeingLedger()
    {
        TxtFdate.Visible = true;
        TxtToDate.Visible = true;
        Session["ReportPath"] = "Reports/RptDyeingLedger.rpt";
        string qry = @"Select CompanyID,Empid,date,empname,typ,billno,hissab,debitbal,'" + TxtFdate.Text + "' FromDate,'" + TxtToDate.Text + "' ToDate From v_dyeingLedger where date >= '" + TxtFdate.Text + "' and date <='" + TxtToDate.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        if (DDDyerName.SelectedIndex > 0)
        {
            qry = qry + " And Empid=" + DDDyerName.SelectedValue;
        }
        Session["dsFileName"] = "~\\ReportSchema\\RptDyeingLedger.xsd";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "MsgPopUp('No record Found.....');", true);
        }
    }
    protected void DyeingHissabDyerWise()
    {
        SqlParameter[] _arrpara = new SqlParameter[5];
        _arrpara[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
        _arrpara[1] = new SqlParameter("@DyerId ", SqlDbType.Int);
        _arrpara[2] = new SqlParameter("@IndentId ", SqlDbType.Int);
        _arrpara[3] = new SqlParameter("@Fromdate ", SqlDbType.SmallDateTime);
        _arrpara[4] = new SqlParameter("@ToDate ", SqlDbType.SmallDateTime);

        _arrpara[0].Value = DDCompany.SelectedValue;
        _arrpara[1].Value = DDDyerName.SelectedIndex > 0 ? DDDyerName.SelectedValue : "0";
        _arrpara[2].Value = DDIndentno.SelectedIndex > 0 ? DDIndentno.SelectedValue : "0";
        _arrpara[3].Value = TxtFdate.Text;
        _arrpara[4].Value = TxtToDate.Text;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_getDyerWiseMatDetail", _arrpara);
        Session["ReportPath"] = "Reports/RptDyeingHissaDyerWise.rpt";
        Session["dsFileName"] = "~\\ReportSchema\\RptDyeingHissaDyerWise.xsd";
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }

    }
}
