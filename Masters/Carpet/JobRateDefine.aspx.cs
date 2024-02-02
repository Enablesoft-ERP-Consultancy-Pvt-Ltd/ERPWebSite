using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Carpet_JobRateDefine : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
            lablechange();
            TxtId.Text = "0";
            UtilityModule.ConditionalComboFill(ref DDCompany, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName", true, "--Select--");
            UtilityModule.ConditionalComboFill(ref DDProcess, "Select PROCESS_NAME_ID,PROCESS_NAME From PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
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
            lblsizename.Text = ParameterList[4];
            lblcategoryname.Text = ParameterList[5];
            lblitemname.Text = ParameterList[6];
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        validate();
        if (LblError.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrpara = new SqlParameter[18];
                _arrpara[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@ProcessTypeId", SqlDbType.Int);
                _arrpara[3] = new SqlParameter("@EmpId", SqlDbType.Int);
                _arrpara[4] = new SqlParameter("@CategoryId", SqlDbType.Int);
                _arrpara[5] = new SqlParameter("@ItemId", SqlDbType.Int);
                _arrpara[6] = new SqlParameter("@UnitId", SqlDbType.Int);
                _arrpara[7] = new SqlParameter("@QualityId", SqlDbType.Int);
                _arrpara[8] = new SqlParameter("@DesignId", SqlDbType.Int);
                _arrpara[9] = new SqlParameter("@ColorId", SqlDbType.Int);
                _arrpara[10] = new SqlParameter("@ShapeId", SqlDbType.Int);
                _arrpara[11] = new SqlParameter("@SizeId", SqlDbType.Int);
                _arrpara[12] = new SqlParameter("@Rate", SqlDbType.Float);
                _arrpara[13] = new SqlParameter("@JobeRateId", SqlDbType.Int);
                _arrpara[14] = new SqlParameter("@Chk", SqlDbType.Int);
                _arrpara[15] = new SqlParameter("@SizeUnit", SqlDbType.Int);
                _arrpara[16] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrpara[17] = new SqlParameter("@varCompanyId", SqlDbType.Int);

                _arrpara[0].Value = DDCompany.SelectedValue;
                _arrpara[1].Value = DDProcess.SelectedValue;
                _arrpara[2].Value = DDProcessType.SelectedValue;
                _arrpara[3].Value = DDEmployeeName.SelectedValue;
                _arrpara[4].Value = DDCategory.SelectedValue;
                _arrpara[5].Value = DDItemName.SelectedValue;
                _arrpara[6].Value = DDUnit.SelectedValue;
                _arrpara[7].Value = DDQuality.SelectedIndex > 0 ? Convert.ToInt32(DDQuality.SelectedValue) : 0;
                if (CHKDesign.Checked == true)
                {
                    _arrpara[8].Value = -1;
                }
                else
                {
                    _arrpara[8].Value = DDDesign.SelectedIndex > 0 ? Convert.ToInt32(DDDesign.SelectedValue) : 0;
                }
                if (CHKColor.Checked == true)
                {
                    _arrpara[9].Value = -1;
                }
                else
                {
                    _arrpara[9].Value = DDColor.SelectedIndex > 0 ? Convert.ToInt32(DDColor.SelectedValue) : 0;
                }
                _arrpara[10].Value = DDShape.SelectedIndex > 0 ? Convert.ToInt32(DDShape.SelectedValue) : 0;
                if (CHKSize.Checked == true)
                {
                    _arrpara[11].Value = -1;
                }
                else
                {
                    _arrpara[11].Value = DDSize.SelectedIndex > 0 ? Convert.ToInt32(DDSize.SelectedValue) : 0;
                }


                _arrpara[12].Value = TxtRate.Text;
                _arrpara[13].Value = TxtId.Text;
                _arrpara[14].Direction = ParameterDirection.Output;
                _arrpara[15].Value = ChMtr.Checked == true ? 1 : 0;
                _arrpara[16].Value = Session["varuserid"].ToString();
                _arrpara[17].Value = Session["varMasterCompanyIDForERP"].ToString();
                // SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_JobeRateValidate", _arrpara);
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_JobRateDefine", _arrpara);

                if (Convert.ToInt32(_arrpara[14].Value) == 1)
                {
                    LblError.Text = "Job Rate Allready Exists...........";
                }
                else
                {
                    LblError.Text = "Data Save successfully...........";
                }
                if (BtnSave.Text == "Update")
                {
                    BtnSave.Text = "Save";
                }
                refresh();
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Carpet/JobRateDefine.aspx");
                Tran.Rollback();
            }
            finally
            {
                Tran.Commit();
                con.Close();
            }
            Fill_Grid();
        }
    }
    protected void DDProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDCompany.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref DDProcessType, "select Distinct ProcessId,ProcessType from ProcessType where ProcessNameid=" + DDProcess.SelectedValue, true, "--Select--");
            LblError.Text = "";
        }
        else
        {
            LblError.Text = "Please select Company first........";
            DDProcess.SelectedIndex = 0;
        }
    }
    protected void DDProcessType_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDEmployeeName, "select Distinct EmpId,EmpName from EmpInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
    }
    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDCategory, "SELECT Distinct CATEGORY_ID,CATEGORY_NAME FROM ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
        Fill_Grid();
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetParameters();

        UtilityModule.ConditionalComboFill(ref DDItemName, "select Item_id, Item_Name from Item_Master where Category_Id=" + DDCategory.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LblError.Text = "";
        UtilityModule.ConditionalComboFill(ref DDQuality, "select Qualityid,Qualityname from Quality where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDDesign, "select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order  by DesignName", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDColor, "SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDShape, "select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by ShapeName", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDUnit, "SELECT DISTINCT U.UNITID,U.UNITNAME FROM UNIT U,UNIT_TYPE_MASTER UT,ITEM_MASTER IM WHERE U.UNITTYPEID=UT.UNITTYPEID AND UT.UNITTYPEID=IM.UNITTYPEID AND ITEM_ID=" + DDItemName.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
    }
    private void visiblePerameter()
    {
        tdQ.Visible = false;
        tdQuality.Visible = false;
        tdD.Visible = false;
        tdDesign.Visible = false;
        tdC.Visible = false;
        tdColor.Visible = false;
        tdSh.Visible = false;
        tdShape.Visible = false;
        tdsz.Visible = false;
        tdSize.Visible = false;
    }
    private void GetParameters()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet DS;
        try
        {
            visiblePerameter();
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select PARAMETER_ID from ITEM_CATEGORY_PARAMETERS where Category_Id=" + DDCategory.SelectedValue);
            if (DS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    switch (dr["PARAMETER_ID"].ToString())
                    {
                        case "1":
                            tdQ.Visible = true;
                            tdQuality.Visible = true;
                            break;
                        case "2":
                            tdD.Visible = true;
                            tdDesign.Visible = true;
                            break;
                        case "3":
                            tdC.Visible = true;
                            tdColor.Visible = true;
                            break;
                        case "4":
                            tdSh.Visible = true;
                            tdShape.Visible = true;
                            break;
                        case "5":
                            tdsz.Visible = true;
                            tdSize.Visible = true;
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/JobRateDefine.aspx");
        }
        finally
        {
            con.Close();
        }
    }

    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillSize();
    }
    protected void ChMtr_CheckedChanged(object sender, EventArgs e)
    {
        fillSize();
    }
    private void fillSize()
    {
        tdsz.Visible = true;
        if (ChMtr.Checked == false)
        {
            UtilityModule.ConditionalComboFill(ref DDSize, "Select SizeId,SizeFt from Size where Shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDSize, "Select SizeId,SizeMtr from Size where Shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
        }
    }

    protected void CHKDesign_CheckedChanged(object sender, EventArgs e)
    {
        if (CHKDesign.Checked == true)
        {
            DDDesign.Enabled = false;
            DDDesign.SelectedIndex = 0;
        }
        else
        {
            DDDesign.Enabled = true;
        }
    }
    protected void CHKColor_CheckedChanged(object sender, EventArgs e)
    {
        if (CHKColor.Checked == true)
        {
            DDColor.Enabled = false;
            DDColor.SelectedIndex = 0;
        }
        else
        {
            DDColor.Enabled = true;
        }
    }
    protected void CHKSize_CheckedChanged(object sender, EventArgs e)
    {
        if (CHKSize.Checked == true)
        {
            ChMtr.Checked = false;
            ChMtr.Enabled = false;
            DDSize.Enabled = false;
            DDSize.SelectedIndex = 0;
        }
        else
        {
            ChMtr.Enabled = true;
            DDSize.Enabled = true;
        }
    }


    private void Fill_Grid()
    {
        DGJobeRate.DataSource = GetDetail();
        DGJobeRate.DataBind();
        if (DDEmployeeName.SelectedIndex > 0)
        {
            Session["ReportPath"] = "Reports/JobRateDefine.rpt";
            Session["CommanFormula"] = "{JobeRate.EmpId}=" + DDEmployeeName.SelectedValue + "";
            BtnPreview.Enabled = true;
        }
        else
        {
            BtnPreview.Enabled = false;
        }
    }
    private DataSet GetDetail()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();

            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select SrNo,CompanyName,PROCESS_NAME,ProcessType,CATEGORY_NAME,ITEM_NAME,UnitName,QualityName,Design,Color,ShapeName,SizeName,Rate from JobeRate where EmpName='" + DDEmployeeName.SelectedItem.Text + "'");
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/JobRateDefine.aspx");
        }
        finally
        {
            con.Close();
        }
        return ds;
    }

    protected void DGJobeRate_SelectedIndexChanged(object sender, EventArgs e)
    {
        BtnSave.Text = "Update";
        LblError.Text = "";
        fillBack();
    }
    private void fillBack()
    {
        TxtId.Text = DGJobeRate.SelectedValue.ToString();
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            ds = null;
            string sql = @"Select * from JobeRateDefine where JobeRateId=" + DGJobeRate.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);


            DDCompany.SelectedValue = ds.Tables[0].Rows[0]["CompanyId"].ToString();
            DDProcess.SelectedValue = ds.Tables[0].Rows[0]["ProcessId"].ToString();
            UtilityModule.ConditionalComboFill(ref DDProcessType, "select Distinct ProcessId,ProcessType from ProcessType where ProcessNameid=" + DDProcess.SelectedValue, true, "--Select--");
            DDProcessType.SelectedValue = ds.Tables[0].Rows[0]["ProcessTypeId"].ToString();
            UtilityModule.ConditionalComboFill(ref DDEmployeeName, "select Distinct EmpId,EmpName from EmpInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
            DDEmployeeName.SelectedValue = ds.Tables[0].Rows[0]["EmpId"].ToString();
            UtilityModule.ConditionalComboFill(ref DDCategory, "SELECT Distinct CATEGORY_ID,CATEGORY_NAME FROM ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
            DDCategory.SelectedValue = ds.Tables[0].Rows[0]["CategoryId"].ToString();
            GetParameters();
            UtilityModule.ConditionalComboFill(ref DDItemName, "select Item_id, Item_Name from Item_Master where Category_Id=" + DDCategory.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
            DDItemName.SelectedValue = ds.Tables[0].Rows[0]["ItemId"].ToString();
            UtilityModule.ConditionalComboFill(ref DDUnit, "SELECT DISTINCT U.UNITID,U.UNITNAME FROM UNIT U,UNIT_TYPE_MASTER UT,ITEM_MASTER IM WHERE U.UNITTYPEID=UT.UNITTYPEID AND UT.UNITTYPEID=IM.UNITTYPEID AND ITEM_ID=" + DDItemName.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
            DDUnit.SelectedValue = ds.Tables[0].Rows[0]["UnitId"].ToString();
            UtilityModule.ConditionalComboFill(ref DDQuality, "select Qualityid,Qualityname from Quality where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
            DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
            int DesignId = Convert.ToInt32(ds.Tables[0].Rows[0]["DesignId"]);
            UtilityModule.ConditionalComboFill(ref DDDesign, "select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order  by DesignName", true, "--Select--");
            if (DesignId > 0)
            {
                CHKDesign.Checked = false;
                DDDesign.Enabled = true;
                DDDesign.SelectedValue = ds.Tables[0].Rows[0]["DesignId"].ToString();
            }
            else if (DesignId == -1)
            {
                CHKDesign.Checked = true;
                DDDesign.Enabled = false;
                DDDesign.SelectedIndex = 0;
            }
            int ColorId = Convert.ToInt16(ds.Tables[0].Rows[0]["ColorId"]);
            UtilityModule.ConditionalComboFill(ref DDColor, "SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
            UtilityModule.ConditionalComboFill(ref DDShape, "select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by ShapeName", true, "--Select--");
            if (ColorId > 0)
            {
                DDColor.Enabled = true;
                DDColor.SelectedValue = ds.Tables[0].Rows[0]["ColorId"].ToString();

            }
            else if (ColorId == -1)
            {
                DDColor.Enabled = false;
                CHKColor.Checked = true;
                DDColor.SelectedIndex = 0;
            }

            DDShape.SelectedValue = ds.Tables[0].Rows[0]["ShapeId"].ToString();


            int sizeid = Convert.ToInt32(ds.Tables[0].Rows[0]["SizeId"]);
            UtilityModule.ConditionalComboFill(ref DDSize, "select SizeId,SizeFt Size_Name from Size where shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid", true, "--Select--");
            if (sizeid > 0)
            {
                CHKSize.Checked = false;

                if (Convert.ToInt32(ds.Tables[0].Rows[0]["SizeUnit"]) == 0)
                {
                    ChMtr.Checked = true;
                    UtilityModule.ConditionalComboFill(ref DDSize, "select SizeId,SizeMtr Size_Name from Size where shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid", true, "--Select--");
                }
                else
                {
                    ChMtr.Checked = false;
                    UtilityModule.ConditionalComboFill(ref DDSize, "select SizeId,SizeFt Size_Name from Size where shapeid=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid", true, "--Select--");
                }
                DDSize.SelectedValue = ds.Tables[0].Rows[0]["SizeId"].ToString();
            }
            else if (sizeid == -1)
            {
                CHKSize.Checked = true;
                DDSize.Enabled = false;
                DDSize.SelectedIndex = 0;
                ChMtr.Checked = false;
                ChMtr.Enabled = false;
            }

            TxtRate.Text = ds.Tables[0].Rows[0]["Rate"].ToString();


        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/JobRateDefine.aspx");
        }
        finally
        {

            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

    }
    private void refresh()
    {
        TxtId.Text = "0";
        TxtRate.Text = "";
        DDItemName.SelectedIndex = 0;
        UtilityModule.ConditionalComboFill(ref DDUnit, "", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDQuality, "", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDDesign, "", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDColor, "", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDShape, "", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDSize, "", true, "--Select--");
    }
    protected void DGJobeRate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGJobeRate, "Select$" + e.Row.RowIndex);
        }
    }
    private void validate()
    {
        if (DDCompany.SelectedIndex > 0 && DDProcess.SelectedIndex > 0 && DDProcessType.SelectedIndex > 0 && DDEmployeeName.SelectedIndex > 0 && DDCategory.SelectedIndex > 0 && DDItemName.SelectedIndex > 0 && DDUnit.SelectedIndex > 0 && DDQuality.SelectedIndex > 0 && TxtRate.Text != "")
        {
            if (tdD.Visible == true && CHKDesign.Checked == false)
            {
                if (DDDesign.SelectedIndex < 1)
                {
                    LblError.Text = "Please select Design...........";
                }
                else if (tdC.Visible == true && CHKColor.Checked == false)
                {
                    if (DDColor.SelectedIndex < 1)
                    {
                        LblError.Text = "Please select Color...........";
                    }
                    else if (tdSh.Visible == true)
                    {
                        if (DDShape.SelectedIndex < 1)
                        {
                            LblError.Text = "Please select Shape...........";
                        }
                        else if (tdSize.Visible == true && CHKSize.Checked == false)
                        {
                            if (DDSize.SelectedIndex < 1)
                            {
                                LblError.Text = "Please select Size...........";
                            }
                            else
                            {
                                LblError.Text = "";
                            }
                        }
                        else
                        {
                            LblError.Text = "";
                        }
                    }
                    else if (tdSize.Visible == true && CHKSize.Checked == false)
                    {
                        if (DDSize.SelectedIndex < 1)
                        {
                            LblError.Text = "Please select Size...........";
                        }
                        else
                        {
                            LblError.Text = "";
                        }
                    }
                    else
                    {
                        LblError.Text = "";
                    }
                }
                else if (tdSh.Visible == true)
                {
                    if (DDShape.SelectedIndex < 1)
                    {
                        LblError.Text = "Please select Shape...........";
                    }
                    else
                    {
                        LblError.Text = "";
                    }
                }
                else if (tdSize.Visible == true && CHKSize.Checked == false)
                {
                    if (DDSize.SelectedIndex < 1)
                    {
                        LblError.Text = "Please select Size...........";
                    }
                    else
                    {
                        LblError.Text = "";
                    }
                }
                else
                {
                    LblError.Text = "";
                }

            }
            else if (tdC.Visible == true && CHKColor.Checked == false)
            {
                if (DDColor.SelectedIndex < 1)
                {
                    LblError.Text = "Please select Color...........";
                }
                else if (tdSh.Visible == true)
                {
                    if (DDShape.SelectedIndex < 1)
                    {
                        LblError.Text = "Please select Shape...........";
                    }
                }
                else if (tdSize.Visible == true && CHKSize.Checked == false)
                {
                    if (DDSize.SelectedIndex < 1)
                    {
                        LblError.Text = "Please select Size...........";
                    }
                }
                else
                {
                    LblError.Text = "";
                }
            }
            else if (tdSh.Visible == true)
            {
                if (DDShape.SelectedIndex < 1)
                {
                    LblError.Text = "Please select Shape...........";
                }
                else if (tdSize.Visible == true && CHKSize.Checked == false)
                {
                    if (DDSize.SelectedIndex < 1)
                    {
                        LblError.Text = "Please select Size...........";
                    }
                }
                else
                {
                    LblError.Text = "";
                }
            }
            else if (tdSize.Visible == true && CHKSize.Checked == false)
            {
                if (DDSize.SelectedIndex < 1)
                {
                    LblError.Text = "Please select Size...........";
                }
            }
            else
            {
                LblError.Text = "";
            }
        }
        else
        {
            LblError.Text = "Importent Fields missing..........";
        }
    }
   
    protected void DGJobeRate_RowCreated(object sender, GridViewRowEventArgs e)
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