using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
public partial class Masters_Inspection_FrmDyedYarnInspectionNew : System.Web.UI.Page
{
    static int approvestatus = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CI.CompanyId,CI.CompanyName from CompanyInfo CI inner join Company_Authentication CA on Ci.CompanyId=CA.CompanyId
                  WHere CI.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " and CA.UserId=" + Session["varuserid"] + @"  order by CompanyName 
                    Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompanyName, ds, 0, true, "Plz Select--");

            if (DDcompanyName.Items.Count > 0)
            {
                DDcompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 1, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            txtdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            switch (Session["usertype"].ToString())
            {
                case "1":
                case "2":
                    btnApprove.Visible = true;
                    Changeapprovebuttoncolor(0);
                    break;
                default:
                    btnApprove.Visible = false;
                    break;
            }


            Fillgridinitialrow();
        }

    }
    protected void Changeapprovebuttoncolor(int approvestatus = 0)
    {
        switch (approvestatus)
        {
            case 1:
                btnApprove.BackColor = System.Drawing.Color.Green;
                break;
            default:
                btnApprove.BackColor = System.Drawing.Color.Red;
                break;
        }
    }
    protected void Fillgridinitialrow()
    {
        //*****************
        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[13] { new DataColumn("Srno"), new DataColumn("Shadeno"), new DataColumn("moisturecontent"), new DataColumn("ColorFastnessToWashing"), new DataColumn("Dry"), new DataColumn("Wet"), new DataColumn("recdqty"), new DataColumn("ShadeVariation"), new DataColumn("PresenceOfRefSample"), new DataColumn("ResultOfPH"), new DataColumn("TransportConditionandDAM"), new DataColumn("ShadeGeneralApperance"), new DataColumn("result") });
        dt.Rows.Add(1, "", "", "", "", "", "", "", "", "", "", "", "");
        dt.Rows.Add(2, "", "", "", "", "", "", "", "", "", "", "", "");
        dt.Rows.Add(3, "", "", "", "", "", "", "", "", "", "", "", "");
        dt.Rows.Add(4, "", "", "", "", "", "", "", "", "", "", "", "");

        dt.Rows.Add(5, "", "", "", "", "", "", "", "", "", "", "", "");
        dt.Rows.Add(6, "", "", "", "", "", "", "", "", "", "", "", "");
        dt.Rows.Add(7, "", "", "", "", "", "", "", "", "", "", "", "");
        dt.Rows.Add(8, "", "", "", "", "", "", "", "", "", "", "", "");

        dt.Rows.Add(9, "", "", "", "", "", "", "", "", "", "", "", "");
        dt.Rows.Add(10, "", "", "", "", "", "", "", "", "", "", "", "");
        dt.Rows.Add(11, "", "", "", "", "", "", "", "", "", "", "", "");
        dt.Rows.Add(12, "", "", "", "", "", "", "", "", "", "", "", "");

        dt.Rows.Add(13, "", "", "", "", "", "", "", "", "", "", "", "");
        dt.Rows.Add(14, "", "", "", "", "", "", "", "", "", "", "", "");
        dt.Rows.Add(15, "", "", "", "", "", "", "", "", "", "", "", "");

        dt.Rows.Add(16, "", "", "", "", "", "", "", "", "", "", "", "");
        dt.Rows.Add(17, "", "", "", "", "", "", "", "", "", "", "", "");
        dt.Rows.Add(18, "", "", "", "", "", "", "", "", "", "", "", "");
        dt.Rows.Add(19, "", "", "", "", "", "", "", "", "", "", "", "");
        dt.Rows.Add(20, "", "", "", "", "", "", "", "", "", "", "", "");
        

        DGDetail.DataSource = dt;
        DGDetail.DataBind();
        //*****************
    }
    private void fillDocno()
    {
        string str = @"SELECT Distinct RIM.DOCID,RIM.DOCNO +' # ' +Replace(convert(nvarchar(11),RIM.Reportdate,106),' ','-') as DocNo 
                FROM DYEDYARNINSPECTIONMASTER RIM 
                INNER JOIN DYEDYARNINSPECTIONDETAIL RID ON RIM.DOCID=RID.DOCID
                Where RIM.COMPANYID=" + DDcompanyName.SelectedValue + " And RIM.BranchID = " + DDBranchName.SelectedValue;
        if (txtsuppliersearch.Text != "")
        {
            str = str + " and RIM.Suppliername like '" + txtsuppliersearch.Text.Trim() + "%'";
        }
        if (txtshadesearch.Text != "")
        {
            str = str + " and RID.ShadeNo like '" + txtshadesearch.Text.Trim() + "%'";
        }
        if (txtlotnosearch.Text != "")
        {
            str = str + " and RIM.Lotno like '%" + txtlotnosearch.Text.Trim() + "%'";
        }
        if (txtTagNosearch.Text != "")
        {
            str = str + " and RIM.TagNo like '%" + txtTagNosearch.Text.Trim() + "%'";
        }

        str = str + " order by DOCID";
        UtilityModule.ConditionalComboFill(ref DDDocNo, str, true, "--Plz Select--");
    }
    protected void DDcompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkedit.Checked == true)
        {
            fillDocno();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";

        //**************DATA TABLE
        DataTable dt = new DataTable();
        dt.Columns.Add("Srno", typeof(int));
        dt.Columns.Add("Shadeno", typeof(string));
        dt.Columns.Add("Moisturecontent", typeof(string));
        dt.Columns.Add("LABTESTCOLOR", typeof(string));
        dt.Columns.Add("Dry", typeof(string));
        dt.Columns.Add("Wet", typeof(string));
        dt.Columns.Add("Recdqty", typeof(string));
        dt.Columns.Add("ShadeVariation", typeof(string));
        dt.Columns.Add("PresenceOfRefSample", typeof(string));
        dt.Columns.Add("ResultOfPH", typeof(string));
        dt.Columns.Add("TransportConditionandDAM", typeof(string));
        dt.Columns.Add("ShadeGeneralApperance", typeof(string));
        dt.Columns.Add("Result", typeof(string));

        for (int i = 0; i < DGDetail.Rows.Count; i++)
        {
            DataRow dr = dt.NewRow();
            Label lblsrno = (Label)DGDetail.Rows[i].FindControl("lblsrno");
            TextBox txtshadeno = (TextBox)DGDetail.Rows[i].FindControl("txtshadeno");
            TextBox txtmoisturecontent = (TextBox)DGDetail.Rows[i].FindControl("txtmoisturecontent");
            TextBox txtlabtestcolor = (TextBox)DGDetail.Rows[i].FindControl("txtlabtestcolor");
            TextBox txtdry = (TextBox)DGDetail.Rows[i].FindControl("txtdry");
            TextBox txtwet = (TextBox)DGDetail.Rows[i].FindControl("txtwet");
            TextBox txtrecdqty = (TextBox)DGDetail.Rows[i].FindControl("txtrecdqty");
            TextBox txtShadeVariation = (TextBox)DGDetail.Rows[i].FindControl("txtShadeVariation");
            TextBox txtPresenceOfRefSample = (TextBox)DGDetail.Rows[i].FindControl("txtPresenceOfRefSample");
            TextBox txtResultOfPH = (TextBox)DGDetail.Rows[i].FindControl("txtResultOfPH");
            TextBox txtTransportConditionandDAM = (TextBox)DGDetail.Rows[i].FindControl("txtTransportConditionandDAM");
            TextBox txtShadeGeneralApperance = (TextBox)DGDetail.Rows[i].FindControl("txtShadeGeneralApperance");
            DropDownList ddresult = (DropDownList)DGDetail.Rows[i].FindControl("ddresult");

            if (txtshadeno.Text != "")
            {
                dr["srno"] = lblsrno.Text;
                dr["shadeno"] = txtshadeno.Text;
                dr["Moisturecontent"] = txtmoisturecontent.Text;
                dr["LABTESTCOLOR"] = txtlabtestcolor.Text;
                dr["dry"] = txtdry.Text;
                dr["wet"] = txtwet.Text;
                dr["recdqty"] = txtrecdqty.Text;
                dr["ShadeVariation"] = txtShadeVariation.Text;
                dr["PresenceOfRefSample"] = txtPresenceOfRefSample.Text;
                dr["ResultOfPH"] = txtResultOfPH.Text;
                dr["TransportConditionandDAM"] = txtTransportConditionandDAM.Text;
                dr["ShadeGeneralApperance"] = txtShadeGeneralApperance.Text;
                dr["Result"] = ddresult.SelectedItem.Text;

                dt.Rows.Add(dr);
            }

        }
        //**************
        if (dt.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('Please enter some data in Grid.')", true);
            return;
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[21];
            param[0] = new SqlParameter("@Docid", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hndocid.Value;
            param[1] = new SqlParameter("@Companyid", DDcompanyName.SelectedValue);
            param[2] = new SqlParameter("@DocNo", SqlDbType.VarChar, 50);
            param[2].Value = txtdocno.Text;
            param[2].Direction = ParameterDirection.InputOutput;
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@ReportDate", txtdate.Text);
            param[6] = new SqlParameter("@Suppliername", txtsuppliername.Text.Trim());
            param[7] = new SqlParameter("@ChallanNodate", txtchallannodate.Text.Trim());
            param[8] = new SqlParameter("@TotalBale", txttotalbale.Text == "" ? "0" : txttotalbale.Text);
            param[9] = new SqlParameter("@SampleSize", txtsamplesize.Text == "" ? "0" : txtsamplesize.Text);
            param[10] = new SqlParameter("@NoofHank", txtnoofhank.Text == "" ? "0" : txtnoofhank.Text);
            param[11] = new SqlParameter("@dt", dt);
            param[12] = new SqlParameter("@LotNo", txtlotno.Text.Trim());
            param[13] = new SqlParameter("@comments", txtcomments.Text.Trim());
            param[14] = new SqlParameter("@TagNo", TxtTagNo.Text.Trim());
            param[15] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);

            param[16] = new SqlParameter("@TotalNotOkQty", txtTotalNotOkQty.Text=="" ?"0": txtTotalNotOkQty.Text);
            param[17] = new SqlParameter("@YarnType", txtYarnType.Text);
            param[18] = new SqlParameter("@InwardsNo", txtInwardsNo.Text);
            param[19] = new SqlParameter("@AcceptedArea", txtAcceptedArea.Text);
            param[20] = new SqlParameter("@RejectedArea", txtRejectedArea.Text);
          

            //*********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEDYEDYARNINSPECTION", param);
            lblmsg.Text = param[4].Value.ToString();
            txtdocno.Text = param[2].Value.ToString();
            hndocid.Value = param[0].Value.ToString();

            
            // at the time of update delete all the data in tables           

            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + param[4].Value.ToString() + "')", true);

            Tran.Commit();
            SaveImage(Convert.ToInt32(hndocid.Value));
            FillDataback();


            //**********

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Docid", hndocid.Value);

            //            string str = @"SELECT Ci.companyName,* FROM RAWYARNINSPECTIONMASTER RIM INNER JOIN RAWYARNINSPECTIONDETAIL RID ON RIM.DOCID=RID.DOCID 
            //                           inner join CompanyInfo ci on RIM.COMPANYID=ci.CompanyId Where RIM.Docid=" + hndocid.Value;

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETDYEDYARNINSPECTIONREPORT", param);

            if (ds.Tables[0].Rows.Count > 0)
            {

                Session["rptFileName"] = "~\\Reports\\RptDyedYarnInspectionNew.rpt";
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptDyedYarnInspectionNew.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt1", "alert('No records found.')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
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
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@Docid", hndocid.Value);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEDYEDYARNINSPECTION", param);
            Tran.Commit();
            if (param[3].Value.ToString() != "")
            {
                lblmsg.Text = param[3].Value.ToString();
            }
            else
            {
                lblmsg.Text = "DOC No. Deleted Successfully.";
                fillDocno();
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        TDSupplierSearch.Visible = false;
        TDDocno.Visible = false;
        TDshadeno.Visible = false;
        TDLotno.Visible = false;
        TDTagNo.Visible = false;
        hndocid.Value = "0";
        DDDocNo.Items.Clear();
        refreshcontrol();
        if (chkedit.Checked == true)
        {
            TDDocno.Visible = true;
            TDSupplierSearch.Visible = true;
            TDshadeno.Visible = true;
            TDLotno.Visible = true;
            TDTagNo.Visible = true;
            fillDocno();
        }
    }
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        fillDocno();
    }

    protected void DDDocNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hndocid.Value = DDDocNo.SelectedValue;
        refreshcontrol();
        FillDataback();
    }

    private void FillDataback()
    {
        string str = @"SELECT DM.DOCID,DM.DOCNO,DM.SUPPLIERNAME,DM.CHALLANNODATE,REPLACE(CONVERT(NVARCHAR(11),DM.REPORTDATE,106),' ','-') AS REPORTDATE,DM.LOTNO,DM.TOTALBALE,DM.SAMPLESIZE,DM.NOOFHANK,
                        DD.SRNO,DD.SHADENO,DD.MOISTURECONTENT,DD.LABTESTCOLOR,DD.DRY,DD.WET,DD.RECDQTY,DD.RESULT,DM.Comments,DM.approvestatus,DM.TagNo,DM.TotalNotOkQty,DM.YarnType,DM.InwardsNo,
                        isnull(DM.ImagePath,'') as ImagePath,DM.AcceptedArea,DM.RejectedArea,DD.ShadeVariation,DD.PRESENCEOFREFSAMPLE,DD.RESULTOFPH,DD.TransportCondition,DD.SHADEGENERALAPPERANCE
                        FROM DYEDYARNINSPECTIONMASTER DM INNER JOIN DYEDYARNINSPECTIONDETAIL DD ON DM.DOCID=DD.DOCID Where DM.DOCID=" + hndocid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtdocno.Text = ds.Tables[0].Rows[0]["DocNo"].ToString();
            txtsuppliername.Text = ds.Tables[0].Rows[0]["Suppliername"].ToString();
            txtdate.Text = ds.Tables[0].Rows[0]["ReportDate"].ToString();
            txtchallannodate.Text = ds.Tables[0].Rows[0]["ChallanNoDate"].ToString();
            txtlotno.Text = ds.Tables[0].Rows[0]["Lotno"].ToString();
            txttotalbale.Text = ds.Tables[0].Rows[0]["totalbale"].ToString();
            txtsamplesize.Text = ds.Tables[0].Rows[0]["Samplesize"].ToString();
            txtnoofhank.Text = ds.Tables[0].Rows[0]["NoofHank"].ToString();
            txtcomments.Text = ds.Tables[0].Rows[0]["comments"].ToString();
            TxtTagNo.Text = ds.Tables[0].Rows[0]["TagNo"].ToString();

            Changeapprovebuttoncolor(Convert.ToInt16(ds.Tables[0].Rows[0]["approvestatus"]));
            EditRights_Button(Convert.ToInt16(Session["usertype"]), Convert.ToInt16(ds.Tables[0].Rows[0]["approvestatus"]));
            approvestatus = Convert.ToInt16(ds.Tables[0].Rows[0]["approvestatus"]);
            //1

            txtTotalNotOkQty.Text = ds.Tables[0].Rows[0]["TotalNotOkQty"].ToString();
            txtYarnType.Text = ds.Tables[0].Rows[0]["YarnType"].ToString();
            txtInwardsNo.Text = ds.Tables[0].Rows[0]["InwardsNo"].ToString();
            txtAcceptedArea.Text = ds.Tables[0].Rows[0]["AcceptedArea"].ToString();
            txtRejectedArea.Text = ds.Tables[0].Rows[0]["RejectedArea"].ToString();

            if (ds.Tables[0].Rows[0]["ImagePath"].ToString() != "")
            {
                lblphotoimage.ImageUrl = ds.Tables[0].Rows[0]["ImagePath"].ToString() + "?" + DateTime.Now.Ticks.ToString();
            }
            else
            {
                lblphotoimage.ImageUrl = null;
            }
            
        }
        DGSavedetails.DataSource = ds.Tables[0];
        DGSavedetails.DataBind();
    }
    protected void EditRights_Button(int usertype, int approvestatus = 0)
    {
        switch (usertype)
        {
            case 1:
            case 2:
                btnsave.Visible = true;
                btndelete.Visible = true;
                break;
            default:
                if (approvestatus == 1)
                {
                    btnsave.Visible = false;
                    btndelete.Visible = false;
                }
                else
                {
                    btnsave.Visible = true;
                    btndelete.Visible = true;
                }
                break;
        }
    }

    private void refreshcontrol()
    {
        txtsuppliername.Text = "";
        txtdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        txtchallannodate.Text = "";
        txtlotno.Text = "";
        TxtTagNo.Text = "";
        txttotalbale.Text = "";
        txtsamplesize.Text = "";
        txtnoofhank.Text = "";
        txtcomments.Text = "";
        Fillgridinitialrow();
    }
    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGDetail, "Select$" + e.Row.RowIndex);
        }
        //GridViewRow gvRow = e.Row;
        //if (gvRow.RowType == DataControlRowType.Header)
        //{
        //    GridViewRow gvrow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
        //    TableCell cell0 = new TableCell();
        //    cell0.Text = "";
        //    cell0.HorizontalAlign = HorizontalAlign.Center;


        //    TableCell cell1 = new TableCell();
        //    cell1.Text = "";
        //    cell1.HorizontalAlign = HorizontalAlign.Center;

        //    TableCell cell2 = new TableCell();
        //    cell2.Text = "";
        //    cell2.HorizontalAlign = HorizontalAlign.Center;

        //    TableCell cell3 = new TableCell();
        //    cell3.Text = "";
        //    cell3.HorizontalAlign = HorizontalAlign.Center;

        //    TableCell cell4 = new TableCell();
        //    cell4.Text = "Lab Test Rubbing Fastness";
        //    cell4.ColumnSpan = 2;
        //    cell4.HorizontalAlign = HorizontalAlign.Center;

        //    TableCell cell5 = new TableCell();
        //    cell5.Text = "";
        //    cell5.HorizontalAlign = HorizontalAlign.Center;

        //    TableCell cell6 = new TableCell();
        //    cell6.Text = "";
        //    cell6.HorizontalAlign = HorizontalAlign.Center;

        //    gvrow.Cells.Add(cell0);
        //    gvrow.Cells.Add(cell1);
        //    gvrow.Cells.Add(cell2);
        //    gvrow.Cells.Add(cell3);
        //    gvrow.Cells.Add(cell4);
        //    gvrow.Cells.Add(cell5);
        //    gvrow.Cells.Add(cell6);

        //    DGDetail.Controls[0].Controls.AddAt(0, gvrow);
        //}

    }
    protected void btnApprove_Click(object sender, EventArgs e)
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
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@Docid", hndocid.Value);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_APPROVEDYEDYARNINSPECTION", param);
            Tran.Commit();
            lblmsg.Text = param[3].Value.ToString();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altappv", "alert('" + param[3].Value.ToString() + "')", true);
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
    protected void lnkdelClick(object sender, EventArgs e)
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
            LinkButton lnkdel = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)lnkdel.NamingContainer;

            Label lbldocid = (Label)gvr.FindControl("lbldocid");
            Label lblsrno = (Label)gvr.FindControl("lblsrno");
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@docid", lbldocid.Text);
            param[1] = new SqlParameter("@srno", lblsrno.Text);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@MastercompanyId", Session["varcompanyNo"]);
            param[4] = new SqlParameter("@Userid", Session["varuserid"]);
            //********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEDYEDYARNINSPECTION", param);
            lblmsg.Text = param[2].Value.ToString();
            Tran.Commit();
            DGSavedetails.EditIndex = -1;
            FillDataback();

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
    protected void DGSavedetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGSavedetails.EditIndex = e.NewEditIndex;
        FillDataback();
    }
    protected void DGSavedetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGSavedetails.EditIndex = -1;
        FillDataback();
    }
    protected void DGSavedetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lbldocid = (Label)DGSavedetails.Rows[e.RowIndex].FindControl("lbldocid");
            Label lblsrno = (Label)DGSavedetails.Rows[e.RowIndex].FindControl("lblsrno");

            TextBox txtshadeno = (TextBox)DGSavedetails.Rows[e.RowIndex].FindControl("txtshadeno");
            TextBox txtmoisturecontent = (TextBox)DGSavedetails.Rows[e.RowIndex].FindControl("txtmoisturecontent");
            TextBox txtlabtestcolor = (TextBox)DGSavedetails.Rows[e.RowIndex].FindControl("txtlabtestcolor");
            TextBox txtdry = (TextBox)DGSavedetails.Rows[e.RowIndex].FindControl("txtdry");
            TextBox txtwet = (TextBox)DGSavedetails.Rows[e.RowIndex].FindControl("txtwet");
            TextBox txtrecdqty = (TextBox)DGSavedetails.Rows[e.RowIndex].FindControl("txtrecdqty");

            TextBox txtShadeVariation = (TextBox)DGSavedetails.Rows[e.RowIndex].FindControl("txtShadeVariation");
            TextBox txtPresenceOfRefSample = (TextBox)DGSavedetails.Rows[e.RowIndex].FindControl("txtPresenceOfRefSample");
            TextBox txtResultOfPH = (TextBox)DGSavedetails.Rows[e.RowIndex].FindControl("txtResultOfPH");
            TextBox txtTransportConditionandDAM = (TextBox)DGSavedetails.Rows[e.RowIndex].FindControl("txtTransportConditionandDAM");
            TextBox txtShadeGeneralApperance = (TextBox)DGSavedetails.Rows[e.RowIndex].FindControl("txtShadeGeneralApperance");

            DropDownList ddresult = (DropDownList)DGSavedetails.Rows[e.RowIndex].FindControl("ddresult");



            SqlParameter[] param = new SqlParameter[18];
            param[0] = new SqlParameter("@docid", lbldocid.Text);
            param[1] = new SqlParameter("@srno", lblsrno.Text);
            param[2] = new SqlParameter("@shadeno", txtshadeno.Text.Trim());
            param[3] = new SqlParameter("@moisturecontent", txtmoisturecontent.Text.Trim());
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@Userid", Session["varuserid"]);
            param[6] = new SqlParameter("@labtestcolor", txtlabtestcolor.Text.Trim());
            param[7] = new SqlParameter("@dry", txtdry.Text.Trim());
            param[8] = new SqlParameter("@wet", txtwet.Text.Trim());
            param[9] = new SqlParameter("@recdqty", txtrecdqty.Text.Trim());
            param[10] = new SqlParameter("@result", ddresult.SelectedItem.Text);
            param[11] = new SqlParameter("@comments", txtcomments.Text);
            param[12] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);

            param[13] = new SqlParameter("@ShadeVariation", txtShadeVariation.Text);
            param[14] = new SqlParameter("@PresenceOfRefSample", txtPresenceOfRefSample.Text);
            param[15] = new SqlParameter("@ResultOfPH", txtResultOfPH.Text);
            param[16] = new SqlParameter("@TransportConditionandDAM", txtTransportConditionandDAM.Text);
            param[17] = new SqlParameter("@ShadeGeneralApperance", txtShadeGeneralApperance.Text);           

            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEDYEDYARNINSPECTION", param);
            //*************
            lblmsg.Text = param[4].Value.ToString();
            Tran.Commit();
            DGSavedetails.EditIndex = -1;
            FillDataback();

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void DGSavedetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (approvestatus == 1)
            {
                switch (Session["usertype"].ToString())
                {
                    case "1":
                    case "2":
                        break;
                    default:
                        DGSavedetails.Columns[0].Visible = false;
                        DGSavedetails.Columns[1].Visible = false;
                        break;
                }
            }

            DropDownList ddresult = (DropDownList)e.Row.FindControl("ddresult");
            if (ddresult != null)
            {
                Label lblresult = (Label)e.Row.FindControl("lblresult");
                ddresult.SelectedValue = lblresult.Text;
            }
        }
    }

    protected void SaveImage(int hndocid)
    {        

        if (fileuploadphoto.PostedFile.FileName != "")
        {
            string filename = Path.GetFileName(fileuploadphoto.PostedFile.FileName);
            string Folderpath = Server.MapPath("../../Item_Image");
            //Check folder
            if (!Directory.Exists(Folderpath))
            {
                Directory.CreateDirectory(Folderpath);
            }
            //
            string targetPath = Server.MapPath("../../Item_Image/" + hndocid + "_photo.gif");

            FileInfo file = new FileInfo(targetPath);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
            }

            string img = "~\\Item_Image\\" + hndocid + "_photo.gif";
            //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
            Stream strm = fileuploadphoto.PostedFile.InputStream;
            var targetFile = targetPath;
            if (fileuploadphoto.FileName != null && fileuploadphoto.FileName != "")
            {
                GenerateThumbnails(0.4, strm, targetFile);
            }
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update DyedyarnInspectionMaster Set ImagePath='" + img + "' Where Docid=" + hndocid + "");
            lblphotoimage.ImageUrl = img + "?" + DateTime.Now.Ticks.ToString(); ;
        }
    }
    private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
    {
        using (var image = System.Drawing.Image.FromStream(sourcePath))
        {
            var newWidth = (int)(image.Width * scaleFactor);
            var newHeight = (int)(image.Height * scaleFactor);
            var thumbnailImg = new Bitmap(newWidth, newHeight);
            var thumbGraph = Graphics.FromImage(thumbnailImg);
            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbGraph.DrawImage(image, imageRectangle);
            thumbnailImg.Save(targetPath, image.RawFormat);
        }
    }
}