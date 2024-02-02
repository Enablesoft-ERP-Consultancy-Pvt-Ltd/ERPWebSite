using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Carpet_ContainerCost : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            // Request.QueryString["itemcode"];
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                TxtLength.Focus();
                con.Open();
                //int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
                txtVarCompanyNo.Text = Session["varMasterCompanyIDForERP"].ToString();
                switch (Convert.ToInt16(Session["varMasterCompanyIDForERP"]))
                {
                case 2:
                        TxtPackingType.Text = Request.QueryString["PackingType"];
                        DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT * FROM CONTAINERCOST PC,ITEM_PARAMETER_MASTER IPM WHERE PC.FINISHEDID=IPM.ITEM_FINISHED_ID And IPM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " AND IPM.PRODUCTCODE='" + Request.QueryString["itemcode"] + "'");
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            TxtLength.Text = Ds.Tables[0].Rows[0]["length"].ToString();
                            TxtWidth.Text = Ds.Tables[0].Rows[0]["Width"].ToString();
                            TxtHeight.Text = Ds.Tables[0].Rows[0]["Height"].ToString();
                            lblBoxCBM.Text = (fnGetArea(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToDouble(TxtHeight.Text))).ToString();
                            TxtPCS.Text = Ds.Tables[0].Rows[0]["PCS"].ToString();
                            lblPcsCBM.Text = (fnGetArea(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToDouble(TxtHeight.Text), Convert.ToDouble(TxtPCS.Text))).ToString();
                            TxtContainerCBM.Text = Ds.Tables[0].Rows[0]["CONTAINERCBM"].ToString();
                            lblNoOFPCS.Text = Math.Round((Convert.ToDouble(TxtContainerCBM.Text) / Convert.ToDouble(lblPcsCBM.Text)), 3).ToString();
                            TxtContainerCost.Text = Ds.Tables[0].Rows[0]["CONTAINERCOST"].ToString();
                            TxtNetcost.Text = Ds.Tables[0].Rows[0]["NetCost"].ToString();
                        }
                        else
                        {
                            Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT * FROM PACKINGCOST PC,ITEM_PARAMETER_MASTER IPM WHERE PC.FINISHEDID=IPM.ITEM_FINISHED_ID AND PACKINGtYPE=3 AND IPM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  AND IPM.PRODUCTCODE='" + Request.QueryString["itemcode"] + "'");
                            if (Ds.Tables[0].Rows.Count > 0)
                            {
                                TxtLength.Text = Ds.Tables[0].Rows[0]["length"].ToString();
                                TxtWidth.Text = Ds.Tables[0].Rows[0]["Width"].ToString();
                                TxtHeight.Text = Ds.Tables[0].Rows[0]["Height"].ToString();
                                lblBoxCBM.Text = (fnGetArea(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToDouble(TxtHeight.Text))).ToString();
                                TxtContainerCBM.Text = "55";
                                TxtContainerCost.Text = "65000";
                                TxtPCS.Focus();
                            }
                        }
                        break;
                case 3:
                        Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT * FROM CONTAINERCOST Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And DraftOrderDetailId='" + Request.QueryString["itemcode"] + "'");
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            DDunit.SelectedIndex = Convert.ToInt32(Ds.Tables[0].Rows[0]["UnitId"]);
                            TxtLength.Text = Ds.Tables[0].Rows[0]["length"].ToString();
                            TxtWidth.Text = Ds.Tables[0].Rows[0]["Width"].ToString();
                            TxtHeight.Text = Ds.Tables[0].Rows[0]["Height"].ToString();
                            lblBoxCBM.Text = (fnGetArea(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToDouble(TxtHeight.Text))).ToString();
                            TxtPCS.Text = Ds.Tables[0].Rows[0]["PCS"].ToString();
                            lblPcsCBM.Text = (fnGetArea(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToDouble(TxtHeight.Text), Convert.ToDouble(TxtPCS.Text))).ToString();
                            TxtContainerCBM.Text = Ds.Tables[0].Rows[0]["CONTAINERCBM"].ToString();
                            lblNoOFPCS.Text = Math.Round((Convert.ToDouble(TxtContainerCBM.Text) / Convert.ToDouble(lblPcsCBM.Text)), 3).ToString();
                            TxtContainerCost.Text = Ds.Tables[0].Rows[0]["CONTAINERCOST"].ToString();
                            TxtNetcost.Text = Ds.Tables[0].Rows[0]["NetCost"].ToString();
                        }
                        else
                        {
                            TxtContainerCBM.Text = "0";
                            TxtContainerCost.Text = "0";
                        }
                        break;
            }   
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ContainerCost.aspx");
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
        if (TxtLength.Text != "" && TxtWidth.Text != "" && TxtHeight.Text != "")
        {
            lblBoxCBM.Text = (fnGetArea(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToDouble(TxtHeight.Text))).ToString();
        }
        if (TxtLength.Text != "" && TxtWidth.Text != "" && TxtHeight.Text != "" && TxtPCS.Text != "" && TxtContainerCBM.Text != "" && TxtContainerCost.Text != "")
        {
            TxtNetcost.Text = (fnNetCost(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToDouble(TxtHeight.Text), Convert.ToDouble(TxtPCS.Text), Convert.ToDouble(TxtContainerCBM.Text), Convert.ToDouble(TxtContainerCost.Text))).ToString();
        }
        TxtWidth.Focus();
    }
    protected void TxtWidth_TextChanged(object sender, EventArgs e)
    {
        if (TxtLength.Text != "" && TxtWidth.Text != "" && TxtHeight.Text != "")
        {
            lblBoxCBM.Text = (fnGetArea(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToDouble(TxtHeight.Text))).ToString();
        }
        if (TxtLength.Text != "" && TxtWidth.Text != "" && TxtHeight.Text != "" && TxtPCS.Text != "" && TxtContainerCBM.Text != "" && TxtContainerCost.Text != "")
        {
            TxtNetcost.Text = (fnNetCost(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToDouble(TxtHeight.Text), Convert.ToDouble(TxtPCS.Text), Convert.ToDouble(TxtContainerCBM.Text), Convert.ToDouble(TxtContainerCost.Text))).ToString();
        }
        TxtHeight.Focus();
    }
    protected void TxtHeight_TextChanged(object sender, EventArgs e)
    {
        if (TxtLength.Text != "" && TxtWidth.Text != "" && TxtHeight.Text != "")
        {
            lblBoxCBM.Text = (fnGetArea(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToDouble(TxtHeight.Text))).ToString();
        }
        if (TxtLength.Text != "" && TxtWidth.Text != "" && TxtHeight.Text != "" && TxtPCS.Text != "" && TxtContainerCBM.Text != "" && TxtContainerCost.Text != "")
        {
            TxtNetcost.Text = (fnNetCost(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToDouble(TxtHeight.Text), Convert.ToDouble(TxtPCS.Text), Convert.ToDouble(TxtContainerCBM.Text), Convert.ToDouble(TxtContainerCost.Text))).ToString();
        }
        TxtPCS.Focus();
    }
    protected void TxtPCS_TextChanged(object sender, EventArgs e)
    {
        if (TxtLength.Text != "" && TxtWidth.Text != "" && TxtHeight.Text != "" && TxtPCS.Text != "")
        {
            lblPcsCBM.Text = (fnGetArea(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToDouble(TxtHeight.Text), Convert.ToDouble(TxtPCS.Text))).ToString();
        }
        if (TxtLength.Text != "" && TxtWidth.Text != "" && TxtHeight.Text != "" && TxtPCS.Text != "" && TxtContainerCBM.Text != "" && TxtContainerCost.Text != "")
        {
            TxtNetcost.Text = (fnNetCost(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToDouble(TxtHeight.Text), Convert.ToDouble(TxtPCS.Text), Convert.ToDouble(TxtContainerCBM.Text), Convert.ToDouble(TxtContainerCost.Text))).ToString();
        }
        TxtContainerCBM.Focus();
    }
    protected void TxtContainerCBM_TextChanged(object sender, EventArgs e)
    {
        if (TxtContainerCBM.Text != "" && lblPcsCBM.Text != "")
        {
            lblNoOFPCS.Text = Math.Round((Convert.ToDouble(TxtContainerCBM.Text) / Convert.ToDouble(lblPcsCBM.Text)), 3).ToString();
        }
        if (TxtLength.Text != "" && TxtWidth.Text != "" && TxtHeight.Text != "" && TxtPCS.Text != "" && TxtContainerCBM.Text != "" && TxtContainerCost.Text != "")
        {
            TxtNetcost.Text = (fnNetCost(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToDouble(TxtHeight.Text), Convert.ToDouble(TxtPCS.Text), Convert.ToDouble(TxtContainerCBM.Text), Convert.ToDouble(TxtContainerCost.Text))).ToString();
        }
        TxtContainerCost.Focus();
    }
    protected void TxtContainerCost_TextChanged(object sender, EventArgs e)
    {
        if (TxtContainerCBM.Text != "" && TxtPCS.Text != "" && TxtContainerCost.Text != "")
        {
            TxtNetcost.Text = Math.Round((Convert.ToDouble(TxtContainerCost.Text) / Convert.ToDouble(lblNoOFPCS.Text)), 3).ToString();
        }
        if (TxtLength.Text != "" && TxtWidth.Text != "" && TxtHeight.Text != "" && TxtPCS.Text != "" && TxtContainerCBM.Text != "" && TxtContainerCost.Text != "")
        {
            TxtNetcost.Text = (fnNetCost(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToDouble(TxtHeight.Text), Convert.ToDouble(TxtPCS.Text), Convert.ToDouble(TxtContainerCBM.Text), Convert.ToDouble(TxtContainerCost.Text))).ToString();
        }
    }
    private double fnNetCost(double varLength, double varWidth, double varHeight, double varPcs, double varConCBM, double varConCost)
    {
        double varArea = 0, varPcsCBM = 0, varNoOfPcs = 0, varNetCost = 0;
        if (DDunit.SelectedIndex ==0)
        {
            varArea = 16.387064*varLength * varWidth * varHeight / 1000000;
        }
        else
        {
            varArea = varLength * varWidth * varHeight / 1000000;
        }
        varPcsCBM = varArea / varPcs;
        varNoOfPcs = varConCBM / varPcsCBM;
        if (varNoOfPcs == 0.0)
        {
            varNetCost = 0.0;
        }
        else
        {
            varNetCost = varConCost / varNoOfPcs;
        }
        varArea = Math.Round(varNetCost, 2);
        return varArea;
    }
    private double fnGetArea(double varLength, double varWidth, double varHeight)
    {
        double varArea = 0;
        varArea = varLength * varWidth * varHeight / 1000000;
        varArea = Math.Round(varArea, 3);
        return varArea;
    }
    private double fnGetArea(double varLength, double varWidth, double varHeight, double varPcs)
    {
        double varArea = 0;
        varArea = (varLength * varWidth * varHeight) / (1000000 * varPcs);
        varArea = Math.Round(varArea, 3);
        return varArea;
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
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
            _arrpara[5].Value = TxtContainerCBM.Text;
            _arrpara[6].Value = TxtContainerCost.Text;
            _arrpara[7].Value = TxtNetcost.Text;
            _arrpara[8].Value = Session["varuserid"];
            _arrpara[9].Value = Session["varMasterCompanyIDForERP"];
            if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 2)
            {
                _arrpara[0].Value = Request.QueryString["itemcode"]; 
                _arrpara[10].Value = 0;
            }
            else
            {
                _arrpara[0].Value = 0;
                _arrpara[10].Value = Request.QueryString["itemcode"];
            }
            _arrpara[11].Value = DDunit.SelectedIndex;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[dbo].[PRO_CONTAINERCOST]", _arrpara);
            Tran.Commit();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ContainerCost.aspx");
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