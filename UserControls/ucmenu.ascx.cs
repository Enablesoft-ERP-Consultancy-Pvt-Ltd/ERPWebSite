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
        if (Session["varUserId"] != null)
        {
            DataSet ds = new DataSet();
            string connStr = ErpGlobal.DBCONNECTIONSTRING;
            string sql=string.Empty;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                //string sql = @"select MenuID,DisplayName,NavigateURL,parentid,ToolTip,case NavigateURL when '' then 'false' else 'true' end Selectable from formname ";//inner join UserRights UR on UR.MenuId=formname.MenuId";
                if (Session["VarCompanyNo"].ToString() == "44")
                {
                    if (Session["varUserId"].ToString() == "1")
                    {
                        sql = @"select UR.MenuID,Upper(DisplayName) As DisplayName,NavigateURL,parentid,ToolTip,case NavigateURL when '' then 'false' else 'true' end Selectable from formname inner join UserRights UR on UR.MenuId=formname.MenuId where UR.userid=" + Session["varuserid"] + " and CompanyId=" + Session["varCompanyId"] + " AND Isvisible=1 order By UR.Menuid ";
                    }
                    else
                    {
                        sql = @"select UR.MenuID,Upper(DisplayName) As DisplayName,NavigateURL,parentid,ToolTip,case NavigateURL when '' then 'false' else 'true' end Selectable from formname inner join UserRights UR on UR.MenuId=formname.MenuId where UR.userid=" + Session["varuserid"] + " and CompanyId=" + Session["varCompanyId"] + " AND Isvisible=1 and ur.MenuId not in(48,49) order By UR.Menuid ";
                        //            string sql = @"select MenuID,DisplayName,NavigateURL,parentid,ToolTip,case NavigateURL when '' then 'false' else 'true' end Selectable from formname WHERE (MenuID IN (SELECT Menuid FROM UserRights WHERE (UserId=" + Session["varuserid"] + @")))
                        //                            UNION select MenuID,DisplayName,NavigateURL,parentid,ToolTip,case NavigateURL when '' then 'false' else 'true' end Selectable from formname WHERE (ParentId IN (SELECT U.Menuid FROM UserRights AS U INNER JOIN FormName AS F ON U.Menuid = F.MenuID WHERE (F.ParentId IS NOT NULL) AND (U.UserId=" + Session["varuserid"] + @")))";
                    }
                }
                else
                {
                    sql = @"select UR.MenuID,Upper(DisplayName) As DisplayName,NavigateURL,parentid,ToolTip,case NavigateURL when '' then 'false' else 'true' end Selectable from formname inner join UserRights UR on UR.MenuId=formname.MenuId where UR.userid=" + Session["varuserid"] + " and CompanyId=" + Session["varCompanyId"] + " AND Isvisible=1 order By UR.Menuid ";
                    //            string sql = @"select MenuID,DisplayName,NavigateURL,parentid,ToolTip,case NavigateURL when '' then 'false' else 'true' end Selectable from formname WHERE (MenuID IN (SELECT Menuid FROM UserRights WHERE (UserId=" + Session["varuserid"] + @")))
                    //                            UNION select MenuID,DisplayName,NavigateURL,parentid,ToolTip,case NavigateURL when '' then 'false' else 'true' end Selectable from formname WHERE (ParentId IN (SELECT U.Menuid FROM UserRights AS U INNER JOIN FormName AS F ON U.Menuid = F.MenuID WHERE (F.ParentId IS NOT NULL) AND (U.UserId=" + Session["varuserid"] + @")))";
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