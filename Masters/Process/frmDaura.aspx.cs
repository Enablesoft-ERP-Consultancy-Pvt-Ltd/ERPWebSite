using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;

public partial class Masters_Process_frmDaura : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from CompanyInfo CI,Company_Authentication CA where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserid"] + @" Order by CI.CompanyName
                          select CustomerId,CustomerCode from Customerinfo";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--Plz Select Company--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCustomercode, ds, 1, true, "--Plz Select Customer Code--");
            // DDCompanyName_SelectedIndexChanged(sender, e);
            txtDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

        }

    }
    protected void fillLoomSummarygrid()
    {
        string str = @"select Detailid As LoomDetailid,PONo,EmpName+'/'+Address As Weaver,QualityName,DesignName,Size,Sum(Qty) As Qty,Sum(RecQty) As RecQty,Sum(Qty-RecQty) As BalQty 
                     ,Loom,LM.IssueOrderId,LM.QualityId,LM.DesignId,LM.SizeId from LoomDetail LM,V_ForLoomDetail  v
                     Where LM.IssueOrderid=V.issueOrderId And LM.Qualityid=V.QualityId And LM.Designid=V.Designid
                     And LM.Sizeid=V.SizeId And v.CompanyId=" + DDCompanyName.SelectedValue;

        if (DDWeaver.SelectedIndex > 0)
        {
            str = str + " And v.EmpId=" + DDWeaver.SelectedValue;
        }
        if (DDPoNo.SelectedIndex > 0)
        {
            str = str + " And v.IssueOrderId=" + DDPoNo.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " And LM.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " And LM.DesignId=" + DDDesign.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " And LM.SizeId=" + DDSize.SelectedValue;
        }
        str = str + " group by  Detailid,PONo,EmpName,Address,QualityName,DesignName,Size,LM.QualityId,LM.DesignId,LM.SizeId,Loom,LM.IssueOrderId having Sum(Qty-RecQty)>0";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gdLoomDetail.DataSource = ds;
            gdLoomDetail.DataBind();
        }
        else
        {
            gdLoomDetail.DataSource = null;
            gdLoomDetail.DataBind();
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record found...');", true);
        }

    }
    protected void DDCustomercode_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDOrderNo, "select OrderId,LocalOrder+'/'+CustomerOrderNo As OrderNo from OrderMaster Where CompanyId=" + DDCompanyName.SelectedValue + " And CustomerId=" + DDCustomercode.SelectedValue + "", true, "--Plz Select Order No--");
    }
    protected void DDWeaver_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select PM.issueOrderId,cast(PM.IssueOrderId as nvarchar)+'/'+Replace(convert(nvarchar(11),Assigndate,106),' ','-')  PONo from PROCESS_ISSUE_MASTER_1 PM,PROCESS_ISSUE_DETAIL_1 PD
                     Where PM.IssueOrderId=PD.IssueOrderId And PM.CompanyID = " + DDCompanyName.SelectedValue + " And Empid=" + DDWeaver.SelectedValue;

        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " and PD.OrderId=" + DDOrderNo.SelectedValue;
        }
        UtilityModule.ConditionalComboFill(ref DDPoNo, str, true, "--Select Po No--");
    }
    protected void btnShowDetail_Click(object sender, EventArgs e)
    {
        fillLoomSummarygrid();
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDWeaver, "select Distinct EI.EmpId,EI.EmpName from Process_Issue_Master_1 PM,Empinfo EI Where PM.EmpId=EI.EmpId ", true, "--Select Weaver--");
        string str = @"select QualityId,QualityName from Quality Where MasterCompanyId=" + Session["varcompanyId"] + @"
                     select DesignId,DesignName from Design Where MasterCompanyid=" + Session["varcompanyId"] + @"
                     select SizeId,SizeMtr from Size where MasterCompanyid=" + Session["varcompanyid"] + "";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        UtilityModule.ConditionalComboFillWithDS(ref DDQuality, ds, 0, true, "--select Quality--");
        UtilityModule.ConditionalComboFillWithDS(ref DDDesign, ds, 1, true, "--select Design--");
        UtilityModule.ConditionalComboFillWithDS(ref DDSize, ds, 2, true, "--select Size--");
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDWeaver, "select Distinct EI.EmpId,EI.EmpName from Process_Issue_Master_1 PM,Process_Issue_Detail_1 PD,Empinfo EI Where PM.IssueOrderId=PD.IssueOrderId and PM.EmpId=EI.EmpId And PD.OrderId=" + DDOrderNo.SelectedValue + "", true, "--Select Weaver--");
    }
    protected void DDPoNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select VF.designId,vf.designName from PROCESS_ISSUE_DETAIL_1 PD,V_FinishedItemDetail vf  Where
                      PD.IssueOrderId=" + DDPoNo.SelectedValue + @" And vf.ITEM_FINISHED_ID=PD.Item_Finished_Id
                    select vf.QualityId,vf.QualityName from PROCESS_ISSUE_DETAIL_1 PD,V_FinishedItemDetail vf  Where
                    PD.IssueOrderId=" + DDPoNo.SelectedValue + @" And vf.ITEM_FINISHED_ID=PD.Item_Finished_Id
                    select vf.SizeId,vf.SizeMtr from PROCESS_ISSUE_DETAIL_1 PD,V_FinishedItemDetail vf  Where
                    PD.IssueOrderId=" + DDPoNo.SelectedValue + @" And vf.ITEM_FINISHED_ID=PD.Item_Finished_Id";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        UtilityModule.ConditionalComboFillWithDS(ref DDDesign, ds, 0, true, "--Select Design--");
        UtilityModule.ConditionalComboFillWithDS(ref DDQuality, ds, 1, true, "--Select Design--");
        UtilityModule.ConditionalComboFillWithDS(ref DDSize, ds, 2, true, "--Select Design--");


    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            lblMessage.Text = "";
            SqlParameter[] array = new SqlParameter[7];
            array[0] = new SqlParameter("@Id", SqlDbType.Int);
            array[1] = new SqlParameter("@LoomDetailId", SqlDbType.Int);
            array[2] = new SqlParameter("@Date", SqlDbType.SmallDateTime);
            array[3] = new SqlParameter("@Remark", SqlDbType.VarChar, 8000);
            array[4] = new SqlParameter("@RunningLoom", SqlDbType.Int);
            array[5] = new SqlParameter("@UserId", SqlDbType.Int);
            array[6] = new SqlParameter("@MasterCompanyid", SqlDbType.Int);

            for (int i = 0; i < gdLoomDetail.Rows.Count; i++)
            {
                if (((CheckBox)gdLoomDetail.Rows[i].FindControl("chkitem")).Checked == true)
                {
                    TextBox txtrunningLoom = ((TextBox)gdLoomDetail.Rows[i].FindControl("txtRunningLoom"));
                    array[0].Direction = ParameterDirection.InputOutput;
                    array[0].Value = ViewState["ID"];
                    array[1].Value = ((Label)gdLoomDetail.Rows[i].FindControl("lblLoomDetailId")).Text;
                    array[2].Value = txtDate.Text;
                    array[3].Value = ((TextBox)gdLoomDetail.Rows[i].FindControl("txtremark")).Text;
                    array[4].Value = txtrunningLoom.Text == "" ? "0" : txtrunningLoom.Text;
                    array[5].Value = Session["varuserId"];
                    array[6].Value = Session["varcompanyId"];

                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_Daura", array);
                    ViewState["ID"] = array[0].Value.ToString();
                }
            }
            Tran.Commit();
            lblMessage.Text = "Data Saved Successfully.....";
            ViewState["ID"] = "0";

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string str = "";
        string QualityId = "";
        string DesignId = "";
        string SizeId = "";
        for (int i = 0; i < gdLoomDetail.Rows.Count; i++)
        {
            if (((CheckBox)gdLoomDetail.Rows[i].FindControl("ChkItem")).Checked == true)
            {
                str = str == "" ? ((Label)gdLoomDetail.Rows[i].FindControl("lblLoomDetailId")).Text : str + "," + ((Label)gdLoomDetail.Rows[i].FindControl("lblLoomDetailId")).Text;
                QualityId = QualityId == "" ? ((Label)gdLoomDetail.Rows[i].FindControl("lblQualityId")).Text : QualityId + "," + ((Label)gdLoomDetail.Rows[i].FindControl("lblQualityId")).Text;
                DesignId = DesignId == "" ? ((Label)gdLoomDetail.Rows[i].FindControl("lblDesignId")).Text : DesignId + "," + ((Label)gdLoomDetail.Rows[i].FindControl("lblDesignId")).Text;
                SizeId = SizeId == "" ? ((Label)gdLoomDetail.Rows[i].FindControl("lblSizeId")).Text : SizeId + "," + ((Label)gdLoomDetail.Rows[i].FindControl("lblSizeId")).Text;
            }
        }
        if (str != "")
        {
            string str1 = @"select Distinct  CI.CompanyName,CompAddr1+' '+CompAddr2+' '+CompAddr3+' '+CompTel As Address,
                      EI.EmpName,Q.QualityName,D.DesignName,case When LD.UnitId=1 then Sizemtr Else case When LD.UnitId=2 Then Sizeft Else Case When LD.UnitId=6 Then Sizeinch Else SizeMtr End End End As Size,LD.IssueOrderId as PONo 
                      ,DM.Date As Date,isnull(DM.Remark,'') As Remark,isnull(DM.RunningLoom,0) As RunnigLoom,LD.Qty,dbo.F_GetLoomRecQty(LD.IssueOrderid,LD.QualityId,LD.DesignId,LD.SizeId) RecQty,LD.Loom from  Quality Q,Design D,Size S,
                      CompanyInfo CI,Empinfo EI,Process_issue_Master_1 PM,LoomDetail LD left Outer  Join DauraMaster DM
                      on LD.DetailId=DM.LoomDetailId Where Q.QualityId=LD.QualityId And D.DesignId=LD.DesignId And S.SizeId=LD.SizeId
                      And PM.IssueOrderId=LD.IssueOrderId And EI.Empid=PM.Empid And CI.CompanyId=PM.CompanyId And 
                      LD.Detailid in(" + str + ") And LD.QualityId in(" + QualityId + ") And LD.DesignId in(" + DesignId + ") And LD.SizeId in(" + SizeId + ")  ";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptDaura1.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptDaura.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No record found....');", true);
            }
        }
    }
}