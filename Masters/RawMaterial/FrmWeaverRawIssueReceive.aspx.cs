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

public partial class Masters_RawMaterial_FrmWeaverRawIssueReceive : System.Web.UI.Page
{
    public int UnitId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName
                           select GoDownID,GodownName from GodownMaster where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by GodownName";

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
                DDGodownName.SelectedIndex = 1;
            }

            TxtAssignDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            BindQualityType();
        }
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
        UtilityModule.ConditionalComboFill(ref DDWeaverName, "select EI.EmpId,EI.EmpName from EmpInfo EI INNER JOIN EmpProcess EP ON EI.EmpId=EP.EmpId Where EP.ProcessId=1 Order by EI.EmpName", true, "--Plz Select--");
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
    protected void DDWeaverName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDWeaverName.SelectedIndex > 0)
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
            FillWeaverRawSubItemRate();
        }
    }
    private void FillWeaverRawSubItemRate()
    {
        if (DDTranType.SelectedValue == "0")
        {
            string str = @"select WR.Rate from WeaverRawSubItemRate WR Where WR.ItemId=" + DDItemName.SelectedValue + " and WR.QualityId=" + DDQuality.SelectedValue + @" 
                and WR.EffectiveDate<='" + TxtAssignDate.Text + "' and (WR.TODate>'" + TxtAssignDate.Text + "' or TODate is null)";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblRate.Text = ds.Tables[0].Rows[0]["Rate"].ToString();
            }
            else
            {
                lblRate.Text = "0";
            }
        }
        else
        {
            lblRate.Text = "0";
        }
    }
    //protected void DDReceiveColor_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (DDReceiveColor.SelectedIndex > 0)
    //    {
    //        FillDyerColorRate();
    //    }
    //    else
    //    {
    //        txtRate.Text = "";
    //    }
    //}
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
            arr[2] = new SqlParameter("@empid", DDWeaverName.SelectedValue);
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
            arr[16] = new SqlParameter("@Rate", lblRate.Text == "" ? "0" : lblRate.Text);
            arr[17] = new SqlParameter("@unitid", lblUnitId.Text);
            arr[18] = new SqlParameter("@userid", Session["varuserid"]);
            arr[19] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[19].Direction = ParameterDirection.Output;

            //**************************************************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveWeaverRawIssueReceive", arr);
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
    protected void Fillgrid()
    {
        string str = @"select WRM.TranId,WRT.TranDetailId,VF.ITEM_NAME as ItemName,VF.QualityName,VF.ShadeColorName,IM.ITEM_NAME as QualityType,FORMAT(WRT.IssRecQty,'#0') as IssRecQty,isnull(WRT.Rate,0) as Rate,
                        WRT.LotNo,GM.GodownName,WRT.Finishedid,WRM.ChallanNo,WRM.CompanyId,replace(convert(varchar(11),WRM.TranDate,106), ' ','-') as TranDate,WRM.TranType
                        From WeaverRawTranMaster WRM inner join WeaverRawTranDetail WRT on WRM.TranId=WRT.TranId 
                        INNER JOIN V_FinishedItemDetail VF ON WRT.FinishedId=VF.ITEM_FINISHED_ID
                        INNER JOIN Item_Master IM ON WRT.QualityTypeId=IM.ITEM_ID
                        INNER JOIN GodownMaster GM ON WRT.godownId=GM.GoDownID Where WRM.TranId=" + hnid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        Report();
    }
    private void Report()
    {
        DataSet ds = new DataSet();
        // string qry = "";
        // string str = "";
        SqlParameter[] array = new SqlParameter[4];
        array[0] = new SqlParameter("@ChallanNo", txtChallanNo.Text);
        array[1] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
        array[2] = new SqlParameter("@msg", SqlDbType.VarChar, 500);
        array[2].Direction = ParameterDirection.Output;
        //array[3] = new SqlParameter("@ReportType", SqlDbType.Int);
        //array[3].Value = 0;

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetWeaverMaterialIssueReceiveReportData", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptWeaverMaterialDirectIssueReceiveReport.rpt";

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptWeaverMaterialDirectIssueReceiveReport.xsd";
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
            Label lblIssRecqty = (Label)e.Row.FindControl("lblIssRecqty");
            TotalQty += Convert.ToDecimal(lblIssRecqty.Text);

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
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteWeaverRawIssueReceive", arr);
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
}