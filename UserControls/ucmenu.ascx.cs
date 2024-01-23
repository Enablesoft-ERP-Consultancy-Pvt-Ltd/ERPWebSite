using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class UserControls_ucmenu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (Session["varUserId"] != null)
        {
            DataSet ds = new DataSet();
            string connStr = ErpGlobal.DBCONNECTIONSTRING;
            string sql = string.Empty;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                if (Session["VarCompanyNo"].ToString() == "44")
                {
                    if (Session["varUserId"].ToString() == "1")
                    {
                        sql = @"select UR.MenuID,Upper(DisplayName) As DisplayName,NavigateURL,parentid,ToolTip,case NavigateURL when '' then 'false' else 'true' end Selectable 
                        From formname(nolock) 
                        join UserRights UR(nolock) on UR.MenuId=formname.MenuId 
                        Where UR.userid=" + Session["varuserid"] + " and CompanyId=" + Session["varMasterCompanyIDForERP"] + " AND Isvisible=1 order By UR.Menuid ";
                    }
                    else
                    {
                        sql = @"select UR.MenuID,Upper(DisplayName) As DisplayName,NavigateURL,parentid,ToolTip,case NavigateURL when '' then 'false' else 'true' end Selectable 
                        From formname(nolock) 
                        join UserRights UR(nolock) on UR.MenuId=formname.MenuId 
                        Where UR.userid=" + Session["varuserid"] + " and CompanyId=" + Session["varMasterCompanyIDForERP"] + " AND Isvisible=1  order By UR.Menuid ";
                    }
                }
                else
                {
                    sql = @"select UR.MenuID,Upper(DisplayName) As DisplayName,NavigateURL,parentid,ToolTip,case NavigateURL when '' then 'false' else 'true' end Selectable 
                    From Formname(nolock) 
                    join UserRights UR(nolock) on UR.MenuId=formname.MenuId 
                    where UR.userid=" + Session["varuserid"] + " and CompanyId=" + Session["varMasterCompanyIDForERP"] + " AND Isvisible=1 order By UR.Menuid ";
                }
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(ds);
                da.Dispose();
            }
            ds.DataSetName = "Menus";
            ds.Tables[0].TableName = "Menu";
            DataRelation relation = null;

            relation = new DataRelation("ParentChild",
            ds.Tables["Menu"].Columns["MenuID"],
            ds.Tables["Menu"].Columns["parentid"], true);

            relation.Nested = true;
            ds.Relations.Add(relation);
            xmlDataSource.Data = ds.GetXml();
            xmlDataSource.EnableCaching = false;
        }
    }
}