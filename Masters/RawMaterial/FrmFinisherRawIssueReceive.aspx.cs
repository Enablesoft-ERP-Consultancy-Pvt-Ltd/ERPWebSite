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

public partial class Masters_RawMaterial_FrmFinisherRawIssueReceive : System.Web.UI.Page
{
    public int UnitId = 0;
    static int rowindex = 0;
    static string btnclickflag = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {           

            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName
                           select GoDownID,GodownName from GodownMaster where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by GodownName
                           select Process_Name_ID,Process_Name from process_name_master where ProcessType=1 and PROCESS_NAME_ID<>1 order by Process_Name";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
                BindDyerName();
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDGodownName, ds, 1, true, "--Plz Select--");

            if (DDGodownName.Items.Count > 0)
            {
                DDGodownName.SelectedIndex = 2;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDJobType, ds, 2, true, "--Plz Select--");

            TxtAssignDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            BindQualityType();           
        }
    }
    protected void ChKForEdit_CheckedChanged(object sender, EventArgs e)
    {
        EditCheckedChanged();        
    }
    private void EditCheckedChanged()
    {
        if (ChKForEdit.Checked == true)
        {
            btncancelorder.Visible = true;
            TDLabelChallanNo.Visible = true;
            TDTxtChallanNo.Visible = true;
            DDCompanyName.Enabled = false;
            DDJobType.Enabled = false;
            DDFinisherName.Enabled = false;
            DDTranType.Enabled = false;
            TR2.Visible = false;
            TR3.Visible = false;
            TR4.Visible = false;
            TR5.Visible = false;
        }
        else
        {
            btncancelorder.Visible = false;
            TDLabelChallanNo.Visible = false;
            TDTxtChallanNo.Visible = false;
            txtEditChallanNo.Text = "";
            DDCompanyName.Enabled = true;
            DDJobType.Enabled = true;
            DDFinisherName.Enabled = true;
            DDTranType.Enabled = true;
            TR2.Visible = true;
            TR3.Visible = true;
            TR4.Visible = true;
            TR5.Visible = true;

           DG.DataSource = "";
            DG.DataBind();
            DDCompanyName.SelectedIndex = -1;
            DDJobType.SelectedIndex = -1;
            DDFinisherName.SelectedIndex = -1;
            hnid.Value = "0";
            txtChallanNo.Text = "";
        }
    }
    private void EditChallanNo()
    {
       // HPRMID.Value = ddPartyChallanNo.SelectedValue;
        string Str = @"Select replace(convert(varchar(11),isnull(TranDate,''),106), ' ','-') TranDate,FRM.CompanyId,FRM.ChallanNo,FRM.ProcessId,FRM.EmpId,FRM.TranType,FRM.TranId 
            From finisherRawMaster FRM Where FRM.CompanyID = " + DDCompanyName.SelectedValue + " And FRM.ChallanNo=" + txtEditChallanNo.Text + " and FRM.Status<>'CancelOrder'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtAssignDate.Text = ds.Tables[0].Rows[0]["Trandate"].ToString();
            DDJobType.SelectedValue = ds.Tables[0].Rows[0]["ProcessId"].ToString();
            DDFinisherName.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
            BindDyerName();
            DDTranType.SelectedValue = ds.Tables[0].Rows[0]["TranType"].ToString();
            hnid.Value = ds.Tables[0].Rows[0]["TranId"].ToString();
            txtChallanNo.Text = ds.Tables[0].Rows[0]["ChallanNo"].ToString();
            Fillgrid();
        }
        else
        {
            DG.DataSource = "";
            DG.DataBind();
            DDJobType.SelectedIndex = -1;
            DDFinisherName.SelectedIndex = -1;
            hnid.Value = "0";
            txtChallanNo.Text = "";

        }
    }
    protected void txtEditChallanNo_TextChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        if (txtEditChallanNo.Text != "")
        {
            EditChallanNo();
        }        
    }
    protected void DDJobType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDyerName();
    }
    protected void DDTranType_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = "0";
        //DDItemName.Items.Clear();
        DDQuality.Items.Clear();
        DDShadeColor.Items.Clear();
        DDLotNo.Items.Clear();
        txtIssueQty.Text = "";
        txtChallanNo.Text = "";
      
    }
    private void BindDyerName()
    {
        UtilityModule.ConditionalComboFill(ref DDFinisherName, "select EI.EmpId,EI.EmpName from EmpInfo EI INNER JOIN EmpProcess EP ON EI.EmpId=EP.EmpId Where EP.ProcessId=" + DDJobType.SelectedValue + " Order by EI.EmpName", true, "--Plz Select--");
        //UtilityModule.ConditionalComboFill(ref DDFinisherName, "select EI.EmpId,EI.EmpName+' '+'S/O'+' '+ isnull(EI.FatherName,'') as EmpName from EmpInfo EI INNER JOIN EmpProcess EP ON EI.EmpId=EP.EmpId Where EP.ProcessId=" + DDJobType.SelectedValue + " Order by EI.EmpName", true, "--Plz Select--");
    }
    private void BindQualityType()
    {
        UtilityModule.ConditionalComboFill(ref DDQualityType, "select ITEM_ID,ITEM_NAME from ITEM_MASTER IM INNER JOIN CategorySeparate CS ON IM.CATEGORY_ID=CS.Categoryid where CS.id=0 and IM.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" Order by IM.Item_Name", true, "--Plz Select--");

    }
    private void BindItemName()
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "select ITEM_ID,ITEM_NAME from ITEM_MASTER IM INNER JOIN CategorySeparate CS ON IM.CATEGORY_ID=CS.Categoryid where CS.id=1 and IM.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" Order by IM.Item_Name", true, "--Plz Select--");

    } 
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDCompanyName.SelectedIndex > 0)
        {
            BindDyerName();
        }
    }
    protected void DDFinisherName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDFinisherName.SelectedIndex > 0)
        {
            BindItemName();            
        }
    }
    private void BindQuality()
    {
        UtilityModule.ConditionalComboFill(ref DDQuality, "select QualityId,QualityName from Quality where Item_Id=" + DDItemName.SelectedValue + " and MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" Order by QualityName", true, "--Plz Select--");
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDItemName.SelectedIndex > 0)
        {
            BindQuality();

            string str = @"select Distinct U.UnitId,u.UnitName from Item_master IM inner join UNIT_TYPE_MASTER UT on IM.UnitTypeID=UT.UnitTypeID 
                            inner join Unit u on U.UnitTypeID=UT.UnitTypeID and Im.ITEM_ID=" + DDItemName.SelectedValue;
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            if (ds.Tables[0].Rows.Count > 0)
            {
                lblUnitId.Text = ds.Tables[0].Rows[0]["UnitId"].ToString();
            }
        }
    }
    private void BindShadeColor()
    {
        string str = "";
        if (MySession.Stockapply == "True")
        {
            str = @"select distinct SC.ShadecolorId,SC.ShadeColorName  from ITEM_PARAMETER_MASTER IPM 
                INNER JOIN stock S ON IPM.ITEM_FINISHED_ID=S.ITEM_FINISHED_ID INNER JOIN ShadeColor SC ON IPM.SHADECOLOR_ID=SC.ShadecolorId 
                Where IPM.ITEM_ID=" + DDItemName.SelectedValue + " and IPM.QUALITY_ID=" + DDQuality.SelectedValue + " and S.Qtyinhand>0  and S.Godownid=" + DDGodownName.SelectedValue + "";
        }
        else
        {
            str = @"select distinct SC.ShadecolorId,SC.ShadeColorName  from ITEM_PARAMETER_MASTER IPM 
                INNER JOIN stock S ON IPM.ITEM_FINISHED_ID=S.ITEM_FINISHED_ID INNER JOIN ShadeColor SC ON IPM.SHADECOLOR_ID=SC.ShadecolorId 
                Where IPM.ITEM_ID=" + DDItemName.SelectedValue + " and IPM.QUALITY_ID=" + DDQuality.SelectedValue + "  and S.Godownid=" + DDGodownName.SelectedValue + "";
        }   

        UtilityModule.ConditionalComboFill(ref DDShadeColor, str, true, "--SELECT--");
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDQuality.SelectedIndex > 0)
        {
            BindShadeColor();
            DDLotNo.Items.Clear();
            //UtilityModule.ConditionalComboFill(ref DDLotNo, "", true, "--Plz Select--");
            txtQtyInHand.Text = "";
            //FillWeaverRawSubItemRate();
        }
    }
