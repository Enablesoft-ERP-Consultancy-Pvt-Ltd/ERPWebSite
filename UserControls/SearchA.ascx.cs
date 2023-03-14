using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_SearchA : System.Web.UI.UserControl
{
    public delegate void SendMessageToThePageHandler(object sender, MyEventArgs e);
    public event SendMessageToThePageHandler sendMessageToThePage;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref ddlItem, "select Masterqualityid,Quality from qualitymaster Order by Masterqualityid", true, "Select Item");
            UtilityModule.ConditionalComboFillOnlySelect(ref ddlQuality, true, "Select Quality");
            UtilityModule.ConditionalComboFillOnlySelect(ref ddlDesign, true, "Select Design");
            UtilityModule.ConditionalComboFillOnlySelect(ref ddlColor, true, "Select Color");
            UtilityModule.ConditionalComboFillOnlySelect(ref ddlShape, true, "Select Shape");
            UtilityModule.ConditionalComboFillOnlySelect(ref ddlSize, true, "Select Size");

        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (sendMessageToThePage != null)
        {
            MyEventArgs objArgs = new MyEventArgs(Convert.ToInt32(ddlItem.SelectedValue),Convert.ToInt32(ddlQuality.SelectedValue),Convert.ToInt32(ddlDesign.SelectedValue),Convert.ToInt32(ddlColor.SelectedValue),Convert.ToInt32(ddlShape.SelectedValue),Convert.ToInt32(ddlSize.SelectedValue), true);
            sendMessageToThePage(this, objArgs);
        }
    }

    protected void cmbItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }

    protected void ddlDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDesign.SelectedIndex != -1)
        {
            UtilityModule.ConditionalComboFill(ref ddlColor, "SELECT ColorId,ColorName FROM Color", true, "Select Color");
        }
    }
    protected void ddlShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlShape.SelectedIndex != -1)
        {
            UtilityModule.ConditionalComboFill(ref ddlSize, "select distinct sizeid ,Export_Format from rptsize where shapeid=" + ddlShape.SelectedValue + "  order by Export_Format", true, "Select Size");
        }

    }
    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlItem.SelectedIndex != -1)
        {
            UtilityModule.ConditionalComboFill(ref ddlQuality, "select Qualityid,Qualityname from Quality where Masterqualityid=" + ddlItem.SelectedValue + " order by QualityName", true, "Select Quality");
            UtilityModule.ConditionalComboFill(ref ddlDesign, "select distinct Designid,DesignName from Design Order  by DesignName", true, "Select Design");
            UtilityModule.ConditionalComboFill(ref ddlShape, "select Shapeid,ShapeName from Shape Order by ShapeName", true, "Select Shape");
        }
    }
}
