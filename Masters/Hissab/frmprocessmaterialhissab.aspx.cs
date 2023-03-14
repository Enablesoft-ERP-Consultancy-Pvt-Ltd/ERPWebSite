using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Hissab_frmprocessmaterialhissab : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"SELECT CI.COMPANYID,CI.COMPANYNAME FROM COMPANYINFO CI(Nolock) 
                        INNER JOIN COMPANY_AUTHENTICATION CA(Nolock) ON CI.COMPANYID=CA.COMPANYID WHERE CA.USERID=" + Session["varuserid"] + @"
                        SELECT PROCESS_NAME_ID,PROCESS_NAME FROM PROCESS_NAME_MASTER(Nolock) 
                        WHERE PROCESS_NAME IN('YARN OPENING+MOTTELING','MOTTELING','HAND SPINNING', 'HANK MAKING') 
                        UNION
                        SELECT 999 AS PROCESS_NAME_ID,'YARN OPENING' PROCESS_NAME 
                        UNION
                        Select Distinct a.ProcessID, PNM.PROCESS_NAME 
                        From ProcessIssueToTasselMakingMaster a(Nolock) 
                        JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = a.ProcessID ORDER BY PROCESS_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessname, ds, 1, true, "--Plz Select--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            txtbilldate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (Session["canedit"].ToString() == "1")
            {
                ChkEditOrder.Visible = true;
            }
        }
    }
    protected void DDProcessname_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        ViewState["hissabid"] = "0";
        switch (DDProcessname.SelectedItem.Text.ToUpper())
        {
            case "HAND SPINNING":
                str = @"SELECT DISTINCT EI.EMPID,EI.EMPNAME + CASE WHEN ISNULL(EI.EMPCODE,'')<> '' THEN  ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME FROM HANDSPINNINGISSUEMASTER HIM 
                        INNER JOIN EMPINFO EI ON HIM.EMPID=EI.EMPID AND HIM.STATUS='COMPLETE' WHERE HIM.COMPANYID=" + DDCompanyName.SelectedValue + " AND HIM.PROCESSID=" + DDProcessname.SelectedValue + @"
                        ORDER BY EMPNAME";
                break;
            case "MOTTELING":
            case "YARN OPENING+MOTTELING":
            case "HANK MAKING":
                str = @"SELECT DISTINCT EI.EMPID,EI.EMPNAME + CASE WHEN ISNULL(EI.EMPCODE,'')<> '' THEN  ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME FROM MOTTELINGISSUEMASTER MIM INNER JOIN EMPINFO EI ON MIM.EMPID=EI.EMPID
                       and MIM.Status='COMPLETE' WHere MIM.companyid=" + DDCompanyName.SelectedValue + " and MIM.processid=" + DDProcessname.SelectedValue + " order by EMPNAME";
                break;
            case "YARN OPENING":
                str = @"SELECT DISTINCT EI.EMPID,EI.EMPNAME + CASE WHEN ISNULL(EI.EMPCODE,'')<> '' THEN  ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME FROM YARNOPENINGISSUEMASTER YIM 
                        INNER JOIN EMPINFO EI ON YIM.VENDORID=EI.EMPID  AND YIM.STATUS='COMPLETE' WHERE YIM.COMPANYID=" + DDCompanyName.SelectedValue + " order by empname";
                break;
            case "TUFTING":
            case "TASSEL MAKING":
            case "POM-POM MAKING":
            case "BRAIDING":
                str = @"SELECT DISTINCT EI.EMPID,EI.EMPNAME + CASE WHEN ISNULL(EI.EMPCODE,'')<> '' THEN  ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME 
                    FROM ProcessIssueToTasselMakingMaster MIM INNER JOIN EMPINFO EI ON MIM.EMPID=EI.EMPID
                       and MIM.Status='COMPLETE' WHere MIM.companyid=" + DDCompanyName.SelectedValue + " and MIM.processid=" + DDProcessname.SelectedValue + " order by EMPNAME";
                break;
            default:
                DDpartyname.Items.Clear();
                break;
        }
        if (str != "")
        {
            UtilityModule.ConditionalComboFill(ref DDpartyname, str, true, "--Plz Select--");
        }
    }
    protected void DDpartyname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["hissabid"] = "0";
        if (Tdbillno.Visible == true)
        {
            PartyNameSelectedChange();
        }
        Fillgrid();
    }
    private void PartyNameSelectedChange()
    {
        if (DDpartyname.SelectedIndex > 0 && ChkEditOrder.Checked == true)
        {
            string str = @"Select HissabId,BillNo From RAWMATERIALPROCESSHISSABMASTER Where billstatus =0 And CompanyId=" + DDCompanyName.SelectedValue + @" 
                          And Processid=" + DDProcessname.SelectedValue + " And empid=" + DDpartyname.SelectedValue;
            str = str + " Order By BillNo";
            UtilityModule.ConditionalComboFill(ref DDBillNo, str, true, "--Select Bill No--");
        }
    }
    protected void Fillgrid()
    {
        DGDetail.DataSource = null;
        DGDetail.DataBind();
        try
        {
            if (DDProcessname.SelectedIndex > 0)
            {

                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@Companyid", DDCompanyName.SelectedValue);
                param[1] = new SqlParameter("@Processname", DDProcessname.SelectedItem.Text);
                param[2] = new SqlParameter("@Empid", DDpartyname.SelectedIndex < 0 ? "0" : DDpartyname.SelectedValue);
                param[3] = new SqlParameter("@Hissabid", DDBillNo.SelectedIndex != -1 ? DDBillNo.SelectedValue : "0");

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETRAWMATERIALPROCESSHISSABDETAIL", param);

                DGDetail.DataSource = ds.Tables[0];
                DGDetail.DataBind();
                decimal Totalamt = 0;
                for (int i = 0; i < DGDetail.Rows.Count; i++)
                {
                    CheckBox chkbox = (CheckBox)DGDetail.Rows[i].FindControl("chkbox");
                    TextBox txttotalamtgrid = (TextBox)DGDetail.Rows[i].FindControl("txttotalamt");
                    if (chkbox.Checked == true)
                    {
                        Totalamt += Convert.ToDecimal(txttotalamtgrid.Text == "" ? "0" : txttotalamtgrid.Text);
                    }
                }
                txttotalamt.Text = Totalamt.ToString();

            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        //*********GET CHECKED DETAILS
        DataTable dt = new DataTable();
        dt.Columns.Add("Issueid", typeof(int));
        dt.Columns.Add("Amount", typeof(float));
        for (int i = 0; i < DGDetail.Rows.Count; i++)
        {
            CheckBox Chkbox = (CheckBox)DGDetail.Rows[i].FindControl("Chkbox");
            if (Chkbox.Checked == true)
            {
                DataRow dr = dt.NewRow();
                Label lblissueid = (Label)DGDetail.Rows[i].FindControl("lblissueid");
                TextBox txttotalamt = (TextBox)DGDetail.Rows[i].FindControl("txttotalamt");
                dr["issueid"] = lblissueid.Text;
                dr["Amount"] = txttotalamt.Text;
                dt.Rows.Add(dr);
            }

        }
        //*********
        if (dt.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select atlest one Checkbox to Save Data.')", true);
            return;
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[15];
            param[0] = new SqlParameter("@Hissabid", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = ViewState["hissabid"];
            param[1] = new SqlParameter("@companyid", DDCompanyName.SelectedValue);
            param[2] = new SqlParameter("@Processid", DDProcessname.SelectedValue);
            param[3] = new SqlParameter("@Empid", DDpartyname.SelectedValue);
            param[4] = new SqlParameter("@BillNo", SqlDbType.VarChar, 50);
            param[4].Direction = ParameterDirection.InputOutput;
            param[4].Value = txtbillno.Text;
            param[5] = new SqlParameter("@TotalAmount", txttotalamt.Text == "" ? "0" : txttotalamt.Text);
            param[6] = new SqlParameter("@BillDate", txtbilldate.Text);
            param[7] = new SqlParameter("@Remark", txtremark.Text);
            param[8] = new SqlParameter("@userid", Session["varuserid"]);
            param[9] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            param[10] = new SqlParameter("@dt", dt);
            param[11] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[11].Direction = ParameterDirection.Output;
            param[12] = new SqlParameter("@DeductionAmt", txtDeductionAmt.Text == "" ? "0" : txtDeductionAmt.Text);
            param[13] = new SqlParameter("@AdditionAmt", txtAdditionAmt.Text == "" ? "0" : txtAdditionAmt.Text);
            param[14] = new SqlParameter("@NetAmt", txtNetAmt.Text == "" ? "0" : txtNetAmt.Text);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVERAWMATERIALPROCESSHISSAB", param);
            ViewState["hissabid"] = param[0].Value.ToString();
            txtbillno.Text = param[4].Value.ToString();
            if (param[11].Value.ToString() != "")
            {
                lblmsg.Text = param[11].Value.ToString();
                Tran.Rollback();
            }
            else
            {
                Tran.Commit();
                lblmsg.Text = "Data Saved Successfully.";
                Refreshcontrol();
                Fillgrid();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            throw;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void Refreshcontrol()
    {
        txttotalamt.Text = "";
        txtremark.Text = "";
        txtDeductionAmt.Text = "";
        txtAdditionAmt.Text = "";
        txtNetAmt.Text = "";
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            Tdbillno.Visible = true;
            PartyNameSelectedChange();
        }
        else
        {
            Tdbillno.Visible = false;
        }
        Fillgrid();
        txttotalamt.Text = "";
        txtbilldate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        txtbillno.Text = "";
        txtremark.Text = "";
    }
    protected void DDBillNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["hissabid"] = DDBillNo.SelectedValue;
        string str = "";
        str = @"Select BillNo,replace(convert(varchar(11),billdate,106), ' ','-') as Date,Remark,isnull(DeductionAmt,0) as DeductionAmt,isnull(AdditionAmt,0) as AdditionAmt,isnull(NetAmt,0) as NetAmt From RAWMATERIALPROCESSHISSABMASTER Where Hissabid=" + DDBillNo.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "";
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            txtbillno.Text = Ds.Tables[0].Rows[0]["BillNo"].ToString();
            txtbilldate.Text = Ds.Tables[0].Rows[0]["Date"].ToString();
            txtremark.Text = Ds.Tables[0].Rows[0]["Remark"].ToString();
            txtDeductionAmt.Text = Ds.Tables[0].Rows[0]["DeductionAmt"].ToString();
            txtAdditionAmt.Text = Ds.Tables[0].Rows[0]["AdditionAmt"].ToString();
            txtNetAmt.Text = Ds.Tables[0].Rows[0]["NetAmt"].ToString();

        }
        Fillgrid();
    }
    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblflag = (Label)e.Row.FindControl("lblflag");
            CheckBox chkbox = (CheckBox)e.Row.FindControl("chkbox");
            if (lblflag.Text == "1")
            {
                chkbox.Checked = true;
            }
        }
    }
    protected void Btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Hissabid", ViewState["hissabid"]);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETRAWHISSABREPORT", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\rptrawhissabreport.rpt";
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptrawhissabreport.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altr", "alert('No recors fetched..);", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void Fill_NetAmountAfterDeductionAddition()
    {
        Double Deduction = 0;
        Double Addition = 0;
        Double Gst = 0;
        Double totalamt = Convert.ToDouble(txttotalamt.Text);       

        if (txtAdditionAmt.Text != "")
        {
            Addition = Convert.ToDouble(txtAdditionAmt.Text == "" ? "0" : txtAdditionAmt.Text);
        }
        if (txtDeductionAmt.Text != "")
        {
            Deduction = Convert.ToDouble(txtDeductionAmt.Text == "" ? "0" : txtDeductionAmt.Text);
        }

        totalamt = totalamt + Addition - Deduction;      

        txtNetAmt.Text= Math.Round((totalamt), 0).ToString();        
    }
    protected void txtDeductionAmt_TextChanged(object sender, EventArgs e)
    {
        Fill_NetAmountAfterDeductionAddition();
    }
    protected void txtAdditionAmt_TextChanged(object sender, EventArgs e)
    {
        Fill_NetAmountAfterDeductionAddition();
    }
}