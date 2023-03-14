using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.IO;
public partial class Masters_ReportForms_frmtoolissrec : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"SELECT DISTINCT EI.EMPID,EI.EMPNAME+' '+ISNULL('['+ EI.EMPCODE +']','') AS EMPNAME FROM EMPINFO EI INNER JOIN TOOLISSRECMASTER TI ON EI.EMPID=TI.EMPID ORDER BY EMPNAME
                           SELECT U.UNITSID,U.UNITNAME FROM UNITS U INNER JOIN UNITS_AUTHENTICATION UA ON U.UNITSID=UA.UNITSID and Ua.Userid=" + Session["varuserid"] + @" ORDER BY UNITNAME
                           SELECT PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME FROM PROCESS_NAME_MASTER PNM INNER JOIN USERRIGHTSPROCESS URP ON PNM.PROCESS_NAME_ID=URP.PROCESSID AND URP.USERID=1 ORDER BY PROCESS_NAME";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDemployee, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDunitname, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDjobname, ds, 2, true, "--Plz Select--");
            txtfrom.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtto.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GETTOOLISSREC", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 30000;
        cmd.Parameters.AddWithValue("@empid", DDemployee.SelectedIndex > 0 ? DDemployee.SelectedValue : "0");
        cmd.Parameters.AddWithValue("@unitid", DDunitname.SelectedIndex > 0 ? DDunitname.SelectedValue : "0");
        cmd.Parameters.AddWithValue("@Jobid", DDjobname.SelectedIndex > 0 ? DDjobname.SelectedValue : "0");
        cmd.Parameters.AddWithValue("@fromdate", txtfrom.Text);
        cmd.Parameters.AddWithValue("@Todate", txtto.Text);

        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        DataSet ds = new DataSet();
        ad.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Toolissrec");
            //**************
            sht.Range("A1:I1").Merge();
            sht.Range("A1").SetValue("Sharp Tool Issue & Receive Record(" + txtfrom.Text + " - " + txtto.Text + ")");
            sht.Range("A1:I1").Style.Font.SetBold();
            sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            int Hrow = 2;

            sht.Range("A" + Hrow).SetValue("Date");
            sht.Range("B" + Hrow).SetValue("Employee Name");
            sht.Range("C" + Hrow).SetValue("Barcode Id");
            sht.Range("D" + Hrow).SetValue("Tools Name");

            sht.Range("E" + Hrow).SetValue("Issue Time");
            sht.Range("F" + Hrow).SetValue("Scan By");

            sht.Range("G" + Hrow).SetValue("Receive Time");
            sht.Range("H" + Hrow).SetValue("Remarks");
            sht.Range("I" + Hrow).SetValue("Scan By");

            sht.Range("A" + Hrow + ":I" + Hrow).Style.Font.SetBold();

            int row = Hrow + 1;
            DataTable dtdistinctprocess = ds.Tables[0].DefaultView.ToTable(true, "Process_name");
            DataView dvprocess = new DataView(dtdistinctprocess);
            dvprocess.Sort = "process_name";
            DataSet dsprocess = new DataSet();
            dsprocess.Tables.Add(dvprocess.ToTable());
            for (int i = 0; i < dsprocess.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":I" + row).Merge();
                sht.Range("A" + row).SetValue(dsprocess.Tables[0].Rows[i]["Process_name"]);
                sht.Range("A" + row + ":I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row + ":I" + row).Style.Font.SetBold();

                DataView dvdetail = new DataView(ds.Tables[0]);
                dvdetail.RowFilter = "Process_name='" + dsprocess.Tables[0].Rows[i]["Process_name"] + "'";
                DataSet dsdetail = new DataSet();
                dsdetail.Tables.Add(dvdetail.ToTable());
                row = row + 1;

                DataTable dtdistinct = dsdetail.Tables[0].DefaultView.ToTable(true, "Trandate", "EMpcode", "Barcodeid", "Toolname");
                DataView dv1 = new DataView(dtdistinct);
                dv1.Sort = "Trandate,empcode,Barcodeid,Toolname";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv1.ToTable());
                int issrow = 0;
                int recrow = 0;
                int issrowcnt = 0, recrowcnt = 0;

                for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                {
                    issrowcnt = 0;
                    recrowcnt = 0;

                    DataView dviss_rec = new DataView(ds.Tables[0]);
                    dviss_rec.RowFilter = "Trandate='" + ds1.Tables[0].Rows[j]["Trandate"] + "' and Empcode='" + ds1.Tables[0].Rows[j]["empcode"] + "' and barcodeid='" + ds1.Tables[0].Rows[j]["Barcodeid"] + "' and Toolname='" + ds1.Tables[0].Rows[j]["Toolname"] + "' and Process_name='" + dsprocess.Tables[0].Rows[i]["Process_name"] + "'";
                    DataSet dsdetailissrec = new DataSet();
                    dsdetailissrec.Tables.Add(dviss_rec.ToTable());


                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[j]["Trandate"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[j]["empcode"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[j]["Barcodeid"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[j]["Toolname"]);

                    DataView dvissue = new DataView(dsdetailissrec.Tables[0]);
                    dvissue.RowFilter = "TranType=0";
                    dvissue.Sort = "Issuetime";
                    DataSet dsissue = new DataSet();
                    dsissue.Tables.Add(dvissue.ToTable());

                    //***********************Issue
                    issrow = row;
                    for (int k = 0; k < dsissue.Tables[0].Rows.Count; k++)
                    {
                        sht.Range("E" + issrow).SetValue(Convert.ToDateTime(dsissue.Tables[0].Rows[k]["issuetime"]).ToShortTimeString());
                        sht.Range("F" + issrow).SetValue(dsissue.Tables[0].Rows[k]["username"]);
                        issrow = issrow + 1;
                        issrowcnt = issrowcnt + 1;
                    }
                    //***********************Receive
                    recrow = row;
                    DataView dvrec = new DataView(dsdetailissrec.Tables[0]);
                    dvrec.RowFilter = "TranType=1";
                    dvrec.Sort = "Issuetime";
                    DataSet dsrec = new DataSet();
                    dsrec.Tables.Add(dvrec.ToTable());
                    for (int L = 0; L < dsrec.Tables[0].Rows.Count; L++)
                    {
                        sht.Range("G" + recrow).SetValue(Convert.ToDateTime(dsrec.Tables[0].Rows[L]["issuetime"]).ToShortTimeString());
                        sht.Range("I" + recrow).SetValue(dsrec.Tables[0].Rows[L]["username"]);
                        recrowcnt = recrowcnt + 1;
                        recrow = recrow + 1;
                    }
                    if (issrowcnt > recrowcnt)
                    {
                        row = row + issrowcnt;
                    }
                    else if (recrowcnt > issrowcnt)
                    {
                        row = row + recrowcnt;
                    }
                    else
                    {
                        row += 1;
                    }
                }
            }

            using (var a = sht.Range("A1" + ":I" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //******SAVE FILE
            sht.Columns(1, 15).AdjustToContents();
            string Path = "";
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("Toolissrec-" + DateTime.Now + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No record found for this combination.');", true);
        }
    }
}