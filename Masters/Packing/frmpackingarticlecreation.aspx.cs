using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.IO;
using ClosedXML.Excel;

public partial class Masters_Packing_frmpackingarticlecreation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select ICm.CATEGORY_ID,ICM.CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=Cs.Categoryid and cs.id=0 order by CATEGORY_NAME
                           select ShapeId,ShapeName From shape Where mastercompanyid=" + Session["varMasterCompanyIDForERP"] + @"
                           select ID,PackingType From Packingtype order by PackingType";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcategory, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDshape, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDpacktype, ds, 2, true, "--Plz Select--");
            if (DDcategory.Items.Count > 0)
            {
                DDcategory.SelectedIndex = 1;
                DDcategory_SelectedIndexChanged(DDcategory, new EventArgs());
            }
            SetInitialRow();

            switch (Convert.ToInt16(Session["varMasterCompanyIDForERP"]))
            {
                case 22: //for DiamondExport
                    TableGridData.Visible = true;
                    psize.Visible = true;
                    tpsize.Visible = true;
                    ShowGridData();
                    break;                
                default:
                    TableGridData.Visible = false;
                    break;
            }

        }
    }
    protected void DDitemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillquality();
    }
    protected void Fillquality()
    {
        UtilityModule.ConditionalComboFill(ref DDquality, "select QualityId,QualityName From Quality Where Item_Id=" + DDitemname.SelectedValue + "  order by QualityName", true, "--Plz Select--");
    }
    protected void FillDesign()
    {
        string str = @"select Distinct Vf.designid,vf.designname From V_FinishedItemDetail vf Where vf.ITEM_ID=" + DDitemname.SelectedValue + " and vf.QualityId=" + DDquality.SelectedValue + @"  and vf.designId<>0
                   order by vf.designName";
        UtilityModule.ConditionalComboFill(ref DDdesign, str, true, "--Plz Select--");
    }
    protected void DDquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDesign();
    }
    protected void FillColor()
    {
        string str = @"select Distinct Vf.colorid,vf.colorname From V_FinishedItemDetail vf Where vf.ITEM_ID=" + DDitemname.SelectedValue + " and vf.QualityId=" + DDquality.SelectedValue + "  and vf.designId=" + DDdesign.SelectedValue + @"
                   order by vf.colorname";
        UtilityModule.ConditionalComboFill(ref DDcolor, str, true, "--Plz Select--");
    }
    protected void DDdesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillColor();
    }
    protected void DDshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void FillSize()
    {
        string str = @"select Distinct Vf.SizeId,vf.SizeMtr From V_FinishedItemDetail vf Where vf.ITEM_ID=" + DDitemname.SelectedValue + " and vf.QualityId=" + DDquality.SelectedValue + "  and vf.designId=" + DDdesign.SelectedValue + @"
                       and vf.colorid=" + DDcolor.SelectedValue + " and vf.shapeid=" + DDshape.SelectedValue + @" and vf.sizeid<>0
                       order by vf.SizeMtr";
        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--Plz Select--");
    }
    private void SetInitialRow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("Effdate", typeof(string)));
        dt.Columns.Add(new DataColumn("Rate", typeof(string)));
        dt.Columns.Add(new DataColumn("Grwt", typeof(string)));
        dt.Columns.Add(new DataColumn("Netwt", typeof(string)));
        dt.Columns.Add(new DataColumn("Vol", typeof(string)));
        dt.Columns.Add(new DataColumn("Pcs", typeof(string)));
        dr = dt.NewRow();
        dr["Effdate"] = string.Empty;
        dr["Rate"] = string.Empty;
        dr["Grwt"] = string.Empty;
        dr["Netwt"] = string.Empty;
        dr["Vol"] = string.Empty;
        dr["Pcs"] = string.Empty;
        dt.Rows.Add(dr);
        //dr = dt.NewRow();

        //Store the DataTable in ViewState
        ViewState["CurrentTable"] = dt;

        DGrate.DataSource = dt;
        DGrate.DataBind();
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
                    TextBox box1 = (TextBox)DGrate.Rows[rowIndex].Cells[1].FindControl("txteffdate");
                    TextBox box2 = (TextBox)DGrate.Rows[rowIndex].Cells[2].FindControl("txtratepcs");
                    TextBox box3 = (TextBox)DGrate.Rows[rowIndex].Cells[3].FindControl("txtgrosswt");
                    TextBox box4 = (TextBox)DGrate.Rows[rowIndex].Cells[4].FindControl("txtnetwt");
                    TextBox box5 = (TextBox)DGrate.Rows[rowIndex].Cells[5].FindControl("txtvol");
                    TextBox box6 = (TextBox)DGrate.Rows[rowIndex].Cells[6].FindControl("txtpcs");

                    drCurrentRow = dtCurrentTable.NewRow();

                    dtCurrentTable.Rows[i - 1]["Effdate"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["Rate"] = box2.Text;
                    dtCurrentTable.Rows[i - 1]["Grwt"] = box3.Text;
                    dtCurrentTable.Rows[i - 1]["Netwt"] = box4.Text;
                    dtCurrentTable.Rows[i - 1]["Vol"] = box5.Text;
                    dtCurrentTable.Rows[i - 1]["Pcs"] = box6.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;

                DGrate.DataSource = dtCurrentTable;
                DGrate.DataBind();
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
                    TextBox box1 = (TextBox)DGrate.Rows[rowIndex].Cells[1].FindControl("txteffdate");
                    TextBox box2 = (TextBox)DGrate.Rows[rowIndex].Cells[2].FindControl("txtratepcs");
                    TextBox box3 = (TextBox)DGrate.Rows[rowIndex].Cells[3].FindControl("txtgrosswt");
                    TextBox box4 = (TextBox)DGrate.Rows[rowIndex].Cells[4].FindControl("txtnetwt");
                    TextBox box5 = (TextBox)DGrate.Rows[rowIndex].Cells[5].FindControl("txtvol");
                    TextBox box6 = (TextBox)DGrate.Rows[rowIndex].Cells[6].FindControl("txtpcs");

                    box1.Text = dt.Rows[i]["Effdate"].ToString();
                    box2.Text = dt.Rows[i]["Rate"].ToString();
                    box3.Text = dt.Rows[i]["Grwt"].ToString();
                    box4.Text = dt.Rows[i]["Netwt"].ToString();
                    box5.Text = dt.Rows[i]["Vol"].ToString();
                    box6.Text = dt.Rows[i]["Pcs"].ToString();




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
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //Table Type
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            DataTable dtrate = new DataTable();
            dtrate.Columns.Add("Effdate", typeof(DateTime));
            dtrate.Columns.Add("Rate", typeof(float));
            dtrate.Columns.Add("Grwt", typeof(float));
            dtrate.Columns.Add("Netwt", typeof(float));
            dtrate.Columns.Add("Vol", typeof(float));
            dtrate.Columns.Add("Pcs", typeof(int));
            for (int i = 0; i < DGrate.Rows.Count; i++)
            {
                TextBox box1 = (TextBox)DGrate.Rows[i].Cells[1].FindControl("txteffdate");
                TextBox box2 = (TextBox)DGrate.Rows[i].Cells[2].FindControl("txtratepcs");
                TextBox box3 = (TextBox)DGrate.Rows[i].Cells[3].FindControl("txtgrosswt");
                TextBox box4 = (TextBox)DGrate.Rows[i].Cells[1].FindControl("txtnetwt");
                TextBox box5 = (TextBox)DGrate.Rows[i].Cells[2].FindControl("txtvol");
                TextBox box6 = (TextBox)DGrate.Rows[i].Cells[3].FindControl("txtpcs");

                if (box1.Text != "")
                {
                    DataRow dr = dtrate.NewRow();
                    dr["Effdate"] = box1.Text;
                    dr["rate"] = string.IsNullOrWhiteSpace(box2.Text) ? DBNull.Value : (object)box2.Text;
                    dr["Grwt"] = string.IsNullOrWhiteSpace(box3.Text) ? DBNull.Value : (object)box3.Text;
                    dr["Netwt"] = string.IsNullOrWhiteSpace(box4.Text) ? DBNull.Value : (object)box4.Text;
                    dr["Vol"] = string.IsNullOrWhiteSpace(box5.Text) ? DBNull.Value : (object)box5.Text;
                    dr["Pcs"] = string.IsNullOrWhiteSpace(box6.Text) ? DBNull.Value : (object)box6.Text;
                    dtrate.Rows.Add(dr);
                }
            }
            //
            SqlParameter[] param = new SqlParameter[19];
            param[0] = new SqlParameter("@ArticleNo", txtarticleno.Text == "" ? "" : txtarticleno.Text);
            param[1] = new SqlParameter("@itemid", DDitemname.SelectedValue);
            param[2] = new SqlParameter("@QualityId", DDquality.SelectedValue);
            param[3] = new SqlParameter("@DesignId", DDdesign.SelectedValue);
            param[4] = new SqlParameter("@Colorid", DDcolor.SelectedValue);
            param[5] = new SqlParameter("@shapeid", DDshape.SelectedValue);
            param[6] = new SqlParameter("@Sizeid", DDSize.SelectedValue);
            param[7] = new SqlParameter("@descofgoods", txtdescofgoods.Text);
            param[8] = new SqlParameter("@Content", txtcontent.Text);
            param[9] = new SqlParameter("@weight_roll", txtwtperroll.Text);
            param[10] = new SqlParameter("@Netwt", txtnetweight.Text);
            param[11] = new SqlParameter("@volume_roll", txtvolroll.Text);
            param[12] = new SqlParameter("@Pcs_roll", txtpcsroll.Text);
            param[13] = new SqlParameter("@userid", Session["varuserid"]);
            param[14] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            //Table Type
            param[15] = new SqlParameter("@dtrate", dtrate);
            //*********
            param[16] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[16].Direction = ParameterDirection.Output;
            param[17] = new SqlParameter("@Packingtypeid", DDpacktype.SelectedValue);
            param[18] = new SqlParameter("@palletsize", tpsize.Visible==true?txtpalletsize.Text:"");
            //*********************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_savePackingarticlecreation", param);
            lblmsg.Text = param[16].Value.ToString();
            Tran.Commit();
            //refreshcontrol();

            if (Session["VarCompanyNo"].ToString() == "22")
            {
                ShowGridData();
            }
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
    }
    protected void FillEditDetail()
    {

    }
    protected void refreshcontrol()
    {
        DDpacktype.SelectedIndex = -1;
        DDitemname.SelectedIndex = -1;
        DDquality.SelectedIndex = -1;
        DDdesign.SelectedIndex = -1;
        DDcolor.SelectedIndex = -1;
        DDshape.SelectedIndex = -1;
        DDSize.SelectedIndex = -1;
        txtdescofgoods.Text = "";
        txtcontent.Text = "";
        txtwtperroll.Text = "";
        txtnetweight.Text = "";
        txtvolroll.Text = "";
        txtpcsroll.Text = "";
        SetInitialRow();
    }
    protected void txtarticleno_TextChanged(object sender, EventArgs e)
    {

    }
    protected void Fillgrid(DataSet ds)
    {
        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
        {
            if (i == 0)
            {
                TextBox box1 = (TextBox)DGrate.Rows[i].Cells[1].FindControl("txteffdate");
                TextBox box2 = (TextBox)DGrate.Rows[i].Cells[2].FindControl("txtratepcs");
                TextBox box3 = (TextBox)DGrate.Rows[i].Cells[3].FindControl("txtgrosswt");
                TextBox box4 = (TextBox)DGrate.Rows[i].Cells[4].FindControl("txtnetwt");
                TextBox box5 = (TextBox)DGrate.Rows[i].Cells[5].FindControl("txtvol");
                TextBox box6 = (TextBox)DGrate.Rows[i].Cells[6].FindControl("txtpcs");


                box1.Text = ds.Tables[1].Rows[i]["effdate"].ToString();
                box2.Text = ds.Tables[1].Rows[i]["rate"].ToString();
                box3.Text = ds.Tables[1].Rows[i]["grwt"].ToString();
                box4.Text = ds.Tables[1].Rows[i]["netwt"].ToString();
                box5.Text = ds.Tables[1].Rows[i]["vol"].ToString();
                box6.Text = ds.Tables[1].Rows[i]["pcs"].ToString();
            }
            else
            {
                AddNewRowToGrid();

                TextBox box1 = (TextBox)DGrate.Rows[i].Cells[1].FindControl("txteffdate");
                TextBox box2 = (TextBox)DGrate.Rows[i].Cells[2].FindControl("txtratepcs");
                TextBox box3 = (TextBox)DGrate.Rows[i].Cells[3].FindControl("txtgrosswt");
                TextBox box4 = (TextBox)DGrate.Rows[i].Cells[4].FindControl("txtnetwt");
                TextBox box5 = (TextBox)DGrate.Rows[i].Cells[5].FindControl("txtvol");
                TextBox box6 = (TextBox)DGrate.Rows[i].Cells[6].FindControl("txtpcs");

                box1.Text = ds.Tables[1].Rows[i]["effdate"].ToString();
                box2.Text = ds.Tables[1].Rows[i]["rate"].ToString();
                box3.Text = ds.Tables[1].Rows[i]["grwt"].ToString();
                box4.Text = ds.Tables[1].Rows[i]["netwt"].ToString();
                box5.Text = ds.Tables[1].Rows[i]["vol"].ToString();
                box6.Text = ds.Tables[1].Rows[i]["pcs"].ToString();
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {

    }
    protected void FillData()
    {
        string str = @"select PA.ArticleNo,PA.Itemid,PA.QualityId,PA.Designid,PA.Colorid,PA.shapeid,PA.sizeid,PA.descofgoods,PA.contents,PA.weight_roll,PA.Netwt,volume_roll,PA.pcs_roll,PA.packingtypeid,IM.category_id,PA.palletsize from Packingarticle PA(NoLock) inner join ITEM_MASTER IM(NoLock) on PA.itemid=IM.Item_id Where  PA.articleno='" + txtarticleno.Text + @"'
                     select Replace(convert(nvarchar(11),EffDate,106),' ','-') as EffDate,Rate,Grwt,Netwt,Vol,Pcs from PackingarticleRate(NoLock) Where Articleno='" + txtarticleno.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DDpacktype.SelectedValue = ds.Tables[0].Rows[0]["packingtypeid"].ToString();
            DDcategory.SelectedValue = ds.Tables[0].Rows[0]["category_id"].ToString();
            DDcategory_SelectedIndexChanged(DDcategory, new EventArgs());
            DDitemname.SelectedValue = ds.Tables[0].Rows[0]["Itemid"].ToString();
            Fillquality();
            DDquality.SelectedValue = ds.Tables[0].Rows[0]["qualityid"].ToString();
            FillDesign();
            DDdesign.SelectedValue = ds.Tables[0].Rows[0]["Designid"].ToString();
            FillColor();
            DDcolor.SelectedValue = ds.Tables[0].Rows[0]["colorid"].ToString();
            DDshape.SelectedValue = ds.Tables[0].Rows[0]["shapeid"].ToString();
            FillSize();
            DDSize.SelectedValue = ds.Tables[0].Rows[0]["Sizeid"].ToString();
            txtdescofgoods.Text = ds.Tables[0].Rows[0]["descofgoods"].ToString();
            txtcontent.Text = ds.Tables[0].Rows[0]["contents"].ToString();
            txtwtperroll.Text = ds.Tables[0].Rows[0]["Weight_roll"].ToString();
            txtnetweight.Text = ds.Tables[0].Rows[0]["Netwt"].ToString();
            txtvolroll.Text = ds.Tables[0].Rows[0]["Volume_roll"].ToString();
            txtpcsroll.Text = ds.Tables[0].Rows[0]["Pcs_roll"].ToString();
            txtpalletsize.Text = ds.Tables[0].Rows[0]["palletsize"].ToString();
            if (ds.Tables[1].Rows.Count > 0)
            {
                Fillgrid(ds);
            }
            else
            {
                SetInitialRow();
            }
        }
        else
        {
            refreshcontrol();
        }
    }
    protected void txtarticleno_TextChanged1(object sender, EventArgs e)
    {
        FillData();
    }
    protected void DDcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDitemname, @"select IM.ITEM_ID,IM.ITEM_NAME From Item_Master IM inner join CategorySeparate CS on IM.CATEGORY_ID=CS.Categoryid
                                                            and CS.id=0 and cs.categoryid=" + DDcategory.SelectedValue + " and IM.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by IM.ITEM_NAME", true, "--Plz Select--");
    }
    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.DGDetail, "Select$" + e.Row.RowIndex);           
            
        }
    }
    protected void DGDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGDetail.PageIndex = e.NewPageIndex;
        ShowGridData();
    }
    protected void DGDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        int rowindex = DGDetail.SelectedRow.RowIndex;
        Label lblArticleNo = ((Label)DGDetail.Rows[rowindex].FindControl("lblArticleNo"));
        txtarticleno.Text = lblArticleNo.Text.Trim();
        txtarticleno_TextChanged1(sender, new EventArgs());
       
    }
    protected void ShowGridData()
    {
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetArticleCreationData", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyNo"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);           

            DataSet Ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(Ds);

            if (Ds.Tables[0].Rows.Count > 0)
            {
                DGDetail.DataSource = Ds.Tables[0];
                DGDetail.DataBind();
            }
           

        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.Message;
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetArticleCreationReportData", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyNo"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);

            DataSet Ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(Ds);

            if (Ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                sht.Range("A1").Value = "ARTICLE CREATION REPORT DETAIL";
                sht.Range("A1:N1").Style.Font.FontName = "Calibri";
                sht.Range("A1:N1").Style.Font.Bold = true;
                sht.Range("A1:N1").Style.Font.FontSize = 12;
                sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:N1").Merge();

                //*******Header
                sht.Range("A2").Value = "Article No";
                sht.Range("B2").Value = "Item Name";
                sht.Range("C2").Value = "Quality";
                sht.Range("D2").Value = "Design";
                sht.Range("E2").Value = "Color";
                sht.Range("F2").Value = "Shape";
                sht.Range("G2").Value = "Size";
                sht.Range("H2").Value = "Description Of Goods";
                sht.Range("I2").Value = "Contents";
                sht.Range("J2").Value = "Weight Roll";
                sht.Range("K2").Value = "Net Wt";
                sht.Range("L2").Value = "Volume Roll";
                sht.Range("M2").Value = "Pcs Roll";
                sht.Range("N2").Value = "Packing Type";                


                sht.Range("A2:N2").Style.Font.FontName = "Calibri";
                sht.Range("A2:N2").Style.Font.FontSize = 11;
                sht.Range("A2:N2").Style.Font.Bold = true;
                //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 3;

                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":N" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":N" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(Ds.Tables[0].Rows[i]["ArticleNo"]);
                    sht.Range("B" + row).SetValue(Ds.Tables[0].Rows[i]["Item_Name"]);
                    sht.Range("C" + row).SetValue(Ds.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("D" + row).SetValue(Ds.Tables[0].Rows[i]["DesignName"]);
                    sht.Range("E" + row).SetValue(Ds.Tables[0].Rows[i]["ColorName"]);
                    sht.Range("F" + row).SetValue(Ds.Tables[0].Rows[i]["ShapeName"]);
                    sht.Range("G" + row).SetValue(Ds.Tables[0].Rows[i]["SizeMtr"]);
                    sht.Range("H" + row).SetValue(Ds.Tables[0].Rows[i]["DescofGoods"]);
                    sht.Range("I" + row).SetValue(Ds.Tables[0].Rows[i]["Contents"]);
                    sht.Range("J" + row).SetValue(Ds.Tables[0].Rows[i]["Weight_Roll"]);
                    sht.Range("K" + row).SetValue(Ds.Tables[0].Rows[i]["NetWt"]);
                    sht.Range("L" + row).SetValue(Ds.Tables[0].Rows[i]["Volume_Roll"]);
                    sht.Range("M" + row).SetValue(Ds.Tables[0].Rows[i]["Pcs_Roll"]);
                    sht.Range("N" + row).SetValue(Ds.Tables[0].Rows[i]["PackingType"]);                 

                    row = row + 1;
                }

                //*************
                sht.Columns(1, 30).AdjustToContents();

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "N")))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("ArticleCreationReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.Message;
        }
    }
    
}