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
        string str = @"select PNM.process_Name_id,PNM.Process_Name from Process_name_Master PNM,Item_Process IP
                      Where PNM.Process_name_id=IP.ProcessId And ItemId=" + Request.QueryString["a"] + " And PNM.MasterCompanyid=" + Session["varcompanyid"] + "";
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and IP.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and IP.DesignId=" + DDDesign.SelectedValue;
        }
        else
        {
            str = str + " and IP.DesignId=0";
        }
        str = str + "  order by IP.SeqNo";
        UtilityModule.ConditonalListFill(ref lstSelectProcess, str);
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
            elm.SetAttribute("ProcessType", itemValue[0]);
            elm.SetAttribute("ProcessId", itemValue[1]);
            el.AppendChild(elm);
        }

        Console.WriteLine(doc.OuterXml);
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] array = new SqlParameter[8];
            array[0] = new SqlParameter("@Itemid", SqlDbType.Int);
            array[1] = new SqlParameter("@UserId", SqlDbType.Int);
            array[2] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            array[3] = new SqlParameter("@ProcessId_SeqNO", SqlDbType.VarChar, 100);
            array[4] = new SqlParameter("@Msg", SqlDbType.VarChar, 50);
            array[5] = new SqlParameter("@QualityId", SqlDbType.Int);
            array[6] = new SqlParameter("@DesignId", SqlDbType.Int);
            array[7] = new SqlParameter("@ProcessType", SqlDbType.TinyInt);
            array[0].Value = Request.QueryString["a"];
            array[1].Value = Session["varuserid"];
            array[2].Value = Session["varcompanyId"];
            array[7].Value = rdbtnLst.SelectedValue;
            string str = "";
            string strnew = "";
            // find ProcessId And SeqNo
            int seqNo = 0;
            for (int i = 0; i < lstSelectProcess.Items.Count; i++)
            {

                seqNo += 1;
                str = lstSelectProcess.Items[i].Value + "," + seqNo;
                if (strnew == "")
                {
                    strnew = str;
                }
                else
                {
                    strnew = strnew + "|" + str;

                }

            }
            array[3].Value = strnew;
            array[4].Direction = ParameterDirection.Output;
            array[5].Value = TDquality.Visible == false ? "0" : DDQuality.SelectedValue;
            array[6].Value = TDDesign.Visible == false ? "0" : (DDDesign.SelectedIndex > 0 ? DDDesign.SelectedValue : "0");
            //Save Data
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveItem_Process", array);
            Tran.Commit();
            lblMessage.Text = array[4].Value.ToString();
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





    protected override object SaveViewState()
    {
        // create object array for Item count + 1
        object[] allStates = new object[this.Items.Count + 1];

        // the +1 is to hold the base info
        object baseState = base.SaveViewState();
        allStates[0] = baseState;

        Int32 i = 1;
        // now loop through and save each Style attribute for the List
        foreach (ListItem li in this.Items)
        {
            Int32 j = 0;
            string[][] attributes = new string[li.Attributes.Count][];
            foreach (string attribute in li.Attributes.Keys)
            {
                attributes[j++] = new string[] { attribute, li.Attributes[attribute] };
            }
            allStates[i++] = attributes;
        }
        return allStates;
    }

    protected override void LoadViewState(object savedState)
    {
        if (savedState != null)
        {
            object[] myState = (object[])savedState;

            // restore base first
            if (myState[0] != null)
                base.LoadViewState(myState[0]);

            Int32 i = 1;
            foreach (ListItem li in this.Items)
            {
                // loop through and restore each style attribute
                foreach (string[] attribute in (string[][])myState[i++])
                {
                    li.Attributes[attribute[0]] = attribute[1];
                }
            }
        }
    }















}