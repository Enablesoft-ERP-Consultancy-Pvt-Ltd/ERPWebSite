using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_frmrawmaterialstocksearch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = string.Empty;
            if (Session["VarCompanyNo"].ToString() == "16")
            {

                str = @"select GM.GODOWNID,GM.GODOWNNAME from GODOWNMASTER GM(NoLock) JOIN  Godown_Authentication GA(NoLock) ON GM.GoDownID=GA.GodownID 
                             Where GM.MasterCompanyId=" + Session["varCompanyId"] + @" and GA.UserId=" + Session["VarUserId"] + @" ORDER BY GM.GODOWNNAME
                            select ShadecolorId,ShadeColorName From ShadeColor order by ShadeColorName
                            Select Distinct CI.CompanyId, CI.CompanyName 
                            From Companyinfo CI(nolock)
                            JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @"  
                            Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By CompanyName 
                            select distinct Q.QualityId,Q.QualityName from Quality Q JOIN ITEM_MASTER IM ON Q.Item_id=IM.Item_id JOIN ITEM_CATEGORY_MASTER IC ON IC.CATEGORY_ID=IM.CATEGORY_ID
                            JOIN CategorySeparate CS ON IC.CATEGORY_ID=CS.Categoryid 
                            where Cs.id=1 and IC.CATEGORY_NAME='RAW MATERIAL' or IC.CATEGORY_NAME= 'RAW MATTERIAL'";

            }
            else
            {
                str = @"select GoDownID,GodownName From Godownmaster order by GodownName
                            select ShadecolorId,ShadeColorName From ShadeColor order by ShadeColorName
                            Select Distinct CI.CompanyId, CI.CompanyName 
                            From Companyinfo CI(nolock)
                            JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @"  
                            Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By CompanyName 
                            select distinct Q.QualityId,Q.QualityName from Quality Q JOIN ITEM_MASTER IM ON Q.Item_id=IM.Item_id JOIN ITEM_CATEGORY_MASTER IC ON IC.CATEGORY_ID=IM.CATEGORY_ID
                            JOIN CategorySeparate CS ON IC.CATEGORY_ID=CS.Categoryid 
                            where Cs.id=1 and IC.CATEGORY_NAME='RAW MATERIAL' or IC.CATEGORY_NAME= 'RAW MATTERIAL'";

            }
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDgodown, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDshadeno, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDcompanyName, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDQuality, ds, 3, true, "--Plz Select--");
            if (DDcompanyName.Items.Count > 0)
            {
                DDcompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompanyName.Enabled = false;
            }
        }
    }
    protected void btnshowdetail_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            string sQry = "";
            sQry = sQry + "  and S.companyid=" + DDcompanyName.SelectedValue;
            if (txttagno.Text.Trim() != "")
            {
                sQry = sQry + " and S.TagNo='" + txttagno.Text.Trim() + "'";
            }
            if (txtBinNo.Text.Trim() != "")
            {
                sQry = sQry + " and S.BinNO='" + txtBinNo.Text.Trim() + "'";
            }
            if (DDgodown.SelectedIndex > 0)
            {
                sQry = sQry + "  and S.godownid=" + DDgodown.SelectedValue;
            }
            if (DDshadeno.SelectedIndex > 0)
            {
                sQry = sQry + " and v.ShadeColorName='" + DDshadeno.SelectedItem.Text + "'";
            }
            if (DDQuality.SelectedIndex > 0)
            {
                sQry = sQry + " and v.QualityName='" + DDQuality.SelectedItem.Text + "'";
            }
            if (Session["varCompanyId"].ToString() == "21")
            {
                if (txtlotno.Text.Trim() != "")
                {
                    sQry = sQry + " and (S.lotno='" + txtlotno.Text.Trim() + "' or Vp.vendorlotno='" + txtlotno.Text.Trim() + "')";
                }
                if (txtPurchaseReceiveBillNo.Text.Trim() != "")
                {
                    sQry = sQry + " and VP.BillNo1='" + txtPurchaseReceiveBillNo.Text.Trim() + "'";
                }
                if (txtDyeingReceiveBillNo.Text.Trim() != "")
                {
                    sQry = sQry + " and PPRM.BillNo='" + txtDyeingReceiveBillNo.Text.Trim() + "'";
                }
            }
            else
            {
                if (txtlotno.Text.Trim() != "")
                {
                    sQry = sQry + " and S.lotno='" + txtlotno.Text.Trim() + "'";
                }
            }

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GET_RAWMATERIAL_STOCK_DETAIL", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
            cmd.Parameters.AddWithValue("@Where", sQry);
            cmd.Parameters.AddWithValue("@ChkStockQty", chkstockqtyzero.Checked == true ? 1 : 0);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();


            #region

            //            string sQry = "";

            //            sQry = @"SELECT v.item_name as [ITEM NAME],v.qualityname as [QUALITY NAME],designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeMtr AS DESCRIPTION,
            //            s.LOTNO,S.TAGNO,isnull(s.BinNo,'') as BINNO ,g.GODOWNNAME,Round(Sum(s.qtyinhand),3) [STOCK QTY]
            //            From stock s 
            //            join V_FinishedItemDetail v on s.ITEM_FINISHED_ID=v.ITEM_FINISHED_ID
            //            join GodownMaster g on s.godownid=g.godownid 
            //            Where  V.MasterCompanyId=" + Session["varCompanyId"];

            //            if (Session["varCompanyId"].ToString() == "21")
            //            {
            //                sQry = @"SELECT v.item_name as [ITEM NAME],v.qualityname as [QUALITY NAME],designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeMtr AS DESCRIPTION,
            //                s.LOTNO,S.TAGNO,isnull(s.BinNo,'') as BINNO ,g.GODOWNNAME
            //                ,ROUND(SUM(CASE WHEN ST.TRANTYPE=1 THEN ST.QUANTITY ELSE 0 END)-SUM(CASE WHEN ST.TRANTYPE=0 THEN ST.QUANTITY ELSE 0 END),3) AS [STOCK QTY]
            //                ,vp.empname AS VENDORNAME,vp.VENDORLOTNO,vp.RATE,VP.BillNo1,PPRM.BillNo
            //                ,isnull(PPRM.RRRemark,'') as Remark       
            //                From stock s INNER JOIN STOCKTRAN ST ON S.STOCKID=ST.STOCKID
            //                join V_FinishedItemDetail v on s.ITEM_FINISHED_ID=v.ITEM_FINISHED_ID
            //                join GodownMaster g on s.godownid=g.godownid 
            //                LEFT JOIN V_PURCHASELOTVENDORDETAIL VP ON S.ITEM_FINISHED_ID=VP.FINISHEDID AND S.LOTNO=VP.LOTNO AND S.TAGNO=VP.TAGNO 
            //                LEFT JOIN PP_PROCESSRECTRAN PPRT ON PPRT.PRTID=ST.PRTID AND ST.TABLENAME='PP_PROCESSRECTRAN' 
            //		        LEFT JOIN PP_PROCESSRECMASTER PPRM ON PPRM.PRMID=PPRT.PRMID 
            //                Where  V.MasterCompanyId=" + Session["varCompanyId"];
            //            }
            //            if (txttagno.Text.Trim() != "")
            //            {
            //                sQry = sQry + " and S.TagNo='" + txttagno.Text.Trim() + "'";
            //            }
            //            if (txtBinNo.Text.Trim() != "")
            //            {
            //                sQry = sQry + " and S.BinNO='" + txtBinNo.Text.Trim() + "'";
            //            }
            //            if (DDgodown.SelectedIndex > 0)
            //            {
            //                sQry = sQry + "  and S.godownid=" + DDgodown.SelectedValue;
            //            }
            //            if (DDshadeno.SelectedIndex > 0)
            //            {
            //                sQry = sQry + " and v.ShadeColorName='" + DDshadeno.SelectedItem.Text + "'";
            //            }
            //            if (DDcompanyName.SelectedIndex>0)
            //            {
            //                sQry = sQry + "  and S.companyid=" + DDcompanyName.SelectedValue;
            //            }
            //            if (DDQuality.SelectedIndex > 0)
            //            {
            //                sQry = sQry + " and v.QualityName='" + DDQuality.SelectedItem.Text + "'";
            //            }
            //            if (Session["varCompanyId"].ToString() == "21")
            //            {
            //                if (txtlotno.Text.Trim() != "")
            //                {
            //                    sQry = sQry + " and (S.lotno='" + txtlotno.Text.Trim() + "' or Vp.vendorlotno='" + txtlotno.Text.Trim() + "')";
            //                }
            //                if (txtPurchaseReceiveBillNo.Text.Trim() != "")
            //                {
            //                    sQry = sQry + " and VP.BillNo1='" + txtPurchaseReceiveBillNo.Text.Trim() + "'";
            //                }
            //                if (txtDyeingReceiveBillNo.Text.Trim() != "")
            //                {
            //                    sQry = sQry + " and PPRM.BillNo='" + txtDyeingReceiveBillNo.Text.Trim() + "'";
            //                }
            //            }
            //            else
            //            {
            //                if (txtlotno.Text.Trim() != "")
            //                {
            //                    sQry = sQry + " and S.lotno='" + txtlotno.Text.Trim() + "'";
            //                }
            //            }

            //            if (Session["varCompanyId"].ToString() == "21")
            //            {
            //                sQry = sQry + @" group by g.godownname,s.lotno,v.category_name,v.item_name,v.qualityname,designName,ColorName,ShadeColorName,ShapeName,SizeMtr,S.TagNo,S.BinNo,
            //                vp.empname, vp.VENDORLOTNO, vp.RATE, VP.BillNo1, PPRM.BillNo,PPRM.RRRemark";
            //            }
            //            else
            //            {
            //                sQry = sQry + @" group by g.godownname,s.lotno,v.category_name,v.item_name,v.qualityname,designName,ColorName,ShadeColorName,ShapeName,SizeMtr,S.TagNo,S.BinNo";
            //            }
            //            if (chkstockqtyzero.Checked == false)
            //            {
            //                sQry = sQry + " having Round(Sum(S.QtyInHand),3)<>0";
            //            }
            //            else
            //            {
            //                sQry = sQry + " having Round(Sum(S.QtyInHand),3)=0";
            //            }

            // DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);
            #endregion

            DgDetail.DataSource = ds.Tables[0];
            DgDetail.DataBind();
            if (ds.Tables[0].Rows.Count == 0)
            {
                lblmsg.Text = "No records Found For this combination";
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;

        }
    }
    protected void lnkshowdetailedreport_Click(object sender, EventArgs e)
    {
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}
        //SqlTransaction Tran = con.BeginTransaction();
        //try
        //{
        DataSet DS = new DataSet();

        string sQry = "  Vf.mastercompanyId=" + Session["varcompanyId"] + "";
        sQry = sQry + " AND S.companyid =" + DDcompanyName.SelectedValue;

        if (txtlotno.Text != "")
        {
            sQry = sQry + " AND S.LotNo = '" + txtlotno.Text + "'";
        }
        if (txttagno.Text != "")
        {
            sQry = sQry + " AND S.TagNo = '" + txttagno.Text + "'";
        }
        if (txtBinNo.Text != "")
        {
            sQry = sQry + " AND S.BinNo = '" + txtBinNo.Text + "'";
        }
        if (DDgodown.SelectedIndex > 0)
        {
            sQry = sQry + " AND S.Godownid =" + DDgodown.SelectedValue;
        }

        if (DDshadeno.SelectedIndex > 0)
        {
            sQry = sQry + " and vf.ShadeColorName='" + DDshadeno.SelectedItem.Text + "'";
        }
        if (DDQuality.SelectedIndex > 0)
        {
            sQry = sQry + " and vf.QualityName='" + DDQuality.SelectedItem.Text + "'";
        }

        //SqlParameter[] array = new SqlParameter[4];
        //array[0] = new SqlParameter("@Where", SqlDbType.VarChar, 8000);
        //array[1] = new SqlParameter("@FROMSTOCKSEARCH", SqlDbType.Int);
        //array[2] = new SqlParameter("@PurchaseReceiveBillNo", SqlDbType.VarChar,50);
        //array[3] = new SqlParameter("@DyeingReceiveBillNo", SqlDbType.VarChar, 50);

        //array[0].Value = sQry;
        //array[1].Value = 1;
        //array[2].Value = txtPurchaseReceiveBillNo.Text;
        //array[3].Value = txtDyeingReceiveBillNo.Text;

        //DataSet DS = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetStockTranDetail", array);


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_GetStockTranDetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 30000;

        cmd.Parameters.AddWithValue("@Where", sQry);
        cmd.Parameters.AddWithValue("@FROMSTOCKSEARCH", 1);
        cmd.Parameters.AddWithValue("@PurchaseReceiveBillNo", txtPurchaseReceiveBillNo.Text);
        cmd.Parameters.AddWithValue("@DyeingReceiveBillNo", txtDyeingReceiveBillNo.Text);
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(DS);


        // DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);
        if (DS.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "reports/RptStockTranDetailLotWiseTagWise.rpt";

            Session["GetDataset"] = DS;
            Session["dsFileName"] = "~\\ReportSchema\\RptStockTranDetail.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
        //}
        //catch (Exception ex)
        //{
        //    lblmsg.Text = ex.ToString();
        //    // UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmProcessDetailIssueReceive.aspx");
        //    Tran.Rollback();
        //}
        //finally
        //{
        //    con.Close();
        //    con.Dispose();
        //}

    }

}