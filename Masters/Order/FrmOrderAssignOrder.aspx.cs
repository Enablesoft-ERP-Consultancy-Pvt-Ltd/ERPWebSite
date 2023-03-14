using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Order_FrmOrderAssignOrder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varuserid"].ToString() == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            CommanFunction.FillCombo(DDCompany, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + " order by CompanyName");


            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                CompanySelectedIndentChange();
            }
            fillOrderto();
        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedIndentChange();
    }
    private void CompanySelectedIndentChange()
    {
        UtilityModule.ConditionalComboFill(ref DDcustomer, " Select customerid,customercode from customerinfo Where MasterCompanyId=" + Session["varCompanyId"] + " Order by customercode", true, "ALL");

        if (DDcustomer.Items.Count > 0)
        {
            DDcustomer.SelectedIndex = 1;
            fillOrder();
        }
    }
    private void fillOrder()
    {
        UtilityModule.ConditionalComboFill(ref DDOrder, @"select Distinct om.OrderId,om.LocalOrder+ ' / ' +om.CustomerOrderNo  from OrderMaster om inner join orderdetail od On om.orderid=od.orderid inner join 
        V_FinishedItemDetail v On od.Item_Finished_Id= v.Item_Finished_Id inner join UserRights_Category uc On v.CATEGORY_ID=uc.CategoryId 
        Where  CustomerId=" + DDcustomer.SelectedValue + @" and uc.userid=" + Session["varuserid"] + " And V.MasterCompanyId=" + Session["varCompanyId"] + "", true, "-Select-");
    }
    private void fillOrderto()
    {
        UtilityModule.ConditionalComboFill(ref DDOrderto, @"select Distinct om.OrderId,om.LocalOrder+ ' / ' +om.CustomerOrderNo  from OrderMaster om inner join orderdetail od On om.orderid=od.orderid inner join 
        V_FinishedItemDetail v On od.Item_Finished_Id= v.Item_Finished_Id inner join UserRights_Category uc On v.CATEGORY_ID=uc.CategoryId  
        Where om.status=0  and uc.userid=" + Session["varuserid"] + " And V.MasterCompanyId=" + Session["varCompanyId"] + "", true, "-Select-");
    }
    protected void DDcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillOrder();
    }
    protected void DDOrderto_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    private void FillGrid()
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select CATEGORY_NAME as caterory,ITEM_NAME as item,QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName as description ,isnull(sum(oc.Qty),0) as ordercon,isnull(sum(oao.qty),0) as assignqty,v.ITEM_FINISHED_ID AS FINISHEDID
        From OrderLocalConsumption oc inner join V_FinishedItemDetail v  On oc.Finishedid=v.ITEM_FINISHED_ID left Outer join 
        OrderAssignQtyOrder oao On oao.toorderid=oc.orderid and oao.Item_Finished_id=v.ITEM_FINISHED_ID 
        Where orderid=" + DDOrderto.SelectedValue + " And V.MasterCompanyId=" + Session["varCompanyId"] + " Group by CATEGORY_NAME ,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,v.ITEM_FINISHED_ID");
        if (ds.Tables[0].Rows.Count > 0)
        {
            GVOrderStockAssign.DataSource = ds;
            GVOrderStockAssign.DataBind();
        }
    }
    public string getActualStock(string Strval)
    {
        string val = "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select isnull(sum(qty),0) from PurchaseReceiveDetail prd inner join PurchaseIndentIssue pii On prd.PindentIssueid=pii.PindentIssueid where orderid=" + DDOrder.SelectedValue + " and finishedid=" + Strval + "");
        DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select isnull(sum(qty),0) from OrderAssignQtyOrder where FromOrderid=" + DDOrder.SelectedValue + " and Item_Finished_id=" + Strval + " And MasterCompanyId=" + Session["varCompanyId"] + "");
        val = Convert.ToString(Convert.ToDouble(ds.Tables[0].Rows[0][0].ToString()) - (0 + Convert.ToDouble(ds2.Tables[0].Rows[0][0].ToString())));
        return val;
    }
    public string getAssignedqty(string Strval)
    {
        string val = "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select isnull(sum(Qty),0) from OrderAssignQtyOrder where Item_Finished_id=" + Strval + " and FromOrderid <>" + DDOrder.SelectedValue + " and ToOrderid=" + DDOrderto.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "");
        val = Convert.ToString(Convert.ToDouble(ds.Tables[0].Rows[0][0].ToString()));
        return val;
    }
    public string getActualAssigned(string Strval)
    {
        string val = "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select isnull(sum(Qty),0) from OrderAssignQtyOrder where Item_Finished_id=" + Strval + " and FromOrderid =" + DDOrder.SelectedValue + " and ToOrderid=" + DDOrderto.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "");
        val = Convert.ToString(Convert.ToDouble(ds.Tables[0].Rows[0][0].ToString()));
        return val;
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (LblMessage.Text == "")
            {
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                SqlParameter[] _arrpara = new SqlParameter[8];
                _arrpara[0] = new SqlParameter("@Companyid", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@Date", SqlDbType.SmallDateTime);
                _arrpara[2] = new SqlParameter("@FromOrderid", SqlDbType.Int);
                _arrpara[3] = new SqlParameter("@ToOrderid", SqlDbType.Int);
                _arrpara[4] = new SqlParameter("@Item_Finished_id", SqlDbType.Int);
                _arrpara[5] = new SqlParameter("@Qty", SqlDbType.Float);
                _arrpara[6] = new SqlParameter("@Userid", SqlDbType.Int);
                _arrpara[7] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                _arrpara[0].Value = DDCompany.SelectedValue;
                _arrpara[1].Value = DateTime.Now.Date.ToString();
                _arrpara[2].Value = DDOrder.SelectedValue;
                _arrpara[3].Value = DDOrderto.SelectedValue;
                _arrpara[6].Value = Session["varuserid"];
                _arrpara[7].Value = Session["varcompanyId"];
                for (int i = 0; i < GVOrderStockAssign.Rows.Count; i++)
                {
                    if (((CheckBox)GVOrderStockAssign.Rows[i].FindControl("Chkbox")).Checked == true)
                    {
                        if (((TextBox)GVOrderStockAssign.Rows[i].FindControl("TxtAssignqty")).Text == "")
                        {
                            ((TextBox)GVOrderStockAssign.Rows[i].FindControl("TxtAssignqty")).Text = "0";
                        }
                        _arrpara[4].Value = Convert.ToInt32(((Label)GVOrderStockAssign.Rows[i].FindControl("lblfinished")).Text);
                        _arrpara[5].Value = Convert.ToDouble(((TextBox)GVOrderStockAssign.Rows[i].FindControl("TxtAssignqty")).Text); ;
                        SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "PRO_OrderAssignQtyOrderl", _arrpara);
                    }
                }
                tran.Commit();
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Data Saved Successfully');", true);
                FillGrid();
            }
        }
        catch (Exception ex)
        {
            LblMessage.Text = ex.Message;
            LblMessage.Visible = true;
        }
    }
    protected void txtqnt1_changed(object sender, EventArgs e)
    {
        int RowIndex = ((sender as TextBox).NamingContainer as GridViewRow).RowIndex;
        if (((TextBox)GVOrderStockAssign.Rows[RowIndex].FindControl("TxtAssignqty")).Text == "")
        {
            ((TextBox)GVOrderStockAssign.Rows[RowIndex].FindControl("TxtAssignqty")).Text = "0";
        }
        int text1 = Convert.ToInt32(((TextBox)GVOrderStockAssign.Rows[RowIndex].FindControl("TxtAssignqty")).Text);
        int lblordercon = Convert.ToInt32(((Label)GVOrderStockAssign.Rows[RowIndex].FindControl("lblOrdercon")).Text);
        int lblActualStock = Convert.ToInt32(((Label)GVOrderStockAssign.Rows[RowIndex].FindControl("lblActualstock")).Text);
        int lblOtherAssigned = Convert.ToInt32(((Label)GVOrderStockAssign.Rows[RowIndex].FindControl("lblAssignedqty")).Text);
        if (text1 <= lblActualStock)
        {
            if (lblordercon >= (text1 + lblOtherAssigned))
            {
                LblMessage.Text = "";
                LblMessage.Visible = false;
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Assign Qty is Greater then Consumption Qty');", true);
                LblMessage.Text = "Assign Qty is Greater then Consumption Qty";
                LblMessage.Visible = true;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Assign Qty is Greater Then Actual Stock Qty');", true);
            LblMessage.Text = "Assign Qty is Greater Then Actual Stock Qty";
            LblMessage.Visible = true;
        }
    }
}