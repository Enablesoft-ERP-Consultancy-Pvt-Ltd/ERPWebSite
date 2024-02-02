using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Packing_frmPreshipmentdocument : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CI.CompanyId,CI.CompanyName from companyinfo CI inner join Company_Authentication CA on CI.CompanyId=CA.CompanyId and CA.UserId=" + Session["varuserid"] + " and CI.mastercompanyid=" + Session["varMasterCompanyIDForERP"] + @"
                          select CustomerId,CustomerCode+'/'+CompanyName as customer from customerinfo where mastercompanyid=" + Session["varMasterCompanyIDForERP"] + @"   order by customer
                          select PaymentId,PaymentName from Payment where mastercompanyid=" + Session["varMasterCompanyIDForERP"] + " order by PaymentName";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "Select");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDBuyer, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDPaymentmode, ds, 2, true, "--Plz Select--");
            txtshipdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDBuyer_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDOrderNo, "select OrderId,CustomerOrderNo from OrderMaster where CompanyId=" + DDCompanyName.SelectedValue + " and CustomerId=" + DDBuyer.SelectedValue + "  order by CustomerOrderNo", true, "--Plz Select--");
        hnid.Value = "0";
    }
    protected void FillProductdescription()
    {
        string str = @"select OM.OrderId,OD.Item_Finished_Id,OD.flagsize,Replace(CONVERT(nvarchar(11),OM.OrderDate,106),' ','-') as OrderDate,
                        dbo.F_getItemDescription(od.Item_Finished_Id,od.flagsize) as ProductDescription,ISNULL(Vp.shipedqty,0) as shipedqty,OD.qtyrequired
                        from OrderMaster OM inner join OrderDetail OD 
                        on OM.OrderId=OD.OrderId
                        left join V_orderWisePreshipment VP on Om.Orderid=VP.orderid and OD.Item_Finished_Id=Vp.item_finished_id Where OM.orderid=" + DDOrderNo.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GvDetail.DataSource = ds.Tables[0];
        GvDetail.DataBind();
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillProductdescription();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        // sql Data Table
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("Customerid", typeof(int));
        dtrecords.Columns.Add("Orderid", typeof(int));
        dtrecords.Columns.Add("Item_finished_id", typeof(int));
        dtrecords.Columns.Add("flagsize", typeof(int));
        dtrecords.Columns.Add("qty", typeof(int));
        dtrecords.Columns.Add("Noofbales", typeof(int));
        dtrecords.Columns.Add("Amount", typeof(double));
        dtrecords.Columns.Add("qtyrequired", typeof(int));
        //********************************************
        for (int i = 0; i < GvDetail.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)GvDetail.Rows[i].FindControl("Chkboxitem"));
            TextBox txtnoofbales = ((TextBox)GvDetail.Rows[i].FindControl("txtnoofbales"));
            TextBox txtqty = ((TextBox)GvDetail.Rows[i].FindControl("txtqty"));
            if (Chkboxitem.Checked == true && txtnoofbales.Text != "" && txtqty.Text != "")
            {
                Label lblorderid = ((Label)GvDetail.Rows[i].FindControl("lblorderid"));
                Label lblitemfinishedid = ((Label)GvDetail.Rows[i].FindControl("lblitemfinishedid"));
                Label lblflagsize = ((Label)GvDetail.Rows[i].FindControl("lblflagsize"));
                Label lblorderqty = ((Label)GvDetail.Rows[i].FindControl("lblorderqty"));
                TextBox txtamount = ((TextBox)GvDetail.Rows[i].FindControl("txtamount"));
                //********Data Row
                DataRow dr = dtrecords.NewRow();
                dr["customerid"] = DDBuyer.SelectedValue;
                dr["orderid"] = lblorderid.Text;
                dr["Item_finished_id"] = lblitemfinishedid.Text;
                dr["flagsize"] = lblflagsize.Text;
                dr["qty"] = txtqty.Text;
                dr["Noofbales"] = txtnoofbales.Text;
                dr["amount"] = txtamount.Text == "" ? "0" : txtamount.Text;
                dr["qtyrequired"] = lblorderqty.Text;
                //*************
                dtrecords.Rows.Add(dr);
            }
        }
        //**********
        if (dtrecords.Rows.Count > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                //**************
                SqlParameter[] param = new SqlParameter[11];
                param[0] = new SqlParameter("@ID", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = hnid.Value == "" ? "0" : hnid.Value;
                param[1] = new SqlParameter("@companyid", DDCompanyName.SelectedValue);
                param[2] = new SqlParameter("@ShipmentID", SqlDbType.VarChar, 50);
                param[2].Direction = ParameterDirection.InputOutput;
                param[2].Value = txtshipid.Text;
                param[3] = new SqlParameter("@ShipmentDate", txtshipdate.Text);
                param[4] = new SqlParameter("@Paymentid", DDPaymentmode.SelectedValue);
                param[5] = new SqlParameter("@Billoflading", txtbilloflading.Text);
                param[6] = new SqlParameter("@Totalweight", txttotalwt.Text == "" ? "0" : txttotalwt.Text);
                param[7] = new SqlParameter("@dtrecords", dtrecords);
                param[8] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[8].Direction = ParameterDirection.Output;
                param[9] = new SqlParameter("@userid", Session["varuserid"]);
                param[10] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);

                //**************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_savePreshipment", param);
                hnid.Value = param[0].Value.ToString();
                //lblmsg.Text = param[8].Value.ToString();
                txtshipid.Text = param[2].Value.ToString();
                //if Data not saved
                if (param[8].Value.ToString() != "")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Erralt", "alert('" + param[8].Value.ToString() + "');", true);
                    lblmsg.Text = param[8].Value.ToString();
                    Tran.Commit();
                    return;
                }
                //
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "savealt", "alert('Data saved successfully...');", true);
                    lblmsg.Text = "Data saved successfully....";
                }
                Tran.Commit();
                FillProductdescription();
                DDOrderNo.SelectedIndex = -1;
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
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = "select * from v_preshipmentreport where id=" + hnid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptfilename"] = "~\\Reports\\rptpreshipment.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptpreshipment.xsd";
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