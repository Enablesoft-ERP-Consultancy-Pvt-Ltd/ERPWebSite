using IExpro.Core.Interfaces.Service;
using IExpro.Core.Models.Reports;
using IExpro.Infrastructure.Repository;
using IExpro.Infrastructure.Services;
using IExpro.Web.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;


public partial class Home : BasePage
{
    public static int IExproId { get; set; }
    public static int CompanyId { get; set; }
    public static long UserId { get; set; }


    static IOrderService OrdSrv;
    public Home()
    {
        OrdSrv = new OrderService(new UnitOfWork());
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Home.IExproId = User.IExproId;
            Home.CompanyId = User.CompanyId;
            Home.UserId = User.UserId;
        }
    }

    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string OrderList()
    {
        string resultString = "";
        try
        {
    

            var obj = OrdSrv.GetOrderList(Home.CompanyId).OrderByDescending(x => x.OrderId);
            var objdata = obj.GroupBy(x => x.OrderStatus).Select(y => new
            {
                OrderStatus = y.Key,
                OrderCount = y.Count(),
            });
            var result = new { data = obj, chartData = objdata.ToList(), totalSum = objdata.Sum(x => x.OrderCount) };
            resultString = JsonConvert.SerializeObject(result);


        }
        catch (Exception ex)
        {

            throw ex;

        }
        return resultString;
    }

    /// <summary>
    /// Logs in the user
    /// </summary>
    /// <param name="Username">The username</param>
    /// <param name="Password">The password</param>
    /// <returns>true if login successful</returns>
    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetPOStatusList()
    {
        string resultString = "";
        var obj = OrdSrv.GetVendorPOStatus(Home.CompanyId);
        var result = new { data = obj };
        resultString = JsonConvert.SerializeObject(result);
        return resultString;
    }

    /// <summary>
    /// Logs in the user
    /// </summary>
    /// <param name="Username">The username</param>
    /// <param name="Password">The password</param>
    /// <returns>true if login successful</returns>
    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetDyeingStatusList()
    {
        string resultString = "";
        var obj = OrdSrv.DyeingStatus(Home.CompanyId);
        var result = new { data = obj };
        resultString = JsonConvert.SerializeObject(result);
        return resultString;
    }

    /// <summary>
    /// Logs in the user
    /// </summary>
    /// <param name="Username">The username</param>
    /// <param name="Password">The password</param>
    /// <returns>true if login successful</returns>
    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetPurchaseList(int OrderId)
    {
        string resultString = "";
        var obj = OrdSrv.GetPurchaseList(OrderId).ToList();
        var result = new { data = obj };
        resultString = JsonConvert.SerializeObject(result);
        return resultString;
    }

    /// <summary>
    /// Get Indent detail According to Process and Order
    /// </summary>
    /// <param name="OrderId">The Order No</param>
    /// <param name="ProcessId">The Process No</param>
    /// <returns>true if login successful</returns>
    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetIndentDetail(int OrderId, int ProcessId)
    {
        string resultString = "";
        var obj = OrdSrv.GetOrderByIndentDetail(OrderId, ProcessId).ToList();
        var result = new { data = obj };
        resultString = JsonConvert.SerializeObject(result);
        return resultString;
    }

    /// <summary>
    /// Get Order Deatil
    /// </summary>
    /// <param name="OrderId">The OrderId</param>
    /// <returns>order detail successful</returns>
    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetOrderDetail(int OrderId)
    {
        string resultString = "";
        var obj = OrdSrv.GetOrderDetail(OrderId).ToList();
        var result = new { data = obj };
        resultString = JsonConvert.SerializeObject(result);
        return resultString;
    }


    /// <summary>
    /// Get Home furnishing Issue detail According to Process and Order
    /// </summary>
    /// <param name="OrderId">The Order No</param>
    /// <param name="ProcessId">The Process No</param>
    /// <returns>true if login successful</returns>
    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetIssueDetail(int OrderId, int ProcessId)
    {
        string resultString = "";
        var obj = OrdSrv.GetOrderByIssueId(OrderId, ProcessId).ToList();
        var result = new { data = obj };
        resultString = JsonConvert.SerializeObject(result);
        return resultString;
    }


    /// <summary>
    /// Get Home furnishing Issue detail According to Process and Order
    /// </summary>
    /// <param name="OrderId">The Order No</param>
    /// <returns>true if login successful</returns>
    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetFinishDetail(int OrderId)
    {
        //OrderId = 8;
        string resultString = "";
        var obj = OrdSrv.GetFinishedItem(OrderId).ToList();
        var result = new { data = obj };
        resultString = JsonConvert.SerializeObject(result);
        return resultString;
    }
    

}