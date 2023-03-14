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

public partial class Masters_Packing_FrmStockNoOutByInvoicePacking : System.Web.UI.Page
{
    private const string SCRIPT_DOFOCUS =
 @"window.setTimeout('DoFocus()', 1);
            function DoFocus()
            {
                try {
                    document.getElementById('REQUEST_LASTFOCUS').focus();
                } catch (ex) {}
            }";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            HookOnFocus(this.Page as Control);

            //replaces REQUEST_LASTFOCUS in SCRIPT_DOFOCUS with the posted value from Request["__LASTFOCUS"]
            //and registers the script to start after Update panel was rendered
            ScriptManager.RegisterStartupScript(
                this,
                typeof(Masters_Packing_FrmStockNoOutByInvoicePacking),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);

            logo();
           
            string Qry = @" select Distinct CI.CompanyId,CI.Companyname From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order by Companyname
                    Select CustomerId,CustomerCode + SPACE(5)+CompanyName From CustomerInfo Where MasterCompanyId=" + Session["varCompanyId"] + @" order by CustomerCode";
            DataSet ds1 = null;
            ds1 = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds1, 0, true, "--SELECT--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds1, 1, true, "--SELECT--");
            CustomerCodeSelectedIndexChange();

            //TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            ParameteLabel();

            int VarProdCode = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarProdCode From MasterSetting"));
            if (VarProdCode == 1)
            {
                TDProdCode.Visible = true;
            }
            else
            {
                TDProdCode.Visible = false;
            }

            // ViewState["PackingID"] = 0;
            //Session["PackingID"] = 0;
            ViewState["PACKINGDETAILID"] = 0;

