using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ClosedXML.Excel;
using System.IO;
public partial class Masters_Order_FrmItemWiseSPIWKForMWS : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            DDLInCompanyName.Focus();
            UtilityModule.ConditionalComboFill(ref DDLInCompanyName, @"select CI.CompanyId,CompanyName From CompanyInfo CI(Nolock),Company_Authentication CA(Nolock) 
            Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName", true, "--Select--");

            if (DDLInCompanyName.Items.Count > 0)
            {
                DDLInCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDLInCompanyName.Enabled = false;
            }

            CompanyNameSelectedIndexChanged();

            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDLInCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanyNameSelectedIndexChanged();
    }
    private void CompanyNameSelectedIndexChanged()
    {
        if (DDLInCompanyName.SelectedIndex > 0)
        {
            Fill_Grid();
        }
    }
    private void Fill_Grid()
    {
        DGItemDetail.DataSource = GetDetail();
        DGItemDetail.DataBind();
    }
    private DataSet GetDetail()
    {
        DataSet ds = null;
        try
        {
            SqlParameter[] para = new SqlParameter[8];
            para[0] = new SqlParameter("@CompanyID", SqlDbType.Int);
            para[1] = new SqlParameter("@UserID", SqlDbType.Int);
            para[2] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            para[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 250);

            para[0].Value = DDLInCompanyName.SelectedValue;
            para[1].Value = Session["varuserid"];
            para[2].Value = Session["varMasterCompanyIDForERP"];
            para[3].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GET_ITEMDETAILSPIWKForMWS", para);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/GetDetail/FrmItemWiseSPIWKForMWS.aspx");
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = ex.Message;
        }
        return ds;
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        Savedetail();
    }
    protected void Savedetail()
    {
        string Str = "";
        if (lblErrorMessage.Text == "")
        {
            for (int i = 0; i < DGItemDetail.Rows.Count; i++)
            {
                CheckBox Chkboxitem = ((CheckBox)DGItemDetail.Rows[i].FindControl("Chkboxitem"));
                if (Chkboxitem.Checked == true)
                {
                    string strItem_Finished_ID = DGItemDetail.DataKeys[i].Value.ToString();
                    TextBox txtStatus = (TextBox)DGItemDetail.Rows[i].FindControl("txtStatus");
                    TextBox txtSPIWKQty = (TextBox)DGItemDetail.Rows[i].FindControl("txtSPIWKQty");
                    if (Str == "")
                    {
                        Str = strItem_Finished_ID + "|" + txtStatus.Text + "|" + txtSPIWKQty.Text + "~";
                    }
                    else
                    {
                        Str = Str + strItem_Finished_ID + "|" + txtStatus.Text + "|" + txtSPIWKQty.Text + "~";
                    }
                }
            }
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("Pro_Save_ItemWiseSPIWKForMWS", con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 3000;

                if (Str != "")
                {
                    cmd.Parameters.AddWithValue("@CompanyID", DDLInCompanyName.SelectedValue);
                    cmd.Parameters.AddWithValue("@Date", TxtDate.Text);
                    cmd.Parameters.AddWithValue("@DetailData", Str);
                    cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@UserID", Session["varuserid"]);
                    cmd.Parameters.AddWithValue("@MastercompanyID", Session["varMasterCompanyIDForERP"]);

                    cmd.ExecuteNonQuery();

                    if (cmd.Parameters["@msg"].Value.ToString() != "")
                    {
                        Tran.Rollback();
                    }
                    else
                    {
                        Tran.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Order/Savedetail/FrmItemWiseSPIWKForMWS.aspx");
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = ex.Message;
                Logs.WriteErrorLog(ex.Message);
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    private void MessageSave()
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('Record(s) has been saved successfully!');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
    protected void DGItemDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGItemDetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGItemDetail_RowDataBound111(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < DGItemDetail.Columns.Count; i++)
            {
                if (DGItemDetail.Columns[i].HeaderText.ToUpper() == "INTERNAL PROD QTY REQ." || DGItemDetail.Columns[i].HeaderText.ToUpper() == "PRE INTERNAL PROD QTY.")
                {
                    if (variable.VarTAGGINGWITHINTERNALPRODUCTION == "1")
                    {
                        DGItemDetail.Columns[i].Visible = true;
                    }
                    else
                    {
                        DGItemDetail.Columns[i].Visible = false;
                    }
                }
                if (Convert.ToInt32(Session["varcompanyno"]) != 16)
                {
                    if (DGItemDetail.Columns[i].HeaderText.ToUpper() == "PROD WEAVING RATE" || DGItemDetail.Columns[i].HeaderText.ToUpper() == "INT WEAVING RATE")
                    {
                        DGItemDetail.Columns[i].Visible = false;
                    }
                }
            }
        }
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        int NoofColumn;
        int Row;
        SqlParameter[] array = new SqlParameter[1];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_WoolConsumptionSPIWK", array);

        NoofColumn = ds.Tables[1].Rows.Count;

        if (ds.Tables[0].Rows.Count > 0)
        {
            string Path = "";

            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Wool_Consumption");
            //*************
            sht.Range("A1:D1").Merge();
            sht.Range("A1:D1").Style.Font.FontSize = 11;
            sht.Range("A1:D1").Style.Font.Bold = true;
            sht.Range("A1:D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:D1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Row(1).Height = 21.75;
            //
            sht.Range("A2:D2").Merge();
            sht.Range("A2:D2").Style.Font.FontSize = 11;
            sht.Range("A2:D2").Style.Font.Bold = true;
            sht.Range("A2:D2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:D2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Row(2).Height = 21.75;
            //Header

            sht.Range("A3").SetValue("ItemName");
            sht.Range("B3").SetValue("QualityName");
            sht.Range("C3").SetValue("DesignName");
            sht.Range("D3").SetValue("ColorName");
            sht.Range("E3").SetValue("ShapeName");
            sht.Range("F3").SetValue("Size");
            sht.Range("G3").SetValue("Status");
            sht.Range("H3").SetValue("SPIWKQty");

            NoofColumn = ds.Tables[1].Rows.Count;
            if (NoofColumn > 50)
            {
                NoofColumn = 50;
            }
            for (int i = 0; i < NoofColumn; i++)
            {
                if (i == 0)
                {
                    sht.Range("I3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 1)
                {
                    sht.Range("J3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 2)
                {
                    sht.Range("K3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 3)
                {
                    sht.Range("L3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 4)
                {
                    sht.Range("M3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 5)
                {
                    sht.Range("N3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 6)
                {
                    sht.Range("O3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 7)
                {
                    sht.Range("P3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 8)
                {
                    sht.Range("Q3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 9)
                {
                    sht.Range("R3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 10)
                {
                    sht.Range("S3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 11)
                {
                    sht.Range("T3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 12)
                {
                    sht.Range("U3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 13)
                {
                    sht.Range("V3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 14)
                {
                    sht.Range("W3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 15)
                {
                    sht.Range("X3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 16)
                {
                    sht.Range("Y3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 17)
                {
                    sht.Range("Z3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 18)
                {
                    sht.Range("AA3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 19)
                {
                    sht.Range("AB3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 20)
                {
                    sht.Range("AC3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 21)
                {
                    sht.Range("AD3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 22)
                {
                    sht.Range("AE3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 23)
                {
                    sht.Range("AF3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 24)
                {
                    sht.Range("AG3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 25)
                {
                    sht.Range("AH3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 26)
                {
                    sht.Range("AI3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 27)
                {
                    sht.Range("AJ3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 28)
                {
                    sht.Range("AK3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 29)
                {
                    sht.Range("AL3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 30)
                {
                    sht.Range("AM3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 31)
                {
                    sht.Range("AN3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 32)
                {
                    sht.Range("AO3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 33)
                {
                    sht.Range("AP3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 34)
                {
                    sht.Range("AQ3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 35)
                {
                    sht.Range("AR3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 36)
                {
                    sht.Range("AS3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 37)
                {
                    sht.Range("AT3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 38)
                {
                    sht.Range("AU3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 39)
                {
                    sht.Range("AV3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 40)
                {
                    sht.Range("AW3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 41)
                {
                    sht.Range("AX3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 42)
                {
                    sht.Range("AY3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 43)
                {
                    sht.Range("AZ3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 44)
                {
                    sht.Range("BA3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 45)
                {
                    sht.Range("BB3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 46)
                {
                    sht.Range("BC3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 47)
                {
                    sht.Range("BD3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 48)
                {
                    sht.Range("BE3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 49)
                {
                    sht.Range("BF3").SetValue(ds.Tables[1].Rows[i][0]);
                }
                if (i == 50)
                {
                    sht.Range("BG3").SetValue(ds.Tables[1].Rows[i][0]);
                }
            }

            Row = 4;
            Decimal Tqty = 0;
            for (int J = 0; J < ds.Tables[0].Rows.Count; J++)
            {
                sht.Range("A" + Row + ":D" + Row).Style.Font.FontSize = 11;

                sht.Range("A" + Row).SetValue(ds.Tables[0].Rows[J]["ItemName"]);
                sht.Range("B" + Row).SetValue(ds.Tables[0].Rows[J]["QualityName"]);
                sht.Range("C" + Row).SetValue(ds.Tables[0].Rows[J]["DesignName"]);
                sht.Range("D" + Row).SetValue(ds.Tables[0].Rows[J]["ColorName"]);
                sht.Range("E" + Row).SetValue(ds.Tables[0].Rows[J]["ShapeName"]);
                sht.Range("F" + Row).SetValue(ds.Tables[0].Rows[J]["SizeFt"]);
                sht.Range("G" + Row).SetValue(ds.Tables[0].Rows[J]["Status"]);
                sht.Range("H" + Row).SetValue(ds.Tables[0].Rows[J]["SPIWKQty"]);
                sht.Range("H" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                for (int i = 0; i < NoofColumn; i++)
                {
                    if (i == 0)
                    {
                        sht.Range("I" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 1)
                    {
                        sht.Range("J" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 2)
                    {
                        sht.Range("K" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 3)
                    {
                        sht.Range("L" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 4)
                    {
                        sht.Range("M" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 5)
                    {
                        sht.Range("N" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 6)
                    {
                        sht.Range("O" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 7)
                    {
                        sht.Range("P" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 8)
                    {
                        sht.Range("Q" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 9)
                    {
                        sht.Range("R" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 10)
                    {
                        sht.Range("S" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 11)
                    {
                        sht.Range("T" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 12)
                    {
                        sht.Range("U" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 13)
                    {
                        sht.Range("V" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 14)
                    {
                        sht.Range("W" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 15)
                    {
                        sht.Range("X" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 16)
                    {
                        sht.Range("Y" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 17)
                    {
                        sht.Range("Z" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 18)
                    {
                        sht.Range("AA" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 19)
                    {
                        sht.Range("AB" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 20)
                    {
                        sht.Range("AC" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 21)
                    {
                        sht.Range("AD" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 22)
                    {
                        sht.Range("AE" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 23)
                    {
                        sht.Range("AF" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 24)
                    {
                        sht.Range("AG" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 25)
                    {
                        sht.Range("AH" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 26)
                    {
                        sht.Range("AI" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 27)
                    {
                        sht.Range("AJ" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 28)
                    {
                        sht.Range("AK" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 29)
                    {
                        sht.Range("AL" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 30)
                    {
                        sht.Range("AM" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 31)
                    {
                        sht.Range("AN" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 32)
                    {
                        sht.Range("AO" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 33)
                    {
                        sht.Range("AP" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 34)
                    {
                        sht.Range("AQ" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 35)
                    {
                        sht.Range("AR" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 36)
                    {
                        sht.Range("AS" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 37)
                    {
                        sht.Range("AT" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 38)
                    {
                        sht.Range("AU" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 39)
                    {
                        sht.Range("AV" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 40)
                    {
                        sht.Range("AW" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 41)
                    {
                        sht.Range("AX" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 42)
                    {
                        sht.Range("AY" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 43)
                    {
                        sht.Range("AZ" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 44)
                    {
                        sht.Range("BA" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 45)
                    {
                        sht.Range("BB" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 46)
                    {
                        sht.Range("BC" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 47)
                    {
                        sht.Range("BD" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 48)
                    {
                        sht.Range("BE" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 49)
                    {
                        sht.Range("BF" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                    if (i == 50)
                    {
                        sht.Range("BG" + Row).SetValue(ds.Tables[0].Rows[J][i + 10]);
                    }
                }

                Row = Row + 1;
            }

            //**********
            sht.Columns(1, 10).AdjustToContents();
            //**************Save
            //******SAVE FILE
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("Wool_Consumption_" + DateTime.Now + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
}