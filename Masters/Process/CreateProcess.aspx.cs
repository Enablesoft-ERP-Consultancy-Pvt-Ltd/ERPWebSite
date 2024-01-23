using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Masters_Campany_Design : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditonalChkBoxListFill(ref ChkForUserType, "Select * From UserType order by ID");
            Fill_Grid();
            RDProcessType.SelectedIndex = 0;
            if (variable.Carpetcompany == "1")
            {
                switch (Session["varMasterCompanyIDForERP"].ToString())
                {
                    case "20":
                    case "8":
                        TDAreaeditable.Visible = false;
                        TDissreconetime.Visible = false;
                        break;
                    case "22":                    
                       TDAreaeditable.Visible = true;
                        TDissreconetime.Visible = true;
                        if (Session["usertype"].ToString() == "1")
                        {
                            TDSizeTolerance.Visible = true;
                            TDWeightTolerance.Visible = true;
                        }
                        else
                        {
                            TDSizeTolerance.Visible = false;
                            TDWeightTolerance.Visible = false;
                        }
                        break;
                    default:
                        TDAreaeditable.Visible = true;
                        TDissreconetime.Visible = true;
                        TDSizeTolerance.Visible = false;
                        TDWeightTolerance.Visible = false;
                        break;
                }

            }
        }
        LblErrer.Visible = false;
    }
    private void Fill_Grid()
    {
        DGCreateProcess.DataSource = Get_Detail();
        DGCreateProcess.DataBind();
        if (DGCreateProcess.Rows.Count > 0)
        {
            TDseqNo.Visible = true;

        }
        else
        {
            TDseqNo.Visible = false;

        }
    }
    protected void DGCreateProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = @"Select PROCESS_NAME_ID,PROCESS_NAME,ShortName,Isnull(ApprovalFlag,0) As ApprovalFlag,IsNull(ID,0) ID,Isnull(ProcessType,0) As ProcessType,
                        isnull(PNM.AreaEditable,0) as AreaEditable,isnull(PNM.issreconetime,0) as issreconetime,isnull(PNM.SizeToleranceFlag,0) as SizeToleranceFlag 
                        ,isnull(PNM.WeightToleranceFlag,0) as WeightToleranceFlag
                        From PROCESS_NAME_MASTER PNM Left Outer Join Process_UserType PUT ON 
                       PNM.PROCESS_NAME_ID=PUT.ProcessID Where PNM.PROCESS_NAME_ID=" + DGCreateProcess.SelectedDataKey.Value + " And PNM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        int n = Ds.Tables[0].Rows.Count;
        if (n > 0)
        {
            TxtProcessName.Text = Ds.Tables[0].Rows[0]["PROCESS_NAME"].ToString();
            TxtShortName.Text = Ds.Tables[0].Rows[0]["ShortName"].ToString();
            RDProcessType.SelectedIndex = Convert.ToInt32(Ds.Tables[0].Rows[0]["ProcessType"]);
            ChkForApproval.Checked = Convert.ToInt32(Ds.Tables[0].Rows[0]["ApprovalFlag"]) == 1 ? true : false;
            Chkareaeditable.Checked = Convert.ToBoolean(Ds.Tables[0].Rows[0]["areaeditable"]);
            chkissreconetime.Checked = Convert.ToBoolean(Ds.Tables[0].Rows[0]["issreconetime"]);
            ChkForSizeTolerance.Checked = Convert.ToBoolean(Ds.Tables[0].Rows[0]["SizeToleranceFlag"]);
            ChkForWeightTolerance.Checked = Convert.ToBoolean(Ds.Tables[0].Rows[0]["WeightToleranceFlag"]);
            int a = ChkForUserType.Items.Count;
            if (a > 0)
            {
                for (int i = 0, j = 0; i < a; i++)
                {
                    if (j < n)
                    {
                        if ((ChkForUserType.Items[i].Value == Ds.Tables[0].Rows[j]["ID"].ToString()))
                        {
                            ChkForUserType.Items[i].Selected = true;
                            j++;
                        }
                        else
                        {
                            ChkForUserType.Items[i].Selected = false;
                        }
                    }
                    else
                    {
                        ChkForUserType.Items[i].Selected = false;
                    }
                }
            }
            btnsave.Text = "Update";
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        CreateProcess();
        Fill_Grid();
        RDProcessType.SelectedIndex = 0;
    }
    //*******************************************************************
    private DataSet Get_Detail()
    {
        DataSet DS = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string sqlstr = "Select PROCESS_NAME_ID as ID,PROCESS_NAME,ShortName,isnull(seqno,0) as seqNo from PROCESS_NAME_MASTER  Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Process_name_id";
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/CreateProcess.aspx");
            LblErrer.Visible = true;
            LblErrer.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return DS;
    }
    protected void DGProcess_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGCreateProcess, "Select$" + e.Row.RowIndex);
        }
    }
    private void validate_processName()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string sqlstr;
            if (btnsave.Text == "Update")
            {
                sqlstr = "Select Isnull(PROCESS_NAME_ID,0) from PROCESS_NAME_MASTER where PROCESS_NAME='" + TxtProcessName.Text + "' and PROCESS_NAME_ID !=" + DGCreateProcess.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            else
            {
                sqlstr = "Select Isnull(max(PROCESS_NAME_ID),0) from PROCESS_NAME_MASTER where PROCESS_NAME='" + TxtProcessName.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            int Processid = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, sqlstr));
            if (Processid > 0)
            {
                LblErrer.Visible = true;
                LblErrer.Text = "Process Name Already Exists......";
                TxtProcessName.Text = "";
                TxtProcessName.Focus();
            }
            else
            {
                LblErrer.Visible = false;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/CreateProcess.aspx");
            LblErrer.Visible = true;
            LblErrer.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void TxtProcessName_TextChanged(object sender, EventArgs e)
    {
        validate_processName();
    }
    private void CreateProcess()
    {
        LblErrer.Text = "";
        LblErrer.Visible = false;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            validate_processName();
            con.Open();
            if (LblErrer.Visible == false)
            {
                SqlParameter[] _arrPara = new SqlParameter[12];
                _arrPara[0] = new SqlParameter("@PROCESS_NAME_ID", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@PROCESS_NAME", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@ShortName", SqlDbType.NVarChar, 50);
                _arrPara[3] = new SqlParameter("@userid", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@MasterCompanyid", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@ApprovalFlag", SqlDbType.Int);
                _arrPara[6] = new SqlParameter("@UserType", SqlDbType.NVarChar, 100);
                _arrPara[7] = new SqlParameter("@ProcessType", SqlDbType.Int);
                _arrPara[8] = new SqlParameter("@AreaEditable", SqlDbType.Int);
                _arrPara[9] = new SqlParameter("@issreconetime", SqlDbType.Int);
                _arrPara[10] = new SqlParameter("@SizeToleranceFlag", SqlDbType.Int);
                _arrPara[11] = new SqlParameter("@WeightToleranceFlag", SqlDbType.Int);

                _arrPara[0].Value = 0;
                if (btnsave.Text == "Update")
                {
                    _arrPara[0].Value = DGCreateProcess.SelectedValue;
                }
                _arrPara[1].Value = TxtProcessName.Text.ToUpper();
                _arrPara[2].Value = TxtShortName.Text.ToUpper();
                _arrPara[3].Value = Session["varuserid"].ToString();
                _arrPara[4].Value = Session["varMasterCompanyIDForERP"].ToString();
                _arrPara[5].Value = ChkForApproval.Checked == true ? 1 : 0;

                int n = ChkForUserType.Items.Count;
                string str = null;
                for (int i = 0; i < n; i++)
                {
                    if (ChkForUserType.Items[i].Selected)
                    {
                        str = str == null ? ChkForUserType.Items[i].Value : str + "," + ChkForUserType.Items[i].Value;
                    }
                }
                _arrPara[6].Value = str;
                _arrPara[7].Value = RDProcessType.SelectedIndex;
                _arrPara[8].Value = TDAreaeditable.Visible == true ? (Chkareaeditable.Checked == true ? "1" : "0") : "0";
                _arrPara[9].Value = TDissreconetime.Visible == true ? (chkissreconetime.Checked == true ? "1" : "0") : "0";
                _arrPara[10].Value = TDSizeTolerance.Visible == true ? (ChkForSizeTolerance.Checked == true ? "1" : "0") : "0";
                _arrPara[11].Value = TDWeightTolerance.Visible == true ? (ChkForWeightTolerance.Checked == true ? "1" : "0") : "0";

                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_PROCESS_MASTER", _arrPara);
                LblErrer.Visible = true;
                LblErrer.Text = "Save Details......";
                btnsave.Text = "Save";
                TxtProcessName.Text = "";
                TxtShortName.Text = "";
                Chkareaeditable.Checked = false;
                chkissreconetime.Checked = false;
                ChkForSizeTolerance.Checked = false;
                ChkForWeightTolerance.Checked = false;
                //  AllEnums.MasterTables.PROCESS_NAME_MASTER.RefreshTable();

            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/CreateProcess.aspx");
            LblErrer.Visible = true;
            LblErrer.Text = ex.Message;
            //TxtProcessName.Text = "";
            TxtProcessName.Focus();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void lnkbutton_Click(object sender, EventArgs e)
    {
        LblErrer.Text = "";
        LblErrer.Visible = false;
        //**********sql Table'
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("Process_name_id", typeof(int));
        dtrecords.Columns.Add("Seqno", typeof(int));
        int rowcount = DGCreateProcess.Rows.Count;
        for (int i = 0; i < rowcount; i++)
        {
            DataRow dr = dtrecords.NewRow();
            TextBox txtseqno = (TextBox)DGCreateProcess.Rows[i].FindControl("textseqno");
            dr["Process_name_id"] = DGCreateProcess.DataKeys[i].Value;
            dr["seqno"] = txtseqno.Text == "" ? "0" : txtseqno.Text;
            dtrecords.Rows.Add(dr);
        }
        if (dtrecords.Rows.Count > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@dtrecords", dtrecords);
                param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[1].Direction = ParameterDirection.Output;
                //**********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateProcessSeqNo", param);
                LblErrer.Text = param[1].Value.ToString();
                LblErrer.Visible = true;
                Tran.Commit();
                Fill_Grid();
            }
            catch (Exception ex)
            {
                LblErrer.Text = ex.Message;
                LblErrer.Visible = true;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
        }
    }
}