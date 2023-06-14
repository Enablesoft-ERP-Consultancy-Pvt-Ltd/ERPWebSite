using IExpro.Core.Entity;
using IExpro.Core.Interfaces.Common;
using IExpro.Core.Models;
using IExpro.Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IExpro.Infrastructure.Repository
{
    public class NavigationRepository : GenericRepository<FormName>, INavigationRepository
    {

        public NavigationRepository(IDataContext context)
          : base(context)
        {
        }
        public IExproContext Context
        {
            get { return base.entities as IExproContext; }
        }

        public IEnumerable<MenuModel> GetMenus(int userId, int clientId)
        {

            try
            {
                var data = from nav in Context.FormNames.Where(x => x.Isvisible == 1)
                           join user in Context.UserRights.Where(x => x.userid == userId && x.companyId == clientId) on nav.MenuID equals user.MenuId
                           select new MenuModel
                           {
                               MenuId = nav.MenuID,
                               ParentId = nav.ParentId.HasValue ? nav.ParentId.Value : 0,
                               MenuName = nav.DisplayName,
                               MenuUrl =  nav.NavigateURL.Replace("../","~/")
                               //MenuIcon = nav.MenuIcon,                
                               //OrderNo = nav.MenuOrderNo.HasValue ? nav.MenuOrderNo.Value : 0.0m
                           };
                return data;

            }
            catch (Exception ex)
            {
                throw ex;


            }




        }

        public List<MenuModel> LoadMenus(IEnumerable<MenuModel> menus, int ParentId)
        {
            List<MenuModel> nodes = new List<MenuModel>();
            nodes = (from node in menus
                     where node.ParentId == ParentId
                     orderby node.OrderNo
                     select new MenuModel
                     {
                         MenuId = node.MenuId,
                         ParentId = node.ParentId,
                         OrderNo = node.OrderNo,
                         MenuName = node.MenuName,
                         MenuUrl = node.MenuUrl,
                         MenuList = (ParentId != node.MenuId ?
                              LoadMenus(menus, node.MenuId) :
                     new List<MenuModel>())
                     }).ToList();
            return nodes;
        }





    }




}

