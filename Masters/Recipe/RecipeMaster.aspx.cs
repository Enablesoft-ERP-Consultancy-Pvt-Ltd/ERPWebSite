using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ClosedXML.Excel;
using System.Configuration;
using System.Text;

public partial class Master_Recipe_RecipeMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
         if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        //DataRow dr = AllEnums.MasterTables.Mastersetting.ToTable().Select()[0];
        //WithBuyerCode = dr["WithBuyerCode"].ToString();
         lblresdate.Text = DateTime.Today.ToShortDateString();
         if (!IsPostBack)
         {
             UtilityModule.ConditionalComboFill(ref ddlcompany, "select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + " Order By CompanyName", true, "--SelectCompany");

             if (ddlcompany.Items.Count > 0)
             {
                 ddlcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                 ddlcompany.Enabled = false;
             }
             string str = @"select Customerid,Customercode From Customerinfo Where MasterCompanyId=" + Session["varCompanyId"] + @" order by Customercode
                         SELECT CATEGORY_ID,CATEGORY_NAME FROM ITEM_CATEGORY_MASTER ICM Where category_name='RAW MATERIAL' And ICM.MasterCompanyId=" + Session["varCompanyId"] + @" 
                         select val,Type from SizeType Order by val
                         select WithBuyerCode from Mastersetting;";

             DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
             UtilityModule.ConditionalComboFillWithDS(ref ddBuyerCode, ds, 0, true, "--Select--");
             UtilityModule.ConditionalComboFillWithDS(ref ddlcategory, ds, 1, true, "--Select--");
             if (ddlcategory.Items.Count > 0)
             {
                 ddlcategory.SelectedIndex = 1;
                 ddlcategory.Enabled = false;
             }
             UtilityModule.ConditionalComboFill(ref ddItemname, "select item_id,item_name from item_master where category_id= "+ddlcategory.SelectedValue+" And MasterCompanyId=" + Session["varCompanyId"] + " Order By item_name", true, "--Select--");
            
         }

    }
    protected void ddItemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillquality();
      //  fillRecipe();
       // fillChemical();
        //fill_size();
        //Fill_GridSize();
    }
    protected void ddBuyerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        //fill_grid();
     //   Fill_GridSize();
        //btnrpt.Enabled = true;
        //Session["ReportPath"] = "RptBuyerMasterCode.rpt";
        //Session["CommanFormula"] = "{Customerinfo.CompanyName}='" + ddBuyerCode.SelectedItem.Text + "'";
    }
    protected void ddLocalQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
      //  filldesign();
        fillcolor();
    }
    protected void ddldesign_SelectedIndexChanged(object sender, EventArgs e)
    {
      
    }
    protected void ddlcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dddyingtype, "select calid,CalType from Process_CalType", true, "--Select Type--");
        UtilityModule.ConditionalComboFill(ref dddyesstuff, "select item_id,item_name from item_master im join ITEM_CATEGORY_MASTER ic on im.CATEGORY_ID=ic.CATEGORY_ID WHERE item_name IN('cotton','yarn','wool','DISPERSE') and CATEGORY_NAME='DYES STUFF'", true, "--Select--");
    }
    protected void dddyesstuff_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillRecipe();
        fillChemical();

    }
    protected void ddlpretreatment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlpretreatment.SelectedIndex > 1)
        {
            fillpretreatment();
        }
    }
    protected void fillquality()
    {
        UtilityModule.ConditionalComboFill(ref ddLocalQuality, @"SELECT QualityId,QualityName FROM QUALITY WHERE ITEM_ID=" + ddItemname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "  Order By QualityId desc", true, "--Select--");
    }
    protected void filldesign()
    {
       // string str = "select Distinct vf.designId,vf.designName From V_FinishedItemDetail vf where vf.QualityId=" + ddLocalQuality.SelectedValue + @" and vf.designid<>0 order by vf.designName";
        //                      select isnull((select processId From Item_Process Where QualityId=" + DDQuality.SelectedValue + " and SeqNo=IP.SeqNo-1),0) as FromProcessid From Item_Process IP Where QualityId=" + DDQuality.SelectedValue + " and processid=" + DDTOProcess.SelectedValue;
        //string str = "select Distinct vf.designId,vf.designName From design vf where designid in(8214,8215)";
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //UtilityModule.ConditionalComboFillWithDS(ref ddldesign, ds, 0, true, "--Plz Select--");


    
    }
    protected void fillRecipe() 
    {
        string str = "DECLARE @start INT = 1;DECLARE @end INT = 12;WITH numbers AS (SELECT @start AS srno,'' as QualityName,0 as qualityid ,0 as PORTION,0 as qty,0 as prate,0 as cost  UNION ALL SELECT srno + 1 ,'' as QualityName,0 as qualityid ,0 as PORTION,0 as qty,0 as prate,0 as cost     FROM  numbers    WHERE srno < @end) SELECT * FROM numbers OPTION (MAXRECURSION 0)";
        //                      select isnull((select processId From Item_Process Where QualityId=" + DDQuality.SelectedValue + " and SeqNo=IP.SeqNo-1),0) as FromProcessid From Item_Process IP Where QualityId=" + DDQuality.SelectedValue + " and processid=" + DDTOProcess.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        grdreceipe.DataSource = ds;
        grdreceipe.DataBind();

    
    }
    protected void fillChemical()
    {//select item_id,item_name from item_master where category_id= "+ddlcategory.SelectedValue+" And MasterCompanyId=" + Session["varCompanyId"] + " Order By item_name
        string str = "SELECT ROW_NUMBER() over(order by (select 1)) as SRNO,item_name as QualityName,item_id as qualityid,0 as PORTION,0 as qty,0 as prate,0 as cost FROM item_master WHERE category_id=51 And MasterCompanyId=" + Session["varCompanyId"] + " and    dyechem=1    Order By item_id desc";
        //                      select isnull((select processId From Item_Process Where QualityId=" + DDQuality.SelectedValue + " and SeqNo=IP.SeqNo-1),0) as FromProcessid From Item_Process IP Where QualityId=" + DDQuality.SelectedValue + " and processid=" + DDTOProcess.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        dgchemical.DataSource = ds;
        dgchemical.DataBind();


    }
    protected void fillpretreatment()
    {//select item_id,item_name from item_master where category_id= "+ddlcategory.SelectedValue+" And MasterCompanyId=" + Session["varCompanyId"] + " Order By item_name
        string str = "SELECT ROW_NUMBER() over(order by (select 1)) as SRNO,item_name as QualityName,item_id as qualityid,0 as PORTION,0 as qty,0 as prate,0 as cost FROM item_master WHERE category_id=51 And MasterCompanyId=" + Session["varCompanyId"] + " and  PRETREAMENT=1   Order By item_name ";
        //                      select isnull((select processId From Item_Process Where QualityId=" + DDQuality.SelectedValue + " and SeqNo=IP.SeqNo-1),0) as FromProcessid From Item_Process IP Where QualityId=" + DDQuality.SelectedValue + " and processid=" + DDTOProcess.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        grdpretreatment.DataSource = ds;
        grdpretreatment.DataBind();


    }
    protected void fillcolor()
    {
      //  str = "select Distinct Shadecolorid,ShadeColorName From V_FinishedItemdetail Where ITEM_ID=" + DDItem.SelectedValue + " and QualityId=" + DDQuality.SelectedValue + "";
        //UtilityModule.ConditionalComboFill(ref ddlcolor, "select Distinct Shadecolorid,ShadeColorName From V_FinishedItemdetail Where ITEM_ID=" + ddItemname.SelectedValue + " and QualityId=" + ddLocalQuality.SelectedValue + " Order By ShadeColorName", true, "--Select Color--");
        UtilityModule.ConditionalComboFill(ref ddlcolor, "select Distinct Shadecolorid,ShadeColorName From V_FinishedItemdetail WHERE ISNULL(ShadeColorName,'')<>'' Order By ShadeColorName", true, "--Select Color--");
    
    }
    protected void btnsearchedit_Click(object sender, EventArgs e)
    {
    }
    protected void dgquality_SelectedIndexChanged(object sender, EventArgs e)
    { 
    
    }
    protected void dgdesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList dgdesign = (DropDownList)sender;
        GridViewRow row = (GridViewRow)dgdesign.Parent.Parent;
        Label qualityid = ((Label)row.FindControl("lbldgqualityid"));
        DropDownList dgcolor = ((DropDownList)row.FindControl("dgcolor"));
        
        //UtilityModule.ConditionalComboFill(ref dgcolor, "select  ColorId,ColorName from Color Where  design=" + dgdesign.SelectedValue + " and MasterCompanyId=" + Session["varCompanyId"] + " Order By ColorName", true, "--Select Color--");
        UtilityModule.ConditionalComboFill(ref dgcolor, "select shadecolorid,shadecolorname from ShadeColor_res where itemid=" + dddyesstuff.SelectedValue + " and mastercompanyId=" + Session["varCompanyId"] + " and brandid=" + dgdesign.SelectedValue + " order by shadecolorname desc", true, "--Select Color--");
    }
    protected void dgcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList dgcolor = (DropDownList)sender;
        GridViewRow row = (GridViewRow)dgcolor.Parent.Parent;
        Label colorrate = ((Label)row.FindControl("lblresprate"));
        DropDownList dgresbrand = ((DropDownList)row.FindControl("dgdesign"));
        string str = "select rate from defineBrandrate where itemid=" + dddyesstuff.SelectedValue + " and brandid=" + dgresbrand.SelectedValue + " and shadecolorid=" + dgcolor.SelectedValue + "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    colorrate.Text = ds.Tables[0].Rows[0]["rate"].ToString();
                }
               
            }

        }

    }
    protected void dgchemdesign_SelectedIndexChanged(object sender, EventArgs e)
    {

        DropDownList dgchemdesign = (DropDownList)sender;
        GridViewRow row = (GridViewRow)dgchemdesign.Parent.Parent;
        //Label qualityid = ((Label)row.FindControl("lbldgqualityid"));
        DropDownList dgchembrand = ((DropDownList)row.FindControl("dgchembrand"));
        //string strbrand = @"select brandid,brandname from brand b join ITEM_MASTER i on b.item_id=i.ITEM_ID WHERE PRETREAMENT=1 and b.ITEM_ID=" + lblbrand.Text;
        //DataSet dsbrand = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strbrand);
        //UtilityModule.ConditionalComboFillWithDS(ref dgprebrand, dsbrand, 0, true, "--Plz Select--");
        UtilityModule.ConditionalComboFill(ref dgchembrand, "select brandid,brandname from brand b join ITEM_MASTER i on b.item_id=i.ITEM_ID WHERE dyechem=1 and b.ITEM_ID=" + dgchemdesign.SelectedValue, true, "--Select Brand--");
    
    
    
    }
    protected void dgchembrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList dgchembrand = (DropDownList)sender;
        GridViewRow row = (GridViewRow)dgchembrand.Parent.Parent;
        DropDownList dgchemdesign = ((DropDownList)row.FindControl("dgchemdesign"));
        Label lblitem = ((Label)row.FindControl("lbldgchemqualityid"));
        Label checmprate = ((Label)row.FindControl("lblchemresprate"));
        string str = "select rate  from defineBrandrate where brandid=" + dgchembrand.SelectedValue + " and itemid=" + dgchemdesign.SelectedValue;
      //  string str = "select rate from RateDescChemical where item_id=" + dgchemdesign.SelectedValue+" and designid="+dgchembrand.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    checmprate.Text = ds.Tables[0].Rows[0]["rate"].ToString();
                }

            }

        }
        //DropDownList dgchemdesign = (DropDownList)sender;
        //GridViewRow row = (GridViewRow)dgchemdesign.Parent.Parent;
        ////Label qualityid = ((Label)row.FindControl("lbldgqualityid"));
        //DropDownList dgchembrand = ((DropDownList)row.FindControl("dgchembrand"));
        //UtilityModule.ConditionalComboFill(ref dgchembrand, "select Distinct vf.designId,vf.designName From design vf where designid in(8214,8215)", true, "--Select Brand--");



    }
    protected void grdreceipe_DataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           
            DropDownList dgdesign = ((DropDownList)e.Row.FindControl("dgdesign"));
            Label lblquality = ((Label)e.Row.FindControl("dglblquality"));
            string str = @"SELECT BRANDID,BRANDNAME FROM ITEM_MASTER i JOIN brand b on i.ITEM_ID=b.item_id JOIN ITEM_CATEGORY_MASTER IC ON I.CATEGORY_ID=IC.CATEGORY_ID WHERE ic.CATEGORY_NAME='DYES STUFF' and I.ITEM_ID=" + dddyesstuff.SelectedValue;

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            //if(lblquality.Text.ToUpper().Contains("YELLOW"))
            //{
            //e.Row.BackColor = System.Drawing.Color.Yellow;
            //}
            //else if (lblquality.Text.ToUpper().Contains("RED"))
            //{
            //    e.Row.BackColor = System.Drawing.Color.Red;
            //}
            //else if (lblquality.Text.ToUpper().Contains("WHITE"))
            //{
            //    e.Row.BackColor = System.Drawing.Color.White;
            //}
            //else if (lblquality.Text.ToUpper().Contains("BLACK"))
            //{
            //    e.Row.BackColor = System.Drawing.Color.Black;
            //    e.Row.ForeColor = System.Drawing.Color.White;
            //} 
            //else if (lblquality.Text.ToUpper().Contains("BLUE"))
            //{
            //    e.Row.BackColor = System.Drawing.Color.Blue;
            //}
            //else if (lblquality.Text.ToUpper().Contains("GREEN"))
            //{
            //    e.Row.BackColor = System.Drawing.Color.Green;
            //}
            UtilityModule.ConditionalComboFillWithDS(ref dgdesign, ds, 0, true, "--Plz Select--");
            DropDownList dgres = ((DropDownList)e.Row.FindControl("dgresunit"));
            string strres = @"select UNITID,UNITNAME from UNIT where UNITID in(11,12)";

            DataSet dsres = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strres);
            UtilityModule.ConditionalComboFillWithDS(ref dgres, dsres, 0, true, "--Plz Select--");


            dgdesign_SelectedIndexChanged(dgdesign, new EventArgs());
            ds.Dispose();

        }

    }
    protected void dgchemical_DataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList dgchemdesign = ((DropDownList)e.Row.FindControl("dgchemdesign"));
            string str = @"select item_id,item_name from item_master where    dyechem=1 ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref dgchemdesign, ds, 0, true, "--Plz Select--");
            DropDownList dgchem = ((DropDownList)e.Row.FindControl("dgchemunit"));
            string strunit = @"select UNITID,UNITNAME from UNIT where UNITID in(11,12)";

            DataSet dsunit = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strunit);
            UtilityModule.ConditionalComboFillWithDS(ref dgchem, dsunit, 0, true, "--Plz Select--");

            dgchemdesign_SelectedIndexChanged(dgchemdesign, new EventArgs());
            ds.Dispose();

        }

    }
    protected void grdpretreatment_DataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList dgpre = ((DropDownList)e.Row.FindControl("dgpreunit"));
            string str = @"select UNITID,UNITNAME from UNIT where UNITID in(11,12)";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref dgpre, ds, 0, true, "--Plz Select--");
            DropDownList dgprebrand = ((DropDownList)e.Row.FindControl("dgprebrand"));
            Label lblbrand = ((Label)e.Row.FindControl("lbldgprequalityid"));
            string strbrand = @"select brandid,brandname from brand b join ITEM_MASTER i on b.item_id=i.ITEM_ID WHERE PRETREAMENT=1 and b.ITEM_ID=" + lblbrand.Text;
            DataSet dsbrand = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strbrand);
            UtilityModule.ConditionalComboFillWithDS(ref dgprebrand, dsbrand, 0, true, "--Plz Select--");
            //dgchemdesign_SelectedIndexChanged(dgchemdesign, new EventArgs());
            //ds.Dispose();

        }

    }
    protected void dgprebrand_SelectedIndexChanged(object sender, EventArgs e)
    {

        DropDownList dgchembrand = (DropDownList)sender;
        GridViewRow row = (GridViewRow)dgchembrand.Parent.Parent;
        DropDownList dgchemdesign = ((DropDownList)row.FindControl("dgchemdesign"));
        Label checmprate = ((Label)row.FindControl("lblprechemresprate"));
        Label lblitem = ((Label)row.FindControl("lbldgprequalityid"));
        string str = "select rate  from defineBrandrate where brandid="+dgchembrand.SelectedValue+" and itemid="+lblitem.Text;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    checmprate.Text = ds.Tables[0].Rows[0]["rate"].ToString();
                }

            }

        }


    }
    protected void txtresportion_TextChanged(object sender, EventArgs e)
    {
        TextBox txtportion = (TextBox)sender;
        GridViewRow row = (GridViewRow)txtportion.Parent.Parent;
        Label rescost = ((Label)row.FindControl("lblrescost"));
        Label resqty = ((Label)row.FindControl("lblresqty"));
        Label rate = ((Label)row.FindControl("lblresprate"));
        DropDownList respreunit = ((DropDownList)row.FindControl("dgresunit"));
        decimal portionval =Convert.ToDecimal(txtportion.Text);
        decimal qty =0;
        if (respreunit.SelectedItem.Text.ToUpper().Contains("GPL"))
        {
            qty = portionval * 10;
        }
        else { qty = (decimal)Math.Round((decimal)(1000 * portionval) / 100);  }
        decimal  prate =Convert.ToDecimal(rate.Text);
        prate = (prate / 1000);
        resqty.Text = Convert.ToString(qty);
        rescost.Text =Convert.ToString(prate * qty);
    }
    protected void txtchemresportion_TextChanged(object sender, EventArgs e)
    {
        decimal qty = 0;
        TextBox txtchemportion = (TextBox)sender;
        GridViewRow row = (GridViewRow)txtchemportion.Parent.Parent;
        Label rescost = ((Label)row.FindControl("lblchemrescost"));
        DropDownList respreunit = ((DropDownList)row.FindControl("dgchemunit"));
        Label resqty = ((Label)row.FindControl("lblchemresqty"));
        Label rate = ((Label)row.FindControl("lblchemresprate"));
        decimal portionval = Convert.ToDecimal(txtchemportion.Text);
        if (respreunit.SelectedItem.Text.ToUpper().Contains("GPL"))
        {
            qty = portionval*10;
        }
        else { qty = (decimal)Math.Round((decimal)(1000 * portionval) / 100); }
        decimal prate = Convert.ToDecimal(rate.Text);
        prate = (prate / 1000);
        resqty.Text = Convert.ToString(qty);
        rescost.Text = Convert.ToString(prate * qty);
    }
    protected void txtprechemresportion_TextChanged(object sender, EventArgs e)
    {
        decimal qty = 0,h2o2portion=0;
        TextBox txtchemportion = (TextBox)sender;
        GridViewRow row = (GridViewRow)txtchemportion.Parent.Parent;
        Label rescost = ((Label)row.FindControl("lblprechemrescost"));
        DropDownList respreunit = ((DropDownList)row.FindControl("dgpreunit"));
        Label resqty = ((Label)row.FindControl("lblprechemresqty"));
        Label rate = ((Label)row.FindControl("lblprechemresprate"));
        decimal portionval = Convert.ToDecimal(txtchemportion.Text);
        Label qsendername = (Label)row.FindControl("lblchemqualityname");
        if (qsendername.Text.ToString().ToUpper().Contains("H2 O2"))
        {
            foreach (GridViewRow resrow in grdpretreatment.Rows)
            {
                Label qname = (Label)resrow.FindControl("lblchemqualityname");
                if (qname.Text.ToString().ToUpper().Contains("STABILISER"))
                {
                    TextBox txtchemh2o2portion = (TextBox)resrow.FindControl("txtprechemresportion");
                    Label stacost = ((Label)row.FindControl("lblprechemrescost"));
                    DropDownList stapreunit = ((DropDownList)row.FindControl("dgpreunit"));
                    Label staqty = ((Label)row.FindControl("lblprechemresqty"));
                    Label starate = ((Label)row.FindControl("lblprechemresprate"));
                    txtchemh2o2portion.Text =Convert.ToString(portionval / 5);
                    decimal staportionval =Convert.ToDecimal(txtchemh2o2portion.Text);
                    decimal stabqty = 0;
                    if (stapreunit.SelectedItem.Text.ToUpper().Contains("GPL"))
                    {
                        stabqty = staportionval * 10;
                    }
                    else { stabqty = staportionval; }
                    decimal staprate = Convert.ToDecimal(starate.Text);
                    staqty.Text = Convert.ToString(qty);
                    stacost.Text = Convert.ToString(staprate * qty);
                    
                }

            }

        }

        if (respreunit.SelectedItem.Text.ToUpper().Contains("GPL"))
        {
            qty = portionval * 10;
        }
        else { qty =(int) Math.Round((double)(1000 * portionval) / 100); }
        decimal prate = Convert.ToDecimal(rate.Text);
        prate = (prate / 1000);
        resqty.Text =Convert.ToString(qty); 
        rescost.Text = Convert.ToString(prate * qty);
    }
    protected void dgpreunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        decimal qty = 0;
        DropDownList txtchemportion = (DropDownList)sender;
        GridViewRow row = (GridViewRow)txtchemportion.Parent.Parent;
        Label rescost = ((Label)row.FindControl("lblprechemrescost"));
        TextBox respreportion = ((TextBox)row.FindControl("txtprechemresportion"));
        DropDownList respreunit = ((DropDownList)row.FindControl("dgpreunit"));
        Label resqty = ((Label)row.FindControl("lblprechemresqty"));
        Label rate = ((Label)row.FindControl("lblprechemresprate"));
        decimal portionval = Convert.ToDecimal(respreportion.Text);
        if (respreunit.SelectedItem.Text.ToUpper().Contains("GPL"))
        {
            qty = portionval * 10;
        }
        else { qty = (int)Math.Round((double)(1000 * portionval) / 100); }
        decimal prate = Convert.ToDecimal(rate.Text);
        prate = (prate / 1000);
        resqty.Text = Convert.ToString(qty);
        rescost.Text = Convert.ToString(prate * qty);
    }
    protected void dgchemunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        decimal qty = 0;
        DropDownList txtchemportion = (DropDownList)sender;
        GridViewRow row = (GridViewRow)txtchemportion.Parent.Parent;
        Label rescost = ((Label)row.FindControl("lblchemrescost"));
        TextBox respreportion = ((TextBox)row.FindControl("txtchemresportion"));
        DropDownList respreunit = ((DropDownList)row.FindControl("dgchemunit"));
        Label resqty = ((Label)row.FindControl("lblchemresqty"));
        Label rate = ((Label)row.FindControl("lblchemresprate"));
        decimal portionval = Convert.ToDecimal(respreportion.Text);
        if (respreunit.SelectedItem.Text.ToUpper().Contains("GPL"))
        {
            qty = portionval * 10;
        }
        else { qty = (int)Math.Round((double)(1000 * portionval) / 100);  }
        decimal prate = Convert.ToDecimal(rate.Text);
        prate = (prate / 1000);
        resqty.Text = Convert.ToString(qty);
        rescost.Text = Convert.ToString(prate * qty);
    }
    protected void dgresunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList obj = (DropDownList)sender;
        GridViewRow row = (GridViewRow)obj.Parent.Parent;
        Label rescost = ((Label)row.FindControl("lblrescost"));
        Label resqty = ((Label)row.FindControl("lblresqty"));
        Label rate = ((Label)row.FindControl("lblresprate"));
        DropDownList respreunit = ((DropDownList)row.FindControl("dgresunit"));
        TextBox txtportion = ((TextBox)row.FindControl("txtresportion"));
        decimal portionval = Convert.ToDecimal(txtportion.Text);
        decimal qty = 0;
        if (respreunit.SelectedItem.Text.ToUpper().Contains("GPL"))
        {
            qty = portionval * 10;
        }
        else { qty = (int)Math.Round((double)(1000 * portionval) / 100); ; }
        decimal prate = Convert.ToDecimal(rate.Text);
        prate = (prate / 1000);
        resqty.Text = Convert.ToString(qty);
        rescost.Text = Convert.ToString(prate * qty);
    
    
    
    
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
//FOR RECEPIE SECTION
            DataTable dtrecords = new DataTable();
            dtrecords.Columns.Add("rfinishedid", typeof(int));
            dtrecords.Columns.Add("rtype", typeof(int));
            dtrecords.Columns.Add("rUnitid", typeof(int));
            dtrecords.Columns.Add("ritemid", typeof(int));
            dtrecords.Columns.Add("rqualityid", typeof(int));
            dtrecords.Columns.Add("rdesignid", typeof(int));
            dtrecords.Columns.Add("rcolorid", typeof(int));
            dtrecords.Columns.Add("portion", typeof(decimal));
            dtrecords.Columns.Add("qty", typeof(int));
            //dtrecords.Columns.Add("Noofcone", typeof(int));
            dtrecords.Columns.Add("rate", typeof(decimal));
            dtrecords.Columns.Add("cost", typeof(decimal));
            for (int i = 0; i < grdreceipe.Rows.Count; i++)
            {
               // CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
                TextBox resportion = ((TextBox)grdreceipe.Rows[i].FindControl("txtresportion"));
                if (resportion.Text != "" && resportion.Text != "0")
                {
                    DropDownList rescolor = ((DropDownList)grdreceipe.Rows[i].FindControl("dgcolor"));
                    DropDownList resdesign = ((DropDownList)grdreceipe.Rows[i].FindControl("dgdesign"));
                    DropDownList resunit = ((DropDownList)grdreceipe.Rows[i].FindControl("dgresunit"));

                    Label resquality = ((Label)grdreceipe.Rows[i].FindControl("lbldgqualityid"));
                    Label cost = ((Label)grdreceipe.Rows[i].FindControl("lblrescost"));
                    Label rate = ((Label)grdreceipe.Rows[i].FindControl("lblresprate"));
                    Label qty = ((Label)grdreceipe.Rows[i].FindControl("lblresqty"));
                    Label unit = ((Label)grdreceipe.Rows[i].FindControl("lblresunit"));
                    DataRow dr = dtrecords.NewRow();
                    int resFinishedId = UtilityModule.getItemFinishedId(Convert.ToInt32(dddyesstuff.SelectedValue), 0, Convert.ToInt32(resdesign.SelectedValue), 0, 0, 0, Convert.ToInt32(rescolor.SelectedValue), "", Convert.ToInt32(Session["varCompanyId"]));
                    dr["rfinishedid"] = resFinishedId;
                    dr["rtype"] = 3;
                    dr["rUnitid"] = resunit.SelectedValue;
                    dr["ritemid"] = 0;
                    dr["rqualityid"] = Convert.ToInt16(resquality.Text);
                    dr["rdesignid"] = Convert.ToInt32(resdesign.SelectedValue);
                    dr["rcolorid"] = Convert.ToInt32(rescolor.SelectedValue); ;
                    dr["portion"] = Convert.ToDecimal(resportion.Text);
                    dr["qty"] = Convert.ToInt32(qty.Text == "" ? "0" : qty.Text);
                    //dr["Noofcone"] = txtnoofcone.Text == "" ? "0" : txtnoofcone.Text;
                    dr["rate"] = Convert.ToDecimal(rate.Text == "" ? "0" : rate.Text);
                    dr["cost"] = Convert.ToDecimal(cost.Text == "" ? "0" : cost.Text);
                    // dr["BinNo"] = variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : "";
                    dtrecords.Rows.Add(dr);
                }



            }
            //FOR PRE TREATMENT
            DataTable dtpre = new DataTable();
            dtpre.Columns.Add("rfinishedid", typeof(int));
            dtpre.Columns.Add("rtype", typeof(int));
            dtpre.Columns.Add("rUnitid", typeof(int));
            dtpre.Columns.Add("ritemid", typeof(int));
            dtpre.Columns.Add("rqualityid", typeof(int));
            dtpre.Columns.Add("rdesignid", typeof(int));
            dtpre.Columns.Add("rcolorid", typeof(int));
            dtpre.Columns.Add("portion", typeof(decimal));
            dtpre.Columns.Add("qty", typeof(int));
            //dtrecords.Columns.Add("Noofcone", typeof(int));
            dtpre.Columns.Add("rate", typeof(decimal));
            dtpre.Columns.Add("cost", typeof(decimal));
            for (int i = 0; i < grdpretreatment.Rows.Count; i++)
            {
                // CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
                TextBox resportion = ((TextBox)grdpretreatment.Rows[i].FindControl("txtprechemresportion"));
                if (resportion.Text != "" && resportion.Text != "0")
                {
                  //  DropDownList rescolor = ((DropDownList)grdpretreatment.Rows[i].FindControl("dgcolor"));
                    DropDownList resdesign = ((DropDownList)grdpretreatment.Rows[i].FindControl("dgprebrand"));
                    DropDownList resunit = ((DropDownList)grdpretreatment.Rows[i].FindControl("dgpreunit"));

                    Label resquality = ((Label)grdpretreatment.Rows[i].FindControl("lbldgprequalityid"));
                    Label cost = ((Label)grdpretreatment.Rows[i].FindControl("lblprechemrescost"));
                    Label rate = ((Label)grdpretreatment.Rows[i].FindControl("lblprechemresprate"));
                    Label qty = ((Label)grdpretreatment.Rows[i].FindControl("lblprechemresqty"));
                   // Label unit = ((Label)grdpretreatment.Rows[i].FindControl("lblresunit"));
                    DataRow dr = dtpre.NewRow();
                    int resFinishedId = UtilityModule.getItemFinishedId(Convert.ToInt32(resdesign.SelectedValue), 0, 0, 0, 0, 0, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                    dr["rfinishedid"] = resFinishedId;
                    dr["rtype"] = 1;
                    dr["rUnitid"] = resunit.SelectedValue;
                    dr["ritemid"] = Convert.ToInt16(resquality.Text);
                    dr["rqualityid"] = Convert.ToInt16(resquality.Text);
                    dr["rdesignid"] = Convert.ToInt32(resdesign.SelectedValue);
                    dr["rcolorid"] = 0 ;
                    dr["portion"] = Convert.ToDecimal(resportion.Text);
                    dr["qty"] = Convert.ToDecimal(qty.Text == "" ? "0" : qty.Text);
                    //dr["Noofcone"] = txtnoofcone.Text == "" ? "0" : txtnoofcone.Text;
                    dr["rate"] = Convert.ToDecimal(rate.Text == "" ? "0" : rate.Text);
                    dr["cost"] = Convert.ToDecimal(cost.Text == "" ? "0" : cost.Text);
                    // dr["BinNo"] = variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : "";
                    dtpre.Rows.Add(dr);
                }



            }

            //FOR PRE TREATMENT
            DataTable dtchem = new DataTable();
            dtchem.Columns.Add("rfinishedid", typeof(int));
            dtchem.Columns.Add("rtype", typeof(int));
            dtchem.Columns.Add("rUnitid", typeof(int));
            dtchem.Columns.Add("ritemid", typeof(int));
            dtchem.Columns.Add("rqualityid", typeof(int));
            dtchem.Columns.Add("rdesignid", typeof(int));
            dtchem.Columns.Add("rcolorid", typeof(int));
            dtchem.Columns.Add("portion", typeof(decimal));
            dtchem.Columns.Add("qty", typeof(int));
            //dtrecords.Columns.Add("Noofcone", typeof(int));
            dtchem.Columns.Add("rate", typeof(decimal));
            dtchem.Columns.Add("cost", typeof(decimal));
            for (int i = 0; i < dgchemical.Rows.Count; i++)
            {
                // CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
                TextBox resportion = ((TextBox)dgchemical.Rows[i].FindControl("txtchemresportion"));
                if (resportion.Text != "" && resportion.Text != "0")
                {
                    DropDownList resquality = ((DropDownList)dgchemical.Rows[i].FindControl("dgchemdesign"));
                    DropDownList resdesign = ((DropDownList)dgchemical.Rows[i].FindControl("dgchembrand"));
                    DropDownList resunit = ((DropDownList)dgchemical.Rows[i].FindControl("dgchemunit"));

                    //Label resquality = ((Label)dgchemical.Rows[i].FindControl("lbldgprequalityid"));
                    Label cost = ((Label)dgchemical.Rows[i].FindControl("lblchemrescost"));
                    Label rate = ((Label)dgchemical.Rows[i].FindControl("lblchemresprate"));
                    Label qty = ((Label)dgchemical.Rows[i].FindControl("lblchemresqty"));
                    // Label unit = ((Label)grdpretreatment.Rows[i].FindControl("lblresunit"));
                    DataRow dr = dtchem.NewRow();
                    int resFinishedId = UtilityModule.getItemFinishedId(Convert.ToInt32(resdesign.SelectedValue), 0, 0, 0, 0, 0, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                    dr["rfinishedid"] = resFinishedId;
                    dr["rtype"] = 2;
                    dr["rUnitid"] = resunit.SelectedValue;
                    dr["ritemid"] = Convert.ToInt16(resquality.SelectedValue);
                    dr["rqualityid"] = Convert.ToInt16(resquality.Text);
                    dr["rdesignid"] = Convert.ToInt32(resdesign.SelectedValue);
                    dr["rcolorid"] = 0;
                    dr["portion"] = Convert.ToDecimal(resportion.Text);
                    dr["qty"] = Convert.ToDecimal(qty.Text == "" ? "0" : qty.Text);
                    //dr["Noofcone"] = txtnoofcone.Text == "" ? "0" : txtnoofcone.Text;
                    dr["rate"] = Convert.ToDecimal(rate.Text == "" ? "0" : rate.Text);
                    dr["cost"] = Convert.ToDecimal(cost.Text == "" ? "0" : cost.Text);
                    // dr["BinNo"] = variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : "";
                    dtchem.Rows.Add(dr);
                }



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
                int ItemFinishedId = UtilityModule.getItemFinishedId(Convert.ToInt32(ddItemname.SelectedValue), Convert.ToInt32(ddLocalQuality.Text), 0, 0, 0, 0, Convert.ToInt32(ddlcolor.SelectedValue), "", Convert.ToInt32(Session["varCompanyId"]));
                SqlParameter[] param = new SqlParameter[16];
                param[0] = new SqlParameter("@resId", SqlDbType.Int);
                param[0].Value = 0;
                param[0].Direction = ParameterDirection.InputOutput;
                param[1] = new SqlParameter("@companyid", ddlcompany.SelectedValue);
                param[2] = new SqlParameter("@IFINISHEDID", ItemFinishedId);
                param[3] = new SqlParameter("@USERID", Session["varuserid"]);
                param[4] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
                param[5] = new SqlParameter("@dtrecords", dtrecords);
                param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[6].Direction = ParameterDirection.Output;
                param[7] = new SqlParameter("@BUYER", SqlDbType.Int);
                param[7].Value =Convert.ToInt32(ddBuyerCode.SelectedValue==""?"0":ddBuyerCode.SelectedValue);
                param[8] = new SqlParameter("@DYETYPEID", SqlDbType.Int);
                param[8].Value = Convert.ToInt32(dddyingtype.SelectedValue == "" ? "0" : dddyingtype.SelectedValue);
                param[9] = new SqlParameter("@DYESTUFFID", SqlDbType.Int);
                param[9].Value = Convert.ToInt32(dddyesstuff.SelectedValue == "" ? "0" : dddyesstuff.SelectedValue);
                param[10] = new SqlParameter("@CHALANNO", SqlDbType.Int);
                param[10].Direction = ParameterDirection.Output;
                param[11] = new SqlParameter("@TEMP", SqlDbType.Decimal);
                param[11].Value = txttemp.Text==""?"0":txttemp.Text;
                param[12] = new SqlParameter("@PHVAL", SqlDbType.Decimal);
                param[12].Value = txtphvalue.Text == "" ? "0" : txtphvalue.Text; ;
                param[13] = new SqlParameter("@HOLDINGTIME", SqlDbType.Decimal);
                param[13].Value = txtholdtime.Text == "" ? "0" : txtholdtime.Text; ;
                param[14] = new SqlParameter("@DTRECORDS_PRE", dtpre);
                param[15] = new SqlParameter("@DTRECORDS_CHECM", dtchem);
               
                //param[10] = new SqlParameter("@EWayBillNo", TxtEWayBillNo.Text);
                //param[11] = new SqlParameter("@StockNoQty", TxtTotalPcs.Text == "" ? "0" : TxtTotalPcs.Text);
                //param[12] = new SqlParameter("@FolioChallanNo", DDFoliono.SelectedIndex > 0 ? DDFoliono.SelectedItem.Text : "");
                //param[13] = new SqlParameter("@CHALANNO", SqlDbType.VarChar, 50);
                //param[13].Value = "";
                //param[13].Direction = ParameterDirection.InputOutput;

                ///**********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVERECEIPE", param);
                //*******************
                ViewState["resip"] = param[10].Value.ToString();
                //txtissueno.Text = param[13].Value.ToString();
                //hnissueid.Value = param[0].Value.ToString();
                Tran.Commit();
                if (param[10].Value.ToString() != "")
                {
                    lblredno.Text = param[10].Value.ToString();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('DATA SAVED SUCCESSFULLY!!!!!');", true);
                    lblmsg.Text = "DATA SAVED SUCCESSFULLY.";
                    grdpretreatment.DataSource = null;
                    grdpretreatment.DataBind();
                    grdreceipe.DataSource = null;
                    grdreceipe.DataBind();
                    dgchemical.DataSource = null;
                    dgchemical.DataBind();
                }
                else
                {
                    lblmsg.Text = "ERROR!!";
                    //Fillgrid();
                    //FillissueGrid();
                }
              //  TxtTotalPcs.Text = "";

            }
            catch (Exception ex)
            {
                Tran.Rollback();
               // lblmessage.Text = ex.Message;
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
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        RESSummaryDetail();

    }
    protected void RESSummaryDetail()
    {
        string where = "";
        lblmsg.Text = "";
        if (!Directory.Exists(Server.MapPath("~/InvoiceExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/InvoiceExcel/"));
        }

        try
        {
            var xapp = new XLWorkbook();

            var sht = xapp.Worksheets.Add("Receipe Summmary");

            sht.Column("A").Width = 10.89;
            sht.Column("B").Width = 30.89;
            sht.Column("C").Width = 25.89;
            sht.Column("D").Width = 10.89;
            sht.Column("E").Width = 15.89;
            sht.Column("F").Width = 20.89;
            sht.Column("G").Width = 15.89;
            sht.Column("H").Width = 15.89;
           
            sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            sht.PageSetup.AdjustTo(90);
            sht.PageSetup.PaperSize = XLPaperSize.A4Paper;


            sht.PageSetup.Margins.Top = 0.1;
            sht.PageSetup.Margins.Left = 0.1;
            sht.PageSetup.Margins.Right = 0.1;
            sht.PageSetup.Margins.Bottom = 0.1;
            //sht.PageSetup.Margins.Header = 1.20;
            //sht.PageSetup.Margins.Footer = 0.3;
            sht.PageSetup.SetScaleHFWithDocument();
            if (ViewState["resip"] == null)
            {
                lblmsg.Text = "Data Not Saved!!!";
                return;
            }
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_RECEIPE_SUMMMARY", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@companyId", ddlcompany.SelectedValue);
            cmd.Parameters.AddWithValue("@RMSID", Convert.ToInt32(ViewState["resip"]));
            //cmd.Parameters.AddWithValue("@WIPDETAILWITHLOTNO", chkwpidetailwithlotno.Checked == true ? "1" : "0");

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            con.Close();
            con.Dispose();
            decimal GrandTotal = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                //ds.Tables[0].Columns.Remove("Stockno");
                //ds.Tables[0].Columns.Remove("SeqNo");
                ////Export to excel
                //var finalheader = from t in ds.Tables[0].AsEnumerable()
                //                  where t.Field<Int32>("qty") > 0
                //                  group t by new { hcolor = t.Field<string>("color"), hsize = t.Field<string>("size"), design = t.Field<string>("design"), colorid = t.Field<Int32>("colorid"), sizeid = t.Field<Int32>("sizeid"), designid = t.Field<Int32>("designid") }
                //                      into res
                //                      select new { hcolor = res.Key.hcolor, hsize = res.Key.hsize, design = res.Key.design, colorid = res.Key.colorid, sizeid = res.Key.sizeid, designid = res.Key.designid };


                sht.Cell("A1").Value = "CHAMPO CARPET";
                sht.Range("A1:H1").Style.Font.FontName = "Arial";
                sht.Range("A1:H1").Style.Font.FontSize = 12;
                sht.Range("A1:H1").Style.Font.Bold = true;
                sht.Range("A1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell("A2").Value = "RECEPIE DETAILS "+DateTime.Today.ToShortDateString();
                sht.Range("A2:H2").Style.Font.FontName = "Arial";
                sht.Range("A2:H2").Style.Font.FontSize = 12;
                sht.Range("A2:H2").Style.Font.Bold = true;
                sht.Range("A2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                using (var a = sht.Range("A1:H2"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                sht.Range("A1:H1").Merge();
                sht.Range("A2:H2").Merge();
                sht.Cell("A3").Value = "INDENT DETAILS";
                sht.Range("A3:H3").Style.Font.FontName = "Arial";
                sht.Range("A3:H3").Style.Font.FontSize = 12;
                sht.Range("A3:H3").Style.Font.Bold = true;
                sht.Range("A3:H3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A3:H3").Merge();


                sht.Cell("A4").Value = "BUYER";
                sht.Range("A4:A4").Style.Font.FontName = "Arial";
                sht.Range("A4:A4").Style.Font.FontSize = 12;
                sht.Range("A4:A4").Style.Font.Bold = true;
                sht.Range("A4:A4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Cell("B4").Value = "ITEM DESC.";
                sht.Range("B4:B4").Style.Font.FontName = "Arial";
                sht.Range("B4:B4").Style.Font.FontSize = 12;
                sht.Range("B4:B4").Style.Font.Bold = true;
                sht.Range("B4:B4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Cell("C4").Value = "TEMP.";
                sht.Range("C4:C4").Style.Font.FontName = "Arial";
                sht.Range("C4:C4").Style.Font.FontSize = 12;
                sht.Range("C4:C4").Style.Font.Bold = true;
                sht.Range("C4:C4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Cell("D4").Value = "PHVAL";
                sht.Range("D4:D4").Style.Font.FontName = "Arial";
                sht.Range("D4:D4").Style.Font.FontSize = 12;
                sht.Range("D4:D4").Style.Font.Bold = true;
                sht.Range("D4:D4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Cell("E4").Value = "HOLDING-TIME";
                sht.Range("E4:E4").Style.Font.FontName = "Arial";
                sht.Range("E4:E4").Style.Font.FontSize = 12;
                sht.Range("E4:E4").Style.Font.Bold = true;
                sht.Range("E4:E4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Cell("F4").Value = "DYES-STUFF";
                sht.Range("F4:F4").Style.Font.FontName = "Arial";
                sht.Range("F4:F4").Style.Font.FontSize = 12;
                sht.Range("F4:F4").Style.Font.Bold = true;
                sht.Range("F4:F4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Cell("G4").Value = "DYEING TYPE";
                sht.Range("G4:G4").Style.Font.FontName = "Arial";
                sht.Range("G4:G4").Style.Font.FontSize = 12;
                sht.Range("G4:G4").Style.Font.Bold = true;
                sht.Range("G4:G4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Cell("H4").Value = "";
                sht.Range("H4:H4").Style.Font.FontName = "Arial";
                sht.Range("H4:H4").Style.Font.FontSize = 12;
                sht.Range("H4:H4").Style.Font.Bold = true;
                sht.Range("H4:H4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                using (var a = sht.Range("A4:H4"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                int row = 5;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sht.Cell("A"+row).Value = dr["BUYERNAME"].ToString();
                    sht.Range("A"+row+":A"+row).Style.Font.FontName = "Arial";
                    sht.Range("A"+row+":A"+row).Style.Font.FontSize = 10;
                    //sht.Range("A"+row+":A"+row).Style.Font.Bold = true;
                    sht.Range("A"+row+":A"+row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    sht.Cell("B"+row).Value = dr["itemdesc"].ToString();
                    sht.Range("B"+row+":B"+row).Style.Font.FontName = "Arial";
                    sht.Range("B"+row+":B"+row).Style.Font.FontSize = 10;
                    //sht.Range("B"+row+":B"+row).Style.Font.Bold = true;
                    sht.Range("B"+row+":B"+row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    sht.Cell("C" + row).Value = dr["temp"].ToString();
                    sht.Range("C"+row+":C"+row).Style.Font.FontName = "Arial";
                    sht.Range("C"+row+":C"+row).Style.Font.FontSize = 10;
                    //sht.Range("C"+row+":C"+row).Style.Font.Bold = true;
                    sht.Range("C"+row+":C"+row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    sht.Cell("D" + row).Value = dr["phval"].ToString();
                    sht.Range("D"+row+":D"+row).Style.Font.FontName = "Arial";
                    sht.Range("D"+row+":D"+row).Style.Font.FontSize = 10;
                    //sht.Range("D"+row+":D"+row).Style.Font.Bold = true;
                    sht.Range("D"+row+":D"+row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    sht.Cell("E" + row).Value = dr["holdingtime"].ToString();
                    sht.Range("E"+row+":E"+row).Style.Font.FontName = "Arial";
                    sht.Range("E"+row+":E"+row).Style.Font.FontSize = 10;
                   // sht.Range("E"+row+":E"+row).Style.Font.Bold = true;
                    sht.Range("E"+row+":E"+row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    sht.Cell("F" + row).Value = dr["ITEM_NAME"].ToString();
                    sht.Range("F"+row+":F"+row).Style.Font.FontName = "Arial";
                    sht.Range("F"+row+":F"+row).Style.Font.FontSize = 10;
                   // sht.Range("F"+row+":F"+row).Style.Font.Bold = true;
                    sht.Range("F"+row+":F"+row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    sht.Cell("G" + row).Value = dr["CalType"].ToString();
                    sht.Range("G"+row+":G"+row).Style.Font.FontName = "Arial";
                    sht.Range("G"+row+":G"+row).Style.Font.FontSize = 10;
                    //sht.Range("G" + row + ":G" + row).Style.Font.Bold = true;
                    sht.Range("G"+row+":G"+row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    sht.Cell("H" + row).Value = "";
                    sht.Range("H"+row+":H"+row).Style.Font.FontName = "Arial";
                    sht.Range("H"+row+":H"+row).Style.Font.FontSize = 10;
                    //sht.Range("H"+row+":H"+row).Style.Font.Bold = true;
                    sht.Range("H"+row+":H"+row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    using (var a = sht.Range("A"+row+":H"+row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                
                }
                row = row + 1;
                decimal SUBTOTALPRE = 0;
                if (ds.Tables[1] != null)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        sht.Cell("A" + row).Value = "PRE-TREATMENT";
                        sht.Range("A" + row + ":H" + row).Style.Font.FontName = "Arial";
                        sht.Range("A" + row + ":H" + row).Style.Font.FontSize = 12;
                        sht.Range("A" + row + ":H" + row).Style.Font.Bold = true;
                        sht.Range("A" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                        sht.Range("A" + row + ":H" + row).Merge();
                        row = row + 1;
                        sht.Cell("A" + row).Value = "SR NO.";
                        sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Arial";
                        sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 10;
                        sht.Range("A" + row + ":A" + row).Style.Font.Bold = true;
                        sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("B" + row).Value = "CHEMICAL TYPE";
                        sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Arial";
                        sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 10;
                        sht.Range("B" + row + ":B" + row).Style.Font.Bold = true;
                        sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("C" + row).Value = "BRAND";
                        sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                        sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 10;
                        sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                        sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("D" + row).Value = "UoM";
                        sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                        sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 10;
                        sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                        sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("E" + row).Value = "PORTION";
                        sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                        sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 10;
                        sht.Range("E" + row + ":E" + row).Style.Font.Bold = true;
                        sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("F" + row).Value = "QUANTITY(GMS PER KG)";
                        sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Arial";
                        sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 10;
                        sht.Range("F" + row + ":F" + row).Style.Font.Bold = true;
                        sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("G" + row).Value = "RATE PER KG.";
                        sht.Range("G" + row + ":G" + row).Style.Font.FontName = "Arial";
                        sht.Range("G" + row + ":G" + row).Style.Font.FontSize = 10;
                        sht.Range("G" + row + ":G" + row).Style.Font.Bold = true;
                        sht.Range("G" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("H" + row).Value = "COST";
                        sht.Range("H" + row + ":H" + row).Style.Font.FontName = "Arial";
                        sht.Range("H" + row + ":H" + row).Style.Font.FontSize = 10;
                        sht.Range("H" + row + ":H" + row).Style.Font.Bold = true;
                        sht.Range("H" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        using (var a = sht.Range("A" + row + ":H" + row))
                        {
                            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        }
                        row = row + 1;
                    }
                }
                if (ds.Tables[1] != null)
                {
                    if(ds.Tables[1].Rows.Count>0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            sht.Cell("A" + row).Value = dr["SRNO"].ToString();
                            sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Arial";
                            sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 10;
                            //sht.Range("A"+row+":A"+row).Style.Font.Bold = true;
                            sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("B" + row).Value = dr["ITEM_NAME"].ToString();
                            sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Arial";
                            sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 10;
                            //sht.Range("B"+row+":B"+row).Style.Font.Bold = true;
                            sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("C" + row).Value = dr["brandname"].ToString();
                            sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                            sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 10;
                            //sht.Range("C"+row+":C"+row).Style.Font.Bold = true;
                            sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("D" + row).Value = dr["UnitName"].ToString();
                            sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                            sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 10;
                            //sht.Range("D"+row+":D"+row).Style.Font.Bold = true;
                            sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("E" + row).Value = dr["portion"].ToString();
                            sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                            sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 10;
                            // sht.Range("E"+row+":E"+row).Style.Font.Bold = true;
                            sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("F" + row).Value = dr["qty"].ToString();
                            sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Arial";
                            sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 10;
                            // sht.Range("F"+row+":F"+row).Style.Font.Bold = true;
                            sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("G" + row).Value = dr["rate"].ToString();
                            sht.Range("G" + row + ":G" + row).Style.Font.FontName = "Arial";
                            sht.Range("G" + row + ":G" + row).Style.Font.FontSize = 10;
                            //sht.Range("G" + row + ":G" + row).Style.Font.Bold = true;
                            sht.Range("G" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("H" + row).Value = dr["cost"].ToString();
                            sht.Range("H" + row + ":H" + row).Style.Font.FontName = "Arial";
                            sht.Range("H" + row + ":H" + row).Style.Font.FontSize = 10;
                            //sht.Range("H"+row+":H"+row).Style.Font.Bold = true;
                            sht.Range("H" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            using (var a = sht.Range("A" + row + ":H" + row))
                            {
                                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            }
                            SUBTOTALPRE = SUBTOTALPRE + Convert.ToDecimal(dr["cost"]);
                            row = row + 1;
                        }
                    }
                }
                row = row + 1;
                sht.Cell("E" + row).Value = "SUBTOTAL : ";
                sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 12;
                sht.Range("E" + row + ":E" + row).Style.Font.Bold = true;
                sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E" + row + ":E" + row).Merge();
                sht.Cell("H" + row).Value = SUBTOTALPRE;
                sht.Range("H" + row + ":H" + row).Style.Font.FontName = "Arial";
                sht.Range("H" + row + ":H" + row).Style.Font.FontSize = 12;
                sht.Range("H" + row + ":H" + row).Style.Font.Bold = true;
                sht.Range("H" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H" + row + ":H" + row).Merge();
                decimal SUBTOTALCHEM = 0;
                if (ds.Tables[2] != null)
                {
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        row = row + 1;
                        sht.Cell("A" + row).Value = "DYE BATH";
                        sht.Range("A" + row + ":H" + row).Style.Font.FontName = "Arial";
                        sht.Range("A" + row + ":H" + row).Style.Font.FontSize = 12;
                        sht.Range("A" + row + ":H" + row).Style.Font.Bold = true;
                        sht.Range("A" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                        sht.Range("A" + row + ":H" + row).Merge();
                        row = row + 1;
                        sht.Cell("A" + row).Value = "SR NO.";
                        sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Arial";
                        sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 10;
                        sht.Range("A" + row + ":A" + row).Style.Font.Bold = true;
                        sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("B" + row).Value = "CHEMICAL TYPE";
                        sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Arial";
                        sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 10;
                        sht.Range("B" + row + ":B" + row).Style.Font.Bold = true;
                        sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("C" + row).Value = "BRAND";
                        sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                        sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 10;
                        sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                        sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("D" + row).Value = "UoM";
                        sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                        sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 10;
                        sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                        sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("E" + row).Value = "PORTION";
                        sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                        sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 10;
                        sht.Range("E" + row + ":E" + row).Style.Font.Bold = true;
                        sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("F" + row).Value = "QUANTITY(GMS PER KG)";
                        sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Arial";
                        sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 10;
                        sht.Range("F" + row + ":F" + row).Style.Font.Bold = true;
                        sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("G" + row).Value = "RATE PER KG.";
                        sht.Range("G" + row + ":G" + row).Style.Font.FontName = "Arial";
                        sht.Range("G" + row + ":G" + row).Style.Font.FontSize = 10;
                        sht.Range("G" + row + ":G" + row).Style.Font.Bold = true;
                        sht.Range("G" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("H" + row).Value = "COST";
                        sht.Range("H" + row + ":H" + row).Style.Font.FontName = "Arial";
                        sht.Range("H" + row + ":H" + row).Style.Font.FontSize = 10;
                        sht.Range("H" + row + ":H" + row).Style.Font.Bold = true;
                        sht.Range("H" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        using (var a = sht.Range("A" + row + ":H" + row))
                        {
                            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        }
                        row = row + 1;
                    }
                }
                if (ds.Tables[2] != null)
                {
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            sht.Cell("A" + row).Value = dr["SRNO"].ToString();
                            sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Arial";
                            sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 10;
                            //sht.Range("A"+row+":A"+row).Style.Font.Bold = true;
                            sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("B" + row).Value = dr["ITEM_NAME"].ToString();
                            sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Arial";
                            sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 10;
                            //sht.Range("B"+row+":B"+row).Style.Font.Bold = true;
                            sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("C" + row).Value = dr["brandname"].ToString();
                            sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                            sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 10;
                            //sht.Range("C"+row+":C"+row).Style.Font.Bold = true;
                            sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("D" + row).Value = dr["UnitName"].ToString();
                            sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                            sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 10;
                            //sht.Range("D"+row+":D"+row).Style.Font.Bold = true;
                            sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("E" + row).Value = dr["portion"].ToString();
                            sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                            sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 10;
                            // sht.Range("E"+row+":E"+row).Style.Font.Bold = true;
                            sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("F" + row).Value = dr["qty"].ToString();
                            sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Arial";
                            sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 10;
                            // sht.Range("F"+row+":F"+row).Style.Font.Bold = true;
                            sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("G" + row).Value = dr["rate"].ToString();
                            sht.Range("G" + row + ":G" + row).Style.Font.FontName = "Arial";
                            sht.Range("G" + row + ":G" + row).Style.Font.FontSize = 10;
                            //sht.Range("G" + row + ":G" + row).Style.Font.Bold = true;
                            sht.Range("G" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("H" + row).Value = dr["cost"].ToString();
                            sht.Range("H" + row + ":H" + row).Style.Font.FontName = "Arial";
                            sht.Range("H" + row + ":H" + row).Style.Font.FontSize = 10;
                            //sht.Range("H"+row+":H"+row).Style.Font.Bold = true;
                            sht.Range("H" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            SUBTOTALCHEM = SUBTOTALCHEM + Convert.ToDecimal(dr["cost"]);
                            using (var a = sht.Range("A" + row + ":H" + row))
                            {
                                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            }
                            row = row + 1;
                        }
                    }
                }
                row = row + 1;
                sht.Cell("E" + row).Value = "SUBTOTAL : ";
                sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 12;
                sht.Range("E" + row + ":E" + row).Style.Font.Bold = true;
                sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E" + row + ":E" + row).Merge();
                sht.Cell("H" + row).Value = SUBTOTALCHEM;
                sht.Range("H" + row + ":H" + row).Style.Font.FontName = "Arial";
                sht.Range("H" + row + ":H" + row).Style.Font.FontSize = 12;
                sht.Range("H" + row + ":H" + row).Style.Font.Bold = true;
                sht.Range("H" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H" + row + ":H" + row).Merge();
                decimal SUBTOTALSTUFF = 0;
                if (ds.Tables[3] != null)
                {
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        row = row + 1;
                        sht.Cell("A" + row).Value = "DYE STUFF";
                        sht.Range("A" + row + ":H" + row).Style.Font.FontName = "Arial";
                        sht.Range("A" + row + ":H" + row).Style.Font.FontSize = 12;
                        sht.Range("A" + row + ":H" + row).Style.Font.Bold = true;
                        sht.Range("A" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                        sht.Range("A" + row + ":H" + row).Merge();
                        row = row + 1;
                        sht.Cell("A" + row).Value = "SR NO.";
                        sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Arial";
                        sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 10;
                        sht.Range("A" + row + ":A" + row).Style.Font.Bold = true;
                        sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("B" + row).Value = "BRAND";
                        sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Arial";
                        sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 10;
                        sht.Range("B" + row + ":B" + row).Style.Font.Bold = true;
                        sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("C" + row).Value = "COLORNAME";
                        sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                        sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 10;
                        sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                        sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("D" + row).Value = "UoM";
                        sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                        sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 10;
                        sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                        sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("E" + row).Value = "PORTION";
                        sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                        sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 10;
                        sht.Range("E" + row + ":E" + row).Style.Font.Bold = true;
                        sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("F" + row).Value = "QUANTITY(GMS PER KG)";
                        sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Arial";
                        sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 10;
                        sht.Range("F" + row + ":F" + row).Style.Font.Bold = true;
                        sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("G" + row).Value = "RATE PER KG.";
                        sht.Range("G" + row + ":G" + row).Style.Font.FontName = "Arial";
                        sht.Range("G" + row + ":G" + row).Style.Font.FontSize = 10;
                        sht.Range("G" + row + ":G" + row).Style.Font.Bold = true;
                        sht.Range("G" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        sht.Cell("H" + row).Value = "COST";
                        sht.Range("H" + row + ":H" + row).Style.Font.FontName = "Arial";
                        sht.Range("H" + row + ":H" + row).Style.Font.FontSize = 10;
                        sht.Range("H" + row + ":H" + row).Style.Font.Bold = true;
                        sht.Range("H" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        using (var a = sht.Range("A" + row + ":H" + row))
                        {
                            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        }
                        row = row + 1;
                    }
                }
                if (ds.Tables[3] != null)
                {
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[3].Rows)
                        {
                            sht.Cell("A" + row).Value = dr["SRNO"].ToString();
                            sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Arial";
                            sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 10;
                            //sht.Range("A"+row+":A"+row).Style.Font.Bold = true;
                            sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("B" + row).Value = dr["BRANDNAME"].ToString();
                            sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Arial";
                            sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 10;
                            //sht.Range("B"+row+":B"+row).Style.Font.Bold = true;
                            sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("C" + row).Value = dr["SHADECOLORNAME"].ToString();
                            sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                            sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 10;
                            //sht.Range("C"+row+":C"+row).Style.Font.Bold = true;
                            sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("D" + row).Value = dr["UnitName"].ToString();
                            sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                            sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 10;
                            //sht.Range("D"+row+":D"+row).Style.Font.Bold = true;
                            sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("E" + row).Value = dr["portion"].ToString();
                            sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                            sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 10;
                            // sht.Range("E"+row+":E"+row).Style.Font.Bold = true;
                            sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("F" + row).Value = dr["qty"].ToString();
                            sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Arial";
                            sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 10;
                            // sht.Range("F"+row+":F"+row).Style.Font.Bold = true;
                            sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("G" + row).Value = dr["rate"].ToString();
                            sht.Range("G" + row + ":G" + row).Style.Font.FontName = "Arial";
                            sht.Range("G" + row + ":G" + row).Style.Font.FontSize = 10;
                            //sht.Range("G" + row + ":G" + row).Style.Font.Bold = true;
                            sht.Range("G" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            sht.Cell("H" + row).Value = dr["cost"].ToString();
                            sht.Range("H" + row + ":H" + row).Style.Font.FontName = "Arial";
                            sht.Range("H" + row + ":H" + row).Style.Font.FontSize = 10;
                            //sht.Range("H"+row+":H"+row).Style.Font.Bold = true;
                            sht.Range("H" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            SUBTOTALSTUFF = SUBTOTALSTUFF + Convert.ToDecimal(dr["cost"]);
                            using (var a = sht.Range("A" + row + ":H" + row))
                            {
                                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            }
                            row = row + 1;
                        }
                    }
                }
                row = row + 1;
                sht.Cell("E" + row).Value = "SUBTOTAL : ";
                sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 12;
                sht.Range("E" + row + ":E" + row).Style.Font.Bold = true;
                sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E" + row + ":E" + row).Merge();
                sht.Cell("H" + row).Value = SUBTOTALSTUFF;
                sht.Range("H" + row + ":H" + row).Style.Font.FontName = "Arial";
                sht.Range("H" + row + ":H" + row).Style.Font.FontSize = 12;
                sht.Range("H" + row + ":H" + row).Style.Font.Bold = true;
                sht.Range("H" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H" + row + ":H" + row).Merge();

                row=row+2;

                 sht.Cell("E" + row).Value = "GRANDTOTAL : ";
                sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 12;
                sht.Range("E" + row + ":E" + row).Style.Font.Bold = true;
                sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E" + row + ":E" + row).Merge();
                sht.Cell("H" + row).Value =SUBTOTALPRE + SUBTOTALSTUFF+SUBTOTALCHEM;
                sht.Range("H" + row + ":H" + row).Style.Font.FontName = "Arial";
                sht.Range("H" + row + ":H" + row).Style.Font.FontSize = 12;
                sht.Range("H" + row + ":H" + row).Style.Font.Bold = true;
                sht.Range("H" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H" + row + ":H" + row).Merge();
                string Path = "";
                string Pathpdf = "";
                string Fileextension = "xlsx";
                string filename = "RECIPE_SUMMARY." + Fileextension + "";
                Path = Server.MapPath("~/InvoiceExcel/" + filename);
                xapp.SaveAs(Path);

                xapp.Dispose();


                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                //   Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();

                //*************    
            }
            else
            {
                lblmsg.Text = "No records found for this combination.";
            }
        }
        catch (Exception)
        {

            throw;
        }


    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        // Confirms that an HtmlForm control is rendered for the
        // specified ASP.NET server control at run time.
    }

}