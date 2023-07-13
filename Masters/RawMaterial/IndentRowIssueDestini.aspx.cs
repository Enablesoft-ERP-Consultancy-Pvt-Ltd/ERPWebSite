using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_RawMaterial_IndentRowIssueDestini : System.Web.UI.Page
{
    int ItemFinishedId = 0;
    static int MasterCompanyId;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varCompanyId"]);
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["Prmid"] = 0;
            ViewState["Prtid"] = 0;
            string str = @"select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By Companyname
                          select DISTINCT PROCESS_NAME_ID,process_name from PROCESS_NAME_MASTER pm inner join IndentMaster im on pm.PROCESS_NAME_ID=im.processid And pm.MasterCompanyId=" + Session["varCompanyId"] + @" order by PROCESS_NAME_ID
                         select godownid,godownname from godownmaster Where MasterCompanyId=" + Session["varCompanyId"];
            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "Select Comp Name");
            
            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddProcessName, ds, 1, true, "Select Process Name");
            UtilityModule.ConditionalComboFillWithDS(ref ddgodown, ds, 2, true, "Select Godown");
            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            lablechange();
            ddProcessName.Focus();
            int VarProdCode = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarProdCode From MasterSetting"));
            switch (VarProdCode)
            {
                case 1:
                    procode.Visible = true;
                    TdFinish_Type.Visible = true;
                    break;
                case 0:
                    procode.Visible = false;
                    TdFinish_Type.Visible = false;
                    break;
            }
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
        lblshadecolor.Text = ParameterList[7];
    }
    protected void ddProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = @"Select distinct id.IndentId,'('+om.localorder+'   '+customerorderno+') ' + IndentNo from IndentMaster im inner join 
                     Indentdetail id on im.indentid=id.indentid inner join ordermaster om on om.orderid=id.orderid Where  im.processid=" + ddProcessName.SelectedValue + " And im.MasterCompanyId=" + Session["varCompanyId"];

        if (TxtProdCode.Text != "")
        {
            Str = Str + @"   AND id.IFINISHEDID in ( select ITEM_FINISHED_ID from ITEM_PARAMETER_MASTER where ProductCode ='" + TxtProdCode.Text.Trim() + "')";
        }
        if (ChkEditOrder.Checked == true)
        {
            Str = Str + @" And IM.Indentid in(select distinct indentid from PP_ProcessRawTran)";
        }

        Str = Str + "   order by  '('+om.localorder+'   '+customerorderno+') ' + IndentNo ";
        UtilityModule.ConditionalComboFill(ref ddindentno, Str, true, "Select Order No");
        ddindentno.Focus();
    }
    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {
        EMP_CHANGE();
        if (ChkEditOrder.Checked == true)
        {
            // Fill_Grid();
            string str = @"Select Distinct Challanno,Challanno ChallanText from PP_ProcessRawMaster PRM,PP_ProcessRawTran PRT
                            Where PRM.PRMid=PRT.PRMid AND PRM.EMpID=" + ddempname.SelectedValue + " AND PRM.ProcessID=" + ddProcessName.SelectedValue + " AND PRT.Indentid=" + ddindentno.SelectedValue + "  Order By ChallanText";
            UtilityModule.ConditionalComboFill(ref DDChallanNo, str, true, "--Select--");
        }
    }
    private void EMP_CHANGE()
    {
        UtilityModule.ConditionalComboFill(ref ddCatagory, @"  SELECT DISTINCT icm.CATEGORY_ID , icm.CATEGORY_NAME  FROM ITEM_MASTER im INNER JOIN
        ITEM_PARAMETER_MASTER ipm ON im.ITEM_ID = ipm.ITEM_ID INNER JOIN ITEM_CATEGORY_MASTER icm ON im.CATEGORY_ID = icm.CATEGORY_ID INNER JOIN
        IndentDetail  id INNER JOIN   IndentMaster idm ON id.IndentId = idm.IndentID ON  ipm.ITEM_FINISHED_ID = id.IFinishedId
        Where idm.partyid=" + ddempname.SelectedValue + " and idm.processid=" + ddProcessName.SelectedValue + " and  idm.IndentId=" + ddindentno.SelectedValue + " And Idm.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Category Name");
        TxtProdCode.Text = "";
        txtdate.Focus();
    }
    protected void ddindentno_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ChkEditOrder.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddempname, "SELECT distinct E.EmpId,E.EmpName FROM EmpInfo E,PP_ProcessRawMaster pp, PP_ProcessRawTRAN PT where  pp.empid=e.empid AND PP.PRMid=PT.PRMid AND PT.Indentid=" + ddindentno.SelectedValue + " And PP.Processid=" + ddProcessName.SelectedValue + " And PP.MasterCompanyId=" + Session["VARCOMPANYNO"] + " And PP.CompanyId=" + ddCompName.SelectedValue + "", true, "Select Emp");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddempname, "SELECT distinct E.EmpId,E.EmpName FROM IndentMaster IM INNER JOIN  EmpInfo E ON IM.PartyId=E.EmpId And IM.Processid=" + ddProcessName.SelectedValue + " and im.IndentID=" + ddindentno.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " And IM.CompanyId= " + ddCompName.SelectedValue, true, "Select Emp");
        }
        ddindent_selectchange();

    }
    private void ddindent_selectchange()
    {
        if (ChkEditOrder.Checked == true)
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select PRMID From PP_ProcessRawTran where indentid=" + ddindentno.SelectedValue + "");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["Prmid"] = ds.Tables[0].Rows[0][0].ToString();
                // Fill_Grid();
                btnpriview.Visible = true;
                Show_Report();
            }
        }

        Fill_Grid_Show();
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_CategorySelectedChange();
    }
    private void Fill_CategorySelectedChange()
    {
        ddlcategorycange();
        UtilityModule.ConditionalComboFill(ref dditemname, @"SELECT DISTINCT dbo.ITEM_MASTER.ITEM_ID, dbo.ITEM_MASTER.ITEM_NAME FROM  dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                      dbo.ITEM_MASTER ON dbo.ITEM_PARAMETER_MASTER.ITEM_ID = dbo.ITEM_MASTER.ITEM_ID
                      WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " AND dbo.ITEM_MASTER.CATEGORY_ID =" + ddCatagory.SelectedValue + " And ITEM_MASTER.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Item--");
    }
    private void ddlcategorycange()
    {
        dditemname.Items.Clear();
        dquality.Items.Clear();
        dddesign.Items.Clear();
        ddcolor.Items.Clear();
        ddshape.Items.Clear();
        ddsize.Items.Clear();
        ddlshade.Items.Clear();
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                      " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                      " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCatagory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        ql.Visible = true;
                        break;
                    case "2":
                        dsn.Visible = true;
                        UtilityModule.ConditionalComboFill(ref dddesign, @"SELECT DISTINCT dbo.Design.designId, dbo.Design.designName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                        dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                        dbo.Design ON dbo.ITEM_PARAMETER_MASTER.DESIGN_ID = dbo.Design.designId
                        WHERE  dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Design--");
                        break;
                    case "3":
                        clr.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddcolor, @"SELECT DISTINCT dbo.color.ColorId, dbo.color.ColorName FROm dbo.ITEM_PARAMETER_MASTER INNER JOIN
                        dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                        dbo.color ON dbo.ITEM_PARAMETER_MASTER.COLOR_ID = dbo.color.ColorId
                        WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Color--");
                        // dsn.Visible = true;
                        break;
                    case "4":
                        shp.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddshape, @"SELECT DISTINCT dbo.Shape.ShapeId, dbo.Shape.ShapeName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                        dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                        dbo.Shape ON dbo.ITEM_PARAMETER_MASTER.SHAPE_ID = dbo.Shape.ShapeId
                        WHERE dbo.IndentDetail.IndentId = " + ddindentno.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Shape--");
                        break;
                    case "5":
                        sz.Visible = true;
                        CheckBox1.Checked = false;
                        UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size.SizeFt FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                        dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                        dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
                        WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Size in Ft");
                        break;
                    case "6":
                        shd.Visible = true;
                        break;
                }
            }
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_ItemSelectedChange();
    }
    private void Fill_ItemSelectedChange()
    {
        dquality.Items.Clear();
        ddlshade.Items.Clear();
        UtilityModule.ConditionalComboFill(ref dquality, @"SELECT DISTINCT dbo.Quality.QualityId,dbo.Quality.QualityName
                     FROM  dbo.ITEM_PARAMETER_MASTER INNER JOIN
                     dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
                     dbo.Quality ON dbo.ITEM_PARAMETER_MASTER.QUALITY_ID = dbo.Quality.QualityId
                     WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + "and quality.item_id=" + dditemname.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Quallity");
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Validated();
        if (lblqty.Visible == false && Lblfinished.Visible == false && Label2.Visible == false)
        {
            save_detail();
        }
    }
    protected void save_detail()
    {
        Label1.Visible = false;
        Label1.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[17];
            arr[0] = new SqlParameter("@prmid", SqlDbType.Int);
            arr[1] = new SqlParameter("@companyid", SqlDbType.Int);
            arr[2] = new SqlParameter("@empid", SqlDbType.Int);
            arr[3] = new SqlParameter("@processid", SqlDbType.Int);
            arr[4] = new SqlParameter("@issuedate", SqlDbType.DateTime);
            arr[5] = new SqlParameter("@chalanno", SqlDbType.NVarChar, 150);
            arr[6] = new SqlParameter("@varuserid", SqlDbType.Int);
            arr[7] = new SqlParameter("@varcompanyid", SqlDbType.Int);
            arr[8] = new SqlParameter("@prtid", SqlDbType.Int);
            arr[9] = new SqlParameter("@finishedId", SqlDbType.Int);
            arr[10] = new SqlParameter("@godownId", SqlDbType.Int);
            arr[11] = new SqlParameter("@issueQuantity", SqlDbType.Float);
            arr[12] = new SqlParameter("@indentid", SqlDbType.Int);
            arr[13] = new SqlParameter("@unitid", SqlDbType.Int);
            arr[14] = new SqlParameter("@lotno", SqlDbType.NVarChar, 50);
            arr[15] = new SqlParameter("@Finish_Type", SqlDbType.NVarChar, 50);
            arr[16] = new SqlParameter("@Remark", SqlDbType.NVarChar, 250);
            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = ViewState["Prmid"];
            arr[1].Value = ddCompName.SelectedValue;
            arr[2].Value = ddempname.SelectedValue;
            arr[3].Value = ddProcessName.SelectedValue;
            arr[4].Value = txtdate.Text;
            arr[5].Direction = ParameterDirection.InputOutput;
            arr[5].Value = txtchalanno.Text;
            arr[6].Value = Session["varuserid"].ToString();
            arr[7].Value = Session["varCompanyId"].ToString();
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, tran, ddlshade, "", Convert.ToInt32(Session["varCompanyId"]));
            arr[8].Direction = ParameterDirection.InputOutput;
            arr[8].Value = ViewState["Prtid"];
            arr[9].Value = Varfinishedid;
            arr[10].Value = ddgodown.SelectedValue;
            arr[11].Value = txtissqty.Text;
            arr[12].Value = ddindentno.SelectedValue;
            arr[13].Value = ddlunit.SelectedValue;
            arr[14].Value = ddlotno.SelectedIndex != -1 ? ddlotno.SelectedItem.Text : "Without Lot No";
            arr[15].Value = TdFinish_Type.Visible == true ? Convert.ToInt32(ddFinish_Type.SelectedValue == "" ? "0" : ddFinish_Type.SelectedValue) : 0;
            arr[16].Value = txtremarks.Text;
            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[PRO_PP_PRM_Destini]", arr);
            txtchalanno.Text = arr[5].Value.ToString();
            ViewState["Prmid"] = arr[0].Value.ToString();
            ViewState["Prtid"] = arr[8].Value.ToString();
            UtilityModule.StockStockTranTableUpdate(Varfinishedid, Convert.ToInt32(ddgodown.SelectedValue), Convert.ToInt32(ddCompName.SelectedValue), arr[14].Value.ToString(), Convert.ToDouble(txtissqty.Text), Convert.ToDateTime(txtdate.Text).ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), "PP_ProcessRawTran", Convert.ToInt32(ViewState["Prtid"]), tran, 0, false, 1, Convert.ToInt32(ddFinish_Type.SelectedValue));
            Show_Report();
            ViewState["Prtid"] = 0;
            tran.Commit();
            Label1.Text = "Data Saved SuccessFully";
            Label1.Visible = true;
            btnpriview.Visible = true;
            pnl1.Enabled = false;
            btnsave.Text = "Save";
            Fill_Grid();
            save_refresh();
        }
        catch
        {
            tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void save_refresh()
    {
        ddlcategorycange();
        dditemname.Items.Clear();
        dquality.Items.Clear();
        ddgodown.SelectedIndex = 0;
        txtissue.Text = "";
        TxtProdCode.Text = "";
        txtpendingqty.Text = "";
        txtpreissue.Text = "";
        txtissqty.Text = "";
        txtstock.Text = "";
        txtremarks.Text = "";
    }
    private void Fill_Grid()
    {
        gvdetail.DataSource = fill_Data_grid();
        gvdetail.DataBind();
        if (gvdetail.Rows.Count > 0)
        {
            gvdetail.Visible = true;
        }
        else
        {
            gvdetail.Visible = false;
        }
        Show_Report();
    }
    private DataSet fill_Data_grid()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"SELECT prt.PRTid as prtid,prm.prmid as prmid,prt.LotNo, icm.CATEGORY_NAME, im.ITEM_NAME, gm.GodownName, 
            prt.IssueQuantity,IPM1.QDCS + Space(2)+isnull(SizeMtr,'')+ft.finished_type_name DESCRIPTION,prt.remark  FROM  
            pp_ProcessRawTran prt  INNER JOIN ITEM_PARAMETER_MASTER ipm ON prt.Finishedid = ipm.ITEM_FINISHED_ID INNER JOIN
            GodownMaster gm ON prt.Godownid = gm.GoDownID INNER JOIN
            item_master im on im.item_id=ipm.item_id inner join
            iTEM_CATEGORY_MASTER icm ON im.Category_id = icm.CATEGORY_ID inner join
            pp_ProcessRawMaster prm on  prt.prmid=prm.prmid inner Join
            ViewFindFinishedidItemidQDCSS IPM1 on IPM.Item_Finished_Id=IPM1.Finishedid inner join
            FINISHED_TYPE ft on ft.id=prt.finish_type_id
            where prm.prmid=" + ViewState["Prmid"] + " And ipm.MasterCompanyId=" + Session["varCompanyId"];
            if (ddempname.SelectedIndex > 0)
            {
                strsql = strsql + " and prm.empid=" + ddempname.SelectedValue + "";
            }
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_process_ProcessRawIssue|fill_Data_grid|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        return ds;
    }
    protected void txtchalan_ontextchange(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            string ChalanNo = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "select isnull(ChalanNo,0) asd from ProcessRawMaster where TypeFlag = 0 And ChalanNo='" + txtchalanno.Text + "' And MasterCompanyId=" + Session["varCompanyId"] + ""));
            if (ChalanNo != "")
            {
                txtchalanno.Text = "";
                txtchalanno.Focus();
                Label1.Visible = true;
                Label1.Text = "ChalanNO already exist";
            }
            else
            {
                Label1.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void gvdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvdetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gvdetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvdetail.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    private int save_finishedid()
    {
        int VarOutfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        ItemFinishedId = VarOutfinishedid;
        return ItemFinishedId;
    }
    protected void txtdate_TextChanged(object sender, EventArgs e)
    {
        if (txtdate.Text == "")
        {
            lbldate.Visible = true;
        }
        else
        {
            lbldate.Visible = false;
        }
    }
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBox1.Checked == false)
        {
            UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size.SizeFt FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
            dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
            dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
            WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"], true, "Size in Ft");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size.Sizemtr FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
            dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
            dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
            WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"], true, "Size in mtr");
        }
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        if (ddindentno.SelectedIndex > 0)
        {
            txt_prod_change();
        }
    }
    private void txt_prod_change()
    {
        DataSet ds;
        string Str;
        if (TxtProdCode.Text != "")
        {
            ddCatagory.SelectedIndex = 0;
            Str = @"SELECT DISTINCT
                  ID.IndentDetailId, ID.IndentId, IM.CATEGORY_ID, IPM.ITEM_FINISHED_ID,
                  IPM.QUALITY_ID, IPM.DESIGN_ID, IPM.COLOR_ID,IPM.SHAPE_ID, IPM.SIZE_ID, IPM.DESCRIPTION, 
                  IPM.ITEM_ID, IPM.ProductCode, IPM.SHADECOLOR_ID  FROM ITEM_MASTER IM INNER JOIN
                  ITEM_PARAMETER_MASTER IPM ON IM.ITEM_ID = IPM.ITEM_ID INNER JOIN
                  IndentDetail ID INNER JOIN IndentMaster IDM ON ID.IndentId = IDM.IndentID ON 
                  IPM.ITEM_FINISHED_ID = ID.IFinishedId where ID.IndentId=" + ddindentno.SelectedValue + " And productcode='" + TxtProdCode.Text + "' And IPM.MasterCompanyId=" + Session["varCompanyId"];

            //            Str = @" SELECT DISTINCT 
            //                  dbo.IndentDetail.IndentDetailId, dbo.IndentDetail.IndentId, dbo.ITEM_MASTER.CATEGORY_ID, dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID, 
            //                  dbo.ITEM_PARAMETER_MASTER.QUALITY_ID, dbo.ITEM_PARAMETER_MASTER.DESIGN_ID, dbo.ITEM_PARAMETER_MASTER.COLOR_ID, 
            //                  dbo.ITEM_PARAMETER_MASTER.SHAPE_ID, dbo.ITEM_PARAMETER_MASTER.SIZE_ID, dbo.ITEM_PARAMETER_MASTER.DESCRIPTION, 
            //                  dbo.ITEM_PARAMETER_MASTER.ITEM_ID, dbo.ITEM_PARAMETER_MASTER.ProductCode, dbo.ITEM_PARAMETER_MASTER.SHADECOLOR_ID  FROM  dbo.ITEM_MASTER INNER JOIN
            //                  dbo.ITEM_PARAMETER_MASTER ON dbo.ITEM_MASTER.ITEM_ID = dbo.ITEM_PARAMETER_MASTER.ITEM_ID INNER JOIN
            //                  dbo.IndentDetail INNER JOIN dbo.IndentMaster ON dbo.IndentDetail.IndentId = dbo.IndentMaster.IndentID ON 
            //                  dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId where dbo.IndentDetail.IndentId=" + ddindentno.SelectedValue + " And productcode='" + TxtProdCode.Text + "' And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddCatagory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                Fill_CategorySelectedChange();
                dditemname.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                Fill_ItemSelectedChange();
                dquality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                dddesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                ddcolor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                ddshape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                ddsize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                ddlshade.SelectedValue = ds.Tables[0].Rows[0]["SHADECOLOR_ID"].ToString();
                Session["finishedid"] = ds.Tables[0].Rows[0]["Item_Finished_id"].ToString();
                Session["indentid"] = ds.Tables[0].Rows[0]["IndentId"].ToString();
                Session["IndentdetailId"] = ds.Tables[0].Rows[0]["IndentDetailId"].ToString();

                Fill_Quantity();
                Label2.Visible = false;
                ddgodown.Focus();
            }
            else
            {
                Label2.Visible = true;
                ddCatagory.SelectedIndex = 0;
                ddgodown.SelectedIndex = 0;
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
            }
        }
        else
        {
            ddCatagory.SelectedIndex = 0;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = @"Select DISTINCT ProductCode from ITEM_PARAMETER_MASTER IPM inner join item_Master IM on IM.Item_Id=IPM.Item_Id inner join CategorySeparate CS on CS.CategoryId=IM.Category_Id 
                            inner join indentdetail IND on IND.IFinishedId=IPM.ITEM_FINISHED_ID  where ProductCode Like  '" + prefixText + "%' And IPM.MasterCompanyId=" + MasterCompanyId;
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(strQuery, con);
        da.Fill(ds);
        count = ds.Tables[0].Rows.Count;
        con.Close();
        List<string> ProductCode = new List<string>();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            ProductCode.Add(ds.Tables[0].Rows[i][0].ToString());
        }
        con.Close();
        return ProductCode.ToArray();
    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddlshade, @"SELECT DISTINCT dbo.ShadeColor.ShadecolorId, dbo.ShadeColor.ShadeColorName FROM   dbo.ITEM_PARAMETER_MASTER INNER JOIN
       dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.IFinishedId INNER JOIN
       dbo.ShadeColor ON dbo.ITEM_PARAMETER_MASTER.SHADECOLOR_ID = dbo.ShadeColor.ShadecolorId
       WHERE dbo.IndentDetail.IndentId =" + ddindentno.SelectedValue + " AND dbo.ITEM_PARAMETER_MASTER.ITEM_ID=" + dditemname.SelectedValue + " AND dbo.ITEM_PARAMETER_MASTER.Quality_Id=" + dquality.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"], true, "--Select--");
        Fill_Quantity();
    }
    protected void ddgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Stock_Iss_PendingQty();
        txtissqty.Focus();
    }
    private void refresh_form()
    {
        ViewState["Prtid"] = 0;
        pnl1.Enabled = true;
        ddempname.SelectedValue = null;
        ddProcessName.SelectedValue = null;
        ddindentno.SelectedValue = null;
        ddempname.SelectedValue = null;
        txtchalanno.Text = "";
        ddCatagory.SelectedValue = null;
        dditemname.SelectedValue = null;
        ddgodown.SelectedValue = null;
        txtissue.Text = "";
        dquality.SelectedValue = null;
        dddesign.SelectedValue = null;
        ddcolor.SelectedValue = null;
        ddshape.SelectedValue = null;
        ddsize.SelectedValue = null;
        ddlshade.SelectedValue = null;
        txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        TxtProdCode.Text = "";
        btnsave.Text = "Save";
        lbldate.Visible = false;
        Label1.Visible = false;
        Lblfinished.Visible = false;
    }
    private void Validated()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            string strsql;
            if (btnsave.Text == "Update")
            {
                strsql = "select * from  pp_processrawtran Where Finishedid=" + Varfinishedid + " and Prmid =" + ViewState["Prmid"] + " And Prtid!=" + ViewState["Prtid"] + "";
            }
            else
            {
                strsql = "select * from pp_processrawtran where finishedid=" + Varfinishedid + " and prmid =" + ViewState["Prmid"] + " and indentid=" + ddindentno.SelectedValue;
            }
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Lblfinished.Visible = true;
            }
            else
            {
                Lblfinished.Visible = false;
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void txtissqty_TextChanged(object sender, EventArgs e)
    {
        if (Convert.ToDouble(txtissqty.Text) > Convert.ToDouble(txtpendingqty.Text) || Convert.ToDouble(txtissqty.Text) <= 0)
        {
            lblqty.Visible = true;
            txtissqty.Text = "";
            txtissqty.Focus();
        }
        else
        {
            lblqty.Visible = false;
            btnsave.Focus();
        }
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Session.Remove("finishedid");
        Session.Remove("indentid");
        Session.Remove("indentdetailid");
    }
    protected void ddlotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Stock_Iss_PendingQty();
    }
    protected void ddFinish_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Quantity();
    }
    private void Fill_Quantity()
    {
        int color = 0;
        int quality = 0;
        int design = 0;
        int shape = 0;
        int size = 0;
        int shadeColor = 0;
        if ((ql.Visible == true && dquality.SelectedIndex > 0) || ql.Visible != true)
        {
            quality = 1;
        }
        if (dsn.Visible == true && dddesign.SelectedIndex > 0 || dsn.Visible != true)
        {
            design = 1;
        }
        if (clr.Visible == true && ddcolor.SelectedIndex > 0 || clr.Visible != true)
        {
            color = 1;
        }
        if (shp.Visible == true && ddshape.SelectedIndex > 0 || shp.Visible != true)
        {
            shape = 1;
        }
        if (sz.Visible == true && ddsize.SelectedIndex > 0 || sz.Visible != true)
        {
            size = 1;
        }
        if (shd.Visible == true && ddlshade.SelectedIndex > 0 || shd.Visible != true)
        {
            shadeColor = 1;
        }
        if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
        {

            try
            {
                int finishedid = Convert.ToInt32(UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"])));
                if (finishedid > 0)
                {
                    UtilityModule.ConditionalComboFill(ref ddFinish_Type, @"SELECT OCD.I_FINISHED_TYPE_ID,FT.FINISHED_TYPE_NAME from ORDER_CONSUMPTION_DETAIL OCD,FINISHED_TYPE FT 
                    Where FT.MasterCompanyId=" + Session["varCompanyId"] + " And OCD.I_FINISHED_TYPE_ID=FT.ID AND IFINISHEDID=" + finishedid + " And OCD.PROCESSID=" + ddProcessName.SelectedValue + " And Orderid in (Select Orderid From IndentDetail Where IndentId=" + ddindentno.SelectedValue + ") ORDER BY PCMDID", true, "Finish Type");
                    if (ddFinish_Type.Items.Count > 0)
                    {
                        ddFinish_Type.SelectedIndex = 1;
                    }
                    CommanFunction.FillCombo(ddlunit, @"SELECT DISTINCT U.UNITID,U.UNITNAME FROM IndentDetail ID,UNIT U WHERE U.UNITID=ID.UNITID And ID.IndentId=" + ddindentno.SelectedValue + "");
                    if (ddlunit.Items.Count > 0)
                    {
                        ddlunit.SelectedIndex = 0;
                    }
                    int VARFINISH_TYPE = 0;
                    VARFINISH_TYPE = TdFinish_Type.Visible == true ? Convert.ToInt32(ddFinish_Type.SelectedValue) : 0;
                    UtilityModule.ConditionalComboFill(ref ddlotno, "Select DISTINCT stockid,lotno from stock where item_finished_id=" + finishedid + " and companyid=" + ddCompName.SelectedValue + " AND FINISHED_TYPE_ID=" + VARFINISH_TYPE + " ", true, " Select ");
                    ddlotno.Focus();
                    if (ddlotno.Items.Count > 0)
                    {
                        ddlotno.SelectedIndex = 1;
                        txtissqty.Focus();
                    }
                    Fill_Stock_Iss_PendingQty();
                    Session["FinishedId"] = finishedid;
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/IndentRawIssueDestini");
            }
            finally
            {
                //con.Close();
                //con.Dispose();
            }
        }
        else
        {
            txtpreissue.Text = "";
            txtpendingqty.Text = "";
        }
    }
    protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Quantity();
    }
    protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Quantity();
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Quantity();
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Quantity();
    }
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {

        Fill_Quantity();
    }
    private void Fill_Stock_Iss_PendingQty()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        int finishedid = Convert.ToInt32(UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"])));
        SqlParameter[] _arrpara = new SqlParameter[10];
        _arrpara[0] = new SqlParameter("@COMPANYID", SqlDbType.Int);
        _arrpara[1] = new SqlParameter("@GODOWNID", SqlDbType.Int);
        _arrpara[2] = new SqlParameter("@FINISHEDID", SqlDbType.Int);
        _arrpara[3] = new SqlParameter("@INDENTID", SqlDbType.Int);
        _arrpara[4] = new SqlParameter("@FINISHED_TYPE_ID", SqlDbType.Int);
        _arrpara[5] = new SqlParameter("@LOTNO", SqlDbType.NVarChar, 50);
        _arrpara[6] = new SqlParameter("@STOCKQTY", SqlDbType.Float, 50);
        _arrpara[7] = new SqlParameter("@INDENT", SqlDbType.Float, 50);
        _arrpara[8] = new SqlParameter("@PREISSQTY", SqlDbType.Float, 50);
        _arrpara[9] = new SqlParameter("@PENDING", SqlDbType.Float, 50);

        _arrpara[0].Value = ddCompName.SelectedValue;
        _arrpara[1].Value = ddgodown.SelectedValue;
        _arrpara[2].Value = finishedid;
        _arrpara[3].Value = ddindentno.SelectedValue;
        _arrpara[4].Value = ddFinish_Type.SelectedValue == "" ? "0" : ddFinish_Type.SelectedValue;
        _arrpara[5].Value = ddlotno.SelectedIndex != -1 ? ddlotno.SelectedItem.Text : "Without Lot No";
        _arrpara[6].Direction = ParameterDirection.Output;
        _arrpara[7].Direction = ParameterDirection.Output;
        _arrpara[8].Direction = ParameterDirection.Output;
        _arrpara[9].Direction = ParameterDirection.Output;
        SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_GET_INDENT_ISSUE_STOCK_QTY", _arrpara);
        txtstock.Text = _arrpara[6].Value.ToString();
        txtissue.Text = _arrpara[7].Value.ToString();
        txtpreissue.Text = _arrpara[8].Value.ToString();
        txtpendingqty.Text = _arrpara[9].Value.ToString();
        con.Close();
        con.Dispose();
    }
    private void Show_Report()
    {
        //int VarCompanyId = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
        if (Convert.ToInt16(Session["varCompanyId"]) == 2)
        {
            Session["ReportPath"] = "Reports/IndentRawIssueDestini.rpt";
        }
        else
        {
            Session["ReportPath"] = "Reports/IndentRawIssue.rpt";
        }
        Session["CommanFormula"] = "{V_IndentRawIssue_Destini.PRMid}=" + ViewState["Prmid"] + "";
    }
    protected void DGSHOWDATA_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGSHOWDATA.PageIndex = e.NewPageIndex;
        Fill_Grid_Show();
    }
    private void Fill_Grid_Show()
    {
        DGSHOWDATA.DataSource = "";
        if (ddindentno.SelectedIndex > 0)
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select Distinct IPM.ITEM_FINISHED_ID, ProductCode ,isnull((SUM(id.quantity)),0)*oc.IQTY as indentqty,isnull(sum(IssueQuantity),0)
        From IndentDetail ID inner join ITEM_PARAMETER_MASTER IPM On ID.IFINISHEDID=IPM.ITEM_FINISHED_ID  Inner join 
        ORDER_CONSUMPTION_DETAIL oc On oc.orderid=id.orderid and id.IFINISHEDID=oc.IFINISHEDID and oc.I_FINISHED_Type_ID=id.I_FINISHED_TYPE_ID
        Inner join indentmaster im  On im.indentid=id.indentid and oc.PROCESSID=im.PROCESSID
        left outer join PP_ProcessRawTran pp On id.indentid=pp.indentid and id.IFINISHEDID=pp.Finishedid 
        WHERE    ID.IndentId=" + ddindentno.SelectedValue + "  And IPM.MasterCompanyId=" + Session["varCompanyId"] + @"
        Group by ProductCode,im.indentid,oc.IQTY,IPM.ITEM_FINISHED_ID
        Having isnull((SUM(id.quantity)),0)*oc.IQTY >isnull(sum(IssueQuantity),0)");
            DGSHOWDATA.DataSource = Ds;
            DGSHOWDATA.DataBind();
            if (DGSHOWDATA.Rows.Count > 0)
                DGSHOWDATA.Visible = true;
            else
                DGSHOWDATA.Visible = false;
        }
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            Tdchallan.Visible = true;
            Td6.Visible = false;
            Td7.Visible = true;
            refresh_edit();
        }
        else
        {
            Tdchallan.Visible = false;
            Td6.Visible = true;
            Td7.Visible = false;
            refresh_edit();
            txt_challanno.Text = "";
        }
    }
    protected void txt_challanno_TextChanged(object sender, EventArgs e)
    {
        if (txt_challanno.Text != "")
        {
            DataSet DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT *, replace(convert(varchar(11),Date,106), ' ','-') as date1 FROM PP_ProcessRawMaster 
                WHERE CompanyID = " + ddCompName.SelectedValue + " And Challanno='" + txt_challanno.Text + "' And MasterCompanyId=" + Session["varCompanyId"]);
            if (DS.Tables[0].Rows.Count > 0)
            {
                ddProcessName.SelectedValue = DS.Tables[0].Rows[0]["processid"].ToString();
                txtdate.Text = DS.Tables[0].Rows[0]["DATE1"].ToString();
                txtchalanno.Text = DS.Tables[0].Rows[0]["challanno"].ToString();
                txt_challanno.Text = txtchalanno.Text;
                string Qry = @"SELECT distinct E.EmpId,E.EmpName FROM IndentMaster IM ,  EmpInfo E,PP_ProcessRawMaster pp where IM.PartyId=E.EmpId and pp.empid=e.empid and pp.processid=im.processid And IM.Processid=" + ddProcessName.SelectedValue + " And e.MasterCompanyId=" + Session["varCompanyId"];
                UtilityModule.ConditionalComboFill(ref ddempname, Qry, true, "Select Emp");
                ddempname.SelectedValue = DS.Tables[0].Rows[0]["empid"].ToString();
                Qry = " SELECT distinct IndentId, IndentNo FROM dbo.IndentMaster  where dbo.IndentMaster.indentid in(select distinct indentid from PP_ProcessRawTran ) and dbo.IndentMaster.partyid=" + ddempname.SelectedValue + " and dbo.IndentMaster.processid=" + ddProcessName.SelectedValue + " And CompanyId=" + ddCompName.SelectedValue;
                UtilityModule.ConditionalComboFill(ref ddindentno, Qry, true, "Select Order No");
                DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select distinct indentid,PRMid from PP_ProcessRawTran where prmid=" + DS.Tables[0].Rows[0]["prmid"].ToString() + "");
                ddindentno.SelectedValue = DS.Tables[0].Rows[0][0].ToString();
                ddindent_selectchange();
                EMP_CHANGE();
                TxtProdCode.Focus();
                ViewState["Prmid"] = DS.Tables[0].Rows[0]["PRMid"].ToString();
                Show_Report();
            }
        }
    }
    protected void gvdetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        int n = gvdetail.SelectedIndex;
        ViewState["Prmid"] = ((Label)gvdetail.Rows[n].FindControl("lblid")).Text;

        DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select ChallanNo,replace(convert(varchar(11),Date,106), ' ','-') as Date,
        productcode,GodownId , PT.remark from PP_ProcessRawMaster PM,PP_ProcessRawTran PT,ITEM_PARAMETER_MASTER IPM Where PM.PRMid=PT.PRMid And PT.Finishedid=IPM.ITEM_FINISHED_ID And PT.PRTid=" + gvdetail.SelectedDataKey.Value + " And IPM.MasterCompanyId=" + Session["varCompanyId"] + "");
        if (ds1.Tables[0].Rows.Count > 0)
        {
            ViewState["Prtid"] = gvdetail.SelectedDataKey.Value;
            TxtProdCode.Text = ds1.Tables[0].Rows[0]["productcode"].ToString();
            txt_prod_change();
            ddgodown.SelectedValue = ds1.Tables[0].Rows[0]["GodownId"].ToString();
            Fill_Stock_Iss_PendingQty();
            txtissqty.Text = gvdetail.Rows[n].Cells[6].Text;
            txtstock.Text = Convert.ToString(Convert.ToInt32(txtstock.Text) + Convert.ToInt32(txtissqty.Text));
            txtpreissue.Text = Convert.ToString(Convert.ToInt32(txtpreissue.Text) - Convert.ToInt32(txtissqty.Text));
            txtchalanno.Text = ds1.Tables[0].Rows[0]["ChallanNo"].ToString();
            txt_challanno.Text = txtchalanno.Text;
            txtdate.Text = ds1.Tables[0].Rows[0]["Date"].ToString();
            txtremarks.Text = ds1.Tables[0].Rows[0]["remark"].ToString();
            if (ChkEditOrder.Checked == true)
                txtpendingqty.Text = Convert.ToString(Convert.ToInt16(txtpendingqty.Text) + Convert.ToInt16(txtissqty.Text));
        }
        btnsave.Text = "Update";
    }
    private void refresh_edit()
    {
        TxtProdCode.Text = "";
        txtissqty.Text = "";
        txtpreissue.Text = "";
        txtpendingqty.Text = "";
        txtstock.Text = "";
        txtissue.Text = "";
        DGSHOWDATA.Visible = false;
        gvdetail.Visible = false;
        pnl1.Enabled = true;
        txtchalanno.Text = "";
        //ddCompName.SelectedIndex = 0;
        ddProcessName.SelectedIndex = 0;
        ddempname.Items.Clear();
        txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        ddindentno.Items.Clear();
        btnpriview.Visible = false;
    }
    protected void btnpriview_Click(object sender, EventArgs e)
    {
        Report();
    }
    private void Report()
    {
        DataSet ds = new DataSet();
        int VarCompanyId = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
        if (VarCompanyId != 2)
        {
            Session["ReportPath"] = "Reports/IndentRawIssueNEW.rpt";
            string qry = @" SELECT EmpName,Address,PhoneNo,Mobile,CompanyName,CompanyAddress,ComPanyPhoneNo,CompanyFaxNo,TinNo,Item_Name,QualityName,ChallanNo,Indentid,PRMid,IssueQuantity,LotNo,GodownNAme,ShortName,ShadeColorName
            FROM   V_IndentRawIssue_Destini where V_IndentRawIssue_Destini.PRMid=" + ViewState["Prmid"] + " And CompanyId=" + ddCompName.SelectedValue + " ORDER BY V_IndentRawIssue_Destini.Item_Name ";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            string str = @"SELECT IssQty,Quality_Id,QualityName,PRMid  FROM   indentRawIssue  ORDER BY Quality_Id";
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            SqlDataAdapter sda = new SqlDataAdapter(str, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ds.Tables.Add(dt);
            Session["dsFileName"] = "~\\ReportSchema\\IndentRawIssueNEW.xsd";
        }
        else
        {
            Session["ReportPath"] = "Reports/IndentRawIssueDestiniNEW.rpt";
            string qry = @"  SELECT V_IndentRawIssue_Destini.EmpName,V_IndentRawIssue_Destini.Address,V_IndentRawIssue_Destini.PhoneNo,V_IndentRawIssue_Destini.Mobile,V_IndentRawIssue_Destini.CompanyName,V_IndentRawIssue_Destini.CompanyAddress,V_IndentRawIssue_Destini.ComPanyPhoneNo,
            V_IndentRawIssue_Destini.CompanyFaxNo,V_IndentRawIssue_Destini.TinNo,V_IndentRawIssue_Destini.Item_Name,V_IndentRawIssue_Destini.QualityName,V_IndentRawIssue_Destini.PRMid,V_IndentRawIssue_Destini.IssueQuantity,V_IndentRawIssue_Destini.GodownNAme,ITEM_PARAMETER_MASTER.ProductCode,
            V_IndentRawIssue_Destini.IndentNo,V_IndentRawIssue_Destini.Category_Name,V_IndentRawIssue_Destini.DesignName,V_IndentRawIssue_Destini.ColorName,PROCESS_NAME_MASTER.PROCESS_NAME,V_IndentRawIssue_Destini.Indentid,V_IndentRawIssue_Destini.GatepassNo,FINISHED_TYPE.FINISHED_TYPE_NAME,
            V_IndentRawIssue_Destini.localorder,V_IndentRawIssue_Destini.customerorderno,V_IndentRawIssue_Destini.reqdate,V_IndentRawIssue_Destini.remark,V_IndentRawIssue_Destini.Issuedate
            FROM   V_IndentRawIssue_Destini LEFT OUTER JOIN FINISHED_TYPE ON V_IndentRawIssue_Destini.Finish_Type_Id=FINISHED_TYPE.Id INNER JOIN ITEM_PARAMETER_MASTER ON V_IndentRawIssue_Destini.FinishedId=ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID
            INNER JOIN PROCESS_NAME_MASTER ON V_IndentRawIssue_Destini.processid=PROCESS_NAME_MASTER.PROCESS_NAME_ID
            Where V_IndentRawIssue_Destini.PRMid=" + ViewState["Prmid"] + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            Session["dsFileName"] = "~\\ReportSchema\\IndentRawIssueDestiniNEW.xsd";
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    protected void gvdetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    protected void DGSHOWDATA_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }

    protected void DGSHOWDATA_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtProdCode.Text = DGSHOWDATA.Rows[DGSHOWDATA.SelectedIndex].Cells[0].Text;
        txt_prod_change();
    }
    protected void DGSHOWDATA_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGSHOWDATA, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[4];
            arr[0] = new SqlParameter("@Prtid", SqlDbType.Int);
            arr[1] = new SqlParameter("@TableName", SqlDbType.NVarChar, 50);
            arr[2] = new SqlParameter("@Count", SqlDbType.Int);
            arr[3] = new SqlParameter("@FlagDeleteOrNot", SqlDbType.Int);

            arr[0].Value = gvdetail.DataKeys[e.RowIndex].Value;
            arr[1].Value = "PP_ProcessRawTran";
            arr[2].Value = gvdetail.Rows.Count;
            arr[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_UpdateStockIssueDelete", arr);
            tran.Commit();
            //save_refresh();
            if (gvdetail.Rows.Count == 1)
            {
                ViewState["Prmid"] = 0;
                ViewState["Prtid"] = 0;
                txtchalanno.Text = "";
                ddgodown.SelectedIndex = 0;
            }
            if (Convert.ToInt32(arr[3].Value) == 1)
            {
                Label1.Text = "AllReady Received Data....";
                Label1.Visible = true;
            }
            else
            {
                Fill_Grid();
                Fill_Grid_Show();
            }
        }
        catch (Exception ex)
        {
            Label1.Text = ex.Message;
            Label1.Visible = true;
            tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strsql = @"SELECT prt.PRTid as prtid,prm.prmid as prmid,prt.LotNo, icm.CATEGORY_NAME, im.ITEM_NAME, gm.GodownName, 
            prt.IssueQuantity,IPM1.QDCS + Space(2)+isnull(SizeMtr,'')+ft.finished_type_name DESCRIPTION,prt.remark  FROM  
            pp_ProcessRawTran prt  INNER JOIN ITEM_PARAMETER_MASTER ipm ON prt.Finishedid = ipm.ITEM_FINISHED_ID INNER JOIN
            GodownMaster gm ON prt.Godownid = gm.GoDownID INNER JOIN
            item_master im on im.item_id=ipm.item_id inner join
            iTEM_CATEGORY_MASTER icm ON im.Category_id = icm.CATEGORY_ID inner join
            pp_ProcessRawMaster prm on  prt.prmid=prm.prmid inner Join
            ViewFindFinishedidItemidQDCSS IPM1 on IPM.Item_Finished_Id=IPM1.Finishedid inner join
            FINISHED_TYPE ft on ft.id=prt.finish_type_id
            where prm.challanno=" + DDChallanNo.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"];
        Session["CommanFormula"] = "{V_IndentRawIssue_Destini.challanno}=" + DDChallanNo.SelectedValue + "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        gvdetail.DataSource = ds;
        gvdetail.DataBind();
        if (gvdetail.Rows.Count > 0)
        {
            gvdetail.Visible = true;
        }
        else
        {
            gvdetail.Visible = false;
        }
    }
}