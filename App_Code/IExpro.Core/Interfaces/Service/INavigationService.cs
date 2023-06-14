using IExpro.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IExpro.Core.Interfaces.Service
{
    public interface INavigationService
    {
        List<MenuModel> GetMenus(int userId, int clientId);
    }
}
