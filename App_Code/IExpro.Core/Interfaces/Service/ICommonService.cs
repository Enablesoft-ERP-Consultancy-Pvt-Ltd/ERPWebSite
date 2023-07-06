using IExpro.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IExpro.Core.Interfaces.Service
{
    public interface ICommonService
    {

        IEnumerable<SelectList> GetCustomerList(int clientId);
        IEnumerable<SelectList> GetDocTypeList(int clientId);
        //List<MenuModel> GetCompanyList();
        //List<MenuModel> GetCustomerList();
        //List<MenuModel> GetSessionList();
        //List<MenuModel> GetDocTypeList();
        //List<MenuModel> GetPrintTypeList();
    }
}
