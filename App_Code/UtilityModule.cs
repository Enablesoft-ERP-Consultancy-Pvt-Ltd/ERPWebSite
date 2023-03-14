using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
//using System.Web.Mail;
//using System.Net.Mail;
using System.Net.Security;
using System.Text;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;

/// <summary>
/// Summary description for UtilityModule
/// </summary>
public class UtilityModule
{
    public UtilityModule()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static void SendMail(string CredID, string CredPass, int Port, string Host, string Mailfrom, string MailTo, string Subject, string MailBody)
    {
        try
        {
            SmtpClient smtpserver = new SmtpClient();
            MailMessage mail = new System.Net.Mail.MailMessage();
            smtpserver.Credentials = new NetworkCredential(CredID, CredPass);
            smtpserver.Port = Port;
            smtpserver.Host = Host;

            mail.From = new MailAddress(Mailfrom);
            mail.To.Add(MailTo);
            mail.Subject = Subject;
            mail.Body = MailBody;
            smtpserver.EnableSsl = true;
            smtpserver.Send(mail);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Send mail");
        }
    }
    public static string SendMessage(string varMobNo = "", string varMessage = "", int mastercompanyId = 0, string From = "")
    {
        string varSMSUrl, varSMSUserId, varSMSFrom, varSMSPWD, RStr;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //DataSet dsGetSMSDetail = SqlHelper.ExecuteDataset(con, CommandType.Text, "select SMSUrl,SMSUserId,SMSFrom,SMSPWD from Sch_Organization where ID=" + ErpGlobal.orgid + " and GateWay=" + varGateWay);
        DataSet dsGetSMSDetail = SqlHelper.ExecuteDataset(con, CommandType.Text, "select SMSUrl,SMSUserId,SMSFrom,SMSPWD,mastercompanyid from SMSSettings where mastercompanyid=" + mastercompanyId + "");
        if (dsGetSMSDetail.Tables[0].Rows.Count > 0)
        {
            if (dsGetSMSDetail.Tables[0].Rows[0].IsNull("SMSUrl") == false)
            {

                if (From != "")
                {
                    varMessage = varMessage + Environment.NewLine + "From:" + From;
                }
                varMessage = HttpUtility.UrlEncode(varMessage);
                varSMSUrl = dsGetSMSDetail.Tables[0].Rows[0]["SMSUrl"].ToString();
                varSMSUserId = dsGetSMSDetail.Tables[0].Rows[0]["SMSUserId"].ToString();
                varSMSFrom = dsGetSMSDetail.Tables[0].Rows[0]["SMSFrom"].ToString();
                varSMSPWD = dsGetSMSDetail.Tables[0].Rows[0]["SMSPWD"].ToString();



                varSMSUrl = varSMSUrl.Replace("varSMSUserId", varSMSUserId);
                varSMSUrl = varSMSUrl.Replace("varSMSFrom", varSMSFrom);
                varSMSUrl = varSMSUrl.Replace("varSMSPWD", varSMSPWD);
                varSMSUrl = varSMSUrl.Replace("varMobNo", varMobNo);
                varSMSUrl = varSMSUrl.Replace("varSMSText", varMessage);
                varMessage = varSMSUrl;

            }
            WebClient client = new WebClient();
            client.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                Byte[] pageData = client.DownloadData(varMessage);
                RStr = Encoding.ASCII.GetString(pageData);

            }
            catch
            {
                RStr = "";
            }
        }
        else
        {
            RStr = "";
        }
        return RStr;
    }
    public static void SendmessageToWeaver_Vendor_Finisher(string MasterTableName = "", string DetailTable = "", String UniqueColName = "", string EmpIdColName = "", long OrderId = 0, string OrderNo = "", int MasterCompanyId = 0, string FinishedidColName = "", string QtyCOlName = "", string ReqByDate = "", string JobName = "", string UnitName = "")
    {

        try
        {

            string str = @"select  PM." + OrderNo + " as OrderNo,EI.Mobile,EI.EmpName,Sum(PD." + QtyCOlName + @") as TotalQty,vf.item_Name,replace(convert(varchar(11),Max(" + ReqByDate + "),106),' ','-') as reqbydate from " + MasterTableName + "  PM inner join " + DetailTable + @" PD on
                     PM." + UniqueColName + "=PD." + UniqueColName + " inner  join Empinfo EI on EI.EmpId=PM." + EmpIdColName + @"
                     inner join V_finishedItemDetail vf on vf.Item_Finished_id=PD." + FinishedidColName + " where PM." + UniqueColName + "=" + OrderId + @"
                     group by PM." + OrderNo + ", EI.Mobile,EI.EmpName,vf.item_Name";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Mobile"].ToString() != "")
                {
                    string Message = "";
                    int i = 0;
                    string itemandquantity = "";
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (itemandquantity == "")
                        {
                            itemandquantity = ds.Tables[0].Rows[i]["item_name"].ToString() + "  " + ds.Tables[0].Rows[i]["TotalQty"].ToString();
                        }
                        else
                        {
                            itemandquantity = itemandquantity + "," + ds.Tables[0].Rows[i]["item_name"].ToString() + "  " + ds.Tables[0].Rows[i]["TotalQty"].ToString();
                        }
                    }
                    itemandquantity = itemandquantity + " " + UnitName + " " + "For " + JobName + " With P.O  " + ds.Tables[0].Rows[0]["OrderNo"].ToString() + " and submit it by " + ds.Tables[0].Rows[0]["reqbydate"].ToString() + "";
                    //string Item = ds.Tables[0].Rows[0]["Item_Name"].ToString();
                    Message = "Dear " + ds.Tables[0].Rows[0]["EmpName"].ToString() + " Receive  " + itemandquantity + "";
                    UtilityModule.SendMessage(ds.Tables[0].Rows[0]["mobile"].ToString(), Message, MasterCompanyId); ;
                }
                else
                {
                    UtilityModule.MessageAlert("", "Please Fill mobile no");
                }
            }
            else
            {

            }
        }
        catch (Exception ex)
        {

            UtilityModule.MessageAlert(ex.Message, "Please Fill mobile no");
        }


    }
    public static string[] ParameteLabel(int LoginCompanyId)
    {
        String[] ParameterList = new String[8];
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            string strsql = "select Parameter_Id,Parameter_Name from parameter_setting where Company_Id=" + LoginCompanyId + " order by Parameter_Id";
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            int n = ds.Tables[0].Rows.Count;
            if (n > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    ParameterList[i] = ds.Tables[0].Rows[i]["Parameter_Name"].ToString();
                }
            }
        }
        catch
        {

        }
        finally
        {
            con.Dispose();
            con.Close();
        }
        return ParameterList;
    }
    public static void ConditionalComboFillWithDS(ref DropDownList comboname, DataSet ds, int i, bool isSelectText, string selecttext)
    {
        try
        {
            comboname.Items.Clear();
            if (ds.Tables[i].Rows.Count > 0)
            {
                comboname.DataSource = ds.Tables[i];
                comboname.DataTextField = ds.Tables[i].Columns[1].ToString();
                comboname.DataValueField = ds.Tables[i].Columns[0].ToString();
                comboname.DataBind();
                if (isSelectText && selecttext != "")
                {
                    comboname.Items.Insert(0, new ListItem(selecttext, "0"));
                }
            }
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("UtilityModule|ConditionalComboFillWithDS|" + ex.Message);
        }
    }

    public static void ConditionalComboFill(ref DropDownList comboname, string strsql, bool isSelectText, string selecttext)
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            comboname.Items.Clear();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                comboname.DataSource = ds.Tables[0];
                comboname.DataTextField = ds.Tables[0].Columns[1].ToString();
                comboname.DataValueField = ds.Tables[0].Columns[0].ToString();
                comboname.DataBind();
                if (isSelectText && selecttext != "")
                {
                    comboname.Items.Insert(0, new ListItem(selecttext, "0"));
                }
            }
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("UtilityModule|ConditionalComboFill|" + ex.Message);
        }
        finally
        {
            con.Close();
            con.Dispose();

        }
    }
    public static void ItemComboFill(ref DropDownList comboname, string qualityID, string designID, string colorID, string ShapeID, string sizeID, string caregoryID)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            comboname.Items.Clear();

            SqlParameter[] arrpara = new SqlParameter[6];
            arrpara[0] = new SqlParameter("@quality_id", SqlDbType.Int);
            arrpara[1] = new SqlParameter("@design_id", SqlDbType.Int);
            arrpara[2] = new SqlParameter("@color_id", SqlDbType.Int);
            arrpara[3] = new SqlParameter("@shape_id", SqlDbType.Int);
            arrpara[4] = new SqlParameter("@size_id", SqlDbType.Int);
            arrpara[5] = new SqlParameter("@category_id", SqlDbType.Int);

            arrpara[0].Value = qualityID == "" ? "0" : qualityID;
            arrpara[1].Value = designID == "" ? "0" : designID;
            arrpara[2].Value = colorID == "" ? "0" : colorID;
            arrpara[3].Value = ShapeID == "" ? "0" : ShapeID;
            arrpara[4].Value = sizeID == "" ? "0" : sizeID;
            arrpara[5].Value = caregoryID == "" ? "0" : caregoryID;

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "pro_get_item", arrpara);

            if (ds.Tables[0].Rows.Count > 0)
            {
                comboname.Items.Add(new ListItem("--Select Item--", "0"));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    comboname.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                }
            }
        }
        catch (Exception ex)
        {

            Logs.WriteErrorLog("UtilityModule|ItemComboFill_NEW|" + ex.Message);
        }
        finally
        {
            con.Close();
            con.Dispose();

        }

    }
    public static void ConditionalComboFillOnlySelect(ref DropDownList comboname, bool isSelectText, string selecttext)
    {
        try
        {
            comboname.Items.Clear();
            if (isSelectText)
                comboname.Items.Add(new ListItem(selecttext, "0"));
        }
        catch (Exception ex)
        {

            Logs.WriteErrorLog("UtilityModule|ConditionalComboFill|" + ex.Message);
        }

    }

    public static int Get_Max_Id(String ID, String TableName, SqlConnection Tran)
    {
        int Var_Max;
        string str;
        str = "select max(" + ID + ") from [" + TableName + "]";
        str = SqlHelper.ExecuteScalar(Tran, CommandType.Text, str).ToString();
        if (str == "")
        {
            Var_Max = 1;
        }
        else
        {
            Var_Max = Convert.ToInt32(str) + 1;
        }
        return Var_Max;
    }
    public static int CalculatePostFix(string Str)
    {
        int CarpetPostFixValue = 0;
        string sql = "";
        if (variable.VarLoomNoGenerated == "1")
        {
            sql = "Select IsNull(Max(LS.Postfix),0)+1 From LoomStockNo LS(Nolock)  Where 1=1 ";
            if (Str != "")
            {
                sql = sql + " AND LS.PreFix like '" + Str + "%'";
            }
            else
            {
                sql = sql + " AND LS.PreFix like '%'";
            }
        }
        else
        {
            sql = "Select IsNull(Max(CN.Postfix),0)+1 from CarpetNumber CN Where 1=1 ";
            if (Str != "")
            {
                sql = sql + " AND CN.PreFix like '" + Str + "%'";
            }
            else
            {
                sql = sql + " AND CN.PreFix like '%'";
            }
        }


        //if (Str == "")
        //{

        //    //CarpetPostFixValue = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, ));
        //}
        //else
        //{
        //    CarpetPostFixValue = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select IsNull(Max(Postfix),0)+1 from CarpetNumber Where PreFix like '" + Str + "%'"));
        //}
        CarpetPostFixValue = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql));

        return CarpetPostFixValue;
    }
    public static int CalculatePostFixLoom(string Str)
    {
        int CarpetPostFixValue = 0;
        if (Str == "")
        {
            CarpetPostFixValue = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select IsNull(Max(Postfix),0)+1 from LoomstockNo(Nolock) Where PreFix like '%'"));
        }
        else
        {
            CarpetPostFixValue = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select IsNull(Max(Postfix),0)+1 from LoomstockNo(Nolock) Where PreFix like '" + Str + "%'"));
        }

        return CarpetPostFixValue;
    }
    public static void Insert_Into_Carpet_Number(int VarItem_Finished_id, int VarOrderid, int VarRecQty, string VarPreFix, int VarPostFix, int VarCompanyid, int VarProcess_Rec_Id, int VarProcess_Rec_Detail_Id, String VarRecDate, SqlTransaction Tran, int VarProcessId)
    {
        string TStockNo, Str;
        int VarStockNo;
        VarPreFix = VarPreFix.ToUpper();
        try
        {
            for (int i = 0; i < VarRecQty; i++)
            {
                TStockNo = VarPreFix + (VarPostFix).ToString();
                DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select * from CarpetNumber Where TStockNo='" + TStockNo + "'");
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    VarPostFix = VarPostFix + 1;
                    VarRecQty = VarRecQty + 1;
                }
                else
                {
                    VarStockNo = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Isnull(Max(StockNo),0)+1 from CarpetNumber"));
                    Str = "Insert Into CarpetNumber(StockNo,Item_Finished_id,TypeId,Pack,Orderid,TStockNo,PreFix,PostFix,Companyid,Process_Rec_Id,Process_Rec_Detail_Id,Rec_Date,CurrentProStatus,IssRecStatus) Values(" + VarStockNo + "," + VarItem_Finished_id + ",1,0," + VarOrderid + ",'" + TStockNo + "','" + VarPreFix + "'," + VarPostFix + "," + VarCompanyid + "," + VarProcess_Rec_Id + "," + VarProcess_Rec_Detail_Id + ",'" + VarRecDate + "'," + VarProcessId + ",0)";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, Str);
                    VarPostFix = VarPostFix + 1;
                }
            }
            //Select StockNo,Item_Finished_id,TypeId,Pack,Orderid,TStockNo,PreFix,PostFix,Companyid,Process_Rec_Id,Process_Rec_Detail_Id,Rec_Date,Pack_Date from CarpetNumber
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            Logs.WriteErrorLog("UtilityModule|ConditionalComboFill|" + ex.Message);
        }
    }
    public static void StockStockTranTableUpdate(int Item_Finished_Id, int Godownid, int Companyid, string LotNo, double Qty, string TranDate, string RealDateTime, string TableName, int PRTId, SqlTransaction Tran, int TranType, bool BlnStockAddInQty, int TypeId, int Finish_Type, int unitid = 0, string TagNo = "Without Tag No", string BinNo = "")
    {
        string StrSql;
        int StockId;
        if (LotNo == "")
        {
            LotNo = "Without Lot No";
        }

        StrSql = "Select StockID From Stock Where Item_Finished_id=" + Item_Finished_Id + " And Godownid=" + Godownid + " And Companyid=" + Companyid + "and lotno='" + LotNo + "' And FINISHED_TYPE_ID=" + Finish_Type + "  and TagNo='" + TagNo + "' and BinNo='" + BinNo + "'";


        StockId = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, StrSql));

        StrSql = "";
        if (StockId > 0)                        //'Update Stock
        {
            if (BlnStockAddInQty == true)              //''''Qty Addition in Stock
            {
                StrSql = "Update Stock Set QtyInHand=  QtyInHand + " + Qty + " Where StockId=" + StockId + " and Godownid=" + Godownid + "  and CompanyID=" + Companyid;
            }
            else                                            //'''Qty Deduct from Stock  
            {
                StrSql = "Update Stock Set QtyInHand=QtyInHand - " + Qty + " Where StockId=" + StockId + " and Godownid=" + Godownid + "  and CompanyID=" + Companyid;
            }
        }
        else
        {                         //''' New Stock Entry
            StockId = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select IsNull(Max(StockId),0)+1 From Stock"));
            if (BlnStockAddInQty == true)                 //''''Qty Addition in Stock
            {
                StrSql = "Insert Into Stock (StockID,Item_Finished_id,QtyinHand,QtyAssigned,OpenStock,GodownId,Companyid,Price,LotNo,TypeId,Finished_Type_Id,TagNo,BinNo) Values (" + StockId + "," + Item_Finished_Id + "," + Qty + ",0,0," + Godownid + "," + Companyid + ",0,'" + LotNo + "'," + TypeId + "," + Finish_Type + ",'" + TagNo + "','" + BinNo + "')";
            }
            else                                     //''''Qty Deduct from Stock
            {
                //Qty = 0 - Qty;
                StrSql = "Insert Into Stock (StockID,Item_Finished_id,QtyinHand,QtyAssigned,OpenStock,GodownId,Companyid,Price,LotNo,TypeId,Finished_Type_Id,TagNo,BinNo) Values (" + StockId + "," + Item_Finished_Id + ", 0 -" + Qty + ",0,0," + Godownid + "," + Companyid + ",0,'" + LotNo + "'," + TypeId + "," + Finish_Type + ",'" + TagNo + "','" + BinNo + "')";
            }
        }
        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, StrSql);
        //''******* Insertion In StockTrans Table
        StrSql = "";
        int TranId = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select IsNull(Max(StockTranId),0)+1 From StockTran"));
        StrSql = "Insert Into StockTran(Stockid,StockTranId,TranType,Quantity,TranDate,Userid,RealDate,TableName,PRTId,unitid) Values (" + StockId + "," + TranId + ",'" + TranType + "'," + Qty + ",'" + TranDate + "'," + HttpContext.Current.Session["varuserid"] + ",'" + RealDateTime + "','" + TableName + "'," + PRTId + "," + unitid + ")";
        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, StrSql);
    }
    public static void StockStockTranTableUpdateNew(int Item_Finished_Id, int Godownid, int Companyid, string LotNo, double Qty, string TranDate, string RealDateTime, string TableName, int PRTId, SqlTransaction Tran, int TranType, bool BlnStockAddInQty, int TypeId, int Finish_Type, int UnitId = 0, string TagNo = "Without Tag No", string BinNo = "")
    {
        SqlParameter[] param = new SqlParameter[17];
        param[0] = new SqlParameter("@item_Finished_id", Item_Finished_Id);
        param[1] = new SqlParameter("@Godownid", Godownid);
        param[2] = new SqlParameter("@Companyid", Companyid);
        param[3] = new SqlParameter("@Lotno", LotNo);
        param[4] = new SqlParameter("@Qty", Qty);
        param[5] = new SqlParameter("@Trandate", TranDate);
        param[6] = new SqlParameter("@RealDateTime", RealDateTime);
        param[7] = new SqlParameter("@TableName", TableName);
        param[8] = new SqlParameter("@Prtid", PRTId);
        param[9] = new SqlParameter("@TranType", TranType);
        param[10] = new SqlParameter("@BlnStockAddInQty", BlnStockAddInQty);
        param[11] = new SqlParameter("@Type_Id", TypeId);
        param[12] = new SqlParameter("@FInish_Type", Finish_Type);
        param[13] = new SqlParameter("@unitid", UnitId);
        param[14] = new SqlParameter("@TagNo", TagNo);
        param[15] = new SqlParameter("@Userid", HttpContext.Current.Session["varuserid"]);
        param[16] = new SqlParameter("@BInNo", BinNo);

        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_savestockqty", param);

    }
    public static int getItemFinishedId(DropDownList ddItemName, DropDownList ddQuality, DropDownList ddDesign, DropDownList ddColor, DropDownList ddShape, DropDownList ddSize, TextBox ProdCode, SqlTransaction Tran, DropDownList ddShadeColor, string VarOurCode, int MasterCompanyId)
    {
        int itemfinishedid = 0;
        try
        {
            string VarProdCode = ProdCode.Text;
            VarProdCode = "";
            SqlParameter[] _arrPara = new SqlParameter[12];
            _arrPara[0] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@QUALITY_ID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@DESIGN_ID", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@COLOR_ID", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@SHAPE_ID", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@SIZE_ID", SqlDbType.Int);
            _arrPara[6] = new SqlParameter("@DESCRIPTION", SqlDbType.VarChar, 100);
            _arrPara[7] = new SqlParameter("@ITEM_ID", SqlDbType.Int);
            _arrPara[8] = new SqlParameter("@ProCode", SqlDbType.NVarChar);
            _arrPara[9] = new SqlParameter("@SHADECOLOR_ID", SqlDbType.Int);
            _arrPara[10] = new SqlParameter("@VarOurCode", SqlDbType.VarChar, 100);
            _arrPara[11] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);

            _arrPara[0].Direction = ParameterDirection.Output;
            ///ddShadeColor
            _arrPara[1].Value = ddQuality.Visible == true ? Convert.ToInt32(ddQuality.SelectedValue) : 0;
            _arrPara[2].Value = ddDesign.Visible == true ? Convert.ToInt32(ddDesign.SelectedValue) : 0;
            _arrPara[3].Value = ddColor.Visible == true ? Convert.ToInt32(ddColor.SelectedValue) : 0;
            _arrPara[4].Value = ddShape.Visible == true ? Convert.ToInt32(ddShape.SelectedValue) : 0;
            //            _arrPara[5].Value = ddSize.Visible == true ?Convert.ToInt32(ddSize.SelectedValue) : 0;
            _arrPara[5].Value = ddSize.Visible == true ? (ddSize.SelectedIndex < 0 ? 0 : Convert.ToInt32(ddSize.SelectedValue)) : 0;
            _arrPara[6].Value = "";
            _arrPara[7].Value = ddItemName.SelectedIndex <= 0 ? "0" : ddItemName.SelectedValue;
            _arrPara[8].Value = VarProdCode;
            _arrPara[9].Value = ddShadeColor.Visible == true ? Convert.ToInt32(ddShadeColor.SelectedValue) : 0;
            _arrPara[10].Value = VarOurCode;
            _arrPara[11].Value = MasterCompanyId;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_GET_ITEM_FINISHED_ID", _arrPara);
            itemfinishedid = Convert.ToInt32(_arrPara[0].Value);
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            Logs.WriteErrorLog("getItemParametreId|" + ex.Message);
        }
        finally
        {

        }
        return itemfinishedid;
    }
    public static int getItemFinishedId(DropDownList ddItemName, DropDownList ddQuality, DropDownList ddDesign, DropDownList ddColor, DropDownList ddShape, DropDownList ddSize, TextBox ProdCode, DropDownList ddShadeColor, int ProdType, string VarOurCode, int MasterCompanyId)
    {
        int itemfinishedid = 0;
        try
        {
            string VarProdCode = ProdCode.Text;
            if (ProdType == 0)
            {
                VarProdCode = "";
            }
            SqlParameter[] _arrPara = new SqlParameter[12];
            _arrPara[0] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@QUALITY_ID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@DESIGN_ID", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@COLOR_ID", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@SHAPE_ID", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@SIZE_ID", SqlDbType.Int);
            _arrPara[6] = new SqlParameter("@DESCRIPTION", SqlDbType.VarChar, 100);
            _arrPara[7] = new SqlParameter("@ITEM_ID", SqlDbType.Int);
            _arrPara[8] = new SqlParameter("@ProCode", SqlDbType.NVarChar);
            _arrPara[9] = new SqlParameter("@SHADECOLOR_ID", SqlDbType.Int);
            _arrPara[10] = new SqlParameter("@VarOurCode", SqlDbType.VarChar, 100);
            _arrPara[11] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);

            _arrPara[0].Direction = ParameterDirection.Output;
            ///ddShadeColor
            _arrPara[1].Value = ddQuality.Visible == true ? Convert.ToInt32(ddQuality.SelectedValue) : 0;
            _arrPara[2].Value = ddDesign.Visible == true ? Convert.ToInt32(ddDesign.SelectedValue) : 0;
            _arrPara[3].Value = ddColor.Visible == true ? Convert.ToInt32(ddColor.SelectedValue) : 0;
            _arrPara[4].Value = ddShape.Visible == true ? Convert.ToInt32(ddShape.SelectedValue) : 0;
            _arrPara[5].Value = ddSize.Visible == true ? Convert.ToInt32(ddSize.SelectedValue) : 0;
            _arrPara[6].Value = "";
            _arrPara[7].Value = ddItemName.SelectedIndex <= 0 ? "0" : ddItemName.SelectedValue;
            _arrPara[8].Value = VarProdCode;
            _arrPara[9].Value = ddShadeColor.Visible == true ? Convert.ToInt32(ddShadeColor.SelectedValue) : 0;
            _arrPara[10].Value = VarOurCode;
            _arrPara[11].Value = MasterCompanyId;

            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GET_ITEM_FINISHED_ID", _arrPara);

            itemfinishedid = Convert.ToInt32(_arrPara[0].Value);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("getItemParametreId|" + ex.Message);
        }
        finally
        {

        }
        return itemfinishedid;
    }
    public static int getItemFinishedIdWithBuyercode(DropDownList ddItemName, DropDownList ddQuality, DropDownList ddDesign, DropDownList ddColor, DropDownList ddShape, DropDownList ddSize, TextBox ProdCode, DropDownList ddShadeColor, int ProdType, string VarOurCode, int MasterCompanyId)
    {
        int itemfinishedid = 0;
        try
        {
            string VarProdCode = ProdCode.Text;
            if (ProdType == 0)
            {
                VarProdCode = "";
            }
            SqlParameter[] _arrPara = new SqlParameter[12];
            _arrPara[0] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@CQID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@DSRNO", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@CSRNO", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@SHAPE_ID", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@SIZE_ID", SqlDbType.Int);
            _arrPara[6] = new SqlParameter("@DESCRIPTION", SqlDbType.VarChar, 100);
            _arrPara[7] = new SqlParameter("@ITEM_ID", SqlDbType.Int);
            _arrPara[8] = new SqlParameter("@ProCode", SqlDbType.NVarChar);
            _arrPara[9] = new SqlParameter("@SHADECOLOR_ID", SqlDbType.Int);
            _arrPara[10] = new SqlParameter("@VarOurCode", SqlDbType.VarChar, 100);
            _arrPara[11] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);

            _arrPara[0].Direction = ParameterDirection.Output;
            ///ddShadeColor
            _arrPara[1].Value = ddQuality.Visible == true ? ddQuality.SelectedValue : "0";
            _arrPara[2].Value = ddDesign.Visible == true ? ddDesign.SelectedValue : "0";
            _arrPara[3].Value = ddColor.Visible == true ? ddColor.SelectedValue : "0";
            _arrPara[4].Value = ddShape.Visible == true ? Convert.ToInt32(ddShape.SelectedValue) : 0;
            _arrPara[5].Value = ddSize.Visible == true ? Convert.ToInt32(ddSize.SelectedValue) : 0;
            _arrPara[6].Value = "";
            _arrPara[7].Value = ddItemName.SelectedIndex <= 0 ? "0" : ddItemName.SelectedValue;
            _arrPara[8].Value = VarProdCode;
            _arrPara[9].Value = ddShadeColor.Visible == true ? Convert.ToInt32(ddShadeColor.SelectedValue) : 0;
            _arrPara[10].Value = VarOurCode;
            _arrPara[11].Value = MasterCompanyId;

            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GET_ITEM_FINISHED_IDWithBuyercode", _arrPara);

            itemfinishedid = Convert.ToInt32(_arrPara[0].Value);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("getItemParametreId|" + ex.Message);
        }
        finally
        {

        }
        return itemfinishedid;
    }
    public static int getItemFinishedId(DropDownList ddItemName, DropDownList ddQuality, DropDownList ddDesign, DropDownList ddColor, DropDownList ddShape, DropDownList ddSize, TextBox ProdCode, SqlTransaction Tran, DropDownList ddShadeColor, CheckBox ChkDesign, CheckBox ChkColor, CheckBox ChkSize, string VarOurCode, int MasterCompanyId)
    {
        int itemfinishedid = 0;
        try
        {
            string VarProdCode = ProdCode.Text;
            VarProdCode = "";
            SqlParameter[] _arrPara = new SqlParameter[12];
            _arrPara[0] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@QUALITY_ID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@DESIGN_ID", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@COLOR_ID", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@SHAPE_ID", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@SIZE_ID", SqlDbType.Int);
            _arrPara[6] = new SqlParameter("@DESCRIPTION", SqlDbType.VarChar, 100);
            _arrPara[7] = new SqlParameter("@ITEM_ID", SqlDbType.Int);
            _arrPara[8] = new SqlParameter("@ProCode", SqlDbType.NVarChar);
            _arrPara[9] = new SqlParameter("@SHADECOLOR_ID", SqlDbType.Int);
            _arrPara[10] = new SqlParameter("@VarOurCode", SqlDbType.VarChar, 100);
            _arrPara[11] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);

            _arrPara[0].Direction = ParameterDirection.Output;
            ///ddShadeColor
            _arrPara[1].Value = ddQuality.Visible == true ? Convert.ToInt32(ddQuality.SelectedValue) : 0;
            if (ChkDesign.Checked == true)
            {
                _arrPara[2].Value = -1;
            }
            else
            {
                _arrPara[2].Value = ddDesign.Visible == true ? Convert.ToInt32(ddDesign.SelectedValue) : 0;
            }
            if (ChkColor.Checked == true)
            {
                _arrPara[3].Value = -1;
            }
            else
            {
                _arrPara[3].Value = ddColor.Visible == true ? Convert.ToInt32(ddColor.SelectedValue) : 0;
            }
            _arrPara[4].Value = ddShape.Visible == true ? Convert.ToInt32(ddShape.SelectedValue) : 0;
            if (ChkSize.Checked == true)
            {
                _arrPara[5].Value = -1;
            }
            else
            {
                _arrPara[5].Value = ddSize.Visible == true ? Convert.ToInt32(ddSize.SelectedValue) : 0;
            }
            _arrPara[6].Value = "";
            _arrPara[7].Value = ddItemName.SelectedValue;
            _arrPara[8].Value = VarProdCode;
            _arrPara[9].Value = ddShadeColor.Visible == true ? Convert.ToInt32(ddShadeColor.SelectedValue) : 0;
            _arrPara[10].Value = VarOurCode;
            _arrPara[11].Value = MasterCompanyId;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_GET_ITEM_FINISHED_ID", _arrPara);
            itemfinishedid = Convert.ToInt32(_arrPara[0].Value);

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            Logs.WriteErrorLog("getItemParametreId|" + ex.Message);
        }
        finally
        {

        }
        return itemfinishedid;
    }
    public static int getItemFinishedId(DropDownList ddItemName, DropDownList ddQuality, DropDownList ddDesign, DropDownList ddColor, DropDownList ddShape, DropDownList ddSize, TextBox ProdCode, DropDownList ddShadeColor, CheckBox ChkDesign, CheckBox ChkColor, CheckBox ChkSize, string VarOurCode, int MasterCompanyId)
    {
        int itemfinishedid = 0;
        try
        {
            string VarProdCode = ProdCode.Text;
            VarProdCode = "";
            SqlParameter[] _arrPara = new SqlParameter[13];
            _arrPara[0] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@QUALITY_ID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@DESIGN_ID", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@COLOR_ID", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@SHAPE_ID", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@SIZE_ID", SqlDbType.Int);
            _arrPara[6] = new SqlParameter("@DESCRIPTION", SqlDbType.VarChar, 100);
            _arrPara[7] = new SqlParameter("@ITEM_ID", SqlDbType.Int);
            _arrPara[8] = new SqlParameter("@ProCode", SqlDbType.NVarChar);
            _arrPara[9] = new SqlParameter("@SHADECOLOR_ID", SqlDbType.Int);
            _arrPara[10] = new SqlParameter("@VarOurCode", SqlDbType.VarChar, 100);
            _arrPara[11] = new SqlParameter("@MasterCompanyId", SqlDbType.VarChar, 100);

            _arrPara[0].Direction = ParameterDirection.Output;
            ///ddShadeColor
            _arrPara[1].Value = ddQuality.Visible == true ? Convert.ToInt32(ddQuality.SelectedValue) : 0;
            if (ChkDesign.Checked == true)
            {
                _arrPara[2].Value = -1;
            }
            else
            {
                _arrPara[2].Value = ddDesign.Visible == true ? Convert.ToInt32(ddDesign.SelectedValue) : 0;
            }
            if (ChkColor.Checked == true)
            {
                _arrPara[3].Value = -1;
            }
            else
            {
                _arrPara[3].Value = ddColor.Visible == true ? Convert.ToInt32(ddColor.SelectedValue) : 0;
            }
            _arrPara[4].Value = ddShape.Visible == true ? Convert.ToInt32(ddShape.SelectedValue) : 0;
            if (ChkSize.Checked == true)
            {
                _arrPara[5].Value = -1;
            }
            else
            {
                _arrPara[5].Value = ddSize.Visible == true ? Convert.ToInt32(ddSize.SelectedValue) : 0;
            }
            _arrPara[6].Value = "";
            _arrPara[7].Value = ddItemName.SelectedValue;
            _arrPara[8].Value = VarProdCode;
            _arrPara[9].Value = ddShadeColor.Visible == true ? Convert.ToInt32(ddShadeColor.SelectedValue) : 0;
            _arrPara[10].Value = VarOurCode;
            _arrPara[11].Value = MasterCompanyId;

            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GET_ITEM_FINISHED_ID", _arrPara);
            itemfinishedid = Convert.ToInt32(_arrPara[0].Value);

        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("getItemParametreId|" + ex.Message);
        }
        finally
        {

        }
        return itemfinishedid;
    }
    public static int getItemFinishedId(int VarItemId, int VarQualityID, int VarDesignID, int VarColorID, int VarShapeID, int VarSizeID, int VarShadeColorID, string VarOurCode, int MasterCompanyId)
    {
        int itemfinishedid = 0;
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[12];
            _arrPara[0] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@QUALITY_ID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@DESIGN_ID", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@COLOR_ID", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@SHAPE_ID", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@SIZE_ID", SqlDbType.Int);
            _arrPara[6] = new SqlParameter("@DESCRIPTION", SqlDbType.VarChar, 100);
            _arrPara[7] = new SqlParameter("@ITEM_ID", SqlDbType.Int);
            _arrPara[8] = new SqlParameter("@ProCode", SqlDbType.NVarChar);
            _arrPara[9] = new SqlParameter("@SHADECOLOR_ID", SqlDbType.Int);
            _arrPara[10] = new SqlParameter("@VarOurCode", SqlDbType.VarChar, 100);
            _arrPara[11] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);

            _arrPara[0].Direction = ParameterDirection.Output;
            _arrPara[1].Value = VarQualityID;
            _arrPara[2].Value = VarDesignID;
            _arrPara[3].Value = VarColorID;
            _arrPara[4].Value = VarShapeID;
            _arrPara[5].Value = VarSizeID;
            _arrPara[6].Value = "";
            _arrPara[7].Value = VarItemId;
            _arrPara[8].Value = "";
            _arrPara[9].Value = VarShadeColorID;
            _arrPara[10].Value = VarOurCode;
            _arrPara[11].Value = MasterCompanyId;
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GET_ITEM_FINISHED_ID", _arrPara);
            itemfinishedid = Convert.ToInt32(_arrPara[0].Value);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("getItemParametreId|" + ex.Message);
        }
        finally
        {

        }
        return itemfinishedid;
    }
    public static bool VALIDDROPDOWNLIST(DropDownList DD)
    {
        bool VALIDVALUE;
        if (DD.SelectedIndex < 1)
        {
            DD.Focus();

            VALIDVALUE = false;
        }
        else
        {
            VALIDVALUE = true;
        }
        return VALIDVALUE;
    }

    public static void SHOWMSG(Label MSG)
    {
        MSG.Text = "ONE OR MORE MANADATORY FIELDS ARE EMPTY.......";
    }
    public static bool VALIDTEXTBOX(TextBox TXT)
    {
        bool VALIDVALUE;
        if (TXT.Text == "")
        {
            TXT.Focus();
            VALIDVALUE = false;
        }
        else
        {
            VALIDVALUE = true;
        }
        return VALIDVALUE;
    }
    public static void ConditonalChkBoxListFill(ref CheckBoxList CheckBoxName, string strsql)
    {
        try
        {
            CheckBoxName.Items.Clear();
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CheckBoxName.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                }
            }
        }
        catch (Exception ex)
        {

            Logs.WriteErrorLog("UtilityModule|ConditionalComboFill|" + ex.Message);
        }
    }
    public static void ConditonalChkBoxListFillWithDs(ref CheckBoxList CheckBoxName, DataSet ds)
    {
        try
        {
            CheckBoxName.Items.Clear();

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CheckBoxName.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                }
            }
        }
        catch (Exception ex)
        {

            Logs.WriteErrorLog("UtilityModule|ConditionalComboFill|" + ex.Message);
        }
    }

    public static void ConditonalListFill(ref ListBox list, string strsql)
    {
        try
        {
            list.Items.Clear();
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                }
            }
        }
        catch (Exception ex)
        {

            Logs.WriteErrorLog("UtilityModule|ConditionalComboFill|" + ex.Message);
        }
    }

    public static void ORDER_CONSUMPTION_DEFINE(int ITEM_FINISHED_ID, int ORDERRID, SqlTransaction Tran)
    {
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[2];
            _arrPara[0] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@ORDERRID", SqlDbType.Int);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_ORDER_CONSUMPTION_DEFINE", _arrPara);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("getItemParametreId|" + ex.Message);
        }
        finally
        {

        }
    }
    public static void ORDER_CONSUMPTION_DEFINE(int ITEM_FINISHED_ID, int ORDERRID, int ORDERDETAILID, int VARUPDATE_FLAG, int UPDATECURRENTCONSUMPTION, string effectivedate = "")
    {
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[7];
            _arrPara[0] = new SqlParameter("@Item_FINISHED_ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@ORDERID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@ORDERDETAILID", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@VARUPDATE_FLAG", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@UPDATECURRENTCONSUMPTION", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@effectivedate", effectivedate == "" ? System.DateTime.Now.ToString("dd-MMM-yyyy") : effectivedate);
            _arrPara[6] = new SqlParameter("@mastercompanyid", HttpContext.Current.Session["varcompanyid"]);


            // _arrPara[0].Value = Ds1.Tables[0].Rows[0]["ITEM_FINISHED_ID"];
            _arrPara[0].Value = ITEM_FINISHED_ID;
            _arrPara[1].Value = ORDERRID;
            _arrPara[2].Value = ORDERDETAILID;
            _arrPara[3].Value = VARUPDATE_FLAG;
            _arrPara[4].Value = UPDATECURRENTCONSUMPTION;
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_saveOrderConsumption", _arrPara);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("getItemParametreId|" + ex.Message);
        }
        finally
        {

        }
    }
    public static void ORDER_CONSUMPTION_DEFINE(int ITEM_FINISHED_ID, int ORDERRID, int ORDERDETAILID, int VARUPDATE_FLAG, int UPDATECURRENTCONSUMPTION, SqlTransaction Tran, string effectivedate = "")
    {
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[7];
            _arrPara[0] = new SqlParameter("@Item_FINISHED_ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@ORDERID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@ORDERDETAILID", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@VARUPDATE_FLAG", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@UPDATECURRENTCONSUMPTION", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@effectivedate", effectivedate == "" ? System.DateTime.Now.ToString("dd-MMM-yyyy") : effectivedate);
            _arrPara[6] = new SqlParameter("@mastercompanyid", HttpContext.Current.Session["varcompanyid"]);


            // _arrPara[0].Value = Ds1.Tables[0].Rows[0]["ITEM_FINISHED_ID"];
            _arrPara[0].Value = ITEM_FINISHED_ID;
            _arrPara[1].Value = ORDERRID;
            _arrPara[2].Value = ORDERDETAILID;
            _arrPara[3].Value = VARUPDATE_FLAG;
            _arrPara[4].Value = UPDATECURRENTCONSUMPTION;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_saveOrderConsumption", _arrPara);


        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("getItemParametreId|" + ex.Message);
        }
        finally
        {

        }
    }
    //public static void ORDER_CONSUMPTION_DEFINE(int ITEM_FINISHED_ID, int ORDERRID, int ORDERDETAILID, int VARUPDATE_FLAG, int UPDATECURRENTCONSUMPTION, SqlTransaction Tran)
    //{
    //    try
    //    {
    //        SqlParameter[] _arrPara = new SqlParameter[6];
    //        DataSet Ds1;
    //        DataSet Ds2;
    //        string Str = "Select * from Item_ParaMeter_Master IPM,ITem_Master IM Where IPM.Item_Id=IM.Item_Id And IPM.Item_Finished_ID=" + ITEM_FINISHED_ID + "";
    //        DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
    //        if (Ds.Tables[0].Rows.Count > 0)
    //        {
    //            Ds2 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select * from Process_Name_Master Order By Process_Name_ID");
    //            if (Ds2.Tables[0].Rows.Count > 0)
    //            {
    //                for (int i = 0; i < Ds2.Tables[0].Rows.Count; i++)
    //                {
    //                    Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And ProcessID=" + Ds2.Tables[0].Rows[i]["Process_Name_ID"] + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + "";
    //                    Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
    //                    if (Ds1.Tables[0].Rows.Count == 0)
    //                    {
    //                        Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And ProcessID=" + Ds2.Tables[0].Rows[i]["Process_Name_ID"] + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=-1";
    //                        Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
    //                    }
    //                    if (Ds1.Tables[0].Rows.Count == 0)
    //                    {
    //                        Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And ProcessID=" + Ds2.Tables[0].Rows[i]["Process_Name_ID"] + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=-1 And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + "";
    //                        Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
    //                    }
    //                    if (Ds1.Tables[0].Rows.Count == 0)
    //                    {
    //                        Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And ProcessID=" + Ds2.Tables[0].Rows[i]["Process_Name_ID"] + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=-1 And Size_ID=-1";
    //                        Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
    //                    }
    //                    if (Ds1.Tables[0].Rows.Count == 0)
    //                    {
    //                        Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And ProcessID=" + Ds2.Tables[0].Rows[i]["Process_Name_ID"] + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + "";
    //                        Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
    //                    }
    //                    if (Ds1.Tables[0].Rows.Count == 0)
    //                    {
    //                        Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And ProcessID=" + Ds2.Tables[0].Rows[i]["Process_Name_ID"] + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=-1";
    //                        Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
    //                    }
    //                    if (Ds1.Tables[0].Rows.Count == 0)
    //                    {
    //                        Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And ProcessID=" + Ds2.Tables[0].Rows[i]["Process_Name_ID"] + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=-1 And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + "";
    //                        Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
    //                    }
    //                    if (Ds1.Tables[0].Rows.Count == 0)
    //                    {
    //                        Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And ProcessID=" + Ds2.Tables[0].Rows[i]["Process_Name_ID"] + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=-1 And Size_ID=-1";
    //                        Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
    //                    }
    //                    if (Ds1.Tables[0].Rows.Count > 0)
    //                    {
    //                        _arrPara[0] = new SqlParameter("@FINISHEDID", SqlDbType.Int);
    //                        _arrPara[1] = new SqlParameter("@ORDERID", SqlDbType.Int);
    //                        _arrPara[2] = new SqlParameter("@ORDERDETAILID", SqlDbType.Int);
    //                        _arrPara[3] = new SqlParameter("@VARUPDATE_FLAG", SqlDbType.Int);
    //                        _arrPara[4] = new SqlParameter("@UPDATECURRENTCONSUMPTION", SqlDbType.Int);
    //                        _arrPara[5] = new SqlParameter("@PROCESSID", SqlDbType.Int);

    //                        _arrPara[0].Value = Ds1.Tables[0].Rows[0]["ITEM_FINISHED_ID"];
    //                        _arrPara[1].Value = ORDERRID;
    //                        _arrPara[2].Value = ORDERDETAILID;
    //                        _arrPara[3].Value = VARUPDATE_FLAG;
    //                        _arrPara[4].Value = UPDATECURRENTCONSUMPTION;
    //                        _arrPara[5].Value = Ds2.Tables[0].Rows[i]["Process_Name_ID"];
    //                        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_ORDER_CONSUMPTION_DEFINE", _arrPara);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Logs.WriteErrorLog("getItemParametreId|" + ex.Message);
    //    }
    //    finally
    //    {

    //    }
    //}
    //public static void PROCESS_CONSUMPTION_DEFINE(int PROCESS_ISSUE_ID, int PROCESS_ISSUE_DETAIL_ID, int ITEM_FINISHED_ID, int PROCESS_ID, SqlTransaction Tran)

    //public static void PROCESS_CONSUMPTION_DEFINE(int PROCESS_ISSUE_ID, int PROCESS_ISSUE_DETAIL_ID, int ITEM_FINISHED_ID, int PROCESS_ID, int ORDER_ID, SqlTransaction Tran)
    //{
    //    try
    //    {
    //        SqlParameter[] _arrPara = new SqlParameter[6];
    //        DataSet Ds1;
    //        string Str = "Select Category_id,IPM.Item_Id,IPM.Quality_Id,IPM.Shape_id,IPM.Design_Id,IPM.Color_Id,IPM.Size_Id from Item_ParaMeter_Master IPM,ITem_Master IM Where IPM.Item_Id=IM.Item_Id And IPM.Item_Finished_ID=" + ITEM_FINISHED_ID + "";
    //        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
    //        if (Ds.Tables[0].Rows.Count > 0)
    //        {
    //            Str = "Select IPCM.Item_Finished_id from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And PM.PROCESSID=" + PROCESS_ID + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + "";
    //            Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
    //            if (Ds1.Tables[0].Rows.Count == 0)
    //            {
    //                Str = "Select IPCM.Item_Finished_id from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And PM.PROCESSID=" + PROCESS_ID + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=-1";
    //                Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
    //            }
    //            if (Ds1.Tables[0].Rows.Count == 0)
    //            {
    //                Str = "Select IPCM.Item_Finished_Id from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And PM.PROCESSID=" + PROCESS_ID + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=-1 And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + "";
    //                Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
    //            }
    //            if (Ds1.Tables[0].Rows.Count == 0)
    //            {
    //                Str = "Select IPCM.Item_Finished_id from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And PM.PROCESSID=" + PROCESS_ID + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=-1 And Size_ID=-1";
    //                Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
    //            }
    //            if (Ds1.Tables[0].Rows.Count == 0)
    //            {
    //                Str = "Select IPCM.Item_Finished_Id from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And PM.PROCESSID=" + PROCESS_ID + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + "";
    //                Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
    //            }
    //            if (Ds1.Tables[0].Rows.Count == 0)
    //            {
    //                Str = "Select IPCM.Item_Finished_Id from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And PM.PROCESSID=" + PROCESS_ID + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=-1";
    //                Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
    //            }
    //            if (Ds1.Tables[0].Rows.Count == 0)
    //            {
    //                Str = "Select IPCM.Item_Finished_Id from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And PM.PROCESSID=" + PROCESS_ID + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=-1 And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + "";
    //                Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
    //            }
    //            if (Ds1.Tables[0].Rows.Count == 0)
    //            {
    //                Str = "Select IPCM.Item_Finished_id from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And PM.PROCESSID=" + PROCESS_ID + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=-1 And Size_ID=-1";
    //                Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
    //            }
    //            if (Ds1.Tables[0].Rows.Count > 0)
    //            {
    //                _arrPara[0] = new SqlParameter("@PROCESS_ISSUE_ID", SqlDbType.Int);
    //                _arrPara[1] = new SqlParameter("@PROCESS_ISSUE_DETAIL_ID", SqlDbType.Int);
    //                _arrPara[2] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
    //                _arrPara[3] = new SqlParameter("@PROCESS_ID", SqlDbType.Int);
    //                _arrPara[4] = new SqlParameter("@ORDER_ID", SqlDbType.Int);
    //                _arrPara[5] = new SqlParameter("@PROCESS_FINISHED_ID", SqlDbType.Int);

    //                _arrPara[0].Value = PROCESS_ISSUE_ID;
    //                _arrPara[1].Value = PROCESS_ISSUE_DETAIL_ID;
    //                _arrPara[2].Value = Ds1.Tables[0].Rows[0]["ITEM_FINISHED_ID"];
    //                _arrPara[3].Value = PROCESS_ID;
    //                _arrPara[4].Value = ORDER_ID;
    //                _arrPara[5].Value = ITEM_FINISHED_ID;
    //                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_PROCESS_CONSUMPTION_DEFINE", _arrPara);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Logs.WriteErrorLog("getItemParametreId|" + ex.Message);
    //    }
    //    finally
    //    {

    //    }
    //}
    public static void PROCESS_CONSUMPTION_DEFINE(int PROCESS_ISSUE_ID, int PROCESS_ISSUE_DETAIL_ID, int ITEM_FINISHED_ID, int PROCESS_ID, int ORDER_ID, SqlTransaction Tran, string effectivedate = "")
    {
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[7];

            _arrPara[0] = new SqlParameter("@PROCESS_ISSUE_ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@PROCESS_ISSUE_DETAIL_ID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@ORDER_ID", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@Process_ID", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@effectivedate", effectivedate == "" ? System.DateTime.Now.ToString("dd-MMM-yyyy") : effectivedate);
            _arrPara[6] = new SqlParameter("@mastercompanyid", HttpContext.Current.Session["varcompanyid"]);


            _arrPara[0].Value = PROCESS_ISSUE_ID;
            _arrPara[1].Value = PROCESS_ISSUE_DETAIL_ID;
            _arrPara[2].Value = ITEM_FINISHED_ID;
            _arrPara[3].Value = ORDER_ID;
            _arrPara[4].Value = PROCESS_ID;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_saveProcessConsumption", _arrPara);


        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("getItemParametreId|" + ex.Message);
        }
        finally
        {

        }
    }
    public static void LogOut(int varuserid)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string str = "Update NewUserdetail Set LogInFlag=0 where UserId=" + varuserid;
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, str);
        }
        catch
        {

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    public static void NewComboFill(DropDownList comboname, string strsql)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            //            //comboname.Items.Clear();
            //            //VarcomboValue = 0
            //            DataSet ds = SqlHelper.ExecuteDataset(con , CommandType.Text, strsql);

            //            if (ds.Tables[0].Rows.Count > 0)
            //            {
            //                comboname.DataSource = ds.Tables[0];
            //                comboname.v
            //                comboname.ValueMember = ds.Tables[0].Columns(0).ColumnName;
            //                comboname.DisplayMember = ds.Tables[0].Columns(1).ColumnName;
            //                comboname.SelectedIndex = -1
            //                //VarcomboValue = 1

            //            }

            DataTable table = new DataTable();
            //table = FillMyDataTable(); //a method that returns required DataTable in our case
            DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select * from ");
            //table = FillMyDataTable(); //a method that returns required DataTable in our case
            comboname.DataSource = table;
            comboname.DataValueField = "myID_Column";
            comboname.DataBind();
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("UtilityModule|ConditionalComboFill|" + ex.Message);
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    public static void StockStockTranTableUpdate2(int Item_Finished_Id, int Godownid, int Companyid, string LotNo, double Qty, string TranDate, string RealDateTime, string TableName, int PRTId, SqlTransaction Tran, int TranType, bool BlnStockAddInQty, int TypeId, bool update, Double PriviousQty, int Finish_Type, int unitid = 0, string TagNo = "Without Tag No", string BinNo = "")
    {
        string StrSql;
        int StockId;
        if (LotNo == "")
        {
            LotNo = "Without Lot No";
        }
        StrSql = "Select StockID From Stock Where Item_Finished_id=" + Item_Finished_Id + " And Godownid=" + Godownid + " And Companyid=" + Companyid + " And Finished_Type_Id=" + Finish_Type + "  And LotNo='" + LotNo + "' and Tagno='" + TagNo + "' and BinNo='" + BinNo + "'";
        StockId = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, StrSql));
        if (update == true)
        {
            StrSql = "Update Stock Set QtyInHand=  QtyInHand -" + PriviousQty + " + " + Qty + " Where StockId=" + StockId + " and Godownid=" + Godownid + "  and CompanyID=" + Companyid;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, StrSql);
            int stocktranid = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select stocktranid from stocktran where trantype='1'and tablename='PP_ProcessrecDetail' and prtid='" + PRTId + "'"));
            StrSql = "Update StockTran set Quantity=" + Qty + " where TranType=" + TranType + " And TableName='" + TableName + "' And PRTId=" + PRTId + " And Stockid=" + StockId + "";
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, StrSql);
        }
        else
        {
            // Updation in Stock Table
            StrSql = "";
            if (StockId > 0)                        //'Update Stock
            {
                if (BlnStockAddInQty == true)              //''''Qty Addition in Stock
                {
                    StrSql = "Update Stock Set QtyInHand=  QtyInHand + " + Qty + " Where StockId=" + StockId + " and Godownid=" + Godownid + "  and CompanyID=" + Companyid;
                }
                else                                            //'''Qty Deduct from Stock  
                {
                    StrSql = "Update Stock Set QtyInHand=QtyInHand - " + Qty + " Where StockId=" + StockId + " and Godownid=" + Godownid + "  and CompanyID=" + Companyid;
                }
            }
            else
            {                         //''' New Stock Entry
                StockId = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select IsNull(Max(StockId),0)+1 From Stock"));
                if (BlnStockAddInQty == true)                 //''''Qty Addition in Stock
                {
                    StrSql = "Insert Into Stock (StockID,Item_Finished_id,QtyinHand,QtyAssigned,OpenStock,GodownId,Companyid,Price,LotNo,TypeId,TagNo,BinNo) Values (" + StockId + "," + Item_Finished_Id + "," + Qty + ",0,0," + Godownid + "," + Companyid + ",0,'" + LotNo + "'," + TypeId + ",'" + TagNo + "','" + BinNo + "')";
                }
                else                                     //''''Qty Deduct from Stock
                {
                    Qty = 0 - Qty;
                    StrSql = "Insert Into Stock (StockID,Item_Finished_id,QtyinHand,QtyAssigned,OpenStock,GodownId,Companyid,Price,LotNo,TypeId,TagNo,BinNo) Values (" + StockId + "," + Item_Finished_Id + "," + Qty + ",0,0," + Godownid + "," + Companyid + ",0,'" + LotNo + "'," + TypeId + ",'" + TagNo + "','" + BinNo + "')";
                }
            }
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, StrSql);
            //''******* Insertion In StockTrans Table
            StrSql = "";
            int TranId = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select IsNull(Max(StockTranId),0)+1 From StockTran"));
            StrSql = "Insert Into StockTran(Stockid,StockTranId,TranType,Quantity,TranDate,Userid,RealDate,TableName,PRTId,unitid) Values (" + StockId + "," + TranId + ",'" + TranType + "'," + Qty + ",'" + TranDate + "',0,'" + RealDateTime + "','" + TableName + "'," + PRTId + "," + unitid + ")";
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, StrSql);
        }
    }
    public static void DRAFT_ORDER_CONSUMPTION_DEFINE(int ITEM_FINISHED_ID, int ORDERRID, int ORDERDETAILID, int VARUPDATE_FLAG, int UPDATECURRENTCONSUMPTION, SqlTransaction Tran)
    {
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[5];
            DataSet Ds1;
            string Str = "Select * from Item_ParaMeter_Master IPM,ITem_Master IM Where IPM.Item_Id=IM.Item_Id And IPM.Item_Finished_ID=" + ITEM_FINISHED_ID + "";
            DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + "";
                Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                if (Ds1.Tables[0].Rows.Count == 0)
                {
                    Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=-1";
                    Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                }
                if (Ds1.Tables[0].Rows.Count == 0)
                {
                    Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=-1 And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + "";
                    Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                }
                if (Ds1.Tables[0].Rows.Count == 0)
                {
                    Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=-1 And Size_ID=-1";
                    Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                }
                if (Ds1.Tables[0].Rows.Count == 0)
                {
                    Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + "";
                    Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                }
                if (Ds1.Tables[0].Rows.Count == 0)
                {
                    Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=-1";
                    Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                }
                if (Ds1.Tables[0].Rows.Count == 0)
                {
                    Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=-1 And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + "";
                    Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                }
                if (Ds1.Tables[0].Rows.Count == 0)
                {
                    Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=-1 And Size_ID=-1";
                    Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                }
                if (Ds1.Tables[0].Rows.Count > 0)
                {
                    _arrPara[0] = new SqlParameter("@FINISHEDID", SqlDbType.Int);
                    _arrPara[1] = new SqlParameter("@ORDERID", SqlDbType.Int);
                    _arrPara[2] = new SqlParameter("@ORDERDETAILID", SqlDbType.Int);
                    _arrPara[3] = new SqlParameter("@VARUPDATE_FLAG", SqlDbType.Int);
                    _arrPara[4] = new SqlParameter("@UPDATECURRENTCONSUMPTION", SqlDbType.Int);
                    _arrPara[0].Value = Ds1.Tables[0].Rows[0]["ITEM_FINISHED_ID"];
                    _arrPara[1].Value = ORDERRID;
                    _arrPara[2].Value = ORDERDETAILID;
                    _arrPara[3].Value = VARUPDATE_FLAG;
                    _arrPara[4].Value = UPDATECURRENTCONSUMPTION;
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DRAFT_ORDER_CONSUMPTION_DEFINE", _arrPara);
                }

            }
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("getItemParametreId|" + ex.Message);
        }
        finally
        {

        }
    }


    public static void StockUpdate(int Item_Finished_Id, int Godownid, int Companyid, int UserId, string LotNo, double Qty, double TagQtyAssigned, string TranDate, string TableName, int PRTId, SqlTransaction Tran, int TranType_0_for_Issue_1_for_Receive)
    {
        SqlParameter[] _arrpara = new SqlParameter[16];
        _arrpara[0] = new SqlParameter("@Item_Finished_Id", SqlDbType.Int);
        _arrpara[1] = new SqlParameter("@QtyAssigned", SqlDbType.Float);
        _arrpara[2] = new SqlParameter("@GodownId", SqlDbType.Int);
        _arrpara[3] = new SqlParameter("@CompanyId", SqlDbType.Int);
        _arrpara[4] = new SqlParameter("@LotNo", SqlDbType.NVarChar, 50);
        _arrpara[5] = new SqlParameter("@TranType", SqlDbType.Int);
        _arrpara[6] = new SqlParameter("@Quantity", SqlDbType.Float);
        _arrpara[7] = new SqlParameter("@TranDate", SqlDbType.DateTime);
        _arrpara[8] = new SqlParameter("@UserId", SqlDbType.Int);
        _arrpara[9] = new SqlParameter("@Realdate", SqlDbType.DateTime);
        _arrpara[10] = new SqlParameter("@TableName", SqlDbType.NVarChar, 50);
        _arrpara[11] = new SqlParameter("@PRTid", SqlDbType.Int);

        _arrpara[0].Value = Item_Finished_Id;
        _arrpara[1].Value = TagQtyAssigned;
        _arrpara[2].Value = Godownid;
        _arrpara[3].Value = Companyid;
        _arrpara[4].Value = LotNo != "" ? LotNo : "Without Lot No";
        _arrpara[5].Value = TranType_0_for_Issue_1_for_Receive;
        _arrpara[6].Value = Qty;
        _arrpara[7].Value = TranDate;
        _arrpara[8].Value = UserId;
        _arrpara[9].Value = DateTime.Now.ToString("dd-MMM-yyyy");
        _arrpara[10].Value = TableName;
        _arrpara[11].Value = PRTId;
        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateStockIssueReceive");
    }
    public static void Insert_Into_Process_Stock_Detail(int FromProcessId, int ToProcessId, int OrderId, String OrderDate, String RecDate, int CompanyId, int ReceiveDetailId, int UserId, int IssueDetailId, int TypeId, SqlTransaction Tran)
    {
        SqlParameter[] _arrpar = new SqlParameter[11];
        _arrpar[0] = new SqlParameter("@StockNo", SqlDbType.Int);
        _arrpar[1] = new SqlParameter("@FromProcessId", SqlDbType.Int);
        _arrpar[2] = new SqlParameter("@ToProcessId", SqlDbType.Int);
        _arrpar[3] = new SqlParameter("@OrderId", SqlDbType.Int);
        _arrpar[4] = new SqlParameter("@OrderDate", SqlDbType.DateTime);
        _arrpar[5] = new SqlParameter("@ReceiveDate", SqlDbType.DateTime);
        _arrpar[6] = new SqlParameter("@CompanyId", SqlDbType.Int);
        _arrpar[7] = new SqlParameter("@ReceiveDetailId", SqlDbType.Int);
        _arrpar[8] = new SqlParameter("@UserId", SqlDbType.Int);
        _arrpar[9] = new SqlParameter("@IssueDetailId", SqlDbType.Int);
        _arrpar[10] = new SqlParameter("@VarTypeId", SqlDbType.Int);

        _arrpar[1].Value = FromProcessId;
        _arrpar[2].Value = ToProcessId;
        _arrpar[3].Value = OrderId;
        _arrpar[4].Value = OrderDate;
        _arrpar[5].Value = RecDate;
        _arrpar[6].Value = CompanyId;
        _arrpar[7].Value = ReceiveDetailId;
        _arrpar[8].Value = UserId;
        _arrpar[9].Value = IssueDetailId;
        _arrpar[10].Value = TypeId;
        DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select * from CarpetNumber Where Process_Rec_Detail_Id=" + ReceiveDetailId + "");
        for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
        {
            _arrpar[0].Value = Ds.Tables[0].Rows[i]["StockNo"];
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_ProcessStockDetail", _arrpar);
        }
    }
    public static void PROCESS_RECEIVE_CONSUMPTION(int VarProcess_Rec_Detail_Id, int VarProcess_Rec_Id, int VarFinishedid, int VarProcessId, double VarArea, double VarWeight, int VarUnitID, int VarIssue_Detail_Id, int VarIssue_Order_Id, SqlTransaction Tran, int VarFlagFixOrWeight, int VarQty, int VarCalType)
    {
        string Str = "";
        if (VarUnitID == 1)
        {
            VarArea = Math.Round(VarArea, 2);
        }
        else
        {
            VarArea = Math.Round(VarArea, 4);
        }
        switch (HttpContext.Current.Session["varcompanyNo"].ToString())
        {
            case "15":
                VarCalType = 0;
                break;
        }
        try
        {
            //Select Process_Rec_Detail_Id,Process_Rec_Id,Finishedid,ProcessId,Area,Weight,IFinishedid,Qty,TConsmp from PROCESS_RECEIVE_CONSUMPTION
            if (VarFlagFixOrWeight == 1)
            {
                Str = "Insert into PROCESS_RECEIVE_CONSUMPTION (Process_Rec_Detail_Id, Process_Rec_Id, Finishedid, ProcessId, Area, Weight, IFinishedid, Qty, TConsmp, Rate, Loss, IssueOrderId, TLoss, RecQty) Select " + VarProcess_Rec_Detail_Id + "," + VarProcess_Rec_Id + "," + VarFinishedid + @",
                      " + VarProcessId + "," + VarArea + "," + VarWeight + @",IFINISHEDID,
                      Round(Case When 1=" + VarUnitID + @" Then Case When PCD.MasterCompanyId<>9 Then Sum(IQTY*1.196) Else Sum(IQty) End Else case When PCD.MasterCompanyId<>9 Then Sum(IQTY) Else Sum(IQty)/10.76391 End End,3) QTY,
                      Sum(Round(CASE WHEN " + VarCalType + "=1 THEN CASE WHEN " + VarUnitID + @"=1 Then Case When PCD.MasterCompanyId<>9 Then " + VarQty + "*IQTY*1.196 Else " + VarQty + "*IQTY End  else Case When PCD.MasterCompanyId<>9 Then  " + VarQty + @"*IQTY Else " + VarQty + @"*IQTY/10.76391  End
                      END ELSE CASE WHEN " + VarUnitID + @"=1 Then Case When PCD.MasterCompanyId<>9 Then " + VarArea + "*IQTY*1.196 Else " + VarArea + "*IQTY End  else case When PCD.MasterCompanyId<>9 Then " + VarArea + @"*IQTY Else " + VarArea + @"*IQTY/10.76391 END END END,3)) QTY,
                      IRATE,Round(Case When 1=" + VarUnitID + " Then Case When PCD.MasterCompanyId<>9 Then  Sum(ILOSS*1.196) Else Sum(ILOSS) End Else case When PCD.MasterCompanyId<>9 Then Sum(ILOSS) Else Sum(ILoSS)/10.76391 END End,3) ILOSS," + VarIssue_Order_Id + @", 
                      Round(Case When 1=" + VarUnitID + " Then case When PCD.MasterCompanyId<>9 Then  Sum(ILOSS*1.196) Else Sum(ILOSS)  End Else Case When PCD.MasterCompanyId<>9 Then Sum(ILOSS) Else Sum(ILOSS)/10.76391 End End,3)*" + VarArea + "," + VarQty + @"
                      From PROCESS_CONSUMPTION_DETAIL PCD,V_FinishedItemDetail VF Where PCD.IFINISHEDID=VF.ITEM_FINISHED_ID And PROCESSID=" + VarProcessId + " And FlagFixWeight=0 And Issue_Detail_ID=" + VarIssue_Detail_Id + " And ISSUEORDERID=" + VarIssue_Order_Id + " Group By IFINISHEDID,IRATE,ILOSS,PCD.MasterCompanyId";
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, Str);

                DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select Isnull(Sum(Isnull(TConsmp,0)),0) TotalConsmp From PROCESS_RECEIVE_CONSUMPTION Where Process_Rec_Id=" + VarProcess_Rec_Id + " And ProcessId=" + VarProcessId + " And Process_Rec_Detail_Id=" + VarProcess_Rec_Detail_Id);
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    double consmp = Convert.ToDouble(Ds.Tables[0].Rows[0]["TotalConsmp"]);
                    VarWeight = Math.Round(VarWeight - Convert.ToDouble(Ds.Tables[0].Rows[0]["TotalConsmp"]), 3);
                }



                Str = "Insert into PROCESS_RECEIVE_CONSUMPTION (Process_Rec_Detail_Id, Process_Rec_Id, Finishedid, ProcessId, Area, Weight, IFinishedid, Qty, TConsmp, Rate, Loss, IssueOrderId, TLoss, RecQty) Select " + VarProcess_Rec_Detail_Id + "," + VarProcess_Rec_Id + "," + VarFinishedid + @",
                      " + VarProcessId + "," + VarArea + "," + VarWeight + @",IFINISHEDID,
                      Round(Case When 1=" + VarUnitID + @" Then Case When PCD.MasterCompanyId<>9 Then  Sum(IQTY*1.196) Else Sum(IQty) End Else case When PCD.MasterCompanyId<>9 Then Sum(IQTY) Else Sum(IQty)/10.76391 End End,3) QTY,
                      Round(Case When 1=" + VarUnitID + " Then Case When PCD.MasterCompanyId<>9 Then  (Sum(IQTY*1.196)*" + VarArea + "*" + VarWeight + ")/(" + VarArea + "*[dbo].[Get_Process_Total_Consmp](" + VarIssue_Detail_Id + "," + VarUnitID + "," + VarProcessId + @"))  Else (Sum(IQTY)*" + VarArea + "*" + VarWeight + ")/(" + VarArea + "*[dbo].[Get_Process_Total_Consmp](" + VarIssue_Detail_Id + "," + VarUnitID + "," + VarProcessId + @")) End
                      Else  (Case When PCD.MasterCompanyId<>9 Then Sum(IQTY) Else Sum(IQty)/10.76391 End *" + VarArea + "*" + VarWeight + ")/(" + VarArea + "*[dbo].[Get_Process_Total_Consmp](" + VarIssue_Detail_Id + "," + VarUnitID + "," + VarProcessId + @")) End,3) TConsmp,
                      IRATE,Round(Case When 1=" + VarUnitID + " Then case When PCD.MasterCompanyId<>9 Then Sum(ILOSS*1.196) Else Sum(ILOss) End Else case When PCD.MasterCompanyId<>9 Then  Sum(ILOSS)  Else Sum(ILoss)/10.76391 End End,3) ILOSS," + VarIssue_Order_Id + @", 
                      Round(Case When 1=" + VarUnitID + " Then case When PCD.MasterCompanyId<>9 Then  Sum(ILOSS*1.196) Else Sum(ILoss) End Else case When PCD.MasterCompanyId<>9 Then  Sum(ILOSS)  Else Sum(ILoss)/10.76391  End End,3)*" + VarArea + "," + VarQty + @"
                      From PROCESS_CONSUMPTION_DETAIL PCD,V_FinishedItemDetail VF Where PCD.IFINISHEDID=VF.ITEM_FINISHED_ID And PROCESSID=" + VarProcessId + " And FlagFixWeight=1 And ISSUEORDERID=" + VarIssue_Order_Id + " And Issue_Detail_ID=" + VarIssue_Detail_Id + " Group By IFINISHEDID,IRATE,ILOSS,PCD.MasterCompanyId";
            }
            else
            {
                Str = "Insert into PROCESS_RECEIVE_CONSUMPTION (Process_Rec_Detail_Id, Process_Rec_Id, Finishedid, ProcessId, Area, Weight, IFinishedid, Qty, TConsmp, Rate, Loss, IssueOrderId, TLoss, RecQty) Select " + VarProcess_Rec_Detail_Id + "," + VarProcess_Rec_Id + "," + VarFinishedid + @",
                      " + VarProcessId + "," + VarArea + "," + VarWeight + @",IFINISHEDID,
                      Round(Case When 1=" + VarUnitID + @" Then case When MasterCompanyId<>9 Then Sum(IQTY*1.196) Else Sum(IQty) End Else Case When MasterCompanyId<>9 Then Sum(IQTY) Else Sum(IQty)/10.76391 End,3) QTY,
                      Sum(Round(CASE WHEN " + VarCalType + "=1 THEN CASE WHEN " + VarUnitID + @"=1 Then Case When MasterCompanyId<>9 Then  " + VarQty + "*IQTY*1.196 Else  " + VarQty + "*IQTY End else case When MasterCompanyId<>9 Then " + VarQty + @"*IQTY Else " + VarQty + @"*IQTY/10.76391  End
                      END ELSE CASE WHEN " + VarUnitID + @"=1 Then case When MasterCompanyId<>9 Then  " + VarArea + "*IQTY*1.196 Else  " + VarArea + "*IQTY End else case When MasterCompanyId<>9 Then " + VarArea + @"*IQTY Else " + VarArea + @"*IQTY/10.76391  End END END,3)) QTY,
                      IRATE,Round(Case When 1=" + VarUnitID + " Then Case When MasterCompanyId<>9 Then Sum(ILOSS*1.196) Else Sum(ILoss) End Else case When MasterCompanyId<>9 Then Sum(ILOSS) Else Sum(ILOSS)/10.76391  End End,3) ILOSS," + VarIssue_Order_Id + @", 
                      Round(Case When 1=" + VarUnitID + " Then case When MasterCompanyId<>9 Then Sum(ILOSS*1.196) Else Sum(ILoss) End Else Case When MasterCompanyId<>9 Then Sum(ILOSS) Else Sum(ILoss)/10.76391 End End,3)*" + VarArea + "," + VarQty + @"
                      From PROCESS_CONSUMPTION_DETAIL Where Issue_Detail_ID=" + VarIssue_Detail_Id + " And PROCESSID=" + VarProcessId + " And ISSUEORDERID=" + VarIssue_Order_Id + " Group By IFINISHEDID,IRATE,ILOSS,MasterCompanyId";
            }
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, Str);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("getItemParametreId|" + ex.Message);
        }
    }

    //public static float PROCESS_RATE(int ITEM_FINISHED_ID, int ORDER_ID, int PROCESS_ID, SqlTransaction Tran)
    //{
    //    int VarRate = 0;
    //    string Str = "SELECT DISTINCT ORATE From ORDER_CONSUMPTION_DETAIL WHERE ORDERID=" + ORDER_ID + " AND ORDERDETAILID IN (SELECT ORDERDETAILID FROM ORDERDETAIL WHERE ORDERID=" + ORDER_ID + " AND ITEM_FINISHED_ID=" + ITEM_FINISHED_ID + ") AND PROCESSID=" + PROCESS_ID + "";
    //    DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
    //    if (Ds.Tables[0].Rows.Count > 0)
    //    {
    //        VarRate = Convert.ToInt32(Ds.Tables[0].Rows[0]["ORATE"]);
    //    }
    //    return VarRate;
    //}
    public static double PROCESS_RATE(int ITEM_FINISHED_ID, int ORDER_ID, int PROCESS_ID, SqlTransaction Tran, int mastercompanyid = 0, string effectivedate = null, int Caltype = 0, int OrderUnitId=0)
    {
        double VarRate = 0;
        if (mastercompanyid == 4) //Deepak rugs
        {
            DataSet Ds1;
            string Str = "Select * from Item_ParaMeter_Master IPM,ITem_Master IM Where IPM.Item_Id=IM.Item_Id And IPM.Item_Finished_ID=" + ITEM_FINISHED_ID + "";
            DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And ProcessID=" + PROCESS_ID + ")";
                Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                if (Ds1.Tables[0].Rows.Count == 0)
                {
                    Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=-1 And ProcessID=" + PROCESS_ID + ")";
                    Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                }
                if (Ds1.Tables[0].Rows.Count == 0)
                {
                    Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=-1 And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And ProcessID=" + PROCESS_ID + ")";
                    Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                }
                if (Ds1.Tables[0].Rows.Count == 0)
                {
                    Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=-1 And Size_ID=-1 And ProcessID=" + PROCESS_ID + ")";
                    Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                }
                if (Ds1.Tables[0].Rows.Count == 0)
                {
                    Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And ProcessID=" + PROCESS_ID + ")";
                    Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                }
                if (Ds1.Tables[0].Rows.Count == 0)
                {
                    Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=-1 And ProcessID=" + PROCESS_ID + ")";
                    Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                }
                if (Ds1.Tables[0].Rows.Count == 0)
                {
                    Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=-1 And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And ProcessID=" + PROCESS_ID + ")";
                    Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                }
                if (Ds1.Tables[0].Rows.Count == 0)
                {
                    Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=-1 And Size_ID=-1 And ProcessID=" + PROCESS_ID + ")";
                    Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                }
                if (Ds1.Tables[0].Rows.Count > 0)
                {
                    VarRate = Convert.ToDouble(Ds1.Tables[0].Rows[0]["ORATE"]);
                }
            }
        }
        else if (variable.VarFINISHERJOBRATEFOR_OLDFORM == "1")
        {
            SqlParameter[] param = new SqlParameter[14];
            param[0] = new SqlParameter("@JOBTYPEID", PROCESS_ID);
            param[1] = new SqlParameter("@ITEM_FINISHED_ID", ITEM_FINISHED_ID);
            param[2] = new SqlParameter("@CUSTOMERID", 0);
            param[3] = new SqlParameter("@EFFECTIVEDATE", effectivedate);
            param[4] = new SqlParameter("@TODATE", effectivedate);
            param[5] = new SqlParameter("@RATEID", SqlDbType.Int);
            param[5].Direction = ParameterDirection.Output;
            param[6] = new SqlParameter("@RATE", SqlDbType.Float);
            param[6].Direction = ParameterDirection.Output;
            param[7] = new SqlParameter("@orderid", ORDER_ID);
            param[8] = new SqlParameter("@caltype", Caltype);
            param[9] = new SqlParameter("@OrderUnitId", OrderUnitId);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_GETFINISHERRATE_OLDFORM", param);
            VarRate = Convert.ToDouble(param[6].Value);

        }
        else
        {
            string Str1 = "SELECT DISTINCT ORATE From ORDER_CONSUMPTION_DETAIL WHERE ORDERID=" + ORDER_ID + " AND ORDERDETAILID IN (SELECT ORDERDETAILID FROM ORDERDETAIL WHERE ORDERID=" + ORDER_ID + " AND ITEM_FINISHED_ID=" + ITEM_FINISHED_ID + ") AND PROCESSID=" + PROCESS_ID + "";
            DataSet Ds2 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str1);
            if (Ds2.Tables[0].Rows.Count > 0)
            {
                VarRate = Convert.ToDouble(Ds2.Tables[0].Rows[0]["ORATE"]);
            }
            else
            {
                DataSet Ds1;
                string Str = "Select * from Item_ParaMeter_Master IPM,ITem_Master IM Where IPM.Item_Id=IM.Item_Id And IPM.Item_Finished_ID=" + ITEM_FINISHED_ID + "";
                DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And ProcessID=" + PROCESS_ID + ")";
                    Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                    if (Ds1.Tables[0].Rows.Count == 0)
                    {
                        Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=-1 And ProcessID=" + PROCESS_ID + ")";
                        Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                    }
                    if (Ds1.Tables[0].Rows.Count == 0)
                    {
                        Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=-1 And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And ProcessID=" + PROCESS_ID + ")";
                        Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                    }
                    if (Ds1.Tables[0].Rows.Count == 0)
                    {
                        Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=-1 And Size_ID=-1 And ProcessID=" + PROCESS_ID + ")";
                        Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                    }
                    if (Ds1.Tables[0].Rows.Count == 0)
                    {
                        Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And ProcessID=" + PROCESS_ID + ")";
                        Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                    }
                    if (Ds1.Tables[0].Rows.Count == 0)
                    {
                        Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=-1 And ProcessID=" + PROCESS_ID + ")";
                        Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                    }
                    if (Ds1.Tables[0].Rows.Count == 0)
                    {
                        Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=-1 And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And ProcessID=" + PROCESS_ID + ")";
                        Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                    }
                    if (Ds1.Tables[0].Rows.Count == 0)
                    {
                        Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=-1 And Size_ID=-1 And ProcessID=" + PROCESS_ID + ")";
                        Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                    }
                    if (Ds1.Tables[0].Rows.Count > 0)
                    {
                        VarRate = Convert.ToDouble(Ds1.Tables[0].Rows[0]["ORATE"]);
                    }
                }
            }
        }
        return VarRate;
    }

    //public static float PROCESS_RATE(int ITEM_FINISHED_ID, int ORDER_ID, int PROCESS_ID)
    //{
    //    int VarRate = 0;
    //    string Str = "SELECT DISTINCT ORATE From ORDER_CONSUMPTION_DETAIL WHERE ORDERID=" + ORDER_ID + " AND ORDERDETAILID IN (SELECT ORDERDETAILID FROM ORDERDETAIL WHERE ORDERID=" + ORDER_ID + " AND ITEM_FINISHED_ID=" + ITEM_FINISHED_ID + ") AND PROCESSID=" + PROCESS_ID + "";
    //    DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
    //    if (Ds.Tables[0].Rows.Count > 0)
    //    {
    //        VarRate = Convert.ToInt32(Ds.Tables[0].Rows[0]["ORATE"]);
    //    }
    //    return VarRate;
    //}
    public static double PROCESS_RATE(int ITEM_FINISHED_ID, int ORDER_ID, int PROCESS_ID, string effectivedate = "", int TypeId = 1, int orderunitid = 0)
    {
        double VarRate = 0;
        SqlParameter[] param = new SqlParameter[7];
        param[0] = new SqlParameter("@orderid", ORDER_ID);
        param[1] = new SqlParameter("itemfinishedid", ITEM_FINISHED_ID);
        param[2] = new SqlParameter("@processId", PROCESS_ID);
        param[3] = new SqlParameter("@rate", SqlDbType.Float);
        param[3].Direction = ParameterDirection.Output;
        param[4] = new SqlParameter("@effectivedate", effectivedate == "" ? System.DateTime.Now.ToString("dd-MMM-yyyy") : effectivedate);
        param[5] = new SqlParameter("@TypeId", TypeId);
        param[6] = new SqlParameter("@OrderUnitid", orderunitid);

        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_Getprocessrate", param);

        VarRate = Convert.ToDouble(param[3].Value);

        #region
        //string Str1 = "SELECT DISTINCT ORATE From ORDER_CONSUMPTION_DETAIL WHERE ORDERID=" + ORDER_ID + " AND ORDERDETAILID IN (SELECT ORDERDETAILID FROM ORDERDETAIL WHERE ORDERID=" + ORDER_ID + " AND ITEM_FINISHED_ID=" + ITEM_FINISHED_ID + ") AND PROCESSID=" + PROCESS_ID + "";
        //DataSet Ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str1);
        //if (Ds2.Tables[0].Rows.Count > 0)
        //{
        //    VarRate = Convert.ToDouble(Ds2.Tables[0].Rows[0]["ORATE"]);
        //}
        //else
        //{
        //    DataSet Ds1;
        //    string Str = "Select * from Item_ParaMeter_Master IPM,ITem_Master IM Where IPM.Item_Id=IM.Item_Id And IPM.Item_Finished_ID=" + ITEM_FINISHED_ID + "";
        //    DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        //    if (Ds.Tables[0].Rows.Count > 0)
        //    {
        //        Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And ProcessID=" + PROCESS_ID + ")";
        //        Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        //        if (Ds1.Tables[0].Rows.Count == 0)
        //        {
        //            Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=-1 And ProcessID=" + PROCESS_ID + ")";
        //            Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        //        }
        //        if (Ds1.Tables[0].Rows.Count == 0)
        //        {
        //            Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=-1 And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And ProcessID=" + PROCESS_ID + ")";
        //            Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        //        }
        //        if (Ds1.Tables[0].Rows.Count == 0)
        //        {
        //            Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=-1 And Size_ID=-1 And ProcessID=" + PROCESS_ID + ")";
        //            Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        //        }
        //        if (Ds1.Tables[0].Rows.Count == 0)
        //        {
        //            Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And ProcessID=" + PROCESS_ID + ")";
        //            Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        //        }
        //        if (Ds1.Tables[0].Rows.Count == 0)
        //        {
        //            Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=-1 And ProcessID=" + PROCESS_ID + ")";
        //            Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        //        }
        //        if (Ds1.Tables[0].Rows.Count == 0)
        //        {
        //            Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=-1 And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And ProcessID=" + PROCESS_ID + ")";
        //            Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        //        }
        //        if (Ds1.Tables[0].Rows.Count == 0)
        //        {
        //            Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=-1 And Size_ID=-1 And ProcessID=" + PROCESS_ID + ")";
        //            Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        //        }
        //        if (Ds1.Tables[0].Rows.Count > 0)
        //        {
        //            VarRate = Convert.ToDouble(Ds1.Tables[0].Rows[0]["ORATE"]);
        //        }
        //    }
        //}
        #endregion
        return Math.Round(VarRate, 2);
    }
    public static void StockStockTranTableUpdateWithOpeningStock(int Item_Finished_Id, int Godownid, int Companyid, string LotNo, double Qty, string TranDate, string RealDateTime, string TableName, int PRTId, SqlTransaction Tran, int TranType, bool BlnStockAddInQty, int TypeId, int Finish_Type, double price, int Unitid = 0, int Noofcone = 0, string TagNo = "Without Tag No", int userid = 0, string BinNo = "")
    {
        string StrSql;
        int StockId;
        if (LotNo == "")
        {
            LotNo = "Without Lot No";
        }
        try
        {
            StrSql = "Select StockID From Stock Where Item_Finished_id=" + Item_Finished_Id + " And Godownid=" + Godownid + " And Companyid=" + Companyid + "and lotno='" + LotNo + "' and TagNo='" + TagNo + "' And FINISHED_TYPE_ID=" + Finish_Type + " and BinNo='" + BinNo + "'";
            StockId = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, StrSql));

            StrSql = "";
            if (StockId > 0)                        //'Update Stock
            {
                if (BlnStockAddInQty == true)              //''''Qty Addition in Stock
                {
                    StrSql = "Update Stock Set QtyInHand=  QtyInHand + " + Qty + ", OpenStock=" + Convert.ToDouble(Qty) + ",Price=" + price + ",noofcone=noofcone+" + Noofcone + " Where StockId=" + StockId + " and Godownid=" + Godownid + "  and CompanyID=" + Companyid;
                }
                else                                            //'''Qty Deduct from Stock  
                {
                    StrSql = "Update Stock Set QtyInHand=QtyInHand - " + Qty + ",OpenStock=" + Convert.ToDouble(Qty) + ",Price=" + price + ",noofcone=noofcone-" + Noofcone + " Where StockId=" + StockId + " and Godownid=" + Godownid + "  and CompanyID=" + Companyid;
                }
            }
            else
            {                         //''' New Stock Entry
                StockId = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select IsNull(Max(StockId),0)+1 From Stock"));
                if (BlnStockAddInQty == true)                 //''''Qty Addition in Stock
                {
                    StrSql = "Insert Into Stock (StockID,Item_Finished_id,QtyinHand,QtyAssigned,OpenStock,GodownId,Companyid,Price,LotNo,TypeId,Finished_Type_Id,Noofcone,TagNo,BinNo) Values (" + StockId + "," + Item_Finished_Id + "," + Qty + ",0," + Qty + "," + Godownid + "," + Companyid + "," + price + ",'" + LotNo + "'," + TypeId + "," + Finish_Type + "," + Noofcone + ",'" + TagNo + "','" + BinNo + "')";
                }
                else                                     //''''Qty Deduct from Stock
                {
                    Qty = 0 - Qty;
                    StrSql = "Insert Into Stock (StockID,Item_Finished_id,QtyinHand,QtyAssigned,OpenStock,GodownId,Companyid,Price,LotNo,TypeId,Finished_Type_Id,Noofcone,TagNo,BinNo) Values (" + StockId + "," + Item_Finished_Id + "," + Qty + ",0," + Qty + "," + Godownid + "," + Companyid + "," + price + ",'" + LotNo + "'," + TypeId + "," + Finish_Type + "," + Noofcone + ",'" + TagNo + "','" + BinNo + "')";
                }
            }
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, StrSql);
            //''******* Insertion In StockTrans Table
            StrSql = "";
            int TranId = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select IsNull(Max(StockTranId),0)+1 From StockTran"));
            StrSql = "Insert Into StockTran(Stockid,StockTranId,TranType,Quantity,TranDate,Userid,RealDate,TableName,PRTId,Unitid,coneuse) Values (" + StockId + "," + TranId + ",'" + TranType + "'," + Qty + ",'" + TranDate + "'," + userid + ",'" + RealDateTime + "','" + TableName + "'," + PRTId + "," + Unitid + "," + Noofcone + ")";
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, StrSql);
            //}
        }
        catch
        {
        }
        finally
        {

        }
    }
    public static double ConvertMtrToFt(double VarMtrFormat)
    {
        int i;
        string X, Y, Z;
        double a, b, VarWidth;

        VarWidth = VarMtrFormat;
        a = Math.Round(VarWidth / 2.54, 0);
        b = a / 12;
        X = Convert.ToString(b);
        i = Convert.ToInt32(X.Split('.')[0]);
        b = a % 12;
        if (b < 10)
        {
            Y = "0" + Convert.ToString(b);
            Z = Convert.ToString(i) + "." + Y;
        }
        else
        {
            Y = Convert.ToString(b);
            Z = Convert.ToString(i) + "." + Y;
        }
        return Convert.ToDouble(Z);
    }
    public static double ConvertFtToMtr(double VarFtFormat)
    {
        string Str;
        int LengthMtr, LengthCm;
        double VarLength, TotalLengthCm;

        Str = string.Format("{0:#0.00}", VarFtFormat);
        LengthMtr = Convert.ToInt32(Str.Split('.')[0]);
        LengthCm = Convert.ToInt32(Str.Split('.')[1]);
        TotalLengthCm = (Convert.ToDouble(LengthMtr * 12) + Convert.ToDouble(LengthCm)) * 2.54;
        Str = string.Format("{0:#0.00}", Convert.ToDouble(TotalLengthCm));
        VarLength = Convert.ToInt32(Str.Split('.')[0]);
        return VarLength;
    }
    public static double Calculate_Area_Ft(double Length, double Width, int CalType, int ShapeId, int UnitId = 0, int Processid = 0, double RoundFullAreaValue = 1)
    {
        int VarFactor = 0;
        int VarRoundFlag = 0;
        int VarCompanyNo;
        int VarProductionArea;
        int vargirh = 0;
        // DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select VarCompanyNo From MasterSetting
        //Select RoundFtFlag From MasterSetting");
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select VarCompanyNo,RoundFtFlag,ProductionArea,vargirh,Varfactor From MasterSetting");

        // DataRow dr = AllEnums.MasterTables.Mastersetting.ToTable().Select()[0];

        //int VarCompanyNo = Convert.ToInt32(Ds.Tables[0].Rows[0]["VarCompanyNo"]);
        //VarRoundFlag = Convert.ToInt32(Ds.Tables[1].Rows[0]["RoundFtFlag"]);
        VarRoundFlag = Convert.ToInt16(ds.Tables[0].Rows[0]["RoundFtFlag"]);
        VarCompanyNo = Convert.ToInt16(ds.Tables[0].Rows[0]["VarCompanyNo"]);

        VarProductionArea = Convert.ToInt16(ds.Tables[0].Rows[0]["ProductionArea"]);
        vargirh = Convert.ToInt16(ds.Tables[0].Rows[0]["vargirh"]);
        VarFactor = Convert.ToInt16(ds.Tables[0].Rows[0]["Varfactor"]);
        #region
        //switch (VarCompanyNo)
        //{
        //    case 1:
        //        VarFactor = 1;
        //        break;
        //    case 2:
        //        VarFactor = 1;
        //        break;
        //    case 3:
        //        VarFactor = 1;
        //        break;
        //    case 4:
        //        VarFactor = 9;
        //        break;
        //    case 5:
        //        VarFactor = 9;
        //        break;
        //    case 8:
        //        VarFactor = 9;
        //        break;
        //    case 11:
        //        VarFactor = 9;
        //        break;
        //    case 15:
        //        VarFactor = 9;
        //        break;
        //    case 16:
        //        VarFactor = 9;
        //        break;
        //    case 17:
        //        VarFactor = 9;
        //        break;
        //    case 18:
        //        VarFactor = 9;
        //        break;
        //    case 19:
        //        VarFactor = 9;
        //        break;
        //    default:
        //        VarFactor = 1;
        //        break;
        //}
        #endregion
        int FootLength = 0;
        int FootWidth = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        int InchLength = 0;
        int InchWidth = 0;
        double VarArea = 0;
        string Str = "";
        //if (UnitId == 6)
        //{
        //    InchLength = Length;
        //    InchWidth = Width;
        //}
        //else
        //{
            Str = string.Format("{0:#0.00}", Length);
            FootLength = Convert.ToInt32(Str.Split('.')[0]);
            FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
            Str = string.Format("{0:#0.00}", Width);
            FootWidth = Convert.ToInt32(Str.Split('.')[0]);
            FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
            InchLength = (FootLength * 12) + FootLengthInch;
            InchWidth = (FootWidth * 12) + FootWidthInch;
       // }
        int VarTotalInches;
        switch (CalType)
        {
            case 2:
                if (ShapeId == 2)                                    //For Round Shape
                {

                    switch (HttpContext.Current.Session["varcompanyNo"].ToString())
                    {
                        case "16":
                        case "28":
                            VarTotalInches = Convert.ToInt32(Math.Round((Convert.ToDouble(InchWidth) * 4.0 * 3 / 4.0), 0));
                            VarArea = Math.Round(ConvertInchesToFt(VarTotalInches), 4);
                            VarArea = Math.Round(VarArea, 4);
                            break;
                        default:
                            VarTotalInches = Convert.ToInt32(Math.Round((Convert.ToDouble(InchWidth) / 2.0) * 2 * 22 / 7.0, 0));
                            VarArea = Math.Round(ConvertInchesToFt(VarTotalInches), 4);
                            VarArea = Math.Round(VarArea, 4);
                            break;
                    }
                }
                else
                {
                    VarTotalInches = ((2 * InchLength) + (2 * InchWidth));
                    VarArea = Math.Round(ConvertInchesToFt(VarTotalInches), 4);
                }
                break;
            case 3:
                VarTotalInches = (2 * InchWidth);
                VarArea = Math.Round(ConvertInchesToFt(VarTotalInches), 4);
                break;
            case 4:
                VarTotalInches = ((2 * InchLength));
                VarArea = Math.Round(ConvertInchesToFt(VarTotalInches), 4);
                break;
            default:
                if (ShapeId == 2)
                {
                    if (HttpContext.Current.Session["varcompanyNo"].ToString() == "20")
                    {
                        VarArea = Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth)) / (144 * VarFactor) * RoundFullAreaValue, 4);
                    }
                    else
                    {
                        VarArea = Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth)) / (144 * VarFactor), 4);
                    }


                }
                else
                {
                    VarArea = Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth)) / (144 * VarFactor), 4);
                }
                //                int VarProductionArea = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ProductionArea From MasterSetting"));
                //***********For Champo
                switch (HttpContext.Current.Session["varcompanyNo"].ToString())
                {
                    case "16":
                    case "28":
                        if (Processid > 0 && Processid != 1 && ShapeId == 2) //Weaving  , Round
                        {
                            VarArea = Math.Round(VarArea - (VarArea * 21 / 100), 4);
                            //*****
                        }
                        break;
                    default:
                        break;
                }
                //***********
                if (VarProductionArea == 1)
                {
                    if (vargirh == 0)
                    {
                        Str = string.Format("{0:#0.0000#}", VarArea);
                        double IntegerValue = Convert.ToDouble(Str.Split('.')[0]);
                        Str = (Convert.ToDouble(Str.Split('.')[1]) / (0.0625 * 10000)).ToString();
                        Str = string.Format("{0:#0.0000#}", Convert.ToDouble(Str));
                        string Str1 = string.Format("{0:#0.0000#}", Convert.ToDouble(Str.Split('.')[1]));
                        double TotalGirhValue = 0;
                        TotalGirhValue = Convert.ToDouble(Str.Split('.')[0]);
                        switch (HttpContext.Current.Session["varcompanyNo"].ToString())
                        {
                            case "16":
                            case "28":
                            case "20":
                            case "15":
                            case "43":
                                break;
                            //case "15": //EM HD
                            //    if (Convert.ToDouble(Str1) > 9999)
                            //    {
                            //        TotalGirhValue = Convert.ToDouble(Str.Split('.')[0]) + 1;
                            //    }
                            //    break;
                            default:
                                if (Convert.ToDouble(Str1) > 5999)
                                {
                                    TotalGirhValue = Convert.ToDouble(Str.Split('.')[0]) + 1;
                                }
                                break;
                        }

                        double DecimalValue = 0;
                        if (TotalGirhValue > 0)
                        {
                            DecimalValue = TotalGirhValue * 0.0625;
                        }
                        VarArea = IntegerValue + DecimalValue;
                    }
                }
                if (UnitId == 6) //for Inch
                {
                    VarArea = VarArea / 144;
                }
                if (Convert.ToInt32(HttpContext.Current.Session["varcompanyNo"]) == 9)
                {
                    if (UnitId == 6) //for Inch
                    {
                        VarArea = Math.Round((Length * Width / 144.0), 4);
                    }
                }
                break;
        }
        if (variable.VarAreaFtRound == "0")////Without Round
        {
            // return Math.Round(VarArea, VarRoundFlag);
            VarArea = DecimalvalueUptoWithoutRounding(VarArea, variable.VarRoundFtFlag);            
        }
        else
        {
            VarArea = Math.Round(VarArea, VarRoundFlag);            
        }
        return VarArea;
        
    }
    public static double ConvertInchesToFt(int VarInches)
    {
        int i;
        string X, Y, Z;
        double a, b;
        a = Convert.ToDouble(VarInches);
        b = a / 12;

        switch (HttpContext.Current.Session["varcompanyNo"].ToString())
        {
            case "16":
            case "28":
                Z = b.ToString();
                break;
            default:
                X = Convert.ToString(b);
                i = Convert.ToInt32(X.Split('.')[0]);
                b = a % 12;
                b = Convert.ToInt32(b);

                if (b < 10)
                {
                    Y = "0" + Convert.ToString(b);
                    Z = Convert.ToString(i) + "." + Y;
                }
                else
                {
                    Y = Convert.ToString(b);
                    Z = Convert.ToString(i) + "." + Y;
                }
                break;
        }
        return Convert.ToDouble(Z);
    }
    public static double Calculate_Area_Mtr(double Length, double Width, int CalType, int ShapeId)
    {
        double VarArea = 0;
        int VarRoundFlag = 0;
        double VarFactor = 0;
        int VarCompanyNo;
        string shapeName = "";
        string str = @"Select VarCompanyNo,RoundFtFlag,Roundmtrflag,ProductionArea,vargirh,Varmfactor From MasterSetting
                     select upper(ShapeName) as ShapeName From Shape Where ShapeId=" + ShapeId;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        // DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo,RoundMtrFlag From MasterSetting");                
        //DataRow dr = AllEnums.MasterTables.Mastersetting.ToTable().Select()[0];
        // VarRoundFlag = Convert.ToInt32(Ds.Tables[0].Rows[0]["RoundMtrFlag"]);
        //VarCompanyNo = Convert.ToInt32(Ds.Tables[0].Rows[0]["VarCompanyNo"]);
        VarRoundFlag = Convert.ToInt16(ds.Tables[0].Rows[0]["Roundmtrflag"]);
        VarCompanyNo = Convert.ToInt16(HttpContext.Current.Session["varcompanyNo"]);
        VarFactor = Convert.ToDouble(ds.Tables[0].Rows[0]["Varmfactor"]);
        if (ds.Tables[1].Rows.Count > 0)
        {
            shapeName = ds.Tables[1].Rows[0]["shapename"].ToString().Trim();
        }
        switch (CalType)
        {
            case 2:
                if (shapeName == "ROUND")                                    //For Round Shape
                {
                    VarArea = Math.Round((3.14 * Convert.ToDouble(Length)) / 100, 4);
                }
                else
                {
                    VarArea = Math.Round((2 * Convert.ToDouble(Length) + 2 * Convert.ToDouble(Width)) / 100, 4);
                }
                break;
            case 3:
                VarArea = Math.Round((2 * Convert.ToDouble(Width)) / 100, 4);
                break;
            case 4:
                VarArea = Math.Round((2 * Convert.ToDouble(Length)) / 100, 4);
                break;
            default:
                if (shapeName == "ROUND")
                {
                    VarArea = Math.Round(((Convert.ToDouble(Length) * Convert.ToDouble(Width)) / 10000) * VarFactor, 5);

                }
                else
                {
                    VarArea = Math.Round((Convert.ToDouble(Length) * Convert.ToDouble(Width)) / 10000, 5);
                }

                break;
        }
        if (variable.VarAreaMtrRound == "0") // No Rounding
        {
            //string Str = string.Format("{0:#0.00000}", Convert.ToDouble(VarArea));
            //int VarNo = Convert.ToInt32(Str.Split('.')[0]);
            //int VarDecimalNo = Convert.ToInt32(Str.Split('.')[1]);
            //VarDecimalNo = VarDecimalNo / 1000;
            //double VarDecimalNo1 = VarDecimalNo * 0.01;
            //VarArea = Convert.ToDouble(VarNo + VarDecimalNo1);
            VarArea = DecimalvalueUptoWithoutRounding(VarArea, VarRoundFlag);
        }
        else
        {
            VarArea = Math.Round(VarArea, VarRoundFlag);

        }
        return VarArea;
    }
    public static void ITEM_CONSUMPTION_DEFINE(int ProcessId, int Finishedid, int ID)
    {
        SqlParameter[] _arrPara = new SqlParameter[6];
        DataSet Ds1;
        DataSet Ds2;
        string Str = "Select * from Item_ParaMeter_Master IPM,ITem_Master IM Where IPM.Item_Id=IM.Item_Id And IPM.Item_Finished_ID=" + Finishedid + "";
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            Ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from Process_Name_Master Where PROCESS_NAME_ID=" + ProcessId + @" Order By Process_Name_ID");
            if (Ds2.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < Ds2.Tables[0].Rows.Count; i++)
                {
                    Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And ProcessID=" + Ds2.Tables[0].Rows[i]["Process_Name_ID"] + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + "";
                    Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                    if (Ds1.Tables[0].Rows.Count == 0)
                    {
                        Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And ProcessID=" + Ds2.Tables[0].Rows[i]["Process_Name_ID"] + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=-1";
                        Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                    }
                    if (Ds1.Tables[0].Rows.Count == 0)
                    {
                        Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And ProcessID=" + Ds2.Tables[0].Rows[i]["Process_Name_ID"] + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=-1 And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + "";
                        Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                    }
                    if (Ds1.Tables[0].Rows.Count == 0)
                    {
                        Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And ProcessID=" + Ds2.Tables[0].Rows[i]["Process_Name_ID"] + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=-1 And Size_ID=-1";
                        Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                    }
                    if (Ds1.Tables[0].Rows.Count == 0)
                    {
                        Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And ProcessID=" + Ds2.Tables[0].Rows[i]["Process_Name_ID"] + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + "";
                        Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                    }
                    if (Ds1.Tables[0].Rows.Count == 0)
                    {
                        Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And ProcessID=" + Ds2.Tables[0].Rows[i]["Process_Name_ID"] + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=-1";
                        Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                    }
                    if (Ds1.Tables[0].Rows.Count == 0)
                    {
                        Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And ProcessID=" + Ds2.Tables[0].Rows[i]["Process_Name_ID"] + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=-1 And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + "";
                        Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                    }
                    if (Ds1.Tables[0].Rows.Count == 0)
                    {
                        Str = "Select IPCM.*,PM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And ProcessID=" + Ds2.Tables[0].Rows[i]["Process_Name_ID"] + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=-1 And Size_ID=-1";
                        Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                    }
                    if (Ds1.Tables[0].Rows.Count > 0)
                    {
                        _arrPara[0] = new SqlParameter("@FINISHEDID", SqlDbType.Int);
                        _arrPara[1] = new SqlParameter("@ID", SqlDbType.Int);
                        _arrPara[2] = new SqlParameter("@PROCESSID", SqlDbType.Int);

                        _arrPara[0].Value = Ds1.Tables[0].Rows[0]["ITEM_FINISHED_ID"];
                        _arrPara[1].Value = ID;
                        _arrPara[2].Value = Ds2.Tables[0].Rows[i]["Process_Name_ID"];
                        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_ITEM_CONSUMPTION_DEFINE", _arrPara);
                    }
                }
            }
        }
    }
    public static double Fill_Comm(int VarFinishedid)
    {
        float VarComm = 0;
        DataSet Ds1;
        string Str = "Select * from Item_ParaMeter_Master IPM,ITem_Master IM Where IPM.Item_Id=IM.Item_Id And IPM.Item_Finished_ID=" + VarFinishedid + "";
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                Str = "Select * From Commission Where CategoryID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And ItemID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And QualityID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And DesignID=" + Ds.Tables[0].Rows[0]["Design_ID"];
                Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                if (Ds1.Tables[0].Rows.Count == 0)
                {
                    Str = "Select * From Commission Where CategoryID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And ItemID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And QualityID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And DesignID=-1";
                    Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                }
                if (Ds1.Tables[0].Rows.Count > 0)
                {
                    VarComm = Convert.ToInt32(Ds1.Tables[0].Rows[0]["Commission"]);
                }
            }
        }
        return VarComm;
    }
    public static void MessageAlert(string msg, string formname)
    {
        SqlParameter[] _arrPara = new SqlParameter[6];
        _arrPara[0] = new SqlParameter("@msg", SqlDbType.NVarChar, (250));
        _arrPara[1] = new SqlParameter("@formname", SqlDbType.NVarChar, 50);
        _arrPara[0].Value = msg;
        _arrPara[1].Value = formname;
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_MessageAlert", _arrPara);
        //DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from MessageAlert");
        //if (dt.Tables[0].Rows.Count > 0)
        //{
        // MailMessage mail = new MailMessage();
        // mail.From = new MailAddress("contact@enable.co.in");
        // mail.To.Add("mail@enableit.co.in");  
        // //to send mail on textbox value use this code.  
        // //mail.To.Add(TextBox2.Text);  
        // mail.IsBodyHtml = true;  
        //mail.Subject = "Form";  
        // mail.Body = "<br><br>Error On Form "+dt.Tables[0].Rows[0][0].ToString()+"" + "<b>Error Message was:</b>" + dt.Tables[0].Rows[i]["message"].ToString() + "" ; 
        //SmtpClient smtp = new SmtpClient("smtp.gmail.com",25);
        //smtp.Credentials = new System.Net.NetworkCredential("contact@enable.co.in", "eit123"); smtp.EnableSsl = true;  
        // smtp.Send(mail); 

        //  SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete from MessageAlert where id=" + dt.Tables[0].Rows[0]["id"].ToString() + "");
        //}
    }
    public static void NewChkBoxListFill(ref CheckBoxList CheckBoxName, string strsql)
    {
        try
        {
            CheckBoxName.Items.Clear();
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                CheckBoxName.DataSource = ds;
                CheckBoxName.DataTextField = ds.Tables[0].Columns[1].ColumnName;
                CheckBoxName.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                CheckBoxName.DataBind();
            }
        }
        catch (Exception ex)
        {

            Logs.WriteErrorLog("UtilityModule|NewChkBoxListFill|" + ex.Message);
        }
    }
    public static void NewChkBoxListFillWithDs(ref CheckBoxList CheckBoxName, DataSet ds, int i)
    {
        try
        {
            CheckBoxName.Items.Clear();

            if (ds.Tables[i].Rows.Count > 0)
            {
                CheckBoxName.DataSource = ds.Tables[i];
                CheckBoxName.DataTextField = ds.Tables[i].Columns[1].ColumnName;
                CheckBoxName.DataValueField = ds.Tables[i].Columns[0].ColumnName;
                CheckBoxName.DataBind();
            }
        }
        catch (Exception ex)
        {

            Logs.WriteErrorLog("UtilityModule|NewChkBoxListFill|" + ex.Message);
        }
    }
    public static void Insert_Into_Carpet_NumberAndProcess_StockDetailwithProc(int VarItem_Finished_id, int VarOrderid, int VarRecQty, string VarPreFix, int VarPostFix, int VarCompanyid, int VarProcess_Rec_Id, int VarProcess_Rec_Detail_Id, String VarRecDate, SqlTransaction Tran, int VarProcessId, int IssueDetailId, int VaruserId)
    {

        SqlParameter[] parparam = new SqlParameter[12];
        parparam[0] = new SqlParameter("@FinishedID", VarItem_Finished_id);
        parparam[1] = new SqlParameter("@OrderID", VarOrderid);
        parparam[2] = new SqlParameter("@RecQty", VarRecQty);
        parparam[3] = new SqlParameter("@PreFix", VarPreFix);
        parparam[4] = new SqlParameter("@CompanyID", VarCompanyid);
        parparam[5] = new SqlParameter("@Process_Rec_ID", VarProcess_Rec_Id);
        parparam[6] = new SqlParameter("@Process_Rec_DetailID", VarProcess_Rec_Detail_Id);
        parparam[7] = new SqlParameter("@RecDate", VarRecDate);
        parparam[8] = new SqlParameter("@ProcessID", VarProcessId);
        parparam[9] = new SqlParameter("@IssueDetailId", IssueDetailId);
        parparam[10] = new SqlParameter("@VarUserID", VaruserId);
        parparam[11] = new SqlParameter("@PostFix", VarPostFix);

        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Proc_InsertIntoCarpetNumber", parparam);

    }
    public static Double getshaderate(int qualityid, int shadecolorid)
    {
        Double rate = 0;
        string str = @"select Top 1 isnull(qr.rate,0) as Rate from ITEM_PARAMETER_MASTER im inner join definequalityrate qr
                    on im.QUALITY_ID=qr.quality_id and im.SHADECOLOR_ID=qr.shadedcolor_id  where im.quality_id=" + qualityid + " and im.shadecolor_id=" + shadecolorid + @"                                                                                                                
                    order by qr.dateadded desc ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            rate = Convert.ToDouble(ds.Tables[0].Rows[0]["rate"]);
        }

        return rate;
    }
    public static Double getItemRate(int vendorid, int finishedid, string ProcessName)
    {
        Double rate = 0;
        string str = "select top(1) Rate  from vendorwiseitemrate V inner join Process_Name_master PNM on V.Processid=PNM.Process_Name_id where vendorid= " + vendorid + " and Item_Finished_id=" + finishedid + " and PNM.Process_Name='" + ProcessName + "' order by id desc";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            rate = Convert.ToDouble(ds.Tables[0].Rows[0]["Rate"]);
        }
        return rate;
    }
    public static string GetFinancialYear(DateTime date, string separator = "/", Boolean fullyear = false)
    {
        int month = 0;
        int day = 0;
        int year;
        System.DateTime date1 = date;
        string Financialyear;
        month = date1.Month;
        day = date1.Day;
        year = date1.Year;

        //'Get Financial Year
        if (month <= 3 & day <= 31)
        {
            if (month >= 1)
            {
                if (fullyear == true)
                {
                    Financialyear = (year - 1).ToString() + separator + Right((year).ToString(), 2);
                }
                else
                {
                    Financialyear = Right((year - 1).ToString(), 2) + separator + Right((year).ToString(), 2);
                }
            }
            else
            {
                if (fullyear == true)
                {
                    Financialyear = (year).ToString() + separator + Right((year + 1).ToString(), 2);
                }
                else
                {
                    Financialyear = Right((year).ToString(), 2) + separator + Right((year + 1).ToString(), 2);
                }
            }
        }
        else
        {
            if (fullyear == true)
            {
                Financialyear = (year).ToString() + separator + Right((year + 1).ToString(), 2);
            }
            else
            {
                Financialyear = Right((year).ToString(), 2) + separator + Right((year + 1).ToString(), 2);
            }
        }

        return Financialyear;


    }
    public static string Right(string param, int length)
    {
        //start at the index based on the lenght of the sting minus
        //the specified length and assign it a variable
        string result = param.Substring(param.Length - length, length);
        //return the result of the operation
        return result;

    }
    public static int PPNoWise(string processid)
    {
        int PPNoWise = 0;
        string str = "select PPnowise from Process_Name_master where  Process_name_id=" + processid;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        PPNoWise = Convert.ToInt16(ds.Tables[0].Rows[0]["PPnowise"]);
        return PPNoWise;
    }
    public static Double getstockQty(string companyid, string Godownid, string Lotno, int Item_finished_id, string TagNo = "Without Tag No", string BinNo = "")
    {
        Double stockqTY = 0.0;
        string str = "select Round(isnull(sum(Qtyinhand),0),3) as Qtyinhand from Stock Where companyId= " + companyid + " and godownId=" + Godownid + " and Lotno='" + Lotno + "' and Item_finished_id=" + Item_finished_id + " and TagNo='" + TagNo + "' and BinNo='" + BinNo + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            stockqTY = Convert.ToDouble(ds.Tables[0].Rows[0]["Qtyinhand"]);
        }
        return stockqTY;
    }
    public static DataTable getarticledescription(string articleno)
    {
        string str = @"select Distinct vf.ITEM_ID,vf.QualityId,vf.designId,vf.ColorId,vf.ShapeId,vf.SizeId,vf.QualityName+' '+vf.designName as Quality,Vf.ColorName,vf.SizeMtr as Size,PT.PackingType,PA.PackingTypeid From Packingarticle PA inner join V_FinishedItemDetail vf on
                    PA.Itemid=vf.ITEM_ID and PA.QualityId=vf.QualityId and Pa.Designid=vf.designid
                    and PA.Colorid=vf.ColorId and pa.shapeid=vf.ShapeId and pa.sizeid=vf.SizeId
                    inner join PackingType PT on PA.PackingTypeid=PT.ID where PA.ArticleNo='" + articleno + "'";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        return ds.Tables[0];
    }
    private static readonly Regex InvalidFileRegex = new Regex(string.Format("[{0}]", Regex.Escape(@"<>:""/\|?*")));
    public static string validateFilename(string filename)
    {
        return InvalidFileRegex.Replace(filename, string.Empty);
    }
    public static double PROCESS_CONSUMPTION(int ITEM_FINISHED_ID, int Unitid, string effectivedate = "", int TypeId = 1)
    {
        double VarConsump = 0;
        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("itemfinishedid", ITEM_FINISHED_ID);
        param[1] = new SqlParameter("@Consump", SqlDbType.Float);
        param[1].Direction = ParameterDirection.Output;
        param[2] = new SqlParameter("@effectivedate", effectivedate == "" ? System.DateTime.Now.ToString("dd-MMM-yyyy") : effectivedate);
        param[3] = new SqlParameter("@TypeId", TypeId);
        param[4] = new SqlParameter("@UnitId", Unitid);

        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GET_CONSUMPTION_QUALITYDETAILWISE", param);

        VarConsump = Convert.ToDouble(param[1].Value);

        return Math.Round(VarConsump, 3);
        //return Math.Round(VarConsump, 2);
    }
    public static string Calculate_Process_Receive_Area(double Width, double Length, string AfterKhapSize, int CalType, double hnWidht, double hnLength, string hnAfterKhapSize)
    {
        string VarAfterKhapSize = "";
        int ActWF, ActWI, ActLF, ActLI, CSWF, CSWI, CSLF, CSLI, BZWF, BZWI, BZLF, BZLI;

        double KhapWidth, KhapLength, Width1, Length1, ActSizeW, ActSizeL, BZW = 0, BZL = 0;
        string SWF, SWI, SLF, SLI;
        int AWIDTH, ALENGTH, CWIDTH, CLENGTH, BW, BL;
        string HStrW = "";
        string HStrL = "";

        string StrW = "";
        string StrL = "";

        string HKStrWL = "";
        string HKStrW = "";
        string HKStrL = "";

        string KStrW = "";
        string KStrL = "";

        ActSizeW = hnWidht;
        ActSizeL = hnLength;

        if (CalType == 2)
        {
            HStrW = string.Format("{0:#0.00}", hnWidht);
            HStrL = string.Format("{0:#0.00}", hnLength);
            ActWF = Convert.ToInt32(HStrW.Split('.')[0]);
            ActWI = Convert.ToInt32(HStrW.Split('.')[1]);
            ActLF = Convert.ToInt32(HStrL.Split('.')[0]);
            ActLI = Convert.ToInt32(HStrL.Split('.')[1]);

            //Change size
            StrW = string.Format("{0:#0.00}", Width);
            StrL = string.Format("{0:#0.00}", Length);
            CSWF = Convert.ToInt32(StrW.Split('.')[0]);
            CSWI = Convert.ToInt32(StrW.Split('.')[1]);
            CSLF = Convert.ToInt32(StrL.Split('.')[0]);
            CSLI = Convert.ToInt32(StrL.Split('.')[1]);

            HKStrWL = string.Format("{0:#0.00}", hnAfterKhapSize);
            BZW = Convert.ToDouble(HKStrWL.Split('x')[0]);
            BZL = Convert.ToDouble(HKStrWL.Split('x')[1]);

            HKStrW = string.Format("{0:#0.00}", BZW);
            HKStrL = string.Format("{0:#0.00}", BZL);

            BZWF = Convert.ToInt32(HKStrW.Split('.')[0]);
            BZWI = Convert.ToInt32(HKStrW.Split('.')[1]);
            BZLF = Convert.ToInt32(HKStrL.Split('.')[0]);
            BZLI = Convert.ToInt32(HKStrL.Split('.')[1]);

            AWIDTH = (Convert.ToInt32(ActWF) * 12 + ActWI);
            ALENGTH = (Convert.ToInt32(ActLF) * 12 + ActLI);
            CWIDTH = (Convert.ToInt32(CSWF) * 12 + CSWI);
            CLENGTH = (Convert.ToInt32(CSLF) * 12 + CSLI);
            BW = (Convert.ToInt32(BZWF) * 12 + BZWI);
            BL = (Convert.ToInt32(BZLF) * 12 + BZLI);

            Width1 = AWIDTH - CWIDTH;
            Length1 = ALENGTH - CLENGTH;

            if (Width1 < AWIDTH)
            {
                if (Width1 > 0)
                {
                    Width1 = Width1 * 2;
                    BW = Convert.ToInt32(BW - Width1);
                    if (BW < 0)
                    {
                        string Val = "0";
                        BW = Convert.ToInt32(Val);
                    }
                }
            }


            if (Length1 < ALENGTH)
            {
                if (Length1 > 0)
                {
                    Length1 = Length1 * 2;
                    BL = Convert.ToInt32(BL - Length1);
                    if (BL < 0)
                    {
                        string Val = "0";
                        BL = Convert.ToInt32(Val);
                    }
                }
            }
            Width1 = UtilityModule.ConvertInchesToFt(BW);
            Length1 = UtilityModule.ConvertInchesToFt(BL);
            string Khap_Format = string.Format("{0:#0.00}", Width1) + "x" + string.Format("{0:#0.00}", Length1);
            VarAfterKhapSize = Khap_Format;
        }
        else
        {
            Width1 = ActSizeW - Width;
            Length1 = ActSizeL - Length;
            if (Width1 < ActSizeW)
            {
                if (Width1 > 0)
                {
                    Width1 = Width1 * 2;
                    BZW = Convert.ToInt32(BZW - Width1);
                    if (BZW < 0)
                    {
                        BZW = 0;
                    }
                }

            }
            if (Length1 < ActSizeL)
            {
                if (Length1 > 0)
                {
                    Length1 = Length1 * 2;
                    BZL = Convert.ToInt32(BZL - Length1);
                    if (BZL < 0)
                    {
                        BZL = 0;
                    }
                }

            }
            Width1 = BZW;
            Length1 = BZL;
            string Khap_Format = string.Format("{0:#0.00}", Width1) + "x" + string.Format("{0:#0.00}", Length1);
            VarAfterKhapSize = Khap_Format;
            //VarAfterKhapSize = Convert.ToString(Width1 + 'x' + Length1);
        }
        //switch (CalType)
        //{
        //    case 2:


        //    default:

        //        break;
        //}
        //if (VarCompanyNo == 5)
        //{
        //    string Str = string.Format("{0:#0.00000}", Convert.ToDouble(VarArea));
        //    int VarNo = Convert.ToInt32(Str.Split('.')[0]);
        //    int VarDecimalNo = Convert.ToInt32(Str.Split('.')[1]);
        //    VarDecimalNo = VarDecimalNo / 1000;
        //    double VarDecimalNo1 = VarDecimalNo * 0.01;
        //    VarArea = Convert.ToDouble(VarNo + VarDecimalNo1);
        //}

        return VarAfterKhapSize;
    }
    public static double BAZAAR_CONSUMPTION_FOR_ACTUAL_WEIGHT(int ITEM_FINISHED_ID, int Unitid, int TypeId, string effectivedate = "")
    {
        double VarConsump = 0;
        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("itemfinishedid", ITEM_FINISHED_ID);
        param[1] = new SqlParameter("@Consump", SqlDbType.Float);
        param[1].Direction = ParameterDirection.Output;
        param[2] = new SqlParameter("@effectivedate", effectivedate == "" ? System.DateTime.Now.ToString("dd-MMM-yyyy") : effectivedate);
        param[3] = new SqlParameter("@TypeId", TypeId);
        param[4] = new SqlParameter("@UnitId", Unitid);

        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_Current_Lagat", param);

        VarConsump = Convert.ToDouble(param[1].Value);

        return Math.Round(VarConsump, 3);
        //return Math.Round(VarConsump, 2);
    }
    public static Boolean Temp_ReportCheck(int Processid, int issueorderid = 0, string ReportType = "", int userid = 0)
    {
        string str = @"select issueorderid From Temp_ReportCheck Where Processid=" + Processid + " and issueorderid=" + issueorderid + " and ReportType='" + ReportType + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count == 0)
        {
            str = @"insert into Temp_ReportCheck(processid,issueorderid,ReportType,userid)values(" + Processid + "," + issueorderid + ",'" + ReportType + "'," + userid + ")";
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            return true;
        }
        else
        {
            return false;
        }

    }
    public static double DecimalvalueUptoWithoutRounding(double num, int Decimalpoint)
    {
        double y = Math.Pow(10, Decimalpoint);
        return Math.Truncate(num * y) / y;
    }
    public static string GetExcelCellColumnName(int row)
    {
        string column = "";
        switch (row)
        {
            case 1:
                column = "A";
                break;
            case 2:
                column = "B";
                break;
            case 3:
                column = "C";
                break;
            case 4:
                column = "D";
                break;
            case 5:
                column = "E";
                break;
            case 6:
                column = "F";
                break;
            case 7:
                column = "G";
                break;
            case 8:
                column = "H";
                break;
            case 9:
                column = "I";
                break;
            case 10:
                column = "J";
                break;
            case 11:
                column = "K";
                break;
            case 12:
                column = "L";
                break;
            case 13:
                column = "M";
                break;
            case 14:
                column = "N";
                break;
            case 15:
                column = "O";
                break;
            case 16:
                column = "P";
                break;
            case 17:
                column = "Q";
                break;
            case 18:
                column = "R";
                break;
            case 19:
                column = "S";
                break;
            case 20:
                column = "T";
                break;
            case 21:
                column = "U";
                break;
            case 22:
                column = "V";
                break;
            case 23:
                column = "W";
                break;
            case 24:
                column = "X";
                break;
            case 25:
                column = "Y";
                break;
            case 26:
                column = "Z";
                break;
            case 27:
                column = "AA";
                break;
            case 28:
                column = "AB";
                break;
            case 29:
                column = "AC";
                break;
            case 30:
                column = "AD";
                break;
            case 31:
                column = "AE";
                break;
            case 32:
                column = "AF";
                break;
            case 33:
                column = "AG";
                break;
        }
        return column;
    }

    public static double DraftOrderCalculate_Area(double Length, double Width, int CalType, string ShapeName, int MasterCompanyId, int VarFactor)
    {
        double VarArea = 0;
        if (CalType == 0)
        {

            int FootLength = 0;
            int FootWidth = 0;
            int FootHeight = 0;
            int FootLengthInch = 0;
            int FootWidthInch = 0;
            int FootHeightInch = 0;
            int InchLength = 0;
            int InchWidth = 0;
            int InchHeight = 0;

            double VarVolume = 0;
            string Str = "";


            Str = string.Format("{0:#0.00}", Convert.ToDouble(Convert.ToString(Length) == "" ? "0" : Convert.ToString(Length)));
            switch (MasterCompanyId)
            {
                case 6:
                case 12:
                    InchLength = Convert.ToInt32(Convert.ToDouble(Length) * 12);
                    InchWidth = Convert.ToInt32(Convert.ToDouble(Width) * 12);
                    break;
                default:
                    if (Convert.ToString(Length) != "")
                    {
                        if (Convert.ToString(Length) != "")
                        {
                            FootLength = Convert.ToInt32(Str.Split('.')[0]);
                            FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                        }
                        if (FootLengthInch > 11)
                        {
                            return 0;
                            //lblMessage.Text = "Inch value must be less than 12";
                            //VarLengthNew.Text = "";
                            //VarLengthNew.Focus();
                        }
                    }
                    if (Convert.ToString(Width) != "")
                    {
                        Str = string.Format("{0:#0.00}", Convert.ToDouble(Width));
                        if (Convert.ToString(Width) != "")
                        {
                            FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                            FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                        }
                        if (FootWidthInch > 11)
                        {
                            return 0;
                            //lblMessage.Text = "Inch value must be less than 12";
                            //VarWidthNew.Text = "";
                            //VarWidthNew.Focus();
                        }
                    }
                    InchLength = (FootLength * 12) + FootLengthInch;
                    InchWidth = (FootWidth * 12) + FootWidthInch;
                    break;
            }
            VarArea = Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth)) / 144 * VarFactor, 4);


        }
        else if (CalType == 1)
        {
            Double Area = 0;
            if (ShapeName.Trim() == "ROUND")
            {
                Area = ((Length * Width) / 10000) * Convert.ToDouble(variable.VarMfactor);
                if (variable.VarAreaMtrRound == "0") // No Rounding            {
                {
                    VarArea = Convert.ToDouble(UtilityModule.DecimalvalueUptoWithoutRounding(Area, variable.VarRoundMtrFlag).ToString());
                }
                else
                {
                    VarArea = Convert.ToDouble(Math.Round(Area, variable.VarRoundMtrFlag).ToString());

                }
            }
            else
            {
                Area = (Length * Width) / 10000;
                VarArea = Convert.ToDouble(Math.Round(Area, variable.VarRoundMtrFlag).ToString());
            }
            if (MasterCompanyId == 9)
            {
                VarArea = Convert.ToDouble(Math.Round(Area, 4).ToString());
            }
        }
        else if (CalType == 2)
        {
            Double Area = (Length * Width);
            VarArea = (Area);
        }
        else
        {
            int FootLength = 0;
            int FootWidth = 0;
            int FootHeight = 0;
            int FootLengthInch = 0;
            int FootWidthInch = 0;
            int FootHeightInch = 0;
            int InchLength = 0;
            int InchWidth = 0;
            int InchHeight = 0;

            double VarVolume = 0;
            string Str = "";


            Str = string.Format("{0:#0.00}", Convert.ToDouble(Convert.ToString(Length) == "" ? "0" : Convert.ToString(Length)));
            switch (MasterCompanyId)
            {
                case 6:
                case 12:
                    InchLength = Convert.ToInt32(Convert.ToDouble(Length) * 12);
                    InchWidth = Convert.ToInt32(Convert.ToDouble(Width) * 12);
                    break;
                default:
                    if (Convert.ToString(Length) != "")
                    {
                        if (Convert.ToString(Length) != "")
                        {
                            FootLength = Convert.ToInt32(Str.Split('.')[0]);
                            FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                        }
                        if (FootLengthInch > 11)
                        {
                            return 0;
                            //lblMessage.Text = "Inch value must be less than 12";
                            //VarLengthNew.Text = "";
                            //VarLengthNew.Focus();
                        }
                    }
                    if (Convert.ToString(Width) != "")
                    {
                        Str = string.Format("{0:#0.00}", Convert.ToDouble(Width));
                        if (Convert.ToString(Width) != "")
                        {
                            FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                            FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                        }
                        if (FootWidthInch > 11)
                        {
                            return 0;
                            //lblMessage.Text = "Inch value must be less than 12";
                            //VarWidthNew.Text = "";
                            //VarWidthNew.Focus();
                        }
                    }
                    InchLength = (FootLength * 12) + FootLengthInch;
                    InchWidth = (FootWidth * 12) + FootWidthInch;
                    break;
            }
            VarArea = Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth)) / 144 * VarFactor, 4);

        }

        return VarArea;
    }
    public static int getItemFinishedIdForDyer(int ddItemName, int ddQuality, int ddDesign, int ddColor, int ddShape, int ddSize, string ProdCode, int ddShadeColor, int ProdType, string VarOurCode, int MasterCompanyId)
    {
        int itemfinishedid = 0;
        try
        {
            string VarProdCode = ProdCode;
            if (ProdType == 0)
            {
                VarProdCode = "";
            }
            SqlParameter[] _arrPara = new SqlParameter[12];
            _arrPara[0] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@QUALITY_ID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@DESIGN_ID", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@COLOR_ID", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@SHAPE_ID", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@SIZE_ID", SqlDbType.Int);
            _arrPara[6] = new SqlParameter("@DESCRIPTION", SqlDbType.VarChar, 100);
            _arrPara[7] = new SqlParameter("@ITEM_ID", SqlDbType.Int);
            _arrPara[8] = new SqlParameter("@ProCode", SqlDbType.NVarChar);
            _arrPara[9] = new SqlParameter("@SHADECOLOR_ID", SqlDbType.Int);
            _arrPara[10] = new SqlParameter("@VarOurCode", SqlDbType.VarChar, 100);
            _arrPara[11] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);

            _arrPara[0].Direction = ParameterDirection.Output;
            ///ddShadeColor
            _arrPara[1].Value = ddQuality;
            _arrPara[2].Value = ddDesign;
            _arrPara[3].Value = ddColor;
            _arrPara[4].Value = ddShape;
            _arrPara[5].Value = ddSize;
            _arrPara[6].Value = "";
            _arrPara[7].Value = ddItemName;
            _arrPara[8].Value = VarProdCode;
            _arrPara[9].Value = ddShadeColor;
            _arrPara[10].Value = VarOurCode;
            _arrPara[11].Value = MasterCompanyId;

            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GET_ITEM_FINISHED_ID", _arrPara);

            itemfinishedid = Convert.ToInt32(_arrPara[0].Value);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("getItemParametreId|" + ex.Message);
        }
        finally
        {

        }
        return itemfinishedid;
    }
    public static int getItemFinishedIdForDyer(int ddItemName, int ddQuality, int ddDesign, int ddColor, int ddShape, int ddSize, string ProdCode, SqlTransaction Tran, int ddShadeColor, string VarOurCode, int MasterCompanyId)
    {
        int itemfinishedid = 0;
        try
        {
            string VarProdCode = ProdCode;
            VarProdCode = "";
            SqlParameter[] _arrPara = new SqlParameter[12];
            _arrPara[0] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@QUALITY_ID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@DESIGN_ID", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@COLOR_ID", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@SHAPE_ID", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@SIZE_ID", SqlDbType.Int);
            _arrPara[6] = new SqlParameter("@DESCRIPTION", SqlDbType.VarChar, 100);
            _arrPara[7] = new SqlParameter("@ITEM_ID", SqlDbType.Int);
            _arrPara[8] = new SqlParameter("@ProCode", SqlDbType.NVarChar);
            _arrPara[9] = new SqlParameter("@SHADECOLOR_ID", SqlDbType.Int);
            _arrPara[10] = new SqlParameter("@VarOurCode", SqlDbType.VarChar, 100);
            _arrPara[11] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);


            _arrPara[0].Direction = ParameterDirection.Output;
            ///ddShadeColor
            _arrPara[1].Value = ddQuality;
            _arrPara[2].Value = ddDesign;
            _arrPara[3].Value = ddColor;
            _arrPara[4].Value = ddShape;
            _arrPara[5].Value = ddSize;
            _arrPara[6].Value = "";
            _arrPara[7].Value = ddItemName;
            _arrPara[8].Value = VarProdCode;
            _arrPara[9].Value = ddShadeColor;
            _arrPara[10].Value = VarOurCode;
            _arrPara[11].Value = MasterCompanyId;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_GET_ITEM_FINISHED_ID", _arrPara);
            itemfinishedid = Convert.ToInt32(_arrPara[0].Value);
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            Logs.WriteErrorLog("getItemParametreId|" + ex.Message);
        }
        finally
        {

        }
        return itemfinishedid;
    }
    public static double Getmottelingrate(string Itemid, string Qualityid, string Jobid, string empid, string @effectivedate, string shadecolorid = "0", string Conetype = "", string PlyType = "", string TransportType = "")
    {
        double Rate = 0;
        SqlParameter[] param = new SqlParameter[10];
        param[0] = new SqlParameter("@EMPID", empid);
        param[1] = new SqlParameter("@ITEMID", Itemid);
        param[2] = new SqlParameter("@QUALITYID", Qualityid);
        param[3] = new SqlParameter("@PROCESSID", Jobid);
        param[4] = new SqlParameter("@Effectivedate", effectivedate);
        param[5] = new SqlParameter("@Rate", SqlDbType.Float);
        param[5].Direction = ParameterDirection.Output;
        param[6] = new SqlParameter("@shadecolorid", shadecolorid);
        param[7] = new SqlParameter("@Conetype", Conetype);
        param[8] = new SqlParameter("@PlyType", PlyType);
        param[9] = new SqlParameter("@TransportType", TransportType);

        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETMOTTELINGRATE", param);
        Rate = Convert.ToDouble(param[5].Value);
        return Rate;
    }
    public static double GetHandSpinningRate(string Itemid, string Qualityid, string Jobid, string empid, string @effectivedate, string shadecolorid = "0")
    {
        double Rate = 0;
        SqlParameter[] param = new SqlParameter[7];
        param[0] = new SqlParameter("@EMPID", empid);
        param[1] = new SqlParameter("@ITEMID", Itemid);
        param[2] = new SqlParameter("@QUALITYID", Qualityid);
        param[3] = new SqlParameter("@PROCESSID", Jobid);
        param[4] = new SqlParameter("@Effectivedate", effectivedate);
        param[5] = new SqlParameter("@Rate", SqlDbType.Float);
        param[5].Direction = ParameterDirection.Output;
        param[6] = new SqlParameter("@shadecolorid", shadecolorid);

        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETHANDSPINNINGRATE", param);
        Rate = Convert.ToDouble(param[5].Value);
        return Rate;
    }
    public static string GetItemProcessid(string Itemid = "0", string Processid = "0", string QualityId = "0", string designid = "0")
    {
        string FromProcessid = "0";
        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@QualityId", QualityId);
        param[1] = new SqlParameter("@Processid", Processid);
        param[2] = new SqlParameter("@DesignId", designid);
        param[3] = new SqlParameter("@FromProcessid", SqlDbType.Int);
        param[3].Direction = ParameterDirection.Output;
        param[4] = new SqlParameter("@ItemId", Itemid);
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "[PRO_GETFROMPROCESSIDOTHER]", param);
        FromProcessid = param[3].Value.ToString();
        return FromProcessid;
    }
    public static Double getDyerstockQty(string companyid, string EmpId, string Lotno, int Item_finished_id, string TagNo = "Without Tag No", string BinNo = "")
    {
        Double DyerstockqTY = 0.0;
        string str = "select Round(isnull(sum(Qtyinhand),0),3) as Qtyinhand from DyerStockTran Where companyId= " + companyid + " and EmpId=" + EmpId + " and Lotno='" + Lotno + "' and Item_finished_id=" + Item_finished_id + " and TagNo='" + TagNo + "' and BinNo='" + BinNo + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DyerstockqTY = Convert.ToDouble(ds.Tables[0].Rows[0]["Qtyinhand"]);
        }
        return DyerstockqTY;
    }
    public static void FillBinNO(DropDownList DDBinNo = null, int godownid = 0, int Item_finished_id = 0, int New_Edit = 0, SqlTransaction Tran = null)
    {
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@Godownid", godownid);
        param[1] = new SqlParameter("@Item_finished_id", Item_finished_id);
        param[2] = new SqlParameter("@New_Edit", New_Edit);
        DataSet ds;
        if (Tran == null)
        {
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "FILL_BINNO", param);
        }
        else
        {
            ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "FILL_BINNO", param);
        }

        UtilityModule.ConditionalComboFillWithDS(ref DDBinNo, ds, 0, true, "--Plz Select--");
    }
    public static Double Getconeweight(string Conetype, int noofcone = 0)
    {
        Double coneweight = 0;
        if (Conetype != "")
        {

            string str = "select ConeWeight From ConeMaster WHere ConeType='" + Conetype + "'";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                coneweight = Convert.ToDouble(ds.Tables[0].Rows[0]["coneweight"]) * noofcone;
            }

        }
        return coneweight;
    }
    public static void updatestatus(int companyid, int userid, string Tablename = "", int Tableid = 0, string status = "")
    {
        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@COMPANYID", companyid);
        param[1] = new SqlParameter("@USERID", userid);
        param[2] = new SqlParameter("@Tablename", Tablename);
        param[3] = new SqlParameter("@Tableid", Tableid);
        param[4] = new SqlParameter("@status", status);

        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_UPDATESTATUS", param);
    }
    public static void FtAreaCalculate_WeaverOrder(TextBox VarLengthNew, TextBox VarWidthNew, TextBox VarAreaNew, int VarFactor, int MastercompanyId = 0)
    {
        int FootLength = 0;
        int FootWidth = 0;
        int FootHeight = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        int FootHeightInch = 0;
        int InchLength = 0;
        int InchWidth = 0;
        int InchHeight = 0;
        double VarArea = 0;
        double VarVolume = 0;
        string Str = "";

        Str = string.Format("{0:#0.00}", Convert.ToDouble(VarLengthNew.Text == "" ? "0" : VarLengthNew.Text));
        switch (MastercompanyId.ToString())
        {
            case "6":
            case "12":
                InchLength = Convert.ToInt32(Convert.ToDouble(VarLengthNew.Text) * 12);
                InchWidth = Convert.ToInt32(Convert.ToDouble(VarWidthNew.Text) * 12);
                break;
            default:
                if (VarLengthNew.Text != "")
                {
                    if (VarLengthNew.Text != "")
                    {
                        FootLength = Convert.ToInt32(Str.Split('.')[0]);
                        FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                    }
                    if (FootLengthInch > 11)
                    {
                        VarLengthNew.Text = "";
                        VarLengthNew.Focus();
                    }
                }
                if (VarWidthNew.Text != "")
                {
                    Str = string.Format("{0:#0.00}", Convert.ToDouble(VarWidthNew.Text));
                    if (VarWidthNew.Text != "")
                    {
                        FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                        FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                    }
                    if (FootWidthInch > 11)
                    {
                        VarWidthNew.Text = "";
                        VarWidthNew.Focus();
                    }
                }
                InchLength = (FootLength * 12) + FootLengthInch;
                InchWidth = (FootWidth * 12) + FootWidthInch;
                break;
        }
        VarArea = Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth)) / 144 * VarFactor, 4);
        VarAreaNew.Text = Convert.ToString(Math.Round(VarArea, Convert.ToInt16(MySession.RoundFtFlag), MidpointRounding.AwayFromZero));
    }
    public static void AreaMtSq_Weaverorder(double txtL, double txtW, TextBox VarAreaNew, string ShapeName = "", int MastercompanyId = 0)
    {
        Double Area = 0;
        if (ShapeName.Trim() == "ROUND")
        {
            Area = ((txtL * txtW) / 10000) * Convert.ToDouble(variable.VarMfactor);
            if (variable.VarAreaMtrRound == "0") // No Rounding            {
            {
                VarAreaNew.Text = UtilityModule.DecimalvalueUptoWithoutRounding(Area, variable.VarRoundMtrFlag).ToString();
            }
            else
            {
                VarAreaNew.Text = Math.Round(Area, variable.VarRoundMtrFlag).ToString();

            }
        }
        else
        {
            Area = (txtL * txtW) / 10000;
            VarAreaNew.Text = Math.Round(Area, variable.VarRoundMtrFlag).ToString();
        }
        if (MastercompanyId.ToString() == "9")
        {
            VarAreaNew.Text = Math.Round(Area, 4).ToString();
        }
    }
    public static string Encrypt(string encrypttext)
    {
        string EncryptionKey = "ENABLESOFTERP2605";
        byte[] clearBytes = Encoding.Unicode.GetBytes(encrypttext);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                encrypttext = Convert.ToBase64String(ms.ToArray());
            }
        }
        return encrypttext;
    }
    public static string Decrypt(string Decrypttext)
    {
        string EncryptionKey = "ENABLESOFTERP2605";
        byte[] cipherBytes = Convert.FromBase64String(Decrypttext);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                Decrypttext = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return Decrypttext;
    }   
    public static double Get_WeightPenalty(int ItemId, double qty, string effectivedate = "")
    {
        double VarWeightPenalityRate = 0;       

        string str = "select Rate from weightpenalitymaster where fromperc<" + qty + " and toPerc>=" + qty + " and ItemId=" + ItemId + " and effectivedatefrom<='" + effectivedate + "' and (effectivedateto is null or effectivedateto>'" + effectivedate + "')";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            VarWeightPenalityRate = Convert.ToDouble(ds.Tables[0].Rows[0]["rate"]);
        }

        return VarWeightPenalityRate;

        ///return Math.Round(VarWeightPenalityRate, 3);
        ///return Math.Round(VarConsump, 2);
    }
    public static double ConvertInchesToFtHafizia(double VarInches)
    {
        int i;
        string X, Y, Z;
        double a, b;
        a = Convert.ToDouble(VarInches);
        b = a / 12;

        switch (HttpContext.Current.Session["varcompanyNo"].ToString())
        {
            case "16":
            case "28":
                Z = b.ToString();
                break;
            default:
                X = Convert.ToString(b);
                i = Convert.ToInt32(X.Split('.')[0]);
                b = a % 12;
                b = Convert.ToInt32(b);

                if (b < 10)
                {
                    Y = "0" + Convert.ToString(b);
                    Z = Convert.ToString(i) + "." + Y;
                }
                else
                {
                    Y = Convert.ToString(b);
                    Z = Convert.ToString(i) + "." + Y;
                }
                break;
        }
        return Convert.ToDouble(Z);
    }
}