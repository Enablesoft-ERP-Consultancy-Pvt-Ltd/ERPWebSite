
SELECT  * FROM [ExportERP].[dbo].[tblXSLTDetails]
SELECT  * FROM [ExportERP].[dbo].[tblXSLTClientMapping]

truncate table  [ExportERP].[dbo].[tblXSLTClientMapping]
truncate table  [ExportERP].[dbo].[tblXSLTDetails]


SET IDENTITY_INSERT [ExportERP].[dbo].[tblXSLTDetails]  ON
GO

Insert Into [ExportERP].[dbo].[tblXSLTDetails]
([XSLTId],[PrintType],[DocumentType],[XSLTText],[XSLTSubject],[CreatedOn],[CreatedBy])
SELECT [XSLTId],[PrintType],[DocumentType],[XSLTText],[XSLTSubject],[CreatedOn],[CreatedBy] FROM [ExportERP.Test].[dbo].[tblXSLTDetails]
-- code to insert explicit ID values

SET IDENTITY_INSERT [ExportERP].[dbo].[tblXSLTDetails]  OFF
GO





Insert Into [ExportERP].[dbo].[tblXSLTClientMapping]
([XSLTId],[ClientId],[UserId],[UserType])
SELECT [XSLTId],[ClientId],[UserId],[UserType] FROM [ExportERP.Test].[dbo].[tblXSLTClientMapping]


SELECT [XSLTId],[ClientId],[UserId],[UserType] FROM [ExportERP].[dbo].[tblXSLTClientMapping]