//    private void FillWeaverRawSubItemRate()
//    {
//        if (DDTranType.SelectedValue == "0")
//        {
//            string str = @"select WR.Rate from WeaverRawSubItemRate WR Where WR.ItemId=" + DDItemName.SelectedValue + " and WR.QualityId=" + DDQuality.SelectedValue + @" 
//                and WR.EffectiveDate<='" + TxtAssignDate.Text + "' and (WR.TODate>'" + TxtAssignDate.Text + "' or TODate is null)";
//            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
//            if (ds.Tables[0].Rows.Count > 0)
//            {
//                lblRate.Text = ds.Tables[0].Rows[0]["Rate"].ToString();
//            }
//            else
//            {
//                lblRate.Text = "0";
//            }
//        }
//        else
//        {
//            lblRate.Text = "0";
//        }
//    }
    
    protected void DDShadeColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDShadeColor.SelectedIndex > 0)
        {
            int Varfinishedid = UtilityModule.getItemFinishedIdForDyer(Convert.ToInt32(DDItemName.SelectedValue), Convert.ToInt32(DDQuality.SelectedValue), 0, 0, 0, 0, "", Convert.ToInt32(DDShadeColor.SelectedValue), 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            FillLotno(Varfinishedid);
        }
        else
        {
            DDLotNo.Items.Clear();
            //UtilityModule.ConditionalComboFill(ref DDLotNo, "", true, "--Plz Select--");
            txtQtyInHand.Text = "";
        }
    }
    protected void FillLotno(int varfinishedid)
    {
        string str = "";
        if (MySession.Stockapply == "True")
        {
            str = "select Distinct LotNo,LotNo as LotNo1 From Stock Where ITEM_FINISHED_ID=" + varfinishedid + " and Companyid=" + DDCompanyName.SelectedValue + " and Godownid=" + DDGodownName.SelectedValue + " And Round(Qtyinhand,3)>0 order by LotNo1";
        }
        else
        {
            str = "select Distinct LotNo,LotNo as LotNo1 From Stock Where ITEM_FINISHED_ID=" + varfinishedid + " and Companyid=" + DDCompanyName.SelectedValue + " and Godownid=" + DDGodownName.SelectedValue + "  order by LotNo1";
        }
        UtilityModule.ConditionalComboFill(ref DDLotNo, str, true, "--Plz Select--");
        if (DDLotNo.Items.Count > 0)
        {
            DDLotNo.SelectedIndex = 1;
            DDLotNo_SelectedIndexChanged(DDLotNo, new EventArgs());            
        }
    }
    //protected void FillTagno(int varfinishedid)
    //{
    //    string str = "";
    //    if (MySession.Stockapply == "True")
    //    {
    //        str = "select Distinct Tagno,Tagno as Tagno1 From Stock Where ITEM_FINISHED_ID=" + varfinishedid + " and Companyid=" + DDCompanyName.SelectedValue + " and Godownid=" + DDgodown.SelectedValue + " And LotNo='" + DDLotno.SelectedItem.Text + "' And Round(Qtyinhand,3)>0 order by Tagno1";
    //    }
    //    else
    //    {
    //        str = "select Distinct Tagno,Tagno as Tagno1 From Stock Where ITEM_FINISHED_ID=" + varfinishedid + " and Companyid=" + DDCompanyName.SelectedValue + " and Godownid=" + DDgodown.SelectedValue + " and LotNo='" + DDLotno.SelectedItem.Text + "'  order by tagNo1";
    //    }
    //    UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "--Plz Select--");
    //    if (DDTagNo.Items.Count > 0)
    //    {
    //        DDTagNo.SelectedIndex = 1;
    //        DDTagNo_SelectedIndexChanged(DDTagNo, new EventArgs());
    //    }
    //}
  
    protected void FillstockQty(int varfinishedid)
    {
        if (DDTranType.SelectedValue == "0")
        {            
            lblStockQty.Visible = true;
            txtStockQty.Visible = true; 
            string Lotno, TagNo = "";
            Lotno = DDLotNo.SelectedItem.Text;
            TagNo = "Without Tag No";
            txtQtyInHand.Text = Convert.ToString(UtilityModule.getstockQty(DDCompanyName.SelectedValue, DDGodownName.SelectedValue, Lotno, varfinishedid, TagNo));
        }
        else if (DDTranType.SelectedValue == "1")
        {
            txtQtyInHand.Text = "0";
            lblStockQty.Visible = false;
            txtStockQty.Visible = false;
        }    
        
    }
    protected void Fillgrid()
    {
        DG.DataSource = fill_Data_grid();
        DG.DataBind();
    }
    protected DataSet fill_Data_grid()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string str = @"select FRM.TranId,FRT.TranDetailId,VF.ITEM_NAME as ItemName,VF.QualityName,VF.ShadeColorName,IM.ITEM_NAME as QualityType,isnull(FRT.IssRecQty,0) as IssRecQty,isnull(FRT.Rate,0) as Rate,
                        FRT.LotNo,GM.GodownName,FRT.Finishedid,FRM.ChallanNo,FRM.CompanyId,replace(convert(varchar(11),FRM.TranDate,106), ' ','-') as TranDate,FRM.TranType,FRM.ProcessId,
						FRT.PendingIssRecQty,PNM.PROCESS_NAME,FRT.GodownId,FRT.QualityTypeId,FRT.UnitId
                        From FinisherRawMaster FRM inner join FinisherRawTran FRT on FRM.TranId=FRT.TranId 
                        INNER JOIN V_FinishedItemDetail VF ON FRT.FinishedId=VF.ITEM_FINISHED_ID
                        INNER JOIN Item_Master IM ON FRT.QualityTypeId=IM.ITEM_ID
                        INNER JOIN GodownMaster GM ON FRT.godownId=GM.GoDownID 
						INNER JOIN Process_Name_Master PNM ON FRM.ProcessId=PNM.PROCESS_NAME_ID
						Where 1=1";
            if (ChKForEdit.Checked == true)
            {
                str = str + " And FRM.ChallanNo=" + txtEditChallanNo.Text;

                str = str + " and FRM.Status<>'CancelOrder'";                
            }
            else
            {
                str = str + " and FRM.TranId=" + hnid.Value;                
            }
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
        }
        catch (Exception ex)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
            Logs.WriteErrorLog("Masters_Rawmeterial_FinisherRawIssueReceive|fill_Data_grid|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        return ds;
    }
    protected void btnsave_Click(object sender, EventArgs e)
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
            SqlParameter[] arr = new SqlParameter[27];
            arr[0] = new SqlParameter("@TranID", SqlDbType.Int);
            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = hnid.Value;
            arr[1] = new SqlParameter("@companyId", DDCompanyName.SelectedValue);            
            arr[2] = new SqlParameter("@empid", DDFinisherName.SelectedValue);           
            arr[3] = new SqlParameter("@TranType", DDTranType.SelectedValue);
            arr[4] = new SqlParameter("@TranDate", TxtAssignDate.Text);
            arr[5] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 50);
            arr[5].Direction = ParameterDirection.InputOutput;
            arr[5].Value = txtChallanNo.Text;
            arr[6] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            arr[7] = new SqlParameter("@TranDetailId", SqlDbType.Int);
            arr[7].Value = 0;
            arr[8] = new SqlParameter("@ItemId", DDItemName.SelectedValue);
            arr[9] = new SqlParameter("@QualityId", DDQuality.SelectedValue);
            arr[10] = new SqlParameter("@ShadeColorId", DDShadeColor.SelectedValue);
            arr[11] = new SqlParameter("@LotNo", DDLotNo.SelectedItem.Text);
            arr[12] = new SqlParameter("@godownid", DDGodownName.SelectedValue);
            arr[13] = new SqlParameter("@IssRecQty", txtIssueQty.Text == "" ? "0" : txtIssueQty.Text);
            arr[14] = new SqlParameter("@QualityTypeId", DDQualityType.SelectedValue);
            int varfinishedid = UtilityModule.getItemFinishedIdForDyer(Convert.ToInt32(DDItemName.SelectedValue), Convert.ToInt32(DDQuality.SelectedValue), 0, 0, 0, 0, "", Tran, Convert.ToInt32(DDShadeColor.SelectedValue), "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            arr[15] = new SqlParameter("@finishedid", varfinishedid);
            arr[16] = new SqlParameter("@Rate", lblRate.Text==""? "0" : lblRate.Text);
            arr[17] = new SqlParameter("@unitid", lblUnitId.Text);           
            arr[18] = new SqlParameter("@userid", Session["varuserid"]);
            arr[19] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[19].Direction = ParameterDirection.Output;
            arr[20] = new SqlParameter("@ProcessId", DDJobType.SelectedValue);
            arr[21] = new SqlParameter("@PendingIssRecQty", txtIssueQty.Text == "" ? "0" : txtIssueQty.Text);            
            
            //**************************************************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveFinisherRawIssueReceive", arr);
            hnid.Value = arr[0].Value.ToString();
            txtChallanNo.Text = arr[5].Value.ToString();
            //txtGatePassNo.Text = arr[24].Value.ToString();
            LblErrorMessage.Text = arr[19].Value.ToString();
            Tran.Commit();            

            DDShadeColor.SelectedIndex = -1;
            txtIssueQty.Text = "";
            //lblRate.Text = "0";
            FillstockQty(varfinishedid);           
            Fillgrid();            
           
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }    
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        Report();        
    }
    private void Report()
    {
        DataSet ds = new DataSet();      
        SqlParameter[] array = new SqlParameter[4];
        array[0] = new SqlParameter("@ChallanNo", txtChallanNo.Text);
        array[1] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
        array[2] = new SqlParameter("@msg", SqlDbType.VarChar, 500);
        array[2].Direction = ParameterDirection.Output;       

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetFinisherMaterialIssueReceiveReportData", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptFinisherMaterialDirectIssueReceiveReport.rpt";

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptFinisherMaterialDirectIssueReceiveReport.xsd";
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
    decimal TotalQty = 0;
    decimal TotalAmt = 0;
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtReqQty = (TextBox)e.Row.FindControl("txtIssRecQty");
            Label lblhnIssRecQty = (Label)e.Row.FindControl("lblhnIssRecQty");
            TotalQty += Convert.ToDecimal(lblhnIssRecQty.Text);            

            //e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            //e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DG, "Select$" + e.Row.RowIndex);

            
            if (ChKForEdit.Checked == true)
            {
                Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                if (DG.EditIndex >= 0)
                {
                    //btnEdit.Visible = false;
                }
                else
                {
                    btnEdit.Visible = true;                    
                }
            }
            else
            {
                for (int i = 0; i < DG.Columns.Count; i++)
                {
                    if (DG.Columns[i].HeaderText == "Edit")
                    {
                        DG.Columns[i].Visible = false;
                    }
                }
                Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                btnEdit.Visible = false;                
            }
                
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblGrandTQty = (Label)e.Row.FindControl("lblGrandTQty");
            lblGrandTQty.Text = TotalQty.ToString();
            //Label lblGrandGTotal = (Label)e.Row.FindControl("lblGrandGTotal");
            //lblGrandGTotal.Text = TotalAmt.ToString();
        }
    }
    protected void DG_RowEditing(object sender, GridViewEditEventArgs e)
    {        
        DG.EditIndex = e.NewEditIndex;
        Fillgrid();
    }
    protected void DG_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DG.EditIndex = -1;
        Fillgrid();
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
    protected void DG_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        btnclickflag = "";
        btnclickflag = "btnUpdate";
        Popup(true);
        txtpwd.Focus();
        rowindex = e.RowIndex;

    }
    protected void Updatedetails(int rowindex)
    {
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarTranDetailId = Convert.ToInt32(DG.DataKeys[rowindex].Value);
            TextBox txtIssRecQty = (TextBox)DG.Rows[rowindex].FindControl("txtIssRecQty");
            Label lblTranid = (Label)DG.Rows[rowindex].FindControl("lblTranid");
            Label lblLotNo = (Label)DG.Rows[rowindex].FindControl("lblLotNo");
            Label lblGodownName = (Label)DG.Rows[rowindex].FindControl("lblGodown");
            Label lblFinishedId = (Label)DG.Rows[rowindex].FindControl("lblFinishedId");
            Label lblTranType = (Label)DG.Rows[rowindex].FindControl("lblTranType");
            Label lblGodownId = (Label)DG.Rows[rowindex].FindControl("lblGodownId");
            Label lblQualityTypeId = (Label)DG.Rows[rowindex].FindControl("lblQualityTypeId");
            Label lblUnitId = (Label)DG.Rows[rowindex].FindControl("lblUnitId");
            Label lblhnIssRecQty = (Label)DG.Rows[rowindex].FindControl("lblhnIssRecQty");            
            

            SqlParameter[] arr = new SqlParameter[19];
            arr[0] = new SqlParameter("@TranID", lblTranid.Text);
            arr[1] = new SqlParameter("@companyId", DDCompanyName.SelectedValue);
            arr[2] = new SqlParameter("@empid", DDFinisherName.SelectedValue);
            arr[3] = new SqlParameter("@TranType", lblTranType.Text);
            arr[4] = new SqlParameter("@TranDate", TxtAssignDate.Text);
            arr[5] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            arr[6] = new SqlParameter("@TranDetailId", VarTranDetailId);
            if (ChkForDate.Checked == true)
            {
                arr[7] = new SqlParameter("@LotNo", "");
                arr[8] = new SqlParameter("@finishedid", 0);
            }
            else
            {
                arr[7] = new SqlParameter("@LotNo", lblLotNo.Text);
                //int varfinishedid = UtilityModule.getItemFinishedIdForDyer(Convert.ToInt32(DDItemName.SelectedValue), Convert.ToInt32(DDQuality.SelectedValue), 0, 0, 0, 0, "", Tran, Convert.ToInt32(DDShadeColor.SelectedValue), "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                arr[8] = new SqlParameter("@finishedid", lblFinishedId.Text);
            }
            arr[9] = new SqlParameter("@godownid", lblGodownId.Text);
            arr[10] = new SqlParameter("@IssRecQty", txtIssRecQty.Text == "" ? "0" : txtIssRecQty.Text);
            arr[11] = new SqlParameter("@QualityTypeId", lblQualityTypeId.Text);
            arr[12] = new SqlParameter("@Rate", "0");
            arr[13] = new SqlParameter("@unitid", lblUnitId.Text);
            arr[14] = new SqlParameter("@userid", Session["varuserid"]);
            arr[15] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[15].Direction = ParameterDirection.Output;
            arr[16] = new SqlParameter("@DateUpdate", ChkForDate.Checked == true ? 1 : 0);
            arr[17] = new SqlParameter("@OldIssRecQty", lblhnIssRecQty.Text);
            //**************************************************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateFinisherRawIssueReceive", arr);
            //hnid.Value = arr[0].Value.ToString();
            //txtChallanNo.Text = arr[5].Value.ToString();
            //txtGatePassNo.Text = arr[24].Value.ToString();
            LblErrorMessage.Text = arr[15].Value.ToString();
            Tran.Commit();
            DG.EditIndex = -1;
            Fillgrid();
            
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void CancelOrder()
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
            SqlParameter[] arr = new SqlParameter[6];
            arr[0] = new SqlParameter("@ChallanNo", txtChallanNo.Text);
            arr[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[1].Direction = ParameterDirection.Output;
            arr[3] = new SqlParameter("@TranType", DDTranType.SelectedValue);
            arr[4] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
            arr[5] = new SqlParameter("@UserID", Session["varuserid"]);

            //***********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_CancelFinisherRawIssueReceive", arr);
            LblErrorMessage.Text = arr[1].Value.ToString();
            Tran.Commit();
            Fillgrid();
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            Tran.Rollback();
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
        if (MySession.ProductionEditPwd == txtpwd.Text)
        {
            //Updatedetails(rowindex);
            //Popup(false);

            if (btnclickflag == "btnUpdate")
            {
                Updatedetails(rowindex);
            }
            //else if (btnclickflag == "BtnDeleteRow")
            //{
            //    DeleteRow(VarTranDetailID);
            //}
            else if (btnclickflag == "btnCancelOrder")
            {
                CancelOrder();
            }
            Popup(false);
        }
        else
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Please Enter Correct Password..";
        }
        //DG.EditIndex = -1;
        //Fillgrid();
    }
    protected void DG_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
            Label lblTrandetailid = (Label)DG.Rows[e.RowIndex].FindControl("lblTrandetailid");
            Label lblTranid = (Label)DG.Rows[e.RowIndex].FindControl("lblTranid");
            Label lblTranType = (Label)DG.Rows[e.RowIndex].FindControl("lblTranType");
            SqlParameter[] arr = new SqlParameter[6];
            arr[0] = new SqlParameter("@TranDetailid", lblTrandetailid.Text);
            arr[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[1].Direction = ParameterDirection.Output;
            arr[2] = new SqlParameter("@TranID", lblTranid.Text);
            arr[3] = new SqlParameter("@TranType", lblTranType.Text);
            arr[4] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
            arr[5] = new SqlParameter("@UserID", Session["varuserid"]);

            //***********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteFinisherRawIssueReceive", arr);
            LblErrorMessage.Text = arr[1].Value.ToString();
            Tran.Commit();
            Fillgrid();
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            Tran.Rollback();           
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void DDLotNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Varfinishedid = UtilityModule.getItemFinishedIdForDyer(Convert.ToInt32(DDItemName.SelectedValue), Convert.ToInt32(DDQuality.SelectedValue), 0, 0, 0, 0, "", Convert.ToInt32(DDShadeColor.SelectedValue), 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        FillstockQty(Varfinishedid);
    }
    protected void btncancelorder_Click(object sender, EventArgs e)
    {
        btnclickflag = "";
        btnclickflag = "btnCancelOrder";
        txtpwd.Focus();
        Popup(true);
    }
}