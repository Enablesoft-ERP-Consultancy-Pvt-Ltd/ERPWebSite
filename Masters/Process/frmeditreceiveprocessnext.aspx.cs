using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Process_frmeditreceiveprocessnext : System.Web.UI.Page
{
    protected static string Focus = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (Focus != "")
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            sm.SetFocus(Focus);
        }
        if (!IsPostBack)
        {
            string str;
            if (Session["varMasterCompanyIDForERP"].ToString() == "8")
            {
                str = "select PROCESS_NAME_ID,Process_name from PROCESS_NAME_MASTER Where PROCESS_NAME_ID<>1 order by Process_Name   ";
            }
            else
            {
                str = @"Select PNM.PROCESS_NAME_ID, PNM.Process_name 
                    From PROCESS_NAME_MASTER PNM(Nolock) 
                    JOIN UserRightsProcess URP(Nolock) ON URP.ProcessId = PNM.PROCESS_NAME_ID And URP.Userid = " + Session["varuserid"] + @" 
                    Where PNM.PROCESS_NAME_ID <> 1 and PNM.Processtype = 1 order by PNM.Process_Name ";
            }
            str = str + " select ICm.CATEGORY_ID,ICM.CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=Cs.Categoryid and cs.id=0 order by CATEGORY_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDTOProcess, ds, 0, false, "--Plz Select Process--");
            UtilityModule.ConditionalComboFillWithDS(ref DDcategory, ds, 1, true, "--Plz Select Quality--");
            if (DDcategory.Items.Count > 0)
            {
                DDcategory.SelectedIndex = 1;
                DDcategory_SelectedIndexChanged(DDcategory, new EventArgs());
            }
            txtfrom.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtto.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = @"select Distinct vf.designId,vf.designName 
        From V_FinishedItemDetail vf(Nolock) where vf.QualityId=" + DDQuality.SelectedValue + @" And Designid <> 0 order by vf.designName
        Select distinct a.PROCESS_REC_ID, a.ChallanNo 
        From PROCESS_RECEIVE_MASTER_" + DDTOProcess.SelectedValue + @" a(Nolock)
        JOIN PROCESS_RECEIVE_DETAIL_" + DDTOProcess.SelectedValue + @" b(Nolock) ON b.PROCESS_REC_ID = a.PROCESS_REC_ID 
        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ITEM_FINISHED_ID And VF.CATEGORY_ID = " + DDcategory.SelectedValue + @" And VF.QualityId = " + DDQuality.SelectedValue + @"
        Where a.RECEIVEDATE >= '" + txtfrom.Text + "' And a.RECEIVEDATE <= '" + txtto.Text + "' Order By a.PROCESS_REC_ID ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        UtilityModule.ConditionalComboFillWithDS(ref DDDesign, ds, 0, true, "--Plz Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDChallanNo, ds, 1, true, "--Plz Select--");
    }

    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddcolour, "select Distinct vf.ColorId,vf.ColorName From V_FinishedItemDetail vf where vf.QualityId=" + DDQuality.SelectedValue + " and vf.designid=" + DDDesign.SelectedValue + " and vf.colorid<>0 order by vf.colorname", true, "--Plz Select--");
        //fillgrid();
    }
    protected void ddcolour_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddsize, "select Distinct vf.sizeid,vf.SizeMtr+' '+ vf.shapename as size From V_FinishedItemDetail vf where vf.QualityId=" + DDQuality.SelectedValue + " and vf.designid=" + DDDesign.SelectedValue + " and vf.colorid=" + ddcolour.SelectedValue + " and vf.sizeid<>0 order by Size", true, "--Plz Select--");
        //fillgrid();
    }
    protected void txtWeaverIdNo_TextChanged(object sender, EventArgs e)
    {
        if (txtWeaverIdNo.Text != "")
        {
            string str = "select Empid,Empname from empinfo(Nolock) Where Empcode='" + txtWeaverIdNo.Text + "'";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (lstWeaverName.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) == null)
                {
                    lstWeaverName.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Empname"].ToString(), ds.Tables[0].Rows[0]["Empid"].ToString()));
                }
                txtWeaverIdNo.Text = "";
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('No Weaver found at this ID No..');", true);
            }
        }
        Focus = "txtWeaverIdNo";
    }
    protected void btngetdetail_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        FillDetail();
    }
    protected void btnDeleteName_Click(object sender, EventArgs e)
    {
        lstWeaverName.Items.Remove(lstWeaverName.SelectedItem);
    }
    protected void FillDetail()
    {
        try
        {
            //Find EmployeeId         
            string StrEmpid = "";
            for (int i = 0; i < lstWeaverName.Items.Count; i++)
            {
                if (StrEmpid == "")
                {
                    StrEmpid = lstWeaverName.Items[i].Value;
                }
                else
                {
                    StrEmpid = StrEmpid + "," + lstWeaverName.Items[i].Value;
                }
            }
            //Check Employee Entry
            if (StrEmpid == "")
            {
                lblmsg.Text = "Plz Enter Weaver ID No...";
                return;

            }
//            string str = @"select CN.stockno,CN.Tstockno,CN.pack,vf.ITEM_NAME+' '+vf.QualityName+' '+vf.designName+' '+vf.ColorName+' '+vf.ShapeName as Itemdescription,
//                    PRD.Width,PRD.Length,PSD.ToProcessId as ProcessId,PSD.IssueDetailId,PSD.ReceiveDetailId,Replace(convert(nvarchar(11),pSD.orderdate,106),' ','-') as OrderDate,
//                    Replace(convert(nvarchar(11),pSD.Receivedate,106),' ','-') as ReceiveDate,PRD.issueorderid,PRD.Process_rec_id,U.UnitName as UnitName,
//                    isnull(PRD.ActualWidth,'') as ActualWidth,isnull(PRD.ActualLength,'') as ActualLength,
//                    isnull((Select count(QD.QCVALUE) FROM QCDETAIL QD(NoLock)  INNER JOIN QCPARAMETER QP(NoLock) ON QD.QCMASTERID=QP.PARAID  AND QP.PROCESSID=" + DDTOProcess.SelectedValue + @"
//	                Where RecieveID=PRM.PROCESS_REC_ID and RecieveDetailid=PRD.PROCESS_REC_DETAIL_ID and QCValue=0),0) as DefectStatus  
//                    From Process_stock_Detail PSD(NoLock)  
//                    inner join carpetNumber CN(NoLock)  on PSD.StockNo=CN.StockNo 
//                    and PSD.ToProcessId=" + DDTOProcess.SelectedValue + " inner join PROCESS_RECEIVE_DETAIL_" + DDTOProcess.SelectedValue + @" PRD(NoLock)  on PSD.ReceiveDetailId=PRD.Process_Rec_Detail_Id
//                    inner join PROCESS_RECEIVE_MASTER_" + DDTOProcess.SelectedValue + @" PRM(NoLock)  on PRD.Process_Rec_Id=PRM.Process_Rec_Id inner join V_FinishedItemDetail vf(NoLock)  on CN.Item_Finished_Id=vf.ITEM_FINISHED_ID
//                    inner join PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + @" PIM(NoLock)  on PRD.IssueOrderId=PIM.IssueOrderId
//                    left join Units U(NoLock)  on PIM.units=U.UnitsId
//                    Where PSD.CompanyID = " + Session["CurrentWorkingCompanyID"] + @" And 
//                    PSD.IssueDetailId in(select IssueDetailId From Employee_ProcessOrderNo(NoLock)  where processid=" + DDTOProcess.SelectedValue + " and empid in(" + StrEmpid + "))";

//            str = str + " and PRM.ReceiveDate>='" + txtfrom.Text + "' and PRM.ReceiveDate<='" + txtto.Text + "'";
//            if (DDQuality.SelectedIndex > 0)
//            {
//                str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
//            }
//            if (DDDesign.SelectedIndex > 0)
//            {
//                str = str + "  and vf.designId=" + DDDesign.SelectedValue;
//            }
//            str = str + " order by CN.stockno";

            string StrQualityID = "0", StrDesignID = "0", StrColorID = "0", StrSizeID = "0";

            if (DDQuality.Items.Count > 0 && DDQuality.SelectedIndex > 0)
            {
                StrQualityID = DDQuality.SelectedValue;
            }
            if (DDDesign.Items.Count > 0 && DDDesign.SelectedIndex > 0)
            {
                StrDesignID = DDDesign.SelectedValue;
            }
            if (ddcolour.Items.Count > 0 && ddcolour.SelectedIndex > 0)
            {
                StrColorID = ddcolour.SelectedValue;
            }
            if (ddsize.Items.Count > 0 && ddsize.SelectedIndex > 0)
            {
                StrSizeID = ddsize.SelectedValue;
            }

            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_GetDataForNextProcessReceive", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@CompanyID", Session["CurrentWorkingCompanyID"]);
            cmd.Parameters.AddWithValue("@ProcessID", DDTOProcess.SelectedValue);
            cmd.Parameters.AddWithValue("@EmpIDs", StrEmpid);
            cmd.Parameters.AddWithValue("@QualityID", StrQualityID);
            cmd.Parameters.AddWithValue("@DesignID", StrDesignID);
            cmd.Parameters.AddWithValue("@FromDate", txtfrom.Text);
            cmd.Parameters.AddWithValue("@ToDate", txtto.Text);
            cmd.Parameters.AddWithValue("@Process_Rec_ID", DDChallanNo.SelectedIndex > 0 ? DDChallanNo.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@ColorID", StrColorID);
            cmd.Parameters.AddWithValue("@SizeID", StrSizeID);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds); 

            con.Close();
            con.Dispose();

            DG.DataSource = ds.Tables[0];
            DG.DataBind();
            txttotalpcs.Text = ds.Tables[0].Compute("Count(stockno)", "").ToString();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (variable.VarQctype == "1")
                {
                    TDQccheck.Visible = true;
                }
            }
            else
            {
                TDQccheck.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void DG_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
            Label lblissuedetailid = (Label)DG.Rows[e.RowIndex].FindControl("lblissuedetailid");
            Label lblprocessid = (Label)DG.Rows[e.RowIndex].FindControl("lblprcessid");
            Label lblrecdetailid = (Label)DG.Rows[e.RowIndex].FindControl("lblrecdetailid");
            Label lblstockno = (Label)DG.Rows[e.RowIndex].FindControl("lblstockno");
            Label lblissueorderid = (Label)DG.Rows[e.RowIndex].FindControl("lblissueorderid");
            Label lblprocessrecid = (Label)DG.Rows[e.RowIndex].FindControl("lblprocessrecid");
            //*****************
            SqlParameter[] param = new SqlParameter[9];
            param[0] = new SqlParameter("@processid", lblprocessid.Text);
            param[1] = new SqlParameter("@issuedetailid", lblissuedetailid.Text);
            param[2] = new SqlParameter("@recdetailid", lblrecdetailid.Text);
            param[3] = new SqlParameter("@stockno", lblstockno.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@issueorderid", lblissueorderid.Text);
            param[6] = new SqlParameter("@Processrecid", lblprocessrecid.Text);
            param[7] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[8] = new SqlParameter("@userid", Session["varuserid"]);
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_JobReceive_Delete", param);
            lblmsg.Text = param[4].Value.ToString();
            Tran.Commit();
            if (Session["varMasterCompanyIDForERP"].ToString() != "14")
            {
                FillDetail();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblpack = (Label)e.Row.FindControl("lblpack");
            LinkButton LinkButton1 = (LinkButton)e.Row.FindControl("LinkButton1");
            if (lblpack.Text == "1" && DDTOProcess.SelectedItem.Text.ToUpper() != "SAMPLING PACKED")
            {
                e.Row.BackColor = System.Drawing.Color.Red;
                LinkButton1.Visible = false;
            }

            Label lblDefectStatus = (Label)e.Row.FindControl("lblDefectStatus");
            LinkButton lnkRemoveDefect = (LinkButton)e.Row.FindControl("lnkRemoveQccheck");

            if (Convert.ToInt32(lblDefectStatus.Text) > 0)
            {
                lnkRemoveDefect.Visible = true;
            }
            else
            {
                lnkRemoveDefect.Visible = false;
            }
        }
    }
    protected void btnqccheck_Click(object sender, EventArgs e)
    {
        lblqcmsg.Text = "";
        int Gridrows = DG.Rows.Count;
        if (Gridrows > 0 && DDTOProcess.SelectedIndex != -1)
        {
            DataTable dttable = new DataTable();
            dttable.Columns.Add("TstockNo", typeof(string));
            dttable.Columns.Add("Process_Rec_Detail_id", typeof(int));
            dttable.Columns.Add("Process_Rec_id", typeof(int));
            for (int i = 0; i < Gridrows; i++)
            {
                DataRow dr = dttable.NewRow();
                Label lbltstockno = (Label)DG.Rows[i].FindControl("lbltstockno");
                Label lblrecdetailid = (Label)DG.Rows[i].FindControl("lblrecdetailid");
                Label lblprocessrecid = (Label)DG.Rows[i].FindControl("lblprocessrecid");
                // int Process_rec_detail_id = Convert.ToInt32(DG.DataKeys[i].Value);
                dr["TstockNo"] = lbltstockno.Text;
                dr["Process_Rec_Detail_id"] = lblrecdetailid.Text;
                dr["Process_rec_id"] = lblprocessrecid.Text;
                dttable.Rows.Add(dr);
            }
            //*********************


            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_getqcparameterJobWise", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Processid", DDTOProcess.SelectedValue);
            cmd.Parameters.AddWithValue("@dttable", dttable);         

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds); 

            //SqlParameter[] param = new SqlParameter[2];
            //param[0] = new SqlParameter("@Processid", DDTOProcess.SelectedValue);
            //param[1] = new SqlParameter("@dttable", dttable);
            ////*******
            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getqcparameterJobWise", param);


            if (ds.Tables[0].Rows.Count > 0)
            {
                Modalpopupextqc.Show();
                DataTable dt = new DataTable();
                dt.Columns.Add("SRNO", typeof(int));
                dt.Columns.Add("STOCKNO", typeof(string));
                dt.Columns.Add("Processrecid", typeof(int));
                dt.Columns.Add("Processrecdetailid", typeof(int));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dt.Columns.Add(dr["Paraname"].ToString(), typeof(bool));

                }
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["SrNo"] = i + 1;
                    dr["StockNo"] = ds.Tables[1].Rows[i]["TstockNo"].ToString();
                    dr["Processrecid"] = ds.Tables[1].Rows[i]["Process_Rec_id"].ToString();
                    dr["Processrecdetailid"] = ds.Tables[1].Rows[i]["Process_Rec_Detail_Id"].ToString();
                    //**********                   
                    dt.Rows.Add(dr);
                }
                dt.Columns["Processrecid"].ColumnMapping = MappingType.Hidden;
                GDQC.DataSource = dt;
                GDQC.DataBind();
                //check checkboxes
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < GDQC.Rows.Count; i++)
                    {
                        int Processrecdetailid = Convert.ToInt32(GDQC.Rows[i].Cells[3].Text);
                        GridViewRow grow = GDQC.Rows[i];
                        for (int k = 4; k < grow.Cells.Count; k++)
                        {
                            string celltext = GDQC.HeaderRow.Cells[k].Text;
                            for (int j = 0; j < ds.Tables[2].Rows.Count; j++)
                            {
                                int subprocessrecdetailid = Convert.ToInt32(ds.Tables[2].Rows[j]["RecieveDetailID"]);
                                string paramname = ds.Tables[2].Rows[j]["ParaName"].ToString();
                                if ((Processrecdetailid == subprocessrecdetailid) && (celltext == paramname))
                                {
                                    CheckBox ch = grow.Cells[k].Controls[0] as CheckBox;
                                    ch.Checked = Convert.ToBoolean(ds.Tables[2].Rows[j]["QCVALUE"]);
                                }
                            }
                        }
                    }
                }
                //
            }
            else
            {

            }
            //*************
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "qc1", "alert('Please Insert Data first to Save QC Detail')", true);
        }
    }
    protected void btnqcsavenew_Click(object sender, EventArgs e)
    {
        lblqcmsg.Text = "";
        try
        {
            DataTable dtrecord = new DataTable();
            dtrecord.Columns.Add("Processrecid", typeof(int));
            dtrecord.Columns.Add("Processrecdetailid", typeof(int));
            dtrecord.Columns.Add("Parameter", typeof(string));
            dtrecord.Columns.Add("paramvalue", typeof(int));
            //**********
            for (int i = 0; i < GDQC.Rows.Count; i++)
            {
                GridViewRow gvr = GDQC.Rows[i];
                for (int j = 4; j < gvr.Cells.Count; j++)
                {
                    DataRow dr = dtrecord.NewRow();
                    dr["Processrecid"] = GDQC.Rows[i].Cells[2].Text; //Processrecid
                    dr["Processrecdetailid"] = GDQC.Rows[i].Cells[3].Text;//Processrecdetailid                    
                    dr["Parameter"] = GDQC.HeaderRow.Cells[j].Text;
                    CheckBox chk = gvr.Cells[j].Controls[0] as CheckBox;
                    dr["paramvalue"] = chk.Checked == true ? 1 : 0;
                    dtrecord.Rows.Add(dr);
                }
            }
            //*********
            if (dtrecord.Rows.Count > 0)
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@dtrecord", dtrecord);
                param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[1].Direction = ParameterDirection.Output;
                param[2] = new SqlParameter("@processid", DDTOProcess.SelectedValue);
                //*****
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_saveQcJobwise", param);
                lblqcmsg.Text = param[1].Value.ToString();
                Modalpopupextqc.Show();
            }
        }
        catch (Exception ex)
        {
            lblqcmsg.Text = ex.Message;
        }
    }
    protected void GDQC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[2].Visible = false;
        e.Row.Cells[3].Visible = false;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // bind checkbox control with gridview :
            for (int i = 4; i < e.Row.Cells.Count; i++)
            {
                CheckBox chk = e.Row.Cells[i].Controls[0] as CheckBox;
                chk.Enabled = true;
                chk.Checked = true;
                //checked box
                //
            }
        }
    }
    protected void DDcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDQuality, @"select Distinct Q.QualityId,q.QualityName+' ['+Im.Item_Name+']' as QualityName From ITEM_MASTER IM inner join CategorySeparate CS on 
                                                         IM.CATEGORY_ID=cs.Categoryid and cs.id=0  inner join Quality Q on IM.ITEM_ID=q.Item_Id and Cs.Categoryid=" + DDcategory.SelectedValue + " and Im.mastercompanyid=" + Session["varMasterCompanyIDForERP"] + " order by Qualityname", true, "--Plz Select--");
    }
    protected void DG_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DG.EditIndex = e.NewEditIndex;
        FillDetail();
    }
    protected void DG_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DG.EditIndex = -1;
        FillDetail();
    }
    protected void DG_RowUpdating(object sender, GridViewUpdateEventArgs e)
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
            Label lblprocessid = (Label)DG.Rows[e.RowIndex].FindControl("lblprcessid");
            Label lblrecdetailid = (Label)DG.Rows[e.RowIndex].FindControl("lblrecdetailid");
            TextBox txtgridactualL = (TextBox)DG.Rows[e.RowIndex].FindControl("txtgridactualL");
            TextBox txtgridactualW = (TextBox)DG.Rows[e.RowIndex].FindControl("txtgridactualW");
            //*****************
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@processid", lblprocessid.Text);
            param[1] = new SqlParameter("@recdetailid", lblrecdetailid.Text);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[4] = new SqlParameter("@userid", Session["varuserid"]);
            param[5] = new SqlParameter("@ActualLength", txtgridactualL.Text.Trim());
            param[6] = new SqlParameter("@ActualWidth", txtgridactualW.Text.Trim());

            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_JobReceive_Update", param);
            lblmsg.Text = param[2].Value.ToString();
            Tran.Commit();
            DG.EditIndex = -1;
            FillDetail();
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
    protected void lnkRemoveQccheck_Click(object sender, EventArgs e)
    {
        lblRemoveqcmsg.Text = "";
        ModalPopuptext2.Show();
        ViewState["qcdetail"] = null;
        LinkButton lnk = sender as LinkButton;
        if (lnk != null)
        {
            GridViewRow grv = lnk.NamingContainer as GridViewRow;
            Label Processrecid = (Label)DG.Rows[grv.RowIndex].FindControl("lblprocessrecid");
            Label processrecdetailid = (Label)DG.Rows[grv.RowIndex].FindControl("lblrecdetailid");
            Label lblprocessid = (Label)DG.Rows[grv.RowIndex].FindControl("lblprcessid");

            lblRemoveQCProcessRecId.Text = Processrecid.Text;
            lblRemoveQCProcessRecDetailId.Text = processrecdetailid.Text;
            lblRemoveQCProcessId.Text = lblprocessid.Text;
            txtRemoveQCDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

        }

        ////Modalpopupext.Show();
        ////ViewState["qcdetail"] = null;
        //LinkButton lnk = sender as LinkButton;
        //if (lnk != null)
        //{
        //    GridViewRow grv = lnk.NamingContainer as GridViewRow;
        //    Label Processrecid = (Label)DG.Rows[grv.RowIndex].FindControl("lblprocessrecid");
        //    Label processrecdetailid = (Label)DG.Rows[grv.RowIndex].FindControl("lblrecdetailid");
        //    Label lblprocessid = (Label)DG.Rows[grv.RowIndex].FindControl("lblprcessid");

        //    if (Convert.ToInt32(Processrecid.Text) > 0)
        //    {
        //        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //        if (con.State == ConnectionState.Closed)
        //        {
        //            con.Open();
        //        }
        //        SqlTransaction Tran = con.BeginTransaction();
        //        try
        //        {
        //            //**********Remove QC Defects
        //            SqlParameter[] param = new SqlParameter[6];
        //            param[0] = new SqlParameter("@Process_Rec_Id", Processrecid.Text);
        //            param[1] = new SqlParameter("@Process_Rec_Detail_Id", processrecdetailid.Text);
        //            param[2] = new SqlParameter("@ProcessId", lblprocessid.Text);
        //            param[3] = new SqlParameter("@MSG", SqlDbType.VarChar, 100);
        //            param[3].Direction = ParameterDirection.Output;
        //            param[4] = new SqlParameter("@UserId", Session["varuserid"]);
        //            param[5] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);

        //            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_REMOVEQCDEFECTS", param);
        //            lblmsg.Text = param[3].Value.ToString();
        //            Tran.Commit();
        //            FillDetail();
        //        }
        //        catch (Exception ex)
        //        {
        //            lblmsg.Text = ex.Message;
        //            Tran.Rollback();
        //        }
        //        finally
        //        {
        //            con.Close();
        //            con.Dispose();
        //        }
        //    }


        //}
    }

    protected void BtnRemoveQCSave_Click(object sender, EventArgs e)
    {
        lblRemoveqcmsg.Text = "";
        try
        {

            if (Convert.ToInt32(lblRemoveQCProcessRecId.Text) > 0)
            {
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlTransaction Tran = con.BeginTransaction();
                try
                {
                    ////**********Remove QC Defects
                    SqlParameter[] param = new SqlParameter[7];
                    param[0] = new SqlParameter("@Process_Rec_Id", lblRemoveQCProcessRecId.Text);
                    param[1] = new SqlParameter("@Process_Rec_Detail_Id", lblRemoveQCProcessRecDetailId.Text);
                    param[2] = new SqlParameter("@ProcessId", lblRemoveQCProcessId.Text);
                    param[3] = new SqlParameter("@MSG", SqlDbType.VarChar, 100);
                    param[3].Direction = ParameterDirection.Output;
                    param[4] = new SqlParameter("@UserId", Session["varuserid"]);
                    param[5] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
                    param[6] = new SqlParameter("@QCRemoveDate", txtRemoveQCDate.Text);

                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_REMOVEQCDEFECTS", param);
                    lblRemoveqcmsg.Text = param[3].Value.ToString();
                    Tran.Commit();
                    ModalPopuptext2.Show();
                    FillDetail();
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
        }
        catch (Exception ex)
        {
            lblqcmsg.Text = ex.Message;
        }
    }
}