using IExpro.Core.Entity;
using IExpro.Core.Interfaces.Common;
using IExpro.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using IExpro.Core.Interfaces.Repository;

namespace IExpro.Infrastructure.Repository
{
    /// <summary>
    /// Summary description for DocumentRepository
    /// </summary>

    public class DocumentRepository : GenericRepository<FormName>, IDocumentRepository
    {

        public DocumentRepository(IDataContext context)
              : base(context)
        {
        }
        public IExproContext Context
        {
            get { return base.entities as IExproContext; }
        }

        public IEnumerable<dynamic> GetDocumentList()
        {
            IEnumerable<dynamic> result = null;
            string sqlQuery = @"SELECT x.XSLTId,y.DocumentId,y.DocumentType From tblXSLTDetails x inner Join tblDocumentType y on x.DocumentType=y.DocumentId
Order By y.DocumentId";
            using (SqlConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {
                var dataSet = SqlHelper.ExecuteDataset(conn, CommandType.Text, sqlQuery);
                result = dataSet.Tables[0].AsEnumerable().Select(dataRow => new
                {
                    XsltId = dataRow.Field<int>("XSLTId"),
                    DocumentId = dataRow.Field<int>("DocumentId"),
                    DocumentType = dataRow.Field<string>("DocumentType")
                });
            }
            return result;
        }

        public string GetDocument(int DocumentId)
        {
            string result = string.Empty;
            string sqlQuery = @"SELECT XSLTText From tblXSLTDetails Where XSLTId=@DocId";
            using (SqlConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@DocId", SqlDbType.Int);
                param[0].Direction = ParameterDirection.Input;
                param[0].Value = DocumentId;
                result = (string)SqlHelper.ExecuteScalar(conn, CommandType.Text, sqlQuery, param);
            }
            return result;
        }


    }

}