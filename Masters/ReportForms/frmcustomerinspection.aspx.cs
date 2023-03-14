using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

using System.Text;

public partial class Masters_ReportForms_frmcustomerinspection : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CustomerId,CustomerCode +' ' +CompanyName as customercode From customerinfo order by customercode
                           select PROCESS_NAME_ID,PROCESS_NAME From PROCESS_NAME_MASTER Where ProcessType=1 order by PROCESS_NAME";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCustCode, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDprocessname, ds, 1, true, "--Plz Select--");
        }
    }
    protected void DDCustCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = @"select orderid,CustomerOrderNo 
                From OrderMaster 
                Where CompanyID = " + Session["CurrentWorkingCompanyID"] + " And Customerid=" + DDCustCode.SelectedValue + " order by CustomerOrderNo";

        if (Session["varcompanyId"].ToString() == "16" || Session["varcompanyId"].ToString() == "28")
        {
            Str = @"select orderid,CustomerOrderNo 
                        From OrderMaster 
                        Where Status = 0 And CompanyID = " + Session["CurrentWorkingCompanyID"] + " And Customerid=" + DDCustCode.SelectedValue + " order by CustomerOrderNo";
        }
        UtilityModule.ConditionalComboFill(ref DDorderNo, Str, true, "--Plz Select--");
    }
    protected void DDprocessname_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDetails();
    }
    protected void FillDetails()
    {
        lblmsg.Text = "";
        if (DDprocessname.SelectedIndex > 0)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@customerid", DDCustCode.SelectedValue);
                param[1] = new SqlParameter("@orderid", DDorderNo.SelectedValue);
                param[2] = new SqlParameter("@processid", DDprocessname.SelectedValue);

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getcustomerinspection", param);
                //*******Table o for Issue
                lbljobissuestartdate.Text = "";
                lbljobissueenddate.Text = "";
                lblnoofpcsissueasondate.Text = "";
                lblissuepcs.Text = "";

                lbljobissuestartdate.Text = ds.Tables[0].Rows[0]["startdate"].ToString();
                lbljobissueenddate.Text = ds.Tables[0].Rows[0]["Enddate"].ToString();
                lblissuepcs.Text = ds.Tables[0].Rows[0]["QTY"].ToString();
                //*******END
                //***Table 1 For Receive Details
                lbljobrecstartdate.Text = "";
                lbljobrecEnddate.Text = "";
                lblRecpcs.Text = "";
                lblRecpcsasondate.Text = "";

                lbljobrecstartdate.Text = ds.Tables[1].Rows[0]["startdate"].ToString();
                lbljobrecEnddate.Text = ds.Tables[1].Rows[0]["Enddate"].ToString();
                lblRecpcs.Text = ds.Tables[1].Rows[0]["QTY"].ToString();
                //*******

            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;

            }

        }
    }
    protected void txtissueasondate_TextChanged(object sender, EventArgs e)
    {
        GetNoofpcsasondate(Datatype: "0");
    }
    protected void GetNoofpcsasondate(string Datatype = "0")
    {

        lblmsg.Text = "";

        try
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@customerid", DDCustCode.SelectedValue);
            param[1] = new SqlParameter("@orderid", DDorderNo.SelectedValue);
            param[2] = new SqlParameter("@processid", DDprocessname.SelectedValue);
            param[3] = new SqlParameter("@AsonDate", (Datatype == "0" ? txtissueasondate.Text : txtrecasondate.Text));
            param[4] = new SqlParameter("@DataType", Datatype);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getcustomerinspectionPcsasonDate", param);
            if (Datatype == "0")
            {
                lblnoofpcsissueasondate.Text = ds.Tables[0].Rows[0]["QTY"].ToString(); ;
            }
            if (Datatype == "1")
            {
                lblRecpcsasondate.Text = ds.Tables[0].Rows[0]["QTY"].ToString(); ;
            }


        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;

        }
    }
    protected void txtrecasondate_TextChanged(object sender, EventArgs e)
    {
        GetNoofpcsasondate(Datatype: "1");
    }
    protected void DDorderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDetails();
    }
}