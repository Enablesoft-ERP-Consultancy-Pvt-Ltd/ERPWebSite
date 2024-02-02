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
using ClosedXML.Excel;

public partial class Masters_ReportForms_frmReportJobWise_Rec_Pending : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            //SqlhelperEnum.FillDropDown(AllEnums.MasterTables.Units, DDUnit, pID: "UnitsId", pName: "Unitname");
            string str = @"select PROCESS_NAME_ID,Process_name from PROCESS_NAME_MASTER order by Process_Name
                           select Item_id,Item_name from Item_master IM,categoryseparate CS Where IM.category_id=CS.CategoryId And CS.id=0 order by Item_name
                           select U.UnitsId,U.UnitName from Units U inner join Units_authentication UA on U.unitsId=UA.UnitsId and UA.Userid=" + Session["varuserid"] + @" order by U.unitsId
                           select ICm.CATEGORY_ID,ICM.CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=Cs.Categoryid and cs.id=0 order by CATEGORY_NAME
                           Select CustomerID, CustomerCode From CustomerInfo Where MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + " Order By CustomerCode ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDjob, ds, 0, true, "--Plz Select Job--");
            UtilityModule.ConditionalComboFillWithDS(ref DDArticle, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDUnitName, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDcategory, ds, 3, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerOrderNo, ds, 4, true, "--Plz Select--");

            txtFromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            RDRecReport.Checked = true;
            //*************
            switch (Convert.ToString(Session["varMasterCompanyIDForERP"]))
            {
                case "8":
                    TRLoomBal.Visible = false;
                    TRMaterial.Visible = true;
                    break;
                case "21":                    
                    TRFolioWiseMaterialIssWithConsumption.Visible = true;
                    TRLoomBal.Visible = true;
                    Trcategory.Visible = true;
                    if (DDcategory.Items.Count > 0)
                    {
                        DDcategory.SelectedIndex = 1;
                        DDcategory_SelectedIndexChanged(DDcategory, new EventArgs());
                    }
                    break;
                case "14":
                    TRFolioWiseMaterialIssWithConsumption.Visible = true;
                    TRLoomBal.Visible = true;
                    Trcategory.Visible = true;
                    if (DDcategory.Items.Count > 0)
                    {
                        DDcategory.SelectedIndex = 1;
                        DDcategory_SelectedIndexChanged(DDcategory, new EventArgs());
                    }
                    break;
                default:
                    TRFolioWiseMaterialIssWithConsumption.Visible = false;
                    TRLoomBal.Visible = true;
                    Trcategory.Visible = true;
                    if (DDcategory.Items.Count > 0)
                    {
                        DDcategory.SelectedIndex = 1;
                        DDcategory_SelectedIndexChanged(DDcategory, new EventArgs());
                    }
                    break;
            }

        }
    }
    protected void Loombal()
    {
        lblErrorMessage.Text = "";
        try
        {
            string FilterBy = "";
            if (DDjob.SelectedIndex > 0)
            {
                FilterBy = FilterBy + " JOB-" + DDjob.SelectedItem.Text;
            }
            if (DDUnitName.SelectedIndex > 0)
            {
                FilterBy = FilterBy + " UNIT-" + DDUnitName.SelectedItem.Text;
            }
            if (DDLoomNo.SelectedIndex > 0)
            {
                FilterBy = FilterBy + " LOOM NO-" + DDLoomNo.SelectedItem.Text;
            }
            if (DDArticle.SelectedIndex > 0)
            {
                FilterBy = FilterBy + " ARTICLE-" + DDArticle.SelectedItem.Text;
            }
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@processid", DDjob.SelectedValue);
            param[1] = new SqlParameter("@Unitid", DDUnitName.SelectedIndex > 0 ? DDUnitName.SelectedValue : "0");
            param[2] = new SqlParameter("@Loomid", DDLoomNo.SelectedIndex > 0 ? DDLoomNo.SelectedValue : "0");
            param[3] = new SqlParameter("@Articleid", DDArticle.SelectedIndex > 0 ? DDArticle.SelectedValue : "0");
            param[4] = new SqlParameter("@FromDate", txtFromdate.Text);
            param[5] = new SqlParameter("@OrderId", DDOrderNo.SelectedIndex > 0 ? DDOrderNo.SelectedValue : "0");

            //*************
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getLoomBalanceqty", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("Loom Bal");
                //***********
                sht.Row(1).Height = 24;
                sht.Range("A1:H1").Merge();
                sht.Range("A1:H1").Style.Font.FontSize = 10;
                sht.Range("A1:H1").Style.Font.Bold = true;
                sht.Range("A1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:H1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1:H1").Style.Alignment.WrapText = true;
                //************
                sht.Range("A1").SetValue("LOOM BAL. UP TO (" + txtFromdate.Text + ") " + FilterBy);
                //Detail headers                
                sht.Range("A2:H2").Style.Font.FontSize = 10;
                sht.Range("A2:H2").Style.Font.Bold = true;
                sht.Range("G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("A2").Value = "UNIT NAME";
                sht.Range("B2").Value = "LOOM NO.";
                sht.Range("C2").Value = "FOLIO NO.";
                sht.Range("D2").Value = "QUALITY";
                sht.Range("E2").Value = "COLOR";
                sht.Range("F2").Value = "SIZE";
                sht.Range("G2").Value = "Bal. Qty";
                sht.Range("H2").Value = "ISSUE DATE";

                int row = 3;

                //**********Sorting
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "unitname,Issueorderid,LoomNo";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());
                //***************

                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    sht.Range("G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["unitname"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Issueorderid"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Designname"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Colorname"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["assigndate"]);
                    row = row + 1;
                }
                ds.Dispose();
                ds1.Dispose();
                //**************grand Total
                var sum = sht.Evaluate("SUM(G3:G" + (row - 1) + ")");
                sht.Range("G" + row).Value = sum;
                sht.Range("G" + row).Style.Font.Bold = true;
                //************** Save
                String Path;
                sht.Columns(1, 12).AdjustToContents();
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("LoomBal_" + DateTime.Now + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('No records found')", true);
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = ex.Message;
        }
    }
    protected void btnprint_Click(object sender, EventArgs e)
    {
        if (RDLoombal.Checked == true)
        {
            Loombal();
            return;
        }
        if (RDMaterial.Checked == true)
        {
            Materialreport();
            return;
        }
        if (RDRMLoomBal.Checked == true)
        {
            if (ChkWithLotTagNo.Checked == true)
            {
                RMLoomBalanceWithLotTag();
                return;
            }
            else
            {
                RMLoomBalance();
                return;
            }
            
        }
        if (RDFolioWiseMaterialIssWithConsumption.Checked == true)
        {
            FolioWiseMaterialIssWithConsumption();
            return;
        }
        if (Session["varMasterCompanyIDForERP"].ToString() == "8")
        {
            ReportForanisa();
        }
        else
        {
            ReportForOthers();
        }

    }
    protected void RMLoomBalance()
    {
        lblErrorMessage.Text = "";
        try
        {
            string FilterBy = "";
            if (DDjob.SelectedIndex > 0)
            {
                FilterBy = FilterBy + " JOB-" + DDjob.SelectedItem.Text;
            }
            if (DDUnitName.SelectedIndex > 0)
            {
                FilterBy = FilterBy + " UNIT-" + DDUnitName.SelectedItem.Text;
            }
            if (DDLoomNo.SelectedIndex > 0)
            {
                FilterBy = FilterBy + " LOOM NO-" + DDLoomNo.SelectedItem.Text;
            }
            if (DDArticle.SelectedIndex > 0)
            {
                FilterBy = FilterBy + " ARTICLE-" + DDArticle.SelectedItem.Text;
            }
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETRAWMATERIALLOOMBALANCE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@Unitid", DDUnitName.SelectedValue);
            cmd.Parameters.AddWithValue("@LoomId", DDLoomNo.SelectedIndex > 0 ? DDLoomNo.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@Fromdate", txtFromdate.Text);
            cmd.Parameters.AddWithValue("@TOdate", txtToDate.Text);


            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************
            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                //***********
                sht.Row(1).Height = 24;
                sht.Range("A1:O1").Merge();
                sht.Range("A1:O1").Style.Font.FontSize = 10;
                sht.Range("A1:O1").Style.Font.Bold = true;
                sht.Range("A1:O1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:O1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1:O1").Style.Alignment.WrapText = true;
                //************
                sht.Range("A1").SetValue("LOOM BAL.-" + FilterBy);
                //Detail headers                
                sht.Range("A2:O2").Style.Font.FontSize = 10;
                sht.Range("A2:O2").Style.Font.Bold = true;

                sht.Range("A2").Value = "LOOM NO";
                sht.Range("B2").Value = "FOLIO NO";
                sht.Range("C2").Value = "ITEM";
                sht.Range("D2").Value = "QUALITY";
                sht.Range("E2").Value = "SHADE";
                sht.Range("F2").Value = "TYPE";
                sht.Range("G2").Value = "ISSUE PCS";
                sht.Range("H2").Value = "ISSUE SHADE";
                sht.Range("I2").Value = "BAZAR PCS";
                sht.Range("J2").Value = "LAGAT FOR ONE PCS";
                sht.Range("K2").Value = "TO BE RECVD";
                sht.Range("L2").Value = "ACTUAL WEIGHT";
                sht.Range("M2").Value = "SHADE WISE %";
                sht.Range("N2").Value = "SHADE WISE CARPET WEIGHT";
                sht.Range("O2").Value = "RM BAL. ON THIS FOLIO";
                sht.Range("H2:O2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                int row = 3;
                int Lastissueorderid = 0;
                //**********Sorting
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "prorderid,TYPE";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());
                //***************

                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {

                    sht.Range("H" + row + ":O" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    var Onepcslagattotal = ds.Tables[0].Compute("sum(ONEPCSLAGAT)", "prorderid=" + ds1.Tables[0].Rows[i]["prorderid"] + "");

                    if (Session["varMasterCompanyIDForERP"].ToString() == "21")
                    {
                        sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
                        sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Prorderid"]);
                    }
                    else
                    {
                        if (Lastissueorderid != Convert.ToInt32(ds1.Tables[0].Rows[i]["Prorderid"]))
                        {
                            sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
                            sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Prorderid"]);
                            Lastissueorderid = Convert.ToInt32(ds1.Tables[0].Rows[i]["Prorderid"]);
                        }
                    }


                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["item_name"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Shadecolorname"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Type"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Issqty"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["issueshadeqty"]);
                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Recqty"]);
                    if (Session["varMasterCompanyIDForERP"].ToString() == "21")
                    {
                        sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["ONEPCSLAGAT"]);
                    }
                    else
                    {
                        sht.Range("J" + row).FormulaA1 = "=H" + row + "/G" + row;
                        sht.Cell(row, "J").Style.NumberFormat.Format = "#,##0.000";
                    }

                    sht.Range("K" + row).FormulaA1 = "=I" + row + "*J" + row;
                    sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["Weightrec"]);
                    sht.Range("M" + row).FormulaA1 = "=J" + row + "/" + Onepcslagattotal + "*" + 100;
                    sht.Cell(row, "M").Style.NumberFormat.Format = "#,##0.00";
                    sht.Range("N" + row).FormulaA1 = "=M" + row + "*L" + row + "/100";
                    sht.Cell(row, "N").Style.NumberFormat.Format = "#,##0.000";
                    sht.Range("O" + row).FormulaA1 = "=H" + row + "-N" + row;
                    sht.Cell(row, "O").Style.NumberFormat.Format = "#,##0.000";

                    row = row + 1;
                }
                ds.Dispose();
                ds1.Dispose();

                sht.Columns(1, 25).AdjustToContents();
                //************** Save
                String Path;

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("LoomBalance_" + DateTime.Now + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('No records found')", true);
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = ex.Message;
        }
    }
    protected void Materialreport()
    {
        string FilterBy = "";
        if (DDjob.SelectedIndex > 0)
        {
            FilterBy = FilterBy + "  Job-" + DDjob.SelectedItem.Text;
        }
        if (DDUnitName.SelectedIndex > 0)
        {
            FilterBy = FilterBy + "  Unit-" + DDUnitName.SelectedItem.Text;
        }
        if (DDArticle.SelectedIndex > 0)
        {
            FilterBy = FilterBy + "  Article-" + DDArticle.SelectedItem.Text;
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlCommand cmd = new SqlCommand("PRO_GETMATERIALDETAILFORANISA", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Processid", DDjob.SelectedIndex > 0 ? DDjob.SelectedValue : "1");
            cmd.Parameters.AddWithValue("@unitsid", DDUnitName.SelectedIndex > 0 ? DDUnitName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@Item_id", DDArticle.SelectedIndex > 0 ? DDArticle.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@Fromdate", txtFromdate.Text);
            cmd.Parameters.AddWithValue("@ToDate", txtToDate.Text);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            sda.Fill(dt);
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dt);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel"));
                }

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");

                //*************
                //***********
                sht.Row(1).Height = 24;
                sht.Range("A1:J1").Merge();
                sht.Range("A1:J1").Style.Font.FontSize = 10;
                sht.Range("A1:J1").Style.Font.Bold = true;
                sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:J1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1:J1").Style.Alignment.WrapText = true;
                //************
                sht.Range("A1").SetValue("Material Detail  (From -" + txtFromdate.Text + " To-" + txtToDate.Text + ") " + FilterBy);

                sht.Range("A2:K2").Style.Font.FontSize = 10;
                sht.Range("A2:K2").Style.Font.Bold = true;

                sht.Range("A2").Value = "UNIT";
                sht.Range("B2").Value = "ARTICLE";
                sht.Range("C2").Value = "COLOUR";
                sht.Range("D2").Value = "SIZE";
                sht.Range("E2").Value = "PCS";
                sht.Range("F2").Value = "RAW MAT. ISSUED(kg)";
                sht.Range("G2").Value = "RAW CONSUMED(kg)";
                sht.Range("H2").Value = "RAW RETURNED(Kg)";
                sht.Range("I2").Value = "WASTE(Kg)";

                int row = 3;

                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["unitname"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Item_name"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Colorname"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Recqty"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Rawiss"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Rawrec"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Consumedqty"]);
                    sht.Range("I" + row).FormulaA1 = "=F" + row + '-' + "($H$" + row + '+' + "$G$" + row + ")";

                    row = row + 1;
                }

                ds1.Dispose();

                //************** Save
                String Path;
                sht.Columns(1, 12).AdjustToContents();
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("Materialreport_" + DateTime.Now + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt3", "alert('No Record Found');", true);
            }

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }



    }
    protected void ReportForanisa()
    {
        lblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] array = new SqlParameter[10];
            array[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
            array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
            array[2] = new SqlParameter("@ArticleId", SqlDbType.Int);
            array[3] = new SqlParameter("@Fromdate", SqlDbType.SmallDateTime);
            array[4] = new SqlParameter("@Todate", SqlDbType.SmallDateTime);
            array[5] = new SqlParameter("@Pending", SqlDbType.Int);
            array[6] = new SqlParameter("@Packing", SqlDbType.Int);
            array[7] = new SqlParameter("@UserId", SqlDbType.Int);
            array[8] = new SqlParameter("@UnitsId", SqlDbType.TinyInt);
            array[9] = new SqlParameter("@Mastercompanyid", SqlDbType.Int);

            array[0].Value = Session["CurrentWorkingCompanyID"];
            array[1].Value = DDjob.SelectedValue;
            array[2].Value = DDArticle.SelectedValue;
            array[3].Value = txtFromdate.Text;
            array[4].Value = txtToDate.Text;
            array[5].Value = RDPendingQty.Checked == true ? 1 : (RDIssueDetail.Checked == true ? 2 : 0);
            array[6].Value = RDPacking.Checked == true ? 1 : 0;
            array[7].Value = Session["varUserid"];
            array[8].Value = DDUnitName.SelectedIndex <= 0 ? "0" : DDUnitName.SelectedValue;
            array[9].Value = Session["varcompanyNo"];

            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_ForJobWise_Rec_Pending_packing", array);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //for Image
                ds.Tables[1].Columns.Add("Image", typeof(System.Byte[]));
                foreach (DataRow dr in ds.Tables[1].Rows)
                {

                    if (Convert.ToString(dr["Photo"]) != "")
                    {
                        FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                        if (TheFile.Exists)
                        {
                            string img = dr["Photo"].ToString();
                            img = Server.MapPath(img);
                            Byte[] img_Byte = File.ReadAllBytes(img);
                            dr["Image"] = img_Byte;
                        }
                    }
                }
                if (RDRecReport.Checked == true)
                {
                    Session["rptFileName"] = "~\\Reports\\RptJobWiseRecQty.rpt";
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\RptJobWiseRecQty.xsd";
                }
                else if (RDPendingQty.Checked == true)
                {
                    switch (Session["varcompanyNo"].ToString())
                    {
                        case "8":
                            Session["rptFileName"] = "~\\Reports\\RptJobWisePendingQty.rpt";
                            break;
                        default:
                            Session["rptFileName"] = "~\\Reports\\RptJobWisePendingQtyOther.rpt";
                            break;
                    }
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\RptJobWisePendingQty.xsd";
                }
                else if (RDPacking.Checked == true)
                {

                    Session["rptFileName"] = "~\\Reports\\RptPackingQty.rpt";
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\RptPackingQty.xsd";
                }
                else if (RDIssueDetail.Checked == true)
                {

                    Session["rptFileName"] = "~\\Reports\\RptJobWiseIssueDetail.rpt";
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\RptJobWiseIssueDetail.xsd";
                }
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Information", "alert('No records found');", true);

            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }

    }
    protected void ReportForOthers()
    {
        lblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] array = new SqlParameter[14];
            array[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
            array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
            array[2] = new SqlParameter("@ArticleId", SqlDbType.Int);
            array[3] = new SqlParameter("@Fromdate", SqlDbType.SmallDateTime);
            array[4] = new SqlParameter("@Todate", SqlDbType.SmallDateTime);
            array[5] = new SqlParameter("@Pending", SqlDbType.Int);
            array[6] = new SqlParameter("@Packing", SqlDbType.Int);
            array[7] = new SqlParameter("@UserId", SqlDbType.Int);
            array[8] = new SqlParameter("@UnitsId", SqlDbType.TinyInt);
            array[9] = new SqlParameter("@Mastercompanyid", SqlDbType.Int);
            array[10] = new SqlParameter("@Withstockdetail", SqlDbType.Int);
            array[11] = new SqlParameter("@ForLoomStockNo", SqlDbType.Int);
            array[12] = new SqlParameter("@CustomerID", SqlDbType.Int);
            array[13] = new SqlParameter("@OrderID", SqlDbType.Int);

            array[0].Value = Session["CurrentWorkingCompanyID"];
            array[1].Value = DDjob.SelectedValue;
            array[2].Value = DDArticle.SelectedValue;
            array[3].Value = txtFromdate.Text;
            array[4].Value = txtToDate.Text;
            array[5].Value = RDPendingQty.Checked == true ? 1 : (RDIssueDetail.Checked == true ? 2 : 0);
            array[6].Value = RDPacking.Checked == true ? 1 : 0;
            array[7].Value = Session["varUserid"];
            array[8].Value = DDUnitName.SelectedIndex <= 0 ? "0" : DDUnitName.SelectedValue;
            array[9].Value = Session["varcompanyNo"];
            array[10].Value = chkwithstockdetail.Checked == true ? "1" : "0";
            array[11].Value = TDForLoomStockNo.Visible == true ? ChkForLoomStock.Checked == true ? "1" : "0" : "0";
            array[12].Value = TrCustomerCode.Visible == true ? DDCustomerOrderNo.SelectedIndex <= 0 ? "0" : DDCustomerOrderNo.SelectedValue : "0";
            array[13].Value = TrOrderNo.Visible == true ? DDOrderNo.SelectedIndex <= 0 ? "0" : DDOrderNo.SelectedValue : "0";

            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_ForJobWise_Rec_Pending_packingOthers", array);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //for Image
                ds.Tables[1].Columns.Add("Image", typeof(System.Byte[]));
                foreach (DataRow dr in ds.Tables[1].Rows)
                {

                    if (Convert.ToString(dr["Photo"]) != "")
                    {
                        FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                        if (TheFile.Exists)
                        {
                            string img = dr["Photo"].ToString();
                            img = Server.MapPath(img);
                            Byte[] img_Byte = File.ReadAllBytes(img);
                            dr["Image"] = img_Byte;
                        }
                    }
                }
                if (RDRecReport.Checked == true)
                {
                    Session["rptFileName"] = "~\\Reports\\RptJobWiseRecQty.rpt";
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\RptJobWiseRecQty.xsd";
                }
                else if (RDPendingQty.Checked == true)
                {
                    switch (Session["varcompanyNo"].ToString())
                    {
                        case "8":
                            Session["rptFileName"] = "~\\Reports\\RptJobWisePendingQty.rpt";
                            break;
                        default:
                            if (chkexcelexport.Checked == true && chkwithstockdetail.Checked == false)
                            {
                                JobwisependingExcelExport(ds);
                                return;
                            }
                            else if (chkexcelexport.Checked == true && chkwithstockdetail.Checked == true)
                            {
                                JobwisependingExcelExport_WithstockDetail(ds);
                                return;
                            }
                            else
                            {
                                Session["rptFileName"] = "~\\Reports\\RptJobWisePendingQtyOther.rpt";
                            }
                            break;
                    }
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\RptJobWisePendingQty.xsd";
                }
                else if (RDPacking.Checked == true)
                {

                    Session["rptFileName"] = "~\\Reports\\RptPackingQty.rpt";
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\RptPackingQty.xsd";
                }
                else if (RDIssueDetail.Checked == true)
                {
                    if (chkexcelexport.Checked == true && chkwithstockdetail.Checked == false)
                    {
                        JobwiseissueExcelExport(ds);
                        return;
                    }
                    else if (chkexcelexport.Checked == true && chkwithstockdetail.Checked == true)
                    {
                        JobwiseissueExcelExport_WithstockDetail(ds);
                        return;
                    }
                    else if (chkexcelexport.Checked == true && chkwithstockdetail.Checked == true && ChkForLoomStock.Checked==true)
                    {
                        JobwiseissueExcelExport_WithLoomStockDetail(ds);
                        return;
                    }
                    else
                    {
                        Session["rptFileName"] = "~\\Reports\\RptJobWiseIssueDetail.rpt";
                        Session["GetDataset"] = ds;
                        Session["dsFileName"] = "~\\ReportSchema\\RptJobWiseIssueDetail.xsd";
                    }
                }
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Information", "alert('No records found');", true);

            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }

    }
    protected void DDUnitName_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (Convert.ToString(DDjob.SelectedItem.Text.ToUpper()))
        {
            case "WEAVING":
                if (RDLoombal.Checked == true)
                {
                    TrLoomNo.Visible = true;
                    UtilityModule.ConditionalComboFill(ref DDLoomNo, @"select Distinct PLM.UID,PLM.LoomNo+'/'+IM.ITEM_NAME as LoomNo,case when ISNUMERIC(loomno)=1 Then CONVERT(int,loomno) Else 9999999 End as Loom from Process_issue_master_" + DDjob.SelectedValue + @" PIM inner join ProductionLoomMaster PLM on PIM.LoomId=PLM.UID
                                                and PIM.Status<>'Canceled' inner join ITEM_MASTER Im on PLm.Itemid=IM.ITEM_ID Where PLm.UnitId=" + DDUnitName.SelectedValue + @"
                                                order by Loom,loomno", true, "--Plz Select--");
                }
                break;
            default:
                if (RDRMLoomBal.Checked == true || RDFolioWiseMaterialIssWithConsumption.Checked == true)
                {
                    TrLoomNo.Visible = true;
                    UtilityModule.ConditionalComboFill(ref DDLoomNo, @"select Distinct PLM.UID,PLM.LoomNo+'/'+IM.ITEM_NAME as LoomNo,case when ISNUMERIC(loomno)=1 Then CONVERT(int,loomno) Else 9999999 End as Loom from Process_issue_master_1 PIM inner join ProductionLoomMaster PLM on PIM.LoomId=PLM.UID
                                                and PIM.Status<>'Canceled' inner join ITEM_MASTER Im on PLm.Itemid=IM.ITEM_ID Where PLm.UnitId=" + DDUnitName.SelectedValue + @"
                                                order by Loom,loomno", true, "--Plz Select--");
                }
                else
                {
                    TrLoomNo.Visible = false;
                }
                break;
        }
    }
    protected void DDLoomNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RDFolioWiseMaterialIssWithConsumption.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDFolioNo, @"select Distinct PIM.IssueOrderId,isnull(PIM.ChallanNo,PIM.IssueOrderId) as ChallanNo from Process_issue_master_1 PIM 
                                                Where PIM.Status<>'Canceled' and PIM.LoomId=" + DDLoomNo.SelectedValue + @" and PIM.Units=" + DDUnitName.SelectedValue + @"
                                                order by PIM.IssueOrderId", true, "--Plz Select--");
        }
        else
        {
            TRFolioNo.Visible = false;
            DDFolioNo.Items.Clear();
        }
    }
    protected void JobwisependingExcelExport(DataSet ds)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            string FilterBy = "";
            if (DDjob.SelectedIndex > 0)
            {
                FilterBy = FilterBy + "  Job-" + DDjob.SelectedItem.Text;
            }
            if (DDUnitName.SelectedIndex > 0)
            {
                FilterBy = FilterBy + "  Unit-" + DDUnitName.SelectedItem.Text;
            }
            if (DDArticle.SelectedIndex > 0)
            {
                FilterBy = FilterBy + "  Article-" + DDArticle.SelectedItem.Text;
            }

            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Job_Article Wise Pending");

            //*************
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:J1").Merge();
            sht.Range("A1:J1").Style.Font.FontSize = 10;
            sht.Range("A1:J1").Style.Font.Bold = true;
            sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:J1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:J1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("JOB/ARTICLE WISE PENDING UP TO (" + txtFromdate.Text + ") " + FilterBy);

            sht.Range("A2:K2").Style.Font.FontSize = 10;
            sht.Range("A2:K2").Style.Font.Bold = true;

            sht.Range("A2").Value = "ISSUE DATE";
            sht.Range("B2").Value = "ISSUE NO";
            sht.Range("C2").Value = "UNIT NAME";
            sht.Range("D2").Value = "JOB NAME";
            sht.Range("E2").Value = "QUALITY";
            sht.Range("F2").Value = "COLOR";
            sht.Range("G2").Value = "SIZE";
            sht.Range("H2").Value = "QTY";
            sht.Range("I2").Value = "EMPLOYEE";
            sht.Range("J2").Value = "REMARK";

            int row = 3;
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "issuedate,issueorderid";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["issuedate"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["issueorderid"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Unitsname"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Process"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Itemname"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Colorname"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["QTY"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Employee"]);
                row = row + 1;
            }
            ds.Dispose();
            ds1.Dispose();
            //**************grand Total
            var sum = sht.Evaluate("SUM(H3:H" + (row - 1) + ")");
            sht.Range("H" + row).SetValue(sum);
            sht.Range("H" + row).Style.Font.Bold = true;
            //************** Save
            String Path;
            sht.Columns(1, 12).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("Job/ArticleWisePending_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "JobPendalt", "alert('No records found..')", true);
        }
    }
    protected void JobwisependingExcelExport_WithstockDetail(DataSet ds)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            string FilterBy = "";
            if (DDjob.SelectedIndex > 0)
            {
                FilterBy = FilterBy + "  Job-" + DDjob.SelectedItem.Text;
            }
            if (DDUnitName.SelectedIndex > 0)
            {
                FilterBy = FilterBy + "  Unit-" + DDUnitName.SelectedItem.Text;
            }
            if (DDArticle.SelectedIndex > 0)
            {
                FilterBy = FilterBy + "  Article-" + DDArticle.SelectedItem.Text;
            }

            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Job_Article Wise Pending");

            //*************
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:K1").Merge();
            sht.Range("A1:K1").Style.Font.FontSize = 10;
            sht.Range("A1:K1").Style.Font.Bold = true;
            sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:K1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:K1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("JOB/ARTICLE WISE PENDING UP TO (" + txtFromdate.Text + ") " + FilterBy);

            sht.Range("A2:K2").Style.Font.FontSize = 10;
            sht.Range("A2:K2").Style.Font.Bold = true;

            sht.Range("A2").Value = "ISSUE DATE";
            sht.Range("B2").Value = "ISSUE NO";
            sht.Range("C2").Value = "STOCK NO";
            sht.Range("D2").Value = "UNIT NAME";
            sht.Range("E2").Value = "JOB NAME";
            sht.Range("F2").Value = "QUALITY";
            sht.Range("G2").Value = "COLOR";
            sht.Range("H2").Value = "SIZE";
            sht.Range("I2").Value = "QTY";
            sht.Range("J2").Value = "EMPLOYEE";
            sht.Range("K2").Value = "REMARK";

            int row = 3;
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "issuedate,issueorderid,Tstockno";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["issuedate"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["issueorderid"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["TSTOCKNO"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Unitsname"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Process"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Itemname"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Colorname"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["QTY"]);
                sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Employee"]);
                //sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Remark"]);
                row = row + 1;
            }
            ds.Dispose();
            ds1.Dispose();
            //**************grand Total
            var sum = sht.Evaluate("SUM(I3:I" + (row - 1) + ")");
            sht.Range("I" + row).SetValue(sum);
            sht.Range("I" + row).Style.Font.Bold = true;
            //************** Save
            String Path;
            sht.Columns(1, 12).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("Job/ArticleWisePending_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "JobPendalt", "alert('No records found..')", true);
        }
    }
    protected void JobwiseissueExcelExport(DataSet ds)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            string FilterBy = "";
            if (DDjob.SelectedIndex > 0)
            {
                FilterBy = FilterBy + "  Job-" + DDjob.SelectedItem.Text;
            }
            if (DDUnitName.SelectedIndex > 0)
            {
                FilterBy = FilterBy + "  Unit-" + DDUnitName.SelectedItem.Text;
            }
            if (DDArticle.SelectedIndex > 0)
            {
                FilterBy = FilterBy + "  Article-" + DDArticle.SelectedItem.Text;
            }

            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Job_Article Wise Issue");

            //*************
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:K1").Merge();
            sht.Range("A1:K1").Style.Font.FontSize = 10;
            sht.Range("A1:K1").Style.Font.Bold = true;
            sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:K1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:K1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("JOB/ARTICLE WISE ISSUE (From -" + txtFromdate.Text + " To-" + txtToDate.Text + ") " + FilterBy);

            sht.Range("A2:K2").Style.Font.FontSize = 10;
            sht.Range("A2:K2").Style.Font.Bold = true;

            sht.Range("A2").Value = "ISSUE DATE";
            sht.Range("B2").Value = "ISSUE NO";
            sht.Range("C2").Value = "UNIT NAME";
            sht.Range("D2").Value = "JOB NAME";
            sht.Range("E2").Value = "QUALITY";
            sht.Range("F2").Value = "COLOR";
            sht.Range("G2").Value = "SIZE";
            sht.Range("H2").Value = "QTY";
            sht.Range("I2").Value = "EMPLOYEE";
            sht.Range("J2").Value = "REMARK";
            sht.Range("K2").Value = "LOOM NO.";

            int row = 3;
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "issuedate,issueorderid";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["issuedate"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["issueorderid"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Unitsname"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Processname"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Articlename"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Colorname"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["QTY"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Employee"]);
                sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
                row = row + 1;
            }
            ds.Dispose();
            ds1.Dispose();
            //**************grand Total
            var sum = sht.Evaluate("SUM(H3:H" + (row - 1) + ")");
            sht.Range("H" + row).SetValue(sum);
            sht.Range("H" + row).Style.Font.Bold = true;
            //************** Save
            String Path;
            sht.Columns(1, 12).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("Job/ArticleWiseissue_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "JobPendalt", "alert('No records found..')", true);
        }
    }
    protected void JobwiseissueExcelExport_WithstockDetail(DataSet ds)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            string FilterBy = "";
            if (DDjob.SelectedIndex > 0)
            {
                FilterBy = FilterBy + "  Job-" + DDjob.SelectedItem.Text;
            }
            if (DDUnitName.SelectedIndex > 0)
            {
                FilterBy = FilterBy + "  Unit-" + DDUnitName.SelectedItem.Text;
            }
            if (DDArticle.SelectedIndex > 0)
            {
                FilterBy = FilterBy + "  Article-" + DDArticle.SelectedItem.Text;
            }

            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Job_Article Wise Issue");

            //*************
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:L1").Merge();
            sht.Range("A1:L1").Style.Font.FontSize = 10;
            sht.Range("A1:L1").Style.Font.Bold = true;
            sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:L1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:L1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("JOB/ARTICLE WISE ISSUE (From -" + txtFromdate.Text + " To-" + txtToDate.Text + ") " + FilterBy);

            sht.Range("A2:M2").Style.Font.FontSize = 10;
            sht.Range("A2:M2").Style.Font.Bold = true;

            sht.Range("A2").Value = "ISSUE DATE";
            sht.Range("B2").Value = "ISSUE NO";
            sht.Range("C2").Value = "STOCK NO";
            sht.Range("D2").Value = "UNIT NAME";
            sht.Range("E2").Value = "JOB NAME";
            sht.Range("F2").Value = "QUALITY";
            sht.Range("G2").Value = "COLOR";
            sht.Range("H2").Value = "SIZE";
            sht.Range("I2").Value = "QTY";
            sht.Range("J2").Value = "EMPLOYEE";
            sht.Range("K2").Value = "REMARK";
            if (Session["varcompanyNo"].ToString() == "22" && DDjob.SelectedItem.Text=="PACKING")
            {
                sht.Range("L2").Value = "ECIS NO";

                sht.Range("M2").Value = "DATE STAMP";
               
            }
            else {
                sht.Range("L2").Value = "LOOM NO.";
            }

            int row = 3;
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "issuedate,issueorderid,Tstockno";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["issuedate"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["issueorderid"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Tstockno"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Unitsname"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Processname"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Articlename"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Colorname"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["QTY"]);
                sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Employee"]);
                sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Remark"]);
                if (Session["varcompanyNo"].ToString() == "22" && DDjob.SelectedItem.Text == "PACKING")
                {
                    sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["ECISNO"]);
                    sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["JOBISSUEDATESTAMP"]);
                }
                else
                {
                    sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
                }
                row = row + 1;
            }
            ds.Dispose();
            ds1.Dispose();
            //**************grand Total
            var sum = sht.Evaluate("SUM(I3:I" + (row - 1) + ")");
            sht.Range("I" + row).SetValue(sum);
            sht.Range("I" + row).Style.Font.Bold = true;
            //************** Save
            String Path;
            sht.Columns(1, 12).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("Job/ArticleWiseissue_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "JobPendalt", "alert('No records found..')", true);
        }
    }
    //protected void RadioButton_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (RDFolioWiseMaterialIssWithConsumption.Checked == true)
    //    {
    //        DDcategory_SelectedIndexChanged(DDcategory, new EventArgs());
    //    }
    //    else
    //    {
    //        TRItemName.Visible = false;
    //        TRDDQuality.Visible = false;
    //        TRDDDesign.Visible = false;
    //        TRDDColor.Visible = false;
    //        TRDDShadeColor.Visible = false;
    //        TRDDShape.Visible = false;
    //        TRDDSize.Visible = false;
    //        ddItemName.Items.Clear();
    //        DDQuality.Items.Clear();
    //        DDDesign.Items.Clear();

    //    }
    //}
    protected void DDcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RDFolioWiseMaterialIssWithConsumption.Checked == true)
        {
            TRItemName.Visible = true;
            string str = "select Item_id,ITEM_NAME From Item_master where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + "";
            if (DDcategory.SelectedIndex > 0)
            {
                str = str + " and Category_id=" + DDcategory.SelectedValue;
            }
            str = str + " order by ITEM_NAME";
            UtilityModule.ConditionalComboFill(ref ddItemName, str, true, "ALL");
            if (ddItemName.Items.Count > 0)
            {
                ddItemName_SelectedIndexChanged(sender, new EventArgs());
            }
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDArticle, @"select IM.ITEM_ID,IM.ITEM_NAME From Item_Master IM inner join CategorySeparate CS on IM.CATEGORY_ID=CS.Categoryid
                                                            and CS.id=0 and cs.categoryid=" + DDcategory.SelectedValue + " and IM.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by IM.ITEM_NAME", true, "--Plz Select--");
        }

    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        TRDDQuality.Visible = true;
        if (ddItemName.SelectedItem.Text.Trim() != "ALL")
        {
            UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality Where Item_Id=" + ddItemName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by QualityName", true, "ALL");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  order by QualityName", true, "ALL");
        }
        DDQuality_SelectedIndexChanged(sender, e);
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {

        TRDDDesign.Visible = false;
        TRDDColor.Visible = false;
        TRDDShadeColor.Visible = false;
        TRDDShape.Visible = false;
        TRDDSize.Visible = false;

        string qry = @"select DESIGNID,DESIGNNAME from DESIGN Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" ORDER BY DESIGNNAME
        select COLORID,COLORNAME from color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" ORDER BY COLORNAME
        SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" ORDER BY SHADECOLORNAME
        SELECT SHAPEID, SHAPENAME FROM SHAPE Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " ORDER BY SHAPENAME";
        DataSet ds = SqlHelper.ExecuteDataset(qry);

        string str = @"select PARAMETER_ID from ITEM_CATEGORY_PARAMETERS where category_id=" + DDcategory.SelectedValue;
        DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds1.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                switch (dr["Parameter_Id"].ToString())
                {
                    case "2":
                        TRDDDesign.Visible = true;
                        UtilityModule.ConditionalComboFillWithDS(ref DDDesign, ds, 0, true, "ALL");
                        break;
                    case "3":
                        TRDDColor.Visible = true;
                        UtilityModule.ConditionalComboFillWithDS(ref DDColor, ds, 1, true, "ALL");
                        break;
                    case "4":
                        TRDDShape.Visible = true;
                        UtilityModule.ConditionalComboFillWithDS(ref DDShape, ds, 3, true, "ALL");
                        break;
                    case "5":
                        TRDDSize.Visible = true;
                        break;
                    case "6":
                        TRDDShadeColor.Visible = true;
                        UtilityModule.ConditionalComboFillWithDS(ref DDShadeColor, ds, 2, true, "ALL");
                        break;
                }
            }
        }

    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        TRDDSize.Visible = true;
        string str = @"SELECT SIZEID, SIZEft AS SIZENAME FROM SIZE WHERE MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        if (DDShape.SelectedIndex > 0)
        {
            str = str + " and SHAPEID=" + DDShape.SelectedValue;
        }
        str = str + "ORDER BY SIZEID";
        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "ALL");
    }
    protected void FolioWiseMaterialIssWithConsumption()
    {
        lblErrorMessage.Text = "";
        try
        {

            string str = "";
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + " and VF2.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and VF2.Qualityid=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and VF2.DesignId=" + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " and VF2.colorid=" + DDColor.SelectedValue;
            }
            if (DDShape.SelectedIndex > 0)
            {
                str = str + " and VF2.shapeid=" + DDShape.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and VF2.sizeid=" + DDSize.SelectedValue;
            }


            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETFolioWiseMaterialIssWithConsumptionReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@Unitid", DDUnitName.SelectedValue);
            cmd.Parameters.AddWithValue("@LoomId", DDLoomNo.SelectedIndex > 0 ? DDLoomNo.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@FolioNo", DDFolioNo.SelectedIndex > 0 ? DDFolioNo.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@Fromdate", txtFromdate.Text);
            cmd.Parameters.AddWithValue("@TOdate", txtToDate.Text);
            cmd.Parameters.AddWithValue("@Where", str);



            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************
            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                //***********
                sht.Row(1).Height = 24;
                sht.Range("A1:O1").Merge();
                sht.Range("A1:O1").Style.Font.FontSize = 10;
                sht.Range("A1:O1").Style.Font.Bold = true;
                sht.Range("A1:O1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:O1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1:O1").Style.Alignment.WrapText = true;
                //************
                sht.Range("A1").SetValue("MATERIAL CONSUMPTION DETAIL.");
                //Detail headers                
                sht.Range("A2:R2").Style.Font.FontSize = 10;
                sht.Range("A2:R2").Style.Font.Bold = true;

                sht.Range("A2").Value = "LOOM NO";
                sht.Range("B2").Value = "FOLIO NO";
                sht.Range("C2").Value = "ITEM DESCRIPTION";
                sht.Range("D2").Value = "BEAM DESCRIPTION";

                sht.Range("E2").Value = "ITEM";
                sht.Range("F2").Value = "QUALITY";
                sht.Range("G2").Value = "SHADE";
                sht.Range("H2").Value = "TYPE";
                sht.Range("I2").Value = "ISSUE PCS";
                sht.Range("J2").Value = "ISSUE SHADE";
                sht.Range("K2").Value = "BAZAR PCS";
                sht.Range("L2").Value = "LAGAT FOR ONE PCS";
                sht.Range("M2").Value = "TO BE RECVD";
                sht.Range("N2").Value = "ACTUAL WEIGHT";
                sht.Range("O2").Value = "SHADE WISE %";
                sht.Range("P2").Value = "SHADE WISE CARPET WEIGHT";
                sht.Range("Q2").Value = "RM BAL. ON THIS FOLIO";
                if (Session["varMasterCompanyIDForERP"].ToString() == "14")
                {
                    sht.Range("R2").Value = "BALANCE PCS";
                }
                sht.Range("H2:R2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                int row = 3;
                int Lastissueorderid = 0;
                //**********Sorting
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "prorderid,TYPE";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());
                //***************

                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {

                    sht.Range("H" + row + ":R" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    var Onepcslagattotal = ds.Tables[0].Compute("sum(ONEPCSLAGAT)", "prorderid=" + ds1.Tables[0].Rows[i]["prorderid"] + "");

                    //if (Session["varMasterCompanyIDForERP"].ToString() == "21")
                    //{
                    //    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
                    //    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Prorderid"]);
                    //}
                    //else
                    //{
                    //    if (Lastissueorderid != Convert.ToInt32(ds1.Tables[0].Rows[i]["Prorderid"]))
                    //    {
                    //        sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
                    //        sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Prorderid"]);
                    //        Lastissueorderid = Convert.ToInt32(ds1.Tables[0].Rows[i]["Prorderid"]);
                    //    }
                    //}



                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Prorderid"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["ItemDescription"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["BeamDescription"]);

                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["item_name"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Shadecolorname"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Type"]);
                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Issqty"]);
                    sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["issueshadeqty"]);
                    sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Recqty"]);
                    sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["ONEPCSLAGAT"]);


                    sht.Range("M" + row).FormulaA1 = "=K" + row + "*L" + row;
                    sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["Weightrec"]);
                    sht.Range("O" + row).FormulaA1 = "=L" + row + "/" + Onepcslagattotal + "*" + 100;
                    sht.Cell(row, "O").Style.NumberFormat.Format = "#,##0.00";
                    sht.Range("P" + row).FormulaA1 = "=O" + row + "*N" + row + "/100";
                    sht.Cell(row, "P").Style.NumberFormat.Format = "#,##0.000";
                    sht.Range("Q" + row).FormulaA1 = "=J" + row + "-P" + row;
                    sht.Cell(row, "Q").Style.NumberFormat.Format = "#,##0.000";

                    if (Session["varMasterCompanyIDForERP"].ToString() == "14")
                    {
                        sht.Range("R" + row).FormulaA1 = "=I" + row + "-K" + row;
                        //sht.Cell(row, "R").Style.NumberFormat.Format = "#,##0.000";
                    }

                    row = row + 1;
                }
                ds.Dispose();
                ds1.Dispose();

                sht.Columns(1, 27).AdjustToContents();
                //************** Save
                String Path;

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("MaterialConsumptionDetail_" + DateTime.Now + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('No records found')", true);
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = ex.Message;
        }
    }
    protected void RMLoomBalanceWithLotTag()
    {
        lblErrorMessage.Text = "";
        try
        {
            string FilterBy = "";
            if (DDjob.SelectedIndex > 0)
            {
                FilterBy = FilterBy + " JOB-" + DDjob.SelectedItem.Text;
            }
            if (DDUnitName.SelectedIndex > 0)
            {
                FilterBy = FilterBy + " UNIT-" + DDUnitName.SelectedItem.Text;
            }
            if (DDLoomNo.SelectedIndex > 0)
            {
                FilterBy = FilterBy + " LOOM NO-" + DDLoomNo.SelectedItem.Text;
            }
            if (DDArticle.SelectedIndex > 0)
            {
                FilterBy = FilterBy + " ARTICLE-" + DDArticle.SelectedItem.Text;
            }
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETRAWMATERIALLOOMBALANCEWITHLOTTAG", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@Unitid", DDUnitName.SelectedValue);
            cmd.Parameters.AddWithValue("@LoomId", DDLoomNo.SelectedIndex > 0 ? DDLoomNo.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@Fromdate", txtFromdate.Text);
            cmd.Parameters.AddWithValue("@TOdate", txtToDate.Text);


            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************
            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                //***********
                sht.Row(1).Height = 24;
                sht.Range("A1:O1").Merge();
                sht.Range("A1:O1").Style.Font.FontSize = 10;
                sht.Range("A1:O1").Style.Font.Bold = true;
                sht.Range("A1:O1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:O1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1:O1").Style.Alignment.WrapText = true;
                //************
                sht.Range("A1").SetValue("LOOM BAL.-" + FilterBy);
                //Detail headers                
                sht.Range("A2:O2").Style.Font.FontSize = 10;
                sht.Range("A2:O2").Style.Font.Bold = true;

                sht.Range("A2").Value = "LOOM NO";
                sht.Range("B2").Value = "FOLIO NO";
                sht.Range("C2").Value = "ITEM";
                sht.Range("D2").Value = "QUALITY";
                sht.Range("E2").Value = "SHADE";

                sht.Range("F2").Value = "LOTNO";
                sht.Range("G2").Value = "TAGNO";

                sht.Range("H2").Value = "TYPE";
                sht.Range("I2").Value = "ISSUE PCS";
                sht.Range("J2").Value = "ISSUE SHADE";
                sht.Range("K2").Value = "BAZAR PCS";
                sht.Range("L2").Value = "LAGAT FOR ONE PCS";
                sht.Range("M2").Value = "TO BE RECVD";
                sht.Range("N2").Value = "ACTUAL WEIGHT";
                sht.Range("O2").Value = "SHADE WISE %";
                sht.Range("P2").Value = "SHADE WISE CARPET WEIGHT";
                sht.Range("Q2").Value = "RM BAL. ON THIS FOLIO";

                //sht.Range("F2").Value = "TYPE";
                //sht.Range("G2").Value = "ISSUE PCS";
                //sht.Range("H2").Value = "ISSUE SHADE";
                //sht.Range("I2").Value = "BAZAR PCS";
                //sht.Range("J2").Value = "LAGAT FOR ONE PCS";
                //sht.Range("K2").Value = "TO BE RECVD";
                //sht.Range("L2").Value = "ACTUAL WEIGHT";
                //sht.Range("M2").Value = "SHADE WISE %";
                //sht.Range("N2").Value = "SHADE WISE CARPET WEIGHT";
                //sht.Range("O2").Value = "RM BAL. ON THIS FOLIO";
                sht.Range("H2:O2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                int row = 3;
                int Lastissueorderid = 0;
                //int Finishedid = 0;
                //decimal Onepcslagattotal = 0;
                //int FolioNo=0;
                //**********Sorting
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "prorderid,TYPE";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());
                //***************

                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {

                    sht.Range("J" + row + ":Q" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);                    
                    

                    var Onepcslagattotal = ds.Tables[1].Compute("sum(ONEPCSLAGAT)", "prorderid=" + ds1.Tables[0].Rows[i]["prorderid"] + "");

                    if (Session["varMasterCompanyIDForERP"].ToString() == "21")
                    {
                        sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
                        sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Prorderid"]);
                    }
                    else
                    {
                        if (Lastissueorderid != Convert.ToInt32(ds1.Tables[0].Rows[i]["Prorderid"]))
                        {
                            sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
                            sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Prorderid"]);
                            Lastissueorderid = Convert.ToInt32(ds1.Tables[0].Rows[i]["Prorderid"]);
                        }
                    }


                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["item_name"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Shadecolorname"]);

                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["LotNo"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["TagNo"]);

                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Type"]);
                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Issqty"]);
                    sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["issueshadeqty"]);
                    sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Recqty"]);
                    if (Session["varMasterCompanyIDForERP"].ToString() == "21")
                    {
                        sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["ONEPCSLAGAT"]);
                    }
                    else
                    {
                        sht.Range("L" + row).FormulaA1 = "=J" + row + "/I" + row;
                        sht.Cell(row, "L").Style.NumberFormat.Format = "#,##0.000";
                    }

                    sht.Range("M" + row).FormulaA1 = "=K" + row + "*L" + row;
                    sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["Weightrec"]);
                    sht.Range("O" + row).FormulaA1 = "=L" + row + "/" + Onepcslagattotal + "*" + 100;
                    sht.Cell(row, "O").Style.NumberFormat.Format = "#,##0.00";
                    sht.Range("P" + row).FormulaA1 = "=O" + row + "*N" + row + "/100";
                    sht.Cell(row, "P").Style.NumberFormat.Format = "#,##0.000";
                    sht.Range("Q" + row).FormulaA1 = "=J" + row + "-P" + row;
                    sht.Cell(row, "Q").Style.NumberFormat.Format = "#,##0.000";




                    //sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Type"]);
                    //sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Issqty"]);
                    //sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["issueshadeqty"]);
                    //sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Recqty"]);
                    //if (Session["varMasterCompanyIDForERP"].ToString() == "21")
                    //{
                    //    sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["ONEPCSLAGAT"]);
                    //}
                    //else
                    //{
                    //    sht.Range("J" + row).FormulaA1 = "=H" + row + "/G" + row;
                    //    sht.Cell(row, "J").Style.NumberFormat.Format = "#,##0.000";
                    //}

                    //sht.Range("K" + row).FormulaA1 = "=I" + row + "*J" + row;
                    //sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["Weightrec"]);
                    //sht.Range("M" + row).FormulaA1 = "=J" + row + "/" + Onepcslagattotal + "*" + 100;
                    //sht.Cell(row, "M").Style.NumberFormat.Format = "#,##0.00";
                    //sht.Range("N" + row).FormulaA1 = "=M" + row + "*L" + row + "/100";
                    //sht.Cell(row, "N").Style.NumberFormat.Format = "#,##0.000";
                    //sht.Range("O" + row).FormulaA1 = "=H" + row + "-N" + row;
                    //sht.Cell(row, "O").Style.NumberFormat.Format = "#,##0.000";

                    row = row + 1;
                }
                ds.Dispose();
                ds1.Dispose();

                sht.Columns(1, 25).AdjustToContents();
                //************** Save
                String Path;

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("LoomBalanceWithLotTag_" + DateTime.Now + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('No records found')", true);
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void DDjob_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (Convert.ToString(DDjob.SelectedItem.Text.ToUpper()))
        {
            case "WEAVING":
                if (RDIssueDetail.Checked == true)
                {
                    TDForLoomStockNo.Visible = true;                   
                }
                break;
            default:
                TDForLoomStockNo.Visible = false;
                ChkForLoomStock.Checked = false;
                break;
        }
    }
    protected void JobwiseissueExcelExport_WithLoomStockDetail(DataSet ds)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            string FilterBy = "";
            if (DDjob.SelectedIndex > 0)
            {
                FilterBy = FilterBy + "  Job-" + DDjob.SelectedItem.Text;
            }
            if (DDUnitName.SelectedIndex > 0)
            {
                FilterBy = FilterBy + "  Unit-" + DDUnitName.SelectedItem.Text;
            }
            if (DDArticle.SelectedIndex > 0)
            {
                FilterBy = FilterBy + "  Article-" + DDArticle.SelectedItem.Text;
            }

            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Job_Article Wise Issue");

            //*************
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:L1").Merge();
            sht.Range("A1:L1").Style.Font.FontSize = 10;
            sht.Range("A1:L1").Style.Font.Bold = true;
            sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:L1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:L1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("JOB/ARTICLE WISE ISSUE (From -" + txtFromdate.Text + " To-" + txtToDate.Text + ") " + FilterBy);

            sht.Range("A2:L2").Style.Font.FontSize = 10;
            sht.Range("A2:L2").Style.Font.Bold = true;

            sht.Range("A2").Value = "ISSUE DATE";
            sht.Range("B2").Value = "ISSUE NO";
            sht.Range("C2").Value = "STOCK NO";
            sht.Range("D2").Value = "UNIT NAME";
            sht.Range("E2").Value = "JOB NAME";
            sht.Range("F2").Value = "QUALITY";
            sht.Range("G2").Value = "COLOR";
            sht.Range("H2").Value = "SIZE";
            sht.Range("I2").Value = "QTY";
            sht.Range("J2").Value = "EMPLOYEE";
            sht.Range("K2").Value = "REMARK";
            sht.Range("L2").Value = "LOOM NO.";

            int row = 3;
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "issuedate,issueorderid,Tstockno";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["issuedate"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["issueorderid"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Tstockno"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Unitsname"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Processname"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Articlename"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Colorname"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["QTY"]);
                sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Employee"]);
                sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Remark"]);
                sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
                row = row + 1;
            }
            ds.Dispose();
            ds1.Dispose();
            //**************grand Total
            var sum = sht.Evaluate("SUM(I3:I" + (row - 1) + ")");
            sht.Range("I" + row).SetValue(sum);
            sht.Range("I" + row).Style.Font.Bold = true;
            //************** Save
            String Path;
            sht.Columns(1, 12).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("Job/ArticleWiseissue_WithStockDetail" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "JobPendalt", "alert('No records found..')", true);
        }
    }
    protected void DDCustomerOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDOrderNo, "Select OrderID, CustomerOrderNo From OrderMaster(Nolock) Where CustomerID = " + DDCustomerOrderNo.SelectedValue + " Order By CustomerOrderNo ", true, "Select");
    }
}