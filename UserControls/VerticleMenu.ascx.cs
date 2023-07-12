using IExpro.Core.Interfaces.Service;
using IExpro.Core.Models;
using IExpro.Core.Common;
using IExpro.Infrastructure.Repository;
using IExpro.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class UserControls_VerticleMenu : System.Web.UI.UserControl
{
    INavigationService NavSrv;
    List<MenuModel> models = new List<MenuModel>();
    public UserControls_VerticleMenu(INavigationService _NavSrv)
    {
        NavSrv = _NavSrv;
    }
    public UserControls_VerticleMenu()
    {
        this.NavSrv = new NavigationService(new UnitOfWork());
    }






    public string FirstCharToUpper(string input)
    {
        switch (input)
        {
            //case null: throw new ArgumentNullException(nameof(input));
            //case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
            default: return input[0].ToString().ToUpper() + input.Substring(1).ToLower();
        }
    }





    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            int userId = Convert.ToInt32(Session["varuserid"].ToString());
            int clientId = Convert.ToInt32(Session["varCompanyId"]);
            this.models = this.NavSrv.GetMenus(userId, clientId);
            this.rptCategories.DataSource = models;
            this.rptCategories.DataBind();
        }
    }
    protected void rptMenu_OnItemBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (this.models != null)
                {
                    MenuModel drv = e.Item.DataItem as MenuModel;
                    string parentId = drv.MenuId.ToString();
                    string parentTitle = drv.MenuName.ToString();
                    MenuModel _menu = this.models.Where(x => x.MenuId == drv.MenuId).FirstOrDefault();
                    if (_menu.MenuList.Count() > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<ul id='" + parentId + "' class='nav nav-second-level'>");
                        foreach (var item in _menu.MenuList)
                        {
                            string childId = item.MenuId.ToString();
                            string childTitle = item.MenuName.ToString();
                            IEnumerable<MenuModel> childRow = item.MenuList;
                            item.MenuUrl = this.GetItemUrl(item.MenuUrl);
                            sb.Append("<li>");
                            if (childRow.Count() > 0)
                            {
                                sb.Append("<a class='second-level-heading' href='" + item.MenuUrl + "'>");
                                sb.Append("<span class='submenu-title'>" + item.MenuName.FirstCharToUpper() + "</span>");
                                sb.Append("<span class='fa arrow  text-white'></span></a>");
                                CreateChild(sb, childId, childTitle, childRow);
                                sb.Append("</li>");
                            }
                            else
                            {
                                sb.Append("<a href='" + item.MenuUrl + "'>");
                                sb.Append("<span class='submenu-title'>" + item.MenuName.FirstCharToUpper() + "</span>");
                                sb.Append("</a>");
                                sb.Append("</li>");
                            }
                        }
                        sb.Append("</ul>");
                        (e.Item.FindControl("ltrlSubMenu") as Literal).Text = sb.ToString();
                    }
                }
            }
        }
    }

    private StringBuilder CreateChild(StringBuilder sb, string parentId, string parentTitle, IEnumerable<MenuModel> parentRows)
    {
        if (parentRows.Count() > 0)
        {
            sb.Append("<ul id='" + parentTitle + "' class='nav nav-third-level'>");
            foreach (var item in parentRows)
            {
                string childId = item.MenuId.ToString();
                string childTitle = item.MenuName.ToString();
                IEnumerable<MenuModel> childRow = item.MenuList;
                item.MenuUrl = this.GetItemUrl(item.MenuUrl);
                if (childRow.Count() > 0)
                {
                    sb.Append("<li><a class='second-level-heading' href='" + item.MenuUrl + "'>");
                    sb.Append("<span class='submenu-title'>" + item.MenuName.FirstCharToUpper() + "</span>");
                    sb.Append("<span class='fa arrow  text-white'></span></a>");
                    CreateChild(sb, childId, childTitle, childRow);
                    sb.Append("</li>");
                }
                else
                {
                    sb.Append("<li><a href='" + item.MenuUrl + "'>");
                    sb.Append("<span class='submenu-title'>" + item.MenuName.FirstCharToUpper() + "</span>");
                    sb.Append("</a>");
                    sb.Append("</li>");
                }
            }
            sb.Append("</ul>");
        }
        return sb;
    }




    public string GetItemUrl(string input)
    {
        try
        {
            if (!string.IsNullOrEmpty(input))
            {


                return Page.ResolveUrl(input.Replace("../", "~/"));


            }
            else
            {
                return "#";
            }
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }





}