using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Process_frmAQl : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                        From Companyinfo CI(nolock)
                        JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @"  
                        Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName 
                        SELECT UNITSID,UNITNAME FROM UNITS ORDER BY UNITNAME
                        SELECT PROCESS_NAME_ID,PROCESS_NAME FROM PROCESS_NAME_MASTER WHERE PROCESS_NAME LIKE 'AQL%' ORDER BY PROCESS_NAME
                        SELECT Distinct IM.ITEM_ID,IM.ITEM_NAME FROM ITEM_MASTER IM INNER JOIN CATEGORYSEPARATE CS ON IM.CATEGORY_ID=CS.CATEGORYID AND CS.ID=0 ORDER BY ITEM_NAME
                        SELECT SHAPEID,SHAPENAME FROM SHAPE";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDUnit, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDJobname, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDItemname, ds, 3, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDShape, ds, 4, true, "--Plz Select--");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            if (DDUnit.Items.Count > 0)
            {
                DDUnit.SelectedIndex = 1;
            }
        }
    }
    protected void FillQuality()
    {
        string str = @"SELECT QUALITYID,QUALITYNAME FROM QUALITY  WHERE ITEM_ID=" + DDItemname.SelectedValue + " order by QualityName";
        UtilityModule.ConditionalComboFill(ref DDQuality, str, true, "--Plz Select--");
    }
    protected void DDItemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQuality();
    }
    protected void FillDesign()
    {
        string str = @"SELECT DISTINCT DESIGNID,DESIGNNAME FROM V_FINISHEDITEMDETAIL WHERE ITEM_ID=" + DDItemname.SelectedValue + " AND QUALITYID=" + DDQuality.SelectedValue + "  and designId>0 ORDER BY DESIGNNAME";
        UtilityModule.ConditionalComboFill(ref DDDesign, str, true, "--Plz Select--");
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDesign();
    }
    protected void FillColor()
    {
        string str = @"SELECT DISTINCT COLORID,COLORNAME FROM V_FINISHEDITEMDETAIL WHERE ITEM_ID=" + DDItemname.SelectedValue + " AND QUALITYID=" + DDQuality.SelectedValue + "  and designId=" + DDDesign.SelectedValue + " and Colorid>0 ORDER BY Colorname";
        UtilityModule.ConditionalComboFill(ref DDColor, str, true, "--Plz Select--");
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillColor();
    }
    protected void FillSize()
    {
        string str = @"select Distinct SizeId,SizeFt From V_finisheditemdetail Where ShapeId=" + DDShape.SelectedValue + " and sizeid>0";
        if (DDItemname.SelectedIndex > 0)
        {
            str = str + " and Item_id=" + DDItemname.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and colorid=" + DDColor.SelectedValue;
        }
        str = str + " order by sizeft";
        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--Plz Select--");
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void btngetstockno_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string where = "";
            if (DDItemname.SelectedIndex > 0)
            {
                where = where + " and vf.item_id=" + DDItemname.SelectedValue;

            }
            if (DDQuality.SelectedIndex > 0)
            {
                where = where + " and vf.qualityid=" + DDQuality.SelectedValue;

            }
            if (DDDesign.SelectedIndex > 0)
            {
                where = where + " and vf.designid=" + DDDesign.SelectedValue;

            }
            if (DDColor.SelectedIndex > 0)
            {
                where = where + " and vf.colorid=" + DDColor.SelectedValue;

            }
            if (DDShape.SelectedIndex > 0)
            {
                where = where + " and vf.shapeid=" + DDShape.SelectedValue;

            }
            if (DDSize.SelectedIndex > 0)
            {
                where = where + " and vf.sizeid=" + DDSize.SelectedValue;

            }

            SqlCommand cmd = new SqlCommand("PRO_GETSTOCKNOFORAQLPROCESS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@unitid", DDUnit.SelectedValue);
            cmd.Parameters.AddWithValue("@Processid", DDJobname.SelectedValue);
            cmd.Parameters.AddWithValue("@ProcessName", DDJobname.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@Where", where);
            cmd.Parameters.AddWithValue("@TOtalpcs", txttotalpcs.Text == "" ? "0" : txttotalpcs.Text);
            cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Samplepcs", SqlDbType.Int);
            cmd.Parameters["@Samplepcs"].Direction = ParameterDirection.Output;
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            if (cmd.Parameters["@msg"].Value.ToString() != "")
            {
                lblmsg.Text = cmd.Parameters["@msg"].Value.ToString();
            }
            //*************
            DGStock.DataSource = dt;
            DGStock.DataBind();
            //*****SELECT RANDOM PCS
            int Samplepcs = Convert.ToInt16(cmd.Parameters["@Samplepcs"].Value);
            Random r = new Random();
            int[] array = new int[Convert.ToInt16(txttotalpcs.Text) + 1];
            int number = 0;

            for (int i = 0; i < Samplepcs; i++)
            {
                number = r.Next(1, Convert.ToInt16(txttotalpcs.Text) + 1);
                if (!array.Contains(number)) //If it's not contains, add number to array;
                {
                    array[i] = number;
                    for (int j = 0; j < DGStock.Rows.Count; j++)
                    {
                        Label lblsrno = (Label)DGStock.Rows[j].FindControl("lblsrno");
                        if (Convert.ToInt16(lblsrno.Text) == number)
                        {
                            CheckBox chkitem = (CheckBox)DGStock.Rows[j].FindControl("chkitem");
                            chkitem.Visible = true;
                            chkitem.Checked = true;
                            DGStock.Rows[j].BackColor = System.Drawing.Color.LightGreen;

                        }
                    }
                }
                else //If it contains, restart random process
                {
                    i--;
                }
            }

            //******
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();

        }
    }

    protected void txttotalpcs_TextChanged(object sender, EventArgs e)
    {
        DGStock.DataSource = null;
        DGStock.DataBind();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        DataTable dt = new DataTable();
        dt.Columns.Add("StockNo", typeof(Int32));
        dt.Columns.Add("TStockNo", typeof(string));
        dt.Columns.Add("Stockstatus", typeof(int));
        dt.Columns.Add("Fromprocessid", typeof(int));
        for (int i = 0; i < DGStock.Rows.Count; i++)
        {
            DataRow dr = dt.NewRow();
            Label lblstockno = (Label)DGStock.Rows[i].FindControl("lblstockno");
            Label lbltstockno = (Label)DGStock.Rows[i].FindControl("lbltstockno");
            CheckBox chkitem = (CheckBox)DGStock.Rows[i].FindControl("chkitem");
            Label lblfromprocessid = (Label)DGStock.Rows[i].FindControl("lblfromprocessid");

            dr["stockno"] = lblstockno.Text;
            dr["Tstockno"] = lbltstockno.Text;
            dr["stockstatus"] = chkitem.Visible == false ? "0" : (chkitem.Checked == false ? "2" : "1");
            dr["fromprocessid"] = lblfromprocessid.Text;
            dt.Rows.Add(dr);
        }
        if (dt.Rows.Count > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("PRO_SAVEAQLDETAIL", con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;

                cmd.Parameters.AddWithValue("@ID", 0);
                cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
                cmd.Parameters.AddWithValue("@Unitid", DDUnit.SelectedValue);
                cmd.Parameters.AddWithValue("@Jobid", DDJobname.SelectedValue);
                cmd.Parameters.AddWithValue("@AQLlevel", Convert.ToDecimal(DDAql.SelectedItem.Text));
                cmd.Parameters.AddWithValue("@EMpcode", txtempcode.Text);
                cmd.Parameters.Add("@Aqllotno", SqlDbType.VarChar, 50);
                cmd.Parameters["@Aqllotno"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Totalnoofpcs", txttotalpcs.Text);
                cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
                cmd.Parameters.AddWithValue("@dt", dt);
                cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
                cmd.Parameters.AddWithValue("@Unitname", DDUnit.SelectedItem.Text);
             
                cmd.ExecuteNonQuery();
                lblmsg.Text = cmd.Parameters["@msg"].Value.ToString();
                Tran.Commit();
                DGStock.DataSource = null;
                DGStock.DataBind();
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
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alsave", "alert('No Data available to save')", true);
        }
    }
    
}