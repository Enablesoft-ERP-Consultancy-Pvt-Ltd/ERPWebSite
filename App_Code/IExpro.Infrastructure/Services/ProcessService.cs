using IExpro.Core.Interfaces.Repository;
using IExpro.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProcessService
/// </summary>
public class ProcessService: IProcessService
{
    private IUnitOfWork IU { get; set; }
    

    public ProcessService(IUnitOfWork _IU)
    {
        this.IU = _IU;
    }


    public IEnumerable<ProcessModel> GetProcessList(int CompanyId)
    {
        return this.IU.ProcRepo.GetProcessList(CompanyId);
    }

     public IEnumerable<ItemProcessModel> GetItemProcessList(int CompanyId, int ItemId, int DesignId, int QualityId)
    {
        return this.IU.ProcRepo.GetItemProcessList(CompanyId, ItemId, DesignId, QualityId);
    }











}