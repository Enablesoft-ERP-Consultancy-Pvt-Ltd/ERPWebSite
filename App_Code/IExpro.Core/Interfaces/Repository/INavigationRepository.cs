using IExpro.Core.Entity;
using IExpro.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IExpro.Core.Interfaces.Repository
{

    public interface INavigationRepository : IGenericRepository<FormName>
    {
        IEnumerable<MenuModel> GetMenus(int userId, int clientId);
        List<MenuModel> LoadMenus(IEnumerable<MenuModel> menus, int ParentId);


    }
}
