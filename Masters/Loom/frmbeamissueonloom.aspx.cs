using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Loom_frmbeamissueonloom : System.Web.UI.Page
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
                           select UnitsId,UnitName from Units order by UnitName
                           Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + @" Order by GodownName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDGodown, ds, 2, true, "--Plz Select--");
            if (DDGodown.Items.Count > 0)
            {
                DDGodown.SelectedIndex = 1;
            }
            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
            }
            //********
            if (Session["usertype"].ToString() == "1")
            {
                TDcomplete.Visible = true;
            }
        }
    }
    protected void DDProdunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        AutoCompleteExtenderloomno.ContextKey = "0#0#" + DDProdunit.SelectedValue;
        string str = @"select Distinct PL.UID,PL.LoomNo,case when ISNUMERIC(PL.LoomNo)=1 then CONVERT(int,replace(PL.loomno, '.', '')) Else '9999999' End Loom1 from PROCESS_ISSUE_MASTER_1 PM inner join ProductionLoomMaster PL
                    on PM.LoomId=PL.UID 
                    And PL.companyid=" + DDcompany.SelectedValue + " and  PL.UnitId=" + DDProdunit.SelectedValue;
        if (chkcomplete.Checked == true)
        {
            str = str + @" And PM.Status='Complete'";
        }
        else
        {
            str = str + @" And PM.Status='Pending'";
        }
        str = str + @" order by Loom1,Pl.Loomno";
        UtilityModule.ConditionalComboFill(ref DDLoomNo, str, true, "--Plz Selec--");
    }
    protected void DDLoomNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select PM.IssueOrderId,isnull(PM.ChallanNo,PM.ISSUEORDERID) as ChallanNo from PROCESS_ISSUE_MASTER_1 PM 
                    where PM.Status='Pending' and PM.Companyid=" + DDcompany.SelectedValue + " and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + DDLoomNo.SelectedValue;

        UtilityModule.ConditionalComboFill(ref DDFoliono, str, true, "--Plz Selec--");
    }
    protected void FillFolioNo()
    {
        //        string str = @"select PM.IssueOrderId,PM.IssueOrderId from PROCESS_ISSUE_MASTER_1 PM 
        //                    where PM.Status='Pending' and PM.Companyid=" + DDcompany.SelectedValue + " and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + DDLoomNo.SelectedValue;

        string str = @"select PM.IssueOrderId,isnull(PM.ChallanNo,PM.ISSUEORDERID) as ChallanNo from PROCESS_ISSUE_MASTER_1 PM 
                    where PM.Companyid=" + DDcompany.SelectedValue + " and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + txtloomid.Text;
        if (chkcomplete.Checked == true)
        {
            str = str + " and Pm.status='Complete'";
        }
        else
        {
            str = str + " and Pm.status='Pending'";
        }

        UtilityModule.ConditionalComboFill(ref DDFoliono, str, true, "--Plz Select--");
    }
    protected void FillGrid()
    {
        string str = "";
        if (Session["varCompanyId"].ToString() == "14")
        {
            str = @"select Distinct V.BeamDescription, V.unitid, V.UnitName, V.LotNo, V.TagNo, V.GodownId, V.BeamNo, V.Grossweight,
                        V.TareWeight, V.NetWeight, V.ofinishedid, V.Srno, oSizeflag, V.pcs 
						from V_BeamStock V 
						inner join PROCESS_ISSUE_DETAIL_1 PD on V.Item_Finished_id=PD.Item_Finished_Id ";

            if (DDFoliono.SelectedIndex <= 0)
            {
                str = str + " and PD.issueorderid=0";
            }
            else
            {
                str = str + " and PD.issueorderid=" + DDFoliono.SelectedValue;
            }
            str = str + @" LEFT JOIN (Select Distinct IsNull(PRT.BeamNo, 0) BeamNo 
                                        From ProcessRawMaster PRM(Nolock) 
                                        JOIN ProcessRawTran PRT(Nolock) ON PRT.PRMID = PRM.PRMID 
								        Where PRM.BeamType = 1 And PRM.TypeFlag = 0) a ON a.BeamNo = V.BeamNo
                            Where 1 = 1 and V.pcs>0";
            if (DDGodown.SelectedIndex <= 0)
            {
                str = str + " and V.godownid = 0";
            }
            else
            {
                str = str + " and V.godownid = " + DDGodown.SelectedValue;
            }
            //str = str + " And a.BeamNo Is Null";
            str = str + "  order by V.Srno";
        }
        else
        {
            str = @"select Distinct BeamDescription,unitid,UnitName,LotNo,TagNo,GodownId,BeamNo,Grossweight,
                        TareWeight,NetWeight,ofinishedid,V.Srno,oSizeflag,V.pcs from V_BeamStock V inner join PROCESS_ISSUE_DETAIL_1 PD
                        on V.Item_Finished_id=PD.Item_Finished_Id 
                        Where 1 = 1 and V.pcs>0";

            if (DDFoliono.SelectedIndex <= 0)
            {
                str = str + " and PD.issueorderid=0";
            }
            else
            {
                str = str + " and PD.issueorderid=" + DDFoliono.SelectedValue;
            }
            if (DDGodown.SelectedIndex <= 0)
            {
                str = str + " and V.godownid=0";
            }
            else
            {
                str = str + " and V.godownid=" + DDGodown.SelectedValue;
            }
            str = str + "  order by V.Srno";
        }



        //        string str = @"select Distinct BeamDescription,unitid,UnitName,LotNo,TagNo,GodownId,BeamNo,Grossweight,
        //                        TareWeight,NetWeight,ofinishedid,V.Srno,oSizeflag,V.pcs from V_BeamStock V inner join PROCESS_ISSUE_DETAIL_1 PD
        //                        on V.Item_Finished_id=PD.Item_Finished_Id ";
        //        if (DDFoliono.SelectedIndex <= 0)
        //        {
        //            str = str + " and PD.issueorderid=0";
        //        }
        //        else
        //        {
        //            str = str + " and PD.issueorderid=" + DDFoliono.SelectedValue;
        //        }        

        //            if (DDGodown.SelectedIndex <= 0)
        //            {
        //                str = str + " and V.godownid=0";
        //            }
        //            else
        //            {
        //                str = str + " and V.godownid=" + DDGodown.SelectedValue;
        //            }      

        //        str = str + "  order by V.Srno";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
    }
    protected void DDFoliono_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkEdit.Checked == true)
        {
            string str = @"select Distinct PRM.PrmId,PRM.ChalanNo 
                        from ProcessRawMaster PRM inner join ProcessRawTran PRT on PRM.PRMid=PRT.PRMid And 
                        PRM.BeamType=1 And PRM.TypeFlag = 0 And PRM.CompanyID = " + DDcompany.SelectedValue + " And PRM.Prorderid=" + DDFoliono.SelectedValue + " and Processid=1 and PRM.TranType=0";
            UtilityModule.ConditionalComboFill(ref DDissueno, str, true, "--Plz Select--");
        }
        else
        {
            FillGrid();
        }
    }
    protected void DDGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //**************Sql Table
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("Finishedid", typeof(int));
        dtrecords.Columns.Add("grosswt", typeof(float));
        dtrecords.Columns.Add("Netwt", typeof(float));
        dtrecords.Columns.Add("GodownId", typeof(int));
        dtrecords.Columns.Add("LotNo", typeof(string));
        dtrecords.Columns.Add("TagNo", typeof(string));
        dtrecords.Columns.Add("UnitId", typeof(int));
        dtrecords.Columns.Add("BeamNo", typeof(string));
        dtrecords.Columns.Add("flagsize", typeof(int));
        dtrecords.Columns.Add("BeamPcs", typeof(int));
        //**************
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            if (Chkboxitem.Checked == true)
            {
                Label lblitemfinishedid = ((Label)DG.Rows[i].FindControl("lblitemfinishedid"));
                Label lblgrossweight = ((Label)DG.Rows[i].FindControl("lblgrossweight"));
                Label lblnetweight = ((Label)DG.Rows[i].FindControl("lblnetweight"));
                Label lbllotno = ((Label)DG.Rows[i].FindControl("lbllotno"));
                Label lbltagno = ((Label)DG.Rows[i].FindControl("lbltagno"));
                Label lblunitid = ((Label)DG.Rows[i].FindControl("lblunitid"));
                Label lblbeamno = ((Label)DG.Rows[i].FindControl("lblbeamno"));
                Label lblflagsize = ((Label)DG.Rows[i].FindControl("lblflagsize"));
                Label lblpcs = ((Label)DG.Rows[i].FindControl("lblpcs"));

                if (Convert.ToInt32(lblpcs.Text) > 0 || Convert.ToDecimal(lblgrossweight.Text) > 0)
                {
                    //********Data Row
                    DataRow dr = dtrecords.NewRow();
                    dr["Finishedid"] = lblitemfinishedid.Text;
                    dr["grosswt"] = lblgrossweight.Text;
                    dr["netwt"] = lblnetweight.Text;
                    dr["godownid"] = DDGodown.SelectedValue;
                    dr["Lotno"] = lbllotno.Text;
                    dr["TagNo"] = lbltagno.Text;
                    dr["Unitid"] = lblunitid.Text;
                    dr["BeamNo"] = lblbeamno.Text;
                    dr["flagsize"] = lblflagsize.Text;
                    dr["BeamPcs"] = lblpcs.Text;
                    dtrecords.Rows.Add(dr);
                }
            }
        }
        if (dtrecords.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[9];
                param[0] = new SqlParameter("@PrmId", SqlDbType.Int);
                param[0].Value = 0;
                param[0].Direction = ParameterDirection.InputOutput;
                param[1] = new SqlParameter("@companyid", DDcompany.SelectedValue);
                param[2] = new SqlParameter("@Processid", 1);//Weaving
                param[3] = new SqlParameter("@Prorderid", DDFoliono.SelectedValue);
                param[4] = new SqlParameter("@issueDate", txtissuedate.Text);
                param[5] = new SqlParameter("@userid", Session["varuserid"]);
                param[6] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
                param[7] = new SqlParameter("@dtrecords", dtrecords);
                param[8] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[8].Direction = ParameterDirection.Output;
                ///**********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveBeamIssueonLoom", param);
                Tran.Commit();
                lblmessage.Text = param[8].Value.ToString();
                txtissueno.Text = param[0].Value.ToString();
                hnprmid.Value = param[0].Value.ToString();
                refreshcontrol();
                Fillissuedetail();
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
    protected void refreshcontrol()
    {
        DDLoomNo.SelectedIndex = -1;
        DDFoliono.SelectedIndex = -1;
        DG.DataSource = null;
        DG.DataBind();
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string str = "select * from  V_BeamIssueonLoom where prmid=" + hnprmid.Value;
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Session["VarCompanyNo"].ToString() == "21")
            {
                Session["rptFileName"] = "~\\Reports\\RptbeamissueonloomKaysons.rpt"; ;
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\Rptbeamissueonloom.rpt";
            }
            
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\Rptbeamissueonloom.xsd";
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
        txtloomno.Text = "";
        DDLoomNo.SelectedIndex = -1;
        DDFoliono.SelectedIndex = -1;
        TDissue.Visible = false;
        DG.DataSource = null;
        DG.DataBind();
        DGIssueDetail.DataSource = null;
        DGIssueDetail.DataBind();
        txtissueno.Text = "";
        txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        TDLoomno.Visible = false;
        TDLoomNotextbox.Visible = true;

        if (chkEdit.Checked == true)
        {
            TDissue.Visible = true;
            DDissueno.SelectedIndex = -1;

            TDLoomno.Visible = true;
            TDLoomNotextbox.Visible = false;
        }
    }
    protected void DDissueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnprmid.Value = DDissueno.SelectedValue;
        Fillissuedetail();
    }
    protected void Fillissuedetail()
    {
        string str = @"select Distinct PRT.BeamNo,dbo.F_getItemDescription(PRT.Finishedid,prt.flagsize) as BeamDescription,
                    PRT.BeamPcs as Pcs,prt.prmid,PRT.PRTid,PRM.Prorderid,PRM.chalanno,Replace(CONVERT(nvarchar(11),PRM.date,106),' ','-') as IssueDate 
                    from ProcessRawMaster PRM inner join ProcessRawTran PRT on PRM.PRMid=PRT.PRMid
                    inner join warploommaster WLM on WLM.LoomNo=PRT.BeamNo inner join WarpLoomDetail WLD on WLM.ID=WLD.ID
                    and WLD.ofinishedid=PRT.Finishedid inner join WarpOrderDetail WD on WLD.Issuemasterid=Wd.Id and Wld.IssueDetailid=WD.Detailid
                    Where PRM.TypeFlag = 0 AND PRM.Prmid=" + hnprmid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGIssueDetail.DataSource = ds.Tables[0];
        DGIssueDetail.DataBind();
        if (chkEdit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtissueno.Text = ds.Tables[0].Rows[0]["chalanNo"].ToString();
                txtissuedate.Text = ds.Tables[0].Rows[0]["issuedate"].ToString();
            }
        }
        else
        {
            txtissueno.Text = "";
            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }

    }
    protected void DGIssueDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblprmid = (Label)DGIssueDetail.Rows[e.RowIndex].FindControl("lblprmid");
            Label lblprtid = (Label)DGIssueDetail.Rows[e.RowIndex].FindControl("lblprtid");
            Label lblprorderod = (Label)DGIssueDetail.Rows[e.RowIndex].FindControl("lblprorderid");

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@prmid", lblprmid.Text);
            param[1] = new SqlParameter("@prtid", lblprtid.Text);
            param[2] = new SqlParameter("@prorderid", lblprorderod.Text);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@MasterCompanyId", Session["VarCompanyId"]);
            param[5] = new SqlParameter("@UserId", Session["VarUserid"]);
            //************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_deletebeamissueonloom", param);
            lblmessage.Text = param[3].Value.ToString();
            Tran.Commit();
            Fillissuedetail();
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
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtloomid.Text != "")
        {
            FillFolioNo();
        }
    }
}