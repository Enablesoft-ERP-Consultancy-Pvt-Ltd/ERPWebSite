using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Order_FrmSamplingToDo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["SrNo"] = 0;
            TxtDispDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            Fill_Grid();
            TxtBuyerCode.Focus();
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[21];
            _arrpara[0] = new SqlParameter("@SrNo", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@PriorityLevel", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@BuyerCode", SqlDbType.NVarChar, 50);
            _arrpara[3] = new SqlParameter("@DNo", SqlDbType.NVarChar, 150);
            _arrpara[4] = new SqlParameter("@DName", SqlDbType.NVarChar, 150);
            _arrpara[5] = new SqlParameter("@Technique", SqlDbType.NVarChar, 150);
            _arrpara[6] = new SqlParameter("@Size", SqlDbType.NVarChar, 150);
            _arrpara[7] = new SqlParameter("@RawMaterialComposition", SqlDbType.NVarChar, 150);
            _arrpara[8] = new SqlParameter("@QualityOfRaw", SqlDbType.NVarChar, 150);
            _arrpara[9] = new SqlParameter("@WashUnWash", SqlDbType.NVarChar, 150);
            _arrpara[10] = new SqlParameter("@PileHeight", SqlDbType.NVarChar, 150);
            _arrpara[11] = new SqlParameter("@PlWt", SqlDbType.NVarChar, 150);
            _arrpara[12] = new SqlParameter("@TWt", SqlDbType.NVarChar, 150);
            _arrpara[13] = new SqlParameter("@DispDate", SqlDbType.SmallDateTime);
            _arrpara[14] = new SqlParameter("@StatusOfDispatch", SqlDbType.NVarChar, 150);
            _arrpara[15] = new SqlParameter("@CourierNo", SqlDbType.NVarChar, 150);
            _arrpara[16] = new SqlParameter("@TrackingNo", SqlDbType.NVarChar, 150);
            _arrpara[17] = new SqlParameter("@ColorNoS", SqlDbType.NVarChar, 150);
            _arrpara[18] = new SqlParameter("@Remark", SqlDbType.NVarChar);
            _arrpara[19] = new SqlParameter("@VarUserId", SqlDbType.Int);
            _arrpara[20] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);

            _arrpara[0].Value = ViewState["SrNo"];
            _arrpara[1].Value = DDPriorityLevel.SelectedValue;
            _arrpara[2].Value = TxtBuyerCode.Text.ToUpper();
            _arrpara[3].Value = TxtDNo.Text.ToUpper();
            _arrpara[4].Value = TxtDName.Text.ToUpper();
            _arrpara[5].Value = TxtTechnique.Text.ToUpper();
            _arrpara[6].Value = TxtSize.Text.ToUpper();
            _arrpara[7].Value = TxtRawMaterialComposition.Text.ToUpper();
            _arrpara[8].Value = TxtQualityOfRaw.Text.ToUpper();
            _arrpara[9].Value = TxtWashUnWash.Text.ToUpper();
            _arrpara[10].Value = TxtPileHeight.Text.ToUpper();
            _arrpara[11].Value = TxtPlWt.Text.ToUpper();
            _arrpara[12].Value = TxtTWt.Text.ToUpper();
            _arrpara[13].Value = TxtDispDate.Text.ToUpper();
            _arrpara[14].Value = TxtStatusOfDispatch.Text.ToUpper();
            _arrpara[15].Value = TxtCourierNo.Text.ToUpper();
            _arrpara[16].Value = TxtTrackingNo.Text.ToUpper();
            _arrpara[17].Value = TxtColorNoS.Text.ToUpper();
            _arrpara[18].Value = TxtRemark.Text.ToUpper();
            _arrpara[19].Value = Session["varuserid"];
            _arrpara[20].Value = Session["VarcompanyNo"];

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SamplingToDo", _arrpara);
            Tran.Commit();
            ReferenceAfterSave();
            Fill_Grid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + ex.Message + "...');", true);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void ReferenceAfterSave()
    {
        TxtBuyerCode.Focus();
        TxtBuyerCode.Text = "";
        TxtBuyerCode.Text = "";
        TxtDNo.Text = "";
        TxtDName.Text = "";
        TxtTechnique.Text = "";
        TxtSize.Text = "";
        TxtRawMaterialComposition.Text = "";
        TxtQualityOfRaw.Text = "";
        TxtWashUnWash.Text = "";
        TxtPileHeight.Text = "";
        TxtPlWt.Text = "";
        TxtTWt.Text = "";
        TxtStatusOfDispatch.Text = "";
        TxtCourierNo.Text = "";
        TxtTrackingNo.Text = "";
        TxtColorNoS.Text = "";
        TxtRemark.Text = "";
        ViewState["SrNo"] = 0;
    }
    private void Fill_Grid()
    {
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select SrNo,Case When PriorityLevel=0 Then 'Normal' Else Case When PriorityLevel=1 Then 'Urgent' Else 'Top Urgent' End End PriorityLevel,BuyerCode,DNo,DName,Technique,Size,
        RawMaterialComposition,QualityOfRaw,WashUnWash,PileHeight,PlWt,TWt,replace(convert(varchar(11),DispDate,106), ' ','-') DispDate,StatusOfDispatch,CourierNo,TrackingNo,ColorNoS,Remark,UserId,MasterCompanyID 
        From SamplingToDo Where MasterCompanyID=" + Session["VarcompanyNo"] + "  Order By BuyerCode");
        DGToDoSampling.DataSource = Ds.Tables[0];
        DGToDoSampling.DataBind();
    }
    //protected void DGToDoSampling_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //Add CSS class on header row.
    //    if (e.Row.RowType == DataControlRowType.Header)
    //        e.Row.CssClass = "header";
    //    //Add CSS class on normal row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Normal)
    //        e.Row.CssClass = "normal";
    //    //Add CSS class on alternate row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Alternate)
    //        e.Row.CssClass = "alternate";
    //}
    protected void DGToDoSampling_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGToDoSampling, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGToDoSampling_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Delete SamplingToDo Where SrNo=" + DGToDoSampling.DataKeys[e.RowIndex].Value);
            Fill_Grid();
            ReferenceAfterSave();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + ex.Message + "...');", true);
        }
    }
    protected void DGToDoSampling_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["SrNo"] = DGToDoSampling.SelectedDataKey.Value;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select SrNo,PriorityLevel,BuyerCode,DNo,DName,Technique,Size,RawMaterialComposition,
        QualityOfRaw,WashUnWash,PileHeight,PlWt,TWt,replace(convert(varchar(11),DispDate,106), ' ','-') DispDate,StatusOfDispatch,CourierNo,TrackingNo,ColorNoS,Remark 
        From SamplingToDo Where SrNo=" + ViewState["SrNo"]);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            DDPriorityLevel.SelectedValue = Ds.Tables[0].Rows[0]["PriorityLevel"].ToString();
            TxtBuyerCode.Text = Ds.Tables[0].Rows[0]["BuyerCode"].ToString();
            TxtDNo.Text = Ds.Tables[0].Rows[0]["DNo"].ToString();
            TxtDName.Text = Ds.Tables[0].Rows[0]["DName"].ToString();
            TxtTechnique.Text = Ds.Tables[0].Rows[0]["Technique"].ToString();
            TxtSize.Text = Ds.Tables[0].Rows[0]["Size"].ToString();
            TxtRawMaterialComposition.Text = Ds.Tables[0].Rows[0]["RawMaterialComposition"].ToString();
            TxtQualityOfRaw.Text = Ds.Tables[0].Rows[0]["QualityOfRaw"].ToString();
            TxtWashUnWash.Text = Ds.Tables[0].Rows[0]["WashUnWash"].ToString();
            TxtPileHeight.Text = Ds.Tables[0].Rows[0]["PileHeight"].ToString();
            TxtPlWt.Text = Ds.Tables[0].Rows[0]["PlWt"].ToString();
            TxtTWt.Text = Ds.Tables[0].Rows[0]["TWt"].ToString();
            TxtDispDate.Text = Ds.Tables[0].Rows[0]["DispDate"].ToString();
            TxtStatusOfDispatch.Text = Ds.Tables[0].Rows[0]["StatusOfDispatch"].ToString();
            TxtCourierNo.Text = Ds.Tables[0].Rows[0]["CourierNo"].ToString();
            TxtTrackingNo.Text = Ds.Tables[0].Rows[0]["TrackingNo"].ToString();
            TxtColorNoS.Text = Ds.Tables[0].Rows[0]["ColorNoS"].ToString();
            TxtRemark.Text = Ds.Tables[0].Rows[0]["Remark"].ToString();
        }
    }
}