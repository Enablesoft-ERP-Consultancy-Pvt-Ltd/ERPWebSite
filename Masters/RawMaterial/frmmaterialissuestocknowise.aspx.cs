using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_RawMaterial_frmmaterialissuestocknowise : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            if (Session["canedit"].ToString() == "1")
            {
                Tdedit.Visible = true;
            }
        }
    }
    protected void txtstockno_TextChanged(object sender, EventArgs e)
    {
        hnissueid.Value = "0";
        lblmsg.Text = "";
        Fillgrid();
        if (chkedit.Checked == true)
        {
            fillissueno();
        }
        txtstockno.Focus();

    }
    protected void fillissueno()
    {
        string str = @"SELECT PM.PRMID,PM.CHALANNO+' # '+REPLACE(CONVERT(NVARCHAR(11),PM.DATE,106),' ','-') AS CHALANNO 
        FROM PROCESSRAWMASTER PM 
        INNER JOIN LOOMSTOCKNO LS ON PM.STOCKNO=LS.STOCKNO AND LS.ProcessID = 1 
        Where PM.TypeFlag = 0 And Ls.Tstockno='" + txtstockno.Text + "' order by prmid";
        UtilityModule.ConditionalComboFill(ref DDissueno, str, true, "--Plz Select--");
        gvdetail.DataSource = null;
        gvdetail.DataBind();
    }
    protected void DDgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlgodown = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlgodown.Parent.Parent;
        Label Ifinishedid = ((Label)row.FindControl("lblifinishedid"));

        if (variable.VarBINNOWISE == "1")
        {
            DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));
            string str = "select Distinct S.BinNo,S.BinNo from Stock S Where S.companyid=" + hncompanyid.Value + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Qtyinhand>0";
            UtilityModule.ConditionalComboFill(ref DDBinNo, str, true, "--Plz Select--");
            if (DDBinNo.Items.Count > 0)
            {
                DDBinNo.SelectedIndex = 1;
            }
        }
        else
        {
            int index = row.RowIndex;

            DropDownList ddLotno = ((DropDownList)row.FindControl("DDLotNo"));
            string str = "select Distinct S.Lotno,S.Lotno from Stock S Where S.companyid=" + hncompanyid.Value + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Qtyinhand>0";
            UtilityModule.ConditionalComboFill(ref ddLotno, str, true, "--Plz Select--");
            if (ddLotno.Items.Count > 0)
            {
                ddLotno.SelectedIndex = 1;
                DDLotno_SelectedIndexChanged(ddLotno, e);
            }
        }

    }
    protected void DDLotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlLotno = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlLotno.Parent.Parent;
        int index = row.RowIndex;
        //Label Ifinishedid = ((Label)DG.Rows[index].FindControl("lblifinishedid"));
        Label Ifinishedid = (Label)row.FindControl("lblifinishedid");
        //DropDownList DDTagNo = ((DropDownList)DG.Rows[index].FindControl("DDTagNo"));
        //DropDownList ddlgodown = ((DropDownList)DG.Rows[index].FindControl("DDgodown"));
        DropDownList DDTagNo = ((DropDownList)row.FindControl("DDTagNo"));
        DropDownList ddlgodown = ((DropDownList)row.FindControl("DDgodown"));
        DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));

        string str = "select Distinct S.TagNo,S.Tagno from Stock S Where S.companyid=" + hncompanyid.Value + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Lotno='" + ddlLotno.Text + "' and S.Qtyinhand>0";
        if (variable.VarBINNOWISE == "1")
        {
            str = str + " and BinNo='" + DDBinNo.SelectedItem.Text + "'";
        }
        UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "--Plz Select--");
        if (DDTagNo.Items.Count > 0)
        {
            DDTagNo.SelectedIndex = 1;
            DDTagno_SelectedIndexChanged(DDTagNo, e);
        }
    }
    protected void DDTagno_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddTagno = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddTagno.Parent.Parent;
        int index = row.RowIndex;
        int Ifinishedid = Convert.ToInt32(((Label)row.FindControl("lblifinishedid")).Text);
        Label lblstockqty = ((Label)row.FindControl("lblstockqty"));
        DropDownList ddgodown = ((DropDownList)row.FindControl("DDgodown"));
        DropDownList ddlotno = ((DropDownList)row.FindControl("DDLotNo"));
        DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));

        Double StockQty = UtilityModule.getstockQty(hncompanyid.Value, ddgodown.SelectedValue, ddlotno.Text, Ifinishedid, ddTagno.Text, BinNo: (variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : ""));
        lblstockqty.Text = StockQty.ToString();
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            for (int i = 0; i < DG.Columns.Count; i++)
            {
                if (variable.VarBINNOWISE == "1")
                {
                    if (DG.Columns[i].HeaderText == "BinNo")
                    {
                        DG.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (DG.Columns[i].HeaderText == "BinNo")
                    {
                        DG.Columns[i].Visible = false;
                    }
                }


            }
            DropDownList DDGodown = ((DropDownList)e.Row.FindControl("DDGodown"));
            string str = @"select GoDownID,GodownName from GodownMaster order by GodownName
                           select godownid From Modulewisegodown Where ModuleName='" + Page.Title + "'";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDGodown, ds, 0, true, "--Plz Select--");

            if (hngodownid.Value == "0")
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (DDGodown.Items.FindByValue(ds.Tables[1].Rows[0]["godownid"].ToString()) != null)
                    {
                        DDGodown.SelectedValue = ds.Tables[1].Rows[0]["godownid"].ToString();
                    }
                }
                else
                {
                    if (DDGodown.Items.Count > 0)
                    {
                        DDGodown.SelectedIndex = 1;
                    }
                }

            }
            else
            {
                if (DDGodown.Items.FindByValue(hngodownid.Value) != null)
                {
                    DDGodown.SelectedValue = hngodownid.Value;
                }
            }
            DDgodown_SelectedIndexChanged(DDGodown, new EventArgs());
            ds.Dispose();
        }
    }
    protected void DDBinNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList DDBinNo = (DropDownList)sender;
        GridViewRow row = (GridViewRow)DDBinNo.Parent.Parent;
        Label Ifinishedid = ((Label)row.FindControl("lblifinishedid"));
        DropDownList ddlgodown = ((DropDownList)row.FindControl("DDGodown"));

        DropDownList ddLotno = ((DropDownList)row.FindControl("DDLotNo"));
        string str = "select Distinct S.Lotno,S.Lotno from Stock S Where S.companyid=" + hncompanyid.Value + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Qtyinhand>0";
        if (variable.VarBINNOWISE == "1")
        {
            str = str + " and BinNo='" + DDBinNo.SelectedItem.Text + "'";
        }
        UtilityModule.ConditionalComboFill(ref ddLotno, str, true, "--Plz Select--");
        if (ddLotno.Items.Count > 0)
        {
            ddLotno.SelectedIndex = 1;
            DDLotno_SelectedIndexChanged(ddLotno, e);
        }
    }
    protected void Fillgrid()
    {
        DG.DataSource = null;
        DG.DataBind();
        hnstockno.Value = "0";
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@tstockno", txtstockno.Text);
        param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
        param[2] = new SqlParameter("@CompanyID", Session["CurrentWorkingCompanyID"]);
        param[1].Direction = ParameterDirection.Output;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETSTOCKWISEMATERIALDETAILS", param);
        if (param[1].Value.ToString() != "")
        {
            lblmsg.Text = param[1].Value.ToString();
            txtstockno.Text = "";
        }
        else
        {
            DG.DataSource = ds.Tables[0];
            DG.DataBind();
            if (ds.Tables[0].Rows.Count > 0)
            {
                hncompanyid.Value = ds.Tables[0].Rows[0]["companyid"].ToString();
                hnstockno.Value = ds.Tables[0].Rows[0]["stockno"].ToString();
            }

        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //********sql table Type
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("ifinishedid", typeof(int));
        dtrecords.Columns.Add("IUnitid", typeof(int));
        dtrecords.Columns.Add("Isizeflag", typeof(int));
        dtrecords.Columns.Add("Godownid", typeof(int));
        dtrecords.Columns.Add("Lotno", typeof(string));
        dtrecords.Columns.Add("TagNo", typeof(string));
        dtrecords.Columns.Add("issueqty", typeof(float));
        //dtrecords.Columns.Add("Noofcone", typeof(int));
        dtrecords.Columns.Add("Prorderid", typeof(int));
        dtrecords.Columns.Add("ConsmpQty", typeof(float));
        dtrecords.Columns.Add("BinNo", typeof(string));
        //*******************
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtissueqty = ((TextBox)DG.Rows[i].FindControl("txtissueqty"));
            DropDownList DDGodown = ((DropDownList)DG.Rows[i].FindControl("DDGodown"));
            DropDownList DDLotNo = ((DropDownList)DG.Rows[i].FindControl("DDLotNo"));
            DropDownList DDTagNo = ((DropDownList)DG.Rows[i].FindControl("DDTagNo"));
            DropDownList DDBinNo = ((DropDownList)DG.Rows[i].FindControl("DDBinNo"));

            if (Chkboxitem.Checked == true && (txtissueqty.Text != "") && DDGodown.SelectedIndex > 0 && DDLotNo.SelectedIndex > 0 && DDTagNo.SelectedIndex > 0)
            {
                Label lblitemfinishedid = ((Label)DG.Rows[i].FindControl("lblifinishedid"));
                Label lblunitid = ((Label)DG.Rows[i].FindControl("lbliunitid"));
                Label lblflagsize = ((Label)DG.Rows[i].FindControl("lblisizeflag"));
                string Lotno = DDLotNo.Text;
                string TagNo = DDTagNo.Text;
                //TextBox txtnoofcone = ((TextBox)DG.Rows[i].FindControl("txtnoofcone"));
                Label lblissueorderid = ((Label)DG.Rows[i].FindControl("lblissueorderid"));
                Label lblconsmpqty = ((Label)DG.Rows[i].FindControl("lblconsmpqty"));
                //*********************
                DataRow dr = dtrecords.NewRow();
                dr["ifinishedid"] = lblitemfinishedid.Text;
                dr["IUnitid"] = lblunitid.Text;
                dr["Isizeflag"] = lblflagsize.Text;
                dr["Godownid"] = DDGodown.SelectedValue;
                dr["Lotno"] = Lotno;
                dr["TagNo"] = TagNo;
                dr["IssueQty"] = txtissueqty.Text == "" ? "0" : txtissueqty.Text;
                //dr["Noofcone"] = txtnoofcone.Text == "" ? "0" : txtnoofcone.Text;
                dr["Prorderid"] = lblissueorderid.Text;
                dr["consmpqty"] = lblconsmpqty.Text;
                dr["BinNo"] = variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : "";
                dtrecords.Rows.Add(dr);
            }
        }
        if (dtrecords.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[11];
                param[0] = new SqlParameter("@PrmId", SqlDbType.Int);
                param[0].Value = hnissueid.Value;
                param[0].Direction = ParameterDirection.InputOutput;
                param[1] = new SqlParameter("@companyid", hncompanyid.Value);
                param[2] = new SqlParameter("@Processid", SqlDbType.Int);
                param[2].Value = 1;
                param[3] = new SqlParameter("@Prorderid", SqlDbType.Int);
                param[3].Value = 0;
                param[4] = new SqlParameter("@issueDate", System.DateTime.Now.ToString("dd-MMM-yyyy"));
                param[5] = new SqlParameter("@userid", Session["varuserid"]);
                param[6] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
                param[7] = new SqlParameter("@dtrecords", dtrecords);
                param[8] = new SqlParameter("@TranType", SqlDbType.TinyInt);
                param[8].Value = 0;
                param[9] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[9].Direction = ParameterDirection.Output;
                param[10] = new SqlParameter("@Stockno", hnstockno.Value);
                ///**********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEMATERIALISSUESTOCKWISE", param);
                //*******************
                ViewState["reportid"] = param[0].Value.ToString();
                //txtissueno.Text = param[0].Value.ToString();
                hnissueid.Value = param[0].Value.ToString();
                Tran.Commit();
                if (param[9].Value.ToString() != "")
                {
                    lblmsg.Text = param[9].Value.ToString();
                }
                else
                {
                    lblmsg.Text = "DATA SAVED SUCCESSFULLY.";
                    Fillgrid();
                    FillissueGrid();
                }

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
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
        }
    }
    protected void FillissueGrid()
    {
        string str = @"select dbo.F_getItemDescription(PT.Finishedid,PT.flagsize) as ItemDescription,
                    PT.Lotno,PT.TagNo,PT.IssueQuantity,PM.chalanNo,Replace(CONVERT(nvarchar(11),PM.date,106),' ','-') as IssueDate,PM.prmid,PT.Prtid,PM.prorderid,PM.processid
                    from ProcessRawMaster PM 
                    inner join ProcessRawTran PT on PM.PRMid=PT.PRMid
                    Where PM.TypeFlag = 0 And PM.BeamType=0 And PM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And PM.prmid=" + hnissueid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
    }

    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblprmid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprmid");
            Label lblprtid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprtid");
            Label lblprorderid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprorderid");
            Label lblprocessid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprocessid");

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@prmid", lblprmid.Text);
            param[1] = new SqlParameter("@prtid", lblprtid.Text);
            param[2] = new SqlParameter("@prorderid", lblprorderid.Text);
            param[3] = new SqlParameter("@Processid", lblprocessid.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEMATERIALISSUESTOCKNOWISE", param);
            lblmsg.Text = param[4].Value.ToString();
            Tran.Commit();
            FillissueGrid();
            //***************
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        Tdissueno.Visible = false;
        DDissueno.SelectedIndex = -1;
        hnissueid.Value = "0";
        if (chkedit.Checked == true)
        {
            Tdissueno.Visible = true;

        }
    }
    protected void DDissueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnissueid.Value = DDissueno.SelectedValue;
        FillissueGrid();
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@processid", 1);
            param[1] = new SqlParameter("@Prmid", hnissueid.Value);
            param[2] = new SqlParameter("@TStockNo", "");
            param[3] = new SqlParameter("@Type", 0);
            //************
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_STOCKNOWISEMATERIALISSUE", param);
            if (ds.Tables[0].Rows.Count > 0)
            {

                Session["rptFileName"] = "~\\Reports\\Rptstocknowisematerialissue.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\Rptstocknowisematerialissue.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
}