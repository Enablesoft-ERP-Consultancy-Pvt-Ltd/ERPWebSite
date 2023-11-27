ALTER TABLE [dbo].[Item_Process] 
ADD  [ProcessType] [tinyint] NULL


update Item_Process set ProcessType=1 Where ProcessType=0 OR ProcessType is Null;


CREATE PROCEDURE [dbo].[ProcessSequenceProc]   
@Flag int=null,  
@ItemId INT=NUll,   
@CompanyId INT=NUll,      
@QualityId INT=NUll,        
@DesignId INT=NUll,      
@ProcessXml Xml=NUll,  
@CreatedBy INT=NUll,    
@Msg VARCHAR(200)  OUTPUT,   
@CreatedOn DateTime=NUll  
  
AS  
BEGIN  
Select @CreatedOn=GETDATE()  
IF @Flag = 1   
BEGIN  
IF @CompanyId<>8   
BEGIN  
  
IF EXISTS(SELECT ITEMID FROM ITEM_PROCESS WHERE ITEMID=@ItemId AND  QUALITYID=@QualityId AND DESIGNID=@DesignId)        
BEGIN        
DELETE FROM ITEM_PROCESS WHERE ITEMID=@ItemId AND QUALITYID=@QualityId AND DESIGNID=@DesignId    
INSERT INTO ITEM_PROCESS(ITEMID,USERID,MASTERCOMPANYID,QUALITYID,PROCESSID,ProcessType,DESIGNID,SEQNO)        
SELECT @ITEMID,@CreatedBy,@CompanyId,@QualityId,T.C.value('@ProcessId', 'INT') ProcessId,  
T.C.value('@ProcessType', 'INT') ProcessType,@DesignId,T.C.value('@SeqNO', 'INT') SeqNO  
FROM @ProcessXml.nodes('/ProcessItems/ProcessItem') T(C)  
  
SET @Msg='DATA UPDATED SUCCESSFULLY...'        
END        
ELSE        
BEGIN        
INSERT INTO ITEM_PROCESS(ITEMID,USERID,MASTERCOMPANYID,QUALITYID,PROCESSID,ProcessType,DESIGNID,SEQNO)        
SELECT @ITEMID,@CreatedBy,@CompanyId,@QualityId,T.C.value('@ProcessId', 'INT') ProcessId,  
T.C.value('@ProcessType', 'INT') ProcessType,@DesignId,T.C.value('@SeqNO', 'INT') SeqNO  
FROM @ProcessXml.nodes('/ProcessItems/ProcessItem') T(C)  
  
SET @Msg='DATA SAVED SUCCESSFULLY'        
END  
  
END  
  
END  
  
IF @Flag = 2   
BEGIN  
  
Select PNM.process_Name_id,PNM.Process_Name,IP.ProcessType,IP.SeqNo from Process_name_Master PNM Inner Join Item_Process IP  
On PNM.Process_name_id=IP.ProcessId  
Where IP.ItemId=@ItemId And PNM.MasterCompanyid=@CompanyId and IP.DesignId=(Case when (@DesignId is  null) then IP.DesignId Else  @DesignId End)  
and IP.QualityId=(Case when (@QualityId is  null) then IP.QualityId Else  @QualityId End)  
order by  IP.SeqNO  
SET @Msg='DATA Readed SUCCESSFULLY...'      
END  
END