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
using IExpro.Core.Interfaces.Service;
using IExpro.Infrastructure.Repository;
using IExpro.Infrastructure.Services;
using IExpro.Web.Pages;
using IExpro.Web.Helper;
using IExpro.Core.Models;
public partial class Masters_Process_frmAddItemProcess : BasePopUpPage
{
    const string itemProcessList = "ProcessList";
    public List<ItemProcessModel> ProcessList
    {
        get
        {
            // check if not exist to make new (normally before the post back)
            // and at the same time check that you did not use the same viewstate for other object
            if (!(ViewState[itemProcessList] is List<ItemProcessModel>))
            {
                // need to fix the memory and added to viewstate
                ViewState[itemProcessList] = new List<ItemProcessModel>();
            }
            return (List<ItemProcessModel>)ViewState[itemProcessList];
        }
    }
    public static int IExproId { get; set; }
    public static int CompanyId { get; set; }
    public static long UserId { get; set; }
    public static int ItemId { get; set; }
    static ICommonService comnSrv;
    static IProcessService procSrv;
    public Masters_Process_frmAddItemProcess()
    {
        comnSrv = new CommonService(new UnitOfWork());
        procSrv = new ProcessService(new UnitOfWork());
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Masters_Process_frmAddItemProcess.IExproId = User.IExproId;
            Masters_Process_frmAddItemProcess.CompanyId = User.CompanyId;
            Masters_Process_frmAddItemProcess.UserId = User.UserId;
            Masters_Process_frmAddItemProcess.ItemId = string.IsNullOrEmpty(Request.QueryString["a"]) ? 0 : Convert.ToInt32(Request.QueryString["a"]);
            BindProcessType(0);
            BindProcessList(Masters_Process_frmAddItemProcess.ItemId);
            BindItemProcessList(this.ProcessList);
        }
    }
    void BindProcessList(int itemId)
    {
        var result = procSrv.GetProcessList(Masters_Process_frmAddItemProcess.IExproId).Select(x => new SelectList
        {
            ItemId = x.ProcessId,
            ItemName = x.ProcessName + "(" + x.ShortName + ")"
        });
        lstProcess.BindList(result);
        ddlItemList.BindList(comnSrv.GetItemList(Masters_Process_frmAddItemProcess.IExproId));
        ddlItemList.SelectedValue = itemId.ToString();
        ddlItemList.Enabled = false;
        ddlQuality.BindList(comnSrv.GetQualityList(itemId));
        int qualityId = string.IsNullOrEmpty(ddlQuality.SelectedValue) ? 0 : Convert.ToInt32(ddlQuality.SelectedValue);
        ddlDesign.BindList(comnSrv.GetDesignList(qualityId));
        int designId = string.IsNullOrEmpty(ddlDesign.SelectedValue) ? 0 : Convert.ToInt32(ddlDesign.SelectedValue);
        var itemResult = procSrv.GetItemProcessList(Masters_Process_frmAddItemProcess.IExproId, itemId, qualityId, designId);
        ViewState[itemProcessList] = itemResult.ToList();

    }
    void BindItemProcessList(List<ItemProcessModel> lst)
    {
        var items = lst.Select(x => new SelectList
        {
            ItemId = x.ProcessId,
            ItemName = x.ProcessName + "(" + x.ProcessType + ")"
        });
        lstSelectProcess.BindList(items);
    }
    void BindProcessType(int index)
    {
        var result = ((ProcessType[])Enum.GetValues(typeof(ProcessType))).Select(x => new SelectedList { ItemId = (int)x, ItemName = x.ToString() });
        rdbtnLst.DataSource = result;
        rdbtnLst.DataBind();
        rdbtnLst.SelectedIndex = index;
    }

    protected void ddlItemList_SelectedIndexChanged(object sender, EventArgs e)
    {
        int itemId = string.IsNullOrEmpty(ddlItemList.SelectedValue) ? 0 : Convert.ToInt32(ddlItemList.SelectedValue);
        ddlQuality.BindList(comnSrv.GetQualityList(itemId));
        int qualityId = string.IsNullOrEmpty(ddlQuality.SelectedValue) ? 0 : Convert.ToInt32(ddlQuality.SelectedValue);
        ddlDesign.BindList(comnSrv.GetDesignList(qualityId));
        BindItemProcessList(this.ProcessList);
    }

    protected void ddlQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        int qualityId = string.IsNullOrEmpty(ddlQuality.SelectedValue) ? 0 : Convert.ToInt32(ddlQuality.SelectedValue);
        ddlDesign.BindList(comnSrv.GetDesignList(qualityId));
        BindItemProcessList(this.ProcessList);
    }
    protected void ddlDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindItemProcessList(this.ProcessList);
    }

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    if (Session["varcompanyId"] == null)
    //    {
    //        Response.Redirect("~/Login.aspx");
    //    }
    //    if (!IsPostBack)
    //    {
    //        BindProcessType(0);
    //        UtilityModule.ConditonalListFill(ref lstProcess, "select Process_name_Id,Process_Name from Process_name_Master order by Process_Name");
    //        lblItemName.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Item_Name from Item_master Where Item_id=" + Request.QueryString["a"] + " And MasterCompanyId=" + Session["varcompanyId"] + "").ToString();

    //        switch (Session["varcompanyid"].ToString())
    //        {
    //            case "8":
    //                TDquality.Visible = false;
    //                Fillselectprocess();
    //                break;
    //            default:
    //                TDquality.Visible = true;
    //                TDDesign.Visible = true;
    //                break;
    //        }
    //        //
    //        if (TDquality.Visible == true)
    //        {
    //            UtilityModule.ConditionalComboFill(ref DDQuality, "select QualityId,QualityName From Quality Where Item_Id=" + Request.QueryString["a"] + " order by QualityName", true, "--Plz Select--");
    //        }
    //    }
    //}

    //    protected void Fillselectprocess()
    //    {
    //        string sqlQuery = @"Select PNM.process_Name_id,PNM.Process_Name,IP.ProcessType,IP.SeqNo,IP.Itemid,IP.DESIGNID,IP.QualityId from 
    //Process_name_Master PNM Inner Join Item_Process IP on PNM.PROCESS_NAME_ID=IP.processId 
    //Where IP.MasterCompanyId=@MasterId and IP.Itemid=@Itemid and IP.DESIGNID=IsNUll(@DESIGNID,IP.DESIGNID) and IP.QualityId= IsNUll(@QualityId,IP.QualityId)
    //order by IP.SeqNo";

    //        string str = @"select PNM.process_Name_id,PNM.Process_Name from Process_name_Master PNM,Item_Process IP
    //                      Where PNM.Process_name_id=IP.ProcessId And ItemId=" + Request.QueryString["a"] + " And PNM.MasterCompanyid=" + Session["varcompanyid"] + "";

    //        if (DDQuality.SelectedIndex > 0)
    //        {
    //            str = str + " and IP.QualityId=" + DDQuality.SelectedValue;
    //        }

    //        if (DDDesign.SelectedIndex > 0)
    //        {
    //            str = str + " and IP.DesignId=" + DDDesign.SelectedValue;
    //        }
    //        else
    //        {
    //            str = str + " and IP.DesignId=0";
    //        }
    //        str = str + "  order by IP.SeqNo";
    //        UtilityModule.ConditonalListFill(ref lstSelectProcess, str);
    //    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        var lst = this.ProcessList;
        foreach (ListItem liItems in lstProcess.Items)
        {
            if (liItems.Selected)
            {
                var item = new ItemProcessModel()
                {
                    ProcessId = liItems.Value.ToInt(),
                    ProcessName = liItems.Text,
                    ProcessType = (short)rdbtnLst.SelectedItem.Value.ToInt()
                };
                string ProcessName = liItems.Text + "(" + rdbtnLst.SelectedItem.Text + ")";
                //item.Attributes.Add("data-processType", rdbtnLst.SelectedValue);
                //Check if process Already Exists
                if (!lst.Contains(item))
                {
                    lst.Add(item);
                }
            }
        }
        ViewState[itemProcessList] = lst;
        BindItemProcessList(this.ProcessList);
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        foreach (ListItem liItems in lstSelectProcess.Items)
        {
            if (liItems.Selected)
            {
                lstSelectProcess.Items.Remove(liItems);
            }
        }
        BindItemProcessList(this.ProcessList);
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

            foreach (ListItem items in lstSelectProcess.Items)
            {
                seqNo += 1;
                str = items.Value + "," + seqNo + "," + items.Attributes["data-processType"];
                if (strnew == "")
                {
                    strnew = str;
                }
                else
                {
                    strnew = strnew + "|" + str;

                }
            }





            //array[3].Value = strnew;
            //array[4].Direction = ParameterDirection.Output;
            //array[5].Value = TDquality.Visible == false ? "0" : DDQuality.SelectedValue;
            //array[6].Value = TDDesign.Visible == false ? "0" : (DDDesign.SelectedIndex > 0 ? DDDesign.SelectedValue : "0");


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
    //protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (TDDesign.Visible == true)
    //    {
    //     /*   FillDesign();*/
    //    }

    //    Fillselectprocess();
    //}

    //private void FillDesign()
    //{
    //    string str = "select Distinct designId,designName From V_FinisheditemDetail  Where QualityId=" + DDQuality.SelectedValue + " order by designName";
    //    UtilityModule.ConditionalComboFill(ref DDDesign, str, true, "--Plz Select--");
    //}

    //protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Fillselectprocess();
    //}


}