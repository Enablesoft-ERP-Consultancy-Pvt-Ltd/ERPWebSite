<%@ WebHandler Language="C#" Class="ImageHandler" %>

using System;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

public class ImageHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dReader;
        cmd.CommandText = "SELECT ID,Photo From Draft_Order_ReferenceImage Where ID=@ID";
        int intImg = Convert.ToInt32(context.Request.QueryString["img"]);
        if (intImg == 1)
        {
            cmd.CommandText = "SELECT OrderDetailId,Photo From DRAFT_ORDER_DETAIL Where OrderDetailId=@ID";
        }
        else if (intImg == 2)
        {
            cmd.CommandText = "SELECT Top 1 OrderDetailId,Photo From Draft_Order_ReferenceImage Where OrderDetailId=@ID";
        }
        else if (intImg == 3)
        {
            cmd.CommandText = "SELECT Finishedid,Photo From MAIN_ITEM_IMAGE Where finishedid=@ID";
        }
        else if (intImg == 4)
        {
            cmd.CommandText = "SELECT OrderDetailId,Photo From orderdetail Where OrderDetailId=@ID";
        }
        else if (intImg == 5)
        {
            cmd.CommandText = "select top(1) orderdetailid,photo from ordermaster Om inner join orderdetail Od On om.orderid=od.orderid where om.orderid=@ID order by orderdetailid desc";
        }
        cmd.CommandType = System.Data.CommandType.Text;
        try
        {
            cmd.Connection = con;
            SqlParameter ImageID = new SqlParameter
                        ("@ID", System.Data.SqlDbType.Int);
            ImageID.Value = context.Request.QueryString["Id"];
            cmd.Parameters.Add(ImageID);
            con.Open();
           dReader = cmd.ExecuteReader();
           
            //if (dReader["ImageID"]==DBNull.Value)
            {
                dReader.Read();
            context.Response.BinaryWrite((byte[])dReader["Photo"]);
            }
            dReader.Close();
        }
        catch (Exception ex) { }
        finally
        {
            
            con.Close();
        }
       
    }
    public bool IsReusable {
     get {
            return false;
        }
    }
}