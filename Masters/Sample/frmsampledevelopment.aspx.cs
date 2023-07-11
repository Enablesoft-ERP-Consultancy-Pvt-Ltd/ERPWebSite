using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
public partial class Masters_Sample_frmsampledevelopment : System.Web.UI.Page
{
    static int DataGridDeleteID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Month_Id,Month_Name from MonthTable(Nolock) order by Month_Id
                         select Year,Year from Yeardata(Nolock)
                         select Distinct IC.CATEGORY_ID,ic.CATEGORY_NAME from ITEM_CATEGORY_MASTER IC(Nolock) inner join CategorySeparate cs(Nolock) on IC.CATEGORY_ID=cs.Categoryid  and cs.id=0
                         and ic.MasterCompanyid=" + Session["varcompanyid"];
            if (Convert.ToInt32(Session["varcompanyid"]) == 44)
            {
                str = str + @" select Distinct IC.CATEGORY_ID,ic.CATEGORY_NAME from ITEM_CATEGORY_MASTER IC(Nolock) inner join CategorySeparate cs(Nolock) on IC.CATEGORY_ID=cs.Categoryid 
                         and ic.MasterCompanyid=" + Session["varcompanyid"];
            }
            else
            {
                str = str + @" select Distinct IC.CATEGORY_ID,ic.CATEGORY_NAME from ITEM_CATEGORY_MASTER IC(Nolock) inner join CategorySeparate cs(Nolock) on IC.CATEGORY_ID=cs.Categoryid And cs.id=1
                         and ic.MasterCompanyid=" + Session["varcompanyid"];
            }

            str = str + @" select DepartmentId,DepartmentName from Department(Nolock) Where MasterCompanyId=" + Session["varcompanyid"] + @" order by DepartmentName
            select CalID,CalType from Process_CalType(Nolock) order by caltype
            Select Process_Name_ID, Process_Name From PROCESS_NAME_MASTER(Nolock) Where   IsNull(AddProcessName, 0) <> 0 And MasterCompanyID = " + Session["varcompanyid"] + " Order By PROCESS_NAME ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            DataTable dtp = new DataTable();
            dtp = ds.Tables[6].AsEnumerable().Where(a => a.Field<int>("Process_Name_ID") == 9).CopyToDataTable();
            UtilityModule.ConditionalComboFillWithDS(ref DDMonth, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDyear, ds, 1, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddSplitCatagory, ds, 2, true, "--Plz Select--");


            if (ddCatagory.Items.Count > 0)
            {
                ddCatagory.SelectedIndex = 1;
                ddSplitCatagory.SelectedIndex = 1;
                ddCatagory_SelectedIndexChanged(ddCatagory, e);
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDRCategory, ds, 3, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDDept, ds, 4, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDDyeingtype, ds, 5, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDRProcessName, ds, 6, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddlpreviousprocessname, dtp, 6, true, "");
            if (DDRProcessName.Items.Count > 0)
            {
                DDRProcessName.SelectedValue = "5";
            }
            
