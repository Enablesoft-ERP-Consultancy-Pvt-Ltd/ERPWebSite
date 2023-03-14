using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.Services;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Text;

public partial class Masters_Carpet_FrmFinisherRawCalMaster : System.Web.UI.Page
{
    //public string mode = "";

    private void BindJobType()
    {
        string str = "select PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER WHERE MasterCompanyId=" + Session["varCompanyId"] + " and ProcessType=1 and Process_Name_id<>1 order by PROCESS_NAME";
       
        UtilityModule.ConditionalComboFill(ref DDJobType, str, true, "--Select--");
    }
    private void BindCalOption()
    {
        string str = "Select CalcId,CalcName From CalcOptions Order by CalcId";

        UtilityModule.ConditionalComboFill(ref DDCalOption, str, true, "--Select--");
    }
   
    protected void BindQualityType()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            Hashtable param = new Hashtable();
            param.Add("@mode", "Get");
            DataTable dt = DataAccess.fetch("Pro_GetQualityType", param);

            DDQualityType.DataValueField = "ITEM_ID";
            DDQualityType.DataTextField = "ITEM_NAME";
            DDQualityType.DataSource = dt;
            DDQualityType.DataBind();
            DDQualityType.Items.Insert(0, "--Select--");
            DDQualityType.Items[0].Value = "0";

            tran.Commit();

