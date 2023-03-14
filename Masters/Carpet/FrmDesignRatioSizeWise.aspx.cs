using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.Services;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Text;

public partial class FrmDesignRatioSizeWise : CustomPage
{
    public string mode = "";
    //public Boolean DgClk;
    public string DgClk = "false";
   public string flag = "false";

    protected void BindQualityType()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            Hashtable param = new Hashtable();
            param.Add("@mode", "Get");
            DataTable dt = DataAccess.fetch("Pro_GetQualityType", param);

            hncategory.Value = dt.Rows[0]["CATEGORY_ID"].ToString();

            ddlQualityType.DataValueField = "ITEM_ID";
            ddlQualityType.DataTextField = "ITEM_NAME";
            ddlQualityType.DataSource = dt;
            ddlQualityType.DataBind();
            ddlQualityType.Items.Insert(0, "--Select--");
            ddlQualityType.Items[0].Value = "0";

            tran.Commit();

            ViewState["CarpetType"] = ddlQualityType.SelectedValue;
        }
        catch (Exception ex)
        {
            tran.Rollback();

            //lblmsg.Text = ex.Message;
            con.Close();
            throw ex;
        }
    }
    protected void BindQualityName()
    {
        if (Convert.ToInt32(ddlQualityType.SelectedValue) > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                Hashtable param = new Hashtable();
                param.Add("@mode", "Get");
                param.Add("@Item_Id", ddlQualityType.SelectedValue);
                DataTable dt = DataAccess.fetch("Pro_GetQualityName", param);

                ddlQuality.DataValueField = "QualityID";
                ddlQuality.DataTextField = "QualityName";
                ddlQuality.DataSource = dt;
                ddlQuality.DataBind();
                ddlQuality.Items.Insert(0, "--Select--");
                ddlQuality.Items[0].Value = "0";

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }
        }
        else
        {
        }
    }
    protected void BindDesignName()
    {
        if (Convert.ToInt32(ddlQuality.SelectedValue) > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (ddlQuality.SelectedIndex != 0)
                {
                    Hashtable param = new Hashtable();
                    param.Add("@mode", "Get");
                    param.Add("@Item_Id", ddlQualityType.SelectedValue);
                    param.Add("@QualityId", ddlQuality.SelectedValue);
                    DataTable dt = DataAccess.fetch("Pro_GetDesignName", param);

                    ddlDesignName.DataValueField = "designId";
                    ddlDesignName.DataTextField = "designName";
                    ddlDesignName.DataSource = dt;
                    ddlDesignName.DataBind();
                    ddlDesignName.Items.Insert(0, "--Select--");
                    ddlDesignName.Items[0].Value = "0";
                }
                else
                {
                    ddlDesignName.Items.Clear();
                    ddlDesignName.DataSource = null;
                    ddlDesignName.DataBind();
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Please select quality type');", true);
        }
    }
    protected void BindColorName()
    {
        if (Convert.ToInt32(ddlDesignName.SelectedValue) > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                Hashtable param = new Hashtable();
                param.Add("@mode", "Get");
                param.Add("@designId", ddlDesignName.SelectedValue);
                DataTable dt = DataAccess.fetch("Pro_GetColorName", param);

                ddlColorName.DataValueField = "colorid";
                ddlColorName.DataTextField = "ColorName";
                ddlColorName.DataSource = dt;
                ddlColorName.DataBind();
                ddlColorName.Items.Insert(0, "--Select--");
                ddlColorName.Items[0].Value = "-1";

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }
        }
        else
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                Hashtable param = new Hashtable();
                param.Add("@mode", "Get");
                param.Add("@designId", 0);
                DataTable dt = DataAccess.fetch("Pro_GetColorName", param);

                ddlColorName.DataValueField = "colorid";
                ddlColorName.DataTextField = "ColorName";
                ddlColorName.DataSource = dt;
                ddlColorName.DataBind();
                ddlColorName.Items.Insert(0, "--Select--");
                ddlColorName.Items[0].Value = "-1";

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }
        }
    }

    protected void BindRawMaterial_Require()
    {

        if (ddlQualityType.SelectedValue != "0" || ddlQuality.SelectedValue != "0" || ddlDesignName.SelectedValue != "0" || ddlColorName.SelectedValue != "0")
        {

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                if (ddlQuality.SelectedIndex != 0)
                {
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@QualityId", ddlQuality.SelectedValue);
                    param[1] = new SqlParameter("@DesignId", ddlDesignName.SelectedValue);
                    param[2] = new SqlParameter("@ColorId", ddlColorName.SelectedValue);

                    //**********
                    DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetRawMaterialRequire", param);

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        SqlParameter[] param2 = new SqlParameter[3];
                        param2[0] = new SqlParameter("@QualityId", ddlQuality.SelectedValue);
                        param2[1] = new SqlParameter("@DesignId", ddlDesignName.SelectedValue);
                        param2[2] = new SqlParameter("@ColorId", 0);

                        //**********
                        ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetRawMaterialRequire", param2);
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblConsumpKgSqMt2.Text = ds.Tables[0].Rows[0]["SubItemQty"].ToString();
                    }
                    else
                    {
                        lblConsumpKgSqMt2.Text = "";
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        int UnitId = Convert.ToInt32(ds.Tables[1].Rows[0]["unitid"].ToString());

                        if (UnitId > 0)
                        {
                            lblConsumpKgSqMt.Text = "Consumed In kg/Sq. Mt.";
                        }
                        else
                        {
                            lblConsumpKgSqMt.Text = "Consumed In kg/Sq. Yard";
                        }
                    }

                }
                //else
                //{
                //    ddlSizeShape.Items.Clear();
                //    ddlSizeShape.DataSource = null;
                //    ddlSizeShape.DataBind();
                //}

                Tran.Commit();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }

        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Please select quality type and quality and design and color');", true);
        }
    }
    protected void BindWoolType()
    {
        if (ddlQualityType.SelectedIndex > 0 && ddlQuality.SelectedIndex > 0 && ddlDesignName.SelectedIndex > 0 && ddlColorName.SelectedIndex > 0)
        {

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                if (ddlQuality.SelectedIndex != 0)
                {
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@QualityId", ddlQuality.SelectedValue);
                    param[1] = new SqlParameter("@DesignId", ddlDesignName.SelectedValue);
                    param[2] = new SqlParameter("@ColorId", ddlColorName.SelectedValue);

                    //**********
                    DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetRawMaterialRequire", param);

                
                        ddlWoolType.DataValueField = "QualityId";
                        ddlWoolType.DataTextField = "QualityName";
                        ddlWoolType.DataSource = ds.Tables[2];
                        ddlWoolType.DataBind();
                        ddlWoolType.Items.Insert(0, "--Select--");
                        ddlWoolType.Items[0].Value = "0";
                   

                }
                //else
                //{
                //    ddlSizeShape.Items.Clear();
                //    ddlSizeShape.DataSource = null;
                //    ddlSizeShape.DataBind();
                //}

                Tran.Commit();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }

        }
    }
    protected void BindSizeShape()
    {
        if (ddlQualityType.SelectedIndex > 0 && ddlQuality.SelectedIndex > 0 && ddlDesignName.SelectedIndex > 0 && ddlColorName.SelectedIndex > 0)
        {

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                if (ddlQuality.SelectedIndex != 0)
                {
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@QualityId", ddlQuality.SelectedValue);
                    param[1] = new SqlParameter("@DesignId", ddlDesignName.SelectedValue);
                    param[2] = new SqlParameter("@ColorId", ddlColorName.SelectedValue);

                    //**********
                    DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetRawMaterialRequire", param);

                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        ddlSizeShape.DataValueField = "SizeId";
                        ddlSizeShape.DataTextField = "Export_Format1";
                        ddlSizeShape.DataSource = ds.Tables[3];
                        ddlSizeShape.DataBind();
                        ddlSizeShape.Items.Insert(0, "--All Size--");
                        ddlSizeShape.Items[0].Value = "0";
                    }

                    if (Session["varcompanyId"].ToString() == "20" && variable.VarNewQualitySize == "1")
                    {
                        ddlSizeShape.Enabled = false;
                    }
                    else
                    {
                        ddlSizeShape.Enabled = true;
                    }

                }
                //else
                //{
                //    ddlSizeShape.Items.Clear();
                //    ddlSizeShape.DataSource = null;
                //    ddlSizeShape.DataBind();
                //}

                Tran.Commit();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }

        }
    }
    protected void BindShadeColor()
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            if (ddlQuality.SelectedIndex != 0)
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ShadecolorId", null);

                //**********
                DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetAllShadeColor", param);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlShadeColor.DataValueField = "ShadecolorId";
                    ddlShadeColor.DataTextField = "ShadeColorName";
                    ddlShadeColor.DataSource = ds;
                    ddlShadeColor.DataBind();
                    ddlShadeColor.Items.Insert(0, "--Select--");
                    ddlShadeColor.Items[0].Value = "0";
                }
            }

            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            //lblmsg.Text = ex.Message;
            con.Close();
        }

    }
    protected void BindGDDesignRatioSizeWise()
    {
        if (ddlQualityType.SelectedIndex > 0 && ddlQuality.SelectedIndex > 0 && ddlDesignName.SelectedIndex > 0 && ddlColorName.SelectedIndex > 0 && ddlSizeShape.SelectedIndex >=0)
        {
            if (GDConsumptionMaster.Rows.Count > 0)
            {
                GDConsumptionMaster.DataSource = null;
                GDConsumptionMaster.DataBind();

                ViewState["AddItem"] = null;
            }


            double TotalConsumpQty3 = 0;
            double TotalConsumpQtyPercentage = 0;
            int SizeId1 ;
            int SizeId2;
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[4];

                param[0] = new SqlParameter("@QualityId", ddlQuality.SelectedValue);
                param[1] = new SqlParameter("@DesignId", ddlDesignName.SelectedValue);
                param[2] = new SqlParameter("@ColorId", ddlColorName.SelectedValue);
                param[3] = new SqlParameter("@SizeId", ddlSizeShape.SelectedValue);



                //**********
                DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetDesignRatioSize", param);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    tbAdjustAge.Text = ds.Tables[0].Rows[0]["AdjustInPer"].ToString();
                    hnID2.Value = ds.Tables[0].Rows[0]["Id"].ToString();
                    hnEditStatus.Value = "True";
                    BtnPreview.Visible = true;

                    dt = ds.Tables[0];

                    ViewState["AddItem"] = dt;

                    GDConsumptionMaster.DataSource = ViewState["AddItem"];
                    GDConsumptionMaster.DataBind();
                }
                else
                {
                    BtnPreview.Visible = false;
                    hnID2.Value = "";
                    hnEditStatus.Value = "";
                }
                
                lblTotalConsumpQtyGms.Text = "";
                lblTotalRemainingQtyGms.Text = "";
                lblTotalRemainingQtyPer.Text = "";

                if (GDConsumptionMaster.Rows.Count > 0)
                {
                    //lblCon.Text = Val(lblCon.Text) + dgColorSize.Item("Quantity", i).Value

                    for (int i = 0; i < GDConsumptionMaster.Rows.Count; i++)
                    {
                        //string id = (lblTotalConsumpQtyGms) + (GDConsumptionMaster.Rows[i]["QtyInPer"].ToString());
                        string QtyinPer = ((Label)(GDConsumptionMaster.Rows[i].FindControl("lblQtyInPer"))).Text;
                        TotalConsumpQty3 += Convert.ToDouble(QtyinPer);
                        lblTotalConsumpQtyGms.Text = Convert.ToString(Math.Round((TotalConsumpQty3), 3));

                        string QtyPercentage = ((Label)(GDConsumptionMaster.Rows[i].FindControl("lblQtyPercentage"))).Text;
                        TotalConsumpQtyPercentage += Convert.ToDouble(QtyPercentage);
                        lblTotalConsumpQtyPer.Text = Convert.ToString(Math.Round((TotalConsumpQtyPercentage), 3));
                       
                    }

                    double TotalRemainingQty = ((Convert.ToDouble(lblConsumpKgSqMt2.Text) * 1000) - Convert.ToDouble(lblTotalConsumpQtyGms.Text));
                    lblTotalRemainingQtyGms.Text = Convert.ToString(Math.Round((TotalRemainingQty), 3));


                    double TotalRemainingQtyPercentage = (100 - Convert.ToDouble(TotalConsumpQtyPercentage));
                    lblTotalRemainingQtyPer.Text = Convert.ToString(Math.Round((TotalRemainingQtyPercentage), 3));
                }


                Tran.Commit();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }
        }
    }

    DataTable dt;
    private void maketable()
    {
        dt.Columns.Add("Id");
        dt.Columns.Add("DyerColorId");
        dt.Columns.Add("DyerColorName");
        dt.Columns.Add("QtyInPer");
        dt.Columns.Add("SubItemId");
        dt.Columns.Add("ItemName");
        dt.Columns.Add("OFinishedId");
        dt.Columns.Add("IFinishedId");
        dt.Columns.Add("Percentage");

    }

    private void checkDeleteButton()
    {
        if (GDConsumptionMaster.Rows.Count != 0)
        {
            //((ImageButton)GDConsumptionMaster.Rows[0].FindControl("ImageButton1")).Visible = true;
            ((Button)GDConsumptionMaster.Rows[0].FindControl("btnDelete")).Visible = true;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            BindQualityType();
            BindQualityName();
            //BindGDConsumptionMaster();
            BindShadeColor();
            hnStatus.Value = "false";

        }
    }
    protected void ddlQualityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindQualityName();
      
    }
    protected void ddlQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlQuality.SelectedValue) > 0)
        {
            BindDesignName();
            //BindSizeShape();
            BindColorName();
            BindGDDesignRatioSizeWise();
        }
    }
    protected void ddlDesignName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlDesignName.SelectedValue) >= 0)
        {
            BindColorName();
            //BindGDConsumptionMaster();
        }
    }
    protected void ddlSizeShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlSizeShape.SelectedValue) >= 0)
        {
            BindGDDesignRatioSizeWise();
        }
    }
    protected void ddlColorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlColorName.SelectedValue) >= 0)
        {
            BindRawMaterial_Require();
            BindWoolType();
            BindSizeShape();
            BindGDDesignRatioSizeWise();
            lblMessage.Text = "";
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        // int TotalConsumpQty=0;
        double TotalConsumpQty = 0;

        double TotalConsumpQtyPercentage = 0;
        int id = 0;
        

        if (hnStatus.Value == "false")
        {
            if (GDConsumptionMaster.Rows.Count > 0)
            {
                if (lblTotalRemainingQtyGms.Text == "0")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Remaining Qty is not Left');", true);


                    //ddlShadeColor.SelectedIndex = 0;
                    tbConsumpGms.Text = "";
                    tbConsumpAge.Text = "";
                    
                }
            }

            if (lblTotalRemainingQtyGms.Text != "0")
            {
                lblTotalConsumpQtyGms.Text = Convert.ToString(TotalConsumpQty);

                lblTotalConsumpQtyPer.Text = Convert.ToString(TotalConsumpQtyPercentage);


                if (ViewState["AddItem"] == null)
                {
                    dt = new DataTable();
                    maketable();
                    ViewState["AddItem"] = dt;
                }
                dt = (DataTable)ViewState["AddItem"];

                int j = 0;
                int i;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["DyerColorName"].ToString() == ddlShadeColor.SelectedItem.Text && dt.Rows[i]["SubItemId"].ToString() == ddlWoolType.SelectedValue)
                    {
                        j = 1;
                        ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Record is already Present');", true);
                        //Label2.Visible = true;
                        //Label2.Text = "Item already added.";
                        // lblMessage.Text = "Record is already Present";
                        break;
                    }

                    if (GDConsumptionMaster.Rows.Count > 0)
                    {
                        //// lblCon.Text = Val(lblCon.Text) + Val(dgColorSize.Item("Quantity", i).Value)
                        // TotalConsumpQty += Convert.ToInt32(dt.Rows[i]["QtyInPer"].ToString());
                        TotalConsumpQty += Convert.ToDouble(dt.Rows[i]["QtyInPer"].ToString());
                        lblTotalConsumpQtyGms.Text = Convert.ToString(TotalConsumpQty);


                        TotalConsumpQtyPercentage += Convert.ToDouble(dt.Rows[i]["Percentage"].ToString());
                        lblTotalConsumpQtyPer.Text = Convert.ToString(TotalConsumpQtyPercentage);
                    }

                }
                if (j == 0)
                {
                    if (string.IsNullOrWhiteSpace(tbAdjustAge.Text))
                    {
                        tbAdjustAge.Text = "0";
                    }

                    double adjust = ((Convert.ToDouble(lblConsumpKgSqMt2.Text) * Convert.ToDouble(tbAdjustAge.Text) / 100) * 1000);
                    // double adjust = (Convert.ToDouble(lblConsumpKgSqMt2.Text) * Convert.ToDouble((Convert.ToDouble(tbAdjustAge.Text)) / 100) * 1000);

                    // TotalConsumpQty += Convert.ToInt32(tbConsumpGms.Text);
                    TotalConsumpQty += Convert.ToDouble(tbConsumpGms.Text == "" ? "0" : tbConsumpGms.Text);
                    lblTotalConsumpQtyGms.Text = Convert.ToString(Math.Round((TotalConsumpQty), 3));

                    TotalConsumpQtyPercentage += Convert.ToDouble(tbConsumpAge.Text == "" ? "0" : tbConsumpAge.Text);
                    lblTotalConsumpQtyPer.Text = Convert.ToString(Math.Round((TotalConsumpQtyPercentage), 3));

                    if (Convert.ToDouble(lblTotalConsumpQtyGms.Text) + adjust > (Convert.ToDouble(lblConsumpKgSqMt2.Text) * 1000))
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Consumption Qty cannot be greater than Required Qty');", true);

                        lblTotalConsumpQtyGms.Text = Convert.ToString(Convert.ToDouble(lblTotalConsumpQtyGms.Text) - Convert.ToDouble(tbConsumpGms.Text));

                        lblTotalRemainingQtyGms.Text = Convert.ToString((Convert.ToDouble(lblConsumpKgSqMt2.Text) * 1000) - Convert.ToDouble(lblTotalConsumpQtyGms.Text) - adjust);

                        lblTotalConsumpQtyPer.Text = Convert.ToString(Convert.ToDouble(lblTotalConsumpQtyPer.Text) - Convert.ToDouble(tbAdjustAge.Text == "" ? "0" : tbAdjustAge.Text));
                        lblTotalRemainingQtyPer.Text = Convert.ToString((Convert.ToDouble(100)) - Convert.ToDouble(lblTotalConsumpQtyPer.Text) - Convert.ToDouble(tbAdjustAge.Text == "" ? "0" : tbAdjustAge.Text));
                    }
                    else
                    {
                        //id++;

                        // int id2=+id;

                        if (ddlShadeColor.SelectedIndex > 0 && ddlWoolType.SelectedIndex > 0 && tbConsumpGms.Text !="" && tbConsumpAge.Text != "")
                        {

                            DataRow dr = dt.NewRow();

                            if (hnEditStatus.Value == "True")
                            {
                                dr["Id"] = hnID2.Value;
                            }
                            else
                            {
                                dr["Id"] = i + 1;
                            }

                            dr["DyerColorId"] = ddlShadeColor.SelectedValue;
                            dr["DyerColorName"] = ddlShadeColor.SelectedItem.Text;
                            dr["QtyInPer"] = tbConsumpGms.Text == "" ? "0" : tbConsumpGms.Text;
                            dr["SubItemId"] = ddlWoolType.SelectedValue;
                            dr["ItemName"] = ddlWoolType.SelectedItem.Text;
                            dr["OFinishedId"] = 0;
                            dr["IFinishedId"] = 0;
                            dr["Percentage"] = tbConsumpAge.Text == "" ? "0" : tbConsumpAge.Text;
                            dt.Rows.Add(dr);

                            ViewState["AddItem"] = dt;

                            GDConsumptionMaster.DataSource = ViewState["AddItem"];
                            GDConsumptionMaster.DataBind();

                            //lblRem.Text = (Val(lblKg.Text) * 1000) - Val(lblCon.Text)
                            lblTotalRemainingQtyGms.Text = Convert.ToString(Math.Round((Convert.ToDouble(lblConsumpKgSqMt2.Text) * 1000) - (Convert.ToDouble(lblTotalConsumpQtyGms.Text)), 3));

                            lblTotalRemainingQtyPer.Text = Convert.ToString(Math.Round((Convert.ToDouble(100)) - (Convert.ToDouble(lblTotalConsumpQtyPer.Text)), 3));
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Please fill all required details');", true);
                        }
                    }

                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Remaining Qty is not Left');", true);
                //ddlShadeColor.SelectedIndex = 0;
                tbConsumpGms.Text = "";
                tbConsumpAge.Text = "";
            }

            

        }
        else if (hnStatus.Value == "true")
        {

            lblTotalConsumpQtyGms.Text = "0";

            DataTable dt2 = (DataTable)ViewState["AddItem"];

            int j = 0;
            //int i;
            //for (i = 0; i < dt2.Rows.Count; i++)
            //{
            //    if (dt2.Rows[i]["DyerColorName"].ToString() == ddlShadeColor.SelectedItem.Text && dt2.Rows[i]["SubItemId"].ToString() == ddlWoolType.SelectedValue)
            //    {
            //        j = 1;
            //        ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Record is already Present');", true);                   
            //        break;
            //    }
            //}

            int count = GDConsumptionMaster.Rows.Count;
            if (GDConsumptionMaster.Rows.Count > 0)
            {
                for (int m = 0; m < dt2.Rows.Count; m++)
                {
                    //string QtyInPer = ((Label)GDConsumptionMaster.Rows[i].FindControl("QtyInPer")).Text;
                    //TotalConsumpQty += Convert.ToInt32(QtyInPer);
                    //lblTotalConsumpQtyGms.Text = Convert.ToString(TotalConsumpQty);

                    //TotalConsumpQty +=Convert.ToInt32(dt2.Rows[m]["QtyInPer"].ToString());
                    TotalConsumpQty += Convert.ToDouble(dt2.Rows[m]["QtyInPer"].ToString());
                    ////TotalConsumpQty += TotalConsumpQty;
                    lblTotalConsumpQtyGms.Text = Convert.ToString(TotalConsumpQty);

                    TotalConsumpQtyPercentage += Convert.ToDouble(dt2.Rows[m]["Percentage"].ToString());
                    ////TotalConsumpQty += TotalConsumpQty;
                    lblTotalConsumpQtyPer.Text = Convert.ToString(TotalConsumpQtyPercentage);
                }
                //lblTotalConsumpQtyGms.Text = (lblTotalConsumpQtyGms.Text) + GDConsumptionMaster.Rows[0]["QtyInPer"];
            }

            if (j == 0)
            {
                double adjust = (Convert.ToDouble(lblConsumpKgSqMt2.Text) * (Convert.ToDouble(tbAdjustAge.Text) / 100) * 1000);

                for (int k = 0; k < count; k++)
                {                   

                    // if (dt2.Rows[k]["Id"].ToString()==hnID.Value)
                    if (((Label)GDConsumptionMaster.Rows[k].FindControl("lblId2")).Text == hnID.Value)
                    {
                        string QtyInPer = dt2.Rows[k]["QtyInPer"].ToString();
                        string QtyInGm = tbConsumpGms.Text;
                        //TotalConsumpQty = (TotalConsumpQty - Convert.ToInt32(QtyInPer)) + Convert.ToInt32(QtyInGm);
                        TotalConsumpQty = (TotalConsumpQty - Convert.ToDouble(QtyInPer)) + Convert.ToDouble(QtyInGm);
                        lblTotalConsumpQtyGms.Text = Convert.ToString(TotalConsumpQty);


                        string QtyInPercentage = dt2.Rows[k]["Percentage"].ToString();
                        string QtyIntbPer = tbConsumpAge.Text;
                        //TotalConsumpQty = (TotalConsumpQty - Convert.ToInt32(QtyInPer)) + Convert.ToInt32(QtyInGm);
                        TotalConsumpQtyPercentage = (TotalConsumpQtyPercentage - Convert.ToDouble(QtyInPercentage)) + Convert.ToDouble(QtyIntbPer);
                       lblTotalConsumpQtyPer.Text = Convert.ToString(TotalConsumpQtyPercentage);
                    }
                    if (Math.Round(Convert.ToDouble(lblTotalConsumpQtyGms.Text) + adjust, 3) > Math.Round((Convert.ToDouble(lblConsumpKgSqMt2.Text) * 1000), 3))
                    {
                        flag = "true";
                        ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Consumption Qty cannot be greater than Required Qty');", true);

                        lblTotalConsumpQtyGms.Text = Convert.ToString(Convert.ToDouble(lblTotalConsumpQtyGms.Text) - Convert.ToDouble(tbConsumpGms.Text));
                        lblTotalRemainingQtyGms.Text = Convert.ToString((Convert.ToDouble(lblConsumpKgSqMt2.Text) * 1000) - Convert.ToDouble(lblTotalConsumpQtyGms.Text) - adjust);


                        lblTotalConsumpQtyPer.Text = Convert.ToString(Convert.ToDouble(lblTotalConsumpQtyPer.Text) - Convert.ToDouble(tbConsumpAge.Text));
                        lblTotalRemainingQtyPer.Text = Convert.ToString(Convert.ToDouble(100) - Convert.ToDouble(lblTotalConsumpQtyPer.Text) - Convert.ToDouble(tbAdjustAge.Text == "" ? "0" : tbAdjustAge.Text));
                    }
                    else
                    {
                        ////lblRem.Text = Math.Round((Val(lblKg.Text) * 1000) - Val(lblCon.Text) - Val(TxtAdjust.Tag), 3)
                        lblTotalRemainingQtyGms.Text = Convert.ToString(Math.Round((Convert.ToDouble(lblConsumpKgSqMt2.Text) * 1000) - Convert.ToDouble(lblTotalConsumpQtyGms.Text) - Convert.ToDouble(tbAdjustAge.Text), 3));

                        lblTotalRemainingQtyPer.Text = Convert.ToString(Math.Round(Convert.ToDouble(100) - Convert.ToDouble(lblTotalConsumpQtyPer.Text) - Convert.ToDouble(tbAdjustAge.Text), 3));

                    }
                }

                //For i = 0 To dgColorSize.Rows.Count - 1
                //    If dgColorSize.Item("SrNo", i).Value = Val(cmdSubmit.Tag) Then

                //lblTotalConsumpQtyGms.Text = (lblTotalConsumpQtyGms.Text) - (GDConsumptionMaster.Rows[0]["QtyInPer"]) +tbConsumpGms.Text;

                if (flag == "false")
                {
                    int l = 0;
                    for (l = 0; l < count; l++)
                    {
                        //if (dt2.Rows[l]["Id"].ToString() == hnID.Value)
                        if (((Label)GDConsumptionMaster.Rows[l].FindControl("lblId2")).Text == hnID.Value)
                        {
                            dt2.Rows[l]["DyerColorId"] = ddlShadeColor.SelectedValue;
                            dt2.Rows[l]["DyerColorName"] = ddlShadeColor.SelectedItem.Text;
                            dt2.Rows[l]["QtyInPer"] = tbConsumpGms.Text;
                            dt2.Rows[l]["SubItemId"] = ddlWoolType.SelectedValue;
                            dt2.Rows[l]["ItemName"] = ddlWoolType.SelectedItem.Text;
                            dt2.Rows[l]["OFinishedId"] = 0;
                            dt2.Rows[l]["IFinishedId"] = 0;
                            dt2.Rows[l]["Percentage"] = tbConsumpAge.Text == "" ? "0" : tbConsumpAge.Text;
                            dt2.AcceptChanges();
                        }
                    }

                    ViewState["AddItem"] = dt2;
                    GDConsumptionMaster.DataSource = ViewState["AddItem"];
                    GDConsumptionMaster.DataBind(); 
                }
                        
                           
                
            }
        }
        //DgClk ="false";
        hnStatus.Value = "false";
        if (lblTotalRemainingQtyGms.Text == "0")
        {
            //save();
        }
        else
        {
            // ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Please select shade color');", true);
        }
        tbConsumpAge.Text = "";
        tbConsumpAge_TextChanged(sender, new EventArgs());

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Savedetail();
        //BindGDConsumptionMaster();
        mode = "";
        return;
    }
    protected void Savedetail()
    {
        if (Convert.ToInt32(lblTotalRemainingQtyGms.Text) > 0 || Convert.ToInt32(lblTotalRemainingQtyGms.Text) < 0)
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('Please insert correct calculation for color in gms...')", true);
        }
        else
        {
            //***********************Start Color shade detail size

            //********sql table Type
            DataTable dtrecords = new DataTable();
            dtrecords.Columns.Add("DyerColorId", typeof(int));
            dtrecords.Columns.Add("DyerColorName", typeof(string));
            dtrecords.Columns.Add("QtyInPer", typeof(float));
            dtrecords.Columns.Add("SubItemId", typeof(int));
            dtrecords.Columns.Add("OFinishedId", typeof(int));
            dtrecords.Columns.Add("IFinishedId", typeof(int));
            dtrecords.Columns.Add("Percentage", typeof(float));


            //*******************

            if (ViewState["AddItem"] != null)
            {
                DataTable dt3 = (DataTable)ViewState["AddItem"];
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    //decimal qty = Math.Round(Convert.ToDecimal(dt3.Rows[i]["QtyInPer"].ToString()), 3);
                    decimal qty = Math.Round(Convert.ToDecimal(dt3.Rows[i]["QtyInPer"].ToString()), 4);

                    if (qty > 0)
                    {
                        DataRow dr = dtrecords.NewRow();
                        dr["DyerColorId"] = dt3.Rows[i]["DyerColorId"].ToString();
                        dr["DyerColorName"] = dt3.Rows[i]["DyerColorName"].ToString();
                        dr["QtyInPer"] = dt3.Rows[i]["QtyInPer"].ToString();
                        dr["SubItemId"] = dt3.Rows[i]["SubItemId"].ToString();
                        dr["OFinishedId"] = 0;
                        dr["IFinishedId"] = 0;
                        dr["Percentage"] = dt3.Rows[i]["Percentage"].ToString();
                        dtrecords.Rows.Add(dr);
                    }
                }
            }

            //***********************End Color shade detail size

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[9];
                param[0] = new SqlParameter("@Id", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = hnID2.Value == "" ? "0" : hnID2.Value;
                param[1] = new SqlParameter("@QualityId", ddlQuality.SelectedIndex > 0 ? ddlQuality.SelectedValue : "0");
                param[2] = new SqlParameter("@DesignId", ddlDesignName.SelectedIndex > 0 ? ddlDesignName.SelectedValue : "0");
                param[3] = new SqlParameter("@ColorId", ddlColorName.SelectedIndex > 0 ? ddlColorName.SelectedValue : "-1");
                param[4] = new SqlParameter("@SizeId", ddlSizeShape.SelectedIndex > 0 ? ddlSizeShape.SelectedValue : "0");
                param[5] = new SqlParameter("@AdjustInPer", tbAdjustAge.Text == "" ? "0" : tbAdjustAge.Text);
                param[6] = new SqlParameter("@dtrecords", dtrecords);
                param[7] = new SqlParameter("@MasterCompanyId", Convert.ToInt32(Session["varCompanyId"]));
                param[8] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[8].Direction = ParameterDirection.Output;
               

                //**********
                //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveDesignRatioSizeWise", param);
                DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_SaveDesignRatioSizeWise", param);

                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    if(tbEffectiveDate.Text==ds.Tables[0]
                //}

                lblMessage.Text = param[8].Value.ToString();

                Tran.Commit();
                mode = "Insert";
                Refreshcontrol();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void Refreshcontrol()
    {
        //ddlQualityType.SelectedValue = "0";       
        //ddlQuality.SelectedValue = "0";
        //ddlDesignName.SelectedValue = "0";
        //ddlColorName.SelectedValue = "-1";
        //ddlSizeShape.SelectedValue = "0";
        //ddlShadeColor.SelectedValue = "0";

       // ddlColorName.SelectedValue = "-1";

        if (ddlColorName.Items.Count > 0)
        {
            ddlColorName.SelectedIndex = 0;
        }
        ddlShadeColor.SelectedValue = "0";
        ddlWoolType.SelectedValue = "0";

        GDConsumptionMaster.DataSource = "";
        GDConsumptionMaster.DataBind();
        lblConsumpKgSqMt2.Text = "";
        lblTotalConsumpQtyGms.Text = "";
        lblTotalConsumpQtyPer.Text = "";
        lblTotalRemainingQtyGms.Text = "";
        lblTotalRemainingQtyPer.Text = "";
        tbAdjustAge.Text = "";

        ViewState["AddItem"] = null;

        hnColorId.Value = "";
        hnSubItemId.Value = "";
        hnID.Value = "";
        hnID2.Value = "";
        hnStatus.Value = "false"; 
        tbConsumpAge.Text = "";
        tbConsumpGms.Text = "";

    }
    protected void GDConsumptionMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GDConsumptionMaster, "select$" + e.Row.RowIndex);
        }
    }
    protected void GDConsumptionMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (GDConsumptionMaster.Rows.Count > 0)
        {
            //DgClk = "true";
            hnStatus.Value = "true";
            flag = "false";

            DataTable dt2 = new DataTable();
            dt2 = (DataTable)ViewState["AddItem"];

            // string id = (GDConsumptionMaster.SelectedRow.FindControl("lblId2") as Label).Text;

            ////string id = GDConsumptionMaster.SelectedDataKey.Value.ToString();
            //hnID.Value = id;

            string id = (GDConsumptionMaster.SelectedRow.FindControl("lblId2") as Label).Text;
            string lblDyerColorId = (GDConsumptionMaster.SelectedRow.FindControl("lblDyerColorId") as Label).Text;
            string lblSubItemId = (GDConsumptionMaster.SelectedRow.FindControl("lblSubItemId") as Label).Text;
            hnColorId.Value = lblDyerColorId;
            hnSubItemId.Value = lblSubItemId;
            hnID.Value = id;

            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                if (lblDyerColorId == dt2.Rows[i]["DyerColorId"].ToString() && lblSubItemId == dt2.Rows[i]["SubItemId"].ToString())
                {
                    ddlShadeColor.SelectedValue = dt2.Rows[i]["DyerColorId"].ToString();
                    tbConsumpGms.Text = dt2.Rows[i]["QtyInPer"].ToString();
                   // tbConsumpAge.Text = dt2.Rows[i]["Percentage"].ToString();
                    ddlWoolType.SelectedValue = dt2.Rows[i]["SubItemId"].ToString();
                    double ConsumpAge = ((Convert.ToDouble(dt2.Rows[i]["QtyInPer"]) * 100) / (1000 * Convert.ToDouble(lblConsumpKgSqMt2.Text)));
                    tbConsumpAge.Text = Convert.ToString(Math.Round((ConsumpAge), 3));
                    tbConsumpAge_TextChanged(sender, new EventArgs());
                    //DgClk = true;
                }
            }
        }
        else
        {
            hnStatus.Value = "false";
        }
    }
    protected void tbConsumpAge_TextChanged(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(tbConsumpAge.Text))
        {
            tbConsumpAge.Text = "0";
        }

        // if (Convert.ToInt32(tbConsumpAge.Text) > 0)
        if (Convert.ToDouble(tbConsumpAge.Text) > 0)
        {
            flag = "false";
            tbConsumpGms.Text = "";
            double consumpgms = Convert.ToDouble((Convert.ToDouble(lblConsumpKgSqMt2.Text) * (Convert.ToDouble(tbConsumpAge.Text)) / 100) * 1000);
            tbConsumpGms.Text = Convert.ToString(Math.Round((consumpgms), 3));
           // tbConsumpGms.Text = Convert.ToString(consumpgms);
        }
        else
        {
            tbConsumpGms.Text = "";
        }
    }
    protected void tbConsumpGms_TextChanged(object sender, EventArgs e)
    {       

        if (string.IsNullOrWhiteSpace(tbConsumpGms.Text))
        {
            tbConsumpGms.Text = "0";
        }
        if (Convert.ToDouble(tbConsumpGms.Text) > 0)
        {
            flag = "false";
            tbConsumpAge.Text = "";
            double consumpper = ((Convert.ToDouble(tbConsumpGms.Text) * 100) / (1000 * Convert.ToDouble(lblConsumpKgSqMt2.Text)));
            tbConsumpAge.Text = Convert.ToString(Math.Round((consumpper), 3));
            //tbConsumpAge.Text = Convert.ToString(consumpper);
            //tbConsumpAge_TextChanged(sender, new EventArgs());
        }
        else
        {
            tbConsumpGms.Text = "";
        }

    }
    protected void GDConsumptionMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GDConsumptionMaster.PageIndex = e.NewPageIndex;
        //BindGDConsumptionMaster();
    }

    protected void GDConsumptionMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string id = ((Label)GDConsumptionMaster.Rows[e.RowIndex].FindControl("lblId2")).Text;

        string Id2 = ((Label)GDConsumptionMaster.Rows[e.RowIndex].FindControl("lblId")).Text;
        string lblDyerColorId = ((Label)GDConsumptionMaster.Rows[e.RowIndex].FindControl("lblDyerColorId")).Text;
        string lblSubItemId = ((Label)GDConsumptionMaster.Rows[e.RowIndex].FindControl("lblSubItemId")).Text;       
       
            DataTable dt1 = (DataTable)ViewState["AddItem"];
            dt1.Rows[e.RowIndex].Delete();
            dt1.AcceptChanges();

            double TotalConsumpQty2 = 0;
            double TotalConsumpQtyPer = 0;
            int n = 0;
            for (n = 0; n < dt1.Rows.Count; n++)
            {
                TotalConsumpQty2 += Convert.ToDouble(dt1.Rows[n]["QtyInPer"].ToString());
                //TotalConsumpQty += TotalConsumpQty;
                lblTotalConsumpQtyGms.Text = Convert.ToString(Math.Round((TotalConsumpQty2), 3));

                TotalConsumpQtyPer += Convert.ToDouble(dt1.Rows[n]["Percentage"].ToString());
                //TotalConsumpQty += TotalConsumpQty;
                lblTotalConsumpQtyPer.Text = Convert.ToString(Math.Round((TotalConsumpQtyPer), 3));
            }

            if (dt1.Rows.Count == 0)
            {
                TotalConsumpQty2 += 0;
                //TotalConsumpQty += TotalConsumpQty;
                lblTotalConsumpQtyGms.Text = Convert.ToString(Math.Round((TotalConsumpQty2), 3));

                TotalConsumpQtyPer += 0;
                lblTotalConsumpQtyPer.Text = Convert.ToString(Math.Round((TotalConsumpQtyPer), 3));

            }
            //// lblRem.Text = (Val(lblKg.Text) * 1000) - Val(lblCon.Text)
            //lblTotalRemainingQtyGms.Text = Convert.ToString(Math.Round((Convert.ToDouble(lblConsumpKgSqMt2.Text) * 1000) - Convert.ToDouble(lblTotalConsumpQtyGms.Text), 3));
            lblTotalRemainingQtyGms.Text = Convert.ToString(Math.Round((Convert.ToDouble(lblConsumpKgSqMt2.Text) * 1000) - Convert.ToDouble(lblTotalConsumpQtyGms.Text), 3));

            lblTotalRemainingQtyPer.Text = Convert.ToString(Math.Round(Convert.ToDouble(100) - Convert.ToDouble(lblTotalConsumpQtyPer.Text), 3));


            if (hnEditStatus.Value == "True")
            {
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlTransaction Tran = con.BeginTransaction();
                try
                {
                    SqlParameter[] param = new SqlParameter[4];
                    param[0] = new SqlParameter("@Id", Id2);
                    param[1] = new SqlParameter("@DyerColorId", lblDyerColorId);
                    param[2] = new SqlParameter("@SubItemId", lblSubItemId);
                   
                    param[3] = new SqlParameter("@Message", SqlDbType.VarChar, 100);
                    param[3].Direction = ParameterDirection.Output;


                    //**********
                    //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveDesignRatioSizeWise", param);
                    DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_DeleteDesignRatioSizeWise", param);

                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    //    if(tbEffectiveDate.Text==ds.Tables[0]
                    //}

                    lblMessage.Text = param[3].Value.ToString();

                    Tran.Commit(); 
                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                    Tran.Rollback();
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }         

            dt1.AcceptChanges();

            GDConsumptionMaster.DataSource = dt1;
            GDConsumptionMaster.DataBind();
            checkDeleteButton();
            ViewState["AddItem"] = dt1;
       

    }

    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        Session["ReportPath"] = "Reports/DesignRatioSizeWiseReport.rpt";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {

            SqlParameter[] _arrPara = new SqlParameter[4];
            _arrPara[0] = new SqlParameter("@QualityId", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@DesignId", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@ColorId", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@SizeId", SqlDbType.Int);

            _arrPara[0].Value = ddlQuality.SelectedValue == "" ? "0" : ddlQuality.SelectedValue;
            _arrPara[1].Value = ddlDesignName.SelectedValue == "" ? "0" : ddlDesignName.SelectedValue;
            _arrPara[2].Value = ddlColorName.SelectedValue == "" ? "0" : ddlColorName.SelectedValue;
            _arrPara[3].Value = ddlSizeShape.SelectedValue == "" ? "0" : ddlSizeShape.SelectedValue;

            DataSet ds = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, "pro_GetAllDesignRatioSizeWiseReportData", _arrPara);

            Session["dsFileName"] = "~\\ReportSchema\\DesignRatioSizeWiseReport.xsd";
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = Session["ReportPath"];
                Session["GetDataset"] = ds;
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
            tran.Rollback();
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void refreshitemdr_Click(object sender, EventArgs e)
    {
        BindQualityType();
        
    }
    protected void refreshqualitydr_Click(object sender, EventArgs e)
    {
        BindQualityName();
    }
    protected void refreshshadecolor_Click(object sender, EventArgs e)
    {
        BindShadeColor();
    }
    
}