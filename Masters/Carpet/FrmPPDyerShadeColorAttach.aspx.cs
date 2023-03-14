using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using ClosedXML.Excel;

public partial class Masters_Carpet_FrmPPDyerShadeColorAttach : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            logo();

            string Str = @"Select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA 
            Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + @" order by CompanyName
            Select PROCESS_NAME_ID, PROCESS_NAME from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"];
            if (Convert.ToInt16(Session["varcompanyId"]) == 16)
            {
                Str = Str + " and Process_name_id in (5, 143)";
            }
            else if (variable.Carpetcompany == "1")
            {
                Str = Str + " and Process_name_id=5";
            }
            Str = Str + " Order By PROCESS_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref ddcompany, ds, 0, false, "");

            if (ddcompany.Items.Count > 0)
            {
                ddcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddprocess, ds, 1, true, "--Plz Select--");
            if (ddprocess.Items.Count > 0)
            {
                ddprocess.SelectedValue = "5";
                ProcessSelectedIndexChanged();
            }

            if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
            {
                Tdcustcode.Visible = false;
            }
        }
    }
    protected void ddcompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedIndexChange();
    }
    private void CompanySelectedIndexChange()
    {
        if (ddcompany.SelectedIndex > 0)
        {            
            ProcessSelectedIndexChanged();
        }
    }

    protected void ddprocess_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessSelectedIndexChanged();
    }
    private void ProcessSelectedIndexChanged()
    {
        UtilityModule.ConditionalComboFill(ref ddcustomer, @"Select Distinct CI.CustomerID, CI.Customercode + SPACE(5) + CI.CompanyName 
                    From ProcessProgram PP
                    JOIN OrderMaster OM ON OM.OrderID = PP.Order_ID And OM.Status = 0 And OM.CompanyID = " + ddcompany.SelectedValue + @" 
                    JOIN CustomerInfo CI ON CI.CustomerID = OM.CustomerID 
                    Where PP.Process_ID = " + ddprocess.SelectedValue + " Order By CI.Customercode + SPACE(5) + CI.CompanyName", true, "--Select--");
    }
    protected void ddcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerSelectedChanged();
    }
    protected void CustomerSelectedChanged()
    {
        string str1 = @"select Distinct P.PPID, cast(P.ChallanNo as varchar) + ' # ' +Orderno.OrderNo 
                    From ProcessProgram  P 
                    inner join OrderMaster om on p.Order_ID=OM.OrderId
                    INNER JOIN customerinfo CI ON OM.CustomerId=CI.CustomerId
                    cross apply (select OrderNo From F_GetPPNo_OrderNo(P.PPID)) Orderno
                    Where P.Process_id=" + ddprocess.SelectedValue + " and OM.CompanyiD=" + ddcompany.SelectedValue;

        if (Tdcustcode.Visible == true)
        {
            str1 = str1 + " and OM.CustomerId=" + ddcustomer.SelectedValue + "";
        }
        str1 = str1 + " Order By P.PPID Desc ";

        UtilityModule.ConditionalComboFill(ref ddprocessprogram, str1, true, "--Select--");
    }

    protected void ddprocessprogram_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessProgramSelectedIndexChanged();
    }
    protected void ProcessProgramSelectedIndexChanged()
    {
        string str1 = @"Select EI.EmpID, EI.EmpName  
            From ProcessProgramWithEmp PPE(Nolock) 
            JOIN Empinfo EI ON EI.EmpID = PPE.EmpID 
            Where PPE.PPID = " + ddprocessprogram.SelectedValue;

        UtilityModule.ConditionalComboFill(ref DDDyerName, str1, true, "--Select--");

        string str = @"Select Distinct PPC.FinishedId, Replace(VF.ITEM_NAME + ', ' + VF.QualityName + ', ' + VF.DesignName + ', ' + VF.ColorName + ', ' + VF.ShapeName + ', ' + VF.SizeFt + ', ' + VF.ShadeColorName, ', , ', '') Description
            From PP_Consumption PPC(Nolock) 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = PPC.FinishedId 
            Where PPID = " + ddprocessprogram.SelectedValue + @" 
            Order By Replace(VF.ITEM_NAME + ', ' + VF.QualityName + ', ' + VF.DesignName + ', ' + VF.ColorName + ', ' + VF.ShapeName + ', ' + VF.SizeFt + ', ' + VF.ShadeColorName, ', , ', '')  ";

        UtilityModule.ConditonalChkBoxListFill(ref ChkBoxListProcessItemDetail, str);
    }

    protected void DDDyerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = null;
        try
        {
            string Str = @"Select PPID, EmpID, FinishedID 
                    From ProcessProgramEmpWithItemDetail Where PPID = " + ddprocessprogram.SelectedValue + " And EmpID = " + DDDyerName.SelectedValue;

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            for (int j = 0; j < ChkBoxListProcessItemDetail.Items.Count; j++)
            {
                ChkBoxListProcessItemDetail.Items[j].Selected = false;
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    for (int j = 0; j < ChkBoxListProcessItemDetail.Items.Count; j++)
                    {
                        if (Convert.ToInt32(ChkBoxListProcessItemDetail.Items[j].Value) == Convert.ToInt32(ds.Tables[0].Rows[i]["FinishedID"]))
                        {
                            ChkBoxListProcessItemDetail.Items[j].Selected = true;
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmPPDyerShadeColorAttach.aspx");
            lblerror.Visible = true;
            lblerror.Text = ex.Message;
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        int Chkcount = 0;
        int n = 0;
        n = ChkBoxListProcessItemDetail.Items.Count;
        for (int i = 0; i < n; i++)
        {
            if (ChkBoxListProcessItemDetail.Items[i].Selected)
            {
                Chkcount += 1;
            }
        }
        if (Chkcount == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please check atleast one process item detail to save Data.')", true);
            return;
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[6];
            _arrPara[0] = new SqlParameter("@PPID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@EmpID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@UserID", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@FinishedID", SqlDbType.VarChar, 4000);

            _arrPara[0].Value = ddprocessprogram.SelectedValue;
            _arrPara[1].Value = DDDyerName.SelectedValue;
            _arrPara[2].Value = Session["varuserid"].ToString();
            _arrPara[3].Value = Session["varCompanyId"].ToString();

            string FinishedID = "";
            for (int i = 0; i < ChkBoxListProcessItemDetail.Items.Count; i++)
            {
                if (ChkBoxListProcessItemDetail.Items[i].Selected)
                {
                    if (FinishedID == "")
                    {
                        FinishedID = ChkBoxListProcessItemDetail.Items[i].Value + '|';
                    }
                    else
                    {
                        FinishedID = FinishedID + ChkBoxListProcessItemDetail.Items[i].Value + '|';
                    }
                }
            }
            _arrPara[4].Value = FinishedID;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "PRO_SavePPDyerShadeColorAttach", _arrPara);
             
            tran.Commit();
            lblerror.Text = "Save Details.................";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/SAVE/FrmPPDyerShadeColorAttach.aspx");
            tran.Rollback();
            lblerror.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }

    private void logo()
    {
        if (File.Exists(Server.MapPath("~/Images/Logo/" + Session["varCompanyId"] + "_company.gif")))
        {
            imgLogo.ImageUrl.DefaultIfEmpty();
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varCompanyId"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
        }
        if (Session["varCompanyName"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        LblCompanyName.Text = Session["varCompanyName"].ToString();
        LblUserName.Text = Session["varusername"].ToString();
    }
    protected void txtprocessprogram_TextChanged(object sender, EventArgs e)
    {
        string Str = @"Select OM.CompanyId, OM.CustomerId, PP.Process_ID ProcessID, PPID  
                From ProcessProgram PP(Nolock)
                JOIN OrderMaster OM(Nolock) ON OM.OrderId = PP.Order_ID And OM.CompanyID = " + ddcompany.SelectedValue + @" 
                Where PPID = '" + txtprocessprogram.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddcustomer.SelectedValue = ds.Tables[0].Rows[0]["CustomerId"].ToString();
            CustomerSelectedChanged();
            ddprocessprogram.SelectedValue = ds.Tables[0].Rows[0]["PPID"].ToString();
            ProcessProgramSelectedIndexChanged();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
}