            ViewState["CarpetType"] = DDQualityType.SelectedValue;
        }
        catch (Exception ex)
        {
            tran.Rollback();

            //lblmsg.Text = ex.Message;
            con.Close();
            throw ex;
        }
    }
    protected void BindQualityName()
    {
        if (Convert.ToInt32(DDQualityType.SelectedValue) > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                Hashtable param = new Hashtable();
                param.Add("@mode", "Get");
                param.Add("@Item_Id", DDQualityType.SelectedValue);
                DataTable dt = DataAccess.fetch("Pro_GetQualityName", param);

                ddlQualityName.DataValueField = "QualityID";
                ddlQualityName.DataTextField = "QualityName";
                ddlQualityName.DataSource = dt;
                ddlQualityName.DataBind();
                ddlQualityName.Items.Insert(0, "--Select--");
                ddlQualityName.Items[0].Value = "0";

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }
        }
        else
        {
        }
    }
    private void BindDesignName()
    {
        string view = "V_FinishedItemDetail";
        if (variable.VarNewQualitySize == "1")
        {
            view = "V_FinishedItemDetailNew";
        }
        string str = "";

        str = @"select Distinct designId,designName From " + view + @" VF Where VF.ITEM_ID=" + DDQualityType.SelectedValue + " and VF.designId<>0";
        if (ddlQualityName.SelectedIndex > 0)
        {
            str = str + " and VF.Qualityid=" + ddlQualityName.SelectedValue;
        }
        str = str + " order by VF.Designname";      

        UtilityModule.ConditionalComboFill(ref DDDesign, str, true, "--Select--");
        
    }  

    protected void BindRawItems()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {

            Hashtable param = new Hashtable();
            param.Add("@mode", "Get");
            DataTable dt = DataAccess.fetch("Pro_GetQualityRawItems", param);

            rptRawItems.DataSource = dt;
            rptRawItems.DataBind();

            tran.Commit();

        }
        catch (Exception ex)
        {
            tran.Rollback();

            //lblmsg.Text = ex.Message;
            con.Close();
            throw ex;
        }
    }

    protected void BindRawSubItems(int ItemId)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            Hashtable param = new Hashtable();
            param.Add("@mode", "Get");
            param.Add("@Item_Id", ItemId);
            DataTable dt = DataAccess.fetch("Pro_GetQualityRawSubItems", param);

            rptRawSubItems.DataSource = dt;
            rptRawSubItems.DataBind();

            tran.Commit();
        }
        catch (Exception ex)
        {
            tran.Rollback();

            //lblmsg.Text = ex.Message;
            con.Close();
            throw ex;
        }
    }

    protected void BindGridData()
    {
        string where = "";
       
        if (DDJobType.SelectedIndex > 0)
        {
            where = where + " and FQM.JobId=" + DDJobType.SelectedValue;
        }
        if (DDCalOption.SelectedIndex > 0)
        {
            where = where + " and FQM.CalcOptionId=" + DDCalOption.SelectedValue;
        }  
        if (DDQualityType.SelectedIndex > 0)
        {
            where = where + " and FQM.Item_Id=" + DDQualityType.SelectedValue;
        }
        if (ddlQualityName.SelectedIndex > 0)
        {
            where = where + " and FQM.QualityId=" + ddlQualityName.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and FQM.Designid=" + DDDesign.SelectedValue;
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@RawId", "0");
            param[1] = new SqlParameter("@Where", where);

            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "PRO_GETFINISHERQUALITYDETAIL", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DGFinisherQualityDetail.DataSource = ds.Tables[0];
                DGFinisherQualityDetail.DataBind();
            }
            else
            {
                DGFinisherQualityDetail.DataSource = null;
                DGFinisherQualityDetail.DataBind();
            }

            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
            con.Close();
        }
        
    }

    DataTable dt;
    private void maketable()
    {
        dt.Columns.Add("ITEM_ID");
        dt.Columns.Add("QualityID");
        dt.Columns.Add("QualityName");
        dt.Columns.Add("qty");
    }

    public string rawitemsid = "";
    public string rawitemsqty = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            BindJobType();
            BindCalOption();
            BindQualityType();
            BindRawItems();
            //BindGridData();       

            txtEffectiveDate.Attributes.Add("readonly", "readonly");          
            txtEffectiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            Session.Remove("AddRawSubItem");
        }
    }
    public static Control FindControlRecursive(Control controlToStartFrom, string ctrlIdToFind)
    {
        Control found = controlToStartFrom.FindControl(ctrlIdToFind);

        if (found != null)
            return found;

        foreach (Control innerCtrl in controlToStartFrom.Controls)
        {
            found = FindControlRecursive(innerCtrl, ctrlIdToFind);

            if (found != null)
                return found;
        }
        return null;
    }
    protected void rptRawItems_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "subitem")
        {
            int ItemId = Convert.ToInt32(((Label)e.Item.FindControl("lbIemId")).Text);
            decimal totalqty = Math.Round(Convert.ToDecimal(((TextBox)e.Item.FindControl("TextBox1")).Text), 3);

            foreach (RepeaterItem item in rptRawItems.Items)
            {
                HtmlTableRow row = (HtmlTableRow)item.FindControl("row");
                row.Attributes["style"] = "background-color:#E3E3E3";

                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lbFavourites = (LinkButton)item.FindControl("lnkUpdate");

                    ScriptManager ScriptManager1 = (ScriptManager)FindControlRecursive(this.Page, "ScriptManager1");
                    ScriptManager1.RegisterAsyncPostBackControl(lbFavourites);
                }
            }

            RepeaterItem selectitem = (RepeaterItem)(((LinkButton)e.CommandSource).NamingContainer);
            HtmlTableRow currentrow = (HtmlTableRow)selectitem.FindControl("row");
            currentrow.Attributes["style"] = "background-color:#0080C0";

            if (totalqty > 0)
            {
                if (Session["AddRawSubItem"] != null)
                {
                    DataTable dt = (DataTable)Session["AddRawSubItem"];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (ItemId == Convert.ToInt32(dt.Rows[i]["ITEM_ID"].ToString()))
                        {
                            DataView dv = new DataView(dt);
                            dv.RowFilter = "ITEM_ID =" + ItemId;

                            rptRawSubItems.DataSource = dv;
                            rptRawSubItems.DataBind();
                        }
                    }
                }
            }
            else
            {
                BindRawSubItems(ItemId);
            }
        }
    }   
    protected void tbqty_TextChanged(object sender, EventArgs e)
    {
        double qty = 0;
        double totalqty = 0;

        int i = 0;
        int j = rptRawSubItems.Items.Count;
        int k = 0;
        int l = rptRawItems.Items.Count;

        if (j != 0)
        {
            for (i = 0; i < j; i++)
            {
                Label lblItemId = ((Label)rptRawSubItems.Items[i].FindControl("lblItemId"));
                Label lblQualityID = ((Label)rptRawSubItems.Items[i].FindControl("lblQualityID"));
                Label lblQualityName = ((Label)rptRawSubItems.Items[i].FindControl("lblQualityName"));
                TextBox TXTSUBQTY = ((TextBox)rptRawSubItems.Items[i].FindControl("tbqty"));
                if (TXTSUBQTY.Text != "" && Decimal.Parse(TXTSUBQTY.Text) > 0)
                {
                    for (k = 0; k < l; k++)
                    {
                        Label lbIemId = ((Label)rptRawItems.Items[k].FindControl("lbIemId"));
                        TextBox totalqty1 = ((TextBox)rptRawItems.Items[k].FindControl("TextBox1")) as TextBox;
                        //totalqty1.Text = "0";

                        if (lblItemId.Text == lbIemId.Text)
                        {
                            if (((TextBox)(rptRawSubItems.Items[i].FindControl("tbqty"))).Text == "")
                            {
                                ((TextBox)(rptRawSubItems.Items[i].FindControl("tbqty"))).Text = "0";
                            }
                            else
                            {
                                //qty = Math.Round(Convert.ToDecimal(((TextBox)(rptRawSubItems.Items[i].FindControl("tbqty"))).Text), 3);
                                qty = Math.Round(Convert.ToDouble(((TextBox)(rptRawSubItems.Items[i].FindControl("tbqty"))).Text), 5);
                            }
                            totalqty += qty;

                            totalqty1.Text = Convert.ToString(totalqty);
                            break;
                        }
                    }
                }

                if (Session["AddRawSubItem"] == null)
                {
                    dt = new DataTable();
                    maketable();
                    Session["AddRawSubItem"] = dt;
                }
                dt = (DataTable)Session["AddRawSubItem"];

                int m = 0;
                for (int n = 0; n < dt.Rows.Count; n++)
                {
                    if (dt.Rows[n]["QualityID"].ToString() == lblQualityID.Text)
                    {
                        m = 1;

                        dt.Rows.RemoveAt(n);
                        dt.AcceptChanges();
                        break;
                    }
                }

                //dt = new DataTable();
                //maketable();              

                DataRow dr = dt.NewRow();
                dr["ITEM_ID"] = lblItemId.Text;
                dr["QualityID"] = lblQualityID.Text;
                dr["QualityName"] = lblQualityName.Text;
                //dr["qty"] = TXTSUBQTY.Text;

                if (TXTSUBQTY.Text == "" || TXTSUBQTY.Text == null)
                {
                    TXTSUBQTY.Text = "0";
                    dr["qty"] = TXTSUBQTY.Text;
                }
                else
                {
                    dr["qty"] = Math.Round(Convert.ToDouble(TXTSUBQTY.Text), 3);
                }

                dt.Rows.Add(dr);
                Session["AddRawSubItem"] = dt;
            }
        }
    }
    protected DataTable getRawItemsnew()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Rawmatid", typeof(string));
        dt.Columns.Add("Rawmatqty", typeof(string));

        foreach (RepeaterItem item in rptRawItems.Items)
        {
            decimal totalqty = Math.Round(Convert.ToDecimal(((TextBox)item.FindControl("TextBox1")).Text), 3);
            double rawtotalqty = 0;

            if (totalqty > 0)
            {
                int ItemId = Convert.ToInt32(((Label)item.FindControl("lbIemId")).Text);

                if ((hnModeUpdate.Value == "" || hnModeUpdate.Value == null))
                {
                    //rawtotalqty = Math.Round(Convert.ToDouble(((TextBox)item.FindControl("TextBox1")).Text) / 1.196, 5);
                     rawtotalqty = Math.Round(Convert.ToDouble(((TextBox)item.FindControl("TextBox1")).Text), 3);
                }
                else if (hnModeUpdate.Value == "update")
                {
                    //rawtotalqty = Math.Round(Convert.ToDouble(((TextBox)item.FindControl("TextBox1")).Text) / 1.196, 5);
                     rawtotalqty = Math.Round(Convert.ToDouble(((TextBox)item.FindControl("TextBox1")).Text), 3);
                }
                else
                {
                    rawtotalqty = Math.Round(Convert.ToDouble(((TextBox)item.FindControl("TextBox1")).Text), 3);
                }


                if (ItemId != null && rawtotalqty != null)
                {
                    rawitemsid += ItemId + ",";
                    rawitemsqty += rawtotalqty + ",";
                }
            }

        }
        if (rawitemsid != "" && rawitemsqty != "")
        {
            DataRow dr = dt.NewRow();
            dr["Rawmatid"] = rawitemsid;
            dr["Rawmatqty"] = rawitemsqty;
            dt.Rows.Add(dr);
        }

        return dt;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Savedetail();
        //BindGridData();
        return;
    }
    protected void Savedetail()
    {

        //***********************Start Raw Sub Items

        //********sql table Type
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("ItemId", typeof(int));
        dtrecords.Columns.Add("SubItemId", typeof(int));
        dtrecords.Columns.Add("SubItemQty", typeof(decimal));
        dtrecords.Columns.Add("colorname", typeof(string));

        //*******************

        if (Session["AddRawSubItem"] != null)
        {
            DataTable dt3 = (DataTable)Session["AddRawSubItem"];
            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                decimal qty = Math.Round(Convert.ToDecimal(dt3.Rows[i]["qty"].ToString()), 3);

                if (qty > 0)
                {
                    DataRow dr = dtrecords.NewRow();
                    dr["ItemId"] = dt3.Rows[i]["ITEM_ID"].ToString();
                    dr["SubItemId"] = dt3.Rows[i]["QualityID"].ToString();
                    dr["SubItemQty"] = dt3.Rows[i]["qty"].ToString();

                    dr["colorname"] = null;
                    dtrecords.Rows.Add(dr);
                }
            }
        }

        //***********************End Raw Sub Items

        //RawMatusedid
        string Rawmatid = "", Rawmatqty = "", Designid = "";
        DataTable dt = getRawItemsnew();
        if (dt.Rows.Count == 0)
        {
        }
        else
        {
            Rawmatid = dt.Rows[0]["Rawmatid"].ToString();
            Rawmatqty = dt.Rows[0]["Rawmatqty"].ToString();

        }
        ////***********Designid
        //int count = 0;
        //for (int i = 0; i < cblDesignName.Items.Count; i++)
        //{
        //    if (cblDesignName.Items[i].Selected == true)
        //    {
        //        count += 1;
        //        Designid = Designid == "" ? cblDesignName.Items[i].Value : Designid + "," + cblDesignName.Items[i].Value;
        //    }
        //}
        //*************

        //if (count > 0)
        //{
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[15];
                param[0] = new SqlParameter("@RawId", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = hnRawId.Value == "" ? "0" : hnRawId.Value;
                param[1] = new SqlParameter("@JobId", DDJobType.SelectedIndex > 0 ? DDJobType.SelectedValue : "0");
                param[2] = new SqlParameter("@CalcOptionId", DDCalOption.SelectedIndex > 0 ? DDCalOption.SelectedValue : "0");
                param[3] = new SqlParameter("@Item_Id", DDQualityType.SelectedIndex > 0 ? DDQualityType.SelectedValue : "0");
                param[4] = new SqlParameter("@QualityId", ddlQualityName.SelectedIndex > 0 ? ddlQualityName.SelectedValue : "0");
                param[5] = new SqlParameter("@DesignId", DDDesign.SelectedIndex > 0 ? DDDesign.SelectedValue : "0");
                param[6] = new SqlParameter("@EffectiveDate", txtEffectiveDate.Text);
                param[7] = new SqlParameter("@dtrecords", dtrecords);
                param[8] = new SqlParameter("@MSGFLAG", SqlDbType.VarChar, 100);
                param[8].Direction = ParameterDirection.Output;
               
                //param[10] = new SqlParameter("@Rawmatusedid", Rawmatid.TrimEnd(','));
                //param[11] = new SqlParameter("@Rawmatquantity", Rawmatqty.TrimEnd(','));              
               

                //**********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveFinisherQualityDetail", param);
                lblmsg.Text = param[8].Value.ToString();
                Tran.Commit();

                Session.Remove("AddRawSubItem");
                Refreshcontrol();
                BindGridData();

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
        //}
        //else
        //{
        //    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('Please select design...')", true);
        //}
    }
    protected void Refreshcontrol()
    {
        //ddlType.SelectedValue = "0";
        ddlQualityName.SelectedValue = "0";
        DDDesign.SelectedValue = "0";       
        hnModeUpdate.Value = "";
        hnQualityId.Value = "";
        BindRawItems();
        rptRawSubItems.DataSource = null;
        rptRawSubItems.DataBind();
        btnsave.Text = "Save";
    }
    protected void DDQualityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(DDQualityType.SelectedValue) > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {

                Hashtable param = new Hashtable();
                param.Add("@mode", "Get");
                param.Add("@Item_Id", DDQualityType.SelectedValue);
                DataTable dt = DataAccess.fetch("Pro_GetQualityName", param);

                ddlQualityName.DataValueField = "QualityID";
                ddlQualityName.DataTextField = "QualityName";
                ddlQualityName.DataSource = dt;
                ddlQualityName.DataBind();
                ddlQualityName.Items.Insert(0, "--Select--");
                ddlQualityName.Items[0].Value = "0";

                tran.Commit();

                BindGridData();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }
        }
    }
    protected void ddlQualityName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (hnModeUpdate.Value == "update")
        //{
        //    cblDesignName.Enabled = false;
        //}
        //else
        //{
        //    cblDesignName.Enabled = true;
        //}

        

        if (Convert.ToInt32(ddlQualityName.SelectedValue) > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {

                BindDesignName();             

                BindGridData();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Please select quality type');", true);
        }
    }
    protected void DDJobType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGridData();
    }
    protected void DDCalOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGridData();
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGridData();
    }   
    protected void DGFinisherQualityDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        { 
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGFinisherQualityDetail, "select$" + e.Row.RowIndex);
        }
    }
    protected void DGFinisherQualityDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        btnsave.Text = "Save";

        rptRawSubItems.DataSource = null;
        rptRawSubItems.DataBind();
        string id = DGFinisherQualityDetail.SelectedDataKey.Value.ToString();
        hnRawId.Value = id;
        hnModeUpdate.Value = "update";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@RawId", id);


            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "PRO_GETFINISHERQUALITYDETAIL", param);

            if (ds.Tables[0].Rows.Count == 1)
            {
                DDJobType.Enabled = false;
                DDCalOption.Enabled = false;
                DDQualityType.Enabled = false;
                ddlQualityName.Enabled = false;
                DDDesign.Enabled = false;
                DDJobType.SelectedValue = ds.Tables[0].Rows[0]["JobId"].ToString();
                DDCalOption.SelectedValue = ds.Tables[0].Rows[0]["CalcId"].ToString();
                DDQualityType.SelectedValue = ds.Tables[0].Rows[0]["Item_Id"].ToString();
                BindQualityName();
                ddlQualityName.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
                hnQualityId.Value = ddlQualityName.SelectedValue;
                BindDesignName();
                DDDesign.SelectedValue = ds.Tables[0].Rows[0]["DesignId"].ToString();
                txtEffectiveDate.Text = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).ToString("dd-MMM-yyyy"));
               
            }           

            if (ds.Tables[1].Rows.Count > 0)
            {
                Session["AddRawSubItem"] = ds.Tables[1];
                DataTable dt3 = (DataTable)Session["AddRawSubItem"];


                int k = 0;
                int l = rptRawItems.Items.Count;
                Object sumObject;
                double totalqty = 0;

                for (k = 0; k < l; k++)
                {
                    Label lbIemId = ((Label)rptRawItems.Items[k].FindControl("lbIemId"));
                    TextBox totalqty1 = ((TextBox)rptRawItems.Items[k].FindControl("TextBox1")) as TextBox;
                    //totalqty1.Text = "0";
                    sumObject = dt3.Compute("Sum(qty)", "Item_Id = " + lbIemId.Text);
                    if (sumObject != DBNull.Value)
                    {
                        totalqty = Convert.ToDouble(sumObject);
                        totalqty1.Text = Convert.ToString(totalqty);

                    }

                    else
                    {
                        totalqty1.Text = "0";
                    }

                }

            }
            BindGridData();

            

            Tran.Commit();
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

        btnsave.Text = "Update";
        //btndelete.Visible = true;
    }
    protected void DGFinisherQualityDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
            int RawId = Convert.ToInt32(DGFinisherQualityDetail.DataKeys[e.RowIndex].Value);
            SqlParameter[] param = new SqlParameter[6];

            param[0] = new SqlParameter("@RawId", RawId);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "DELETEFINISHERQUALITYDETAIL", param);
            Tran.Commit();
            lblmsg.Visible = true;
            lblmsg.Text = param[3].Value.ToString();
            BindGridData();
           
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmFinisherRawCalMaster.aspx");
            Tran.Rollback();
            lblmsg.Visible = true;
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void FinisherQualityDetailReport()
    {
        string where = "";

        if (DDJobType.SelectedIndex > 0)
        {
            where = where + " and FQM.JobId=" + DDJobType.SelectedValue;
        }
        if (DDCalOption.SelectedIndex > 0)
        {
            where = where + " and FQM.CalcOptionId=" + DDCalOption.SelectedValue;
        }
        if (DDQualityType.SelectedIndex > 0)
        {
            where = where + " and FQM.Item_Id=" + DDQualityType.SelectedValue;
        }
        if (ddlQualityName.SelectedIndex > 0)
        {
            where = where + " and FQM.QualityId=" + ddlQualityName.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and FQM.Designid=" + DDDesign.SelectedValue;
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@RawId", "0");
            param[1] = new SqlParameter("@Where", where);
            param[2] = new SqlParameter("@MasterCompanyId", Session["VarCompanyId"]);
            param[3] = new SqlParameter("@UserId", Session["VarUserId"]);

            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "PRO_GETFINISHERQUALITYDETAILREPORT", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptFinisherRawMasterDetail.rpt";                   
                
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptFinisherRawMasterDetail.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }

            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
            con.Close();
        }
    }
    protected void FinisherQualityConsumptionNotDefineReport()
    {  
      
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@JobId", DDJobType.SelectedValue);            
            param[2] = new SqlParameter("@MasterCompanyId", Session["VarCompanyId"]);
            param[3] = new SqlParameter("@UserId", Session["VarUserId"]);

            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "PRO_GETFINISHERQUALITYCONSMPNOTDEFINEREPORT", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptFinisherConspNotDefine.rpt";

                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptFinisherConspNotDefine.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }

            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
            con.Close();
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        if (ChkForConsumptionNotDefine.Checked == true)
        {
            FinisherQualityConsumptionNotDefineReport();
        }
        else
        {
            FinisherQualityDetailReport();
        }
    }
    
}