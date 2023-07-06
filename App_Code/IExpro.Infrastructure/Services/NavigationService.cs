using IExpro.Core.Common;
using IExpro.Core.Interfaces.Repository;
using IExpro.Core.Interfaces.Service;
using IExpro.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IExpro.Infrastructure.Services
{
    public class NavigationService : INavigationService
    {
        private IUnitOfWork IU { get; set; }
        private INavigationRepository NavRepo { get; set; }
        
        public NavigationService(IUnitOfWork _IU)
        {
            this.IU = _IU;
            this.NavRepo = IU.NavRepo;
        }

        public List<MenuModel> GetMenus(int userId, int clientId)
        {
            
            List<MenuModel> menuItemsList = new List<MenuModel>();
            var menuItems = this.NavRepo.GetMenus(userId, clientId);
           
            
            if (menuItems != null)
            {
                menuItemsList = this.NavRepo.LoadMenus(menuItems, 0);
            }
            return menuItemsList;
        }

    }
}
