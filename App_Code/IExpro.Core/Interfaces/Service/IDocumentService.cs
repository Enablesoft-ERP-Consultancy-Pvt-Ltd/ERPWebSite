using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for IDocumentService
/// </summary>
/// 
namespace IExpro.Core.Interfaces.Service
{
    public interface IDocumentService
    {
        IEnumerable<dynamic> GetDocumentList();
        string GetDocument(int DocumentId);
    }
}