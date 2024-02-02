using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Data.SqlTypes;
using System.Text;
using System.Collections.Specialized;

public partial class Masters_Campany_FrmTechnicalDetailSheet : System.Web.UI.Page
{
    public static Boolean fileNoexist = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {

            if (!Page.IsPostBack)
            {
                SetInitialRow();
                SetInitialRow1();
                SetInitialRow3();
                SetInitialRow4();
                SetInitialRow5();
            }
            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            UtilityModule.ConditionalComboFill(ref dditemname, "select Item_Id,Item_Name from Item_Master  Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Item_name", true, "--Plz Select Item--");
            UtilityModule.ConditionalComboFill(ref dddesign, "SELECT DESIGNID,DESIGNNAME from DESIGN where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "Order By DESIGNNAME", true, "--Select--");
        }
    }
    private void SetInitialRow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("Column1", typeof(string)));
        dt.Columns.Add(new DataColumn("Column2", typeof(string)));
        dt.Columns.Add(new DataColumn("Column3", typeof(string)));
        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["Column1"] = string.Empty;
        dr["Column2"] = string.Empty;
        dr["Column3"] = string.Empty;
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
                    TextBox box3 = (TextBox)Gv1.Rows[rowIndex].Cells[3].FindControl("TextBox3");

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RowNumber"] = i + 1;

                    dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["Column3"] = box3.Text;

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
                    TextBox box3 = (TextBox)Gv1.Rows[rowIndex].Cells[3].FindControl("TextBox3");

                    box1.Text = dt.Rows[i]["Column1"].ToString();
                    box2.Text = dt.Rows[i]["Column2"].ToString();
                    box3.Text = dt.Rows[i]["Column3"].ToString();




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
    private void SetInitialRow1()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("Column1", typeof(string)));
        dt.Columns.Add(new DataColumn("Column2", typeof(string)));
        dt.Columns.Add(new DataColumn("Column3", typeof(string)));
        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["Column1"] = string.Empty;
        dr["Column2"] = string.Empty;
        dr["Column3"] = string.Empty;
        dt.Rows.Add(dr);
        //dr = dt.NewRow();

        //Store the DataTable in ViewState
        ViewState["CurrentTable1"] = dt;

        Gv2.DataSource = dt;
        Gv2.DataBind();
    }
    private void AddNewRowToGrid1()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable1"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable1"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox box1 = (TextBox)Gv2.Rows[rowIndex].Cells[1].FindControl("Tb1");
                    TextBox box2 = (TextBox)Gv2.Rows[rowIndex].Cells[2].FindControl("Tb2");
                    TextBox box3 = (TextBox)Gv2.Rows[rowIndex].Cells[3].FindControl("Tb3");

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RowNumber"] = i + 1;

                    dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["Column3"] = box3.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable1"] = dtCurrentTable;

                Gv2.DataSource = dtCurrentTable;
                Gv2.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }

        //Set Previous Data on Postbacks
        SetPreviousData1();
    }
    private void SetPreviousData1()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTable1"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable1"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box1 = (TextBox)Gv2.Rows[rowIndex].Cells[1].FindControl("Tb1");
                    TextBox box2 = (TextBox)Gv2.Rows[rowIndex].Cells[2].FindControl("Tb2");
                    TextBox box3 = (TextBox)Gv2.Rows[rowIndex].Cells[3].FindControl("Tb3");

                    box1.Text = dt.Rows[i]["Column1"].ToString();
                    box2.Text = dt.Rows[i]["Column2"].ToString();
                    box3.Text = dt.Rows[i]["Column3"].ToString();

                    rowIndex++;
                }
            }
        }
    }
    protected void ButtonAdd1_Click1(object sender, EventArgs e)
    {
        AddNewRowToGrid1();
    }
    //private void InsertRecords(StringCollection sc)
    //{

    //    SqlConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

    //    StringBuilder sb = new StringBuilder(string.Empty);

    //    string[] splitItems = null;

    //    foreach (string item in sc)
    //    {
    //        const string sqlStatement = "INSERT INTO TechnicalsheetWARP_WEFT_PILE (WARP_WEFT_PILE ,yarn,DESCREPTION,fileno,Userid,Mastercompanyid) VALUES";

    //        if (item.Contains(","))
    //        {

    //            splitItems = item.Split(",".ToCharArray());

    //            sb.AppendFormat("{0}('{1}','{2}','{3}'); ", sqlStatement, splitItems[0], splitItems[1], splitItems[2], txtfileno.Text, Session["varuserid"], Session["varMasterCompanyIDForERP"]);

    //        }
    //    }
    //    try
    //    {

    //        conn.Open();

    //        SqlCommand cmd = new SqlCommand(sb.ToString(), conn);

    //        cmd.CommandType = CommandType.Text;

    //        cmd.ExecuteNonQuery();



    //        //Display a popup which indicates that the record was successfully inserted

    //        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Records Successfuly Saved!');", true);
    //    }

    //    catch (System.Data.SqlClient.SqlException ex)
    //    {

    //        string msg = "Insert Error:";

    //        msg += ex.Message;

    //        throw new Exception(msg);
    //    }

    //    finally
    //    {

    //        conn.Close();

    //    }

    //}

    private void Gv1insertrecord(SqlTransaction Tran)
    {

        if (Gv1.Rows.Count != 0)
        {

            for (int i = 0; i < Gv1.Rows.Count; i++)
            {
                TextBox box1 = (TextBox)Gv1.Rows[i].Cells[1].FindControl("TextBox1");

                TextBox box2 = (TextBox)Gv1.Rows[i].Cells[2].FindControl("TextBox2");

                TextBox box3 = (TextBox)Gv1.Rows[i].Cells[3].FindControl("TextBox3");
                if (box1.Text != "" || box2.Text != "" || box3.Text != "")
                {
                    string str = @"Insert into TechnicalsheetWARP_WEFT_PILE(WARP_WEFT_PILE,yarn,DESCREPTION,fileno,Userid,Mastercompanyid)
                   values ('" + box1.Text + "','" + box2.Text + "','" + box3.Text + "','" + txtfileno.Text + "',1,9)";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                }
            }
        }

    }
    private void Gv2insertrecord(SqlTransaction Tran)
    {
        if (Gv2.Rows.Count != 0)
        {
            for (int i = 0; i < Gv2.Rows.Count; i++)
            {
                TextBox box1 = (TextBox)Gv2.Rows[i].Cells[1].FindControl("Tb1");

                TextBox box2 = (TextBox)Gv2.Rows[i].Cells[2].FindControl("Tb2");

                TextBox box3 = (TextBox)Gv2.Rows[i].Cells[3].FindControl("Tb3");
                if (box1.Text != "" || box2.Text != "" || box3.Text != "")
                {

                    string str = @"Insert into Technicalsheetforprocess(Processsteps,ProcessName,ResponsibleDepartment,fileno,Userid,Mastercompanyid)
                   values ('" + box1.Text + "','" + box2.Text + "','" + box3.Text + "','" + txtfileno.Text + "',1,9)";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                }
            }
        }
    }
    private void Gv3insertrecord(SqlTransaction Tran)
    {
        if (Gv3.Rows.Count != 0)
        {
            for (int i = 0; i < Gv3.Rows.Count; i++)
            {
                TextBox box1 = (TextBox)Gv3.Rows[i].Cells[1].FindControl("txtcustomer1");

                TextBox box2 = (TextBox)Gv3.Rows[i].Cells[2].FindControl("txtcustomer2");

                TextBox box3 = (TextBox)Gv3.Rows[i].Cells[3].FindControl("txtcustomer3");
                if (box1.Text != "" || box2.Text != "" || box3.Text != "")
                {
                    string str = @"Insert into detailcustomertestingrequirement(NameofTest,TestMethod,ScopeofTest,fileno,Userid,Mastercompanyid)
                   values ('" + box1.Text + "','" + box2.Text + "','" + box3.Text + "','" + txtfileno.Text + "'," + Session["varuserid"] + "," + Session["varMasterCompanyIDForERP"] + ")";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                }
            }

        }

    }
    private void Gv4insertrecord(SqlTransaction Tran)
    {

        if (Gv4.Rows.Count != 0)
        {
            for (int i = 0; i < Gv4.Rows.Count; i++)
            {
                TextBox box1 = (TextBox)Gv4.Rows[i].Cells[1].FindControl("txtcutting1");

                TextBox box2 = (TextBox)Gv4.Rows[i].Cells[2].FindControl("txtcutting2");

                if (box1.Text != "" || box2.Text != "")
                {

                    string str = @"Insert into DetailCutting(parametername,scopeofrequirement_test,otherinformation,FileNo,Userid,Mastercompanyid)
                   values ('" + box1.Text + "','" + box2.Text + "',N'" + txtotherinformation.Text + "','" + txtfileno.Text + "'," + Session["varuserid"] + "," + Session["varMasterCompanyIDForERP"] + ")";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);

                }
            }

        }

    }
    private void Gv5insertrecord(SqlTransaction Tran)
    {
        if (Gv5.Rows.Count != 0)
        {
            for (int i = 0; i < Gv5.Rows.Count; i++)
            {
                TextBox box1 = (TextBox)Gv5.Rows[i].Cells[1].FindControl("txtprocessname");
                TextBox box2 = (TextBox)Gv5.Rows[i].Cells[2].FindControl("txtwarp");
                TextBox box3 = (TextBox)Gv5.Rows[i].Cells[3].FindControl("txtweft");
                TextBox box4 = (TextBox)Gv5.Rows[i].Cells[4].FindControl("txtgrm");
                if (box1.Text != "" || box2.Text != "" || box3.Text != "" || box4.Text != "")
                {
                    string str = @"insert into processloss(processname,warp,weft,Specifygrm,fileno,otherinformation,userid,mastercompanyid)
                   values ('" + box1.Text + "','" + box2.Text + "','" + box3.Text + "','" + box4.Text + "','" + txtfileno.Text + "',N'" + txtotherinfo.Text.Trim() + "'," + Session["varuserid"] + "," + Session["varMasterCompanyIDForERP"] + ")";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                }
            }
        }
    }
    private void insertrecordweaving(SqlTransaction Tran)
    {
        string str = "";
        if (txtfinishweight.Text != "" || txtactualweight.Text != "" || txtreed.Text != "" || txtfeeding.Text != "" || txtpick.Text != "" || txtshaft.Text != "" || txtpunchcard.Text != "" || txtpilescale.Text != "" || txtpileheight.Text != "" || txtsizetollerance.Text != "" || txtweighttollerance.Text != "")
        {
            str = @"Insert into WEAVING(FINISHWEIGHT,ACTUALWEIGHT,REED,FEEDING,PICK_LINE,SHAFT,PUNCHCARD,PILESCALE,PILEHEIGHT,SIZETOLLERANCE,WEIGHTTOLLERANCE,FileNo,userid,Mastercompanyid)
            values ( N'" + txtfinishweight.Text + "',N'" + txtactualweight.Text + "',N'" + txtreed.Text + "',N'" + txtfeeding.Text + "',N'" + txtpick.Text + "',N'" + txtshaft.Text + "',N'" + txtpunchcard.Text + "',N'" + txtpilescale.Text + "',N'" + txtpileheight.Text + "',N'" + txtsizetollerance.Text + "',N'" + txtweighttollerance.Text + "','" + txtfileno.Text + "'," + Session["varuserid"] + "," + Session["varMasterCompanyIDForERP"] + ")";
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
        }
    }
    private void insertrecordends(SqlTransaction Tran)
    {
        if (txtfringes.Text != "" || txtlength.Text != "" || txtfoldededges.Text != "" || txtedges.Text != "" || txtplain.Text != "" || txtcortrise.Text != "" || txtbeedsground.Text != "" || txtbeeds.Text != "" || txtbinding.Text != "" || txtpoms.Text != "" || txtspecify1.Text != "" || txtlace.Text != "" || txtspecify2.Text != "" || txtwidth.Text != "" || txtlength1.Text != "" || txtallround.Text != "" || txtotherforends.Text != "" || txtfileno.Text != "")
        {
            string str = @"insert into detailends(FRINGES,LENGTH,FOLDEDEDGES,EDGESLENGTH_WIDTH,PLAIN,CORTRISE,BEEDS_GROUND,BEEDS,BINDING,POMS,SPECIFYNO_1SIDE,LACE,SPECIFYNO,WIDTH,detailLENGTH,ALL_AROUND,otherinformation,fileNo,userid,mastercompanyid,melt)
             values('" + txtfringes.Text + "','" + txtlength.Text + "','" + txtfoldededges.Text + "','" + txtedges.Text + "','" + txtplain.Text + "','" + txtcortrise.Text + "','" + txtbeedsground.Text + "','" + txtbeeds.Text + "','" + txtbinding.Text + "','" + txtpoms.Text + "','" + txtspecify1.Text + "','" + txtlace.Text + "','" + txtspecify2.Text + "','" + txtwidth.Text + "','" + txtlength1.Text + "','" + txtallround.Text + "',N'" + txtotherforends.Text.Trim() + "','" + txtfileno.Text + "'," + Session["varuserid"] + "," + Session["varMasterCompanyIDForERP"] + ",N'"+ txtmelt.Text + "')";

            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
        }
    }
    private void SetInitialRow3()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("Column1", typeof(string)));
        dt.Columns.Add(new DataColumn("Column2", typeof(string)));
        dt.Columns.Add(new DataColumn("Column3", typeof(string)));
        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["Column1"] = string.Empty;
        dr["Column2"] = string.Empty;
        dr["Column3"] = string.Empty;
        dt.Rows.Add(dr);
        //dr = dt.NewRow();

        //Store the DataTable in ViewState
        ViewState["CurrentTable2"] = dt;

        Gv3.DataSource = dt;
        Gv3.DataBind();
    }
    private void AddNewRowToGrid3()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable2"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable2"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox box1 = (TextBox)Gv3.Rows[rowIndex].Cells[1].FindControl("txtcustomer1");
                    TextBox box2 = (TextBox)Gv3.Rows[rowIndex].Cells[2].FindControl("txtcustomer2");
                    TextBox box3 = (TextBox)Gv3.Rows[rowIndex].Cells[3].FindControl("txtcustomer3");

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RowNumber"] = i + 1;

                    dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["Column3"] = box3.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable2"] = dtCurrentTable;

                Gv3.DataSource = dtCurrentTable;
                Gv3.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }

        //Set Previous Data on Postbacks
        SetPreviousData2();
    }
    private void SetPreviousData2()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTable2"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable2"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box1 = (TextBox)Gv3.Rows[rowIndex].Cells[1].FindControl("txtcustomer1");
                    TextBox box2 = (TextBox)Gv3.Rows[rowIndex].Cells[2].FindControl("txtcustomer2");
                    TextBox box3 = (TextBox)Gv3.Rows[rowIndex].Cells[3].FindControl("txtcustomer3");

                    box1.Text = dt.Rows[i]["Column1"].ToString();
                    box2.Text = dt.Rows[i]["Column2"].ToString();
                    box3.Text = dt.Rows[i]["Column3"].ToString();

                    rowIndex++;
                }
            }
        }
    }
    protected void btnadd2_click(object sender, EventArgs e)
    {
        AddNewRowToGrid3();
    }
    private void SetInitialRow4()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("Column1", typeof(string)));
        dt.Columns.Add(new DataColumn("Column2", typeof(string)));

        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["Column1"] = string.Empty;
        dr["Column2"] = string.Empty;

        dt.Rows.Add(dr);
        //dr = dt.NewRow();

        //Store the DataTable in ViewState
        ViewState["CurrentTable3"] = dt;

        Gv4.DataSource = dt;
        Gv4.DataBind();
    }
    private void AddNewRowToGrid4()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable3"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable3"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox box1 = (TextBox)Gv4.Rows[rowIndex].Cells[1].FindControl("txtcutting1");
                    TextBox box2 = (TextBox)Gv4.Rows[rowIndex].Cells[2].FindControl("txtcutting2");


                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RowNumber"] = i + 1;

                    dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;


                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable3"] = dtCurrentTable;

                Gv4.DataSource = dtCurrentTable;
                Gv4.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }

        //Set Previous Data on Postbacks
        SetPreviousData3();
    }
    private void SetPreviousData3()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTable3"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable3"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box1 = (TextBox)Gv4.Rows[rowIndex].Cells[1].FindControl("txtcutting1");
                    TextBox box2 = (TextBox)Gv4.Rows[rowIndex].Cells[2].FindControl("txtcutting2");


                    box1.Text = dt.Rows[i]["Column1"].ToString();
                    box2.Text = dt.Rows[i]["Column2"].ToString();


                    rowIndex++;
                }
            }
        }
    }

    protected void btnadd3_click(object sender, EventArgs e)
    {
        AddNewRowToGrid4();
    }
    private void SetInitialRow5()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("Column1", typeof(string)));
        dt.Columns.Add(new DataColumn("Column2", typeof(string)));
        dt.Columns.Add(new DataColumn("Column3", typeof(string)));
        dt.Columns.Add(new DataColumn("Column4", typeof(string)));

        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["Column1"] = string.Empty;
        dr["Column2"] = string.Empty;
        dr["Column3"] = string.Empty;
        dr["Column4"] = string.Empty;

        dt.Rows.Add(dr);
        //dr = dt.NewRow();

        //Store the DataTable in ViewState
        ViewState["CurrentTable4"] = dt;

        Gv5.DataSource = dt;
        Gv5.DataBind();
    }
    private void AddNewRowToGrid5()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable4"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable4"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox box1 = (TextBox)Gv5.Rows[rowIndex].Cells[1].FindControl("txtprocessname");
                    TextBox box2 = (TextBox)Gv5.Rows[rowIndex].Cells[2].FindControl("txtwarp");
                    TextBox box3 = (TextBox)Gv5.Rows[rowIndex].Cells[3].FindControl("txtweft");
                    TextBox box4 = (TextBox)Gv5.Rows[rowIndex].Cells[4].FindControl("txtgrm");


                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RowNumber"] = i + 1;

                    dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["Column3"] = box3.Text;
                    dtCurrentTable.Rows[i - 1]["Column4"] = box4.Text;


                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable4"] = dtCurrentTable;

                Gv5.DataSource = dtCurrentTable;
                Gv5.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }

        //Set Previous Data on Postbacks
        SetPreviousData5();
    }
    private void SetPreviousData5()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTable4"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable4"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box1 = (TextBox)Gv5.Rows[rowIndex].Cells[1].FindControl("txtprocessname");
                    TextBox box2 = (TextBox)Gv5.Rows[rowIndex].Cells[2].FindControl("txtwarp");
                    TextBox box3 = (TextBox)Gv5.Rows[rowIndex].Cells[3].FindControl("txtweft");
                    TextBox box4 = (TextBox)Gv5.Rows[rowIndex].Cells[4].FindControl("txtgrm");


                    box1.Text = dt.Rows[i]["Column1"].ToString();
                    box2.Text = dt.Rows[i]["Column2"].ToString();
                    box3.Text = dt.Rows[i]["Column3"].ToString();
                    box4.Text = dt.Rows[i]["Column4"].ToString();


                    rowIndex++;
                }
            }
        }
    }
    protected void btnadd4_click(object sender, EventArgs e)
    {
        AddNewRowToGrid5();
    }

    protected void btnsave_Click1(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //StringCollection sc = new StringCollection();
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[4];
            _arrpara[0] = new SqlParameter("@fileno", SqlDbType.VarChar, 100);
            _arrpara[1] = new SqlParameter("@Version", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@UserId", SqlDbType.Int);


            System.Data.SqlTypes.SqlDateTime getDate;
            //set DateTime null
            getDate = SqlDateTime.Null;


            _arrpara[0].Value = txtfileno.Text;
            _arrpara[1].Direction = ParameterDirection.Output;
            _arrpara[2].Value = Session["varuserid"].ToString();
            //Create FileVersion
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_fileversion", _arrpara);
            txtversion.Text = _arrpara[1].Value.ToString();
            //

            // at the time of update delete all the data in tables
            string str1 = @"delete from Technicaldetail where fileno = '" + txtfileno.Text + @"'
                            delete from  TechnicalsheetWARP_WEFT_PILE where fileno = '" + txtfileno.Text + @"'
                            delete from Technicalsheetforprocess where fileno ='" + txtfileno.Text + @"'
                            delete from detailcustomertestingrequirement where fileno ='" + txtfileno.Text + @"'
                            delete from DetailCutting where fileno ='" + txtfileno.Text + @"'
                            delete from processloss where fileno ='" + txtfileno.Text + @"'
                            delete from WEAVING where fileno = '" + txtfileno.Text + @"'
                            delete from detailends where fileno = '" + txtfileno.Text + "'";
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);
            // string str = "insert into Technicaldetail(Masterquality,fileno,Subquality,Version,Ourref,date,Design,UserId,MasterCompanyId)values('" +txtmasterquality.Text + "','" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "','" + _arrpara[4].Value + "','" + _arrpara[5].Value + "','" + _arrpara[6].Value + "', " + Session["varuserid"].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + ")";
            string str = "insert into Technicaldetail(ITEM_ID,fileno,QualityID,Version,Ourref,date,Designid,UserId,MasterCompanyId,LocalOrderNo)values('" + dditemname.SelectedValue + "','" + txtfileno.Text + "','" + ddsubquality.SelectedValue + "','" + txtversion.Text + "','" + txtourref.Text + "','" + txtdate.Text + "','" + dddesign.SelectedValue + "', " + Session["varuserid"].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + ",'" + txtsrno.Text +"')";
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            //InsertRecords(sc);

            Gv1insertrecord(Tran);
            Gv2insertrecord(Tran);
            Gv3insertrecord(Tran);
            Gv4insertrecord(Tran);
            Gv5insertrecord(Tran);
            insertrecordweaving(Tran);
            insertrecordends(Tran);


            Tran.Commit();
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('Data Saved Successfully....')", true);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/FrmTechnicalDetailSheet.aspx");
            Tran.Rollback();
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (con != null)
            {
                con.Dispose();
            }
        }

    }

    protected void chkcustomertesting_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcustomertesting.Checked == true)
        {
            cutomertestingrequriment.Visible = true;
        }
        else
        {
            cutomertestingrequriment.Visible = false;
        }
    }
    protected void chkcutting_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcutting.Checked == true)
        {
            detailcutting.Visible = true;
        }
        else
        {
            detailcutting.Visible = false;
        }


    }
    protected void chkforweaving_CheckedChanged(object sender, EventArgs e)
    {
        if (chkforweaving.Checked == true)
        {
            detailweaving.Visible = true;
        }
        else
        {
            detailweaving.Visible = false;
        }

    }
    protected void chkends_CheckedChanged(object sender, EventArgs e)
    {
        if (chkends.Checked == true)
        {
            detailends.Visible = true;
        }
        else
        {
            detailends.Visible = false;
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        Report();
    }
    private void Report()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            lblMessage.Text = "";
            SqlParameter[] _array = new SqlParameter[8];
            _array[0] = new SqlParameter("@fileno", SqlDbType.VarChar, 100);
            _array[1] = new SqlParameter("@Warp_WeftPrint", SqlDbType.TinyInt);
            _array[2] = new SqlParameter("@ProcessPrint", SqlDbType.TinyInt);
            _array[3] = new SqlParameter("@CustomertestingPrint", SqlDbType.TinyInt);
            _array[4] = new SqlParameter("@CuttingPrint", SqlDbType.TinyInt);
            _array[5] = new SqlParameter("@WeavingPrint", SqlDbType.TinyInt);
            _array[6] = new SqlParameter("@ProcessLossPrint", SqlDbType.TinyInt);
            _array[7] = new SqlParameter("@Endsprint", SqlDbType.TinyInt);


            _array[0].Value = txtfileno.Text;
            _array[1].Value = ChkWarp_WeftPrint.Checked == true ? 1 : 0;
            _array[2].Value = chkDefineProcess.Checked == true ? 1 : 0;
            _array[3].Value = chkcustomertesting.Checked == true ? 1 : 0;
            _array[4].Value = chkcutting.Checked == true ? 1 : 0;
            _array[5].Value = chkforweaving.Checked == true ? 1 : 0;
            _array[6].Value = ChkProcessLoss.Checked == true ? 1 : 0;
            _array[7].Value = chkends.Checked == true ? 1 : 0;

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "pro_tecnicaldetailsheet", _array);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptTechnicalDetailSheet.rpt";
                //Session["rptFileName"] = Session["ReportPath"];
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptTechnicalDetailSheet.xsd";
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
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }

    }

    protected void txtfileno_TextChanged(object sender, EventArgs e)
    {

        try
        {
            fileNoexist = false;
            if (txtfileno.Text != "")
            {
                fill_GV1();
                fill_GV2();
                fill_GV3();
                fill_GV4();
                fill_GV5();
                fill_ends();
                chkcustomertesting_CheckedChanged(sender, e);
                chkcutting_CheckedChanged(sender, e);
                chkforweaving_CheckedChanged(sender, e);
                chkends_CheckedChanged(sender, e);
            }
            if (fileNoexist == false)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(),"alt","alert('File No. does not exist..');", true);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;

        }
        finally
        {


        }

    }
    protected void fill_GV1()
    {
        string str1 = "select ITEM_ID,QualityID,F.Version,Ourref,Replace(convert(nvarchar(11),TC.date,106),' ','-') As Date,Designid,LocalOrderNo from Technicaldetail TC,FileVersion F where TC.FileNo=F.FileNo and  TC.fileno='" + txtfileno.Text + "'";

        DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str1);

        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            dditemname.SelectedValue = ds1.Tables[0].Rows[i]["ITEM_ID"].ToString();
            UtilityModule.ConditionalComboFill(ref ddsubquality, "select QualityId,QualityName from quality where Item_id=" + dditemname.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "Order By QualityName", true, "-- Pls Select Quality--");
            ddsubquality.SelectedValue = ds1.Tables[0].Rows[i]["QualityID"].ToString();
            txtversion.Text = ds1.Tables[0].Rows[i]["Version"].ToString();
            txtourref.Text = ds1.Tables[0].Rows[i]["Ourref"].ToString();
            txtdate.Text = ds1.Tables[0].Rows[i]["date"].ToString();
            dddesign.SelectedValue = ds1.Tables[0].Rows[i]["DesignId"].ToString();
            txtsrno.Text = ds1.Tables[0].Rows[i]["LocalOrderNo"].ToString();
            fileNoexist = true;
        }


        string str = "Select WARP_WEFT_PILE,yarn,DESCREPTION from  TechnicalsheetWARP_WEFT_PILE where fileno='" + txtfileno.Text + "'";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if (i == 0)
            {
                TextBox box1 = (TextBox)Gv1.Rows[i].Cells[1].FindControl("TextBox1");
                TextBox box2 = (TextBox)Gv1.Rows[i].Cells[2].FindControl("TextBox2");
                TextBox box3 = (TextBox)Gv1.Rows[i].Cells[3].FindControl("TextBox3");

                box1.Text = ds.Tables[0].Rows[i]["WARP_WEFT_PILE"].ToString();
                box2.Text = ds.Tables[0].Rows[i]["yarn"].ToString();
                box3.Text = ds.Tables[0].Rows[i]["DESCREPTION"].ToString();
            }
            else
            {
                AddNewRowToGrid();
                TextBox box1 = (TextBox)Gv1.Rows[i].Cells[1].FindControl("TextBox1");
                TextBox box2 = (TextBox)Gv1.Rows[i].Cells[2].FindControl("TextBox2");
                TextBox box3 = (TextBox)Gv1.Rows[i].Cells[3].FindControl("TextBox3");

                box1.Text = ds.Tables[0].Rows[i]["WARP_WEFT_PILE"].ToString();
                box2.Text = ds.Tables[0].Rows[i]["yarn"].ToString();
                box3.Text = ds.Tables[0].Rows[i]["DESCREPTION"].ToString();
            }


        }
    }
    protected void fill_GV2()
    {
        string str = "select Processsteps,ProcessName,ResponsibleDepartment from Technicalsheetforprocess where fileno='" + txtfileno.Text + "'";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if (i == 0)
            {
                TextBox box1 = (TextBox)Gv2.Rows[i].Cells[1].FindControl("Tb1");
                TextBox box2 = (TextBox)Gv2.Rows[i].Cells[2].FindControl("Tb2");
                TextBox box3 = (TextBox)Gv2.Rows[i].Cells[3].FindControl("Tb3");

                box1.Text = ds.Tables[0].Rows[i]["Processsteps"].ToString();
                box2.Text = ds.Tables[0].Rows[i]["ProcessName"].ToString();
                box3.Text = ds.Tables[0].Rows[i]["ResponsibleDepartment"].ToString();
            }
            else
            {
                AddNewRowToGrid1();
                TextBox box1 = (TextBox)Gv2.Rows[i].Cells[1].FindControl("Tb1");
                TextBox box2 = (TextBox)Gv2.Rows[i].Cells[2].FindControl("Tb2");
                TextBox box3 = (TextBox)Gv2.Rows[i].Cells[3].FindControl("Tb3");

                box1.Text = ds.Tables[0].Rows[i]["Processsteps"].ToString();
                box2.Text = ds.Tables[0].Rows[i]["ProcessName"].ToString();
                box3.Text = ds.Tables[0].Rows[i]["ResponsibleDepartment"].ToString();
            }


        }
    }
    protected void fill_GV3()
    {
        string str = "select NameofTest,TestMethod,ScopeofTest from detailcustomertestingrequirement where fileno='" + txtfileno.Text + "'";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {

            chkcustomertesting.Checked = true;
            if (i == 0)
            {
                TextBox box1 = (TextBox)Gv3.Rows[i].Cells[1].FindControl("txtcustomer1");
                TextBox box2 = (TextBox)Gv3.Rows[i].Cells[2].FindControl("txtcustomer2");
                TextBox box3 = (TextBox)Gv3.Rows[i].Cells[3].FindControl("txtcustomer3");

                box1.Text = ds.Tables[0].Rows[i]["NameofTest"].ToString();
                box2.Text = ds.Tables[0].Rows[i]["TestMethod"].ToString();
                box3.Text = ds.Tables[0].Rows[i]["ScopeofTest"].ToString();

            }
            else
            {
                AddNewRowToGrid3();
                TextBox box1 = (TextBox)Gv3.Rows[i].Cells[1].FindControl("txtcustomer1");
                TextBox box2 = (TextBox)Gv3.Rows[i].Cells[2].FindControl("txtcustomer2");
                TextBox box3 = (TextBox)Gv3.Rows[i].Cells[3].FindControl("txtcustomer3");

                box1.Text = ds.Tables[0].Rows[i]["NameofTest"].ToString();
                box2.Text = ds.Tables[0].Rows[i]["TestMethod"].ToString();
                box3.Text = ds.Tables[0].Rows[i]["ScopeofTest"].ToString();
            }


        }
    }
    protected void fill_GV4()
    {
        string str = "select parametername,scopeofrequirement_test,otherinformation from DetailCutting where fileno='" + txtfileno.Text + "'";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            chkcutting.Checked = true;
            if (i == 0)
            {
                txtotherinformation.Text = ds.Tables[0].Rows[i]["otherinformation"].ToString();
                TextBox box1 = (TextBox)Gv4.Rows[i].Cells[1].FindControl("txtcutting1");
                TextBox box2 = (TextBox)Gv4.Rows[i].Cells[2].FindControl("txtcutting2");


                box1.Text = ds.Tables[0].Rows[i]["parametername"].ToString();
                box2.Text = ds.Tables[0].Rows[i]["scopeofrequirement_test"].ToString();

            }
            else
            {
                AddNewRowToGrid4();
                TextBox box1 = (TextBox)Gv4.Rows[i].Cells[1].FindControl("txtcutting1");
                TextBox box2 = (TextBox)Gv4.Rows[i].Cells[2].FindControl("txtcutting2");


                box1.Text = ds.Tables[0].Rows[i]["parametername"].ToString();
                box2.Text = ds.Tables[0].Rows[i]["scopeofrequirement_test"].ToString();
            }


        }
    }
    protected void fill_GV5()
    {
        string str1 = "select FINISHWEIGHT,ACTUALWEIGHT,REED,FEEDING,PICK_LINE,SHAFT,PUNCHCARD,PILESCALE,PILEHEIGHT,SIZETOLLERANCE,WEIGHTTOLLERANCE from WEAVING where fileno='" + txtfileno.Text + "'";

        DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str1);
        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            chkforweaving.Checked = true;
            txtfinishweight.Text = ds1.Tables[0].Rows[i]["FINISHWEIGHT"].ToString();
            txtactualweight.Text = ds1.Tables[0].Rows[i]["ACTUALWEIGHT"].ToString();
            txtreed.Text = ds1.Tables[0].Rows[i]["REED"].ToString();
            txtfeeding.Text = ds1.Tables[0].Rows[i]["FEEDING"].ToString();
            txtpick.Text = ds1.Tables[0].Rows[i]["PICK_LINE"].ToString();
            txtshaft.Text = ds1.Tables[0].Rows[i]["SHAFT"].ToString();
            txtpunchcard.Text = ds1.Tables[0].Rows[i]["PUNCHCARD"].ToString();
            txtpilescale.Text = ds1.Tables[0].Rows[i]["PILESCALE"].ToString();
            txtpileheight.Text = ds1.Tables[0].Rows[i]["PILEHEIGHT"].ToString();
            txtsizetollerance.Text = ds1.Tables[0].Rows[i]["SIZETOLLERANCE"].ToString();
            txtweighttollerance.Text = ds1.Tables[0].Rows[i]["WEIGHTTOLLERANCE"].ToString();

        }

        string str = "select processname,warp,weft,Specifygrm,otherinformation from processloss where fileno='" + txtfileno.Text + "'";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            chkforweaving.Checked = true;
            if (i == 0)
            {

                txtotherinfo.Text = ds.Tables[0].Rows[i]["otherinformation"].ToString();
                TextBox box1 = (TextBox)Gv5.Rows[i].Cells[1].FindControl("txtprocessname");
                TextBox box2 = (TextBox)Gv5.Rows[i].Cells[2].FindControl("txtwarp");
                TextBox box3 = (TextBox)Gv5.Rows[i].Cells[3].FindControl("txtweft");
                TextBox box4 = (TextBox)Gv5.Rows[i].Cells[4].FindControl("txtgrm");


                box1.Text = ds.Tables[0].Rows[i]["processname"].ToString();
                box2.Text = ds.Tables[0].Rows[i]["warp"].ToString();
                box3.Text = ds.Tables[0].Rows[i]["weft"].ToString();
                box4.Text = ds.Tables[0].Rows[i]["Specifygrm"].ToString();
            }
            else
            {
                AddNewRowToGrid5();
                TextBox box1 = (TextBox)Gv5.Rows[i].Cells[1].FindControl("txtprocessname");
                TextBox box2 = (TextBox)Gv5.Rows[i].Cells[2].FindControl("txtwarp");
                TextBox box3 = (TextBox)Gv5.Rows[i].Cells[3].FindControl("txtweft");
                TextBox box4 = (TextBox)Gv5.Rows[i].Cells[4].FindControl("txtgrm");


                box1.Text = ds.Tables[0].Rows[i]["processname"].ToString();
                box2.Text = ds.Tables[0].Rows[i]["warp"].ToString();
                box3.Text = ds.Tables[0].Rows[i]["weft"].ToString();
                box4.Text = ds.Tables[0].Rows[i]["Specifygrm"].ToString();
            }


        }
    }
    protected void fill_ends()
    {
        string str = "select FRINGES,LENGTH,FOLDEDEDGES,EDGESLENGTH_WIDTH,PLAIN,CORTRISE,BEEDS_GROUND,BEEDS,BINDING,POMS,SPECIFYNO_1SIDE,LACE,SPECIFYNO,WIDTH,detailLENGTH,ALL_AROUND,otherinformation,Melt from detailends where fileno='" + txtfileno.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            chkends.Checked = true;
            txtfringes.Text = ds.Tables[0].Rows[i]["FRINGES"].ToString();
            txtlength.Text = ds.Tables[0].Rows[i]["LENGTH"].ToString();
            txtfoldededges.Text = ds.Tables[0].Rows[i]["FOLDEDEDGES"].ToString();
            txtedges.Text = ds.Tables[0].Rows[i]["EDGESLENGTH_WIDTH"].ToString();
            txtplain.Text = ds.Tables[0].Rows[i]["PLAIN"].ToString();
            txtcortrise.Text = ds.Tables[0].Rows[i]["CORTRISE"].ToString();
            txtbeedsground.Text = ds.Tables[0].Rows[i]["BEEDS_GROUND"].ToString();
            txtbeeds.Text = ds.Tables[0].Rows[i]["BEEDS"].ToString();
            txtbinding.Text = ds.Tables[0].Rows[i]["BINDING"].ToString();
            txtpoms.Text = ds.Tables[0].Rows[i]["POMS"].ToString();
            txtspecify1.Text = ds.Tables[0].Rows[i]["SPECIFYNO_1SIDE"].ToString();
            txtlace.Text = ds.Tables[0].Rows[i]["LACE"].ToString();
            txtspecify2.Text = ds.Tables[0].Rows[i]["SPECIFYNO"].ToString();
            txtwidth.Text = ds.Tables[0].Rows[i]["WIDTH"].ToString();
            txtlength1.Text = ds.Tables[0].Rows[i]["detailLENGTH"].ToString();
            txtallround.Text = ds.Tables[0].Rows[i]["ALL_AROUND"].ToString();
            txtotherforends.Text = ds.Tables[0].Rows[i]["otherinformation"].ToString();
            txtmelt.Text = ds.Tables[0].Rows[i]["Melt"].ToString();
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {

        UtilityModule.ConditionalComboFill(ref ddsubquality, "select QualityId,QualityName from quality where Item_id=" + dditemname.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "Order By QualityName", true, "-- Pls Select Quality--");

    }
    

}