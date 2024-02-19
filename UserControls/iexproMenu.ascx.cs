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

public partial class UserControls_iexproMenu : System.Web.UI.UserControl
{
    INavigationService NavSrv;
    List<MenuModel> models = new List<MenuModel>();
    public UserControls_iexproMenu(INavigationService _NavSrv)
    {
        NavSrv = _NavSrv;
    }
    public UserControls_iexproMenu()
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
            int clientId = Convert.ToInt32(Session["varMasterCompanyIDForERP"]);
            this.models = this.NavSrv.GetMenus(userId, clientId);
            this.rptMenu.DataSource = models;
            this.rptMenu.DataBind();
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
                        sb.Append("<ul id='" + parentId + "' class='dropdown-menu'>");
                        foreach (var item in _menu.MenuList)
                        {
                            string childId = item.MenuId.ToString();
                            string childTitle = item.MenuName.ToString();
                            IEnumerable<MenuModel> childRow = item.MenuList;
                            item.MenuUrl = this.GetItemUrl(item.MenuUrl);

                            if (childRow.Count() > 0)
                            {
                                sb.Append("<li class='dropdown-submenu'>");
                                sb.Append("<a data-bs-toggle='dropdown' class='dropdown-item dropdown-toggle' href='" + item.MenuUrl + "'>");
                                sb.Append(item.MenuName.FirstCharToUpper() + "</a>");
                                CreateChild(sb, childId, childTitle, childRow);
                                sb.Append("</li>");
                            }
                            else
                            {
                                sb.Append("<li>");
                                sb.Append("<a class='dropdown-item' href='" + item.MenuUrl + "'>");
                                sb.Append(item.MenuName.FirstCharToUpper() + "</a>");
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
            sb.Append("<ul id='" + parentTitle + "' class='dropdown-menu'>");
            foreach (var item in parentRows)
            {
                string childId = item.MenuId.ToString();
                string childTitle = item.MenuName.ToString();
                IEnumerable<MenuModel> childRow = item.MenuList;
                item.MenuUrl = this.GetItemUrl(item.MenuUrl);

                if (childRow.Count() > 0)
                {
                    sb.Append("<li class='dropdown-submenu'>");
                    sb.Append("<a data-bs-toggle='dropdown' class='dropdown-item dropdown-toggle' href='" + item.MenuUrl + "'>");
                    sb.Append(item.MenuName.FirstCharToUpper() + "</a>");
                    CreateChild(sb, childId, childTitle, childRow);
                    sb.Append("</li>");
                }
                else
                {
                    sb.Append("<li>");
                    sb.Append("<a class='dropdown-item' href='" + item.MenuUrl + "'>");
                    sb.Append(item.MenuName.FirstCharToUpper() + "</a>");
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
                input = input.Replace("\\", "//");
                input = input.Replace("../", "~/");
                
                    
                    
                   


                return Page.ResolveUrl(input);


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