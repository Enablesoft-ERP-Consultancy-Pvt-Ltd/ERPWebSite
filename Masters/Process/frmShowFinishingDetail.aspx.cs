using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Drawing;
public partial class Masters_Process_frmShowFinishingDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void gettickvalue(object sender, EventArgs e)
    {

        FillGrid();

    }
    protected void FillGrid()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            tblrecord.Rows.Clear();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = con;
            cmd.CommandTimeout = 100;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Pro_ShowFinishingDetail";
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(dt);
            ad.Dispose();
            ad = null;

            TableRow tr = new TableRow();
            tr.BackColor = System.Drawing.Color.Teal;
            tr.ForeColor = System.Drawing.Color.White;
            //Show Columns
            foreach (DataColumn row in dt.Columns)
            {
                TableCell td = new TableCell();
                td.Width = 50;
                td.Height = 50;
                td.Font.Size = 15;
                td.Text = row.ColumnName.ToString();
                tr.Cells.Add(td);
            }
            //  tblrecord.Rows.Add(tr);
            //tblMenu.Rows.Add(tr);
            //Adding Rows in table
            foreach (DataRow dr in dt.Rows)
            {
                TableRow tr1 = new TableRow();
                foreach (DataColumn dc in dt.Columns)
                {
                    TableCell td = new TableCell();
                    td.Width = 50;
                    td.Font.Size = 12;
                    td.Text = dr[dc.ColumnName].ToString();

                    if (dc.ColumnName.ToString() == "LateBy")
                    {
                        if (Convert.ToInt16(dr[dc.ColumnName].ToString()) > 10)
                        {
                            tr1.BackColor = Color.Yellow;

                        }
                    }

                    tr1.Cells.Add(td);
                }
                tblrecord.Rows.Add(tr1);

            }

        }
        catch
        {
        }
        finally
        {
            con.Dispose();
            con.Close();

        }
    }
}