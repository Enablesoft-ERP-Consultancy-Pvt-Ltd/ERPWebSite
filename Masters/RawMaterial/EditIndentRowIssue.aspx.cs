using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Rawmaterial_EditIndentRowIssue : System.Web.UI.Page
{
    int ItemFinishedId = 0;
    static double varissuedQty = 0;
    static Boolean Editflag = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["Prmid"] = 0;
            ViewState["Prtid"] = 0;
            string str = @"Select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By Companyname 
                 Select DISTINCT PROCESS_NAME_ID,process_name from PROCESS_NAME_MASTER pm inner join IndentMaster im on pm.PROCESS_NAME_ID=im.processid And pm.MasterCompanyId=" + Session["varCompanyId"] + @" order by PROCESS_NAME_ID
                 Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + @" Order by GodownName
                 Select flagforsampleorder from mastersetting
                 Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];

            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "Select Comp Name");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 4, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddProcessName, ds, 1, true, "Select Process Name");
            UtilityModule.ConditionalComboFillWithDS(ref ddgodown, ds, 2, true, "--Select--");
            //DataRow dr = AllEnums.MasterTables.Mastersetting.ToTable().Select()[0];
            if (Convert.ToInt16(ds.Tables[3].Rows[0]["flagforsampleorder"]) == 1)
            {
                TDChkForOrder.Visible = true;
            }
            else
            {
                TDChkForOrder.Visible = false;
            }
            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            lablechange();
            Session["ReportPath"] = "Reports/IndentRawIssue.rpt";
            Session["CommanFormula"] = "{V_IndentRawIssue.PRMid}=" + ViewState["Prmid"] + "";
            //Without BOM
            if (Session["WithoutBOM"].ToString() == "1")
            {
                ChkForOrder.Checked = true;

            }

            switch (Session["varcompanyNo"].ToString())
            {
                case "10":
                    TDLotNo.Visible = false;
                    break;
                case "16":
                    TDMoisture.Visible = true;
                    BtnProcessToPNM.Visible = true;
                    break;
                case "28":
                    TDMoisture.Visible = true;
                    break;
                case "42":
                    TDMoisture.Visible = true;
                    break;
                case "43":
                    Label19.Text = "UCN No";
                    break;
            }
            //TagNo wise
            if (MySession.TagNowise == "1")
            {
                TDTagNo.Visible = true;
            }
            //
            if (Session["canedit"].ToString() == "1")
            {
                TDcomplete.Visible = true;
            }
            //Bin No.
            if (variable.VarBINNOWISE == "1")
            {
                TDBinNo.Visible = true;
            }

            if (variable.VarIndentRawIssueRateManual == "1")
            {
                TDManualRate.Visible = true;
            }
            if (variable.VarGSTForIndentRawIssue == "1")
            {
                TDGSTType.Visible = true;
                ChkForGSTReport.Visible = true;
            }
            if (Session["usertype"].ToString() != "1" && variable.VarOnlyPreviewButtonShowOnAllEditForm == "1")
            {
                btnsave.Visible = false;
                gvdetail.Columns[10].Visible = false;               

            }


        }
        //if (Session["canedit"].ToString() == "0") //non authenticated person
        //{
        //    // ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('You are not an authenticated person.Please contact to admin..');", true);
        //    System.Windows.Forms.MessageBox.Show("You are not an authenticated person.Please contact to admin..", "Alert", System.Windows.Forms.MessageBoxButtons.OK);
        //    Response.Redirect("~/main.aspx");
        //}
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        //lblsizename.Text = ParameterList[4];
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
        lblshadecolor.Text = ParameterList[7];
    }
    protected void ddProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessSelectedChange();
    }
    private void ProcessSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref ddempname, "SELECT distinct E.EmpId,E.EmpName FROM IndentMaster IM INNER JOIN  EmpInfo E ON IM.PartyId=E.EmpId And IM.Processid=" + ddProcessName.SelectedValue + " And E.MasterCompanyId=" + Session["varCompanyId"] + " order by Empname", true, "Select Emp");
    }
    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmpSelectedChange();
    }
    private void EmpSelectedChange()
    {
        string status = "";
        if (chkcomplete.Checked == true)
        {
            status = "Complete";
        }
        else
        {
            status = "Pending";
        }
        if (Session["varcompanyno"].ToString() == "6")
        {
            if (ChkForOrder.Checked == true)
            {
                UtilityModule.ConditionalComboFill(ref ddindentno, @"select distinct im.IndentId, IndentNo+'/'+CustomerOrderNo as IndentNo from IndentMaster im inner join 
                                                           Indentdetail id on id.IndentID=im.IndentID inner join 
                                                           ordermaster om on om.orderid=id.orderid
                                                           Where im.Approvalflag=1 and im.partyid=" + ddempname.SelectedValue + " AND im.Status <> 'Cancelled' and Im.status='" + status + "' and im.processid=" + ddProcessName.SelectedValue + " And im.CompanyId=" + ddCompName.SelectedValue + "", true, "Select Order No.");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref ddindentno, @"select distinct im.IndentId,Customercode+'/'+IndentNo+'/'+CustomerOrderNo as IndentNo from IndentMaster im inner join 
                                                           Indentdetail id on id.IndentID=im.IndentID inner join 
                                                           ordermaster om on om.orderid=id.orderid inner join Customerinfo CI on  CI.CustomerId=Om.CustomerId
                                                           Where im.Approvalflag=1 and im.partyid=" + ddempname.SelectedValue + " AND im.Status <> 'Cancelled' and Im.status='" + status + "' and im.processid=" + ddProcessName.SelectedValue + " And im.CompanyId=" + ddCompName.SelectedValue + "", true, "Select Order No");
            }
        }
        else
        {
            if (Session["WithoutBOM"].ToString() == "1")
            {
                UtilityModule.ConditionalComboFill(ref ddindentno, @"select distinct im.IndentId, IndentNo+'/'+CustomerOrderNo as IndentNo from IndentMaster im inner join 
                                                           Indentdetail id on id.IndentID=im.IndentID inner join 
                                                           ordermaster om on om.orderid=id.orderid
                                                           Where im.partyid=" + ddempname.SelectedValue + " AND im.Status <> 'Cancelled' and Im.status='" + status + "' and im.processid=" + ddProcessName.SelectedValue + " And im.CompanyId=" + ddCompName.SelectedValue + "", true, "Select Order No");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref ddindentno, @"select distinct im.IndentId,case When " + Session["varcompanyId"].ToString() + @"=9 Then om.localOrder+' / '+IndentNo Else IndentNo+'/'+CustomerOrderNo End as IndentNo from IndentMaster im inner join 
                                                           Indentdetail id on id.IndentID=im.IndentID inner join 
                                                           ordermaster om on om.orderid=id.orderid
                                                           Where im.partyid=" + ddempname.SelectedValue + " AND im.Status <> 'Cancelled' and Im.status='" + status + "' and im.processid=" + ddProcessName.SelectedValue + " And im.CompanyId=" + ddCompName.SelectedValue + "", true, "Select Order No");

            }
        }

    }
    protected void ddindentno_SelectedIndexChanged(object sender, EventArgs e)
    {
        IndentSelectedChange();
        btnpriview.Visible = true;
    }
    private void IndentSelectedChange()
    {

        if (Convert.ToInt32(Session["varcompanyNo"]) == 42)
        {
            UtilityModule.ConditionalComboFill(ref ddgodown, @"Select Distinct ID.GodownID, GM.GodownName  
                From IndentDetail ID(Nolock)
                JOIN GodownMaster GM(Nolock) ON GM.GoDownID = ID.GodownID 
                Where IndentId = " + ddindentno.SelectedValue + @" 
                Order By GM.GodownName ", true, "--Select--");
        }
        UtilityModule.ConditionalComboFill(ref DDChallan, "Select Distinct PRM.PRMid,ChallanNo from PP_ProcessRawMaster PRM inner join PP_ProcessRawTran PRT on PRM.PRMid=PRT.PRMid  where  PRT.IndentId=" + ddindentno.SelectedValue, true, "--Select--");
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Category();
    }
    private void Fill_Category()
    {

        if (Session["OrderWiseFlag"].ToString() == "1") // OrderWiseIndent
        {
            ddlcategorycangeOrderWise();
            UtilityModule.ConditionalComboFill(ref dditemname, @"SELECT DISTINCT dbo.ITEM_MASTER.ITEM_ID, dbo.ITEM_MASTER.ITEM_NAME FROM  dbo.ITEM_PARAMETER_MASTER INNER JOIN
            OrderLocalConsumption OC ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = OC.FinishedId INNER JOIN dbo.ITEM_MASTER ON dbo.ITEM_PARAMETER_MASTER.ITEM_ID = dbo.ITEM_MASTER.ITEM_ID
            WHERE OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ") AND dbo.ITEM_MASTER.CATEGORY_ID =" + ddCatagory.SelectedValue + " And ITEM_MASTER.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Item--");
        }
        else
        {
            ddlcategorycange();
            if (MySession.IndentAsProduction == "1")
            {
                UtilityModule.ConditionalComboFill(ref dditemname, @"select distinct vf.ITEM_ID,vf.ITEM_NAME from IndentDetail ID inner join PP_Consumption PP
                on ID.PPNo=PP.PPId and Id.oFinishedid=Pp.finishedid inner join V_FinishedItemDetail  vf on vf.ITEM_FINISHED_ID=pp.IFinishedid
                WHERE ID.IndentId =" + ddindentno.SelectedValue + " AND vf.CATEGORY_ID =" + ddCatagory.SelectedValue, true, "--Select Item--");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref dditemname, @"SELECT DISTINCT dbo.ITEM_MASTER.ITEM_ID, dbo.ITEM_MASTER.ITEM_NAME FROM  dbo.ITEM_PARAMETER_MASTER INNER JOIN
            dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
            dbo.ITEM_MASTER ON dbo.ITEM_PARAMETER_MASTER.ITEM_ID = dbo.ITEM_MASTER.ITEM_ID
            WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " AND dbo.ITEM_MASTER.CATEGORY_ID =" + ddCatagory.SelectedValue + " And ITEM_MASTER.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Item--");
            }
        }
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
                      " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCatagory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
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
                        if (MySession.IndentAsProduction == "1")
                        {
                            UtilityModule.ConditionalComboFill(ref dddesign, @"select distinct vf.designId,vf.designName from IndentDetail ID inner join PP_Consumption PP
                                on ID.PPNo=PP.PPId and Id.oFinishedid=Pp.finishedid inner join V_FinishedItemDetail  vf on vf.ITEM_FINISHED_ID=pp.IFinishedid
                                WHERE  ID.IndentId =" + ddindentno.SelectedValue + "  and vf.category_id=" + ddCatagory.SelectedValue, true, "--Select Design--");
                        }
                        else
                        {
                            UtilityModule.ConditionalComboFill(ref dddesign, @"SELECT DISTINCT dbo.Design.designId, dbo.Design.designName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                        dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                        dbo.Design ON dbo.ITEM_PARAMETER_MASTER.DESIGN_ID = dbo.Design.designId
                        WHERE  dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " And Design.MasterCompanyId=" + Session["varCompanyId"], true, "--Select Design--");
                        }
                        break;
                    case "3":
                        clr.Visible = true;
                        if (MySession.IndentAsProduction == "1")
                        {
                            UtilityModule.ConditionalComboFill(ref ddcolor, @"select distinct vf.ColorId,vf.ColorName from IndentDetail ID inner join PP_Consumption PP
                                on ID.PPNo=PP.PPId and Id.oFinishedid=Pp.finishedid inner join V_FinishedItemDetail  vf on vf.ITEM_FINISHED_ID=pp.IFinishedid
                                WHERE  ID.IndentId =" + ddindentno.SelectedValue + "  and vf.category_id=" + ddCatagory.SelectedValue, true, "--Select Color--");
                        }
                        else
                        {
                            UtilityModule.ConditionalComboFill(ref ddcolor, @"SELECT DISTINCT dbo.color.ColorId, dbo.color.ColorName FROm dbo.ITEM_PARAMETER_MASTER INNER JOIN
                        dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                        dbo.color ON dbo.ITEM_PARAMETER_MASTER.COLOR_ID = dbo.color.ColorId
                        WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " And Color.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Color--");
                        }
                        break;
                    case "4":
                        shp.Visible = true;
                        if (MySession.IndentAsProduction == "1")
                        {
                            UtilityModule.ConditionalComboFill(ref ddshape, @"select distinct vf.ShapeId,vf.ShapeName from IndentDetail ID inner join PP_Consumption PP
                                on ID.PPNo=PP.PPId and Id.oFinishedid=Pp.finishedid inner join V_FinishedItemDetail  vf on vf.ITEM_FINISHED_ID=pp.IFinishedid
                                WHERE  ID.IndentId =" + ddindentno.SelectedValue + "  and vf.category_id=" + ddCatagory.SelectedValue, true, "--Select Shape--");
                        }
                        else
                        {
                            UtilityModule.ConditionalComboFill(ref ddshape, @"SELECT DISTINCT dbo.Shape.ShapeId, dbo.Shape.ShapeName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                        dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                        dbo.Shape ON dbo.ITEM_PARAMETER_MASTER.SHAPE_ID = dbo.Shape.ShapeId
                        WHERE dbo.IndentDetail.IndentId = " + ddindentno.SelectedValue + " And shape.MasterCompanyId=" + Session["varCompanyId"], true, "--Select Shape--");
                        }
                        break;
                    case "5":
                        sz.Visible = true;
                        //ChkMtr.Checked = false;
                        if (MySession.IndentAsProduction == "1")
                        {
                            UtilityModule.ConditionalComboFill(ref ddsize, @"select distinct vf.SizeId,vf.SizeFt from IndentDetail ID inner join PP_Consumption PP
                                on ID.PPNo=PP.PPId and Id.oFinishedid=Pp.finishedid  inner join V_FinishedItemDetail  vf on vf.ITEM_FINISHED_ID=pp.IFinishedid
                                WHERE  ID.IndentId =" + ddindentno.SelectedValue + "  and vf.category_id=" + ddCatagory.SelectedValue, true, "--Select Size--");
                        }
                        else
                        {
                            UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size.SizeFt FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                        dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                        dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
                        WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " And Size.MasterCompanyId=" + Session["varCompanyId"], true, "Size in Ft");
                        }
                        break;
                    case "6":
                        shd.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddlshade, @"SELECT DISTINCT dbo.ShadeColor.ShadecolorId, dbo.ShadeColor.ShadeColorName FROM   dbo.ITEM_PARAMETER_MASTER INNER JOIN
                        dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                        dbo.ShadeColor ON dbo.ITEM_PARAMETER_MASTER.SHADECOLOR_ID = dbo.ShadeColor.ShadecolorId
                        WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " And shadeColor.MasterCompanyId=" + Session["varCompanyId"], true, "Select ShadeColor");
                        break;
                    case "10":
                        clr.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddcolor, @"SELECT DISTINCT dbo.color.ColorId, dbo.color.ColorName FROm dbo.ITEM_PARAMETER_MASTER INNER JOIN
                        dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                        dbo.color ON dbo.ITEM_PARAMETER_MASTER.COLOR_ID = dbo.color.ColorId
                        WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " And Color.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Color--");
                        break;
                }
            }
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        item_Change();
    }
    private void item_Change()
    {
        if (Session["OrderWiseFlag"].ToString() == "1") //OrderWiseFlag
        {
            CommanFunction.FillCombo(ddlunit, @"select distinct UnitId,UnitName from OrderLocalConsumption OC inner join Unit U on U.UnitId=OCD.IUnitId where OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ")");
            UtilityModule.ConditionalComboFill(ref dquality, @"SELECT DISTINCT dbo.Quality.QualityId,dbo.Quality.QualityName
            FROM  dbo.ITEM_PARAMETER_MASTER INNER JOIN OrderLocalConsumption OC ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = OC.FinishedId INNER JOIN
            dbo.Quality ON dbo.ITEM_PARAMETER_MASTER.QUALITY_ID = dbo.Quality.QualityId
            WHERE OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ") and quality.item_id=" + dditemname.SelectedValue + " And Quality.MasterCompanyId=" + Session["varCompanyId"], true, "Select Quallity");
        }
        else
        {
            if (Session["varCompanyId"].ToString() == "44")
            {
                CommanFunction.FillCombo(ddlunit, @"select Distinct UnitId,unitName From  Unit U,Item_Master IM where U.UnitTypeId=IM.UnitTypeId And ITEM_ID=" + dditemname.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + "");
            }
            else
            {
                CommanFunction.FillCombo(ddlunit, @"select distinct UnitId,UnitName from PP_Consumption PP inner join IndentDetail ID on ID.PPNo=PP.PPId inner Join IndentMaster IM on IM.IndentId=Id.IndentId inner join ORDER_CONSUMPTION_DETAIL OCD on OCD.OrderDetailId=PP.OrderDetailId inner join Unit U on U.UnitId=OCD.IUnitId where IndentNo='" + ddindentno.SelectedItem.Text + "'");
            }
            if (MySession.IndentAsProduction == "1")
            {
                UtilityModule.ConditionalComboFill(ref dquality, @"select distinct vf.QualityId,vf.QualityName from IndentDetail ID inner join PP_Consumption PP
                on ID.PPNo=PP.PPId  inner join V_FinishedItemDetail  vf on vf.ITEM_FINISHED_ID=pp.IFinishedid
                WHERE ID.IndentId =" + ddindentno.SelectedValue + "and vf.item_id=" + dditemname.SelectedValue, true, "Select Quallity");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref dquality, @"SELECT DISTINCT dbo.Quality.QualityId,dbo.Quality.QualityName
            FROM  dbo.ITEM_PARAMETER_MASTER INNER JOIN dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
            dbo.Quality ON dbo.ITEM_PARAMETER_MASTER.QUALITY_ID = dbo.Quality.QualityId
            WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + "and quality.item_id=" + dditemname.SelectedValue + " And Quality.MasterCompanyId=" + Session["varCompanyId"], true, "Select Quallity");
            }
        }
    }
    protected void save_detail()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[29];
            arr[0] = new SqlParameter("@prmid", SqlDbType.Int);
            arr[1] = new SqlParameter("@companyid", SqlDbType.Int);
            arr[2] = new SqlParameter("@empid", SqlDbType.Int);
            arr[3] = new SqlParameter("@processid", SqlDbType.Int);
            arr[4] = new SqlParameter("@issuedate", SqlDbType.DateTime);
            arr[5] = new SqlParameter("@chalanno", SqlDbType.NVarChar, 150);
            arr[6] = new SqlParameter("@varuserid", SqlDbType.Int);
            arr[7] = new SqlParameter("@varcompanyid", SqlDbType.Int);
            arr[8] = new SqlParameter("@prtid", SqlDbType.Int);
            arr[9] = new SqlParameter("@finishedId", SqlDbType.Int);
            arr[10] = new SqlParameter("@godownId", SqlDbType.Int);
            arr[11] = new SqlParameter("@issueQuantity", SqlDbType.Float);
            arr[12] = new SqlParameter("@indentid", SqlDbType.Int);
            arr[13] = new SqlParameter("@unitid", SqlDbType.Int);
            arr[14] = new SqlParameter("@lotno", SqlDbType.NVarChar, 50);
            arr[15] = new SqlParameter("@Finish_Type", SqlDbType.Int);
            arr[16] = new SqlParameter("@Remark", SqlDbType.NVarChar, 250);
            arr[17] = new SqlParameter("@Sizeflag", SqlDbType.Int);
            arr[18] = new SqlParameter("@CanQty", SqlDbType.Float);
            arr[19] = new SqlParameter("@TagNo", SqlDbType.NVarChar, 50);
            arr[20] = new SqlParameter("@BinNo", SqlDbType.VarChar, 50);
            arr[21] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            arr[22] = new SqlParameter("@ManualRate", SqlDbType.Float);
            arr[23] = new SqlParameter("@GSTType", SqlDbType.Int);
            arr[24] = new SqlParameter("@CGST", SqlDbType.Float);
            arr[25] = new SqlParameter("@SGST", SqlDbType.Float);
            arr[26] = new SqlParameter("@IGST", SqlDbType.Float);
            arr[27] = new SqlParameter("@Moisture", SqlDbType.Float);
            arr[28] = new SqlParameter("@BranchID", SqlDbType.Int);

            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = ViewState["Prmid"];
            arr[1].Value = ddCompName.SelectedValue;
            arr[2].Value = ddempname.SelectedValue;
            arr[3].Value = ddProcessName.SelectedValue;
            arr[4].Value = txtdate.Text;
            arr[5].Direction = ParameterDirection.InputOutput;
            arr[5].Value = DDChallan.SelectedItem.Text;
            arr[6].Value = Session["varuserid"].ToString();
            arr[7].Value = Session["varCompanyId"].ToString();
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, tran, ddlshade, "", Convert.ToInt32(Session["varCompanyId"]));
            arr[8].Direction = ParameterDirection.InputOutput;
            arr[8].Value = ViewState["Prtid"];
            arr[9].Value = Varfinishedid;
            arr[10].Value = ddgodown.SelectedValue;
            arr[11].Value = txtissqty.Text;
            arr[12].Value = ddindentno.SelectedValue;
            arr[13].Value = ddlunit.SelectedValue;
            arr[14].Value = ddlotno.SelectedItem.Text;
            arr[15].Value = 0;
            arr[16].Value = txtremarks.Text;
            arr[17].Value = DDsizetype.Visible == true ? DDsizetype.SelectedValue : "0";
            arr[18].Value = txtCanQty.Text == "" ? "0" : txtCanQty.Text;
            string TagNo;
            if (TDTagNo.Visible == true)
            {
                TagNo = DDTagNo.SelectedItem.Text;
            }
            else
            {
                TagNo = "Without Tag No";
            }
            arr[19].Value = TagNo;
            string BinNo = "";
            BinNo = TDBinNo.Visible == false ? "" : (DDBinNo.SelectedIndex > 0 ? DDBinNo.SelectedItem.Text : "");
            arr[20].Value = BinNo;
            arr[21].Direction = ParameterDirection.Output;

            arr[22].Value = TDManualRate.Visible == true ? TxtManualRate.Text == "" ? "0" : TxtManualRate.Text : "0";
            arr[23].Value = TDGSTType.Visible == true ? DDGSType.SelectedValue : "0";
            arr[24].Value = TDGSTType.Visible == true ? lblCGST.Text : "0";
            arr[25].Value = TDGSTType.Visible == true ? lblSGST.Text : "0";
            arr[26].Value = TDGSTType.Visible == true ? lblIGST.Text : "0";
            arr[27].Value = TDMoisture.Visible == true ? TxtMoisture.Text : "0";
            arr[28].Value = DDBranchName.SelectedValue;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[PRO_PP_PRM]", arr);
            ViewState["Prmid"] = arr[0].Value.ToString();
            ViewState["Prtid"] = arr[8].Value.ToString();
            
            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + arr[21].Value.ToString() + "');", true);

            Session["ReportPath"] = "Reports/IndentRawIssue.rpt";
            Session["CommanFormula"] = "{V_IndentRawIssue.PRMid}=" + ViewState["Prmid"] + "";
            ViewState["Prtid"] = 0;
            ViewState["FinishedID"] = 0;
            tran.Commit();
            Enable();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/EditIndentRowIssue.aspx");
            tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        pnl1.Enabled = false;
        btnsave.Text = "Save";
        Editflag = false;
        Fill_Grid();
        save_refresh();
    }
    private void CheckGSTType()
    {
        Label1.Visible = false;
        Label1.Text = "";
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select PRTid,GSTType from PP_ProcessRawTran where PRMid=" + ViewState["Prmid"] + "  order by PRTid");
        if (Ds.Tables[0].Rows.Count > 1)
        {
            if (DDGSType.SelectedValue != Ds.Tables[0].Rows[0]["GSTType"].ToString())
            {
                Label1.Visible = true;
                Label1.Text = "Please select same GST Type";
                return;
            }
        }
        FillGSTIGST();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Label1.Visible = false;
        if (Convert.ToInt32(Session["varCompanyId"]) != 16)
        {
            Validated();
        }
        if (Convert.ToInt32(Session["varCompanyId"]) == 5)
        {
            string ChkMsg = CheckStockQty();
            if (ChkMsg == "G")
            {
                Label1.Visible = true;
                Label1.Text = "Qty should not be greater than assigned stock";
                return;
            }
        }
        if (txtissqty.Text != "")
        {
            if (variable.VarGSTForIndentRawIssue == "1")
            {
                CheckGSTType();
            }
            else
            {
                Label1.Visible = false;
                Label1.Text = "";
            }
        }

        if (lblqty.Visible == false && Lblfinished.Visible == false && Label2.Visible == false && Label1.Visible == false)
        {
            save_detail();
        }
    }
    private void save_refresh()
    {
        ddlcategorycange();
        dditemname.SelectedIndex = 0;
        dquality.SelectedIndex = Td1.Visible == true ? 0 : -1;
        ddgodown.SelectedIndex = 0;
        txtissue.Text = "";
        txtCanQty.Text = "";
        txtpendingqty.Text = "";
        txtpreissue.Text = "";
        txtissqty.Text = "";
        txtstock.Text = "";
        ddlotno.SelectedIndex = 0;
        txtremarks.Text = "";
        TxtMoisture.Text = "";
    }
    private void raw_stock_update2()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[6];
            arr[0] = new SqlParameter("@Prtid", SqlDbType.Int);
            arr[1] = new SqlParameter("@TableName", SqlDbType.NVarChar, 150);
            arr[2] = new SqlParameter("@Qty", SqlDbType.Float);
            arr[3] = new SqlParameter("@TranDate", SqlDbType.DateTime);
            arr[4] = new SqlParameter("@RealDate", SqlDbType.DateTime);
            arr[5] = new SqlParameter("@UserId", SqlDbType.Int);

            arr[0].Value = ViewState["Prtid"];
            arr[1].Value = "PP_ProcessRawTran";
            arr[2].Value = txtissqty.Text;
            arr[3].Value = txtdate.Text;
            arr[4].Value = DateTime.Now;
            arr[5].Value = Session["varuserid"].ToString();
            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_UpdateStockIssue", arr);
            tran.Commit();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/EditIndentRowIssue.aspx");
            tran.Rollback();
        }
    }
    protected void btnnew_Click(object sender, EventArgs e)
    {
        refresh_form();
    }
    private void Fill_Grid()
    {
        gvdetail.DataSource = fill_Data_grid();
        gvdetail.DataBind();
        Session["ReportPath"] = "Reports/IndentRawIssue.rpt";
        Session["CommanFormula"] = "{V_IndentRawIssue.PRMid}=" + ViewState["Prmid"] + "";
    }
    private DataSet fill_Data_grid()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"SELECT prt.PRTid,prt.LotNo, icm.CATEGORY_NAME, im.ITEM_NAME, gm.GodownName, 
            prt.IssueQuantity,IPM1.QDCS + Space(2)+isnull(SizeMtr,'') DESCRIPTION,CanQty,prt.tagno,prt.BinNo  FROM pp_ProcessRawTran prt  INNER JOIN ITEM_PARAMETER_MASTER ipm ON prt.Finishedid = ipm.ITEM_FINISHED_ID INNER JOIN
            GodownMaster gm ON prt.Godownid = gm.GoDownID INNER JOIN  item_master im on im.item_id=ipm.item_id inner join iTEM_CATEGORY_MASTER icm ON im.Category_id = icm.CATEGORY_ID inner join
            PP_ProcessRawMaster prm on  prt.prmid=prm.prmid inner Join ViewFindFinishedidItemidQDCSS IPM1 on IPM.Item_Finished_Id=IPM1.Finishedid where PRM.PRMid=" + DDChallan.SelectedValue + " and prt.indentid=" + ddindentno.SelectedValue + "  And im.MasterCompanyId=" + Session["varCompanyId"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/EditIndentRowIssue.aspx");
            Logs.WriteErrorLog("Masters_process_ProcessRawIssue|fill_Data_grid|" + ex.Message);
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
    protected void gvdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvdetail, "Select$" + e.Row.RowIndex);

            for (int i = 0; i < gvdetail.Columns.Count; i++)
            {
                if (variable.VarBINNOWISE == "1")
                {
                    if (gvdetail.Columns[i].HeaderText.ToUpper() == "BIN NO.")
                    {
                        gvdetail.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (gvdetail.Columns[i].HeaderText.ToUpper() == "BIN NO.")
                    {
                        gvdetail.Columns[i].Visible = false;
                    }

                }
            }
        }

    }
    protected void gvdetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvdetail.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    private int save_finishedid()
    {
        try
        {
            int VarOutfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            ItemFinishedId = VarOutfinishedid;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/EditIndentRowIssue.aspx");
        }
        return ItemFinishedId;
    }
    private void Disable()
    {
        ddCatagory.Enabled = false;
        dditemname.Enabled = false;
        dquality.Enabled = false;
        dddesign.Enabled = false;
        ddcolor.Enabled = false;
        ddshape.Enabled = false;
        ddsize.Enabled = false;
        ddlshade.Enabled = false;
        ddgodown.Enabled = false;
        ddlotno.Enabled = false;
        DDBinNo.Enabled = false;
        DDTagNo.Enabled = false;
        //ddlunit.Enabled = false;
        //btndelete.Enabled = true;
    }
    private void Enable()
    {
        ddCatagory.Enabled = true;
        dditemname.Enabled = true;
        dquality.Enabled = true;
        dddesign.Enabled = true;
        ddcolor.Enabled = true;
        ddshape.Enabled = true;
        ddsize.Enabled = true;
        ddlshade.Enabled = true;
        ddgodown.Enabled = true;
        ddlotno.Enabled = true;
        ddlunit.Enabled = true;
        DDBinNo.Enabled = true;
        DDTagNo.Enabled = true;
        //btndelete.Enabled = false;
    }
    protected void gvdetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            ds = null;
            Editflag = true;
            string sql = @" select distinct PRT.GodownId,PRT.FinishedId,Stock.StockId,ICM.Category_Id,IM.Item_Id,
                         Quality_Id,Design_Id,Color_Id,ShadeColor_Id,Shape_Id,Size_Id,PRM.PRMid,GatePassNo,IssueQuantity,PRT.UnitId,prt.flagsize,CanQty,PRT.LotNo,PRT.TagNo,isnull(Prt.BinNo,'') as BinNo
                        ,isnull(PRT.ManualRate,0) as ManualRate,isnull(PRT.GSTType,0) as GSTType,isnull(PRT.CGST,0) as CGST,isnull(PRT.SGST,0) as SGST,isnull(PRT.IGST,0) as IGST from PP_ProcessRawMaster PRM inner join 
                         PP_ProcessRawTran PRT on PRM.PRMId=PRT.PRMid inner join Stock on Stock.LotNo=PRT.LotNo inner join Item_Parameter_Master IPM on 
                         PRT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IM.Item_Id=IPM.Item_Id inner join Item_Category_Master ICM on 
                         ICM.Category_Id=IM.Category_Id inner join StockTran on StockTran.Stockid=Stock.Stockid And StockTran.PrtId=Prt.PrtId where PRT.PrtId=" + gvdetail.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddCatagory.SelectedValue = ds.Tables[0].Rows[0]["Category_Id"].ToString();
                Fill_Category();
                dditemname.SelectedValue = ds.Tables[0].Rows[0]["Item_Id"].ToString();
                item_Change();
                if (ql.Visible == true)
                {
                    dquality.SelectedValue = ds.Tables[0].Rows[0]["Quality_Id"].ToString();
                }
                if (dsn.Visible == true)
                {
                    dddesign.SelectedValue = ds.Tables[0].Rows[0]["Design_Id"].ToString();
                }
                if (clr.Visible == true)
                {
                    ddcolor.SelectedValue = ds.Tables[0].Rows[0]["Color_Id"].ToString();
                }
                if (shd.Visible == true)
                {
                    ddlshade.SelectedValue = ds.Tables[0].Rows[0]["ShadeColor_Id"].ToString();
                }
                if (shp.Visible == true)
                {
                    ddshape.SelectedValue = ds.Tables[0].Rows[0]["Shape_Id"].ToString();
                    DDsizetype.SelectedValue = ds.Tables[0].Rows[0]["flagsize"].ToString();
                    fillSize();
                }
                if (sz.Visible == true)
                {
                    ddsize.SelectedValue = ds.Tables[0].Rows[0]["Size_Id"].ToString();
                }
                ddgodown.SelectedValue = ds.Tables[0].Rows[0]["GodownId"].ToString();
                ddgodown_SelectedIndexChanged(sender, new EventArgs());

                if (ddlotno.Items.FindByValue(ds.Tables[0].Rows[0]["Lotno"].ToString()) != null)
                {
                    ddlotno.SelectedValue = ds.Tables[0].Rows[0]["Lotno"].ToString();
                    ddlotno_SelectedIndexChanged(sender, new EventArgs());
                    if (TDBinNo.Visible == true)
                    {
                        if (DDBinNo.Items.FindByValue(ds.Tables[0].Rows[0]["BinNo"].ToString()) != null)
                        {
                            DDBinNo.SelectedValue = ds.Tables[0].Rows[0]["BinNo"].ToString();
                            DDBinNo_SelectedIndexChanged(sender, new EventArgs());
                            if (TDTagNo.Visible == true)
                            {
                                if (DDTagNo.Items.FindByValue(ds.Tables[0].Rows[0]["TagNo"].ToString()) != null)
                                {
                                    DDTagNo.SelectedValue = ds.Tables[0].Rows[0]["TagNo"].ToString();
                                    DDTagNo_SelectedIndexChanged(sender, new EventArgs());
                                }
                            }
                        }
                    }
                    else
                    {
                        if (TDTagNo.Visible == true)
                        {
                            if (DDTagNo.Items.FindByValue(ds.Tables[0].Rows[0]["TagNo"].ToString()) != null)
                            {
                                DDTagNo.SelectedValue = ds.Tables[0].Rows[0]["TagNo"].ToString();
                                DDTagNo_SelectedIndexChanged(sender, new EventArgs());
                            }
                        }
                    }
                }
                if (ddlunit.Items.FindByValue(ds.Tables[0].Rows[0]["Unitid"].ToString()) != null)
                {
                    ddlunit.SelectedValue = ds.Tables[0].Rows[0]["Unitid"].ToString();
                }
                int Varfinishedid = Convert.ToInt32(ds.Tables[0].Rows[0]["FinishedId"]);
                ViewState["FinishedID"] = Varfinishedid;
                //  UtilityModule.ConditionalComboFill(ref ddlotno, "select lotno,lotno from stock where godownid=" + ddgodown.SelectedValue + "and item_finished_id=" + Varfinishedid, true, "--Select--");

                txtissqty.Text = ds.Tables[0].Rows[0]["IssueQuantity"].ToString();
                txtCanQty.Text = ds.Tables[0].Rows[0]["CanQty"].ToString();
                //double pending = Convert.ToDouble(txtissue.Text) - Convert.ToDouble(txtpreissue.Text);
                //txtpendingqty.Text = pending.ToString();

                if (TDLotNo.Visible == true)
                {
                    ddlotno.SelectedValue = ds.Tables[0].Rows[0]["LotNo"].ToString();
                }
                if (TDTagNo.Visible == true)
                {
                    UtilityModule.ConditionalComboFill(ref DDTagNo, "select TagNo,TagNo From PP_ProcessRawTran  Where PRTid=" + gvdetail.SelectedValue + "", true, "Select Tagno");
                    if (DDTagNo.Items.FindByValue(ds.Tables[0].Rows[0]["TagNo"].ToString()) != null)
                    {
                        DDTagNo.SelectedValue = ds.Tables[0].Rows[0]["TagNo"].ToString();
                    }
                }
                double qty;
                if (Session["OrderWiseFlag"].ToString() == "1")
                {
                    qty = Convert.ToDouble(SqlHelper.ExecuteScalar(con, CommandType.Text, @"SELECT  SUM(qtyinhand) as qtyinhand FROM stock where item_finished_id=" + Varfinishedid + " and godownid=" + ddgodown.SelectedValue + " group by item_finished_id"));
                }
                else
                {
                    qty = UtilityModule.getstockQty(ddCompName.SelectedValue, ddgodown.SelectedValue, ds.Tables[0].Rows[0]["Lotno"].ToString(), Varfinishedid, ds.Tables[0].Rows[0]["TagNo"].ToString(), ds.Tables[0].Rows[0]["BinNo"].ToString());

                }
                if (qty > 0)
                {
                    txtstock.Text = (qty + Convert.ToDouble(txtissqty.Text)).ToString();
                }
                else
                {
                    txtstock.Text = "0";
                }
                txtpreissue.Text = Convert.ToString(Math.Round(Convert.ToDecimal(txtpreissue.Text) - Convert.ToDecimal(txtissqty.Text), 3));
                if (TDManualRate.Visible == true)
                {
                    TxtManualRate.Text = ds.Tables[0].Rows[0]["ManualRate"].ToString();
                }
                if (TDGSTType.Visible == true)
                {
                    DDGSType.SelectedValue = ds.Tables[0].Rows[0]["GSTType"].ToString();
                    if (DDGSType.SelectedValue == "1")
                    {
                        TDCGST.Visible = true;
                        TDSGST.Visible = true;
                        lblCGST.Text = ds.Tables[0].Rows[0]["CGST"].ToString();
                        lblSGST.Text = ds.Tables[0].Rows[0]["SGST"].ToString();
                    }
                    else if (DDGSType.SelectedValue == "2")
                    {
                        TDIGST.Visible = true;
                        lblIGST.Text = ds.Tables[0].Rows[0]["IGST"].ToString();
                    }
                    else
                    {
                        lblCGST.Text = "";
                        lblSGST.Text = "";
                        lblIGST.Text = "";
                    }

                }

                ViewState["Prmid"] = ds.Tables[0].Rows[0]["Prmid"].ToString(); ;
                ViewState["Prtid"] = gvdetail.SelectedValue;
                Disable();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/EditIndentRowIssue.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        btnsave.Text = "Update";
        //btndelete.Visible = true;
    }
    protected void ChkMtr_CheckedChanged(object sender, EventArgs e)
    {
        //if (ChkMtr.Checked == false)
        //{
        string strSize;

        if (DDsizetype.SelectedIndex == 0)
        {
            strSize = " Sizeft";
        }
        else if (DDsizetype.SelectedIndex == 1)
        {
            strSize = "Sizemtr";
        }
        else
        {
            strSize = "Sizeinch";
        }
        UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size." + strSize + @" FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
            dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
            dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
            WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"], true, "Size in Ft");
        //        }
        //        else
        //        {
        //            UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size.Sizemtr FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
        //            dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
        //            dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
        //            WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"], true, "Size in mtr");
        //        }
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds;
        string Str;
        if (TxtProdCode.Text != "")
        {
            ddCatagory.SelectedIndex = 0;
            Str = @" SELECT DISTINCT 
                  dbo.IndentDetail.IndentDetailId, dbo.IndentDetail.IndentId, dbo.ITEM_MASTER.CATEGORY_ID, dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID, 
                  dbo.ITEM_PARAMETER_MASTER.QUALITY_ID, dbo.ITEM_PARAMETER_MASTER.DESIGN_ID, dbo.ITEM_PARAMETER_MASTER.COLOR_ID, 
                  dbo.ITEM_PARAMETER_MASTER.SHAPE_ID, dbo.ITEM_PARAMETER_MASTER.SIZE_ID, dbo.ITEM_PARAMETER_MASTER.DESCRIPTION, 
                  dbo.ITEM_PARAMETER_MASTER.ITEM_ID, dbo.ITEM_PARAMETER_MASTER.ProductCode, dbo.ITEM_PARAMETER_MASTER.SHADECOLOR_ID  FROM  dbo.ITEM_MASTER INNER JOIN
                  dbo.ITEM_PARAMETER_MASTER ON dbo.ITEM_MASTER.ITEM_ID = dbo.ITEM_PARAMETER_MASTER.ITEM_ID INNER JOIN
                  dbo.IndentDetail INNER JOIN dbo.IndentMaster ON dbo.IndentDetail.IndentId = dbo.IndentMaster.IndentID ON 
                  dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId where productcode='" + TxtProdCode.Text + "' And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"] + " And INDENTMASTER.CompanyId=" + ddCompName.SelectedValue;
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                UtilityModule.ConditionalComboFill(ref ddCatagory, @"  SELECT DISTINCT 
                dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID AS Expr1, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME, dbo.IndentDetail.IFinishedId FROM  dbo.ITEM_MASTER INNER JOIN
                dbo.ITEM_PARAMETER_MASTER ON dbo.ITEM_MASTER.ITEM_ID = dbo.ITEM_PARAMETER_MASTER.ITEM_ID INNER JOIN
                dbo.ITEM_CATEGORY_MASTER ON dbo.ITEM_MASTER.CATEGORY_ID = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID INNER JOIN
                dbo.IndentDetail INNER JOIN dbo.IndentMaster ON dbo.IndentDetail.IndentId = dbo.IndentMaster.IndentID ON 
                dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId
                Where dbo.IndentMaster.partyid=" + ddempname.SelectedValue + " and IndentMaster.processid=" + ddProcessName.SelectedValue + " and dbo.IndentDetail.IndentId=" + ddindentno.SelectedValue + " And IndentMaster.CompanyId=" + ddCompName.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"], true, "Select Category Name");
                ddCatagory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                string Qry = @"Select Distinct Item_Id,Item_Name from Item_Master where Category_Id=" + ddCatagory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
                Qry = Qry + " select qualityid,qualityname from quality where MasterCompanyId=" + Session["varCompanyId"] + " ANd item_id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["ITEM_ID"].ToString());
                Qry = Qry + " select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varCompanyId"] + " Order  by DesignName  ";
                Qry = Qry + " SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varCompanyId"] + " order by colorid ";
                Qry = Qry + " select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " Order by Shapeid ";
                Qry = Qry + " SELECT SIZEID,SIZEFT fROM SIZE WhERE MasterCompanyId=" + Session["varCompanyId"] + " ANd SHAPEID=" + Convert.ToInt32(ds.Tables[0].Rows[0]["SHAPE_ID"].ToString()) + "";
                Qry = Qry + " select shadecolorid,shadecolorname from shadecolor Where MasterCompanyId=" + Session["varCompanyId"] + " order by shadecolorid ";
                DataSet QDS = SqlHelper.ExecuteDataset(Qry);

                UtilityModule.ConditionalComboFillWithDS(ref dditemname, QDS, 0, true, "--Select Item--");
                dditemname.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref dquality, QDS, 1, true, "Select Quallity");
                dquality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref dddesign, QDS, 2, true, "--Select Design--");
                dddesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddcolor, QDS, 3, true, "--Select Color--");
                ddcolor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddshape, QDS, 4, true, "--Select Shape--");
                ddshape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddsize, QDS, 5, true, "--SELECT SIZE--");
                ddsize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddlshade, QDS, 6, true, "Select ShadeColor");
                ddlshade.SelectedValue = ds.Tables[0].Rows[0]["SHADECOLOR_ID"].ToString();

                Session["finishedid"] = ds.Tables[0].Rows[0]["Item_Finished_id"].ToString();
                Session["indentid"] = ds.Tables[0].Rows[0]["IndentId"].ToString();
                Session["IndentdetailId"] = ds.Tables[0].Rows[0]["IndentDetailId"].ToString();
                int q = (dquality.SelectedIndex > 0 ? Convert.ToInt32(dquality.SelectedValue) : 0);
                if (q > 0)
                {
                    ql.Visible = true;
                }
                else
                {
                    ql.Visible = false;
                }
                if (Convert.ToInt32(dddesign.SelectedValue) > 0)
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
                int sd = (ddlshade.SelectedIndex > 0 ? Convert.ToInt32(ddlshade.SelectedValue) : 0);
                if (sd > 0)
                {
                    shd.Visible = true;
                }
                else
                {
                    shd.Visible = false;
                }
                CommanFunction.FillCombo(ddlunit, @"select distinct UnitId,UnitName from PP_Consumption PP inner join IndentDetail ID on ID.PPNo=PP.PPId inner Join IndentMaster IM on IM.IndentId=Id.IndentId inner join ORDER_CONSUMPTION_DETAIL OCD on OCD.OrderDetailId=PP.OrderDetailId inner join Unit U on U.UnitId=OCD.IUnitId where IndentNo='" + ddindentno.SelectedItem.Text + "' And u.MasterCompanyId=" + Session["varCompanyId"] + "");
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
            ddCatagory.SelectedIndex = 0;
        }
    }
    protected void ddgodown_SelectedIndexChanged(object sender, EventArgs e)
    {

        gowdownchange();
        switch (Session["varcompanyNo"].ToString())
        {
            case "10":
                ddlotno_SelectedIndexChanged(sender, e);
                break;
        }


    }
    private void gowdownchange()
    {
        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        string str = "";
        if (variable.Carpetcompany == "1")
        {
            if (variable.VarGENRATEINDENTWITHOUTLOT_STOCK == "1")
            {
                str = "select Distinct Lotno,Lotno From stock WHere Companyid=" + ddCompName.SelectedValue + " and Godownid=" + ddgodown.SelectedValue + " and ITEM_FINISHED_ID=" + Varfinishedid;
                if (MySession.Stockapply == "True" && Editflag == false)
                {
                    str = str + " and Round(Qtyinhand,3)>0";
                }
            }
            else
            {
                str = "SELECT Distinct LotNo,LotNo FROM IndentDetail Where IndentId=" + ddindentno.SelectedValue + " and Ifinishedid=" + Varfinishedid + "";
            }
            UtilityModule.ConditionalComboFill(ref ddlotno, str, true, "Select LotNo");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddlotno, "SELECT Distinct LotNo,LotNo FROM IndentDetail Where IndentId=" + ddindentno.SelectedValue + "", true, "Select LotNo");
        }
        SqlParameter[] _array = new SqlParameter[7];
        _array[0] = new SqlParameter("@Finishedid", Varfinishedid);
        _array[1] = new SqlParameter("@indentId", ddindentno.SelectedValue);
        _array[2] = new SqlParameter("@StockQty", SqlDbType.Float);
        _array[3] = new SqlParameter("@IndentQty", SqlDbType.Float);
        _array[4] = new SqlParameter("@PreIssueQty", SqlDbType.Float);
        _array[5] = new SqlParameter("@PendingQty", SqlDbType.Float);
        _array[6] = new SqlParameter("@varcompanyType", SqlDbType.Int);
        _array[2].Direction = ParameterDirection.Output;
        _array[3].Direction = ParameterDirection.Output;
        _array[4].Direction = ParameterDirection.Output;
        _array[5].Direction = ParameterDirection.Output;
        _array[6].Direction = ParameterDirection.Output;

        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_FillIndent_Issue_Qty", _array);
        if (_array[6].Value.ToString() == "7")
        {
            txtstock.Text = "0";
            if (Convert.ToDouble(_array[2].Value) > 0)
            {
                txtstock.Text = _array[2].Value.ToString();//For Stock
            }
        }
        else
        {
            txtstock.Text = "0";
            txtissue.Text = _array[3].Value.ToString();//IndentQty
            txtpreissue.Text = _array[4].Value.ToString();//PreIssue
            txtpendingqty.Text = _array[5].Value.ToString();//PendingQty
        }

        //}
    }

    private void refresh_form()
    {
        ViewState["Prtid"] = 0;
        pnl1.Enabled = true;
        ddempname.SelectedValue = null;
        ddProcessName.SelectedValue = null;
        ddindentno.SelectedValue = null;
        ddempname.SelectedValue = null;
        ddCatagory.SelectedValue = null;
        dditemname.SelectedValue = null;
        ddgodown.SelectedValue = null;
        txtissue.Text = "";
        dquality.SelectedValue = null;
        dddesign.SelectedValue = null;
        ddcolor.SelectedValue = null;
        ddshape.SelectedValue = null;
        ddsize.SelectedValue = null;
        ddlshade.SelectedValue = null;
        txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        TxtProdCode.Text = "";
        btnsave.Text = "Save";
        lbldate.Visible = false;
        Label1.Visible = false;
        Lblfinished.Visible = false;
        //btndelete.Visible = false;
    }
    private void Validated()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            string strsql, BinNo, TagNo;
            if (txtCanQty.Text != "" && txtCanQty.Text != "0")
            {
                if (Convert.ToDouble(txtCanQty.Text) > Convert.ToDouble(txtissqty.Text))
                {
                    Label1.Visible = true;
                    Label1.Text = "Cancel Qty Can not be greater than IssueQty...";
                    txtCanQty.Text = "0";
                    return;
                }
            }
            //****************
            if (TDBinNo.Visible == true)
            {
                BinNo = DDBinNo.SelectedItem.Text;
            }
            else
            {
                BinNo = "";
            }
            if (TDTagNo.Visible == true)
            {
                TagNo = DDTagNo.SelectedItem.Text;
            }
            else
            {
                TagNo = "Without Tag No";
            }
            strsql = "select finishedid from pp_processrawtran where finishedid=" + Varfinishedid + " and Prtid != " + ViewState["Prtid"] + " and prmid =" + ViewState["Prmid"] + " and indentid=" + ddindentno.SelectedValue + " And Lotno='" + ddlotno.SelectedItem.Text + "' and BinNo='" + BinNo + "' and TagNo='" + TagNo + "'";
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);

            if (Session["varCompanyNo"].ToString() != "42")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Lblfinished.Visible = true;
                }
                else
                {
                    Lblfinished.Visible = false;
                }
            }
           
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/EditIndentRowIssue.aspx");
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
    protected void txtissqty_TextChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Session["varCompanyId"]) == 5)
        {
            string ChkMsg = CheckStockQty();
            if (ChkMsg == "G")
            {
                Label1.Visible = true;
                Label1.Text = "Qty should not be greater than assigned stock";
                return;
            }
        }
        if (Session["OrderWiseFlag"].ToString() == "1")
        {
            if (Convert.ToDouble(txtissqty.Text) > Convert.ToDouble(txtstock.Text))
            {
                lblqty.Visible = true;
            }
            else
            {
                lblqty.Visible = false;
            }
        }
        else
        {
            double indentQty = Convert.ToDouble(txtissue.Text == "" ? "0" : txtissue.Text);


            double VarPercentQty = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select PercentageExecssQtyForIndent from MasterSetting"));
            double varTotalIssQty = indentQty + (indentQty * VarPercentQty / 100);
            double varpreissueQty = Convert.ToDouble(txtpreissue.Text == "" ? "0" : txtpreissue.Text);
            //double PendingQty = Convert.ToDouble(txtpendingqty.Text) + (Convert.ToDouble(txtpendingqty.Text) * VarPercentQty / 100);
            double PendingQty = varTotalIssQty - (varpreissueQty - varissuedQty);
            if (Convert.ToDouble(txtissqty.Text == "" ? "0" : txtissqty.Text) > PendingQty)
            {
                lblqty.Visible = true;
                lblqty.Text = lblqty.Text + " i.e " + PendingQty.ToString();
            }
            else
            {
                lblqty.Visible = false;
            }
        }
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Session.Remove("finishedid");
        Session.Remove("indentid");
        Session.Remove("indentdetailid");
    }

    private string CheckStockQty()
    {
        string str = "";
        try
        {
            SqlParameter[] parparam = new SqlParameter[7];
            parparam[0] = new SqlParameter("@IndentID", ddindentno.SelectedValue);
            parparam[1] = new SqlParameter("@FinishedID", ViewState["FinishedID"]);
            parparam[2] = new SqlParameter("@TxtQty", txtissqty.Text);
            parparam[3] = new SqlParameter("@PrtId", ViewState["Prtid"]);
            parparam[4] = new SqlParameter("@Message", SqlDbType.NVarChar, 2);
            parparam[5] = new SqlParameter("@LotNo", ddlotno.SelectedItem.Text);
            parparam[6] = new SqlParameter("@CompanyId", ddCompName.SelectedValue);
            parparam[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_CheckStockQty", parparam);
            str = parparam[4].Value.ToString();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Masters/RawMaterial/IndentRawIssue");
        }
        return str;
    }
    protected void ddlotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        if (TDBinNo.Visible == true)
        {

            string str = "select Distinct BinNo,BinNo as BinNo1 from Stock Where Item_Finished_id=" + Varfinishedid + " and Godownid=" + ddgodown.SelectedValue + " and CompanyId=" + ddCompName.SelectedValue + "  and Round(Qtyinhand,3)>0 and LotNo='" + ddlotno.SelectedItem.Text + "' order by BinNo";
            UtilityModule.ConditionalComboFill(ref DDBinNo, str, true, "--Plz Select--");
            DDBinNo.SelectedIndex = -1;
            if (DDBinNo.Items.Count > 0)
            {
                DDBinNo.SelectedIndex = 1;
                DDBinNo_SelectedIndexChanged(sender, new EventArgs());
            }
            else
            {
                DDBinNo_SelectedIndexChanged(sender, new EventArgs());
            }
        }
        else
        {
            FillTagNo(Varfinishedid, sender);

        }

    }
    protected void FillTagNo(int Varfinishedid, object sender = null)
    {

        string str = "";
        if (TDTagNo.Visible == true)
        {
            DDTagNo.SelectedIndex = -1;
            str = "select Distinct TagNo,TagNo From Stock  Where Companyid=" + ddCompName.SelectedValue + " and Godownid=" + ddgodown.SelectedValue + " and LotNo='" + ddlotno.SelectedItem.Text + "' and ITEM_FINISHED_ID=" + Varfinishedid;
            if (MySession.Stockapply == "True")
            {
                str = str + " and Round(Qtyinhand,3)>0";
            }
            if (TDBinNo.Visible == true)
            {
                str = str + " and BinNo='" + (DDBinNo.SelectedIndex > 0 ? DDBinNo.SelectedItem.Text : "") + "'";
            }
            UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "Select Tagno");
            if (DDTagNo.Items.Count > 0)
            {
                if (sender != null)
                {
                    DDTagNo.SelectedIndex = 1;
                    DDTagNo_SelectedIndexChanged(sender, new EventArgs());
                }
            }
        }
        else
        {
            getstockQty(Varfinishedid);
        }
        // double qty = 0;

    }
    protected void getstockQty(int Varfinishedid)
    {
        double qty = 0;
        string Lotno;
        string TagNo;
        string BinNo = "";
        if (TDLotNo.Visible == true)
        {
            Lotno = ddlotno.SelectedItem.Text;
        }
        else
        {
            Lotno = "Without Lot No";
        }
        if (TDTagNo.Visible == true)
        {
            if (DDTagNo.Items.Count > 0)
            {
                TagNo = DDTagNo.SelectedItem.Text;
            }
            else
            {
                TagNo = "Without Tag No";
            }
        }
        else
        {
            TagNo = "Without Tag No";
        }
        if (TDBinNo.Visible == true)
        {
            if (DDBinNo.Items.Count > 0)
            {
                BinNo = DDBinNo.SelectedItem.Text;
            }
            else
            {
                BinNo = "";
            }
        }
        ///******get stock qty
        qty = UtilityModule.getstockQty(ddCompName.SelectedValue, ddgodown.SelectedValue, Lotno, Varfinishedid, TagNo, BinNo: BinNo);
        if (qty > 0)
        {
            txtstock.Text = qty.ToString();
        }
        else
        {
            txtstock.Text = "0";
        }
    }
    protected void DDChallan_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChallanNoSelectedChange();
    }
    private void ChallanNoSelectedChange()
    {
        string str = "select IsNull(OrderWiseFlag,0) As OrderWiseFlag  From IndentMaster where IndentId=" + ddindentno.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["OrderWiseFlag"] = ds.Tables[0].Rows[0]["OrderWiseFlag"].ToString();

            string str1 = "select top(1) isnull(Remark,'') as Remark from PP_ProcessRawTran where PRMid=" + DDChallan.SelectedValue + " and remark<>''";
            DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str1);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                txtremarks.Text = ds1.Tables[0].Rows[0]["Remark"].ToString();
            }

            if (Convert.ToInt32(ds.Tables[0].Rows[0]["OrderWiseFlag"]) == 1)
            {
                TDLotNo.Visible = false;
                TDPreIssue.Visible = false;
                TDtxtissue.Visible = false;
                TDtxtpendingqty.Visible = false;
                UtilityModule.ConditionalComboFill(ref ddCatagory, @"  SELECT DISTINCT icm.CATEGORY_ID , icm.CATEGORY_NAME  FROM ITEM_MASTER im INNER JOIN
                  ITEM_PARAMETER_MASTER ipm ON im.ITEM_ID = ipm.ITEM_ID INNER JOIN ITEM_CATEGORY_MASTER icm ON im.CATEGORY_ID = icm.CATEGORY_ID INNER JOIN
                  IndentDetail  id INNER JOIN   IndentMaster idm ON id.IndentId = idm.IndentID Inner Join OrderLocalConsumption OC ON ID.OrderId=OC.OrderId   ON  ipm.ITEM_FINISHED_ID = OC.FinishedId
                  Where idm.partyid=" + ddempname.SelectedValue + " and idm.processid=" + ddProcessName.SelectedValue + " and  idm.IndentId=" + ddindentno.SelectedValue + " And im.MasterCompanyId=" + Session["varCompanyId"] + " And idm.CompanyId=" + ddCompName.SelectedValue, true, "Select Category Name");
                CommanFunction.FillCombo(ddlunit, @"Select DISTINCT U.UnitId,UnitName from IndentDetail ID,ORDERLocalConsumption OCD,Unit U
                  Where U.UnitId=OCD.UnitId  And ID.OrderId=OCD.OrderId
                  And Id.IndentId=" + ddindentno.SelectedValue + " And U.MasterCompanyId=" + Session["varCompanyId"]);
                Fill_Grid();
            }
            else
            {
                TDLotNo.Visible = true;
                TDPreIssue.Visible = true;
                TDtxtissue.Visible = true;
                TDtxtpendingqty.Visible = true;
                if (MySession.IndentAsProduction == "1")
                {
                    UtilityModule.ConditionalComboFill(ref ddCatagory, @"  select distinct vf.CATEGORY_ID,vf.CATEGORY_NAME from indentmaster idm inner join IndentDetail ID on idm.indentid=id.indentid inner join PP_Consumption PP
                    on ID.PPNo=PP.PPId and Id.oFinishedid=Pp.finishedid  inner join V_FinishedItemDetail  vf on vf.ITEM_FINISHED_ID=pp.IFinishedid
                 Where idm.partyid=" + ddempname.SelectedValue + " and idm.processid=" + ddProcessName.SelectedValue + " and  idm.IndentId=" + ddindentno.SelectedValue, true, "Select Category Name");
                }
                else
                {
                    UtilityModule.ConditionalComboFill(ref ddCatagory, @"  SELECT DISTINCT icm.CATEGORY_ID , icm.CATEGORY_NAME  FROM ITEM_MASTER im INNER JOIN
                    ITEM_PARAMETER_MASTER ipm ON im.ITEM_ID = ipm.ITEM_ID INNER JOIN ITEM_CATEGORY_MASTER icm ON im.CATEGORY_ID = icm.CATEGORY_ID INNER JOIN
                    IndentDetail  id INNER JOIN   IndentMaster idm ON id.IndentId = idm.IndentID ON  ipm.ITEM_FINISHED_ID = id.IFinishedId
                    Where idm.partyid=" + ddempname.SelectedValue + " and idm.processid=" + ddProcessName.SelectedValue + " and  idm.IndentId=" + ddindentno.SelectedValue + " And im.MasterCompanyId=" + Session["varCompanyId"] + " And idm.CompanyId= " + ddCompName.SelectedValue, true, "Select Category Name");
                }
                CommanFunction.FillCombo(ddlunit, @"Select DISTINCT U.UnitId,UnitName from PP_Consumption PP,IndentDetail ID,ORDER_CONSUMPTION_DETAIL OCD,Unit U,ProcessProgram P
                    Where ID.PPNo=PP.PPId And OCD.OrderDetailId=PP.OrderDetailId And U.UnitId=OCD.IUnitId And P.PPID=PP.PPId And OCD.ProcessiD=P.Process_iD
                    And Id.IndentId=" + ddindentno.SelectedValue + " And u.MasterCompanyId=" + Session["varCompanyId"] + "");
                Fill_Grid();
            }
        }
        Session["ReportPath"] = "Reports/IndentRawIssue.rpt";
        Session["CommanFormula"] = "{V_IndentRawIssue.PRMid}=" + DDChallan.SelectedValue + "";
        btnpriview.Visible = true;
        ViewState["Prmid"] = DDChallan.SelectedValue;
        //Report();
    }
    protected void txtidnt_TextChanged(object sender, EventArgs e)
    {
        Label1.Text = "";
        if (txtidnt.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            try
            {
                string str = @"Select Distinct PM.CompanyId,PM.ProcessID,PM.Empid,PT.IndentId,replace(convert(varchar(11),PM.Date,106), ' ','-') Date,PM.ChallanNo From PP_ProcessRawMaster PM,PP_ProcessRawTran PT,
                             IndentMaster IM Where PM.PRMID=PT.PRMID And PT.IndentId=IM.IndentId And PM.CompanyId = " + ddCompName.SelectedValue + " And IM.IndentNo='" + txtidnt.Text + "'";
                if (chkcomplete.Checked == true)
                {
                    str = str + " and Im.status='Complete'";
                }
                else
                {
                    str = str + " and Im.status='Pending'";
                }
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddProcessName.SelectedValue = ds.Tables[0].Rows[0]["ProcessId"].ToString();
                    ProcessSelectedChange();
                    ddempname.SelectedValue = ds.Tables[0].Rows[0]["EmpId"].ToString();
                    EmpSelectedChange();
                    ddindentno.SelectedValue = ds.Tables[0].Rows[0]["IndentId"].ToString();
                    IndentSelectedChange();
                    txtdate.Text = ds.Tables[0].Rows[0]["Date"].ToString();
                    if (DDChallan.Items.Count > 0)
                    {
                        DDChallan.SelectedIndex = 1;
                        ChallanNoSelectedChange();
                    }
                }
                else
                {
                    Label1.Visible = true;
                    Label1.Text = "Indent No does not exists or Indent is (Complete or Pending)..........";
                    txtidnt.Text = "";
                    txtidnt.Focus();
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/EditIndentRowIssue.aspx");
                Label1.Visible = true;
                Label1.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    private void ddlcategorycangeOrderWise()
    {
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                      " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                      " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCatagory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
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
                        UtilityModule.ConditionalComboFill(ref dddesign, @"SELECT DISTINCT dbo.Design.designId, dbo.Design.designName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                        OrderLocalConsumption OC ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = OC.FinishedId INNER JOIN
                        dbo.Design ON dbo.ITEM_PARAMETER_MASTER.DESIGN_ID = dbo.Design.designId
                        WHERE ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"] + " And  OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ")", true, "--Select Design--");
                        break;
                    case "3":
                        clr.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddcolor, @"SELECT DISTINCT dbo.color.ColorId, dbo.color.ColorName FROm dbo.ITEM_PARAMETER_MASTER INNER JOIN
                        OrderLocalConsumption OC ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = OC.FinishedId INNER JOIN
                        dbo.color ON dbo.ITEM_PARAMETER_MASTER.COLOR_ID = dbo.color.ColorId
                        WHERE ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"] + " And  OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ")", true, "--Select Color--");
                        break;
                    case "4":
                        shp.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddshape, @"SELECT DISTINCT dbo.Shape.ShapeId, dbo.Shape.ShapeName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                        OrderLocalConsumption OC ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = OC.FinishedId INNER JOIN
                        dbo.Shape ON dbo.ITEM_PARAMETER_MASTER.SHAPE_ID = dbo.Shape.ShapeId
                        WHERE ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"] + " And  OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ")", true, "--Select Shape--");
                        break;
                    case "5":
                        sz.Visible = true;
                        //ChkMtr.Checked = false;
                        UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size.SizeFt FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                        OrderLocalConsumption OC ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = OC.FinishedId INNER JOIN
                        dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
                        WHERE ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"] + " And  OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ")", true, "Size in Ft");
                        break;
                    case "6":
                        shd.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddlshade, @"SELECT DISTINCT dbo.ShadeColor.ShadecolorId, dbo.ShadeColor.ShadeColorName FROM   dbo.ITEM_PARAMETER_MASTER INNER JOIN
                        OrderLocalConsumption OC ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = OC.FinishedId INNER JOIN
                        dbo.ShadeColor ON dbo.ITEM_PARAMETER_MASTER.SHADECOLOR_ID = dbo.ShadeColor.ShadecolorId
                        WHERE ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"] + " And OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ") AND dbo.ITEM_PARAMETER_MASTER.ITEM_ID=" + dditemname.SelectedValue + " AND dbo.ITEM_PARAMETER_MASTER.Quality_Id=" + dquality.SelectedValue, true, "--Select--");
                        break;
                    case "10":
                        clr.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddcolor, @"SELECT DISTINCT dbo.color.ColorId, dbo.color.ColorName FROm dbo.ITEM_PARAMETER_MASTER INNER JOIN
                        OrderLocalConsumption OC ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = OC.FinishedId INNER JOIN
                        dbo.color ON dbo.ITEM_PARAMETER_MASTER.COLOR_ID = dbo.color.ColorId
                        WHERE ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"] + " And  OrderId in(Select Distinct OrderId From  dbo.IndentDetail where IndentId =" + ddindentno.SelectedValue + ")", true, "--Select Color--");
                        break;
                }
            }
        }
    }
    protected void btnpriview_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Session["varCompanyId"]) == 5)
        {
            Session["ReportPath"] = "Reports/IndentRawIssueDuplicate1.rpt";
            Session["CommanFormula"] = "{V_IndentRawIssue.PRMid}=" + ViewState["Prmid"] + "";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
        }
        else
        {
            if (ChkForGSTReport.Checked == true)
            {
                ReportGST();
            }
            else
            {
                Report();
            }
        }
    }
    private void Report()
    {
        string qry = @" SELECT EmpName,Address,PhoneNo,Mobile,CompanyName,CompanyAddress,ComPanyPhoneNo,CompanyFaxNo,TinNo,Category_Name,Item_Name,QualityName,Designname,colorname,
                        shapename,shadecolorname,SizeMtr,ChallanNo,Indentid,PRMid,IssueQuantity,LotNo,GodownNAme,ShortName,ShadeColorName,unitname,companyid,IndentNo,Date,flagsize,CanQty,
                         MastercompanyId,Process_Name,Buyercode,localorder,customerorderno,(select top(1) Remark from PP_ProcessRawTran where PRMid=" + ViewState["Prmid"] + @" and remark<>'') as Remark,GSTIN,EMPGSTNO,BinNo,UserName,cast(replace(Convert(nvarchar(11), V_IndentRawIssue.reqdate, 106), ' ', '-')  as varchar) as reqdate
                     FROM  V_IndentRawIssue where V_IndentRawIssue.PRMid=" + ViewState["Prmid"] + "  ORDER BY V_IndentRawIssue.Item_Name ";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        string str = @"SELECT IssQty,CATEGORY_NAME,ITEM_NAME,Quality_Id,QualityName,PRMid,unitname  FROM indentRawIssue Where PRMid=" + ViewState["Prmid"] + " ORDER BY Quality_Id";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        SqlDataAdapter sda = new SqlDataAdapter(str, con);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        ds.Tables.Add(dt);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Session["varcompanyId"].ToString() == "16")
            {
                Session["rptFileName"] = "~\\Reports\\IndentRawIssueNEWChampo.rpt";
            }
            else if (Session["varcompanyId"].ToString() == "30")
            {
                Session["rptFileName"] = "~\\Reports\\IndentRawIssueNEWSamara.rpt";
            }
            else if (Session["varcompanyId"].ToString() == "42")
            {
                Session["rptFileName"] = "~\\Reports\\IndentRawIssueNEWVikramMirzapur.rpt";
            }
            else if (Session["varcompanyId"].ToString() == "44")
            {
                Session["rptFileName"] = "~\\Reports\\IndentRawIssueagni.rpt";
            }
            else
            {
                if (MySession.TagNowise == "1")
                {
                    Session["rptFileName"] = "~\\Reports\\IndentRawIssueTagwiseNEW.rpt";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\IndentRawIssueNEW.rpt";
                }
            }
            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\IndentRawIssueNEW.xsd";
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
    private void ReportGST()
    {
        SqlParameter[] _array = new SqlParameter[4];
        _array[0] = new SqlParameter("@PRMID", SqlDbType.Int);
        _array[1] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        _array[2] = new SqlParameter("@UserId", SqlDbType.Int);

        _array[0].Value = ViewState["Prmid"];
        _array[1].Value = Session["varcompanyId"];
        _array[2].Value = Session["varuserId"];

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetIndentRawIssueWithGSTReport", _array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptIndentRawIssueWithGST.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptIndentRawIssueWithGST.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    //protected void gvdetail_RowCreated(object sender, GridViewRowEventArgs e)
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

    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[5];
            arr[0] = new SqlParameter("@Prtid", SqlDbType.Int);
            arr[1] = new SqlParameter("@TableName", SqlDbType.NVarChar, 50);
            arr[2] = new SqlParameter("@Count", SqlDbType.Int);
            arr[3] = new SqlParameter("@FlagDeleteOrNot", SqlDbType.Int);
            arr[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);

            arr[0].Value = gvdetail.DataKeys[e.RowIndex].Value;
            arr[1].Value = "PP_ProcessRawTran";
            arr[2].Value = gvdetail.Rows.Count;
            arr[3].Direction = ParameterDirection.Output;
            arr[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_UpdateStockIssueDelete", arr);
            tran.Commit();
            if (arr[4].Value.ToString() != "")
            {
                Label1.Text = arr[4].Value.ToString();
                Label1.Visible = true;
            }
            else
            {
                Enable();
                //save_refresh();
                if (Convert.ToInt32(arr[3].Value) == 1)
                {
                    Label1.Text = "Data already received against this item.";
                    Label1.Visible = true;
                }
                else
                {
                    Fill_Grid();
                }
            }
        }
        catch (Exception ex)
        {
            Label1.Text = ex.Message;
            Label1.Visible = true;
            tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillSize();
    }
    private void fillSize()
    {
        string strSize;

        if (DDsizetype.SelectedIndex == 0)
        {
            strSize = " Sizeft";
        }
        else if (DDsizetype.SelectedIndex == 1)
        {
            strSize = "Sizemtr";
        }
        else
        {
            strSize = "Sizeinch";
        }
        if (MySession.IndentAsProduction == "1")
        {
            UtilityModule.ConditionalComboFill(ref ddsize, @"select distinct vf.SizeId,vf." + strSize + @" from IndentDetail ID inner join PP_Consumption PP
                                on ID.PPNo=PP.PPId  inner join V_FinishedItemDetail  vf on vf.ITEM_FINISHED_ID=pp.IFinishedid
                                WHERE  ID.IndentId =" + ddindentno.SelectedValue + "  and vf.category_id=" + ddCatagory.SelectedValue, true, "--Select Size--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size." + strSize + @" FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
            dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
            dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
            WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"], true, "Size in Ft");
        }
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddCompName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddempname.Items.Count > 0)
        {
            ddempname.SelectedIndex = 0;
        }
    }
    protected void DDTagNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        ViewState["FinishedID"] = Varfinishedid;
        getstockQty(Varfinishedid);
    }
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddgodown.Items.Count > 0 && ddgodown.SelectedIndex > 0)
        {
            ddgodown_SelectedIndexChanged(sender, new EventArgs());
        }
    }
    protected void DDBinNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));

        if (TDTagNo.Visible == true)
        {
            FillTagNo(Varfinishedid, sender);
        }
        else
        {
            getstockQty(Varfinishedid);
        }
    }

    private void FillGSTIGST()
    {
        Label1.Visible = false;
        Label1.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[11];
            param[0] = new SqlParameter("@ProcessId", ddProcessName.SelectedValue);
            param[1] = new SqlParameter("@CategoryId", ddCatagory.SelectedValue);
            param[2] = new SqlParameter("@ItemId", dditemname.SelectedValue);
            param[3] = new SqlParameter("@QualityId", dquality.SelectedValue);
            param[4] = new SqlParameter("@EffectiveDate", txtdate.Text);
            param[5] = new SqlParameter("@GSTType", DDGSType.SelectedValue);
            param[6] = new SqlParameter("@CGSTRate", SqlDbType.Float);
            param[6].Direction = ParameterDirection.Output;
            param[7] = new SqlParameter("@SGSTRate", SqlDbType.Float);
            param[7].Direction = ParameterDirection.Output;
            param[8] = new SqlParameter("@IGSTRate", SqlDbType.Float);
            param[8].Direction = ParameterDirection.Output;
            param[9] = new SqlParameter("@CompanyID", ddCompName.SelectedValue);
            param[10] = new SqlParameter("@BranchID", 1);

            //**********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_GetCGST_SGST_IGST_Rate", param);

            if (DDGSType.SelectedIndex > 0)
            {
                if (param[6].Value.ToString() != "" && param[7].Value.ToString() != "" || param[8].Value.ToString() != "")
                {
                    lblCGST.Text = param[6].Value.ToString();
                    lblSGST.Text = param[7].Value.ToString();
                    lblIGST.Text = param[8].Value.ToString();
                }
                else
                {
                    lblCGST.Text = "0";
                    lblSGST.Text = "0";
                    lblIGST.Text = "0";
                    Label1.Visible = true;
                    Label1.Text = "Please add GST/IGST regarding selected item";
                    return;
                }
            }
            else
            {
                lblCGST.Text = "0";
                lblSGST.Text = "0";
                lblIGST.Text = "0";

            }

            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            Label1.Text = ex.Message;
            con.Close();
        }
    }
    protected void DDGSType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDGSType.SelectedValue == "1")
        {
            TDCGST.Visible = true;
            TDSGST.Visible = true;
            TDIGST.Visible = false;
            FillGSTIGST();
        }
        else if (DDGSType.SelectedValue == "2")
        {
            TDCGST.Visible = false;
            TDSGST.Visible = false;
            TDIGST.Visible = true;
            FillGSTIGST();
        }
        else
        {
            TDCGST.Visible = false;
            TDSGST.Visible = false;
            TDIGST.Visible = false;
            lblCGST.Text = "0";
            lblSGST.Text = "0";
            lblIGST.Text = "0";
            // fill_text();
        }
    }
    protected void BtnProcessToPNM_Click(object sender, EventArgs e)
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
            param[0] = new SqlParameter("@ChampoPrmID", ViewState["Prmid"]);
            param[1] = new SqlParameter("@USERID", 1);
            param[2] = new SqlParameter("@MASTERCOMPANYID", 28);
            param[3] = new SqlParameter("@MSG", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@TranType", 1);
            param[5] = new SqlParameter("@TYPEFLAG", 0);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_Save_ChampoIndentRawIssueToOtherCompany", param);
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('" + param[3].Value + "')", true);
            Tran.Commit();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('" + ex.Message + "')", true);
            Tran.Rollback();
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

    protected void BtnUpdateRemark_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[7];

            arr[0] = new SqlParameter("@PRMID", SqlDbType.Int);
            arr[1] = new SqlParameter("@TranType", SqlDbType.Int);
            arr[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            arr[3] = new SqlParameter("@userid", Session["varuserid"]);
            arr[4] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            arr[5] = new SqlParameter("@Remark", txtremarks.Text);
            arr[6] = new SqlParameter("@ProcessId", ddProcessName.SelectedValue);

            arr[0].Value = DDChallan.SelectedValue;
            arr[1].Value = 0;
            arr[2].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_IndentRawIssue_Update_Remark", arr);

            Label1.Visible = true;
            Label1.Text = arr[2].Value.ToString();
            //if (arr[2].Value.ToString() != "")
            //{
            //    ScriptManager.RegisterStartupScript(Page, GetType(), "altdel", "alert('" + arr[2].Value.ToString() + "');", true);
            //}
            //else
            //{
            //    LblError.Text = arr[2].Value.ToString();
            //}
            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            Label1.Visible = true;
            Label1.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        Fill_Grid();
    }
}