using DocumentFormat.OpenXml.Drawing.Charts;
using IExpro.Core.Interfaces.Service;
using IExpro.Infrastructure.Repository;
using IExpro.Infrastructure.Services;
using IExpro.Web.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;

using System.Web.UI.WebControls;


public partial class Home : BasePage
{

    public static int CompanyId { get; set; }
    public static int UserId { get; set; }
    public static int BranchId { get; set; }


    static IOrderService OrdSrv;
    public Home()
    {
        OrdSrv = new OrderService(new UnitOfWork());
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Home.BranchId = Session["CurrentWorkingCompanyID"] != null ? Convert.ToInt32(Session["CurrentWorkingCompanyID"]) : 0;
            Home.CompanyId = Session["varCompanyId"] != null ? Convert.ToInt32(Session["varCompanyId"]) : 0;
            Home.UserId = Session["varuserid"] != null ? Convert.ToInt32(Session["varuserid"]) : 0;
        }

    }





    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]

    public static string OrderList()
    {
        string resultString = "";
        try
        {

            var obj = OrdSrv.GetOrderList(Home.CompanyId).OrderByDescending(x=>x.OrderId);
            var objdata = obj.GroupBy(x => x.OrderStatus).Select(y => new
            {
                OrderStatus = y.Key,
                OrderCount = y.Count(),
            });
            var result = new { data = obj, chartData = objdata.ToList(), totalSum = objdata.Sum(x => x.OrderCount) };
            resultString = JsonConvert.SerializeObject(result);


        }
        catch(Exception ex) {

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
        var obj = OrdSrv.GetVendorPOStatus(Home.BranchId);
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
        var obj = OrdSrv.DyeingStatus(Home.BranchId);
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
























}