using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class UserControls_Navigation : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            DataTable dt = this.GetData(0);
            PopulateMenu(dt, 0, null);
        }
    }






    private DataTable GetData(int parentMenuId)
    {

        string query = @"select UR.MenuID,Upper(DisplayName) As DisplayName,NavigateURL,parentid,ToolTip,case NavigateURL when '' then 'false' else 'true' end Selectable from 
formname inner join UserRights UR on UR.MenuId=formname.MenuId 
where IsNUll(formname.ParentId,0)=@ParentId AND UR.userid=@UserId and CompanyId=@CompanyId AND Isvisible=1  order By UR.Menuid";

        using (SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(query))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Parameters.AddWithValue("@ParentId", parentMenuId);
                    cmd.Parameters.AddWithValue("@UserId", Session["varuserid"]);
                    cmd.Parameters.AddWithValue("@CompanyId", Session["varCompanyId"]);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                }
            }
            return dt;
        }
    }



    private void PopulateMenu(DataTable dt, int parentMenuId, MenuItem parentMenuItem)
    {
        string currentPage = Path.GetFileName(Request.Url.AbsolutePath);
        foreach (DataRow row in dt.Rows)
        {
            MenuItem menuItem = new MenuItem
            {
                Value = row["MenuID"].ToString(),
                Text = row["DisplayName"].ToString(),
                NavigateUrl = row["NavigateURL"].ToString(),

                Selected = row["NavigateURL"].ToString().EndsWith(currentPage, StringComparison.CurrentCultureIgnoreCase)
            };
            if (parentMenuId == 0)
            {
                tblMenu.Items.Add(menuItem);
                DataTable dtChild = this.GetData(int.Parse(menuItem.Value));
                PopulateMenu(dtChild, int.Parse(menuItem.Value), menuItem);
            }
            else
            {
                parentMenuItem.ChildItems.Add(menuItem);
            }
        }
    }
}
