using IExpro.Core.Interfaces.Repository;
using IExpro.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace IExpro.Infrastructure.Services
{
    /// <summary>
    /// Summary description for DocumentService
    /// </summary>
    public class DocumentService : IDocumentService
    {
        private IDocumentRepository DocRepo { get; set; }
        private IUnitOfWork IU { get; set; }
        public DocumentService(IUnitOfWork _IU)
        {
            this.IU = _IU;
            this.DocRepo = IU.DocRepo;
        }
        public IEnumerable<dynamic> GetDocumentList()
        {
            return this.DocRepo.GetDocumentList();
        }
        public string GetDocument(int DocumentId)
        {
            return this.DocRepo.GetDocument(DocumentId);
        }

    }
}