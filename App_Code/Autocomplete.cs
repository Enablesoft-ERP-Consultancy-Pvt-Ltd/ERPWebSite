using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
/// <summary>
/// Summary description for Autocomplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
[System.Web.Script.Services.ScriptService]
public class Autocomplete : System.Web.Services.WebService
{
    public Autocomplete()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    [WebMethod]
    public string[] GetEmployeeAll(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');

//        sql = @"select EI.EmpId,Case When EI.Empcode = '' Then EI.EmpName Else EI.Empcode End Empname from EmpInfo EI inner join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentName in ('PRODUCTION', 'PIT LOOM') 
//        and EI.Status='P' and EI.Blacklist=0 and";

        sql = @"select EI.EmpId,isnull(EI.EmpCode,'')+' - '+EI.EmpName as Empname from EmpInfo EI inner join Department D on EI.Departmentid=D.DepartmentId  
        and EI.Status='P' and EI.Blacklist=0 and";
        sql = sql + "(EI.Empcode like '%" + prefixText + "%' or  EI.EMPName like '%" + prefixText + "%') ";
        sql = sql + " Order by EI.Empname";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["Empname"].ToString());


            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["Empid"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetEmployeeAll_HR(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        sql = @"select EI.EmpId,EI.EMPNAME + '   [' +  EI.Empcode+']' as  Empname from EmpInfo EI inner join Department D on EI.Departmentid=D.DepartmentId 
        and EI.Status='P' and Ei.empcode<>'' and";
        sql = sql + "(Empcode like '" + prefixText + "%' or  EMPName like '" + prefixText + "%') ";
        sql = sql + " Order by Empname";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["Empname"].ToString());


            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["Empid"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetLotNo(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        sql = @"select Distinct LotNo,LotNo as Lotno1 from Stock Where ";
        sql = sql + "(Lotno like '%" + prefixText + "%') ";
        sql = sql + " order by LotNo1";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["Lotno1"].ToString());

            //ValtoShow = dt.Rows[i]["regFormNo"].ToString();
            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["LotNo"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetTagNo(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        sql = @"select Distinct TagNo,TagNo as TagNo1 from Stock Where ";
        sql = sql + "(TagNo like '%" + prefixText + "%') ";
        sql = sql + " order by TagNo1";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["TagNo1"].ToString());

            //ValtoShow = dt.Rows[i]["regFormNo"].ToString();
            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["TagNo"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public string[] GetLoomNo(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        string unitid = ids[2];
        string companyId = ids[1];
        sql = @"select UID,LoomNo from ProductionLoomMaster Where EnableDisableStatus=1 and unitid=" + unitid;
        if (companyId != "0" && companyId != "")
        {
            sql = sql + " and CompanyId=" + companyId + "";
        }
        sql = sql + " and  LoomNo like '%" + prefixText + "%' ";
        sql = sql + " order by LoomNo";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["LoomNo"].ToString());

            //ValtoShow = dt.Rows[i]["regFormNo"].ToString();
            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["UID"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] Getsamplecode(string prefixText, int count, string contextKey)
    {

        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        sql = @"select ID,Samplecode from Samplecode Where ";
        sql = sql + "   Samplecode like '%" + prefixText + "%' ";
        sql = sql + " order by id";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["Samplecode"].ToString());

            //ValtoShow = dt.Rows[i]["regFormNo"].ToString();
            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["ID"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] Getsamplecodedevelopment(string prefixText, int count, string contextKey)
    {

        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        sql = @"select ID,Samplecode from SampleDevelopmentMaster Where ";
        sql = sql + "   Samplecode like '%" + prefixText + "%' ";
        sql = sql + " order by id";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["Samplecode"].ToString());

            //ValtoShow = dt.Rows[i]["regFormNo"].ToString();
            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["ID"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetEmployeeForJob(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        sql = @"select  EI.EmpId, Case When EI.Empcode = '' Then EI.EmpName Else EI.Empcode End Empname From EmpInfo EI inner join Department D on EI.Departmentid=D.DepartmentId
        and EI.Status='P' and EI.Blacklist=0 and";
        sql = sql + "(Empcode like '%" + prefixText + "%' or  EMPName like '%" + prefixText + "%') ";
        sql = sql + " Order by Empname";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["Empname"].ToString());


            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["Empid"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetEmployeeForJobNew(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        sql = @"select  EI.EmpId,EmpName+' S/o '+isnull(FatherName,'')+' - '+Address+'  '+(case When Isnull(EMPCode,'')<>'' Then '['+EMpcode+']' Else '' ENd) as Empname from EmpInfo EI inner join Department D on EI.Departmentid=D.DepartmentId
        and EI.Status='P' and EI.Blacklist=0 inner join EmpProcess EP on EI.Empid=EP.Empid Where EP.Processid=" + ids[0] + @" and";
        sql = sql + "(Empcode like '%" + prefixText + "%' or  EMPName like '%" + prefixText + "%') ";
        sql = sql + " Order by Empname";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["Empname"].ToString());


            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["Empid"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetEmployeecode(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        sql = @"select  EI.EmpId,EMpcode as Empname from EmpInfo EI inner join Department D on EI.Departmentid=D.DepartmentId
         Where ";
        sql = sql + "(Empcode like '%" + prefixText + "%') ";
        sql = sql + " Order by Empname";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["Empname"].ToString());


            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["Empid"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetEmployeeName(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        sql = @"select  EI.EmpId,EMPNAME as Empname from EmpInfo EI inner join Department D on EI.Departmentid=D.DepartmentId
         Where ";
        sql = sql + "(Empname like '" + prefixText + "%')  and  EI.Departmentid=" + ids[0];
        sql = sql + " Order by Empname";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["Empname"].ToString());


            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["Empid"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetDestcode(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        string unitid = ids[2];
        sql = @"select ID,Destcode from Destinationmaster Where 1=1";
        sql = sql + " and  Destcode like '%" + prefixText + "%' ";
        sql = sql + " order by Destcode";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["Destcode"].ToString());

            //ValtoShow = dt.Rows[i]["regFormNo"].ToString();
            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["ID"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetArticleno(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        string unitid = ids[2];
        sql = @"select Articleno,Articleno as Articleno1 from Packingarticle Where 1=1";
        sql = sql + " and  Articleno like '%" + prefixText + "%' ";
        sql = sql + " order by Articleno";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["Articleno1"].ToString());

            //ValtoShow = dt.Rows[i]["regFormNo"].ToString();
            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["Articleno"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetBatchno(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        string status = ids[0];
        sql = @"select Batchno,Batchno as Batchno1 from Packingplanmaster Where 1=1";
        sql = sql + " and  Batchno like '%" + prefixText + "%' and Status='" + status + "'";
        sql = sql + " order by Batchno";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["Batchno1"].ToString());

            //ValtoShow = dt.Rows[i]["regFormNo"].ToString();
            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["Batchno"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetDispatchBatchno(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        string unitid = ids[2];
        sql = @"select Batchno,Batchno as Batchno1 from Dispatchplanmaster Where 1=1";
        sql = sql + " and  Batchno like '%" + prefixText + "%' ";
        sql = sql + " order by Batchno";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["Batchno1"].ToString());

            //ValtoShow = dt.Rows[i]["regFormNo"].ToString();
            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["Batchno"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetInvoiceno(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }

        string[] ids = contextKey.Split('#');
        
        sql = @"select Invoiceid,Tinvoiceno From Invoice(Nolock) Where 1=1";
        sql = sql + " and Tinvoiceno like '%" + prefixText + "%' ";
        sql = sql + " and InvoiceYear = " + ids[0];
        sql = sql + " order by Tinvoiceno";

        //string[] ids = contextKey.Split('#');
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["Tinvoiceno"].ToString());

            //ValtoShow = dt.Rows[i]["regFormNo"].ToString();
            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["Invoiceid"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetCostingsamplecode(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        string unitid = ids[2];
        sql = @"select ID,Samplecode From CostingMaster";
        sql = sql + " Where  Samplecode like '%" + prefixText + "%' ";
        sql = sql + " order by ID";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["Samplecode"].ToString());

            //ValtoShow = dt.Rows[i]["regFormNo"].ToString();
            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["ID"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    //GetProductionSpecDesignName

    public string[] GetProductionSpecDesignName(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        string unitid = ids[2];
        sql = @"select Distinct DesignName,DesignName as Designname1 From PRODUCTIONSPECIFICATIONDETAIL";
        sql = sql + " Where  DesignName like '" + prefixText + "%' ";
        sql = sql + " order by Designname1";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["DesignName1"].ToString());

            //ValtoShow = dt.Rows[i]["regFormNo"].ToString();
            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["DesignName"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetPurchaseVendor(string prefixText, int count, string contextKey)
   {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        sql = @"select distinct EI.empid,EI.empname from empinfo EI inner join Department DM on EI.Departmentid=DM.Departmentid  
         Where EI.blacklist=0 AND DM.Departmentname='PURCHASE'";
        sql = sql + " and (Empname like '" + prefixText + "%') ";
        sql = sql + " Order by Empname";       

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["Empname"].ToString());


            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["Empid"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }

    [WebMethod]
    public string[] GetDyeingVendor(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        sql = @"select distinct EI.empid,EI.empname from empinfo EI inner join EmpProcess EP on EI.EmpId=EP.EmpId   
         Where EI.blacklist=0 AND EP.processId=5";
        sql = sql + " and (Empname like '" + prefixText + "%') ";
        sql = sql + " Order by Empname";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["Empname"].ToString());


            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["Empid"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetItemName(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        sql = @"select  item_id,item_name+'('+b.CATEGORY_NAME+')' as item_name from ITEM_MASTER a join ITEM_CATEGORY_MASTER b on a.CATEGORY_ID=b.CATEGORY_ID where ";
        sql = sql + "(item_name like '" + prefixText + "%') ";
        sql = sql + " Order by item_name";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["item_name"].ToString());


            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["item_id"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetQualityName(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        sql = @"select  qualityid,QualityName+'('+b.ITEM_NAME+')' as QualityName from Quality a join ITEM_MASTER b on a.Item_Id=b.ITEM_ID where";
        sql = sql + "(QualityName like '" + prefixText + "%') ";
        sql = sql + " Order by QualityName";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["QualityName"].ToString());


            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["qualityid"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetDesignName(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        sql = @"select  designid,designname from design where";
        sql = sql + "(designname like '" + prefixText + "%') ";
        sql = sql + " Order by designname";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["designname"].ToString());


            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["designid"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetColorName(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        sql = @"select  colorid,colorname from color where";
        sql = sql + "(colorname like '" + prefixText + "%') ";
        sql = sql + " Order by colorname";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["colorname"].ToString());


            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["colorid"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetShadeColorName(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        sql = @"select  shadecolorid,ShadeColorName from ShadeColor where";
        sql = sql + "(ShadeColorName like '" + prefixText + "%') ";
        sql = sql + " Order by ShadeColorName";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["ShadeColorName"].ToString());


            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["shadecolorid"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetProcessName(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        sql = @"select  PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER where";
        sql = sql + "(PROCESS_NAME like '" + prefixText + "%') ";
        sql = sql + " Order by PROCESS_NAME";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["PROCESS_NAME"].ToString());


            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["PROCESS_NAME_ID"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }
    [WebMethod]
    public string[] GetOrderName(string prefixText, int count, string contextKey)
    {
        string sql, ValtoShow, itemkv;
        if (count == 0)
        {
            count = 20;
        }
        string[] ids = contextKey.Split('#');
        sql = @"select  orderid,CustomerOrderNo from OrderMaster where";
        sql = sql + "(CustomerOrderNo like '" + prefixText + "%') ";
        sql = sql + " Order by CustomerOrderNo";


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        DataTable dt = ds.Tables[0];
        count = dt.Rows.Count;
        List<string> items = new List<string>(dt.Rows.Count);

        for (int i = 0; i < count; i++)
        {
            ValtoShow = string.Format("{0}", dt.Rows[i]["CustomerOrderNo"].ToString());


            itemkv = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ValtoShow, dt.Rows[i]["orderid"].ToString());
            items.Add(itemkv);
        }
        return items.ToArray();
    }

}

