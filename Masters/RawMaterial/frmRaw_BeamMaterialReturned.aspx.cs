using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


public partial class Masters_RawMaterial_frmRawMaterialReturned : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

    }
    protected void Fill_Grid()
    {
        try
        {
            DGDetail.DataSource = null;
            DGDetail.DataBind();
            if (txtFolioNo.Text != "")
            {
                string str = @"Select PM.CompanyId,CATEGORY_NAME,ITEM_NAME,QualityName+ Space(2)+DesignName+ Space(2)+ColorName+ Space(2)+ShapeName+ Space(2)+SizeMtr+ Space(2)+ShadeColorName DESCRIPTION,
                            Sum(case When Trantype=0 Then IssueQuantity Else 0 End) As IssuedQty,Sum(case When TranType=1 Then IssueQuantity Else 0 End) as ReceivedQty,LotNo,Godownid,Prorderid As IssueOrderId,PT.Finishedid,PT.UnitId,PT.CategoryId,PT.TagNo,PM.Beamtype,PT.BeamNo,PT.Itemid,PT.flagsize
                            From ProcessRawMaster PM,ProcessRawTran PT,V_FinishedItemDetail VF 
                            Where PM.TypeFlag = 0 And PM.PrmId=PT.PrmId and  PT.Finishedid=VF.Item_Finished_id  And PM.PrOrderid='" + txtFolioNo.Text + @"' and Pm.processid=1 And PM.CompanyID = " + Session["CurrentWorkingCompanyID"] + @" 
                            Group by PM.CompanyId,Category_name,Item_name,QualityName,DesignName,Colorname,ShapeName,Sizemtr,ShadeColorname,lotno,godownid,prorderid,PT.Finishedid,PT.UnitId,PT.CategoryId,PT.TagNo,PM.Beamtype,PT.BeamNo,PT.Itemid,PT.flagsize";
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DGDetail.DataSource = ds;
                    DGDetail.DataBind();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Information", "alert('No record found......');", true);
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
    protected void txtFolioNo_TextChanged(object sender, EventArgs e)
    {
        //Fill Detail in Grid
        ViewState["Prmid"] = 0;
        ViewState["Beamtype"] = 0;
        Fill_Grid();
    }
    protected void DGDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = "";
        if (e.CommandName == "Save")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {

                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int row = gvr.RowIndex;

                Label lbllotNo = ((Label)DGDetail.Rows[row].FindControl("lblLotNo"));
                Label lblGodownid = ((Label)DGDetail.Rows[row].FindControl("lblGodownId"));
                Label lblUnitId = ((Label)DGDetail.Rows[row].FindControl("lblUnitId"));
                Label lblIssueOrderId = ((Label)DGDetail.Rows[row].FindControl("lblIssueOrderId"));
                Label lblFinishedId = ((Label)DGDetail.Rows[row].FindControl("lblFinishedId"));
                Label lblCategoryId = ((Label)DGDetail.Rows[row].FindControl("lblCategoryId"));
                Label lblCompanyId = ((Label)DGDetail.Rows[row].FindControl("lblCompanyId"));
                Label lblIssuedQty = ((Label)DGDetail.Rows[row].FindControl("lblIssuedQty"));
                Label lblTagNo = ((Label)DGDetail.Rows[row].FindControl("lblTagNo"));
                Label lblBeamtype = ((Label)DGDetail.Rows[row].FindControl("lblbeamtype"));
                Label lblBeamNo = ((Label)DGDetail.Rows[row].FindControl("lblBeamNo"));
                Label lblitemid = ((Label)DGDetail.Rows[row].FindControl("lblitemid"));
                Label lblflagsize = ((Label)DGDetail.Rows[row].FindControl("lblflagsize"));
                TextBox txtreturnQty = ((TextBox)DGDetail.Rows[row].FindControl("txtreturnQty"));

                //Check Condition
                if (txtreturnQty.Text == "" || txtreturnQty.Text == "0")
                {
                    lblMessage.Text = "Plz Enter Return Qty......";
                    Tran.Commit();
                    return;
                }
                if ((Convert.ToDouble(lblIssuedQty.Text) < Convert.ToDouble(txtreturnQty.Text)))
                {
                    lblMessage.Text = "Return Qty Can not be greater than Issued Qty..";
                    Tran.Commit();
                    return;
                }

                SqlParameter[] arr = new SqlParameter[28];
                arr[0] = new SqlParameter("@PrmID", SqlDbType.Int);
                arr[1] = new SqlParameter("@CompanyId", SqlDbType.Int);
                arr[2] = new SqlParameter("@EmpId", SqlDbType.Int);
                arr[3] = new SqlParameter("@ProcessId", SqlDbType.Int);
                arr[4] = new SqlParameter("@OrderId", SqlDbType.Int);
                arr[5] = new SqlParameter("@IssueDate", SqlDbType.SmallDateTime);
                arr[6] = new SqlParameter("@ChalanNo", SqlDbType.NVarChar, 50);
                arr[7] = new SqlParameter("@TranType", SqlDbType.Int);
                arr[8] = new SqlParameter("@userid", SqlDbType.Int);
                arr[9] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
                arr[10] = new SqlParameter("@Prtid", SqlDbType.Int);
                arr[11] = new SqlParameter("@CategoryId", SqlDbType.Int);
                arr[12] = new SqlParameter("@Itemid", SqlDbType.Int);
                arr[13] = new SqlParameter("@FinishedId", SqlDbType.Int);
                arr[14] = new SqlParameter("@GodownId", SqlDbType.Int);
                arr[15] = new SqlParameter("@IssueQuantity", SqlDbType.Float);
                arr[16] = new SqlParameter("@lotNo", SqlDbType.NVarChar, 50);
                arr[17] = new SqlParameter("@UnitId", SqlDbType.Int);
                arr[18] = new SqlParameter("@PrmIdOutPut", SqlDbType.Int);
                arr[19] = new SqlParameter("@PrtIdOutPut", SqlDbType.Int);
                arr[20] = new SqlParameter("@UpdateFlag", SqlDbType.Int);
                arr[21] = new SqlParameter("@ConeTypeId", SqlDbType.Int);
                arr[22] = new SqlParameter("@ItemRemarks", SqlDbType.VarChar, 500);
                arr[23] = new SqlParameter("@msg", SqlDbType.VarChar, 500);
                arr[24] = new SqlParameter("@TagNo", SqlDbType.VarChar, 50);
                arr[25] = new SqlParameter("@BeamType", SqlDbType.TinyInt);
                arr[26] = new SqlParameter("@BeamNo", SqlDbType.VarChar, 50);
                arr[27] = new SqlParameter("@flagsize", SqlDbType.Int);

                //int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, Tran, ddlshade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                if (ViewState["Prmid"] == null)
                {
                    ViewState["Prmid"] = "0";
                }
                //Beam type
                if (ViewState["Beamtype"].ToString() != lblBeamtype.Text)
                {
                    ViewState["Prmid"] = "0";
                }
                //
                arr[0].Value = ViewState["Prmid"];
                arr[1].Value = lblCompanyId.Text;// ddCompName.SelectedValue;
                arr[2].Value = 0;// ddempname.SelectedValue;
                arr[3].Value = 1;// ddProcessName.SelectedValue;
                arr[4].Value = lblIssueOrderId.Text;
                arr[5].Value = System.DateTime.Now.ToString("dd-MMM-yyyy");
                arr[6].Value = "";// txtchalanno.Text;
                arr[6].Direction = ParameterDirection.InputOutput;
                arr[7].Value = 1; //TranType
                arr[8].Value = Session["varuserid"].ToString();
                arr[9].Value = Session["varMasterCompanyIDForERP"].ToString();
                arr[10].Value = 0;
                //if (btnsave.Text == "Update")
                //{
                //    arr[10].Value = gvdetail.SelectedDataKey.Value;
                //    arr[20].Value = 1;
                //}
                arr[11].Value = lblCategoryId.Text;
                arr[12].Value = lblitemid.Text;
                arr[13].Value = lblFinishedId.Text;
                arr[14].Value = lblGodownid.Text;
                arr[15].Value = txtreturnQty.Text;
                arr[16].Value = lbllotNo.Text;
                arr[17].Value = lblUnitId.Text;
                arr[18].Direction = ParameterDirection.Output;
                arr[19].Direction = ParameterDirection.Output;
                arr[20].Value = 0;
                arr[21].Value = 0;// conetypeId
                arr[22].Value = "";
                arr[23].Direction = ParameterDirection.Output;
                arr[24].Value = lblTagNo.Text;
                arr[25].Value = lblBeamtype.Text == "" ? "0" : lblBeamtype.Text;
                arr[26].Value = lblBeamNo.Text;
                arr[27].Value = lblflagsize.Text;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PROCESS_RAW_BEAM_RETURN", arr);

                // UtilityModule.StockStockTranTableUpdate(Convert.ToInt16(arr[13].Value), Convert.ToInt16(lblGodownid.Text), Convert.ToInt16(lblCompanyId.Text), lbllotNo.Text, Convert.ToDouble(txtreturnQty.Text), DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToString("dd-MMM-yyyy"), "ProcessRawTran", Convert.ToInt32(arr[19].Value), Tran, 1, true, 1, 0);

                Tran.Commit();
                ViewState["Prmid"] = arr[18].Value;
                ViewState["Beamtype"] = arr[25].Value;
                lblMessage.Text = "Data  Saved Successfully....";
                Fill_Grid();

            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblMessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
}
