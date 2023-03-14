using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Process_frmEditDirectProductionNew : System.Web.UI.Page
{
    public static int ItemFinishedId;
    public static int IssueOrderId = 0;
    public static int OrderId;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select UnitId,UnitName from Unit  Where UnitId in(1,2)
             select IC.Category_Id,Category_Name from Item_Category_Master IC,CategorySeparate CS Where IC.Category_Id=CS.CategoryId And id=0 order by IC.Category_Id
             select U.UnitsId,U.UnitName from Units U inner join Units_authentication UA on U.unitsId=UA.UnitsId and UA.Userid=" + Session["varuserid"] + " order by U.unitsId";

            ////////////////////////
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(str);
            //Fill DropDown Vendor And Unit
            // UtilityModule.ConditionalComboFillWithDS(ref DDvendorName, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDunit, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddUnits, ds, 2, false, "");
            if (DDunit.Items.Count > 0)
            {
                DDunit.SelectedIndex = 1;
            }
            if (ddCatagory.Items.Count > 0)
            {
                ddCatagory.SelectedIndex = 1;
                ddCatagory_SelectedIndexChanged(sender, e);

            }
            ddUnits.SelectedIndex = 0;
            DDcaltype.SelectedIndex = 1;
            TxtAssignDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtRequiredDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            //lablechange();
            ViewState["IssueOrderId"] = 0;
        }
    }

    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        lblsize.Text = ParameterList[4];
        lblCategory.Text = ParameterList[5];
        lblItemName.Text = ParameterList[6];
        lblshadecolor.Text = ParameterList[7];
    }
    private void ddlcategorycange()
    {
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
        TDShape.Visible = false;
        TDShade.Visible = false;
        TdSize.Visible = false;
        //TdArea.Visible = false;

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
                        TDQuality.Visible = true;
                        break;
                    case "2":
                        TDDesign.Visible = true;
                        break;
                    case "3":
                        TDColor.Visible = true;
                        break;
                    case "4":
                        TDShape.Visible = true;
                        break;
                    case "5":
                        TdSize.Visible = true;
                        if (Session["VarcompanyNo"].ToString() == "7")
                        {
                            TdArea.Visible = false;

                        }
                        else
                        {
                            // TdArea.Visible = true;

                        }
                        break;
                    case "6":
                        TDShade.Visible = true;
                        break;
                }
            }
        }
        UtilityModule.ConditionalComboFill(ref dditemname, "select Item_id, Item_Name from Item_Master where Category_Id=" + ddCatagory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Item_Name", true, "---Plz Select----");

    }
    private void fillCombo()
    {
        if (TDDesign.Visible == true)
        {

            UtilityModule.ConditionalComboFill(ref dddesign, @"select designid,Designname from Design Order by Designname ", true, "--Plz Select--");
        }
        if (TDColor.Visible == true)
        {

            UtilityModule.ConditionalComboFill(ref ddcolor, @"select ColorId,ColorName from Color Order by ColorName", true, "--Plz Select--");
        }
        if (TDShape.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddshape, "SELECT ShapeId,Shapename from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " order by shapename", true, "--Plz Select--");
            if (ddshape.Items.Count > 0)
            {
                ddshape.SelectedIndex = 1;
                if (Convert.ToInt32(DDunit.SelectedValue) == 6)
                {
                    UtilityModule.ConditionalComboFill(ref ddsize, "SELECT SizeId,Sizeinch Size_Name from Size where shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Sizeinch", true, "--Plz Select--");
                }
                else if (Convert.ToInt32(DDunit.SelectedValue) == 2)
                {
                    //UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SizeId,SizeFt Size_Name from Size where shapeid=" + DDShape.SelectedValue + " order by sizeid", true, "--SELECT--");
                    UtilityModule.ConditionalComboFill(ref ddsize, "Select S.Sizeid,SizeFt from Size S Where shapeid=" + ddshape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " order by S.SizeFt", true, "--Plz Select--");
                }
                else
                {
                    // UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SizeId,SizeMtr Size_Name from Size where shapeid=" + DDShape.SelectedValue + " order by sizeid", true, "--SELECT--");
                    UtilityModule.ConditionalComboFill(ref ddsize, "select S.Sizeid,SizeMtr from Size S where shapeid=" + ddshape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " order by S.SizeMtr", true, "--Plz Select--");
                }

            }
        }
        if (TDShade.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddshape, "SELECT * from ShadeColor Where MasterCompanyId=" + Session["varCompanyId"] + " order by ShadeColorName", true, "--Plz Select--");
        }
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
        fillCombo();
    }
    private void shapeselectedindexchange()
    {
        if (ddshape.SelectedIndex > 0)
        {
            if (Convert.ToInt32(DDunit.SelectedValue) == 6)
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "SELECT SizeId,Sizeinch Size_Name from Size where shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Sizeinch", true, "--Plz Select--");
            }
            else if (Convert.ToInt32(DDunit.SelectedValue) == 2)
            {
                //UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SizeId,SizeFt Size_Name from Size where shapeid=" + DDShape.SelectedValue + " order by sizeid", true, "--SELECT--");
                UtilityModule.ConditionalComboFill(ref ddsize, "Select S.Sizeid,SizeFt from Size S Where shapeid=" + ddshape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " order by S.SizeFt", true, "--Plz Select--");
            }
            else
            {
                // UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SizeId,SizeMtr Size_Name from Size where shapeid=" + DDShape.SelectedValue + " order by sizeid", true, "--SELECT--");
                UtilityModule.ConditionalComboFill(ref ddsize, "select S.Sizeid,SizeMtr from Size S where shapeid=" + ddshape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " order by S.SizeMtr", true, "--Plz Select--");
            }

        }

    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        shapeselectedindexchange();
    }
    private void Area()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            //DataSet dt;
            DataSet Ds;
            //int ItemFinishedId = UtilityModule.getItemFinishedId(dditemname, ddquality, dddesign, ddcolor, ddshape, ddsize, TxtProductCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            // Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "select size_Id,SHAPE_ID from Item_Parameter_Master where Item_Finished_Id=" + ItemFinishedId + " And MasterCompanyId=" + Session["varCompanyId"] + "");
            int SizeId = Convert.ToInt32((ddsize.SelectedValue == "" || ddsize.SelectedValue == null ? "0" : ddsize.SelectedValue));

            //if (Ds.Tables[0].Rows.Count > 0)
            ////{           

            if (SizeId != 0 && hncomp.Value != "6")
            {

                //TdArea.Visible = true;
                SqlParameter[] _arrpara = new SqlParameter[6];
                _arrpara[0] = new SqlParameter("@size_Id", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@UnitTypeId", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@Length", SqlDbType.Float);
                _arrpara[3] = new SqlParameter("@width", SqlDbType.Float);
                _arrpara[4] = new SqlParameter("@Area", SqlDbType.Float);
                _arrpara[5] = new SqlParameter("@ShapeId", SqlDbType.Int);

                _arrpara[0].Value = SizeId;
                _arrpara[1].Value = DDunit.SelectedValue;
                _arrpara[2].Direction = ParameterDirection.Output;
                _arrpara[3].Direction = ParameterDirection.Output;
                _arrpara[4].Direction = ParameterDirection.Output;
                _arrpara[5].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_Area", _arrpara);
                //TxtLength.Text = string.Format("{0:#0.00}", _arrpara[2].Value);
                //TxtWidth.Text = string.Format("{0:#0.00}", _arrpara[3].Value);
                TxtLength.Text = _arrpara[2].Value.ToString();
                TxtWidth.Text = _arrpara[3].Value.ToString();
                TxtArea.Text = string.Format("{0:#0.0000}", _arrpara[4].Value);
                int Shapeid = Convert.ToInt16(_arrpara[5].Value);

                //hdArea.Value = string.Format("{0:#0.0000}", _arrpara[4].Value);
                if (Convert.ToInt32(DDunit.SelectedValue) == 1)
                {
                    TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Shapeid));
                    //hdArea.Value = TxtArea.Text;
                }
                if (Convert.ToInt32(DDunit.SelectedValue) == 2)
                {
                    TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Shapeid));
                    // hdArea.Value = TxtArea.Text;
                }
            }
            else if (SizeId != 0 && hncomp.Value == "6")
            {
                //datatset dt1 = SqlHelper.ExecuteDataset(con, CommandType.Text, "");
                string str = "";
                str = "select WidthFt,LengthFt,HeightFt,WidthMtr,LengthMtr,HeightMtr,AreaFt,AreaMtr from size where sizeid=" + SizeId + " And MasterCompanyId=" + Session["varCompanyId"];
                Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
                if (DDunit.SelectedValue == "2")
                {
                    TxtLength.Text = string.Format("{0:#0.00}", Ds.Tables[0].Rows[0]["LengthFt"].ToString());
                    TxtWidth.Text = string.Format("{0:#0.00}", Ds.Tables[0].Rows[0]["WidthFt"].ToString());
                    TxtArea.Text = string.Format("{0:#0.0000}", Ds.Tables[0].Rows[0]["AreaFt"].ToString());
                    //hdArea.Value = TxtArea.Text;
                }
                else
                {
                    TxtLength.Text = string.Format("{0:#0.00}", Ds.Tables[0].Rows[0]["LengthMtr"].ToString());
                    TxtWidth.Text = string.Format("{0:#0.00}", Ds.Tables[0].Rows[0]["Widthmtr"].ToString());
                    decimal area;
                    area = Convert.ToDecimal((Convert.ToDecimal(TxtLength.Text) * Convert.ToDecimal(TxtWidth.Text) * Convert.ToDecimal(10.764)) / 10000);
                    TxtArea.Text = string.Format("{0:#0.0000}", area);
                    //hdArea.Value = TxtArea.Text;
                }

            }
            else
            {
                TdArea.Visible = false;
                TxtArea.Text = "0";
                //hdArea.Value = TxtArea.Text;
            }
            //datatset dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "");
            // }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssue.aspx");
        }
        finally
        {
            con.Close();
        }
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemFinishedId = UtilityModule.getItemFinishedId(dditemname, ddquality, dddesign, ddcolor, ddshape, ddsize, TxtProductCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        Area();
        //1 For Fix Weaving
        //TxtRate.Text = MasterRate(ItemFinishedId, 1).ToString();
        TxtRate.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "exec Pro_getJobrate " + ItemFinishedId + ",1," + ddUnits.SelectedValue + "").ToString();
        LblErrorMessage.Text = "";
        if (TxtRate.Text == "")
        {
            LblErrorMessage.Text = "Rate is not Define.Please Define rate first..";
        }
    }
    public static double MasterRate(int ItemFinishedId, int PROCESS_ID)
    {
        DataSet Ds1;
        Double VarRate = 0;

        string Str = "Select * from Item_ParaMeter_Master IPM,ITem_Master IM Where IPM.Item_Id=IM.Item_Id And IPM.Item_Finished_ID=" + ItemFinishedId + "";
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And ProcessID=" + PROCESS_ID + ")";
            Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (Ds1.Tables[0].Rows.Count == 0)
            {
                Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=-1 And ProcessID=" + PROCESS_ID + ")";
                Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            }
            if (Ds1.Tables[0].Rows.Count == 0)
            {
                Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=-1 And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And ProcessID=" + PROCESS_ID + ")";
                Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            }
            if (Ds1.Tables[0].Rows.Count == 0)
            {
                Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID AND Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=-1 And Size_ID=-1 And ProcessID=" + PROCESS_ID + ")";
                Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            }
            if (Ds1.Tables[0].Rows.Count == 0)
            {
                Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And ProcessID=" + PROCESS_ID + ")";
                Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            }
            if (Ds1.Tables[0].Rows.Count == 0)
            {
                Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=-1 And ProcessID=" + PROCESS_ID + ")";
                Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            }
            if (Ds1.Tables[0].Rows.Count == 0)
            {
                Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=-1 And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And ProcessID=" + PROCESS_ID + ")";
                Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            }
            if (Ds1.Tables[0].Rows.Count == 0)
            {
                Str = "Select ORate from PROCESSCONSUMPTIONDETAIL Where PCMID in (Select PM.PCMID from ITEM_PARAMETER_MASTER IPCM,PROCESSCONSUMPTIONMASTER PM,ITEM_MASTER IM Where IPCM.ITEM_FINISHED_ID=PM.FINISHEDID And IM.ITEM_ID=IPCM.ITEM_ID And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=-1 And Size_ID=-1 And ProcessID=" + PROCESS_ID + ")";
                Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            }
            if (Ds1.Tables[0].Rows.Count > 0)
            {
                VarRate = Convert.ToDouble(Ds1.Tables[0].Rows[0]["ORATE"]);
            }
        }
        return Math.Round(VarRate, 2);
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        string StrEmpid = "";
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {

            if (IssueOrderId.ToString() == "" || IssueOrderId == null)
            {
                IssueOrderId = 0;
            }
            if (ViewState["IssueOrderId"] == null)
            {
                ViewState["IssueOrderId"] = 0;
            }
            ItemFinishedId = UtilityModule.getItemFinishedId(dditemname, ddquality, dddesign, ddcolor, ddshape, ddsize, TxtProductCode, Tran, ddlshade, "", Convert.ToInt32(Session["varCompanyId"]));
            //string str = "select Item_Finished_id From Process_Issue_Detail_1 Where IssueOrderId=" + ViewState["IssueOrderId"] + " And Item_Finished_id=" + ItemFinishedId + "";
            //DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    LblErrorMessage.Text = "Item Already Exists.............";
            //    Tran.Commit();
            //    return;
            //}
            SqlParameter[] _arrpara = new SqlParameter[37];
            _arrpara[0] = new SqlParameter("@IssueOrderid", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@Empid", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@Assign_Date", SqlDbType.SmallDateTime);
            _arrpara[3] = new SqlParameter("@Status", SqlDbType.NVarChar);
            _arrpara[4] = new SqlParameter("@UnitId", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@User_Id", SqlDbType.Int);
            _arrpara[6] = new SqlParameter("@Remarks", SqlDbType.NVarChar);
            _arrpara[7] = new SqlParameter("@Instruction", SqlDbType.NVarChar);
            _arrpara[8] = new SqlParameter("@Companyid", SqlDbType.Int);

            _arrpara[9] = new SqlParameter("@Issue_Detail_Id", SqlDbType.Int);
            _arrpara[10] = new SqlParameter("@Item_Finished_id", SqlDbType.Int);
            _arrpara[11] = new SqlParameter("@Length", SqlDbType.NVarChar);
            _arrpara[12] = new SqlParameter("@Width", SqlDbType.NVarChar);
            _arrpara[13] = new SqlParameter("@Area", SqlDbType.Float);
            _arrpara[14] = new SqlParameter("@Rate", SqlDbType.Float);
            _arrpara[15] = new SqlParameter("@Amount", SqlDbType.Float);
            _arrpara[16] = new SqlParameter("@Qty", SqlDbType.Int);
            _arrpara[17] = new SqlParameter("@ReqByDate", SqlDbType.SmallDateTime);
            _arrpara[18] = new SqlParameter("@PQty", SqlDbType.Int);

            _arrpara[19] = new SqlParameter("@Comm", SqlDbType.Float);
            _arrpara[20] = new SqlParameter("@CommAmt", SqlDbType.Float);
            _arrpara[21] = new SqlParameter("@Orderid", SqlDbType.Int);
            _arrpara[22] = new SqlParameter("@CalType", SqlDbType.Int);
            _arrpara[23] = new SqlParameter("@Freight", SqlDbType.Int);
            _arrpara[24] = new SqlParameter("@Insurance", SqlDbType.Int);
            _arrpara[25] = new SqlParameter("@PaymentAt", SqlDbType.Int);
            _arrpara[26] = new SqlParameter("@Destination", SqlDbType.NVarChar, 100);
            _arrpara[27] = new SqlParameter("@Liasoning", SqlDbType.NVarChar, 50);
            _arrpara[28] = new SqlParameter("@Inspection", SqlDbType.NVarChar, 50);
            _arrpara[29] = new SqlParameter("@SampleNumber", SqlDbType.NVarChar, 100);
            _arrpara[30] = new SqlParameter("@FlagFixOrWeight", SqlDbType.Int);
            _arrpara[31] = new SqlParameter("@FromProcessId", SqlDbType.Int);
            _arrpara[32] = new SqlParameter("@ItemId", SqlDbType.Int);
            _arrpara[33] = new SqlParameter("@OrderDetailId", SqlDbType.Int);
            _arrpara[34] = new SqlParameter("@StrEmpid", SqlDbType.VarChar, 50);
            _arrpara[35] = new SqlParameter("@Units", SqlDbType.Int);//unit For Anisha
            _arrpara[36] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);//unit For Anisha


            //_arrpara[0].Value = IssueOrderId;
            _arrpara[0].Value = ViewState["IssueOrderId"];
            _arrpara[0].Direction = ParameterDirection.InputOutput;
            _arrpara[1].Value = 0;// DDvendorName.SelectedValue;
            _arrpara[2].Value = TxtAssignDate.Text;
            _arrpara[3].Value = "Pending";
            _arrpara[4].Value = DDunit.SelectedValue;
            _arrpara[5].Value = Session["varuserid"];
            _arrpara[6].Value = TxtRemarks.Text.ToUpper();
            _arrpara[7].Value = TxtInstructions.Text.ToUpper();
            _arrpara[8].Value = Session["CurrentWorkingCompanyID"];
            _arrpara[9].Direction = ParameterDirection.InputOutput;
            _arrpara[9].Value = 0;  //IssueDetailId
            _arrpara[10].Value = ItemFinishedId;
            _arrpara[11].Value = TxtLength.Text;
            _arrpara[12].Value = TxtWidth.Text;
            _arrpara[13].Value = TxtArea.Text;

            _arrpara[14].Value = TxtRate.Text == "" ? "0" : TxtRate.Text;
            if (DDcaltype.SelectedValue == "0" || DDcaltype.SelectedValue == "2" || DDcaltype.SelectedValue == "3" || DDcaltype.SelectedValue == "4")
            {
                _arrpara[15].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtRate.Text == "" ? "0" : TxtRate.Text) * Convert.ToDouble(TxtQtyRequired.Text)));
                _arrpara[20].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtCommission.Text == "" ? "0" : TxtCommission.Text) * Convert.ToDouble(TxtQtyRequired.Text)));
            }
            if (DDcaltype.SelectedValue == "1")
            {
                _arrpara[15].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtRate.Text == "" ? "0" : TxtRate.Text) * Convert.ToDouble(TxtQtyRequired.Text)));
                _arrpara[20].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtCommission.Text == "" ? "0" : TxtCommission.Text) * Convert.ToDouble(TxtQtyRequired.Text)));
            }
            _arrpara[16].Value = TxtQtyRequired.Text == "" ? "0" : TxtQtyRequired.Text;
            _arrpara[17].Value = TxtAssignDate.Text;//TxtRequiredDate.Text;
            _arrpara[18].Value = TxtQtyRequired.Text == "" ? "0" : TxtQtyRequired.Text;
            _arrpara[19].Value = TxtCommission.Text == "" ? "0" : TxtCommission.Text;
            _arrpara[21].Direction = ParameterDirection.InputOutput;
            if (OrderId.ToString() == "" || OrderId == null)
            {
                OrderId = 0;
            }
            _arrpara[21].Value = OrderId;
            _arrpara[22].Value = DDcaltype.SelectedValue;

            _arrpara[23].Value = 0;
            _arrpara[24].Value = 0;
            _arrpara[25].Value = 0;
            _arrpara[26].Value = "";
            _arrpara[27].Value = "";
            _arrpara[28].Value = "";
            _arrpara[29].Value = "";
            _arrpara[30].Value = 1;//For Fix
            //Fix For Weaving
            _arrpara[31].Value = 1;
            _arrpara[32].Value = dditemname.SelectedValue;
            _arrpara[33].Direction = ParameterDirection.Output;
            //Find EmployeeId
            for (int i = 0; i < listWeaverName.Items.Count; i++)
            {
                if (StrEmpid == "")
                {
                    StrEmpid = listWeaverName.Items[i].Value;
                }
                else
                {
                    StrEmpid = StrEmpid + "," + listWeaverName.Items[i].Value;
                }
            }
            //Check Employee Entry
            if (StrEmpid == "")
            {
                LblErrorMessage.Text = "Plz Enter Weaver ID No...";
                Tran.Commit();
                return;

            }
            _arrpara[34].Value = @StrEmpid;
            _arrpara[35].Value = ddUnits.SelectedIndex < 0 ? "0" : ddUnits.SelectedValue;
            _arrpara[36].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_DirectProduction]", _arrpara);
            //IssueOrderId = Convert.ToInt16(_arrpara[0].Value);
            ViewState["IssueOrderId"] = Convert.ToInt32(_arrpara[0].Value);
            TxtChallanNo.Text = _arrpara[0].Value.ToString();
            OrderId = Convert.ToInt16(_arrpara[21].Value);
            Tran.Commit();
            TxtQtyRequired.Text = "";
            TxtRate.Text = "";
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Save", "alert('" + _arrpara[36].Value.ToString() + "');", true);
            Fill_Grid();
            BtnPreview.Enabled = true;
            //            LblErrorMessage.Text = "Data Successfully Saved.......";

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void Fill_Grid()
    {
        string sqlstr = @"Select Issue_Detail_Id as IssueDetailId,vf.Category_Name as Category,vf.Item_Name as Articles,vf.ColorName As Colour,Length,Width,
                        Width + 'x' + Length Size,Area,Rate,Qty,Amount,OrderId,PD.Item_Finished_Id,PM.IssueOrderId From PROCESS_ISSUE_MASTER_1 PM,PROCESS_ISSUE_DETAIL_1 PD,
                        V_FinishedItemDetail vf
                        Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=vf.Item_Finished_id  
                        And PM.IssueOrderid=" + ViewState["IssueOrderId"] + " And vf.MasterCompanyId=" + Session["varCompanyId"] + " Order By Issue_Detail_Id Desc";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {

            DataSet DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
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
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    private DataSet GetDetail()
    {
        DataSet DS = null;
        string sqlstr = "";

        sqlstr = @"Select Issue_Detail_Id as IssueDetailId,vf.Category_Name as Category,vf.Item_Name as Articles,vf.ColorName As Colour,Length,Width,
                        Length + 'x' + Width Size,Area,Rate,Qty,Amount From PROCESS_ISSUE_MASTER_1 PM,PROCESS_ISSUE_DETAIL_1 PD,
                        V_FinishedItemDetail vf
                        Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=vf.Item_Finished_id  
                        And PM.IssueOrderid=" + ViewState["IssueOrderId"] + " And vf.MasterCompanyId=" + Session["varCompanyId"] + " Order By Issue_Detail_Id Desc";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
            if (DS.Tables[0].Rows.Count == 0)
            {
                DS = null;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/DirectProduction.aspx");
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return DS;
    }

    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _array = new SqlParameter[4];
            _array[0] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
            _array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
            _array[2] = new SqlParameter("@Trantype", SqlDbType.Int);

            _array[0].Value = ViewState["IssueOrderId"];
            _array[1].Value = 1;//For IST Process
            _array[2].Value = 0; //For Issue

            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "[Pro_OrderFolio_IssuedSlip]", _array);

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
            else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void txtWeaverIdNo_TextChanged(object sender, EventArgs e)
    {
        string str = "";
        SqlConnection con = new SqlConnection(ErpGlobalNew.DBCONNECTIONSTRINGNEW);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            if (txtWeaverIdNo.Text != "")
            {
                str = @"select Emp.Empid,PM.IssueOrderId 
                        From process_issue_Master_1 PM,Process_issue_Detail_1 PD,Employee_ProcessOrderNo EMP,Empinfo EI
                        Where PM.IssueOrderid=PD.IssueOrderid And PM.issueOrderid=EMP.IssueOrderid And Processid=1 And EMP.EMPID=EI.EmpID
                        And PM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And EI.EmpCode='" + txtWeaverIdNo.Text + "' And pqty>0 ";
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "AlertEmp", "alert('Folio-" + ds.Tables[0].Rows[0]["IssueOrderId"] + " Already pending at this ID No..');", true);
                    return;
                }
                //DataRow[] rows = AllEnums.MasterTables.Empinfo.ToTable().Select("EmpCode='" + txtWeaverIdNo.Text + "'");
                //foreach (DataRow dr in rows)
                //{

                //    listWeaverName.Items.Add(new ListItem("" + dr["Empname"] + "", dr["Empid"].ToString()));
                //    txtWeaverIdNo.Text = "";

                //}
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Empid,Empname from empinfo Where Empcode='" + txtWeaverIdNo.Text + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (listWeaverName.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) == null)
                    {

                        listWeaverName.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Empname"].ToString(), ds.Tables[0].Rows[0]["Empid"].ToString()));
                    }

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
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }

    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        listWeaverName.Items.Remove(listWeaverName.SelectedItem);
    }
    protected void txtPoNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtPoNo.Text != "")
            {
                DGOrderdetail.DataSource = null;
                DGOrderdetail.DataBind();
                listWeaverName.Items.Clear();
                LblErrorMessage.Text = "";
                SqlParameter[] array = new SqlParameter[3];
                array[0] = new SqlParameter("@IssueOrderid", SqlDbType.Int);
                array[1] = new SqlParameter("@CompanyID", SqlDbType.Int);
                array[2] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);

                array[0].Value = txtPoNo.Text;
                array[1].Value = Session["CurrentWorkingCompanyID"];
                array[2].Value = Session["varcompanyId"];

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_fillProductionOrderBack", array);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //fill employee
                    ViewState["IssueOrderId"] = ds.Tables[0].Rows[0]["IssueOrderid"].ToString();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        listWeaverName.Items.Add(new ListItem("" + ds.Tables[0].Rows[i]["EmpName"] + "", ds.Tables[0].Rows[i]["Empid"].ToString()));
                        txtWeaverIdNo.Text = "";
                    }
                    //fill master Detail
                    ddUnits.SelectedValue = ds.Tables[1].Rows[0]["units"].ToString();
                    TxtAssignDate.Text = ds.Tables[1].Rows[0]["AssignDate"].ToString();
                    TxtRequiredDate.Text = ds.Tables[1].Rows[0]["AssignDate"].ToString();
                    TxtChallanNo.Text = ds.Tables[1].Rows[0]["IssueOrderId"].ToString();
                    DGOrderdetail.DataSource = ds.Tables[1];
                    DGOrderdetail.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    txtPoNo.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = "";
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
        }
    }

    protected void DGOrderdetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGOrderdetail.EditIndex = e.NewEditIndex;
        Fill_Grid();

    }
    protected void DGOrderdetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGOrderdetail.EditIndex = -1;

        Fill_Grid();
    }
    protected void DGOrderdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
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
            SqlParameter[] array = new SqlParameter[8];
            array[0] = new SqlParameter("@IssueOrderId ", SqlDbType.BigInt);
            array[1] = new SqlParameter("@IssueDetailId", SqlDbType.BigInt);
            array[2] = new SqlParameter("@Qty", SqlDbType.Int);
            array[3] = new SqlParameter("@ItemFinishedid", SqlDbType.Int);
            array[4] = new SqlParameter("@OrderId", SqlDbType.Int);
            array[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 50);
            array[6] = new SqlParameter("@MastercompanyId", SqlDbType.Int);
            array[7] = new SqlParameter("@Userid", SqlDbType.Int);

            array[0].Value = ((Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblissueOrderId")).Text;
            array[1].Value = Convert.ToInt32(DGOrderdetail.DataKeys[e.RowIndex].Value);
            array[2].Value = ((TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtQty")).Text;
            array[3].Value = ((Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblItemFinishedId")).Text;
            array[4].Value = ((Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblOrderId")).Text;
            array[5].Direction = ParameterDirection.Output;
            array[6].Value = Session["varcompanyId"].ToString();
            array[7].Value = Session["varuserid"].ToString();

            //Check Qty
            if (Convert.ToInt16(((TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtQty")).Text) <= 0)
            {
                LblErrorMessage.Text = "Qty can not less than or equal to Zero....";
                Tran.Commit();
                return;
            }
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateDirectProduction", array);
            LblErrorMessage.Text = array[5].Value.ToString();
            Tran.Commit();
            DGOrderdetail.EditIndex = -1;
            Fill_Grid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            LblErrorMessage.Text = ex.Message;

        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void DGOrderdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] array = new SqlParameter[5];
            array[0] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
            array[1] = new SqlParameter("@IssueDetailId", SqlDbType.Int);
            array[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            array[3] = new SqlParameter("@MastercompanyId", SqlDbType.Int);
            array[4] = new SqlParameter("@Userid", SqlDbType.Int);

            array[0].Value = ((Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblissueOrderId")).Text;
            array[1].Value = Convert.ToInt32(DGOrderdetail.DataKeys[e.RowIndex].Value);
            array[2].Direction = ParameterDirection.Output;
            array[3].Value = Session["varcompanyId"].ToString();
            array[4].Value = Session["varuserid"].ToString();

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteProductionOrder", array);

            Tran.Commit();
            LblErrorMessage.Text = array[2].Value.ToString();
            Fill_Grid();

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void btnchangeWeaver_Click(object sender, EventArgs e)
    {
        if (chkchangeWeaver.Checked == true)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                if (DGOrderdetail.Rows.Count == 0)
                {
                    Tran.Commit();
                    return;
                }
                string str = "";
                //Check foliostatus
                str = "select status From process_issue_master_1 Where issueorderid=" + ViewState["IssueOrderId"] + "";
                DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (string.Equals(ds.Tables[0].Rows[0]["status"].ToString(), "complete", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn2", "alert('Folio is completed.So you can not change Weaver ID')", true);
                        Tran.Commit();
                        return;
                    }
                    else if (string.Equals(ds.Tables[0].Rows[0]["status"].ToString(), "canceled", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn3", "alert('Folio is canceled.So you can not change Weaver ID')", true);
                        Tran.Commit();
                        return;
                    }

                }

                //
                //Delete And Update Existing record
                str = @"Delete from Employee_ProcessOrderNo Where IssueOrderId=" + ViewState["IssueOrderId"] + @" And ProcessId=1
                      Update Process_Issue_Master_1 set Units=" + ddUnits.SelectedValue + " where IssueOrderId=" + ViewState["IssueOrderId"];

                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                str = "";
                //***************
                for (int i = 0; i < listWeaverName.Items.Count; i++)
                {
                    for (int j = 0; j < DGOrderdetail.Rows.Count; j++)
                    {
                        str = str + " Insert into Employee_ProcessOrderNo (ProcessId,IssueOrderId,IssueDetailId,Empid,userid)values(1," + ((Label)DGOrderdetail.Rows[j].FindControl("lblissueOrderId")).Text + "," + DGOrderdetail.DataKeys[j].Value + "," + listWeaverName.Items[i].Value + "," + Session["varuserid"] + ")";
                    }
                }
                if (str != "")
                {
                    str = str + @" insert into UpdateStatus(Id,CompanyId,UserId,Tablename,TableId,Date,Status)
                                    values((select max(id)+1 from UpdateStatus)," + Session["varcompanyid"] + "," + Session["varuserid"] + ",'Process_Issue_Master_1'," + ViewState["IssueOrderId"] + ",GETDATE(),'Update Employee')";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                }
                //**************
                Tran.Commit();
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('Data saved successfully...')", true);
                //**************
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
    }
}