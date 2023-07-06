using IExpro.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace IExpro.Core.Interfaces.Repository
{
    public interface IDocumentRepository : IGenericRepository<FormName>
    {
        IEnumerable<dynamic> GetDocumentList();
        string GetDocument(int DocumentId);
    }
}