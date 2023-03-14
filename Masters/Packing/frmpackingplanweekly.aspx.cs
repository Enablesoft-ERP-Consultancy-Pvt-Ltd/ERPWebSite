using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Packing_frmpackingplanweekly : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select ICm.CATEGORY_ID,ICM.CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=Cs.Categoryid and cs.id=0 order by CATEGORY_NAME
                           select ID,PackingType From Packingtype order by PackingType";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcategory, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDPacktype, ds, 1, true, "--Plz Select--");
            if (DDcategory.Items.Count > 0)
            {
                DDcategory.SelectedIndex = 1;
                DDcategory_SelectedIndexChanged(DDcategory, new EventArgs());
            }
            txtstartdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtcompdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (Session["usertype"].ToString() == "1")
            {
                TDclosebatchno.Visible = true;
            }

            if (Session["VarCompanyNo"].ToString() == "22")
            {
                FillArticleno();
            }
        }
    }
    protected void DDarticleno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["varCompanyNo"].ToString() == "22")
        {
            FillData();
        }
    }
    protected void DDitemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillquality();
    }
    protected void Fillquality()
    {
        UtilityModule.ConditionalComboFill(ref DDquality, "select QualityId,QualityName From Quality Where Item_Id=" + DDitemname.SelectedValue + "  order by QualityName", true, "--Plz Select--");
    }
    protected void DDquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDesign();
    }
    protected void FillDesign()
    {
        string str = @"select Distinct Vf.designid,vf.designname From V_FinishedItemDetail vf Where vf.ITEM_ID=" + DDitemname.SelectedValue + " and vf.QualityId=" + DDquality.SelectedValue + @"  and vf.designId<>0
                   order by vf.designName";
        UtilityModule.ConditionalComboFill(ref DDdesign, str, true, "--Plz Select--");
    }
    protected void DDdesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillColor();
    }
    protected void FillColor()
    {
        string str = @"select Distinct Vf.colorid,vf.colorname From V_FinishedItemDetail vf Where vf.ITEM_ID=" + DDitemname.SelectedValue + " and vf.QualityId=" + DDquality.SelectedValue + "  and vf.designId=" + DDdesign.SelectedValue + @"
                   order by vf.colorname";
        UtilityModule.ConditionalComboFill(ref DDcolor, str, true, "--Plz Select--");

    }
    protected void DDshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void FillSize()
    {
        string str = @"select Distinct Vf.SizeId,vf.SizeMtr From V_FinishedItemDetail vf Where vf.ITEM_ID=" + DDitemname.SelectedValue + " and vf.QualityId=" + DDquality.SelectedValue + "  and vf.designId=" + DDdesign.SelectedValue + @"
                       and vf.colorid=" + DDcolor.SelectedValue + " and vf.shapeid=" + DDshape.SelectedValue + @" and vf.sizeid<>0
                       order by vf.SizeMtr";
        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--Plz Select--");
    }
    protected void DDcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillShape();
    }
    protected void FillShape()
    {
        string str = @"select Distinct Vf.shapeid,vf.shapename From V_FinishedItemDetail vf Where vf.ITEM_ID=" + DDitemname.SelectedValue + " and vf.QualityId=" + DDquality.SelectedValue + "  and vf.designId=" + DDdesign.SelectedValue + " and vf.colorid=" + DDcolor.SelectedValue + @"
                   order by vf.shapename";
        UtilityModule.ConditionalComboFill(ref DDshape, str, true, "--Plz Select--");
    }
    protected void FillArticleno()
    {
        string str = "";
        if (Session["VarCompanyNo"].ToString() == "22")
        {
            str = @"select  ArticleNo,ArticleNo as Articleno1 From Packingarticle where 1=1 ";
            if (DDPacktype.SelectedIndex > 0)
            {
                str = str + " and PackingTypeid=" + DDPacktype.SelectedValue + "";
            }
            str = str + " order by ArticleNo";           

            UtilityModule.ConditionalComboFill(ref DDarticleno, str, true, "--Plz Select--");
        }
        else
        {
            str = @"select  ArticleNo,ArticleNo as Articleno1 From Packingarticle where Itemid=" + DDitemname.SelectedValue + " and QualityId=" + DDquality.SelectedValue + " and Designid=" + DDdesign.SelectedValue + " and Colorid=" + DDcolor.SelectedValue + @"
                       and shapeid=" + DDshape.SelectedValue + " and sizeid=" + DDSize.SelectedValue + " and PackingTypeid=" + DDPacktype.SelectedValue + " order by ArticleNo";

            UtilityModule.ConditionalComboFill(ref DDarticleno, str, true, "--Plz Select--");
        }

       
    }
    protected void DDSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillArticleno();
    }
    protected void btnaddrow_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[23];
            param[0] = new SqlParameter("@ID", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hnplanid.Value;
            param[1] = new SqlParameter("@Batchno", txtbatchno.Text);
            param[2] = new SqlParameter("@startDate", txtstartdate.Text);
            param[3] = new SqlParameter("@CompDate", txtcompdate.Text);
            param[4] = new SqlParameter("@userid", Session["varuserid"]);
            param[5] = new SqlParameter("@mastercompanyid", Session["varcompanyId"]);
            param[6] = new SqlParameter("@Detailid", SqlDbType.Int);
            param[6].Value = 0;
            param[7] = new SqlParameter("@itemid", DDitemname.SelectedValue);
            param[8] = new SqlParameter("@Qualityid", DDquality.SelectedValue);
            param[9] = new SqlParameter("@DesignId", DDdesign.SelectedValue);
            param[10] = new SqlParameter("@colorid", DDcolor.SelectedValue);
            param[11] = new SqlParameter("@shapeid", DDshape.SelectedValue);
            param[12] = new SqlParameter("@Sizeid", DDSize.SelectedValue);
            param[13] = new SqlParameter("@Packtypeid", DDPacktype.SelectedValue);
            param[14] = new SqlParameter("@Articleno", DDarticleno.SelectedItem.Text);
            param[15] = new SqlParameter("@Planpcs", txtplanpcs.Text == "" ? "0" : txtplanpcs.Text);
            param[16] = new SqlParameter("@Packpcs", txtpackpcs.Text == "" ? "0" : txtpackpcs.Text);
            param[17] = new SqlParameter("@WipPcs", txtWippcs.Text == "" ? "0" : txtWippcs.Text);
            param[18] = new SqlParameter("@ESICNo", txtEsicno.Text);
            param[19] = new SqlParameter("@DEST", txtdest.Text);
            param[20] = new SqlParameter("@ShipDate", txtshipdate.Text == "" ? DBNull.Value : (object)txtshipdate.Text);
            param[21] = new SqlParameter("@PONo", txtpono.Text);
            param[22] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[22].Direction = ParameterDirection.Output;
            //*************************************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SavepackingPlan", param);
            Tran.Commit();
            lblmsg.Text = param[22].Value.ToString();
            hnplanid.Value = param[0].Value.ToString();
            Fillgrid();
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
    protected void Fillgrid()
    {
        string str = @"select PM.id,PD.Detailid,PD.itemid,PD.qualityid,PD.designid,PD.colorid,pd.shapeid,pd.sizeid,Q.QualityName+' '+D.designName as Quality,C.ColorName,S.SizeMtr as Size,PT.PackingType,
                        Pd.Articleno,pd.Planpcs,pd.Packpcs,pd.Wippcs,pd.ESICNo,pd.dest,Replace(convert(nvarchar(11),pd.shipdate,106),' ','-') as Shipdate,pd.PoNo,pD.packtypeid
                        From PackingPlanMaster PM inner join PackingPlanDetail PD on PM.ID=PD.Masterid
                        inner join Quality Q on PD.Qualityid=Q.QualityId
                        inner join Design D on PD.DesignId=D.designId
                        inner join color C on PD.ColorId=C.ColorId
                        inner join Size s on PD.Sizeid=s.SizeId
                        inner join PackingType PT on PD.Packtypeid=PT.ID Where PM.Id=" + hnplanid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {

    }
    protected void FillBatchDetails()
    {
        lblmsg.Text = "";
        string str = "select id,Replace(convert(nvarchar(11),startDate,106),' ','-') as Startdate,Replace(convert(nvarchar(11),compdate,106),' ','-') as Compdate,status From packingplanmaster Where Batchno='" + txtbatchno.Text + "'";
        if (Chkclosebatchno.Checked == true)
        {
            str = str + " and  status='CLOSE'";
        }
        else
        {
            str = str + " and  status='OPEN'";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            hnplanid.Value = ds.Tables[0].Rows[0]["id"].ToString();
            txtstartdate.Text = ds.Tables[0].Rows[0]["startdate"].ToString();
            txtcompdate.Text = ds.Tables[0].Rows[0]["compdate"].ToString();
            btndelete.Visible = true;
            if (string.Equals("OPEN", (string)ds.Tables[0].Rows[0]["status"], StringComparison.OrdinalIgnoreCase))
            {
                btncompletebatchno.Text = "CLOSE BATCH NO";
            }
            else
            {
                btncompletebatchno.Text = "OPEN BATCH NO";
            }
        }
        else
        {
            btncompletebatchno.Text = "CLOSE BATCH NO";
            hnplanid.Value = "0";
            btndelete.Visible = false;
            lblmsg.Text = "Batch does not exist or Batch is closed.";
        }
        Fillgrid();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //*****************
        DataTable dtpack = new DataTable();
        dtpack.Columns.Add("Stockno", typeof(int));
        dtpack.Columns.Add("PlanId", typeof(int));
        dtpack.Columns.Add("PlanDetailid", typeof(int));
        dtpack.Columns.Add("Packdate", typeof(DateTime));
        dtpack.Columns.Add("Datestamp", typeof(string));
        //*****************
        for (int i = 0; i < DGStockDetail.Rows.Count; i++)
        {
            Label lblstockno = (Label)DGStockDetail.Rows[i].FindControl("lblstockno");
            Label lblpackid = (Label)DGStockDetail.Rows[i].FindControl("lblpackid");
            Label lblpackdetailid = (Label)DGStockDetail.Rows[i].FindControl("lblpackdetailid");
            CheckBox Chkboxitem = (CheckBox)DGStockDetail.Rows[i].FindControl("Chkboxitem");
            Label lbldatestamp = (Label)DGStockDetail.Rows[i].FindControl("lbldatestamp");
            if (Chkboxitem.Checked == true)
            {
                DataRow dr = dtpack.NewRow();
                dr["Stockno"] = lblstockno.Text;
                dr["Planid"] = lblpackid.Text;
                dr["PlanDetailId"] = lblpackdetailid.Text;
                dr["packdate"] = txtpackdate.Text;
                dr["datestamp"] = lbldatestamp.Text;
                dtpack.Rows.Add(dr);
            }
        }
        //*****************
        if (dtpack.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@dtpack", dtpack);
                param[1] = new SqlParameter("@userid", Session["varuserid"]);
                param[2] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
                //***********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SavePackplan_Stockno", param);
                Tran.Commit();
                lblmsg.Text = "Carpet No. saved successfully.";
                txtdatestamp.Text = "";
                txtpackdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
                Fillgrid();
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
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "packalt", "alert('Please select atleast one check box to save Detail.')", true);
        }

    }
    protected void lblpackcarpet_Click(object sender, EventArgs e)
    {

        txtdate_stamp.Text = "";
        txtpackpcs_chk.Text = "";

        LinkButton btn = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        Label lblitemid = (Label)gvr.FindControl("lblitemid");
        Label lblqualityid = (Label)gvr.FindControl("lblqualityid");
        Label lbldesignid = (Label)gvr.FindControl("lbldesignid");
        Label lblcolorid = (Label)gvr.FindControl("lblcolorid");
        Label lblshapeid = (Label)gvr.FindControl("lblshapeid");
        Label lblsizeid = (Label)gvr.FindControl("lblsizeid");
        Label lblid = (Label)gvr.FindControl("lblid");
        Label lbldetailid = (Label)gvr.FindControl("lbldetailid");
        Label lblpackingtypeid = (Label)gvr.FindControl("lblpackingtypeid");
        Label lblarticleno = (Label)gvr.FindControl("lblarticleno");



        SqlParameter[] param = new SqlParameter[11];
        param[0] = new SqlParameter("@itemId", lblitemid.Text);
        param[1] = new SqlParameter("@QualityId", lblqualityid.Text);
        param[2] = new SqlParameter("@Designid", lbldesignid.Text);
        param[3] = new SqlParameter("@Colorid", lblcolorid.Text);
        param[4] = new SqlParameter("@shapeid", lblshapeid.Text);
        param[5] = new SqlParameter("@sizeid", lblsizeid.Text);
        param[6] = new SqlParameter("@ID", lblid.Text);
        param[7] = new SqlParameter("@DetailID", lbldetailid.Text);
        param[8] = new SqlParameter("@packingtypeid", lblpackingtypeid.Text);
        param[9] = new SqlParameter("@articleno", lblarticleno.Text);
        param[10] = new SqlParameter("@Date_stamp", txtdate_stamp.Text);
        //*****************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getPackcarpetForPackingPlan", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ModalPopupExtenderpackcarpet.Show();
            txtpackdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "Date_stamp asc";
            DGStockDetail.DataSource = dv.ToTable();
            DGStockDetail.DataBind();
            txttotalpackpcs.Text = ds.Tables[0].Compute("Count(stockno)", "").ToString();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "al", "alert('No Stock is available to Pack')", true);
        }
    }
    protected void lblunpackcarpet_Click(object sender, EventArgs e)
    {
        DGPackingsummary.DataSource = null;
        DGPackingsummary.DataBind();
        DGUnpackcarpet.DataSource = null;
        DGUnpackcarpet.DataBind();

        LinkButton lnk = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)lnk.NamingContainer;
        Label lblid = (Label)gvr.FindControl("lblid");
        Label lbldetailid = (Label)gvr.FindControl("lbldetailid");
        //************
        string str = @"select Distinct Packno,Replace(CONVERT(nvarchar(11),Packdate,106),' ','-') as PackDate,Datestamp,Planid,Plandetailid
                       From PackingplanStockno where Planid=" + lblid.Text + " and PlanDetailid=" + lbldetailid.Text + " order by Packdate ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGPackingsummary.DataSource = ds.Tables[0];
        DGPackingsummary.DataBind();
        ModalPopupExtenderunpackcarpet.Show();
        //************
    }
    protected void lnkdelete_Click(object sender, EventArgs e)
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
            LinkButton lnk = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)lnk.NamingContainer;
            Label lblid = (Label)gvr.FindControl("lblid");
            Label lbldetailid = (Label)gvr.FindControl("lbldetailid");

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@ID", lblid.Text);
            param[1] = new SqlParameter("@DetailID", lbldetailid.Text);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            param[4] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            //*********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_Deletepackingplansinglerow", param);
            lblmsg.Text = param[2].Value.ToString();
            Tran.Commit();
            Fillgrid();
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
    protected void DGPackingsummary_SelectedIndexChanged(object sender, EventArgs e)
    {
        ModalPopupExtenderunpackcarpet.Show();
        //get unpack carpet Details
        GridViewRow gvr = DGPackingsummary.SelectedRow;
        Label lblpackno = (Label)gvr.FindControl("lblpackno");
        Label lblpackstockplanid = (Label)gvr.FindControl("lblpackstockplanid");
        Label lblpackstockPlandetailid = (Label)gvr.FindControl("lblpackstockPlandetailid");
        Label lbldtstamp = (Label)gvr.FindControl("lbldtstamp");
        Label lbldate = (Label)gvr.FindControl("lbldate");
        //***
        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@packno", lblpackno.Text);
        param[1] = new SqlParameter("@planid", lblpackstockplanid.Text);
        param[2] = new SqlParameter("@Plandetailid", lblpackstockPlandetailid.Text);
        param[3] = new SqlParameter("@Date_stamp", lbldtstamp.Text);
        param[4] = new SqlParameter("@PackDate", lbldate.Text);
        //
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getunpackcarpetforpackingplan", param);
        DGUnpackcarpet.DataSource = ds.Tables[0];
        DGUnpackcarpet.DataBind();
        txttotalunpackpcs.Text = ds.Tables[0].Compute("Count(stockno)", "").ToString();
    }
    protected void DGUnpackcarpet_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblpackstatus = (Label)e.Row.FindControl("lblpackstatus");
            if (lblpackstatus.Text == "1") //Packed
            {
                CheckBox Chkboxitemunpack = (CheckBox)e.Row.FindControl("Chkboxitemunpack");
                Chkboxitemunpack.Enabled = false;
                e.Row.BackColor = System.Drawing.Color.Red;
            }
        }
    }
    protected void btnunpackcarpet_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //*****************
        DataTable dtunpack = new DataTable();
        dtunpack.Columns.Add("Stockno", typeof(int));
        dtunpack.Columns.Add("PlanId", typeof(int));
        dtunpack.Columns.Add("PlanDetailid", typeof(int));
        dtunpack.Columns.Add("Packno", typeof(int));
        //*****************
        for (int i = 0; i < DGUnpackcarpet.Rows.Count; i++)
        {
            Label lblstockno = (Label)DGUnpackcarpet.Rows[i].FindControl("lblstockno");
            Label lblplanidunpack = (Label)DGUnpackcarpet.Rows[i].FindControl("lblplanidunpack");
            Label lblplandetailidunpack = (Label)DGUnpackcarpet.Rows[i].FindControl("lblplandetailidunpack");
            Label lblpacknounpack = (Label)DGUnpackcarpet.Rows[i].FindControl("lblpacknounpack");

            CheckBox Chkboxitemunpack = (CheckBox)DGUnpackcarpet.Rows[i].FindControl("Chkboxitemunpack");

            if (Chkboxitemunpack.Checked == true)
            {
                DataRow dr = dtunpack.NewRow();
                dr["Stockno"] = lblstockno.Text;
                dr["Planid"] = lblplanidunpack.Text;
                dr["PlanDetailId"] = lblplandetailidunpack.Text;
                dr["Packno"] = lblpacknounpack.Text;
                dtunpack.Rows.Add(dr);
            }
        }
        //*****************
        if (dtunpack.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@dtunpack", dtunpack);
                param[1] = new SqlParameter("@userid", Session["varuserid"]);
                param[2] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
                //***********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveUnPackplan_Stockno", param);
                Tran.Commit();
                lblmsg.Text = "Carpet No. UNPACKED saved successfully.";
                Fillgrid();
                txttotalunpackpcs.Text = "";
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
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "unpackalt", "alert('Please select atleast one check box to save Detail.')", true);
        }
    }
    protected void DDPacktype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillArticleno();
    }
    protected void txtbatchno_TextChanged(object sender, EventArgs e)
    {
        FillBatchDetails();
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
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@BatchNo", txtbatchno.Text);
            param[1] = new SqlParameter("@Planid", hnplanid.Value);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            //**********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeletePackingplanweekly", param);
            if (param[2].Value.ToString() != "")
            {
                lblmsg.Text = param[2].Value.ToString();
                Tran.Rollback();
            }
            else
            {
                lblmsg.Text = "Packing Plan deleted successfully..";
                hnplanid.Value = "0";
                Tran.Commit();
            }
            FillBatchDetails();
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

    protected void lnkcheckpcs_Click(object sender, EventArgs e)
    {
        int j = Convert.ToInt16(txtpackpcs_chk.Text == "" ? "0" : txtpackpcs_chk.Text);
        int k = DGStockDetail.Rows.Count;
        int rowcount = 0;
        //       
        if (j > 0)
        {
            for (int i = 0; i < k; i++)
            {
                if (j == rowcount)
                {
                    break;
                }
                if (txtdate_stamp.Text != "")
                {
                    Label lbldatestamp = (Label)DGStockDetail.Rows[i].FindControl("lbldatestamp");
                    if (string.Compare(lbldatestamp.Text, txtdate_stamp.Text, true) == 0)
                    {
                        CheckBox Chkboxitem = (CheckBox)DGStockDetail.Rows[i].FindControl("Chkboxitem");
                        Chkboxitem.Checked = true;
                        rowcount++;
                    }
                }
                else
                {
                    CheckBox Chkboxitem = (CheckBox)DGStockDetail.Rows[i].FindControl("Chkboxitem");
                    Chkboxitem.Checked = true;
                    rowcount++;
                }

            }
        }
        ModalPopupExtenderpackcarpet.Show();
    }
    protected void DG_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DG.EditIndex = e.NewEditIndex;
        Fillgrid();
    }
    protected void DG_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DG.EditIndex = -1;
        Fillgrid();
    }
    protected void DG_RowUpdating(object sender, GridViewUpdateEventArgs e)
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
            Label lbldetailid = (Label)DG.Rows[e.RowIndex].FindControl("lbldetailid");
            TextBox txteditplanpcs = (TextBox)DG.Rows[e.RowIndex].FindControl("txteditplanpcs");

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@Plandetailid", lbldetailid.Text);
            param[1] = new SqlParameter("@Planpcs", txteditplanpcs.Text == "" ? "0" : txteditplanpcs.Text);
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            //**********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_Updatepackingplanpcs", param);
            lblmsg.Text = param[4].Value.ToString();
            Tran.Commit();
            DG.EditIndex = -1;
            Fillgrid();
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
    protected void btncompletebatchno_Click(object sender, EventArgs e)
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
            param[0] = new SqlParameter("@BatchNo", txtbatchno.Text);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            param[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            //***********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_updatepackingBatchStatus", param);
            lblmsg.Text = param[3].Value.ToString();
            Tran.Commit();

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
    protected void Chkclosebatchno_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkclosebatchno.Checked == true)
        {
            txtbatchno_AutoCompleteExtender.ContextKey = "CLOSE#0#0";
        }
        else
        {
            txtbatchno_AutoCompleteExtender.ContextKey = "OPEN#0#0";
        }
    }
    protected void DDcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDitemname, @"select IM.ITEM_ID,IM.ITEM_NAME From Item_Master IM inner join CategorySeparate CS on IM.CATEGORY_ID=CS.Categoryid
                                                            and CS.id=0 and cs.categoryid=" + DDcategory.SelectedValue + " and IM.MasterCompanyid=" + Session["varcompanyId"] + " order by IM.ITEM_NAME", true, "--Plz Select--");
    }
    protected void FillData()
    {
        string str = @"select PA.ArticleNo,PA.Itemid,PA.QualityId,PA.Designid,PA.Colorid,PA.shapeid,PA.sizeid,PA.descofgoods,PA.contents,PA.weight_roll,PA.Netwt,volume_roll,PA.pcs_roll,PA.packingtypeid,IM.category_id from Packingarticle PA(NoLock) inner join ITEM_MASTER IM(NoLock) on PA.itemid=IM.Item_id Where  PA.articleno='" + DDarticleno.SelectedItem.Text + @"'
                     select Replace(convert(nvarchar(11),EffDate,106),' ','-') as EffDate,Rate,Grwt,Netwt,Vol,Pcs from PackingarticleRate(NoLock) Where Articleno='" + DDarticleno.SelectedItem.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {

            DDPacktype.SelectedValue = ds.Tables[0].Rows[0]["packingtypeid"].ToString();
            DDcategory.SelectedValue = ds.Tables[0].Rows[0]["category_id"].ToString();
            DDcategory_SelectedIndexChanged(DDcategory, new EventArgs());
            DDitemname.SelectedValue = ds.Tables[0].Rows[0]["Itemid"].ToString();
            Fillquality();
            DDquality.SelectedValue = ds.Tables[0].Rows[0]["qualityid"].ToString();
            FillDesign();
            DDdesign.SelectedValue = ds.Tables[0].Rows[0]["Designid"].ToString();
            FillColor();
            DDcolor.SelectedValue = ds.Tables[0].Rows[0]["colorid"].ToString();
            FillShape();
            DDshape.SelectedValue = ds.Tables[0].Rows[0]["shapeid"].ToString();
            FillSize();
            DDSize.SelectedValue = ds.Tables[0].Rows[0]["Sizeid"].ToString(); 
            
        }
        else
        {
            //refreshcontrol();
        }
    }
}