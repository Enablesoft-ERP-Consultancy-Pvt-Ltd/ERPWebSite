using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class UserControls_YarnOpeningRateMaster : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select ICM.CATEGORY_ID,ICm.CATEGORY_NAME From Item_category_master ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=cs.Categoryid Where cs.id=1 order by ICm.CATEGORY_NAME
                            Select PROCESS_NAME_ID,PROCESS_NAME From Process_Name_Master WHere Process_name in('YARN OPENING','WEFT DEPARTMENT')
                            Select ConeType, ConeType From ConeMaster Order By SrNo ";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcategoryName, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDjobname, ds, 1, true, "--Plz Select--");
            if (DDjobname.Items.Count > 0)
            {
                DDjobname.SelectedIndex = 1;
                JobNameSelectedIndexChanged();
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDConetype, ds, 2, false, "");

            ViewState["id"] = "0";
            txteffectivedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDcategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDitemname, "select IM.ITEM_ID,Im.ITEM_NAME From Item_master IM  Where IM.category_id=" + DDcategoryName.SelectedValue + " order by Item_name", true, "--Plz Select--");
    }
    protected void DDitemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDquality, "select QualityId,QualityName From Quality Q WHere Item_Id=" + DDitemname.SelectedValue + " order by QualityName", true, "--Plz Select--");
        Fillgrid();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[17];
            param[0] = new SqlParameter("@ID", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = ViewState["id"];
            param[1] = new SqlParameter("@Categoryid", DDcategoryName.SelectedValue);
            param[2] = new SqlParameter("@Itemid", DDitemname.SelectedValue);
            param[3] = new SqlParameter("@QualityId", DDquality.SelectedValue);
            param[4] = new SqlParameter("@Shadecolorid", DDShade.SelectedIndex > 0 ? DDShade.SelectedValue : "0");
            param[5] = new SqlParameter("@Processid", DDjobname.SelectedValue);
            param[6] = new SqlParameter("@EmpId", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");
            param[7] = new SqlParameter("@rate", txtRate.Text == "" ? "0" : txtRate.Text);
            param[8] = new SqlParameter("@Effectivedate", txteffectivedate.Text);
            param[9] = new SqlParameter("@userid", Session["varuserid"]);
            param[10] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[11] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[11].Direction = ParameterDirection.Output;
            param[12] = new SqlParameter("@ORate", TxtORate.Text == "" ? "0" : TxtORate.Text);
            param[13] = new SqlParameter("@conetype", DDConetype.SelectedItem.Text);
            param[14] = new SqlParameter("@PlyType", DDPly.SelectedItem.Text);
            param[15] = new SqlParameter("@TransportType", DDTransport.SelectedItem.Text);
            param[16] = new SqlParameter("@Remark", TxtRemark.Text);
            //**************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SaveYarnOpeningRateMaster", param);
            lblmsg.Text = param[11].Value.ToString();
            ViewState["id"] = param[0].Value.ToString();
            Tran.Commit();
            clearaftersave();
            Fillgrid();

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
    protected void clearaftersave()
    {
        ViewState["id"] = "0";
        txtRate.Text = "";
        TxtORate.Text = "";
        TxtRemark.Text = "";
        txteffectivedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
    }
    protected void Fillgrid()
    {
        string str = @"select Im.Item_name,Q.QualityName,PNM.PROCESS_NAME,YOR.Rate,Replace(convert(nvarchar(11),YOR.EffectiveDate,106),' ','-') as EffectiveDate,Replace(convert(nvarchar(11),YOR.Todate,106),' ','-') as ToDate,
                    YOR.categoryid,YOR.Itemid,YOR.QualityId,YOR.Processid,YOR.Id,YOR.shadecolorid,sc.shadecolorname,YOR.EmpId,EI.EmpName, 
                    YOR.ORate, YOR.CONETYPE, YOR.PlyType, YOR.TransportType, YOR.Remark 
                    From YarnOpeningRateMaster YOR 
                    inner join Item_master Im on YOR.Itemid=IM.ITEM_ID
                    inner join Quality Q on YOR.QualityId=Q.QualityId
                    Left join ShadeColor sc on YOR.shadecolorid=Sc.shadecolorid
                    Left join EmpInfo EI on YOR.EmpId=EI.EmpId
                    inner join PROCESS_NAME_MASTER PNM on YOR.Processid=PNM.PROCESS_NAME_ID 
                    Where 1 = 1";

        if (DDConetype.SelectedValue != "")
        {
            str = str + " And YOR.Conetype = '" + DDConetype.SelectedItem.Text + "'";
        }
        if (DDPly.SelectedValue != "")
        {
            str = str + " And YOR.PlyType = '" + DDPly.SelectedItem.Text + "'";
        }
        if (DDTransport.SelectedValue != "")
        {
            str = str + " And YOR.TransportType = '" + DDTransport.SelectedItem.Text + "'";
        }
        if (DDcategoryName.SelectedIndex > 0)
        {
            str = str + " ANd YOR.categoryid=" + DDcategoryName.SelectedValue;
        }
        if (DDitemname.SelectedIndex > 0)
        {
            str = str + " And YOR.itemid=" + DDitemname.SelectedValue;
        }
        if (DDquality.SelectedIndex > 0)
        {
            str = str + " And YOR.Qualityid=" + DDquality.SelectedValue;
        }
        if (DDShade.SelectedIndex > 0)
        {
            str = str + " And YOR.shadecolorid=" + DDShade.SelectedValue;
        }
        if (DDjobname.SelectedIndex > 0)
        {
            str = str + " ANd YOR.Processid=" + DDjobname.SelectedValue;
        }
        if (DDEmpName.SelectedIndex > 0)
        {
            str = str + " ANd YOR.EmpId=" + DDEmpName.SelectedValue;
        }
        
        str = str + "  order by YOR.Effectivedate desc,YOR.ToDate";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DgDetail.DataSource = ds.Tables[0];
        DgDetail.DataBind();
    }

    protected void DDquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDShade, @"select vf.ShadecolorId,vf.ShadeColorName from V_FinishedItemDetail VF inner join ShadeColor sc on vf.ShadecolorId=sc.ShadecolorId
                                                        Where Vf.ITEM_ID=" + DDitemname.SelectedValue + " and vf.QualityId=" + DDquality.SelectedValue + " order by vf.ShadeColorName", true, "--Plz Select--");
        Fillgrid();
    }
    protected void DgDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.DgDetail, "select$" + e.Row.RowIndex);


            //for (int i = 0; i < DgDetail.Columns.Count; i++)
            //{
            //    if (DgDetail.Columns[i].HeaderText.ToUpper() == "CONE TYPE" && DDjobname.SelectedItem.Text.ToUpper() == "YARN OPENING+MOTTELING")
            //    {
            //        DgDetail.Columns[i].Visible = true;
            //    }
            //    else
            //    {
            //        if (DgDetail.Columns[i].HeaderText.ToUpper() == "CONE TYPE")
            //        {
            //            DgDetail.Columns[i].Visible = false;
            //        }

            //    }
            //}

        }
    }
    protected void DgDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = DgDetail.SelectedRow;
        Label lblcategoryid = (Label)row.FindControl("lblcategoryid");
        Label lblitemid = (Label)row.FindControl("lblitemid");
        Label lblqualityid = (Label)row.FindControl("lblqualityid");
        Label lblprocessid = (Label)row.FindControl("lblprocessid");
        Label lblid = (Label)row.FindControl("lblid");
        Label lblrate = (Label)row.FindControl("lblrate");
        Label lblEmpId = (Label)row.FindControl("lblEmpId");
        Label lbleffectivefrom = (Label)row.FindControl("lbleffectivefrom");
        Label lblshadecolorid = (Label)row.FindControl("lblshadecolorid");
        Label lblEmpName = (Label)row.FindControl("lblEmpName");
        Label lblORate = (Label)row.FindControl("lblORate");
        Label lblConeType = (Label)row.FindControl("lblConeType");
        Label lblPlyType = (Label)row.FindControl("lblPlyType");
        Label lblTransportType = (Label)row.FindControl("lblTransportType");
        Label LblRemark = (Label)row.FindControl("LblRemark");

        DDcategoryName.SelectedValue = lblcategoryid.Text;
        DDcategoryName_SelectedIndexChanged(sender, new EventArgs());
        if (DDitemname.Items.FindByValue(lblitemid.Text) != null)
        {
            DDitemname.SelectedValue = lblitemid.Text;
            DDitemname_SelectedIndexChanged(sender, new EventArgs());
        }
        if (DDquality.Items.FindByValue(lblqualityid.Text) != null)
        {
            DDquality.SelectedValue = lblqualityid.Text;
            DDquality_SelectedIndexChanged(sender, e);
        }
        if (DDShade.Items.FindByValue(lblshadecolorid.Text) != null)
        {
            DDShade.SelectedValue = lblshadecolorid.Text;
        }
        if (DDjobname.Items.FindByValue(lblprocessid.Text) != null)
        {
            DDjobname.SelectedValue = lblprocessid.Text;
            DDjobname_SelectedIndexChanged(sender, e);
        }
        if (DDEmpName.Items.FindByValue(lblEmpId.Text) != null)
        {
            DDEmpName.SelectedValue = lblEmpId.Text;
        }
        if (DDConetype.Items.FindByText(lblConeType.Text) != null)
        {
            DDConetype.SelectedValue = lblConeType.Text;
        }
        if (DDPly.Items.FindByText(lblPlyType.Text) != null)
        {
            DDPly.SelectedValue = lblPlyType.Text;
        }
        if (DDTransport.Items.FindByText(lblTransportType.Text) != null)
        {
            DDTransport.SelectedValue = lblTransportType.Text;
        }
        txtRate.Text = lblrate.Text;
        TxtORate.Text = lblORate.Text;
        txteffectivedate.Text = lbleffectivefrom.Text;
        TxtRemark.Text = LblRemark.Text;
        ViewState["id"] = lblid.Text;
    }

    protected void btnnew_Click(object sender, EventArgs e)
    {
        DDcategoryName.SelectedIndex = -1;
        DDitemname.SelectedIndex = -1;
        DDquality.SelectedIndex = -1;
        DDEmpName.SelectedIndex = -1;
        txtRate.Text = "";
        TxtORate.Text = "";
        txteffectivedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        ViewState["id"] = "0";
    }
    protected void DDjobname_SelectedIndexChanged(object sender, EventArgs e)
    {
        JobNameSelectedIndexChanged();
    }
    protected void JobNameSelectedIndexChanged()
    {
        string str = "";
        if (variable.VarYARNOPENINGISSUEEMPWISE == "1")
        {
            str = str + @"     select EI.EmpId,EI.EmpName + CASE WHEN EI.EMPCODE<>'' THEN ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME from empinfo EI inner join Department D 
                           on EI.departmentId=D.DepartmentId Where D.DepartmentName in('Yarn Opening','WEFT DEPARTMENT')
                           and isnull(Ei.Blacklist,0)=0 order by EmpName  ";
        }
        else
        {
            str = str + @"     select EI.EmpId,EI.EmpName from empinfo EI inner join Department D 
                           on EI.departmentId=D.DepartmentId Where D.DepartmentName='Yarn Opening' and EI.EmpName in('Yarn Opening','YARN OPENING-2','YARN OPENING-3')
                           and isnull(Ei.Blacklist,0)=0 order by EmpName  ";
        }

        UtilityModule.ConditionalComboFill(ref DDEmpName, str, true, "--Plz Select--");

        Fillgrid();
    }
    protected void DDShade_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillgrid();
    }
    protected void DDConetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillgrid();
    }
    protected void DDPly_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillgrid();
    }
    protected void DDTransport_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillgrid();
    }
    protected void DDBranchName_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}