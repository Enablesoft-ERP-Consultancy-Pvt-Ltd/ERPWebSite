using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


public partial class Masters_Campany_frmDefineConeType : System.Web.UI.Page
{
    DropDownList dddesign = new DropDownList();
    DropDownList ddcolor = new DropDownList();
    DropDownList ddshape = new DropDownList();
    DropDownList ddsize = new DropDownList();
    TextBox TxtProdCode = new TextBox();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        dddesign.Visible = false;
        ddcolor.Visible = false;
        ddshape.Visible = false;
        ddsize.Visible = false;
        TxtProdCode.Text = "";
        if (!IsPostBack)
        {

            // UtilityModule.ConditionalComboFill(ref DDQuality, "select Q.QualityId,Q.QualityName from Quality Q,Item_Master IM Where  IM.Item_id=Q.Item_id And Im.Category_Id=2 And Q.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  order by QualityName", true, "--Plz Select Quality--");
            UtilityModule.ConditionalComboFill(ref DDItem, @"select Distinct Item_id,Item_name  from Item_Master IM,Item_Category_master ICM,categoryseparate CS 
                                                            Where Im.Category_Id=ICM.Category_Id And ICM.Category_Id=CS.CategoryId And Cs.Id=1 And IM.MastercompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Plz Select Item--");

            //UtilityModule.ConditionalComboFill(ref DDGodown, "select Godownid,GodownName from GodownMaster Order by GodownName", true, "--Plz Select Godown--");

            // if (DDGodown.Items.Count > 0)
            // {
            //     DDGodown.SelectedIndex = 1;
            // }
            Fill_Grid();
        }
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
            SqlParameter[] _array = new SqlParameter[10];
            _array[0] = new SqlParameter("@Id", SqlDbType.Int);
            _array[1] = new SqlParameter("@ConeType", SqlDbType.VarChar, 100);
            _array[2] = new SqlParameter("@Item_Finished_id", SqlDbType.Int);
            _array[3] = new SqlParameter("@Qty", SqlDbType.Float);
            _array[4] = new SqlParameter("@GodownId", SqlDbType.Int);
            _array[5] = new SqlParameter("@lotNo", SqlDbType.VarChar, 100);
            _array[6] = new SqlParameter("@Userid", SqlDbType.Int);
            _array[7] = new SqlParameter("@MasterCompanyid", SqlDbType.Int);
            _array[8] = new SqlParameter("@msg", SqlDbType.VarChar, 50);

            _array[0].Direction = ParameterDirection.InputOutput;
            _array[0].Value = 0;
            _array[1].Value = txtConeType.Text.ToUpper();
            int Varfinishedid = UtilityModule.getItemFinishedId(DDItem, DDQuality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, Tran, DDColours, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            _array[2].Value = Varfinishedid;
            _array[3].Value = txtQty.Text;
            _array[4].Value = 1;// DDGodown.SelectedValue fix
            _array[5].Value = DDLotNo.SelectedIndex <= 0 ? "" : DDLotNo.SelectedItem.Text;
            _array[6].Value = Session["varuserid"];
            _array[7].Value = Session["varMasterCompanyIDForERP"];
            _array[8].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveConeType", _array);
            Tran.Commit();
            refreshControl();
            lblmessage.Text = _array[8].Value.ToString();
            Fill_Grid();
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
    protected void Fill_Grid()
    {
        string str = @"select ID,ConeType ConeType,vf.Item_Name+' / '+QualityName+' / '+Vf.ShadeColorName As Item_Description ,Lotno As LotNo,Qty 
                       from ConeType CT,V_FinishedItemDetail Vf  Where VF.Item_Finished_id=CT.Item_Finished_id Order by ConeType";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGConeType.DataSource = ds;
        DGConeType.DataBind();


    }
    protected void DGConeType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGConeType, "Select$" + e.Row.RowIndex);


        }
    }
    protected void refreshControl()
    {
        txtConeType.Text = "";
        txtQty.Text = "";
        //txtLotNo.Text = "";
    }
    protected void DGConeType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGConeType.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void DDItem_SelectedIndexChanged(object sender, EventArgs e)
    {
         UtilityModule.ConditionalComboFill(ref DDQuality, "select Q.QualityId,Q.QualityName from Quality Q,Item_Master IM Where  IM.Item_id=Q.Item_id And Im.Item_id=" + DDItem.SelectedValue +" And Q.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  order by QualityName", true, "--Plz Select Quality--");
                //SqlhelperEnum.FillDropDown(AllEnums.MasterTables.Quality, DDQuality, pWhere: "Item_Id=" + DDItem.SelectedValue + "", pID: "QualityId", pName: "QualityName", pFillBlank: true, Selecttext: "--Plz Select Quality--");
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
         UtilityModule.ConditionalComboFill(ref DDColours, "select ShadeColorId,ShadeColorName  from ShadeColor Order by ShadeColorName", true, "--Plz Select COlours--");
        //SqlhelperEnum.FillDropDown(AllEnums.MasterTables.ShadeColor, DDColours, pID: "ShadecolorId", pFillBlank: true, pName: "ShadeColorname", Selecttext: "--Plz Select Colours--");

    }
    protected void DDColours_SelectedIndexChanged(object sender, EventArgs e)
    {
       // FIll_LotNo();
    }
    protected void FIll_LotNo()
    {

        int Varfinishedid = UtilityModule.getItemFinishedId(DDItem, DDQuality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, DDColours, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));

        UtilityModule.ConditionalComboFill(ref DDLotNo, "select LotNo,LotNo from Stock Where Item_Finished_id=" + Varfinishedid + @" And CompanyId=1 And Godownid=1
                                                        And Qtyinhand>0 Order by StockId", true, "--Plz Select Lot No.");


    }
}