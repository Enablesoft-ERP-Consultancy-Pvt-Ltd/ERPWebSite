using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Process_frmproductiondirectreceive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName
                           select UnitsId,UnitName from Units order by UnitName
                           select UnitId,UnitName From Unit where UnitId in(1,2)
                           select Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner Join CategorySeparate CS on ICM.CATEGORY_ID=CS.Categoryid and cs.id=0 order by ICM.CATEGORY_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDunit, ds, 2, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, ds, 3, true, "--Plz Select--");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            if (DDProdunit.Items.Count > 0)
            {
                DDProdunit.SelectedIndex = 1;
                DDProdunit_SelectedIndexChanged(sender, new EventArgs());
            }
            if (ddCatagory.Items.Count > 0)
            {
                ddCatagory.SelectedIndex = 1;
                ddCatagory_SelectedIndexChanged(sender, new EventArgs());
            }
            txtrecdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            DDcaltype.SelectedValue = "1";
            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
            }
        }
    }
    protected void DDProdunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnissueorderid.Value = "0";
        hnprocessrecid.Value = "0";
        UtilityModule.ConditionalComboFill(ref DDLoomNo, @"select  PM.UID,PM.LoomNo+'/'+IM.ITEM_NAME as LoomNo from ProductionLoomMaster PM 
                                            inner join ITEM_MASTER IM on PM.Itemid=IM.ITEM_ID                                            
                                            Where  PM.CompanyId=" + DDcompany.SelectedValue + " and PM.UnitId=" + DDProdunit.SelectedValue + " order by case when ISNUMERIC(PM.loomno)=1 Then CONVERT(int,PM.loomno) Else 9999999 End,PM.loomno", true, "--Plz Select--");
        if (chkEdit.Checked == true)
        {
            fillReceiveNo();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtgetvalue.Text != "")
        {
            FillWeaver();
        }
    }
    protected void FillWeaver()
    {
        string str = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            if (txtWeaverIdNo.Text != "")
            {
                DataSet ds = null;

                str = @"select Empid,Empcode+'-'+Empname as Empname from empinfo Where empid=" + txtgetvalue.Text + "";
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Empid,Empname from empinfo Where Empcode='" + txtWeaverIdNo.Text + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (listWeaverName.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) == null)
                    {

                        listWeaverName.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Empname"].ToString(), ds.Tables[0].Rows[0]["Empid"].ToString()));
                    }

                    txtWeaverIdNo.Text = "";
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('No Weaver found at this ID No..');", true);

                }

                ds.Dispose();
            }
            txtWeaverIdNo.Focus();

        }
        catch (Exception ex)
        {

        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        listWeaverName.Items.Remove(listWeaverName.SelectedItem);
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select Item_Id,ITEM_NAME from Item_master Where CATEGORY_ID=" + ddCatagory.SelectedValue + " order by ITEM_NAME";
        UtilityModule.ConditionalComboFill(ref dditemname, str, true, "--Plz Select--");
        FillCombo();
    }
    protected void FillCombo()
    {
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
        TDShade.Visible = false;
        TDShape.Visible = false;
        TDSize.Visible = false;
        TDShade.Visible = false;

        string str = @"select PARAMETER_ID from ITEM_CATEGORY_PARAMETERS where category_id=" + ddCatagory.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["Parameter_Id"].ToString())
                {
                    case "1":
                        TDQuality.Visible = true;
                        break;
                    case "2":
                        TDDesign.Visible = true;
                        break;
                    case "3":
                        TDColor.Visible = true;
                        break;
                    case "4":
                        TDShape.Visible = true;
                        break;
                    case "5":
                        TDSize.Visible = true;
                        UtilityModule.ConditionalComboFill(ref DDsizetype, "select val,Type from SizeType Order by val", false, "");
                        DDsizetype.SelectedValue = "1";
                        break;
                    case "6":
                        TDShade.Visible = true;
                        break;
                }
            }
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQDCS();
    }

    protected void FillQDCS()
    {
        string str = null;
        //Quality
        if (TDQuality.Visible == true)
        {

            str = "select Distinct QualityId,QualityName from Quality Where Item_Id=" + dditemname.SelectedValue + " and QualityId>0 order by QualityName";
            UtilityModule.ConditionalComboFill(ref dquality, str, true, "--Select--");

        }
        //Design
        if (TDDesign.Visible == true)
        {
            str = "select Distinct designId,designName from V_FinishedItemDetail vf  Where Item_Id=" + dditemname.SelectedValue + " and DesignId>0 order by designname";
            UtilityModule.ConditionalComboFill(ref dddesign, str, true, "--Select--");
        }
        //Color
        if (TDColor.Visible == true)
        {
            str = "select Distinct colorid,colorname from V_FinishedItemDetail vf  Where Item_Id=" + dditemname.SelectedValue + " and colorid>0 order by Colorname";
            UtilityModule.ConditionalComboFill(ref ddcolor, str, true, "--Select--");

        }
        //Shape
        if (TDShape.Visible == true)
        {

            str = "select Distinct shapeid,shapename from V_FinishedItemDetail vf  Where Item_Id=" + dditemname.SelectedValue + " and shapeid>0 order by shapeid";

            UtilityModule.ConditionalComboFill(ref ddshape, str, true, "--Select--");

        }
        //Shade
        if (TDShade.Visible == true)
        {
            str = "select shadecolorid,shadecolorname from shadecolor   order by shadecolorname";
            UtilityModule.ConditionalComboFill(ref ddlshade, str, true, "--Select--");
        }
        //Unit
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
    protected void Fillsize()
    {

        string str = null, size = null;
        switch (DDsizetype.SelectedValue.ToString())
        {
            case "0":
                size = "SizeFt";
                break;
            case "1":
                size = "Sizemtr";
                break;
            case "2":
                size = "Sizeinch";
                break;
            default:
                size = "SizeFt";
                break;

        }

        str = "select Distinct sizeid," + size + " from V_FinishedItemDetail vf Where ITEM_ID=" + dditemname.SelectedValue + " and vf.shapeid=" + ddshape.SelectedValue + " and Sizeid>0 order by " + size;

        UtilityModule.ConditionalComboFill(ref ddsize, str, true, "--Select--");

    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        //Get Empid 
        string StrEmpid = "";
        for (int i = 0; i < listWeaverName.Items.Count; i++)
        {
            if (StrEmpid == "")
            {
                StrEmpid = listWeaverName.Items[i].Value;
            }
            else
            {
                StrEmpid = StrEmpid + "," + listWeaverName.Items[i].Value;
            }
        }
        //Check Employee Entry
        if (StrEmpid == "")
        {
            lblmsg.Text = "Plz Enter Weaver ID No...";
            return;
        }
        //*********************************************
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int Processid = 1;
            SqlParameter[] param = new SqlParameter[20];
            param[0] = new SqlParameter("@issueorderid", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hnissueorderid.Value;
            param[1] = new SqlParameter("@companyId", DDcompany.SelectedValue);
            param[2] = new SqlParameter("@Unitsid", DDProdunit.SelectedValue);
            param[3] = new SqlParameter("@Loomid", SqlDbType.Int);
            param[3].Value = 0;
            param[4] = new SqlParameter("@Unitid", DDunit.SelectedValue);
            param[5] = new SqlParameter("@Caltype", DDcaltype.SelectedValue);
            param[6] = new SqlParameter("@Receivedate", txtrecdate.Text);
            param[7] = new SqlParameter("@Empid", StrEmpid);
            int Itemfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            param[8] = new SqlParameter("@Item_finished_id", Itemfinishedid);
            param[9] = new SqlParameter("@Width", TxtWidth.Text);
            param[10] = new SqlParameter("@Length", TxtLength.Text);
            param[11] = new SqlParameter("@Area", TxtArea.Text);
            param[12] = new SqlParameter("@Qty", txtqty.Text);
            param[13] = new SqlParameter("@userid", Session["varuserid"]);
            param[14] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[15] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[15].Direction = ParameterDirection.Output;
            param[16] = new SqlParameter("@Processid", Processid);
            param[17] = new SqlParameter("@Process_rec_id", SqlDbType.Int);
            param[17].Direction = ParameterDirection.InputOutput;
            param[17].Value = hnprocessrecid.Value;
            //*****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveDirectProductionWithoutorder", param);
            lblmsg.Text = param[15].Value.ToString();
            hnissueorderid.Value = param[0].Value.ToString();
            hnprocessrecid.Value = param[17].Value.ToString();

            if (lblmsg.Text == "")
            {
                lblmsg.Text = "Data saved successfully....";
                Tran.Commit();
                lblrecno.Text = "Last Receive No. generated : " + hnprocessrecid.Value + "";
                Fillgrid();
                txtqty.Text = "";
                ddsize.SelectedIndex = -1;
            }
            else
            {
                Tran.Rollback();
            }


        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        Area();

    }

    private void Area()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {

            int SizeId = Convert.ToInt32((ddsize.SelectedValue == "" || ddsize.SelectedValue == null ? "0" : ddsize.SelectedValue));

            if (SizeId != 0)
            {

                //TdArea.Visible = true;
                SqlParameter[] _arrpara = new SqlParameter[6];
                _arrpara[0] = new SqlParameter("@size_Id", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@UnitTypeId", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@Length", SqlDbType.Float);
                _arrpara[3] = new SqlParameter("@width", SqlDbType.Float);
                _arrpara[4] = new SqlParameter("@Area", SqlDbType.Float);
                _arrpara[5] = new SqlParameter("@Shapeid", SqlDbType.Int);

                _arrpara[0].Value = SizeId;
                _arrpara[1].Value = DDunit.SelectedValue;
                _arrpara[2].Direction = ParameterDirection.Output;
                _arrpara[3].Direction = ParameterDirection.Output;
                _arrpara[4].Direction = ParameterDirection.Output;
                _arrpara[5].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_Area", _arrpara);
                //TxtLength.Text = string.Format("{0:#0.00}", _arrpara[2].Value);
                //TxtWidth.Text = string.Format("{0:#0.00}", _arrpara[3].Value);
                TxtLength.Text = _arrpara[2].Value.ToString();
                TxtWidth.Text = _arrpara[3].Value.ToString();
                TxtArea.Text = string.Format("{0:#0.0000}", _arrpara[4].Value);
                int shapeid = Convert.ToInt16(_arrpara[5].Value);

                //hdArea.Value = string.Format("{0:#0.0000}", _arrpara[4].Value);
                if (Convert.ToInt32(DDunit.SelectedValue) == 1)
                {
                    TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), shapeid));
                    //hdArea.Value = TxtArea.Text;
                }
                if (Convert.ToInt32(DDunit.SelectedValue) == 2)
                {
                    TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), shapeid));
                    // hdArea.Value = TxtArea.Text;
                }
            }


        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssue.aspx");
        }
        finally
        {
            con.Close();
        }
    }
    private void Fillgrid()
    {
        DataSet DS = null;

        string sqlstr = "";
        string View = "V_FinishedItemDetail";
        if (variable.VarNewQualitySize == "1")
        {
            View = "V_FinishedItemDetailNew";
        }


        sqlstr = @"Select PD.Process_Rec_Detail_Id as Sr_No,vf.item_name as Item,vf.Qualityname,vf.Designname,vf.ColorName,vf.ShapeName,PD.Length,PD.Width,PD.Qty,PD.Rate,PD.Qty*PD.Area Area,PD.Amount,PD.Weight,PD.Penality,
                        case When " + Session["varMasterCompanyIDForERP"] + @"=9 Then '' Else [dbo].[F_GetstockNo_RecDetailWise](PD.Process_Rec_Id,PD.Process_Rec_Detail_Id) End TStockNo,PD.Premarks,PD.Comm,
                        PD.ActualLength,PD.ActualWidth,PD.issueorderid,Pd.issue_detail_id,PM.process_rec_id
                        From PROCESS_RECEIVE_MASTER_1 PM,PROCESS_RECEIVE_DETAIL_1 PD," + View + @" vf
                        Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=vf.Item_finished_id  
                        and  PM.Process_Rec_Id=" + hnprocessrecid.Value + "  And vf.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sr_no desc";

        DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sqlstr);
        DGRecDetail.DataSource = DS.Tables[0];
        DGRecDetail.DataBind();

    }
    protected void DGRecDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarProcess_Rec_Detail_Id = Convert.ToInt32(DGRecDetail.DataKeys[e.RowIndex].Value);
            Label lblissue_detail_id = (Label)DGRecDetail.Rows[e.RowIndex].FindControl("lblissue_detail_id");
            Label lblissueorderid = (Label)DGRecDetail.Rows[e.RowIndex].FindControl("lblissueorderid");
            Label lblprocess_rec_id = (Label)DGRecDetail.Rows[e.RowIndex].FindControl("lblprocess_rec_id");

            SqlParameter[] _array = new SqlParameter[6];
            _array[0] = new SqlParameter("@ProcessId", SqlDbType.Int);
            _array[0].Value = "1";
            _array[1] = new SqlParameter("@ReceiveDetailId", VarProcess_Rec_Detail_Id);
            _array[2] = new SqlParameter("@Issuedetailid", lblissue_detail_id.Text);
            _array[3] = new SqlParameter("@Issueorderid", lblissueorderid.Text);
            _array[4] = new SqlParameter("@process_rec_id", lblprocess_rec_id.Text);
            _array[5] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            _array[5].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_DeleteProductionReceiveWithoutorder]", _array);
            lblmsg.Text = _array[5].Value.ToString();
            Tran.Commit();
            Fillgrid();
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
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
        TDreceiveno.Visible = false;
        DGRecDetail.DataSource = null;
        DGRecDetail.DataBind();
        listWeaverName.Items.Clear();
        hnissueorderid.Value = "0";
        hnprocessrecid.Value = "0";
        if (chkEdit.Checked == true)
        {
            TDreceiveno.Visible = true;
            fillReceiveNo();
        }
    }
    protected void fillReceiveNo()
    {
        string str = @"select Distinct PRM.Process_Rec_Id,PRM.ChallanNo+ ' / '+Replace(CONVERT(nvarchar(11),PRM.ReceiveDate,106),' ','-') as ChallanNo From Process_Receive_master_1 PRM 
                        inner Join PROCESS_RECEIVE_DETAIL_1 PRD on PRM.Process_Rec_Id=PRD.Process_Rec_Id
                        inner join PROCESS_ISSUE_MASTER_1 PIM on PRD.IssueOrderId=PIM.IssueOrderId Where PRM.Companyid=" + DDcompany.SelectedValue + " and PIM.Units=" + DDProdunit.SelectedValue + " and PRM.Receivedate>='" + txtfromdate.Text + "'";
        UtilityModule.ConditionalComboFill(ref DDreceiveNo, str, true, "--Plz Select--");
    }
    protected void txtfromdate_TextChanged(object sender, EventArgs e)
    {
        fillReceiveNo();
    }
    protected void DDreceiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        string str = @"select Distinct PRD.IssueOrderId,PRD.Process_Rec_Id,EMP.Empid,EI.Empcode+'-'+EI.Empname as Empname From PROCESS_RECEIVE_DETAIL_1 PRD 
                    inner join Employee_ProcessReceiveNo EMP on PRD.Process_Rec_Id=EMP.Process_Rec_id
                    and EMP.ProcessId=1  inner join EmpInfo EI on EMP.Empid=EI.EmpId
                    WHere PRD.Process_Rec_Id=" + DDreceiveNo.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        hnissueorderid.Value = "0";
        hnprocessrecid.Value = "0";
        listWeaverName.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            hnissueorderid.Value = ds.Tables[0].Rows[0]["Issueorderid"].ToString();
            hnprocessrecid.Value = ds.Tables[0].Rows[0]["Process_rec_id"].ToString();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                listWeaverName.Items.Add(new ListItem(dr["Empname"].ToString(), dr["empid"].ToString()));
            }
            //*********
            Fillgrid();
        }
        else
        {
            lblmsg.Text = "Please select correct receive No.";
        }

    }
}