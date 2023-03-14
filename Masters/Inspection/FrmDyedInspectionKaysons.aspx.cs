using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

public partial class Masters_Inspection_FrmDyedInspectionKaysons : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CI.CompanyId,CI.CompanyName from CompanyInfo CI inner join Company_Authentication CA on Ci.CompanyId=CA.CompanyId
                  WHere CI.MasterCompanyid=" + Session["varcompanyid"] + " and CA.UserId=" + Session["varuserid"] + @"  order by CompanyName 
                    Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompanyName, ds, 0, true, "Plz Select--");

            if (DDcompanyName.Items.Count > 0)
            {
                DDcompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 1, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            txtdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            switch (Session["usertype"].ToString())
            {
                case "1":
                case "2":
                    btnApprove.Visible = true;
                    Changeapprovebuttoncolor(0);
                    break;
                default:
                    btnApprove.Visible = false;
                    break;
            }
        }
    }

    protected void DDDocNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hndocid.Value = DDDocNo.SelectedValue;
        refreshcontrol();
        FillDataback();
    }
    protected void FillDataback()
    {
        string str = @"SELECT RIM.DOCNO,RID.SUPPLIERNAME,REPLACE(CONVERT(NVARCHAR(11),RIM.REPORTDATE,106),' ','-') AS REPORTDATE,RID.CHALLANNO_DATE,RID.YARNTYPE,RID.COUNT,RID.LOTNO,
                        RID.totalQty,TotalOkQyt, TotalNotOkQty, RID.SAMPLESIZE,RID.NOOFHANK,
                        RIM.COMMENTS,RIM.STATUS,RIM.Approvestatus,RID.TagNo,RIM.AcceptedAreaStatus,isnull(RID.InwardsNo,'') as InwardsNo
                        ,isnull(RID.ImagePhoto,'') as ImagePhoto, Cone, RejectedAreaStatus
                        FROM DYEDYARNINSPECTIONMASTERKaysons RIM 
                        INNER JOIN DYEDYARNINSPECTIONDETAILKaysons RID ON RIM.DOCID=RID.DOCID 
                        Where RIM.DOCID=" + hndocid.Value;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtdocno.Text = ds.Tables[0].Rows[0]["DocNo"].ToString();
            txtsuppliername.Text = ds.Tables[0].Rows[0]["Suppliername"].ToString();
            txtdate.Text = ds.Tables[0].Rows[0]["ReportDate"].ToString();
            txtchallannodate.Text = ds.Tables[0].Rows[0]["ChallanNo_Date"].ToString();
            txtyarntype.Text = ds.Tables[0].Rows[0]["YarnType"].ToString();
            txtcount.Text = ds.Tables[0].Rows[0]["count"].ToString();
            txtlotno.Text = ds.Tables[0].Rows[0]["Lotno"].ToString();
            txttotalQty.Text = ds.Tables[0].Rows[0]["totalQty"].ToString();
            txtsamplesize.Text = ds.Tables[0].Rows[0]["Samplesize"].ToString();
            txtnoofhank.Text = ds.Tables[0].Rows[0]["NoofHank"].ToString();
            txtCone.Text = ds.Tables[0].Rows[0]["Cone"].ToString();
            txtTagNo.Text = ds.Tables[0].Rows[0]["TagNo"].ToString();
            txtTotalOkQty.Text = ds.Tables[0].Rows[0]["TotalOkQyt"].ToString();
            txtTotalNotOkQty.Text = ds.Tables[0].Rows[0]["TotalNotOkQty"].ToString();

            bindCheckPointControls();
           
            txtInwardsNo.Text = ds.Tables[0].Rows[0]["InwardsNo"].ToString();
            //
            txtcomments.Text = ds.Tables[0].Rows[0]["comments"].ToString();
            //ddresult.SelectedItem.Text = ds.Tables[0].Rows[0]["status"].ToString();
            if (ddresult.Items.FindByText(ds.Tables[0].Rows[0]["status"].ToString()) != null)
            {
                ddresult.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
            }

            if (DDAcceptedArea.Items.FindByText(ds.Tables[0].Rows[0]["AcceptedAreaStatus"].ToString()) != null)
            {
                DDAcceptedArea.SelectedValue = ds.Tables[0].Rows[0]["AcceptedAreaStatus"].ToString();
            }
            if (DDRejectedArea.Items.FindByText(ds.Tables[0].Rows[0]["RejectedAreaStatus"].ToString()) != null)
            {
                DDRejectedArea.SelectedValue = ds.Tables[0].Rows[0]["RejectedAreaStatus"].ToString();
            }
            if (ds.Tables[0].Rows[0]["ImagePhoto"].ToString() != "")
            {
                lblphotoimage.ImageUrl = ds.Tables[0].Rows[0]["ImagePhoto"].ToString() + "?" + DateTime.Now.Ticks.ToString();
            }
            else
            {
                lblphotoimage.ImageUrl = null;
            }

            Changeapprovebuttoncolor(Convert.ToInt16(ds.Tables[0].Rows[0]["approvestatus"]));
            EditRights_Button(Convert.ToInt16(Session["usertype"]), Convert.ToInt16(ds.Tables[0].Rows[0]["approvestatus"]));

        }
    }
    protected void Changeapprovebuttoncolor(int approvestatus = 0)
    {
        switch (approvestatus)
        {
            case 1:
                btnApprove.BackColor = System.Drawing.Color.Green;
                break;
            default:
                btnApprove.BackColor = System.Drawing.Color.Red;
                break;
        }
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        TDSupplierSearch.Visible = false;
        TDcountsearch.Visible = false;
        Tdlotno.Visible = false;
        TDDocno.Visible = false;
        TdVenLotNo.Visible = false;

        hndocid.Value = "0";
        DDDocNo.Items.Clear();
        refreshcontrol();
        if (chkedit.Checked == true)
        {
            TDDocno.Visible = true;
            TDSupplierSearch.Visible = true;
            TDcountsearch.Visible = true;
            Tdlotno.Visible = true;
            TdVenLotNo.Visible = true;
            fillDocno();
        }
    }
    protected void EditRights_Button(int usertype, int approvestatus = 0)
    {
        switch (usertype)
        {
            case 1:
            case 2:
                btnsave.Visible = true;
                btndelete.Visible = true;
                break;
            default:
                if (approvestatus == 1)
                {
                    btnsave.Visible = false;
                    btndelete.Visible = false;
                }
                else
                {
                    btnsave.Visible = true;
                    btndelete.Visible = true;
                }
                break;
        }
    }
    private void refreshcontrol()
    {
        txtsuppliername.Text = "";
        txtdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        txtchallannodate.Text = "";
        txtyarntype.Text = "";
        txtcount.Text = "";
        txtlotno.Text = "";
        txttotalQty.Text = "";
        txtsamplesize.Text = "";
        txtnoofhank.Text = "";
        txtCone.Text = "";
        txtTagNo.Text = "";
        txtcomments.Text = "";
        txtInwardsNo.Text = "";

        refreshCheckPOintControls();
        lblphotoimage.ImageUrl = null;
        lblmsg.Text = string.Empty;
        txtdocno.Text = string.Empty;
        txtTotalOkQty.Text = string.Empty;
        txtTotalNotOkQty.Text = string.Empty;
        
    }
    private void refreshCheckPOintControls()
    {
        txtSpecification1.Text = "";
        txtVal1_1.Text = "";
        txtVal1_2.Text = "";
        txtVal1_3.Text = "";
        txtVal1_4.Text = "";
        txtVal1_5.Text = "";
        txtRemark1.Text = "";

        txtSpecification2.Text = "";
        txtVal2_1.Text = "";
        txtVal2_2.Text = "";
        txtVal2_3.Text = "";
        txtVal2_4.Text = "";
        txtVal2_5.Text = "";
        txtRemark2.Text = "";

        txtSpecification3.Text = "";
        txtVal3_1.Text = "";
        txtVal3_2.Text = "";
        txtVal3_3.Text = "";
        txtVal3_4.Text = "";
        txtVal3_5.Text = "";
        txtRemark3.Text = "";

        txtSpecification4.Text = "";
        txtVal4_1.Text = "";
        txtVal4_2.Text = "";
        txtVal4_3.Text = "";
        txtVal4_4.Text = "";
        txtVal4_5.Text = "";
        txtRemark4.Text = "";

        txtSpecification5.Text = "";
        txtVal5_1.Text = "";
        txtVal5_2.Text = "";
        txtVal5_3.Text = "";
        txtVal5_4.Text = "";
        txtVal5_5.Text = "";
        txtRemark5.Text = "";

        txtSpecification6.Text = "";
        txtVal6_1.Text = "";
        txtVal6_2.Text = "";
        txtVal6_3.Text = "";
        txtVal6_4.Text = "";
        txtVal6_5.Text = "";
        txtRemark6.Text = "";

        txtSpecification7.Text = "";
        txtVal7_1.Text = "";
        txtVal7_2.Text = "";
        txtVal7_3.Text = "";
        txtVal7_4.Text = "";
        txtVal7_5.Text = "";
        txtRemark7.Text = "";

        txtSpecification8.Text = "";
        txtVal8_1.Text = "";
        txtVal8_2.Text = "";
        txtVal8_3.Text = "";
        txtVal8_4.Text = "";
        txtVal8_5.Text = "";
        txtRemark8.Text = "";

        txtSpecification9.Text = "";
        txtVal9_1.Text = "";
        txtVal9_2.Text = "";
        txtVal9_3.Text = "";
        txtVal9_4.Text = "";
        txtVal9_5.Text = "";
        txtRemark9.Text = "";
    }
    protected void bindCheckPointControls()
    {
        string str = @"select DYM.ID as CheckPointId,  DYD.ID as chkPointDetailsId, * from DyedYarnInspectionCheckpointsMaster DYM Left Join DyedYarnInspectionCheckpointsDetails DYD on DYM.ID = DYD.ChechPointId Where DOCID=" + hndocid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                switch (Convert.ToInt32(row["CheckPointId"]))
                {
                    case 1:
                        txtSpecification1.Text = row["Specification"].ToString();
                        txtVal1_1.Text = row["Val_1"].ToString();
                        txtVal1_2.Text = row["Val_2"].ToString();
                        txtVal1_3.Text = row["Val_3"].ToString();
                        txtVal1_4.Text = row["Val_4"].ToString();
                        txtVal1_5.Text = row["Val_5"].ToString();
                        txtRemark1.Text = row["Remark"].ToString();
                        break;
                    case 2:
                        txtSpecification2.Text = row["Specification"].ToString();
                        txtVal2_1.Text = row["Val_1"].ToString();
                        txtVal2_2.Text = row["Val_2"].ToString();
                        txtVal2_3.Text = row["Val_3"].ToString();
                        txtVal2_4.Text = row["Val_4"].ToString();
                        txtVal2_5.Text = row["Val_5"].ToString();
                        txtRemark2.Text = row["Remark"].ToString();
                        break;
                    case 3:
                        txtSpecification3.Text = row["Specification"].ToString();
                        txtVal3_1.Text = row["Val_1"].ToString();
                        txtVal3_2.Text = row["Val_2"].ToString();
                        txtVal3_3.Text = row["Val_3"].ToString();
                        txtVal3_4.Text = row["Val_4"].ToString();
                        txtVal3_5.Text = row["Val_5"].ToString();
                        txtRemark3.Text = row["Remark"].ToString();
                        break;
                    case 4:
                        txtSpecification4.Text = row["Specification"].ToString();
                        txtVal4_1.Text = row["Val_1"].ToString();
                        txtVal4_2.Text = row["Val_2"].ToString();
                        txtVal4_3.Text = row["Val_3"].ToString();
                        txtVal4_4.Text = row["Val_4"].ToString();
                        txtVal4_5.Text = row["Val_5"].ToString();
                        txtRemark4.Text = row["Remark"].ToString();
                        break;
                    case 5:
                        txtSpecification5.Text = row["Specification"].ToString();
                        txtVal5_1.Text = row["Val_1"].ToString();
                        txtVal5_2.Text = row["Val_2"].ToString();
                        txtVal5_3.Text = row["Val_3"].ToString();
                        txtVal5_4.Text = row["Val_4"].ToString();
                        txtVal5_5.Text = row["Val_5"].ToString();
                        txtRemark5.Text = row["Remark"].ToString();
                        break;
                    case 6:
                        txtSpecification6.Text = row["Specification"].ToString();
                        txtVal6_1.Text = row["Val_1"].ToString();
                        txtVal6_2.Text = row["Val_2"].ToString();
                        txtVal6_3.Text = row["Val_3"].ToString();
                        txtVal6_4.Text = row["Val_4"].ToString();
                        txtVal6_5.Text = row["Val_5"].ToString();
                        txtRemark6.Text = row["Remark"].ToString();
                        break;
                    case 7:
                        txtSpecification7.Text = row["Specification"].ToString();
                        txtVal7_1.Text = row["Val_1"].ToString();
                        txtVal7_2.Text = row["Val_2"].ToString();
                        txtVal7_3.Text = row["Val_3"].ToString();
                        txtVal7_4.Text = row["Val_4"].ToString();
                        txtVal7_5.Text = row["Val_5"].ToString();
                        txtRemark7.Text = row["Remark"].ToString();
                        break;
                    case 8:
                        txtSpecification8.Text = row["Specification"].ToString();
                        txtVal8_1.Text = row["Val_1"].ToString();
                        txtVal8_2.Text = row["Val_2"].ToString();
                        txtVal8_3.Text = row["Val_3"].ToString();
                        txtVal8_4.Text = row["Val_4"].ToString();
                        txtVal8_5.Text = row["Val_5"].ToString();
                        txtRemark8.Text = row["Remark"].ToString();
                        break;
                    case 9:
                        txtSpecification9.Text = row["Specification"].ToString();
                        txtVal9_1.Text = row["Val_1"].ToString();
                        txtVal9_2.Text = row["Val_2"].ToString();
                        txtVal9_3.Text = row["Val_3"].ToString();
                        txtVal9_4.Text = row["Val_4"].ToString();
                        txtVal9_5.Text = row["Val_5"].ToString();
                        txtRemark9.Text = row["Remark"].ToString();
                        break;
                }
            }
        }

    }
    protected void btnsave_Click(object sender, EventArgs e)
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
            SqlParameter[] param = new SqlParameter[11];
            param[0] = new SqlParameter("@Docid", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hndocid.Value;
            param[1] = new SqlParameter("@Companyid", DDcompanyName.SelectedValue);
            param[2] = new SqlParameter("@DocNo", SqlDbType.VarChar, 50);
            param[2].Value = txtdocno.Text;
            param[2].Direction = ParameterDirection.InputOutput;
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@Comments", txtcomments.Text.Trim());
            param[6] = new SqlParameter("@Status", ddresult.SelectedItem.Text);
            param[7] = new SqlParameter("@ReportDate", txtdate.Text);
            param[8] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);
            param[9] = new SqlParameter("@AcceptedAreaStatus", DDAcceptedArea.SelectedItem.Text);
            param[10] = new SqlParameter("@RejectedAreaStatus", DDRejectedArea.SelectedItem.Text);

            //RejectedAreaStatus

            //*********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEDYEDYARNINSPECTIONKaysons", param);
            lblmsg.Text = param[4].Value.ToString();
            txtdocno.Text = param[2].Value.ToString();
            hndocid.Value = param[0].Value.ToString();

            // at the time of update delete all the data in tables
            string str1 = @"DELETE FROM DYEDYARNINSPECTIONDETAILKaysons WHERE DOCID=" + hndocid.Value;
            str1 = str1 + @"DELETE FROM DyedYarnInspectionCheckpointsDetails WHERE DOCID=" + hndocid.Value;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);

            insertinto_RAWYARNINSPECTIONDETAIL(Tran);

            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + param[4].Value.ToString() + "')", true);

            Tran.Commit();
            SaveImage(Convert.ToInt32(hndocid.Value));
            //**********

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

    private void insertinto_RAWYARNINSPECTIONDETAIL(SqlTransaction Tran)
    {
        string str = string.Empty;
        str = @"Insert into DYEDYARNINSPECTIONDETAILKaysons (DOCID, SUPPLIERNAME, CHALLANNO_DATE, Color, TotalOkQyt, TotalNotOkQty, YARNTYPE, COUNT, LOTNO, 
                     TagNO, totalQty, SAMPLESIZE, NOOFHANK, InwardsNo, Cone )
                     values(" + hndocid.Value + ",'" + txtsuppliername.Text.Replace("'", "''") + "','" + txtchallannodate.Text.Replace("'", "''")
                       + "','" + txtColor.Text.Replace("'", "''") + "','" + txtTotalOkQty.Text.Replace("'", "''")
                       + "','" + txtTotalNotOkQty.Text.Replace("'", "''") + "','" + txtyarntype.Text.Replace("'", "''") + "','" + txtcount.Text.Replace("'", "''") + "','" + txtlotno.Text.Replace("'", "''")
                       + "','" + txtTagNo.Text.Replace("'", "''") + @"'," + (txttotalQty.Text == "" ? "0" : txttotalQty.Text) + ", " + (txtsamplesize.Text == "" ? "0" : txtsamplesize.Text) + "," + (txtnoofhank.Text == "" ? "0" : txtnoofhank.Text)
                      + ",'" + txtInwardsNo.Text.Replace("'", "''") + "','" + txtCone.Text.Replace("'", "''") + "')";


        str =  str + @"Insert Into DyedYarnInspectionCheckpointsDetails (DOCID, ChechPointId, Specification, Val_1, Val_2, Val_3, Val_4, Val_5, Remark)  Values("
            + hndocid.Value + "," + lblSrNo1.Text + ",'" + txtSpecification1.Text + "','" + txtVal1_1.Text + "','" + txtVal1_2.Text + "','" + txtVal1_3.Text + "','" + txtVal1_4.Text + "','" + txtVal1_5.Text + "','" + txtRemark1.Text + "')";

        str = str + @"Insert Into DyedYarnInspectionCheckpointsDetails (DOCID, ChechPointId, Specification, Val_1, Val_2, Val_3, Val_4, Val_5, Remark)  Values("
            + hndocid.Value + "," + lblSrNo2.Text + ",'" + txtSpecification2.Text + "','" + txtVal2_1.Text + "','" + txtVal2_2.Text + "','" + txtVal2_3.Text + "','" + txtVal2_4.Text + "','" + txtVal2_5.Text + "','" + txtRemark2.Text + "')";

        str = str + @"Insert Into DyedYarnInspectionCheckpointsDetails (DOCID, ChechPointId, Specification, Val_1, Val_2, Val_3, Val_4, Val_5, Remark)  Values("
            + hndocid.Value + "," + lblSrNo3.Text + ",'" + txtSpecification3.Text + "','" + txtVal3_1.Text + "','" + txtVal3_2.Text + "','" + txtVal3_3.Text + "','" + txtVal3_4.Text + "','" + txtVal3_5.Text + "','" + txtRemark3.Text + "')";

        str = str + @"Insert Into DyedYarnInspectionCheckpointsDetails (DOCID, ChechPointId, Specification, Val_1, Val_2, Val_3, Val_4, Val_5, Remark)  Values("
            + hndocid.Value + "," + lblSrNo4.Text + ",'" + txtSpecification4.Text + "','" + txtVal4_1.Text + "','" + txtVal4_2.Text + "','" + txtVal4_3.Text + "','" + txtVal4_4.Text + "','" + txtVal4_5.Text + "','" + txtRemark4.Text + "')";

        str = str + @"Insert Into DyedYarnInspectionCheckpointsDetails (DOCID, ChechPointId, Specification, Val_1, Val_2, Val_3, Val_4, Val_5, Remark)  Values("
            + hndocid.Value + "," + lblSrNo5.Text + ",'" + txtSpecification5.Text + "','" + txtVal5_1.Text + "','" + txtVal5_2.Text + "','" + txtVal5_3.Text + "','" + txtVal5_4.Text + "','" + txtVal5_5.Text + "','" + txtRemark5.Text + "')";

        str = str + @"Insert Into DyedYarnInspectionCheckpointsDetails (DOCID, ChechPointId, Specification, Val_1, Val_2, Val_3, Val_4, Val_5, Remark)  Values("
            + hndocid.Value + "," + lblSrNo6.Text + ",'" + txtSpecification6.Text + "','" + txtVal6_1.Text + "','" + txtVal6_2.Text + "','" + txtVal6_3.Text + "','" + txtVal6_4.Text + "','" + txtVal6_5.Text + "','" + txtRemark6.Text + "')";

        str = str + @"Insert Into DyedYarnInspectionCheckpointsDetails (DOCID, ChechPointId, Specification, Val_1, Val_2, Val_3, Val_4, Val_5, Remark)  Values("
            + hndocid.Value + "," + lblSrNo7.Text + ",'" + txtSpecification7.Text + "','" + txtVal7_1.Text + "','" + txtVal7_2.Text + "','" + txtVal7_3.Text + "','" + txtVal7_4.Text + "','" + txtVal7_5.Text + "','" + txtRemark7.Text + "')";

        str = str + @"Insert Into DyedYarnInspectionCheckpointsDetails (DOCID, ChechPointId, Specification, Val_1, Val_2, Val_3, Val_4, Val_5, Remark)  Values("
            + hndocid.Value + "," + lblSrNo8.Text + ",'" + txtSpecification8.Text + "','" + txtVal8_1.Text + "','" + txtVal8_2.Text + "','" + txtVal8_3.Text + "','" + txtVal8_4.Text + "','" + txtVal8_5.Text + "','" + txtRemark8.Text + "')";

        str = str + @"Insert Into DyedYarnInspectionCheckpointsDetails (DOCID, ChechPointId, Specification, Val_1, Val_2, Val_3, Val_4, Val_5, Remark)  Values("
            + hndocid.Value + "," + lblSrNo9.Text + ",'" + txtSpecification9.Text + "','" + txtVal9_1.Text + "','" + txtVal9_2.Text + "','" + txtVal9_3.Text + "','" + txtVal9_4.Text + "','" + txtVal9_5.Text + "','" + txtRemark9.Text + "')";



        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
    }
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        fillDocno();
    }
    private void fillDocno()
    {
        string str = @"SELECT  RIM.DOCID,RIM.DOCNO +' # ' +Replace(convert(nvarchar(11),RIM.Reportdate,106),' ','-') as DocNo 
                    FROM DyedyarnInspectionMasterkaysons RIM(nolock) 
                    INNER JOIN RAWYARNINSPECTIONDETAIL RID(nolock) ON RIM.DOCID=RID.DOCID
                    Where RIM.COMPANYID=" + DDcompanyName.SelectedValue + " And RIM.BranchID = " + DDBranchName.SelectedValue;
        if (txtsuppliersearch.Text != "")
        {
            str = str + " and RID.Suppliername like '" + txtsuppliersearch.Text.Trim() + "%'";
        }
        if (txtcountsearch.Text != "")
        {
            str = str + " and RID.Count like '" + txtcountsearch.Text.Trim() + "%'";
        }
        if (txtsearchlotno.Text != "")
        {
            str = str + " and RID.Lotno like '" + txtsearchlotno.Text.Trim() + "%'";
        }
        if (TxtSearchVenLotNo.Text != "")
        {
            str = str + " and RID.TagNo like '" + TxtSearchVenLotNo.Text.Trim() + "%'";
        }
        str = str + " order by DOCID";
        UtilityModule.ConditionalComboFill(ref DDDocNo, str, true, "--Plz Select--");
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Docid", hndocid.Value);

            //            string str = @"SELECT Ci.companyName,* FROM RAWYARNINSPECTIONMASTER RIM INNER JOIN RAWYARNINSPECTIONDETAIL RID ON RIM.DOCID=RID.DOCID 
            //                           inner join CompanyInfo ci on RIM.COMPANYID=ci.CompanyId Where RIM.Docid=" + hndocid.Value;

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETDYEDYARNINSPECTIONREPORTKaysons", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    row["ImagePhoto"] = Server.MapPath(row["ImagePhoto"].ToString());
                }
                Session["rptFileName"] = "~\\Reports\\RptDyedYarnInspectionKaysons.rpt";
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptDyedYarnInspectionKaysons.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt1", "alert('No records found.')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void btndelete_Click(object sender, EventArgs e)
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
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@Docid", hndocid.Value);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEDYEDYARNINSPECTIONKaysons", param);
            Tran.Commit();
            if (param[3].Value.ToString() != "")
            {
                lblmsg.Text = param[3].Value.ToString();
            }
            else
            {
                lblmsg.Text = "DOC No. Deleted Successfully.";
                fillDocno();
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
    protected void DDcompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkedit.Checked == true)
        {
            fillDocno();
        }
    }


    protected void btnApprove_Click(object sender, EventArgs e)
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
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@Docid", hndocid.Value);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_APPROVEDYEDYARNINSPECTIONKaysons", param);
            Tran.Commit();
            lblmsg.Text = param[3].Value.ToString();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altappv", "alert('" + param[3].Value.ToString() + "')", true);
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

    protected void BtnSearchVenLotNo_Click(object sender, EventArgs e)
    {
        fillDocno();
    }

    protected void SaveImage(int hndocid)
    {

        if (fileuploadphoto.FileName != "")
        {
            string filename = Path.GetFileName(fileuploadphoto.PostedFile.FileName);
            string Folderpath = Server.MapPath("../../Item_Image");
            //Check folder
            if (!Directory.Exists(Folderpath))
            {
                Directory.CreateDirectory(Folderpath);
            }
            //
            string targetPath = Server.MapPath("../../Item_Image/" + hndocid + "_photo.gif");

            FileInfo file = new FileInfo(targetPath);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
            }

            string img = "~\\Item_Image\\" + hndocid + "_photo.gif";
            //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
            Stream strm = fileuploadphoto.PostedFile.InputStream;
            var targetFile = targetPath;
            if (fileuploadphoto.FileName != null && fileuploadphoto.FileName != "")
            {
                GenerateThumbnails(0.4, strm, targetFile);
            }
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update DYEDYARNINSPECTIONDETAILKaysons Set ImagePhoto='" + img + "' Where Docid=" + hndocid + "");
            lblphotoimage.ImageUrl = img + "?" + DateTime.Now.Ticks.ToString(); ;
        }
    }
    private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
    {
        using (var image = System.Drawing.Image.FromStream(sourcePath))
        {
            var newWidth = (int)(image.Width * scaleFactor);
            var newHeight = (int)(image.Height * scaleFactor);
            var thumbnailImg = new Bitmap(newWidth, newHeight);
            var thumbGraph = Graphics.FromImage(thumbnailImg);
            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbGraph.DrawImage(image, imageRectangle);
            thumbnailImg.Save(targetPath, image.RawFormat);
        }
    }


}