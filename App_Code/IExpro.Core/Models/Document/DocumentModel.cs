using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace IExpro.Core.Models.Document
{
    /// <summary>
    /// Summary description for DocumentModel
    /// </summary>
    public class DocumentModel : Entity<int>
    {
        public int DocType { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public byte UserType { get; set; }
        public byte PrintType { get; set; }
        
    }
}