using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using ClosedXML.Excel;

public partial class Masters_Packing_FrmStockNoOutByInvoicePackingNew : System.Web.UI.Page
{
    private const string SCRIPT_DOFOCUS =
 @"window.setTimeout('DoFocus()', 1);
            function DoFocus()
            {
                try {
                    document.getElementById('REQUEST_LASTFOCUS').focus();
                } catch (ex) {}
            }";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            HookOnFocus(this.Page as Control);

            //replaces REQUEST_LASTFOCUS in SCRIPT_DOFOCUS with the posted value from Request["__LASTFOCUS"]
            //and registers the script to start after Update panel was rendered
            ScriptManager.RegisterStartupScript(
                this,
                typeof(Masters_Packing_FrmStockNoOutByInvoicePackingNew),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);

            logo();
           
            string Qry = @" select Distinct CI.CompanyId,CI.Companyname From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by Companyname
                    Select CustomerId,CustomerCode + SPACE(5)+CompanyName From CustomerInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by CustomerCode";
            DataSet ds1 = null;
            ds1 = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds1, 0, true, "--SELECT--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds1, 1, true, "--SELECT--");
            CustomerCodeSelectedIndexChange();
           
            ViewState["PACKINGDETAILID"] = 0;          


        }
    }
    private void logo()
    {
        LblCompanyName.Text = Session["varCompanyName"].ToString();
        LblUserName.Text = Session["varusername"].ToString();
    }   
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerCodeSelectedIndexChange();       
    }
    private void CustomerCodeSelectedIndexChange()
    {
       
        ////UtilityModule.ConditionalComboFill(ref ddInvoiceNo, "Select PackingID,TPackingNo+' / '+Replace(Convert(VarChar(11),PackingDate,106), ' ','-') PackingNo from Packing Where ConsignorId=" + DDCompanyName.SelectedValue + " And ConsigneeId=" + DDCustomerCode.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--SELECT--");

        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_BindInvoiceNoForStockOut", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@CompanyId", DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@CustomerId", DDCustomerCode.SelectedValue);           
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
            cmd.Parameters.AddWithValue("@UserId", Session["varuserId"]);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

                UtilityModule.ConditionalComboFillWithDS(ref ddInvoiceNo, ds, 0, true, "--Plz Select--");
            }
            else
            {
                ddInvoiceNo.Items.Clear();
            }
        }
        catch (Exception)
        {
        }
        finally
        {

        }     
   
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "You Are Successfully LoggedOut..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }   
    protected void BtnSave_Click(object sender, EventArgs e)
    {
    #region
        CHECKVALIDCONTROL();
        if (LblErrorMessage.Text == "")
        {
            string Strdetail = "";

            ////for (int i = 0; i < DGStock.Rows.Count; i++)
            ////{
            ////    CheckBox Chkboxitem = ((CheckBox)DGStock.Rows[i].FindControl("Chkboxitem"));
            ////    Label lblTStockNo = ((Label)DGStock.Rows[i].FindControl("lblTStockNo"));
            ////    Label lblStockNo = ((Label)DGStock.Rows[i].FindControl("lblStockNo"));
            ////    Label lblItemFinishedId = ((Label)DGStock.Rows[i].FindControl("lblItemFinishedId"));

            ////    if (Chkboxitem.Checked == true && (lblStockNo.Text != "") && DDCustomerCode.SelectedIndex > 0 && ddInvoiceNo.SelectedIndex > 0 && DDCustomerOrderNo.SelectedIndex > 0)
            ////    {
            ////        Strdetail = Strdetail + lblTStockNo.Text + '|' + lblStockNo.Text + '|' + lblItemFinishedId.Text + '~';
            ////    }
            ////}

            //if (Strdetail != "")
            //{
            //    LblErrorMessage.Text = "";

            //    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            //    if (con.State == ConnectionState.Closed)
            //    {
            //        con.Open();
            //    }
            //    SqlTransaction Tran = con.BeginTransaction();
            //    try
            //    {                    
            //        SqlCommand cmd = new SqlCommand("Pro_SaveStockOutCarpetByPackingInvoice", con, Tran);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandTimeout = 3000;
            //        cmd.Parameters.AddWithValue("@CompanyId", DDCompanyName.SelectedValue);
            //        cmd.Parameters.AddWithValue("@CustomerId", DDCustomerCode.SelectedValue);
            //        cmd.Parameters.AddWithValue("@PackingId",ddInvoiceNo.SelectedValue);
            //        //cmd.Parameters.AddWithValue("@OrderId", DDCustomerOrderNo.SelectedValue);
            //        cmd.Parameters.AddWithValue("@StringDetail", Strdetail);
            //        cmd.Parameters.AddWithValue("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            //        cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
            //        cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 300);
            //        cmd.Parameters["@Msg"].Direction = ParameterDirection.Output; 

            //        cmd.ExecuteNonQuery();
            //        if (cmd.Parameters["@Msg"].Value.ToString() != "") //IF DATA NOT SAVED
            //        {
            //            LblErrorMessage.Visible = true;
            //            LblErrorMessage.Text = cmd.Parameters["@Msg"].Value.ToString();
            //            Tran.Rollback();
            //        }
            //        else
            //        {
            //            BindCarpetNumber();
            //            LblErrorMessage.Visible = true;
            //            LblErrorMessage.Text = "Data Saved Successfully.";
            //            Tran.Commit();
                        
            //        }


            //    }
            //    catch (Exception ex)
            //    {
            //        LblErrorMessage.Text = ex.Message;
            //        Tran.Rollback();
            //    }
            //    finally
            //    {
            //        con.Dispose();
            //        con.Close();
            //    }





            // }


        }
    #endregion

    }
    private void CHECKVALIDCONTROL()
    {
        LblErrorMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompanyName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerCode) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddInvoiceNo) == false)
        {
            goto a;
        }        
       
        else
        {
            goto B;
        } 
    a:
        LblErrorMessage.Visible = true;
        UtilityModule.SHOWMSG(LblErrorMessage);
    B: ;
    }
    private void Save_Referce()
    {
        TxtStockNo.Text = "";
        hnsampletype.Value = "1";
    }
    protected void ddInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {

        //FillorderNo();
    }
    public string getStockNo(string strVal, string strval1)
    {
        string val = "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select strcarpets From [dbo].[GetTStockNosPack](" + strVal + ",0)");
        val = ds.Tables[0].Rows[0]["strcarpets"].ToString();
        return val;
    }
    private void HookOnFocus(Control CurrentControl)
    {
        //checks if control is one of TextBox, DropDownList, ListBox or Button
        if ((CurrentControl is TextBox) ||
            (CurrentControl is DropDownList) ||
            (CurrentControl is ListBox) ||
            (CurrentControl is Button))
            //adds a script which saves active control on receiving focus in the hidden field __LASTFOCUS.
            (CurrentControl as WebControl).Attributes.Add(
                "onfocus",
                "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");

        //checks if the control has children
        if (CurrentControl.HasControls())
            //if yes do them all recursively
            foreach (Control CurrentChildControl in CurrentControl.Controls)
                HookOnFocus(CurrentChildControl);
    }
    protected void TxtStockNo_TextChanged(object sender, EventArgs e)
    {
         CHECKVALIDCONTROL();
         if (LblErrorMessage.Text == "")
         {
             if (DDCustomerCode.SelectedIndex > 0 && ddInvoiceNo.SelectedIndex > 0 && TxtStockNo.Text != "")
             {

                 SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                 if (con.State == ConnectionState.Closed)
                 {
                     con.Open();
                 }
                 SqlTransaction Tran = con.BeginTransaction();
                 try
                 {
                     SqlCommand cmd = new SqlCommand("Pro_SaveStockOutCarpetByPackingInvoiceNew", con, Tran);
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.CommandTimeout = 3000;
                     cmd.Parameters.AddWithValue("@CompanyId", DDCompanyName.SelectedValue);
                     cmd.Parameters.AddWithValue("@CustomerId", DDCustomerCode.SelectedValue);
                     cmd.Parameters.AddWithValue("@PackingId", ddInvoiceNo.SelectedValue);
                     //cmd.Parameters.AddWithValue("@OrderId", DDCustomerOrderNo.SelectedValue);
                     cmd.Parameters.AddWithValue("@Tstockno", TxtStockNo.Text);
                     cmd.Parameters.AddWithValue("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
                     cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
                     cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 300);
                     cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;

                     cmd.ExecuteNonQuery();
                     if (cmd.Parameters["@Msg"].Value.ToString() != "") //IF DATA NOT SAVED
                     {
                         LblErrorMessage.Visible = true;
                         LblErrorMessage.Text = cmd.Parameters["@Msg"].Value.ToString();
                         Tran.Rollback();
                     }
                     else
                     {
                         //BindCarpetNumber();
                         LblErrorMessage.Visible = true;
                         LblErrorMessage.Text = "Data Saved Successfully.";
                         Tran.Commit();
                     }

                 }
                 catch (Exception ex)
                 {
                     LblErrorMessage.Text = ex.Message;
                     Tran.Rollback();
                 }
                 finally
                 {
                     con.Dispose();
                     con.Close();
                 }

             }
             else
             {                
                 LblErrorMessage.Visible = true;
                 LblErrorMessage.Text = "Please select Invoice no, customer orderno";
             }
         }      
        
    }
}