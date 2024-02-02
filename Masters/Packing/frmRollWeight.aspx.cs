using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Packing_frmRollWeight : System.Web.UI.Page
{
    string Msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["IsEdited"] = "0";
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            TxtPKLNo.Text = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "Select Distinct TPackingNo from V_PackingMaster where PackingID = " + Request["ID"]));
            LoadRollWEight();
            CalcTotals();
        }
    }
    private void LoadRollWEight()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        string sql = @"select distinct P.RollNo, P.RollFrom as RollFrom,P.RollTo as RollTo,isnull(R.Weight,0) as 'NetWeight',isnull(R.GrWt,0) as 'GrossWeight',isnull(R.Width,0) as 'Width', 
                isnull(R.Length,0) as 'Length',isnull(R.Height,0) as 'Height',isnull(R.Area,0) as 'Area'
                 from PackingInformation P left outer join RollWeight R 
                 on p.rollno=r.rollno and p.PackingID=R.PackingID where P.PackingID = " + Request["ID"] + @"
                Select distinct RollNo,RollFrom from PackingInformation where PackingID=" + Request["ID"] + @"
                Select distinct RollNo,RollTo from PackingInformation where PackingID=" + Request["ID"];

        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
        GdvRollWeight.DataSource = ds.Tables[0];
        GdvRollWeight.DataBind();
        UtilityModule.ConditionalComboFillWithDS(ref DDRollFrom, ds, 1, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDRollTo, ds, 2, true, "--Select--");

    }
    private void CalcTotals()
    {
        string Netwt, GRsWt, Width, Len, Height, Ara;
        Double tNet = 0, tGross = 0, tArea = 0;
        for (int i = 0; i < GdvRollWeight.Rows.Count; i++)
        {
            string RollNo = ((Label)GdvRollWeight.Rows[i].FindControl("lblRollno")).Text;
            if (Convert.ToInt32(ViewState["IsEdited"].ToString()) == 1)
            {
                TextBox TxtNetWt = (TextBox)GdvRollWeight.Rows[i].FindControl("TxtNetWt");
                TextBox TxtGrossswt = (TextBox)GdvRollWeight.Rows[i].FindControl("TxtGrossswt");
                TextBox TxtWidth = (TextBox)GdvRollWeight.Rows[i].FindControl("TxtWidth");
                TextBox TxtLength = (TextBox)GdvRollWeight.Rows[i].FindControl("TxtLength");
                TextBox TxtHeight = (TextBox)GdvRollWeight.Rows[i].FindControl("TxtHeight");
                Netwt = TxtNetWt.Text;
                GRsWt = TxtGrossswt.Text;
                Width = TxtWidth.Text;
                Len = TxtLength.Text;
                Height = TxtHeight.Text;
            }
            else
            {
                Label TxtNetWt = (Label)GdvRollWeight.Rows[i].FindControl("lblNetWt");
                Label TxtGrossswt = (Label)GdvRollWeight.Rows[i].FindControl("lblGrossswt");
                Label TxtWidth = (Label)GdvRollWeight.Rows[i].FindControl("lblWidth");
                Label TxtLength = (Label)GdvRollWeight.Rows[i].FindControl("lblLength");
                Label TxtHeight = (Label)GdvRollWeight.Rows[i].FindControl("lblHeight");

                Netwt = TxtNetWt.Text;
                GRsWt = TxtGrossswt.Text;
                Width = TxtWidth.Text;
                Len = TxtLength.Text;
                Height = TxtHeight.Text;
            }

            Label LblArea = (Label)GdvRollWeight.Rows[i].FindControl("lblArea");
            LblArea.Text = ((Convert.ToDouble(Width == "" ? "0" : Width) * Convert.ToDouble(Len == "" ? "0" : Len) * Convert.ToDouble(Height == "" ? "0" : Height)) / 1000000).ToString();
            tNet += Convert.ToDouble(Netwt);
            tGross += Convert.ToDouble(GRsWt);
            tArea += Convert.ToDouble(LblArea.Text);
        }
        txtArea.Text = tArea.ToString();
        TxtGrosWeight.Text = tGross.ToString();
        TxtNetWeight.Text = tNet.ToString();
        ViewState["CalArea"] = tArea;
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        string Netwt, GRsWt, Width, Len, Height, Ara;
        Double tNet = 0, tGross = 0, tArea = 0;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();

        string sql = "Delete from RollWeight where PAckingID= " + Request["ID"];
        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, sql);
        try
        {
            for (int i = 0; i < GdvRollWeight.Rows.Count; i++)
            {
                string RollNo = ((Label)GdvRollWeight.Rows[i].FindControl("lblRollno")).Text;
                if (Convert.ToInt32(ViewState["IsEdited"].ToString()) == 1)
                {
                    TextBox TxtNetWt = (TextBox)GdvRollWeight.Rows[i].FindControl("TxtNetWt");
                    TextBox TxtGrossswt = (TextBox)GdvRollWeight.Rows[i].FindControl("TxtGrossswt");
                    TextBox TxtWidth = (TextBox)GdvRollWeight.Rows[i].FindControl("TxtWidth");
                    TextBox TxtLength = (TextBox)GdvRollWeight.Rows[i].FindControl("TxtLength");
                    TextBox TxtHeight = (TextBox)GdvRollWeight.Rows[i].FindControl("TxtHeight");
                    Netwt = TxtNetWt.Text;
                    GRsWt = TxtGrossswt.Text;
                    Width = TxtWidth.Text;
                    Len = TxtLength.Text;
                    Height = TxtHeight.Text;
                }
                else
                {
                    Label TxtNetWt = (Label)GdvRollWeight.Rows[i].FindControl("lblNetWt");
                    Label TxtGrossswt = (Label)GdvRollWeight.Rows[i].FindControl("lblGrossswt");
                    Label TxtWidth = (Label)GdvRollWeight.Rows[i].FindControl("lblWidth");
                    Label TxtLength = (Label)GdvRollWeight.Rows[i].FindControl("lblLength");
                    Label TxtHeight = (Label)GdvRollWeight.Rows[i].FindControl("lblHeight");

                    Netwt = TxtNetWt.Text;
                    GRsWt = TxtGrossswt.Text;
                    Width = TxtWidth.Text;
                    Len = TxtLength.Text;
                    Height = TxtHeight.Text;
                }

                Label LblArea = (Label)GdvRollWeight.Rows[i].FindControl("lblArea");

                SqlParameter[] _param = new SqlParameter[8];
                _param[0] = new SqlParameter("@PackingID", Request["ID"]);
                _param[1] = new SqlParameter("@RollNo", RollNo);
                _param[2] = new SqlParameter("@NetWeight", Netwt);
                _param[3] = new SqlParameter("@GrossWeight", GRsWt);
                _param[4] = new SqlParameter("@Width", Width);
                _param[5] = new SqlParameter("@Length", Len);
                _param[6] = new SqlParameter("@Height", Height);
                _param[7] = new SqlParameter("@Output", SqlDbType.NVarChar, 200);
                _param[7].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateRollWeight", _param);
                Msg = _param[7].Value.ToString();

            }
            Tran.Commit();
            LoadRollWEight();
            CalcTotals();
            MessageSave(Msg);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Masters/Packing/frmRollWeight.aspx");
            Msg = "Go to Catch during save in RollWeight";
            MessageSave(Msg);
            Tran.Rollback();
        }

    }
    protected void BtnApply_Click(object sender, EventArgs e)
    {

        if (DDRollFrom.SelectedIndex == -1)
            return;
        for (int i = DDRollFrom.SelectedIndex - 1; i < DDRollFrom.SelectedIndex; i++)
        {

            if (Convert.ToInt32(ViewState["IsEdited"].ToString()) == 1)
            {
                TextBox TxtWidth = (TextBox)GdvRollWeight.Rows[i].FindControl("TxtNetWt");
                TextBox TxtLength = (TextBox)GdvRollWeight.Rows[i].FindControl("TxtGrossswt");
                TxtWidth.Text = (Convert.ToDouble(TxtNetWeightB.Text == "" ? "0" : TxtNetWeightB.Text) - Convert.ToDouble(TxtLessGrossWt.Text == "" ? "0" : TxtLessGrossWt.Text)).ToString();
                TxtLength.Text = TxtNetWeightB.Text == "" ? "0" : TxtNetWeightB.Text;
            }
            else
            {
                Label TxtWidth = (Label)GdvRollWeight.Rows[i].FindControl("lblNetWt");
                Label TxtLength = (Label)GdvRollWeight.Rows[i].FindControl("lblGrossswt");
                TxtWidth.Text = (Convert.ToDouble(TxtNetWeightB.Text == "" ? "0" : TxtNetWeightB.Text) - Convert.ToDouble(TxtLessGrossWt.Text == "" ? "0" : TxtLessGrossWt.Text)).ToString();
                TxtLength.Text = TxtNetWeightB.Text == "" ? "0" : TxtNetWeightB.Text;
            }
        }
        CalcTotals();

    }
    protected void DDRollTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtTotalRolls.Text = "";
        TxtTotalRolls.Text = (Convert.ToInt32(DDRollTo.SelectedIndex) - Convert.ToInt32(DDRollFrom.SelectedIndex)).ToString();
    }
    protected void GdvRollWeight_RowEditing(object sender, GridViewEditEventArgs e)
    {

        GdvRollWeight.EditIndex = e.NewEditIndex;
        LoadRollWEight();
        GdvRollWeight.Rows[GdvRollWeight.EditIndex].Cells[6].Text = ViewState["CalArea"].ToString();
        ViewState["CalArea"] = GdvRollWeight.Rows[GdvRollWeight.EditIndex].Cells[6].Text;
        ViewState["IsEdited"] = "1";

    }
    protected void GdvRollWeight_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        ViewState["IsEdited"] = "0";
        GdvRollWeight.EditIndex = -1;
        LoadRollWEight();
        CalcTotals();

    }
    private void MessageSave(string msg)
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('");
        stb.Append(msg);
        stb.Append("');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
    protected void GdvRollWeight_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string RollNo = ((Label)GdvRollWeight.Rows[e.RowIndex].FindControl("lblRollno")).Text;
        string NtWt = "", Gwt = "";

        TextBox TxtNetWt = (TextBox)GdvRollWeight.Rows[e.RowIndex].FindControl("TxtNetWt");
        TextBox TxtGrossswt = (TextBox)GdvRollWeight.Rows[e.RowIndex].FindControl("TxtGrossswt");
        NtWt = TxtNetWt.Text;
        Gwt = TxtGrossswt.Text;
        TextBox TxtWidth = (TextBox)GdvRollWeight.Rows[e.RowIndex].FindControl("TxtWidth");
        TextBox TxtLength = (TextBox)GdvRollWeight.Rows[e.RowIndex].FindControl("TxtLength");
        TextBox TxtHeight = (TextBox)GdvRollWeight.Rows[e.RowIndex].FindControl("TxtHeight");
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlParameter[] _param = new SqlParameter[8];
            _param[0] = new SqlParameter("@PackingID", Request["ID"]);
            _param[1] = new SqlParameter("@RollNo", RollNo);
            _param[2] = new SqlParameter("@NetWeight", NtWt);
            _param[3] = new SqlParameter("@GrossWeight", Gwt);
            _param[4] = new SqlParameter("@Width", TxtWidth.Text);
            _param[5] = new SqlParameter("@Length", TxtLength.Text);
            _param[6] = new SqlParameter("@Height", TxtHeight.Text);
            // _param[7] = new SqlParameter("@Area", Area);
            _param[7] = new SqlParameter("@Output", SqlDbType.NVarChar, 200);
            _param[7].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_UpdateRollWeight", _param);
            Msg = _param[7].Value.ToString();            
            MessageSave(Msg);
            GdvRollWeight.EditIndex = -1;
           
            LoadRollWEight();
            CalcTotals();
            
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Masters/Packing/frmRollWeight.aspx");
            Msg = "Go to Catch in Grid RowUpdating";
            MessageSave(Msg);
            con.Close();
            con.Dispose();
        }
    }
}