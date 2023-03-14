using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ClosedXML.Excel;
using System.IO;

public partial class Masters_MachineProcess_FrmRollStitchingMaterialIssue : System.Web.UI.Page
{
    static string btnclickflag = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                            select PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where ProcessType=1 and MasterCompanyid=" + Session["varcompanyid"] + @" and PROCESS_NAME='STITCHING' order by PROCESS_NAME_ID
                            Select ID, BranchName 
                            From BRANCHMASTER BM(nolock) 
                            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"]+@"
                            Select EI.EmpId, EI.EmpName  From Empinfo EI(Nolock) JOIN EMPPROCESS EP(Nolock) ON EP.EmpId = EI.EmpId";

                            if (Convert.ToInt32(Session["varcompanyid"]) == 21)
                            {
                                str = str + " And EP.ProcessId = 17 ";
                            }
                            else
                            {
                                str = str + " And EP.ProcessId = 1 ";
                            }

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            
            UtilityModule.ConditionalComboFillWithDS(ref DDprocess, ds, 1, true, "--Plz Select--");
            
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 2, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 3, true, "--Plz Select--");

            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
            }
           
            //if (variable.VarWeaverRawMaterialIssueToCompleteStatus == "1" && Session["usertype"].ToString() == "1")
            //{
            //    TDForCompleteStatus.Visible = true;
            //}           
           
        }
    }
    protected void DDprocess_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (DDprocess.SelectedItem.Text.ToUpper())
        {
            case "WEAVING":
                //TDProductionunit.Visible = true;
                //TDLoomNo.Visible = true;
                break;
            default:
                //TDProductionunit.Visible = false;
                //TDLoomNo.Visible = false;
                FillFolioNo();
                break;
        }

    }
    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillFolioNo();
//        if (ChKForEdit.Checked == true)
//        {
//            string str = @"Select RollIssueOtherProcessID, IssueNo 
//            From RollIssueOtherProcessMatser(Nolock) 
//            Where MasterCompanyID = " + Session["VarCompanyId"] + " And CompanyID = " + ddCompName.SelectedValue + " And ProcessID = " + DDProcessName.SelectedValue + @" 
//            And EmpID = " + DDEmployeeName.SelectedValue + " Order By RollIssueOtherProcessID Desc ";

//            DataSet ds = SqlHelper.ExecuteDataset(str);
        //            UtilityModule.ConditionalComboFillWithDS(ref DDFoliono, ds, 0, true, "-Select Issue No-");
