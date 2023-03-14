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

public partial class Masters_Inspection_frminspection : System.Web.UI.Page
{
    private int numOfColumns = 1;
    private int ctr = 0;
    private int ctrtext = 0;
    Table table = null;
    protected void Page_Init(object sender, EventArgs e)
    {
       
            if ((Session["TOTALCHECKPOINT"] != null) )
            {
                // DataSet ds = (DataSet)Session["ds"];
                InitTable(Convert.ToInt32(Session["totaltest"]), Convert.ToInt32(Session["TOTALCHECKPOINT"]));
            }
       // }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CI.CompanyId,CI.CompanyName from CompanyInfo CI inner join Company_Authentication CA on Ci.CompanyId=CA.CompanyId
                  WHere CI.MasterCompanyid=" + Session["varcompanyid"] + " and CA.UserId=" + Session["varuserid"] + @"  order by CompanyName";
            UtilityModule.ConditionalComboFill(ref DDcompanyName, str, true, "Plz Select--");
            UtilityModule.ConditionalComboFill(ref ddCategoryName, "SELECT CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER Where MasterCompanyid=" + Session["varCompanyId"] + " order by CATEGORY_NAME", true, "---Select---");

            if (DDcompanyName.Items.Count > 0)
            {
                DDcompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompanyName.Enabled = false;
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
    private void FillGridParameter()
    {
        string Str = "Select QC.ParaID Sr_No,SrNo,ParaName ParameterName,SName ShortName,Specification Specification,Method Method,QC.ParaID,QM.ItemID From QCParameter QC INNER JOIN QCMaster QM ON QC.PARAID = QM.PARAID Where CategoryID =" + ddCategoryName.SelectedValue + " AND QM.ITEMID = " + ddCategoryName.SelectedValue + " AND QM.Qualityid = "+dquality.SelectedValue;
        //if (ddProcessName.SelectedIndex > 0)
        //{
        //    Str = Str + " And ProcessId=" + ddProcessName.SelectedValue;
        //}
        //Str = Str + " Order By SrNo";
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        Fillgridinitialrow(Ds);
        //DGShowData.DataSource = Ds;
        //DGShowData.DataBind();
    }
    protected void Fillgridinitialrow(DataSet ds)
    {
        pnlchecklist.Controls.Clear();
        Session["totaltest"] = null;
        Session["TOTALCHECKPOINT"] = null;
        if (ds!=null)
        {
            if(ds.Tables.Count>0)
            {

                if (ds.Tables[0].Rows.Count > 0)
                {

                    Session["totaltest"] = 5;
                    Session["TOTALCHECKPOINT"] = ds.Tables[0].Rows.Count;
                    Session["ds"] = ds;
                    GenerateTable(ds,Convert.ToInt32(Session["totaltest"]));
                }

            }
            else
            {
                lblmsg.Text = "Please define QC Parameters First.";

            }

        }
       else
        { lblmsg.Text = "Please define QC Parameters First."; }
       // table =(Table)Session["table"];
      
        //*****************
    }
    private void GenerateTable(DataSet ds,int nooftests)
    {
        table = new Table();
        table.Style.Add("border", "1px");
        table.Style.Add("width","100%");
        string idctr = string.Empty;
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            ctr++;
            TableRow row = new TableRow();
            row.ID = "tr_" + ctr;

            TableCell cellstart = new TableCell();
            cellstart.Style.Add("width", "10%");
            cellstart.Style.Add("padding", "1 % 1 % 1 % 1 %");
            cellstart.Style.Add("text-align", "center");
            Label lblsr = new Label();
            lblsr.ID= "lblsr_" + ctr;
            lblsr.CssClass = "labelbold";
            lblsr.Text = dr["SrNo"].ToString();
            idctr = dr["SrNo"].ToString();
            cellstart.Controls.Add(lblsr);
            row.Cells.Add(cellstart);
            TableCell cellchkpoint = new TableCell();
            cellchkpoint.Style.Add("width", "15%");
            cellchkpoint.Style.Add("padding", "1 % 1 % 1 % 1 %");
            cellchkpoint.Style.Add("text-align", "center");
            Label lblchkpoint = new Label();
            lblchkpoint.ID = "lblchkpoint_" + idctr;
            lblchkpoint.CssClass = "labelbold";
            lblchkpoint.Text = dr["ParameterName"].ToString();
          //  lblchkpoint.Style.Add("width", "21%");
            cellchkpoint.Controls.Add(lblchkpoint);
            row.Cells.Add(cellchkpoint);
            TableCell cellspeci = new TableCell();
            TextBox tbspeci = new TextBox();
            
            tbspeci.ID = "txtspecification_" + idctr;

            tbspeci.CssClass = "textb";
            tbspeci.Style.Add("width", "85%");
            cellspeci.Controls.Add(tbspeci);
                   
            
            row.Cells.Add(cellspeci);
            for (int i = 1; i <= nooftests; i++)
            {
                ctrtext++;
                TableCell cellNext = new TableCell();
                TextBox tb = new TextBox();
                tb.ID = "Txtvalue_"+ idctr+"_" + i;
                tb.CssClass = "textb";
                tb.Attributes.Add("onchange", "return keypress(" + idctr + ",1)");
                tb.Width = 80;
                // Add the control to the TableCell
                cellNext.Controls.Add(tb);
                // Add the TableCell to the TableRow
                row.Cells.Add(cellNext);
            }
            TableCell cellavg = new TableCell();
            TextBox tbavg = new TextBox();
            tbavg.ID = "txtavg_" + ctr;
            tbavg.CssClass = "textb";
            tbavg.Style.Add("width", "85%");
            cellavg.Controls.Add(tbavg);
            row.Cells.Add(cellavg);
            table.Rows.Add(row);
        }
        pnlchecklist.Controls.Add(table);
    }
    private void FillTable(DataSet ds, int nooftests = 5)
    {
       pnlchecklist.Controls.Clear();
        table = new Table();
        table.Style.Add("border", "1px");
        table.Style.Add("width", "100%");
        string idctr = string.Empty;
        DataView view = new DataView(ds.Tables[1]);
        DataTable distinctValues = view.ToTable(true, "checkpointid");


        // string Str = "Select QC.ParaID Sr_No,SrNo,ParaName ParameterName From QCParameter QC INNER JOIN QCMaster QM ON QC.PARAID = QM.PARAID Where CategoryID =" + //ddCategoryName.SelectedValue + " AND QM.ITEMID = " + ddCategoryName.SelectedValue + " AND QM.Qualityid = " + dquality.SelectedValue;
        //if (ddProcessName.SelectedIndex > 0)
        //{
        //    Str = Str + " And ProcessId=" + ddProcessName.SelectedValue;
        //}
        //Str = Str + " Order By SrNo";
        //   DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Session["TOTALCHECKPOINT"] == null)
        {
            Session["TOTALCHECKPOINT"] = distinctValues.Rows.Count;



        }

        foreach (DataRow dr in distinctValues.Rows)
        {

            DataRow[] filteredRows = ds.Tables[1].Select("checkpointid ="+ dr["checkpointid"]);
            if(Session["totaltest"] ==null)
            {

                Session["totaltest"] = filteredRows.Count();

            }
            
            ctr++;
            TableRow row = new TableRow();
            row.ID = "tr_" + ctr;

            TableCell cellstart = new TableCell();
            cellstart.Style.Add("width", "10%");
            cellstart.Style.Add("padding", "1 % 1 % 1 % 1 %");
            cellstart.Style.Add("text-align", "center");
            Label lblsr = new Label();
            lblsr.ID = "lblsr_" + ctr;
            lblsr.CssClass = "labelbold";
            lblsr.Text = dr["CheckPointId"].ToString();
            idctr = dr["CheckPointId"].ToString();
            cellstart.Controls.Add(lblsr);
            row.Cells.Add(cellstart);
            TableCell cellchkpoint = new TableCell();
            cellchkpoint.Style.Add("width", "15%");
            cellchkpoint.Style.Add("padding", "1 % 1 % 1 % 1 %");
            cellchkpoint.Style.Add("text-align", "center");
            Label lblchkpoint = new Label();
            lblchkpoint.ID = "lblchkpoint_" + idctr;
            lblchkpoint.CssClass = "labelbold";
            lblchkpoint.Text = filteredRows[0]["checkpointname"].ToString();
              lblchkpoint.Style.Add("width", "21%");
            cellchkpoint.Controls.Add(lblchkpoint);
            row.Cells.Add(cellchkpoint);
            TableCell cellspeci = new TableCell();
            TextBox tbspeci = new TextBox();

            tbspeci.ID = "txtspecification_" + idctr;
            tbspeci.Text = filteredRows[0]["specificationvalue"].ToString();
            tbspeci.CssClass = "textb";
            tbspeci.Style.Add("width", "85%");
            cellspeci.Controls.Add(tbspeci);


            row.Cells.Add(cellspeci);
            for (int i = 0; i < nooftests; i++)
            {
                ctrtext++;
                TableCell cellNext = new TableCell();
                TextBox tb = new TextBox();
                tb.ID = "Txtvalue_" + idctr + "_" + (i+1);
                tb.CssClass = "textb";
                tb.Attributes.Add("onchange", "return keypress(" + idctr + ",1)");
                tb.Width = 80;
                tb.Text = filteredRows[i]["value"].ToString();
                // Add the control to the TableCell
                cellNext.Controls.Add(tb);
                // Add the TableCell to the TableRow
                row.Cells.Add(cellNext);
            }
            TableCell cellavg = new TableCell();
            TextBox tbavg = new TextBox();
            tbavg.ID = "txtavg_" + ctr;
            tbavg.CssClass = "textb";
            tbavg.Style.Add("width", "85%");
            tbavg.Text = filteredRows[0]["avgvalue"].ToString();
            cellavg.Controls.Add(tbavg);
            row.Cells.Add(cellavg);
            table.Rows.Add(row);
        }
        pnlchecklist.Controls.Add(table);
    }
    private void InitTable(int nooftests,int totalchkpnt)
    {
        string chkpointname = string.Empty;
        table = null;
        table = new Table();
        
        table.Style.Add("border", "1px");
        table.Style.Add("width", "100%");
        string idctr = string.Empty;
        
        for(int k=1;k<=Convert.ToInt16(Session["TOTALCHECKPOINT"]);k++)
        {
            if (Session["ds"] != null)
            {
                DataSet dsinit = (DataSet)Session["ds"];

                if (dsinit.Tables[0].Rows.Count > 0)
                {

                    chkpointname = dsinit.Tables[0].Rows[k-1]["parametername"].ToString();
    
            }
            }
            ctr++;
            TableRow row = new TableRow();
            row.ID = "tr_" + ctr;

            TableCell cellstart = new TableCell();
            cellstart.Style.Add("width", "10%");
            cellstart.Style.Add("padding", "1 % 1 % 1 % 1 %");
            cellstart.Style.Add("text-align", "center");
            Label lblsr = new Label();
            lblsr.ID = "lblsr_" + ctr;
            lblsr.CssClass = "labelbold";
           // lblsr.Text = ;
            idctr = k.ToString(); 
            cellstart.Controls.Add(lblsr);
            row.Cells.Add(cellstart);
            TableCell cellchkpoint = new TableCell();
            cellchkpoint.Style.Add("width", "15%");
            cellchkpoint.Style.Add("padding", "1 % 1 % 1 % 1 %");
            cellchkpoint.Style.Add("text-align", "center");
            Label lblchkpoint = new Label();
            lblchkpoint.ID = "lblchkpoint_" + idctr;
            lblchkpoint.CssClass = "labelbold";
            lblchkpoint.Text = chkpointname; //dr["ParameterName"].ToString();
            //  lblchkpoint.Style.Add("width", "21%");
            cellchkpoint.Controls.Add(lblchkpoint);
            row.Cells.Add(cellchkpoint);
            TableCell cellspeci = new TableCell();
            TextBox tbspeci = new TextBox();

            tbspeci.ID = "txtspecification_" + idctr;

            tbspeci.CssClass = "textb";
            tbspeci.Style.Add("width", "85%");
            cellspeci.Controls.Add(tbspeci);


            row.Cells.Add(cellspeci);
            for (int i = 1; i <= nooftests; i++)
            {
                ctrtext++;
                TableCell cellNext = new TableCell();
                TextBox tb = new TextBox();
                tb.ID = "Txtvalue_" + idctr + "_" + i;
                tb.CssClass = "textb";
                tb.Width = 80;
                tb.Attributes.Add("onchange","return keypress("+ idctr+",1)");
                // Add the control to the TableCell
                cellNext.Controls.Add(tb);
                // Add the TableCell to the TableRow
                row.Cells.Add(cellNext);
            }
            TableCell cellavg = new TableCell();
            TextBox tbavg = new TextBox();
            tbavg.ID = "txtavg_" + ctr;
            tbavg.CssClass = "textb";
            tbavg.Style.Add("width", "85%");
            cellavg.Controls.Add(tbavg);
            row.Cells.Add(cellavg);
            table.Rows.Add(row);
        }
        pnlchecklist.Controls.Add(table);
    }
    protected void DDDocNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hndocid.Value = DDDocNo.SelectedValue;
        refreshcontrol();
        FillDataback();
    }
    protected void FillDataback()
    {
       // hndocid.Value = string.Empty;
        string str = @"SELECT RIM.DOCNO,RID.SUPPLIERNAME,REPLACE(CONVERT(NVARCHAR(11),RIM.REPORTDATE,106),' ','-') AS REPORTDATE,RID.CHALLANNO_DATE,RID.YARNTYPE,RID.COUNT,RID.LOTNO,
                        RID.TOTALBALE,RID.SAMPLESIZE,RID.NOOFHANK,
                        
                        RIM.COMMENTS,RIM.STATUS,RIM.Approvestatus,RID.VenderLotNo 
                        FROM RAWYARNINSPECTIONMASTER RIM 
                        INNER JOIN RAWYARNINSPECTIONDETAIL_NEW RID ON RIM.DOCID=RID.DOCID 
						
                        Where RIM.DOCID=" + DDDocNo.SelectedValue+ ";select * from RAWYARNINSPECTIONCHECKPOINTDETAIL WHERE DOCID=" + DDDocNo.SelectedValue;

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
            TxtVenderLotNo.Text = ds.Tables[0].Rows[0]["VenderLotNo"].ToString();

            FillTable(ds, 5);
            //1
            //txtspecification_1.Text = ds.Tables[0].Rows[0]["Specification_1"].ToString();
            //txt1_1.Text = ds.Tables[0].Rows[0]["One_1"].ToString();
            //txt1_2.Text = ds.Tables[0].Rows[0]["Two_1"].ToString();
            //txt1_3.Text = ds.Tables[0].Rows[0]["Three_1"].ToString();
            //txt1_4.Text = ds.Tables[0].Rows[0]["Four_1"].ToString();
            //txt1_5.Text = ds.Tables[0].Rows[0]["Five_1"].ToString();
            //txtavgvalue_1.Text = ds.Tables[0].Rows[0]["Avgvalue_1"].ToString();
            ////2
            //txtspecification_2.Text = ds.Tables[0].Rows[0]["Specification_2"].ToString();
            //txt2_1.Text = ds.Tables[0].Rows[0]["One_2"].ToString();
            //txt2_2.Text = ds.Tables[0].Rows[0]["Two_2"].ToString();
            //txt2_3.Text = ds.Tables[0].Rows[0]["Three_2"].ToString();
            //txt2_4.Text = ds.Tables[0].Rows[0]["Four_2"].ToString();
            //txt2_5.Text = ds.Tables[0].Rows[0]["Five_2"].ToString();
            //txtavgvalue_2.Text = ds.Tables[0].Rows[0]["Avgvalue_2"].ToString();
            ////3
            //txtspecification_3.Text = ds.Tables[0].Rows[0]["Specification_3"].ToString();
            //txt3_1.Text = ds.Tables[0].Rows[0]["One_3"].ToString();
            //txt3_2.Text = ds.Tables[0].Rows[0]["Two_3"].ToString();
            //txt3_3.Text = ds.Tables[0].Rows[0]["Three_3"].ToString();
            //txt3_4.Text = ds.Tables[0].Rows[0]["Four_3"].ToString();
            //txt3_5.Text = ds.Tables[0].Rows[0]["Five_3"].ToString();
            //txtavgvalue_3.Text = ds.Tables[0].Rows[0]["Avgvalue_3"].ToString();
            ////4
            //txtspecificationpet_4.Text = ds.Tables[0].Rows[0]["Specificationpet_4"].ToString();
            //lblcheckpointpet_4.Text = ds.Tables[0].Rows[0]["CHECKPOINTPET_4"].ToString();
            //txtpet4_1.Text = ds.Tables[0].Rows[0]["Onepet_4"].ToString();
            //txtpet4_2.Text = ds.Tables[0].Rows[0]["Twopet_4"].ToString();
            //txtpet4_3.Text = ds.Tables[0].Rows[0]["Threepet_4"].ToString();
            //txtpet4_4.Text = ds.Tables[0].Rows[0]["Fourpet_4"].ToString();
            //txtpet4_5.Text = ds.Tables[0].Rows[0]["Fivepet_4"].ToString();
            //txtavgvaluepet_4.Text = ds.Tables[0].Rows[0]["Avgvaluepet_4"].ToString();

            //txtspecificationother_4.Text = ds.Tables[0].Rows[0]["Specificationother_4"].ToString();
            //lblcheckpointother_4.Text = ds.Tables[0].Rows[0]["CHECKPOINTOTHER_4"].ToString();
            //txtother4_1.Text = ds.Tables[0].Rows[0]["Oneother_4"].ToString();
            //txtother4_2.Text = ds.Tables[0].Rows[0]["Twoother_4"].ToString();
            //txtother4_3.Text = ds.Tables[0].Rows[0]["Threeother_4"].ToString();
            //txtother4_4.Text = ds.Tables[0].Rows[0]["Fourother_4"].ToString();
            //txtother4_5.Text = ds.Tables[0].Rows[0]["Fiveother_4"].ToString();
            //txtavgvalueother_4.Text = ds.Tables[0].Rows[0]["AvgvalueOther_4"].ToString();
            ////5
            //txtspecification_5.Text = ds.Tables[0].Rows[0]["Specification_5"].ToString();
            //txt5_1.Text = ds.Tables[0].Rows[0]["One_5"].ToString();
            //txt5_2.Text = ds.Tables[0].Rows[0]["Two_5"].ToString();
            //txt5_3.Text = ds.Tables[0].Rows[0]["Three_5"].ToString();
            //txt5_4.Text = ds.Tables[0].Rows[0]["Four_5"].ToString();
            //txt5_5.Text = ds.Tables[0].Rows[0]["Five_5"].ToString();
            //txtavgvalue_5.Text = ds.Tables[0].Rows[0]["Avgvalue_5"].ToString();
            ////5
            //txtspecification_6.Text = ds.Tables[0].Rows[0]["Specification_6"].ToString();
            //txt6_1.Text = ds.Tables[0].Rows[0]["One_6"].ToString();
            //txt6_2.Text = ds.Tables[0].Rows[0]["Two_6"].ToString();
            //txt6_3.Text = ds.Tables[0].Rows[0]["Three_6"].ToString();
            //txt6_4.Text = ds.Tables[0].Rows[0]["Four_6"].ToString();
            //txt6_5.Text = ds.Tables[0].Rows[0]["Five_6"].ToString();
            //txtavgvalue_6.Text = ds.Tables[0].Rows[0]["Avgvalue_6"].ToString();
            ////7
            //txtspecification_7.Text = ds.Tables[0].Rows[0]["Specification_7"].ToString();
            //txt7_1.Text = ds.Tables[0].Rows[0]["One_7"].ToString();
            //txt7_2.Text = ds.Tables[0].Rows[0]["Two_7"].ToString();
            //txt7_3.Text = ds.Tables[0].Rows[0]["Three_7"].ToString();
            //txt7_4.Text = ds.Tables[0].Rows[0]["Four_7"].ToString();
            //txt7_5.Text = ds.Tables[0].Rows[0]["Five_7"].ToString();
            //txtavgvalue_7.Text = ds.Tables[0].Rows[0]["Avgvalue_7"].ToString();
            ////
            txtcomments.Text = ds.Tables[0].Rows[0]["comments"].ToString();
            //ddresult.SelectedItem.Text = ds.Tables[0].Rows[0]["status"].ToString();
            if (ddresult.Items.FindByText(ds.Tables[0].Rows[0]["status"].ToString()) != null)
            {
                ddresult.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
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
        TxtVenderLotNo.Text = "";
        //1
        //txtspecification_1.Text = "";
        //txt1_1.Text = "";
        //txt1_2.Text = "";
        //txt1_3.Text = "";
        //txt1_4.Text = "";
        //txt1_5.Text = "";
        //txtavgvalue_1.Text = "";
        ////2
        //txtspecification_2.Text = "";
        //txt2_1.Text = "";
        //txt2_2.Text = "";
        //txt2_3.Text = "";
        //txt2_4.Text = "";
        //txt2_5.Text = "";
        //txtavgvalue_2.Text = "";
        ////3
        //txtspecification_3.Text = "";
        //txt3_1.Text = "";
        //txt3_2.Text = "";
        //txt3_3.Text = "";
        //txt3_4.Text = "";
        //txt3_5.Text = "";
        //txtavgvalue_3.Text = "";
        ////4
        //txtspecificationpet_4.Text = "";
        //txtpet4_1.Text = "";
        //txtpet4_2.Text = "";
        //txtpet4_3.Text = "";
        //txtpet4_4.Text = "";
        //txtpet4_5.Text = "";
        //txtavgvaluepet_4.Text = "";

        //txtspecificationother_4.Text = "";
        //txtother4_1.Text = "";
        //txtother4_2.Text = "";
        //txtother4_3.Text = "";
        //txtother4_4.Text = "";
        //txtother4_5.Text = "";
        //txtavgvalueother_4.Text = "";
        ////5
        //txtspecification_5.Text = "";
        //txt5_1.Text = "";
        //txt5_2.Text = "";
        //txt5_3.Text = "";
        //txt5_4.Text = "";
        //txt5_5.Text = "";
        //txtavgvalue_5.Text = "";
        ////5
        //txtspecification_6.Text = "";
        //txt6_1.Text = "";
        //txt6_2.Text = "";
        //txt6_3.Text = "";
        //txt6_4.Text = "";
        //txt6_5.Text = "";
        //txtavgvalue_6.Text = "";
        ////7
        //txtspecification_7.Text = "";
        //txt7_1.Text = "";
        //txt7_2.Text = "";
        //txt7_3.Text = "";
        //txt7_4.Text = "";
        //txt7_5.Text = "";
        //txtavgvalue_7.Text = "";
        //
        txtcomments.Text = "";

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
            SqlParameter[] param = new SqlParameter[9];
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

            //*********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVERAWYARNINSPECTION", param);
            lblmsg.Text = param[4].Value.ToString();
            txtdocno.Text = param[2].Value.ToString();
            hndocid.Value = param[0].Value.ToString();

            // at the time of update delete all the data in tables
            string str1 = @"DELETE FROM RAWYARNINSPECTIONDETAIL_NEW WHERE DOCID=" + hndocid.Value;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);

            insertinto_RAWYARNINSPECTIONDETAIL(Tran);

            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + param[4].Value.ToString() + "')", true);

