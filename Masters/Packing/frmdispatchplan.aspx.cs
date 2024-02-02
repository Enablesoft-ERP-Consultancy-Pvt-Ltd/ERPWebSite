using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Office2010.Drawing;
using System.IO;
using System.Text.RegularExpressions;

public partial class Masters_Packing_frmdispatchplan : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtstartdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtcompdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void btnimport_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        //********************************
        if (fileupload.HasFile)
        {
            //***********check File type
            if (Path.GetExtension(fileupload.FileName) != ".xlsx" && Path.GetExtension(fileupload.FileName) != ".xls")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "al4", "alert('Please select valid .xls or .xlsx file')", true);
                return;
            }
            //***********
            DataTable dt = new DataTable();
            dt.Columns.Add("QUALITY", typeof(string));
            dt.Columns.Add("COLOUR", typeof(string));
            dt.Columns.Add("SIZE", typeof(string));
            dt.Columns.Add("PACKTYPE", typeof(string));
            dt.Columns.Add("ARTICLENO", typeof(string));
            dt.Columns.Add("PACKPCS", typeof(int));
            dt.Columns.Add("PLANPCS", typeof(int));
            dt.Columns.Add("WIPPCS", typeof(int));
            dt.Columns.Add("ECISNO", typeof(string));
            dt.Columns.Add("DEST", typeof(string));
            dt.Columns.Add("PODATE", typeof(string));
            dt.Columns.Add("PONO", typeof(string));
            dt.Columns.Add("DTSTAMP", typeof(string));
            dt.Columns.Add("PlanID", typeof(int));
            dt.Columns.Add("PlanDetailid", typeof(int));
            dt.Columns.Add("Itemid", typeof(int));
            dt.Columns.Add("QualityId", typeof(int));
            dt.Columns.Add("DesignId", typeof(int));
            dt.Columns.Add("Colorid", typeof(int));
            dt.Columns.Add("shapeid", typeof(int));
            dt.Columns.Add("Sizeid", typeof(int));
            dt.Columns.Add("PackingTypeid", typeof(int));
            dt.Columns.Add("Ratedate", typeof(string));
            dt.Columns.Add("PalletNo", typeof(string));

            if (!Directory.Exists(Server.MapPath("~/Dispatchplan/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Dispatchplan/"));
            }
            fileupload.SaveAs(Server.MapPath("~/Dispatchplan/" + fileupload.FileName.ToString()));
            string filename = Server.MapPath("~/Dispatchplan/" + fileupload.FileName.ToString());
            using (var document = SpreadsheetDocument.Open(filename, true))
            {
                try
                {
                    WorkbookPart wbPart = document.WorkbookPart;
                    Sheet sheet = (Sheet)wbPart.Workbook.Sheets.FirstChild;
                    WorksheetPart wsp = (WorksheetPart)wbPart.GetPartById(sheet.Id);
                    for (int rNo = 3; rNo < 5000; rNo++)
                    {
                        if (wsp.Readcell("A" + rNo).Trim() == "")
                        {
                            break;
                        }
                        //Get quality,Design,color size from article creation

                        string articleno = "";
                        if (Session["varMasterCompanyIDForERP"].ToString() == "14")
                        {
                            double articleno2 = Convert.ToDouble(wsp.Readcell("C" + rNo).Trim());  //articleno
                            articleno = Convert.ToString(articleno2);
                        }
                        else
                        {                          
                            articleno = wsp.Readcell("C" + rNo).Trim();//articleno
                        }

                      
                        
                        DataTable dtitemdesc = UtilityModule.getarticledescription(articleno);
                        //                        
                        if (dtitemdesc.Rows.Count == 0)
                        {
                            lblmsg.Text = "Article No does not exists For Row Number " + rNo + ".'";
                            //ScriptManager.RegisterStartupScript(Page, GetType(), "al", "alert('Article No does not exists For Row Number " + rNo + ".')", true);
                            dt.Rows.Clear();
                            break;
                        }
                        //****Rate Date
                        string rateDate = wsp.Readcell("M" + rNo).Trim();
                        if (rateDate == "")
                        {
                            //lblmsg.Text = "Article No does not exists For Row Number " + rNo + ".'";
                            lblmsg.Text = "Rate Date can not be blank For Row Number " + rNo + ".'";
                            dt.Rows.Clear();
                            break;
                        }
                        //**************
                        DataRow dr = dt.NewRow();
                        dr["Quality"] = dtitemdesc.Rows[0]["Quality"];
                        dr["Colour"] = dtitemdesc.Rows[0]["ColorName"];
                        dr["Size"] = dtitemdesc.Rows[0]["Size"];
                        dr["PACktype"] = dtitemdesc.Rows[0]["PackingType"];
                        dr["Articleno"] = articleno;
                        dr["PACKPCS"] = 0;
                        dr["PLANPCS"] = wsp.Readcell("I" + rNo).Trim(); ;
                        dr["WIPPCS"] = wsp.Readcell("J" + rNo).Trim();
                        dr["ECISNO"] = Regex.Match(wsp.Readcell("A" + rNo).Trim(), @"\d+").Value;
                        dr["DEST"] = wsp.Readcell("E" + rNo).Trim();
                        dr["PODATE"] = DateTime.FromOADate(Convert.ToDouble(wsp.Readcell("F" + rNo))).ToString("dd-MMM-yyyy");
                        dr["PONO"] = wsp.Readcell("B" + rNo).Trim();
                        dr["DTSTAMP"] = wsp.Readcell("L" + rNo).Trim();
                        dr["PlanID"] = 0;
                        dr["PlanDetailid"] = 0;
                        dr["Itemid"] = dtitemdesc.Rows[0]["Item_id"]; ;
                        dr["QualityId"] = dtitemdesc.Rows[0]["qualityid"];
                        dr["DesignId"] = dtitemdesc.Rows[0]["Designid"];
                        dr["Colorid"] = dtitemdesc.Rows[0]["colorid"];
                        dr["shapeid"] = dtitemdesc.Rows[0]["shapeid"];
                        dr["Sizeid"] = dtitemdesc.Rows[0]["Sizeid"];
                        dr["Packingtypeid"] = dtitemdesc.Rows[0]["packingtypeid"];
                        dr["Ratedate"] = DateTime.FromOADate(Convert.ToDouble(wsp.Readcell("M" + rNo))).ToString("dd-MMM-yyyy");
                        if (Session["varMasterCompanyIDForERP"].ToString() == "14")
                        {
                            dr["PalletNo"] = wsp.Readcell("N" + rNo).Trim();
                        }
                        else
                        {
                            dr["PalletNo"] = "";
                        }
                        dt.Rows.Add(dr);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        FillGrid(dt);
                    }
                    else
                    {
                        if (lblmsg.Text == "")
                        {
                            lblmsg.Text = "Excel sheet has no data to import.";
                        }
                        DG.DataSource = null;
                        DG.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    lblmsg.Text = ex.Message;
                }
                finally
                {
                    document.Close();
                    document.Dispose();
                    //File.Delete(filename);
                }
            }
        }

    }
    protected void FillGrid(DataTable dt)
    {
        DG.DataSource = dt;
        DG.DataBind();
        if (DG.Rows.Count > 0)
        {
            TBSave.Visible = true;
        }
        else
        {
            TBSave.Visible = false;
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        Label lblpallet = new Label();
        //*************Table Type
        DataTable dt = new DataTable();
        dt.Columns.Add("Itemid", typeof(int));
        dt.Columns.Add("QualityId", typeof(int));
        dt.Columns.Add("designid", typeof(int));
        dt.Columns.Add("colorid", typeof(int));
        dt.Columns.Add("shapeid", typeof(int));
        dt.Columns.Add("sizeid", typeof(int));
        dt.Columns.Add("Packtypeid", typeof(int));
        dt.Columns.Add("articleno", typeof(string));
        dt.Columns.Add("Planpcs", typeof(int));
        dt.Columns.Add("Packpcs", typeof(int));
        dt.Columns.Add("WIPpcs", typeof(int));
        dt.Columns.Add("ECISNo", typeof(string));
        dt.Columns.Add("Dest", typeof(string));
        dt.Columns.Add("Podate", typeof(string));
        dt.Columns.Add("Pono", typeof(string));
        dt.Columns.Add("Dtstamp", typeof(string));
        dt.Columns.Add("Ratedate", typeof(string));
        dt.Columns.Add("ShipId", typeof(string));
        dt.Columns.Add("PalletNo", typeof(string));
        //***********
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            GridViewRow gvr = DG.Rows[i];
            //Label lblid = (Label)gvr.FindControl("lblid");
            //Label lbldetailid = (Label)gvr.FindControl("lbldetailid");
            Label lblitemid = (Label)gvr.FindControl("lblitemid");
            Label lblqualityid = (Label)gvr.FindControl("lblqualityid");
            Label lbldesignid = (Label)gvr.FindControl("lbldesignid");
            Label lblcolorid = (Label)gvr.FindControl("lblcolorid");
            Label lblshapeid = (Label)gvr.FindControl("lblshapeid");
            Label lblsizeid = (Label)gvr.FindControl("lblsizeid");
            Label lblpackingtypeid = (Label)gvr.FindControl("lblpackingtypeid");
            Label lblarticleno = (Label)gvr.FindControl("lblarticleno");
            Label lblplanpcs = (Label)gvr.FindControl("lblplanpcs");
            Label lblpackpcs = (Label)gvr.FindControl("lblpackpcs");
            Label lblwippcs = (Label)gvr.FindControl("lblwippcs");
            Label lblecisno = (Label)gvr.FindControl("lblecisno");
            Label lbldest = (Label)gvr.FindControl("lbldest");
            Label lblpodate = (Label)gvr.FindControl("lblpodate");
            Label lblpono = (Label)gvr.FindControl("lblpono");
            Label lbldtstamp = (Label)gvr.FindControl("lbldtstamp");
            Label lblratedate = (Label)gvr.FindControl("lblratedate");
            if (Session["varMasterCompanyIDForERP"].ToString() == "14")
            {
                 lblpallet = (Label)gvr.FindControl("lblpalletno");
            }
            
            //***************
            DataRow dr = dt.NewRow();
            dr["Itemid"] = lblitemid.Text;
            dr["QualityId"] = lblqualityid.Text;
            dr["designid"] = lbldesignid.Text;
            dr["colorid"] = lblcolorid.Text;
            dr["shapeid"] = lblshapeid.Text;
            dr["sizeid"] = lblsizeid.Text;
            dr["Packtypeid"] = lblpackingtypeid.Text;
            dr["articleno"] = lblarticleno.Text;
            dr["Planpcs"] = lblplanpcs.Text;
            dr["Packpcs"] = lblpackpcs.Text;
            dr["WIPpcs"] = lblwippcs.Text;
            dr["ECISNo"] = lblecisno.Text;
            dr["Dest"] = lbldest.Text;
            dr["Podate"] = lblpodate.Text;
            dr["Pono"] = lblpono.Text;
            dr["Dtstamp"] = lbldtstamp.Text;
            dr["Ratedate"] = lblratedate.Text;
            dr["ShipId"] = "";
            dr["PalletNo"] = lblpallet.Text;
            dt.Rows.Add(dr);
        }
        if (dt.Rows.Count > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {

                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@ID", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = hnplanid.Value;
                param[1] = new SqlParameter("@Batchno", txtbatchno.Text);
                param[2] = new SqlParameter("@Startdate", txtstartdate.Text);
                param[3] = new SqlParameter("@Compdate", txtcompdate.Text);
                param[4] = new SqlParameter("@userid", Session["varuserid"]);
                param[5] = new SqlParameter("@mastercomanyId", Session["varMasterCompanyIDForERP"]);
                param[6] = new SqlParameter("@dtdetail", dt);
                param[7] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[7].Direction = ParameterDirection.Output;
                //*************************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_saveDispatchPlan", param);
                lblmsg.Text = param[7].Value.ToString();
                hnplanid.Value = param[0].Value.ToString();
                Tran.Commit();
                FillGridDetail();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblmsg.Text = ex.Message;

            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "al2", "alert('No Data saved.')", true);
        }
    }
    protected void FillGridDetail()
    {
        string str = @"select vf.QualityName+' '+vf.designName as Quality,vf.ColorName as Colour,vf.SizeMtr as Size,pt.PackingType AS Packtype,
                        DPD.articleno,dpd.PlanPcs,dpd.Packpcs,dpd.WIPpcs,dpd.ECISNo,dpd.Dest,Replace(CONVERT(nvarchar(11),dpd.Podate,106),' ','-') as Podate,dpd.Pono,dpd.Dtstamp,
                        DPM.ID as Planid,DPD.DetailId as Plandetailid,dpd.Itemid,dpd.Qualityid,dpd.Designid,dpd.ColorId,dpd.Shapeid,dpd.Sizeid,dpd.Packtypeid as packingtypeid,Replace(convert(nvarchar(11),DPD.RateDate,106),' ','-') as Ratedate,isnull(DPD.palletnos,'') as PalletNo
                        From DispatchPlanmaster DPM inner join DispatchPlanDetail DPD on DPM.ID=DPD.Masterid
                        inner join V_FinishedItemDetail vf on DPD.Itemid=vf.item_id
                        and DPD.Qualityid=vf.QualityId and dpd.Designid=vf.designId and dpd.ColorId=vf.ColorId
                        and dpd.Shapeid=vf.ShapeId and dpd.Sizeid=vf.SizeId
                        inner join PackingType PT on DPD.Packtypeid=pt.ID Where DPM.Batchno='" + txtbatchno.Text + "' and DpM.id=" + hnplanid.Value + " order by Dpd.detailid";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            FillGrid(ds.Tables[0]);
        }
        else
        {
            txtstartdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtcompdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            hnplanid.Value = "0";
            DG.DataSource = null;
            DG.DataBind();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
    }
    protected void FillBatchDetail()
    {

        string str = "select id,Replace(convert(nvarchar(11),startDate,106),' ','-') as Startdate,Replace(convert(nvarchar(11),compdate,106),' ','-') as Compdate,Allocationstatus From Dispatchplanmaster Where Batchno='" + txtbatchno.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            hnplanid.Value = ds.Tables[0].Rows[0]["id"].ToString();
            txtstartdate.Text = ds.Tables[0].Rows[0]["startdate"].ToString();
            txtcompdate.Text = ds.Tables[0].Rows[0]["compdate"].ToString();
            if (ds.Tables[0].Rows[0]["allocationstatus"].ToString() == "1")
            {
                btnallocate.Enabled = false;
                btnallocate.BackColor = System.Drawing.Color.DarkGray;
                fileupload.Enabled = false;
                btnimport.Enabled = false;
            }
            else
            {
                btnallocate.Enabled = true;
                fileupload.Enabled = true;
                fileupload.Enabled = true;
            }
            btndelete.Visible = true;
        }
        else
        {
            hnplanid.Value = "0";
            btndelete.Visible = false;
        }
        FillGridDetail();
    }
    protected void btnallocate_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        Label lblpalletno = new Label();
        //*************Table Type
        DataTable dt = new DataTable();
        dt.Columns.Add("Itemid", typeof(int));
        dt.Columns.Add("QualityId", typeof(int));
        dt.Columns.Add("designid", typeof(int));
        dt.Columns.Add("colorid", typeof(int));
        dt.Columns.Add("shapeid", typeof(int));
        dt.Columns.Add("sizeid", typeof(int));
        dt.Columns.Add("Packtypeid", typeof(int));
        dt.Columns.Add("articleno", typeof(string));
        dt.Columns.Add("Planpcs", typeof(int));
        dt.Columns.Add("Packpcs", typeof(int));
        dt.Columns.Add("WIPpcs", typeof(int));
        dt.Columns.Add("ECISNo", typeof(string));
        dt.Columns.Add("Dest", typeof(string));
        dt.Columns.Add("Podate", typeof(string));
        dt.Columns.Add("Pono", typeof(string));
        dt.Columns.Add("Dtstamp", typeof(string));
        dt.Columns.Add("PlanId", typeof(int));
        dt.Columns.Add("PlanDetailId", typeof(int));
        dt.Columns.Add("PalletNo", typeof(string));
        
        //***********
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            GridViewRow gvr = DG.Rows[i];
            //Label lblid = (Label)gvr.FindControl("lblid");
            //Label lbldetailid = (Label)gvr.FindControl("lbldetailid");
            Label lblitemid = (Label)gvr.FindControl("lblitemid");
            Label lblqualityid = (Label)gvr.FindControl("lblqualityid");
            Label lbldesignid = (Label)gvr.FindControl("lbldesignid");
            Label lblcolorid = (Label)gvr.FindControl("lblcolorid");
            Label lblshapeid = (Label)gvr.FindControl("lblshapeid");
            Label lblsizeid = (Label)gvr.FindControl("lblsizeid");
            Label lblpackingtypeid = (Label)gvr.FindControl("lblpackingtypeid");
            Label lblarticleno = (Label)gvr.FindControl("lblarticleno");
            Label lblplanpcs = (Label)gvr.FindControl("lblplanpcs");
            Label lblpackpcs = (Label)gvr.FindControl("lblpackpcs");
            Label lblwippcs = (Label)gvr.FindControl("lblwippcs");
            Label lblecisno = (Label)gvr.FindControl("lblecisno");
            Label lbldest = (Label)gvr.FindControl("lbldest");
            Label lblpodate = (Label)gvr.FindControl("lblpodate");
            Label lblpono = (Label)gvr.FindControl("lblpono");
            Label lbldtstamp = (Label)gvr.FindControl("lbldtstamp");
            Label lblid = (Label)gvr.FindControl("lblid");
            Label lbldetailid = (Label)gvr.FindControl("lbldetailid");
            if (Session["varMasterCompanyIDForERP"].ToString() == "14")
            {
                 lblpalletno = (Label)gvr.FindControl("lblpalletno");
            }
            //***************
            DataRow dr = dt.NewRow();
            dr["Itemid"] = lblitemid.Text;
            dr["QualityId"] = lblqualityid.Text;
            dr["designid"] = lbldesignid.Text;
            dr["colorid"] = lblcolorid.Text;
            dr["shapeid"] = lblshapeid.Text;
            dr["sizeid"] = lblsizeid.Text;
            dr["Packtypeid"] = lblpackingtypeid.Text;
            dr["articleno"] = lblarticleno.Text;
            dr["Planpcs"] = lblplanpcs.Text;
            dr["Packpcs"] = lblpackpcs.Text;
            dr["WIPpcs"] = lblwippcs.Text;
            dr["ECISNo"] = lblecisno.Text;
            dr["Dest"] = lbldest.Text;
            dr["Podate"] = lblpodate.Text;
            dr["Pono"] = lblpono.Text;
            dr["Dtstamp"] = lbldtstamp.Text;
            dr["Planid"] = lblid.Text;
            dr["Plandetailid"] = lbldetailid.Text;
            dr["PalletNo"] = lblpalletno.Text;

            //*****************
            dt.Rows.Add(dr);
        }
        if (dt.Rows.Count > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {

                SqlCommand cmd = new SqlCommand("Pro_SaveDispatchAllocation", con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;

                cmd.Parameters.AddWithValue("@PlanID", hnplanid.Value);
                cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
                cmd.Parameters.AddWithValue("@mastercomanyId", Session["varMasterCompanyIDForERP"]);
                cmd.Parameters.AddWithValue("@dtdetail", dt);
                cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                cmd.Parameters["@msg"].Direction = ParameterDirection.Output;             


                cmd.ExecuteNonQuery();
                lblmsg.Text = cmd.Parameters["@msg"].Value.ToString();
                hnplanid.Value = cmd.Parameters["@PlanID"].Value.ToString();
                Tran.Commit();
                FillGridDetail();              

                //SqlParameter[] param = new SqlParameter[5];
                //param[0] = new SqlParameter("@PlanID", hnplanid.Value);
                //param[1] = new SqlParameter("@userid", Session["varuserid"]);
                //param[2] = new SqlParameter("@mastercomanyId", Session["varMasterCompanyIDForERP"]);
                //param[3] = new SqlParameter("@dtdetail", dt);
                //param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                //param[4].Direction = ParameterDirection.Output;
                ////**************
                //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveDispatchAllocation", param);
                //lblmsg.Text = param[4].Value.ToString();
                //hnplanid.Value = param[0].Value.ToString();
                //Tran.Commit();
                //FillGridDetail();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblmsg.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "al5", "alert('No Data available for allocation.')", true);
        }
    }
    protected void txtbatchno_TextChanged(object sender, EventArgs e)
    {
        FillBatchDetail();
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //SqlParameter[] param = new SqlParameter[4];
            //param[0] = new SqlParameter("@DispatchPlanid", hnplanid.Value);
            //param[1] = new SqlParameter("@BatchNo", txtbatchno.Text);
            //param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            //param[2].Direction = ParameterDirection.Output;
            //param[3] = new SqlParameter("@userid", Session["varuserid"]);
            ////*************
            //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteDispatchPlan", param);


            SqlCommand cmd = new SqlCommand("Pro_DeleteDispatchPlan", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;
           
            cmd.Parameters.AddWithValue("@DispatchPlanid", hnplanid.Value);
            cmd.Parameters.AddWithValue("@BatchNo", txtbatchno.Text);
            cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);


            cmd.ExecuteNonQuery();
            if (cmd.Parameters["@msg"].Value.ToString() != "")
            {
                lblmsg.Text = cmd.Parameters["@msg"].Value.ToString();
                Tran.Rollback();
            }
            else
            {
                Tran.Commit();
                lblmsg.Text = "Dispatch Plan Deleted Successfully..";
                hnplanid.Value = "0";
            }
            FillBatchDetail();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
    }
    private bool CheckDate(String date)
    {
        try
        {
            DateTime dt = DateTime.Parse(date);
            return true;
        }
        catch
        {
            return false;
        }
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Convert.ToInt16(Session["varMasterCompanyIDForERP"]) ==14)
            {
                DG.Columns[15].Visible = true;
                
            }
        }
    }
}