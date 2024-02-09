using DocumentFormat.OpenXml.Spreadsheet;
using IExpro.Core.Common;
using IExpro.Core.Entity;
using IExpro.Core.Interfaces.Common;
using IExpro.Core.Interfaces.Repository;
using IExpro.Core.Models.Account;
using System;
using System.Collections.Generic;
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





        public string GetXSLTDetail(int clientId, short printType,  short docType, int userId, short userType)
        {
            IEnumerable<dynamic> result = null;
            string sqlQuery = @"select x.XSLTId,y.XSLTText,IsNUll(x.UserId,0) UserId from tblXSLTClientMapping x inner join tblXSLTDetails y on x.XSLTId=y.XSLTId 
Where x.ClientId=@ClientId and IsNUll(x.UserId,@UserId)=@UserId and x.DocumentType=@DocumentType and x.PrintType=@PrintType";
            using (SqlConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@DocumentType", SqlDbType.Int);
                param[0].Direction = ParameterDirection.Input;
                param[0].Value = docType;
                param[1] = new SqlParameter("@UserId", SqlDbType.Int);
                param[1].Direction = ParameterDirection.Input;
                param[1].Value = userId;
                param[2] = new SqlParameter("@ClientId", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Input;
                param[2].Value = clientId;
                param[3] = new SqlParameter("@UserType", SqlDbType.Int);
                param[3].Direction = ParameterDirection.Input;
                param[3].Value = userType;
                param[4] = new SqlParameter("@PrintType", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Input;
                param[4].Value = printType;

                var dataSet = SqlHelper.ExecuteDataset(conn, CommandType.Text, sqlQuery, param);
                result = dataSet.Tables[0].AsEnumerable().Select(dataRow => new
                {
                    XsltId = dataRow.Field<int>("XSLTId"),
                    XSLTText = dataRow.Field<string>("XSLTText"),
                    UserId = dataRow.Field<int>("UserId"),                
                });
            }
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
                Context.Database.Connection.Close();



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
