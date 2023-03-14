using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Purchase_FrmPurchaseRevisedDate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            if (Session["varCompanyId"].ToString() == "7")
            {
                if (Session["varuserid"].ToString() == "1" || Session["varuserid"].ToString() == "12")
                {
                    BtnSave.Visible = true;
                }
                else
                {
                    BtnSave.Visible = false;
                }
            }
            UtilityModule.ConditionalComboFill(ref ddCatagory, "Select Distinct CATEGORY_ID,CATEGORY_NAME from CategorySeparate CS,ITEM_CATEGORY_MASTER IM ,UserRights_Category UC  Where IM.Category_Id=UC.Categoryid And UC.UserId=" + Session["varuserid"] + " And IM.Category_Id=CS.CategoryId And IM.MasterCompanyId=" + Session["varCompanyId"] + "  Order by CATEGORY_NAME", true, "--SELECT--");
            if (ddCatagory.Items.Count > 0)
            {
                ddCatagory.SelectedIndex = 1;
                fillemp();
                FillGrid();
            }
        }
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillemp();
        FillGrid();
    }
    private void fillemp()
    {
        UtilityModule.ConditionalComboFill(ref DdEmp, "select Distinct empid,empname from PurchaseIndentIssue pii left outer join Ordermaster om On pii.orderid=om.orderid inner join Orderdetail od On od.orderid=om.orderid inner join V_FinishedItemDetail v On v.item_finished_id=od.Item_Finished_Id inner join Empinfo e On e.empid=pii.partyid Where om.status=0 and v.CATEGORY_ID=" + ddCatagory.SelectedValue + " And V.MasterCompanyId=" + Session["varCompanyId"] + " order by empname", true, "-Select-");
    }
    private void FillGrid()
    {
        string str = "";
        str = @"select Distinct om.LocalOrder+' '+CustomerOrderNo OrderNo,PindentIssueid,replace(convert(varchar(11),pii.duedate,106),' ','-')as duedate,pii.status from PurchaseIndentIssue pii left outer join 
              Ordermaster om On pii.orderid=om.orderid inner join Orderdetail od On od.orderid=om.orderid inner join 
              V_FinishedItemDetail v On v.item_finished_id=od.Item_Finished_Id inner join UserRights_Category uc On uc.CategoryId=v.CATEGORY_ID 
              Where om.status=0 and pii.status='Pending' and v.CATEGORY_ID=" + ddCatagory.SelectedValue + " And V.MasterCompanyId=" + Session["varCompanyId"];
        if (DdEmp.SelectedIndex > 0)
        {
            str=str+" and pii.partyid=" + DdEmp.SelectedValue + "";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGOrderDetail.DataSource = ds;
        DGOrderDetail.DataBind();
    }
    protected void DGOrderDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    public string getDate(string strVal)
    {
        string val = "";
        hnremark.Value = "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Replace(convert(varchar(11),date,106),' ','-') as date,Remark from PurchaseTracking where TableName='PurchaseIndentIssue' and ptrackid=" + strVal + " order by ptrackingid desc");
        if(ds.Tables[0].Rows.Count>0)
        {
            val = ds.Tables[0].Rows[0]["date"].ToString();
            hnremark.Value = ds.Tables[0].Rows[0]["Remark"].ToString();
        }
        return val;
    }
    public string getRemark(string strVal)
    {
        string val = "";
        val = hnremark.Value;
        return val;
    }
    protected void DDEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            SqlParameter[] _arrpara = new SqlParameter[4];
            _arrpara[0] = new SqlParameter("@Userid", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@date", SqlDbType.DateTime);
            _arrpara[2] = new SqlParameter("@Remark", SqlDbType.NVarChar,250);
            _arrpara[3] = new SqlParameter("@PTrackId", SqlDbType.Int);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
            {
                string Date = Convert.ToString(((TextBox)DGOrderDetail.Rows[i].FindControl("Txtreviseddate")).Text);
                if (((CheckBox)DGOrderDetail.Rows[i].FindControl("Chkbox")).Checked == true )
                {
                    if (Date != "")
                    {
                        _arrpara[0].Value = Session["varuserid"];
                        _arrpara[1].Value = Convert.ToString(((TextBox)DGOrderDetail.Rows[i].FindControl("Txtreviseddate")).Text);
                        _arrpara[2].Value = Convert.ToString(((TextBox)DGOrderDetail.Rows[i].FindControl("TxtRemark")).Text);
                        _arrpara[3].Value = Convert.ToString(((Label)DGOrderDetail.Rows[i].FindControl("lblpono")).Text);
                        SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "PRO_PurchaseTracking", _arrpara);
                    }
                    if (((CheckBox)DGOrderDetail.Rows[i].FindControl("Chkpend")).Checked == true)
                    {
                        string pono = Convert.ToString(((Label)DGOrderDetail.Rows[i].FindControl("lblpono")).Text);
                        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update purchaseindentissue set status='Complete' where PindentIssueid="+pono+"");
                    }
                }
            }
            tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Data Saves Successfully!');", true);
            FillGrid();
        }
        catch (Exception ex)
        {

        }
    }
   
    protected void DGOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderDetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGOrderDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Show")
        {
            string resourcedatabaseid = e.CommandArgument.ToString();
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select CATEGORY_NAME +'  '+ITEM_NAME+'  '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName as description,
            Sum(quantity) as Qty,V.ITEM_FINISHED_ID,pii.PindentIssueid from PurchaseIndentIssue pii inner join PurchaseIndentIssueTran pit On pii.PindentIssueid=pit.PindentIssueid inner join 
            V_FinishedItemDetail V On V.ITEM_FINISHED_ID=pit.Finishedid where pii.PindentIssueid=" + resourcedatabaseid + " And V.MasterCompanyId=" + Session["varCompanyId"] + " group by CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,V.ITEM_FINISHED_ID,pii.PindentIssueid");
            GridView1.DataSource = ds;
            GridView1.DataBind();
        }
    }
   
    public string getgiven(string Strval, string Strval1)
    {
        string val = "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select isnull(sum(QTY),0) from PurchaseReceiveDetail where PindentIssueid="+Strval+" and finishedid="+Strval1+"");
        val=ds.Tables[0].Rows[0][0].ToString();
        hnrecqty.Value = val;
        return val;
    }
    public string getpending(string Strval)
    {
        string val = "";
        val = Convert.ToString(Convert.ToDouble(Strval) - Convert.ToDouble(hnrecqty.Value));
        return val;
    }
    public string getlastDate(string Strval)
    {
        string val = "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select replace(convert(varchar(11),RemarkCurrentDate,106),' ','-') as date from Purchasetracking where ptrackid=" + Strval + " order by date desc");
        if (ds.Tables[0].Rows.Count > 0)
        {
            val = ds.Tables[0].Rows[0][0].ToString();
        }
        return val;
    }
}