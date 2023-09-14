using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace IExpro.Core.Interfaces.Service
{
    /// <summary>
    /// Summary description for IProcessService
    /// </summary>
    public interface IProcessService
    {
        IEnumerable<ProcessModel> GetProcessList(int CompanyId);
        IEnumerable<ItemProcessModel> GetItemProcessList(int CompanyId, int ItemId, int DesignId, int QualityId);
    }
}