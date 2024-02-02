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

public partial class Masters_Loom_FrmDouraReportSaveDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CI.CompanyId,CompanyName 
                        From CompanyInfo CI(Nolock) 
                        inner Join Company_Authentication CA(Nolock) on CA.CompanyId = CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by CompanyName 
                        Select ID, BranchName 
                        From BRANCHMASTER BM(nolock) 
                        JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                        Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @"
                        Select EI.EmpId,EI.Empcode+' ['+EI.Empname+']' Empname 
                        From EmpInfo EI(Nolock) 
                        inner join Department D(Nolock) on EI.Departmentid = D.DepartmentId And D.DepartmentName = 'PRODUCTION' And EI.Status = 'P' 
                        And EI.Blacklist = 0 order by Empname
                        select ICM.Category_id,ICM.CATEGORY_NAME From Item_category_Master  ICM 
                          inner join CategorySeparate CS on ICM.CATEGORY_ID=Cs.Categoryid and Cs.id=0 and ICM.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDBranch, ds, 1, false, "");
            DDBranch.Enabled = false;
            if (DDBranch.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }
            //UtilityModule.ConditionalComboFillWithDS(ref DDunitname, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 3, true, "---Plz Select---");


            txtDouraDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }

    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_EmployeeName();
    }
    protected void Fill_EmployeeName()
    {
        //TxtIssRecNo.Text = "";
        string Str = @"Select Distinct a.IssueOrderId, a.CHALLANNO 
            From PROCESS_ISSUE_MASTER_1 a(Nolock) 
            JOIN Employee_ProcessOrderNo EPO(Nolock) ON EPO.IssueOrderId = a.IssueOrderId And EPO.ProcessId = 1 And EPO.EmpID = " + DDEmployeeName.SelectedValue + @" 
            Where a.Companyid = " + DDcompany.SelectedValue + "  Order By a.IssueOrderId Desc";


        UtilityModule.ConditionalComboFill(ref DDFolioNo, Str, true, "--Plz Select--");
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "select IM.ITEM_ID,IM.ITEM_NAME From Item_Master IM Where IM.category_id=" + DDCategory.SelectedValue + " order by ITEM_NAME", true, "--Plz Select--");
        Fillcombo();
    }
    protected void Fillcombo()
    {
        Trquality.Visible = false;
        Trdesign.Visible = false;
        Trcolor.Visible = false;
        Trsize.Visible = false;
        Trshadecolor.Visible = false;
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
                        Trquality.Visible = true;
                        break;
                    case "2":
                        Trdesign.Visible = true;
                        break;
                    case "3":
                        Trcolor.Visible = true;
                        break;
                    case "5":
                        Trsize.Visible = true;
                        break;
                    case "6":
                        Trshadecolor.Visible = true;
                        break;
                }
            }
        }       

    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQuality();
    }
    protected void FillQuality()
    {
        string str = @"select Distinct Q.QualityId,Q.QualityName From ITEM_MASTER IM inner Join Quality Q on Q.Item_Id=IM.ITEM_ID inner Join CategorySeparate cs on IM.CATEGORY_ID=cs.Categoryid where 1=1";
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + "  and IM.Category_id=" + DDCategory.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + "  and IM.Item_id=" + DDItemName.SelectedValue;
        }
        str = str + "  order by QualityName";
        UtilityModule.ConditionalComboFill(ref DDQuality, str, true, "---Plz Select---");
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {

        FillDesign();
        FillSize();
        if (Trshadecolor.Visible == true)
        {
            FillShade();
        }
    }
    protected void FillDesign()
    {
        string str = @"select Distinct Vf.designId,vf.designName From V_finishedItemDetail Vf Where Vf.designId>0";
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + "  and vf.Item_id=" + DDItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
        }

        str = str + "  order by designName";
        UtilityModule.ConditionalComboFill(ref DDDesign, str, true, "---Plz Select---");
    }
    protected void FillShade()
    {
        string str = @"select Distinct ShadecolorId,ShadeColorName From V_finishedItemDetail Vf Where Vf.ShadecolorId>0";
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + "  and vf.Item_id=" + DDItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
        }

        str = str + "  order by ShadeColorName";
        UtilityModule.ConditionalComboFill(ref DDshade, str, true, "---Plz Select---");
    }
    protected void FillColor()
    {
        string str = @"select Distinct Vf.Colorid,vf.Colorname From V_finishedItemDetail Vf Where Vf.Colorid>0";
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + "  and vf.Item_id=" + DDItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + "  and vf.DesignId=" + DDDesign.SelectedValue;
        }


        str = str + "  order by Colorname";
        UtilityModule.ConditionalComboFill(ref DDColor, str, true, "---Plz Select---");
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillColor();
    }
    protected void FillSize()
    {
        string size = "ProdSizeFt";
        if (Chkmtrsize.Checked == true)
        {
            size = "ProdSizemtr";
        }

        string str = @"select Distinct  Vf.Sizeid,vf." + size + " as Size  From V_finishedItemDetail Vf Where Vf.Sizeid>0";
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + "  and vf.Item_id=" + DDItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + "  and vf.DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + "  and vf.Colorid=" + DDColor.SelectedValue;
        }

        str = str + "  order by Size";
        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "---Plz Select---");
    }
    protected void Chkmtrsize_CheckedChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void DDFolioNo_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void BtnShowData_Click(object sender, EventArgs e)
    {
        try
        {
            string str = "";

          
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Issueorderid=" + DDFolioNo.SelectedValue;
            //}
            if (DDItemName.SelectedIndex > 0)
            {
                str = str + " and VF.Item_id=" + DDItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and VF.Qualityid=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and VF.DesignId=" + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " and VF.Colorid=" + DDColor.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and VF.Sizeid=" + DDSize.SelectedValue;
            }            


            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETDOURAREPORTINSPECTIONDATA", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@Companyid", DDcompany.SelectedValue);
            //cmd.Parameters.AddWithValue("@EMPID", DDWeaver.SelectedIndex > 0 ? DDWeaver.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@IssueOrderId", DDFolioNo.SelectedValue); 
            cmd.Parameters.AddWithValue("@WHERE", str);            
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();
            //***********
            if (ds.Tables[0].Rows.Count > 0)
            {
                DG.DataSource = ds.Tables[0];
                DG.DataBind();
            }
            else
            {
                DG.DataSource = "";
                DG.DataBind();

                ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    private void CHECKVALIDCONTROL()
    {

        lblmsg.Text = "";
        //if (UtilityModule.VALIDDROPDOWNLIST(DDCompany) == false)
        //{
        //    goto a;
        //}   

        if (UtilityModule.VALIDDROPDOWNLIST(DDEmployeeName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDFolioNo) == false)
        {
            goto a;
        }       
        else
        {
            goto B;
        }
    a:
        lblmsg.Visible = true;
    UtilityModule.SHOWMSG(lblmsg);
    B: ;
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();

        if (lblmsg.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            //for (int i = 0; i < DG.Rows.Count; i++)
            //{
            //    //CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));

            //    Label lblTStockNo = ((Label)DG.Rows[i].FindControl("lblTStockNo"));
            //    Label lblIssueOrderID = ((Label)DG.Rows[i].FindControl("lblIssueOrderID"));
            //    Label lblItemFinishedId = ((Label)DG.Rows[i].FindControl("lblItemFinishedId"));

            //    TextBox txtOffLoom = ((TextBox)DG.Rows[i].FindControl("txtOffLoom"));
            //    TextBox txtLoomPosition = ((TextBox)DG.Rows[i].FindControl("txtLoomPosition"));               

            //    //if (Chkboxitem.Checked == false)   // Change when Updated Completed
            //    //{
            //    //    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please Select Checkbox');", true);               
            //    //    return;
            //    //}
               
            //    if (txtOffLoom.Text=="" && txtLoomPosition.Text=="" )   // Change when Updated Completed
            //    {
            //        ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Off Loom and Loom Position Can Not Be Blank');", true);
            //        //txtReceiveNoOfSet.Focus();
            //        return;
            //    }
            //    //if (Convert.ToDecimal(txtReceiveQty.Text == "" ? "0" : txtReceiveQty.Text) > Convert.ToDecimal(lblBalToRecQty.Text) && Chkboxitem.Checked == true)   // Change when Updated Completed
            //    //{
            //    //    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Receive qty can not be greater than balance qty');", true);
            //    //    txtReceiveQty.Text = "";
            //    //    txtReceiveQty.Focus();
            //    //    return;
            //    //}
            //}

            string Strdetail = "";
            for (int i = 0; i < DG.Rows.Count; i++)
            {
                //CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));

                Label lblTStockNo = ((Label)DG.Rows[i].FindControl("lblTStockNo"));
                Label lblIssueOrderID = ((Label)DG.Rows[i].FindControl("lblIssueOrderID"));
                Label lblItemFinishedId = ((Label)DG.Rows[i].FindControl("lblItemFinishedId"));

                TextBox txtOffLoom = ((TextBox)DG.Rows[i].FindControl("txtOffLoom"));
                TextBox txtLoomPosition = ((TextBox)DG.Rows[i].FindControl("txtLoomPosition")); 


                if (txtOffLoom.Text!="" && txtLoomPosition.Text!="" && DDFolioNo.SelectedIndex > 0 && DDEmployeeName.SelectedIndex>0)
                {
                    Strdetail = Strdetail + lblTStockNo.Text + '|' + lblIssueOrderID.Text + '|' + lblItemFinishedId.Text + '|' + txtOffLoom.Text + '|' + txtLoomPosition.Text + '~';
                }
            }


            if (Strdetail != "")
            {
                SqlTransaction Tran = con.BeginTransaction();
                try
                {

                    //        //******
                    SqlCommand cmd = new SqlCommand("Pro_SaveDouraReportInspectionData", con, Tran);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 3000;
                    cmd.Parameters.Add("@DouraID", SqlDbType.Int);
                    cmd.Parameters["@DouraID"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["@DouraID"].Value = 0;
                    //cmd.Parameters["@DouraID"].Value = hnDouraId.Value;
                    cmd.Parameters.AddWithValue("@Companyid", DDcompany.SelectedValue);
                    cmd.Parameters.AddWithValue("@ProcessID", 1);                    
                    cmd.Parameters.AddWithValue("@DouraDate", txtDouraDate.Text);
                    cmd.Parameters.AddWithValue("@DouraInspector", txtDouraInspector.Text);
                    cmd.Parameters.AddWithValue("@Remarks", txtDouraRemark.Text);
                    cmd.Parameters.AddWithValue("@StringDetail", Strdetail);
                    cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
                    cmd.Parameters.AddWithValue("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
                    cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    cmd.Parameters["@msg"].Direction = ParameterDirection.Output; 
                 
                    //cmd.Parameters.Add("@ChallanNo", SqlDbType.VarChar, 30);
                    //cmd.Parameters["@ChallanNo"].Direction = ParameterDirection.InputOutput;
                    //cmd.Parameters["@ChallanNo"].Value = "";
                   
                    cmd.ExecuteNonQuery();
                    if (cmd.Parameters["@msg"].Value.ToString() != "") //IF DATA NOT SAVED
                    {
                        lblmsg.Text = cmd.Parameters["@msg"].Value.ToString();
                        Tran.Rollback();
                    }
                    else
                    {
                        lblmsg.Text = "Data Saved Successfully.";
                        Tran.Commit();                        
                        hnDouraId.Value = cmd.Parameters["@DouraID"].Value.ToString();// param[0].Value.ToString();
                        //txtIssueNo.Text = cmd.Parameters["@ChallanNo"].Value.ToString();

                        DG.DataSource = "";
                        DG.DataBind();
                       
                    }
                    //******                    

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
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please fill atleast one row to save data.');", true);
            }
        }
       
    }
    protected void Refreshcontrol()
    {
        //txtstockno.Text = "";
        //txtstockno.Focus();
    }
//    protected void FillGrid()
//    {
//        string str = @"Select a.IssRecNo, VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + VF.SizeFt + ' ' + VF.ShadeColorName [Description], 
//                a.TStockNo, a.IssueOrderID, a.StockNo 
//                From ProcessIssueAttachMasterPC a(Nolock) 
//                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = a.ITEM_FINISHED_ID 
//                Where a.IssRecFlag = " + DDIssRecType.SelectedValue + " And IssueOrderID = " + DDFolioNo.SelectedValue;

//        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
//        DG.DataSource = ds.Tables[0];
//        DG.DataBind();
//    }

    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        }
    }   
    
}