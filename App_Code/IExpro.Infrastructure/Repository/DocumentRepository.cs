﻿using IExpro.Core.Entity;
using IExpro.Core.Interfaces.Common;
using IExpro.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using IExpro.Core.Interfaces.Repository;
using DocumentFormat.OpenXml.Office2010.Word;
using IExpro.Core.Models.Document;
using DocumentFormat.OpenXml.Wordprocessing;

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
            string sqlQuery = @"SELECT x.XSLTId,x.XSLTSubject,y.DocumentId,y.DocumentType From tblXSLTDetails x inner Join tblDocumentType y on x.DocumentType=y.DocumentId
Order By y.DocumentId";
            using (SqlConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {
                var dataSet = SqlHelper.ExecuteDataset(conn, CommandType.Text, sqlQuery);
                result = dataSet.Tables[0].AsEnumerable().Select(dataRow => new
                {
                    XsltId = dataRow.Field<int>("XSLTId"),
                    Title = dataRow.Field<string>("XSLTSubject"),
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



        public int AddDocument(DocumentModel doc)
        {
            var result = 0;
            string sqlQuery = @"IF NOT EXISTS (SELECT 0 FROM tblXSLTDetails WHERE XSLTSubject=@XSLTSubject and DocumentType=@DocumentType)
BEGIN

INSERT INTO tblXSLTDetails
(DocumentType,XSLTText,XSLTSubject,CreatedOn,CreatedBy)
VALUES(@DocumentType,@XSLTText,@XSLTSubject,@CreatedOn,@CreatedBy)
Select @XSLTId=SCOPE_IDENTITY()
INSERT INTO tblXSLTClientMapping
(XSLTId,ClientId,UserId,UserType)
VALUES(@XSLTId,@ClientId,@UserId,@UserType)

END

Else
BEGIN
Select @XSLTId=XSLTId FROM tblXSLTDetails WHERE XSLTSubject=@XSLTSubject and DocumentType=@DocumentType
Update  tblXSLTDetails Set XSLTText=@XSLTText Where XSLTId=@XSLTId



IF NOT EXISTS (SELECT 0 FROM tblXSLTClientMapping WHERE XSLTId=@XSLTId and UserId=@UserId )
BEGIN
INSERT INTO tblXSLTClientMapping
(XSLTId,ClientId,UserId,UserType)
VALUES(@XSLTId,@ClientId,@UserId,@UserType)

END




END


";
            using (SqlConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {
                SqlParameter[] param = new SqlParameter[9];
                param[0] = new SqlParameter("@DocumentType", SqlDbType.Int);
                param[0].Direction = ParameterDirection.Input;
                param[0].Value = doc.DocType;

                param[1] = new SqlParameter("@XSLTText", SqlDbType.NVarChar);
                param[1].Direction = ParameterDirection.Input;
                param[1].Value = doc.Content;

                param[2] = new SqlParameter("@UserType", SqlDbType.TinyInt);
                param[2].Direction = ParameterDirection.Input;
                param[2].Value = doc.UserType;

                param[3] = new SqlParameter("@UserId", SqlDbType.Int);
                param[3].Direction = ParameterDirection.Input;
                param[3].Value = doc.UserId;

                param[4] = new SqlParameter("@ClientId", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Input;
                param[4].Value = doc.CompanyId;

                param[5] = new SqlParameter("@CreatedOn", SqlDbType.DateTime);
                param[5].Direction = ParameterDirection.Input;
                param[5].Value = doc.CreatedOn;

                param[6] = new SqlParameter("@CreatedBy", SqlDbType.Int);
                param[6].Direction = ParameterDirection.Input;
                param[6].Value = doc.CreatedBy;

                param[7] = new SqlParameter("@XSLTId", SqlDbType.Int);
                param[7].Direction = ParameterDirection.Input;


                param[8] = new SqlParameter("@XSLTSubject", SqlDbType.VarChar);
                param[8].Direction = ParameterDirection.Input;
                param[8].Value = doc.Title;

                result = SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sqlQuery, param);
            }
            return result;
        }








        public int DeleteDocument(int DocumentId)
        {
            var result = 0;
            string sqlQuery = @"Begin Try 
BEGIN TRANSACTION
Delete FROM  tblXSLTClientMapping  where XSLTId =@XSLTId
Delete FROM tblXSLTDetails  where XSLTId =@XSLTId 
COMMIT
End Try 
Begin Catch 
ROLLBACK
End Catch";
            using (SqlConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@XSLTId", SqlDbType.Int);
                param[0].Direction = ParameterDirection.Input;
                param[0].Value = DocumentId;


                result = SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sqlQuery, param);
            }
            return result;
        }















    }

}