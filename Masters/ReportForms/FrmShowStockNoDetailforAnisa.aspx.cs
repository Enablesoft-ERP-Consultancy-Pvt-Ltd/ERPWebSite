using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using ClosedXML.Excel;

public partial class Masters_ReportForms_FrmShowStockNoDetailforAnisa : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtStockNo.Text = "";
            txtStockNo.Focus();
            switch (Session["varcompanyid"].ToString())
            {
                case "14":
                case "45":
                    if (Session["usertype"].ToString() == "1")
                    {
                        TBVendorid.Visible = true;
                        TDPackingBarCode.Visible = false;
                        TDTxtBoxPackingBarCode.Visible = false;
                        Trpack.Visible = false;
                    }
                    else
                    {
                        TBVendorid.Visible = false;
                        TDPackingBarCode.Visible = false;
                        TDTxtBoxPackingBarCode.Visible = false;
                        Trpack.Visible = false;
                    }
                    BtnPreviewGridData.Visible = true;
                    break;
                case "21":
                    TBVendorid.Visible = false;
                    TDPackingBarCode.Visible = true;
                    TDTxtBoxPackingBarCode.Visible = true;
                    break;
                case "22":
                    if (Session["usertype"].ToString() == "1")
                    {
                        btnprintstockrawdetail.Visible = false;
                        TBVendorid.Visible = false;
                        TDPackingBarCode.Visible = false;
                        TDTxtBoxPackingBarCode.Visible = false;
                    }
                    else
                    {
                        btnprintstockrawdetail.Visible = false;
                        TBVendorid.Visible = false;
                        TDPackingBarCode.Visible = false;
                        TDTxtBoxPackingBarCode.Visible = false;
                    }
                    break;
                default:
                    TBVendorid.Visible = false;
                    TDPackingBarCode.Visible = false;
                    TDTxtBoxPackingBarCode.Visible = false;
                    break;
            }
        }
    }

    protected void BtnShow_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();

        try
        {
            int VarNum = 0;
            if (txtStockNo.Text != "")
            {
                string StrNew = "";
                string[] Str = txtStockNo.Text.Split(',');
                foreach (string arrStr in Str)
                {
                    if (VarNum == 0)
                    {
                        StrNew = "'" + arrStr + "'";
                        VarNum = 1;
                    }
                    else
                    {
                        StrNew = StrNew + "," + "'" + arrStr + "'";
                    }
                }
                string Query = @"Select Replace(Str(CN.StockNo)+'|'+Str(IssueDetailId)+'|'+Str(ReceiveDetailId)+'|'+Str(ToProcessId),' ','') StockNo,CN.TStockNo,
                                '' EmpName,'' IssueChallanNo,replace(convert(varchar(11),OrderDate,106), ' ','-') OrderDate,'' RecChallanNo,
                                Replace(convert(varchar(11),ReceiveDate,106), ' ','-') ReceiveDate,CN.CompanyId,FromProcessId,ToProcessId,ReceiveDetailId,IssueDetailId,Pack,
                                PM.PROCESS_NAME,VF.Item_Name+' '+VF.QualityName+' '+VF.DesignName+' '+VF.ColorName+' '+VF.ShapeName+' '+VF.SizeFt  Description,'' as UnitName
                                From Process_Stock_Detail PSD,CarpetNumber CN,Process_Name_Master PM,V_FinishedItemDetail VF 
                                Where PSD.StockNo=CN.StockNo And PSD.ToProcessId=PM.PROCESS_NAME_ID And CN.Item_Finished_id=VF.Item_Finished_id And 
                                CN.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And TSTOCKNO in (" + StrNew + @") And 
                                VF.MasterCompanyId=" + Session["varCompanyId"] + " Order By CN.TSTOCKNO,PSD.ProcessDetailId";

                DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Query);
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    DGStock.DataSource = Ds;
                    DGStock.DataBind();
                }
                VarNum = 0;
                for (int i = 0; i < DGStock.Rows.Count; i++)
                {
                    StrNew = DGStock.DataKeys[i].Value.ToString();
                    int VarIssueDetailId = Convert.ToInt32(StrNew.Split('|')[1]);
                    int VarReceiveDetailId = Convert.ToInt32(StrNew.Split('|')[2]);
                    int VarProcessId = Convert.ToInt32(StrNew.Split('|')[3]);
                    if (VarIssueDetailId > 0)
                    {
                        Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select replace(convert(varchar(11),PM.AssignDate,106), ' ','-') AssignDate,PM.IssueOrderId,(select * from [dbo].[Get_Employeename](PM.IssueOrderId,PD.issue_Detail_Id," + VarProcessId + ")) As EmpName From PROCESS_ISSUE_MASTER_" + VarProcessId + " PM,PROCESS_ISSUE_DETAIL_" + VarProcessId + " PD Where PM.IssueOrderId=PD.IssueOrderId  And Issue_Detail_Id=" + VarIssueDetailId + " ");
                        DGStock.Rows[i].Cells[2].Text = (Ds.Tables[0].Rows[0]["EmpName"]).ToString();
                        DGStock.Rows[i].Cells[3].Text = (Ds.Tables[0].Rows[0]["IssueOrderId"]).ToString();
                        if (DGStock.Rows[i].Cells[4].Text != "")
                        {
                            DGStock.Rows[i].Cells[4].Text = Ds.Tables[0].Rows[0]["AssignDate"].ToString();
                        }
                    }
                    if (VarReceiveDetailId > 0)
                    {
                        DGStock.Rows[i].Cells[5].Text = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Process_Rec_Id From PROCESS_Receive_DETAIL_" + VarProcessId + " Where Process_Rec_Detail_Id=" + VarReceiveDetailId + "").ToString();
                    }

                }
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmShowStockNoDetail.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DGStock_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DG, "select$" + e.Row.RowIndex);
            LinkButton linkforseeDetail = e.Row.FindControl("linkforseeDetail") as LinkButton;
            LinkButton LnkBtnJobSequence = e.Row.FindControl("LnkBtnJobSequence") as LinkButton;

            Label lblprocessId = e.Row.FindControl("lblProcessId") as Label;

            if (lblprocessId.Text != "1")
            {
                linkforseeDetail.Visible = false;
                LnkBtnJobSequence.Visible = false;
            }

            for (int i = 0; i < DGStock.Columns.Count; i++)
            {

                if (Session["varcompanyId"].ToString() == "21")
                {
                    if (Session["usertype"].ToString() == "1")
                    {
                        if (DGStock.Columns[i].HeaderText.ToUpper() == "EMP_NAME" || DGStock.Columns[i].HeaderText == "Stock Status")
                        {
                            DGStock.Columns[i].Visible = true;
                        }
                    }
                    else
                    {
                        if (DGStock.Columns[i].HeaderText.ToUpper() == "EMP_NAME" || DGStock.Columns[i].HeaderText == "Stock Status")
                        {
                            DGStock.Columns[i].Visible = false;
                        }
                    }
                }

                if (Session["varcompanyId"].ToString() == "22")
                {
                    if (DGStock.Columns[i].HeaderText == "Cotton LotNo")
                    {
                        DGStock.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (DGStock.Columns[i].HeaderText == "Cotton LotNo")
                    {
                        DGStock.Columns[i].Visible = false;
                    }
                }

                if (Session["varcompanyId"].ToString() == "14" || Session["varcompanyId"].ToString() == "45")
                {
                    if (DGStock.Columns[i].HeaderText == "ECIS NO & DEST.CODE" || DGStock.Columns[i].HeaderText == "JobSequence")
                    {
                        DGStock.Columns[i].Visible = true;                        
                    }
                }
                else
                {
                    if (DGStock.Columns[i].HeaderText == "ECIS NO & DEST.CODE" || DGStock.Columns[i].HeaderText == "JobSequence")
                    {
                        DGStock.Columns[i].Visible = false;
                        
                    }
                }
            }
        }

    }
    protected void lnkstockDetail_Click(object sender, EventArgs e)
    {

        LinkButton lnk = sender as LinkButton;

        if (lnk != null)
        {
            GridViewRow grv = lnk.NamingContainer as GridViewRow;
            hngridrowindex.Value = grv.RowIndex.ToString();



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
    protected void lnkchangestatus_Click(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;
        if (lnk != null)
        {
            GridViewRow gvr = lnk.NamingContainer as GridViewRow;
            hngridrowindex.Value = gvr.RowIndex.ToString();
            Popup(true);
            txtpwd.Focus();

        }
    }

    protected void txtpwd_TextChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        if (variable.VarCHANGESTOCKSTATUS_PWD == txtpwd.Text)
        {
            UpdateStockStatus(Convert.ToInt16(hngridrowindex.Value));
            Popup(false);
        }
        else
        {
            LblErrorMessage.Text = "Please Enter Correct Password..";
        }
    }

    protected void UpdateStockStatus(int rowindex)
    {
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lbltoprocessid = (Label)DGStock.Rows[rowindex].FindControl("lbltoprocessid");
            Label lblreceivedetailid = (Label)DGStock.Rows[rowindex].FindControl("lblreceivedetailid");
            Label lblqualitytype = (Label)DGStock.Rows[rowindex].FindControl("lblqualitytype");
            Label lbltstockno = (Label)DGStock.Rows[rowindex].FindControl("lbltstockno");

            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@Processid", lbltoprocessid.Text);
            param[1] = new SqlParameter("@Process_rec_detail_id", lblreceivedetailid.Text);
            param[2] = new SqlParameter("@Userid", Session["varuserid"]);
            param[3] = new SqlParameter("@MastercompanyId", Session["varcompanyid"]);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@QualityType", lblqualitytype.Text);
            param[6] = new SqlParameter("@TstockNo", lbltstockno.Text);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATESTOCKSTATUS", param);
            Tran.Commit();
            txtStockNo_TextChanged(txtStockNo, new EventArgs());
            LblErrorMessage.Text = param[4].Value.ToString();

        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void txtStockNo_TextChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        txtremark.Text = "";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            int VarNum = 0;
            if (txtStockNo.Text != "")
            {
                string StrNew = "";
                string[] Str = txtStockNo.Text.Split(',');
                foreach (string arrStr in Str)
                {
                    if (VarNum == 0)
                    {
                        StrNew = "'" + arrStr + "'";
                        VarNum = 1;
                    }
                    else
                    {
                        StrNew = StrNew + "," + "'" + arrStr + "'";
                    }
                }

                string Query = @"select '' Stockno, CN.Tstockno, '-' Empname, P.TInvoiceNo IssueChallanNo, Replace(convert(nvarchar(11), P.PackingDate, 106), ' ', '-') orderDate,
                            '-' as RecchallanNo, '-' Receivedate, CN.CompanyId, 0 FromProcessId, 0 ToProcessId, 0 ReceiveDetailId, 0 IssueDetailId, CN.Pack, 'STOCK OUT' Process_name,
                            VF.Item_Name + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + VF.SizeFt  Description, IsNull(CC.CustomerCode, '') CustomerCode, 
                            IsNull(OM.customerorderNO, '') customerorderNO, 999999999999999999999999 ProcessDetailId, isnull(CN.PackingId, 0) PackingId, 0 ToProcessid, 
                            CN.Item_Finished_Id, '' UnitName, '' LoomNo, '' StockStatus, 1 QualityType, '' TanaLotNo, IsNull(NUD.UserName, '') UserName
                            ,isnull((Select distinct Ecisno from PackingInformation(NoLock) where PackingId=P.PackingId),'') as Ecisno,
							isnull((Select distinct Destcode from INVOICE(NoLock) where TInvoiceNo=P.TInvoiceNo and InvoiceId=P.InvoiceNo),'') as Destcode,'' as FolioChallanNo
                            From carpetnumber CN 
                            inner Join packing P on CN.PackingID=P.PackingId
                            inner join V_FinishedItemDetail vf on CN.Item_Finished_Id=vf.ITEM_FINISHED_ID
                            left join OrderMaster Om on CN.OrderId=OM.OrderId
                            left join customerinfo CC on OM.CustomerId=CC.customerid 
                            left JOIN NewUserDetail NUD ON NUD.UserId = P.UserId                            
                            Where CN.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And CN.TSTOCKNO in (" + StrNew + @")
                            UNION ALL
                            Select Replace(Str(CN.StockNo)+'|'+Str(IssueDetailId)+'|'+Str(ReceiveDetailId)+'|'+Str(ToProcessId),' ','') StockNo,CN.TStockNo,
                            '' EmpName,'' IssueChallanNo,replace(convert(varchar(11), PSD.OrderDate,106), ' ','-') OrderDate,'' RecChallanNo,
                            Replace(convert(varchar(11),ReceiveDate,106), ' ','-') ReceiveDate,CN.CompanyId,FromProcessId,ToProcessId,ReceiveDetailId,IssueDetailId,Pack,
                            PM.PROCESS_NAME,VF.Item_Name+' '+VF.QualityName+' '+VF.DesignName+' '+VF.ColorName+' '+VF.ShapeName+' '+VF.SizeMtr
                            + case when psd.toprocessid=1 Then ' ['+isnull( dbo.[F_GetStockActualSize](psd.stockno," + Session["varcompanyid"] + @"),'')+']' else '' end  Description, 
                            IsNull(CC.CustomerCode, '') Customercode,'' as customerorderNO,psd.Processdetailid,isnull(CN.PackingId,0) as PackingId,0 as ToProcessid,CN.Item_Finished_id,'' as UnitName,'' as LoomNo,
                            '' as StockStatus,1 as QualityType,'' as TanaLotNo, IsNull(NUD.UserName, '') UserName ,'' as Ecisno,'' as Destcode,'' as FolioChallanNo
                            From CarpetNumber CN 
                            inner join Process_Stock_Detail PSD on PSD.StockNo=CN.StockNo 
                            inner join  Process_Name_Master PM on  PSD.ToProcessId=PM.PROCESS_NAME_ID
                            inner join V_FinishedItemDetail VF on CN.Item_Finished_id=VF.Item_Finished_id                              
                            left JOIN NewUserDetail NUD ON NUD.UserId = PSD.UserId 
                            left join OrderMaster Om on CN.OrderId=OM.OrderId
                            left join customerinfo CC on OM.CustomerId=CC.customerid                             
                            Where CN.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And TSTOCKNO in (" + StrNew + ") And VF.MasterCompanyId=" + Session["varCompanyId"] + " Order By CN.TSTOCKNO,ProcessDetailId";

                DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Query);
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    DGStock.DataSource = Ds;
                    DGStock.DataBind();
                }
                else
                {
                    DGStock.DataSource = null;
                    DGStock.DataBind();
                    LblErrorMessage.Text = "Stock does not exists in ERP.";
                    return;
                }
                VarNum = 0;
                for (int i = 0; i < DGStock.Rows.Count; i++)
                {
                    //if (Convert.ToString(DGStock.Rows[i].Cells[1].Text) != "STOCK OUT")
                    //{
                      if (((Label)DGStock.Rows[i].FindControl("lblPROCESS_NAME")).Text != "STOCK OUT")
                      {

                        StrNew = DGStock.DataKeys[i].Value.ToString();
                        int VarIssueDetailId = Convert.ToInt32(StrNew.Split('|')[1]);
                        int VarReceiveDetailId = Convert.ToInt32(StrNew.Split('|')[2]);
                        int VarProcessId = Convert.ToInt32(StrNew.Split('|')[3]);
                        if (VarIssueDetailId > 0)
                        {
                            Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select replace(convert(varchar(11),PM.AssignDate,106), ' ','-') AssignDate,PM.IssueOrderId,(select * from [dbo].[Get_Employeename](PM.IssueOrderId,PD.issue_Detail_Id," + VarProcessId + ")) As EmpName,U.UnitName,isnull(plm.loomno,'') as LoomNo,isnull(PM.TanaLotNo,'') as TanaLotNo,isnull(PM.ChallanNo,PM.ISSUEORDERID) as FolioChallanNo  From PROCESS_ISSUE_MASTER_" + VarProcessId + " PM inner join PROCESS_ISSUE_DETAIL_" + VarProcessId + " PD on PM.IssueOrderId=PD.IssueOrderId  And Issue_Detail_Id=" + VarIssueDetailId + " left join Units u on PM.Units=U.UnitsId left join  productionloommaster PLM on pm.loomid=plm.uid  ");

                            //DGStock.Rows[i].Cells[2].Text = (Ds.Tables[0].Rows[0]["EmpName"]).ToString();
                            //DGStock.Rows[i].Cells[3].Text = (Ds.Tables[0].Rows[0]["IssueOrderId"]).ToString();

                            Label lblEmpName = (Label)DGStock.Rows[i].FindControl("lblEmpName");
                            Label lblIssueChallanNo = (Label)DGStock.Rows[i].FindControl("lblIssueChallanNo");
                            Label lblIssueChallanNoNew = (Label)DGStock.Rows[i].FindControl("lblIssueChallanNoNew");

                            Label lblunitname = (Label)DGStock.Rows[i].FindControl("lblunitname");
                            Label lblloomno = (Label)DGStock.Rows[i].FindControl("lblloomno");
                            Label lblTanaLotNo = (Label)DGStock.Rows[i].FindControl("lblTanaLotNo");
                            lblunitname.Text = Ds.Tables[0].Rows[0]["unitname"].ToString();
                            lblloomno.Text = Ds.Tables[0].Rows[0]["LoomNo"].ToString();
                            lblTanaLotNo.Text = Ds.Tables[0].Rows[0]["TanaLotNo"].ToString();

                            lblEmpName.Text=Ds.Tables[0].Rows[0]["EmpName"].ToString();
                            lblIssueChallanNo.Text = Ds.Tables[0].Rows[0]["IssueOrderId"].ToString();

                            lblIssueChallanNoNew.Text = Ds.Tables[0].Rows[0]["FolioChallanNo"].ToString();


                            Label lblOrderDate = (Label)DGStock.Rows[i].FindControl("lblOrderDate");
                            if (lblOrderDate.Text != "")
                            {
                                lblOrderDate.Text = Ds.Tables[0].Rows[0]["AssignDate"].ToString();
                            }
                            if (lblOrderDate.Text == "")
                            {
                                lblOrderDate.Text = Ds.Tables[0].Rows[0]["AssignDate"].ToString();
                            }
                            //if (DGStock.Rows[i].Cells[4].Text != "")
                            //{
                            //    DGStock.Rows[i].Cells[4].Text = Ds.Tables[0].Rows[0]["AssignDate"].ToString();                               
                            //}
                        }
                        if (VarReceiveDetailId > 0)
                        {
                            Label lblRecChallanNo = (Label)DGStock.Rows[i].FindControl("lblRecChallanNo");
                            switch (Session["varcompanyNo"].ToString())
                            {

                                case "8":
                                    //DGStock.Rows[i].Cells[5].Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Process_Rec_Id From PROCESS_Receive_DETAIL_" + VarProcessId + " Where Process_Rec_Detail_Id=" + VarReceiveDetailId + "").ToString();
                                    lblRecChallanNo.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Process_Rec_Id From PROCESS_Receive_DETAIL_" + VarProcessId + " Where Process_Rec_Detail_Id=" + VarReceiveDetailId + "").ToString();
                                    break;
                                default:
                                    //Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Distinct ERP.Process_Rec_id,EI.EmpName+ CASE WHEN ISNULL(EI.EMPCODE,'')<>'' THEN ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME From Employee_processreceiveno ERP  inner Join EmpInfo Ei on ERP.Empid=EI.EmpId Where ERP.processid=" + VarProcessId + " and ERP.Process_Rec_Detail_id=" + VarReceiveDetailId + "");
                                    Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT PROCESS_REC_ID,DBO.F_GETJOBRECEIVEEMPLOYEEDETAIL_PROCESSRECDETAILWISE(PROCESS_NAME_ID,PROCESS_REC_DETAIL_ID) AS EMPNAME,
                                                                                                           QUALITYTYPE FROM VIEW_PROCESS_RECEIVE_DETAIL WHERE PROCESS_NAME_ID=" + VarProcessId + " AND PROCESS_REC_DETAIL_ID=" + VarReceiveDetailId + "");
                                    if (Ds.Tables[0].Rows.Count > 0)
                                    {
                                        //DGStock.Rows[i].Cells[5].Text = Ds.Tables[0].Rows[0]["Process_rec_id"].ToString();
                                        lblRecChallanNo.Text = Ds.Tables[0].Rows[0]["Process_rec_id"].ToString();

                                        //var empname = "";
                                        //for (int j = 0; j < Ds.Tables[0].Rows.Count; j++)
                                        //{
                                        //    empname = empname + "," + Ds.Tables[0].Rows[j]["empname"];
                                        //}
                                        //DGStock.Rows[i].Cells[2].Text = empname.ToString().TrimStart(',');

                                        Label lblEmpName = (Label)DGStock.Rows[i].FindControl("lblEmpName");
                                        lblEmpName.Text = Ds.Tables[0].Rows[0]["EMpname"].ToString();

                                        //DGStock.Rows[i].Cells[2].Text = Ds.Tables[0].Rows[0]["EMpname"].ToString();
                                        
                                        Label lblqualityType = (Label)DGStock.Rows[i].FindControl("lblqualityType");
                                        lblqualityType.Text = Ds.Tables[0].Rows[0]["Qualitytype"].ToString();
                                    }
                                    //DGStock.Rows[i].Cells[5].Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Process_Rec_Id From PROCESS_Receive_DETAIL_" + VarProcessId + " Where Process_Rec_Detail_Id=" + VarReceiveDetailId + "").ToString();
                                    break;
                            }
                        }

                    }
                }
                //***************GRID LOOP FOR STOCK STATUS
                foreach (GridViewRow row in DGStock.Rows)
                {
                    Label lblqualityType = row.FindControl("lblqualityType") as Label;
                    Label lblstockstatus = row.FindControl("lblstockstatus") as Label;
                    LinkButton lnkchangestatus = row.FindControl("lnkchangestatus") as LinkButton;
                    //******STOCK STATUS
                    lnkchangestatus.Visible = false;
                    switch (lblqualityType.Text)
                    {
                        case "1":
                            lblstockstatus.Text = "Finished";

                            if (Session["usertype"].ToString() == "1")
                            {
                                lnkchangestatus.Visible = true;
                                lnkchangestatus.Text = "Change to Hold";
                            }
                            break;
                        case "2":
                            if (Session["varcompanyNo"].ToString() == "22")
                            {
                                //lblstockstatus.Text = "Finished";
                                lblstockstatus.Text = "Hold";
                                if (Session["usertype"].ToString() == "1")
                                {
                                    lnkchangestatus.Visible = true;
                                }
                                else
                                {
                                    lnkchangestatus.Visible = false;
                                }
                            }
                            else
                            {
                                lblstockstatus.Text = "Hold";
                                lnkchangestatus.Visible = true;
                            }
                            lnkchangestatus.Text = "Change to Finished";
                            // lblstockstatus.Text = "Hold";
                            //lnkchangestatus.Visible = true;
                            break;
                        case "3":
                            if (Session["varcompanyNo"].ToString() == "22")
                            {
                                //lblstockstatus.Text = "Finished";
                                lblstockstatus.Text = "Reject";
                            }
                            else
                            {
                                lblstockstatus.Text = "Reject";
                            }
                            //lblstockstatus.Text = "Reject";
                            break;
                        default:
                            break;
                    }
                    //******
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmShowStockNoDetail.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        int vendorid = Convert.ToInt16(txtvendorid.Text == "" ? "0" : txtvendorid.Text);
        string str = "select empname,address From Empinfo where empid=" + vendorid + "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblvendorname.Text = ds.Tables[0].Rows[0]["Empname"].ToString();
            lblvendoraddress.Text = ds.Tables[0].Rows[0]["address"].ToString();
        }
        else
        {
            lblvendorname.Text = "No Vendor found";
            lblvendoraddress.Text = "";
        }
    }
    protected void btnprintstockrawdetail_Click(object sender, EventArgs e)
    {
        if (Session["varCompanyId"].ToString() == "21")
        {
            GetStockRawdetailKaysons();
        }
        else
        {
            GetStockRawdetail();
        }

        ModalPopupExtender1.Show();

    }
    protected void btnpack_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@TstockNo", txtStockNo.Text);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@Remark", txtremark.Text.Trim());
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            //**
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DIRECTSTOCKPACK", param);
            if (param[3].Value.ToString() != "")
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = param[3].Value.ToString();
                Tran.Rollback();
            }
            else
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Stock No. packed successfully.";
                Tran.Commit();

            }


        }
        catch (Exception ex)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void btnstocktrace_Click(object sender, EventArgs e)
    {
        if (DGStock.Rows.Count > 0)
        {
            //int IssueOrderId = Convert.ToInt32(DGStock.Rows[0].Cells[3].Text);
            int IssueOrderId = Convert.ToInt32(((Label)DGStock.Rows[0].FindControl("lblIssueChallanNo")).Text);
            int ProcessId = Convert.ToInt16(((Label)DGStock.Rows[0].FindControl("lblProcessId")).Text);
            int Item_Finished_Id = Convert.ToInt16(((Label)DGStock.Rows[0].FindControl("lblFinishedid")).Text);
            string tstockno = ((Label)DGStock.Rows[0].FindControl("lbltstockno")).Text;
            Label lblloomno = ((Label)DGStock.Rows[0].FindControl("lblloomno"));

            string issuedate = ((Label)DGStock.Rows[0].FindControl("lblOrderDate")).Text;
            string Recdate = ((Label)DGStock.Rows[0].FindControl("lblReceiveDate")).Text;

            //string issuedate = DGStock.Rows[0].Cells[4].Text;
            //string Recdate = DGStock.Rows[0].Cells[6].Text;
            string Finissdate = "", Finrecdate = "", Packdate = "", Dispatchdate = "";

            for (int i = 0; i < DGStock.Rows.Count; i++)
            {

                //string Jobname = DGStock.Rows[i].Cells[1].Text.ToUpper();

                string Jobname = ((Label)DGStock.Rows[0].FindControl("lblPROCESS_NAME")).Text;

                switch (Jobname)
                {
                    case "FINISHING":
                    case "FINISHING-1":
                        //Finissdate = DGStock.Rows[i].Cells[4].Text;
                        //Finrecdate = DGStock.Rows[i].Cells[6].Text;

                        Finissdate = ((Label)DGStock.Rows[0].FindControl("lblOrderDate")).Text;
                        Finrecdate = ((Label)DGStock.Rows[0].FindControl("lblReceiveDate")).Text;
                        break;
                    case "PACKING":
                        //Packdate = DGStock.Rows[i].Cells[6].Text;
                        Packdate = ((Label)DGStock.Rows[0].FindControl("lblReceiveDate")).Text;
                        break;
                    case "STOCK OUT":
                        //Dispatchdate = DGStock.Rows[i].Cells[4].Text;
                        Dispatchdate = ((Label)DGStock.Rows[0].FindControl("lblOrderDate")).Text;
                        break;
                    default:
                        break;
                }
            }

            SqlParameter[] param = new SqlParameter[11];
            param[0] = new SqlParameter("@TstockNo", tstockno);
            param[1] = new SqlParameter("@Item_finished_id", Item_Finished_Id);
            param[2] = new SqlParameter("@Processid", SqlDbType.Int);
            param[2].Value = 1;
            param[3] = new SqlParameter("@issueorderid", IssueOrderId);
            param[4] = new SqlParameter("@issuedate", issuedate);
            param[5] = new SqlParameter("@Receivedate", Recdate);
            param[6] = new SqlParameter("@LoomNo", lblloomno.Text);
            param[7] = new SqlParameter("@FINISHINGISSUEDATE", Finissdate);
            param[8] = new SqlParameter("@FINISHINGRECDATE", Finrecdate);
            param[9] = new SqlParameter("@PACKINGRECDATE", Packdate);
            param[10] = new SqlParameter("@Dispatchdate", Dispatchdate);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETSTOCKNOTRACEABILITYREPORT", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;

                GridView1.DataSource = ds;
                GridView1.DataBind();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition",
                 "attachment;filename=StockNoTrace" + DateTime.Now + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //Apply text style to each Row
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            else
            {
                LblErrorMessage.Text = "No records found for this combination.";
            }
        }
    }
    protected void GDLinkedtoCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            switch (Session["varcompanyId"].ToString())
            {
                case "22":
                    GDLinkedtoCustomer.HeaderRow.Cells[4].Text = "Secondary LotNo.";                   
                    break;
                default:
                    GDLinkedtoCustomer.HeaderRow.Cells[4].Text = "Tag No.";
                    break;
            }

            for (int i = 0; i < GDLinkedtoCustomer.Columns.Count; i++)
            {
                switch (Session["varcompanyId"].ToString())
                {
                    case "14":
                    case "45":
                    case "8":
                        if (GDLinkedtoCustomer.Columns[i].HeaderText == "Tag No.")
                        {
                            GDLinkedtoCustomer.Columns[i].Visible = false;
                        }
                        break;
                    default:
                        break;
                }

            }
        }
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = new DataSet();
        int rowindex = Convert.ToInt16(hngridrowindex.Value);
        //int IssueOrderId = Convert.ToInt32(DGStock.Rows[rowindex].Cells[3].Text);

        int IssueOrderId = Convert.ToInt32(((Label)DGStock.Rows[rowindex].FindControl("lblIssueChallanNo")).Text);
        int ProcessId = Convert.ToInt16(((Label)DGStock.Rows[rowindex].FindControl("lblProcessId")).Text);
        int Item_Finished_Id = Convert.ToInt16(((Label)DGStock.Rows[rowindex].FindControl("lblFinishedid")).Text);
        string Tstockno = ((Label)DGStock.Rows[rowindex].FindControl("lbltstockno")).Text;

        //string ReceiveDate = DGStock.Rows[rowindex].Cells[6].Text;
        string ReceiveDate = ((Label)DGStock.Rows[rowindex].FindControl("lblReceiveDate")).Text;

        //////*****************
        ////SqlParameter[] param = new SqlParameter[5];
        ////param[0] = new SqlParameter("@processid", ProcessId);
        ////param[1] = new SqlParameter("@Finishedid", Item_Finished_Id);
        ////param[2] = new SqlParameter("@issueorderid", IssueOrderId);
        ////param[3] = new SqlParameter("@Receivedate", ReceiveDate);
        ////param[4] = new SqlParameter("@Tstockno", Tstockno);

        ////ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetStockRawdetail", param);


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_GetStockRawdetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@processid", ProcessId);
        cmd.Parameters.AddWithValue("@Finishedid", Item_Finished_Id);
        cmd.Parameters.AddWithValue("@issueorderid", IssueOrderId);
        cmd.Parameters.AddWithValue("@Receivedate", ReceiveDate);
        cmd.Parameters.AddWithValue("@Tstockno", Tstockno);
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);


        return ds;
    }
    protected void GetStockRawdetail()
    {          

        DataSet ds = Fill_Grid_Data();      

        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1"); 
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:N1").Merge();
            sht.Range("A1:N1").Style.Font.FontSize = 11;
            sht.Range("A1:N1").Style.Font.Bold = true;
            sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:N1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:N1").Style.Alignment.WrapText = true;
            //************
            ///////sht.Range("A1").SetValue("LOOM BAL.-" + FilterBy);
            //Detail headers       

            sht.Column("A").Width = 9.44;
            sht.Column("B").Width = 14.11;
            sht.Column("C").Width = 14.22;
            sht.Column("D").Width = 22.33;
            sht.Column("E").Width = 15.89;
            sht.Column("F").Width = 15;
            sht.Column("G").Width = 22.22;
            sht.Column("H").Width = 34.78;
            sht.Column("I").Width = 34.78;
            sht.Column("J").Width = 34.78;
            sht.Column("K").Width = 34.78;
            sht.Column("L").Width = 34.78;
            sht.Column("M").Width = 34.78;
            sht.Column("N").Width = 34.78;


            sht.Range("A2:N2").Style.Font.FontSize = 11;
            sht.Range("A2:N2").Style.Font.Bold = true;

            sht.Range("A2").Value = "STOCKNO";
            sht.Range("B2").Value = "LOTNO";
            sht.Range("C2").Value = "TAGNO";
            sht.Range("D2").Value = "ITEM DESCRIPTION";
            sht.Range("E2").Value = "ITEM_NAME";
            sht.Range("F2").Value = "QUALITY NAME";
            sht.Range("G2").Value = "SHADECOLOR NAME";
            sht.Range("H2").Value = "PURCHASE DETAIL";
            sht.Range("I2").Value = "DYEING DETAIL";
            sht.Range("J2").Value = "YOP DETAIL";
            sht.Range("K2").Value = "WARPING DETAIL";

            if (Session["VarCompanyNo"].ToString() == "14" || Session["VarCompanyNo"].ToString() == "45")
            {
                sht.Range("L2").Value = "WEAVING DETAIL";
            }
            else
            {
                sht.Range("L2").Value = "";
            }


            int row = 3;
            ////***************

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                sht.Range("A" + row + ":N" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A" + row + ":N" + row).Style.Font.FontSize = 10;
                sht.Range("A" + row + ":N" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);


                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["STOCKNO"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["LOTNO"]);

                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["TAGNO"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ITEMDESCRIPTION"]);
                sht.Range("D" + row).Style.Alignment.SetWrapText();
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["SHADECOLORNAME"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["PURCHASEDETAIL"]);
                sht.Range("H" + row).Style.Alignment.SetWrapText();
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["DYEINGDETAIL"]);
                sht.Range("I" + row).Style.Alignment.SetWrapText();
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["YOPDETAIL"]);
                sht.Range("J" + row).Style.Alignment.SetWrapText();

                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["WARPINGDETAIL"]);
                sht.Range("K" + row).Style.Alignment.SetWrapText();

                if (Session["VarCompanyNo"].ToString() == "14" || Session["VarCompanyNo"].ToString() == "45")
                {
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["WEAVINGDETAIL"]);
                    sht.Range("L" + row).Style.Alignment.SetWrapText();
                }
                else
                {
                    sht.Range("L" + row).SetValue("");
                }



                row = row + 1;
            }

            ds.Dispose();
            ds.Dispose();

            ////sht.Columns(1, 25).AdjustToContents();
            //************** Save            

            using (var a = sht.Range("A1" + ":N" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            if (Session["VarCompanyNo"].ToString() == "14" || Session["VarCompanyNo"].ToString() == "45")
            {
                row = row + 3;
                int RowFrom = row;

                sht.Range("A" + row).Value = "Stock No.";
                sht.Range("B" + row).Value = "Process_Name";
                sht.Range("C" + row).Value = "Emp_Name";
                sht.Range("D" + row).Value = "IssueChallanNo";
                sht.Range("E" + row).Value = "IssueDate";
                sht.Range("F" + row).Value = "RecChallanNo";
                sht.Range("G" + row).Value = "ReceiveDate";
                sht.Range("H" + row).Value = "Description";
                sht.Range("I" + row).Value = "Unit Name";
                sht.Range("J" + row).Value = "Loom No.";
                sht.Range("K" + row).Value = "Stock Status";
                sht.Range("L" + row).Value = "UserName";
                sht.Range("M" + row).Value = "Cust Code";
                sht.Range("N" + row).Value = "ECIS NO & DEST. CODE";

                sht.Range("A" + row + ":N" + row).Style.Font.FontSize = 11;
                sht.Range("A" + row + ":N" + row).Style.Font.Bold = true;

                for (int j = 0; j < DGStock.Rows.Count; j++)
                {
                    row = row + 1;

                    Label lbltstockno = (Label)DGStock.Rows[j].FindControl("lbltstockno");
                    Label lblPROCESS_NAME = (Label)DGStock.Rows[j].FindControl("lblPROCESS_NAME");
                    Label lblEmpName = (Label)DGStock.Rows[j].FindControl("lblEmpName");
                    Label lblIssueChallanNo = (Label)DGStock.Rows[j].FindControl("lblIssueChallanNo");
                    Label lblOrderDate = (Label)DGStock.Rows[j].FindControl("lblOrderDate");
                    Label lblRecChallanNo = (Label)DGStock.Rows[j].FindControl("lblRecChallanNo");
                    Label lblReceiveDate = (Label)DGStock.Rows[j].FindControl("lblReceiveDate");
                    Label lblDescription = (Label)DGStock.Rows[j].FindControl("lblDescription");
                    Label lblunitname = (Label)DGStock.Rows[j].FindControl("lblunitname");
                    Label lblloomno = (Label)DGStock.Rows[j].FindControl("lblloomno");

                    Label lblstockstatus = (Label)DGStock.Rows[j].FindControl("lblstockstatus");

                    Label lblUserName = (Label)DGStock.Rows[j].FindControl("lblUserName");
                    Label lblCustomerCode = (Label)DGStock.Rows[j].FindControl("lblCustomerCode");
                    Label lblECISNoDestCode = (Label)DGStock.Rows[j].FindControl("lblECISNoDestCode");
                    Label lblDestCode = (Label)DGStock.Rows[j].FindControl("lblDestCode");

                    sht.Range("A" + row + ":N" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":N" + row).Style.Font.FontSize = 10;
                    sht.Range("A" + row + ":N" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);


                    sht.Range("A" + row).SetValue(lbltstockno.Text);
                    sht.Range("B" + row).SetValue(lblPROCESS_NAME.Text);

                    sht.Range("C" + row).SetValue(lblEmpName.Text);
                    sht.Range("C" + row).Style.Alignment.SetWrapText();
                    sht.Range("D" + row).SetValue(lblIssueChallanNo.Text);
                    sht.Range("D" + row).Style.Alignment.SetWrapText();
                    sht.Range("E" + row).SetValue(lblOrderDate.Text);
                    sht.Range("F" + row).SetValue(lblRecChallanNo.Text);
                    sht.Range("G" + row).SetValue(lblReceiveDate.Text);
                    sht.Range("H" + row).SetValue(lblDescription.Text);
                    sht.Range("H" + row).Style.Alignment.SetWrapText();
                    sht.Range("I" + row).SetValue(lblunitname.Text);
                    sht.Range("I" + row).Style.Alignment.SetWrapText();
                    sht.Range("J" + row).SetValue(lblloomno.Text);
                    sht.Range("J" + row).Style.Alignment.SetWrapText();

                    sht.Range("K" + row).SetValue(lblstockstatus.Text);
                    sht.Range("K" + row).Style.Alignment.SetWrapText();
                    sht.Range("L" + row).SetValue(lblUserName.Text);
                    sht.Range("L" + row).Style.Alignment.SetWrapText();
                    sht.Range("M" + row).SetValue(lblCustomerCode.Text);
                    sht.Range("N" + row).SetValue (lblECISNoDestCode.Text + ", " + lblDestCode.Text);
                    sht.Range("N" + row).Style.NumberFormat.Format = "@";

                }

                //***********BOrders
                using (var a = sht.Range("A" + RowFrom + ":N" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }
            }
              

            String Path;
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("STOCKRAWDETAIL" + DateTime.Now + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //////Download File
            Response.ClearContent();
            Response.ClearHeaders();
            ////// Response.Clear();
            ////Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            //// File.Delete(Path);
            Response.End(); 


        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('No records found')", true);
        }


        ////Export to excel
        //GridView GridView1 = new GridView();
        //GridView1.AllowPaging = false;

        //GridView1.DataSource = ds;
        //GridView1.DataBind();

        //Response.Clear();
        //Response.Buffer = true;
        //Response.AddHeader("content-disposition",
        // "attachment;filename=STOCKRAWDETAIL" + DateTime.Now + ".xls");
        //Response.Charset = "";
        //Response.ContentType = "application/vnd.ms-excel";
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(sw);

        //for (int i = 0; i < GridView1.Rows.Count; i++)
        //{
        //    //Apply text style to each Row
        //    GridView1.Rows[i].Attributes.Add("class", "textmode");
        //}
        //GridView1.RenderControl(hw);

        ////style to format numbers to string
        //string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        //Response.Write(style);
        //Response.Output.Write(sw.ToString());
        //Response.Flush();
        //Response.End();
    }
    protected void GetStockRawdetailKaysons()
    {
        DataSet ds = Fill_Grid_Data();

        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:P1").Merge();
            sht.Range("A1:P1").Style.Font.FontSize = 11;
            sht.Range("A1:P1").Style.Font.Bold = true;
            sht.Range("A1:P1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:P1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:P1").Style.Alignment.WrapText = true;
            //************
            ///////sht.Range("A1").SetValue("LOOM BAL.-" + FilterBy);
            //Detail headers       

            sht.Column("A").Width = 20.44;
            sht.Column("B").Width = 14.11;
            sht.Column("C").Width = 14.22;
            sht.Column("D").Width = 22.33;
            sht.Column("E").Width = 22.89;
            sht.Column("F").Width = 15;
            sht.Column("G").Width = 22.22;
            sht.Column("H").Width = 34.78;
            sht.Column("I").Width = 34.78;
            sht.Column("J").Width = 34.78;
            sht.Column("K").Width = 34.78;
            sht.Column("L").Width = 34.78;
            sht.Column("M").Width = 34.78;
            sht.Column("N").Width = 34.78;
            sht.Column("O").Width = 34.78;
            sht.Column("P").Width = 34.78;


            sht.Range("A2:P2").Style.Font.FontSize = 11;
            sht.Range("A2:P2").Style.Font.Bold = true;

            sht.Range("A2").Value = "PACKING BARCODE";
            sht.Range("B2").Value = "STOCKNO";
            sht.Range("C2").Value = "LOTNO";
            sht.Range("D2").Value = "TAGNO";
            sht.Range("E2").Value = "ITEM DESCRIPTION";
            sht.Range("F2").Value = "ITEM_NAME";
            sht.Range("G2").Value = "QUALITY NAME";
            sht.Range("H2").Value = "SHADECOLOR NAME";
            sht.Range("I2").Value = "PURCHASE DETAIL";
            sht.Range("J2").Value = "DYEING DETAIL";
            sht.Range("K2").Value = "YOP DETAIL";
            sht.Range("L2").Value = "WEFTD ETAIL";
            sht.Range("M2").Value = "WARPING DETAIL";
            sht.Range("N2").Value = "FINISHING DETAIL";
            sht.Range("O2").Value = "FRINGING_FINISHING DETAIL";
            sht.Range("P2").Value = "STITCHING DETAIL";



            int row = 3;
            ////***************

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                sht.Range("A" + row + ":P" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A" + row + ":P" + row).Style.Font.FontSize = 10;
                sht.Range("A" + row + ":P" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);


                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["PackingBarCodeNo"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["STOCKNO"]);

                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["LOTNO"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["TAGNO"]);
                sht.Range("D" + row).Style.Alignment.SetWrapText();
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ITEMDESCRIPTION"]);
                sht.Range("E" + row).Style.Alignment.SetWrapText();
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["SHADECOLORNAME"]);
                sht.Range("H" + row).Style.Alignment.SetWrapText();
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["PURCHASEDETAIL"]);
                sht.Range("I" + row).Style.Alignment.SetWrapText();
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["DYEINGDETAIL"]);
                sht.Range("J" + row).Style.Alignment.SetWrapText();
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["YOPDETAIL"]);
                sht.Range("K" + row).Style.Alignment.SetWrapText();
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["WEFTDETAIL"]);
                sht.Range("L" + row).Style.Alignment.SetWrapText();
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["WARPINGDETAIL"]);
                sht.Range("M" + row).Style.Alignment.SetWrapText();

                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["FinishingDetail"]);
                sht.Range("N" + row).Style.Alignment.SetWrapText();
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["FRINGING_FINISHINGDetail"]);
                sht.Range("O" + row).Style.Alignment.SetWrapText();

                sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["StitchingDetail"]);
                sht.Range("P" + row).Style.Alignment.SetWrapText();


                row = row + 1;
            }
            ds.Dispose();
            ds.Dispose();

            ////sht.Columns(1, 25).AdjustToContents();
            //************** Save

            //***********BOrders
            using (var a = sht.Range("A1" + ":P" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            String Path;

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("STOCKRAWDETAIL" + DateTime.Now + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('No records found')", true);
        }
    }
    protected void lnkbtnName_Click(object sender, EventArgs e)
    {
       
        ModalPopupExtender1.Show();
        DataSet ds = Fill_Grid_Data();
        GDLinkedtoCustomer.DataSource = ds;
        GDLinkedtoCustomer.DataBind();
        
    }

    protected void txtPackingBarCode_TextChanged(object sender, EventArgs e)
    {
        if (txtPackingBarCode.Text != "")
        {
            LblErrorMessage.Text = "";
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@TUniqueSeqNo", txtPackingBarCode.Text);
                param[1] = new SqlParameter("@userid", Session["varuserid"]);
                param[2] = new SqlParameter("@MasterCompanyId", Session["VarCompanyId"]);
                param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[3].Direction = ParameterDirection.Output;
                //**
                ////SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DIRECTSTOCKPACK", param);

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETPackingBarCodeStockNo", param);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtStockNo.Text = ds.Tables[0].Rows[0]["TStockNo"].ToString();
                    txtStockNo_TextChanged(txtStockNo, new EventArgs());

                    //LblErrorMessage.Visible = true;
                    //LblErrorMessage.Text = param[3].Value.ToString();
                    //Tran.Rollback();
                }
                else
                {
                    txtStockNo.Text = "";
                    DGStock.DataSource = null;
                    DGStock.DataBind();
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = "Packing BarCode Does Not Belong To StockNo";
                    Tran.Commit();
                }
            }
            catch (Exception ex)
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void BtnPreviewGridData_Click(object sender, EventArgs e)
    {
        Report();
    }
    protected void Report()
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "STOCKNOSTATUSREORT" + DateTime.Now + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        DGStock.GridLines = GridLines.Both;
        DGStock.HeaderStyle.Font.Bold = true;
        DGStock.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        Response.End();
    }
    private DataSet Fill_Grid_JobSeq_Data()
    {
        DataSet ds = new DataSet();
        int rowindex = Convert.ToInt16(hngridrowindex.Value);
        //int IssueOrderId = Convert.ToInt32(DGStock.Rows[rowindex].Cells[3].Text);

        int IssueOrderId = Convert.ToInt32(((Label)DGStock.Rows[rowindex].FindControl("lblIssueChallanNo")).Text);
        int ProcessId = Convert.ToInt16(((Label)DGStock.Rows[rowindex].FindControl("lblProcessId")).Text);
        int Item_Finished_Id = Convert.ToInt16(((Label)DGStock.Rows[rowindex].FindControl("lblFinishedid")).Text);
        string Tstockno = ((Label)DGStock.Rows[rowindex].FindControl("lbltstockno")).Text;

        //string ReceiveDate = DGStock.Rows[rowindex].Cells[6].Text;
        string ReceiveDate = ((Label)DGStock.Rows[rowindex].FindControl("lblReceiveDate")).Text;

        //////*****************
        ////SqlParameter[] param = new SqlParameter[5];
        ////param[0] = new SqlParameter("@processid", ProcessId);
        ////param[1] = new SqlParameter("@Finishedid", Item_Finished_Id);
        ////param[2] = new SqlParameter("@issueorderid", IssueOrderId);
        ////param[3] = new SqlParameter("@Receivedate", ReceiveDate);
        ////param[4] = new SqlParameter("@Tstockno", Tstockno);

        ////ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetStockRawdetail", param);


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GETSTOCKJOBSEQUENCEDETAIL", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@processid", ProcessId);
        cmd.Parameters.AddWithValue("@Finishedid", Item_Finished_Id);
        cmd.Parameters.AddWithValue("@issueorderid", IssueOrderId);
        cmd.Parameters.AddWithValue("@Receivedate", ReceiveDate);
        cmd.Parameters.AddWithValue("@Tstockno", Tstockno);
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);


        return ds;
    }
    protected void LnkBtnJobSequence_Click(object sender, EventArgs e)
    {

        ModalPopupExtender2.Show();

        DataSet ds = Fill_Grid_JobSeq_Data();

        if (ds.Tables[0].Rows.Count > 0)
        {
            GridViewJobSequence.DataSource = ds;
            GridViewJobSequence.DataBind();
        }
        else
        {
            GridViewJobSequence.DataSource = null;
            GridViewJobSequence.DataBind();
        }

    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }
}