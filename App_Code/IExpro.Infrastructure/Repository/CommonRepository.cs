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
using DocumentFormat.OpenXml.Drawing.Charts;
using Dapper;

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


        public IEnumerable<SelectList> GetItemList(int CompanyId)
        {

            using (IDbConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {
                string sqlQuery = @"Select x.ITEM_ID ItemId, x.Item_Name ItemName from Item_master x Where x.MasterCompanyId = @CompanyId";
                return (conn.Query<SelectList>(sqlQuery, new { @CompanyId = CompanyId }));
            }
        }
        public IEnumerable<SelectList> GetQualityList(int ItemId)
        {
            using (IDbConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {
                string sqlQuery = @"Select x.QualityId ItemId, x.QualityName ItemName From Quality x Where x.Item_Id= @ItemId order by QualityName";
                return (conn.Query<SelectList>(sqlQuery, new { @ItemId = ItemId }));
            }

        }

        public IEnumerable<SelectList> GetDesignList(int QualityId)
        {

            using (IDbConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {
                string sqlQuery = @"SELECT distinct D.designId ItemId,D.DesignName ItemName FROM ITEM_PARAMETER_MASTER IPM(Nolock) 
Inner JOIN Design D(Nolock) ON D.DesignId = IPM.DESIGN_ID   
Where IPM.QUALITY_ID=@QualityId";

                return (conn.Query<SelectList>(sqlQuery, new { @QualityId = QualityId }));
            }
        }



    }

}