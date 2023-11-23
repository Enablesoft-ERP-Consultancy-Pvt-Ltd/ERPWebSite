using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_MapStencil_FrmMapReceive_OnProductionOrder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                           select UnitsId,UnitName from Units order by UnitName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            txtReceivedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
            }

            hnRecid.Value = "0";
        }
    }

    //    protected void FillIssueNo()
    //    {
    //        string str = @"select IssueId,ChallanNo from Map_IssueOnProductionOrderMaster MIM
    //                       Where MIM.CompanyId=" + DDcompany.SelectedValue + " and MIM.IssueOrderId=" + DDFolioNo.SelectedValue + @" 
    //                        and  MIM.MapStencilType=" + DDMapStencilType.SelectedValue + @" ";

    //        UtilityModule.ConditionalComboFill(ref DDIssueNo, str, true, "--Plz Select--");

    //    }
    //    protected void DDIssueNo_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        hnissueid.Value = DDIssueNo.SelectedValue;
    //        FillissueGrid();
    //    }
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
        if (chkEdit.Checked == true)
        {
            TDChallanNo.Visible = true;
            DDcompany.Enabled = false;
            DDMapStencilType.Enabled = false;
        }
        else
        {
            TDChallanNo.Visible = false;
            txtChallanNo.Text = "";
            DDcompany.Enabled = true;
            DDMapStencilType.Enabled = true;
        }
        txtReceiveNo.Text = "";
        hnRecid.Value = "0";

        DG.DataSource = null;
        DG.DataBind();

        gvdetail.DataSource = null;
        gvdetail.DataBind();

    }
    protected void DDMapStencilType_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtReceiveNo.Text = "";
        hnRecid.Value = "0";

        DG.DataSource = null;
        DG.DataBind();

        gvdetail.DataSource = null;
        gvdetail.DataBind();
    }
    protected void txtMapStencilstockno_TextChanged(object sender, EventArgs e)
    {
        lblmessage.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@CompanyId", DDcompany.SelectedValue);
            param[1] = new SqlParameter("@MapStencilType", DDMapStencilType.SelectedValue);
            //param[2] = new SqlParameter("@IssueOrderId", DDFolioNo.SelectedValue);
            param[3] = new SqlParameter("@MSStockno", txtMapStencilstockno.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETWEAVERMAPRECEIVEDETAIL_WITHMAPSTOCKNO", param);
            if (param[4].Value.ToString() != "")
            {
                lblmessage.Text = param[4].Value.ToString();
                DG.DataSource = null;
                DG.DataBind();
            }
            else
            {
                DG.DataSource = ds.Tables[0];
                DG.DataBind();
                for (int i = 0; i < DG.Rows.Count; i++)
                {
                    CheckBox Chkboxitem = (CheckBox)DG.Rows[i].FindControl("Chkboxitem");
                    TextBox txtqty = (TextBox)DG.Rows[i].FindControl("txtqty");
                    Chkboxitem.Checked = true;
                    txtqty.Text = "1";
                }
                btnsave_Click(sender, new EventArgs());
                DG.DataSource = null;
                DG.DataBind();
                txtMapStencilstockno.Text = "";
            }
            txtMapStencilstockno.Focus();
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        //if (txtloomid.Text == "" || txtloomid.Text == "0")
        //{
        //    ScriptManager.RegisterStartupScript(Page, GetType(), "loomid", "alert('Please select Loom No.');", true);
        //    return;
        //}

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //**************Sql Table
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("Itemfinishedid", typeof(int));
        dtrecords.Columns.Add("IssueOrderId", typeof(int));
        dtrecords.Columns.Add("IssueId", typeof(int));
        dtrecords.Columns.Add("IssueDetailId", typeof(int));
        dtrecords.Columns.Add("Qty", typeof(int));
        dtrecords.Columns.Add("MapStencilNo", typeof(int));
        dtrecords.Columns.Add("OrderId", typeof(int));
        //**************
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtqty = ((TextBox)DG.Rows[i].FindControl("txtqty"));
            if (Chkboxitem.Checked == true && (txtqty.Text != "" && txtqty.Text != "0"))
            {
                Label lblitemfinishedid = ((Label)DG.Rows[i].FindControl("lblitemfinishedid"));
                Label lblIssueOrderID = ((Label)DG.Rows[i].FindControl("lblIssueOrderID"));
                Label lblIssueId = ((Label)DG.Rows[i].FindControl("lblIssueId"));
                Label lblIssueDetailId = ((Label)DG.Rows[i].FindControl("lblIssueDetailId"));
                Label lblMapStencilNo = ((Label)DG.Rows[i].FindControl("lblMapStencilNo"));
                Label lblorderid = ((Label)DG.Rows[i].FindControl("lblorderid"));

                //********Data Row
                DataRow dr = dtrecords.NewRow();
                dr["Itemfinishedid"] = lblitemfinishedid.Text;
                dr["IssueOrderId"] = lblIssueOrderID.Text;
                dr["IssueId"] = lblIssueId.Text;
                dr["IssueDetailId"] = lblIssueDetailId.Text;
                dr["Qty"] = txtqty.Text;
                dr["MapStencilNo"] = lblMapStencilNo.Text;
                dr["OrderId"] = lblorderid.Text;
                dtrecords.Rows.Add(dr);
            }
        }
        if (dtrecords.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {

                //******
                SqlCommand cmd = new SqlCommand("PRO_SAVEMAPTRACENO_RECEIVEONPRODUCTIONORDER", con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 3000;
                cmd.Parameters.Add("@Recid", SqlDbType.Int);
                cmd.Parameters["@Recid"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@Recid"].Value = hnRecid.Value;
                cmd.Parameters.AddWithValue("@Companyid", DDcompany.SelectedValue);
                cmd.Parameters.AddWithValue("@Receivedate", txtReceivedate.Text);
                cmd.Parameters.AddWithValue("@MapStencilType", DDMapStencilType.SelectedValue);
                cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
                cmd.Parameters.AddWithValue("@Mastercompanyid", Session["varcompanyid"]);
                cmd.Parameters.AddWithValue("@dtrecords", dtrecords);
                cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@MSStockno", txtMapStencilstockno.Text);
                cmd.Parameters.Add("@ChallanNo", SqlDbType.VarChar,30);
                cmd.Parameters["@ChallanNo"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@ChallanNo"].Value = "";
                cmd.ExecuteNonQuery();
                if (cmd.Parameters["@msg"].Value.ToString() != "") //IF DATA NOT SAVED
                {
                    lblmessage.Text = cmd.Parameters["@msg"].Value.ToString();
                    Tran.Rollback();
                }
                else
                {
                    lblmessage.Text = "Data Saved Successfully.";
                    Tran.Commit();
                    //txtfoliono.Text = cmd.Parameters["@FolioNo"].Value.ToString(); //param[5].Value.ToString();
                    hnRecid.Value = cmd.Parameters["@Recid"].Value.ToString();// param[0].Value.ToString();
                    txtReceiveNo.Text = cmd.Parameters["@ChallanNo"].Value.ToString();
                    FillissueGrid();
                }
                //******

            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblmessage.Text = ex.Message;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
        }
    }

    protected void FillissueGrid()
    {
        string str = @"select MIM.RecId,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,
                        CASE WHEN MSSN.UnitID = 2 THEN VF.SIZEFT ELSE VF.SIZEMTR END As Size,
                        MID.RecDetailId,MID.IssueOrderId,MID.ItemFinishedID,MIM.CompanyId,MIM.MapStencilType,MIM.ChallanNo,
                        Replace(CONVERT(nvarchar(11),MIM.ReceiveDate,106),' ','-') as ReceiveDate,
                        1 as Qty,MSSN.MSStockNo,MSSN.MapStencilNo,MID.IssueId,MID.IssueDetailId,MID.IssueOrderId
                        from Map_ReceiveOnProductionOrderMaster MIM INNER JOIN Map_ReceiveOnProductionOrderDetail MID ON MIM.RecId=MID.RecId 
                        INNER JOIN V_FinishedItemDetail VF ON MID.ItemFinishedId=VF.Item_Finished_Id
                        INNER JOIN OrderDetail OD ON MID.OrderId=OD.OrderID and MID.ItemFinishedId=OD.Item_Finished_id
                        INNER JOIN Map_StencilStockNo_Detail MSSND ON MIM.RecId=MSSND.ReceiveID and MID.RecDetailId=MSSND.ReceiveDetailId
                        INNER JOIN Map_StencilStockNo MSSN ON MSSND.MapStencilNo=MSSN.MapStencilNo
                        where MIM.CompanyId=" + DDcompany.SelectedValue + " ";
        //where MIM.ID=" + hnissueid.Value;
        if (txtChallanNo.Text != "")
        {
            str = str + " and MIM.ChallanNo='" + txtChallanNo.Text + "'";
        }
        else
        {
            str = str + " and MIM.RecID=" + hnRecid.Value + "";
        }
        str = str + " Union";
        str = str + @" select MIM.RecId,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,
                        CASE WHEN MSSN.UnitID = 2 THEN VF.SIZEFT ELSE VF.SIZEMTR END As Size,
                        MID.RecDetailId,MID.IssueOrderId,MID.ItemFinishedID,MIM.CompanyId,MIM.MapStencilType,MIM.ChallanNo,
                        Replace(CONVERT(nvarchar(11),MIM.ReceiveDate,106),' ','-') as ReceiveDate,
                        1 as Qty,MSSN.MSStockNo,MSSN.MapStencilNo,MID.IssueId,MID.IssueDetailId,MID.IssueOrderId
                        from Map_ReceiveOnProductionOrderMaster MIM INNER JOIN Map_ReceiveOnProductionOrderDetail MID ON MIM.RecId=MID.RecId 
                        INNER JOIN V_FinishedItemDetail VF ON MID.ItemFinishedId=VF.Item_Finished_Id                       
                        INNER JOIN Map_StencilStockNo_Detail MSSND ON MIM.RecId=MSSND.ReceiveID and MID.RecDetailId=MSSND.ReceiveDetailId
                        INNER JOIN Map_StencilStockNo MSSN ON MSSND.MapStencilNo=MSSN.MapStencilNo and MSSN.Type=0
                        where MIM.CompanyId=" + DDcompany.SelectedValue + " ";
        if (txtChallanNo.Text != "")
        {
            str = str + " and MIM.ChallanNo='" + txtChallanNo.Text + "'";
        }
        else
        {
            str = str + " and MIM.RecID=" + hnRecid.Value + "";
        }


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();

        if (chkEdit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDMapStencilType.SelectedValue = ds.Tables[0].Rows[0]["MapStencilType"].ToString();
                //DDcompany.SelectedValue = ds.Tables[0].Rows[0]["CompanyID"].ToString();
                txtReceiveNo.Text = ds.Tables[0].Rows[0]["ChallanNo"].ToString();
                hnRecid.Value = ds.Tables[0].Rows[0]["RecId"].ToString();
            }
        }
    }
    protected void txtChallanNo_TextChanged(object sender, EventArgs e)
    {
        FillissueGrid();
    }
    protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow && gvdetail.EditIndex == e.Row.RowIndex)
        //{
        //    DropDownList DDMapType = (DropDownList)e.Row.FindControl("DDMapType");

        //    string str = @"select ID,MapType from MapType order by MapType";

        //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //    UtilityModule.ConditionalComboFillWithDS(ref DDMapType, ds, 0, true, "--Plz Select--");

        //    string selectedMapIssueType = DataBinder.Eval(e.Row.DataItem, "MapIssueType").ToString();
        //    DDMapType.Items.FindByValue(selectedMapIssueType).Selected = true;

        //}
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
            //Label lblhqty = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblhqty");
            Label lblRecId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblRecId");
            Label lblRecDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblRecDetailId");
            Label lblMapStencilNo = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblMapStencilNo");
            Label lblIssueId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblIssueId");
            Label lblIssueDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblIssueDetailId");
            Label lblIssueOrderId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblIssueOrderId");

            Label lblQty = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblQty");
            Label lblItemFinishedId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblItemFinishedId");

            SqlParameter[] param = new SqlParameter[9];
            param[0] = new SqlParameter("@RecId", lblRecId.Text);
            param[1] = new SqlParameter("@RecDetailid", lblRecDetailId.Text);
            param[2] = new SqlParameter("@IssueId", lblIssueId.Text);
            param[3] = new SqlParameter("@IssueDetailid", lblIssueDetailId.Text);
            param[4] = new SqlParameter("@IssueOrderid", lblIssueOrderId.Text);
            param[5] = new SqlParameter("@Qty", lblQty.Text);
            param[6] = new SqlParameter("@ItemFinishedId", lblItemFinishedId.Text);
            param[7] = new SqlParameter("@MapStencilNo", lblMapStencilNo.Text);
            param[8] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[8].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEMAPRECEIVE_ONPRODUCTIONORDER", param);
            lblmessage.Text = param[8].Value.ToString();
            Tran.Commit();
            FillissueGrid();
            //***************
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@CompanyId", DDcompany.SelectedValue);
        param[1] = new SqlParameter("@RecID", hnRecid.Value);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_WeaverMapReceiveReport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptWeaverMapReceive.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptWeaverMapReceive.xsd";
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
}