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
public partial class Masters_Carpet_FrmFinisherJobRate : System.Web.UI.Page
{
    static int MasterCompanyId;
    static bool VarBool;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varCompanyId"]);
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (IsPostBack != true)
        {
            string str = "select PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER(Nolock) WHERE MasterCompanyId=" + Session["varCompanyId"] + " and ProcessType=1 and Process_Name_id<>1 order by PROCESS_NAME";

            if (Session["varcompanyId"].ToString() == "9")
            {
                str = "select pnm.PROCESS_NAME_ID,pnm.PROCESS_NAME from PROCESS_NAME_MASTER(Nolock) PNM left join V_PRODUCTIONPROCESSIDFORHAFIZIA VP(Nolock) on PNM.Process_name_id=vp.process_name_id WHERE MasterCompanyId=" + Session["varCompanyId"] + "  and vp.process_name_id is  null order by PROCESS_NAME";
            }
            str = str + " select CustomerId,CustomerCode from customerinfo(Nolock) order by CustomerCode";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDJobType, ds, 0, true, "--Select--");

            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds, 1, true, "--Select--");

            TDRate2.Visible = false;
            txtEffectiveDate.Attributes.Add("readonly", "readonly");
            BindQualityType();
            UtilityModule.ConditionalComboFill(ref DDShape, "select Shapeid,ShapeName From Shape ", true, "--Plz Select--");
            if (Session["varcompanyId"].ToString() == "9")
            {
                UtilityModule.ConditionalComboFill(ref DDUnit, "select UnitId,unitName from Unit where UnitId in(1,2,6) order by UnitId ", true, "--Plz Select--");
                ChkForInchSize.Visible = true;
            }
            else if (Session["varcompanyId"].ToString() == "44")
            {
                UtilityModule.ConditionalComboFill(ref DDUnit, "select UnitId,unitName from Unit where UnitId in(1,2) order by UnitId ", true, "--Plz Select--");
                ChkForInchSize.Visible = true;
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDUnit, "select UnitId,unitName from Unit where UnitId in(1,2) order by UnitId ", true, "--Plz Select--");
                ChkForInchSize.Visible = false;
            }
            if (MasterCompanyId == 20)
            {
                TDCustomercode.Visible = true;
                tdShape.Visible = false;
            }
            if (MasterCompanyId == 16 || MasterCompanyId == 28 || MasterCompanyId == 42 || MasterCompanyId == 38 || MasterCompanyId == 46)
            {
                TDEmployeeName.Visible = true;
            }         

            txtEffectiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            txtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            if (variable.VarFINISHINGRATE_LOCATIONWISE == "1")
            {
                TDratelocation.Visible = true;
            }
            if (variable.VarAddFinisherJobRateByFtMtr == "1")
            {
                TDRateFtMtr.Visible = true;
            }

            if (MasterCompanyId == 42)
            {
                TDBonusRate.Visible = true;
            }
        }
    }
    private void BindQualityType()
    {
        if (DDCustomerCode.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref DDQualityType, @"select distinct IM.ITEM_ID,IM.ITEM_NAME from ITEM_MASTER IM INNER JOIN OrderDetail OD ON OD.ITEM_ID=IM.ITEM_ID
                                            INNER JOIN OrderMaster OM ON OM.OrderId=OD.OrderId WHERE OM.CustomerId=" + DDCustomerCode.SelectedValue + " ", true, "--Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDQualityType, @"select IM.ITEM_ID,ITEM_NAME From Item_master IM inner Join CategorySeparate cs on IM.CATEGORY_ID=Cs.Categoryid and Cs.id=0 order by Item_Name ", true, "--Select--");
        }

    }
    private void BindQuality()
    {
        string view = "V_FinishedItemDetail";
        if (variable.VarNewQualitySize == "1")
        {
            view = "V_FinishedItemDetailNew";
        }
        string str = "";
        if (DDCustomerCode.SelectedIndex > 0)
        {
            str = @"select Distinct vf.QualityId,vf.QualityName From " + view + @" vf inner join OrderDetail od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id
                      inner join OrderMaster om on od.orderid=om.OrderId Where Om.customerid=" + DDCustomerCode.SelectedValue + " and  vf.QualityId<>0 and vf.ITEM_ID=" + DDQualityType.SelectedValue + "  order by QualityName";
        }
        else
        {
            str = "Select Qualityid,QualityName From Quality Where item_id=" + DDQualityType.SelectedValue + " Order By QualityName";
        }
        UtilityModule.ConditionalComboFill(ref DDQuality, str, true, "--Select--");

        //        if (DDCustomerCode.SelectedIndex > 0 && DDQualityType.SelectedIndex > 0)
        //        {
        //            UtilityModule.ConditionalComboFill(ref DDQuality, @"select distinct CQ.QualityId,Q.QualityName as Quality from CustomerQuality CQ INNER JOIN OrderMaster OM ON OM.CustomerId=CQ.CustomerId
        //                                                INNER JOIN OrderDetail OD ON OM.OrderId=OD.OrderId  INNER JOIN Quality Q ON Q.QualityId=CQ.QualityId
        //                                                INNER JOIN OrderDetail OD2 ON OD.CQID=CQ.SrNo WHERE Q.Item_Id=" + DDQualityType.SelectedValue + @" 
        //                                                and CQ.CustomerId=" + DDCustomerCode.SelectedValue + " ", true, "--Select--");
        //        }
        //        else if (DDQualityType.SelectedIndex > 0)
        //        {
        //            UtilityModule.ConditionalComboFill(ref DDQuality, @"Select Qualityid,QualityName From Quality Where item_id=" + DDQualityType.SelectedValue + " Order By QualityName", true, "--Select--");
        //        }
    }
    private void BindDesign()
    {
        string view = "V_FinishedItemDetail";
        if (variable.VarNewQualitySize == "1")
        {
            view = "V_FinishedItemDetailNew";
        }
        string str = "";
        if (DDCustomerCode.SelectedIndex > 0)
        {
            str = @"select Distinct vf.designId,vf.designName From " + view + @" vf inner join OrderDetail od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id
                      inner join OrderMaster om on od.orderid=om.OrderId Where OM.customerid=" + DDCustomerCode.SelectedValue + " and  vf.ITEM_ID=" + DDQualityType.SelectedValue;
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and vf.QualityId=" + DDQuality.SelectedValue + " ";
            }
            str = str + " order by Designname";
        }
        else
        {
            str = @"select Distinct designId,designName From V_FinishedItemDetail Where ITEM_ID=" + DDQualityType.SelectedValue + " and designId<>0";
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and Qualityid=" + DDQuality.SelectedValue;
            }
            str = str + " order by Designname";
        }

        UtilityModule.ConditionalComboFill(ref DDDesign, str, true, "--Select--");


        //        if (DDCustomerCode.SelectedIndex > 0 && DDQualityType.SelectedIndex > 0 && DDQuality.SelectedIndex > 0)
        //        {
        //            UtilityModule.ConditionalComboFill(ref DDDesign, @"select distinct D.designId,D.designName as Design from CustomerDesign CD INNER JOIN OrderMaster OM ON OM.CustomerId=CD.CustomerId
        //                                                INNER JOIN OrderDetail OD ON OM.OrderId=OD.OrderId INNER JOIN Design D ON D.designId=CD.DesignId
        //                                                INNER JOIN OrderDetail OD2 ON OD2.DSRNO=CD.SrNo INNER JOIN ITEM_PARAMETER_MASTER IPM ON IPM.DESIGN_ID=CD.DesignId
        //                                                WHERE IPM.QUALITY_ID=" + DDQuality.SelectedValue + @" 
        //                                                and CD.CustomerId=" + DDCustomerCode.SelectedValue + " ", true, "--Select--");
        //        }
        //        else if (DDQuality.SelectedIndex > 0)
        //        {
        //            UtilityModule.ConditionalComboFill(ref DDDesign, @"select distinct D.designId,D.designName from Design D INNER JOIN ITEM_PARAMETER_MASTER IPM ON IPM.DESIGN_ID=D.designId 
        //                                                Where IPM.QUALITY_ID=" + DDQuality.SelectedValue + " Order By designName", true, "--Select--");
        //        }
    }
    private void BindColor()
    {

        string view = "V_FinishedItemDetail";
        if (variable.VarNewQualitySize == "1")
        {
            view = "V_FinishedItemDetailNew";
        }
        string str = "";
        if (DDCustomerCode.SelectedIndex > 0)
        {
            str = @"select Distinct vf.ColorId,vf.ColorName From " + view + @" vf inner join OrderDetail od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id
                      inner join OrderMaster om on od.orderid=om.OrderId Where om.customerid=" + DDCustomerCode.SelectedValue + " and  vf.ITEM_ID=" + DDQualityType.SelectedValue;
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
            }
            str = str + " order by colorname";
        }
        else
        {
            str = @"select Distinct colorid,colorname From V_FinishedItemDetail Where ITEM_ID=" + DDQualityType.SelectedValue + " and colorid<>0";
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and Qualityid=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and designid=" + DDDesign.SelectedValue;
            }
            str = str + " order by colorname";
        }

        UtilityModule.ConditionalComboFill(ref DDColor, str, true, "--Select--");


        //        if (DDCustomerCode.SelectedIndex > 0 && DDDesign.SelectedIndex > 0)
        //        {
        //            UtilityModule.ConditionalComboFill(ref DDColor, @"select Distinct C.ColorId,C.ColorName from CustomerColor CC INNER JOIN color C ON CC.ColorId=C.ColorId
        //                                                INNER JOIN OrderMaster OM ON OM.CustomerId=CC.CustomerId INNER JOIN OrderDetail OD ON OM.OrderId=OD.OrderId
        //                                                INNER JOIN OrderDetail OD2 ON OD2.CSRNO=CC.SrNo INNER JOIN ITEM_PARAMETER_MASTER IPM ON IPM.COLOR_ID=CC.ColorId
        //                                                WHERE IPM.DESIGN_ID=" + DDDesign.SelectedValue + @" 
        //                                                and CC.CustomerId=" + DDCustomerCode.SelectedValue + " ", true, "--Select--");
        //        }
        //        else if (DDDesign.SelectedIndex > 0)
        //        {
        //            UtilityModule.ConditionalComboFill(ref DDColor, @"select distinct C.ColorId,C.ColorName from color C INNER JOIN ITEM_PARAMETER_MASTER IPM ON IPM.COLOR_ID=C.ColorId 
        //                                                Where IPM.Item_id=" + DDQualityType.SelectedValue + " and IPM.Quality_Id=" + DDQuality.SelectedValue + " and  IPM.DESIGN_ID=" + DDDesign.SelectedValue + " Order by ColorName", true, "--Select--");
        //        }
    }
    private void BindSize()
    {
        string str = "";
        string ColumnName = "";
        string ColumnName2 = "";
        if (chkForFtSize.Checked == true)
        {
            if (variable.VarNewQualitySize == "1")
            {

                ColumnName = "Finishing_Ft_Size";
            }
            else
            {
                if (Session["varCompanyNo"].ToString() == "43")
                {
                    ColumnName = "SizeFt";
                    ColumnName2 = "ItemFinishingSizeFT";

                }
                else
                {
                    ColumnName = "SizeFt";
                }
            }
        }
        else if (ChkForInchSize.Checked == true)
        {
            if (variable.VarNewQualitySize == "1")
            {

                ColumnName = "SizeInch";
            }
            else
            {

                ColumnName = "SizeInch";
            }
        }
        else
        {

            if (variable.VarNewQualitySize == "1")
            {

                ColumnName = "Finishing_Mt_Size";
            }
            else
            {
                if (Session["varCompanyNo"].ToString() == "43")
                {
                    ColumnName = "SizeMtr";
                    ColumnName2 = "ItemFinishingSizeMtr";
                }
                else
                {
                    ColumnName = "SizeMtr";
                }
            }

        }

        if (variable.VarNewQualitySize == "1")
        {

            if (DDCustomerCode.SelectedIndex > 0)
            {
                str = @"select Distinct CS.Sizeid,QSN." + ColumnName + @"+'['+cast(QSN.SizeId as nvarchar)+']'+SPACE(5)+S.ShapeName as Size from QualitySizeNew QSN INNER JOIN CustomerSize CS  ON QSN.SizeId=CS.Sizeid
                    INNER JOIN OrderMaster OM ON OM.CustomerId=CS.CustomerId  INNER JOIN OrderDetail OD ON OM.OrderId=OD.OrderId
                    INNER JOIN V_FinishedItemDetailNew VF ON VF.ITEM_FINISHED_ID=OD.Item_Finished_Id  INNER JOIN V_FinishedItemDetailNew VF2 ON VF2.SizeId=CS.Sizeid
                    INNER JOIN Shape S ON QSN.ShapeId=S.ShapeId where CS.CustomerId=" + DDCustomerCode.SelectedValue + " and SizeNameAToC <>'' ";

                if (DDQuality.SelectedIndex > 0)
                {
                    str = str + " and QSN.QualityId=" + DDQuality.SelectedValue;
                }
                if (DDShape.SelectedIndex > 0)
                {
                    str = str + " and S.ShapeId=" + DDShape.SelectedValue;
                }
                str = str + " order by Size";

            }
            else
            {
                str = @"select QSN.SizeId,QSN." + ColumnName + @"+'['+Cast(QSN.Sizeid As Nvarchar)+']'+ Space(5) + ShapeName as Size  
                    from QualitySizeNew QSN INNER JOIN Shape S ON QSN.ShapeId=S.ShapeId where TypeS=1 ";

                str = str + " order by " + ColumnName + @"";
            }
        }
        else
        {
            if (DDCustomerCode.SelectedIndex > 0)
            {
                if (Session["VarCompanyNo"].ToString() == "43")
                {
                    str = @"select Distinct vf.sizeid,vf." + ColumnName + "+ '- ['" + "+SA." + ColumnName2 + "+']'" + @" as size From V_FinishedItemDetail vf(NoLock) 
                            inner join OrderDetail od(NoLock) on vf.ITEM_FINISHED_ID=od.Item_Finished_Id
                            inner join OrderMaster om(NoLock) on od.orderid=om.OrderId 
                            Join SizeAttachedWithItem SA(NoLock) ON VF.sizeId=SA.SizeId and VF.Item_Id=SA.ItemId and VF.QualityId=SA.QualityId
                            Where om.customerid=" + DDCustomerCode.SelectedValue + " and  vf.ITEM_ID=" + DDQualityType.SelectedValue + @" 
                            and vf.shapeid=" + DDShape.SelectedValue;
                }
                else
                {
                    str = @"select Distinct vf.sizeid,vf." + ColumnName + @" as size From V_FinishedItemDetail vf inner join OrderDetail od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id
                      inner join OrderMaster om on od.orderid=om.OrderId Where om.customerid=" + DDCustomerCode.SelectedValue + " and  vf.ITEM_ID=" + DDQualityType.SelectedValue + " and vf.shapeid=" + DDShape.SelectedValue;
                }
                if (DDQuality.SelectedIndex > 0)
                {
                    str = str + " and Vf.QualityId=" + DDQuality.SelectedValue;
                }
                if (DDDesign.SelectedIndex > 0)
                {
                    str = str + " and Vf.DesignId=" + DDDesign.SelectedValue;
                }
                if (DDColor.SelectedIndex > 0)
                {
                    str = str + " and Vf.Colorid=" + DDColor.SelectedValue;
                }
                str = str + " order by Size";
            }
            else
            {
                if (Session["VarCompanyNo"].ToString() == "43")
                {
                    str = @"select Distinct Vf.SizeId,Vf." + ColumnName+ "+ '- ['"+  "+SA." + ColumnName2 + "+']'" + @" as Size From V_FinishedItemDetail Vf(NoLock)  
                    JOIN SizeAttachedWithItem SA(NoLock) ON VF.sizeId=SA.SizeId and VF.Item_Id=SA.ItemId and VF.QualityId=SA.QualityId
                    Where Shapeid=" + DDShape.SelectedValue;
                }
                else
                {
                    str = @"select Distinct Vf.SizeId,Vf." + ColumnName + " as Size From V_FinishedItemDetail Vf  Where Shapeid=" + DDShape.SelectedValue;
                }
                
                if (DDQualityType.SelectedIndex > 0)
                {
                    str = str + " and Vf.Item_id=" + DDQualityType.SelectedValue;
                }
                if (DDQuality.SelectedIndex > 0)
                {
                    str = str + " and Vf.QualityId=" + DDQuality.SelectedValue;
                }
                if (DDDesign.SelectedIndex > 0)
                {
                    str = str + " and Vf.DesignId=" + DDDesign.SelectedValue;
                }
                if (DDColor.SelectedIndex > 0)
                {
                    str = str + " and Vf.Colorid=" + DDColor.SelectedValue;
                }
                str = str + " order by Size";
            }

        }

        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--Select--");

    }
    private void BindCalc()
    {
        string str = "";
        if (Session["varcompanyid"].ToString() == "9")
        {
            str = "Select CalcId,CalcName From CalcOptions Order by CalcId";
            UtilityModule.ConditionalComboFill(ref DDCalcOption, str, false, "--Select--");
        }
        else
        {
            str = "Select CalcId,CalcName From CalcOptions Order by CalcId";
            UtilityModule.ConditionalComboFill(ref DDCalcOption, str, true, "--Select--");
        }
    }
    protected void DDJobType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDJobType.SelectedIndex > 0)
        {
            if (MasterCompanyId != 27)
            {
                DDCustomerCode.SelectedIndex = -1;
                DDShape.SelectedIndex = -1;
                DDCalcOption.SelectedIndex = -1;

                txtRate.Text = "";
                txtRate2.Text = "";
                txtReIssRate.Text = "";
                //txtEffectiveDate.Text = "";                
            }
            FillEmployeeName();
            BindGrid(0);
            BindCalc();
        }
    }
    private void FillEmployeeName()
    {
        UtilityModule.ConditionalComboFill(ref DDEmployeeName, @"Select Distinct EI.EmpID, EI.EmpName + '[' + EI.EmpCode + ']' EmpName
                From Empinfo EI(Nolock) 
                JOIN EmpProcess EP(Nolock) ON EP.EmpId = EI.EmpId And EP.ProcessId = " + DDJobType.SelectedValue + @" 
                Where EI.Blacklist = 0 
                Order By EmpName ", true, "--Plz Select--");
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {

        DDShape.SelectedIndex = -1;
        DDCalcOption.SelectedIndex = -1;
        DDQualityType.Items.Clear();
        DDQuality.Items.Clear();
        DDDesign.Items.Clear();
        DDColor.Items.Clear();
        DDSize.Items.Clear();

        //UtilityModule.ConditionalComboFill(ref DDQualityType, " ", true, "--Select--");
        //UtilityModule.ConditionalComboFill(ref DDQuality, " ", true, "--Select--");
        //UtilityModule.ConditionalComboFill(ref DDDesign, " ", true, "--Select--");
        //UtilityModule.ConditionalComboFill(ref DDColor, " ", true, "--Select--");
        //UtilityModule.ConditionalComboFill(ref DDSize, " ", true, "--Select--");

        txtRate.Text = "";
        txtRate2.Text = "";
        txtReIssRate.Text = "";
        txtBonusRate.Text = "";
        //txtEffectiveDate.Text = "";

        BindQualityType();
        BindGrid(0);
    }
    protected void DDQualityType_SelectedIndexChanged(object sender, EventArgs e)
    {

        DDShape.SelectedIndex = -1;
        DDCalcOption.SelectedIndex = -1;

        DDQuality.Items.Clear();
        DDDesign.Items.Clear();
        DDColor.Items.Clear();
        DDSize.Items.Clear();
        //UtilityModule.ConditionalComboFill(ref DDQuality, " ", true, "--Select--");
        //UtilityModule.ConditionalComboFill(ref DDDesign, " ", true, "--Select--");
        //UtilityModule.ConditionalComboFill(ref DDColor, " ", true, "--Select--");
        //UtilityModule.ConditionalComboFill(ref DDSize, " ", true, "--Select--");

        txtRate.Text = "";
        txtRate2.Text = "";
        txtReIssRate.Text = "";
        txtBonusRate.Text = "";
        //txtEffectiveDate.Text = "";

        BindQuality();
        BindGrid(0);
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDDesign.Items.Clear();
        DDColor.Items.Clear();
        DDSize.Items.Clear();
        //UtilityModule.ConditionalComboFill(ref DDDesign, " ", true, "--Select--");
        //UtilityModule.ConditionalComboFill(ref DDColor, " ", true, "--Select--");
        //UtilityModule.ConditionalComboFill(ref DDSize, " ", true, "--Select--");

        BindDesign();
        if (DDQuality.SelectedIndex > 0)
        {
            BindSize();
        }

        if (ViewState["RateId"] != null)
        {
            BindGrid(1);
        }
        else
        {
            BindGrid(0);
        }
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDColor, " ", true, "--Select--");

        BindColor();
        if (ViewState["RateId"] != null)
        {
            BindGrid(1);
        }
        else
        {
            BindGrid(0);
        }
    }
    protected void chkForFtSize_CheckedChanged(object sender, EventArgs e)
    {
        if (Session["varCompanyId"].ToString() == "9")
        {
            if (chkForFtSize.Checked == true)
            {
                ChkForInchSize.Checked = false;
            }
        }
        BindSize();
    }
    protected void ChkForInchSize_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForInchSize.Checked == true)
        {
            chkForFtSize.Checked = false;
        }
        BindSize();
    }
    protected void DDCalcOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDCalcOption.SelectedIndex > 0)
        {
            if (DDCalcOption.SelectedValue == "9")
            {
                TDRate2.Visible = true;
            }
            else
            {
                TDRate2.Visible = false;
            }
        }
    }
    private void CHECKVALIDCONTROL()
    {
        llMessageBox.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDJobType) == false)
        {
            goto a;
        }
        if (TDCustomercode.Visible == true)
        {
            if (Session["varcompanyId"].ToString() == "20")
            {

                if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerCode) == false)
                {
                    goto a;
                }

            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDQualityType) == false)
        {
            goto a;
        }
        if ((Session["varcompanyId"].ToString() == "16" || Session["varcompanyId"].ToString() == "28") && (DDJobType.SelectedItem.Text != "PACKING" && DDJobType.SelectedItem.Text != "RE-PACKING"))
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDQuality) == false)
            {
                goto a;
            }
        }
        if (Session["varcompanyId"].ToString() != "20")
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDShape) == false)
            {
                goto a;
            }
        }
        if (Session["varcompanyid"].ToString() != "9")
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDCalcOption) == false)
            {
                goto a;
            }
        }

        if (TDratelocation.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDratelocation) == false)
            {
                goto a;
            }
        }
        if (TDRateFtMtr.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDUnit) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDTEXTBOX(txtRate) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtEffectiveDate) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        llMessageBox.Visible = true;
        UtilityModule.SHOWMSG(llMessageBox);
    B: ;
    }
    private void SubmitData()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[20];
            if (ViewState["RateId"] == null)
            {
                ViewState["RateId"] = 0;
            }
            _arrpara[0] = new SqlParameter("@RateId", SqlDbType.Int);
            _arrpara[0].Direction = ParameterDirection.InputOutput;
            _arrpara[0].Value = ViewState["RateId"];
            _arrpara[1] = new SqlParameter("@JobTypeId", DDJobType.SelectedValue);
            _arrpara[2] = new SqlParameter("@CustomerId", DDCustomerCode.SelectedIndex > 0 ? DDCustomerCode.SelectedValue : "0");
            _arrpara[3] = new SqlParameter("@Item_Id", DDQualityType.SelectedValue);
            _arrpara[4] = new SqlParameter("@QualityId", DDQuality.SelectedIndex > 0 ? DDQuality.SelectedValue : "0");
            _arrpara[5] = new SqlParameter("@DesignId", DDDesign.SelectedIndex > 0 ? DDDesign.SelectedValue : "0");
            _arrpara[6] = new SqlParameter("@ColorId", DDColor.SelectedIndex > 0 ? DDColor.SelectedValue : "0");
            _arrpara[7] = new SqlParameter("@SizeId", DDSize.SelectedIndex > 0 ? DDSize.SelectedValue : "0");
            _arrpara[8] = new SqlParameter("@CalcOptionId", DDCalcOption.SelectedValue);
            _arrpara[9] = new SqlParameter("@Rate", txtRate.Text == "" ? "0" : txtRate.Text);
            _arrpara[10] = new SqlParameter("@Rate2", txtRate2.Text == "" ? "0" : txtRate2.Text);
            _arrpara[11] = new SqlParameter("@ReIssRate", txtReIssRate.Text == "" ? "0" : txtReIssRate.Text);
            _arrpara[12] = new SqlParameter("@EffectiveDate", txtEffectiveDate.Text);
            _arrpara[13] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);
            _arrpara[13].Direction = ParameterDirection.Output;
            _arrpara[14] = new SqlParameter("@Shapeid", DDShape.SelectedValue);
            _arrpara[15] = new SqlParameter("@RateLocation", TDratelocation.Visible == false ? "0" : DDratelocation.SelectedValue);
            _arrpara[16] = new SqlParameter("@UnitId", TDRateFtMtr.Visible == false ? "0" : DDUnit.SelectedValue);
            _arrpara[17] = new SqlParameter("@Remark", TxtRemark.Text);
            _arrpara[18] = new SqlParameter("@EmpID", TDEmployeeName.Visible == true ? DDEmployeeName.SelectedValue : "0");
            _arrpara[19] = new SqlParameter("@Bonus", TDBonusRate.Visible == true ? txtBonusRate.Text=="" ? "0" : txtBonusRate.Text : "0");

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_SaveFinisherJobRate]", _arrpara);

            llMessageBox.Visible = true;
            llMessageBox.Text = _arrpara[13].Value.ToString();
            //llMessageBox.Text = "Data Successfully Saved.......";

            ViewState["RateId"] = 0;
            Tran.Commit();
            ClearAfterSave();
            BindGrid(0);
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            ViewState["RateId"] = 0;
            llMessageBox.Visible = true;
            llMessageBox.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (llMessageBox.Text == "")
        {
            SubmitData();
        }
    }
    private void ClearAfterSave()
    {
        //DDJobType.SelectedIndex = -1;
        //DDCustomerCode.SelectedIndex = -1;
        //DDQualityType.SelectedIndex = -1;
        //DDQuality.SelectedIndex = -1;
        //DDDesign.SelectedIndex = -1;
        //DDColor.SelectedIndex = -1;
        //DDSize.SelectedIndex = -1;
        //DDCalcOption.SelectedIndex = -1;
        //txtRate.Text = "";
        //txtRate2.Text = "";
        //txtReIssRate.Text = "";
        //txtEffectiveDate.Text = "";
        VarBool = false;
        TxtRemark.Text = "";
    }
    protected void BindGrid(int Flag)
    {
        string where = "";
        string sizeColumn = "SizeFt";
        if (variable.VarNewQualitySize == "1")
        {
            sizeColumn = "Finishing_Mt_Size";
        }        
        if (DDJobType.SelectedIndex > 0)
        {
            where = where + " and RD.JobTypeId=" + DDJobType.SelectedValue;
        }
        if (DDCustomerCode.SelectedIndex > 0)
        {
            where = where + " and RD.CustomerId=" + DDCustomerCode.SelectedValue;
        }
        if (DDEmployeeName.SelectedIndex > 0)
        {
            where = where + " and RD.EmpId = " + DDEmployeeName.SelectedValue;
        }
        if (DDQualityType.SelectedIndex > 0)
        {
            where = where + " and RD.Item_Id=" + DDQualityType.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and RD.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and RD.Designid=" + DDDesign.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and RD.SizeId=" + DDSize.SelectedValue;
        }
        if (Flag == 0)
        {
            where = where + " and ToDate Is Null";
        }
        else if (Flag == 1)
        {
            if (DDJobType.SelectedIndex > 0 && DDQualityType.SelectedIndex < 0)
            {
                where = where + "And Effectivedate in (Select max(Effectivedate) From RateDesc Where Effectivedate <='" + txtEffectiveDate.Text + "' And JobTypeId=" + DDJobType.SelectedValue + @" 
                        Group by IM.ITEM_NAME,QualityName,DesignName,ColorName," + sizeColumn + ",ShapeName,PNM.PROCESS_NAME,CO.CalcName,Rate,RD.QualityId,RD.JobTypeId,CO.CalcId,RateId,IM.ITEM_ID,RD.DesignId,RD.ColorId,RD.SizeId,Rate2,EffectiveDate,RD.Shapeid,S.Shapename , RD.Ratelocation,RD.Unitid,U.UnitName, RD.Remark, EI.EmpCode, EI.EmpName,RD.Bonus )";
            }
            else if (DDJobType.SelectedIndex > 0 && DDQualityType.SelectedIndex > 0 && DDQuality.SelectedIndex < 0 && DDSize.SelectedIndex < 0)
            {
                where = where + "And Effectivedate in (Select max(Effectivedate) From RateDesc Where Effectivedate <='" + txtEffectiveDate.Text + "' And JobTypeId=" + DDJobType.SelectedValue + " and RD.Item_Id=" + DDQualityType.SelectedValue + @"
                        Group by IM.ITEM_NAME,QualityName,DesignName,ColorName," + sizeColumn + ",ShapeName,PNM.PROCESS_NAME,CO.CalcName,Rate,RD.QualityId,RD.JobTypeId,CO.CalcId,RateId,IM.ITEM_ID,RD.DesignId,RD.ColorId,RD.SizeId,Rate2,EffectiveDate,RD.Shapeid,S.Shapename, RD.Ratelocation,RD.Unitid,U.UnitName, RD.Remark, EI.EmpCode, EI.EmpName,RD.Bonus )";

            }
            else if (DDJobType.SelectedIndex > 0 && DDQualityType.SelectedIndex > 0 && DDQuality.SelectedIndex > 0 && DDSize.SelectedIndex < 0)
            {
                where = where + "And Effectivedate in (Select max(Effectivedate) From RateDesc Where Effectivedate <='" + txtEffectiveDate.Text + "' And JobTypeId=" + DDJobType.SelectedValue + " and RD.Item_Id=" + DDQualityType.SelectedValue + " and RD.QualityId=" + DDQuality.SelectedValue + @"
                        Group by IM.ITEM_NAME,QualityName,DesignName,ColorName," + sizeColumn + @",ShapeName,PNM.PROCESS_NAME,CO.CalcName,Rate,RD.QualityId,RD.JobTypeId,CO.CalcId,RateId,IM.ITEM_ID,RD.DesignId,RD.ColorId,RD.SizeId,Rate2,EffectiveDate,RD.Shapeid,S.Shapename, RD.Ratelocation,RD.Unitid,U.UnitName, RD.Remark, EI.EmpCode, EI.EmpName,RD.Bonus )";
            }
            else if (DDJobType.SelectedIndex > 0 && DDQualityType.SelectedIndex > 0 && DDQuality.SelectedIndex > 0 && DDSize.SelectedIndex > 0)
            {
                where = where + "And Effectivedate in (Select max(Effectivedate) From RateDesc Where Effectivedate <='" + txtEffectiveDate.Text + "' And JobTypeId=" + DDJobType.SelectedValue + " and RD.Item_Id=" + DDQualityType.SelectedValue + " and RD.QualityId=" + DDQuality.SelectedValue + " and RD.SizeId=" + DDSize.SelectedValue + @"
                        Group by IM.ITEM_NAME,QualityName,DesignName,ColorName," + sizeColumn + @",ShapeName,PNM.PROCESS_NAME,CO.CalcName,Rate,RD.QualityId,RD.JobTypeId,CO.CalcId,RateId,IM.ITEM_ID,RD.DesignId,RD.ColorId,RD.SizeId,Rate2,EffectiveDate,RD.Shapeid,S.Shapename, RD.Ratelocation,RD.Unitid,U.UnitName, RD.Remark, EI.EmpCode, EI.EmpName,RD.Bonus )";
            }
            else if (DDJobType.SelectedIndex > 0 && DDQualityType.SelectedIndex > 0 && DDQuality.SelectedIndex < 0 && DDSize.SelectedIndex > 0)
            {
                where = where + "And Effectivedate in (Select max(Effectivedate) From RateDesc Where Effectivedate <='" + txtEffectiveDate.Text + "' And JobTypeId=" + DDJobType.SelectedValue + " and RD.Item_Id=" + DDQualityType.SelectedValue + " and RD.SizeId=" + DDSize.SelectedValue + @"
                        Group by IM.ITEM_NAME,QualityName,DesignName,ColorName," + sizeColumn + @",ShapeName,PNM.PROCESS_NAME,CO.CalcName,Rate,RD.QualityId,RD.JobTypeId,CO.CalcId,RateId,IM.ITEM_ID,RD.DesignId,RD.ColorId,RD.SizeId,Rate2,EffectiveDate,RD.Shapeid,S.Shapename, RD.Ratelocation,RD.Unitid,U.UnitName, RD.Remark, EI.EmpCode, EI.EmpName,RD.Bonus )";
            }
        }
        where = where + @" Group by IM.ITEM_NAME,QualityName,DesignName,ColorName," + sizeColumn + @",ShapeName,PNM.PROCESS_NAME,CO.CalcName,Rate,RD.QualityId,RD.JobTypeId,CO.CalcId,RateId,
                            IM.ITEM_ID,RD.DesignId,RD.ColorId,RD.SizeId,Rate2,EffectiveDate,ReIssRate,ToDate,RD.CustomerId,RD.Shapeid,S.Shapename,CI.customercode,RD.Shapeid,RD.Ratelocation,RD.Unitid,U.UnitName, RD.Remark, EI.EmpCode, EI.EmpName,RD.Bonus ";

        where = where + @" Order by QualityName";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@RateId", "0");
            param[1] = new SqlParameter("@Where", where);

            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetFinisherJobRate", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                GVFinisherJobRate.DataSource = ds.Tables[0];
                GVFinisherJobRate.DataBind();
            }
            else
            {
                GVFinisherJobRate.DataSource = null;
                GVFinisherJobRate.DataBind();
            }

            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            llMessageBox.Text = ex.Message;
            con.Close();
        }
    }
    protected void GVFinisherJobRate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVFinisherJobRate, "select$" + e.Row.RowIndex);

            for (int i = 0; i < GVFinisherJobRate.Columns.Count; i++)
            {
                if (GVFinisherJobRate.Columns[i].HeaderText == "Shape")
                {
                    if (Session["varcompanyId"].ToString() == "20")
                    {
                        GVFinisherJobRate.Columns[i].Visible = false;
                    }
                    else
                    {
                        GVFinisherJobRate.Columns[i].Visible = true;
                    }
                }
                if (GVFinisherJobRate.Columns[i].HeaderText == "Rate Location")
                {
                    if (variable.VarFINISHINGRATE_LOCATIONWISE == "1")
                    {
                        GVFinisherJobRate.Columns[i].Visible = true;
                    }
                    else
                    {
                        GVFinisherJobRate.Columns[i].Visible = false;
                    }
                }
                if (GVFinisherJobRate.Columns[i].HeaderText == "Unit")
                {
                    if (variable.VarAddFinisherJobRateByFtMtr == "1")
                    {
                        GVFinisherJobRate.Columns[i].Visible = true;
                    }
                    else
                    {
                        GVFinisherJobRate.Columns[i].Visible = false;
                    }
                }
                if (GVFinisherJobRate.Columns[i].HeaderText == "Bonus")
                {
                    if (Session["varcompanyId"].ToString() == "42")
                    {
                        GVFinisherJobRate.Columns[i].Visible = true;
                    }
                    else
                    {
                        GVFinisherJobRate.Columns[i].Visible = false;
                    }
                }
              
            }

        }
    }
    protected void GVFinisherJobRate_SelectedIndexChanged(object sender, EventArgs e)
    {
        int r = Convert.ToInt32(GVFinisherJobRate.SelectedIndex.ToString());

        llMessageBox.Text = "";

        BtnSave.Text = "Save";

        string id = GVFinisherJobRate.SelectedDataKey.Value.ToString();
        hnRateId.Value = id;

        ViewState["RateId"] = id;

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@RateId", id);

            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetFinisherJobRate", param);

            if (ds.Tables[0].Rows.Count == 1)
            {
                DDJobType.SelectedValue = ds.Tables[0].Rows[0]["JobTypeId"].ToString();
                //BindJobType();
                DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["CustomerId"].ToString();
                // BindCustomerCode();

                BindQualityType();
                DDQualityType.SelectedValue = ds.Tables[0].Rows[0]["Item_Id"].ToString();

                BindQuality();
                DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();

                BindDesign();
                DDDesign.SelectedValue = ds.Tables[0].Rows[0]["DesignId"].ToString();

                BindColor();
                DDColor.SelectedValue = ds.Tables[0].Rows[0]["ColorId"].ToString();
                if (DDShape.Items.FindByValue(ds.Tables[0].Rows[0]["shapeid"].ToString()) != null)
                {
                    DDShape.SelectedValue = ds.Tables[0].Rows[0]["shapeid"].ToString();
                }


                if (Session["varCompanyId"].ToString() == "9")
                {
                    if (ds.Tables[0].Rows[0]["UnitId"].ToString() == "2")
                    {
                        chkForFtSize.Checked = true;
                        ChkForInchSize.Checked = false;
                    }
                    else if (ds.Tables[0].Rows[0]["UnitId"].ToString() == "6")
                    {
                        ChkForInchSize.Checked = true;
                        chkForFtSize.Checked = false;
                    }
                }

                BindSize();
                DDSize.SelectedValue = ds.Tables[0].Rows[0]["SizeId"].ToString();

                BindCalc();
                DDCalcOption.SelectedValue = ds.Tables[0].Rows[0]["CalcId"].ToString();
                if (DDratelocation.Items.FindByValue(ds.Tables[0].Rows[0]["Ratelocationval"].ToString()) != null)
                {
                    DDratelocation.SelectedValue = ds.Tables[0].Rows[0]["Ratelocationval"].ToString();
                }
                txtRate.Text = ds.Tables[0].Rows[0]["Rate"].ToString();
                txtRate2.Text = ds.Tables[0].Rows[0]["Rate2"].ToString();
                txtReIssRate.Text = ds.Tables[0].Rows[0]["ReIssRate"].ToString();
                txtEffectiveDate.Text = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[0]["EffectiveDate"].ToString()).ToString("dd-MMM-yyyy"));
                txtBonusRate.Text = ds.Tables[0].Rows[0]["Bonus"].ToString();

                TxtRemark.Text = ds.Tables[0].Rows[0]["Remark"].ToString();

                if (DDUnit.Items.FindByValue(ds.Tables[0].Rows[0]["Unitid"].ToString()) != null)
                {
                    DDUnit.SelectedValue = ds.Tables[0].Rows[0]["UnitId"].ToString();
                }
             

                DDJobType.Enabled = false;
                DDQualityType.Enabled = false;
                DDQuality.Enabled = false;
                DDDesign.Enabled = false;
                DDColor.Enabled = false;
                DDSize.Enabled = false;
                DDCalcOption.Enabled = false;

            }
            //BindGrid();            

            Tran.Commit();
        }
        catch (Exception ex)
        {
            llMessageBox.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        BtnSave.Text = "Update";
    }


    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSize();
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        string str = "";
        //Check Conditions 
       
        if (DDCustomerCode.SelectedIndex > 0)
        {
            str = str + " And RD.CustomerID=" + DDCustomerCode.SelectedValue;
        }
        if (DDJobType.SelectedIndex > 0)
        {
            str = str + " And RD.JobTypeId=" + DDJobType.SelectedValue;
        }
        if (DDQualityType.SelectedIndex > 0)
        {
            str = str + " And RD.Item_Id=" + DDQualityType.SelectedValue;
        }
        if (DDJobType.SelectedIndex > 0 && DDQualityType.SelectedIndex > 0)
        {
            str = str + "  And RD.Effectivedate In (Select Max(EffectiveDate) From RateDesc Where Effectivedate <='" + txtEffectiveDate.Text + @"' and RD.ToDate is null";
            str = str + " Group By JobTypeId,Item_Id,QualityId,DesignId,ColorId,SizeId,CalcOptionId,Rate,Rate2,ReIssRate,ShapeId,EffectiveDate,Bonus) ";
        }
        if (DDJobType.SelectedIndex > 0 && DDQualityType.SelectedIndex == 0)
        {
            str = str + "  And RD.Effectivedate In (Select Max(EffectiveDate) From RateDesc Where Effectivedate <='" + txtEffectiveDate.Text + @"' And JobTypeId=" + DDJobType.SelectedValue + @" and RD.ToDate is null";
            str = str + " Group By JobTypeId,Item_Id,QualityId,DesignId,ColorId,SizeId,CalcOptionId,Rate,Rate2,ReIssRate,ShapeId,EffectiveDate,Bonus)";
        }
        else if (DDJobType.SelectedIndex > 0 && DDQualityType.SelectedIndex > 0)
        {
            str = str + "  And RD.Effectivedate In (Select Max(EffectiveDate) From RateDesc Where Effectivedate <='" + txtEffectiveDate.Text + @"' And JobTypeId=" + DDJobType.SelectedValue + @"
                        And Item_Id=" + DDQualityType.SelectedValue + @" and RD.ToDate is null";
            str = str + " Group By JobTypeId,Item_Id,QualityId,DesignId,ColorId,SizeId,CalcOptionId,Rate,Rate2,ReIssRate,ShapeId,EffectiveDate,Bonus)";
        }
        str = str + " Group By PNM.Process_Name,IM.Item_Name,RD.QualityId,Q.QualityName,RD.DesignId,D.DesignName,RD.ColorId,C.ColorName,RD.SizeID,QS.Finishing_Mt_Size,S.ShapeName,RD.Rate,Rd.Rate2,RD.ReIssRate,RD.EffectiveDate,CO.CalcName,CI.CustomerCode,RD.Remark,RD.Bonus  ";
        str = str + " Order by IM.Item_Name";


        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@userid", Session["varuserid"]);
        param[1] = new SqlParameter("@MasterCompanyId", Session["varCompanyId"]);
        param[2] = new SqlParameter("@where", str);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetFinisherJobRateReport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Session["VarCompanyNo"].ToString() == "16" || Session["VarCompanyNo"].ToString() == "28")
            {
                FinisherJobRateInExcel(ds);
            }
            else
            {
                Session["rptFileName"] = "Reports/RptFinisherRateList.rpt";
                Session["dsFileName"] = "~\\ReportSchema\\RptFinisherRateList.xsd";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
        }
        //*************
    }
    protected void BtnPreviewBetweenDate_Click(object sender, EventArgs e)
    {
        
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@FromDate", txtFromDate.Text);
            param[1] = new SqlParameter("@ToDate", txtToDate.Text);
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@MasterCompanyId", Session["varCompanyId"]);
            param[4] = new SqlParameter("@JobTypeId", DDJobType.SelectedIndex>0 ? DDJobType.SelectedValue:"0");

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetFinisherJobRateReportBetweenDate", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Session["varCompanyNo"].ToString() == "42")
                {
                    Session["rptFileName"] = "Reports/RptFinisherRateListBetweenDateVikram.rpt";
                }
                else
                {
                    Session["rptFileName"] = "Reports/RptFinisherRateListBetweenDate.rpt";
                }
               
                Session["dsFileName"] = "~\\ReportSchema\\RptFinisherRateListBetweenDate.xsd";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
            }
               

    }
    private void FinisherJobRateInExcel(DataSet ds)
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

            sht.Range("A1:O1").Merge();
            sht.Range("A1").Value = "FINISHING RATE LIST";
            //sht.Range("A2:X2").Merge();
            //sht.Range("A2").Value = "Filter By :  " + FilterBy;
            //sht.Row(2).Height = 30;
            sht.Range("A1:O1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:O2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:O2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A2:O2").Style.Alignment.SetWrapText();
            sht.Range("A1:O2").Style.Font.FontName = "Arial";
            sht.Range("A1:O2").Style.Font.FontSize = 10;
            sht.Range("A1:O2").Style.Font.Bold = true;

            //*******Header
            sht.Range("A3").Value = "Sr No.";
            sht.Range("B3").Value = "Quality Type";
            sht.Range("C3").Value = "Quality Name";
            sht.Range("D3").Value = "Design";
            sht.Range("E3").Value = "Color";
            sht.Range("F3").Value = "Size";
            sht.Range("G3").Value = "Shape";
            sht.Range("H3").Value = "JobType";

            sht.Range("I3").Value = "Cust Code";
            sht.Range("J3").Value = "CalcOption";
            sht.Range("K3").Value = "Rate";
            sht.Range("L3").Value = "Rate2";
            sht.Range("M3").Value = "Effective Date";
            sht.Range("N3").Value = "ReIssRate";
            sht.Range("O3").Value = "Remark";


            sht.Range("A3:O3").Style.Font.FontName = "Arial";
            sht.Range("A3:O3").Style.Font.FontSize = 9;
            sht.Range("A3:O3").Style.Font.Bold = true;
            sht.Range("S3:O3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3:O3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A3:O3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A3:O3").Style.Alignment.SetWrapText();


            //DataView dv = new DataView(ds.Tables[0]);
            //dv.Sort = "FOLIONO";
            //DataSet ds1 = new DataSet();
            //ds1.Tables.Add(dv.ToTable());

            int srno = 0;
            row = 4;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":O" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":O" + row).Style.Font.FontSize = 8;

                srno = srno + 1;

                sht.Range("A" + row).SetValue(srno);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["QualityType"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Quality"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Size"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["ShapeName"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Process_Name"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["calcName"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Rate2"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["EffectiveDate"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["ReIssRate"]);
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["Remark"]);


                row = row + 1;

            }
            //*************
            sht.Columns(1, 26).AdjustToContents();

            //sht.Columns("K").Width = 13.43;

            using (var a = sht.Range("A1" + ":O" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("FinisherJobRate_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid(0);
    }

    protected void lnkdelClick(object sender, EventArgs e)
    {
        llMessageBox.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {

            LinkButton lnkdel = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)lnkdel.NamingContainer;

            Label lblRateID = (Label)gvr.FindControl("lblRateId");

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@RateID", lblRateID.Text);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@MastercompanyId", Session["varcompanyNo"]);
            param[3] = new SqlParameter("@Userid", Session["varuserid"]);
            //********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteFinisherJobRate", param);
            llMessageBox.Text = param[1].Value.ToString();
            Tran.Commit();
            BindGrid(0);
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            llMessageBox.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
}