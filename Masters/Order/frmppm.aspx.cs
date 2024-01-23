using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
public partial class Masters_Order_frmppm : System.Web.UI.Page
{
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
                  select CustomerId,customercode+'  '+companyname from customerinfo where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @"  order by CustomerCode
                  select PROCESS_NAME_ID,PROCESS_NAME From Process_Name_Master where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + "  order by PROCESS_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDCustomer, ds, 1, true, "--Select Buyer--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 2, true, "--Select Process--");

            ds.Dispose();
            txtstartdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtendate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDOrderNo, "select OM.OrderId,OM.LocalOrder+'/'+OM.CustomerOrderNo as CustomerOrderNo from ordermaster OM Where OM.CustomerId=" + DDCustomer.SelectedValue + " and OM.CompanyId=" + DDCompanyName.SelectedValue + " and OM.status=0 order by OM.OrderId", true, "--Select Order No.--");
    }
    protected void fillOrderDetail()
    {
        string str;
        str = @"select distinct vf.ITEM_NAME+'  '+vf.QualityName+'  '+vf.designName+'  '+vf.ColorName
                +'  '+vf.ShadeColorName+'  '+vf.ShapeName+'  '+case when od.flagsize=1 Then vf.SizeMtr Else case When Od.flagsize=0 Then vf.SizeFt
                Else case When od.flagsize=2 then vf.SizeInch else '' End End End as ItemDescription,
                Od.item_finished_id,od.OrderDetailId,od.QtyRequired,Replace(convert(nvarchar(11),Om.orderdate,106),' ','-') as Orderdate,Replace(convert(nvarchar(11),Om.dispatchdate,106),' ','-') as  DispatchDate,
                PPM.orderdetailid as PPMOrderdetailid   from OrderMaster OM inner join
                OrderDetail OD on om.OrderId=od.OrderId inner join V_FinishedItemDetail vf on od.Item_Finished_Id=vf.ITEM_FINISHED_ID
                left join PPMItemDetail PPM on PPM.orderdetailid=od.OrderDetailId Where Od.orderid=" + DDOrderNo.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GVOrderDetail.DataSource = ds;
        GVOrderDetail.DataBind();
        ds.Dispose();

    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillOrderDetail();
        //********PPMNO
        UtilityModule.ConditionalComboFill(ref DDPPMNo, "select PPMID,PPMID as PPMNo From PPMMaster Where orderid=" + DDOrderNo.SelectedValue + " order by PPMID", true, "--Please select--");
        if (DDPPMNo.Items.Count > 0)
        {
            TDPPMNO.Visible = true;
        }
        else
        {
            TDPPMNO.Visible = false;
        }

        //*******
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {

        //*****Get PPM Item Details
        lblmsg.Text = "";
        DataTable dtppm = new DataTable();
        dtppm.Columns.Add("Orderdetailid", typeof(int));

        for (int i = 0; i < GVOrderDetail.Rows.Count; i++)
        {
            CheckBox ChkItem = (CheckBox)GVOrderDetail.Rows[i].FindControl("ChkItem");
            if (ChkItem.Checked == true)
            {
                Label lblorderdetailid = (Label)GVOrderDetail.Rows[i].FindControl("lblorderdetailid");
                DataRow dr = dtppm.NewRow();
                dr["orderdetailid"] = lblorderdetailid.Text;
                dtppm.Rows.Add(dr);
            }

        }
        //**********
        if (dtppm.Rows.Count > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[20];
                param[0] = new SqlParameter("@PPMID", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = hnppmid.Value;
                param[1] = new SqlParameter("@CompanyId", DDCompanyName.SelectedValue);
                param[2] = new SqlParameter("@orderid", DDOrderNo.SelectedValue);
                param[3] = new SqlParameter("@userid", Session["varuserid"]);
                param[4] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
                param[5] = new SqlParameter("@Processid", DDProcessName.SelectedValue);
                param[6] = new SqlParameter("@Requirements", txtrequirements.Text);
                param[7] = new SqlParameter("@StartDate", txtstartdate.Text);
                param[8] = new SqlParameter("@Enddate", txtendate.Text);
                param[9] = new SqlParameter("@Ncpdetails", txtncpdetails.Text);
                param[10] = new SqlParameter("@Comments", txtcomments.Text);
                param[11] = new SqlParameter("@dtppm", dtppm);
                param[12] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[12].Direction = ParameterDirection.Output;
                param[13] = new SqlParameter("@Fincomdt", txtfincomdt.Text == "" ? DBNull.Value : (object)txtfincomdt.Text);
                //*******************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEPPM", param);
                Tran.Commit();
                hnppmid.Value = param[0].Value.ToString();
                if (param[12].Value.ToString() != "")
                {
                    lblmsg.Text = param[12].Value.ToString();
                }
                else
                {
                    lblmsg.Text = "DATA SAVED SUCCESSFULLY...";
                    DDProcessName.Focus();
                    GVOrderDetail.Enabled = false;
                    clearcontrols();
                }
                FillPPMdetails();
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select atleast one Checkbox to save Data!!!')", true);
        }
        //******
    }
    protected void clearcontrols()
    {
        DDProcessName.SelectedIndex = -1;
        txtrequirements.Text = "";
        txtncpdetails.Text = "";
        txtcomments.Text = "";

    }
    protected void GVOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblRawtnaorderdetailid = ((Label)e.Row.FindControl("lblPPMorderdetailid"));
            CheckBox ChkItem = (CheckBox)e.Row.FindControl("ChkItem");
            if (lblRawtnaorderdetailid.Text != "")
            {
                e.Row.BackColor = Color.LightGreen;
                ChkItem.Enabled = false;
            }
        }
    }
    protected void DDPPMNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        //uncheck all checknbox
        for (int j = 0; j < GVOrderDetail.Rows.Count; j++)
        {
            Label lblRawtnaorderdetailid = ((Label)GVOrderDetail.Rows[j].FindControl("lblPPMorderdetailid"));
            CheckBox ChkItem = (CheckBox)GVOrderDetail.Rows[j].FindControl("ChkItem");
            ChkItem.Checked = false;
            if (lblRawtnaorderdetailid.Text != "")
            {
                GVOrderDetail.Rows[j].BackColor = Color.LightGreen;
                ChkItem.Enabled = false;
            }
        }
        //

        hnppmid.Value = DDPPMNo.SelectedValue;
        string str = "select Orderdetailid From PPMItemdetail Where PPMid=" + DDPPMNo.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < GVOrderDetail.Rows.Count; j++)
                {
                    Label lblorderdetailid = (Label)GVOrderDetail.Rows[j].FindControl("lblorderdetailid");
                    if (ds.Tables[0].Rows[i]["orderdetailid"].ToString() == lblorderdetailid.Text)
                    {
                        CheckBox ChkItem = (CheckBox)GVOrderDetail.Rows[j].FindControl("ChkItem");
                        ChkItem.Checked = true;
                        ChkItem.Enabled = true;
                        break;
                    }
                }
            }
        }
        //**********Fill grid
        FillPPMdetails();
    }
    protected void FillPPMdetails()
    {
        string str = @"select PPM.PPMID as PPMNo,pnm.process_name,ppd.Requirements,REPLACE(CONVERT(nvarchar(11),ppd.startdate,106),' ','-') as startdate,
                    REPLACE(CONVERT(nvarchar(11),ppd.enddate,106),' ','-') as enddate,REPLACE(CONVERT(nvarchar(11),ppd.fincomdate,106),' ','-') as fincomdate,ppd.NcpDetails,ppd.Comments,PPD.detailid
                    From PPMMaster PPM inner Join PPMDetail PPD on PPM.PPMID=PPD.PPMID
                    inner join PROCESS_NAME_MASTER PNM on ppd.processid=pnm.process_name_id Where PPM.PPMId=" + hnppmid.Value + " order by Detailid";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GVPPMDETAIL.DataSource = ds.Tables[0];
        GVPPMDETAIL.DataBind();
    }
    protected void GVPPMDETAIL_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
            Label lbldetailid = (Label)GVPPMDETAIL.Rows[e.RowIndex].FindControl("lblppmdetailid");
            Label lblprocess = (Label)GVPPMDETAIL.Rows[e.RowIndex].FindControl("lblprocess");
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@Detailid", lbldetailid.Text);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@Process_name", lblprocess.Text);
            //**********            
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEPPMITEM", param);
            Tran.Commit();
            lblmsg.Text = param[3].Value.ToString();
            FillPPMdetails();
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
    protected void GVPPMDETAIL_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GVPPMDETAIL.EditIndex = e.NewEditIndex;
        FillPPMdetails();
    }
    protected void GVPPMDETAIL_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GVPPMDETAIL.EditIndex = -1;
        FillPPMdetails();
    }
    protected void GVPPMDETAIL_RowUpdating(object sender, GridViewUpdateEventArgs e)
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
            Label lblppmdetailid = (Label)GVPPMDETAIL.Rows[e.RowIndex].FindControl("lblppmdetailid");
            TextBox txtrequirementsedit = (TextBox)GVPPMDETAIL.Rows[e.RowIndex].FindControl("txtrequirementsedit");
            TextBox txtstartdateedit = (TextBox)GVPPMDETAIL.Rows[e.RowIndex].FindControl("txtstartdateedit");
            TextBox txtenddateedit = (TextBox)GVPPMDETAIL.Rows[e.RowIndex].FindControl("txtenddateedit");
            TextBox txtncpdetailsedit = (TextBox)GVPPMDETAIL.Rows[e.RowIndex].FindControl("txtncpdetailsedit");
            TextBox txtcommentsedit = (TextBox)GVPPMDETAIL.Rows[e.RowIndex].FindControl("txtcommentsedit");
            TextBox txtfincomdtgrid = (TextBox)GVPPMDETAIL.Rows[e.RowIndex].FindControl("txtfincomdtgrid");
            //************
            SqlParameter[] param = new SqlParameter[10];
            param[0] = new SqlParameter("@Detailid", lblppmdetailid.Text);
            param[1] = new SqlParameter("@requirements", txtrequirementsedit.Text);
            param[2] = new SqlParameter("@startdate", txtstartdateedit.Text);
            param[3] = new SqlParameter("@enddate", txtenddateedit.Text);
            param[4] = new SqlParameter("@ncpdetails", txtncpdetailsedit.Text);
            param[5] = new SqlParameter("@comments", txtcommentsedit.Text);
            param[6] = new SqlParameter("@userid", Session["varuserid"]);
            param[7] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[8] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[8].Direction = ParameterDirection.Output;
            param[9] = new SqlParameter("@Fincomdt", txtfincomdtgrid.Text == "" ? DBNull.Value : (object)txtfincomdtgrid.Text);
            //***********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEPPMDETAIL", param);
            Tran.Commit();
            lblmsg.Text = param[8].Value.ToString();
            GVPPMDETAIL.EditIndex = -1;
            FillPPMdetails();
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
    protected void GVPPMDETAIL_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    for (int i = 0; i < GVPPMDETAIL.Columns.Count; i++)
        //    {
        //        if (Session["varMasterCompanyIDForERP"].ToString() == "16")
        //        {
        //            if (GVPPMDETAIL.Columns[i].HeaderText.ToUpper() == "REQUIREMENTS" || GVPPMDETAIL.Columns[i].HeaderText.ToUpper() == "NCP DETAILS IF ANY" || GVPPMDETAIL.Columns[i].HeaderText.ToUpper() == "COMMENTS")
        //            {
        //                GVPPMDETAIL.Columns[i].Visible = false;
        //            }
        //        }
        //    }
        //}
    }
}