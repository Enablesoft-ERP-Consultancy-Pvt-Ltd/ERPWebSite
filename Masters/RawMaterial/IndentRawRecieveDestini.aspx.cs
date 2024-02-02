using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_RawMaterial_IndentRawRecieveDestini : System.Web.UI.Page
{
    int ItemFinishedId = 0;
    static int MasterCompanyID;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyID = Convert.ToInt16(Session["varMasterCompanyIDForERP"]);
        if (!IsPostBack)
        {
            if (Session["varMasterCompanyIDForERP"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            ViewState["masterid"] = 0;
            txtprmid.Text = "0";
            TXTPRTID.Text = "0";
            UtilityModule.ConditionalComboFill(ref ddCompName, "select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By Companyname", true, "Select Comp Name");
            UtilityModule.ConditionalComboFill(ref ddProcessName, " Select Distinct PROCESS_NAME_ID,process_name from PROCESS_NAME_MASTER pm inner join IndentMaster im on pm.PROCESS_NAME_ID=im.processid And pm.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by PROCESS_NAME_ID", true, "Select Process Name");
            UtilityModule.ConditionalComboFill(ref ddgodown, "Select godownid,godownname from godownmaster Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "Select GodownName");
            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }
            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            lablechange();
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
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        LblQuality.Text = ParameterList[0];
        LblDesign.Text = ParameterList[1];
        LblColor.Text = ParameterList[2];
        LblShape.Text = ParameterList[3];
        LblSize.Text = ParameterList[4];
        LblCategory.Text = ParameterList[5];
        LblItemName.Text = ParameterList[6];
        LblColorShade.Text = ParameterList[7];
    }
    protected void ddProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = "";
        if (ChkEditOrder.Checked == true)
        {
            Str = @"select distinct IM.IndentId,IndentNo+' ('+om.localorder+'   '+customerorderno+')'
                from IndentMaster IM inner join 
                PP_ProcessRecTran PRT  on PRT.IndentId=IM.IndentId inner join 
                PP_ProcessRecMaster PRM ON PRM.PRMID=PRT.PRMID inner join
                indentdetail id On id.indentid=im.indentid inner join
                ordermaster om on om.orderid=id.orderid where PRM.ProcessId=" + ddProcessName.SelectedValue + "  And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And IM.CompanyId=" + ddCompName.SelectedValue;
            //            UtilityModule.ConditionalComboFill(ref ddindent, @"select distinct IM.IndentId,IndentNo+' ('+om.localorder+'   '+customerorderno+')'
            //                                                             from IndentMaster IM inner join 
            //                                                             PP_ProcessRecTran PRT  on PRT.IndentId=IM.IndentId inner join 
            //                                                             PP_ProcessRecMaster PRM ON PRM.PRMID=PRT.PRMID inner join
            //                                                             indentdetail id On id.indentid=im.indentid inner join
            //                                                             ordermaster om on om.orderid=id.orderid where PRM.ProcessId=" + ddProcessName.SelectedValue + "  And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And IM.CompanyId=" + ddCompName.SelectedValue, true, "Select Indent No");
        }
        else
        {
            Str = @"select distinct IM.IndentId,IndentNo+' ('+om.localorder+'   '+customerorderno+')'
                    from IndentMaster IM inner join 
                    PP_ProcessRawTran PRT  on PRT.IndentId=IM.IndentId inner join 
                    PP_ProcessRawMaster PRM ON PRM.PRMID=PRT.PRMID inner join
                    indentdetail id On id.indentid=im.indentid inner join
                    ordermaster om on om.orderid=id.orderid where PRM.ProcessId=" + ddProcessName.SelectedValue + "  and  IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And IM.CompanyId=" + ddCompName.SelectedValue;
            //            UtilityModule.ConditionalComboFill(ref ddindent, @"select distinct IM.IndentId,IndentNo+' ('+om.localorder+'   '+customerorderno+')'
            //                                                             from IndentMaster IM inner join 
            //                                                             PP_ProcessRawTran PRT  on PRT.IndentId=IM.IndentId inner join 
            //                                                             PP_ProcessRawMaster PRM ON PRM.PRMID=PRT.PRMID inner join
            //                                                             indentdetail id On id.indentid=im.indentid inner join
            //                                                             ordermaster om on om.orderid=id.orderid where PRM.ProcessId=" + ddProcessName.SelectedValue + "  and  IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And IM.CompanyId=" + ddCompName.SelectedValue, true, "Select Indent No");
        }
        if (txtcode.Text != "")
        {
            Str = Str + "  AND PRT.Finishedid in (select ITEM_FINISHED_ID from ITEM_PARAMETER_MASTER where ProductCode ='" + txtcode.Text.Trim() + "')";
        }
        UtilityModule.ConditionalComboFill(ref ddindent, Str, true, "Select Indent No");
        ddempname.Focus();
    }
    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddProcessName.SelectedValue != "9")
        {
            ddindent_selectchange();
        }
        else
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select ifinishedid from v_IndentissueAndConsumption where indentid=" + ddindent.SelectedValue + "  and issueqty=0 AND PROCESSID=9");
            if (ds.Tables[0].Rows.Count == 0)
            {
                ddindent_selectchange();
                lblqty.Visible = false;
                DGSHOWDATA.Visible = true;
                lblqty.Text = "";
            }
            else
            {
                ddCatagory.Items.Clear();
                DGSHOWDATA.Visible = false;
                lblqty.Visible = true;
                lblqty.Text = "First Issue All Items";
            }
        }
        txtdate.Focus();
    }
    protected void ddindent_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
            UtilityModule.ConditionalComboFill(ref ddempname, "SELECT distinct E.EmpId,E.EmpName FROM EmpInfo E,PP_ProcessRecMaster ppm ,PP_ProcessRectran ppt where ppm.empid=e.empid and ppm.PRMid=ppt.PRMid and ppt.indentid=" + ddindent.SelectedValue + "  And ppm.Processid=" + ddProcessName.SelectedValue + " And E.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And ppm.CompanyId=" + ddCompName.SelectedValue, true, "Select Emp");
        else
            UtilityModule.ConditionalComboFill(ref ddempname, "SELECT distinct E.EmpId,E.EmpName FROM IndentMaster IM INNER JOIN  EmpInfo E ON IM.PartyId=E.EmpId And IM.Processid=" + ddProcessName.SelectedValue + " and im.indentid=" + ddindent.SelectedValue + " And E.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And IM.CompanyId=" + ddCompName.SelectedValue, true, "Select Emp");

    }
    private void ddindent_selectchange()
    {
        if (ChkEditOrder.Checked == true)
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select PRMID From PP_ProcessRecTran  where indentid=" + ddindent.SelectedValue + "");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["masterid"] = ds.Tables[0].Rows[0][0].ToString();
                // Fill_Grid();
                string str = @"Select Distinct PPM.PrmId,PPM.ChallanNo from PP_ProcessRecMaster PPM,PP_ProcessRecTran PPT Where 
                                PPM.PrmId=PPT.PrmId And PPM.CompanyID=" + ddCompName.SelectedValue + "And PPM.ProcessID=" + ddProcessName.SelectedValue + " And PPm.EmpId=" + ddempname.SelectedValue + " And PPT.IndentID=" + ddindent.SelectedValue;
                UtilityModule.ConditionalComboFill(ref DDChallanNo, str, true, "--Select--");

            }
        }
        UtilityModule.ConditionalComboFill(ref ddCatagory, @"SELECT DISTINCT dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID AS Expr1, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME
                      FROM  dbo.ITEM_MASTER INNER JOIN dbo.ITEM_PARAMETER_MASTER ON dbo.ITEM_MASTER.ITEM_ID = dbo.ITEM_PARAMETER_MASTER.ITEM_ID INNER JOIN
                      dbo.ITEM_CATEGORY_MASTER ON dbo.ITEM_MASTER.CATEGORY_ID = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID INNER JOIN
                      dbo.IndentDetail INNER JOIN dbo.IndentMaster ON dbo.IndentDetail.IndentId = dbo.IndentMaster.IndentID ON 
                      dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.OFinishedId where dbo.IndentMaster.partyid=" + ddempname.SelectedValue + " and IndentMaster.processid=" + ddProcessName.SelectedValue + " and  dbo.Indentmaster.IndentId=" + ddindent.SelectedValue + " And ITEM_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Category Name");
        Fill_Grid_Show();
    }
    private void ddlcategorycange()
    {
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
                      " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCatagory.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
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
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.OFinishedId INNER JOIN
                      dbo.Design ON dbo.ITEM_PARAMETER_MASTER.DESIGN_ID = dbo.Design.designId
                       WHERE  dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select Design--");
                        break;

                    case "3":
                        clr.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddcolor, @"SELECT DISTINCT dbo.color.ColorId, dbo.color.ColorName FROm dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.OFinishedId INNER JOIN
                      dbo.color ON dbo.ITEM_PARAMETER_MASTER.COLOR_ID = dbo.color.ColorId
                      WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select Color--");
                        break;

                    case "4":
                        shp.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddshape, @"SELECT DISTINCT dbo.Shape.ShapeId, dbo.Shape.ShapeName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.OFinishedId INNER JOIN
                      dbo.Shape ON dbo.ITEM_PARAMETER_MASTER.SHAPE_ID = dbo.Shape.ShapeId
                      WHERE dbo.IndentDetail.IndentId = " + ddindent.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select Shape--");
                        break;
                    case "5":
                        sz.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size.SizeFt FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.OFinishedId INNER JOIN
                      dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
                       WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Size in Ft");
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
        txtcode.Text = "";
        UtilityModule.ConditionalComboFill(ref ddlunit, "SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + dditemname.SelectedValue + " And i.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Unit");
        ddlunit.SelectedIndex = 1;
        ddlshade.Items.Clear();
        UtilityModule.ConditionalComboFill(ref dquality, @"SELECT DISTINCT dbo.Quality.QualityId,dbo.Quality.QualityName
                     FROM  dbo.ITEM_PARAMETER_MASTER INNER JOIN
                      dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.OFinishedId INNER JOIN
                      dbo.Quality ON dbo.ITEM_PARAMETER_MASTER.QUALITY_ID = dbo.Quality.QualityId
                     WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + "and quality.item_id=" + dditemname.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Quallity");

        UtilityModule.ConditionalComboFill(ref dddesign, @"Select Distinct Designid,DesignName   From Item_Parameter_Master IM,Design D,pp_ProcessRawTran PT
                                                         Where IM.Item_Finished_id=PT.Finishedid And IM.Design_ID=D.Designid And PT.PRMID=" + ddindent.SelectedValue + " and pt.itemid=" + dditemname.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Design");
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dditemname, @"SELECT DISTINCT dbo.ITEM_MASTER.ITEM_ID, dbo.ITEM_MASTER.ITEM_NAME FROM  dbo.ITEM_PARAMETER_MASTER INNER JOIN
        dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.OFinishedId INNER JOIN
        dbo.ITEM_MASTER ON dbo.ITEM_PARAMETER_MASTER.ITEM_ID = dbo.ITEM_MASTER.ITEM_ID
        WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " AND dbo.ITEM_MASTER.CATEGORY_ID =" + ddCatagory.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select Item--");
        ddlcategorycange();
    }
    protected void save_detail(int Flag)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

        ViewState["PRMid"] = 0;
        ViewState["PRTid"] = 0;
        try
        {
            SqlParameter[] arr = new SqlParameter[20];

            arr[0] = new SqlParameter("@prmid", SqlDbType.Int);
            arr[1] = new SqlParameter("@companyid", SqlDbType.Int);
            arr[2] = new SqlParameter("@empid", SqlDbType.Int);
            arr[3] = new SqlParameter("@processid", SqlDbType.Int);
            arr[4] = new SqlParameter("@issuedate", SqlDbType.DateTime);
            arr[5] = new SqlParameter("@prtid", SqlDbType.Int);
            arr[6] = new SqlParameter("@finishedId", SqlDbType.Int);
            arr[7] = new SqlParameter("@godownId", SqlDbType.Int);
            arr[8] = new SqlParameter("@RecQuantity", SqlDbType.Float);
            arr[9] = new SqlParameter("@indentid", SqlDbType.Int);
            arr[10] = new SqlParameter("@UnitId", SqlDbType.Int);
            arr[11] = new SqlParameter("@IssPrmID", SqlDbType.Int);
            arr[12] = new SqlParameter("@IssPrtID", SqlDbType.Int);
            arr[13] = new SqlParameter("@ID", SqlDbType.Int);
            arr[14] = new SqlParameter("@Flag", SqlDbType.Int);
            arr[15] = new SqlParameter("@lotno", SqlDbType.NVarChar, 50);
            arr[16] = new SqlParameter("@challanno", SqlDbType.Int);
            arr[17] = new SqlParameter("@retqty", SqlDbType.Int);
            arr[18] = new SqlParameter("@OrderId", SqlDbType.Int);
            arr[19] = new SqlParameter("@Finish_Type_Id", SqlDbType.Int);

            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = txtprmid.Text;
            arr[1].Value = ddCompName.SelectedValue;
            arr[2].Value = ddempname.SelectedValue;
            arr[3].Value = ddProcessName.SelectedValue;
            arr[4].Value = txtdate.Text;

            arr[5].Value = Convert.ToInt32(TXTPRTID.Text);
            ItemFinishedId = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, txtcode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            arr[6].Value = ItemFinishedId;
            arr[7].Value = Convert.ToInt32(ddgodown.SelectedValue);
            arr[8].Value = txtrec.Text;
            arr[9].Value = ddindent.SelectedValue;
            arr[10].Value = ddlunit.SelectedValue;
            arr[11].Value = Convert.ToInt32(ViewState["PRMid"].ToString());
            arr[12].Value = Convert.ToInt32(ViewState["PRTid"].ToString());
            arr[13].Direction = ParameterDirection.Output;
            arr[14].Value = Flag;
            if (txtlotno.Text == "")
            {
                arr[15].Value = "Without Lot No";
            }
            else
            {
                arr[15].Value = txtlotno.Text;
            }

            arr[16].Direction = ParameterDirection.InputOutput;
            if (ChkEditOrder.Checked == true)
            {
                arr[16].Value = txtidnt.Text;
            }
            if (txtretrn.Text != "")
            {
                arr[17].Value = txtretrn.Text;
            }
            else { arr[17].Value = "0"; }
            arr[18].Value = 0;
            arr[19].Value = TdFinish_Type.Visible == true ? ddFinish_Type.SelectedValue : "0";
            con.Open();
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "[PRO_PP_PRM_recieve_destini]", arr);
            txtindent.Text = arr[13].Value.ToString();
            ViewState["masterid"] = Convert.ToInt32(arr[0].Value.ToString());
            txtchallan.Text = arr[16].Value.ToString();
        }
        catch
        {

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void validate()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, txtcode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            string str = null;
            if (btnsave.Text == "Update")
            {

                str = "Select isnull(FinishedId,0) From PP_ProcessRecTran where PRMid=" + ViewState["masterid"] + " and FinishedId=" + Varfinishedid + " and PRtid !=" + TXTPRTID.Text;
            }
            else
            {
                str = "Select isnull(FinishedId,0) From PP_ProcessRecTran where PRMid=" + ViewState["masterid"] + " and FinishedId=" + Varfinishedid;
            }
            int a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, str));

            if (a != 0)
            {
                lblind.Visible = true;
                lblind.Text = "Duplicate Entry..........";
            }
            else
            {
                lblind.Visible = false;
                lblind.Text = "";
            }
        }

        catch
        {
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (ddProcessName.SelectedValue == "9")
        {
            ItemFinishedId = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, txtcode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @" select ifinishedid,(sum(Recqty)+10)*ConQty as rec,sum(issueqty),OFINISHEDID,O_FINISHED_Type_ID from v_IndentissueAndConsumption
           Where indentid=" + ddindent.SelectedValue + " and OFINISHEDID=" + ItemFinishedId + " and O_FINISHED_Type_ID=" + ddFinish_Type.SelectedValue + " group by ifinishedid,OFINISHEDID,O_FINISHED_Type_ID,ConQty having (sum(Recqty)+10)*ConQty >sum(issueqty)");
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblqty.Text = "Recieve Qty Is Greater Then Issue Qty";
                lblqty.Visible = true;
            }
            else
            {
                lblqty.Text = "";
                lblqty.Visible = false;
            }
        }
        ViewState["masterid"] = 0;
        validate();
        if (lblqty.Visible == false && lblind.Visible == false)
        {
            //if (pnl1.Enabled)
            //{
            //    int Flag = 1;
            //    save_detail(Flag);
            //}
            //else
            //{
            //    int Flag;
            //    if (ChkEditOrder.Checked == true)
            //        Flag = 1;
            //    else
            //        Flag = 0;
            //    save_detail(Flag);
            //}

            if (pnl1.Enabled || btnsave.Text == "Update")
            {
                int Flag = 1;
                save_detail(Flag);
            }
            else
            {
                int Flag;
                if (ChkEditOrder.Checked == true)
                    Flag = 1;
                else
                    Flag = 0;
                save_detail(Flag);
            }
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            string str = "select max(gatepassno) from PP_processrecmaster";
            txtgatepass.Text = SqlHelper.ExecuteScalar(con, CommandType.Text, str).ToString();

            SqlTransaction Tran = con.BeginTransaction();
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, txtcode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            int prt = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select isnull(max(PRTid),0) from PP_ProcessRecTran"));
            try
            {
                if (btnsave.Text == "Update")
                {
                    string delete = "delete from stocktran where tablename='processrawtran'and prtid=" + Convert.ToInt32(TXTPRTID.Text);
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, delete);

                    UtilityModule.StockStockTranTableUpdate2(Varfinishedid, Convert.ToInt32(ddgodown.SelectedValue), Convert.ToInt32(ddCompName.SelectedValue), txtlotno.Text.ToString(), Convert.ToDouble(txtrec.Text), txtdate.Text.ToString(), txtdate.Text.ToString(), "pp_processrectran", prt, Tran, 1, true, 1, true, Convert.ToDouble(Session["issueqty"]), Convert.ToInt32(ddFinish_Type.SelectedValue));
                }
                else
                {

                    UtilityModule.StockStockTranTableUpdate(Varfinishedid, Convert.ToInt32(ddgodown.SelectedValue), Convert.ToInt32(ddCompName.SelectedValue), txtlotno.Text.ToString(), Convert.ToDouble(txtrec.Text), txtdate.Text.ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), "pp_processrectran", prt, Tran, 1, true, 1, Convert.ToInt32(ddFinish_Type.SelectedValue));

                }
                Tran.Commit();
                Show_Report();
                pnl1.Enabled = false;
                txtcode.Text = "";
                //ddCatagory.SelectedValue = null;
                //dditemname.SelectedValue = null;
                txtissue.Text = "";
                txtrecqty.Text = "";
                //    txtlotno.Text = "";
                txtlotno.Enabled = false;
                txtprerec.Text = "";
                txtpending.Text = "";
                txtrec.Text = "";
                dquality.SelectedValue = null;
                dddesign.SelectedValue = null;
                ddcolor.SelectedValue = null;
                ddshape.SelectedValue = null;
                ddsize.SelectedValue = null;
                btnsave.Text = "Save";
                btndelete.Visible = false;
                ddlshade.SelectedValue = null;
                if (ChkEditOrder.Checked == false)
                    txtidnt.Enabled = false;
                Fill_Grid();
            }
            catch
            {
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    private void Fill_Grid()
    {
        gvdetail.DataSource = fill_Data_grid();
        gvdetail.DataBind();
        if (gvdetail.Rows.Count > 0)
            gvdetail.Visible = true;
        else
            gvdetail.Visible = false;
    }
    private DataSet fill_Data_grid()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            //            string strsql = @"SELECT ICM.CATEGORY_NAME,IM.ITEM_NAME,VF.QDCS+Space(2)+FT.Finished_Type_Name as Description,GM.GodownName, PPRT.RecQuantity,PPRT.PRTid as PRTID,PPRT.PRMid as PRMID
            //            FROM dbo.ITEM_MASTER IM,ITEM_PARAMETER_MASTER IPM,ITEM_CATEGORY_MASTER ICM,GodownMaster GM,PP_ProcessRecTran PPRT,ViewFindFinishedidItemidQDCSS VF,
            //            FINISHED_TYPE FT WHERE IM.ITEM_ID=IPM.ITEM_ID AND IM.CATEGORY_ID=ICM.CATEGORY_ID And GM.GoDownID=PPRT.Godownid AND IPM.ITEM_FINISHED_ID=PPRT.Finishedid AND 
            //            IPM.Item_Finished_ID=VF.Finishedid And FT.ID=PPRT.Finish_Type And PPRT.prmid=" + ViewState["masterid"] + " And IPM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

            string strsql = @"SELECT ICM.CATEGORY_NAME,IM.ITEM_NAME,VF.QDCS+Space(2)+FT.Finished_Type_Name as Description,GM.GodownName, PPRT.RecQuantity,PPRT.PRTid as PRTID,PPRT.PRMid as PRMID
            FROM dbo.ITEM_MASTER IM,ITEM_PARAMETER_MASTER IPM,ITEM_CATEGORY_MASTER ICM,GodownMaster GM,PP_ProcessRecTran PPRT,ViewFindFinishedidItemidQDCSS VF,
            FINISHED_TYPE FT, PP_ProcessRecMaster PPM WHERE IM.ITEM_ID=IPM.ITEM_ID AND IM.CATEGORY_ID=ICM.CATEGORY_ID And GM.GoDownID=PPRT.Godownid AND IPM.ITEM_FINISHED_ID=PPRT.Finishedid AND 
            IPM.Item_Finished_ID=VF.Finishedid And FT.ID=PPRT.Finish_Type AND PPM.prmid=PPRT.prmid And PPM.PRMid=" + ViewState["masterid"] + " And IPM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_Rawmeterial_rawRecieve|fill_Data_grid|" + ex.Message);
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
    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtidnt.Enabled = true;
        txtidnt.Text = "";
        pnl1.Enabled = true;
        txtprmid.Text = "0";
        TXTPRTID.Text = "0";
        ddProcessName.SelectedValue = null;
        ddempname.SelectedValue = null;
        ddindent.SelectedValue = null;
        txtcode.Text = "";

        ddCatagory.SelectedValue = null;
        dditemname.SelectedValue = null;
        ddgodown.SelectedValue = null;
        txtissue.Text = "";
        txtrecqty.Text = "";
        dquality.SelectedValue = null;
        dddesign.SelectedValue = null;
        ddcolor.SelectedValue = null;
        ddshape.SelectedValue = null;
        ddsize.SelectedValue = null;
        ddlshade.SelectedValue = null;
        txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        btnsave.Text = "Save";
        lblmsg.Visible = false;
        lblqty.Visible = false;
        Label2.Visible = false;
        ////////////
        ddCatagory.Enabled = true;
        dditemname.Enabled = true;
        dquality.Enabled = true;
        dddesign.Enabled = true;
        ddcolor.Enabled = true;
        ddshape.Enabled = true;
        ddsize.Enabled = true;
        ddlshade.Enabled = true;
        ddgodown.Enabled = true;
        ddlunit.Enabled = true;
        ddCompName.Enabled = true;
        ddProcessName.Enabled = true;
        ddempname.Enabled = true;
        ddindent.Enabled = true;
        txtdate.Enabled = true;
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
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Session.Remove("issueqty");
        Session.Remove("inhand");
        Session.Remove("finishedid");
    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_qty();
        UtilityModule.ConditionalComboFill(ref ddlshade, @"SELECT DISTINCT dbo.ShadeColor.ShadecolorId, dbo.ShadeColor.ShadeColorName FROM   dbo.ITEM_PARAMETER_MASTER INNER JOIN
                         dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.OFinishedId INNER JOIN  dbo.ShadeColor ON dbo.ITEM_PARAMETER_MASTER.SHADECOLOR_ID = dbo.ShadeColor.ShadecolorId
                         WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " and dbo.ITEM_PARAMETER_MASTER.Item_Id=" + dditemname.SelectedValue + " and dbo.ITEM_PARAMETER_MASTER.Quality_Id=" + dquality.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select ShadeColor");
        fill_Order_qty();
    }
    private void fill_qty()
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
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, txtcode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            UtilityModule.ConditionalComboFill(ref ddFinish_Type, @"SELECT Distinct ID.O_FINISHED_TYPE_ID,FT.FINISHED_TYPE_NAME from IndentDetail ID,FINISHED_TYPE FT 
                    Where ID.O_FINISHED_TYPE_ID=FT.ID AND OFINISHEDID=" + Varfinishedid + " And IndentId=" + ddindent.SelectedValue + "", true, "Finish Type");
            if (ddFinish_Type.Items.Count > 0)
            {
                ddFinish_Type.SelectedIndex = 1;
            }
        }
        else
        {
            txtrec.Text = "";
            txtprerec.Text = "";
        }
    }
    private void fill_Order_qty()
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
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, txtcode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            DataSet ds;
            string Str;
            if (ddProcessName.SelectedValue == "9")
                Str = @"SELECT  IsNull(SUM(Quantity),0) as IssueQuantity FROM VIEW_INDENTRECQTY Where OFinishedId=" + Varfinishedid + "  And Indentid=" + ddindent.SelectedValue;
            else
            {
                if (Session["varMasterCompanyIDForERP"].ToString() == "2")
                {
                    Str = "select isnull(sum(issueQuantity),0) as issuequantity from V_IndentRawIssue_Destini where indentid=" + ddindent.SelectedValue + " and finishedid=" + Varfinishedid + " ";
                    //Str = @"SELECT  IsNull(SUM(Quantity),0) as IssueQuantity FROM VIEW_INDENTRECQTY Where OFinishedId=" + Varfinishedid + "  And Indentid=" + ddindent.SelectedValue;
                }
                else
                {
                    Str = "select isnull(sum(issueQuantity),0) as issuequantity from V_IndentRawIssue where indentid=" + ddindent.SelectedValue + " and finishedid=" + Varfinishedid + " ";
                }
            }
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtrecqty.Text = (ds.Tables[0].Rows[0]["issuequantity"].ToString());
            }
            else
            {
                txtrecqty.Text = "0";
            }
            DataSet ds2;
            string Str2;
            Str2 = @"SELECT   isnull(SUM(recquantity),0) as recquantity FROM PP_ProcessRectran  where finishedid=" + Varfinishedid + " and indentid=" + ddindent.SelectedValue;
            ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str2);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                txtprerec.Text = (ds2.Tables[0].Rows[0]["recquantity"].ToString());
            }
            else
            {
                txtprerec.Text = "0";
            }
            Double pending = Convert.ToDouble(txtrecqty.Text) - Convert.ToDouble(txtprerec.Text);
            txtpending.Text = pending.ToString();
            txtlotno.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select LotNo from IndentDetail Where IndentId=" + ddindent.SelectedValue + " And O_Finished_Type_id=" + ddFinish_Type.SelectedValue).ToString();
        }
        else
        {
            txtrec.Text = "";
            txtprerec.Text = "";
        }

    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string dlttran = "delete from pp_processrectran where prtid=" + Convert.ToInt32(TXTPRTID.Text);
        SqlHelper.ExecuteNonQuery(con, CommandType.Text, dlttran);
        string dltmastr = "delete from pp_processrecmaster where prmid=" + Convert.ToInt32(txtprmid.Text);
        SqlHelper.ExecuteNonQuery(con, CommandType.Text, dltmastr);
        Fill_Grid();
    }
    protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_qty();
    }
    protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_qty();
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_qty();
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_qty();
    }
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_qty();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER IPM inner join item_Master IM on IM.Item_Id=IPM.Item_Id inner join CategorySeparate CS on CS.CategoryId=IM.Category_Id  where ProductCode Like  '" + prefixText + "%' And IPM.MasterCompanyId=" + MasterCompanyID;
        //string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER  where ProductCode Like  '" + prefixText + "%'";
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
    protected void txtcode_TextChanged(object sender, EventArgs e)
    {
        if (ddindent.SelectedIndex > 0)
        {
            txtcode_change();
        }
    }
    private void txtcode_change()
    {
        DataSet ds;
        string Str;
        if (txtcode.Text != "")
        {
            Str = "SELECT DISTINCT IM.Category_Id,IPM.* FROM  ITEM_PARAMETER_MASTER IPM INNER JOIN IndentDetail ID ON IPM.ITEM_FINISHED_ID = ID.OFinishedId inner join Item_Master IM on IM.Item_Id=IPM.Item_Id  WHERE ID.IndentId = " + ddindent.SelectedValue + " and  ProductCode='" + txtcode.Text + "' And IPM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddCatagory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                ddlcategorycange();
                UtilityModule.ConditionalComboFill(ref dditemname, @"SELECT DISTINCT dbo.ITEM_MASTER.ITEM_ID, dbo.ITEM_MASTER.ITEM_NAME FROM  dbo.ITEM_PARAMETER_MASTER INNER JOIN dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.OFinishedId INNER JOIN dbo.ITEM_MASTER ON dbo.ITEM_PARAMETER_MASTER.ITEM_ID = dbo.ITEM_MASTER.ITEM_ID
                      WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " AND dbo.ITEM_MASTER.CATEGORY_ID =" + ddCatagory.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--Select Item--");
                dditemname.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref ddlunit, "SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + dditemname.SelectedValue + " And i.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Unit");
                ddlunit.SelectedIndex = 1;
                ddlshade.Items.Clear();
                UtilityModule.ConditionalComboFill(ref dquality, @"SELECT DISTINCT dbo.Quality.QualityId,dbo.Quality.QualityName FROM  dbo.ITEM_PARAMETER_MASTER INNER JOIN dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.OFinishedId INNER JOIN
                     dbo.Quality ON dbo.ITEM_PARAMETER_MASTER.QUALITY_ID = dbo.Quality.QualityId
                     WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + "and quality.item_id=" + dditemname.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Quallity");
                //UtilityModule.ConditionalComboFill(ref dddesign, @"Select Distinct Designid,DesignName   From Item_Parameter_Master IM,Design D,pp_ProcessRawTran PT Where IM.Item_Finished_id=PT.Finishedid And IM.Design_ID=D.Designid And PT.PRMID=" + ddindent.SelectedValue + " and pt.itemid=" + dditemname.SelectedValue, true, "Select Design");
                UtilityModule.ConditionalComboFill(ref dquality, "select qualityid,qualityname from quality where item_id=" + dditemname.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Quallity");
                dquality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                string str = "Select Distinct Designid,DesignName   From Item_Parameter_Master IM,Design D,pp_ProcessRawTran PT Where IM.Item_Finished_id=PT.Finishedid And IM.Design_ID=D.Designid";
                UtilityModule.ConditionalComboFill(ref dddesign, str, true, "--Select Design--");
                dddesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                fill_qty();
                UtilityModule.ConditionalComboFill(ref ddcolor, "SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by colorid", true, "--Select Color--");
                ddcolor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref ddshape, "select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Shapeid", true, "--Select Shape--");
                ddshape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref ddsize, "SELECT SIZEID,SIZEFT fROM SIZE WhERE SHAPEID=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "--SELECT SIZE--");
                ddsize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref ddlshade, @"SELECT DISTINCT dbo.ShadeColor.ShadecolorId, dbo.ShadeColor.ShadeColorName FROM   dbo.ITEM_PARAMETER_MASTER INNER JOIN
                dbo.IndentDetail ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = dbo.IndentDetail.OFinishedId INNER JOIN
                dbo.ShadeColor ON dbo.ITEM_PARAMETER_MASTER.SHADECOLOR_ID = dbo.ShadeColor.ShadecolorId
                WHERE dbo.IndentDetail.IndentId =" + ddindent.SelectedValue + " and dbo.ITEM_PARAMETER_MASTER.Item_Id=" + dditemname.SelectedValue + " and dbo.ITEM_PARAMETER_MASTER.Quality_Id=" + dquality.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select ShadeColor");
                ddlshade.SelectedValue = ds.Tables[0].Rows[0]["SHADECOLOR_ID"].ToString();
                Session["finishedid"] = ds.Tables[0].Rows[0]["Item_Finished_id"].ToString();
                //ddgodown.SelectedIndex = 1;
                int q = (dquality.SelectedIndex > 0 ? Convert.ToInt32(dquality.SelectedValue) : 0);
                if (q > 0)
                {
                    ql.Visible = true;
                }
                else
                {
                    ql.Visible = false;
                }
                if (Convert.ToInt32(dddesign.SelectedValue) > 0)
                {
                    dsn.Visible = true;
                }
                else
                {
                    dsn.Visible = false;
                }
                int c = (ddcolor.SelectedIndex > 0 ? Convert.ToInt32(ddcolor.SelectedValue) : 0);
                if (c > 0)
                {
                    clr.Visible = true;
                }
                else
                {
                    clr.Visible = false;
                }
                int s = (ddshape.SelectedIndex > 0 ? Convert.ToInt32(ddshape.SelectedValue) : 0);
                if (s > 0)
                {
                    shp.Visible = true;
                }
                else
                {
                    shp.Visible = false;
                }
                int si = (ddsize.SelectedIndex > 0 ? Convert.ToInt32(ddsize.SelectedValue) : 0);
                if (si > 0)
                {
                    sz.Visible = true;
                }
                else
                {
                    sz.Visible = false;
                }
                int sd = (ddlshade.SelectedIndex > 0 ? Convert.ToInt32(ddlshade.SelectedValue) : 0);
                if (sd > 0)
                {
                    shd.Visible = true;
                }
                else
                {
                    shd.Visible = false;
                }
                fill_Order_qty();
                ddgodown.Focus();
                Label2.Visible = false;
            }
            else
            {
                Label2.Visible = true;
                txtcode.Text = "";
                txtcode.Focus();
            }
        }
        else
        {
            ddCatagory.SelectedIndex = 0;
        }
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_Order_qty();
    }
    protected void ddFinish_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_Order_qty();
    }
    protected void ddgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_Order_qty();
        txtrec.Focus();
    }
    protected void txtrec_TextChanged(object sender, EventArgs e)
    {
        double VarRecQty = 0.0;
        double VarReturnQty = 0.0;
        double VarPendingQty = 0.0;
        if (txtretrn.Text != "")
        {
            VarReturnQty = Convert.ToDouble(txtretrn.Text);
        }
        if (txtrec.Text != "")
        {
            VarRecQty = Convert.ToDouble(txtrec.Text);
        }
        if (txtpending.Text != "")
        {
            VarPendingQty = Convert.ToDouble(txtpending.Text);
        }
        if ((VarRecQty + VarReturnQty) > VarPendingQty)
        {
            txtrec.Text = "";
            lblqty.Visible = true;
            txtrec.Focus();
        }
        else
        {
            lblqty.Visible = false;
            txtretrn.Focus();
        }
    }
    private void Show_Report()
    {
        //int VarCompanyId = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
        if (Convert.ToInt16(Session["varMasterCompanyIDForERP"]) == 2)
        {
            Session["ReportPath"] = "Reports/IndentRawRecDestini.rpt";
        }
        else
        {
            Session["ReportPath"] = "Reports/IndentRawRec.rpt";
        }
        Session["CommanFormula"] = "{V_IndentRawRec.PRMid}=" + ViewState["masterid"] + " ";
    }
    protected void DGSHOWDATA_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGSHOWDATA.PageIndex = e.NewPageIndex;
        Fill_Grid_Show();
    }
    private void Fill_Grid_Show()
    {
        DGSHOWDATA.DataSource = "";
        if (ddindent.SelectedIndex > 0)
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Distinct ProductCode from IndentDetail ID,ITEM_PARAMETER_MASTER IPM WHERE ID.OFINISHEDID=IPM.ITEM_FINISHED_ID And ID.IndentId=" + ddindent.SelectedValue + " And IPM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"]);
            DGSHOWDATA.DataSource = Ds;
            DGSHOWDATA.DataBind();
            if (DGSHOWDATA.Rows.Count > 0)
                DGSHOWDATA.Visible = true;
            else
                DGSHOWDATA.Visible = false;
        }
    }
    protected void gvdetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            btnsave.Text = "Update";
            int n = gvdetail.SelectedIndex;
            ViewState["masterid"] = 0;
            string pmid = ((Label)gvdetail.Rows[n].FindControl("lblid")).Text;
            ViewState["masterid"] = pmid;
            DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select challanno,replace(convert(varchar(11),Date,106), ' ','-') as date from PP_ProcessRecMaster where prmid=" + pmid + "");
            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select ipm.productcode,ppt.returnqty,ppt.Godownid from ITEM_PARAMETER_MASTER ipm,PP_ProcessRecTran ppt where ipm.item_finished_id=ppt.finishedid and ppt.prtid=" + gvdetail.SelectedDataKey.Value + "");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                string prod = ds2.Tables[0].Rows[0][0].ToString();
                txtcode.Text = prod;
                txtcode_change();
                txtretrn.Text = ds2.Tables[0].Rows[0]["returnqty"].ToString();
                txtrec.Text = gvdetail.Rows[n].Cells[5].Text;
                //txtpending.Text = Convert.ToString(Convert.ToInt32(txtpending.Text) + Convert.ToInt32(txtrec.Text) + Convert.ToInt32(txtretrn.Text));
                txtpending.Text = Convert.ToString(Convert.ToInt32(txtpending.Text) + Convert.ToInt32(txtrec.Text));
                txtprerec.Text = Convert.ToString(Convert.ToInt32(txtprerec.Text) - Convert.ToInt32(txtrec.Text));
                ddgodown.SelectedValue = ds2.Tables[0].Rows[0]["Godownid"].ToString();
            }
            txtidnt.Text = ds1.Tables[0].Rows[0]["challanno"].ToString();
            txtdate.Text = ds1.Tables[0].Rows[0]["date"].ToString();
        }
        catch
        {

        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            tdIndentNo.Visible = true;
            refresh_edit();
            txtidnt.Visible = true;
            TdChallanno.Visible = true;
        }
        else
        {
            tdIndentNo.Visible = false;
            refresh_edit();
            txtidnt.Text = "";
            TdChallanno.Visible = false;
            btnsave.Text = "Save";
        }
    }
    protected void txtidnt_TextChanged(object sender, EventArgs e)
    {
        if (txtidnt.Text != "")
        {
            DataSet DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
                @"SELECT *,replace(convert(varchar(11),Date,106), ' ','-') as date1 FROM PP_ProcessRecMaster WHERE companyid = " + ddCompName.SelectedValue + " And Challanno=" + txtidnt.Text + "");
            if (DS.Tables[0].Rows.Count > 0)
            {
                ddProcessName.SelectedValue = DS.Tables[0].Rows[0]["processid"].ToString();
                txtdate.Text = DS.Tables[0].Rows[0]["DATE1"].ToString();
                //txtchalanno.Text = DS.Tables[0].Rows[0]["challanno"].ToString();
                UtilityModule.ConditionalComboFill(ref ddempname, "SELECT distinct E.EmpId,E.EmpName FROM IndentMaster IM ,EmpInfo E,PP_ProcessRecMaster ppm where ppm.empid=e.empid and  IM.PartyId=E.EmpId And IM.Processid=" + ddProcessName.SelectedValue + " ANd IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And IM.CompanyId=" + ddCompName.SelectedValue, true, "Select Emp");
                ddempname.SelectedValue = DS.Tables[0].Rows[0]["empid"].ToString();
                UtilityModule.ConditionalComboFill(ref ddindent, @"SELECT distinct IndentId, IndentNo FROM dbo.IndentMaster  where dbo.IndentMaster.indentid in(select distinct indentid from PP_ProcessRecTran ) and dbo.IndentMaster.partyid=" + ddempname.SelectedValue + " and dbo.IndentMaster.processid=" + ddProcessName.SelectedValue + " And IndentMaster.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And IndentMaster.CompanyId=" + ddCompName.SelectedValue, true, "Select Order No");
                DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select distinct indentid from PP_ProcessRecTran where prmid=" + DS.Tables[0].Rows[0]["prmid"].ToString() + "");
                ddindent.SelectedValue = ds1.Tables[0].Rows[0][0].ToString();
                ddindent_selectchange();
                DDChallanNo.SelectedValue = txtidnt.Text;
                challanno_change();
                txtcode.Focus();
            }
        }
    }
    private void refresh_edit()
    {
        txtcode.Text = "";
        txtrecqty.Text = "";
        txtrec.Text = "";
        txtpending.Text = "";
        txtstock.Text = "";
        txtretrn.Text = "";
        txtprerec.Text = "";
        DGSHOWDATA.Visible = false;
        gvdetail.Visible = false;
        pnl1.Enabled = true;
        //ddCompName.SelectedIndex = 0;
        ddProcessName.SelectedIndex = 0;
        ddempname.Items.Clear();
        txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        ddindent.Items.Clear();
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        Report();
    }
    private void Report()
    {
        string qry = @"  SELECT V_IndentRawRec.CATEGORY_NAME,V_IndentRawRec.ITEM_NAME,V_IndentRawRec.DESCRIPTION,V_IndentRawRec.GodownName,V_IndentRawRec.RecQuantity,V_IndentRawRec.EmpName,
                     V_IndentRawRec.CompanyName,V_IndentRawRec.gatepassno,V_IndentRawRec.indentno,V_IndentRawRec.EmpAddress,V_IndentRawRec.EmpPhoneNo,V_IndentRawRec.EmpMobile,V_IndentRawRec.CompanyAddress,
                     V_IndentRawRec.CompanyPhoneNo,V_IndentRawRec.CompanyFaxNo,V_IndentRawRec.TinNo,V_IndentRawRec.ProductCode,V_IndentRawRec.PRTid,FINISHED_TYPE.FINISHED_TYPE_NAME,OrderMaster.CustomerOrderNo,OrderMaster.LocalOrder
                     FROM   FINISHED_TYPE INNER JOIN V_IndentRawRec ON FINISHED_TYPE.Id=V_IndentRawRec.finish_type INNER JOIN OrderMaster ON V_IndentRawRec.orderid=OrderMaster.OrderId
                     where V_IndentRawRec.PRMid=" + ViewState["masterid"] + "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\IndentRawRecDestiniNEW.rpt";
            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\IndentRawRecDestiniNEW.xsd";
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
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int Varprtid = Convert.ToInt32(gvdetail.DataKeys[e.RowIndex].Value);
            SqlParameter[] _arrpara = new SqlParameter[4];
            _arrpara[0] = new SqlParameter("@Prtid", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@VarRowCount", SqlDbType.Int);
            _arrpara[0].Value = Varprtid;
            _arrpara[1].Value = gvdetail.Rows.Count;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_IndentReceiveDeleteRow", _arrpara);
            Tran.Commit();
            DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
            SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'PurchaseReceiveDetail'," + Varprtid + ",getdate(),'Delete')");
            Fill_Grid();
            Fill_Grid_Show();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/IndentRawRecieveDestini.aspx");
            Tran.Rollback();
        }
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        challanno_change();
    }
    private void challanno_change()
    {
        ViewState["masterid"] = DDChallanNo.SelectedValue;
        Fill_Grid();
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
    protected void DGSHOWDATA_SelectedIndexChanged(object sender, EventArgs e)
    {
        int n = DGSHOWDATA.SelectedIndex;
        txtcode.Text = DGSHOWDATA.Rows[n].Cells[0].Text;
        txtcode_change();
    }
}
