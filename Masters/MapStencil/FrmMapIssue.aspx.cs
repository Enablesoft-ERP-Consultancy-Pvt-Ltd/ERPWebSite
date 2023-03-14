using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_MapStencil_FrmMapIssue : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                            Select UnitID, UnitName From Unit(Nolock) Where UnitID in (1, 2) ";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDunit, ds, 1, false, "");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                FillCustomercode();
            }

           
            DDunit.SelectedValue = "2";
            txtAssignDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtRequiredDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
            }

            //switch (Session["varcompanyId"].ToString())
            //{
            //    case "2":

            switch(Session["VarCompanyId"].ToString())
            {
                case "30":
                    TDCustomerCode.Visible = false;
                    FillCustomerOrderNo();
                    break;
                default:
                    TDCustomerCode.Visible = true;
                    break;
            }

            
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
        if (Session["VarCompanyId"].ToString() == "30")
        {
            FillCustomerOrderNo();
        }
        else
        {
            FillCustomercode();
        }

    }
    protected void FillCustomerOrderNo()
    {
        string str = "";
        str = @"select Distinct OM.OrderId,OM.CustomerOrderNo+'/ '+OM.LocalOrder as OrderNo from OrderDetail OD  inner join  OrderMaster OM on  OM.OrderId=OD.OrderId 
                    Where OM.Status=0 and OM.CompanyId=" + DDCompany.SelectedValue;
        if (DDCustomerCode.SelectedIndex > 0)
        {
            str = str + " and OM.CustomerId=" + DDCustomerCode.SelectedValue;
        }
        str = str + " order by OM.OrderId";

        UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, str, true, "--Plz Select--");
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCustomerOrderNo();
        //FillDesignerName();

        DG.DataSource = null;
        DG.DataBind();

    }
    protected void FillDesignerName()
    {
        if (chkEdit.Checked == true)
        {
            string str = "";
            str = @"Select distinct EI.EmpId,EI.EmpName from Map_IssueMaster MIM inner join Map_IssueDetail MID on MIM.ID=MID.MasterID 
                    INNER JOIN EmpInfo EI ON MIM.EmpId=EI.EmpId
                    INNER JOIN Department DP ON EI.DepartmentId=DP.DepartmentId 
                    Where MID.OrderId=" + DDCustomerOrderNo.SelectedValue + " and EI.DepartmentId=DP.DepartmentId  And DP.DepartmentName='DESIGNING' and isnull(Ei.blacklist,0)=0 order by EI.Empname";

            UtilityModule.ConditionalComboFill(ref DDDesignerName, str, true, "--Plz Select--");
        }
        else
        {
            string str = "";
            str = @"Select EI.EmpId,EI.EmpName from EmpInfo EI, Department DP Where EI.DepartmentId=DP.DepartmentId 
                And DP.DepartmentName='DESIGNING' And EI.MasterCompanyId=" + Session["varCompanyId"] + " and isnull(Ei.blacklist,0)=0 order by EI.Empname";

            UtilityModule.ConditionalComboFill(ref DDDesignerName, str, true, "--Plz Select--");
        }
    }
    protected void DDCustomerOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDesignerName();
        Fillgrid();

        hnissueid.Value = "0";

        gvdetail.DataSource = null;
        gvdetail.DataBind();

        DDDesignerName.SelectedIndex = -1;
        DDissueno.Items.Clear();
        txtissueno.Text = "";
    }
    protected void FillIssueNo()
    {
        if (chkEdit.Checked == true)
        {
            string str = @"select Distinct MIM.ID,MIM.ChallanNo from Map_IssueMaster MIM inner join Map_IssueDetail MID on MIM.ID=MID.MasterID
                           and MID.OrderID=" + DDCustomerOrderNo.SelectedValue + " and MIM.EmpID=" + DDDesignerName.SelectedValue + " and MIM.MapStencilType=" + DDMapStencilType.SelectedValue + " ";

            UtilityModule.ConditionalComboFill(ref DDissueno, str, true, "--Plz Select--");
        }
        //else
        //{
        //    Fillgrid();
        //}
    }
    protected void DDDesignerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillIssueNo();

    }
    protected void DDMapStencilType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkEdit.Checked == true)
        {
            FillIssueNo();
            gvdetail.DataSource = null;
            gvdetail.DataBind();
            txtissueno.Text = "";

        }
        hnissueid.Value = "0";
        Fillgrid();
    }
    protected void Fillgrid()
    {

        string str = @"select OM.OrderId,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,VF.SIZEMtr,
                        CASE WHEN " + DDunit.SelectedValue + @" = 2 THEN VF.SIZEFT ELSE VF.SIZEMtr END As Size,
                        sum(OD.QtyRequired) as OrderQty,OD.Item_Finished_Id, dbo.[GET_MAPISSUEQTY](OD.Item_Finished_Id,OM.OrderID," + DDMapStencilType.SelectedValue + "," + DDCompany.SelectedValue + @") as PreIssueQty,
                        CASE WHEN " + DDunit.SelectedValue + @" = 2 THEN VF.ProdAreaFt ELSE VF.AreaMtr END As Area
                        from OrderMaster OM INNER JOIN OrderDetail OD ON OM.OrderId=OD.OrderId
                        INNER JOIN V_FinishedItemDetail VF ON OD.Item_Finished_Id=VF.Item_Finished_Id
                        Where OM.Orderid=" + DDCustomerOrderNo.SelectedValue + @"
                        Group By OM.OrderId,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,OD.OrderUnitId,VF.SIZEFT,VF.SIZEMTR,VF.SIZEINCH,OD.Item_Finished_Id,VF.ProdAreaFt,VF.AreaMtr";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();

    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            //for (int i = 0; i < DG.Columns.Count; i++)
            //{
            //    if (variable.VarBINNOWISE == "1")
            //    {
            //        if (DG.Columns[i].HeaderText == "BinNo")
            //        {
            //            DG.Columns[i].Visible = true;
            //        }
            //    }
            //    else
            //    {
            //        if (DG.Columns[i].HeaderText == "BinNo")
            //        {
            //            DG.Columns[i].Visible = false;
            //        }
            //    }
            //}
            DropDownList DDMapType = ((DropDownList)e.Row.FindControl("DDMapType"));
            string str = @"select ID,MapType from MapType order by MapType";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDMapType, ds, 0, true, "--Plz Select--");
        }
    }
    private void CHECKVALIDCONTROL()
    {
        lblmessage.Text = "";
        //if (UtilityModule.VALIDDROPDOWNLIST(DDCompany) == false)
        //{
        //    goto a;
        //} 
        if (Session["varCompanyId"].ToString() != "30")
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerCode) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerOrderNo) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDDesignerName) == false)
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
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            //********sql table Type
            DataTable dtrecords = new DataTable();
            dtrecords.Columns.Add("ItemFinishedId", typeof(int));
            dtrecords.Columns.Add("OrderID", typeof(int));
            dtrecords.Columns.Add("MapIssueType", typeof(int));
            dtrecords.Columns.Add("MapIssueQty", typeof(float));           
            dtrecords.Columns.Add("MapIssueRate", typeof(float));
            dtrecords.Columns.Add("MapIssueArea", typeof(float));
            dtrecords.Columns.Add("RateCalcType", typeof(int));
           

            //*******************
            for (int i = 0; i < DG.Rows.Count; i++)
            {
                CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
                TextBox txtMapIssueQty = ((TextBox)DG.Rows[i].FindControl("txtMapIssueQty"));
                DropDownList DDMapType = ((DropDownList)DG.Rows[i].FindControl("DDMapType"));
                TextBox txtMapRate = ((TextBox)DG.Rows[i].FindControl("txtMapRate"));
                Label lblArea = ((Label)DG.Rows[i].FindControl("lblArea"));



                if (Chkboxitem.Checked == true && (txtMapIssueQty.Text != "") && DDMapType.SelectedIndex > 0)
                {
                    Label lblItemFinishedId = ((Label)DG.Rows[i].FindControl("lblItemFinishedId"));
                    Label lblOrderId = ((Label)DG.Rows[i].FindControl("lblOrderId"));
                    //*********************
                    DataRow dr = dtrecords.NewRow();
                    dr["ItemFinishedId"] = lblItemFinishedId.Text;
                    dr["OrderID"] = lblOrderId.Text;
                    dr["MapIssueType"] = DDMapType.SelectedValue;
                    dr["MapIssueQty"] = txtMapIssueQty.Text == "" ? "0" : txtMapIssueQty.Text;
                    dr["MapIssueRate"] = txtMapRate.Text == "" ? "0" : txtMapRate.Text;
                    dr["MapIssueArea"] = lblArea.Text == "" ? "0" : lblArea.Text;
                    dr["RateCalcType"] = DDRateCalcType.SelectedValue;
                    dtrecords.Rows.Add(dr);
                }
            }
            if (dtrecords.Rows.Count > 0)
            {
                SqlTransaction Tran = con.BeginTransaction();
                try
                {
                    SqlParameter[] param = new SqlParameter[14];
                    param[0] = new SqlParameter("@ID", SqlDbType.Int);
                    param[0].Value = hnissueid.Value;
                    param[0].Direction = ParameterDirection.InputOutput;
                    param[1] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
                    param[2] = new SqlParameter("@CustomerId", DDCustomerCode.SelectedValue);
                    param[3] = new SqlParameter("@EmpId", DDDesignerName.SelectedValue);
                    param[4] = new SqlParameter("@AssignDate", txtAssignDate.Text);
                    param[5] = new SqlParameter("@RequiredDate", txtRequiredDate.Text);
                    param[6] = new SqlParameter("@userid", Session["varuserid"]);
                    param[7] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
                    param[8] = new SqlParameter("@dtrecords", dtrecords);
                    param[9] = new SqlParameter("@MapStencilType", SqlDbType.TinyInt);
                    param[9].Value = DDMapStencilType.SelectedValue;
                    param[10] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                    param[10].Direction = ParameterDirection.Output;
                    param[11] = new SqlParameter("@UnitID", SqlDbType.TinyInt);
                    param[11].Value = DDunit.SelectedValue;
                    param[12] = new SqlParameter("@Remarks", TxtRemarks.Text);
                    param[13] = new SqlParameter("@CHALLANNO", SqlDbType.VarChar, 50);
                    param[13].Value = "";
                    param[13].Direction = ParameterDirection.InputOutput;
                   

                    ///**********
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveMapIssue", param);
                    //*******************
                    //ViewState["reportid"] = param[0].Value.ToString();
                    txtissueno.Text = param[13].Value.ToString();
                    hnissueid.Value = param[0].Value.ToString();
                    Tran.Commit();
                    if (param[10].Value.ToString() != "")
                    {
                        lblmessage.Text = param[10].Value.ToString();
                    }
                    else
                    {
                        lblmessage.Text = "DATA SAVED SUCCESSFULLY.";
                        Fillgrid();
                        FillissueGrid();
                    }

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
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
            }
        }

    }
    protected void FillissueGrid()
    {
        TxtRemarks.Text = "";
        string str = @"select MIM.Id,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,
                        CASE WHEN MIM.UnitID = 2 THEN VF.SIZEFT ELSE VF.SIZEMtr END As Size,
                        MID.DetailId,MID.OrderId,MID.ItemFinishedID,MIM.CompanyId,MIM.CustomerId,MIM.EmpId,MIM.MapStencilType,MIM.ChallanNo,
                        Replace(CONVERT(nvarchar(11),MIM.AssignDate,106),' ','-') as AssignDate,Replace(CONVERT(nvarchar(11),MIM.RequiredDate,106),' ','-') as RequiredDate,
                        MID.MapIssueType,MID.MapIssueQty, MT.MapType as MapIssueTypeName, MIM.UnitID ,MIM.Remarks, MID.MapArea, isnull(MID.Rate,0) as MapIssueRate,MID.RateCalcType,
                        isnull(MID.Amount,0) as MapAmount
                        from Map_IssueMaster MIM 
                        INNER JOIN Map_IssueDetail MID ON MIM.Id=MID.MasterId 
                        INNER JOIN V_FinishedItemDetail VF ON MID.ItemFinishedId=VF.Item_Finished_Id
                        INNER JOIN OrderDetail OD ON MID.OrderId=OD.OrderID and MID.ItemFinishedId=OD.Item_Finished_id
                        INNER JOIN MapType MT ON MID.MapIssueType=MT.ID                     
                        Where MIM.CompanyId = " + DDCompany.SelectedValue;

        //where MIM.ID=" + hnissueid.Value;
        if (txtEditIssueNo.Text != "")
        {
            str = str + " and MIM.ChallanNo='" + txtEditIssueNo.Text + "'";
        }
        else
        {
            str = str + " and MIM.ID=" + hnissueid.Value + "";
        }

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
        if (chkEdit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtissueno.Text = ds.Tables[0].Rows[0]["ChallanNo"].ToString();
                txtAssignDate.Text = ds.Tables[0].Rows[0]["AssignDate"].ToString();
                txtRequiredDate.Text = ds.Tables[0].Rows[0]["RequiredDate"].ToString();
                //DDCompany.SelectedValue = ds.Tables[0].Rows[0]["CompanyId"].ToString();
                DDunit.SelectedValue = ds.Tables[0].Rows[0]["UnitId"].ToString();
                if (Session["varCompanyId"].ToString() != "30")
                {
                    FillCustomercode();
                    DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["CustomerId"].ToString();
                }
                FillCustomerOrderNo();
                DDCustomerOrderNo.SelectedValue = ds.Tables[0].Rows[0]["OrderId"].ToString();
                FillDesignerName();
                DDDesignerName.SelectedValue = ds.Tables[0].Rows[0]["EmpId"].ToString();
                DDMapStencilType.SelectedValue = ds.Tables[0].Rows[0]["MapStencilType"].ToString();
                FillIssueNo();
                DDissueno.SelectedValue = ds.Tables[0].Rows[0]["ID"].ToString();
                TxtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                DDRateCalcType.SelectedValue = ds.Tables[0].Rows[0]["RateCalcType"].ToString();
            }
            else
            {
                txtissueno.Text = "";
                txtAssignDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
                txtRequiredDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@Id", hnissueid.Value);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_MapTraceIssueReport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\RptMapTraceIssue.rpt";            
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptMapTraceIssue.xsd";
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
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
        if (chkEdit.Checked == true)
        {
            TDEditIssueNo.Visible = true;
            DDCustomerCode.SelectedIndex = -1;
            DDCustomerOrderNo.SelectedIndex = -1;
            DDDesignerName.SelectedIndex = -1;
            TDIssueNo.Visible = true;
            DDissueno.SelectedIndex = -1;
            DG.DataSource = null;
            DG.DataBind();
            gvdetail.DataSource = null;
            gvdetail.DataBind();
            txtissueno.Text = "";
            txtAssignDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            DDRateCalcType.Enabled = false;
        }
        else
        {
            TDEditIssueNo.Visible = false;
            txtEditIssueNo.Text = "";
            DDCustomerCode.SelectedIndex = -1;
            DDCustomerOrderNo.SelectedIndex = -1;
            DDDesignerName.SelectedIndex = -1;
            TDIssueNo.Visible = false;
            DDissueno.SelectedIndex = -1;
            DG.DataSource = null;
            DG.DataBind();
            gvdetail.DataSource = null;
            gvdetail.DataBind();
            txtissueno.Text = "";
            txtAssignDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            DDRateCalcType.Enabled = true;
        }
    }
    protected void txtEditIssueNo_TextChanged(object sender, EventArgs e)
    {
        FillissueGrid();
        Fillgrid();
        DDissueno_SelectedIndexChanged(sender, new EventArgs());
    }
    protected void DDissueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkEdit.Checked == true)
        {
            txtEditIssueNo.Text = "";
        }
       
         hnissueid.Value =DDissueno.SelectedIndex >0 ? DDissueno.SelectedValue : "0";        
        
        FillissueGrid();
    }
    protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gvdetail.EditIndex == e.Row.RowIndex)
        {
            DropDownList DDMapType = (DropDownList)e.Row.FindControl("DDMapType");

            string str = @"select ID,MapType from MapType order by MapType";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDMapType, ds, 0, true, "--Plz Select--");

            string selectedMapIssueType = DataBinder.Eval(e.Row.DataItem, "MapIssueType").ToString();
            DDMapType.Items.FindByValue(selectedMapIssueType).Selected = true;
        }
    }
    protected void gvdetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvdetail.EditIndex = e.NewEditIndex;
        FillissueGrid();
    }
    protected void gvdetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvdetail.EditIndex = -1;
        FillissueGrid();
    }
    protected void gvdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblhqty = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblhqty");
            Label lblId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblId");
            Label lblDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblDetailId");
            Label lblOrderId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblOrderId");
            TextBox txtqty = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtqty");
            Label lblItemFinishedId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblItemFinishedId");
            DropDownList DDMapType = (DropDownList)gvdetail.Rows[e.RowIndex].FindControl("DDMapType");
            Label lblMapStencilType = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblMapStencilType");
            TextBox txtremarks = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtRemarks");
            TextBox txtMapIssueRate = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtMapIssueRate");
            Label lblMapArea = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblMapArea");


            //**************
            SqlParameter[] param = new SqlParameter[14];
            param[0] = new SqlParameter("@Id", lblId.Text);
            param[1] = new SqlParameter("@DetailId", lblDetailId.Text);
            param[2] = new SqlParameter("@OrderId", lblOrderId.Text);
            param[3] = new SqlParameter("@hqty", lblhqty.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@MapIssueQty", txtqty.Text == "" ? "0" : txtqty.Text);
            param[6] = new SqlParameter("@ItemFinishedId", lblItemFinishedId.Text);
            param[7] = new SqlParameter("@userid", Session["varuserid"]);
            param[8] = new SqlParameter("@MapIssueType", DDMapType.SelectedValue);
            param[9] = new SqlParameter("@MapStencilType", lblMapStencilType.Text);
            param[10] = new SqlParameter("@MapRemarks", txtremarks.Text);
            param[11] = new SqlParameter("@MapIssueRate", txtMapIssueRate.Text == "" ? "0" : txtMapIssueRate.Text);
            param[12] = new SqlParameter("@RateCalcType", DDRateCalcType.SelectedValue);
            param[13] = new SqlParameter("@MapIssueArea", lblMapArea.Text);
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEMAPISSUE", param);
            lblmessage.Text = param[4].Value.ToString();
            Tran.Commit();
            gvdetail.EditIndex = -1;
            FillissueGrid();
            Fillgrid();
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblhqty = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblhqty");
            Label lblId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblId");
            Label lblDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblDetailId");
            Label lblOrderId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblOrderId");
            TextBox txtqty = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtqty");
            Label lblItemFinishedId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblItemFinishedId");

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@Id", lblId.Text);
            param[1] = new SqlParameter("@Detailid", lblDetailId.Text);
            param[2] = new SqlParameter("@OrderId", lblOrderId.Text);
            param[3] = new SqlParameter("@ItemFinishedId", lblItemFinishedId.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEMAPISSUE", param);
            lblmessage.Text = param[4].Value.ToString();
            Tran.Commit();
            FillissueGrid();
            Fillgrid();
            //***************
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }

}