using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class UserControls_Motellingratemaster : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select ICM.CATEGORY_ID,ICm.CATEGORY_NAME From Item_category_master ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=cs.Categoryid Where cs.id=1 order by ICm.CATEGORY_NAME
                            select PROCESS_NAME_ID,PROCESS_NAME From Process_Name_Master WHere Process_name in('MOTTELING','HAND SPINNING','YARN OPENING+MOTTELING', 'HANK MAKING')
                            Select ConeType, ConeType From ConeMaster Order By SrNo ";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcategoryName, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDjobname, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDConetype, ds, 2, false, "");
            //if (DDjobname.Items.Count > 0)
            //{
            //    DDjobname.SelectedIndex = 1;
            //}
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
            param[4] = new SqlParameter("@Processid", DDjobname.SelectedValue);
            param[5] = new SqlParameter("@Irate", txtIRate.Text == "" ? "0" : txtIRate.Text);
            param[6] = new SqlParameter("@Orate", txtOrate.Text == "" ? "0" : txtOrate.Text);
            param[7] = new SqlParameter("@Effectivedate", txteffectivedate.Text);
            param[8] = new SqlParameter("@userid", Session["varuserid"]);
            param[9] = new SqlParameter("@Mastercompanyid", Session["Varcompanyid"]);
            param[10] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[10].Direction = ParameterDirection.Output;
            param[11] = new SqlParameter("@Shadecolorid", DDShade.SelectedIndex > 0 ? DDShade.SelectedValue : "0");
            param[12] = new SqlParameter("@conetype", TDconetype.Visible == true ? DDConetype.SelectedItem.Text : "");
            param[13] = new SqlParameter("@PlyType", TDPly.Visible == true ? DDPly.SelectedItem.Text : "");
            param[14] = new SqlParameter("@TransportType", TDTransport.Visible == true ? DDTransport.SelectedItem.Text : "");
            param[15] = new SqlParameter("@EmpID", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");
            param[16] = new SqlParameter("@Remark", TxtRemark.Text);

            //**************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEMOTTELINGRATE", param);
            lblmsg.Text = param[10].Value.ToString();
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
        txtIRate.Text = "";
        txtOrate.Text = "";
        TxtRemark.Text = "";
        txteffectivedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
    }
    protected void Fillgrid()
    {
        string str = @"select Im.Item_name,Q.QualityName,PNM.PROCESS_NAME,MR.IRate,MR.ORate,Replace(convert(nvarchar(11),MR.EffectiveDate,106),' ','-') as EffectiveDate,Replace(convert(nvarchar(11),MR.Todate,106),' ','-') as ToDate,
                    Mr.categoryid,MR.Itemid,MR.QualityId,MR.Processid,MR.Id,MR.shadecolorid,sc.shadecolorname,Mr.conetype, MR.PlyType, MR.TransportType, MR.EmpID, 
                    IsNull(EI.EmpName, '') EmpName, MR.Remark
                    From MottelingRatemaster MR 
                    inner join Item_master Im on MR.Itemid=IM.ITEM_ID
                    inner join Quality Q on MR.QualityId=Q.QualityId
                    Left join ShadeColor sc on Mr.shadecolorid=Sc.shadecolorid
                    inner join PROCESS_NAME_MASTER PNM on MR.Processid=PNM.PROCESS_NAME_ID 
                    Left JOIN EmpInfo EI ON EI.EmpID = MR.EmpID 
                    Where 1=1";

        if (TDPly.Visible == true)
        {
            if (DDPly.SelectedValue != "")
            {
                str = str + " And MR.PlyType = '" + DDPly.SelectedItem.Text + "'";
            }
        }
        if (TDTransport.Visible == true)
        {
            if (DDTransport.SelectedValue != "")
            {
                str = str + " And MR.TransportType = '" + DDTransport.SelectedItem.Text + "'";
            }
        }
        if (DDcategoryName.SelectedIndex > 0)
        {
            str = str + " ANd Mr.categoryid=" + DDcategoryName.SelectedValue;
        }
        if (DDitemname.SelectedIndex > 0)
        {
            str = str + " And Mr.itemid=" + DDitemname.SelectedValue;
        }
        if (DDquality.SelectedIndex > 0)
        {
            str = str + " And Mr.Qualityid=" + DDquality.SelectedValue;
        }
        if (DDShade.SelectedIndex > 0)
        {
            str = str + " And Mr.shadecolorid=" + DDShade.SelectedValue;
        }
        if (DDjobname.SelectedIndex > 0)
        {
            str = str + " ANd Mr.Processid=" + DDjobname.SelectedValue;
        }
        if (DDEmpName.SelectedIndex > 0)
        {
            str = str + " And MR.EmpID = " + DDEmpName.SelectedValue;
        }
        if (TDconetype.Visible == true)
        {
            if (DDConetype.SelectedValue != "")
            {
                str = str + " ANd Mr.Conetype='" + DDConetype.SelectedItem.Text + "'";
            }
        }
        str = str + "  order by MR.Effectivedate desc,MR.ToDate";
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


            for (int i = 0; i < DgDetail.Columns.Count; i++)
            {
                if (DgDetail.Columns[i].HeaderText.ToUpper() == "CONE TYPE" && DDjobname.SelectedItem.Text.ToUpper() == "YARN OPENING+MOTTELING")
                {
                    DgDetail.Columns[i].Visible = true;
                }
                else
                {
                    if (DgDetail.Columns[i].HeaderText.ToUpper() == "CONE TYPE")
                    {
                        DgDetail.Columns[i].Visible = false;
                    }
                }
            }

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
        Label lblirate = (Label)row.FindControl("lblirate");
        Label lblorate = (Label)row.FindControl("lblorate");
        Label lbleffectivefrom = (Label)row.FindControl("lbleffectivefrom");
        Label lblshadecolorid = (Label)row.FindControl("lblshadecolorid");
        Label lblconetype = (Label)row.FindControl("lblconetype");
        Label lblPlyType = (Label)row.FindControl("lblPlyType");
        Label lblTransportType = (Label)row.FindControl("lblTransportType");
        Label lblEmpid = (Label)row.FindControl("lblEmpid");
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
        }
        if (TDconetype.Visible == true)
        {
            if (DDConetype.Items.FindByText(lblconetype.Text) != null)
            {
                DDConetype.SelectedValue = lblconetype.Text;
            }
        }
        if (TDPly.Visible == true)
        {
            if (DDPly.Items.FindByText(lblPlyType.Text) != null)
            {
                DDPly.SelectedValue = lblPlyType.Text;
            }
        }
        if (TDTransport.Visible == true)
        {
            if (DDTransport.Items.FindByText(lblTransportType.Text) != null)
            {
                DDTransport.SelectedValue = lblTransportType.Text;
            }
        }

        if (DDEmpName.Items.FindByValue(lblEmpid.Text) != null)
        {
            DDEmpName.SelectedValue = lblEmpid.Text;
        }
        txtIRate.Text = lblirate.Text;
        txtOrate.Text = lblorate.Text;
        txteffectivedate.Text = lbleffectivefrom.Text;
        TxtRemark.Text = LblRemark.Text;
        ViewState["id"] = lblid.Text;
    }

    protected void btnnew_Click(object sender, EventArgs e)
    {
        DDcategoryName.SelectedIndex = -1;
        DDitemname.SelectedIndex = -1;
        DDquality.SelectedIndex = -1;
        txtIRate.Text = "";
        txtOrate.Text = "";
        txteffectivedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        ViewState["id"] = "0";
    }
    protected void DDjobname_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDConetype.SelectedValue = "";
        switch (DDjobname.SelectedItem.Text.ToUpper())
        {
            case "YARN OPENING+MOTTELING":
                TDconetype.Visible = true;
                break;
            default:
                TDconetype.Visible = false;
                break;
        }
        switch (DDjobname.SelectedItem.Text.ToUpper())
        {
            case "HAND SPINNING":
                TDPly.Visible = false;
                TDTransport.Visible = false;
                break;
            default:
                TDPly.Visible = true;
                TDTransport.Visible = true;                
                break;
        }
        JobNameSelectedIndexChanged();
    }
    protected void JobNameSelectedIndexChanged()
    {
        string str = @"Select EI.EmpID, EI.EmpName 
            From Empinfo EI(Nolock)
            JOIN EmpProcess EP(Nolock) ON EP.EmpID = EI.EmpID And EP.ProcessId = " + DDjobname.SelectedValue + @"
            Where EI.Blacklist = 0 Order By EI.EmpName ";
    
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
    protected void DDEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillgrid();
    }
}