using AjaxControlToolkit.HTMLEditor.ToolbarButton;
using IExpro.Core.Interfaces.Repository;
using IExpro.Core.Interfaces.Service;
using IExpro.Core.Models;
using IExpro.Core.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IExpro.Infrastructure.Services
{
    public class CommonService : ICommonService
    {
        private ICommonRepository CommRepo { get; set; }
        private IUnitOfWork IU { get; set; }
        public CommonService(IUnitOfWork _IU)
        {
            this.IU = _IU;
            this.CommRepo = IU.CommRepo;
        }

        public IEnumerable<SelectList> GetCustomerList(int clientId)
        {
            return this.CommRepo.GetCustomerList(clientId);

        }
        public IEnumerable<SelectList> GetDocTypeList(int clientId)
        {
            return this.CommRepo.GetDocTypeList(clientId);
        }
        public IEnumerable<SelectList> GetItemList(int CompanyId)
        {
            return this.CommRepo.GetItemList(CompanyId);
        }
        public IEnumerable<SelectList> GetQualityList(int ItemId)
        {
            return this.CommRepo.GetQualityList(ItemId);
        }
        public IEnumerable<SelectList> GetDesignList(int QualityId)
        {
            return this.CommRepo.GetDesignList(QualityId);
        }

    }
}
