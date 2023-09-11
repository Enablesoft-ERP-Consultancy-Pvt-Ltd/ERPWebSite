using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Process_FrmMasterDyeingReceiveDetailDiamond : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }


            txtRecDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            //if (Session["canedit"].ToString() == "1")
            //{
            //    TDEdit.Visible = true;
            //}

            FillissueGrid();
        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FillCustomercode(); 
    }
    protected void FillIssueNo()
    {
        if (chkEdit.Checked == true)
        {
            string str = @"select Distinct MIM.DRID,MIM.RecChallanNo from MasterDyeingReceiveDetailDiamond MIM 
                           Where MIM.CompanyId=" + DDCompany.SelectedValue + " ";

            UtilityModule.ConditionalComboFill(ref DDissueno, str, true, "--Plz Select--");
        }
    }
    private void CHECKVALIDCONTROL()
    {
        lblmessage.Text = "";
        //if (UtilityModule.VALIDDROPDOWNLIST(DDCompany) == false)
        //{
        //    goto a;
        //}   
        if (UtilityModule.VALIDTEXTBOX(txtIndentNo) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtItemName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtQualityName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtColorName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtLotNo) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtRecQty) == false)
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

            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[13];
                param[0] = new SqlParameter("@DRID", SqlDbType.Int);
                param[0].Value = hnRecId.Value;
                param[0].Direction = ParameterDirection.InputOutput;
                param[1] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
                param[2] = new SqlParameter("@RecChallanNo", SqlDbType.VarChar, 50);
                param[2].Value = "";
                param[2].Direction = ParameterDirection.InputOutput;
                param[3] = new SqlParameter("@RecDate", txtRecDate.Text);
                param[4] = new SqlParameter("@IndentNo", txtIndentNo.Text);
                param[5] = new SqlParameter("@ItemName", txtItemName.Text);

                param[6] = new SqlParameter("@QualityName", txtQualityName.Text);
                param[7] = new SqlParameter("@ShadeColorName", txtColorName.Text);
                param[8] = new SqlParameter("@LotNo", txtLotNo.Text);
                param[9] = new SqlParameter("@RecQty", txtRecQty.Text); 
                param[10] = new SqlParameter("@userid", Session["varuserid"]);
                param[11] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
                param[12] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[12].Direction = ParameterDirection.Output;


                ///**********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveMasterDyeingReceiveDetailDiamond", param);
                //*******************
                //ViewState["reportid"] = param[0].Value.ToString();
                txtRecNo.Text = param[2].Value.ToString();
                hnRecId.Value = param[0].Value.ToString();
                Tran.Commit();
                if (param[12].Value.ToString() != "")
                {
                    lblmessage.Text = param[12].Value.ToString();
                }
                else
                {
                    lblmessage.Text = "DATA SAVED SUCCESSFULLY.";
                    FillissueGrid();

                    txtIndentNo.Text = "";
                    txtItemName.Text = "";
                    txtQualityName.Text = "";
                    txtColorName.Text = "";
                    txtLotNo.Text = "";
                    txtRecQty.Text = "";
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

    }
    protected void FillissueGrid()
    {
        string str = @"Select MPR.DRID,MPR.CompanyId,MPR.RecChallanNo,MPR.RecDate,MPR.IndentNo,MPR.ItemName, MPR.QualityName,MPR.ShadeColorName,MPR.LotNo,isnull(MPR.RecQty,0) as RecQty
                        From MasterDyeingReceiveDetailDiamond MPR(NoLock)                      
                        Where MPR.CompanyId = " + DDCompany.SelectedValue;

        //if (txtEditIssueNo.Text != "")
        //{
        //    str = str + " and MPR.RecChallanNo='" + txtEditIssueNo.Text + "'";
        //}
        //else
        //{
        //    str = str + " and MPR.PRID=" + hnRecId.Value + "";
        //}

        str = str + " Order by MPR.DRID desc";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvdetail.DataSource = ds.Tables[0];
            gvdetail.DataBind();
        }
        else
        {
            gvdetail.DataSource = null;
            gvdetail.DataBind();
        }
        if (chkEdit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtRecNo.Text = ds.Tables[0].Rows[0]["RecChallanNo"].ToString();
                txtRecDate.Text = ds.Tables[0].Rows[0]["RecDate"].ToString();
                FillIssueNo();
                DDissueno.SelectedValue = ds.Tables[0].Rows[0]["DRID"].ToString();

            }
            else
            {
                txtRecNo.Text = "";
                txtRecDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        //SqlParameter[] param = new SqlParameter[2];
        //param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
        //param[1] = new SqlParameter("@Id", hnissueid.Value);
        ////************
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_PunchCardIndentOrderReport", param);
        //if (ds.Tables[0].Rows.Count > 0)
        //{

        //    Session["rptFileName"] = "~\\Reports\\RptPunchCardIndentOrderReport.rpt";
        //    Session["Getdataset"] = ds;
        //    Session["dsFileName"] = "~\\ReportSchema\\RptPunchCardIndentOrderReport.xsd";
        //    StringBuilder stb = new StringBuilder();
        //    stb.Append("<script>");
        //    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
        //    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        //}

    }
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
        if (chkEdit.Checked == true)
        {
            TDEditIssueNo.Visible = true;
            TDIssueNo.Visible = true;
            DDissueno.SelectedIndex = -1;
            gvdetail.DataSource = null;
            gvdetail.DataBind();
            txtRecNo.Text = "";
            txtRecDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

        }
        else
        {
            TDEditIssueNo.Visible = false;
            txtEditIssueNo.Text = "";
            TDIssueNo.Visible = false;
            DDissueno.SelectedIndex = -1;
            gvdetail.DataSource = null;
            gvdetail.DataBind();
            txtRecNo.Text = "";
            txtRecDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void txtEditIssueNo_TextChanged(object sender, EventArgs e)
    {
        FillissueGrid();
        DDissueno_SelectedIndexChanged(sender, new EventArgs());
    }
    protected void DDissueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkEdit.Checked == true)
        {
            txtEditIssueNo.Text = "";
        }

        hnRecId.Value = DDissueno.SelectedIndex > 0 ? DDissueno.SelectedValue : "0";

        FillissueGrid();
    }
    protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow && gvdetail.EditIndex == e.Row.RowIndex)
        //{
        
        //}
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
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}
        //SqlTransaction Tran = con.BeginTransaction();
        //try
        //{
        //    Label lblhqty = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblhqty");
        //    Label lblId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblId");
        //    Label lblDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblDetailId");
        //    Label lblOrderId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblOrderId");
        //    TextBox txtqty = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtqty");
        //    Label lblItemFinishedId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblItemFinishedId");
        //    DropDownList DDMapType = (DropDownList)gvdetail.Rows[e.RowIndex].FindControl("DDMapType");
        //    Label lblMapStencilType = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblMapStencilType");
        //    TextBox txtremarks = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtRemarks");
        //    TextBox txtMapIssueRate = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtMapIssueRate");
        //    Label lblMapArea = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblMapArea");


        //    //**************
        //    SqlParameter[] param = new SqlParameter[14];
        //    param[0] = new SqlParameter("@Id", lblId.Text);
        //    param[1] = new SqlParameter("@DetailId", lblDetailId.Text);
        //    param[2] = new SqlParameter("@OrderId", lblOrderId.Text);
        //    param[3] = new SqlParameter("@hqty", lblhqty.Text);
        //    param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
        //    param[4].Direction = ParameterDirection.Output;
        //    param[5] = new SqlParameter("@MapIssueQty", txtqty.Text == "" ? "0" : txtqty.Text);
        //    param[6] = new SqlParameter("@ItemFinishedId", lblItemFinishedId.Text);
        //    param[7] = new SqlParameter("@userid", Session["varuserid"]);
        //    param[8] = new SqlParameter("@MapIssueType", DDMapType.SelectedValue);
        //    param[9] = new SqlParameter("@MapStencilType", lblMapStencilType.Text);
        //    param[10] = new SqlParameter("@MapRemarks", txtremarks.Text);
        //    param[11] = new SqlParameter("@MapIssueRate", txtMapIssueRate.Text == "" ? "0" : txtMapIssueRate.Text);
        //    param[12] = new SqlParameter("@RateCalcType", DDRateCalcType.SelectedValue);
        //    param[13] = new SqlParameter("@MapIssueArea", lblMapArea.Text);
        //    //*************
        //    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEMAPISSUE", param);
        //    lblmessage.Text = param[4].Value.ToString();
        //    Tran.Commit();
        //    gvdetail.EditIndex = -1;
        //    FillissueGrid();
        //    Fillgrid();
        //}
        //catch (Exception ex)
        //{
        //    lblmessage.Text = ex.Message;
        //    Tran.Rollback();
        //}
        //finally
        //{
        //    con.Dispose();
        //    con.Close();
        //}
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
            Label lblDRID = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblDRID");           

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@DRID", lblDRID.Text);
            param[1] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            param[2] = new SqlParameter("@UserID", Session["varuserid"]);
            param[3] = new SqlParameter("@MasterCompanyId", Session["VarCompanyId"]);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DeleteMasterDyeingReceiveDetailDiamond", param);
            lblmessage.Text = param[4].Value.ToString();
            Tran.Commit();
            FillissueGrid();           
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