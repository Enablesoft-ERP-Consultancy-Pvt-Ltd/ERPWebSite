using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_RawMaterial_frmRawMaterialIssueForAnisa : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
    }
    protected void txtfolioNo_TextChanged(object sender, EventArgs e)
    {
        if (txtfolioNo.Text != "")
        {
            ViewState["Prmid"] = "0";
            TdDGConsumptionConeType.Style.Add("Display", "none");
            TdDgConsumption.Style.Add("Display", "none");
            ViewState["IssueOrderId"] = txtfolioNo.Text;
            TDPreview.Visible = true;
            Fill_Grid();
            if (chkforCone.Checked == true)
            {
                TdDGConsumptionConeType.Style.Add("Display", "Block");
                fill_Grid_ShowConsmptionConeType();
            }
            else
            {
                TdDgConsumption.Style.Add("Display", "Block");
                fill_Grid_ShowConsmption();
            }
        }
    }
    private void fill_Grid_ShowConsmptionConeType()
    {
        DataSet ds = null;
        try
        {
            string strsql = @"SELECT VF1.Category_Name,VF1.Item_Name,VF1.QualityName+Space(2)+VF1.DesignName+Space(2)+VF1.ColorName+Space(2)+VF1.ShapeName+Space(2)+
                            CASE WHEN PM.UnitId=1 Then VF1.SizeMtr else VF1.SizeFt END+Space(2)+VF1.ShadeColorName Description,
                            Isnull(Round(Sum(CASE WHEN PM.CalType=0 or PM.Caltype=2 THEN CASE WHEN PM.UnitId=1 Then PD.Qty*PD.Area*OCD.IQTY*1.196 else PD.Qty*PD.Area*OCD.IQTY END ELSE 
                            CASE WHEN PM.UnitId=1 Then PD.Qty*OCD.IQTY else PD.Qty*OCD.IQTY END END),3),0) ConsmpQTY,
                            [dbo].[Get_ProcessIssueQty] (OCD.IFINISHEDID,PM.Issueorderid) IssuedQty,Round(Isnull(Round(Sum(CASE WHEN PM.CalType=0 or PM.Caltype=2 THEN CASE WHEN 
                            PM.UnitId=1 Then PD.Qty*PD.Area*OCD.IQTY*1.196 else PD.Qty*PD.Area*OCD.IQTY END ELSE CASE WHEN PM.UnitId=1 Then PD.Qty*OCD.IQTY else 
                            PD.Qty*OCD.IQTY END END),3),0)-[dbo].[Get_ProcessIssueQty] (OCD.IFINISHEDID,PM.Issueorderid),3) PendQty,OCD.IFinishedid 
                            FROM PROCESS_CONSUMPTION_DETAIL OCD,PROCESS_ISSUE_MASTER_1 PM,PROCESS_ISSUE_DETAIL_1 PD,
                            V_FinishedItemDetail VF1 Where PM.IssueOrderid=PD.IssueOrderid And OCD.Issueorderid=PD.Issueorderid And OCD.Issue_Detail_Id=PD.Issue_Detail_Id And 
                            VF1.ITEM_FINISHED_ID=OCD.IFINISHEDID And PM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And PM.Issueorderid=" + ViewState["IssueOrderId"] + " And VF1.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                            Group By VF1.Category_Name,VF1.Item_Name,VF1.QualityName,VF1.DesignName,VF1.ColorName,VF1.ShapeName,PM.UnitId,VF1.SizeMtr,VF1.SizeFt,
                            VF1.ShadeColorName,OCD.IFINISHEDID,PM.Issueorderid";

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DGConsumptionConeType.DataSource = ds;
                DGConsumptionConeType.DataBind();
            }
            else
            {
                DGConsumptionConeType.DataSource = ds;
                DGConsumptionConeType.DataBind();
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Infromation", "alert('No Records found...');", true);
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = ex.Message;
        }
    }
    private void fill_Grid_ShowConsmption()
    {
        DataSet ds = null;
        try
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@issueOrderid", ViewState["IssueOrderId"]);
            param[1] = new SqlParameter("@Processid", 1);
            param[2] = new SqlParameter("@CompanyID", Session["CurrentWorkingCompanyID"]);

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_RawMaterailIssueDataForANISA", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DGConsumption.DataSource = ds;
                DGConsumption.DataBind();
            }
            else
            {
                DGConsumption.DataSource = ds;
                DGConsumption.DataBind();
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Infromation", "alert('No Records found...');", true);
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = ex.Message;
        }
    }
    protected void DGConsumption_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlgodown = (DropDownList)e.Row.FindControl("ddgodown");
            int itemFinishedid = Convert.ToInt32(DGConsumption.DataKeys[e.Row.RowIndex].Value);
            UtilityModule.ConditionalComboFill(ref ddlgodown, "Select Distinct GM.GodownID,GM.GodownName From GodownMaster GM,Stock S Where GM.GodownID=S.GodownID And Round(Qtyinhand, 3) > 0 And CompanyId=" + Session["CurrentWorkingCompanyID"] + " And item_finished_id=" + itemFinishedid + " And GM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By GodownName", true, "--Plz Select godown--");
            DropDownList ddlotno = (DropDownList)e.Row.FindControl("ddlotno");
            int Units;
            Units = Convert.ToInt16(((Label)DGOrderdetail.Rows[0].FindControl("lblUnits")).Text);
            string ItemName = ((Label)e.Row.FindControl("lblItemName")).Text;
            string str = "";
            if (ItemName == "WOOLLEN YARN")
            {
                str = "select Top 1 ";
            }
            else
            {
                str = "select ";
            }
            str = str + "s.Lotno, s.LotNo From Stock s(Nolock) Where Round(s.Qtyinhand, 3) > 0 And s.CompanyId=" + Session["CurrentWorkingCompanyID"] + " And s.item_Finished_id=" + itemFinishedid;
            if (ItemName == "WOOLLEN YARN")
            {
                str = str + " And s.Qtyinhand > " + ((Label)e.Row.FindControl("lblPendQty")).Text;
            }
            switch (Convert.ToInt16(Units))
            {
                case 1: //Kanpur
                    str = str + " And s.godownId=1";
                    break;
                case 2:  //Biswan
                    str = str + " And s.godownId=2";
                    break;
                case 3: //Laharpur
                    str = str + " And s.godownId=3";
                    break;
                case 4: //KHAIRABAD
                    str = str + " And s.godownId=4";
                    break;
                case 5: //ISMAILPUR
                    str = str + " And s.godownId=5";
                    break;
            }
            str = str + " Order By s.Lotno";
            UtilityModule.ConditionalComboFill(ref ddlotno, str, true, "--Plz Select Lot No.--");
        }
    }
    protected void DGConsumption_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblConsumption.Text = "";
        if (e.CommandName == "Save")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] arr = new SqlParameter[24];
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                int row = gvr.RowIndex;
                int godownid = 1;
                int units = Convert.ToInt16(((Label)DGOrderdetail.Rows[0].FindControl("lblUnits")).Text);
                switch (Convert.ToInt16(units))
                {
                    case 1:   //Kanpur
                        godownid = 1;
                        break;
                    case 2:   //Biswan
                        godownid = 2;
                        break;
                    case 3:  //Laharpur
                        godownid = 3;
                        break;
                    case 4: //KHAIRABAD
                        godownid = 4;
                        break;
                    case 5: //ISMAILPUR
                        godownid = 5;
                        break;
                }
                //godownid = Convert.ToInt16(((DropDownList)DGConsumption.Rows[row].FindControl("ddgodown")).SelectedValue);
                int lotnoindex = ((DropDownList)DGConsumption.Rows[row].FindControl("ddlotNo")).SelectedIndex;
                string lotno = "";
                if (lotnoindex > 0)
                {
                    lotno = ((DropDownList)DGConsumption.Rows[row].FindControl("ddlotNo")).SelectedItem.Text;
                }
                Double IssueQty = Convert.ToDouble(((TextBox)DGConsumption.Rows[row].FindControl("txtIssueQty")).Text == "" ? "0" : ((TextBox)DGConsumption.Rows[row].FindControl("txtIssueQty")).Text);
                if (godownid == 0 || lotnoindex <= 0 || IssueQty == 0)
                {
                    string Message = "";
                    if (godownid == 0)
                    {
                        Message = "Plz Select godown Name..." + Environment.NewLine;
                    }
                    if (lotnoindex <= 0)
                    {
                        Message = Message + "Plz Select Lot No..." + Environment.NewLine;
                    }
                    if (IssueQty == 0)
                    {
                        Message = Message + "Plz fill Issue Qty..." + Environment.NewLine;
                    }
                    lblConsumption.Text = Message;
                    Tran.Commit();
                    return;
                }

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
                arr[23] = new SqlParameter("@Msg", SqlDbType.VarChar, 500);

                //int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, Tran, ddlshade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                if (ViewState["Prmid"] == null)
                {
                    ViewState["Prmid"] = "0";
                }
                arr[0].Value = ViewState["Prmid"];
                arr[1].Value = Session["CurrentWorkingCompanyID"];// ddCompName.SelectedValue;
                arr[2].Value = 0;// ddempname.SelectedValue;
                arr[3].Value = 1;// ddProcessName.SelectedValue;
                arr[4].Value = ViewState["IssueOrderId"];// ddOrderNo.SelectedValue;
                arr[5].Value = System.DateTime.Now.ToString("dd-MMM-yyyy");
                arr[6].Value = "";// txtchalanno.Text;
                arr[6].Direction = ParameterDirection.InputOutput;
                arr[7].Value = 0;
                arr[8].Value = Session["varuserid"].ToString();
                arr[9].Value = Session["varMasterCompanyIDForERP"].ToString();
                arr[10].Value = 0;
                arr[20].Value = 0;
                //if (btnsave.Text == "Update")
                //{
                //    arr[10].Value = gvdetail.SelectedDataKey.Value;
                //    arr[20].Value = 1;
                //}
                arr[11].Value = 2;// ddCatagory.SelectedValue;
                arr[12].Value = 0;// dditemname.SelectedValue;
                arr[13].Value = DGConsumption.DataKeys[row].Value;

                arr[14].Value = godownid;
                arr[15].Value = IssueQty;
                arr[16].Value = lotno;
                arr[17].Value = 3;//Unit KG
                arr[18].Direction = ParameterDirection.Output;
                arr[19].Direction = ParameterDirection.Output;
                arr[21].Value = 0;// conetypeId
                arr[22].Value = ((TextBox)DGConsumption.Rows[row].FindControl("txtRemarks")).Text;
                arr[23].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PROCESS_RAW_ISSUEForAnisha", arr);
                if (arr[23].Value.ToString() == "")  //msg
                {
                    UtilityModule.StockStockTranTableUpdate(Convert.ToInt16(arr[13].Value), godownid, 1, lotno, IssueQty, Convert.ToDateTime(System.DateTime.Now.ToString("dd-MMM-yyyy")).ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), "ProcessRawTran", Convert.ToInt32(arr[19].Value), Tran, 0, false, 1, 0);
                    ViewState["Prmid"] = arr[18].Value;
                    lblConsumption.Text = "Consumption Saved Siccessfully....";
                    Tran.Commit();
                    fill_Grid_ShowConsmption();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "msg", "alert('" + arr[23].Value.ToString() + "');", true);
                    Tran.Commit();
                }
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblConsumption.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void ddgodown_onSelectedindexChanged(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((DropDownList)sender).Parent.Parent;
        int itemFinishedid = Convert.ToInt16(DGConsumption.DataKeys[row.RowIndex].Value);
        DropDownList ddlgodown = (DropDownList)sender;
        DropDownList ddllotno = (DropDownList)row.FindControl("ddlotNo");

        UtilityModule.ConditionalComboFill(ref ddllotno, "select Lotno,LotNo From Stock Where CompanyId=" + Session["CurrentWorkingCompanyID"] + " and godownId=" + ddlgodown.SelectedValue + " And item_Finished_id=" + itemFinishedid + "", true, "--Plz Select Lot No.--");

    }
    protected void ddlotnoDgConsumption_onSelectedindexChanged(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((DropDownList)sender).Parent.Parent;
        int itemFinishedid = Convert.ToInt16(DGConsumption.DataKeys[row.RowIndex].Value);
        DropDownList ddlgodown = (DropDownList)sender;
        DropDownList ddllotno = (DropDownList)row.FindControl("ddlotNo");
        TextBox txtStockQty = (TextBox)row.FindControl("txtStockQty");
        int godownId = 1;
        int Units;
        Units = Convert.ToInt16(((Label)DGOrderdetail.Rows[0].FindControl("lblUnits")).Text);
        switch (Units)
        {
            case 1:
                godownId = 1;
                break;
            case 2:
                godownId = 2;

                break;
            case 3:
                godownId = 3;
                break;
            case 4:
                godownId = 4;
                break;
            case 5:
                godownId = 5;
                break;
        }
        txtStockQty.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Round(isnull(Qtyinhand,0),3) As Stock from Stock Where godownId=" + godownId + " and CompanyId=" + Session["CurrentWorkingCompanyID"] + " And LotNo='" + ddllotno.SelectedItem.Text + "'  And Item_Finished_id=" + itemFinishedid + "").ToString();
        // UtilityModule.ConditionalComboFill(ref ddllotno, "select Lotno,LotNo From Stock Where CompanyId=1 and godownId=" + ddlgodown.SelectedValue + " And item_Finished_id=" + itemFinishedid + "", true, "--Plz Select Lot No.--");

    }
    protected void ddlotnoDgConsumptionConeType_onSelectedindexChanged(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((DropDownList)sender).Parent.Parent;
        int itemFinishedid = Convert.ToInt16(DGConsumptionConeType.DataKeys[row.RowIndex].Value);
        DropDownList ddlgodown = (DropDownList)sender;
        DropDownList ddllotno = (DropDownList)row.FindControl("ddlotNo");
        TextBox txtStockQty = (TextBox)row.FindControl("txtStockQty");
        int godownId = 1;
        int Units;
        Units = Convert.ToInt16(((Label)DGOrderdetail.Rows[0].FindControl("lblUnits")).Text);
        switch (Units)
        {
            case 1:
                godownId = 1;
                break;
            case 2:
                godownId = 2;

                break;
            case 3:
                godownId = 3;
                break;
            case 4:
                godownId = 4;
                break;
            case 5:
                godownId = 5;
                break;
        }
        txtStockQty.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Round(isnull(Qtyinhand,0),3) As Stock from Stock Where godownId=" + godownId + " and CompanyId=" + Session["CurrentWorkingCompanyID"] + " And LotNo='" + ddllotno.SelectedItem.Text + "'  And Item_Finished_id=" + itemFinishedid + "").ToString();
        // UtilityModule.ConditionalComboFill(ref ddllotno, "select Lotno,LotNo From Stock Where CompanyId=1 and godownId=" + ddlgodown.SelectedValue + " And item_Finished_id=" + itemFinishedid + "", true, "--Plz Select Lot No.--");

    }

    protected void DGConsumptionConeType_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        lblConsumption.Text = "";
        if (e.CommandName == "Save")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] arr = new SqlParameter[23];
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int row = gvr.RowIndex;

                int godownid = 1;
                int units = Convert.ToInt16(((Label)DGOrderdetail.Rows[0].FindControl("lblUnits")).Text);
                switch (Convert.ToInt16(units))
                {
                    case 1:   //Kanpur
                        godownid = 1;
                        break;
                    case 2:   //Biswan
                        godownid = 2;
                        break;
                    case 3:  //Laharpur
                        godownid = 3;
                        break;
                    case 4:  //KHAIRABAD
                        godownid = 4;
                        break;
                    case 5:  //ISMAILPUR
                        godownid = 5;
                        break;
                }
                //godownid = Convert.ToInt16(((DropDownList)DGConsumption.Rows[row].FindControl("ddgodown")).SelectedValue);
                int lotnoindex = ((DropDownList)DGConsumptionConeType.Rows[row].FindControl("ddlotNo")).SelectedIndex;
                string lotno = "";
                if (lotnoindex > 0)
                {
                    lotno = ((DropDownList)DGConsumptionConeType.Rows[row].FindControl("ddlotNo")).SelectedItem.Text;
                }
                Double IssueQty = Convert.ToDouble(((TextBox)DGConsumptionConeType.Rows[row].FindControl("txtIssueQty")).Text == "" ? "0" : ((TextBox)DGConsumptionConeType.Rows[row].FindControl("txtIssueQty")).Text);
                Double NoofCones = Convert.ToDouble(((TextBox)DGConsumptionConeType.Rows[row].FindControl("txtNoofCones")).Text == "" ? "0" : ((TextBox)DGConsumptionConeType.Rows[row].FindControl("txtNoofCones")).Text);
                //Check ConeType And Find Lotno And Weight
                int ConeTypeId = 0;
                Double ConeQty = 0;
                int ConeTypeIndex = ((DropDownList)DGConsumptionConeType.Rows[row].FindControl("DDConeType")).SelectedIndex;
                if (ConeTypeIndex > 0)
                {
                    ConeTypeId = Convert.ToInt16(((DropDownList)DGConsumptionConeType.Rows[row].FindControl("DDConeType")).SelectedValue);
                    string str = "select LotNo,Qty from Conetype Where id=" + ConeTypeId + "";
                    DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        ConeQty = Convert.ToDouble(ds.Tables[0].Rows[0]["Qty"]);
                        IssueQty = IssueQty - (ConeQty * NoofCones);
                    }
                }
                //// End
                #region
                //if (godownid == 0 || lotnoindex <= 0 || IssueQty == 0)
                //{
                //    string Message = "";
                //    if (godownid == 0)
                //    {
                //        Message = "Plz Select godown Name..." + Environment.NewLine;
                //    }
                //    if (lotnoindex <= 0)
                //    {
                //        Message = Message + "Plz Select Lot No..." + Environment.NewLine;
                //    }
                //    if (IssueQty == 0)
                //    {
                //        Message = Message + "Plz fill Issue Qty..." + Environment.NewLine;
                //    }
                //    lblConsumption.Text = Message;
                //    Tran.Commit();
                //    return;
                //}
                #endregion
                if (ConeTypeIndex <= 0 || lotnoindex <= 0 || IssueQty == 0 || NoofCones == 0)
                {
                    string Message = "";
                    if (lotnoindex <= 0)
                    {
                        Message = "Plz select Lot No.." + Environment.NewLine;
                    }
                    if (ConeTypeIndex <= 0)
                    {
                        Message = Message + "Plz select Cone Type.." + Environment.NewLine;
                    }
                    if (NoofCones == 0)
                    {
                        Message = Message + "Plz fill No. of Cones.." + Environment.NewLine;
                    }
                    if (IssueQty == 0)
                    {
                        Message = Message + "Plz fill Issue Qty.." + Environment.NewLine;
                    }
                    lblConsumption.Text = Message;
                    Tran.Commit();
                    return;
                }

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
                //int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, Tran, ddlshade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                if (ViewState["Prmid"] == null)
                {
                    ViewState["Prmid"] = "0";
                }
                arr[0].Value = ViewState["Prmid"];
                arr[1].Value = 1;// ddCompName.SelectedValue;
                arr[2].Value = 0;// ddempname.SelectedValue;
                arr[3].Value = 1;// ddProcessName.SelectedValue;
                arr[4].Value = ViewState["IssueOrderId"];// ddOrderNo.SelectedValue;
                arr[5].Value = System.DateTime.Now.ToString("dd-MMM-yyyy");
                arr[6].Value = "";// txtchalanno.Text;
                arr[6].Direction = ParameterDirection.InputOutput;
                arr[7].Value = 0;
                arr[8].Value = Session["varuserid"].ToString();
                arr[9].Value = Session["varMasterCompanyIDForERP"].ToString();
                arr[10].Value = 0;
                arr[20].Value = 0;
                //if (btnsave.Text == "Update")
                //{
                //    arr[10].Value = gvdetail.SelectedDataKey.Value;
                //    arr[20].Value = 1;
                //}
                arr[11].Value = 2;// ddCatagory.SelectedValue;
                arr[12].Value = 0;// dditemname.SelectedValue;
                arr[13].Value = DGConsumptionConeType.DataKeys[row].Value;

                arr[14].Value = godownid;
                arr[15].Value = IssueQty;
                arr[16].Value = lotno;
                arr[17].Value = 3;//Unit KG
                arr[18].Direction = ParameterDirection.Output;
                arr[19].Direction = ParameterDirection.Output;
                arr[21].Value = ConeTypeId;
                arr[22].Value = ((TextBox)DGConsumptionConeType.Rows[row].FindControl("txtRemarks")).Text;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PROCESS_RAW_ISSUEForAnisha", arr);
                UtilityModule.StockStockTranTableUpdate(Convert.ToInt16(arr[13].Value), godownid, 1, lotno, IssueQty, Convert.ToDateTime(System.DateTime.Now.ToString("dd-MMM-yyyy")).ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), "ProcessRawTran", Convert.ToInt32(arr[19].Value), Tran, 0, false, 1, 0);
                Tran.Commit();
                ViewState["Prmid"] = arr[18].Value;
                lblConsumption.Text = "Consumption Saved Siccessfully....";
                fill_Grid_ShowConsmptionConeType();

            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblConsumption.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

    }
    protected void DGConsumptionConeType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddconetype = (DropDownList)e.Row.FindControl("DDConeType");
            int Item_Finished_id = Convert.ToInt16(DGConsumptionConeType.DataKeys[e.Row.RowIndex].Value);
            UtilityModule.ConditionalComboFill(ref ddconetype, "select ID,ConeType+space(2)+'/'+cast(Qty as nvarchar)+'  '+'kg.' As ConeType From ConeType Where Item_FInished_id=" + Item_Finished_id + " order by ConeType", true, "--Plz Select Cone Type--");
            DropDownList ddlotno = (DropDownList)e.Row.FindControl("ddlotno");

            int Units;
            Units = Convert.ToInt16(((Label)DGOrderdetail.Rows[0].FindControl("lblUnits")).Text);
            switch (Convert.ToInt16(Units))
            {
                case 1: //Kanpur
                    UtilityModule.ConditionalComboFill(ref ddlotno, "select Lotno,LotNo From Stock Where Round(Qtyinhand, 3) > 0 And CompanyId=" + Session["CurrentWorkingCompanyID"] + " and godownId=1 And item_Finished_id=" + Item_Finished_id + "", true, "--Plz Select Lot No.--");
                    break;
                case 2:  //Biswan
                    UtilityModule.ConditionalComboFill(ref ddlotno, "select Lotno,LotNo From Stock Where Round(Qtyinhand, 3) > 0 And CompanyId=" + Session["CurrentWorkingCompanyID"] + " and godownId=2 And item_Finished_id=" + Item_Finished_id + "", true, "--Plz Select Lot No.--");
                    break;
                case 3: //Laharpur
                    UtilityModule.ConditionalComboFill(ref ddlotno, "select Lotno,LotNo From Stock Where Round(Qtyinhand, 3) > 0 And CompanyId=" + Session["CurrentWorkingCompanyID"] + " and godownId=3 And item_Finished_id=" + Item_Finished_id + "", true, "--Plz Select Lot No.--");
                    break;
                case 4: //KHAIRABAD
                    UtilityModule.ConditionalComboFill(ref ddlotno, "select Lotno,LotNo From Stock Where Round(Qtyinhand, 3) > 0 And CompanyId=" + Session["CurrentWorkingCompanyID"] + " and godownId=4 And item_Finished_id=" + Item_Finished_id + "", true, "--Plz Select Lot No.--");
                    break;
                case 5: //ISMAILPUR
                    UtilityModule.ConditionalComboFill(ref ddlotno, "select Lotno,LotNo From Stock Where Round(Qtyinhand, 3) > 0 And CompanyId=" + Session["CurrentWorkingCompanyID"] + " and godownId=5 And item_Finished_id=" + Item_Finished_id + "", true, "--Plz Select Lot No.--");
                    break;
            }
        }
    }
    protected void chkforCone_CheckedChanged(object sender, EventArgs e)
    {
        if (txtfolioNo.Text != "")
        {
            TdDGConsumptionConeType.Style.Add("Display", "none");
            TdDgConsumption.Style.Add("Display", "none");
            ViewState["IssueOrderId"] = txtfolioNo.Text;
            if (chkforCone.Checked == true)
            {
                TdDGConsumptionConeType.Style.Add("Display", "Block");
                fill_Grid_ShowConsmptionConeType();
            }
            else
            {
                TdDgConsumption.Style.Add("Display", "Block");
                fill_Grid_ShowConsmption();
            }
        }
    }
    protected void DGOrderdetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    protected void Fill_Grid()
    {

        string sqlstr = @"Select Issue_Detail_Id as IssueDetailId,vf.Category_Name as Category,vf.Item_Name as Articles,vf.ColorName As Colour,Length,Width,
                        Width + 'x' + Length Size,Area,Rate,Qty,Amount,OrderId,PD.Item_Finished_Id,units From PROCESS_ISSUE_MASTER_1 PM,PROCESS_ISSUE_DETAIL_1 PD,
                        V_FinishedItemDetail vf
                        Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=vf.Item_Finished_id  and PM.status<>'canceled'
                        And PM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And PM.IssueOrderid=" + ViewState["IssueOrderId"] + " And vf.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By Issue_Detail_Id Desc";

        try
        {
            DataSet DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sqlstr);
            if (DS.Tables[0].Rows.Count > 0)
            {
                DGOrderdetail.DataSource = DS.Tables[0];
                DGOrderdetail.DataBind();
            }
            else
            {
                DGOrderdetail.DataSource = null;
                DGOrderdetail.DataBind();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/DirectProduction.aspx");
            lblErrmsg.Text = ex.Message;
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] _array = new SqlParameter[4];
        _array[0] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
        _array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
        _array[2] = new SqlParameter("@Trantype", SqlDbType.Int);

        _array[0].Value = ViewState["IssueOrderId"];
        _array[1].Value = 1;//For IST Process
        _array[2].Value = 0; //For Issue

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "[Pro_OrderFolio_IssuedSlip]", _array);

        if (ds.Tables[1].Rows.Count > 0) // 1 For OrderDetail
        {
            Session["rptFileName"] = "~\\Reports\\RptOrderFolio_Issuedslip.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptOrderFolio_Issuedslip.xsd";

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
}