            Tran.Commit();
            Session["TOTALCHECKPOINT"] = null;
            Session["totaltest"] = null;
            Session["ds"] = null;

            pnlchecklist.Controls.Clear();
            hndocid.Value = "0";
            dquality.SelectedIndex = -1;
            //txtdocno.Text = string.Empty;
            

            //**********Upload image
            //SaveImage(Convert.ToInt32(hndocid.Value));
            //**********

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
          //  Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
            Session["TOTALCHECKPOINT"] = null;
        }
    }

    private void insertinto_RAWYARNINSPECTIONDETAIL(SqlTransaction Tran)
    {
        string str = @"Insert into RAWYARNINSPECTIONDETAIL_NEW (DOCID, SUPPLIERNAME, CHALLANNO_DATE, YARNTYPE, COUNT, LOTNO, 
                     TOTALBALE, SAMPLESIZE, NOOFHANK, VenderLotNo)
                     values(" + hndocid.Value + ",'" + txtsuppliername.Text.Replace("'", "''") + "','" + txtchallannodate.Text.Replace("'", "''") + "','" + txtyarntype.Text.Replace("'", "''") + "','" + txtcount.Text.Replace("'", "''") + "','" + txtlotno.Text.Replace("'", "''") + @"',
                     " + (txttotalbale.Text == "" ? "0" : txttotalbale.Text) + ", " + (txtsamplesize.Text == "" ? "0" : txtsamplesize.Text) + "," + (txtnoofhank.Text == "" ? "0" : txtnoofhank.Text) + ",'" + TxtVenderLotNo.Text.Replace("'", "''") + "')";

        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);

       
        DataTable dt = new DataTable();

        dt.Columns.Add("DOCID", typeof(int));
        dt.Columns.Add("CHECKPOINTID", typeof(int));
        dt.Columns.Add("CHECKPOINTNAME", typeof(string));
        dt.Columns.Add("SPECIFICATIONID", typeof(int));
        dt.Columns.Add("SPECIFICATIONVALUE", typeof(int));
        dt.Columns.Add("VALUEID", typeof(int));
        dt.Columns.Add("VALUE", typeof(double));
        dt.Columns.Add("AVGID", typeof(int));
        dt.Columns.Add("AVGVALUE", typeof(double));
        for (int i = 1; i <= Convert.ToInt32(Session["TOTALCHECKPOINT"]); i++)
        {
          
            
            Label lblcheckpointID = (Label)pnlchecklist.FindControl("lblsr_"+i);
            Label lblcheckpointName = (Label)pnlchecklist.FindControl("lblchkpoint_"+i);
            TextBox txtspecification = (TextBox)pnlchecklist.FindControl("txtspecification_" + i);
            TextBox txtavg = (TextBox)pnlchecklist.FindControl("txtavg_" + i);
            if (txtavg.Text != "")
            {
                for (int j = 1; j <= Convert.ToInt32(Session["totaltest"]); j++)
                {
                    DataRow dr = dt.NewRow();
                    TextBox txtval = (TextBox)pnlchecklist.FindControl("Txtvalue_" + i + "_" + j);
                    dr["DOCID"] = hndocid.Value;
                    dr["CHECKPOINTID"] = i;
                    dr["CHECKPOINTNAME"] = lblcheckpointName.Text;
                    dr["SPECIFICATIONID"] = i;
                    dr["SPECIFICATIONVALUE"] = txtspecification.Text;
                    dr["VALUEID"] = j;
                    dr["VALUE"] = txtval.Text;
                    //  dr["Result"] = ddresult.SelectedItem.Text;
                    dr["AVGID"] = i;
                    dr["AVGVALUE"] = txtavg.Text;
                    dt.Rows.Add(dr);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('Please enter some data before submitting.')", true);
                Tran.Rollback();
                dt.Clear();
                return;


            }
               
           
        }
        if (dt.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('Please enter some data before submitting.')", true);
            Tran.Rollback();
            dt.Clear();
            return;
        }
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}
     //   SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@Docid", SqlDbType.Int);
          //  param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hndocid.Value;
            param[1] = new SqlParameter("@dt", dt);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;

            //*********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEYARNCHECKPOINTINSPECTION", param);
            //lblmsg.Text = param[4].Value.ToString();
            //txtdocno.Text = param[2].Value.ToString();
            lblmsg.Text = param[2].Value.ToString();

            // at the time of update delete all the data in tables           

            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + param[0].Value.ToString() + "')", true);

           // Tran.Commit();
          //  FillDataback();
            //**********

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            //con.Close();
            //con.Dispose();
        }

    }
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        fillDocno();
    }
    private void fillDocno()
    {
        string str = @"SELECT RIM.DOCID,RIM.DOCNO +' # ' +Replace(convert(nvarchar(11),RIM.Reportdate,106),' ','-') as DocNo 
                    FROM RAWYARNINSPECTIONMASTER RIM(nolock) 
                    INNER JOIN RAWYARNINSPECTIONDETAIL_NEW RID(nolock) ON RIM.DOCID=RID.DOCID
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

                Session["rptFileName"] = "~\\Reports\\rptrawyarninspection.rpt";
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptrawyarninspection.xsd";
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
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dquality, "select qualityid,qualityname from quality where item_id=" + ddItemName.SelectedValue, true, "--Select--");

    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["totaltest"] = null;
        Session["TOTALCHECKPOINT"] = null;
        if (ddCategoryName.SelectedIndex > 0 && ddItemName.SelectedIndex > 0 && dquality.SelectedIndex > 0)
        {
            FillGridParameter();
        }
    }
    protected void ddcategoryname_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["totaltest"] = null;
        Session["TOTALCHECKPOINT"] = null;
        UtilityModule.ConditionalComboFill(ref ddItemName, "SELECT ITEM_ID, ITEM_NAME froM ITEM_MASTER where CATEGORY_ID=" + ddCategoryName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By ITEM_NAME", true, "---Select --");

    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        pnlchecklist.Controls.Clear();
       // Session["TOTALCHECKPOINT"] = null;
       // Session["totaltest"] = null;

    }
}