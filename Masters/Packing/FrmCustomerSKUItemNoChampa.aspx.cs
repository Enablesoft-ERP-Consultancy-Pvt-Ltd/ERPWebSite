using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Packing_FrmCustomerSKUItemNoChampa : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, true, "--Select--");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
            FillCustomercode();
        }
    }
    protected void FillCustomercode()
    {
        string str = @"select Distinct CU.Customerid,Cu.customercode from OrderDetail OD  
                       inner join  OrderMaster OM on  OM.OrderId=OD.OrderId
                       inner Join Customerinfo cu on om.customerid=cu.customerid
                       Where OM.CompanyId=" + DDCompany.SelectedValue + " and Status=0 order by customercode";

        UtilityModule.ConditionalComboFill(ref DDCustomerCode, str, true, "--Plz Select--");
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCustomercode();
        DG.DataSource = null;
        DG.DataBind();
    }
    protected void DDType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDCompany.SelectedIndex > 0 && DDCustomerCode.SelectedIndex > 0)
        {
            Fillgrid();
        }
        else
        {
            DG.DataSource = null;
            DG.DataBind();
        }
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDCompany.SelectedIndex > 0 && DDCustomerCode.SelectedIndex > 0)
        {
            Fillgrid();
        }
        else
        {
            DG.DataSource = null;
            DG.DataBind();
        }
    }
    protected void Fillgrid()
    {
        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@Type", DDType.SelectedValue);
        param[2] = new SqlParameter("@CustomerId", DDCustomerCode.SelectedValue);
        param[3] = new SqlParameter("@MasterCompanyId", Session["VarCompanyId"]);
        param[4] = new SqlParameter("@UserId", Session["VarUserid"]);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_FillCustomerSKUItemNo", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DG.DataSource = ds.Tables[0];
            DG.DataBind();
        }
        else
        {
            DG.DataSource = null;
            DG.DataBind();
        }

    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ////for (int i = 0; i < DG.Columns.Count; i++)
            ////{
            ////    if (variable.VarBINNOWISE == "1")
            ////    {
            ////        if (DG.Columns[i].HeaderText == "BinNo")
            ////        {
            ////            DG.Columns[i].Visible = true;
            ////        }
            ////    }
            ////    else
            ////    {
            ////        if (DG.Columns[i].HeaderText == "BinNo")
            ////        {
            ////            DG.Columns[i].Visible = false;
            ////        }
            ////    }
            ////}
            //DropDownList DDMapType = ((DropDownList)e.Row.FindControl("DDMapType"));
            //string str = @"select ID,MapType from MapType order by MapType";

            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            //UtilityModule.ConditionalComboFillWithDS(ref DDMapType, ds, 0, true, "--Plz Select--");           
        }
    }
    private void CHECKVALIDCONTROL()
    {
        lblmessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompany) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerCode) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        lblmessage.Visible = true;
        UtilityModule.SHOWMSG(lblmessage);
    B: ;
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (lblmessage.Text == "")
        {
            string Strdetail = "";
            for (int i = 0; i < DG.Rows.Count; i++)
            {
                Label lblItemFinishedId = ((Label)DG.Rows[i].FindControl("lblItemFinishedId"));
                TextBox txtHDCItemNo = ((TextBox)DG.Rows[i].FindControl("txtHDCItemNo"));
                TextBox txtProductCode = ((TextBox)DG.Rows[i].FindControl("txtProductCode"));
                TextBox txtGrossWt = ((TextBox)DG.Rows[i].FindControl("txtGrossWt"));
                TextBox txtNetWt = ((TextBox)DG.Rows[i].FindControl("txtNetWt"));
                TextBox txtPcs = ((TextBox)DG.Rows[i].FindControl("txtPcs"));
                TextBox txtCBM = ((TextBox)DG.Rows[i].FindControl("txtCBM"));
                TextBox txtMaterialDescription = ((TextBox)DG.Rows[i].FindControl("txtMaterialDescription"));
                TextBox txtBaleDimension = ((TextBox)DG.Rows[i].FindControl("txtBaleDimension"));

                if (txtHDCItemNo.Text.Trim() != "" || txtProductCode.Text.Trim() != "" || txtGrossWt.Text != "0" || txtNetWt.Text != "0" || txtPcs.Text != "0" || txtCBM.Text != "0" || txtMaterialDescription.Text.Trim() != "" || txtBaleDimension.Text.Trim() != "")
                {
                    Strdetail = Strdetail + lblItemFinishedId.Text + '|' + txtHDCItemNo.Text + '|' + txtProductCode.Text + '|' + txtGrossWt.Text + '|' + txtNetWt.Text + '|' + txtPcs.Text + '|' + txtCBM.Text + '|' + txtMaterialDescription.Text + '|' + txtBaleDimension.Text + '~';
                }
            }

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@CompanyID", DDCompany.SelectedValue);
                param[1] = new SqlParameter("@Type", DDType.SelectedValue);
                param[2] = new SqlParameter("@CustomerId", DDCustomerCode.SelectedValue);
                param[3] = new SqlParameter("@UserID", Session["varuserid"]);
                param[4] = new SqlParameter("@MasterCompanyID", Session["varcompanyid"]);
                param[5] = new SqlParameter("@StringDetail", Strdetail);
                param[6] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                param[6].Direction = ParameterDirection.Output;

                ///**********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveCustomerSKUItemNo", param);
                //*******************               

                lblmessage.Text = param[6].Value.ToString();
                Tran.Commit();
                Fillgrid();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblmessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }

}