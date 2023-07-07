using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Sample_Material_frmsampledyeingMaterialReceive : System.Web.UI.Page
{
    public static int rowindex = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                           select PROCESS_NAME_ID,Process_name From process_name_master  where Processtype=0 and mastercompanyid=" + Session["varcompanyid"] + @" order by PROCESS_NAME 
                           Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + @" Order by GodownName
                           select Godownid From ModuleWiseGodown Where ModuleName='" + Page.Title + @"' 
                           Select ID, BranchName 
                                From BRANCHMASTER BM(nolock) 
                                JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                                Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];

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
                if (DDProcessName.Items.FindByValue("5") != null) //DYEING
                {
                    DDProcessName.SelectedValue = "5";
                    DDProcessName_SelectedIndexChanged(sender, e);
                }
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDgodown, ds, 2, true, "--Plz Select--");
            if (ds.Tables[3].Rows.Count > 0)
            {
                if (DDgodown.Items.FindByValue(ds.Tables[3].Rows[0]["godownid"].ToString()) != null)
                {
                    DDgodown.SelectedValue = ds.Tables[3].Rows[0]["godownid"].ToString();
                }
            }
            txtrecdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (Session["canedit"].ToString() == "1")
            {
                TRedit.Visible = true;
            }
            //********
            switch (Session["varcompanyNo"].ToString())
            {
                case "16":
                    lblindentno.Text = "Challan No.";
                    break;
                case "22":
                    lblcheckedby.Text = "Inwards No.";
                    break;
                default:
                    break;
            }
            //if (variable.VarBINNOWISE == "1")
            //{
            //    TDBinNo.Visible = true;
            //}
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = "0";
        txtchallanNo.Text = "";
        UtilityModule.ConditionalComboFill(ref DDPartyName, "Select EI.EmpId,EI.EmpName From EmpInfo EI inner join EmpProcess EP on EI.EmpId=EP.EmpId and EP.ProcessId=" + DDProcessName.SelectedValue + " order by EI.empname", true, "--Plz Select--");
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = "0";
        txtchallanNo.Text = "";
        FillIndentNo();
    }
    protected void FillIndentNo()
    {
        string str = @"select ID,indentNo from sampledyeingmaster Where companyid=" + DDCompanyName.SelectedValue + " and processid=" + DDProcessName.SelectedValue + " and empid=" + DDPartyName.SelectedValue;
        if (chkcomplete.Checked == true)
        {
            str = str + " and Status='Complete'";
        }
        else
        {
            str = str + " and Status='Pending'";
        }
        str = str + " order by ID desc";
        UtilityModule.ConditionalComboFill(ref DDindentNo, str, true, "--Plz Select--");
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Boolean saveflag = true;
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
        dtrecord.Columns.Add("BinNo", typeof(string));
        dtrecord.Columns.Add("BELLWT", typeof(float));
        dtrecord.Columns.Add("IssQtyOnMachine", typeof(float));
        //************
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            Label lblrfinishedid = (Label)DG.Rows[i].FindControl("lblrfinishedid");
            // DropDownList DDgodown = (DropDownList)DG.Rows[i].FindControl("DDgodown");
            Label lblrlotno = (Label)DG.Rows[i].FindControl("lblrlotno");
            TextBox txtrtagno = (TextBox)DG.Rows[i].FindControl("txtrtagno");
            TextBox lblrate = (TextBox)DG.Rows[i].FindControl("txtrate");
            TextBox txtrecqty = (TextBox)DG.Rows[i].FindControl("txtrecqty");
            TextBox txtbellwt = (TextBox)DG.Rows[i].FindControl("txtbellwt");
            TextBox txtlossqty = (TextBox)DG.Rows[i].FindControl("txtlossqty");
            Label lblissuedetailid = (Label)DG.Rows[i].FindControl("lblissuedetailid");
            Label lblunitid = (Label)DG.Rows[i].FindControl("lblunitid");
            Label lblrflagsize = (Label)DG.Rows[i].FindControl("lblrflagsize");
            TextBox txtundyedqty = (TextBox)DG.Rows[i].FindControl("txtundyedqty");
            Label lblifinishedid = (Label)DG.Rows[i].FindControl("lblifinishedid");
            DropDownList DDBinNo = (DropDownList)DG.Rows[i].FindControl("DDBinNo");
            TextBox txtIssQtyOnMachine = (TextBox)DG.Rows[i].FindControl("txtIssQtyOnMachine");

            saveflag = false;
            double finalqty = Convert.ToDouble(txtrecqty.Text == "" ? "0" : txtrecqty.Text) - Convert.ToDouble(txtbellwt.Text == "" ? "0" : txtbellwt.Text);
            if (variable.VarBINNOWISE == "1")
            {
                if (DDgodown.SelectedIndex > 0 && DDBinNo.SelectedIndex > 0 && (Convert.ToDouble(finalqty) > 0 || Convert.ToDouble(txtlossqty.Text == "" ? "0" : txtlossqty.Text) > 0 || Convert.ToDouble(txtundyedqty.Text == "" ? "0" : txtundyedqty.Text) > 0))
                {
                    saveflag = true;
                }
            }
            else
            {
                if (DDgodown.SelectedIndex > 0 && (Convert.ToDouble(finalqty) > 0 || Convert.ToDouble(txtlossqty.Text == "" ? "0" : txtlossqty.Text) > 0 || Convert.ToDouble(txtundyedqty.Text == "" ? "0" : txtundyedqty.Text) > 0))
                {
                    saveflag = true;
                }
            }
            //check rec qty and loss qty
            if (saveflag == true)
            {

                DataRow dr = dtrecord.NewRow();
                dr["Rfinishedid"] = lblrfinishedid.Text;
                dr["godownid"] = DDgodown.SelectedValue;
                dr["Lotno"] = lblrlotno.Text;
                dr["TagNo"] = txtrtagno.Text;
                dr["Rate"] = lblrate.Text == "" ? "0" : lblrate.Text;
                dr["Recqty"] = finalqty;
                dr["Lossqty"] = txtlossqty.Text == "" ? "0" : txtlossqty.Text;
                dr["issuedetailid"] = lblissuedetailid.Text;
                dr["unitid"] = lblunitid.Text;
                dr["Rflagsize"] = lblrflagsize.Text;
                dr["undyedqty"] = txtundyedqty.Text == "" ? "0" : txtundyedqty.Text;
                dr["Ifinishedid"] = lblifinishedid.Text;
                dr["BinNo"] = variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : "";
                dr["BELLWT"] = txtbellwt.Text == "" ? "0" : txtbellwt.Text;
                dr["IssQtyOnMachine"] = txtIssQtyOnMachine.Text == "" ? "0" : txtIssQtyOnMachine.Text;
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
                arr[4] = new SqlParameter("@Indentid", DDindentNo.SelectedValue);
                arr[5] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 50);
                arr[5].Direction = ParameterDirection.InputOutput;
                arr[5].Value = txtchallanNo.Text;
                arr[6] = new SqlParameter("@RecDate", txtrecdate.Text);
                arr[7] = new SqlParameter("@userid", Session["varuserid"]);
                arr[8] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
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
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SavesampleDyeingReceive", arr);
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
                    //DDindentNo.SelectedIndex = -1;
                    DDindentNo_SelectedIndexChanged(sender, new EventArgs());
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
            if (variable.VarBINNOWISE == "1")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select Godown or Bin No. or Enter (Recqty or Loss qty) to save data.');", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select Godown or Enter (Recqty or Loss qty) to save data.');", true);
            }

        }
    }
    protected void FillGridReceive()
    {
        string str = @"select SM.id as Issueid,SD.Detailid as IssueDetailid,dbo.F_getItemDescription(SD.Ifinishedid,SD.iflagsize) as Iitemdescription,
                    dbo.F_getItemDescription(SD.Rfinishedid,SD.Rflagsize) as Ritemdescription,SD.RecLotNo,case when " + Session["varcompanyId"].ToString() + @"=16 Then '' else SD.RecTagno end as RecTagno,SD.issueqty as Issuedqty,dbo.F_getsamplereceiveqty(SD.Detailid) as Receivedqty,
                    SD.Rate,sd.Rfinishedid,SD.unitid,SD.Rflagsize,SD.ifinishedid,isnull(PCT.CalType,'') as RecCalType
                    From SampleDyeingMaster SM inner join SampleDyeingDetail SD on SM.ID=SD.masterid 
                    LEFT JOIN Process_CalType PCT ON SD.caltype=PCT.CalId
                    Where SM.ID=" + DDindentNo.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
    }
    protected void DDindentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGridReceive();

        string str = @"select Distinct SRM.ID,SRM.ChallanNo 
                    From SampleDyeingReceivemaster SRM 
                    join SampleDyeingReceiveDetail SRD on SRM.ID=SRD.Masterid
                    join SampleDyeingmaster SM on SRD.issueid=SM.ID 
                    Where SRM.companyid=" + DDCompanyName.SelectedValue + " And SRM.Processid=" + DDProcessName.SelectedValue + @" And SRM.BranchID=" + DDBranchName.SelectedValue + @" And 
                    SRM.empid=" + DDPartyName.SelectedValue + " and SM.ID=" + DDindentNo.SelectedValue + " order by srm.ID desc";

        UtilityModule.ConditionalComboFill(ref DDpartychallan, str, true, "--Plz Select--");
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < DG.Columns.Count; i++)
            {

                if (DG.Columns[i].HeaderText.ToUpper() == "BIN NO.")
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

                if (Session["VarCompanyNo"].ToString() == "43")
                {
                    if (DG.Columns[i].HeaderText.ToUpper() == "ISSQTY ONMACHINE")
                    {
                        DG.Columns[i].Visible = true;
                    }
                }
            }
            if (variable.VarBINNOWISE == "1")
            {
                DropDownList DDBinNo = ((DropDownList)e.Row.FindControl("DDBinNo"));
                if (DDBinNo != null)
                {
                    if (variable.VarCHECKBINCONDITION == "1")
                    {
                        Label lblrfinishedid = (Label)e.Row.FindControl("lblrfinishedid");
                        UtilityModule.FillBinNO(DDBinNo, Convert.ToInt32(DDgodown.SelectedValue), Convert.ToInt32(lblrfinishedid.Text), New_Edit: 0);
                    }
                    else
                    {
                        UtilityModule.ConditionalComboFill(ref DDBinNo, "select BINNO,BINNO From BinMaster Where Godownid=" + DDgodown.SelectedValue + " order by BINID", true, "--Plz Select--");
                    }
                }
            }

        }
    }
    protected void fillgrid()
    {
        string str = @"select SRM.ID,SRD.Detailid,dbo.F_getItemDescription(srd.Rfinishedid,SRD.Rflagsize) as RItemdescription,
                        gm.GodownName,SRD.LotNo,SRD.TagNo,SRD.Recqty,SRD.Lossqty,SRD.Rate,SRM.GateinNo,SRM.challanNo,SRM.receiveDate,srd.Undyedqty,
                        isnull(Srm.Checkedby,'') as Checkedby,isnull(Srm.Approvedby,'') as Approvedby,isnull(SRD.bellWtQty,0) as BellWtQty,
                        isnull(SRD.IssQtyOnMachine,0) as IssQtyOnMachine
                        From SampleDyeingReceivemaster SRM inner join SampleDyeingReceiveDetail SRD on SRM.ID=SRD.Masterid
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
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = "select * from v_sampledyeingreceive where id=" + hnid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Session["VarCompanyId"].ToString() == "22")
            {
                Session["rptFileName"] = "~\\Reports\\rptsampledyeingreceiveDiamond.rpt";
            }
            else if (Session["VarCompanyId"].ToString() == "43")
            {
                Session["rptFileName"] = "~\\Reports\\rptsampledyeingreceiveCI.rpt";
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\rptsampledyeingreceive.rpt";
            }
           
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptsampledyeingreceive.xsd";
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
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        chkcomplete.Checked = false;
        DDpartychallan.SelectedIndex = -1;
        DDindentNo.Items.Clear();
        DDpartychallan.Items.Clear();
        TDPartychallan.Visible = false;
        DGrecdetail.DataSource = null;
        DGrecdetail.DataBind();
        DG.DataSource = null;
        DG.DataBind();
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
    protected void DDpartychallan_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = DDpartychallan.SelectedValue;
        fillgrid();
    }
    protected void DGrecdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < DGrecdetail.Columns.Count; i++)
            {
                if (Session["VarCompanyNo"].ToString() == "43")
                {
                    if (DGrecdetail.Columns[i].HeaderText.ToUpper() == "ISSQTY ONMACHINE")
                    {
                        DGrecdetail.Columns[i].Visible = true;
                    }
                }
            }          

        }
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
    protected void DGrecdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Popup(true);
        txtpwd.Focus();
        rowindex = e.RowIndex;
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
            arr[4] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            //***********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeletesampledyeingReceive", arr);
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
    protected void Updatedetails(int rowindex)
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
            Label lblid = (Label)DGrecdetail.Rows[rowindex].FindControl("lblid");
            Label lbldetailid = (Label)DGrecdetail.Rows[rowindex].FindControl("lbldetailid");
            TextBox txtrecqty = (TextBox)DGrecdetail.Rows[rowindex].FindControl("txtrecqty");
            TextBox txtbellwtqty = (TextBox)DGrecdetail.Rows[rowindex].FindControl("txtBellWt");
            TextBox txtlossqty = (TextBox)DGrecdetail.Rows[rowindex].FindControl("txtlossqty");
            TextBox txteditrate = (TextBox)DGrecdetail.Rows[rowindex].FindControl("txteditrate");
            TextBox txtundyedqtyedit = (TextBox)DGrecdetail.Rows[rowindex].FindControl("txtundyedqtyedit");
            TextBox txtIssQtyOnMachine = (TextBox)DGrecdetail.Rows[rowindex].FindControl("txtIssQtyOnMachine");
            double finalqty = Convert.ToDouble(txtrecqty.Text == "" ? "0" : txtrecqty.Text) - Convert.ToDouble(txtbellwtqty.Text == "" ? "0" : txtbellwtqty.Text);
            //*************
            SqlParameter[] arr = new SqlParameter[11];
            arr[0] = new SqlParameter("@ID", lblid.Text);
            arr[1] = new SqlParameter("@Detailid", lbldetailid.Text);
            arr[2] = new SqlParameter("@recqty", finalqty);
            arr[3] = new SqlParameter("@Lossqty", txtlossqty.Text == "" ? "0" : txtlossqty.Text);
            arr[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[4].Direction = ParameterDirection.Output;
            arr[5] = new SqlParameter("@userid", Session["varuserid"]);
            arr[6] = new SqlParameter("@Rate", txteditrate.Text == "" ? "0" : txteditrate.Text);
            arr[7] = new SqlParameter("@undyedqty", txtundyedqtyedit.Text == "" ? "0" : txtundyedqtyedit.Text);
            arr[8] = new SqlParameter("@checkedby", txtcheckedby.Text);
            arr[9] = new SqlParameter("@Approvedby", txtapprovedby.Text);
            arr[10] = new SqlParameter("@IssQtyOnMachine", txtIssQtyOnMachine.Text == "" ? "0" : txtIssQtyOnMachine.Text);
            //*******
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_updateSampleDyeingReceive", arr);
            lblmsg.Text = arr[4].Value.ToString();
            Tran.Commit();
            DGrecdetail.EditIndex = -1;
            fillgrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void txtpwd_TextChanged(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        if (variable.VarDyeingeditpwd == txtpwd.Text)
        {
            Updatedetails(rowindex);
            Popup(false);
        }
        else
        {
            lblmsg.Text = "Please Enter Correct Password..";
        }
        DGrecdetail.EditIndex = -1;
        fillgrid();
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
    protected void DDgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (TDBinNo.Visible == true)
        //{

        //    UtilityModule.ConditionalComboFill(ref DDBinNo, "select BINNO,BINNO From BinMaster Where Godownid=" + DDgodown.SelectedValue + " order by BINID", true, "--Plz Select--");

        //}
        FillGridReceive();
    }
    protected void BtnForComplete_Click(object sender, EventArgs e)
    {
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update sampledyeingmaster Set Status = 'Complete' Where ID = " + DDindentNo.SelectedValue);
        ScriptManager.RegisterStartupScript(Page, GetType(), "altupd", "alert('Status update successfully')", true);
    }
}