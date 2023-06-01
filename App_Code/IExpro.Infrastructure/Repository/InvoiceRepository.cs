using IExpro.Core.Common;
using IExpro.Core.Entity;
using IExpro.Core.Interfaces.Common;
using IExpro.Core.Interfaces.Repository;
using IExpro.Core.Models.Account;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Xml.Linq;


namespace IExpro.Infrastructure.Repository
{

    public class InvoiceRepository : GenericRepository<INVOICE>, IInvoiceRepository
    {
        public InvoiceRepository(IDataContext context)
          : base(context)
        {
        }
        public IExproContext Context
        {
            get { return base.entities as IExproContext; }
        }


        //public dynamic GetInvoiceDetail(int _invoiceId)
        //{
        //    try
        //    {
        //        SqlParameter flag = new SqlParameter("@flag", 1);
        //        SqlParameter invoiceId = new SqlParameter("@InvoiceId", _invoiceId);

        //        var result = Context.Database.ExecuteSqlCommand("Invoiceproc @flag, @InvoiceId", flag, invoiceId);

        //        if (result != null)
        //        {
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;

        //    }
        //}





        public string GetXSLTDetail(int clientId, short docType, int userId, short userType)
        {


            //var resu2lt = (from xltMap in Context.tblXSLTClientMappings
            //               join xlt in Context.tblXSLTDetails on xltMap.XSLTId equals xlt.XSLTId
            //               where (xltMap.UserType == userType && xltMap.ClientId == clientId && (xltMap.UserId == userId || xltMap.UserId == null))
            //               select new { UserId = (xltMap.UserId.HasValue ? xltMap.UserId.Value : 0), xlt.XSLTText }).ToList();



            var result = (from xltMap in Context.tblXSLTClientMappings
                          join xlt in Context.tblXSLTDetails on xltMap.XSLTId equals xlt.XSLTId
                          where (xltMap.UserType == userType && xltMap.ClientId == clientId && xlt.DocumentType == docType && xltMap.UserId == userId)
                          select new { UserId = (xltMap.UserId.HasValue ? xltMap.UserId.Value : 0), xlt.XSLTText }).
                          Union(from xltMap in Context.tblXSLTClientMappings
                                join xlt in Context.tblXSLTDetails on xltMap.XSLTId equals xlt.XSLTId
                                where (xltMap.ClientId == clientId && xlt.DocumentType == docType && xltMap.UserId == null)
                                select new { UserId = (xltMap.UserId.HasValue ? xltMap.UserId.Value : 0), xlt.XSLTText });








            if (result.Where(x => x.UserId == userId).Count() > 0)
            {
                return result.Where(x => x.UserId == userId).SingleOrDefault().XSLTText;
            }
            else
            {
                return result.Where(x => x.UserId == 0).SingleOrDefault().XSLTText;
            }

        }


        public XElement GetInvoiceDetail(int _invoiceId)
        {
            string result = string.Empty;
            using (SqlCommand command = (SqlCommand)Context.Database.Connection.CreateCommand())
            {
                command.CommandText = "Invoiceproc";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@flag",
                    Direction = ParameterDirection.Input,
                    SqlDbType = SqlDbType.Int,
                    Value = 1
                });
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@InvoiceId",
                    Direction = ParameterDirection.Input,
                    SqlDbType = SqlDbType.Int,
                    Value = _invoiceId
                });
                Context.Database.Connection.Open();
                using (var readerXl = command.ExecuteXmlReader())
                {
                    while (readerXl.Read())
                    {
                        result = readerXl.ReadOuterXml();
                    }
                }
            }
            if (!string.IsNullOrEmpty(result))
            {
                return XElement.Parse(result);
            }
            else
            {
                return null;
            }

        }


















    }
}
