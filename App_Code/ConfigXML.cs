using System;
using System.Data;
using System.Configuration;
using System.Xml;
using System.Xml.XPath;
using System.Web;
using System.Security.Principal;

/// <summary>
/// MoCoXML class use for XML operatons
/// </summary>
public class MoCoXML
{
    /// <summary>
    /// This function return message from XML file for messages
    /// </summary>
    /// <param name="Key">string: key</param>
    /// <param name="source">string: Source</param>
    /// <returns>Message of key and source comobination return blank string if combination not exist</returns>
    public static string GetConfigMessage(string Key)
    {
        try
        {
            string xmlfilepath=string.Empty;

            //if(HttpContext.Current.User.IsInRole(CorpCommon.USERROLES.HR.ToString()))
            //    xmlfilepath = HttpContext.Current.Server.MapPath("~/App_Data/HrConfigMessage.xml");
            //else if(HttpContext.Current.User.IsInRole(CorpCommon.USERROLES.SALES.ToString()))
            //    xmlfilepath = HttpContext.Current.Server.MapPath("~/App_Data/SalesConfigMessage.xml");
            //else
                xmlfilepath = HttpContext.Current.Server.MapPath("~/App_Data/CommonConfigMessage.xml");

            XPathDocument doc = new XPathDocument(xmlfilepath);
            XPathNavigator nav = doc.CreateNavigator();
            string xpath = "";
            xpath = "/CustomSettings/ConfigMessage[Key = '" + Key + "']/Value";
            XPathNavigator node = nav.SelectSingleNode(xpath);
            if (node != null)
                return node.InnerXml;
            else
                return "";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
