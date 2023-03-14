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

public partial class Masters_Process_FrmRemoveQCDefectProcessJobWise : System.Web.UI.Page
{
    public static string Export = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName                           
                           select PROCESS_NAME_ID,PROCESS_NAME From PROCESS_NAME_MASTER Where MasterCompanyid=" + Session["varcompanyid"] + @" and ProcessType=1 order by PROCESS_NAME
                            select CATEGORY_ID,CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM JOIN CategorySeparate CS ON ICM.CATEGORY_ID=CS.Categoryid and CS.id=0";


            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");           
            UtilityModule.ConditionalComboFillWithDS(ref DDprocess, ds, 1, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, "---Plz Select---");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            
        }
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDQtype, "select IM.ITEM_ID,IM.ITEM_NAME From Item_Master IM Where IM.category_id=" + DDCategory.SelectedValue + " order by ITEM_NAME", true, "--Plz Select--");
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
                  " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
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
                        Trcolor.Visible = false;
                        break;
                    case "5":
                        Trsize.Visible = false;
                        break;
                    case "6":
                        Trshadecolor.Visible = false;
                        break;
                }
            }
        }

    }
    protected void DDQtype_SelectedIndexChanged(object sender, EventArgs e)
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
        if (DDQtype.SelectedIndex > 0)
        {
            str = str + "  and IM.Item_id=" + DDQtype.SelectedValue;
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
        if (DDQtype.SelectedIndex > 0)
        {
            str = str + "  and vf.Item_id=" + DDQtype.SelectedValue;
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
        if (DDQtype.SelectedIndex > 0)
        {
            str = str + "  and vf.Item_id=" + DDQtype.SelectedValue;
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
        if (DDQtype.SelectedIndex > 0)
        {
            str = str + "  and vf.Item_id=" + DDQtype.SelectedValue;
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
        if (DDQtype.SelectedIndex > 0)
        {
            str = str + "  and vf.Item_id=" + DDQtype.SelectedValue;
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

    protected void txtfromdate_TextChanged(object sender, EventArgs e)
    {
        if (ChkForSelectRemoveDate.Checked == true)
        {
            txttodate.Text = txtfromdate.Text;
            calto.StartDate = Convert.ToDateTime(txtfromdate.Text);
            calto.EndDate = Convert.ToDateTime(txtfromdate.Text);
            CalendarExtender1.StartDate = Convert.ToDateTime(txtfromdate.Text);
            CalendarExtender1.EndDate = DateTime.Today.AddYears(1);
            txtRemoveDefectDate.Text = txtfromdate.Text;
        }
       
    }
    protected void ChkForSelectRemoveDate_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForSelectRemoveDate.Checked == true)
        {
            txttodate.Enabled = false;
            txttodate.Text = txtfromdate.Text;
            TRRemoveDefectDate.Visible = true;
            txtRemoveDefectDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            CalendarExtender1.StartDate = Convert.ToDateTime(txtfromdate.Text);
            CalendarExtender1.EndDate = DateTime.Today.AddYears(1);
            //CalendarExtender1.EndDate = DateTime.Today.AddMonths(1);
        }
        else
        {
            txttodate.Enabled = true;
            TRRemoveDefectDate.Visible = false;
            txtRemoveDefectDate.Text = "";
        }
    }


    protected void btnRemoveQCDefect_Click(object sender, EventArgs e)
    {
        string str = "";

        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " and Vf.Category_id=" + DDCategory.SelectedValue;
        }
        if (DDQtype.SelectedIndex > 0)
        {
            str = str + " and Vf.Item_id=" + DDQtype.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and Vf.Qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
        }
        //if (DDColor.SelectedIndex > 0)
        //{
        //    str = str + " and vf.Colorid=" + DDColor.SelectedValue;
        //}
        //if (DDSize.SelectedIndex > 0)
        //{
        //    str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
        //}

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {

            //******
            SqlCommand cmd = new SqlCommand("PRO_REMOVEQCDEFECTPROCESSJOBWISE", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;           
            cmd.Parameters.AddWithValue("@Companyid", DDcompany.SelectedValue);
            cmd.Parameters.AddWithValue("@ProcessId", DDprocess.SelectedValue );
            cmd.Parameters.AddWithValue("@FromDate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@ToDate", txttodate.Text);
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyId"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
            cmd.Parameters.AddWithValue("@UserType", Session["usertype"]);

            cmd.Parameters.AddWithValue("@ChkForSelectRemoveDate", ChkForSelectRemoveDate.Checked==true ?"1" : "0");
            cmd.Parameters.AddWithValue("@RemoveDefectDate", ChkForSelectRemoveDate.Checked == true ? txtRemoveDefectDate.Text : "01-Jan-1900");

            cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@where", str);           

            cmd.ExecuteNonQuery();
            if (cmd.Parameters["@msg"].Value.ToString() != "") //IF DATA NOT FOUND
            {
                lblmessage.Text = cmd.Parameters["@msg"].Value.ToString();
                Tran.Rollback();
            }
            else
            {
                lblmessage.Text = "QC Defects Removed Successfully.";
                Tran.Commit();              
            }
            //******

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }      
            

    }

   
    
    
}