using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

using Dapper;
using IExpro.Core.Entity;
using IExpro.Core.Interfaces.Common;
using IExpro.Infrastructure.Repository;

/// <summary>
/// Summary description for ProcessRepository
/// </summary>
public class ProcessRepository : GenericRepository<FormName>, IProcessRepository
{
    public ProcessRepository(IDataContext context)
       : base(context)
    {
    }
    public IExproContext Context
    {
        get { return base.entities as IExproContext; }
    }

    public IEnumerable<ItemProcessModel> GetItemProcessList(int CompanyId, int ItemId, int? DesignId, int? QualityId)
    {
        using (IDbConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
        {
            string sqlQuery = @"Select PNM.process_Name_id ProcessId,PNM.Process_Name ProcessType,IP.ProcessType,IP.SeqNo from 
Process_name_Master PNM Inner Join Item_Process IP on PNM.PROCESS_NAME_ID=IP.processId 
Where IP.MasterCompanyId=@CompanyId and IP.Itemid=@Itemid and IP.DESIGNID=IsNUll(@DESIGNID,IP.DESIGNID) and IP.QualityId= IsNUll(@QualityId,IP.QualityId)
order by IP.SeqNo";
            return (conn.Query<ItemProcessModel>(sqlQuery, new { @CompanyId = CompanyId, @ItemId = ItemId, @DesignId = DesignId, @QualityId = QualityId }));
        }
    }


    public IEnumerable<ProcessModel> GetProcessList(int CompanyId)
    {
        using (IDbConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
        {
            string sqlQuery = @"Select PNM.process_Name_id ProcessId,PNM.Process_Name ProcessType,PNM.ShortName from 
Process_name_Master PNM Where IP.MasterCompanyId=@CompanyId 
order by PNM.process_Name_id";
            return (conn.Query<ProcessModel>(sqlQuery, new { @CompanyId = CompanyId }));
        }
    }


}