//        }
    }
    protected void FillFolioNo(object sender = null)
    {
        string str = @"Select distinct RIOPM.RollIssueOtherProcessID,IssueNo from RollIssueOtherProcessMatser RIOPM 
                    Where RIOPM.Companyid=" + DDcompany.SelectedValue + " And RIOPM.ProcessID=" + DDprocess.SelectedValue + " and RIOPM.EmpId="+DDEmployeeName.SelectedValue+" ";
        //if (TDForCompleteStatus.Visible == true)
        //{
        //    if (ChkForCompleteStatus.Checked == true)
        //    {
        //        str = str + " and PM.status='Complete'";
        //    }
        //    else
        //    {
        //        str = str + " and PM.status='Pending'";
        //    }
        //}
        //else
        //{
        //    str = str + " and PM.status='Pending'";
        //}       
        
        if (TDFolioNo.Visible == true)
        {
            if (txtFolioNo.Text != "")
            {
                str = str + " and RINPM.IssueNo='" + txtFolioNo.Text + "'";
            }
        }
        str = str + " Order By RollIssueOtherProcessID Desc";

        UtilityModule.ConditionalComboFill(ref DDFoliono, str, true, "--Plz Select--");
        if (DDFoliono.Items.Count > 0)
        {
            if (sender != null)
            {
                DDFoliono.SelectedIndex = 1;
                DDFoliono_SelectedIndexChanged(sender, new EventArgs());
            }

        }
    }
    protected void DDFoliono_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkEdit.Checked == true)
        {
            string str = @"select Distinct PRM.RollStitchingMaterialIssueId,PRM.ChallanNo 
                        From RollStitchingMaterialIssueMaster PRM 
                        inner join RollStitchingMaterialIssueDetail PRT on PRM.RollStitchingMaterialIssueId=PRT.RollStitchingMaterialIssueId 
                        Where PRM.MaterialIssuePageTypeId=1 and PRM.TranType=0 and PRM.CompanyID = " + DDcompany.SelectedValue + " and PRM.IssueNoId=" + DDFoliono.SelectedValue + " and PRM.Processid=" + DDprocess.SelectedValue+" and PRM.EmpId="+DDEmployeeName.SelectedValue+" ";

            UtilityModule.ConditionalComboFill(ref DDissueno, str, true, "--Plz Select--");
            if (Session["varcompanyid"].ToString() == "21")
            {
                Fillgrid();
            }
           
        }
        else
        {
            Fillgrid();
        }

    }
    protected void Fillgrid()
    {       

        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@RollIssueOtherProcessID", DDFoliono.SelectedValue);
        param[1] = new SqlParameter("@ProcessId", DDprocess.SelectedValue);
        param[2] = new SqlParameter("@UserId", Session["VarUserId"]);
        param[3] = new SqlParameter("@MasterCompanyId", Session["VarCompanyNo"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "FILL_STITCHING_ROLLCONSUMPTION_FOR_ISSUEMATERIAL", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DG.DataSource = ds.Tables[0];
            DG.DataBind();
        }
        else
        {
            DG.DataSource = null;
            DG.DataBind();
        }
        

    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            for (int i = 0; i < DG.Columns.Count; i++)
            {
                if (variable.VarBINNOWISE == "1")
                {
                    if (DG.Columns[i].HeaderText == "BinNo")
                    {
                        DG.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (DG.Columns[i].HeaderText == "BinNo")
                    {
                        DG.Columns[i].Visible = false;
                    }
                }
            }
            DropDownList DDGodown = ((DropDownList)e.Row.FindControl("DDGodown"));
            string str = @"Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + @" Order by GodownName
                           select godownid From Modulewisegodown Where ModuleName='" + Page.Title + "'";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDGodown, ds, 0, true, "--Plz Select--");

            if (hngodownid.Value == "0")
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (DDGodown.Items.FindByValue(ds.Tables[1].Rows[0]["godownid"].ToString()) != null)
                    {
                        DDGodown.SelectedValue = ds.Tables[1].Rows[0]["godownid"].ToString();
                    }
                }
                else
                {
                    if (DDGodown.Items.Count > 0)
                    {
                        DDGodown.SelectedIndex = 1;
                    }
                }
            }
            else
            {
                if (DDGodown.Items.FindByValue(hngodownid.Value) != null)
                {
                    DDGodown.SelectedValue = hngodownid.Value;
                }
            }
            DDgodown_SelectedIndexChanged(DDGodown, new EventArgs());
            ds.Dispose();
        }
    }
    protected void DDBinNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList DDBinNo = (DropDownList)sender;
        GridViewRow row = (GridViewRow)DDBinNo.Parent.Parent;
        Label Ifinishedid = ((Label)row.FindControl("lblifinishedid"));
        DropDownList ddlgodown = ((DropDownList)row.FindControl("DDGodown"));

        DropDownList ddLotno = ((DropDownList)row.FindControl("DDLotNo"));

        string str = "select Distinct S.Lotno,S.Lotno from Stock S Where S.companyid=" + DDcompany.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Qtyinhand>0";
        //if (variable.VarBINNOWISE == "1")
        //{
        //    str = str + " and BinNo='" + DDBinNo.SelectedItem.Text + "'";
        //}
        UtilityModule.ConditionalComboFill(ref ddLotno, str, true, "--Plz Select--");
       
       
        if (ddLotno.Items.Count > 0)
        {
            ddLotno.SelectedIndex = 1;
            DDLotno_SelectedIndexChanged(ddLotno, e);
        }
    }
    protected void DDgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlgodown = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlgodown.Parent.Parent;
        Label Ifinishedid = ((Label)row.FindControl("lblifinishedid"));

        int index = row.RowIndex;

        DropDownList ddLotno = ((DropDownList)row.FindControl("DDLotNo"));
       
         string str = "select Distinct S.Lotno,S.Lotno from Stock S Where S.companyid=" + DDcompany.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Qtyinhand>0";
         UtilityModule.ConditionalComboFill(ref ddLotno, str, true, "--Plz Select--");
       
        if (ddLotno.Items.Count > 0)
        {
            ddLotno.SelectedIndex = 1;
            DDLotno_SelectedIndexChanged(ddLotno, e);
        }          


    }
    protected void DDLotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlLotno = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlLotno.Parent.Parent;
        int index = row.RowIndex;
        //Label Ifinishedid = ((Label)DG.Rows[index].FindControl("lblifinishedid"));
        Label Ifinishedid = (Label)row.FindControl("lblifinishedid");
        //DropDownList DDTagNo = ((DropDownList)DG.Rows[index].FindControl("DDTagNo"));
        //DropDownList ddlgodown = ((DropDownList)DG.Rows[index].FindControl("DDgodown"));
        DropDownList DDTagNo = ((DropDownList)row.FindControl("DDTagNo"));
        DropDownList ddlgodown = ((DropDownList)row.FindControl("DDgodown"));
        DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));

        string str = "select Distinct S.TagNo,S.Tagno from Stock S Where S.companyid=" + DDcompany.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Lotno='" + ddlLotno.Text + "' and S.Qtyinhand>0";
        //if (variable.VarBINNOWISE == "1")
        //{
        //    str = str + " and BinNo='" + DDBinNo.SelectedItem.Text + "'";
        //}
        UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "--Plz Select--");

              
        if (DDTagNo.Items.Count > 0)
        {
            DDTagNo.SelectedIndex = 1;
            DDTagno_SelectedIndexChanged(DDTagNo, e);
        }
    }
    protected void DDTagno_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddTagno = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddTagno.Parent.Parent;
        int index = row.RowIndex;
        int Ifinishedid = Convert.ToInt32(((Label)row.FindControl("lblifinishedid")).Text);
        Label lblstockqty = ((Label)row.FindControl("lblstockqty"));
        DropDownList ddgodown = ((DropDownList)row.FindControl("DDgodown"));
        DropDownList ddlotno = ((DropDownList)row.FindControl("DDLotNo"));
        DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));

        Double StockQty = UtilityModule.getstockQty(DDcompany.SelectedValue, ddgodown.SelectedValue, ddlotno.Text, Ifinishedid, ddTagno.Text, BinNo: (variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : ""));
        lblstockqty.Text = StockQty.ToString();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        string DetailData = "";
        if (Session["varcompanyid"].ToString() == "21")
        {
            string status = "";            

            for (int i = 0; i < DG.Rows.Count; i++)
            {
                CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
                TextBox txtissueqty = ((TextBox)DG.Rows[i].FindControl("txtissueqty"));

                if (Chkboxitem.Checked == true)   // Change when Updated Completed
                {
                    status = "1";
                }
                if (txtissueqty.Text == "" && Chkboxitem.Checked == true)   // Change when Updated Completed
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Issue qty can not be blank');", true);
                    txtissueqty.Focus();
                    return;
                }
                if (Chkboxitem.Checked == true && (Convert.ToDecimal(txtissueqty.Text) <= 0))   // Change when Updated Completed
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Issue qty always greater then zero');", true);
                    txtissueqty.Focus();
                    return;
                }
               
            }

            if (status == "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check boxes');", true);
                return;
            }
        }

        ////********sql table Type
        //DataTable dtrecords = new DataTable();
        //dtrecords.Columns.Add("ifinishedid", typeof(int));
        //dtrecords.Columns.Add("IUnitid", typeof(int));        
        //dtrecords.Columns.Add("Godownid", typeof(int));
        //dtrecords.Columns.Add("Lotno", typeof(string));
        //dtrecords.Columns.Add("TagNo", typeof(string));
        //dtrecords.Columns.Add("issueqty", typeof(float));
        ////dtrecords.Columns.Add("Noofcone", typeof(int));
        //dtrecords.Columns.Add("IssueNoId", typeof(int));
        //dtrecords.Columns.Add("ConsmpQty", typeof(float));       
        ////*******************
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtissueqty = ((TextBox)DG.Rows[i].FindControl("txtissueqty"));
            DropDownList DDGodown = ((DropDownList)DG.Rows[i].FindControl("DDGodown"));
            DropDownList DDLotNo = ((DropDownList)DG.Rows[i].FindControl("DDLotNo"));
            DropDownList DDTagNo = ((DropDownList)DG.Rows[i].FindControl("DDTagNo"));            

            if (Chkboxitem.Checked == true && (txtissueqty.Text != "") && DDGodown.SelectedIndex > 0 && DDLotNo.SelectedIndex > 0 && DDTagNo.SelectedIndex > 0)
            {
                Label lblitemfinishedid = ((Label)DG.Rows[i].FindControl("lblifinishedid"));
                Label lblunitid = ((Label)DG.Rows[i].FindControl("lbliunitid"));                
                string Lotno = DDLotNo.Text;
                string TagNo = DDTagNo.Text;
                //TextBox txtnoofcone = ((TextBox)DG.Rows[i].FindControl("txtnoofcone"));
                Label lblRollIssueOtherProcessID = ((Label)DG.Rows[i].FindControl("lblRollIssueOtherProcessID"));
                Label lblconsmpqty = ((Label)DG.Rows[i].FindControl("lblconsmpqty"));
                //*********************
                  
                //DataRow dr = dtrecords.NewRow();
                //dr["ifinishedid"] = lblitemfinishedid.Text;
                //dr["IUnitid"] = lblunitid.Text;               
                //dr["Godownid"] = DDGodown.SelectedValue;
                //dr["Lotno"] = Lotno;
                //dr["TagNo"] = TagNo;
                //dr["IssueQty"] = txtissueqty.Text == "" ? "0" : txtissueqty.Text;
                ////dr["Noofcone"] = txtnoofcone.Text == "" ? "0" : txtnoofcone.Text;
                //dr["IssueNoId"] = lblRollIssueToNextID.Text;
                //dr["consmpqty"] = lblconsmpqty.Text;               
                //dtrecords.Rows.Add(dr);


                if (DetailData == "")
                {
                    DetailData = lblitemfinishedid.Text + "|" + lblunitid.Text + "|" + DDGodown.SelectedValue + "|" + Lotno + "|" + TagNo + "|" + txtissueqty.Text + "|" + lblRollIssueOtherProcessID.Text + "|" + lblconsmpqty.Text + "~";
                }
                else
                {
                    DetailData = DetailData + lblitemfinishedid.Text + "|" + lblunitid.Text + "|" + DDGodown.SelectedValue + "|" + Lotno + "|" + TagNo + "|" + txtissueqty.Text + "|" + lblRollIssueOtherProcessID.Text + "|" + lblconsmpqty.Text + "~";
                }
            }
        }
        if (DetailData == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box');", true);
            return;
        }
        //if (Session["varcompanyid"].ToString() == "21")
        //{
        //    if (chkEdit.Checked == false)     // Change when Updated Completed
        //    {
        //        if (dtrecords.Rows.Count != DG.Rows.Count)
        //        {
        //            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please check stock qty in all rows');", true);
        //            return;
        //        }
        //    }
        //}
        

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        if (DetailData!="")
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[14];
                param[0] = new SqlParameter("@RollStitchingMaterialIssueId", SqlDbType.Int);
                if (chkEdit.Checked == true && Session["varcompanyid"].ToString() == "21")
                {
                    param[0].Value = DDissueno.SelectedValue;
                }
                else
                {
                    param[0].Value = 0;
                }

                param[0].Direction = ParameterDirection.InputOutput;
                param[1] = new SqlParameter("@companyid", DDcompany.SelectedValue);
                param[2] = new SqlParameter("@Processid", DDprocess.SelectedValue);
                param[3] = new SqlParameter("@EmpId", DDEmployeeName.SelectedValue);

                param[4] = new SqlParameter("@IssueNoId", DDFoliono.SelectedValue);
                param[5] = new SqlParameter("@ChallanDate", txtissuedate.Text);
                param[6] = new SqlParameter("@userid", Session["varuserid"]);
                param[7] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
                param[8] = new SqlParameter("@DetailData", DetailData);
                param[9] = new SqlParameter("@TranType", SqlDbType.TinyInt);
                param[9].Value = 0;
                param[10] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[10].Direction = ParameterDirection.Output;                
                param[11] = new SqlParameter("@CHALANNO", SqlDbType.VarChar, 50);
                param[11].Value = "";
                param[11].Direction = ParameterDirection.InputOutput;
                param[12] = new SqlParameter("@BranchId", DDBranchName.SelectedValue);
                param[13] = new SqlParameter("@MaterialIssuePageTypeId", 1);

                ///**********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEROLLSTITCHINGMATERIALISSUE", param);
                //*******************
                ViewState["reportid"] = param[0].Value.ToString();
                txtissueno.Text = param[11].Value.ToString();
                hnissueid.Value = param[0].Value.ToString();
                Tran.Commit();
                if (param[10].Value.ToString() != "")
                {
                    lblmessage.Text = param[10].Value.ToString();
                }
                else
                {
                    lblmessage.Text = "DATA SAVED SUCCESSFULLY.";
                    Fillgrid();
                    FillissueGrid();
                }                

            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblmessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
        }
    }
    protected void FillissueGrid()
    {
        string str = @"select VF.ITEM_NAME+SPACE(2)+VF.QUALITYNAME+SPACE(2)+VF.DESIGNNAME+SPACE(2)+VF.COLORNAME+SPACE(2)+VF.SHAPENAME+SPACE(2)+ VF.SIZEMTR +SPACE(2)+VF.SHADECOLORNAME  as ItemDescription,
                        RSMID.Lotno,RSMID.TagNo,RSMID.IssueQty,RSMIM.ChallanNo,Replace(CONVERT(nvarchar(11),RSMIM.ChallanDate,106),' ','-') as ChallanDate,RSMIM.RollStitchingMaterialIssueID,
			            RSMID.RollStitchingMaterialIssueDetailID,RSMIM.IssueNoId,RSMIM.processid
                    From RollStitchingMaterialIssueMaster RSMIM inner join RollStitchingMaterialIssueDetail RSMID on RSMIM.RollStitchingMaterialIssueID=RSMID.RollStitchingMaterialIssueID
		            JOIN V_FINISHEDITEMDETAIL VF ON VF.ITEM_FINISHED_ID=RSMID.FinishedId
                    Where RSMIM.TranType=0 And RSMIM.MaterialIssuePageTypeId = 1 and RSMIM.RollStitchingMaterialIssueID=" + hnissueid.Value;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
        if (chkEdit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtissueno.Text = ds.Tables[0].Rows[0]["ChallanNo"].ToString();
                txtissuedate.Text = ds.Tables[0].Rows[0]["ChallanDate"].ToString();                
            }
            else
            {
                txtissueno.Text = "";
                txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@RollStitchingMaterialIssueId", hnissueid.Value);
        param[1] = new SqlParameter("@processid", DDprocess.SelectedValue);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_ROLLSTITCHINGMATERIALISSUEREPORT", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptRollStitchingMaterialIssue.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRollStitchingMaterialIssue.xsd";
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
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
       
        DDFoliono.SelectedIndex = -1;
        TDIssueNo.Visible = false;
        DG.DataSource = null;
        DG.DataBind();
        gvdetail.DataSource = null;
        gvdetail.DataBind();
        txtissueno.Text = "";
        txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

        if (variable.VarWeaverRawMaterialIssueToCompleteStatus == "1" && Session["usertype"].ToString() == "1")
        {
            //TDForCompleteStatus.Visible = true;
        }

        if (chkEdit.Checked == true)
        {
            TDIssueNo.Visible = true;
            DDissueno.SelectedIndex = -1;

            //if (Session["varcompanyid"].ToString() == "21")
            //{
            //    if (variable.VarWeaverRawMaterialIssueToCompleteStatus == "1" && Session["usertype"].ToString() == "1")
            //    {
            //        //TDForCompleteStatus.Visible = true;
            //        //////ChkForCompleteStatus.Checked = false;
            //    }
            //}
            //else
            //{
            //    if (variable.VarWeaverRawMaterialIssueToCompleteStatus == "1" && Session["usertype"].ToString() == "1")
            //    {
            //        //TDForCompleteStatus.Visible = false;
            //        //ChkForCompleteStatus.Checked = false;
            //    }
            //}
           
        }
    }
    protected void DDissueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnissueid.Value = DDissueno.SelectedValue;
        FillissueGrid();
    }
    protected void gvdetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvdetail.EditIndex = e.NewEditIndex;
        FillissueGrid();
    }
    protected void gvdetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvdetail.EditIndex = -1;
        FillissueGrid();
    }
    protected void gvdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblhqty = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblhqty");
            Label lblRollStitchingMaterialIssueId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblRollStitchingMaterialIssueId");
            Label lblRollStitchingMaterialIssueDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblRollStitchingMaterialIssueDetailId");
            Label lblIssueNoId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblIssueNoId");
            TextBox txtqty = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtqty");
            Label lblprocessid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprocessid");
            //**************
            SqlParameter[] param = new SqlParameter[9];
            param[0] = new SqlParameter("@RollStitchingMaterialIssueId", lblRollStitchingMaterialIssueId.Text);
            param[1] = new SqlParameter("@RollStitchingMaterialIssueDetailId", lblRollStitchingMaterialIssueDetailId.Text);
            param[2] = new SqlParameter("@IssueNoId", lblIssueNoId.Text);
            param[3] = new SqlParameter("@hqty", lblhqty.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@qty", txtqty.Text == "" ? "0" : txtqty.Text);
            param[6] = new SqlParameter("@processid", lblprocessid.Text);
            param[7] = new SqlParameter("@userid", Session["varuserid"]);           
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEROLLSTITCHINGMATERIALISSUE", param);
            lblmessage.Text = param[4].Value.ToString();
            Tran.Commit();
            gvdetail.EditIndex = -1;
            FillissueGrid();
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
            Label lblRollStitchingMaterialIssueId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblRollStitchingMaterialIssueId");
            Label lblRollStitchingMaterialIssueDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblRollStitchingMaterialIssueDetailId");
            Label lblIssueNoId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblIssueNoId");
            Label lblprocessid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprocessid");

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@RollStitchingMaterialIssueId", lblRollStitchingMaterialIssueId.Text);
            param[1] = new SqlParameter("@RollStitchingMaterialIssueDetailId", lblRollStitchingMaterialIssueDetailId.Text);
            param[2] = new SqlParameter("@IssueNoId", lblIssueNoId.Text);
            param[3] = new SqlParameter("@Processid", lblprocessid.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEROLLSTITCHINGMATERIALISSUE", param);
            lblmessage.Text = param[4].Value.ToString();
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
   
    protected void ChkForCompleteStatus_CheckedChanged(object sender, EventArgs e)
    {
        //if (ChkForCompleteStatus.Checked == false)
        //{
        //    DDLoomNo.SelectedIndex = -1;
        //    DDFoliono.Items.Clear();
        //    TDIssueNo.Visible = false;
        //    DG.DataSource = null;
        //    DG.DataBind();
        //    gvdetail.DataSource = null;
        //    gvdetail.DataBind();
        //    txtissueno.Text = "";
        //    txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        //}
        //else if (ChkForCompleteStatus.Checked == true)
        //{
        //    DDLoomNo.SelectedIndex = -1;
        //    DDFoliono.Items.Clear();
        //    DDLoomNo_SelectedIndexChanged(sender, new EventArgs());

        //}
    }   

    protected void btnCheck_Click(object sender, EventArgs e)
    {
        //if (MySession.ProductionEditPwd == txtpwd.Text)
        //{
        //    if (btnclickflag == "BtnUpdateStockNoQty")
        //    {
        //        UpdateStockNoQty();
        //    }           
        //    Popup(false);
        //}
        //else
        //{
        //    lblmessage.Visible = true;
        //    lblmessage.Text = "Please Enter Correct Password..";
        //}
    }
    void Popup(bool isDisplay)
    {
        StringBuilder builder = new StringBuilder();
        if (isDisplay)
        {
            builder.Append("<script>");
            builder.Append("ShowPopup();</script>");
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", builder.ToString());
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ShowPopup", builder.ToString(), false);
        }
        else
        {
            builder.Append("<script>");
            builder.Append("HidePopup();</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HidePopup", builder.ToString(), false);
        }
    }

    protected void txtFolioNo_TextChanged(object sender, EventArgs e)
    {
//        string str = @"select PIM.IssueOrderId From PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + @" PIM
//                       Where PIM.IssueOrderId='" + txtFolioNo.Text + "'";

//        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
//        if (ds.Tables[0].Rows.Count > 0)
//        {

//            DDprocess_SelectedIndexChanged(sender, new EventArgs());
//            //***********Fill PROduction UNI
//            str = @"SELECT DISTINCT PIM.UNITS FROM PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + @" PIM Where PIM.IssueOrderID=" + ds.Tables[0].Rows[0]["IssueOrderId"];

//            DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
//            if (ds1.Tables[0].Rows.Count > 0)
//            {
//                if (DDProdunit.Items.FindByValue(ds1.Tables[0].Rows[0]["UNITS"].ToString()) != null)
//                {
//                    DDProdunit.SelectedValue = ds1.Tables[0].Rows[0]["UNITS"].ToString();
//                    DDProdunit_SelectedIndexChanged(sender, new EventArgs());
//                }
//            }
//            //**************
//            DDissueno.Focus();
           
//        }
//        else
//        {
//            ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('Folio No does not exists in this Department.')", true);
//            txtFolioNo.Focus();
//        }
    }
}
