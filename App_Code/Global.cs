using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using System.Configuration;
using System.Web.Routing;
using System.Web.Http;

/// <summary>
/// Summary description for Global
/// </summary>
public class Global : System.Web.HttpApplication
{
    string StartDate, Enddate;
    public Global()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    void Application_Start(object sender, EventArgs e)
    {
        RouteTable.Routes.MapHttpRoute(
                 name: "DefaultApiRoute",
                 routeTemplate: "api/{controller}/{id}",
                 defaults: new { id = RouteParameter.Optional });



        //UnityConfig.RegisterComponents();

        ////////// Code that runs on application startup 
        ////// int companyid = Convert.ToInt16(ConfigurationManager.AppSettings["xxpc36"]);
        ////// switch (companyid)
        ////// {
        //////     case 27:
        //////          string autosendIndentPendingdetail = ConfigurationManager.AppSettings["autosendIndentPendingdetail"];
        //////          if (autosendIndentPendingdetail == "1")
        //////          {
        //////              // Code that runs on application startup
        //////              System.Timers.Timer Autotimer = new System.Timers.Timer();
        //////              //// Set the Interval to 7 Days (604800000 milliseconds) & for 1 minutes 60000
        //////              //Autotimer.Interval = 60000;

        //////              //// Set the Interval to 3 Days (259200000 milliseconds)
        //////              Autotimer.Interval = ((((3 * 24) * 60) * 60) * 1000);
        //////              Autotimer.AutoReset = true;
        //////              Autotimer.Elapsed += new System.Timers.ElapsedEventHandler(AutoTimer_Elapsed);
        //////              Autotimer.AutoReset = false;
        //////              Autotimer.Enabled = true;
        //////          }               
        //////         break;

        ////// }

        // Code that runs on application startup        
        // SqlhelperEnum.LoadMasters();
        //SET Timer
        #region  Buyer OrderDetail
        DateTime NextSatDate = new DateTime();
        for (int i = 0; i <= 7; i++)
        {
            NextSatDate = System.DateTime.Now.Date.AddDays(i);
            if (NextSatDate.DayOfWeek == DayOfWeek.Saturday)
            {
                if (i == 0 && System.DateTime.Now.Hour > 17)
                {
                    NextSatDate = NextSatDate.AddDays(7).AddHours(18);
                }
                else
                {
                    NextSatDate = NextSatDate.AddHours(18);
                }
                break;
            }
        }

        TimeSpan tsNextSat = (NextSatDate - System.DateTime.Now);
        //Schedule tast weekly      
        StartDate = NextSatDate.AddDays(-7).ToShortDateString();
        Enddate = NextSatDate.ToShortDateString();
        string autosendbuyerorderdetail = ConfigurationManager.AppSettings["autosendbuyerorderdetail"];
        if (autosendbuyerorderdetail == "1")
        {
            System.Timers.Timer mytimer = new System.Timers.Timer();
            mytimer.Interval = tsNextSat.TotalMilliseconds;

            mytimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);
            mytimer.AutoReset = false;
            mytimer.Enabled = true;
        }
        #endregion
        //Login Flag Enable Time For Anisa
        #region
        //System.Timers.Timer dailyuserenbale = new System.Timers.Timer();
        //TimeSpan Userinterval = new TimeSpan();
        //if (System.DateTime.Now.Hour >= 7)
        //{
        //    Userinterval = (System.DateTime.Now.AddDays(1).AddHours(7).AddMinutes(30) - System.DateTime.Now);
        //}
        //else
        //{
        //    Userinterval = (System.DateTime.Now.AddHours(7).AddMinutes(30) - System.DateTime.Now);
        //}
        //dailyuserenbale.Interval = Userinterval.TotalMilliseconds;
        //dailyuserenbale.Elapsed += new ElapsedEventHandler(dailyuserenbale_Elapsed);
        //dailyuserenbale.AutoReset = false;
        //dailyuserenbale.Enabled = true;
        ////Login flag Disable Time For Anisa
        //System.Timers.Timer dailyuserdisable = new System.Timers.Timer();
        //TimeSpan userdisableinterval = new TimeSpan();
        //if (System.DateTime.Now.Hour >= 18)
        //{
        //    userdisableinterval = (System.DateTime.Now.AddDays(1).AddHours(18).AddMinutes(15) - System.DateTime.Now);
        //}
        //else
        //{
        //    userdisableinterval = (System.DateTime.Now.AddHours(18).AddMinutes(15) - System.DateTime.Now);
        //}
        //dailyuserdisable.Interval = userdisableinterval.TotalMilliseconds;
        //dailyuserdisable.Elapsed += new ElapsedEventHandler(dailyuserdisable_Elapsed);
        //dailyuserdisable.AutoReset = false;
        //dailyuserdisable.Enabled = true;
        //
        #endregion
        //Send TNA Messages
        #region
        System.Timers.Timer dailySendmsg = new System.Timers.Timer();
        TimeSpan dailysendmsginterval = new TimeSpan();
        if (System.DateTime.Now.Hour >= 17)
        {

            dailysendmsginterval = (System.DateTime.Now.AddDays(1).AddHours(17).AddMinutes(30) - System.DateTime.Now);
        }
        else
        {
            dailysendmsginterval = (System.DateTime.Now.AddHours(17).AddMinutes(30) - System.DateTime.Now);
        }
        dailySendmsg.Interval = dailysendmsginterval.TotalMilliseconds;
        dailySendmsg.Elapsed += new ElapsedEventHandler(dailySendmsg_Elapsed);
        dailySendmsg.AutoReset = false;
        dailySendmsg.Enabled = true;
        #endregion
        //AUTO RECEIVE FROM BARCODE SCAN
        string autosavebarcodedata = ConfigurationManager.AppSettings["autosavebarcodedata"];
        if (autosavebarcodedata == "1")
        {
            System.Timers.Timer autosave = new System.Timers.Timer();
            autosave.Interval = 60000;
            autosave.Elapsed += new ElapsedEventHandler(autosave_Elapsed);
            autosave.Start();
        }
        //
    }

    void autosave_Elapsed(object sender, ElapsedEventArgs e)
    {
        ScheduleTasks obj = new ScheduleTasks();
        obj.Autosavebarcodestart();

    }
    void dailySendmsg_Elapsed(object source, System.Timers.ElapsedEventArgs e)
    {
        Timer mytimer = source as Timer;
        mytimer.Interval = (24 * 60 * 60000);
        ScheduleTasks obj = new ScheduleTasks();
        int companyid = Convert.ToInt16(ConfigurationManager.AppSettings["xxpc36"]);
        switch (companyid)
        {
            case 12:
                obj.SenddailyTNAupdate();
                break;
        }

    }
    void dailyuserdisable_Elapsed(object source, System.Timers.ElapsedEventArgs e)
    {
        Timer Mytimer = source as Timer;
        Mytimer.Interval = (24 * 60 * 60000);
        ScheduleTasks obj = new ScheduleTasks();
        int companyid = Convert.ToInt16(ConfigurationManager.AppSettings["xxpc36"]);
        switch (companyid)
        {
            case 8:
                obj.UserEnable_Disable(false);
                break;
        }

    }
    void dailyuserenbale_Elapsed(object source, System.Timers.ElapsedEventArgs e)
    {
        Timer Mytimer = source as Timer;
        Mytimer.Interval = (24 * 60 * 60000);
        ScheduleTasks obj = new ScheduleTasks();

        int companyid = Convert.ToInt16(ConfigurationManager.AppSettings["xxpc36"]);
        switch (companyid)
        {
            case 8:
                obj.UserEnable_Disable(true);
                break;
        }
    }
    void myTimer_Elapsed(object source, System.Timers.ElapsedEventArgs e)
    {
        //Resetting Interval of the Timer
        Timer myTimer = source as Timer;
        myTimer.Interval = (7 * 24 * 60 * 60000);   // 7 Days
        // use your mailer code
        ScheduleTasks obj = new ScheduleTasks();
        int companyid = Convert.ToInt16(ConfigurationManager.AppSettings["xxpc36"]);
        switch (companyid)
        {
            case 16:
                obj.Sendweeklymails(StartDate, Enddate);
                break;
        }

        //VndrWeeklyMail obj = new VndrWeeklyMail();
        //obj.sendWeeklyMail();
    }
    //////void AutoTimer_Elapsed(object source, System.Timers.ElapsedEventArgs e)
    //////{
    //////    //Resetting Interval of the Timer
    //////    Timer myTimer = source as Timer;
    //////   //// myTimer.Interval = 60000;   // 1 minutes
    //////    myTimer.Interval = ((((3 * 24) * 60) * 60) * 1000);

    //////    // use your mailer code
    //////    ScheduleTasks obj = new ScheduleTasks();
    //////    int companyid = Convert.ToInt16(ConfigurationManager.AppSettings["xxpc36"]);
    //////    switch (companyid)
    //////    {
    //////        case 27:
    //////            obj.SendAfterThreeDaysmails();               
    //////            break;
    //////    }

    //////    //VndrWeeklyMail obj = new VndrWeeklyMail();
    //////    //obj.sendWeeklyMail();
    //////}
    void Session_Start(object sender, EventArgs e)
    {
        // SqlhelperEnum.LoadMasters();
    }

}