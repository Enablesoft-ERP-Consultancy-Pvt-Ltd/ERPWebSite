using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Packing_frmpackingRegister : System.Web.UI.Page
{
    static int MaxRollNo = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            //SqlhelperEnum.FillDropDown(AllEnums.MasterTables.Units, ddUnits, pID: "UnitsId", pName: "Unitname");
            UtilityModule.ConditionalComboFill(ref ddUnits, "select U.UnitsId,U.UnitName from Units U inner join Units_authentication UA on U.unitsId=UA.UnitsId and UA.Userid=" + Session["varuserid"] + " order by U.unitsId", true, "");
            MaxRollNo = 0;
        }
    }
    protected void txtStockNo_TextChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            if (txtStockNo.Text != "")
            {
                lblErrorMessage.Text = "";
                SqlParameter[] array = new SqlParameter[3];
                array[0] = new SqlParameter("@MaxRollNo", SqlDbType.Int);
                array[1] = new SqlParameter("@Date", SqlDbType.SmallDateTime);
                array[2] = new SqlParameter("@TStockNo", SqlDbType.VarChar, 30);
                

                array[0].Value = MaxRollNo;
                array[1].Value = System.DateTime.Now.ToString("dd-MMM-yyyy");
                array[2].Value = txtStockNo.Text;
                

                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Pro_fillforPackingRegister", array);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["CompanyID"]) == Convert.ToInt16(Session["CurrentWorkingCompanyID"]))
                    {
                        lblErrorMessage.Text = " This Stock no. is not belongs to this company... ";
                        lblArticlevalue.Text = "";
                        lblItemDetail.Text = "";
                        return;
                    }
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["Pack"]) == 1)
                    {
                        lblErrorMessage.Text = " This Stock No. is already packed... ";
                        lblArticlevalue.Text = "";
                        lblItemDetail.Text = "";
                        return;
                    }
                    else if (Convert.ToInt16(ds.Tables[0].Rows[0]["IssRecStatus"]) == 1)
                    {
                        lblErrorMessage.Text = " This Stock Number Status is Issued ... ";
                        lblArticlevalue.Text = "";
                        lblItemDetail.Text = "";
                        return;
                    }
                    else if (Convert.ToInt16(ds.Tables[0].Rows[0]["CurrentProstatus"]) != Convert.ToInt16(ds.Tables[0].Rows[0]["ItemProcessid"]))
                    {
                        lblErrorMessage.Text = "The last process should be " + ds.Tables[0].Rows[0]["ItemProcessName"] + " to pack this stockNo.Current Process status is " + ds.Tables[0].Rows[0]["Process_Name"] + ".";
                        lblArticleNo.Text = "";
                        lblItemDetail.Text = "";
                        return;
                    }
                    else
                    {
                        lblArticlevalue.Text = ds.Tables[0].Rows[0]["Articleno"].ToString();
                        lblItemDetail.Text = ds.Tables[0].Rows[0]["Item"].ToString();
                        DGPackingRegister.DataSource = ds.Tables[0];
                        DGPackingRegister.DataBind();
                        txtStockNo.Text = "";
                    }
                }
                else
                {
                    lblErrorMessage.Text = "Stock No. is not available in Finishing....";
                }

            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void DGPackingRegister_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "Save")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                int i = gvr.RowIndex;
                SqlParameter[] array = new SqlParameter[25];
                array[0] = new SqlParameter("@packingRegisterId", SqlDbType.Int);
                array[1] = new SqlParameter("@RollNo", SqlDbType.Int);
                array[2] = new SqlParameter("@TStockNo", SqlDbType.VarChar, 100);
                array[3] = new SqlParameter("@Item_Finished_Id", SqlDbType.Int);
                array[4] = new SqlParameter("@LayFlat", SqlDbType.VarChar, 20);
                array[5] = new SqlParameter("@Hand_FeetFeel", SqlDbType.VarChar, 20);
                array[6] = new SqlParameter("@Smell", SqlDbType.VarChar, 20);
                array[7] = new SqlParameter("@Colourvariation", SqlDbType.VarChar, 20);
                array[8] = new SqlParameter("@Stain", SqlDbType.VarChar, 20);
                array[9] = new SqlParameter("@Moisture", SqlDbType.VarChar, 20);
                array[10] = new SqlParameter("@TotalAppearance", SqlDbType.VarChar, 20);
                array[11] = new SqlParameter("@ClarityofDesign", SqlDbType.VarChar, 20);
                array[12] = new SqlParameter("@Insect", SqlDbType.VarChar, 20);
                array[13] = new SqlParameter("@WeavingDefect", SqlDbType.VarChar, 20);
                array[14] = new SqlParameter("@Weightgross", SqlDbType.Float);
                array[15] = new SqlParameter("@WeightNet", SqlDbType.Float);
                array[16] = new SqlParameter("@Fringes", SqlDbType.VarChar, 20);
                array[17] = new SqlParameter("@ActualSize", SqlDbType.VarChar, 50);
                array[18] = new SqlParameter("@Binding", SqlDbType.VarChar, 20);
                array[19] = new SqlParameter("@Date_Stamp", SqlDbType.VarChar, 50);
                array[20] = new SqlParameter("@UnitsId", SqlDbType.Int);
                array[21] = new SqlParameter("@UserId", SqlDbType.Int);
                array[22] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                array[23] = new SqlParameter("@CompanyId", SqlDbType.Int);
                array[24] = new SqlParameter("@TableNo", SqlDbType.Int);

                //Find out DropDown in a Gridview
                DropDownList DDlayFlat = ((DropDownList)DGPackingRegister.Rows[i].FindControl("DDLayFlat"));
                DropDownList DDHandfeetfeel = ((DropDownList)DGPackingRegister.Rows[i].FindControl("DDHandfeetfeel"));
                DropDownList DDSmell = ((DropDownList)DGPackingRegister.Rows[i].FindControl("DDSmell"));
                DropDownList DDColourVariation = ((DropDownList)DGPackingRegister.Rows[i].FindControl("DDColourVariation"));
                DropDownList DDStain = ((DropDownList)DGPackingRegister.Rows[i].FindControl("DDStain"));
                DropDownList DDMoisture = ((DropDownList)DGPackingRegister.Rows[i].FindControl("DDMoisture"));
                DropDownList DDTotalAppearance = ((DropDownList)DGPackingRegister.Rows[i].FindControl("DDTotalAppearance"));
                DropDownList DDClarityofDesign = ((DropDownList)DGPackingRegister.Rows[i].FindControl("DDClarityofDesign"));
                DropDownList DDInsect = ((DropDownList)DGPackingRegister.Rows[i].FindControl("DDInsect"));
                DropDownList DDWeavingDefect = ((DropDownList)DGPackingRegister.Rows[i].FindControl("DDWeavingDefect"));
                DropDownList DDFringes = ((DropDownList)DGPackingRegister.Rows[i].FindControl("DDFringes"));
                DropDownList DDBinding = ((DropDownList)DGPackingRegister.Rows[i].FindControl("DDBinding"));
                TextBox txtRollNo = ((TextBox)DGPackingRegister.Rows[i].FindControl("txtRollNo"));
                TextBox txtTStockNo = ((TextBox)DGPackingRegister.Rows[i].FindControl("txtStockNo"));
                TextBox txtWeightGross = ((TextBox)DGPackingRegister.Rows[i].FindControl("txtWeightGross"));
                TextBox txtWeightNet = ((TextBox)DGPackingRegister.Rows[i].FindControl("txtWeightNet"));
                TextBox txtDateStamp = ((TextBox)DGPackingRegister.Rows[i].FindControl("txtDateStamp"));
                TextBox txtActualSize = ((TextBox)DGPackingRegister.Rows[i].FindControl("txtActualSize"));
                TextBox txtwidth = ((TextBox)DGPackingRegister.Rows[i].FindControl("txtwidth"));
                TextBox txtLength = ((TextBox)DGPackingRegister.Rows[i].FindControl("txtlength"));
                TextBox txttableno = ((TextBox)DGPackingRegister.Rows[i].FindControl("txttableno"));

                array[0].Direction = ParameterDirection.InputOutput;
                if (ViewState["packingRegisterId"] == null)
                {
                    ViewState["packingRegisterId"] = 0;
                }
                array[0].Value = ViewState["packingRegisterId"];
                array[1].Value = txtRollNo.Text == "" ? "0" : txtRollNo.Text;
                array[2].Value = txtTStockNo.Text;
                array[3].Value = DGPackingRegister.DataKeys[i].Value.ToString();
                array[4].Value = DDlayFlat.SelectedItem.Text;
                array[5].Value = DDHandfeetfeel.SelectedItem.Text;
                array[6].Value = DDSmell.SelectedItem.Text;
                array[7].Value = DDColourVariation.SelectedItem.Text;
                array[8].Value = DDStain.SelectedItem.Text;
                array[9].Value = DDMoisture.SelectedItem.Text;
                array[10].Value = DDTotalAppearance.SelectedItem.Text;
                array[11].Value = DDClarityofDesign.SelectedItem.Text;
                array[12].Value = DDInsect.SelectedItem.Text;
                array[13].Value = DDWeavingDefect.SelectedItem.Text;
                array[14].Value = txtWeightGross.Text == "" ? "0" : txtWeightGross.Text;
                array[15].Value = txtWeightNet.Text == "" ? "0" : txtWeightNet.Text;
                array[16].Value = DDFringes.SelectedItem.Text;
                array[17].Value = txtwidth.Text + 'x' + txtLength.Text;
                //array[17].Value = txtActualSize.Text;
                array[18].Value = DDBinding.SelectedItem.Text;
                array[19].Value = txtDateStamp.Text;
                array[20].Value = ddUnits.SelectedValue;
                array[21].Value = Session["varuserid"];
                array[22].Value = Session["Varcompanyid"];
                array[23].Value = Session["CurrentWorkingCompanyID"];//fix companyid
                array[24].Value = txttableno.Text == "" ? "0" : txttableno.Text;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SavePackingregister", array);
                Tran.Commit();
                ViewState["packingRegisterId"] = array[0].Value.ToString();
                lblErrorMessage.Text = "Data Saved Successfully.....";
                DGPackingRegister.DataSource = null;
                DGPackingRegister.DataBind();
                lblArticlevalue.Text = "";
                lblItemDetail.Text = "";
                txtStockNo.Text = "";
                txtStockNo.Focus();
                if (MaxRollNo == 0)
                {
                    MaxRollNo = Convert.ToInt16(txtRollNo.Text) + 1;
                }
                else
                {
                    MaxRollNo += 1;
                }
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblErrorMessage.Text = ex.Message;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
        }
    }    
}
