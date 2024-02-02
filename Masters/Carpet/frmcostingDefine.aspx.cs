using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;


public partial class Masters_Carpet_frmcostingDefine : System.Web.UI.Page
{
    Double Total = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str;
            str = @"select ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=Cs.Categoryid and cs.id=0
                   select ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=Cs.Categoryid and cs.id=1
                   select Process_name_id,Process_name from Process_name_master order by Process_Name
                   select val,Type  from SizeType ";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCategoryname, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategoryRD, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcess, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 3, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDSizeTypeRD, ds, 3, false, "");
            ds.Dispose();
            TDCBM.Attributes.Add("style", "display:none;");
            txtcostingDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            //*************Create Directory
            string directorypath = Server.MapPath("~/CostingImage");
            if (!Directory.Exists(directorypath))
            {
                Directory.CreateDirectory(directorypath);
            }
            //*************
        }
    }
    protected void CategoryDependControls(DropDownList ddcategoryname, DropDownList dditemName, HtmlTableCell tdquality = null, HtmlTableCell tddesign = null, HtmlTableCell tdColor = null, HtmlTableCell tdshape = null, HtmlTableCell tdsize = null, HtmlTableCell tdShade = null)
    {
        tdquality.Visible = false;
        tddesign.Visible = false;
        tdColor.Visible = false;
        tdshape.Visible = false;
        tdsize.Visible = false;
        tdShade.Visible = false;
        UtilityModule.ConditionalComboFill(ref dditemName, "select ITEM_ID,ITEM_NAME from ITEM_MASTER where CATEGORY_ID=" + ddcategoryname.SelectedValue + " order by ITEM_NAME", true, "--Select--");
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select PARAMETER_ID from ITEM_CATEGORY_PARAMETERS Where CATEGORY_ID=" + ddcategoryname.SelectedValue + "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (Convert.ToString(dr["PARAMETER_ID"]))
                {
                    case "1":
                        tdquality.Visible = true;
                        break;
                    case "2":
                        tddesign.Visible = true;
                        break;
                    case "3":
                        tdColor.Visible = true;
                        break;
                    case "4":
                        tdshape.Visible = true;
                        break;
                    case "5":
                        tdsize.Visible = true;
                        break;
                    case "6":
                        tdShade.Visible = true;
                        break;
                }
            }

        }

    }
    protected void DDCategoryname_SelectedIndexChanged(object sender, EventArgs e)
    {
        CategoryDependControls(DDCategoryname, DDItemName, TDQuality, TDDesign, TDColor, TDShape, TDSize, TDShade);
    }
    protected void DDCategoryRD_SelectedIndexChanged(object sender, EventArgs e)
    {
        CategoryDependControls(DDCategoryRD, DDItemNameRD, TDQualityRD, TDDesignRD, TDColorRD, TDShapeRD, TDSizeRD, TDShadeRD);
    }
    protected void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Sizetype, int Itemid, System.Web.UI.HtmlControls.HtmlTableCell tdQuality = null, System.Web.UI.HtmlControls.HtmlTableCell tdDesign = null, System.Web.UI.HtmlControls.HtmlTableCell tdcolor = null, System.Web.UI.HtmlControls.HtmlTableCell tdshape = null)
    {
        if (tdQuality.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref Quality, "select QualityId,QualityName from Quality Where Item_Id=" + Itemid + " order by QualityName", true, "--Select--");
        }

        string str;
        str = @"SELECT DESIGNID,DESIGNNAME from DESIGN Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By DESIGNNAME
            SELECT COLORID,COLORNAME FROM COLOR Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By COLORNAME
            SELECT SHAPEID,SHAPENAME FROM SHAPE Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By SHAPENAME";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (tdDesign.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref Design, ds, 0, true, "--Select--");
        }
        if (tdcolor.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref Color, ds, 1, true, "--Select--");
        }
        if (tdshape.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref Shape, ds, 2, true, "--Select--");
            UtilityModule.ConditionalComboFill(ref Sizetype, "select val,type from sizetype", false, "");
        }
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(DDQuality, DDDesign, DDColor, DDshape, DDsizetype, Convert.ToInt16(DDItemName.SelectedValue), TDQuality, TDDesign, TDColor, TDShape);
    }
    protected void DDItemNameRD_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(DDQualityRD, DDDesignRD, DDColorRD, DDShapeRD, DDSizeTypeRD, Convert.ToInt16(DDItemNameRD.SelectedValue), TDQualityRD, TDDesignRD, TDColorRD, TDShapeRD);
    }
    protected void FillSize(DropDownList SizeType, DropDownList Shape, DropDownList Size)
    {
        string size = "";
        string str = "";

        switch (SizeType.SelectedValue)
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

        str = "Select Distinct S.Sizeid,S." + size + " As  " + size + @" From Size S 
                 Where shapeid=" + Shape.SelectedValue + " And S.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by " + size + "";

        UtilityModule.ConditionalComboFill(ref Size, str, true, "--Select--");
    }
    protected void DDshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize(DDsizetype, DDshape, DDSize);
    }
    protected void DDShapeRD_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize(DDSizeTypeRD, DDShapeRD, DDSizeRD);
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize(DDsizetype, DDshape, DDSize);
    }
    protected void DDSizeTypeRD_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize(DDSizeTypeRD, DDShapeRD, DDSizeRD);
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[19];
            param[0] = new SqlParameter("@CostingId", SqlDbType.Int);
            param[1] = new SqlParameter("@Item_Finished_id", SqlDbType.Int);
            param[2] = new SqlParameter("@SampleCode", SqlDbType.VarChar, 50);
            param[3] = new SqlParameter("@CostingType", SqlDbType.TinyInt);//--0 For Sample 1 For customer
            param[4] = new SqlParameter("@Item_Finished_id_Detail", SqlDbType.Int);
            param[5] = new SqlParameter("@RateDetail", SqlDbType.Float);
            param[6] = new SqlParameter("@QtyDetail", SqlDbType.Float);
            param[7] = new SqlParameter("@AmountDetail", SqlDbType.Float);
            param[8] = new SqlParameter("@Processid", SqlDbType.Int);
            param[9] = new SqlParameter("@RateProcess", SqlDbType.Float);
            param[10] = new SqlParameter("@QtyProcess", SqlDbType.Float);
            param[11] = new SqlParameter("@AmountProcess", SqlDbType.Float);
            param[12] = new SqlParameter("@Userid", SqlDbType.Int);
            param[13] = new SqlParameter("@mastercompanyId", SqlDbType.Int);
            param[14] = new SqlParameter("@msg", SqlDbType.VarChar, 500);
            param[15] = new SqlParameter("@flagsize", SqlDbType.TinyInt);
            param[16] = new SqlParameter("@flagsizeItemDetail", SqlDbType.TinyInt);
            param[17] = new SqlParameter("@CostingDefineDate", SqlDbType.DateTime);
            param[18] = new SqlParameter("@ProcessRemark", SqlDbType.VarChar, 1000);
            //*******************
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = ViewState["Id"];
            int item_finished_id = UtilityModule.getItemFinishedId(DDItemName, DDQuality, DDDesign, DDColor, DDshape, DDSize, txtprodcode, Tran, DDshade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"])); ;
            param[1].Value = item_finished_id;
            param[2].Value = txtsamplecode.Text;
            param[3].Value = DDCostingFor.SelectedValue;
            int item_finished_id_Detail = 0;
            if (DDCategoryRD.SelectedIndex > 0 && DDItemNameRD.SelectedIndex > 0)
            {
                item_finished_id_Detail = UtilityModule.getItemFinishedId(DDItemNameRD, DDQualityRD, DDDesignRD, DDColorRD, DDShapeRD, DDSizeRD, txtItemCodeRD, Tran, DDShadeRD, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"])); ;
            }
            param[4].Value = item_finished_id_Detail;
            param[5].Value = txtRate.Text == "" ? "0" : txtRate.Text;
            param[6].Value = txtQty.Text == "" ? "0" : txtQty.Text;
            param[7].Value = txtAmount.Text == "" ? "0" : txtAmount.Text;
            param[8].Value = DDProcess.SelectedValue;
            param[9].Value = txtProcessRate.Text == "" ? "0" : txtProcessRate.Text;
            param[10].Value = txtProcessQty.Text == "" ? "0" : txtProcessQty.Text;
            param[11].Value = txtProcessAmount.Text == "" ? "0" : txtProcessAmount.Text;
            param[12].Value = Session["varuserid"];
            param[13].Value = Session["varMasterCompanyIDForERP"];
            param[14].Direction = ParameterDirection.Output;
            param[15].Value = DDsizetype.SelectedValue;
            param[16].Value = DDSizeTypeRD.SelectedValue;
            param[17].Value = txtcostingDate.Text;
            param[18].Value = txtprocessremark.Text;
            //**********************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_costingSave", param);
            ViewState["Id"] = param[0].Value;
            //**************Message
            if (param[14].Value.ToString() != "")
            {
                lblmessage.Text = param[14].Value.ToString();
            }
            else
            {
                lblmessage.Text = "Data saved successfully...";
            }

            Tran.Commit();
            FillGrid();
            Refreshcontrol();
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void Refreshcontrol()
    {
        //**
        DDCategoryRD.SelectedIndex = -1;
        DDItemNameRD.SelectedIndex = -1;
        DDQualityRD.SelectedIndex = -1;
        DDDesignRD.SelectedIndex = -1;
        DDColorRD.SelectedIndex = -1;
        DDShapeRD.SelectedIndex = -1;
        DDSizeRD.SelectedIndex = -1;
        DDShadeRD.SelectedIndex = -1;
        txtQty.Text = "";
        txtRate.Text = "";
        txtAmount.Text = "";

        //**
        DDProcess.SelectedIndex = -1;
        txtProcessRate.Text = "";
        txtProcessQty.Text = "";
        txtProcessAmount.Text = "";
        txtprocessremark.Text = "";
    }
    protected void FillGrid()
    {
        string str;
        str = @"select CM.id as costingid,Vf.CATEGORY_NAME+' '+vf.ITEM_NAME+'  '+vf.Qualityname+' '+vf.designName+' '+vf.ColorName+' '+vf.ShapeName+' ' +
                (case when CM.flagsize=0 Then Vf.SizeFt  When CM.flagsize=1 Then vf.SizeMtr 
                when cm.flagsize=2 then vf.SizeInch else '' End)+' '+case When vf.sizeid>0 Then sz.type else '' End as ItemDescription,
                CM.Samplecode,case When CM.CostingType=0 then 'Sample' Else 'Customer' End as CostingType,
                cd.Details,cd.rate,cd.Qty,cd.Amount,isnull(cd.DetailType,0) as DetailType,isnull(Cd.FinishedId_ProcessId,0) as FinishedId_ProcessId
                from costingmaster CM inner join V_FinishedItemDetail vf  on 
                CM.Item_Finished_id=vf.ITEM_FINISHED_ID
                inner join SizeType SZ on cm.flagsize=Sz.val
                left join V_costingDetail CD on CM.ID=cd.CostingMID where 1=1";
        if (DDCategoryname.SelectedIndex > 0)
        {
            str = str + " and vf.CATEGORY_ID=" + DDCategoryname.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + " and vf.Item_ID=" + DDItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.Qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and vf.ColorId=" + DDColor.SelectedValue;
        }
        if (DDshape.SelectedIndex > 0)
        {
            str = str + " and vf.shapeid=" + DDshape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and vf.SizeId=" + DDSize.SelectedValue;
        }
        if (txtsamplecode.Text != "")
        {
            str = str + " and CM.samplecode='" + txtsamplecode.Text + "'";
        }
        if (DDCostingFor.SelectedIndex != -1)
        {
            str = str + " and CM.CostingType=" + DDCostingFor.SelectedValue;
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GVDetail.DataSource = ds.Tables[0];
        GVDetail.DataBind();
    }
    protected void btnshowdetail_Click(object sender, EventArgs e)
    {
        //****Fill Costing Item Description
        Fillsampledetail();
        //**********
        FillGrid();
    }
    protected void Fillsampledetail()
    {
        string str = @"select VF.CATEGORY_ID,vf.ITEM_ID,vf.QualityId,vf.designId,vf.ColorId,vf.ShapeId,vf.SizeId,cm.flagsize,vf.ShadecolorId
                    From Costingmaster CM inner join V_FinishedItemDetail vf on CM.Item_Finished_id=vf.ITEM_FINISHED_ID where cm.samplecode='" + txtsamplecode.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DDCategoryname.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["Category_id"]);
            CategoryDependControls(DDCategoryname, DDItemName, TDQuality, TDDesign, TDColor, TDShape, TDSize, TDShade);
            DDItemName.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["Item_id"]);
            QDCSDDFill(DDQuality, DDDesign, DDColor, DDshape, DDsizetype, Convert.ToInt16(DDItemName.SelectedValue), TDQuality, TDDesign, TDColor, TDShape);
            if (TDQuality.Visible == true)
            {
                DDQuality.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["Qualityid"]);
            }
            if (TDDesign.Visible == true)
            {
                DDDesign.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["Designid"]);
            }
            if (TDColor.Visible == true)
            {
                DDColor.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["Colorid"]);
            }
            if (TDShape.Visible == true)
            {
                DDshape.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["Shapeid"]);
                DDsizetype.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["flagsize"]);
                FillSize(DDsizetype, DDshape, DDSize);
            }
            if (TDSize.Visible == true)
            {
                DDSize.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["Sizeid"]);
            }
            if (TDShade.Visible == true)
            {
                DDshade.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["shadecolorid"]);
            }
        }

    }
    protected void GVDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@Costingid", SqlDbType.Int);
            param[1] = new SqlParameter("@DetailType", SqlDbType.TinyInt);
            param[2] = new SqlParameter("@Finishedid_ProcessId", SqlDbType.Int);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4] = new SqlParameter("@Imgdelflag", SqlDbType.TinyInt);//0 for No 1 for yes
            //*************
            Label lblcostingid = ((Label)GVDetail.Rows[e.RowIndex].FindControl("lblcostingid"));
            Label lblDetailType = ((Label)GVDetail.Rows[e.RowIndex].FindControl("lblDetailType"));
            Label lblFinishedId_ProcessId = ((Label)GVDetail.Rows[e.RowIndex].FindControl("lblFinishedId_ProcessId"));
            param[0].Value = lblcostingid.Text;
            param[1].Value = lblDetailType.Text;
            param[2].Value = lblFinishedId_ProcessId.Text;
            param[3].Direction = ParameterDirection.Output;
            param[4].Direction = ParameterDirection.Output;
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_deletecosting", param);
            lblmessage.Text = param[3].Value.ToString();
            Tran.Commit();
            //************imgdele
            if (param[4].Value.ToString() == "1")
            {
                if (File.Exists(Server.MapPath("~/CostingImage/" + lblcostingid.Text)))
                {
                    File.Delete(Server.MapPath("~/CostingImage/" + lblcostingid.Text));
                }
            }
            //**************

            FillGrid();
        }
        catch (Exception ex)
        {

            Tran.Rollback();
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void GVDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblAmount = ((Label)e.Row.FindControl("lblAmount"));
            Double amount = Double.Parse(lblAmount.Text == "" ? "0" : lblAmount.Text);
            Total = Total + amount;
            Label lblDetailType = (Label)e.Row.FindControl("lblDetailType");
            if (lblDetailType.Text=="2")
            {
                LinkButton lnkDel = (LinkButton)e.Row.FindControl("lnkDel");
                lnkDel.Visible = false;
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblTotalqty = (Label)e.Row.FindControl("lblTotalqty");
            lblTotalqty.Text = Total.ToString();
        }
    }
    protected void GVDetail_DataBound(object sender, EventArgs e)
    {
        string oldvalue = string.Empty;
        string Newvalue = string.Empty;
        //*****Column Loop
        for (int j = 2; j < 4; j++)
        {
            for (int count = 0; count < GVDetail.Rows.Count; count++)
            {
                oldvalue = GVDetail.Rows[count].Cells[j].Text;
                if (oldvalue == Newvalue)
                {
                    GVDetail.Rows[count].Cells[j].Text = string.Empty;
                    if (j == 2)
                    {
                        LinkButton lnkbutton = ((LinkButton)GVDetail.Rows[count].FindControl("lnkAddImage"));
                        //
                        LinkButton lnkbuttonFiller = ((LinkButton)GVDetail.Rows[count].FindControl("lnlFillershell"));
                        lnkbutton.Visible = false;
                        lnkbuttonFiller.Visible = false;
                    }
                }
                Newvalue = oldvalue;
            }
        }
    }

    protected void GVDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddImage")
        {
            GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int rowindex = row.RowIndex;
            Label lblcostingid = ((Label)GVDetail.Rows[rowindex].FindControl("lblcostingid"));
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../Carpet/AddPhotoRefImage1.aspx?SrNo=" + lblcostingid.Text + "&img=COSTING&PPI=yes', 'nwwin', 'toolbar=0, titlebar=1,  top=200px, left=100px, scrollbars=1, resizable = yes,width=550px,Height=200px');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else if (e.CommandName == "Filler")
        {
            GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int rowindex = row.RowIndex;
            Label lblcostingid = ((Label)GVDetail.Rows[rowindex].FindControl("lblcostingid"));
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../Carpet/frmFiller_Shell.aspx?id=" + lblcostingid.Text + "&PP=yes', 'nwwin', 'toolbar=0, titlebar=1,  top=100px, left=100px, scrollbars=1, resizable = yes,width=350px,Height=400px');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "filler", stb.ToString(), false);
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string where = " Where 1=1";
        if (DDCategoryname.SelectedIndex > 0)
        {
            where = where + " and vf.CATEGORY_ID=" + DDCategoryname.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            where = where + " and vf.Item_ID=" + DDItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and vf.Qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and vf.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and vf.ColorId=" + DDColor.SelectedValue;
        }
        if (DDshape.SelectedIndex > 0)
        {
            where = where + " and vf.shapeid=" + DDshape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and vf.SizeId=" + DDSize.SelectedValue;
        }

        if (DDCostingFor.SelectedIndex != -1)
        {
            where = where + " and CM.CostingType=" + DDCostingFor.SelectedValue;
        }
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@Where", SqlDbType.VarChar, 1000);
        param[1] = new SqlParameter("@Samplecode", SqlDbType.VarChar, 50);
        //*************
        param[0].Value = where;
        param[1].Value = txtsamplecode.Text;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_costingReport", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            //*******Image
            ds.Tables[0].Columns.Add("Image", typeof(System.Byte[]));


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string ImgName, Imgext, Filepath, img;
                if (dr["ImageName"] != "")
                {
                    ImgName = dr["ImageName"].ToString();
                    Imgext = ImgName.Substring(ImgName.LastIndexOf("."));
                    Filepath = "~\\CostingImage\\" + dr["Costingid"] + Imgext;
                    FileInfo file = new FileInfo(Server.MapPath(Filepath));
                    if (file.Exists)
                    {
                        img = Server.MapPath(Filepath);
                        Byte[] img_Byte = File.ReadAllBytes(img);
                        dr["Image"] = img_Byte;
                    }

                }
            }
            //**********
            Session["dsFileName"] = "~\\ReportSchema\\RptcostingDetail.xsd";
            Session["ReportPath"] = "Reports/RptCostingReport.rpt";

            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }



    }
  
}