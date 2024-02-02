using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ClosedXML.Excel;
using System.IO;

public partial class Masters_Process_frmProcessKit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varcompanyNo"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = null;
            DataSet ds = null;
            str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                    From Companyinfo CI(nolock)
                    JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @"  
                    Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName 

                    select  UnitsId,UnitName from Units with(nolock) Where Mastercompanyid = " + Session["varMasterCompanyIDForERP"] + @" 
                    select ITEM_ID,ITEM_NAME from ITEM_MASTER IM with(nolock) Inner Join CategorySeparate CS with(nolock) on 
                    cs.Categoryid=IM.CATEGORY_ID and Cs.id=0 And IM.Mastercompanyid = " + Session["varMasterCompanyIDForERP"] + @" 
                    select  ShapeId,ShapeName from Shape with(nolock) 
                    select PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER With(nolock) Where MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" and PROCESS_NAME_ID=29  Order By PROCESS_NAME 
                    select ICm.CATEGORY_ID,ICM.CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=Cs.Categoryid and cs.id=0 order by CATEGORY_NAME 
                    select val,Type From Sizetype
                    select OrderCategoryId,OrderCategory from OrderCategory order by OrderCategory";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDUnitName, ds, 1, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDArticleName, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDShape, ds, 3, true, "--Plz Select-- ");
            UtilityModule.ConditionalComboFillWithDS(ref ddJob, ds, 4, true, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 5, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizeType, ds, 6, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDOrderType, ds, 7, false, "");

            if (DDsizeType.Items.FindByValue(variable.VarDefaultSizeId) != null)
            {
                DDsizeType.SelectedValue = variable.VarDefaultSizeId;
            }
            if (DDShape.Items.Count > 0)
            {
                DDShape.SelectedIndex = 0;
            }
            ds.Dispose();
            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "8":
                    Divuniname.Visible = true;
                    break;
                default:
                    divQuality.Visible = true;
                    divDesign.Visible = true;
                    Divuniname.Visible = false;
                    divCategory.Visible = true;
                    if (DDCategory.Items.Count > 0)
                    {
                        DDCategory.SelectedIndex = 1;
                        DDCategory_SelectedIndexChanged(DDCategory, new EventArgs());
                    }
                    break;
            }
            if (Session["varMasterCompanyIDForERP"].ToString() == "16")
            {
                DivWeavingEmployee.Visible = true;
                BindWeavingEmp();
            }
            if (Session["varMasterCompanyIDForERP"].ToString() == "28")
            {
                Divuniname.Visible = true;
                DivWeavingEmployee.Visible = true;
                BindWeavingEmp();
            }
            if (Session["varMasterCompanyIDForERP"].ToString() == "27")
            {
                if (DDRateLocation.SelectedValue == "1")
                {
                    DivWeavingEmployee.Visible = true;
                    BindWeavingEmp();
                }
                else
                {
                    DivWeavingEmployee.Visible = false;
                    DDWeavingEmp.SelectedIndex = 0;
                }
            }
            if (Session["varMasterCompanyIDForERP"].ToString() == "42")
            {
                //DivBonus.Visible = true;
                //DivFinisherRate.Visible = true;
                //DivDateRange.Visible = true;
                DivOrderType.Visible = true;
                if (DDRateLocation.SelectedValue == "1")
                {
                    DivWeavingEmployee.Visible = true;
                    BindWeavingEmp();
                }
                else
                {
                    DivWeavingEmployee.Visible = false;
                    DDWeavingEmp.SelectedIndex = 0;
                }
            }
            if (variable.VarDEFINEPROCESSRATE_LOCATIONWISE == "1")
            {
                DivRateLocation.Visible = true;

            }
        }
    }
    protected void BindWeavingEmp()
    {
        string str2 = null;

        if (Session["varMasterCompanyIDForERP"].ToString() == "16" || Session["varMasterCompanyIDForERP"].ToString() == "28")
        {
            str2 = @"Select EI.EmpID, Case When EI.Empcode <> '' Then EI.EmpCode Else EI.EmpName End Empname 
                From Empinfo EI(Nolock)
                JOIN EmpProcess EP(Nolock) ON EP.EmpId = EI.EmpId And EP.ProcessId = " + ddJob.SelectedValue + @" 
                Where EI.Blacklist = 0 
                Order By EI.EmpName";
        }
        else
        {
            str2 = @"select EI.EmpId, case When EI.Empcode<>'' then EI.EmpCode Else EI.EmpName End Empname,EmployeeType 
                from EmpInfo EI(Nolock) 
                inner join Department D(Nolock) on EI.Departmentid=D.DepartmentId and D.DepartmentName in ('PRODUCTION') 
	            JOIN EmpProcess EP(Nolock) ON EI.EmpId=EP.EmpId and EP.ProcessId=" + ddJob.SelectedValue + @"
                Where EI.Status='P' and EI.Blacklist=0 and EI.EmployeeType=" + DDRateLocation.SelectedValue + @"
	            Order by Empname";
        }
        DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
        UtilityModule.ConditionalComboFillWithDS(ref DDWeavingEmp, ds2, 0, true, "--Plz Select--");
    }
    protected void DDArticleName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (divQuality.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddquality, @"select Distinct vf.Qualityid,vf.Qualityname from V_FinishedItemDetail vf with(nolock)
                                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + " And QualityId<>0 order by vf.Qualityname", true, "--Plz Select--");
        }
        UtilityModule.ConditionalComboFill(ref DDColor, @"select Distinct vf.ColorId,vf.ColorName from V_FinishedItemDetail vf with(nolock)
                                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + " And ColorId<>0 order by ColorName", true, "--Plz Select--");
        fillGrid();
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        string size = "SizeMtr";
        switch (DDsizeType.SelectedValue)
        {
            case "0":
                size = "sizeft";
                break;
            case "1":
                size = "SizeMtr";
                break;
            case "2":
                size = "Sizeinch";
                break;
            default:
                break;
        }
        string str = @"select Distinct SizeId," + size + @" as size from V_FinishedItemDetail vf with(nolock)
                       inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + @"
                       And ShapeId= " + DDShape.SelectedValue + " and Sizeid<>0";
        if (ddquality.SelectedIndex > 0)
        {
            str = str + " and  vf.QualityId=" + ddquality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and  vf.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and  vf.colorid=" + DDColor.SelectedValue;
        }
        str = str + " order by size";
        UtilityModule.ConditionalComboFill(ref ddSize, str, true, "--Plz Select--");
    }
    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDShape_SelectedIndexChanged(sender, e);
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {

        if (Session["varMasterCompanyIDForERP"].ToString() == "22")
        {
            string status = "";
            if (txtpcs.Text == "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please enter total pcs.');", true);
                txtpcs.Focus();
                return;
            }

            for (int i = 0; i < DG.Rows.Count; i++)
            {
                CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
                TextBox txtissueqty = ((TextBox)DG.Rows[i].FindControl("txtissueqty"));

                if (Chkboxitem.Checked == true)   // Change when Updated Completed
                {
                    status = "1";
                }
                if (txtissueqty.Text == "" && Chkboxitem.Checked == true)   // Change when Updated Completed
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Issue qty can not be blank');", true);
                    txtissueqty.Focus();
                    return;
                }
                if (Chkboxitem.Checked == true && (Convert.ToDecimal(txtissueqty.Text) <= 0))   // Change when Updated Completed
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Issue qty always greater then zero');", true);
                    txtissueqty.Focus();
                    return;
                }
               
            }

            if (status == "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check boxes');", true);
                return;
            }
        }

        //********sql table Type
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("ifinishedid", typeof(int));
        dtrecords.Columns.Add("IUnitid", typeof(int));
        dtrecords.Columns.Add("Isizeflag", typeof(int));
        dtrecords.Columns.Add("Godownid", typeof(int));
        dtrecords.Columns.Add("Lotno", typeof(string));
        dtrecords.Columns.Add("TagNo", typeof(string));
        dtrecords.Columns.Add("issueqty", typeof(float));
        //dtrecords.Columns.Add("Noofcone", typeof(int));
        dtrecords.Columns.Add("Prorderid", typeof(int));
        dtrecords.Columns.Add("ConsmpQty", typeof(float));
        dtrecords.Columns.Add("BinNo", typeof(string));
        dtrecords.Columns.Add("STOCKQTY", typeof(float));
        //*******************
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtissueqty = ((TextBox)DG.Rows[i].FindControl("txtissueqty"));
            DropDownList DDGodown = ((DropDownList)DG.Rows[i].FindControl("DDGodown"));
            DropDownList DDLotNo = ((DropDownList)DG.Rows[i].FindControl("DDLotNo"));
            DropDownList DDTagNo = ((DropDownList)DG.Rows[i].FindControl("DDTagNo"));
            DropDownList DDBinNo = ((DropDownList)DG.Rows[i].FindControl("DDBinNo"));

            if (Chkboxitem.Checked == true && (txtissueqty.Text != "") && DDGodown.SelectedIndex > 0 && DDLotNo.SelectedIndex > 0 && DDTagNo.SelectedIndex > 0)
            {
                Label lblitemfinishedid = ((Label)DG.Rows[i].FindControl("lblifinishedid"));
                Label lblunitid = ((Label)DG.Rows[i].FindControl("lbliunitid"));
                Label lblstockqty = ((Label)DG.Rows[i].FindControl("lblstockqty"));
               // Label lblflagsize = ((Label)DG.Rows[i].FindControl("lblisizeflag"));
                string Lotno = DDLotNo.Text;
                string TagNo = DDTagNo.Text;
                //TextBox txtnoofcone = ((TextBox)DG.Rows[i].FindControl("txtnoofcone"));
                Label lblissueorderid = ((Label)DG.Rows[i].FindControl("lblissueorderid"));
                Label lblconsmpqty = ((Label)DG.Rows[i].FindControl("lblconsmpqty"));
                //*********************
                DataRow dr = dtrecords.NewRow();
                dr["ifinishedid"] = lblitemfinishedid.Text;
                dr["IUnitid"] = DDsizeType.SelectedValue;
                dr["Isizeflag"] = 0;
                dr["Godownid"] = DDGodown.SelectedValue;
                dr["Lotno"] = Lotno;
                dr["TagNo"] = TagNo;
                dr["IssueQty"] =Convert.ToDecimal(txtissueqty.Text) == 0 ? 0 : Convert.ToDecimal(txtissueqty.Text);
                //dr["Noofcone"] = txtnoofcone.Text == "" ? "0" : txtnoofcone.Text;
                dr["Prorderid"] = 0;
                dr["consmpqty"] = lblconsmpqty.Text;
                dr["BinNo"] = variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : "";
                dr["STOCKQTY"] = lblstockqty.Text == "" ? "0" : lblstockqty.Text; ;
                dtrecords.Rows.Add(dr);
            }
        }
        //if (Session["varMasterCompanyIDForERP"].ToString() == "21")
        //{
        //    if (chkEdit.Checked == false)     // Change when Updated Completed
        //    {
        //        if (dtrecords.Rows.Count != DG.Rows.Count)
        //        {
        //            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please check stock qty in all rows');", true);
        //            return;
        //        }
        //    }
        //}

        if (Session["varMasterCompanyIDForERP"].ToString() == "22")
        {
            //DateTime IssueDate = Convert.ToDateTime(txtissuedate.Text.ToString());
            //DateTime FolioDate = Convert.ToDateTime(txtFolioIssueDate.Text.ToString());
            DateTime IssueDate = DateTime.Today;
            DateTime FolioDate = DateTime.Today;

            if (IssueDate < FolioDate)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select date greater then folio date');", true);
                return;
            }

            //if (chkEdit.Checked == false)     // Change when Updated Completed
            //{
            //    if (dtrecords.Rows.Count != DG.Rows.Count)
            //    {
            //        ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please check stock qty in all rows');", true);
            //        return;
            //    }
            //}
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        if (dtrecords.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                if ((DDArticleName.SelectedValue !="0") && (ddquality.SelectedValue !="0") && (DDDesign.SelectedValue !="0") && (DDColor.SelectedValue !="0") && (DDDesign.SelectedValue !="0") && (DDShape.SelectedValue !="0"))
                {
                    int VARFINISHEDID = UtilityModule.getItemFinishedId(Convert.ToInt32(DDArticleName.SelectedValue), Convert.ToInt32(ddquality.SelectedValue), Convert.ToInt32(DDDesign.SelectedValue), Convert.ToInt32(DDColor.SelectedValue), Convert.ToInt32(DDShape.SelectedValue), Convert.ToInt32(ddSize.SelectedValue), 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                    SqlParameter[] param = new SqlParameter[11];
                    param[0] = new SqlParameter("@PrmId", SqlDbType.Int);
                    //if (chkEdit.Checked == true && Session["varMasterCompanyIDForERP"].ToString() == "21")
                    //{
                    //    param[0].Value = DDissueno.SelectedValue;
                    //}
                    //else
                    //{
                    param[0].Value = 0;
                    //}

                    param[0].Direction = ParameterDirection.InputOutput;
                    param[1] = new SqlParameter("@companyid", DDCompanyName.SelectedValue);
                    param[2] = new SqlParameter("@Processid", ddJob.SelectedValue);
                    param[3] = new SqlParameter("@issueDate", DateTime.Today);
                    param[4] = new SqlParameter("@userid", Session["varuserid"]);
                    param[5] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
                    param[6] = new SqlParameter("@dtrecords", dtrecords);
                    param[7] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                    param[7].Direction = ParameterDirection.Output;
                    param[8] = new SqlParameter("@StockNoQty", txtpcs.Text == "" ? "0" : txtpcs.Text);
                    param[9] = new SqlParameter("@CHALANNO", SqlDbType.VarChar, 50);
                    param[9].Value = "";
                    param[9].Direction = ParameterDirection.InputOutput;
                    param[10] = new SqlParameter("@finished", VARFINISHEDID);

                    ///**********
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEKMISSUEONLOOM", param);
                    //*******************
                    ViewState["reportid"] = param[0].Value.ToString();
                    txtissueno.Text = param[9].Value.ToString();
                    hnissueid.Value = param[0].Value.ToString();
                    Tran.Commit();
                    if (param[7].Value.ToString() != "")
                    {
                        lblMessage.Text = param[7].Value.ToString();
                    }
                    else
                    {
                        lblMessage.Text = "DATA SAVED SUCCESSFULLY.";
                        FILLGRID(Convert.ToInt32(txtpcs.Text));
                        FillissueGrid();
                    }
                    txtpcs.Text = "";
                }
                else { lblMessage.Text = "PLEASE SELECT ALL MANDATORY FIELDS."; return; }

            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblMessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
        }


    }
    protected void fillGrid()
    {
//        DataSet ds = null;
//        string str = @"select  vf.item_id,vf.ITEM_NAME+'  '+vf.QualityName+'  '+vf.designName as Articles,PNM.Process_name as JobName,vf.ColorName as Colour,case when tj.flagsize=1 then vf.SizeMtr else vf.sizeft end as Size,
//                    Tj.Unitrate,Tj.Date,Vf.shapeName as Shape,Ratetype=case when tj.ratetype=1 then 'Pcs Wise' else 'Area Wise' End,tj.Commrate,
//                    case when Tj.RateLocation=0 then 'InHouse' else 'OutSide' end as RateLocation,isnull(EI.EmpName,'') as EmpName, Tj.Bonus
//                    ,isnull(TJ.FinisherRate,0) as FinisherRate,isnull(TJ.OrderTypeId,0) as OrderTypeId ,isnull(OC.Ordercategory,'') as OrderType
//                    from tbjobrate Tj(Nolock) 
//                    inner join V_FinishedItemDetail vf(Nolock) on Tj.finishedid=vf.ITEM_FINISHED_ID  
//                    LEFT JOIN EmpInfo EI(Nolock) ON TJ.EmpId=EI.EmpID                 
//                    inner join PROCESS_NAME_MASTER PNM(Nolock) on PNM.PROCESS_NAME_ID=Tj.jobid 
//                    LEFT JOIN OrderCategory OC(NoLock) ON TJ.OrderTypeId=OC.OrderCategoryId
//                    Where Tj.mastercompanyId=" + Session["varMasterCompanyIDForERP"] + " And Tj.companyId=" + DDCompanyName.SelectedValue;

//        if (Divuniname.Visible == true)
//        {
//            if (DDUnitName.SelectedIndex >= 0)
//            {
//                str = str + "  And Tj.Unitid= " + DDUnitName.SelectedValue;
//            }
//        }
//        if (DivOrderType.Visible == true)
//        {
//            if (DDOrderType.SelectedIndex >= 0)
//            {
//                str = str + "  And Tj.OrderTypeId= " + DDOrderType.SelectedValue;
//            }
//        }
//        if (ddJob.SelectedIndex > 0)
//        {
//            str = str + "  And Tj.Jobid= " + ddJob.SelectedValue;
//        }
//        if (DDArticleName.SelectedIndex > 0)
//        {
//            str = str + "  And vf.Item_Id= " + DDArticleName.SelectedValue;
//        }
//        if (ddquality.SelectedIndex > 0)
//        {
//            str = str + "  And vf.QualityId= " + ddquality.SelectedValue;
//        }
//        if (DDDesign.SelectedIndex > 0)
//        {
//            str = str + "  And vf.DesignId= " + DDDesign.SelectedValue;
//        }
//        if (DDRatetype.SelectedIndex != -1)
//        {
//            str = str + "  And TJ.Ratetype= " + DDRatetype.SelectedValue;
//        }
//        if (DDRateLocation.SelectedIndex != -1)
//        {
//            str = str + "  And TJ.RateLocation= " + DDRateLocation.SelectedValue;
//        }
//        if (DivWeavingEmployee.Visible == true)
//        {
//            if (DDWeavingEmp.SelectedIndex >= 0)
//            {
//                str = str + "  And Tj.EmpId= " + DDWeavingEmp.SelectedValue;
//            }
//        }

//        str = str + " order by tj.id desc";
//        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
//        //var strSort = "Articles asc";
//        //var dv = ds.Tables[0].DefaultView;
//        //dv.Sort = strSort;
//        //var newDS = new DataSet();
//        //var newDT = dv.ToTable();
//        ////newDS.Tables.Add(newDT);        
//        //ds.Tables[0].DefaultView.Sort = "Articles asc";
//        //ds.Tables[0].DefaultView.Sort = "Date desc";
//        DGRateDetail.DataSource = ds;
//        DGRateDetail.DataBind();
    }
    protected void ddJob_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddJob.SelectedItem.Text.ToUpper())
        {
            case "WEAVING":
                divratetype.Visible = true;
                break;
            case "PANEL MAKING":
                divratetype.Visible = true;
                break;
            case "FILLER MAKING":
                divratetype.Visible = true;
                break;
            default:
                divratetype.Visible = false;
                break;
        }
        fillGrid();
    }
    protected void DDUnitName_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillGrid();
    }

    protected void ddquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDDesign, @"select Distinct designId,designName from V_FinishedItemDetail vf with(nolock)
                                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + " and QualityId=" + ddquality.SelectedValue + " And Designid<>0 order by designName", true, "--Plz Select--");
        fillGrid();
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDColor, @"select Distinct Colorid,ColorName from V_FinishedItemDetail vf with(nolock)
                                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + " and QualityId=" + ddquality.SelectedValue + " and DesignId=" + DDDesign.SelectedValue + " And Colorid<>0 order by ColorName", true, "--Plz Select--");
        if (DDColor.Items.Count > 0)
        {
            DDColor_SelectedIndexChanged(sender, new EventArgs());
        }
        fillGrid();
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
    }

    private void ddlcategorycange()
    {
        divQuality.Visible = false;
        divDesign.Visible = false;
        divColor.Visible = false;
        divShape.Visible = false;
        divSize.Visible = false;
        divshade.Visible = false;

        UtilityModule.ConditionalComboFill(ref DDArticleName, @"select IM.ITEM_ID,IM.ITEM_NAME From Item_Master IM inner join CategorySeparate CS on IM.CATEGORY_ID=CS.Categoryid
                                                            and CS.id=0 and cs.categoryid=" + DDCategory.SelectedValue + " and IM.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by IM.ITEM_NAME", true, "--Plz Select--");

        string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                      " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                      " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        divQuality.Visible = true;
                        break;
                    case "2":
                        divDesign.Visible = true;
                        break;
                    case "3":
                        divColor.Visible = true;
                        break;
                    case "4":
                        divShape.Visible = true;
                        break;
                    case "5":
                        divSize.Visible = true;
                        break;
                    case "6":
                        divshade.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddlshade, @"Select Distinct SC.ShadeColorID, SC.ShadeColorName 
                                From ShadeColor SC(nolock)
                                Order By ShadeColorName", true, "--Plz Select--");
                        break;
                }
            }
        }
    }
    protected void DDsizeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDShape_SelectedIndexChanged(sender, new EventArgs());
    }
    protected void DDRatetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillGrid();
    }
    protected void DDRateLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"].ToString() == "27" || Session["varMasterCompanyIDForERP"].ToString() == "42")
        {
            if (DDRateLocation.SelectedValue == "1")
            {
                DivWeavingEmployee.Visible = true;
                BindWeavingEmp();
            }
            else
            {
                DivWeavingEmployee.Visible = false;
                DDWeavingEmp.SelectedIndex = 0;
            }
        }
        if (Session["varMasterCompanyIDForERP"].ToString() == "16" || Session["varMasterCompanyIDForERP"].ToString() == "28")
        {
            DivWeavingEmployee.Visible = true;
            BindWeavingEmp();
        }
        fillGrid();
    }
    protected void DDWeavingEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillGrid();
    }
    protected void DGRateDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
        //    e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        //    //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVFinisherJobRate, "select$" + e.Row.RowIndex);

        //    for (int i = 0; i < DGRateDetail.Columns.Count; i++)
        //    {
        //        if (DGRateDetail.Columns[i].HeaderText == "Rate Location")
        //        {
        //            if (variable.VarDEFINEPROCESSRATE_LOCATIONWISE == "1")
        //            {
        //                DGRateDetail.Columns[i].Visible = true;
        //            }
        //            else
        //            {
        //                DGRateDetail.Columns[i].Visible = false;
        //            }
        //        }
        //        if (DGRateDetail.Columns[i].HeaderText == "Bonus" || DGRateDetail.Columns[i].HeaderText == "Finisher Rate" || DGRateDetail.Columns[i].HeaderText == "Order Type")
        //        {
        //            if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 42)
        //            {
        //                DGRateDetail.Columns[i].Visible = true;
        //            }
        //            else
        //            {
        //                DGRateDetail.Columns[i].Visible = false;
        //            }
        //        }

        //        //if (DGRateDetail.Columns[i].HeaderText == "Emp Name")
        //        //{
        //        //    if (Session["varMasterCompanyIDForERP"].ToString() == "27")
        //        //    {
        //        //        DGRateDetail.Columns[i].Visible = true;
        //        //    }
        //        //    else
        //        //    {
        //        //        DGRateDetail.Columns[i].Visible = false;
        //        //    }
        //        //}
        //    }
        //}
    }
    //protected void btnpreview_Click(object sender, EventArgs e)
    //{
    //    //lblMessage.Text = "";
    //    //try
    //    //{
    //    //    string str = "";
    //    //    if (Divuniname.Visible == true)
    //    //    {
    //    //        if (DDUnitName.SelectedIndex >= 0)
    //    //        {
    //    //            str = str + "  And Tj.Unitid= " + DDUnitName.SelectedValue;
    //    //        }
    //    //    }
    //    //    if (ddJob.SelectedIndex > 0)
    //    //    {
    //    //        str = str + "  And Tj.Jobid= " + ddJob.SelectedValue;
    //    //    }
    //    //    if (DDArticleName.SelectedIndex > 0)
    //    //    {
    //    //        str = str + "  And vf.Item_Id= " + DDArticleName.SelectedValue;
    //    //    }
    //    //    if (ddquality.SelectedIndex > 0)
    //    //    {
    //    //        str = str + "  And vf.QualityId= " + ddquality.SelectedValue;
    //    //    }
    //    //    if (DDDesign.SelectedIndex > 0)
    //    //    {
    //    //        str = str + "  And vf.DesignId= " + DDDesign.SelectedValue;
    //    //    }
    //    //    if (DDRatetype.SelectedIndex != -1)
    //    //    {
    //    //        str = str + "  And TJ.Ratetype= " + DDRatetype.SelectedValue;
    //    //    }
    //    //    //if (DivDateRange.Visible == true)
    //    //    //{
    //    //    //    if (ChkForDate.Checked == true)
    //    //    //    {
    //    //    //        str = str + " and DATEADD(dd, 0, DATEDIFF(dd, 0, TJ.Date))>='" + txtFromDate.Text + "' and DATEADD(dd, 0, DATEDIFF(dd, 0, TJ.Date))<='" + txtToDate.Text + "'";
    //    //    //    }
    //    //    //}

    //    //    SqlParameter[] param = new SqlParameter[3];
    //    //    param[0] = new SqlParameter("@companyid", DDCompanyName.SelectedValue);
    //    //    param[1] = new SqlParameter("@where", str);
    //    //    param[2] = new SqlParameter("@CurrentRate", chkcurrentrate.Checked == true ? "1" : "0");

    //    //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_PRINTDEFINEJOBRATE", param);
    //    //    if (ds.Tables[0].Rows.Count > 0)
    //    //    {
    //    //        if (Session["VarCompanyNo"].ToString() == "16" || Session["VarCompanyNo"].ToString() == "28")
    //    //        {
    //    //            if (chkcurrentrate.Checked == true)
    //    //            {
    //    //                ProcessJobRateInExcel(ds);
    //    //            }
    //    //            else
    //    //            {
    //    //                Session["rptFileName"] = "~\\Reports\\rptjobrate.rpt";
    //    //                Session["GetDataset"] = ds;
    //    //                Session["dsFileName"] = "~\\ReportSchema\\rptjobrate.xsd";
    //    //            }

    //    //        }
    //    //        else if (Session["VarCompanyNo"].ToString() == "42")
    //    //        {
    //    //            Session["rptFileName"] = "~\\Reports\\RptJobRateVikramMirzapur.rpt";
    //    //            Session["GetDataset"] = ds;
    //    //            Session["dsFileName"] = "~\\ReportSchema\\rptjobrate.xsd";
    //    //        }
    //    //        else
    //    //        {
    //    //            Session["rptFileName"] = "~\\Reports\\rptjobrate.rpt";
    //    //            Session["GetDataset"] = ds;
    //    //            Session["dsFileName"] = "~\\ReportSchema\\rptjobrate.xsd";
    //    //        }
    //    //        StringBuilder stb = new StringBuilder();
    //    //        stb.Append("<script>");
    //    //        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
    //    //        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    //    //    }
    //    //    else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    lblMessage.Text = ex.Message;
    //    //}
    //}
    private void ProcessJobRateInExcel(DataSet ds)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 0;

            sht.Range("A1:L1").Merge();
            sht.Range("A1").Value = ds.Tables[0].Rows[0]["JobName"] + " " + "RATE LIST FORMAT";
            //sht.Range("A2:X2").Merge();
            //sht.Range("A2").Value = "Filter By :  " + FilterBy;
            //sht.Row(2).Height = 30;
            sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:L2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A2:L2").Style.Alignment.SetWrapText();
            sht.Range("A1:L2").Style.Font.FontName = "Arial";
            sht.Range("A1:L2").Style.Font.FontSize = 10;
            sht.Range("A1:L2").Style.Font.Bold = true;

            //*******Header
            sht.Range("A3").Value = "SR NO.";
            sht.Range("B3").Value = "ITEM";
            sht.Range("C3").Value = "QUALITY";
            sht.Range("D3").Value = "DESIGN";
            sht.Range("E3").Value = "COLOR";
            sht.Range("F3").Value = "SHAPE";
            sht.Range("G3").Value = "SIZE";
            sht.Range("H3").Value = "RATE TYPE";
            sht.Range("I3").Value = "RATE";
            sht.Range("J3").Value = "COMM. RATE";
            sht.Range("K3").Value = "LOCATION";
            sht.Range("L3").Value = "DATE";


            sht.Range("A3:L3").Style.Font.FontName = "Arial";
            sht.Range("A3:L3").Style.Font.FontSize = 9;
            sht.Range("A3:L3").Style.Font.Bold = true;
            sht.Range("S3:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A3:L3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A3:L3").Style.Alignment.SetWrapText();


            //DataView dv = new DataView(ds.Tables[0]);
            //dv.Sort = "FOLIONO";
            //DataSet ds1 = new DataSet();
            //ds1.Tables.Add(dv.ToTable());

            int srno = 0;
            row = 4;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":L" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":L" + row).Style.Font.FontSize = 8;

                srno = srno + 1;

                sht.Range("A" + row).SetValue(srno);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Colour"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Shape"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Size"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["RateType"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["UnitRate"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["CommRate"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["RateLocation"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Date"]);


                row = row + 1;

            }
            //*************
            sht.Columns(1, 26).AdjustToContents();

            //sht.Columns("K").Width = 13.43;

            using (var a = sht.Range("A1" + ":L" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("ProcessJobRate_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
    }
    protected void txtpcs_TextChanged(object sender, EventArgs e)
    {
        int pcs = 0;
        if (!string.IsNullOrEmpty(txtpcs.Text))
        {
            pcs = Convert.ToInt32(txtpcs.Text);
        }
        
        
        FILLGRID(pcs);
    }

    private void FILLGRID(int pcs)
    {
      //  DG.DataSource = "";


        String STR = @"select PCD.PCMDID,PM.PROCESS_NAME,VD1.Category_Name+space(2)+VD1.Item_Name+space(2)+VD1.QualityName+ Space(2) +VD1.DesignName+ Space(2) +VD1.ColorName+ Space(2) +VD1.ShapeName+SPACE(2)+ 
                        case When PCM.sizeflag=0 Then  VD1.SIZEFT When PCM.sizeflag=1 Then VD1.SizeMtr When PCM.sizeflag=2 Then VD1.sizeinch Else VD2.Sizeft End+SPACE(2)+VD1.SHADECOLORName+
                        SPACE(5)+Isnull(FT.FINISHED_TYPE_NAME,'')+' '+case When vD1.SizeId>0 Then ST.Type Else '' End as ITEMCODE,
                        VD2.Category_Name+space(2)+VD2.Item_Name+space(2)+VD2.QualityName+ Space(2) +VD2.DesignName+ Space(2) +VD2.ColorName+ Space(2) +VD2.ShapeName+SPACE(2)+ case When PCD.ISizeflag=0 Then  VD2.SIZEFT 
                        When pcd.ISizeflag=1 Then VD2.SizeMtr When PCD.ISizeflag=2 Then VD2.sizeinch  Else VD2.sizeft End+SPACE(2)+VD2.SHADECOLORName+
                        SPACE(5)+Isnull(FT.FINISHED_TYPE_NAME,'')+case When VD2.sizeid>0 Then STI.type else '' End INPUT_ITEM,Round(case When PCM.FlagMtrFt=1 then IQTY*1.196 else IQTY End,3) IQTY,
                        Round(Case When PCM.LossPercentageFlag=1 Then Round(PCD.ILOSS/PCD.IQTY*100,0) Else Case When PCM.FlagMtrFt=1 then ILOSS*1.196 else ILOSS End End,3) ILOSS,
                        IRATE,IU.UNITNAME I_UNIT,VD3.Category_Name+space(2)+VD3.Item_Name+space(2)+VD3.QualityName+ Space(2) +VD3.DesignName+ Space(2) +VD3.ColorName+ Space(2) +VD3.ShapeName+SPACE(2)+case When PCD.OSizeflag=0 Then  VD2.SIZEFT 
                        When pcd.OSizeflag=1 Then VD3.SizeMtr When PCD.OSizeflag=2 Then VD3.sizeinch  Else VD3.sizeft End+SPACE(2)+VD3.SHADECOLORName+
                        SPACE(5)+Isnull(FT.FINISHED_TYPE_NAME,'')+case When VD3.sizeid>0 Then STO.type else '' End OUTPUT_ITEM,Round(Case When PCM.FlagMtrFt=1 then OQTY*1.196 else OQTY End,3) OQTY,ORATE,
                        OU.UNITNAME O_UNIT,QCM.QUALITYCODEID,PCM.FlagMtrFt,Dyingmatch,DyingType,Dyeing,case When PM.Process_Name='PURCHASE' and " + variable.BOM_PurchaseIncludeProcess + @"=1 Then dbo.F_GetBOMProcessIncluding(PCM.Finishedid,PCD.Ifinishedid,PCM.Processid) Else '' End as INCLUDING_PROCESS,
                        Round(Case When PCM.FlagMtrFt=1 then OLOSS*1.196 else OLOSS End,3) OLOSS,isnull(PCD.OQtyPercentage,'') OQtyPercentage,PCD.Ifinishedid,'"+pcs+@"' * Round(case When PCM.FlagMtrFt=1 then IQTY*1.196 else IQTY End,3) as consmpqty  from PROCESSCONSUMPTIONMASTER PCM inner Join PROCESSCONSUMPTIONDETAIL PCD
                        on PCm.PCMID=PCD.PCMID
                        inner join ITEM_PARAMETER_MASTER IPM on IPM.ITEM_FINISHED_ID=PCM.FINISHEDID
                        inner join V_FinishedItemDetail VD1  on IPM.ITEM_FINISHED_ID=VD1.Item_FINISHED_ID and PCM.FINISHEDID=VD1.Item_Finished_id
                        inner join V_FinishedItemDetail VD2  on PCD.IFINISHEDID=Vd2.ITEM_FINISHED_ID
                        inner join V_FinishedItemDetail VD3 on  pCd.OFINISHEDID=vd3.ITEM_FINISHED_ID
                        inner join UNIT IU on PCD.IUNITID=IU.UnitId
                        inner join Unit OU on Pcd.OUNITID=ou.UnitId
                        LEFT OUTER JOIN qualityCodeMaster QCM ON  PCD.SUB_QUALITY_ID=QCM.QUALITYCODEID
                        LEFT OUTER JOIN FINISHED_TYPE FT ON PCD.I_FINISHED_TYPE_ID=FT.ID 
                        LEFT OUTER JOIN FINISHED_TYPE FT1  ON PCD.O_FINISHED_TYPE_ID=FT1.ID 
                        inner join PROCESS_NAME_MASTER PM on PM.PROCESS_NAME_ID=PCM.PROCESSID
                        inner join SizeType ST on PCM.sizeflag=ST.Val
                        inner join Sizetype STI on PCD.ISizeflag=STI.Val
                        inner join SizeType STO on Pcd.OSizeflag=STO.Val where IQTY>0 and  PCM.PROCESSID=1 and PCM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        if (ddquality.SelectedIndex > 0)
        {
            STR = STR + @" AND QUALITY_ID=" + ddquality.SelectedValue + "";
        }

        if (DDDesign.SelectedIndex > 0)
        {
            STR = STR + @" AND DESIGN_ID=" + DDDesign.SelectedValue + "";

        }
         if (DDColor.SelectedIndex > 0)
        {
            STR = STR + @" AND COLOR_ID=" + DDColor.SelectedValue + "";
        }
        if (DDShape.SelectedIndex > 0)
        {
            STR = STR + @" AND SHAPE_ID=" + DDShape.SelectedValue + "";
        }
        if (ddSize.SelectedIndex > 0)
        {
            STR = STR + @" AND SIZE_ID=" + ddSize.SelectedValue + "";
        }
       
       
        STR = STR + " group by PCD.PCMDID,PM.PROCESS_NAME,VD1.Category_Name,VD1.Item_Name,VD1.QualityName,VD1.DesignName,VD1.ColorName,VD1.ShapeName,PCM.sizeflag,VD1.SIZEFT,						VD2.SizeFt,VD2.SizeInch,VD1.SizeInch,VD2.SizeMtr,VD1.SizeMtr,VD2.SizeId,VD1.SizeId,ST.Type, VD1.SHADECOLORName,Isnull(FT.FINISHED_TYPE_NAME,''),VD2.Category_Name,VD2.Item_Name,VD2.QualityName,VD2.DesignName,VD2.ColorName,VD2.ShapeName,PCD.ISizeflag,						VD2.SHADECOLORName,STI.Type,PCM.FlagMtrFt,IQTY,PCM.LossPercentageFlag,PCD.ILOSS,IRATE,iu.UnitName,VD3.Category_Name,VD3.Item_Name,VD3.QualityName,VD3.DesignName,						VD3.ColorName,VD3.ShapeName,PCD.OSizeflag,VD3.SizeMtr,VD3.SizeFt,VD3.SizeInch,VD3.SHADECOLORName,VD3.sizeid,STO.type,OQTY,ORATE,OU.UnitName,QCM.QUALITYCODEID,PCM.FlagMtrFt,Dyingmatch,DyingType,Dyeing,OLOSS,OQtyPercentage,Ifinishedid order by PCD.PCMDID";

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, STR);
        DG.DataSource = Ds;
        DG.DataBind();

        //if (Ds.Tables[0].Rows.Count > 0)
        //{
        //    if (Convert.ToInt32(Ds.Tables[0].Rows[0]["FlagMtrFt"]) == 1)
        //    {
        //        ChkForMtr.Checked = true;
        //    }
        //}
       
        if (DG.Rows.Count > 0)
        {
            CheckBox ChkBoxHeader = (CheckBox)DG.HeaderRow.FindControl("ChkAllItem");
            ChkBoxHeader.Checked = true;

            foreach (GridViewRow row in DG.Rows)
            {
                CheckBox Chkboxitem = (CheckBox)row.FindControl("Chkboxitem");

                if (ChkBoxHeader.Checked == true)
                {
                    Chkboxitem.Checked = true;
                }
                else
                {
                    Chkboxitem.Checked = false;
                }
            }
        }
    }
    protected void FillissueGrid()
    {
        string str = @"select dbo.F_getItemDescription(PT.Finishedid,0) as ItemDescription,
                    PT.Lotno,PT.TagNo,PT.IssueQuantity,PM.chalanNo,Replace(CONVERT(nvarchar(11),PM.date,106),' ','-') as IssueDate,PM.prmid,PT.Prtid,PM.processid
                    from ProcessRawMaster_KM PM inner join ProcessRawTran_KM PT on PM.PRMid=PT.PRMid
                     and PM.prmid=" + hnissueid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
        //if (chkEdit.Checked == true)
        //{
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        txtissueno.Text = ds.Tables[0].Rows[0]["chalanno"].ToString();
        //        txtissuedate.Text = ds.Tables[0].Rows[0]["issuedate"].ToString();
        //        TxtEWayBillNo.Text = ds.Tables[0].Rows[0]["EWayBillNo"].ToString();
        //    }
        //    else
        //    {
        //        txtissueno.Text = "";
        //        txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        //    }

        //}

    }
    protected void FillEditGrid()
    {
        string str = @"select dbo.F_getItemDescription(PT.Finishedid,0) as ItemDescription,
                    PT.Lotno,PT.TagNo,PT.IssueQuantity,PM.chalanNo,Replace(CONVERT(nvarchar(11),PM.date,106),' ','-') as IssueDate,PM.prmid,PT.Prtid,PM.processid
                    from ProcessRawMaster_KM PM inner join ProcessRawTran_KM PT on PM.PRMid=PT.PRMid
                     and PM.kitno='" + txtissueno.Text+"'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
        //if (chkEdit.Checked == true)
        //{
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        txtissueno.Text = ds.Tables[0].Rows[0]["chalanno"].ToString();
        //        txtissuedate.Text = ds.Tables[0].Rows[0]["issuedate"].ToString();
        //        TxtEWayBillNo.Text = ds.Tables[0].Rows[0]["EWayBillNo"].ToString();
        //    }
        //    else
        //    {
        //        txtissueno.Text = "";
        //        txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        //    }

        //}

    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            for (int i = 0; i < DG.Columns.Count; i++)
            {
                if (variable.VarBINNOWISE == "1")
                {
                    if (DG.Columns[i].HeaderText == "BinNo")
                    {
                        DG.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (DG.Columns[i].HeaderText == "BinNo")
                    {
                        DG.Columns[i].Visible = false;
                    }
                }
            }
            DropDownList DDGodown = ((DropDownList)e.Row.FindControl("DDGodown"));
            string str = @"Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName
                           select godownid From Modulewisegodown Where ModuleName='" + Page.Title + "'";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDGodown, ds, 0, true, "--Plz Select--");
           

            if (hngodownid.Value == "0")
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (DDGodown.Items.FindByValue(ds.Tables[1].Rows[0]["godownid"].ToString()) != null)
                    {
                        DDGodown.SelectedValue = ds.Tables[1].Rows[0]["godownid"].ToString();
                    }
                }
                else
                {
                    if (DDGodown.Items.Count > 0)
                    {
                        DDGodown.SelectedValue = "1";
                    }
                }
            }
            else
            {
                if (DDGodown.Items.FindByValue(hngodownid.Value) != null)
                {
                    DDGodown.SelectedValue = hngodownid.Value;
                }
            }
            DDgodown_SelectedIndexChanged(DDGodown, new EventArgs());
            ds.Dispose();
        }
    }
    protected void DDBinNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList DDBinNo = (DropDownList)sender;
        GridViewRow row = (GridViewRow)DDBinNo.Parent.Parent;
        Label Ifinishedid = ((Label)row.FindControl("lblifinishedid"));
        DropDownList ddlgodown = ((DropDownList)row.FindControl("DDGodown"));

        DropDownList ddLotno = ((DropDownList)row.FindControl("DDLotNo"));
        if (Session["VarcompanyNo"].ToString() == "22")
        {
            SqlParameter[] array = new SqlParameter[4];
            array[0] = new SqlParameter("@CompanyId", DDCompanyName.SelectedValue);
            array[1] = new SqlParameter("@GodownId", ddlgodown.SelectedValue);
            array[2] = new SqlParameter("@item_finished_id", Ifinishedid.Text);
            array[3] = new SqlParameter("@MASTERCOMPANYID", Session["varcompanyNo"]);
            array[4] = new SqlParameter("@BinNo", variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : "");

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_DDBIND_LOTNO", array);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddLotno.DataSource = ds;
                ddLotno.DataTextField = "Lotno";
                ddLotno.DataValueField = "Lotno";
                ddLotno.DataBind();
                ddLotno.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }
        else
        {
            string str = "select Distinct S.Lotno,S.Lotno from Stock S Where S.companyid=" + DDCompanyName.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Qtyinhand>0";
            if (variable.VarBINNOWISE == "1")
            {
                str = str + " and BinNo='" + DDBinNo.SelectedItem.Text + "'";
            }
            UtilityModule.ConditionalComboFill(ref ddLotno, str, true, "--Plz Select--");
        }

        if (ddLotno.Items.Count > 0)
        {
            ddLotno.SelectedIndex = 1;
            DDLotno_SelectedIndexChanged(ddLotno, e);
        }
    }
    protected void DDgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlgodown = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlgodown.Parent.Parent;
        Label Ifinishedid = ((Label)row.FindControl("lblifinishedid"));

        if (variable.VarBINNOWISE == "1")
        {
            DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));
            string str = "select Distinct S.BinNo,S.BinNo from Stock S Where S.companyid=" + DDCompanyName.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Qtyinhand>0";
            UtilityModule.ConditionalComboFill(ref DDBinNo, str, true, "--Plz Select--");
            if (DDBinNo.Items.Count > 0)
            {
                DDBinNo.SelectedIndex = 1;
            }
        }
        else
        {
            int index = row.RowIndex;

            DropDownList ddLotno = ((DropDownList)row.FindControl("DDLotNo"));

            if (Session["VarcompanyNo"].ToString() == "22")
            {
                SqlParameter[] array = new SqlParameter[4];
                array[0] = new SqlParameter("@CompanyId", DDCompanyName.SelectedValue);
                array[1] = new SqlParameter("@GodownId", ddlgodown.SelectedValue);
                array[2] = new SqlParameter("@item_finished_id", Ifinishedid.Text);
                array[3] = new SqlParameter("@MASTERCOMPANYID", Session["varcompanyNo"]);

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_DDBIND_LOTNO", array);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddLotno.DataSource = ds;
                    ddLotno.DataTextField = "Lotno";
                    ddLotno.DataValueField = "Lotno";
                    ddLotno.DataBind();
                    ddLotno.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
            else
            {
                string str = "select Distinct S.Lotno,S.Lotno from Stock S Where S.companyid=" + DDCompanyName.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Qtyinhand>0";
                UtilityModule.ConditionalComboFill(ref ddLotno, str, true, "--Plz Select--");
            }

            if (ddLotno.Items.Count > 0)
            {
                ddLotno.SelectedIndex = 1;
                DDLotno_SelectedIndexChanged(ddLotno, e);
            }
        }

    }
    protected void DDLotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlLotno = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlLotno.Parent.Parent;
        int index = row.RowIndex;
        //Label Ifinishedid = ((Label)DG.Rows[index].FindControl("lblifinishedid"));
        Label Ifinishedid = (Label)row.FindControl("lblifinishedid");
        //DropDownList DDTagNo = ((DropDownList)DG.Rows[index].FindControl("DDTagNo"));
        //DropDownList ddlgodown = ((DropDownList)DG.Rows[index].FindControl("DDgodown"));
        DropDownList DDTagNo = ((DropDownList)row.FindControl("DDTagNo"));
        DropDownList ddlgodown = ((DropDownList)row.FindControl("DDgodown"));
        DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));

        if (Session["VarcompanyNo"].ToString() == "22")
        {
            SqlParameter[] array = new SqlParameter[6];
            array[0] = new SqlParameter("@CompanyId", DDCompanyName.SelectedValue);
            array[1] = new SqlParameter("@GodownId", ddlgodown.SelectedValue);
            array[2] = new SqlParameter("@item_finished_id", Ifinishedid.Text);
            array[3] = new SqlParameter("@Lotno", ddlLotno.Text);
            array[4] = new SqlParameter("@BinNo", variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : "");
            array[5] = new SqlParameter("@MASTERCOMPANYID", Session["varcompanyNo"]);


            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_DDBIND_TAGNO", array);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DDTagNo.DataSource = ds;
                DDTagNo.DataTextField = "TagNo";
                DDTagNo.DataValueField = "TagNo";
                DDTagNo.DataBind();
                DDTagNo.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }
        else
        {
            string str = "select Distinct S.TagNo,S.Tagno from Stock S Where S.companyid=" + DDCompanyName.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Lotno='" + ddlLotno.Text + "' and S.Qtyinhand>0";
            if (variable.VarBINNOWISE == "1")
            {
                str = str + " and BinNo='" + DDBinNo.SelectedItem.Text + "'";
            }
            UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "--Plz Select--");
        }


        if (DDTagNo.Items.Count > 0)
        {
            DDTagNo.SelectedIndex = 1;
            DDTagno_SelectedIndexChanged(DDTagNo, e);
        }
    }
    protected void DDTagno_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddTagno = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddTagno.Parent.Parent;
        int index = row.RowIndex;
        int Ifinishedid = Convert.ToInt32(((Label)row.FindControl("lblifinishedid")).Text);
        Label lblstockqty = ((Label)row.FindControl("lblstockqty"));
        DropDownList ddgodown = ((DropDownList)row.FindControl("DDgodown"));
        DropDownList ddlotno = ((DropDownList)row.FindControl("DDLotNo"));
     //   DropDownList ddTagno = ((DropDownList)row.FindControl("DDLotNo"));
        DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));

        Double StockQty = UtilityModule.getstockQty(DDCompanyName.SelectedValue, ddgodown.SelectedValue, ddlotno.Text, Ifinishedid, ddTagno.Text, BinNo: (variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : ""));

        string strfinal = @"select isnull(SUM(IssueQuantity),0) as  qty  from  ProcessRawTran_km pk  left JOIN ProcessRawMaster pm on pk.PRMid=pm.kitid where PM.kitno IS  NULL and Finishedid=" + Ifinishedid + " and  Lotno='" + ddlotno.SelectedValue + "' and TagNo='" + ddTagno.SelectedValue + "' and Godownid=" + ddgodown.SelectedValue + " ";
        DataSet dsfinal = new DataSet();
        dsfinal = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strfinal);
        decimal issuedstockqty = 0, finalqty = 0;
        if (dsfinal != null)
        {

            if (dsfinal.Tables.Count > 0)
            {
                if (dsfinal.Tables[0].Rows.Count > 0)
                {

                    issuedstockqty = Convert.ToDecimal(dsfinal.Tables[0].Rows[0]["qty"] == null ? "0" : dsfinal.Tables[0].Rows[0]["qty"]);
                }

            }
        }
        finalqty = Convert.ToDecimal(lblstockqty.Text) - issuedstockqty;
        if (finalqty == 0)
        {
            ((DropDownList)row.FindControl("DDLotNo")).Items.Remove(((DropDownList)row.FindControl("DDLotNo")).Items.FindByValue(ddlotno.SelectedValue));
            ((DropDownList)row.FindControl("ddTagno")).Items.Remove(((DropDownList)row.FindControl("ddTagno")).Items.FindByValue(ddTagno.SelectedValue));
            // ((DropDownList)row.FindControl("DDgodown")).Items.Remove(((DropDownList)row.FindControl("DDgodown")).Items.FindByValue(ddgodown.SelectedValue));

        }
        lblstockqty.Text = StockQty.ToString();



    }

    protected void gvdetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvdetail.EditIndex = e.NewEditIndex;
        if (chkEdit.Checked)
        {
            FillEditGrid();
            
        }
        else
        {
            FillissueGrid();
        
        }
    }
    protected void gvdetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvdetail.EditIndex = -1;
        if (chkEdit.Checked)
        {
            FillEditGrid();
            
        }
        else
        {

            FillissueGrid();
        }
    }
    protected void gvdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblhqty = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblhqty");
            Label lblprmid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprmid");
            Label lblprtid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprtid");
            Label lblprorderid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprorderid");
            TextBox txtqty = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtqty");
            Label lblprocessid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprocessid");
            //**************
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter("@prmid", lblprmid.Text);
            param[1] = new SqlParameter("@prtid", lblprtid.Text);
            param[2] = new SqlParameter("@hqty", lblhqty.Text);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@qty", txtqty.Text == "" ? "0" : txtqty.Text);
            param[5] = new SqlParameter("@processid", lblprocessid.Text);
            param[6] = new SqlParameter("@userid", Session["varuserid"]);
            param[7] = new SqlParameter("@EWayBillNo", "");
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEKMISSUE", param);
            lblMessage.Text = param[3].Value.ToString();
            Tran.Commit();
            gvdetail.EditIndex = -1;
            if (chkEdit.Checked)
            {
                FillEditGrid();
                
            }
            else
            {
                FillissueGrid();

            }
           // FillissueGrid();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblprmid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprmid");
            Label lblprtid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprtid");
            Label lblprorderid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprorderid");
            Label lblprocessid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprocessid");

            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@prmid", lblprmid.Text);
            param[1] = new SqlParameter("@prtid", lblprtid.Text);
            param[2] = new SqlParameter("@Processid", lblprocessid.Text);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEKMISSUE", param);
            lblMessage.Text = param[3].Value.ToString();
            Tran.Commit();
            if (chkEdit.Checked)
            {
                FillEditGrid();

            }
            else
            {
                FillissueGrid();

            }
            //***************
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
        //BtnUpdateStockNoQty.Visible = false;
        //DDLoomNo.SelectedIndex = -1;
        //DDFoliono.SelectedIndex = -1;
        //TDIssueNo.Visible = false;
        DG.DataSource = null;
        DG.DataBind();
        gvdetail.DataSource = null;
        gvdetail.DataBind();
        txtissueno.Text = "";
       // txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

        //if (variable.VarWeaverRawMaterialIssueToCompleteStatus == "1" && Session["usertype"].ToString() == "1")
        //{
        //    TDForCompleteStatus.Visible = true;
        //}

        if (chkEdit.Checked == true)
        {
            txtissueno.Enabled = true;
            //TDIssueNo.Visible = true;
            //DDissueno.SelectedIndex = -1;
            //if (Session["varMasterCompanyIDForERP"].ToString() == "21" || Session["varMasterCompanyIDForERP"].ToString() == "16" || Session["varMasterCompanyIDForERP"].ToString() == "28")
            //{
            //    if (variable.VarWeaverRawMaterialIssueToCompleteStatus == "1" && Session["usertype"].ToString() == "1")
            //    {
            //        TDForCompleteStatus.Visible = true;
            //        //ChkForCompleteStatus.Checked = false;
            //    }
            //}
            //else
            //{
                //if (variable.VarWeaverRawMaterialIssueToCompleteStatus == "1" && Session["usertype"].ToString() == "1")
                //{
                //    TDForCompleteStatus.Visible = false;
                //    ChkForCompleteStatus.Checked = false;
                //}
          //  }

            //if (Session["varMasterCompanyIDForERP"].ToString() == "21")
            //{
            //    BtnUpdateStockNoQty.Visible = true;
            //}
        }
    }
    protected void txtissueno_TextChanged(object sender, EventArgs e)
    {
        int pcs = 0;
        if (!string.IsNullOrEmpty(txtissueno.Text))
        {
            FillEditGrid();
        }


       // FILLGRID(pcs);
    }
}