            ////hnid.Value = "0";
            ////hnpackingid.Value = "0";
            ////hnfinished.Value = "";


        }
    }
    private void logo()
    {
        LblCompanyName.Text = Session["varCompanyName"].ToString();
        LblUserName.Text = Session["varusername"].ToString();
    }
    private void ParameteLabel()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblQualityName.Text = ParameterList[0];
        lblDesignName.Text = ParameterList[1];
        lblColorName.Text = ParameterList[2];
        lblShapeName.Text = ParameterList[3];
        lblSizeName.Text = ParameterList[4];
        lblCategoryName.Text = ParameterList[5];
        lblItemName.Text = ParameterList[6];
        lblShade.Text = ParameterList[7];
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerCodeSelectedIndexChange();
        if (DDCustomerOrderNo.SelectedIndex > 0)
        {
            ddlcategorycange();
        }
        // TxtInvoiceNo.Focus();
    }
    private void CustomerCodeSelectedIndexChange()
    {
       
        ////UtilityModule.ConditionalComboFill(ref ddInvoiceNo, "Select PackingID,TPackingNo+' / '+Replace(Convert(VarChar(11),PackingDate,106), ' ','-') PackingNo from Packing Where ConsignorId=" + DDCompanyName.SelectedValue + " And ConsigneeId=" + DDCustomerCode.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");

        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_BindInvoiceNoForStockOut", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@CompanyId", DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@CustomerId", DDCustomerCode.SelectedValue);           
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varcompanyId"]);
            cmd.Parameters.AddWithValue("@UserId", Session["varuserId"]);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

                UtilityModule.ConditionalComboFillWithDS(ref ddInvoiceNo, ds, 0, true, "--Plz Select--");
            }
            else
            {
                ddInvoiceNo.Items.Clear();
            }
        }
        catch (Exception)
        {
        }
        finally
        {

        }

        
    }
    protected void FillorderNo()
    {

        string str = "";
        str = @"select distinct OM.OrderId,OM.CustomerOrderNo CustomerOrderNo from Packing P JOIN PackingInformation PI ON P.PackingId=PI.PackingId
                JOIN OrderMaster OM ON PI.OrderId=OM.OrderId 
                 Where P.ConsignorId=" + DDCompanyName.SelectedValue + " And P.ConsigneeId=" + DDCustomerCode.SelectedValue + " and P.PackingId=" + ddInvoiceNo.SelectedValue + "  And P.MasterCompanyId=" + Session["varCompanyId"] + "  order by CustomerOrderNo";

        UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, str, true, "--SELECT--");

    }

    protected void DDCustomerOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDCustomerOrderNo_SelectedIndexChanged();
        if(DDCustomerOrderNo.SelectedIndex>0)
        {
            ddlcategorycange();
        }
        // ddCategoryName.Focus();       
    }
    private void DDCustomerOrderNo_SelectedIndexChanged()
    {
        UtilityModule.ConditionalComboFill(ref ddCategoryName, "Select Distinct Category_ID, Category_Name from OrderDetail OD,V_FinishedItemDetail VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And Orderid=" + DDCustomerOrderNo.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");

        //if (ddSize.SelectedIndex > 0)
        //{
        //    Fill_Price();
        //}

    }
    protected void ddCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Category_SelectedIndex_Change();
        //ddItemName.Focus();
    }
    private void Category_SelectedIndex_Change()
    {
        ddlcategorycange();
        string Str = @"Select Distinct VF.Item_ID,VF.Item_Name from V_FinishedItemDetail VF 
                JOIN OrderDetail OD ON VF.Item_Finished_Id=OD.Item_Finished_Id
        --left Outer join V_OrderPackDetailForStockOutPage OD on OD.Item_Finished_Id=VF.Item_Finished_Id 
        Where  Category_Id=" + ddCategoryName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "";
        //if (ChkForWithoutOrder.Checked != true)
        //{
        if (DDCustomerOrderNo.SelectedIndex > 0)
        {
            Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue;
        }

        //}
        Str = Str + " order by Item_Name";
        UtilityModule.ConditionalComboFill(ref ddItemName, Str, true, "--SELECT--");
    }
    private void ddlcategorycange()
    {
        ddQuality.Items.Clear();
        ddDesign.Items.Clear();
        ddColor.Items.Clear();
        ddShape.Items.Clear();
        ddSize.Items.Clear();
        ddShade.Items.Clear();
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
        TDShade.Visible = false;
        TDShape.Visible = false;
        TDSize.Visible = false;

        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME 
                      FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on 
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCategoryName.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
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
                    case "6":
                        TDShade.Visible = true;
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
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddItemName.SelectedIndex > 0)
        {
            BtnShow.Visible = true;
        }
        else
        {
            BtnShow.Visible = false;
        }
        ItemNameSelectedIndexChange();
        // ddQuality.Focus();

        FillShape();
        ShapeSelectedChange();

    }
    private void ItemNameSelectedIndexChange()
    {
        string Str = "";
        if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
        {
            Str = @"select Distinct CQ.SrNo,CQ.QualityNameAToC 
                    from CustomerQuality CQ 
                    inner join V_FinishedItemDetail vf on CQ.QualityId=vf.QualityId and CQ.CustomerId=" + DDCustomerCode.SelectedValue + @" 
                    --left join V_OrderPackDetailForStockOutPage Od on Od.Item_Finished_Id=vf.ITEM_FINISHED_ID
                    JOIN OrderDetail OD ON VF.Item_Finished_Id=OD.Item_Finished_Id 
                    Where vf.item_id=" + ddItemName.SelectedValue + " and vf.mastercompanyid=" + Session["varcompanyid"];

            if (DDCustomerOrderNo.SelectedIndex > 0)
            {
                Str = Str + " and Od.orderid=" + DDCustomerOrderNo.SelectedValue;
            }

            Str = Str + " order by QualityNameAToC";
        }
        else
        {
            Str = @"select Distinct vf.QualityId,vf.QualityName 
                    from V_FinishedItemDetail vf 
                    --left join V_OrderPackDetailForStockOutPage OD on vf.ITEM_FINISHED_ID=od.Item_Finished_Id 
                    JOIN OrderDetail OD ON VF.Item_Finished_Id=OD.Item_Finished_Id
                    Where  VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "";

            if (DDCustomerOrderNo.SelectedIndex > 0)
            {
                Str = Str + @" And OD.Orderid=" + DDCustomerOrderNo.SelectedValue;
            }

            Str = Str + " order by QualityName";
        }
        UtilityModule.ConditionalComboFill(ref ddQuality, Str, true, "--SELECT--");
    }
    protected void ddQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        ComboFill();
        //Fill_Price();
        //// ddDesign.Focus();
    }
    protected void ComboFill()
    {
        ddDesign.Items.Clear();
        ddColor.Items.Clear();
        ddShape.Items.Clear();
        ddSize.Items.Clear();
        ddShade.Items.Clear();

        if (TDDesign.Visible == true)
        {
            FillDesign();
        }
        if (TDColor.Visible == true)
        {
            FillColor();
        }
        if (TDShape.Visible == true)
        {
            FillShape();
        }
        if (TDShade.Visible == true)
        {
            FillShadeColor();
        }
    }

    protected void FillDesign()
    {
        string str = "";
        if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
        {
            //            str = @"select Distinct cd.SrNo,cd.DesignNameAToC 
            //                    From CustomerDesign cd 
            //                    inner join V_FinishedItemDetail vf on cd.DesignId=vf.designId                     
            //                   left join V_OrderPackDetailForStockOutPage od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id 
            //                    Where cd.CustomerId=" + DDCustomerCode.SelectedValue + @" And vf.item_id=" + ddItemName.SelectedValue + " and vf.mastercompanyid=" + Session["varcompanyid"];

            str = @"select Distinct cd.SrNo,cd.DesignNameAToC 
                    From CustomerDesign cd 
                    inner join V_FinishedItemDetail vf on cd.DesignId=vf.designId 
                    JOIN OrderDetail OD ON VF.Item_Finished_Id=OD.Item_Finished_Id                    
                    Where cd.CustomerId=" + DDCustomerCode.SelectedValue + @" And vf.item_id=" + ddItemName.SelectedValue + " and vf.mastercompanyid=" + Session["varcompanyid"];

            //if (ChkForWithoutOrder.Checked == false)
            //{
            if (DDCustomerOrderNo.SelectedIndex > 0)
            {
                str = str + "  and od.orderid=" + DDCustomerOrderNo.SelectedValue;
            }
            //}
            if (TDQuality.Visible == true)
            {
                if (ddQuality.SelectedIndex > 0)
                {
                    str = str + " and cd.CQSRNO=" + ddQuality.SelectedValue;
                }
            }
            str = str + " order by DesignNameAToC";
        }
        else
        {
            str = @"select Distinct vf.designId,vf.designName 
                        From V_FinishedItemDetail vf 
                        JOIN OrderDetail OD ON VF.Item_Finished_Id=OD.Item_Finished_Id 
                        --left join V_OrderPackDetailForStockOutPage od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id
                        Where vf.item_id=" + ddItemName.SelectedValue + " and vf.mastercompanyid=" + Session["varcompanyid"];
            //if (ChkForWithoutOrder.Checked == false)
            //{
            if (DDCustomerOrderNo.SelectedIndex > 0)
            {
                str = str + " and od.orderid=" + DDCustomerOrderNo.SelectedValue;
            }
            //}

            if (TDQuality.Visible == true)
            {
                if (ddQuality.SelectedIndex > 0)
                {
                    str = str + " and vf.qualityid=" + ddQuality.SelectedValue;
                }
            }
            str = str + "  order by designName";
        }
        UtilityModule.ConditionalComboFill(ref ddDesign, str, true, "--Select--");
    }
    protected void FillColor()
    {
        string str = "";
        if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
        {
            str = @"select Distinct CC.SrNo,CC.ColorNameToC 
                    From CustomerColor cc 
                    inner join V_FinishedItemDetail vf on CC.ColorId=vf.ColorId 
                    JOIN OrderDetail OD ON VF.Item_Finished_Id=OD.Item_Finished_Id 
                    --left join V_OrderPackDetailForStockOutPage od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id 
                    Where cc.CustomerId=" + DDCustomerCode.SelectedValue + " And vf.item_id=" + ddItemName.SelectedValue + " and vf.mastercompanyid=" + Session["varcompanyid"];

            //if (ChkForWithoutOrder.Checked == false)
            //{
            if (DDCustomerOrderNo.SelectedIndex > 0)
            {
                str = str + " and od.orderid=" + DDCustomerOrderNo.SelectedValue;
            }
            //}
            if (ddDesign.SelectedIndex > 0)
            {
                str = str + " and cc.CDSRNO=" + ddDesign.SelectedValue;
            }
            str = str + " order by ColorNameToC";
        }
        else
        {
            str = @"select Distinct vf.ColorId,vf.ColorName 
                    From V_FinishedItemDetail vf 
                    JOIN OrderDetail OD ON VF.Item_Finished_Id=OD.Item_Finished_Id 
                    --left join V_OrderPackDetailForStockOutPage od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id 
                    Where vf.ITEM_ID=" + ddItemName.SelectedValue + " and vf.MasterCompanyId=" + Session["varcompanyId"];
            //if (ChkForWithoutOrder.Checked == false)
            //{
            if (DDCustomerOrderNo.SelectedIndex > 0)
            {
                str = str + " and od.orderid=" + DDCustomerOrderNo.SelectedValue;
            }
            //}
            str = str + " order by ColorName";
        }
        UtilityModule.ConditionalComboFill(ref ddColor, str, true, "--Select--");
    }
    private void FillShape()
    {
        string Str = "";
        if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
        {
            Str = @"Select Distinct VF.ShapeId, VF.ShapeName 
                    From V_FinishedItemDetail VF";

            if (TDQuality.Visible == true)
            {
                Str = Str + " JOIN CustomerQuality CQ ON CQ.QualityID = VF.QualityID And CQ.CustomerID = " + DDCustomerCode.SelectedValue;
                if (ddQuality.SelectedIndex > 0)
                {
                    Str = Str + " And CQ.SrNo = " + ddQuality.SelectedValue;
                }
            }
            if (TDDesign.Visible == true)
            {
                Str = Str + " JOIN CustomerDesign CD ON CD.DesignID = VF.DesignID And CD.CQSRNO = CQ.SRNO And CD.CustomerID = " + DDCustomerCode.SelectedValue;
                if (ddDesign.SelectedIndex > 0)
                {
                    Str = Str + " And CD.SrNo = " + ddDesign.SelectedValue;
                }
            }
            if (TDColor.Visible == true)
            {
                Str = Str + " JOIN CustomerColor CC ON CC.ColorID = VF.ColorID And CC.CDSRNO = CD.SRNO And CC.CustomerID = " + DDCustomerCode.SelectedValue;
                if (ddColor.SelectedIndex > 0)
                {
                    Str = Str + " And CC.SrNo = " + ddColor.SelectedValue;
                }
            }
            //Str = Str + " LEFT JOIN V_OrderPackDetailForStockOutPage OD ON OD.Item_Finished_Id = VF.ITEM_FINISHED_ID ";
            Str = Str + " JOIN OrderDetail OD ON VF.Item_Finished_Id=OD.Item_Finished_Id ";
            Str = Str + " Where 1 = 1";
        }
        else
        {
            Str = @"Select Distinct VF.ShapeId, VF.ShapeName 
                From V_FinishedItemDetail VF 
                --LEFT JOIN V_OrderPackDetailForStockOutPage OD ON OD.Item_Finished_Id=VF.ITEM_FINISHED_ID 
                JOIN OrderDetail OD ON VF.Item_Finished_Id=OD.Item_Finished_Id
                Where VF.Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            if (TDQuality.Visible == true)
            {
                Str = Str + " And VF.QualityID = " + ddQuality.SelectedValue;
            }
            if (TDDesign.Visible == true)
            {
                Str = Str + " And VF.DesignID = " + ddDesign.SelectedValue;
            }
            if (TDColor.Visible == true)
            {
                Str = Str + " And VF.ColorID = " + ddColor.SelectedValue;
            }
        }

        //if (ChkForWithoutOrder.Checked != true)
        //{
        if (DDCustomerOrderNo.SelectedIndex > 0)
        {
            Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue + "";
        }
        // }
        UtilityModule.ConditionalComboFill(ref ddShape, Str, true, "--Select--");
    }
    protected void FillShadeColor()
    {
        //        string Str = @"Select Distinct VF.ShadeColorId, VF.ShadeColorName From V_OrderPackDetailForStockOutPage OD, V_FinishedItemDetail VF 
        //                Where OD.Item_Finished_Id=VF.Item_Finished_Id And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + @" And 
        //                VF.MasterCompanyId=" + Session["varCompanyId"] + "";

        string Str = @"Select Distinct VF.ShadeColorId, VF.ShadeColorName From OrderDetail OD, V_FinishedItemDetail VF 
                Where OD.Item_Finished_Id=VF.Item_Finished_Id And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + @" And 
                VF.MasterCompanyId=" + Session["varCompanyId"] + "";
        //if (ChkForWithoutOrder.Checked != true)
        //{
        if (DDCustomerOrderNo.SelectedIndex > 0)
        {
            Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue + "";
        }
        //}
        UtilityModule.ConditionalComboFill(ref ddShade, Str, true, "--Select--");
    }
    protected void ddShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShapeSelectedChange();
        // ddSize.Focus();
    }
    private void ShapeSelectedChange(int sizeid = 0)
    {

        //TxtWidth.Text = "";
        //TxtLength.Text = "";
        //TxtArea.Text = "";
        ////TxtPrice.Text = "";
        string Str = "", size = "SizeFt", custsize = "SizeNameAToC";

        //if (DDUnit.SelectedValue == "1")
        //{
        //    size = "SizeMtr";
        //    custsize = "MtSizeAToC";
        //}


        size = "SizeMtr";
        custsize = "MtSizeAToC";

        if (Session["varcompanyId"].ToString() == "30")
        {
            if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
            {
                Str = @"select Distinct VF.SizeId,Case when " + custsize + " is null Then " + size + @" Else " + custsize + @" End SizeName  
                from CustomerSize CS 
                inner join V_FinishedItemDetail vf on CS.SizeID=vf.SizeID And vf.item_id=" + ddItemName.SelectedValue + @" 
                JOIN OrderDetail OD ON VF.Item_Finished_Id=OD.Item_Finished_Id
                --left join V_OrderPackDetailForStockOutPage od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id
                ";

                if (ddDesign.SelectedIndex > 0)
                {
                    Str = Str + " JOIN CustomerDesign CD ON CD.DesignID = VF.DesignID And CD.Srno = " + ddDesign.SelectedValue;
                }

                if (ddColor.SelectedIndex > 0)
                {
                    Str = Str + " JOIN CustomerColor CC ON CC.ColorID = VF.ColorID And CC.Srno = " + ddColor.SelectedValue;
                }

                Str = Str + " Where CS.CustomerId=" + DDCustomerCode.SelectedValue + @" and vf.mastercompanyid=" + Session["varcompanyid"];
                //if (ChkForWithoutOrder.Checked == false)
                //{
                if (DDCustomerOrderNo.SelectedIndex > 0)
                {
                    Str = Str + " and od.orderid=" + DDCustomerOrderNo.SelectedValue;
                }
                //}
                Str = Str + " order by SizeName";
            }
            else
            {
                Str = @"select sizeid," + size + " as SIze From V_finisheditemdetail vf where vf.CATEGORY_ID=" + ddCategoryName.SelectedValue + " and vf.ITEM_ID=" + ddItemName.SelectedValue + "  and vf.ShapeId=" + ddShape.SelectedValue;
                if (sizeid > 0)
                {
                    Str = Str + " and  vf.sizeid=" + sizeid;
                }
            }
        }
        else
        {
            if (hnsampletype.Value == "1")
            {
                //                Str = @"Select Distinct VF.SizeId,Case when " + custsize + " is null Then " + size + @" Else " + custsize + @" End SizeName 
                //                  from V_OrderPackDetailForStockOutPage OD,V_FinishedItemDetail VF Left outer join CustomerSize CS on CS.Sizeid=Vf.Sizeid And CustomerId=" + DDCustomerCode.SelectedValue + @" Where OD.Item_Finished_Id=VF.Item_Finished_Id
                //                  And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + "  And VF.ShapeId=" + ddShape.SelectedValue + "   And VF.MasterCompanyId=" + Session["varCompanyId"] + "";

                Str = @"Select Distinct VF.SizeId,Case when " + custsize + " is null Then " + size + @" Else " + custsize + @" End SizeName 
                  from OrderDetail OD,V_FinishedItemDetail VF Left outer join CustomerSize CS on CS.Sizeid=Vf.Sizeid And CustomerId=" + DDCustomerCode.SelectedValue + @" Where OD.Item_Finished_Id=VF.Item_Finished_Id
                  And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + "  And VF.ShapeId=" + ddShape.SelectedValue + "   And VF.MasterCompanyId=" + Session["varCompanyId"] + "";


                //if (ChkForWithoutOrder.Checked != true)
                //{
                if (DDCustomerOrderNo.SelectedIndex > 0)
                {
                    if (DDCustomerOrderNo.SelectedIndex > 0)
                    {
                        Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue + "";
                    }
                }
                //}
            }
            else
            {
                Str = @"select distinct sizeid," + size + " as SIze From V_finisheditemdetail vf where vf.CATEGORY_ID=" + ddCategoryName.SelectedValue + " and vf.ITEM_ID=" + ddItemName.SelectedValue + " and vf.ShapeId=" + ddShape.SelectedValue;
                if (sizeid > 0)
                {
                    Str = Str + " and  vf.sizeid=" + sizeid;
                }
            }
        }
        UtilityModule.ConditionalComboFill(ref ddSize, Str, true, "--Select--");
    }
    protected void ddSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        SizeSelectedIndexChange();

    }
    private void SizeSelectedIndexChange()
    {

        //Fill_Price();
    }

    protected void ddDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        DesignSelectedChange();
    }
    private void DesignSelectedChange()
    {
        FillColor();
        //Fill_Price();
        ////ddColor.Focus();
    }
    protected void ddColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillShape();
        //Fill_Price();
        ShapeSelectedChange();
        //// ddShape.Focus();
    }

    protected void ddShade_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Fill_Price();
    }
    //private void Fill_Price()
    //{
    //    LblErrorMessage.Visible = false;
    //    LblErrorMessage.Text = "";
    //    int color = 0;
    //    int quality = 0;
    //    int design = 0;
    //    int shape = 0;
    //    int size = 0;
    //    int shadeColor = 0;
    //    if ((TDQuality.Visible == true && ddQuality.SelectedIndex > 0) || TDQuality.Visible != true)
    //    {
    //        quality = 1;
    //    }
    //    if (TDDesign.Visible == true && ddDesign.SelectedIndex > 0 || TDDesign.Visible != true)
    //    {
    //        design = 1;
    //    }
    //    if (TDColor.Visible == true && ddColor.SelectedIndex > 0 || TDColor.Visible != true)
    //    {
    //        color = 1;
    //    }
    //    if (TDShape.Visible == true && ddShape.SelectedIndex > 0 || TDShape.Visible != true)
    //    {
    //        shape = 1;
    //    }
    //    if (TDSize.Visible == true && ddSize.SelectedIndex > 0 || TDSize.Visible != true)
    //    {
    //        size = 1;
    //    }
    //    if (TDShade.Visible == true && ddShade.SelectedIndex > 0 || TDShade.Visible != true)
    //    {
    //        shadeColor = 1;
    //    }
    //    int VarQuality = 0, VarDesign = 0, VarColor = 0;
    //    //if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
    //    //{
    //    //if (TxtStockNo.Text != "")
    //    //{

    //    //    if (ddQuality.Visible == true)
    //    //    {
    //    //        VarQuality = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VF.QualityId from V_FinishedItemDetail VF, CarpetNumber CR where CR.Item_Finished_ID= VF.Item_Finished_Id AND TStockNo='" + TxtStockNo.Text + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + ""));
    //    //    }

    //    //    if (ddDesign.Visible == true)
    //    //    {
    //    //        VarDesign = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VF.DesignID from V_FinishedItemDetail VF, CarpetNumber CR where CR.Item_Finished_ID= VF.Item_Finished_Id AND TStockNo ='" + TxtStockNo.Text + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + ""));

    //    //    }

    //    //    if (ddColor.Visible == true)
    //    //    {
    //    //        VarColor = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VF.colorID from V_FinishedItemDetail VF, CarpetNumber CR where CR.Item_Finished_ID= VF.Item_Finished_Id AND TStockNo ='" + TxtStockNo.Text + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + ""));
    //    //    }
    //    //}
    //    //else
    //    //{
    //    VarQuality = ddQuality.Visible == true ? (ddQuality.SelectedIndex > 0 ? Convert.ToInt32(ddQuality.SelectedValue) : 0) : 0;
    //    VarDesign = ddDesign.Visible == true ? (ddDesign.SelectedIndex > 0 ? Convert.ToInt32(ddDesign.SelectedValue) : 0) : 0;
    //    VarColor = ddColor.Visible == true ? (ddColor.SelectedIndex > 0 ? Convert.ToInt32(ddColor.SelectedValue) : 0) : 0;

    //    //// VarDesign = ddDesign.Visible == true ? Convert.ToInt32(ddDesign.SelectedValue) : 0;
    //    ////VarColor = ddColor.Visible == true ? Convert.ToInt32(ddColor.SelectedValue) : 0;
    //    // }
    //    int VarShape = ddShape.Visible == true ? (ddShape.SelectedIndex > 0 ? Convert.ToInt32(ddShape.SelectedValue) : 0) : 0;
    //    int VarSize = ddSize.Visible == true ? (ddSize.SelectedIndex > 0 ? Convert.ToInt32(ddSize.SelectedValue) : 0) : 0;
    //    int VarShadeColor = ddShade.Visible == true ? (ddShade.SelectedIndex > 0 ? Convert.ToInt32(ddShade.SelectedValue) : 0) : 0;


    //    //////int VarShape = ddShape.Visible == true ? Convert.ToInt32(ddShape.SelectedValue) : 0;
    //    //////int VarSize = ddSize.Visible == true ? Convert.ToInt32(ddSize.SelectedValue) : 0;
    //    //////int VarShadeColor = ddShade.Visible == true ? Convert.ToInt32(ddShade.SelectedValue) : 0;
    //    int finishedid = 0;
    //    if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
    //    {
    //        finishedid = UtilityModule.getItemFinishedIdWithBuyercode(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, ddShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
    //    }
    //    else
    //    {
    //        finishedid = UtilityModule.getItemFinishedId(Convert.ToInt32(ddItemName.SelectedValue), VarQuality, VarDesign, VarColor, VarShape, VarSize, VarShadeColor, "", Convert.ToInt32(Session["varCompanyId"]));
    //    }
    //    if (finishedid > 0)
    //    {
    //        Fill_StockGrid(finishedid);
    //    }
    //    //}
    //}
    //    private void Fill_StockGrid(int VarFinishedID)
    //    {
    //        string Str = "";

    //        Str = @"SELECT CN.STOCKNO,CN.TSTOCKNO as TSTOCKNO,CN.PACK,CN.Item_Finished_Id FROM CARPETNUMBER CN INNER JOIN PROCESS_RECEIVE_DETAIL_1 PRD ON CN.PROCESS_REC_DETAIL_ID=PRD.PROCESS_REC_DETAIL_ID
    //                        WHERE CN.PACK=0 AND CN.ISSRECSTATUS=0 AND CN.COMPANYID=" + DDCompanyName.SelectedValue + " AND CN.ITEM_FINISHED_ID=" + VarFinishedID + @" AND PRD.QUALITYTYPE=1";

    //        if (DDCustomerOrderNo.SelectedIndex > 0)
    //        {
    //            Str = Str + " And CN.OrderId=" + DDCustomerOrderNo.SelectedValue;
    //        }
    //        Str = Str + "   UNION ";
    //        Str = Str + "  SELECT CN.STOCKNO,CN.TSTOCKNO as TSTOCKNO,CN.PACK,CN.Item_Finished_Id FROM CARPETNUMBER CN WHERE CN.PACK=0 AND CN.ISSRECSTATUS=0 AND CN.COMPANYID=" + DDCompanyName.SelectedValue + " AND CN.ITEM_FINISHED_ID=" + VarFinishedID + " AND CN.PROCESS_REC_DETAIL_ID=0";

    //        if (DDCustomerOrderNo.SelectedIndex > 0)
    //        {
    //            Str = Str + " And CN.OrderId=" + DDCustomerOrderNo.SelectedValue;
    //        }


    //        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
    //        DGStock.DataSource = ds;
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            DGStock.DataBind();
    //            DGStock.Visible = true;
    //            //for (int i = 0; i < DGStock.Rows.Count; i++)
    //            //{
    //            //    if (((TextBox)DGStock.Rows[i].FindControl("TxtPack")).Text == "1")
    //            //    {
    //            //        ((CheckBox)DGStock.Rows[i].FindControl("Chkbox")).Checked = true;
    //            //    }
    //            //    else
    //            //    {
    //            //        ((CheckBox)DGStock.Rows[i].FindControl("Chkbox")).Checked = false;
    //            //    }
    //            //}
    //        }
    //        else
    //        {
    //            DGStock.Visible = false;
    //            DGStock.DataSource = null;
    //            DGStock.DataBind();

    //        }
    //        //ForCheckAllRows();

    //    }
    //private void ForCheckAllRows()
    //{
    //    ////if (TxtPackQty.Text != "")
    //    ////{
    //    //    for (int i = 0; i < DGStock.Rows.Count; i++)
    //    //    {
    //    //        ((CheckBox)DGStock.Rows[i].FindControl("Chkbox")).Checked = false;
    //    //    }

    //    //    if (DGStock.Rows.Count >= Convert.ToInt32(TxtPackQty.Text))
    //    //    {
    //    //        for (int i = 0; i < Convert.ToInt32(TxtPackQty.Text); i++)
    //    //        {
    //    //            GridViewRow row = DGStock.Rows[i];
    //    //            ((CheckBox)row.FindControl("Chkbox")).Checked = true;
    //    //        }
    //    //    }
    //    //    else
    //    //    {
    //    //        LblErrorMessage.Visible = true;
    //    //        LblErrorMessage.Text = "Pls Enter Correct Value";
    //    //        if (ChkForEdit.Checked == false)
    //    //        {
    //    //            TxtPackQty.Text = "";
    //    //            TxtPackQty.Focus();
    //    //        }
    //    //    }
    //    ////}
    //}


    private void FillCarpetNo()
    {

        //        string Str = "";

        ////        Str = @"SELECT CN.STOCKNO,CN.TSTOCKNO as TSTOCKNO,CN.PACK,CN.Item_Finished_Id FROM CARPETNUMBER CN INNER JOIN PROCESS_RECEIVE_DETAIL_1 PRD ON CN.PROCESS_REC_DETAIL_ID=PRD.PROCESS_REC_DETAIL_ID
        ////                JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = CN.Item_Finished_Id 
        ////                WHERE CN.PACK=0 AND CN.ISSRECSTATUS=0 AND CN.COMPANYID=" + DDCompanyName.SelectedValue + @"  AND PRD.QUALITYTYPE=1";

        //        if (ddItemName.SelectedIndex > 0)
        //        {
        //            Str = Str + " And VF.Item_id =" + ddItemName.SelectedValue;
        //        }
        //        if (ddQuality.SelectedIndex > 0)
        //        {
        //            Str = Str + " And VF.QualityId =" + ddQuality.SelectedValue;
        //        }
        //        if (ddDesign.SelectedIndex > 0)
        //        {
        //            Str = Str + " And VF.DesignId =" + ddDesign.SelectedValue;
        //        }
        //        if (ddColor.SelectedIndex > 0)
        //        {
        //            Str = Str + " And VF.ColorId =" + ddColor.SelectedValue;
        //        }
        //        if (ddShape.SelectedIndex > 0)
        //        {
        //            Str = Str + " And VF.ShapeId =" + ddShape.SelectedValue;
        //        }
        //        if (ddSize.SelectedIndex > 0)
        //        {
        //            Str = Str + " And VF.SizeId =" + ddSize.SelectedValue;
        //        }
        //        ////if (DDCustomerOrderNo.SelectedIndex > 0)
        //        ////{
        //        ////    Str = Str + " And CN.OrderId=" + DDCustomerOrderNo.SelectedValue;
        //        ////}
        //        //Str = Str + "   UNION ";
        //        //Str = Str + "  SELECT CN.STOCKNO,CN.TSTOCKNO as TSTOCKNO,CN.PACK,CN.Item_Finished_Id FROM CARPETNUMBER CN JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = CN.Item_Finished_Id  WHERE CN.PACK=0 AND CN.ISSRECSTATUS=0 AND CN.COMPANYID=" + DDCompanyName.SelectedValue + "  AND CN.PROCESS_REC_DETAIL_ID=0";

        //        //if (ddItemName.SelectedIndex > 0)
        //        //{
        //        //    Str = Str + " And VF.Item_id =" + ddItemName.SelectedValue;
        //        //}
        //        //if (ddQuality.SelectedIndex > 0)
        //        //{
        //        //    Str = Str + " And VF.QualityId =" + ddQuality.SelectedValue;
        //        //}
        //        //if (ddDesign.SelectedIndex > 0)
        //        //{
        //        //    Str = Str + " And VF.DesignId =" + ddDesign.SelectedValue;
        //        //}
        //        //if (ddColor.SelectedIndex > 0)
        //        //{
        //        //    Str = Str + " And VF.ColorId =" + ddColor.SelectedValue;
        //        //}
        //        //if (ddShape.SelectedIndex > 0)
        //        //{
        //        //    Str = Str + " And VF.ShapeId =" + ddShape.SelectedValue;
        //        //}
        //        //if (ddSize.SelectedIndex > 0)
        //        //{
        //        //    Str = Str + " And VF.SizeId =" + ddSize.SelectedValue;
        //        //}
        //        ////if (DDCustomerOrderNo.SelectedIndex > 0)
        //        ////{
        //        ////    Str = Str + " And CN.OrderId=" + DDCustomerOrderNo.SelectedValue;
        //        ////}


        LblErrorMessage.Visible = false;
        LblErrorMessage.Text = "";
        int color = 0;
        int quality = 0;
        int design = 0;
        int shape = 0;
        int size = 0;
        int shadeColor = 0;
        if ((TDQuality.Visible == true && ddQuality.SelectedIndex > 0) || TDQuality.Visible != true)
        {
            quality = 1;
        }
        if (TDDesign.Visible == true && ddDesign.SelectedIndex > 0 || TDDesign.Visible != true)
        {
            design = 1;
        }
        if (TDColor.Visible == true && ddColor.SelectedIndex > 0 || TDColor.Visible != true)
        {
            color = 1;
        }
        if (TDShape.Visible == true && ddShape.SelectedIndex > 0 || TDShape.Visible != true)
        {
            shape = 1;
        }
        if (TDSize.Visible == true && ddSize.SelectedIndex > 0 || TDSize.Visible != true)
        {
            size = 1;
        }
        if (TDShade.Visible == true && ddShade.SelectedIndex > 0 || TDShade.Visible != true)
        {
            shadeColor = 1;
        }
        int VarQuality = 0, VarDesign = 0, VarColor = 0;
        if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
        {
            VarQuality = ddQuality.Visible == true ? (ddQuality.SelectedIndex > 0 ? Convert.ToInt32(ddQuality.SelectedValue) : 0) : 0;
            VarDesign = ddDesign.Visible == true ? (ddDesign.SelectedIndex > 0 ? Convert.ToInt32(ddDesign.SelectedValue) : 0) : 0;
            VarColor = ddColor.Visible == true ? (ddColor.SelectedIndex > 0 ? Convert.ToInt32(ddColor.SelectedValue) : 0) : 0;

            //// VarDesign = ddDesign.Visible == true ? Convert.ToInt32(ddDesign.SelectedValue) : 0;
            ////VarColor = ddColor.Visible == true ? Convert.ToInt32(ddColor.SelectedValue) : 0;
            // }
            int VarShape = ddShape.Visible == true ? (ddShape.SelectedIndex > 0 ? Convert.ToInt32(ddShape.SelectedValue) : 0) : 0;
            int VarSize = ddSize.Visible == true ? (ddSize.SelectedIndex > 0 ? Convert.ToInt32(ddSize.SelectedValue) : 0) : 0;
            int VarShadeColor = ddShade.Visible == true ? (ddShade.SelectedIndex > 0 ? Convert.ToInt32(ddShade.SelectedValue) : 0) : 0;


            //////int VarShape = ddShape.Visible == true ? Convert.ToInt32(ddShape.SelectedValue) : 0;
            //////int VarSize = ddSize.Visible == true ? Convert.ToInt32(ddSize.SelectedValue) : 0;
            //////int VarShadeColor = ddShade.Visible == true ? Convert.ToInt32(ddShade.SelectedValue) : 0;
            int finishedid = 0;
            if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
            {
                finishedid = UtilityModule.getItemFinishedIdWithBuyercode(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, ddShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            }
            else
            {
                finishedid = UtilityModule.getItemFinishedId(Convert.ToInt32(ddItemName.SelectedValue), VarQuality, VarDesign, VarColor, VarShape, VarSize, VarShadeColor, "", Convert.ToInt32(Session["varCompanyId"]));
            }

            //**********************************
            try
            {
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("Pro_GetCarpetNoForStockOut", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;

                cmd.Parameters.AddWithValue("@CompanyId", DDCompanyName.SelectedValue);
                cmd.Parameters.AddWithValue("@OrderId", DDCustomerOrderNo.SelectedValue);
                //cmd.Parameters.AddWithValue("@Where", Str);
                cmd.Parameters.AddWithValue("@Item_Finished_Id", finishedid);
                cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varcompanyId"]);
                cmd.Parameters.AddWithValue("@UserId", Session["varuserId"]);

                DataSet ds = new DataSet();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                cmd.ExecuteNonQuery();
                ad.Fill(ds);

                con.Close();
                con.Dispose();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DGStock.DataSource = ds.Tables[0];
                    DGStock.DataBind();
                    DGStock.Visible = true;
                }
                else
                {
                    DGStock.Visible = false;
                    DGStock.DataSource = null;
                    DGStock.DataBind();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                }
            }
            catch (Exception)
            {
            }
            finally
            {

            }

        }




        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        //DGStock.DataSource = ds;
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    DGStock.DataBind();
        //    DGStock.Visible = true;

        //}
        //else
        //{
        //    DGStock.Visible = false;
        //    DGStock.DataSource = null;
        //    DGStock.DataBind();

        //}
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "You Are Successfully LoggedOut..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void DDUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShapeSelectedChange();

        //if (Session["VarCompanyId"].ToString() == "30")
        //{
        //    ChangeRate();
        //}
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (LblErrorMessage.Text == "")
        {
            string Strdetail = "";

            for (int i = 0; i < DGStock.Rows.Count; i++)
            {
                CheckBox Chkboxitem = ((CheckBox)DGStock.Rows[i].FindControl("Chkboxitem"));
                Label lblTStockNo = ((Label)DGStock.Rows[i].FindControl("lblTStockNo"));
                Label lblStockNo = ((Label)DGStock.Rows[i].FindControl("lblStockNo"));
                Label lblItemFinishedId = ((Label)DGStock.Rows[i].FindControl("lblItemFinishedId"));

                if (Chkboxitem.Checked == true && (lblStockNo.Text != "") && DDCustomerCode.SelectedIndex > 0 && ddInvoiceNo.SelectedIndex > 0 && DDCustomerOrderNo.SelectedIndex > 0)
                {
                    Strdetail = Strdetail + lblTStockNo.Text + '|' + lblStockNo.Text + '|' + lblItemFinishedId.Text + '~';
                }
            }

            if (Strdetail != "")
            {
                LblErrorMessage.Text = "";

                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlTransaction Tran = con.BeginTransaction();
                try
                {                    
                    SqlCommand cmd = new SqlCommand("Pro_SaveStockOutCarpetByPackingInvoice", con, Tran);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 3000;
                    cmd.Parameters.AddWithValue("@CompanyId", DDCompanyName.SelectedValue);
                    cmd.Parameters.AddWithValue("@CustomerId", DDCustomerCode.SelectedValue);
                    cmd.Parameters.AddWithValue("@PackingId",ddInvoiceNo.SelectedValue);
                    cmd.Parameters.AddWithValue("@OrderId", DDCustomerOrderNo.SelectedValue);
                    cmd.Parameters.AddWithValue("@StringDetail", Strdetail);
                    cmd.Parameters.AddWithValue("@Mastercompanyid", Session["varcompanyid"]);
                    cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
                    cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 300);
                    cmd.Parameters["@Msg"].Direction = ParameterDirection.Output; 

                    cmd.ExecuteNonQuery();
                    if (cmd.Parameters["@Msg"].Value.ToString() != "") //IF DATA NOT SAVED
                    {
                        LblErrorMessage.Visible = true;
                        LblErrorMessage.Text = cmd.Parameters["@Msg"].Value.ToString();
                        Tran.Rollback();
                    }
                    else
                    {
                        BindCarpetNumber();
                        LblErrorMessage.Visible = true;
                        LblErrorMessage.Text = "Data Saved Successfully.";
                        Tran.Commit();
                        
                    }


                }
                catch (Exception ex)
                {
                    LblErrorMessage.Text = ex.Message;
                    Tran.Rollback();
                }
                finally
                {
                    con.Dispose();
                    con.Close();
                }




                //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                //if (con.State == ConnectionState.Closed)
                //{
                //    con.Open();
                //}
                //SqlTransaction Tran = con.BeginTransaction();
                //try
                //{
                //    SqlParameter[] array = new SqlParameter[8];
                //    array[0] = new SqlParameter("@CompanyId", DDCompanyName.SelectedValue);
                //    array[1] = new SqlParameter("@CustomerId", DDCustomerCode.SelectedValue);
                //    array[2] = new SqlParameter("@PackingId", ddInvoiceNo.SelectedValue);
                //    array[3] = new SqlParameter("@OrderId", DDCustomerOrderNo.SelectedValue);
                //    array[4] = new SqlParameter("@StringDetail", Strdetail);
                //    array[5] = new SqlParameter("@MasterCompanyId", Session["varCompanyId"]);
                //    array[6] = new SqlParameter("@UserID", Session["varUserId"]);
                //    array[7] = new SqlParameter("@Msg", SqlDbType.VarChar, 300);
                //    array[7].Direction = ParameterDirection.Output;

                //    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_SaveStockOutCarpetByPackingInvoice]", array);
                //    Tran.Commit();

                //    if (array[7].Value.ToString() != "")
                //    {
                //        LblErrorMessage.Visible = true;
                //        LblErrorMessage.Text = array[7].Value.ToString();
                //    }
                   

                //    //Fill_Grid();
                //}
                //catch (Exception ex)
                //{
                //    LblErrorMessage.Text = ex.Message;
                //    Tran.Rollback();
                //}
                //finally
                //{
                //    con.Dispose();
                //    con.Close();
                //}
            }


        }

    }
    private void CHECKVALIDCONTROL()
    {
        LblErrorMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompanyName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerCode) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddInvoiceNo) == false)
        {
            goto a;
        }
        
        if (TxtStockNo.Text == "")
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddCategoryName) == false)
            {
                goto a;
            }
            if (UtilityModule.VALIDDROPDOWNLIST(ddItemName) == false)
            {
                goto a;

            }
            if (ddQuality.Visible == true)
            {
                if (UtilityModule.VALIDDROPDOWNLIST(ddQuality) == false)
                {
                    goto a;
                }
            }
            if (ddDesign.Visible == true)
            {
                if (UtilityModule.VALIDDROPDOWNLIST(ddDesign) == false)
                {
                    goto a;
                }
            }
            if (ddColor.Visible == true)
            {
                if (UtilityModule.VALIDDROPDOWNLIST(ddColor) == false)
                {
                    goto a;
                }
            }
            if (ddShape.Visible == true)
            {
                if (UtilityModule.VALIDDROPDOWNLIST(ddShape) == false)
                {
                    goto a;
                }
            }
            if (ddSize.Visible == true)
            {
                if (UtilityModule.VALIDDROPDOWNLIST(ddSize) == false)
                {
                    goto a;
                }
            }
            if (ddShade.Visible == true)
            {
                if (UtilityModule.VALIDDROPDOWNLIST(ddShade) == false)
                {
                    goto a;
                }
            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerOrderNo) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    //if (ChkForWithoutOrder.Checked == false)
    //{
    //    if (UtilityModule.VALIDTEXTBOX(TxtTotalQty) == false)
    //    {
    //        goto a;
    //    }
    //}
    //if (UtilityModule.VALIDTEXTBOX(TxtPackQty) == false)
    //{
    //    goto a;
    //}
    //else
    //{
    //    goto B;
    //}
    a:
        LblErrorMessage.Visible = true;
        UtilityModule.SHOWMSG(LblErrorMessage);
    B: ;
    }
    private void Save_Referce()
    {
        //TxtStockNo.Text = "";
        if (Session["varcompanyno"].ToString() != "30")
        {
            if (ddCategoryName.Items.Count > 0)
            {
                ddCategoryName.SelectedIndex = 0;
                Category_SelectedIndex_Change();
                ItemNameSelectedIndexChange();
                //DDSubQuality.Items.Clear();
            }
            if (DDCustomerOrderNo.Items.Count > 0)
            {
                DDCustomerOrderNo.SelectedIndex = 0;
            }
        }

        ShapeSelectedChange();
        if (ddSize.Items.Count == 0)
        {
            ComboFill();
        }
        hnsampletype.Value = "1";
    }


    protected void ddInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {

        FillorderNo();
    }

    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        LblErrorMessage.Visible = false;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select distinct CATEGORY_ID,v.ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId from V_FinishedItemDetail v ,orderdetail od Where OD.Item_Finished_Id=V.Item_Finished_Id and od.orderid=" + DDCustomerOrderNo.SelectedValue + " and ProductCode='" + TxtProdCode.Text + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ddCategoryName.Items.Count > 0)
            {
                ddCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                Category_SelectedIndex_Change();
            }
            if (ddItemName.Items.Count > 0)
            {
                ddItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                ItemNameSelectedIndexChange();
            }
            if (ddQuality.Items.Count > 0 && ddQuality.Visible == true)
            {
                ddQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
                ComboFill();
                //Fill_Price();
            }
            if (ddDesign.Items.Count > 0 && ddDesign.Visible == true)
            {
                ddDesign.SelectedValue = ds.Tables[0].Rows[0]["designId"].ToString();
                DesignSelectedChange();
            }
            if (ddColor.Items.Count > 0 && ddColor.Visible == true)
            {
                ddColor.SelectedValue = ds.Tables[0].Rows[0]["ColorId"].ToString();
                //Fill_Price();
            }
            if (ddShape.Items.Count > 0 && ddShape.Visible == true)
            {
                ddShape.SelectedValue = ds.Tables[0].Rows[0]["ShapeId"].ToString();
                ShapeSelectedChange();
            }
            if (ddSize.Items.Count > 0 && ddSize.Visible == true)
            {
                ddSize.SelectedValue = ds.Tables[0].Rows[0]["SizeId"].ToString();
                SizeSelectedIndexChange();
            }
            if (ddShade.Items.Count > 0 && ddShade.Visible == true)
            {
                ddShade.SelectedValue = ds.Tables[0].Rows[0]["ShadecolorId"].ToString();
                //Fill_Price();
            }
        }
        else
        {
            LblErrorMessage.Text = "Product Code Does Not Exist";
            LblErrorMessage.Visible = true;
        }
    }

    public string getStockNo(string strVal, string strval1)
    {
        string val = "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select strcarpets From [dbo].[GetTStockNosPack](" + strVal + ",0)");
        val = ds.Tables[0].Rows[0]["strcarpets"].ToString();
        return val;
    }


    private void HookOnFocus(Control CurrentControl)
    {
        //checks if control is one of TextBox, DropDownList, ListBox or Button
        if ((CurrentControl is TextBox) ||
            (CurrentControl is DropDownList) ||
            (CurrentControl is ListBox) ||
            (CurrentControl is Button))
            //adds a script which saves active control on receiving focus in the hidden field __LASTFOCUS.
            (CurrentControl as WebControl).Attributes.Add(
                "onfocus",
                "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");

        //checks if the control has children
        if (CurrentControl.HasControls())
            //if yes do them all recursively
            foreach (Control CurrentChildControl in CurrentControl.Controls)
                HookOnFocus(CurrentChildControl);
    }
    protected void lnkchnginvoice_Click(object sender, EventArgs e)
    {
        //LblErrorMessage.Visible = false;
        //LblErrorMessage.Text = "";
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}
        //SqlTransaction Tran = con.BeginTransaction();
        //try
        //{
        //    SqlParameter[] param = new SqlParameter[4];
        //    param[0] = new SqlParameter("@Invoiceid", ddInvoiceNo.SelectedValue);
        //    param[1] = new SqlParameter("@InvoiceNo", SqlDbType.VarChar, 100);
        //    param[1].Direction = ParameterDirection.InputOutput;
        //    param[1].Value = TxtInvoiceNo.Text;
        //    param[2] = new SqlParameter("@Invoicedate", SqlDbType.VarChar, 50);
        //    param[2].Direction = ParameterDirection.InputOutput;
        //    param[2].Value = TxtDate.Text;
        //    param[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
        //    param[3].Direction = ParameterDirection.Output;
        //    //*************
        //    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_CHANGEINVOICENODATE", param);
        //    Tran.Commit();
        //    LblErrorMessage.Visible = true;
        //    LblErrorMessage.Text = param[3].Value.ToString();

        //}
        //catch (Exception ex)
        //{
        //    Tran.Commit();
        //    LblErrorMessage.Visible = true;
        //    LblErrorMessage.Text = ex.Message;
        //}
        //finally
        //{
        //    con.Close();
        //    con.Dispose();
        //}
    }
    protected void BindCarpetNumber()
    {
        LblErrorMessage.Visible = false;
        LblErrorMessage.Text = "";
        if (ddItemName.SelectedIndex > 0 && ddQuality.SelectedIndex > 0 && ddDesign.SelectedIndex > 0 && ddColor.SelectedIndex > 0 && ddShape.SelectedIndex > 0 && ddSize.SelectedIndex > 0)
        {
            FillCarpetNo();
        }
        else
        {
            DGStock.Visible = false;
            DGStock.DataSource = null;
            DGStock.DataBind();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Please select all mandatory fields";
        }
    }
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        BindCarpetNumber();
    }
    //protected void saveDetail()
    //{
       
    //}
    protected void TxtStockNo_TextChanged(object sender, EventArgs e)
    {
        if (DDCustomerCode.SelectedIndex > 0 && ddInvoiceNo.SelectedIndex > 0 && DDCustomerOrderNo.SelectedIndex > 0 && TxtStockNo.Text!="")
        {
            LblErrorMessage.Text = "";
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@CompanyId", DDCompanyName.SelectedValue);
                param[1] = new SqlParameter("@PackingId", ddInvoiceNo.SelectedValue);
                param[2] = new SqlParameter("@OrderId", DDCustomerOrderNo.SelectedValue);
                param[3] = new SqlParameter("@Tstockno", TxtStockNo.Text);
                param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[4].Direction = ParameterDirection.Output;

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_CheckPackingStatusWithStockNo", param);
                if (param[4].Value.ToString() != "")
                {
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = param[4].Value.ToString();
                    DGStock.DataSource = null;
                    DGStock.DataBind();
                }
                else
                {
                    DGStock.DataSource = ds.Tables[0];
                    DGStock.DataBind();
                    for (int i = 0; i < DGStock.Rows.Count; i++)
                    {
                        CheckBox Chkboxitem = (CheckBox)DGStock.Rows[i].FindControl("Chkboxitem");
                        //TextBox txtloomqty = (TextBox)DG.Rows[i].FindControl("txtloomqty");
                        Chkboxitem.Checked = true;
                        //txtloomqty.Text = "1";
                    }

                    BtnSave_Click(sender, new EventArgs());
                    DGStock.DataSource = null;
                    DGStock.DataBind();
                    TxtStockNo.Text = "";
                }
                TxtStockNo.Focus();
            }
            catch (Exception ex)
            {
                LblErrorMessage.Text = ex.Message;
            }
        }
        else
        {
            DGStock.Visible = false;
            DGStock.DataSource = null;
            DGStock.DataBind();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Please select Invoice no, customer orderno";
        }
        
    }
}