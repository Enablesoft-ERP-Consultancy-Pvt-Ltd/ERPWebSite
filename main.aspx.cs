using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
public partial class main : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
        //if (VarCompanyNo == 7)
        //{
        //    TDDg.Visible = true;
        //   // FillGrid();
        //}
    }  
    protected void DG_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void FillGrid()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = "select LocalOrder+'/'+CustomerOrderNo + space(7) + Replace(convert(nvarchar(11),OrderDate,106),' ',' -') As OrderNo,OP.OrderId As OrderId  from OrderProcessPlanning OP,OrderMaster OM where OM.OrderId=OP.OrderId";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        DG.DataSource = ds;
        DG.DataBind();
    }
}