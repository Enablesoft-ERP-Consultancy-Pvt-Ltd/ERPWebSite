using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Packing_frmpackingForHomefurn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string Qry = @" Select Distinct CI.CompanyId, CI.CompanyName 
                    From Companyinfo CI(nolock)
                    JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                    Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName 
                    Select CustomerId,CustomerCode + SPACE(5)+CompanyName From CustomerInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by CustomerCode
                    select Distinct Unitid,UnitName from Unit order by UnitId ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Qry);
            UtilityModule.ConditionalComboFillWithDS(ref DDConsignor, ds, 0, false, "");
            if (DDConsignor.Items.Count > 0)
            {
                DDConsignor.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDConsignor.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDConsignee, ds, 1, true, "--select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDunit, ds, 2, true, "--select--");

            txtInvoiceDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            RDPcsWise.Checked = true;
            ViewState["PackingId"] = "0";
        }
    }
    protected void DDConsignee_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select OM.orderid,OM.customerorderNo from ordermaster OM where OM.customerid=" + DDConsignee.SelectedValue + @" and OM.Status=0 order by  OM.customerorderno
                      select ci.CurrencyId,ci.CurrencyName from currencyinfo Ci inner join customerinfo CC on Ci.CurrencyId=CC.CurrencyId where CC.customerid=" + DDConsignee.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        UtilityModule.ConditionalComboFillWithDS(ref DDOrderNo, ds, 0, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDCurrency, ds, 1, false, "");
        ViewState["PackingId"] = "0";
        if (Chkedit.Checked == true)
        {
            str = "select packingid,TinvoiceNo from Packing Where consigneeid=" + DDConsignee.SelectedValue + " order by packingid";
            UtilityModule.ConditionalComboFill(ref DDinvoiceNo, str, true, "--Select--");
        }

    }
    protected void chkwithoutorder_CheckedChanged(object sender, EventArgs e)
    {
        FillCategory();
    }
    protected void FillCategory()
    {
        string str = null;
        if (chkwithoutorder.Checked == true)
        {
            DDOrderNo.SelectedIndex = -1;
            str = @"select ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner join CategorySeparate CS 
                  on ICM.CATEGORY_ID=CS.Categoryid and cs.id=0 and ICM.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by ICM.CATEGORY_NAME";
        }
        else
        {

            str = @"select distinct vf.CATEGORY_ID,vf.CATEGORY_NAME from OrderDetail OD inner join V_FinishedItemDetail vf on OD.Item_Finished_Id=vf.ITEM_FINISHED_ID
                  where OD.orderid=" + DDOrderNo.SelectedValue + " order by vf.CATEGORY_NAME";
        }
        UtilityModule.ConditionalComboFill(ref DDCategoryName, str, true, "--Select--");
    }

    protected void FillItemName()
    {
        string str = null;
        if (chkwithoutorder.Checked == true)
        {
            str = "select Item_Id,ITEM_NAME from Item_master Where CATEGORY_ID=" + DDCategoryName.SelectedValue + " order by ITEM_NAME";
        }
        else
        {
            str = @"select distinct vf.ITEM_ID,vf.item_name from OrderDetail OD inner join V_FinishedItemDetail vf on OD.Item_Finished_Id=vf.ITEM_FINISHED_ID
                  where OD.orderid=" + DDOrderNo.SelectedValue + " and vf.Category_id=" + DDCategoryName.SelectedValue + " order by vf.Item_NAME";
        }
        UtilityModule.ConditionalComboFill(ref DDItemName, str, true, "--Select--");
    }
    protected void FillCombo()
    {
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
        TDShade.Visible = false;
        TDSize.Visible = false;
        TDShade.Visible = false;

        string str = @"select PARAMETER_ID from ITEM_CATEGORY_PARAMETERS where category_id=" + DDCategoryName.SelectedValue;
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
    protected void DDCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillItemName();
        FillCombo();
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQDCS();
    }
    protected void FillQDCS()
    {
        string str = null;
        //Quality
        if (TDQuality.Visible == true)
        {
            if (chkwithoutorder.Checked == true)
            {
                str = "select Distinct QualityId,QualityName from Quality Where Item_Id=" + DDItemName.SelectedValue + " order by QualityName";
            }
            else
            {
                str = @"select distinct vf.qualityid,vf.QualityName from OrderDetail OD inner join V_FinishedItemDetail vf on OD.Item_Finished_Id=vf.ITEM_FINISHED_ID
                       Where  OD.orderid=" + DDOrderNo.SelectedValue + " and vf.Item_Id=" + DDItemName.SelectedValue + " order by vf.QualityName";
            }
            UtilityModule.ConditionalComboFill(ref DDQuality, str, true, "--Select--");

        }
        //Design
        if (TDDesign.Visible == true)
        {
            if (chkwithoutorder.Checked == true)
            {
                str = "select Distinct designId,designName from V_FinishedItemDetail vf  Where Item_Id=" + DDItemName.SelectedValue + " order by designname";
            }
            else
            {
                str = @"select distinct vf.DesignId,vf.DesignName from OrderDetail OD inner join V_FinishedItemDetail vf on OD.Item_Finished_Id=vf.ITEM_FINISHED_ID
                       Where  OD.orderid=" + DDOrderNo.SelectedValue + " and vf.Item_Id=" + DDItemName.SelectedValue + " order by vf.DesignName";
            }
            UtilityModule.ConditionalComboFill(ref DDDesign, str, true, "--Select--");

        }
        //Color
        if (TDColor.Visible == true)
        {
            if (chkwithoutorder.Checked == true)
            {
                str = "select Distinct colorid,colorname from V_FinishedItemDetail vf  Where Item_Id=" + DDItemName.SelectedValue + " order by Colorname";
            }
            else
            {
                str = @"select distinct vf.Colorid,vf.ColorName from OrderDetail OD inner join V_FinishedItemDetail vf on OD.Item_Finished_Id=vf.ITEM_FINISHED_ID
                       Where  OD.orderid=" + DDOrderNo.SelectedValue + " and vf.Item_Id=" + DDItemName.SelectedValue + " order by vf.ColorName";
            }
            UtilityModule.ConditionalComboFill(ref DDColor, str, true, "--Select--");

        }
        //Shape
        if (TDShape.Visible == true)
        {
            if (chkwithoutorder.Checked == true)
            {
                str = "select Distinct shapeid,shapename from V_FinishedItemDetail vf  Where Item_Id=" + DDItemName.SelectedValue + " order by shapename";
            }
            else
            {
                str = @"select distinct vf.shapeid,vf.shapename from OrderDetail OD inner join V_FinishedItemDetail vf on OD.Item_Finished_Id=vf.ITEM_FINISHED_ID
                       Where  OD.orderid=" + DDOrderNo.SelectedValue + " and vf.Item_Id=" + DDItemName.SelectedValue + " order by vf.shapeName";
            }
            UtilityModule.ConditionalComboFill(ref DDShape, str, true, "--Select--");

        }
        //Shade
        if (TDShade.Visible == true)
        {
            if (chkwithoutorder.Checked == true)
            {
                str = "select shadecolorid,shadecolorname from V_FinishedItemDetail vf  Where Item_Id=" + DDItemName.SelectedValue + " order by shadename";
            }
            else
            {
                str = @"select distinct vf.shadecolorid,vf.shadecolornamefrom OrderDetail OD inner join V_FinishedItemDetail vf on OD.Item_Finished_Id=vf.ITEM_FINISHED_ID
                       Where  OD.orderid=" + DDOrderNo.SelectedValue + " and vf.Item_Id=" + DDItemName.SelectedValue + " order by vf.shadeName";
            }
            UtilityModule.ConditionalComboFill(ref DDShade, str, true, "--Select--");

        }
        //Unit



    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
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
        if (chkwithoutorder.Checked == true)
        {
            str = "select Distinct sizeid," + size + " from V_FinishedItemDetail vf Where ITEM_ID=" + DDItemName.SelectedValue + " and vf.shapeid=" + DDShape.SelectedValue + " order by " + size;
        }
        else
        {
            str = @"select distinct vf.sizeid," + size + @" from OrderDetail OD inner join V_FinishedItemDetail vf on OD.Item_Finished_Id=vf.ITEM_FINISHED_ID
                    where vf.item_id=" + DDItemName.SelectedValue + " and vf.shapeid=" + DDShape.SelectedValue + " order by " + size;

        }
        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--Select--");

    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategory();
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
            SqlParameter[] arr = new SqlParameter[36];
            arr[0] = new SqlParameter("@PackingId", SqlDbType.Int);
            arr[1] = new SqlParameter("@CompanyId", SqlDbType.Int);
            arr[2] = new SqlParameter("@Customerid", SqlDbType.Int);
            arr[3] = new SqlParameter("@TInvoiceNo", SqlDbType.VarChar, 50);
            arr[4] = new SqlParameter("@packingDate", SqlDbType.DateTime);
            arr[5] = new SqlParameter("@CurrencyId", SqlDbType.Int);
            arr[6] = new SqlParameter("@unitid", SqlDbType.Int);
            arr[7] = new SqlParameter("@InvoiceYear", SqlDbType.Int);
            arr[8] = new SqlParameter("@Rollno", SqlDbType.Int);
            arr[9] = new SqlParameter("@Finishedid", SqlDbType.Int);
            arr[10] = new SqlParameter("@Orderid", SqlDbType.Int);
            arr[11] = new SqlParameter("@PackingDetailid", SqlDbType.Int);// output,
            arr[12] = new SqlParameter("@RollFrom", SqlDbType.Int);
            arr[13] = new SqlParameter("@RollTo", SqlDbType.Int);
            arr[14] = new SqlParameter("@Rpcs", SqlDbType.Int);
            arr[15] = new SqlParameter("@Totalpcs", SqlDbType.Int);
            arr[16] = new SqlParameter("@TotalRoll", SqlDbType.Int);
            arr[17] = new SqlParameter("@CalTypeAmt", SqlDbType.Int);
            arr[18] = new SqlParameter("@Netwtperpcs", SqlDbType.Float);
            arr[19] = new SqlParameter("@NetWtFabric", SqlDbType.Float);
            arr[20] = new SqlParameter("@NetWtBeads", SqlDbType.Float);
            arr[21] = new SqlParameter("@TotalnetwtFabric", SqlDbType.Float);
            arr[22] = new SqlParameter("@TotalnetwtBeads", SqlDbType.Float);
            arr[23] = new SqlParameter("@Totalnetwt", SqlDbType.Float);
            arr[24] = new SqlParameter("@CrtnWt", SqlDbType.Float);
            arr[25] = new SqlParameter("@TotalGrossWt", SqlDbType.Float);
            arr[26] = new SqlParameter("@userid", SqlDbType.Int);
            arr[27] = new SqlParameter("@Mastercompanyid", SqlDbType.Int);
            arr[28] = new SqlParameter("@msg", SqlDbType.VarChar, 100);// output
            arr[29] = new SqlParameter("@Sizeflag", SqlDbType.TinyInt);
            arr[30] = new SqlParameter("@Barcode", SqlDbType.VarChar, 50);
            arr[31] = new SqlParameter("@Description", SqlDbType.VarChar, 100);
            arr[32] = new SqlParameter("@L", SqlDbType.VarChar, 10);  //Length
            arr[33] = new SqlParameter("@W", SqlDbType.VarChar, 10);
            arr[34] = new SqlParameter("@H", SqlDbType.VarChar, 10);
            arr[35] = new SqlParameter("@CBM", SqlDbType.Float);

            //************Passing values
            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = ViewState["PackingId"];
            arr[1].Value = DDConsignor.SelectedValue;
            arr[2].Value = DDConsignee.SelectedValue;
            arr[3].Value = txtInvoiceno.Text;
            arr[4].Value = txtInvoiceDate.Text;
            arr[5].Value = DDCurrency.SelectedValue;
            arr[6].Value = DDunit.SelectedValue;
            string VarInvoiceMonth = DateTime.Now.ToString("MM");
            string VarInvoiceYear;
            if (Convert.ToInt32(VarInvoiceMonth) > 04)
            {
                VarInvoiceYear = DateTime.Now.ToString("yyyy");
            }
            else
            {
                VarInvoiceYear = DateTime.Now.ToString("yyyy");
                VarInvoiceYear = (Convert.ToInt32(VarInvoiceYear) - 1).ToString();
            }
            arr[7].Value = VarInvoiceYear;
            arr[8].Value = txtcrtnFrom.Text;
            int varfinishedid = UtilityModule.getItemFinishedId(DDItemName, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, Tran, DDShade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            arr[9].Value = varfinishedid;
            arr[10].Value = chkwithoutorder.Checked == true ? "0" : DDOrderNo.SelectedValue;
            arr[11].Value = ParameterDirection.InputOutput;
            arr[11].Value = 0;  //PackingDetailid
            arr[12].Value = txtcrtnFrom.Text;
            arr[13].Value = txtcrtnTo.Text;
            arr[14].Value = txtpcspercrnt.Text;
            arr[15].Value = txttotalpcs.Text;
            arr[16].Value = txttotalcrnt.Text;
            arr[17].Value = RDAreaWise.Checked == true ? 0 : 1;
            arr[18].Value = txtnetwtperpcs.Text == "" ? "0" : txtnetwtperpcs.Text;
            arr[19].Value = txtnetwtfabric.Text == "" ? "0" : txtnetwtfabric.Text;
            arr[20].Value = txtnetwtbeads.Text == "" ? "0" : txtnetwtbeads.Text;
            arr[21].Value = txttotalnetwtfabric.Text == "" ? "0" : txttotalnetwtfabric.Text;
            arr[22].Value = txttotalnetwtbeads.Text == "" ? "0" : txttotalnetwtbeads.Text;
            arr[23].Value = txttotalnetwt.Text == "" ? "0" : txttotalnetwt.Text;
            arr[24].Value = txtcrtnwt.Text == "" ? "0" : txtcrtnwt.Text;
            arr[25].Value = txtTgrwt.Text == "" ? "0" : txtTgrwt.Text;
            arr[26].Value = Session["varuserid"];
            arr[27].Value = Session["varMasterCompanyIDForERP"];
            arr[28].Direction = ParameterDirection.Output;
            arr[29].Value = DDsizetype.SelectedValue;
            arr[30].Value = txtbarcode.Text;
            arr[31].Value = txtdescription.Text;
            arr[32].Value = txtLength.Text;
            arr[33].Value = txtWidth.Text;
            arr[34].Value = txtHeight.Text;
            arr[35].Value = txtcbm.Text == "" ? "0" : txtcbm.Text;
            //
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_savepackinginformationForHomeFurn", arr);
            ViewState["PackingId"] = arr[0].Value.ToString();
            if (arr[28].Value.ToString() != "")
            {
                lblmessage.Text = arr[28].Value.ToString();
            }
            else
            {
                lblmessage.Text = "Data saved successfully......";
            }
            Tran.Commit();
            Refreshcontrol();
            FillGrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void Refreshcontrol()
    {
        txtcrtnFrom.Text = "";
        txtcrtnTo.Text = "";
        txttotalcrnt.Text = "";
        txtpcspercrnt.Text = "";
        txttotalpcs.Text = "";
        DDQuality.SelectedIndex = -1;
        DDDesign.SelectedIndex = -1;
        DDColor.SelectedIndex = -1;
        DDShape.SelectedIndex = -1;
        DDSize.SelectedIndex = -1;
        txtnetwtperpcs.Text = "";
        txtnetwtfabric.Text = "";
        txtnetwtbeads.Text = "";
        txttotalnetwtfabric.Text = "";
        txttotalnetwtbeads.Text = "";
        txttotalnetwt.Text = "";
        txtcrtnwt.Text = "";
        txtTgrwt.Text = "";
        txtbarcode.Text = "";
        txtdescription.Text = "";
        txtLength.Text = "";
        txtWidth.Text = "";
        txtHeight.Text = "";
        txtcbm.Text = "";
    }
    protected void FillGrid()
    {
        string str = @"select * from V_GetPackingDetail Where Packingid=" + ViewState["PackingId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GVDetail.DataSource = ds.Tables[0];
        GVDetail.DataBind();
        if (Chkedit.Checked == true)
        {
            if (DDinvoiceNo.SelectedIndex > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtInvoiceno.Text = ds.Tables[0].Rows[0]["TinvoiceNo"].ToString();
                    txtInvoiceDate.Text = ds.Tables[0].Rows[0]["PackingDate"].ToString();
                    DDCurrency.SelectedValue = ds.Tables[0].Rows[0]["Currencyid"].ToString();
                    DDunit.SelectedValue = ds.Tables[0].Rows[0]["Unitid"].ToString();
                }
            }
        }
    }
    protected void Chkedit_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkedit.Checked == true)
        {
            TDinvoiceNo.Visible = true;

        }
        else
        {
            TDinvoiceNo.Visible = false;
            DDConsignee.SelectedIndex = -1;
            DDinvoiceNo.SelectedIndex = -1;
        }
    }
    protected void DDinvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["PackingId"] = DDinvoiceNo.SelectedValue;
        FillGrid();
    }
    protected void GVDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[1];
            arr[0] = new SqlParameter("@packingDetailId", SqlDbType.Int);
            Label lblpackingDetailId = ((Label)GVDetail.Rows[e.RowIndex].FindControl("lblPackingDetailId"));
            arr[0].Value = lblpackingDetailId.Text;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeletepackingForHomeFurn", arr);
            lblmessage.Text = "Data Deleted successfully....";
            Tran.Commit();
            FillGrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
}