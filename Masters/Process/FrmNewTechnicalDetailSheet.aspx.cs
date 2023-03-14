using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;
using System.Drawing;
using System.Data.SqlTypes;
using System.Text;
using System.IO;
using System.Collections.Specialized;

public partial class Masters_Process_FrmNewTechnicalDetailSheet : System.Web.UI.Page
{
    public static Boolean fileNoexist = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            SetInitialRow();
            ViewState["Id"] = "0";
            UtilityModule.ConditionalComboFill(ref DDItemName, "SELECT Item_Id,Item_Name from Item_Master  Where MasterCompanyId=" + Session["varcompanyId"] + " Order by Item_name", true, "--Select--");
            UtilityModule.ConditionalComboFill(ref DDDesign, "SELECT DESIGNID,DESIGNNAME from DESIGN where MasterCompanyId=" + Session["varcompanyid"] + "Order By DESIGNNAME", true, "--Select--");
            UtilityModule.ConditionalComboFill(ref DDColor, "SELECT ColorId,ColorName from color where MasterCompanyId=" + Session["varcompanyid"] + "Order By ColorName", true, "--Select--");

            hncomp.Value = Convert.ToString(Session["varCompanyId"]);
        }
    }
    protected void BindQuality()
    {
        UtilityModule.ConditionalComboFill(ref DDQuality, "select QualityId,QualityName from quality where Item_id=" + DDItemName.SelectedValue + " And MasterCompanyId=" + Session["varcompanyid"] + "Order By QualityName", true, "-- Pls Select Quality--");
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindQuality();
    }
    private void SetInitialRow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("Column1", typeof(string)));
        dt.Columns.Add(new DataColumn("Column2", typeof(string)));
        //dt.Columns.Add(new DataColumn("Column3", typeof(string)));
        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["Column1"] = string.Empty;
        dr["Column2"] = string.Empty;
        //dr["Column3"] = string.Empty;
        dt.Rows.Add(dr);
        //dr = dt.NewRow();

        //Store the DataTable in ViewState
        ViewState["CurrentTable"] = dt;

        Gv1.DataSource = dt;
        Gv1.DataBind();
    }
    private void AddNewRowToGrid()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox box1 = (TextBox)Gv1.Rows[rowIndex].Cells[1].FindControl("TextBox1");
                    TextBox box2 = (TextBox)Gv1.Rows[rowIndex].Cells[2].FindControl("TextBox2");
                    //TextBox box3 = (TextBox)Gv1.Rows[rowIndex].Cells[3].FindControl("TextBox3");

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RowNumber"] = i + 1;

                    dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;
                    //dtCurrentTable.Rows[i - 1]["Column3"] = box3.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;

                Gv1.DataSource = dtCurrentTable;
                Gv1.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }

        //Set Previous Data on Postbacks
        SetPreviousData();
    }
    private void SetPreviousData()
    {
        int rowIndex = 0;
        //StringCollection sc = new StringCollection();
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box1 = (TextBox)Gv1.Rows[rowIndex].Cells[1].FindControl("TextBox1");
                    TextBox box2 = (TextBox)Gv1.Rows[rowIndex].Cells[2].FindControl("TextBox2");
                    //TextBox box3 = (TextBox)Gv1.Rows[rowIndex].Cells[3].FindControl("TextBox3");

                    box1.Text = dt.Rows[i]["Column1"].ToString();
                    box2.Text = dt.Rows[i]["Column2"].ToString();
                    //box3.Text = dt.Rows[i]["Column3"].ToString();

                    // sc.Add(box1.Text + "," + box2.Text + "," + box3.Text);

                    rowIndex++;
                }
                //InsertRecords(sc);
            }
        }
    }
    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid();
    }
    private void Gv1insertrecord(SqlTransaction Tran)
    {
        if (Gv1.Rows.Count != 0)
        {
            for (int i = 0; i < Gv1.Rows.Count; i++)
            {
                TextBox box1 = (TextBox)Gv1.Rows[i].Cells[1].FindControl("TextBox1");

                TextBox box2 = (TextBox)Gv1.Rows[i].Cells[2].FindControl("TextBox2");

                // TextBox box3 = (TextBox)Gv1.Rows[i].Cells[3].FindControl("TextBox3");
                if (box1.Text != "" || box2.Text != "")
                {
                    string str = @"Insert into NewTechnicalBanaColorPly(FileNo,BanaColor,BanaPly,Userid,Mastercompanyid)
                   values ('" + txtFileNo.Text + "','" + box1.Text + "','" + box2.Text + "','" + Session["varuserid"] + "','" + Session["varCompanyId"] + "')";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                }
            }
        }

    }
    private void CHECKVALIDCONTROL()
    {
        Lblmessage.Visible = true;
        Lblmessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDItemName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDQuality) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDDesign) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDColor) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtFileNo) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtKanghi) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtBharti) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtPick) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtTana) == false)
        {
            goto a;
        }
        //if (UtilityModule.VALIDTEXTBOX(txtBanaColor) == false)
        //{
        //    goto a;
        //}
        //if (UtilityModule.VALIDTEXTBOX(txtBanaPly) == false)
        //{
        //    goto a;
        //}
        if (UtilityModule.VALIDTEXTBOX(txtKhatiPly) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtKhati) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtTarika) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtFrenzes) == false)
        {
            goto a;
        }
        //if (TDBinNo.Visible == true)
        //{
        //    if (UtilityModule.VALIDDROPDOWNLIST(DDBinNo) == false)
        //    {
        //        goto a;
        //    }
        //}       

        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(Lblmessage);
    B: ;
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SaveIndent();
    }
    private void SaveIndent()
    {
        CHECKVALIDCONTROL();

        if (Lblmessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (txtFileNo.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Please fill file No');", true);
                    tran.Commit();
                    return;
                }

                SqlParameter[] _arrpara = new SqlParameter[4];
                _arrpara[0] = new SqlParameter("@fileno", SqlDbType.VarChar, 100);
                _arrpara[1] = new SqlParameter("@Version", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@UserId", SqlDbType.Int);


                System.Data.SqlTypes.SqlDateTime getDate;
                //set DateTime null
                getDate = SqlDateTime.Null;


                _arrpara[0].Value = txtFileNo.Text;
                _arrpara[1].Direction = ParameterDirection.Output;
                _arrpara[2].Value = Session["varuserid"].ToString();
                //Create FileVersion
                SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "pro_NewTechnicalFileVersion", _arrpara);
                txtFileVersion.Text = _arrpara[1].Value.ToString();


                SqlParameter[] _arrpara1 = new SqlParameter[20];
                _arrpara1[0] = new SqlParameter("@Id", SqlDbType.Int);
                _arrpara1[1] = new SqlParameter("@FileNo", SqlDbType.VarChar, 20);
                _arrpara1[2] = new SqlParameter("@ItemId", SqlDbType.Int);
                _arrpara1[3] = new SqlParameter("@QualityId", SqlDbType.Int);
                _arrpara1[4] = new SqlParameter("@DesignId", SqlDbType.Int);
                _arrpara1[5] = new SqlParameter("@ColorId", SqlDbType.Int);
                _arrpara1[6] = new SqlParameter("@Kanghi", SqlDbType.VarChar, 50);
                _arrpara1[7] = new SqlParameter("@Bharti", SqlDbType.VarChar, 50);
                _arrpara1[8] = new SqlParameter("@Pick", SqlDbType.VarChar, 50);
                _arrpara1[9] = new SqlParameter("@Tana", SqlDbType.VarChar, 50);
                //_arrpara[10] = new SqlParameter("@BanaColor", SqlDbType.VarChar, 50);
                //_arrpara[11] = new SqlParameter("@BanaPly", SqlDbType.VarChar, 50);
                _arrpara1[10] = new SqlParameter("@KhatiPly", SqlDbType.VarChar, 50);
                _arrpara1[11] = new SqlParameter("@Khati", SqlDbType.VarChar, 50);
                _arrpara1[12] = new SqlParameter("@Tarika", SqlDbType.VarChar, 50);
                _arrpara1[13] = new SqlParameter("@Frenzes", SqlDbType.VarChar, 50);
                _arrpara1[14] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                _arrpara1[15] = new SqlParameter("@UserId", SqlDbType.Int);
                _arrpara1[16] = new SqlParameter("@msg", SqlDbType.VarChar, 200);
                _arrpara1[16].Direction = ParameterDirection.Output;
                _arrpara1[17] = new SqlParameter("@Version", SqlDbType.Int);
                _arrpara1[18] = new SqlParameter("@LocalOrderNo", SqlDbType.VarChar, 50);
                _arrpara1[19] = new SqlParameter("@GSM", SqlDbType.VarChar, 50);

                _arrpara1[0].Direction = ParameterDirection.InputOutput;
                //if (ChkEditOrder.Checked == true)
                //    _arrpara[0].Value = ViewState["Id"];
                //else
                _arrpara[0].Value = ViewState["Id"];
                _arrpara1[1].Value = txtFileNo.Text;
                _arrpara1[2].Value = DDItemName.SelectedValue;
                _arrpara1[3].Value = DDQuality.SelectedValue;
                _arrpara1[4].Value = DDDesign.SelectedValue;
                _arrpara1[5].Value = DDColor.SelectedValue;
                _arrpara1[6].Value = txtKanghi.Text;
                _arrpara1[7].Value = txtBharti.Text;
                _arrpara1[8].Value = txtPick.Text;
                _arrpara1[9].Value = txtTana.Text;
                //_arrpara[10].Value = txtBanaColor.Text;
                //_arrpara[11].Value = txtBanaPly.Text;
                _arrpara1[10].Value = txtKhatiPly.Text;
                _arrpara1[11].Value = txtKhati.Text;
                _arrpara1[12].Value = txtTarika.Text;
                _arrpara1[13].Value = txtFrenzes.Text;
                _arrpara1[14].Value = Session["varCompanyId"];
                _arrpara1[15].Value = Session["varuserid"];
                _arrpara1[17].Value = txtFileVersion.Text;
                _arrpara1[18].Value = txtSRNo.Text;
                _arrpara1[19].Value = txtGSM.Text;

                SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "PRO_SAVENEWTECHNICALDETAILSHEET", _arrpara1);
                Gv1insertrecord(tran);
                tran.Commit();
                ViewState["Id"] = 0;


                Lblmessage.Visible = true;
                Lblmessage.Text = _arrpara1[16].Value.ToString();
                //refreshForm();
                //fill_grid();
                BtnSave.Text = "Save";

            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Purchase/FrmNewTechnicalDetailSheet.aspx");
                tran.Rollback();
                Lblmessage.Visible = true;
                Lblmessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

    }

    protected void txtFileNo_TextChanged(object sender, EventArgs e)
    {

        try
        {
            fileNoexist = false;
            if (txtFileNo.Text != "")
            {
                fill_GV1();
            }
            if (fileNoexist == false)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('File No. does not exist..');", true);
            }
        }
        catch (Exception ex)
        {
            Lblmessage.Visible = true;
            Lblmessage.Text = ex.Message;

        }
        finally
        {


        }

    }
    protected void fill_GV1()
    {
        string str1 = "select Id,TC.FileNo,ItemId,QualityId,DesignId,ColorId,Kanghi,Bharti,Pick,Tana,KhatiPly,Khati,Tarika,Frenzes,Replace(convert(nvarchar(11),TC.AddDate,106),' ','-') As AddDate,F.Version,LocalOrderNo,GSM from NewTechnicalDetailSheet TC,NewTechnicalFileVersion F where TC.FileNo=F.FileNo and  TC.fileno='" + txtFileNo.Text + "'";

        DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str1);

        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            DDItemName.SelectedValue = ds1.Tables[0].Rows[0]["ItemId"].ToString();
            BindQuality();
            DDQuality.SelectedValue = ds1.Tables[0].Rows[0]["QualityId"].ToString();
            DDDesign.SelectedValue = ds1.Tables[0].Rows[0]["DesignId"].ToString();
            DDColor.SelectedValue = ds1.Tables[0].Rows[0]["ColorId"].ToString();
            txtFileVersion.Text = ds1.Tables[0].Rows[i]["Version"].ToString();
            txtSRNo.Text = ds1.Tables[0].Rows[i]["LocalOrderNo"].ToString();
            txtGSM.Text = ds1.Tables[0].Rows[i]["GSM"].ToString();
            txtKanghi.Text = ds1.Tables[0].Rows[0]["Kanghi"].ToString();
            txtBharti.Text = ds1.Tables[0].Rows[0]["Bharti"].ToString();
            txtPick.Text = ds1.Tables[0].Rows[0]["Pick"].ToString();
            txtTana.Text = ds1.Tables[0].Rows[0]["Tana"].ToString();
            txtKhatiPly.Text = ds1.Tables[0].Rows[0]["KhatiPly"].ToString();
            txtKhati.Text = ds1.Tables[0].Rows[0]["Khati"].ToString();
            txtTarika.Text = ds1.Tables[0].Rows[0]["Tarika"].ToString();
            txtFrenzes.Text = ds1.Tables[0].Rows[0]["Frenzes"].ToString();
            fileNoexist = true;
        }


        string str = "Select BanaColor,BanaPly from  NewTechnicalBanaColorPly where fileno='" + txtFileNo.Text + "'";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if (i == 0)
            {
                TextBox box1 = (TextBox)Gv1.Rows[i].Cells[1].FindControl("TextBox1");
                TextBox box2 = (TextBox)Gv1.Rows[i].Cells[2].FindControl("TextBox2");
                //TextBox box3 = (TextBox)Gv1.Rows[i].Cells[3].FindControl("TextBox3");

                box1.Text = ds.Tables[0].Rows[i]["BanaColor"].ToString();
                box2.Text = ds.Tables[0].Rows[i]["BanaPly"].ToString();
                //box3.Text = ds.Tables[0].Rows[i]["DESCREPTION"].ToString();
            }
            else
            {
                AddNewRowToGrid();
                TextBox box1 = (TextBox)Gv1.Rows[i].Cells[1].FindControl("TextBox1");
                TextBox box2 = (TextBox)Gv1.Rows[i].Cells[2].FindControl("TextBox2");
                //TextBox box3 = (TextBox)Gv1.Rows[i].Cells[3].FindControl("TextBox3");

                box1.Text = ds.Tables[0].Rows[i]["BanaColor"].ToString();
                box2.Text = ds.Tables[0].Rows[i]["BanaPly"].ToString();
                //box3.Text = ds.Tables[0].Rows[i]["DESCREPTION"].ToString();
            }


        }
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
        int rowID = gvRow.RowIndex;
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            if (dt.Rows.Count > 1)
            {
                if (gvRow.RowIndex < dt.Rows.Count - 1)
                {
                    //Remove the Selected Row data and reset row number  
                    dt.Rows.Remove(dt.Rows[rowID]);
                    ResetRowID(dt);
                }
            }

            //Store the current data in ViewState for future reference  
            ViewState["CurrentTable"] = dt;

            //Re bind the GridView for the updated data  
            Gv1.DataSource = dt;
            Gv1.DataBind();
        }

        //Set Previous Data on Postbacks  
        SetPreviousData();
    }

    private void ResetRowID(DataTable dt)
    {
        int rowNumber = 1;
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                row[0] = rowNumber;
                rowNumber++;
            }
        }
    }

    private void Report()
    {
        DataSet ds = new DataSet();
        SqlParameter[] array = new SqlParameter[3];
        array[0] = new SqlParameter("@FileNo", SqlDbType.VarChar, 20);
        array[1] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);

        array[0].Value = txtFileNo.Text;
        array[1].Value = Session["varcompanyId"];

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForNewTechnicalDetailSheetReport", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptNewTechnicalDetailSheet.rpt";

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptNewTechnicalDetailSheet.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        Report();
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("../../Main.aspx");
    }

}