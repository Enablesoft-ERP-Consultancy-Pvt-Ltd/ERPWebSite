using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Packing_FrmAddCollection : System.Web.UI.Page
{
    public static int Collectionid = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["VarCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref ddcustomercode, "SELECT customerid,Customercode+' '+CompanyName from customerinfo Where MasterCompanyId=" + Session["varCompanyId"] + " order by customercode", true, "--SELECT--");
            UtilityModule.ConditonalChkBoxListFill(ref chlDesign, "Select designId,designName from design where MasterCompanyId=" + Session["varCompanyId"] + " order by designName");
        }
    }
    protected void refresh_form()
    {
        ddcustomercode.SelectedValue = "0";
        txttypeofcollection.Text = "";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {

            SqlParameter[] _arrPara = new SqlParameter[6];
            _arrPara[0] = new SqlParameter("@Collectionid", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@Customerid", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@CollectionName", SqlDbType.VarChar, 50);
            _arrPara[3] = new SqlParameter("@Designid", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@userid", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@mastercompanyid", SqlDbType.Int);


            _arrPara[0].Direction = ParameterDirection.InputOutput;
            _arrPara[0].Value = Collectionid;
            _arrPara[1].Value = ddcustomercode.SelectedValue;
            _arrPara[2].Value = txttypeofcollection.Text.ToUpper();
            //string str = "Select designId,designName from design where MasterCompanyId=" + Session["varCompanyId"] + "";

            //SqlHelper.ExecuteNonQuery(con, CommandType.Text, str);
            int n = chlDesign.Items.Count;
            string str = null;
            for (int i = 0; i < n; i++)
            {
                if (chlDesign.Items[i].Selected)
                {
                    str = str == null ? chlDesign.Items[i].Value : str + "," + chlDesign.Items[i].Value;
                }
            }
            _arrPara[3].Value = chlDesign.Items.Count;
            _arrPara[4].Value = Session["varuserid"].ToString();
            _arrPara[5].Value = Session["varcompanyid"].ToString();
            Collectionid = Convert.ToInt16(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select isnull(Max(CollectionId),0)+1  from collection"));
            //Delete existing record
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "delete from collection where collectionname='" + _arrPara[2].Value + "' and customerid=" + _arrPara[1].Value + "");
            string str1 = "Insert into collection (Collectionid,Customerid,CollectionName,Designid,userid,mastercompanyid)  Select Distinct  " + Collectionid + "," + ddcustomercode.SelectedValue + ",'" + txttypeofcollection.Text + "',*," + Session["varuserid"] + "," + Session["varCompanyId"] + " from Split('" + str + @"',',')";
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);

            Tran.Commit();
            lblMessage.Visible = true;
            lblMessage.Text = "Data Saved Successfully";
            refresh_form();
            UtilityModule.ConditonalChkBoxListFill(ref chlDesign, "Select designId,designName from design where MasterCompanyId=" + Session["varCompanyId"] + " order by designName");
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblMessage.Visible = true;
            UtilityModule.MessageAlert(ex.Message, "Masters_Packing_FrmAddCollection.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (con != null)
            {
                con.Dispose();
            }
        }

    }




    protected void ddcustomercode_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillGrid();
    }
    protected void fillGrid()
    {
        string str = @"select CollectionId,CollectionName,D.designId,D.designName,CI.customerId,CI.CustomerCode+' '+CI.CompanyName as Customer from collection  CC inner Join Design D on CC.Designid=D.designId
                     inner join Customerinfo CI on CI.CustomerId=CC.Customerid Where Ci.CustomerId=" + ddcustomercode.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        DGCollection.DataSource = ds;
        DGCollection.DataBind();

        ds.Dispose();
    }
    protected void DGCollection_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            string Collection = ((Label)DGCollection.Rows[e.RowIndex].FindControl("lblCollectionId")).Text;
            string Designid = ((Label)DGCollection.Rows[e.RowIndex].FindControl("lblDesignId")).Text;
            string CustomerId = ((Label)DGCollection.Rows[e.RowIndex].FindControl("lblCustomerId")).Text;

            string str = "Delete From Collection Where CollectionId=" + Collection + " And DesignId=" + Designid + " And CustomerId=" + CustomerId + "";
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);

            Tran.Commit();
            lblMessage.Visible = true;
            lblMessage.Text = "Data Deleted successfully....";

            fillGrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;

        }
        finally
        {
            con.Dispose();
            con.Close();
        }

    }
}