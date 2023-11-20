using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using ClosedXML.Excel;

public partial class Masters_MapStencil_FrmDirectMapTraceStock : System.Web.UI.Page
{
    int ItemFinishedId = 0;
    string remainstock = null;
    static int MasterCompanyId;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varCompanyId"]);

        hnVarNewQualitySize.Value = variable.VarNewQualitySize;
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }   

        lblerror.Text = "";
        Label2.Text = "";
        lblMessageVal.Text = "";
        lblsave.Text = "";
        lblmessage.Text = "";
        txtstockid.Text = "0";
        if (!IsPostBack)
        {
            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            //txttrandate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            txtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            string str = @"select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By Companyname 
                           Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + @" Order by GodownName
                        Select VarProdCode from MasterSetting";
            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref ddlcompany, ds, 0, true, "Select Comp Name");
            //UtilityModule.ConditionalComboFillWithDS(ref ddlgodown, ds, 1, true, "Select Godown");

            if (ddlcompany.Items.Count > 0)
            {
                ddlcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddlcompany.Enabled = false;
            }

            int VarProdCode = Convert.ToInt32(ds.Tables[2].Rows[0][0].ToString());
            if (VarProdCode == 1)
            {
                code.Visible = true;
            }
            else
            {
                code.Visible = false;
            }
            lablechange();
            //****************************************************

            UtilityModule.ConditionalComboFill(ref ddlcatagoryname, @"SELECT  Distinct dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME  FROM  dbo.CategorySeparate INNER JOIN
        dbo.ITEM_CATEGORY_MASTER ON dbo.CategorySeparate.Categoryid = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID 
        WHERE dbo.CategorySeparate.id =0 And ITEM_CATEGORY_MASTER.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Catagory");

            Txtpostfix.Visible = true;
            txtprefix.Visible = true;
            txtprefix.Text = "";            
            //TRcurrentstock.Visible = false;
            if (variable.Carpetcompany == "1")
            {
                TbFinished.Visible = true;
                //TDgvcarpetdetail.Visible = false;

                TRFinishedStockReport.Visible = true;

                //switch (Session["varcompanyId"].ToString())
                //{
                //    case "21":
                //        TDDirectStockRemark.Visible = true;
                //        break;
                //    case "36":
                //        TDDirectStockRemark.Visible = true;
                //        break;
                //    case "42":
                //        TRFinishedStockReport.Visible = true;
                //        break;
                //}
            }

            switch (Session["varcompanyId"].ToString())
            {
               
                case "21":
                    ChkMtr.Checked = true;                    
                    break;                
                case "9":
                    ChkForExcelExport.Visible = true;
                    break;
                case "42":
                    //BtnStockNoToVCMSale.Visible = true;                   
                    ChkForInchSize.Visible = true;
                    break;
                case "44":
                    ChkForExcelExport.Visible = false;
                    ChkForInchSize.Visible = true;
                    break;
                default:
                    ChkForExcelExport.Visible = false;
                    break;
            }
           
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
        lblsizename.Text = ParameterList[4];
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
        lblshadecolor.Text = ParameterList[7];
    }
    protected void ddlcatagoryname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
        TxtFinishedid.Text = "Category=" + ddlcatagoryname.SelectedValue;
    }
    private void ddlcategorycange()
    {
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                      " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                      " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddlcatagoryname.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        ql.Visible = true;
                        UtilityModule.ConditionalComboFill(ref dquality, "select Qualityid,Qualityname from Quality Where MasterCompanyId=" + Session["varCompanyId"] + " order by qualityname", true, "--Select Quality--");
                        break;
                    case "2":
                        dsn.Visible = true;
                        UtilityModule.ConditionalComboFill(ref dddesign, "select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varCompanyId"] + " Order  by DesignName ", true, "Select Design");
                        break;
                    case "3":
                        clr.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddcolor, "SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varCompanyId"] + " order by ColorName", true, "--Select Color--");
                        break;
                    case "4":
                        shp.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddshape, "select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " Order by ShapeName", true, "--Select Shape--");
                        break;
                    case "5":
                        sz.Visible = true;
                        if (variable.VarNewQualitySize == "1")
                        {
                            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,Export_Format from QualitySizeNew order by sizeid ", true, "Size in Ft");
                        }
                        else
                        {
                            if (Session["varcompanyId"].ToString() == "21")
                            {
                                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizemtr from size Where MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid ", true, "Size in Mtr");
                            }
                            else
                            {
                                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size Where MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid ", true, "Size in Ft");
                            }
                        }
                        break;
                    case "6":
                        shd.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddlshade, "select shadecolorid,shadecolorname from shadecolor Where MasterCompanyId=" + Session["varCompanyId"] + " order by shadecolorname", true, "Select Shadecolor");
                        break;
                }
            }
        }
        UtilityModule.ConditionalComboFill(ref ddlitemname, "Select Distinct Item_Id,Item_Name from Item_Master where Category_Id=" + ddlcatagoryname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order by Item_Name", true, "--Select Item--");
    }
    protected void ddMapTraceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Txtpostfix.Text = "";
        txtprefix.Text = "";       
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        CheckBackDateEntryStop();
        if (lblMessageVal.Text == "")
        {
            if (txtopeningstock.Text != "")
            {
                ItemFinishedId = UtilityModule.getItemFinishedId(ddlitemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
               
               
                    savecarpetdetail();
               
                if (variable.Carpetcompany == "1")
                {
                    FillGridFinisheditem();
                }
                //else
                //{
                //    fill_grid();
                //}
                //if (ddlcatagorytype.SelectedValue != "0")
                //{
                //    Refresh();
                //}

            }
            else
            {
                lblsave.Visible = true;
            }
            hnstockid.Value = "0";
        }
    }
    
    private void savecarpetdetail()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            int VarOpeningQty = Convert.ToInt32(txtopeningstock.Text);
            int a = 0;
            //for (int i = 1; i <= VarOpeningQty; i++)
            //{
            //    string VarTStockNoCheck = txtprefix.Text + Txtpostfix.Text;
            //    DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select TstockNo from CarpetNumber Where TstockNo='" + VarTStockNoCheck + "'");
            //    if (Ds.Tables[0].Rows.Count > 0)
            //    {
            //        a = 1;
            //    }
            //}
            if (a == 0)
            {
                //if (btnsave.Text == "Update")
                //{
                //    saverawdetail2();
                //}
                //else
                //{
                //    saverawdetailcarpet();
                //}
                ItemFinishedId = UtilityModule.getItemFinishedId(ddlitemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                string VarTStockNo = txtprefix.Text + Txtpostfix.Text;
                int VarTStockNo1 = Convert.ToInt32(Txtpostfix.Text);
                SqlParameter[] arr = new SqlParameter[12];
                arr[0] = new SqlParameter("@Item_Finished_ID", SqlDbType.Int);
                arr[1] = new SqlParameter("@companyid", SqlDbType.Int);
                arr[2] = new SqlParameter("@MSStockNo", SqlDbType.NVarChar);
                arr[3] = new SqlParameter("@MSStockNo1", SqlDbType.Int);
                arr[4] = new SqlParameter("@prefix", SqlDbType.NVarChar);
                arr[5] = new SqlParameter("@recdate", SqlDbType.DateTime);
                arr[6] = new SqlParameter("@stock", SqlDbType.Int);
                arr[7] = new SqlParameter("@Userid", SqlDbType.Int);
                arr[8] = new SqlParameter("@Unitid", SqlDbType.Int);
                arr[9] = new SqlParameter("@Recqty", SqlDbType.Int);
                arr[10] = new SqlParameter("@Postfix", SqlDbType.Int);
                arr[11] = new SqlParameter("@MapTraceType", SqlDbType.Int);
                //arr[11] = new SqlParameter("@DirectStockRemark", SqlDbType.VarChar, (100));

                arr[0].Value = ItemFinishedId;
                arr[1].Value = ddlcompany.SelectedValue;
                arr[2].Value = VarTStockNo;
                arr[3].Value = VarTStockNo1;
                arr[4].Value = txtprefix.Text;
                arr[5].Value = txtdate.Text;
                arr[6].Value = txtstockid.Text;
                arr[7].Value = Session["varuserid"].ToString();
                arr[8].Value = ddlunit.SelectedValue;
                arr[9].Value = txtopeningstock.Text == "" ? "0" : txtopeningstock.Text;
                arr[10].Direction = ParameterDirection.InputOutput;
                arr[10].Value = Txtpostfix.Text == "" ? "0" : Txtpostfix.Text;
                arr[11].Value = ddMapTraceType.SelectedValue;
                //arr[11].Value = txtDirectStockRemark.Text;


                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "[PRO_DIRECTSTOCKMAPTRACE]", arr);
                Txtpostfix.Text = arr[10].Value.ToString();
                
                lblerror.Visible = false;
            }
            else
            {
                lblerror.Visible = true;
                lblmessage.Visible = false;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/MapStencil/FrmDirectMapTraceStock.aspx");
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

    protected void txtprefix_TextChanged(object sender, EventArgs e)
    {
        Txtpostfix.Text = UtilityModule.CalculatePostFixMapTrace(txtprefix.Text).ToString();
    }
    
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlunit.SelectedValue == "1")
        {
            if (variable.VarNewQualitySize == "1")
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,MtrSize from QualitySizeNew where Shapeid=" + ddshape.SelectedValue + " order by MtrSize", true, "select size");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizemtr from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizemtr", true, "select size");
            }
        }
        else if (ddlunit.SelectedValue == "2")
        {
            if (variable.VarNewQualitySize == "1")
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,Export_Format from QualitySizeNew where Shapeid=" + ddshape.SelectedValue + " order by Export_Format", true, "select size");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeft", true, "select size");
            }
        }
        // fill_grid();
    }
    protected void ddlitemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        itemchanged();
        TxtFinishedid.Text = "Category=" + ddlcatagoryname.SelectedValue + "&Item=" + ddlitemname.SelectedValue;
    }
    private void itemchanged()
    {
        //if (ddlcatagorytype.SelectedValue == "0")
        //{
            if (ddlitemname.SelectedIndex > 0)
            {
                if (variable.VarCarpetPrefixauto == "1")
                {
                    txtprefix.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Item_Code from Item_Master Where Item_Id=" + ddlitemname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"]).ToString();
                    string get_year = DateTime.Now.ToString("dd-MMM-yyyy");
                    string lastTwoChars = get_year.Substring(get_year.Length - 2);
                    if (txtprefix.Text != "")
                    {
                        txtprefix.Text = (txtprefix.Text + "-" + lastTwoChars).Replace(" ", "");
                    }
                    else
                    {
                        txtprefix.Text = "";
                    }
                }

            }
            else
            {
                txtprefix.Text = "";
            }
            Txtpostfix.Text = Convert.ToString(UtilityModule.CalculatePostFixMapTrace((txtprefix.Text).ToUpper()));
        //}
        string str = @"SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + ddlitemname.SelectedValue + " And i.MasterCompanyId=" + Session["varCompanyId"];
        str = str + @" select qualityid,qualityname from quality where item_id=" + ddlitemname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by qualityname";
        DataSet ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref ddlunit, ds, 0, true, "Select Unit");
        UtilityModule.ConditionalComboFillWithDS(ref dquality, ds, 1, true, "Select Quallity");
        if (ddlunit.Items.Count > 0)
        {
            if (Session["varcompanyId"].ToString() == "21")
            {
                ddlunit.SelectedIndex = 1;
            }
            else
            {
                if (ddlunit.Items.FindByValue("2") != null)
                {
                    ddlunit.SelectedValue = "2";
                }
                else
                {
                    ddlunit.SelectedIndex = 1;
                }
            }

        }
        //  fill_grid();
    }
    
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER IPM inner join item_Master IM on IM.Item_Id=IPM.Item_Id inner join CategorySeparate CS on CS.CategoryId=IM.Category_Id  where ProductCode Like  '" + prefixText + "%' And IM.MasterCompanyId=" + MasterCompanyId;
        //string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER  where ProductCode Like  '" + prefixText + "%'";
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
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds1;
        string Str;
        if (TxtProdCode.Text != "")
        {
            ddlcatagoryname.SelectedIndex = -1;
            Str = @"select IPM.*,IM.CATEGORY_ID,cs.id from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,CategorySeparate cs where IPM.ITEM_ID=IM.ITEM_ID and im.CATEGORY_ID=cs.Categoryid
                  and ProductCode='" + TxtProdCode.Text + "' And IM.MasterCompanyId=" + Session["varCompanyId"];
            ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                string Qry;
                if (variable.VarNewQualitySize == "1")
                {
                    Qry = @"SELECT dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME  FROM  dbo.CategorySeparate INNER JOIN
                    dbo.ITEM_CATEGORY_MASTER ON dbo.CategorySeparate.Categoryid = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID And ITEM_CATEGORY_MASTER.MasterCompanyId=" + Session["varCompanyId"];
                    Qry = Qry + "  Select Distinct Item_Id,Item_Name from Item_Master where MasterCompanyId=" + Session["varCompanyId"] + " And  Category_Id=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["CATEGORY_ID"].ToString()) + " Order by Item_Name";
                    Qry = Qry + "  select qualityid,qualityname from quality where MasterCompanyId=" + Session["varCompanyId"] + " And item_id=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["ITEM_ID"].ToString()) + " order by qualityname";
                    Qry = Qry + @" select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varCompanyId"] + " Order  by DesignName";
                    Qry = Qry + @" SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varCompanyId"];
                    Qry = Qry + @" select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " Order by ShapeName";
                    Qry = Qry + @"  SELECT SIZEID,Export_Format fROM QualitySizeNew WhERE SHAPEID=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["SIZE_ID"].ToString());
                }
                else
                {
                    Qry = @"SELECT dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME  FROM  dbo.CategorySeparate INNER JOIN
                    dbo.ITEM_CATEGORY_MASTER ON dbo.CategorySeparate.Categoryid = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID And ITEM_CATEGORY_MASTER.MasterCompanyId=" + Session["varCompanyId"];
                    Qry = Qry + "  Select Distinct Item_Id,Item_Name from Item_Master where MasterCompanyId=" + Session["varCompanyId"] + " And  Category_Id=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["CATEGORY_ID"].ToString()) + " Order by Item_Name";
                    Qry = Qry + "  select qualityid,qualityname from quality where MasterCompanyId=" + Session["varCompanyId"] + " And item_id=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["ITEM_ID"].ToString()) + " order by qualityname";
                    Qry = Qry + @" select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varCompanyId"] + " Order  by DesignName";
                    Qry = Qry + @" SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varCompanyId"];
                    Qry = Qry + @" select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " Order by ShapeName";
                    Qry = Qry + @"  SELECT SIZEID,SIZEFT fROM SIZE WhERE MasterCompanyId=" + Session["varCompanyId"] + " And SHAPEID=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["SIZE_ID"].ToString());
                }
                DataSet DSQ = SqlHelper.ExecuteDataset(Qry);

                UtilityModule.ConditionalComboFillWithDS(ref ddlcatagoryname, DSQ, 0, true, "Select Catagory");
                ddlcatagoryname.SelectedValue = ds1.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddlitemname, DSQ, 1, true, "--Select Item--");
                ddlitemname.SelectedValue = ds1.Tables[0].Rows[0]["ITEM_ID"].ToString();

                UtilityModule.ConditionalComboFillWithDS(ref dquality, DSQ, 2, true, "Select Quallity");
                dquality.SelectedValue = ds1.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref dddesign, DSQ, 3, true, "Select Design");
                dddesign.SelectedValue = ds1.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddcolor, DSQ, 4, true, "--Select Color--");
                ddcolor.SelectedValue = ds1.Tables[0].Rows[0]["COLOR_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddshape, DSQ, 5, true, "--Select Shape--");
                ddshape.SelectedValue = ds1.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddsize, DSQ, 6, true, "--SELECT SIZE--");
                ddsize.SelectedValue = ds1.Tables[0].Rows[0]["SIZE_ID"].ToString();

                Session["finishedid"] = ds1.Tables[0].Rows[0]["Item_Finished_id"].ToString();
                if (Convert.ToInt32(dquality.SelectedValue) > 0)
                {
                    ql.Visible = true;
                }
                else
                {
                    ql.Visible = false;
                }
                int d = (dddesign.SelectedIndex > 0 ? Convert.ToInt32(dddesign.SelectedValue) : 0);
                if (d > 0)
                {
                    dsn.Visible = true;
                }
                else
                {
                    dsn.Visible = false;
                }
                int c = (ddcolor.SelectedIndex > 0 ? Convert.ToInt32(ddcolor.SelectedValue) : 0);
                if (c > 0)
                {
                    clr.Visible = true;
                }
                else
                {
                    clr.Visible = false;
                }
                int s = (ddshape.SelectedIndex > 0 ? Convert.ToInt32(ddshape.SelectedValue) : 0);
                if (s > 0)
                {
                    shp.Visible = true;
                }
                else
                {
                    shp.Visible = false;
                }
                int si = (ddsize.SelectedIndex > 0 ? Convert.ToInt32(ddsize.SelectedValue) : 0);
                if (si > 0)
                {
                    sz.Visible = true;
                }
                else
                {
                    sz.Visible = false;
                }
                UtilityModule.ConditionalComboFill(ref ddlunit, "SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + ddlitemname.SelectedValue + " And i.MasterCompanyId=" + Session["varCompanyId"], true, "Select Unit");
                Label2.Visible = false;
            }
            else
            {
                Label2.Visible = true;
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
            }
        }
        else
        {
            ddlcatagoryname.SelectedIndex = 0;
        }
    }
    
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Response.Redirect("../../main.aspx");
    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (Session["VarCompanyNo"].ToString() == "30")
        //{
        //    if (ddlcatagorytype.SelectedValue == "0")
        //    {
        //        if (ddlitemname.SelectedIndex > 0)
        //        {
        //            string str = @"select distinct D.DesignId,D.DesignName from Design D JOIN V_FinishedItemDetail Vf ON D.designId=Vf.designId Where VF.CATEGORY_ID=" + ddlcatagoryname.SelectedValue + " and VF.ITEM_ID=" + ddlitemname.SelectedValue + " and Vf.QualityId=" + dquality.SelectedValue + " Order  by DesignName";
        //            DataSet ds = SqlHelper.ExecuteDataset(str);
        //            UtilityModule.ConditionalComboFillWithDS(ref dddesign, ds, 0, true, "Select Design");
        //        }
        //    }
        //}
        //// fill_grid();
    }
    protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        // fill_grid();
    }
    protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        // fill_grid();
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        // fill_grid();
    }
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        // fill_grid();
    }
    private void CHECKVALIDCONTROL()
    {
        lblMessageVal.Text = "";
        lblMessageVal.Visible = true;
        if (UtilityModule.VALIDDROPDOWNLIST(ddlcompany) == false)
        {
            goto a;
        }
        //if (UtilityModule.VALIDDROPDOWNLIST(ddlcatagorytype) == false)
        //{
        //    goto a;
        //}
        if (UtilityModule.VALIDDROPDOWNLIST(ddlcatagoryname) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddlitemname) == false)
        {
            goto a;
        }
        if (ql.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(dquality) == false)
            {
                goto a;
            }
        }
        if (clr.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddcolor) == false)
            {
                goto a;
            }
        }
        if (shp.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddshape) == false)
            {
                goto a;
            }
        }
        if (sz.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddsize) == false)
            {
                goto a;
            }
        }
        if (shd.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddlshade) == false)
            {
                goto a;
            }
        }
        //if (TDGodownId.Visible == true)
        //{
        //    if (UtilityModule.VALIDDROPDOWNLIST(ddlgodown) == false)
        //    {
        //        goto a;
        //    }
        //}
        if (UtilityModule.VALIDDROPDOWNLIST(ddlunit) == false)
        {
            goto a;
        }
        //if (UtilityModule.VALIDTEXTBOX(txtRate) == false)
        //{
        //    goto a;
        //
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(lblMessageVal);
    B: ;
    }
    protected void Refresh()
    {
        lblsave.Visible = false;
        txtopeningstock.Text = "";
        TxtProdCode.Text = "";
        //txtlotno.Text = "";
        //txttagno.Text = "";
        lblerror.Visible = false;
        txtprefix.Visible = false;
        Txtpostfix.Visible = false;
        btnsave.Text = "Save";
        //ddlgodown.Enabled = true;
       
        //TxtRate.Text = "0";
        //Txtminstock.Text = "0";

        switch (Session["varcompanyid"].ToString())
        {
            case "30":
            case "27":
                break;
            default:
                ddlunit.SelectedValue = null;
                break;
        }

        if (ql.Visible == true)
        {
            //if (Session["varcompanyId"].ToString() != "27") 
            //{
            //    dquality.SelectedValue = null;
            //}

            switch (Session["varcompanyid"].ToString())
            {
                case "30":
                case "27":
                    break;
                default:
                    dquality.SelectedValue = null;
                    break;
            }
           
        }
        if (clr.Visible == true)
        {
            ddcolor.SelectedValue = null;
        }
        if (shp.Visible == true)
        {
            ddshape.SelectedValue = null;
        }
        if (sz.Visible == true)
        {
            ddsize.SelectedValue = null;
        }
        if (shd.Visible == true)
        {
           ddlshade.SelectedValue = null;          
        }
        //txtnoofcone.Text = "";
        hnstockid.Value = "0";
        hnqtyinhand.Value = "0";
        //btnupdatestock.Enabled = false;
        //txtcurrentstock.Text = "";
    }
    protected void btnpriview_Click(object sender, EventArgs e)
    {
//        if (ChkForExcelExport.Checked == true)
//        {
//            if (Session["varCompanyId"].ToString() == "9")
//            {
//                ExportExcelReport();
//            }
//        }
//        else
//        {
//            if (Session["varCompanyId"].ToString() == "9")
//            {
//                Session["ReportPath"] = "reports/OpenStockHafizia.rpt";
//            }
//            else
//            {
//                Session["ReportPath"] = "reports/openstock.rpt";
//            }
//            string qry = "";
//            qry = @" SELECT CATEGORY_NAME,ITEM_NAME,DESCRIPTION,OpenStock,Qtyinhand,LotNo,GodownName FROM  openstock
//                 Where OpenStock.CompanyID=" + ddlcompany.SelectedValue + " And CompanyId=" + ddlcompany.SelectedValue + " And (Round(OpenStock,3)<>0 or Round(QtyinHand,3)<>0)";
//            //Session["CommanFormula"] = "{OpenStock.CompanyID}=" + ddlcompany.SelectedValue + "";
//            if (ddlcatagorytype.SelectedIndex > 0)
//            {
//                qry = qry + "and OpenStock.TypeID=" + ddlcatagorytype.SelectedValue + "";
//                // Session["CommanFormula"] = Session["CommanFormula"] + " And {OpenStock.TypeID}=" + ddlcatagorytype.SelectedValue + "";
//            }
//            if (ddlcatagoryname.SelectedIndex > 0)
//            {
//                qry = qry + "and OpenStock.Category_id=" + ddlcatagoryname.SelectedValue + "";
//                //Session["CommanFormula"] = Session["CommanFormula"] + " And {OpenStock.Category_id}=" + ddlcatagoryname.SelectedValue + "";
//            }
//            if (ddlitemname.SelectedIndex > 0)
//            {
//                qry = qry + " and OpenStock.Item_id=" + ddlitemname.SelectedValue + "";
//                //Session["CommanFormula"] = Session["CommanFormula"] + " And {OpenStock.Item_id}=" + ddlitemname.SelectedValue + "";
//            }
//            if (dquality.SelectedIndex > 0 && ql.Visible == true)
//            {
//                qry = qry + "and OpenStock.Qualityid=" + dquality.SelectedValue + "";
//                //Session["CommanFormula"] = Session["CommanFormula"] + " And {OpenStock.Qualityid}=" + dquality.SelectedValue + "";
//            }
//            if (dddesign.SelectedIndex > 0 && dsn.Visible == true)
//            {
//                qry = qry + " and OpenStock.DesignID=" + dddesign.SelectedValue + "";
//                //Session["CommanFormula"] = Session["CommanFormula"] + " And {OpenStock.DesignID}=" + dddesign.SelectedValue + "";
//            }
//            if (ddcolor.SelectedIndex > 0 && clr.Visible == true)
//            {
//                qry = qry + " and OpenStock.Colorid=" + ddcolor.SelectedValue + "";
//                //Session["CommanFormula"] = Session["CommanFormula"] + " And {OpenStock.Colorid}=" + ddcolor.SelectedValue + "";
//            }
//            if (ddshape.SelectedIndex > 0 && shp.Visible == true)
//            {
//                qry = qry + " and OpenStock.Shapeid=" + ddshape.SelectedValue + " ";
//                //Session["CommanFormula"] = Session["CommanFormula"] + " And {OpenStock.Shapeid}=" + ddshape.SelectedValue + "";
//            }
//            if (ddsize.SelectedIndex > 0 && sz.Visible == true)
//            {
//                qry = qry + " and OpenStock.Sizeid=" + ddsize.SelectedValue + "";
//                //Session["CommanFormula"] = Session["CommanFormula"] + " And {OpenStock.Sizeid}=" + ddsize.SelectedValue + "";
//            }
//            if (ddlshade.SelectedIndex > 0 && shd.Visible == true)
//            {
//                qry = qry + " And  OpenStock.ShadeColorid=" + ddlshade.SelectedValue + "";
//                Session["CommanFormula"] = Session["CommanFormula"] + " And {OpenStock.ShadeColorid}=" + ddlshade.SelectedValue + "";
//            }
//            if (ddlgodown.SelectedIndex > 0)
//            {
//                qry = qry + " And OpenStock.GodownId=" + ddlgodown.SelectedValue + "";
//            }
//            qry = qry + " ORDER BY CATEGORY_NAME,DESCRIPTION";
//            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
//            //Session["rptFileName"] = "~\\Reports\\PGenrateIndentNEW.rpt";
//            Session["rptFileName"] = Session["ReportPath"];
//            Session["GetDataset"] = ds;
//            Session["dsFileName"] = "~\\ReportSchema\\openstock.xsd";
//            StringBuilder stb = new StringBuilder();
//            stb.Append("<script>");
//            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
//            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
//        }
    }
    //protected void gvcarpetdetail_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //Add CSS class on header row.
    //    if (e.Row.RowType == DataControlRowType.Header)
    //        e.Row.CssClass = "header";
    //    //Add CSS class on normal row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Normal)
    //        e.Row.CssClass = "normal";
    //    //Add CSS class on alternate row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Alternate)
    //        e.Row.CssClass = "alternate";
    //}
    protected void refreshcategory_Click(object sender, EventArgs e)
    {
        ddlcategorycange();
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }

    protected void BtnRefreshItem_Click(object sender, EventArgs e)
    {
        itemchanged();
    }
    protected void refreshquality_Click(object sender, EventArgs e)
    {
        // fill_grid();
    }
    protected void refreshdesign_Click(object sender, EventArgs e)
    {
        // fill_grid();
    }
    protected void refreshcolor_Click(object sender, EventArgs e)
    {
        //  fill_grid();
    }
    protected void refreshshape_Click(object sender, EventArgs e)
    {
        //  fill_grid();
    }
    protected void BtnRefreshSize_Click(object sender, EventArgs e)
    {
        //fill_grid();
    }
    protected void refreshshade_Click(object sender, EventArgs e)
    {
        // fill_grid();
    }
    protected void ChkMtr_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkMtr.Checked == true)
        {
            if (variable.VarNewQualitySize == "1")
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,MtrSize from QualitySizeNew where Shapeid=" + ddshape.SelectedValue, true, "select size");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizemtr from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"], true, "select size");
            }
        }
        else
        {
            if (variable.VarNewQualitySize == "1")
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,Export_Format from QualitySizeNew where Shapeid=" + ddshape.SelectedValue + " order by Export_Format", true, "select size");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeft", true, "select size");
            }
        }

    }
    protected void btnshowDetail_Click(object sender, EventArgs e)
    {
        //System.Threading.Thread.Sleep(3000);
        lblloading.Visible = true;
        FillGridFinisheditem();        

        lblloading.Visible = false;
    }
    protected void FillGridFinisheditem()
    {
        string view = "V_FinishedItemDetail";
        if (variable.VarNewQualitySize == "1")
        {
            view = "V_FinishedItemDetailNew";
        }
        string str =string.Empty;
        if (Session["varCompanyId"].ToString() == "44")
        {
            str = @"select vf.item_name,Vf.QualityName,Vf.designname,vf.ColorName,vf.ShapeName,
                case when CN.Unitid=1 then vf.SizeMtr when CN.Unitid=6 then vf.SizeInch else vf.SizeFt end as SizeFt,CN.MSStockNo,CN.MapStencilNo
                From  MAP_STENCILSTOCKNO CN Inner Join " + view + @" Vf on CN.Item_Finished_Id=Vf.ITEM_FINISHED_ID
                Where CN.Type=0 and CN.PackStatus=0 ";
        }
        else
        {
            str = @"select vf.item_name,Vf.QualityName,Vf.designname,vf.ColorName,vf.ShapeName,
                case when CN.Unitid=1 then vf.SizeMtr  else vf.SizeFt end as SizeFt,CN.MSStockNo,CN.MapStencilNo
                From  MAP_STENCILSTOCKNO CN Inner Join " + view + @" Vf on CN.Item_Finished_Id=Vf.ITEM_FINISHED_ID
                Where CN.Type=0 and CN.PackStatus=0 ";        
        
        
        }
        if (ddlitemname.SelectedIndex > 0)
        {
            str = str + " and Vf.Item_id=" + ddlitemname.SelectedValue;
        }
        if (dquality.SelectedIndex > 0)
        {
            str = str + " and Vf.QualityId=" + dquality.SelectedValue;
        }
        if (dddesign.SelectedIndex > 0)
        {
            str = str + " and Vf.DesignId=" + dddesign.SelectedValue;
        }
        if (ddcolor.SelectedIndex > 0)
        {
            str = str + " and Vf.Colorid=" + ddcolor.SelectedValue;
        }
        if (ddshape.SelectedIndex > 0)
        {
            str = str + "  and Vf.Shapeid=" + ddshape.SelectedValue;
        }
        if (ddsize.SelectedIndex > 0)
        {
            str = str + " and Vf.Sizeid=" + ddsize.SelectedValue;
        }
        str = str + "  order by CN.MapStencilNo";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GDFinishedDetail.DataSource = ds.Tables[0];
        GDFinishedDetail.DataBind();

    }
    protected int GetGridColumnId(string ColName)
    {
        int columnid = -1;
        //foreach (DataControlField col in gvcarpetdetail.Columns)
        //{
        //    if (col.HeaderText.ToUpper().Trim() == ColName.ToUpper())
        //    {
        //        columnid = gvcarpetdetail.Columns.IndexOf(col);
        //        break;
        //    }
        //}
        return columnid;
    }

    protected void GDFinishedDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblMessageVal.Visible = false;
        lblMessageVal.Text = "";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();

        try
        {
            Label lblMapStencilNo = (Label)GDFinishedDetail.Rows[e.RowIndex].FindControl("lblMapStencilNo");
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@MapStencilNo", lblMapStencilNo.Text);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEDIRECTMAPTRACESTOCKENTRY", param);
            Tran.Commit();
            lblMessageVal.Visible = true;
            lblMessageVal.Text = param[1].Value.ToString();
            FillGridFinisheditem();
        }
        catch (Exception ex)
        {
            lblMessageVal.Visible = true;
            lblMessageVal.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    //protected void ddlgodown_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (TDBinnowise.Visible == true)
    //    {
    //        string str = "select BINNO,BINNO as BINNO1,BINid From BINMASTER Where Godownid=" + ddlgodown.SelectedValue + " order by BINID";
    //        UtilityModule.ConditionalComboFill(ref DDBinNo, str, true, "--Plz Select--");
    //    }
    //}
    private void CheckBackDateEntryStop()
    {
        //Check StopBackEntryDate
       //// if (variable.VarStopBackDateEntryOnAllForms == "1" && Session["varuserid"].ToString() != "1")
       //// {
        if (variable.VarStopBackDateEntryOnAllForms == "1")
        {
            string currentdate = DateTime.Now.ToString("dd-MMM-yyyy");

            //if (Convert.ToDateTime(txtdate.Text) < Convert.ToDateTime(currentdate))
            //{
            //    lblMessageVal.Visible = true;
            //    lblMessageVal.Text = "DATE NOT BE LESS THAN CURRENT DATE";
            //    return;
            //}

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();


            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter("@COMPANYID", ddlcompany.SelectedValue);
            param[1] = new SqlParameter("@DATE", txtdate.Text);
            param[2] = new SqlParameter("@MASTERCOMPANYID", Session["VarcompanyNo"]);
            param[3] = new SqlParameter("@ReturnDate", SqlDbType.VarChar, 30);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@ReturnHolidayType", SqlDbType.VarChar, 50);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@UserTypeId", Session["usertype"]);
            param[6] = new SqlParameter("@UserTypeNew", SqlDbType.Int);
            param[6].Direction = ParameterDirection.Output;
            param[7] = new SqlParameter("@UserID", Session["varuserId"]);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_GETRETURNDATE_HOLIDAYNAME", param);

            if (param[6].Value.ToString() == "1")
            {
                if (Convert.ToDateTime(txtdate.Text) < Convert.ToDateTime(currentdate))
                {
                    lblMessageVal.Visible = true;
                    lblMessageVal.Text = "DATE NOT BE LESS THAN CURRENT DATE";
                    return;
                }
            }
            
            if (txtdate.Text == param[3].Value.ToString())
            {
                lblMessageVal.Visible = true;
                lblMessageVal.Text = "PLEASE SELECT ANOTHER DATE DUE TO HOLIDAY.";
                Tran.Commit();
                return;
            }
            else
            {
                if (param[3].Value.ToString() != "")
                {
                    txtdate.Text = param[3].Value.ToString();
                }
            }
        }
    }

    
    protected void DirectFinishedStockExcelReport()
    {
        //if (ddlcatagorytype.SelectedValue == "0")
        //{
            #region Where Condition
            string Where = "";
            if (ddlitemname.SelectedIndex > 0)
            {
                Where = Where + " And VF.Item_Id=" + ddlitemname.SelectedValue;
                // filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
            }
            if (dquality.SelectedIndex > 0)
            {
                Where = Where + " And VF.QualityId=" + dquality.SelectedValue;
                //filterby = filterby + " Order No : " + ddOrderno.SelectedItem.Text;
            }
            if (dddesign.SelectedIndex > 0)
            {
                Where = Where + " And VF.DesignId=" + dddesign.SelectedValue;
                //filterby = filterby + " Supp Name : " + dsuppl.SelectedItem.Text;
            }
            if (ddcolor.SelectedIndex > 0)
            {
                Where = Where + " And VF.ColorId=" + ddcolor.SelectedValue;
                //filterby = filterby + " PO No : " + DDPONo.SelectedItem.Text;
            }
            if (ddshape.SelectedIndex > 0)
            {
                Where = Where + " And VF.ShapeId=" + ddshape.SelectedValue;
                //filterby = filterby + " Category : " + ddCatagory.SelectedItem.Text;
            }
            if (ddsize.SelectedIndex > 0)
            {
                Where = Where + " And VF.SizeId=" + ddsize.SelectedValue;
                // filterby = filterby + " Item : " + dditemname.SelectedItem.Text;
            }
            if (ddlshade.SelectedIndex > 0)
            {
                Where = Where + " And VF.ShadecolorId=" + ddlshade.SelectedValue;
                //filterby = filterby + " Quality : " + dquality.SelectedItem.Text;
            }
            //if (ddlgodown.SelectedIndex > 0)
            //{
            //    Where = Where + " And S.godownId=" + ddlgodown.SelectedValue;
            //    //filterby = filterby + " design : " + dddesign.SelectedItem.Text;
            //}
            if (ddlunit.SelectedIndex > 0)
            {
                Where = Where + " And St.unitid=" + ddlunit.SelectedValue;
                //filterby = filterby + " Color : " + ddcolor.SelectedItem.Text;
            }
            //if (txtlotno.Text != "")
            //{
            //    Where = Where + " And S.LotNo='" + txtlotno.Text + "'";

            //}
            #endregion

            //SqlParameter[] param = new SqlParameter[4];
            //param[0] = new SqlParameter("@CompanyId", ddlcompany.SelectedValue);
            //param[1] = new SqlParameter("@TypeId", ddlcatagorytype.SelectedValue);
            //param[2] = new SqlParameter("@Where", Where);
            //param[3] = new SqlParameter("@MasterCompayId", Session["VarCompanyId"]);

            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETQtyInHandStockExcelReport", param);


            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETDirectMapTraceStockExcelReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", ddlcompany.SelectedValue);
            //cmd.Parameters.AddWithValue("@TypeId", ddlcatagorytype.SelectedValue);
            cmd.Parameters.AddWithValue("@FromDate", txtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);
            cmd.Parameters.AddWithValue("@Where", Where);
            cmd.Parameters.AddWithValue("@MasterCompayId", Session["VarCompanyId"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);


            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                //Decimal TQty = 0, TAmount = 0, TBillQty = 0, TBillAmt = 0, TTransportAmt = 0, TUnloadingAmt = 0, ActualRecQty = 0;
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;
                //***********
                sht.Range("A1:N1").Merge();
                //sht.Range("A1").Value = "Purchase Material Received " + "For - " + ddCompName.SelectedItem.Text;
                sht.Range("A1").Value = "Direct Map/Trace Stock Report " + "For - " + ddlcompany.SelectedItem.Text;
                sht.Range("A2:N2").Merge();
                //sht.Range("A2").Value = "Filter By :  " + filterby;
                sht.Range("A2").Value = "";
                sht.Row(2).Height = 30;
                sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:N2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A1:N2").Style.Font.Bold = true;
                //***********Show Data
                row = 3;

                sht.Range("A" + row).Value = "ITEM NAME";
                sht.Range("B" + row).Value = "QUALITY";
                sht.Range("C" + row).Value = "DESIGN";
                sht.Range("D" + row).Value = "COLOR";
                sht.Range("E" + row).Value = "SHAPE";
                sht.Range("F" + row).Value = "SIZE ";
                sht.Range("G" + row).Value = "STOCKNO";
                //sht.Range("H" + row).Value = "PRICE";

                //sht.Range("O" + row + ":AA" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A" + row + ":H" + row).Style.Font.SetBold();
                using (var a = sht.Range("A" + row + ":H" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //******
                row = row + 1;
                int Rowfrom = 0;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (Rowfrom == 0)
                    {
                        Rowfrom = row;
                    }
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ShapeName"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["SizeFt"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["MSStockNo"]);
                    //sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Price"]);

                    using (var a = sht.Range("A" + row + ":H" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    row = row + 1;
                }

                row = row + 1;
                ds.Dispose();

                ////*************GRAND TOTAL
                //sht.Range("N" + row + ":U" + row).Style.Font.SetBold();
                //sht.Range("N" + row).Value = "Grand Total";
                //sht.Range("O" + row).SetValue(TBillQty);
                //sht.Range("Q" + row).SetValue(TQty);
                //sht.Range("R" + row).SetValue(TBillAmt);
                //sht.Range("S" + row).SetValue(TAmount);
                //sht.Range("T" + row).SetValue(TTransportAmt);
                //sht.Range("U" + row).SetValue(TUnloadingAmt);
                ////*************
                sht.Columns(1, 30).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("DirectMapTraceStockReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "purchaserec", "alert('No Record Found!');", true);
            }
        //}
    }
    protected void BtnExcelPreview_Click(object sender, EventArgs e)
    {
        DirectFinishedStockExcelReport();
    }
    protected void ChkForInchSize_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForInchSize.Checked == true)
        {
            ChkMtr.Checked = false;
        }
        if (ChkForInchSize.Checked == true)
        {
            //if (variable.VarNewQualitySize == "1")
            //{
            //    UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,MtrSize from QualitySizeNew where Shapeid=" + ddshape.SelectedValue, true, "select size");
            //}
            //else
            //{
                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeInch from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"], true, "select size");
            //}
        }
    }
}