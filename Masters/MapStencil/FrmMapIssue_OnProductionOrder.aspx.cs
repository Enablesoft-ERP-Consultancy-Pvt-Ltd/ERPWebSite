using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_MapStencil_FrmMapIssue_OnProductionOrder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                           select UnitsId,UnitName from Units order by UnitName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 1, true, "--Plz Select--");

            if (DDProdunit.Items.Count > 0)
            {
                DDProdunit.SelectedIndex = 1;
                DDProdunit_SelectedIndexChanged(sender, new EventArgs());
            }
            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

           
            switch (Session["varcompanyId"].ToString())
            {
                case "30":
                    TDProductionUnit.Visible = false;
                    TDLoomNo.Visible = false;
                    TDEmployee.Visible = false;
                    TDWeaverName.Visible = false;
                    TDFolioEmployee.Visible = true;
                    //FillFolioEmployee();
                    FillFolioEmployee(sender);
                    break;
                case "38":
                    TDProductionUnit.Visible = false;
                    TDLoomNo.Visible = false;
                    TDEmployee.Visible = false;
                    TDWeaverName.Visible = false;
                    TDFolioEmployee.Visible = true;
                    //FillFolioEmployee();
                    FillFolioEmployee(sender);
                    break;
                default:
                    TDProductionUnit.Visible = true;
                    TDLoomNo.Visible = true;
                    TDEmployee.Visible = true;
                    TDWeaverName.Visible = true;
                    TDFolioEmployee.Visible = false;                   
                    break;
            }

            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
            }

            hnissueid.Value = "0";
            hnissueorderid.Value = "0";
        }
    }

    protected void DDProdunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        //*********************

        DG.DataSource = null;
        DG.DataBind();

        gvdetail.DataSource = null;
        gvdetail.DataBind();

        listWeaverName.Items.Clear();
        hnissueorderid.Value = "0";
        hnissueid.Value = "0";
        // enablecontrols();
        //*********************
        //  AutoCompleteExtenderloomno.ContextKey = "0#" + DDcompany.SelectedValue + "#" + DDProdunit.SelectedValue;


        string str = @"select Distinct PLM.UID,PLM.LoomNo+'/'+isnull(IM.ITEM_NAME,'') as LoomNo,case when ISNUMERIC(loomno)=1 Then CONVERT(int,loomno) Else 9999999 End as Loom from Process_issue_master_1 PIM inner join ProductionLoomMaster PLM on PIM.LoomId=PLM.UID
                              Left join ITEM_MASTER Im on PLm.Itemid=IM.ITEM_ID 
                              left join Employee_ProcessOrderNo EMP on PIM.IssueOrderId=EMP.IssueOrderId and EMp.ProcessId=1
                              Where Plm.CompanyId=" + DDcompany.SelectedValue + " and PLm.UnitId=" + DDProdunit.SelectedValue + "";

        str = str + " and PIM.status='Pending'";

        //if (txteditempid.Text != "")
        //{
        //    str = str + " and EMP.EMPID=" + txteditempid.Text + "";
        //}
        if (txtfolionoedit.Text != "")
        {
            str = str + " and PIM.ChallanNo='" + txtfolionoedit.Text + "'";
            //str = str + " and PIM.issueorderid=" + txtfolionoedit.Text + "";
        }
        str = str + " order by Loom,loomno";

        UtilityModule.ConditionalComboFill(ref DDLoomNo, str, true, "--Plz Select--");
        if (DDLoomNo.Items.Count > 0)
        {
            DDLoomNo.SelectedIndex = 1;
            DDLoomNo_SelectedIndexChanged(sender, new EventArgs());
        }

    }
    protected void FillGrid()
    {
        //Employeedetail
        string str = @" select Distinct Ei.Empid,EI.EmpCode+'-'+EI.EmpName as Empname,activestatus from Employee_ProcessOrderNo EMP 
                    inner Join EmpInfo EI on EMP.Empid=EI.EmpId 
                    Where Emp.ProcessId=1 and Emp.IssueOrderId=" + hnissueorderid.Value;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //Employee
        if (ds.Tables[0].Rows.Count > 0)
        {
            listWeaverName.Items.Clear();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                listWeaverName.Items.Add(new ListItem(ds.Tables[0].Rows[i]["Empname"].ToString(), ds.Tables[0].Rows[i]["Empid"].ToString()));
                if (ds.Tables[0].Rows[i]["activestatus"].ToString() == "0")
                {
                    listWeaverName.Items[i].Attributes.Add("style", "background-color:red;");
                }
                else
                {
                    listWeaverName.Items[i].Attributes.Add("style", "background-color:white;");
                }
            }
        }

    }
    protected void FillIssueNo()
    {
        string str = @"select IssueId,ChallanNo from Map_IssueOnProductionOrderMaster MIM
                       Where MIM.CompanyId=" + DDcompany.SelectedValue + " and MIM.IssueOrderId=" + DDFolioNo.SelectedValue + @" 
                        and  MIM.MapStencilType=" + DDMapStencilType.SelectedValue + @" ";

        UtilityModule.ConditionalComboFill(ref DDIssueNo, str, true, "--Plz Select--");

    }
    protected void DDIssueNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnissueid.Value = DDIssueNo.SelectedValue;
        FillissueGrid();
    }
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
        if (chkEdit.Checked == true)
        {
            TDIssueNo.Visible = true;
        }
        else
        {
            TDIssueNo.Visible = false;
            DDIssueNo.Items.Clear();
        }
        txtIssueNo.Text = "";
        hnissueid.Value = "0";

        DG.DataSource = null;
        DG.DataBind();

        gvdetail.DataSource = null;
        gvdetail.DataBind();
    }
    protected void DDFolioEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            if (DDFolioEmployee.SelectedIndex > 0)
            {
//                str = @"select EI.Empid,Pm.IssueOrderId,PM.ChallanNo  from dbo.PROCESS_ISSUE_MASTER_1 PM inner join dbo.PROCESS_ISSUE_DETAIL_1 PD
//                        on PM.IssueOrderId=Pd.IssueOrderId  and Pd.PQty>0                       
//                        inner join EmpInfo EI on Ei.EmpId=PM.Empid
//                        And EI.empid=" + DDFolioEmployee.SelectedValue;


//                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
//                if (ds.Tables[0].Rows.Count > 0)
//                {
//                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "AlertEmp", "alert('Folio -" + ds.Tables[0].Rows[0]["ChallanNo"] + " Already pending at this ID No..');", true);
//                    return;
//                }

                fillFolio(sender);

                ////txtWeaverIdNo.Text = "";


               // ds.Dispose();
            }
           // // txtWeaverIdNo.Focus();

        }
        catch (Exception ex)
        {
            lblmessage.Visible = true;
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void DDemployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            if (DDemployee.SelectedIndex > 0)
            {
                str = @"select EMp.Empid,Pm.IssueOrderId  from dbo.PROCESS_ISSUE_MASTER_1 PM inner join dbo.PROCESS_ISSUE_DETAIL_1 PD
                        on PM.IssueOrderId=Pd.IssueOrderId  and Pd.PQty>0
                        inner join  dbo.Employee_ProcessOrderNo EMP on EMP.IssueOrderId=PM.IssueOrderId and EMP.ProcessId=1
                        inner join EmpInfo EI on Ei.EmpId=EMP.Empid
                        And EI.empid=" + DDemployee.SelectedValue;


                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "AlertEmp", "alert('Folio -" + ds.Tables[0].Rows[0]["IssueOrderId"] + " Already pending at this ID No..');", true);
                    return;
                }

                if (listWeaverName.Items.FindByValue(DDemployee.SelectedValue) == null)
                {

                    listWeaverName.Items.Add(new ListItem(DDemployee.SelectedItem.Text, DDemployee.SelectedValue));
                }

                //txtWeaverIdNo.Text = "";


                ds.Dispose();
            }
            // txtWeaverIdNo.Focus();

        }
        catch (Exception ex)
        {
            lblmessage.Visible = true;
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void fillFolio(object sender = null)
    {
        string str;
        // txtloomid.Text = DDLoomNo.SelectedValue;

        str = @"select Distinct PIM.IssueOrderId,PIM.ChallanNo from Process_issue_master_1 PIM
                   left join Employee_ProcessOrderNo EMP on PIM.IssueOrderId=EMP.IssueOrderId and EMp.ProcessId=1
                   Where PIM.CompanyId=" + DDcompany.SelectedValue + " and PIM.EmpId=" + DDFolioEmployee.SelectedValue + "";
        //if (chkcomplete.Checked == true)
        //{
        //    str = str + " and PIM.Status='Complete'";
        //}
        //else
        //{
        str = str + " and PIM.Status='Pending'";
        //}
        //if (txteditempid.Text != "")
        //{
        //    str = str + " and EMP.EMPID=" + txteditempid.Text + "";
        //}
        if (txtfolionoedit.Text != "")
        {
            str = str + " and PIM.ChallanNo='" + txtfolionoedit.Text + "'";
        }
        str = str + " order by PIM.IssueOrderId";
         DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
         UtilityModule.ConditionalComboFillWithDS(ref DDFolioNo, ds, 0, true, "--Plz Select--");
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (DDFolioNo.Items.Count > 0)
            {
                if (DDFolioNo.Items.FindByValue(ds.Tables[0].Rows[0]["IssueOrderId"].ToString()) != null)
                {
                    DDFolioNo.SelectedValue = ds.Tables[0].Rows[0]["IssueOrderId"].ToString();
                }
                DDFolioNo_SelectedIndexChanged(sender, new EventArgs());
            }
        }
        //UtilityModule.ConditionalComboFill(ref DDFolioNo, str, true, "--Plz Select--");
        

       
    }

    protected void DDLoomNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str;
        // txtloomid.Text = DDLoomNo.SelectedValue;

        str = @"select Distinct PIM.IssueOrderId,isnull(PIM.ChallanNo,PIM.IssueOrderId) from Process_issue_master_1 PIM
                   left join Employee_ProcessOrderNo EMP on PIM.IssueOrderId=EMP.IssueOrderId and EMp.ProcessId=1
                   Where PIM.CompanyId=" + DDcompany.SelectedValue + " and PIM.Units=" + DDProdunit.SelectedValue + " and PIM.LoomId=" + DDLoomNo.SelectedValue;
        //if (chkcomplete.Checked == true)
        //{
        //    str = str + " and PIM.Status='Complete'";
        //}
        //else
        //{
        str = str + " and PIM.Status='Pending'";
        //}
        //if (txteditempid.Text != "")
        //{
        //    str = str + " and EMP.EMPID=" + txteditempid.Text + "";
        //}
        if (txtfolionoedit.Text != "")
        {
            //str = str + " and PIM.issueorderid=" + txtfolionoedit.Text + "";
            str = str + " and PIM.ChallanNo='" + txtfolionoedit.Text + "'";
        }
        str = str + " order by PIM.IssueOrderId";
        UtilityModule.ConditionalComboFill(ref DDFolioNo, str, true, "--Plz Select--");
        if (DDFolioNo.Items.Count > 0)
        {
            DDFolioNo.SelectedIndex = 1;
            DDFolioNo_SelectedIndexChanged(sender, new EventArgs());
        }

        //employee
        str = @"select EI.EmpId,EI.Empcode+' ['+EI.Empname+']' as Empname from EmpInfo EI inner join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentName='PRODUCTION'
        and EI.Status='P' and EI.Blacklist=0 order by Empname";
        UtilityModule.ConditionalComboFill(ref DDemployee, str, true, "--Plz select--");
    }
    protected void DDFolioNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnissueid.Value = "0";
        hnissueorderid.Value = DDFolioNo.SelectedValue;
        FillGrid();       
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        //if (txtgetvalue.Text != "")
        //{
        //    FillWeaver();
        //}
    }
    //protected void btnsearchedit_Click(object sender, EventArgs e)
    //{
    //    FIllProdUnit(sender);
    //}
    protected void FIllProdUnit(object sender = null)
    {

        string str = @"select Distinct U.UnitsId,U.UnitName,PIm.CompanyId From Process_issue_master_1 PIM inner Join  Units U on PIM.Units=U.UnitsId
                        inner join Employee_ProcessOrderNo EMP on PIM.Issueorderid=EMP.IssueOrderId and EMP.ProcessId=1
                        Where PIM.Companyid=" + DDcompany.SelectedValue;

        //if (txteditempid.Text != "")
        //{
        //    str = str + " and EMP.EMPID=" + txteditempid.Text;
        //}
        if (txtfolionoedit.Text != "")
        {
            str = str + " and PIM.ChallanNo='" + txtfolionoedit.Text + "'";
            //str = str + " and EMP.Issueorderid=" + txtfolionoedit.Text;
        }
        str = str + " order by Unitname";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 0, true, "--Plz Select--");
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (DDcompany.Items.FindByValue(ds.Tables[0].Rows[0]["CompanyId"].ToString()) != null)
            {
                DDcompany.SelectedValue = ds.Tables[0].Rows[0]["CompanyId"].ToString();
            }
        }

        //UtilityModule.ConditionalComboFill(ref DDProdunit, str, true, "--Plz Select--");
        if (DDProdunit.Items.Count > 0)
        {
            DDProdunit.SelectedIndex = 1;
            DDProdunit_SelectedIndexChanged(sender, new EventArgs());
        }

    }
    protected void FillFolioEmployee(object sender = null)
    {
        string str = "";
        //employee
        str = @"select distinct EI.EmpId,EI.Empcode+' ['+EI.Empname+']' as Empname from EmpInfo EI inner join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentName='PRODUCTION'
                JOIN EmpProcess EP ON EI.EmpId=EP.EmpId JOIN Process_Issue_Master_1 PIM ON EI.EmpId=PIM.EmpId
                Where EI.Status='P' and EI.Blacklist=0 and EP.ProcessId=1  ";
        if (txtfolionoedit.Text != "")
        {
            str = str + " and PIM.ChallanNo='" + txtfolionoedit.Text + "'";
        }
        str = str + " order by Empname";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        UtilityModule.ConditionalComboFillWithDS(ref DDFolioEmployee, ds, 0, true, "--Plz Select--");

        //UtilityModule.ConditionalComboFill(ref DDFolioEmployee, str, true, "--Plz select--");

        if (txtfolionoedit.Text != "")
        {
            if (DDFolioEmployee.Items.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (DDFolioEmployee.Items.FindByValue(ds.Tables[0].Rows[0]["EmpId"].ToString()) != null)
                    {
                        DDFolioEmployee.SelectedValue = ds.Tables[0].Rows[0]["EmpId"].ToString();
                    }
                }

                if (DDFolioEmployee.SelectedIndex > 0)
                {
                    DDFolioEmployee_SelectedIndexChanged(sender, new EventArgs());
                }

            }
        }
       
    }
    protected void txtfolionoedit_TextChanged(object sender, EventArgs e)
    {
        if (Session["VarCompanyId"].ToString() == "30" || Session["VarCompanyId"].ToString() == "38")
        {
            FillFolioEmployee(sender);
        }
        else
        {
            FIllProdUnit(sender);
        }
        FillIssueNo();
    }
    protected void DDMapStencilType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkEdit.Checked == true)
        {
            FillIssueNo();
        }
        txtIssueNo.Text = "";
        hnissueid.Value = "0";

        DG.DataSource = null;
        DG.DataBind();

        gvdetail.DataSource = null;
        gvdetail.DataBind();

    }
    protected void txtMapStencilstockno_TextChanged(object sender, EventArgs e)
    {
        lblmessage.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@CompanyId", DDcompany.SelectedValue);
            param[1] = new SqlParameter("@MapStencilType", DDMapStencilType.SelectedValue);
            param[2] = new SqlParameter("@IssueOrderId", DDFolioNo.SelectedValue);
            param[3] = new SqlParameter("@MSStockno", txtMapStencilstockno.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETMAPRECEIVEDETAIL_WITHMAPSTOCKNO", param);
            if (param[4].Value.ToString() != "")
            {
                lblmessage.Text = param[4].Value.ToString();
                DG.DataSource = null;
                DG.DataBind();
            }
            else
            {
                DG.DataSource = ds.Tables[0];
                DG.DataBind();
                for (int i = 0; i < DG.Rows.Count; i++)
                {
                    CheckBox Chkboxitem = (CheckBox)DG.Rows[i].FindControl("Chkboxitem");
                    TextBox txtqty = (TextBox)DG.Rows[i].FindControl("txtqty");
                    Chkboxitem.Checked = true;
                    txtqty.Text = "1";
                }
                btnsave_Click(sender, new EventArgs());
                DG.DataSource = null;
                DG.DataBind();
                txtMapStencilstockno.Text = "";
            }
            txtMapStencilstockno.Focus();
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        //if (txtloomid.Text == "" || txtloomid.Text == "0")
        //{
        //    ScriptManager.RegisterStartupScript(Page, GetType(), "loomid", "alert('Please select Loom No.');", true);
        //    return;
        //}

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //**************Sql Table
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("Itemfinishedid", typeof(int));
        dtrecords.Columns.Add("MapReceiveQty", typeof(int));
        dtrecords.Columns.Add("Qty", typeof(int));
        dtrecords.Columns.Add("MapStencilNo", typeof(int));
        dtrecords.Columns.Add("OrderId", typeof(int));
        //**************
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtqty = ((TextBox)DG.Rows[i].FindControl("txtqty"));
            if (Chkboxitem.Checked == true && (txtqty.Text != "" && txtqty.Text != "0"))
            {
                Label lblitemfinishedid = ((Label)DG.Rows[i].FindControl("lblitemfinishedid"));
                Label lblReceiveQty = ((Label)DG.Rows[i].FindControl("lblReceiveQty"));
                Label lblMapStencilNo = ((Label)DG.Rows[i].FindControl("lblMapStencilNo"));
                Label lblorderid = ((Label)DG.Rows[i].FindControl("lblorderid"));

                //********Data Row
                DataRow dr = dtrecords.NewRow();
                dr["Itemfinishedid"] = lblitemfinishedid.Text;
                dr["MapReceiveQty"] = lblReceiveQty.Text;
                dr["Qty"] = txtqty.Text;
                dr["MapStencilNo"] = lblMapStencilNo.Text;
                dr["OrderId"] = lblorderid.Text;
                dtrecords.Rows.Add(dr);
            }
        }
        if (dtrecords.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {

                //******
                SqlCommand cmd = new SqlCommand("PRO_SAVEMAPTRACENO_ONPRODUCTIONORDER", con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30000;
                cmd.Parameters.Add("@issueid", SqlDbType.Int);
                cmd.Parameters["@issueid"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@issueid"].Value = hnissueid.Value;
                cmd.Parameters.AddWithValue("@Companyid", DDcompany.SelectedValue);
                cmd.Parameters.AddWithValue("@ProductionUnitId", DDProdunit.SelectedValue);
                cmd.Parameters.AddWithValue("@LoomNoId", DDLoomNo.SelectedValue);
                cmd.Parameters.AddWithValue("@IssueOrderId", DDFolioNo.SelectedValue);
                cmd.Parameters.AddWithValue("@Issuedate", txtissuedate.Text);
                cmd.Parameters.AddWithValue("@MapStencilType", DDMapStencilType.SelectedValue);
                cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
                cmd.Parameters.AddWithValue("@Mastercompanyid", Session["varcompanyid"]);
                cmd.Parameters.AddWithValue("@dtrecords", dtrecords);
                cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@MSStockno", txtMapStencilstockno.Text);
                cmd.Parameters.Add("@ChallanNo", SqlDbType.VarChar,30);
                cmd.Parameters["@ChallanNo"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@ChallanNo"].Value = "";
                cmd.ExecuteNonQuery();
                if (cmd.Parameters["@msg"].Value.ToString() != "") //IF DATA NOT SAVED
                {
                    lblmessage.Text = cmd.Parameters["@msg"].Value.ToString();
                    Tran.Rollback();
                }
                else
                {
                    lblmessage.Text = "Data Saved Successfully.";
                    Tran.Commit();
                    //txtfoliono.Text = cmd.Parameters["@FolioNo"].Value.ToString(); //param[5].Value.ToString();
                    hnissueid.Value = cmd.Parameters["@issueid"].Value.ToString();// param[0].Value.ToString();
                    txtIssueNo.Text = cmd.Parameters["@ChallanNo"].Value.ToString();
                    FillissueGrid();
                }
                //******

            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblmessage.Text = ex.Message;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
        }
    }

    protected void FillissueGrid()
    {
        string str = @"select MIM.IssueId,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,
                        CASE WHEN MSSN.UnitID = 2 THEN VF.SIZEFT ELSE VF.SIZEMTR END As Size,
                        MID.IssueDetailId,MIM.IssueOrderId,MID.ItemFinishedID,MIM.CompanyId,MIM.ProductionUnitId,MIM.LoomNoId,MIM.MapStencilType,MIM.ChallanNo,
                        Replace(CONVERT(nvarchar(11),MIM.IssueDate,106),' ','-') as IssueDate,
                        1 as Qty,MSSN.MSStockNo,MSSN.MapStencilNo
                        from Map_IssueOnProductionOrderMaster MIM INNER JOIN Map_IssueOnProductionOrderDetail MID ON MIM.IssueId=MID.IssueId 
                        INNER JOIN V_FinishedItemDetail VF ON MID.ItemFinishedId=VF.Item_Finished_Id
                        INNER JOIN OrderDetail OD ON MID.OrderId=OD.OrderID and MID.ItemFinishedId=OD.Item_Finished_id
                        INNER JOIN Map_StencilStockNo_Detail MSSND ON MIM.IssueId=MSSND.IssueID and MID.IssueDetailId=MSSND.IssueDetailId
                        INNER JOIN Map_StencilStockNo MSSN ON MSSND.MapStencilNo=MSSN.MapStencilNo
                        where MIM.CompanyId=" + DDcompany.SelectedValue + " ";
        ////where MIM.ID=" + hnissueid.Value;
        //if (txtEditIssueNo.Text != "")
        //{
        //    str = str + " and MIM.ChallanNo='" + txtEditIssueNo.Text + "'";
        //}
        //else
        //{
        str = str + " and MIM.IssueID=" + hnissueid.Value + "";
        //}

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
    }
    protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow && gvdetail.EditIndex == e.Row.RowIndex)
        //{
        //    DropDownList DDMapType = (DropDownList)e.Row.FindControl("DDMapType");

        //    string str = @"select ID,MapType from MapType order by MapType";

        //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //    UtilityModule.ConditionalComboFillWithDS(ref DDMapType, ds, 0, true, "--Plz Select--");

        //    string selectedMapIssueType = DataBinder.Eval(e.Row.DataItem, "MapIssueType").ToString();
        //    DDMapType.Items.FindByValue(selectedMapIssueType).Selected = true;

        //}
    }

    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //Label lblhqty = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblhqty");
            Label lblIssueId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblIssueId");
            Label lblIssueDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblIssueDetailId");
            Label lblMapStencilNo = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblMapStencilNo");

            Label lblQty = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblQty");
            Label lblItemFinishedId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblItemFinishedId");

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@IssueId", lblIssueId.Text);
            param[1] = new SqlParameter("@IssueDetailid", lblIssueDetailId.Text);
            param[2] = new SqlParameter("@Qty", lblQty.Text);
            param[3] = new SqlParameter("@ItemFinishedId", lblItemFinishedId.Text);
            param[4] = new SqlParameter("@MapStencilNo", lblMapStencilNo.Text);
            param[5] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[5].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEMAPISSUE_ONPRODUCTIONORDER", param);
            lblmessage.Text = param[5].Value.ToString();
            Tran.Commit();
            FillissueGrid();
            //***************
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@CompanyId", DDcompany.SelectedValue);
        param[1] = new SqlParameter("@IssueID", hnissueid.Value);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_WeaverMapIssueReport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Session["rptFileName"] = "~\\Reports\\RptWeaverMapIssue.rpt";
            Session["rptFileName"] = "~\\Reports\\RptWeaverMapIssueDuplicate.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptWeaverMapIssue.xsd";
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