using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
public partial class Masters_RawMaterial_RawMaterial_report1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            trshow.Visible = true;
            trgrid.Visible = true;
            string str1 = @"select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER";
            DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str1);
            UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, ds1, 0, true, "-Select Category-");
        }
        
        //UtilityModule.ConditionalComboFillWithDS(ref dditemname, ds1, 1, true, "-Select Item-");
        //UtilityModule.ConditionalComboFillWithDS(ref dquality, ds1, 2, true, "-Select Quality-");
    }
    protected void btnshow_Click(object sender, EventArgs e)
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from sysobjects where name ='TempCombStock'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "DROP TABLE TempCombStock");
        }
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "CREATE TABLE [dbo].[TempCombStock]([Finished_id] [int] NULL,[Finish_type] [int] NULL,[Description] [nvarchar](100) NULL,[Productcode] [nvarchar](50) NULL,[Recqty] [Float]) ON [PRIMARY]");
        string str="";
        if (ddCatagory.SelectedIndex > 0)
        {
            str = "Where vf.CATEGORY_ID=" + ddCatagory.SelectedValue + "";
        }
        if (dditemname.SelectedIndex > 0)
        {
            str = str + " and vf.ITEM_ID=" + dditemname.SelectedValue + "";
        }
        if (dquality.SelectedIndex > 0)
        {
            str = str + " and vf.QualityId=" + dquality.SelectedValue + "";
        }
        string str1= @"Insert into TempCombStock select distinct vf.item_finished_id as finished_id ,ppt.finish_type as finish_type,vf.category_name+' '+vf.item_name+''+vf.designname+' '+vf.colorname+' '+vf.shadecolorname+' '+vf.shapename+' '+ft.finished_type_name as description,vf.qualityName as productcode,0 as recqty
        From V_FinishedItemDetail vf right outer join PP_ProcessRecTran ppt on vf.item_finished_id=ppt.finishedid inner join 
        FINISHED_TYPE ft on ft.id=ppt.finish_type "+str+@" union 
        Select distinct vf.item_finished_id as finished_id,ppt.finished_type_id as finish_type,vf.category_name+' '+vf.item_name+''+vf.designname+' '+vf.colorname+' '+vf.shadecolorname+' '+vf.shapename+' '+ft.finished_type_name as description,vf.qualityName as productcode,[dbo].[Get_BalRecStock] (item_finished_id,ppt.finished_type_id)
        From V_FinishedItemDetail vf Right outer join PurchaseReceiveDetail ppt on ppt.finishedid=vf.item_finished_id inner join 
        FINISHED_TYPE ft on ft.id=ppt.finished_type_id "+str+@"
        group by item_finished_id,finished_type_id,vf.category_name,vf.item_name,vf.designname,vf.colorname,vf.shadecolorname,vf.shapename,vf.qualityName,ft.finished_type_name ";
        
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,str1);
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from TempCombStock");
        int ready = 0;
        //DataSet ds6;
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select sum(recquantity) as rec,finishedid,finish_type from PP_ProcessRecMaster ppm,PP_ProcessRecTran ppt where ppm.prmid=ppt.prmid and processid=" + ds1.Tables[0].Rows[i][0].ToString() + " group by finishedid,finish_type ");

                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Alter table TempCombStock add " + ds1.Tables[0].Rows[i][1].ToString() + " Float ");
                        for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                        {
                            if (ds1.Tables[0].Rows[i][0].ToString() == "9" || ds1.Tables[0].Rows[i][0].ToString() == "6" || ds1.Tables[0].Rows[i][0].ToString() == "8")
                            {
                                
                                DataSet ds10 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Qtyinhand from stock where item_finished_id=" + ds2.Tables[0].Rows[j][1].ToString() + " and finished_type_id=" + ds2.Tables[0].Rows[j][2].ToString() + " ");
                                ready = Convert.ToInt32(ds10.Tables[0].Rows[0][0].ToString());
                            }
                            else
                            {
                                ready = Convert.ToInt32(ds2.Tables[0].Rows[j][0].ToString());
                            }
                            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "update TempCombStock set " + ds1.Tables[0].Rows[i][1].ToString() + "=" + ready + " where Finished_id=" + ds2.Tables[0].Rows[j]["finishedid"].ToString() + " and Finish_type=" + ds2.Tables[0].Rows[j]["finish_type"].ToString() + "");
                        }
                    }
                }
            }
            
            
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select *,0 as Total from TempCombStock   order by productcode");
            gdDesign.DataSource = ds;
            gdDesign.DataBind();
            if (gdDesign.Rows.Count > 0)
                gdDesign.Visible = true;
            else
                gdDesign.Visible = false;
        }
    }
    protected void gdDesign_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
        double tot = 0;
        int rr = e.Row.RowIndex;
        if (rr > 0)
        {
            int lcoun = gdDesign.Rows[0].Cells.Count;
            for (int i = 4; i < lcoun - 1; i++)
            {
                if (e.Row.Cells[i].Text == " " || e.Row.Cells[i].Text == "&nbsp;")
                    e.Row.Cells[i].Text = "0";
                tot = tot + Convert.ToDouble(e.Row.Cells[i].Text);
            }
            e.Row.Cells[lcoun - 1].Text = Convert.ToString(tot);
            tot = 0;
            if (gdDesign.Rows[0].Cells[lcoun - 1].Text == "0")
            {
                for (int j = 4; j < lcoun - 1; j++)
                {
                    if (gdDesign.Rows[0].Cells[j].Text == " " || gdDesign.Rows[0].Cells[j].Text == "&nbsp;")
                        gdDesign.Rows[0].Cells[j].Text = "0";
                    tot = tot + Convert.ToDouble(gdDesign.Rows[0].Cells[j].Text);
                }
                gdDesign.Rows[0].Cells[lcoun - 1].Text = Convert.ToString(tot);
            }
        }
    }
    
    protected void btnexp_Click(object sender, EventArgs e)
    {
        gdDesign.Style.Add("font-size", "1em");
        Response.Clear();
        string attachment = "attachment; filename=Combine Stock Detail.xls";
        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.ContentType = "application/ms-excel";
        Response.Charset = "UTF-8";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        gdDesign.GridLines = GridLines.Horizontal;
        gdDesign.RenderControl(htw);
        Response.Write("<TABLE><TR><td></td><TD>Combine Stock Detail</TD></TR><TR><td></td><td></td><TD></td></TR></TABLE>" + sw.ToString());
        Response.End();
    }
    protected void gdDesign_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dditemname, "select ITEM_ID,ITEM_NAME from ITEM_MASTER where CATEGORY_ID="+ddCatagory.SelectedValue+"", true, "-ALL-");
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dquality, "select QualityId,QualityName from quality Where Item_Id=" + dditemname.SelectedValue + "", true, "-ALL-");
    }
    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
        //confirms that an HtmlForm control is rendered for the
        //specified ASP.NET server control at run time.
    }
}