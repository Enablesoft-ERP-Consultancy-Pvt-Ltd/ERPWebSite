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
public partial class Masters_RawMaterial_RawMaterial_report : System.Web.UI.Page
{
    static int MasterCompanyId;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varCompanyId"]);
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            TxtFRDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtTODate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtProdCode.Focus();
            if (Request.QueryString["Type"] == "2")
            {
                frdate.Visible = true;
                todate.Visible = true;
                trdd.Visible = true;
                trbtn.Visible = true;
                tritem.Visible = false;
                TdGodown.Visible = false;
            }
            else if (Request.QueryString["Type"] == "1")
            {
                tritem.Visible = true;
                trdd.Visible = true;
                TdFINISHED_TYPE.Visible = false;
                frdate.Visible = true;
                todate.Visible = true;
                code.Visible = false;
                trbtn.Visible = true;
                TdGodown.Visible = true;
                fillchecklist();
            }
            else
            {
                trdd.Visible = true;
                trbtn.Visible = true;
                tritem.Visible = false;
            }
        }
    }
    private void fillchecklist()
    {
        string str2 = "";
        if (txtitem.Text != "")
            str2 = "select ITEM_FINISHED_ID,ProductCode from V_FinishedItemDetail where productcode like '" + txtitem.Text + "%'";
        else
            str2 = "select ITEM_FINISHED_ID,ProductCode from V_FinishedItemDetail";
        UtilityModule.ConditonalChkBoxListFill(ref CHkitem, str2);
        str2 = "select Distinct GodownID,GodownName from godownmaster Order by GodownName";
        UtilityModule.ConditionalComboFill(ref DDGodown, str2, true, "--All--");
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER IPM inner join item_Master IM on IM.Item_Id=IPM.Item_Id inner join CategorySeparate CS on CS.CategoryId=IM.Category_Id  where ProductCode Like  '" + prefixText + "%' And IPM.MasterCompanyId=" + MasterCompanyId;
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
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        if (TxtProdCode.Text != "")
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select item_finished_id from ITEM_PARAMETER_MASTER where productcode='" + TxtProdCode.Text + "' And MasterCompanyId=" + Session["varCompanyId"] + "");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["item_finished_id"] = ds.Tables[0].Rows[0][0].ToString();
                string str = "select distinct id,finished_type_name from stock s,FINISHED_TYPE ft where s.finished_type_id=ft.id and s.item_finished_id =" + ds.Tables[0].Rows[0][0].ToString() + " ";
                UtilityModule.ConditionalComboFill(ref ddFINISHED_TYPE, str, true, "-Select-");
            }
        }
    }
    protected void btnpriview_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["Type"] == "1")
        {
            Session["ReportPath"] = "Reports/Rptstock_DetailNEW.rpt";
        }
        else if (Request.QueryString["Type"] == "2")
        {
            DateTime dt;
            string ss;
            ss = Convert.ToDateTime(TxtTODate.Text).ToString("dd-MM-yyyy");
            dt = DateTime.ParseExact(ss.ToString(), "dd-mm-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete from Temp_stock");
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"insert into Temp_stock
            SELECT SUM(IQTY)*SUM(QTYREQUIRED) as qty,OM.ORDERID,ORDERDATE,LOCALORDER,IFINISHEDID,i_finished_type_id,[dbo].[GET_Stock] (IFINISHEDID,i_finished_type_id,'" + TxtTODate.Text + "') as stock ,vf.category_name+vf.item_name as item,qualityname,ft.finished_type_name,'" + TxtFRDate.Text + "' AS FRDATE,'" + TxtTODate.Text + "' AS todate FROM ORDER_CONSUMPTION_DETAIL OCD, ORDERMASTER OM,ORDERDETAIL OD,V_FinishedItemDetail vf ,FINISHED_TYPE ft WHERE ft.id=ocd.i_finished_type_id and vf.item_finished_id=ocd.IFINISHEDID and OCD.ORDERID=OM.ORDERID AND OM.ORDERID=OD.ORDERID AND OCD.ORDERDETAILID=OD.ORDERDETAILID and IFINISHEDID=" + Session["item_finished_id"] + " and i_finished_type_id=" + ddFINISHED_TYPE.SelectedValue + "  GROUP BY OM.ORDERID,ORDERDATE,LOCALORDER,IFINISHEDID,i_finished_type_id,vf.category_name,vf.item_name ,qualityname,finished_type_name ");
            Session["ReportPath"] = "Reports/Rptstockorder1NEW.rpt";
            Session["CommanFormula"] = "";
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "priview();", true);
        }
        Report();
    }
    private void Report()
    {
        string qry = "";
        DataSet ds = new DataSet();
        if (Convert.ToString(Session["ReportPath"]) == "Reports/Rptstock_DetailNEW.rpt")
        {
            string str = "";
            foreach (ListItem li in CHkitem.Items)
            {
                if (li.Selected == true)
                {
                    if (str == "")
                        str = li.Value;
                    else
                        str = str + "," + li.Value;
                }
            }
            if (str != "")
            {
                qry = @"  SELECT V_Stock_Detail.partyid,V_Stock_Detail.date,V_Stock_Detail.issqty,V_Stock_Detail.rqty,V_Stock_Detail.type,CompanyInfo.CompanyName,V_Stock_Detail.product_code,V_Stock_Detail.finish_name,'" + TxtFRDate.Text + "' AS FRDATE,'" + TxtTODate.Text + @"' AS todate,[dbo].[GET_Stock] (V_Stock_Detail.finishedid,V_Stock_Detail.finished_type,'" + TxtFRDate.Text + @"') as stock,V_Stock_Detail.finishedid,V_Stock_Detail.finished_type,' " + DDGodown.SelectedItem.Text.ToUpper() + @"'
            FROM   V_Stock_Detail INNER JOIN CompanyInfo ON V_Stock_Detail.companyid=CompanyInfo.CompanyId
            Where  V_Stock_Detail.finishedid in(" + str + ") ";
                if (TxtFRDate.Text != "" && TxtTODate.Text != "")
                {
                    qry = qry + " and date>='" + TxtFRDate.Text + "' and date<='" + TxtTODate.Text + "'";
                }

                if (DDGodown.SelectedIndex > 0)
                {
                    qry = qry + " AND V_Stock_Detail.GodownID= " + DDGodown.SelectedValue;
                }
                qry = qry + " order by date Asc,type desc";
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('Select Atleast one Item');", true);
                return;
            }
            Session["dsFileName"] = "~\\ReportSchema\\Rptstock_DetailNEW.xsd";
        }
        else
        {
            qry = @" SELECT qualityname,finished_type_name,stock,localorder,qty,'" + TxtFRDate.Text + "' AS FRDATE,'" + TxtTODate.Text + "' AS todate FROM   Temp_stock ";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            Session["dsFileName"] = "~\\ReportSchema\\Rptstockorder1NEW.xsd";
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Session["rptFileName"] = "~\\Reports\\Rptstock_DetailNEW.rpt";
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            // Session["dsFileName"] = "~\\ReportSchema\\Rptstock_DetailNEW.xsd";
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
    protected void txtitem_TextChanged(object sender, EventArgs e)
    {
        fillchecklist();
    }
}