using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Process_GenrateIndentApproval : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TxtApprovalDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            CommanFunction.FillCombo(DDCompanyName, "select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.USERID=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + " Order by Companyname");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFill(ref DDprocess, "select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where ProcessType=0 And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Process--");
            //UtilityModule.ConditionalComboFill(ref DDPartyName, "Select Distinct EI.EmpId,EmpName from IndentMaster IM inner join EmpInfo EI ON IM.PartyId=EI.EmpId inner join EmpProcess EP on EI.EmpId=EP.EmpId Where IM.Companyid=" + DDCompanyName.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
        }
    }

    protected void DDprocess_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDPartyName, "Select Distinct EI.EmpId,EmpName from IndentMaster IM inner join EmpInfo EI ON IM.PartyId=EI.EmpId inner join EmpProcess EP on EI.EmpId=EP.EmpId Where ep.processid=" + DDprocess.SelectedValue + " and  IM.Companyid=" + DDCompanyName.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDProcessProgramNo, "Select Distinct PPNo,PPNo from IndentMaster IM,IndentDetail ID where IM.IndentId=ID.IndentId And IM.CompanyId=" + DDCompanyName.SelectedValue + " And IM.ProcessId=" + DDprocess.SelectedValue + " And IM.PartyID=" + DDPartyName.SelectedValue, true, "--Select--");
    }
    protected void DDProcessProgramNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditonalChkBoxListFill(ref CHkPindentNo, "Select Distinct IM.IndentId,IndentNo from IndentMaster IM inner join IndentDetail ID on IM.IndentId=ID.IndentId Where im.Approvalflag=0 and IM.CompanyId=" + DDCompanyName.SelectedValue + " And IM.Status <> 'Cancelled' And ID.PPNo=" + DDProcessProgramNo.SelectedValue + " And IM.partyId=" + DDPartyName.SelectedValue + " And IM.CompanyId=" + DDCompanyName.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + "");
    }
    protected void CHkPindentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["IndentNo"] = null;
        ViewState["IndentNo"] = CHkPindentNo.SelectedValue;
        foreach (ListItem li in CHkPindentNo.Items)
        {
            if (li.Selected == true)
            {
                if (li.Value == ViewState["IndentNo"])
                    ViewState["IndentNo"] = li.Value;
                else
                    li.Selected = false;
            }
            else
                li.Selected = false;
        }
        ViewState["IndentNo"] = ViewState["IndentNo"] != null ? ViewState["IndentNo"] : 0;
        fill_grid(ViewState["IndentNo"].ToString());
    }
    private void fill_grid(string PindentNo)
    {
        DGIndentDetail.DataSource = Fill_Grid_Data(PindentNo);
        DGIndentDetail.DataBind();
    }
    private DataSet Fill_Grid_Data(string PindentNo)
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"Select IND.IndentDetailId,PPNo,IndentNo,Quantity,Rate,VF1.CATEGORY_NAME+space(3)+VF1.ITEM_NAME+space(3)+VF1.QualityName+ Space(3)+VF1.designName+ 
                            Space(3)+VF1.ColorName+ Space(3)+VF1.ShadeColorName+ Space(3)+VF1.ShapeName+ Space(3)+VF1.SizeMtr InDescription,VF.CATEGORY_NAME+space(3)+VF.ITEM_NAME+space(3)+
                            VF.QualityName+ Space(3)+VF.designName+ Space(3)+VF.ColorName+ Space(3)+VF.ShadeColorName+ Space(3)+VF.ShapeName+ Space(3)+VF.SizeMtr OutDescription 
                            From IndentMaster INM
                            inner join IndentDetail IND on INM.indentid=IND.IndentId
                            inner join V_FinishedItemDetail VF on vf.ITEM_FINISHED_ID=ind.OFinishedId
                            left join V_FinishedItemDetail VF1 on vf1.ITEM_FINISHED_ID=ind.IFinishedId
                            Where IND.IndentId in (select * from Split('" + PindentNo + "',',')) And INM.MasterCompanyId=" + Session["varCompanyId"];

            //            string strsql = @"Select IND.IndentDetailId,PPNo,IndentNo,Quantity,Rate,VF1.CATEGORY_NAME+space(3)+VF1.ITEM_NAME+space(3)+VF1.QualityName+ Space(3)+VF1.designName+ 
            //            Space(3)+VF1.ColorName+ Space(3)+VF1.ShadeColorName+ Space(3)+VF1.ShapeName+ Space(3)+VF1.SizeMtr InDescription,VF.CATEGORY_NAME+space(3)+VF.ITEM_NAME+space(3)+
            //            VF.QualityName+ Space(3)+VF.designName+ Space(3)+VF.ColorName+ Space(3)+VF.ShadeColorName+ Space(3)+VF.ShapeName+ Space(3)+VF1.SizeMtr OutDescription 
            //            From IndentMaster INM,IndentDetail IND,V_FinishedItemDetail VF,V_FinishedItemDetail VF1
            //            Where IND.IndentId=INM.IndentId And IND.OFinishedId=VF.ITEM_FINISHED_ID And IND.IFinishedId=VF1.ITEM_FINISHED_ID And IND.IndentId in (select * from Split('" + PindentNo + "',',')) And INM.MasterCompanyId=" + Session["varCompanyId"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchaseApproval.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        return ds;
    }
    protected void DGPIndentDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGIndentDetail.PageIndex = e.NewPageIndex;
        fill_grid(ViewState["IndentNo"].ToString());
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        if (ViewState["IndentNo"] != null)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                con.Open();
                SqlParameter[] _arrPara = new SqlParameter[9];
                _arrPara[0] = new SqlParameter("@GIndentid", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@ApprovalDate", SqlDbType.DateTime);
                _arrPara[2] = new SqlParameter("@userId", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@ApprovedBy", SqlDbType.NVarChar, 200);
                _arrPara[5] = new SqlParameter("@Qty", SqlDbType.Float);
                _arrPara[6] = new SqlParameter("@Pindentdetailid", SqlDbType.Int);
                _arrPara[0].Direction = ParameterDirection.InputOutput;
                _arrPara[0].Value = 0;
                _arrPara[1].Value = TxtApprovalDate.Text;
                _arrPara[2].Value = Session["varuserid"].ToString();
                _arrPara[3].Value = Session["varCompanyId"].ToString();
                _arrPara[4].Value = TxtApprovedBy.Text;
                for (int i = 0; i < DGIndentDetail.Rows.Count; i++)
                {
                    _arrPara[5].Value = ((TextBox)DGIndentDetail.Rows[i].FindControl("TxtQty")).Text;
                    _arrPara[6].Value = DGIndentDetail.DataKeys[i].Value.ToString();
                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_GIndentApproval", _arrPara);
                }
                LblErr.Text = "Data saved..............";
                UtilityModule.ConditonalChkBoxListFill(ref CHkPindentNo, "Select Distinct IM.IndentId,IndentNo from IndentMaster IM inner join IndentDetail ID on IM.IndentId=ID.IndentId Where im.Approvalflag=0 and IM.CompanyId=" + DDCompanyName.SelectedValue + " And IM.Status <> 'Cancelled' And ID.PPNo=" + DDProcessProgramNo.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + "");
                BtnPreview.Enabled = true;
                Session["ReportPath"] = "Reports/GenrateIndentApproval.rpt";
                Session["CommanFormula"] = "{GenrateIndentReport.indentid} =" + _arrPara[0].Value + "";
                ViewState["PIApprovalId"] = _arrPara[0].Value;
                fill_grid("0");
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Purchase/GenrateApproval.aspx");
                LblErr.Text = ex.Message;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        Showreport();
        //StringBuilder stb = new StringBuilder();
        //stb.Append("<script>");
        //stb.Append("window.open('../../ReportViewer.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
        //ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
    protected void Showreport()
    {
        string str = "select * from [GenrateIndentReport] where indentid=" + ViewState["PIApprovalId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Update reportcount status
            str = "update indentmaster set reportcount=isnull(reportcount,0)+1 Where indentid=" + ViewState["PIApprovalId"];
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            //end
            Session["rptFileName"] = "~\\Reports\\rptgenerateindentapprovalnew.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptgenerateindentapprovalnew.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }
    }
}
