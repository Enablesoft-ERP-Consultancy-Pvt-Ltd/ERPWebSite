using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_RawMaterial_frmsamplerawissueagni : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName                
                           select pnm.PROCESS_NAME_ID,pnm.process_name from PROCESS_NAME_MASTER PNM inner join UserRightsProcess UP on Pnm.PROCESS_NAME_ID=up.ProcessId and up.Userid=" + Session["varuserid"] + @" and pnm.ProcessType=1 order by PROCESS_NAME               
                           select Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner Join CategorySeparate CS on ICM.CATEGORY_ID=CS.Categoryid and cs.id=1 order by ICM.CATEGORY_NAME
                           Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName
                           select godownid From Modulewisegodown Where ModuleName='" + Page.Title + "'";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDprocess, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDGodown, ds, 3, true, "--Plz Select--");
            if (ds.Tables[4].Rows.Count > 0)
            {
                hnmodulegodownid.Value = ds.Tables[4].Rows[0]["godownid"].ToString();
            }
            if (DDprocess.Items.Count > 0)
            {
                DDprocess.SelectedIndex = 1;
                DDprocess_SelectedIndexChanged(sender, new EventArgs());
            }
            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            switch (MySession.TagNowise)
            {
                case "1":
                    TDTagNo.Visible = true;
                    break;
                default:
                    TDTagNo.Visible = false;
                    break;
            }
            //***********
            if (Session["canedit"].ToString() == "1")
            {
                TDCheckedit.Visible = true;
                TDComplete.Visible = true;
            }
            //*********
            if (variable.Carpetcompany == "1")
            {
                TRempcodescan.Visible = true;
            }
            if (variable.VarBINNOWISE == "1")
            {
                TDBinNo.Visible = true;
            }
        }
    }
    protected void DDprocess_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDvendor, "select EI.EmpId,EI.EmpName + case when isnull(ei.empcode,'')<>'' then ' ['+Ei.empcode+' ]' else '' End as empname  From Empinfo EI inner join EmpProcess EP on EI.empid=EP.EmpId  and EP.ProcessId=" + DDprocess.SelectedValue + " and Ei.mastercompanyid=" + Session["varMasterCompanyIDForERP"] + " order by Ei.EmpName", true, "--Plz Select--");
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select Item_Id,ITEM_NAME from Item_master Where CATEGORY_ID=" + ddCatagory.SelectedValue + " order by ITEM_NAME";
        UtilityModule.ConditionalComboFill(ref dditemname, str, true, "--Plz Select--");
        FillCombo();
    }
    protected void FillCombo()
    {
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
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
    protected void FillQDCS()
    {
        string str = null;
        str = "select Distinct QualityId,QualityName from Quality Where Item_Id=" + dditemname.SelectedValue + @" order by QualityName
               select Distinct D.designId,D.designName from V_FinishedItemDetail vf inner Join Design D on vf.DesignId=D.designid  Where Item_Id=" + dditemname.SelectedValue + @" order by D.designname
               select Distinct C.colorid,C.colorname from V_FinishedItemDetail vf inner Join Color C on Vf.colorid=C.colorid  Where Item_Id=" + dditemname.SelectedValue + @" order by C.Colorname
               select Distinct Sh.shapeid,Sh.shapename from Shape Sh  order by shapeid
               select shadecolorid,shadecolorname from shadecolor   order by shadecolorname";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //Quality
        if (TDQuality.Visible == true)
        {

            //str = "select Distinct QualityId,QualityName from Quality Where Item_Id=" + dditemname.SelectedValue + " order by QualityName";
            UtilityModule.ConditionalComboFillWithDS(ref dquality, ds, 0, true, "--Select--");

        }
        //Design
        if (TDDesign.Visible == true)
        {
            //str = "select Distinct D.designId,D.designName from V_FinishedItemDetail vf inner Join Design D on vf.DesignId=D.designid  Where Item_Id=" + dditemname.SelectedValue + " order by D.designname";
            UtilityModule.ConditionalComboFillWithDS(ref dddesign, ds, 1, true, "--Select--");
        }
        //Color
        if (TDColor.Visible == true)
        {
            //  str = "select Distinct C.colorid,C.colorname from V_FinishedItemDetail vf inner Join Color C on Vf.colorid=C.colorid  Where Item_Id=" + dditemname.SelectedValue + " order by C.Colorname";
            UtilityModule.ConditionalComboFillWithDS(ref ddcolor, ds, 2, true, "--Select--");

        }
        //Shape
        if (TDShape.Visible == true)
        {

            //str = "select Distinct Sh.shapeid,Sh.shapename from Shape Sh  order by shapeid";
            UtilityModule.ConditionalComboFillWithDS(ref ddshape, ds, 3, true, "--Select--");
            if (ddshape.Items.Count > 0)
            {
                ddshape.SelectedIndex = 1;
                ddshape_SelectedIndexChanged(ddshape, new EventArgs());
            }

        }
        //Shade
        if (TDShade.Visible == true)
        {
            //str = "select shadecolorid,shadecolorname from shadecolor   order by shadecolorname";
            UtilityModule.ConditionalComboFillWithDS(ref ddlshade, ds, 4, true, "--Select--");
        }
        //Unit
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDunit, @"select Distinct U.UnitId,U.UnitName From Item_Master IM inner join UNIT_TYPE_MASTER UT on IM.UnitTypeID=UT.UnitTypeID 
        inner join Unit u on UT.UnitTypeID=U.UnitTypeID and IM.item_id=" + dditemname.SelectedValue + " and IM.MastercompanyId=" + Session["varMasterCompanyIDForERP"] + " order by U.UnitName", false, "");
        FillQDCS();
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
    protected void Fillsize()
    {

        string str = null, size = null;
        switch (DDunit.SelectedValue.ToString())
        {
            case "0":
                size = "sizeft";
                break;
            case "1":
                size = "sizemtr";
                break;
            case "2":
                size = "sizeinch";
                break;
            default:
                size = "sizeft";
                break;
        }
        str = "select Distinct Vf.sizeid,Vf." + size + " from V_FinishedItemDetail vf inner join  Size S on vf.Sizeid=S.sizeid Where ITEM_ID=" + dditemname.SelectedValue + " and vf.shapeid=" + ddshape.SelectedValue + "";
        if (TDQuality.Visible == true && dquality.SelectedIndex > 0)
        {
            str = str + " and vf.QualityId=" + dquality.SelectedValue;
        }
        if (TDDesign.Visible == true && dddesign.SelectedIndex > 0)
        {
            str = str + " and vf.designId=" + dddesign.SelectedValue;
        }
        if (TDColor.Visible == true && ddcolor.SelectedIndex > 0)
        {
            str = str + " and vf.colorid=" + ddcolor.SelectedValue;
        }
        str = str + " order by Vf." + size;
        UtilityModule.ConditionalComboFill(ref ddsize, str, true, "--Select--");

    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
    protected void DDvendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillIssueNo();

    }
    protected void FillIssueNo()
    {
        string str = "";
        switch (DDprocess.SelectedValue.ToString())
        {
            case "48": //WEAVING
               
                    str = "select Distinct IssueOrderId,IssueOrderId as Issueorderid1 From Process_Issue_master_" + DDprocess.SelectedValue + @" PIM 
                                       Where PIM.Companyid=" + DDcompany.SelectedValue + " and PIM.Empid=" + DDvendor.SelectedValue + @" 
                                       and PIM.SampleNumber<>'' and isnull(pim.FOLIOSTATUS,0)=0";
                    if (chkforcomplete.Checked == true)
                    {
                        str = str + " and PIM.Status='Complete'";
                    }
                    else
                    {
                        str = str + " and PIM.Status='Pending'";
                    }
                    str = str + " UNION ";
                    str = str + @"select Distinct pim.issueorderid,pim.IssueOrderId as IssueOrderId1 From PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + " PIM inner join PROCESS_ISSUE_DETAIL_" + DDprocess.SelectedValue + @" PID on PIM.IssueOrderId=PID.IssueOrderId and pim.Status<>'canceled'
                     inner join OrderMaster om on pid.orderid=om.OrderId and om.OrderCategoryId=2 Where PIm.companyid=" + DDcompany.SelectedValue + " and PIM.EMPID=" + DDvendor.SelectedValue + @" and isnull(pim.FOLIOSTATUS,0)=0 ";
                    if (chkforcomplete.Checked == true)
                    {
                        str = str + " and PIM.Status='Complete'";
                    }
                    else
                    {
                        str = str + " and PIM.Status='Pending'";
                    }
                    str = str + " UNION ";
                    str = str + " select Distinct pim.issueorderid,pim.IssueOrderId as IssueOrderId1 From PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + " PIM inner join PROCESS_ISSUE_DETAIL_" + DDprocess.SelectedValue + @" PID on PIM.IssueOrderId=PID.IssueOrderId and pim.Status<>'canceled'
                     inner join OrderMaster om on pid.orderid=om.OrderId and om.OrderCategoryId=2 inner join Employee_processorderno emp on pim.issueorderid=emp.issueorderid and emp.processid=" + DDprocess.SelectedValue + " Where PIm.companyid=" + DDcompany.SelectedValue + " and EMP.EMPID=" + DDvendor.SelectedValue + @" and isnull(pim.FOLIOSTATUS,0)=0 ";
                    if (chkforcomplete.Checked == true)
                    {
                        str = str + " and PIM.Status='Complete'";
                    }
                    else
                    {
                        str = str + " and PIM.Status='Pending'";
                    }
                    str = str + " order by Issueorderid1";
                break;
            default:
                if (variable.VarFinishingNewModuleWise == "1")
                {
                    str = @"select Distinct PIM.IssueOrderId,PIM.IssueOrderId as Issueorderid1 From Process_issue_Master_" + DDprocess.SelectedValue + " PIM inner join Employee_ProcessOrderNo EMP on PIM.IssueOrderId=EMP.IssueOrderId and EMP.ProcessId=" + DDprocess.SelectedValue + " Where PIM.CompanyId=" + DDcompany.SelectedValue + @"
                            and EMP.EMPID=" + DDvendor.SelectedValue + @" and isnull(pim.FOLIOSTATUS,0)=0 ";
                }
                else
                {
                    str = "select Distinct PIM.IssueOrderId,PIM.IssueOrderId as Issueorderid1 From Process_Issue_master_" + DDprocess.SelectedValue + @" PIM Where PIM.Companyid=" + DDcompany.SelectedValue + " and PIM.Empid=" + DDvendor.SelectedValue + @" and isnull(pim.FOLIOSTATUS,0)=0 ";
                }
                if (chkforcomplete.Checked == true)
                {
                    str = str + " and PIM.Status='Complete'";
                }
                else
                {
                    str = str + " and PIM.Status='Pending'";
                }
                str = str + " order by Issueorderid1";
                break;
        }


        UtilityModule.ConditionalComboFill(ref DDissueNo, str, true, "--Plz select--");
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
            SqlParameter[] arr = new SqlParameter[22];
            arr[0] = new SqlParameter("@PrmId", SqlDbType.Int);
            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = hnprmid.Value;
            arr[1] = new SqlParameter("@CompanyId", DDcompany.SelectedValue);
            arr[2] = new SqlParameter("@Empid", DDvendor.SelectedValue);
            arr[3] = new SqlParameter("@Processid", DDprocess.SelectedValue);
            arr[4] = new SqlParameter("@Prorderid", DDissueNo.SelectedValue);
            arr[5] = new SqlParameter("@IssueDate", txtissuedate.Text);
            arr[6] = new SqlParameter("@ChalanNo", SqlDbType.VarChar, 50);
            arr[6].Value = txtchallanNo.Text;
            arr[6].Direction = ParameterDirection.InputOutput;
            arr[7] = new SqlParameter("@TranType", SqlDbType.Int);
            arr[7].Value = 0;
            arr[8] = new SqlParameter("@userid", Session["varuserid"]);
            arr[9] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            arr[10] = new SqlParameter("@Prtid", SqlDbType.Int);
            arr[10].Value = 0;
            arr[11] = new SqlParameter("@CategoryId", ddCatagory.SelectedValue);
            arr[12] = new SqlParameter("@Itemid", dditemname.SelectedValue);
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, Tran, ddlshade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            arr[13] = new SqlParameter("@FinishedId", Varfinishedid);
            arr[14] = new SqlParameter("@GodownId", DDGodown.SelectedValue);
            arr[15] = new SqlParameter("@IssueQuantity", txtissueqty.Text);
            arr[16] = new SqlParameter("@lotNo", DDLotno.SelectedItem.Text);
            arr[17] = new SqlParameter("@UnitId", DDunit.SelectedValue);
            arr[18] = new SqlParameter("@Tagno", TDTagNo.Visible == true ? DDTagNo.SelectedItem.Text : "Without Tag No");
            arr[19] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[19].Direction = ParameterDirection.Output;
            arr[20] = new SqlParameter("@BinNo", TDBinNo.Visible == true ? DDBinNo.SelectedItem.Text : "");
            arr[21] = new SqlParameter("@Remark", (txtremark.Text).Trim());
            //*******************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAMPLEPROCESS_RAW_ISSUE", arr);
            lblmsg.Text = arr[19].Value.ToString();
            hnprmid.Value = arr[0].Value.ToString();
            txtchallanNo.Text = arr[6].Value.ToString();
            Tran.Commit();
            fill_Data_grid();
            //******
            txtissueqty.Text = "";
            ddlshade.Focus();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = "select * From V_SamplematerialIssue where prmid=" + hnprmid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptsamplematerialissue.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptsamplematerialissue.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    protected void DDGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (TDBinNo.Visible == true)
        //{
        //    FillBinNo();
        //    if (DDBinNo.Items.Count > 0)
        //    {
        //        DDBinNo.SelectedIndex = 1;
        //        DDBinNo_SelectedIndexChanged(sender, new EventArgs());
        //    }
        //}
        //else
        //{
        //    FillLotno();

        //}
        FillLotno(sender);

    }
    protected void FillBinNo(object sender = null)
    {
        DDBinNo.SelectedIndex = -1;
        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        string str = "select Distinct BInNo,BInNo as BInNo1 From stock Where Companyid=" + DDcompany.SelectedValue + " and Godownid=" + DDGodown.SelectedValue + " and ITEM_FINISHED_ID=" + Varfinishedid + " and LotNo='" + DDLotno.SelectedItem.Text + "'";
        if (TDTagNo.Visible == true)
        {
            str = str + " and TagNo='" + DDTagNo.SelectedItem.Text + "'";
        }
        else
        {
            str = str + " and TagNo='Without Tag No'";
        }

        if (MySession.Stockapply == "True")
        {
            str = str + " and Round(Qtyinhand,3)>0 ";
        }

        str = str + "  order by BInNo1";
        UtilityModule.ConditionalComboFill(ref DDBinNo, str, true, "--Plz Select--");
        if (DDBinNo.Items.Count > 0)
        {
            DDBinNo.SelectedIndex = 1;
            if (sender != null)
            {
                DDBinNo_SelectedIndexChanged(sender, new EventArgs());
            }
        }
    }
    protected void FillLotno(object sender = null)
    {
        DDLotno.SelectedIndex = -1;
        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        string str = "select Distinct Lotno,Lotno as Lotno1 From stock Where Companyid=" + DDcompany.SelectedValue + " and Godownid=" + DDGodown.SelectedValue + " and ITEM_FINISHED_ID=" + Varfinishedid;
        if (MySession.Stockapply == "True")
        {
            str = str + " and Round(Qtyinhand,3)>0 ";
        }
        //if (TDBinNo.Visible == true)
        //{
        //    str = str + " and BinNo='" + DDBinNo.SelectedItem.Text + "' ";
        //}
        str = str + "  order by Lotno1";
        UtilityModule.ConditionalComboFill(ref DDLotno, str, true, "--Plz Select--");
        if (DDLotno.Items.Count > 0)
        {
            DDLotno.SelectedIndex = 1;
            DDLotno_SelectedIndexChanged(sender, new EventArgs());
        }
    }
    protected void FillTagNo(object sender = null)
    {
        DDTagNo.SelectedIndex = -1;
        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        string str = "select Distinct TagNo,TagNo as TagNo1 From stock Where Companyid=" + DDcompany.SelectedValue + " and Godownid=" + DDGodown.SelectedValue + " and ITEM_FINISHED_ID=" + Varfinishedid + " and Lotno='" + DDLotno.SelectedItem.Text + "'";
        if (MySession.Stockapply == "True")
        {
            str = str + " and Round(Qtyinhand,3)>0 ";
        }
        //if (TDBinNo.Visible == true)
        //{
        //    str = str + " and BinNo='" + DDBinNo.SelectedItem.Text + "' ";
        //}
        str = str + "  order by TagNo1";
        UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "--Plz Select--");
        if (DDTagNo.Items.Count > 0)
        {
            DDTagNo.SelectedIndex = 1;
            if (sender != null)
            {
                DDTagNo_SelectedIndexChanged(sender, new EventArgs());
            }

        }
    }
    protected void DDLotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TDTagNo.Visible == true)
        {
            FillTagNo(sender);
        }
        else
        {
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            txtstockqty.Text = Convert.ToString(UtilityModule.getstockQty(DDcompany.SelectedValue, DDGodown.SelectedValue, DDLotno.SelectedItem.Text, Varfinishedid, BinNo: (TDBinNo.Visible == true ? DDBinNo.SelectedItem.Text : "")));
        }

    }
    protected void DDTagNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TDBinNo.Visible == true)
        {
            FillBinNo(sender);
        }
        else
        {
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            txtstockqty.Text = Convert.ToString(UtilityModule.getstockQty(DDcompany.SelectedValue, DDGodown.SelectedValue, DDLotno.SelectedItem.Text, Varfinishedid, DDTagNo.SelectedItem.Text, BinNo: (TDBinNo.Visible == true ? DDBinNo.SelectedItem.Text : "")));
        }
    }
    protected void fill_Data_grid()
    {
        try
        {
            string strsql = @"Select PrtId,ITEM_NAME,QualityName+ Space(2)+DesignName+ Space(2)+ColorName+ Space(2)+ShapeName+ Space(2)+SizeFt+ Space(2)+ShadeColorName DESCRIPTION,
                             IssueQuantity Qty,LotNo,GodownName,PT.TagNo,PM.ChalanNo,Replace(convert(nvarchar(11),PM.Date,106),' ','-') as IssueDate,IsNull(PM.Remark,'') as Remark 
                             From Processrawmaster PM,ProcessRawTran PT,V_FinishedItemDetail VF,GodownMaster GM 
                             Where PM.TypeFlag = 0 And PM.Prmid=Pt.prmid and  PT.Finishedid=VF.Item_Finished_id And 
                             PT.GodownId=GM.GodownId And PT.PrmID=" + hnprmid.Value + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            if (chkedit.Checked == true)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtchallanNo.Text = ds.Tables[0].Rows[0]["chalanno"].ToString();
                    txtissuedate.Text = ds.Tables[0].Rows[0]["issuedate"].ToString();
                    txtremark.Text = ds.Tables[0].Rows[0]["Remark"].ToString();
                }

            }
            gvdetail.DataSource = ds.Tables[0];
            gvdetail.DataBind();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }

    protected void gvdetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvdetail.EditIndex = e.NewEditIndex;
        fill_Data_grid();
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        DDvendor.SelectedIndex = -1;
        DDissueNo.SelectedIndex = -1;
        DDChallanNo.SelectedIndex = -1;
        TDChallanNo.Visible = false;
        hnprmid.Value = "0";
        txtchallanNo.Text = "";
        txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        gvdetail.DataSource = null;
        gvdetail.DataBind();

        if (chkedit.Checked == true)
        {
            TDChallanNo.Visible = true;
        }
    }
    protected void DDissueNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDChallanNo, @"select PRMid,ChalanNo 
                From ProcessRawMaster 
                Where TypeFlag = 0 And Companyid=" + DDcompany.SelectedValue + " and Processid=" + DDprocess.SelectedValue + " and Empid=" + DDvendor.SelectedValue + " and Prorderid=" + DDissueNo.SelectedValue + @" and trantype=0
                                                           and BeamType=0 order by PRMid", true, "--Plz Select--");

    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnprmid.Value = DDChallanNo.SelectedValue;
        fill_Data_grid();
    }
    protected void gvdetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvdetail.EditIndex = -1;
        fill_Data_grid();
    }
    protected void gvdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int prtid = Convert.ToInt32(gvdetail.DataKeys[e.RowIndex].Value);
            TextBox txtqtyedit = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtqtyedit");
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@prtid", prtid);
            param[1] = new SqlParameter("@qty", txtqtyedit.Text == "" ? "0" : txtqtyedit.Text);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            //*****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_updatesamplerowissue", param);
            lblmsg.Text = param[2].Value.ToString();
            Tran.Commit();
            gvdetail.EditIndex = -1;
            fill_Data_grid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int Prtid = Convert.ToInt32(gvdetail.DataKeys[e.RowIndex].Value);
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@prtid", Prtid);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@userid", Session["varuserid"]);

            //**************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRo_Deletesamplerawissue", param);
            lblmsg.Text = param[1].Value.ToString();
            Tran.Commit();
            fill_Data_grid();

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void chkforcomplete_CheckedChanged(object sender, EventArgs e)
    {
        FillIssueNo();
    }
    protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    {
        FillProcess_Employee(sender);
    }
    protected void FillProcess_Employee(object sender = null)
    {
        string str = @"SELECT Top(1) EMP.ProcessId,EI.EmpId FROM EMPLOYEE_PROCESSORDERNO EMP INNER JOIN EMPINFO EI ON EMP.EMPID=EI.EMPID WHERE EI.EMPCODE='" + txtWeaverIdNoscan.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (DDprocess.Items.FindByValue(ds.Tables[0].Rows[0]["Processid"].ToString()) != null)
            {
                DDprocess.SelectedValue = ds.Tables[0].Rows[0]["Processid"].ToString();
                if (sender != null)
                {
                    DDprocess_SelectedIndexChanged(sender, new EventArgs());
                }

            }
            if (DDvendor.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) != null)
            {
                DDvendor.SelectedValue = ds.Tables[0].Rows[0]["Empid"].ToString();
                if (sender != null)
                {
                    DDvendor_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            DDissueNo.Focus();
        }
        else
        {
            DDprocess.SelectedIndex = -1;
            DDvendor.SelectedIndex = -1;
            ScriptManager.RegisterStartupScript(Page, GetType(), "fillemp", "alert('Please Enter correct Emp. Code or No entry from this employee')", true);

        }
    }


    protected void DDBinNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        txtstockqty.Text = Convert.ToString(UtilityModule.getstockQty(DDcompany.SelectedValue, DDGodown.SelectedValue, DDLotno.SelectedItem.Text, Varfinishedid, TagNo: (TDTagNo.Visible == true ? DDTagNo.SelectedItem.Text : "Without Tag No"), BinNo: (TDBinNo.Visible == true ? DDBinNo.SelectedItem.Text : "")));
    }
}