using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;
public partial class ImageSave : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
    }
    private DataSet GetData()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"select * from ITEM_CATEGORY_MASTER order by CATEGORY_ID ";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "/ImageSave.aspx"); 
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        return ds;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        DataSet dsExport = GetData();
        System.IO.StringWriter tw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw =
           new System.Web.UI.HtmlTextWriter(tw);
        DataGrid dgGrid = new DataGrid();
        dgGrid.DataSource = dsExport;

        //Report Header

        hw.WriteLine("<b><u><font size='5'>Report for the Fiscal year: </font></u></b>");
        hw.WriteLine("<br>&mp;nbsp;");
        // Get the HTML for the control.

        dgGrid.HeaderStyle.Font.Bold = true;
        dgGrid.DataBind();
        dgGrid.RenderControl(hw);

        // Write the HTML back to the browser.

        Response.ContentType = "application/vnd.ms-excel";
        this.EnableViewState = false;
        Response.Write(tw.ToString());
        Response.End();
        //if (compneyImage.FileName != "")
        //{
        //    SqlConnection myConnection = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //    //SqlParameter[] _arrpara = new SqlParameter[1];
        //    //_arrpara[0] = new SqlParameter("@image", SqlDbType.Image);
        //    //_arrpara[0].Value = compneyImage.FileBytes;

        //    byte[] myimage = new byte[compneyImage.PostedFile.ContentLength];
        //    HttpPostedFile Image = compneyImage.PostedFile;
        //    Image.InputStream.Read(myimage, 0, (int)compneyImage.PostedFile.ContentLength);
        //    SqlCommand storeimage = new SqlCommand("insert into  A (id,Photo) values(2,@image)", myConnection);
        //    storeimage.Parameters.Add("@image", SqlDbType.Image, myimage.Length).Value = myimage;
        //    System.Drawing.Image img = System.Drawing.Image.FromStream(compneyImage.PostedFile.InputStream);
        //    myConnection.Open();
        //    storeimage.ExecuteNonQuery();

        //    Image1.ImageUrl = "~/ImageHandler.ashx?Id=" + 1;
        //    Image2.ImageUrl = "~/ImageHandler.ashx?Id=" + 2;

        //    //string sql = "Select Photo from A where id=1";
        //    //SqlCommand cmd = new SqlCommand(sql, myConnection);
        //    //cmd.Prepare();
        //    //SqlDataReader dr = cmd.ExecuteReader();
        //    //dr.Read();
        //    //HttpContext context = null; ;
        //    //context.Response.BinaryWrite((byte[])dr["Photo"]);
        //    //dr.Close();
        //    //myConnection.Close();
        //   // string Str = "Insert into A Values(id,Photo)(1,'" + _arrpara[0].Value + "')";
        //    //SqlHelper.ExecuteNonQuery(con, CommandType.Text, Str);
        //}
    }
}