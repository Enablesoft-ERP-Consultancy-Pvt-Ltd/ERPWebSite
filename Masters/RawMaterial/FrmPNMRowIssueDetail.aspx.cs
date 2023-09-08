using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Rawmaterial_FrmPNMRowIssueDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By Companyname 
                Select ID, BranchName 
                From BRANCHMASTER BM(nolock) 
                JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"] + @"
                Select DISTINCT PROCESS_NAME_ID,process_name 
                From PROCESS_NAME_MASTER pm 
                Where pm.MasterCompanyId=" + Session["varCompanyId"] + @" And pm.PROCESS_NAME_ID = 5 order by PROCESS_NAME_ID";

            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "Select Comp Name");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 1, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddProcessName, ds, 2, true, "Select Process Name");
            ddProcessName.SelectedIndex = 1;

            ProcessSelectedChange();

            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            if (MySession.TagNowise == "1")
            {
                TDTagNo.Visible = true;
            }
            if (variable.VarBINNOWISE == "1")
            {
                TDBinNo.Visible = true;
            }
            ViewState["Prmid"] = "0";
        }
    }
    protected void ddProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Prmid"] = "0";
        ProcessSelectedChange();
    }
    private void ProcessSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref ddChampoIndentNo, @"Select b.CHAMPOINDENTID, b.CHAMPOINDENTNo 
            From PP_ProcessDirectRawMaster a(Nolock) 
            JOIN PP_ProcessDirectRawTran b(Nolock)  on b.PRMID = a.PRMID 
            Where TRANTYPE = 1 And a.CompanyID = " + ddCompName.SelectedValue + " And a.BRANCHID = " + DDBranchName.SelectedValue + @" And 
            a.PROCESSID = " + ddProcessName.SelectedValue + " And a.MASTERCOMPANYID = " + Session["varCompanyId"] + @" 
            Order By b.CHAMPOINDENTNo", true, "Select Indent No");
    }
    protected void ddChampoIndentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Prmid"] = "0";
        ChampoIndentSelectedChange();
    }
    private void ChampoIndentSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select Distinct a.PRMID, a.ChampoChallanNo 
            From PP_ProcessDirectRawMaster a(Nolock) 
            JOIN PP_ProcessDirectRawTran b(Nolock)  on b.PRMID = a.PRMID And b.ChampoIndentID = " + ddChampoIndentNo.SelectedValue + @" 
            Where a.TranType = 1 And a.CompanyID = " + ddCompName.SelectedValue + " And a.BRANCHID = " + DDBranchName.SelectedValue + " And a.PROCESSID = " + ddProcessName.SelectedValue + @" 
            And a.MASTERCOMPANYID = " + Session["varCompanyId"] + " Order By a.ChampoChallanNo Desc ", true, "Select Indent No");
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Prmid"] = "0";
        ChallanNoSelectedChange();
    }
    private void ChallanNoSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref ddCatagory, @"Select Distinct VF.CATEGORY_ID, VF.CATEGORY_NAME 
            From PP_ProcessDirectRawMaster a(Nolock) 
            JOIN PP_ProcessDirectRawTran b(Nolock) on b.PRMID = a.PRMID And b.CHAMPOINDENTID = " + ddChampoIndentNo.SelectedValue + @" 
            JOIN V_FinishedItemDetail VF(Nolock) on VF.ITEM_FINISHED_ID = b.FINISHEDID 
            Where a.CompanyID = " + ddCompName.SelectedValue + " And a.BRANCHID = " + DDBranchName.SelectedValue + @" And 
            a.PROCESSID = " + ddProcessName.SelectedValue + " And a.PRMID = " + DDChallanNo.SelectedValue + @" 
            And a.MASTERCOMPANYID = " + Session["varCompanyId"] + @" 
            Order By VF.CATEGORY_NAME ", true, "Select Category Name");
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
        Fill_Category();
    }
    private void Fill_Category()
    {
        UtilityModule.ConditionalComboFill(ref dditemname, @"Select Distinct VF.ITEM_ID, VF.ITEM_NAME 
            From PP_ProcessDirectRawMaster a(Nolock) 
            JOIN PP_ProcessDirectRawTran b(Nolock) on b.PRMID = a.PRMID And b.CHAMPOINDENTID = " + ddChampoIndentNo.SelectedValue + @" 
            JOIN V_FinishedItemDetail VF(Nolock) on VF.ITEM_FINISHED_ID = b.FINISHEDID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @" 
            Where a.CompanyID = " + ddCompName.SelectedValue + " And a.BRANCHID = " + DDBranchName.SelectedValue + @" And 
            a.PROCESSID = " + ddProcessName.SelectedValue + " And a.PRMID = " + DDChallanNo.SelectedValue + @" 
            And a.MASTERCOMPANYID = " + Session["varCompanyId"] + @" 
            Order By VF.ITEM_NAME ", true, "Select Item Name");
    }
    private void ddlcategorycange()
    {
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME 
                      FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on 
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCatagory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        ql.Visible = true;
                        break;
                    case "2":
                        dsn.Visible = true;
                        break;
                    case "3":
                        clr.Visible = true;
                        break;
                    case "4":
                        shp.Visible = true;
                        break;
                    case "5":
                        sz.Visible = true;
                        break;
                    case "6":
                        shd.Visible = true;
                        break;
                }
            }
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemChange();
    }
    private void ItemChange()
    {
        CommanFunction.FillCombo(ddlunit, @"select Distinct UnitId,unitName 
            From Unit U(Nolock)
            JOIN Item_Master IM(Nolock) ON IM.UnitTypeId = U.UnitTypeId 
            Where IM.ITEM_ID=" + dditemname.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"]);

        UtilityModule.ConditionalComboFill(ref dquality, @"Select Distinct VF.QualityID, VF.QualityName 
            From PP_ProcessDirectRawMaster a(Nolock) 
            JOIN PP_ProcessDirectRawTran b(Nolock) on b.PRMID = a.PRMID And b.CHAMPOINDENTID = " + ddChampoIndentNo.SelectedValue + @" 
            JOIN V_FinishedItemDetail VF(Nolock) on VF.ITEM_FINISHED_ID = b.FINISHEDID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @" 
                     And VF.ITEM_ID = " + dditemname.SelectedValue + @" 
            Where a.CompanyID = " + ddCompName.SelectedValue + " And a.BRANCHID = " + DDBranchName.SelectedValue + @" And 
            a.PROCESSID = " + ddProcessName.SelectedValue + " And a.PRMID = " + DDChallanNo.SelectedValue + @" 
            And a.MASTERCOMPANYID = " + Session["varCompanyId"] + @" 
            Order By VF.QualityName ", true, "Select Quallity");
    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddQualitySelectedChanged();
    }
    private void ddQualitySelectedChanged()
    {
        if (dsn.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref dddesign, @"Select Distinct VF.DesignID, VF.DesignName 
            From PP_ProcessDirectRawMaster a(Nolock) 
            JOIN PP_ProcessDirectRawTran b(Nolock) on b.PRMID = a.PRMID And b.CHAMPOINDENTID = " + ddChampoIndentNo.SelectedValue + @" 
            JOIN V_FinishedItemDetail VF(Nolock) on VF.ITEM_FINISHED_ID = b.FINISHEDID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @" 
                     And VF.ITEM_ID = " + dditemname.SelectedValue + " And VF.QualityID = " + dquality.SelectedValue + @" 
            Where a.CompanyID = " + ddCompName.SelectedValue + " And a.BRANCHID = " + DDBranchName.SelectedValue + @" And 
            a.PROCESSID = " + ddProcessName.SelectedValue + " And a.PRMID = " + DDChallanNo.SelectedValue + @" 
            And a.MASTERCOMPANYID = " + Session["varCompanyId"] + @" 
            Order By VF.DesignName ", true, "Select Design");
        }
        if (clr.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddcolor, @"Select Distinct VF.ColorID, VF.ColorName 
            From PP_ProcessDirectRawMaster a(Nolock) 
            JOIN PP_ProcessDirectRawTran b(Nolock) on b.PRMID = a.PRMID And b.CHAMPOINDENTID = " + ddChampoIndentNo.SelectedValue + @" 
            JOIN V_FinishedItemDetail VF(Nolock) on VF.ITEM_FINISHED_ID = b.FINISHEDID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @" 
                     And VF.ITEM_ID = " + dditemname.SelectedValue + " And VF.QualityID = " + dquality.SelectedValue + @" 
            Where a.CompanyID = " + ddCompName.SelectedValue + " And a.BRANCHID = " + DDBranchName.SelectedValue + @" And 
            a.PROCESSID = " + ddProcessName.SelectedValue + " And a.PRMID = " + DDChallanNo.SelectedValue + @" 
            And a.MASTERCOMPANYID = " + Session["varCompanyId"] + @" 
            Order By VF.ColorName ", true, "Select Color");
        }
        if (shp.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddshape, @"Select Distinct VF.ShapeID, VF.ShapeName 
            From PP_ProcessDirectRawMaster a(Nolock) 
            JOIN PP_ProcessDirectRawTran b(Nolock) on b.PRMID = a.PRMID And b.CHAMPOINDENTID = " + ddChampoIndentNo.SelectedValue + @" 
            JOIN V_FinishedItemDetail VF(Nolock) on VF.ITEM_FINISHED_ID = b.FINISHEDID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @" 
                     And VF.ITEM_ID = " + dditemname.SelectedValue + " And VF.QualityID = " + dquality.SelectedValue + @" 
            Where a.CompanyID = " + ddCompName.SelectedValue + " And a.BRANCHID = " + DDBranchName.SelectedValue + @" And 
            a.PROCESSID = " + ddProcessName.SelectedValue + " And a.PRMID = " + DDChallanNo.SelectedValue + @" 
            And a.MASTERCOMPANYID = " + Session["varCompanyId"] + @" 
            Order By VF.ShapeName ", true, "Select Shape");
        }
        if (shd.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddlshade, @"Select Distinct VF.ShadeColorID, VF.ShadeColorName 
            From PP_ProcessDirectRawMaster a(Nolock) 
            JOIN PP_ProcessDirectRawTran b(Nolock) on b.PRMID = a.PRMID And b.CHAMPOINDENTID = " + ddChampoIndentNo.SelectedValue + @" 
            JOIN V_FinishedItemDetail VF(Nolock) on VF.ITEM_FINISHED_ID = b.FINISHEDID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @" 
                     And VF.ITEM_ID = " + dditemname.SelectedValue + " And VF.QualityID = " + dquality.SelectedValue + @" 
            Where a.CompanyID = " + ddCompName.SelectedValue + " And a.BRANCHID = " + DDBranchName.SelectedValue + @" And 
            a.PROCESSID = " + ddProcessName.SelectedValue + " And a.PRMID = " + DDChallanNo.SelectedValue + @" 
            And a.MASTERCOMPANYID = " + Session["varCompanyId"] + @" 
            Order By VF.ShadeColorName ", true, "Select ShadeColor");
        }
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddsize, @"Select Distinct VF.SizeID, VF.SizeFt 
            From PP_ProcessDirectRawMaster a(Nolock) 
            JOIN PP_ProcessDirectRawTran b(Nolock) on b.PRMID = a.PRMID And b.CHAMPOINDENTID = " + ddChampoIndentNo.SelectedValue + @" 
            JOIN V_FinishedItemDetail VF(Nolock) on VF.ITEM_FINISHED_ID = b.FINISHEDID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @" 
                    And VF.ITEM_ID = " + dditemname.SelectedValue + " And VF.QualityID = " + dquality.SelectedValue + @" 
                    And VF.ShapeID = " + ddshape.SelectedValue + @" 
            Where a.CompanyID = " + ddCompName.SelectedValue + " And a.BRANCHID = " + DDBranchName.SelectedValue + @" And 
            a.PROCESSID = " + ddProcessName.SelectedValue + " And a.PRMID = " + DDChallanNo.SelectedValue + @" 
            And a.MASTERCOMPANYID = " + Session["varCompanyId"] + @" 
            Order By VF.ShadeColorName ", true, "Select ShadeColor");
    }
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        UtilityModule.ConditionalComboFill(ref ddgodown, @"Select Distinct GM.GodownID, GM.GodownName 
            From PP_ProcessDirectRawMaster a(Nolock) 
            JOIN PP_ProcessDirectRawTran b(Nolock) on b.PRMID = a.PRMID And b.CHAMPOINDENTID = " + ddChampoIndentNo.SelectedValue + @" 
                And b.FinishedID = " + Varfinishedid + @" 
            JOIN GodownMaster GM(Nolock) ON GM.GodownID = b.GodownID 
            Where a.CompanyID = " + ddCompName.SelectedValue + " And a.BRANCHID = " + DDBranchName.SelectedValue + @" And 
            a.PROCESSID = " + ddProcessName.SelectedValue + " And a.PRMID = " + DDChallanNo.SelectedValue + @" 
            And a.MASTERCOMPANYID = " + Session["varCompanyId"] + @" 
            Order By GM.GodownName", true, "Select GodownName");
    }
    protected void ddgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        gowdownchange();
    }
    private void gowdownchange()
    {
        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        string str = @"Select Distinct b.LOTNO, b.LOTNO 
            From PP_ProcessDirectRawMaster a(Nolock) 
            JOIN PP_ProcessDirectRawTran b(Nolock) on b.PRMID = a.PRMID And b.CHAMPOINDENTID = " + ddChampoIndentNo.SelectedValue + @" And 
                b.FinishedID = " + Varfinishedid + @" 
            Where a.CompanyID = " + ddCompName.SelectedValue + " And a.BRANCHID = " + DDBranchName.SelectedValue + @" And 
                a.PROCESSID = " + ddProcessName.SelectedValue + @" And a.PRMID = " + DDChallanNo.SelectedValue + @" 
            And a.MASTERCOMPANYID = " + Session["varCompanyId"] + @" 
            Order By b.LOTNO ";

        UtilityModule.ConditionalComboFill(ref ddlotno, str, true, "Select LotNo");
    }
    protected void ddlotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        if (TDTagNo.Visible == true)
        {
            FillTagNo(Varfinishedid, sender);
        }
        else if (TDBinNo.Visible == true)
        {
            FillBinNo(Varfinishedid, sender);
        }
        else
        {
            getstockQty(Varfinishedid);
        }
    }
    protected void FillBinNo(int Varfinishedid, object sender = null)
    {
        string str = @"Select Distinct b.BinNo, b.BinNo 
            From PP_ProcessDirectRawMaster a(Nolock) 
            JOIN PP_ProcessDirectRawTran b(Nolock) on b.PRMID = a.PRMID And b.CHAMPOINDENTID = " + ddChampoIndentNo.SelectedValue + @" And 
                b.FinishedID = " + Varfinishedid + @" And b.LotNo = '" + ddlotno.SelectedValue + @"'";
        if (TDTagNo.Visible == true)
        {
            str = str + @" And b.TagNo = '" + DDTagNo.SelectedValue + @"'";
        }
        str = str + @" Where a.CompanyID = " + ddCompName.SelectedValue + " And a.BRANCHID = " + DDBranchName.SelectedValue + @" And 
            a.PROCESSID = " + ddProcessName.SelectedValue + @" And a.PRMID = " + DDChallanNo.SelectedValue + @" 
        And a.MASTERCOMPANYID = " + Session["varCompanyId"] + @" 
        Order By b.BinNo";

        UtilityModule.ConditionalComboFill(ref DDBinNo, str, true, "Select Bin No");
    }
    protected void FillTagNo(int Varfinishedid, object sender = null)
    {
        string str = @"Select Distinct b.TagNo, b.TagNo 
            From PP_ProcessDirectRawMaster a(Nolock) 
            JOIN PP_ProcessDirectRawTran b(Nolock) on b.PRMID = a.PRMID And b.CHAMPOINDENTID = " + ddChampoIndentNo.SelectedValue + @" And 
                b.FinishedID = " + Varfinishedid + @" And b.LotNo = '" + ddlotno.SelectedValue + @"'
            Where a.CompanyID = " + ddCompName.SelectedValue + " And a.BRANCHID = " + DDBranchName.SelectedValue + @" And 
                a.PROCESSID = " + ddProcessName.SelectedValue + @" And a.PRMID = " + DDChallanNo.SelectedValue + @" 
            And a.MASTERCOMPANYID = " + Session["varCompanyId"] + @" 
            Order By b.TagNo";

        UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "Select Tagno");
    }
    protected void DDTagNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        if (TDBinNo.Visible == true)
        {
            FillBinNo(Varfinishedid, sender);
        }
        else
        {
            getstockQty(Varfinishedid);
        }
    }
    protected void DDBinNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        getstockQty(Varfinishedid);
    }
    protected void getstockQty(int Varfinishedid)
    {
        TxtStockQty.Text = "";
        string str = @"Select Sum(QtyinHand) StockQty  
            From Stock(Nolock) 
            Where CompanyID = " + ddCompName.SelectedValue + " And ITEM_FINISHED_ID = " + Varfinishedid + " And GodownID = " + ddgodown.SelectedValue + @" 
            And LotNo = '" + ddlotno.SelectedItem.Text + "' And TagNo = '" + DDTagNo.SelectedItem.Text + "' And BINNO = '" + DDBinNo.SelectedItem.Text + "'";

        DataSet ds = SqlHelper.ExecuteDataset(str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtStockQty.Text = ds.Tables[0].Rows[0]["StockQty"].ToString();
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        save_detail();
    }
    protected void save_detail()
    {
        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[21];
            arr[0] = new SqlParameter("@PrmID", SqlDbType.Int);
            arr[1] = new SqlParameter("@CompanyID", SqlDbType.Int);
            arr[2] = new SqlParameter("@BranchID", SqlDbType.Int);
            arr[3] = new SqlParameter("@ProcessID", SqlDbType.Int);
            arr[4] = new SqlParameter("@ChampoIndentID", SqlDbType.Int);
            arr[5] = new SqlParameter("@ReceivePRMID", SqlDbType.Int);
            arr[6] = new SqlParameter("@IssueDate", SqlDbType.DateTime);
            arr[7] = new SqlParameter("@FinishedID", SqlDbType.Int);
            arr[8] = new SqlParameter("@UnitID", SqlDbType.Int);
            arr[9] = new SqlParameter("@GodownID", SqlDbType.Int);
            arr[10] = new SqlParameter("@LotNo", SqlDbType.NVarChar, 200);
            arr[11] = new SqlParameter("@TagNo", SqlDbType.NVarChar, 200);
            arr[12] = new SqlParameter("@BinNo", SqlDbType.NVarChar, 50);
            arr[13] = new SqlParameter("@IssueQty", SqlDbType.Decimal);
            arr[14] = new SqlParameter("@varuserid", SqlDbType.Int);
            arr[15] = new SqlParameter("@varcompanyid", SqlDbType.Int);
            arr[16] = new SqlParameter("@Remark", SqlDbType.VarChar, 1000);
            arr[17] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            arr[18] = new SqlParameter("@TranType", SqlDbType.Int);
            arr[19] = new SqlParameter("@IssueNo", SqlDbType.NVarChar, 100);

            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = ViewState["Prmid"];
            arr[1].Value = ddCompName.SelectedValue;
            arr[2].Value = DDBranchName.SelectedValue;
            arr[3].Value = ddProcessName.SelectedValue;
            arr[4].Value = ddChampoIndentNo.SelectedValue;
            arr[5].Value = DDChallanNo.SelectedValue;
            arr[6].Value = txtdate.Text;
            arr[7].Value = Varfinishedid;
            arr[8].Value = ddlunit.SelectedValue;
            arr[9].Value = ddgodown.SelectedValue;
            arr[10].Value = ddlotno.SelectedItem.Text;

            string TagNo;
            if (TDTagNo.Visible == true)
            {
                TagNo = DDTagNo.SelectedItem.Text;
            }
            else
            {
                TagNo = "Without Tag No";
            }
            string BinNo = "";
            BinNo = TDBinNo.Visible == false ? "" : (DDBinNo.SelectedIndex > 0 ? DDBinNo.SelectedItem.Text : "");
            
            arr[11].Value = TagNo;
            arr[12].Value = BinNo;
            arr[13].Value = txtissqty.Text;
            arr[14].Value = Session["varuserid"].ToString();
            arr[15].Value = Session["varCompanyId"].ToString();
            arr[16].Value = txtremarks.Text;
            arr[17].Direction = ParameterDirection.InputOutput;
            arr[18].Value = 0;
            arr[19].Direction = ParameterDirection.InputOutput;
            arr[19].Value = txtIssueNo.Text;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[PRO_Save_PNMRawIssueDetail]", arr);
            ViewState["Prmid"] = arr[0].Value.ToString();

            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + arr[17].Value.ToString() + "');", true);

            tran.Commit();
            txtIssueNo.Text = arr[19].Value.ToString();
            pnl1.Enabled = false;
            btnsave.Text = "Save";
            Fill_Grid();
            save_refresh();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/FrmPNMRowIssueDetail.aspx");
            tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void save_refresh()
    {
        dditemname.SelectedIndex = 0;
        dquality.SelectedIndex = ql.Visible == true ? 0 : -1;
        ddlshade.SelectedIndex = shd.Visible == true ? 0 : -1;
        ddgodown.SelectedIndex = 0;
        ddlotno.SelectedIndex = 0;
        DDTagNo.SelectedIndex = 0;
        DDBinNo.SelectedIndex = 0;
        TxtStockQty.Text = "";
        txtpendingqty.Text = "";
        txtissqty.Text = "";        
        txtremarks.Text = "";
    }
    protected void btnnew_Click(object sender, EventArgs e)
    {
        refresh_form();
    }
    private void Fill_Grid()
    {
        gvdetail.DataSource = fill_Data_grid();
        gvdetail.DataBind();
        Session["ReportPath"] = "Reports/IndentRawIssue.rpt";
        Session["CommanFormula"] = "{V_IndentRawIssue.PRMid}=" + ViewState["Prmid"] + "";
    }
    private DataSet fill_Data_grid()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"Select PRT.PRTID, VF.ITEM_NAME, Replace(ISNULL(VF.QualityName, '') + '  ' + ISNULL(VF.DesignName, '') + '  ' + ISNULL(VF.ColorName, '') + '  ' + 
                ISNULL(VF.ShapeName, '') + '  ' + ISNULL(VF.ShadeColorName, '') + '  ' + ISNULL(VF.SizeFt, ''), '   ', '') [DESCRIPTION], 
                GM.GodownName, PRT.QTY, PRT.LOTNO , PRT.TAGNO, PRT.BINNO 
                From PP_ProcessDirectRawMaster PRM(Nolock) 
                JOIN PP_ProcessDirectRawTran PRT(Nolock) ON PRT.PRMID = PRM.PRMID 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = PRT.FINISHEDID 
                JOIN GodownMaster GM ON GM.GoDownID = PRT.GODOWNID 
                Where PRM.PRMid = " + ViewState["Prmid"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/FrmPNMRowIssueDetail.aspx");
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
    protected void gvdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvdetail, "Select$" + e.Row.RowIndex);

            for (int i = 0; i < gvdetail.Columns.Count; i++)
            {
                if (variable.VarBINNOWISE == "1")
                {
                    if (gvdetail.Columns[i].HeaderText.ToUpper() == "BIN NO.")
                    {
                        gvdetail.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (gvdetail.Columns[i].HeaderText.ToUpper() == "BIN NO.")
                    {
                        gvdetail.Columns[i].Visible = false;
                    }

                }
            }
        }

    }
    protected void gvdetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvdetail.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {

    }
    private void refresh_form()
    {
        ViewState["Prtid"] = 0;
        pnl1.Enabled = true;
        ddProcessName.SelectedValue = null;
        ddCatagory.SelectedValue = null;
        dditemname.SelectedValue = null;
        ddgodown.SelectedValue = null;
        dquality.SelectedValue = null;
        dddesign.SelectedValue = null;
        ddcolor.SelectedValue = null;
        ddshape.SelectedValue = null;
        ddsize.SelectedValue = null;
        ddlshade.SelectedValue = null;
        txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        TxtProdCode.Text = "";
        btnsave.Text = "Save";
        Label1.Visible = false;
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Session.Remove("finishedid");
        Session.Remove("indentid");
        Session.Remove("indentdetailid");
    }
    protected void btnpriview_Click(object sender, EventArgs e)
    {
        Report();
    }
    private void Report()
    {
        string qry = @"Select Distinct '' EmpName, '' Address, '' PhoneNo, '' Mobile, '' EmailAdd, vc.companyid, Vc.CompanyName, BM.BranchAddress CompanyAddress, 
            BM.PhoneNo ComPanyPhoneNo, CompanyFaxNo, VC.TinNo, IPM.Category_Name, IPM.Item_Name, IPM.QualityName, IPM.DesignName, IPM.ColorName, IPM.ShapeName, 
            IPM.ShadeColorName, IPM.SizeMtr, IPM.SizeFt, PRM.ChallanNo, PNM.ShortName, 0 Indentid, GatepassNo, PRT.PRMid, PRTid, PRT.QTY IssueQuantity, 
            prt.LotNo, GM.GodownNAme, PRT.FinishedId, 0 Finish_Type_Id, '' IndentNo, PRM.PROCESSID, u.unitname, PRM.Date, 0 orderid, '' localorder, 
            '' customerorderno,'' reqdate,prt.remark,replace(convert(varchar(11),prm.date,106),' ','-') Issuedate, 0 O_FINISHED_TYPE_ID, prt.GoDownID, 
            0 CanQty,0 flagsize, PRM.MASTERCOMPANYID, PNM.Process_Name, '' Buyercode, Prt.TagNo, BM.GSTNo GSTIN, '' EmpGStno, ISNULL(PRT.BINNO,0) BinNo, 0 ManualRate, 
            0 GSTType, 0 CGST, 0 SGST, 0 IGST, isnull(IPM.HSNCode,'') HSNCode, '' VehicleNo, '' DriverName, '' EWayBillNo, 0 PurchaseRate, 0 PurchaseAmt, 
            0 CGSTAmt, 0 SGSTAmt, 0 IGSTAmt , isnull(NU.UserName,'') as UserName 
            From PP_ProcessDirectRawMaster PRM(Nolock) 
            JOIN PP_ProcessDirectRawTran PRT(Nolock) on PRT.PRMid = PRM.PRMid 
            JOIN V_FinishedItemDetail IPM(Nolock) on IPM.Item_Finished_Id = PRT.FinishedId 
            JOIN BRANCHMASTER BM(Nolock) ON BM.ID = PRM.BranchID 
            JOIN GodownMaster GM(Nolock) on GM.GodownId=PRT.GodownId 
            JOIN V_CompanyInfo VC(Nolock) on Prm.CompanyId=VC.CompanyId   
            JOIN Process_Name_Master PNM(Nolock) on PRM.ProcessId=PNM.Process_Name_ID   
            JOIN unit u(Nolock) on u.unitid=prt.unitid   
            JOIN NewUserDetail NU(NoLock) ON PRM.UserId=NU.UserId 
            Where PRM.PRMID = " + ViewState["Prmid"];

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\IndentRawDirectIssueToPNM.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\IndentRawDirectIssueToPNM.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
//        string qry = @" SELECT '' EmpName, '' Address, '' PhoneNo, '' Mobile, CI.CompanyName, CI.CompAddr1 CompanyAddress, CI.CompTel ComPanyPhoneNo, CI.CompFax CompanyFaxNo, 
//            CI.TinNo, VF.Category_Name, VF.Item_Name, VF.QualityName, VF.Designname, VF.Colorname, VF.Shapename, VF.ShadeColorName, VF.SizeMtr, PRM.ChallanNo, 0 Indentid, 
//            PRM.PRMid, PRT.Qty IssueQuantity, PRT.LotNo, GM.GodownNAme, '' ShortName, VF.ShadeColorName, U.unitname, CI.companyid, '' IndentNo, PRM.Date, 
//            0 flagsize, 0 CanQty,PRM.MastercompanyId, PNM.Process_Name, '' Buyercode, '' localorder, '' customerorderno, PRT.Remark,
//            '' GSTIN, '' EMPGSTNO, PRT.BinNo, NUD.UserName, '' reqdate 
//            From PP_ProcessDirectRawMaster PRM(Nolock) 
//            JOIN PP_ProcessDirectRawTran PRT(Nolock) ON PRT.PRMID = PRM.PRMID 
//            JOIN CompanyInfo CI ON CI.CompanyID = PRM.CompanyID 
//            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = PRT.FINISHEDID 
//            JOIN GodownMaster GM(Nolock) ON GM.GoDownID = PRT.GODOWNID 
//            JOIN NewUserDetail NUD(Nolock) ON NUD.UserId = PRM.USERID 
//            JOIN Unit U(Nolock) ON U.UnitId = PRT.UNITID 
//            JOIN Process_name_Master PNM(Nolock) ON PNM.PROCESS_NAME_ID = PRM.PROCESSID 
//            Where PRM.PRMID = " + ViewState["Prmid"];

//        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);

//        DataTable dt = new DataTable();
//        ds.Tables.Add(dt);
//        if (ds.Tables[0].Rows.Count > 0)
//        {
//            Session["rptFileName"] = "~\\Reports\\IndentRawIssueNEWChampo.rpt";

//            Session["GetDataset"] = ds;
//            Session["dsFileName"] = "~\\ReportSchema\\IndentRawIssueNEW.xsd";
//            StringBuilder stb = new StringBuilder();
//            stb.Append("<script>");
//            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
//            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
//        }
//        else
//        {
//            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
//        }
    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            //SqlParameter[] arr = new SqlParameter[5];
            //arr[0] = new SqlParameter("@Prtid", SqlDbType.Int);
            //arr[1] = new SqlParameter("@TableName", SqlDbType.NVarChar, 50);
            //arr[2] = new SqlParameter("@Count", SqlDbType.Int);
            //arr[3] = new SqlParameter("@FlagDeleteOrNot", SqlDbType.Int);
            //arr[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);

            //arr[0].Value = gvdetail.DataKeys[e.RowIndex].Value;
            //arr[1].Value = "PP_ProcessRawTran";
            //arr[2].Value = gvdetail.Rows.Count;
            //arr[3].Direction = ParameterDirection.Output;
            //arr[4].Direction = ParameterDirection.Output;

            //SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_UpdateStockIssueDelete", arr);
            //tran.Commit();
            //if (arr[4].Value.ToString() != "")
            //{
            //    Label1.Text = arr[4].Value.ToString();
            //    Label1.Visible = true;
            //}
            //else
            //{
            //    if (Convert.ToInt32(arr[3].Value) == 1)
            //    {
            //        Label1.Text = "Data already received against this item.";
            //        Label1.Visible = true;
            //    }
            //    else
            //    {
            //        Fill_Grid();
            //    }
            //}
        }
        catch (Exception ex)
        {
            Label1.Text = ex.Message;
            Label1.Visible = true;
            tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
}
