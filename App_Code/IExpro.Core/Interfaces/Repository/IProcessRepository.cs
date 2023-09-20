using IExpro.Core.Entity;
using IExpro.Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace IExpro.Core.Interfaces.Repository
{
    /// <summary>
    /// Summary description for IProcessRepository
    /// </summary>
    public interface IProcessRepository : IGenericRepository<FormName>
    {
        IEnumerable<ProcessModel> GetProcessList(int CompanyId);
        IEnumerable<ItemProcessModel> GetItemProcessList(int CompanyId, int ItemId, int? DesignId, int? QualityId);
    }
}