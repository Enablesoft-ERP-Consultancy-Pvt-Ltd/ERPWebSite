using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_MapStencil_FrmMapReceive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName 
                from Companyinfo CI,Company_Authentication CA 
                Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName 
                Select ID, Name From MapStencilType Order By ID ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                FillCustomercode();
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDMapStencilType, ds, 1, false, "");

            txtReceiveDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            //txtRequiredDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
            }

            hnReceiveid.Value = "0";

            switch (Session["VarCompanyId"].ToString())
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
    protected void FillissueGrid()
    {

        string str = @"select MIM.Id,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,
                CASE WHEN MIM.UnitID = 2 THEN VF.SIZEFT ELSE VF.SIZEMtr END As Size, 
                MID.DetailId,MID.OrderId,MID.ItemFinishedID,MIM.CompanyId,MIM.CustomerId,MIM.EmpId,MIM.MapStencilType,MIM.ChallanNo,
                Replace(CONVERT(nvarchar(11),MIM.AssignDate,106),' ','-') as AssignDate,Replace(CONVERT(nvarchar(11),MIM.RequiredDate,106),' ','-') as RequiredDate,
                MID.MapIssueType,MID.MapIssueQty, MT.MapType as MapIssueTypeName, dbo.[GET_MAPRECEIVEQTY](MIM.ID, MID.ItemFinishedId,MID.OrderID,
                MIM.MapStencilType," + DDCompany.SelectedValue + @") as PreReceiveQty,MIM.Remarks,isnull(MID.MapArea,0) as MapArea,isnull(MID.Rate,0) as Rate,MID.RateCalcType,
                Case When MID.RateCalcType=0 Then 'Area Wise' Else 'Pc Wise' End as RateCalcTypeName,
                isnull(MID.Amount,0) as Amount,U.UnitName
                From Map_IssueMaster MIM 
                INNER JOIN Map_IssueDetail MID ON MIM.Id=MID.MasterId 
                INNER JOIN V_FinishedItemDetail VF ON MID.ItemFinishedId=VF.Item_Finished_Id
                INNER JOIN OrderDetail OD ON MID.OrderId=OD.OrderID and MID.ItemFinishedId=OD.Item_Finished_id
                INNER JOIN MapType MT ON MID.MapIssueType=MT.ID
                INNER JOIN Unit U ON MIM.UnitID=U.UnitId
                where MIM.CompanyId=" + DDCompany.SelectedValue + " ";

        if (txtIssueNo.Text != "")
        {
            str = str + " and MIM.ChallanNo='" + txtIssueNo.Text + "'";
        }
        else if (DDissueno.SelectedIndex > 0)
        {
            str = str + " and MIM.Id=" + DDissueno.SelectedValue + "";
        }
        else
        {
            str = str + " and MIM.ChallanNo=''";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DG.DataSource = ds.Tables[0];
            DG.DataBind();

            txtIssueNo.Text = ds.Tables[0].Rows[0]["ChallanNo"].ToString();
            //DDCompany.SelectedValue = ds.Tables[0].Rows[0]["CompanyId"].ToString();
            FillCustomercode();
            DDMapStencilType.SelectedValue = ds.Tables[0].Rows[0]["MapStencilType"].ToString();
            DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["CustomerId"].ToString();
            FillCustomerOrderNo();
            DDCustomerOrderNo.SelectedValue = ds.Tables[0].Rows[0]["OrderId"].ToString();
            FillDesignerName();
            DDDesignerName.SelectedValue = ds.Tables[0].Rows[0]["EmpId"].ToString();
            DDMapStencilType.SelectedValue = ds.Tables[0].Rows[0]["MapStencilType"].ToString();
            FillIssueNo();
            DDissueno.SelectedValue = ds.Tables[0].Rows[0]["ID"].ToString();
            TxtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
            lblMapIssueUnit.Text = ds.Tables[0].Rows[0]["UnitName"].ToString();
            lblMapIssueRateCalcType.Text = ds.Tables[0].Rows[0]["RateCalcTypeName"].ToString();
        }
        else
        {
            txtIssueNo.Text = "";
            DG.DataSource = null;
            DG.DataBind();

        }

    }
    protected void FillIssueNo()
    {

        string str = @"select Distinct MIM.ID,MIM.ChallanNo from Map_IssueMaster MIM inner join Map_IssueDetail MID on MIM.ID=MID.MasterID
                       Where MIM.CompanyId=" + DDCompany.SelectedValue + " and MIM.CustomerId=" + DDCustomerCode.SelectedValue + " and  MID.OrderID=" + DDCustomerOrderNo.SelectedValue + @" 
                       and MIM.EmpID=" + DDDesignerName.SelectedValue + " and MIM.MapStencilType=" + DDMapStencilType.SelectedValue + " ";

        UtilityModule.ConditionalComboFill(ref DDissueno, str, true, "--Plz Select--");

    }
    protected void txtIssueNo_TextChanged(object sender, EventArgs e)
    {

        FillissueGrid();
        if (chkEdit.Checked == true)
        {
            DDissueno_SelectedIndexChanged(sender, new EventArgs());
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
        string str = "";
        if (chkEdit.Checked == true)
        {
            str = @"Select distinct EI.EmpId,EI.EmpName from Map_IssueMaster MIM inner join Map_IssueDetail MID on MIM.ID=MID.MasterID 
                    INNER JOIN EmpInfo EI ON MIM.EmpId=EI.EmpId
                    INNER JOIN Department DP ON EI.DepartmentId=DP.DepartmentId 
                    Where MID.OrderId=" + DDCustomerOrderNo.SelectedValue + " and EI.DepartmentId=DP.DepartmentId  and isnull(Ei.blacklist,0)=0";
        }
        else
        {
            str = @"Select EI.EmpId,EI.EmpName from EmpInfo EI, Department DP Where EI.DepartmentId=DP.DepartmentId 
                And EI.MasterCompanyId=" + Session["varCompanyId"] + " and isnull(Ei.blacklist,0)=0 ";

        } 
        
        if (DDMapStencilType.SelectedValue == "3")
        {
            str = str + " And DP.DepartmentName='PRODUCTION'";
        }
        else
        {
            str = str + " And DP.DepartmentName='DESIGNING'";
        }

        str = str + " order by EI.Empname";
        UtilityModule.ConditionalComboFill(ref DDDesignerName, str, true, "--Plz Select--");
    }
    protected void DDCustomerOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDesignerName();
        FillissueGrid();
        //Fillgrid();

        //hnissueid.Value = "0";

        //gvdetail.DataSource = null;
        //gvdetail.DataBind();

        DDDesignerName.SelectedIndex = -1;
        DDissueno.Items.Clear();
        //txtissueno.Text = "";
    }
    protected void DDDesignerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillissueGrid();
        FillIssueNo();

    }
    protected void DDMapStencilType_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnReceiveid.Value = "0";
        FillIssueNo();
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

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
            dtrecords.Columns.Add("ID", typeof(int));
            dtrecords.Columns.Add("DetailId", typeof(int));
            dtrecords.Columns.Add("ItemFinishedId", typeof(int));
            dtrecords.Columns.Add("OrderID", typeof(int));
            dtrecords.Columns.Add("MapIssueType", typeof(int));
            dtrecords.Columns.Add("ReceiveQty", typeof(float));
            dtrecords.Columns.Add("MapReceiveRate", typeof(float));
            dtrecords.Columns.Add("MapReceiveArea", typeof(float));
            dtrecords.Columns.Add("ReceiveRateCalcType", typeof(int));

            //*******************
            for (int i = 0; i < DG.Rows.Count; i++)
            {
                CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
                TextBox txtMapReceiveQty = ((TextBox)DG.Rows[i].FindControl("txtMapReceiveQty"));

                if (Chkboxitem.Checked == true && (txtMapReceiveQty.Text != ""))
                {
                    Label lblItemFinishedId = ((Label)DG.Rows[i].FindControl("lblItemFinishedId"));
                    Label lblOrderId = ((Label)DG.Rows[i].FindControl("lblOrderId"));
                    Label lblId = ((Label)DG.Rows[i].FindControl("lblId"));
                    Label lblDetailId = ((Label)DG.Rows[i].FindControl("lblDetailId"));
                    Label lblMapIssueTypeid = ((Label)DG.Rows[i].FindControl("lblMapIssueTypeid"));

                    TextBox txtMapIssueRate = ((TextBox)DG.Rows[i].FindControl("txtMapIssueRate"));
                    Label lblMapIssueArea = ((Label)DG.Rows[i].FindControl("lblMapIssueArea"));
                    Label lblRateCalcType = ((Label)DG.Rows[i].FindControl("lblRateCalcType"));
                    //*********************
                    DataRow dr = dtrecords.NewRow();
                    dr["ID"] = lblId.Text;
                    dr["DetailID"] = lblDetailId.Text;
                    dr["ItemFinishedId"] = lblItemFinishedId.Text;
                    dr["OrderID"] = lblOrderId.Text;
                    dr["MapIssueType"] = lblMapIssueTypeid.Text;
                    dr["ReceiveQty"] = txtMapReceiveQty.Text == "" ? "0" : txtMapReceiveQty.Text;

                    dr["MapReceiveRate"] = txtMapIssueRate.Text == "" ? "0" : txtMapIssueRate.Text;
                    dr["MapReceiveArea"] = lblMapIssueArea.Text == "" ? "0" : lblMapIssueArea.Text;
                    dr["ReceiveRateCalcType"] = lblRateCalcType.Text;
                    dtrecords.Rows.Add(dr);
                }
            }
            if (dtrecords.Rows.Count > 0)
            {
                SqlTransaction Tran = con.BeginTransaction();
                try
                {
                    SqlParameter[] param = new SqlParameter[12];
                    param[0] = new SqlParameter("@RID", SqlDbType.Int);
                    param[0].Value = hnReceiveid.Value;
                    param[0].Direction = ParameterDirection.InputOutput;
                    param[1] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
                    param[2] = new SqlParameter("@CustomerId", DDCustomerCode.SelectedValue);
                    param[3] = new SqlParameter("@EmpId", DDDesignerName.SelectedValue);
                    param[4] = new SqlParameter("@ReceiveDate", txtReceiveDate.Text);
                    param[5] = new SqlParameter("@userid", Session["varuserid"]);
                    param[6] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
                    param[7] = new SqlParameter("@dtrecords", dtrecords);
                    param[8] = new SqlParameter("@MapStencilType", SqlDbType.TinyInt);
                    param[8].Value = DDMapStencilType.SelectedValue;
                    param[9] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                    param[9].Direction = ParameterDirection.Output;
                    param[10] = new SqlParameter("@Remarks", TxtRemarks.Text);
                    param[11] = new SqlParameter("@CHALLANNO", SqlDbType.VarChar, 50);
                    param[11].Value = "";
                    param[11].Direction = ParameterDirection.InputOutput;
                    ///**********
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveMapReceive", param);
                    //*******************
                    //ViewState["reportid"] = param[0].Value.ToString();
                    txtReceiveNo.Text = param[11].Value.ToString();
                    hnReceiveid.Value = param[0].Value.ToString();
                    Tran.Commit();
                    if (param[9].Value.ToString() != "")
                    {
                        lblmessage.Text = param[9].Value.ToString();
                    }
                    else
                    {
                        lblmessage.Text = "DATA SAVED SUCCESSFULLY.";
                        //Fillgrid();
                        FillissueGrid();
                        FillReceiveGrid();
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
    protected void FillReceiveGrid()
    {
        string str = @"select MRM.RId,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,
                        CASE WHEN MRD.UnitID = 2 THEN VF.SIZEFT ELSE VF.SIZEMtr END As Size, 
                        MRD.RDetailId,MRD.OrderId,MRD.ItemFinishedID,MRM.CompanyId,MRM.CustomerId,MRM.EmpId,MRM.MapStencilType,MRM.ChallanNo,
                        Replace(CONVERT(nvarchar(11),MRM.ReceiveDate,106),' ','-') as ReceiveDate,
                        MRD.MapIssueType,MRD.ReceiveQty, MT.MapType as MapIssueTypeName,MRD.ID,MRD.DetailID, MRM.Remarks,
                        MRD.ReceiveMapArea, isnull(MRD.ReceiveRate,0) as MapReceiveRate,MRD.ReceiveRateCalcType,
                        isnull(MRD.ReceiveAmount,0) as ReceiveAmount
                        from Map_ReceiveMaster MRM INNER JOIN Map_ReceiveDetail MRD ON MRM.RId=MRD.RID
                        INNER JOIN V_FinishedItemDetail VF ON MRD.ItemFinishedId=VF.Item_Finished_Id
                        INNER JOIN OrderDetail OD ON MRD.OrderId=OD.OrderID and MRD.ItemFinishedId=OD.Item_Finished_id
                        INNER JOIN MapType MT ON MRD.MapIssueType=MT.ID
                        Where MRM.CompanyId = " + DDCompany.SelectedValue + " And MRM.RId = " + hnReceiveid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
        if (chkEdit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtReceiveNo.Text = ds.Tables[0].Rows[0]["ChallanNo"].ToString();
                txtReceiveDate.Text = ds.Tables[0].Rows[0]["ReceiveDate"].ToString();
                //DDCompany.SelectedValue = ds.Tables[0].Rows[0]["CompanyId"].ToString();
                DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["CustomerId"].ToString();
                DDCustomerOrderNo.SelectedValue = ds.Tables[0].Rows[0]["OrderId"].ToString();
                DDDesignerName.SelectedValue = ds.Tables[0].Rows[0]["EmpId"].ToString();
                TxtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
            }
            else
            {
                TxtRemarks.Text = "";
                txtReceiveNo.Text = "";
                txtReceiveDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@RID", hnReceiveid.Value);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_MapTraceReceiveReport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptMapTraceReceive.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptMapTraceReceive.xsd";
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
            TDReceiveNo.Visible = true;
        }
        else
        {
            TDReceiveNo.Visible = false;
            DDReceiveNo.Items.Clear();
        }

    }
    protected void FillReceiveNo()
    {
        string str = @"select Distinct MRM.RID,MRM.ChallanNo from Map_ReceiveMaster MRM inner join Map_ReceiveDetail MRD on MRM.RID=MRD.RID
                       Where MRD.ID =" + DDissueno.SelectedValue + " ";

        UtilityModule.ConditionalComboFill(ref DDReceiveNo, str, true, "--Plz Select--");

    }
    protected void DDissueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkEdit.Checked == true)
        {
            FillReceiveNo();
            gvdetail.DataSource = null;
            gvdetail.DataBind();

        }
        else
        {
            gvdetail.DataSource = null;
            gvdetail.DataBind();
            //hnissueid.Value = DDissueno.SelectedValue;
            hnReceiveid.Value = "0";
            txtIssueNo.Text = "";
            FillissueGrid();
        }
    }
    protected void DDReceiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnReceiveid.Value = DDReceiveNo.SelectedValue;
        FillReceiveGrid();
    }
    protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow && gvdetail.EditIndex == e.Row.RowIndex)
        //{
        //    DropDownList DDMapType = (DropDownList)e.Row.FindControl("DDMapType");

        //    string str = @"select ID,MapType from MapType order by MapType";

        //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //    UtilityModule.ConditionalComboFillWithDS(ref DDMapType, ds, 0, true, "--Plz Select--");

        //    string selectedMapIssueType = DataBinder.Eval(e.Row.DataItem, "MapIssueType").ToString();
        //    DDMapType.Items.FindByValue(selectedMapIssueType).Selected = true;

        //}
    }
    protected void gvdetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvdetail.EditIndex = e.NewEditIndex;
        FillReceiveGrid();
        //FillissueGrid();
    }
    protected void gvdetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvdetail.EditIndex = -1;
        FillReceiveGrid();
        //FillissueGrid();
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
            Label lblRId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblRId");
            Label lblRDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblRDetailId");
            Label lblId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblId");
            Label lblDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblDetailId");
            Label lblOrderId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblOrderId");
            TextBox txtqty = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtqty");
            Label lblItemFinishedId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblItemFinishedId");
            DropDownList DDMapType = (DropDownList)gvdetail.Rows[e.RowIndex].FindControl("DDMapType");

            TextBox txtMapReceiveRate = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtMapReceiveRate");
            Label lblReceiveMapArea = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblReceiveMapArea");
            Label lblReceiveRateCalcType = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblReceiveRateCalcType");
            Label lblMapIssueType = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblMapIssueType");

            //**************
            SqlParameter[] param = new SqlParameter[15];
            param[0] = new SqlParameter("@RId", lblRId.Text);
            param[1] = new SqlParameter("@RDetailId", lblRDetailId.Text);
            param[2] = new SqlParameter("@Id", lblId.Text);
            param[3] = new SqlParameter("@DetailId", lblDetailId.Text);
            param[4] = new SqlParameter("@OrderId", lblOrderId.Text);
            param[5] = new SqlParameter("@hqty", lblhqty.Text);
            param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[6].Direction = ParameterDirection.Output;
            param[7] = new SqlParameter("@ReceiveQty", txtqty.Text == "" ? "0" : txtqty.Text);
            param[8] = new SqlParameter("@ItemFinishedId", lblItemFinishedId.Text);
            param[9] = new SqlParameter("@userid", Session["varuserid"]);
            param[10] = new SqlParameter("@MapStencilType", DDMapStencilType.SelectedValue);
            param[11] = new SqlParameter("@ReceiveRATE", txtMapReceiveRate.Text == "" ? "0" : txtMapReceiveRate.Text);
            param[12] = new SqlParameter("@ReceiveRateCalcType", lblReceiveRateCalcType.Text);
            param[13] = new SqlParameter("@ReceiveMapArea", lblReceiveMapArea.Text);
            param[14] = new SqlParameter("@MapIssueType", lblMapIssueType.Text);
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEMAPRECEIVE", param);
            lblmessage.Text = param[6].Value.ToString();
            Tran.Commit();
            gvdetail.EditIndex = -1;
            FillissueGrid();
            FillReceiveGrid();
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
            Label lblRId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblRId");
            Label lblRDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblRDetailId");
            Label lblId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblId");
            Label lblDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblDetailId");
            Label lblOrderId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblOrderId");
            TextBox txtqty = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtqty");
            Label lblItemFinishedId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblItemFinishedId");

            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@RId", lblRId.Text);
            param[1] = new SqlParameter("@RDetailid", lblRDetailId.Text);
            param[2] = new SqlParameter("@Id", lblId.Text);
            param[3] = new SqlParameter("@Detailid", lblDetailId.Text);
            param[4] = new SqlParameter("@OrderId", lblOrderId.Text);
            param[5] = new SqlParameter("@ItemFinishedId", lblItemFinishedId.Text);
            param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[6].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEMAPRECEIVE", param);
            lblmessage.Text = param[6].Value.ToString();
            Tran.Commit();
            FillissueGrid();
            FillReceiveGrid();
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