            DDMonth.SelectedValue = DateTime.Now.Month.ToString();
            DDyear.SelectedValue = DateTime.Now.Year.ToString();
            ds.Dispose();
            tbsample.ActiveTabIndex = 0;
            //**************
            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
            }
            
            TxtDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");


            if (Session["varcompanyid"].ToString() == "44")
            {
                ChkForWeavingConsumptionSave.Text = "For Save Cutting Consumption";
                lblpreviousprocessname.Visible = true;
                ddlpreviousprocessname.Visible = true;
                if (DDDyeingtype.Items.Count > 0)
                {
                    DDDyeingtype.SelectedValue = "7";
                }

                if (DDRCalType.Items.Count > 0)
                {
                    DDRCalType.SelectedValue = "1";
                }
                if (TDRShade.Visible)
                {
                    if (ddRlshade.Items.Count > 0)
                    {
                        if (ddRlshade.Items.FindByValue("102") != null)
                        {
                            ddRlshade.SelectedValue = "102";
                        }
                    }
                }
                
                int UID = 0, devcom = 0, prodcom = 0;
                if (Session["varuserid"] != null)
                {
                    if (Session["usertype"].ToString() != "1")
                    {
                        UID = Convert.ToInt16(Session["varuserid"]);
                        string Str = @" Select isnull(canseeDevelopmentcons,0) as canseeDevelopmentcons,isnull(canseeProductioncons,0) as canseeProductioncons from NewUserDetail where userid=" + UID;

                        DataSet dsuser = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

                        if (dsuser != null)
                        {
                            if (dsuser.Tables.Count > 0)
                            {
                                if (dsuser.Tables[0].Rows.Count > 0)
                                {
                                    devcom = Convert.ToInt16(dsuser.Tables[0].Rows[0]["canseeDevelopmentcons"]);
                                    prodcom = Convert.ToInt16(dsuser.Tables[0].Rows[0]["canseeProductioncons"]);

                                    if (devcom > 0)
                                    {
                                        txtanticipatedwt.Enabled = true;
                                        txtprodwt.Enabled = false;
                                    }
                                    else if (prodcom > 0)
                                    {
                                        txtanticipatedwt.Enabled = false;
                                        txtprodwt.Enabled = true;
                                    }
                                    else
                                    {
                                        txtanticipatedwt.Enabled = true;
                                        txtprodwt.Enabled = false;
                                    }

                                }
                            }
                        }
                    }
                }
            }

        }
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select Item_Id,ITEM_NAME from Item_master Where CATEGORY_ID=" + ddCatagory.SelectedValue + " order by ITEM_NAME";
        if (chkedit.Checked == true)
        {

        }
        UtilityModule.ConditionalComboFill(ref dditemname, str, true, "--Plz Select--");
        Fillcombo();
        ddSplitCatagory.SelectedValue = ddCatagory.SelectedValue;
        SplitCategorySelectedIndexChanged();
    }
    protected void Fillcombo()
    {
        TDQuality.Visible = false;
        TDShape.Visible = false;
        TDSize.Visible = false;
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
                    case "4":
                        TDShape.Visible = true;
                        break;
                    case "5":
                        TDSize.Visible = true;
                        UtilityModule.ConditionalComboFill(ref DDsizetype, "select val,Type from SizeType Order by val", false, "");
                        break;
                }
            }
        }
    }
    protected void Fillsize()
    {
        string str = null, size = null;
        switch (DDsizetype.SelectedValue.ToString())
        {
            case "0":
                size = "SizeFt";
                break;
            case "1":
                size = "Sizemtr";
                break;
            case "2":
                size = "Sizeinch";
                break;
            default:
                size = "SizeFt";
                break;
        }

        if (Session["varcompanyid"].ToString() == "44")
        {
            switch (DDsizetype.SelectedValue.ToString())
            {
                case "0":
                    str = "Select Distinct S.sizeid,cast(s.WidthFt as varchar)+'x'+cast(s.LengthFt as varchar) +case when s.Heightft>0 then 'x'+cast(s.HeightFt as varchar) else ''  end as  SizeFt from Size S  Where S.shapeid=" + ddshape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
                    break;
                case "1":
                    str = " Select Distinct S.sizeid,cast(s.WidthMtr as varchar)+'x'+cast(s.LengthMtr as varchar) +case when s.HeightMtr>0 then 'x'+cast(s.HeightMtr as varchar) else ''  end as  Sizemtr from Size S Where S.shapeid=" + ddshape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];

                    break;
                case "2":
                    str = "Select Distinct S.sizeid,cast(s.WidthInch as varchar)+'x'+cast(s.LengthInch as varchar) +case when s.HeightInch>0 then 'x'+cast(s.HeightInch as varchar) else ''  end as  Sizeinch from Size S  Where S.shapeid=" + ddshape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
                    break;
                default:
                    str = "Select Distinct S.sizeid,cast(s.WidthFt as varchar)+'x'+cast(s.LengthFt as varchar) +case when s.Heightft>0 then 'x'+cast(s.HeightFt as varchar) else ''  end as  SizeFt from Size S  Where S.shapeid=" + ddshape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
                    break;
            }
        }
        else
        {
            str = "Select Distinct S.sizeid, S." + size + " from Size S  Where S.shapeid=" + ddshape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
        }

        if (Session["varcompanyId"].ToString() == "16" || Session["varcompanyId"].ToString() == "28")
        {
            str = str + " order by S." + size + ", S.sizeid";
        }
        else if (Session["varcompanyId"].ToString() == "44")
        {
            str = str + " order by 1";
        }
        else
        {
            str = str + " order by S." + size;
        }
        UtilityModule.ConditionalComboFill(ref ddsize, str, true, "--Select--");

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
        //Shape
        if (TDShape.Visible == true)
        {

            str = "select Distinct Sh.shapeid,Sh.shapename from Shape Sh  order by shapename";
            UtilityModule.ConditionalComboFill(ref ddshape, str, true, "--Select--");
            if (ddshape.Items.Count > 0)
            {
                ddshape.SelectedIndex = 1;
                ddshape_SelectedIndexChanged(ddshape, new EventArgs());
            }
        }
        //Design And Color 

        str = @"Select DesignID, DesignName From Design(Nolock) Where MasterCompanyid = " + Session["varcompanyid"] + @" Order By DesignName 
                Select ColorID, ColorName From Color(Nolock) Where MasterCompanyid = " + Session["varcompanyid"] + " Order By ColorName ";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            UtilityModule.ConditionalComboFillWithDS(ref DDDesignName, ds, 0, true, "Select Design");
            UtilityModule.ConditionalComboFillWithDS(ref DDColorName, ds, 1, true, "Select Color");
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQDCS();
        ddSplitItemname.SelectedValue = dditemname.SelectedValue;
        FillSplitQDCS();
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
        FillSplitSize();
    }
    protected void DDpurpose_SelectedIndexChanged(object sender, EventArgs e)
    {
        TDbuyer.Visible = false;
        TDgeneral.Visible = true;
        switch (DDpurpose.SelectedIndex)
        {
            case 0:
                break;
            case 1:
                TDbuyer.Visible = true;
                if (Session["varcompanyid"].ToString() == "44")
                {
                    UtilityModule.ConditionalComboFill(ref DDbuyer, "select customerid,CustomerCode as Customer from customerinfo  order by customer", true, "--Plz Select--");
                }
                else { UtilityModule.ConditionalComboFill(ref DDbuyer, "select customerid,CustomerCode+'/'+CompanyName as Customer from customerinfo  order by customer", true, "--Plz Select--"); }
                if (DDbuyer.Items.Count > 0)
                {
                    DDbuyer.SelectedIndex = 0;
                }
                TDgeneral.Visible = false;
                break;
        }

    }
    protected void DDRCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select Item_Id,ITEM_NAME from Item_master Where CATEGORY_ID=" + DDRCategory.SelectedValue + " order by ITEM_NAME";
        UtilityModule.ConditionalComboFill(ref DDRitemname, str, true, "--Plz Select--");
        FillComboRaw();
    }
    protected void FillComboRaw()
    {
        TDRQuality.Visible = false;
        TDRDesign.Visible = false;
        TDRColor.Visible = false;
        TDRShape.Visible = false;
        TDRSize.Visible = false;
        if (Session["varcompanyid"].ToString() == "44")
        {
            if (DDRCategory.SelectedIndex > 0)
            {
                if (Convert.ToInt16(DDRCategory.SelectedValue) == 2)
                {
                    TDRShade.Visible = false;
                    TDRINPUTSHADECOLOR.Visible = true;
                    TDROUTPUTSHADECOLOR.Visible = true;

                }
                else
                {
                   // TDRShade.Visible = true;
                    TDRINPUTSHADECOLOR.Visible = false;
                    TDROUTPUTSHADECOLOR.Visible = false;
                }

            }
            else
            {
               // TDRShade.Visible = true;
                TDRINPUTSHADECOLOR.Visible = false;
                TDROUTPUTSHADECOLOR.Visible = false;
            }

        }
        //else
        //{

        //    TDRShade.Visible = true;
        //}
      

        string str = @"select PARAMETER_ID from ITEM_CATEGORY_PARAMETERS where category_id=" + DDRCategory.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["Parameter_Id"].ToString())
                {
                    case "1":
                        TDRQuality.Visible = true;
                        break;
                    case "2":
                        TDRDesign.Visible = true;
                        break;
                    case "3":
                        TDRColor.Visible = true;
                        break;
                    case "4":
                        TDRShape.Visible = true;
                        break;
                    case "5":
                        TDRSize.Visible = true;
                        UtilityModule.ConditionalComboFill(ref DDRSizetype, "select val,Type from SizeType Order by val", false, "");
                        break;
                    case "6":
                        if (Session["varcompanyid"].ToString() == "44")
                        {
                            if (DDRCategory.SelectedIndex > 0)
                            {
                                if (Convert.ToInt16(DDRCategory.SelectedValue) == 2)
                                {
                                    TDRShade.Visible = false;
                                    TDRINPUTSHADECOLOR.Visible = true;
                                    TDROUTPUTSHADECOLOR.Visible = true;

                                }
                                //else
                                //{
                                //    TDRShade.Visible = true;
                                //    TDRINPUTSHADECOLOR.Visible = false;
                                //    TDROUTPUTSHADECOLOR.Visible = false;
                                //}

                            }
                            //else
                            //{
                            //    TDRShade.Visible = true;
                            //    TDRINPUTSHADECOLOR.Visible = false;
                            //    TDROUTPUTSHADECOLOR.Visible = false;
                            //}

                        }
                        else
                        {
                            TDRShade.Visible = true;
                        }
                        //TDRShade.Visible = true;
                        break;
                }
            }
        }
    }
    protected void DDRitemname_SelectedIndexChanged(object sender, EventArgs e)
    {

        FillQDCSRaw();
    }
    protected void FillQDCSRaw()
    {
        string str = null;
        //Quality
       
        if (TDRQuality.Visible == true)
        {

            str = "select Distinct QualityId,QualityName from Quality Where Item_Id=" + DDRitemname.SelectedValue + " order by QualityName";
            UtilityModule.ConditionalComboFill(ref DDRquality, str, true, "--Select--");

        }
        //Design
        if (TDRDesign.Visible == true)
        {
            str = "select designId,designName from Design Where MasterCompanyid=" + Session["varcompanyid"] + " order by designName";
            UtilityModule.ConditionalComboFill(ref ddRdesign, str, true, "--Select--");
        }
        //Color
        if (TDRColor.Visible == true)
        {
            str = "select ColorId,ColorName from color Where MasterCompanyId=" + Session["varcompanyid"] + " order by ColorName";
            UtilityModule.ConditionalComboFill(ref ddRcolor, str, true, "--Select--");

        }
        //Shape
        if (TDRShape.Visible == true)
        {

            str = "select Distinct Sh.shapeid,Sh.shapename from Shape Sh  order by shapename";
            UtilityModule.ConditionalComboFill(ref DDRshape, str, true, "--Select--");
            if (DDRshape.Items.Count > 0)
            {
                DDRshape.SelectedIndex = 1;
                DDRshape_SelectedIndexChanged(ddshape, new EventArgs());
            }

        }
        //Shade
        if (TDRShade.Visible == true)
        {
            str = "select shadecolorid,shadecolorname from shadecolor   order by shadecolorname";
            UtilityModule.ConditionalComboFill(ref ddRlshade, str, true, "--Select--");
        }
        if (TDRINPUTSHADECOLOR.Visible == true)
        {
            str = "select shadecolorid,shadecolorname from shadecolor   order by shadecolorname";
            UtilityModule.ConditionalComboFill(ref ddRloutputshadecolor, str, true, "--Select--");
        }
        if (TDROUTPUTSHADECOLOR.Visible == true)
        {
            str = "select shadecolorid,shadecolorname from shadecolor   order by shadecolorname";
            UtilityModule.ConditionalComboFill(ref ddRlinputshadecolor, str, true, "--Select--");
        }
        if (TDRShade.Visible)
        {
            if (ddRlshade.Items.Count > 0)
            {
                if (ddRlshade.Items.FindByValue("8") != null)
                {
                    ddRlshade.SelectedValue = "8";
                }
            }
        }
        //Unit
        UtilityModule.ConditionalComboFill(ref DDUnit, @"SELECT distinct u.UnitId, u.UnitName 
        FROM ITEM_MASTER i(Nolock) 
        JOIN  Unit u(Nolock) ON i.UnitTypeID = u.UnitTypeID 
        Where item_id = " + DDRitemname.SelectedValue + @" And i.MasterCompanyId = " + Session["varCompanyId"] + " Order By u.UnitName ", true, "Select Unit");
    }

    protected void FillsizeRaw()
    {

        string str = null, size = null;
        switch (DDRSizetype.SelectedValue.ToString())
        {
            case "0":
                size = "SizeFt";
                break;
            case "1":
                size = "Sizemtr";
                break;
            case "2":
                size = "Sizeinch";
                break;
            default:
                size = "SizeFt";
                break;

        }

        str = "select Distinct S.sizeid,S." + size + " from Size S Where S.shapeid=" + DDRshape.SelectedValue;

        if (Session["varcompanyId"].ToString() == "16" || Session["varcompanyId"].ToString() == "28")
        {
            str = str + " order by S." + size + ", S.sizeid";
        }
        else
        {
            str = str + " order by S." + size;
        }
        UtilityModule.ConditionalComboFill(ref DDRsize, str, true, "--Select--");

    }
    protected void DDRshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillsizeRaw();
    }
    protected void DDRSizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillsizeRaw();
    }
    protected void DDDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select EmpId,EmpName from EmpInfo Where Departmentid=" + DDDept.SelectedValue + " and MasterCompanyid=" + Session["varcompanyid"] + " order by EmpName";
        UtilityModule.NewChkBoxListFill(ref chkemp, str);
    }
    protected void btnaddmaterial_Click(object sender, EventArgs e)
    {
        lblDeleteButtonCall.Text = "2";
        if (Session["varcompanyid"].ToString() == "44")
        {
            if (CheckValidationRawMaterial() == false)
            {
                return;
            }

        }
        if (chkedit.Checked == true)
        {
            Popup(true);
            txtpwd.Focus();
            lblmsg.Text = "";
        }
        else
        {
            AddMaterialButtonClick();
        }
    }
    protected void AddMaterialButtonClick()
    {
        if (ViewState["rawtable"] == null)
        {
            setinitialrowForRawmaterial();
        }
        else
        {
            AddnewrowtoRawgrid();
        }
        txtanticipatedwt.Text = "";
    }
    protected void setinitialrowForRawmaterial()
    {
        DataTable dt = new DataTable();
        DataRow DR = null;
        dt.Columns.Add("Itemdescription", typeof(string));
        dt.Columns.Add("Item_Finished_id", typeof(int));
        if (Session["varcompanyid"].ToString() == "44")
        {
            dt.Columns.Add("OSHADEID", typeof(int));
        }
        dt.Columns.Add("LotNo", typeof(string));
        dt.Columns.Add("DyeingType", typeof(string));
        dt.Columns.Add("UnitName", typeof(string));
        dt.Columns.Add("AnticipatedWt", typeof(float));
        dt.Columns.Add("ActualWt", typeof(float));
        dt.Columns.Add("Vendorname", typeof(string));
        dt.Columns.Add("ProcessID", typeof(int));
        dt.Columns.Add("CalType", typeof(int));        
        dt.Columns.Add("UnitID", typeof(int));

        DR = dt.NewRow();
        DR["Itemdescription"] = getitemdescription();
        if (Session["varcompanyid"].ToString() == "44")
        {
            if (DDRCategory.SelectedIndex > 0)
            {
                if (Convert.ToInt16(DDRCategory.SelectedValue) == 2)
                {
                    DR["Item_Finished_id"] = UtilityModule.getItemFinishedId(DDRitemname, DDRquality, ddRdesign, ddRcolor, DDRshape, DDRsize, TxtProdCode, ddRlinputshadecolor, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                    DR["OSHADEID"] = ddRloutputshadecolor.SelectedValue;
                }
                else
                {
                    DR["Item_Finished_id"] = UtilityModule.getItemFinishedId(DDRitemname, DDRquality, ddRdesign, ddRcolor, DDRshape, DDRsize, TxtProdCode, ddRlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                    //DR["O_Item_Finished_id"] = UtilityModule.getItemFinishedId(DDRitemname, DDRquality, ddRdesign, ddRcolor, DDRshape, DDRsize, TxtProdCode, ddRlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                
                }
            }
            else
            {
                DR["Item_Finished_id"] = UtilityModule.getItemFinishedId(DDRitemname, DDRquality, ddRdesign, ddRcolor, DDRshape, DDRsize, TxtProdCode, ddRlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                //DR["O_Item_Finished_id"] = UtilityModule.getItemFinishedId(DDRitemname, DDRquality, ddRdesign, ddRcolor, DDRshape, DDRsize, TxtProdCode, ddRlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));

            }
        }
        else
        {

            DR["Item_Finished_id"] = UtilityModule.getItemFinishedId(DDRitemname, DDRquality, ddRdesign, ddRcolor, DDRshape, DDRsize, TxtProdCode, ddRlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            //DR["O_Item_Finished_id"] = UtilityModule.getItemFinishedId(DDRitemname, DDRquality, ddRdesign, ddRcolor, DDRshape, DDRsize, TxtProdCode, ddRlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        }
        DR["Lotno"] = DDLotno.SelectedIndex > 0 ? DDLotno.SelectedValue : "";
        DR["Dyeingtype"] = DDDyeingtype.SelectedIndex > 0 ? DDDyeingtype.SelectedItem.Text : "";
        DR["UnitName"] = DDUnit.SelectedIndex > 0 ? DDUnit.SelectedItem.Text : "";
        DR["anticipatedwt"] = txtanticipatedwt.Text == "" ? "0" : txtanticipatedwt.Text;
        DR["vendorname"] = DDLotno.SelectedIndex > 0 ? lblvendorname.Text : "";
        DR["ProcessID"] = DDRProcessName.SelectedValue;
        DR["CalType"] = DDRCalType.SelectedValue;
        DR["UnitID"] = DDUnit.SelectedValue;

        dt.Rows.Add(DR);
        ViewState["rawtable"] = dt;
        DGraw.DataSource = dt;
        DGraw.DataBind();
    }
    protected void setinitialrowForvendorDetail()
    {
        DataTable dt = new DataTable();
        DataRow DR = null;
        dt.Columns.Add("Department", typeof(string));
        dt.Columns.Add("vendor", typeof(string));
        dt.Columns.Add("Departmentid", typeof(int));
        dt.Columns.Add("empid", typeof(int));
        for (int i = 0; i < chkemp.Items.Count; i++)
        {
            if (chkemp.Items[i].Selected == true)
            {

                DR = dt.NewRow();
                DR["Department"] = DDDept.SelectedItem.Text;
                DR["vendor"] = chkemp.Items[i].Text;
                DR["Departmentid"] = DDDept.SelectedValue;
                DR["empid"] = chkemp.Items[i].Value;
                dt.Rows.Add(DR);
            }
        }

        ViewState["vendortable"] = dt;
        DGvendor.DataSource = dt;
        DGvendor.DataBind();

    }
    protected string getitemdescription()
    {
        string Itemdescription = "";
        StringBuilder sb = new StringBuilder();
        if (DDRCategory.SelectedIndex > 0)
        {
            sb.Append(DDRCategory.SelectedItem.Text + ",");
        }
        if (DDRitemname.SelectedIndex > 0)
        {
            sb.Append(DDRitemname.SelectedItem.Text + ",");
        }
        if (TDRQuality.Visible == true)
        {
            if (DDRquality.SelectedIndex > 0)
            {
                sb.Append(DDRquality.SelectedItem.Text + ",");
            }
        }
        if (TDRDesign.Visible == true)
        {
            if (ddRdesign.SelectedIndex > 0)
            {
                sb.Append(ddRdesign.SelectedItem.Text + ",");
            }
        }
        if (TDRColor.Visible == true)
        {
            if (ddRcolor.SelectedIndex > 0)
            {
                sb.Append(ddRcolor.SelectedItem.Text + ",");
            }
        }
        if (TDRShape.Visible == true)
        {
            if (DDRshape.SelectedIndex > 0)
            {
                sb.Append(DDRshape.SelectedItem.Text + ",");
            }
        }
        if (TDRSize.Visible == true)
        {
            if (DDRsize.SelectedIndex > 0)
            {
                sb.Append(DDRsize.SelectedItem.Text + ",");
            }
        }
        if (TDRShade.Visible == true)
        {
            if (ddRlshade.SelectedIndex > 0)
            {
                sb.Append(ddRlshade.SelectedItem.Text + ",");
            }
        }
        if (TDRINPUTSHADECOLOR.Visible == true)
        {
            if (ddRlinputshadecolor.SelectedIndex > 0)
            {
                sb.Append("Inputshadecolor:-");
                sb.Append(ddRlinputshadecolor.SelectedItem.Text + ",");
            }
        }
        if (TDROUTPUTSHADECOLOR.Visible == true)
        {
            if (ddRloutputshadecolor.SelectedIndex > 0)
            {
                sb.Append("Outputshadecolor:-");
                sb.Append(ddRloutputshadecolor.SelectedItem.Text + ",");
            }
        }
        sb.Append("  (" + DDRProcessName.SelectedItem.Text + " " + DDRCalType.SelectedItem.Text + ")" + ",");

        Itemdescription = sb.ToString().TrimEnd(',');
        return Itemdescription;
    }
    protected void AddnewrowtoRawgrid()
    {
        if (ViewState["rawtable"] != null)
        {
            DataTable dtcurrentTable = (DataTable)ViewState["rawtable"];
            DataRow drcurrentRow = null;
            drcurrentRow = dtcurrentTable.NewRow();
            drcurrentRow["Itemdescription"] = getitemdescription();


            if (Session["varcompanyid"].ToString() == "44")
            {
                if (DDRCategory.SelectedIndex > 0)
                {
                    if (Convert.ToInt16(DDRCategory.SelectedValue) == 2)
                    {
                        drcurrentRow["Item_Finished_id"] = UtilityModule.getItemFinishedId(DDRitemname, DDRquality, ddRdesign, ddRcolor, DDRshape, DDRsize, TxtProdCode, ddRlinputshadecolor, 0, "", Convert.ToInt32(Session["varCompanyId"]));

                        drcurrentRow["OSHADEID"] = ddRloutputshadecolor.SelectedValue;
                    }
                    else
                    {
                        drcurrentRow["Item_Finished_id"] = UtilityModule.getItemFinishedId(DDRitemname, DDRquality, ddRdesign, ddRcolor, DDRshape, DDRsize, TxtProdCode, ddRlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                        //drcurrentRow["O_Item_Finished_id"] = UtilityModule.getItemFinishedId(DDRitemname, DDRquality, ddRdesign, ddRcolor, DDRshape, DDRsize, TxtProdCode, ddRlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));

                    }
                }
                else
                {
                    drcurrentRow["Item_Finished_id"] = UtilityModule.getItemFinishedId(DDRitemname, DDRquality, ddRdesign, ddRcolor, DDRshape, DDRsize, TxtProdCode, ddRlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                    //drcurrentRow["O_Item_Finished_id"] = UtilityModule.getItemFinishedId(DDRitemname, DDRquality, ddRdesign, ddRcolor, DDRshape, DDRsize, TxtProdCode, ddRlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));

                }
            }
            else
            {

                drcurrentRow["Item_Finished_id"] = UtilityModule.getItemFinishedId(DDRitemname, DDRquality, ddRdesign, ddRcolor, DDRshape, DDRsize, TxtProdCode, ddRlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                //drcurrentRow["O_Item_Finished_id"] = UtilityModule.getItemFinishedId(DDRitemname, DDRquality, ddRdesign, ddRcolor, DDRshape, DDRsize, TxtProdCode, ddRlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            }
            //drcurrentRow["Item_Finished_id"] = UtilityModule.getItemFinishedId(DDRitemname, DDRquality, ddRdesign, ddRcolor, DDRshape, DDRsize, TxtProdCode, ddRlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            drcurrentRow["LotNo"] = DDLotno.SelectedIndex > 0 ? DDLotno.SelectedValue : "";
            drcurrentRow["Dyeingtype"] = DDDyeingtype.SelectedIndex > 0 ? DDDyeingtype.SelectedItem.Text : "";
            drcurrentRow["UnitName"] = DDUnit.SelectedIndex > 0 ? DDUnit.SelectedItem.Text : "";
            drcurrentRow["anticipatedwt"] = txtanticipatedwt.Text == "" ? "0" : txtanticipatedwt.Text;
            drcurrentRow["actualwt"] = txtprodwt.Text == "" ? "0" : txtprodwt.Text;
            drcurrentRow["vendorname"] = DDLotno.SelectedIndex > 0 ? lblvendorname.Text : "";
            drcurrentRow["ProcessID"] = DDRProcessName.SelectedValue;
            drcurrentRow["CalType"] = DDRCalType.SelectedValue;
            drcurrentRow["UnitID"] = DDUnit.SelectedValue;

            dtcurrentTable.Rows.Add(drcurrentRow);
            ViewState["rawtable"] = dtcurrentTable;
            DGraw.DataSource = dtcurrentTable;
            DGraw.DataBind();
        }
    }
    protected void AddnewrowtovendorGrid()
    {
        if (ViewState["vendortable"] != null)
        {
            DataTable dtcurrentTable = (DataTable)ViewState["vendortable"];
            DataRow drcurrentRow = null;
            for (int i = 0; i < chkemp.Items.Count; i++)
            {
                if (chkemp.Items[i].Selected == true)
                {
                    drcurrentRow = dtcurrentTable.NewRow();
                    drcurrentRow["Department"] = DDDept.SelectedItem.Text;
                    drcurrentRow["vendor"] = chkemp.Items[i].Text;
                    drcurrentRow["Departmentid"] = DDDept.SelectedValue;
                    drcurrentRow["empid"] = chkemp.Items[i].Value;
                    dtcurrentTable.Rows.Add(drcurrentRow);
                }
            }
            ViewState["vendortable"] = dtcurrentTable;
            DGvendor.DataSource = dtcurrentTable;
            DGvendor.DataBind();
        }
    }
    protected void btnaddvendor_Click(object sender, EventArgs e)
    {
        if (ViewState["vendortable"] == null)
        {
            setinitialrowForvendorDetail();
        }
        else
        {
            AddnewrowtovendorGrid();
        }

        DGvendor.UseAccessibleHeader = true;
        DGvendor.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        ClickOK();
        lblmsg.Text = "";
        int VarWeavingConsumptionSaveFlag = 0;
        //************Raw Material Description
        DataTable dtraw = new DataTable();
        dtraw.Columns.Add("Ifinishedid", typeof(int));
        if (Session["varcompanyid"].ToString() == "44")
        {
            dtraw.Columns.Add("OSHADEID", typeof(int));
        }
        dtraw.Columns.Add("Anticipatedwt", typeof(float));
        dtraw.Columns.Add("Actualwt", typeof(float));
        dtraw.Columns.Add("Lotno", typeof(string));
        dtraw.Columns.Add("DyeingType", typeof(string));
        dtraw.Columns.Add("vendorname", typeof(string));
        dtraw.Columns.Add("ProcessID", typeof(string));
        dtraw.Columns.Add("CalType", typeof(string));
        dtraw.Columns.Add("UnitID", typeof(string));

        for (int i = 0; i < DGraw.Rows.Count; i++)
        {
            if (Session["varcompanyid"].ToString() == "44")
            {
                if (ddlpreviousprocessname.SelectedValue =="9")
                {
                    CheckBox CHKITEM = (CheckBox)DGraw.Rows[i].FindControl("Chkboxitem");
                    if (CHKITEM.Checked)
                    {
                        Label lblitemfinishedid = (Label)DGraw.Rows[i].FindControl("lblitemfinishedid");
                        Label lblOitemfinishedid = (Label)DGraw.Rows[i].FindControl("lblOitemfinishedid");
                        TextBox lblanticipatedwt = (TextBox)DGraw.Rows[i].FindControl("lblanticipatedwt");
                        TextBox txtactualwt = (TextBox)DGraw.Rows[i].FindControl("txtactualwt");
                        Label lbllotno = (Label)DGraw.Rows[i].FindControl("lbllotno");
                        Label lbldyeingtype = (Label)DGraw.Rows[i].FindControl("lbldyeingtype");
                        Label lblvendornamegrid = (Label)DGraw.Rows[i].FindControl("lblvendornamegrid");
                        Label lblProcessID = (Label)DGraw.Rows[i].FindControl("lblProcessID");
                        Label lblCalType = (Label)DGraw.Rows[i].FindControl("lblCalType");
                        Label lblUnitID = (Label)DGraw.Rows[i].FindControl("lblUnitID");

                        DataRow dr = dtraw.NewRow();
                        dr["Ifinishedid"] = lblitemfinishedid.Text;
                        dr["OSHADEID"] = string.IsNullOrEmpty(lblOitemfinishedid.Text) ? "0" : lblOitemfinishedid.Text;
                        dr["Anticipatedwt"] = lblanticipatedwt.Text == "" ? "0" : lblanticipatedwt.Text;
                        dr["Actualwt"] = txtactualwt.Text == "" ? "0" : txtactualwt.Text;
                        dr["Lotno"] = lbllotno.Text;
                        dr["Dyeingtype"] = lbldyeingtype.Text;
                        dr["vendorname"] = lblvendornamegrid.Text;
                        if (ddlpreviousprocessname.Visible)
                        {
                            dr["ProcessID"] = ddlpreviousprocessname.SelectedValue == "9" ? DDRProcessName.SelectedValue : lblProcessID.Text;
                        }
                        else
                        {
                            dr["ProcessID"] = lblProcessID.Text;
                        }
                        dr["CalType"] = lblCalType.Text;
                        dr["UnitID"] = lblUnitID.Text;

                        dtraw.Rows.Add(dr);
                    }
                   
                }
                else
                {

                    Label lblitemfinishedid = (Label)DGraw.Rows[i].FindControl("lblitemfinishedid");
                    Label lblOitemfinishedid = (Label)DGraw.Rows[i].FindControl("lblOitemfinishedid");
                    TextBox lblanticipatedwt = (TextBox)DGraw.Rows[i].FindControl("lblanticipatedwt");
                    TextBox txtactualwt = (TextBox)DGraw.Rows[i].FindControl("txtactualwt");
                    Label lbllotno = (Label)DGraw.Rows[i].FindControl("lbllotno");
                    Label lbldyeingtype = (Label)DGraw.Rows[i].FindControl("lbldyeingtype");
                    Label lblvendornamegrid = (Label)DGraw.Rows[i].FindControl("lblvendornamegrid");
                    Label lblProcessID = (Label)DGraw.Rows[i].FindControl("lblProcessID");
                    Label lblCalType = (Label)DGraw.Rows[i].FindControl("lblCalType");
                    Label lblUnitID = (Label)DGraw.Rows[i].FindControl("lblUnitID");

                    DataRow dr = dtraw.NewRow();
                    dr["Ifinishedid"] = lblitemfinishedid.Text;
                    dr["OSHADEID"] = string.IsNullOrEmpty(lblOitemfinishedid.Text) ? "0" : lblOitemfinishedid.Text;
                    dr["Anticipatedwt"] = lblanticipatedwt.Text == "" ? "0" : lblanticipatedwt.Text;
                    dr["Actualwt"] = txtactualwt.Text == "" ? "0" : txtactualwt.Text;
                    dr["Lotno"] = lbllotno.Text;
                    dr["Dyeingtype"] = lbldyeingtype.Text;
                    dr["vendorname"] = lblvendornamegrid.Text;
                    if (ddlpreviousprocessname.Visible)
                    {
                        dr["ProcessID"] = ddlpreviousprocessname.SelectedValue=="9" ? DDRProcessName.SelectedValue : lblProcessID.Text;
                    }
                    else
                    {
                        dr["ProcessID"] = lblProcessID.Text;
                    }
                    dr["CalType"] = lblCalType.Text;
                    dr["UnitID"] = lblUnitID.Text;

                    dtraw.Rows.Add(dr);


                }
            }
            else
            {

                Label lblitemfinishedid = (Label)DGraw.Rows[i].FindControl("lblitemfinishedid");

              //  Label lblOitemfinishedid = (Label)DGraw.Rows[i].FindControl("lblOitemfinishedid");
                TextBox lblanticipatedwt = (TextBox)DGraw.Rows[i].FindControl("lblanticipatedwt");
                TextBox txtactualwt = (TextBox)DGraw.Rows[i].FindControl("txtactualwt");
                Label lbllotno = (Label)DGraw.Rows[i].FindControl("lbllotno");
                Label lbldyeingtype = (Label)DGraw.Rows[i].FindControl("lbldyeingtype");
                Label lblvendornamegrid = (Label)DGraw.Rows[i].FindControl("lblvendornamegrid");
                Label lblProcessID = (Label)DGraw.Rows[i].FindControl("lblProcessID");
                Label lblCalType = (Label)DGraw.Rows[i].FindControl("lblCalType");
                Label lblUnitID = (Label)DGraw.Rows[i].FindControl("lblUnitID");

                DataRow dr = dtraw.NewRow();
                dr["Ifinishedid"] = lblitemfinishedid.Text;
             //   dr["OSHADEID"] = string.IsNullOrEmpty(lblOitemfinishedid.Text) ? "0" : lblOitemfinishedid.Text;
                dr["Anticipatedwt"] = lblanticipatedwt.Text == "" ? "0" : lblanticipatedwt.Text;
                dr["Actualwt"] = txtactualwt.Text == "" ? "0" : txtactualwt.Text;
                dr["Lotno"] = lbllotno.Text;
                dr["Dyeingtype"] = lbldyeingtype.Text;
                dr["vendorname"] = lblvendornamegrid.Text;
                if (ddlpreviousprocessname.Visible)
                {
                    dr["ProcessID"] = ddlpreviousprocessname.SelectedValue=="9" ? DDRProcessName.SelectedValue : lblProcessID.Text;
                }
                else
                {
                    dr["ProcessID"] = lblProcessID.Text;
                }
                dr["CalType"] = lblCalType.Text;
                dr["UnitID"] = lblUnitID.Text;

                dtraw.Rows.Add(dr);


            }

        }
        //************Vendor Detail
        DataTable dtvendor = new DataTable();
        dtvendor.Columns.Add("Deptid", typeof(int));
        dtvendor.Columns.Add("empid", typeof(int));
        for (int i = 0; i < DGvendor.Rows.Count; i++)
        {
            Label lbldeptid = (Label)DGvendor.Rows[i].FindControl("lbldeptid");
            Label lblempid = (Label)DGvendor.Rows[i].FindControl("lblempid");
            DataRow dr = dtvendor.NewRow();
            dr["deptid"] = lbldeptid.Text;
            dr["empid"] = lblempid.Text;
            dtvendor.Rows.Add(dr);
        }
        
        if(chkedit .Checked ==true )
        {
            if(ChkForWeavingConsumptionSave .Checked ==true )
            {
                VarWeavingConsumptionSaveFlag = 1;
            }
        }

        //***************
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            string sp = string.Empty;
            SqlParameter[] param = new SqlParameter[27];
            param[0] = new SqlParameter("@id", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hnid.Value;
            param[1] = new SqlParameter("@Samplecode", SqlDbType.VarChar, 50);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@Monthid", DDMonth.SelectedValue);
            param[3] = new SqlParameter("@SampleYear", DDyear.SelectedValue);
            param[4] = new SqlParameter("@Purpose", DDpurpose.SelectedIndex);
            param[5] = new SqlParameter("@Customerid", TDbuyer.Visible == true ? DDbuyer.SelectedValue : "0");
            param[6] = new SqlParameter("@Purposeval", TDgeneral.Visible == true ? txtgeneral.Text : "");
            param[7] = new SqlParameter("@Categoryid", ddCatagory.SelectedValue);
            param[8] = new SqlParameter("@Itemid", dditemname.SelectedValue);
            param[9] = new SqlParameter("@QualityId", TDQuality.Visible == true ? dquality.SelectedValue : "0");
            param[10] = new SqlParameter("@DesignName", DDDesignName.SelectedItem.Text);
            param[11] = new SqlParameter("@ColorName", DDColorName.SelectedItem.Text);
            param[12] = new SqlParameter("@shapeid", TDShape.Visible == true ? ddshape.SelectedValue : "0");
            param[13] = new SqlParameter("@Sizeflag", DDsizetype.SelectedValue);
            param[14] = new SqlParameter("@Sizeid", TDSize.Visible == true ? ddsize.SelectedValue : "0");
            param[15] = new SqlParameter("@userid", Session["varuserid"]);
            param[16] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            param[17] = new SqlParameter("@Remark", txtremark.Text);
            param[18] = new SqlParameter("@Wtgsm", txtwtgsm.Text == "" ? "0" : txtwtgsm.Text);
            param[19] = new SqlParameter("@Totalgsm", txttotalgsm.Text == "" ? "0" : txttotalgsm.Text);
            param[20] = new SqlParameter("@dtraw", dtraw);
            param[21] = new SqlParameter("@dtvendor", dtvendor);
            param[22] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[22].Direction = ParameterDirection.Output;
            param[23] = new SqlParameter("@SplitFinishedID", LblSplitFinishedID.Text);
            param[24] = new SqlParameter("@WavingConsmptionSaveFlag", VarWeavingConsumptionSaveFlag);
            param[25] = new SqlParameter("@Date", TxtDate.Text);
            param[26] = new SqlParameter("@ProcID", DDRProcessName.SelectedValue);

            //if (ddlpreviousprocessname.Visible)
            //{
            //    param[26] = new SqlParameter("@ProcID", ddlpreviousprocessname.SelectedIndex > 0 ? DDRProcessName.SelectedValue : DDRProcessName.SelectedValue);
            //}
            //else
            //{
              
            //}
            

            //*********************


            if (Session["varcompanyid"].ToString() == "44")
            {
                sp = "PRO_SAVESAMPLEDEVELIPMENT_AGNI";
            }
            else
            {
                sp = "PRO_SAVESAMPLEDEVELIPMENT";
            }
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, sp, param);
            Tran.Commit();
            txtsamplecode.Text = param[1].Value.ToString();
            if (param[22].Value.ToString() != "")
            {
                lblmsg.Text = param[22].Value.ToString();
            }
            else
            {
                hnid.Value = param[0].Value.ToString();
                lblmsg.Text = "Sample code generated successfully..";


                //*****raw Description
                if (ViewState["rawtable"] != null)
                {
                    ViewState["rawtable"] = null;
                }
                //********
                if (ViewState["vendortable"] != null)
                {
                    ViewState["vendortable"] = null;
                }
                BindSplitItemGrid();
                tbsample.ActiveTabIndex = 0;
                
            }
            //**************Save Image
            SaveImage(Convert.ToInt32(param[0].Value));
            //*************
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
    protected void SaveImage(int Id)
    {
        if (PhotoImage.FileName != "")
        {
            string filename = Path.GetFileName(PhotoImage.PostedFile.FileName);
            string Folderpath = Server.MapPath("../../SampleImage");
            //Check folder
            if (!Directory.Exists(Folderpath))
            {
                Directory.CreateDirectory(Folderpath);
            }
            //
            string targetPath = Server.MapPath("../../SampleImage/" + Id + "_Sampledev.gif");

            FileInfo file = new FileInfo(targetPath);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
            }

            string img = "~\\SampleImage\\" + Id + "_Sampledev.gif";
            //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
            Stream strm = PhotoImage.PostedFile.InputStream;
            var targetFile = targetPath;
            if (PhotoImage.FileName != null && PhotoImage.FileName != "")
            {
                GenerateThumbnails(0.3, strm, targetFile);
            }
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update SampleDevelopmentMaster Set imgpath='" + img + "' Where id=" + Id + "");
            lblimage.ImageUrl = img + "?" + DateTime.Now.Ticks.ToString(); ;
        }

        if (Photocadimage.FileName != "")
        {
            string filename = Path.GetFileName(Photocadimage.PostedFile.FileName);
            string Folderpath = Server.MapPath("../../SampleImage");
            //Check folder
            if (!Directory.Exists(Folderpath))
            {
                Directory.CreateDirectory(Folderpath);
            }
            //
            string targetPath = Server.MapPath("../../SampleImage/" + Id + "_Sampledevcad.gif");
            string img = "~\\SampleImage\\" + Id + "_Sampledevcad.gif";
            //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
            Stream strm = Photocadimage.PostedFile.InputStream;
            var targetFile = targetPath;
            if (Photocadimage.FileName != null && Photocadimage.FileName != "")
            {
                GenerateThumbnails(0.3, strm, targetFile);
            }
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update SampleDevelopmentMaster Set Cadimgpath='" + img + "' Where id=" + Id + "");
            imgcadupload.ImageUrl = img + "?" + DateTime.Now.Ticks.ToString(); ;
        }
        if (fileupload.FileName != "")
        {
            if (!Directory.Exists(Server.MapPath("~/SampleCostingFile")))
            {
                Directory.CreateDirectory(Server.MapPath("~/SampleCostingFile"));
            }
            //Upload and save the file
            string csvPath = Server.MapPath("~/SampleCostingFile/" + Id + "_Costing.xlsx");
            fileupload.SaveAs(csvPath);
        }
    }
    private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
    {
        using (var image = System.Drawing.Image.FromStream(sourcePath))
        {
            var newWidth = (int)(image.Width * scaleFactor);
            var newHeight = (int)(image.Height * scaleFactor);
            var thumbnailImg = new Bitmap(newWidth, newHeight);
            var thumbGraph = Graphics.FromImage(thumbnailImg);
            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbGraph.DrawImage(image, imageRectangle);
            thumbnailImg.Save(targetPath, image.RawFormat);
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        try
        {

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_rptforsampledev", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@samplecode", txtsamplecode.Text);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            //SqlParameter[] param = new SqlParameter[1];
            //param[0] = new SqlParameter("@samplecode", txtsamplecode.Text);
            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_rptforsampledev", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //Add Image
                ds.Tables[0].Columns.Add("image", typeof(System.Byte[]));
                ds.Tables[0].Columns.Add("cadimage", typeof(System.Byte[]));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Convert.ToString(dr["imgpath"]) != "")
                    {
                        FileInfo file = new FileInfo(Server.MapPath(dr["imgpath"].ToString()));
                        if (file.Exists)
                        {
                            string img = dr["imgpath"].ToString();
                            img = Server.MapPath(img);
                            Byte[] img_byte = File.ReadAllBytes(img);
                            dr["image"] = img_byte;

                        }
                    }
                    //*******Cad image
                    if (Convert.ToString(dr["cadimgpath"]) != "")
                    {
                        FileInfo file = new FileInfo(Server.MapPath(dr["cadimgpath"].ToString()));
                        if (file.Exists)
                        {
                            string img = dr["cadimgpath"].ToString();
                            img = Server.MapPath(img);
                            Byte[] img_byte = File.ReadAllBytes(img);
                            dr["cadimage"] = img_byte;

                        }
                    }
                }
                if (Session["varcompanyid"].ToString() == "44")
                {
                    Session["rptFileName"] = "~\\Reports\\rptsampledevelopmentAgni.rpt";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\rptsampledevelopment.rpt";
                }
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptsampledevelopment.xsd";
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
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;

        }
        
    }
    protected void DGraw_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblDeleteButtonCall.Text = "3";
        DataGridDeleteID = e.RowIndex;
        if (chkedit.Checked == true)
        {
            Popup(true);
            txtpwd.Focus();
            lblmsg.Text = "";
        }
        else
        {
            DGRawDeleteClick(DataGridDeleteID);
        }
    }
    protected void DGRawDeleteClick(int rowindex)
    {
        if (ViewState["rawtable"] != null)
        {
            DataTable dt = (DataTable)ViewState["rawtable"];
            //int rowindex = e.RowIndex;
            if (dt.Rows.Count > 0)
            {
                dt.Rows.Remove(dt.Rows[rowindex]);
                ViewState["rawtable"] = dt;
                DGraw.DataSource = dt;
                DGraw.DataBind();
            }
        }
    }
    protected void DGvendor_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblDeleteButtonCall.Text = "4";
        DataGridDeleteID = e.RowIndex;
        if (chkedit.Checked == true)
        {
            Popup(true);
            txtpwd.Focus();
            lblmsg.Text = "";
        }
        else
        {
            DGvendorDeleteClick(DataGridDeleteID);
        }
    }

    protected void DGvendorDeleteClick(int rowindex)
    {
        if (ViewState["vendortable"] != null)
        {
            DataTable dt = (DataTable)ViewState["vendortable"];
            //int rowindex = e.RowIndex;
            if (dt.Rows.Count > 0)
            {
                dt.Rows.Remove(dt.Rows[rowindex]);
                ViewState["vendortable"] = dt;
                DGvendor.DataSource = dt;
                DGvendor.DataBind();
            }
        }
    }
    public SortDirection dir
    {
        get
        {
            if (ViewState["dirState"] == null)
            {
                ViewState["dirState"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["dirState"];
        }
        set
        {
            ViewState["dirState"] = value;
        }
    }
    protected void DGvendor_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortingdirection = string.Empty;
        if (dir == SortDirection.Ascending)
        {
            dir = SortDirection.Descending;
            sortingdirection = "Desc";
        }
        else
        {
            dir = SortDirection.Ascending;
            sortingdirection = "Asc";
        }
        DataView sortedview = new DataView((DataTable)ViewState["vendortable"]);
        sortedview.Sort = e.SortExpression + " " + sortingdirection;
        DGvendor.DataSource = sortedview;
        DGvendor.DataBind();
        //Apply Css Class 


        //
    }
    private int GetColumnIndex(string SortExpression)
    {
        int i = 0;
        foreach (DataControlField c in DGvendor.Columns)
        {
            if (c.SortExpression == SortExpression)
                break;
            i++;
        }
        return i;
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        hnid.Value = "0";
        txtsamplecode.Text = "";
        txttypesamplecode.Text = "";
        TRSearch.Visible = false;
        btndelete.Visible = false;
        ChkForWeavingConsumptionSave.Checked = false;
        ChkForWeavingConsumptionSave.Visible = false;
        if (chkedit.Checked == true)
        {
            TRSamplecode.Visible = true;
            TRSearch.Visible = true;
            btndelete.Visible = true;
            TxtSampleCodeNew.Visible = true;
            ChkForWeavingConsumptionSave.Visible = true;
            ChkForWeavingConsumptionSave.Checked = true;
        }
        else
        {
            TRSamplecode.Visible = false;
            TxtSampleCodeNew.Visible = false;
        }
        //******************
        if (TRSearch.Visible == true)
        {
            string str = @"select customerid,CustomerCode+'/'+CompanyName as Customer from customerinfo  order by customer
                          select IM.ITEM_ID,IM.ITEM_NAME From ITEM_MASTER IM inner join CategorySeparate cs on IM.CATEGORY_ID=cs.Categoryid and cs.id=0 order by ITEM_NAME
                          select Distinct DesignName,DesignName as Designname1 From  SampleDevelopmentMaster order by Designname1
                          select Distinct Colorname,Colorname as Colorname1 From  SampleDevelopmentMaster order by Colorname1";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDEbuyer, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddEitem, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref dddesign, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddcolor, ds, 3, true, "--Plz Select--");
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string str = @"select SDM.ID,SDM.Samplecode,SDM.Monthid,SDM.SampleYear,SDM.Purpose,SDM.customerid,SDM.Purposeval,SDM.Categoryid,SDM.Itemid,SDM.Qualityid,
                        SDM.DesignName,SDM.ColorName,SDM.shapeid,SDM.Sizeflag,SDM.Sizeid,isnull(SDM.imgpath,'') as Imgpath,isnull(SDM.Cadimgpath,'') as Cadimgpath,
                        SDM.Remark,SDM.Weightgsm,SDM.Totalgsm, D.DesignID, C.ColorID 
                        From SampleDevelopmentMaster SDM(Nolock) 
                        JOIN Design D(Nolock) ON D.DesignName = SDM.DesignName 
                        JOIN Color C(Nolock) ON C.ColorName = SDM.ColorName
                        Where SDM.Samplecode='" + txttypesamplecode.Text + @"'

                        Select Vf.ITEM_NAME+' '+vf.QualityName+' '+vf.designName+' '+vf.ColorName+' '+vf.ShapeName+' '+vf.SizeFt+' '+vf.ShadeColorName + 
                        ' ' + PNM.PROCESS_NAME + Case When SRMD.CalType = 0 Then ' AREA' Else ' PCS' End + ')' ItemDescription,
                        vf.ITEM_FINISHED_ID,Sr.anticipatedwt,sr.actualwt, SR.ProcessID, SR.CalType 
                        From SampledevRawmaterialDescription SR 
                        JOIN SampledevRawmaterialDescription SRMD(Nolock) ON SRMD.Samplecode = SR.SampleCode And SRMD.ProcessID = " + DDRProcessName.SelectedValue + @"
                        inner join V_FinishedItemDetail vf on SR.Ifinishedid=vf.ITEM_FINISHED_ID 
                        Where SR.Samplecode='" + txttypesamplecode.Text + @"'

                        select D.DepartmentName as Department,Ei.EmpName as vendor,D.DepartmentId,Ei.empid
                        From SampleDevVendorDetail SD inner Join Department D on SD.deptid=D.DepartmentId 
                        inner join EmpInfo ei on Sd.Empid=Ei.EmpId where Sd.Samplecode='" + txttypesamplecode.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //*************Master Detail
            txtsamplecode.Text = ds.Tables[0].Rows[0]["samplecode"].ToString();
            DDMonth.SelectedValue = ds.Tables[0].Rows[0]["Monthid"].ToString();
            DDyear.SelectedValue = ds.Tables[0].Rows[0]["SampleYear"].ToString();
            DDpurpose.SelectedIndex = Convert.ToInt16(ds.Tables[0].Rows[0]["Purpose"]);
            DDpurpose_SelectedIndexChanged(sender, new EventArgs());
            if (TDbuyer.Visible == true)
            {
                DDbuyer.SelectedValue = ds.Tables[0].Rows[0]["customerid"].ToString();
            }
            txtgeneral.Text = ds.Tables[0].Rows[0]["Purposeval"].ToString();
            ddCatagory.SelectedValue = ds.Tables[0].Rows[0]["CategoryId"].ToString();
            ddCatagory_SelectedIndexChanged(sender, new EventArgs());
            dditemname.SelectedValue = ds.Tables[0].Rows[0]["Itemid"].ToString();
            dditemname_SelectedIndexChanged(sender, new EventArgs());
            dquality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
            DDDesignName.SelectedValue = ds.Tables[0].Rows[0]["DesignID"].ToString();
            DDColorName.SelectedValue = ds.Tables[0].Rows[0]["ColorID"].ToString();
            ddshape.SelectedValue = ds.Tables[0].Rows[0]["Shapeid"].ToString();
            DDsizetype.SelectedValue = ds.Tables[0].Rows[0]["Sizeflag"].ToString();
            ddshape_SelectedIndexChanged(sender, new EventArgs());
            ddsize.SelectedValue = ds.Tables[0].Rows[0]["Sizeid"].ToString();
            txtremark.Text = ds.Tables[0].Rows[0]["Remark"].ToString();
            txttotalgsm.Text = ds.Tables[0].Rows[0]["Totalgsm"].ToString();
            txtwtgsm.Text = ds.Tables[0].Rows[0]["Weightgsm"].ToString();
            if (ds.Tables[0].Rows[0]["Imgpath"].ToString() != "")
            {
                if (File.Exists(Server.MapPath(ds.Tables[0].Rows[0]["Imgpath"].ToString())))
                {
                    lblimage.ImageUrl = ds.Tables[0].Rows[0]["Imgpath"].ToString() + "?" + DateTime.Now.Ticks.ToString();
                }
            }
            if (ds.Tables[0].Rows[0]["cadImgpath"].ToString() != "")
            {
                if (File.Exists(Server.MapPath(ds.Tables[0].Rows[0]["cadImgpath"].ToString())))
                {
                    imgcadupload.ImageUrl = ds.Tables[0].Rows[0]["cadImgpath"].ToString() + "?" + DateTime.Now.Ticks.ToString();
                }
            }
            //**************Raw Material
            ViewState["rawtable"] = null;
            ViewState["rawtable"] = ds.Tables[1];
            DGraw.DataSource = ds.Tables[1];
            DGraw.DataBind();
            //**************Vendor Detail
            ViewState["vendortable"] = null;
            ViewState["vendortable"] = ds.Tables[2];
            DGvendor.DataSource = ds.Tables[2];
            DGvendor.DataBind();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Sample code does not exists...')", true);
        }

    }
    protected void ddEitem_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select Distinct QualityId,QualityName from Quality Where Item_Id=" + ddEitem.SelectedValue + " order by QualityName";
        UtilityModule.ConditionalComboFill(ref ddEquality, str, true, "--Select--");

    }

    protected void lnkgetsamplecode_Click(object sender, EventArgs e)
    {
        hnid.Value = "0";
        txtsamplecode.Text = "";
        txttypesamplecode.Text = "";

        string str = @"select ID,Samplecode From  SampleDevelopmentMaster Where 1 = 1";
        if (TxtSampleCodeNew.Text != "")
        {
            str = str + " And SampleCode = '" + TxtSampleCodeNew.Text + "'";
        }
        if (DDEbuyer.SelectedIndex > 0)
        {
            str = str + " and customerid=" + DDEbuyer.SelectedValue;
        }
        if (ddEitem.SelectedIndex > 0)
        {
            str = str + " and Itemid=" + ddEitem.SelectedValue;
        }
        if (ddEquality.SelectedIndex > 0)
        {
            str = str + " and QualityId=" + ddEquality.SelectedValue;
        }
        if (dddesign.SelectedIndex > 0)
        {
            str = str + " and Designname='" + dddesign.SelectedItem.Text + "'";
        }
        if (ddcolor.SelectedIndex > 0)
        {
            str = str + " and Colorname='" + ddcolor.SelectedItem.Text + "'";
        }
        str = str + " order by id desc";
        UtilityModule.ConditionalComboFill(ref DDsamplecode, str, true, "--Plz Select--");
        if (DDsamplecode.Items.Count > 0)
        {
            DDsamplecode.SelectedIndex = 1;
            txttypesamplecode.Text = DDsamplecode.SelectedItem.Text;
            DDsamplecode_SelectedIndexChanged(sender, new EventArgs());
            if (Session["varcompanyid"].ToString() == "44")
            {
                ddlpreviousprocessname_SelectedIndexChanged(sender, new EventArgs());
            }
        }
      


    }
    protected void DDsamplecode_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = DDsamplecode.SelectedValue;
        string str = @"Select SDM.ID,SDM.Samplecode,SDM.Monthid,SDM.SampleYear,SDM.Purpose,SDM.customerid,SDM.Purposeval,SDM.Categoryid,SDM.Itemid,SDM.Qualityid,
                        SDM.DesignName,SDM.ColorName,SDM.shapeid,SDM.Sizeflag,SDM.Sizeid,isnull(SDM.imgpath,'') as Imgpath,isnull(SDM.Cadimgpath,'') as Cadimgpath,
                        SDM.Remark,SDM.Weightgsm,SDM.Totalgsm, D.DesignID, C.ColorID, REPLACE(CONVERT(NVARCHAR(11), SDM.DATE,106),' ','-') Date , 
                        Cast(SDM.ID as Nvarchar)+'_Costing.xlsx' CostingFile 
                        From SampleDevelopmentMaster SDM(Nolock) 
                        JOIN Design D(Nolock) ON D.DesignName = SDM.DesignName 
                        JOIN Color C(Nolock) ON C.ColorName = SDM.ColorName
                        Where SDM.ID=" + DDsamplecode.SelectedValue + @"

                        Select D.DepartmentName as Department,Ei.EmpName as vendor,D.DepartmentId,Ei.empid
                        From SampleDevVendorDetail SD inner Join Department D on SD.deptid=D.DepartmentId 
                        inner join EmpInfo ei on Sd.Empid=Ei.EmpId where Sd.Samplecode = '" + DDsamplecode.SelectedItem.Text + @"'

                        Select VF.ITEM_FINISHED_ID FinishedID, VF.CATEGORY_ID CategoryID, VF.ITEM_ID ItemID, VF.QualityID, VF.DesignID, VF.ColorID, VF.ShapeID, 
                        VF.SizeID, VF.ShadeColorID, VF.ITEM_NAME ItemName, VF.QualityName Quality, 
                        VF.DesignName + ' / ' + VF.ColorName + ' / ' + VF.ShapeName + ' / ' + 
                        Case When SDM.Sizeflag = 1 Then (case when SDM.Mastercompanyid=44 THEN VF.LWHMtr ELSE VF.SizeMtr  END) Else Case When SDM.Sizeflag = 2 Then (case when SDM.Mastercompanyid=44 THEN VF.LWHInch ELSE VF.SizeInch  END) Else (case when SDM.Mastercompanyid=44 THEN VF.LWHFt ELSE VF.SizeFt  END) End End [Description]
                        From SAMPLEDEVELOPMENTITEMSPLITDETAIL a(Nolock)
                        JOIN SampleDevelopmentMaster SDM(Nolock) ON SDM.SampleCode = a.SampleCode 
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = a.FinishedID 
                        Where a.Samplecode = '" + DDsamplecode.SelectedItem.Text + @"'";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //*************Master Detail
            txtsamplecode.Text = ds.Tables[0].Rows[0]["samplecode"].ToString();
            TxtDate.Text = ds.Tables[0].Rows[0]["Date"].ToString();
            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Monthid"])))
            {
                if (DDMonth.Items.FindByValue(ds.Tables[0].Rows[0]["Monthid"].ToString()) != null)
                {
                    DDMonth.SelectedValue = ds.Tables[0].Rows[0]["Monthid"].ToString();
                }

            }
            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["SampleYear"])))
            {
                if (DDyear.Items.FindByValue(ds.Tables[0].Rows[0]["SampleYear"].ToString()) != null)
                {
                    DDyear.SelectedValue = ds.Tables[0].Rows[0]["SampleYear"].ToString();
                }
            }
            if (DDpurpose != null)
            {
                DDpurpose.SelectedIndex = Convert.ToInt16(ds.Tables[0].Rows[0]["Purpose"]);
            }
            DDpurpose_SelectedIndexChanged(sender, new EventArgs());
            if (TDbuyer.Visible == true)
            {
                DDbuyer.SelectedValue = ds.Tables[0].Rows[0]["customerid"].ToString();
            }
            txtgeneral.Text = ds.Tables[0].Rows[0]["Purposeval"].ToString();
            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["CategoryId"])))
            {
                if (ddCatagory.Items.FindByValue(ds.Tables[0].Rows[0]["CategoryId"].ToString()) != null)
                {
                    ddCatagory.SelectedValue = ds.Tables[0].Rows[0]["CategoryId"].ToString();
                }
            }
            ddCatagory_SelectedIndexChanged(sender, new EventArgs());
            dditemname.SelectedValue = ds.Tables[0].Rows[0]["Itemid"].ToString();
            dditemname_SelectedIndexChanged(sender, new EventArgs());
            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["QualityId"])))
            {
                if (dquality.Items.FindByValue(ds.Tables[0].Rows[0]["QualityId"].ToString()) != null)
                {
                    dquality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
                }
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["DesignID"])))
            {
                if (DDDesignName.Items.FindByValue(ds.Tables[0].Rows[0]["DesignID"].ToString()) != null)
                {
                    DDDesignName.SelectedValue = ds.Tables[0].Rows[0]["DesignID"].ToString();
                }
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["ColorID"])))
            {
                if (DDColorName.Items.FindByValue(ds.Tables[0].Rows[0]["ColorID"].ToString()) != null)
                {
                    DDColorName.SelectedValue = ds.Tables[0].Rows[0]["ColorID"].ToString();
                }
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Shapeid"])))
            {
                if (ddshape.Items.FindByValue(ds.Tables[0].Rows[0]["Shapeid"].ToString()) != null)
                {
                    ddshape.SelectedValue = ds.Tables[0].Rows[0]["Shapeid"].ToString();
                }
            }
            DDsizetype.SelectedValue = ds.Tables[0].Rows[0]["Sizeflag"].ToString();
            ddshape_SelectedIndexChanged(sender, new EventArgs());
            ddsize.SelectedValue = ds.Tables[0].Rows[0]["Sizeid"].ToString();
            txtremark.Text = ds.Tables[0].Rows[0]["Remark"].ToString();
            txttotalgsm.Text = ds.Tables[0].Rows[0]["Totalgsm"].ToString();
            txtwtgsm.Text = ds.Tables[0].Rows[0]["Weightgsm"].ToString();
            if (ds.Tables[0].Rows[0]["Imgpath"].ToString() != "")
            {
                if (File.Exists(Server.MapPath(ds.Tables[0].Rows[0]["Imgpath"].ToString())))
                {
                    lblimage.ImageUrl = ds.Tables[0].Rows[0]["Imgpath"].ToString() + "?" + DateTime.Now.Ticks.ToString();
                }
            }
            else
            {
                lblimage.ImageUrl = null + "?time=" + DateTime.Now.ToString(); ;
            }
            if (ds.Tables[0].Rows[0]["cadImgpath"].ToString() != "")
            {
                if (File.Exists(Server.MapPath(ds.Tables[0].Rows[0]["cadImgpath"].ToString())))
                {
                    imgcadupload.ImageUrl = ds.Tables[0].Rows[0]["cadImgpath"].ToString() + "?" + DateTime.Now.Ticks.ToString();
                }
            }
            else
            {
                imgcadupload.ImageUrl = null + "?time=" + DateTime.Now.ToString(); ; ;
            }
                        
            //**************Vendor Detail
            ViewState["vendortable"] = null;
            ViewState["vendortable"] = ds.Tables[1];
            DGvendor.DataSource = ds.Tables[1];
            DGvendor.DataBind();

            GDSplitDescription.DataSource = ds.Tables[2];
            GDSplitDescription.DataBind();
            if (ds.Tables[2] != null)
            {
                if (ds.Tables[2].Rows.Count > 0)
                {
                    if (ds.Tables[2].Rows[0]["FinishedID"] != null)
                    {
                        GDSplitDescriptionSelectedChanged(Convert.ToInt32(ds.Tables[2].Rows[0]["FinishedID"]));
                    }
                }
            }
          //  GDSplitDescriptionSelectedChanged(Convert.ToInt32(ds.Tables[2].Rows[0]["FinishedID"]));
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Sample code does not exists...')", true);
        }
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        lblDeleteButtonCall.Text = "1";
        if (chkedit.Checked == true)
        {
            Popup(true);
            txtpwd.Focus();
            lblmsg.Text = "";
        }
        else
        {
            DeleteButtonClick();
        }
    }
    protected void DeleteButtonClick()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();

        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@Id", hnid.Value);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@Mastercompanyid", Session["varcompanyId"]);
            param[4] = new SqlParameter("@Imgpath", SqlDbType.VarChar, 200);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@CADIMGPATH", SqlDbType.VarChar, 200);
            param[5].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETESAMPLEDEVELOPMENT", param);
            lblmsg.Text = param[1].Value.ToString();
            //**************Delete Images
            if (param[4].Value.ToString() != "")
            {
                File.Delete(Server.MapPath(param[4].Value.ToString()));
            }
            if (param[5].Value.ToString() != "")
            {
                File.Delete(Server.MapPath(param[5].Value.ToString()));
            }
            //**************
            Tran.Commit();

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void txtpwd_TextChanged(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        if (variable.VarSAMPLECODEEDIT_DELETE_PWD == (txtpwd.Text).ToUpper())
        {
            if (lblDeleteButtonCall.Text == "1")
            {
                DeleteButtonClick();
            }
            else if (lblDeleteButtonCall.Text == "2")
            {
                AddMaterialButtonClick();
            }
            else if (lblDeleteButtonCall.Text == "3")
            {
                DGRawDeleteClick(DataGridDeleteID);
            }
            else if (lblDeleteButtonCall.Text == "4")
            {
                DGvendorDeleteClick(DataGridDeleteID);
            }
            Popup(false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please Enter Correct Password..')", true);
            lblmsg.Text = "Please Enter Correct Password..";
        }
    }
    void Popup(bool isDisplay)
    {
        StringBuilder builder = new StringBuilder();
        if (isDisplay)
        {
            builder.Append("<script>");
            builder.Append("ShowPopup();</script>");
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", builder.ToString());
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ShowPopup", builder.ToString(), false);
        }
        else
        {
            builder.Append("<script>");
            builder.Append("HidePopup();</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HidePopup", builder.ToString(), false);
        }
    }
    protected void DDRquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillLotNo();
    }
    private void FillLotNo()
    {
        lblvendorname.Text = "";
        string str = @"SELECT  DISTINCT S.LOTNO,S.LOTNO+ CASE WHEN PRD.LOTNO IS NULL THEN '' ELSE ' # '+PRD.VENDORLOTNO END AS LOTNO1
                    FROM STOCK S 
                    INNER JOIN V_FINISHEDITEMDETAIL VF ON S.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID
                    LEFT  JOIN PURCHASERECEIVEDETAIL PRD ON S.ITEM_FINISHED_ID=PRD.FINISHEDID AND S.LOTNO=PRD.LOTNO
                    WHERE VF.Qualityid=" + DDRquality.SelectedValue;
        if (ChkForAllLotNoofItem.Checked == false)
        {
            str = str + " And ROUND(QTYINHAND, 3) > 0";
        }
        //if (Session["varcompanyId"].ToString() == "16")
        //{
        //    str = str + " and S.GODOWNID=5";
        //}
        str = str + " order by S.Lotno";
        UtilityModule.ConditionalComboFill(ref DDLotno, str, true, "--Plz Select--");
    }
    protected void DDLotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select top(1) ei.EmpName From Purchasereceivemaster prm inner join PurchaseReceiveDetail prd on prm.PurchaseReceiveId=prd.PurchaseReceiveId
                    inner join EmpInfo ei on prm.PartyId=ei.EmpId
                    Where prd.LotNo='" + DDLotno.SelectedValue + "' order by prd.PurchaseReceiveDetailId desc";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblvendorname.Text = ds.Tables[0].Rows[0]["empname"].ToString();
        }
        else
        {
            lblvendorname.Text = "";
        }
    }
    protected void ChkForAllLotNoofItem_CheckedChanged(object sender, EventArgs e)
    {
        FillLotNo();
    }
    protected void ddSplitCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        SplitCategorySelectedIndexChanged();
    }
    protected void SplitCategorySelectedIndexChanged()
    {
        string str = "select Item_Id, ITEM_NAME from Item_master Where CATEGORY_ID = " + ddSplitCatagory.SelectedValue + " order by ITEM_NAME";
        UtilityModule.ConditionalComboFill(ref ddSplitItemname, str, true, "--Plz Select--");
        FillSplitCombo();
    }
    protected void FillSplitCombo()
    {
        TRSplitQuality.Visible = false;
        TRSplitDesignName.Visible = false;
        TRSplitColorName.Visible = false;
        TRSplitShape.Visible = false;
        TRSplitSize.Visible = false;
        TRSplitShadeColor.Visible = false;

        string str = @"select PARAMETER_ID from ITEM_CATEGORY_PARAMETERS where category_id=" + ddCatagory.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["Parameter_Id"].ToString())
                {
                    case "1":
                        TRSplitQuality.Visible = true;
                        break;
                    case "2":
                        TRSplitDesignName.Visible = true;
                        break;
                    case "3":
                        TRSplitColorName.Visible = true;
                        break;
                    case "4":
                        TRSplitShape.Visible = true;
                        break;
                    case "5":
                        TRSplitSize.Visible = true;
                        break;
                    case "6":
                        TRSplitShadeColor.Visible = true;
                        break;
                }
            }
        }
    }
    protected void ddSplitItemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSplitQDCS();
    }

    protected void FillSplitQDCS()
    {
        string str = null;
        if (TRSplitQuality.Visible == true)
        {
            str = "select Distinct QualityId,QualityName from Quality(Nolock) Where Item_Id=" + ddSplitItemname.SelectedValue + " order by QualityName";
            UtilityModule.ConditionalComboFill(ref DDSplitQuality, str, true, "--Select--");
        }
        if (TRSplitDesignName.Visible == true)
        {
            str = "Select DesignID, DesignName From Design(Nolock) Where MasterCompanyid = " + Session["varcompanyid"] + @" Order By DesignName ";
            UtilityModule.ConditionalComboFill(ref DDSplitDesignName, str, true, "--Select--");
        }
        if (TRSplitColorName.Visible == true)
        {
            str = "Select ColorID, ColorName From Color(Nolock) Where MasterCompanyid = " + Session["varcompanyid"] + " Order By ColorName ";
            UtilityModule.ConditionalComboFill(ref DDSplitColorName, str, true, "--Select--");
        }
        if (TRSplitShape.Visible == true)
        {
            str = "select Distinct Sh.shapeid,Sh.shapename from Shape Sh  order by shapename";
            UtilityModule.ConditionalComboFill(ref DDSplitShape, str, true, "--Select--");
        }
        if (TRSplitShadeColor.Visible == true)
        {
            str = "select Distinct ShadeColorId, ShadeColorName From ShadeColor Where MasterCompanyid = " + Session["varcompanyid"] + " Order By ShadeColorName";
            UtilityModule.ConditionalComboFill(ref ddSplitShadeColor, str, true, "--Select--");
        }
    }
    protected void DDSplitShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSplitSize();
    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDSplitQuality.SelectedValue = dquality.SelectedValue;
    }
    protected void DDDesignName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDSplitDesignName.SelectedValue = DDDesignName.SelectedValue;
    }
    protected void DDColorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDSplitColorName.SelectedValue = DDColorName.SelectedValue;
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
        DDSplitShape.SelectedValue = ddshape.SelectedValue;
        FillSplitSize();
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddSplitSize.SelectedValue = ddsize.SelectedValue;
    }
    protected void FillSplitSize()
    {
        string str = null, size = null;
        
        switch (DDsizetype.SelectedValue.ToString())
        {
            case "0":
                size = "SizeFt";
                break;
            case "1":
                size = "Sizemtr";
                break;
            case "2":
                size = "Sizeinch";
                break;
            default:
                size = "SizeFt";
                break;
        }
        if (Session["varcompanyid"].ToString() == "44")
        {
            switch (DDsizetype.SelectedValue.ToString())
            {
                case "0":
                    str = "Select Distinct S.sizeid,cast(s.WidthFt as varchar)+'x'+cast(s.LengthFt as varchar) +case when s.Heightft>0 then 'x'+cast(s.HeightFt as varchar) else ''  end as  SizeFt from Size S  Where S.shapeid=" + ddshape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
                    break;
                case "1":
                    str = " Select Distinct S.sizeid,cast(s.WidthMtr as varchar)+'x'+cast(s.LengthMtr as varchar) +case when s.HeightMtr>0 then 'x'+cast(s.HeightMtr as varchar) else ''  end as  Sizemtr from Size S Where S.shapeid=" + ddshape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];

                    break;
                case "2":
                    str = "Select Distinct S.sizeid,cast(s.WidthInch as varchar)+'x'+cast(s.LengthInch as varchar) +case when s.HeightInch>0 then 'x'+cast(s.HeightInch as varchar) else ''  end as  Sizeinch from Size S  Where S.shapeid=" + ddshape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
                    break;
                default:
                    str = "Select Distinct S.sizeid,cast(s.WidthFt as varchar)+'x'+cast(s.LengthFt as varchar) +case when s.Heightft>0 then 'x'+cast(s.HeightFt as varchar) else ''  end as  SizeFt from Size S  Where S.shapeid=" + ddshape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
                    break;
            }


        }
        else
        {
            str = "select Distinct S.sizeid,S." + size + " from Size S  Where S.shapeid=" + DDSplitShape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
        }
        if (Session["varcompanyId"].ToString() == "16" || Session["varcompanyId"].ToString() == "28")
        {
            str = str + " order by S." + size + ", S.sizeid";
        }
        else if (Session["varcompanyId"].ToString() == "44")
        {
            str = str + " order by 1";
        }
        else
        {
            str = str + " order by S." + size;
        }
        UtilityModule.ConditionalComboFill(ref ddSplitSize, str, true, "--Select--");
    }
    protected void GDSplitDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        GDSplitDescriptionSelectedChanged(Convert.ToInt32(GDSplitDescription.SelectedDataKey.Value));
    }
    protected void GDSplitDescriptionSelectedChanged(int FinishedID)
    {
        string str = @"Select VF.ITEM_FINISHED_ID FinishedID, VF.CATEGORY_ID CategoryID, VF.ITEM_ID ItemID, VF.QualityID, VF.DesignID, VF.ColorID, VF.ShapeID, 
                    VF.SizeID, VF.ShadeColorID 
                    From V_FinishedItemDetail VF(Nolock) 
                    Where VF.ITEM_FINISHED_ID = " + FinishedID;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddSplitCatagory.SelectedValue = ds.Tables[0].Rows[0]["CategoryID"].ToString();
            SplitCategorySelectedIndexChanged();
            ddSplitItemname.SelectedValue = ds.Tables[0].Rows[0]["ItemID"].ToString();
            FillSplitQDCS();
            //if (TRSplitQuality.Visible == true)
            //{
            //    DDSplitQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityID"].ToString();
            //}
            //if (TRSplitDesignName.Visible == true)
            //{
            //    DDSplitDesignName.SelectedValue = ds.Tables[0].Rows[0]["DesignID"].ToString();
            //}
            //if (TRSplitColorName.Visible == true)
            //{
            //    DDSplitColorName.SelectedValue = ds.Tables[0].Rows[0]["ColorID"].ToString();
            //}
            //if (TRSplitShape.Visible == true)
            //{
            //    DDSplitShape.SelectedValue = ds.Tables[0].Rows[0]["ShapeID"].ToString();
            //    FillSplitSize();
            //}
            //if (TRSplitSize.Visible == true)
            //{
            //    ddSplitSize.SelectedValue = ds.Tables[0].Rows[0]["SizeID"].ToString();
            //}
            //if (TRSplitShadeColor.Visible == true)
            //{
            //    ddSplitShadeColor.SelectedValue = ds.Tables[0].Rows[0]["ShadeColorID"].ToString();
            //}
            if (TRSplitQuality.Visible == true)
            {
                if (DDSplitQuality.Items.FindByValue(ds.Tables[0].Rows[0]["QualityID"].ToString().Trim()) != null)
                {
                    DDSplitQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityID"].ToString();
                }
            }
            if (TRSplitDesignName.Visible == true)
            {
                if (DDSplitDesignName.Items.FindByValue(ds.Tables[0].Rows[0]["DesignID"].ToString().Trim()) != null)
                {
                    DDSplitDesignName.SelectedValue = ds.Tables[0].Rows[0]["DesignID"].ToString();
                }
            }
            if (TRSplitColorName.Visible == true)
            {
                if (DDSplitColorName.Items.FindByValue(ds.Tables[0].Rows[0]["ColorID"].ToString().Trim()) != null)
                {
                    DDSplitColorName.SelectedValue = ds.Tables[0].Rows[0]["ColorID"].ToString();
                }
            }
            if (TRSplitShape.Visible == true)
            {
                if (DDSplitShape.Items.FindByValue(ds.Tables[0].Rows[0]["ShapeID"].ToString().Trim()) != null)
                {
                    DDSplitShape.SelectedValue = ds.Tables[0].Rows[0]["ShapeID"].ToString();
                }
                FillSplitSize();
            }
            if (TRSplitSize.Visible == true)
            {
                if (ddSplitSize.Items.FindByValue(ds.Tables[0].Rows[0]["SizeID"].ToString().Trim()) != null)
                {
                    ddSplitSize.SelectedValue = ds.Tables[0].Rows[0]["SizeID"].ToString();
                }
            }
            if (TRSplitShadeColor.Visible == true)
            {
                if (ddSplitShadeColor.Items.FindByValue(ds.Tables[0].Rows[0]["ShadeColorID"].ToString().Trim()) != null)
                {
                    ddSplitShadeColor.SelectedValue = ds.Tables[0].Rows[0]["ShadeColorID"].ToString();
                }
            }

        }
        Fill_DGRaw(FinishedID);
    }
    protected void Fill_DGRaw(int FinishedID)
    {
        string str = string.Empty;

        if (Session["varcompanyid"].ToString() == "44")
        {
            str = @"Select Vf.ITEM_NAME+' '+vf.QualityName+' '+vf.designName+' '+vf.ColorName+' '+vf.ShapeName+' '+vf.SizeFt+'inputcolor:'+vf.ShadeColorName +'outputcolor:'+isnull(vf1.ShadeColorName,'')+'
            (' + PNM.PROCESS_NAME + Case When SRMD.CalType = 0 Then ' AREA' Else ' PCS' End + ')' ItemDescription,
            vf.ITEM_FINISHED_ID,case when SRMD.Mastercompanyid=44 then ISNULL(SRMD.OSHADEID,0) ELSE 0 END AS OSHADEID, SRMD.anticipatedwt, SRMD.actualwt, SRMD.Lotno, SRMD.Dyeingtype, SRMD.vendorname, SRMD.ProcessID, 
            SRMD.CalType, SRMD.UnitID, U.UnitName 
            From SAMPLEDEVELOPMENTMASTER SR(Nolock) 
            JOIN SampledevRawmaterialDescription SRMD(Nolock) ON SRMD.Samplecode = SR.SampleCode And SRMD.ProcessID = " + DDRProcessName.SelectedValue + @"
            JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.Process_Name_ID = SRMD.ProcessID 
            join V_FinishedItemDetail vf(Nolock) ON SRMD.Ifinishedid = vf.ITEM_FINISHED_ID 
            left join ShadeColor vf1(Nolock) ON  vf1.ShadecolorId = SRMD.OSHADEID
            JOIN Unit U(Nolock) ON U.UnitID = SRMD.UnitID 
            Where SR.ID=" + hnid.Value + @" And SRMD.SplitFinishedID = " + FinishedID;
        }
        else {
             str = @"Select Vf.ITEM_NAME+' '+vf.QualityName+' '+vf.designName+' '+vf.ColorName+' '+vf.ShapeName+' '+vf.SizeFt+' '+vf.ShadeColorName +
            '  (' + PNM.PROCESS_NAME + Case When SRMD.CalType = 0 Then ' AREA' Else ' PCS' End + ')' ItemDescription,
            vf.ITEM_FINISHED_ID,case when SRMD.Mastercompanyid=44 then ISNULL(SRMD.OSHADEID,0) ELSE 0 END AS OSHADEID, SRMD.anticipatedwt, SRMD.actualwt, SRMD.Lotno, SRMD.Dyeingtype, SRMD.vendorname, SRMD.ProcessID, 
            SRMD.CalType, SRMD.UnitID, U.UnitName 
            From SAMPLEDEVELOPMENTMASTER SR(Nolock) 
            JOIN SampledevRawmaterialDescription SRMD(Nolock) ON SRMD.Samplecode = SR.SampleCode And SRMD.ProcessID = " + DDRProcessName.SelectedValue + @"
            JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.Process_Name_ID = SRMD.ProcessID 
            join V_FinishedItemDetail vf(Nolock) ON SRMD.Ifinishedid = vf.ITEM_FINISHED_ID 
            JOIN Unit U(Nolock) ON U.UnitID = SRMD.UnitID 
            Where SR.ID=" + hnid.Value + @" And SRMD.SplitFinishedID = " + FinishedID;
        
        
        
        
        }

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        object o = new object();
        //**************Raw Material
        ViewState["rawtable"] = null;
        ViewState["rawtable"] = ds.Tables[0];

        if(ds !=null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DGraw.DataSource = ds.Tables[0];
                DGraw.DataBind();
            }
            else
            {
                if (Session["varcompanyid"].ToString() == "44")
                {
                    ddlpreviousprocessname_SelectedIndexChanged(o, new EventArgs());
                }
            }
        }
        else
        {
            if (Session["varcompanyid"].ToString() == "44")
            { ddlpreviousprocessname_SelectedIndexChanged(o, new EventArgs()); }
        }
    }
    protected void Fill_DGRawPrevious(int FinishedID)
    {
        string str = @"Select Vf.ITEM_NAME+' '+vf.QualityName+' '+vf.designName+' '+vf.ColorName+' '+vf.ShapeName+' '+vf.SizeFt+'inputcolor:'+vf.ShadeColorName +'outputcolor:'+isnull(vf1.ShadeColorName,'') +' (' + PNM.PROCESS_NAME + Case When SRMD.CalType = 0 Then ' AREA' Else ' PCS' End + ')' ItemDescription,
            vf.ITEM_FINISHED_ID,case when SRMD.Mastercompanyid=44 then ISNULL(SRMD.OSHADEID,0) ELSE 0 END AS OSHADEID, SRMD.anticipatedwt, SRMD.actualwt, SRMD.Lotno, SRMD.Dyeingtype, SRMD.vendorname, SRMD.ProcessID, 
            SRMD.CalType, SRMD.UnitID, U.UnitName 
            From SAMPLEDEVELOPMENTMASTER SR(Nolock) 
            JOIN SampledevRawmaterialDescription SRMD(Nolock) ON SRMD.Samplecode = SR.SampleCode And SRMD.ProcessID = " + ddlpreviousprocessname.SelectedValue + @"
            JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.Process_Name_ID = SRMD.ProcessID 
            join V_FinishedItemDetail vf(Nolock) ON SRMD.Ifinishedid = vf.ITEM_FINISHED_ID 
            left join ShadeColor vf1(Nolock) ON  vf1.ShadecolorId = SRMD.OSHADEID
            JOIN Unit U(Nolock) ON U.UnitID = SRMD.UnitID 
            Where SR.ID=" + hnid.Value + @" And SRMD.SplitFinishedID = " + FinishedID + "and srmd.ProcessID=" + ddlpreviousprocessname.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        //**************Raw Material
        ViewState["rawtable"] = null;
        ViewState["rawtable"] = ds.Tables[0];
        DGraw.DataSource = ds.Tables[0];
        DGraw.DataBind();
    }
    protected void OK_Click(object sender, EventArgs e)
    {
        ClickOK();
        Fill_DGRaw(Convert.ToInt32(LblSplitFinishedID.Text));
    }
    protected void ClickOK()
    {
        CheckValidation();
        TextBox txtboxprodCode = new TextBox();
        txtboxprodCode.Text = "";
        //int FinishedID = 0;
        //if (Session["varcompanyid"].ToString() == "44")
        //{
        //    if (DDRCategory.SelectedIndex > 0)
        //    {
        //        if (Convert.ToInt16(DDRCategory.SelectedValue) == 2)
        //        {
        //            FinishedID = UtilityModule.getItemFinishedId(ddSplitItemname, DDSplitQuality, DDSplitDesignName, DDSplitColorName, DDSplitShape, ddSplitSize, txtboxprodCode, ddRloutputshadecolor, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        //        }
        //        else {

        //            FinishedID = UtilityModule.getItemFinishedId(ddSplitItemname, DDSplitQuality, DDSplitDesignName, DDSplitColorName, DDSplitShape, ddSplitSize, txtboxprodCode, ddSplitShadeColor, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        //        }
        //    }
        //    else
        //    {

        //        FinishedID = UtilityModule.getItemFinishedId(ddSplitItemname, DDSplitQuality, DDSplitDesignName, DDSplitColorName, DDSplitShape, ddSplitSize, txtboxprodCode, ddSplitShadeColor, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        //    }
        //}
        //else
        //{
            //FinishedID = UtilityModule.getItemFinishedId(ddSplitItemname, DDSplitQuality, DDSplitDesignName, DDSplitColorName, DDSplitShape, ddSplitSize, txtboxprodCode, ddSplitShadeColor, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        //}
        int FinishedID = UtilityModule.getItemFinishedId(ddSplitItemname, DDSplitQuality, DDSplitDesignName, DDSplitColorName, DDSplitShape, ddSplitSize, txtboxprodCode, ddSplitShadeColor, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        LblSplitFinishedID.Text = FinishedID.ToString();        
    }
    protected void CheckValidation()
    {
        if (ddSplitCatagory.SelectedIndex <= 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select product split Category..')", true);
            return;
        }
        if (ddSplitItemname.SelectedIndex <= 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select product split Item Name..')", true);
            return;
        }
        if (TRSplitQuality.Visible == true)
        {
            if (DDSplitQuality.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select product split Quality..')", true);
                return;
            }
        }
        if (TRSplitDesignName.Visible == true)
        {
            if (DDSplitDesignName.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select product split Design..')", true);
                return;
            }
        }
        if (TRSplitColorName.Visible == true)
        {
            if (DDSplitColorName.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select product split Color..')", true);
                return;
            }
        }
        if (TRSplitShape.Visible == true)
        {
            if (DDSplitShape.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select product split shape..')", true);
                return;
            }
        }
        if (TRSplitSize.Visible == true)
        {
            if (ddSplitSize.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select product split size..')", true);
                return;
            }
        }
        if (TRSplitShadeColor.Visible == true)
        {
            if (ddSplitShadeColor.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select product split shade color..')", true);
                return;
            }
        }
    }
    protected bool CheckValidationRawMaterial()
    {
        bool result = true;
        if (DDRProcessName.SelectedIndex <= 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select Raw Material Process  Name..')", true);
            result= false;
        }
        //if (DDRCalType.SelectedIndex <= 0)
        //{
        //    ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select Raw Material Cal Type..')", true);
        //    result= false;
        //}
        if (DDRCategory.SelectedIndex <= 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select Raw Material Category')", true);
            result= false;
        }
        if (DDRitemname.SelectedIndex <= 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select Raw Material Item')", true);
            result= false;
        }
        if (TDRQuality.Visible == true)
        {
            if (DDRquality.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select Raw Material Quality..')", true);
                result= false;
            }
        }
        if (TDRDesign.Visible == true)
        {
            if (ddRdesign.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select Raw Material  Design..')", true);
                result= false;
            }
        }
        if (TDRColor.Visible == true)
        {
            if (ddRcolor.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select Raw Material Color..')", true);
                result= false;
            }
        }
        if (TDRShape.Visible == true)
        {
            if (DDRshape.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select Raw Material shape..')", true);
                result= false;
            }
        }
        if (TDRSize.Visible == true)
        {
            if (DDRsize.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select Raw Material size..')", true);
                result= false;
            }
        }
        if (TDRShade.Visible == true)
        {
            if (ddRlshade.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select Raw Material shade color..')", true);
                result= false;
            }
        }
        if (DDDyeingtype.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select Raw Material Dyeing Type..')", true);
                result= false;
            }
        if (DDUnit.SelectedIndex <= 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select Unit..')", true);
            result= false;
        }
        if (string.IsNullOrEmpty(txtanticipatedwt.Text) && string.IsNullOrEmpty(txtprodwt.Text))
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Both Consumptions Cannot be 0')", true);
            result= false;
        
        }
        return result;
        
    }
    protected void GDSplitDescription_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();

        try
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@Id", hnid.Value);
            param[1] = new SqlParameter("@SplitFinishedID", GDSplitDescription.DataKeys[e.RowIndex].Value);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETESAMPLEDEVELOPMENTSPLITITEM", param);
            
            lblmsg.Text = param[2].Value.ToString();
            Tran.Commit();
            BindSplitItemGrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void BindSplitItemGrid()
    {
        string Str = @" Select VF.ITEM_FINISHED_ID FinishedID, VF.CATEGORY_ID CategoryID, VF.ITEM_ID ItemID, VF.QualityID, VF.DesignID, VF.ColorID, VF.ShapeID, 
            VF.SizeID, VF.ShadeColorID, VF.ITEM_NAME ItemName, VF.QualityName Quality, 
            VF.DesignName + ' / ' + VF.ColorName + ' / ' + VF.ShapeName + ' / ' + 
            Case When SDM.Sizeflag = 1 Then VF.SizeMtr Else Case When SDM.Sizeflag = 2 Then VF.SizeInch Else VF.SizeFt End End [Description]
            From SampleDevelopmentMaster SDM(Nolock) 
            JOIN SAMPLEDEVELOPMENTITEMSPLITDETAIL a(Nolock) ON a.SampleCode = SDM.SampleCode 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = a.FinishedID 
            Where SDM.ID = " + hnid.Value;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        GDSplitDescription.DataSource = ds.Tables[0];
        GDSplitDescription.DataBind();
    }
    protected void DDRProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClickOK();
        Fill_DGRaw(Convert.ToInt32(LblSplitFinishedID.Text));
        //Fill_DGRawPrevious(Convert.ToInt32(LblSplitFinishedID.Text));
    }
    protected void ddlpreviousprocessname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClickOK();
        Fill_DGRawPrevious(Convert.ToInt32(LblSplitFinishedID.Text));
    }
    protected void DGraw_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["varcompanyid"].ToString() == "44")
                {
                    if (Convert.ToInt16(ddlpreviousprocessname.SelectedValue) > 0)
                    {
                        DGraw.Columns[0].Visible = true;
                    }
                }

                int UID = 0, devcom = 0, prodcom = 0;
                if (Session["varuserid"] != null)
                {

                    UID = Convert.ToInt16(Session["varuserid"]);
                    string Str = @" Select isnull(canseeDevelopmentcons,0) as canseeDevelopmentcons,isnull(canseeProductioncons,0) as canseeProductioncons from NewUserDetail where userid=" + UID;

                    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                devcom = Convert.ToInt16(ds.Tables[0].Rows[0]["canseeDevelopmentcons"]);
                                prodcom = Convert.ToInt16(ds.Tables[0].Rows[0]["canseeProductioncons"]);
                                TextBox lblanticipatedwt = (TextBox)e.Row.FindControl("lblanticipatedwt");
                                TextBox txtactualwt = (TextBox)e.Row.FindControl("txtactualwt");
                                if (Session["varcompanyid"].ToString() == "44")
                                {
                                    if (devcom > 0)
                                    {
                                        if (!string.IsNullOrEmpty(txtactualwt.Text))
                                        {
                                            if (Convert.ToDecimal(lblanticipatedwt.Text) > 0)
                                            {
                                                if (Session["usertype"].ToString() == "1")
                                                {
                                                    lblanticipatedwt.Enabled = true;

                                                }

                                            }
                                            else
                                            {
                                                lblanticipatedwt.Enabled = true;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(lblanticipatedwt.Text))
                                        {
                                            if (Convert.ToDecimal(lblanticipatedwt.Text) > 0)
                                            {
                                                if (Session["usertype"].ToString() == "1")
                                                {
                                                    lblanticipatedwt.Enabled = true;

                                                }

                                            }
                                            else
                                            {
                                                if (Session["usertype"].ToString() == "1")
                                                {
                                                    lblanticipatedwt.Enabled = true;

                                                }
                                            }
                                        }

                                    }
                                    if (prodcom > 0)
                                    {
                                        if (!string.IsNullOrEmpty(txtactualwt.Text))
                                        {
                                            if (Convert.ToDecimal(txtactualwt.Text) > 0)
                                            {
                                                if (Session["usertype"].ToString() == "1")
                                                {
                                                    txtactualwt.Enabled = true;

                                                }

                                            }
                                            else { txtactualwt.Enabled = true; }
                                        }

                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(txtactualwt.Text))
                                        {
                                            if (Convert.ToDecimal(txtactualwt.Text) > 0)
                                            {
                                                if (Session["usertype"].ToString() == "1")
                                                {
                                                    txtactualwt.Enabled = true;

                                                }

                                            }
                                            else
                                            {
                                                if (Session["usertype"].ToString() == "1")
                                                {
                                                    txtactualwt.Enabled = true;

                                                }

                                            }

                                        }

                                    }



                                    //else

                                }
                                else
                                {
                                    lblanticipatedwt.Enabled = true;
                                    txtactualwt.Enabled = true;

                                }

                        }

                    }


                }


            }

        }
        

    }
    protected void DGraw_DataBound(object sender, EventArgs e)
    {
      

    }
    protected void BtnGetCostingFile_Click(object sender, EventArgs e)
    {
        if (hnid.Value != "0")
        {
            string filePath = Server.MapPath("~//SampleCostingFile//" + hnid.Value + "_Costing.xlsx");
            if (File.Exists(filePath))
            {
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + filePath);
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(filePath);
                Response.End();
            }
            else
            {
                lblmsg.Text = "Document does not exists...";
            }
        }
    }
}
