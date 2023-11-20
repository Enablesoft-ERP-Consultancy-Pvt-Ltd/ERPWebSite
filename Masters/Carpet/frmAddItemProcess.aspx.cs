using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using IExpro.Core.Common;
using IExpro.Web.Models;
using System.Windows.Forms;
using System.Xml;
using System.ServiceModel.Activities;
using DocumentFormat.OpenXml.Office.Word;
using IExpro.Core.Models;
using System.Windows.Interop;

public partial class Masters_Process_frmAddItemProcess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {


            var result = ((ProcessType[])Enum.GetValues(typeof(ProcessType))).Select(x => new SelectedList { ItemId = (int)x, ItemName = x.ToString() });

            rdbtnLst.DataSource = result;
            rdbtnLst.DataBind();
            rdbtnLst.SelectedIndex = 0;




            UtilityModule.ConditonalListFill(ref lstProcess, "select Process_name_Id,Process_Name from Process_name_Master order by Process_Name");
            lblItemName.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Item_Name from Item_master Where Item_id=" + Request.QueryString["a"] + " And MasterCompanyId=" + Session["varcompanyId"] + "").ToString();
            switch (Session["varcompanyid"].ToString())
            {
                case "8":
                    TDquality.Visible = false;
                    Fillselectprocess();
                    break;
                default:
                    TDquality.Visible = true;
                    TDDesign.Visible = true;
                    break;





            }
            //
            if (TDquality.Visible == true)
            {
                UtilityModule.ConditionalComboFill(ref DDQuality, "select QualityId,QualityName From Quality Where Item_Id=" + Request.QueryString["a"] + " order by QualityName", true, "--Plz Select--");
            }
        }
    }
    protected void Fillselectprocess()
    {
        SqlConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        SqlParameter[] param = new SqlParameter[6];

        param[0] = new SqlParameter("@Flag", SqlDbType.Int);
        param[0].Direction = ParameterDirection.Input;
        param[0].Value = 2;

        param[1] = new SqlParameter("@CompanyId", SqlDbType.VarChar);
        param[1].Direction = ParameterDirection.Input;
        param[1].Value = Convert.ToInt32(Session["varcompanyid"]);

        param[2] = new SqlParameter("@ItemId", SqlDbType.Int);
        param[2].Direction = ParameterDirection.Input;
        param[2].Value = Convert.ToInt32(Request.QueryString["a"]);

        param[3] = new SqlParameter("@QualityId", SqlDbType.Int);
        param[3].Direction = ParameterDirection.Input;
        param[3].Value = Convert.ToInt32(DDQuality.SelectedValue);

        param[4] = new SqlParameter("@DesignId", SqlDbType.Int);
        param[4].Direction = ParameterDirection.Input;
        param[4].Value = Convert.ToInt32(DDDesign.SelectedValue);


        param[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 200);
        param[5].Direction = ParameterDirection.Output;




        List<ItemList> lstselected = new List<ItemList>();

        using (var reader = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, "ProcessSequenceProc", param))
        {
            while (reader.Read())
            {
                ItemList _obj = new ItemList();

                _obj.Index = reader["SeqNo"] != null ? Convert.ToInt32(reader["SeqNo"]) : 0;
                int ProcessId = reader["process_Name_id"] != null ? Convert.ToInt32(reader["process_Name_id"]) : 0;
                int _processType = reader["ProcessType"] != null ? Convert.ToInt32(reader["ProcessType"]) : 0;
                _obj.ItemId = reader["ProcessType"].ToString() + '-' + reader["process_Name_id"].ToString();
                _obj.ItemName = ((ProcessType)_processType).ToString() + '-' + reader["Process_Name"].ToString();
                lstselected.Add(_obj);
            }
            lstSelectProcess.DataSource = lstselected;
            lstSelectProcess.DataValueField = "ItemId";
            lstSelectProcess.DataTextField = "ItemName";
            lstSelectProcess.DataBind();
        }




    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < lstProcess.Items.Count; i++)
        {
            if (lstProcess.Items[i].Selected)
            {
                //Check if process Already Exists
                if (!lstSelectProcess.Items.Contains(lstProcess.Items[i]))
                {

                    var lstItem = new ListItem('(' + rdbtnLst.SelectedItem.Text + ") " + lstProcess.Items[i].Text, rdbtnLst.SelectedValue + '-' + lstProcess.Items[i].Value);
                    //lstItem.Attributes.Add("ProcessType", rdbtnLst.SelectedValue); 
                    lstSelectProcess.Items.Add(lstItem);
                }
            }
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        List<ListItem> lstselected = new List<ListItem>();

        foreach (ListItem liItems in lstSelectProcess.Items)
        {
            if (liItems.Selected)
            {
                lstselected.Add(liItems);
            }
        }

        //3. Loop through the List "lstSelected" and
        // remove ListItems from ListBox "lstSelectProcess" that are in 
        // lstSelected List
        foreach (ListItem liSelected in lstselected)
        {
            lstSelectProcess.Items.Remove(liSelected);
        }

    }
    protected void btnsave_Click(object sender, EventArgs e)
    {


        XmlDocument doc = new XmlDocument();
        XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("ProcessItems"));
        for (int i = 0; i < lstSelectProcess.Items.Count; i++)
        {
            XmlElement elm = doc.CreateElement("ProcessItem");
            elm.InnerText = lstSelectProcess.Items[i].Text;
            var itemValue = lstSelectProcess.Items[i].Value.Split('-');
            int seqNo = i + 1;
            elm.SetAttribute("SeqNO", seqNo.ToString());
            elm.SetAttribute("ProcessType", itemValue[0]);
            elm.SetAttribute("ProcessId", itemValue[1]);
            el.AppendChild(elm);
        }

        Console.WriteLine();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] array = new SqlParameter[8];
            array[0] = new SqlParameter("@Flag", SqlDbType.Int);
            array[1] = new SqlParameter("@CreatedBy", SqlDbType.Int);
            array[2] = new SqlParameter("@CompanyId", SqlDbType.Int);
            array[3] = new SqlParameter("@QualityId", SqlDbType.Int);
            array[4] = new SqlParameter("@DesignId", SqlDbType.Int);
            array[5] = new SqlParameter("@ProcessXml", SqlDbType.Xml);
            array[6] = new SqlParameter("@Msg", SqlDbType.VarChar, 200);
            array[7] = new SqlParameter("@ItemId", SqlDbType.Int);

            array[0].Value = 1;
            array[1].Value = Session["varuserid"];
            array[2].Value = Session["varcompanyId"];
            array[3].Value = Convert.ToInt32(DDQuality.SelectedValue);
            array[4].Value = Convert.ToInt32(DDDesign.SelectedValue);
            array[5].Value = doc.OuterXml;
            array[6].Direction = ParameterDirection.Output;

            array[7].Value = Request.QueryString["a"];
            //Save Data
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "ProcessSequenceProc", array);
            Tran.Commit();
            lblMessage.Text = array[6].Value.ToString();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TDDesign.Visible == true)
        {
            FillDesign();
        }

        Fillselectprocess();
    }

    private void FillDesign()
    {
        string str = "select Distinct designId,designName From V_FinisheditemDetail  Where QualityId=" + DDQuality.SelectedValue + " order by designName";
        UtilityModule.ConditionalComboFill(ref DDDesign, str, true, "--Plz Select--");
    }

    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillselectprocess();
    }



















}