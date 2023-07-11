using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ClosedXML.Excel;

public partial class Masters_Carpet_BuyerMasterCode : CustomPage
{
    string strC, str, strD;
    static string WithBuyerCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        //DataRow dr = AllEnums.MasterTables.Mastersetting.ToTable().Select()[0];
        //WithBuyerCode = dr["WithBuyerCode"].ToString();
        if (!IsPostBack)
        {
            logo();
            ParameteLabel();
            string str = @"select Customerid,Customercode From Customerinfo Where MasterCompanyId=" + Session["varCompanyId"] + @" order by Customercode
                         SELECT CATEGORY_ID,CATEGORY_NAME FROM ITEM_CATEGORY_MASTER ICM,CategorySeparate CS Where ICM.CATEGORY_ID=CS.CATEGORYID And CS.ID=0 And ICM.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CATEGORY_NAME
                         select val,Type from SizeType Order by val
                         select WithBuyerCode from Mastersetting";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref ddBuyerCode, ds, 0, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddCategory, ds, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 2, false, "");
            if (DDsizetype.Items.FindByValue(variable.VarDefaultSizeId) != null)
            {
                DDsizetype.SelectedValue = variable.VarDefaultSizeId;
            }

            if (ddCategory.Items.Count > 0)
            {
                ddCategory.SelectedValue = "1";
            }


            UtilityModule.ConditionalComboFill(ref ddItemname, "select item_id,item_name from item_master where category_id= " + ddCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By item_name", true, "--Select--");
            UtilityModule.ConditionalComboFill(ref ddShape, @"SELECT Distinct ShapeId,ShapeName FROM Shape Where MasterCompanyId=" + Session["varCompanyId"] + "  Order By ShapeId asc", true, "--Select--");

            if (ddShape.Items.Count > 0)
            {
                ddShape.SelectedIndex = 1;
            }
            ViewState["WithBuyerCode"] = ds.Tables[3].Rows[0]["WithBuyerCode"].ToString();

            ds.Dispose();

            if (Request.QueryString["ABC"] == "1")
            {
                zzz.Style.Add("display", "none");
            }

            if (variable.VarNewQualitySize == "1")
            {
                btnaddsize.Visible = false;
                btnAddQualitySize.Visible = true;
            }
            else
            {
                btnaddsize.Visible = true;
                btnAddQualitySize.Visible = false;
            }

            if (Request.QueryString["CustomerID"] != null)
            {
                ddBuyerCode.SelectedValue = Request.QueryString["CustomerID"];
                ddCategory.SelectedValue = Request.QueryString["VarCategory"];

                UtilityModule.ConditionalComboFill(ref ddItemname, "select item_id,item_name from item_master where category_id= " + ddCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By item_name", true, "--Select--");
                ddItemname.SelectedValue = Request.QueryString["VarItem"];
                ChkForMtrSize.Checked = true;
                fill_combo();
                fill_size();
                Fill_GridSize();
            }
            if (Session["varcompanyId"].ToString() == "9")
            {
                lblbuyermtrSize.Text = "CM Size";
            }

            if (Session["varcompanyId"].ToString() == "20")
            {
                BuyerInch.Visible = false;
                BuyerInchtxtb.Visible = false;
                trheader.Visible = false;
            }
            else
            {
                BuyerInch.Visible = true;
                BuyerInchtxtb.Visible = true;
                trheader.Visible = true;
            }

            if (Session["varcompanyId"].ToString() == "30")
            {
                lblSkuNo.Visible = true;
                txtSKUNo.Visible = true;
            }

        }
    }
    private void logo()
    {
        if (File.Exists(Server.MapPath("~/Images/Logo/" + Session["varCompanyId"] + "_company.gif")))
        {
            imgLogo.ImageUrl.DefaultIfEmpty();
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varCompanyId"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
        }
        LblCompanyName.Text = Session["varCompanyName"].ToString();
        LblUserName.Text = Session["varusername"].ToString();
    }
    private void ParameteLabel()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblQualityName.Text = ParameterList[0];
        lblDesignName.Text = ParameterList[1];
        lblColorName.Text = ParameterList[2];
        lblShapeName.Text = ParameterList[3];
        lblSizeName.Text = ParameterList[4];
        lblCategoryName.Text = ParameterList[5];
        lblItemName.Text = ParameterList[6];
    }
    protected void ddBuyerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        //fill_grid();
        Fill_GridSize();
        //btnrpt.Enabled = true;
        //Session["ReportPath"] = "RptBuyerMasterCode.rpt";
        //Session["CommanFormula"] = "{Customerinfo.CompanyName}='" + ddBuyerCode.SelectedItem.Text + "'";
    }
    private void fill_Quality_Grid()
    {
        DataSet ds = null;
        if (ddBuyerCode.SelectedIndex > 0 && ddItemname.SelectedIndex > 0)
        {
            str = "select SrNo Sr_No,QualityNameAToC + '  [' + QualityName + ']' Quality,case when CQ.Enable_Disable=1 Then 'Disable' Else 'Enable' ENd as Status,CQ.Enable_Disable from CustomerQuality CQ,Quality C where CQ.QualityId=C.QualityId and CQ.customerId=" + ddBuyerCode.SelectedValue + " And C.Item_Id=" + ddItemname.SelectedValue;
            if (ddLocalQuality.SelectedIndex > 0)
            {
                str = str + " And C.QualityID=" + ddLocalQuality.SelectedValue;
            }
            str = str + " Order By QualityNameAToC";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        }
        gdBuyerQuality.DataSource = ds;
        gdBuyerQuality.DataBind();
    }
    private void fill_Design_Grid()
    {
        DataSet ds = null;
        if (ddBuyerCode.SelectedIndex > 0 && ddItemname.SelectedIndex > 0)
        {
            //strD = "select SrNo Sr_No,DesignNameAToC + '  [' + DesignName + ']' Design,case when CD.Enable_Disable=1 Then 'Disable' Else 'Enable' ENd as Status,CD.Enable_Disable from CustomerDesign CD, Design D Where CD.DesignId=D.DesignId  And CD.CustomerId=" + ddBuyerCode.SelectedValue;
            //if (ddLocalDesign.SelectedIndex > 0)
            //{
            //    strD = strD + " And D.DesignId=" + ddLocalDesign.SelectedValue;
            //}
            //strD = strD + " Order By DesignNameAToC";
            strD = @"select CD.SrNo Sr_No,DesignNameAToC + '  [' + DesignName + ']' Design, case when CD.Enable_Disable=1 Then 'Disable' Else 'Enable' ENd as Status,CD.Enable_Disable 
            From CustomerDesign CD, Design D ";

            if (ddLocalQuality.SelectedIndex > 0)
            {
                strD = strD + " , CustomerQuality CQ ";
            }
            strD = strD + " Where CD.DesignId=D.DesignId  And CD.CustomerId=" + ddBuyerCode.SelectedValue;
            if (ddLocalDesign.SelectedIndex > 0)
            {
                strD = strD + " And D.DesignId=" + ddLocalDesign.SelectedValue;
            }
            if (ddLocalQuality.SelectedIndex > 0)
            {
                strD = strD + " And CD.CQSRNO = CQ.SrNo  And CQ.QualityID = " + ddLocalQuality.SelectedValue;
            }
            strD = strD + " Order By DesignNameAToC";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strD);
        }
        gdBuyerDesign.DataSource = ds;
        gdBuyerDesign.DataBind();
    }
    private void fill_Color_Grid()
    {
        DataSet ds = null;
        if (ddBuyerCode.SelectedIndex > 0 && ddLocalDesign.SelectedIndex > 0)
        {
            if (variable.VarMasterBuyercodeSeqWise == "1")
            {
                strC = @"select CC.SrNo Sr_No,CC.ColorNameToC + '  [' + C.ColorName + ']' Color,case when CC.Enable_Disable=1 Then 'Disable' Else 'Enable' ENd as Status,CC.Enable_Disable
                        From CustomerColor CC inner join CustomerDesign CD on CC.CustomerId=CD.CustomerId and CC.CDSRNO=CD.SrNo
                        inner join color c on CC.ColorId=C.ColorId
                        Where CC.CustomerId=" + ddBuyerCode.SelectedValue;
                if (ddLocalDesign.SelectedIndex > 0)
                {
                    strC = strC + " and  CD.DesignId=" + ddLocalDesign.SelectedValue;
                }
                if (ddLocalColor.SelectedIndex > 0)
                {
                    strC = strC + " And C.ColorId=" + ddLocalColor.SelectedValue;
                }
                strC = strC + " order By CC.ColorNameToC";
            }
            else
            {

                strC = "select SrNo Sr_No,ColorNameToC + '  [' + ColorName + ']' Color,case when CC.Enable_Disable=1 Then 'Disable' Else 'Enable' ENd as Status,CC.Enable_Disable from CustomerColor CC,Color C where CC.ColorId=C.ColorId  And CC.CustomerId=" + ddBuyerCode.SelectedValue;
                if (ddLocalColor.SelectedIndex > 0)
                {
                    strC = strC + " And C.ColorId=" + ddLocalColor.SelectedValue;
                }
                strC = strC + " order By ColorNameToC";

            }
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strC);
        }
        gdBuyerColor.DataSource = ds;
        gdBuyerColor.DataBind();
    }
    private void Fill_GridSize()
    {
        gdBuyerSize.DataSource = Fill_Grid_Data3();
        gdBuyerSize.DataBind();
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        lblErr.Text = "";

        if (Session["VarCompanyNo"].ToString() == "20")
        {
            if (ddItemname.SelectedIndex > 0)
            {
                if (ddLocalQuality.SelectedIndex <= 0 || txtBuyerQuality.Text == "")
                {
                    lblErr.Text = "Please select Quality or Enter Buyer Quality.";
                    return;
                }
            }

        }        
        getcustomerquality();

        if (variable.VarMasterBuyercodeSeqWise == "1")
        {
            if (ddLocalDesign.SelectedIndex > 0)
            {
                if (ddLocalQuality.SelectedIndex <= 0 || txtBuyerQuality.Text == "")
                {
                    lblErr.Text = "Please select Quality or Enter Buyer Quality.";
                    return;
                }
            }
            if (ddLocalColor.SelectedIndex > 0)
            {
                if (ddLocalDesign.SelectedIndex <= 0 || txtBuyerDesign.Text == "")
                {
                    lblErr.Text = "Please select Design or Enter Buyer Design.";
                    return;
                }
            }

        }
        getcustomerdesign();
        getcustomercolor();
        getcustomersize();

        fill_Quality_Grid();
        fill_Design_Grid();
        fill_Color_Grid();

        Fill_GridSize();
        if ((Session["varcompanyId"].ToString() == "30") || (Session["varcompanyId"].ToString() == "16") || (Session["varcompanyId"].ToString() == "37"))
        {
            UpdateSkuNo();
        }
        BtnSave.Text = "Save";

        switch (Session["varcompanyId"].ToString())
        {
            case "30":
            case "42":
            case "43":
            case "46":
                break;
            default:
                txtBuyerQuality.Text = "";
                txtBuyerDesign.Text = "";
                txtBuyerColor.Text = "";
                TxtBuyerFtLength.Text = "";
                TxtBuyerFtWidth.Text = "";
                TxtBuyerMtrLenth.Text = "";
                TxtBuyerMtrWidth.Text = "";
                txtinchlength.Text = "";
                txtInchWidth.Text = "";
                TxtBuyerFtHeight.Text = "";
                TxtBuyerMtrHeight.Text = "";
                txtinchHeight.Text = "";                
                hnQSRNO.Value = "0";
                hnDSRNO.Value = "0";
                hnCSRNO.Value = "0";
                txtSKUNo.Text = "";

                if (ddLocalQuality.Items.Count > 0)
                {
                    ddLocalQuality.SelectedIndex = 0;
                    fill_localQuality();
                }
                if (ddLocalDesign.Items.Count > 0)
                {
                    ddLocalDesign.SelectedIndex = 0;
                    fill_localDesign();
                }
                if (ddLocalColor.Items.Count > 0)
                {
                    ddLocalColor.SelectedIndex = 0;
                }
                if (ddSize.Items.Count > 0)
                {
                    ddSize.SelectedIndex = 0;
                    LblShowSize.Text = "";
                }
                break;
        }

    }
    protected void lnkquality_ED(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;
        if (lnk != null)
        {
            GridViewRow gvr = lnk.NamingContainer as GridViewRow;
            Label lblqualityenable_disable = (Label)gvr.FindControl("lblqualityenable_disable");
            string updateval = lblqualityenable_disable.Text == "1" ? "0" : "1";
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "update Customerquality set Enable_Disable=" + updateval + " where srno=" + gdBuyerQuality.DataKeys[gvr.RowIndex].Value + "");
            fill_Quality_Grid();
        }
    }
    protected void lnkdesign_ED(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;
        if (lnk != null)
        {
            GridViewRow gvr = lnk.NamingContainer as GridViewRow;
            Label lbldesignenable_disable = (Label)gvr.FindControl("lbldesignenable_disable");
            string updateval = lbldesignenable_disable.Text == "1" ? "0" : "1";
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "update customerdesign set Enable_Disable=" + updateval + " where srno=" + gdBuyerDesign.DataKeys[gvr.RowIndex].Value + "");
            fill_Design_Grid();
        }
    }
    protected void lnkcolor_ED(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;
        if (lnk != null)
        {
            GridViewRow gvr = lnk.NamingContainer as GridViewRow;
            Label lblcolorenable_disable = (Label)gvr.FindControl("lblcolorenable_disable");
            string updateval = lblcolorenable_disable.Text == "1" ? "0" : "1";
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "update customercolor set Enable_Disable=" + updateval + " where srno=" + gdBuyerColor.DataKeys[gvr.RowIndex].Value + "");
            fill_Color_Grid();
        }
    }
    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        CategorySelectedChanged();
    }
    protected void CategorySelectedChanged()
    {
        UtilityModule.ConditionalComboFill(ref ddItemname, "select item_id,item_name from item_master where category_id= " + ddCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By item_name", true, "--Select--");
    }
    protected void ddItemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_combo();
        fill_size();
        Fill_GridSize();
    }
    protected void ddLocalQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddLocalQualitySelectedChanged();
    }
    protected void ddLocalQualitySelectedChanged()
    {        
        if (ddLocalQuality.SelectedIndex > 0)
        {
            btnadddesign.Visible = true;
            fill_size();
        }
        else
        {
            txtBuyerQuality.Text = "";
        }
        fill_localQuality();
        fill_Quality_Grid();
        fill_Design_Grid();
        Fill_GridSize();
    }
    protected void ddLocalDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddLocalDesignSelectedChanged();
    }
    protected void ddLocalDesignSelectedChanged()
    {
        if (ddLocalDesign.SelectedIndex > 0)
        {
            btnaddcolor.Visible = true;
            fill_size();
            if (Session["varcompanyId"].ToString() == "30")
            {
                GetSkuNo();
            }
        }
        else
        {
            txtBuyerDesign.Text = "";
        }
        fill_localDesign();
        fill_Design_Grid();
        fill_Color_Grid();
        Fill_GridSize();
    }
    protected void ddShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_size();
        if (Session["varcompanyId"].ToString() == "30")
        {
            GetSkuNo();
        }
    }
    protected void ddSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddSizeSelectedChanged();
    }
    protected void ddSizeSelectedChanged()
    {
        fill_buyerSize();

        if (Session["varcompanyId"].ToString() == "30")
        {
            GetSkuNo();
        }
    }
    protected void Btnbakword_Click(object sender, EventArgs e)
    {
        if (ddLocalQuality.Items.Count > 0 && ddLocalQuality.SelectedIndex > 0)
        {
            txtBuyerQuality.Text = ddLocalQuality.SelectedItem.Text;
        }
        if (ddLocalDesign.Items.Count > 0 && ddLocalDesign.SelectedIndex > 0)
        {
            txtBuyerDesign.Text = ddLocalDesign.SelectedItem.Text;
        }
        if (ddLocalColor.Items.Count > 0 && ddLocalColor.SelectedIndex > 0)
        {
            txtBuyerColor.Text = ddLocalColor.SelectedItem.Text;
        }
    }
    protected void fill_localDesign()
    {
        string str;
        if (ViewState["WithBuyerCode"].ToString() == "1")
        {
            str = @"select Distinct C.ColorId,C.ColorName from ITEM_PARAMETER_MASTER IM inner join color C
                  on IM.COLOR_ID=C.ColorId  and IM.QUALITY_ID=" + ddLocalQuality.SelectedValue + " and Im.DESIGN_ID=" + ddLocalDesign.SelectedValue + " and C.MasterCompanyId=" + Session["varCompanyId"] + "  order by colorname";
        }
        else
        {
            str = "SELECT C.COLORID,C.COLORNAME FROM COLOR C Where MasterCompanyId=" + Session["varCompanyId"] + " Order By C.COLORNAME";
        }

        UtilityModule.ConditionalComboFill(ref ddLocalColor, str, true, "--Select--");
    }
    protected void fill_localQuality()
    {
        string str;
        if (ViewState["WithBuyerCode"].ToString() == "1")
        {
            str = @"select Distinct D.designId,D.designName from ITEM_PARAMETER_MASTER IM inner join Design D
                 on IM.DESIGN_ID=D.designId and IM.QUALITY_ID=" + ddLocalQuality.SelectedValue + " and D.MasterCompanyId=" + Session["varCompanyId"] + "  order by Designname";
        }
        else
        {
            str = "SELECT D.DESIGNID,D.DESIGNNAME FROM DESIGN D Where MasterCompanyId=" + Session["varCompanyId"] + " Order By D.DESIGNNAME";
        }
        UtilityModule.ConditionalComboFill(ref ddLocalDesign, str, true, "--Select--");
    }
    protected void fill_combo()
    {
        fill_Quality_Grid();
        string str;
        DataSet ds;
        if (ViewState["WithBuyerCode"].ToString() == "1")
        {
            //Both Shape and quality
            if (Session["varcompanyId"].ToString() == "20")
            {
                str = @"select Distinct q.QualityId,Q.QualityName from ITEM_PARAMETER_MASTER IM inner join Quality Q 
                  on IM.QUALITY_ID=Q.QualityId  and IM.ITEM_ID= " + ddItemname.SelectedValue + " and Q.mastercompanyId=" + Session["varcompanyId"] + @" order by Qualityname
                  SELECT Distinct ShapeId,ShapeName FROM Shape Where MasterCompanyId=" + Session["varCompanyId"] + " Order By ShapeId asc";

            }
            else
            {
                str = @"select Distinct q.QualityId,Q.QualityName from ITEM_PARAMETER_MASTER IM inner join Quality Q 
                  on IM.QUALITY_ID=Q.QualityId  and IM.ITEM_ID= " + ddItemname.SelectedValue + " and Q.mastercompanyId=" + Session["varcompanyId"] + @" order by Qualityname
                  select Distinct sh.ShapeId,sh.ShapeName from ITEM_PARAMETER_MASTER IM inner join shape sh 
                  on IM.SHAPE_ID=sh.ShapeId  and IM.ITEM_ID=" + ddItemname.SelectedValue + " and sh.MasterCompanyId=" + Session["varCompanyId"] + " Order By ShapeName";
            }
        }
        else
        {
            str = "SELECT QualityId,QualityName FROM QUALITY WHERE ITEM_ID=" + ddItemname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + @" Order By QualityName
                   SELECT Distinct ShapeId,ShapeName FROM Shape Where MasterCompanyId=" + Session["varCompanyId"] + " Order By ShapeName";
        }
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        UtilityModule.ConditionalComboFillWithDS(ref ddLocalQuality, ds, 0, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref ddShape, ds, 1, true, "--Select--");
        if (ddShape.Items.Count > 0)
        {
            ddShape.SelectedIndex = 1;
        }
    }
    private DataSet Fill_Grid_Data3()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            if (variable.VarNewQualitySize == "1")
            {
                if (Session["varcompanyId"].ToString() == "20")
                {
                    string ddItem;
                    string ddQuality;
                    if (ddItemname.SelectedIndex > 0)
                    {
                        ddItem = ddItemname.SelectedValue;
                    }
                    else
                    {
                        ddItem = "";
                    }
                    if (ddLocalQuality.SelectedIndex > 0)
                    {
                        ddQuality = ddLocalQuality.SelectedValue;
                    }
                    else
                    {
                        ddQuality = "";
                    }
                    ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "select SrNo  Sr_No,SizeNameAToC SIZEFt,MtSizeAToC SIZEMtr,inchsize as Sizeinch,QSN.Production_Ft_Format as ProdSizeft,QSN.Production_Mt_Format as ProdSizeMtr From Customersize cs inner join QualitySizeNew QSN on QSN.sizeid=cs.sizeid  where customerId=" + ddBuyerCode.SelectedValue + " and (QSN.QualityTypeId='" + ddItem + "' OR '" + ddItem + "' = '' ) and (QSN.QualityId='" + ddQuality + "' OR '" + ddQuality + "' = '' )  Order By SizeNameAToC");
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "select SrNo  Sr_No,SizeNameAToC SIZEFt,MtSizeAToC SIZEMtr,inchsize as Sizeinch,QSN.Production_Ft_Format as ProdSizeft,QSN.Production_Mt_Format as ProdSizeMtr From Customersize cs inner join QualitySizeNew QSN on QSN.sizeid=cs.sizeid  where customerId=" + ddBuyerCode.SelectedValue + " Order By SizeNameAToC");
                }
            }
            else
            {
                ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "select SrNo  Sr_No,SizeNameAToC SIZEFt,MtSizeAToC SIZEMtr,inchsize as Sizeinch,s.prodsizeft,s.prodsizemtr From Customersize cs inner join size s on s.sizeid=cs.sizeid where customerId=" + ddBuyerCode.SelectedValue + " Order By SizeNameAToC");
            }
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_Carpet_BuyerMasterCode|Fill_Grid_Data|" + ex.Message);
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
    private void getcustomerquality()
    {
        if (txtBuyerQuality.Text != "" && ddLocalQuality.SelectedIndex > 0 && ddBuyerCode.SelectedIndex > 0)
        {

            try
            {
                SqlParameter[] _arrPara = new SqlParameter[7];
                _arrPara[0] = new SqlParameter("@SrNo", SqlDbType.Int);
                _arrPara[0].Direction = ParameterDirection.InputOutput;
                _arrPara[1] = new SqlParameter("@QualityId", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@CustomerId", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@QualityNameAToC", SqlDbType.NVarChar, 50);
                _arrPara[4] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                _arrPara[6] = new SqlParameter("@Message", SqlDbType.Int);

                if (BtnSave.Text == "Update")
                {
                    //_arrPara[0].Value = gdBuyerQuality.SelectedDataKey.Value;
                    _arrPara[0].Value = hnQSRNO.Value;
                }
                else
                {
                    _arrPara[0].Value = 0;
                }
                _arrPara[1].Value = ddLocalQuality.SelectedValue;
                _arrPara[2].Value = ddBuyerCode.SelectedValue;
                _arrPara[3].Value = txtBuyerQuality.Text.ToUpper();
                _arrPara[4].Value = Session["varuserid"].ToString();
                _arrPara[5].Value = Session["varCompanyId"].ToString();
                _arrPara[6].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GET_CustomerQuality", _arrPara);
                hnCQsrno.Value = _arrPara[0].Value.ToString();
                if (_arrPara[6].Value.ToString() == "1")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Quality Name Already Exists');", true);
                }
            }
            catch (Exception ex)
            {

                Logs.WriteErrorLog("getcustomerquality|" + ex.Message);
            }

        }
    }
    private void getcustomerdesign()
    {
        if (txtBuyerDesign.Text != "" && ddLocalDesign.SelectedIndex > 0 && ddBuyerCode.SelectedIndex > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[8];
                _arrPara[0] = new SqlParameter("@SrNo", SqlDbType.Int);
                _arrPara[0].Direction = ParameterDirection.InputOutput;
                _arrPara[1] = new SqlParameter("@DesignId", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@CustomerId", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@DesignNameAToC", SqlDbType.NVarChar, 80);
                _arrPara[4] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                _arrPara[6] = new SqlParameter("@Message", SqlDbType.Int);
                _arrPara[7] = new SqlParameter("@CQSRNO", SqlDbType.Int);

                if (BtnSave.Text == "Update")
                {
                    //   _arrPara[0].Value = gdBuyerDesign.SelectedDataKey.Value;
                    _arrPara[0].Value = hnDSRNO.Value;
                }
                else
                {
                    _arrPara[0].Value = 0;
                }

                _arrPara[1].Value = ddLocalDesign.SelectedValue;
                _arrPara[2].Value = ddBuyerCode.SelectedValue;
                _arrPara[3].Value = txtBuyerDesign.Text.ToUpper();
                _arrPara[4].Value = Session["varuserid"].ToString();
                _arrPara[5].Value = Session["varCompanyId"].ToString();
                _arrPara[6].Direction = ParameterDirection.Output;
                _arrPara[7].Value = hnCQsrno.Value;
                con.Open();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_GET_CustomerDesign", _arrPara);
                hnCDsrno.Value = _arrPara[0].Value.ToString();
                if (_arrPara[6].Value.ToString() == "1")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Design Name Already Exists');", true);
                }
            }
            catch (Exception ex)
            {

                Logs.WriteErrorLog("getcustomerDesign|" + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con.Dispose();
            }
        }
    }

    private void getcustomercolor()
    {
        if (txtBuyerColor.Text != "" && ddLocalColor.SelectedIndex > 0 && ddBuyerCode.SelectedIndex > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[8];
                _arrPara[0] = new SqlParameter("@SrNo", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@ColorId", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@CustomerId", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@ColorNameToC", SqlDbType.NVarChar, 50);
                _arrPara[4] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                _arrPara[6] = new SqlParameter("@Message", SqlDbType.Int);
                _arrPara[7] = new SqlParameter("@CDSRNO", SqlDbType.Int);

                if (BtnSave.Text == "Update")
                {
                    // _arrPara[0].Value = gdBuyerColor.SelectedDataKey.Value;
                    _arrPara[0].Value = hnCSRNO.Value;
                }
                else
                {
                    _arrPara[0].Value = 0;
                }
                _arrPara[1].Value = ddLocalColor.SelectedValue;
                _arrPara[2].Value = ddBuyerCode.SelectedValue;
                _arrPara[3].Value = txtBuyerColor.Text.ToUpper();
                _arrPara[4].Value = Session["varuserid"].ToString();
                _arrPara[5].Value = Session["varCompanyId"].ToString();
                _arrPara[6].Direction = ParameterDirection.Output;
                _arrPara[7].Value = hnCDsrno.Value;
                con.Open();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_GET_CustomerColor", _arrPara);
                if (_arrPara[6].Value.ToString() == "1")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Color Name Already Exists');", true);
                }
            }
            catch (Exception ex)
            {

                Logs.WriteErrorLog("getcustomercolor|" + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
    }
    private void getcustomersize()
    {
        if (ddSize.SelectedValue != "" && TxtBuyerFtWidth.Text != "" && TxtBuyerFtLength.Text != "" && TxtBuyerMtrWidth.Text != "" && TxtBuyerMtrLenth.Text != "")
        {
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[10];
                _arrPara[0] = new SqlParameter("@SrNo", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@SizeId", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@CustomerId", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@SizeNameAToC", SqlDbType.NVarChar, 30);
                _arrPara[4] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                _arrPara[6] = new SqlParameter("@BuyerMtrFormat", SqlDbType.NVarChar, 30);
                _arrPara[7] = new SqlParameter("@Message", SqlDbType.Int);
                _arrPara[8] = new SqlParameter("@Message1", SqlDbType.Int);
                _arrPara[9] = new SqlParameter("@InchSize", SqlDbType.VarChar, 20);

                //if (BtnSave.Text == "Update")
                //{
                //    _arrPara[0].Value = gdBuyerSize.SelectedDataKey.Value;
                //}
                //else
                //{
                //    _arrPara[0].Value = 0;
                //}
                _arrPara[0].Direction = ParameterDirection.Output;
                _arrPara[2].Value = ddBuyerCode.SelectedValue;
                _arrPara[1].Value = ddSize.SelectedValue;
                if (TxtBuyerFtHeight.Text == "" || TxtBuyerFtHeight.Text == "0")
                {
                    _arrPara[3].Value = TxtBuyerFtWidth.Text + "x" + TxtBuyerFtLength.Text;
                }
                else
                {
                    _arrPara[3].Value = TxtBuyerFtWidth.Text + "x" + TxtBuyerFtLength.Text + "x" + TxtBuyerFtHeight.Text;
                }
                _arrPara[4].Value = Session["varuserid"].ToString();
                _arrPara[5].Value = Session["varCompanyId"].ToString();
                if (TxtBuyerMtrHeight.Text == "" || TxtBuyerMtrHeight.Text == "0")
                {
                    _arrPara[6].Value = TxtBuyerMtrWidth.Text + "x" + TxtBuyerMtrLenth.Text;
                }
                else
                {
                    _arrPara[6].Value = TxtBuyerMtrWidth.Text + "x" + TxtBuyerMtrLenth.Text + "x" + TxtBuyerMtrHeight.Text;
                }
                _arrPara[7].Direction = ParameterDirection.Output;
                _arrPara[8].Direction = ParameterDirection.Output;

                if (txtinchHeight.Text == "" || txtinchHeight.Text == "0")
                {
                    _arrPara[9].Value = txtInchWidth.Text + "x" + txtinchlength.Text;
                }
                else
                {
                    _arrPara[9].Value = txtInchWidth.Text + "x" + txtinchlength.Text + "x" + txtinchHeight.Text;
                }

                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GET_CustomerSize", _arrPara);
                if (_arrPara[7].Value.ToString() == "1" || _arrPara[8].Value.ToString() == "1")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Size Name Already Exists');", true);
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("getcustomersize|" + ex.Message);
            }
        }
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully logedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");

    }
    protected void ChkForMtrSize_CheckedChanged(object sender, EventArgs e)
    {
        fill_size();
    }
    protected void fill_size()
    {
        string str = "";
        string Size, ProdSize;

        if (variable.VarNewQualitySize == "1")
        {
            switch (DDsizetype.SelectedValue)
            {
                case "0":
                    Size = "Export_Format";
                    ProdSize = "Production_Ft_Format";
                    TxtUnitID.Text = "2";
                    break;
                case "1":
                    Size = "MtrSize";
                    ProdSize = "Production_Mt_Format";
                    TxtUnitID.Text = "1";
                    break;
                case "2":
                    Size = "MtrSize";
                    ProdSize = "Production_Ft_Format";
                    TxtUnitID.Text = "6";
                    break;
                default:
                    Size = "Export_Format";
                    ProdSize = "Production_Ft_Format";
                    TxtUnitID.Text = "2";
                    break;
            }
        }
        else
        {
            switch (DDsizetype.SelectedValue)
            {
                case "0":
                    Size = "Sizeft";
                    ProdSize = "ProdSizeft";
                    TxtUnitID.Text = "2";
                    break;
                case "1":
                    Size = "Sizemtr";
                    ProdSize = "ProdSizemtr";
                    TxtUnitID.Text = "1";
                    break;
                case "2":
                    Size = "Sizeinch";
                    ProdSize = "ProdSizeft";
                    TxtUnitID.Text = "6";
                    break;
                default:
                    Size = "Sizeft";
                    ProdSize = "ProdSizeft";
                    TxtUnitID.Text = "2";
                    break;
            }
        }
        //im.QUALITY_ID,im.DESIGN_ID,im.COLOR_ID
        if (ViewState["WithBuyerCode"].ToString() == "1")
        {
            if (variable.VarNewQualitySize == "1")
            {
                if (Session["varcompanyId"].ToString() == "20")
                {
                    str = @"Select Distinct QSN.SizeId,QSN." + Size + "+'  '+'['+QSN." + ProdSize + @"+']' as Sizeft from QualitySizeNew QSN
                       where QSN.ShapeId=" + ddShape.SelectedValue + " and QSN.QualityTypeId=" + ddItemname.SelectedValue;
                }
                else
                {
                    str = @"Select Distinct QSN.SizeId,QSN." + Size + "+'  '+'['+QSN." + ProdSize + @"+']' as Sizeft from ITEM_PARAMETER_MASTER IM inner join QualitySizeNew QSN
                      on IM.size_id=QSN.Sizeid  where QSN.ShapeId=" + ddShape.SelectedValue + " and Im.Item_Id=" + ddItemname.SelectedValue;
                }
            }
            else
            {

                if (Session["varcompanyid"].ToString() == "44")
                {
                    switch (DDsizetype.SelectedValue.ToString())
                    {
                        case "0":
                            str = "Select Distinct S.sizeid,cast(s.WidthFt as varchar)+'x'+cast(s.LengthFt as varchar) +case when s.Heightft>0 then 'x'+cast(s.HeightFt as varchar) +'  '+'['+S." + ProdSize + @"+']' else ''  end as  SizeFt  from Size S  Where S.shapeid=" + ddShape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
                            break;
                        case "1":
                            str = " Select Distinct S.sizeid,cast(s.WidthMtr as varchar)+'x'+cast(s.LengthMtr as varchar) +case when s.HeightMtr>0 then 'x'+cast(s.HeightMtr as varchar)  +'  '+'['+S." + ProdSize + @"+']'  else ''  end as  Sizemtr from Size S Where S.shapeid=" + ddShape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];

                            break;
                        case "2":
                            str = "Select Distinct S.sizeid,cast(s.WidthInch as varchar)+'x'+cast(s.LengthInch as varchar) +case when s.HeightInch>0 then 'x'+cast(s.HeightInch as varchar) +'  '+'['+S." + ProdSize + @"+']' else ''  end as  Sizeinch from Size S  Where S.shapeid=" + ddShape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
                            break;
                        default:
                            str = "Select Distinct S.sizeid,cast(s.WidthFt as varchar)+'x'+cast(s.LengthFt as varchar) +case when s.Heightft>0 then 'x'+cast(s.HeightFt as varchar) +'  '+'['+S." + ProdSize + @"+']' else ''  end as  SizeFt   from Size S  Where S.shapeid=" + ddShape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
                            break;
                    }


                }
                else
                {



                    str = @"Select Distinct S.SizeId,S." + Size + "+'  '+'['+S." + ProdSize + @"+']' as Sizeft from ITEM_PARAMETER_MASTER IM inner join Size S
                      on IM.size_id=S.Sizeid  where S.ShapeId=" + ddShape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + "  and Im.Item_Id=" + ddItemname.SelectedValue;
                }
            }

            if (ddLocalQuality.SelectedIndex > 0)
            {
                if (Session["varcompanyId"].ToString() == "20")
                {
                    str = str + " and QSN.QualityId=" + ddLocalQuality.SelectedValue;
                }
                else
                {
                    str = str + " and im.QUALITY_ID=" + ddLocalQuality.SelectedValue;
                }

            }
            if (Session["varcompanyId"].ToString() != "20")
            {
                if (ddLocalDesign.SelectedIndex > 0)
                {
                    str = str + " and im.Design_id=" + ddLocalDesign.SelectedValue;
                }
                if (ddLocalColor.SelectedIndex > 0)
                {
                    str = str + " and im.color_id=" + ddLocalColor.SelectedValue;
                }
            }

            str = str + " Order By Sizeft";

        }
        else
        {
            if (variable.VarNewQualitySize == "1")
            {
                str = "Select SizeId," + Size + "+'  '+'['+" + ProdSize + "+']' as Sizeft from QualitySizeNew where ShapeId=" + ddShape.SelectedValue + "  Order By " + Size;
            }
            else
            {
                if (Session["varcompanyid"].ToString() == "44")
                {
                    switch (DDsizetype.SelectedValue.ToString())
                    {
                        case "0":
                            str = "Select Distinct S.sizeid,cast(s.WidthFt as varchar)+'x'+cast(s.LengthFt as varchar) +case when s.Heightft>0 then 'x'+cast(s.HeightFt as varchar) +'  '+'['+S." + ProdSize + @"+']' else ''  end as  SizeFt  from Size S  Where S.shapeid=" + ddShape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
                            break;
                        case "1":
                            str = " Select Distinct S.sizeid,cast(s.WidthMtr as varchar)+'x'+cast(s.LengthMtr as varchar) +case when s.HeightMtr>0 then 'x'+cast(s.HeightMtr as varchar)  +'  '+'['+S." + ProdSize + @"+']'  else ''  end as  Sizemtr from Size S Where S.shapeid=" + ddShape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];

                            break;
                        case "2":
                            str = "Select Distinct S.sizeid,cast(s.WidthInch as varchar)+'x'+cast(s.LengthInch as varchar) +case when s.HeightInch>0 then 'x'+cast(s.HeightInch as varchar) +'  '+'['+S." + ProdSize + @"+']' else ''  end as  Sizeinch from Size S  Where S.shapeid=" + ddShape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
                            break;
                        default:
                            str = "Select Distinct S.sizeid,cast(s.WidthFt as varchar)+'x'+cast(s.LengthFt as varchar) +case when s.Heightft>0 then 'x'+cast(s.HeightFt as varchar) +'  '+'['+S." + ProdSize + @"+']' else ''  end as  SizeFt   from Size S  Where S.shapeid=" + ddShape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
                            break;
                    }


                }
                else
                {


                    str = "Select SizeId," + Size + "+'  '+'['+" + ProdSize + "+']' as Sizeft from Size where ShapeId=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By " + Size;
                }
            }
        }

        UtilityModule.ConditionalComboFill(ref ddSize, str, true, "--select--");

    }
    protected void fill_buyerSize()
    {
        if (ddSize.SelectedIndex > 0)
        {
            int VarNo = 0;

            switch (DDsizetype.SelectedValue)
            {
                case "0":
                    VarNo = 0; //ft
                    break;
                case "1":
                    VarNo = 1; //mtr
                    break;
                case "2":
                    VarNo = 2; //inch
                    break;
            }

            string Str=string.Empty;
            if (variable.VarNewQualitySize == "1")
            {
                Str = @"Select Case When " + VarNo + "=0 Then Export_Format Else case When " + VarNo + @"=1 Then MtrSize Else MtrSize End End as Size,Export_Format,MtrSize,MtrSize
                           From QualitySizeNew Where SizeId=" + ddSize.SelectedValue + " ";
            }
            else
            {
                if (Session["varcompanyid"].ToString() == "44")
                {


                    Str = "Select Case When 2=0 Then cast(s.WidthFt as varchar)+'x'+cast(s.LengthFt as varchar) +case when s.Heightft>0 then 'x'+cast(s.HeightFt as varchar)  else ''  end Else case When 2=1 Then cast(s.WidthMtr as varchar)+'x'+cast(s.LengthMtr as varchar) +case when s.HeightMtr>0 then 'x'+cast(s.HeightMtr as varchar) else ''  end Else cast(s.WidthInch as varchar)+'x'+cast(s.LengthInch as varchar) +case when s.HeightInch>0 then 'x'+cast(s.HeightInch as varchar) else '' END End End as Size,cast(s.WidthInch as varchar)+'x'+cast(s.LengthInch as varchar) +case when s.HeightInch>0 then 'x'+cast(s.HeightInch as varchar) else '' END AS SIZEINCH,cast(s.WidthMtr as varchar)+'x'+cast(s.LengthMtr as varchar) +case when s.HeightMtr>0 then 'x'+cast(s.HeightMtr as varchar) else ''  end AS SIZEMTR,cast(s.WidthFt as varchar)+'x'+cast(s.LengthFt as varchar) +case when s.Heightft>0 then 'x'+cast(s.HeightFt as varchar)  else ''  end AS SIZEFT From Size S  Where SizeId=" + ddSize.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
                           
                    


                }
                else
                {
                    Str = @"Select Case When " + VarNo + "=0 Then SIZEFt Else case When " + VarNo + @"=1 Then SIZEMtr Else Sizeinch End End as Size,Sizeft,SizeMtr,Sizeinch
                           From Size Where SizeId=" + ddSize.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
                }
            }

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblShowSize.Text = ds.Tables[0].Rows[0]["Size"].ToString();
                //if (VarNo == 1)
                //{
                //    TxtBuyerFtWidth.Text = (LblShowSize.Text.Split('x')[0]).ToString();
                //    TxtBuyerFtLength.Text = (LblShowSize.Text.Split('x')[1]).ToString();                    
                //    TxtBuyerMtrWidth.Text = ds.Tables[0].Rows[0]["Sizemtr"].ToString().Split('x')[0];
                //    TxtBuyerMtrLenth.Text = ds.Tables[0].Rows[0]["Sizemtr"].ToString().Split('x')[1];
                //}
                //else if (VarNo == 0)
                //{
                //    TxtBuyerFtWidth.Text = ds.Tables[0].Rows[0]["Sizeft"].ToString().Split('x')[0];
                //    TxtBuyerFtLength.Text = ds.Tables[0].Rows[0]["sizeft"].ToString().Split('x')[1];
                //    TxtBuyerMtrWidth.Text = (LblShowSize.Text.Split('x')[0]).ToString();
                //    TxtBuyerMtrLenth.Text = (LblShowSize.Text.Split('x')[1]).ToString();
                //}
                //else if(VarNo==2)
                //{
                //    TxtBuyerFtWidth.Text = ds.Tables[0].Rows[0]["Sizeft"].ToString().Split('x')[0];
                //    TxtBuyerFtLength.Text = ds.Tables[0].Rows[0]["sizeft"].ToString().Split('x')[1];
                //    TxtBuyerMtrWidth.Text = (LblShowSize.Text.Split('x')[0]).ToString();
                //    TxtBuyerMtrLenth.Text = (LblShowSize.Text.Split('x')[1]).ToString();
                //}

                if (variable.VarNewQualitySize == "1")
                {
                    TxtBuyerFtWidth.Text = ds.Tables[0].Rows[0]["Export_Format"].ToString().Split('x')[0];
                    TxtBuyerFtLength.Text = ds.Tables[0].Rows[0]["Export_Format"].ToString().Split('x')[1];
                    TxtBuyerMtrWidth.Text = ds.Tables[0].Rows[0]["MtrSize"].ToString().Split('x')[0];
                    TxtBuyerMtrLenth.Text = ds.Tables[0].Rows[0]["MtrSize"].ToString().Split('x')[1];
                    txtInchWidth.Text = ds.Tables[0].Rows[0]["MtrSize"].ToString().Split('x')[0];
                    txtinchlength.Text = ds.Tables[0].Rows[0]["MtrSize"].ToString().Split('x')[1];
                }
                else
                {
                    TxtBuyerFtWidth.Text = ds.Tables[0].Rows[0]["Sizeft"].ToString().Split('x')[0];
                    TxtBuyerFtLength.Text = ds.Tables[0].Rows[0]["sizeft"].ToString().Split('x')[1];
                    TxtBuyerMtrWidth.Text = ds.Tables[0].Rows[0]["Sizemtr"].ToString().Split('x')[0];
                    TxtBuyerMtrLenth.Text = ds.Tables[0].Rows[0]["Sizemtr"].ToString().Split('x')[1];
                    txtInchWidth.Text = ds.Tables[0].Rows[0]["sizeinch"].ToString().Split('x')[0];
                    txtinchlength.Text = ds.Tables[0].Rows[0]["sizeinch"].ToString().Split('x')[1];
                    TxtBuyerFtHeight.Text = "0";
                    TxtBuyerMtrHeight.Text = "0";
                    txtinchHeight.Text = "0";
                    if ((ds.Tables[0].Rows[0]["Sizeft"].ToString().Split('x')).Length.ToString() == "3")
                    {
                        TxtBuyerFtHeight.Text = ds.Tables[0].Rows[0]["sizeft"].ToString().Split('x')[2];
                        TxtBuyerMtrHeight.Text = ds.Tables[0].Rows[0]["Sizemtr"].ToString().Split('x')[2];
                        txtinchHeight.Text = ds.Tables[0].Rows[0]["sizeinch"].ToString().Split('x')[2];
                    }
                }
            }
        }
    }
    protected void ddLocalColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddLocalColorSelectedChanged();
    }
    protected void ddLocalColorSelectedChanged()
    {
        fill_Color_Grid();
        if (ddLocalColor.SelectedIndex == 0)
        {
            txtBuyerColor.Text = "";
        }
        fill_size();
        if (Session["varcompanyId"].ToString() == "30")
        {
            GetSkuNo();
        }
    }
    protected void BtnRefereceCustomer_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddBuyerCode, "select Customerid,Customercode From Customerinfo Where MasterCompanyId=" + Session["varCompanyId"] + "  order by Customercode", true, "--Select--");
    }
    protected void refreshcategory_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddCategory, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + "  Order by CATEGORY_Id desc", true, "--Select--");
        if (ddCategory.Items.Count > 0)
        {
            ddCategory.SelectedValue = "1";
        }
    }
    protected void refreshitem_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddItemname, "select item_id,item_name from item_master where category_id= " + ddCategory.SelectedValue + " ANd MasterCompanyId=" + Session["varCompanyId"] + "  Order By item_id desc", true, "--Select--");
    }
    protected void refreshquality_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddLocalQuality, @"SELECT QualityId,QualityName FROM QUALITY WHERE ITEM_ID=" + ddItemname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "  Order By QualityId desc", true, "--Select--");
    }
    protected void refreshdesign_Click(object sender, EventArgs e)
    {
        fill_localQuality();

        //UtilityModule.ConditionalComboFill(ref ddLocalDesign, @"SELECT DESIGNID,DESIGNNAME FROM DESIGN Order By DESIGNID desc", true, "--Select--");
    }
    protected void refreshcolor_Click(object sender, EventArgs e)
    {
        //UtilityModule.ConditionalComboFill(ref ddLocalColor, @"SELECT COLORID,COLORNAME FROM COLOR Order By COLORID desc", true, "--Select--");
        fill_localDesign();
    }
    protected void refreshshape_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddShape, @"SELECT Distinct ShapeId,ShapeName FROM Shape Where MasterCompanyId=" + Session["varCompanyId"] + "  Order By ShapeId desc", true, "--Select--");
        if (ddShape.Items.Count > 0)
        {
            ddShape.SelectedIndex = 1;
        }
    }
    protected void BtnRefreshSize_Click(object sender, EventArgs e)
    {
        if (variable.VarNewQualitySize == "1")
        {
            UtilityModule.ConditionalComboFill(ref ddSize, @"Select SizeId,MtrSize+'  '+'['+Production_Mt_Format+']' as SIZEMtr from QualitySizeNew where ShapeId=" + ddShape.SelectedValue + "  Order By SizeId desc", true, "--select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddSize, @"Select SizeId,SIZEMtr+'  '+'['+ProdSizeMtr+']' as SIZEMtr from Size where ShapeId=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "  Order By SizeId desc", true, "--select--");
        }
    }
    protected void gdBuyerQuality_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdBuyerQuality, "Select$" + e.Row.RowIndex);
            Label lblqualityenable_disable = (Label)e.Row.FindControl("lblqualityenable_disable");
            if (lblqualityenable_disable.Text == "0")
            {
                e.Row.BackColor = System.Drawing.Color.Red;
            }
        }
    }
    protected void gdBuyerDesign_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdBuyerDesign, "Select$" + e.Row.RowIndex);

            Label lbldesignenable_disable = (Label)e.Row.FindControl("lbldesignenable_disable");
            if (lbldesignenable_disable.Text == "0")
            {
                e.Row.BackColor = System.Drawing.Color.Red;
            }
        }

    }
    protected void gdBuyerColor_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdBuyerColor, "Select$" + e.Row.RowIndex);

            Label lblcolorenable_disable = (Label)e.Row.FindControl("lblcolorenable_disable");
            if (lblcolorenable_disable.Text == "0")
            {
                e.Row.BackColor = System.Drawing.Color.Red;
            }
        }
    }
    protected void gdBuyerSize_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (Session["varcompanyId"].ToString() == "9")
            {
                e.Row.Cells[1].Text = "Size CM Format";
                e.Row.Cells[4].Text = "Prod. CM Format";
            }

            if (Session["varcompanyId"].ToString() == "20")
            {
                gdBuyerSize.Columns[2].Visible = false;

            }

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdBuyerSize, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gdBuyerQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = null;
        hnQSRNO.Value = "0";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            string sql = @"Select SrNo,QualityID,QualityNameAToC from CustomerQuality Where SrNo=" + gdBuyerQuality.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                fill_combo();
                ddLocalQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityID"].ToString();
                txtBuyerQuality.Text = ds.Tables[0].Rows[0]["QualityNameAToC"].ToString();
                hnQSRNO.Value = gdBuyerQuality.SelectedValue.ToString();
                BtnSave.Text = "Update";
            }
        }
        catch (Exception ex)
        {
            lblErr.Visible = true;
            lblErr.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void gdBuyerDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = null;
        hnDSRNO.Value = "0";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            //string sql = @"Select SrNo,DesignID,DesignNameAToC from CustomerDesign Where SrNo=" + gdBuyerDesign.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
            string sql = @"select Distinct CD.SrNo,CD.DesignId,Cd.DesignNameAToC,isnull(CQ.QualityId,0) as QualityId,isnull(CQ.QualityNameAToC,'') as QualityNameAToC From CustomerDesign CD left join CustomerQuality CQ ON CD.CQSRNO=CQ.SrNo Where 
                           CD.srno=" + gdBuyerDesign.SelectedValue + " and CD.Mastercompanyid=" + Session["varcompanyId"] + "";

            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                UtilityModule.ConditionalComboFill(ref ddLocalDesign, "Select DesignId,DesignName from Design Where DesignId=" + ds.Tables[0].Rows[0]["DesignID"] + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--select--");
                ddLocalDesign.SelectedValue = ds.Tables[0].Rows[0]["DesignID"].ToString();
                txtBuyerDesign.Text = ds.Tables[0].Rows[0]["DesignNameAToC"].ToString();
                hnDSRNO.Value = gdBuyerDesign.SelectedValue.ToString();
                BtnSave.Text = "Update";
                //***************
                if (ddLocalQuality.Items.FindByValue(ds.Tables[0].Rows[0]["QualityId"].ToString()) != null)
                {
                    ddLocalQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
                }
                else
                {
                    ddLocalQuality.SelectedIndex = -1;
                }
                txtBuyerQuality.Text = ds.Tables[0].Rows[0]["QualityNameAToC"].ToString();
            }
        }
        catch (Exception ex)
        {
            lblErr.Visible = true;
            lblErr.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void gdBuyerColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = null;
        hnCSRNO.Value = "0";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            //string sql = @"SELECT SrNo,ColorId,ColorNameToC from CustomerColor Where Customerid=" + ddBuyerCode.SelectedValue + " And SrNo=" + gdBuyerColor.SelectedValue;
            string sql = @"select  Distinct CC.SrNo,cc.ColorId,CC.ColorNameToC,ISNULL(CD.designId,0) as DesignId,ISNULL(CQ.QualityId,0) as Qualityid,
                            isnull(CD.DesignNameAToC,'') as DesignNameAtoc,Isnull(CQ.QualityNameAToC,'') as QualityNameAtoc
                            From CustomerColor CC Left Join CustomerDesign cd on cc.CDSRNO=Cd.SrNo
                            Left join CustomerQuality cq on cd.CQSRNO=Cq.SrNo Where CC.CustomerId=" + ddBuyerCode.SelectedValue + " and CC.srno=" + gdBuyerColor.SelectedValue;
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                UtilityModule.ConditionalComboFill(ref ddLocalColor, "Select ColorId,ColorName from Color Where ColorId=" + ds.Tables[0].Rows[0]["ColorId"] + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--select--");
                ddLocalColor.SelectedValue = ds.Tables[0].Rows[0]["ColorId"].ToString();
                txtBuyerColor.Text = ds.Tables[0].Rows[0]["ColorNameToC"].ToString();
                hnCSRNO.Value = gdBuyerColor.SelectedValue.ToString();
                //**********
                if (ddLocalQuality.Items.FindByValue(ds.Tables[0].Rows[0]["QualityId"].ToString()) != null)
                {
                    ddLocalQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
                    txtBuyerQuality.Text = ds.Tables[0].Rows[0]["QualityNameAtoc"].ToString();
                    ddLocalQuality_SelectedIndexChanged(sender, new EventArgs());
                }
                if (ddLocalDesign.Items.FindByValue(ds.Tables[0].Rows[0]["DesignId"].ToString()) != null)
                {
                    ddLocalDesign.SelectedValue = ds.Tables[0].Rows[0]["DesignId"].ToString();
                    txtBuyerDesign.Text = ds.Tables[0].Rows[0]["DesignNameAtoc"].ToString();
                }
                BtnSave.Text = "Update";
            }
        }
        catch (Exception ex)
        {
            lblErr.Visible = true;
            lblErr.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    //protected void gdBuyerSize_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    DataSet ds = null;
    //    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //    con.Open();
    //    try
    //    {
    //        string sql = @"SELECT SrNo,Sizeid,SizeNameAToC,MtSizeAToC from CustomerSize Where Customerid=" + ddBuyerCode.SelectedValue + " And SrNo=" + gdBuyerSize.SelectedValue;
    //        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            //UtilityModule.ConditionalComboFill(ref ddLocalColor, "Select ColorId,ColorName from Color Where ColorId=" + ds.Tables[0].Rows[0]["SizeId"] + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--select--");
    //            //ddLocalColor.SelectedValue = ds.Tables[0].Rows[0]["ColorId"].ToString();
    //            //txtBuyerColor.Text = ds.Tables[0].Rows[0]["ColorNameToC"].ToString();
    //            fill_size();
    //            ddSize.SelectedValue = ds.Tables[0].Rows[0]["Sizeid"].ToString();               
    //            fill_buyerSize();
    //            BtnSave.Text = "Update";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErr.Visible = true;
    //        lblErr.Text = ex.Message;
    //    }
    //    finally
    //    {
    //        con.Close();
    //        con.Dispose();
    //    }
    //}
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["ABC"] == "1")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "CloseForm();", true);
        }
        else
        {
            Response.Redirect("~/main.aspx");
        }
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_size();
    }

    protected void gdBuyerQuality_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string id = ((Label)gdBuyerQuality.Rows[e.RowIndex].FindControl("lblId2")).Text;


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@SrNo", id);
            param[1] = new SqlParameter("@MasterCompanyid", Session["VarCompanyId"]);
            param[2] = new SqlParameter("@UserId", Session["VarUserId"]);

            param[3] = new SqlParameter("@VarMsg", SqlDbType.VarChar, 500);
            param[3].Direction = ParameterDirection.Output;


            //**********
            //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveDesignRatioSizeWise", param);
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_Delete_CustomerQuality", param);

            lblErr.Text = param[3].Value.ToString();

            Tran.Commit();
            fill_Quality_Grid();

        }
        catch (Exception ex)
        {
            lblErr.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void gdBuyerDesign_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string id = ((Label)gdBuyerDesign.Rows[e.RowIndex].FindControl("lblDesignId")).Text;

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@SrNo", id);
            param[1] = new SqlParameter("@MasterCompanyid", Session["VarCompanyId"]);
            param[2] = new SqlParameter("@UserId", Session["VarUserId"]);

            param[3] = new SqlParameter("@VarMsg", SqlDbType.VarChar, 500);
            param[3].Direction = ParameterDirection.Output;

            //**********
            //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveDesignRatioSizeWise", param);
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_Delete_CustomerDesign", param);

            lblErr.Text = param[3].Value.ToString();

            Tran.Commit();
            fill_Design_Grid();
            //fill_Color_Grid();

            //Fill_GridSize();
        }
        catch (Exception ex)
        {
            lblErr.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void gdBuyerColor_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string id = ((Label)gdBuyerColor.Rows[e.RowIndex].FindControl("lblColorId")).Text;

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@SrNo", id);
            param[1] = new SqlParameter("@MasterCompanyid", Session["VarCompanyId"]);
            param[2] = new SqlParameter("@UserId", Session["VarUserId"]);

            param[3] = new SqlParameter("@VarMsg", SqlDbType.VarChar, 500);
            param[3].Direction = ParameterDirection.Output;

            //**********
            //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveDesignRatioSizeWise", param);
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_Delete_CustomerColor", param);

            lblErr.Text = param[3].Value.ToString();

            Tran.Commit();
            fill_Color_Grid();

            //Fill_GridSize();
        }
        catch (Exception ex)
        {
            lblErr.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    private void GetSkuNo()
    {
        if (ddItemname.SelectedIndex > 0 && ddLocalQuality.SelectedIndex > 0 && ddLocalDesign.SelectedIndex > 0 && ddLocalColor.SelectedIndex > 0 && ddShape.SelectedIndex > 0 && ddSize.SelectedIndex > 0)
        {

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[9];
                _arrPara[0] = new SqlParameter("@ItemID", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@QualityId", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@DesignId", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@ColorId", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@ShapeId", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@SizeId", SqlDbType.Int);
                _arrPara[6] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[7] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                _arrPara[8] = new SqlParameter("@Message", SqlDbType.Int);


                _arrPara[0].Value = ddItemname.SelectedValue;
                _arrPara[1].Value = ddLocalQuality.SelectedValue;
                _arrPara[2].Value = ddLocalDesign.SelectedValue;
                _arrPara[3].Value = ddLocalColor.SelectedValue;
                _arrPara[4].Value = ddShape.SelectedValue;
                _arrPara[5].Value = ddSize.SelectedValue;
                _arrPara[6].Value = Session["varuserid"].ToString();
                _arrPara[7].Value = Session["varCompanyId"].ToString();
                _arrPara[8].Direction = ParameterDirection.Output;

                //SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_UpdateSkuNo", _arrPara);
                DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "PRO_GetSkuNo", _arrPara);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtSKUNo.Text = ds.Tables[0].Rows[0]["Sku_no"].ToString();
                }
                else
                {
                    txtSKUNo.Text = "";
                }

            }
            catch (Exception ex)
            {
                Tran.Rollback();
                UtilityModule.MessageAlert(ex.Message, "Master/BuyerMasterCode.aspx");
                //Logs.WriteErrorLog("GetSkuNo|" + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
    }
    private void UpdateSkuNo()
    {
        if (ddItemname.SelectedIndex > 0 && ddLocalQuality.SelectedIndex > 0 && ddLocalDesign.SelectedIndex > 0 && ddLocalColor.SelectedIndex > 0 && ddShape.SelectedIndex > 0 && ddSize.SelectedIndex > 0)
        {
            lblErr.Text = "";
            //lblErr.Visible = false;           
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[12];
                _arrPara[0] = new SqlParameter("@ItemID", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@QualityId", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@DesignId", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@ColorId", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@ShapeId", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@SizeId", SqlDbType.Int);
                _arrPara[6] = new SqlParameter("@SkuNo", SqlDbType.VarChar, 200);

                _arrPara[7] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[8] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                _arrPara[9] = new SqlParameter("@Message", SqlDbType.Int);
                _arrPara[10] = new SqlParameter("@compo", SqlDbType.VarChar, 200);
                _arrPara[11] = new SqlParameter("@skudesc", SqlDbType.VarChar, 200);


                _arrPara[0].Value = ddItemname.SelectedValue;
                _arrPara[1].Value = ddLocalQuality.SelectedValue;
                _arrPara[2].Value = ddLocalDesign.SelectedValue;
                _arrPara[3].Value = ddLocalColor.SelectedValue;
                _arrPara[4].Value = ddShape.SelectedValue;
                _arrPara[5].Value = ddSize.SelectedValue;
                _arrPara[6].Value = txtSKUNo.Text;
                _arrPara[7].Value = Session["varuserid"].ToString();
                _arrPara[8].Value = Session["varCompanyId"].ToString();
                _arrPara[9].Direction = ParameterDirection.Output;
                _arrPara[10].Value = txtcomposition.Text;
                _arrPara[11].Value = txtskudesc.Text;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UpdateSkuNo", _arrPara);
                lblErr.Text = _arrPara[9].Value.ToString();
                Tran.Commit();
                //if (_arrPara[9].Value.ToString() == "1")
                //{
                //    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Sku No Already Exists');", true);
                //}
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                UtilityModule.MessageAlert(ex.Message, "Master/BuyerMasterCode.aspx");
                //Logs.WriteErrorLog("UpdateSkuNo|" + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
    }
    protected void TxtSampleCode_TextChanged(object sender, EventArgs e)
    {
        string str = @"Select SDM.customerid, SDM.Categoryid, SDM.Itemid, SDM.Qualityid, SDM.DesignName, SDM.ColorName, SDM.shapeid, SDM.Sizeflag, SDM.Sizeid, 
                        D.DesignID, C.ColorID 
                        From SampleDevelopmentMaster SDM(Nolock) 
                        JOIN Design D(Nolock) ON D.DesignName = SDM.DesignName 
                        JOIN Color C(Nolock) ON C.ColorName = SDM.ColorName
                        Where SDM.Samplecode='" + TxtSampleCode.Text + @"'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddCategory.SelectedValue = ds.Tables[0].Rows[0]["Categoryid"].ToString();
            CategorySelectedChanged();
            ddItemname.SelectedValue = ds.Tables[0].Rows[0]["Itemid"].ToString();
            fill_combo();
            ddLocalQuality.SelectedValue = ds.Tables[0].Rows[0]["Qualityid"].ToString();
            ddShape.SelectedValue = ds.Tables[0].Rows[0]["shapeid"].ToString();
            ddLocalQualitySelectedChanged();
            ddLocalDesign.SelectedValue = ds.Tables[0].Rows[0]["DesignID"].ToString();
            ddLocalDesignSelectedChanged();
            ddLocalColor.SelectedValue = ds.Tables[0].Rows[0]["ColorID"].ToString();
            ddLocalColorSelectedChanged();
            ddSize.SelectedValue = ds.Tables[0].Rows[0]["Sizeid"].ToString();
            ddSizeSelectedChanged();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Sample code does not exists...')", true);
        }
    }
    protected void MasterBuyerCodeReport()
    {       

        string where = "";
        lblErr.Text = "";

        if (ddCategory.SelectedIndex > 0)
        {
            where = where + " and vf.CATEGORY_ID=" + ddCategory.SelectedValue;
        }
        if (ddItemname.SelectedIndex > 0)
        {
            where = where + " and vf.Item_id=" + ddItemname.SelectedValue;
        }
        if (ddLocalQuality.SelectedIndex > 0)
        {
            where = where + " and CQ.QualityId=" + ddLocalQuality.SelectedValue;
        }
        if (ddLocalDesign.SelectedIndex > 0)
        {
            where = where + " and CD.Designid=" + ddLocalDesign.SelectedValue;
        }
        if (ddLocalColor.SelectedIndex > 0)
        {
            where = where + " and CC.colorid=" + ddLocalColor.SelectedValue;
        }
        if (ddShape.SelectedIndex > 0)
        {
            where = where + " and vf.shapeid=" + ddShape.SelectedValue;
        }
        if (ddSize.SelectedIndex > 0)
        {
            where = where + " and CS.sizeid=" + ddSize.SelectedValue;
        }
        //********Proc
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_MasterBuyerCodeReport", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;
        cmd.Parameters.AddWithValue("@CustomerID", ddBuyerCode.SelectedValue);        
        cmd.Parameters.AddWithValue("@where", where);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varcompanyid"]);
       
        ////******
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_MasterBuyerCodeReport", param);
        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("MasterBuyerCode");
            //*************
            int row = 2;
            //int DetailHstart = row;
            //int Defectlabelrow = row - 1;
            //int Defectcellstart = 0;
            //int Firstdefectcell = 0;
            //int lastdefectcell = 0;
            //int Failedpcscell = 0;
            //int Remarkcell = 0;

            sht.Column("A").Width = 13.22;
            sht.Column("B").Width = 15.33;
            sht.Column("C").Width = 15.89;
            sht.Column("D").Width = 14.89;
            sht.Column("E").Width = 15.89;
            sht.Column("F").Width = 15.89;
            sht.Column("G").Width = 15.89;
            sht.Column("H").Width = 15.89;
            sht.Column("I").Width = 15.89;
            sht.Column("J").Width = 15.89;
            sht.Column("K").Width = 15.89;
            sht.Column("L").Width = 15.89;
            sht.Column("M").Width = 15.89;
            sht.Column("N").Width = 15.89;
            sht.Column("O").Width = 15.89;
            sht.Column("P").Width = 15.89;
            sht.Column("Q").Width = 15.89;
           

            sht.Range("A1:Q1").Merge();
            sht.Range("A1").SetValue("Master Buyer Code Report");
            sht.Range("A1:Q1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:Q1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:Q1").Style.Font.SetBold();
            sht.Range("A1:Q1").Style.Font.FontSize = 12;

            
            sht.Range("A" + row).Value = "Buyer Code";
            sht.Range("B" + row).Value = "Category";
            sht.Range("C" + row).Value = "Item Name";
            sht.Range("D" + row).Value = "Quality";
            sht.Range("E" + row).Value = "Design";
            sht.Range("F" + row).Value = "Color";
            sht.Range("G" + row).Value = "Buyer Quality";
            sht.Range("H" + row).Value = "Buyer Quality Status";
            sht.Range("I" + row).Value = "Buyer Design";
            sht.Range("J" + row).Value = "Buyer Design Status";
            sht.Range("K" + row).Value = "Buyer Color";
            sht.Range("L" + row).Value = "Buyer Color Status";
            sht.Range("M" + row).Value = "Shape";
            sht.Range("N" + row).Value = "Size";
            sht.Range("O" + row).Value = "Buyer Ft Size";
            sht.Range("P" + row).Value = "Buyer Mtr Size";
            sht.Range("Q" + row).Value = "Buyer Inch Size";

            sht.Range("C" + row).Style.Alignment.SetWrapText();
            sht.Range("D" + row).Style.Alignment.SetWrapText();
            sht.Range("E" + row).Style.Alignment.SetWrapText();
            sht.Range("F" + row).Style.Alignment.SetWrapText();
            sht.Range("G" + row).Style.Alignment.SetWrapText();
            sht.Range("I" + row).Style.Alignment.SetWrapText();
            sht.Range("K" + row).Style.Alignment.SetWrapText();

            sht.Range("A" + row + ":Q" + row).Style.Font.FontName = "Calibri";
            sht.Range("A" + row + ":Q" + row).Style.Font.FontSize = 11;
            sht.Range("A" + row + ":Q" + row).Style.Font.SetBold();

             using (var a = sht.Range("A" + row + ":Q" + row))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            row = row + 1;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                //sht.Range("A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Range("A" + row).SetValue(i + 1);

                 sht.Range("A" + row + ":Q" + row).Style.Font.FontName = "Calibri";
                sht.Range("A" + row + ":Q" + row).Style.Font.FontSize = 10;
                sht.Range("A" + row + ":Q" + row).Style.Font.SetBold();
                sht.Range("A" + row + ":Q" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Category_Name"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["BuyerQualityName"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["BuyerQualityStatus"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["BuyerDesignName"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["BuyerDesignStatus"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["BuyerColorName"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["BuyerColorStatus"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["ShapeName"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["SizeFt"]);
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["BuyerFtSize"]);
                sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["BuyerMtrSize"]);
                sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["BuyerInchSize"]);

                using (var a = sht.Range("A" + row + ":Q" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row += 1;
            }          
          
            //*********Borders

            string Fileextension = "xlsx";
            string filename = "MasterBuyerCode_" + UtilityModule.validateFilename(ds.Tables[0].Rows[0]["CustomerCode"].ToString())+ "." + Fileextension + "";
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();

            //Download File
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            Response.End();
          
            //*****
            //File.Delete(Path);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void MasterBuyerCodeReport_agni()
    {

        string where = "";
        lblErr.Text = "";

        if (ddCategory.SelectedIndex > 0)
        {
            where = where + " and vf.CATEGORY_ID=" + ddCategory.SelectedValue;
        }
        if (ddItemname.SelectedIndex > 0)
        {
            where = where + " and vf.Item_id=" + ddItemname.SelectedValue;
        }
        if (ddLocalQuality.SelectedIndex > 0)
        {
            where = where + " and CQ.QualityId=" + ddLocalQuality.SelectedValue;
        }
        if (ddLocalDesign.SelectedIndex > 0)
        {
            where = where + " and CD.Designid=" + ddLocalDesign.SelectedValue;
        }
        if (ddLocalColor.SelectedIndex > 0)
        {
            where = where + " and CC.colorid=" + ddLocalColor.SelectedValue;
        }
        if (ddShape.SelectedIndex > 0)
        {
            where = where + " and vf.shapeid=" + ddShape.SelectedValue;
        }
        if (ddSize.SelectedIndex > 0)
        {
            where = where + " and CS.sizeid=" + ddSize.SelectedValue;
        }
        //********Proc
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_MasterBuyerCodeReport", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;
        cmd.Parameters.AddWithValue("@CustomerID", ddBuyerCode.SelectedValue);
        cmd.Parameters.AddWithValue("@where", where);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varcompanyid"]);

        ////******
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_MasterBuyerCodeReport", param);
        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("MasterBuyerCode");
            //*************
            int row = 5;
            //int DetailHstart = row;
            //int Defectlabelrow = row - 1;
            //int Defectcellstart = 0;
            //int Firstdefectcell = 0;
            //int lastdefectcell = 0;
            //int Failedpcscell = 0;
            //int Remarkcell = 0;

            sht.Column("A").Width = 13.22;
            sht.Column("B").Width = 15.33;
            sht.Column("C").Width = 15.89;
            sht.Column("D").Width = 14.89;
            sht.Column("E").Width = 15.89;
            sht.Column("F").Width = 15.89;
            sht.Column("G").Width = 15.89;
            sht.Column("H").Width = 15.89;
            sht.Column("I").Width = 15.89;
            sht.Column("J").Width = 15.89;
            sht.Column("K").Width = 15.89;
            sht.Column("L").Width = 15.89;
            sht.Column("M").Width = 15.89;
            sht.Column("N").Width = 15.89;
            sht.Column("O").Width = 15.89;
            sht.Column("P").Width = 15.89;
            sht.Column("Q").Width = 15.89;

            sht.Range("A1:Q1").Value = ds.Tables[0].Rows[0]["CompanyName"].ToString();
            sht.Range("A1:Q1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:Q1").Style.Font.FontSize = 13;
            sht.Range("A1:Q1").Style.Font.Bold = true;
            sht.Range("A1:Q1").Merge();
            sht.Range("A2:Q2").Value = ds.Tables[0].Rows[0]["COMPADDR1"].ToString() + "    " + "Print Date-" + DateTime.Now.ToShortDateString();
            sht.Range("A2:Q2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:Q2").Style.Font.FontSize = 13;
            sht.Range("A2:Q2").Style.Font.Bold = true;
            sht.Range("A2:Q2").Merge();
            sht.Range("A3:Q3").Value = "GSTNo.-" + ds.Tables[0].Rows[0]["GSTNO"].ToString();
            sht.Range("A3:Q3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A3:Q3").Style.Font.FontSize = 12;
            sht.Range("A3:Q3").Style.Font.Bold = true;
            sht.Range("A3:Q3").Merge();       

            sht.Range("A4:Q4").Merge();
            sht.Range("A4").SetValue("Master Buyer Code Report");
            sht.Range("A4:Q4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A4:Q4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A4:Q4").Style.Font.SetBold();
            sht.Range("A4:Q4").Style.Font.FontSize = 12;


            sht.Range("A" + row).Value = "Buyer Code";
            sht.Range("B" + row).Value = "Category";
            sht.Range("C" + row).Value = "Item Name";
            sht.Range("D" + row).Value = "Quality";
            sht.Range("E" + row).Value = "Design";
            sht.Range("F" + row).Value = "Color";
            sht.Range("G" + row).Value = "Buyer Quality";
            sht.Range("H" + row).Value = "Buyer Quality Status";
            sht.Range("I" + row).Value = "Buyer Design";
            sht.Range("J" + row).Value = "Buyer Design Status";
            sht.Range("K" + row).Value = "Buyer Color";
            sht.Range("L" + row).Value = "Buyer Color Status";
            sht.Range("M" + row).Value = "Shape";
            sht.Range("N" + row).Value = "Size";
            sht.Range("O" + row).Value = "Buyer Ft Size";
            sht.Range("P" + row).Value = "Buyer Mtr Size";
            sht.Range("Q" + row).Value = "Buyer Inch Size";

            sht.Range("C" + row).Style.Alignment.SetWrapText();
            sht.Range("D" + row).Style.Alignment.SetWrapText();
            sht.Range("E" + row).Style.Alignment.SetWrapText();
            sht.Range("F" + row).Style.Alignment.SetWrapText();
            sht.Range("G" + row).Style.Alignment.SetWrapText();
            sht.Range("I" + row).Style.Alignment.SetWrapText();
            sht.Range("K" + row).Style.Alignment.SetWrapText();

            sht.Range("A" + row + ":Q" + row).Style.Font.FontName = "Calibri";
            sht.Range("A" + row + ":Q" + row).Style.Font.FontSize = 11;
            sht.Range("A" + row + ":Q" + row).Style.Font.SetBold();

            using (var a = sht.Range("A" + row + ":Q" + row))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            row = row + 1;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                //sht.Range("A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Range("A" + row).SetValue(i + 1);

                sht.Range("A" + row + ":Q" + row).Style.Font.FontName = "Calibri";
                sht.Range("A" + row + ":Q" + row).Style.Font.FontSize = 10;
                sht.Range("A" + row + ":Q" + row).Style.Font.SetBold();
                sht.Range("A" + row + ":Q" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Category_Name"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["BuyerQualityName"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["BuyerQualityStatus"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["BuyerDesignName"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["BuyerDesignStatus"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["BuyerColorName"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["BuyerColorStatus"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["ShapeName"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["SizeFt"]);
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["BuyerFtSize"]);
                sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["BuyerMtrSize"]);
                sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["BuyerInchSize"]);

                using (var a = sht.Range("A" + row + ":Q" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row += 1;
            }

            //*********Borders

            string Fileextension = "xlsx";
            string filename = "MasterBuyerCode_" + UtilityModule.validateFilename(ds.Tables[0].Rows[0]["CustomerCode"].ToString()) + "." + Fileextension + "";
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();

            //Download File
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            Response.End();

            //*****
            //File.Delete(Path);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        if (Session["VarCompanyId"].ToString() == "44")
        { MasterBuyerCodeReport_agni(); }
        else
        { MasterBuyerCodeReport(); }
        
    }
}