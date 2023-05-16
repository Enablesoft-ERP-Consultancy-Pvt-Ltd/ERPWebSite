using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;
using System.Text;
using ClosedXML.Excel;
using System.IO;

public partial class Masters_ReportForms_frmOrderWiseDyeingConsumption : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = "Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varCompanyId"] + @" Order by Companyname 
                          select CustomerId,CustomerCode+' / '+CompanyName as Customer from customerinfo Where mastercompanyid=" + Session["varCompanyId"] + " order by customer";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, true, "-ALL-");
            UtilityModule.ConditionalComboFillWithDS(ref DDbuyer, ds, 1, true, "-Select-");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;                
            }

            fillgrid();
           
            //*******

        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {

        UtilityModule.ConditionalComboFill(ref DDbuyer, "Select CustomerId,CustomerCode+' / '+CompanyName From CustomerInfo Where MasterCompanyId=" + Session["varCompanyId"] + " Order By CustomerCode", true, "--Select--");
    }
    private void fillgrid()
    {
        string str = "";
        string qry = "";        
        if (DDbuyer.SelectedIndex > 0)
        {
            qry = qry + " and OM.customerid=" + DDbuyer.SelectedValue + " ";
        }

        str = @"select distinct LocalOrder+'/'+ CustomerOrderNo as OrderNo,om.orderid,om.Remarks as Remark 
                   From OrderMaster om  inner join orderdetail od On om.orderid=od.orderid inner join 
                   V_FinishedItemDetail v On od.Item_Finished_Id= v.Item_Finished_Id 
                   Where  om.Status=0 And V.MasterCompanyId=" + Session["varCompanyId"] + " " + qry + " ";
        str = str + " order by orderNo ";


        if (str != "")
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            {
                DGOrderDetail.DataSource = ds;
                DGOrderDetail.DataBind();
            }
        }

    }
    protected void DGOrderDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        string status = "";
        string DetailData = "";
        string remark = "";
        //sql Table Type
        DataTable dt = new DataTable();
        dt.Columns.Add("Remark", typeof(string));
        dt.Columns.Add("Orderid", typeof(int));

        for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DGOrderDetail.Rows[i].FindControl("Chkbox"));
            Label lblOrderId = ((Label)DGOrderDetail.Rows[i].FindControl("lblOrderId"));
            Label lblOrderNo = ((Label)DGOrderDetail.Rows[i].FindControl("lblOrderNo"));

            if (Chkboxitem.Checked == true)   // Change when Updated Completed
            {
                status = "1";

                if (DetailData == "")
                {
                    DetailData = lblOrderId.Text + "|" + lblOrderNo.Text + "~";
                }
                else
                {
                    DetailData = DetailData + lblOrderId.Text + "|" + lblOrderNo.Text + "~";
                }

            }

            //if (((CheckBox)DGOrderDetail.Rows[i].FindControl("Chkbox")).Checked == true)
            //{
            //    remark = ((TextBox)DGOrderDetail.Rows[i].FindControl("txtRemark")).Text;
            //    DataRow dr = dt.NewRow();
            //    dr["remark"] = remark;
            //    dr["orderid"] = DGOrderDetail.DataKeys[i].Value;
            //    dt.Rows.Add(dr);
            //}
        }

        if (status == "" || DetailData == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check boxes');", true);
            return;
        }

        if (DetailData!="")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
                param[1] = new SqlParameter("@Customerid", DDbuyer.SelectedValue);
                param[2] = new SqlParameter("@DetailData", DetailData);
                param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[3].Direction = ParameterDirection.Output;
                param[4] = new SqlParameter("@userid", Session["varuserid"]);
                param[5] = new SqlParameter("@Mastercompanyid", Session["varcompanyNo"]);                
                //****
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetOrderWiseDyingConsumptionReport", param);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var xapp = new XLWorkbook();
                    var sht = xapp.Worksheets.Add("DyeingConsumptionDetail_");

                    sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                    sht.PageSetup.AdjustTo(83);
                    sht.PageSetup.PaperSize = XLPaperSize.A4Paper;


                    sht.PageSetup.Margins.Top = 1.21;
                    sht.PageSetup.Margins.Left = 0.47;
                    sht.PageSetup.Margins.Right = 0.36;
                    sht.PageSetup.Margins.Bottom = 0.19;
                    sht.PageSetup.Margins.Header = 1.20;
                    sht.PageSetup.Margins.Footer = 0.3;
                    sht.PageSetup.SetScaleHFWithDocument();


                    //Export to excel
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;

                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition",
                     "attachment;filename=DyeingConsumptionDetailOrderWise" + DateTime.Now + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter hw = new HtmlTextWriter(sw);

                    int columncount = GridView1.Rows[0].Cells.Count;

                    ////Change the Header Row back to white color
                    //GridView1.HeaderRow.Style.Add("background-color", "#FFFFFF");
                    ////Applying stlye to gridview header cells

                    if (GridView1.Rows.Count > 0)
                    {

                        string StrHeaderText = "";
                        for (int i = 0; i < GridView1.HeaderRow.Cells.Count; i++)
                        {
                            if (GridView1.HeaderRow.Cells[i].Text == "FinishedId" || GridView1.HeaderRow.Cells[i].Text == "PK1")
                            {
                                StrHeaderText = GridView1.HeaderRow.Cells[i].Text;

                                GridView1.HeaderRow.Cells[i].Visible = false;
                                //GridView1.Columns[i].Visible = false;
                            }
                        }

                        for (int i = 0; i < GridView1.Rows.Count; i++)
                        {
                            GridView1.Rows[i].Cells[0].Visible = false;
                            GridView1.Rows[i].Cells[columncount - 1].Visible = false;
                           
                            
                        }
                       
                    }
                    //GridView1.Caption = "OrderWise Consumption Detail";
                    GridView1.RenderControl(hw);

                    //style to format numbers to string
                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();   
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No data fetched.')", true);
                }
                //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_GetOrderWiseDyingConsumptionReport", param);
                //Tran.Commit();
                //ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + param[1].Value.ToString() + "');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('" + ex.Message + "');", true);
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            #region
          
            #endregion
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Nodata", "alert('Please select atleast one checkbox')", true);
        }
        //SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update ordermaster set status=" + ddststatuschange.SelectedValue+ " where orderid in(" + CustOrderNo + ")");

        
    }
    protected void DDbuyer_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }
    
    protected void DDweaver_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }    
   
}