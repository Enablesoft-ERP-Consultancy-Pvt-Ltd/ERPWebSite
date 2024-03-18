using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Motteling_frmmottelingreceive : System.Web.UI.Page
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
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName 
                           select PROCESS_NAME_ID,Process_name From process_name_master  where Process_Name in('MOTTELING','HAND SPINNING','YARN OPENING+MOTTELING', 'HANK MAKING', 'WRAPPING') order by PROCESS_NAME 
                           Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName
                           select Godownid From ModuleWiseGodown Where ModuleName='" + Page.Title + @"' 
                           Select ID, BranchName 
                                From BRANCHMASTER BM(nolock) 
                                JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                                Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");
            
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 4, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Plz Select--");
            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedIndex = 1;
                DDProcessName_SelectedIndexChanged(sender, new EventArgs());

            }
            UtilityModule.ConditionalComboFillWithDS(ref DDgodown, ds, 2, true, "--Plz Select--");
            if (ds.Tables[3].Rows.Count > 0)
            {
                if (DDgodown.Items.FindByValue(ds.Tables[3].Rows[0]["godownid"].ToString()) != null)
                {
                    DDgodown.SelectedValue = ds.Tables[3].Rows[0]["godownid"].ToString();
                    DDgodown_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            txtrecdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (Session["canedit"].ToString() == "1")
            {
                TRedit.Visible = true;
            }
            //********

            if (variable.VarBINNOWISE == "1")
            {
                TDBinNo.Visible = true;
            }
            //**********
            if (variable.Carpetcompany == "1")
            {
                TRempcodescan.Visible = true;
            }
            if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 16 || Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 28)
            {
                BtnComplete.Visible = true;
            }
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = "0";
        txtchallanNo.Text = "";
        UtilityModule.ConditionalComboFill(ref DDPartyName, "Select EI.EmpId,EI.EmpName+case when Isnull(Ei.Empcode,'')='' Then '' Else '['+EI.Empcode+']'  End as Empname From EmpInfo EI inner join EmpProcess EP on EI.EmpId=EP.EmpId and EP.ProcessId=" + DDProcessName.SelectedValue + " order by empname", true, "--Plz Select--");
    }
    protected void DDgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TDBinNo.Visible == true)
        {
            if (variable.VarCHECKBINCONDITION == "1")
            {
                UtilityModule.FillBinNO(DDBinNo, Convert.ToInt32(DDgodown.SelectedValue), Item_finished_id: 0, New_Edit: 0);
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDBinNo, "select BINNO,BINNO From BinMaster Where Godownid=" + DDgodown.SelectedValue + " order by BINID", true, "--Plz Select--");
            }
        }
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = "0";
        txtchallanNo.Text = "";
        FillIndentNo();
    }
    protected void FillIndentNo()
    {
        string str = @"select ID,IssueNo 
        from Mottelingissuemaster 
        Where companyid=" + DDCompanyName.SelectedValue + " And BranchID = " + DDBranchName.SelectedValue + " and processid=" + DDProcessName.SelectedValue + @" and 
        empid=" + DDPartyName.SelectedValue;

        if (chkcomplete.Checked == true)
        {
            str = str + " and Status='Complete'";
        }
        else
        {
            str = str + " and Status='Pending'";
        }
        str = str + " order by ID desc";
        UtilityModule.ConditionalComboFill(ref DDissueno, str, true, "--Plz Select--");
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        chkcomplete.Checked = false;
        DDpartychallan.SelectedIndex = -1;
        DDissueno.Items.Clear();
        DDpartychallan.Items.Clear();
        TDPartychallan.Visible = false;
        //DGrecdetail.DataSource = null;
        //DGrecdetail.DataBind();
        //DG.DataSource = null;
        //DG.DataBind();
        hnid.Value = "0";
        txtgateinno.Text = "";
        txtchallanNo.Text = "";
        txtrecdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        chkcomplete.Visible = false;
        if (chkedit.Checked == true)
        {
            TDPartychallan.Visible = true;
            chkcomplete.Visible = true;
        }
    }
    protected void BindIssueGrid()
    {
        if (chkedit.Checked == false)
        {
            FillGridReceive();
            
        }
        else
        {
            string str = @"select Distinct SRM.ID,SRM.ChallanNo From MOTTELINGRECEIVEMASTER SRM inner join MOTTELINGRECEIVEDETAIL SRD on SRM.ID=SRD.Masterid
                                     inner join MOTTELINGISSUEMASTER SM on SRD.issueid=SM.ID Where SRM.companyid=" + DDCompanyName.SelectedValue + " and SRM.Processid=" + DDProcessName.SelectedValue + " and SRM.empid=" + DDPartyName.SelectedValue + " and SM.ID=" + DDissueno.SelectedValue + " order by srm.ID desc";
            UtilityModule.ConditionalComboFill(ref DDpartychallan, str, true, "--Plz Select--");
        }
    }

    protected void DDissueno_SelectedIndexChanged(object sender, EventArgs e)
    {

        BindIssueGrid();

        if (chkedit.Checked == false)
        {
            hnid.Value = "0";
            txtchallanNo.Text = "";
        }

    }

    private void FillGridReceive()
    {
        string str = @"select dbo.F_getItemDescription(MD.Rfinishedid,MD.Rflagsize) as RItemdescription,
                        MD.unitid,dbo.F_GETMOTTELINGRECLOTNO(MM.id,MD.Rfinishedid,MD.unitid) as Lotno,
                        dbo.F_GETMOTTELINGRECTAGNO(MM.id,MD.Rfinishedid,MD.unitid) as TAGNO,
                        Isnull(SUM(MD.issueqty-isnull(vr.retqty,0)),0) as Issuedqty,
                        isnull(DBO.F_GETMOTTELINGRECDQTY(MM.ID,MD.RFINISHEDID,MD.Unitid, MM.ProcessID, MD.CONETYPE, MD.PlyType, MD.TransportType, MD.Moisture),0) as Receivedqty,
                        MD.Rate,MM.ID as Issueid,0 as IssueDetailid,MD.Rfinishedid,MD.Rflagsize,
                        md.Conetype,vf.item_id,vf.qualityid,vf.shadecolorid,MM.Remark, MD.PlyType, MD.TransportType, MD.Moisture 
                        From Mottelingissuemaster MM 
                        inner join MOTTELINGISSUEDETAIL MD on MM.ID=MD.masterid
                        inner join V_finisheditemdetail vf on MD.Rfinishedid=vf.item_finished_id
                        LEFT JOIN V_GETMOTTELINGRETURNQTY VR ON MD.DETAILID=VR.ISSUEDETAILID 
                        WHere MM.ID=" + DDissueno.SelectedValue + @" 
                        group by MM.ID,MD.Rfinishedid,MD.Rflagsize,MD.unitid,MD.Rate,md.Conetype, vf.item_id,vf.qualityid,vf.shadecolorid,MM.Remark,MM.ProcessID, 
                        MD.PlyType, MD.TransportType, MD.Moisture";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
        txtissuedremark.Text = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtissuedremark.Text = ds.Tables[0].Rows[0]["remark"].ToString();
        }
    }
    protected void DGrecdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList DDConetype = (DropDownList)e.Row.FindControl("DDConetype");

            //TextBox lblrate = (TextBox)e.Row.FindControl("lblrate");

            //if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 16 || Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 28)
            //{
            //    lblrate.Enabled = false;
            //}
            string str = @"Select ConeType, ConeType From ConeMaster Order By SrNo ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDConetype, ds, 0, false, "");
        }
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            DropDownList DDConetype = (DropDownList)e.Row.FindControl("DDConetype");  
        
            string str = @"Select ConeType, ConeType From ConeMaster Order By SrNo ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDConetype, ds, 0, false, "");

            Label lblconetype = (Label)e.Row.FindControl("lblconetype");
            DropDownList DDconetype = (DropDownList)e.Row.FindControl("DDconetype");

            Label lblPlyType = (Label)e.Row.FindControl("lblPlyType");
            DropDownList DDPlyType = (DropDownList)e.Row.FindControl("DDPly");

            Label lblTransportType = (Label)e.Row.FindControl("lblTransportType");
            DropDownList DDTransportType = (DropDownList)e.Row.FindControl("DDTransport");

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

            for (int i = 0; i < DG.Columns.Count; i++)
            {
                if ((DG.Columns[i].HeaderText.ToUpper() == "CONE TYPE" || DG.Columns[i].HeaderText.ToUpper() == "NO OF CONE") && DDProcessName.SelectedItem.Text.ToUpper() == "YARN OPENING+MOTTELING")
                {
                    DG.Columns[i].Visible = true;
                }
                else
                {
                    if ((DG.Columns[i].HeaderText.ToUpper() == "CONE TYPE" || DG.Columns[i].HeaderText.ToUpper() == "NO OF CONE"))
                    {
                        DG.Columns[i].Visible = false;
                    }
                }
            }
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        //**********Data table
        DataTable dtrecord = new DataTable();
        dtrecord.Columns.Add("Rfinishedid", typeof(int));
        dtrecord.Columns.Add("godownid", typeof(int));
        dtrecord.Columns.Add("lotno", typeof(string));
        dtrecord.Columns.Add("Tagno", typeof(string));
        dtrecord.Columns.Add("Rate", typeof(float));
        dtrecord.Columns.Add("Recqty", typeof(float));
        dtrecord.Columns.Add("Lossqty", typeof(float));
        dtrecord.Columns.Add("issuedetailid", typeof(int));
        dtrecord.Columns.Add("unitid", typeof(int));
        dtrecord.Columns.Add("Rflagsize", typeof(int));
        dtrecord.Columns.Add("Undyedqty", typeof(float));
        dtrecord.Columns.Add("Ifinishedid", typeof(int));
        dtrecord.Columns.Add("Conetype", typeof(string));
        dtrecord.Columns.Add("NoofCone", typeof(int));
        dtrecord.Columns.Add("Penalityamt", typeof(float));
        dtrecord.Columns.Add("Penalityrate", typeof(float));
        dtrecord.Columns.Add("PlyType", typeof(string));
        dtrecord.Columns.Add("TransportType", typeof(string));
        dtrecord.Columns.Add("Moisture", typeof(string));
        dtrecord.Columns.Add("RecMoisture", typeof(string));
        dtrecord.Columns.Add("BellWt", typeof(float));

        //************
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            Label lblrfinishedid = (Label)DG.Rows[i].FindControl("lblrfinishedid");
            // DropDownList DDgodown = (DropDownList)DG.Rows[i].FindControl("DDgodown");
            Label lblrlotno = (Label)DG.Rows[i].FindControl("lblrlotno");
            Label lblrtagno = (Label)DG.Rows[i].FindControl("lblrtagno");
            Label lblrate = (Label)DG.Rows[i].FindControl("lblrate");
            TextBox txtrecqty = (TextBox)DG.Rows[i].FindControl("txtrecqty");
            TextBox txtlossqty = (TextBox)DG.Rows[i].FindControl("txtlossqty");
            Label lblissuedetailid = (Label)DG.Rows[i].FindControl("lblissuedetailid");
            Label lblunitid = (Label)DG.Rows[i].FindControl("lblunitid");
            Label lblrflagsize = (Label)DG.Rows[i].FindControl("lblrflagsize");
            DropDownList DDconetype = (DropDownList)DG.Rows[i].FindControl("DDConetype");
            TextBox txtnoofcone = (TextBox)DG.Rows[i].FindControl("txtnoofcone");
            Label txtpenalityamt = (Label)DG.Rows[i].FindControl("txtpenalityamt");
            TextBox txtpenalityrate = (TextBox)DG.Rows[i].FindControl("txtpenalityrate");
            Label lblissqty = (Label)DG.Rows[i].FindControl("lblissqty");
            DropDownList DDPlyType = (DropDownList)DG.Rows[i].FindControl("DDPly");
            DropDownList DDTransportType = (DropDownList)DG.Rows[i].FindControl("DDTransport");
            TextBox TxtMoisture = (TextBox)DG.Rows[i].FindControl("TxtMoisture");
            TextBox TxtRecMoisture = (TextBox)DG.Rows[i].FindControl("TxtRecMoisture");
            TextBox TxtBellWt = (TextBox)DG.Rows[i].FindControl("TxtBellWt");

            Decimal penalityamt = 0;
            penalityamt = Convert.ToDecimal(Convert.ToDecimal(lblissqty.Text) * Convert.ToDecimal((txtpenalityrate.Text == "" ? "0" : txtpenalityrate.Text)));

            //check rec qty and loss qty
            if (DDgodown.SelectedIndex > 0 && (Convert.ToDouble(txtrecqty.Text == "" ? "0" : txtrecqty.Text) > 0 || Convert.ToDouble(txtlossqty.Text == "" ? "0" : txtlossqty.Text) > 0))
            {
                DataRow dr = dtrecord.NewRow();
                dr["Rfinishedid"] = lblrfinishedid.Text;
                dr["godownid"] = DDgodown.SelectedValue;
                dr["Lotno"] = lblrlotno.Text;
                dr["TagNo"] = lblrtagno.Text;
                dr["Rate"] = lblrate.Text == "" ? "0" : lblrate.Text;
                dr["Recqty"] = txtrecqty.Text == "" ? "0" : txtrecqty.Text;
                dr["Lossqty"] = txtlossqty.Text == "" ? "0" : txtlossqty.Text;
                dr["issuedetailid"] = lblissuedetailid.Text;
                dr["unitid"] = lblunitid.Text;
                dr["Rflagsize"] = lblrflagsize.Text;
                dr["undyedqty"] = 0;
                dr["Ifinishedid"] = 0;
                if (DDProcessName.SelectedItem.Text.ToUpper() == "YARN OPENING+MOTTELING")
                {
                    dr["Conetype"] = DDconetype.SelectedItem.Text;
                }
                else
                {
                    dr["Conetype"] = "";
                }
                dr["noofcone"] = txtnoofcone == null ? "0" : txtnoofcone.Text == "" ? "0" : txtnoofcone.Text;
                dr["Penalityamt"] = penalityamt;
                dr["Penalityrate"] = txtpenalityrate.Text == "" ? "0" : txtpenalityrate.Text;
                dr["PlyType"] = DDPlyType.SelectedItem.Text;
                dr["TransportType"] = DDTransportType.SelectedItem.Text;
                dr["Moisture"] = TxtMoisture == null ? "0" : TxtMoisture.Text == "" ? "0" : TxtMoisture.Text;
                dr["RecMoisture"] = TxtRecMoisture == null ? "0" : TxtRecMoisture.Text == "" ? "0" : TxtRecMoisture.Text;
                dr["BellWt"] = TxtBellWt.Text == "" ? "0" : TxtBellWt.Text;
                dtrecord.Rows.Add(dr);
            }
            //************
        }
        if (dtrecord.Rows.Count > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] arr = new SqlParameter[16];
                arr[0] = new SqlParameter("@ID", SqlDbType.Int);
                arr[0].Direction = ParameterDirection.InputOutput;
                arr[0].Value = hnid.Value;
                arr[1] = new SqlParameter("@companyid", DDCompanyName.SelectedValue);
                arr[2] = new SqlParameter("@Processid", DDProcessName.SelectedValue);
                arr[3] = new SqlParameter("@empid", DDPartyName.SelectedValue);
                arr[4] = new SqlParameter("@Indentid", DDissueno.SelectedValue);
                arr[5] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 50);
                arr[5].Direction = ParameterDirection.InputOutput;
                arr[5].Value = txtchallanNo.Text;
                arr[6] = new SqlParameter("@RecDate", txtrecdate.Text);
                arr[7] = new SqlParameter("@userid", Session["varuserid"]);
                arr[8] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
                arr[9] = new SqlParameter("@dtrecord", dtrecord);
                arr[10] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                arr[10].Direction = ParameterDirection.Output;
                arr[11] = new SqlParameter("@GateinNo", SqlDbType.VarChar, 100);
                arr[11].Direction = ParameterDirection.InputOutput;
                arr[11].Value = txtgateinno.Text;
                arr[12] = new SqlParameter("@CheckedBy", txtcheckedby.Text);
                arr[13] = new SqlParameter("@Approvedby", txtapprovedby.Text);
                arr[14] = new SqlParameter("@BinNo", TDBinNo.Visible == true ? DDBinNo.SelectedItem.Text : "");
                arr[15] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);

                //*******************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_MOTTELINGRECEIVE", arr);

                hnid.Value = arr[0].Value.ToString();
                txtchallanNo.Text = arr[5].Value.ToString();
                txtgateinno.Text = arr[11].Value.ToString();

                if (arr[10].Value.ToString() != "")
                {
                    lblmsg.Text = arr[10].Value.ToString();
                    Tran.Rollback();
                }
                else
                {
                    Tran.Commit();
                    //DDissueno.SelectedIndex = -1;
                    //DG.DataSource = null;
                    //DG.DataBind();
                   // DDissueno_SelectedIndexChanged(sender, new EventArgs());
                    BindIssueGrid();
                    lblmsg.Text = "Data saved successfully.";
                    fillgrid();
                }
            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select Godown or Enter (Recqty or Loss qty) to save data.');", true);
        }
    }

    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = "select * from V_MOTTELINGRECEIVE where id= " + hnid.Value; 
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptMottelingreceive.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptMottelingreceive.xsd";
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
    protected void fillgrid()
    {
        string str = @"select SRM.ID,SRD.Detailid,dbo.F_getItemDescription(srd.Rfinishedid,SRD.Rflagsize) as RItemdescription,
                        gm.GodownName,SRD.LotNo,SRD.TagNo,SRD.Recqty,SRD.Lossqty,SRD.Rate,isnull(SRM.GateinNo,'') as GateinNo,SRM.challanNo,SRM.receiveDate,srd.Undyedqty,
                        isnull(Srm.Checkedby,'') as Checkedby,isnull(Srm.Approvedby,'') as Approvedby,Srd.Penalityamt,srd.Penalityrate
                        From MOTTELINGRECEIVEMASTER SRM inner join MOTTELINGRECEIVEDETAIL SRD on SRM.ID=SRD.Masterid
                        inner join GodownMaster gm on SRD.godownid=gm.GoDownID Where SRM.id=" + hnid.Value;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        DGrecdetail.DataSource = ds.Tables[0];
        DGrecdetail.DataBind();
        if (chkedit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {

                txtgateinno.Text = ds.Tables[0].Rows[0]["gateinno"].ToString();
                txtchallanNo.Text = ds.Tables[0].Rows[0]["challanNo"].ToString();
                txtrecdate.Text = ds.Tables[0].Rows[0]["ReceiveDate"].ToString();
                txtcheckedby.Text = ds.Tables[0].Rows[0]["checkedby"].ToString();
                txtapprovedby.Text = ds.Tables[0].Rows[0]["approvedby"].ToString();

            }
            else
            {
                txtgateinno.Text = "";
                txtchallanNo.Text = "";
                txtrecdate.Text = "";
                txtcheckedby.Text = "";
                txtapprovedby.Text = "";
            }
        }
    }
    protected void DGrecdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblDetailid = (Label)DGrecdetail.Rows[e.RowIndex].FindControl("lblDetailid");
            Label lblid = (Label)DGrecdetail.Rows[e.RowIndex].FindControl("lblid");
            SqlParameter[] arr = new SqlParameter[5];
            arr[0] = new SqlParameter("@Detailid", lblDetailid.Text);
            arr[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[1].Direction = ParameterDirection.Output;
            arr[2] = new SqlParameter("@ID", lblid.Text);
            arr[3] = new SqlParameter("@userid", Session["varuserid"]);
            arr[4] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            //***********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEMOTTELINGRECEIVE", arr);
            lblmsg.Text = arr[1].Value.ToString();
            Tran.Commit();
            fillgrid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }

    }
    protected void DGrecdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    protected void DGrecdetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGrecdetail.EditIndex = e.NewEditIndex;
        fillgrid();
    }
    protected void DGrecdetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGrecdetail.EditIndex = -1;
        fillgrid();
    }
    protected void DDpartychallan_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = DDpartychallan.SelectedValue;
        fillgrid();
    }
    protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    {
        string str = "select empid From Empinfo Where empcode='" + txtWeaverIdNoscan.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
           
            if (DDPartyName.Items.FindByValue(ds.Tables[0].Rows[0]["empid"].ToString()) != null)
            {
                DDPartyName.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
                DDPartyName_SelectedIndexChanged(sender, new EventArgs());
            }
        }
    }
    protected void DDConetype_SelectedIndexchanges(object sender, EventArgs e)
    {
        DropDownList DDConetype = sender as DropDownList;

        if (DDConetype != null)
        {
            GridViewRow gvr = DDConetype.NamingContainer as GridViewRow;
            Label lblrate = (Label)gvr.FindControl("lblrate");
            Label lblritemid = (Label)gvr.FindControl("lblritemid");
            Label lblrqualityid = (Label)gvr.FindControl("lblrqualityid");
            Label lblrshadecolorid = (Label)gvr.FindControl("lblrshadecolorid");

            DropDownList DDPly = (DropDownList)gvr.FindControl("DDPly");
            DropDownList DDTransportType = (DropDownList)gvr.FindControl("DDTransport");

            lblrate.Text = UtilityModule.Getmottelingrate(lblritemid.Text, lblrqualityid.Text, DDProcessName.SelectedValue, DDPartyName.SelectedValue, txtrecdate.Text, shadecolorid: lblrshadecolorid.Text, Conetype: DDConetype.SelectedItem.Text, PlyType: DDPly.SelectedItem.Text, TransportType: DDTransportType.SelectedItem.Text).ToString();
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
        lblmsg.Text = "";
        if (variable.VarMOTTELINGEDITPWD == txtpwd.Text)
        {
            UpdateRate(rowindex);
            Popup(false);
        }
        else
        {
            lblmsg.Text = "Please Enter Correct Password..";
        }
    }
    protected void UpdateRate(int rowindex)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblDetailid = (Label)DGrecdetail.Rows[rowindex].FindControl("lblDetailid");
            Label lblid = (Label)DGrecdetail.Rows[rowindex].FindControl("lblid");
            TextBox lblrate = (TextBox)DGrecdetail.Rows[rowindex].FindControl("lblrate");
            Label txtpenalityamtrec = (Label)DGrecdetail.Rows[rowindex].FindControl("txtpenalityamtrec");
            TextBox txtpenalityrate_rec = (TextBox)DGrecdetail.Rows[rowindex].FindControl("txtpenalityrate_rec");

            SqlParameter[] arr = new SqlParameter[8];
            arr[0] = new SqlParameter("@Detailid", lblDetailid.Text);
            arr[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[1].Direction = ParameterDirection.Output;
            arr[2] = new SqlParameter("@ID", lblid.Text);
            arr[3] = new SqlParameter("@userid", Session["varuserid"]);
            arr[4] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            arr[5] = new SqlParameter("@Rate", lblrate.Text == "" ? "0" : lblrate.Text);
            arr[6] = new SqlParameter("@Penalityamt", txtpenalityamtrec.Text == "" ? "0" : txtpenalityamtrec.Text);
            arr[7] = new SqlParameter("@Penalityrate", txtpenalityrate_rec.Text == "" ? "0" : txtpenalityrate_rec.Text);
            //***********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_UPDATEMOTTELINGRECRATE]", arr);
            lblmsg.Text = arr[1].Value.ToString();
            Tran.Commit();
            fillgrid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DDPly_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList DDPly = sender as DropDownList;

        if (DDPly != null)
        {
            GridViewRow gvr = DDPly.NamingContainer as GridViewRow;
            Label lblrate = (Label)gvr.FindControl("lblrate");
            Label lblritemid = (Label)gvr.FindControl("lblritemid");
            Label lblrqualityid = (Label)gvr.FindControl("lblrqualityid");
            Label lblrshadecolorid = (Label)gvr.FindControl("lblrshadecolorid");

            DropDownList DDConeType = (DropDownList)gvr.FindControl("DDconetype");
            DropDownList DDTransportType = (DropDownList)gvr.FindControl("DDTransport");

            lblrate.Text = UtilityModule.Getmottelingrate(lblritemid.Text, lblrqualityid.Text, DDProcessName.SelectedValue, DDPartyName.SelectedValue, txtrecdate.Text, shadecolorid: lblrshadecolorid.Text, Conetype: DDConeType.SelectedItem.Text, PlyType: DDPly.SelectedItem.Text, TransportType: DDTransportType.SelectedItem.Text).ToString();
        }
    }
    protected void DDTransport_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList DDTransport = sender as DropDownList;

        if (DDTransport != null)
        {
            GridViewRow gvr = DDTransport.NamingContainer as GridViewRow;
            Label lblrate = (Label)gvr.FindControl("lblrate");
            Label lblritemid = (Label)gvr.FindControl("lblritemid");
            Label lblrqualityid = (Label)gvr.FindControl("lblrqualityid");
            Label lblrshadecolorid = (Label)gvr.FindControl("lblrshadecolorid");

            DropDownList DDConeType = (DropDownList)gvr.FindControl("DDconetype");
            DropDownList DDPly = (DropDownList)gvr.FindControl("DDPly");

            lblrate.Text = UtilityModule.Getmottelingrate(lblritemid.Text, lblrqualityid.Text, DDProcessName.SelectedValue, DDPartyName.SelectedValue, txtrecdate.Text, shadecolorid: lblrshadecolorid.Text, Conetype: DDConeType.SelectedItem.Text, PlyType: DDPly.SelectedItem.Text, TransportType: DDTransport.SelectedItem.Text).ToString();
        }
    }
    protected void BtnComplete_Click(object sender, EventArgs e)
    {

    }
}
