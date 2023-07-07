using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using IExpro.Core.Entity;
using IExpro.Core.Interfaces.Common;
using DocumentFormat.OpenXml.EMMA;
using System.Data.Entity.Infrastructure;
using IExpro.Infrastructure.Repository;
using IExpro.Core.Interfaces.Repository;
using IExpro.Core.Models;

namespace IExpro.Infrastructure.Repository
{
    public class CommonRepository : GenericRepository<FormName>, ICommonRepository
    {

        public CommonRepository(IDataContext context)
              : base(context)
        {
        }
        public IExproContext Context
        {
            get { return base.entities as IExproContext; }
        }


        public IEnumerable<SelectList> GetCustomerList(int clientId)
        {
            IEnumerable<SelectList> result = null;
            string sqlQuery = @"Select CustomerId,CustomerCode+'/'+CompanyName  as  CustomerName  from customerinfo 
Where MasterCompanyId=@CompanyId
Order by CustomerCode";
            using (SqlConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
                param[0].Direction = ParameterDirection.Input;
                param[0].Value = clientId;
                var dataSet = SqlHelper.ExecuteDataset(conn, CommandType.Text, sqlQuery, param);
                result = dataSet.Tables[0].AsEnumerable().Select(dataRow => new SelectList
                {
                    ItemId = dataRow.Field<int>("CustomerId"),
                    ItemName = dataRow.Field<string>("CustomerName")
                });
            }
            return result;
        }

        public IEnumerable<SelectList> GetDocTypeList(int clientId)
        {
            IEnumerable<SelectList> result = null;
            string sqlQuery = @"SELECT DocumentId,DocumentType From tblDocumentType
Order By DocumentType";
            using (SqlConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {
                var dataSet = SqlHelper.ExecuteDataset(conn, CommandType.Text, sqlQuery);
                result = dataSet.Tables[0].AsEnumerable().Select(dataRow => new SelectList
                {
                    ItemId = dataRow.Field<int>("DocumentId"),
                    ItemName = dataRow.Field<string>("DocumentType")
                });
            }
            return result;
        }



    }

}