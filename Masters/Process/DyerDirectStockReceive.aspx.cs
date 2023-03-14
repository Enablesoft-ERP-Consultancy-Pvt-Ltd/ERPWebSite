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

public partial class Masters_ProcessIssue_DyerDirectStockReceive : System.Web.UI.Page
{
    public int UnitId = 0;
    public string LastReceivedDate2="";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {         

            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                           select GoDownID,GodownName from GodownMaster where MasterCompanyid=" + Session["varcompanyid"] + @" order by GodownName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
                BindDyerName();
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDGodownName, ds, 1, true, "--Plz Select--");

            TxtAssignDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            if (Session["canedit"].ToString() == "1")
            {
                TREdit.Visible = true;
                TDComplete.Visible = false;
                txtChallanNo.Enabled = false;
            }
        }
    }
    private void BindDyerName()
    {
        UtilityModule.ConditionalComboFill(ref DDDyerName, "select EI.EmpId,EI.EmpName from EmpInfo EI INNER JOIN EmpProcess EP ON EI.EmpId=EP.EmpId Where EP.ProcessId=5 Order by EI.EmpName", true, "--Plz Select--");
    }
    private void BindItemName()
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "select ITEM_ID,ITEM_NAME from ITEM_MASTER where CATEGORY_ID=2 and MasterCompanyid=" + Session["varCompanyId"] + @" Order by Item_Name", true, "--Plz Select--");
    }
  
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDCompanyName.SelectedIndex > 0)
        {
            BindDyerName();
        }
    }
    protected void DDDyerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDDyerName.SelectedIndex > 0)
        {
            BindItemName();
            BindReceiveColor();            
        }

        if (chkedit.Checked == true)
        {
            string str = @"select ID,ChallanNo From DyerDirectStockReceiveMaster Where Companyid=" + DDCompanyName.SelectedValue + " and empid=" + DDDyerName.SelectedValue;
            if (chkcomplete.Checked == true)
            {
                str = str + " and status='Complete'";
            }
            else
            {
                str = str + " and status='Pending'";
            }
            str = str + "  order by id desc";

            UtilityModule.ConditionalComboFill(ref DDChallanNo, str, true, "--Plz Select--");
        }
    }
    private void BindQuality()
    {
        UtilityModule.ConditionalComboFill(ref DDQuality, "select QualityId,QualityName from Quality where Item_Id=" + DDItemName.SelectedValue + " and MasterCompanyid=" + Session["varCompanyId"] + @" Order by QualityName", true, "--Plz Select--");
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
    private void BindGivenColor()
    {
        string str = "";
        //str = @"select distinct SC.ShadecolorId,SC.ShadeColorName  from ITEM_PARAMETER_MASTER IPM 
               // INNER JOIN stock S ON IPM.ITEM_FINISHED_ID=S.ITEM_FINISHED_ID INNER JOIN ShadeColor SC ON IPM.SHADECOLOR_ID=SC.ShadecolorId 
               // Where IPM.ITEM_ID=" + DDItemName.SelectedValue + " and IPM.QUALITY_ID=" + DDQuality.SelectedValue + " and S.Qtyinhand>0  and S.Godownid=" + DDGodownName.SelectedValue + "";

        str = @"select distinct SC.ShadecolorId,SC.ShadeColorName  from ITEM_PARAMETER_MASTER IPM 
                INNER JOIN stock S ON IPM.ITEM_FINISHED_ID=S.ITEM_FINISHED_ID INNER JOIN ShadeColor SC ON IPM.SHADECOLOR_ID=SC.ShadecolorId 
                Where IPM.ITEM_ID=" + DDItemName.SelectedValue + " and IPM.QUALITY_ID=" + DDQuality.SelectedValue + "";

        UtilityModule.ConditionalComboFill(ref DDGivenColor, str, true, "--SELECT--");
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDQuality.SelectedIndex > 0)
        {
            BindGivenColor();
            UtilityModule.ConditionalComboFill(ref DDLotNo, "", true, "--Plz Select--");
            txtQtyInHand.Text = "";
        }
    }

    protected void DDGivenColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDGivenColor.SelectedIndex > 0)
        {
            int Varfinishedid = UtilityModule.getItemFinishedIdForDyer(Convert.ToInt32(DDItemName.SelectedValue), Convert.ToInt32(DDQuality.SelectedValue), 0, 0, 0, 0, "", Convert.ToInt32(DDGivenColor.SelectedValue), 0, "", Convert.ToInt32(Session["varCompanyId"]));
            FillLotno(Varfinishedid);
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDLotNo, "", true, "--Plz Select--");
            txtQtyInHand.Text = "";
        }
    }
    protected void FillLotno(int varfinishedid)
    {
        string str = "";
        if (MySession.Stockapply == "True")
        {
            //str = "select Distinct LotNo,LotNo as LotNo1 From Stock Where ITEM_FINISHED_ID=" + varfinishedid + " and Companyid=" + DDCompanyName.SelectedValue + " and Godownid=" + DDGodownName.SelectedValue + " And Round(Qtyinhand,3)>0 order by LotNo1";
            str = "select Distinct LotNo,LotNo as LotNo1 From DyerStockTran Where ITEM_FINISHED_ID=" + varfinishedid + " and Companyid=" + DDCompanyName.SelectedValue + " and EmpId=" + DDDyerName.SelectedValue + " order by LotNo1";
        }
        else
        {
            //str = "select Distinct LotNo,LotNo as LotNo1 From Stock Where ITEM_FINISHED_ID=" + varfinishedid + " and Companyid=" + DDCompanyName.SelectedValue + " and Godownid=" + DDGodownName.SelectedValue + "  order by LotNo1";
            str = "select Distinct LotNo,LotNo as LotNo1 From DyerStockTran Where ITEM_FINISHED_ID=" + varfinishedid + " and Companyid=" + DDCompanyName.SelectedValue + "  order by LotNo1";
        }
        UtilityModule.ConditionalComboFill(ref DDLotNo, str, true, "--Plz Select--");
        if (DDLotNo.Items.Count > 0)
        {
            DDLotNo.SelectedIndex = 1;
            DDLotNo_SelectedIndexChanged(DDLotNo, new EventArgs());
        }
        else
        {
            txtQtyInHand.Text = "";
        }
    }
    
    protected void DDLotNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Varfinishedid = UtilityModule.getItemFinishedIdForDyer(Convert.ToInt32(DDItemName.SelectedValue), Convert.ToInt32(DDQuality.SelectedValue), 0, 0, 0, 0, "", Convert.ToInt32(DDGivenColor.SelectedValue), 0, "", Convert.ToInt32(Session["varCompanyId"]));
        FillstockQty(Varfinishedid);

    }   
    protected void FillstockQty(int varfinishedid)
    {
        string Lotno, TagNo = "";
        Lotno = DDLotNo.SelectedItem.Text;
        TagNo = "Without Tag No";

        txtQtyInHand.Text = Convert.ToString(UtilityModule.getDyerstockQty(DDCompanyName.SelectedValue, DDDyerName.SelectedValue, Lotno, varfinishedid, TagNo));
    }
    private void BindReceiveColor()
    {
        string str = "";
        str = @"select ShadecolorId,ShadeColorName from ShadeColor order by ShadeColorName";

        // str = @"select SC.ShadecolorId,SC.ShadeColorName from DyerColorRate DCR INNER JOIN ShadeColor SC ON DCR.DyerShadeColorId=SC.ShadecolorId 
        //  where DyerId=" + DDDyerName.SelectedValue + " and DCR.EffectiveDate<='" + TxtAssignDate.Text + "' and (DCR.TODate>'" + TxtAssignDate.Text + "' or TODate is null)";

        UtilityModule.ConditionalComboFill(ref DDReceiveColor, str, true, "--SELECT--");
    }
    private void LastReceivedDate()
    {
        string str = @"select Top 1 Date from DyerDirectStockReceiveMaster order by Date desc";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            LastReceivedDate2 = ds.Tables[0].Rows[0]["Date"].ToString();
        }
        else
        {
            LastReceivedDate2 = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    
    protected void btnsave_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";

        if (chkedit.Checked == false)
        {
            if (LastReceivedDate2 == "")
            {
                LastReceivedDate();
            }          

            DateTime dtVal = DateTime.Parse(TxtAssignDate.Text);
            DateTime LastDate = DateTime.Parse(LastReceivedDate2);
            if (dtVal < LastDate)
            {
                LblErrorMessage.Text = "Receive Date Must be Greater Then Last Receive Date!";
                return;
            }
            else
            {
                LblErrorMessage.Text = "";
            }
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[20];
            arr[0] = new SqlParameter("@ID", SqlDbType.Int);
            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = hnid.Value;
            arr[1] = new SqlParameter("@companyId", DDCompanyName.SelectedValue);
            arr[2] = new SqlParameter("@Processid", 5);
            arr[3] = new SqlParameter("@empid", DDDyerName.SelectedValue);
            arr[4] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 50);
            arr[4].Direction = ParameterDirection.InputOutput;
            arr[4].Value = txtChallanNo.Text;
            arr[5] = new SqlParameter("@Date", TxtAssignDate.Text);            
            arr[6] = new SqlParameter("@Mastercompanyid", Session["varcompanyId"]);
            arr[7] = new SqlParameter("@DetailId", SqlDbType.Int);
            arr[7].Value = 0;
            int varfinishedid = UtilityModule.getItemFinishedIdForDyer(Convert.ToInt32(DDItemName.SelectedValue), Convert.ToInt32(DDQuality.SelectedValue), 0, 0, 0, 0, "", Tran, Convert.ToInt32(DDGivenColor.SelectedValue), "", Convert.ToInt32(Session["varCompanyId"]));

            arr[8] = new SqlParameter("@Ifinishedid", varfinishedid);
            arr[9] = new SqlParameter("@flagsize", 0);
            arr[10] = new SqlParameter("@unitid", lblUnitId.Text);
            arr[11] = new SqlParameter("@godownid", DDGodownName.SelectedValue);
            arr[12] = new SqlParameter("@LotNo", DDLotNo.SelectedItem.Text);
            arr[13] = new SqlParameter("@TagNo", "Without Tag No");
            arr[14] = new SqlParameter("@Caltype", 0);
            arr[15] = new SqlParameter("@IssueReceiveType", SqlDbType.Int);
            arr[15].Value = 1;  //*0 For Issue 1 For Receive*//          
            arr[16] = new SqlParameter("@RecQty", txtIssueQty.Text == "" ? "0" : txtIssueQty.Text);           
            arr[17] = new SqlParameter("@userid", Session["varuserid"]);
            arr[18] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[18].Direction = ParameterDirection.Output;
            int varRfinishedid = UtilityModule.getItemFinishedIdForDyer(Convert.ToInt32(DDItemName.SelectedValue), Convert.ToInt32(DDQuality.SelectedValue), 0, 0, 0, 0, "", Tran, Convert.ToInt32(DDReceiveColor.SelectedValue), "", Convert.ToInt32(Session["varCompanyId"]));
            arr[19] = new SqlParameter("@Rfinishedid", varRfinishedid);           
           
            //**************************************************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveDyerDirectReceiveMaster", arr);
            hnid.Value = arr[0].Value.ToString();
            txtChallanNo.Text = arr[4].Value.ToString();
         
            LblErrorMessage.Text = arr[18].Value.ToString();           
            Tran.Commit();
          
            txtIssueQty.Text = "";
           // txtRate.Text = "";
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
    protected void Fillgrid()
    {
        string str = @"select Sm.ID,Sd.Detailid,dbo.F_getItemDescription(Sd.Rfinishedid,sd.flagsize) as RItemdescription,SD.LotNo,SD.TagNo,
                       FORMAT(SD.RecQty,'#0.00') as RecQty,SM.ChallanNo,replace(convert(varchar(11),Sm.Date,106), ' ','-') as Date,GM.GodownName
                       From DyerDirectStockReceiveMaster Sm inner join DyerDirectStockReceiveDetail SD on Sm.ID=SD.masterid 
                       INNER JOIN GodownMaster GM ON SD.godownId=GM.GoDownID Where Sm.id=" + hnid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (chkedit.Checked == true)
            {
                txtChallanNo.Text = ds.Tables[0].Rows[0]["ChallanNo"].ToString();                
                TxtAssignDate.Text = ds.Tables[0].Rows[0]["Date"].ToString();                
            }
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
        array[0] = new SqlParameter("@Id", hnid.Value);
        array[1] = new SqlParameter("@MasterCompanyId", Session["varcompanyId"]);      
        array[2] = new SqlParameter("@msg", SqlDbType.VarChar, 500);
        array[2].Direction = ParameterDirection.Output;
       // array[3] = new SqlParameter("@ReportType", SqlDbType.Int);
        //array[3].Value = 0;

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetDyerDirectReceiveReportData", array);
               
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptDyerDirectReceiveReport.rpt";

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptDyerDirectReceiveReport.xsd";
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
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        chkcomplete.Checked = false;
        TDChallanNo.Visible = false;
        TDChallanNoDD.Visible = false;
        hnid.Value = "0";
        txtChallanNo.Text = "";
       // txtGatePassNo.Text = "";
        TxtAssignDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        //txtRequiredDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        DDDyerName.SelectedIndex = -1;
        if (chkedit.Checked == true)
        {
            DDChallanNo.Items.Clear();
            TDChallanNo.Visible = true;
            TDChallanNoDD.Visible = true;
        }
        DG.DataSource = null;
        DG.DataBind();
    }

    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = DDChallanNo.SelectedValue;
        Fillgrid();
    }
    decimal TotalQty = 0;
    decimal TotalAmt = 0;
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblQty = (Label)e.Row.FindControl("lblqty");
            TotalQty += Convert.ToDecimal(lblQty.Text);
            //Label lblTotalAmt = (Label)e.Row.FindControl("lblAmount");
            //TotalAmt += Convert.ToDecimal(lblTotalAmt.Text);

            //e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            //e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DG, "Select$" + e.Row.RowIndex);
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
    protected void DG_RowUpdating(object sender, GridViewUpdateEventArgs e)
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
            Label lblid = (Label)DG.Rows[e.RowIndex].FindControl("lblid");
            Label lbldetailid = (Label)DG.Rows[e.RowIndex].FindControl("lbldetailid");
            TextBox txtqty = (TextBox)DG.Rows[e.RowIndex].FindControl("txtqty");
            //TextBox txtRate = (TextBox)DG.Rows[e.RowIndex].FindControl("txtRate");
            Label lblqty = (Label)DG.Rows[e.RowIndex].FindControl("lblqty");

            if (txtqty.Text == "" || txtqty.Text =="0")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('Please Enter Qty');", true);
                return;
            }

            //ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            //*************
            SqlParameter[] arr = new SqlParameter[8];
            arr[0] = new SqlParameter("@ID", lblid.Text);
            arr[1] = new SqlParameter("@Detailid", lbldetailid.Text);
            arr[2] = new SqlParameter("@RecQty", txtqty.Text == "" ? "0" : txtqty.Text);
            arr[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[3].Direction = ParameterDirection.Output;
            arr[4] = new SqlParameter("@userid", Session["varuserid"]);
            //arr[5] = new SqlParameter("@Rate", txtRate.Text == "" ? "0" : txtRate.Text);
            arr[6] = new SqlParameter("@hnQty", lblqty.Text == "" ? "0" : lblqty.Text);
            arr[7] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
           
           
            //*******
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateDyerDirectReceive", arr);
            LblErrorMessage.Text = arr[3].Value.ToString();
            Tran.Commit();
            DG.EditIndex = -1;
            Fillgrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
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
            Label lblDetailid = (Label)DG.Rows[e.RowIndex].FindControl("lblDetailid");
            Label lblid = (Label)DG.Rows[e.RowIndex].FindControl("lblid");
            SqlParameter[] arr = new SqlParameter[5];
            arr[0] = new SqlParameter("@Detailid", lblDetailid.Text);
            arr[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[1].Direction = ParameterDirection.Output;
            arr[2] = new SqlParameter("@ID", lblid.Text);
            arr[3] = new SqlParameter("@userid", Session["varuserid"]);
            arr[4] = new SqlParameter("@mastercompanyid", Session["varcompanyId"]);
  
            //***********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteDirectDyerReceive", arr);
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

    //protected void btngatepass_Click(object sender, EventArgs e)
    //{
    //    ReportGatePass();        
    //}
    //private void ReportGatePass()
    //{
    //    DataSet ds = new DataSet();
    //    // string qry = "";
    //    // string str = "";
    //    SqlParameter[] array = new SqlParameter[4];
    //    array[0] = new SqlParameter("@Id", hnid.Value);
    //    array[1] = new SqlParameter("@MasterCompanyId", Session["varcompanyId"]);       
    //    array[2] = new SqlParameter("@msg", SqlDbType.VarChar, 500);
    //    array[2].Direction = ParameterDirection.Output;
    //    array[3] = new SqlParameter("@ReportType", SqlDbType.Int);
    //    array[3].Value = 1;

    //    ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetDyerIssueReportData", array);

    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        Session["rptFileName"] = "~\\Reports\\RptDyerIssueGatePass.rpt";

    //        Session["GetDataset"] = ds;
    //        Session["dsFileName"] = "~\\ReportSchema\\RptDyerIssueReport.xsd";
    //        StringBuilder stb = new StringBuilder();
    //        stb.Append("<script>");
    //        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
    //        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
    //    }
    //}
    //protected void refreshDyerColor_Click(object sender, EventArgs e)
    //{
    //    BindReceiveColor();       
    //}
    ////protected void DDColorShade_SelectedIndexChanged(object sender, EventArgs e)
    ////{
    ////    DDgodown_SelectedIndexChanged(sender, new EventArgs());
    ////}
}