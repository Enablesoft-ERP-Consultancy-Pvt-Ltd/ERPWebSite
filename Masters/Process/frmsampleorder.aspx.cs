using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Process_frmsampleorder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName
                Select Distinct CI.Customerid,CI.Customercode  from CustomerInfo CI where CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by customercode
                select PROCESS_NAME_ID,PROCESS_NAME From Process_Name_Master Where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" and PROCESS_NAME='WEAVING'
                select unitid,unitname from unit where unitid in (1,2) order by unitid desc
                select Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner Join CategorySeparate CS on ICM.CATEGORY_ID=CS.Categoryid and cs.id=0 order by ICM.CATEGORY_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDprocess, ds, 2, true, "--Plz Select--");
            if (DDprocess.Items.Count > 0)
            {
                DDprocess.SelectedIndex = 1;
                DDprocess_SelectedIndexChanged(sender, new EventArgs());
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDunit, ds, 3, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, ds, 4, true, "--Plz Select--");

            txtassigndate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtreqdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            lablechange();
            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
            }
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select Item_Id,ITEM_NAME from Item_master Where CATEGORY_ID=" + ddCatagory.SelectedValue + " order by ITEM_NAME";
        UtilityModule.ConditionalComboFill(ref dditemname, str, true, "--Plz Select--");
        FillCombo();
    }
    protected void FillCombo()
    {
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
        TDShape.Visible = false;
        TDSize.Visible = false;
        TDShade.Visible = false;


        string str = @"select PARAMETER_ID from ITEM_CATEGORY_PARAMETERS where category_id=" + ddCatagory.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["Parameter_Id"].ToString())
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
                        TDSize.Visible = true;
                        UtilityModule.ConditionalComboFill(ref DDsizetype, "select val,Type from SizeType Order by val", false, "");
                        break;
                    case "6":
                        TDShade.Visible = true;
                        break;
                }
            }
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQDCS();
    }
    protected void FillQDCS()
    {
        string str = null;
        //Quality
        if (TDQuality.Visible == true)
        {

            str = "select Distinct QualityId,QualityName from Quality Where Item_Id=" + dditemname.SelectedValue + " order by QualityName";
            UtilityModule.ConditionalComboFill(ref dquality, str, true, "--Select--");

        }
        //Design
        if (TDDesign.Visible == true)
        {
            //str = "select Distinct D.designId,D.designName from V_FinishedItemDetail vf inner Join Design D on vf.DesignId=D.designid  Where Item_Id=" + dditemname.SelectedValue + " order by D.designname";
            str = "select Designid,Designname From Design Where mastercompanyid=" + Session["varMasterCompanyIDForERP"] + " order by designname";
            UtilityModule.ConditionalComboFill(ref dddesign, str, true, "--Select--");
        }
        //Color
        if (TDColor.Visible == true)
        {
            // str = "select Distinct C.colorid,C.colorname from V_FinishedItemDetail vf inner Join Color C on Vf.colorid=C.colorid  Where Item_Id=" + dditemname.SelectedValue + " order by C.Colorname";
            str = "select Colorid,colorname From color Where mastercompanyid=" + Session["varMasterCompanyIDForERP"] + " order by colorname";
            UtilityModule.ConditionalComboFill(ref ddcolor, str, true, "--Select--");

        }
        //Shape
        if (TDShape.Visible == true)
        {

            str = "select Distinct Sh.shapeid,Sh.shapename from Shape Sh  order by shapeid";
            UtilityModule.ConditionalComboFill(ref ddshape, str, true, "--Select--");
            if (ddshape.Items.Count > 0)
            {
                ddshape.SelectedIndex = 1;
                ddshape_SelectedIndexChanged(ddshape, new EventArgs());
            }

        }
        //Shade
        if (TDShade.Visible == true)
        {
            str = "select shadecolorid,shadecolorname from shadecolor   order by shadecolorname";
            UtilityModule.ConditionalComboFill(ref ddlshade, str, true, "--Select--");
        }
        //Unit
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
    //protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //UtilityModule.ConditionalComboFill(ref dddesign, "select Distinct Vf.designid,vf.designname From V_finisheditemdetail vf Where ITEM_ID=" + dditemname.SelectedValue + " and QualityId=" + dquality.SelectedValue + " and vf.designId>0 order by vf.designname", true, "--Plz Select");
    //}
    //protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    UtilityModule.ConditionalComboFill(ref ddcolor, "select Distinct Vf.colorid,vf.ColorName From V_finisheditemdetail vf Where ITEM_ID=" + dditemname.SelectedValue + " and QualityId=" + dquality.SelectedValue + " and designId=" + dddesign.SelectedValue + " and vf.ColorId>0 order by colorname", true, "--Plz Select--");
    //}
    protected void Fillsize()
    {

        string str = null, size = null;
        if (variable.VarNewQualitySize == "1")
        {
            switch (DDunit.SelectedValue.ToString())
            {
                case "2":
                    if (chkexportsize.Checked == true)
                    {
                        size = "Export_Format";
                    }
                    else { size = "Production_Ft_Format"; }
                    break;
                case "1":
                    if (chkexportsize.Checked == true)
                    {
                        size = "MtrSize";
                    }
                    else { size = "Production_Mt_Format"; }
                    break;
                default:
                    if (chkexportsize.Checked == true)
                    {
                        size = "Export_Format";
                    }
                    else { size = "Production_Ft_Format"; }
                    break;
            }
            #region
            //// str = "select Distinct Vf.sizeid,Vf." + size + " from V_FinishedItemDetail vf inner join  Size S on vf.Sizeid=S.sizeid Where ITEM_ID=" + dditemname.SelectedValue + " and vf.shapeid=" + ddshape.SelectedValue + "";
            // if (TDQuality.Visible == true && dquality.SelectedIndex > 0)
            // {
            //     str = str + " and vf.QualityId=" + dquality.SelectedValue;
            // }
            // if (TDDesign.Visible == true && dddesign.SelectedIndex > 0)
            // {
            //     str = str + " and vf.designId=" + dddesign.SelectedValue;
            // }
            // if (TDColor.Visible == true && ddcolor.SelectedIndex > 0)
            // {
            //     str = str + " and vf.colorid=" + ddcolor.SelectedValue;
            // }
            #endregion
            str = "select Distinct S.sizeid,S." + size + " From QualitySizeNew S Where S.shapeid=" + ddshape.SelectedValue;
            str = str + " order by S." + size;
        }
        else
        {
            switch (DDunit.SelectedValue.ToString())
            {
                case "2":
                    if (chkexportsize.Checked == true)
                    {
                        size = "Sizeft";
                    }
                    else { size = "Prodsizeft"; }
                    break;
                case "1":
                    if (chkexportsize.Checked == true)
                    {
                        size = "Sizemtr";
                    }
                    else { size = "Prodsizemtr"; }
                    break;
                default:
                    if (chkexportsize.Checked == true)
                    {
                        size = "Sizeft";
                    }
                    else { size = "Prodsizeft"; }
                    break;
            }
            #region
            //// str = "select Distinct Vf.sizeid,Vf." + size + " from V_FinishedItemDetail vf inner join  Size S on vf.Sizeid=S.sizeid Where ITEM_ID=" + dditemname.SelectedValue + " and vf.shapeid=" + ddshape.SelectedValue + "";
            // if (TDQuality.Visible == true && dquality.SelectedIndex > 0)
            // {
            //     str = str + " and vf.QualityId=" + dquality.SelectedValue;
            // }
            // if (TDDesign.Visible == true && dddesign.SelectedIndex > 0)
            // {
            //     str = str + " and vf.designId=" + dddesign.SelectedValue;
            // }
            // if (TDColor.Visible == true && ddcolor.SelectedIndex > 0)
            // {
            //     str = str + " and vf.colorid=" + ddcolor.SelectedValue;
            // }
            #endregion
            str = "select Distinct S.sizeid,S." + size + " From size S Where S.shapeid=" + ddshape.SelectedValue + "  and S.mastercompanyid=" + Session["varMasterCompanyIDForERP"];
            str = str + " order by S." + size;
        }
        UtilityModule.ConditionalComboFill(ref ddsize, str, true, "--Select--");

    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
    protected void Area()
    {
        txtarea.Text = "";
        SqlParameter[] _arrpara = new SqlParameter[7];
        _arrpara[0] = new SqlParameter("@size_Id", SqlDbType.Int);
        _arrpara[1] = new SqlParameter("@UnitTypeId", SqlDbType.Int);
        _arrpara[2] = new SqlParameter("@Length", SqlDbType.Float);
        _arrpara[3] = new SqlParameter("@width", SqlDbType.Float);
        _arrpara[4] = new SqlParameter("@Area", SqlDbType.Float);
        _arrpara[5] = new SqlParameter("@Shapeid", SqlDbType.Int);
        _arrpara[6] = new SqlParameter("@ExportSize", SqlDbType.Int);

        _arrpara[0].Value = ddsize.SelectedValue;
        _arrpara[1].Value = DDunit.SelectedValue;
        _arrpara[2].Direction = ParameterDirection.Output;
        _arrpara[3].Direction = ParameterDirection.Output;
        _arrpara[4].Direction = ParameterDirection.Output;
        _arrpara[5].Direction = ParameterDirection.Output;
        _arrpara[6].Value = chkexportsize.Checked == true ? 1 : 0;

        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Areasample", _arrpara);
        switch (DDunit.SelectedValue)
        {
            case "2": //ft
                txtlength.Text = string.Format("{0:#0.00}", _arrpara[2].Value);
                txtwidth.Text = string.Format("{0:#0.00}", _arrpara[3].Value);
                break;
            default:
                txtlength.Text = _arrpara[2].Value.ToString();
                txtwidth.Text = _arrpara[3].Value.ToString();
                break;
        }
        txtarea.Text = string.Format("{0:#0.0000}", _arrpara[4].Value);
        if (Convert.ToInt32(DDunit.SelectedValue) == 1)
        {
            txtarea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(txtlength.Text), Convert.ToDouble(txtwidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(ddshape.SelectedValue)));
        }
        if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
        {
            txtarea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(txtlength.Text), Convert.ToDouble(txtwidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(ddshape.SelectedValue), UnitId: Convert.ToInt16(DDunit.SelectedValue)));
        }
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        Area();
    }
    private void Check_Length_Width_Format()
    {
        int FootLength = 0;
        int FootWidth = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        string Str = "";
        if (txtlength.Text != "")
        {
            if (Convert.ToInt32(DDunit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(txtlength.Text));
                txtlength.Text = Str;
                FootLength = Convert.ToInt32(Str.Split('.')[0]);
                FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootLengthInch > 11)
                {
                    lblmsg.Text = "Inch value must be less than 12";
                    txtlength.Text = "";
                    txtlength.Focus();
                }
            }
        }
        if (txtwidth.Text != "")
        {
            if (Convert.ToInt32(DDunit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(txtwidth.Text));
                txtwidth.Text = Str;
                FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootWidthInch > 11)
                {
                    lblmsg.Text = "Inch value must be less than 12";
                    txtwidth.Text = "";
                    txtwidth.Focus();
                }
            }
        }
        if (txtlength.Text != "" && txtwidth.Text != "")
        {
            int Shape = 0;

            Shape = Convert.ToInt32(ddshape.SelectedValue);

            if (Convert.ToInt32(DDunit.SelectedValue) == 1)
            {
                txtarea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(txtlength.Text), Convert.ToDouble(txtwidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Shape));
            }
            if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
            {
                txtarea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(txtlength.Text), Convert.ToDouble(txtwidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Shape, UnitId: Convert.ToInt16(DDunit.SelectedValue)));
            }
        }
    }
    protected void txtwidth_TextChanged(object sender, EventArgs e)
    {
        Check_Length_Width_Format();
    }
    protected void txtlength_TextChanged(object sender, EventArgs e)
    {
        Check_Length_Width_Format();
    }
    protected void DDprocess_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDvendor, "select EI.EmpId,EI.EmpName  From Empinfo EI inner join EmpProcess EP on EI.empid=EP.EmpId  and EP.ProcessId=" + DDprocess.SelectedValue + " and Ei.mastercompanyid=" + Session["varMasterCompanyIDForERP"] + " order by Ei.EmpName", true, "--Plz Select--");
    }
    protected void btnsave_Click(object sender, EventArgs e)
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
            SqlParameter[] param = new SqlParameter[30];
            param[0] = new SqlParameter("@issueorderid", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hnissueorderid.Value;
            param[1] = new SqlParameter("@orderid", SqlDbType.Int);
            param[1].Direction = ParameterDirection.InputOutput;
            param[1].Value = hnorderid.Value;
            param[2] = new SqlParameter("@companyId", DDcompany.SelectedValue);
            param[3] = new SqlParameter("@customerid", DDcustcode.SelectedValue);
            param[4] = new SqlParameter("@Processid", DDprocess.SelectedValue);
            param[5] = new SqlParameter("@empid", DDvendor.SelectedValue);
            param[6] = new SqlParameter("@assigndate", txtassigndate.Text);
            param[7] = new SqlParameter("@Reqdate", txtreqdate.Text);
            param[8] = new SqlParameter("@Unitid", DDunit.SelectedValue);
            param[9] = new SqlParameter("@caltype", DDcaltype.SelectedValue);
            param[10] = new SqlParameter("@Remarks", txtremarks.Text);
            int Item_finished_id = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, Tran, ddlshade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            param[11] = new SqlParameter("@Item_finished_id", Item_finished_id);
            param[12] = new SqlParameter("@Width", txtwidth.Text);
            param[13] = new SqlParameter("@Length", txtlength.Text);
            param[14] = new SqlParameter("@Area", txtarea.Text);
            param[15] = new SqlParameter("@Rate", txtrate.Text == "" ? "0" : txtrate.Text);
            param[16] = new SqlParameter("@Commrate", txtcomm.Text == "" ? "0" : txtcomm.Text);
            param[17] = new SqlParameter("@Qty", txtpcs.Text);
            param[18] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[18].Direction = ParameterDirection.Output;
            param[19] = new SqlParameter("@userid", Session["varuserid"]);
            param[20] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);

            param[21] = new SqlParameter("@Quality", dquality.SelectedItem.Text);
            param[22] = new SqlParameter("@Design", dddesign.SelectedItem.Text);
            param[23] = new SqlParameter("@color", ddcolor.SelectedItem.Text);

            param[24] = new SqlParameter("@QualityId", dquality.SelectedValue);
            param[25] = new SqlParameter("@Designid", dddesign.SelectedValue);
            param[26] = new SqlParameter("@Colorid", ddcolor.SelectedValue);
            param[27] = new SqlParameter("@varcarpetcompany", variable.Carpetcompany);
            param[28] = new SqlParameter("@Itemid", dditemname.SelectedValue);
            param[29] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 100);
            param[29].Direction = ParameterDirection.Output;

            //*********************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_Savesampleorder", param);
            //**************
            lblmsg.Text = param[18].Value.ToString();
            hnissueorderid.Value = param[0].Value.ToString();
            hnorderid.Value = param[1].Value.ToString();
            txtchallanNo.Text = param[29].Value.ToString();
            ddsize.Focus();
            //**************
            Tran.Commit();
            refreshcontrol();
            Fill_Grid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void refreshcontrol()
    {
        ddsize.SelectedIndex = -1;
        txtwidth.Text = "";
        txtlength.Text = "";
        txtarea.Text = "";
        txtrate.Text = "";
        txtcomm.Text = "";
        txtpcs.Text = "";
    }
    //protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  //  Fillsize();
    //}
    private void Fill_Grid()
    {
        DGOrderdetail.DataSource = GetDetail();
        DGOrderdetail.DataBind();
    }
    //*********************************************************FIll Gride*********************************************************
    private DataSet GetDetail()
    {
        DataSet DS = null;
        string sqlstr = "";

        //        sqlstr = @"Select Issue_Detail_Id as Sr_No,ICM.Category_Name as Category,IM.Item_Name as Item,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else case When PM.unitid=6 Then Sizeinch  Else  SizeFt End End Description,Length,Width,
        //                        Length + 'x' + Width Size,Qty*Area as Area,Rate,Qty,Amount From PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDprocess.SelectedValue + @" PD,
        //                        ViewFindFinishedidItemidQDCSS IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM 
        //                        Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
        //                        PM.IssueOrderid=" + hnissueorderid.Value + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By Issue_Detail_Id Desc";

        if (variable.VarNewQualitySize == "1")
        {
            sqlstr = @"Select Issue_Detail_Id as Sr_No,ICM.Category_Name as Category,IM.Item_Name as Item,IPM.QualityName+' '+IPM.designName+' '+IPM.ColorName+' ' +IPM.ShapeName+ Space(5) +
                    Case When PM.Unitid=1 Then IPM.SizeMtr Else case When PM.unitid=6 Then IPM.Sizeinch  Else  SizeFt End End Description,Length,Width,
                    Length + 'x' + Width Size,Qty*Area as Area,Rate,Qty,Amount,PD.orderid,PD.Item_Finished_Id,Replace(CONVERT(nvarchar(11),PM.assigndate,106),' ','-') as assigndate,Replace(CONVERT(nvarchar(11),PD.ReqByDate,106),' ','-') as Reqbydate,
                    PM.UnitId,PM.CalType,PD.comm,PM.issueorderid,PM.remarks,PM.Unitid,IPM.ShapeId From PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + DDprocess.SelectedValue + @" PD,
                    V_FinishedItemDetailNew IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM 
                    Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.ITEM_FINISHED_ID 
                    And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
                    PM.IssueOrderid=" + hnissueorderid.Value + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By Issue_Detail_Id Desc";
        }
        else
        {
            sqlstr = @"Select Issue_Detail_Id as Sr_No,ICM.Category_Name as Category,IM.Item_Name as Item,IPM.QualityName+' '+IPM.designName+' '+IPM.ColorName+' ' +IPM.ShapeName+ Space(5) +
                    Case When PM.Unitid=1 Then IPM.SizeMtr Else case When PM.unitid=6 Then IPM.Sizeinch  Else  SizeFt End End Description,Length,Width,
                    Length + 'x' + Width Size,Qty*Area as Area,Rate,Qty,Amount,PD.orderid,PD.Item_Finished_Id,Replace(CONVERT(nvarchar(11),PM.assigndate,106),' ','-') as assigndate,Replace(CONVERT(nvarchar(11),PD.ReqByDate,106),' ','-') as Reqbydate,
                    PM.UnitId,PM.CalType,PD.comm,PM.issueorderid,PM.remarks,PM.Unitid,IPM.ShapeId From PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + DDprocess.SelectedValue + @" PD,
                    V_FinishedItemDetail IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM 
                    Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.ITEM_FINISHED_ID 
                    And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
                    PM.IssueOrderid=" + hnissueorderid.Value + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By Issue_Detail_Id Desc";
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
            if (chkedit.Checked == true)
            {
                if (DS.Tables[0].Rows.Count > 0)
                {
                    txtchallanNo.Text = DDchallanNo.SelectedValue;
                    txtassigndate.Text = DS.Tables[0].Rows[0]["assigndate"].ToString();
                    txtreqdate.Text = DS.Tables[0].Rows[0]["Reqbydate"].ToString();
                    DDunit.SelectedValue = DS.Tables[0].Rows[0]["unitid"].ToString();
                    DDcaltype.SelectedValue = DS.Tables[0].Rows[0]["caltype"].ToString();
                    hnorderid.Value = DS.Tables[0].Rows[0]["orderid"].ToString();
                    txtremarks.Text = DS.Tables[0].Rows[0]["Remarks"].ToString();
                }
                else
                {
                    txtchallanNo.Text = "";
                    txtassigndate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
                    txtreqdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
                }
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return DS;
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        TDChallanNO.Visible = false;
        DDvendor.SelectedIndex = -1;
        DDchallanNo.SelectedIndex = -1;
        DGOrderdetail.DataSource = null;
        DGOrderdetail.DataBind();
        hnissueorderid.Value = "0";
        hnorderid.Value = "0";
        TDeditchallanNo.Visible = false;

        if (chkedit.Checked == true)
        {
            TDChallanNO.Visible = true;
            TDeditchallanNo.Visible = true;
        }
    }
    protected void DDvendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select Distinct PIM.IssueOrderId,PIM.IssueOrderId as Issueorderid1 From process_issue_master_" + DDprocess.SelectedValue + " PIM inner join Process_issue_Detail_" + DDprocess.SelectedValue + @" PID on PIM.Issueorderid=PID.issueorderid
                    inner join ordermaster OM on PID.orderid=OM.Orderid  and Om.ORDERFROMSAMPLE=1
                    Where SampleNumber<>''  and PIM.Companyid=" + DDcompany.SelectedValue + " And PIM.Empid=" + DDvendor.SelectedValue;
        if (Chkcomplete.Checked == true)
        {
            str = str + " and PIM.Status='Complete'";
        }
        else
        {
            str = str + " and PIM.Status='Pending'";
        }
        if (DDcustcode.SelectedIndex > 0)
        {
            str = str + " and OM.customerid=" + DDcustcode.SelectedValue;
        }
        str = str + " order by Issueorderid1";

        UtilityModule.ConditionalComboFill(ref DDchallanNo, str, true, "--Plz Select--");

    }
    protected void DDchallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnissueorderid.Value = DDchallanNo.SelectedValue;
        Fill_Grid();
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
    protected void txtchallanNoedit_TextChanged(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        if (DDprocess.SelectedIndex <= 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt1", "alert('Please Select Process.')", true);
            DDprocess.Focus();
            return;
        }
        string str = @"select  Distinct PIM.Companyid,OM.CustomerId," + DDprocess.SelectedValue + @" as Processid,PIM.empid,PIM.IssueOrderId
                        From Process_Issue_Master_" + DDprocess.SelectedValue + " PIM inner join PROCESS_ISSUE_DETAIL_" + DDprocess.SelectedValue + @" PID on PIM.IssueOrderId=PID.IssueOrderId 
                        inner join OrderMaster OM on PID.orderid=OM.OrderId
                        Where PIM.companyid = " + DDcompany.SelectedValue + " And PIM.IssueOrderId=" + txtchallanNoedit.Text;
        if (Chkcomplete.Checked == true)
        {
            str = str + " and PIM.Status='Complete'";
        }
        else
        {
            str = str + " and PIM.Status='Pending'";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DDprocess.SelectedValue = ds.Tables[0].Rows[0]["Processid"].ToString();
            DDprocess_SelectedIndexChanged(sender, new EventArgs());
            DDcustcode.SelectedValue = ds.Tables[0].Rows[0]["customerid"].ToString();
            DDvendor.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
            DDvendor_SelectedIndexChanged(sender, new EventArgs());
            DDchallanNo.SelectedValue = ds.Tables[0].Rows[0]["issueorderid"].ToString();
            DDchallanNo_SelectedIndexChanged(sender, new EventArgs());
        }
        else
        {
            lblmsg.Text = "Please enter proper Challan No. ";
            txtchallanNoedit.Focus();
            DDcustcode.SelectedIndex = -1;
            DDvendor.SelectedIndex = -1;
            DDchallanNo.SelectedIndex = -1;
            DGOrderdetail.DataSource = null;
            DGOrderdetail.DataBind();
            return;
        }
    }
    protected void DGOrderdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
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
            int issue_detail_id = Convert.ToInt32(DGOrderdetail.DataKeys[e.RowIndex].Value);
            TextBox txtqtyedit = (TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtqtyedit");
            TextBox txtrateedit = (TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtrateedit");
            TextBox txtcommrateedit = (TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtcommrateedit");
            Label lblissueorderid = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblissueorderid");
            Label lblorderid = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblorderid");
            Label lblitemfinishedid = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblitemfinishedid");
            Label lblcaltype = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblcaltype");
            Label lblWidth = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblWidth");
            Label lblLength = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblLength");
            Label lblunitid = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblunitid");
            Label lblshapeid = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblshapeid");
            string AreaOnePcs = "0";
            //***************CalCulate Area
            if (lblLength.Text != "" && lblWidth.Text != "")
            {
                int Shape = 0;

                Shape = Convert.ToInt32(lblshapeid.Text);

                if (Convert.ToInt32(lblunitid.Text) == 1)
                {
                    AreaOnePcs = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(lblLength.Text), Convert.ToDouble(lblWidth.Text), Convert.ToInt32(lblcaltype.Text), Shape));
                }
                if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
                {
                    AreaOnePcs = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(lblLength.Text), Convert.ToDouble(lblWidth.Text), Convert.ToInt32(lblcaltype.Text), Shape, UnitId: Convert.ToInt16(lblunitid.Text)));
                }
            }
            //**********
            if (AreaOnePcs == "" || Convert.ToDecimal(AreaOnePcs) == 0)
            {
                lblmsg.Text = "Area can not be zero or Blank.";
                Tran.Commit();
                return;
            }
            SqlParameter[] param = new SqlParameter[13];
            param[0] = new SqlParameter("@issueorderid", lblissueorderid.Text);
            param[1] = new SqlParameter("@issue_Detail_id", issue_detail_id);
            param[2] = new SqlParameter("@Processid", DDprocess.SelectedValue);
            param[3] = new SqlParameter("@orderid", lblorderid.Text);
            param[4] = new SqlParameter("@Item_finished_id", lblitemfinishedid.Text);
            param[5] = new SqlParameter("@caltype", lblcaltype.Text);
            param[6] = new SqlParameter("@Qty", txtqtyedit.Text == "" ? "0" : txtqtyedit.Text);
            param[7] = new SqlParameter("@rate", txtrateedit.Text == "" ? "0" : txtrateedit.Text);
            param[8] = new SqlParameter("@commrate", txtcommrateedit.Text == "" ? "0" : txtcommrateedit.Text);
            param[9] = new SqlParameter("@userid", Session["varuserid"]);
            param[10] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[10].Direction = ParameterDirection.Output;
            param[11] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[12] = new SqlParameter("@AreaOnepcs", AreaOnePcs);
            //**************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateSampleorder", param);
            lblmsg.Text = param[10].Value.ToString();
            Tran.Commit();
            DGOrderdetail.EditIndex = -1;
            Fill_Grid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void DGOrderdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
            int issuedetailid = Convert.ToInt32(DGOrderdetail.DataKeys[e.RowIndex].Value);

            Label lblissueorderid = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblissueorderid");
            Label lblorderid = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblorderid");
            Label lblitemfinishedid = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblitemfinishedid");

            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@Issue_detail_id", issuedetailid);
            param[1] = new SqlParameter("@issueorderid", lblissueorderid.Text);
            param[2] = new SqlParameter("@orderid", lblorderid.Text);
            param[3] = new SqlParameter("@Item_finished_id", lblitemfinishedid.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@userid", Session["varuserid"]);
            param[6] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            //*****
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteSampleOrder", param);
            lblmsg.Text = param[4].Value.ToString();
            Tran.Commit();
            Fill_Grid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = "select * From V_Sampleorder Where issueorderid=" + hnissueorderid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptsampleorder.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptsampleorder.xsd";
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
    protected void chkexportsize_CheckedChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
}