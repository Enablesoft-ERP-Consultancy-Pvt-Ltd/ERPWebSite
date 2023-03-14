using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

public partial class Masters_ReportForms_FrmSampleReviewDestini : System.Web.UI.Page
{
    static int mastercompany;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref ddCompName, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + " order by CompanyName", true, "-Select Company-");
            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            UtilityModule.ConditionalComboFill(ref ddcustomername, "Select customerid,customercode from customerinfo order by customercode", true, "-Select Company-");
        }
        mastercompany = Convert.ToInt32(Session["varcompanyno"].ToString());
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        string str = @"Select IPM.item_finished_id,ProductCode,IM.CATEGORY_ID,ic.CATEGORY_NAME,ITEM_NAME,max(ApprovedAmount) as appamt,IsNull(MII.REMARKS,'') REMARKS,IsNull(MII.Photo,'') Photo 
        from ITEM_MASTER IM,CategorySeparate CS,Item_Approval_Detail id,ITEM_CATEGORY_MASTER ic,ITEM_PARAMETER_MASTER IPM Left Outer Join MAIN_ITEM_IMAGE MII ON IPM.ITEM_FINISHED_ID=MII.FINISHEDID 
        Where IM.Category_Id=CS.CategoryId And IPM.ITEM_ID=IM.ITEM_ID and ic.CATEGORY_ID=im.CATEGORY_ID and id.FinishedID=ipm.item_finished_id  And Id=0 and ProductCode='" + TxtProdCode.Text + @"'
        group by IPM.item_finished_id,ProductCode,IM.CATEGORY_ID,ITEM_NAME,MII.REMARKS,MII.Photo,ic.CATEGORY_NAME";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["finished_id"] = ds.Tables[0].Rows[0][0].ToString();
            ViewState["Photo"] = ds.Tables[0].Rows[0]["Photo"].ToString();
            txtprice.Text = ds.Tables[0].Rows[0]["appamt"].ToString();
            txtmaterial.Text = ds.Tables[0].Rows[0]["CATEGORY_NAME"].ToString() + "  " + ds.Tables[0].Rows[0]["ITEM_NAME"].ToString();
            txtdescription.Text = ds.Tables[0].Rows[0]["REMARKS"].ToString();
            string str1 = @"select FINISHEDID,max(NETWEIGHT) Netweight,(select * from [dbo].[F_Get_finishedtype](FINISHEDID)) as finish
            from PROCESSCONSUMPTIONMASTER pcm ,PROCESSCONSUMPTIONDETAIL pcd where  pcm.PCMID=pcd.PCMID  and FINISHEDID=" + ds.Tables[0].Rows[0]["item_finished_id"].ToString() + " group by FINISHEDID";
            DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str1);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                txtweigth.Text = ds1.Tables[0].Rows[0]["Netweight"].ToString();
                Txtfinish.Text = ds1.Tables[0].Rows[0]["finish"].ToString();
            }
            string str2 = @"select pcs,cast(length as varchar)+'x'+cast(width as varchar)+'x'+cast(height as varchar) from PACKINGCOST where finishedid=" + ds.Tables[0].Rows[0]["item_finished_id"].ToString() + @" and PackingType=1
            select pcs,cast(length as varchar)+'x'+cast(width as varchar)+'x'+cast(height as varchar) from PACKINGCOST where finishedid=" + ds.Tables[0].Rows[0]["item_finished_id"].ToString() + @" and PackingType=3";
            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                txtinnerqty.Text = ds2.Tables[0].Rows[0][0].ToString();
                txtinnersize.Text = ds2.Tables[0].Rows[0][1].ToString();
                if (ds2.Tables[1].Rows.Count > 0)
                {
                    txtmasterqty.Text = ds2.Tables[1].Rows[0][0].ToString();
                    txtmastersize.Text = ds2.Tables[1].Rows[0][1].ToString();
                }
            }
            fill_grid();
        }
    }
    public static string[] GetQuality1(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "SELECT ProductCode from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,ITEM_CATEGORY_MASTER ICM,CategorySeparate CS Where IPM.Item_Id=IM.Item_Id AND IM.Category_Id=ICM.Category_Id And ICM.Category_Id=CS.CategoryId And Id=0 And IM.MasterCompanyId=" + mastercompany + "  And ProductCode Like '" + prefixText + "%' order by ProductCode";
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
    protected void btnsybmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Sampleid"] = 0;
            SqlParameter[] _arrpara = new SqlParameter[30];
            _arrpara[0] = new SqlParameter("@Sampleid", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@Companyid", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@Customerid", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@Userid", SqlDbType.Int);
            _arrpara[4] = new SqlParameter("@MasterCompanyid", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@Finishedid", SqlDbType.Int);
            _arrpara[6] = new SqlParameter("@ProductCode", SqlDbType.NVarChar, 50);
            _arrpara[7] = new SqlParameter("@Price", SqlDbType.Float);
            _arrpara[8] = new SqlParameter("@Weight", SqlDbType.Float);
            _arrpara[9] = new SqlParameter("@ItemDescription", SqlDbType.NVarChar, 250);
            _arrpara[10] = new SqlParameter("@Material", SqlDbType.NVarChar, 100);
            _arrpara[11] = new SqlParameter("@finish", SqlDbType.NVarChar, 250);
            _arrpara[12] = new SqlParameter("@SampleTest", SqlDbType.NVarChar, 50);
            _arrpara[13] = new SqlParameter("@ItemApproved", SqlDbType.NVarChar, 50);
            _arrpara[14] = new SqlParameter("@Length", SqlDbType.NVarChar, 50);
            _arrpara[15] = new SqlParameter("@Width", SqlDbType.NVarChar, 50);
            _arrpara[16] = new SqlParameter("@Height", SqlDbType.NVarChar, 50);
            _arrpara[17] = new SqlParameter("@DIA", SqlDbType.NVarChar, 50);
            _arrpara[18] = new SqlParameter("@Bulb_Cfl", SqlDbType.NVarChar, 250);
            _arrpara[19] = new SqlParameter("@InnerCtnQty", SqlDbType.NVarChar, 50);
            _arrpara[20] = new SqlParameter("@InnerCtnSize", SqlDbType.NVarChar, 50);
            _arrpara[21] = new SqlParameter("@MasterCtnQty", SqlDbType.NVarChar, 50);
            _arrpara[22] = new SqlParameter("@MasterCtnSize", SqlDbType.NVarChar, 50);
            _arrpara[23] = new SqlParameter("@TestRequired", SqlDbType.NVarChar, 250);
            _arrpara[24] = new SqlParameter("@TransitTest", SqlDbType.NVarChar, 250);
            _arrpara[25] = new SqlParameter("@Lableling", SqlDbType.NVarChar, 250);
            _arrpara[26] = new SqlParameter("@Wiring", SqlDbType.NVarChar, 250);
            _arrpara[27] = new SqlParameter("@Comment", SqlDbType.NVarChar, 1000);
            _arrpara[28] = new SqlParameter("@Message", SqlDbType.NVarChar, 100);
            _arrpara[29] = new SqlParameter("@Photo", SqlDbType.NVarChar, 250);
            _arrpara[0].Direction = ParameterDirection.InputOutput;
            _arrpara[0].Value = ViewState["Sampleid"];
            _arrpara[1].Value = ddCompName.SelectedValue;
            _arrpara[2].Value = ddcustomername.SelectedValue;
            _arrpara[3].Value = Session["varuserid"].ToString();
            _arrpara[4].Value = Session["varcompanyno"].ToString();
            _arrpara[5].Value = ViewState["finished_id"];
            _arrpara[6].Value = TxtProdCode.Text;
            _arrpara[7].Value = txtprice.Text != "" ? txtprice.Text : "0";
            _arrpara[8].Value = txtweigth.Text != "" ? txtweigth.Text : "0";
            _arrpara[9].Value = txtdescription.Text;
            _arrpara[10].Value = txtmaterial.Text;
            _arrpara[11].Value = Txtfinish.Text;
            _arrpara[12].Value = DDSample.SelectedItem.Text;
            _arrpara[13].Value = ddlitemapp.SelectedItem.Text;
            _arrpara[14].Value = txtlength.Text;
            _arrpara[15].Value = txtwidth.Text;
            _arrpara[16].Value = txtheigth.Text;
            _arrpara[17].Value = txtdia.Text;
            _arrpara[18].Value = txtbulb.Text;
            _arrpara[19].Value = txtinnerqty.Text;
            _arrpara[20].Value = txtinnersize.Text;
            _arrpara[21].Value = txtmasterqty.Text;
            _arrpara[22].Value = txtmastersize.Text;
            _arrpara[23].Value = txttestreq.Text;
            _arrpara[24].Value = txttranreq.Text;
            _arrpara[25].Value = txtlabel.Text;
            _arrpara[26].Value = txtwiring.Text;
            _arrpara[27].Value = txtcomment.Text;
            _arrpara[28].Direction = ParameterDirection.Output;
            _arrpara[29].Value = ViewState["Photo"].ToString();
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_SampleReview", _arrpara);
            ViewState["Sampleid"] = _arrpara[0].Value;
            fill_grid();
            btnpreview.Visible = true;
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Data Save Successfully!');", true);
            refresh();
        }
        catch (Exception)
        {

        }
    }
    private void refresh()
    {
        txtlength.Text = "";
        txtwidth.Text = "";
        txtheigth.Text = "";
        txtdia.Text = "";
        txtbulb.Text = "";
        txtinnerqty.Text = "";
        txtinnersize.Text = "";
        txtmasterqty.Text = "";
        txtmastersize.Text = "";
        txttestreq.Text = "";
        txttranreq.Text = "";
        txtlabel.Text = "";
        txtwiring.Text = "";
        txtcomment.Text = "";
    }
    private void fill_grid()
    {
        DataSet ds4 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from SampleReview where customerid=" + ddcustomername.SelectedValue + " and productcode='" + TxtProdCode.Text + "' and companyid=" + ddCompName.SelectedValue + "");
        DGsampleprivew.DataSource = ds4;
        DGsampleprivew.DataBind();
    }
    protected void DGsampleprivew_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    protected void DGsampleprivew_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGsampleprivew, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGsampleprivew_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //con.Open();
        //SqlTransaction Tran = con.BeginTransaction();
        //try
        //{
        //    int VarPurchase_Rec_Detail_Id = Convert.ToInt32(DGPurchaseReceiveDetail.DataKeys[e.RowIndex].Value);
        //    SqlParameter[] _arrpara = new SqlParameter[3];
        //    _arrpara[0] = new SqlParameter("@PurchaseReceiveDetailId", SqlDbType.Int);
        //    _arrpara[1] = new SqlParameter("@VarRowCount", SqlDbType.Int);
        //    _arrpara[2] = new SqlParameter("@varMsgFlag", SqlDbType.NVarChar, 250);
        //    _arrpara[0].Value = VarPurchase_Rec_Detail_Id;
        //    _arrpara[1].Value = DGPurchaseReceiveDetail.Rows.Count;
        //    _arrpara[2].Direction = ParameterDirection.Output;
        //    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PurchaseReceiveDeleteRow", _arrpara);
        //    Tran.Commit();
        //    if (_arrpara[2].Value != "")
        //    {
        //        ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('" + _arrpara[2].Value + "');", true);
        //    }
        //    DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
        //    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'PurchaseReceiveDetail'," + VarPurchase_Rec_Detail_Id + ",getdate(),'Delete')");
        //    fill_grid();
        //    //Fill_Grid_Show();
        //    if (DGPurchaseReceiveDetail.Rows.Count == 0)
        //    {
        //        UtilityModule.ConditionalComboFill(ref ddlrecchalanno, "select distinct prm.PurchaseReceiveId,receiveno+' / '+BillNo from PurchaseReceiveMaster prm left outer join PurchaseReceiveDetail prd  on prd.purchasereceiveid=prm.purchasereceiveid where pindentissueid=" + DDChallanNo.SelectedValue + " And prm.MasterCompanyId=" + Session["varCompanyId"], true, "--SELECT--");
        //        UtilityModule.ConditionalComboFill(ref DDCategory, "select distinct ICM.Category_Id,ICM.Category_Name from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id inner join Item_Category_Master ICM on ICM.Category_Id=IM.Category_Id inner join UserRights_Category UC on(icm.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ") where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"], true, "--Select--");
        //    }
        //}
        //catch (Exception ex)
        //{
        //    UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchaseReceive.aspx");
        //    Tran.Rollback();
        //    Lblmessage.Visible = true;
        //    Lblmessage.Text = ex.Message;
        //}
        //finally
        //{
        //    con.Close();
        //    con.Dispose();
        //}
    }
    protected void DGsampleprivew_SelectedIndexChanged(object sender, EventArgs e)
    {
        int n = DGsampleprivew.SelectedIndex;
        ViewState["Sampleid"] = DGsampleprivew.SelectedDataKey.Value;
        txtprice.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lblPrice")).Text;
        txtweigth.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lblweigth")).Text;
        txtdescription.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lblItemDescription")).Text;
        txtmaterial.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lblMaterial")).Text;
        Txtfinish.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lblfinish")).Text;
        DDSample.SelectedItem.Text = ((Label)DGsampleprivew.Rows[n].FindControl("LblSampleTest")).Text;
        ddlitemapp.SelectedItem.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lblitemApp")).Text;
        txtlength.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lbllength")).Text;
        txtwidth.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lblWidth")).Text;
        txtheigth.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lblheight")).Text;
        txtdia.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lblDIA")).Text;
        txtbulb.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lblBulb_Cfl")).Text;
        txtinnerqty.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lblinnerctnqty")).Text;
        txtinnersize.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lblinnerctnsize")).Text;
        txtmasterqty.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lblmasterqty")).Text;
        txtmastersize.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lblmastersize")).Text;
        txttestreq.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lbltestreq")).Text;
        txttranreq.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lbltranreq")).Text;
        txtlabel.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lbllableing")).Text;
        txtwiring.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lblwiring")).Text;
        txtcomment.Text = ((Label)DGsampleprivew.Rows[n].FindControl("lblcomment")).Text;
        Session["Photo"] = ((Label)DGsampleprivew.Rows[n].FindControl("lblphoto")).Text;
        btnpreview.Visible = true;
    }

    private void report()
    {
        Session["ReportPath"] = "Reports/RptSampleReview.rpt";
        string qry = @"Select sr.*,CustomerName,c.CompanyName from SampleReview sr inner join customerinfo ci On  sr.customerid=ci.customerid 
        Inner join companyinfo c On c.CompanyId=sr.Companyid Where sr.sampleid=" + ViewState["Sampleid"] + " ";
        SqlDataAdapter sda = new SqlDataAdapter(qry, ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds5 = new DataSet();
        sda.Fill(ds5);
        ds5.Tables[0].Columns.Add("ImageThumbNail", typeof(System.Byte[]));
        foreach (DataRow dr in ds5.Tables[0].Rows)
        {
            if (Convert.ToString(dr["Photo"]) != "")
            {
                FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                if (TheFile.Exists)
                {
                    string img = dr["Photo"].ToString();
                    img = Server.MapPath(img);
                    Byte[] img_Byte = File.ReadAllBytes(img);
                    dr["ImageThumbNail"] = img_Byte;
                }
            }
        }
        Session["dsFileName"] = "~\\ReportSchema\\RptSampleReview.xsd";
        if (ds5.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds5;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    protected void btnpreview_Click1(object sender, EventArgs e)
    {
        report();
    }
}