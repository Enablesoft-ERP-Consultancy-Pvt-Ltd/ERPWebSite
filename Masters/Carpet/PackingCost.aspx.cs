using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Carpet_PackingCost : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
           
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                con.Open();
                lblGSMC1.Text = "0";
                lblArea.Text = "0";
                lblGSMC2.Text = "0";
                TxtPCS.Text = "1";
                if (Convert.ToInt16(Session["varCompanyId"]) == 2)
                {
                    TxtWaste1.Text = "2";
                    TxtWaste2.Text = "1";
                    TxtCraft.Text = "1";
                    TxtGSM1.Text = "100";
                    TxtGSM2.Text = "150";
                    TxtRate1.Text = "35";
                    TxtRate2.Text = "45";
                    if (Request.QueryString["PackingType"] == "3")
                    {
                        TxtPly.Text = "6";
                    }
                    else
                    {
                        TxtPly.Text = "4";
                    }
                    PlyCraftGS1Changed();
                    PlyCraftGS1_Texthange();
                    
                }

                FILLGRID();
                TxtPackingType.Text = Request.QueryString["PackingType"];
                TxtAmount.Text = SqlHelper.ExecuteScalar(con, CommandType.Text, "SELECT IsNull(Sum(NetCost),0) FROM PACKINGCOST PC,ITEM_PARAMETER_MASTER IPM WHERE PC.FINISHEDID=IPM.ITEM_FINISHED_ID AND PACKINGTYPE=" + Request.QueryString["PackingType"] + " AND IPM.PRODUCTCODE='" + Request.QueryString["itemcode"] + "' And IPM.MasterCompanyId=" + Session["varCompanyId"] + "").ToString();
                if (Request.QueryString["itemcode"] != "")
                {
                    tdlblProdCode.Visible = true;
                    tdddProdCode.Visible = true;
                    int VarFinishedid = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "SELECT ITEM_FINISHED_ID FROM ITEM_PARAMETER_MASTER WHERE PRODUCTCODE='" + Request.QueryString["itemcode"] + "' And MasterCompanyId=" + Session["varCompanyId"] + ""));
                    UtilityModule.ConditionalComboFill(ref ddProdCode, "SELECT DISTINCT PD.IFINISHEDID,PRODUCTCODE from PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD,ITEM_PARAMETER_MASTER IPM WHERE PM.PCMID=PD.PCMID AND IPM.ITEM_FINISHED_ID=PD.IFINISHEDID AND PM.FINISHEDID IN (" + VarFinishedid + ") And IPM.MasterCompanyId=" + Session["varCompanyId"] + " ORDER BY PRODUCTCODE", true, "--SELECT--");
                    ddProdCode.SelectedValue = VarFinishedid.ToString();
                }             
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Carpet/PackingCost.aspx");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }
    }
    protected void TxtLength_TextChanged(object sender, EventArgs e)
    {
        if (TxtLength.Text != "" && TxtWidth.Text != "" && TxtHeight.Text != "" && TxtWaste1.Text != "")
        {
            lblArea.Text = (fnGetArea(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToDouble(TxtHeight.Text), Convert.ToDouble(TxtWaste1.Text))).ToString();
        }
        TxtWidth.Focus();
    }
    protected void TxtWidth_TextChanged(object sender, EventArgs e)
    {
        if (TxtLength.Text != "" && TxtWidth.Text != "" && TxtHeight.Text != "" && TxtWaste1.Text != "")
        {
            lblArea.Text = (fnGetArea(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text==""?"0":TxtWidth.Text), Convert.ToDouble(TxtHeight.Text), Convert.ToDouble(TxtWaste1.Text))).ToString();
        }
        TxtHeight.Focus();
    }
    protected void TxtHeight_TextChanged(object sender, EventArgs e)
    {
        if (TxtLength.Text != "" && TxtWidth.Text != "" && TxtHeight.Text != "" && TxtWaste1.Text != "")
        {
            lblArea.Text = (fnGetArea(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToDouble(TxtHeight.Text), Convert.ToDouble(TxtWaste1.Text))).ToString();
        }
        TxtWaste1.Focus();
    }
            
    protected void TxtWaste1_TextChanged(object sender, EventArgs e)
    {
        if (TxtLength.Text != "" && TxtWidth.Text != "" && TxtHeight.Text != "" && TxtWaste1.Text != "")
        {
            lblArea.Text = (fnGetArea(Convert.ToDouble(TxtLength .Text), Convert.ToDouble(TxtWidth .Text), Convert.ToDouble(TxtHeight .Text), Convert.ToDouble(TxtWaste1.Text))).ToString();
        }
        TxtWaste2.Text = "";
        TxtWaste2.Focus();
    }
    private double fnGetArea(double varLength, double varWidth, double varHeight, double varWaste)
        {
        double varArea=0;
        double varArea1= 0;
        double varL1=0;
        if (TxtWaste2.Text == "")
        {
            TxtWaste2.Text = "0";
        }
        double varWaste1=Convert.ToDouble(TxtWaste2.Text);
        if (DDunit.SelectedItem.Text.Trim() == "Cms")
        {
            varArea = varArea / 10000;
            varLength = Math.Round(varLength / 2.54, 2);
            varWidth = Math.Round(varWidth / 2.54, 2);
            varHeight = Math.Round(varHeight / 2.54, 2);
        }
        if (varLength != 0 || varWidth != 0 || varHeight != 0)
        {
            varL1= ((varLength + varWidth) + varWaste);
           // varL1 = (((varLength + varWidth) *2 ) + varWaste);
            varL1 = Math.Round(varL1, 0);
            if(varL1%2 != 0 )
            {
                varL1 = varL1 + 1;
            }
           // varArea = varL1
            varArea = (varL1 +2) * 2  ;
            varL1 = ((varWidth + varHeight) + varWaste1);
            varL1 = Math.Round(varL1, 0);
            if( varL1%2 != 0 )
            {
                varL1 = varL1 + 1;
            }
            varArea1 = varL1 + 2;
            varArea = varArea * varArea1;
            //varArea = varArea * varArea1 * 2;
        }       
        varArea = Math.Round(varArea / 1550, 2);
        return varArea;
    }
    private double fnGetGSM(double varPly, double varCraft, double varGSM)
    {
        double varC;
        double varCurveQty=0.0;
        double varCurve = 0.0;
        double varNorm = 0.0;
        if (TxtGSM2.Text == "")
        {
            TxtGSM2.Text = "0";
        }
        if (Convert.ToInt32(TxtGSM2.Text) == 0)
        {
            if (varCraft == 1.0)
            {
                varC = varGSM + varGSM * 0.5;
            }
            else if (varCraft == 2.0)
            {
                varC = (varGSM + varGSM * 0.5) * 2;
            }
            else
            {
                varC = varGSM;
            }
        }
        else
        {
            varC = Convert.ToInt32(TxtGSM2.Text) * varCraft;
        }
        
        lblGSMC2.Text = varC.ToString () ;
        if (varPly / 2 != 0)
        {
            varCurveQty = Math.Round ((varPly) / 2,0);
            varCurve = varCurveQty * (varGSM + varGSM * 0.4);
        }
        varNorm = varCurve +(varPly - varCurveQty) * varGSM;       
        return varNorm;
    }
    protected void TxtPly_TextChanged(object sender, EventArgs e)
    {
        PlyCraftGS1_Texthange();
         PlyCraftGS1Changed();
        TxtCraft.Focus();
    }
    protected void TxtCraft_TextChanged(object sender, EventArgs e)
    {
        PlyCraftGS1_Texthange();
        TxtGSM1.Focus();
    }
    protected void TxtGSM1_TextChanged(object sender, EventArgs e)
    {
        PlyCraftGS1_Texthange();
        TxtGSM2.Text = "";
        TxtGSM2.Focus();
    }
    private void PlyCraftGS1Changed()
    {
        double Varply, VarCraft, VarGSM1;
        if (TxtPly.Text == "")
        {
            Varply = 0.0;
        }
        else
        {
            Varply = Convert.ToDouble(TxtPly.Text);
        }
        if (TxtCraft.Text == "")
        {
            VarCraft = 0.0;
        }
        else
        {
            VarCraft = Convert.ToDouble(TxtCraft.Text);
        }
        if (TxtGSM1.Text == "")
        {
            VarGSM1 = 0.0;
        }
        else
        {
            VarGSM1 = Convert.ToDouble(TxtGSM1.Text);
        }
        lblGSMC1.Text = (fnGetGSM(Varply, VarCraft, VarGSM1)).ToString();
    }
    private void PlyCraftGS1_Texthange()
    {
        PlyCraftGS1Changed();
    }
    protected void BtnCalculate_Click(object sender, EventArgs e)
    {
        double varNetCost=0.0;
        if (TxtRate1.Text == "")
        {
            TxtRate1.Text = "0";
        }
        if (TxtRate2.Text == "")
        {
            TxtRate2.Text = "0";
        }
        
        TxtFrtAmt.Text= fnNetCost(Convert.ToDouble(TxtLength.Text==""?"0":TxtLength.Text), Convert.ToDouble(TxtWidth.Text==""?"0":TxtWidth.Text), Convert.ToDouble(TxtHeight.Text), Convert.ToDouble(TxtPCS.Text), 55, 65000).ToString();
        varNetCost = (Convert.ToDouble(lblGSMC1.Text) * Convert.ToDouble(lblArea.Text) * Convert.ToDouble(TxtRate1.Text) / 1000 + Convert.ToDouble(lblGSMC2.Text) * Convert.ToDouble(lblArea.Text) * Convert.ToDouble(TxtRate2.Text) / 1000);
        varNetCost = varNetCost / Convert.ToDouble(TxtPCS.Text);
        TxtNetcost.Text = string.Format("{0:#0.00}", varNetCost);
    }
    private double fnNetCost(double varLength, double varWidth, double varHeight, double varPcs, double varConCBM, double varConCost)
    {
        double varArea = 0, varPcsCBM = 0, varNoOfPcs = 0, varNetCost = 0;
        varArea = varLength * varWidth * varHeight / 1000000;
        varPcsCBM = varArea / varPcs;
        varNoOfPcs = varConCBM / varPcsCBM;
        varNetCost = varConCost / varNoOfPcs;
        varArea = Math.Round(varNetCost, 2);
        return varArea;
    }
    protected void TxtWaste2_TextChanged(object sender, EventArgs e)
    {
        if (TxtLength.Text != "" && TxtWidth.Text != "" && TxtHeight.Text != "" && TxtWaste1.Text != "")
        {
            lblArea.Text = (fnGetArea(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToDouble(TxtHeight.Text), Convert.ToDouble(TxtWaste1.Text))).ToString();
        }
        TxtPly.Focus();
    }
    protected void TxtGSM2_TextChanged(object sender, EventArgs e)
    {
        TxtRate1.Focus();
    }
    protected void TxtRate2_TextChanged(object sender, EventArgs e)
    {
        TxtPCS.Focus();

    }
    protected void TxtRate1_TextChanged(object sender, EventArgs e)
    {
        TxtRate2.Focus();
    }
    protected void TxtPCS_TextChanged(object sender, EventArgs e)
    {
        BtnCalculate.Focus();
    }
    private void FILLGRID()
    {
        DG.DataSource = Fill_Grid_Data();
        DG.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {

            string strsql = @"SELECT Length,Width,Height,Waste1,Waste2,Ply,Craft,GSM1,GSM2,Rate1,Rate2,Pcs,NetCost,ID Sr_No FROM PACKINGCOST PC,ITEM_PARAMETER_MASTER IPM WHERE PC.FINISHEDID=IPM.ITEM_FINISHED_ID AND PACKINGTYPE=" + Request.QueryString["PackingType"] + " AND IPM.PRODUCTCODE='" + Request.QueryString["itemcode"] + "' And IPM.MasterCompanyId=" + Session["varCompanyId"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/PackingCost.aspx");
            Logs.WriteErrorLog("Charge|Fill_Grid_Data|" + ex.Message);
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
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (lblMessage.Text == "")
        {
         SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
         con.Open();
         SqlTransaction Tran = con.BeginTransaction();
         try
         {
            SqlParameter[] _arrpara = new SqlParameter[20];
            _arrpara[0] = new SqlParameter("@PACKINGTYPE", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@PRODCODE", SqlDbType.NVarChar, 50);
            _arrpara[2] = new SqlParameter("@UNITID", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@LENGTH", SqlDbType.Float);
            _arrpara[4] = new SqlParameter("@WIDTH", SqlDbType.Float);
            _arrpara[5] = new SqlParameter("@HEIGHT", SqlDbType.Float);
            _arrpara[6] = new SqlParameter("@WASTE1", SqlDbType.Float);
            _arrpara[7] = new SqlParameter("@WASTE2", SqlDbType.Float);
            _arrpara[8] = new SqlParameter("@PLY", SqlDbType.Float);
            _arrpara[9] = new SqlParameter("@CRAFT", SqlDbType.Float);
            _arrpara[10] = new SqlParameter("@GSM1", SqlDbType.Float);
            _arrpara[11] = new SqlParameter("@GSM2", SqlDbType.Float);
            _arrpara[12] = new SqlParameter("@RATE1", SqlDbType.Float);
            _arrpara[13] = new SqlParameter("@RATE2", SqlDbType.Float);
            _arrpara[14] = new SqlParameter("@NETCOST", SqlDbType.Float);
            _arrpara[15] = new SqlParameter("@varuserid", SqlDbType.Int);
            _arrpara[16] = new SqlParameter("@varCompanyId", SqlDbType.Int);
            _arrpara[17] = new SqlParameter("@PCS", SqlDbType.Int);
            _arrpara[18] = new SqlParameter("@ID", SqlDbType.Int);
            _arrpara[19] = new SqlParameter("@PROD_FINISHEDID", SqlDbType.Int);

            _arrpara[0].Value = Request.QueryString["PackingType"];
            _arrpara[1].Value = Request.QueryString["itemcode"];
            _arrpara[2].Value = DDunit.SelectedIndex;
            _arrpara[3].Value = TxtLength.Text;
            _arrpara[4].Value = TxtWidth.Text;
            _arrpara[5].Value = TxtHeight.Text;
            _arrpara[6].Value = TxtWaste1.Text;
            _arrpara[7].Value = TxtWaste2.Text;
            _arrpara[8].Value = TxtPly.Text;
            _arrpara[9].Value = TxtCraft.Text;
            _arrpara[10].Value = TxtGSM1.Text;
            _arrpara[11].Value = TxtGSM2.Text;
            _arrpara[12].Value = TxtRate1.Text;
            _arrpara[13].Value = TxtRate2.Text;
            _arrpara[14].Value = TxtNetcost.Text;
            _arrpara[15].Value = Session["varuserid"];
            _arrpara[16].Value = Session["varCompanyId"];
            _arrpara[17].Value = TxtPCS.Text;
            _arrpara[18].Value = 0;
            if (BtnSave.Text == "UpDate")
            {
                _arrpara[18].Value = DG.SelectedDataKey.Value;
            }
            if (tdddProdCode.Visible == true)
            {
                _arrpara[19].Value = ddProdCode.SelectedValue;
            }
            else
            {
                _arrpara[19].Value = 0;
            }
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[dbo].[PRO_PACKINGCOST]", _arrpara);
            if (Convert.ToInt32(Request.QueryString["PackingType"]) == 3)
            {
                Insert_Into_Container_Cost(Tran);
            }
            TxtAmount.Text = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "SELECT IsNull(Sum(NetCost),0) FROM PACKINGCOST PC,ITEM_PARAMETER_MASTER IPM WHERE PC.FINISHEDID=IPM.ITEM_FINISHED_ID AND PACKINGTYPE=" + Request.QueryString["PackingType"] + " AND IPM.PRODUCTCODE='" + Request.QueryString["itemcode"] + "' And IPM.MasterCompanyId=" + Session["varCompanyId"] + "").ToString();
            Tran.Commit();
            ClearData();
         }
         catch (Exception ex)
         {
             UtilityModule.MessageAlert(ex.Message, "Master/Carpet/PackingCost.aspx");
            Tran.Rollback();
         }
         finally
         { 
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
         }
       }
    }
    private void ClearData()
    {
        TxtLength.Text = "";
        TxtWidth.Text = "";
        TxtHeight.Text = "";
        //TxtWaste1.Text = "";
        //TxtWaste2.Text = "";
        //TxtPly.Text = "";
        //TxtCraft.Text = "";
        //TxtGSM1.Text = "";
        //TxtGSM2.Text = "";
        //TxtRate1.Text = "";
        //TxtRate2.Text = "";
        TxtPCS.Text = "";
        //TxtNetcost.Text = "";
        BtnSave.Text = "Save";
        FILLGRID();
    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (UtilityModule.VALIDTEXTBOX(TxtLength) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtWidth) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtHeight) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
    }
    private void Insert_Into_Container_Cost(SqlTransaction Tran)
    {
        SqlParameter[] _arrpara = new SqlParameter[12];
        _arrpara[0] = new SqlParameter("@PRODCODE", SqlDbType.NVarChar, 50);
        _arrpara[1] = new SqlParameter("@LENGTH", SqlDbType.Float);
        _arrpara[2] = new SqlParameter("@WIDTH", SqlDbType.Float);
        _arrpara[3] = new SqlParameter("@HEIGHT", SqlDbType.Float);
        _arrpara[4] = new SqlParameter("@PCS", SqlDbType.Float);
        _arrpara[5] = new SqlParameter("@CONTAINERCBM", SqlDbType.Float);
        _arrpara[6] = new SqlParameter("@CONTAINERCOST", SqlDbType.Float);
        _arrpara[7] = new SqlParameter("@NETCOST", SqlDbType.Float);
        _arrpara[8] = new SqlParameter("@varuserid", SqlDbType.Int);
        _arrpara[9] = new SqlParameter("@varCompanyId", SqlDbType.Int);
        _arrpara[10] = new SqlParameter("@DraftOrderDetailId", SqlDbType.Int);
        _arrpara[11] = new SqlParameter("@UnitId", SqlDbType.Int);

        _arrpara[0].Value = Request.QueryString["itemcode"];
        _arrpara[1].Value = TxtLength.Text;
        _arrpara[2].Value = TxtWidth.Text;
        _arrpara[3].Value = TxtHeight.Text;
        _arrpara[4].Value = TxtPCS.Text;
        _arrpara[5].Value = 55;
        _arrpara[6].Value = 65000;
        _arrpara[7].Value = fnNetCost(Convert.ToDouble(TxtLength.Text==""?"0":TxtLength.Text), Convert.ToDouble(TxtWidth.Text==""?"0":TxtWidth.Text), Convert.ToDouble(TxtHeight.Text), Convert.ToDouble(TxtPCS.Text), 55, 65000);
        _arrpara[8].Value = Session["varuserid"];
        _arrpara[9].Value = Session["varCompanyId"];
        _arrpara[10].Value = 0;
        _arrpara[11].Value = DDunit.SelectedIndex;

        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[dbo].[PRO_CONTAINERCOST]", _arrpara);
        TxtFrtAmt.Text = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "SELECT NetCost FROM CONTAINERCOST PC,ITEM_PARAMETER_MASTER IPM WHERE PC.FINISHEDID=IPM.ITEM_FINISHED_ID AND IPM.PRODUCTCODE='" + Request.QueryString["itemcode"] + "' And IPM.MasterCompanyId=" + Session["varCompanyId"] + "").ToString();
        TxtAmount.Text = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "SELECT IsNull(Sum(NetCost),0) FROM PACKINGCOST PC,ITEM_PARAMETER_MASTER IPM WHERE PC.FINISHEDID=IPM.ITEM_FINISHED_ID AND PACKINGTYPE=" + Request.QueryString["PackingType"] + " AND IPM.PRODUCTCODE='" + Request.QueryString["itemcode"] + "' And IPM.MasterCompanyId=" + Session["varCompanyId"] + "").ToString();
    }
    protected void DG_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT * FROM PACKINGCOST PC,ITEM_PARAMETER_MASTER IPM WHERE PC.FINISHEDID=IPM.ITEM_FINISHED_ID AND ID=" + DG.SelectedDataKey.Value + " And IPM.MasterCompanyId=" + Session["varCompanyId"] + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                DDunit.SelectedIndex = Convert.ToInt32(Ds.Tables[0].Rows[0]["UnitId"]);
                if (tdddProdCode.Visible == true)
                {
                    ddProdCode.SelectedValue = Ds.Tables[0].Rows[0]["PRODCODE_FINISHEDID"].ToString();
                }
                TxtLength.Text = Ds.Tables[0].Rows[0]["length"].ToString();
                TxtWidth.Text = Ds.Tables[0].Rows[0]["Width"].ToString();
                TxtHeight.Text = Ds.Tables[0].Rows[0]["Height"].ToString();
                TxtWaste1.Text = Ds.Tables[0].Rows[0]["Waste1"].ToString();
                lblArea.Text = (fnGetArea(Convert.ToDouble(TxtLength.Text==""?"0":TxtLength.Text), Convert.ToDouble(TxtWidth.Text==""?"0":TxtWidth.Text), Convert.ToDouble(TxtHeight.Text), Convert.ToDouble(TxtWaste1.Text))).ToString();
                TxtWaste2.Text = Ds.Tables[0].Rows[0]["Waste2"].ToString();
                TxtPly.Text = Ds.Tables[0].Rows[0]["Ply"].ToString();
                TxtCraft.Text = Ds.Tables[0].Rows[0]["Craft"].ToString();
                TxtGSM1.Text = Ds.Tables[0].Rows[0]["GSM1"].ToString();
                TxtGSM2.Text = Ds.Tables[0].Rows[0]["GSM2"].ToString();
                TxtRate1.Text = Ds.Tables[0].Rows[0]["Rate1"].ToString();
                TxtRate2.Text = Ds.Tables[0].Rows[0]["Rate2"].ToString();
                TxtPCS.Text = Ds.Tables[0].Rows[0]["PCS"].ToString();
                TxtNetcost.Text = Ds.Tables[0].Rows[0]["NetCost"].ToString();
                PlyCraftGS1_Texthange();
                if (Convert.ToInt32(Request.QueryString["PackingType"]) == 3)
                {
                    TxtFrtAmt.Text = SqlHelper.ExecuteScalar(con, CommandType.Text, "SELECT NetCost FROM CONTAINERCOST PC,ITEM_PARAMETER_MASTER IPM WHERE PC.FINISHEDID=IPM.ITEM_FINISHED_ID AND IPM.PRODUCTCODE='" + Request.QueryString["itemcode"] + "' And IPM.MasterCompanyId=" + Session["varCompanyId"] + "").ToString();
                }
                BtnSave.Text = "UpDate";
                btnDelete.Visible = true;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/PackingCost.aspx");

        }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                    con.Dispose();
                }
            }
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DG, "select$" + e.Row.RowIndex);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            lblMessage.Text = "";
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, "DELETE PACKINGCOST WHERE ID=" + DG.SelectedDataKey.Value);
            DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
            SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'PACKINGCOST'," + DG.SelectedDataKey.Value + ",getdate(),'Delete')");
            ClearData();
            btnDelete.Visible = false;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/PackingCost.aspx");
            lblMessage.Visible = true;
            lblMessage.Text = "Error In Deleting";
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DG_RowCreated(object sender, GridViewRowEventArgs e)
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
}