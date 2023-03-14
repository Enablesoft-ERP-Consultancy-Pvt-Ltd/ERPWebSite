using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class UserControls_attachConsumption : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            UtilityModule.ConditionalComboFill(ref DDCategoryname, "select ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=Cs.Categoryid and cs.id=0", true, "--Plz Select--");
            UtilityModule.ConditionalComboFill(ref DDACategory, "select ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=Cs.Categoryid and cs.id=0", true, "--Plz Select--");


            string str = "Select Process_Name_id,Process_Name from process_Name_Master order by process_Name";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDProcess, ds, 0, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDAProcess, ds, 0, true, "--Select--");

            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select WithBuyerCode,VarProdCode from Mastersetting");
            ViewState["WithBuyercode"] = ds2.Tables[0].Rows[0]["WithBuyerCode"].ToString();
            ds.Dispose();
        }

    }
    protected void DDCategoryname_SelectedIndexChanged(object sender, EventArgs e)
    {
        CategoryDependControls();
    }
    protected void CategoryDependControls()
    {
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
        TDShape.Visible = false;
        TDSize.Visible = false;
        TDShade.Visible = false;

        UtilityModule.ConditionalComboFill(ref DDItemName, "select ITEM_ID,ITEM_NAME from ITEM_MASTER where CATEGORY_ID=" + DDCategoryname.SelectedValue + " order by ITEM_NAME", true, "--Select--");
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select PARAMETER_ID from ITEM_CATEGORY_PARAMETERS Where CATEGORY_ID=" + DDCategoryname.SelectedValue + "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (Convert.ToString(dr["PARAMETER_ID"]))
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
                        break;
                }
            }

        }
    }
    protected void CategoryAttachDependControls()
    {
        TDAQuality.Visible = false;
        TDADesign.Visible = false;
        TDAColor.Visible = false;
        TDAShape.Visible = false;
        TDASize.Visible = false;
        TDAShade.Visible = false;

        UtilityModule.ConditionalComboFill(ref DDAItem, "select ITEM_ID,ITEM_NAME from ITEM_MASTER where CATEGORY_ID=" + DDACategory.SelectedValue + " order by ITEM_NAME", true, "--Select--");
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select PARAMETER_ID from ITEM_CATEGORY_PARAMETERS Where CATEGORY_ID=" + DDACategory.SelectedValue + "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (Convert.ToString(dr["PARAMETER_ID"]))
                {
                    case "1":
                        TDAQuality.Visible = true;
                        break;
                    case "2":
                        TDADesign.Visible = true;

                        break;
                    case "3":
                        TDAColor.Visible = true;

                        break;
                    case "4":
                        TDAShape.Visible = true;
                        break;
                    case "5":
                        TDASize.Visible = true;
                        break;
                }
            }

        }
    }
    protected void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Sizetype, int Itemid, System.Web.UI.HtmlControls.HtmlTableCell tdQuality = null, System.Web.UI.HtmlControls.HtmlTableCell tdDesign = null, System.Web.UI.HtmlControls.HtmlTableCell tdcolor = null, System.Web.UI.HtmlControls.HtmlTableCell tdshape = null)
    {
        if (tdQuality.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref Quality, "select QualityId,QualityName from Quality Where Item_Id=" + Itemid + " order by QualityName", true, "--Select--");
        }

        string str;
        str = @"SELECT DESIGNID,DESIGNNAME from DESIGN Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By DESIGNNAME
            SELECT COLORID,COLORNAME FROM COLOR Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By COLORNAME
            SELECT SHAPEID,SHAPENAME FROM SHAPE Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By SHAPENAME";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (tdDesign.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref Design, ds, 0, true, "--Select--");
        }
        if (tdcolor.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref Color, ds, 1, true, "--Select--");
        }
        if (tdshape.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref Shape, ds, 2, true, "--Select--");
            UtilityModule.ConditionalComboFill(ref Sizetype, "select val,type from sizetype", false, "");
        }
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(DDQuality, DDDesign, DDColor, DDshape, DDsizetype, Convert.ToInt16(DDItemName.SelectedValue), TDQuality, TDDesign, TDColor, TDShape);
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDesign(DDItemName, DDQuality, DDDesign, TDDesign);
    }
    protected void FillDesign(DropDownList dditem, DropDownList ddquality, DropDownList DDDesign, System.Web.UI.HtmlControls.HtmlTableCell tdDesign = null)
    {
        // string str = "";        
        if (tdDesign.Visible == true)
        {
            if (ViewState["WithBuyercode"].ToString() == "1")
            {
                UtilityModule.ConditionalComboFill(ref DDDesign, @"select Distinct V.DesignId,DesignName As Designname  
                                from ITEM_PARAMETER_MASTER  IM inner join Design V on Im.DESIGN_ID=V.designId 
                                inner join CustomerDesign CD on CD.DesignId=V.designId and Im.Item_Id=" + dditem.SelectedValue + @" and 
                                    IM.QUALITY_ID=" + ddquality.SelectedValue + " and V.masterCompanyId=" + Session["varcompanyId"] + " order by designname", true, "--SELECT--");

            }
            else
            {
                //                UtilityModule.ConditionalComboFill(ref DDDesign, @"select Distinct V.DesignId,DesignName+Space(2)+isnull(DesignNameAtoC,'') As Designname
                //                                                             from Design V Left Outer Join CustomerDesign CD on V.DesignId=CD.DesignId And CustomerId=" + DDCustomerCode.SelectedValue + " Where V.MasterCompanyId=" + Session["varCompanyId"] + " order by designname", true, "--SELECT--");
                UtilityModule.ConditionalComboFill(ref DDDesign, "SELECT DESIGNID,DESIGNNAME from DESIGN Where  MasterCompanyId=" + Session["varCompanyId"] + " Order By DESIGNNAME", true, "--SELECT--");
            }
        }

    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {

        fillColor(DDItemName, DDQuality, DDDesign, DDColor, TDColor);    
        
        //if (Session["varcompanyId"].ToString() == "4")
        //{
        //    fillColor(ddItemName, ddQuality, ddDesign, ddColor, Color);
        //}
        //UtilityModule.ConditionalComboFill(ref ddColor, "SELECT COLORID,COLORNAME FROM COLOR", true, "--SELECT COLOR--");
    }
    protected void fillColor(DropDownList dditem, DropDownList ddquality, DropDownList DDDesign, DropDownList DDColor, System.Web.UI.HtmlControls.HtmlTableCell tdcolor = null)
    {
        if (tdcolor.Visible == true)
        {
            if (ViewState["WithBuyercode"].ToString() == "1")
            {

                UtilityModule.ConditionalComboFill(ref DDColor, @"select Distinct C.ColorId,C.ColorName As ColorName from ITEM_PARAMETER_MASTER  IM inner join Color c on
                                                                     Im.COLOR_ID=C.ColorId inner join CustomerColor  CC on CC.ColorId=C.ColorId
                                                                      and Im.Item_Id=" + dditem.SelectedValue + " and IM.QUALITY_ID=" + ddquality.SelectedValue + " and DESIGN_ID=" + DDDesign.SelectedValue + " and C.MasterCompanyId=" + Session["varCompanyId"] + " order by ColorName", true, "--SELECT--");

            }
            else
            {
                //                UtilityModule.ConditionalComboFill(ref DDColor, @"select Distinct C.ColorId,C.ColorName+space(2)+isnull(CC.ColorNameToC,'') As ColorName
                //                                                             from Color C Left Outer Join CustomerColor CC on C.ColorId=CC.ColorId And CustomerId=" + DDCustomerCode.SelectedValue + " Where C.MasterCompanyId=" + Session["varCompanyId"] + " order by ColorName", true, "--SELECT--");
                UtilityModule.ConditionalComboFill(ref DDColor, "SELECT COLORID,COLORNAME FROM COLOR Where  MasterCompanyId=" + Session["varCompanyId"] + " Order By COLORNAME", true, "--SELECT--");
            }
        }
    }
  
    protected void DDshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize(DDsizetype, DDshape, DDSize);
    }
    protected void FillSize(DropDownList SizeType, DropDownList Shape, DropDownList Size)
    {
        string size = "";
        string str = "";

        switch (SizeType.SelectedValue)
        {
            case "1":
                size = "Sizemtr";
                break;
            case "0":
                size = "Sizeft";
                break;
            case "2":
                size = "Sizeinch";
                break;
            default:
                size = "Sizeft";
                break;
        }

        str = "Select Distinct S.Sizeid,S." + size + " As  " + size + @" From Size S 
                 Where shapeid=" + Shape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " order by " + size + "";

        UtilityModule.ConditionalComboFill(ref Size, str, true, "--Select--");
    }

    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize(DDsizetype, DDshape, DDSize);
    }
    protected void DDACategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        CategoryAttachDependControls();
    }
    protected void DDAItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(DDAQuality, DDADesign, DDAColor, DDAShape, DDASizeType, Convert.ToInt16(DDAItem.SelectedValue), TDAQuality, TDADesign, TDAColor, TDAShape);
    }
    protected void DDAShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize(DDASizeType, DDAShape, DDASize);
    }
    protected void DDASizeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize(DDASizeType, DDAShape, DDASize);
    }
    protected void FIllGrid()
    {
        string str = @"select pcd.PCMDID,pcm.PCMID,pcm.finishedid,pcd.IFINISHEDID,pcd.OFINISHEDID,pnm.PROCESS_NAME,PNM.Process_Name_id,vfinput.CATEGORY_NAME+SPACE(2)+vfinput.ITEM_NAME+SPACE(2)+
                    vfinput.designName+SPACE(2)+vfinput.ColorName+SPACE(2)+vfinput.ShapeName+SPACE(2)+case When pcd.ISizeflag=0 Then vfinput.SizeFt else case When pcd.ISizeflag=1 Then vfinput.sizemtr else vfinput.sizeinch End End+space(2)+vfinput.shadecolorname as InputItem, 
                    Round(case When PCM.flagmtrft=1 Then pcd.IQTY*1.196 Else PCD.IQTy End,3) as IQty,pcd.ILOSS,pcd.IRATE,Uinput.UnitName as IUnit,
                    vfout.CATEGORY_NAME+SPACE(2)+vfout.ITEM_NAME+SPACE(2)+
                    vfout.designName+SPACE(2)+vfout.ColorName+SPACE(2)+vfout.ShapeName+SPACE(2)+case When pcd.ISizeflag=0 Then vfout.SizeFt else case When pcd.ISizeflag=1 Then vfout.sizemtr else vfout.sizeinch End End+space(2)+vfout.shadecolorname as OutputItem,
                    Round(case When PCM.Flagmtrft=1 Then pcd.OQTY*1.196 Else PCD.OQTY ENd,3) as OQty,pcd.ORATE,Uoutput.UnitName as OUnit,pcd.DyingMatch,pcd.Dyeing,pcd.DyingType
                    from PROCESSCONSUMPTIONMASTER PCM inner join PROCESSCONSUMPTIONDETAIL PCD
                    on PCM.PCMID=PCD.PCMID
                    inner join PROCESS_NAME_MASTER PNM on PNM.PROCESS_NAME_ID=PCM.processid
                    inner join V_FinishedItemDetail vfinput on vfinput.ITEM_FINISHED_ID=pcd.IFINISHEDID
                    inner join V_FinishedItemDetail vfOut on vfOut.ITEM_FINISHED_ID=Pcd.OFINISHEDID
                    inner join Unit Uinput on Uinput.UnitId=pcd.IUNITID
                    inner join unit Uoutput on Uoutput.UnitId=pcd.OUNITID
                    inner join V_FinishedItemDetail vf on vf.ITEM_FINISHED_ID=pcm.FINISHEDID Where PCM.MastercompanyId=" + Session["varcompanyId"] + " and vf.CATEGORY_ID=" + DDCategoryname.SelectedValue + " and vf.item_id=" + DDItemName.SelectedValue;
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.qualityid=" + DDQuality.SelectedValue;
        }
        if (CHKFORALLDESIGN.Checked == true)
        {
            str = str + " and vf.DesignId=-1";
        }
        else if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
        }
        if (CHKFORALLCOLOR.Checked == true)
        {
            str = str + " and vf.Colorid=-1";
        }
        else if (DDColor.SelectedIndex > 0)
        {
            str = str + " and vf.Colorid=" + DDColor.SelectedValue;
        }
        if (DDshape.SelectedIndex > 0)
        {
            str = str + " and vf.Shapeid=" + DDshape.SelectedValue;
        }
        if (CHKFORALLSIZE.Checked == true)
        {
            str = str + " and vf.sizeid=-1";
        }
        else if (DDSize.SelectedIndex > 0)
        {
            str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
        }
        if (DDProcess.SelectedIndex > 0)
        {
            str = str + " and PCM.Processid=" + DDProcess.SelectedValue;
        }
        //str = str + " order by pcd.pcmdid,PNM.Process_name";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
        ds.Dispose();
    }
    protected void BtnShowDetail_Click(object sender, EventArgs e)
    {
        FIllGrid();
    }
    protected void CHKFORALLDESIGN_CheckedChanged(object sender, EventArgs e)
    {
        if (CHKFORALLDESIGN.Checked == true)
        {
            DDDesign.Items.RemoveAt(0);
            ListItem li = new ListItem();
            li.Text = "--ALL--";
            li.Value = "0";
            DDDesign.Items.Insert(0, li);
        }
        else
        {
            DDDesign.Items.RemoveAt(0);
            ListItem li = new ListItem();
            li.Text = "--Select--";
            li.Value = "0";
            DDDesign.Items.Insert(0, li);
        }

    }
    protected void CHKFORALLCOLOR_CheckedChanged(object sender, EventArgs e)
    {
        if (CHKFORALLCOLOR.Checked == true)
        {
            DDColor.Items.RemoveAt(0);
            ListItem li = new ListItem();
            li.Text = "--ALL--";
            li.Value = "0";
            DDColor.Items.Insert(0, li);
        }
        else
        {
            DDColor.Items.RemoveAt(0);
            ListItem li = new ListItem();
            li.Text = "--Select--";
            li.Value = "0";
            DDColor.Items.Insert(0, li);
        }
    }
    protected void CHKFORALLSIZE_CheckedChanged(object sender, EventArgs e)
    {
        if (CHKFORALLSIZE.Checked == true)
        {
            if (DDSize.Items.Count > 0)
            {
                DDSize.Items.RemoveAt(0);
            }
            ListItem li = new ListItem();
            li.Text = "--ALL--";
            li.Value = "0";
            DDSize.Items.Insert(0, li);
        }
        else
        {
            if (DDSize.Items.Count > 0)
            {
                DDSize.Items.RemoveAt(0);
            }
            ListItem li = new ListItem();
            li.Text = "--Select--";
            li.Value = "0";
            DDSize.Items.Insert(0, li);
        }
    }
    protected void chkAForAllDesign_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAForAllDesign.Checked == true)
        {
            DDADesign.Items.RemoveAt(0);
            ListItem li = new ListItem();
            li.Text = "--ALL--";
            li.Value = "0";
            DDADesign.Items.Insert(0, li);
        }
        else
        {
            DDADesign.Items.RemoveAt(0);
            ListItem li = new ListItem();
            li.Text = "--Select--";
            li.Value = "0";
            DDADesign.Items.Insert(0, li);
        }

    }
    protected void chkAForallColor_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAForallColor.Checked == true)
        {
            DDAColor.Items.RemoveAt(0);
            ListItem li = new ListItem();
            li.Text = "--ALL--";
            li.Value = "0";
            DDAColor.Items.Insert(0, li);
        }
        else
        {
            DDAColor.Items.RemoveAt(0);
            ListItem li = new ListItem();
            li.Text = "--Select--";
            li.Value = "0";
            DDAColor.Items.Insert(0, li);
        }
    }
    protected void chkAForallSize_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAForallSize.Checked == true)
        {
            if (DDASize.Items.Count > 0)
            {
                DDASize.Items.RemoveAt(0);
            }
            ListItem li = new ListItem();
            li.Text = "--ALL--";
            li.Value = "0";
            DDASize.Items.Insert(0, li);
        }
        else
        {
            if (DDASize.Items.Count > 0)
            {
                DDASize.Items.RemoveAt(0);
            }
            ListItem li = new ListItem();
            li.Text = "--Select--";
            li.Value = "0";
            DDASize.Items.Insert(0, li);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection Con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (Con.State == ConnectionState.Closed)
        {
            Con.Open();
        }
        SqlTransaction Tran = Con.BeginTransaction();
        try
        {
            lblerrormsg.Text = "";
            int PCMID = 0;
            int AttachFinishedid = UtilityModule.getItemFinishedId(DDAItem, DDAQuality, DDADesign, DDAColor, DDAShape, DDASize, txtAProdCode, DDAshade, chkAForAllDesign, chkAForallColor, chkAForallSize, "", Convert.ToInt32(Session["varCompanyId"]));
            SqlParameter[] param = new SqlParameter[18];
            param[0] = new SqlParameter("@EPCMID", SqlDbType.Int);
            param[1] = new SqlParameter("@EPCMDID", SqlDbType.Int);
            param[2] = new SqlParameter("@Finishedid", SqlDbType.Int);
            param[3] = new SqlParameter("@IFinishedid", SqlDbType.Int);
            param[4] = new SqlParameter("@OFinishedid", SqlDbType.Int);
            param[5] = new SqlParameter("@PCMID", SqlDbType.Int);
            param[6] = new SqlParameter("@PCMDID", SqlDbType.Int);
            param[7] = new SqlParameter("@EProcessid", SqlDbType.Int);
            param[8] = new SqlParameter("@Processid", SqlDbType.Int);
            param[9] = new SqlParameter("@AttachFinishedid", SqlDbType.Int);
            param[10] = new SqlParameter("@GridSrno", SqlDbType.Int);
            param[11] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[12] = new SqlParameter("@Userid", SqlDbType.Int);
            param[13] = new SqlParameter("@IQTY", SqlDbType.Float);
            param[14] = new SqlParameter("@ILoss", SqlDbType.Float);
            param[15] = new SqlParameter("@IRate", SqlDbType.Float);
            param[16] = new SqlParameter("@OQTY", SqlDbType.Float);
            param[17] = new SqlParameter("@ORate", SqlDbType.Float);


            param[5].Direction = ParameterDirection.InputOutput;
            param[11].Direction = ParameterDirection.Output;

            for (int i = 0; i < DG.Rows.Count; i++)
            {
                CheckBox chk = ((CheckBox)DG.Rows[i].FindControl("ChkItem"));
                if (chk.Checked == true)
                {
                    string EPCMID = ((Label)DG.Rows[i].FindControl("lblPCMID")).Text;
                    string EPCMDID = DG.DataKeys[i].Value.ToString();
                    string Finishedid = ((Label)DG.Rows[i].FindControl("lblFinishedid")).Text;
                    string IFinishedid = ((Label)DG.Rows[i].FindControl("lblIFinishedid")).Text;
                    string OFinishedid = ((Label)DG.Rows[i].FindControl("lblOFinishedid")).Text;
                    string EProcessid = ((Label)DG.Rows[i].FindControl("lblProcessid")).Text;
                    TextBox txtIQty = ((TextBox)DG.Rows[i].FindControl("txtIQty"));
                    TextBox txtILoss = ((TextBox)DG.Rows[i].FindControl("txtILoss"));
                    TextBox txtIRate = ((TextBox)DG.Rows[i].FindControl("txtIRate"));
                    TextBox txtOQty = ((TextBox)DG.Rows[i].FindControl("txtOQty"));
                    TextBox txtORate = ((TextBox)DG.Rows[i].FindControl("txtORATE"));

                    param[0].Value = EPCMID;
                    param[1].Value = EPCMDID;
                    param[2].Value = Finishedid;
                    param[3].Value = IFinishedid;
                    param[4].Value = OFinishedid;
                    param[5].Value = PCMID;
                    param[6].Value = 0;
                    param[7].Value = EProcessid;
                    param[8].Value = DDAProcess.SelectedValue;
                    param[9].Value = AttachFinishedid;
                    param[10].Value = DG.Rows[i].DataItemIndex + 1;
                    param[12].Value = Session["varuserid"];
                    param[13].Value = txtIQty.Text == "" ? "0" : txtIQty.Text;
                    param[14].Value = txtILoss.Text == "" ? "0" : txtILoss.Text;
                    param[15].Value = txtIRate.Text == "" ? "0" : txtIRate.Text;
                    param[16].Value = txtOQty.Text == "" ? "0" : txtOQty.Text;
                    param[17].Value = txtORate.Text == "" ? "0" : txtORate.Text;

                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_saveAttachconsumption", param);

                    if (Convert.ToString(param[11].Value) != "")
                    {
                        PCMID = 0;
                        lblerrormsg.Text = Convert.ToString(param[11].Value);
                        Tran.Rollback();
                        return;
                    }
                }
            }
            Tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Data saved successfully....');", true);
            DG.DataSource = null;
            DG.DataBind();
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.Message;
            Tran.Rollback();

        }
        finally
        {
            Con.Close();
            Con.Dispose();
        }
    }
}
