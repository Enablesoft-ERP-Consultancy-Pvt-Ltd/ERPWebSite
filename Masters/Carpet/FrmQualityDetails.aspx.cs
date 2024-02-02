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

public partial class Masters_Carpet_FrmQualityDetails : System.Web.UI.Page
{
    //public string mode = "";

    protected void BindLossSq()
    {
        if (ddlUnit.SelectedValue == "1")
        {
            lLossMeter.Visible = true;
            lLossSq.Visible = false;
        }
        else
        {
            lLossMeter.Visible = false;
            lLossSq.Visible = true;
        }
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

            ddlType.DataValueField = "ITEM_ID";
            ddlType.DataTextField = "ITEM_NAME";
            ddlType.DataSource = dt;
            ddlType.DataBind();
            ddlType.Items.Insert(0, "--Select--");
            ddlType.Items[0].Value = "0";

            tran.Commit();

            ViewState["CarpetType"] = ddlType.SelectedValue;
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
        if (Convert.ToInt32(ddlType.SelectedValue) > 0)
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
                param.Add("@Item_Id", ddlType.SelectedValue);
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
    protected void BindDesignName()
    {
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
                if (ddlQualityName.SelectedIndex != 0)
                {
                    Hashtable param = new Hashtable();
                    param.Add("@mode", "Get");
                    param.Add("@Item_Id", ddlType.SelectedValue);
                    param.Add("@QualityId", ddlQualityName.SelectedValue);
                    DataTable dt = DataAccess.fetch("Pro_GetDesignName", param);

                    cblDesignName.DataValueField = "designId";
                    cblDesignName.DataTextField = "designName";
                    cblDesignName.DataSource = dt;
                    cblDesignName.DataBind();

                    PDesignName.Visible = true;
                }
                else
                {
                    PDesignName.Visible = false;
                    cblDesignName.Items.Clear();
                    cblDesignName.DataSource = null;
                    cblDesignName.DataBind();
                }

                if (hnModeUpdate.Value == "update")
                {
                    cblDesignName.Enabled = false;
                }
                else
                {
                    cblDesignName.Enabled = true;
                }

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
            ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Please select quality type');", true);
        }
    }
    protected void BindColorName(int colorid)
    {
        int count = 0;
        foreach (ListItem li in cblDesignName.Items)
        {
            if (li.Selected == true)
            {
                count = count + 1;
            }
        }

        if (colorid > 0)
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
                param.Add("@designId", colorid);
                DataTable dt = DataAccess.fetch("Pro_GetColorName", param);

                ddlColorName.DataValueField = "colorid";
                ddlColorName.DataTextField = "ColorName";
                ddlColorName.DataSource = dt;
                ddlColorName.DataBind();
                ddlColorName.Items.Insert(0, "All Color");
                ddlColorName.Items[0].Value = "0";
                //ddlColorName.Items.Insert(0, "--Select--");
                //ddlColorName.Items[0].Value = "-1";


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
                param.Add("@designId", 0);
                DataTable dt = DataAccess.fetch("Pro_GetColorName", param);

                ddlColorName.DataValueField = "colorid";
                ddlColorName.DataTextField = "ColorName";
                ddlColorName.DataSource = dt;
                ddlColorName.DataBind();
                //ddlColorName.Items.Insert(0, "All Color");
                //ddlColorName.Items[0].Value = "0";

                tran.Commit();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();

            }
        }
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

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            Hashtable param = new Hashtable();
            param.Add("@qualityid", hnQualityId.Value);
            DataTable dt = DataAccess.fetch("pro_GetAllQualityCarpetDetails", param);

            //for (int i = 0; i < dt.Columns.Count; i++)
            //{
            //    BoundField boundfield = new BoundField();
            //    boundfield.DataField = dt.Columns[i].ColumnName.ToString();
            //    boundfield.HeaderText = dt.Columns[i].ColumnName.ToString();
            //    DGReceivedDetail.Columns.Add(boundfield);
            //}   

            DataView dv = new DataView(dt);
            if (ddlType.SelectedIndex > 0)
            {

                dv.RowFilter = "QualityTypeId="+ddlType.SelectedValue;
            }
            if (ddlType.SelectedIndex > 0 && ddlQualityName.SelectedIndex > 0)
            {
                dv.RowFilter = "QualityTypeId=" + ddlType.SelectedValue + " and QualityId=" + ddlQualityName.SelectedValue;
            }

            DGReceivedDetail.DataSource = dv;
            DGReceivedDetail.DataBind();

           

            if (DGReceivedDetail.Rows.Count > 0)
            {
                //DGReceivedDetail.HeaderRow.Cells[0].Visible = false;
                DGReceivedDetail.HeaderRow.Cells[0].Style.Add("display", "none");
                DGReceivedDetail.HeaderRow.Cells[1].Style.Add("display", "none");
                DGReceivedDetail.HeaderRow.Cells[2].Style.Add("display", "none");
                DGReceivedDetail.HeaderRow.Cells[3].Style.Add("display", "none");
                DGReceivedDetail.HeaderRow.Cells[4].Style.Add("display", "none");
                DGReceivedDetail.HeaderRow.Cells[5].Style.Add("display", "none");
                DGReceivedDetail.HeaderRow.Cells[6].Style.Add("display", "none");
            }

            string QualityPrice = (Math.Round(Convert.ToDecimal(tbWeavingCharges.Text), 3) + Math.Round(Convert.ToDecimal(tbCommission.Text), 3)).ToString();
            tbProductionPrice.Text = QualityPrice;

            tran.Commit();

        }
        catch (Exception ex)
        {
            tran.Rollback();
            //lblmsg.Text = ex.Message;
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
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            BindQualityType();
            BindRawItems();
            BindGridData();
            tbRelaxation.Text = "5";

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
    protected void tbWeavingCharges_TextChanged(object sender, EventArgs e)
    {
        ////string QualityPrice = (Convert.ToInt32(tbWeavingCharges.Text) + Convert.ToInt32(tbCommission.Text)).ToString();
        string QualityPrice = (Math.Round(tbWeavingCharges.Text == "" ? 0 : Convert.ToDecimal(tbWeavingCharges.Text), 3) + Math.Round(tbCommission.Text=="" ?0: Convert.ToDecimal(tbCommission.Text), 3)).ToString();

       tbProductionPrice.Text =QualityPrice;
    }
    protected void tbCommission_TextChanged(object sender, EventArgs e)
    {
        ////string QualityPrice = (Convert.ToInt32(tbWeavingCharges.Text) + Convert.ToInt32(tbCommission.Text)).ToString();
        string QualityPrice = (Math.Round(tbWeavingCharges.Text == "" ? 0 : Convert.ToDecimal(tbWeavingCharges.Text), 3) + Math.Round(tbCommission.Text=="" ?0: Convert.ToDecimal(tbCommission.Text), 3)).ToString();
        
        tbProductionPrice.Text = QualityPrice;
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

                if (ddlUnit.SelectedValue != "0" && (hnModeUpdate.Value == "" || hnModeUpdate.Value == null))
                {
                    rawtotalqty = Math.Round(Convert.ToDouble(((TextBox)item.FindControl("TextBox1")).Text) / 1.196, 5);
                    // rawtotalqty = Math.Round(Convert.ToDouble(((TextBox)item.FindControl("TextBox1")).Text), 3);
                }
                else if (ddlUnit.SelectedValue != "0" && hnModeUpdate.Value == "update")
                {
                    rawtotalqty = Math.Round(Convert.ToDouble(((TextBox)item.FindControl("TextBox1")).Text) / 1.196, 5);
                    // rawtotalqty = Math.Round(Convert.ToDouble(((TextBox)item.FindControl("TextBox1")).Text), 3);
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
        BindGridData();
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
        //***********Designid
        int count = 0;
        for (int i = 0; i < cblDesignName.Items.Count; i++)
        {
            if (cblDesignName.Items[i].Selected == true)
            {
                count += 1;
                Designid = Designid == "" ? cblDesignName.Items[i].Value : Designid + "," + cblDesignName.Items[i].Value;
            }
        }
        //*************

        if (count > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[15];
                param[0] = new SqlParameter("@Quality1TableId", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = hnQuality1TableId.Value == "" ? "0" : hnQuality1TableId.Value;
                param[1] = new SqlParameter("@QualityTypeId", ddlType.SelectedIndex > 0 ? ddlType.SelectedValue : "0");
                param[2] = new SqlParameter("@QualityId", ddlQualityName.SelectedIndex > 0 ? ddlQualityName.SelectedValue : "0");
                param[3] = new SqlParameter("@unitid", ddlUnit.SelectedIndex > 0 ? ddlUnit.SelectedValue : "0");
                param[4] = new SqlParameter("@colorid", ddlColorName.SelectedIndex > 0 ? ddlColorName.SelectedValue : "0");
                param[5] = new SqlParameter("@WeavingCharges", tbWeavingCharges.Text == "" ? "0" : tbWeavingCharges.Text);
                param[6] = new SqlParameter("@Commission", tbCommission.Text == "" ? "0" : tbCommission.Text);
                param[7] = new SqlParameter("@QualityPrice", tbProductionPrice.Text == "" ? "0" : tbProductionPrice.Text);
                param[8] = new SqlParameter("@Loss", tbLossSqYd.Text == "" ? "0" : tbLossSqYd.Text);
                param[9] = new SqlParameter("@RelaxationInPer", tbRelaxation.Text == "" ? "0" : tbRelaxation.Text);
                param[10] = new SqlParameter("@Rawmatusedid", Rawmatid.TrimEnd(','));
                param[11] = new SqlParameter("@Rawmatquantity", Rawmatqty.TrimEnd(','));
                param[12] = new SqlParameter("@Designid", Designid);
                param[13] = new SqlParameter("@dtrecords", dtrecords);
                param[14] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[14].Direction = ParameterDirection.Output;

                //**********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_Savequalitydetail", param);
                lblmsg.Text = param[14].Value.ToString();
                Tran.Commit();

                Session.Remove("AddRawSubItem");
                Refreshcontrol();
                //BindGridData();

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
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('Please select design...')", true);
        }
    }
    protected void Refreshcontrol()
    {
        //ddlType.SelectedValue = "0";
        //ddlQualityName.SelectedValue = "0";
        if (ddlColorName.Items.Count > 0)
        {
            ddlColorName.SelectedIndex = 0;
        }
        
       
        //cblDesignName.Items.Clear();
        tbWeavingCharges.Text = "";
        tbCommission.Text = "";
        tbProductionPrice.Text = "";
        tbLossSqYd.Text = "";
        tbRelaxation.Text = "";
        hnModeUpdate.Value = "";
        hnQualityId.Value = "";
        BindRawItems();
        rptRawSubItems.DataSource = null;
        rptRawSubItems.DataBind();
        btnsave.Text = "Save";
    }
    protected void ddlUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindLossSq();
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlType.SelectedValue) > 0)
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
                param.Add("@Item_Id", ddlType.SelectedValue);
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
        if (hnModeUpdate.Value == "update")
        {
            cblDesignName.Enabled = false;
        }
        else
        {
            cblDesignName.Enabled = true;
        }

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
                if (ddlQualityName.SelectedIndex != 0)
                {
                    Hashtable param = new Hashtable();
                    param.Add("@mode", "Get");
                    param.Add("@Item_Id", ddlType.SelectedValue);
                    param.Add("@QualityId", ddlQualityName.SelectedValue);
                    DataTable dt = DataAccess.fetch("Pro_GetDesignName", param);

                    cblDesignName.DataValueField = "designId";
                    cblDesignName.DataTextField = "designName";
                    cblDesignName.DataSource = dt;
                    cblDesignName.DataBind();

                    PDesignName.Visible = true;
                }
                else
                {
                    PDesignName.Visible = false;
                    cblDesignName.Items.Clear();
                    cblDesignName.DataSource = null;
                    cblDesignName.DataBind();
                }

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
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Please select quality type');", true);
        }
    }
    protected void cblDesignName_SelectedIndexChnaged(object sender, System.EventArgs e)
    {
        int count = 0;
        foreach (ListItem li in cblDesignName.Items)
        {
            if (li.Selected == true)
            {
                count = count + 1;
            }
        }

        if (count > 1)
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
                param.Add("@designId", 0);
                DataTable dt = DataAccess.fetch("Pro_GetColorName", param);

                ddlColorName.DataValueField = "colorid";
                ddlColorName.DataTextField = "ColorName";
                ddlColorName.DataSource = dt;
                ddlColorName.DataBind();
                //ddlColorName.Items.Insert(0, "ALL Color");
                //ddlColorName.Items[0].Value = "0";

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }
        }
        else if (count == 1)
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
                param.Add("@designId", cblDesignName.SelectedValue);
                DataTable dt = DataAccess.fetch("Pro_GetColorName", param);

                ddlColorName.DataValueField = "colorid";
                ddlColorName.DataTextField = "ColorName";
                ddlColorName.DataSource = dt;
                ddlColorName.DataBind();
                ddlColorName.Items.Insert(0, "All Color");
                ddlColorName.Items[0].Value = "0";
                //ddlColorName.Items.Insert(0, "--Select--");
                //ddlColorName.Items[0].Value = "-1";

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }
        }
        else if (count <=0)
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
                param.Add("@designId", cblDesignName.SelectedValue);
                DataTable dt = DataAccess.fetch("Pro_GetColorName", param);

                ddlColorName.DataValueField = "colorid";
                ddlColorName.DataTextField = "ColorName";
                ddlColorName.DataSource = dt;
                ddlColorName.DataBind();
                ddlColorName.Items.Insert(0, "--Select--");
                ddlColorName.Items[0].Value = "-1";

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }
        }
    }
    protected void DGReceivedDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // e.Row.Cells[0].Visible = false;
            e.Row.Cells[0].Style.Add("display", "none");
            e.Row.Cells[1].Style.Add("display", "none");
            e.Row.Cells[2].Style.Add("display", "none");
            e.Row.Cells[3].Style.Add("display", "none");
            e.Row.Cells[4].Style.Add("display", "none");
            e.Row.Cells[5].Style.Add("display", "none");
            e.Row.Cells[6].Style.Add("display", "none");

            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGReceivedDetail, "select$" + e.Row.RowIndex);

        }
    }
    protected void DGReceivedDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        btnsave.Text = "Save";

        rptRawSubItems.DataSource = null;
        rptRawSubItems.DataBind();
        string id = DGReceivedDetail.SelectedDataKey.Value.ToString();
        hnQuality1TableId.Value = id;
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
            param[0] = new SqlParameter("@Quality1TableId", id);


            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetCarpetDetailsOnId", param);

            if (ds.Tables[0].Rows.Count == 1)
            {
                int designid = 0;
                ddlType.SelectedValue = ds.Tables[0].Rows[0]["QualityTypeId"].ToString();
                BindQualityName();
                ddlQualityName.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();

                hnQualityId.Value = ddlQualityName.SelectedValue;

                ddlUnit.SelectedValue = ds.Tables[0].Rows[0]["unitid"].ToString();
                tbWeavingCharges.Text = ds.Tables[0].Rows[0]["WeavingCharges"].ToString();
                tbCommission.Text = ds.Tables[0].Rows[0]["Commission"].ToString();
                tbProductionPrice.Text = ds.Tables[0].Rows[0]["QualityPrice"].ToString();
                tbLossSqYd.Text = ds.Tables[0].Rows[0]["Loss"].ToString();
                BindDesignName();
                cblDesignName.SelectedValue = ds.Tables[0].Rows[0]["DesignId"].ToString();

                if (ds.Tables[0].Rows[0]["Colorid"].ToString() == "0")
                {
                    designid = 0;
                    BindColorName(designid);
                    ddlColorName.SelectedValue = ds.Tables[0].Rows[0]["Colorid"].ToString();
                }
                else
                {

                    designid = Convert.ToInt32(cblDesignName.SelectedValue);
                    BindColorName(designid);
                    ddlColorName.SelectedValue = ds.Tables[0].Rows[0]["Colorid"].ToString();
                }
                tbRelaxation.Text = ds.Tables[0].Rows[0]["RelaxationInPer"].ToString();
            }
            BindLossSq();

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

            cblDesignName.Enabled = false;

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

    protected void DGReceivedDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGReceivedDetail.PageIndex = e.NewPageIndex;
        BindGridData();
    }
}