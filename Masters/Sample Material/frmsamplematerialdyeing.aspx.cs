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

public partial class Masters_Sample_Material_frmsamplematerialdyeing : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                           select PROCESS_NAME_ID,Process_name From process_name_master  where Processtype=0 and mastercompanyid=" + Session["varcompanyid"] + @" order by PROCESS_NAME
                           select Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=cs.Categoryid and cs.id=1 and ICM.MasterCompanyid=" + Session["varcompanyid"] + @"
                           Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + @" Order by GodownName
                           select CalID,CalType from Process_CalType order by caltype
                           select Val,Type from Sizetype
                           select godownid From Modulewisegodown Where ModuleName='" + Page.Title + @"' 
                           Select ID, BranchName 
                                From BRANCHMASTER BM(nolock) 
                                JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                                Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 7, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }
            
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDRcategory, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDgodown, ds, 3, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDcaltype, ds, 4, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 5, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDRSizetype, ds, 5, false, "--Plz Select--");
            //auto select godown
            if (ds.Tables[6].Rows.Count > 0)
            {
                if (DDgodown.Items.FindByValue(ds.Tables[6].Rows[0]["godownid"].ToString()) != null)
                {
                    DDgodown.SelectedValue = ds.Tables[6].Rows[0]["godownid"].ToString();
                }
            }
            if (DDProcessName.Items.Count > 0)
            {
                if (DDProcessName.Items.FindByValue("5") != null) //DYEING
                {
                    DDProcessName.SelectedValue = "5";
                    DDProcessName_SelectedIndexChanged(sender, e);
                }
            }

            TxtDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            TxtReqDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (MySession.TagNowise == "1")
            {
                TDTagno.Visible = true;
                TDrectagno.Visible = true;
            }
            if (variable.VarBINNOWISE == "1")
            {
                TDBinNo.Visible = true;
            }
            ds.Dispose();
            chkfillsame.Checked = true;
            //**********Edit
            if (Session["canedit"].ToString() == "1")
            {
                TREdit.Visible = true;
                TDComplete.Visible = true;
            }
            //**********
            switch (Session["varcompanyno"].ToString())
            {
                case "16":
                    lblindentnoedit.Text = "Challan No.";
                    lblindentno.Text = "Challan No.";
                    TxtIndentNo.Enabled = true;
                    TDrectagno.Visible = false;
                    break;
                case "31":
                    TxtIndentNo.Enabled = true;
                    break;
                default:
                    break;
            }
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDPartyName, "Select EI.EmpId,EI.EmpName From EmpInfo EI inner join EmpProcess EP on EI.EmpId=EP.EmpId and EP.ProcessId=" + DDProcessName.SelectedValue + " order by EI.empname", true, "--Plz Select--");
        switch (DDProcessName.SelectedItem.Text.ToUpper())
        {
            case "DYEING":
                TDDyeingMatch.Visible = true;
                break;
            default:
                TDDyeingMatch.Visible = false;
                break;
        }
    }
    private void ddlcategorycange()
    {
        DDQuality.Items.Clear();
        DDDesign.Items.Clear();
        DDColor.Items.Clear();
        DDShape.Items.Clear();
        DDSize.Items.Clear();
        DDColorShade.Items.Clear();
        TDIquality.Visible = false;
        TDIdesign.Visible = false;
        TDIcolor.Visible = false;
        TDIColorShade.Visible = false;
        TDIShape.Visible = false;
        TDISize.Visible = false;
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
                        TDIquality.Visible = true;
                        break;
                    case "2":
                        TDIdesign.Visible = true;
                        break;
                    case "3":
                        TDIcolor.Visible = true;
                        break;
                    case "6":
                        TDIColorShade.Visible = true;
                        break;
                    case "4":
                        TDIShape.Visible = true;
                        break;
                    case "5":
                        TDISize.Visible = true;
                        break;
                    case "10":
                        TDIcolor.Visible = true;
                        break;
                }
            }
        }

        string stritem = "select distinct IM.Item_Id,IM.Item_Name from  Item_Parameter_Master IPM  inner Join Item_Master IM on IM.Item_Id=IPM.Item_Id inner join Item_Category_Master ICM on ICM.Category_Id=IM.Category_Id where  IM.Category_Id=" + DDCategory.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " order by IM.item_name";
        UtilityModule.ConditionalComboFill(ref DDItem, stritem, true, "---Select Item----");
    }
    private void Rcategorychange()
    {
        DDRquality.Items.Clear();
        DDRdesign.Items.Clear();
        DDRcolor.Items.Clear();
        DDRshape.Items.Clear();
        DDRsize.Items.Clear();
        DDRShadecolor.Items.Clear();
        TDRquality.Visible = false;
        TDRdesign.Visible = false;
        TDRcolor.Visible = false;
        TDRShadecolor.Visible = false;
        TDRShape.Visible = false;
        TDRSize.Visible = false;
        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME 
                      FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on 
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDRcategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        TDRquality.Visible = true;
                        break;
                    case "2":
                        TDRdesign.Visible = true;
                        break;
                    case "3":
                        TDRcolor.Visible = true;
                        break;
                    case "6":
                        TDRShadecolor.Visible = true;
                        break;
                    case "4":
                        TDRShape.Visible = true;
                        break;
                    case "5":
                        TDRSize.Visible = true;
                        break;
                    case "10":
                        TDRcolor.Visible = true;
                        break;
                }
            }
        }
        string stritem = "select distinct IM.Item_Id,IM.Item_Name from  Item_Parameter_Master IPM  inner Join Item_Master IM on IM.Item_Id=IPM.Item_Id inner join Item_Category_Master ICM on ICM.Category_Id=IM.Category_Id where  IM.Category_Id=" + DDRcategory.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " order by IM.item_name";
        UtilityModule.ConditionalComboFill(ref DDRitemName, stritem, true, "---Select Item----");
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
        if (chkfillsame.Checked == true)
        {
            DDRcategory.SelectedValue = DDCategory.SelectedValue;
            DDRcategory_SelectedIndexChanged1(sender, e);
        }
    }

    protected void DDRcategory_SelectedIndexChanged1(object sender, EventArgs e)
    {
        Rcategorychange();
    }
    private void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Shade, int Itemid)
    {
        string Str = @"SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + Itemid + " And MasterCompanyId=" + Session["varCompanyId"] + @" Order By QUALITYNAME
                     SELECT DESIGNID,DESIGNNAME from DESIGN Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By DESIGNNAME
                     SELECT COLORID,COLORNAME FROM COLOR Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By COLORNAME
                     SELECT SHAPEID,SHAPENAME FROM SHAPE Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By SHAPENAME
                     SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR Where  MasterCompanyId=" + Session["varCompanyId"] + " Order By SHADECOLORNAME";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref Quality, ds, 0, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Design, ds, 1, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Color, ds, 2, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Shape, ds, 3, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Shade, ds, 4, true, "--SELECT--");
    }
    protected void DDItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(DDQuality, DDDesign, DDColor, DDShape, DDColorShade, Convert.ToInt16(DDItem.SelectedValue));
        UtilityModule.ConditionalComboFill(ref ddUnit, "select Distinct U.UnitId,u.UnitName from Item_master IM inner join UNIT_TYPE_MASTER UT on IM.UnitTypeID=UT.UnitTypeID inner join Unit u on U.UnitTypeID=UT.UnitTypeID and Im.ITEM_ID=" + DDItem.SelectedValue + "", false, "");
        if (chkfillsame.Checked == true)
        {
            DDRitemName.SelectedValue = DDItem.SelectedValue;
            DDRitemName_SelectedIndexChanged(sender, e);
        }


    }
    protected void DDRitemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(DDRquality, DDRdesign, DDRcolor, DDRshape, DDRShadecolor, Convert.ToInt16(DDRitemName.SelectedValue));
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkfillsame.Checked == true)
        {
            DDRquality.SelectedValue = DDQuality.SelectedValue;
        }
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkfillsame.Checked == true)
        {
            DDRdesign.SelectedValue = DDDesign.SelectedValue;
        }
    }
    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkfillsame.Checked == true)
        {
            DDRcolor.SelectedValue = DDColor.SelectedValue;
        }
    }

    protected void FillSize()
    {
        string size = "";
        string str = "";

        switch (DDsizetype.SelectedValue)
        {
            case "1":
                size = "Sizemtr";
                break;
            case "0":
                size = "Sizeft";
                break;
            case "2":
                size = "Sizeinch";
                break;
            default:
                size = "Sizeft";
                break;
        }
        //size Query

        str = "Select Distinct S.Sizeid,S." + size + " As  " + size + @" From Size S 
                 Where shapeid=" + DDShape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " order by " + size + "";

        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--SELECT--");
        //
        if (chkfillsame.Checked == true)
        {
            DDRshape.SelectedValue = DDShape.SelectedValue;
            DDRSizetype.SelectedValue = DDsizetype.SelectedValue;
            UtilityModule.ConditionalComboFill(ref DDRsize, str, true, "--SELECT--");
        }
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    private void FILL_SIZE_RECEIVE()
    {
        string Str, SizeString = "";

        switch (DDRSizetype.SelectedValue)
        {
            case "1":
                SizeString = "Sizemtr";
                break;
            case "0":
                SizeString = "Sizeft";
                break;
            case "2":
                SizeString = "Sizeinch";
                break;
            default:
                SizeString = "Sizeft";
                break;
        }
        Str = "SELECT SIZEID," + SizeString + " fROM SIZE WhERE SHAPEID=" + DDRshape.SelectedValue + " And MasterCompanyid=" + Session["varCompanyId"] + "";

        UtilityModule.ConditionalComboFill(ref DDRshape, Str, true, "--SELECT--");

    }
    protected void DDRshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FILL_SIZE_RECEIVE();
    }
    protected void DDRSizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FILL_SIZE_RECEIVE();
    }
    protected void DDgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (TDBinNo.Visible == true)
        //{
        //    int Varfinishedid = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        //    FillBinNo(Varfinishedid, sender);
        //}
        //else
        //{
        //    int Varfinishedid = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        //    FillLotno(Varfinishedid);
        //}
        int Varfinishedid = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        FillLotno(Varfinishedid);
    }

    protected void FillBinNo(int varfinishedid, object sender = null)
    {
        string str = "";
        str = "select Distinct BInNo,BinNo as BinNo1 From Stock Where ITEM_FINISHED_ID=" + varfinishedid + " and Companyid=" + DDCompanyName.SelectedValue + " and Godownid=" + DDgodown.SelectedValue + " And LotNo='" + DDLotno.SelectedItem.Text + "'";
        if (TDTagno.Visible == true)
        {
            str = str + " and TagNo='" + DDTagNo.SelectedItem.Text + "'";
        }
        if (MySession.Stockapply == "True")
        {
            str = str + " And Round(Qtyinhand,3)>0 ";
        }
        str = str + " order by BinNo1";
        UtilityModule.ConditionalComboFill(ref DDBinNo, str, true, "--Plz Select--");
        if (DDBinNo.Items.Count > 0)
        {
            DDBinNo.SelectedIndex = 1;
            if (sender != null)
            {
                DDBinNo_SelectedIndexChanged(sender, new EventArgs());
            }
        }
    }
    protected void FillLotno(int varfinishedid)
    {
        DDLotno.SelectedIndex = -1;
        string str = "";
        lblstockqty.Text = "";
        str = "select Distinct LotNo,LotNo as LotNo1 From Stock Where ITEM_FINISHED_ID=" + varfinishedid + " and Companyid=" + DDCompanyName.SelectedValue + " and Godownid=" + DDgodown.SelectedValue + "";
        if (MySession.Stockapply == "True")
        {
            str = str + " And Round(Qtyinhand,3)>0 ";
        }

        str = str + " order by LotNo1";

        UtilityModule.ConditionalComboFill(ref DDLotno, str, true, "--Plz Select--");
        //if (DDLotno.Items.Count > 0)
        //{
        //    DDLotno.SelectedIndex = 1;
        //    DDLotno_SelectedIndexChanged(DDLotno, new EventArgs());
        //}
    }
    protected void FillTagno(int varfinishedid)
    {
        DDTagNo.SelectedIndex = -1;
        string str = "";
        str = "select Distinct Tagno,Tagno as Tagno1 From Stock Where ITEM_FINISHED_ID=" + varfinishedid + " and Companyid=" + DDCompanyName.SelectedValue + " and Godownid=" + DDgodown.SelectedValue + " And LotNo='" + DDLotno.SelectedItem.Text + "'";
        if (MySession.Stockapply == "True")
        {
            str = str + " and Round(Qtyinhand,3)>0";
        }

        str = str + " order by Tagno1";

        UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "--Plz Select--");
        if (DDTagNo.Items.Count > 0)
        {
            DDTagNo.SelectedIndex = 1;
            DDTagNo_SelectedIndexChanged(DDTagNo, new EventArgs());
        }
    }
    protected void DDLotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Varfinishedid = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        if (TDTagno.Visible == true)
        {
            FillTagno(Varfinishedid);
        }
        else if (TDBinNo.Visible == true)
        {
            FillBinNo(Varfinishedid, sender);
        }
        else
        {
            FillstockQty(Varfinishedid);
        }

        if (chkfillsame.Checked == true)
        {
            txtreclotno.Text = DDLotno.SelectedItem.Text;
        }

    }
    protected void DDTagNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Varfinishedid = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        if (TDBinNo.Visible == true)
        {
            FillBinNo(Varfinishedid, sender);
        }
        else
        {
            FillstockQty(Varfinishedid);
        }
        if (chkfillsame.Checked == true)
        {
            txtrectagno.Text = DDTagNo.SelectedItem.Text;
        }
    }
    protected void FillstockQty(int varfinishedid)
    {
        string Lotno, TagNo = "";
        string BinNo = "";
        Lotno = DDLotno.SelectedItem.Text;
        if (TDTagno.Visible == true)
        {
            TagNo = DDTagNo.SelectedItem.Text;
        }
        else
        {
            TagNo = "Without Tag No";
        }
        if (TDBinNo.Visible == true)
        {
            BinNo = TDBinNo.Visible == true ? DDBinNo.SelectedItem.Text : "";
        }
        lblstockqty.Text = Convert.ToString(UtilityModule.getstockQty(DDCompanyName.SelectedValue, DDgodown.SelectedValue, Lotno, varfinishedid, TagNo, BinNo: BinNo));
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        double finalqty = Convert.ToDouble(txtrecqty.Text == "" ? "0" : txtrecqty.Text);
        //-Convert.ToDouble(TxtBellWeight.Text == "" ? "0" : TxtBellWeight.Text);
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[31];
            arr[0] = new SqlParameter("@ID", SqlDbType.Int);
            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = hnid.Value;
            arr[1] = new SqlParameter("@companyId", DDCompanyName.SelectedValue);
            arr[2] = new SqlParameter("@Processid", DDProcessName.SelectedValue);
            arr[3] = new SqlParameter("@empid", DDPartyName.SelectedValue);
            arr[4] = new SqlParameter("@IndentNo", SqlDbType.VarChar, 50);
            arr[4].Direction = ParameterDirection.InputOutput;
            arr[4].Value = TxtIndentNo.Text;
            arr[5] = new SqlParameter("@IssueDate", TxtDate.Text);
            arr[6] = new SqlParameter("@ReqDate", TxtReqDate.Text);
            arr[7] = new SqlParameter("@Mastercompanyid", Session["varcompanyId"]);
            arr[8] = new SqlParameter("@DetailId", SqlDbType.Int);
            arr[8].Value = 0;
            int varfinishedid = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, Tran, DDColorShade, "", Convert.ToInt32(Session["varCompanyId"]));
            arr[9] = new SqlParameter("@Ifinishedid", varfinishedid);
            arr[10] = new SqlParameter("@Iflagsize", DDsizetype.SelectedValue);
            arr[11] = new SqlParameter("@unitid", ddUnit.SelectedValue);
            arr[12] = new SqlParameter("@godownid", DDgodown.SelectedValue);
            arr[13] = new SqlParameter("@LotNo", DDLotno.SelectedItem.Text);
            arr[14] = new SqlParameter("@TagNo", TDTagno.Visible == false ? "Without Tag No" : DDTagNo.SelectedItem.Text);
            int varRfinishedid = UtilityModule.getItemFinishedId(DDRitemName, DDRquality, DDRdesign, DDRcolor, DDRshape, DDRsize, txtRprodcode, Tran, DDRShadecolor, "", Convert.ToInt32(Session["varCompanyId"]));
            arr[15] = new SqlParameter("@Rfinishedid", varRfinishedid);
            arr[16] = new SqlParameter("@Rflagsize", DDRSizetype.SelectedValue);
            arr[17] = new SqlParameter("@Caltype", DDcaltype.SelectedValue);
            arr[18] = new SqlParameter("@RecLotNo", txtreclotno.Text == "" ? "Without Lot No" : txtreclotno.Text);
            arr[19] = new SqlParameter("@RecTagNo", TDrectagno.Visible == false ? "Without Tag No" : (txtrectagno.Text == "" ? "Without Tag No" : txtrectagno.Text));
            arr[20] = new SqlParameter("@issueqty", finalqty);
            arr[21] = new SqlParameter("@Rate", txtrate.Text == "" ? "0" : txtrate.Text);
            arr[22] = new SqlParameter("@userid", Session["varuserid"]);
            arr[23] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[23].Direction = ParameterDirection.Output;
            arr[24] = new SqlParameter("@GatepassNo", SqlDbType.VarChar, 100);
            arr[24].Direction = ParameterDirection.InputOutput;
            arr[24].Value = txtgatepassno.Text;
            arr[25] = new SqlParameter("@DyeingMatch", TDDyeingMatch.Visible == true ? DDDyeingMatch.SelectedItem.Text : "");
            arr[26] = new SqlParameter("@Remark", (txtremark.Text).Trim());
            arr[27] = new SqlParameter("@BinNo", TDBinNo.Visible == true ? DDBinNo.SelectedItem.Text : "");
            arr[28] = new SqlParameter("@EWayBillNo", TxtEWayBillNo.Text);
            arr[29] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);
            arr[30] = new SqlParameter("@BellWeight", TxtBellWeight.Text == "" ? "0" : TxtBellWeight.Text);

            //**************************************************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveSampleDyeingMaster", arr);
            hnid.Value = arr[0].Value.ToString();
            TxtIndentNo.Text = arr[4].Value.ToString();
            txtgatepassno.Text = arr[24].Value.ToString();
            lblmsg.Text = arr[23].Value.ToString();
            Tran.Commit();

            DDRShadecolor.SelectedIndex = -1;
            txtrecqty.Text = "";
            txtrate.Text = "";
            FillstockQty(varfinishedid);
            Fillgrid();
            if (TDRShadecolor.Visible == true)
            {
                DDRShadecolor.Focus();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
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
        string str = @"select Sm.ID,Sd.Detailid,dbo.F_getItemDescription(Sd.Ifinishedid,sd.iflagsize) as IItemdescription,
                dbo.F_getItemDescription(Sd.Rfinishedid,sd.Rflagsize) as RItemdescription,SD.RecLotNo,SD.RectagNo,SD.issueqty,SD.Rate,SM.indentNo,Sm.gatepassNo,
                replace(convert(nvarchar(11),Sm.issueDate,106),' ','-') as issueDate,replace(convert(nvarchar(11),Sm.Reqdate,106),' ','-') as Reqdate, 
                Sm.remark,isnull(SM.EWayBillNo,'') as EWayBillNo
                From SampleDyeingmaster Sm inner join SampleDyeingDetail SD on Sm.ID=SD.masterid Where SM.CompanyID = " + DDCompanyName.SelectedValue + " And Sm.id=" + hnid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (chkedit.Checked == true)
            {
                TxtIndentNo.Text = ds.Tables[0].Rows[0]["IndentNo"].ToString();
                txtgatepassno.Text = ds.Tables[0].Rows[0]["GatepassNo"].ToString();
                TxtDate.Text = ds.Tables[0].Rows[0]["issuedate"].ToString();
                TxtReqDate.Text = ds.Tables[0].Rows[0]["reqdate"].ToString();
                txtremark.Text = ds.Tables[0].Rows[0]["Remark"].ToString();
                TxtEWayBillNo.Text = ds.Tables[0].Rows[0]["EWayBillNo"].ToString();
            }
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = "select * from v_sampledyeingissuereport where id=" + hnid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Session["VarCompanyId"].ToString() == "31")
            {
                Session["rptFileName"] = "~\\Reports\\rptsampledyeingissueOPEE.rpt";
            }
            else if (Session["VarCompanyId"].ToString() == "42")
            {
                Session["rptFileName"] = "~\\Reports\\rptsampledyeingissueVikarmCarpet.rpt";
            }
            else if (Session["VarCompanyId"].ToString() == "43")
            {
                Session["rptFileName"] = "~\\Reports\\rptsampledyeingissueCarpetInternational.rpt";
            }
            else if (Session["VarCompanyId"].ToString() == "22")
            {
                Session["rptFileName"] = "~\\Reports\\rptsampledyeingissueDiamond.rpt";
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\rptsampledyeingissue.rpt";
            }
           
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptsampledyeingissue.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        BtnUpdateRemark.Visible = false;
        chkcomplete.Checked = false;
        TDIndentNo.Visible = false;
        hnid.Value = "0";
        TxtIndentNo.Text = "";
        txtgatepassno.Text = "";
        TxtDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        TxtReqDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        DDPartyName.SelectedIndex = -1;
        if (chkedit.Checked == true)
        {
            DDindentNo.Items.Clear();
            TDIndentNo.Visible = true;
            BtnUpdateRemark.Visible = true;
        }
        DG.DataSource = null;
        DG.DataBind();
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkedit.Checked == true)
        {
            string str = @"select ID,indentNo 
            From SampleDyeingmaster 
            Where Companyid=" + DDCompanyName.SelectedValue + " and processid=" + DDProcessName.SelectedValue + @" And 
                BranchID = " + DDBranchName.SelectedValue + " And empid=" + DDPartyName.SelectedValue;
            if (chkcomplete.Checked == true)
            {
                str = str + " and status='Complete'";
            }
            else
            {
                str = str + " and status='Pending'";
            }
            str = str + "  order by id desc";

            UtilityModule.ConditionalComboFill(ref DDindentNo, str, true, "--Plz Select--");
        }
    }
    protected void DDindentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = DDindentNo.SelectedValue;
        Fillgrid();
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
        lblmsg.Text = "";
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
            TextBox txtRate = (TextBox)DG.Rows[e.RowIndex].FindControl("txtRate");
            //*************
            SqlParameter[] arr = new SqlParameter[8];
            arr[0] = new SqlParameter("@ID", lblid.Text);
            arr[1] = new SqlParameter("@Detailid", lbldetailid.Text);
            arr[2] = new SqlParameter("@qty", txtqty.Text == "" ? "0" : txtqty.Text);
            arr[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[3].Direction = ParameterDirection.Output;
            arr[4] = new SqlParameter("@userid", Session["varuserid"]);
            arr[5] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            arr[6] = new SqlParameter("@EWayBillNo", TxtEWayBillNo.Text);
            arr[7] = new SqlParameter("@Rate", txtRate.Text == "" ? "0" : txtRate.Text);
            //*******
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_updatesampledyeing", arr);
            lblmsg.Text = arr[3].Value.ToString();
            Tran.Commit();
            DG.EditIndex = -1;
            Fillgrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void DG_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblmsg.Text = "";
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
            SqlParameter[] arr = new SqlParameter[3];
            arr[0] = new SqlParameter("@Detailid", lblDetailid.Text);
            arr[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[1].Direction = ParameterDirection.Output;
            arr[2] = new SqlParameter("@ID", lblid.Text);
            //***********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_Deletesampledyeing", arr);
            lblmsg.Text = arr[1].Value.ToString();
            Tran.Commit();
            Fillgrid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }

    protected void btngatepass_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            string str = "select * from V_SampleYarnGatepass where id=" + hnid.Value;
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\rptsampledyeingissueGatepassVikarmCarpet.rpt";
                Session["rptFileName"] = "~\\Reports\\rptsampledyeingissueGatepass.rpt";
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptsampledyeingissueGatepass.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "o", "alert('No records found!!!');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void DDColorShade_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDgodown_SelectedIndexChanged(sender, new EventArgs());
    }

    protected void DDBinNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Varfinishedid = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        FillstockQty(Varfinishedid);
    }
    protected void BtnUpdateRemark_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[6];

            arr[0] = new SqlParameter("@ID", SqlDbType.Int);
            arr[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
            arr[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            arr[3] = new SqlParameter("@userid", Session["varuserid"]);
            arr[4] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            arr[5] = new SqlParameter("@Remark", txtremark.Text);


            arr[0].Value = DDindentNo.SelectedValue;
            arr[1].Value = DDProcessName.SelectedValue;
            arr[2].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAMPLE_DYEING_UPDATE_REMARK", arr);

            lblmsg.Text = arr[2].Value.ToString();
            Tran.Commit();
            Fillgrid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
}