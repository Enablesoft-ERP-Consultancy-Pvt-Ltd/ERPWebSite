using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Loom_frmLoomMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = "select CI.CompanyId,CompanyName From CompanyInfo CI inner Join Company_Authentication CA on CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + @" order by CompanyName
                          select Unitsid,Unitname from Units order by unitname
                          select shapeid,Shapename from shape order by shapeid
                          select val,Type from sizetype";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDunitname, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDshape, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 3, false, "");

            UtilityModule.ConditionalComboFill(ref DDSupervisorName, "select SupervisorId,SupervisorName from SupervisorMaster", true, "--Select Supervisor--");

            switch (Convert.ToInt16(Session["varcompanyId"]))
            {
                case 22: //for Diamond Export
                    TDSuperVisorName.Visible = true;                    
                    break;                
                default:
                    TDSuperVisorName.Visible = false;                      
                    break;
            }  
        }
    }
    protected void DDunitname_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDitem, @"select Distinct Im.ITEM_ID,IM.item_name from Item_Master IM inner join CategorySeparate cs 
                                          on im.CATEGORY_ID=Cs.Categoryid and cs.id=0 order by Item_name", true, "--Plz Select--");
        FillGrid();

    }
    protected void DDshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void FillSize()
    {
        string size;
        switch (Convert.ToInt16(DDsizetype.SelectedValue))
        {
            case 0:
                size = "sizeft";
                break;
            case 1:
                size = "sizemtr";
                break;
            case 2:
                size = "sizeinch";
                break;
            default:
                size = "sizeft";
                break;
        }
        UtilityModule.ConditionalComboFill(ref DDSize, "select Distinct Sizeid," + size + " from size Where shapeid=" + DDshape.SelectedValue + " order by " +size+"", true, "--Plz Select--");

    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //********************
            SqlParameter[] param = new SqlParameter[12];
            param[0] = new SqlParameter("@UID", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hnuid.Value == "" ? "0" : hnuid.Value;
            param[1] = new SqlParameter("@companyId", DDcompany.SelectedValue);
            param[2] = new SqlParameter("@Unitid", DDunitname.SelectedValue);
            param[3] = new SqlParameter("@itemId", DDitem.SelectedValue);
            param[4] = new SqlParameter("@shapeid", DDshape.SelectedValue);
            param[5] = new SqlParameter("@Sizeid", DDSize.SelectedValue);
            param[6] = new SqlParameter("@flagsize", DDsizetype.SelectedValue);
            param[7] = new SqlParameter("@LoomNo", txtLoomNo.Text);
            param[8] = new SqlParameter("@LoomType", DDLoomType.SelectedItem.Text);
            param[9] = new SqlParameter("@userid", Session["varuserid"]);
            param[10] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[10].Direction = ParameterDirection.Output;
            param[11] = new SqlParameter("@SupervisorId", DDSupervisorName.SelectedIndex != -1 ? DDSupervisorName.SelectedValue : "0");
            //**********************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveProductionLoomMaster", param);
            Tran.Commit();
            lblmsg.Text = param[10].Value.ToString();
            FillGrid();
            Refreshcontrol();
            Enablecontrol();
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
    protected void Refreshcontrol()
    {
        txtLoomNo.Text = "";
        DDSupervisorName.SelectedIndex = -1;     
        hnuid.Value = "0";
        BtnSave.Text = "Save";
    }
    protected void FillGrid()
    {
        string str = @"select PM.UnitName,IM.ITEM_NAME,
                        case When Pl.flagsize=0 Then S.SizeFt When Pl.flagsize=1 Then S.SizeMtr When Pl.flagsize=2 Then S.SizeInch
                        Else S.SizeFt End as Size,PL.LoomNo,PL.LoomType,Pl.UID,companyid,PL.unitid,PL.itemid,PL.shapeid,PL.sizeid,PL.flagsize,isnull(SVM.SupervisorName,'') as SupervisorName,
                        isnull(SVM.SupervisorId,0) as SupervisorId,Case When PL.EnableDisableStatus=1 Then 'Disable' Else 'Enable' ENd as Status,
                        isnull(PL.EnableDisableStatus,0) as EnableDisableStatus
                        from ProductionLoomMaster PL Inner join Units PM
                        on PL.UnitId=PM.Unitsid                                    
                        inner join shape sh on PL.ShapeId=sh.ShapeId           
                        inner join Size	 S on Pl.Sizeid=S.SizeId              
                        inner join SizeType sz on Pl.flagsize=Sz.Val           
                        inner join ITEM_MASTER IM on PL.Itemid=IM.ITEM_ID
                        left join SupervisorMaster	SVM on PL.SupervisorId=SVM.SupervisorId";
        str = str + " Where PL.CompanyId=" + DDcompany.SelectedValue + " and PL.unitid=" + DDunitname.SelectedValue;

        if (DDitem.SelectedIndex > 0)
        {
            str = str + " ANd  PL.ItemId=" + DDitem.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " And  PL.Sizeid=" + DDSize.SelectedValue;
        }
        str = str + " Order by PL.UID";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
    }

    protected void DDitem_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void DDSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";           

            for (int i = 0; i < DG.Columns.Count; i++)
            {
                if (Session["varcompanyId"].ToString() == "22")
                {
                    if (DG.Columns[i].HeaderText == "Supervisor Name")
                    {
                        DG.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (DG.Columns[i].HeaderText == "Supervisor Name")
                    {
                        DG.Columns[i].Visible = false;
                    }
                }
            }

            Label lblLoomMasterEnable_Disable = (Label)e.Row.FindControl("lblLoomMasterEnable_Disable");
            if (lblLoomMasterEnable_Disable.Text == "0")
            {
                e.Row.BackColor = System.Drawing.Color.Red;
            }
        }
    }
    protected void lbEdit_Click(object sender, EventArgs e)
    {
        Disablecontrol();
        LinkButton lb = (LinkButton)sender;
        GridViewRow grv = (GridViewRow)lb.NamingContainer;
        hnuid.Value = ((Label)DG.Rows[grv.RowIndex].FindControl("lbluid")).Text;
        //string companyid = ((Label)DG.Rows[grv.RowIndex].FindControl("lblcompanyid")).Text;
        string unitid = ((Label)DG.Rows[grv.RowIndex].FindControl("lblunitid")).Text;
        string itemid = ((Label)DG.Rows[grv.RowIndex].FindControl("lblitemid")).Text;
        string shapeid = ((Label)DG.Rows[grv.RowIndex].FindControl("lblshapeid")).Text;
        string sizeid = ((Label)DG.Rows[grv.RowIndex].FindControl("lblsizeid")).Text;
        string sizetype = ((Label)DG.Rows[grv.RowIndex].FindControl("lblsizetype")).Text;
        string LoomNo = ((Label)DG.Rows[grv.RowIndex].FindControl("lblLoomNo")).Text;
        string Loomtype = ((Label)DG.Rows[grv.RowIndex].FindControl("lblLoomType")).Text;
        string lblSupervisorName = ((Label)DG.Rows[grv.RowIndex].FindControl("lblSupervisorName")).Text;
        string lblSupervisorId = ((Label)DG.Rows[grv.RowIndex].FindControl("lblSupervisorId")).Text;
        BtnSave.Text = "Update";
        //********************
        //DDcompany.SelectedValue = companyid;
        DDunitname.SelectedValue = unitid;
        DDitem.SelectedValue = itemid;
        DDshape.SelectedValue = shapeid;
        DDshape_SelectedIndexChanged(sender, e);
        DDSize.SelectedValue = sizeid;
        DDsizetype.SelectedValue = sizetype;
        DDLoomType.SelectedItem.Text = Loomtype;
        txtLoomNo.Text = LoomNo;
        
        DDSupervisorName.SelectedValue = lblSupervisorId;             
        
    }
    protected void Disablecontrol()
    {
        DDcompany.Enabled = false;
        DDunitname.Enabled = false;
        DDitem.Enabled = false;
        DDshape.Enabled = false;
        DDSize.Enabled = false;
    }
    protected void Enablecontrol()
    {
        DDcompany.Enabled = true;
        DDunitname.Enabled = true;
        DDitem.Enabled = true;
        DDshape.Enabled = true;
        DDSize.Enabled = true;
    }
    protected void lbDelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State==ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            LinkButton lb = (LinkButton)sender;
            GridViewRow grv = (GridViewRow)lb.NamingContainer;
            string Uid = ((Label)DG.Rows[grv.RowIndex].FindControl("lbluid")).Text;
            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@uid", Uid);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            //***********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_delProductionLoomMaster", param);
            Tran.Commit();
            lblmsg.Text = param[1].Value.ToString();
            FillGrid();
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

    protected void lnkLoomMaster_ED(object sender, EventArgs e)
    {  
        LinkButton lnk = sender as LinkButton;
        if (lnk != null)
        {
            GridViewRow gvr = lnk.NamingContainer as GridViewRow;
            string Uid = ((Label)DG.Rows[gvr.RowIndex].FindControl("lbluid")).Text;
            Label lblLoomMasterEnable_Disable = (Label)gvr.FindControl("lblLoomMasterEnable_Disable");
            string updateval = lblLoomMasterEnable_Disable.Text == "1" ? "0" : "1";
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update ProductionLoomMaster set EnableDisableStatus=" + updateval + " where uid=" + Uid + "");
            FillGrid();
        }
    }
}