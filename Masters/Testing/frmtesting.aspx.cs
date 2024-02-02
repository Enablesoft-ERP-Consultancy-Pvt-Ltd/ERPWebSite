using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Testing_frmtesting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.USERID=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by CompanyId
                          select PROCESS_NAME_ID,PROCESS_NAME From process_Name_master Where PROCESS_NAME<>'WEAVING' and mastercompanyid=" + Session["varMasterCompanyIDForERP"] + " order by PROCESS_NAME";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");
            
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDJobname, ds, 1, true, "--Plz Select--");
            ds.Dispose();
            FillIntialData();
            ViewState["testid"] = "0";
            txttestdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str;
        switch (DDJobname.SelectedItem.Text.ToUpper())
        {
            case "PURCHASE":
                str = "select PindentIssueid,ChallanNo from PurchaseIndentIssue Where Companyid=" + DDCompanyName.SelectedValue + " and Partyid=" + DDPartyName.SelectedValue + " order by PindentIssueid desc";
                break;
            default:
                str = "select Im.Indentid,IM.IndentNo From Indentmaster IM WHere Im.companyid=" + DDCompanyName.SelectedValue + " and IM.Partyid=" + DDPartyName.SelectedValue + " and IM.Processid=" + DDJobname.SelectedValue + " order by IM.indentid desc";
                break;
        }
        UtilityModule.ConditionalComboFill(ref DDPONo, str, true, "--Plz Select--");

    }
    protected void DDPONo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str;
        switch (DDJobname.SelectedItem.Text.ToUpper())
        {
            case "PURCHASE":
                str = @"select Distinct PRM.PurchaseReceiveId,BillNo+'    '+Replace(Convert(nvarchar(11),Receivedate,106),' ','-') as ChallanNo from Purchasereceivemaster PRM 
                        inner join PurchaseReceiveDetail PRD
                        on PRM.PurchaseReceiveId=PRD.PurchaseReceiveId
                        Where PRD.PIndentIssueId=" + DDPONo.SelectedValue + " order by PRM.PurchaseReceiveId desc";
                break;
            default:
                str = @"select Distinct PRM.PRMid,PRM.ChallanNo+'/'+Replace(CONVERT(nvarchar(11),PRM.Date,106),' ','-') as ChallanNo From PP_ProcessRecMaster PRM inner join PP_ProcessRecTran PRT on PRM.PRMid=PRT.PRMid
                      Where PRT.IndentId=" + DDPONo.SelectedValue + " order by PRM.PRMid desc";
                break;
        }
        UtilityModule.ConditionalComboFill(ref ddlrecchalanno, str, true, "--Plz Select--");
        ViewState["testid"] = "0";
    }
    protected void ddlrecchalanno_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str;
        switch (DDJobname.SelectedItem.Text.ToUpper())
        {
            case "PURCHASE":
                str = "select Distinct finishedid,item_description from v_purchase_emp_order_wise where purchasereceiveid= " + ddlrecchalanno.SelectedValue + "";
                break;
            default:
                str = @"select Distinct OFinishedid,CATEGORY_NAME+' '+Item_name+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+Size+' '+SizeType as ItemDescription
                     From V_IndentReceiveDetailReport where  PRMID=" + ddlrecchalanno.SelectedValue;
                break;
        }
        UtilityModule.ConditionalComboFill(ref DDDescription, str, true, "--Plz Select--");
        ViewState["testid"] = "0";

    }
    protected void DDDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str;
        switch (DDJobname.SelectedItem.Text.ToUpper())
        {
            case "PURCHASE":
                str = "select isnull(Sum(recqty),0) as recQty,Max(flagSize) as Flagsize from v_purchase_emp_order_wise where purchasereceiveid= " + ddlrecchalanno.SelectedValue + " and finishedid=" + DDDescription.SelectedValue;
                break;
            default:
                str = @"select isnull(Sum(Qty),0) as Recqty,isnull(max(Sz.val),0) as flagsize
                      From V_IndentReceiveDetailReport V left join SizeType Sz on V.Sizetype=Sz.Type  Where prmid=" + ddlrecchalanno.SelectedValue + " and Ofinishedid=" + DDDescription.SelectedValue;
                break;
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtRecQty.Text = ds.Tables[0].Rows[0]["recQty"].ToString();
            hnflagsize.Value = ds.Tables[0].Rows[0]["flagsize"].ToString();
        }
        else
        {
            txtRecQty.Text = "";
            hnflagsize.Value = "0";
        }
        if (chkedit.Checked == true)
        {
            FillTestNo();
        }
        ViewState["testid"] = "0";
    }
    protected void FillIntialData()
    {
        string str = @"select TC.categoryId,TC.Category,TS.SubCategory,Ts.SubCategoryId,'' as Requirement,'' as Received
                        from TestCategory TC left join Testsubcategory TS 
                    on Tc.categoryId=TS.CategoryId";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GVDetail.DataSource = ds.Tables[0];
        GVDetail.DataBind();
    }

    protected void GVDetail_DataBound(object sender, EventArgs e)
    {
        string oldvalue = string.Empty;
        string Newvalue = string.Empty;
        //*****Column Loop
        for (int j = 0; j < 1; j++)
        {
            for (int count = 0; count < GVDetail.Rows.Count; count++)
            {
                oldvalue = GVDetail.Rows[count].Cells[j].Text;
                if (oldvalue == Newvalue)
                {
                    GVDetail.Rows[count].Cells[j].Text = string.Empty;
                }
                Newvalue = oldvalue;
            }
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //For Sql table Type
            DataTable dttestdetail = new DataTable();
            dttestdetail.Columns.Add("CategoryId", typeof(int));
            dttestdetail.Columns.Add("SubCategoryId", typeof(int));
            dttestdetail.Columns.Add("Requirement", typeof(string));
            dttestdetail.Columns.Add("Received", typeof(string));
            //***********Loop
            for (int i = 0; i < GVDetail.Rows.Count; i++)
            {

                Label lblCategoryId = ((Label)GVDetail.Rows[i].FindControl("lblcategoryid"));
                Label lblSubCategoryId = ((Label)GVDetail.Rows[i].FindControl("lblsubcategoryid"));
                TextBox txtrequirement = ((TextBox)GVDetail.Rows[i].FindControl("txtrequirement"));
                TextBox txtreceived = ((TextBox)GVDetail.Rows[i].FindControl("txtreceived"));

                if (txtrequirement.Text != "" || txtreceived.Text != "")
                {
                    DataRow drtest = dttestdetail.NewRow();
                    drtest["CategoryId"] = lblCategoryId.Text;
                    drtest["SubCategoryId"] = lblSubCategoryId.Text == "" ? "0" : lblSubCategoryId.Text;
                    drtest["Requirement"] = txtrequirement.Text;
                    drtest["Received"] = txtreceived.Text;
                    dttestdetail.Rows.Add(drtest);
                }

            }
            //*******Procedure
            SqlParameter[] param = new SqlParameter[17];
            param[0] = new SqlParameter("@testid", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = ViewState["testid"];
            param[1] = new SqlParameter("@companyid", DDCompanyName.SelectedValue);
            param[2] = new SqlParameter("@Partyid", DDPartyName.SelectedValue);
            param[3] = new SqlParameter("@Pindentissueid", DDPONo.SelectedValue);
            param[4] = new SqlParameter("@Purchasereciveid", ddlrecchalanno.SelectedValue);
            param[5] = new SqlParameter("@finishedid", DDDescription.SelectedValue);
            param[6] = new SqlParameter("@flagsize", hnflagsize.Value);
            param[7] = new SqlParameter("@recQty", txtRecQty.Text);
            param[8] = new SqlParameter("@testdate", txttestdate.Text);
            param[9] = new SqlParameter("@remark", txtremark.Text);
            param[10] = new SqlParameter("@testdetail", dttestdetail);
            param[11] = new SqlParameter("@userid", Session["varuserid"]);
            param[12] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[12].Direction = ParameterDirection.Output;
            param[13] = new SqlParameter("@testresult", DDtestresult.Text);
            param[14] = new SqlParameter("@Passby", txtpassby.Text);
            param[15] = new SqlParameter("@Processid", DDJobname.SelectedValue);
            //**************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveTesting", param);
            Tran.Commit();
            ViewState["testid"] = param[0].Value.ToString();
            lblmessage.Text = "Data saved successfully....";

        }
        catch (Exception ex)
        {

            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        if (chkedit.Checked == true)
        {
            TDTestNo.Visible = true;
        }
        else
        {
            TDTestNo.Visible = false;
        }
    }
    protected void FillTestNo()
    {
        string str = @"select testid,cast(testid as nvarchar)+'  '+Replace(CONVERT(nvarchar(11),testdate,106),' ','-') as testno from Testmaster Where 1=1";
        if (DDCompanyName.SelectedIndex != -1)
        {
            str = str + " and companyId=" + DDCompanyName.SelectedValue;
        }
        if (DDPartyName.SelectedIndex > 0)
        {
            str = str + " and Partyid=" + DDPartyName.SelectedValue;
        }
        if (DDPONo.SelectedIndex > 0)
        {
            str = str + " and Pindentissueid=" + DDPONo.SelectedValue;
        }
        if (ddlrecchalanno.SelectedIndex > 0)
        {
            str = str + " and Purchasereceiveid=" + ddlrecchalanno.SelectedValue;
        }
        if (DDDescription.SelectedIndex > 0)
        {
            str = str + " and finishedid=" + DDDescription.SelectedValue;
        }
        if (DDJobname.SelectedIndex>0)
        {
            str = str + " and processid=" + DDJobname.SelectedValue;
        }
        str = str + "  order by testid";
        UtilityModule.ConditionalComboFill(ref DDtestNo, str, true, "--Plz Select");
    }
    protected void Fillgrid()
    {
        string str = @"select Distinct TC.categoryId,TC.Category,TS.SubCategory,Ts.SubCategoryId,TD.Requirement,
                    Td.Received as Received
                    from  TestCategory TC left join  Testsubcategory TS on Tc.categoryId=Ts.CategoryId
                    left join TestDetail TD on TD.categoryid=TC.categoryId and Td.Subcategoryid=ts.SubCategoryId                    
                    And TD.testid=" + ViewState["testid"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        GVDetail.DataSource = ds.Tables[0];
        GVDetail.DataBind();
    }
    protected void DDtestNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["testid"] = DDtestNo.SelectedValue;
        string str = "select RecQty,remark,replace(convert(nvarchar(11),testdate,106),' ','-') as testdate,isnull(testresult,'') as testresult,Passby from testmaster TM where testid=" + ViewState["testid"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtRecQty.Text = ds.Tables[0].Rows[0]["Recqty"].ToString();
            txttestdate.Text = ds.Tables[0].Rows[0]["testdate"].ToString();
            txtremark.Text = ds.Tables[0].Rows[0]["remark"].ToString();
            DDtestresult.SelectedValue = ds.Tables[0].Rows[0]["testresult"].ToString();
            txtpassby.Text = ds.Tables[0].Rows[0]["passby"].ToString();

        }
        Fillgrid();
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = "select * from V_testingReport where testid=" + ViewState["testid"];

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["dsFileName"] = "~\\ReportSchema\\RptTestingDetail.xsd";
            Session["ReportPath"] = "Reports/RptTestingDetail.rpt";

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

    protected void DDJobname_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str;
        switch (DDJobname.SelectedItem.Text.ToUpper())
        {
            case "PURCHASE":
                str = @"select Distinct  EI.EmpId,EI.EmpName From PurchaseIndentIssue PIM inner Join EmpInfo EI on PIM.Partyid=EI.EmpId
                       and PIM.Companyid=" + DDCompanyName.SelectedValue + " order by EI.EmpName";
                break;
            default:
                str = @"Select Distinct EI.EmpId,Ei.EmpName From Indentmaster IM inner join EmpInfo EI on IM.PartyId=Ei.EmpId 
                       and IM.CompanyId=" + DDCompanyName.SelectedValue + " and IM.ProcessID=" + DDJobname.SelectedValue + "  order by Ei.EmpName";
                break;
        }
        UtilityModule.ConditionalComboFill(ref DDPartyName, str, true, "--Plz Select--");
    }
}