using DocumentFormat.OpenXml.Drawing.Charts;
using IExpro.Core.Interfaces.Service;
using IExpro.Infrastructure.Repository;
using IExpro.Infrastructure.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for Test
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class Test : System.Web.Services.WebService
{
    static IOrderService OrdSrv;
    public Test()
    {
        OrdSrv = new OrderService(new UnitOfWork());
    }
    [WebMethod]
    public string HelloWorld()
    {

        string resultString = "";
        var obj = OrdSrv.GetOrderByIndentDetail(69, 5).ToList();
        var result = new { data = obj };
        resultString = JsonConvert.SerializeObject(result);
        return resultString;
   
    }

}
