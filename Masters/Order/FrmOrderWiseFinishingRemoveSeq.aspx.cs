using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Order_FrmOrderWiseFinishingRemoveSeq : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string Str = @"Select CI.CompanyId,CompanyName 
                        From CompanyInfo CI(Nolock) 
                        JOIN Company_Authentication CA(Nolock) on CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + @" And 
                        CA.MasterCompanyid=" + Session["varCompanyId"] + @" order by CompanyName
                        Select Distinct CI.Customerid, CI.CompanyName + SPACE(5) + CI.Customercode CompanyName 
                        From OrderMaster OM(Nolock) 
                        JOIN customerinfo CI(Nolock) ON CI.CustomerId = OM.CustomerId And CI.MasterCompanyId=" + Session["varCompanyId"] + @"
                        --LEFT JOIN JobAssigns JA(Nolock) ON JA.OrderId = OM.OrderId 
                        Where --JA.OrderID is null And 
                        OM.CompanyId = " + Session["CurrentWorkingCompanyID"] + @" 
                        Order By CompanyName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref ddcompany, ds, 0, false, "");
            if (ddcompany.Items.Count > 0)
            {
                ddcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddcustomercode, ds, 1, true, "Select customer Code");
        }
    }
    protected void ddcustomercode_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillOrderNo();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            int OrderWiseFinishingIssueProcessSeq = 0;
            string Msg = "Finishing Sequence Remove successfully...";

            if (ChkAddFinishingJobSequence.Checked == true)
            {
                OrderWiseFinishingIssueProcessSeq = 1;
                Msg = "Finishing Sequence Add successfully...";
        
            }
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Update OrderMaster Set OrderWiseFinishingIssueProcessSeq = " + OrderWiseFinishingIssueProcessSeq + @" 
                    Where CompanyId = " + ddcompany.SelectedValue + " And CustomerId = " + ddcustomercode.SelectedValue + @" And
                    OrderID = " + ddOrderNo.SelectedValue + @"
                    Insert into OrderSequenceChangeDetail(OrderID, UserID, AddRemoveFlag, DateAdded)
                    Select " + ddOrderNo.SelectedValue + ", " + Session["varuserid"] + ", " + OrderWiseFinishingIssueProcessSeq + ", GetDate()");

            ScriptManager.RegisterStartupScript(Page, GetType(), "a", "alert('" + Msg + "')", true);
            ddOrderNo.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void ChkAddFinishingJobSequence_CheckedChanged(object sender, EventArgs e)
    {
        ddOrderNo.Items.Clear();
        if (ddcustomercode.Items.Count > 0 && ddcustomercode.SelectedIndex > 0)
        {
            FillOrderNo();
        }
    }
    private void FillOrderNo()
    {
        string Str = @"Select Distinct OM.OrderId, OM.CustomerOrderNo 
                From OrderMaster OM(Nolock) 
                LEFT JOIN JobAssigns JA(Nolock) ON JA.OrderId = OM.OrderId 
                Where OM.CompanyId = " + ddcompany.SelectedValue + " And OM.CustomerId = " + ddcustomercode.SelectedValue;
        if (ChkAddFinishingJobSequence.Checked == true)
        {
            Str = Str + @" And JA.OrderID is null ";
        }
        else
        {
            Str = Str + @" And OM.OrderWiseFinishingIssueProcessSeq = 1";
        }

        Str = Str + @" Order By OM.OrderID Desc";

        UtilityModule.ConditionalComboFill(ref ddOrderNo, Str, true, "--SELECT--");
    }
}