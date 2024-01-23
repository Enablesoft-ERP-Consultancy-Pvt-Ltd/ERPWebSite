using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;
using System.IO;
using ClosedXML.Excel;
public partial class Masters_Order_FrmOrderChecklist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            lblvalidMessage.Text = string.Empty;
            Session["order_id"] = 0;
            TxtCustOrderNo.Enabled = true;

            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName", true, "--SELECT--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "SELECT customerid,CompanyName + SPACE(5)+Customercode from customerinfo where Customercode<>'' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName", true, "--SELECT--");
        }
    }
    protected void TxtCustOrderNo_TextChanged(object sender, EventArgs e)
    {
        TxtCustOrderNo_Validate();
    }
    protected void TxtCustOrderNo_Validate()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            string CustOrderNo = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "select isnull(CustomerOrderNo,0) asd from OrderMaster where CustomerOrderNo='" + TxtCustOrderNo.Text + "'"));
            if (CustOrderNo != "")
            {
                TxtCustOrderNo.Text = "";
                //  TxtLocalOrderNo.Text = "";
                TxtCustOrderNo.Focus();
                Lblmessage.Visible = true;
                Lblmessage.Text = "Customer Order Number Already Exist......";
            }
            else
            {
                string Str = SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Max(IsNull(Round(Replace(LocalOrder,'L ',''),0),0)+1),1) From ORDERMASTER Where LocalOrder Like 'L %'").ToString();
                //  TxtLocalOrderNo.Text = "L " + Str;
                Lblmessage.Visible = false;
                Lblmessage.Text = "";
            }
        }
        catch (Exception)
        {
            Lblmessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        bool isvalid = CHECKVALIDCONTROL();
        lblvalidMessage.Text = string.Empty;
        Lblmessage.Text = "";
        if (isvalid)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@Docid", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = hndocid.Value;
                param[1] = new SqlParameter("@Companyid", DDCompanyName.SelectedValue);
                param[2] = new SqlParameter("@DocNo", SqlDbType.VarChar, 50);
                param[2].Value = txtdocno.Text;
                param[2].Direction = ParameterDirection.InputOutput;
                param[3] = new SqlParameter("@userid", Session["varuserid"]);
                param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[4].Direction = ParameterDirection.Output;

                //*********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEORDERCHECKLIST", param);
                Lblmessage.Text = param[4].Value.ToString();
                txtdocno.Text = param[2].Value.ToString();
                hndocid.Value = param[0].Value.ToString();

                //  at the time of update delete all the data in tables
                string str1 = @"DELETE FROM ORDERCHECKLISTDETAILS WHERE DOCID=" + hndocid.Value;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);

                insertinto_RAWYARNINSPECTIONDETAIL(Tran);

                ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + param[4].Value.ToString() + "')", true);

                Tran.Commit();
                // refreshform();
                //**********

            }
            catch (Exception ex)
            {
                Lblmessage.Text = ex.Message;
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
            lblvalidMessage.Text = "Date is must for checked process.";
            refreshform();

        }
    }
    private void refreshform()
    {
        // lblvalidMessage.Text = string.Empty;
        TxtCustOrderNo.Text = string.Empty;
        //   hndocid.Value = "0";
        txtdocno.Text = string.Empty;
        ChkbuyerOrder.Checked = false;
        chksystemorder.Checked = false;
        chkproforma.Checked = false;
        chkrawmaterial.Checked = false;
        chkaccessories.Checked = false;
        chkBOM.Checked = false;
        chkspecification.Checked = false;
        chkppm.Checked = false;
        chckprocessrate.Checked = false;
        chkta.Checked = false;
        chkchecklist.Checked = false;
        txtdatebuyer.Text = String.Empty;
        txtsystemorder.Text = String.Empty;
        txtproforma.Text = String.Empty;
        txtrawmetrial.Text = String.Empty;
        txtaccessories.Text = String.Empty;
        txtBOM.Text = String.Empty;
        txtspecification.Text = String.Empty;
        txtppm.Text = String.Empty;
        txtprocessrate.Text = String.Empty;
        txtta.Text = String.Empty;
        txtchecklist.Text = String.Empty;
    }
    private bool CHECKVALIDCONTROL()
    {
        bool validate = true;
        if (ChkbuyerOrder.Checked)
        {
            if (!string.IsNullOrEmpty(txtdatebuyer.Text))
            { validate = true; }
            else
            { return false; }
        }
        if (chksystemorder.Checked)
        {
            if (!string.IsNullOrEmpty(txtsystemorder.Text))
            { validate = true; }
            else
            { return false; }
        }
        if (chkproforma.Checked)
        {
            if (!string.IsNullOrEmpty(txtproforma.Text))
            { validate = true; }
            else
            { return false; }
        }
        if (chkrawmaterial.Checked)
        {
            if (!string.IsNullOrEmpty(txtrawmetrial.Text))
            { validate = true; }
            else
            { return false; }
        }
        if (chkaccessories.Checked)
        {
            if (!string.IsNullOrEmpty(txtaccessories.Text))
            { validate = true; }
            else
            { return false; }
        }
        if (chkBOM.Checked)
        {
            if (!string.IsNullOrEmpty(txtBOM.Text))
            { validate = true; }
            else
            { return false; }
        }
        if (chkspecification.Checked)
        {
            if (!string.IsNullOrEmpty(txtspecification.Text))
            { validate = true; }
            else
            { return false; }
        }
        if (chkta.Checked)
        {
            if (!string.IsNullOrEmpty(txtta.Text))
            { validate = true; }
            else
            { return false; }
        }
        if (chkppm.Checked)
        {
            if (!string.IsNullOrEmpty(txtppm.Text))
            { validate = true; }
            else
            { return false; }
        }
        if (chckprocessrate.Checked)
        {
            if (!string.IsNullOrEmpty(txtprocessrate.Text))
            { validate = true; }
            else
            { return false; }
        }
        if (chkchecklist.Checked)
        {
            if (!string.IsNullOrEmpty(txtchecklist.Text))
            { validate = true; }
            else
            { return false; }
        }
        return validate;
    }

    private void insertinto_RAWYARNINSPECTIONDETAIL(SqlTransaction Tran)
    {

        int BUYERORDRSTATUS = ChkbuyerOrder.Checked ? 1 : 0;
        int SYSTEMORDRSTATUS = chksystemorder.Checked ? 1 : 0;
        int PIORDRSTATUS = chkproforma.Checked ? 1 : 0;
        int RAWCONSUMPTIONORDRSTATUS = chkrawmaterial.Checked ? 1 : 0;
        int ACCCONSUMPTIONORDRSTATUS = chkaccessories.Checked ? 1 : 0;
        int BOMORDRSTATUS = chkBOM.Checked ? 1 : 0;
        int SPECIFICATIONORDRSTATUS = chkspecification.Checked ? 1 : 0;
        int PPMORDRSTATUS = chkppm.Checked ? 1 : 0;
        int PROCESSRATEORDRSTATUS = chckprocessrate.Checked ? 1 : 0;
        int TAORDRSTATUS = chkta.Checked ? 1 : 0;
        int CHECKLISTORDRSTATUS = chkchecklist.Checked ? 1 : 0;


        //   string result = null;
        string str = @"Insert into ORDERCHECKLISTDETAILS (DOCID, COMPANYID, USERID,CUSTOMERCODE,CUSTORDERNO, BUYERORDREDATE, BUYERORDRSTATUS, SYSTEMORDREDATE, 
                     SYSTEMORDRSTATUS, PIORDREDATE, PIORDRSTATUS, RAWCONSUMPTIONORDREDATE, RAWCONSUMPTIONORDRSTATUS, ACCCONSUMPTIONORDREDATE, ACCCONSUMPTIONORDRSTATUS, BOMORDREDATE, BOMORDRSTATUS, SPECIFICATIONORDREDATE,SPECIFICATIONORDRSTATUS, PPMORDREDATE, PPMORDRSTATUS, PROCESSRATEORDREDATE, PROCESSRATEORDRSTATUS, TAORDREDATE, TAORDRSTATUS, CHECKLISTORDREDATE, CHECKLISTORDRSTATUS, DATEADDED, APPROVER_USERID)
                     values(" + hndocid.Value + "," + DDCompanyName.SelectedValue + "," + Session["varuserid"].ToString() + "," + DDCustomerCode.SelectedValue + ",'" + TxtCustOrderNo.Text + "','" + txtdatebuyer.Text + "'," + BUYERORDRSTATUS + ",'" + txtsystemorder.Text + "'," + SYSTEMORDRSTATUS + ",'" + txtproforma.Text + "'," + PIORDRSTATUS + ",'" + txtrawmetrial.Text + "'," + RAWCONSUMPTIONORDRSTATUS + ",'" + txtaccessories.Text + "'," + ACCCONSUMPTIONORDRSTATUS + ",'" + txtBOM.Text + "'," + BOMORDRSTATUS + ",'" + txtspecification.Text + "'," + SPECIFICATIONORDRSTATUS + ",'" + txtppm.Text + "'," + PPMORDRSTATUS + ",'" + txtprocessrate.Text + "'," + PROCESSRATEORDRSTATUS + ",'" + txtta.Text + "'," + TAORDRSTATUS + ",'" + txtchecklist.Text + "'," + CHECKLISTORDRSTATUS + ",'" + DateTime.Now + "'," + Session["varuserid"].ToString() + ")";

        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["order_id"] = 0;
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void DDDocNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hndocid.Value = DDDocNo.SelectedValue;
        refreshform();
        FillDataback();
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {

        TDDocno.Visible = false;


        hndocid.Value = "0";
        DDDocNo.Items.Clear();
        // refreshcontrol();
        if (chkedit.Checked == true)
        {
            TDDocno.Visible = true;

            fillDocno();
        }
    }
    private void fillDocno()
    {
        string str = @"SELECT  CHKM.DOCID,CHKM.DOCNO +' # ' +Replace(convert(nvarchar(11),CHKM.DATEADDED,106),' ','-') as DocNo 
                    FROM ORDERCHECKLISTMASTER CHKM(nolock) 
                    INNER JOIN ORDERCHECKLISTDETAILS CHKD(nolock) ON CHKM.DOCID=CHKD.DOCID
                    Where CHKM.COMPANYID=" + DDCompanyName.SelectedValue;

        str = str + " order by DOCID";
        UtilityModule.ConditionalComboFill(ref DDDocNo, str, true, "--Plz Select--");
    }
    protected void FillDataback()
    {
        string str = @"SELECT RIM.DOCID,DOCNO,RIM.USERID,RIM.COMPANYID,CUSTOMERCODE,CUSTORDERNO, BUYERORDREDATE, BUYERORDRSTATUS, SYSTEMORDREDATE, 
                     SYSTEMORDRSTATUS, PIORDREDATE, PIORDRSTATUS, RAWCONSUMPTIONORDREDATE, RAWCONSUMPTIONORDRSTATUS, ACCCONSUMPTIONORDREDATE, ACCCONSUMPTIONORDRSTATUS, BOMORDREDATE, BOMORDRSTATUS, SPECIFICATIONORDREDATE,SPECIFICATIONORDRSTATUS, PPMORDREDATE, PPMORDRSTATUS, PROCESSRATEORDREDATE, PROCESSRATEORDRSTATUS, TAORDREDATE, TAORDRSTATUS, CHECKLISTORDREDATE, CHECKLISTORDRSTATUS 
                        FROM ORDERCHECKLISTMASTER(nolock)  RIM 
                        INNER JOIN ORDERCHECKLISTDETAILS(nolock)  RID ON RIM.DOCID=RID.DOCID 
                        Where RIM.COMPANYID = " + DDCompanyName.SelectedValue + " And RIM.DOCID=" + DDDocNo.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtdocno.Text = ds.Tables[0].Rows[0]["DocNo"].ToString();
            //if (DDCompanyName.Items.FindByText(ds.Tables[0].Rows[0]["COMPANYID"].ToString()) != null)
            //{
            //    DDCompanyName.SelectedValue = ds.Tables[0].Rows[0]["COMPANYID"].ToString();
            //}
            if (DDCustomerCode.Items.FindByText(ds.Tables[0].Rows[0]["CUSTOMERCODE"].ToString()) != null)
            {
                DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["CUSTOMERCODE"].ToString();
            }
            TxtCustOrderNo.Text = ds.Tables[0].Rows[0]["CUSTORDERNO"].ToString();

            if (ds.Tables[0].Rows[0]["BUYERORDRSTATUS"].ToString() == "1")
            {
                ChkbuyerOrder.Checked = true;
                txtdatebuyer.Text = ds.Tables[0].Rows[0]["BUYERORDREDATE"].ToString();
            }
            else ChkbuyerOrder.Checked = false;
            if (ds.Tables[0].Rows[0]["SYSTEMORDRSTATUS"].ToString() == "1")
            {
                chksystemorder.Checked = true;
                txtsystemorder.Text = ds.Tables[0].Rows[0]["SYSTEMORDREDATE"].ToString();
            }
            else chksystemorder.Checked = false;

            if (ds.Tables[0].Rows[0]["PIORDRSTATUS"].ToString() == "1")
            {
                chkproforma.Checked = true;
                txtproforma.Text = ds.Tables[0].Rows[0]["PIORDREDATE"].ToString();
            }
            else chkproforma.Checked = false;

            if (ds.Tables[0].Rows[0]["RAWCONSUMPTIONORDRSTATUS"].ToString() == "1")
            {
                chkrawmaterial.Checked = true;
                txtrawmetrial.Text = ds.Tables[0].Rows[0]["RAWCONSUMPTIONORDREDATE"].ToString();
            }
            else chkrawmaterial.Checked = false;

            if (ds.Tables[0].Rows[0]["ACCCONSUMPTIONORDRSTATUS"].ToString() == "1")
            {
                chkaccessories.Checked = true;
                txtaccessories.Text = ds.Tables[0].Rows[0]["ACCCONSUMPTIONORDREDATE"].ToString();
            }
            else chkaccessories.Checked = false;

            if (ds.Tables[0].Rows[0]["BOMORDRSTATUS"].ToString() == "1")
            {
                chkBOM.Checked = true;
                txtBOM.Text = ds.Tables[0].Rows[0]["BOMORDREDATE"].ToString();
            }
            else chkBOM.Checked = false;

            if (ds.Tables[0].Rows[0]["SPECIFICATIONORDRSTATUS"].ToString() == "1")
            {
                chkspecification.Checked = true;
                txtspecification.Text = ds.Tables[0].Rows[0]["SPECIFICATIONORDREDATE"].ToString();
            }
            else chkspecification.Checked = false;

            if (ds.Tables[0].Rows[0]["PPMORDRSTATUS"].ToString() == "1")
            {
                chkppm.Checked = true;
                txtppm.Text = ds.Tables[0].Rows[0]["PPMORDREDATE"].ToString();
            }
            else chkppm.Checked = false;

            if (ds.Tables[0].Rows[0]["PROCESSRATEORDRSTATUS"].ToString() == "1")
            {
                chckprocessrate.Checked = true;
                txtprocessrate.Text = ds.Tables[0].Rows[0]["PROCESSRATEORDREDATE"].ToString();
            }
            else chckprocessrate.Checked = false;

            if (ds.Tables[0].Rows[0]["TAORDRSTATUS"].ToString() == "1")
            {
                chkta.Checked = true;
                txtta.Text = ds.Tables[0].Rows[0]["TAORDREDATE"].ToString();
            }
            else chkta.Checked = false;

            if (ds.Tables[0].Rows[0]["CHECKLISTORDRSTATUS"].ToString() == "1")
            {
                chkchecklist.Checked = true;
                txtchecklist.Text = ds.Tables[0].Rows[0]["CHECKLISTORDREDATE"].ToString();
            }
            else chkchecklist.Checked = false;
            //txtdate.Text = ds.Tables[0].Rows[0]["ReportDate"].ToString();
            //txtchallannodate.Text = ds.Tables[0].Rows[0]["ChallanNo_Date"].ToString();
            //txtyarntype.Text = ds.Tables[0].Rows[0]["YarnType"].ToString();
            //txtcount.Text = ds.Tables[0].Rows[0]["count"].ToString();
            //txtlotno.Text = ds.Tables[0].Rows[0]["Lotno"].ToString();
            //txttotalbale.Text = ds.Tables[0].Rows[0]["totalbale"].ToString();
            //txtsamplesize.Text = ds.Tables[0].Rows[0]["Samplesize"].ToString();
            //txtnoofhank.Text = ds.Tables[0].Rows[0]["NoofHank"].ToString();
            //TxtVenderLotNo.Text = ds.Tables[0].Rows[0]["VenderLotNo"].ToString();
            ////1
            //txtspecification_1.Text = ds.Tables[0].Rows[0]["Specification_1"].ToString();
            //txt1_1.Text = ds.Tables[0].Rows[0]["One_1"].ToString();
            //txt1_2.Text = ds.Tables[0].Rows[0]["Two_1"].ToString();
            //txt1_3.Text = ds.Tables[0].Rows[0]["Three_1"].ToString();
            //txt1_4.Text = ds.Tables[0].Rows[0]["Four_1"].ToString();
            //txt1_5.Text = ds.Tables[0].Rows[0]["Five_1"].ToString();
            //txtavgvalue_1.Text = ds.Tables[0].Rows[0]["Avgvalue_1"].ToString();
            ////2
            //txtspecification_2.Text = ds.Tables[0].Rows[0]["Specification_2"].ToString();
            //txt2_1.Text = ds.Tables[0].Rows[0]["One_2"].ToString();
            //txt2_2.Text = ds.Tables[0].Rows[0]["Two_2"].ToString();
            //txt2_3.Text = ds.Tables[0].Rows[0]["Three_2"].ToString();
            //txt2_4.Text = ds.Tables[0].Rows[0]["Four_2"].ToString();
            //txt2_5.Text = ds.Tables[0].Rows[0]["Five_2"].ToString();
            //txtavgvalue_2.Text = ds.Tables[0].Rows[0]["Avgvalue_2"].ToString();
            ////3
            //txtspecification_3.Text = ds.Tables[0].Rows[0]["Specification_3"].ToString();
            //txt3_1.Text = ds.Tables[0].Rows[0]["One_3"].ToString();
            //txt3_2.Text = ds.Tables[0].Rows[0]["Two_3"].ToString();
            //txt3_3.Text = ds.Tables[0].Rows[0]["Three_3"].ToString();
            //txt3_4.Text = ds.Tables[0].Rows[0]["Four_3"].ToString();
            //txt3_5.Text = ds.Tables[0].Rows[0]["Five_3"].ToString();
            //txtavgvalue_3.Text = ds.Tables[0].Rows[0]["Avgvalue_3"].ToString();
            ////4
            //txtspecificationpet_4.Text = ds.Tables[0].Rows[0]["Specificationpet_4"].ToString();
            //lblcheckpointpet_4.Text = ds.Tables[0].Rows[0]["CHECKPOINTPET_4"].ToString();
            //txtpet4_1.Text = ds.Tables[0].Rows[0]["Onepet_4"].ToString();
            //txtpet4_2.Text = ds.Tables[0].Rows[0]["Twopet_4"].ToString();
            //txtpet4_3.Text = ds.Tables[0].Rows[0]["Threepet_4"].ToString();
            //txtpet4_4.Text = ds.Tables[0].Rows[0]["Fourpet_4"].ToString();
            //txtpet4_5.Text = ds.Tables[0].Rows[0]["Fivepet_4"].ToString();
            //txtavgvaluepet_4.Text = ds.Tables[0].Rows[0]["Avgvaluepet_4"].ToString();

            //txtspecificationother_4.Text = ds.Tables[0].Rows[0]["Specificationother_4"].ToString();
            //lblcheckpointother_4.Text = ds.Tables[0].Rows[0]["CHECKPOINTOTHER_4"].ToString();
            //txtother4_1.Text = ds.Tables[0].Rows[0]["Oneother_4"].ToString();
            //txtother4_2.Text = ds.Tables[0].Rows[0]["Twoother_4"].ToString();
            //txtother4_3.Text = ds.Tables[0].Rows[0]["Threeother_4"].ToString();
            //txtother4_4.Text = ds.Tables[0].Rows[0]["Fourother_4"].ToString();
            //txtother4_5.Text = ds.Tables[0].Rows[0]["Fiveother_4"].ToString();
            //txtavgvalueother_4.Text = ds.Tables[0].Rows[0]["AvgvalueOther_4"].ToString();
            ////5
            //txtspecification_5.Text = ds.Tables[0].Rows[0]["Specification_5"].ToString();
            //txt5_1.Text = ds.Tables[0].Rows[0]["One_5"].ToString();
            //txt5_2.Text = ds.Tables[0].Rows[0]["Two_5"].ToString();
            //txt5_3.Text = ds.Tables[0].Rows[0]["Three_5"].ToString();
            //txt5_4.Text = ds.Tables[0].Rows[0]["Four_5"].ToString();
            //txt5_5.Text = ds.Tables[0].Rows[0]["Five_5"].ToString();
            //txtavgvalue_5.Text = ds.Tables[0].Rows[0]["Avgvalue_5"].ToString();
            ////5
            //txtspecification_6.Text = ds.Tables[0].Rows[0]["Specification_6"].ToString();
            //txt6_1.Text = ds.Tables[0].Rows[0]["One_6"].ToString();
            //txt6_2.Text = ds.Tables[0].Rows[0]["Two_6"].ToString();
            //txt6_3.Text = ds.Tables[0].Rows[0]["Three_6"].ToString();
            //txt6_4.Text = ds.Tables[0].Rows[0]["Four_6"].ToString();
            //txt6_5.Text = ds.Tables[0].Rows[0]["Five_6"].ToString();
            //txtavgvalue_6.Text = ds.Tables[0].Rows[0]["Avgvalue_6"].ToString();
            ////7
            //txtspecification_7.Text = ds.Tables[0].Rows[0]["Specification_7"].ToString();
            //txt7_1.Text = ds.Tables[0].Rows[0]["One_7"].ToString();
            //txt7_2.Text = ds.Tables[0].Rows[0]["Two_7"].ToString();
            //txt7_3.Text = ds.Tables[0].Rows[0]["Three_7"].ToString();
            //txt7_4.Text = ds.Tables[0].Rows[0]["Four_7"].ToString();
            //txt7_5.Text = ds.Tables[0].Rows[0]["Five_7"].ToString();
            //txtavgvalue_7.Text = ds.Tables[0].Rows[0]["Avgvalue_7"].ToString();
            ////
            //txtcomments.Text = ds.Tables[0].Rows[0]["comments"].ToString();
            ////ddresult.SelectedItem.Text = ds.Tables[0].Rows[0]["status"].ToString();
            //if (ddresult.Items.FindByText(ds.Tables[0].Rows[0]["status"].ToString()) != null)
            //{
            //    ddresult.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
            //}
            //Changeapprovebuttoncolor(Convert.ToInt16(ds.Tables[0].Rows[0]["approvestatus"]));
            //EditRights_Button(Convert.ToInt16(Session["usertype"]), Convert.ToInt16(ds.Tables[0].Rows[0]["approvestatus"]));

        }
    }
    protected void preview()
    {
        Lblmessage.Text = "";
        // hdnfinishedid.Value = string.Empty;
        string Query = string.Empty;
        int totalrows = 0;
        //   newPreview1.Src = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //     SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarNum = 0;

            DataSet Ds;


            string str = @"SELECT DOCNO,CUSTORDERNO, BUYERORDREDATE, BUYERORDRSTATUS, SYSTEMORDREDATE, 
                     SYSTEMORDRSTATUS, PIORDREDATE, PIORDRSTATUS, RAWCONSUMPTIONORDREDATE, RAWCONSUMPTIONORDRSTATUS, ACCCONSUMPTIONORDREDATE, ACCCONSUMPTIONORDRSTATUS, BOMORDREDATE, BOMORDRSTATUS, SPECIFICATIONORDREDATE,SPECIFICATIONORDRSTATUS, PPMORDREDATE, PPMORDRSTATUS, PROCESSRATEORDREDATE, PROCESSRATEORDRSTATUS, TAORDREDATE, TAORDRSTATUS, CHECKLISTORDREDATE, CHECKLISTORDRSTATUS 
                        FROM ORDERCHECKLISTMASTER(nolock)  RIM 
                        INNER JOIN ORDERCHECKLISTDETAILS(nolock)  RID ON RIM.DOCID=RID.DOCID 
                        Where RIM.DOCID=" + hndocid.Value;

            Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
            //**********Confirm
            lblmsg.Text = "";
            if (Ds != null)
            {
                totalrows = Ds.Tables[0].Rows.Count;
                if (totalrows > 0)
                {

                    if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                    }
                    string Path = "";
                    var xapp = new XLWorkbook();
                    var sht = xapp.Worksheets.Add("sheet1");
                    int row = 0;

                    sht.Range("G1").Value = "CHAMPO CARPETS";
                    sht.Range("G1:H1").Style.Font.Bold = true;
                    sht.Range("G1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("G1:H1").Merge();
                    sht.Range("G1:H1").Style.Font.Bold = true;
                    sht.Range("F2").Value = "Order Process Checklist Status Report";
                    sht.Range("F2:I2").Style.Font.Bold = true;
                    sht.Range("F2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("F2:I2").Merge();
                    sht.Range("F2:I2").Style.Font.Bold = true;
                    // sht.Range("A1").SetValue("STOCKNO");
                    sht.Range("H3").SetValue("Doc. No.");
                    sht.Range("I3").SetValue(Ds.Tables[0].Rows[0]["DOCNO"]);
                    sht.Range("G5:H5").Style.Font.Bold = true;
                    sht.Range("G5").SetValue("CheklistName");
                    sht.Range("H5").SetValue("Date");
                    sht.Range("G6").SetValue("buyer order sheet");
                    sht.Range("H6").SetValue(Ds.Tables[0].Rows[0]["BUYERORDREDATE"]);
                    sht.Range("G7").SetValue("system order sheet");
                    sht.Range("H7").SetValue(Ds.Tables[0].Rows[0]["SYSTEMORDREDATE"]);
                    sht.Range("G8").SetValue("PI(PROFORMA INVOICE)");
                    sht.Range("H8").SetValue(Ds.Tables[0].Rows[0]["PIORDREDATE"]);
                    sht.Range("G9").SetValue("raw material consumption entry");
                    sht.Range("H9").SetValue(Ds.Tables[0].Rows[0]["RAWCONSUMPTIONORDREDATE"]);
                    sht.Range("G10").SetValue("accessories consumption entry");
                    sht.Range("H10").SetValue(Ds.Tables[0].Rows[0]["ACCCONSUMPTIONORDREDATE"]);
                    sht.Range("G11").SetValue("BOM");
                    sht.Range("H11").SetValue(Ds.Tables[0].Rows[0]["BOMORDREDATE"]);
                    sht.Range("G11").SetValue("specification");
                    sht.Range("H11").SetValue(Ds.Tables[0].Rows[0]["SPECIFICATIONORDREDATE"]);
                    sht.Range("G12").SetValue("ppm(PRE PRODUCTION MEETING)");
                    sht.Range("H12").SetValue(Ds.Tables[0].Rows[0]["PPMORDREDATE"]);
                    sht.Range("G13").SetValue("PROCESS RATES");
                    sht.Range("H13").SetValue(Ds.Tables[0].Rows[0]["PROCESSRATEORDREDATE"]);
                    sht.Range("G14").SetValue("T&A & PRODUCTION PLANNING");
                    sht.Range("H14").SetValue(Ds.Tables[0].Rows[0]["TAORDREDATE"]);
                    sht.Range("G15").SetValue("DOCUMENT CHECKLIST");
                    sht.Range("H15").SetValue(Ds.Tables[0].Rows[0]["CHECKLISTORDREDATE"]);


                    string Fileextension = ".xlsx";
                    string filename = UtilityModule.validateFilename("checklistSummaryDetails_" + DateTime.Now + Fileextension);
                    String Path1 = Server.MapPath("~/Tempexcel/" + filename);
                    xapp.SaveAs(Path1);
                    xapp.Dispose();
                    //Download File
                    Response.ClearContent();
                    Response.ClearHeaders();
                    // Response.Clear();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                    Response.WriteFile(Path1);
                    // File.Delete(Path);
                    Response.End();
                }
                else
                { Lblmessage.Text = "No Data Found!!!."; }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmStocktransferDetail.aspx");
            Lblmessage.Visible = true;
            Lblmessage.Text = ex.Message;
            // Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void btnpreview_Click(object sender, EventArgs e)
    {
        preview();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
}