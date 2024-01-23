using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Process_frmDirectProductionorderNew : System.Web.UI.Page
{
    public static int ItemFinishedId;
    public static int IssueOrderId = 0;
    public static int OrderId;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
             string str = @"select UnitId,UnitName from Unit  Where UnitId in(1,2)
             select IC.Category_Id,Category_Name from Item_Category_Master IC,CategorySeparate CS Where IC.Category_Id=CS.CategoryId And id=0 order by IC.Category_Id
             select U.UnitsId,U.UnitName from Units U inner join Units_authentication UA on U.unitsId=UA.UnitsId and UA.Userid=" + Session["varuserid"] + " order by U.unitsId";
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(str);
            //Fill DropDown Vendor And Unit            
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
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
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
                        IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCatagory.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
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
        UtilityModule.ConditionalComboFill(ref dditemname, "select Item_id, Item_Name from Item_Master where Category_Id=" + ddCatagory.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Item_Name", true, "---Plz Select----");

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
            UtilityModule.ConditionalComboFill(ref ddshape, "SELECT ShapeId,Shapename from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by shapename", true, "--Plz Select--");
            if (ddshape.Items.Count > 0)
            {
                ddshape.SelectedIndex = 1;
                if (Convert.ToInt32(DDunit.SelectedValue) == 6)
                {
                    UtilityModule.ConditionalComboFill(ref ddsize, "SELECT SizeId,Sizeinch Size_Name from Size where shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Sizeinch", true, "--Plz Select--");
                }
                else if (Convert.ToInt32(DDunit.SelectedValue) == 2)
                {
                    //UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SizeId,SizeFt Size_Name from Size where shapeid=" + DDShape.SelectedValue + " order by sizeid", true, "--SELECT--");
                    UtilityModule.ConditionalComboFill(ref ddsize, "Select S.Sizeid,SizeFt from Size S Where shapeid=" + ddshape.SelectedValue + " And S.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by S.SizeFt", true, "--Plz Select--");
                }
                else
                {
                    // UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SizeId,SizeMtr Size_Name from Size where shapeid=" + DDShape.SelectedValue + " order by sizeid", true, "--SELECT--");
                    UtilityModule.ConditionalComboFill(ref ddsize, "select S.Sizeid,SizeMtr from Size S where shapeid=" + ddshape.SelectedValue + " And S.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by S.SizeMtr", true, "--Plz Select--");
                }

            }
        }
        if (TDShade.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddshape, "SELECT * from ShadeColor Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ShadeColorName", true, "--Plz Select--");
        }
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
        fillCombo();
    }
    //protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    UtilityModule.ConditionalComboFill(ref ddquality, "select QualityId,QualityName from Quality Where Item_Id=" + dditemname.SelectedValue + " Order by QualityName", true, "--Plz Select --");

    //}
    private void shapeselectedindexchange()
    {
        if (ddshape.SelectedIndex > 0)
        {
            if (Convert.ToInt32(DDunit.SelectedValue) == 6)
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "SELECT SizeId,Sizeinch Size_Name from Size where shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Sizeinch", true, "--Plz Select--");
            }
            else if (Convert.ToInt32(DDunit.SelectedValue) == 2)
            {
                //UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SizeId,SizeFt Size_Name from Size where shapeid=" + DDShape.SelectedValue + " order by sizeid", true, "--SELECT--");
                UtilityModule.ConditionalComboFill(ref ddsize, "Select S.Sizeid,SizeFt from Size S Where shapeid=" + ddshape.SelectedValue + " And S.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by S.SizeFt", true, "--Plz Select--");
            }
            else
            {
                // UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SizeId,SizeMtr Size_Name from Size where shapeid=" + DDShape.SelectedValue + " order by sizeid", true, "--SELECT--");
                UtilityModule.ConditionalComboFill(ref ddsize, "select S.Sizeid,SizeMtr from Size S where shapeid=" + ddshape.SelectedValue + " And S.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by S.SizeMtr", true, "--Plz Select--");
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
            DataSet Ds;
            // int ItemFinishedId = UtilityModule.getItemFinishedId(dditemname, ddquality, dddesign, ddcolor, ddshape, ddsize, TxtProductCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            // Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "select size_Id,SHAPE_ID from Item_Parameter_Master where Item_Finished_Id=" + ItemFinishedId + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");

            int SizeId = Convert.ToInt32((ddsize.SelectedValue == "" || ddsize.SelectedValue == null ? "0" : ddsize.SelectedValue));

            if (SizeId != 0 && hncomp.Value != "6")
            {

                //TdArea.Visible = true;
                SqlParameter[] _arrpara = new SqlParameter[6];
                _arrpara[0] = new SqlParameter("@size_Id", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@UnitTypeId", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@Length", SqlDbType.Float);
                _arrpara[3] = new SqlParameter("@width", SqlDbType.Float);
                _arrpara[4] = new SqlParameter("@Area", SqlDbType.Float);
                _arrpara[5] = new SqlParameter("@Shapeid", SqlDbType.Int);

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
                int shapeid =Convert.ToInt16(_arrpara[5].Value);

                //hdArea.Value = string.Format("{0:#0.0000}", _arrpara[4].Value);
                if (Convert.ToInt32(DDunit.SelectedValue) == 1)
                {
                    TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), shapeid));
                    //hdArea.Value = TxtArea.Text;
                }
                if (Convert.ToInt32(DDunit.SelectedValue) == 2)
                {
                    TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), shapeid));
                    // hdArea.Value = TxtArea.Text;
                }
            }
            else if (SizeId != 0 && hncomp.Value == "6")
            {
                //datatset dt1 = SqlHelper.ExecuteDataset(con, CommandType.Text, "");
                string str = "";
                str = "select WidthFt,LengthFt,HeightFt,WidthMtr,LengthMtr,HeightMtr,AreaFt,AreaMtr from size where sizeid=" + SizeId + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
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
        ItemFinishedId = UtilityModule.getItemFinishedId(dditemname, ddquality, dddesign, ddcolor, ddshape, ddsize, TxtProductCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        Area();
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
        {   // Check if Item already exist..
            if (IssueOrderId.ToString() == "" || IssueOrderId == null)
            {
                IssueOrderId = 0;
            }
            if (ViewState["IssueOrderId"] == null || ViewState["IssueOrderId"].ToString() == "0")
            {
                ViewState["IssueOrderId"] = 0;
            }
            ItemFinishedId = UtilityModule.getItemFinishedId(dditemname, ddquality, dddesign, ddcolor, ddshape, ddsize, TxtProductCode, Tran, ddlshade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));

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

            ViewState["IssueOrderId"] = Convert.ToInt32(_arrpara[0].Value);
            TxtChallanNo.Text = _arrpara[0].Value.ToString();
            OrderId = Convert.ToInt16(_arrpara[21].Value);
            Tran.Commit();
            TxtQtyRequired.Text = "";
            TxtRate.Text = "";
            //if Item already exists..

            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Save", "alert('" + _arrpara[36].Value.ToString() + "');", true);

            Fill_Grid();
            BtnPreview.Enabled = true;
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
                        Width + 'x' + Length Size,Area,Rate,Qty,Amount,OrderId,PD.Item_Finished_Id From PROCESS_ISSUE_MASTER_1 PM,PROCESS_ISSUE_DETAIL_1 PD,
                        V_FinishedItemDetail vf
                        Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=vf.Item_Finished_id  
                        And PM.IssueOrderid=" + ViewState["IssueOrderId"] + " And vf.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By Issue_Detail_Id Desc";

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
                        And PM.IssueOrderid=" + ViewState["IssueOrderId"] + " And vf.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By Issue_Detail_Id Desc";

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

    protected void DGOrderdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
        //    e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        //    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderdetail, "Select$" + e.Row.RowIndex);
        //}
    }
    protected void DDvendorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemFinishedId = 0;
        ViewState["IssueOrderId"] = 0;
        OrderId = 0;
    }
    private void ProcessReportPath()
    {
        #region Author: Rajeev, Date: 30-Nov-12...
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string str = "";
        str = "Delete TEMP_PROCESS_ISSUE_MASTER ";
        str = str + " Delete TEMP_PROCESS_ISSUE_DETAIL  ";
        str = str + " Insert into TEMP_PROCESS_ISSUE_MASTER Select IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight,1 from PROCESS_ISSUE_MASTER_1 Where IssueOrderId=" + ViewState["IssueOrderId"] + "";
        str = str + " Insert into TEMP_PROCESS_ISSUE_DETAIL Select Issue_Detail_Id,IssueOrderId,Item_Finished_Id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,ArticalNo,QualityCodeId,RejectQty,CancelQty,Approvalflag,estimatedweight from PROCESS_ISSUE_DETAIL_1 Where IssueOrderId=" + ViewState["IssueOrderId"] + "";

        SqlHelper.ExecuteNonQuery(con, CommandType.Text, str);


        con.Close();
        con.Dispose();
        #endregion
    }
    private void Report()
    {
        string qry = @"SELECT VPI.Item_Name,VPI.Description,VPI.AssignDate,VPI.Remarks,VPI.Instruction,VPI.IssueOrderid,Round(VPI.Area,4) Area,VPI.Qty,VPI.ReqByDate,CI.CompanyName, CI.CompAddr1,CI.CompAddr2,CI.CompAddr3,CI.CompTel,CI.TinNo,EI.EmpName,EI.Address,EI.PhoneNo,OM.CustomerOrderNo,OM.LocalOrder,Unit.UnitName,
                     PNM.ShortName,VPI.Rate,VPI.Amount,VPI.UnitId,OM.CUSTOMERORDERNO,CIC.CUSTOMERCODE ,u.unitname,VPI.Comm,CancelQty
                     FROM View_Production_Issue_Order VPI INNER JOIN CompanyInfo CI ON VPI.Companyid=CI.CompanyId INNER JOIN EmpInfo EI ON VPI.Empid=EI.EmpId INNER JOIN 
                     OrderMaster OM ON VPI.Orderid=OM.OrderId INNER JOIN Unit ON VPI.UnitId=Unit.UnitId INNER JOIN PROCESS_NAME_MASTER PNM ON VPI.PROCESSID=PNM.PROCESS_NAME_ID INNER JOIN
                     CUSTOMERINFO CIC ON CIC.CUSTOMERID=OM.CUSTOMERID inner join unit u on vpi.unitid=u.unitid Where VPI.IssueOrderid=" + ViewState["IssueOrderId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string str = @"SELECT VPIC.QTY,IM.ITEM_ID,IM.ITEM_NAME,VF.Finishedid,VF.Quality,vf.Design,vf.Color,vf.Shape,VF.ShadeColor,VPIC.Issueorderid,vpic.unitname FROM VIEW_PROCESS_ISSUE_CONSUMPTION VPIC INNER JOIN 
                       ViewFindFinishedId2 VF ON VPIC.FINISHEDID=VF.Finishedid INNER JOIN ITEM_MASTER IM ON VF.ITEM_ID=IM.ITEM_ID And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " ORDER BY IM.ITEM_ID,VF.Quality,VF.Finishedid";
        SqlDataAdapter sda = new SqlDataAdapter(str, con);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        ds.Tables.Add(dt);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToInt32(Session["VarcompanyNo"]) == 5)
            {
                Session["rptFileName"] = "~\\Reports\\ProductionOrderPoshNew.rpt";
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\ProductionOrderNew.rpt";
            }
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\ProductionOrderNew.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
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
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {

        }
    }
    protected void btnShowConsumption_Click(object sender, EventArgs e)
    {
        ViewState["Prmid"] = "0";
        TdDGConsumptionConeType.Style.Add("Display", "none");
        TdDgConsumption.Style.Add("Display", "none");
        if (ChkForCone.Checked == true)
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
    protected void txtWeaverIdNo_TextChanged(object sender, EventArgs e)
    {
        ViewState["IssueOrderId"] = 0;

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
                str = @"select EMp.Empid,Pm.IssueOrderId  from dbo.PROCESS_ISSUE_MASTER_1 PM inner join dbo.PROCESS_ISSUE_DETAIL_1 PD
                        on PM.IssueOrderId=Pd.IssueOrderId  and Pd.PQty>0
                        inner join  dbo.Employee_ProcessOrderNo EMP on EMP.IssueOrderId=PM.IssueOrderId and EMP.ProcessId=1
                        inner join EmpInfo EI on Ei.EmpId=EMP.Empid
                        And EI.EmpCode='" + txtWeaverIdNo.Text + @"'
                        select Empid,Empname from empinfo Where Empcode='" + txtWeaverIdNo.Text + "'";

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "AlertEmp", "alert('Folio -" + ds.Tables[0].Rows[0]["IssueOrderId"] + " Already pending at this ID No..');", true);
                    return;
                }

                //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Empid,Empname from empinfo Where Empcode='" + txtWeaverIdNo.Text + "'");
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (listWeaverName.Items.FindByValue(ds.Tables[1].Rows[0]["Empid"].ToString()) == null)
                    {

                        listWeaverName.Items.Add(new ListItem(ds.Tables[1].Rows[0]["Empname"].ToString(), ds.Tables[1].Rows[0]["Empid"].ToString()));
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
    private void fill_Grid_ShowConsmption()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@issueOrderid", ViewState["IssueOrderId"]);
            param[1] = new SqlParameter("Processid", 1);
            param[2] = new SqlParameter("@CompanyID", Session["CurrentWorkingCompanyID"]);

            ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Pro_RawMaterailIssueDataForANISA", param);

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
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;

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
    private void fill_Grid_ShowConsmptionConeType()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
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
                            VF1.ITEM_FINISHED_ID=OCD.IFINISHEDID And PM.Issueorderid=" + ViewState["IssueOrderId"] + " And VF1.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                            Group By VF1.Category_Name,VF1.Item_Name,VF1.QualityName,VF1.DesignName,VF1.ColorName,VF1.ShapeName,PM.UnitId,VF1.SizeMtr,VF1.SizeFt,
                            VF1.ShadeColorName,OCD.IFINISHEDID,PM.Issueorderid";

            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
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
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;

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

    protected void DGConsumption_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlgodown = (DropDownList)e.Row.FindControl("ddgodown");
            int itemFinishedid = Convert.ToInt32(DGConsumption.DataKeys[e.Row.RowIndex].Value);
            UtilityModule.ConditionalComboFill(ref ddlgodown, "Select Distinct GM.GodownID,GM.GodownName From GodownMaster GM,Stock S Where GM.GodownID=S.GodownID And QtyInHand>0 And CompanyId=" + Session["CurrentWorkingCompanyID"] + " And item_finished_id=" + itemFinishedid + " And GM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By GodownName", true, "--Plz Select godown--");
            DropDownList ddlotno = (DropDownList)e.Row.FindControl("ddlotno");
            
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
            switch (Convert.ToInt16(ddUnits.SelectedValue))
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
        switch (Convert.ToInt16(ddUnits.SelectedValue))
        {
            case 1:
                godownId = 1; //Kanpur
                break;
            case 2:
                godownId = 2; //Biswan

                break;
            case 3:
                godownId = 3; //Laharpur
                break;
            case 4://KHAIRABAD
                godownId = 4;
                break;
            case 5://ISMAILPUR
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
        switch (Convert.ToInt16(ddUnits.SelectedValue))
        {
            case 1:
                godownId = 1;   //Kanpur
                break;
            case 2:
                godownId = 2; //Biswan

                break;
            case 3:
                godownId = 3; //Laharpur
                break;
            case 4://KHAIRABAD
                godownId = 4;
                break;
            case 5://ISMAILPUR
                godownId = 5;
                break;
        }
        txtStockQty.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Round(isnull(Qtyinhand,0),3) As Stock from Stock Where godownId=" + godownId + " and CompanyId=" + Session["CurrentWorkingCompanyID"] + " And LotNo='" + ddllotno.SelectedItem.Text + "'  And Item_Finished_id=" + itemFinishedid + "").ToString();
        // UtilityModule.ConditionalComboFill(ref ddllotno, "select Lotno,LotNo From Stock Where CompanyId=1 and godownId=" + ddlgodown.SelectedValue + " And item_Finished_id=" + itemFinishedid + "", true, "--Plz Select Lot No.--");

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
                switch (Convert.ToInt16(ddUnits.SelectedValue))
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
                    case 4://KHAIRABAD
                        godownid = 4;
                        break;
                    case 5://ISMAILPUR
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
                arr[23] = new SqlParameter("@msg", SqlDbType.VarChar, 500);

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
                arr[5].Value = TxtAssignDate.Text;
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
                arr[11].Value = ddCatagory.SelectedValue;
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
                    UtilityModule.StockStockTranTableUpdate(Convert.ToInt16(arr[13].Value), godownid, 1, lotno, IssueQty, Convert.ToDateTime(TxtAssignDate.Text).ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), "ProcessRawTran", Convert.ToInt32(arr[19].Value), Tran, 0, false, 1, 0);
                    ViewState["Prmid"] = arr[18].Value;
                    lblConsumption.Text = "Raw Material issued Successfully....";
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
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }

    protected void DGOrderdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
            int index = e.RowIndex;
            int VarProcess_Issue_Detail_Id = Convert.ToInt32(DGOrderdetail.DataKeys[index].Value);
            int OrderId = Convert.ToInt16(((Label)DGOrderdetail.Rows[index].FindControl("lblOrderId")).Text);
            int ItemFinishedId = Convert.ToInt16(((Label)DGOrderdetail.Rows[index].FindControl("lblItemFinishedId")).Text);

            string str = @"Delete Process_Issue_Detail_1 Where Issue_Detail_Id=" + VarProcess_Issue_Detail_Id + @"
                           Delete PROCESS_CONSUMPTION_DETAIL Where IssueOrderID=" + ViewState["IssueOrderId"] + " And Issue_Detail_ID=" + VarProcess_Issue_Detail_Id + @" And ProcessId=1
                           Delete  from Employee_ProcessOrderNo Where IssueDetailId=" + VarProcess_Issue_Detail_Id + "  And IssueOrderId=" + ViewState["IssueOrderId"] + @" And Processid=1                           
	                       insert into UpdateStatus(Id,CompanyId,UserId,Tablename,TableId,Date,Status)
                           values((select max(id)+1 from UpdateStatus)," + Session["varMasterCompanyIDForERP"] + "," + Session["varuserid"] + ",'Process_Issue_Detail_1','" + ViewState["IssueOrderId"] + "',GETDATE(),'Delete')";

            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            Tran.Commit();
            LblErrorMessage.Text = "Data Deleted Successfully..............";
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
    protected void DGConsumptionConeType_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddconetype = (DropDownList)e.Row.FindControl("DDConeType");
            int Item_Finished_id = Convert.ToInt16(DGConsumptionConeType.DataKeys[e.Row.RowIndex].Value);
            UtilityModule.ConditionalComboFill(ref ddconetype, "select ID,ConeType+space(2)+'/'+cast(Qty as nvarchar)+'  '+'kg.' As ConeType From ConeType Where Item_FInished_id=" + Item_Finished_id + " order by ConeType", true, "--Plz Select Cone Type--");
            DropDownList ddlotno = (DropDownList)e.Row.FindControl("ddlotno");
            switch (Convert.ToInt16(ddUnits.SelectedValue))
            {
                case 1: //Kanpur
                    UtilityModule.ConditionalComboFill(ref ddlotno, "select Lotno,LotNo From Stock Where CompanyId=" + Session["CurrentWorkingCompanyID"] + " and godownId=1 And item_Finished_id=" + Item_Finished_id + "", true, "--Plz Select Lot No.--");
                    break;
                case 2:  //Biswan
                    UtilityModule.ConditionalComboFill(ref ddlotno, "select Lotno,LotNo From Stock Where CompanyId=" + Session["CurrentWorkingCompanyID"] + " and godownId=2 And item_Finished_id=" + Item_Finished_id + "", true, "--Plz Select Lot No.--");
                    break;
                case 3: //Laharpur
                    UtilityModule.ConditionalComboFill(ref ddlotno, "select Lotno,LotNo From Stock Where CompanyId=" + Session["CurrentWorkingCompanyID"] + " and godownId=3 And item_Finished_id=" + Item_Finished_id + "", true, "--Plz Select Lot No.--");
                    break;
                case 4: //KHAIRABAD
                    UtilityModule.ConditionalComboFill(ref ddlotno, "select Lotno,LotNo From Stock Where CompanyId=" + Session["CurrentWorkingCompanyID"] + " and godownId=4 And item_Finished_id=" + Item_Finished_id + "", true, "--Plz Select Lot No.--");
                    break;
                case 5: //ISMAILPUR
                    UtilityModule.ConditionalComboFill(ref ddlotno, "select Lotno,LotNo From Stock Where CompanyId=" + Session["CurrentWorkingCompanyID"] + " and godownId=5 And item_Finished_id=" + Item_Finished_id + "", true, "--Plz Select Lot No.--");
                    break;
            }

        }
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
                switch (Convert.ToInt16(ddUnits.SelectedValue))
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
                    case 4://KHAIRABAD
                        godownid = 4;
                        break;
                    case 5://ISMAILPUR
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
                arr[5].Value = TxtAssignDate.Text;
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
                arr[11].Value = ddCatagory.SelectedValue;
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
                UtilityModule.StockStockTranTableUpdate(Convert.ToInt16(arr[13].Value), godownid, 1, lotno, IssueQty, Convert.ToDateTime(TxtAssignDate.Text).ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), "ProcessRawTran", Convert.ToInt32(arr[19].Value), Tran, 0, false, 1, 0);
                Tran.Commit();
                ViewState["Prmid"] = arr[18].Value;
                lblConsumption.Text = "Consumption Saved Siccessfully....";
                fill_Grid_ShowConsmptionConeType();

            }
            catch (Exception ex)
            {
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }


}