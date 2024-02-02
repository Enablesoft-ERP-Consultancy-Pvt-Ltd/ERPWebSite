using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_YarnOpening_frmyarnopeningReceive : System.Web.UI.Page
{
    static int rowindex = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select CI.CompanyId,CompanyName 
                            From CompanyInfo CI 
                            JOIN Company_Authentication CA on CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + @" And 
                            CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by CompanyName 
                           select EI.EmpId,EI.EmpName+case when isnull(EI.empcode,'')<>'' Then ' ['+EI.empcode+']' Else '' End Empname  from empinfo EI inner join Department D 
                           on EI.departmentId=D.DepartmentId Where D.DepartmentName in('Yarn Opening','WEFT DEPARTMENT') and isnull(Blacklist,0)=0";
            if (Session["varcompanyNo"].ToString() != "16")
            {
                if (variable.VarYARNOPENINGISSUEEMPWISE == "0")
                {
                    str = str + "  and EI.EmpName in('Yarn Opening','YARN OPENING-2','YARN OPENING-3')";
                }

            }
            str = str + " order by EmpName";
            str = str + " Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName";
            str = str + " select Godownid From ModuleWiseGodown where ModuleName='" + Page.Title + "' ";
            str = str + " Select D.Departmentid,D.DepartmentName From Department D Where D.DepartmentName in('Yarn Opening','WEFT DEPARTMENT') ";
            str = str + @" Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 5, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDvendor, ds, 1, false, "");
            if (DDvendor.Items.Count > 0)
            {
                DDvendor_SelectedIndexChanged(sender, e);
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDGodown, ds, 2, true, "--Plz Select--");
            if (ds.Tables[3].Rows.Count > 0)
            {
                if (DDGodown.Items.FindByValue(ds.Tables[3].Rows[0]["godownid"].ToString()) != null)
                {
                    DDGodown.SelectedValue = ds.Tables[3].Rows[0]["godownid"].ToString();
                    DDGodown_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            else
            {
                if (DDGodown.Items.Count > 0)
                {
                    DDGodown.SelectedIndex = 1;
                    DDGodown_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDdept, ds, 4, true, "--Select Department--");

            txtRecdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (variable.Carpetcompany == "1")
            {
                TRempcodescan.Visible = true;
            }
            if (variable.VarYARNOPENINGISSUEEMPWISE == "1")
            {
                lblyarnopendept.Text = "Employee Name";
                TDdept.Visible = true;
            }
            //**************
            if (Session["canedit"].ToString() == "1")
            {
                Chkedit.Visible = true;
                chkcompleteissueno.Visible = true;
            }
            //if (variable.VarBINNOWISE == "1")
            //{
            //    TDBinNo.Visible = true;
            //}
        }
    }
    protected void DDvendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillIssueno();

        if (Chkedit.Checked == true)
        {
            fillReceiveNo();
        }
    }
    protected void FillIssueno()
    {

        string str = @"select  Distinct YM.Id,YM.Issueno+'/'+REPLACE(CONVERT(nvarchar(11),YM.Issuedate,106),' ','-') from YarnOpeningIssueMaster YM  inner join YarnOpeningIssueTran YT on YM.ID=YT.MasterId Where ym.vendorId=" + DDvendor.SelectedValue + @" and ym.companyid=" + DDcompany.SelectedValue + " and ym.Status='Pending'";
        if (txtLotno.Text != "")
        {
            str = str + " and yt.Lotno='" + txtLotno.Text + "'";
        }
        str = str + " order by ym.id desc";
        if (DDdept.SelectedIndex > 0)
        {
            str = str + @" select EI.EmpId,EI.EmpName+' '+case WHen EI.EMpcode<>'' Then '['+EI.empcode+']' Else '' End as EmpName from empinfo EI inner join Department D 
                       on EI.departmentId=D.DepartmentId Where D.DepartmentId=" + DDdept.SelectedValue + " and isnull(Blacklist,0)=0 order by EmpName";
        }
        else
        {
            str = str + @" select EI.EmpId,EI.EmpName+' '+case WHen EI.EMpcode<>'' Then '['+EI.empcode+']' Else '' End as EmpName from empinfo EI inner join Department D 
                       on EI.departmentId=D.DepartmentId Where D.DepartmentName in('Yarn Opening','WEFT DEPARTMENT') and isnull(Blacklist,0)=0 order by EmpName";
        }


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        UtilityModule.ConditionalComboFillWithDS(ref DDIssueno, ds, 0, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDemployee, ds, 1, true, "--Select--");
        if (DDemployee.Items.Count > 0)
        {
            if (DDemployee.Items.FindByValue(DDvendor.SelectedValue) != null)
            {
                DDemployee.SelectedValue = DDvendor.SelectedValue;
            }
        }
    }
    protected void FillIssueDetail()
    {
        string str = @"select dbo.F_getItemDescription(YT.Item_Finished_id,YT.flagsize) as ItemDescription,
                        U.UnitName,YT.Lotno,YT.Tagno,Round(yt.IssueQty-isnull(VRT.Retqty,0),3) as IssueQty,Round(isnull(YR.ReceivedQty,0),3) As ReceivedQty,YT.Item_Finished_id,YT.Unitid,YT.flagsize,YM.ID,yt.Detailid,
                        ISNULL(Rectype,'') as RecType,Isnull(YT.ConeType,'') as Conetype,
                        Isnull(YT.Rate,0) as Rate,isnull(YM.Remarks,'') as Remarks, YT.PlyType, YT.TransportType, YT.Moisture 
                        from YarnOpeningIssueMaster YM 
                        inner join YarnOpeningIssueTran YT on YM.ID=YT.MasterId
                        inner join Unit U on YT.Unitid=U.UnitId
                        left join V_getYarnOpeningReceivedQty YR on Yt.Detailid=YR.issuemasterDetailid and YT.MasterId=YR.issuemasterid
                        left join V_getYarnOpeningReturnQty VRT on YT.MasterId=VRT.issuemasterid and YT.Detailid=VRT.Issuedetailid
                        Where YM.Id=" + DDIssueno.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
        txtissuedremark.Text = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtissuedremark.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
        }
    }
    protected void DDIssueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillIssueDetail();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //********sql table Type
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("Item_Finished_id", typeof(int));
        dtrecords.Columns.Add("Unitid", typeof(int));
        dtrecords.Columns.Add("flagsize", typeof(int));
        dtrecords.Columns.Add("Godownid", typeof(int));
        dtrecords.Columns.Add("Lotno", typeof(string));
        dtrecords.Columns.Add("TagNo", typeof(string));
        dtrecords.Columns.Add("RecQty", typeof(float));
        dtrecords.Columns.Add("LossQty", typeof(float));
        dtrecords.Columns.Add("Rectype", typeof(string));
        dtrecords.Columns.Add("Noofcone", typeof(int));
        dtrecords.Columns.Add("Conetype", typeof(string));
        dtrecords.Columns.Add("Issuemasterid", typeof(int));
        dtrecords.Columns.Add("IssuemasterDetailid", typeof(int));
        dtrecords.Columns.Add("Rate", typeof(float));
        dtrecords.Columns.Add("BinNo", typeof(string));
        dtrecords.Columns.Add("PlyType", typeof(string));
        dtrecords.Columns.Add("TransportType", typeof(string));
        dtrecords.Columns.Add("Moisture", typeof(float));
        dtrecords.Columns.Add("RecMoisture", typeof(float));
        dtrecords.Columns.Add("PenalityRate", typeof(float));
        dtrecords.Columns.Add("PenalityAmt", typeof(float));
        dtrecords.Columns.Add("BellWt", typeof(float));

        //*******************
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtrecqty = ((TextBox)DG.Rows[i].FindControl("txtrecqty"));
            TextBox txtlossqty = ((TextBox)DG.Rows[i].FindControl("txtlossqty"));
            DropDownList DDGodown = ((DropDownList)DG.Rows[i].FindControl("DDGodown"));
            DropDownList DDBinNo = ((DropDownList)DG.Rows[i].FindControl("DDBinNo"));
            TextBox txtnoofcone = ((TextBox)DG.Rows[i].FindControl("txtnoofcone"));
            TextBox TxtMoisture = ((TextBox)DG.Rows[i].FindControl("TxtMoisture"));
            TextBox TxtRecMoisture = ((TextBox)DG.Rows[i].FindControl("TxtRecMoisture"));
            TextBox TxtPenalityRate = (TextBox)DG.Rows[i].FindControl("txtpenalityrate");
            TextBox TxtBellWt = (TextBox)DG.Rows[i].FindControl("TxtBellWt");

            if (Chkboxitem.Checked == true && TxtMoisture.Text == "" && (Session["varcompanyNo"].ToString() == "16" || Session["varcompanyNo"].ToString() == "28"))
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please fill moisture text box');", true);
                return;
            }

            //if (Chkboxitem.Checked == true && (txtrecqty.Text != "" || txtlossqty.Text != "") && Convert.ToInt16(txtnoofcone.Text == "" ? "0" : txtnoofcone.Text) > 0 && DDGodown.SelectedIndex != -1)
            if (Chkboxitem.Checked == true && (txtrecqty.Text != "" || txtlossqty.Text != "") && DDGodown.SelectedIndex != -1 && (variable.VarBINNOWISE == "1" ? DDBinNo.SelectedIndex > 0 : DDBinNo.SelectedIndex <= 0))
            {
                Label lblitemfinishedid = ((Label)DG.Rows[i].FindControl("lblitemfinishedid"));
                Label lblunitid = ((Label)DG.Rows[i].FindControl("lblunitid"));
                Label lblflagsize = ((Label)DG.Rows[i].FindControl("lblflagsize"));
                Label lblLotno = ((Label)DG.Rows[i].FindControl("lblLotno"));
                Label lblTagno = ((Label)DG.Rows[i].FindControl("lblTagno"));
                DropDownList DDRecType = ((DropDownList)DG.Rows[i].FindControl("DDRecType"));

                DropDownList DDconetype = ((DropDownList)DG.Rows[i].FindControl("DDconetype"));
                Label lblissuemasterid = ((Label)DG.Rows[i].FindControl("lblissuemasterid"));
                Label lblissuemasterdetailid = ((Label)DG.Rows[i].FindControl("lblissuemasterdetailid"));
                Label lblrate = ((Label)DG.Rows[i].FindControl("lblrate"));

                DropDownList DDPlyType = ((DropDownList)DG.Rows[i].FindControl("DDPlyType"));
                DropDownList DDTransportType = ((DropDownList)DG.Rows[i].FindControl("DDTransportType"));
                Label LblIsuedQty = (Label)DG.Rows[i].FindControl("lblisuedQty");

                //**********Data Row
                DataRow dr = dtrecords.NewRow();
                
                Decimal PenalityAmt = 0;
                PenalityAmt = Convert.ToDecimal(Convert.ToDecimal(LblIsuedQty.Text) * Convert.ToDecimal((TxtPenalityRate.Text == "" ? "0" : TxtPenalityRate.Text)));

                dr["Item_Finished_id"] = lblitemfinishedid.Text;
                dr["Unitid"] = lblunitid.Text;
                dr["flagsize"] = lblflagsize.Text;
                dr["Godownid"] = DDGodown.SelectedValue;
                dr["Lotno"] = lblLotno.Text;
                dr["TagNo"] = lblTagno.Text;
                dr["RecQty"] = txtrecqty.Text == "" ? "0" : txtrecqty.Text;
                dr["LossQty"] = txtlossqty.Text == "" ? "0" : txtlossqty.Text;
                dr["Rectype"] = DDRecType.SelectedItem.Text;
                dr["Noofcone"] = txtnoofcone.Text == "" ? "0" : txtnoofcone.Text;
                dr["Conetype"] = DDconetype.SelectedItem.Text;
                dr["Issuemasterid"] = lblissuemasterid.Text;
                dr["IssuemasterDetailid"] = lblissuemasterdetailid.Text;
                dr["Rate"] = lblrate.Text;
                dr["BinNo"] = DDBinNo.SelectedIndex > 0 ? DDBinNo.SelectedItem.Text : "";
                dr["PlyType"] = DDPlyType.SelectedItem.Text;
                dr["TransportType"] = DDTransportType.SelectedItem.Text;
                dr["Moisture"] = TxtMoisture.Text == "" ? "0" : TxtMoisture.Text;
                dr["RecMoisture"] = TxtRecMoisture.Text == "" ? "0" : TxtRecMoisture.Text;
                dr["PenalityRate"] = TxtPenalityRate.Text == "" ? "0" : TxtPenalityRate.Text;
                dr["PenalityAmt"] = PenalityAmt;
                dr["BellWt"] = TxtBellWt.Text == "" ? "0" : TxtBellWt.Text;
                
                dtrecords.Rows.Add(dr);
            }
        }
        if (dtrecords.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[13];
                param[0] = new SqlParameter("@id", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = 0;
                param[1] = new SqlParameter("@dtrecords", dtrecords);
                param[2] = new SqlParameter("@companyId", DDcompany.SelectedValue);
                param[3] = new SqlParameter("@vendorid", DDvendor.SelectedValue);
                param[4] = new SqlParameter("@receiveNo", txtreceiveNo.Text);
                param[5] = new SqlParameter("@RecDate", txtRecdate.Text);
                param[6] = new SqlParameter("@Issuemasterid", DDIssueno.SelectedValue);
                param[7] = new SqlParameter("@userid", Session["varuserid"]);
                param[8] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[8].Direction = ParameterDirection.Output;
                param[9] = new SqlParameter("@Empid", DDemployee.SelectedValue);
                param[10] = new SqlParameter("@BinNo", TDBinNo.Visible == true ? DDBinNo.SelectedItem.Text : "");
                param[11] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);
                param[12] = new SqlParameter("@PartyChallanNo", TxtPartyChallanNo.Text);

                //*******************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveYarnReceive", param);
                //*******************
                ViewState["reportid"] = param[0].Value.ToString();
                lblmessage.Text = param[8].Value.ToString();
                Tran.Commit();
                FillIssueDetail();
                FillReceiveDetails();
                Refreshcontrol();
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box or Godown Name " + (variable.VarBINNOWISE == "1" ? " or Bin No" : "") + " to save data.');", true);
        }

    }
    protected void Refreshcontrol()
    {
        //DDIssueno.SelectedIndex = -1;
        DDemployee.SelectedIndex = -1;
        txtreceiveNo.Text = "";
        txtRecdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        TxtPartyChallanNo.Text = "";
        // DG.DataSource = null;
        //DG.DataBind();
    }
    protected void FillReceiveDetails()
    {
        string str = @"select YM.ReceiveNo,dbo.F_getItemDescription(YT.Item_Finished_id,YT.flagsize) as ItemDescription,
                        U.UnitName,Gm.GodownName,YT.Lotno,YT.Tagno,yt.ReceiveQty,Yt.LossQty,YM.ID,yt.Detailid,YT.issuemasterid,YT.issuemasterDetailid,yt.rectype,yt.noofcone,yt.Conetype
                        ,isnull(YT.Rate,'') as Rate,isnull(YM.PartyChallanNo,'') as PartyChallanNo
                        from YarnOpeningreceiveMaster YM inner join YarnOpeningReceiveTran YT on 
                        YM.ID=YT.MasterId
                        inner join Unit U on YT.Unitid=U.UnitId
                        inner join GodownMaster GM on YT.GodownId=Gm.GoDownID Where YM.id=" + ViewState["reportid"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGReceivedDetail.DataSource = ds.Tables[0];
        DGReceivedDetail.DataBind();
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList DDGodown = ((DropDownList)e.Row.FindControl("DDGodown"));
            Label lblrectype = (Label)e.Row.FindControl("lblrectype");
            Label lblconetype = (Label)e.Row.FindControl("lblconetype");
            DropDownList DDRecType = (DropDownList)e.Row.FindControl("DDRecType");
            DropDownList DDconetype = (DropDownList)e.Row.FindControl("DDconetype");
            DropDownList DDBinNo = ((DropDownList)e.Row.FindControl("DDBinNo"));


            Label lblPlyType = (Label)e.Row.FindControl("lblplytype");
            DropDownList DDPlyType = (DropDownList)e.Row.FindControl("DDPlyType");
            Label lblTransportType = (Label)e.Row.FindControl("lbltransporttype");
            DropDownList DDTransportType = (DropDownList)e.Row.FindControl("DDTransportType");

            if (DDRecType.Items.FindByText(lblrectype.Text) != null)
            {
                DDRecType.SelectedValue = lblrectype.Text;
            }
            if (DDconetype.Items.FindByText(lblconetype.Text) != null)
            {
                DDconetype.SelectedValue = lblconetype.Text;
            }
            if (DDPlyType.Items.FindByText(lblPlyType.Text) != null)
            {
                DDPlyType.SelectedValue = lblPlyType.Text;
            }
            if (DDTransportType.Items.FindByText(lblTransportType.Text) != null)
            {
                DDTransportType.SelectedValue = lblTransportType.Text;
            }

            if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 16 || Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 28)
            {
                DDconetype.Enabled = false;
                DDPlyType.Enabled = false;
                DDTransportType.Enabled = false;
            }

            string str = @"Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName
                            select godownid From Modulewisegodown Where ModuleName='" + Page.Title + @"'
                            Select ConeType, ConeType From ConeMaster Order By SrNo ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDconetype, ds, 2, false, "");

            UtilityModule.ConditionalComboFillWithDS(ref DDGodown, ds, 0, false, "");
            if (ds.Tables[1].Rows.Count > 0)
            {
                if (DDGodown.Items.FindByValue(ds.Tables[1].Rows[0]["godownid"].ToString()) != null)
                {
                    DDGodown.SelectedValue = ds.Tables[1].Rows[0]["godownid"].ToString();
                    if (variable.VarBINNOWISE == "1")
                    {
                        if (variable.VarCHECKBINCONDITION == "1")
                        {
                            UtilityModule.FillBinNO(DDBinNo, Convert.ToInt32(DDGodown.SelectedValue), Item_finished_id: 0, New_Edit: 0);
                        }
                        else
                        {
                            UtilityModule.ConditionalComboFill(ref DDBinNo, "SELECT DISTINCT BINNO,BINNO AS BINNO1 FROM STOCK S WHERE GODOWNID=" + DDGodown.SelectedValue + "  ORDER BY BINNO1", true, "--Plz Select--");
                        }
                    }
                }
            }
            //**************COLUMN BIN NO VISIBLE TRUE OR FALSE
            for (int i = 0; i < DG.Columns.Count; i++)
            {
                if (DG.Columns[i].HeaderText.ToUpper() == "BIN NO")
                {
                    if (variable.VarBINNOWISE == "1")
                    {
                        DG.Columns[i].Visible = true;
                    }
                    else
                    {
                        DG.Columns[i].Visible = false;
                    }
                }
                if (DG.Columns[i].HeaderText.ToUpper() == "MOISTURE")
                {
                    if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 16 || Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 28)
                    {
                        DG.Columns[i].Visible = true;
                    }
                    else
                    {
                        DG.Columns[i].Visible = false;
                    }
                }
            }
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string str = @"select  * from [V_yarnOpeningReceive] Where id=" + ViewState["reportid"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "14":
                    Session["rptFileName"] = "~\\Reports\\rptyarnopeningReceive.rpt";
                    break;
                default:
                    Session["rptFileName"] = "~\\Reports\\rptyarnopeningReceivewithrate.rpt";
                    break;
            }
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptyarnopeningReceive.xsd";
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
    protected void Chkedit_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkedit.Checked == true)
        {
            TDIssueNo.Visible = false;
            TDReceiveNo.Visible = true;
            DG.DataSource = null;
            DG.DataBind();
        }
        else
        {
            TDIssueNo.Visible = true;
            TDReceiveNo.Visible = false;
            DGReceivedDetail.DataSource = null;
            DGReceivedDetail.DataBind();
        }
    }
    protected void DDemployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillReceiveNo();
    }
    protected void fillReceiveNo()
    {
        string str;
        str = @"select Distinct YRM.Id,ReceiveNo+'/'+REPLACE(CONVERT(nvarchar(11),Receivedate,106),' ','-') as Receivedate  from YarnOpeningReceiveMaster YRM 
        inner Join YarnOpeningReceiveTran YRT on YRM.ID=YRT.MasterId inner join YarnOpeningIssueMaster YIM on YRT.issuemasterid=YIM.ID  
        Where YRM.CompanyId=" + DDcompany.SelectedValue + " And YRM.BranchID = " + DDBranchName.SelectedValue + " And YRM.Empid=" + DDemployee.SelectedValue + " and Yrm.vendorid=" + DDvendor.SelectedValue + " ";
        if (chkcompleteissueno.Checked == true)
        {
            str = str + " And YIM.status='Complete' ";
        }
        else
        {
            str = str + "  And YIM.status='Pending' ";
        }
        str = str + " order by YRM.ID desc";
        UtilityModule.ConditionalComboFill(ref DDReceiveNo, str, true, "--Plz Select--");
    }
    protected void DDReceiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["reportid"] = DDReceiveNo.SelectedValue;
        FillReceiveDetails();
    }
    protected void DGReceivedDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            string masterid = ((Label)DGReceivedDetail.Rows[e.RowIndex].FindControl("lblid")).Text;
            string Detailid = ((Label)DGReceivedDetail.Rows[e.RowIndex].FindControl("lbldetailid")).Text;
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@Id", masterid);
            param[1] = new SqlParameter("@Detailid", Detailid);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            param[4] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            //************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteYarnOpeningReceive", param);
            Tran.Commit();
            lblmessage.Text = param[2].Value.ToString();
            FillReceiveDetails();

        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
    }
    protected void chkcomplete_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcomplete.Checked == true)
        {
            if (DDIssueno.SelectedIndex <= 0)
            {
                lblmessage.Text = "Please select Issue No.";
                return;
            }
            DG.DataSource = null;
            DG.DataBind();
            ModalPopupExtender1.Show();
            string str = @"select dbo.F_getItemDescription(YT.Item_Finished_id,YT.flagsize) as ItemDescription,yt.IssueQty-isnull(vrt.retqty,0) as Issueqty,
                            isnull(YR.ReceivedQty,0) As ReceivedQty,
                            YT.Item_Finished_id,YM.ID,yt.Detailid,Round(case When ((Yt.issueqty-isnull(vrt.retqty,0))-Isnull(YR.ReceivedQty,0))>0 
                            Then ((Yt.issueqty-isnull(VRT.Retqty,0))-Isnull(YR.ReceivedQty,0)) ELse 0 End,3) as LossQty,
                            Round(case When ((Yt.issueqty-isnull(vrt.retqty,0))-Isnull(YR.ReceivedQty,0))<=0 Then (Isnull(YR.ReceivedQty,0)-(Yt.issueqty-isnull(vrt.retqty,0))) ELse 0 End,3) as GainQty
                            from YarnOpeningIssueMaster YM 
                            inner join YarnOpeningIssueTran YT on YM.ID=YT.MasterId
                            inner join Unit U on YT.Unitid=U.UnitId
                            left join V_getYarnOpeningReceivedQty YR on Yt.Detailid=YR.issuemasterDetailid and YT.MasterId=YR.issuemasterid 
                            left join V_getYarnOpeningReturnQty VRT on YT.detailid=VRT.Issuedetailid 
                            Where YM.Status='Pending' and YM.Id=" + DDIssueno.SelectedValue;

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            DGGridForComp.DataSource = ds.Tables[0];
            DGGridForComp.DataBind();

        }
        //else
        //{
        //    FillIssueno();
        //}
    }
    protected void btncomp_Click(object sender, EventArgs e)
    {
        string strinsert = "";
        string Id, Detailid, GainQty, LossQty, str;
        Id = "";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //********Check Pending 
            str = @"select Distinct Yt.MasterId From YarnOpeningIssueTran YT Left join YarnOpeningReceiveTran YRT on YT.Detailid=YRT.issuemasterDetailid
             Where  YT.MasterId=" + DDIssueno.SelectedValue + " and YRT.issuemasterDetailid is null ";
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altr", "alert('All shade must be final to Complete status.')", true);
                ModalPopupExtender1.Show();
                Tran.Commit();
                return;
            }
            //
            for (int i = 0; i < DGGridForComp.Rows.Count; i++)
            {
                Id = ((Label)DGGridForComp.Rows[i].FindControl("lblId")).Text;
                Detailid = ((Label)DGGridForComp.Rows[i].FindControl("lblDetailId")).Text;
                GainQty = ((Label)DGGridForComp.Rows[i].FindControl("lblgainQty")).Text;
                LossQty = ((Label)DGGridForComp.Rows[i].FindControl("lblLossqty")).Text;
                if (i == 0)
                {
                    strinsert = strinsert + " Delete from YarnOpeningCompleteStatus Where IssueId=" + Id;
                }
                strinsert = strinsert + " Insert into YarnOpeningCompleteStatus(IssueId,DetailId,GainQty,LossQty,Userid)values(" + Id + "," + Detailid + "," + GainQty + "," + LossQty + "," + Session["varuserid"] + ")";
            }
            if (strinsert != "")
            {
                strinsert = strinsert + " update yarnOpeningIssuemaster set Status='Complete' where Id=" + Id + "";
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, strinsert);
            }
            Tran.Commit();
            lblmessage.Text = "Issue No. Compeleted Successfully.";
            FillIssueno();
            chkcomplete.Checked = false;
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
    protected void txtLotno_TextChanged(object sender, EventArgs e)
    {
        FillIssueno();
    }
    protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    {
        string str = @"select Empid From EmpInfo EI inner join Department Dp on EI.Departmentid=DP.departmentid 
                       and Dp.DepartmentName in('Yarn Opening','WEFT DEPARTMENT') Where EmpCode='" + txtWeaverIdNoscan.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (DDvendor.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) != null)
            {
                DDvendor.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
                DDvendor_SelectedIndexChanged(sender, new EventArgs());
                DDIssueno.Focus();
                if (Session["varcompanyNo"].ToString() == "16")
                {
                    if (DDemployee.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) != null)
                    {
                        DDemployee.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
                    }

                }
                txtWeaverIdNoscan.Text = "";
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('Employee Code does not exists in this Department.')", true);
            txtWeaverIdNoscan.Focus();
        }
    }
    protected void chkcompleteissueno_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcompleteissueno.Checked == true)
        {
            if (TDReceiveNo.Visible == true)
            {
                fillReceiveNo();
            }
            DG.DataSource = null;
            DG.DataBind();
        }

    }
    protected void DDGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (variable.VarCHECKBINCONDITION == "1")
        {
            UtilityModule.FillBinNO(DDBinNo, Convert.ToInt32(DDGodown.SelectedValue), Item_finished_id: 0, New_Edit: 0);
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDBinNo, "SELECT DISTINCT BINNO,BINNO AS BINNO1 FROM STOCK S WHERE GODOWNID=" + DDGodown.SelectedValue + "  ORDER BY BINNO1", true, "--Plz Select--");
        }
    }
    protected void DDGodownDG_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList DDGodown = (DropDownList)sender;
        GridViewRow gvr = (GridViewRow)DDGodown.Parent.Parent;

        DropDownList DDBinNo = (DropDownList)gvr.FindControl("DDBinNo");

        if (variable.VarBINNOWISE == "1")
        {
            if (variable.VarCHECKBINCONDITION == "1")
            {
                UtilityModule.FillBinNO(DDBinNo, Convert.ToInt32(DDGodown.SelectedValue), Item_finished_id: 0, New_Edit: 0);
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDBinNo, "SELECT DISTINCT BINNO,BINNO AS BINNO1 FROM STOCK S WHERE GODOWNID=" + DDGodown.SelectedValue + "  ORDER BY BINNO1", true, "--Plz Select--");
            }
        }
    }
    protected void DDdept_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @" select EI.EmpId,EI.EmpName + CASE WHEN EI.EMPCODE<>'' THEN ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME from empinfo EI inner join Department D 
                           on EI.departmentId=D.DepartmentId Where isnull(Ei.Blacklist,0)=0 and D.departmentid=" + DDdept.SelectedValue + " order by EmpName  ";
        UtilityModule.ConditionalComboFill(ref DDvendor, str, false, "");
        if (DDvendor.Items.Count > 0)
        {
            DDvendor_SelectedIndexChanged(sender, new EventArgs());
        }

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
    protected void lnkeditrate_Click(object sender, EventArgs e)
    {
        Popup(true);
        txtpwd.Focus();
        LinkButton lnkeditrate = sender as LinkButton;
        GridViewRow gvr = lnkeditrate.NamingContainer as GridViewRow;
        rowindex = gvr.RowIndex;
    }
    protected void txtpwd_TextChanged(object sender, EventArgs e)
    {
        lblmessage.Text = "";
        if (variable.VarMOTTELINGEDITPWD == txtpwd.Text)
        {
            UpdateRate(rowindex);
            Popup(false);
        }
        else
        {
            lblmessage.Text = "Please Enter Correct Password..";
        }
    }
    protected void UpdateRate(int rowindex)
    {
        lblmessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblDetailid = (Label)DGReceivedDetail.Rows[rowindex].FindControl("lblDetailid");
            Label lblid = (Label)DGReceivedDetail.Rows[rowindex].FindControl("lblid");
            TextBox lblrate = (TextBox)DGReceivedDetail.Rows[rowindex].FindControl("lblrate");


            SqlParameter[] arr = new SqlParameter[8];
            arr[0] = new SqlParameter("@Detailid", lblDetailid.Text);
            arr[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[1].Direction = ParameterDirection.Output;
            arr[2] = new SqlParameter("@ID", lblid.Text);
            arr[3] = new SqlParameter("@userid", Session["varuserid"]);
            arr[4] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            arr[5] = new SqlParameter("@Rate", lblrate.Text == "" ? "0" : lblrate.Text);
            //***********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_UPDATEYARNOPENINGRECRATE]", arr);
            lblmessage.Text = arr[1].Value.ToString();
            Tran.Commit();
            FillReceiveDetails();
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
}