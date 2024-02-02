using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


public partial class Masters_Process_FrmMixingColor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref ddcompanyname, "select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By CompanyName", true, "--Select Company--");
            UtilityModule.ConditionalComboFill(ref ddprocessname, "select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where Process_name like 'MIX%' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select Process--");
            UtilityModule.ConditionalComboFill(ref ddgodownname, "select GoDownID,GodownName from GodownMaster Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select Godown--");
            UtilityModule.ConditionalComboFill(ref ddcategoryname, @"select CM.CATEGORY_ID,cm.CATEGORY_NAME from ITEM_CATEGORY_MASTER  CM Inner join CategorySeparate CS
                                                                  on CM.CATEGORY_ID=cs.Categoryid And cs.id=1 And CM.Mastercompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CATEGORY_NAME", true, "--Plz Select--");
            if (ddcompanyname.Items.Count > 0)
            {
                ddcompanyname.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddcompanyname.Enabled = false;
            }
        }
    }
    protected void ddgodownname_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddlotno, "select Distinct LotNo,LotNo As LotNo1 from Stock Where Godownid=" + ddgodownname.SelectedValue + " Order by LotNo1", true, "--Select LotNo--");
    }
    protected void ddcategoryname_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dditemname, "select Item_id,ITEM_NAME from ITEM_MASTER where CATEGORY_ID= " + ddcategoryname.SelectedValue + " And MasterCompanyid= " + Session["varMasterCompanyIDForERP"] + " Order by ITEM_NAME", true, "--Select Item--");
        UtilityModule.ConditionalComboFill(ref ddcategoryname1, @"select CM.CATEGORY_ID,cm.CATEGORY_NAME from ITEM_CATEGORY_MASTER  CM Inner join CategorySeparate CS
                                                             on CM.CATEGORY_ID=cs.Categoryid And cs.id=1 And CM.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CATEGORY_NAME", true, "--Select Item--");

    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddqualityname, "select QualityId,QualityName from Quality Where Item_Id=" + dditemname.SelectedValue + " And MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " Order by QualityName", true, "--Select Quality--");
    }
    protected void ddqualityname_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();

    }
    protected void FillGrid()
    {
        string str = string.Empty;
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            str = @"select Distinct S.godownId,S.CompanyId,S.LotNo,ShadeColorId,ShadeColorName ,Vf.item_finished_id,Round(Sum(Qtyinhand),3) As StockQty  from  V_FinishedItemDetail vf Inner join Stock S  on s.Item_finished_id=vf.Item_finished_id And S.CompanyId=" + ddcompanyname.SelectedValue + @"  where ColorId=0 And designId=0 
               And SizeId=0 And ShapeId=0 And ShadecolorId<>0 And QualityId=" + ddqualityname.SelectedValue + " and Item_id=" + dditemname.SelectedValue + " and category_id=" + ddcategoryname.SelectedValue + "  And S.godownId=" + ddgodownname.SelectedValue + " And S.LotNo='" + ddlotno.SelectedItem.Text + @"' 
               group by S.godownId,S.CompanyId,S.LotNo,ShadeColorId,ShadeColorName,Vf.item_finished_id";
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
            gvmixingcolor.DataSource = ds;
            gvmixingcolor.DataBind();

        }
        catch (Exception ex)
        {

            throw;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void ddcategoryname1_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dditemname1, "select Item_id,ITEM_NAME from ITEM_MASTER where CATEGORY_ID= " + ddcategoryname1.SelectedValue + " And MasterCompanyid= " + Session["varMasterCompanyIDForERP"] + " Order by ITEM_NAME", true, "--Select Item--");
        ddlcategorycange();
    }
    private void ddlcategorycange()
    {

        ddqualityname1.Items.Clear();
        ddcolorname.Items.Clear();
        dddesign1.Items.Clear();
        ddsize.Items.Clear();
        ddshadecolor.Items.Clear();
        ddshapename.Visible = false;
        TdDesign.Visible = false;
        TdColor.Visible = false;
        TdColorShade.Visible = false;
        TdShape.Visible = false;
        TdSize.Visible = false;
        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME 
                      FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on 
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddcategoryname1.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        TdQuality.Visible = true;
                        break;
                    case "2":
                        TdDesign.Visible = true;
                        break;
                    case "3":
                        TdColor.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddcolorname, "select ColorId,ColorName from color Where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " Order by ColorName", true, "--select Color--");
                        break;
                    case "6":
                        TdColorShade.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddshadecolor, "select shadecolorId,shadecolorname from shadecolor order by Shadecolorname", true, "--Select Shadecolor--");
                        break;
                    case "4":
                        TdShape.Visible = true;
                        break;
                    case "5":
                        TdSize.Visible = true;
                        break;
                    case "10":
                        TdColor.Visible = true;
                        break;
                }
            }
        }


    }
    protected void dditemname1_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddqualityname1, "select QualityId,QualityName from Quality Where Item_Id=" + dditemname1.SelectedValue + " And MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " Order by QualityName", true, "--Select Quality--");
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        int saveflag = 0;
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        { con.Open(); }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[11];
            param[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
            param[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
            param[2] = new SqlParameter("@GodownId", SqlDbType.Int);
            param[3] = new SqlParameter("@LotNo", SqlDbType.VarChar, 50);
            param[4] = new SqlParameter("@FItem_Finished_id", SqlDbType.Int);
            param[5] = new SqlParameter("@TItem_Finished_id", SqlDbType.Int);
            param[6] = new SqlParameter("@MIxId", SqlDbType.Int);
            param[7] = new SqlParameter("@MixDetailId", SqlDbType.Int);
            param[8] = new SqlParameter("@Qty", SqlDbType.Float);
            param[9] = new SqlParameter("@Userid", SqlDbType.Int);
            param[10] = new SqlParameter("@MasterCompanyid", SqlDbType.Int);

            //Assing Value

            for (int i = 0; i < gvmixingcolor.Rows.Count; i++)
            {

                Label lblcompanyId = ((Label)gvmixingcolor.Rows[i].FindControl("lblCompanyid"));
                Label lblgodownid = ((Label)gvmixingcolor.Rows[i].FindControl("lblgodownid"));
                Label lblLotno = ((Label)gvmixingcolor.Rows[i].FindControl("lblLotno"));
                Label lblitemfinishedid = ((Label)gvmixingcolor.Rows[i].FindControl("lblitemfinishedid"));
                TextBox txtmixQty = ((TextBox)gvmixingcolor.Rows[i].FindControl("txtmixQty"));
                CheckBox chk = ((CheckBox)gvmixingcolor.Rows[i].FindControl("chkmix"));
                if (chk.Checked == true && (txtmixQty.Text != "0" || txtmixQty.Text != ""))
                {
                    saveflag = saveflag + 1;
                    param[0].Value = lblcompanyId.Text;
                    param[1].Value = ddprocessname.SelectedValue;
                    param[2].Value = lblgodownid.Text;
                    param[3].Value = lblLotno.Text;
                    param[4].Value = lblitemfinishedid.Text;
                    int TItem_finished_id = UtilityModule.getItemFinishedId(dditemname1, ddqualityname1, dddesign1, ddcolorname, ddshapename, ddsize, TxtProdCode, Tran, ddshadecolor, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                    param[5].Value = TItem_finished_id;
                    if (hnMixId.Value == "0" || hnMixId.Value == "")
                    {
                        hnMixId.Value = "0";
                    }
                    param[6].Value = hnMixId.Value;
                    param[6].Direction = ParameterDirection.InputOutput;
                    param[7].Value = 0;
                    param[7].Direction = ParameterDirection.InputOutput;
                    param[8].Value = txtmixQty.Text == "" ? "0" : txtmixQty.Text;
                    param[9].Value = Session["varuserid"];
                    param[10].Value = Session["varMasterCompanyIDForERP"];

                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveMixColor", param);
                    hnMixId.Value = param[6].Value.ToString();

                    //Update Stock Qty
                    UtilityModule.StockStockTranTableUpdate(Convert.ToInt32(lblitemfinishedid.Text), Convert.ToInt32(lblgodownid.Text), Convert.ToInt32(lblcompanyId.Text), Convert.ToString(lblLotno.Text), Convert.ToDouble(param[8].Value), DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToString("dd-MMM-yyyy"), "MIXCOLOR", Convert.ToInt32(param[7].Value), Tran, 0, false, 1, 0);

                    UtilityModule.StockStockTranTableUpdate(Convert.ToInt32(TItem_finished_id), Convert.ToInt32(lblgodownid.Text), Convert.ToInt32(lblcompanyId.Text), Convert.ToString(lblLotno.Text), Convert.ToDouble(param[8].Value), DateTime.Now.ToString("dd-MMM-yyyy"), DateTime.Now.ToString("dd-MMM-yyyy"), "MIXCOLOR", Convert.ToInt32(param[7].Value), Tran, 1, true, 1, 0);

                    //End

                }
            }
            Tran.Commit();
            if (saveflag == 0)
            {
                lblmsg.Text = "Plz Select Check box And Enter Mix Qty";
            }
            else
            {
                lblmsg.Text = "Data saved Successfully........";
            }
            refreshControl();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;

        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void MessageBox(string message)
    {
        Label lblmessage = new Label();
        lblmessage.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + message + "')</script>";
        Page.Controls.Add(lblmessage);
    }
    protected void refreshControl()
    {
        gvmixingcolor.DataSource = "";
        gvmixingcolor.DataBind();
        hnMixId.Value = "0";
        dditemname.SelectedIndex = -1;
        dditemname1.SelectedIndex = -1;

    }
    protected void ddlotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvmixingcolor.DataSource = "";
        gvmixingcolor.DataBind();
        ddqualityname.SelectedIndex = -1;
    }
}