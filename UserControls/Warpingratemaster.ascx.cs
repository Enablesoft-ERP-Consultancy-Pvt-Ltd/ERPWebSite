using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


public partial class UserControls_Warpingratemaster : System.Web.UI.UserControl
{
    static int CategorySeperateId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            if(Session["varMasterCompanyIDForERP"].ToString()=="21")
            {
                CategorySeperateId = 1;
                TDJobName.Visible = true;
                TDEmpName.Visible = true;
                string str2 = "";
                str2 = "select PROCESS_NAME_ID,PROCESS_NAME From Process_Name_Master WHere Process_name in('WARPING COTTON','WARPING WOOL')  ";

                UtilityModule.ConditionalComboFill(ref DDjobname, str2, true, "--Plz Select--");
                if (DDjobname.Items.Count > 0)
                {
                    DDjobname.SelectedIndex = 1;
                    JobNameSelectedIndexChanged();
                }
            }
            else
            {
                CategorySeperateId = 0;
            }

            string str = @"select ICM.CATEGORY_ID,ICm.CATEGORY_NAME From Item_category_master ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=cs.Categoryid Where cs.id="+CategorySeperateId+@" order by ICm.CATEGORY_NAME
                          select Val,Type from Sizetype";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcategoryName, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 1, false, "--Plz Select--");
            ViewState["id"] = "0";
            txteffectivedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDcategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
        Fillgrid();
    }
    private void ddlcategorycange()
    {
        DDquality.Items.Clear();
        DDdesign.Items.Clear();
        DDcolor.Items.Clear();
        DDshape.Items.Clear();
        DDsize.Items.Clear();
        DDshadename.Items.Clear();
        Tdquality.Visible = false;
        Tddesign.Visible = false;
        Tdcolor.Visible = false;
        Tdshadename.Visible = false;
        Tdshape.Visible = false;
        TdSize.Visible = false;
        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME 
                      FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on 
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDcategoryName.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        Tdquality.Visible = true;
                        break;
                    case "2":
                        Tddesign.Visible = true;
                        break;
                    case "3":
                        Tdcolor.Visible = true;
                        break;
                    case "6":
                        Tdshadename.Visible = true;
                        break;
                    case "4":
                        Tdshape.Visible = true;
                        break;
                    case "5":
                        Tdshape.Visible = true;
                        TdSize.Visible = true;
                        break;
                    case "10":
                        Tdcolor.Visible = true;
                        break;
                }
            }
        }

        string stritem = "select distinct IM.Item_Id,IM.Item_Name from  Item_Parameter_Master IPM  inner Join Item_Master IM on IM.Item_Id=IPM.Item_Id inner join Item_Category_Master ICM on ICM.Category_Id=IM.Category_Id where  IM.Category_Id=" + DDcategoryName.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by IM.item_name";
        UtilityModule.ConditionalComboFill(ref DDitemname, stritem, true, "---Select Item----");
    }
    private void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Shade, int Itemid)
    {

        string Str = @"SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        if (Itemid > 0)
        {
            Str = Str + " and ITEM_ID=" + Itemid + "";
        }
        Str = Str + " Order By QUALITYNAME";
        Str = Str + @" SELECT DESIGNID,DESIGNNAME from DESIGN Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By DESIGNNAME
                     SELECT COLORID,COLORNAME FROM COLOR Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By COLORNAME
                     SELECT SHAPEID,SHAPENAME FROM SHAPE Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By SHAPENAME
                     SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By SHADECOLORNAME";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref Quality, ds, 0, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Design, ds, 1, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Color, ds, 2, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Shape, ds, 3, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Shade, ds, 4, true, "--SELECT--");
    }
    protected void DDitemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(DDquality, DDdesign, DDcolor, DDshape, DDshadename, Convert.ToInt32(DDitemname.SelectedValue));
        Fillgrid();
    }
    protected void FillSize()
    {
        string size = "";
        string str = "";

        switch (DDsizetype.SelectedValue)
        {
            case "1":
                size = "Sizemtr";
                break;
            case "0":
                size = "Sizeft";
                break;
            case "2":
                size = "Sizeinch";
                break;
            default:
                size = "Sizeft";
                break;
        }
        //size Query

        str = "Select Distinct S.Sizeid,S." + size + " As  " + size + @" From Size S 
                 Where shapeid=" + DDshape.SelectedValue + " And S.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by " + size + "";

        UtilityModule.ConditionalComboFill(ref DDsize, str, true, "--SELECT--");
        //

    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void DDshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
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
            SqlParameter[] param = new SqlParameter[20];
            param[0] = new SqlParameter("@ID", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = ViewState["id"];
            param[1] = new SqlParameter("@categoryid", DDcategoryName.SelectedValue);
            param[2] = new SqlParameter("@Itemid", DDitemname.SelectedValue);
            param[3] = new SqlParameter("@qualityid", Tdquality.Visible == false ? "0" : DDquality.SelectedValue);
            param[4] = new SqlParameter("@Designid", Tddesign.Visible == false ? "0" : DDdesign.SelectedValue);
            param[5] = new SqlParameter("@colorid", Tdcolor.Visible == false ? "0" : DDcolor.SelectedValue);
            param[6] = new SqlParameter("@shadeid", Tdshadename.Visible == false ? "0" : DDshadename.SelectedValue);
            param[7] = new SqlParameter("@shapeid", Tdshape.Visible == false ? "0" : DDshape.SelectedValue);
            param[8] = new SqlParameter("@sizeid", TdSize.Visible == false ? "0" : DDsize.SelectedValue);
            param[9] = new SqlParameter("@effectivedate", txteffectivedate.Text);
            param[10] = new SqlParameter("@userid", Session["varuserid"]);
            param[11] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[11].Direction = ParameterDirection.Output;
            param[12] = new SqlParameter("@Rate", txtRate.Text == "" ? "0" : txtRate.Text);
            param[13] = new SqlParameter("@Sizeflag", TdSize.Visible == true ? DDsizetype.SelectedValue : "0");
            param[14] = new SqlParameter("@ProcessId", TDJobName.Visible == true ? DDjobname.SelectedValue : "0");
            param[15] = new SqlParameter("@EmpId", TDEmpName.Visible == true ? DDEmpName.SelectedValue : "0");

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEWARPINGRATEMASTER", param);
            Tran.Commit();
            if (param[11].Value.ToString() != "")
            {
                lblmsg.Text = param[11].Value.ToString();
            }
            else
            {
                lblmsg.Text = "Data saved successfully.";
                txtRate.Text = "";
                txteffectivedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
                ViewState["id"] = "0";
            }
            Fillgrid();

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
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
                if (Session["varMasterCompanyIDForERP"].ToString() == "21")
                {
                    if (DgDetail.Columns[i].HeaderText == "Job_Name" || DgDetail.Columns[i].HeaderText == "Emp_Name")
                    {
                        DgDetail.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (DgDetail.Columns[i].HeaderText == "Job_Name" || DgDetail.Columns[i].HeaderText == "Emp_Name")
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
        Label lbldesignid = (Label)row.FindControl("lbldesignid");
        Label lblcolorid = (Label)row.FindControl("lblcolorid");
        Label lblshapeid = (Label)row.FindControl("lblshapeid");
        Label lblsizeid = (Label)row.FindControl("lblsizeid");
        Label lblshadecolorid = (Label)row.FindControl("lblshadecolorid");
        Label lblid = (Label)row.FindControl("lblid");
        Label lblsizeflag = (Label)row.FindControl("lblsizeflag");
        Label lblrate = (Label)row.FindControl("lblrate");
        Label lbleffectivefrom = (Label)row.FindControl("lbleffectivefrom");
        Label lblprocessid = (Label)row.FindControl("lblprocessid");
        Label lblEmpName = (Label)row.FindControl("lblEmpName");
        Label lblEmpId = (Label)row.FindControl("lblEmpId");

        DDcategoryName.SelectedValue = lblcategoryid.Text;
        DDcategoryName_SelectedIndexChanged(sender, new EventArgs());

        if (DDitemname.Items.FindByValue(lblitemid.Text) != null)
        {
            DDitemname.SelectedValue = lblitemid.Text;
            DDitemname_SelectedIndexChanged(sender, new EventArgs());
        }
        if (Tdquality.Visible == true)
        {
            if (DDquality.Items.FindByValue(lblqualityid.Text) != null)
            {
                DDquality.SelectedValue = lblqualityid.Text;

            }
        }
        if (Tddesign.Visible == true)
        {
            if (DDdesign.Items.FindByValue(lbldesignid.Text) != null)
            {
                DDdesign.SelectedValue = lbldesignid.Text;

            }
        }
        if (Tdcolor.Visible == true)
        {
            if (DDcolor.Items.FindByValue(lblcolorid.Text) != null)
            {
                DDcolor.SelectedValue = lblcolorid.Text;

            }
        }
        if (Tdshape.Visible == true)
        {
            if (DDshape.Items.FindByValue(lblshapeid.Text) != null)
            {
                DDshape.SelectedValue = lblshapeid.Text;

            }
        }
        if (TdSize.Visible == true)
        {
            if (DDsizetype.Items.FindByValue(lblsizeflag.Text) != null)
            {
                DDsizetype.SelectedValue = lblsizeflag.Text;
                DDsizetype_SelectedIndexChanged(sender, new EventArgs());
            }
            if (DDsize.Items.FindByValue(lblsizeid.Text) != null)
            {
                DDsize.SelectedValue = lblsizeid.Text;
            }
        }
        if (Tdshadename.Visible == true)
        {

            if (DDshadename.Items.FindByValue(lblshadecolorid.Text) != null)
            {
                DDshadename.SelectedValue = lblshadecolorid.Text;
            }
        }

        if (TDJobName.Visible == true)
        {
            if (DDjobname.Items.FindByValue(lblprocessid.Text) != null)
            {
                DDjobname.SelectedValue = lblprocessid.Text;
                DDjobname_SelectedIndexChanged(sender, e);
            }
        }
        if (TDEmpName.Visible == true)
        {
            if (DDEmpName.Items.FindByValue(lblEmpId.Text) != null)
            {
                DDEmpName.SelectedValue = lblEmpId.Text;
            }
        }
       
        
        txtRate.Text = lblrate.Text;
        txteffectivedate.Text = lbleffectivefrom.Text;
        ViewState["id"] = lblid.Text;
    }
    protected void Fillgrid()
    {
        try
        {
            string str = @"SELECT WR.ID,IM.ITEM_NAME,Q.QUALITYNAME,D.DESIGNNAME,C.COLORNAME,SC.SHADECOLORNAME,SH.SHAPENAME,
                        CASE WHEN WR.SIZEFLAG=1 THEN S.SIZEMTR WHEN WR.SIZEFLAG=2 THEN S.SIZEINCH ELSE S.SIZEFT END AS SIZE,
                        WR.RATE,REPLACE(CONVERT(NVARCHAR(11),WR.EFFECTIVEDATE,106),' ','-') AS EFFECTIVEDATE,REPLACE(CONVERT(NVARCHAR(11),WR.TODATE,106),' ','-') AS TODATE,
                        WR.ITEMID,WR.QUALITYID,WR.DESIGNID,WR.COLORID,WR.CATEGORYID,WR.SHADEID,WR.SHAPEID,WR.SIZEID,WR.SIZEFLAG,ISNULL(WR.ProcessId,0) as ProcessId,
                        ISNULL(PNM.PROCESS_NAME,'') as ProcessName,isnull(WR.EmpId,0) as EmpId, ISNULL(EI.EmpName,'') as EmpName
                        FROM WARPINGRATEMASTER WR LEFT JOIN ITEM_MASTER IM  ON WR.ITEMID=IM.ITEM_ID
                        LEFT JOIN QUALITY Q ON WR.QUALITYID=Q.QUALITYID
                        LEFT JOIN DESIGN D ON WR.DESIGNID=D.DESIGNID
                        LEFT JOIN COLOR C ON WR.COLORID=C.COLORID
                        LEFT JOIN SHAPE SH ON WR.SHAPEID=SH.SHAPEID
                        LEFT JOIN SIZE S ON WR.SIZEID=S.SIZEID
                        LEFT JOIN SHADECOLOR SC ON WR.SHADEID=SC.SHADECOLORID
                        Left join EmpInfo EI on WR.EmpId=EI.EmpId
                        LEFT join PROCESS_NAME_MASTER PNM on WR.Processid=PNM.PROCESS_NAME_ID
                        WHERE 1=1";
            if (DDcategoryName.SelectedIndex > 0)
            {
                str = str + "  and wr.categoryid=" + DDcategoryName.SelectedValue;
            }
            if (DDitemname.SelectedIndex > 0)
            {
                str = str + "  and wr.itemid=" + DDitemname.SelectedValue;
            }
            if (DDquality.SelectedIndex > 0)
            {
                str = str + "  and wr.qualityid=" + DDquality.SelectedValue;
            }
            if (DDdesign.SelectedIndex > 0)
            {
                str = str + "  and wr.designid=" + DDdesign.SelectedValue;
            }
            if (DDcolor.SelectedIndex > 0)
            {
                str = str + "  and wr.colorid=" + DDcolor.SelectedValue;
            }
            if (DDshape.SelectedIndex > 0)
            {
                str = str + "  and wr.shapeid=" + DDshape.SelectedValue;
            }
            if (DDsize.SelectedIndex > 0)
            {
                str = str + "  and wr.sizeid=" + DDsize.SelectedValue;
            }
            if (DDshadename.SelectedIndex > 0)
            {
                str = str + "  and wr.shadeid=" + DDshadename.SelectedValue;
            }

            if (DDjobname.SelectedIndex > 0)
            {
                str = str + "  and wr.ProcessId=" + DDjobname.SelectedValue;
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                str = str + "  and wr.EmpId=" + DDEmpName.SelectedValue;
            }

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            DgDetail.DataSource = ds.Tables[0];
            DgDetail.DataBind();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
    protected void btnnew_Click(object sender, EventArgs e)
    {
        DDcategoryName.SelectedIndex = -1;
        DDitemname.SelectedIndex = -1;
        DDquality.SelectedIndex = -1;
        DDdesign.SelectedIndex = -1;
        DDcolor.SelectedIndex = -1;
        DDshadename.SelectedIndex = -1;
        DDshape.SelectedIndex = -1;
        DDsize.SelectedIndex = -1;
        txtRate.Text = "";
        txteffectivedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        ViewState["id"] = "0";
        //DDEmpName.SelectedIndex = -1;

    }
    protected void DDquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillgrid();
    }
    protected void DDdesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillgrid();
    }
    protected void DDcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillgrid();
    }
    protected void DDshadename_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillgrid();
    }
    protected void DDsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillgrid();
    }
    protected void DDjobname_SelectedIndexChanged(object sender, EventArgs e)
    {
        JobNameSelectedIndexChanged();
    }
    protected void JobNameSelectedIndexChanged()
    {
        string str = "";
        if (Session["varMasterCompanyIDForERP"].ToString()=="21")
        {
            str = str + @"select EI.EmpId,EI.EmpName + CASE WHEN EI.EMPCODE<>'' THEN ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME from empinfo EI inner join Department D 
                           on EI.departmentId=D.DepartmentId Where D.DepartmentName in('WARPING')
                           and isnull(Ei.Blacklist,0)=0 order by EmpName ";
        }
        else
        {
            str = str + @"select EI.EmpId,EI.EmpName from empinfo EI inner join Department D 
                           on EI.departmentId=D.DepartmentId Where D.DepartmentName='WARPING' and EI.EmpName in('WARPING SECTION')
                           and isnull(Ei.Blacklist,0)=0 order by EmpName ";
        }

        UtilityModule.ConditionalComboFill(ref DDEmpName, str, true, "--Plz Select--");

        Fillgrid();
    }
}