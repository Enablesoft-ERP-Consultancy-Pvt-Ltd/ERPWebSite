using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using AjaxControlToolkit;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for CCDDLWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
//[System.Web.Script.Services.ScriptService]
public class CCDDLWebService : System.Web.Services.WebService
{

    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    DataSet ds = new DataSet();
    
    
    //Fill Customer
    [System.Web.Services.WebMethod(EnableSession=true)]
    public CascadingDropDownNameValue[] FillCustomer(string knownCategoryValues, string category)
    {
        
        SqlCommand cmd = new SqlCommand("SELECT customerid,CompanyName + SPACE(5)+Customercode as CustomerCode from customerinfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by CompanyName", con);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        con.Close();
        List<CascadingDropDownNameValue> customers = new List<CascadingDropDownNameValue>();
        foreach(DataRow dr in ds.Tables[0].Rows)
        {
            string CustomerId = dr["CustomerId"].ToString();
            string Customer = dr["CustomerCode"].ToString();
            customers.Add(new CascadingDropDownNameValue(Customer,CustomerId));
        }
        return customers.ToArray();
    }
    [WebMethod(EnableSession = true)]
    public CascadingDropDownNameValue[] FillBank(string knownCategoryValues, string category)
    {

        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("select BankId,BankName from Bank where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + "", con);
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        con.Close();
        List<CascadingDropDownNameValue> Bank = new List<CascadingDropDownNameValue>();

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            string BankId = dr["BankId"].ToString();
            string BankName = dr["BankName"].ToString();
            Bank.Add(new CascadingDropDownNameValue(BankName, BankId));
        }
        return Bank.ToArray();
    }

    [WebMethod]
    public CascadingDropDownNameValue[] BindCategory(string knownCategoryValues, string category)
    {

        
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("select CATEGORY_ID,CATEGORY_NAME from Item_category_master ", con);
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        con.Close();
        List<CascadingDropDownNameValue> CategoryDetails = new List<CascadingDropDownNameValue>();

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            string CategoryId = dr["CATEGORY_ID"].ToString();
            string Categoryname = dr["CATEGORY_NAME"].ToString();
            CategoryDetails.Add(new CascadingDropDownNameValue(Categoryname, CategoryId));
        }
        return CategoryDetails.ToArray();
    }
    //Web method for bind city
    [WebMethod]

    public CascadingDropDownNameValue[] BindItem(string knownCategoryValues, string category)
    {
        
        int categoryId;
        StringDictionary categorydetail = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
        categoryId = Convert.ToInt16(categorydetail["Category"]);
        con.Open();
        SqlCommand cmd = new SqlCommand("select Item_Id,ITEM_NAME from Item_master where Category_id=@CategoryId", con);
        cmd.Parameters.Add("@categoryId", categoryId);
        cmd.ExecuteNonQuery();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        ad.Fill(ds);
        con.Close();
        List<CascadingDropDownNameValue> ItemDetails = new List<CascadingDropDownNameValue>();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            string ItemId = dr["Item_Id"].ToString();
            string Item_Name = dr["ITEM_NAME"].ToString();
            ItemDetails.Add(new CascadingDropDownNameValue(Item_Name, ItemId));
        }
        return ItemDetails.ToArray();
    }

}
