using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_YarnOpening_frmyarnopeningIssue : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select CI.CompanyId,CompanyName 
                            From CompanyInfo CI 
                            JOIN Company_Authentication CA on CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + @" And 
                            CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by CompanyName ";
            if (variable.VarYARNOPENINGISSUEEMPWISE == "1")
            {
                lblyarnopendept.Text = "Employee Name";
                TDdept.Visible = true;

                str = str + @"     select EI.EmpId,EI.EmpName + CASE WHEN EI.EMPCODE<>'' THEN ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME from empinfo EI inner join Department D 
                           on EI.departmentId=D.DepartmentId Where D.DepartmentName in('Yarn Opening','WEFT DEPARTMENT')
                           and isnull(Ei.Blacklist,0)=0 order by EmpName  ";
            }
            else
            {
                str = str + @"  select Distinct EI.EmpId,EI.EmpName+case when isnull(EI.empcode,'')<>'' Then ' ['+EI.empcode+']' Else '' End Empname from empinfo EI inner join Department D 
                           on EI.departmentId=D.DepartmentId Where D.DepartmentName='Yarn Opening'  order by EmpName";
            }
            str = str + @" select Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner Join CategorySeparate CS on ICM.CATEGORY_ID=CS.Categoryid and cs.id=1 order by ICM.CATEGORY_NAME
                          select D.Departmentid,D.DepartmentName From Department D Where D.DepartmentName in('Yarn Opening','WEFT DEPARTMENT') 
                 Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @"
                Select ConeType, ConeType From ConeMaster Order By SrNo ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 4, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDvendor, ds, 1, false, "");
            if (DDvendor.Items.Count > 0)
            {
                DDvendor_SelectedIndexChanged(sender, new EventArgs());
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDdept, ds, 3, true, "--Select Department--");
            if (DDdept.Items.Count > 0)
            {
                DDdept.SelectedIndex = 1;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDconetype, ds, 5, false, "");

            ViewState["id"] = "0";
            ViewState["OrderCategoryId"] = 0;
            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttargetdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (variable.Carpetcompany == "1")
            {
                TRempcodescan.Visible = true;
            }
            //********Bin No
            if (variable.VarBINNOWISE == "1")
            {
                TDBinno.Visible = true;
            }
            if (Session["varMasterCompanyIDForERP"].ToString() == "21")
            {
                Tdcustcode.Visible = false;
                Tdorderno.Visible = false;
                Tdrate.Visible = false;
            }

            if ((Session["varMasterCompanyIDForERP"].ToString() == "16" || Session["varMasterCompanyIDForERP"].ToString() == "28") && (Convert.ToInt32(Session["usertype"])) != 1)
            {
                txtrate.Enabled = false;
            }
        }
    }

    protected void FillCombo()
    {
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
        TDShade.Visible = false;
        TDShape.Visible = false;
        TDSize.Visible = false;
        TDShade.Visible = false;

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
                    case "2":
                        TDDesign.Visible = true;
                        break;
                    case "3":
                        TDColor.Visible = true;
                        break;
                    case "4":
                        TDShape.Visible = true;
                        break;
                    case "5":
                        TDSize.Visible = true;
                        UtilityModule.ConditionalComboFill(ref DDsizetype, "select val,Type from SizeType Order by val", false, "");
                        break;
                    case "6":
                        TDShade.Visible = true;
                        break;
                }
            }
        }
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select Item_Id,ITEM_NAME from Item_master Where CATEGORY_ID=" + ddCatagory.SelectedValue + " order by ITEM_NAME";

        if (Session["varMasterCompanyIDForERP"].ToString() == "16" && Convert.ToInt32(ViewState["OrderCategoryId"]) == 1)
        {
            str = @"Select Distinct VF.Item_ID, VF.Item_NAME 
                From ORDER_CONSUMPTION_DETAIL OCD(Nolock) 
                JOIN OrderDetail OD(Nolock) ON OD.OrderID = OCD.OrderID And OD.OrderDetailId = OCD.ORDERDETAILID 
                JOIN Jobassigns J(Nolock) ON J.OrderId = OD.ORDERID And J.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OCD.IFINISHEDID 
                JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = OCD.PROCESSID 
                Where OCD.OrderID = " + DDorderno.SelectedValue + @"
                And OCD.ProcessID = 5 And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + " Order By VF.ITEM_NAME";
        }

        UtilityModule.ConditionalComboFill(ref dditemname, str, true, "--Plz Select--");
        FillCombo();
    }
    protected void FillQDCS()
    {
        string str = null;
        //Quality
        if (TDQuality.Visible == true)
        {

            str = "select Distinct QualityId,QualityName from Quality Where Item_Id=" + dditemname.SelectedValue + " order by QualityName";

            if (Session["varMasterCompanyIDForERP"].ToString() == "16" && Convert.ToInt32(ViewState["OrderCategoryId"]) == 1)
            {
                str = @"Select Distinct VF.QualityId, VF.QualityName 
                From ORDER_CONSUMPTION_DETAIL OCD(Nolock) 
                JOIN OrderDetail OD(Nolock) ON OD.OrderID = OCD.OrderID And OD.OrderDetailId = OCD.ORDERDETAILID 
                JOIN Jobassigns J(Nolock) ON J.OrderId = OD.ORDERID And J.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OCD.IFINISHEDID 
                JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = OCD.PROCESSID 
                Where OCD.OrderID = " + DDorderno.SelectedValue + @"
                And OCD.ProcessID = 5 And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + " And VF.ITEM_ID = " + dditemname.SelectedValue + "  Order By VF.QualityName ";
            }
            UtilityModule.ConditionalComboFill(ref dquality, str, true, "--Select--");

        }
        //Design
        if (TDDesign.Visible == true)
        {
            str = "select Distinct designId,designName from V_FinishedItemDetail vf  Where Item_Id=" + dditemname.SelectedValue + " order by designname";

            if (Session["varMasterCompanyIDForERP"].ToString() == "16" && Convert.ToInt32(ViewState["OrderCategoryId"]) == 1)
            {
                str = @"Select Distinct VF.DesignID, VF.DesignName
                From ORDER_CONSUMPTION_DETAIL OCD(Nolock) 
                JOIN OrderDetail OD(Nolock) ON OD.OrderID = OCD.OrderID And OD.OrderDetailId = OCD.ORDERDETAILID 
                JOIN Jobassigns J(Nolock) ON J.OrderId = OD.ORDERID And J.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OCD.IFINISHEDID 
                JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = OCD.PROCESSID 
                Where OCD.OrderID = " + DDorderno.SelectedValue + @"
                And OCD.ProcessID = 5 And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + " And VF.ITEM_ID = " + dditemname.SelectedValue + "  Order By VF.DesignName";
            }
            UtilityModule.ConditionalComboFill(ref dddesign, str, true, "--Select--");
        }
        //Color
        if (TDColor.Visible == true)
        {
            str = "select Distinct colorid,colorname from V_FinishedItemDetail vf  Where Item_Id=" + dditemname.SelectedValue + " order by Colorname";

            if (Session["varMasterCompanyIDForERP"].ToString() == "16" && Convert.ToInt32(ViewState["OrderCategoryId"]) == 1)
            {
                str = @"Select Distinct VF.ColorID, VF.ColorName
                From ORDER_CONSUMPTION_DETAIL OCD(Nolock) 
                JOIN OrderDetail OD(Nolock) ON OD.OrderID = OCD.OrderID And OD.OrderDetailId = OCD.ORDERDETAILID 
                JOIN Jobassigns J(Nolock) ON J.OrderId = OD.ORDERID And J.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OCD.IFINISHEDID 
                JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = OCD.PROCESSID 
                Where OCD.OrderID = " + DDorderno.SelectedValue + @"
                And OCD.ProcessID = 5 And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + " And VF.ITEM_ID = " + dditemname.SelectedValue + "  Order By VF.ColorName";
            }
            UtilityModule.ConditionalComboFill(ref ddcolor, str, true, "--Select--");

        }
        //Shape
        if (TDShape.Visible == true)
        {

            str = "select Distinct shapeid,shapename from V_FinishedItemDetail vf  Where Item_Id=" + dditemname.SelectedValue + " order by shapename";

            if (Session["varMasterCompanyIDForERP"].ToString() == "16" && Convert.ToInt32(ViewState["OrderCategoryId"]) == 1)
            {
                str = @"Select Distinct VF.ShapeId, VF.ShapeName
                From ORDER_CONSUMPTION_DETAIL OCD(Nolock) 
                JOIN OrderDetail OD(Nolock) ON OD.OrderID = OCD.OrderID And OD.OrderDetailId = OCD.ORDERDETAILID 
                JOIN Jobassigns J(Nolock) ON J.OrderId = OD.ORDERID And J.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OCD.IFINISHEDID 
                JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = OCD.PROCESSID 
                Where OCD.OrderID = " + DDorderno.SelectedValue + @"
                And OCD.ProcessID = 5 And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + " And VF.ITEM_ID = " + dditemname.SelectedValue + "  Order By VF.ShapeName";
            }
            UtilityModule.ConditionalComboFill(ref ddshape, str, true, "--Select--");

        }
        //Shade
        if (TDShade.Visible == true)
        {
            str = "select shadecolorid,shadecolorname from shadecolor   order by shadecolorname";

            if (Session["varMasterCompanyIDForERP"].ToString() == "16" && Convert.ToInt32(ViewState["OrderCategoryId"]) == 1)
            {
                str = @"Select Distinct VF.ShadecolorId, VF.ShadeColorName
                From ORDER_CONSUMPTION_DETAIL OCD(Nolock) 
                JOIN OrderDetail OD(Nolock) ON OD.OrderID = OCD.OrderID And OD.OrderDetailId = OCD.ORDERDETAILID 
                JOIN Jobassigns J(Nolock) ON J.OrderId = OD.ORDERID And J.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OCD.OFINISHEDID 
                JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = OCD.PROCESSID 
                Where OCD.OrderID = " + DDorderno.SelectedValue + @"
                And OCD.ProcessID = 5 And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + " And VF.ITEM_ID = " + dditemname.SelectedValue + "  Order By VF.ShadeColorName";
            }
            UtilityModule.ConditionalComboFill(ref ddlshade, str, true, "--Select--");
        }
        //Unit
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select Distinct U.UnitId,U.UnitName from ITEM_MASTER IM inner Join Unit U on IM.UnitTypeID=U.UnitTypeID Where Im.Item_id=" + dditemname.SelectedValue + @" order by U.UnitName
                     Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName
                     select godownid From Modulewisegodown Where ModuleName='" + Page.Title + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        UtilityModule.ConditionalComboFillWithDS(ref ddlunit, ds, 0, false, "");
        UtilityModule.ConditionalComboFillWithDS(ref DDGodown, ds, 1, true, "--Select--");
        if (ds.Tables[2].Rows.Count > 0)
        {
            if (DDGodown.Items.FindByValue(ds.Tables[2].Rows[0]["godownid"].ToString()) != null)
            {
                DDGodown.SelectedValue = ds.Tables[2].Rows[0]["godownid"].ToString();
            }
        }
        FillQDCS();
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

        str = "select Distinct sizeid," + size + " from V_FinishedItemDetail vf Where ITEM_ID=" + dditemname.SelectedValue + " and vf.shapeid=" + ddshape.SelectedValue + " order by " + size;

        if (Session["varMasterCompanyIDForERP"].ToString() == "16" && Convert.ToInt32(ViewState["OrderCategoryId"]) == 1)
        {
            str = @"Select Distinct VF.SizeId, VF." + size + @" 
                From ORDER_CONSUMPTION_DETAIL OCD(Nolock) 
                JOIN OrderDetail OD(Nolock) ON OD.OrderID = OCD.OrderID And OD.OrderDetailId = OCD.ORDERDETAILID 
                JOIN Jobassigns J(Nolock) ON J.OrderId = OD.ORDERID And J.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OCD.IFINISHEDID 
                JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = OCD.PROCESSID 
                Where OCD.OrderID = " + DDorderno.SelectedValue + @"
                And OCD.ProcessID = 5 And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + " And VF.ITEM_ID = " + dditemname.SelectedValue + " And VF.ShapeId = " + ddshape.SelectedValue + " Order By VF." + size + "";
        }
        UtilityModule.ConditionalComboFill(ref ddsize, str, true, "--Select--");
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
    protected void DDGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        int varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        ViewState["varfinishedid"] = varfinishedid;
        Filllotno(sender);
    }
    protected void Filllotno(object sender = null)
    {
        DDLotNo.SelectedIndex = -1;
        string str = "select Distinct Lotno,Lotno from Stock Where Item_Finished_id=" + ViewState["varfinishedid"] + " and Godownid=" + DDGodown.SelectedValue + " and CompanyId=" + DDcompany.SelectedValue + " and Round(Qtyinhand,3)>0";

        UtilityModule.ConditionalComboFill(ref DDLotNo, str, true, "--Select--");
        if (DDLotNo.Items.Count > 0)
        {
            DDLotNo.SelectedIndex = 1;
            if (sender != null)
            {
                DDLotNo_SelectedIndexChanged(sender, new EventArgs());
            }
        }
    }
    protected void FillTagno(object sender = null)
    {
        DDTagNo.SelectedIndex = -1;
        string str = "select Distinct TagNo,TagNo from Stock Where Item_Finished_id=" + ViewState["varfinishedid"] + " and Godownid=" + DDGodown.SelectedValue + " and CompanyId=" + DDcompany.SelectedValue + " and Lotno='" + DDLotNo.SelectedItem.Text + "' and Round(Qtyinhand,3)>0";

        UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "--Select--");
        if (DDTagNo.Items.Count > 0)
        {
            DDTagNo.SelectedIndex = 1;
            if (sender != null)
            {
                DDTagNo_SelectedIndexChanged(sender, new EventArgs());
            }
        }
    }
    protected void FillBinNo(object sender = null)
    {
        string str = "select Distinct BinNo,BinNo as BinNo1 from Stock Where Item_Finished_id=" + ViewState["varfinishedid"] + " and Godownid=" + DDGodown.SelectedValue + " and CompanyId=" + DDcompany.SelectedValue + "  and Round(Qtyinhand,3)>0 and Lotno='" + DDLotNo.SelectedItem.Text + "' and TagNo='" + DDTagNo.SelectedItem.Text + "' order by BinNo";
        UtilityModule.ConditionalComboFill(ref DDbinno, str, true, "--Select--");
        if (DDbinno.Items.Count > 0)
        {
            DDbinno.SelectedIndex = 1;
            if (sender != null)
            {
                DDbinno_SelectedIndexChanged(sender, new EventArgs());
            }
        }
    }
    protected void DDLotNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillTagno(sender);
    }
    protected void DDTagNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TDBinno.Visible == true)
        {
            FillBinNo(sender);
        }
        else
        {
            Double StockQty = UtilityModule.getstockQty(DDcompany.SelectedValue, DDGodown.SelectedValue, DDLotNo.SelectedItem.Text, Convert.ToInt32(ViewState["varfinishedid"]), TagNo: DDTagNo.SelectedItem.Text, BinNo: TDBinno.Visible == true ? DDbinno.SelectedItem.Text : "");
            txtstockqty.Text = StockQty.ToString();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"].ToString() == "16" && Convert.ToInt32(ViewState["OrderCategoryId"]) == 1)
        {
            if (DDorderno.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('Please select order no');", true);
                return;
            }
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, Tran, ddlshade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            SqlParameter[] param = new SqlParameter[30];

            param[0] = new SqlParameter("@ID", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = ViewState["id"] == null ? 0 : ViewState["id"];
            param[1] = new SqlParameter("Detailid", SqlDbType.BigInt);
            param[1].Value = 0;
            param[2] = new SqlParameter("@CompanyId", DDcompany.SelectedValue);
            param[3] = new SqlParameter("@vendorid", DDvendor.SelectedValue);
            param[4] = new SqlParameter("IssueNo", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.InputOutput;
            param[4].Value = txtissueno.Text;
            param[5] = new SqlParameter("@IssueDate", txtissuedate.Text);
            param[6] = new SqlParameter("@TargetDate", txttargetdate.Text);
            param[7] = new SqlParameter("@Item_Finished_id", varfinishedid);
            param[8] = new SqlParameter("@GodownId", DDGodown.SelectedValue);
            param[9] = new SqlParameter("@LotNo", DDLotNo.SelectedItem.Text);
            param[10] = new SqlParameter("@TagNo", DDTagNo.SelectedItem.Text);
            param[11] = new SqlParameter("@IssueQty", txtissqty.Text);
            param[12] = new SqlParameter("@Rectype", DDrectype.SelectedItem.Text);
            param[13] = new SqlParameter("@Noofcone", txtnoofcone.Text == "" ? "0" : txtnoofcone.Text);
            param[14] = new SqlParameter("@ConeType", DDconetype.SelectedItem.Text);
            param[15] = new SqlParameter("@Userid", Session["varuserid"]);
            param[16] = new SqlParameter("@Unitid", ddlunit.SelectedValue);
            param[17] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[17].Direction = ParameterDirection.Output;
            param[18] = new SqlParameter("@flagSize", DDsizetype.SelectedValue);
            param[19] = new SqlParameter("@Rate", txtrate.Text == "" ? "0" : txtrate.Text);
            param[20] = new SqlParameter("@BinNo", TDBinno.Visible == true ? DDbinno.SelectedItem.Text : "");
            param[21] = new SqlParameter("@orderid", DDorderno.SelectedIndex > 0 ? DDorderno.SelectedValue : "0");
            param[22] = new SqlParameter("@DepartMentId", DDdept.SelectedIndex > 0 ? DDdept.SelectedValue : "0");
            param[23] = new SqlParameter("@DepartmentName", DDdept.SelectedIndex > 0 ? DDdept.SelectedItem.Text : "");
            param[24] = new SqlParameter("@Remarks", (txtremark.Text).Trim());
            param[25] = new SqlParameter("@BellWt", TxtBellWt.Text == "" ? "0" : TxtBellWt.Text);
            param[26] = new SqlParameter("@PlyType", DDPly.SelectedItem.Text);
            param[27] = new SqlParameter("@TransportType", DDTransport.SelectedItem.Text);
            param[28] = new SqlParameter("@Moisture", TxtMoisture.Text == "" ? "0" : TxtMoisture.Text);
            param[29] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);

            //********************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_SaveYarnOpeningIssue]", param);

            ViewState["id"] = param[0].Value.ToString();
            txtissueno.Text = param[4].Value.ToString();
            Tran.Commit();

            if (param[17].Value.ToString() == "")
            {
                lblmessage.Text = "Data Saved successfully..";
                //********************
                FillGrid();
                Refreshcontrol();
                txtissqty.Text = "";
                txtnoofcone.Text = "";
                TxtBellWt.Text = "";
            }
            else
            {
                lblmessage.Text = param[17].Value.ToString();
                txtissqty.Text = "";
            }
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmessage.Text = ex.Message;
        }
    }
    protected void FillGrid()
    {
        string str = @"select dbo.F_getItemDescription(Yt.Item_Finished_id,YT.flagsize) as ItemDescription,GM.GodownName,YT.Lotno
                      ,Yt.Tagno,U.UnitName,
                    YT.issueQty, YT.Rectype, YT.Noofcone, YT.Conetype,YM.ID,YT.Detailid,YM.issueNo,isnull(YM.Remarks,'') as Remarks, YT.Rate 
                    from YarnOpeningIssueMaster YM inner join YarnOpeningIssueTran YT on YM.ID=YT.MasterId 
                    inner join GodownMaster GM on YT.GodownId=GM.GoDownId
                    inner join Unit U on Yt.Unitid=U.UnitId                     
                    Where YM.Id=" + ViewState["id"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
        if (Chkedit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtissueno.Text = ds.Tables[0].Rows[0]["issueNo"].ToString();
                txtremark.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
            }
        }
    }
    protected void Refreshcontrol()
    {
        dddesign.SelectedIndex = -1;
        ddcolor.SelectedIndex = -1;
        ddshape.SelectedIndex = -1;
        ddsize.SelectedIndex = -1;
        ddlshade.SelectedIndex = -1;
        dddesign.SelectedIndex = -1;
        DDGodown.SelectedIndex = -1;
        DDLotNo.SelectedIndex = -1;
        DDTagNo.SelectedIndex = -1;
        DDbinno.SelectedIndex = -1;
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string str = @"select  * from [V_yarnOpeningIssue] Where id=" + ViewState["id"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptyarnopeningissue.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptyarnopeningissue.xsd";
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
    protected void DDvendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["id"] = "0";
        txtissueno.Text = "";
        UtilityModule.ConditionalComboFill(ref DDcustcode, "select customerid,case when customercode='' Then CompanyName else CustomerCode End as Customercode From customerinfo order by CustomerCode", true, "--Plz Select--");
        FillIssueNo();
    }
    protected void DG_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DG.EditIndex = e.NewEditIndex;
        FillGrid();
    }
    protected void DG_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DG.EditIndex = -1;
        FillGrid();
    }
    protected void DDissuedNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["id"] = DDissuedNo.SelectedValue;
        FillGrid();
        BtnComplete.Visible = true;
    }
    protected void Chkedit_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkedit.Checked == true)
        {
            TDIssuedNo.Visible = true;
            lnkremark.Visible = true;
        }
        else
        {
            TDIssuedNo.Visible = false;
            lnkremark.Visible = false;
            BtnComplete.Visible = false;
        }
        FillIssueNo();
    }
    protected void FillIssueNo()
    {
        string str;

        str = @"select ID,IssueNo+'/'+REPLACE(CONVERT(nvarchar(11),IssueDate,106),' ','-') as IssueNo from YarnOpeningIssueMaster 
        Where companyid=" + DDcompany.SelectedValue + " And BranchID = " + DDBranchName.SelectedValue + " and vendorId=" + DDvendor.SelectedValue + "";

        if (chkforComp.Checked == true)
        {
            str = str + " and Status='Complete'";
        }
        else
        {
            str = str + " and Status='Pending'";
        }
        str = str + "  order by id desc";

        UtilityModule.ConditionalComboFill(ref DDissuedNo, str, true, "--Plz Select--");
    }
    protected void chkforComp_CheckedChanged(object sender, EventArgs e)
    {
        FillIssueNo();
    }
    protected void DG_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            string masterid = ((Label)DG.Rows[e.RowIndex].FindControl("lblid")).Text;
            string Detailid = ((Label)DG.Rows[e.RowIndex].FindControl("lbldetailid")).Text;
            TextBox txtqty = ((TextBox)DG.Rows[e.RowIndex].FindControl("txtqty"));
            TextBox txtrectype = ((TextBox)DG.Rows[e.RowIndex].FindControl("txtrectype"));
            TextBox txtnoofcone = ((TextBox)DG.Rows[e.RowIndex].FindControl("txtnoofcone"));
            TextBox txtconetype = ((TextBox)DG.Rows[e.RowIndex].FindControl("txtconetype"));
            TextBox txtRate = ((TextBox)DG.Rows[e.RowIndex].FindControl("txtRate"));

            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter("@Id", masterid);
            param[1] = new SqlParameter("@Detailid", Detailid);
            param[2] = new SqlParameter("@IssueQty", (txtqty.Text == "" ? "0" : txtqty.Text));
            param[3] = new SqlParameter("@Rectype", txtrectype.Text);
            param[4] = new SqlParameter("@Noofcone", txtnoofcone.Text);
            param[5] = new SqlParameter("@Conetype", txtconetype.Text);
            param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[6].Direction = ParameterDirection.Output;
            param[7] = new SqlParameter("@Rate", txtRate.Text);
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateYarnOpeningIssue", param);
            Tran.Commit();
            lblmessage.Text = param[6].Value.ToString();
            DG.EditIndex = -1;
            FillGrid();
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
    }
    protected void DG_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            string masterid = ((Label)DG.Rows[e.RowIndex].FindControl("lblid")).Text;
            string Detailid = ((Label)DG.Rows[e.RowIndex].FindControl("lbldetailid")).Text;
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@Id", masterid);
            param[1] = new SqlParameter("@Detailid", Detailid);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            //************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteYarnOpeningIssue", param);
            Tran.Commit();
            lblmessage.Text = param[2].Value.ToString();
            FillGrid();

        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
    }

    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (variable.Carpetcompany == "1")
        {
            DDGodown_SelectedIndexChanged(sender, new EventArgs());
        }
    }
    protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    {
        string str = @"select Empid From EmpInfo EI inner join Department Dp on EI.Departmentid=DP.departmentid 
                       and Dp.DepartmentName in('Yarn Opening','WEFT DEPARTMENT') Where EmpCode='" + txtWeaverIdNoscan.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (DDvendor.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) != null)
            {
                DDvendor.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
                DDvendor_SelectedIndexChanged(sender, new EventArgs());
                txtWeaverIdNoscan.Text = "";
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('Employee Code does not exists in this Department.')", true);
            txtWeaverIdNoscan.Focus();
        }
        //        string str = @"select Empid From EmpInfo EI inner join Department Dp on EI.Departmentid=DP.departmentid 
        //                       and Dp.Departmentname='Yarn Opening' Where EmpCode='" + txtWeaverIdNoscan.Text + "'";
        //        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            if (DDvendor.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) != null)
        //            {
        //                DDvendor.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
        //                DDvendor_SelectedIndexChanged(sender, new EventArgs());
        //                txtWeaverIdNoscan.Text = "";
        //            }
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('Employee Code does not exists in this Department.')", true);
        //            txtWeaverIdNoscan.Focus();
        //        }
    }

    protected void DDbinno_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Filllotno(sender);
        Double StockQty = UtilityModule.getstockQty(DDcompany.SelectedValue, DDGodown.SelectedValue, DDLotNo.SelectedItem.Text, Convert.ToInt32(ViewState["varfinishedid"]), TagNo: DDTagNo.SelectedItem.Text, BinNo: DDbinno.SelectedItem.Text);
        txtstockqty.Text = StockQty.ToString();
    }

    protected void DDcustcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDorderno, "select OM.OrderId,om.CustomerOrderNo From Ordermaster Om WHere CompanyId=" + DDcompany.SelectedValue + " and CustomerId=" + DDcustcode.SelectedValue + " and Status=0 order by CustomerorderNo", true, "--Plz Select--");
    }
    protected void DDorderno_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["OrderCategoryId"] = 0;

        if (Session["varMasterCompanyIDForERP"].ToString() == "16")
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select OrderCategoryId From OrderMaster(Nolock) Where OrderID = " + DDorderno.SelectedValue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["OrderCategoryId"] = ds.Tables[0].Rows[0]["OrderCategoryId"];
            }
        }

        if (Session["varMasterCompanyIDForERP"].ToString() == "16" && Convert.ToInt32(ViewState["OrderCategoryId"]) == 1)
        {
            UtilityModule.ConditionalComboFill(ref ddCatagory, @"Select Distinct VF.CATEGORY_ID, VF.CATEGORY_NAME 
                From ORDER_CONSUMPTION_DETAIL OCD(Nolock) 
                JOIN OrderDetail OD(Nolock) ON OD.OrderID = OCD.OrderID And OD.OrderDetailId = OCD.ORDERDETAILID 
                JOIN Jobassigns J(Nolock) ON J.OrderId = OD.ORDERID And J.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OCD.IFINISHEDID 
                JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = OCD.PROCESSID 
                Where OCD.OrderID = " + DDorderno.SelectedValue + @"
                And OCD.ProcessID = 5 Order By VF.CATEGORY_NAME", true, "--Plz Select--");
        }
    }
    protected void DDdept_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @" select EI.EmpId,EI.EmpName + CASE WHEN EI.EMPCODE<>'' THEN ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME from empinfo EI inner join Department D 
                           on EI.departmentId=D.DepartmentId Where isnull(Ei.Blacklist,0)=0 and D.departmentid=" + DDdept.SelectedValue + " order by EmpName  ";
        UtilityModule.ConditionalComboFill(ref DDvendor, str, false, "");
        if (DDvendor.Items.Count > 0)
        {
            DDvendor_SelectedIndexChanged(sender, new EventArgs());
        }
    }
    protected void lnkremark_Click(object sender, EventArgs e)
    {
        string str = "Update YARNOPENINGISSUEMASTER set Remarks=N'" + txtremark.Text.Replace("'", " \'").Trim() + "' where Id=" + DDissuedNo.SelectedValue;
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        lblmessage.Text = "Remark Updated successfully..";

    }
    protected void BtnComplete_Click(object sender, EventArgs e)
    {
        if (Chkedit.Checked == true && DDissuedNo.SelectedIndex > 0)
        {
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update YarnOpeningIssueMaster Set Status = 'Complete' Where ID = " + ViewState["id"]);
        }
    }
}