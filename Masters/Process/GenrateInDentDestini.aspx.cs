using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Process_GenrateInDentDestini : System.Web.UI.Page
{
    static int MasterCompanyId;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varCompanyId"]);
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            logo();
            ViewState["IndentID"] = 0;
            ViewState["IndentDetailId"] = 0;
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtrecDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            tdindentno.Visible = false;
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + " Order By CompanyName", true, "--SelectCompany");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "SELECT DISTINCT CI.Customerid,CI.Customercode + SPACE(5)+CompanyName from CustomerInfo CI,ORDER_CONSUMPTION_DETAIL OCD,ORDERMASTER OM WHERE CI.CUSTOMERID=OM.CUSTOMERID AND OM.ORDERID=OCD.ORDERID And CI.MasterCompanyId=" + Session["varCompanyId"] + " Order By CI.Customercode + SPACE(5)+CompanyName", true, "--SELECT--");
            ParameteLabel();
            int VarProdCode = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarProdCode From MasterSetting"));
            switch (VarProdCode)
            {
                case 1:
                    procode.Visible = true;
                    TdFinish_Type.Visible = true;
                    break;
                case 0:
                    procode.Visible = false;
                    TdFinish_Type.Visible = false;
                    break;
            }
            DDCustomerCode.Focus();
        }
    }
    private void ParameteLabel()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        LblQuality.Text = ParameterList[0];
        LblDesign.Text = ParameterList[1];
        LblColor.Text = ParameterList[2];
        LblShape.Text = ParameterList[3];
        LblSize.Text = ParameterList[4];
        LblCategory.Text = ParameterList[5];
        LblItemName.Text = ParameterList[6];
        LblColorShade.Text = ParameterList[7];
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "SELECT DISTINCT CI.Customerid,CI.Customercode + SPACE(5)+CompanyName from CustomerInfo CI,ORDER_CONSUMPTION_DETAIL OCD,ORDERMASTER OM WHERE CI.MasterCompanyId=" + Session["varCompanyId"] + " And CI.CUSTOMERID=OM.CUSTOMERID AND OM.ORDERID=OCD.ORDERID and ocd.orderid in(select orderid from indentdetail where indentid in(select indentid from indentmaster where  companyid=" + DDCompanyName.SelectedValue + "))  Order BY CI.Customercode + SPACE(5)+CompanyName", true, "--SELECT--");
        else
            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "SELECT DISTINCT CI.Customerid,CI.Customercode + SPACE(5)+CompanyName from CustomerInfo CI,ORDER_CONSUMPTION_DETAIL OCD,ORDERMASTER OM WHERE CI.MasterCompanyId=" + Session["varCompanyId"] + " And  CI.CUSTOMERID=OM.CUSTOMERID AND OM.ORDERID=OCD.ORDERID And OM.CompanyID = " + DDCompanyName.SelectedValue + " Order by CI.Customercode + SPACE(5)+CompanyName", true, "--SELECT--");
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
            UtilityModule.ConditionalComboFill(ref ddOrderNo, "Select Distinct OM.OrderId,OM.LocalOrder+' / '+OM.CustomerOrderNo FROM OrderMaster OM,ORDER_CONSUMPTION_DETAIL OCD,indentdetail id WHERE OM.ORDERID=OCD.ORDERID and id.orderid=om.orderid AND OM.CUSTOMERID=" + DDCustomerCode.SelectedValue + " And OM.CompanyID = " + DDCompanyName.SelectedValue + "  Order By OM.LocalOrder+' / '+OM.CustomerOrderNo", true, "--Select--");
        else
            UtilityModule.ConditionalComboFill(ref ddOrderNo, "Select Distinct OM.OrderId,OM.LocalOrder+' / '+OM.CustomerOrderNo FROM OrderMaster OM,ORDER_CONSUMPTION_DETAIL OCD WHERE OM.ORDERID=OCD.ORDERID AND OM.CUSTOMERID=" + DDCustomerCode.SelectedValue + " And OM.CompanyID = " + DDCompanyName.SelectedValue + " Order By OM.LocalOrder+' / '+OM.CustomerOrderNo", true, "--Select--");
        ddOrderNo.Focus();
    }
    protected void ddOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
            UtilityModule.ConditionalComboFill(ref DDProcessName, "Select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER PNM,ORDER_CONSUMPTION_DETAIL OCD,indentmaster im,indentdetail id WHERE PNM.PROCESS_NAME_ID=OCD.PROCESSID and im.indentid=id.indentid and id.orderid=ocd.orderid AND id.ORDERID=" + ddOrderNo.SelectedValue + " And PNM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
        else
            UtilityModule.ConditionalComboFill(ref DDProcessName, "Select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER PNM,ORDER_CONSUMPTION_DETAIL OCD WHERE PNM.PROCESS_NAME_ID=OCD.PROCESSID AND ORDERID=" + ddOrderNo.SelectedValue + " And PNM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
        DDProcessName.Focus();
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDPartyName, "select distinct EI.EmpId,EmpName from indentmaster im ,indentdetail id,EmpInfo EI where im.indentid=id.indentid AND EI.EMPID=IM.PARTYID and id.orderid=" + ddOrderNo.SelectedValue + " and processid=" + DDProcessName.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"] + " And im.CompanyId=" + DDCompanyName.SelectedValue + " Order By EmpName", true, "--Select--");
        }
        else
            UtilityModule.ConditionalComboFill(ref DDPartyName, "select EI.EmpId,EmpName from EmpInfo EI inner join EmpProcess EP on EI.EmpId=EP.EmpId where processId=" + DDProcessName.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"] + " Order By EmpName", true, "--Select--");
        Fill_Grid_Show();
        DDPartyName.Focus();
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddindentno, "select distinct im.indentid,indentno from indentmaster im,indentdetail id where im.indentid=id.indentid and processid=" + DDProcessName.SelectedValue + " and partyid=" + DDPartyName.SelectedValue + " and orderid=" + ddOrderNo.SelectedValue + " And im.CompanyId=" + DDCompanyName.SelectedValue, true, "--Select--");
            string strcategory = "SELECT DISTINCT IPM.CATEGORY_ID,IPM.CATEGORY_NAME FROM ORDER_CONSUMPTION_DETAIL OCD,V_FINISHEDITEMDETAIL VF,ITEM_CATEGORY_MASTER IPM WHERE OCD.OFINISHEDID=VF.ITEM_FINISHED_ID AND VF.CATEGORY_ID=IPM.CATEGORY_ID AND OCD.PROCESSID=" + DDProcessName.SelectedValue + " AND ORDERID=" + ddOrderNo.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            UtilityModule.ConditionalComboFill(ref DDCategory, strcategory, true, "--Select Category--");
        }
        //string strcategory = "SELECT DISTINCT IPM.CATEGORY_ID,IPM.CATEGORY_NAME FROM ORDER_CONSUMPTION_DETAIL OCD,V_FINISHEDITEMDETAIL VF,ITEM_CATEGORY_MASTER IPM WHERE OCD.OFINISHEDID=VF.ITEM_FINISHED_ID AND VF.CATEGORY_ID=IPM.CATEGORY_ID AND OCD.PROCESSID=" + DDProcessName.SelectedValue + " AND ORDERID=" + ddOrderNo.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        //UtilityModule.ConditionalComboFill(ref DDCategory, strcategory, true, "--Select Category--");
        TxtDate.Focus();
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
        string stritem = "SELECT DISTINCT IM.ITEM_ID,IM.ITEM_NAME FROM ORDER_CONSUMPTION_DETAIL OCD,V_FINISHEDITEMDETAIL VF,ITEM_MASTER IM WHERE OCD.OFINISHEDID=VF.ITEM_FINISHED_ID AND VF.ITEM_ID=IM.ITEM_ID AND OCD.PROCESSID=" + DDProcessName.SelectedValue + " AND ORDERID=" + ddOrderNo.SelectedValue + " AND VF.CATEGORY_ID=" + DDCategory.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        UtilityModule.ConditionalComboFill(ref DDItem, stritem, true, "---Select Item----");
    }
    private void ddlcategorycange()
    {
        DDQuality.Items.Clear();
        DDDesign.Items.Clear();
        DDColor.Items.Clear();
        DDShape.Items.Clear();
        DDSize.Items.Clear();
        DDColorShade.Items.Clear();
        TdQuality.Visible = false;
        TdDesign.Visible = false;
        TdColor.Visible = false;
        TdColorShade.Visible = false;
        TdShape.Visible = false;
        TdSize.Visible = false;
        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME 
                      FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on 
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        TdQuality.Visible = true;
                        break;
                    case "2":
                        TdDesign.Visible = true;
                        break;
                    case "3":
                        TdColor.Visible = true;
                        break;
                    case "6":
                        TdColorShade.Visible = true;
                        break;
                    case "4":
                        TdShape.Visible = true;
                        break;
                    case "5":
                        TdSize.Visible = true;
                        break;
                }
            }
        }
    }
    protected void DDItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strquality = "SELECT DISTINCT Q.QUALITYID,Q.QUALITYNAME FROM ORDER_CONSUMPTION_DETAIL OCD,V_FINISHEDITEMDETAIL VF,QUALITY Q  WHERE OCD.OFINISHEDID=VF.ITEM_FINISHED_ID AND VF.QUALITYID=Q.QUALITYID AND OCD.PROCESSID=" + DDProcessName.SelectedValue + " AND ORDERID=" + ddOrderNo.SelectedValue + " AND VF.ITEM_ID=" + DDItem.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        UtilityModule.ConditionalComboFill(ref DDQuality, strquality, true, "--Select Quality--");
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Quantity();
        ComboFill();
    }
    private void Fill_Quantity()
    {
        int color = 0;
        int quality = 0;
        int design = 0;
        int shape = 0;
        int size = 0;
        int shadeColor = 0;
        if ((TdQuality.Visible == true && DDQuality.SelectedIndex > 0) || TdQuality.Visible != true)
        {
            quality = 1;
        }
        if (TdDesign.Visible == true && DDDesign.SelectedIndex > 0 || TdDesign.Visible != true)
        {
            design = 1;
        }
        if (TdColor.Visible == true && DDColor.SelectedIndex > 0 || TdColor.Visible != true)
        {
            color = 1;
        }
        if (TdShape.Visible == true && DDShape.SelectedIndex > 0 || TdShape.Visible != true)
        {
            shape = 1;
        }
        if (TdSize.Visible == true && DDSize.SelectedIndex > 0 || TdSize.Visible != true)
        {
            size = 1;
        }
        if (TdColorShade.Visible == true && DDColorShade.SelectedIndex > 0 || TdColorShade.Visible != true)
        {
            shadeColor = 1;
        }
        if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                int finishedid = Convert.ToInt32(UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, Tran, DDColorShade, "", Convert.ToInt32(Session["varCompanyId"])));
                if (finishedid > 0)
                {
                    UtilityModule.ConditionalComboFill(ref ddFinish_Type, @"SELECT DISTINCT OCD.O_FINISHED_TYPE_ID,FT.FINISHED_TYPE_NAME from ORDER_CONSUMPTION_DETAIL OCD,FINISHED_TYPE FT 
                    Where OCD.O_FINISHED_TYPE_ID=FT.ID AND ORDERID=" + ddOrderNo.SelectedValue + " AND OFINISHEDID=" + finishedid + " AND PROCESSID=" + DDProcessName.SelectedValue + " ORDER BY FT.FINISHED_TYPE_NAME ", true, "Finish Type");
                    if (ddFinish_Type.Items.Count > 0)
                    {
                        ddFinish_Type.SelectedIndex = 1;
                    }
                    int VARFINISH_TYPE = 0;
                    VARFINISH_TYPE = TdFinish_Type.Visible == true ? Convert.ToInt32(ddFinish_Type.SelectedValue) : 0;
                    UtilityModule.ConditionalComboFill(ref ddUnit, "SELECT DISTINCT U.UNITID,U.UNITNAME FROM ORDER_CONSUMPTION_DETAIL OCD,V_FINISHEDITEMDETAIL VF,UNIT U WHERE OCD.OFINISHEDID=VF.ITEM_FINISHED_ID AND U.UNITID=OCD.OUNITID AND OCD.PROCESSID=" + DDProcessName.SelectedValue + " AND ORDERID=" + ddOrderNo.SelectedValue + " AND VF.ITEM_ID=" + DDItem.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "", true, " Select ");
                    if (ddUnit.Items.Count > 0)
                    {
                        ddUnit.SelectedIndex = 1;
                    }
                    UtilityModule.ConditionalComboFill(ref ddllotno, @"SELECT DISTINCT STOCKID,LOTNO FROM STOCK WHERE ITEM_FINISHED_ID IN (Select IFINISHEDID from ORDER_CONSUMPTION_DETAIL Where OrderId=" + ddOrderNo.SelectedValue + @" And ProcessId=" + DDProcessName.SelectedValue + @" And 
                    OFinishedid=" + finishedid + ") and COMPANYID=" + DDCompanyName.SelectedValue + " AND FINISHED_TYPE_ID IN (SELECT I_FINISHED_TYPE_ID FROM ORDER_CONSUMPTION_DETAIL WHERE ORDERID=" + ddOrderNo.SelectedValue + " AND OFINISHEDID=" + finishedid + "  AND O_FINISHED_TYPE_ID=" + ddFinish_Type.SelectedValue + ")", true, "-Select-");
                    ddllotno.Focus();
                    if (ddllotno.Items.Count > 0)
                    {
                        ddllotno.SelectedIndex = 1;
                        txtQty.Focus();
                    }
                    ViewState["FinishedId"] = finishedid;
                    string Qry = " select QTY,RATE,IWEIGHT from VIEW_GENERATEINDENTQTY where OFINISHEDID=" + finishedid + "and ORDERID=" + ddOrderNo.SelectedValue + " and PROCESSID=" + DDProcessName.SelectedValue;
                    // Qry = Qry + @"   Select max(IWEIGHT ) IWEIGHT from processconsumptiondetail where pcmid in  (select pcmid from processconsumptionmaster where finishedid="+finishedid + ") and ifinishedid=682";
                    Qry = Qry + @"   Select max(IWEIGHT ) IWEIGHT from processconsumptiondetail where pcmid in  (select pcmid from processconsumptionmaster where finishedid=" + finishedid + ") ";
                    Tran.Commit();
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Qry);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtQty.Text = ds.Tables[0].Rows[0]["QTY"].ToString();
                        TxtRate.Text = ds.Tables[0].Rows[0]["RATE"].ToString();
                        txtweight.Text = ds.Tables[0].Rows[0]["IWEIGHT"].ToString();
                    }
                    FillGrid_New();
                }
            }
            catch
            {

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        else
        {
            TxtPreQty.Text = "";
            txtTotalQty.Text = "";
        }
    }
    protected void ComboFill()
    {
        DDDesign.Items.Clear();
        DDColor.Items.Clear();
        DDShape.Items.Clear();
        DDSize.Items.Clear();
        DDColorShade.Items.Clear();
        if (TdDesign.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDDesign, "SELECT DISTINCT D.DESIGNID,D.DESIGNNAME FROM ORDER_CONSUMPTION_DETAIL OCD,V_FINISHEDITEMDETAIL VF,DESIGN D WHERE OCD.OFINISHEDID=VF.ITEM_FINISHED_ID AND VF.DESIGNID=D.DESIGNID AND OCD.PROCESSID=" + DDProcessName.SelectedValue + " AND ORDERID=" + ddOrderNo.SelectedValue + " AND VF.ITEM_ID=" + DDItem.SelectedValue + " AND VF.QUALITYID=" + DDQuality.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"], true, "--Select Design--");
        }
        if (TdColor.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDColor, "SELECT DISTINCT C.COLORID,C.COLORNAME FROM ORDER_CONSUMPTION_DETAIL OCD,V_FINISHEDITEMDETAIL VF,COLOR C WHERE OCD.OFINISHEDID=VF.ITEM_FINISHED_ID AND VF.COLORID=C.COLORID AND OCD.PROCESSID=" + DDProcessName.SelectedValue + " AND ORDERID=" + ddOrderNo.SelectedValue + " AND VF.ITEM_ID=" + DDItem.SelectedValue + " AND VF.QUALITYID=" + DDQuality.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"], true, "--Select Color--");
        }
        if (TdShape.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDShape, "SELECT DISTINCT S.SHAPEID,S.SHAPENAME FROM ORDER_CONSUMPTION_DETAIL OCD,V_FINISHEDITEMDETAIL VF,SHAPE S WHERE OCD.OFINISHEDID=VF.ITEM_FINISHED_ID AND VF.SHAPEID=S.SHAPEID AND OCD.PROCESSID=" + DDProcessName.SelectedValue + " AND ORDERID=" + ddOrderNo.SelectedValue + " AND VF.ITEM_ID=" + DDItem.SelectedValue + " AND VF.QUALITYID=" + DDQuality.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"], true, "--Select Shape--");
        }
        if (TdSize.Visible == true)
        {
            fillsize();
        }
        if (TdColorShade.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDColorShade, "SELECT DISTINCT S.SHADECOLORID,S.SHADECOLORNAME FROM ORDER_CONSUMPTION_DETAIL OCD,V_FINISHEDITEMDETAIL VF,SHADECOLOR S WHERE OCD.OFINISHEDID=VF.ITEM_FINISHED_ID AND VF.SHADECOLORID=S.SHADECOLORID AND OCD.PROCESSID=" + DDProcessName.SelectedValue + " AND ORDERID=" + ddOrderNo.SelectedValue + " AND VF.ITEM_ID=" + DDItem.SelectedValue + " AND VF.QUALITYID=" + DDQuality.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"], true, "--Select ShadeColor--");
        }
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillsize();
    }
    protected void ChkFt_CheckedChanged(object sender, EventArgs e)
    {
        fillsize();
    }
    private void fillsize()
    {
        string strSize;
        if (ChkFt.Checked == true)
        {
            strSize = "SELECT DISTINCT S.SIZEID,S.SIZEFT FROM ORDER_CONSUMPTION_DETAIL OCD,V_FINISHEDITEMDETAIL VF,SIZE S WHERE OCD.OFINISHEDID=VF.ITEM_FINISHED_ID AND VF.SIZEID=S.SIZEID AND OCD.PROCESSID=" + DDProcessName.SelectedValue + " AND ORDERID=" + ddOrderNo.SelectedValue + " AND VF.ITEM_ID=" + DDItem.SelectedValue + " AND VF.QUALITYID=" + DDQuality.SelectedValue + " AND S.SHAPEID=" + DDShape.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        }
        else
        {
            strSize = "SELECT DISTINCT S.SIZEID,S.SIZEMTR FROM ORDER_CONSUMPTION_DETAIL OCD,V_FINISHEDITEMDETAIL VF,SIZE S WHERE OCD.OFINISHEDID=VF.ITEM_FINISHED_ID AND VF.SIZEID=S.SIZEID AND OCD.PROCESSID=" + DDProcessName.SelectedValue + " AND VF.ITEM_ID=" + DDItem.SelectedValue + " AND VF.QUALITYID=" + DDQuality.SelectedValue + " AND S.SHAPEID=" + DDShape.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        }
        UtilityModule.ConditionalComboFill(ref DDSize, strSize, true, "--Select Size--");
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        //check_qty();
        if (lblMessage.Visible == false || lblMessage.Text == "")
        {
            Save_indent();
        }
    }
    private void Save_indent()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[25];
            if (BtnSave.Text != "Update")
            {
                ViewState["IndentDetailId"] = 0;
            }

            _arrpara[0] = new SqlParameter("@IndentId", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@IndentDetailId", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@PPNo", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@IFinishedId", SqlDbType.Int);
            _arrpara[4] = new SqlParameter("@OFinishedId", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@Quantity", SqlDbType.Float);
            _arrpara[6] = new SqlParameter("@Rate", SqlDbType.Float);
            _arrpara[7] = new SqlParameter("@DyingType", SqlDbType.Int);
            _arrpara[8] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _arrpara[9] = new SqlParameter("@PartyId", SqlDbType.Int);
            _arrpara[10] = new SqlParameter("@ProcessID", SqlDbType.Int);
            _arrpara[11] = new SqlParameter("@Date", SqlDbType.DateTime);
            _arrpara[12] = new SqlParameter("@IndentNo", SqlDbType.NVarChar, 50);
            _arrpara[13] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _arrpara[14] = new SqlParameter("@UserId", SqlDbType.Int);
            _arrpara[15] = new SqlParameter("@Id", SqlDbType.Int);
            _arrpara[16] = new SqlParameter("@lotno", SqlDbType.NVarChar, 50);
            _arrpara[17] = new SqlParameter("@Orderid", SqlDbType.Int);
            _arrpara[18] = new SqlParameter("@O_FINISHED_TYPE_ID", SqlDbType.Int);
            _arrpara[19] = new SqlParameter("@UNITID", SqlDbType.Int);
            _arrpara[20] = new SqlParameter("@IQty", SqlDbType.Float);
            _arrpara[21] = new SqlParameter("@I_FINISHED_TYPE_ID", SqlDbType.Int);
            _arrpara[22] = new SqlParameter("@receivedate", SqlDbType.DateTime);
            _arrpara[23] = new SqlParameter("@remark", SqlDbType.NVarChar, 250);
            _arrpara[24] = new SqlParameter("@weight", SqlDbType.Float);

            _arrpara[0].Value = ViewState["IndentID"];
            _arrpara[1].Value = ViewState["IndentDetailId"];
            //_arrpara[2].Value = DDProcessProgramNo.SelectedValue;
            _arrpara[2].Value = 0;
            _arrpara[4].Value = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, Tran, DDColorShade, "", Convert.ToInt32(Session["varCompanyId"]));
            //_arrpara[3].Value = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select IFinishedId from Order_Consumption_Detail OCD inner join ProcessProgram PP on OCD.OrderId=PP.Order_Id  where PP.PPId=" + DDProcessProgramNo.SelectedValue + " and OCD.OFinishedId=" + _arrpara[4].Value);
            _arrpara[3].Value = 0;
            _arrpara[5].Value = txtQty.Text;
            _arrpara[6].Value = TxtRate.Text;
            //_arrpara[7].Value = DDcaltype.SelectedValue;
            _arrpara[7].Value = 0;
            _arrpara[8].Value = DDCompanyName.SelectedValue;
            _arrpara[9].Value = DDPartyName.SelectedValue;
            _arrpara[10].Value = DDProcessName.SelectedValue;
            _arrpara[11].Value = TxtDate.Text;
            _arrpara[12].Direction = ParameterDirection.InputOutput;
            _arrpara[12].Value = TxtIndentNo.Text.ToUpper();
            _arrpara[13].Value = Session["varCompanyId"];
            _arrpara[14].Value = Session["varuserid"];
            _arrpara[15].Direction = ParameterDirection.Output;
            _arrpara[16].Value = ddllotno.SelectedIndex > -1 ? ddllotno.SelectedItem.Text : "Without Lot No";
            _arrpara[17].Value = ddOrderNo.SelectedValue;
            _arrpara[18].Value = ddFinish_Type.SelectedValue;
            _arrpara[19].Value = ddUnit.SelectedValue;
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "select indentdetailid from Indentdetail where ofinishedid=" + _arrpara[4].Value + " and o_finished_type_id=" + _arrpara[18].Value + " and indentid=" + _arrpara[0].Value + "");
            if (ds.Tables[0].Rows.Count > 0 && ChkEditOrder.Checked == false)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Dublicate Entry');", true);
                return;
            }
            string Str = "Select * from VIEW_GENERATEINDENTQTY Where OrderId=" + _arrpara[17].Value + " And ProcessId=" + _arrpara[10].Value + " And Ofinishedid=" + _arrpara[4].Value + "";
            DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                {
                    //if (DDProcessName.SelectedValue == "9")
                    //    _arrpara[0].Value =ViewState[''IndentId"];
                    //else
                    _arrpara[0].Value = ViewState["IndentID"];
                    //_arrpara[0].Value = 0;
                    _arrpara[3].Value = Convert.ToInt32(Ds.Tables[0].Rows[i]["IFINISHEDID"].ToString());
                    _arrpara[20].Value = Convert.ToDouble(Ds.Tables[0].Rows[i]["IIqty"].ToString()) * Convert.ToDouble(txtQty.Text); //IFINISHEDID,I_FINISHED_TYPE_ID;
                    _arrpara[21].Value = Convert.ToInt32(Ds.Tables[0].Rows[i]["I_FINISHED_TYPE_ID"].ToString());
                    _arrpara[22].Value = TxtrecDate.Text;
                    _arrpara[23].Value = txtremark.Text;
                    _arrpara[24].Value = txtweight.Text != "" ? txtweight.Text : "0";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveIndent_Destini", _arrpara);
                    if (Convert.ToInt32(ViewState["IndentId"]) == 0)
                    {
                        ViewState["IndentID"] = _arrpara[15].Value;
                    }
                }
            }
            Tran.Commit();
            ViewState["IndentID"] = _arrpara[15].Value;
            TxtIndentNo.Text = _arrpara[12].Value.ToString();
            report();
            //Session["ReportPath"] = "Reports/GenrateIndentdestini.rpt";
            //Session["CommanFormula"] = "{GenrateIndentReport_INPUT.IndentDetailId}=" +ViewState[''IndentId"] + "";
            BtnPreview.Enabled = true;
            lblMessage.Visible = false;


            txtQty.Text = "0";
            Fill_Grid();
            SaveRefresh();
            //if (ChkEditOrder.Checked == false)
            //{
            BtnSave.Text = "Save";
            //}
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void FillGrid_New()
    {
        DGShowConsumption.DataSource = GetDetailNew();
        DGShowConsumption.DataBind();
        if (DGShowConsumption.Rows.Count > 0)
            DGShowConsumption.Visible = true;
        else
            DGShowConsumption.Visible = false;
    }
    private DataSet GetDetailNew()
    {
        DataSet DS = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            int FinshedId = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            string sqlstr = @"Select VF.Item_Finished_Id,VF.ProductCode,VF.Category_Name+'  '+VF.Item_Name+'  '+VF.QualityName +'  '+FINISHED_TYPE_Name Description,
                         vg.orderqty-[dbo].[Get_IndentIssQty](VG.PROCESSID,VG.ORDERID,VG.IFinishedid,VG.I_Finished_Type_Id) OrderQty,
                         VG.IQty Consmp,[dbo].[Get_StockQty_DESTINI] (" + ddOrderNo.SelectedValue + @"," + DDProcessName.SelectedValue + @"," + FinshedId + @"," + ddFinish_Type.SelectedValue + @",VF.Item_Finished_Id) StockQty
                         From VIEW_GENERATEINDENTQTY VG,V_FinishedItemDetail VF,Finished_Type FT Where VF.Item_Finished_Id=VG.IFinishedid And VG.I_Finished_Type_Id=FT.Id And OrderId=" + ddOrderNo.SelectedValue + @" And 
                         ProcessId=" + DDProcessName.SelectedValue + @" And OFinishedid=" + FinshedId + @" And O_Finished_Type_Id=" + ddFinish_Type.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
            txtstock.Text = DS.Tables[0].Rows[0]["StockQty"].ToString();

            txtQty.Text = DS.Tables[0].Rows[0]["OrderQty"].ToString();
            if (ChkEditOrder.Checked == true)
            {
                txtstock.Text = Convert.ToString(Convert.ToInt32(txtstock.Text) + Convert.ToInt32(txtQty.Text));
            }
        }
        catch
        {

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return DS;
    }
    private void Fill_Grid()
    {

        DGIndentDetail.DataSource = GetDetail();
        DGIndentDetail.DataBind();
        if (DGIndentDetail.Rows.Count > 0)
            DGIndentDetail.Visible = true;
        else
            DGIndentDetail.Visible = true;

    }
    private DataSet GetDetail()
    {
        DataSet DS = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

        string sqlstr = @"Select Distinct ind.indentdetailid, IND.IndentId IndentId,''PPNo,IndentNo,sum(Quantity) Quantity,Rate,IPM.Category_Name+space(3)+IPM.Item_Name+space(3)+IPM.QualityName +space(3)+IPM.DesignName+space(3)+IPM.ColorName + Space(3)+IPM.ShapeName+ Space(3)+
                        IPM.SizeMtr+space(3)+IPM.ShadeColorName+FT.FINISHED_TYPE_NAME InDescription,'' OutDescription,isnull(ind.weight,0) weight,OFinishedId,O_FINISHED_TYPE_ID
                        From IndentMaster INM,V_FinishedItemDetail IPM,IndentDetail IND LEFT OUTER JOIN FINISHED_TYPE FT ON IND.O_FINISHED_Type_ID=FT.ID
                        where IND.IndentId=INM.IndentId And IND.OFinishedId=IPM.Item_Finished_id and inm.processid=" + DDProcessName.SelectedValue + " And IND.IndentId=" + ViewState["IndentID"].ToString() + " And IPM.MasterCompanyId=" + Session["varCompanyId"] + @"
                        Group by IND.IndentId,ind.indentdetailid,IndentNo,Rate,IPM.Category_Name,IPM.Item_Name,IPM.QualityName,IPM.DesignName,IPM.ColorName,IPM.ShapeName,IPM.SizeMtr,IPM.ShadeColorName,FT.FINISHED_TYPE_NAME,ind.weight,OFinishedId,O_FINISHED_TYPE_ID";
        try
        {
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
        }
        catch
        {

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return DS;
    }
    private void SaveRefresh()
    {
        DDCompanyName.Enabled = false;
        DDPartyName.Enabled = false;
        DDProcessName.Enabled = false;
        //DDProcessProgramNo.Enabled = false;
        TxtIndentNo.Enabled = false;
        TxtDate.Enabled = false;
        TxtIndentNo.Enabled = false;
        DDQuality.SelectedIndex = 0;
        DDDesign.SelectedIndex = -1;
        DDColor.SelectedIndex = -1;
        DDShape.SelectedIndex = -1;
        DDSize.SelectedIndex = -1;
        DDColorShade.SelectedIndex = -1;
        TxtProdCode.Text = "";
        TxtProdCode.Focus();
        txtTotalQty.Text = "";
        TxtPreQty.Text = "";
        txtQty.Text = "";
        TxtRate.Text = "";
        txtweight.Text = "";
        txtremark.Text = "";

    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        Response.RedirectLocation = "../../GenrateInDent.aspx";
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        TXT_PROD_CODE_CHANGE();
    }
    private void TXT_PROD_CODE_CHANGE()
    {
        DataSet ds;
        string Str = "";
        if (TxtProdCode.Text != "")
        {
            lblMessage.Text = "";
            Str = "SELECT DISTINCT IPM.*,IM.CATEGORY_ID,orate as rate FROM ORDER_CONSUMPTION_DETAIL OCD,ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM WHERE OCD.OFINISHEDID=IPM.ITEM_FINISHED_ID AND IPM.ITEM_ID=IM.ITEM_ID AND IPM.ProductCode='" + TxtProdCode.Text + "' And IM.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDCategory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                ddlcategorycange();
                string stritem = "SELECT DISTINCT IM.ITEM_ID,IM.ITEM_NAME FROM ORDER_CONSUMPTION_DETAIL OCD,V_FINISHEDITEMDETAIL VF,ITEM_MASTER IM WHERE OCD.OFINISHEDID=VF.ITEM_FINISHED_ID AND VF.ITEM_ID=IM.ITEM_ID AND OCD.PROCESSID=" + DDProcessName.SelectedValue + " AND ORDERID=" + ddOrderNo.SelectedValue + " AND VF.CATEGORY_ID=" + DDCategory.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
                UtilityModule.ConditionalComboFill(ref DDItem, stritem, true, "---Select Item----");
                DDItem.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                string strquality = "SELECT DISTINCT Q.QUALITYID,Q.QUALITYNAME FROM ORDER_CONSUMPTION_DETAIL OCD,V_FINISHEDITEMDETAIL VF,QUALITY Q  WHERE OCD.OFINISHEDID=VF.ITEM_FINISHED_ID AND VF.QUALITYID=Q.QUALITYID AND OCD.PROCESSID=" + DDProcessName.SelectedValue + " AND ORDERID=" + ddOrderNo.SelectedValue + " AND VF.ITEM_ID=" + DDItem.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                UtilityModule.ConditionalComboFill(ref DDQuality, strquality, true, "--Select Quality--");
                DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                ComboFill();
                //DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                DDDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                DDColor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                DDColorShade.SelectedValue = ds.Tables[0].Rows[0]["ShadeColor_Id"].ToString();
                DDShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SIZEID,SIZEFT fROM SIZE WhERE SHAPEID=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
                DDSize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                Str = "SELECT DISTINCT IPM.*,IM.CATEGORY_ID,orate as rate FROM ORDER_CONSUMPTION_DETAIL OCD,ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM WHERE OCD.OFINISHEDID=IPM.ITEM_FINISHED_ID AND IPM.ITEM_ID=IM.ITEM_ID and ocd.processid=" + DDProcessName.SelectedValue + " AND IPM.ProductCode='" + TxtProdCode.Text + "' And IM.MasterCompanyId=" + Session["varCompanyId"];
                DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                TxtRate.Text = ds1.Tables[0].Rows[0]["Rate"].ToString();
                //txtTotalQty.Text = ds.Tables[0].Rows[0]["QTY"].ToString();
                Fill_Quantity();
                //LOTSELECTEDVALUE();
                if (ddllotno.Items.Count > 0)
                {
                    LOTSELECTEDVALUE();
                }
            }
            else
            {
                lblMessage.Text = "ITEM CODE DOES NOT EXISTS....";
                DDCategory.SelectedIndex = 0;
                ddlcategorycange();
                if (ddllotno.Items.Count > 0)
                {
                    ddllotno.SelectedIndex = 0;
                }
                LOTSELECTEDVALUE();
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
            }
        }
        else
        {
            DDCategory.SelectedIndex = 0;
            ddlcategorycange();
        }
    }
    protected void DGIndentDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Session["IndentDetailId"] = DGIndentDetail.SelectedValue;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            int n = DGIndentDetail.SelectedIndex;

            BtnPreview.Enabled = true;
            ViewState["IndentID"] = DGIndentDetail.SelectedDataKey.Value;
            // report();
            ViewState["IndentID"] = ((Label)DGIndentDetail.Rows[n].FindControl("LblindentID")).Text;
            report();
            //string detailid = ((Label)DGIndentDetail.Rows[n].FindControl("lbldetailid")).Text;
            // ViewState["IndentDetailId"] = detailid;
            ViewState["IndentDetailId"] = DGIndentDetail.SelectedDataKey.Value;
            // DataSet DS1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select IndentId,rate from IndentDetail where IndentDetailId=" +ViewState["IndentDetailId"] + "");
            // ViewState["IndentID"] = DS1.Tables[0].Rows[0]["IndentId"].ToString();

            string str = @"SELECT DISTINCT IPM.PRODUCTCODE FROM ORDER_CONSUMPTION_DETAIL OCD,ITEM_PARAMETER_MASTER IPM WHERE IPM.MasterCompanyId=" + Session["varCompanyId"] + " ANd  OCD.OFINISHEDID=IPM.ITEM_FINISHED_ID AND ORDERID=" + ddOrderNo.SelectedValue + " AND OCD.PROCESSID=" + DDProcessName.SelectedValue + " AND ITEM_FINISHED_ID IN(SELECT OFINISHEDID FROM INDENTDETAIL WHERE INDENTDETAILID=" + ViewState["IndentDetailId"].ToString() + @")
                            select Date, receivedate from indentmaster WHERE IndentID=" + ViewState["IndentID"];
            DataSet DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            //DataSet DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT DISTINCT IPM.PRODUCTCODE FROM ORDER_CONSUMPTION_DETAIL OCD,ITEM_PARAMETER_MASTER IPM WHERE IPM.MasterCompanyId=" + Session["varCompanyId"] + " ANd  OCD.OFINISHEDID=IPM.ITEM_FINISHED_ID AND ORDERID=" + ddOrderNo.SelectedValue + " AND OCD.PROCESSID=" + DDProcessName.SelectedValue + " AND ITEM_FINISHED_ID IN(SELECT OFINISHEDID FROM INDENTDETAIL WHERE INDENTDETAILID=" +ViewState["IndentDetailId"].ToString() + ")");
            if (DS.Tables[0].Rows.Count > 0)
            {
                TxtProdCode.Text = DS.Tables[0].Rows[0][0].ToString();
            }
            //txtQty.Text = DGIndentDetail.Rows[n].Cells[7].Text;
            txtweight.Text = DGIndentDetail.Rows[n].Cells[8].Text;
            TXT_PROD_CODE_CHANGE();
            txtQty.Text = DGIndentDetail.Rows[n].Cells[7].Text;
            TxtDate.Text = Convert.ToDateTime(DS.Tables[1].Rows[0]["date"].ToString()).ToString("dd-MMM-yyyy");
            TxtrecDate.Text = Convert.ToDateTime(DS.Tables[1].Rows[0]["receivedate"].ToString()).ToString("dd-MMM-yyyy");
            if (TxtPreQty.Text != "" && txtQty.Text != "" && TxtPreQty.Text != "0")
            {
                TxtPreQty.Text = Convert.ToString(Convert.ToInt32(TxtPreQty.Text) - Convert.ToInt32(txtQty.Text));
            }
            TxtRate.Text = DGIndentDetail.Rows[n].Cells[6].Text;
            //  ViewState["IndentID"] = indent;
            BtnPreview.Enabled = true;
            BtnSave.Text = "Update";
            //report();
        }
        catch
        {

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        //BtnSave.Text = "Update";
    }
    private void Fill_Rate()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            string st = "select Rate from DyeingRateMaster where PartyId=" + DDPartyName.SelectedValue + " and FromoQty <=" + txtQty.Text + " and ToQty>=" + txtQty.Text + " and  FINISHEDID=" + ViewState["FinishedId"] + " And MasterCompanyId=" + Session["varCompanyId"];
            TxtRate.Text = SqlHelper.ExecuteScalar(Tran, CommandType.Text, st).ToString();
            Tran.Commit();
            if (TxtRate.Text == null)
            {
                lblMessage.Text = "Please add rate first for desired QTY..........";
            }
            else
            {
                lblMessage.Text = "";
            }
        }
        catch
        {

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Quantity();
    }
    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Quantity();
    }
    protected void DDColorShade_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Quantity();
    }
    protected void DDSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Quantity();
    }
    protected void TxtQty_TextChanged(object sender, EventArgs e)
    {

        TxtRate.Focus();
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    private void logo()
    {
        imgLogo.ImageUrl.DefaultIfEmpty();
        imgLogo.ImageUrl = "~/Images/Logo/" + Session["varCompanyId"] + "_company.gif?" + DateTime.Now.ToString("ddyyhhmmss");
        LblCompanyName.Text = Session["varCompanyName"].ToString();
        LblUserName.Text = Session["varusername"].ToString();
    }
    protected void ddllotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        LOTSELECTEDVALUE();
    }
    private void LOTSELECTEDVALUE()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[12];

            _arrpara[0] = new SqlParameter("@COMPANYID", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@ORDERID", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@PROCESSID", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@FINISHEDID", SqlDbType.Int);
            _arrpara[4] = new SqlParameter("@PARTYID", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@FINISHED_TYPE_ID", SqlDbType.Int);
            _arrpara[6] = new SqlParameter("@LOTNO", SqlDbType.NVarChar, 50);
            _arrpara[7] = new SqlParameter("@STOCKQTY", SqlDbType.Float, 50);
            _arrpara[8] = new SqlParameter("@CONSQTY", SqlDbType.Float, 50);
            _arrpara[9] = new SqlParameter("@PREINDENTQTY", SqlDbType.Float, 50);
            _arrpara[10] = new SqlParameter("@RATE", SqlDbType.Float, 50);
            _arrpara[11] = new SqlParameter("@O_FINISHED_TYPE_ID", SqlDbType.Int);

            int VARFINISHEDID = Convert.ToInt32(UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, Tran, DDColorShade, "", Convert.ToInt32(Session["varCompanyId"])));
            _arrpara[0].Value = DDCompanyName.SelectedValue;
            _arrpara[1].Value = ddOrderNo.SelectedValue;
            _arrpara[2].Value = DDProcessName.SelectedValue;
            _arrpara[3].Value = VARFINISHEDID;
            _arrpara[4].Value = DDPartyName.SelectedValue;
            int VAR_FINISH_TYPE = TdFinish_Type.Visible == true ? Convert.ToInt32(ddFinish_Type.SelectedValue) : 0;
            int FINISH_TYPE = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "SELECT I_FINISHED_TYPE_ID FROM ORDER_CONSUMPTION_DETAIL WHERE ORDERID=" + _arrpara[1].Value + " AND OFINISHEDID=" + _arrpara[3].Value + "  AND O_FINISHED_TYPE_ID=" + VAR_FINISH_TYPE + ""));
            _arrpara[5].Value = FINISH_TYPE;
            _arrpara[6].Value = ddllotno.SelectedItem.Text != "" ? ddllotno.SelectedItem.Text : "Without Lot No";
            _arrpara[7].Direction = ParameterDirection.Output;
            _arrpara[8].Direction = ParameterDirection.Output;
            _arrpara[9].Direction = ParameterDirection.Output;
            _arrpara[10].Direction = ParameterDirection.Output;
            _arrpara[11].Value = VAR_FINISH_TYPE;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_GET_INDENT_CONSMP_STOCK_QTY", _arrpara);
            Tran.Commit();

            //txtstock.Text = _arrpara[7].Value.ToString();
            txtTotalQty.Text = _arrpara[8].Value.ToString();
            TxtPreQty.Text = _arrpara[9].Value.ToString();
            TxtRate.Text = _arrpara[10].Value.ToString();
        }
        catch
        {
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void check_qty()
    {
        lblMessage.Visible = false;
        lblMessage.Text = "";
        if (ChkEditOrder.Checked == false)
        {
            int FinshedId = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            string Str = @"Select VF.Item_Finished_Id,VF.ProductCode,VF.Category_Name+'  '+VF.Item_Name+'  '+VF.QualityName +'  '+FINISHED_TYPE_Name Description,OrderQty-[dbo].[Get_IndentIssQty](VG.PROCESSID,VG.ORDERID,VG.IFinishedid,VG.I_Finished_Type_Id) OrderQty,
                    VG.Qty Consmp,[dbo].[Get_StockQty] (" + ddOrderNo.SelectedValue + @"," + DDProcessName.SelectedValue + @"," + FinshedId + @"," + ddFinish_Type.SelectedValue + @",VF.Item_Finished_Id) StockQty
                    From VIEW_GENERATEINDENTQTY VG,V_FinishedItemDetail VF,Finished_Type FT Where VF.Item_Finished_Id=VG.IFinishedid And VG.I_Finished_Type_Id=FT.Id And VF.MasterCompanyId=" + Session["varCompanyId"] + @" And
                    OrderId=" + ddOrderNo.SelectedValue + @" And ProcessId=" + DDProcessName.SelectedValue + @" And OFinishedid=" + FinshedId + @" And 
                    O_Finished_Type_Id=" + ddFinish_Type.SelectedValue + @" And OrderQty-[dbo].[Get_IndentIssQty](VG.PROCESSID,VG.ORDERID,VG.IFinishedid,VG.I_Finished_Type_Id)>=" + txtQty.Text + @" And VG.Qty>=" + txtQty.Text + @" And 
                    [dbo].[Get_StockQty] (" + ddOrderNo.SelectedValue + @"," + DDProcessName.SelectedValue + @"," + FinshedId + @"," + ddFinish_Type.SelectedValue + @",VF.Item_Finished_Id)>=" + txtQty.Text + "";
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (Ds.Tables[0].Rows.Count == 0)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Qty is greater than Stock or Total Qty";
                txtQty.Text = "";
                txtQty.Focus();
            }
            else
            {
                txtweight.Focus();
            }
        }
        else
        {
            if (Convert.ToInt32(txtTotalQty.Text) < Convert.ToInt32(txtQty.Text) || Convert.ToInt32(txtstock.Text) < Convert.ToInt32(txtQty.Text))
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Qty is greater than Stock or Total Qty";
                txtQty.Text = "";
                txtQty.Focus();
            }
            else
            {
                txtweight.Focus();
            }
        }
    }
    //---------------------------------------Product code autocomplete--------------
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER IPM WHERE ProductCode Like '" + prefixText + "%' And MasterCompanyId=" + MasterCompanyId;
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(strQuery, con);
        da.Fill(ds);
        count = ds.Tables[0].Rows.Count;
        con.Close();
        List<string> ProductCode = new List<string>();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            ProductCode.Add(ds.Tables[0].Rows[i][0].ToString());
        }
        con.Close();
        return ProductCode.ToArray();
    }
    protected void DGSHOWDATA_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGSHOWDATA.PageIndex = e.NewPageIndex;
        Fill_Grid_Show();
    }
    private void Fill_Grid_Show()
    {
        DGSHOWDATA.DataSource = "";
        if (ddOrderNo.SelectedIndex > 0 && DDProcessName.SelectedIndex > 0)
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT DISTINCT IPM.PRODUCTCODE FROM ORDER_CONSUMPTION_DETAIL OCD,ITEM_PARAMETER_MASTER IPM WHERE OCD.OFINISHEDID=IPM.ITEM_FINISHED_ID AND ORDERID=" + ddOrderNo.SelectedValue + " AND OCD.PROCESSID=" + DDProcessName.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"] + "");
            DGSHOWDATA.DataSource = Ds;
            DGSHOWDATA.DataBind();
            if (DGSHOWDATA.Rows.Count > 0)
                DGSHOWDATA.Visible = true;
            else
                DGSHOWDATA.Visible = false;
        }
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            refreshchk();
            tdindentno.Visible = true;
            BtnPreview.Enabled = false;
            //BtnSave.Text = "Update";
        }
        else
        {
            tdindentno.Visible = false;
            refreshchk();
            BtnPreview.Enabled = false;
            BtnSave.Text = "Save";
        }

    }
    private void refreshchk()
    {
        DDCompanyName.Enabled = true;
        DDPartyName.Enabled = true;
        DDProcessName.Enabled = true;
        DGSHOWDATA.Visible = false;
        DGIndentDetail.Visible = false;
        DGShowConsumption.Visible = false;
        //DDProcessProgramNo.Enabled = false;
        TxtIndentNo.Enabled = true;
        TxtDate.Enabled = true;
        TxtIndentNo.Enabled = true;
        // DDCompanyName.SelectedIndex = 0;
        DDPartyName.Items.Clear();
        DDProcessName.Items.Clear();
        DDQuality.Items.Clear();
        DDDesign.SelectedIndex = -1;
        DDColor.SelectedIndex = -1;
        DDShape.SelectedIndex = -1;
        DDSize.SelectedIndex = -1;
        DDColorShade.SelectedIndex = -1;
        TxtProdCode.Text = "";
        TxtProdCode.Focus();
        txtTotalQty.Text = "";
        TxtPreQty.Text = "";
        txtQty.Text = "";
        TxtRate.Text = "";
        txtweight.Text = "";
        txtremark.Text = "";
    }
    protected void DGIndentDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGIndentDetail, "Select$" + e.Row.RowIndex);
        }
    }
    private void report()
    {
        Session["ReportPath"] = "Reports/GenrateIndentdestini.rpt";
        Session["CommanFormula"] = "{GenrateIndentReport_INPUT.IndentDetailId}=" + ViewState["IndentID"] + "";
    }
    protected void DGShowConsumption_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGIndentDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGSHOWDATA_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGIndentDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (hnsst.Value == "true")
        {
            try
            {
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);


                int indentID = Convert.ToInt32(DGIndentDetail.DataKeys[e.RowIndex].Value);
                //  int indentID = Convert.ToInt32(DGIndentDetail.SelectedRow.Cells[0].Text);
                SqlParameter[] parparam = new SqlParameter[6];
                parparam[0] = new SqlParameter("@IndentDetailID", SqlDbType.Int);
                parparam[1] = new SqlParameter("@OFinishedId", SqlDbType.Int);
                parparam[2] = new SqlParameter("@O_FINISHED_TYPE_ID", SqlDbType.Int);
                parparam[3] = new SqlParameter("@VarRowcount", SqlDbType.Int);
                parparam[4] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
                parparam[5] = new SqlParameter("@Message", SqlDbType.NVarChar, 200);
                parparam[5].Direction = ParameterDirection.Output;
                //parparam[3] = new SqlParameter("@Message", SqlDbType.VarChar, 200);
                parparam[0].Value = indentID;
                parparam[1].Value = ((Label)DGIndentDetail.Rows[e.RowIndex].FindControl("OFinishedId")).Text;
                parparam[2].Value = ((Label)DGIndentDetail.Rows[e.RowIndex].FindControl("O_FINISHED_TYPE_ID")).Text;
                parparam[3].Value = DGIndentDetail.Rows.Count;
                parparam[4].Value = Session["varCompanyId"].ToString();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_Delete_GenIndent_Destini", parparam);

                // TXT_PROD_CODE_CHANGE();
                FillGrid_New();
                Fill_Grid();
                lblMessage.Text = parparam[5].Value.ToString();
                BtnSave.Text = "Save";
                //lblMessage.Text = "Data Delete Successfully";
                ////parparam[0] = new SqlParameter("@IndentID", "");
                //parparam[0] = new SqlParameter("@IndentID", "");
            }

            catch (Exception ex)
            {
                lblMessage.Text = ex.ToString();
            }
        }
    }

    protected void BtnPreview_Click1(object sender, EventArgs e)
    {
        report();
    }
    protected void ddindentno_SelectedIndexChanged(object sender, EventArgs e)
    {

        ViewState["IndentID"] = ddindentno.SelectedValue;
        TxtIndentNo.Text = ddindentno.SelectedValue;
        Fill_Grid();
        BtnPreview.Enabled = true;
        report();
    }
    protected void DGSHOWDATA_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strcategory = "SELECT DISTINCT IPM.CATEGORY_ID,IPM.CATEGORY_NAME FROM ORDER_CONSUMPTION_DETAIL OCD,V_FINISHEDITEMDETAIL VF,ITEM_CATEGORY_MASTER IPM WHERE OCD.OFINISHEDID=VF.ITEM_FINISHED_ID AND VF.CATEGORY_ID=IPM.CATEGORY_ID AND OCD.PROCESSID=" + DDProcessName.SelectedValue + " AND ORDERID=" + ddOrderNo.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        UtilityModule.ConditionalComboFill(ref DDCategory, strcategory, true, "--Select Category--");
        int n = DGSHOWDATA.SelectedIndex;
        TxtProdCode.Text = ((Label)DGSHOWDATA.Rows[n].FindControl("lblPRODUCTCODE")).Text;
        TXT_PROD_CODE_CHANGE();
    }
    protected void DGSHOWDATA_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGSHOWDATA, "Select$" + e.Row.RowIndex);
        }
    }
}
