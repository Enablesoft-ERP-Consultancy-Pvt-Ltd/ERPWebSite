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

public partial class Masters_Inspection_FrmYarnInspectionNew : System.Web.UI.Page
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
                        RID.TOTALBALE,RID.SAMPLESIZE,RID.NOOFHANK,
                        RID.SPECIFICATION_1,RID.ONE_1,RID.TWO_1,RID.THREE_1,RID.FOUR_1,RID.FIVE_1,RID.AVGVALUE_1,
                        RID.SPECIFICATION_2,RID.ONE_2,RID.TWO_2,RID.THREE_2,RID.FOUR_2,RID.FIVE_2,RID.AVGVALUE_2,
                        RID.SPECIFICATION_3,RID.ONE_3,RID.TWO_3,RID.THREE_3,RID.FOUR_3,RID.FIVE_3,RID.AVGVALUE_3,
                        RID.SPECIFICATIONPET_4,RID.ONEPET_4,RID.TWOPET_4,RID.THREEPET_4,RID.FOURPET_4,RID.FIVEPET_4,RID.AVGVALUEPET_4,
                        RID.SPECIFICATIONOTHER_4,RID.ONEOTHER_4,RID.TWOOTHER_4,RID.THREEOTHER_4,RID.FOUROTHER_4,RID.FIVEOTHER_4,RID.AVGVALUEOTHER_4,
                        RID.SPECIFICATION_5,RID.ONE_5,RID.TWO_5,RID.THREE_5,RID.FOUR_5,RID.FIVE_5,RID.AVGVALUE_5,
                        RID.SPECIFICATION_6,RID.ONE_6,RID.TWO_6,RID.THREE_6,RID.FOUR_6,RID.FIVE_6,RID.AVGVALUE_6,
                        RID.SPECIFICATION_7,RID.ONE_7,RID.TWO_7,RID.THREE_7,RID.FOUR_7,RID.FIVE_7,RID.AVGVALUE_7,
                        RIM.COMMENTS,RIM.STATUS,CHECKPOINTPET_4,CHECKPOINTOTHER_4,RIM.Approvestatus,RID.VenderLotNo,
                        RID.SPECIFICATION_8,RID.ONE_8,RID.TWO_8,RID.THREE_8,RID.FOUR_8,RID.FIVE_8,RID.AVGVALUE_8,
                        RID.SPECIFICATION_9,RID.ONE_9,RID.TWO_9,RID.THREE_9,RID.FOUR_9,RID.FIVE_9,RID.AVGVALUE_9,RIM.AcceptedAreaStatus,isnull(RID.InwardsNo,'') as InwardsNo
                        ,isnull(RID.ImagePhoto,'') as ImagePhoto, Cone, RejectedAreaStatus
                        FROM RAWYARNINSPECTIONMASTER RIM 
                        INNER JOIN RAWYARNINSPECTIONDETAIL RID ON RIM.DOCID=RID.DOCID 
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
            txttotalbale.Text = ds.Tables[0].Rows[0]["totalbale"].ToString();
            txtsamplesize.Text = ds.Tables[0].Rows[0]["Samplesize"].ToString();
            txtnoofhank.Text = ds.Tables[0].Rows[0]["NoofHank"].ToString();
            txtCone.Text = ds.Tables[0].Rows[0]["Cone"].ToString();
            TxtVenderLotNo.Text = ds.Tables[0].Rows[0]["VenderLotNo"].ToString();
            //1
            txtspecification_1.Text = ds.Tables[0].Rows[0]["Specification_1"].ToString();
            txt1_1.Text = ds.Tables[0].Rows[0]["One_1"].ToString();
            txt1_2.Text = ds.Tables[0].Rows[0]["Two_1"].ToString();
            txt1_3.Text = ds.Tables[0].Rows[0]["Three_1"].ToString();
            txt1_4.Text = ds.Tables[0].Rows[0]["Four_1"].ToString();
            txt1_5.Text = ds.Tables[0].Rows[0]["Five_1"].ToString();
            txtavgvalue_1.Text = ds.Tables[0].Rows[0]["Avgvalue_1"].ToString();
            //2
            txtspecification_2.Text = ds.Tables[0].Rows[0]["Specification_2"].ToString();
            txt2_1.Text = ds.Tables[0].Rows[0]["One_2"].ToString();
            txt2_2.Text = ds.Tables[0].Rows[0]["Two_2"].ToString();
            txt2_3.Text = ds.Tables[0].Rows[0]["Three_2"].ToString();
            txt2_4.Text = ds.Tables[0].Rows[0]["Four_2"].ToString();
            txt2_5.Text = ds.Tables[0].Rows[0]["Five_2"].ToString();
            txtavgvalue_2.Text = ds.Tables[0].Rows[0]["Avgvalue_2"].ToString();
            //3
            txtspecification_3.Text = ds.Tables[0].Rows[0]["Specification_3"].ToString();
            txt3_1.Text = ds.Tables[0].Rows[0]["One_3"].ToString();
            txt3_2.Text = ds.Tables[0].Rows[0]["Two_3"].ToString();
            txt3_3.Text = ds.Tables[0].Rows[0]["Three_3"].ToString();
            txt3_4.Text = ds.Tables[0].Rows[0]["Four_3"].ToString();
            txt3_5.Text = ds.Tables[0].Rows[0]["Five_3"].ToString();
            txtavgvalue_3.Text = ds.Tables[0].Rows[0]["Avgvalue_3"].ToString();
            //4
            txtspecificationpet_4.Text = ds.Tables[0].Rows[0]["Specificationpet_4"].ToString();
            lblcheckpointpet_4.Text = ds.Tables[0].Rows[0]["CHECKPOINTPET_4"].ToString();
            txtpet4_1.Text = ds.Tables[0].Rows[0]["Onepet_4"].ToString();
            txtpet4_2.Text = ds.Tables[0].Rows[0]["Twopet_4"].ToString();
            txtpet4_3.Text = ds.Tables[0].Rows[0]["Threepet_4"].ToString();
            txtpet4_4.Text = ds.Tables[0].Rows[0]["Fourpet_4"].ToString();
            txtpet4_5.Text = ds.Tables[0].Rows[0]["Fivepet_4"].ToString();
            txtavgvaluepet_4.Text = ds.Tables[0].Rows[0]["Avgvaluepet_4"].ToString();

            txtspecificationother_4.Text = ds.Tables[0].Rows[0]["Specificationother_4"].ToString();
            lblcheckpointother_4.Text = ds.Tables[0].Rows[0]["CHECKPOINTOTHER_4"].ToString();
            txtother4_1.Text = ds.Tables[0].Rows[0]["Oneother_4"].ToString();
            txtother4_2.Text = ds.Tables[0].Rows[0]["Twoother_4"].ToString();
            txtother4_3.Text = ds.Tables[0].Rows[0]["Threeother_4"].ToString();
            txtother4_4.Text = ds.Tables[0].Rows[0]["Fourother_4"].ToString();
            txtother4_5.Text = ds.Tables[0].Rows[0]["Fiveother_4"].ToString();
            txtavgvalueother_4.Text = ds.Tables[0].Rows[0]["AvgvalueOther_4"].ToString();
            //5
            txtspecification_5.Text = ds.Tables[0].Rows[0]["Specification_5"].ToString();
            txt5_1.Text = ds.Tables[0].Rows[0]["One_5"].ToString();
            txt5_2.Text = ds.Tables[0].Rows[0]["Two_5"].ToString();
            txt5_3.Text = ds.Tables[0].Rows[0]["Three_5"].ToString();
            txt5_4.Text = ds.Tables[0].Rows[0]["Four_5"].ToString();
            txt5_5.Text = ds.Tables[0].Rows[0]["Five_5"].ToString();
            txtavgvalue_5.Text = ds.Tables[0].Rows[0]["Avgvalue_5"].ToString();
            //5
            txtspecification_6.Text = ds.Tables[0].Rows[0]["Specification_6"].ToString();
            txt6_1.Text = ds.Tables[0].Rows[0]["One_6"].ToString();
            txt6_2.Text = ds.Tables[0].Rows[0]["Two_6"].ToString();
            txt6_3.Text = ds.Tables[0].Rows[0]["Three_6"].ToString();
            txt6_4.Text = ds.Tables[0].Rows[0]["Four_6"].ToString();
            txt6_5.Text = ds.Tables[0].Rows[0]["Five_6"].ToString();
            txtavgvalue_6.Text = ds.Tables[0].Rows[0]["Avgvalue_6"].ToString();
            //7
            txtspecification_7.Text = ds.Tables[0].Rows[0]["Specification_7"].ToString();
            txt7_1.Text = ds.Tables[0].Rows[0]["One_7"].ToString();
            txt7_2.Text = ds.Tables[0].Rows[0]["Two_7"].ToString();
            txt7_3.Text = ds.Tables[0].Rows[0]["Three_7"].ToString();
            txt7_4.Text = ds.Tables[0].Rows[0]["Four_7"].ToString();
            txt7_5.Text = ds.Tables[0].Rows[0]["Five_7"].ToString();
            txtavgvalue_7.Text = ds.Tables[0].Rows[0]["Avgvalue_7"].ToString();
            //8
            txtspecification_8.Text = ds.Tables[0].Rows[0]["Specification_8"].ToString();
            txt8_1.Text = ds.Tables[0].Rows[0]["One_8"].ToString();
            txt8_2.Text = ds.Tables[0].Rows[0]["Two_8"].ToString();
            txt8_3.Text = ds.Tables[0].Rows[0]["Three_8"].ToString();
            txt8_4.Text = ds.Tables[0].Rows[0]["Four_8"].ToString();
            txt8_5.Text = ds.Tables[0].Rows[0]["Five_8"].ToString();
            txtavgvalue_8.Text = ds.Tables[0].Rows[0]["Avgvalue_8"].ToString();
            //Below section is for OTHER
            txtspecification_9.Text = ds.Tables[0].Rows[0]["Specification_9"].ToString();
            txt9_1.Text = ds.Tables[0].Rows[0]["One_9"].ToString();
            txt9_2.Text = ds.Tables[0].Rows[0]["Two_9"].ToString();
            txt9_3.Text = ds.Tables[0].Rows[0]["Three_9"].ToString();
            txt9_4.Text = ds.Tables[0].Rows[0]["Four_9"].ToString();
            txt9_5.Text = ds.Tables[0].Rows[0]["Five_9"].ToString();
            txtavgvalue_9.Text = ds.Tables[0].Rows[0]["Avgvalue_9"].ToString();

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
                if (approvestatus==1)
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
        txttotalbale.Text = "";
        txtsamplesize.Text = "";
        txtnoofhank.Text = "";
        txtCone.Text = "";
        TxtVenderLotNo.Text = "";
        //1
        txtspecification_1.Text = "";
        txt1_1.Text = "";
        txt1_2.Text = "";
        txt1_3.Text = "";
        txt1_4.Text = "";
        txt1_5.Text = "";
        txtavgvalue_1.Text = "";
        //2
        txtspecification_2.Text = "";
        txt2_1.Text = "";
        txt2_2.Text = "";
        txt2_3.Text = "";
        txt2_4.Text = "";
        txt2_5.Text = "";
        txtavgvalue_2.Text = "";
        //3
        txtspecification_3.Text = "";
        txt3_1.Text = "";
        txt3_2.Text = "";
        txt3_3.Text = "";
        txt3_4.Text = "";
        txt3_5.Text = "";
        txtavgvalue_3.Text = "";
        //4
        txtspecificationpet_4.Text = "";
        txtpet4_1.Text = "";
        txtpet4_2.Text = "";
        txtpet4_3.Text = "";
        txtpet4_4.Text = "";
        txtpet4_5.Text = "";
        txtavgvaluepet_4.Text = "";

        txtspecificationother_4.Text = "";
        txtother4_1.Text = "";
        txtother4_2.Text = "";
        txtother4_3.Text = "";
        txtother4_4.Text = "";
        txtother4_5.Text = "";
        txtavgvalueother_4.Text = "";
        //5
        txtspecification_5.Text = "";
        txt5_1.Text = "";
        txt5_2.Text = "";
        txt5_3.Text = "";
        txt5_4.Text = "";
        txt5_5.Text = "";
        txtavgvalue_5.Text = "";
        //5
        txtspecification_6.Text = "";
        txt6_1.Text = "";
        txt6_2.Text = "";
        txt6_3.Text = "";
        txt6_4.Text = "";
        txt6_5.Text = "";
        txtavgvalue_6.Text = "";
        //7
        txtspecification_7.Text = "";
        txt7_1.Text = "";
        txt7_2.Text = "";
        txt7_3.Text = "";
        txt7_4.Text = "";
        txt7_5.Text = "";
        txtavgvalue_7.Text = "";
        //8
        txtspecification_8.Text = "";
        txt8_1.Text = "";
        txt8_2.Text = "";
        txt8_3.Text = "";
        txt8_4.Text = "";
        txt8_5.Text = "";
        txtavgvalue_8.Text = "";
        //9 [others]
        txtspecification_9.Text = "";
        txt9_1.Text = "";
        txt9_2.Text = "";
        txt9_3.Text = "";
        txt9_4.Text = "";
        txt9_5.Text = "";
        txtavgvalue_9.Text = "";
        //
        txtcomments.Text = "";

        txtInwardsNo.Text = "";

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
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVERAWYARNINSPECTION", param);
            lblmsg.Text = param[4].Value.ToString();
            txtdocno.Text = param[2].Value.ToString();
            hndocid.Value = param[0].Value.ToString();

            // at the time of update delete all the data in tables
            string str1 = @"DELETE FROM RAWYARNINSPECTIONDETAIL WHERE DOCID=" + hndocid.Value;
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
        string str = @"Insert into RAWYARNINSPECTIONDETAIL (DOCID, SUPPLIERNAME, CHALLANNO_DATE, YARNTYPE, COUNT, LOTNO, 
                     TOTALBALE, SAMPLESIZE, NOOFHANK, CHECKPOINT_1, SPECIFICATION_1, ONE_1, TWO_1, THREE_1, FOUR_1, FIVE_1, 
                     AVGVALUE_1, CHECKPOINT_2, SPECIFICATION_2, ONE_2, TWO_2, THREE_2, FOUR_2, FIVE_2, AVGVALUE_2, CHECKPOINT_3, 
                     SPECIFICATION_3, ONE_3, TWO_3, THREE_3, FOUR_3, FIVE_3, AVGVALUE_3, CHECKPOINTPET_4, SPECIFICATIONPET_4, 
                     ONEPET_4, TWOPET_4, THREEPET_4, FOURPET_4, FIVEPET_4, AVGVALUEPET_4, CHECKPOINTOTHER_4, SPECIFICATIONOTHER_4, 
                     ONEOTHER_4, TWOOTHER_4, THREEOTHER_4, FOUROTHER_4, FIVEOTHER_4, AVGVALUEOTHER_4, CHECKPOINT_5, SPECIFICATION_5, ONE_5, 
                     TWO_5, THREE_5, FOUR_5, FIVE_5, AVGVALUE_5, CHECKPOINT_6, SPECIFICATION_6, ONE_6, TWO_6, THREE_6, FOUR_6, FIVE_6, 
                     AVGVALUE_6, CHECKPOINT_7, SPECIFICATION_7, ONE_7, TWO_7, THREE_7, FOUR_7, FIVE_7, AVGVALUE_7, VenderLotNo,
                     CHECKPOINT_8, SPECIFICATION_8, ONE_8, TWO_8, THREE_8, FOUR_8, FIVE_8, AVGVALUE_8,
                     CHECKPOINT_9, SPECIFICATION_9, ONE_9, TWO_9, THREE_9, FOUR_9, FIVE_9, AVGVALUE_9,InwardsNo, Cone )
                     values(" + hndocid.Value + ",'" + txtsuppliername.Text.Replace("'", "''") + "','" + txtchallannodate.Text.Replace("'", "''") + "','" + txtyarntype.Text.Replace("'", "''") + "','" + txtcount.Text.Replace("'", "''") + "','" + txtlotno.Text.Replace("'", "''") + @"',
                     " + (txttotalbale.Text == "" ? "0" : txttotalbale.Text) + ", " + (txtsamplesize.Text == "" ? "0" : txtsamplesize.Text) + "," + (txtnoofhank.Text == "" ? "0" : txtnoofhank.Text) + ",'" + lblcheckpoint_1.Text.Replace("'", "''") + "','" + txtspecification_1.Text.Replace("'", "''") + "','" + txt1_1.Text.Replace("'", "''") + "','" + txt1_2.Text.Replace("'", "''") + "','" + txt1_3.Text.Replace("'", "''") + "','" + txt1_4.Text.Replace("'", "''") + "','" + txt1_5.Text.Replace("'", "''") + @"',
                     '" + txtavgvalue_1.Text.Replace("'", "''") + "','" + lblcheckpoint_2.Text.Replace("'", "''") + "','" + txtspecification_2.Text.Replace("'", "''") + "','" + txt2_1.Text.Replace("'", "''") + "','" + txt2_2.Text.Replace("'", "''") + "','" + txt2_3.Text.Replace("'", "''") + "','" + txt2_4.Text.Replace("'", "''") + "','" + txt2_5.Text.Replace("'", "''") + @"',
                      '" + txtavgvalue_2.Text.Replace("'", "''") + "','" + lblcheckpoint_3.Text.Replace("'", "''") + "','" + txtspecification_3.Text.Replace("'", "''") + "','" + txt3_1.Text.Replace("'", "''") + "','" + txt3_2.Text.Replace("'", "''") + "','" + txt3_3.Text.Replace("'", "''") + "','" + txt3_4.Text.Replace("'", "''") + "','" + txt3_5.Text.Replace("'", "''") + @"',
                      '" + txtavgvalue_3.Text.Replace("'", "''") + "','" + lblcheckpointpet_4.Text.Replace("'", "''") + "','" + txtspecificationpet_4.Text.Replace("'", "''") + "','" + txtpet4_1.Text.Replace("'", "''") + "','" + txtpet4_2.Text.Replace("'", "''") + "','" + txtpet4_3.Text.Replace("'", "''") + "','" + txtpet4_4.Text.Replace("'", "''") + "','" + txtpet4_5.Text.Replace("'", "''") + @"',
                    '" + txtavgvaluepet_4.Text.Replace("'", "''") + "','" + lblcheckpointother_4.Text.Replace("'", "''") + "','" + txtspecificationother_4.Text.Replace("'", "''") + "','" + txtother4_1.Text.Replace("'", "''") + "','" + txtother4_2.Text.Replace("'", "''") + "','" + txtother4_3.Text.Replace("'", "''") + "','" + txtother4_4.Text.Replace("'", "''") + "','" + txtother4_5.Text.Replace("'", "''") + @"',
                    '" + txtavgvalueother_4.Text.Replace("'", "''") + "','" + lblcheckpoint_5.Text.Replace("'", "''") + "','" + txtspecification_5.Text.Replace("'", "''") + "','" + txt5_1.Text.Replace("'", "''") + "','" + txt5_2.Text.Replace("'", "''") + "','" + txt5_3.Text.Replace("'", "''") + "','" + txt5_4.Text.Replace("'", "''") + "','" + txt5_5.Text.Replace("'", "''") + @"',
                    '" + txtavgvalue_5.Text.Replace("'", "''") + "','" + lblcheckpoint_6.Text.Replace("'", "''") + "','" + txtspecification_6.Text.Replace("'", "''") + "','" + txt6_1.Text.Replace("'", "''") + "','" + txt6_2.Text.Replace("'", "''") + "','" + txt6_3.Text.Replace("'", "''") + "','" + txt6_4.Text.Replace("'", "''") + "','" + txt6_5.Text.Replace("'", "''") + @"',
                    '" + txtavgvalue_6.Text.Replace("'", "''") + "','" + lblcheckpoint_7.Text.Replace("'", "''") + "','" + txtspecification_7.Text.Replace("'", "''") + "','" + txt7_1.Text.Replace("'", "''") + "','" + txt7_2.Text.Replace("'", "''") + "','" + txt7_3.Text.Replace("'", "''") + "','" + txt7_4.Text.Replace("'", "''") + "','" + txt7_5.Text.Replace("'", "''") + @"',
                    '" + txtavgvalue_7.Text.Replace("'", "''") + "','" + TxtVenderLotNo.Text.Replace("'", "''") + "','" + lblcheckpoint_8.Text.Replace("'", "''") + "','" + txtspecification_8.Text.Replace("'", "''") + "','" + txt8_1.Text.Replace("'", "''") + "','" + txt8_2.Text.Replace("'", "''") + "','" + txt8_3.Text.Replace("'", "''") + "','" + txt8_4.Text.Replace("'", "''") + "','" + txt8_5.Text.Replace("'", "''") + @"',
                    '" + txtavgvalue_8.Text.Replace("'", "''") + "','" + lblcheckpoint_9.Text.Replace("'", "''") + "','" + txtspecification_9.Text.Replace("'", "''") + "','" + txt9_1.Text.Replace("'", "''") + "','" + txt9_2.Text.Replace("'", "''") + "','" + txt9_3.Text.Replace("'", "''") + "','" + txt9_4.Text.Replace("'", "''") + "','" + txt9_5.Text.Replace("'", "''") + @"',
                    '" + txtavgvalue_9.Text.Replace("'", "''") + "','" + txtInwardsNo.Text.Replace("'", "''") + "','" + txtCone.Text.Replace("'", "''") + "')";

        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
    }
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        fillDocno();
    }
    private void fillDocno()
    {
        string str = @"SELECT  RIM.DOCID,RIM.DOCNO +' # ' +Replace(convert(nvarchar(11),RIM.Reportdate,106),' ','-') as DocNo 
                    FROM RAWYARNINSPECTIONMASTER RIM(nolock) 
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
        if (txtsearchlotno.Text!="")
        {
            str = str + " and RID.Lotno like '" + txtsearchlotno.Text.Trim() + "%'";
        }
        if (TxtSearchVenLotNo.Text != "")
        {
            str = str + " and RID.VenderLotNo like '" + TxtSearchVenLotNo.Text.Trim() + "%'";
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

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getRawyarninspectionreport", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    row["ImagePhoto"] = Server.MapPath(row["ImagePhoto"].ToString());
                }

                if (Session["VarCompanyNo"].ToString() == "21")
                {
                    Session["rptFileName"] = "~\\Reports\\RptRawYarnInspectionNewKaysons.rpt";
                }
                else
                {

                    Session["rptFileName"] = "~\\Reports\\RptRawYarnInspectionNew.rpt";
                }
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptRawYarnInspectionNew.xsd";
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

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETERAWYARNINSPECTION", param);
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

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_APPROVERAWYARNINSPECTION", param);
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
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update RAWYARNINSPECTIONDETAIL Set ImagePhoto='" + img + "' Where Docid=" + hndocid + "");
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