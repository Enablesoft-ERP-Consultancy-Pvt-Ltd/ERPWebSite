using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Data.SqlTypes;


public partial class Masters_Process_frmProductionReceive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {

            //SqlhelperEnum.FillDropDown(AllEnums.MasterTables.Units, ddUnits, pID: "UnitsId", pName: "Unitname",pSort:true,PsortName:"UnitsId");
            UtilityModule.ConditionalComboFill(ref ddUnits, "select U.UnitsId,U.UnitName from Units U inner join Units_authentication UA on U.unitsId=UA.UnitsId and UA.Userid=" + Session["varuserid"] + " order by U.unitsId", true, "");
        }
    }
    protected void btnshowDetail_Click(object sender, EventArgs e)
    {

        fill_Grid();
    }
    protected void fill_Grid()
    {
        string strEmpid = "";
        //Find EmployeeId in listbox
        for (int i = 0; i < listWeaverName.Items.Count; i++)
        {
            if (strEmpid == "")
            {
                strEmpid = listWeaverName.Items[i].Value;
            }
            else
            {
                strEmpid = strEmpid + "," + listWeaverName.Items[i].Value;
            }
        }
        ///////
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            //Check EmployeeId
            if (strEmpid == "")
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Alert", "alert('Plz Enter Weaver ID No. First..');", true);
                return;
            }
            SqlParameter[] array = new SqlParameter[4];
            array[0] = new SqlParameter("@Date", SqlDbType.SmallDateTime);
            array[1] = new SqlParameter("@EmpId", SqlDbType.VarChar, 30);
            array[2] = new SqlParameter("@UnitsId", SqlDbType.Int);
            array[3] = new SqlParameter("@CompanyID", SqlDbType.Int);


            array[0].Value = System.DateTime.Now.ToString("dd-MMM-yyyy");
            array[1].Value = strEmpid;
            array[2].Value = ddUnits.SelectedValue;
            array[3].Value = Session["CurrentWorkingCompanyID"];

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Pro_fillDirectionProductionData", array);

            divmultipleconstruction.Visible = false;
            if (ds.Tables[0].Rows.Count > 0)
            {
                divGrid.Style.Add("Display", "block");
                DGReceiveDetail.DataSource = ds.Tables[0];
                DGReceiveDetail.DataBind();
                ////Find Stock on the basis of Units
                string Prefix = "";
                string Postfix = "";
                //get Prefix
                if (ddUnits.SelectedValue == "4") //KhairaBad
                {
                    Prefix = (ddUnits.SelectedItem.Text).Substring(0, 2) + "-";
                }
                else
                {
                    Prefix = (ddUnits.SelectedItem.Text).Substring(0, 1) + "-";
                }

                //Post fix
                Postfix = UtilityModule.CalculatePostFix(Prefix).ToString();

                //Assign value in stockNo textbox
                for (int i = 0; i < DGReceiveDetail.Rows.Count; i++)
                {
                    TextBox StockNo = ((TextBox)DGReceiveDetail.Rows[i].FindControl("txtStockNo"));
                    StockNo.Text = Prefix + Postfix;
                }
                //*****Multiple Construction                
                if (ds.Tables[1].Rows.Count > 0)
                {
                    divmultipleconstruction.Visible = true;
                    Dgmultiplecons.DataSource = ds.Tables[1];
                    Dgmultiplecons.DataBind();
                }

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Alert1", "alert('No More Records available for Receive...');", true);
                DGReceiveDetail.DataSource = null;
                DGReceiveDetail.DataBind();
                return;
            }

        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }

    }

    protected void txtWeaverIdNo_TextChanged(object sender, EventArgs e)
    {
        ViewState["Process_Rec_Id"] = 0;

        try
        {
            if (txtWeaverIdNo.Text != "")
            {

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Empid,Empname from empinfo Where Empcode='" + txtWeaverIdNo.Text + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    listWeaverName.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Empname"].ToString(), ds.Tables[0].Rows[0]["Empid"].ToString()));
                    txtWeaverIdNo.Text = "";
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('No Weaver found at this ID No..');", true);
                }

                ds.Dispose();

            }
            txtWeaverIdNo.Focus();
        }
        catch (Exception ex)
        {
            lblmessage.Visible = true;
            lblmessage.Text = ex.Message;
        }
        finally
        {

        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        listWeaverName.Items.Remove(listWeaverName.SelectedItem);
    }

    protected void txtLength_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = ((TextBox)sender).NamingContainer as GridViewRow;
        TextBox TxtLength = ((TextBox)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("txtlength"));
        TxtLength.Text = ((TextBox)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("txtlength")).Text;
        TextBox TxtWidth = ((TextBox)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("txtWidth"));
        TxtWidth.Text = ((TextBox)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("txtWidth")).Text;

        Label lblArea = ((Label)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("lblArea"));
        Label lblUnitId = ((Label)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("lblUnitId"));
        Label lblFinishedid = ((Label)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("lblItemFinishedid"));
        Label lblCalType = ((Label)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("lblCalType"));

        ////////////////
        int FootLength = 0;
        int FootWidth = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        string Str = "";
        if (TxtLength.Text != "")
        {
            if (Convert.ToInt32(lblUnitId.Text) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(TxtLength.Text));
                TxtLength.Text = Str;
                FootLength = Convert.ToInt32(Str.Split('.')[0]);
                FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootLengthInch > 11)
                {
                    lblmessage.Text = "Inch value must be less than 12";
                    TxtLength.Text = "";
                    TxtLength.Focus();
                }
            }
        }
        if (TxtWidth.Text != "")
        {
            if (Convert.ToInt32(lblUnitId.Text) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(TxtWidth.Text));
                TxtWidth.Text = Str;
                FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootWidthInch > 11)
                {
                    lblmessage.Text = "Inch value must be less than 12";
                    TxtWidth.Text = "";
                    TxtWidth.Focus();
                }
            }
        }
        if (TxtLength.Text != "" && TxtWidth.Text != "")
        {
            int Shape = 0;
            Shape = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From V_FinishedItemDetail Where Item_Finished_Id=" + lblFinishedid.Text + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + ""));

            if (Convert.ToInt32(lblUnitId.Text) == 1)
            {
                lblArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(lblCalType.Text), Shape));
            }
            if (Convert.ToInt32(lblUnitId.Text) == 2)
            {
                lblArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(lblCalType.Text), Shape));
            }
        }

    }
    protected void txtWidth_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = ((TextBox)sender).NamingContainer as GridViewRow;
        TextBox TxtLength = ((TextBox)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("txtlength"));
        TxtLength.Text = ((TextBox)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("txtlength")).Text;
        TextBox TxtWidth = ((TextBox)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("txtWidth"));
        TxtWidth.Text = ((TextBox)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("txtWidth")).Text;

        Label lblArea = ((Label)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("lblArea"));
        Label lblUnitId = ((Label)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("lblUnitId"));
        Label lblFinishedid = ((Label)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("lblItemFinishedid"));
        Label lblCalType = ((Label)DGReceiveDetail.Rows[gvr.RowIndex].FindControl("lblCalType"));

        ////////////////
        int FootLength = 0;
        int FootWidth = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        string Str = "";
        if (TxtLength.Text != "")
        {
            if (Convert.ToInt32(lblUnitId.Text) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(TxtLength.Text));
                TxtLength.Text = Str;
                FootLength = Convert.ToInt32(Str.Split('.')[0]);
                FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootLengthInch > 11)
                {
                    lblmessage.Text = "Inch value must be less than 12";
                    TxtLength.Text = "";
                    TxtLength.Focus();
                }
            }
        }
        if (TxtWidth.Text != "")
        {
            if (Convert.ToInt32(lblUnitId.Text) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(TxtWidth.Text));
                TxtWidth.Text = Str;
                FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootWidthInch > 11)
                {
                    lblmessage.Text = "Inch value must be less than 12";
                    TxtWidth.Text = "";
                    TxtWidth.Focus();
                }
            }
        }
        if (TxtLength.Text != "" && TxtWidth.Text != "")
        {
            int Shape = 0;
            Shape = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From V_FinishedItemDetail Where Item_Finished_Id=" + lblFinishedid.Text + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + ""));

            if (Convert.ToInt32(lblUnitId.Text) == 1)
            {
                lblArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(lblCalType.Text), Shape));
            }
            if (Convert.ToInt32(lblUnitId.Text) == 2)
            {
                lblArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(lblCalType.Text), Shape));
            }
        }

    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        //Check Recors in Grid
        if (DGReceiveDetail.Rows.Count == 0)
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Inform", "alert('No Records available for Receive...');", true);
            return;
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {

            SqlParameter[] _arrpara = new SqlParameter[38];
            _arrpara[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@Empid", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@ReceiveDate", SqlDbType.DateTime);
            _arrpara[3] = new SqlParameter("@Unitid", SqlDbType.Int);
            _arrpara[4] = new SqlParameter("@Userid", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 100);
            _arrpara[6] = new SqlParameter("@Companyid", SqlDbType.Int);
            _arrpara[7] = new SqlParameter("@Remarks", SqlDbType.VarChar, 100);
            _arrpara[8] = new SqlParameter("@Process_Rec_Detail_Id", SqlDbType.Int);
            _arrpara[9] = new SqlParameter("@Item_Finished_id", SqlDbType.Int);
            _arrpara[10] = new SqlParameter("@Length", SqlDbType.VarChar, 100);
            _arrpara[11] = new SqlParameter("@Width", SqlDbType.VarChar, 100);
            _arrpara[12] = new SqlParameter("@Area", SqlDbType.Float);
            _arrpara[13] = new SqlParameter("@Rate", SqlDbType.Float);
            _arrpara[14] = new SqlParameter("@Amount", SqlDbType.Float);
            _arrpara[15] = new SqlParameter("@Qty", SqlDbType.Int);
            _arrpara[16] = new SqlParameter("@Weight", SqlDbType.Float);
            _arrpara[17] = new SqlParameter("@Comm", SqlDbType.Float);
            _arrpara[18] = new SqlParameter("@CommAmt", SqlDbType.Float);
            _arrpara[19] = new SqlParameter("@IssueOrderid", SqlDbType.Int);
            _arrpara[20] = new SqlParameter("@Issue_Detail_Id", SqlDbType.Int);
            _arrpara[21] = new SqlParameter("@Orderid", SqlDbType.Int);
            _arrpara[22] = new SqlParameter("@Penality", SqlDbType.Float);
            _arrpara[23] = new SqlParameter("@QualityType", SqlDbType.Int);
            _arrpara[24] = new SqlParameter("@PRemark", SqlDbType.VarChar, 100);
            _arrpara[25] = new SqlParameter("@CalType", SqlDbType.Int);
            _arrpara[26] = new SqlParameter("@FlagFixOrWeight", SqlDbType.Int);
            _arrpara[27] = new SqlParameter("@TDSPercentage", SqlDbType.Float);
            _arrpara[28] = new SqlParameter("@ProcessId", SqlDbType.Int);

            _arrpara[29] = new SqlParameter("@Warp_10cm", SqlDbType.VarChar, 50);
            _arrpara[30] = new SqlParameter("@Weft_10cm", SqlDbType.VarChar, 500);
            _arrpara[31] = new SqlParameter("@Straightness", SqlDbType.VarChar, 50);
            _arrpara[32] = new SqlParameter("@Design", SqlDbType.VarChar, 50);
            _arrpara[33] = new SqlParameter("@OBA", SqlDbType.VarChar, 50);
            _arrpara[34] = new SqlParameter("@Date_Stamp", SqlDbType.VarChar, 50);
            _arrpara[35] = new SqlParameter("@WyPly", SqlDbType.Int);
            _arrpara[36] = new SqlParameter("@CyPly", SqlDbType.Int);
            _arrpara[37] = new SqlParameter("@Msg", SqlDbType.VarChar, 80);

            _arrpara[0].Direction = ParameterDirection.InputOutput;
            if (ViewState["Process_Rec_Id"].ToString() == "0" || ViewState["Process_Rec_Id"] == null)
            {
                ViewState["Process_Rec_Id"] = 0;
            }
            _arrpara[0].Value = ViewState["Process_Rec_Id"];
            _arrpara[1].Value = 0;//fix employeeId

            for (int i = 0; i < DGReceiveDetail.Rows.Count; i++)
            {
                Label lblCalType = ((Label)DGReceiveDetail.Rows[i].FindControl("lblCalType"));
                Label TxtArea = ((Label)DGReceiveDetail.Rows[i].FindControl("lblArea"));
                Label TxtRate = ((Label)DGReceiveDetail.Rows[i].FindControl("lblRate"));
                Label TxtRecQty = ((Label)DGReceiveDetail.Rows[i].FindControl("lblQty"));
                Label lblFlagFixorWeight = ((Label)DGReceiveDetail.Rows[i].FindControl("lblFlagFixorWeight"));
                Label TxtCommission = ((Label)DGReceiveDetail.Rows[i].FindControl("lblCommission"));
                Label lblItemFinishedid = ((Label)DGReceiveDetail.Rows[i].FindControl("lblItemFinishedid"));
                Label lblOrderId = ((Label)DGReceiveDetail.Rows[i].FindControl("lblOrderId"));
                TextBox TxtWeight = ((TextBox)DGReceiveDetail.Rows[i].FindControl("txtWeight"));
                TextBox TxtRecDate = ((TextBox)DGReceiveDetail.Rows[i].FindControl("txtDate"));
                TextBox txtWarp_10cm = ((TextBox)DGReceiveDetail.Rows[i].FindControl("txtWarp"));
                TextBox txtWeft_10cm = ((TextBox)DGReceiveDetail.Rows[i].FindControl("txtWeft"));
                TextBox txtstraightness = ((TextBox)DGReceiveDetail.Rows[i].FindControl("txtStraightness"));
                TextBox txtDesign = ((TextBox)DGReceiveDetail.Rows[i].FindControl("txtDesign"));
                TextBox txtOBA = ((TextBox)DGReceiveDetail.Rows[i].FindControl("txtOBA"));
                Label lblIssueOrderId = ((Label)DGReceiveDetail.Rows[i].FindControl("lblIssueOrderId"));
                Label lblIssueDetailid = ((Label)DGReceiveDetail.Rows[i].FindControl("lblIssueDetailId"));
                TextBox txtremarks = ((TextBox)DGReceiveDetail.Rows[i].FindControl("txtremarks"));
                TextBox txtDatestamp = ((TextBox)DGReceiveDetail.Rows[i].FindControl("txtDateStamp"));
                TextBox txtWyPly = ((TextBox)DGReceiveDetail.Rows[i].FindControl("txtWyPly"));
                TextBox txtCyPly = ((TextBox)DGReceiveDetail.Rows[i].FindControl("txtCyPly"));

                //*********Multiple Transaction
                string MultipleWeft = "";
                for (int j = 0; j < Dgmultiplecons.Rows.Count; j++)
                {
                    Label lblshadecolor = (Label)Dgmultiplecons.Rows[j].FindControl("lblshadecolor");
                    TextBox txtnoofweft_Shade = (TextBox)Dgmultiplecons.Rows[j].FindControl("txtnoofweft_Shade");

                    if (txtnoofweft_Shade.Text != "")
                    {

                        MultipleWeft = MultipleWeft + ", " + lblshadecolor.Text + " = " + txtnoofweft_Shade.Text;

                    }
                }
                MultipleWeft = MultipleWeft.TrimStart(',');
                //*********END

                _arrpara[2].Value = TxtRecDate.Text;
                _arrpara[3].Value = ((Label)DGReceiveDetail.Rows[i].FindControl("lblUnitId")).Text;
                _arrpara[4].Value = Session["varuserId"];
                _arrpara[5].Value = ViewState["Process_Rec_Id"].ToString();
                _arrpara[6].Value = Session["CurrentWorkingCompanyID"].ToString();
                _arrpara[7].Value = txtremarks.Text;
                _arrpara[8].Direction = ParameterDirection.Output;
                _arrpara[8].Value = 0;//Process_Receive_DetailId
                _arrpara[9].Value = lblItemFinishedid.Text;
                _arrpara[10].Value = ((TextBox)DGReceiveDetail.Rows[i].FindControl("txtlength")).Text;
                _arrpara[11].Value = ((TextBox)DGReceiveDetail.Rows[i].FindControl("txtWidth")).Text;
                _arrpara[12].Value = TxtArea.Text;
                _arrpara[13].Value = TxtRate.Text;

                if (lblCalType.Text == "0" || lblCalType.Text == "2" || lblCalType.Text == "3" || lblCalType.Text == "4")
                {
                    _arrpara[14].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtRate.Text) * Convert.ToDouble(TxtRecQty.Text)));
                    _arrpara[18].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtCommission.Text) * Convert.ToDouble(TxtRecQty.Text)));
                }
                if (lblCalType.Text == "1")
                {
                    _arrpara[14].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtRate.Text) * Convert.ToDouble(TxtRecQty.Text)));
                    _arrpara[18].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtCommission.Text) * Convert.ToDouble(TxtRecQty.Text)));
                }
                _arrpara[15].Value = TxtRecQty.Text;
                _arrpara[16].Value = TxtWeight.Text == "" ? "0" : TxtWeight.Text;
                _arrpara[17].Value = TxtCommission.Text == "" ? "0" : TxtCommission.Text;
                _arrpara[19].Value = lblIssueOrderId.Text;
                _arrpara[20].Value = lblIssueDetailid.Text;
                _arrpara[21].Value = lblOrderId.Text;
                _arrpara[22].Value = 0;//Penality
                _arrpara[23].Value = 1;//Stock type 1 Finishedid
                _arrpara[24].Value = "";//Penality Remark
                _arrpara[25].Value = lblCalType.Text;
                _arrpara[26].Value = lblFlagFixorWeight.Text;
                _arrpara[27].Value = 0;//TDS Percentage
                _arrpara[28].Value = 1; //Fix ProcessId  for Weaving
                _arrpara[29].Value = txtWarp_10cm.Text;
                if (MultipleWeft != "")
                {
                    _arrpara[30].Value = MultipleWeft;
                }
                else
                {
                    _arrpara[30].Value = txtWeft_10cm.Text;
                }
                _arrpara[31].Value = txtstraightness.Text;
                _arrpara[32].Value = txtDesign.Text;
                _arrpara[33].Value = txtOBA.Text;
                _arrpara[34].Value = txtDatestamp.Text;
                _arrpara[35].Value = txtWyPly.Text == "" ? "0" : txtWyPly.Text;
                _arrpara[36].Value = txtCyPly.Text == "" ? "0" : txtCyPly.Text;
                _arrpara[37].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_DirectProcessReceive]", _arrpara);
                if (_arrpara[37].Value.ToString() == "")
                {
                    ViewState["Process_Rec_Id"] = _arrpara[0].Value.ToString();
                    string Prefix = "";
                    //First two character of Units
                    if (ddUnits.SelectedValue == "4")
                    {
                        Prefix = ddUnits.SelectedItem.Text.Substring(0, 2) + "-";
                    }
                    else
                    {
                        Prefix = ddUnits.SelectedItem.Text.Substring(0, 1) + "-";
                    }
                    string Postfix = Convert.ToString(UtilityModule.CalculatePostFix((Prefix).ToUpper()));
                    UtilityModule.Insert_Into_Carpet_NumberAndProcess_StockDetailwithProc(Convert.ToInt32(lblItemFinishedid.Text), Convert.ToInt32(lblOrderId.Text), Convert.ToInt32(TxtRecQty.Text), Prefix, Convert.ToInt32(Postfix), Convert.ToInt32(Session["CurrentWorkingCompanyID"]), Convert.ToInt32(ViewState["Process_Rec_Id"]), Convert.ToInt32(_arrpara[8].Value), TxtRecDate.Text, Tran, 1, Convert.ToInt32(lblIssueDetailid.Text), Convert.ToInt32(_arrpara[4].Value));
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Save", "alert('Data Saved Successfully...');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "msg", "alert('" + _arrpara[37].Value.ToString() + "');", true);
                }
                Tran.Commit();
                DGReceiveDetail.DataSource = null;
                DGReceiveDetail.DataBind();
            }
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmessage.Text = ex.Message;

        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string qry;
            qry = @"Select CI.CompanyName,CI.CompAddr1,CI.CompAddr2,PD.Qty,IM.Item_Name,
                    Colorname ,Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Size,
                    PM.ReceiveDate,PD.IssueOrderid,U.UnitName,PM.UnitId,PM.ChallanNo,
                    1 PROCESSID,Penality,PRemarks From 
                    PROCESS_RECEIVE_MASTER_1 PM,PROCESS_RECEIVE_DETAIL_1 PD,V_FinishedItemDetail vf,
                    Item_Master IM,CompanyInfo CI,Unit U Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=vf.Item_Finished_id And 
                    IM.Item_Id=vf.Item_Id And  PM.Companyid=CI.CompanyId  And PM.UnitId=U.UnitId And QualityType<>3 and 
                    PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And PM.CompanyId=1";

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
            ////Add Table 
            qry = @"select Distinct EI.EmpId,EI.Empname,EMP.IssueOrderId,PM.Process_Rec_Id,UI.UnitName,ReceiveDate from process_Receive_Master_1 PM,Process_Receive_Detail_1 PD,Employee_ProcessOrderNo EMP,Empinfo EI,Process_issue_Master_1 PIM,Units UI
                  Where PM.Process_Rec_id=PD.Process_Rec_Id And Emp.IssueOrderId=PD.issueOrderId And EI.Empid=EMP.EmpId And ProcessId=1 And UI.unitsId=PIM.Units And PIM.IssueOrderId=PD.IssueOrderId And PM.Process_Rec_id=" + ViewState["Process_Rec_Id"] + "";
            SqlDataAdapter ad = new SqlDataAdapter(qry, con);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            ds.Tables.Add(dt);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptBazarSlip.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptBazarSlip.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
        }
        catch (Exception)
        {
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
}