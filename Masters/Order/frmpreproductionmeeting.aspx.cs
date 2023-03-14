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

public partial class Masters_Order_frmpreproductionmeeting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            SetInitialRow_Constructionyarndetail();
            SetInitialRow_Tolerence();
            string str = @"select CI.CompanyId,CI.CompanyName from CompanyInfo CI inner join Company_Authentication CA on Ci.CompanyId=CA.CompanyId
                      WHere CI.MasterCompanyid=" + Session["varcompanyid"] + " and CA.UserId=" + Session["varuserid"] + @"  order by CompanyName
                      select CustomerId,customercode+'  '+companyname from customerinfo where MasterCompanyid=" + Session["varcompanyid"] + @"  order by CustomerCode
                      select PROCESS_NAME_ID,PROCESS_NAME From Process_Name_Master Where ProcessType=1 order by SeqNo,process_name";


            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDCustomer, ds, 1, true, "--Select Buyer--");
            UtilityModule.ConditionalComboFillWithDS(ref DDDept, ds, 2, true, "--Plz Select--");
            txtppmmeetingdt.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        hndocid.Value = "0";
        if (chkedit.Checked == true)
        {
            string str = @"SELECT DOCID,DOCNO + ' # ' +REPLACE(convert(nvarchar(11),PPMMEETINGDT,106),' ','-') as Docno FROM PPMMASTER_NEW WHERE COMPANYID=" + DDCompanyName.SelectedValue + " AND CUSTOMERID=" + DDCustomer.SelectedValue + " order by DOCID";
            UtilityModule.ConditionalComboFill(ref DDDocNo, str, true, "--Plz Select--");
        }
        UtilityModule.ConditionalComboFill(ref DDOrderNo, "select OM.OrderId,OM.CustomerOrderNo as CustomerOrderNo from ordermaster OM Where OM.CustomerId=" + DDCustomer.SelectedValue + " and OM.CompanyId=" + DDCompanyName.SelectedValue + " and OM.status=0 order by OM.OrderId", true, "--Select Order No.--");
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDItemdescription, @"SELECT DISTINCT VF.ITEM_FINISHED_ID,VF.ITEM_NAME+' '+VF.QUALITYNAME+' '+VF.DESIGNNAME+'  '+VF.COLORNAME+' '+VF.SHAPENAME+'  '+CASE WHEN OD.FLAGSIZE=1 THEN VF.SIZEMTR 
                                                                    ELSE SIZEFT END AS ITEMDESCRIPTION FROM ORDERDETAIL OD INNER JOIN V_FINISHEDITEMDETAIL VF ON OD.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID WHere od.orderid=" + DDOrderNo.SelectedValue + " order by Itemdescription", true, "--Plz Select--");

    }
    protected void lbladdpono_Click(object sender, EventArgs e)
    {
        if (DDOrderNo.SelectedIndex > 0)
        {
            if (listpono.Items.FindByValue(DDOrderNo.SelectedValue) == null)
            {
                listpono.Items.Add(new ListItem(DDOrderNo.SelectedItem.Text, DDOrderNo.SelectedValue));
            }
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        listpono.Items.Remove(listpono.SelectedItem);
    }
    protected void lnkitemdescription_Click(object sender, EventArgs e)
    {
        if (DDItemdescription.SelectedIndex > 0)
        {
            if (listitemdesc.Items.FindByValue(DDItemdescription.SelectedValue) == null)
            {
                listitemdesc.Items.Add(new ListItem(DDItemdescription.SelectedItem.Text, DDItemdescription.SelectedValue));
            }
        }
    }
    protected void lnkdelitem_Click(object sender, EventArgs e)
    {
        listitemdesc.Items.Remove(listitemdesc.SelectedItem);
    }
    private void SetInitialRow_Constructionyarndetail()
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
        ViewState["constructionyarn"] = dt;

        DGConstructionyarndetail.DataSource = dt;
        DGConstructionyarndetail.DataBind();
    }
    private void SetInitialRow_Tolerence()
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
        ViewState["tolerence"] = dt;

        DGTolerence.DataSource = dt;
        DGTolerence.DataBind();
    }
    protected void ButtonAddconstruction_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid_Construction();
    }
    private void AddNewRowToGrid_Construction()
    {
        int rowIndex = 0;

        if (ViewState["constructionyarn"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["constructionyarn"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox box1 = (TextBox)DGConstructionyarndetail.Rows[rowIndex].Cells[1].FindControl("TextBox1");
                    TextBox box2 = (TextBox)DGConstructionyarndetail.Rows[rowIndex].Cells[2].FindControl("TextBox2");
                    TextBox box3 = (TextBox)DGConstructionyarndetail.Rows[rowIndex].Cells[3].FindControl("TextBox3");

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RowNumber"] = i + 1;

                    dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["Column3"] = box3.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["constructionyarn"] = dtCurrentTable;

                DGConstructionyarndetail.DataSource = dtCurrentTable;
                DGConstructionyarndetail.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }

        //Set Previous Data on Postbacks
        SetPreviousData_Constructionyarn();
    }
    private void SetPreviousData_Constructionyarn()
    {
        int rowIndex = 0;
        //StringCollection sc = new StringCollection();
        if (ViewState["constructionyarn"] != null)
        {
            DataTable dt = (DataTable)ViewState["constructionyarn"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box1 = (TextBox)DGConstructionyarndetail.Rows[rowIndex].Cells[1].FindControl("TextBox1");
                    TextBox box2 = (TextBox)DGConstructionyarndetail.Rows[rowIndex].Cells[2].FindControl("TextBox2");
                    TextBox box3 = (TextBox)DGConstructionyarndetail.Rows[rowIndex].Cells[3].FindControl("TextBox3");

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
    protected void DGvendor_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (ViewState["jobdetail"] != null)
        {
            DataTable dt = (DataTable)ViewState["jobdetail"];
            int rowindex = e.RowIndex;
            if (dt.Rows.Count > 0)
            {
                dt.Rows.Remove(dt.Rows[rowindex]);
                ViewState["jobdetail"] = dt;
                DGjobdetail.DataSource = dt;
                DGjobdetail.DataBind();
            }
        }
    }
    protected void btnaddjobdetail_Click(object sender, EventArgs e)
    {
        if (ViewState["jobdetail"] == null)
        {
            setinitialrowForjobDetail();
        }
        else
        {
            AddnewrowtoJobdetailGrid();
        }
        DGjobdetail.UseAccessibleHeader = true;
        DGjobdetail.HeaderRow.TableSection = TableRowSection.TableHeader;
        txtjobcomments.Text = "";
    }
    protected void setinitialrowForjobDetail()
    {
        DataTable dt = new DataTable();
        DataRow DR = null;
        dt.Columns.Add("Jobname", typeof(string));
        dt.Columns.Add("jobvalue", typeof(string));
        dt.Columns.Add("comments", typeof(string));
        dt.Columns.Add("jobid", typeof(int));

        DR = dt.NewRow();
        DR["Jobname"] = DDDept.SelectedItem.Text;
        DR["jobvalue"] = DDjobvalue.SelectedItem.Text;
        DR["comments"] = txtjobcomments.Text.Trim();
        DR["Jobid"] = DDDept.SelectedValue;

        dt.Rows.Add(DR);


        ViewState["jobdetail"] = dt;
        DGjobdetail.DataSource = dt;
        DGjobdetail.DataBind();

    }
    protected void AddnewrowtoJobdetailGrid()
    {
        if (ViewState["jobdetail"] != null)
        {
            DataTable dtcurrentTable = (DataTable)ViewState["jobdetail"];
            DataRow drcurrentRow = null;

            drcurrentRow = dtcurrentTable.NewRow();
            drcurrentRow["Jobname"] = DDDept.SelectedItem.Text;
            drcurrentRow["jobvalue"] = DDjobvalue.SelectedItem.Text;
            drcurrentRow["comments"] = txtjobcomments.Text.Trim();
            drcurrentRow["Jobid"] = DDDept.SelectedValue;
            dtcurrentTable.Rows.Add(drcurrentRow);

            ViewState["jobdetail"] = dtcurrentTable;

            DGjobdetail.DataSource = dtcurrentTable;
            DGjobdetail.DataBind();
        }
    }
    protected void ButtonAddTolerence_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid_Tolerence();
    }
    private void AddNewRowToGrid_Tolerence()
    {
        int rowIndex = 0;

        if (ViewState["tolerence"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["tolerence"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox box1 = (TextBox)DGTolerence.Rows[rowIndex].Cells[1].FindControl("TextBox1");
                    TextBox box2 = (TextBox)DGTolerence.Rows[rowIndex].Cells[2].FindControl("TextBox2");


                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RowNumber"] = i + 1;

                    dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;


                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["tolerence"] = dtCurrentTable;

                DGTolerence.DataSource = dtCurrentTable;
                DGTolerence.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }

        //Set Previous Data on Postbacks
        SetPreviousData_Tolerence();
    }
    private void SetPreviousData_Tolerence()
    {
        int rowIndex = 0;
        //StringCollection sc = new StringCollection();
        if (ViewState["tolerence"] != null)
        {
            DataTable dt = (DataTable)ViewState["tolerence"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box1 = (TextBox)DGTolerence.Rows[rowIndex].Cells[1].FindControl("TextBox1");
                    TextBox box2 = (TextBox)DGTolerence.Rows[rowIndex].Cells[2].FindControl("TextBox2");


                    box1.Text = dt.Rows[i]["Column1"].ToString();
                    box2.Text = dt.Rows[i]["Column2"].ToString();





                    // sc.Add(box1.Text + "," + box2.Text + "," + box3.Text);



                    rowIndex++;


                }

                //InsertRecords(sc);
            }
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        string Orderids = "", Item_finished_ids = "";
        //*******orderids
        for (int i = 0; i < listpono.Items.Count; i++)
        {
            Orderids = Orderids + "," + listpono.Items[i].Value;
        }
        Orderids = Orderids.TrimStart(',');
        //******Item_finished_ids
        for (int i = 0; i < listitemdesc.Items.Count; i++)
        {
            Item_finished_ids = Item_finished_ids + "," + listitemdesc.Items[i].Value;
        }
        Item_finished_ids = Item_finished_ids.TrimStart(',');

        if (Orderids == "" || Item_finished_ids == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please add atlest one Po No and Item Description')", true);
            return;
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();

        try
        {
            SqlParameter[] param = new SqlParameter[20];
            param[0] = new SqlParameter("@Docid", SqlDbType.Int);
            param[0].Value = hndocid.Value;
            param[0].Direction = ParameterDirection.InputOutput;
            param[1] = new SqlParameter("@companyId", DDCompanyName.SelectedValue);
            param[2] = new SqlParameter("@customerid", DDCustomer.SelectedValue);
            param[3] = new SqlParameter("@orderids", Orderids);
            param[4] = new SqlParameter("@Item_finished_ids", Item_finished_ids);
            param[5] = new SqlParameter("@Meetingdt", txtppmmeetingdt.Text);
            param[6] = new SqlParameter("@DocNo", SqlDbType.VarChar, 50);
            param[6].Value = txtdocno.Text;
            param[6].Direction = ParameterDirection.InputOutput;
            param[7] = new SqlParameter("@Remark", txtremarks.Text);
            param[8] = new SqlParameter("@userid", Session["varuserid"]);
            param[9] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[9].Direction = ParameterDirection.Output;
            //***********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEPPMMASTER_NEW", param);

            hndocid.Value = param[0].Value.ToString();
            txtdocno.Text = param[6].Value.ToString();
            lblmsg.Text = param[9].Value.ToString();

            // at the time of update delete all the data in tables
            string str1 = @"DELETE FROM PPM_CONSTRUCTION_YARNDETAIL WHERE DOCID=" + hndocid.Value + @"
                            DELETE FROM PPM_CONSTRUCTION WHERE DOCID=" + hndocid.Value + @"
                            DELETE FROM PPM_TESTINGREQUIREMENTS WHERE DOCID=" + hndocid.Value + @"
                            DELETE FROM PPM_JOBDETAIL WHERE DOCID=" + hndocid.Value + @"
                            DELETE FROM PPM_TOLERENCE WHERE DOCID=" + hndocid.Value + @"
                            DELETE FROM PPM_PACKING WHERE DOCID=" + hndocid.Value + @"
                            DELETE FROM PPM_DEFECTS WHERE DOCID=" + hndocid.Value + @"
                            DELETE FROM PPM_PREVENTIVEACTION WHERE DOCID=" + hndocid.Value;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);

            insertinto_PPM_CONSTRUCTION_YARNDETAIL(Tran);
            insertinto_PPM_CONSTRUCTION(Tran);
            insertinto_PPM_TESTINGREQUIREMENTS(Tran);
            insertinto_PPM_JOBDETAIL(Tran);
            insertinto_PPM_TOLERENCE(Tran);
            insertinto_PPM_PACKING(Tran);
            insertinto_PPM_DEFECTS(Tran);
            insertinto_PPM_PREVENTIVEACTION(Tran);


            Tran.Commit();
            //********************SAVE IMAGE
            SaveImage(Convert.ToInt32(hndocid.Value));
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

    private void insertinto_PPM_PREVENTIVEACTION(SqlTransaction Tran)
    {
        string str = @"Insert into PPM_Preventiveaction(DOCID,COMMON,CRITICAL)
                       values(" + hndocid.Value + ",'" + txtcommonpreventive.Text.Replace("'", "''") + "','" + txtcriticalpreventive.Text.Replace("'", "''") + "')";
        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
    }

    private void insertinto_PPM_DEFECTS(SqlTransaction Tran)
    {

        string str = @"Insert into PPM_Defects(DOCID,COMMON,CRITICAL)
                       values(" + hndocid.Value + ",'" + txtcommondefects.Text.Replace("'", "''") + "','" + txtcriticaldefects.Text.Replace("'", "''") + "')";
        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
    }

    private void insertinto_PPM_PACKING(SqlTransaction Tran)
    {
        string str = @"Insert into PPM_Packing (DOCID,FOLDINGINSTRUCTION,WASHCARE_SEWLABEL,UCARDS,INSERTS,TAG,UPC,PRICETICKET,POLYBAG,PCSPERPLOYBAG,PCSPERCARTON_BALE,WHITEFABRIC,MARKINGS,CARTONDIMENSIONS,MAXCARTONWEIGHT,CARTON_BALESTRAPPING,WARNINGLABEL,CARTONPLY,Kaleenlabel)
                    values(" + hndocid.Value + ",'" + txtfoldinginstruction.Text.Replace("'", "''") + "','" + txtwashcare_sewinlabel.Text.Replace("'", "''") + "','" + txtucards.Text.Replace("'", "''") + "','" + txtinserts.Text.Replace("'", "''") + "','" + txttag.Text.Replace("'", "''") + "','" + txtUpc.Text.Replace("'", "''") + "','" + txtpriceticket.Text.Replace("'", "''") + @"',
                     '" + txtpolybag.Text.Replace("'", "''") + "','" + txtpcsperpolybag.Text.Replace("'", "''") + "','" + txtpcspercarton_bale.Text.Replace("'", "''") + "','" + txtwhitefabric.Text.Replace("'", "''") + "','" + txtmarkings.Text.Replace("'", "''") + "','" + txtcartondimensions.Text.Replace("'", "''") + "','" + txtmaxcartonweight.Text.Replace("'", "''") + "','" + txtcarton_balstrapping.Text.Replace("'", "''") + "','" + txtwarninglabel.Text.Replace("'", "''") + "','" + txtcartonply.Text.Replace("'", "''") + "','" + txtkaleenlabel.Text.Replace("'", "''") + "')";

        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);

    }

    private void insertinto_PPM_TOLERENCE(SqlTransaction Tran)
    {
        string strinsert = "";
        //****insert into Tolerence
        for (int i = 0; i < DGTolerence.Rows.Count; i++)
        {
            TextBox box1 = (TextBox)DGTolerence.Rows[i].Cells[1].FindControl("TextBox1");

            TextBox box2 = (TextBox)DGTolerence.Rows[i].Cells[2].FindControl("TextBox2");

            if (box1.Text != "")
            {
                strinsert = strinsert + " " + @"insert into PPM_Tolerence (DOCID,TYPE,VALUE)
                   values (" + hndocid.Value + ",'" + box1.Text.Replace("'", "''") + "','" + box2.Text.Replace("'", "''") + "')";
            }

        }
        if (strinsert != "")
        {
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, strinsert);
            strinsert = "";
        }
    }

    private void insertinto_PPM_JOBDETAIL(SqlTransaction Tran)
    {
        //insert into Jobdetail
        string strinsert = "";
        for (int i = 0; i < DGjobdetail.Rows.Count; i++)
        {
            Label lbljob = (Label)DGjobdetail.Rows[i].FindControl("lbljob");
            Label lblvalue = (Label)DGjobdetail.Rows[i].FindControl("lblvalue");
            Label lblcomments = (Label)DGjobdetail.Rows[i].FindControl("lblcomments");

            strinsert = strinsert + "  " + @" insert into PPM_JOBDETAIL (DOCID,JOBNAME,JOBVALUE,COMMENTS)values
                                       (" + hndocid.Value + ",'" + lbljob.Text + "','" + lblvalue.Text + "','" + lblcomments.Text + "')";
        }
        if (strinsert != "")
        {
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, strinsert);
            strinsert = "";
        }

    }

    private void insertinto_PPM_TESTINGREQUIREMENTS(SqlTransaction Tran)
    {
        string str = @"insert into PPM_testingrequirements (DOCID,RAWMATERIAL_TPI,RAWMATERIAL_COUNT,COLOURFORLIGHT,CROCKING_WET,CROCKING_DRY,COLOURMATCHING,BUYERREQUIREMENTS)
                     values(" + hndocid.Value + ",'" + txttpi.Text.Replace("'", "''") + "','" + txttestreq_count.Text.Replace("'", "''") + "','" + txtcolourforlight.Text.Replace("'", "''") + "','" + txtcrockingwet.Text.Replace("'", "''") + "','" + txtcrockingdry.Text.Replace("'", "''") + "','" + txtcolourmatching.Text.Replace("'", "''") + "','" + txtbuyerrequirements.Text.Replace("'", "''") + "')";
        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
    }

    private void insertinto_PPM_CONSTRUCTION(SqlTransaction Tran)
    {
        string str = @"Insert into PPM_construction (DOCID,BASECLOTH,BACKINGCLOTH,PILEHEIGHT_CUT,PILEHEIGHT_LOOP,QUALITY_RAWWT,QUALITY_FINISHWT,CONSTRUCTIONTECHNIQUE)
                    values(" + hndocid.Value + ",'" + txtbasecloth.Text.Replace("'", "''") + "','" + txtbackingcloth.Text.Replace("'", "''") + "','" + txtpileheightcut.Text.Replace("'", "''") + "','" + txtpileheightLoop.Text.Replace("'", "''") + "','" + txtqualityrawwt.Text.Replace("'", "''") + "','" + txtqualityFinishwt.Text.Replace("'", "''") + "','" + txtconstructiontechnique.Text.Replace("'", "''") + "')";
        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
    }

    private void insertinto_PPM_CONSTRUCTION_YARNDETAIL(SqlTransaction Tran)
    {
        if (DGConstructionyarndetail.Rows.Count != 0)
        {
            string strinsert = "";
            for (int i = 0; i < DGConstructionyarndetail.Rows.Count; i++)
            {
                TextBox box1 = (TextBox)DGConstructionyarndetail.Rows[i].Cells[1].FindControl("TextBox1");

                TextBox box2 = (TextBox)DGConstructionyarndetail.Rows[i].Cells[2].FindControl("TextBox2");

                TextBox box3 = (TextBox)DGConstructionyarndetail.Rows[i].Cells[3].FindControl("TextBox3");

                if (box1.Text != "")
                {
                    strinsert = strinsert + " " + @"insert into PPM_CONSTRUCTION_YARNDETAIL (TYPEOFYARN,YARNCOUNT,PLY,DOCID)
                   values ('" + box1.Text.Replace("'", "''") + "','" + box2.Text.Replace("'", "''") + "','" + box3.Text.Replace("'", "''") + "'," + hndocid.Value + ")";
                }
            }
            if (strinsert != "")
            {
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, strinsert);
            }
        }
    }
    protected void SaveImage(int Id)
    {
        if (PhotoImage.FileName != "")
        {
            string filename = Path.GetFileName(PhotoImage.PostedFile.FileName);
            string Folderpath = Server.MapPath("../../PPMIMAGE");
            //Check folder
            if (!Directory.Exists(Folderpath))
            {
                Directory.CreateDirectory(Folderpath);
            }
            //
            string targetPath = Server.MapPath("../../PPMIMAGE/" + Id + "_PPM.gif");

            FileInfo file = new FileInfo(targetPath);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
            }

            string img = "~\\PPMIMAGE\\" + Id + "_PPM.gif";
            //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
            Stream strm = PhotoImage.PostedFile.InputStream;
            var targetFile = targetPath;
            if (PhotoImage.FileName != null && PhotoImage.FileName != "")
            {
                GenerateThumbnails(0.3, strm, targetFile);
            }
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update PPMMASTER_NEW Set imgpath='" + img + "' Where Docid=" + Id + "");
            lblimage.ImageUrl = img + "?" + DateTime.Now.Ticks.ToString(); ;
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
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Docid", hndocid.Value);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_PREPRODUCTIONREPORT", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //Add Image
                ds.Tables[0].Columns.Add("image", typeof(System.Byte[]));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Convert.ToString(dr["imgpath"]) != "")
                    {
                        FileInfo file = new FileInfo(Server.MapPath(dr["imgpath"].ToString()));
                        if (file.Exists)
                        {
                            string img = dr["imgpath"].ToString();
                            img = Server.MapPath(img);
                            Byte[] img_byte = File.ReadAllBytes(img);
                            dr["image"] = img_byte;

                        }
                    }

                }

                Session["rptFileName"] = "~\\Reports\\rptpreproductionmeeting.rpt";
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptpreproductionmeeting.xsd";
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
    protected void refreshcontrol()
    {
        SetInitialRow_Constructionyarndetail();
        SetInitialRow_Tolerence();
        listpono.Items.Clear();
        listitemdesc.Items.Clear();
        txtppmmeetingdt.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        txtdocno.Text = "";
        hndocid.Value = "0";
        txtremarks.Text = "";
        //*******Construction tab
        txtbasecloth.Text = "";
        txtbackingcloth.Text = "";
        txtpileheightcut.Text = "";
        txtpileheightLoop.Text = "";
        txtqualityrawwt.Text = "";
        txtqualityFinishwt.Text = "";
        txtconstructiontechnique.Text = "";
        //**********Testing Requirement
        txttpi.Text = "";
        txttestreq_count.Text = "";
        txtcolourforlight.Text = "";
        txtcrockingwet.Text = "";
        txtcrockingdry.Text = "";
        txtcolourmatching.Text = "";
        txtbuyerrequirements.Text = "";
        //*********construction & Finishing
        DGjobdetail.DataSource = null;
        DGjobdetail.DataBind();
        //*********Packing
        txtfoldinginstruction.Text = "";
        txtwarninglabel.Text = "";
        txtwashcare_sewinlabel.Text = "";
        txtkaleenlabel.Text = "";
        txtucards.Text = "";
        txtinserts.Text = "";
        txttag.Text = "";
        txtUpc.Text = "";
        txtpriceticket.Text = "";
        txtpolybag.Text = "";
        txtpcsperpolybag.Text = "";
        txtpcspercarton_bale.Text = "";
        txtcartonply.Text = "";
        txtwhitefabric.Text = "";
        txtmarkings.Text = "";
        txtcartondimensions.Text = "";
        txtmaxcartonweight.Text = "";
        txtcarton_balstrapping.Text = "";

        //***************Defects
        txtcommondefects.Text = "";
        txtcriticaldefects.Text = "";
        //*************Preventive action
        txtcommonpreventive.Text = "";
        txtcriticalpreventive.Text = "";
    }

    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        Tddocno.Visible = false;
        refreshcontrol();
        if (chkedit.Checked == true)
        {
            DDCustomer.SelectedIndex = -1;
            Tddocno.Visible = true;
        }
    }
    protected void DDDocNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        refreshcontrol();
        hndocid.Value = DDDocNo.SelectedValue;
        try
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Docid", hndocid.Value);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_FILLPPMDETAILBACK", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //*****Master Detail
                txtppmmeetingdt.Text = ds.Tables[0].Rows[0]["meetingdt"].ToString();
                txtdocno.Text = ds.Tables[0].Rows[0]["docno"].ToString();
                txtremarks.Text = ds.Tables[0].Rows[0]["remark"].ToString();
                if (ds.Tables[0].Rows[0]["Imgpath"].ToString() != "")
                {
                    if (File.Exists(Server.MapPath(ds.Tables[0].Rows[0]["Imgpath"].ToString())))
                    {
                        lblimage.ImageUrl = ds.Tables[0].Rows[0]["Imgpath"].ToString() + "?" + DateTime.Now.Ticks.ToString();
                    }
                }
                else
                {
                    lblimage.ImageUrl = null + "?time=" + DateTime.Now.ToString(); ;
                }
                //****PO  No
                listpono.Items.Clear();
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    if (listpono.Items.FindByValue(ds.Tables[1].Rows[i]["orderid"].ToString()) == null)
                    {
                        listpono.Items.Add(new ListItem(ds.Tables[1].Rows[i]["customerorderno"].ToString(), ds.Tables[1].Rows[i]["orderid"].ToString()));
                    }
                }
                //**Item description
                listitemdesc.Items.Clear();

                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    if (listitemdesc.Items.FindByValue(ds.Tables[2].Rows[i]["item_finished_id"].ToString()) == null)
                    {
                        listitemdesc.Items.Add(new ListItem(ds.Tables[2].Rows[i]["itemdesc"].ToString(), ds.Tables[2].Rows[i]["item_finished_id"].ToString()));
                    }
                }
                //*********construction
                //ViewState["constructionyarn"] = null;
                //ViewState["constructionyarn"] = ds.Tables[3];

                //DGConstructionyarndetail.DataSource = ds.Tables[3];
                //DGConstructionyarndetail.DataBind();

                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        TextBox box1 = (TextBox)DGConstructionyarndetail.Rows[i].Cells[1].FindControl("TextBox1");
                        TextBox box2 = (TextBox)DGConstructionyarndetail.Rows[i].Cells[2].FindControl("TextBox2");
                        TextBox box3 = (TextBox)DGConstructionyarndetail.Rows[i].Cells[3].FindControl("TextBox3");

                        box1.Text = ds.Tables[3].Rows[i]["TYPEOFYARN"].ToString();
                        box2.Text = ds.Tables[3].Rows[i]["yarncount"].ToString();
                        box3.Text = ds.Tables[3].Rows[i]["Ply"].ToString();
                    }
                    else
                    {
                        AddNewRowToGrid_Construction();
                        TextBox box1 = (TextBox)DGConstructionyarndetail.Rows[i].Cells[1].FindControl("TextBox1");
                        TextBox box2 = (TextBox)DGConstructionyarndetail.Rows[i].Cells[2].FindControl("TextBox2");
                        TextBox box3 = (TextBox)DGConstructionyarndetail.Rows[i].Cells[3].FindControl("TextBox3");

                        box1.Text = ds.Tables[3].Rows[i]["TYPEOFYARN"].ToString();
                        box2.Text = ds.Tables[3].Rows[i]["yarncount"].ToString();
                        box3.Text = ds.Tables[3].Rows[i]["Ply"].ToString();
                    }
                }
                if (ds.Tables[4].Rows.Count > 0)
                {

                    txtbasecloth.Text = ds.Tables[4].Rows[0]["basecloth"].ToString();
                    txtbackingcloth.Text = ds.Tables[4].Rows[0]["backingcloth"].ToString();
                    txtpileheightcut.Text = ds.Tables[4].Rows[0]["pileheight_cut"].ToString();
                    txtpileheightLoop.Text = ds.Tables[4].Rows[0]["pileheight_loop"].ToString();
                    txtqualityrawwt.Text = ds.Tables[4].Rows[0]["quality_rawwt"].ToString();
                    txtqualityFinishwt.Text = ds.Tables[4].Rows[0]["quality_finishwt"].ToString();
                    txtconstructiontechnique.Text = ds.Tables[4].Rows[0]["constructiontechnique"].ToString();

                }
                //********Testing Requirements
                if (ds.Tables[5].Rows.Count > 0)
                {

                    txttpi.Text = ds.Tables[5].Rows[0]["Rawmaterial_tpi"].ToString();
                    txttestreq_count.Text = ds.Tables[5].Rows[0]["Rawmaterial_count"].ToString();
                    txtcolourforlight.Text = ds.Tables[5].Rows[0]["colourforlight"].ToString();
                    txtcrockingwet.Text = ds.Tables[5].Rows[0]["crocking_wet"].ToString();
                    txtcrockingdry.Text = ds.Tables[5].Rows[0]["crocking_dry"].ToString();
                    txtcolourmatching.Text = ds.Tables[5].Rows[0]["colourmatching"].ToString();
                    txtbuyerrequirements.Text = ds.Tables[5].Rows[0]["Buyerrequirements"].ToString();

                }
                //********Construction & FInishing
                ViewState["jobdetail"] = null;
                ViewState["jobdetail"] = ds.Tables[6];
                DGjobdetail.DataSource = ds.Tables[6];
                DGjobdetail.DataBind();
                //**Tolerence
                //ViewState["tolerence"] = null;
                //ViewState["tolerence"] = ds.Tables[7];
                //DGTolerence.DataSource = ds.Tables[7];
                //DGTolerence.DataBind();

                for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        TextBox box1 = (TextBox)DGTolerence.Rows[i].Cells[1].FindControl("TextBox1");
                        TextBox box2 = (TextBox)DGTolerence.Rows[i].Cells[2].FindControl("TextBox2");


                        box1.Text = ds.Tables[7].Rows[i]["Type"].ToString();
                        box2.Text = ds.Tables[7].Rows[i]["value"].ToString();

                    }
                    else
                    {
                        AddNewRowToGrid_Tolerence();
                        TextBox box1 = (TextBox)DGTolerence.Rows[i].Cells[1].FindControl("TextBox1");
                        TextBox box2 = (TextBox)DGTolerence.Rows[i].Cells[2].FindControl("TextBox2");


                        box1.Text = ds.Tables[7].Rows[i]["Type"].ToString();
                        box2.Text = ds.Tables[7].Rows[i]["value"].ToString();

                    }
                }

                //*****************Packing
                if (ds.Tables[8].Rows.Count > 0)
                {

                    txtfoldinginstruction.Text = ds.Tables[8].Rows[0]["foldinginstruction"].ToString();
                    txtwashcare_sewinlabel.Text = ds.Tables[8].Rows[0]["washcare_sewlabel"].ToString();
                    txtucards.Text = ds.Tables[8].Rows[0]["ucards"].ToString();
                    txtinserts.Text = ds.Tables[8].Rows[0]["inserts"].ToString();
                    txttag.Text = ds.Tables[8].Rows[0]["Tag"].ToString();
                    txtUpc.Text = ds.Tables[8].Rows[0]["Upc"].ToString();
                    txtpriceticket.Text = ds.Tables[8].Rows[0]["priceticket"].ToString();
                    txtpolybag.Text = ds.Tables[8].Rows[0]["polybag"].ToString();
                    txtpcsperpolybag.Text = ds.Tables[8].Rows[0]["pcsperploybag"].ToString();
                    txtpcspercarton_bale.Text = ds.Tables[8].Rows[0]["pcspercarton_bale"].ToString();
                    txtwhitefabric.Text = ds.Tables[8].Rows[0]["whitefabric"].ToString();
                    txtmarkings.Text = ds.Tables[8].Rows[0]["markings"].ToString();
                    txtcartondimensions.Text = ds.Tables[8].Rows[0]["cartondimensions"].ToString();
                    txtmaxcartonweight.Text = ds.Tables[8].Rows[0]["maxcartonweight"].ToString();
                    txtcarton_balstrapping.Text = ds.Tables[8].Rows[0]["carton_balestrapping"].ToString();
                    txtwarninglabel.Text = ds.Tables[8].Rows[0]["warninglabel"].ToString();
                    txtcartonply.Text = ds.Tables[8].Rows[0]["cartonply"].ToString();
                    txtkaleenlabel.Text = ds.Tables[8].Rows[0]["kaleenlabel"].ToString();
                }
                //*********Defects
                if (ds.Tables[9].Rows.Count > 0)
                {

                    txtcommondefects.Text = ds.Tables[9].Rows[0]["common"].ToString();
                    txtcriticaldefects.Text = ds.Tables[9].Rows[0]["critical"].ToString();

                }
                //*******Preventive actions
                if (ds.Tables[10].Rows.Count > 0)
                {

                    txtcommonpreventive.Text = ds.Tables[10].Rows[0]["common"].ToString();
                    txtcriticalpreventive.Text = ds.Tables[10].Rows[0]["critical"].ToString();
                }
            }
            else
            {
                lblmsg.Text = "Please select Proper Doc No.";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}