using IExpro.Core.Entity;
using IExpro.Core.Interfaces.Repository;
using IExpro.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ICommonRepository
/// </summary>
public interface ICommonRepository : IGenericRepository<FormName>
{
    IEnumerable<SelectList> GetCustomerList(int clientId);
    IEnumerable<SelectList> GetDocTypeList(int clientId);
}