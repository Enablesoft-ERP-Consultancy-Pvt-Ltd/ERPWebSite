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

public partial class Masters_Process_FrmRateUpdateOnFolioAndReceiveNew : System.Web.UI.Page
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
                    Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By CompanyName 
                    
                    select ITEM_ID,ITEM_NAME from ITEM_MASTER IM with(nolock) Inner Join CategorySeparate CS with(nolock) on 
                    cs.Categoryid=IM.CATEGORY_ID and Cs.id=0 And IM.Mastercompanyid = " + Session["varcompanyid"] + @" 
                    select  ShapeId,ShapeName from Shape with(nolock) 
                    select PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER With(nolock) Where MasterCompanyid = " + Session["varcompanyid"] + @" and PROCESS_NAME='WEAVING' Order By PROCESS_NAME 
                    select ICm.CATEGORY_ID,ICM.CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=Cs.Categoryid and cs.id=0 order by CATEGORY_NAME 
                    select val,Type From Sizetype";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            
            UtilityModule.ConditionalComboFillWithDS(ref DDArticleName, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDShape, ds, 2, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref ddJob, ds, 3, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 4, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizeType, ds, 5, false, "--Plz Select--");           

            if (DDsizeType.Items.FindByValue(variable.VarDefaultSizeId) != null)
            {
                DDsizeType.SelectedValue = variable.VarDefaultSizeId;
            }
            if (DDShape.Items.Count > 0)
            {
                DDShape.SelectedIndex = 0;
            }
            ds.Dispose();
            switch (Session["varcompanyId"].ToString())
            {
                
                default:
                    DivDateRange.Visible = true;    
                    divQuality.Visible = true;
                    divDesign.Visible = true;                    
                    divCategory.Visible = true;
                    if (DDCategory.Items.Count > 0)
                    {
                        DDCategory.SelectedIndex = 1;
                        DDCategory_SelectedIndexChanged(DDCategory, new EventArgs());
                    }
                    break;
            }           
           
            if (variable.VarDEFINEPROCESSRATE_LOCATIONWISE == "1")
            {
                DivRateLocation.Visible = true;

            }
            txtFromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
           
        }
    }
    protected void BindWeavingEmp()
    {
        string str2 = null;

        if (Session["varcompanyId"].ToString() == "16" || Session["varcompanyId"].ToString() == "28")
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
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";

        if (UtilityModule.VALIDDROPDOWNLIST(ddquality) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDDesign) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDColor) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddSize) == false)
        {
            goto a;
        }        
        else
        {
            goto B;
        }
    a:
        lblMessage.Visible = true;
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        string str = "";

        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " and Vf.Category_id=" + DDCategory.SelectedValue;
        }
        if (DDArticleName.SelectedIndex > 0)
        {
            str = str + " and Vf.Item_id=" + DDArticleName.SelectedValue;
        }
        if (ddquality.SelectedIndex > 0)
        {
            str = str + " and Vf.Qualityid=" + ddquality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and vf.Colorid=" + DDColor.SelectedValue;
        }
        str = str + " and vf.shapeId=" + DDShape.SelectedValue;
        //if (DDShape.SelectedIndex > 0)
        //{
        //    str = str + " and vf.shapeId=" + DDShape.SelectedValue;
        //}
        if (ddSize.SelectedIndex > 0)
        {
            str = str + " and vf.Sizeid=" + ddSize.SelectedValue;
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] array = new SqlParameter[12];
            array[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
            array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
            array[2] = new SqlParameter("@RateType", SqlDbType.Int);
            array[3] = new SqlParameter("@RateLocation", SqlDbType.Int);
            array[4] = new SqlParameter("@Rate", SqlDbType.VarChar, 10);
            array[5] = new SqlParameter("@FromDate", SqlDbType.SmallDateTime);
            array[6] = new SqlParameter("@ToDate", SqlDbType.SmallDateTime); 
            array[7] = new SqlParameter("@Mastercompanyid", SqlDbType.Int);
            array[8] = new SqlParameter("@Userid", SqlDbType.Int);
            array[9] = new SqlParameter("@where", SqlDbType.VarChar, 500);
            array[10] = new SqlParameter("@msg", SqlDbType.VarChar, 200);
            array[10].Direction = ParameterDirection.Output;
            array[11] = new SqlParameter("@CommRate", SqlDbType.VarChar, 10);

            array[0].Value = DDCompanyName.SelectedValue;
            array[1].Value = ddJob.SelectedValue;
            array[2].Value = DDRatetype.SelectedValue;
            array[3].Value = DDRateLocation.SelectedValue;
            array[4].Value = txtrate.Text == "" ? "0" : txtrate.Text;
            array[5].Value = txtFromDate.Text;
            array[6].Value = txtToDate.Text;
            array[7].Value = Session["varcompanyId"].ToString();
            array[8].Value = Session["varuserid"].ToString(); 
            array[9].Value = str;
            array[11].Value = txtcommrate.Text == "" ? "0" : txtcommrate.Text;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_RateUpdateOnFolioAndReceiveNew", array);
            //Tran.Commit();
            //ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('Rates Updated Successfully!');", true);

            if (array[10].Value.ToString() != "")
            {
                Tran.Rollback();
                lblMessage.Text = array[10].Value.ToString();                
            }
            else
            {
                Tran.Commit();
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('Rates Updated Successfully...!!!');", true);                
            }

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('" + ex.Message + "');", true);
        }
        finally
        {
            con.Dispose();
            con.Close();
        }

    }   
    protected void ddJob_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddJob.SelectedItem.Text.ToUpper())
        {
            case "WEAVING":
                divratetype.Visible = true;
                break;           
            default:
                divratetype.Visible = false;
                break;
        }       
    }   
    protected void ddquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDDesign, @"select Distinct designId,designName from V_FinishedItemDetail vf with(nolock)
                                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + " and QualityId=" + ddquality.SelectedValue + " And Designid<>0 order by designName", true, "--Plz Select--");
        
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDColor, @"select Distinct Colorid,ColorName from V_FinishedItemDetail vf with(nolock)
                                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + " and QualityId=" + ddquality.SelectedValue + " and DesignId=" + DDDesign.SelectedValue + " And Colorid<>0 order by ColorName", true, "--Plz Select--");
        if (DDColor.Items.Count > 0)
        {
            DDColor_SelectedIndexChanged(sender, new EventArgs());
        }
        
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
                                                            and CS.id=0 and cs.categoryid=" + DDCategory.SelectedValue + " and IM.MasterCompanyid=" + Session["varcompanyId"] + " order by IM.ITEM_NAME", true, "--Plz Select--");

        string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                      " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                      " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
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
        
    }
    protected void DDRateLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["VarCompanyId"].ToString() == "27" || Session["VarCompanyId"].ToString() == "42")
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
        if (Session["VarCompanyId"].ToString() == "16" || Session["VarCompanyId"].ToString() == "28")
        {
            DivWeavingEmployee.Visible = true;
            BindWeavingEmp();
        }
        
    }
    protected void BtnGetRate_Click(object sender, EventArgs e)
    {
         lblMessage.Text = "";
         try
         {
             string str = "";

             if (ddJob.SelectedIndex > 0)
             {
                 str = str + "  And Tj.Jobid= " + ddJob.SelectedValue;
             }
             if (DDCategory.SelectedIndex > 0)
             {
                 str = str + "  And vf.Category_Id= " + DDCategory.SelectedValue;
             }
             if (DDArticleName.SelectedIndex > 0)
             {
                 str = str + "  And vf.Item_Id= " + DDArticleName.SelectedValue;
             }
             if (ddquality.SelectedIndex > 0)
             {
                 str = str + "  And vf.QualityId= " + ddquality.SelectedValue;
             }
             if (DDDesign.SelectedIndex > 0)
             {
                 str = str + "  And vf.DesignId= " + DDDesign.SelectedValue;
             }
             if (DDColor.SelectedIndex > 0)
             {
                 str = str + "  And vf.ColorId= " + DDColor.SelectedValue;
             }
             if (DDShape.SelectedIndex > 0)
             {
                 str = str + "  And vf.ShapeId= " + DDShape.SelectedValue;
             }
             if (ddSize.SelectedIndex > 0)
             {
                 str = str + "  And vf.SizeId= " + ddSize.SelectedValue;
             }

             if (divratetype.Visible == true)
             {
                 if (DDRatetype.SelectedIndex != -1)
                 {
                     str = str + "  And TJ.Ratetype= " + DDRatetype.SelectedValue;
                 }
             }
             if (DivRateLocation.Visible == true)
             {
                 str = str + "  And TJ.RateLocation = " + DDRateLocation.SelectedValue;
             }

             SqlParameter[] param = new SqlParameter[4];
             param[0] = new SqlParameter("@companyid", DDCompanyName.SelectedValue);
             param[1] = new SqlParameter("@where", str);
             param[2] = new SqlParameter("@MasterCompanyId", Session["VarCompanyNo"]);
             param[3] = new SqlParameter("@UserId", Session["VarUserId"]);

             DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GetLatestDefineWeavingJobRate", param);
             if (ds.Tables[0].Rows.Count > 0)
             {
                 txtrate.Text = ds.Tables[0].Rows[0]["UNITRATE"].ToString();
                 txtcommrate.Text = ds.Tables[0].Rows[0]["COMMRATE"].ToString();
             }
             else
             {
                 txtrate.Text = "";
                 txtcommrate.Text = "";
                 lblMessage.Text = "Rate Not Define Please Define Rate For Update";
             }
         }
         catch (Exception ex)
         {
             lblMessage.Text = ex.Message;
         }
    }
}