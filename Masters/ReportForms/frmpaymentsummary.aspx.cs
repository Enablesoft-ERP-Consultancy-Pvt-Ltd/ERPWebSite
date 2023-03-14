using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_frmpaymentsummary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
            string str = @"select CompanyId,CompanyName From Companyinfo CI where MasterCompanyid=" + Session["varcompanyId"] + @" order by CompanyId
                           select Distinct IM.ITEM_ID,IM.ITEM_NAME From Item_Master IM inner Join CategorySeparate cs on IM.CATEGORY_ID=cs.Categoryid and cs.id=0 order by ITEM_NAME
                           select PROCESS_NAME_ID,PROCESS_NAME From PROCESS_NAME_MASTER Where ProcessType=1 order by PROCESS_NAME";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDQtype, ds, 1, true, "---Plz Select---");
            UtilityModule.ConditionalComboFillWithDS(ref DDprocess, ds, 2, true, "---Plz Select---");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
            txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (Session["varcompanyId"].ToString() == "9")
            {
                TDsrno.Visible = true;
            }
        }
    }
    protected void DDprocess_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select EI.EmpId,EI.EmpName From Empinfo Ei inner Join EmpProcess EP on EI.EmpId=EP.EmpId Where EP.ProcessId=" + DDprocess.SelectedValue + " order by EmpName";
        UtilityModule.ConditionalComboFill(ref DDWeaver, str, true, "---Plz Select---");
    }
    protected void DDQtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQuality();
    }
    protected void FillQuality()
    {
        string str = @"select Distinct Q.QualityId,Q.QualityName From ITEM_MASTER IM inner Join Quality Q on Q.Item_Id=IM.ITEM_ID inner Join CategorySeparate cs on IM.CATEGORY_ID=cs.Categoryid
                       and cs.id=0 ";
        if (DDQtype.SelectedIndex > 0)
        {
            str = str + "  and IM.Item_id=" + DDQtype.SelectedValue;
        }
        str = str + "  order by QualityName";
        UtilityModule.ConditionalComboFill(ref DDQuality, str, true, "---Plz Select---");
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

    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDesign();
        FillSize();
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillColor();
    }
    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void Chkmtrsize_CheckedChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        int Dateflag = 0;
        if (ChkselectDate.Checked == true)
        {
            Dateflag = 1;
        }
        string str = "select *," + Dateflag + " as Dateflag,'" + txtfromDate.Text + "' as FromDate,'" + txttodate.Text + "' as ToDate from VIEW_PROCESS_HISSAB_StockNo Where Companyid=" + DDCompany.SelectedValue + " and Commpaymentflag=0";
        if (DDprocess.SelectedIndex > 0)
        {
            str = str + "  and Process_Name_id=" + DDprocess.SelectedValue;
        }
        if (DDWeaver.SelectedIndex > 0)
        {
            str = str + "  and Empid=" + DDWeaver.SelectedValue;
        }
        if (DDQtype.SelectedIndex > 0)
        {
            str = str + "  and Item_id=" + DDQtype.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + "  and QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + "  and DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + "  and Colorid=" + DDColor.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + "  and Sizeid=" + DDSize.SelectedValue;
        }
        if (ChkselectDate.Checked == true)
        {
            str = str + " And FromDate>='" + txtfromDate.Text + "' and TOdate<='" + txttodate.Text + "'";
        }
        if (TDsrno.Visible == true)
        {
            if (txtsrno.Text != "")
            {
                str = str + "  and Localorder='" + txtsrno.Text.Trim() + "'";
            }
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "Reports/rptpaymentjobsummary.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptpaymentjobsummary.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
}