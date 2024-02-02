using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_FrmUPCNO : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            UtilityModule.ConditionalComboFill(ref ddCustomerCode, @"SELECT CUSTOMERID,CUSTOMERCODE+' / '+COMPANYNAME CODE from CUSTOMERINFO Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
            if (ddCustomerCode.Items.Count > 0)
            {
                ddCustomerCode.SelectedIndex = 1;
            }
            UtilityModule.ConditionalComboFill(ref ddcategory, @"Select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
            lablechange();
            ddcategory.Focus();
        }
    }
    public void lablechange()
    {
      
        try
        {
            
            String[] ParameterList = new String[8];
            ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            lblqualityname.Text = ParameterList[0];
            lbldesignname.Text = ParameterList[1];
            lblcolorname.Text = ParameterList[2];
            lblshapename.Text = ParameterList[3];
            lblcategoryname.Text = ParameterList[5];
            lblitemname.Text = ParameterList[6];

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmUPCNO.aspx");
            // Lblerrer.Visible = true;
            //  Lblerrer.Text = "Data base errer..................";
        }      

    }
    protected void ddcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        Gvdefineitem.Visible = true;
        ddlcategorycange();
        dditemname.Focus();
    }
    private void ddlcategorycange()
    {
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        Shd.Visible = false;
        UtilityModule.ConditionalComboFill(ref dditemname, @"Select Distinct Item_Id,Item_Name from Item_Master where Category_Id=" + ddcategory.SelectedValue, true, "--Select--");
        string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                      " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                      " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddcategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
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
                        //UtilityModule.ConditionalComboFill(ref dddesign, "select distinct Designid,DesignName from Design Order  by DesignName", true, "--Select--");
                        break;
                    case "3":
                        clr.Visible = true;
                        //UtilityModule.ConditionalComboFill(ref ddcolor, "SELECT ColorId,ColorName FROM Color", true, "--Select--");                        
                        break;
                    case "4":
                        shp.Visible = true;
                        //UtilityModule.ConditionalComboFill(ref ddshape, "select Shapeid,ShapeName from Shape Order by Shapeid",true,"--Select--");
                        break;
                    case "5":
                        sz.Visible = true;
                        break;
                    case "6":
                        Shd.Visible = true;
                        break;
                }
            }
        }
    }

    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        //UtilityModule.ConditionalComboFill(ref dquality, "select qualityid,qualityname from quality where item_id=" + dditemname.SelectedValue, true, "--Select--");
        QDCSDDFill(dquality, dddesign, ddcolor, ddshape, ddShade, Convert.ToInt32(dditemname.SelectedValue));
        Fill_Grid();
        dquality.Focus();
        btnsave.Text = "Save";
    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dditemname.SelectedIndex > 0 && dquality.SelectedIndex > 0)
        {
            Fill_Grid();
            dddesign.Focus();
        }
    }
    protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dditemname.SelectedIndex > 0 && dddesign.SelectedIndex > 0)
        {
            Fill_Grid();
            ddcolor.Focus();
        }
    }
    protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dditemname.SelectedIndex > 0 && ddcolor.SelectedIndex > 0)
        {
            Fill_Grid();
            ddshape.Focus();
        }
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dditemname.SelectedIndex > 0 && ddshape.SelectedIndex > 0)
        {
            FillSize();
            Fill_Grid();
            chkbox.Focus();
        }
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dditemname.SelectedIndex > 0 && ddsize.SelectedIndex > 0)
        {
            Fill_Grid();
            txtUPCNO.Focus();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        CHECKVALIDCONTROL();
        try
        {
            if (lblerror.Text == "")
            {
                SqlParameter[] _arrPara1 = new SqlParameter[6];
                _arrPara1[0] = new SqlParameter("@ID", SqlDbType.Int);
                _arrPara1[1] = new SqlParameter("@Customerid", SqlDbType.Int);
                _arrPara1[2] = new SqlParameter("@Finishedid", SqlDbType.Int);
                _arrPara1[3] = new SqlParameter("@UPCNO", SqlDbType.Int);
                _arrPara1[4] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara1[5] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                _arrPara1[0].Value = 0;
                _arrPara1[1].Value = ddCustomerCode.SelectedValue;
                int ItemFinishedId = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddShade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                _arrPara1[2].Value = ItemFinishedId;
                _arrPara1[3].Value = txtUPCNO.Text;
                _arrPara1[4].Value = Session["varuserid"].ToString();
                _arrPara1[5].Value = Session["varMasterCompanyIDForERP"].ToString();
                if (btnsave.Text == "Update")
                {
                    _arrPara1[0].Value = 1;
                }
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPCNO", _arrPara1);
                btnsave.Text = "Save";
                Tran.Commit();
                lblerror.Text = "SAVED SUCCCESSFULLY...";
                txtUPCNO.Text = "";
                dditemname.Focus();
                Fill_Grid();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmUPCNO.aspx");
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void CHECKVALIDCONTROL()
    {
        lblerror.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(ddCustomerCode) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddcategory) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(dditemname) == false)
        {
            goto a;

        }
        if (dquality.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(dquality) == false)
            {
                goto a;
            }
        }
        if (dddesign.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(dddesign) == false)
            {
                goto a;
            }
        }
        if (ddcolor.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddcolor) == false)
            {
                goto a;
            }
        }
        if (ddshape.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddshape) == false)
            {
                goto a;
            }
        }
        if (ddsize.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddsize) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDTEXTBOX(txtUPCNO) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(lblerror);
    B: ;
    }

    private void Fill_Grid()
    {
        Gvdefineitem.DataSource = Fill_Grid_Data();
        Gvdefineitem.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            int A = 2;
            if (chkbox.Checked == true)
            {
                A = 1;
            }
            string strsql = @"Select Item_Finished_id as Sr_No,IsNull(QualityName,'') as " + lblqualityname.Text + ",IsNull(DesignName,'')as " + lbldesignname.Text + ",IsNull(ColorName,'') as " + lblcolorname.Text + " ,IsNull(ShapeName,'') as " + lblshapename.Text + ",IsNull(ShadeColorName,'') as " + lblshadename.Text + ",Case When UnitId=" + A +
                @" then IsNull(SizeFt,'') else IsNull(SizeMtr,'') End Size,UP.UPCNO From UPCNO UP,Item_Parameter_Master IPM Left Outer Join Quality Q ON 
                IPM.Quality_id=Q.Qualityid Left Outer Join Design D ON IPM.Design_id=D.Designid Left Outer Join Color C ON IPM.Color_id=C.Colorid Left Outer Join Shape Sh 
                ON IPM.Shape_id=Sh.Shapeid Left Outer Join Size S ON IPM.Size_id=S.Sizeid Left Outer Join ShadeColor Sd ON IPM.SHADECOLOR_ID=Sd.ShadecolorId 
                WHERE UP.Finishedid=IPM.Item_Finished_id AND UP.CUSTOMERID=" + ddCustomerCode.SelectedValue + " AND IPM.ITEM_ID=" + dditemname.SelectedValue + " And IPM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            if (dquality.Visible == true && dquality.SelectedIndex > 0)
            {
                strsql = strsql + @" And IPM.QUALITY_ID=" + dquality.SelectedValue + "";
            }
            if (dddesign.Visible == true && dddesign.SelectedIndex > 0)
            {
                strsql = strsql + @" And IPM.DESIGN_ID=" + dddesign.SelectedValue + "";
            }
            if (ddcolor.Visible == true && ddcolor.SelectedIndex > 0)
            {
                strsql = strsql + @" And IPM.COLOR_ID=" + ddcolor.SelectedValue + "";
            }
            if (ddshape.Visible == true && ddshape.SelectedIndex > 0)
            {
                strsql = strsql + @" And IPM.SHAPE_ID=" + ddshape.SelectedValue + "";
            }
            if (ddsize.Visible == true && ddsize.SelectedIndex > 0)
            {
                strsql = strsql + @" And IPM.SIZE_ID=" + ddsize.SelectedValue + "";
            }
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmUPCNO.aspx");
            Logs.WriteErrorLog("DefineItemCode|Fill_Grid_Data|" + ex.Message);
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
    protected void Gvdefineitem_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = null;
        string id = Gvdefineitem.SelectedDataKey.Value.ToString();
        Session["id"] = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "SELECT IPM.*,IM.CATEGORY_ID,UP.CUSTOMERID,UP.UPCNO FROM UPCNO UP INNER JOIN ITEM_PARAMETER_MASTER IPM ON UP.FINISHEDID=IPM.ITEM_FINISHED_ID INNER JOIN ITEM_MASTER IM ON IPM.ITEM_ID=IM.ITEM_ID WHERE ITEM_FINISHED_ID=" + id + " And IPM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                ddCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["CUSTOMERID"].ToString();
                ddcategory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                ddlcategorycange();
                dditemname.SelectedValue = ds.Tables[0].Rows[0]["item_id"].ToString();
                QDCSDDFill(dquality, dddesign, ddcolor, ddshape, ddShade, Convert.ToInt32(dditemname.SelectedValue));
                dquality.SelectedValue = ds.Tables[0].Rows[0]["quality_id"].ToString();
                dddesign.SelectedValue = ds.Tables[0].Rows[0]["design_id"].ToString();
                ddcolor.SelectedValue = ds.Tables[0].Rows[0]["color_id"].ToString();
                ddshape.SelectedValue = ds.Tables[0].Rows[0]["shape_id"].ToString();
                FillSize();
                ddsize.SelectedValue = ds.Tables[0].Rows[0]["size_id"].ToString();
                txtUPCNO.Text = ds.Tables[0].Rows[0]["UPCNO"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmUPCNO.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        btnsave.Text = "Update";
    }
    protected void Gvdefineitem_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Gvdefineitem.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void Gvdefineitem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Gvdefineitem, "Select$" + e.Row.RowIndex);
        }
    }

    protected void btnnew_Click(object sender, EventArgs e)
    {
        //UtilityModule.ConditionalComboFill(ref ddcategory, @"Select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER", true, "Select Category");
        ddcategory.SelectedIndex = 0;
        ddlcategorycange();
        //dditemname.SelectedIndex = 0;
        Gvdefineitem.Visible = false;
        btnsave.Text = "Save";
    }
    protected void refreshcategory_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddcategory, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by CATEGORY_Id", true, "--SELECT--");
    }
    protected void refreshitem_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dditemname, "Select Distinct Item_Id,Item_Name from Item_Master where Category_Id=" + ddcategory.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select Item--");
    }
    protected void refreshquality_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dquality, "select qualityid,qualityname from quality Where Item_Id=" + dditemname.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By QUALITYID Desc", true, "--Select--");
    }
    protected void refreshdesign_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dddesign, "select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order  by DesignName", true, "--Select--");
    }
    protected void refreshcolor_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddcolor, "SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by colorid", true, "--Select--");
    }
    protected void refreshshape_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddshape, "select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Shapeid", true, "--Select--");
    }
    protected void refreshsize_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid ", true, "--Select--");
    }
    private void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Shade, int Itemid)
    {
        UtilityModule.ConditionalComboFill(ref Quality, "SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + Itemid + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By QUALITYID Desc", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref Design, "SELECT DESIGNID,DESIGNNAME from DESIGN Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref Color, "SELECT COLORID,COLORNAME FROM COLOR Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref Shape, "SELECT SHAPEID,SHAPENAME FROM SHAPE Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref Shade, "SELECT ShadecolorId,ShadeColorName FROM ShadeColor Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--SELECT--");
    }

    protected void refreshshade_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddShade, "SELECT ShadecolorId,ShadeColorName FROM ShadeColor Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--SELECT--");
    }
    protected void chkbox_CheckedChanged(object sender, EventArgs e)
    {
        FillSize();
        Fill_Grid();
    }
    private void FillSize()
    {
        if (chkbox.Checked == false)
        {
            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, " Select Size");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizemtr from size Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, " Select Size");
        }
    }
    
    protected void Gvdefineitem_RowCreated(object sender, GridViewRowEventArgs e)
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